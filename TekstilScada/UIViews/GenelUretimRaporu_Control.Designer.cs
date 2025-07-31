namespace TekstilScada.UI.Views
{
    partial class GenelUretimRaporu_Control
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

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.pnlFilters = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioBuhar = new System.Windows.Forms.RadioButton();
            this.radioSu = new System.Windows.Forms.RadioButton();
            this.radioElektrik = new System.Windows.Forms.RadioButton();
            this.btnRaporOlustur = new System.Windows.Forms.Button();
            this.dtpEndTime = new System.Windows.Forms.DateTimePicker();
            this.dtpStartTime = new System.Windows.Forms.DateTimePicker();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.dgvReport = new System.Windows.Forms.DataGridView();
            this.pnlSelection = new System.Windows.Forms.Panel();
            this.flpMachineGroups = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnRemoveAll = new System.Windows.Forms.Button();
            this.btnAddAll = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.listBoxSeciliMakineler = new System.Windows.Forms.ListBox();
            this.pnlFilters.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).BeginInit();
            this.pnlSelection.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFilters
            // 
            this.pnlFilters.Controls.Add(this.groupBox1);
            this.pnlFilters.Controls.Add(this.btnRaporOlustur);
            this.pnlFilters.Controls.Add(this.dtpEndTime);
            this.pnlFilters.Controls.Add(this.dtpStartTime);
            this.pnlFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilters.Location = new System.Drawing.Point(0, 0);
            this.pnlFilters.Name = "pnlFilters";
            this.pnlFilters.Size = new System.Drawing.Size(1200, 80);
            this.pnlFilters.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioBuhar);
            this.groupBox1.Controls.Add(this.radioSu);
            this.groupBox1.Controls.Add(this.radioElektrik);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(380, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 65);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tüketim Tipi";
            // 
            // radioBuhar
            // 
            this.radioBuhar.AutoSize = true;
            this.radioBuhar.Location = new System.Drawing.Point(110, 25);
            this.radioBuhar.Name = "radioBuhar";
            this.radioBuhar.Size = new System.Drawing.Size(71, 24);
            this.radioBuhar.TabIndex = 2;
            this.radioBuhar.Text = "Buhar";
            this.radioBuhar.UseVisualStyleBackColor = true;
            this.radioBuhar.CheckedChanged += new System.EventHandler(this.radioConsumption_CheckedChanged);
            // 
            // radioSu
            // 
            this.radioSu.AutoSize = true;
            this.radioSu.Location = new System.Drawing.Point(60, 25);
            this.radioSu.Name = "radioSu";
            this.radioSu.Size = new System.Drawing.Size(49, 24);
            this.radioSu.TabIndex = 1;
            this.radioSu.Text = "Su";
            this.radioSu.UseVisualStyleBackColor = true;
            this.radioSu.CheckedChanged += new System.EventHandler(this.radioConsumption_CheckedChanged);
            // 
            // radioElektrik
            // 
            this.radioElektrik.AutoSize = true;
            this.radioElektrik.Checked = true;
            this.radioElektrik.Location = new System.Drawing.Point(10, 25);
            this.radioElektrik.Name = "radioElektrik";
            this.radioElektrik.Size = new System.Drawing.Size(51, 24);
            this.radioElektrik.TabIndex = 0;
            this.radioElektrik.TabStop = true;
            this.radioElektrik.Text = "Elk";
            this.radioElektrik.UseVisualStyleBackColor = true;
            this.radioElektrik.CheckedChanged += new System.EventHandler(this.radioConsumption_CheckedChanged);
            // 
            // btnRaporOlustur
            // 
            this.btnRaporOlustur.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.btnRaporOlustur.Location = new System.Drawing.Point(600, 20);
            this.btnRaporOlustur.Name = "btnRaporOlustur";
            this.btnRaporOlustur.Size = new System.Drawing.Size(150, 40);
            this.btnRaporOlustur.TabIndex = 2;
            this.btnRaporOlustur.Text = "Raporla";
            this.btnRaporOlustur.UseVisualStyleBackColor = true;
            this.btnRaporOlustur.Click += new System.EventHandler(this.btnRaporOlustur_Click);
            // 
            // dtpEndTime
            // 
            this.dtpEndTime.Location = new System.Drawing.Point(15, 45);
            this.dtpEndTime.Name = "dtpEndTime";
            this.dtpEndTime.Size = new System.Drawing.Size(250, 27);
            this.dtpEndTime.TabIndex = 1;
            // 
            // dtpStartTime
            // 
            this.dtpStartTime.Location = new System.Drawing.Point(15, 12);
            this.dtpStartTime.Name = "dtpStartTime";
            this.dtpStartTime.Size = new System.Drawing.Size(250, 27);
            this.dtpStartTime.TabIndex = 0;
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.dgvReport);
            this.pnlMain.Controls.Add(this.pnlSelection);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 80);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(1200, 620);
            this.pnlMain.TabIndex = 1;
            // 
            // dgvReport
            // 
            this.dgvReport.AllowUserToAddRows = false;
            this.dgvReport.AllowUserToDeleteRows = false;
            this.dgvReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvReport.Location = new System.Drawing.Point(0, 250);
            this.dgvReport.Name = "dgvReport";
            this.dgvReport.ReadOnly = true;
            this.dgvReport.RowHeadersWidth = 51;
            this.dgvReport.RowTemplate.Height = 29;
            this.dgvReport.Size = new System.Drawing.Size(1200, 370);
            this.dgvReport.TabIndex = 1;
            // 
            // pnlSelection
            // 
            this.pnlSelection.Controls.Add(this.flpMachineGroups);
            this.pnlSelection.Controls.Add(this.panel1);
            this.pnlSelection.Controls.Add(this.listBoxSeciliMakineler);
            this.pnlSelection.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSelection.Location = new System.Drawing.Point(0, 0);
            this.pnlSelection.Name = "pnlSelection";
            this.pnlSelection.Size = new System.Drawing.Size(1200, 250);
            this.pnlSelection.TabIndex = 2;
            // 
            // flpMachineGroups
            // 
            this.flpMachineGroups.AutoScroll = true;
            this.flpMachineGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpMachineGroups.Location = new System.Drawing.Point(0, 0);
            this.flpMachineGroups.Name = "flpMachineGroups";
            this.flpMachineGroups.Padding = new System.Windows.Forms.Padding(5);
            this.flpMachineGroups.Size = new System.Drawing.Size(830, 250);
            this.flpMachineGroups.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnRemoveAll);
            this.panel1.Controls.Add(this.btnAddAll);
            this.panel1.Controls.Add(this.btnRemove);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(830, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(70, 250);
            this.panel1.TabIndex = 1;
            // 
            // btnRemoveAll
            // 
            this.btnRemoveAll.Location = new System.Drawing.Point(10, 180);
            this.btnRemoveAll.Name = "btnRemoveAll";
            this.btnRemoveAll.Size = new System.Drawing.Size(50, 30);
            this.btnRemoveAll.TabIndex = 3;
            this.btnRemoveAll.Text = "<<";
            this.btnRemoveAll.UseVisualStyleBackColor = true;
            this.btnRemoveAll.Click += new System.EventHandler(this.btnRemoveAll_Click);
            // 
            // btnAddAll
            // 
            this.btnAddAll.Location = new System.Drawing.Point(10, 140);
            this.btnAddAll.Name = "btnAddAll";
            this.btnAddAll.Size = new System.Drawing.Size(50, 30);
            this.btnAddAll.TabIndex = 2;
            this.btnAddAll.Text = ">>";
            this.btnAddAll.UseVisualStyleBackColor = true;
            this.btnAddAll.Click += new System.EventHandler(this.btnAddAll_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(10, 80);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(50, 30);
            this.btnRemove.TabIndex = 1;
            this.btnRemove.Text = "<";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(10, 40);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(50, 30);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = ">";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // listBoxSeciliMakineler
            // 
            this.listBoxSeciliMakineler.Dock = System.Windows.Forms.DockStyle.Right;
            this.listBoxSeciliMakineler.FormattingEnabled = true;
            this.listBoxSeciliMakineler.ItemHeight = 20;
            this.listBoxSeciliMakineler.Location = new System.Drawing.Point(900, 0);
            this.listBoxSeciliMakineler.Name = "listBoxSeciliMakineler";
            this.listBoxSeciliMakineler.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxSeciliMakineler.Size = new System.Drawing.Size(300, 250);
            this.listBoxSeciliMakineler.TabIndex = 0;
            // 
            // GenelUretimRaporu_Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlFilters);
            this.Name = "GenelUretimRaporu_Control";
            this.Size = new System.Drawing.Size(1200, 700);
            this.Load += new System.EventHandler(this.GenelUretimRaporu_Control_Load);
            this.pnlFilters.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).EndInit();
            this.pnlSelection.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlFilters;
        private System.Windows.Forms.Button btnRaporOlustur;
        private System.Windows.Forms.DateTimePicker dtpEndTime;
        private System.Windows.Forms.DateTimePicker dtpStartTime;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioBuhar;
        private System.Windows.Forms.RadioButton radioSu;
        private System.Windows.Forms.RadioButton radioElektrik;
        private System.Windows.Forms.DataGridView dgvReport;
        private System.Windows.Forms.Panel pnlSelection;
        private System.Windows.Forms.FlowLayoutPanel flpMachineGroups;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnRemoveAll;
        private System.Windows.Forms.Button btnAddAll;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ListBox listBoxSeciliMakineler;
    }
}