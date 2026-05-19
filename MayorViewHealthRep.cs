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
    public partial class MayorViewHealthRep : Form
    {
        public MayorViewHealthRep()
        {
            InitializeComponent();
            Load += MayorViewHealthRep_Load;
            cmbBarangay.SelectedIndexChanged += (s, e) => LoadReport();
            txtSearch.TextChanged += (s, e) => LoadReport();
            cmbHealthRepDate.SelectedIndexChanged += (s, e) => LoadReport();
            btnDownloadResHealthRec.Click += (s, e) =>
                MayorFormHelper.ExportGridToPdf(dgvMayorViewHealthRep, "Mayor Barangay Health Report");
            MayorFormHelper.AttachTopBar(btnNotification, btnSettings, btnLogout, this);
        }

        private void MayorViewHealthRep_Load(object? sender, EventArgs e)
        {
            MayorFormHelper.ConfigureMayorCombo(cmbBarangay);
            cmbHealthRepDate.DropDownStyle = ComboBoxStyle.DropDownList;
            if (cmbHealthRepDate.Items.Count == 0)
                cmbHealthRepDate.Items.AddRange(new object[] { "Weekly", "Monthly", "Yearly" });
            cmbHealthRepDate.SelectedItem = "Yearly";
            ConfigureHealthReportLayout();
            LoadReport();
        }

        private void ConfigureHealthReportLayout()
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
                dgvMayorViewHealthRep.DataSource = MayorFormHelper.LoadHealthRecords(
                    cmbBarangay.Text, txtSearch.Text.Trim(), cmbHealthRepDate.SelectedItem?.ToString() ?? "Yearly");
                MayorFormHelper.StyleGrid(dgvMayorViewHealthRep);
                MayorFormHelper.RenderHealthSummary(PTotalCases, PTotalRecovered, PActiveCases, picPieChartMostCommon,
                    cmbBarangay.Text, cmbHealthRepDate.SelectedItem?.ToString() ?? "Yearly");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading health report: " + ex.Message,
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void btnViewHealthReport_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            cmbBarangay.SelectedIndex = 0;
            LoadReport();
        }

        private void btnViewInventory_Click(object sender, EventArgs e)
        {
            BsmartFormNavigator.Open(this, new MayorViewInventory());
        }

        private void picPieChartMostCommon_Click(object sender, EventArgs e)
        {

        }

        private void btnLogout_Click(object sender, EventArgs e)
        {

        }

        private void cmbHealthRepDate_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void MayorViewHealthRep_Load_1(object sender, EventArgs e)
        {

        }
    }
}
