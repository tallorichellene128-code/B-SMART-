using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BSMART
{
    public partial class TayudBCapManageResAcc : Form
    {
        private string connectionString = "Server=localhost;Database=bsmart_db;Uid=root;Pwd=;";
        private int selectedUserID = 0;
        private TextBox _txtMiddlename;
        private Label _lblMiddleName;

        public TayudBCapManageResAcc()
        {
            InitializeComponent();
            EnsureMiddleNameControl();
        }

        private void EnsureMiddleNameControl()
        {
            if (_txtMiddlename != null) return;
            Control[] existingControls = Controls.Find("txtMiddlename", true);
            if (existingControls.Length == 0)
                existingControls = Controls.Find("txtMiddleName", true);
            if (existingControls.Length > 0 && existingControls[0] is TextBox existingTextBox)
            {
                _txtMiddlename = existingTextBox;
                return;
            }

            txtFirstName.Size = new Size(260, txtFirstName.Height);
            txtLastName.Location = new Point(1058, txtLastName.Location.Y);
            txtLastName.Size = new Size(260, txtLastName.Height);
            label5.Location = new Point(938, label5.Location.Y);

            _lblMiddleName = new Label
            {
                Text = "Middle Name",
                AutoSize = true,
                Font = label4.Font,
                Location = new Point(768, label4.Location.Y)
            };

            _txtMiddlename = new TextBox
            {
                Name = "txtMiddlename",
                Font = txtFirstName.Font,
                Location = new Point(768, txtFirstName.Location.Y),
                Size = new Size(260, txtFirstName.Height)
            };

            Controls.Add(_lblMiddleName);
            Controls.Add(_txtMiddlename);
            _txtMiddlename.BringToFront();
            _lblMiddleName.BringToFront();
        }

        private void TayudBCapManageResAcc_Load(object sender, EventArgs e)
        {
            cmbAge.Items.Clear();
            for (int i = 1; i <= 120; i++) cmbAge.Items.Add(i.ToString());
            cmbGender.Items.Clear();
            cmbGender.Items.Add("Male");
            cmbGender.Items.Add("Female");

            // Lock barangay to captain's own barangay
            cmbBarangay.Items.Clear();
            cmbBarangay.Items.Add(Session.BarangayName);
            cmbBarangay.SelectedIndex = 0;
            cmbBarangay.Enabled = false;

            SetButtonState(false);
            LoadAccounts();

            // Wire CellClick in code so designer binding isn't needed
            dgvResAccRec.CellClick -= dgvResAccRec_CellClick;
            dgvResAccRec.CellClick += dgvResAccRec_CellClick;
        }

        // =============================================
        // LOAD ACTIVE RESIDENT ACCOUNTS
        // =============================================
        private void LoadAccounts(string keyword = "")
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    BsmartIdHelper.EnsureCodes(conn);
                    MySqlCommand cmd = new MySqlCommand("", conn);
                    string violationExists = BsmartResidentViolationService.ExistsSql("u");

                    string keyFilter = "";
                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        keyFilter = $@" AND (COALESCE(u.resident_code, CAST(u.id AS CHAR)) LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR  u.first_name LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR  COALESCE(u.middle_name, '') LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR  u.last_name LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR  u.full_name LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR  u.gender LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR  CAST(u.age AS CHAR) LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR  u.email LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR  u.mobile_number LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR  u.address LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR  (CASE WHEN COALESCE(u.is_frozen, 0) = 1 AND {violationExists} THEN 'Frozen' ELSE 'Active' END) LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR  DATE_FORMAT(u.birthday, '%d/%m/%Y') LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR  DATE_FORMAT(u.birthday, '%m/%d/%Y') LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR  DATE_FORMAT(u.birthday, '%Y-%m-%d') LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR  EXISTS (
                                           SELECT 1 FROM health_records h
                                           WHERE COALESCE(h.is_archived, 0) = 0
                                             AND h.violation IS NOT NULL
                                             AND TRIM(h.violation) <> ''
                                             AND h.violation LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                             AND LOWER(TRIM(h.first_name)) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(COALESCE(u.first_name, ''))) COLLATE utf8mb4_unicode_ci
                                             AND LOWER(TRIM(COALESCE(h.middle_name, ''))) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(COALESCE(u.middle_name, ''))) COLLATE utf8mb4_unicode_ci
                                             AND LOWER(TRIM(h.last_name)) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(COALESCE(u.last_name, ''))) COLLATE utf8mb4_unicode_ci
                                             AND (u.birthday IS NULL OR h.birthday IS NULL OR DATE(h.birthday) = DATE(u.birthday))
                                             AND (h.barangay_id = u.barangay_id OR h.barangay_id = @brgyId)
                                       ))";
                        cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");
                    }

                    cmd.CommandText = $@"SELECT
                                            u.id AS _DBID,
                                            COALESCE((
                                                SELECT h2.resident_code
                                                FROM health_records h2
                                                WHERE COALESCE(h2.is_archived, 0) = 0
                                                  AND h2.resident_code IS NOT NULL
                                                  AND h2.resident_code <> ''
                                                  AND h2.barangay_id = u.barangay_id
                                                  AND LOWER(TRIM(h2.first_name)) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(COALESCE(u.first_name, ''))) COLLATE utf8mb4_unicode_ci
                                                  AND LOWER(TRIM(COALESCE(h2.middle_name, ''))) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(COALESCE(u.middle_name, ''))) COLLATE utf8mb4_unicode_ci
                                                  AND LOWER(TRIM(h2.last_name)) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(COALESCE(u.last_name, ''))) COLLATE utf8mb4_unicode_ci
                                                  AND (u.birthday IS NULL OR h2.birthday IS NULL OR DATE(h2.birthday) = DATE(u.birthday))
                                                ORDER BY h2.ID ASC
                                                LIMIT 1
                                            ), u.resident_code, CAST(u.id AS CHAR)) AS ID,
                                            COALESCE(u.first_name, '') AS FirstName,
                                            COALESCE(u.middle_name, '') AS MiddleName,
                                            COALESCE(u.last_name, '') AS LastName,
                                            COALESCE(NULLIF(TRIM(u.full_name), ''),
                                                     NULLIF(TRIM(CONCAT(COALESCE(u.first_name, ''), ' ', COALESCE(u.middle_name, ''), ' ', COALESCE(u.last_name, ''))), ''),
                                                     u.username) AS Resident,
                                            COALESCE(u.gender, '') AS Gender,
                                            COALESCE(u.age, '') AS Age,
                                            u.birthday AS Birthday,
                                            COALESCE(u.email, '') AS Email,
                                            CASE WHEN COALESCE(u.is_frozen, 0) = 1 AND {violationExists} THEN 'Frozen' ELSE 'Active' END AS Status,
                                            COALESCE((
                                                SELECT GROUP_CONCAT(DISTINCT TRIM(h.violation) ORDER BY h.violation SEPARATOR ', ')
                                                FROM health_records h
                                                WHERE COALESCE(h.is_archived, 0) = 0
                                                  AND h.violation IS NOT NULL
                                                  AND TRIM(h.violation) <> ''
                                                  AND LOWER(TRIM(h.first_name)) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(COALESCE(u.first_name, ''))) COLLATE utf8mb4_unicode_ci
                                                  AND LOWER(TRIM(COALESCE(h.middle_name, ''))) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(COALESCE(u.middle_name, ''))) COLLATE utf8mb4_unicode_ci
                                                  AND LOWER(TRIM(h.last_name)) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(COALESCE(u.last_name, ''))) COLLATE utf8mb4_unicode_ci
                                                  AND (u.birthday IS NULL OR h.birthday IS NULL OR DATE(h.birthday) = DATE(u.birthday))
                                                  AND (h.barangay_id = u.barangay_id OR h.barangay_id = @brgyId)
                                            ), '') AS Violation
                                         FROM users u
                                         LEFT JOIN barangays b ON b.id = u.barangay_id
                                         WHERE LOWER(TRIM(u.role)) = 'resident'
                                           AND COALESCE(u.is_archived, 0) = 0
                                           AND (u.barangay_id = @brgyId
                                             OR LOWER(TRIM(COALESCE(b.name, ''))) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(@brgyName)) COLLATE utf8mb4_unicode_ci){keyFilter}
                                         ORDER BY Resident ASC";
                    cmd.Parameters.AddWithValue("@brgyId", Session.BarangayID);
                    cmd.Parameters.AddWithValue("@brgyName", Session.BarangayName ?? "");

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvResAccRec.DataSource = dt;
                    StyleGrid();
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                dgvResAccRec.DataSource = null;
                MessageBox.Show("Error loading resident accounts: " + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void StyleGrid()
        {
            dgvResAccRec.ReadOnly = true;
            dgvResAccRec.AllowUserToAddRows = false;
            dgvResAccRec.AllowUserToDeleteRows = false;
            dgvResAccRec.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            if (dgvResAccRec.Columns.Contains("_DBID"))
                dgvResAccRec.Columns["_DBID"].Visible = false;
            if (dgvResAccRec.Columns.Contains("FirstName"))
                dgvResAccRec.Columns["FirstName"].Visible = false;
            if (dgvResAccRec.Columns.Contains("MiddleName"))
                dgvResAccRec.Columns["MiddleName"].Visible = false;
            if (dgvResAccRec.Columns.Contains("LastName"))
                dgvResAccRec.Columns["LastName"].Visible = false;
            dgvResAccRec.BackgroundColor = Color.White;
            dgvResAccRec.BorderStyle = BorderStyle.None;
            dgvResAccRec.RowHeadersVisible = false;
            dgvResAccRec.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvResAccRec.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 102, 204);
            dgvResAccRec.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvResAccRec.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 9, FontStyle.Bold);
            dgvResAccRec.ColumnHeadersHeight = 35;
            dgvResAccRec.EnableHeadersVisualStyles = false;
            dgvResAccRec.DefaultCellStyle.Font = new Font("Arial", 9);
            dgvResAccRec.RowTemplate.Height = 30;
            dgvResAccRec.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(220, 235, 255);

            if (dgvResAccRec.Columns.Contains("ID"))
                dgvResAccRec.Columns["ID"].FillWeight = 35;
            if (dgvResAccRec.Columns.Contains("Resident"))
                dgvResAccRec.Columns["Resident"].FillWeight = 120;
            if (dgvResAccRec.Columns.Contains("Status"))
                dgvResAccRec.Columns["Status"].FillWeight = 65;
            if (dgvResAccRec.Columns.Contains("Violation"))
                dgvResAccRec.Columns["Violation"].FillWeight = 130;
        }

        // =============================================
        // ROW CLICK ? populate fields
        // =============================================
        private void dgvResAccRec_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvResAccRec.Rows[e.RowIndex];

            string idText = dgvResAccRec.Columns.Contains("_DBID")
                ? row.Cells["_DBID"].Value?.ToString() ?? ""
                : row.Cells["ID"].Value?.ToString() ?? "";
            bool hasAccount = int.TryParse(idText, out selectedUserID);
            txtFirstName.Text = dgvResAccRec.Columns.Contains("FirstName")
                ? row.Cells["FirstName"].Value?.ToString() ?? ""
                : "";
            _txtMiddlename.Text = dgvResAccRec.Columns.Contains("MiddleName")
                ? row.Cells["MiddleName"].Value?.ToString() ?? ""
                : "";
            txtLastName.Text = dgvResAccRec.Columns.Contains("LastName")
                ? row.Cells["LastName"].Value?.ToString() ?? ""
                : "";
            cmbAge.SelectedItem = row.Cells["Age"].Value.ToString();
            cmbGender.SelectedItem = row.Cells["Gender"].Value.ToString();
            txtEmailAdd.Text = row.Cells["Email"].Value?.ToString() ?? "";
            SetButtonState(hasAccount);
        }

        private void dgvResAccRec_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void SetButtonState(bool enabled)
        {
            btnArchiveAcc.Enabled = enabled;
            btnFreezeAcc.Enabled = enabled;
            btnRemoveAcc.Enabled = enabled;
        }

        private void ClearFields()
        {
            selectedUserID = 0;
            txtFirstName.Clear(); _txtMiddlename.Clear(); txtLastName.Clear();
            cmbAge.SelectedIndex = -1; cmbGender.SelectedIndex = -1;
            txtEmailAdd.Clear();
            SetButtonState(false);
        }
