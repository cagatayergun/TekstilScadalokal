// Bu dosyanın içeriğini tamamen aşağıdakiyle değiştirin
namespace TekstilScada.UI.Controls
{
    partial class DashboardMachineCard_Control
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing) { if (disposing && (components != null)) { components.Dispose(); } base.Dispose(disposing); }
        #region Component Designer generated code
        private void InitializeComponent()
        {
            pnlStatusIndicator = new Panel();
            lblMachineName = new Label();
            lblStatus = new Label();
            lblRecipeName = new Label();
            lblBatchId = new Label();
            pnlSparkline = new Panel();
            lblTemperature = new Label();
            gaugeRpm = new CircularProgressBar.CircularProgressBar();
            SuspendLayout();
            // 
            // pnlStatusIndicator
            // 
            pnlStatusIndicator.BackColor = Color.SlateGray;
            pnlStatusIndicator.Dock = DockStyle.Left;
            pnlStatusIndicator.Location = new Point(0, 0);
            pnlStatusIndicator.Margin = new Padding(4, 3, 4, 3);
            pnlStatusIndicator.Name = "pnlStatusIndicator";
            pnlStatusIndicator.Size = new Size(12, 173);
            pnlStatusIndicator.TabIndex = 0;
            // 
            // lblMachineName
            // 
            lblMachineName.AutoSize = true;
            lblMachineName.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblMachineName.Location = new Point(23, 6);
            lblMachineName.Margin = new Padding(4, 0, 4, 0);
            lblMachineName.Name = "lblMachineName";
            lblMachineName.Size = new Size(97, 21);
            lblMachineName.TabIndex = 1;
            lblMachineName.Text = "Makine Adı";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold);
            lblStatus.Location = new Point(24, 32);
            lblStatus.Margin = new Padding(4, 0, 4, 0);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(69, 17);
            lblStatus.TabIndex = 2;
            lblStatus.Text = "DURUYOR";
            // 
            // lblRecipeName
            // 
            lblRecipeName.Font = new Font("Segoe UI", 8.25F);
            lblRecipeName.Location = new Point(152, 9);
            lblRecipeName.Margin = new Padding(4, 0, 4, 0);
            lblRecipeName.Name = "lblRecipeName";
            lblRecipeName.Size = new Size(131, 21);
            lblRecipeName.TabIndex = 4;
            lblRecipeName.Text = "Reçete: -";
            lblRecipeName.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblBatchId
            // 
            lblBatchId.Font = new Font("Segoe UI", 8.25F);
            lblBatchId.Location = new Point(152, 31);
            lblBatchId.Margin = new Padding(4, 0, 4, 0);
            lblBatchId.Name = "lblBatchId";
            lblBatchId.Size = new Size(131, 21);
            lblBatchId.TabIndex = 8;
            lblBatchId.Text = "Parti: -";
            lblBatchId.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlSparkline
            // 
            pnlSparkline.Location = new Point(21, 127);
            pnlSparkline.Margin = new Padding(4, 3, 4, 3);
            pnlSparkline.Name = "pnlSparkline";
            pnlSparkline.Size = new Size(261, 40);
            pnlSparkline.TabIndex = 9;
            pnlSparkline.Paint += pnlSparkline_Paint;
            // 
            // lblTemperature
            // 
            lblTemperature.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold);
            lblTemperature.ForeColor = Color.FromArgb(64, 64, 64);
            lblTemperature.Location = new Point(22, 66);
            lblTemperature.Margin = new Padding(4, 0, 4, 0);
            lblTemperature.Name = "lblTemperature";
            lblTemperature.Size = new Size(130, 51);
            lblTemperature.TabIndex = 11;
            lblTemperature.Text = "25 °C";
            // 
            // gaugeRpm
            // 
            gaugeRpm.AnimationFunction = WinFormAnimation.KnownAnimationFunctions.Liner;
            gaugeRpm.AnimationSpeed = 500;
            gaugeRpm.BackColor = Color.Transparent;
            gaugeRpm.Font = new Font("Segoe UI Black", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 162);
            gaugeRpm.ForeColor = Color.FromArgb(64, 64, 64);
            gaugeRpm.InnerColor = Color.White;
            gaugeRpm.InnerMargin = 2;
            gaugeRpm.InnerWidth = -1;
            gaugeRpm.Location = new Point(201, 54);
            gaugeRpm.Margin = new Padding(3, 2, 3, 2);
            gaugeRpm.MarqueeAnimationSpeed = 2000;
            gaugeRpm.Maximum = 500;
            gaugeRpm.Name = "gaugeRpm";
            gaugeRpm.OuterColor = Color.FromArgb(224, 224, 224);
            gaugeRpm.OuterMargin = -25;
            gaugeRpm.OuterWidth = 26;
            gaugeRpm.ProgressColor = Color.FromArgb(46, 204, 113);
            gaugeRpm.ProgressWidth = 18;
            gaugeRpm.SecondaryFont = new Font("Segoe UI Black", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 162);
            gaugeRpm.Size = new Size(78, 68);
            gaugeRpm.StartAngle = 135;
            gaugeRpm.SubscriptColor = Color.FromArgb(100, 100, 100);
            gaugeRpm.SubscriptMargin = new Padding(-4, -37, 0, 0);
            gaugeRpm.SubscriptText = "RPM";
            gaugeRpm.SuperscriptColor = Color.FromArgb(166, 166, 166);
            gaugeRpm.SuperscriptMargin = new Padding(0, 0, 50, 0);
            gaugeRpm.SuperscriptText = "";
            gaugeRpm.TabIndex = 12;
            gaugeRpm.Text = "0";
            gaugeRpm.TextMargin = new Padding(7, 25, 0, 0);
            gaugeRpm.Value = 68;
            // 
            // DashboardMachineCard_Control
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(gaugeRpm);
            Controls.Add(lblTemperature);
            Controls.Add(pnlSparkline);
            Controls.Add(lblBatchId);
            Controls.Add(lblRecipeName);
            Controls.Add(lblStatus);
            Controls.Add(lblMachineName);
            Controls.Add(pnlStatusIndicator);
            Margin = new Padding(9);
            Name = "DashboardMachineCard_Control";
            Size = new Size(292, 173);
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion
        private System.Windows.Forms.Panel pnlStatusIndicator;
        private System.Windows.Forms.Label lblMachineName;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblRecipeName;
        private System.Windows.Forms.Label lblBatchId;
        private System.Windows.Forms.Panel pnlSparkline;
        private System.Windows.Forms.Label lblTemperature;
        private CircularProgressBar.CircularProgressBar gaugeRpm;
    }
}