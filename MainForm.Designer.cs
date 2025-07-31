// MainForm.Designer.cs
namespace TekstilScada
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlNavigation = new System.Windows.Forms.Panel();
            this.btnAyarlar = new System.Windows.Forms.Button();
            this.btnRaporlar = new System.Windows.Forms.Button();
            this.btnProsesKontrol = new System.Windows.Forms.Button();
            this.btnProsesIzleme = new System.Windows.Forms.Button();
            this.btnGenelBakis = new System.Windows.Forms.Button();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.dilToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.türkçeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.englishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oturumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.çıkışYapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatusCurrentUser = new System.Windows.Forms.ToolStripStatusLabel();
            this.springLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatusLiveEvents = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlNavigation.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlNavigation
            // 
            this.pnlNavigation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(52)))), ((int)(((byte)(54)))));
            this.pnlNavigation.Controls.Add(this.btnAyarlar);
            this.pnlNavigation.Controls.Add(this.btnRaporlar);
            this.pnlNavigation.Controls.Add(this.btnProsesKontrol);
            this.pnlNavigation.Controls.Add(this.btnProsesIzleme);
            this.pnlNavigation.Controls.Add(this.btnGenelBakis);
            this.pnlNavigation.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlNavigation.Location = new System.Drawing.Point(0, 28);
            this.pnlNavigation.Name = "pnlNavigation";
            this.pnlNavigation.Size = new System.Drawing.Size(200, 499);
            this.pnlNavigation.TabIndex = 0;
            // 
            // btnAyarlar
            // 
            this.btnAyarlar.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAyarlar.FlatAppearance.BorderSize = 0;
            this.btnAyarlar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAyarlar.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.btnAyarlar.ForeColor = System.Drawing.Color.White;
            this.btnAyarlar.Location = new System.Drawing.Point(0, 180);
            this.btnAyarlar.Name = "btnAyarlar";
            this.btnAyarlar.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnAyarlar.Size = new System.Drawing.Size(200, 45);
            this.btnAyarlar.TabIndex = 3;
            this.btnAyarlar.Text = "Ayarlar";
            this.btnAyarlar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAyarlar.UseVisualStyleBackColor = true;
            this.btnAyarlar.Click += new System.EventHandler(this.btnAyarlar_Click);
            // 
            // btnRaporlar
            // 
            this.btnRaporlar.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnRaporlar.FlatAppearance.BorderSize = 0;
            this.btnRaporlar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRaporlar.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.btnRaporlar.ForeColor = System.Drawing.Color.White;
            this.btnRaporlar.Location = new System.Drawing.Point(0, 135);
            this.btnRaporlar.Name = "btnRaporlar";
            this.btnRaporlar.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnRaporlar.Size = new System.Drawing.Size(200, 45);
            this.btnRaporlar.TabIndex = 2;
            this.btnRaporlar.Text = "Raporlar";
            this.btnRaporlar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRaporlar.UseVisualStyleBackColor = true;
            this.btnRaporlar.Click += new System.EventHandler(this.btnRaporlar_Click);
            // 
            // btnProsesKontrol
            // 
            this.btnProsesKontrol.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnProsesKontrol.FlatAppearance.BorderSize = 0;
            this.btnProsesKontrol.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProsesKontrol.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.btnProsesKontrol.ForeColor = System.Drawing.Color.White;
            this.btnProsesKontrol.Location = new System.Drawing.Point(0, 90);
            this.btnProsesKontrol.Name = "btnProsesKontrol";
            this.btnProsesKontrol.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnProsesKontrol.Size = new System.Drawing.Size(200, 45);
            this.btnProsesKontrol.TabIndex = 1;
            this.btnProsesKontrol.Text = "Proses Kontrol";
            this.btnProsesKontrol.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProsesKontrol.UseVisualStyleBackColor = true;
            this.btnProsesKontrol.Click += new System.EventHandler(this.btnProsesKontrol_Click);
            // 
            // btnProsesIzleme
            // 
            this.btnProsesIzleme.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnProsesIzleme.FlatAppearance.BorderSize = 0;
            this.btnProsesIzleme.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProsesIzleme.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.btnProsesIzleme.ForeColor = System.Drawing.Color.White;
            this.btnProsesIzleme.Location = new System.Drawing.Point(0, 45);
            this.btnProsesIzleme.Name = "btnProsesIzleme";
            this.btnProsesIzleme.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnProsesIzleme.Size = new System.Drawing.Size(200, 45);
            this.btnProsesIzleme.TabIndex = 0;
            this.btnProsesIzleme.Text = "Proses İzleme";
            this.btnProsesIzleme.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProsesIzleme.UseVisualStyleBackColor = true;
            this.btnProsesIzleme.Click += new System.EventHandler(this.btnProsesIzleme_Click);
            // 
            // btnGenelBakis
            // 
            this.btnGenelBakis.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnGenelBakis.FlatAppearance.BorderSize = 0;
            this.btnGenelBakis.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenelBakis.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.btnGenelBakis.ForeColor = System.Drawing.Color.White;
            this.btnGenelBakis.Location = new System.Drawing.Point(0, 0);
            this.btnGenelBakis.Name = "btnGenelBakis";
            this.btnGenelBakis.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnGenelBakis.Size = new System.Drawing.Size(200, 45);
            this.btnGenelBakis.TabIndex = 4;
            this.btnGenelBakis.Text = "Genel Bakış";
            this.btnGenelBakis.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGenelBakis.UseVisualStyleBackColor = true;
            this.btnGenelBakis.Click += new System.EventHandler(this.btnGenelBakis_Click);
            // 
            // pnlContent
            // 
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(200, 28);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(822, 499);
            this.pnlContent.TabIndex = 1;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dilToolStripMenuItem,
            this.oturumToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1022, 28);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // dilToolStripMenuItem
            // 
            this.dilToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.türkçeToolStripMenuItem,
            this.englishToolStripMenuItem});
            this.dilToolStripMenuItem.Name = "dilToolStripMenuItem";
            this.dilToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.dilToolStripMenuItem.Text = "Dil";
            // 
            // türkçeToolStripMenuItem
            // 
            this.türkçeToolStripMenuItem.Name = "türkçeToolStripMenuItem";
            this.türkçeToolStripMenuItem.Size = new System.Drawing.Size(136, 26);
            this.türkçeToolStripMenuItem.Text = "Türkçe";
            this.türkçeToolStripMenuItem.Click += new System.EventHandler(this.türkçeToolStripMenuItem_Click);
            // 
            // englishToolStripMenuItem
            // 
            this.englishToolStripMenuItem.Name = "englishToolStripMenuItem";
            this.englishToolStripMenuItem.Size = new System.Drawing.Size(136, 26);
            this.englishToolStripMenuItem.Text = "English";
            this.englishToolStripMenuItem.Click += new System.EventHandler(this.englishToolStripMenuItem_Click);
            // 
            // oturumToolStripMenuItem
            // 
            this.oturumToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.çıkışYapToolStripMenuItem});
            this.oturumToolStripMenuItem.Name = "oturumToolStripMenuItem";
            this.oturumToolStripMenuItem.Size = new System.Drawing.Size(70, 24);
            this.oturumToolStripMenuItem.Text = "Oturum";
            // 
            // çıkışYapToolStripMenuItem
            // 
            this.çıkışYapToolStripMenuItem.Name = "çıkışYapToolStripMenuItem";
            this.çıkışYapToolStripMenuItem.Size = new System.Drawing.Size(145, 26);
            this.çıkışYapToolStripMenuItem.Text = "Çıkış Yap";
            this.çıkışYapToolStripMenuItem.Click += new System.EventHandler(this.çıkışYapToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatusCurrentUser,
            this.springLabel,
            this.lblStatusLiveEvents});
            this.statusStrip1.Location = new System.Drawing.Point(0, 527);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1022, 26);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatusCurrentUser
            // 
            this.lblStatusCurrentUser.Name = "lblStatusCurrentUser";
            this.lblStatusCurrentUser.Size = new System.Drawing.Size(83, 20);
            this.lblStatusCurrentUser.Text = "Giriş Yapan: -";
            // 
            // springLabel
            // 
            this.springLabel.Name = "springLabel";
            this.springLabel.Size = new System.Drawing.Size(779, 20);
            this.springLabel.Spring = true;
            // 
            // lblStatusLiveEvents
            // 
            this.lblStatusLiveEvents.IsLink = true;
            this.lblStatusLiveEvents.Name = "lblStatusLiveEvents";
            this.lblStatusLiveEvents.Size = new System.Drawing.Size(142, 20);
            this.lblStatusLiveEvents.Text = "Canlı Olay Akışı Göster";
            this.lblStatusLiveEvents.Click += new System.EventHandler(this.lblStatusLiveEvents_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1022, 553);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlNavigation);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Tekstil SCADA Sistemi";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.pnlNavigation.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel pnlNavigation;
        private System.Windows.Forms.Button btnAyarlar;
        private System.Windows.Forms.Button btnRaporlar;
        private System.Windows.Forms.Button btnProsesKontrol;
        private System.Windows.Forms.Button btnProsesIzleme;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Button btnGenelBakis;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem dilToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem türkçeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem englishToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusCurrentUser;
        private System.Windows.Forms.ToolStripStatusLabel springLabel;
        private System.Windows.Forms.ToolStripStatusLabel lblStatusLiveEvents;
        private System.Windows.Forms.ToolStripMenuItem oturumToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem çıkışYapToolStripMenuItem;
    }
}
