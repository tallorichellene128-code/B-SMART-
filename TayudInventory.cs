// ============================================================
//  B-SMART : TayudInventory.cs
//  Full inventory management for Medicines and Vaccines.
//  All operations are scoped to Session.BarangayID
//  (Mayor sees and manages all barangays).
// ============================================================
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BSMART
{
    public partial class TayudInventory : Form
    {
        private string connectionString = "Server=localhost;Database=bsmart_db;Uid=root;Pwd=;";

        // Track selected row IDs
        private int selectedMedID = 0;
        private int selectedVaccID = 0;
        private string selectedMedIDs = "";
        private string selectedVaccIDs = "";

        public TayudInventory()
        {
            InitializeComponent();
            btnLogout.Click += (s, e) => SettingsNavigationService.OpenLogin(this);
        }

        // =============================================
        // FORM LOAD
        // =============================================
        private void TayudInventory_Load(object sender, EventArgs e)
        {
            // Populate quantity comboboxes 1–999
            PopulateQtyCombo(cmbMedQty);
            PopulateQtyCombo(cmbVaccQty);

            LoadMedicines();
            LoadVaccines();
            ResetMedFields();
            ResetVaccFields();
        }

        private void PopulateQtyCombo(ComboBox cmb)
        {
            cmb.Items.Clear();
            for (int i = 1; i <= 999; i++)
                cmb.Items.Add(i.ToString());
        }

        // +------------------------------------------+
        // ¦           BARANGAY FILTER HELPER         ¦
        // +------------------------------------------+
        private string BarangayFilter(MySqlCommand cmd, string tableAlias = "")
        {
            if (Session.IsMayor) return "";
            cmd.Parameters.AddWithValue("@brgyId", Session.BarangayID);
            string column = string.IsNullOrWhiteSpace(tableAlias) ? "barangay_id" : tableAlias + ".barangay_id";
            return " AND " + column + " = @brgyId";
        }

        // +------------------------------------------+
        // ¦               MEDICINES                  ¦
        // +------------------------------------------+

        private void LoadMedicines(string keyword = "")
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    BsmartIdHelper.EnsureCodes(conn);
                    MySqlCommand cmd = new MySqlCommand("", conn);
                    string filter = BarangayFilter(cmd, "i");

                    string keyFilter = "";
                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        keyFilter = @" AND (
                            COALESCE(i.medicine_code, CAST(i.id AS CHAR)) LIKE @kw
                            OR i.name LIKE @kw
                            OR CAST(COALESCE(i.quantity, 0) AS CHAR) LIKE @kw
                            OR DATE_FORMAT(i.expiry_date, '%d/%m/%Y') LIKE @kw
                            OR DATE_FORMAT(i.expiry_date, '%m/%d/%Y') LIKE @kw
                            OR DATE_FORMAT(i.expiry_date, '%Y-%m-%d') LIKE @kw
                            OR DATE_FORMAT(i.expiry_date, '%M %e, %Y') LIKE @kw
                            OR COALESCE(b.name, '') LIKE @kw
                            OR 'medicine' LIKE @kw
                            OR (CASE
                                WHEN i.expiry_date < CURDATE() THEN 'expired'
                                WHEN i.expiry_date <= DATE_ADD(CURDATE(), INTERVAL 30 DAY) THEN 'expiring soon'
                                ELSE 'available'
                            END) LIKE @kw
                            OR (CASE
                                WHEN COALESCE(i.quantity, 0) < @threshold THEN 'low stock'
                                ELSE 'in stock'
                            END) LIKE @kw
                        )";
                        cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");
                    }

                    cmd.Parameters.AddWithValue("@threshold", BsmartAppSettings.LowStockThreshold);

                    cmd.CommandText = $@"SELECT MIN(i.id) AS '_DBID',
                                                GROUP_CONCAT(i.id ORDER BY i.id SEPARATOR ',') AS '_DBIDS',
                                                COALESCE(MIN(NULLIF(i.medicine_code, '')), CAST(MIN(i.id) AS CHAR)) AS 'ID',
                                                MIN(i.name) AS 'Medicine Name',
                                                COALESCE(b.name, 'No barangay') AS 'Barangay',
                                                SUM(COALESCE(i.quantity, 0)) AS 'Quantity',
                                                DATE(i.expiry_date) AS 'Expiry Date',
                                                CASE
                                                    WHEN MIN(DATE(i.expiry_date)) < CURDATE() THEN 'Expired'
                                                    WHEN MIN(DATE(i.expiry_date)) <= DATE_ADD(CURDATE(), INTERVAL 30 DAY) THEN 'Expiring Soon'
                                                    WHEN SUM(COALESCE(i.quantity, 0)) < @threshold THEN 'Low Stock'
                                                    ELSE 'Available'
                                                END AS 'Status'
                                         FROM medicines i
                                         LEFT JOIN barangays b ON b.id = i.barangay_id
                                         WHERE i.is_archived = 0{filter}{keyFilter}
                                         GROUP BY i.barangay_id, COALESCE(b.name, 'No barangay'), LOWER(TRIM(i.name)), DATE(i.expiry_date)
                                         ORDER BY MIN(DATE(i.expiry_date)) ASC, MIN(i.name) ASC";

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvMedicines.DataSource = dt;
                    StyleGrid(dgvMedicines);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading medicines: " + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadVaccines(string keyword = "")
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    BsmartIdHelper.EnsureCodes(conn);
                    MySqlCommand cmd = new MySqlCommand("", conn);
                    string filter = BarangayFilter(cmd, "i");

                    string keyFilter = "";
                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        keyFilter = @" AND (
                            COALESCE(i.vaccine_code, CAST(i.id AS CHAR)) LIKE @kw
                            OR i.name LIKE @kw
                            OR CAST(COALESCE(i.quantity, 0) AS CHAR) LIKE @kw
                            OR DATE_FORMAT(i.expiry_date, '%d/%m/%Y') LIKE @kw
                            OR DATE_FORMAT(i.expiry_date, '%m/%d/%Y') LIKE @kw
                            OR DATE_FORMAT(i.expiry_date, '%Y-%m-%d') LIKE @kw
                            OR DATE_FORMAT(i.expiry_date, '%M %e, %Y') LIKE @kw
                            OR COALESCE(b.name, '') LIKE @kw
                            OR 'vaccine' LIKE @kw
                            OR (CASE
                                WHEN i.expiry_date < CURDATE() THEN 'expired'
                                WHEN i.expiry_date <= DATE_ADD(CURDATE(), INTERVAL 30 DAY) THEN 'expiring soon'
                                ELSE 'available'
                            END) LIKE @kw
                            OR (CASE
                                WHEN COALESCE(i.quantity, 0) < @threshold THEN 'low stock'
                                ELSE 'in stock'
                            END) LIKE @kw
                        )";
                        cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");
                    }

                    cmd.Parameters.AddWithValue("@threshold", BsmartAppSettings.LowStockThreshold);

                    cmd.CommandText = $@"SELECT MIN(i.id) AS '_DBID',
                                                GROUP_CONCAT(i.id ORDER BY i.id SEPARATOR ',') AS '_DBIDS',
                                                COALESCE(MIN(NULLIF(i.vaccine_code, '')), CAST(MIN(i.id) AS CHAR)) AS 'ID',
                                                MIN(i.name) AS 'Vaccine Name',
                                                COALESCE(b.name, 'No barangay') AS 'Barangay',
                                                SUM(COALESCE(i.quantity, 0)) AS 'Quantity',
                                                DATE(i.expiry_date) AS 'Expiry Date',
                                                CASE
                                                    WHEN MIN(DATE(i.expiry_date)) < CURDATE() THEN 'Expired'
                                                    WHEN MIN(DATE(i.expiry_date)) <= DATE_ADD(CURDATE(), INTERVAL 30 DAY) THEN 'Expiring Soon'
                                                    WHEN SUM(COALESCE(i.quantity, 0)) < @threshold THEN 'Low Stock'
                                                    ELSE 'Available'
                                                END AS 'Status'
                                         FROM vaccines i
                                         LEFT JOIN barangays b ON b.id = i.barangay_id
                                         WHERE i.is_archived = 0{filter}{keyFilter}
                                         GROUP BY i.barangay_id, COALESCE(b.name, 'No barangay'), LOWER(TRIM(i.name)), DATE(i.expiry_date)
                                         ORDER BY MIN(DATE(i.expiry_date)) ASC, MIN(i.name) ASC";

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvVaccines.DataSource = dt;
                    StyleGrid(dgvVaccines);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading vaccines: " + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // -- MEDICINE SEARCH -----------------------------------------------
        private void txtMedSearch_TextChanged(object sender, EventArgs e)
        {
            LoadMedicines(txtMedSearch.Text.Trim());
        }

        // -- VACCINE SEARCH ------------------------------------------------
        private void txtVaccSearch_TextChanged(object sender, EventArgs e)
        {
            LoadVaccines(txtVaccSearch.Text.Trim());
        }

        // -- MEDICINE GRID CLICK ? populate fields -------------------------
        private void dgvMedicines_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvMedicines.Rows[e.RowIndex];
            selectedMedID = Convert.ToInt32(row.Cells["_DBID"].Value);
            selectedMedIDs = GetGroupedIds(row, selectedMedID);
            txtMed.Text = row.Cells["Medicine Name"].Value.ToString();
            cmbMedQty.SelectedItem = row.Cells["Quantity"].Value.ToString();
            if (cmbMedQty.SelectedIndex == -1) cmbMedQty.Text = row.Cells["Quantity"].Value.ToString();
            dtpExpiryMed.Value = Convert.ToDateTime(row.Cells["Expiry Date"].Value);
        }

        // -- VACCINE GRID CLICK ? populate fields --------------------------
        private void dgvVaccines_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow row = dgvVaccines.Rows[e.RowIndex];
            selectedVaccID = Convert.ToInt32(row.Cells["_DBID"].Value);
            selectedVaccIDs = GetGroupedIds(row, selectedVaccID);
            txtVaccines.Text = row.Cells["Vaccine Name"].Value.ToString();
            cmbVaccQty.SelectedItem = row.Cells["Quantity"].Value.ToString();
            if (cmbVaccQty.SelectedIndex == -1) cmbVaccQty.Text = row.Cells["Quantity"].Value.ToString();
            dtpExpiryVacc.Value = Convert.ToDateTime(row.Cells["Expiry Date"].Value);
        }

        // -- ADD MEDICINE --------------------------------------------------
        private void btnAddMed_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMed.Text))
            {
                MessageBox.Show("Please enter medicine name.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(cmbMedQty.Text) || !int.TryParse(cmbMedQty.Text, out int qty) || qty < 0)
            {
                MessageBox.Show("Please enter a valid quantity.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    BsmartIdHelper.EnsureCodes(conn);
                    int barangayId = Session.BarangayID;

                    using (MySqlCommand existing = new MySqlCommand(@"
                        SELECT id
                        FROM medicines
                        WHERE COALESCE(is_archived, 0) = 0
                          AND LOWER(TRIM(name)) = LOWER(TRIM(@name))
                          AND DATE(expiry_date) = @expiry
                          AND ((barangay_id IS NULL AND @brgyId IS NULL) OR barangay_id = @brgyId)
                        ORDER BY id ASC
                        LIMIT 1", conn))
                    {
                        existing.Parameters.AddWithValue("@name", txtMed.Text.Trim());
                        existing.Parameters.AddWithValue("@expiry", dtpExpiryMed.Value.Date);
                        existing.Parameters.AddWithValue("@brgyId", barangayId == 0 ? (object)DBNull.Value : barangayId);
                        object existingId = existing.ExecuteScalar();
                        if (existingId != null && existingId != DBNull.Value)
                        {
                            using MySqlCommand updateExisting = new MySqlCommand(@"
                                UPDATE medicines
                                SET quantity = COALESCE(quantity, 0) + @qty
                                WHERE id = @id", conn);
                            updateExisting.Parameters.AddWithValue("@qty", qty);
                            updateExisting.Parameters.AddWithValue("@id", Convert.ToInt32(existingId));
                            updateExisting.ExecuteNonQuery();
                            BsmartAuditService.Log("Update Medicine", "medicines", Convert.ToInt32(existingId), txtMed.Text.Trim());
                            MessageBox.Show("Medicine stock updated!", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ResetMedFields();
                            LoadMedicines();
                            BsmartNotificationService.RefreshOpenIndicators(qty < BsmartAppSettings.LowStockThreshold || dtpExpiryMed.Value.Date <= DateTime.Today.AddDays(7));
                            return;
                        }
                    }

                    string medicineCode = BsmartIdHelper.NextMedicineCode(conn, barangayId);
                    MySqlCommand cmd = new MySqlCommand(@"
                        INSERT INTO medicines (medicine_code, name, quantity, expiry_date, is_archived, barangay_id)
                        VALUES (@code, @name, @qty, @expiry, 0, @brgyId)", conn);
                    cmd.Parameters.AddWithValue("@code", medicineCode);
                    cmd.Parameters.AddWithValue("@name", txtMed.Text.Trim());
                    cmd.Parameters.AddWithValue("@qty", qty);
                    cmd.Parameters.AddWithValue("@expiry", dtpExpiryMed.Value.Date);
                    cmd.Parameters.AddWithValue("@brgyId",
                        barangayId == 0 ? (object)DBNull.Value : barangayId);
                    cmd.ExecuteNonQuery();
                    BsmartAuditService.Log("Add Medicine", "medicines", medicineCode, txtMed.Text.Trim());
                    MessageBox.Show("Medicine added!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetMedFields();
                    LoadMedicines();
                    BsmartNotificationService.RefreshOpenIndicators(qty < 10 || dtpExpiryMed.Value.Date <= DateTime.Today.AddDays(7));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // -- ARCHIVE MEDICINE ----------------------------------------------
        private void btnArchiveMed_Click(object sender, EventArgs e)
        {
            if (selectedMedID == 0)
            {
                MessageBox.Show("Please select a medicine first.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Archive this medicine?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "UPDATE medicines SET is_archived = 1 WHERE FIND_IN_SET(CAST(id AS CHAR), @ids)", conn);
                    cmd.Parameters.AddWithValue("@ids", string.IsNullOrWhiteSpace(selectedMedIDs) ? selectedMedID.ToString() : selectedMedIDs);
                    cmd.ExecuteNonQuery();
                    BsmartAuditService.Log("Archive Medicine", "medicines", selectedMedID, txtMed.Text.Trim());
                    MessageBox.Show("Medicine archived!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetMedFields();
                    LoadMedicines();
                    BsmartNotificationService.RefreshOpenIndicators(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // -- VIEW EXPIRED MEDICINES ----------------------------------------
        private void btnViewExpMed_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    BsmartIdHelper.EnsureCodes(conn);
                    MySqlCommand cmd = new MySqlCommand("", conn);
                    string filter = BarangayFilter(cmd);
                    BsmartIdHelper.EnsureCodes(conn);
                    cmd.CommandText = $@"SELECT id AS '_DBID',
                                                COALESCE(NULLIF(medicine_code, ''), CAST(id AS CHAR)) AS 'ID',
                                                name AS 'Medicine Name',
                                                quantity AS 'Quantity', expiry_date AS 'Expiry Date'
                                         FROM medicines
                                         WHERE is_archived = 0
                                           AND expiry_date < CURDATE(){filter}
                                         ORDER BY expiry_date ASC";

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvMedicines.DataSource = dt;
                    StyleGrid(dgvMedicines);

                    MessageBox.Show($"{dt.Rows.Count} expired medicine(s) found.",
                        "Expired Medicines", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // -- REMOVE (PERMANENT DELETE) MEDICINE ---------------------------
        private void btnRemoveMed_Click(object sender, EventArgs e)
        {
            if (selectedMedID == 0)
            {
                MessageBox.Show("Please select a medicine first.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Permanently delete this medicine? This cannot be undone!",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "DELETE FROM medicines WHERE FIND_IN_SET(CAST(id AS CHAR), @ids)", conn);
                    cmd.Parameters.AddWithValue("@ids", string.IsNullOrWhiteSpace(selectedMedIDs) ? selectedMedID.ToString() : selectedMedIDs);
                    cmd.ExecuteNonQuery();
                    BsmartAuditService.Log("Delete Medicine", "medicines", selectedMedID, txtMed.Text.Trim());
                    MessageBox.Show("Medicine permanently deleted!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetMedFields();
                    LoadMedicines();
                    BsmartNotificationService.RefreshOpenIndicators(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // +------------------------------------------+
        // ¦               VACCINES                   ¦
        // +------------------------------------------+

        // -- ADD VACCINE ---------------------------------------------------
        private void btnAddVacc_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtVaccines.Text))
            {
                MessageBox.Show("Please enter vaccine name.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(cmbVaccQty.Text) || !int.TryParse(cmbVaccQty.Text, out int qty) || qty < 0)
            {
                MessageBox.Show("Please enter a valid quantity.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    BsmartIdHelper.EnsureCodes(conn);
                    int barangayId = Session.BarangayID;

                    using (MySqlCommand existing = new MySqlCommand(@"
                        SELECT id
                        FROM vaccines
                        WHERE COALESCE(is_archived, 0) = 0
                          AND LOWER(TRIM(name)) = LOWER(TRIM(@name))
                          AND DATE(expiry_date) = @expiry
                          AND ((barangay_id IS NULL AND @brgyId IS NULL) OR barangay_id = @brgyId)
                        ORDER BY id ASC
                        LIMIT 1", conn))
                    {
                        existing.Parameters.AddWithValue("@name", txtVaccines.Text.Trim());
                        existing.Parameters.AddWithValue("@expiry", dtpExpiryVacc.Value.Date);
                        existing.Parameters.AddWithValue("@brgyId", barangayId == 0 ? (object)DBNull.Value : barangayId);
                        object existingId = existing.ExecuteScalar();
                        if (existingId != null && existingId != DBNull.Value)
                        {
                            using MySqlCommand updateExisting = new MySqlCommand(@"
                                UPDATE vaccines
                                SET quantity = COALESCE(quantity, 0) + @qty
                                WHERE id = @id", conn);
                            updateExisting.Parameters.AddWithValue("@qty", qty);
                            updateExisting.Parameters.AddWithValue("@id", Convert.ToInt32(existingId));
                            updateExisting.ExecuteNonQuery();
                            BsmartAuditService.Log("Update Vaccine", "vaccines", Convert.ToInt32(existingId), txtVaccines.Text.Trim());
                            MessageBox.Show("Vaccine stock updated!", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ResetVaccFields();
                            LoadVaccines();
                            BsmartNotificationService.RefreshOpenIndicators(qty < BsmartAppSettings.LowStockThreshold || dtpExpiryVacc.Value.Date <= DateTime.Today.AddDays(7));
                            return;
                        }
                    }

                    string vaccineCode = BsmartIdHelper.NextVaccineCode(conn, barangayId);
                    MySqlCommand cmd = new MySqlCommand(@"
                        INSERT INTO vaccines (vaccine_code, name, quantity, expiry_date, is_archived, barangay_id)
                        VALUES (@code, @name, @qty, @expiry, 0, @brgyId)", conn);
                    cmd.Parameters.AddWithValue("@code", vaccineCode);
                    cmd.Parameters.AddWithValue("@name", txtVaccines.Text.Trim());
                    cmd.Parameters.AddWithValue("@qty", qty);
                    cmd.Parameters.AddWithValue("@expiry", dtpExpiryVacc.Value.Date);
                    cmd.Parameters.AddWithValue("@brgyId",
                        barangayId == 0 ? (object)DBNull.Value : barangayId);
                    cmd.ExecuteNonQuery();
                    BsmartAuditService.Log("Add Vaccine", "vaccines", vaccineCode, txtVaccines.Text.Trim());
                    MessageBox.Show("Vaccine added!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetVaccFields();
                    LoadVaccines();
                    BsmartNotificationService.RefreshOpenIndicators(qty < 10 || dtpExpiryVacc.Value.Date <= DateTime.Today.AddDays(7));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // -- ARCHIVE VACCINE -----------------------------------------------
        private void btnArchiveVacc_Click(object sender, EventArgs e)
        {
            if (selectedVaccID == 0)
            {
                MessageBox.Show("Please select a vaccine first.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Archive this vaccine?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "UPDATE vaccines SET is_archived = 1 WHERE FIND_IN_SET(CAST(id AS CHAR), @ids)", conn);
                    cmd.Parameters.AddWithValue("@ids", string.IsNullOrWhiteSpace(selectedVaccIDs) ? selectedVaccID.ToString() : selectedVaccIDs);
                    cmd.ExecuteNonQuery();
                    BsmartAuditService.Log("Archive Vaccine", "vaccines", selectedVaccID, txtVaccines.Text.Trim());
                    MessageBox.Show("Vaccine archived!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetVaccFields();
                    LoadVaccines();
                    BsmartNotificationService.RefreshOpenIndicators(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // -- VIEW EXPIRED VACCINES -----------------------------------------
        private void btnViewExpVacc_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    BsmartIdHelper.EnsureCodes(conn);
                    MySqlCommand cmd = new MySqlCommand("", conn);
                    string filter = BarangayFilter(cmd);
                    BsmartIdHelper.EnsureCodes(conn);
                    cmd.CommandText = $@"SELECT id AS '_DBID',
                                                COALESCE(NULLIF(vaccine_code, ''), CAST(id AS CHAR)) AS 'ID',
                                                name AS 'Vaccine Name',
                                                quantity AS 'Quantity', expiry_date AS 'Expiry Date'
                                         FROM vaccines
                                         WHERE is_archived = 0
                                           AND expiry_date < CURDATE(){filter}
                                         ORDER BY expiry_date ASC";

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvVaccines.DataSource = dt;
                    StyleGrid(dgvVaccines);

                    MessageBox.Show($"{dt.Rows.Count} expired vaccine(s) found.",
                        "Expired Vaccines", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // -- REMOVE (PERMANENT DELETE) VACCINE ----------------------------
        private void btnRemoveVacc_Click(object sender, EventArgs e)
        {
            if (selectedVaccID == 0)
            {
                MessageBox.Show("Please select a vaccine first.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Permanently delete this vaccine? This cannot be undone!",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "DELETE FROM vaccines WHERE FIND_IN_SET(CAST(id AS CHAR), @ids)", conn);
                    cmd.Parameters.AddWithValue("@ids", string.IsNullOrWhiteSpace(selectedVaccIDs) ? selectedVaccID.ToString() : selectedVaccIDs);
                    cmd.ExecuteNonQuery();
                    BsmartAuditService.Log("Delete Vaccine", "vaccines", selectedVaccID, txtVaccines.Text.Trim());
                    MessageBox.Show("Vaccine permanently deleted!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetVaccFields();
                    LoadVaccines();
                    BsmartNotificationService.RefreshOpenIndicators(false);
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
            selectedMedIDs = "";
            txtMed.Clear();
            cmbMedQty.SelectedIndex = -1;
            dtpExpiryMed.Value = DateTime.Now;
        }

        private void ResetVaccFields()
        {
            selectedVaccID = 0;
            selectedVaccIDs = "";
            txtVaccines.Clear();
            cmbVaccQty.SelectedIndex = -1;
            dtpExpiryVacc.Value = DateTime.Now;
        }

        private static string GetGroupedIds(DataGridViewRow row, int fallbackId)
        {
            if (row.DataGridView.Columns.Contains("_DBIDS"))
            {
                string ids = Convert.ToString(row.Cells["_DBIDS"].Value)?.Trim() ?? "";
                if (!string.IsNullOrWhiteSpace(ids))
                    return ids;
            }

            return fallbackId.ToString();
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
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 102, 204);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 9, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 48;
            dgv.EnableHeadersVisualStyles = false;
            dgv.DefaultCellStyle.Font = new Font("Arial", 9);
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dgv.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgv.RowTemplate.Height = 32;
            foreach (DataGridViewRow row in dgv.Rows)
                row.Height = 32;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(220, 235, 255);
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                if (column.Name.StartsWith("_", StringComparison.Ordinal))
                {
                    column.Visible = false;
                    continue;
                }

                column.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
                column.ToolTipText = column.HeaderText;
            }

            ApplyInventoryColumnWidths(dgv);

            // Highlight rows expiring within 30 days in orange
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.Cells["Expiry Date"].Value != null)
                {
                    DateTime exp = Convert.ToDateTime(row.Cells["Expiry Date"].Value);
                    if (exp < DateTime.Today)
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 200, 200); // red = expired
                    else if (exp <= DateTime.Today.AddDays(30))
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 235, 180); // orange = expiring soon
                }
            }
        }

        private void ApplyInventoryColumnWidths(DataGridView dgv)
        {
            if (dgv.Columns.Contains("ID"))
                dgv.Columns["ID"].FillWeight = 70;

            string itemColumn = dgv.Columns.Contains("Medicine Name")
                ? "Medicine Name"
                : dgv.Columns.Contains("Vaccine Name") ? "Vaccine Name" : "";

            if (!string.IsNullOrEmpty(itemColumn))
                dgv.Columns[itemColumn].FillWeight = 170;

            if (dgv.Columns.Contains("Barangay"))
                dgv.Columns["Barangay"].FillWeight = 115;

            if (dgv.Columns.Contains("Quantity"))
                dgv.Columns["Quantity"].FillWeight = 85;

            if (dgv.Columns.Contains("Expiry Date"))
            {
                dgv.Columns["Expiry Date"].FillWeight = 120;
                dgv.Columns["Expiry Date"].DefaultCellStyle.Format = "dd/MM/yyyy";
            }

            if (dgv.Columns.Contains("Status"))
                dgv.Columns["Status"].FillWeight = 110;
        }

        private void txtMed_TextChanged(object sender, EventArgs e) { }
        private void txtVaccines_TextChanged(object sender, EventArgs e) { }
        private void dtpExpiryMed_ValueChanged(object sender, EventArgs e) { }
        private void dtpExpiryVacc_ValueChanged(object sender, EventArgs e) { }

        // +------------------------------------------+
        // ¦              NAVIGATION                  ¦
        // +------------------------------------------+
        private void btnInventory_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudInventory()); }

        private void btnManageRecord_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudManageHealth()); }

        private void btnViewRecord_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudViewHealth()); }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudGenerateReport()); }

        private void btnViewArchive_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudViewArchive()); }

        private void btnAdminDash_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudLGU()); }

        private void btnViewApp_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudViewAppointments()); }

        private void cmbMedQty_SelectedIndexChanged(object sender, EventArgs e) { }
        private void cmbVaccQty_SelectedIndexChanged(object sender, EventArgs e) { }

        private void btnUpdateMed_Click(object sender, EventArgs e)
        {
            if (selectedMedID == 0)
            {
                MessageBox.Show("Please select a medicine first.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtMed.Text))
            {
                MessageBox.Show("Please enter medicine name.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(cmbMedQty.Text) || !int.TryParse(cmbMedQty.Text, out int qty) || qty < 0)
            {
                MessageBox.Show("Please enter a valid quantity.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(@"
                        UPDATE medicines
                        SET name = @name,
                            quantity = @qty,
                            expiry_date = @expiry
                        WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@name", txtMed.Text.Trim());
                    cmd.Parameters.AddWithValue("@qty", qty);
                    cmd.Parameters.AddWithValue("@expiry", dtpExpiryMed.Value.Date);
                    cmd.Parameters.AddWithValue("@id", selectedMedID);
                    cmd.ExecuteNonQuery();
                    using (MySqlCommand archiveDuplicates = new MySqlCommand(@"
                        UPDATE medicines
                        SET is_archived = 1
                        WHERE id <> @keepId
                          AND FIND_IN_SET(CAST(id AS CHAR), @ids)", conn))
                    {
                        archiveDuplicates.Parameters.AddWithValue("@keepId", selectedMedID);
                        archiveDuplicates.Parameters.AddWithValue("@ids", string.IsNullOrWhiteSpace(selectedMedIDs) ? selectedMedID.ToString() : selectedMedIDs);
                        archiveDuplicates.ExecuteNonQuery();
                    }
                    BsmartAuditService.Log("Update Medicine", "medicines", selectedMedID, txtMed.Text.Trim());

                    MessageBox.Show("Medicine updated!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetMedFields();
                    LoadMedicines();
                    BsmartNotificationService.RefreshOpenIndicators(qty < 10 || dtpExpiryMed.Value.Date <= DateTime.Today.AddDays(7));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating medicine: " + ex.Message, "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateVacc_Click(object sender, EventArgs e)
        {
            if (selectedVaccID == 0)
            {
                MessageBox.Show("Please select a vaccine first.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtVaccines.Text))
            {
                MessageBox.Show("Please enter vaccine name.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(cmbVaccQty.Text) || !int.TryParse(cmbVaccQty.Text, out int qty) || qty < 0)
            {
                MessageBox.Show("Please enter a valid quantity.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(@"
                        UPDATE vaccines
                        SET name = @name,
                            quantity = @qty,
                            expiry_date = @expiry
                        WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@name", txtVaccines.Text.Trim());
                    cmd.Parameters.AddWithValue("@qty", qty);
                    cmd.Parameters.AddWithValue("@expiry", dtpExpiryVacc.Value.Date);
                    cmd.Parameters.AddWithValue("@id", selectedVaccID);
                    cmd.ExecuteNonQuery();
                    using (MySqlCommand archiveDuplicates = new MySqlCommand(@"
                        UPDATE vaccines
                        SET is_archived = 1
                        WHERE id <> @keepId
                          AND FIND_IN_SET(CAST(id AS CHAR), @ids)", conn))
                    {
                        archiveDuplicates.Parameters.AddWithValue("@keepId", selectedVaccID);
                        archiveDuplicates.Parameters.AddWithValue("@ids", string.IsNullOrWhiteSpace(selectedVaccIDs) ? selectedVaccID.ToString() : selectedVaccIDs);
                        archiveDuplicates.ExecuteNonQuery();
                    }
                    BsmartAuditService.Log("Update Vaccine", "vaccines", selectedVaccID, txtVaccines.Text.Trim());

                    MessageBox.Show("Vaccine updated!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetVaccFields();
                    LoadVaccines();
                    BsmartNotificationService.RefreshOpenIndicators(qty < 10 || dtpExpiryVacc.Value.Date <= DateTime.Today.AddDays(7));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating vaccine: " + ex.Message, "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}









