namespace BSMART
{
    partial class loginLGU
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(loginLGU));
            btnBack = new Button();
            cmbBarangay = new ComboBox();
            pictureBox3 = new PictureBox();
            btnLogin = new Button();
            pictureBox2 = new PictureBox();
            pictureBox4 = new PictureBox();
            txtPassword = new TextBox();
            txtUsername = new TextBox();
            label2 = new Label();
            label1 = new Label();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // btnBack
            // 
            btnBack.BackColor = Color.Transparent;
            btnBack.BackgroundImage = (Image)resources.GetObject("btnBack.BackgroundImage");
            btnBack.BackgroundImageLayout = ImageLayout.Zoom;
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.Location = new Point(0, 1);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(65, 45);
            btnBack.TabIndex = 35;
            btnBack.UseVisualStyleBackColor = false;
            btnBack.Click += btnBack_Click;
            // 
            // cmbBarangay
            // 
            cmbBarangay.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBarangay.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbBarangay.FormattingEnabled = true;
            cmbBarangay.IntegralHeight = false;
            cmbBarangay.Items.AddRange(new object[] { "Cabadiangan", "Calero", "Catarman", "Cotcot", "Jubay", "Lataban", "Mulao", "Poblacion", "San Roque", "San Vicente", "Santa Cruz", "Tabla", "Tayud", "Yati" });
            cmbBarangay.Location = new Point(331, 467);
            cmbBarangay.MaxDropDownItems = 3;
            cmbBarangay.Name = "cmbBarangay";
            cmbBarangay.Size = new Size(320, 46);
            cmbBarangay.TabIndex = 34;
            cmbBarangay.SelectedIndexChanged += cmbBarangay_SelectedIndexChanged;
            // 
            // pictureBox3
            // 
            pictureBox3.BackColor = Color.Transparent;
            pictureBox3.BackgroundImage = (Image)resources.GetObject("pictureBox3.BackgroundImage");
            pictureBox3.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox3.Location = new Point(274, 467);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(51, 45);
            pictureBox3.TabIndex = 33;
            pictureBox3.TabStop = false;
            // 
            // btnLogin
            // 
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnLogin.Location = new Point(274, 558);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(377, 47);
            btnLogin.TabIndex = 32;
            btnLogin.Text = "Login";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.Transparent;
            pictureBox2.BackgroundImage = (Image)resources.GetObject("pictureBox2.BackgroundImage");
            pictureBox2.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox2.Location = new Point(274, 407);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(51, 45);
            pictureBox2.TabIndex = 31;
            pictureBox2.TabStop = false;
            // 
            // pictureBox4
            // 
            pictureBox4.BackColor = Color.Transparent;
            pictureBox4.BackgroundImage = (Image)resources.GetObject("pictureBox4.BackgroundImage");
            pictureBox4.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox4.Location = new Point(274, 345);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(51, 45);
            pictureBox4.TabIndex = 30;
            pictureBox4.TabStop = false;
            // 
            // txtPassword
            // 
            txtPassword.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtPassword.Location = new Point(331, 407);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(320, 45);
            txtPassword.TabIndex = 29;
            // 
            // txtUsername
            // 
            txtUsername.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtUsername.Location = new Point(331, 345);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(320, 45);
            txtUsername.TabIndex = 28;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Impact", 14F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label2.Location = new Point(386, 169);
            label2.Name = "label2";
            label2.Size = new Size(408, 70);
            label2.TabIndex = 27;
            label2.Text = "Baranggay System for Monitoring, \r\nAccess, and Record of Treatment";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Impact", 20F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(386, 109);
            label1.Name = "label1";
            label1.Size = new Size(171, 48);
            label1.TabIndex = 26;
            label1.Text = "B-SMART";
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.BackgroundImage = (Image)resources.GetObject("pictureBox1.BackgroundImage");
            pictureBox1.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox1.Location = new Point(106, 59);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(320, 255);
            pictureBox1.TabIndex = 25;
            pictureBox1.TabStop = false;
            // 
            // loginLGU
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.BCO_677f5122_1ca8_402c_addd_6f3eadad8129;
            ClientSize = new Size(894, 634);
            Controls.Add(btnBack);
            Controls.Add(cmbBarangay);
            Controls.Add(pictureBox3);
            Controls.Add(btnLogin);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox4);
            Controls.Add(txtPassword);
            Controls.Add(txtUsername);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            Name = "loginLGU";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "loginLGU";
            Load += loginLGU_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnBack;
        private ComboBox cmbBarangay;
        private PictureBox pictureBox3;
        private Button btnLogin;
        private PictureBox pictureBox2;
        private PictureBox pictureBox4;
        private TextBox txtPassword;
        private TextBox txtUsername;
        private Label label2;
        private Label label1;
        private PictureBox pictureBox1;
    }
}