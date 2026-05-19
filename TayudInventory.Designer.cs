namespace BSMART
{
    partial class TayudInventory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TayudInventory));
            label1 = new Label();
            btnSettings = new Button();
            btnLogout = new Button();
            pictureBox4 = new PictureBox();
            btnAdminDash = new Button();
            panel2 = new Panel();
            btnViewApp = new Button();
            btnInventory = new Button();
            panel1 = new Panel();
            btnManageRecord = new Button();
            btnViewRecord = new Button();
            btnGenerateReport = new Button();
            btnViewArchive = new Button();
            label2 = new Label();
            dgvMedicines = new DataGridView();
            label3 = new Label();
            dgvVaccines = new DataGridView();
            label4 = new Label();
            btnAddMed = new Button();
            btnArchiveMed = new Button();
            btnRemoveMed = new Button();
            btnAddVacc = new Button();
            btnArchiveVacc = new Button();
            btnRemoveVacc = new Button();
            txtMed = new TextBox();
            label5 = new Label();
            dtpExpiryMed = new DateTimePicker();
            label6 = new Label();
            label7 = new Label();
            dtpExpiryVacc = new DateTimePicker();
            label8 = new Label();
            txtVaccines = new TextBox();
            txtMedSearch = new TextBox();
            txtVaccSearch = new TextBox();
            cmbMedQty = new ComboBox();
            label9 = new Label();
            cmbVaccQty = new ComboBox();
            label10 = new Label();
            btnNotification = new Button();
            btnUpdateMed = new Button();
            btnUpdateVacc = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMedicines).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvVaccines).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.SkyBlue;
            label1.Font = new Font("Stencil", 20F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.Location = new Point(768, 13);
            label1.Name = "label1";
            label1.Size = new Size(187, 47);
            label1.TabIndex = 31;
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
            btnSettings.TabIndex = 30;
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
            btnLogout.TabIndex = 29;
            btnLogout.UseVisualStyleBackColor = false;
            // 
            // pictureBox4
            // 
            pictureBox4.BackColor = Color.SkyBlue;
            pictureBox4.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox4.Location = new Point(259, 0);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(1252, 75);
            pictureBox4.TabIndex = 28;
            pictureBox4.TabStop = false;
            // 
            // btnAdminDash
            // 
            btnAdminDash.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnAdminDash.Location = new Point(0, 229);
            btnAdminDash.Name = "btnAdminDash";
            btnAdminDash.Size = new Size(260, 81);
            btnAdminDash.TabIndex = 27;
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
            panel2.TabIndex = 26;
            // 
            // btnViewApp
            // 
            btnViewApp.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnViewApp.Location = new Point(0, 796);
            btnViewApp.Name = "btnViewApp";
            btnViewApp.Size = new Size(260, 74);
            btnViewApp.TabIndex = 52;
            btnViewApp.Text = "View Resident Appointments";
            btnViewApp.UseVisualStyleBackColor = true;
            btnViewApp.Click += btnViewApp_Click;
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
            label2.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(758, 98);
            label2.Name = "label2";
            label2.Size = new Size(217, 60);
            label2.TabIndex = 32;
            label2.Text = "Inventory";
            // 
            // dgvMedicines
            // 
            dgvMedicines.BackgroundColor = SystemColors.GradientInactiveCaption;
            dgvMedicines.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMedicines.Location = new Point(318, 256);
            dgvMedicines.Name = "dgvMedicines";
            dgvMedicines.RowHeadersWidth = 62;
            dgvMedicines.Size = new Size(526, 309);
            dgvMedicines.TabIndex = 33;
            dgvMedicines.CellContentClick += dgvMedicines_CellContentClick;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(504, 170);
            label3.Name = "label3";
            label3.Size = new Size(145, 38);
            label3.TabIndex = 34;
            label3.Text = "Medicines";
            // 
            // dgvVaccines
            // 
            dgvVaccines.BackgroundColor = SystemColors.GradientInactiveCaption;
            dgvVaccines.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvVaccines.Location = new Point(917, 256);
            dgvVaccines.Name = "dgvVaccines";
            dgvVaccines.RowHeadersWidth = 62;
            dgvVaccines.Size = new Size(526, 309);
            dgvVaccines.TabIndex = 35;
            dgvVaccines.CellContentClick += dgvVaccines_CellContentClick;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(1123, 170);
            label4.Name = "label4";
            label4.Size = new Size(124, 38);
            label4.TabIndex = 36;
            label4.Text = "Vaccines";
            // 
            // btnAddMed
            // 
            btnAddMed.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnAddMed.Location = new Point(318, 770);
            btnAddMed.Name = "btnAddMed";
            btnAddMed.Size = new Size(258, 78);
            btnAddMed.TabIndex = 37;
            btnAddMed.Text = "Add Medicine";
            btnAddMed.UseVisualStyleBackColor = true;
            btnAddMed.Click += btnAddMed_Click;
            // 
            // btnArchiveMed
            // 
            btnArchiveMed.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnArchiveMed.Location = new Point(586, 770);
            btnArchiveMed.Name = "btnArchiveMed";
            btnArchiveMed.Size = new Size(258, 78);
            btnArchiveMed.TabIndex = 38;
            btnArchiveMed.Text = "Archive Medicine";
            btnArchiveMed.UseVisualStyleBackColor = true;
            btnArchiveMed.Click += btnArchiveMed_Click;
            // 
            // btnRemoveMed
            // 
            btnRemoveMed.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnRemoveMed.Location = new Point(586, 861);
            btnRemoveMed.Name = "btnRemoveMed";
            btnRemoveMed.Size = new Size(258, 75);
            btnRemoveMed.TabIndex = 38;
            btnRemoveMed.Text = "Remove Medicine";
            btnRemoveMed.UseVisualStyleBackColor = true;
            btnRemoveMed.Click += btnRemoveMed_Click;
            // 
            // btnAddVacc
            // 
            btnAddVacc.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnAddVacc.Location = new Point(917, 770);
            btnAddVacc.Name = "btnAddVacc";
            btnAddVacc.Size = new Size(258, 78);
            btnAddVacc.TabIndex = 40;
            btnAddVacc.Text = "Add Vaccine";
            btnAddVacc.UseVisualStyleBackColor = true;
            btnAddVacc.Click += btnAddVacc_Click;
            // 
            // btnArchiveVacc
            // 
            btnArchiveVacc.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnArchiveVacc.Location = new Point(1185, 770);
            btnArchiveVacc.Name = "btnArchiveVacc";
            btnArchiveVacc.Size = new Size(258, 78);
            btnArchiveVacc.TabIndex = 41;
            btnArchiveVacc.Text = "Archive Vaccine";
            btnArchiveVacc.UseVisualStyleBackColor = true;
            btnArchiveVacc.Click += btnArchiveVacc_Click;
            // 
            // btnRemoveVacc
            // 
            btnRemoveVacc.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnRemoveVacc.Location = new Point(1185, 861);
            btnRemoveVacc.Name = "btnRemoveVacc";
            btnRemoveVacc.Size = new Size(258, 75);
            btnRemoveVacc.TabIndex = 42;
            btnRemoveVacc.Text = "Remove Vaccine";
            btnRemoveVacc.UseVisualStyleBackColor = true;
            btnRemoveVacc.Click += btnRemoveVacc_Click;
            // 
            // txtMed
            // 
            txtMed.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtMed.Location = new Point(318, 616);
            txtMed.Name = "txtMed";
            txtMed.Size = new Size(406, 39);
            txtMed.TabIndex = 22;
            txtMed.TextChanged += txtMed_TextChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(318, 581);
            label5.Name = "label5";
            label5.Size = new Size(113, 32);
            label5.TabIndex = 43;
            label5.Text = "Medicine";
            // 
            // dtpExpiryMed
            // 
            dtpExpiryMed.CalendarFont = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpExpiryMed.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpExpiryMed.Location = new Point(318, 707);
            dtpExpiryMed.Name = "dtpExpiryMed";
            dtpExpiryMed.Size = new Size(526, 39);
            dtpExpiryMed.TabIndex = 44;
            dtpExpiryMed.ValueChanged += dtpExpiryMed_ValueChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.Transparent;
            label6.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(318, 672);
            label6.Name = "label6";
            label6.Size = new Size(134, 32);
            label6.TabIndex = 45;
            label6.Text = "Expiry Date";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.Transparent;
            label7.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label7.Location = new Point(917, 672);
            label7.Name = "label7";
            label7.Size = new Size(134, 32);
            label7.TabIndex = 49;
            label7.Text = "Expiry Date";
            // 
            // dtpExpiryVacc
            // 
            dtpExpiryVacc.CalendarFont = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpExpiryVacc.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpExpiryVacc.Location = new Point(917, 707);
            dtpExpiryVacc.Name = "dtpExpiryVacc";
            dtpExpiryVacc.Size = new Size(526, 39);
            dtpExpiryVacc.TabIndex = 48;
            dtpExpiryVacc.ValueChanged += dtpExpiryVacc_ValueChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.BackColor = Color.Transparent;
            label8.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label8.Location = new Point(917, 581);
            label8.Name = "label8";
            label8.Size = new Size(104, 32);
            label8.TabIndex = 47;
            label8.Text = "Vaccines";
            // 
            // txtVaccines
            // 
            txtVaccines.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtVaccines.Location = new Point(917, 616);
            txtVaccines.Name = "txtVaccines";
            txtVaccines.Size = new Size(408, 39);
            txtVaccines.TabIndex = 46;
            txtVaccines.TextChanged += txtVaccines_TextChanged;
            // 
            // txtMedSearch
            // 
            txtMedSearch.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtMedSearch.Location = new Point(400, 211);
            txtMedSearch.Name = "txtMedSearch";
            txtMedSearch.Size = new Size(356, 39);
            txtMedSearch.TabIndex = 50;
            txtMedSearch.TextChanged += txtMedSearch_TextChanged;
            // 
            // txtVaccSearch
            // 
            txtVaccSearch.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtVaccSearch.Location = new Point(1006, 211);
            txtVaccSearch.Name = "txtVaccSearch";
            txtVaccSearch.Size = new Size(356, 39);
            txtVaccSearch.TabIndex = 51;
            txtVaccSearch.TextChanged += txtVaccSearch_TextChanged;
            // 
            // cmbMedQty
            // 
            cmbMedQty.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMedQty.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbMedQty.FormattingEnabled = true;
            cmbMedQty.IntegralHeight = false;
            cmbMedQty.Items.AddRange(new object[] { "10", "20", "30", "40", "50", "60", "70", "80", "90", "100", "110", "120", "130", "140", "150" });
            cmbMedQty.Location = new Point(730, 616);
            cmbMedQty.MaxDropDownItems = 5;
            cmbMedQty.Name = "cmbMedQty";
            cmbMedQty.Size = new Size(114, 40);
            cmbMedQty.TabIndex = 52;
            cmbMedQty.SelectedIndexChanged += cmbMedQty_SelectedIndexChanged;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.BackColor = Color.Transparent;
            label9.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label9.Location = new Point(730, 577);
            label9.Name = "label9";
            label9.Size = new Size(57, 32);
            label9.TabIndex = 53;
            label9.Text = "Qty.";
            // 
            // cmbVaccQty
            // 
            cmbVaccQty.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbVaccQty.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbVaccQty.FormattingEnabled = true;
            cmbVaccQty.IntegralHeight = false;
            cmbVaccQty.Items.AddRange(new object[] { "10", "20", "30", "40", "50", "60", "70", "80", "90", "100", "110", "120", "130", "140", "150" });
            cmbVaccQty.Location = new Point(1329, 615);
            cmbVaccQty.MaxDropDownItems = 5;
            cmbVaccQty.Name = "cmbVaccQty";
            cmbVaccQty.Size = new Size(114, 40);
            cmbVaccQty.TabIndex = 54;
            cmbVaccQty.SelectedIndexChanged += cmbVaccQty_SelectedIndexChanged;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.BackColor = Color.Transparent;
            label10.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label10.Location = new Point(1329, 581);
            label10.Name = "label10";
            label10.Size = new Size(57, 32);
            label10.TabIndex = 55;
            label10.Text = "Qty.";
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
            btnNotification.TabIndex = 56;
            btnNotification.UseVisualStyleBackColor = false;
            // 
            // btnUpdateMed
            // 
            btnUpdateMed.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnUpdateMed.Location = new Point(318, 854);
            btnUpdateMed.Name = "btnUpdateMed";
            btnUpdateMed.Size = new Size(258, 78);
            btnUpdateMed.TabIndex = 57;
            btnUpdateMed.Text = "Update Medicine";
            btnUpdateMed.UseVisualStyleBackColor = true;
            btnUpdateMed.Click += btnUpdateMed_Click;
            // 
            // btnUpdateVacc
            // 
            btnUpdateVacc.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnUpdateVacc.Location = new Point(917, 854);
            btnUpdateVacc.Name = "btnUpdateVacc";
            btnUpdateVacc.Size = new Size(258, 78);
            btnUpdateVacc.TabIndex = 58;
            btnUpdateVacc.Text = "Update Vaccine";
            btnUpdateVacc.UseVisualStyleBackColor = true;
            btnUpdateVacc.Click += btnUpdateVacc_Click;
            // 
            // TayudInventory
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.BCO_677f5122_1ca8_402c_addd_6f3eadad8129;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1511, 948);
            Controls.Add(btnUpdateVacc);
            Controls.Add(btnUpdateMed);
            Controls.Add(btnNotification);
            Controls.Add(label10);
            Controls.Add(cmbVaccQty);
            Controls.Add(label9);
            Controls.Add(cmbMedQty);
            Controls.Add(txtVaccSearch);
            Controls.Add(txtMedSearch);
            Controls.Add(label7);
            Controls.Add(dtpExpiryVacc);
            Controls.Add(label8);
            Controls.Add(txtVaccines);
            Controls.Add(label6);
            Controls.Add(dtpExpiryMed);
            Controls.Add(label5);
            Controls.Add(txtMed);
            Controls.Add(btnRemoveVacc);
            Controls.Add(btnArchiveVacc);
            Controls.Add(btnAddVacc);
            Controls.Add(btnRemoveMed);
            Controls.Add(btnArchiveMed);
            Controls.Add(btnAddMed);
            Controls.Add(label4);
            Controls.Add(dgvVaccines);
            Controls.Add(label3);
            Controls.Add(dgvMedicines);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnSettings);
            Controls.Add(btnLogout);
            Controls.Add(pictureBox4);
            Controls.Add(btnAdminDash);
            Controls.Add(panel2);
            Name = "TayudInventory";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "InventoryLGU";
            Load += TayudInventory_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvMedicines).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvVaccines).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button btnSettings;
        private Button btnLogout;
        private PictureBox pictureBox4;
        private Button btnAdminDash;
        private Panel panel2;
        private Button btnInventory;
        private Panel panel1;
        private Button btnManageRecord;
        private Button btnViewRecord;
        private Button btnGenerateReport;
        private Button btnViewArchive;
        private Label label2;
        private DataGridView dgvMedicines;
        private Label label3;
        private DataGridView dgvVaccines;
        private Label label4;
        private Button btnAddMed;
        private Button btnArchiveMed;
        private Button btnRemoveMed;
        private Button btnAddVacc;
        private Button btnArchiveVacc;
        private Button btnRemoveVacc;
        private TextBox txtMed;
        private Label label5;
        private DateTimePicker dtpExpiryMed;
        private Label label6;
        private Label label7;
        private DateTimePicker dtpExpiryVacc;
        private Label label8;
        private TextBox txtVaccines;
        private TextBox txtMedSearch;
        private TextBox txtVaccSearch;
        private Button btnViewApp;
        private ComboBox cmbMedQty;
        private Label label9;
        private ComboBox cmbVaccQty;
        private Label label10;
        private Button btnNotification;
        private Button btnUpdateMed;
        private Button btnUpdateVacc;
    }
}