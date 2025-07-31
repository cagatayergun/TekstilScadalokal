namespace TekstilScada.UI.Views
{
    partial class TrendAnaliz_Control
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlFilters = new System.Windows.Forms.Panel();
            this.btnGenerateChart = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkRpm = new System.Windows.Forms.CheckBox();
            this.chkWaterLevel = new System.Windows.Forms.CheckBox();
            this.chkTemperature = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.clbMachines = new System.Windows.Forms.CheckedListBox();
            this.dtpEndTime = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpStartTime = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.formsPlot1 = new ScottPlot.WinForms.FormsPlot();
            this.pnlFilters.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFilters
            // 
            this.pnlFilters.Controls.Add(this.btnGenerateChart);
            this.pnlFilters.Controls.Add(this.groupBox2);
            this.pnlFilters.Controls.Add(this.groupBox1);
            this.pnlFilters.Controls.Add(this.dtpEndTime);
            this.pnlFilters.Controls.Add(this.label2);
            this.pnlFilters.Controls.Add(this.dtpStartTime);
            this.pnlFilters.Controls.Add(this.label1);
            this.pnlFilters.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlFilters.Location = new System.Drawing.Point(0, 0);
            this.pnlFilters.Name = "pnlFilters";
            this.pnlFilters.Padding = new System.Windows.Forms.Padding(10);
            this.pnlFilters.Size = new System.Drawing.Size(250, 600);
            this.pnlFilters.TabIndex = 0;
            // 
            // btnGenerateChart
            // 
            this.btnGenerateChart.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.btnGenerateChart.Location = new System.Drawing.Point(13, 450);
            this.btnGenerateChart.Name = "btnGenerateChart";
            this.btnGenerateChart.Size = new System.Drawing.Size(224, 40);
            this.btnGenerateChart.TabIndex = 6;
            this.btnGenerateChart.Text = "Grafiği Oluştur";
            this.btnGenerateChart.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkRpm);
            this.groupBox2.Controls.Add(this.chkWaterLevel);
            this.groupBox2.Controls.Add(this.chkTemperature);
            this.groupBox2.Location = new System.Drawing.Point(13, 320);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(224, 115);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Görüntülenecek Veriler";
            // 
            // chkRpm
            // 
            this.chkRpm.AutoSize = true;
            this.chkRpm.Location = new System.Drawing.Point(15, 80);
            this.chkRpm.Name = "chkRpm";
            this.chkRpm.Size = new System.Drawing.Size(66, 24);
            this.chkRpm.TabIndex = 2;
            this.chkRpm.Text = "Devir";
            this.chkRpm.UseVisualStyleBackColor = true;
            // 
            // chkWaterLevel
            // 
            this.chkWaterLevel.AutoSize = true;
            this.chkWaterLevel.Location = new System.Drawing.Point(15, 53);
            this.chkWaterLevel.Name = "chkWaterLevel";
            this.chkWaterLevel.Size = new System.Drawing.Size(102, 24);
            this.chkWaterLevel.TabIndex = 1;
            this.chkWaterLevel.Text = "Su Seviyesi";
            this.chkWaterLevel.UseVisualStyleBackColor = true;
            // 
            // chkTemperature
            // 
            this.chkTemperature.AutoSize = true;
            this.chkTemperature.Checked = true;
            this.chkTemperature.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTemperature.Location = new System.Drawing.Point(15, 26);
            this.chkTemperature.Name = "chkTemperature";
            this.chkTemperature.Size = new System.Drawing.Size(80, 24);
            this.chkTemperature.TabIndex = 0;
            this.chkTemperature.Text = "Sıcaklık";
            this.chkTemperature.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.clbMachines);
            this.groupBox1.Location = new System.Drawing.Point(13, 110);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(224, 200);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Makineler";
            // 
            // clbMachines
            // 
            this.clbMachines.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbMachines.FormattingEnabled = true;
            this.clbMachines.Location = new System.Drawing.Point(3, 23);
            this.clbMachines.Name = "clbMachines";
            this.clbMachines.Size = new System.Drawing.Size(218, 174);
            this.clbMachines.TabIndex = 0;
            // 
            // dtpEndTime
            // 
            this.dtpEndTime.CustomFormat = "dd.MM.yyyy HH:mm";
            this.dtpEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndTime.Location = new System.Drawing.Point(13, 77);
            this.dtpEndTime.Name = "dtpEndTime";
            this.dtpEndTime.Size = new System.Drawing.Size(224, 27);
            this.dtpEndTime.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Bitiş Tarihi:";
            // 
            // dtpStartTime
            // 
            this.dtpStartTime.CustomFormat = "dd.MM.yyyy HH:mm";
            this.dtpStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartTime.Location = new System.Drawing.Point(13, 24);
            this.dtpStartTime.Name = "dtpStartTime";
            this.dtpStartTime.Size = new System.Drawing.Size(224, 27);
            this.dtpStartTime.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Başlangıç Tarihi:";
            // 
            // formsPlot1
            // 
            this.formsPlot1.DisplayScale = 1F;
            this.formsPlot1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formsPlot1.Location = new System.Drawing.Point(250, 0);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(550, 600);
            this.formsPlot1.TabIndex = 1;
            // 
            // TrendAnaliz_Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.formsPlot1);
            this.Controls.Add(this.pnlFilters);
            this.Name = "TrendAnaliz_Control";
            this.Size = new System.Drawing.Size(800, 600);
            this.pnlFilters.ResumeLayout(false);
            this.pnlFilters.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        // DEĞİŞİKLİK: Değişken tanımlamaları buraya eklendi.
        private System.Windows.Forms.Panel pnlFilters;
        private System.Windows.Forms.Button btnGenerateChart;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkRpm;
        private System.Windows.Forms.CheckBox chkWaterLevel;
        private System.Windows.Forms.CheckBox chkTemperature;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox clbMachines;
        private System.Windows.Forms.DateTimePicker dtpEndTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpStartTime;
        private System.Windows.Forms.Label label1;
        private ScottPlot.WinForms.FormsPlot formsPlot1;
    }
}