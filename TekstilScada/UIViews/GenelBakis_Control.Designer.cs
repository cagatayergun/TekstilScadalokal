// Bu dosyanın içeriğini tamamen aşağıdakiyle değiştirin
namespace TekstilScada.UI.Views
{
    partial class GenelBakis_Control
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing) { if (disposing && (components != null)) { components.Dispose(); } base.Dispose(disposing); }
        #region Component Designer generated code
        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.flpTopKpis = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.tlpMainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.flpMachineGroups = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.gbTopAlarms = new System.Windows.Forms.GroupBox();
            this.formsPlotTopAlarms = new ScottPlot.WinForms.FormsPlot();
            this.gbHourlyConsumption = new System.Windows.Forms.GroupBox();
            this.formsPlotHourly = new ScottPlot.WinForms.FormsPlot();
            this.pnlHeader.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.tlpMainLayout.SuspendLayout();
            this.pnlSidebar.SuspendLayout();
            this.gbTopAlarms.SuspendLayout();
            this.gbHourlyConsumption.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.Controls.Add(this.flpTopKpis);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(10, 10);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Padding = new System.Windows.Forms.Padding(5);
            this.pnlHeader.Size = new System.Drawing.Size(1180, 100);
            this.pnlHeader.TabIndex = 0;
            // 
            // flpTopKpis
            // 
            this.flpTopKpis.BackColor = System.Drawing.Color.White;
            this.flpTopKpis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpTopKpis.Location = new System.Drawing.Point(5, 5);
            this.flpTopKpis.Name = "flpTopKpis";
            this.flpTopKpis.Size = new System.Drawing.Size(1170, 90);
            this.flpTopKpis.TabIndex = 0;
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.tlpMainLayout);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(10, 110);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(1180, 680);
            this.pnlMain.TabIndex = 2;
            // 
            // tlpMainLayout
            // 
            this.tlpMainLayout.ColumnCount = 2;
            this.tlpMainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 350F));
            this.tlpMainLayout.Controls.Add(this.flpMachineGroups, 0, 0);
            this.tlpMainLayout.Controls.Add(this.pnlSidebar, 1, 0);
            this.tlpMainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMainLayout.Location = new System.Drawing.Point(0, 0);
            this.tlpMainLayout.Name = "tlpMainLayout";
            this.tlpMainLayout.RowCount = 1;
            this.tlpMainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMainLayout.Size = new System.Drawing.Size(1180, 680);
            this.tlpMainLayout.TabIndex = 0;
            // 
            // flpMachineGroups
            // 
            this.flpMachineGroups.AutoScroll = true;
            this.flpMachineGroups.BackColor = System.Drawing.SystemColors.Control;
            this.flpMachineGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpMachineGroups.Location = new System.Drawing.Point(3, 3);
            this.flpMachineGroups.Name = "flpMachineGroups";
            this.flpMachineGroups.Padding = new System.Windows.Forms.Padding(5);
            this.flpMachineGroups.Size = new System.Drawing.Size(824, 674);
            this.flpMachineGroups.TabIndex = 1;
            // 
            // pnlSidebar
            // 
            this.pnlSidebar.Controls.Add(this.gbTopAlarms);
            this.pnlSidebar.Controls.Add(this.gbHourlyConsumption);
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSidebar.Location = new System.Drawing.Point(833, 3);
            this.pnlSidebar.Name = "pnlSidebar";
            this.pnlSidebar.Size = new System.Drawing.Size(344, 674);
            this.pnlSidebar.TabIndex = 2;
            // 
            // gbTopAlarms
            // 
            this.gbTopAlarms.Controls.Add(this.formsPlotTopAlarms);
            this.gbTopAlarms.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbTopAlarms.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.gbTopAlarms.Location = new System.Drawing.Point(0, 300);
            this.gbTopAlarms.Name = "gbTopAlarms";
            this.gbTopAlarms.Size = new System.Drawing.Size(344, 300);
            this.gbTopAlarms.TabIndex = 1;
            this.gbTopAlarms.TabStop = false;
            this.gbTopAlarms.Text = "Son 24 Saatin Popüler Alarmları";
            // 
            // formsPlotTopAlarms
            // 
            this.formsPlotTopAlarms.DisplayScale = 1F;
            this.formsPlotTopAlarms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formsPlotTopAlarms.Location = new System.Drawing.Point(3, 21);
            this.formsPlotTopAlarms.Name = "formsPlotTopAlarms";
            this.formsPlotTopAlarms.Size = new System.Drawing.Size(338, 276);
            this.formsPlotTopAlarms.TabIndex = 0;
            // 
            // gbHourlyConsumption
            // 
            this.gbHourlyConsumption.Controls.Add(this.formsPlotHourly);
            this.gbHourlyConsumption.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbHourlyConsumption.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.gbHourlyConsumption.Location = new System.Drawing.Point(0, 0);
            this.gbHourlyConsumption.Name = "gbHourlyConsumption";
            this.gbHourlyConsumption.Size = new System.Drawing.Size(344, 300);
            this.gbHourlyConsumption.TabIndex = 0;
            this.gbHourlyConsumption.TabStop = false;
            this.gbHourlyConsumption.Text = "Saatlik Toplam Tüketim (Bugün)";
            // 
            // formsPlotHourly
            // 
            this.formsPlotHourly.DisplayScale = 1F;
            this.formsPlotHourly.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formsPlotHourly.Location = new System.Drawing.Point(3, 21);
            this.formsPlotHourly.Name = "formsPlotHourly";
            this.formsPlotHourly.Size = new System.Drawing.Size(338, 276);
            this.formsPlotHourly.TabIndex = 0;
            // 
            // GenelBakis_Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlHeader);
            this.Name = "GenelBakis_Control";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(1200, 800);
            this.Load += new System.EventHandler(this.GenelBakis_Control_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.tlpMainLayout.ResumeLayout(false);
            this.pnlSidebar.ResumeLayout(false);
            this.gbTopAlarms.ResumeLayout(false);
            this.gbHourlyConsumption.ResumeLayout(false);
            this.ResumeLayout(false);
        }
        #endregion
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.FlowLayoutPanel flpTopKpis;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.TableLayoutPanel tlpMainLayout;
        private System.Windows.Forms.FlowLayoutPanel flpMachineGroups;
        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.GroupBox gbTopAlarms;
        private ScottPlot.WinForms.FormsPlot formsPlotTopAlarms;
        private System.Windows.Forms.GroupBox gbHourlyConsumption;
        private ScottPlot.WinForms.FormsPlot formsPlotHourly;
    }
}