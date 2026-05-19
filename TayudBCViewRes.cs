using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BSMART
{
    public partial class TayudBCViewRes : Form
    {
        private string connectionString = "Server=localhost;Database=bsmart_db;Uid=root;Pwd=;";

        public TayudBCViewRes()
        {
            InitializeComponent();
        }

        private void TayudBCViewRes_Load(object sender, EventArgs e)
        {
            LoadRecords();
        }

        // =============================================
        // LOAD RECORDS — scoped to captain's barangay
        // =============================================
        public void LoadRecords(string keyword = "")
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    BsmartIdHelper.EnsureCodes(conn);
                    MySqlCommand cmd = new MySqlCommand("", conn);

                    string keyFilter = "";
                    string accountKeyFilter = "";
                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        keyFilter = @" AND (h.first_name LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR  h.last_name  LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR  CONCAT(h.first_name,' ',h.last_name) LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR  h.Diagnosis  LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR  h.Treatment  LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR  COALESCE(h.violation, '') LIKE @kw COLLATE utf8mb4_0900_ai_ci)";
                        accountKeyFilter = @" AND (u.first_name LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                               OR  u.last_name  LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                               OR  CONCAT(u.first_name,' ',u.last_name) LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                               OR  u.full_name LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                               OR  u.email LIKE @kw COLLATE utf8mb4_0900_ai_ci)";
                        cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");
                    }

                    cmd.CommandText = $@"SELECT
                                            h.ID AS '_DBID',
                                            COALESCE(h.resident_code, CAST(h.ID AS CHAR)) COLLATE utf8mb4_unicode_ci AS 'ID',
                                            CONCAT(h.first_name,' ',h.last_name) COLLATE utf8mb4_unicode_ci AS Resident,
                                            COALESCE(h.gender, '') COLLATE utf8mb4_unicode_ci AS Gender,
                                            h.age       AS Age,
                                            CASE WHEN UPPER(TRIM(COALESCE(h.Diagnosis, ''))) = 'N/A' THEN '' ELSE COALESCE(h.Diagnosis, '') END COLLATE utf8mb4_unicode_ci AS Diagnosis,
                                            CASE WHEN UPPER(TRIM(COALESCE(h.Diagnosis, ''))) = 'N/A' THEN '' ELSE COALESCE(h.Treatment, '') END COLLATE utf8mb4_unicode_ci AS Treatment,
                                            COALESCE(h.assigned_doc_name, '') COLLATE utf8mb4_unicode_ci AS 'Assigned Doctor/Nurse',
                                            COALESCE(h.violation, '') COLLATE utf8mb4_unicode_ci AS Violation,
                                            h.Date
                                         FROM health_records h
                                         WHERE h.is_archived = 0
                                           AND (h.barangay_id = @brgyId OR h.barangay_id IN (SELECT id FROM barangays WHERE name COLLATE utf8mb4_unicode_ci = @brgyName COLLATE utf8mb4_unicode_ci)){keyFilter}

                                         UNION ALL

                                         SELECT
                                            u.id AS '_DBID',
                                            COALESCE((
                                                SELECT h2.resident_code
                                                FROM health_records h2
                                                WHERE COALESCE(h2.is_archived, 0) = 0
                                                  AND h2.resident_code IS NOT NULL
                                                  AND h2.resident_code <> ''
                                                  AND h2.barangay_id = u.barangay_id
                                                  AND LOWER(TRIM(h2.first_name)) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(u.first_name)) COLLATE utf8mb4_unicode_ci
                                                  AND LOWER(TRIM(h2.last_name)) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(u.last_name)) COLLATE utf8mb4_unicode_ci
                                                  AND (u.birthday IS NULL OR h2.birthday IS NULL OR DATE(h2.birthday) = DATE(u.birthday))
                                                ORDER BY h2.ID ASC
                                                LIMIT 1
                                            ), u.resident_code, CAST(u.id AS CHAR)) COLLATE utf8mb4_unicode_ci AS 'ID',
                                            TRIM(COALESCE(NULLIF(u.full_name, ''), CONCAT(u.first_name,' ',u.last_name))) COLLATE utf8mb4_unicode_ci AS Resident,
                                            COALESCE(u.gender, '') COLLATE utf8mb4_unicode_ci AS Gender,
                                            u.age AS Age,
                                            '' COLLATE utf8mb4_unicode_ci AS Diagnosis,
                                            '' COLLATE utf8mb4_unicode_ci AS Treatment,
                                            '' COLLATE utf8mb4_unicode_ci AS 'Assigned Doctor/Nurse',
                                            '' COLLATE utf8mb4_unicode_ci AS Violation,
                                            NULL AS Date
                                         FROM users u
                                         WHERE u.role COLLATE utf8mb4_unicode_ci = 'Resident' COLLATE utf8mb4_unicode_ci
                                           AND COALESCE(u.is_archived, 0) = 0
                                           AND (u.barangay_id = @brgyId OR u.barangay_id IN (SELECT id FROM barangays WHERE name COLLATE utf8mb4_unicode_ci = @brgyName COLLATE utf8mb4_unicode_ci)){accountKeyFilter}
                                           AND NOT EXISTS (
                                               SELECT 1
                                               FROM health_records h
                                               WHERE h.is_archived = 0
                                                 AND (h.barangay_id = @brgyId OR h.barangay_id IN (SELECT id FROM barangays WHERE name COLLATE utf8mb4_unicode_ci = @brgyName COLLATE utf8mb4_unicode_ci))
                                                 AND LOWER(TRIM(h.first_name)) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(u.first_name)) COLLATE utf8mb4_unicode_ci
                                                 AND LOWER(TRIM(h.last_name)) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(u.last_name)) COLLATE utf8mb4_unicode_ci
                                                 AND (u.birthday IS NULL OR h.birthday IS NULL OR DATE(h.birthday) = DATE(u.birthday))
                                           )
                                         ORDER BY Date DESC, Resident ASC";
                    cmd.Parameters.AddWithValue("@brgyId", Session.BarangayID);
                    cmd.Parameters.AddWithValue("@brgyName", Session.BarangayName);

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvResViewRecord.DataSource = dt;
                    StyleGrid();
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
            dgvResViewRecord.ReadOnly = true;
            dgvResViewRecord.AllowUserToAddRows = false;
            dgvResViewRecord.AllowUserToDeleteRows = false;
            dgvResViewRecord.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvResViewRecord.BackgroundColor = Color.White;
            dgvResViewRecord.BorderStyle = BorderStyle.None;
            dgvResViewRecord.RowHeadersVisible = false;
            dgvResViewRecord.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvResViewRecord.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 102, 204);
            dgvResViewRecord.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvResViewRecord.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 9, FontStyle.Bold);
            dgvResViewRecord.ColumnHeadersHeight = 35;
            dgvResViewRecord.EnableHeadersVisualStyles = false;
            dgvResViewRecord.DefaultCellStyle.Font = new Font("Arial", 9);
            dgvResViewRecord.RowTemplate.Height = 30;
            dgvResViewRecord.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(220, 235, 255);
            if (dgvResViewRecord.Columns.Contains("_DBID"))
                dgvResViewRecord.Columns["_DBID"].Visible = false;
            if (dgvResViewRecord.Columns.Contains("Violation"))
                dgvResViewRecord.Columns["Violation"].FillWeight = 140;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        { LoadRecords(txtSearch.Text.Trim()); }

        private void dgvResViewRecord_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        // =============================================
        // NAVIGATION
        // =============================================
        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Logout?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            { Session.Clear(); BsmartFormNavigator.Open(this, new loginBCap()); }
        }

        private void btnSettings_Click(object sender, EventArgs e) { SettingsNavigationService.OpenSettings(this); }
        private void btnBCapDash_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudBCapDash()); }

        private void btnResManageRecord_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudBCManageRes()); }

        private void btnResViewRecord_Click(object sender, EventArgs e)
        { txtSearch.Clear(); LoadRecords(); }

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
    }
}