// UI/Views/MachineSettings_Control.Designer.cs
namespace TekstilScada.UI.Views
{
    partial class MachineSettings_Control
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
            dgvMachines = new DataGridView();
            groupBox1 = new GroupBox();
            txtMachineSubType = new TextBox();
            label9 = new Label();
            txtFtpPassword = new TextBox();
            label8 = new Label();
            txtFtpUsername = new TextBox();
            label7 = new Label();
            cmbMachineType = new ComboBox();
            label6 = new Label();
            btnDelete = new Button();
            btnSave = new Button();
            btnNew = new Button();
            chkIsEnabled = new CheckBox();
            txtVncAddress = new TextBox();
            label5 = new Label();
            txtPort = new TextBox();
            label4 = new Label();
            txtIpAddress = new TextBox();
            label3 = new Label();
            txtMachineName = new TextBox();
            label2 = new Label();
            txtMachineId = new TextBox();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvMachines).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // dgvMachines
            // 
            dgvMachines.AllowUserToAddRows = false;
            dgvMachines.AllowUserToDeleteRows = false;
            dgvMachines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMachines.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMachines.Dock = DockStyle.Fill;
            dgvMachines.Location = new Point(0, 0);
            dgvMachines.Margin = new Padding(3, 2, 3, 2);
            dgvMachines.MultiSelect = false;
            dgvMachines.Name = "dgvMachines";
            dgvMachines.ReadOnly = true;
            dgvMachines.RowHeadersWidth = 51;
            dgvMachines.RowTemplate.Height = 29;
            dgvMachines.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMachines.Size = new Size(700, 412);
            dgvMachines.TabIndex = 0;
            dgvMachines.SelectionChanged += dgvMachines_SelectionChanged;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(txtMachineSubType);
            groupBox1.Controls.Add(label9);
            groupBox1.Controls.Add(txtFtpPassword);
            groupBox1.Controls.Add(label8);
            groupBox1.Controls.Add(txtFtpUsername);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(cmbMachineType);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(btnDelete);
            groupBox1.Controls.Add(btnSave);
            groupBox1.Controls.Add(btnNew);
            groupBox1.Controls.Add(chkIsEnabled);
            groupBox1.Controls.Add(txtVncAddress);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(txtPort);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(txtIpAddress);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(txtMachineName);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(txtMachineId);
            groupBox1.Controls.Add(label1);
            groupBox1.Dock = DockStyle.Bottom;
            groupBox1.Location = new Point(0, 224);
            groupBox1.Margin = new Padding(3, 2, 3, 2);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(3, 2, 3, 2);
            groupBox1.Size = new Size(700, 188);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Makine Bilgileri";
            // 
            // txtMachineSubType
            // 
            txtMachineSubType.Location = new Point(455, 52);
            txtMachineSubType.Margin = new Padding(3, 2, 3, 2);
            txtMachineSubType.Name = "txtMachineSubType";
            txtMachineSubType.Size = new Size(219, 23);
            txtMachineSubType.TabIndex = 21;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(368, 56);
            label9.Name = "label9";
            label9.Size = new Size(80, 15);
            label9.TabIndex = 20;
            label9.Text = "Makine Tipi 2:";
            // 
            // txtFtpPassword
            // 
            txtFtpPassword.Location = new Point(455, 112);
            txtFtpPassword.Margin = new Padding(3, 2, 3, 2);
            txtFtpPassword.Name = "txtFtpPassword";
            txtFtpPassword.PasswordChar = '*';
            txtFtpPassword.Size = new Size(219, 23);
            txtFtpPassword.TabIndex = 19;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(368, 115);
            label8.Name = "label8";
            label8.Size = new Size(55, 15);
            label8.TabIndex = 18;
            label8.Text = "FTP Şifre:";
            // 
            // txtFtpUsername
            // 
            txtFtpUsername.Location = new Point(455, 82);
            txtFtpUsername.Margin = new Padding(3, 2, 3, 2);
            txtFtpUsername.Name = "txtFtpUsername";
            txtFtpUsername.Size = new Size(219, 23);
            txtFtpUsername.TabIndex = 17;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(368, 85);
            label7.Name = "label7";
            label7.Size = new Size(77, 15);
            label7.TabIndex = 16;
            label7.Text = "FTP Kullanıcı:";
            // 
            // cmbMachineType
            // 
            cmbMachineType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMachineType.FormattingEnabled = true;
            cmbMachineType.Location = new Point(455, 22);
            cmbMachineType.Margin = new Padding(3, 2, 3, 2);
            cmbMachineType.Name = "cmbMachineType";
            cmbMachineType.Size = new Size(219, 23);
            cmbMachineType.TabIndex = 15;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(368, 26);
            label6.Name = "label6";
            label6.Size = new Size(71, 15);
            label6.TabIndex = 14;
            label6.Text = "Makine Tipi:";
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(592, 154);
            btnDelete.Margin = new Padding(3, 2, 3, 2);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(82, 22);
            btnDelete.TabIndex = 13;
            btnDelete.Text = "Sil";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(504, 154);
            btnSave.Margin = new Padding(3, 2, 3, 2);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(82, 22);
            btnSave.TabIndex = 12;
            btnSave.Text = "Kaydet";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnNew
            // 
            btnNew.Location = new Point(416, 154);
            btnNew.Margin = new Padding(3, 2, 3, 2);
            btnNew.Name = "btnNew";
            btnNew.Size = new Size(82, 22);
            btnNew.TabIndex = 11;
            btnNew.Text = "Yeni";
            btnNew.UseVisualStyleBackColor = true;
            btnNew.Click += btnNew_Click;
            // 
            // chkIsEnabled
            // 
            chkIsEnabled.AutoSize = true;
            chkIsEnabled.Location = new Point(114, 142);
            chkIsEnabled.Margin = new Padding(3, 2, 3, 2);
            chkIsEnabled.Name = "chkIsEnabled";
            chkIsEnabled.Size = new Size(110, 19);
            chkIsEnabled.TabIndex = 10;
            chkIsEnabled.Text = "İzleme Aktif Mi?";
            chkIsEnabled.UseVisualStyleBackColor = true;
            // 
            // txtVncAddress
            // 
            txtVncAddress.Location = new Point(114, 112);
            txtVncAddress.Margin = new Padding(3, 2, 3, 2);
            txtVncAddress.Name = "txtVncAddress";
            txtVncAddress.Size = new Size(219, 23);
            txtVncAddress.TabIndex = 9;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(18, 115);
            label5.Name = "label5";
            label5.Size = new Size(70, 15);
            label5.TabIndex = 8;
            label5.Text = "VNC Adresi:";
            // 
            // txtPort
            // 
            txtPort.Location = new Point(223, 82);
            txtPort.Margin = new Padding(3, 2, 3, 2);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(110, 23);
            txtPort.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(182, 85);
            label4.Name = "label4";
            label4.Size = new Size(32, 15);
            label4.TabIndex = 6;
            label4.Text = "Port:";
            // 
            // txtIpAddress
            // 
            txtIpAddress.Location = new Point(114, 82);
            txtIpAddress.Margin = new Padding(3, 2, 3, 2);
            txtIpAddress.Name = "txtIpAddress";
            txtIpAddress.Size = new Size(64, 23);
            txtIpAddress.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(18, 85);
            label3.Name = "label3";
            label3.Size = new Size(56, 15);
            label3.TabIndex = 4;
            label3.Text = "IP Adresi:";
            // 
            // txtMachineName
            // 
            txtMachineName.Location = new Point(114, 52);
            txtMachineName.Margin = new Padding(3, 2, 3, 2);
            txtMachineName.Name = "txtMachineName";
            txtMachineName.Size = new Size(219, 23);
            txtMachineName.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(18, 55);
            label2.Name = "label2";
            label2.Size = new Size(70, 15);
            label2.TabIndex = 2;
            label2.Text = "Makine Adı:";
            // 
            // txtMachineId
            // 
            txtMachineId.Location = new Point(114, 22);
            txtMachineId.Margin = new Padding(3, 2, 3, 2);
            txtMachineId.Name = "txtMachineId";
            txtMachineId.Size = new Size(219, 23);
            txtMachineId.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(18, 25);
            label1.Name = "label1";
            label1.Size = new Size(63, 15);
            label1.TabIndex = 0;
            label1.Text = "Makine ID:";
            // 
            // MachineSettings_Control
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(groupBox1);
            Controls.Add(dgvMachines);
            Margin = new Padding(3, 2, 3, 2);
            Name = "MachineSettings_Control";
            Size = new Size(700, 412);
            Load += MachineSettings_Control_Load;
            ((System.ComponentModel.ISupportInitialize)dgvMachines).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }
        #endregion
        private System.Windows.Forms.DataGridView dgvMachines;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.CheckBox chkIsEnabled;
        private System.Windows.Forms.TextBox txtVncAddress;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtIpAddress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMachineName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMachineId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbMachineType;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtFtpPassword;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtFtpUsername;
        private System.Windows.Forms.Label label7;
        private TextBox txtMachineSubType;
        private Label label9;
    }
}
