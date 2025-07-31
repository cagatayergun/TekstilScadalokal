namespace TekstilScada.UI.Controls
{
    partial class KpiCard_Control
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
            this.lblKpiValue = new System.Windows.Forms.Label();
            this.lblKpiTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblKpiValue
            // 
            this.lblKpiValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblKpiValue.Font = new System.Drawing.Font("Segoe UI", 19.8F, System.Drawing.FontStyle.Bold);
            this.lblKpiValue.ForeColor = System.Drawing.Color.White;
            this.lblKpiValue.Location = new System.Drawing.Point(0, 25);
            this.lblKpiValue.Name = "lblKpiValue";
            this.lblKpiValue.Size = new System.Drawing.Size(220, 55);
            this.lblKpiValue.TabIndex = 1;
            this.lblKpiValue.Text = "0";
            this.lblKpiValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblKpiTitle
            // 
            this.lblKpiTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblKpiTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblKpiTitle.ForeColor = System.Drawing.Color.White;
            this.lblKpiTitle.Location = new System.Drawing.Point(0, 0);
            this.lblKpiTitle.Name = "lblKpiTitle";
            this.lblKpiTitle.Size = new System.Drawing.Size(220, 25);
            this.lblKpiTitle.TabIndex = 0;
            this.lblKpiTitle.Text = "KPI BAŞLIĞI";
            this.lblKpiTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // KpiCard_Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.Controls.Add(this.lblKpiValue);
            this.Controls.Add(this.lblKpiTitle);
            this.Margin = new System.Windows.Forms.Padding(10);
            this.Name = "KpiCard_Control";
            this.Size = new System.Drawing.Size(220, 80);
            this.ResumeLayout(false);
        }
        #endregion
        private System.Windows.Forms.Label lblKpiValue;
        private System.Windows.Forms.Label lblKpiTitle;
    }
}