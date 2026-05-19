using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BSMART
{
    public partial class TayudBCManageRes : Form
    {
        private string connectionString = "Server=localhost;Database=bsmart_db;Uid=root;Pwd=;";
        private int selectedID = 0;
        private int selectedUserID = 0;
        private ListBox residentSearchDropDown;
        private bool suppressSearchEvents = false;

        public TayudBCManageRes()
        {
            InitializeComponent();
        }

        private void TayudBCManageRes_Load(object sender, EventArgs e)
        {
            EnsureResidentSearchDropDown();

            cmbAge.Items.Clear();
            for (int i = 1; i <= 120; i++) cmbAge.Items.Add(i.ToString());

            cmbGender.Items.Clear();
            cmbGender.Items.Add("Male");
            cmbGender.Items.Add("Female");

            cmbBarangay.Items.Clear();
            cmbBarangay.Items.Add(Session.BarangayName);
            cmbBarangay.SelectedIndex = 0;
            cmbBarangay.Enabled = false;

            dtpBirthday.Value = DateTime.Now;
            dtpRecord.Value = DateTime.Now;
            SetButtonState(false);
        }

        private string BarangayFilter(MySqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@brgyId", Session.BarangayID);
            return " AND barangay_id = @brgyId";
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            { MessageBox.Show("Please enter First Name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtFirstName.Focus(); return false; }
            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            { MessageBox.Show("Please enter Last Name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtLastName.Focus(); return false; }
            if (cmbAge.SelectedIndex == -1)
            { MessageBox.Show("Please select Age.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); cmbAge.Focus(); return false; }
            if (cmbGender.SelectedIndex == -1)
            { MessageBox.Show("Please select Gender.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); cmbGender.Focus(); return false; }
            return true;
        }

        private void ClearFields()
        {
            suppressSearchEvents = true;
            selectedID = 0;
            selectedUserID = 0;
            txtFirstName.Clear();
            txtMiddleName.Clear();
            txtLastName.Clear();
            cmbAge.SelectedIndex = -1;
            cmbGender.SelectedIndex = -1;
            dtpBirthday.Value = DateTime.Now;
            dtpRecord.Value = DateTime.Now;
            txtSitioAddress.Clear();
            txtViolation.Clear();
            txtSearch.Clear();
            suppressSearchEvents = false;
            HideResidentSearchDropDown();
            SetButtonState(false);
        }

        private void EnsureResidentSearchDropDown()
        {
            if (residentSearchDropDown != null) return;

            residentSearchDropDown = new ListBox
            {
                Name = "lstResidentSearchDropDown",
                Font = txtSearch.Font,
                IntegralHeight = false,
                Visible = false,
                Height = 170,
                Width = Math.Max(txtSearch.Width, 360),
                Location = new Point(txtSearch.Left, txtSearch.Bottom + 2),
                DisplayMember = "DisplayName"
            };

            residentSearchDropDown.MouseDown += ResidentSearchDropDown_MouseDown;
            residentSearchDropDown.KeyDown += ResidentSearchDropDown_KeyDown;
            txtSearch.KeyDown += TxtSearch_KeyDown;

            txtSearch.Parent.Controls.Add(residentSearchDropDown);
            residentSearchDropDown.BringToFront();
        }

        private void HideResidentSearchDropDown()
        {
            if (residentSearchDropDown != null) residentSearchDropDown.Visible = false;
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (residentSearchDropDown == null || !residentSearchDropDown.Visible) return;

            if (e.KeyCode == Keys.Down && residentSearchDropDown.Items.Count > 0)
            {
                residentSearchDropDown.Focus();
                residentSearchDropDown.SelectedIndex = 0;
                e.Handled = true;
            }
        }

        private void ResidentSearchDropDown_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SelectResidentSearchSuggestion();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                HideResidentSearchDropDown();
                txtSearch.Focus();
                e.Handled = true;
            }
        }

        private void ResidentSearchDropDown_MouseDown(object sender, MouseEventArgs e)
        {
            int index = residentSearchDropDown.IndexFromPoint(e.Location);
            if (index >= 0) residentSearchDropDown.SelectedIndex = index;
            SelectResidentSearchSuggestion();
        }

        private void SelectResidentSearchSuggestion()
        {
            if (residentSearchDropDown?.SelectedItem is not ResidentSearchSuggestion suggestion) return;

            suppressSearchEvents = true;
            txtSearch.Text = suggestion.DisplayName;
            txtSearch.SelectionStart = txtSearch.Text.Length;
            suppressSearchEvents = false;
            HideResidentSearchDropDown();
            LoadSelectedResident(suggestion.LookupText);
            txtSearch.Focus();
        }

        private void ShowResidentSearchSuggestions(string keyword)
        {
            EnsureResidentSearchDropDown();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                HideResidentSearchDropDown();
                return;
            }

            List<ResidentSearchSuggestion> suggestions = GetResidentSearchSuggestions(keyword);
            residentSearchDropDown.BeginUpdate();
            residentSearchDropDown.Items.Clear();
            foreach (ResidentSearchSuggestion suggestion in suggestions)
                residentSearchDropDown.Items.Add(suggestion);
            residentSearchDropDown.EndUpdate();

            residentSearchDropDown.Visible = suggestions.Count > 0;
            residentSearchDropDown.BringToFront();
        }

        private List<ResidentSearchSuggestion> GetResidentSearchSuggestions(string keyword)
        {
            List<ResidentSearchSuggestion> suggestions = new List<ResidentSearchSuggestion>();
            HashSet<string> seenResidents = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                BsmartIdHelper.EnsureCodes(conn);

                using (MySqlCommand cmd = new MySqlCommand(@"
                    SELECT COALESCE(resident_code, CAST(ID AS CHAR)) AS resident_key,
                           COALESCE(resident_code, CAST(ID AS CHAR)) AS resident_code,
                           first_name, middle_name, last_name
                    FROM health_records
                    WHERE is_archived = 0
                      AND barangay_id = @brgyId
                      AND (COALESCE(resident_code, CAST(ID AS CHAR)) LIKE @kw COLLATE utf8mb4_0900_ai_ci
                       OR  first_name LIKE @kw COLLATE utf8mb4_0900_ai_ci
                       OR  COALESCE(middle_name, '') LIKE @kw COLLATE utf8mb4_0900_ai_ci
                       OR  last_name LIKE @kw COLLATE utf8mb4_0900_ai_ci
                       OR  CONCAT_WS(' ', NULLIF(TRIM(first_name), ''), NULLIF(TRIM(middle_name), ''), NULLIF(TRIM(last_name), '')) LIKE @kw COLLATE utf8mb4_0900_ai_ci)
                    ORDER BY Date DESC, ID DESC
                    LIMIT 12", conn))
                {
                    cmd.Parameters.AddWithValue("@brgyId", Session.BarangayID);
                    cmd.Parameters.AddWithValue("@kw", "%" + keyword.Trim() + "%");
                    using (MySqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                            AddResidentSuggestion(suggestions, seenResidents, r["resident_key"].ToString(), r["resident_code"].ToString(), r["first_name"].ToString(), r["middle_name"].ToString(), r["last_name"].ToString());
                    }
                }

                using (MySqlCommand cmd = new MySqlCommand(@"
                    SELECT COALESCE(u.resident_code, CAST(u.id AS CHAR)) AS resident_key,
                           COALESCE(u.resident_code, CAST(u.id AS CHAR)) AS resident_code,
                           u.first_name, u.middle_name, u.last_name
                    FROM users u
                    LEFT JOIN barangays b ON b.id = u.barangay_id
                    WHERE LOWER(TRIM(u.role)) = 'resident'
                      AND COALESCE(u.is_archived, 0) = 0
                      AND (u.barangay_id = @brgyId OR LOWER(TRIM(COALESCE(b.name, ''))) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(@brgyName)) COLLATE utf8mb4_unicode_ci)
                      AND (COALESCE(u.resident_code, CAST(u.id AS CHAR)) LIKE @kw COLLATE utf8mb4_0900_ai_ci
                       OR  u.first_name LIKE @kw COLLATE utf8mb4_0900_ai_ci
                       OR  COALESCE(u.middle_name, '') LIKE @kw COLLATE utf8mb4_0900_ai_ci
                       OR  u.last_name LIKE @kw COLLATE utf8mb4_0900_ai_ci
                       OR  COALESCE(u.full_name, '') LIKE @kw COLLATE utf8mb4_0900_ai_ci
                       OR  CONCAT_WS(' ', NULLIF(TRIM(u.first_name), ''), NULLIF(TRIM(u.middle_name), ''), NULLIF(TRIM(u.last_name), '')) LIKE @kw COLLATE utf8mb4_0900_ai_ci)
                    ORDER BY u.id DESC
                    LIMIT 12", conn))
                {
                    cmd.Parameters.AddWithValue("@brgyId", Session.BarangayID);
                    cmd.Parameters.AddWithValue("@brgyName", Session.BarangayName ?? "");
                    cmd.Parameters.AddWithValue("@kw", "%" + keyword.Trim() + "%");
                    using (MySqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                            AddResidentSuggestion(suggestions, seenResidents, r["resident_key"].ToString(), r["resident_code"].ToString(), r["first_name"].ToString(), r["middle_name"].ToString(), r["last_name"].ToString());
                    }
                }
            }

            return suggestions.Take(12).ToList();
        }

        private static void AddResidentSuggestion(List<ResidentSearchSuggestion> suggestions, HashSet<string> seenResidents, string residentKey, string residentCode, string firstName, string middleName, string lastName)
        {
            string fullName = string.Join(" ", new[] { firstName, middleName, lastName }.Where(v => !string.IsNullOrWhiteSpace(v)));
            string uniqueKey = string.IsNullOrWhiteSpace(residentKey) ? fullName : residentKey;
            if (string.IsNullOrWhiteSpace(fullName) || !seenResidents.Add(uniqueKey)) return;

            suggestions.Add(new ResidentSearchSuggestion
            {
                LookupText = string.IsNullOrWhiteSpace(residentCode) ? fullName : residentCode,
                DisplayName = fullName
            });
        }

        private void SetButtonState(bool enabled)
        {
            btnUpdate.Enabled = enabled;
            btnArchive.Enabled = enabled;
            btnDelete.Enabled = enabled;
            btnRemoveViolation.Enabled = enabled;
            btnAdd.Enabled = true;
        }

        private void PopulateFromHealthRecord(MySqlDataReader r)
        {
            selectedID = Convert.ToInt32(r["ID"]);
            selectedUserID = 0;
            txtFirstName.Text = r["first_name"].ToString();
            txtMiddleName.Text = r["middle_name"] == DBNull.Value ? "" : r["middle_name"].ToString();
            txtLastName.Text = r["last_name"].ToString();
            cmbAge.SelectedItem = r["age"].ToString();
            cmbGender.SelectedItem = r["gender"].ToString();
            dtpBirthday.Value = Convert.ToDateTime(r["birthday"]);
            dtpRecord.Value = Convert.ToDateTime(r["Date"]);
            txtSitioAddress.Text = r["address"] == DBNull.Value ? "" : r["address"].ToString();
            try { txtViolation.Text = r["violation"] == DBNull.Value ? "" : r["violation"].ToString(); }
            catch { txtViolation.Clear(); }
            SetButtonState(true);
        }

        private void PopulateFromResidentAccount(MySqlDataReader r)
        {
            selectedID = 0;
            selectedUserID = Convert.ToInt32(r["id"]);
            txtFirstName.Text = r["first_name"].ToString();
            txtMiddleName.Text = r["middle_name"] == DBNull.Value ? "" : r["middle_name"].ToString();
            txtLastName.Text = r["last_name"].ToString();
            cmbAge.SelectedItem = r["age"].ToString();
            cmbGender.SelectedItem = r["gender"].ToString();
            if (r["birthday"] != DBNull.Value) dtpBirthday.Value = Convert.ToDateTime(r["birthday"]);
            dtpRecord.Value = DateTime.Now;
            txtSitioAddress.Text = r["address"] == DBNull.Value ? "" : r["address"].ToString();
            txtViolation.Clear();
            btnAdd.Enabled = true;
            btnUpdate.Enabled = true;
            btnArchive.Enabled = false;
            btnDelete.Enabled = false;
            btnRemoveViolation.Enabled = false;
        }

        private int EnsureResidentHealthRecord(MySqlConnection conn)
        {
            string residentCode = BsmartIdHelper.GetOrCreateResidentCode(
                conn, txtFirstName.Text.Trim(), txtMiddleName.Text.Trim(), txtLastName.Text.Trim(),
                dtpBirthday.Value.Date, Session.BarangayID, txtSitioAddress.Text.Trim());

            using (MySqlCommand find = new MySqlCommand(@"
                SELECT ID
                FROM health_records
                WHERE is_archived = 0
                  AND barangay_id = @brgyId
                  AND (
                       COALESCE(resident_code, '') = @residentCode
                       OR (
                           LOWER(TRIM(first_name)) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(@fn)) COLLATE utf8mb4_unicode_ci
                           AND LOWER(TRIM(COALESCE(middle_name, ''))) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(@mn)) COLLATE utf8mb4_unicode_ci
                           AND LOWER(TRIM(last_name)) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(@ln)) COLLATE utf8mb4_unicode_ci
                           AND (birthday IS NULL OR DATE(birthday) = @bday)
                       )
                  )
                ORDER BY ID ASC
                LIMIT 1", conn))
            {
                find.Parameters.AddWithValue("@brgyId", Session.BarangayID);
                find.Parameters.AddWithValue("@residentCode", residentCode);
                find.Parameters.AddWithValue("@fn", txtFirstName.Text.Trim());
                find.Parameters.AddWithValue("@mn", txtMiddleName.Text.Trim());
                find.Parameters.AddWithValue("@ln", txtLastName.Text.Trim());
                find.Parameters.AddWithValue("@bday", dtpBirthday.Value.Date);
                object existing = find.ExecuteScalar();
                if (existing != null && existing != DBNull.Value)
                    return Convert.ToInt32(existing);
            }

            using MySqlCommand insert = new MySqlCommand(@"
                INSERT INTO health_records
                    (resident_code, first_name, middle_name, last_name, gender, age, birthday,
                     Treatment, Date, is_archived, barangay_id, Diagnosis, address, violation)
                VALUES
                    (@residentCode, @fn, @mn, @ln, @gender, @age, @bday,
                     'N/A', @date, 0, @brgyId, 'N/A', @address, @violation)", conn);
            insert.Parameters.AddWithValue("@residentCode", residentCode);
            insert.Parameters.AddWithValue("@fn", txtFirstName.Text.Trim());
            insert.Parameters.AddWithValue("@mn", string.IsNullOrWhiteSpace(txtMiddleName.Text) ? (object)DBNull.Value : txtMiddleName.Text.Trim());
            insert.Parameters.AddWithValue("@ln", txtLastName.Text.Trim());
            insert.Parameters.AddWithValue("@gender", cmbGender.SelectedItem.ToString());
            insert.Parameters.AddWithValue("@age", Convert.ToInt32(cmbAge.SelectedItem));
            insert.Parameters.AddWithValue("@bday", dtpBirthday.Value.Date);
            insert.Parameters.AddWithValue("@date", dtpRecord.Value.Date);
            insert.Parameters.AddWithValue("@brgyId", Session.BarangayID);
            insert.Parameters.AddWithValue("@address", string.IsNullOrWhiteSpace(txtSitioAddress.Text) ? (object)DBNull.Value : txtSitioAddress.Text.Trim());
            insert.Parameters.AddWithValue("@violation", string.IsNullOrWhiteSpace(txtViolation.Text) ? (object)DBNull.Value : txtViolation.Text.Trim());
            insert.ExecuteNonQuery();
            return Convert.ToInt32(insert.LastInsertedId);
        }
private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (suppressSearchEvents) return;

            string keyword = txtSearch.Text.Trim();
            if (string.IsNullOrWhiteSpace(keyword))
            {
                ClearFields();
                return;
            }

            ShowResidentSearchSuggestions(keyword);
        }

        private void LoadSelectedResident(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) { ClearFields(); return; }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    BsmartIdHelper.EnsureCodes(conn);
                    MySqlCommand cmd = new MySqlCommand("", conn);
                    string brgyFilter = BarangayFilter(cmd);

                    cmd.CommandText = $@"SELECT * FROM health_records
                                         WHERE is_archived = 0{brgyFilter}
                                         AND (COALESCE(resident_code, CAST(ID AS CHAR)) LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                         OR   first_name LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                         OR   COALESCE(middle_name, '') LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                         OR   last_name  LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                         OR   CONCAT_WS(' ', NULLIF(TRIM(first_name), ''), NULLIF(TRIM(middle_name), ''), NULLIF(TRIM(last_name), '')) LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                         OR   TRIM(CONCAT(first_name,' ',last_name)) LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                         OR   gender LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                         OR   CAST(age AS CHAR) LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                         OR   Diagnosis LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                         OR   Treatment LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                         OR   COALESCE(address, '') LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                         OR   COALESCE(violation, '') LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                         OR   DATE_FORMAT(Date, '%d/%m/%Y') LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                         OR   DATE_FORMAT(Date, '%m/%d/%Y') LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                         OR   DATE_FORMAT(Date, '%Y-%m-%d') LIKE @kw COLLATE utf8mb4_0900_ai_ci)
                                         ORDER BY Date DESC LIMIT 1";
                    cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");

                    using (MySqlDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            PopulateFromHealthRecord(r);
                            return;
                        }
                    }

                    cmd.Parameters.Clear();
                    cmd.CommandText = @"SELECT u.*
                                         FROM users u
                                         LEFT JOIN barangays b ON b.id = u.barangay_id
                                         WHERE LOWER(TRIM(u.role)) = 'resident'
                                           AND COALESCE(u.is_archived, 0) = 0
                                           AND (u.barangay_id = @brgyId OR LOWER(TRIM(COALESCE(b.name, ''))) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(@brgyName)) COLLATE utf8mb4_unicode_ci)
                                           AND (COALESCE(u.resident_code, CAST(u.id AS CHAR)) LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                            OR  u.first_name LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                            OR  COALESCE(u.middle_name, '') LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                            OR  u.last_name LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                            OR  COALESCE(u.full_name, '') LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                            OR  CONCAT_WS(' ', NULLIF(TRIM(u.first_name), ''), NULLIF(TRIM(u.middle_name), ''), NULLIF(TRIM(u.last_name), '')) LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                            OR  TRIM(CONCAT(u.first_name,' ',u.last_name)) LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                            OR  u.gender LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                            OR  CAST(u.age AS CHAR) LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                            OR  COALESCE(u.address, '') LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                            OR  COALESCE(u.email, '') LIKE @kw COLLATE utf8mb4_0900_ai_ci
                                            OR  COALESCE(u.mobile_number, '') LIKE @kw COLLATE utf8mb4_0900_ai_ci)
                                         ORDER BY u.id DESC LIMIT 1";
                    cmd.Parameters.AddWithValue("@brgyId", Session.BarangayID);
                    cmd.Parameters.AddWithValue("@brgyName", Session.BarangayName ?? "");
                    cmd.Parameters.AddWithValue("@kw", "%" + keyword + "%");
                    using (MySqlDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read()) PopulateFromResidentAccount(r);
                        else ClearFields();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    BsmartIdHelper.EnsureCodes(conn);
                    string residentCode = BsmartIdHelper.GetOrCreateResidentCode(
                        conn, txtFirstName.Text.Trim(), txtMiddleName.Text.Trim(), txtLastName.Text.Trim(), dtpBirthday.Value.Date, Session.BarangayID, txtSitioAddress.Text.Trim());

                    MySqlCommand cmd = new MySqlCommand(@"
                        INSERT INTO health_records
                            (resident_code, first_name, middle_name, last_name, gender, age, birthday,
                             Treatment, Date, is_archived, barangay_id,
                             Diagnosis, address, violation)
                        VALUES
                            (@residentCode, @fn, @mn, @ln, @gender, @age, @bday,
                             @treatment, @date, 0, @brgyId,
                             @diagnosis, @address, @violation)", conn);

                    cmd.Parameters.AddWithValue("@residentCode", residentCode);
                    cmd.Parameters.AddWithValue("@fn", txtFirstName.Text.Trim());
                    cmd.Parameters.AddWithValue("@mn", string.IsNullOrWhiteSpace(txtMiddleName.Text) ? (object)DBNull.Value : txtMiddleName.Text.Trim());
                    cmd.Parameters.AddWithValue("@ln", txtLastName.Text.Trim());
                    cmd.Parameters.AddWithValue("@gender", cmbGender.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@age", Convert.ToInt32(cmbAge.SelectedItem));
                    cmd.Parameters.AddWithValue("@bday", dtpBirthday.Value.Date);
                    cmd.Parameters.AddWithValue("@treatment", "N/A");
                    cmd.Parameters.AddWithValue("@date", dtpRecord.Value.Date);
                    cmd.Parameters.AddWithValue("@brgyId", Session.BarangayID);
                    cmd.Parameters.AddWithValue("@diagnosis", "N/A");
                    cmd.Parameters.AddWithValue("@address", string.IsNullOrWhiteSpace(txtSitioAddress.Text) ? (object)DBNull.Value : txtSitioAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@violation", string.IsNullOrWhiteSpace(txtViolation.Text)
                        ? (object)DBNull.Value : txtViolation.Text.Trim());

                    cmd.ExecuteNonQuery();
                    BsmartAuditService.Log("Add Resident Health Record", "health_records", cmd.LastInsertedId);
                    MessageBox.Show("Record added successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                    BsmartNotificationService.RefreshOpenIndicators(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedID == 0 && selectedUserID == 0) return;
            if (!ValidateInputs()) return;
            if (MessageBox.Show("Update this record?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    BsmartIdHelper.EnsureCodes(conn);
                    if (selectedID == 0 && selectedUserID > 0)
                    {
                        MySqlCommand updateUser = new MySqlCommand(@"
                            UPDATE users SET
                                first_name = @fn,
                                middle_name = @mn,
                                last_name = @ln,
                                full_name = @fullName,
                                gender = @gender,
                                age = @age,
                                birthday = @bday,
                                address = @address
                            WHERE id = @id", conn);
                        updateUser.Parameters.AddWithValue("@fn", txtFirstName.Text.Trim());
                        updateUser.Parameters.AddWithValue("@mn", string.IsNullOrWhiteSpace(txtMiddleName.Text) ? (object)DBNull.Value : txtMiddleName.Text.Trim());
                        updateUser.Parameters.AddWithValue("@ln", txtLastName.Text.Trim());
                        updateUser.Parameters.AddWithValue("@fullName", string.Join(" ", new[] { txtFirstName.Text.Trim(), txtMiddleName.Text.Trim(), txtLastName.Text.Trim() }.Where(v => !string.IsNullOrWhiteSpace(v))));
                        updateUser.Parameters.AddWithValue("@gender", cmbGender.SelectedItem.ToString());
                        updateUser.Parameters.AddWithValue("@age", Convert.ToInt32(cmbAge.SelectedItem));
                        updateUser.Parameters.AddWithValue("@bday", dtpBirthday.Value.Date);
                        updateUser.Parameters.AddWithValue("@address", string.IsNullOrWhiteSpace(txtSitioAddress.Text) ? (object)DBNull.Value : txtSitioAddress.Text.Trim());
                        updateUser.Parameters.AddWithValue("@id", selectedUserID);
                        updateUser.ExecuteNonQuery();

                        string residentCode = BsmartIdHelper.GetOrCreateResidentCode(
                            conn, txtFirstName.Text.Trim(), txtMiddleName.Text.Trim(), txtLastName.Text.Trim(),
                            dtpBirthday.Value.Date, Session.BarangayID, txtSitioAddress.Text.Trim());
                        int healthRecordId = EnsureResidentHealthRecord(conn);
                        MySqlCommand updateHealth = new MySqlCommand(@"
                            UPDATE health_records SET
                                resident_code = @residentCode,
                                first_name = @fn,
                                middle_name = @mn,
                                last_name = @ln,
                                gender = @gender,
                                age = @age,
                                birthday = @bday,
                                Date = @date,
                                address = @address,
                                violation = @violation
                            WHERE ID = @id", conn);
                        updateHealth.Parameters.AddWithValue("@residentCode", residentCode);
                        updateHealth.Parameters.AddWithValue("@fn", txtFirstName.Text.Trim());
                        updateHealth.Parameters.AddWithValue("@mn", string.IsNullOrWhiteSpace(txtMiddleName.Text) ? (object)DBNull.Value : txtMiddleName.Text.Trim());
                        updateHealth.Parameters.AddWithValue("@ln", txtLastName.Text.Trim());
                        updateHealth.Parameters.AddWithValue("@gender", cmbGender.SelectedItem.ToString());
                        updateHealth.Parameters.AddWithValue("@age", Convert.ToInt32(cmbAge.SelectedItem));
                        updateHealth.Parameters.AddWithValue("@bday", dtpBirthday.Value.Date);
                        updateHealth.Parameters.AddWithValue("@date", dtpRecord.Value.Date);
                        updateHealth.Parameters.AddWithValue("@address", string.IsNullOrWhiteSpace(txtSitioAddress.Text) ? (object)DBNull.Value : txtSitioAddress.Text.Trim());
                        updateHealth.Parameters.AddWithValue("@violation", string.IsNullOrWhiteSpace(txtViolation.Text)
                            ? (object)DBNull.Value : txtViolation.Text.Trim());
                        updateHealth.Parameters.AddWithValue("@id", healthRecordId);
                        updateHealth.ExecuteNonQuery();

                        BsmartAuditService.Log("Update Resident", "users", selectedUserID);
                        BsmartAuditService.Log("Update Resident Health Record", "health_records", healthRecordId);
                        MessageBox.Show("Resident account information updated!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearFields();
                        return;
                    }

                    MySqlCommand cmd = new MySqlCommand(@"
                        UPDATE health_records SET
                            first_name = @fn, middle_name = @mn, last_name = @ln,
                            gender = @gender, age = @age, birthday = @bday,
                            Date = @date,
                            address = @address,
                            violation = @violation
                        WHERE ID = @id", conn);

                    cmd.Parameters.AddWithValue("@fn", txtFirstName.Text.Trim());
                    cmd.Parameters.AddWithValue("@mn", string.IsNullOrWhiteSpace(txtMiddleName.Text) ? (object)DBNull.Value : txtMiddleName.Text.Trim());
                    cmd.Parameters.AddWithValue("@ln", txtLastName.Text.Trim());
                    cmd.Parameters.AddWithValue("@gender", cmbGender.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@age", Convert.ToInt32(cmbAge.SelectedItem));
                    cmd.Parameters.AddWithValue("@bday", dtpBirthday.Value.Date);
                    cmd.Parameters.AddWithValue("@date", dtpRecord.Value.Date);
                    cmd.Parameters.AddWithValue("@address", string.IsNullOrWhiteSpace(txtSitioAddress.Text) ? (object)DBNull.Value : txtSitioAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@violation", string.IsNullOrWhiteSpace(txtViolation.Text)
                        ? (object)DBNull.Value : txtViolation.Text.Trim());
                    cmd.Parameters.AddWithValue("@id", selectedID);

                    cmd.ExecuteNonQuery();
                    BsmartAuditService.Log("Update Resident Health Record", "health_records", selectedID);
                    MessageBox.Show("Record updated!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnArchive_Click(object sender, EventArgs e)
        {
            if (selectedID == 0) return;
            if (MessageBox.Show("Archive this record?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "UPDATE health_records SET is_archived = 1 WHERE ID = @id", conn);
                    cmd.Parameters.AddWithValue("@id", selectedID);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record archived!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error archiving: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedID == 0) return;
            if (MessageBox.Show("PERMANENTLY delete? This cannot be undone!", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "DELETE FROM health_records WHERE ID = @id", conn);
                    cmd.Parameters.AddWithValue("@id", selectedID);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record deleted!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRemoveViolation_Click(object sender, EventArgs e)
        {
            if (selectedID == 0) return;
            if (MessageBox.Show("Remove violation for this resident?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "UPDATE health_records SET violation = NULL WHERE ID = @id", conn);
                    cmd.Parameters.AddWithValue("@id", selectedID);
                    cmd.ExecuteNonQuery();
                    txtViolation.Clear();
                    MessageBox.Show("Violation removed!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Logout?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            { Session.Clear(); BsmartFormNavigator.Open(this, new loginBCap()); }
        }

        private void btnBCapDash_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudBCapDash()); }

        private void btnResManageRecord_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudBCManageRes()); }

        private void btnResViewRecord_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudBCViewRes()); }

        private void btnManageResAcc_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudBCapManageResAcc()); }

        private void btnViewResAcc_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudBCapViewResAcc()); }

        private void btnManageFrozenAcc_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudBCapManageFAcc()); }

        private void btnViewHealthRep_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudBCViewResHealthRep()); }

        private void btnViewArchive_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudBCapViewArch()); }

        private void txtFirstName_TextChanged(object sender, EventArgs e) { }
        private void txtLastName_TextChanged(object sender, EventArgs e) { }
        private void cmbAge_SelectedIndexChanged(object sender, EventArgs e) { }
        private void cmbGender_SelectedIndexChanged(object sender, EventArgs e) { }
        private void dtpBirthday_ValueChanged(object sender, EventArgs e) { }
        private void dtpRecord_ValueChanged(object sender, EventArgs e) { }
        private void cmbBarangay_SelectedIndexChanged(object sender, EventArgs e) { }
        private void txtSitio_TextChanged(object sender, EventArgs e) { }
        private void txtViolation_TextChanged(object sender, EventArgs e) { }

        private void txtMiddleName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
