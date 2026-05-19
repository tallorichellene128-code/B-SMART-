namespace BSMART
{
    partial class TayudBCViewResHealthRep
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TayudBCViewResHealthRep));
            panel1 = new Panel();
            label1 = new Label();
            btnSettings = new Button();
            btnLogout = new Button();
            pictureBox4 = new PictureBox();
            btnBCapDash = new Button();
            panel2 = new Panel();
            btnViewHealthRep = new Button();
            btnManageFrozenAcc = new Button();
            btnResManageRecord = new Button();
            btnViewResAcc = new Button();
            btnViewArchive = new Button();
            btnManageResAcc = new Button();
            btnResViewRecord = new Button();
            panel3 = new Panel();
            cmbHealthRepDate = new ComboBox();
            btnDownloadResHealthRec = new Button();
            picPieChartMostCommon = new PictureBox();
            PActiveCases = new Panel();
            PTotalRecovered = new Panel();
            PTotalCases = new Panel();
            txtSearch = new TextBox();
            dgvMayorViewHealthRep = new DataGridView();
            label2 = new Label();
            btnNotification = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picPieChartMostCommon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvMayorViewHealthRep).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.Transparent;
            panel1.BackgroundImage = (Image)resources.GetObject("panel1.BackgroundImage");
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
            panel1.Location = new Point(3, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(260, 223);
            panel1.TabIndex = 20;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.SkyBlue;
            label1.Font = new Font("Stencil", 20F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.Location = new Point(768, 13);
            label1.Name = "label1";
            label1.Size = new Size(187, 47);
            label1.TabIndex = 55;
            label1.Text = "B-SMART";
            // 
            // btnSettings
            // 
            btnSettings.BackColor = Color.SkyBlue;
            btnSettings.BackgroundImage = (Image)resources.GetObject("btnSettings.BackgroundImage");
            btnSettings.BackgroundImageLayout = ImageLayout.Zoom;
            btnSettings.FlatStyle = FlatStyle.Flat;
            btnSettings.Location = new Point(1357, 12);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(68, 48);
            btnSettings.TabIndex = 54;
            btnSettings.UseVisualStyleBackColor = false;
            // 
            // btnLogout
            // 
            btnLogout.BackColor = Color.SkyBlue;
            btnLogout.BackgroundImage = (Image)resources.GetObject("btnLogout.BackgroundImage");
            btnLogout.BackgroundImageLayout = ImageLayout.Zoom;
            btnLogout.FlatStyle = FlatStyle.Popup;
            btnLogout.Location = new Point(1431, 12);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(68, 48);
            btnLogout.TabIndex = 53;
            btnLogout.UseVisualStyleBackColor = false;
            btnLogout.Click += btnLogout_Click;
            // 
            // pictureBox4
            // 
            pictureBox4.BackColor = Color.SkyBlue;
            pictureBox4.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox4.Location = new Point(259, 0);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(1252, 75);
            pictureBox4.TabIndex = 52;
            pictureBox4.TabStop = false;
            // 
            // btnBCapDash
            // 
            btnBCapDash.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnBCapDash.Location = new Point(0, 229);
            btnBCapDash.Name = "btnBCapDash";
            btnBCapDash.Size = new Size(260, 81);
            btnBCapDash.TabIndex = 51;
            btnBCapDash.Text = "Barangay Captain Dashboard\r\n";
            btnBCapDash.UseVisualStyleBackColor = true;
            btnBCapDash.Click += btnBCapDash_Click;
            // 
            // panel2
            // 
            panel2.BackgroundImage = (Image)resources.GetObject("panel2.BackgroundImage");
            panel2.Controls.Add(btnViewHealthRep);
            panel2.Controls.Add(panel1);
            panel2.Controls.Add(btnManageFrozenAcc);
            panel2.Controls.Add(btnResManageRecord);
            panel2.Controls.Add(btnViewResAcc);
            panel2.Controls.Add(btnViewArchive);
            panel2.Controls.Add(btnManageResAcc);
            panel2.Controls.Add(btnResViewRecord);
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(260, 949);
            panel2.TabIndex = 50;
            // 
            // btnViewHealthRep
            // 
            btnViewHealthRep.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnViewHealthRep.Location = new Point(0, 742);
            btnViewHealthRep.Name = "btnViewHealthRep";
            btnViewHealthRep.Size = new Size(260, 74);
            btnViewHealthRep.TabIndex = 63;
            btnViewHealthRep.Text = "View Residents Health Report";
            btnViewHealthRep.UseVisualStyleBackColor = true;
            btnViewHealthRep.Click += btnViewHealthRep_Click;
            // 
            // btnManageFrozenAcc
            // 
            btnManageFrozenAcc.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnManageFrozenAcc.Location = new Point(0, 662);
            btnManageFrozenAcc.Name = "btnManageFrozenAcc";
            btnManageFrozenAcc.Size = new Size(260, 74);
            btnManageFrozenAcc.TabIndex = 62;
            btnManageFrozenAcc.Text = "Manage Frozen Accounts";
            btnManageFrozenAcc.UseVisualStyleBackColor = true;
            btnManageFrozenAcc.Click += btnManageFrozenAcc_Click;
            // 
            // btnResManageRecord
            // 
            btnResManageRecord.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnResManageRecord.Location = new Point(0, 342);
            btnResManageRecord.Name = "btnResManageRecord";
            btnResManageRecord.Size = new Size(260, 74);
            btnResManageRecord.TabIndex = 57;
            btnResManageRecord.Text = "Manage Resident Record";
            btnResManageRecord.UseVisualStyleBackColor = true;
            btnResManageRecord.Click += btnResManageRecord_Click;
            // 
            // btnViewResAcc
            // 
            btnViewResAcc.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnViewResAcc.Location = new Point(0, 582);
            btnViewResAcc.Name = "btnViewResAcc";
            btnViewResAcc.Size = new Size(260, 74);
            btnViewResAcc.TabIndex = 61;
            btnViewResAcc.Text = "View Resident Accounts";
            btnViewResAcc.UseVisualStyleBackColor = true;
            btnViewResAcc.Click += btnViewResAcc_Click;
            // 
            // btnViewArchive
            // 
            btnViewArchive.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnViewArchive.Location = new Point(0, 822);
            btnViewArchive.Name = "btnViewArchive";
            btnViewArchive.Size = new Size(260, 74);
            btnViewArchive.TabIndex = 60;
            btnViewArchive.Text = "View Archive";
            btnViewArchive.UseVisualStyleBackColor = true;
            btnViewArchive.Click += btnViewArchive_Click;
            // 
            // btnManageResAcc
            // 
            btnManageResAcc.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnManageResAcc.Location = new Point(0, 502);
            btnManageResAcc.Name = "btnManageResAcc";
            btnManageResAcc.Size = new Size(260, 74);
            btnManageResAcc.TabIndex = 59;
            btnManageResAcc.Text = "Manage Resident Accounts";
            btnManageResAcc.UseVisualStyleBackColor = true;
            btnManageResAcc.Click += btnManageResAcc_Click;
            // 
            // btnResViewRecord
            // 
            btnResViewRecord.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnResViewRecord.Location = new Point(0, 422);
            btnResViewRecord.Name = "btnResViewRecord";
            btnResViewRecord.Size = new Size(260, 74);
            btnResViewRecord.TabIndex = 58;
            btnResViewRecord.Text = "View Resident Record\r\n";
            btnResViewRecord.UseVisualStyleBackColor = true;
            btnResViewRecord.Click += btnResViewRecord_Click;
            // 
            // panel3
            // 
            panel3.BackColor = Color.Transparent;
            panel3.Controls.Add(cmbHealthRepDate);
            panel3.Controls.Add(btnDownloadResHealthRec);
            panel3.Controls.Add(picPieChartMostCommon);
            panel3.Controls.Add(PActiveCases);
            panel3.Controls.Add(PTotalRecovered);
            panel3.Controls.Add(PTotalCases);
            panel3.Controls.Add(txtSearch);
            panel3.Controls.Add(dgvMayorViewHealthRep);
            panel3.Location = new Point(269, 159);
            panel3.Name = "panel3";
            panel3.Size = new Size(1233, 777);
            panel3.TabIndex = 61;
            // 
            // cmbHealthRepDate
            // 
            cmbHealthRepDate.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbHealthRepDate.FormattingEnabled = true;
            cmbHealthRepDate.Items.AddRange(new object[] { "Weekly", "Monthly", "Yearly" });
            cmbHealthRepDate.Location = new Point(3, 286);
            cmbHealthRepDate.Name = "cmbHealthRepDate";
            cmbHealthRepDate.Size = new Size(310, 40);
            cmbHealthRepDate.TabIndex = 6;
            cmbHealthRepDate.Text = "Health report:";
            cmbHealthRepDate.SelectedIndexChanged += cmbHealthRepDate_SelectedIndexChanged;
            // 
            // btnDownloadResHealthRec
            // 
            btnDownloadResHealthRec.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDownloadResHealthRec.Location = new Point(974, 290);
            btnDownloadResHealthRec.Name = "btnDownloadResHealthRec";
            btnDownloadResHealthRec.Size = new Size(257, 36);
            btnDownloadResHealthRec.TabIndex = 2;
            btnDownloadResHealthRec.Text = "Download Records(PDF)";
            btnDownloadResHealthRec.UseVisualStyleBackColor = true;
            // 
            // picPieChartMostCommon
            // 
            picPieChartMostCommon.Location = new Point(945, 100);
            picPieChartMostCommon.Name = "picPieChartMostCommon";
            picPieChartMostCommon.Size = new Size(281, 150);
            picPieChartMostCommon.TabIndex = 5;
            picPieChartMostCommon.TabStop = false;
            picPieChartMostCommon.Click += picPieChartMostCommon_Click;
            // 
            // PActiveCases
            // 
            PActiveCases.BackColor = Color.LightBlue;
            PActiveCases.Location = new Point(639, 100);
            PActiveCases.Name = "PActiveCases";
            PActiveCases.Size = new Size(300, 150);
            PActiveCases.TabIndex = 4;
            // 
            // PTotalRecovered
            // 
            PTotalRecovered.BackColor = Color.LightBlue;
            PTotalRecovered.Location = new Point(311, 100);
            PTotalRecovered.Name = "PTotalRecovered";
            PTotalRecovered.Size = new Size(300, 150);
            PTotalRecovered.TabIndex = 3;
            // 
            // PTotalCases
            // 
            PTotalCases.BackColor = Color.LightBlue;
            PTotalCases.Location = new Point(3, 100);
            PTotalCases.Name = "PTotalCases";
            PTotalCases.Size = new Size(300, 150);
            PTotalCases.TabIndex = 2;
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSearch.Location = new Point(311, 17);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(612, 45);
            txtSearch.TabIndex = 1;
            // 
            // dgvMayorViewHealthRep
            // 
            dgvMayorViewHealthRep.BackgroundColor = SystemColors.GradientInactiveCaption;
            dgvMayorViewHealthRep.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMayorViewHealthRep.GridColor = SystemColors.HotTrack;
            dgvMayorViewHealthRep.Location = new Point(3, 336);
            dgvMayorViewHealthRep.Name = "dgvMayorViewHealthRep";
            dgvMayorViewHealthRep.RowHeadersWidth = 62;
            dgvMayorViewHealthRep.Size = new Size(1227, 438);
            dgvMayorViewHealthRep.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI", 22F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(625, 96);
            label2.Name = "label2";
            label2.Size = new Size(508, 60);
            label2.TabIndex = 62;
            label2.Text = "Resident Health Report";
            // 
            // btnNotification
            // 
            btnNotification.BackColor = Color.SkyBlue;
            btnNotification.BackgroundImage = Properties.Resources.alert;
            btnNotification.BackgroundImageLayout = ImageLayout.Zoom;
            btnNotification.FlatStyle = FlatStyle.Popup;
            btnNotification.ForeColor = Color.White;
            btnNotification.Location = new Point(1283, 13);
            btnNotification.Name = "btnNotification";
            btnNotification.Size = new Size(68, 48);
            btnNotification.TabIndex = 63;
            btnNotification.UseVisualStyleBackColor = false;
            // 
            // TayudBCViewResHealthRep
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.BCO_677f5122_1ca8_402c_addd_6f3eadad8129;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1511, 948);
            Controls.Add(btnNotification);
            Controls.Add(label2);
            Controls.Add(panel3);
            Controls.Add(label1);
            Controls.Add(btnSettings);
            Controls.Add(btnLogout);
            Controls.Add(pictureBox4);
            Controls.Add(btnBCapDash);
            Controls.Add(panel2);
            Name = "TayudBCViewResHealthRep";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "TayudBCViewResHealthRep";
            Load += TayudBCViewResHealthRep_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picPieChartMostCommon).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvMayorViewHealthRep).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Panel panel1;
        private Label label1;
        private Button btnSettings;
        private Button btnLogout;
        private PictureBox pictureBox4;
        private Button btnBCapDash;
        private Panel panel2;
        private Button btnViewHealthRep;
        private Button btnManageFrozenAcc;
        private Button btnResManageRecord;
        private Button btnViewResAcc;
        private Button btnViewArchive;
        private Button btnManageResAcc;
        private Button btnResViewRecord;
        private Panel panel3;
        private Button btnDownloadResHealthRec;
        private PictureBox picPieChartMostCommon;
        private Panel PActiveCases;
        private Panel PTotalRecovered;
        private Panel PTotalCases;
        private TextBox txtSearch;
        private DataGridView dgvMayorViewHealthRep;
        private ComboBox cmbHealthRepDate;
        private Label label2;
        private Button btnNotification;
    }
}