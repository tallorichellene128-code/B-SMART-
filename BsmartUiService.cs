using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace BSMART
{
    internal static class BsmartUiService
    {
        private static readonly HashSet<Form> StyledForms = new HashSet<Form>();
        private static readonly Color Primary = Color.FromArgb(0, 102, 204);
        private static readonly Color PrimaryDark = Color.FromArgb(0, 82, 165);
        private static readonly Color Danger = Color.FromArgb(205, 55, 65);
        private static readonly Color Success = Color.FromArgb(20, 145, 88);
        private static readonly Color Warning = Color.FromArgb(225, 145, 30);
        private static readonly Color PanelBlue = Color.FromArgb(127, 211, 237);
        private static readonly Color SoftBlue = Color.FromArgb(228, 245, 252);
        private static readonly Color Line = Color.FromArgb(175, 205, 220);
        private static readonly Color TextDark = Color.FromArgb(20, 42, 65);
        private const int ButtonRadius = 10;

        public static void AttachToOpenForms()
        {
            foreach (Form form in Application.OpenForms.Cast<Form>().ToArray())
            {
                PrepareForm(form);
                StyleOpenGrids(form);
            }
        }

        public static void PrepareForm(Form form)
        {
            if (StyledForms.Contains(form)) return;
            StyledForms.Add(form);
            BsmartActivityLogNavigation.Attach(form);
            StyleForm(form);
            form.FormClosed += (s, e) => StyledForms.Remove(form);
        }

        private static void StyleForm(Form form)
        {
            Size originalSize = form.Size;
            Size originalClientSize = form.ClientSize;
            Point originalLocation = form.Location;
            FormWindowState originalWindowState = form.WindowState;

            EnableDoubleBuffering(form);
            form.SuspendLayout();
            StyleControls(form);
            form.ResumeLayout(false);
            form.PerformLayout();

            form.WindowState = originalWindowState;
            if (originalWindowState == FormWindowState.Normal)
            {
                form.ClientSize = originalClientSize;
                form.Size = originalSize;
                form.Location = originalLocation;
            }
        }

        private static void StyleControls(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                switch (control)
                {
                    case Button button:
                        StyleButton(button);
                        break;
                    case TextBox textBox:
                        StyleTextBox(textBox);
                        break;
                    case ComboBox comboBox:
                        StyleComboBox(comboBox);
                        break;
                    case DateTimePicker picker:
                        picker.Font = new Font("Segoe UI", Math.Max(10F, picker.Font.Size), FontStyle.Regular);
                        break;
                    case DataGridView grid:
                        StyleGrid(grid);
                        AttachGridExportMenu(grid);
                        grid.DataBindingComplete -= Grid_DataBindingComplete;
                        grid.DataBindingComplete += Grid_DataBindingComplete;
                        break;
                    case Label label:
                        StyleLabel(label);
                        break;
                    case Panel panel:
                        if (panel.BackColor == Color.SkyBlue)
                            panel.BackColor = PanelBlue;
                        break;
                }

                if (control.HasChildren)
                    StyleControls(control);
            }

            AttachSearchIcons(parent);
        }

        private static void Grid_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (sender is DataGridView grid)
            {
                StyleGrid(grid);
                AttachGridExportMenu(grid);
            }
        }

        private static void StyleOpenGrids(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control is DataGridView grid)
                {
                    StyleGrid(grid);
                    AttachGridExportMenu(grid);
                }

                if (control.HasChildren)
                    StyleOpenGrids(control);
            }
        }

        private static void AttachGridExportMenu(DataGridView grid)
        {
            if (grid == null || grid.IsDisposed) return;

            ContextMenuStrip menu = grid.ContextMenuStrip ?? new ContextMenuStrip();
            if (!menu.Items.Cast<ToolStripItem>().Any(i => i.Name == "BsmartExportCsv"))
            {
                ToolStripMenuItem exportCsv = new ToolStripMenuItem("Export table to CSV")
                {
                    Name = "BsmartExportCsv"
                };
                exportCsv.Click += (s, e) => BsmartTableExportService.ExportGridToCsv(grid, grid.FindForm()?.Text ?? "BSMART Table");
                menu.Items.Add(exportCsv);
            }

            if (!menu.Items.Cast<ToolStripItem>().Any(i => i.Name == "BsmartPrintPreview"))
            {
                ToolStripMenuItem previewPrint = new ToolStripMenuItem("Print preview table")
                {
                    Name = "BsmartPrintPreview"
                };
                previewPrint.Click += (s, e) => BsmartTableExportService.PrintPreviewGrid(grid, grid.FindForm()?.Text ?? "BSMART Table");
                menu.Items.Add(previewPrint);
            }
            grid.ContextMenuStrip = menu;
        }

        private static void StyleButton(Button button)
        {
            string text = (button.Text ?? "").Trim().ToLowerInvariant();
            string name = (button.Name ?? "").ToLowerInvariant();
            bool iconButton = button.Width <= 80 && button.Height <= 80 && string.IsNullOrWhiteSpace(button.Text);
            if (iconButton) return;
            bool sideMenuButton = IsSideMenuButton(button);

            button.FlatStyle = FlatStyle.Flat;
            button.UseVisualStyleBackColor = false;
            button.Font = new Font("Segoe UI Semibold", ButtonFontSize(button), FontStyle.Bold);
            if (!sideMenuButton)
                FitButtonToText(button);
            button.FlatAppearance.BorderSize = 1;
            button.Cursor = Cursors.Hand;
            RoundButton(button);
            button.Resize -= Button_Resize;
            button.Resize += Button_Resize;

            if (sideMenuButton)
            {
                button.BackColor = Color.White;
                button.ForeColor = TextDark;
                button.FlatAppearance.BorderColor = Line;
                button.TextAlign = ContentAlignment.MiddleCenter;
                button.Padding = new Padding(4, 0, 4, 0);
                return;
            }

            if (IsDanger(text, name))
            {
                button.BackColor = Danger;
                button.ForeColor = Color.White;
                button.FlatAppearance.BorderColor = Color.FromArgb(170, 40, 50);
                return;
            }

            if (IsPrimary(text, name))
            {
                button.BackColor = Primary;
                button.ForeColor = Color.White;
                button.FlatAppearance.BorderColor = PrimaryDark;
                return;
            }

            if (IsSuccess(text, name))
            {
                button.BackColor = Success;
                button.ForeColor = Color.White;
                button.FlatAppearance.BorderColor = Color.FromArgb(15, 110, 70);
                return;
            }

            if (IsWarning(text, name))
            {
                button.BackColor = Warning;
                button.ForeColor = Color.White;
                button.FlatAppearance.BorderColor = Color.FromArgb(190, 115, 20);
                return;
            }

            button.BackColor = Color.White;
            button.ForeColor = TextDark;
            button.FlatAppearance.BorderColor = Line;
        }

        private static void Button_Resize(object? sender, EventArgs e)
        {
            if (sender is Button button) RoundButton(button);
        }

        private static void RoundButton(Button button)
        {
            int radius = Math.Min(ButtonRadius, Math.Min(button.Width, button.Height) / 3);
            if (radius <= 1) return;

            Rectangle bounds = new Rectangle(0, 0, Math.Max(1, button.Width), Math.Max(1, button.Height));
            button.Region?.Dispose();
            button.Region = new Region(RoundedRect(bounds, radius));
        }

        private static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Rectangle arc = new Rectangle(bounds.Location, new Size(diameter, diameter));
            GraphicsPath path = new GraphicsPath();

            path.AddArc(arc, 180, 90);
            arc.X = bounds.Right - diameter - 1;
            path.AddArc(arc, 270, 90);
            arc.Y = bounds.Bottom - diameter - 1;
            path.AddArc(arc, 0, 90);
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();
            return path;
        }

        private static float ButtonFontSize(Button button)
        {
            if (button.Width < 95 || button.Height < 42) return 9F;
            if (button.Width > 230 && button.Height > 60) return 11.5F;
            return 10.5F;
        }

        private static void FitButtonToText(Button button)
        {
            string text = (button.Text ?? "").Trim();
            if (string.IsNullOrWhiteSpace(text)) return;

            Size preferred = TextRenderer.MeasureText(text, button.Font);
            int neededWidth = preferred.Width + 28;
            int neededHeight = Math.Max(36, preferred.Height + 14);
            int newWidth = Math.Max(button.Width, neededWidth);
            int newHeight = Math.Max(button.Height, neededHeight);

            if (button.Parent != null)
            {
                int maxWidth = Math.Max(button.Width, button.Parent.ClientSize.Width - button.Left - 8);
                int maxHeight = Math.Max(button.Height, button.Parent.ClientSize.Height - button.Top - 8);
                newWidth = Math.Min(newWidth, maxWidth);
                newHeight = Math.Min(newHeight, maxHeight);
            }

            if (newWidth > button.Width || newHeight > button.Height)
                button.Size = new Size(newWidth, newHeight);

            if (button.Width < neededWidth || button.Height < neededHeight)
            {
                float smaller = Math.Max(8F, button.Font.Size - 1F);
                button.Font = new Font(button.Font.FontFamily, smaller, button.Font.Style);
            }

            button.Padding = new Padding(4, 0, 4, 1);
        }

        private static bool IsSideMenuButton(Button button)
        {
            string text = (button.Text ?? "").Trim().ToLowerInvariant();
            string name = (button.Name ?? "").ToLowerInvariant();
            bool menuSized = button.Width >= 180 && button.Height >= 55;
            bool nearLeftEdge = button.Left <= 10;
            bool insideMenuPanel = button.Parent is Panel parent && parent.Left <= 5 && parent.Width <= 320;
            bool likelySidebarButton = nearLeftEdge && button.Width <= 320 && button.Top >= 70;
            bool navigationText = text.Contains("dashboard") || text.Contains("record")
                || text.Contains("report") || text.Contains("inventory") || text.Contains("archive")
                || text.Contains("account") || text.Contains("appointment") || text.Contains("service")
                || name.Contains("dash") || name.Contains("record") || name.Contains("report")
                || name.Contains("inventory") || name.Contains("archive") || name.Contains("account")
                || name.Contains("app") || name.Contains("service");

            return menuSized && navigationText && (insideMenuPanel || likelySidebarButton);
        }

        private static bool IsPrimary(string text, string name)
        {
            return text.Contains("login") || text.Contains("register") || text.Contains("download")
                || text.Contains("book") || text.Contains("approve") || text.Contains("confirm")
                || text.Contains("save") || text.Contains("update")
                || name.Contains("login") || name.Contains("download") || name.Contains("update");
        }

        private static bool IsSuccess(string text, string name)
        {
            return text.Contains("add") || text.Contains("restore") || text.Contains("unfreeze")
                || name.Contains("add") || name.Contains("restore") || name.Contains("unfreeze");
        }

        private static bool IsWarning(string text, string name)
        {
            return text.Contains("archive") || text.Contains("freeze")
                || name.Contains("archive") || name.Contains("freeze");
        }

        private static bool IsDanger(string text, string name)
        {
            return text.Contains("delete") || text.Contains("remove") || text.Contains("reject")
                || text.Contains("cancel") || text.Contains("logout")
                || name.Contains("delete") || name.Contains("remove") || name.Contains("reject")
                || name.Contains("cancel") || name.Contains("logout");
        }

        private static void StyleTextBox(TextBox textBox)
        {
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.Font = new Font("Segoe UI", Math.Max(10F, textBox.Font.Size), FontStyle.Regular);
            textBox.BackColor = Color.White;
            textBox.ForeColor = TextDark;

            string name = textBox.Name.ToLowerInvariant();
            if (name.Contains("password") || name.Contains("pass"))
                textBox.UseSystemPasswordChar = true;
        }

        private static void AttachSearchIcons(Control parent)
        {
            foreach (TextBox searchBox in parent.Controls.OfType<TextBox>().Where(IsSearchTextBox).ToList())
            {
                string iconName = "bsmartSearchIcon_" + searchBox.Name;
                if (parent.Controls.Find(iconName, false).FirstOrDefault() is PictureBox existingIcon)
                {
                    PositionSearchIcon(searchBox, existingIcon);
                    continue;
                }

                PictureBox icon = new PictureBox
                {
                    Name = iconName,
                    Size = new Size(42, Math.Max(34, searchBox.Height)),
                    BackColor = Color.Transparent,
                    Cursor = Cursors.Hand,
                    TabStop = false
                };

                icon.Paint += SearchIcon_Paint;
                icon.Click += (s, e) => searchBox.Focus();
                searchBox.LocationChanged += (s, e) => PositionSearchIcon(searchBox, icon);
                searchBox.SizeChanged += (s, e) => PositionSearchIcon(searchBox, icon);
                parent.SizeChanged += (s, e) => PositionSearchIcon(searchBox, icon);

                parent.Controls.Add(icon);
                PositionSearchIcon(searchBox, icon);
                icon.BringToFront();
            }
        }

        private static bool IsSearchTextBox(TextBox textBox)
        {
            string name = (textBox.Name ?? "").ToLowerInvariant();
            return name.Contains("search");
        }

        private static void PositionSearchIcon(TextBox searchBox, PictureBox icon)
        {
            if (searchBox.IsDisposed || icon.IsDisposed || searchBox.Parent == null) return;

            int gap = 6;
            int rightPadding = 8;
            int iconLeft = searchBox.Right + gap;
            int maxLeft = searchBox.Parent.ClientSize.Width - icon.Width - rightPadding;
            int nearestRight = searchBox.Parent.Controls.Cast<Control>()
                .Where(c => c != searchBox
                    && c != icon
                    && c.Visible
                    && c.Top < searchBox.Bottom + 8
                    && c.Bottom > searchBox.Top - 8
                    && c.Left >= searchBox.Right)
                .Select(c => c.Left)
                .DefaultIfEmpty(int.MaxValue)
                .Min();

            if (nearestRight != int.MaxValue)
                maxLeft = Math.Min(maxLeft, nearestRight - icon.Width - gap);

            if (maxLeft > searchBox.Left && iconLeft > maxLeft)
            {
                iconLeft = maxLeft;
                int availableWidth = iconLeft - searchBox.Left - gap;
                if (availableWidth >= 120 && availableWidth < searchBox.Width)
                    searchBox.Width = availableWidth;
            }

            icon.Height = Math.Max(34, searchBox.Height);
            icon.Location = new Point(Math.Max(searchBox.Left, iconLeft), searchBox.Top - 1);
            icon.Invalidate();
        }

        private static void SearchIcon_Paint(object? sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle lens = new Rectangle(7, 5, 23, 23);

            using Pen lensPen = new Pen(Color.FromArgb(80, 170, 220), 4F);
            using Pen handlePen = new Pen(Color.FromArgb(25, 45, 85), 5F)
            {
                StartCap = LineCap.Round,
                EndCap = LineCap.Round
            };

            e.Graphics.DrawEllipse(lensPen, lens);
            e.Graphics.DrawLine(handlePen, 27, 27, 36, 36);
        }

        private static void StyleComboBox(ComboBox comboBox)
        {
            comboBox.FlatStyle = FlatStyle.Flat;
            comboBox.Font = new Font("Segoe UI", Math.Max(10F, comboBox.Font.Size), FontStyle.Regular);
            comboBox.BackColor = Color.White;
            comboBox.ForeColor = TextDark;
        }

        public static void StyleGrid(DataGridView grid)
        {
            if (grid.IsDisposed) return;

            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.FixedSingle;
            grid.RowHeadersVisible = false;
            grid.EnableHeadersVisualStyles = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grid.GridColor = Color.FromArgb(190, 215, 230);

            grid.ColumnHeadersDefaultCellStyle.BackColor = Primary;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;

            grid.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            grid.DefaultCellStyle.ForeColor = TextDark;
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 126, 210);
            grid.DefaultCellStyle.SelectionForeColor = Color.White;
            grid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            grid.AlternatingRowsDefaultCellStyle.BackColor = SoftBlue;
            grid.RowTemplate.Height = 42;

            foreach (DataGridViewColumn column in grid.Columns)
            {
                if (column == null) continue;

                try
                {
                    if (column.Name != null && column.Name.StartsWith("_", StringComparison.Ordinal))
                        column.Visible = false;

                    if (column.DefaultCellStyle != null)
                        column.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

                    if (column.HeaderCell != null)
                        column.HeaderCell.Style.WrapMode = DataGridViewTriState.True;

                    column.MinimumWidth = PreferredMinimumWidth(column);
                    column.FillWeight = PreferredFillWeight(column);
                }
                catch
                {
                    // Skip columns that are not fully initialized
                }
            }
        }

        private static int PreferredMinimumWidth(DataGridViewColumn column)
        {
            string key = (string.IsNullOrEmpty(column.HeaderText)
                ? (column.Name ?? "")
                : column.HeaderText).ToLowerInvariant();

            if (key == "id" || key.EndsWith(" id")) return 70;
            if (key.Contains("age")) return 55;
            if (key.Contains("date") || key.Contains("birthday") || key.Contains("expiry")) return 105;
            if (key.Contains("assigned") || key.Contains("doctor") || key.Contains("nurse")) return 150;
            if (key.Contains("treatment") || key.Contains("description")) return 210;
            if (key.Contains("diagnosis") || key.Contains("violation")) return 140;
            if (key.Contains("resident") || key.Contains("name")) return 150;
            if (key.Contains("address")) return 190;
            if (key.Contains("email")) return 170;
            if (key.Contains("mobile") || key.Contains("phone")) return 120;
            if (key.Contains("barangay") || key.Contains("status")) return 110;
            return 95;
        }

        private static float PreferredFillWeight(DataGridViewColumn column)
        {
            string key = (string.IsNullOrEmpty(column.HeaderText)
                ? (column.Name ?? "")
                : column.HeaderText).ToLowerInvariant();

            if (key == "id" || key.EndsWith(" id")) return 55;
            if (key.Contains("age")) return 45;
            if (key.Contains("date") || key.Contains("birthday") || key.Contains("expiry")) return 90;
            if (key.Contains("assigned") || key.Contains("doctor") || key.Contains("nurse")) return 140;
            if (key.Contains("treatment") || key.Contains("description")) return 220;
            if (key.Contains("diagnosis") || key.Contains("violation")) return 130;
            if (key.Contains("resident") || key.Contains("name")) return 145;
            if (key.Contains("address")) return 170;
            if (key.Contains("email")) return 150;
            if (key.Contains("mobile") || key.Contains("phone")) return 100;
            if (key.Contains("barangay") || key.Contains("status")) return 105;
            return 100;
        }

        private static void StyleLabel(Label label)
        {
            if (label.Font.Size >= 18)
            {
                label.ForeColor = Color.Black;
                return;
            }

            if (label.BackColor == Color.SkyBlue)
                label.BackColor = PanelBlue;
        }

        private static void EnableDoubleBuffering(Control control)
        {
            try
            {
                typeof(Control)
                    .GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
                    ?.SetValue(control, true, null);
            }
            catch
            {
                // Visual polish only; ignore controls that do not allow this property.
            }

            foreach (Control child in control.Controls)
                EnableDoubleBuffering(child);
        }
    }
}
