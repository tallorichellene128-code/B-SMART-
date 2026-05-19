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
    public partial class TayudBCViewResHealthRep : Form
    {
        public TayudBCViewResHealthRep()
        {
            InitializeComponent();
            txtSearch.TextChanged += (s, e) => LoadReport();
            btnDownloadResHealthRec.Click += (s, e) =>
                MayorFormHelper.ExportGridToPdf(dgvMayorViewHealthRep, $"{Session.BarangayName} Barangay Captain Health Report");
        }

        private void TayudBCViewResHealthRep_Load(object sender, EventArgs e)
        {
            ConfigureReportLayout();
            cmbHealthRepDate.DropDownStyle = ComboBoxStyle.DropDownList;
            if (cmbHealthRepDate.Items.Count == 0)
                cmbHealthRepDate.Items.AddRange(new object[] { "Weekly", "Monthly", "Yearly" });
            cmbHealthRepDate.SelectedItem = "Yearly";
            LoadReport();
        }

        private void ConfigureReportLayout()
        {
            int top = 100;
            int cardWidth = 235;
            int cardHeight = 150;
            int gap = 14;

            PTotalCases.Location = new Point(3, top);
            PTotalCases.Size = new Size(cardWidth, cardHeight);

            PTotalRecovered.Location = new Point(PTotalCases.Right + gap, top);
            PTotalRecovered.Size = new Size(cardWidth, cardHeight);

            PActiveCases.Location = new Point(PTotalRecovered.Right + gap, top);
            PActiveCases.Size = new Size(cardWidth, cardHeight);

            picPieChartMostCommon.Location = new Point(PActiveCases.Right + gap, 86);
            picPieChartMostCommon.Size = new Size(panel3.Width - picPieChartMostCommon.Left - 6, 170);
            picPieChartMostCommon.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void LoadReport()
        {
            try
            {
                string period = cmbHealthRepDate.SelectedItem?.ToString() ?? "Yearly";
                dgvMayorViewHealthRep.DataSource = MayorFormHelper.LoadHealthRecords(
                    Session.BarangayName, txtSearch.Text.Trim(), period);
                MayorFormHelper.StyleGrid(dgvMayorViewHealthRep);
                MayorFormHelper.RenderHealthSummary(
                    PTotalCases, PTotalRecovered, PActiveCases, picPieChartMostCommon,
                    Session.BarangayName, period);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading health report: " + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBCapDash_Click(object sender, EventArgs e)
        {
            BsmartFormNavigator.Open(this, new TayudBCapDash());
        }

        private void btnResManageRecord_Click(object sender, EventArgs e)
        {
            BsmartFormNavigator.Open(this, new TayudBCManageRes());
        }

        private void btnResViewRecord_Click(object sender, EventArgs e)
        {
            BsmartFormNavigator.Open(this, new TayudBCViewRes());
        }

        private void btnManageResAcc_Click(object sender, EventArgs e)
        {
            BsmartFormNavigator.Open(this, new TayudBCapManageResAcc());
        }

        private void btnViewResAcc_Click(object sender, EventArgs e)
        {
            BsmartFormNavigator.Open(this, new TayudBCapViewResAcc());
        }

        private void btnManageFrozenAcc_Click(object sender, EventArgs e)
        {
            BsmartFormNavigator.Open(this, new TayudBCapManageFAcc());
        }

        private void btnViewHealthRep_Click(object sender, EventArgs e)
        {
            BsmartFormNavigator.Open(this, new TayudBCViewResHealthRep());
        }

        private void btnViewArchive_Click(object sender, EventArgs e)
        {
            BsmartFormNavigator.Open(this, new TayudBCapViewArch());
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            SettingsNavigationService.OpenLogin(this);
        }

        private void picPieChartMostCommon_Click(object sender, EventArgs e)
        {

        }

        private void cmbHealthRepDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadReport();
        }
    }
}
