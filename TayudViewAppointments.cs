using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BSMART
{
    public partial class TayudViewAppointments : Form
    {
        private string connectionString = "Server=localhost;Database=bsmart_db;Uid=root;Pwd=;";
        private int selectedAppointmentID = 0;
        private string selectedStatus = "";

        public TayudViewAppointments()
        {
            InitializeComponent();
        }

        private void TayudViewAppointments_Load(object sender, EventArgs e)
        {
            LoadAppointments();
            AddCalendarButton();
        }

        private void AddCalendarButton()
        {
            if (Controls.Find("btnCalendarView", true).Length > 0) return;

            Button calendar = new Button
            {
                Name = "btnCalendarView",
                Text = "Calendar View",
                Size = btnApprove.Size,
                Location = new Point(btnApprove.Right + ((btnReject.Left - btnApprove.Right - btnApprove.Width) / 2), btnApprove.Top),
                Font = btnApprove.Font,
                UseVisualStyleBackColor = true
            };
            calendar.Click += (s, e) => new BsmartAppointmentCalendarForm().Show();
            Controls.Add(calendar);
            calendar.BringToFront();
            BsmartUiService.PrepareForm(this);
        }

        // =============================================
        // BARANGAY FILTER
        // =============================================
        private string BarangayFilter(MySqlCommand cmd)
        {
            if (Session.IsMayor) return "";
            cmd.Parameters.AddWithValue("@brgyId", Session.BarangayID);
            cmd.Parameters.AddWithValue("@brgyName", Session.BarangayName);
            return " AND (a.barangay_id = @brgyId OR a.barangay_id IN (SELECT id FROM barangays WHERE name = @brgyName))";
        }

        // =============================================
        // LOAD APPOINTMENTS INTO GRID
        // Shows all appointments for this barangay
        // =============================================
        private void LoadAppointments(string keyword = "")
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    BsmartIdHelper.EnsureCodes(conn);
                    MySqlCommand cmd = new MySqlCommand("", conn);
                    string brgyFilter = BarangayFilter(cmd);

                    string keyFilter = "";
                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        keyFilter = @" AND (u.first_name LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR  COALESCE(a.appointment_code, CAST(a.id AS CHAR)) LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR  u.last_name  LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR  CONCAT(u.first_name,' ',u.last_name) LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR  s.name       LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                       OR  a.status     LIKE @kw COLLATE utf8mb4_0900_ai_ci)";
                        cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");
                    }

                    cmd.CommandText = $@"
                        SELECT
                            a.id                                    AS '_DBID',
                            COALESCE(NULLIF(a.appointment_code, ''), CAST(a.id AS CHAR)) AS 'ID',
                            CONCAT(u.first_name,' ',u.last_name)    AS 'Resident',
                            s.name                                  AS 'Service',
                            a.appt_date                             AS 'Date',
                            a.time_slot                             AS 'Time',
                            COALESCE(a.notes, '')                   AS 'Notes/Symptoms',
                            COALESCE(a.assigned_doc_name, 'Not Assigned') AS 'Assigned Doctor/Nurse',
                            a.status                                AS 'Status'
                        FROM appointments a
                        JOIN users           u ON u.id  = a.user_id
                        JOIN health_services s ON s.id  = a.service_id
                        WHERE 1=1{brgyFilter}{keyFilter}
                        ORDER BY
                            FIELD(a.status,'Pending','Confirmed','Cancelled'),
                            a.appt_date ASC,
                            a.time_slot ASC";

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvViewResApp.DataSource = dt;
                    StyleGrid();
                    ClearSelection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading appointments: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =============================================
        // STYLE GRID
        // =============================================
        private void StyleGrid()
        {
            dgvViewResApp.ReadOnly = true;
            dgvViewResApp.AllowUserToAddRows = false;
            dgvViewResApp.AllowUserToDeleteRows = false;
            dgvViewResApp.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvViewResApp.MultiSelect = false;
            dgvViewResApp.BackgroundColor = Color.White;
            dgvViewResApp.BorderStyle = BorderStyle.None;
            dgvViewResApp.RowHeadersVisible = false;
            dgvViewResApp.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvViewResApp.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 102, 204);
            dgvViewResApp.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvViewResApp.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 9, FontStyle.Bold);
            dgvViewResApp.ColumnHeadersHeight = 35;
            dgvViewResApp.EnableHeadersVisualStyles = false;
            dgvViewResApp.DefaultCellStyle.Font = new Font("Arial", 9);
            dgvViewResApp.RowTemplate.Height = 30;
            dgvViewResApp.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(220, 235, 255);

            if (dgvViewResApp.Columns.Contains("_DBID"))
                dgvViewResApp.Columns["_DBID"].Visible = false;

            // Color-code status column
            foreach (DataGridViewRow row in dgvViewResApp.Rows)
            {
                if (row.IsNewRow) continue;
                string status = row.Cells["Status"].Value?.ToString() ?? "";
                row.Cells["Status"].Style.ForeColor =
                    status == "Confirmed" ? Color.FromArgb(0, 150, 80) :
                    status == "Cancelled" ? Color.Red :
                                            Color.FromArgb(200, 100, 0);
                row.Cells["Status"].Style.Font = new Font("Arial", 9, FontStyle.Bold);
            }
        }

        // =============================================
        // ROW CLICK — load selected appointment
        // =============================================
        private void dgvViewResApp_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            SelectRow(dgvViewResApp.Rows[e.RowIndex]);
        }

        private void dgvViewResApp_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            SelectRow(dgvViewResApp.Rows[e.RowIndex]);
        }

        private void SelectRow(DataGridViewRow row)
        {
            selectedAppointmentID = Convert.ToInt32(row.Cells["_DBID"].Value);
            selectedStatus = row.Cells["Status"].Value?.ToString() ?? "";

            // Only allow Approve/Reject on Pending appointments
            btnApprove.Enabled = selectedStatus == "Pending";
            btnReject.Enabled = selectedStatus == "Pending";
        }

        private void ClearSelection()
        {
            selectedAppointmentID = 0;
            selectedStatus = "";
            btnApprove.Enabled = false;
            btnReject.Enabled = false;
        }

        private string SelectedAppointmentSummary()
        {
            if (dgvViewResApp.CurrentRow == null) return "";

            string id = dgvViewResApp.CurrentRow.Cells["ID"]?.Value?.ToString() ?? "";
            string resident = dgvViewResApp.CurrentRow.Cells["Resident"]?.Value?.ToString() ?? "";
            string service = dgvViewResApp.CurrentRow.Cells["Service"]?.Value?.ToString() ?? "";
            string summary = "";
            if (!string.IsNullOrWhiteSpace(id)) summary = id;
            if (!string.IsNullOrWhiteSpace(resident)) summary += (summary.Length == 0 ? "" : " - ") + resident;
            if (!string.IsNullOrWhiteSpace(service)) summary += (summary.Length == 0 ? "" : " - ") + service;
            return summary;
        }

        // =============================================
        // APPROVE — prompts for doctor/nurse name,
        // sets status = 'Confirmed', saves assigned_doc_name
        // Reflects immediately on resident's booking view
        // =============================================
        private void btnApprove_Click(object sender, EventArgs e)
        {
            if (selectedAppointmentID == 0)
            { MessageBox.Show("Please select a Pending appointment first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            // Prompt for doctor/nurse to assign
            string docName = PromptForDoctor();
            if (docName == null) return; // user cancelled the prompt

            if (MessageBox.Show(
                $"Approve this appointment?\nAssigned to: {docName}",
                "Confirm Approve", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(@"
                        UPDATE appointments
                        SET    status            = 'Confirmed',
                               assigned_doc_name = @docName
                        WHERE  id = @id", conn);
                    cmd.Parameters.AddWithValue("@docName", docName);
                    cmd.Parameters.AddWithValue("@id", selectedAppointmentID);
                    cmd.ExecuteNonQuery();
                    BsmartAuditService.Log("Approve Appointment", "appointments", selectedAppointmentID, SelectedAppointmentSummary());

                    MessageBox.Show(
                        $"Appointment confirmed!\nAssigned to: {docName}",
                        "Approved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadAppointments();
                    BsmartNotificationService.RefreshOpenIndicators(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error approving: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =============================================
        // REJECT — sets status = 'Cancelled'
        // Reflects immediately on resident's booking view
        // =============================================
        private void btnReject_Click(object sender, EventArgs e)
        {
            if (selectedAppointmentID == 0)
            { MessageBox.Show("Please select a Pending appointment first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            if (MessageBox.Show(
                "Reject and cancel this appointment?\nThe resident will see it as Cancelled.",
                "Confirm Reject", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "UPDATE appointments SET status = 'Cancelled' WHERE id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", selectedAppointmentID);
                    cmd.ExecuteNonQuery();
                    BsmartAuditService.Log("Reject Appointment", "appointments", selectedAppointmentID, SelectedAppointmentSummary());

                    MessageBox.Show("Appointment rejected.", "Rejected",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadAppointments();
                    BsmartNotificationService.RefreshOpenIndicators(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error rejecting: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =============================================
        // PROMPT FOR DOCTOR/NURSE NAME
        // Small input dialog — returns null if cancelled
        // =============================================
        private string PromptForDoctor()
        {
            Form prompt = new Form
            {
                Text = "Assign Doctor / Nurse",
                Size = new Size(520, 210),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                TopMost = true
            };

            Label lbl = new Label
            {
                Text = "Enter Doctor / Nurse name to assign:",
                Location = new Point(12, 15),
                AutoSize = true,
                Font = new Font("Arial", 9)
            };

            TextBox txt = new TextBox
            {
                Location = new Point(12, 38),
                Size = new Size(480, 30),
                Font = new Font("Arial", 9)
            };

            Button btnOk = new Button
            {
                Text = "Assign",
                DialogResult = DialogResult.OK,
                Location = new Point(12, 108),
                Size = new Size(230, 36),
                BackColor = Color.FromArgb(0, 102, 180),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 9, FontStyle.Bold)
            };
            btnOk.FlatAppearance.BorderSize = 0;

            Button btnCancel = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Location = new Point(252, 108),
                Size = new Size(230, 36),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Arial", 9)
            };
            btnCancel.FlatAppearance.BorderSize = 0;

            prompt.Controls.AddRange(new Control[] { lbl, txt, btnOk, btnCancel });
            prompt.AcceptButton = btnOk;
            prompt.CancelButton = btnCancel;

            if (prompt.ShowDialog() == DialogResult.OK)
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    MessageBox.Show("Please enter a doctor or nurse name.", "Required",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }
                return txt.Text.Trim();
            }
            return null;
        }

        // =============================================
        // NAVIGATION
        // =============================================
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

        private void btnViewApp_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudViewAppointments()); }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Logout?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            { Session.Clear(); BsmartFormNavigator.Open(this, new loginLGU()); }
        }
    }
}

