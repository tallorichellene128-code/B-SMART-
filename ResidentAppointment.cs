using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BSMART
{
    public partial class ResidentAppointment : Form
    {
        private string connectionString = "Server=localhost;Database=bsmart_db;Uid=root;Pwd=;";
        private int selectedServiceID = 0;
        private string selectedServiceName = "";
        private string selectedTimeSlot = "";
        private Button activeTimeBtn = null;

        public ResidentAppointment()
        {
            InitializeComponent();
        }

        private void ResidentAppointment_Load(object sender, EventArgs e)
        {
            dtpPreferredDate.MinDate = DateTime.Now.Date.AddDays(1);
            dtpPreferredDate.Value = DateTime.Now.Date.AddDays(1);
            LoadServices();
            WireTimeSlotButtons();
        }

        private void ResidentAppointment_Load_1(object sender, EventArgs e)
        {

        }

        private void cmbFilterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadServices(txtSearch.Text.Trim());
        }
        private void EnsureServiceCatalog(MySqlConnection conn)
        {
            DataTable duplicates = new DataTable();
            using (MySqlCommand findDupes = new MySqlCommand(@"
                SELECT MIN(id) AS keep_id, GROUP_CONCAT(id ORDER BY id) AS ids
                FROM health_services
                GROUP BY LOWER(TRIM(name))
                HAVING COUNT(*) > 1", conn))
            using (MySqlDataAdapter da = new MySqlDataAdapter(findDupes))
            {
                da.Fill(duplicates);
            }

            foreach (DataRow row in duplicates.Rows)
            {
                int keepId = Convert.ToInt32(row["keep_id"]);
                string[] ids = (row["ids"].ToString() ?? "")
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string rawId in ids)
                {
                    if (!int.TryParse(rawId, out int duplicateId) || duplicateId == keepId)
                        continue;

                    using (MySqlCommand moveAppointments = new MySqlCommand(
                        "UPDATE appointments SET service_id = @keepId WHERE service_id = @duplicateId", conn))
                    {
                        moveAppointments.Parameters.AddWithValue("@keepId", keepId);
                        moveAppointments.Parameters.AddWithValue("@duplicateId", duplicateId);
                        moveAppointments.ExecuteNonQuery();
                    }

                    using (MySqlCommand deleteDuplicate = new MySqlCommand(
                        "DELETE FROM health_services WHERE id = @duplicateId", conn))
                    {
                        deleteDuplicate.Parameters.AddWithValue("@duplicateId", duplicateId);
                        deleteDuplicate.ExecuteNonQuery();
                    }
                }
            }

            AddServiceIfMissing(conn, "General Check-up", "Basic consultation for common symptoms and health concerns.", "Consultation", "Available");
            AddServiceIfMissing(conn, "Fever and Cough Consultation", "Assessment for fever, cough, colds, sore throat, and flu-like symptoms.", "Consultation", "Available");
            AddServiceIfMissing(conn, "Rabies Vaccination", "Anti-rabies vaccination or referral after dog, cat, or other animal bites.", "Vaccination", "Available");
            AddServiceIfMissing(conn, "Wound Cleaning and Dressing", "Cleaning and dressing for minor cuts, scratches, and simple wounds.", "Treatment", "Available");
            AddServiceIfMissing(conn, "Blood Pressure Check", "Routine blood pressure screening and monitoring.", "Screening", "Available");
            AddServiceIfMissing(conn, "Blood Sugar Screening", "Simple blood sugar screening for diabetes monitoring.", "Screening", "Available");
            AddServiceIfMissing(conn, "Prenatal Check-up", "Basic prenatal consultation and monitoring for pregnant residents.", "Maternal Care", "Available");
            AddServiceIfMissing(conn, "Child Immunization", "Routine vaccination service for infants and children.", "Vaccination", "Available");
            AddServiceIfMissing(conn, "Family Planning Consultation", "Counseling and basic family planning support.", "Consultation", "Available");
            AddServiceIfMissing(conn, "Medicine Refill Request", "Request maintenance or prescribed medicine availability from the barangay health center.", "Medicine", "Available");
            SetServiceStatus(conn, "Senior Citizen Check-up", "Available");
            SetServiceStatus(conn, "Senior Citizen Check", "Available");
        }

        private void AddServiceIfMissing(MySqlConnection conn, string name, string description, string serviceType, string status)
        {
            using MySqlCommand cmd = new MySqlCommand(@"
                INSERT INTO health_services (name, description, service_type, status)
                SELECT @name, @description, @serviceType, @status
                WHERE NOT EXISTS (
                    SELECT 1 FROM health_services
                    WHERE LOWER(TRIM(name)) = LOWER(TRIM(@name))
                )", conn);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@description", description);
            cmd.Parameters.AddWithValue("@serviceType", serviceType);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.ExecuteNonQuery();
        }
        private void SetServiceStatus(MySqlConnection conn, string name, string status)
        {
            using MySqlCommand cmd = new MySqlCommand(@"
                UPDATE health_services
                SET status = @status
                WHERE LOWER(TRIM(name)) = LOWER(TRIM(@name))", conn);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.ExecuteNonQuery();
        }
        // =============================================
        // LOAD SERVICES INTO flpAvailHealthService
        // =============================================
        private void LoadServices(string keyword = "")
        {
            flpAvailHealthService.Controls.Clear();
            selectedServiceID = 0;
            selectedServiceName = "";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                                        EnsureServiceCatalog(conn);
                    MySqlCommand cmd = new MySqlCommand("", conn);

                    string filter = " WHERE (barangay_id IS NULL OR barangay_id = @brgyId)";
                    cmd.Parameters.AddWithValue("@brgyId", Session.BarangayID);

                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        filter += " AND (name LIKE @kw COLLATE utf8mb4_0900_ai_ci OR description LIKE @kw COLLATE utf8mb4_0900_ai_ci OR service_type LIKE @kw COLLATE utf8mb4_0900_ai_ci)";
                        cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");
                    }

                    cmd.CommandText = $@"SELECT MIN(id) AS id,
                                               name,
                                               MAX(description) AS description,
                                               COALESCE(MAX(status), 'Available') AS status
                                        FROM health_services{filter}
                                        GROUP BY LOWER(TRIM(name)), name
                                        ORDER BY name ASC";

                    MySqlDataReader r = cmd.ExecuteReader();
                    while (r.Read())
                        flpAvailHealthService.Controls.Add(BuildServiceCard(
                            Convert.ToInt32(r["id"]),
                            r["name"].ToString(),
                            r["description"].ToString(),
                            r["status"].ToString()));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading services: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =============================================
        // BUILD EACH SERVICE CARD
        // =============================================
        private Panel BuildServiceCard(int id, string name, string description, string status)
        {
            Color statusColor = status == "Available"
                ? Color.FromArgb(0, 150, 80)
                : status == "Limited Slots"
                    ? Color.FromArgb(200, 100, 0)
                    : Color.Gray;

            Panel card = new Panel
            {
                Width = flpAvailHealthService.Width - 28,
                Height = 80,
                BackColor = Color.White,
                Cursor = Cursors.Hand,
                Margin = new Padding(4, 4, 4, 2)
            };

            card.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.FromArgb(190, 215, 240), 1.5f))
                    e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
            };

            Label lblName = new Label
            {
                Text = name,
                Font = new Font("Arial", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(20, 40, 80),
                Location = new Point(12, 10),
                AutoSize = true
            };
            Label lblDesc = new Label
            {
                Text = description,
                Font = new Font("Arial", 8.5f),
                ForeColor = Color.FromArgb(80, 100, 130),
                Location = new Point(12, 32),
                AutoSize = true
            };
            Label lblStatus = new Label
            {
                Text = status,
                Font = new Font("Arial", 8.5f, FontStyle.Bold),
                ForeColor = statusColor,
                Location = new Point(12, 54),
                AutoSize = true
            };

            card.Controls.Add(lblName);
            card.Controls.Add(lblDesc);
            card.Controls.Add(lblStatus);

            EventHandler select = (s, e) =>
            {
                selectedServiceID = id;
                selectedServiceName = name;
                foreach (Control c in flpAvailHealthService.Controls)
                    c.BackColor = Color.White;
                card.BackColor = Color.FromArgb(210, 235, 255);
            };

            card.Click += select;
            lblName.Click += select;
            lblDesc.Click += select;
            lblStatus.Click += select;

            return card;
        }

        // =============================================
        // WIRE TIME SLOT BUTTONS BY EXACT NAME
        // =============================================
        private void WireTimeSlotButtons()
        {
            var slots = new System.Collections.Generic.Dictionary<string, string>
            {
                { "btn9AM",    "9:00 AM"  },
                { "btn930AM",  "9:30 AM"  },
                { "btn10AM",   "10:00 AM" },
                { "btn1030AM", "10:30 AM" },
                { "btn11AM",   "11:00 AM" },
                { "btn130PM",  "1:30 PM"  },
                { "btn2PM",    "2:00 PM"  },
                { "btn230PM",  "2:30 PM"  },
                { "btn3PM",    "3:00 PM"  },
                { "btn330PM",  "3:30 PM"  },
                { "btn4PM",    "4:00 PM"  },
                { "btn430PM",  "4:30 PM"  }
            };

            foreach (var kvp in slots)
            {
                Control[] found = this.Controls.Find(kvp.Key, true);
                if (found.Length == 0) continue;
                Button btn = found[0] as Button;
                if (btn == null) continue;

                string slot = kvp.Value;
                btn.Click += (s, e) =>
                {
                    if (activeTimeBtn != null)
                    {
                        activeTimeBtn.BackColor = SystemColors.Control;
                        activeTimeBtn.ForeColor = Color.Black;
                    }
                    btn.BackColor = Color.FromArgb(0, 102, 180);
                    btn.ForeColor = Color.White;
                    activeTimeBtn = btn;
                    selectedTimeSlot = slot;
                };
            }
        }

        // =============================================
        // SEARCH
        // =============================================
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadServices(txtSearch.Text.Trim());
        }

        // =============================================
        // BOOK APPOINTMENT
        // =============================================
        private void btnBookApp_Click(object sender, EventArgs e)
        {
            if (selectedServiceID == 0)
            { MessageBox.Show("Please select a health service from the list.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (string.IsNullOrEmpty(selectedTimeSlot))
            { MessageBox.Show("Please select a time slot.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (dtpPreferredDate.Value.Date <= DateTime.Now.Date)
            { MessageBox.Show("Same-day appointment booking is not allowed. Please select tomorrow or another future date.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    // Check if slot already booked
                    MySqlCommand checkCmd = new MySqlCommand(@"
                        SELECT COUNT(*) FROM appointments
                        WHERE  service_id = @svcId
                          AND  appt_date  = @date
                          AND  time_slot  = @slot
                          AND  status    != 'Cancelled'", conn);
                    checkCmd.Parameters.AddWithValue("@svcId", selectedServiceID);
                    checkCmd.Parameters.AddWithValue("@date", dtpPreferredDate.Value.Date);
                    checkCmd.Parameters.AddWithValue("@slot", selectedTimeSlot);

                    if (Convert.ToInt32(checkCmd.ExecuteScalar()) > 0)
                    {
                        MessageBox.Show("This time slot is already taken. Please choose another.",
                            "Slot Unavailable", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    BsmartIdHelper.EnsureCodes(conn);
                    string appointmentCode = BsmartIdHelper.NextAppointmentCode(conn, Session.BarangayID);
                    MySqlCommand cmd = new MySqlCommand(@"
                        INSERT INTO appointments
                            (appointment_code, user_id, service_id, appt_date, time_slot, notes, status, barangay_id)
                        VALUES
                            (@code, @uid, @svcId, @date, @slot, @notes, 'Pending', @brgyId)", conn);

                    cmd.Parameters.AddWithValue("@code", appointmentCode);

                    cmd.Parameters.AddWithValue("@uid", Session.UserID);
                    cmd.Parameters.AddWithValue("@svcId", selectedServiceID);
                    cmd.Parameters.AddWithValue("@date", dtpPreferredDate.Value.Date);
                    cmd.Parameters.AddWithValue("@slot", selectedTimeSlot);
                    cmd.Parameters.AddWithValue("@notes", txtNotesSymptoms.Text.Trim());
                    cmd.Parameters.AddWithValue("@brgyId", Session.BarangayID);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show(
                        $"Appointment booked!\n\nService: {selectedServiceName}\nDate: {dtpPreferredDate.Value:MMMM dd, yyyy}\nTime: {selectedTimeSlot}",
                        "Booking Confirmed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    BsmartNotificationService.RefreshOpenIndicators(true);

                    txtNotesSymptoms.Clear();
                    selectedTimeSlot = "";
                    if (activeTimeBtn != null)
                    {
                        activeTimeBtn.BackColor = SystemColors.Control;
                        activeTimeBtn.ForeColor = Color.Black;
                        activeTimeBtn = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error booking: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =============================================
        // CANCEL APPOINTMENT
        // =============================================
        private void btnCancelApp_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    MySqlCommand getCmd = new MySqlCommand(@"
                        SELECT a.id, s.name, a.appt_date, a.time_slot
                        FROM   appointments a
                        JOIN   health_services s ON s.id = a.service_id
                        WHERE  a.user_id = @uid AND a.status = 'Pending'
                        ORDER  BY a.created_at DESC LIMIT 1", conn);
                    getCmd.Parameters.AddWithValue("@uid", Session.UserID);

                    MySqlDataReader r = getCmd.ExecuteReader();
                    if (!r.Read())
                    {
                        r.Close();
                        MessageBox.Show("You have no pending appointments to cancel.", "Info",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    int apptId = Convert.ToInt32(r["id"]);
                    string svcName = r["name"].ToString();
                    string apptDate = Convert.ToDateTime(r["appt_date"]).ToString("MMMM dd, yyyy");
                    string timeSlot = r["time_slot"].ToString();
                    r.Close();

                    if (MessageBox.Show(
                        $"Cancel this appointment?\n\nService: {svcName}\nDate: {apptDate}\nTime: {timeSlot}",
                        "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

                    MySqlCommand cancelCmd = new MySqlCommand(
                        "UPDATE appointments SET status = 'Cancelled' WHERE id = @id", conn);
                    cancelCmd.Parameters.AddWithValue("@id", apptId);
                    cancelCmd.ExecuteNonQuery();

                    MessageBox.Show("Appointment cancelled successfully.", "Cancelled",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =============================================
        // VIEW ALL MY BOOKINGS — linklblViewBookings
        // Resident can select any Pending appointment and cancel it
        // =============================================
        private void linklblViewBookings_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowBookingsPopup();
        }

        private void ShowBookingsPopup()
        {
            try
            {
                DataTable dt = LoadBookingsData();

                // -- Popup form -------------------------------------------
                Form popup = new Form
                {
                    Text = "My Bookings",
                    Size = new Size(1100, 580),
                    StartPosition = FormStartPosition.CenterScreen,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MaximizeBox = false,
                    MinimizeBox = false,
                    TopMost = true,
                    BackColor = Color.White
                };

                // -- Grid -------------------------------------------------
                DataGridView dgv = new DataGridView
                {
                    Location = new Point(10, 10),
                    Size = new Size(1060, 440),
                    DataSource = dt,
                    ReadOnly = true,
                    AllowUserToAddRows = false,
                    AllowUserToDeleteRows = false,
                    SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                    MultiSelect = false,
                    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                    BackgroundColor = Color.White,
                    BorderStyle = BorderStyle.None,
                    RowHeadersVisible = false
                };

                dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 102, 204);
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 9, FontStyle.Bold);
                dgv.EnableHeadersVisualStyles = false;
                dgv.ColumnHeadersHeight = 35;
                dgv.DefaultCellStyle.Font = new Font("Arial", 9);
                dgv.RowTemplate.Height = 30;
                dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(220, 235, 255);

                // -- Cancel button at bottom ------------------------------
                Button btnCancel = new Button
                {
                    Text = "Cancel Selected Appointment",
                    Location = new Point(10, 460),
                    Size = new Size(1060, 38),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(200, 50, 50),
                    ForeColor = Color.White,
                    Font = new Font("Arial", 10, FontStyle.Bold),
                    Enabled = false,
                    Cursor = Cursors.Hand
                };
                btnCancel.FlatAppearance.BorderSize = 0;

                // -- Color-code status + enable Cancel only for Pending ---
                dgv.DataBindingComplete += (s2, e2) =>
                {
                    if (dgv.Columns.Contains("_DBID"))
                        dgv.Columns["_DBID"].Visible = false;
                    if (!dgv.Columns.Contains("Status")) return;
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        string st = row.Cells["Status"].Value?.ToString() ?? "";
                        row.Cells["Status"].Style.ForeColor =
                            st == "Confirmed" ? Color.FromArgb(0, 150, 80) :
                            st == "Cancelled" ? Color.Red :
                                                Color.FromArgb(200, 100, 0);
                        row.Cells["Status"].Style.Font =
                            new Font("Arial", 9, FontStyle.Bold);
                    }
                };

                // Enable Cancel button only when a Pending row is selected
                dgv.SelectionChanged += (s2, e2) =>
                {
                    if (dgv.SelectedRows.Count == 0) { btnCancel.Enabled = false; return; }
                    string status = dgv.SelectedRows[0].Cells["Status"].Value?.ToString() ?? "";
                    btnCancel.Enabled = status == "Pending";
                    btnCancel.BackColor = btnCancel.Enabled
                        ? Color.FromArgb(200, 50, 50)
                        : Color.FromArgb(160, 160, 160);
                };

                // Cancel the selected appointment
                btnCancel.Click += (s2, e2) =>
                {
                    if (dgv.SelectedRows.Count == 0) return;

                    DataGridViewRow row = dgv.SelectedRows[0];
                    int apptId = Convert.ToInt32(row.Cells["_DBID"].Value);
                    string svcName = row.Cells["Service"].Value?.ToString() ?? "";
                    string apptDate = Convert.ToDateTime(row.Cells["Date"].Value).ToString("MMMM dd, yyyy");
                    string timeSlot = row.Cells["Time"].Value?.ToString() ?? "";

                    if (MessageBox.Show(
                        $"Cancel this appointment?\n\nService: {svcName}\nDate: {apptDate}\nTime: {timeSlot}",
                        "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2) == DialogResult.No) return;

                    try
                    {
                        using (MySqlConnection conn2 = new MySqlConnection(connectionString))
                        {
                            conn2.Open();
                            MySqlCommand cancelCmd = new MySqlCommand(
                                "UPDATE appointments SET status = 'Cancelled' WHERE id = @id AND user_id = @uid", conn2);
                            cancelCmd.Parameters.AddWithValue("@id", apptId);
                            cancelCmd.Parameters.AddWithValue("@uid", Session.UserID);
                            cancelCmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Appointment cancelled successfully.", "Cancelled",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Refresh the grid in place
                        dgv.DataSource = LoadBookingsData();
                        btnCancel.Enabled = false;
                        btnCancel.BackColor = Color.FromArgb(160, 160, 160);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error cancelling: " + ex.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                popup.Controls.Add(dgv);
                popup.Controls.Add(btnCancel);
                popup.Load += (s3, e3) => { popup.BringToFront(); popup.Activate(); };
                popup.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading bookings: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Reusable data loader for bookings grid
        private DataTable LoadBookingsData()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                BsmartIdHelper.EnsureCodes(conn);
                MySqlCommand cmd = new MySqlCommand(@"
                    SELECT
                        a.id                                            AS '_DBID',
                        COALESCE(NULLIF(a.appointment_code, ''), CAST(a.id AS CHAR)) AS 'ID',
                        s.name                                          AS 'Service',
                        a.appt_date                                     AS 'Date',
                        a.time_slot                                     AS 'Time',
                        COALESCE(a.notes, '')                           AS 'Notes/Symptoms',
                        COALESCE(a.assigned_doc_name, 'Pending Assign') AS 'Assigned Doctor/Nurse',
                        a.status                                        AS 'Status'
                    FROM appointments a
                    JOIN health_services s ON s.id = a.service_id
                    WHERE a.user_id = @uid
                    ORDER BY a.appt_date DESC, a.time_slot ASC", conn);
                cmd.Parameters.AddWithValue("@uid", Session.UserID);

                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        // =============================================
        // NAVIGATION
        // =============================================
        private void btnResidentDash_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new ResidentDash()); }

        private void btnResViewHealth_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new ResViewHealth()); }

        private void btnServicesAppointment_Click(object sender, EventArgs e)
        { txtSearch.Clear(); LoadServices(); }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Logout?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            { Session.Clear(); BsmartFormNavigator.Open(this, new loginRes()); }
        }

        // Stub handlers required by designer
        private void flpAvailHealthService_Paint(object sender, PaintEventArgs e) { }
        private void PBooking_Paint(object sender, PaintEventArgs e) { }
        private void dtpPreferredDate_ValueChanged(object sender, EventArgs e) { }
        private void flpTimeSlots_Paint(object sender, PaintEventArgs e) { }
        private void txtNotesSymptoms_TextChanged(object sender, EventArgs e) { }
        private void btn9AM_Click(object sender, EventArgs e) { }
        private void btn930AM_Click(object sender, EventArgs e) { }
        private void btn10AM_Click(object sender, EventArgs e) { }
        private void btn1030AM_Click(object sender, EventArgs e) { }
        private void btn11AM_Click(object sender, EventArgs e) { }
        private void btn130PM_Click(object sender, EventArgs e) { }
        private void btn2PM_Click(object sender, EventArgs e) { }
        private void btn230PM_Click(object sender, EventArgs e) { }
        private void btn3PM_Click(object sender, EventArgs e) { }
        private void btn330PM_Click(object sender, EventArgs e) { }
        private void btn4PM_Click(object sender, EventArgs e) { }
        private void btn430PM_Click(object sender, EventArgs e) { }
    }
}

