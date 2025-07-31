// Dosya: TekstilScada.Core/Core/ConfigurationManager.cs
using Microsoft.Extensions.Configuration;
using System.IO;

namespace TekstilScada.Core
{
    public static class AppConfig
    {
        public static string ConnectionString { get; private set; }

        static AppConfig()
        {
            var configuration = new ConfigurationBuilder()
                // Çalışan uygulamanın bulunduğu dizini baz al
                .SetBasePath(Directory.GetCurrentDirectory())
                // Bu dizindeki appsettings.json dosyasını oku
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }
    }
}