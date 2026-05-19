using System;
using System.Globalization;
using System.Linq;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;

namespace BSMART
{
    internal static class BsmartTableExportService
    {
        private static string[] printLines = Array.Empty<string>();
        private static int printLineIndex;

        public static void ExportGridToCsv(DataGridView grid, string title = "BSMART Export")
        {
            if (grid == null || grid.Rows.Cast<DataGridViewRow>().All(r => r.IsNewRow))
            {
                MessageBox.Show("There are no records to export.", "Export",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "CSV File (*.csv)|*.csv",
                FileName = SafeFileName(title) + "-" + DateTime.Now.ToString("yyyyMMdd-HHmm", CultureInfo.InvariantCulture) + ".csv"
            };

            if (dialog.ShowDialog() != DialogResult.OK) return;

            try
            {
                StringBuilder csv = new StringBuilder();
                var columns = grid.Columns.Cast<DataGridViewColumn>()
                    .Where(c => c.Visible && !c.Name.StartsWith("_", StringComparison.Ordinal))
                    .OrderBy(c => c.DisplayIndex)
                    .ToList();

                csv.AppendLine(string.Join(",", columns.Select(c => Csv(c.HeaderText))));

                foreach (DataGridViewRow row in grid.Rows)
                {
                    if (row.IsNewRow) continue;
                    csv.AppendLine(string.Join(",", columns.Select(c => Csv(Format(row.Cells[c.Index].Value)))));
                }

                System.IO.File.WriteAllText(dialog.FileName, csv.ToString(), new UTF8Encoding(true));
                BsmartAuditService.Log("Export CSV", "table", null, title);
                MessageBox.Show("CSV export created successfully.", "Export",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error exporting CSV: " + ex.Message, "Export Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string Format(object value)
        {
            if (value == null) return "";
            if (value is DateTime date) return date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            return Convert.ToString(value, CultureInfo.InvariantCulture) ?? "";
        }

        private static string Csv(string text)
        {
            text ??= "";
            return "\"" + text.Replace("\"", "\"\"") + "\"";
        }

        private static string SafeFileName(string title)
        {
            string safe = string.Join("-", (title ?? "BSMART Export").Split(System.IO.Path.GetInvalidFileNameChars()));
            return string.IsNullOrWhiteSpace(safe) ? "BSMART-Export" : safe.Trim();
        }

        public static void PrintPreviewGrid(DataGridView grid, string title = "BSMART Table")
        {
            if (grid == null || grid.Rows.Cast<DataGridViewRow>().All(r => r.IsNewRow))
            {
                MessageBox.Show("There are no records to preview.", "Print Preview",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var columns = grid.Columns.Cast<DataGridViewColumn>()
                .Where(c => c.Visible && !c.Name.StartsWith("_", StringComparison.Ordinal))
                .OrderBy(c => c.DisplayIndex)
                .ToList();

            var lines = new System.Collections.Generic.List<string> { title, "Generated: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture), "" };
            lines.Add(string.Join(" | ", columns.Select(c => c.HeaderText)));
            lines.Add(new string('-', 110));
            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.IsNewRow) continue;
                lines.Add(string.Join(" | ", columns.Select(c => Format(row.Cells[c.Index].Value))));
            }

            printLines = lines.ToArray();
            printLineIndex = 0;

            using PrintDocument document = new PrintDocument();
            document.DocumentName = title;
            document.PrintPage += PrintPage;
            using PrintPreviewDialog preview = new PrintPreviewDialog
            {
                Document = document,
                Width = 1000,
                Height = 700
            };
            preview.ShowDialog();
            BsmartAuditService.Log("Print Preview", "table", null, title);
        }

        private static void PrintPage(object sender, PrintPageEventArgs e)
        {
            Font font = new Font("Segoe UI", 9F);
            float y = e.MarginBounds.Top;
            float lineHeight = font.GetHeight(e.Graphics) + 5;

            while (printLineIndex < printLines.Length)
            {
                e.Graphics.DrawString(printLines[printLineIndex], font, Brushes.Black,
                    new RectangleF(e.MarginBounds.Left, y, e.MarginBounds.Width, lineHeight + 4));
                y += lineHeight;
                printLineIndex++;

                if (y + lineHeight > e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true;
                    return;
                }
            }

            e.HasMorePages = false;
            printLineIndex = 0;
        }
    }
}
