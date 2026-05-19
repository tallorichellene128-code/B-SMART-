using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BSMART
{
    public partial class TayudBCapViewArch2 : Form
    {
        private string connectionString = "Server=localhost;Database=bsmart_db;Uid=root;Pwd=;";
        private int selectedUserID = 0;

        public TayudBCapViewArch2()
        {
            InitializeComponent();
        }

        private void TayudBCapViewArch2_Load(object sender, EventArgs e)
        {
            cmbAge.Items.Clear();
            for (int i = 1; i <= 120; i++) cmbAge.Items.Add(i.ToString());
            cmbGender.Items.Clear();
            cmbGender.Items.Add("Male");
            cmbGender.Items.Add("Female");

            SetButtonState(false);
            LoadArchivedAccounts();
        }

        // =============================================
        // LOAD ARCHIVED ACCOUNTS
        // Pulls from users table where is_archived = 1
        // and barangay_id matches captain's barangay.
        // Placeholder until resident registration is built.
        // =============================================
        private void LoadArchivedAccounts(string keyword = "")
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("", conn);

                    string keyFilter = "";
                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        keyFilter = " AND (first_name LIKE @kw COLLATE utf8mb4_0900_ai_ci OR last_name LIKE @kw COLLATE utf8mb4_0900_ai_ci OR email LIKE @kw COLLATE utf8mb4_0900_ai_ci)";
                        cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");
                    }

                    // Uses users table — will show data once resident registration is done
                    cmd.CommandText = $@"SELECT
                                            id         AS ID,
                                            CONCAT(first_name,' ',last_name) AS Resident,
                                            gender     AS Gender,
                                            age        AS Age,
                                            birthday   AS Birthday,
                                            email      AS Email
                                         FROM users
                                         WHERE role        = 'Resident'
                                           AND is_archived = 1
                                           AND (barangay_id = @brgyId OR barangay_id IN (SELECT id FROM barangays WHERE name = @brgyName)){keyFilter}
                                         ORDER BY last_name ASC";
                    cmd.Parameters.AddWithValue("@brgyId", Session.BarangayID);
                    cmd.Parameters.AddWithValue("@brgyName", Session.BarangayName);

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvArchResAcc.DataSource = dt;
                    StyleGrid();
                    ClearFields();
                }
            }
            catch
            {
                // users table may not have is_archived yet — show empty grid gracefully
                dgvArchResAcc.DataSource = null;
            }
        }

        private void StyleGrid()
        {
            dgvArchResAcc.ReadOnly = true;
            dgvArchResAcc.AllowUserToAddRows = false;
            dgvArchResAcc.AllowUserToDeleteRows = false;
            dgvArchResAcc.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvArchResAcc.BackgroundColor = Color.White;
            dgvArchResAcc.BorderStyle = BorderStyle.None;
            dgvArchResAcc.RowHeadersVisible = false;
            dgvArchResAcc.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvArchResAcc.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 102, 204);
            dgvArchResAcc.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvArchResAcc.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 9, FontStyle.Bold);
            dgvArchResAcc.ColumnHeadersHeight = 35;
            dgvArchResAcc.EnableHeadersVisualStyles = false;
            dgvArchResAcc.DefaultCellStyle.Font = new Font("Arial", 9);
            dgvArchResAcc.RowTemplate.Height = 30;
            dgvArchResAcc.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(220, 235, 255);
        }

        // =============================================
        // ROW CLICK ? populate fields
        // =============================================
        private void dgvArchResAcc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvArchResAcc.Rows[e.RowIndex];

            selectedUserID = Convert.ToInt32(row.Cells["ID"].Value);

            string[] parts = row.Cells["Resident"].Value.ToString().Split(new char[] { ' ' }, 2);
            txtFirstName.Text = parts[0];
            txtLastName.Text = parts.Length > 1 ? parts[1] : "";
            cmbAge.SelectedItem = row.Cells["Age"].Value.ToString();
            cmbGender.SelectedItem = row.Cells["Gender"].Value.ToString();

            try { dtpBirthday.Value = Convert.ToDateTime(row.Cells["Birthday"].Value); }
            catch { dtpBirthday.Value = DateTime.Now; }

            txtEmailAdd.Text = row.Cells["Email"].Value?.ToString() ?? "";
            CheckAndEnableButtons();
        }

        private void SetButtonState(bool enabled)
        {
            if (!enabled) { btnRestore.Enabled = false; btnDelete.Enabled = false; return; }
            CheckAndEnableButtons();
        }

        private void CheckAndEnableButtons()
        {
            bool ok = selectedUserID > 0
                && !string.IsNullOrWhiteSpace(txtFirstName.Text)
                && !string.IsNullOrWhiteSpace(txtLastName.Text)
                && cmbAge.SelectedIndex != -1
                && cmbGender.SelectedIndex != -1;

            btnRestore.Enabled = ok;
            btnDelete.Enabled = ok;
        }

        private void txtFirstName_TextChanged(object sender, EventArgs e) { CheckAndEnableButtons(); }
        private void txtLastName_TextChanged(object sender, EventArgs e) { CheckAndEnableButtons(); }
        private void cmbAge_SelectedIndexChanged(object sender, EventArgs e) { CheckAndEnableButtons(); }
        private void cmbGender_SelectedIndexChanged(object sender, EventArgs e) { CheckAndEnableButtons(); }
        private void dtpBirthday_ValueChanged(object sender, EventArgs e) { }
        private void txtEmailAdd_TextChanged(object sender, EventArgs e) { }

        private void ClearFields()
        {
            selectedUserID = 0;
            txtFirstName.Clear(); txtLastName.Clear();
            cmbAge.SelectedIndex = -1; cmbGender.SelectedIndex = -1;
            dtpBirthday.Value = DateTime.Now;
            txtEmailAdd.Clear();
            SetButtonState(false);
        }

        private void txtArchAccSearch_TextChanged(object sender, EventArgs e)
        { LoadArchivedAccounts(txtArchAccSearch.Text.Trim()); }

        // =============================================
        // RESTORE ACCOUNT
        // =============================================
        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (selectedUserID == 0) return;
            if (MessageBox.Show("Restore this account?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "UPDATE users SET is_archived = 0 WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", selectedUserID);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Account restored!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadArchivedAccounts();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =============================================
        // DELETE ACCOUNT
        // =============================================
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedUserID == 0) return;
            if (MessageBox.Show("PERMANENTLY delete this account?", "Confirm",
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
                    MessageBox.Show("Account deleted!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadArchivedAccounts();
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
        private void btnBack_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudBCapViewArch()); }

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
    }
}