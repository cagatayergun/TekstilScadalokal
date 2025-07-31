// Program.cs
using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using TekstilScada.Core;
using TekstilScada.UI;
using TekstilScada.Properties; // Settings için eklendi

namespace TekstilScada
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // --- YENÝ: DÝL AYARLARI ---
            // Uygulamanýn varsayýlan dilini Türkçe olarak ayarlýyoruz.
            // Bu, .resx dosyalarýndan doðru olanýn seçilmesini saðlar.
            // --- YENÝ: KAYITLI DÝLÝ YÜKLEME ---
            // Kullanýcýnýn son seçtiði dili ayar dosyasýndan oku.
            // --- YENÝ: KAYITLI DÝLÝ YÜKLEME ---
            // Kullanýcýnýn son seçtiði dili ayar dosyasýndan oku.
         //   string savedLanguage = Settings.Default.UserLanguage;
          //  if (!string.IsNullOrEmpty(savedLanguage))
          //  {
          //      // Eðer bir dil kayýtlýysa, LanguageManager aracýlýðýyla uygula.
          //      LanguageManager.SetLanguage(savedLanguage);
            //}
            // --- YÜKLEME SONU ---

            // --- DÝL AYARLARI SONU ---

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var loginForm = new LoginForm())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    Application.Run(new MainForm());
                    
                }
            }
        }
    }
}
