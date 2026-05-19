using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BSMART
{
    public partial class TayudBCapViewArch : Form
    {
        private string connectionString = "Server=localhost;Database=bsmart_db;Uid=root;Pwd=;";
        private int selectedID = 0;

        public TayudBCapViewArch()
        {
            InitializeComponent();
        }

        private void TayudBCapViewArch_Load(object sender, EventArgs e)
        {
            cmbAge.Items.Clear();
            for (int i = 1; i <= 120; i++) cmbAge.Items.Add(i.ToString());
            cmbGender.Items.Clear();
            cmbGender.Items.Add("Male");
            cmbGender.Items.Add("Female");

            SetButtonState(false);
            LoadArchivedRecords();
        }

        // =============================================
        // LOAD ARCHIVED RECORDS — barangay scoped
        // =============================================
        public void LoadArchivedRecords(string keyword = "")
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
                        keyFilter = @" AND (first_name LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR  last_name  LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR  CONCAT(first_name,' ',last_name) LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR  Diagnosis LIKE @kw COLLATE utf8mb4_0900_ai_ci)";
                        cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");
                    }

                    cmd.CommandText = $@"SELECT
                                            ID,
                                            CONCAT(first_name,' ',last_name) AS Resident,
                                            gender   AS Gender,
                                            age      AS Age,
                                            birthday AS Birthday,
                                            Diagnosis,
                                            Treatment,
                                            Date     AS 'Record Date'
                                         FROM health_records
                                         WHERE is_archived = 1
                                           AND (barangay_id = @brgyId OR barangay_id IN (SELECT id FROM barangays WHERE name = @brgyName)){keyFilter}
                                         ORDER BY Date DESC";
                    cmd.Parameters.AddWithValue("@brgyId", Session.BarangayID);
                    cmd.Parameters.AddWithValue("@brgyName", Session.BarangayName);

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvArchResRec.DataSource = dt;
                    StyleGrid();
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading archived records: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StyleGrid()
        {
            dgvArchResRec.ReadOnly = true;
            dgvArchResRec.AllowUserToAddRows = false;
            dgvArchResRec.AllowUserToDeleteRows = false;
            dgvArchResRec.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvArchResRec.BackgroundColor = Color.White;
            dgvArchResRec.BorderStyle = BorderStyle.None;
            dgvArchResRec.RowHeadersVisible = false;
            dgvArchResRec.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvArchResRec.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 102, 204);
            dgvArchResRec.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvArchResRec.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 9, FontStyle.Bold);
            dgvArchResRec.ColumnHeadersHeight = 35;
            dgvArchResRec.EnableHeadersVisualStyles = false;
            dgvArchResRec.DefaultCellStyle.Font = new Font("Arial", 9);
            dgvArchResRec.RowTemplate.Height = 30;
            dgvArchResRec.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(220, 235, 255);
        }

        // =============================================
        // ROW CLICK ? populate fields + enable buttons
        // =============================================
        private void dgvArchResRec_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvArchResRec.Rows[e.RowIndex];

            selectedID = Convert.ToInt32(row.Cells["ID"].Value);

            string[] parts = row.Cells["Resident"].Value.ToString().Split(new char[] { ' ' }, 2);
            txtFirstName.Text = parts[0];
            txtLastName.Text = parts.Length > 1 ? parts[1] : "";

            cmbAge.SelectedItem = row.Cells["Age"].Value.ToString();
            cmbGender.SelectedItem = row.Cells["Gender"].Value.ToString();
            dtpBirthday.Value = Convert.ToDateTime(row.Cells["Birthday"].Value);

            CheckAndEnableButtons();
        }

        // =============================================
        // VALIDATE — enable buttons only when all filled
        // =============================================
        private void SetButtonState(bool enabled)
        {
            if (!enabled) { btnRestore.Enabled = false; btnDelete.Enabled = false; return; }
            CheckAndEnableButtons();
        }

        private void CheckAndEnableButtons()
        {
            bool ok = selectedID > 0
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

        private void ClearFields()
        {
            selectedID = 0;
            txtFirstName.Clear(); txtLastName.Clear();
            cmbAge.SelectedIndex = -1; cmbGender.SelectedIndex = -1;
            dtpBirthday.Value = DateTime.Now;
            SetButtonState(false);
        }

        private void txtArchHealthSearch_TextChanged(object sender, EventArgs e)
        { LoadArchivedRecords(txtArchHealthSearch.Text.Trim()); }

        // =============================================
        // RESTORE — reflects immediately in ManageRes/ViewRes
        // =============================================
        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (selectedID == 0) return;
            if (MessageBox.Show("Restore this record to active?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "UPDATE health_records SET is_archived = 0 WHERE ID = @id", conn);
                    cmd.Parameters.AddWithValue("@id", selectedID);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record restored!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadArchivedRecords();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =============================================
        // DELETE
        // =============================================
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedID == 0) return;
            if (MessageBox.Show("PERMANENTLY delete? Cannot be undone!", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "DELETE FROM health_records WHERE ID = @id", conn);
                    cmd.Parameters.AddWithValue("@id", selectedID);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record deleted!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadArchivedRecords();
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
        private void btnNext_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudBCapViewArch2()); }

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
    }
}