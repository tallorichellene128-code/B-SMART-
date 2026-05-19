using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BSMART
{
    public partial class loginRes : Form
    {
        private string connectionString = "Server=localhost;Database=bsmart_db;Uid=root;Pwd=;";

        public loginRes()
        {
            InitializeComponent();
            txtPassword.UseSystemPasswordChar = true;
        }

        private void LoginAS_Load(object sender, EventArgs e) { }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            { MessageBox.Show("Please enter your username.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtUsername.Focus(); return; }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            { MessageBox.Show("Please enter your password.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtPassword.Focus(); return; }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    MySqlCommand cmd = new MySqlCommand(@"
                        SELECT u.id, u.username, u.role,
                               u.barangay_id,
                               COALESCE(b.name, '') AS barangay_name,
                               COALESCE(u.is_frozen, 0) AS is_frozen,
                               u.password
                        FROM   users u
                        LEFT JOIN barangays b ON b.id = u.barangay_id
                        WHERE  u.username = @username
                          AND  u.role     = 'Resident'
                          AND  COALESCE(u.is_archived, 0) = 0", conn);

                    cmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim());

                    MySqlDataReader r = cmd.ExecuteReader();

                    if (r.Read() && BsmartPasswordService.Verify(txtPassword.Text, r["password"].ToString() ?? ""))
                    {
                        int userId = Convert.ToInt32(r["id"]);
                        string username = r["username"].ToString() ?? "";
                        int barangayId = r["barangay_id"] == DBNull.Value
                            ? 0 : Convert.ToInt32(r["barangay_id"]);
                        string barangayName = r["barangay_name"].ToString() ?? "";
                        string storedPassword = r["password"].ToString() ?? "";
                        bool isFrozen = Convert.ToInt32(r["is_frozen"]) == 1;
                        r.Close();

                        bool hasViolation = BsmartResidentViolationService.HasViolation(conn, userId);
                        if (isFrozen && hasViolation)
                        {
                            MessageBox.Show(
                                "Account frozen due to violation.\nPlease go to the barangay hall for assistance.",
                                "Account Frozen", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtPassword.Clear();
                            txtPassword.Focus();
                            return;
                        }

                        if (isFrozen && !hasViolation)
                            UnfreezeAccountWithoutViolation(conn, userId);

                        // ── Set session so all forms know who is logged in ──
                        Session.UserID = userId;
                        Session.Username = username;
                        Session.Role = "Resident";
                        Session.BarangayID = barangayId;
                        Session.BarangayName = barangayName;
                        UpgradePasswordIfNeeded(conn, userId, txtPassword.Text, storedPassword);

                        BsmartFormNavigator.Open(this, new ResidentDash());
                    }
                    else
                    {
                        r.Close();
                        MessageBox.Show(
                            "Invalid username or password.",
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

        private void btnRegister_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new Register()); }

        private void btnBack_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new loginAS()); }

        private static void UpgradePasswordIfNeeded(MySqlConnection conn, int userId, string password, string storedPassword)
        {
            if (!BsmartPasswordService.NeedsUpgrade(storedPassword)) return;
            using MySqlCommand update = new MySqlCommand("UPDATE users SET password = @password WHERE id = @id", conn);
            update.Parameters.AddWithValue("@password", BsmartPasswordService.Hash(password));
            update.Parameters.AddWithValue("@id", userId);
            update.ExecuteNonQuery();
        }

        private static void UnfreezeAccountWithoutViolation(MySqlConnection conn, int userId)
        {
            using MySqlCommand update = new MySqlCommand("UPDATE users SET is_frozen = 0 WHERE id = @id", conn);
            update.Parameters.AddWithValue("@id", userId);
            update.ExecuteNonQuery();
        }
    }
}
