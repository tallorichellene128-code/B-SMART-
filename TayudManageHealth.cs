using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BSMART
{
    public partial class TayudManageHealth : Form
    {
        private string connectionString = "Server=localhost;Database=bsmart_db;Uid=root;Pwd=;";
        private int selectedHealthID = 0;
        private int selectedUserID = 0;
        private ListBox residentSearchDropDown;
        private bool suppressSearchEvents = false;
        private TextBox _txtMiddlename;
        private Label _lblMiddleName;
        private TextBox _txtSitio;
        private Label _lblSitio;

        public TayudManageHealth()
        {
            InitializeComponent();
            EnsureMiddleNameControl();
            EnsureSitioControl();
            EnsureResidentSearchDropDown();
        }

        private void EnsureMiddleNameControl()
        {
            if (_txtMiddlename != null) return;
            Control[] existingControls = Controls.Find("txtMiddlename", true);
            if (existingControls.Length == 0)
                existingControls = Controls.Find("txtMiddleName", true);
            if (existingControls.Length > 0 && existingControls[0] is TextBox existingTextBox)
            {
                _txtMiddlename = existingTextBox;
                return;
            }

            txtFirstName.Size = new Size(300, txtFirstName.Height);
            txtLastName.Location = new Point(724, txtLastName.Location.Y);
            txtLastName.Size = new Size(300, txtLastName.Height);
            label5.Location = new Point(724, label5.Location.Y);

            _lblMiddleName = new Label
            {
                Text = "Middle Name",
                AutoSize = true,
                Font = label4.Font,
                Location = new Point(382, label4.Location.Y)
            };

            _txtMiddlename = new TextBox
            {
                Name = "txtMiddlename",
                Font = txtFirstName.Font,
                Location = new Point(382, txtFirstName.Location.Y),
                Size = new Size(300, txtFirstName.Height)
            };

            panel3.Controls.Add(_lblMiddleName);
            panel3.Controls.Add(_txtMiddlename);
        }

        private void EnsureSitioControl()
        {
            if (_txtSitio != null) return;

            string[] names = { "txtSitio", "txtAddress", "txtSitioAddress" };
            foreach (string name in names)
            {
                Control[] existingControls = Controls.Find(name, true);
                if (existingControls.Length > 0 && existingControls[0] is TextBox existingTextBox)
                {
                    _txtSitio = existingTextBox;
                    return;
                }
            }

            _lblSitio = new Label
            {
                Text = "Sitio/Address",
                AutoSize = true,
                Font = label9.Font,
                Location = new Point(txtTreatment.Left, label9.Location.Y)
            };

            _txtSitio = new TextBox
            {
                Name = "txtSitio",
                Font = txtTreatment.Font,
                Location = new Point(txtTreatment.Left, txtDiagnosis.Top),
                Size = txtTreatment.Size
            };

            panel3.Controls.Add(_lblSitio);
            panel3.Controls.Add(_txtSitio);
            _txtSitio.BringToFront();
        }

        private void TayudManageHealth_Load(object sender, EventArgs e)
        {
            EnsureResidentSearchDropDown();

            try
            {
                cmbAge.Items.Clear();
                for (int age = 1; age <= 120; age++)
                    cmbAge.Items.Add(age.ToString());

                cmbGender.Items.Clear();
                cmbGender.Items.Add("Male");
                cmbGender.Items.Add("Female");

                dtpBirthday.Value = DateTime.Now;
                dtpRecord.Value = DateTime.Now;

                btnUpdate.Enabled = false;
                btnArchive.Enabled = false;
                btnDelete.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =============================================
        // BARANGAY FILTER HELPER
        // Mayor sees all; others see only their barangay
        // =============================================
        private string BarangayFilter(MySqlCommand cmd)
        {
            if (Session.IsMayor) return "";
            cmd.Parameters.AddWithValue("@brgyId", Session.BarangayID);
            cmd.Parameters.AddWithValue("@brgyName", Session.BarangayName);
            return " AND (barangay_id = @brgyId OR barangay_id IN (SELECT id FROM barangays WHERE name = @brgyName))";
        }

        // =============================================
        // VALIDATE
        // =============================================
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
            if (string.IsNullOrWhiteSpace(txtDiagnosis.Text))
            { MessageBox.Show("Please enter Diagnosis.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtDiagnosis.Focus(); return false; }
            if (string.IsNullOrWhiteSpace(txtTreatment.Text))
            { MessageBox.Show("Please enter Treatment.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtTreatment.Focus(); return false; }
            // assigned_doc_name is optional ÃƒÂ¢Ã¢â€šÂ¬Ã¢â‚¬Â no validation required
            return true;
        }

        // =============================================
        // CLEAR
        // =============================================
        private void ClearFields()
        {
            suppressSearchEvents = true;
            txtFirstName.Clear();
            _txtMiddlename.Clear();
            txtLastName.Clear();
            cmbAge.SelectedIndex = -1;
            cmbGender.SelectedIndex = -1;
            dtpBirthday.Value = DateTime.Now;
            dtpRecord.Value = DateTime.Now;
            txtDiagnosis.Clear();
            txtTreatment.Clear();
            txtAssignDocName.Clear();   // ? clear assigned doctor/nurse field
            _txtSitio.Clear();

            selectedHealthID = 0;
            selectedUserID = 0;
            txtSearch.Clear();
            suppressSearchEvents = false;
            HideResidentSearchDropDown();
            btnAdd.Enabled = true;
            btnUpdate.Enabled = false;
            btnArchive.Enabled = false;
            btnDelete.Enabled = false;
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

                using (MySqlCommand cmd = new MySqlCommand("", conn))
                {
                    string brgyFilter = BarangayFilter(cmd);
                    cmd.CommandText = $@"
                        SELECT COALESCE(resident_code, CAST(ID AS CHAR)) AS resident_key,
                               COALESCE(resident_code, CAST(ID AS CHAR)) AS resident_code,
                               first_name, middle_name, last_name
                        FROM health_records
                        WHERE is_archived = 0{brgyFilter}
                          AND (COALESCE(resident_code, CAST(ID AS CHAR)) LIKE @kw COLLATE utf8mb4_0900_ai_ci
                           OR  first_name LIKE @kw COLLATE utf8mb4_0900_ai_ci
                           OR  COALESCE(middle_name, '') LIKE @kw COLLATE utf8mb4_0900_ai_ci
                           OR  last_name LIKE @kw COLLATE utf8mb4_0900_ai_ci
                           OR  CONCAT_WS(' ', NULLIF(TRIM(first_name), ''), NULLIF(TRIM(middle_name), ''), NULLIF(TRIM(last_name), '')) LIKE @kw COLLATE utf8mb4_0900_ai_ci)
                        ORDER BY Date DESC, ID DESC
                        LIMIT 12";
                    cmd.Parameters.AddWithValue("@kw", "%" + keyword.Trim() + "%");
                    using (MySqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                            AddResidentSuggestion(suggestions, seenResidents, r["resident_key"].ToString(), r["resident_code"].ToString(), r["first_name"].ToString(), r["middle_name"].ToString(), r["last_name"].ToString());
                    }
                }

                using (MySqlCommand cmd = new MySqlCommand("", conn))
                {
                    string userBarangayFilter = Session.IsMayor
                        ? ""
                        : " AND (u.barangay_id = @brgyId OR LOWER(TRIM(COALESCE(b.name, ''))) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(@brgyName)) COLLATE utf8mb4_unicode_ci)";
                    cmd.CommandText = $@"
                        SELECT COALESCE(u.resident_code, CAST(u.id AS CHAR)) AS resident_key,
                               COALESCE(u.resident_code, CAST(u.id AS CHAR)) AS resident_code,
                               u.first_name, u.middle_name, u.last_name
                        FROM users u
                        LEFT JOIN barangays b ON b.id = u.barangay_id
                        WHERE LOWER(TRIM(u.role)) = 'resident'
                          AND COALESCE(u.is_archived, 0) = 0{userBarangayFilter}
                          AND (COALESCE(u.resident_code, CAST(u.id AS CHAR)) LIKE @kw COLLATE utf8mb4_0900_ai_ci
                           OR  u.first_name LIKE @kw COLLATE utf8mb4_0900_ai_ci
                           OR  COALESCE(u.middle_name, '') LIKE @kw COLLATE utf8mb4_0900_ai_ci
                           OR  u.last_name LIKE @kw COLLATE utf8mb4_0900_ai_ci
                           OR  COALESCE(u.full_name, '') LIKE @kw COLLATE utf8mb4_0900_ai_ci
                           OR  CONCAT_WS(' ', NULLIF(TRIM(u.first_name), ''), NULLIF(TRIM(u.middle_name), ''), NULLIF(TRIM(u.last_name), '')) LIKE @kw COLLATE utf8mb4_0900_ai_ci)
                        ORDER BY u.id DESC
                        LIMIT 12";
                    if (!Session.IsMayor)
                    {
                        cmd.Parameters.AddWithValue("@brgyId", Session.BarangayID);
                        cmd.Parameters.AddWithValue("@brgyName", Session.BarangayName ?? "");
                    }
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

        private void PopulateFromHealthRecord(MySqlDataReader reader)
        {
            selectedHealthID = Convert.ToInt32(reader["ID"]);
            selectedUserID = 0;
            txtFirstName.Text = reader["first_name"].ToString();
            _txtMiddlename.Text = reader["middle_name"] == DBNull.Value ? "" : reader["middle_name"].ToString();
            txtLastName.Text = reader["last_name"].ToString();
            cmbAge.SelectedItem = reader["age"].ToString();
            cmbGender.SelectedItem = reader["gender"].ToString();
            dtpBirthday.Value = Convert.ToDateTime(reader["birthday"]);
            dtpRecord.Value = Convert.ToDateTime(reader["Date"]);
            txtDiagnosis.Text = reader["Diagnosis"].ToString();
            txtTreatment.Text = reader["Treatment"].ToString();
            _txtSitio.Text = reader["address"] == DBNull.Value ? "" : reader["address"].ToString();
            txtAssignDocName.Text = reader["assigned_doc_name"] == DBNull.Value ? "" : reader["assigned_doc_name"].ToString();

            btnUpdate.Enabled = true;
            btnArchive.Enabled = true;
            btnDelete.Enabled = true;
            btnAdd.Enabled = true;
        }

        private void PopulateFromResidentAccount(MySqlDataReader reader)
        {
            selectedHealthID = 0;
            selectedUserID = Convert.ToInt32(reader["id"]);
            txtFirstName.Text = reader["first_name"].ToString();
            _txtMiddlename.Text = reader["middle_name"] == DBNull.Value ? "" : reader["middle_name"].ToString();
            txtLastName.Text = reader["last_name"].ToString();
            cmbAge.SelectedItem = reader["age"].ToString();
            cmbGender.SelectedItem = reader["gender"].ToString();
            if (reader["birthday"] != DBNull.Value) dtpBirthday.Value = Convert.ToDateTime(reader["birthday"]);
            dtpRecord.Value = DateTime.Now;
            txtDiagnosis.Clear();
            txtTreatment.Clear();
            txtAssignDocName.Clear();
            _txtSitio.Text = reader["address"] == DBNull.Value ? "" : reader["address"].ToString();

            btnAdd.Enabled = true;
            btnUpdate.Enabled = false;
            btnArchive.Enabled = false;
            btnDelete.Enabled = false;
        }

        // =============================================
        // SEARCH ÃƒÂ¢Ã¢â€šÂ¬Ã¢â‚¬Â scoped to current barangay
        // =============================================
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
            try
            {
                if (string.IsNullOrWhiteSpace(keyword)) { ClearFields(); return; }

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    BsmartIdHelper.EnsureCodes(conn);

                    MySqlCommand cmd = new MySqlCommand("", conn);
                    string brgyFilter = BarangayFilter(cmd);

                    cmd.CommandText = $@"SELECT * FROM health_records
                                         WHERE is_archived = 0{brgyFilter}
                                         AND (COALESCE(resident_code, CAST(ID AS CHAR)) COLLATE utf8mb4_0900_ai_ci LIKE @keyword COLLATE utf8mb4_0900_ai_ci
                                         OR   first_name COLLATE utf8mb4_0900_ai_ci LIKE @keyword COLLATE utf8mb4_0900_ai_ci
                                         OR   COALESCE(middle_name, '') COLLATE utf8mb4_0900_ai_ci LIKE @keyword COLLATE utf8mb4_0900_ai_ci
                                         OR   last_name COLLATE utf8mb4_0900_ai_ci LIKE @keyword COLLATE utf8mb4_0900_ai_ci
                                         OR   CONCAT_WS(' ', NULLIF(TRIM(first_name), ''), NULLIF(TRIM(middle_name), ''), NULLIF(TRIM(last_name), '')) COLLATE utf8mb4_0900_ai_ci LIKE @keyword COLLATE utf8mb4_0900_ai_ci
                                         OR   TRIM(CONCAT(first_name,' ',last_name)) COLLATE utf8mb4_0900_ai_ci LIKE @keyword COLLATE utf8mb4_0900_ai_ci
                                         OR   gender COLLATE utf8mb4_0900_ai_ci LIKE @keyword COLLATE utf8mb4_0900_ai_ci
                                         OR   CAST(age AS CHAR) LIKE @keyword COLLATE utf8mb4_0900_ai_ci
                                         OR   Diagnosis COLLATE utf8mb4_0900_ai_ci LIKE @keyword COLLATE utf8mb4_0900_ai_ci
                                         OR   Treatment COLLATE utf8mb4_0900_ai_ci LIKE @keyword COLLATE utf8mb4_0900_ai_ci
                                         OR   assigned_doc_name COLLATE utf8mb4_0900_ai_ci LIKE @keyword COLLATE utf8mb4_0900_ai_ci
                                         OR   COALESCE(address, '') COLLATE utf8mb4_0900_ai_ci LIKE @keyword COLLATE utf8mb4_0900_ai_ci
                                         OR   COALESCE(violation, '') COLLATE utf8mb4_0900_ai_ci LIKE @keyword COLLATE utf8mb4_0900_ai_ci
                                         OR   DATE_FORMAT(Date, '%d/%m/%Y') LIKE @keyword COLLATE utf8mb4_0900_ai_ci
                                         OR   DATE_FORMAT(Date, '%m/%d/%Y') LIKE @keyword COLLATE utf8mb4_0900_ai_ci
                                         OR   DATE_FORMAT(Date, '%Y-%m-%d') LIKE @keyword COLLATE utf8mb4_0900_ai_ci)
                                         ORDER BY Date DESC
                                         LIMIT 1";

                    cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            PopulateFromHealthRecord(reader);
                            return;
                        }
                    }

                    cmd.Parameters.Clear();
                    string userBarangayFilter = Session.IsMayor
                        ? ""
                        : " AND (u.barangay_id = @brgyId OR LOWER(TRIM(COALESCE(b.name, ''))) COLLATE utf8mb4_unicode_ci = LOWER(TRIM(@brgyName)) COLLATE utf8mb4_unicode_ci)";
                    cmd.CommandText = $@"SELECT u.*
                                         FROM users u
                                         LEFT JOIN barangays b ON b.id = u.barangay_id
                                         WHERE LOWER(TRIM(u.role)) = 'resident'
                                           AND COALESCE(u.is_archived, 0) = 0{userBarangayFilter}
                                           AND (COALESCE(u.resident_code, CAST(u.id AS CHAR)) LIKE @keyword COLLATE utf8mb4_0900_ai_ci
                                            OR  u.first_name LIKE @keyword COLLATE utf8mb4_0900_ai_ci
                                            OR  COALESCE(u.middle_name, '') LIKE @keyword COLLATE utf8mb4_0900_ai_ci
                                            OR  u.last_name LIKE @keyword COLLATE utf8mb4_0900_ai_ci
                                            OR  COALESCE(u.full_name, '') LIKE @keyword COLLATE utf8mb4_0900_ai_ci
                                            OR  CONCAT_WS(' ', NULLIF(TRIM(u.first_name), ''), NULLIF(TRIM(u.middle_name), ''), NULLIF(TRIM(u.last_name), '')) LIKE @keyword COLLATE utf8mb4_0900_ai_ci
                                            OR  TRIM(CONCAT(u.first_name,' ',u.last_name)) LIKE @keyword COLLATE utf8mb4_0900_ai_ci)
                                         ORDER BY u.id DESC LIMIT 1";
                    if (!Session.IsMayor)
                    {
                        cmd.Parameters.AddWithValue("@brgyId", Session.BarangayID);
                        cmd.Parameters.AddWithValue("@brgyName", Session.BarangayName ?? "");
                    }
                    cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) PopulateFromResidentAccount(reader);
                        else ClearFields();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search Error: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                    // NULL for Mayor (no specific barangay), or staff's barangay ID
                    object brgyValue = Session.IsMayor
                        ? (object)DBNull.Value
                        : Session.BarangayID;
                    int barangayId = Session.IsMayor ? 0 : Session.BarangayID;
                    string residentCode = barangayId == 0
                        ? ""
                        : BsmartIdHelper.GetOrCreateResidentCode(conn, txtFirstName.Text.Trim(), _txtMiddlename.Text.Trim(), txtLastName.Text.Trim(), dtpBirthday.Value.Date, barangayId, _txtSitio.Text.Trim());

                    // NULL if doctor/nurse field is left blank
                    object docValue = string.IsNullOrWhiteSpace(txtAssignDocName.Text)
                        ? (object)DBNull.Value
                        : txtAssignDocName.Text.Trim();

                    MySqlCommand cmd = new MySqlCommand(@"
                        INSERT INTO health_records
                            (resident_code, first_name, middle_name, last_name, Diagnosis, Treatment, assigned_doc_name,
                             Date, gender, age, birthday, address, is_archived, barangay_id)
                        VALUES
                            (@residentCode, @firstName, @middleName, @lastName, @diagnosis, @treatment, @docName,
                             @date, @gender, @age, @birthday, @address, 0, @brgyId)", conn);

                    cmd.Parameters.AddWithValue("@residentCode", string.IsNullOrWhiteSpace(residentCode) ? (object)DBNull.Value : residentCode);
                    cmd.Parameters.AddWithValue("@firstName", txtFirstName.Text.Trim());
                    cmd.Parameters.AddWithValue("@middleName", string.IsNullOrWhiteSpace(_txtMiddlename.Text) ? (object)DBNull.Value : _txtMiddlename.Text.Trim());
                    cmd.Parameters.AddWithValue("@lastName", txtLastName.Text.Trim());
                    cmd.Parameters.AddWithValue("@diagnosis", txtDiagnosis.Text.Trim());
                    cmd.Parameters.AddWithValue("@treatment", txtTreatment.Text.Trim());
                    cmd.Parameters.AddWithValue("@docName", docValue);
                    cmd.Parameters.AddWithValue("@date", dtpRecord.Value.Date);
                    cmd.Parameters.AddWithValue("@gender", cmbGender.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@age", Convert.ToInt32(cmbAge.SelectedItem));
                    cmd.Parameters.AddWithValue("@birthday", dtpBirthday.Value.Date);
                    cmd.Parameters.AddWithValue("@address", string.IsNullOrWhiteSpace(_txtSitio.Text) ? (object)DBNull.Value : _txtSitio.Text.Trim());
                    cmd.Parameters.AddWithValue("@brgyId", brgyValue);

                    cmd.ExecuteNonQuery();
                    BsmartAuditService.Log("Add Health Record", "health_records", cmd.LastInsertedId);

                    MessageBox.Show("Record added successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                    BsmartNotificationService.RefreshOpenIndicators(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding record: " + ex.Message, "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =============================================
        // UPDATE ÃƒÂ¢Ã¢â€šÂ¬Ã¢â‚¬Â includes assigned_doc_name
        // =============================================
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedHealthID == 0)
            { MessageBox.Show("Please search a record first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            if (!ValidateInputs()) return;

            if (MessageBox.Show("Update this record?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    object docValue = string.IsNullOrWhiteSpace(txtAssignDocName.Text)
                        ? (object)DBNull.Value
                        : txtAssignDocName.Text.Trim();

                    MySqlCommand cmd = new MySqlCommand(@"
                        UPDATE health_records SET
                            first_name         = @firstName,
                            middle_name        = @middleName,
                            last_name          = @lastName,
                            Diagnosis          = @diagnosis,
                            Treatment          = @treatment,
                            assigned_doc_name  = @docName,
                            Date               = @date,
                            gender             = @gender,
                            age                = @age,
                            birthday           = @birthday,
                            address            = @address
                        WHERE ID = @id", conn);

                    cmd.Parameters.AddWithValue("@firstName", txtFirstName.Text.Trim());
                    cmd.Parameters.AddWithValue("@middleName", string.IsNullOrWhiteSpace(_txtMiddlename.Text) ? (object)DBNull.Value : _txtMiddlename.Text.Trim());
                    cmd.Parameters.AddWithValue("@lastName", txtLastName.Text.Trim());
                    cmd.Parameters.AddWithValue("@diagnosis", txtDiagnosis.Text.Trim());
                    cmd.Parameters.AddWithValue("@treatment", txtTreatment.Text.Trim());
                    cmd.Parameters.AddWithValue("@docName", docValue);
                    cmd.Parameters.AddWithValue("@date", dtpRecord.Value.Date);
                    cmd.Parameters.AddWithValue("@gender", cmbGender.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@age", Convert.ToInt32(cmbAge.SelectedItem));
                    cmd.Parameters.AddWithValue("@birthday", dtpBirthday.Value.Date);
                    cmd.Parameters.AddWithValue("@address", string.IsNullOrWhiteSpace(_txtSitio.Text) ? (object)DBNull.Value : _txtSitio.Text.Trim());
                    cmd.Parameters.AddWithValue("@id", selectedHealthID);

                    cmd.ExecuteNonQuery();
                    BsmartAuditService.Log("Update Health Record", "health_records", selectedHealthID);

                    MessageBox.Show("Record updated successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating: " + ex.Message, "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =============================================
        // ARCHIVE
        // =============================================
        private void btnArchive_Click(object sender, EventArgs e)
        {
            if (selectedHealthID == 0)
            { MessageBox.Show("Please search a record first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            if (MessageBox.Show("Archive this record?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "UPDATE health_records SET is_archived = 1 WHERE ID = @id", conn);
                    cmd.Parameters.AddWithValue("@id", selectedHealthID);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Record archived successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error archiving: " + ex.Message, "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =============================================
        // DELETE
        // =============================================
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedHealthID == 0)
            { MessageBox.Show("Please search a record first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            if (MessageBox.Show("PERMANENTLY delete this record? This cannot be undone!", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "DELETE FROM health_records WHERE ID = @id", conn);
                    cmd.Parameters.AddWithValue("@id", selectedHealthID);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Record permanently deleted!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting: " + ex.Message, "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =============================================
        // NAVIGATION
        // =============================================
        private void btnNotification_Click(object sender, EventArgs e)
        { BsmartNotificationService.Show(this, btnNotification); }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Logout?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Session.Clear();
                BsmartFormNavigator.Open(this, new loginLGU());
            }
        }

        private void btnViewRecord_Click(object sender, EventArgs e)
        {
            BsmartFormNavigator.Open(this, new TayudViewHealth());
        }

        private void btnAdminDash_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudLGU()); }

        private void btnManageRecord_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudManageHealth()); }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudGenerateReport()); }

        private void btnViewArchive_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudViewArchive()); }

        private void btnViewApp_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudViewAppointments()); }

        private void btnInventory_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new TayudInventory()); }

        private void txtFirstName_TextChanged(object sender, EventArgs e) { }
        private void txtMiddleName_TextChanged(object sender, EventArgs e) { }
        private void _txtMiddlename_TextChanged(object sender, EventArgs e) { }
        private void txtLastName_TextChanged(object sender, EventArgs e) { }
        private void cmbAge_SelectedIndexChanged(object sender, EventArgs e) { }
        private void cmbGender_SelectedIndexChanged(object sender, EventArgs e) { }
        private void dtpBirthday_ValueChanged(object sender, EventArgs e) { }
        private void dtpRecord_ValueChanged(object sender, EventArgs e) { }
        private void txtDiagnosis_TextChanged(object sender, EventArgs e) { }
        private void txtTreatment_TextChanged(object sender, EventArgs e) { }
        private void txtAssignDocName_TextChanged(object sender, EventArgs e) { }
        private void txtSitioAddress_TextChanged(object sender, EventArgs e) { }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            SettingsNavigationService.OpenSettings(this);
        }
    }
}
