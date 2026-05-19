namespace BSMART
{
    partial class ResidentAppointment
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResidentAppointment));
            btnResidentDash = new Button();
            btnResViewHealth = new Button();
            pictureBox4 = new PictureBox();
            panel1 = new Panel();
            btnServicesAppointment = new Button();
            panel2 = new Panel();
            btnLogout = new Button();
            btnSettings = new Button();
            label1 = new Label();
            flpAvailHealthService = new FlowLayoutPanel();
            label2 = new Label();
            PBooking = new Panel();
            lblCreateApptFor = new Label();
            linklblViewBookings = new LinkLabel();
            btnBookApp = new Button();
            txtNotesSymptoms = new TextBox();
            label6 = new Label();
            flpTimeSlots = new FlowLayoutPanel();
            btn9AM = new Button();
            btn930AM = new Button();
            btn10AM = new Button();
            btn1030AM = new Button();
            btn11AM = new Button();
            btn130PM = new Button();
            btn2PM = new Button();
            btn230PM = new Button();
            btn3PM = new Button();
            btn330PM = new Button();
            btn4PM = new Button();
            btn430PM = new Button();
            label5 = new Label();
            dtpPreferredDate = new DateTimePicker();
            label4 = new Label();
            lblBookAppTitle = new Label();
            txtSearch = new TextBox();
            pictureBox1 = new PictureBox();
            btnNotification = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            panel1.SuspendLayout();
            PBooking.SuspendLayout();
            flpTimeSlots.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // btnResidentDash
            // 
            btnResidentDash.BackColor = SystemColors.Control;
            btnResidentDash.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnResidentDash.Location = new Point(1, 266);
            btnResidentDash.Name = "btnResidentDash";
            btnResidentDash.Size = new Size(260, 97);
            btnResidentDash.TabIndex = 19;
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
            pictureBox4.Location = new Point(258, 1);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(1254, 75);
            pictureBox4.TabIndex = 20;
            pictureBox4.TabStop = false;
            // 
            // panel1
            // 
            panel1.BackgroundImage = (Image)resources.GetObject("panel1.BackgroundImage");
            panel1.Controls.Add(btnServicesAppointment);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(btnResViewHealth);
            panel1.Location = new Point(1, 1);
            panel1.Name = "panel1";
            panel1.Size = new Size(260, 951);
            panel1.TabIndex = 21;
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
            // btnLogout
            // 
            btnLogout.BackColor = Color.SkyBlue;
            btnLogout.BackgroundImage = (Image)resources.GetObject("btnLogout.BackgroundImage");
            btnLogout.BackgroundImageLayout = ImageLayout.Zoom;
            btnLogout.FlatStyle = FlatStyle.Popup;
            btnLogout.Location = new Point(1410, 13);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(68, 48);
            btnLogout.TabIndex = 24;
            btnLogout.UseVisualStyleBackColor = false;
            btnLogout.Click += btnLogout_Click;
            // 
            // btnSettings
            // 
            btnSettings.BackColor = Color.SkyBlue;
            btnSettings.BackgroundImage = (Image)resources.GetObject("btnSettings.BackgroundImage");
            btnSettings.BackgroundImageLayout = ImageLayout.Zoom;
            btnSettings.FlatStyle = FlatStyle.Flat;
            btnSettings.Location = new Point(1336, 13);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(68, 48);
            btnSettings.TabIndex = 23;
            btnSettings.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.SkyBlue;
            label1.Font = new Font("Stencil", 20F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.Location = new Point(774, 20);
            label1.Name = "label1";
            label1.Size = new Size(187, 47);
            label1.TabIndex = 22;
            label1.Text = "B-SMART";
            // 
            // flpAvailHealthService
            // 
            flpAvailHealthService.AutoScroll = true;
            flpAvailHealthService.FlowDirection = FlowDirection.TopDown;
            flpAvailHealthService.Location = new Point(314, 266);
            flpAvailHealthService.Name = "flpAvailHealthService";
            flpAvailHealthService.Padding = new Padding(5);
            flpAvailHealthService.Size = new Size(647, 623);
            flpAvailHealthService.TabIndex = 25;
            flpAvailHealthService.WrapContents = false;
            flpAvailHealthService.Paint += flpAvailHealthService_Paint;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = SystemColors.ActiveCaptionText;
            label2.Location = new Point(314, 208);
            label2.Name = "label2";
            label2.Size = new Size(381, 45);
            label2.TabIndex = 26;
            label2.Text = "Available Health Services";
            // 
            // PBooking
            // 
            PBooking.BackColor = Color.White;
            PBooking.Controls.Add(lblCreateApptFor);
            PBooking.Controls.Add(linklblViewBookings);
            PBooking.Controls.Add(btnBookApp);
            PBooking.Controls.Add(txtNotesSymptoms);
            PBooking.Controls.Add(label6);
            PBooking.Controls.Add(flpTimeSlots);
            PBooking.Controls.Add(label5);
            PBooking.Controls.Add(dtpPreferredDate);
            PBooking.Controls.Add(label4);
            PBooking.Location = new Point(989, 266);
            PBooking.Name = "PBooking";
            PBooking.Size = new Size(480, 562);
            PBooking.TabIndex = 27;
            PBooking.Paint += PBooking_Paint;
            // 
            // lblCreateApptFor
            // 
            lblCreateApptFor.AutoSize = true;
            lblCreateApptFor.BackColor = Color.Transparent;
            lblCreateApptFor.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblCreateApptFor.Location = new Point(7, 9);
            lblCreateApptFor.Name = "lblCreateApptFor";
            lblCreateApptFor.Size = new Size(0, 30);
            lblCreateApptFor.TabIndex = 9;
            // 
            // linklblViewBookings
            // 
            linklblViewBookings.AutoSize = true;
            linklblViewBookings.Location = new Point(143, 530);
            linklblViewBookings.Name = "linklblViewBookings";
            linklblViewBookings.Size = new Size(183, 25);
            linklblViewBookings.TabIndex = 8;
            linklblViewBookings.TabStop = true;
            linklblViewBookings.Text = "View All My Bookings";
            linklblViewBookings.LinkClicked += linklblViewBookings_LinkClicked;
            // 
            // btnBookApp
            // 
            btnBookApp.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnBookApp.Location = new Point(6, 422);
            btnBookApp.Name = "btnBookApp";
            btnBookApp.Size = new Size(466, 37);
            btnBookApp.TabIndex = 6;
            btnBookApp.Text = "Book Appointment";
            btnBookApp.UseVisualStyleBackColor = true;
            btnBookApp.Click += btnBookApp_Click;
            // 
            // txtNotesSymptoms
            // 
            txtNotesSymptoms.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtNotesSymptoms.Location = new Point(6, 311);
            txtNotesSymptoms.Multiline = true;
            txtNotesSymptoms.Name = "txtNotesSymptoms";
            txtNotesSymptoms.Size = new Size(466, 92);
            txtNotesSymptoms.TabIndex = 5;
            txtNotesSymptoms.TextChanged += txtNotesSymptoms_TextChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.Transparent;
            label6.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(3, 278);
            label6.Name = "label6";
            label6.Size = new Size(283, 30);
            label6.TabIndex = 4;
            label6.Text = "Notes/Symptoms(Optional):";
            // 
            // flpTimeSlots
            // 
            flpTimeSlots.Controls.Add(btn9AM);
            flpTimeSlots.Controls.Add(btn930AM);
            flpTimeSlots.Controls.Add(btn10AM);
            flpTimeSlots.Controls.Add(btn1030AM);
            flpTimeSlots.Controls.Add(btn11AM);
            flpTimeSlots.Controls.Add(btn130PM);
            flpTimeSlots.Controls.Add(btn2PM);
            flpTimeSlots.Controls.Add(btn230PM);
            flpTimeSlots.Controls.Add(btn3PM);
            flpTimeSlots.Controls.Add(btn330PM);
            flpTimeSlots.Controls.Add(btn4PM);
            flpTimeSlots.Controls.Add(btn430PM);
            flpTimeSlots.Location = new Point(3, 131);
            flpTimeSlots.Name = "flpTimeSlots";
            flpTimeSlots.Size = new Size(473, 130);
            flpTimeSlots.TabIndex = 3;
            flpTimeSlots.Paint += flpTimeSlots_Paint;
            // 
            // btn9AM
            // 
            btn9AM.Location = new Point(3, 3);
            btn9AM.Name = "btn9AM";
            btn9AM.Size = new Size(112, 34);
            btn9AM.TabIndex = 0;
            btn9AM.Text = "9:00 AM";
            btn9AM.UseVisualStyleBackColor = true;
            btn9AM.Click += btn9AM_Click;
            // 
            // btn930AM
            // 
            btn930AM.Location = new Point(121, 3);
            btn930AM.Name = "btn930AM";
            btn930AM.Size = new Size(112, 34);
            btn930AM.TabIndex = 1;
            btn930AM.Text = "9:30 AM";
            btn930AM.UseVisualStyleBackColor = true;
            btn930AM.Click += btn930AM_Click;
            // 
            // btn10AM
            // 
            btn10AM.Location = new Point(239, 3);
            btn10AM.Name = "btn10AM";
            btn10AM.Size = new Size(112, 34);
            btn10AM.TabIndex = 2;
            btn10AM.Text = "10:00 AM";
            btn10AM.UseVisualStyleBackColor = true;
            btn10AM.Click += btn10AM_Click;
            // 
            // btn1030AM
            // 
            btn1030AM.Location = new Point(357, 3);
            btn1030AM.Name = "btn1030AM";
            btn1030AM.Size = new Size(112, 34);
            btn1030AM.TabIndex = 3;
            btn1030AM.Text = "10:30 AM";
            btn1030AM.UseVisualStyleBackColor = true;
            btn1030AM.Click += btn1030AM_Click;
            // 
            // btn11AM
            // 
            btn11AM.Location = new Point(3, 43);
            btn11AM.Name = "btn11AM";
            btn11AM.Size = new Size(112, 34);
            btn11AM.TabIndex = 4;
            btn11AM.Text = "11:00 AM";
            btn11AM.UseVisualStyleBackColor = true;
            btn11AM.Click += btn11AM_Click;
            // 
            // btn130PM
            // 
            btn130PM.Location = new Point(121, 43);
            btn130PM.Name = "btn130PM";
            btn130PM.Size = new Size(112, 34);
            btn130PM.TabIndex = 5;
            btn130PM.Text = "1:30 PM";
            btn130PM.UseVisualStyleBackColor = true;
            btn130PM.Click += btn130PM_Click;
            // 
            // btn2PM
            // 
            btn2PM.Location = new Point(239, 43);
            btn2PM.Name = "btn2PM";
            btn2PM.Size = new Size(112, 34);
            btn2PM.TabIndex = 6;
            btn2PM.Text = "2:00 PM";
            btn2PM.UseVisualStyleBackColor = true;
            btn2PM.Click += btn2PM_Click;
            // 
            // btn230PM
            // 
            btn230PM.Location = new Point(357, 43);
            btn230PM.Name = "btn230PM";
            btn230PM.Size = new Size(112, 34);
            btn230PM.TabIndex = 7;
            btn230PM.Text = "2:30 PM";
            btn230PM.UseVisualStyleBackColor = true;
            btn230PM.Click += btn230PM_Click;
            // 
            // btn3PM
            // 
            btn3PM.Location = new Point(3, 83);
            btn3PM.Name = "btn3PM";
            btn3PM.Size = new Size(112, 34);
            btn3PM.TabIndex = 8;
            btn3PM.Text = "3:00 PM";
            btn3PM.UseVisualStyleBackColor = true;
            btn3PM.Click += btn3PM_Click;
            // 
            // btn330PM
            // 
            btn330PM.Location = new Point(121, 83);
            btn330PM.Name = "btn330PM";
            btn330PM.Size = new Size(112, 34);
            btn330PM.TabIndex = 9;
            btn330PM.Text = "3:30 PM";
            btn330PM.UseVisualStyleBackColor = true;
            btn330PM.Click += btn330PM_Click;
            // 
            // btn4PM
            // 
            btn4PM.Location = new Point(239, 83);
            btn4PM.Name = "btn4PM";
            btn4PM.Size = new Size(112, 34);
            btn4PM.TabIndex = 10;
            btn4PM.Text = "4:00 PM";
            btn4PM.UseVisualStyleBackColor = true;
            btn4PM.Click += btn4PM_Click;
            // 
            // btn430PM
            // 
            btn430PM.Location = new Point(357, 83);
            btn430PM.Name = "btn430PM";
            btn430PM.Size = new Size(112, 34);
            btn430PM.TabIndex = 11;
            btn430PM.Text = "4:30 PM";
            btn430PM.UseVisualStyleBackColor = true;
            btn430PM.Click += btn430PM_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(3, 98);
            label5.Name = "label5";
            label5.Size = new Size(211, 30);
            label5.TabIndex = 2;
            label5.Text = "Available Time Slots:";
            // 
            // dtpPreferredDate
            // 
            dtpPreferredDate.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpPreferredDate.Location = new Point(3, 42);
            dtpPreferredDate.Name = "dtpPreferredDate";
            dtpPreferredDate.Size = new Size(473, 37);
            dtpPreferredDate.TabIndex = 1;
            dtpPreferredDate.ValueChanged += dtpPreferredDate_ValueChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(3, 9);
            label4.Name = "label4";
            label4.Size = new Size(161, 30);
            label4.TabIndex = 0;
            label4.Text = "Preferred Date:";
            // 
            // lblBookAppTitle
            // 
            lblBookAppTitle.AutoSize = true;
            lblBookAppTitle.BackColor = Color.Transparent;
            lblBookAppTitle.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBookAppTitle.ForeColor = SystemColors.ActiveCaptionText;
            lblBookAppTitle.Location = new Point(1038, 208);
            lblBookAppTitle.Name = "lblBookAppTitle";
            lblBookAppTitle.Size = new Size(296, 45);
            lblBookAppTitle.TabIndex = 28;
            lblBookAppTitle.Text = "Book Appointment";
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSearch.Location = new Point(314, 150);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(597, 39);
            txtSearch.TabIndex = 29;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.BackgroundImage = (Image)resources.GetObject("pictureBox1.BackgroundImage");
            pictureBox1.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox1.Location = new Point(910, 150);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(51, 39);
            pictureBox1.TabIndex = 30;
            pictureBox1.TabStop = false;
            // 
            // btnNotification
            // 
            btnNotification.BackColor = Color.SkyBlue;
            btnNotification.BackgroundImage = Properties.Resources.alert;
            btnNotification.BackgroundImageLayout = ImageLayout.Zoom;
            btnNotification.FlatStyle = FlatStyle.Popup;
            btnNotification.ForeColor = Color.White;
            btnNotification.Location = new Point(1262, 13);
            btnNotification.Name = "btnNotification";
            btnNotification.Size = new Size(68, 48);
            btnNotification.TabIndex = 55;
            btnNotification.UseVisualStyleBackColor = false;
            // 
            // ResidentAppointment
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.BCO_677f5122_1ca8_402c_addd_6f3eadad8129;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1511, 948);
            Controls.Add(btnNotification);
            Controls.Add(pictureBox1);
            Controls.Add(txtSearch);
            Controls.Add(lblBookAppTitle);
            Controls.Add(PBooking);
            Controls.Add(label2);
            Controls.Add(flpAvailHealthService);
            Controls.Add(btnResidentDash);
            Controls.Add(panel1);
            Controls.Add(btnLogout);
            Controls.Add(btnSettings);
            Controls.Add(label1);
            Controls.Add(pictureBox4);
            Name = "ResidentAppointment";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ResidentAppointment";
            TopMost = true;
            Load += ResidentAppointment_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            panel1.ResumeLayout(false);
            PBooking.ResumeLayout(false);
            PBooking.PerformLayout();
            flpTimeSlots.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnResidentDash;
        private Button btnResViewHealth;
        private PictureBox pictureBox4;
        private Panel panel1;
        private Button btn10AM;
        private Button btnServicesAppointment;
        private Panel panel2;
        private Button btnLogout;
        private Button btnSettings;
        private Label label1;
        private FlowLayoutPanel flpAvailHealthService;
        private Label label2;
        private Panel PBooking;
        private Label lblBookAppTitle;
        private DateTimePicker dtpPreferredDate;
        private Label label4;
        private TextBox txtSearch;
        private FlowLayoutPanel flpTimeSlots;
        private Button btn9AM;
        private Label label5;
        private Button btn930AM;
        private Button btn1030AM;
        private Button btn11AM;
        private Button btn130PM;
        private Button btn2PM;
        private Button btn230PM;
        private TextBox txtNotesSymptoms;
        private Label label6;
        private Button btn3PM;
        private Button btn330PM;
        private Button btn4PM;
        private Button btn430PM;
        private LinkLabel linklblViewBookings;
        private Button btnBookApp;
        private PictureBox pictureBox1;
        private Label lblCreateApptFor;
        private Button btnNotification;
    }
}