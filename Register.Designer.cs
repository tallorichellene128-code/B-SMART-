namespace BSMART
{
    partial class Register
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Register));
            panel1 = new Panel();
            label1 = new Label();
            txtFirstName = new TextBox();
            txtLastName = new TextBox();
            dtpBirthday = new DateTimePicker();
            cmbAge = new ComboBox();
            cmbGender = new ComboBox();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            btnBack = new Button();
            btnNext = new Button();
            txtMiddleName = new TextBox();
            label9 = new Label();
            txtPlaceofbirth = new TextBox();
            txtReligion = new TextBox();
            txtCitizenship = new TextBox();
            label7 = new Label();
            label8 = new Label();
            label10 = new Label();
            label11 = new Label();
            comboBox1 = new ComboBox();
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
            panel1.TabIndex = 0;
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
            // txtFirstName
            // 
            txtFirstName.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtFirstName.Location = new Point(36, 166);
            txtFirstName.Name = "txtFirstName";
            txtFirstName.Size = new Size(432, 45);
            txtFirstName.TabIndex = 1;
            txtFirstName.TextChanged += txtFirstName_TextChanged;
            // 
            // txtLastName
            // 
            txtLastName.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtLastName.Location = new Point(548, 166);
            txtLastName.Name = "txtLastName";
            txtLastName.Size = new Size(432, 45);
            txtLastName.TabIndex = 2;
            txtLastName.TextChanged += txtLastName_TextChanged;
            // 
            // dtpBirthday
            // 
            dtpBirthday.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dtpBirthday.Location = new Point(548, 277);
            dtpBirthday.Name = "dtpBirthday";
            dtpBirthday.Size = new Size(432, 45);
            dtpBirthday.TabIndex = 3;
            dtpBirthday.ValueChanged += dtpBirthday_ValueChanged;
            // 
            // cmbAge
            // 
            cmbAge.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbAge.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbAge.FormattingEnabled = true;
            cmbAge.IntegralHeight = false;
            cmbAge.Location = new Point(36, 387);
            cmbAge.MaxDropDownItems = 5;
            cmbAge.Name = "cmbAge";
            cmbAge.Size = new Size(432, 46);
            cmbAge.TabIndex = 4;
            cmbAge.SelectedIndexChanged += cmbAge_SelectedIndexChanged;
            // 
            // cmbGender
            // 
            cmbGender.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbGender.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbGender.FormattingEnabled = true;
            cmbGender.IntegralHeight = false;
            cmbGender.Location = new Point(548, 387);
            cmbGender.Name = "cmbGender";
            cmbGender.Size = new Size(432, 46);
            cmbGender.TabIndex = 5;
            cmbGender.SelectedIndexChanged += cmbGender_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(36, 131);
            label2.Name = "label2";
            label2.Size = new Size(132, 32);
            label2.TabIndex = 8;
            label2.Text = "First Name";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(548, 131);
            label3.Name = "label3";
            label3.Size = new Size(130, 32);
            label3.TabIndex = 9;
            label3.Text = "Last Name";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(548, 242);
            label4.Name = "label4";
            label4.Size = new Size(156, 32);
            label4.TabIndex = 10;
            label4.Text = "Date of Birth";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(36, 352);
            label5.Name = "label5";
            label5.Size = new Size(57, 32);
            label5.TabIndex = 11;
            label5.Text = "Age";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.Transparent;
            label6.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(548, 352);
            label6.Name = "label6";
            label6.Size = new Size(94, 32);
            label6.TabIndex = 12;
            label6.Text = "Gender";
            // 
            // btnBack
            // 
            btnBack.BackColor = Color.Transparent;
            btnBack.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnBack.Location = new Point(36, 697);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(228, 50);
            btnBack.TabIndex = 15;
            btnBack.Text = "Back";
            btnBack.UseVisualStyleBackColor = false;
            btnBack.Click += btnBack_Click;
            // 
            // btnNext
            // 
            btnNext.BackColor = Color.Transparent;
            btnNext.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnNext.Location = new Point(752, 697);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(228, 50);
            btnNext.TabIndex = 16;
            btnNext.Text = "Next";
            btnNext.UseVisualStyleBackColor = false;
            btnNext.Click += btnNext_Click;
            // 
            // txtMiddleName
            // 
            txtMiddleName.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtMiddleName.Location = new Point(36, 277);
            txtMiddleName.Name = "txtMiddleName";
            txtMiddleName.Size = new Size(432, 45);
            txtMiddleName.TabIndex = 17;
            txtMiddleName.TextChanged += txtMiddleName_TextChanged;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.BackColor = Color.Transparent;
            label9.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label9.Location = new Point(36, 242);
            label9.Name = "label9";
            label9.Size = new Size(161, 32);
            label9.TabIndex = 18;
            label9.Text = "Middle Name";
            // 
            // txtPlaceofbirth
            // 
            txtPlaceofbirth.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtPlaceofbirth.Location = new Point(36, 494);
            txtPlaceofbirth.Name = "txtPlaceofbirth";
            txtPlaceofbirth.Size = new Size(432, 45);
            txtPlaceofbirth.TabIndex = 19;
            txtPlaceofbirth.TextChanged += txtPlaceofbirth_TextChanged;
            // 
            // txtReligion
            // 
            txtReligion.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtReligion.Location = new Point(36, 601);
            txtReligion.Name = "txtReligion";
            txtReligion.Size = new Size(432, 45);
            txtReligion.TabIndex = 21;
            txtReligion.TextChanged += txtReligion_TextChanged;
            // 
            // txtCitizenship
            // 
            txtCitizenship.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtCitizenship.Location = new Point(548, 601);
            txtCitizenship.Name = "txtCitizenship";
            txtCitizenship.Size = new Size(432, 45);
            txtCitizenship.TabIndex = 22;
            txtCitizenship.TextChanged += txtCitizenship_TextChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.Transparent;
            label7.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.Location = new Point(36, 459);
            label7.Name = "label7";
            label7.Size = new Size(161, 32);
            label7.TabIndex = 23;
            label7.Text = "Place of Birth";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.BackColor = Color.Transparent;
            label8.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.Location = new Point(548, 459);
            label8.Name = "label8";
            label8.Size = new Size(133, 32);
            label8.TabIndex = 24;
            label8.Text = "Civil Status";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.BackColor = Color.Transparent;
            label10.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label10.Location = new Point(36, 566);
            label10.Name = "label10";
            label10.Size = new Size(101, 32);
            label10.TabIndex = 25;
            label10.Text = "Religion";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.BackColor = Color.Transparent;
            label11.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label11.Location = new Point(548, 566);
            label11.Name = "label11";
            label11.Size = new Size(132, 32);
            label11.TabIndex = 26;
            label11.Text = "Citizenship";
            // 
            // comboBox1
            // 
            comboBox1.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "Single", "Married", "Widowed", "Divorced", "Legally separated" });
            comboBox1.Location = new Point(548, 494);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(432, 46);
            comboBox1.TabIndex = 83;
            // 
            // Register
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1028, 780);
            Controls.Add(comboBox1);
            Controls.Add(label11);
            Controls.Add(label10);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(txtCitizenship);
            Controls.Add(txtReligion);
            Controls.Add(txtPlaceofbirth);
            Controls.Add(label9);
            Controls.Add(txtMiddleName);
            Controls.Add(btnNext);
            Controls.Add(btnBack);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(cmbGender);
            Controls.Add(cmbAge);
            Controls.Add(dtpBirthday);
            Controls.Add(txtLastName);
            Controls.Add(txtFirstName);
            Controls.Add(panel1);
            Name = "Register";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Register";
            Load += Register_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Label label1;
        private TextBox txtFirstName;
        private TextBox txtLastName;
        private DateTimePicker dtpBirthday;
        private ComboBox cmbAge;
        private ComboBox cmbGender;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Button btnBack;
        private Button btnNext;
        private TextBox txtMiddleName;
        private Label label9;
        private TextBox txtPlaceofbirth;
        private TextBox txtReligion;
        private TextBox txtCitizenship;
        private Label label7;
        private Label label8;
        private Label label10;
        private Label label11;
        private ComboBox comboBox1;
    }
}