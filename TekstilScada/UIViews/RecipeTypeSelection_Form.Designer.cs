namespace TekstilScada.UIViews
{
    partial class RecipeTypeSelection_Form
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            makinetip = new Label();
            cmbRecipeType = new ComboBox();
            btnOk = new Button();
            SuspendLayout();
            // 
            // makinetip
            // 
            makinetip.AutoSize = true;
            makinetip.Font = new Font("Segoe UI", 18F);
            makinetip.Location = new Point(111, 30);
            makinetip.Name = "makinetip";
            makinetip.Size = new Size(240, 32);
            makinetip.TabIndex = 0;
            makinetip.Text = "Makine Tipini Seçiniz";
            // 
            // cmbRecipeType
            // 
            cmbRecipeType.Font = new Font("Segoe UI", 16F);
            cmbRecipeType.FormattingEnabled = true;
            cmbRecipeType.Location = new Point(106, 98);
            cmbRecipeType.Name = "cmbRecipeType";
            cmbRecipeType.Size = new Size(250, 38);
            cmbRecipeType.TabIndex = 1;
            // 
            // btnOk
            // 
            btnOk.Location = new Point(150, 173);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(162, 61);
            btnOk.TabIndex = 2;
            btnOk.Text = "TAMAM";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += btnOk_Click;
            // 
            // RecipeTypeSelection_Form
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(462, 273);
            Controls.Add(btnOk);
            Controls.Add(cmbRecipeType);
            Controls.Add(makinetip);
            Name = "RecipeTypeSelection_Form";
            Text = "RecipeTypeSelection_Form";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label makinetip;
        private ComboBox cmbRecipeType;
        private Button btnOk;
    }
}