namespace BSMART
{
    partial class MayorViewInventory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MayorViewInventory));
            btnNotification = new Button();
            label1 = new Label();
            btnSettings = new Button();
            btnLogout = new Button();
            pictureBox4 = new PictureBox();
            btnMayorDash = new Button();
            panel2 = new Panel();
            btnViewInventory = new Button();
            panel1 = new Panel();
            btnResRecByBar = new Button();
            btnResHealthRec = new Button();
            btnViewHealthReport = new Button();
            cmbBarangay = new ComboBox();
            label3 = new Label();
            panel3 = new Panel();
            btnDownloadResHealthRec = new Button();
            txtSearch = new TextBox();
            dgvMayorViewInventory = new DataGridView();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMayorViewInventory).BeginInit();
            SuspendLayout();
            // 
            // btnNotification
            // 
            btnNotification.BackColor = Color.SkyBlue;
            btnNotification.BackgroundImage = (Image)resources.GetObject("btnNotification.BackgroundImage");
            btnNotification.BackgroundImageLayout = ImageLayout.Zoom;
            btnNotification.FlatStyle = FlatStyle.Popup;
            btnNotification.ForeColor = Color.White;
            btnNotification.Location = new Point(1283, 12);
            btnNotification.Name = "btnNotification";
            btnNotification.Size = new Size(68, 48);
            btnNotification.TabIndex = 54;
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
            // btnSettings
            // 
            btnSettings.BackColor = Color.SkyBlue;
            btnSettings.BackgroundImage = (Image)resources.GetObject("btnSettings.BackgroundImage");
            btnSettings.BackgroundImageLayout = ImageLayout.Zoom;
            btnSettings.FlatStyle = FlatStyle.Flat;
            btnSettings.Location = new Point(1357, 12);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(68, 48);
            btnSettings.TabIndex = 52;
            btnSettings.UseVisualStyleBackColor = false;
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
            btnLogout.Click += btnLogout_Click;
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
            // btnMayorDash
            // 
            btnMayorDash.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnMayorDash.Location = new Point(0, 229);
            btnMayorDash.Name = "btnMayorDash";
            btnMayorDash.Size = new Size(260, 81);
            btnMayorDash.TabIndex = 49;
            btnMayorDash.Text = "Mayor Dashboard\r\n";
            btnMayorDash.UseVisualStyleBackColor = true;
            btnMayorDash.Click += btnMayorDash_Click;
            // 
            // panel2
            // 
            panel2.BackgroundImage = (Image)resources.GetObject("panel2.BackgroundImage");
            panel2.Controls.Add(btnViewInventory);
            panel2.Controls.Add(panel1);
            panel2.Controls.Add(btnResRecByBar);
            panel2.Controls.Add(btnResHealthRec);
            panel2.Controls.Add(btnViewHealthReport);
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(260, 949);
            panel2.TabIndex = 48;
            // 
            // btnViewInventory
            // 
            btnViewInventory.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnViewInventory.Location = new Point(0, 636);
            btnViewInventory.Name = "btnViewInventory";
            btnViewInventory.Size = new Size(260, 74);
            btnViewInventory.TabIndex = 21;
            btnViewInventory.Text = "View Inventory by Barangay\r\n";
            btnViewInventory.UseVisualStyleBackColor = true;
            btnViewInventory.Click += btnViewInventory_Click;
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
            // btnResRecByBar
            // 
            btnResRecByBar.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnResRecByBar.Location = new Point(0, 396);
            btnResRecByBar.Name = "btnResRecByBar";
            btnResRecByBar.Size = new Size(260, 74);
            btnResRecByBar.TabIndex = 4;
            btnResRecByBar.Text = "Resident Record by Barangay";
            btnResRecByBar.UseVisualStyleBackColor = true;
            btnResRecByBar.Click += btnResRecByBar_Click;
            // 
            // btnResHealthRec
            // 
            btnResHealthRec.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnResHealthRec.Location = new Point(0, 476);
            btnResHealthRec.Name = "btnResHealthRec";
            btnResHealthRec.Size = new Size(260, 74);
            btnResHealthRec.TabIndex = 7;
            btnResHealthRec.Text = "Resident Health Record by Barangay";
            btnResHealthRec.UseVisualStyleBackColor = true;
            btnResHealthRec.Click += btnResHealthRec_Click;
            // 
            // btnViewHealthReport
            // 
            btnViewHealthReport.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnViewHealthReport.Location = new Point(0, 556);
            btnViewHealthReport.Name = "btnViewHealthReport";
            btnViewHealthReport.Size = new Size(260, 74);
            btnViewHealthReport.TabIndex = 8;
            btnViewHealthReport.Text = "View Health Report by Barangay";
            btnViewHealthReport.UseVisualStyleBackColor = true;
            btnViewHealthReport.Click += btnViewHealthReport_Click;
            // 
            // cmbBarangay
            // 
            cmbBarangay.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbBarangay.FormattingEnabled = true;
            cmbBarangay.Items.AddRange(new object[] { "Cabadiangan", "Calero", "Catarman", "Cotcot", "Jubay", "Lataban", "Mulao", "Poblacion", "San Roque", "San Vicente", "Santa Cruz", "Tabla", "Tayud", "Yati" });
            cmbBarangay.Location = new Point(476, 183);
            cmbBarangay.Name = "cmbBarangay";
            cmbBarangay.Size = new Size(249, 40);
            cmbBarangay.TabIndex = 62;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(266, 191);
            label3.Name = "label3";
            label3.Size = new Size(215, 32);
            label3.TabIndex = 61;
            label3.Text = "Select a Barangay:\r\n";
            // 
            // panel3
            // 
            panel3.BackColor = Color.Transparent;
            panel3.Controls.Add(btnDownloadResHealthRec);
            panel3.Controls.Add(txtSearch);
            panel3.Controls.Add(dgvMayorViewInventory);
            panel3.Location = new Point(266, 226);
            panel3.Name = "panel3";
            panel3.Size = new Size(1233, 688);
            panel3.TabIndex = 60;
            // 
            // btnDownloadResHealthRec
            // 
            btnDownloadResHealthRec.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDownloadResHealthRec.Location = new Point(973, 83);
            btnDownloadResHealthRec.Name = "btnDownloadResHealthRec";
            btnDownloadResHealthRec.Size = new Size(257, 36);
            btnDownloadResHealthRec.TabIndex = 2;
            btnDownloadResHealthRec.Text = "Download Records(PDF)";
            btnDownloadResHealthRec.UseVisualStyleBackColor = true;
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSearch.Location = new Point(311, 17);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(612, 45);
            txtSearch.TabIndex = 1;
            // 
            // dgvMayorViewInventory
            // 
            dgvMayorViewInventory.BackgroundColor = SystemColors.GradientInactiveCaption;
            dgvMayorViewInventory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMayorViewInventory.GridColor = SystemColors.HotTrack;
            dgvMayorViewInventory.Location = new Point(3, 125);
            dgvMayorViewInventory.Name = "dgvMayorViewInventory";
            dgvMayorViewInventory.RowHeadersWidth = 62;
            dgvMayorViewInventory.Size = new Size(1227, 563);
            dgvMayorViewInventory.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI", 22F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(635, 78);
            label2.Name = "label2";
            label2.Size = new Size(591, 60);
            label2.TabIndex = 59;
            label2.Text = "Barangay Inventory Report";
            // 
            // MayorViewInventory
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.BCO_677f5122_1ca8_402c_addd_6f3eadad8129;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1511, 948);
            Controls.Add(cmbBarangay);
            Controls.Add(btnNotification);
            Controls.Add(label3);
            Controls.Add(label1);
            Controls.Add(panel3);
            Controls.Add(btnSettings);
            Controls.Add(label2);
            Controls.Add(btnLogout);
            Controls.Add(pictureBox4);
            Controls.Add(btnMayorDash);
            Controls.Add(panel2);
            Name = "MayorViewInventory";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "MayorViewInventory";
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMayorViewInventory).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnNotification;
        private Label label1;
        private Button btnSettings;
        private Button btnLogout;
        private PictureBox pictureBox4;
        private Button btnMayorDash;
        private Panel panel2;
        private Button btnViewInventory;
        private Panel panel1;
        private Button btnResRecByBar;
        private Button btnResHealthRec;
        private Button btnViewHealthReport;
        private ComboBox cmbBarangay;
        private Label label3;
        private Panel panel3;
        private Button btnDownloadResHealthRec;
        private TextBox txtSearch;
        private DataGridView dgvMayorViewInventory;
        private Label label2;
    }
}