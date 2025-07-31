// UI/Views/Prosesİzleme_Control.Designer.cs
namespace TekstilScada.UI.Views
{
    partial class Prosesİzleme_Control
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                // Abone olunan event'leri temizlemek çok önemlidir, bellek sızıntısını önler.
                if (_pollingService != null)
                {
                    _pollingService.OnMachineDataRefreshed -= PollingService_OnMachineDataRefreshed;
                    _pollingService.OnMachineConnectionStateChanged -= PollingService_OnMachineConnectionStateChanged;
                }
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.flowLayoutPanelMachines = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // flowLayoutPanelMachines
            // 
            this.flowLayoutPanelMachines.AutoScroll = true;
            this.flowLayoutPanelMachines.BackColor = System.Drawing.SystemColors.ControlDark;
            this.flowLayoutPanelMachines.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelMachines.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelMachines.Name = "flowLayoutPanelMachines";
            this.flowLayoutPanelMachines.Padding = new System.Windows.Forms.Padding(10);
            this.flowLayoutPanelMachines.Size = new System.Drawing.Size(1024, 768);
            this.flowLayoutPanelMachines.TabIndex = 0;
            // 
            // Prosesİzleme_Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanelMachines);
            this.Name = "Prosesİzleme_Control";
            this.Size = new System.Drawing.Size(1024, 768);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelMachines;
    }
}