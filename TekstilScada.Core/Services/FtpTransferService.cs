using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TekstilScada.Core;
using TekstilScada.Models;
using TekstilScada.Repositories;

namespace TekstilScada.Services
{
    public enum TransferType { Gonder, Al }
    public enum TransferStatus { Beklemede, Aktarılıyor, Başarılı, Hatalı }

    public class TransferJob : INotifyPropertyChanged // INotifyPropertyChanged arayüzünü ekliyoruz
    {
        // INotifyPropertyChanged için gerekli event
        public event PropertyChangedEventHandler PropertyChanged;
        //public static event EventHandler RecipeListChanged;
      
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private TransferStatus _durum = TransferStatus.Beklemede;
        private int _ilerleme = 0;
        private string _hataMesaji = string.Empty;

        public Guid Id { get; } = Guid.NewGuid();
        public Machine Makine { get; set; }
        public ScadaRecipe? YerelRecete { get; set; }
        public string? UzakDosyaAdi { get; set; }
        public TransferType IslemTipi { get; set; }

        public TransferStatus Durum
        {
            get => _durum;
            set
            {
                if (_durum != value)
                {
                    _durum = value;
                    OnPropertyChanged(nameof(Durum)); // Değişikliği bildir
                }
            }
        }
        public int Ilerleme
        {
            get => _ilerleme;
            set
            {
                if (_ilerleme != value)
                {
                    _ilerleme = value;
                    OnPropertyChanged(nameof(Ilerleme)); // Değişikliği bildir
                }
            }
        }
        public string HataMesaji
        {
            get => _hataMesaji;
            set
            {
                if (_hataMesaji != value)
                {
                    _hataMesaji = value;
                    OnPropertyChanged(nameof(HataMesaji)); // Değişikliği bildir
                }
            }
        }

        // DataGridView'de göstermek için property'ler
        public string MakineAdi => Makine.MachineName;
        public string ReceteAdi => IslemTipi == TransferType.Gonder ? YerelRecete?.RecipeName : UzakDosyaAdi;
    }

