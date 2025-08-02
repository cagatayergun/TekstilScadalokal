using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TekstilScada.UI.Views
{
    partial class RecipeStepDesigner_Control
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing) { if (disposing && (components != null)) { components.Dispose(); } base.Dispose(disposing); }
        #region Component Designer generated code
        private void InitializeComponent()
        {
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlDesignSurface = new System.Windows.Forms.Panel();
            this.pnlToolbox = new System.Windows.Forms.Panel();
            this.btnTextbox = new System.Windows.Forms.Button();
            this.btnCheckbox = new System.Windows.Forms.Button();
            this.btnNumeric = new System.Windows.Forms.Button();
            this.btnLabel = new System.Windows.Forms.Button();
            this.pnlProperties = new System.Windows.Forms.Panel();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.btnNewLayout = new System.Windows.Forms.Button();
            this.btnSaveLayout = new System.Windows.Forms.Button();
            this.cmbStepType = new System.Windows.Forms.ComboBox();
            this.lblStepType = new System.Windows.Forms.Label();
            this.cmbMachineSubType = new System.Windows.Forms.ComboBox();
            this.lblMachineSubType = new System.Windows.Forms.Label();
            this.pnlMain.SuspendLayout();
            this.pnlToolbox.SuspendLayout();
            this.pnlProperties.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.pnlDesignSurface);
            this.pnlMain.Controls.Add(this.pnlToolbox);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 50);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(550, 400);
            this.pnlMain.TabIndex = 0;
            // 
            // pnlDesignSurface
            // 
            this.pnlDesignSurface.AllowDrop = true;
            this.pnlDesignSurface.BackColor = System.Drawing.Color.White;
            this.pnlDesignSurface.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDesignSurface.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDesignSurface.Location = new System.Drawing.Point(120, 0);
            this.pnlDesignSurface.Name = "pnlDesignSurface";
            this.pnlDesignSurface.Size = new System.Drawing.Size(430, 400);
            this.pnlDesignSurface.TabIndex = 1;
            // 
            // pnlToolbox
            // 
            this.pnlToolbox.BackColor = System.Drawing.SystemColors.ControlLight;
            this.pnlToolbox.Controls.Add(this.btnTextbox);
            this.pnlToolbox.Controls.Add(this.btnCheckbox);
            this.pnlToolbox.Controls.Add(this.btnNumeric);
            this.pnlToolbox.Controls.Add(this.btnLabel);
            this.pnlToolbox.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlToolbox.Location = new System.Drawing.Point(0, 0);
            this.pnlToolbox.Name = "pnlToolbox";
            this.pnlToolbox.Size = new System.Drawing.Size(120, 400);
            this.pnlToolbox.TabIndex = 0;
            // 
            // btnTextbox
            // 
            this.btnTextbox.Location = new System.Drawing.Point(15, 120);
            this.btnTextbox.Name = "btnTextbox";
            this.btnTextbox.Size = new System.Drawing.Size(90, 30);
            this.btnTextbox.TabIndex = 3;
            this.btnTextbox.Text = "Metin Kutusu";
            this.btnTextbox.Tag = typeof(TextBox);
            this.btnTextbox.UseVisualStyleBackColor = true;
            // 
            // btnCheckbox
            // 
            this.btnCheckbox.Location = new System.Drawing.Point(15, 85);
            this.btnCheckbox.Name = "btnCheckbox";
            this.btnCheckbox.Size = new System.Drawing.Size(90, 30);
            this.btnCheckbox.TabIndex = 2;
            this.btnCheckbox.Text = "Onay Kutusu";
            this.btnCheckbox.Tag = typeof(CheckBox);
            this.btnCheckbox.UseVisualStyleBackColor = true;
            // 
            // btnNumeric
            // 
            this.btnNumeric.Location = new System.Drawing.Point(15, 50);
            this.btnNumeric.Name = "btnNumeric";
            this.btnNumeric.Size = new System.Drawing.Size(90, 30);
            this.btnNumeric.TabIndex = 1;
            this.btnNumeric.Text = "Sayı Kutusu";
            this.btnNumeric.Tag = typeof(NumericUpDown);
            this.btnNumeric.UseVisualStyleBackColor = true;
            // 
            // btnLabel
            // 
            this.btnLabel.Location = new System.Drawing.Point(15, 15);
            this.btnLabel.Name = "btnLabel";
            this.btnLabel.Size = new System.Drawing.Size(90, 30);
            this.btnLabel.TabIndex = 0;
            this.btnLabel.Text = "Etiket";
            this.btnLabel.Tag = typeof(Label);
            this.btnLabel.UseVisualStyleBackColor = true;
            // 
            // pnlProperties
            // 
            this.pnlProperties.Controls.Add(this.propertyGrid);
            this.pnlProperties.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlProperties.Location = new System.Drawing.Point(550, 50);
            this.pnlProperties.Name = "pnlProperties";
            this.pnlProperties.Size = new System.Drawing.Size(250, 400);
            this.pnlProperties.TabIndex = 1;
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(250, 400);
            this.propertyGrid.TabIndex = 0;
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.btnNewLayout);
            this.pnlTop.Controls.Add(this.btnSaveLayout);
            this.pnlTop.Controls.Add(this.cmbStepType);
            this.pnlTop.Controls.Add(this.lblStepType);
            this.pnlTop.Controls.Add(this.cmbMachineSubType);
            this.pnlTop.Controls.Add(this.lblMachineSubType);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(800, 50);
            this.pnlTop.TabIndex = 2;
            // 
            // btnNewLayout
            // 
            this.btnNewLayout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNewLayout.Location = new System.Drawing.Point(482, 10);
            this.btnNewLayout.Name = "btnNewLayout";
            this.btnNewLayout.Size = new System.Drawing.Size(150, 30);
            this.btnNewLayout.TabIndex = 5;
            this.btnNewLayout.Text = "Yeni Tasarım";
            this.btnNewLayout.UseVisualStyleBackColor = true;
            // 
            // btnSaveLayout
            // 
            this.btnSaveLayout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveLayout.Location = new System.Drawing.Point(638, 10);
            this.btnSaveLayout.Name = "btnSaveLayout";
            this.btnSaveLayout.Size = new System.Drawing.Size(150, 30);
            this.btnSaveLayout.TabIndex = 4;
            this.btnSaveLayout.Text = "Tasarımı Kaydet";
            this.btnSaveLayout.UseVisualStyleBackColor = true;
            // 
            // cmbStepType
            // 
            this.cmbStepType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStepType.FormattingEnabled = true;
            this.cmbStepType.Location = new System.Drawing.Point(300, 12);
            this.cmbStepType.Name = "cmbStepType";
            this.cmbStepType.Size = new System.Drawing.Size(150, 28);
            this.cmbStepType.TabIndex = 3;
            // 
            // lblStepType
            // 
            this.lblStepType.AutoSize = true;
            this.lblStepType.Location = new System.Drawing.Point(219, 15);
            this.lblStepType.Name = "lblStepType";
            this.lblStepType.Size = new System.Drawing.Size(75, 20);
            this.lblStepType.TabIndex = 2;
            this.lblStepType.Text = "Adım Tipi:";
            // 
            // cmbMachineSubType
            // 
            this.cmbMachineSubType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMachineSubType.FormattingEnabled = true;
            this.cmbMachineSubType.Location = new System.Drawing.Point(63, 12);
            this.cmbMachineSubType.Name = "cmbMachineSubType";
            this.cmbMachineSubType.Size = new System.Drawing.Size(150, 28);
            this.cmbMachineSubType.TabIndex = 1;
            // 
            // lblMachineSubType
            // 
            this.lblMachineSubType.AutoSize = true;
            this.lblMachineSubType.Location = new System.Drawing.Point(12, 15);
            this.lblMachineSubType.Name = "lblMachineSubType";
            this.lblMachineSubType.Size = new System.Drawing.Size(45, 20);
            this.lblMachineSubType.TabIndex = 0;
            this.lblMachineSubType.Text = "Mak.:";
            //
            // RecipeStepDesigner_Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlProperties);
            this.Controls.Add(this.pnlTop);
            this.Name = "RecipeStepDesigner_Control";
            this.Size = new System.Drawing.Size(800, 450);
            this.pnlMain.ResumeLayout(false);
            this.pnlToolbox.ResumeLayout(false);
            this.pnlProperties.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.ResumeLayout(false);
        }
        #endregion
        private Panel pnlMain;
        private Panel pnlDesignSurface;
        private Panel pnlToolbox;
        private Button btnCheckbox;
        private Button btnNumeric;
        private Button btnLabel;
        private Panel pnlProperties;
        private PropertyGrid propertyGrid;
        private Panel pnlTop;
        private Button btnSaveLayout;
        private ComboBox cmbStepType;
        private Label lblStepType;
        private ComboBox cmbMachineSubType;
        private Label lblMachineSubType;
        private Button btnNewLayout;
        private Button btnTextbox;
    }
}