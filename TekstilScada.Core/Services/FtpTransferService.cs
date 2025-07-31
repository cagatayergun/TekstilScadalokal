using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TekstilScada.Core;
using TekstilScada.Models;
using TekstilScada.Repositories;

namespace TekstilScada.Services
{
    public enum TransferType { Gonder, Al }
    public enum TransferStatus { Beklemede, Aktarılıyor, Başarılı, Hatalı }

    public class TransferJob
    {
        public Guid Id { get; } = Guid.NewGuid();
        public Machine Makine { get; set; }
        public ScadaRecipe? YerelRecete { get; set; } // Gönderme işlemi için
        public string? UzakDosyaAdi { get; set; } // Alma işlemi için
        public TransferType IslemTipi { get; set; }
        public TransferStatus Durum { get; set; } = TransferStatus.Beklemede;
        public int Ilerleme { get; set; } = 0;
        public string HataMesaji { get; set; } = string.Empty;

        // DataGridView'de göstermek için property'ler
        public string MakineAdi => Makine.MachineName;
        public string ReceteAdi => IslemTipi == TransferType.Gonder ? YerelRecete?.RecipeName : UzakDosyaAdi;
    }

    public class FtpTransferService
    {
        private static readonly Lazy<FtpTransferService> _instance = new Lazy<FtpTransferService>(() => new FtpTransferService());
        public static FtpTransferService Instance => _instance.Value;

        public BindingList<TransferJob> Jobs { get; } = new BindingList<TransferJob>();
        private bool _isProcessing = false;

        private FtpTransferService() { }

        public void QueueSendJobs(List<ScadaRecipe> receteler, Machine makine)
        {
            foreach (var recete in receteler)
            {
                if (!Jobs.Any(j => j.Makine.Id == makine.Id && j.YerelRecete?.Id == recete.Id && j.IslemTipi == TransferType.Gonder))
                {
                    Jobs.Add(new TransferJob { Makine = makine, YerelRecete = recete, IslemTipi = TransferType.Gonder });
                }
            }
            StartProcessingIfNotRunning();
        }

        public void QueueReceiveJobs(List<string> dosyaAdlari, Machine makine)
        {
            foreach (var dosya in dosyaAdlari)
            {
                if (!Jobs.Any(j => j.Makine.Id == makine.Id && j.UzakDosyaAdi == dosya && j.IslemTipi == TransferType.Al))
                {
                    Jobs.Add(new TransferJob { Makine = makine, UzakDosyaAdi = dosya, IslemTipi = TransferType.Al });
                }
            }
            StartProcessingIfNotRunning();
        }

        private void StartProcessingIfNotRunning()
        {
            if (!_isProcessing)
            {
                Task.Run(() => ProcessQueue(new RecipeRepository())); // Repository'i anlık oluşturuyoruz
            }
        }

        private async Task ProcessQueue(RecipeRepository recipeRepo)
        {
            _isProcessing = true;
            while (Jobs.Any(j => j.Durum == TransferStatus.Beklemede))
            {
                var job = Jobs.FirstOrDefault(j => j.Durum == TransferStatus.Beklemede);
                if (job == null) continue;

                job.Durum = TransferStatus.Aktarılıyor;
                try
                {
                    var ftpService = new FtpService(job.Makine.IpAddress, job.Makine.FtpUsername, job.Makine.FtpPassword);
                    job.Ilerleme = 20;

                    if (job.IslemTipi == TransferType.Gonder)
                    {
                        var fullRecipe = recipeRepo.GetRecipeById(job.YerelRecete.Id);
                        job.Ilerleme = 50;
                        string csvContent = RecipeCsvConverter.ToCsv(fullRecipe);
                        string remoteFileName = $"/RECIPE_{job.YerelRecete.Id}.csv"; // Örnek isimlendirme
                        await ftpService.UploadFileAsync(remoteFileName, csvContent);
                    }
                    else // Alma işlemi
                    {
                        string remoteFilePath = $"/{job.UzakDosyaAdi}";
                        string csvContent = await ftpService.DownloadFileAsync(remoteFilePath);
                        job.Ilerleme = 50;
                        string newRecipeName = $"HMI_{Path.GetFileNameWithoutExtension(job.UzakDosyaAdi)}_{DateTime.Now:yyMMddHHmm}";
                        ScadaRecipe newRecipe = RecipeCsvConverter.ToRecipe(csvContent, newRecipeName);
                        recipeRepo.SaveRecipe(newRecipe);
                    }

                    job.Ilerleme = 100;
                    job.Durum = TransferStatus.Başarılı;
                }
                catch (Exception ex)
                {
                    job.Durum = TransferStatus.Hatalı;
                    job.HataMesaji = ex.Message;
                }
            }
            _isProcessing = false;
        }
    }
}
