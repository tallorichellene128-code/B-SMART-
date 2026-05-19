using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BSMART
{
    public partial class ResidentDash : Form
    {
        private string connectionString = "Server=localhost;Database=bsmart_db;Uid=root;Pwd=;";

        public ResidentDash()
        {
            InitializeComponent();
            ResidentProfileUiService.Apply(PResProfile, lblResidentName, lblAge, lblAddress, lblBarangay, lblEmail);
        }

        private void ResidentDash_Load(object sender, EventArgs e)
        {
            LoadResidentProfile();
            AddTimelineButton();
        }

        private void AddTimelineButton()
        {
            if (Controls.Find("btnResidentTimeline", true).Length > 0) return;

            Button timeline = new Button
            {
                Name = "btnResidentTimeline",
                Text = "History Timeline",
                Location = new Point(PResProfile.Left, PResProfile.Bottom + 16),
                Size = new Size(220, 38),
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                UseVisualStyleBackColor = true
            };
            timeline.Click += (s, e) => new BsmartResidentTimelineForm().Show();
            Controls.Add(timeline);
            timeline.BringToFront();
            BsmartUiService.PrepareForm(this);
        }

        private void LoadResidentProfile()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(@"
                        SELECT u.first_name, u.last_name, u.age, u.gender, u.birthday,
                               u.address, u.email, u.mobile_number,
                               COALESCE(b.name, 'N/A') AS barangay_name
                        FROM   users u
                        LEFT JOIN barangays b ON b.id = u.barangay_id
                        WHERE  u.id = @uid", conn);
                    cmd.Parameters.AddWithValue("@uid", Session.UserID);

                    MySqlDataReader r = cmd.ExecuteReader();
                    if (r.Read())
                    {
                        ResidentProfileUiService.SetValues(
                            lblResidentName,
                            lblAge,
                            lblAddress,
                            lblBarangay,
                            lblEmail,
                            $"{ResidentProfileUiService.Value(r["first_name"], "")} {ResidentProfileUiService.Value(r["last_name"], "")}".Trim(),
                            ResidentProfileUiService.Value(r["age"]),
                            ResidentProfileUiService.Value(r["gender"]),
                            ResidentProfileUiService.DateValue(r["birthday"]),
                            ResidentProfileUiService.Value(r["address"]),
                            ResidentProfileUiService.Value(r["barangay_name"]),
                            ResidentProfileUiService.Value(r["email"]),
                            ResidentProfileUiService.Value(r["mobile_number"]));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading profile: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnResidentDash_Click(object sender, EventArgs e)
        { LoadResidentProfile(); }

        private void btnResViewHealth_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new ResViewHealth()); }

        private void btnServicesAppointment_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new ResidentAppointment()); }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Logout?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            { Session.Clear(); BsmartFormNavigator.Open(this, new loginRes()); }
        }

        private void PResProfile_Paint(object sender, PaintEventArgs e) { }
        private void lblResidentName_Click(object sender, EventArgs e) { }
        private void lblAge_Click(object sender, EventArgs e) { }
        private void lblAddress_Click(object sender, EventArgs e) { }
        private void lblBarangay_Click(object sender, EventArgs e) { }
        private void lblEmail_Click(object sender, EventArgs e) { }
    }
}
