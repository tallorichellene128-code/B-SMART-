namespace BSMART
{
    partial class Register3
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
            panel1 = new Panel();
            label1 = new Label();
            btnSubmit = new Button();
            btnPrev = new Button();
            label4 = new Label();
            txtPassConfirm = new TextBox();
            label3 = new Label();
            label2 = new Label();
            txtPassword = new TextBox();
            txtUsername = new TextBox();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.LightBlue;
            panel1.Controls.Add(label1);
            panel1.Location = new Point(1, -1);
            panel1.Name = "panel1";
            panel1.Size = new Size(1025, 95);
            panel1.TabIndex = 25;
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
            // btnSubmit
            // 
            btnSubmit.BackColor = Color.Transparent;
            btnSubmit.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnSubmit.Location = new Point(760, 688);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new Size(228, 50);
            btnSubmit.TabIndex = 35;
            btnSubmit.Text = "Submit";
            btnSubmit.UseVisualStyleBackColor = false;
            btnSubmit.Click += btnSubmit_Click;
            // 
            // btnPrev
            // 
            btnPrev.BackColor = Color.Transparent;
            btnPrev.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnPrev.Location = new Point(44, 688);
            btnPrev.Name = "btnPrev";
            btnPrev.Size = new Size(228, 50);
            btnPrev.TabIndex = 34;
            btnPrev.Text = "Previous";
            btnPrev.UseVisualStyleBackColor = false;
            btnPrev.Click += btnPrev_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(44, 439);
            label4.Name = "label4";
            label4.Size = new Size(210, 32);
            label4.TabIndex = 33;
            label4.Text = "Confirm password";
            // 
            // txtPassConfirm
            // 
            txtPassConfirm.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtPassConfirm.Location = new Point(44, 474);
            txtPassConfirm.Name = "txtPassConfirm";
            txtPassConfirm.Size = new Size(944, 45);
            txtPassConfirm.TabIndex = 32;
            txtPassConfirm.TextChanged += txtPassConfirm_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(44, 311);
            label3.Name = "label3";
            label3.Size = new Size(115, 32);
            label3.TabIndex = 31;
            label3.Text = "Password";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(44, 185);
            label2.Name = "label2";
            label2.Size = new Size(124, 32);
            label2.TabIndex = 30;
            label2.Text = "Username";
            // 
            // txtPassword
            // 
            txtPassword.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtPassword.Location = new Point(44, 346);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(944, 45);
            txtPassword.TabIndex = 29;
            txtPassword.TextChanged += txtPassword_TextChanged;
            // 
            // txtUsername
            // 
            txtUsername.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtUsername.Location = new Point(44, 220);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(944, 45);
            txtUsername.TabIndex = 28;
            txtUsername.TextChanged += txtUsername_TextChanged;
            // 
            // Register3
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.BCO_677f5122_1ca8_402c_addd_6f3eadad8129;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1028, 780);
            Controls.Add(panel1);
            Controls.Add(btnSubmit);
            Controls.Add(btnPrev);
            Controls.Add(label4);
            Controls.Add(txtPassConfirm);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(txtPassword);
            Controls.Add(txtUsername);
            Name = "Register3";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Register3";
            Load += Register3_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Label label1;
        private Button btnSubmit;
        private Button btnPrev;
        private Label label4;
        private TextBox txtPassConfirm;
        private Label label3;
        private Label label2;
        private TextBox txtPassword;
        private TextBox txtUsername;
    }
}