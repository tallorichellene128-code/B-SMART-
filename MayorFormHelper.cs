using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BSMART
{
    internal static class MayorFormHelper
    {
        private const string ConnectionString = "Server=localhost;Database=bsmart_db;Uid=root;Pwd=;";

        public static void ConfigureMayorCombo(ComboBox combo)
        {
            if (!combo.Items.Contains("All Barangays"))
                combo.Items.Insert(0, "All Barangays");

            combo.DropDownStyle = ComboBoxStyle.DropDownList;
            combo.SelectedIndex = 0;
        }

        public static void AttachTopBar(Button notification, Button settings, Button logout, Form current)
        {
            BsmartNotificationService.Attach(notification, current);

            settings.Click += (s, e) =>
            {
                SettingsNavigationService.OpenSettings(current);
            };

            logout.Click += (s, e) =>
            {
                SettingsNavigationService.OpenLogin(current);
            };
        }

        public static DataTable LoadResidentAccounts(string barangay, string keyword)
        {
            using MySqlConnection conn = new MySqlConnection(ConnectionString);
            conn.Open();
            BsmartIdHelper.EnsureCodes(conn);

            DataTable result = BuildResidentResultTable();
            Dictionary<string, DataRow> residents = new Dictionary<string, DataRow>();

            using MySqlCommand accountCmd = new MySqlCommand(@"
                SELECT
                    u.id,
                    u.resident_code,
                    b.name AS barangay,
                    TRIM(COALESCE(NULLIF(u.full_name, ''), CONCAT(u.first_name, ' ', u.last_name))) AS resident,
                    u.gender,
                    u.age,
                    u.address,
                    u.mobile_number,
                    u.email,
                    u.first_name,
                    u.last_name,
                    u.birthday,
                    u.barangay_id
                FROM users u
                LEFT JOIN barangays b ON b.id = u.barangay_id
                WHERE u.role = 'Resident'
                  AND COALESCE(u.is_archived, 0) = 0", conn);
            DataTable accounts = new DataTable();
            new MySqlDataAdapter(accountCmd).Fill(accounts);

            using MySqlCommand healthCmd = new MySqlCommand(@"
                SELECT
                    h.ID AS id,
                    h.resident_code,
                    b.name AS barangay,
                    CONCAT(h.first_name, ' ', h.last_name) AS resident,
                    h.gender,
                    h.age,
                    h.address,
                    h.violation,
                    h.first_name,
                    h.last_name,
                    h.birthday,
                    h.barangay_id
                FROM health_records h
                LEFT JOIN barangays b ON b.id = h.barangay_id
                WHERE h.is_archived = 0", conn);
            DataTable healthRecords = new DataTable();
            new MySqlDataAdapter(healthCmd).Fill(healthRecords);

            foreach (DataRow row in accounts.Rows)
            {
                if (!ResidentMatches(row, barangay, keyword, includeAccountFields: true)) continue;

                string key = ResidentKey(row);
                DataRow target = result.NewRow();
                FillResidentRow(target, row, "Account");
                result.Rows.Add(target);
                residents[key] = target;
            }

            foreach (DataRow row in healthRecords.Rows)
            {
                if (!ResidentMatches(row, barangay, keyword, includeAccountFields: false)) continue;

                string key = ResidentKey(row);
                if (residents.TryGetValue(key, out DataRow? existing))
                {
                    if (string.IsNullOrWhiteSpace(Text(existing["Address"]))
                        && row.Table.Columns.Contains("address"))
                    {
                        existing["Address"] = Text(row["address"]);
                    }

                    existing["Violation"] = MergeViolation(Text(existing["Violation"]), Text(row["violation"]));
                    continue;
                }

                DataRow target = result.NewRow();
                FillResidentRow(target, row, "Health Record Only");
                result.Rows.Add(target);
                residents[key] = target;
            }

            DataView view = result.DefaultView;
            view.Sort = "Barangay ASC, Resident ASC";
            return view.ToTable();
        }

        private static DataTable BuildResidentResultTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("ID", typeof(string));
            table.Columns.Add("Barangay", typeof(string));
            table.Columns.Add("Resident", typeof(string));
            table.Columns.Add("Gender", typeof(string));
            table.Columns.Add("Age", typeof(string));
            table.Columns.Add("Address", typeof(string));
            table.Columns.Add("Mobile Number", typeof(string));
            table.Columns.Add("Email", typeof(string));
            table.Columns.Add("Violation", typeof(string));
            return table;
        }

        private static void FillResidentRow(DataRow target, DataRow source, string rowSource)
        {
            target["ID"] = source.Table.Columns.Contains("resident_code") && !string.IsNullOrWhiteSpace(Text(source["resident_code"]))
                ? Text(source["resident_code"])
                : Text(source["id"]);
            target["Barangay"] = Text(source["barangay"]);
            target["Resident"] = Text(source["resident"]);
            target["Gender"] = Text(source["gender"]);
            target["Age"] = Text(source["age"]);
            target["Address"] = source.Table.Columns.Contains("address") ? Text(source["address"]) : "";
            target["Mobile Number"] = rowSource == "Account" ? Text(source["mobile_number"]) : "";
            target["Email"] = rowSource == "Account" ? Text(source["email"]) : "";
            target["Violation"] = source.Table.Columns.Contains("violation") ? Text(source["violation"]) : "";
        }

        private static string MergeViolation(string current, string next)
        {
            if (string.IsNullOrWhiteSpace(next)) return current;
            if (string.IsNullOrWhiteSpace(current)) return next;

            string[] parts = current.Split(new[] { "; " }, StringSplitOptions.RemoveEmptyEntries);
            return parts.Any(p => string.Equals(p, next, StringComparison.OrdinalIgnoreCase))
                ? current
                : current + "; " + next;
        }

        private static bool ResidentMatches(DataRow row, string barangay, string keyword, bool includeAccountFields)
        {
            string rowBarangay = Text(row["barangay"]);
            if (!string.IsNullOrWhiteSpace(barangay)
                && barangay != "All Barangays"
                && !string.Equals(rowBarangay, barangay, StringComparison.OrdinalIgnoreCase))
                return false;

            if (string.IsNullOrWhiteSpace(keyword)) return true;

            string key = keyword.Trim();
            if (ContainsText(Text(row["resident"]), key)) return true;
            if (ContainsText(Text(row["first_name"]), key)) return true;
            if (ContainsText(Text(row["last_name"]), key)) return true;
            if (ContainsText(Text(row["id"]), key)) return true;
            if (ContainsText(Text(row["resident_code"]), key)) return true;
            if (ContainsText(Text(row["barangay"]), key)) return true;
            if (ContainsText(Text(row["gender"]), key)) return true;
            if (ContainsText(Text(row["age"]), key)) return true;
            if (ContainsText(Text(row["birthday"]), key)) return true;
            if (row.Table.Columns.Contains("violation") && ContainsText(Text(row["violation"]), key)) return true;

            if (row.Table.Columns.Contains("address") && ContainsText(Text(row["address"]), key)) return true;

            return includeAccountFields
                && (ContainsText(Text(row["mobile_number"]), key)
                    || ContainsText(Text(row["email"]), key));
        }

        private static string ResidentKey(DataRow row)
        {
            string first = Text(row["first_name"]).Trim().ToLowerInvariant();
            string last = Text(row["last_name"]).Trim().ToLowerInvariant();
            string birthday = row["birthday"] == DBNull.Value
                ? ""
                : Convert.ToDateTime(row["birthday"], CultureInfo.InvariantCulture).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            string barangayId = Text(row["barangay_id"]);
            return $"{first}|{last}|{birthday}|{barangayId}";
        }

        private static bool ContainsText(string value, string keyword)
        {
            return value.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static string Text(object value)
        {
            return value == null || value == DBNull.Value ? "" : Convert.ToString(value, CultureInfo.InvariantCulture) ?? "";
        }

        private static int ToInt(object value)
        {
            return value == null || value == DBNull.Value ? 0 : Convert.ToInt32(value, CultureInfo.InvariantCulture);
        }

        public static DataTable LoadHealthRecords(string barangay, string keyword)
        {
            return LoadHealthRecords(barangay, keyword, "");
        }

        public static DataTable LoadHealthRecords(string barangay, string keyword, string period)
        {
            return FillTable(@"SELECT
                           h.ID AS '_DBID',
                           COALESCE(h.resident_code, CAST(h.ID AS CHAR)) AS 'ID',
                           b.name AS 'Barangay',
                           CONCAT(h.first_name, ' ', h.last_name) AS 'Resident',
                           h.gender AS 'Gender',
                           h.age AS 'Age',
                           CASE WHEN UPPER(TRIM(COALESCE(h.Diagnosis, ''))) = 'N/A' THEN '' ELSE COALESCE(h.Diagnosis, '') END AS 'Diagnosis',
                           CASE WHEN UPPER(TRIM(COALESCE(h.Diagnosis, ''))) = 'N/A' THEN '' ELSE COALESCE(h.Treatment, '') END AS 'Treatment',
                           h.assigned_doc_name AS 'Assigned Doctor/Nurse',
                           h.Date AS 'Date'
                       FROM health_records h
                       LEFT JOIN barangays b ON b.id = h.barangay_id
                       WHERE h.is_archived = 0
                         AND h.Diagnosis IS NOT NULL
                         AND TRIM(h.Diagnosis) <> ''
                         AND UPPER(TRIM(h.Diagnosis)) <> 'N/A'",
                " ORDER BY h.Date DESC",
                barangay,
                keyword,
                @"COALESCE(h.resident_code, CAST(h.ID AS CHAR)) LIKE @kw COLLATE utf8mb4_0900_ai_ci
                  OR b.name LIKE @kw COLLATE utf8mb4_0900_ai_ci
                  OR h.first_name LIKE @kw COLLATE utf8mb4_0900_ai_ci
                  OR h.last_name LIKE @kw COLLATE utf8mb4_0900_ai_ci
                  OR CONCAT(h.first_name, ' ', h.last_name) LIKE @kw COLLATE utf8mb4_0900_ai_ci
                  OR h.gender LIKE @kw COLLATE utf8mb4_0900_ai_ci
                  OR CAST(h.age AS CHAR) LIKE @kw COLLATE utf8mb4_0900_ai_ci
                  OR DATE_FORMAT(h.Date, '%d/%m/%Y') LIKE @kw COLLATE utf8mb4_0900_ai_ci
                  OR DATE_FORMAT(h.Date, '%m/%d/%Y') LIKE @kw COLLATE utf8mb4_0900_ai_ci
                  OR DATE_FORMAT(h.Date, '%Y-%m-%d') LIKE @kw COLLATE utf8mb4_0900_ai_ci
                  OR h.Diagnosis LIKE @kw COLLATE utf8mb4_0900_ai_ci
                  OR h.Treatment LIKE @kw COLLATE utf8mb4_0900_ai_ci
                  OR h.assigned_doc_name LIKE @kw COLLATE utf8mb4_0900_ai_ci
                  OR COALESCE(h.violation, '') LIKE @kw COLLATE utf8mb4_0900_ai_ci",
                period,
                "h.Date");
        }

        public static DataTable LoadInventory(string barangay, string keyword)
        {
            DataTable medicines = FillTable(@"SELECT
                                                  m.id AS '_DBID',
                                                  COALESCE(NULLIF(m.medicine_code, ''), CAST(m.id AS CHAR)) AS 'ID',
                                                  b.name AS 'Barangay',
                                                  'Medicine' AS 'Type',
                                                  m.name AS 'Item',
                                                  m.quantity AS 'Quantity',
                                                  m.expiry_date AS 'Expiry Date'
                                              FROM medicines m
                                              LEFT JOIN barangays b ON b.id = m.barangay_id
                                              WHERE m.is_archived = 0",
                "", barangay, keyword,
                @"COALESCE(m.medicine_code, CAST(m.id AS CHAR)) LIKE @kw COLLATE utf8mb4_0900_ai_ci
                  OR b.name LIKE @kw COLLATE utf8mb4_0900_ai_ci
                  OR 'Medicine' LIKE @kw COLLATE utf8mb4_0900_ai_ci
                  OR m.name LIKE @kw COLLATE utf8mb4_0900_ai_ci
                  OR CAST(m.quantity AS CHAR) LIKE @kw COLLATE utf8mb4_0900_ai_ci
                  OR DATE_FORMAT(m.expiry_date, '%d/%m/%Y') LIKE @kw COLLATE utf8mb4_0900_ai_ci
                  OR DATE_FORMAT(m.expiry_date, '%m/%d/%Y') LIKE @kw COLLATE utf8mb4_0900_ai_ci
                  OR DATE_FORMAT(m.expiry_date, '%Y-%m-%d') LIKE @kw COLLATE utf8mb4_0900_ai_ci");

            DataTable vaccines = FillTable(@"SELECT
                                                 v.id AS '_DBID',
                                                 COALESCE(NULLIF(v.vaccine_code, ''), CAST(v.id AS CHAR)) AS 'ID',
                                                 b.name AS 'Barangay',
                                                 'Vaccine' AS 'Type',
                                                 v.name AS 'Item',
                                                 v.quantity AS 'Quantity',
                                                 v.expiry_date AS 'Expiry Date'
                                             FROM vaccines v
                                             LEFT JOIN barangays b ON b.id = v.barangay_id
                                             WHERE v.is_archived = 0",
                "", barangay, keyword,
                @"COALESCE(v.vaccine_code, CAST(v.id AS CHAR)) LIKE @kw COLLATE utf8mb4_0900_ai_ci
                  OR b.name LIKE @kw COLLATE utf8mb4_0900_ai_ci
                  OR 'Vaccine' LIKE @kw COLLATE utf8mb4_0900_ai_ci
                  OR v.name LIKE @kw COLLATE utf8mb4_0900_ai_ci
                  OR CAST(v.quantity AS CHAR) LIKE @kw COLLATE utf8mb4_0900_ai_ci
                  OR DATE_FORMAT(v.expiry_date, '%d/%m/%Y') LIKE @kw COLLATE utf8mb4_0900_ai_ci
                  OR DATE_FORMAT(v.expiry_date, '%m/%d/%Y') LIKE @kw COLLATE utf8mb4_0900_ai_ci
                  OR DATE_FORMAT(v.expiry_date, '%Y-%m-%d') LIKE @kw COLLATE utf8mb4_0900_ai_ci");

            medicines.Merge(vaccines);
            medicines = CollapseInventoryDuplicates(medicines);
            DataView view = medicines.DefaultView;
            view.Sort = "Barangay ASC, Type ASC, Item ASC";
            return view.ToTable();
        }

        private static DataTable CollapseInventoryDuplicates(DataTable source)
        {
            if (source == null || source.Rows.Count == 0) return source ?? new DataTable();

            DataTable result = source.Clone();
            var groups = source.AsEnumerable()
                .GroupBy(row => new
                {
                    Barangay = Convert.ToString(row["Barangay"])?.Trim() ?? "",
                    Type = Convert.ToString(row["Type"])?.Trim() ?? "",
                    Item = (Convert.ToString(row["Item"])?.Trim() ?? "").ToLowerInvariant(),
                    Expiry = row["Expiry Date"] == DBNull.Value ? "" : Convert.ToDateTime(row["Expiry Date"]).Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                });

            foreach (var group in groups)
            {
                DataRow first = group.First();
                DataRow row = result.NewRow();
                row["_DBID"] = first["_DBID"];
                row["ID"] = first["ID"];
                row["Barangay"] = first["Barangay"];
                row["Type"] = first["Type"];
                row["Item"] = first["Item"];
                row["Quantity"] = group.Sum(r => r["Quantity"] == DBNull.Value ? 0 : Convert.ToInt32(r["Quantity"]));
                row["Expiry Date"] = first["Expiry Date"];
                result.Rows.Add(row);
            }

            return result;
        }

        public static DataTable LoadRecentHealthRecords()
        {
            return FillTable(@"SELECT
                                   h.ID AS '_DBID',
                                   COALESCE(h.resident_code, CAST(h.ID AS CHAR)) AS 'ID',
                                   b.name AS 'Barangay',
                                   CONCAT(h.first_name, ' ', h.last_name) AS 'Resident',
                                   CASE WHEN UPPER(TRIM(COALESCE(h.Diagnosis, ''))) = 'N/A' THEN '' ELSE COALESCE(h.Diagnosis, '') END AS 'Diagnosis',
                                   CASE WHEN UPPER(TRIM(COALESCE(h.Diagnosis, ''))) = 'N/A' THEN '' ELSE COALESCE(h.Treatment, '') END AS 'Treatment',
                                   h.Date AS 'Date'
                               FROM health_records h
                               LEFT JOIN barangays b ON b.id = h.barangay_id
                               WHERE h.is_archived = 0
                               ORDER BY h.Date DESC
                               LIMIT 10");
        }

        public static int CountResidents()
        {
            return BsmartResidentCountService.CountResidents();
        }

        public static int CountHealthRecords(string barangay = "")
        {
            using MySqlConnection conn = new MySqlConnection(ConnectionString);
            conn.Open();
            using MySqlCommand cmd = new MySqlCommand(
                "SELECT COUNT(*) FROM health_records h LEFT JOIN barangays b ON b.id = h.barangay_id WHERE h.is_archived = 0 AND h.Diagnosis IS NOT NULL AND TRIM(h.Diagnosis) <> '' AND UPPER(TRIM(h.Diagnosis)) <> 'N/A'" + BarangayClause("b.name", barangay), conn);
            AddBarangayParameter(cmd, barangay);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public static string MostCommonDiagnosis()
        {
            object result = Scalar(@"SELECT Diagnosis
                                     FROM health_records
                                     WHERE is_archived = 0
                                       AND Diagnosis IS NOT NULL
                                       AND TRIM(Diagnosis) <> ''
                                       AND UPPER(TRIM(Diagnosis)) <> 'N/A'
                                     GROUP BY Diagnosis
                                     ORDER BY COUNT(*) DESC
                                     LIMIT 1");
            return result == null || result == DBNull.Value ? "N/A" : result.ToString() ?? "N/A";
        }

        public static void StyleGrid(DataGridView grid)
        {
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.None;
            grid.RowHeadersVisible = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 102, 204);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 9, FontStyle.Bold);
            grid.ColumnHeadersHeight = 35;
            grid.EnableHeadersVisualStyles = false;
            grid.DefaultCellStyle.Font = new Font("Arial", 9);
            grid.RowTemplate.Height = 30;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(220, 235, 255);
            foreach (DataGridViewColumn column in grid.Columns)
                if (column.Name.StartsWith("_", StringComparison.Ordinal))
                    column.Visible = false;
        }

        public static void ExportGridToPdf(DataGridView grid, string title)
        {
            if (grid == null || grid.Rows.Cast<DataGridViewRow>().All(r => r.IsNewRow))
            {
                MessageBox.Show("There are no records to download.", "Download",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            GridExportData data = GridSnapshot(grid);
            if (!ShowDownloadPreview(title, data))
                return;

            using SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                FileName = $"{SanitizeFileName(title)}-{DateTime.Now:yyyyMMdd-HHmm}.pdf"
            };

            if (dialog.ShowDialog() != DialogResult.OK) return;

            try
            {
                WriteSimplePdf(dialog.FileName, title, data);
                MessageBox.Show("PDF downloaded successfully.", "Download",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating PDF: " + ex.Message, "Download Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void ExportResidentMedicalCertificate(
            DataGridView grid,
            string residentName,
            string age,
            string barangay,
            string address,
            string email)
        {
            if (grid == null || grid.Rows.Cast<DataGridViewRow>().All(r => r.IsNewRow))
            {
                MessageBox.Show("There are no records to download.", "Download",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            GridExportData data = GridSnapshot(grid);
            string details = $"Resident: {residentName}\r\nAge: {age}\r\nBarangay: {barangay}\r\nAddress: {address}\r\nEmail: {email}";
            if (!ShowDownloadPreview("Medical Certificate", data, details))
                return;

            using SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                FileName = $"{SanitizeFileName(residentName)}-Medical-Certificate-{DateTime.Now:yyyyMMdd-HHmm}.pdf"
            };

            if (dialog.ShowDialog() != DialogResult.OK) return;

            try
            {
                WriteResidentMedicalCertificatePdf(dialog.FileName, data,
                    residentName, age, barangay, address, email);
                MessageBox.Show("Medical certificate downloaded successfully.", "Download",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating medical certificate: " + ex.Message,
                    "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static bool ShowDownloadPreview(string title, GridExportData data, string details = "")
        {
            using Form preview = new Form
            {
                Text = "Preview Before Download",
                StartPosition = FormStartPosition.CenterParent,
                Size = new Size(960, 640),
                MinimumSize = new Size(760, 480),
                BackColor = Color.White
            };

            Label titleLabel = new Label
            {
                Text = title,
                Location = new Point(24, 18),
                Size = new Size(900, 38),
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                AutoEllipsis = true
            };

            Label instruction = new Label
            {
                Text = "Preview the records below before downloading.",
                Location = new Point(26, 58),
                Size = new Size(880, 24),
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(70, 70, 70)
            };

            TextBox detailBox = new TextBox
            {
                Text = details,
                Location = new Point(28, 88),
                Size = new Size(888, string.IsNullOrWhiteSpace(details) ? 0 : 88),
                Multiline = true,
                ReadOnly = true,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(245, 250, 253),
                Font = new Font("Segoe UI", 10F),
                ScrollBars = ScrollBars.Vertical,
                Visible = !string.IsNullOrWhiteSpace(details)
            };

            int gridTop = string.IsNullOrWhiteSpace(details) ? 94 : 188;
            DataGridView previewGrid = new DataGridView
            {
                Location = new Point(28, gridTop),
                Size = new Size(888, 356 + (string.IsNullOrWhiteSpace(details) ? 94 : 0)),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                DataSource = BuildPreviewTable(data)
            };
            previewGrid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            previewGrid.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            previewGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 102, 204);
            previewGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            previewGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            previewGrid.EnableHeadersVisualStyles = false;

            Label countLabel = new Label
            {
                Text = $"{data.Rows.Count:N0} record(s) will be included.",
                Location = new Point(28, 548),
                Size = new Size(360, 28),
                Anchor = AnchorStyles.Left | AnchorStyles.Bottom,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 45, 45)
            };

            Button download = new Button
            {
                Text = "Download",
                DialogResult = DialogResult.OK,
                Size = new Size(132, 40),
                Location = new Point(638, 542),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                BackColor = Color.FromArgb(0, 112, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            Button cancel = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Size = new Size(132, 40),
                Location = new Point(784, 542),
                Anchor = AnchorStyles.Right | AnchorStyles.Bottom,
                BackColor = Color.FromArgb(210, 55, 65),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            preview.Controls.Add(titleLabel);
            preview.Controls.Add(instruction);
            preview.Controls.Add(detailBox);
            preview.Controls.Add(previewGrid);
            preview.Controls.Add(countLabel);
            preview.Controls.Add(download);
            preview.Controls.Add(cancel);
            preview.AcceptButton = download;
            preview.CancelButton = cancel;

            Form owner = Form.ActiveForm;
            return owner == null
                ? preview.ShowDialog() == DialogResult.OK
                : preview.ShowDialog(owner) == DialogResult.OK;
        }

        private static DataTable BuildPreviewTable(GridExportData data)
        {
            DataTable table = new DataTable();
            Dictionary<string, int> usedNames = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            foreach (string header in data.Headers)
            {
                string baseName = string.IsNullOrWhiteSpace(header) ? "Column" : header.Trim();
                string name = baseName;
                if (usedNames.TryGetValue(baseName, out int count))
                {
                    count++;
                    usedNames[baseName] = count;
                    name = $"{baseName} {count}";
                }
                else
                {
                    usedNames[baseName] = 1;
                }

                table.Columns.Add(name);
            }

            foreach (string[] row in data.Rows)
            {
                DataRow tableRow = table.NewRow();
                for (int i = 0; i < table.Columns.Count; i++)
                    tableRow[i] = i < row.Length ? row[i] : "";
                table.Rows.Add(tableRow);
            }

            return table;
        }

        public static void ShowMayorArchiveMessage()
        {
            MessageBox.Show("Mayor archive viewing is not available on these forms yet.",
                "Archive", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void HideMayorArchiveButton(Control parent)
        {
            foreach (Control control in parent.Controls.Find("btnViewArchive", true))
            {
                control.Visible = false;
                control.Enabled = false;
            }
        }

        public static void RenderHealthSummary(Panel totalPanel, Panel recoveredPanel, Panel activePanel, PictureBox chart, string barangay)
        {
            RenderHealthSummary(totalPanel, recoveredPanel, activePanel, chart, barangay, "");
        }

        public static void RenderHealthSummary(Panel totalPanel, Panel recoveredPanel, Panel activePanel, PictureBox chart, string barangay, string period)
        {
            int total = CountHealthRecords(barangay, period);
            int recovered = CountRecovered(barangay, period);
            int active = Math.Max(0, total - recovered);

            SetMetricPanel(totalPanel, TotalCasesCaption(period), total.ToString("N0", CultureInfo.InvariantCulture),
                "", Color.FromArgb(30, 30, 30), "");
            SetMetricPanel(recoveredPanel, "TOTAL RECOVERED", recovered.ToString("N0", CultureInfo.InvariantCulture),
                "", Color.FromArgb(22, 160, 73), "");
            SetMetricPanel(activePanel, "ACTIVE CASES", active.ToString("N0", CultureInfo.InvariantCulture),
                "", Color.FromArgb(220, 45, 50), "");
            RenderMostCommonCasesChart(chart, GetTopCommonCases(barangay, period));
        }

        private static DataTable FillTable(string baseSql, string orderSql = "", string barangay = "", string keyword = "", string keywordCondition = "")
        {
            return FillTable(baseSql, orderSql, barangay, keyword, keywordCondition, "", "");
        }

        private static DataTable FillTable(string baseSql, string orderSql, string barangay, string keyword, string keywordCondition, string period, string dateColumn)
        {
            using MySqlConnection conn = new MySqlConnection(ConnectionString);
            conn.Open();
            BsmartIdHelper.EnsureCodes(conn);

            string sql = baseSql + BarangayClause("b.name", barangay);
            sql += PeriodClause(dateColumn, period);
            if (!string.IsNullOrWhiteSpace(keyword) && !string.IsNullOrWhiteSpace(keywordCondition))
                sql += " AND (" + keywordCondition + ")";
            sql += orderSql;

            using MySqlCommand cmd = new MySqlCommand(sql, conn);
            AddBarangayParameter(cmd, barangay);
            AddPeriodParameters(cmd, period);
            if (!string.IsNullOrWhiteSpace(keyword))
                cmd.Parameters.AddWithValue("@kw", "%" + keyword.Trim() + "%");

            DataTable table = new DataTable();
            new MySqlDataAdapter(cmd).Fill(table);
            return table;
        }

        private static string BarangayClause(string columnName, string barangay)
        {
            return string.IsNullOrWhiteSpace(barangay) || barangay == "All Barangays"
                ? ""
                : $" AND {columnName} = @barangay";
        }

        private static void AddBarangayParameter(MySqlCommand cmd, string barangay)
        {
            if (!string.IsNullOrWhiteSpace(barangay) && barangay != "All Barangays")
                cmd.Parameters.AddWithValue("@barangay", barangay);
        }

        private static string PeriodClause(string dateColumn, string period)
        {
            if (string.IsNullOrWhiteSpace(dateColumn)) return "";

            switch (NormalizePeriod(period))
            {
                case "Weekly":
                    return $@" AND {dateColumn} >= DATE_SUB(
                                  COALESCE((SELECT MAX(hr.Date) FROM health_records hr WHERE hr.is_archived = 0), CURDATE()),
                                  INTERVAL 6 DAY)
                               AND {dateColumn} <= COALESCE((SELECT MAX(hr.Date) FROM health_records hr WHERE hr.is_archived = 0), CURDATE())";
                case "Monthly":
                    return $@" AND YEAR({dateColumn}) = YEAR(COALESCE((SELECT MAX(hr.Date) FROM health_records hr WHERE hr.is_archived = 0), CURDATE()))
                               AND MONTH({dateColumn}) = MONTH(COALESCE((SELECT MAX(hr.Date) FROM health_records hr WHERE hr.is_archived = 0), CURDATE()))";
                case "Yearly":
                    return $@" AND YEAR({dateColumn}) = YEAR(COALESCE((SELECT MAX(hr.Date) FROM health_records hr WHERE hr.is_archived = 0), CURDATE()))";
                default:
                    return "";
            }
        }

        private static void AddPeriodParameters(MySqlCommand cmd, string period)
        {
            // Period filters are anchored in SQL to the latest health record date,
            // so sample data still appears even when the computer date has moved on.
        }

        private static string NormalizePeriod(string period)
        {
            if (string.Equals(period, "Weekly", StringComparison.OrdinalIgnoreCase)) return "Weekly";
            if (string.Equals(period, "Monthly", StringComparison.OrdinalIgnoreCase)) return "Monthly";
            if (string.Equals(period, "Yearly", StringComparison.OrdinalIgnoreCase)) return "Yearly";
            return "";
        }

        private static string TotalCasesCaption(string period)
        {
            switch (NormalizePeriod(period))
            {
                case "Weekly": return "TOTAL CASES (WEEK)";
                case "Monthly": return "TOTAL CASES (MONTH)";
                default: return "TOTAL CASES (YTD)";
            }
        }

        private static object Scalar(string sql)
        {
            using MySqlConnection conn = new MySqlConnection(ConnectionString);
            conn.Open();
            using MySqlCommand cmd = new MySqlCommand(sql, conn);
            return cmd.ExecuteScalar();
        }

        public static int CountHealthRecords(string barangay, string period)
        {
            using MySqlConnection conn = new MySqlConnection(ConnectionString);
            conn.Open();
            using MySqlCommand cmd = new MySqlCommand(
                "SELECT COUNT(*) FROM health_records h LEFT JOIN barangays b ON b.id = h.barangay_id WHERE h.is_archived = 0 AND h.Diagnosis IS NOT NULL AND TRIM(h.Diagnosis) <> '' AND UPPER(TRIM(h.Diagnosis)) <> 'N/A'"
                + BarangayClause("b.name", barangay)
                + PeriodClause("h.Date", period), conn);
            AddBarangayParameter(cmd, barangay);
            AddPeriodParameters(cmd, period);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        private static int CountRecovered(string barangay, string period)
        {
            using MySqlConnection conn = new MySqlConnection(ConnectionString);
            conn.Open();

            string lowLevelRecoveryClause = @"
                (
                    h.Date <= DATE_SUB(
                        COALESCE((SELECT MAX(hr.Date) FROM health_records hr WHERE hr.is_archived = 0), CURDATE()),
                        INTERVAL 7 DAY)
                    AND (
                        LOWER(COALESCE(h.Diagnosis, '')) LIKE '%fever%'
                        OR LOWER(COALESCE(h.Diagnosis, '')) LIKE '%cough%'
                        OR LOWER(COALESCE(h.Diagnosis, '')) LIKE '%cold%'
                        OR LOWER(COALESCE(h.Diagnosis, '')) LIKE '%flu%'
                        OR LOWER(COALESCE(h.Diagnosis, '')) LIKE '%influenza%'
                        OR LOWER(COALESCE(h.Diagnosis, '')) LIKE '%headache%'
                        OR LOWER(COALESCE(h.Diagnosis, '')) LIKE '%sore throat%'
                        OR LOWER(COALESCE(h.Diagnosis, '')) LIKE '%runny nose%'
                        OR LOWER(COALESCE(h.Diagnosis, '')) LIKE '%allerg%'
                        OR LOWER(COALESCE(h.Diagnosis, '')) LIKE '%diarrhea%'
                        OR LOWER(COALESCE(h.Diagnosis, '')) LIKE '%stomachache%'
                        OR LOWER(COALESCE(h.Diagnosis, '')) LIKE '%toothache%'
                    )
                    AND LOWER(COALESCE(h.Diagnosis, '')) NOT LIKE '%dengue%'
                    AND LOWER(COALESCE(h.Diagnosis, '')) NOT LIKE '%pneumonia%'
                    AND LOWER(COALESCE(h.Diagnosis, '')) NOT LIKE '%tuberculosis%'
                    AND LOWER(COALESCE(h.Diagnosis, '')) NOT LIKE '%hypertension%'
                    AND LOWER(COALESCE(h.Diagnosis, '')) NOT LIKE '%diabetes%'
                    AND LOWER(COALESCE(h.Diagnosis, '')) NOT LIKE '%asthma%'
                    AND LOWER(COALESCE(h.Diagnosis, '')) NOT LIKE '%arthritis%'
                    AND LOWER(COALESCE(h.Diagnosis, '')) NOT LIKE '%urinary tract%'
                    AND LOWER(COALESCE(h.Diagnosis, '')) NOT LIKE '%uti%'
                    AND LOWER(COALESCE(h.Diagnosis, '')) NOT LIKE '%laceration%'
                    AND LOWER(COALESCE(h.Diagnosis, '')) NOT LIKE '%wound%'
                )";

            using MySqlCommand cmd = new MySqlCommand(
                @"SELECT COUNT(*)
                  FROM health_records h
                  LEFT JOIN barangays b ON b.id = h.barangay_id
                  WHERE h.is_archived = 0
                    AND h.Diagnosis IS NOT NULL
                    AND TRIM(h.Diagnosis) <> ''
                    AND UPPER(TRIM(h.Diagnosis)) <> 'N/A'
                    AND (
                        h.Treatment LIKE @recovered COLLATE utf8mb4_0900_ai_ci
                        OR h.Diagnosis LIKE @recovered COLLATE utf8mb4_0900_ai_ci
                        OR " + lowLevelRecoveryClause + @"
                    )"
                + BarangayClause("b.name", barangay)
                + PeriodClause("h.Date", period), conn);
            cmd.Parameters.AddWithValue("@recovered", "%recover%");
            AddBarangayParameter(cmd, barangay);
            AddPeriodParameters(cmd, period);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        private static void SetMetricPanel(Panel panel, string caption, string value, string trend, Color accentColor, string arrow)
        {
            panel.Controls.Clear();
            panel.BackColor = Color.FromArgb(123, 211, 238);

            Panel shadow = new Panel
            {
                BackColor = Color.FromArgb(160, 185, 195),
                Location = new Point(5, 7),
                Size = new Size(Math.Max(1, panel.Width - 10), Math.Max(1, panel.Height - 12)),
                Enabled = false
            };
            panel.Controls.Add(shadow);

            Panel card = new Panel
            {
                BackColor = Color.FromArgb(125, 214, 240),
                Location = new Point(0, 0),
                Size = new Size(panel.Width - 6, panel.Height - 9)
            };
            panel.Controls.Add(card);
            card.BringToFront();

            Label title = new Label
            {
                Text = caption,
                Font = FitLabelFont(caption, "Segoe UI Semibold", FontStyle.Bold, card.Width - 16, 11f, 8.5f),
                Location = new Point(8, 16),
                Size = new Size(card.Width - 16, 26),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoEllipsis = true,
                BackColor = Color.Transparent
            };
            card.Controls.Add(title);

            Label valueLabel = new Label
            {
                Text = value,
                Font = FitLabelFont(value, "Segoe UI", FontStyle.Bold, card.Width - 58, 27f, 18f),
                Location = new Point(8, 43),
                Size = new Size(card.Width - 16, 55),
                TextAlign = ContentAlignment.MiddleCenter,
                AutoEllipsis = true,
                BackColor = Color.Transparent
            };
            card.Controls.Add(valueLabel);

            if (!string.IsNullOrWhiteSpace(arrow))
            {
                Label arrowLabel = new Label
                {
                    Text = arrow,
                    Font = new Font("Segoe UI", 15, FontStyle.Bold),
                    ForeColor = accentColor,
                    Location = new Point(card.Width - 58, 55),
                    Size = new Size(34, 34),
                    TextAlign = ContentAlignment.MiddleLeft,
                    BackColor = Color.Transparent
                };
                card.Controls.Add(arrowLabel);
                arrowLabel.BringToFront();
            }

            if (!string.IsNullOrWhiteSpace(trend))
            {
                Label trendLabel = new Label
                {
                    Text = trend,
                    Font = FitLabelFont(trend, "Segoe UI", FontStyle.Regular, card.Width - 16, 10f, 8f),
                    ForeColor = accentColor,
                    Location = new Point(8, 99),
                    Size = new Size(card.Width - 16, 28),
                    TextAlign = ContentAlignment.MiddleCenter,
                    AutoEllipsis = true,
                    BackColor = Color.Transparent
                };
                card.Controls.Add(trendLabel);
            }
        }

        private static Font FitLabelFont(string text, string family, FontStyle style, int maxWidth, float maxSize, float minSize)
        {
            using Bitmap bitmap = new Bitmap(1, 1);
            using Graphics g = Graphics.FromImage(bitmap);

            for (float size = maxSize; size >= minSize; size -= 0.5f)
            {
                Font candidate = new Font(family, size, style);
                if (g.MeasureString(text, candidate).Width <= maxWidth)
                    return candidate;
                candidate.Dispose();
            }

            return new Font(family, minSize, style);
        }

        private static void SetPanelText(Panel panel, string caption, string value)
        {
            panel.Controls.Clear();
            panel.Controls.Add(new Label
            {
                Text = caption,
                Font = new Font("Segoe UI Semibold", 11, FontStyle.Bold),
                Location = new Point(12, 18),
                AutoSize = true,
                BackColor = Color.Transparent
            });
            panel.Controls.Add(new Label
            {
                Text = value,
                Font = new Font("Segoe UI Semibold", 22, FontStyle.Bold),
                Location = new Point(12, 62),
                AutoSize = true,
                BackColor = Color.Transparent
            });
        }

        private static List<(string Diagnosis, int Count)> GetTopCommonCases(string barangay, string period)
        {
            using MySqlConnection conn = new MySqlConnection(ConnectionString);
            conn.Open();
            using MySqlCommand cmd = new MySqlCommand(
                @"SELECT h.Diagnosis, COUNT(*) AS CaseCount
                  FROM health_records h
                  LEFT JOIN barangays b ON b.id = h.barangay_id
                  WHERE h.is_archived = 0
                    AND h.Diagnosis IS NOT NULL
                    AND TRIM(h.Diagnosis) <> ''
                    AND UPPER(TRIM(h.Diagnosis)) <> 'N/A'"
                + BarangayClause("b.name", barangay)
                + PeriodClause("h.Date", period)
                + @" GROUP BY h.Diagnosis
                     ORDER BY CaseCount DESC, h.Diagnosis ASC
                     LIMIT 3", conn);
            AddBarangayParameter(cmd, barangay);
            AddPeriodParameters(cmd, period);

            List<(string Diagnosis, int Count)> items = new List<(string Diagnosis, int Count)>();
            using MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                items.Add((reader["Diagnosis"].ToString() ?? "Unknown",
                    Convert.ToInt32(reader["CaseCount"], CultureInfo.InvariantCulture)));
            }

            return items;
        }

        private static void RenderMostCommonCasesChart(PictureBox chart, List<(string Diagnosis, int Count)> items)
        {
            Bitmap bitmap = new Bitmap(chart.Width, chart.Height);
            using Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Color.Transparent);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            Rectangle card = new Rectangle(4, 4, chart.Width - 10, chart.Height - 10);
            using SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(55, 0, 0, 0));
            using SolidBrush cardBrush = new SolidBrush(Color.FromArgb(248, 250, 252));
            using System.Drawing.Drawing2D.GraphicsPath shadowPath = RoundedRect(new Rectangle(card.X + 3, card.Y + 4, card.Width, card.Height), 7);
            using System.Drawing.Drawing2D.GraphicsPath cardPath = RoundedRect(card, 7);
            g.FillPath(shadowBrush, shadowPath);
            g.FillPath(cardBrush, cardPath);
            g.DrawPath(Pens.LightGray, cardPath);

            using Font titleFont = new Font("Segoe UI", 10.5f, FontStyle.Bold);
            using Font textFont = new Font("Segoe UI", 8.5f, FontStyle.Regular);
            using Font percentFont = new Font("Segoe UI", 7.0f, FontStyle.Regular);
            using SolidBrush textBrush = new SolidBrush(Color.Black);

            RectangleF titleBounds = new RectangleF(card.X + 14, card.Y + 10, card.Width - 28, 24);
            using StringFormat centeredFormat = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
                Trimming = StringTrimming.EllipsisCharacter,
                FormatFlags = StringFormatFlags.NoWrap
            };
            g.DrawString("MOST COMMON CASES (TOP 3)", titleFont, textBrush, titleBounds, centeredFormat);

            int total = items.Sum(i => i.Count);
            if (total == 0 || items.Count == 0)
            {
                g.DrawString("No data", textFont, Brushes.Gray, card.X + 88, card.Y + 72);
            }
            else
            {
                Color[] colors =
                {
                    Color.FromArgb(45, 130, 182),
                    Color.FromArgb(116, 193, 220),
                    Color.FromArgb(242, 150, 62)
                };

                int pieSize = Math.Min(chart.Height - 62, 112);
                Rectangle pie = new Rectangle(card.X + 38, card.Y + 45, pieSize, pieSize);
                float startAngle = -90f;

                for (int i = 0; i < items.Count; i++)
                {
                    float sweep = 360f * items[i].Count / total;
                    using SolidBrush sliceBrush = new SolidBrush(colors[i]);
                    g.FillPie(sliceBrush, pie, startAngle, sweep);
                    g.DrawPie(Pens.White, pie, startAngle, sweep);

                    float midAngle = startAngle + sweep / 2f;
                    double radians = Math.PI * midAngle / 180d;
                    int labelX = pie.X + pie.Width / 2 + (int)(Math.Cos(radians) * pie.Width * 0.25);
                    int labelY = pie.Y + pie.Height / 2 + (int)(Math.Sin(radians) * pie.Height * 0.25);
                    int percentage = (int)Math.Round(100d * items[i].Count / total);
                    string label = percentage + "%";
                    SizeF labelSize = g.MeasureString(label, percentFont);
                    g.DrawString(label, percentFont, textBrush,
                        labelX - labelSize.Width / 2,
                        labelY - labelSize.Height / 2);

                    startAngle += sweep;
                }

                int legendX = pie.Right + 18;
                int legendY = pie.Y + 10;
                for (int i = 0; i < items.Count; i++)
                {
                    using SolidBrush legendBrush = new SolidBrush(colors[i]);
                    g.FillEllipse(legendBrush, legendX, legendY + (i * 21), 12, 12);
                    float labelX = legendX + 18;
                    float labelWidth = Math.Max(10, card.Right - labelX - 12);
                    string diagnosis = FitText(g, items[i].Diagnosis, textFont, labelWidth);
                    g.DrawString(diagnosis, textFont, textBrush,
                        new RectangleF(labelX, legendY - 4 + (i * 21), labelWidth, 20),
                        new StringFormat { Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap });
                }
            }

            Image oldImage = chart.Image;
            chart.Image = bitmap;
            oldImage?.Dispose();
        }

        private static string FitText(Graphics g, string text, Font font, float maxWidth)
        {
            if (g.MeasureString(text, font).Width <= maxWidth) return text;

            const string ellipsis = "...";
            string fitted = text;
            while (fitted.Length > 0 && g.MeasureString(fitted + ellipsis, font).Width > maxWidth)
                fitted = fitted.Substring(0, fitted.Length - 1);

            return fitted.Length == 0 ? ellipsis : fitted + ellipsis;
        }

        private static System.Drawing.Drawing2D.GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Y, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }

        private sealed class GridExportData
        {
            public string[] Headers { get; set; } = Array.Empty<string>();
            public List<string[]> Rows { get; set; } = new List<string[]>();
        }

        private static GridExportData GridSnapshot(DataGridView grid)
        {
            DataGridViewColumn[] columns = grid.Columns.Cast<DataGridViewColumn>()
                .Where(c => c.Visible)
                .ToArray();

            GridExportData data = new GridExportData
            {
                Headers = columns.Select(c => c.HeaderText).ToArray(),
                Rows = grid.Rows.Cast<DataGridViewRow>()
                .Where(r => !r.IsNewRow)
                .Select(r => columns
                    .Select(c => Convert.ToString(r.Cells[c.Index].Value, CultureInfo.InvariantCulture) ?? "")
                    .ToArray())
                .ToList()
            };

            data.Rows = RemoveDuplicateExportRows(data.Rows);
            return data;
        }

        private static List<string[]> RemoveDuplicateExportRows(IEnumerable<string[]> rows)
        {
            HashSet<string> seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            List<string[]> uniqueRows = new List<string[]>();

            foreach (string[] row in rows)
            {
                string key = string.Join("\u001F", row.Select(value => NormalizeExportValue(value)));
                if (!seen.Add(key))
                    continue;

                uniqueRows.Add(row);
            }

            return uniqueRows;
        }

        private static string NormalizeExportValue(string value)
        {
            return string.Join(" ", (value ?? "").Trim().Split(new[] { ' ', '\r', '\n', '\t' },
                StringSplitOptions.RemoveEmptyEntries));
        }

        private static void WriteSimplePdf(string filePath, string title, GridExportData data)
        {
            int columnCount = Math.Max(1, data.Headers.Length);
            float pageLeft = 28f;
            float tableTop = 740f;
            float tableWidth = 556f;
            float colWidth = tableWidth / columnCount;
            float fontSize = columnCount <= 6 ? 7f : 6f;
            float lineHeight = fontSize + 2f;
            float bottom = 58f;
            string generated = "Generated: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

            List<string[]> headerLines = data.Headers
                .Select(h => WrapExportCell(h, colWidth, fontSize).ToArray())
                .ToList();
            float headerHeight = Math.Max(22f, headerLines.Max(lines => lines.Length) * lineHeight + 9f);

            List<string> pageStreams = new List<string>();
            int rowIndex = 0;

            do
            {
                StringBuilder content = new StringBuilder();
                float yTop = tableTop;
                int rowsOnPage = 0;

                AppendText(content, 40, 790, title, 16);
                AppendText(content, 40, 772, generated, 9);
                AppendText(content, 530, 772, "Page " + (pageStreams.Count + 1), 8);

                AppendRect(content, pageLeft, yTop - headerHeight, tableWidth, headerHeight, fillGray: 0.82f);
                AppendRowGrid(content, pageLeft, yTop, tableWidth, colWidth, columnCount, headerHeight);

                for (int c = 0; c < data.Headers.Length; c++)
                    AppendCellLines(content, headerLines[c], pageLeft + c * colWidth + 3, yTop - 10, fontSize, lineHeight);

                yTop -= headerHeight;

                while (rowIndex < data.Rows.Count)
                {
                    List<string[]> rowLines = new List<string[]>();
                    float rowHeight = 22f;
                    for (int c = 0; c < columnCount; c++)
                    {
                        string text = c < data.Rows[rowIndex].Length ? data.Rows[rowIndex][c] : "";
                        string[] lines = WrapExportCell(text, colWidth, fontSize).ToArray();
                        rowLines.Add(lines);
                        rowHeight = Math.Max(rowHeight, lines.Length * lineHeight + 9f);
                    }

                    if (yTop - rowHeight < bottom && rowsOnPage > 0)
                        break;

                    AppendRowGrid(content, pageLeft, yTop, tableWidth, colWidth, columnCount, rowHeight);
                    for (int c = 0; c < columnCount; c++)
                        AppendCellLines(content, rowLines[c], pageLeft + c * colWidth + 3, yTop - 10, fontSize, lineHeight);

                    yTop -= rowHeight;
                    rowIndex++;
                    rowsOnPage++;
                }

                pageStreams.Add(content.ToString());
            }
            while (rowIndex < data.Rows.Count);

            WritePdfPages(filePath, pageStreams);
        }

        private static void WriteResidentMedicalCertificatePdf(
            string filePath,
            GridExportData data,
            string residentName,
            string age,
            string barangay,
            string address,
            string email)
        {
            const float pageLeft = 42f;
            const float tableTop = 472f;
            const float tableWidth = 528f;
            const float bottom = 62f;

            int columnCount = Math.Max(1, data.Headers.Length);
            float colWidth = tableWidth / columnCount;
            float fontSize = columnCount <= 5 ? 7f : 6f;
            float lineHeight = fontSize + 2f;

            List<string[]> headerLines = data.Headers
                .Select(h => WrapExportCell(h, colWidth, fontSize).ToArray())
                .ToList();
            float headerHeight = Math.Max(22f, headerLines.Max(lines => lines.Length) * lineHeight + 9f);
            string generated = DateTime.Now.ToString("MMMM d, yyyy", CultureInfo.InvariantCulture);

            List<string> pageStreams = new List<string>();
            int rowIndex = 0;

            do
            {
                StringBuilder content = new StringBuilder();
                float yTop = tableTop;
                int rowsOnPage = 0;

                AppendText(content, 205, 790, "B-SMART HEALTH CENTER", 14);
                AppendText(content, 210, 772, "Barangay Health Management System", 9);
                AppendLine(content, 42, 755, 570, 755);
                AppendText(content, 205, 730, "MEDICAL CERTIFICATE", 16);

                AppendText(content, 54, 694, "This is to certify that the resident named below has health record data in B-SMART.", 9);
                AppendText(content, 54, 664, "Resident Name:", 9);
                AppendText(content, 170, 664, residentName, 9);
                AppendLine(content, 168, 660, 430, 660);
                AppendText(content, 446, 664, "Age:", 9);
                AppendText(content, 485, 664, age, 9);
                AppendLine(content, 482, 660, 560, 660);

                AppendText(content, 54, 634, "Barangay:", 9);
                AppendText(content, 170, 634, barangay, 9);
                AppendLine(content, 168, 630, 430, 630);
                AppendText(content, 54, 604, "Address:", 9);
                AppendWrappedText(content, address, 170, 604, 360, 9, 11, 2);
                AppendLine(content, 168, 600, 560, 600);
                AppendText(content, 54, 574, "Email:", 9);
                AppendText(content, 170, 574, email, 9);
                AppendLine(content, 168, 570, 560, 570);

                AppendText(content, 54, 535, "Health Record Summary", 11);
                AppendText(content, 466, 535, "Issued: " + generated, 8);

                AppendRect(content, pageLeft, yTop - headerHeight, tableWidth, headerHeight, fillGray: 0.82f);
                AppendRowGrid(content, pageLeft, yTop, tableWidth, colWidth, columnCount, headerHeight);
                for (int c = 0; c < data.Headers.Length; c++)
                    AppendCellLines(content, headerLines[c], pageLeft + c * colWidth + 3, yTop - 10, fontSize, lineHeight);
                yTop -= headerHeight;

                while (rowIndex < data.Rows.Count)
                {
                    List<string[]> rowLines = new List<string[]>();
                    float rowHeight = 22f;
                    for (int c = 0; c < columnCount; c++)
                    {
                        string text = c < data.Rows[rowIndex].Length ? data.Rows[rowIndex][c] : "";
                        string[] lines = WrapExportCell(text, colWidth, fontSize).ToArray();
                        rowLines.Add(lines);
                        rowHeight = Math.Max(rowHeight, lines.Length * lineHeight + 9f);
                    }

                    if (yTop - rowHeight < bottom && rowsOnPage > 0)
                        break;

                    AppendRowGrid(content, pageLeft, yTop, tableWidth, colWidth, columnCount, rowHeight);
                    for (int c = 0; c < columnCount; c++)
                        AppendCellLines(content, rowLines[c], pageLeft + c * colWidth + 3, yTop - 10, fontSize, lineHeight);

                    yTop -= rowHeight;
                    rowIndex++;
                    rowsOnPage++;
                }

                AppendText(content, 54, 34, "This certificate is generated from the resident's B-SMART health records.", 7);
                AppendText(content, 506, 34, "Page " + (pageStreams.Count + 1), 7);
                pageStreams.Add(content.ToString());
            }
            while (rowIndex < data.Rows.Count);

            WritePdfPages(filePath, pageStreams);
        }

        private static void AppendWrappedText(StringBuilder content, string text, float x, float y, float width, float fontSize, float lineHeight, int maxLines)
        {
            int chars = Math.Max(8, (int)(width / (fontSize * 0.52f)));
            string[] lines = WrapTextByChars(text, chars).Take(maxLines).ToArray();
            for (int i = 0; i < lines.Length; i++)
                AppendText(content, x, y - i * lineHeight, lines[i], fontSize);
        }

        private static IEnumerable<string> WrapTextByChars(string text, int maxChars)
        {
            text = (text ?? "").Replace("\r", " ").Replace("\n", " ").Trim();
            if (string.IsNullOrWhiteSpace(text)) return new[] { "" };

            List<string> lines = new List<string>();
            string current = "";
            foreach (string word in text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string candidate = string.IsNullOrWhiteSpace(current) ? word : current + " " + word;
                if (candidate.Length > maxChars)
                {
                    if (!string.IsNullOrWhiteSpace(current))
                        lines.Add(current);
                    current = word;
                }
                else
                {
                    current = candidate;
                }
            }
            if (!string.IsNullOrWhiteSpace(current)) lines.Add(current);
            return lines.Count == 0 ? new[] { "" } : lines;
        }

        private static void WritePdfPages(string filePath, List<string> pageStreams)
        {
            using FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            using StreamWriter writer = new StreamWriter(fs, Encoding.ASCII, 1024, true) { NewLine = "\n" };

            int pageCount = Math.Max(1, pageStreams.Count);
            int firstPageObject = 4;
            int firstContentObject = firstPageObject + pageCount;
            int objectCount = firstContentObject + pageCount - 1;
            long[] offsets = new long[objectCount + 1];

            writer.Write("%PDF-1.4\n");
            writer.Flush();
            offsets[1] = fs.Position;
            writer.Write("1 0 obj << /Type /Catalog /Pages 2 0 R >> endobj\n");
            writer.Flush();

            offsets[2] = fs.Position;
            string kids = string.Join(" ", Enumerable.Range(firstPageObject, pageCount).Select(i => $"{i} 0 R"));
            writer.Write($"2 0 obj << /Type /Pages /Kids [{kids}] /Count {pageCount} >> endobj\n");
            writer.Flush();

            offsets[3] = fs.Position;
            writer.Write("3 0 obj << /Type /Font /Subtype /Type1 /BaseFont /Helvetica >> endobj\n");
            writer.Flush();

            for (int i = 0; i < pageCount; i++)
            {
                int pageObj = firstPageObject + i;
                int contentObj = firstContentObject + i;
                offsets[pageObj] = fs.Position;
                writer.Write($"{pageObj} 0 obj << /Type /Page /Parent 2 0 R /MediaBox [0 0 612 842] /Resources << /Font << /F1 3 0 R >> >> /Contents {contentObj} 0 R >> endobj\n");
                writer.Flush();
            }

            for (int i = 0; i < pageCount; i++)
            {
                int contentObj = firstContentObject + i;
                byte[] streamBytes = Encoding.ASCII.GetBytes(pageStreams[i]);
                offsets[contentObj] = fs.Position;
                writer.Write($"{contentObj} 0 obj << /Length {streamBytes.Length} >> stream\n");
                writer.Flush();
                fs.Write(streamBytes, 0, streamBytes.Length);
                writer.Write("\nendstream endobj\n");
                writer.Flush();
            }

            long xref = fs.Position;
            writer.Write($"xref\n0 {objectCount + 1}\n0000000000 65535 f \n");
            for (int i = 1; i <= objectCount; i++)
                writer.Write($"{offsets[i]:0000000000} 00000 n \n");
            writer.Write($"trailer << /Size {objectCount + 1} /Root 1 0 R >>\nstartxref\n{xref}\n%%EOF");
        }

        private static string EscapePdf(string text)
        {
            return (text ?? "").Replace("\\", "\\\\").Replace("(", "\\(").Replace(")", "\\)");
        }

        private static void AppendCellLines(StringBuilder content, string[] lines, float x, float firstLineY, float fontSize, float lineHeight)
        {
            for (int i = 0; i < lines.Length; i++)
                AppendText(content, x, firstLineY - i * lineHeight, lines[i], fontSize);
        }

        private static IEnumerable<string> WrapExportCell(string text, float colWidth, float fontSize)
        {
            text = (text ?? "").Replace("\r", " ").Replace("\n", " ").Trim();
            if (string.IsNullOrWhiteSpace(text))
                return new[] { "" };

            int maxChars = Math.Max(4, (int)((colWidth - 6) / (fontSize * 0.52f)));
            List<string> lines = new List<string>();
            string current = "";

            foreach (string rawWord in text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string word = rawWord;
                while (word.Length > maxChars)
                {
                    if (!string.IsNullOrWhiteSpace(current))
                    {
                        lines.Add(current);
                        current = "";
                    }
                    lines.Add(word.Substring(0, maxChars));
                    word = word.Substring(maxChars);
                }

                string candidate = string.IsNullOrWhiteSpace(current) ? word : current + " " + word;
                if (candidate.Length > maxChars)
                {
                    lines.Add(current);
                    current = word;
                }
                else
                {
                    current = candidate;
                }
            }

            if (!string.IsNullOrWhiteSpace(current))
                lines.Add(current);

            return lines.Count == 0 ? new[] { "" } : lines;
        }

        private static void AppendText(StringBuilder content, float x, float y, string text, float fontSize)
        {
            content.AppendLine("BT");
            content.AppendLine($"/F1 {fontSize.ToString(CultureInfo.InvariantCulture)} Tf");
            content.AppendLine($"{x.ToString(CultureInfo.InvariantCulture)} {y.ToString(CultureInfo.InvariantCulture)} Td");
            content.AppendLine($"({EscapePdf(text)}) Tj");
            content.AppendLine("ET");
        }

        private static void AppendRowGrid(StringBuilder content, float left, float top, float tableWidth, float colWidth, int columnCount, float rowHeight)
        {
            for (int i = 0; i <= columnCount; i++)
                AppendLine(content, left + i * colWidth, top, left + i * colWidth, top - rowHeight);
            AppendLine(content, left, top, left + tableWidth, top);
            AppendLine(content, left, top - rowHeight, left + tableWidth, top - rowHeight);
        }

        private static void AppendRect(StringBuilder content, float x, float y, float width, float height, float fillGray)
        {
            content.AppendLine("q");
            content.AppendLine($"{fillGray.ToString(CultureInfo.InvariantCulture)} g");
            content.AppendLine($"{x.ToString(CultureInfo.InvariantCulture)} {y.ToString(CultureInfo.InvariantCulture)} {width.ToString(CultureInfo.InvariantCulture)} {height.ToString(CultureInfo.InvariantCulture)} re f");
            content.AppendLine("Q");
        }

        private static void AppendLine(StringBuilder content, float x1, float y1, float x2, float y2)
        {
            content.AppendLine($"{x1.ToString(CultureInfo.InvariantCulture)} {y1.ToString(CultureInfo.InvariantCulture)} m {x2.ToString(CultureInfo.InvariantCulture)} {y2.ToString(CultureInfo.InvariantCulture)} l S");
        }

        private static string SanitizeFileName(string text)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
                text = text.Replace(c, '-');
            return text;
        }
    }
}
