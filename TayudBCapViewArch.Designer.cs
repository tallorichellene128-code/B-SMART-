namespace BSMART
{
    partial class TayudBCapViewArch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TayudBCapViewArch));
            panel1 = new Panel();
            label1 = new Label();
            btnSettings = new Button();
            btnLogout = new Button();
            pictureBox4 = new PictureBox();
            btnBCapDash = new Button();
            panel2 = new Panel();
            btnViewHealthRep = new Button();
            btnManageFrozenAcc = new Button();
            btnViewArchive = new Button();
            btnViewResAcc = new Button();
            btnManageResAcc = new Button();
            btnResManageRecord = new Button();
            btnResViewRecord = new Button();
            cmbGender = new ComboBox();
            cmbAge = new ComboBox();
            label7 = new Label();
            label6 = new Label();
            label5 = new Label();
            txtLastName = new TextBox();
            label4 = new Label();
            txtFirstName = new TextBox();
            label3 = new Label();
            txtArchHealthSearch = new TextBox();
            dgvArchResRec = new DataGridView();
            btnNext = new Button();
            btnDelete = new Button();
            btnRestore = new Button();
            dtpBirthday = new DateTimePicker();
            label8 = new Label();
            label2 = new Label();
            btnNotification = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvArchResRec).BeginInit();
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
            label1.TabIndex = 48;
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
            btnSettings.TabIndex = 47;
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
            btnLogout.TabIndex = 46;
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
            pictureBox4.TabIndex = 45;
            pictureBox4.TabStop = false;
            // 
            // btnBCapDash
            // 
            btnBCapDash.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnBCapDash.Location = new Point(0, 229);
            btnBCapDash.Name = "btnBCapDash";
            btnBCapDash.Size = new Size(260, 81);
            btnBCapDash.TabIndex = 44;
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
            panel2.Controls.Add(btnViewArchive);
            panel2.Controls.Add(btnViewResAcc);
            panel2.Controls.Add(btnManageResAcc);
            panel2.Controls.Add(btnResManageRecord);
            panel2.Controls.Add(btnResViewRecord);
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(260, 949);
            panel2.TabIndex = 43;
            // 
            // btnViewHealthRep
            // 
            btnViewHealthRep.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnViewHealthRep.Location = new Point(0, 747);
            btnViewHealthRep.Name = "btnViewHealthRep";
            btnViewHealthRep.Size = new Size(260, 74);
            btnViewHealthRep.TabIndex = 56;
            btnViewHealthRep.Text = "View Residents Health Report";
            btnViewHealthRep.UseVisualStyleBackColor = true;
            btnViewHealthRep.Click += btnViewHealthRep_Click;
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
            // cmbGender
            // 
            cmbGender.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbGender.FormattingEnabled = true;
            cmbGender.Location = new Point(1086, 692);
            cmbGender.Name = "cmbGender";
            cmbGender.Size = new Size(300, 40);
            cmbGender.TabIndex = 60;
            cmbGender.SelectedIndexChanged += cmbGender_SelectedIndexChanged;
            // 
            // cmbAge
            // 
            cmbAge.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbAge.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbAge.FormattingEnabled = true;
            cmbAge.IntegralHeight = false;
            cmbAge.Location = new Point(573, 692);
            cmbAge.MaxDropDownItems = 5;
            cmbAge.Name = "cmbAge";
            cmbAge.Size = new Size(300, 40);
            cmbAge.TabIndex = 59;
            cmbAge.SelectedIndexChanged += cmbAge_SelectedIndexChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.Transparent;
            label7.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label7.Location = new Point(983, 695);
            label7.Name = "label7";
            label7.Size = new Size(97, 32);
            label7.TabIndex = 58;
            label7.Text = "Gender:";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.Transparent;
            label6.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(506, 695);
            label6.Name = "label6";
            label6.Size = new Size(61, 32);
            label6.TabIndex = 57;
            label6.Text = "Age:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(949, 623);
            label5.Name = "label5";
            label5.Size = new Size(131, 32);
            label5.TabIndex = 56;
            label5.Text = "Last Name:";
            // 
            // txtLastName
            // 
            txtLastName.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtLastName.Location = new Point(1086, 616);
            txtLastName.Name = "txtLastName";
            txtLastName.Size = new Size(300, 39);
            txtLastName.TabIndex = 55;
            txtLastName.TextChanged += txtLastName_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(433, 626);
            label4.Name = "label4";
            label4.Size = new Size(134, 32);
            label4.TabIndex = 54;
            label4.Text = "First Name:";
            // 
            // txtFirstName
            // 
            txtFirstName.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtFirstName.Location = new Point(573, 619);
            txtFirstName.Name = "txtFirstName";
            txtFirstName.Size = new Size(300, 39);
            txtFirstName.TabIndex = 53;
            txtFirstName.TextChanged += txtFirstName_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(810, 200);
            label3.Name = "label3";
            label3.Size = new Size(199, 32);
            label3.TabIndex = 52;
            label3.Text = "Resident Records";
            // 
            // txtArchHealthSearch
            // 
            txtArchHealthSearch.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtArchHealthSearch.Location = new Point(586, 235);
            txtArchHealthSearch.Name = "txtArchHealthSearch";
            txtArchHealthSearch.Size = new Size(645, 39);
            txtArchHealthSearch.TabIndex = 51;
            txtArchHealthSearch.TextChanged += txtArchHealthSearch_TextChanged;
            // 
            // dgvArchResRec
            // 
            dgvArchResRec.BackgroundColor = SystemColors.GradientInactiveCaption;
            dgvArchResRec.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvArchResRec.Location = new Point(433, 280);
            dgvArchResRec.Name = "dgvArchResRec";
            dgvArchResRec.RowHeadersWidth = 62;
            dgvArchResRec.Size = new Size(953, 314);
            dgvArchResRec.TabIndex = 50;
            dgvArchResRec.CellClick += dgvArchResRec_CellClick;
            // 
            // btnNext
            // 
            btnNext.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnNext.Location = new Point(1336, 896);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(175, 49);
            btnNext.TabIndex = 65;
            btnNext.Text = "Next";
            btnNext.UseVisualStyleBackColor = true;
            btnNext.Click += btnNext_Click;
            // 
            // btnDelete
            // 
            btnDelete.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnDelete.Location = new Point(1086, 803);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(300, 49);
            btnDelete.TabIndex = 64;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnRestore
            // 
            btnRestore.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnRestore.Location = new Point(573, 803);
            btnRestore.Name = "btnRestore";
            btnRestore.Size = new Size(300, 49);
            btnRestore.TabIndex = 63;
            btnRestore.Text = "Restore";
            btnRestore.UseVisualStyleBackColor = true;
            btnRestore.Click += btnRestore_Click;
            // 
            // dtpBirthday
            // 
            dtpBirthday.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpBirthday.Location = new Point(573, 743);
            dtpBirthday.Name = "dtpBirthday";
            dtpBirthday.Size = new Size(813, 39);
            dtpBirthday.TabIndex = 62;
            dtpBirthday.ValueChanged += dtpBirthday_ValueChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.BackColor = Color.Transparent;
            label8.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label8.Location = new Point(460, 743);
            label8.Name = "label8";
            label8.Size = new Size(107, 32);
            label8.TabIndex = 61;
            label8.Text = "Birthday:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(810, 95);
            label2.Name = "label2";
            label2.Size = new Size(194, 60);
            label2.TabIndex = 66;
            label2.Text = "Archives";
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
            btnNotification.TabIndex = 67;
            btnNotification.UseVisualStyleBackColor = false;
            // 
            // TayudBCapViewArch
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.BCO_677f5122_1ca8_402c_addd_6f3eadad8129;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1511, 948);
            Controls.Add(btnNotification);
            Controls.Add(label2);
            Controls.Add(btnNext);
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
            Controls.Add(txtArchHealthSearch);
            Controls.Add(dgvArchResRec);
            Controls.Add(label1);
            Controls.Add(btnSettings);
            Controls.Add(btnLogout);
            Controls.Add(pictureBox4);
            Controls.Add(btnBCapDash);
            Controls.Add(panel2);
            Name = "TayudBCapViewArch";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "TayudBCapViewArch";
            Load += TayudBCapViewArch_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvArchResRec).EndInit();
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
        private Button btnViewArchive;
        private Button btnViewResAcc;
        private Button btnManageResAcc;
        private Button btnResManageRecord;
        private Button btnResViewRecord;
        private ComboBox cmbGender;
        private ComboBox cmbAge;
        private Label label7;
        private Label label6;
        private Label label5;
        private TextBox txtLastName;
        private Label label4;
        private TextBox txtFirstName;
        private Label label3;
        private TextBox txtArchHealthSearch;
        private DataGridView dgvArchResRec;
        private Button btnNext;
        private Button btnDelete;
        private Button btnRestore;
        private DateTimePicker dtpBirthday;
        private Label label8;
        private Label label2;
        private Button btnNotification;
    }
}