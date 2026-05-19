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
    public partial class MayorResHealthRec : Form
    {
        public MayorResHealthRec()
        {
            InitializeComponent();
            Load += MayorResHealthRec_Load;
            cmbBarangay.SelectedIndexChanged += (s, e) => LoadRecords();
            txtSearch.TextChanged += (s, e) => LoadRecords();
            btnDownloadResHealthRec.Click += (s, e) =>
                MayorFormHelper.ExportGridToPdf(dgvMayorResHealthRec, "Mayor Resident Health Records");
            MayorFormHelper.AttachTopBar(btnNotification, btnSettings, btnLogout, this);
            MayorFormHelper.HideMayorArchiveButton(this);
        }

        private void MayorResHealthRec_Load(object? sender, EventArgs e)
        {
            MayorFormHelper.ConfigureMayorCombo(cmbBarangay);
            LoadRecords();
        }

        private void LoadRecords()
        {
            try
            {
                dgvMayorResHealthRec.DataSource = MayorFormHelper.LoadHealthRecords(
                    cmbBarangay.Text, txtSearch.Text.Trim());
                MayorFormHelper.StyleGrid(dgvMayorResHealthRec);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading health records: " + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnResRecByBar_Click(object sender, EventArgs e)
        {
            BsmartFormNavigator.Open(this, new MayorResRec());
        }

        private void btnResHealthRec_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            cmbBarangay.SelectedIndex = 0;
            LoadRecords();
        }

        private void btnViewHealthReport_Click(object sender, EventArgs e)
        {
            BsmartFormNavigator.Open(this, new MayorViewHealthRep());
        }

        private void btnViewInventory_Click(object sender, EventArgs e)
        {
            BsmartFormNavigator.Open(this, new MayorViewInventory());
        }

        private void btnMayorDash_Click(object sender, EventArgs e)
        {
            BsmartFormNavigator.Open(this, new MayorDash());
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
