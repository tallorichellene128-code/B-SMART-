using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BSMART
{
    public partial class Register2 : Form
    {
        private string connectionString = "Server=localhost;Database=bsmart_db;Uid=root;Pwd=;";

        public Register2()
        {
            InitializeComponent();
        }

        private void Register2_Load(object sender, EventArgs e)
        {
            // Load barangays from DB
            cmbBarangay.Items.Clear();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(
                        "SELECT id, name FROM barangays ORDER BY name ASC", conn);
                    MySqlDataReader r = cmd.ExecuteReader();
                    while (r.Read())
                        cmbBarangay.Items.Add(new BarangayItem(
                            Convert.ToInt32(r["id"]), r["name"].ToString()));
                }
            }
            catch { }

            // Restore if coming back from Register3
            if (!string.IsNullOrEmpty(RegisterData.Address))
            {
                txtAddress.Text = RegisterData.Address;
                txtEmail.Text = RegisterData.Email;
                txtMobileNum.Text = RegisterData.MobileNumber;
                // Re-select barangay
                foreach (BarangayItem item in cmbBarangay.Items)
                    if (item.ID == RegisterData.BarangayID)
                    { cmbBarangay.SelectedItem = item; break; }
            }
        }

        private bool ValidatePage2()
        {
            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            { MessageBox.Show("Please enter Address.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtAddress.Focus(); return false; }
            if (cmbBarangay.SelectedIndex == -1)
            { MessageBox.Show("Please select Barangay.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); cmbBarangay.Focus(); return false; }
            if (!string.IsNullOrWhiteSpace(txtEmail.Text) && !txtEmail.Text.Contains("@"))
            { MessageBox.Show("Please enter a valid email.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtEmail.Focus(); return false; }
            return true;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (!ValidatePage2()) return;

            var selected = (BarangayItem)cmbBarangay.SelectedItem;
            RegisterData.Address = txtAddress.Text.Trim();
            RegisterData.Email = txtEmail.Text.Trim();
            RegisterData.MobileNumber = txtMobileNum.Text.Trim();
            RegisterData.BarangayID = selected.ID;
            RegisterData.BarangayName = selected.Name;

            BsmartFormNavigator.Open(this, new Register3());
        }

        private void btnPrev_Click(object sender, EventArgs e)
        { BsmartFormNavigator.Open(this, new Register()); }

        private void cmbBarangay_SelectedIndexChanged(object sender, EventArgs e) { }
        private void txtAddress_TextChanged(object sender, EventArgs e) { }
        private void txtEmail_TextChanged(object sender, EventArgs e) { }
        private void txtMobileNum_TextChanged(object sender, EventArgs e) { }
    }

    // Helper class for barangay combobox
    public class BarangayItem
    {
        public int ID { get; }
        public string Name { get; }
        public BarangayItem(int id, string name) { ID = id; Name = name; }
        public override string ToString() => Name;
    }
}