using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BSMART
{
    public partial class loginMayor : Form
    {
        private string connectionString = "Server=localhost;Database=bsmart_db;Uid=root;Pwd=;";

        public loginMayor()
        {
            InitializeComponent();
            txtPassword.UseSystemPasswordChar = true;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            BsmartFormNavigator.Open(this, new loginAS());
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
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

            try
            {
                using MySqlConnection conn = new MySqlConnection(connectionString);
                conn.Open();

                using MySqlCommand cmd = new MySqlCommand(@"
                    SELECT id, username, role, full_name, password
                    FROM users
                    WHERE username = @username
                      AND role = 'Mayor'
                    LIMIT 1", conn);
                cmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim());

                using MySqlDataReader reader = cmd.ExecuteReader();
                if (!reader.Read() || !BsmartPasswordService.Verify(txtPassword.Text, reader["password"].ToString() ?? ""))
                {
                    MessageBox.Show("Invalid mayor username or password.",
                        "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassword.Clear();
                    txtPassword.Focus();
                    return;
                }

                Session.UserID = Convert.ToInt32(reader["id"]);
                Session.Username = reader["username"].ToString() ?? "";
                Session.Role = "Mayor";
                Session.BarangayID = 0;
                Session.BarangayName = "All Barangays";
                int userId = Session.UserID;
                string storedPassword = reader["password"].ToString() ?? "";
                reader.Close();
                UpgradePasswordIfNeeded(conn, userId, txtPassword.Text, storedPassword);
                BsmartAuditService.Log("Login", "user", userId);

                BsmartFormNavigator.Open(this, new MayorDash());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Login error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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
