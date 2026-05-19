using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BSMART
{
    public partial class Register3 : Form
    {
        private string connectionString = "Server=localhost;Database=bsmart_db;Uid=root;Pwd=;";

        public Register3()
        {
            InitializeComponent();
            txtPassword.UseSystemPasswordChar = true;
            txtPassConfirm.UseSystemPasswordChar = true;
        }

        private void Register3_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(RegisterData.Username))
                txtUsername.Text = RegisterData.Username;
        }

        private bool ValidatePage3()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            { MessageBox.Show("Please enter a Username.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtUsername.Focus(); return false; }
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            { MessageBox.Show("Please enter a Password.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtPassword.Focus(); return false; }
            if (txtPassword.Text.Length < 6)
            { MessageBox.Show("Password must be at least 6 characters.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtPassword.Focus(); return false; }
            if (txtPassword.Text != txtPassConfirm.Text)
            { MessageBox.Show("Passwords do not match.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtPassConfirm.Focus(); return false; }
            return true;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!ValidatePage3()) return;

            RegisterData.Username = txtUsername.Text.Trim();
            RegisterData.Password = BsmartPasswordService.Hash(txtPassword.Text);

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    BsmartIdHelper.EnsureCodes(conn);

                    if (ResidentAccountAlreadyExists(conn))
                    {
                        MessageBox.Show("Resident already has an existing account.",
                            "Existing Account", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Check if username already exists
                    MySqlCommand checkCmd = new MySqlCommand(
                        "SELECT COUNT(*) FROM users WHERE username = @uname", conn);
                    checkCmd.Parameters.AddWithValue("@uname", RegisterData.Username);
                    if (Convert.ToInt32(checkCmd.ExecuteScalar()) > 0)
                    {
                        MessageBox.Show("Username already taken. Please choose another.",
                            "Username Taken", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtUsername.Focus();
                        return;
                    }

                    // Insert new resident account
                    // Registered resident is linked to their barangay via barangay_id.
                    // This makes them visible to their barangay captain and LGU staff.
                    string residentCode = BsmartIdHelper.GetOrCreateResidentCode(
                        conn,
                        RegisterData.FirstName,
                        RegisterData.MiddleName,
                        RegisterData.LastName,
                        Convert.ToDateTime(RegisterData.Birthday),
                        RegisterData.BarangayID);

                    MySqlCommand cmd = new MySqlCommand(@"
                        INSERT INTO users
                            (resident_code, username, password, role, barangay_id, full_name,
                             first_name, middle_name, last_name, birthday, age, gender,
                             place_of_birth, civil_status, religion, citizenship,
                             address, email, mobile_number, is_frozen, is_archived)
                        VALUES
                            (@residentCode, @username, @password, 'Resident', @brgyId,
                             @fullName, @firstName, @middleName, @lastName,
                             @birthday, @age, @gender,
                             @placeOfBirth, @civilStatus, @religion, @citizenship,
                             @address, @email, @mobileNumber, 0, 0)", conn);

                    cmd.Parameters.AddWithValue("@residentCode", residentCode);
                    cmd.Parameters.AddWithValue("@username",     RegisterData.Username);
                    cmd.Parameters.AddWithValue("@password",     RegisterData.Password);
                    cmd.Parameters.AddWithValue("@brgyId",       RegisterData.BarangayID);
                    cmd.Parameters.AddWithValue("@fullName",     $"{RegisterData.FirstName} {RegisterData.LastName}");
                    cmd.Parameters.AddWithValue("@firstName",    RegisterData.FirstName);
                    cmd.Parameters.AddWithValue("@middleName",   RegisterData.MiddleName);
                    cmd.Parameters.AddWithValue("@lastName",     RegisterData.LastName);
                    cmd.Parameters.AddWithValue("@birthday",     RegisterData.Birthday);
                    cmd.Parameters.AddWithValue("@age",          RegisterData.Age);
                    cmd.Parameters.AddWithValue("@gender",       RegisterData.Gender);
                    cmd.Parameters.AddWithValue("@placeOfBirth", RegisterData.PlaceOfBirth);
                    cmd.Parameters.AddWithValue("@civilStatus",  RegisterData.CivilStatus);
                    cmd.Parameters.AddWithValue("@religion",     RegisterData.Religion);
                    cmd.Parameters.AddWithValue("@citizenship",  RegisterData.Citizenship);
                    cmd.Parameters.AddWithValue("@address",      RegisterData.Address);
                    cmd.Parameters.AddWithValue("@email",        RegisterData.Email);
                    cmd.Parameters.AddWithValue("@mobileNumber", RegisterData.MobileNumber);

                    cmd.ExecuteNonQuery();
                    EnsureRegisteredResidentRecord(conn, residentCode);

                    string registeredUsername = RegisterData.Username;
                    RegisterData.Clear();

                    MessageBox.Show($"Registration successful!\nYou can now log in as {registeredUsername}.",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    BsmartFormNavigator.Open(this, new loginRes());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Registration error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new Register2()); }

        private void txtUsername_TextChanged(object sender, EventArgs e) { }
        private void txtPassword_TextChanged(object sender, EventArgs e) { }
        private void txtPassConfirm_TextChanged(object sender, EventArgs e) { }

        private bool ResidentAccountAlreadyExists(MySqlConnection conn)
        {
            using MySqlCommand duplicate = new MySqlCommand(@"
                SELECT COUNT(*)
                FROM users
                WHERE role = 'Resident'
                  AND barangay_id = @barangayId
                  AND BINARY LOWER(TRIM(COALESCE(first_name, ''))) = BINARY LOWER(TRIM(@firstName))
                  AND BINARY LOWER(TRIM(COALESCE(middle_name, ''))) = BINARY LOWER(TRIM(@middleName))
                  AND BINARY LOWER(TRIM(COALESCE(last_name, ''))) = BINARY LOWER(TRIM(@lastName))
                  AND DATE(birthday) = @birthday
                  AND age = @age
                  AND BINARY LOWER(TRIM(COALESCE(gender, ''))) = BINARY LOWER(TRIM(@gender))
                  AND BINARY LOWER(TRIM(COALESCE(citizenship, ''))) = BINARY LOWER(TRIM(@citizenship))
                LIMIT 1", conn);

            duplicate.Parameters.AddWithValue("@barangayId", RegisterData.BarangayID);
            duplicate.Parameters.AddWithValue("@firstName", RegisterData.FirstName ?? "");
            duplicate.Parameters.AddWithValue("@middleName", RegisterData.MiddleName ?? "");
            duplicate.Parameters.AddWithValue("@lastName", RegisterData.LastName ?? "");
            duplicate.Parameters.AddWithValue("@birthday", Convert.ToDateTime(RegisterData.Birthday).Date);
            duplicate.Parameters.AddWithValue("@age", RegisterData.Age);
            duplicate.Parameters.AddWithValue("@gender", RegisterData.Gender ?? "");
            duplicate.Parameters.AddWithValue("@citizenship", RegisterData.Citizenship ?? "");

            return Convert.ToInt32(duplicate.ExecuteScalar()) > 0;
        }
        private void EnsureRegisteredResidentRecord(MySqlConnection conn, string residentCode)
        {
            using (MySqlCommand find = new MySqlCommand(@"
                SELECT ID
                FROM health_records
                WHERE is_archived = 0
                  AND barangay_id = @barangayId
                  AND (
                       COALESCE(resident_code, '') = @residentCode
                       OR (
                           LOWER(TRIM(first_name)) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(@firstName)) COLLATE utf8mb4_unicode_ci
                           AND LOWER(TRIM(COALESCE(middle_name, ''))) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(@middleName)) COLLATE utf8mb4_unicode_ci
                           AND LOWER(TRIM(last_name)) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(@lastName)) COLLATE utf8mb4_unicode_ci
                           AND DATE(birthday) = @birthday
                       )
                  )
                LIMIT 1", conn))
            {
                find.Parameters.AddWithValue("@barangayId", RegisterData.BarangayID);
                find.Parameters.AddWithValue("@residentCode", residentCode);
                find.Parameters.AddWithValue("@firstName", RegisterData.FirstName ?? "");
                find.Parameters.AddWithValue("@middleName", RegisterData.MiddleName ?? "");
                find.Parameters.AddWithValue("@lastName", RegisterData.LastName ?? "");
                find.Parameters.AddWithValue("@birthday", Convert.ToDateTime(RegisterData.Birthday).Date);

                object existing = find.ExecuteScalar();
                if (existing != null && existing != DBNull.Value)
                    return;
            }

            using MySqlCommand insert = new MySqlCommand(@"
                INSERT INTO health_records
                    (resident_code, first_name, middle_name, last_name, gender, age, birthday,
                     Treatment, Date, is_archived, barangay_id, Diagnosis, address, violation)
                VALUES
                    (@residentCode, @firstName, @middleName, @lastName, @gender, @age, @birthday,
                     'N/A', CURDATE(), 0, @barangayId, 'N/A', @address, NULL)", conn);
            insert.Parameters.AddWithValue("@residentCode", residentCode);
            insert.Parameters.AddWithValue("@firstName", RegisterData.FirstName);
            insert.Parameters.AddWithValue("@middleName", string.IsNullOrWhiteSpace(RegisterData.MiddleName) ? (object)DBNull.Value : RegisterData.MiddleName);
            insert.Parameters.AddWithValue("@lastName", RegisterData.LastName);
            insert.Parameters.AddWithValue("@gender", RegisterData.Gender);
            insert.Parameters.AddWithValue("@age", RegisterData.Age);
            insert.Parameters.AddWithValue("@birthday", RegisterData.Birthday);
            insert.Parameters.AddWithValue("@barangayId", RegisterData.BarangayID);
            insert.Parameters.AddWithValue("@address", string.IsNullOrWhiteSpace(RegisterData.Address) ? (object)DBNull.Value : RegisterData.Address);
            insert.ExecuteNonQuery();
        }
    }
}
