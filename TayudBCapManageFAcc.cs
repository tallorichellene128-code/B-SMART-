using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BSMART
{
    public partial class TayudBCapManageFAcc : Form
    {
        private string connectionString = "Server=localhost;Database=bsmart_db;Uid=root;Pwd=;";
        private int selectedUserID = 0;

        public TayudBCapManageFAcc()
        {
            InitializeComponent();
        }

        private void TayudBCapManageFAcc_Load(object sender, EventArgs e)
        {
            cmbAge.Items.Clear();
            for (int i = 1; i <= 120; i++) cmbAge.Items.Add(i.ToString());
            cmbGender.Items.Clear();
            cmbGender.Items.Add("Male");
            cmbGender.Items.Add("Female");

            cmbBarangay.Items.Clear();
            cmbBarangay.Items.Add(Session.BarangayName);
            cmbBarangay.SelectedIndex = 0;
            cmbBarangay.Enabled = false;

            SetButtonState(false);
            LoadFrozenAccounts();

            // Wire CellClick in code so designer binding isn't needed
            dgvResFroAccRec.CellClick -= dgvResFroAccRec_CellClick;
            dgvResFroAccRec.CellClick += dgvResFroAccRec_CellClick;
        }

        // =============================================
        // LOAD FROZEN ACCOUNTS — barangay scoped
        // =============================================
        private void LoadFrozenAccounts(string keyword = "")
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("", conn);
                    string violationExists = BsmartResidentViolationService.ExistsSql("u");

                    string keyFilter = "";
                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        keyFilter = @" AND (CAST(u.id AS CHAR) LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR u.first_name LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR u.last_name LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR CONCAT(u.first_name,' ',u.last_name) LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR u.gender LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR CAST(u.age AS CHAR) LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR u.birthday LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR u.email LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR u.mobile_number LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR u.address LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR 'Frozen' LIKE @kw COLLATE utf8mb4_0900_ai_ci)";
                        cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");
                    }

                    cmd.CommandText = $@"SELECT
                                            u.id         AS ID,
                                            CONCAT(u.first_name,' ',u.last_name) AS Resident,
                                            u.gender     AS Gender,
                                            u.age        AS Age,
                                            u.birthday   AS Birthday,
                                            u.email      AS Email
                                         FROM users u
                                         WHERE u.role        = 'Resident'
                                           AND COALESCE(u.is_frozen, 0) = 1
                                           AND {violationExists}
                                           AND COALESCE(u.is_archived, 0) = 0
                                           AND (u.barangay_id = @brgyId OR u.barangay_id IN (SELECT id FROM barangays WHERE name = @brgyName)){keyFilter}
                                         ORDER BY u.last_name ASC";
                    cmd.Parameters.AddWithValue("@brgyId", Session.BarangayID);
                    cmd.Parameters.AddWithValue("@brgyName", Session.BarangayName);

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvResFroAccRec.DataSource = dt;
                    StyleGrid();
                    ClearFields();
                }
            }
            catch
            {
                dgvResFroAccRec.DataSource = null;
            }
        }

        private void StyleGrid()
        {
            dgvResFroAccRec.ReadOnly = true;
            dgvResFroAccRec.AllowUserToAddRows = false;
            dgvResFroAccRec.AllowUserToDeleteRows = false;
            dgvResFroAccRec.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvResFroAccRec.BackgroundColor = Color.White;
            dgvResFroAccRec.BorderStyle = BorderStyle.None;
            dgvResFroAccRec.RowHeadersVisible = false;
            dgvResFroAccRec.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvResFroAccRec.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 102, 204);
            dgvResFroAccRec.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvResFroAccRec.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 9, FontStyle.Bold);
            dgvResFroAccRec.ColumnHeadersHeight = 35;
            dgvResFroAccRec.EnableHeadersVisualStyles = false;
            dgvResFroAccRec.DefaultCellStyle.Font = new Font("Arial", 9);
            dgvResFroAccRec.RowTemplate.Height = 30;
            dgvResFroAccRec.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(220, 235, 255);
        }

        // =============================================
        // ROW CLICK ? populate fields
        // =============================================
        private void dgvResFroAccRec_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvResFroAccRec.Rows[e.RowIndex];

            selectedUserID = Convert.ToInt32(row.Cells["ID"].Value);
            string[] parts = row.Cells["Resident"].Value.ToString().Split(new char[] { ' ' }, 2);
            txtFirstName.Text = parts[0];
            txtLastName.Text = parts.Length > 1 ? parts[1] : "";
            cmbAge.SelectedItem = row.Cells["Age"].Value.ToString();
            cmbGender.SelectedItem = row.Cells["Gender"].Value.ToString();
            txtEmailAdd.Text = row.Cells["Email"].Value?.ToString() ?? "";
            SetButtonState(true);
        }

        private void dgvResFroAccRec_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void SetButtonState(bool enabled)
        {
            btnUnFreezeAcc.Enabled = enabled;
            btnArchiveAcc.Enabled = enabled;
            btnRemoveAcc.Enabled = enabled;
        }

        private void ClearFields()
        {
            selectedUserID = 0;
            txtFirstName.Clear(); txtLastName.Clear();
            cmbAge.SelectedIndex = -1; cmbGender.SelectedIndex = -1;
            txtEmailAdd.Clear();
            SetButtonState(false);
        }
private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadFrozenAccounts(txtSearch.Text.Trim());
        }

        // =============================================
        // UNFREEZE ? account goes back to active list
        // =============================================
        private void btnUnFreezeAcc_Click(object sender, EventArgs e)
        {
            if (selectedUserID == 0) return;
            if (MessageBox.Show("Unfreeze this account?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "UPDATE users SET is_frozen = 0 WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", selectedUserID);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Account unfrozen!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadFrozenAccounts();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =============================================
        // ARCHIVE ? moves to ViewArch2
        // =============================================
        private void btnArchiveAcc_Click(object sender, EventArgs e)
        {
            if (selectedUserID == 0) return;
            if (MessageBox.Show("Archive this frozen account?", "Confirm",
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
                    LoadFrozenAccounts();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =============================================
        // REMOVE ? permanent delete
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
                    LoadFrozenAccounts();
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

        private void btnViewHealthRep_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudBCViewResHealthRep()); }

        private void btnViewArchive_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudBCapViewArch()); }

        private void txtFirstName_TextChanged(object sender, EventArgs e) { }
        private void txtLastName_TextChanged(object sender, EventArgs e) { }
        private void cmbAge_SelectedIndexChanged(object sender, EventArgs e) { }
        private void cmbGender_SelectedIndexChanged(object sender, EventArgs e) { }
        private void txtEmailAdd_TextChanged(object sender, EventArgs e) { }
        private void cmbBarangay_SelectedIndexChanged(object sender, EventArgs e) { }
    }
}