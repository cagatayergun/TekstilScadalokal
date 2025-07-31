// UI/Controls/MachineCard_Control.Designer.cs
namespace TekstilScada.UI.Controls
{
    partial class MachineCard_Control
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
            this.pnlMain = new System.Windows.Forms.Panel();
            this.lblMachineIdValue = new System.Windows.Forms.Label();
            this.lblMachineNameValue = new System.Windows.Forms.Label();
            this.lblStepValue = new System.Windows.Forms.Label();
            this.lblOperatorValue = new System.Windows.Forms.Label();
            this.lblRecipeNameValue = new System.Windows.Forms.Label();
            this.lblMachineIdTitle = new System.Windows.Forms.Label();
            this.lblMachineNameTitle = new System.Windows.Forms.Label();
            this.lblStepTitle = new System.Windows.Forms.Label();
            this.lblOperatorTitle = new System.Windows.Forms.Label();
            this.lblRecipeNameTitle = new System.Windows.Forms.Label();
            this.lblPercentage = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblProcessing = new System.Windows.Forms.Label();
            this.pnlIcons = new System.Windows.Forms.Panel();
            this.btnInfo = new System.Windows.Forms.Button();
            this.picAlarm = new System.Windows.Forms.PictureBox();
            this.picPause = new System.Windows.Forms.PictureBox();
            this.picPlay = new System.Windows.Forms.PictureBox();
            this.btnVnc = new System.Windows.Forms.Button();
            this.lblMachineNumber = new System.Windows.Forms.Label();
            this.picConnection = new System.Windows.Forms.PictureBox();
            this.pnlMain.SuspendLayout();
            this.pnlIcons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAlarm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPause)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPlay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picConnection)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(230)))), ((int)(((byte)(233)))));
            this.pnlMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlMain.Controls.Add(this.lblMachineIdValue);
            this.pnlMain.Controls.Add(this.lblMachineNameValue);
            this.pnlMain.Controls.Add(this.lblStepValue);
            this.pnlMain.Controls.Add(this.lblOperatorValue);
            this.pnlMain.Controls.Add(this.lblRecipeNameValue);
            this.pnlMain.Controls.Add(this.lblMachineIdTitle);
            this.pnlMain.Controls.Add(this.lblMachineNameTitle);
            this.pnlMain.Controls.Add(this.lblStepTitle);
            this.pnlMain.Controls.Add(this.lblOperatorTitle);
            this.pnlMain.Controls.Add(this.lblRecipeNameTitle);
            this.pnlMain.Controls.Add(this.lblPercentage);
            this.pnlMain.Controls.Add(this.progressBar);
            this.pnlMain.Controls.Add(this.lblProcessing);
            this.pnlMain.Controls.Add(this.pnlIcons);
            this.pnlMain.Controls.Add(this.lblMachineNumber);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(320, 240);
            this.pnlMain.TabIndex = 0;
            // 
            // lblMachineIdValue
            // 
            this.lblMachineIdValue.BackColor = System.Drawing.Color.White;
            this.lblMachineIdValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMachineIdValue.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblMachineIdValue.Location = new System.Drawing.Point(120, 200);
            this.lblMachineIdValue.Name = "lblMachineIdValue";
            this.lblMachineIdValue.Size = new System.Drawing.Size(180, 25);
            this.lblMachineIdValue.TabIndex = 14;
            this.lblMachineIdValue.Text = "---";
            this.lblMachineIdValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMachineNameValue
            // 
            this.lblMachineNameValue.BackColor = System.Drawing.Color.White;
            this.lblMachineNameValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMachineNameValue.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblMachineNameValue.Location = new System.Drawing.Point(120, 170);
            this.lblMachineNameValue.Name = "lblMachineNameValue";
            this.lblMachineNameValue.Size = new System.Drawing.Size(180, 25);
            this.lblMachineNameValue.TabIndex = 13;
            this.lblMachineNameValue.Text = "---";
            this.lblMachineNameValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblStepValue
            // 
            this.lblStepValue.BackColor = System.Drawing.Color.White;
            this.lblStepValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblStepValue.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblStepValue.Location = new System.Drawing.Point(120, 140);
            this.lblStepValue.Name = "lblStepValue";
            this.lblStepValue.Size = new System.Drawing.Size(180, 25);
            this.lblStepValue.TabIndex = 12;
            this.lblStepValue.Text = "---";
            this.lblStepValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblOperatorValue
            // 
            this.lblOperatorValue.BackColor = System.Drawing.Color.White;
            this.lblOperatorValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblOperatorValue.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblOperatorValue.Location = new System.Drawing.Point(120, 110);
            this.lblOperatorValue.Name = "lblOperatorValue";
            this.lblOperatorValue.Size = new System.Drawing.Size(180, 25);
            this.lblOperatorValue.TabIndex = 11;
            this.lblOperatorValue.Text = "---";
            this.lblOperatorValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRecipeNameValue
            // 
            this.lblRecipeNameValue.BackColor = System.Drawing.Color.White;
            this.lblRecipeNameValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRecipeNameValue.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblRecipeNameValue.Location = new System.Drawing.Point(120, 80);
            this.lblRecipeNameValue.Name = "lblRecipeNameValue";
            this.lblRecipeNameValue.Size = new System.Drawing.Size(180, 25);
            this.lblRecipeNameValue.TabIndex = 10;
            this.lblRecipeNameValue.Text = "---";
            this.lblRecipeNameValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMachineIdTitle
            // 
            this.lblMachineIdTitle.AutoSize = true;
            this.lblMachineIdTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblMachineIdTitle.Location = new System.Drawing.Point(15, 202);
            this.lblMachineIdTitle.Name = "lblMachineIdTitle";
            this.lblMachineIdTitle.Size = new System.Drawing.Size(89, 20);
            this.lblMachineIdTitle.TabIndex = 9;
            this.lblMachineIdTitle.Text = "MAKİNA ID:";
            // 
            // lblMachineNameTitle
            // 
            this.lblMachineNameTitle.AutoSize = true;
            this.lblMachineNameTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblMachineNameTitle.Location = new System.Drawing.Point(15, 172);
            this.lblMachineNameTitle.Name = "lblMachineNameTitle";
            this.lblMachineNameTitle.Size = new System.Drawing.Size(99, 20);
            this.lblMachineNameTitle.TabIndex = 8;
            this.lblMachineNameTitle.Text = "MAKİNA ADI:";
            // 
            // lblStepTitle
            // 
            this.lblStepTitle.AutoSize = true;
            this.lblStepTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblStepTitle.Location = new System.Drawing.Point(15, 142);
            this.lblStepTitle.Name = "lblStepTitle";
            this.lblStepTitle.Size = new System.Drawing.Size(54, 20);
            this.lblStepTitle.TabIndex = 7;
            this.lblStepTitle.Text = "ADIM:";
            // 
            // lblOperatorTitle
            // 
            this.lblOperatorTitle.AutoSize = true;
            this.lblOperatorTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblOperatorTitle.Location = new System.Drawing.Point(15, 112);
            this.lblOperatorTitle.Name = "lblOperatorTitle";
            this.lblOperatorTitle.Size = new System.Drawing.Size(90, 20);
            this.lblOperatorTitle.TabIndex = 6;
            this.lblOperatorTitle.Text = "OPERATOR:";
            // 
            // lblRecipeNameTitle
            // 
            this.lblRecipeNameTitle.AutoSize = true;
            this.lblRecipeNameTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblRecipeNameTitle.Location = new System.Drawing.Point(15, 82);
            this.lblRecipeNameTitle.Name = "lblRecipeNameTitle";
            this.lblRecipeNameTitle.Size = new System.Drawing.Size(95, 20);
            this.lblRecipeNameTitle.TabIndex = 5;
            this.lblRecipeNameTitle.Text = "RECETE ADI:";
            // 
            // lblPercentage
            // 
            this.lblPercentage.AutoSize = true;
            this.lblPercentage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblPercentage.Location = new System.Drawing.Point(260, 50);
            this.lblPercentage.Name = "lblPercentage";
            this.lblPercentage.Size = new System.Drawing.Size(35, 20);
            this.lblPercentage.TabIndex = 3;
            this.lblPercentage.Text = "0 %";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(120, 52);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(130, 15);
            this.progressBar.TabIndex = 2;
            // 
            // lblProcessing
            // 
            this.lblProcessing.AutoSize = true;
            this.lblProcessing.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblProcessing.Location = new System.Drawing.Point(15, 50);
            this.lblProcessing.Name = "lblProcessing";
            this.lblProcessing.Size = new System.Drawing.Size(99, 20);
            this.lblProcessing.TabIndex = 1;
            this.lblProcessing.Text = "PROSESSING";
            // 
            // pnlIcons
            // 
            this.pnlIcons.Controls.Add(this.picConnection);
            this.pnlIcons.Controls.Add(this.btnInfo);
            this.pnlIcons.Controls.Add(this.picAlarm);
            this.pnlIcons.Controls.Add(this.picPause);
            this.pnlIcons.Controls.Add(this.picPlay);
            this.pnlIcons.Controls.Add(this.btnVnc);
            this.pnlIcons.Location = new System.Drawing.Point(50, 5);
            this.pnlIcons.Name = "pnlIcons";
            this.pnlIcons.Size = new System.Drawing.Size(260, 40);
            this.pnlIcons.TabIndex = 15;
            // 
            // btnInfo
            // 
            this.btnInfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnInfo.Location = new System.Drawing.Point(220, 5);
            this.btnInfo.Name = "btnInfo";
            this.btnInfo.Size = new System.Drawing.Size(30, 30);
            this.btnInfo.TabIndex = 5;
            this.btnInfo.Text = "i";
            this.btnInfo.UseVisualStyleBackColor = true;
            this.btnInfo.Click += new System.EventHandler(this.btnInfo_Click);
            // 
            // picAlarm
            // 
            this.picAlarm.BackColor = System.Drawing.Color.Transparent;
            this.picAlarm.Location = new System.Drawing.Point(130, 5);
            this.picAlarm.Name = "picAlarm";
            this.picAlarm.Size = new System.Drawing.Size(30, 30);
            this.picAlarm.TabIndex = 3;
            this.picAlarm.TabStop = false;
            // 
            // picPause
            // 
            this.picPause.BackColor = System.Drawing.Color.Transparent;
            this.picPause.Location = new System.Drawing.Point(90, 5);
            this.picPause.Name = "picPause";
            this.picPause.Size = new System.Drawing.Size(30, 30);
            this.picPause.TabIndex = 2;
            this.picPause.TabStop = false;
            // 
            // picPlay
            // 
            this.picPlay.BackColor = System.Drawing.Color.Transparent;
            this.picPlay.Location = new System.Drawing.Point(50, 5);
            this.picPlay.Name = "picPlay";
            this.picPlay.Size = new System.Drawing.Size(30, 30);
            this.picPlay.TabIndex = 1;
            this.picPlay.TabStop = false;
            // 
            // btnVnc
            // 
            this.btnVnc.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnVnc.Location = new System.Drawing.Point(10, 5);
            this.btnVnc.Name = "btnVnc";
            this.btnVnc.Size = new System.Drawing.Size(30, 30);
            this.btnVnc.TabIndex = 0;
            this.btnVnc.Text = "V";
            this.btnVnc.UseVisualStyleBackColor = true;
            this.btnVnc.Click += new System.EventHandler(this.btnVnc_Click);
            // 
            // lblMachineNumber
            // 
            this.lblMachineNumber.AutoSize = true;
            this.lblMachineNumber.Font = new System.Drawing.Font("Segoe UI", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblMachineNumber.Location = new System.Drawing.Point(10, 5);
            this.lblMachineNumber.Name = "lblMachineNumber";
            this.lblMachineNumber.Size = new System.Drawing.Size(41, 38);
            this.lblMachineNumber.TabIndex = 0;
            this.lblMachineNumber.Text = "1.";
            // 
            // picConnection
            // 
            this.picConnection.BackColor = System.Drawing.Color.Red;
            this.picConnection.Location = new System.Drawing.Point(170, 5);
            this.picConnection.Name = "picConnection";
            this.picConnection.Size = new System.Drawing.Size(30, 30);
            this.picConnection.TabIndex = 6;
            this.picConnection.TabStop = false;
            // 
            // MachineCard_Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlMain);
            this.Margin = new System.Windows.Forms.Padding(10);
            this.Name = "MachineCard_Control";
            this.Size = new System.Drawing.Size(320, 240);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.pnlIcons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picAlarm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPause)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPlay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picConnection)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Label lblMachineNumber;
        private System.Windows.Forms.Panel pnlIcons;
        private System.Windows.Forms.Button btnVnc;
        private System.Windows.Forms.PictureBox picPlay;
        private System.Windows.Forms.PictureBox picPause;
        private System.Windows.Forms.PictureBox picAlarm;
        private System.Windows.Forms.Button btnInfo;
        private System.Windows.Forms.Label lblProcessing;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label lblPercentage;
        private System.Windows.Forms.Label lblRecipeNameTitle;
        private System.Windows.Forms.Label lblOperatorTitle;
        private System.Windows.Forms.Label lblStepTitle;
        private System.Windows.Forms.Label lblMachineNameTitle;
        private System.Windows.Forms.Label lblMachineIdTitle;
        private System.Windows.Forms.Label lblRecipeNameValue;
        private System.Windows.Forms.Label lblOperatorValue;
        private System.Windows.Forms.Label lblStepValue;
        private System.Windows.Forms.Label lblMachineNameValue;
        private System.Windows.Forms.Label lblMachineIdValue;
        private System.Windows.Forms.PictureBox picConnection;
    }
}
