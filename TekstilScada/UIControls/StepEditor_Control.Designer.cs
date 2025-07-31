// StepEditor_Control.Designer.cs
// Bu kod bloğunu StepEditor_Control.Designer.cs dosyasına yapıştırın.
namespace TekstilScada.UI.Controls.RecipeStepEditors
{
    partial class StepEditor_Control
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
            this.pnlStepTypes = new System.Windows.Forms.Panel();
            this.chkSikma = new System.Windows.Forms.CheckBox();
            this.chkBosaltma = new System.Windows.Forms.CheckBox();
            this.chkDozaj = new System.Windows.Forms.CheckBox();
            this.chkCalisma = new System.Windows.Forms.CheckBox();
            this.chkIsitma = new System.Windows.Forms.CheckBox();
            this.chkSuAlma = new System.Windows.Forms.CheckBox();
            this.pnlParameters = new System.Windows.Forms.Panel();
            this.pnlStepTypes.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlStepTypes
            // 
            this.pnlStepTypes.AutoScroll = true;
            this.pnlStepTypes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlStepTypes.Controls.Add(this.chkSikma);
            this.pnlStepTypes.Controls.Add(this.chkBosaltma);
            this.pnlStepTypes.Controls.Add(this.chkDozaj);
            this.pnlStepTypes.Controls.Add(this.chkCalisma);
            this.pnlStepTypes.Controls.Add(this.chkIsitma);
            this.pnlStepTypes.Controls.Add(this.chkSuAlma);
            this.pnlStepTypes.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlStepTypes.Location = new System.Drawing.Point(0, 0);
            this.pnlStepTypes.Name = "pnlStepTypes";
            this.pnlStepTypes.Size = new System.Drawing.Size(150, 450);
            this.pnlStepTypes.TabIndex = 0;
            // 
            // chkSikma
            // 
            this.chkSikma.AutoSize = true;
            this.chkSikma.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.chkSikma.Location = new System.Drawing.Point(10, 160);
            this.chkSikma.Name = "chkSikma";
            this.chkSikma.Size = new System.Drawing.Size(78, 24);
            this.chkSikma.TabIndex = 5;
            this.chkSikma.Text = "SIKMA";
            this.chkSikma.UseVisualStyleBackColor = true;
            // 
            // chkBosaltma
            // 
            this.chkBosaltma.AutoSize = true;
            this.chkBosaltma.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.chkBosaltma.Location = new System.Drawing.Point(10, 130);
            this.chkBosaltma.Name = "chkBosaltma";
            this.chkBosaltma.Size = new System.Drawing.Size(110, 24);
            this.chkBosaltma.TabIndex = 4;
            this.chkBosaltma.Text = "BOŞALTMA";
            this.chkBosaltma.UseVisualStyleBackColor = true;
            // 
            // chkDozaj
            // 
            this.chkDozaj.AutoSize = true;
            this.chkDozaj.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.chkDozaj.Location = new System.Drawing.Point(10, 100);
            this.chkDozaj.Name = "chkDozaj";
            this.chkDozaj.Size = new System.Drawing.Size(81, 24);
            this.chkDozaj.TabIndex = 3;
            this.chkDozaj.Text = "DOZAJ";
            this.chkDozaj.UseVisualStyleBackColor = true;
            // 
            // chkCalisma
            // 
            this.chkCalisma.AutoSize = true;
            this.chkCalisma.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.chkCalisma.Location = new System.Drawing.Point(10, 70);
            this.chkCalisma.Name = "chkCalisma";
            this.chkCalisma.Size = new System.Drawing.Size(98, 24);
            this.chkCalisma.TabIndex = 2;
            this.chkCalisma.Text = "ÇALIŞMA";
            this.chkCalisma.UseVisualStyleBackColor = true;
            // 
            // chkIsitma
            // 
            this.chkIsitma.AutoSize = true;
            this.chkIsitma.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.chkIsitma.Location = new System.Drawing.Point(10, 40);
            this.chkIsitma.Name = "chkIsitma";
            this.chkIsitma.Size = new System.Drawing.Size(82, 24);
            this.chkIsitma.TabIndex = 1;
            this.chkIsitma.Text = "ISITMA";
            this.chkIsitma.UseVisualStyleBackColor = true;
            // 
            // chkSuAlma
            // 
            this.chkSuAlma.AutoSize = true;
            this.chkSuAlma.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.chkSuAlma.Location = new System.Drawing.Point(10, 10);
            this.chkSuAlma.Name = "chkSuAlma";
            this.chkSuAlma.Size = new System.Drawing.Size(96, 24);
            this.chkSuAlma.TabIndex = 0;
            this.chkSuAlma.Text = "SU ALMA";
            this.chkSuAlma.UseVisualStyleBackColor = true;
            // 
            // pnlParameters
            // 
            this.pnlParameters.AutoScroll = true;
            this.pnlParameters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlParameters.Location = new System.Drawing.Point(150, 0);
            this.pnlParameters.Name = "pnlParameters";
            this.pnlParameters.Size = new System.Drawing.Size(250, 450);
            this.pnlParameters.TabIndex = 1;
            // 
            // StepEditor_Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlParameters);
            this.Controls.Add(this.pnlStepTypes);
            this.Name = "StepEditor_Control";
            this.Size = new System.Drawing.Size(400, 450);
            this.pnlStepTypes.ResumeLayout(false);
            this.pnlStepTypes.PerformLayout();
            this.ResumeLayout(false);
        }
        #endregion
        private System.Windows.Forms.Panel pnlStepTypes;
        private System.Windows.Forms.CheckBox chkSuAlma;
        private System.Windows.Forms.CheckBox chkIsitma;
        private System.Windows.Forms.CheckBox chkCalisma;
        private System.Windows.Forms.CheckBox chkDozaj;
        private System.Windows.Forms.CheckBox chkBosaltma;
        private System.Windows.Forms.CheckBox chkSikma;
        private System.Windows.Forms.Panel pnlParameters;
    }
}
