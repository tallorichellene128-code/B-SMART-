namespace BSMART
{
    partial class TayudLGU
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TayudLGU));
            btnInventory = new Button();
            panel1 = new Panel();
            btnManageRecord = new Button();
            btnViewRecord = new Button();
            btnGenerateReport = new Button();
            btnViewArchive = new Button();
            label2 = new Label();
            dgvRecentHealth = new DataGridView();
            label1 = new Label();
            btnSettings = new Button();
            btnLogout = new Button();
            pictureBox4 = new PictureBox();
            btnAdminDash = new Button();
            panel2 = new Panel();
            btnViewApp = new Button();
            PMostCommon = new Panel();
            lblMostCommon = new Label();
            label6 = new Label();
            pictureBox3 = new PictureBox();
            PTotalRecords = new Panel();
            lblTotalRecords = new Label();
            label5 = new Label();
            pictureBox2 = new PictureBox();
            PTotalResidents = new Panel();
            lblTotalResidents = new Label();
            label3 = new Label();
            pictureBox1 = new PictureBox();
            btnNotification = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvRecentHealth).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            panel2.SuspendLayout();
            PMostCommon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            PTotalRecords.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            PTotalResidents.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // btnInventory
            // 
            btnInventory.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnInventory.Location = new Point(0, 636);
            btnInventory.Name = "btnInventory";
            btnInventory.Size = new Size(260, 74);
            btnInventory.TabIndex = 21;
            btnInventory.Text = "Inventory";
            btnInventory.UseVisualStyleBackColor = true;
            btnInventory.Click += btnInventory_Click;
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
            // btnManageRecord
            // 
            btnManageRecord.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnManageRecord.Location = new Point(0, 396);
            btnManageRecord.Name = "btnManageRecord";
            btnManageRecord.Size = new Size(260, 74);
            btnManageRecord.TabIndex = 4;
            btnManageRecord.Text = "Manage Health Record";
            btnManageRecord.UseVisualStyleBackColor = true;
            btnManageRecord.Click += btnManageRecord_Click;
            // 
            // btnViewRecord
            // 
            btnViewRecord.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnViewRecord.Location = new Point(0, 476);
            btnViewRecord.Name = "btnViewRecord";
            btnViewRecord.Size = new Size(260, 74);
            btnViewRecord.TabIndex = 7;
            btnViewRecord.Text = "View Records";
            btnViewRecord.UseVisualStyleBackColor = true;
            btnViewRecord.Click += btnViewRecord_Click;
            // 
            // btnGenerateReport
            // 
            btnGenerateReport.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnGenerateReport.Location = new Point(0, 556);
            btnGenerateReport.Name = "btnGenerateReport";
            btnGenerateReport.Size = new Size(260, 74);
            btnGenerateReport.TabIndex = 8;
            btnGenerateReport.Text = "Generate Report";
            btnGenerateReport.UseVisualStyleBackColor = true;
            btnGenerateReport.Click += btnGenerateReport_Click;
            // 
            // btnViewArchive
            // 
            btnViewArchive.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnViewArchive.Location = new Point(0, 716);
            btnViewArchive.Name = "btnViewArchive";
            btnViewArchive.Size = new Size(260, 74);
            btnViewArchive.TabIndex = 9;
            btnViewArchive.Text = "View Archive";
            btnViewArchive.UseVisualStyleBackColor = true;
            btnViewArchive.Click += btnViewArchive_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(319, 422);
            label2.Name = "label2";
            label2.Size = new Size(364, 48);
            label2.TabIndex = 41;
            label2.Text = "Recent Health Record";
            // 
            // dgvRecentHealth
            // 
            dgvRecentHealth.BackgroundColor = SystemColors.GradientActiveCaption;
            dgvRecentHealth.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRecentHealth.Location = new Point(319, 481);
            dgvRecentHealth.Name = "dgvRecentHealth";
            dgvRecentHealth.ReadOnly = true;
            dgvRecentHealth.RowHeadersWidth = 62;
            dgvRecentHealth.Size = new Size(1106, 398);
            dgvRecentHealth.TabIndex = 40;
            dgvRecentHealth.CellContentClick += dgvRecentHealth_CellContentClick;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.SkyBlue;
            label1.Font = new Font("Stencil", 20F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.Location = new Point(768, 13);
            label1.Name = "label1";
            label1.Size = new Size(187, 47);
            label1.TabIndex = 36;
            label1.Text = "B-SMART";
            // 
            // btnSettings
            // 
            btnSettings.BackColor = Color.SkyBlue;
            btnSettings.BackgroundImage = (Image)resources.GetObject("btnSettings.BackgroundImage");
            btnSettings.BackgroundImageLayout = ImageLayout.Zoom;
            btnSettings.FlatStyle = FlatStyle.Popup;
            btnSettings.Location = new Point(1357, 12);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(68, 48);
            btnSettings.TabIndex = 35;
            btnSettings.UseVisualStyleBackColor = false;
            btnSettings.Click += btnSettings_Click;
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
            btnLogout.TabIndex = 34;
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
            pictureBox4.TabIndex = 33;
            pictureBox4.TabStop = false;
            // 
            // btnAdminDash
            // 
            btnAdminDash.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnAdminDash.Location = new Point(0, 229);
            btnAdminDash.Name = "btnAdminDash";
            btnAdminDash.Size = new Size(260, 81);
            btnAdminDash.TabIndex = 32;
            btnAdminDash.Text = "Admin Dashboard\r\n";
            btnAdminDash.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            panel2.BackgroundImage = (Image)resources.GetObject("panel2.BackgroundImage");
            panel2.Controls.Add(btnViewApp);
            panel2.Controls.Add(btnInventory);
            panel2.Controls.Add(panel1);
            panel2.Controls.Add(btnManageRecord);
            panel2.Controls.Add(btnViewRecord);
            panel2.Controls.Add(btnGenerateReport);
            panel2.Controls.Add(btnViewArchive);
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(260, 949);
            panel2.TabIndex = 31;
            // 
            // btnViewApp
            // 
            btnViewApp.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnViewApp.Location = new Point(0, 796);
            btnViewApp.Name = "btnViewApp";
            btnViewApp.Size = new Size(260, 74);
            btnViewApp.TabIndex = 46;
            btnViewApp.Text = "View Resident Appointments";
            btnViewApp.UseVisualStyleBackColor = true;
            btnViewApp.Click += btnViewApp_Click;
            // 
            // PMostCommon
            // 
            PMostCommon.BackgroundImage = (Image)resources.GetObject("PMostCommon.BackgroundImage");
            PMostCommon.BackgroundImageLayout = ImageLayout.Stretch;
            PMostCommon.Controls.Add(lblMostCommon);
            PMostCommon.Controls.Add(label6);
            PMostCommon.Controls.Add(pictureBox3);
            PMostCommon.Location = new Point(1139, 148);
            PMostCommon.Name = "PMostCommon";
            PMostCommon.Size = new Size(286, 162);
            PMostCommon.TabIndex = 44;
            PMostCommon.Paint += PMostCommon_Paint;
            // 
            // lblMostCommon
            // 
            lblMostCommon.AutoSize = true;
            lblMostCommon.BackColor = Color.Transparent;
            lblMostCommon.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblMostCommon.Location = new Point(116, 81);
            lblMostCommon.Name = "lblMostCommon";
            lblMostCommon.Size = new Size(20, 32);
            lblMostCommon.TabIndex = 25;
            lblMostCommon.Text = ".";
            lblMostCommon.Click += lblMostCommon_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.Transparent;
            label6.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(105, 19);
            label6.Name = "label6";
            label6.Size = new Size(175, 32);
            label6.TabIndex = 27;
            label6.Text = "Most Common";
            // 
            // pictureBox3
            // 
            pictureBox3.BackColor = Color.Transparent;
            pictureBox3.BackgroundImage = (Image)resources.GetObject("pictureBox3.BackgroundImage");
            pictureBox3.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox3.Location = new Point(0, 39);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(99, 75);
            pictureBox3.TabIndex = 23;
            pictureBox3.TabStop = false;
            // 
            // PTotalRecords
            // 
            PTotalRecords.BackColor = Color.Transparent;
            PTotalRecords.BackgroundImage = (Image)resources.GetObject("PTotalRecords.BackgroundImage");
            PTotalRecords.BackgroundImageLayout = ImageLayout.Stretch;
            PTotalRecords.Controls.Add(lblTotalRecords);
            PTotalRecords.Controls.Add(label5);
            PTotalRecords.Controls.Add(pictureBox2);
            PTotalRecords.Location = new Point(721, 148);
            PTotalRecords.Name = "PTotalRecords";
            PTotalRecords.Size = new Size(286, 162);
            PTotalRecords.TabIndex = 43;
            PTotalRecords.Paint += PTotalRecords_Paint;
            // 
            // lblTotalRecords
            // 
            lblTotalRecords.AutoSize = true;
            lblTotalRecords.BackColor = Color.Transparent;
            lblTotalRecords.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblTotalRecords.Location = new Point(124, 81);
            lblTotalRecords.Name = "lblTotalRecords";
            lblTotalRecords.Size = new Size(20, 32);
            lblTotalRecords.TabIndex = 24;
            lblTotalRecords.Text = ".";
            lblTotalRecords.Click += lblTotalRecords_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(105, 19);
            label5.Name = "label5";
            label5.Size = new Size(159, 32);
            label5.TabIndex = 24;
            label5.Text = "Total Records";
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.Transparent;
            pictureBox2.BackgroundImage = (Image)resources.GetObject("pictureBox2.BackgroundImage");
            pictureBox2.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox2.Location = new Point(0, 39);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(99, 75);
            pictureBox2.TabIndex = 22;
            pictureBox2.TabStop = false;
            // 
            // PTotalResidents
            // 
            PTotalResidents.BackgroundImage = (Image)resources.GetObject("PTotalResidents.BackgroundImage");
            PTotalResidents.BackgroundImageLayout = ImageLayout.Stretch;
            PTotalResidents.Controls.Add(lblTotalResidents);
            PTotalResidents.Controls.Add(label3);
            PTotalResidents.Controls.Add(pictureBox1);
            PTotalResidents.Location = new Point(319, 148);
            PTotalResidents.Name = "PTotalResidents";
            PTotalResidents.Size = new Size(286, 162);
            PTotalResidents.TabIndex = 42;
            PTotalResidents.Paint += PTotalResidents_Paint;
            // 
            // lblTotalResidents
            // 
            lblTotalResidents.AutoSize = true;
            lblTotalResidents.BackColor = Color.Transparent;
            lblTotalResidents.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblTotalResidents.Location = new Point(135, 81);
            lblTotalResidents.Name = "lblTotalResidents";
            lblTotalResidents.Size = new Size(20, 32);
            lblTotalResidents.TabIndex = 23;
            lblTotalResidents.Text = ".";
            lblTotalResidents.Click += lblTotalResidents_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(105, 19);
            label3.Name = "label3";
            label3.Size = new Size(177, 32);
            label3.TabIndex = 22;
            label3.Text = "Total Residents";
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.BackgroundImage = (Image)resources.GetObject("pictureBox1.BackgroundImage");
            pictureBox1.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox1.Location = new Point(0, 39);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(99, 75);
            pictureBox1.TabIndex = 21;
            pictureBox1.TabStop = false;
            // 
            // btnNotification
            // 
            btnNotification.BackColor = Color.SkyBlue;
            btnNotification.BackgroundImage = (Image)resources.GetObject("btnNotification.BackgroundImage");
            btnNotification.BackgroundImageLayout = ImageLayout.Zoom;
            btnNotification.FlatStyle = FlatStyle.Popup;
            btnNotification.ForeColor = Color.White;
            btnNotification.Location = new Point(1283, 12);
            btnNotification.Name = "btnNotification";
            btnNotification.Size = new Size(68, 48);
            btnNotification.TabIndex = 45;
            btnNotification.UseVisualStyleBackColor = false;
            btnNotification.Click += btnNotification_Click;
            // 
            // TayudLGU
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.BCO_677f5122_1ca8_402c_addd_6f3eadad8129;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1511, 948);
            Controls.Add(btnNotification);
            Controls.Add(PMostCommon);
            Controls.Add(PTotalRecords);
            Controls.Add(PTotalResidents);
            Controls.Add(label2);
            Controls.Add(dgvRecentHealth);
            Controls.Add(label1);
            Controls.Add(btnSettings);
            Controls.Add(btnLogout);
            Controls.Add(pictureBox4);
            Controls.Add(btnAdminDash);
            Controls.Add(panel2);
            Name = "TayudLGU";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "TayudLGU";
            Load += TayudLGU_Load;
            ((System.ComponentModel.ISupportInitialize)dgvRecentHealth).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            panel2.ResumeLayout(false);
            PMostCommon.ResumeLayout(false);
            PMostCommon.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            PTotalRecords.ResumeLayout(false);
            PTotalRecords.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            PTotalResidents.ResumeLayout(false);
            PTotalResidents.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnInventory;
        private Panel panel1;
        private Button btnManageRecord;
        private Button btnViewRecord;
        private Button btnGenerateReport;
        private Button btnViewArchive;
        private Label label2;
        private DataGridView dgvRecentHealth;
        private Label label1;
        private Button btnSettings;
        private Button btnLogout;
        private PictureBox pictureBox4;
        private Button btnAdminDash;
        private Panel panel2;
        private Panel PMostCommon;
        private Label lblMostCommon;
        private Label label6;
        private PictureBox pictureBox3;
        private Panel PTotalRecords;
        private Label lblTotalRecords;
        private Label label5;
        private PictureBox pictureBox2;
        private Panel PTotalResidents;
        private Label lblTotalResidents;
        private Label label3;
        private PictureBox pictureBox1;
        private Button btnNotification;
        private Button btnViewApp;
    }
}