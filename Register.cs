using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BSMART
{
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }

        private void Register_Load(object sender, EventArgs e)
        {
            cmbAge.Items.Clear();
            for (int i = 1; i <= 120; i++) cmbAge.Items.Add(i.ToString());

            cmbGender.Items.Clear();
            cmbGender.Items.Add("Male");
            cmbGender.Items.Add("Female");

            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            if (comboBox1.Items.Count == 0)
            {
                comboBox1.Items.Add("Single");
                comboBox1.Items.Add("Married");
                comboBox1.Items.Add("Widowed");
                comboBox1.Items.Add("Divorced");
                comboBox1.Items.Add("Legally separated");
            }

            dtpBirthday.Value = DateTime.Now.AddYears(-18);

            // Auto-compute age when birthday changes
            dtpBirthday.ValueChanged += (s, ev) =>
            {
                int age = DateTime.Now.Year - dtpBirthday.Value.Year;
                if (dtpBirthday.Value.Date > DateTime.Now.AddYears(-age)) age--;
                if (age >= 1 && age <= 120)
                    cmbAge.SelectedItem = age.ToString();
            };

            // Restore data if coming back from Register2
            if (!string.IsNullOrEmpty(RegisterData.FirstName))
            {
                txtFirstName.Text = RegisterData.FirstName;
                txtMiddleName.Text = RegisterData.MiddleName;
                txtLastName.Text = RegisterData.LastName;
                try { dtpBirthday.Value = DateTime.Parse(RegisterData.Birthday); } catch { }
                cmbAge.SelectedItem = RegisterData.Age.ToString();
                cmbGender.SelectedItem = RegisterData.Gender;
                txtPlaceofbirth.Text = RegisterData.PlaceOfBirth;
                comboBox1.SelectedItem = RegisterData.CivilStatus;
                txtReligion.Text = RegisterData.Religion;
                txtCitizenship.Text = RegisterData.Citizenship;
            }
        }

        private bool ValidatePage1()
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            { MessageBox.Show("Please enter First Name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtFirstName.Focus(); return false; }
            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            { MessageBox.Show("Please enter Last Name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtLastName.Focus(); return false; }
            if (cmbAge.SelectedIndex == -1)
            { MessageBox.Show("Please select Age.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); cmbAge.Focus(); return false; }
            if (cmbGender.SelectedIndex == -1)
            { MessageBox.Show("Please select Gender.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); cmbGender.Focus(); return false; }
            if (comboBox1.SelectedIndex == -1)
            { MessageBox.Show("Please select Civil Status.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); comboBox1.Focus(); return false; }
            return true;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (!ValidatePage1()) return;

            RegisterData.FirstName = txtFirstName.Text.Trim();
            RegisterData.MiddleName = txtMiddleName.Text.Trim();
            RegisterData.LastName = txtLastName.Text.Trim();
            RegisterData.Birthday = dtpBirthday.Value.ToString("yyyy-MM-dd");
            RegisterData.Age = Convert.ToInt32(cmbAge.SelectedItem);
            RegisterData.Gender = cmbGender.SelectedItem.ToString();
            RegisterData.PlaceOfBirth = txtPlaceofbirth.Text.Trim();
            RegisterData.CivilStatus = comboBox1.SelectedItem?.ToString() ?? "";
            RegisterData.Religion = txtReligion.Text.Trim();
            RegisterData.Citizenship = txtCitizenship.Text.Trim();

            BsmartFormNavigator.Open(this, new Register2());
        }

        private void btnBack_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new loginRes()); }

        private void txtFirstName_TextChanged(object sender, EventArgs e) { }
        private void txtLastName_TextChanged(object sender, EventArgs e) { }
        private void txtMiddleName_TextChanged(object sender, EventArgs e) { }
        private void dtpBirthday_ValueChanged(object sender, EventArgs e) { }
        private void cmbAge_SelectedIndexChanged(object sender, EventArgs e) { }
        private void cmbGender_SelectedIndexChanged(object sender, EventArgs e) { }
        private void txtPlaceofbirth_TextChanged(object sender, EventArgs e) { }
        private void txtReligion_TextChanged(object sender, EventArgs e) { }
        private void txtCitizenship_TextChanged(object sender, EventArgs e) { }
    }
}
