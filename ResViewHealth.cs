using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BSMART
{
    public partial class ResViewHealth : Form
    {
        private string connectionString = "Server=localhost;Database=bsmart_db;Uid=root;Pwd=;";

        public ResViewHealth()
        {
            InitializeComponent();
            ResidentProfileUiService.Apply(PResProfile, lblResidentName, lblAge, lblAddress, lblBarangay, lblEmail);
            BsmartNotificationService.Attach(btnNotification, this);
            btnDownloadResRec.Click += (s, e) => DownloadResidentMedicalCertificate();
        }

        // -- ResViewHealth_Load is the ONLY Load handler ------------------
        // If your designer still points to ResViewHealth_Load_1,
        // open ResViewHealth.Designer.cs and change:
        //   this.Load += ResViewHealth_Load_1;
        // to:
        //   this.Load += ResViewHealth_Load;
        // -----------------------------------------------------------------
        private void ResViewHealth_Load(object sender, EventArgs e)
        {
            LoadProfile();
            LoadMyHealthRecords();
        }

        // Keep this stub so the designer doesn't error if it's still bound
        private void ResViewHealth_Load_1(object sender, EventArgs e)
        {
            LoadProfile();
            LoadMyHealthRecords();
        }

        // =============================================
        // LOAD PROFILE — fills all 5 labels
        // =============================================
        private void LoadProfile()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(@"
                        SELECT u.first_name, u.last_name, u.age, u.gender, u.birthday,
                               u.address,   u.email, u.mobile_number,
                               COALESCE(b.name, 'N/A') AS barangay_name
                        FROM   users u
                        LEFT JOIN barangays b ON b.id = u.barangay_id
                        WHERE  u.id = @uid", conn);
                    cmd.Parameters.AddWithValue("@uid", Session.UserID);

                    MySqlDataReader r = cmd.ExecuteReader();
                    if (r.Read())
                    {
                        ResidentProfileUiService.SetValues(
                            lblResidentName,
                            lblAge,
                            lblAddress,
                            lblBarangay,
                            lblEmail,
                            $"{ResidentProfileUiService.Value(r["first_name"], "")} {ResidentProfileUiService.Value(r["last_name"], "")}".Trim(),
                            ResidentProfileUiService.Value(r["age"]),
                            ResidentProfileUiService.Value(r["gender"]),
                            ResidentProfileUiService.DateValue(r["birthday"]),
                            ResidentProfileUiService.Value(r["address"]),
                            ResidentProfileUiService.Value(r["barangay_name"]),
                            ResidentProfileUiService.Value(r["email"]),
                            ResidentProfileUiService.Value(r["mobile_number"]));
                    }
                }
            }
            catch { }
        }

        // =============================================
        // LOAD HEALTH RECORDS
        // Matches health_records to the logged-in resident
        // by verifying: first_name + last_name + barangay_id
        // + age + gender + birthday (all must match).
        // This ensures only their own records show — not
        // someone else with the same name.
        // =============================================
        private void LoadMyHealthRecords()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    BsmartIdHelper.EnsureCodes(conn);

                    // Step 1: Get full personal info from users table
                    MySqlCommand infoCmd = new MySqlCommand(@"
                        SELECT first_name, last_name, age, gender, birthday
                        FROM   users
                        WHERE  id = @uid", conn);
                    infoCmd.Parameters.AddWithValue("@uid", Session.UserID);

                    MySqlDataReader info = infoCmd.ExecuteReader();
                    string fn = "", ln = "", gender = "";
                    int age = 0;
                    DateTime birthday = DateTime.MinValue;

                    if (info.Read())
                    {
                        fn = info["first_name"].ToString();
                        ln = info["last_name"].ToString();
                        gender = info["gender"] == DBNull.Value ? "" : info["gender"].ToString();
                        age = info["age"] == DBNull.Value ? 0 : Convert.ToInt32(info["age"]);

                        if (info["birthday"] != DBNull.Value)
                            birthday = Convert.ToDateTime(info["birthday"]);
                    }
                    info.Close();

                    // Step 2: Query health_records with full identity verification
                    // birthday match is only added if the resident has it on file
                    string birthdayFilter = birthday != DateTime.MinValue
                        ? " AND birthday = @birthday"
                        : "";

                    // gender filter only if gender is recorded
                    string genderFilter = !string.IsNullOrEmpty(gender)
                        ? " AND gender = @gender"
                        : "";

                    MySqlCommand cmd = new MySqlCommand($@"
                        SELECT
                            COALESCE(resident_code, CAST(ID AS CHAR)) AS ID,
                            CASE WHEN UPPER(TRIM(COALESCE(Diagnosis, ''))) = 'N/A' THEN '' ELSE COALESCE(Diagnosis, '') END AS Diagnosis,
                            CASE WHEN UPPER(TRIM(COALESCE(Diagnosis, ''))) = 'N/A' THEN '' ELSE COALESCE(Treatment, '') END AS Treatment,
                            assigned_doc_name AS 'Assigned Doctor/Nurse',
                            Date
                        FROM health_records
                        WHERE is_archived  = 0
                          AND Diagnosis IS NOT NULL
                          AND TRIM(Diagnosis) <> ''
                          AND UPPER(TRIM(Diagnosis)) <> 'N/A'
                          AND barangay_id  = @brgyId
                          AND first_name   = @fn
                          AND last_name    = @ln
                          AND age          = @age
                          {genderFilter}
                          {birthdayFilter}
                        ORDER BY Date DESC", conn);

                    cmd.Parameters.AddWithValue("@brgyId", Session.BarangayID);
                    cmd.Parameters.AddWithValue("@fn", fn);
                    cmd.Parameters.AddWithValue("@ln", ln);
                    cmd.Parameters.AddWithValue("@age", age);

                    if (!string.IsNullOrEmpty(gender))
                        cmd.Parameters.AddWithValue("@gender", gender);
                    if (birthday != DateTime.MinValue)
                        cmd.Parameters.AddWithValue("@birthday", birthday.Date);

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvResHealthRec.DataSource = dt;
                    StyleGrid();

                    // Inform resident if no matching records found
                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show(
                            "No health records found for your account.\n" +
                            "Records will appear here once your barangay health staff adds them.",
                            "No Records", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading records: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StyleGrid()
        {
            BsmartUiService.StyleGrid(dgvResHealthRec);
        }

        private void DownloadResidentMedicalCertificate()
        {
            List<DataGridViewRow> rows = GetHealthRecordRows();
            if (rows.Count == 0)
            {
                MessageBox.Show("There are no records to download.", "Download",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataGridViewRow selectedRow = rows.Count == 1
                ? rows[0]
                : ShowHealthRecordPicker(rows);

            if (selectedRow == null) return;

            using DataGridView singleRecordGrid = BuildSingleRecordGrid(selectedRow);
            MayorFormHelper.ExportResidentMedicalCertificate(singleRecordGrid,
                lblResidentName.Text, lblAge.Text, lblBarangay.Text, lblAddress.Text, lblEmail.Text);
        }

        private List<DataGridViewRow> GetHealthRecordRows()
        {
            List<DataGridViewRow> rows = new List<DataGridViewRow>();
            foreach (DataGridViewRow row in dgvResHealthRec.Rows)
            {
                if (!row.IsNewRow)
                    rows.Add(row);
            }

            return rows;
        }

        private DataGridViewRow ShowHealthRecordPicker(List<DataGridViewRow> rows)
        {
            using Form picker = new Form
            {
                Text = "Select Health Record",
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                ClientSize = new Size(540, 360),
                BackColor = Color.White
            };

            Label instruction = new Label
            {
                Text = "Choose the health record to include in your medical certificate:",
                Location = new Point(18, 18),
                Size = new Size(500, 30),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            ListBox list = new ListBox
            {
                Location = new Point(18, 56),
                Size = new Size(504, 230),
                Font = new Font("Segoe UI", 10),
                IntegralHeight = false
            };

            foreach (DataGridViewRow row in rows)
                list.Items.Add(new HealthRecordChoice(row));

            if (list.Items.Count > 0)
                list.SelectedIndex = 0;

            Button btnOk = new Button
            {
                Text = "Download",
                Location = new Point(302, 306),
                Size = new Size(104, 34),
                DialogResult = DialogResult.OK
            };

            Button btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(418, 306),
                Size = new Size(104, 34),
                DialogResult = DialogResult.Cancel
            };

            picker.Controls.Add(instruction);
            picker.Controls.Add(list);
            picker.Controls.Add(btnOk);
            picker.Controls.Add(btnCancel);
            picker.AcceptButton = btnOk;
            picker.CancelButton = btnCancel;

            if (picker.ShowDialog(this) != DialogResult.OK || list.SelectedItem == null)
                return null;

            return ((HealthRecordChoice)list.SelectedItem).Row;
        }

        private DataGridView BuildSingleRecordGrid(DataGridViewRow selectedRow)
        {
            DataGridView grid = new DataGridView
            {
                AllowUserToAddRows = false,
                AutoGenerateColumns = false
            };

            foreach (DataGridViewColumn sourceColumn in dgvResHealthRec.Columns)
            {
                if (!sourceColumn.Visible) continue;

                grid.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = sourceColumn.Name,
                    HeaderText = sourceColumn.HeaderText,
                    ValueType = typeof(string)
                });
            }

            int rowIndex = grid.Rows.Add();
            DataGridViewRow targetRow = grid.Rows[rowIndex];
            int targetColumnIndex = 0;
            foreach (DataGridViewColumn sourceColumn in dgvResHealthRec.Columns)
            {
                if (!sourceColumn.Visible) continue;

                object value = selectedRow.Cells[sourceColumn.Index].Value;
                targetRow.Cells[targetColumnIndex].Value = FormatCertificateCell(sourceColumn.HeaderText, value);
                targetColumnIndex++;
            }

            return grid;
        }

        private static string FormatCertificateCell(string headerText, object value)
        {
            if (value == null || value == DBNull.Value) return "";

            string text = Convert.ToString(value)?.Trim() ?? "";
            if (!string.Equals((headerText ?? "").Trim(), "Date", StringComparison.OrdinalIgnoreCase))
                return text;

            if (value is DateTime date)
                return date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            return DateTime.TryParse(text, out DateTime parsed)
                ? parsed.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
                : text;
        }

        private sealed class HealthRecordChoice
        {
            public DataGridViewRow Row { get; }

            public HealthRecordChoice(DataGridViewRow row)
            {
                Row = row;
            }

            public override string ToString()
            {
                string id = CellText("ID");
                string diagnosis = CellText("Diagnosis");
                string treatment = CellText("Treatment");
                string date = CellText("Date");

                if (string.IsNullOrWhiteSpace(diagnosis))
                    diagnosis = "No diagnosis";
                if (string.IsNullOrWhiteSpace(treatment))
                    treatment = "No treatment";

                return $"{date}  |  {id}  |  {diagnosis}  |  {treatment}";
            }

            private string CellText(string columnName)
            {
                if (!Row.DataGridView.Columns.Contains(columnName))
                    return "";

                object value = Row.Cells[columnName].Value;
                if (value == null || value == DBNull.Value)
                    return "";

                string text = Convert.ToString(value)?.Trim() ?? "";
                if (string.Equals(columnName, "Date", StringComparison.OrdinalIgnoreCase)
                    && DateTime.TryParse(text, out DateTime parsed))
                    return parsed.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                return text;
            }
        }
        // =============================================
        // NAVIGATION
        // =============================================
        private void btnResidentDash_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new ResidentDash()); }

        private void btnResViewHealth_Click(object sender, EventArgs e)
        { LoadProfile(); LoadMyHealthRecords(); }

        private void btnServicesAppointment_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new ResidentAppointment()); }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Logout?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            { Session.Clear(); BsmartFormNavigator.Open(this, new loginRes()); }
        }

        private void dgvResHealthRec_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void PResProfile_Paint(object sender, PaintEventArgs e) { }
        private void lblResidentName_Click(object sender, EventArgs e) { }
        private void lblAge_Click(object sender, EventArgs e) { }
        private void lblAddress_Click(object sender, EventArgs e) { }
        private void lblBarangay_Click(object sender, EventArgs e) { }
        private void lblEmail_Click(object sender, EventArgs e) { }
    }
}
