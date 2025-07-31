// Services/FtpService.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinSCP; // WinSCP kütüphanesini kullanıyoruz

namespace TekstilScada.Services
{
    public class FtpService
    {
        private readonly string _host;
        private readonly string _username;
        private readonly string _password;
        private readonly int _port;

        public FtpService(string host, string username, string password)
        {
            var hostParts = host.Split(':');
            _host = hostParts[0];
            _port = hostParts.Length > 1 ? int.Parse(hostParts[1]) : 21;
            _username = username;
            _password = password;
        }

        private SessionOptions GetSessionOptions()
        {
            return new SessionOptions
            {
                Protocol = Protocol.Ftp,
                HostName = _host,
                PortNumber = _port,
                UserName = _username,
                Password = _password,
                // HMI sunucusu şifresiz (düz) FTP kullandığı için bu ayar önemli
                FtpSecure = FtpSecure.None
            };
        }

        public async Task<List<string>> ListDirectoryAsync(string remoteDirectory)
        {
            var fileList = new List<string>();
            var sessionOptions = GetSessionOptions();

            await Task.Run(() =>
            {
                using (var session = new Session())
                {
                    session.Open(sessionOptions);
                    // RemoteDirectoryInfo, sunucudaki dosyaları ve klasörleri listeler
                    RemoteDirectoryInfo directoryInfo = session.ListDirectory(remoteDirectory);

                    foreach (RemoteFileInfo fileInfo in directoryInfo.Files)
                    {
                        // Sadece dosyaları listeye ekle
                        if (!fileInfo.IsDirectory)
                        {
                            fileList.Add(fileInfo.Name);
                        }
                    }
                }
            });

            return fileList;
        }

        public async Task<string> DownloadFileAsync(string remoteFilePath)
        {
            var sessionOptions = GetSessionOptions();
            string content = "";
            string tempFile = Path.GetTempFileName();

            await Task.Run(() =>
            {
                using (var session = new Session())
                {
                    session.Open(sessionOptions);
                    // Dosyayı önce geçici bir yere indir
                    session.GetFiles(remoteFilePath, tempFile).Check();
                }
            });

            // Geçici dosyayı oku ve sil
            content = await File.ReadAllTextAsync(tempFile, Encoding.ASCII);
            File.Delete(tempFile);
            return content;
        }

        public async Task UploadFileAsync(string remoteFilePath, string fileContent)
        {
            var sessionOptions = GetSessionOptions();
            string tempFile = Path.GetTempFileName();
            await File.WriteAllTextAsync(tempFile, fileContent, Encoding.ASCII);

            await Task.Run(() =>
            {
                using (var session = new Session())
                {
                    session.Open(sessionOptions);
                    // Geçici dosyayı sunucuya yükle
                    session.PutFiles(tempFile, remoteFilePath).Check();
                }
            });

            File.Delete(tempFile);
        }
    }
}