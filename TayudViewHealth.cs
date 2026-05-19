using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BSMART
{
    public partial class TayudViewHealth : Form
    {
        private string connectionString = "Server=localhost;Database=bsmart_db;Uid=root;Pwd=;";

        public TayudViewHealth()
        {
            InitializeComponent();
            btnLogout.Click += (s, e) => SettingsNavigationService.OpenLogin(this);
        }

        private void TayudViewHealth_Load(object sender, EventArgs e)
        {
            if (cmbResorHealth.SelectedIndex < 0)
                cmbResorHealth.SelectedIndex = 0;

            LoadHealthRecords();
        }

        // =============================================
        // BARANGAY FILTER HELPER
        // Mayor (BarangayID = 0) sees all barangays.
        // All other roles see only their own barangay.
        // =============================================
        private string BarangayFilter(MySqlCommand cmd, string tableAlias = "")
        {
            if (Session.IsMayor) return "";
            return $" AND ({tableAlias}barangay_id = @brgyId OR {tableAlias}barangay_id IN (SELECT id FROM barangays WHERE name = @brgyName))";
        }

        // =============================================
        // LOAD HEALTH RECORDS
        // =============================================
        public void LoadHealthRecords(string searchKeyword = "")
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    BsmartIdHelper.EnsureCodes(conn);

                    MySqlCommand cmd = new MySqlCommand("", conn);
                    string healthBrgyFilter = BarangayFilter(cmd, "h.");
                    string userBrgyFilter = BarangayFilter(cmd, "u.");

                    string keyFilter = "";
                    if (!string.IsNullOrWhiteSpace(searchKeyword))
                    {
                        keyFilter = @" AND (
                                        DisplayID COLLATE utf8mb4_unicode_ci LIKE @keyword COLLATE utf8mb4_unicode_ci
                                   OR   Resident COLLATE utf8mb4_unicode_ci LIKE @keyword COLLATE utf8mb4_unicode_ci
                                   OR   Gender COLLATE utf8mb4_unicode_ci LIKE @keyword COLLATE utf8mb4_unicode_ci
                                   OR   CAST(Age AS CHAR) LIKE @keyword COLLATE utf8mb4_unicode_ci
                                   OR   Diagnosis COLLATE utf8mb4_unicode_ci LIKE @keyword COLLATE utf8mb4_unicode_ci
                                   OR   Treatment COLLATE utf8mb4_unicode_ci LIKE @keyword COLLATE utf8mb4_unicode_ci
                                   OR   AssignedDoctor COLLATE utf8mb4_unicode_ci LIKE @keyword COLLATE utf8mb4_unicode_ci
                                   OR   Violation COLLATE utf8mb4_unicode_ci LIKE @keyword COLLATE utf8mb4_unicode_ci
                                   OR   DATE_FORMAT(RecordDate, '%d/%m/%Y') LIKE @keyword COLLATE utf8mb4_unicode_ci
                                   OR   DATE_FORMAT(RecordDate, '%m/%d/%Y') LIKE @keyword COLLATE utf8mb4_unicode_ci
                                   OR   DATE_FORMAT(RecordDate, '%Y-%m-%d') LIKE @keyword COLLATE utf8mb4_unicode_ci
                                   )";
                        cmd.Parameters.AddWithValue("@keyword", "%" + searchKeyword + "%");
                    }

                    string mode = cmbResorHealth.SelectedItem?.ToString() ?? "All Records";
                    string modeFilter = mode switch
                    {
                        "With Health Records" => " AND HasHealthRecord = 1",
                        "Without Health Records" => " AND HasHealthRecord = 0",
                        _ => ""
                    };
                    cmd.Parameters.AddWithValue("@brgyId", Session.BarangayID);
                    cmd.Parameters.AddWithValue("@brgyName", Session.BarangayName);

                    cmd.CommandText = $@"
                        SELECT
                            _DBID,
                            DisplayID AS 'ID',
                            Resident,
                            Gender,
                            Age,
                            Diagnosis,
                            Treatment,
                            RecordDate AS 'Date'
                        FROM
                        (
                            SELECT
                                h.ID AS _DBID,
                                CONVERT(COALESCE(h.resident_code, CAST(h.ID AS CHAR)) USING utf8mb4) COLLATE utf8mb4_unicode_ci AS DisplayID,
                                CONVERT(TRIM(CONCAT(COALESCE(h.first_name, ''), ' ', COALESCE(h.last_name, ''))) USING utf8mb4) COLLATE utf8mb4_unicode_ci AS Resident,
                                CONVERT(COALESCE(h.gender, '') USING utf8mb4) COLLATE utf8mb4_unicode_ci AS Gender,
                                h.age AS Age,
                                CASE
                                    WHEN UPPER(TRIM(COALESCE(h.Diagnosis, ''))) IN ('', 'N/A') THEN ''
                                    ELSE CONVERT(COALESCE(h.Diagnosis, '') USING utf8mb4) COLLATE utf8mb4_unicode_ci
                                END AS Diagnosis,
                                CASE
                                    WHEN UPPER(TRIM(COALESCE(h.Diagnosis, ''))) IN ('', 'N/A') THEN ''
                                    ELSE CONVERT(COALESCE(h.Treatment, '') USING utf8mb4) COLLATE utf8mb4_unicode_ci
                                END AS Treatment,
                                CONVERT(COALESCE(h.assigned_doc_name, '') USING utf8mb4) COLLATE utf8mb4_unicode_ci AS AssignedDoctor,
                                CONVERT(COALESCE(h.violation, '') USING utf8mb4) COLLATE utf8mb4_unicode_ci AS Violation,
                                h.Date AS RecordDate,
                                CASE WHEN UPPER(TRIM(COALESCE(h.Diagnosis, ''))) IN ('', 'N/A') THEN 0 ELSE 1 END AS HasHealthRecord
                            FROM health_records h
                            WHERE h.is_archived = 0{healthBrgyFilter}

                            UNION ALL

                            SELECT
                                u.id AS _DBID,
                                CONVERT(COALESCE(u.resident_code, CAST(u.id AS CHAR)) USING utf8mb4) COLLATE utf8mb4_unicode_ci AS DisplayID,
                                CONVERT(TRIM(COALESCE(NULLIF(u.full_name, ''), CONCAT(COALESCE(u.first_name, ''), ' ', COALESCE(u.last_name, '')))) USING utf8mb4) COLLATE utf8mb4_unicode_ci AS Resident,
                                CONVERT(COALESCE(u.gender, '') USING utf8mb4) COLLATE utf8mb4_unicode_ci AS Gender,
                                u.age AS Age,
                                '' AS Diagnosis,
                                '' AS Treatment,
                                '' AS AssignedDoctor,
                                '' AS Violation,
                                NULL AS RecordDate,
                                0 AS HasHealthRecord
                            FROM users u
                            WHERE u.role = 'Resident'
                              AND COALESCE(u.is_archived, 0) = 0{userBrgyFilter}
                              AND NOT EXISTS
                              (
                                  SELECT 1
                                  FROM health_records h2
                                  WHERE h2.is_archived = 0
                                    AND (
                                        (COALESCE(u.resident_code, '') <> ''
                                         AND CONVERT(h2.resident_code USING utf8mb4) COLLATE utf8mb4_unicode_ci =
                                             CONVERT(u.resident_code USING utf8mb4) COLLATE utf8mb4_unicode_ci)
                                        OR
                                        (CONVERT(h2.first_name USING utf8mb4) COLLATE utf8mb4_unicode_ci =
                                             CONVERT(u.first_name USING utf8mb4) COLLATE utf8mb4_unicode_ci
                                         AND CONVERT(COALESCE(h2.middle_name, '') USING utf8mb4) COLLATE utf8mb4_unicode_ci =
                                             CONVERT(COALESCE(u.middle_name, '') USING utf8mb4) COLLATE utf8mb4_unicode_ci
                                         AND CONVERT(h2.last_name USING utf8mb4) COLLATE utf8mb4_unicode_ci =
                                             CONVERT(u.last_name USING utf8mb4) COLLATE utf8mb4_unicode_ci
                                         AND h2.barangay_id = u.barangay_id)
                                    )
                              )
                        ) records
                        WHERE 1 = 1{modeFilter}{keyFilter}
                        ORDER BY
                            CASE WHEN RecordDate IS NULL THEN 1 ELSE 0 END,
                            RecordDate DESC,
                            Resident ASC";

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvViewRecord.DataSource = dt;
                    StyleGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading records: " + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =============================================
        // STYLE GRID
        // =============================================
        private void StyleGrid()
        {
            dgvViewRecord.ReadOnly = true;
            dgvViewRecord.AllowUserToAddRows = false;
            dgvViewRecord.AllowUserToDeleteRows = false;
            dgvViewRecord.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvViewRecord.BackgroundColor = Color.White;
            dgvViewRecord.BorderStyle = BorderStyle.None;
            dgvViewRecord.RowHeadersVisible = false;
            dgvViewRecord.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvViewRecord.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 102, 204);
            dgvViewRecord.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvViewRecord.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
            dgvViewRecord.ColumnHeadersHeight = 40;
            dgvViewRecord.EnableHeadersVisualStyles = false;

            dgvViewRecord.DefaultCellStyle.Font = new Font("Arial", 9);
            dgvViewRecord.RowTemplate.Height = 35;
            dgvViewRecord.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(220, 235, 255);

            if (dgvViewRecord.Columns.Count > 0)
            {
                if (dgvViewRecord.Columns.Contains("_DBID"))
                    dgvViewRecord.Columns["_DBID"].Visible = false;
                dgvViewRecord.Columns["ID"].FillWeight = 30;
                dgvViewRecord.Columns["Resident"].FillWeight = 120;
                dgvViewRecord.Columns["Gender"].FillWeight = 60;
                dgvViewRecord.Columns["Age"].FillWeight = 40;
                dgvViewRecord.Columns["Diagnosis"].FillWeight = 120;
                dgvViewRecord.Columns["Treatment"].FillWeight = 220;
                dgvViewRecord.Columns["Date"].FillWeight = 80;
            }
        }

        // =============================================
        // SEARCH
        // =============================================
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadHealthRecords(txtSearch.Text.Trim());
        }

        private void cmbResorHealth_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadHealthRecords(txtSearch.Text.Trim());
        }

        private void dgvViewRecord_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        // =============================================
        // NAVIGATION
        // =============================================
        private void btnManageRecord_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudManageHealth()); }

        private void btnViewRecord_Click(object sender, EventArgs e)
        { txtSearch.Clear(); LoadHealthRecords(); }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudGenerateReport()); }

        private void btnInventory_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudInventory()); }

        private void btnViewArchive_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudViewArchive()); }

        private void btnAdminDash_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudLGU()); }

        private void btnViewApp_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudViewAppointments()); }
    }
}
