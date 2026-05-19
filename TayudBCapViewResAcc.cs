using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BSMART
{
    public partial class TayudBCapViewResAcc : Form
    {
        private string connectionString = "Server=localhost;Database=bsmart_db;Uid=root;Pwd=;";

        public TayudBCapViewResAcc()
        {
            InitializeComponent();
        }

        private void TayudBCapViewResAcc_Load(object sender, EventArgs e)
        {
            dgvViewResAcc.CellClick -= dgvViewResAcc_CellClick;
            dgvViewResAcc.CellClick += dgvViewResAcc_CellClick;
            dgvViewResAcc.CellContentClick -= dgvViewResAcc_CellContentClick;
            dgvViewResAcc.CellContentClick += dgvViewResAcc_CellClick;

            LoadAccounts();
        }

        public void LoadAccounts(string keyword = "")
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
                                       OR  u.last_name LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR  u.full_name LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR  u.gender LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR  CAST(u.age AS CHAR) LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR  u.address LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR  u.email LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR  u.mobile_number LIKE @kw COLLATE utf8mb4_0900_ai_ci
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
                                             AND LOWER(TRIM(h.last_name)) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(COALESCE(u.last_name, ''))) COLLATE utf8mb4_unicode_ci
                                             AND (u.birthday IS NULL OR h.birthday IS NULL OR DATE(h.birthday) = DATE(u.birthday))
                                             AND (h.barangay_id = u.barangay_id OR h.barangay_id = @brgyId)
                                       ))";
                        cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");
                    }

                    cmd.CommandText = $@"
                        SELECT
                            u.id AS '_DBID',
                            COALESCE((
                                SELECT h2.resident_code
                                FROM health_records h2
                                WHERE COALESCE(h2.is_archived, 0) = 0
                                  AND h2.resident_code IS NOT NULL
                                  AND h2.resident_code <> ''
                                  AND h2.barangay_id = u.barangay_id
                                  AND LOWER(TRIM(h2.first_name)) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(COALESCE(u.first_name, ''))) COLLATE utf8mb4_unicode_ci
                                  AND LOWER(TRIM(h2.last_name)) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(COALESCE(u.last_name, ''))) COLLATE utf8mb4_unicode_ci
                                  AND (u.birthday IS NULL OR h2.birthday IS NULL OR DATE(h2.birthday) = DATE(u.birthday))
                                ORDER BY h2.ID ASC
                                LIMIT 1
                            ), u.resident_code, CAST(u.id AS CHAR)) AS 'ID',
                            COALESCE(NULLIF(TRIM(u.full_name), ''),
                                     NULLIF(TRIM(CONCAT(COALESCE(u.first_name, ''), ' ', COALESCE(u.last_name, ''))), ''),
                                     u.username) AS 'Resident',
                            COALESCE(u.gender, '') AS 'Gender',
                            COALESCE(u.age, '') AS 'Age',
                            u.birthday AS 'Birthday',
                            COALESCE(u.address, '') AS 'Address',
                            COALESCE(u.email, '') AS 'Email',
                            COALESCE(u.mobile_number, '') AS 'Mobile',
                            CASE WHEN COALESCE(u.is_frozen, 0) = 1 AND {violationExists} THEN 'Frozen' ELSE 'Active' END AS 'Status',
                            COALESCE((
                                SELECT GROUP_CONCAT(DISTINCT TRIM(h.violation) ORDER BY h.violation SEPARATOR ', ')
                                FROM health_records h
                                WHERE COALESCE(h.is_archived, 0) = 0
                                  AND h.violation IS NOT NULL
                                  AND TRIM(h.violation) <> ''
                                  AND LOWER(TRIM(h.first_name)) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(COALESCE(u.first_name, ''))) COLLATE utf8mb4_unicode_ci
                                  AND LOWER(TRIM(h.last_name)) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(COALESCE(u.last_name, ''))) COLLATE utf8mb4_unicode_ci
                                  AND (u.birthday IS NULL OR h.birthday IS NULL OR DATE(h.birthday) = DATE(u.birthday))
                                  AND (h.barangay_id = u.barangay_id OR h.barangay_id = @brgyId)
                            ), '') AS 'Violation'
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

                    dgvViewResAcc.DataSource = dt;
                    StyleGrid();
                }
            }
            catch (Exception ex)
            {
                dgvViewResAcc.DataSource = null;
                MessageBox.Show("Error loading resident accounts: " + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void StyleGrid()
        {
            dgvViewResAcc.ReadOnly = true;
            dgvViewResAcc.AllowUserToAddRows = false;
            dgvViewResAcc.AllowUserToDeleteRows = false;
            dgvViewResAcc.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            if (dgvViewResAcc.Columns.Contains("_DBID"))
                dgvViewResAcc.Columns["_DBID"].Visible = false;
            dgvViewResAcc.MultiSelect = false;
            dgvViewResAcc.BackgroundColor = Color.White;
            dgvViewResAcc.BorderStyle = BorderStyle.None;
            dgvViewResAcc.RowHeadersVisible = false;
            dgvViewResAcc.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvViewResAcc.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 102, 204);
            dgvViewResAcc.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvViewResAcc.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 9, FontStyle.Bold);
            dgvViewResAcc.ColumnHeadersHeight = 35;
            dgvViewResAcc.EnableHeadersVisualStyles = false;
            dgvViewResAcc.DefaultCellStyle.Font = new Font("Arial", 9);
            dgvViewResAcc.RowTemplate.Height = 30;
            dgvViewResAcc.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(220, 235, 255);

            if (dgvViewResAcc.Columns.Contains("ID"))
                dgvViewResAcc.Columns["ID"].FillWeight = 30;
            if (dgvViewResAcc.Columns.Contains("Resident"))
                dgvViewResAcc.Columns["Resident"].FillWeight = 120;
            if (dgvViewResAcc.Columns.Contains("Gender"))
                dgvViewResAcc.Columns["Gender"].FillWeight = 50;
            if (dgvViewResAcc.Columns.Contains("Age"))
                dgvViewResAcc.Columns["Age"].FillWeight = 35;
            if (dgvViewResAcc.Columns.Contains("Birthday"))
                dgvViewResAcc.Columns["Birthday"].FillWeight = 80;
            if (dgvViewResAcc.Columns.Contains("Address"))
                dgvViewResAcc.Columns["Address"].FillWeight = 160;
            if (dgvViewResAcc.Columns.Contains("Email"))
                dgvViewResAcc.Columns["Email"].FillWeight = 120;
            if (dgvViewResAcc.Columns.Contains("Mobile"))
                dgvViewResAcc.Columns["Mobile"].FillWeight = 80;
            if (dgvViewResAcc.Columns.Contains("Status"))
                dgvViewResAcc.Columns["Status"].FillWeight = 65;
            if (dgvViewResAcc.Columns.Contains("Violation"))
                dgvViewResAcc.Columns["Violation"].FillWeight = 130;
        }

        private void dgvViewResAcc_CellClick(object sender, DataGridViewCellEventArgs e) { }
        private void dgvViewResAcc_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        { LoadAccounts(txtSearch.Text.Trim()); }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Logout?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
        { txtSearch.Clear(); LoadAccounts(); }

        private void btnManageFrozenAcc_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudBCapManageFAcc()); }

        private void btnHealthRep_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudBCViewResHealthRep()); }

        private void btnViewArchive_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudBCapViewArch()); }
    }
}
