using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BSMART
{
    public partial class MayorResRec : Form
    {
        public MayorResRec()
        {
            InitializeComponent();
            Load += MayorResRec_Load;
            cmbBarangay.SelectedIndexChanged += (s, e) => LoadRecords();
            txtSearch.TextChanged += (s, e) => LoadRecords();
            btnDownloadResRec.Click += (s, e) =>
                MayorFormHelper.ExportGridToPdf(dgvMayorResRec, "Mayor Resident Records");
            MayorFormHelper.AttachTopBar(btnNotification, btnSettings, btnLogout, this);
            MayorFormHelper.HideMayorArchiveButton(this);
        }

        private void MayorResRec_Load(object? sender, EventArgs e)
        {
            MayorFormHelper.ConfigureMayorCombo(cmbBarangay);
            LoadRecords();
        }

        private void LoadRecords()
        {
            try
            {
                dgvMayorResRec.DataSource = MayorFormHelper.LoadResidentAccounts(
                    cmbBarangay.Text, txtSearch.Text.Trim());
                MayorFormHelper.StyleGrid(dgvMayorResRec);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading resident records: " + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMayorDash_Click(object sender, EventArgs e)
        {
            BsmartFormNavigator.Open(this, new MayorDash());
        }

        private void btnResRecByBar_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            cmbBarangay.SelectedIndex = 0;
            LoadRecords();
        }

        private void btnResHealthRec_Click(object sender, EventArgs e)
        {
            BsmartFormNavigator.Open(this, new MayorResHealthRec());
        }

        private void btnViewHealthReport_Click(object sender, EventArgs e)
        {
            BsmartFormNavigator.Open(this, new MayorViewHealthRep());
        }

        private void btnViewInventory_Click(object sender, EventArgs e)
        {
            BsmartFormNavigator.Open(this, new MayorViewInventory());
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            
        }

        private void MayorResRec_Load_1(object sender, EventArgs e)
        {

        }
    }
}
