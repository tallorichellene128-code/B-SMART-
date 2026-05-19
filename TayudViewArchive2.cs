// ============================================================
//  B-SMART : TayudViewArchive2.cs
//  Shows ARCHIVED medicines and vaccines.
//  Supports: search, click-to-load, restore, and permanent delete.
//  btnPrev ? TayudViewArchive (health record archive).
// ============================================================
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BSMART
{
    public partial class TayudViewArchive2 : Form
    {
        private string connectionString = "Server=localhost;Database=bsmart_db;Uid=root;Pwd=;";
        private int selectedMedID = 0;
        private int selectedVaccID = 0;

        public TayudViewArchive2()
        {
            InitializeComponent();
            btnLogout.Click += (s, e) => SettingsNavigationService.OpenLogin(this);
        }

        // =============================================
        // FORM LOAD
        // =============================================
        private void TayudViewArchive2_Load(object sender, EventArgs e)
        {
            PopulateQtyCombo(cmbArchMedQty);
            PopulateQtyCombo(cmbArchVaccQty);
            LoadArchivedMedicines();
            LoadArchivedVaccines();
            ResetMedFields();
            ResetVaccFields();
        }

        private void PopulateQtyCombo(ComboBox cmb)
        {
            cmb.Items.Clear();
            for (int i = 1; i <= 999; i++)
                cmb.Items.Add(i.ToString());
        }

        // =============================================
        // BARANGAY FILTER HELPER
        // =============================================
        private string BarangayFilter(MySqlCommand cmd, string paramName = "@brgyId")
        {
            if (Session.IsMayor) return "";
            cmd.Parameters.AddWithValue(paramName, Session.BarangayID);
            return $" AND barangay_id = {paramName}";
        }

        // +------------------------------------------+
        // ¦          ARCHIVED MEDICINES              ¦
        // +------------------------------------------+

        private void LoadArchivedMedicines(string keyword = "")
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    BsmartIdHelper.EnsureCodes(conn);
                    MySqlCommand cmd = new MySqlCommand("", conn);
                    string filter = BarangayFilter(cmd, "@brgyIdMed");

                    string keyFilter = "";
                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        keyFilter = " AND name LIKE @kwMed COLLATE utf8mb4_0900_ai_ci";
                        cmd.Parameters.AddWithValue("@kwMed", "%" + keyword + "%");
                    }

                    cmd.CommandText = $@"SELECT id          AS '_DBID',
                                                COALESCE(NULLIF(medicine_code, ''), CAST(id AS CHAR)) AS 'ID',
                                                name        AS 'Medicine Name',
                                                quantity    AS 'Quantity',
                                                expiry_date AS 'Expiry Date'
                                         FROM medicines
                                         WHERE is_archived = 1{filter}{keyFilter}
                                         ORDER BY expiry_date ASC";

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvArchMedicines.DataSource = dt;
                    StyleGrid(dgvArchMedicines);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading archived medicines: " + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadArchivedVaccines(string keyword = "")
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    BsmartIdHelper.EnsureCodes(conn);
                    MySqlCommand cmd = new MySqlCommand("", conn);
                    string filter = BarangayFilter(cmd, "@brgyIdVacc");

                    string keyFilter = "";
                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        keyFilter = " AND name LIKE @kwVacc COLLATE utf8mb4_0900_ai_ci";
                        cmd.Parameters.AddWithValue("@kwVacc", "%" + keyword + "%");
                    }

                    cmd.CommandText = $@"SELECT id          AS '_DBID',
                                                COALESCE(NULLIF(vaccine_code, ''), CAST(id AS CHAR)) AS 'ID',
                                                name        AS 'Vaccine Name',
                                                quantity    AS 'Quantity',
                                                expiry_date AS 'Expiry Date'
                                         FROM vaccines
                                         WHERE is_archived = 1{filter}{keyFilter}
                                         ORDER BY expiry_date ASC";

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvArchVaccines.DataSource = dt;
                    StyleGrid(dgvArchVaccines);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading archived vaccines: " + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // -- SEARCH --------------------------------------------------------
        private void txtArchMedSearch_TextChanged(object sender, EventArgs e)
        {
            LoadArchivedMedicines(txtArchMedSearch.Text.Trim());
        }

        private void txtArchVaccSearch_TextChanged(object sender, EventArgs e)
        {
            LoadArchivedVaccines(txtArchVaccSearch.Text.Trim());
        }

        // -- MEDICINE GRID CLICK ? populate fields -------------------------
        private void dgvArchMedicines_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvArchMedicines.Rows[e.RowIndex];
            selectedMedID = Convert.ToInt32(row.Cells["_DBID"].Value);
            txtArchMed.Text = row.Cells["Medicine Name"].Value.ToString();
            cmbArchMedQty.SelectedItem = row.Cells["Quantity"].Value.ToString();
            if (cmbArchMedQty.SelectedIndex == -1) cmbArchMedQty.Text = row.Cells["Quantity"].Value.ToString();
            dtpArchExpiryMed.Value = Convert.ToDateTime(row.Cells["Expiry Date"].Value);
        }

        // -- VACCINE GRID CLICK ? populate fields --------------------------
        private void dgvArchVaccines_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvArchVaccines.Rows[e.RowIndex];
            selectedVaccID = Convert.ToInt32(row.Cells["_DBID"].Value);
            txtArchVacc.Text = row.Cells["Vaccine Name"].Value.ToString();
            cmbArchVaccQty.SelectedItem = row.Cells["Quantity"].Value.ToString();
            if (cmbArchVaccQty.SelectedIndex == -1) cmbArchVaccQty.Text = row.Cells["Quantity"].Value.ToString();
            dtpArchExpiryVacc.Value = Convert.ToDateTime(row.Cells["Expiry Date"].Value);
        }

        // +------------------------------------------+
        // ¦         MEDICINE RESTORE / DELETE        ¦
        // +------------------------------------------+

        private void btnRestoreMed_Click(object sender, EventArgs e)
        {
            if (selectedMedID == 0)
            {
                MessageBox.Show("Please select a medicine to restore.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Restore this medicine to active inventory?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "UPDATE medicines SET is_archived = 0 WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", selectedMedID);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Medicine restored!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetMedFields();
                    LoadArchivedMedicines();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteMed_Click(object sender, EventArgs e)
        {
            if (selectedMedID == 0)
            {
                MessageBox.Show("Please select a medicine to delete.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("PERMANENTLY delete this medicine? This cannot be undone!",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "DELETE FROM medicines WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", selectedMedID);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Medicine permanently deleted!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetMedFields();
                    LoadArchivedMedicines();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // +------------------------------------------+
        // ¦         VACCINE RESTORE / DELETE         ¦
        // +------------------------------------------+

        private void btnRestoreVacc_Click(object sender, EventArgs e)
        {
            if (selectedVaccID == 0)
            {
                MessageBox.Show("Please select a vaccine to restore.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Restore this vaccine to active inventory?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "UPDATE vaccines SET is_archived = 0 WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", selectedVaccID);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Vaccine restored!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetVaccFields();
                    LoadArchivedVaccines();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteVacc_Click(object sender, EventArgs e)
        {
            if (selectedVaccID == 0)
            {
                MessageBox.Show("Please select a vaccine to delete.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("PERMANENTLY delete this vaccine? This cannot be undone!",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "DELETE FROM vaccines WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", selectedVaccID);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Vaccine permanently deleted!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetVaccFields();
                    LoadArchivedVaccines();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // +------------------------------------------+
        // ¦           HELPERS & STYLING              ¦
        // +------------------------------------------+

        private void ResetMedFields()
        {
            selectedMedID = 0;
            txtArchMed.Clear();
            cmbArchMedQty.SelectedIndex = -1;
            dtpArchExpiryMed.Value = DateTime.Now;
        }

        private void ResetVaccFields()
        {
            selectedVaccID = 0;
            txtArchVacc.Clear();
            cmbArchVaccQty.SelectedIndex = -1;
            dtpArchExpiryVacc.Value = DateTime.Now;
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
            if (dgv.Columns.Contains("_DBID"))
                dgv.Columns["_DBID"].Visible = false;
        }

        // Stub handlers bound by designer
        private void txtArchMed_TextChanged(object sender, EventArgs e) { }
        private void txtArchVacc_TextChanged(object sender, EventArgs e) { }
        private void dtpArchExpiryMed_ValueChanged(object sender, EventArgs e) { }
        private void dtpArchExpiryVacc_ValueChanged(object sender, EventArgs e) { }

        // =============================================
        // NAVIGATION
        // =============================================
        private void btnPrev_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudViewArchive()); }

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

        private void cmbMedQty_SelectedIndexChanged(object sender, EventArgs e) { }
        private void cmbVaccQty_SelectedIndexChanged(object sender, EventArgs e) { }
    }
}



