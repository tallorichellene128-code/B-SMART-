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
    public partial class Settings : Form
    {
        private const string ConnectionString = "Server=localhost;Database=bsmart_db;Uid=root;Pwd=;";

        public Settings()
        {
            InitializeComponent();
            btnLogout.Click += (s, e) => SettingsNavigationService.OpenLogin(this);
            LockResidentIdentityFields();
        }

        private void btnPassword_Click(object sender, EventArgs e)
        {
            BsmartFormNavigator.Open(this, new Settings2());
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            if (!SettingsNavigationService.IsResident())
            {
                MessageBox.Show("Only residents can access profile and account details.",
                    "Access Restricted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                BsmartFormNavigator.Open(this, new Settings2());
            }
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            if (!SettingsNavigationService.IsResident())
            {
                BsmartFormNavigator.Open(this, new Settings2());
                return;
            }

            LoadResidentProfile();
        }

        private void txtFirstName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtMiddleName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtEmailAdd_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtMobileNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtLastName_TextChanged(object sender, EventArgs e)
        {

        }

        private void dtpBirthdate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            try
            {
                using MySqlConnection conn = new MySqlConnection(ConnectionString);
                conn.Open();

                using MySqlCommand cmd = new MySqlCommand(@"
                    UPDATE users SET
                        username = @username,
                        email = @email,
                        mobile_number = @mobile,
                        address = @address
                    WHERE id = @userId
                      AND role = 'Resident'", conn);

                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    MessageBox.Show("Username cannot be blank.", "Settings",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUsername.Focus();
                    return;
                }

                using MySqlCommand duplicate = new MySqlCommand(@"
                    SELECT COUNT(*)
                    FROM users
                    WHERE username = @username
                      AND id <> @userId", conn);
                duplicate.Parameters.AddWithValue("@username", txtUsername.Text.Trim());
                duplicate.Parameters.AddWithValue("@userId", Session.UserID);
                if (Convert.ToInt32(duplicate.ExecuteScalar()) > 0)
                {
                    MessageBox.Show("Username is already taken. Please choose another one.",
                        "Settings", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUsername.Focus();
                    return;
                }

                cmd.Parameters.AddWithValue("@username", txtUsername.Text.Trim());
                cmd.Parameters.AddWithValue("@email", txtEmailAdd.Text.Trim());
                cmd.Parameters.AddWithValue("@mobile", txtMobileNo.Text.Trim());
                cmd.Parameters.AddWithValue("@address", txtAddress.Text.Trim());
                cmd.Parameters.AddWithValue("@userId", Session.UserID);

                if (cmd.ExecuteNonQuery() == 0)
                {
                    MessageBox.Show("Profile was not updated. Please login again.",
                        "Settings", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                MessageBox.Show("Profile updated successfully.", "Settings",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                Session.Username = txtUsername.Text.Trim();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving profile: " + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            LoadResidentProfile();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            SettingsNavigationService.OpenDashboard(this);
        }

        private void LoadResidentProfile()
        {
            try
            {
                using MySqlConnection conn = new MySqlConnection(ConnectionString);
                conn.Open();

                using MySqlCommand cmd = new MySqlCommand(@"
                    SELECT username, first_name, middle_name, last_name, birthday,
                           citizenship, religion, civil_status,
                           email, mobile_number, address
                    FROM users
                    WHERE id = @userId
                      AND role = 'Resident'
                    LIMIT 1", conn);
                cmd.Parameters.AddWithValue("@userId", Session.UserID);

                using MySqlDataReader reader = cmd.ExecuteReader();
                if (!reader.Read())
                    return;

                txtUsername.Text = ValueText(reader["username"]);
                txtFirstName.Text = ValueText(reader["first_name"]);
                txtMiddleName.Text = ValueText(reader["middle_name"]);
                txtLastName.Text = ValueText(reader["last_name"]);
                txtCitizenship.Text = ValueText(reader["citizenship"]);
                txtReligion.Text = ValueText(reader["religion"]);
                SetCivilStatus(ValueText(reader["civil_status"]));
                txtEmailAdd.Text = ValueText(reader["email"]);
                txtMobileNo.Text = ValueText(reader["mobile_number"]);
                txtAddress.Text = ValueText(reader["address"]);
                if (reader["birthday"] != DBNull.Value)
                    dtpBirthdate.Value = Convert.ToDateTime(reader["birthday"]);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading profile: " + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string ValueText(object value)
        {
            return value == null || value == DBNull.Value ? "" : Convert.ToString(value) ?? "";
        }

        private void LockResidentIdentityFields()
        {
            txtFirstName.ReadOnly = true;
            txtMiddleName.ReadOnly = true;
            txtLastName.ReadOnly = true;
            txtCitizenship.ReadOnly = true;
            txtReligion.ReadOnly = true;
            comboBox1.Enabled = false;
            dtpBirthdate.Enabled = false;

            Color readOnlyBackColor = Color.FromArgb(235, 242, 247);
            txtFirstName.BackColor = readOnlyBackColor;
            txtMiddleName.BackColor = readOnlyBackColor;
            txtLastName.BackColor = readOnlyBackColor;
            txtCitizenship.BackColor = readOnlyBackColor;
            txtReligion.BackColor = readOnlyBackColor;
            comboBox1.BackColor = readOnlyBackColor;
            dtpBirthdate.CalendarMonthBackground = readOnlyBackColor;
        }

        private void SetCivilStatus(string civilStatus)
        {
            if (string.IsNullOrWhiteSpace(civilStatus))
            {
                comboBox1.SelectedIndex = -1;
                return;
            }

            foreach (object item in comboBox1.Items)
            {
                if (string.Equals(Convert.ToString(item), civilStatus, StringComparison.OrdinalIgnoreCase))
                {
                    comboBox1.SelectedItem = item;
                    return;
                }
            }

            comboBox1.Items.Add(civilStatus);
            comboBox1.SelectedItem = civilStatus;
        }
    }
}
