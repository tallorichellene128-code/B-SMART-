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
    public partial class MayorViewInventory : Form
    {
        public MayorViewInventory()
        {
            InitializeComponent();
            Load += MayorViewInventory_Load;
            cmbBarangay.SelectedIndexChanged += (s, e) => LoadInventory();
            txtSearch.TextChanged += (s, e) => LoadInventory();
            btnDownloadResHealthRec.Click += (s, e) =>
                MayorFormHelper.ExportGridToPdf(dgvMayorViewInventory, "Mayor Inventory Records");
            MayorFormHelper.AttachTopBar(btnNotification, btnSettings, btnLogout, this);
        }

        private void MayorViewInventory_Load(object? sender, EventArgs e)
        {
            label2.Text = "Inventory Records";
            MayorFormHelper.ConfigureMayorCombo(cmbBarangay);
            LoadInventory();
        }

        private void LoadInventory()
        {
            try
            {
                dgvMayorViewInventory.DataSource = MayorFormHelper.LoadInventory(
                    cmbBarangay.Text, txtSearch.Text.Trim());
                MayorFormHelper.StyleGrid(dgvMayorViewInventory);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading inventory: " + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnViewHealthReport_Click(object sender, EventArgs e)
        {
            BsmartFormNavigator.Open(this, new MayorViewHealthRep());
        }

        private void btnMayorDash_Click(object sender, EventArgs e)
        {
            BsmartFormNavigator.Open(this, new MayorDash());
        }

        private void btnResRecByBar_Click(object sender, EventArgs e)
        {
            BsmartFormNavigator.Open(this, new MayorResRec());
        }

        private void btnResHealthRec_Click(object sender, EventArgs e)
        {
            BsmartFormNavigator.Open(this, new MayorResHealthRec());
        }

        private void btnViewInventory_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            cmbBarangay.SelectedIndex = 0;
            LoadInventory();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
        }
    }
}
