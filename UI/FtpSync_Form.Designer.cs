namespace TekstilScada.UI
{
    partial class FtpSync_Form
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlTop = new System.Windows.Forms.Panel();
            this.cmbMachines = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lstLocalRecipes = new System.Windows.Forms.ListBox();
            this.pnlMiddle = new System.Windows.Forms.Panel();
            this.btnReceive = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lstHmiRecipes = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnRefreshHmi = new System.Windows.Forms.Button();
            this.dgvTransfers = new System.Windows.Forms.DataGridView();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.pnlMiddle.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransfers)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.cmbMachines);
            this.pnlTop.Controls.Add(this.label1);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(10, 10);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(962, 50);
            this.pnlTop.TabIndex = 0;
            // 
            // cmbMachines
            // 
            this.cmbMachines.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMachines.FormattingEnabled = true;
            this.cmbMachines.Location = new System.Drawing.Point(120, 11);
            this.cmbMachines.Name = "cmbMachines";
            this.cmbMachines.Size = new System.Drawing.Size(350, 28);
            this.cmbMachines.TabIndex = 1;
            this.cmbMachines.SelectedIndexChanged += new System.EventHandler(this.cmbMachines_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(3, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Hedef Makine:";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainer1.Location = new System.Drawing.Point(10, 60);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pnlMiddle);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Size = new System.Drawing.Size(962, 240);
            this.splitContainer1.SplitterDistance = 420;
            this.splitContainer1.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lstLocalRecipes);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(420, 240);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SCADA Reçeteleri (Çoklu Seçim)";
            // 
            // lstLocalRecipes
            // 
            this.lstLocalRecipes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstLocalRecipes.FormattingEnabled = true;
            this.lstLocalRecipes.ItemHeight = 20;
            this.lstLocalRecipes.Location = new System.Drawing.Point(3, 23);
            this.lstLocalRecipes.Name = "lstLocalRecipes";
            this.lstLocalRecipes.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstLocalRecipes.Size = new System.Drawing.Size(414, 214);
            this.lstLocalRecipes.TabIndex = 0;
            // 
            // pnlMiddle
            // 
            this.pnlMiddle.Controls.Add(this.btnReceive);
            this.pnlMiddle.Controls.Add(this.btnSend);
            this.pnlMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMiddle.Location = new System.Drawing.Point(0, 0);
            this.pnlMiddle.Name = "pnlMiddle";
            this.pnlMiddle.Size = new System.Drawing.Size(118, 240);
            this.pnlMiddle.TabIndex = 1;
            // 
            // btnReceive
            // 
            this.btnReceive.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnReceive.Location = new System.Drawing.Point(9, 130);
            this.btnReceive.Name = "btnReceive";
            this.btnReceive.Size = new System.Drawing.Size(100, 50);
            this.btnReceive.TabIndex = 1;
            this.btnReceive.Text = "<< Al";
            this.btnReceive.UseVisualStyleBackColor = true;
            this.btnReceive.Click += new System.EventHandler(this.btnReceive_Click);
            // 
            // btnSend
            // 
            this.btnSend.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnSend.Location = new System.Drawing.Point(9, 60);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(100, 50);
            this.btnSend.TabIndex = 0;
            this.btnSend.Text = "Gönder >>";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lstHmiRecipes);
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox2.Location = new System.Drawing.Point(118, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(420, 240);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "HMI Reçeteleri (Çoklu Seçim)";
            // 
            // lstHmiRecipes
            // 
            this.lstHmiRecipes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstHmiRecipes.FormattingEnabled = true;
            this.lstHmiRecipes.ItemHeight = 20;
            this.lstHmiRecipes.Location = new System.Drawing.Point(3, 63);
            this.lstHmiRecipes.Name = "lstHmiRecipes";
            this.lstHmiRecipes.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstHmiRecipes.Size = new System.Drawing.Size(414, 174);
            this.lstHmiRecipes.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnRefreshHmi);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 23);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(414, 40);
            this.panel1.TabIndex = 1;
            // 
            // btnRefreshHmi
            // 
            this.btnRefreshHmi.Location = new System.Drawing.Point(3, 5);
            this.btnRefreshHmi.Name = "btnRefreshHmi";
            this.btnRefreshHmi.Size = new System.Drawing.Size(120, 30);
            this.btnRefreshHmi.TabIndex = 0;
            this.btnRefreshHmi.Text = "Listeyi Yenile";
            this.btnRefreshHmi.UseVisualStyleBackColor = true;
            this.btnRefreshHmi.Click += new System.EventHandler(this.btnRefreshHmi_Click);
            // 
            // dgvTransfers
            // 
            this.dgvTransfers.AllowUserToAddRows = false;
            this.dgvTransfers.AllowUserToDeleteRows = false;
            this.dgvTransfers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTransfers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTransfers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTransfers.Location = new System.Drawing.Point(10, 300);
            this.dgvTransfers.Name = "dgvTransfers";
            this.dgvTransfers.ReadOnly = true;
            this.dgvTransfers.RowHeadersWidth = 51;
            this.dgvTransfers.RowTemplate.Height = 29;
            this.dgvTransfers.Size = new System.Drawing.Size(962, 243);
            this.dgvTransfers.TabIndex = 2;
            // 
            // FtpSync_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(982, 553);
            this.Controls.Add(this.dgvTransfers);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.pnlTop);
            this.Name = "FtpSync_Form";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Toplu FTP Reçete Senkronizasyonu";
            this.Load += new System.EventHandler(this.FtpSync_Form_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.pnlMiddle.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTransfers)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.ComboBox cmbMachines;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox lstLocalRecipes;
        private System.Windows.Forms.Panel pnlMiddle;
        private System.Windows.Forms.Button btnReceive;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox lstHmiRecipes;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnRefreshHmi;
        private System.Windows.Forms.DataGridView dgvTransfers;
    }
}