private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadAccounts(txtSearch.Text.Trim());
        }

        // =============================================
        // ARCHIVE ACCOUNT ? moves to ViewArch2
        // =============================================
        private void btnArchiveAcc_Click(object sender, EventArgs e)
        {
            if (selectedUserID == 0) return;
            if (MessageBox.Show("Archive this account?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "UPDATE users SET is_archived = 1 WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", selectedUserID);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Account archived!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadAccounts();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =============================================
        // FREEZE ACCOUNT ? moves to ManageFAcc
        // =============================================
        private void btnFreezeAcc_Click(object sender, EventArgs e)
        {
            if (selectedUserID == 0) return;
            if (MessageBox.Show("Freeze this account?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    if (!BsmartResidentViolationService.HasViolation(conn, selectedUserID))
                    {
                        MessageBox.Show("This resident has no violation, so the account cannot be frozen.",
                            "No Violation Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    MySqlCommand cmd = new MySqlCommand(
                        "UPDATE users SET is_frozen = 1 WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", selectedUserID);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Account frozen!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadAccounts();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =============================================
        // REMOVE ACCOUNT ? permanent delete
        // =============================================
        private void btnRemoveAcc_Click(object sender, EventArgs e)
        {
            if (selectedUserID == 0) return;
            if (MessageBox.Show("PERMANENTLY remove this account?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "DELETE FROM users WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", selectedUserID);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Account removed!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadAccounts();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =============================================
        // NAVIGATION
        // =============================================
        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Logout?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            { Session.Clear(); BsmartFormNavigator.Open(this, new loginBCap()); }
        }

        private void btnBCapDash_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudBCapDash()); }

        private void btnResManageRecord_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudBCManageRes()); }

        private void btnResViewRecord_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudBCViewRes()); }

        private void btnManageResAcc_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudBCapManageResAcc()); }

        private void btnViewResAcc_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudBCapViewResAcc()); }

        private void btnManageFrozenAcc_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudBCapManageFAcc()); }

        private void btnHealthRep_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudBCViewResHealthRep()); }

        private void btnViewArchive_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudBCapViewArch()); }

        private void txtFirstName_TextChanged(object sender, EventArgs e) { }
        private void txtMiddleName_TextChanged(object sender, EventArgs e) { }
        private void _txtMiddlename_TextChanged(object sender, EventArgs e) { }
        private void txtLastName_TextChanged(object sender, EventArgs e) { }
        private void cmbAge_SelectedIndexChanged(object sender, EventArgs e) { }
        private void cmbGender_SelectedIndexChanged(object sender, EventArgs e) { }
        private void txtEmailAdd_TextChanged(object sender, EventArgs e) { }
        private void cmbBarangay_SelectedIndexChanged(object sender, EventArgs e) { }

        private void txtMiddleName_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}