    public class FtpTransferService
    {
        private static readonly Lazy<FtpTransferService> _instance = new Lazy<FtpTransferService>(() => new FtpTransferService());
        public static FtpTransferService Instance => _instance.Value;
          public event EventHandler RecipeListChanged;
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
        public void QueueSendJobs(List<ScadaRecipe> receteler, List<Machine> makineler)
        {
            foreach (var makine in makineler)
            {
                foreach (var recete in receteler)
                {
                    // Her makine-reçete kombinasyonu için bir iş oluştur
                    if (!Jobs.Any(j => j.Makine.Id == makine.Id && j.YerelRecete?.Id == recete.Id && j.IslemTipi == TransferType.Gonder))
                    {
                        Jobs.Add(new TransferJob { Makine = makine, YerelRecete = recete, IslemTipi = TransferType.Gonder });
                    }
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
        private string GenerateNewRecipeName(TransferJob job, ScadaRecipe recipe, RecipeRepository recipeRepo)
        {
            // 1. Kısım: Makine Adı
            string machineName = job.Makine.MachineName;

            // 2. Kısım: Reçete Numarası (Dosya Adından) - GÜNCELLENDİ
            string recipeNumberPart = "0"; // Varsayılan değer
            try
            {
                string fileName = Path.GetFileNameWithoutExtension(job.UzakDosyaAdi); // Örn: "XPR00095"

                // Dosya adının sonundaki rakamları bulmak için Regex kullan.
                Match match = Regex.Match(fileName, @"\d+$");
                if (match.Success)
                {
                    // Başındaki sıfırları kaldırmak için integer'a çevirip geri string yap.
                    recipeNumberPart = int.Parse(match.Value).ToString(); // "95"
                }
            }
            catch
            {
                recipeNumberPart = "NO_HATA"; // Hata durumunda
            }
            string asciiPart = "BILGI_YOK";
            try
            {
                // Yeni kural: Reçete adımlarından 99 numaralı adımı bul.
                var step99 = recipe.Steps.FirstOrDefault(s => s.StepNumber == 99);

                // 99. adımın var olup olmadığını ve ilk 5 word'ü içerecek kadar verisi olup olmadığını kontrol et.
                if (step99 != null && step99.StepDataWords.Length >= 5)
                {
                    // 5 word = 10 byte
                    byte[] asciiBytes = new byte[10];
                    for (int i = 0; i < 5; i++)
                    {
                        // 99. adımın ilk 5 word'ünü sırayla al (indeks 0'dan 4'e kadar).
                        short word = step99.StepDataWords[i];
                        byte[] wordBytes = BitConverter.GetBytes(word);

                        // Byte sırasının (endianness) doğru olduğundan emin olmalıyız.
                        // PLC'ler genellikle Big Endian, PC'ler Little Endian kullanır.
                        // Gerekirse Array.Reverse(wordBytes) kullanılabilir.
                        // Little Endian varsayımıyla devam ediyoruz.
                        asciiBytes[i * 2] = wordBytes[0];
                        asciiBytes[i * 2 + 1] = wordBytes[1];
                    }

                    // Byte dizisini ASCII metne çevir ve temizle (boş karakterleri sil).
                    asciiPart = Encoding.ASCII.GetString(asciiBytes).Replace("\0", "").Trim();
                    if (string.IsNullOrEmpty(asciiPart))
                    {
                        asciiPart = "BOS";
                    }
                }
                else
                {
                    // Reçetede 99. adım bulunamazsa veya yeterli veri yoksa bu ismi ver.
                    asciiPart = "ADIM99_YOK";
                }
            }
            catch
            {
                // Beklenmedik bir hata olursa bu ismi ver.
                asciiPart = "HATA";
            }

            // Temel ismi oluştur
            string baseName = $"{machineName}-{recipeNumberPart}-{asciiPart}";

            // 4. Kısım: İsim çakışması varsa numaralandır
            string finalName = baseName;
            int copyCounter = 1;
            // Not: Bu sorgu çok sayıda reçete varsa yavaş olabilir. 
            // RecipeRepository'de RecipeNameExists(name) gibi daha verimli bir metot olması idealdir.
            var existingNames = new HashSet<string>(recipeRepo.GetAllRecipes().Select(r => r.RecipeName));

            while (existingNames.Contains(finalName))
            {
                finalName = $"{baseName}_Kopya{copyCounter}";
                copyCounter++;
            }

            return finalName;
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
                    var ftpService = new FtpService(job.Makine.VncAddress, job.Makine.FtpUsername, job.Makine.FtpPassword);
                    job.Ilerleme = 20;

                    if (job.IslemTipi == TransferType.Gonder)
                    {
                        var fullRecipe = recipeRepo.GetRecipeById(job.YerelRecete.Id);
                        job.Ilerleme = 50;
                        string csvContent = RecipeCsvConverter.ToCsv(fullRecipe);
                        string remoteFileName = $"{job.YerelRecete.Id}.csv"; // Örnek isimlendirme
                        await ftpService.UploadFileAsync(remoteFileName, csvContent);
                    }
                    else // Alma işlemi
                    {
                        string remoteFilePath = $"/{job.UzakDosyaAdi}";
                        string csvContent = await ftpService.DownloadFileAsync(remoteFilePath);
                        job.Ilerleme = 50;

                        // --- DEĞİŞİKLİK BURADA BAŞLIYOR ---

                        // ESKİ SATIRI SİLİN (Eğer varsa):
                        // string newRecipeName = $"HMI_{Path.GetFileNameWithoutExtension(job.UzakDosyaAdi)}_{DateTime.Now:yyMMddHHmm}";
                        // ScadaRecipe newRecipe = RecipeCsvConverter.ToRecipe(csvContent, newRecipeName);
                        // recipeRepo.SaveRecipe(newRecipe);

                        // YERİNE BU DOĞRU KODLARI EKLEYİN:

                        // Önce reçeteyi geçici bir isimle parse et
                        ScadaRecipe newRecipe = RecipeCsvConverter.ToRecipe(csvContent, "temp_name");

                        // YENİ MANTIK: Özel formata göre yeni ismi oluşturmak için metodu ÇAĞIRIN
                        string newRecipeName = GenerateNewRecipeName(job, newRecipe, recipeRepo);

                        // Final ismini ata ve kaydet
                        newRecipe.RecipeName = newRecipeName;
                        recipeRepo.SaveRecipe(newRecipe);
                        // YENİ EKLENEN SATIR: İşlem bitince olayı tetikle!
                        RecipeListChanged?.Invoke(this, EventArgs.Empty);
                        // --- DEĞİŞİKLİK BURADA BİTİYOR ---
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
