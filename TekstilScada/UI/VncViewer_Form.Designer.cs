namespace TekstilScada.UI
{
    partial class VncViewer_Form
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
            // Eğer yönetilen kaynaklar serbest bırakılıyorsa
            if (disposing)
            {
                // components konteynerini Dispose et
                if (components != null)
                {
                    components.Dispose();
                }

                // remoteDesktop1 kontrolü de burada dispose edilecektir çünkü components tarafından yönetilir.
                // Ancak, emin olmak için veya özel durumlar için burada açıkça Dispose çağrılabilir.
                // RemoteDesktop sınıfı kendi içindeki vnc ve desktop kaynaklarını temizlemelidir.
                // if (this.remoteDesktop1 != null) // Zaten components.Dispose() tarafından halledildiğinden genellikle gereksiz.
                // {
                //    this.remoteDesktop1.Dispose();
                // }
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
            this.remoteDesktop1 = new VncSharpCore.RemoteDesktop();
            this.SuspendLayout();
            //
            // remoteDesktop1
            //
            this.remoteDesktop1.AutoScroll = true;
            this.remoteDesktop1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.remoteDesktop1.Location = new System.Drawing.Point(0, 0);
            this.remoteDesktop1.Name = "remoteDesktop1";
            this.remoteDesktop1.Size = new System.Drawing.Size(800, 600);
            this.remoteDesktop1.TabIndex = 0;
            //
            // VncViewer_Form
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.remoteDesktop1);
            this.Name = "VncViewer_Form";
            this.Text = "VNC Görüntüleyici";
            this.Load += new System.EventHandler(this.VncViewer_Form_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.VncViewer_Form_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private VncSharpCore.RemoteDesktop remoteDesktop1;
    }
}