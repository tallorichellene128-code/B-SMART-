namespace BSMART
{
    partial class Register2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Register2));
            panel1 = new Panel();
            label1 = new Label();
            btnNext = new Button();
            btnPrev = new Button();
            label8 = new Label();
            label7 = new Label();
            txtAddress = new TextBox();
            cmbBarangay = new ComboBox();
            label2 = new Label();
            txtMobileNum = new TextBox();
            label3 = new Label();
            txtEmail = new TextBox();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.LightBlue;
            panel1.Controls.Add(label1);
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1025, 95);
            panel1.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 20F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.Location = new Point(249, 21);
            label1.Name = "label1";
            label1.Size = new Size(585, 54);
            label1.TabIndex = 1;
            label1.Text = "Lilo-an B-SMART Registration";
            // 
            // btnNext
            // 
            btnNext.BackColor = Color.Transparent;
            btnNext.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnNext.Location = new Point(759, 689);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(228, 50);
            btnNext.TabIndex = 24;
            btnNext.Text = "Next";
            btnNext.UseVisualStyleBackColor = false;
            btnNext.Click += btnNext_Click;
            // 
            // btnPrev
            // 
            btnPrev.BackColor = Color.Transparent;
            btnPrev.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnPrev.Location = new Point(43, 689);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(228, 50);
            btnPrev.TabIndex = 23;
            btnPrev.Text = "Previous";
            btnPrev.UseVisualStyleBackColor = false;
            btnPrev.Click += btnPrev_Click;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.BackColor = Color.Transparent;
            label8.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.Location = new Point(43, 242);
            label8.Name = "label8";
            label8.Size = new Size(233, 32);
            label8.TabIndex = 28;
            label8.Text = "Sitio/Address/Street";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.Transparent;
            label7.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.Location = new Point(43, 124);
            label7.Name = "label7";
            label7.Size = new Size(117, 32);
            label7.TabIndex = 27;
            label7.Text = "Barangay";
            // 
            // txtAddress
            // 
            txtAddress.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtAddress.Location = new Point(43, 277);
            txtAddress.Name = "txtAddress";
            txtAddress.Size = new Size(944, 45);
            txtAddress.TabIndex = 26;
            txtAddress.TextChanged += txtAddress_TextChanged;
            // 
            // cmbBarangay
            // 
            cmbBarangay.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbBarangay.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbBarangay.FormattingEnabled = true;
            cmbBarangay.IntegralHeight = false;
            cmbBarangay.Location = new Point(43, 159);
            cmbBarangay.MaxDropDownItems = 5;
            cmbBarangay.Name = "cmbBarangay";
            cmbBarangay.Size = new Size(944, 46);
            cmbBarangay.TabIndex = 25;
            cmbBarangay.SelectedIndexChanged += cmbBarangay_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(43, 508);
            label2.Name = "label2";
            label2.Size = new Size(134, 32);
            label2.TabIndex = 34;
            label2.Text = "Mobile No.";
            // 
            // txtMobileNum
            // 
            txtMobileNum.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtMobileNum.Location = new Point(43, 543);
            txtMobileNum.Name = "txtMobileNum";
            txtMobileNum.Size = new Size(944, 45);
            txtMobileNum.TabIndex = 33;
            txtMobileNum.TextChanged += txtMobileNum_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(43, 380);
            label3.Name = "label3";
            label3.Size = new Size(72, 32);
            label3.TabIndex = 32;
            label3.Text = "Email";
            // 
            // txtEmail
            // 
            txtEmail.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtEmail.Location = new Point(43, 415);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(944, 45);
            txtEmail.TabIndex = 31;
            txtEmail.TextChanged += txtEmail_TextChanged;
            // 
            // Register2
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1028, 780);
            Controls.Add(label2);
            Controls.Add(txtMobileNum);
            Controls.Add(label3);
            Controls.Add(txtEmail);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(txtAddress);
            Controls.Add(cmbBarangay);
            Controls.Add(btnNext);
            Controls.Add(btnPrev);
            Controls.Add(panel1);
            Name = "Register2";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Register2";
            Load += Register2_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Label label1;
        private Button btnNext;
        private Button btnPrev;
        private Label label8;
        private Label label7;
        private TextBox txtAddress;
        private ComboBox cmbBarangay;
        private Label label2;
        private TextBox txtMobileNum;
        private Label label3;
        private TextBox txtEmail;
    }
}