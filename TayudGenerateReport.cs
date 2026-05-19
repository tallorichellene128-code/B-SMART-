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
    public partial class TayudGenerateReport : Form
    {
        public TayudGenerateReport()
        {
            InitializeComponent();
            btnLogout.Click += (s, e) => SettingsNavigationService.OpenLogin(this);
            Load += TayudGenerateReport_Load;
            txtSearch.TextChanged += (s, e) => LoadReport();
            btnDownloadResHealthRec.Click += (s, e) =>
                MayorFormHelper.ExportGridToPdf(dgvMayorViewHealthRep, $"{Session.BarangayName} Health Report");
        }

        private void TayudGenerateReport_Load(object? sender, EventArgs e)
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

        private void btnViewApp_Click(object sender, EventArgs e)
        {
            BsmartFormNavigator.Open(this, new TayudViewAppointments());
        }

        private void btnAdminDash_Click(object sender, EventArgs e)
        {
            BsmartFormNavigator.Open(this, new TayudLGU());
        }

        private void btnManageRecord_Click(object sender, EventArgs e)
        {
            BsmartFormNavigator.Open(this, new TayudManageHealth());
        }

        private void btnViewRecord_Click(object sender, EventArgs e)
        {
            BsmartFormNavigator.Open(this, new TayudViewHealth());
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            BsmartFormNavigator.Open(this, new TayudGenerateReport());
        }

        private void btnInventory_Click(object sender, EventArgs e)
        {
            BsmartFormNavigator.Open(this, new TayudInventory());
        }

        private void btnViewArchive_Click(object sender, EventArgs e)
        {
            BsmartFormNavigator.Open(this, new TayudViewArchive());
        }

        private void cmbHealthRepDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadReport();
        }

        private void TayudGenerateReport_Load_1(object sender, EventArgs e)
        {

        }
    }
}
