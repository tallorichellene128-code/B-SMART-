namespace BSMART
{
    partial class ResViewHealth
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResViewHealth));
            btnResidentDash = new Button();
            btnResViewHealth = new Button();
            panel1 = new Panel();
            btnServicesAppointment = new Button();
            panel2 = new Panel();
            pictureBox4 = new PictureBox();
            btnLogout = new Button();
            btnSettings = new Button();
            label1 = new Label();
            PResProfile = new Panel();
            lblEmail = new Label();
            lblBarangay = new Label();
            lblAddress = new Label();
            lblAge = new Label();
            lblResidentName = new Label();
            dgvResHealthRec = new DataGridView();
            label2 = new Label();
            btnDownloadResRec = new Button();
            btnNotification = new Button();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            PResProfile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvResHealthRec).BeginInit();
            SuspendLayout();
            // 
            // btnResidentDash
            // 
            btnResidentDash.BackColor = SystemColors.Control;
            btnResidentDash.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnResidentDash.Location = new Point(0, 264);
            btnResidentDash.Name = "btnResidentDash";
            btnResidentDash.Size = new Size(260, 97);
            btnResidentDash.TabIndex = 25;
            btnResidentDash.Text = "Resident Dashboard";
            btnResidentDash.UseVisualStyleBackColor = false;
            btnResidentDash.Click += btnResidentDash_Click;
            // 
            // btnResViewHealth
            // 
            btnResViewHealth.BackColor = SystemColors.Control;
            btnResViewHealth.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnResViewHealth.Location = new Point(0, 413);
            btnResViewHealth.Name = "btnResViewHealth";
            btnResViewHealth.Size = new Size(260, 97);
            btnResViewHealth.TabIndex = 4;
            btnResViewHealth.Text = "View Health Record";
            btnResViewHealth.UseVisualStyleBackColor = false;
            btnResViewHealth.Click += btnResViewHealth_Click;
            // 
            // panel1
            // 
            panel1.BackgroundImage = (Image)resources.GetObject("panel1.BackgroundImage");
            panel1.Controls.Add(btnServicesAppointment);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(btnResViewHealth);
            panel1.Location = new Point(0, -1);
            panel1.Name = "panel1";
            panel1.Size = new Size(260, 951);
            panel1.TabIndex = 27;
            // 
            // btnServicesAppointment
            // 
            btnServicesAppointment.BackColor = SystemColors.Control;
            btnServicesAppointment.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnServicesAppointment.Location = new Point(0, 516);
            btnServicesAppointment.Name = "btnServicesAppointment";
            btnServicesAppointment.Size = new Size(260, 97);
            btnServicesAppointment.TabIndex = 19;
            btnServicesAppointment.Text = "View Services and Book Appointment\r\n";
            btnServicesAppointment.UseVisualStyleBackColor = false;
            btnServicesAppointment.Click += btnServicesAppointment_Click;
            // 
            // panel2
            // 
            panel2.BackColor = Color.Transparent;
            panel2.BackgroundImage = (Image)resources.GetObject("panel2.BackgroundImage");
            panel2.BackgroundImageLayout = ImageLayout.Stretch;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(260, 232);
            panel2.TabIndex = 14;
            // 
            // pictureBox4
            // 
            pictureBox4.BackColor = Color.SkyBlue;
            pictureBox4.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox4.Location = new Point(257, -1);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(1254, 75);
            pictureBox4.TabIndex = 26;
            pictureBox4.TabStop = false;
            // 
            // btnLogout
            // 
            btnLogout.BackColor = Color.SkyBlue;
            btnLogout.BackgroundImage = (Image)resources.GetObject("btnLogout.BackgroundImage");
            btnLogout.BackgroundImageLayout = ImageLayout.Zoom;
            btnLogout.FlatStyle = FlatStyle.Popup;
            btnLogout.Location = new Point(1409, 11);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(68, 48);
            btnLogout.TabIndex = 30;
            btnLogout.UseVisualStyleBackColor = false;
            btnLogout.Click += btnLogout_Click;
            // 
            // btnSettings
            // 
            btnSettings.BackColor = Color.SkyBlue;
            btnSettings.BackgroundImage = (Image)resources.GetObject("btnSettings.BackgroundImage");
            btnSettings.BackgroundImageLayout = ImageLayout.Zoom;
            btnSettings.FlatStyle = FlatStyle.Flat;
            btnSettings.Location = new Point(1335, 11);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(68, 48);
            btnSettings.TabIndex = 29;
            btnSettings.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.SkyBlue;
            label1.Font = new Font("Stencil", 20F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.Location = new Point(773, 18);
            label1.Name = "label1";
            label1.Size = new Size(187, 47);
            label1.TabIndex = 28;
            label1.Text = "B-SMART";
            // 
            // PResProfile
            // 
            PResProfile.BackgroundImage = (Image)resources.GetObject("PResProfile.BackgroundImage");
            PResProfile.Controls.Add(lblEmail);
            PResProfile.Controls.Add(lblBarangay);
            PResProfile.Controls.Add(lblAddress);
            PResProfile.Controls.Add(lblAge);
            PResProfile.Controls.Add(lblResidentName);
            PResProfile.Location = new Point(366, 264);
            PResProfile.Name = "PResProfile";
            PResProfile.Size = new Size(1062, 211);
            PResProfile.TabIndex = 31;
            PResProfile.Paint += PResProfile_Paint;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.BackColor = Color.Transparent;
            lblEmail.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEmail.Location = new Point(501, 138);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(86, 38);
            lblEmail.TabIndex = 22;
            lblEmail.Text = "Email";
            lblEmail.Click += lblEmail_Click;
            // 
            // lblBarangay
            // 
            lblBarangay.AutoSize = true;
            lblBarangay.BackColor = Color.Transparent;
            lblBarangay.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBarangay.Location = new Point(501, 84);
            lblBarangay.Name = "lblBarangay";
            lblBarangay.Size = new Size(136, 38);
            lblBarangay.TabIndex = 21;
            lblBarangay.Text = "Barangay";
            lblBarangay.Click += lblBarangay_Click;
            // 
            // lblAddress
            // 
            lblAddress.AutoSize = true;
            lblAddress.BackColor = Color.Transparent;
            lblAddress.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblAddress.Location = new Point(40, 138);
            lblAddress.Name = "lblAddress";
            lblAddress.Size = new Size(119, 38);
            lblAddress.TabIndex = 20;
            lblAddress.Text = "Address";
            lblAddress.Click += lblAddress_Click;
            // 
            // lblAge
            // 
            lblAge.AutoSize = true;
            lblAge.BackColor = Color.Transparent;
            lblAge.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblAge.Location = new Point(40, 84);
            lblAge.Name = "lblAge";
            lblAge.Size = new Size(68, 38);
            lblAge.TabIndex = 2;
            lblAge.Text = "Age";
            lblAge.Click += lblAge_Click;
            // 
            // lblResidentName
            // 
            lblResidentName.AutoSize = true;
            lblResidentName.BackColor = Color.Transparent;
            lblResidentName.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblResidentName.Location = new Point(40, 27);
            lblResidentName.Name = "lblResidentName";
            lblResidentName.Size = new Size(215, 38);
            lblResidentName.TabIndex = 1;
            lblResidentName.Text = "Resident Name";
            lblResidentName.Click += lblResidentName_Click;
            // 
            // dgvResHealthRec
            // 
            dgvResHealthRec.BackgroundColor = SystemColors.GradientInactiveCaption;
            dgvResHealthRec.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvResHealthRec.Location = new Point(366, 549);
            dgvResHealthRec.Name = "dgvResHealthRec";
            dgvResHealthRec.RowHeadersWidth = 62;
            dgvResHealthRec.Size = new Size(1062, 225);
            dgvResHealthRec.TabIndex = 32;
            dgvResHealthRec.CellContentClick += dgvResHealthRec_CellContentClick;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(705, 133);
            label2.Name = "label2";
            label2.Size = new Size(347, 60);
            label2.TabIndex = 54;
            label2.Text = "View My Record";
            // 
            // btnDownloadResRec
            // 
            btnDownloadResRec.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDownloadResRec.Location = new Point(1171, 507);
            btnDownloadResRec.Name = "btnDownloadResRec";
            btnDownloadResRec.Size = new Size(257, 36);
            btnDownloadResRec.TabIndex = 55;
            btnDownloadResRec.Text = "Download Records(PDF)";
            btnDownloadResRec.UseVisualStyleBackColor = true;
            // 
            // btnNotification
            // 
            btnNotification.BackColor = Color.SkyBlue;
            btnNotification.BackgroundImage = Properties.Resources.alert;
            btnNotification.BackgroundImageLayout = ImageLayout.Zoom;
            btnNotification.FlatStyle = FlatStyle.Popup;
            btnNotification.ForeColor = Color.White;
            btnNotification.Location = new Point(1261, 11);
            btnNotification.Name = "btnNotification";
            btnNotification.Size = new Size(68, 48);
            btnNotification.TabIndex = 56;
            btnNotification.UseVisualStyleBackColor = false;
            // 
            // ResViewHealth
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.BCO_677f5122_1ca8_402c_addd_6f3eadad8129;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1511, 948);
            Controls.Add(btnNotification);
            Controls.Add(btnDownloadResRec);
            Controls.Add(label2);
            Controls.Add(dgvResHealthRec);
            Controls.Add(PResProfile);
            Controls.Add(btnResidentDash);
            Controls.Add(panel1);
            Controls.Add(btnLogout);
            Controls.Add(btnSettings);
            Controls.Add(label1);
            Controls.Add(pictureBox4);
            Name = "ResViewHealth";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ResViewHealth";
            Load += ResViewHealth_Load_1;
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            PResProfile.ResumeLayout(false);
            PResProfile.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvResHealthRec).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnResidentDash;
        private Button btnResViewHealth;
        private Panel panel1;
        private Button btnServicesAppointment;
        private Panel panel2;
        private PictureBox pictureBox4;
        private Button btnLogout;
        private Button btnSettings;
        private Label label1;
        private Panel PResProfile;
        private Label lblAddress;
        private Label lblAge;
        private Label lblResidentName;
        private DataGridView dgvResHealthRec;
        private Label lblEmail;
        private Label lblBarangay;
        private Label label2;
        private Button btnDownloadResRec;
        private Button btnNotification;
    }
}