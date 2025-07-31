// UI/Views/Ayarlar_Control.Designer.cs
namespace TekstilScada.UI.Views
{
    partial class Ayarlar_Control
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
            this.tabControlSettings = new System.Windows.Forms.TabControl();
            this.tabPageMachineSettings = new System.Windows.Forms.TabPage();
            this.tabPageUserSettings = new System.Windows.Forms.TabPage();
            this.tabPageAlarmSettings = new System.Windows.Forms.TabPage();
            this.tabPageCostSettings = new System.Windows.Forms.TabPage(); // YENİ
            // YENİ: PLC Operatörleri için sekme eklendi.
            this.tabPagePlcOperators = new System.Windows.Forms.TabPage();
            this.tabControlSettings.SuspendLayout();
            this.SuspendLayout();

            // 
            // tabControlSettings
            // 
            this.tabControlSettings.Controls.Add(this.tabPageMachineSettings);
            this.tabControlSettings.Controls.Add(this.tabPageUserSettings);
            this.tabControlSettings.Controls.Add(this.tabPageAlarmSettings);
            this.tabControlSettings.Controls.Add(this.tabPageCostSettings); // YENİ
            // YENİ: Sekme kontrole eklendi.
            this.tabControlSettings.Controls.Add(this.tabPagePlcOperators);
            this.tabControlSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlSettings.Location = new System.Drawing.Point(0, 0);
            this.tabControlSettings.Name = "tabControlSettings";
            this.tabControlSettings.SelectedIndex = 0;
            this.tabControlSettings.Size = new System.Drawing.Size(800, 600);
            this.tabControlSettings.TabIndex = 0;
            // ...
            // 
            // tabPageCostSettings (YENİ SEKMEYİ EKLE)
            // 
            this.tabPageCostSettings.Location = new System.Drawing.Point(4, 29);
            this.tabPageCostSettings.Name = "tabPageCostSettings";
            this.tabPageCostSettings.Size = new System.Drawing.Size(792, 567);
            this.tabPageCostSettings.TabIndex = 4;
            this.tabPageCostSettings.Text = "Maliyet Parametreleri";
            this.tabPageCostSettings.UseVisualStyleBackColor = true;
            // 
            // tabPageMachineSettings
            // 
            this.tabPageMachineSettings.Location = new System.Drawing.Point(4, 29);
            this.tabPageMachineSettings.Name = "tabPageMachineSettings";
            this.tabPageMachineSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMachineSettings.Size = new System.Drawing.Size(792, 567);
            this.tabPageMachineSettings.TabIndex = 0;
            this.tabPageMachineSettings.Text = "Makine Yönetimi";
            this.tabPageMachineSettings.UseVisualStyleBackColor = true;
            // 
            // tabPageUserSettings
            // 
            this.tabPageUserSettings.Location = new System.Drawing.Point(4, 29);
            this.tabPageUserSettings.Name = "tabPageUserSettings";
            this.tabPageUserSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUserSettings.Size = new System.Drawing.Size(792, 567);
            this.tabPageUserSettings.TabIndex = 1;
            this.tabPageUserSettings.Text = "Kullanıcı Yönetimi";
            this.tabPageUserSettings.UseVisualStyleBackColor = true;
            // 
            // tabPageAlarmSettings
            // 
            this.tabPageAlarmSettings.Location = new System.Drawing.Point(4, 29);
            this.tabPageAlarmSettings.Name = "tabPageAlarmSettings";
            this.tabPageAlarmSettings.Size = new System.Drawing.Size(792, 567);
            this.tabPageAlarmSettings.TabIndex = 2;
            this.tabPageAlarmSettings.Text = "Alarm Tanımlama";
            this.tabPageAlarmSettings.UseVisualStyleBackColor = true;
            // 
            // tabPagePlcOperators
            // 
            this.tabPagePlcOperators.Location = new System.Drawing.Point(4, 29);
            this.tabPagePlcOperators.Name = "tabPagePlcOperators";
            this.tabPagePlcOperators.Size = new System.Drawing.Size(792, 567);
            this.tabPagePlcOperators.TabIndex = 3;
            this.tabPagePlcOperators.Text = "PLC Operatör Yönetimi";
            this.tabPagePlcOperators.UseVisualStyleBackColor = true;

            // 
            // Ayarlar_Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlSettings);
            this.Name = "Ayarlar_Control";
            this.Size = new System.Drawing.Size(800, 600);
            this.tabControlSettings.ResumeLayout(false);
            this.ResumeLayout(false);
        }
        #endregion
        private System.Windows.Forms.TabControl tabControlSettings;
        private System.Windows.Forms.TabPage tabPageMachineSettings;
        private System.Windows.Forms.TabPage tabPageUserSettings;
        private System.Windows.Forms.TabPage tabPageAlarmSettings;
        private System.Windows.Forms.TabPage tabPagePlcOperators;
        private System.Windows.Forms.TabPage tabPageCostSettings; // YENİ
    }
}
