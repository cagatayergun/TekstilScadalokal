// UI/Views/PlcOperatorSettings_Control.Designer.cs
namespace TekstilScada.UI.Views
{
    partial class PlcOperatorSettings_Control
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) { components.Dispose(); }
            base.Dispose(disposing);
        }
        #region Component Designer generated code
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.cmbMachines = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbSlot = new System.Windows.Forms.ComboBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnRead = new System.Windows.Forms.Button();
            this.dgvOperators = new System.Windows.Forms.DataGridView();
            this.btnDelete = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOperators)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Hedef Makine:";
            // 
            // cmbMachines
            // 
            this.cmbMachines.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMachines.FormattingEnabled = true;
            this.cmbMachines.Location = new System.Drawing.Point(120, 17);
            this.cmbMachines.Name = "cmbMachines";
            this.cmbMachines.Size = new System.Drawing.Size(250, 28);
            this.cmbMachines.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(390, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Kullanıcı Sırası:";
            // 
            // cmbSlot
            // 
            this.cmbSlot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSlot.FormattingEnabled = true;
            this.cmbSlot.Location = new System.Drawing.Point(500, 17);
            this.cmbSlot.Name = "cmbSlot";
            this.cmbSlot.Size = new System.Drawing.Size(60, 28);
            this.cmbSlot.TabIndex = 3;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(580, 17);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(200, 29);
            this.btnSend.TabIndex = 4;
            this.btnSend.Text = "Seçili Şablonu PLC\'ye Gönder ->";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnRead
            // 
            this.btnRead.Location = new System.Drawing.Point(580, 52);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(200, 29);
            this.btnRead.TabIndex = 5;
            this.btnRead.Text = "<- PLC\'deki Operatörü Oku";
            this.btnRead.UseVisualStyleBackColor = true;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // dgvOperators
            // 
            this.dgvOperators.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOperators.Location = new System.Drawing.Point(20, 100);
            this.dgvOperators.Name = "dgvOperators";
            this.dgvOperators.RowHeadersWidth = 51;
            this.dgvOperators.RowTemplate.Height = 29;
            this.dgvOperators.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOperators.Size = new System.Drawing.Size(760, 350);
            this.dgvOperators.TabIndex = 6;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(650, 460);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(130, 29);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Text = "Seçili Şablonu Sil";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // PlcOperatorSettings_Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.dgvOperators);
            this.Controls.Add(this.btnRead);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.cmbSlot);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbMachines);
            this.Controls.Add(this.label1);
            this.Name = "PlcOperatorSettings_Control";
            this.Size = new System.Drawing.Size(800, 500);
            this.Load += new System.EventHandler(this.PlcOperatorSettings_Control_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOperators)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbMachines;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbSlot;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.DataGridView dgvOperators;
        private System.Windows.Forms.Button btnDelete;
    }
}
