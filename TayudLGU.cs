using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BSMART
{
    public partial class TayudLGU : Form
    {
        private string connectionString = "Server=localhost;Database=bsmart_db;Uid=root;Pwd=;";
        public TayudLGU()
        {
            InitializeComponent();
            btnNotification.Click -= btnNotification_Click;
            BsmartNotificationService.Attach(btnNotification, this);
        }

        // =============================================
        // FORM LOAD
        // =============================================
        private void TayudLGU_Load(object sender, EventArgs e)
        {
            this.Text = Session.IsMayor
                ? "B-SMART â€” Mayor Dashboard (All Barangays)"
                : $"B-SMART â€” {Session.BarangayName} Dashboard";

            LoadRecentHealthRecords();
            LoadSummaryStats();
            BsmartNotificationService.RefreshIndicator(this, btnNotification, showToast: true);
        }

        // =============================================
        // BARANGAY FILTER HELPER
        // =============================================
        private string BarangayFilter(MySqlCommand cmd)
        {
            if (Session.IsMayor)
                return "";

            cmd.Parameters.AddWithValue("@brgyId", Session.BarangayID);
            cmd.Parameters.AddWithValue("@brgyName", Session.BarangayName);
            return " AND (barangay_id = @brgyId OR barangay_id IN (SELECT id FROM barangays WHERE name = @brgyName))";
        }

        // =============================================
        // LOAD RECENT HEALTH RECORDS
        // =============================================
        private void LoadRecentHealthRecords()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    BsmartIdHelper.EnsureCodes(conn);

                    MySqlCommand cmd = new MySqlCommand("", conn);
                    string healthFilter = BarangayFilter(cmd);

                    cmd.CommandText = $@"SELECT
                        h.ID AS '_DBID',
                        COALESCE(h.resident_code, CAST(h.ID AS CHAR)) COLLATE utf8mb4_unicode_ci AS 'ID',
                        CONCAT(h.first_name, ' ', h.last_name) COLLATE utf8mb4_unicode_ci AS 'Resident',
                        CASE WHEN UPPER(TRIM(COALESCE(h.Diagnosis, ''))) = 'N/A' THEN '' ELSE COALESCE(h.Diagnosis, '') END COLLATE utf8mb4_unicode_ci AS 'Diagnosis',
                        CASE WHEN UPPER(TRIM(COALESCE(h.Diagnosis, ''))) = 'N/A' THEN '' ELSE COALESCE(h.Treatment, '') END COLLATE utf8mb4_unicode_ci AS 'Treatment',
                        h.Date AS 'Date'
                    FROM health_records h
                    WHERE h.is_archived = 0
                      AND h.Diagnosis IS NOT NULL
                      AND TRIM(h.Diagnosis) <> ''
                      AND UPPER(TRIM(h.Diagnosis)) <> 'N/A'{healthFilter}
                    ORDER BY h.Date DESC, CONCAT(h.first_name, ' ', h.last_name) ASC";

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvRecentHealth.DataSource = dt;
                    StyleRecentGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading recent records: " + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =============================================
        // STYLE RECENT HEALTH DATAGRIDVIEW
        // =============================================
        private void StyleRecentGrid()
        {
            dgvRecentHealth.ReadOnly = true;
            dgvRecentHealth.AllowUserToAddRows = false;
            dgvRecentHealth.AllowUserToDeleteRows = false;
            dgvRecentHealth.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRecentHealth.BackgroundColor = Color.White;
            dgvRecentHealth.BorderStyle = BorderStyle.None;
            dgvRecentHealth.RowHeadersVisible = false;
            dgvRecentHealth.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvRecentHealth.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 102, 204);
            dgvRecentHealth.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvRecentHealth.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 9, FontStyle.Bold);
            dgvRecentHealth.ColumnHeadersHeight = 35;
            dgvRecentHealth.EnableHeadersVisualStyles = false;

            dgvRecentHealth.DefaultCellStyle.Font = new Font("Arial", 9);
            dgvRecentHealth.RowTemplate.Height = 30;
            dgvRecentHealth.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(220, 235, 255);

            if (dgvRecentHealth.Columns.Count > 0)
            {
                if (dgvRecentHealth.Columns.Contains("_DBID"))
                    dgvRecentHealth.Columns["_DBID"].Visible = false;
                dgvRecentHealth.Columns["ID"].FillWeight = 30;
                dgvRecentHealth.Columns["Resident"].FillWeight = 100;
                dgvRecentHealth.Columns["Diagnosis"].FillWeight = 120;
                dgvRecentHealth.Columns["Treatment"].FillWeight = 200;
                dgvRecentHealth.Columns["Date"].FillWeight = 80;
            }
        }

        // =============================================
        // LOAD SUMMARY STATS
        // =============================================
        private void LoadSummaryStats()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string filter = Session.IsMayor
                        ? ""
                        : " AND (barangay_id = @brgyId OR barangay_id IN (SELECT id FROM barangays WHERE name = @brgyName))";

                    MySqlCommand totalCmd = new MySqlCommand(
                        $"SELECT COUNT(*) FROM health_records WHERE is_archived = 0 AND Diagnosis IS NOT NULL AND TRIM(Diagnosis) <> '' AND UPPER(TRIM(Diagnosis)) <> 'N/A'{filter}", conn);
                    if (!Session.IsMayor)
                    {
                        totalCmd.Parameters.AddWithValue("@brgyId", Session.BarangayID);
                        totalCmd.Parameters.AddWithValue("@brgyName", Session.BarangayName);
                    }
                    lblTotalRecords.Text = Convert.ToInt32(totalCmd.ExecuteScalar()).ToString();

                    MySqlCommand commonCmd = new MySqlCommand(
                        $@"SELECT Diagnosis FROM health_records
                           WHERE is_archived = 0
                             AND Diagnosis IS NOT NULL
                             AND TRIM(Diagnosis) <> ''
                             AND UPPER(TRIM(Diagnosis)) <> 'N/A'{filter}
                           GROUP BY Diagnosis ORDER BY COUNT(*) DESC LIMIT 1", conn);
                    if (!Session.IsMayor)
                    {
                        commonCmd.Parameters.AddWithValue("@brgyId", Session.BarangayID);
                        commonCmd.Parameters.AddWithValue("@brgyName", Session.BarangayName);
                    }
                    object result = commonCmd.ExecuteScalar();
                    lblMostCommon.Text = result != null ? result.ToString() : "N/A";
                    lblTotalResidents.Text = Session.IsMayor
                        ? BsmartResidentCountService.CountResidents().ToString()
                        : BsmartResidentCountService.CountResidents(Session.BarangayName, Session.BarangayID).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading statistics: " + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =============================================
        // BELL BUTTON
        // =============================================
        private void btnNotification_Click(object sender, EventArgs e)
        {
            BsmartNotificationService.Show(this, btnNotification);
        }

        // =============================================
        // NOTIFICATION DB LOADER
        // =============================================
        private List<NotificationData> LoadNotificationsFromDB()
        {
            var list = new List<NotificationData>();

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string brgyFilter = Session.IsMayor ? "" :
                        " AND (barangay_id = @brgyId OR barangay_id IN " +
                        "(SELECT id FROM barangays WHERE name = @brgyName))";

                    // â”€â”€ 1. Health records added TODAY â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
                    try
                    {
                        using (var cmd = new MySqlCommand(
                            $@"SELECT COUNT(*) FROM health_records
                       WHERE is_archived = 0
                         AND DATE(Date) = CURDATE()
                         AND Diagnosis IS NOT NULL
                         AND TRIM(Diagnosis) <> ''
                         AND UPPER(TRIM(Diagnosis)) <> 'N/A'
                         {brgyFilter}", conn))
                        {
                            AddBarangayParams(cmd);
                            int count = Convert.ToInt32(cmd.ExecuteScalar());
                            if (count > 0)
                            {
                                list.Add(new NotificationData
                                {
                                    Initial = "H",
                                    AvatarColor = Color.FromArgb(220, 80, 80),
                                    Message = $"{count} new health record{(count > 1 ? "s" : "")} submitted today.",
                                    TimeAgo = "Today",
                                    IsUnread = true
                                });
                            }
                        }
                    }
                    catch { /* health_records query failed â€” skip */ }

                    // â”€â”€ 2. Low inventory items â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
                    // !! Replace 'inventory', 'item_name', 'quantity'
                    // !! with your actual table/column names if different
                    try
                    {
                        using (var cmd = new MySqlCommand(
                            @"SELECT item_name, quantity
                      FROM inventory
                      WHERE quantity <= 5
                      ORDER BY quantity ASC
                      LIMIT 5", conn))
                        {
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string itemName = reader.GetString(0);
                                    int qty = reader.GetInt32(1);
                                    list.Add(new NotificationData
                                    {
                                        Initial = "I",
                                        AvatarColor = Color.FromArgb(60, 160, 100),
                                        Message = $"Low stock: '{itemName}' has only {qty} " +
                                                  $"unit{(qty == 1 ? "" : "s")} left.",
                                        TimeAgo = "Now",
                                        IsUnread = true
                                    });
                                }
                            }
                        }
                    }
                    catch { /* inventory query failed â€” skip */ }

                    // â”€â”€ 3. Appointments scheduled for TODAY â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
                    // !! Replace 'appointments', 'appointment_date', 'status'
                    // !! with your actual table/column names if different
                    try
                    {
                        using (var cmd = new MySqlCommand(
                            $@"SELECT COUNT(*) FROM appointments
                       WHERE DATE(appointment_date) = CURDATE()
                         AND status NOT IN ('Cancelled','Done')
                         {brgyFilter}", conn))
                        {
                            AddBarangayParams(cmd);
                            int count = Convert.ToInt32(cmd.ExecuteScalar());
                            if (count > 0)
                            {
                                list.Add(new NotificationData
                                {
                                    Initial = "A",
                                    AvatarColor = Color.FromArgb(80, 120, 200),
                                    Message = $"{count} appointment{(count > 1 ? "s" : "")} scheduled for today.",
                                    TimeAgo = "Today",
                                    IsUnread = true
                                });
                            }
                        }
                    }
                    catch { /* appointments query failed â€” skip */ }

                    // â”€â”€ 4. Health records from this week (earlier) â”€â”€â”€â”€â”€â”€â”€â”€
                    try
                    {
                        using (var cmd = new MySqlCommand(
                            $@"SELECT COUNT(*) FROM health_records
                       WHERE is_archived = 0
                         AND DATE(Date) BETWEEN
                             DATE_SUB(CURDATE(), INTERVAL 7 DAY)
                             AND DATE_SUB(CURDATE(), INTERVAL 1 DAY)
                         AND Diagnosis IS NOT NULL
                         AND TRIM(Diagnosis) <> ''
                         AND UPPER(TRIM(Diagnosis)) <> 'N/A'
                         {brgyFilter}", conn))
                        {
                            AddBarangayParams(cmd);
                            int count = Convert.ToInt32(cmd.ExecuteScalar());
                            if (count > 0)
                            {
                                list.Add(new NotificationData
                                {
                                    Initial = "H",
                                    AvatarColor = Color.FromArgb(150, 100, 200),
                                    Message = $"{count} health record{(count > 1 ? "s" : "")} " +
                                              "submitted this week.",
                                    TimeAgo = "This week",
                                    IsUnread = false
                                });
                            }
                        }
                    }
                    catch { /* weekly health query failed â€” skip */ }
                }
            }
            catch (Exception ex)
            {
                list.Add(new NotificationData
                {
                    Initial = "!",
                    AvatarColor = Color.FromArgb(200, 60, 60),
                    Message = "Could not connect to database: " + ex.Message,
                    TimeAgo = "",
                    IsUnread = false
                });
            }

            return list;
        }

        // =============================================
        // BARANGAY PARAMS HELPER
        // =============================================
        private void AddBarangayParams(MySqlCommand cmd)
        {
            if (!Session.IsMayor)
            {
                if (!cmd.Parameters.Contains("@brgyId"))
                    cmd.Parameters.AddWithValue("@brgyId", Session.BarangayID);
                if (!cmd.Parameters.Contains("@brgyName"))
                    cmd.Parameters.AddWithValue("@brgyName", Session.BarangayName);
            }
        }

        // =============================================
        // NAVIGATION
        // =============================================
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

        private void btnViewApp_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudViewAppointments()); }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult confirm = MessageBox.Show("Logout?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                Session.Clear();
                BsmartFormNavigator.Open(this, new loginLGU());
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        { SettingsNavigationService.OpenSettings(this); }

        private void lblMostCommon_Click(object sender, EventArgs e) { }
        private void lblTotalRecords_Click(object sender, EventArgs e) { }
        private void lblTotalResidents_Click(object sender, EventArgs e) { }
        private void dgvRecentHealth_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void PTotalResidents_Paint(object sender, PaintEventArgs e) { }
        private void PTotalRecords_Paint(object sender, PaintEventArgs e) { }
        private void PMostCommon_Paint(object sender, PaintEventArgs e) { }
    }
}
