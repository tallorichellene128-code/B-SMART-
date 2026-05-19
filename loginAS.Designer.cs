namespace BSMART
{
    partial class loginAS
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(loginAS));
            pictureBox1 = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            btnRes = new Button();
            btnBCap = new Button();
            btnMayor = new Button();
            label3 = new Label();
            btnLGUStaff = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.BackgroundImage = (Image)resources.GetObject("pictureBox1.BackgroundImage");
            pictureBox1.BackgroundImageLayout = ImageLayout.Zoom;
            pictureBox1.Location = new Point(91, 39);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(320, 255);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Impact", 20F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(371, 89);
            label1.Name = "label1";
            label1.Size = new Size(171, 48);
            label1.TabIndex = 1;
            label1.Text = "B-SMART";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Impact", 14F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label2.Location = new Point(371, 149);
            label2.Name = "label2";
            label2.Size = new Size(408, 70);
            label2.TabIndex = 2;
            label2.Text = "Baranggay System for Monitoring, \r\nAccess, and Record of Treatment";
            // 
            // btnRes
            // 
            btnRes.BackColor = Color.Transparent;
            btnRes.FlatStyle = FlatStyle.Popup;
            btnRes.Font = new Font("Segoe UI", 12F);
            btnRes.Location = new Point(666, 431);
            btnRes.Name = "btnRes";
            btnRes.Size = new Size(181, 66);
            btnRes.TabIndex = 16;
            btnRes.Text = "Resident";
            btnRes.UseVisualStyleBackColor = false;
            btnRes.Click += btnRes_Click;
            // 
            // btnBCap
            // 
            btnBCap.BackColor = Color.Transparent;
            btnBCap.FlatStyle = FlatStyle.Popup;
            btnBCap.Font = new Font("Segoe UI", 12F);
            btnBCap.Location = new Point(305, 431);
            btnBCap.Name = "btnBCap";
            btnBCap.Size = new Size(289, 66);
            btnBCap.TabIndex = 15;
            btnBCap.Text = "Barangay Captain";
            btnBCap.UseVisualStyleBackColor = false;
            btnBCap.Click += btnBCap_Click_1;
            // 
            // btnMayor
            // 
            btnMayor.BackColor = Color.Transparent;
            btnMayor.FlatStyle = FlatStyle.Popup;
            btnMayor.Font = new Font("Segoe UI", 12F);
            btnMayor.Location = new Point(41, 431);
            btnMayor.Name = "btnMayor";
            btnMayor.Size = new Size(181, 66);
            btnMayor.TabIndex = 14;
            btnMayor.Text = "Mayor";
            btnMayor.UseVisualStyleBackColor = false;
            btnMayor.Click += btnMayor_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(371, 341);
            label3.Name = "label3";
            label3.Size = new Size(153, 45);
            label3.TabIndex = 13;
            label3.Text = "Login As:";
            // 
            // btnLGUStaff
            // 
            btnLGUStaff.BackColor = Color.Transparent;
            btnLGUStaff.FlatStyle = FlatStyle.Popup;
            btnLGUStaff.Font = new Font("Segoe UI", 12F);
            btnLGUStaff.Location = new Point(305, 534);
            btnLGUStaff.Name = "btnLGUStaff";
            btnLGUStaff.Size = new Size(289, 66);
            btnLGUStaff.TabIndex = 17;
            btnLGUStaff.Text = "LGU Staff";
            btnLGUStaff.UseVisualStyleBackColor = false;
            btnLGUStaff.Click += btnLGUStaff_Click;
            // 
            // loginAS
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlLight;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Center;
            ClientSize = new Size(894, 612);
            Controls.Add(btnLGUStaff);
            Controls.Add(btnRes);
            Controls.Add(btnBCap);
            Controls.Add(btnMayor);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "loginAS";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login As";
            Load += LoginRes_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Label label1;
        private Label label2;
        private Button btnRes;
        private Button btnBCap;
        private Button btnMayor;
        private Label label3;
        private Button btnLGUStaff;
    }
}
