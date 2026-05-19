// ============================================================
//  B-SMART : TayudViewArchive.cs
//  Shows ARCHIVED health records scoped to Session.BarangayID.
//  Supports: search, click-to-load, restore, and permanent delete.
//  btnNext ? TayudViewArchive2 (medicine/vaccine archive).
// ============================================================
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BSMART
{
    public partial class TayudViewArchive : Form
    {
        private string connectionString = "Server=localhost;Database=bsmart_db;Uid=root;Pwd=;";
        private int selectedHealthID = 0;

        public TayudViewArchive()
        {
            InitializeComponent();
            btnLogout.Click += (s, e) => SettingsNavigationService.OpenLogin(this);
        }

        // =============================================
        // FORM LOAD
        // =============================================
        private void TayudViewArchive_Load(object sender, EventArgs e)
        {
            // Populate age combo
            cmbAge.Items.Clear();
            for (int i = 1; i <= 120; i++) cmbAge.Items.Add(i.ToString());

            cmbGender.Items.Clear();
            cmbGender.Items.Add("Male");
            cmbGender.Items.Add("Female");

            SetButtonState(false);
            LoadArchivedRecords();
        }

        // =============================================
        // BARANGAY FILTER HELPER
        // =============================================
        private string BarangayFilter(MySqlCommand cmd)
        {
            if (Session.IsMayor) return "";
            cmd.Parameters.AddWithValue("@brgyId", Session.BarangayID);
            cmd.Parameters.AddWithValue("@brgyName", Session.BarangayName);
            return " AND (barangay_id = @brgyId OR barangay_id IN (SELECT id FROM barangays WHERE name = @brgyName))";
        }

        // =============================================
        // LOAD ARCHIVED HEALTH RECORDS
        // =============================================
        private void LoadArchivedRecords(string keyword = "")
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    BsmartIdHelper.EnsureCodes(conn);
                    MySqlCommand cmd = new MySqlCommand("", conn);
                    string filter = BarangayFilter(cmd);

                    string keyFilter = "";
                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        keyFilter = @" AND (first_name LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                        OR last_name  LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                        OR CONCAT(first_name,' ',last_name) LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                        OR Diagnosis  LIKE @kw COLLATE utf8mb4_0900_ai_ci)";
                        cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");
                    }

                    cmd.CommandText = $@"SELECT
                                            ID                                     AS '_DBID',
                                        COALESCE(resident_code, CAST(ID AS CHAR)) AS 'ID',
                                            CONCAT(first_name,' ',last_name)  AS 'Resident',
                                            gender                            AS 'Gender',
                                            age                               AS 'Age',
                                            birthday                          AS 'Birthday',
                                            Diagnosis                         AS 'Diagnosis',
                                            Treatment                         AS 'Treatment',
                                            Date                              AS 'Record Date'
                                         FROM health_records
                                         WHERE is_archived = 1{filter}{keyFilter}
                                         ORDER BY Date DESC";

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvArchHealthRec.DataSource = dt;
                    StyleGrid();
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading archived records: " + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =============================================
        // STYLE GRID
        // =============================================
        private void StyleGrid()
        {
            dgvArchHealthRec.ReadOnly = true;
            dgvArchHealthRec.AllowUserToAddRows = false;
            dgvArchHealthRec.AllowUserToDeleteRows = false;
            dgvArchHealthRec.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvArchHealthRec.BackgroundColor = Color.White;
            dgvArchHealthRec.BorderStyle = BorderStyle.None;
            dgvArchHealthRec.RowHeadersVisible = false;
            dgvArchHealthRec.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvArchHealthRec.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 102, 204);
            dgvArchHealthRec.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvArchHealthRec.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 9, FontStyle.Bold);
            dgvArchHealthRec.ColumnHeadersHeight = 35;
            dgvArchHealthRec.EnableHeadersVisualStyles = false;
            dgvArchHealthRec.DefaultCellStyle.Font = new Font("Arial", 9);
            dgvArchHealthRec.RowTemplate.Height = 30;
            dgvArchHealthRec.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(220, 235, 255);
            if (dgvArchHealthRec.Columns.Contains("_DBID"))
                dgvArchHealthRec.Columns["_DBID"].Visible = false;
        }

        // =============================================
        // GRID ROW CLICK ? populate fields
        // =============================================
        private void dgvArchHealthRec_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvArchHealthRec.Rows[e.RowIndex];

            selectedHealthID = Convert.ToInt32(row.Cells["_DBID"].Value);
            txtFirstName.Text = row.Cells["Resident"].Value.ToString().Split(' ')[0];
            txtLastName.Text = string.Join(" ",
                row.Cells["Resident"].Value.ToString().Split(' '),
                1,
                row.Cells["Resident"].Value.ToString().Split(' ').Length - 1);
            cmbAge.SelectedItem = row.Cells["Age"].Value.ToString();
            cmbGender.SelectedItem = row.Cells["Gender"].Value.ToString();
            dtpBirthday.Value = Convert.ToDateTime(row.Cells["Birthday"].Value);

            SetButtonState(true);
        }

        // =============================================
        // SEARCH
        // =============================================
        private void txtArchHealthSearch_TextChanged(object sender, EventArgs e)
        {
            LoadArchivedRecords(txtArchHealthSearch.Text.Trim());
        }

        // =============================================
        // RESTORE (un-archive)
        // =============================================
        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (selectedHealthID == 0)
            {
                MessageBox.Show("Please select a record to restore.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Restore this record to active?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "UPDATE health_records SET is_archived = 0 WHERE ID = @id", conn);
                    cmd.Parameters.AddWithValue("@id", selectedHealthID);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record restored successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                    LoadArchivedRecords();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error restoring record: " + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =============================================
        // PERMANENT DELETE
        // =============================================
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedHealthID == 0)
            {
                MessageBox.Show("Please select a record to delete.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("PERMANENTLY delete this archived record? This cannot be undone!",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "DELETE FROM health_records WHERE ID = @id", conn);
                    cmd.Parameters.AddWithValue("@id", selectedHealthID);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record permanently deleted!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                    LoadArchivedRecords();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting record: " + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =============================================
        // HELPERS
        // =============================================
        private void ClearFields()
        {
            selectedHealthID = 0;
            txtFirstName.Clear();
            txtLastName.Clear();
            cmbAge.SelectedIndex = -1;
            cmbGender.SelectedIndex = -1;
            dtpBirthday.Value = DateTime.Now;
            SetButtonState(false);
        }
        // =============================================
        // VALIDATE & ENABLE BUTTONS
        // Buttons only enable when a record is selected
        // AND all required fields are filled in.
        // =============================================
        private void SetButtonState(bool enabled)
        {
            if (!enabled)
            {
                btnRestore.Enabled = false;
                btnDelete.Enabled = false;
                return;
            }
            CheckAndEnableButtons();
        }

        private void CheckAndEnableButtons()
        {
            bool allFilled = selectedHealthID > 0
                && !string.IsNullOrWhiteSpace(txtFirstName.Text)
                && !string.IsNullOrWhiteSpace(txtLastName.Text)
                && cmbAge.SelectedIndex != -1
                && cmbGender.SelectedIndex != -1;

            btnRestore.Enabled = allFilled;
            btnDelete.Enabled = allFilled;
        }

        // Re-check every time any required field changes
        private void txtFirstName_TextChanged(object sender, EventArgs e)
        { CheckAndEnableButtons(); }

        private void txtLastName_TextChanged(object sender, EventArgs e)
        { CheckAndEnableButtons(); }

        private void cmbAge_SelectedIndexChanged(object sender, EventArgs e)
        { CheckAndEnableButtons(); }

        private void cmbGender_SelectedIndexChanged(object sender, EventArgs e)
        { CheckAndEnableButtons(); }

        private void dtpBirthday_ValueChanged(object sender, EventArgs e) { }

        // =============================================
        // NAVIGATION
        // =============================================
        private void btnNext_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudViewArchive2()); }

        private void btnViewApp_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudViewAppointments()); }

        private void btnAdminDash_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudLGU()); }

        private void btnManageRecord_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudManageHealth()); }

        private void btnViewRecord_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudViewHealth()); }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudGenerateReport()); }

        private void btnInventory_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudInventory()); }

        private void btnViewArchive_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudViewArchive()); }
    }
}
