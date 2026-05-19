namespace BSMART
{
    partial class TayudBCViewRes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TayudBCViewRes));
            panel1 = new Panel();
            label1 = new Label();
            btnSettings = new Button();
            btnLogout = new Button();
            pictureBox4 = new PictureBox();
            btnBCapDash = new Button();
            panel2 = new Panel();
            btnHealthRep = new Button();
            btnManageFrozenAcc = new Button();
            btnViewResAcc = new Button();
            btnViewArchive = new Button();
            btnResManageRecord = new Button();
            btnManageResAcc = new Button();
            btnResViewRecord = new Button();
            dgvResViewRecord = new DataGridView();
            txtSearch = new TextBox();
            label2 = new Label();
            btnNotification = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvResViewRecord).BeginInit();
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
            btnBCapDash.BackgroundImageLayout = ImageLayout.Stretch;
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
            panel2.Controls.Add(btnHealthRep);
            panel2.Controls.Add(btnManageFrozenAcc);
            panel2.Controls.Add(panel1);
            panel2.Controls.Add(btnViewResAcc);
            panel2.Controls.Add(btnViewArchive);
            panel2.Controls.Add(btnResManageRecord);
            panel2.Controls.Add(btnManageResAcc);
            panel2.Controls.Add(btnResViewRecord);
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(260, 949);
            panel2.TabIndex = 43;
            // 
            // btnHealthRep
            // 
            btnHealthRep.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnHealthRep.Location = new Point(0, 751);
            btnHealthRep.Name = "btnHealthRep";
            btnHealthRep.Size = new Size(260, 74);
            btnHealthRep.TabIndex = 59;
            btnHealthRep.Text = "View Residents Health Report";
            btnHealthRep.UseVisualStyleBackColor = true;
            btnHealthRep.Click += btnHealthRep_Click;
            // 
            // btnManageFrozenAcc
            // 
            btnManageFrozenAcc.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnManageFrozenAcc.Location = new Point(0, 671);
            btnManageFrozenAcc.Name = "btnManageFrozenAcc";
            btnManageFrozenAcc.Size = new Size(260, 74);
            btnManageFrozenAcc.TabIndex = 58;
            btnManageFrozenAcc.Text = "Manage Frozen Accounts";
            btnManageFrozenAcc.UseVisualStyleBackColor = true;
            btnManageFrozenAcc.Click += btnManageFrozenAcc_Click;
            // 
            // btnViewResAcc
            // 
            btnViewResAcc.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnViewResAcc.Location = new Point(0, 591);
            btnViewResAcc.Name = "btnViewResAcc";
            btnViewResAcc.Size = new Size(260, 74);
            btnViewResAcc.TabIndex = 57;
            btnViewResAcc.Text = "View Resident Accounts";
            btnViewResAcc.UseVisualStyleBackColor = true;
            btnViewResAcc.Click += btnViewResAcc_Click;
            // 
            // btnViewArchive
            // 
            btnViewArchive.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnViewArchive.Location = new Point(0, 831);
            btnViewArchive.Name = "btnViewArchive";
            btnViewArchive.Size = new Size(260, 74);
            btnViewArchive.TabIndex = 56;
            btnViewArchive.Text = "View Archive";
            btnViewArchive.UseVisualStyleBackColor = true;
            btnViewArchive.Click += btnViewArchive_Click;
            // 
            // btnResManageRecord
            // 
            btnResManageRecord.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnResManageRecord.Location = new Point(0, 351);
            btnResManageRecord.Name = "btnResManageRecord";
            btnResManageRecord.Size = new Size(260, 74);
            btnResManageRecord.TabIndex = 53;
            btnResManageRecord.Text = "Manage Resident Record";
            btnResManageRecord.UseVisualStyleBackColor = true;
            btnResManageRecord.Click += btnResManageRecord_Click;
            // 
            // btnManageResAcc
            // 
            btnManageResAcc.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnManageResAcc.Location = new Point(0, 511);
            btnManageResAcc.Name = "btnManageResAcc";
            btnManageResAcc.Size = new Size(260, 74);
            btnManageResAcc.TabIndex = 55;
            btnManageResAcc.Text = "Manage Resident Accounts";
            btnManageResAcc.UseVisualStyleBackColor = true;
            btnManageResAcc.Click += btnManageResAcc_Click;
            // 
            // btnResViewRecord
            // 
            btnResViewRecord.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnResViewRecord.Location = new Point(0, 431);
            btnResViewRecord.Name = "btnResViewRecord";
            btnResViewRecord.Size = new Size(260, 74);
            btnResViewRecord.TabIndex = 54;
            btnResViewRecord.Text = "View Resident Record\r\n";
            btnResViewRecord.UseVisualStyleBackColor = true;
            btnResViewRecord.Click += btnResViewRecord_Click;
            // 
            // dgvResViewRecord
            // 
            dgvResViewRecord.BackgroundColor = SystemColors.GradientActiveCaption;
            dgvResViewRecord.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvResViewRecord.Location = new Point(349, 334);
            dgvResViewRecord.Name = "dgvResViewRecord";
            dgvResViewRecord.RowHeadersWidth = 62;
            dgvResViewRecord.Size = new Size(1066, 481);
            dgvResViewRecord.TabIndex = 52;
            dgvResViewRecord.CellContentClick += dgvResViewRecord_CellContentClick;
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSearch.Location = new Point(445, 263);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(899, 45);
            txtSearch.TabIndex = 51;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(661, 138);
            label2.Name = "label2";
            label2.Size = new Size(456, 60);
            label2.TabIndex = 50;
            label2.Text = "View Resident Record";
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
            btnNotification.TabIndex = 55;
            btnNotification.UseVisualStyleBackColor = false;
            // 
            // TayudBCViewRes
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.BCO_677f5122_1ca8_402c_addd_6f3eadad8129;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1511, 948);
            Controls.Add(btnNotification);
            Controls.Add(dgvResViewRecord);
            Controls.Add(txtSearch);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnSettings);
            Controls.Add(btnLogout);
            Controls.Add(pictureBox4);
            Controls.Add(btnBCapDash);
            Controls.Add(panel2);
            Name = "TayudBCViewRes";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ViewResident";
            Load += TayudBCViewRes_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvResViewRecord).EndInit();
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
        private DataGridView dgvResViewRecord;
        private TextBox txtSearch;
        private Label label2;
        private Button btnHealthRep;
        private Button btnManageFrozenAcc;
        private Button btnViewResAcc;
        private Button btnViewArchive;
        private Button btnResManageRecord;
        private Button btnManageResAcc;
        private Button btnResViewRecord;
        private Button btnNotification;
    }
}