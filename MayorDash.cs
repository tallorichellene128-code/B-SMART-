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
    public partial class MayorDash : Form
    {
        public MayorDash()
        {
            InitializeComponent();
            Load += MayorDash_Load;
            MayorFormHelper.AttachTopBar(btnNotification, btnSettings, btnLogout, this);
            MayorFormHelper.HideMayorArchiveButton(this);
        }

        private void MayorDash_Load(object? sender, EventArgs e)
        {
            try
            {
                lblTotalResidents.Text = MayorFormHelper.CountResidents().ToString();
                lblTotalRecords.Text = MayorFormHelper.CountHealthRecords().ToString();
                lblMostCommon.Text = MayorFormHelper.MostCommonDiagnosis();

                dgvRecentHealth.DataSource = MayorFormHelper.LoadRecentHealthRecords();
                MayorFormHelper.StyleGrid(dgvRecentHealth);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading Mayor dashboard: " + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnResRecByBar_Click(object sender, EventArgs e)
        {
            BsmartFormNavigator.Open(this, new MayorResRec());
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

        private void btnMayorDash_Click(object sender, EventArgs e)
        {
            MayorDash_Load(sender, e);
        }

        private void MayorDash_Load_1(object sender, EventArgs e)
        {

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {

        }

        private void btnViewArchive_Click(object sender, EventArgs e)
        {

        }
    }
}
