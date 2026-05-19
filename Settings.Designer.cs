namespace BSMART
{
    partial class Settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            btnNotification = new Button();
            label1 = new Label();
            btnHome = new Button();
            btnLogout = new Button();
            btnProfile = new Button();
            pictureBox4 = new PictureBox();
            panel1 = new Panel();
            panel2 = new Panel();
            btnPassword = new Button();
            label2 = new Label();
            label3 = new Label();
            txtFirstName = new TextBox();
            txtMiddleName = new TextBox();
            label4 = new Label();
            txtEmailAdd = new TextBox();
            label5 = new Label();
            txtMobileNo = new TextBox();
            label6 = new Label();
            txtLastName = new TextBox();
            label7 = new Label();
            dtpBirthdate = new DateTimePicker();
            label8 = new Label();
            txtAddress = new TextBox();
            label9 = new Label();
            btnSaveChanges = new Button();
            btnCancel = new Button();
            label10 = new Label();
            txtUsername = new TextBox();
            label11 = new Label();
            txtReligion = new TextBox();
            label12 = new Label();
            txtCitizenship = new TextBox();
            label13 = new Label();
            comboBox1 = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
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
            btnNotification.TabIndex = 57;
            btnNotification.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.SkyBlue;
            label1.Font = new Font("Stencil", 20F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.Location = new Point(768, 13);
            label1.Name = "label1";
            label1.Size = new Size(187, 47);
            label1.TabIndex = 53;
            label1.Text = "B-SMART";
            // 
            // btnHome
            // 
            btnHome.BackColor = Color.SkyBlue;
            btnHome.BackgroundImage = (Image)resources.GetObject("btnHome.BackgroundImage");
            btnHome.BackgroundImageLayout = ImageLayout.Zoom;
            btnHome.FlatStyle = FlatStyle.Flat;
            btnHome.Location = new Point(1357, 12);
            btnHome.Name = "btnHome";
            btnHome.Size = new Size(68, 48);
            btnHome.TabIndex = 52;
            btnHome.UseVisualStyleBackColor = false;
            btnHome.Click += btnHome_Click;
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
            btnLogout.TabIndex = 51;
            btnLogout.UseVisualStyleBackColor = false;
            // 
            // btnProfile
            // 
            btnProfile.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnProfile.Location = new Point(0, 229);
            btnProfile.Name = "btnProfile";
            btnProfile.Size = new Size(260, 81);
            btnProfile.TabIndex = 49;
            btnProfile.Text = "Profile and Account Details";
            btnProfile.UseVisualStyleBackColor = true;
            btnProfile.Click += btnProfile_Click;
            // 
            // pictureBox4
            // 
            pictureBox4.BackColor = Color.SkyBlue;
            pictureBox4.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox4.Location = new Point(259, 0);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(1252, 75);
            pictureBox4.TabIndex = 50;
            pictureBox4.TabStop = false;
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
            // panel2
            // 
            panel2.BackgroundImage = (Image)resources.GetObject("panel2.BackgroundImage");
            panel2.Controls.Add(btnPassword);
            panel2.Controls.Add(panel1);
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(260, 949);
            panel2.TabIndex = 48;
            // 
            // btnPassword
            // 
            btnPassword.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnPassword.Location = new Point(0, 332);
            btnPassword.Name = "btnPassword";
            btnPassword.Size = new Size(260, 81);
            btnPassword.TabIndex = 58;
            btnPassword.Text = "Password and Security";
            btnPassword.UseVisualStyleBackColor = true;
            btnPassword.Click += btnPassword_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(311, 120);
            label2.Name = "label2";
            label2.Size = new Size(511, 54);
            label2.TabIndex = 58;
            label2.Text = "Profile and Account Details";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(311, 193);
            label3.Name = "label3";
            label3.Size = new Size(134, 32);
            label3.TabIndex = 59;
            label3.Text = "First Name:";
            // 
            // txtFirstName
            // 
            txtFirstName.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtFirstName.Location = new Point(311, 229);
            txtFirstName.Name = "txtFirstName";
            txtFirstName.Size = new Size(459, 45);
            txtFirstName.TabIndex = 60;
            txtFirstName.TextChanged += txtFirstName_TextChanged;
            // 
            // txtMiddleName
            // 
            txtMiddleName.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtMiddleName.Location = new Point(311, 332);
            txtMiddleName.Name = "txtMiddleName";
            txtMiddleName.Size = new Size(459, 45);
            txtMiddleName.TabIndex = 62;
            txtMiddleName.TextChanged += txtMiddleName_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(311, 296);
            label4.Name = "label4";
            label4.Size = new Size(165, 32);
            label4.TabIndex = 61;
            label4.Text = "Middle Name:";
            // 
            // txtEmailAdd
            // 
            txtEmailAdd.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtEmailAdd.Location = new Point(982, 229);
            txtEmailAdd.Name = "txtEmailAdd";
            txtEmailAdd.Size = new Size(443, 45);
            txtEmailAdd.TabIndex = 64;
            txtEmailAdd.TextChanged += txtEmailAdd_TextChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(982, 193);
            label5.Name = "label5";
            label5.Size = new Size(167, 32);
            label5.TabIndex = 63;
            label5.Text = "Email Address:";
            // 
            // txtMobileNo
            // 
            txtMobileNo.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtMobileNo.Location = new Point(982, 341);
            txtMobileNo.Name = "txtMobileNo";
            txtMobileNo.Size = new Size(443, 45);
            txtMobileNo.TabIndex = 66;
            txtMobileNo.TextChanged += txtMobileNo_TextChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.Transparent;
            label6.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(982, 305);
            label6.Name = "label6";
            label6.Size = new Size(138, 32);
            label6.TabIndex = 65;
            label6.Text = "Mobile No.:";
            // 
            // txtLastName
            // 
            txtLastName.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtLastName.Location = new Point(311, 436);
            txtLastName.Name = "txtLastName";
            txtLastName.Size = new Size(459, 45);
            txtLastName.TabIndex = 68;
            txtLastName.TextChanged += txtLastName_TextChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.Transparent;
            label7.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label7.Location = new Point(311, 400);
            label7.Name = "label7";
            label7.Size = new Size(131, 32);
            label7.TabIndex = 67;
            label7.Text = "Last Name:";
            // 
            // dtpBirthdate
            // 
            dtpBirthdate.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpBirthdate.Location = new Point(984, 535);
            dtpBirthdate.Name = "dtpBirthdate";
            dtpBirthdate.Size = new Size(441, 45);
            dtpBirthdate.TabIndex = 69;
            dtpBirthdate.ValueChanged += dtpBirthdate_ValueChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.BackColor = Color.Transparent;
            label8.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label8.Location = new Point(984, 499);
            label8.Name = "label8";
            label8.Size = new Size(123, 32);
            label8.TabIndex = 70;
            label8.Text = "Birthdate: ";
            // 
            // txtAddress
            // 
            txtAddress.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtAddress.Location = new Point(982, 659);
            txtAddress.Multiline = true;
            txtAddress.Name = "txtAddress";
            txtAddress.Size = new Size(443, 111);
            txtAddress.TabIndex = 71;
            txtAddress.TextChanged += txtAddress_TextChanged;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.BackColor = Color.Transparent;
            label9.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label9.Location = new Point(982, 624);
            label9.Name = "label9";
            label9.Size = new Size(103, 32);
            label9.TabIndex = 72;
            label9.Text = "Address:";
            // 
            // btnSaveChanges
            // 
            btnSaveChanges.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnSaveChanges.Location = new Point(982, 798);
            btnSaveChanges.Name = "btnSaveChanges";
            btnSaveChanges.Size = new Size(443, 49);
            btnSaveChanges.TabIndex = 73;
            btnSaveChanges.Text = "Save Changes";
            btnSaveChanges.UseVisualStyleBackColor = true;
            btnSaveChanges.Click += btnSaveChanges_Click;
            // 
            // btnCancel
            // 
            btnCancel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCancel.Location = new Point(984, 870);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(441, 49);
            btnCancel.TabIndex = 74;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.BackColor = Color.Transparent;
            label10.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label10.Location = new Point(982, 408);
            label10.Name = "label10";
            label10.Size = new Size(126, 32);
            label10.TabIndex = 75;
            label10.Text = "Username:";
            // 
            // txtUsername
            // 
            txtUsername.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtUsername.Location = new Point(982, 444);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(443, 45);
            txtUsername.TabIndex = 76;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.BackColor = Color.Transparent;
            label11.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label11.Location = new Point(311, 714);
            label11.Name = "label11";
            label11.Size = new Size(135, 32);
            label11.TabIndex = 81;
            label11.Text = "Civil Status:";
            // 
            // txtReligion
            // 
            txtReligion.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtReligion.Location = new Point(311, 646);
            txtReligion.Name = "txtReligion";
            txtReligion.Size = new Size(459, 45);
            txtReligion.TabIndex = 80;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.BackColor = Color.Transparent;
            label12.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label12.Location = new Point(311, 610);
            label12.Name = "label12";
            label12.Size = new Size(105, 32);
            label12.TabIndex = 79;
            label12.Text = "Religion:";
            // 
            // txtCitizenship
            // 
            txtCitizenship.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtCitizenship.Location = new Point(311, 543);
            txtCitizenship.Name = "txtCitizenship";
            txtCitizenship.Size = new Size(459, 45);
            txtCitizenship.TabIndex = 78;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.BackColor = Color.Transparent;
            label13.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label13.Location = new Point(311, 507);
            label13.Name = "label13";
            label13.Size = new Size(136, 32);
            label13.TabIndex = 77;
            label13.Text = "Citizenship:";
            // 
            // comboBox1
            // 
            comboBox1.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "Single", "Married", "Widowed", "Divorced", "Legally separated" });
            comboBox1.Location = new Point(311, 762);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(459, 46);
            comboBox1.TabIndex = 82;
            // 
            // Settings
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.BCO_677f5122_1ca8_402c_addd_6f3eadad8129;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1511, 948);
            Controls.Add(comboBox1);
            Controls.Add(label11);
            Controls.Add(txtReligion);
            Controls.Add(label12);
            Controls.Add(txtCitizenship);
            Controls.Add(label13);
            Controls.Add(txtUsername);
            Controls.Add(label10);
            Controls.Add(btnCancel);
            Controls.Add(btnSaveChanges);
            Controls.Add(label9);
            Controls.Add(txtAddress);
            Controls.Add(label8);
            Controls.Add(dtpBirthdate);
            Controls.Add(txtLastName);
            Controls.Add(label7);
            Controls.Add(txtMobileNo);
            Controls.Add(label6);
            Controls.Add(txtEmailAdd);
            Controls.Add(label5);
            Controls.Add(txtMiddleName);
            Controls.Add(label4);
            Controls.Add(txtFirstName);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(btnNotification);
            Controls.Add(label1);
            Controls.Add(btnHome);
            Controls.Add(btnLogout);
            Controls.Add(btnProfile);
            Controls.Add(panel2);
            Controls.Add(pictureBox4);
            Name = "Settings";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Settings";
            Load += Settings_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            panel2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnNotification;
        private Label label1;
        private Button btnHome;
        private Button btnLogout;
        private Button btnProfile;
        private PictureBox pictureBox4;
        private Panel panel1;
        private Panel panel2;
        private Button btnPassword;
        private Label label2;
        private Label label3;
        private TextBox txtFirstName;
        private TextBox txtMiddleName;
        private Label label4;
        private TextBox txtEmailAdd;
        private Label label5;
        private TextBox txtMobileNo;
        private Label label6;
        private TextBox txtLastName;
        private Label label7;
        private DateTimePicker dtpBirthdate;
        private Label label8;
        private TextBox txtAddress;
        private Label label9;
        private Button btnSaveChanges;
        private Button btnCancel;
        private Label label10;
        private TextBox txtUsername;
        private Label label11;
        private TextBox txtReligion;
        private Label label12;
        private TextBox txtCitizenship;
        private Label label13;
        private ComboBox comboBox1;
    }
}
