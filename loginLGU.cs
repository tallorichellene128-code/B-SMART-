using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BSMART
{
    public partial class loginLGU : Form
    {
        private string connectionString = "Server=localhost;Database=bsmart_db;Uid=root;Pwd=;";

        public loginLGU()
        {
            InitializeComponent();
            txtPassword.UseSystemPasswordChar = true;
        }

        private void loginLGU_Load(object sender, EventArgs e)
        {
            cmbBarangay.SelectedIndex = -1;
        }

        private int ResolveBarangayId(MySqlConnection conn, string barangayName)
        {
            using (MySqlCommand ensure = new MySqlCommand(@"
                INSERT INTO barangays (name)
                SELECT @barangay
                WHERE NOT EXISTS (SELECT 1 FROM barangays WHERE name = @barangay)", conn))
            {
                ensure.Parameters.AddWithValue("@barangay", barangayName);
                ensure.ExecuteNonQuery();
            }

            using (MySqlCommand cmd = new MySqlCommand(
                "SELECT MIN(id) FROM barangays WHERE name = @barangay", conn))
            {
                cmd.Parameters.AddWithValue("@barangay", barangayName);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private void UpdateUserBarangay(MySqlConnection conn, int userId, int barangayId)
        {
            using (MySqlCommand cmd = new MySqlCommand(
                "UPDATE users SET barangay_id = @barangayId WHERE id = @userId", conn))
            {
                cmd.Parameters.AddWithValue("@barangayId", barangayId);
                cmd.Parameters.AddWithValue("@userId", userId);
                cmd.ExecuteNonQuery();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // -- Validate inputs ------------------------------------------
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Please enter your username.", "Login",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter your password.", "Login",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            // Mayor does not need barangay selection
            bool isMayorAttempt = txtUsername.Text.Trim().ToLower().Contains("mayor");

            if (!isMayorAttempt && cmbBarangay.SelectedIndex == -1)
            {
                MessageBox.Show("Please select your Barangay.", "Login",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbBarangay.Focus();
                return;
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query;
                    string selectedBarangay = "";
                    int selectedBarangayId = 0;

                    if (isMayorAttempt)
                    {
                        // Mayor: no barangay filter
                        query = @"SELECT u.id, u.username, u.role, u.full_name, u.password,
                                         COALESCE(u.barangay_id, 0) AS barangay_id,
                                         COALESCE(b.name, 'All Barangays') AS barangay_name
                                  FROM   users u
                                  LEFT JOIN barangays b ON b.id = u.barangay_id
                                  WHERE  u.username = @username
                                  AND    u.role     = 'Mayor'";
                    }
                    else
                    {
                        selectedBarangay = cmbBarangay.SelectedItem.ToString();
                        selectedBarangayId = ResolveBarangayId(conn, selectedBarangay);

                        // LGU Staff: accept the selected barangay by name too, so older duplicate
                        // barangay IDs in MySQL do not block valid logins.
                        query = @"SELECT u.id, u.username, u.role, u.full_name, u.password,
                                         COALESCE(u.barangay_id, @selectedBarangayId) AS barangay_id,
                                         COALESCE(b.name, @barangay) AS barangay_name
                                  FROM   users u
                                  LEFT JOIN barangays b ON b.id = u.barangay_id
                                  WHERE  u.username    = @username
                                  AND    u.role        = 'LGUStaff'
                                  AND   (u.barangay_id = @selectedBarangayId
                                      OR b.name = @barangay
                                      OR LOWER(u.username) LIKE @barangayKey COLLATE utf8mb4_0900_ai_ci
                                      OR LOWER(COALESCE(u.full_name, '')) LIKE @barangayNameKey COLLATE utf8mb4_0900_ai_ci)";
                    }

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim());

                    if (!isMayorAttempt)
                    {
                        cmd.Parameters.AddWithValue("@barangay", selectedBarangay);
                        cmd.Parameters.AddWithValue("@selectedBarangayId", selectedBarangayId);
                        cmd.Parameters.AddWithValue("@barangayKey", "%" + selectedBarangay.ToLower().Replace(" ", "_") + "%");
                        cmd.Parameters.AddWithValue("@barangayNameKey", "%" + selectedBarangay.ToLower() + "%");
                    }

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read() && BsmartPasswordService.Verify(txtPassword.Text, reader["password"].ToString() ?? ""))
                    {
                        // -- Set session ----------------------------------
                        int userId = Convert.ToInt32(reader["id"]);
                        Session.UserID = userId;
                        Session.Username = reader["username"].ToString();
                        Session.Role = reader["role"].ToString();
                        Session.BarangayID = isMayorAttempt
                            ? Convert.ToInt32(reader["barangay_id"])
                            : selectedBarangayId;
                        Session.BarangayName = isMayorAttempt
                            ? reader["barangay_name"].ToString()
                            : selectedBarangay;
                        string storedPassword = reader["password"].ToString() ?? "";

                        reader.Close();
                        UpgradePasswordIfNeeded(conn, userId, txtPassword.Text, storedPassword);

                        if (!isMayorAttempt)
                            UpdateUserBarangay(conn, userId, selectedBarangayId);
                        BsmartAuditService.Log("Login", "user", userId);

                        // -- Route to correct dashboard -------------------
                        // ALL roles open the same TayudLGU dashboard.
                        // Session.BarangayID controls which data they see.
                        var dashboard = new TayudLGU();
                        BsmartFormNavigator.Open(this, dashboard);
                    }
                    else
                    {
                        reader.Close();
                        MessageBox.Show(
                            "Invalid username, password, or barangay.\nPlease try again.",
                            "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtPassword.Clear();
                        txtPassword.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Login error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            BsmartFormNavigator.Open(this, new loginAS());
        }

        private void cmbBarangay_SelectedIndexChanged(object sender, EventArgs e) { }

        private static void UpgradePasswordIfNeeded(MySqlConnection conn, int userId, string password, string storedPassword)
        {
            if (!BsmartPasswordService.NeedsUpgrade(storedPassword)) return;
            using MySqlCommand update = new MySqlCommand("UPDATE users SET password = @password WHERE id = @id", conn);
            update.Parameters.AddWithValue("@password", BsmartPasswordService.Hash(password));
            update.Parameters.AddWithValue("@id", userId);
            update.ExecuteNonQuery();
        }
    }
}
