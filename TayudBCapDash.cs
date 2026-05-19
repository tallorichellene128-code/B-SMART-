using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BSMART
{
    public partial class TayudBCapDash : Form
    {
        private string connectionString = "Server=localhost;Database=bsmart_db;Uid=root;Pwd=;";

        public TayudBCapDash()
        {
            InitializeComponent();
        }

        private void TayudBCapDash_Load(object sender, EventArgs e)
        {
            this.Text = $"B-SMART — {Session.BarangayName} Barangay Captain Dashboard";
            LoadStats();
            LoadRecentRecords();
        }

        // =============================================
        // BARANGAY FILTER HELPER
        // =============================================
        private string BarangayFilter(MySqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@brgyId", Session.BarangayID);
            cmd.Parameters.AddWithValue("@brgyName", Session.BarangayName);
            return " AND (barangay_id = @brgyId OR barangay_id IN (SELECT id FROM barangays WHERE name = @brgyName))";
        }

        // =============================================
        // LOAD SUMMARY STATS
        // lblTotalResidents, lblTotalRecords, lblFrozenAccounts
        // =============================================
        private void LoadStats()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Total health records
                    MySqlCommand c1 = new MySqlCommand(
                        "SELECT COUNT(*) FROM health_records WHERE is_archived = 0 AND (barangay_id = @brgyId OR barangay_id IN (SELECT id FROM barangays WHERE name = @brgyName))", conn);
                    c1.Parameters.AddWithValue("@brgyId", Session.BarangayID);
                    c1.Parameters.AddWithValue("@brgyName", Session.BarangayName);
                    lblTotalRecords.Text = c1.ExecuteScalar().ToString();
                    // Total distinct residents from registered accounts and health records
                    lblTotalResidents.Text = BsmartResidentCountService.CountResidents(Session.BarangayName, Session.BarangayID).ToString();
                    // Frozen resident accounts in this barangay
                    string violationExists = BsmartResidentViolationService.ExistsSql("u");
                    MySqlCommand c3 = new MySqlCommand(@"
                        SELECT COUNT(*)
                        FROM users u
                        LEFT JOIN barangays b ON b.id = u.barangay_id
                        WHERE LOWER(TRIM(u.role)) COLLATE utf8mb4_unicode_ci = 'resident' COLLATE utf8mb4_unicode_ci
                          AND COALESCE(u.is_frozen, 0) = 1
                          AND " + violationExists + @"
                          AND COALESCE(u.is_archived, 0) = 0
                          AND (u.barangay_id = @brgyId
                            OR LOWER(TRIM(COALESCE(b.name, ''))) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(@brgyName)) COLLATE utf8mb4_unicode_ci)", conn);
                    c3.Parameters.AddWithValue("@brgyId", Session.BarangayID);
                    c3.Parameters.AddWithValue("@brgyName", Session.BarangayName ?? "");
                    lblFrozenAccounts.Text = c3.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading stats: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =============================================
        // LOAD RECENT RECORDS INTO GRID
        // =============================================
        private void LoadRecentRecords()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    BsmartIdHelper.EnsureCodes(conn);

                    MySqlCommand cmd = new MySqlCommand(@"
                        SELECT COALESCE(resident_code, CAST(ID AS CHAR)) AS ID,
                               CONCAT(first_name,' ',last_name) AS Resident,
                               gender    AS Gender,
                               age       AS Age,
                               CASE WHEN UPPER(TRIM(COALESCE(Diagnosis, ''))) = 'N/A' THEN '' ELSE COALESCE(Diagnosis, '') END AS Diagnosis,
                               CASE WHEN UPPER(TRIM(COALESCE(Diagnosis, ''))) = 'N/A' THEN '' ELSE COALESCE(Treatment, '') END AS Treatment,
                               assigned_doc_name AS 'Assigned Doctor/Nurse',
                               Date
                        FROM   health_records
                        WHERE  is_archived = 0
                          AND  (barangay_id = @brgyId OR barangay_id IN (SELECT id FROM barangays WHERE name = @brgyName))
                        ORDER  BY Date DESC
                        LIMIT  10", conn);
                    cmd.Parameters.AddWithValue("@brgyId", Session.BarangayID);
                    cmd.Parameters.AddWithValue("@brgyName", Session.BarangayName);

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvRecentResRec.DataSource = dt;
                    StyleGrid(dgvRecentResRec);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading records: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StyleGrid(DataGridView dgv)
        {
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.RowHeadersVisible = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 102, 204);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 9, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 35;
            dgv.EnableHeadersVisualStyles = false;
            dgv.DefaultCellStyle.Font = new Font("Arial", 9);
            dgv.RowTemplate.Height = 30;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(220, 235, 255);
        }

        // =============================================
        // NAVIGATION
        // =============================================
        private void btnBCapDash_Click(object sender, EventArgs e)
        { LoadStats(); LoadRecentRecords(); }

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

        private void btnViewArchive_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudBCapViewArch()); }

        private void btnHealthRep_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudBCViewResHealthRep()); }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Logout?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Session.Clear();
                BsmartFormNavigator.Open(this, new loginBCap());
            }
        }

        private void PTotalResidents_Paint(object sender, PaintEventArgs e) { }
        private void PTotalRecords_Paint(object sender, PaintEventArgs e) { }
        private void PFrozenAccounts_Paint(object sender, PaintEventArgs e) { }
        private void lblTotalResidents_Click(object sender, EventArgs e) { }
        private void lblTotalRecords_Click(object sender, EventArgs e) { }
        private void lblFrozenAccounts_Click(object sender, EventArgs e) { }
        private void dgvRecentResRec_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void btnNotification_Click(object sender, EventArgs e)
        {

        }

        private void btnActivityLog_Click(object sender, EventArgs e)
        {

        }
    }
}