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
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }
    }
}