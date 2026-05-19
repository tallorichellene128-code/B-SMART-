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
    public partial class Settings2 : Form
    {
        private const string ConnectionString = "Server=localhost;Database=bsmart_db;Uid=root;Pwd=;";

        public Settings2()
        {
            InitializeComponent();
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            if (!SettingsNavigationService.IsResident())
            {
                MessageBox.Show("Only residents can access profile and account details.",
                    "Access Restricted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            BsmartFormNavigator.Open(this, new Settings());
        }

        private void Settings2_Load(object sender, EventArgs e)
        {
            bool resident = SettingsNavigationService.IsResident();
            btnProfile.Enabled = resident;
            btnProfile.Visible = resident;
            txtCurrentPass.UseSystemPasswordChar = true;
            txtNewPass.UseSystemPasswordChar = true;
            txtConfirmNewPass.UseSystemPasswordChar = true;
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            SettingsNavigationService.OpenLogin(this);
        }

        private void btnPassword_Click(object sender, EventArgs e)
        {
            txtCurrentPass.Focus();
        }

        private void txtCurrentPass_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtNewPass_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtConfirmNewPass_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            string current = txtCurrentPass.Text;
            string next = txtNewPass.Text;
            string confirm = txtConfirmNewPass.Text;

            if (string.IsNullOrWhiteSpace(current) ||
                string.IsNullOrWhiteSpace(next) ||
                string.IsNullOrWhiteSpace(confirm))
            {
                MessageBox.Show("Please complete all password fields.", "Settings",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (next.Length < 6)
            {
                MessageBox.Show("New password must be at least 6 characters.", "Settings",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (next != confirm)
            {
                MessageBox.Show("New password and confirmation do not match.", "Settings",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using MySqlConnection conn = new MySqlConnection(ConnectionString);
                conn.Open();

                using MySqlCommand check = new MySqlCommand(
                    "SELECT password FROM users WHERE id = @userId LIMIT 1", conn);
                check.Parameters.AddWithValue("@userId", Session.UserID);
                string storedPassword = Convert.ToString(check.ExecuteScalar()) ?? "";

                if (!BsmartPasswordService.Verify(current, storedPassword))
                {
                    MessageBox.Show("Current password is incorrect.", "Settings",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using MySqlCommand update = new MySqlCommand(
                    "UPDATE users SET password = @newPassword WHERE id = @userId", conn);
                update.Parameters.AddWithValue("@newPassword", BsmartPasswordService.Hash(next));
                update.Parameters.AddWithValue("@userId", Session.UserID);
                update.ExecuteNonQuery();

                txtCurrentPass.Clear();
                txtNewPass.Clear();
                txtConfirmNewPass.Clear();

                MessageBox.Show("Password updated successfully.", "Settings",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating password: " + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtCurrentPass.Clear();
            txtNewPass.Clear();
            txtConfirmNewPass.Clear();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            SettingsNavigationService.OpenDashboard(this);
        }
    }
}
