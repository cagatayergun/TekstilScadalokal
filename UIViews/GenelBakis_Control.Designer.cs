namespace TekstilScada.UI.Views
{
    partial class GenelBakis_Control
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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.flpTopKpis = new System.Windows.Forms.FlowLayoutPanel();
            this.flpMachineGroups = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.flpTopKpis);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(10, 10);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1180, 100);
            this.pnlHeader.TabIndex = 0;
            // 
            // flpTopKpis
            // 
            this.flpTopKpis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpTopKpis.Location = new System.Drawing.Point(0, 0);
            this.flpTopKpis.Name = "flpTopKpis";
            this.flpTopKpis.Size = new System.Drawing.Size(1180, 100);
            this.flpTopKpis.TabIndex = 0;
            // 
            // flpMachineGroups
            // 
            this.flpMachineGroups.AutoScroll = true;
            this.flpMachineGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpMachineGroups.Location = new System.Drawing.Point(10, 110);
            this.flpMachineGroups.Name = "flpMachineGroups";
            this.flpMachineGroups.Padding = new System.Windows.Forms.Padding(5);
            this.flpMachineGroups.Size = new System.Drawing.Size(1180, 680);
            this.flpMachineGroups.TabIndex = 1;
            // 
            // GenelBakis_Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Controls.Add(this.flpMachineGroups);
            this.Controls.Add(this.pnlHeader);
            this.Name = "GenelBakis_Control";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(1200, 800);
            this.Load += new System.EventHandler(this.GenelBakis_Control_Load);
            this.pnlHeader.ResumeLayout(false);
            this.ResumeLayout(false);
        }
        #endregion
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.FlowLayoutPanel flpMachineGroups;
        private System.Windows.Forms.FlowLayoutPanel flpTopKpis;
    }
}