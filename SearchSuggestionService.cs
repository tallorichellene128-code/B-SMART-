using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace BSMART
{
    internal static class SearchSuggestionService
    {
        private static readonly Dictionary<TextBox, SearchDropdown> Dropdowns = new Dictionary<TextBox, SearchDropdown>();
        private static readonly Dictionary<TextBox, string> Signatures = new Dictionary<TextBox, string>();
        private static readonly Dictionary<string, List<string>> SearchHistory = new Dictionary<string, List<string>>();

        public static void AttachToOpenForms()
        {
            foreach (Form form in Application.OpenForms)
                AttachToForm(form);
        }

        private static void AttachToForm(Form form)
        {
            List<DataGridView> grids = FindControls<DataGridView>(form).ToList();
            List<string> fallbackSuggestions = grids.Count == 0
                ? FindControls<Label>(form)
                    .Select(label => label.Text)
                    .Where(text => !string.IsNullOrWhiteSpace(text))
                    .Select(text => text.Trim())
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(text => text)
                    .Take(250)
                    .ToList()
                : new List<string>();

            foreach (TextBox searchBox in FindControls<TextBox>(form).Where(IsSearchTextBox))
            {
                ApplySuggestions(searchBox, grids, fallbackSuggestions);
            }
        }

        private static bool IsSearchTextBox(TextBox textBox)
        {
            if (textBox.Name.IndexOf("search", StringComparison.OrdinalIgnoreCase) >= 0)
                return true;

            string tag = Convert.ToString(textBox.Tag) ?? "";
            if (tag.IndexOf("search", StringComparison.OrdinalIgnoreCase) >= 0)
                return true;

            Control? parent = textBox.Parent;
            if (parent == null) return false;

            foreach (Label label in parent.Controls.OfType<Label>())
            {
                bool nearby = Math.Abs(label.Top - textBox.Top) <= 90 || Math.Abs(label.Bottom - textBox.Top) <= 90;
                if (nearby && label.Text.IndexOf("search", StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
            }

            return false;
        }

        private static void ApplySuggestions(TextBox searchBox, List<DataGridView> grids, List<string> fallbackSuggestions)
        {
            List<string> suggestions = grids.Count > 0
                ? grids
                    .SelectMany(GridValues)
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Select(s => s.Trim())
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(s => s)
                    .Take(350)
                    .ToList()
                : fallbackSuggestions;

            string signature = string.Join("|", suggestions);
            if (Signatures.TryGetValue(searchBox, out string existing) && existing == signature)
                return;

            searchBox.AutoCompleteMode = AutoCompleteMode.None;
            searchBox.AutoCompleteSource = AutoCompleteSource.None;
            searchBox.AutoCompleteCustomSource = null;

            SearchDropdown dropdown = GetOrCreateDropdown(searchBox);
            dropdown.SetSuggestions(suggestions);
            Signatures[searchBox] = signature;
        }

        private static SearchDropdown GetOrCreateDropdown(TextBox searchBox)
        {
            if (Dropdowns.TryGetValue(searchBox, out SearchDropdown existing) && !existing.IsDisposed)
                return existing;

            SearchDropdown dropdown = new SearchDropdown(searchBox);
            Dropdowns[searchBox] = dropdown;
            return dropdown;
        }

        private static IEnumerable<string> GridValues(DataGridView grid)
        {
            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.IsNewRow) continue;
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (!cell.Visible) continue;
                    string value = FormatSuggestionValue(cell.Value);
                    if (value.Length >= 2) yield return value;
                }
            }
        }

        private static string FormatSuggestionValue(object value)
        {
            if (value == null) return "";
            if (value is DateTime date)
                return date.ToString("dd/MM/yyyy");

            string text = Convert.ToString(value) ?? "";
            if (DateTime.TryParse(text, out DateTime parsed) && text.IndexOf("12:00:00", StringComparison.OrdinalIgnoreCase) >= 0)
                return parsed.ToString("dd/MM/yyyy");

            return text;
        }

        private static IEnumerable<T> FindControls<T>(Control root) where T : Control
        {
            foreach (Control child in root.Controls)
            {
                if (child is T match) yield return match;
                foreach (T nested in FindControls<T>(child))
                    yield return nested;
            }
        }

        private sealed class SearchDropdown : ListBox
        {
            private readonly TextBox searchBox;
            private readonly Form ownerForm;
            private readonly List<string> suggestions = new List<string>();
            private readonly string historyKey;
            private bool selecting;

            public SearchDropdown(TextBox searchBox)
            {
                this.searchBox = searchBox;
                ownerForm = searchBox.FindForm() ?? throw new InvalidOperationException("Search box must be on a form.");
                historyKey = ownerForm.GetType().FullName + "." + searchBox.Name;

                BorderStyle = BorderStyle.FixedSingle;
                IntegralHeight = false;
                DrawMode = DrawMode.OwnerDrawFixed;
                ItemHeight = 46;
                Font = new Font("Segoe UI", 12F, FontStyle.Regular);
                BackColor = Color.White;
                ForeColor = Color.FromArgb(32, 33, 36);
                Visible = false;
                TabStop = false;

                ownerForm.Controls.Add(this);
                BringToFront();

                searchBox.TextChanged += (s, e) =>
                {
                    if (!selecting) RefreshDropdownItems();
                };
                searchBox.GotFocus += (s, e) => RefreshDropdownItems();
                searchBox.LostFocus += (s, e) => searchBox.BeginInvoke(new Action(HideIfUnfocused));
                searchBox.KeyDown += SearchBox_KeyDown;
                searchBox.Leave += (s, e) => RememberCurrentSearch();
                searchBox.Disposed += (s, e) => Dispose();

                MouseDown += (s, e) =>
                {
                    int index = IndexFromPoint(e.Location);
                    if (index >= 0) SelectItem(index);
                };
                LostFocus += (s, e) => searchBox.BeginInvoke(new Action(HideIfUnfocused));
            }

            public void SetSuggestions(List<string> values)
            {
                suggestions.Clear();
                suggestions.AddRange(values);
                RefreshDropdownItems();
            }

            protected override void OnDrawItem(DrawItemEventArgs e)
            {
                if (e.Index < 0) return;

                bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
                using Brush background = new SolidBrush(selected ? Color.FromArgb(241, 243, 244) : BackColor);
                using Brush foreground = new SolidBrush(ForeColor);
                e.Graphics.FillRectangle(background, e.Bounds);

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using Pen iconPen = new Pen(Color.FromArgb(95, 99, 104), 2F);
                Rectangle iconBounds = new Rectangle(e.Bounds.X + 17, e.Bounds.Y + 15, 13, 13);
                e.Graphics.DrawEllipse(iconPen, iconBounds);
                e.Graphics.DrawLine(iconPen, e.Bounds.X + 28, e.Bounds.Y + 26, e.Bounds.X + 35, e.Bounds.Y + 33);

                e.Graphics.DrawString(Items[e.Index]?.ToString() ?? "", Font, foreground,
                    new Rectangle(e.Bounds.X + 52, e.Bounds.Y + 10, e.Bounds.Width - 66, e.Bounds.Height - 12));
            }

            private void SearchBox_KeyDown(object? sender, KeyEventArgs e)
            {
                if (!Visible || Items.Count == 0) return;

                if (e.KeyCode == Keys.Down)
                {
                    SelectedIndex = Math.Min(SelectedIndex + 1, Items.Count - 1);
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Up)
                {
                    SelectedIndex = Math.Max(SelectedIndex - 1, 0);
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Enter && SelectedIndex >= 0)
                {
                    SelectItem(SelectedIndex);
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    RememberCurrentSearch();
                    Visible = false;
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    Visible = false;
                    e.Handled = true;
                }
            }

            private void RefreshDropdownItems()
            {
                if (searchBox.Parent == null || searchBox.IsDisposed || ownerForm.IsDisposed) return;

                string keyword = searchBox.Text.Trim();
                List<string> filtered = BuildDefaultMatches(keyword);

                BeginUpdate();
                Items.Clear();
                foreach (string item in filtered.Take(12))
                    Items.Add(item);
                EndUpdate();

                if (Items.Count == 0 || !searchBox.Focused)
                {
                    Visible = false;
                    return;
                }

                Point formPoint = ownerForm.PointToClient(searchBox.Parent.PointToScreen(
                    new Point(searchBox.Left, searchBox.Bottom + 6)));

                int maxWidth = Math.Max(260, ownerForm.ClientSize.Width - 40);
                Width = Math.Min(Math.Max(260, searchBox.Width), maxWidth);
                int left = Math.Max(20, Math.Min(formPoint.X, ownerForm.ClientSize.Width - Width - 20));
                Location = new Point(left, formPoint.Y);
                int availableHeight = Math.Max(80, ownerForm.ClientSize.Height - formPoint.Y - 20);
                Height = Math.Min(Math.Min(300, availableHeight), Math.Max(52, Items.Count * ItemHeight + 12));
                SelectedIndex = -1;
                Visible = true;
                BringToFront();
            }

            private void SelectItem(int index)
            {
                if (index < 0 || index >= Items.Count) return;

                selecting = true;
                searchBox.Text = Items[index]?.ToString() ?? "";
                searchBox.SelectionStart = searchBox.Text.Length;
                selecting = false;
                RememberCurrentSearch();
                Visible = false;
                searchBox.Focus();
            }


            private List<string> BuildDefaultMatches(string keyword)
            {
                List<string> history = GetHistory();
                return string.IsNullOrWhiteSpace(keyword)
                    ? history
                    : history
                        .Where(s => s.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                        .Concat(suggestions.Where(s => s.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0))
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToList();
            }
            private List<string> GetHistory()
            {
                return SearchHistory.TryGetValue(historyKey, out List<string>? values)
                    ? values.ToList()
                    : new List<string>();
            }

            private void RememberCurrentSearch()
            {
                string value = searchBox.Text.Trim();
                if (value.Length < 2) return;

                if (!SearchHistory.TryGetValue(historyKey, out List<string>? values))
                {
                    values = new List<string>();
                    SearchHistory[historyKey] = values;
                }

                values.RemoveAll(item => item.Equals(value, StringComparison.OrdinalIgnoreCase));
                values.Insert(0, value);
                if (values.Count > 10)
                    values.RemoveRange(10, values.Count - 10);
            }

            private void HideIfUnfocused()
            {
                if (!Focused && !searchBox.Focused)
                    Visible = false;
            }
        }
    }
}
