namespace BSMART
{
    partial class Settings2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings2));
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
            txtConfirmNewPass = new TextBox();
            label5 = new Label();
            txtNewPass = new TextBox();
            label4 = new Label();
            txtCurrentPass = new TextBox();
            label3 = new Label();
            btnCancel = new Button();
            btnSaveChanges = new Button();
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
            btnNotification.TabIndex = 64;
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
            label1.TabIndex = 63;
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
            btnHome.TabIndex = 62;
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
            btnLogout.TabIndex = 61;
            btnLogout.UseVisualStyleBackColor = false;
            btnLogout.Click += btnLogout_Click;
            // 
            // btnProfile
            // 
            btnProfile.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnProfile.Location = new Point(0, 229);
            btnProfile.Name = "btnProfile";
            btnProfile.Size = new Size(260, 81);
            btnProfile.TabIndex = 59;
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
            pictureBox4.TabIndex = 60;
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
            panel2.TabIndex = 58;
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
            label2.Location = new Point(645, 122);
            label2.Name = "label2";
            label2.Size = new Size(427, 54);
            label2.TabIndex = 65;
            label2.Text = "Password and Security";
            // 
            // txtConfirmNewPass
            // 
            txtConfirmNewPass.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtConfirmNewPass.Location = new Point(645, 495);
            txtConfirmNewPass.Name = "txtConfirmNewPass";
            txtConfirmNewPass.Size = new Size(461, 45);
            txtConfirmNewPass.TabIndex = 71;
            txtConfirmNewPass.TextChanged += txtConfirmNewPass_TextChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(645, 459);
            label5.Name = "label5";
            label5.Size = new Size(264, 32);
            label5.TabIndex = 70;
            label5.Text = "Confirm New Password:";
            // 
            // txtNewPass
            // 
            txtNewPass.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtNewPass.Location = new Point(645, 392);
            txtNewPass.Name = "txtNewPass";
            txtNewPass.Size = new Size(461, 45);
            txtNewPass.TabIndex = 69;
            txtNewPass.TextChanged += txtNewPass_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(645, 356);
            label4.Name = "label4";
            label4.Size = new Size(171, 32);
            label4.TabIndex = 68;
            label4.Text = "New Password:";
            // 
            // txtCurrentPass
            // 
            txtCurrentPass.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtCurrentPass.Location = new Point(645, 289);
            txtCurrentPass.Name = "txtCurrentPass";
            txtCurrentPass.Size = new Size(461, 45);
            txtCurrentPass.TabIndex = 67;
            txtCurrentPass.TextChanged += txtCurrentPass_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(645, 253);
            label3.Name = "label3";
            label3.Size = new Size(203, 32);
            label3.TabIndex = 66;
            label3.Text = "Current Password:";
            // 
            // btnCancel
            // 
            btnCancel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCancel.Location = new Point(647, 656);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(457, 49);
            btnCancel.TabIndex = 76;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnSaveChanges
            // 
            btnSaveChanges.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnSaveChanges.Location = new Point(645, 584);
            btnSaveChanges.Name = "btnSaveChanges";
            btnSaveChanges.Size = new Size(461, 49);
            btnSaveChanges.TabIndex = 75;
            btnSaveChanges.Text = "Save Changes";
            btnSaveChanges.UseVisualStyleBackColor = true;
            btnSaveChanges.Click += btnSaveChanges_Click;
            // 
            // Settings2
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.BCO_677f5122_1ca8_402c_addd_6f3eadad8129;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1511, 948);
            Controls.Add(btnCancel);
            Controls.Add(btnSaveChanges);
            Controls.Add(txtConfirmNewPass);
            Controls.Add(label5);
            Controls.Add(txtNewPass);
            Controls.Add(label4);
            Controls.Add(txtCurrentPass);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(btnNotification);
            Controls.Add(label1);
            Controls.Add(btnHome);
            Controls.Add(btnLogout);
            Controls.Add(btnProfile);
            Controls.Add(pictureBox4);
            Controls.Add(panel2);
            Name = "Settings2";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Settings2";
            Load += Settings2_Load;
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
        private TextBox txtConfirmNewPass;
        private Label label5;
        private TextBox txtNewPass;
        private Label label4;
        private TextBox txtCurrentPass;
        private Label label3;
        private Button btnCancel;
        private Button btnSaveChanges;
    }
}