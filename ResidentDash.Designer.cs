namespace BSMART
{
    partial class ResidentDash
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResidentDash));
            btnResidentDash = new Button();
            btnResViewHealth = new Button();
            pictureBox4 = new PictureBox();
            panel1 = new Panel();
            btnServicesAppointment = new Button();
            panel2 = new Panel();
            label1 = new Label();
            btnSettings = new Button();
            btnLogout = new Button();
            PResProfile = new Panel();
            lblEmail = new Label();
            lblResidentName = new Label();
            lblBarangay = new Label();
            lblAge = new Label();
            lblAddress = new Label();
            label2 = new Label();
            btnNotification = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            panel1.SuspendLayout();
            PResProfile.SuspendLayout();
            SuspendLayout();
            // 
            // btnResidentDash
            // 
            btnResidentDash.BackColor = SystemColors.Control;
            btnResidentDash.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnResidentDash.Location = new Point(0, 265);
            btnResidentDash.Name = "btnResidentDash";
            btnResidentDash.Size = new Size(260, 97);
            btnResidentDash.TabIndex = 3;
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
            // pictureBox4
            // 
            pictureBox4.BackColor = Color.SkyBlue;
            pictureBox4.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox4.Location = new Point(257, 0);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(1254, 75);
            pictureBox4.TabIndex = 12;
            pictureBox4.TabStop = false;
            // 
            // panel1
            // 
            panel1.BackgroundImage = (Image)resources.GetObject("panel1.BackgroundImage");
            panel1.Controls.Add(btnServicesAppointment);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(btnResViewHealth);
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(260, 947);
            panel1.TabIndex = 13;
            // 
            // btnServicesAppointment
            // 
            btnServicesAppointment.BackColor = SystemColors.Control;
            btnServicesAppointment.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnServicesAppointment.Location = new Point(0, 516);
            btnServicesAppointment.Name = "btnServicesAppointment";
            btnServicesAppointment.Size = new Size(260, 97);
            btnServicesAppointment.TabIndex = 19;
            btnServicesAppointment.Text = "View Services and Book Appointment";
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
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.SkyBlue;
            label1.Font = new Font("Stencil", 20F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.Location = new Point(773, 19);
            label1.Name = "label1";
            label1.Size = new Size(187, 47);
            label1.TabIndex = 15;
            label1.Text = "B-SMART";
            // 
            // btnSettings
            // 
            btnSettings.BackColor = Color.SkyBlue;
            btnSettings.BackgroundImage = (Image)resources.GetObject("btnSettings.BackgroundImage");
            btnSettings.BackgroundImageLayout = ImageLayout.Zoom;
            btnSettings.FlatStyle = FlatStyle.Flat;
            btnSettings.Location = new Point(1335, 12);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(68, 48);
            btnSettings.TabIndex = 16;
            btnSettings.UseVisualStyleBackColor = false;
            // 
            // btnLogout
            // 
            btnLogout.BackColor = Color.SkyBlue;
            btnLogout.BackgroundImage = (Image)resources.GetObject("btnLogout.BackgroundImage");
            btnLogout.BackgroundImageLayout = ImageLayout.Zoom;
            btnLogout.FlatStyle = FlatStyle.Popup;
            btnLogout.Location = new Point(1409, 12);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(68, 48);
            btnLogout.TabIndex = 17;
            btnLogout.UseVisualStyleBackColor = false;
            btnLogout.Click += btnLogout_Click;
            // 
            // PResProfile
            // 
            PResProfile.BackgroundImage = (Image)resources.GetObject("PResProfile.BackgroundImage");
            PResProfile.Controls.Add(lblEmail);
            PResProfile.Controls.Add(lblResidentName);
            PResProfile.Controls.Add(lblBarangay);
            PResProfile.Controls.Add(lblAge);
            PResProfile.Controls.Add(lblAddress);
            PResProfile.Location = new Point(366, 375);
            PResProfile.Name = "PResProfile";
            PResProfile.Size = new Size(1062, 211);
            PResProfile.TabIndex = 18;
            PResProfile.Paint += PResProfile_Paint;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.BackColor = Color.Transparent;
            lblEmail.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEmail.Location = new Point(543, 141);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(86, 38);
            lblEmail.TabIndex = 27;
            lblEmail.Text = "Email";
            lblEmail.Click += lblEmail_Click;
            // 
            // lblResidentName
            // 
            lblResidentName.AutoSize = true;
            lblResidentName.BackColor = Color.Transparent;
            lblResidentName.Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblResidentName.Location = new Point(91, 26);
            lblResidentName.Name = "lblResidentName";
            lblResidentName.Size = new Size(215, 38);
            lblResidentName.TabIndex = 23;
            lblResidentName.Text = "Resident Name";
            lblResidentName.Click += lblResidentName_Click;
            // 
            // lblBarangay
            // 
            lblBarangay.AutoSize = true;
            lblBarangay.BackColor = Color.Transparent;
            lblBarangay.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBarangay.Location = new Point(543, 82);
            lblBarangay.Name = "lblBarangay";
            lblBarangay.Size = new Size(136, 38);
            lblBarangay.TabIndex = 26;
            lblBarangay.Text = "Barangay";
            lblBarangay.Click += lblBarangay_Click;
            // 
            // lblAge
            // 
            lblAge.AutoSize = true;
            lblAge.BackColor = Color.Transparent;
            lblAge.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblAge.Location = new Point(91, 82);
            lblAge.Name = "lblAge";
            lblAge.Size = new Size(68, 38);
            lblAge.TabIndex = 24;
            lblAge.Text = "Age";
            lblAge.Click += lblAge_Click;
            // 
            // lblAddress
            // 
            lblAddress.AutoSize = true;
            lblAddress.BackColor = Color.Transparent;
            lblAddress.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblAddress.Location = new Point(91, 136);
            lblAddress.Name = "lblAddress";
            lblAddress.Size = new Size(119, 38);
            lblAddress.TabIndex = 25;
            lblAddress.Text = "Address";
            lblAddress.Click += lblAddress_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(366, 134);
            label2.Name = "label2";
            label2.Size = new Size(692, 228);
            label2.TabIndex = 19;
            label2.Text = resources.GetString("label2.Text");
            // 
            // btnNotification
            // 
            btnNotification.BackColor = Color.SkyBlue;
            btnNotification.BackgroundImage = Properties.Resources.alert;
            btnNotification.BackgroundImageLayout = ImageLayout.Zoom;
            btnNotification.FlatStyle = FlatStyle.Popup;
            btnNotification.ForeColor = Color.White;
            btnNotification.Location = new Point(1261, 12);
            btnNotification.Name = "btnNotification";
            btnNotification.Size = new Size(68, 48);
            btnNotification.TabIndex = 55;
            btnNotification.UseVisualStyleBackColor = false;
            // 
            // ResidentDash
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(1511, 948);
            Controls.Add(btnNotification);
            Controls.Add(label2);
            Controls.Add(btnLogout);
            Controls.Add(btnSettings);
            Controls.Add(label1);
            Controls.Add(pictureBox4);
            Controls.Add(btnResidentDash);
            Controls.Add(panel1);
            Controls.Add(PResProfile);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "ResidentDash";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ResidentDash";
            Load += ResidentDash_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            panel1.ResumeLayout(false);
            PResProfile.ResumeLayout(false);
            PResProfile.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button btnResidentDash;
        private Button btnResViewHealth;
        private PictureBox pictureBox4;
        private Panel panel1;
        private Panel panel2;
        private Label label1;
        private Button btnSettings;
        private Button btnLogout;
        private Panel PResProfile;
        private Button button2;
        private Button btnServicesAppointment;
        private Label label2;
        private Label lblEmail;
        private Label lblResidentName;
        private Label lblBarangay;
        private Label lblAge;
        private Label lblAddress;
        private Button btnNotification;
    }
}