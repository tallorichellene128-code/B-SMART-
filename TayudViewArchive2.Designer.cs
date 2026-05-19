namespace BSMART
{
    partial class TayudViewArchive2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TayudViewArchive2));
            label2 = new Label();
            btnInventory = new Button();
            panel1 = new Panel();
            btnManageRecord = new Button();
            btnViewRecord = new Button();
            btnGenerateReport = new Button();
            btnViewArchive = new Button();
            label1 = new Label();
            btnSettings = new Button();
            btnLogout = new Button();
            pictureBox4 = new PictureBox();
            btnAdminDash = new Button();
            panel2 = new Panel();
            btnViewApp = new Button();
            label7 = new Label();
            dtpArchExpiryVacc = new DateTimePicker();
            label8 = new Label();
            txtArchVacc = new TextBox();
            label6 = new Label();
            dtpArchExpiryMed = new DateTimePicker();
            label5 = new Label();
            txtArchMed = new TextBox();
            label4 = new Label();
            dgvArchVaccines = new DataGridView();
            label3 = new Label();
            dgvArchMedicines = new DataGridView();
            txtArchMedSearch = new TextBox();
            txtArchVaccSearch = new TextBox();
            btnDeleteMed = new Button();
            btnRestoreMed = new Button();
            btnDeleteVacc = new Button();
            btnRestoreVacc = new Button();
            btnPrev = new Button();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            label9 = new Label();
            cmbArchMedQty = new ComboBox();
            label10 = new Label();
            cmbArchVaccQty = new ComboBox();
            btnNotification = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvArchVaccines).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvArchMedicines).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(768, 78);
            label2.Name = "label2";
            label2.Size = new Size(194, 60);
            label2.TabIndex = 46;
            label2.Text = "Archives";
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
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.SkyBlue;
            label1.Font = new Font("Stencil", 20F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.Location = new Point(768, 13);
            label1.Name = "label1";
            label1.Size = new Size(187, 47);
            label1.TabIndex = 45;
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
            btnSettings.TabIndex = 44;
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
            btnLogout.TabIndex = 43;
            btnLogout.UseVisualStyleBackColor = false;
            // 
            // pictureBox4
            // 
            pictureBox4.BackColor = Color.SkyBlue;
            pictureBox4.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox4.Location = new Point(259, 0);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(1252, 75);
            pictureBox4.TabIndex = 42;
            pictureBox4.TabStop = false;
            // 
            // btnAdminDash
            // 
            btnAdminDash.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnAdminDash.Location = new Point(0, 229);
            btnAdminDash.Name = "btnAdminDash";
            btnAdminDash.Size = new Size(260, 81);
            btnAdminDash.TabIndex = 41;
            btnAdminDash.Text = "Admin Dashboard\r\n";
            btnAdminDash.UseVisualStyleBackColor = true;
            btnAdminDash.Click += btnAdminDash_Click;
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
            panel2.TabIndex = 40;
            // 
            // btnViewApp
            // 
            btnViewApp.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnViewApp.Location = new Point(0, 796);
            btnViewApp.Name = "btnViewApp";
            btnViewApp.Size = new Size(260, 74);
            btnViewApp.TabIndex = 22;
            btnViewApp.Text = "View Resident Appointments";
            btnViewApp.UseVisualStyleBackColor = true;
            btnViewApp.Click += btnViewApp_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.Transparent;
            label7.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label7.Location = new Point(920, 686);
            label7.Name = "label7";
            label7.Size = new Size(134, 32);
            label7.TabIndex = 61;
            label7.Text = "Expiry Date";
            // 
            // dtpArchExpiryVacc
            // 
            dtpArchExpiryVacc.CalendarFont = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpArchExpiryVacc.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpArchExpiryVacc.Location = new Point(920, 721);
            dtpArchExpiryVacc.Name = "dtpArchExpiryVacc";
            dtpArchExpiryVacc.Size = new Size(526, 39);
            dtpArchExpiryVacc.TabIndex = 60;
            dtpArchExpiryVacc.ValueChanged += dtpArchExpiryVacc_ValueChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.BackColor = Color.Transparent;
            label8.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label8.Location = new Point(920, 595);
            label8.Name = "label8";
            label8.Size = new Size(94, 32);
            label8.TabIndex = 59;
            label8.Text = "Vaccine";
            // 
            // txtArchVacc
            // 
            txtArchVacc.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtArchVacc.Location = new Point(920, 630);
            txtArchVacc.Name = "txtArchVacc";
            txtArchVacc.Size = new Size(406, 39);
            txtArchVacc.TabIndex = 58;
            txtArchVacc.TextChanged += txtArchVacc_TextChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.Transparent;
            label6.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(321, 686);
            label6.Name = "label6";
            label6.Size = new Size(134, 32);
            label6.TabIndex = 57;
            label6.Text = "Expiry Date";
            // 
            // dtpArchExpiryMed
            // 
            dtpArchExpiryMed.CalendarFont = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpArchExpiryMed.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpArchExpiryMed.Location = new Point(321, 721);
            dtpArchExpiryMed.Name = "dtpArchExpiryMed";
            dtpArchExpiryMed.Size = new Size(526, 39);
            dtpArchExpiryMed.TabIndex = 56;
            dtpArchExpiryMed.ValueChanged += dtpArchExpiryMed_ValueChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(321, 595);
            label5.Name = "label5";
            label5.Size = new Size(113, 32);
            label5.TabIndex = 55;
            label5.Text = "Medicine";
            // 
            // txtArchMed
            // 
            txtArchMed.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtArchMed.Location = new Point(321, 630);
            txtArchMed.Name = "txtArchMed";
            txtArchMed.Size = new Size(404, 39);
            txtArchMed.TabIndex = 50;
            txtArchMed.TextChanged += txtArchMed_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(1122, 184);
            label4.Name = "label4";
            label4.Size = new Size(124, 38);
            label4.TabIndex = 54;
            label4.Text = "Vaccines";
            // 
            // dgvArchVaccines
            // 
            dgvArchVaccines.BackgroundColor = SystemColors.GradientInactiveCaption;
            dgvArchVaccines.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvArchVaccines.Location = new Point(920, 270);
            dgvArchVaccines.Name = "dgvArchVaccines";
            dgvArchVaccines.RowHeadersWidth = 62;
            dgvArchVaccines.Size = new Size(526, 309);
            dgvArchVaccines.TabIndex = 53;
            dgvArchVaccines.CellContentClick += dgvArchVaccines_CellContentClick;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(502, 184);
            label3.Name = "label3";
            label3.Size = new Size(145, 38);
            label3.TabIndex = 52;
            label3.Text = "Medicines";
            // 
            // dgvArchMedicines
            // 
            dgvArchMedicines.BackgroundColor = SystemColors.GradientInactiveCaption;
            dgvArchMedicines.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvArchMedicines.Location = new Point(321, 270);
            dgvArchMedicines.Name = "dgvArchMedicines";
            dgvArchMedicines.RowHeadersWidth = 62;
            dgvArchMedicines.Size = new Size(526, 309);
            dgvArchMedicines.TabIndex = 51;
            dgvArchMedicines.CellContentClick += dgvArchMedicines_CellContentClick;
            // 
            // txtArchMedSearch
            // 
            txtArchMedSearch.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtArchMedSearch.Location = new Point(401, 225);
            txtArchMedSearch.Name = "txtArchMedSearch";
            txtArchMedSearch.Size = new Size(383, 39);
            txtArchMedSearch.TabIndex = 62;
            txtArchMedSearch.TextChanged += txtArchMedSearch_TextChanged;
            // 
            // txtArchVaccSearch
            // 
            txtArchVaccSearch.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtArchVaccSearch.Location = new Point(993, 225);
            txtArchVaccSearch.Name = "txtArchVaccSearch";
            txtArchVaccSearch.Size = new Size(383, 39);
            txtArchVaccSearch.TabIndex = 63;
            txtArchVaccSearch.TextChanged += txtArchVaccSearch_TextChanged;
            // 
            // btnDeleteMed
            // 
            btnDeleteMed.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnDeleteMed.Location = new Point(667, 791);
            btnDeleteMed.Name = "btnDeleteMed";
            btnDeleteMed.Size = new Size(180, 49);
            btnDeleteMed.TabIndex = 65;
            btnDeleteMed.Text = "Delete";
            btnDeleteMed.UseVisualStyleBackColor = true;
            btnDeleteMed.Click += btnDeleteMed_Click;
            // 
            // btnRestoreMed
            // 
            btnRestoreMed.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnRestoreMed.Location = new Point(321, 791);
            btnRestoreMed.Name = "btnRestoreMed";
            btnRestoreMed.Size = new Size(180, 49);
            btnRestoreMed.TabIndex = 64;
            btnRestoreMed.Text = "Restore";
            btnRestoreMed.UseVisualStyleBackColor = true;
            btnRestoreMed.Click += btnRestoreMed_Click;
            // 
            // btnDeleteVacc
            // 
            btnDeleteVacc.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnDeleteVacc.Location = new Point(1266, 791);
            btnDeleteVacc.Name = "btnDeleteVacc";
            btnDeleteVacc.Size = new Size(180, 49);
            btnDeleteVacc.TabIndex = 67;
            btnDeleteVacc.Text = "Delete";
            btnDeleteVacc.UseVisualStyleBackColor = true;
            btnDeleteVacc.Click += btnDeleteVacc_Click;
            // 
            // btnRestoreVacc
            // 
            btnRestoreVacc.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnRestoreVacc.Location = new Point(920, 791);
            btnRestoreVacc.Name = "btnRestoreVacc";
            btnRestoreVacc.Size = new Size(180, 49);
            btnRestoreVacc.TabIndex = 66;
            btnRestoreVacc.Text = "Restore";
            btnRestoreVacc.UseVisualStyleBackColor = true;
            btnRestoreVacc.Click += btnRestoreVacc_Click;
            // 
            // btnPrev
            // 
            btnPrev.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnPrev.Location = new Point(266, 887);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(190, 49);
            btnPrev.TabIndex = 68;
            btnPrev.Text = "Previous";
            btnPrev.UseVisualStyleBackColor = true;
            btnPrev.Click += btnPrev_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.BackgroundImage = (Image)resources.GetObject("pictureBox1.BackgroundImage");
            pictureBox1.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox1.Location = new Point(733, 225);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(51, 39);
            pictureBox1.TabIndex = 69;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.Transparent;
            pictureBox2.BackgroundImage = (Image)resources.GetObject("pictureBox2.BackgroundImage");
            pictureBox2.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox2.Location = new Point(1325, 225);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(51, 39);
            pictureBox2.TabIndex = 70;
            pictureBox2.TabStop = false;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.BackColor = Color.Transparent;
            label9.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label9.Location = new Point(733, 590);
            label9.Name = "label9";
            label9.Size = new Size(57, 32);
            label9.TabIndex = 72;
            label9.Text = "Qty.";
            // 
            // cmbArchMedQty
            // 
            cmbArchMedQty.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbArchMedQty.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbArchMedQty.FormattingEnabled = true;
            cmbArchMedQty.IntegralHeight = false;
            cmbArchMedQty.Items.AddRange(new object[] { "10", "20", "30", "40", "50", "60", "70", "80", "90", "100", "110", "120", "130", "140", "150" });
            cmbArchMedQty.Location = new Point(733, 629);
            cmbArchMedQty.MaxDropDownItems = 5;
            cmbArchMedQty.Name = "cmbArchMedQty";
            cmbArchMedQty.Size = new Size(114, 40);
            cmbArchMedQty.TabIndex = 71;
            cmbArchMedQty.SelectedIndexChanged += cmbMedQty_SelectedIndexChanged;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.BackColor = Color.Transparent;
            label10.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label10.Location = new Point(1332, 590);
            label10.Name = "label10";
            label10.Size = new Size(57, 32);
            label10.TabIndex = 74;
            label10.Text = "Qty.";
            // 
            // cmbArchVaccQty
            // 
            cmbArchVaccQty.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbArchVaccQty.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbArchVaccQty.FormattingEnabled = true;
            cmbArchVaccQty.IntegralHeight = false;
            cmbArchVaccQty.Items.AddRange(new object[] { "10", "20", "30", "40", "50", "60", "70", "80", "90", "100", "110", "120", "130", "140", "150" });
            cmbArchVaccQty.Location = new Point(1332, 629);
            cmbArchVaccQty.MaxDropDownItems = 5;
            cmbArchVaccQty.Name = "cmbArchVaccQty";
            cmbArchVaccQty.Size = new Size(114, 40);
            cmbArchVaccQty.TabIndex = 73;
            cmbArchVaccQty.SelectedIndexChanged += cmbVaccQty_SelectedIndexChanged;
            // 
            // btnNotification
            // 
            btnNotification.BackColor = Color.SkyBlue;
            btnNotification.BackgroundImage = Properties.Resources.alert;
            btnNotification.BackgroundImageLayout = ImageLayout.Zoom;
            btnNotification.FlatStyle = FlatStyle.Popup;
            btnNotification.ForeColor = Color.White;
            btnNotification.Location = new Point(1283, 12);
            btnNotification.Name = "btnNotification";
            btnNotification.Size = new Size(68, 48);
            btnNotification.TabIndex = 75;
            btnNotification.UseVisualStyleBackColor = false;
            // 
            // TayudViewArchive2
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.BCO_677f5122_1ca8_402c_addd_6f3eadad8129;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1511, 948);
            Controls.Add(btnNotification);
            Controls.Add(label10);
            Controls.Add(cmbArchVaccQty);
            Controls.Add(label9);
            Controls.Add(cmbArchMedQty);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Controls.Add(btnPrev);
            Controls.Add(btnDeleteVacc);
            Controls.Add(btnRestoreVacc);
            Controls.Add(btnDeleteMed);
            Controls.Add(btnRestoreMed);
            Controls.Add(txtArchVaccSearch);
            Controls.Add(txtArchMedSearch);
            Controls.Add(label7);
            Controls.Add(dtpArchExpiryVacc);
            Controls.Add(label8);
            Controls.Add(txtArchVacc);
            Controls.Add(label6);
            Controls.Add(dtpArchExpiryMed);
            Controls.Add(label5);
            Controls.Add(txtArchMed);
            Controls.Add(label4);
            Controls.Add(dgvArchVaccines);
            Controls.Add(label3);
            Controls.Add(dgvArchMedicines);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnSettings);
            Controls.Add(btnLogout);
            Controls.Add(pictureBox4);
            Controls.Add(btnAdminDash);
            Controls.Add(panel2);
            Name = "TayudViewArchive2";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "TayudViewArchive2";
            Load += TayudViewArchive2_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvArchVaccines).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvArchMedicines).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label2;
        private Button btnInventory;
        private Panel panel1;
        private Button btnManageRecord;
        private Button btnViewRecord;
        private Button btnGenerateReport;
        private Button btnViewArchive;
        private Label label1;
        private Button btnSettings;
        private Button btnLogout;
        private PictureBox pictureBox4;
        private Button btnAdminDash;
        private Panel panel2;
        private Label label7;
        private DateTimePicker dtpArchExpiryVacc;
        private Label label8;
        private TextBox txtArchVacc;
        private Label label6;
        private DateTimePicker dtpArchExpiryMed;
        private Label label5;
        private TextBox txtArchMed;
        private Label label4;
        private DataGridView dgvArchVaccines;
        private Label label3;
        private DataGridView dgvArchMedicines;
        private TextBox txtArchMedSearch;
        private TextBox txtArchVaccSearch;
        private Button btnDeleteMed;
        private Button btnRestoreMed;
        private Button btnDeleteVacc;
        private Button btnRestoreVacc;
        private Button btnPrev;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private Button btnViewApp;
        private Label label9;
        private ComboBox cmbArchMedQty;
        private Label label10;
        private ComboBox cmbArchVaccQty;
        private Button btnNotification;
    }
}