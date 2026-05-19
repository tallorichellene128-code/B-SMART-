namespace BSMART
{
    partial class TayudBCapViewArch2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TayudBCapViewArch2));
            panel1 = new Panel();
            label1 = new Label();
            btnSettings = new Button();
            btnLogout = new Button();
            pictureBox4 = new PictureBox();
            btnBCapDash = new Button();
            panel2 = new Panel();
            btnHealthRep = new Button();
            btnManageFrozenAcc = new Button();
            btnViewArchive = new Button();
            btnViewResAcc = new Button();
            btnManageResAcc = new Button();
            btnResManageRecord = new Button();
            btnResViewRecord = new Button();
            btnDelete = new Button();
            btnRestore = new Button();
            dtpBirthday = new DateTimePicker();
            label8 = new Label();
            cmbGender = new ComboBox();
            cmbAge = new ComboBox();
            label7 = new Label();
            label6 = new Label();
            label5 = new Label();
            txtLastName = new TextBox();
            label4 = new Label();
            txtFirstName = new TextBox();
            label3 = new Label();
            dgvArchResAcc = new DataGridView();
            btnBack = new Button();
            label2 = new Label();
            txtEmailAdd = new TextBox();
            label9 = new Label();
            txtArchAccSearch = new TextBox();
            btnNotification = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvArchResAcc).BeginInit();
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
            panel2.Controls.Add(btnHealthRep);
            panel2.Controls.Add(panel1);
            panel2.Controls.Add(btnManageFrozenAcc);
            panel2.Controls.Add(btnViewArchive);
            panel2.Controls.Add(btnViewResAcc);
            panel2.Controls.Add(btnManageResAcc);
            panel2.Controls.Add(btnResManageRecord);
            panel2.Controls.Add(btnResViewRecord);
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(260, 949);
            panel2.TabIndex = 50;
            // 
            // btnHealthRep
            // 
            btnHealthRep.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnHealthRep.Location = new Point(0, 747);
            btnHealthRep.Name = "btnHealthRep";
            btnHealthRep.Size = new Size(260, 74);
            btnHealthRep.TabIndex = 56;
            btnHealthRep.Text = "View Residents Health Report";
            btnHealthRep.UseVisualStyleBackColor = true;
            btnHealthRep.Click += btnHealthRep_Click;
            // 
            // btnManageFrozenAcc
            // 
            btnManageFrozenAcc.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnManageFrozenAcc.Location = new Point(0, 667);
            btnManageFrozenAcc.Name = "btnManageFrozenAcc";
            btnManageFrozenAcc.Size = new Size(260, 74);
            btnManageFrozenAcc.TabIndex = 55;
            btnManageFrozenAcc.Text = "Manage Frozen Accounts";
            btnManageFrozenAcc.UseVisualStyleBackColor = true;
            btnManageFrozenAcc.Click += btnManageFrozenAcc_Click;
            // 
            // btnViewArchive
            // 
            btnViewArchive.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnViewArchive.Location = new Point(0, 827);
            btnViewArchive.Name = "btnViewArchive";
            btnViewArchive.Size = new Size(260, 74);
            btnViewArchive.TabIndex = 53;
            btnViewArchive.Text = "View Archive";
            btnViewArchive.UseVisualStyleBackColor = true;
            btnViewArchive.Click += btnViewArchive_Click;
            // 
            // btnViewResAcc
            // 
            btnViewResAcc.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnViewResAcc.Location = new Point(0, 587);
            btnViewResAcc.Name = "btnViewResAcc";
            btnViewResAcc.Size = new Size(260, 74);
            btnViewResAcc.TabIndex = 54;
            btnViewResAcc.Text = "View Resident Accounts";
            btnViewResAcc.UseVisualStyleBackColor = true;
            btnViewResAcc.Click += btnViewResAcc_Click;
            // 
            // btnManageResAcc
            // 
            btnManageResAcc.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnManageResAcc.Location = new Point(0, 507);
            btnManageResAcc.Name = "btnManageResAcc";
            btnManageResAcc.Size = new Size(260, 74);
            btnManageResAcc.TabIndex = 52;
            btnManageResAcc.Text = "Manage Resident Accounts";
            btnManageResAcc.UseVisualStyleBackColor = true;
            btnManageResAcc.Click += btnManageResAcc_Click;
            // 
            // btnResManageRecord
            // 
            btnResManageRecord.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnResManageRecord.Location = new Point(0, 347);
            btnResManageRecord.Name = "btnResManageRecord";
            btnResManageRecord.Size = new Size(260, 74);
            btnResManageRecord.TabIndex = 50;
            btnResManageRecord.Text = "Manage Resident Record";
            btnResManageRecord.UseVisualStyleBackColor = true;
            btnResManageRecord.Click += btnResManageRecord_Click;
            // 
            // btnResViewRecord
            // 
            btnResViewRecord.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnResViewRecord.Location = new Point(0, 427);
            btnResViewRecord.Name = "btnResViewRecord";
            btnResViewRecord.Size = new Size(260, 74);
            btnResViewRecord.TabIndex = 51;
            btnResViewRecord.Text = "View Resident Record\r\n";
            btnResViewRecord.UseVisualStyleBackColor = true;
            btnResViewRecord.Click += btnResViewRecord_Click;
            // 
            // btnDelete
            // 
            btnDelete.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnDelete.Location = new Point(1051, 815);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(300, 49);
            btnDelete.TabIndex = 79;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnRestore
            // 
            btnRestore.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnRestore.Location = new Point(538, 815);
            btnRestore.Name = "btnRestore";
            btnRestore.Size = new Size(300, 49);
            btnRestore.TabIndex = 78;
            btnRestore.Text = "Restore";
            btnRestore.UseVisualStyleBackColor = true;
            btnRestore.Click += btnRestore_Click;
            // 
            // dtpBirthday
            // 
            dtpBirthday.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpBirthday.Location = new Point(538, 745);
            dtpBirthday.Name = "dtpBirthday";
            dtpBirthday.Size = new Size(300, 39);
            dtpBirthday.TabIndex = 77;
            dtpBirthday.ValueChanged += dtpBirthday_ValueChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.BackColor = Color.Transparent;
            label8.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label8.Location = new Point(425, 745);
            label8.Name = "label8";
            label8.Size = new Size(107, 32);
            label8.TabIndex = 76;
            label8.Text = "Birthday:";
            // 
            // cmbGender
            // 
            cmbGender.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbGender.FormattingEnabled = true;
            cmbGender.Location = new Point(1051, 685);
            cmbGender.Name = "cmbGender";
            cmbGender.Size = new Size(300, 40);
            cmbGender.TabIndex = 75;
            cmbGender.SelectedIndexChanged += cmbGender_SelectedIndexChanged;
            // 
            // cmbAge
            // 
            cmbAge.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbAge.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbAge.FormattingEnabled = true;
            cmbAge.IntegralHeight = false;
            cmbAge.Location = new Point(538, 685);
            cmbAge.MaxDropDownItems = 5;
            cmbAge.Name = "cmbAge";
            cmbAge.Size = new Size(300, 40);
            cmbAge.TabIndex = 74;
            cmbAge.SelectedIndexChanged += cmbAge_SelectedIndexChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.Transparent;
            label7.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label7.Location = new Point(948, 688);
            label7.Name = "label7";
            label7.Size = new Size(97, 32);
            label7.TabIndex = 73;
            label7.Text = "Gender:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.Transparent;
            label6.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(471, 688);
            label6.Name = "label6";
            label6.Size = new Size(61, 32);
            label6.TabIndex = 72;
            label6.Text = "Age:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(914, 635);
            label5.Name = "label5";
            label5.Size = new Size(131, 32);
            label5.TabIndex = 71;
            label5.Text = "Last Name:";
            // 
            // txtLastName
            // 
            txtLastName.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtLastName.Location = new Point(1051, 628);
            txtLastName.Name = "txtLastName";
            txtLastName.Size = new Size(300, 39);
            txtLastName.TabIndex = 70;
            txtLastName.TextChanged += txtLastName_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(398, 638);
            label4.Name = "label4";
            label4.Size = new Size(134, 32);
            label4.TabIndex = 69;
            label4.Text = "First Name:";
            // 
            // txtFirstName
            // 
            txtFirstName.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtFirstName.Location = new Point(538, 631);
            txtFirstName.Name = "txtFirstName";
            txtFirstName.Size = new Size(300, 39);
            txtFirstName.TabIndex = 68;
            txtFirstName.TextChanged += txtFirstName_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(757, 212);
            label3.Name = "label3";
            label3.Size = new Size(213, 32);
            label3.TabIndex = 67;
            label3.Text = "Resident Accounts";
            // 
            // dgvArchResAcc
            // 
            dgvArchResAcc.BackgroundColor = SystemColors.GradientInactiveCaption;
            dgvArchResAcc.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvArchResAcc.Location = new Point(398, 292);
            dgvArchResAcc.Name = "dgvArchResAcc";
            dgvArchResAcc.RowHeadersWidth = 62;
            dgvArchResAcc.Size = new Size(953, 314);
            dgvArchResAcc.TabIndex = 65;
            dgvArchResAcc.CellClick += dgvArchResAcc_CellClick;
            // 
            // btnBack
            // 
            btnBack.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnBack.Location = new Point(266, 900);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(175, 49);
            btnBack.TabIndex = 80;
            btnBack.Text = "Previous";
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += btnBack_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(768, 88);
            label2.Name = "label2";
            label2.Size = new Size(194, 60);
            label2.TabIndex = 81;
            label2.Text = "Archives";
            // 
            // txtEmailAdd
            // 
            txtEmailAdd.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtEmailAdd.Location = new Point(1051, 747);
            txtEmailAdd.Name = "txtEmailAdd";
            txtEmailAdd.Size = new Size(300, 39);
            txtEmailAdd.TabIndex = 83;
            txtEmailAdd.TextChanged += txtEmailAdd_TextChanged;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.BackColor = Color.Transparent;
            label9.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label9.Location = new Point(897, 752);
            label9.Name = "label9";
            label9.Size = new Size(153, 30);
            label9.TabIndex = 82;
            label9.Text = "Email Address:";
            // 
            // txtArchAccSearch
            // 
            txtArchAccSearch.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtArchAccSearch.Location = new Point(557, 247);
            txtArchAccSearch.Name = "txtArchAccSearch";
            txtArchAccSearch.Size = new Size(645, 39);
            txtArchAccSearch.TabIndex = 84;
            txtArchAccSearch.TextChanged += txtArchAccSearch_TextChanged;
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
            btnNotification.TabIndex = 85;
            btnNotification.UseVisualStyleBackColor = false;
            // 
            // TayudBCapViewArch2
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.BCO_677f5122_1ca8_402c_addd_6f3eadad8129;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1511, 948);
            Controls.Add(btnNotification);
            Controls.Add(txtArchAccSearch);
            Controls.Add(txtEmailAdd);
            Controls.Add(label9);
            Controls.Add(label2);
            Controls.Add(btnBack);
            Controls.Add(btnDelete);
            Controls.Add(btnRestore);
            Controls.Add(dtpBirthday);
            Controls.Add(label8);
            Controls.Add(cmbGender);
            Controls.Add(cmbAge);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(txtLastName);
            Controls.Add(label4);
            Controls.Add(txtFirstName);
            Controls.Add(label3);
            Controls.Add(dgvArchResAcc);
            Controls.Add(label1);
            Controls.Add(btnSettings);
            Controls.Add(btnLogout);
            Controls.Add(pictureBox4);
            Controls.Add(btnBCapDash);
            Controls.Add(panel2);
            Name = "TayudBCapViewArch2";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "TayudBCapViewArch2";
            Load += TayudBCapViewArch2_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvArchResAcc).EndInit();
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
        private Button btnHealthRep;
        private Button btnManageFrozenAcc;
        private Button btnViewArchive;
        private Button btnViewResAcc;
        private Button btnManageResAcc;
        private Button btnResManageRecord;
        private Button btnResViewRecord;
        private Button btnDelete;
        private Button btnRestore;
        private DateTimePicker dtpBirthday;
        private Label label8;
        private ComboBox cmbGender;
        private ComboBox cmbAge;
        private Label label7;
        private Label label6;
        private Label label5;
        private TextBox txtLastName;
        private Label label4;
        private TextBox txtFirstName;
        private Label label3;
        private DataGridView dgvArchResAcc;
        private Button btnBack;
        private Label label2;
        private TextBox txtEmailAdd;
        private Label label9;
        private TextBox txtArchAccSearch;
        private Button btnNotification;
    }
}