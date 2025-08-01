// UI/Views/MakineDetay_Control.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TekstilScada.Core;
using TekstilScada.Models;
using TekstilScada.Repositories;
using TekstilScada.Services;

namespace TekstilScada.UI.Views
{
    public partial class MakineDetay_Control : UserControl
    {
        public event EventHandler BackRequested;

        private PlcPollingService _pollingService;
        private ProcessLogRepository _logRepository;
        private AlarmRepository _alarmRepository;
        private RecipeRepository _recipeRepository;
        private ProductionRepository _productionRepository;
        private Machine _machine;
        private ScottPlot.Plottables.Scatter _tempPlot;
        private ScottPlot.Plottables.Scatter _rpmPlot;
        private ScottPlot.Plottables.Scatter _waterLevelPlot; // Eğer su seviyesi çizgisini de eklediyseniz veya ekleyecekseniz
       
        private System.Windows.Forms.Timer _uiUpdateTimer;
        private string _lastLoadedBatchIdForChart = null; // Sadece bu değişken kalacak

        public MakineDetay_Control()
        {
            InitializeComponent();
            btnGeri.Click += (sender, args) => BackRequested?.Invoke(this, EventArgs.Empty);
            this.progressTemp.Paint += new System.Windows.Forms.PaintEventHandler(this.progressTemp_Paint);
            
        }

        public void InitializeControl(Machine machine, PlcPollingService service, ProcessLogRepository logRepo, AlarmRepository alarmRepo, RecipeRepository recipeRepo, ProductionRepository productionRepo)
        {
            _machine = machine;
            _pollingService = service;

            _logRepository = logRepo;
            _alarmRepository = alarmRepo;
            _recipeRepository = recipeRepo;
            _productionRepository = productionRepo;
            _uiUpdateTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            _uiUpdateTimer.Tick += (sender, args) => UpdateLiveGauges();
            _uiUpdateTimer.Start();
            _pollingService.OnMachineDataRefreshed += OnDataRefreshed;
            _pollingService.OnMachineConnectionStateChanged += OnConnectionStateChanged;
            this.VisibleChanged += MakineDetay_Control_VisibleChanged;
            LoadInitialData();
        }

        private void LoadInitialData()
        {
            if (_pollingService.MachineDataCache.TryGetValue(_machine.Id, out var status))
            {
                UpdateUI(status);
            }
          
        }

        private void OnConnectionStateChanged(int machineId, FullMachineStatus status)
        {
            if (machineId == _machine.Id && this.IsHandleCreated && !this.IsDisposed)
            {
                this.BeginInvoke(new Action(() => UpdateUI(status)));
            }
        }

        private void OnDataRefreshed(int machineId, FullMachineStatus status)
        {
            if (machineId == _machine.Id && this.IsHandleCreated && !this.IsDisposed)
            {
                this.BeginInvoke(new Action(() => UpdateUI(status)));
            }
        }

        private void MakineDetay_Control_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible && _machine != null)
            {
                _lastLoadedBatchIdForChart = null; // Sayfa göründüğünde izleyiciyi sıfırla
                if (_pollingService.MachineDataCache.TryGetValue(_machine.Id, out var status))
                {
                    UpdateUI(status);
                }
            }
        }

        private void UpdateLiveGauges()
        {
            if (_machine != null && _pollingService.MachineDataCache.TryGetValue(_machine.Id, out var status))
            {
                SafeInvoke(() =>
                {
                    gaugeRpm.Value = status.AnlikDevirRpm;
                    gaugeRpm.Text = status.AnlikDevirRpm.ToString();

                    // Anlık sıcaklık değerini Panel'in Tag özelliğine atıyoruz.
                    // Maksimum değeri (150) burada veya Paint metodunda sabit tutabilirsiniz,
                    // veya onu da Tag'in farklı bir parçası olarak geçirebilirsiniz.
                    progressTemp.Tag = status.AnlikSicaklik;

                    lblTempValue.Text = $"{status.AnlikSicaklik} °C";
                    lblTempValue.ForeColor = GetTemperatureColor(status.AnlikSicaklik);
                    progressTemp.Invalidate(); // Panel'in Paint olayını tetikler

                    waterTankGauge1.Value = status.AnlikSuSeviyesi;
                });
            }
        }

        private void UpdateUI(FullMachineStatus status)
        {
            // 1. Temel bilgileri her zaman güncelle
            lblMakineAdi.Text = status.MachineName;
            lblOperator.Text = string.IsNullOrEmpty(status.OperatorIsmi) ? "---" : status.OperatorIsmi;
            lblReceteAdi.Text = string.IsNullOrEmpty(status.RecipeName) ? "---" : status.RecipeName;
            lblMusteriNo.Text = string.IsNullOrEmpty(status.MusteriNumarasi) ? "---" : status.MusteriNumarasi;
            lblBatchNo.Text = string.IsNullOrEmpty(status.BatchNumarasi) ? "---" : status.BatchNumarasi;
            lblSiparisNo.Text = string.IsNullOrEmpty(status.SiparisNumarasi) ? "---" : status.SiparisNumarasi;
            lblCalisanAdim.Text = $"#{status.AktifAdimNo} - {status.AktifAdimAdi}";

            // 2. Bağlantı durumunu kontrol et
            if (status.ConnectionState != ConnectionStatus.Connected)
            {
                ClearAllFieldsWithMessage("Bağlantı bekleniyor...");
                return;
            }

            // 3. Parti durumuna göre veri yükleme modunu seç
            if (!string.IsNullOrEmpty(status.BatchNumarasi))
            {
                // PARTİ MODU: Sadece parti ID'si değiştiyse tüm verileri yeniden yükle
                if (status.BatchNumarasi != _lastLoadedBatchIdForChart)
                {
                    LoadDataForBatch(status);
                }
            }
            else
            {
                // CANLI MOD (PARTİ YOK): Canlı verileri göster
                if (_lastLoadedBatchIdForChart != null) // Parti modundan canlı moda yeni geçildiyse grafiği temizle/yenile
                {
                    LoadDataForLive(status);
                }
                _lastLoadedBatchIdForChart = null; // Parti bittiğinde izleyiciyi sıfırla
                LoadDataForLive(status);
            }

            // 4. Her döngüde mevcut adımı vurgula
            HighlightCurrentStep(status.AktifAdimNo);
        }

        private void LoadDataForBatch(FullMachineStatus status)
        {
            _lastLoadedBatchIdForChart = status.BatchNumarasi;

            // Partiye özel alarmları yükle
            var alarms = _alarmRepository.GetAlarmDetailsForBatch(status.BatchNumarasi, _machine.Id);
            lstAlarmlar.DataSource = alarms.Any() ? alarms.Select(a => a.AlarmDescription).ToList() : new List<string> { "Bu parti için kayıtlı alarm yok." };

            // Partinin reçetesine ait adımları yükle
            LoadRecipeSteps(status.RecipeName);

            // Tüm parti süresini kapsayan grafiği yükle
            LoadTimelineChartForBatch(status.BatchNumarasi);
        }

        private void LoadDataForLive(FullMachineStatus status)
        {
            // Son 1 saatteki alarmları yükle
            var alarms = _alarmRepository.GetAlarmsForDateRange(_machine.Id, DateTime.Now.AddHours(-1), DateTime.Now);
            lstAlarmlar.DataSource = alarms.Any() ? alarms.Select(a => a.AlarmText).ToList() : new List<string> { "Yakın zamanda alarm yok." };

            // PLC'de o an yüklü olan reçetenin adımlarını yükle
            LoadRecipeSteps(status.RecipeName);

            // Son 30 dakikanın canlı grafiğini yükle
            LoadTimelineChartForLive();
        }

        private void LoadRecipeSteps(string recipeName)
        {
            dgvAdimlar.DataSource = null;
            if (!string.IsNullOrEmpty(recipeName))
            {
                var recipe = _recipeRepository.GetRecipeByName(recipeName);
                if (recipe != null)
                {
                    dgvAdimlar.DataSource = recipe.Steps.Select(s => new { Adım = s.StepNumber, Açıklama = GetStepTypeName(s) }).ToList();
                }
                else
                {
                    var placeholder = new List<object> { new { Adım = 0, Açıklama = $"'{recipeName}' reçetesi veritabanında bulunamadı." } };
                    dgvAdimlar.DataSource = placeholder;
                }
            }
        }

        private void LoadTimelineChartForBatch(string batchId)
        {
            SafeInvoke(() =>
            {
                formsPlot1.Plot.Clear();
                var (startTime, endTime) = _productionRepository.GetBatchTimestamps(batchId, _machine.Id);

                if (!startTime.HasValue)
                {
                    formsPlot1.Plot.Title("Parti başlangıç zamanı bulunamadı.");
                    formsPlot1.Refresh();
                    return;
                }

                DateTime effectiveEndTime = endTime ?? DateTime.Now;
                var dataPoints = _logRepository.GetLogsForDateRange(_machine.Id, startTime.Value, effectiveEndTime);

                if (!dataPoints.Any())
                {
                    formsPlot1.Plot.Title("Bu parti için henüz proses verisi kaydedilmemiş.");
                    formsPlot1.Refresh();
                    return;
                }

                formsPlot1.Plot.Title($"{_machine.MachineName} - Proses Zaman Çizgisi ({batchId})");
                var tempPlot = formsPlot1.Plot.Add.Scatter(
                    dataPoints.Select(p => p.Timestamp.ToOADate()).ToArray(),
                    dataPoints.Select(p => (double)p.Temperature).ToArray());
                tempPlot.Color = ScottPlot.Colors.Red;
                tempPlot.LegendText = "Sıcaklık";
                tempPlot.LineWidth = 2;

                var rpmAxis = formsPlot1.Plot.Axes.AddLeftAxis();
                rpmAxis.Label.Text = "Devir (RPM)";
                var rpmPlot = formsPlot1.Plot.Add.Scatter(
                    dataPoints.Select(p => p.Timestamp.ToOADate()).ToArray(),
                    dataPoints.Select(p => (double)p.Rpm).ToArray());
                rpmPlot.Color = ScottPlot.Colors.Blue;
                rpmPlot.LegendText = "Devir";
                rpmPlot.Axes.YAxis = rpmAxis;

                // YENİ EKLENEN KOD: Su Seviyesi (Water Level) verisini grafiğe ekle
                var waterLevelAxis = formsPlot1.Plot.Axes.AddRightAxis();
                waterLevelAxis.Label.Text = "Su Seviyesi (L)";
                var waterLevelPlot = formsPlot1.Plot.Add.Scatter(
                    dataPoints.Select(p => p.Timestamp.ToOADate()).ToArray(),
                    dataPoints.Select(p => (double)p.WaterLevel).ToArray());
                waterLevelPlot.Color = ScottPlot.Colors.Green; // Su seviyesi için yeşil renk
                waterLevelPlot.LegendText = "Su Seviyesi";
                waterLevelPlot.Axes.YAxis = waterLevelAxis;


                formsPlot1.Plot.Axes.DateTimeTicksBottom();
                formsPlot1.Plot.ShowLegend(ScottPlot.Alignment.UpperLeft);
                formsPlot1.Plot.Axes.AutoScale();
                formsPlot1.Refresh();
            });
        }

        // MakineDetay_Control.cs
        private void LoadTimelineChartForLive()
        {
           
            SafeInvoke(() =>
            {
                formsPlot1.Plot.Clear();
                DateTime endTime = DateTime.Now;
                // Son 5 saati (300 dakika) kapsayacak şekilde başlangıç zamanı
                DateTime startTime = endTime.AddMinutes(-100);

                var dataPoints = _logRepository.GetManualLogs(_machine.Id, startTime, endTime);

                if (!dataPoints.Any())
                {
                    formsPlot1.Plot.Clear();
                    formsPlot1.Plot.Title("Canlı veri akışı bekleniyor...");
                    formsPlot1.Refresh();
                    return;
                }

                double[] timeData = dataPoints.Select(p => p.Timestamp.ToOADate()).ToArray();
                double[] tempData = dataPoints.Select(p => (double)p.Temperature).ToArray();
                double[] rpmData = dataPoints.Select(p => (double)p.Rpm).ToArray();
                double[] waterLevelData = dataPoints.Select(p => (double)p.WaterLevel).ToArray();

                // Grafik nesneleri henüz oluşturulmadıysa (yani proses detay sayfası yeni açıldıysa)
                if (_tempPlot == null)
                {
                    formsPlot1.Plot.Clear(); // İlk oluşturmada her şeyi temizle
                    formsPlot1.Plot.Title($"{_machine.MachineName} - Canlı Proses Verileri");
                    formsPlot1.Plot.Axes.DateTimeTicksBottom();
                    formsPlot1.Plot.ShowLegend(ScottPlot.Alignment.UpperLeft);

                    // Sıcaklık Çizgisi
                    _tempPlot = formsPlot1.Plot.Add.Scatter(timeData, tempData);
                    _tempPlot.Color = ScottPlot.Colors.Red;
                    _tempPlot.LegendText = "Sıcaklık";
                    _tempPlot.LineWidth = 2;

                    // Devir Çizgisi
                    _rpmPlot = formsPlot1.Plot.Add.Scatter(timeData, rpmData);
                    _rpmPlot.Color = ScottPlot.Colors.Blue;
                    _rpmPlot.LegendText = "Devir";

                    // Su Seviyesi Çizgisi
                    _waterLevelPlot = formsPlot1.Plot.Add.Scatter(timeData, waterLevelData);
                    _waterLevelPlot.Color = ScottPlot.Colors.Green;
                    _waterLevelPlot.LegendText = "Su Seviyesi";

                    // SADECE İLK AÇILIŞTA EKSEN REFERANSLAMASI
                    // X eksenini endTime'a (şu anki zamana) göre ayarla ve geçmiş 5 saati göster
                    formsPlot1.Plot.Axes.SetLimitsX(startTime.ToOADate(), endTime.ToOADate());

                    // Y eksenlerini mevcut verilere göre otomatik ölçeklendir
                    formsPlot1.Plot.Axes.AutoScaleY();
                }
                else
                {
                    // Eğer grafik nesneleri zaten oluşturulduysa (yani sayfa açıkken sonraki güncellemeler geliyorsa)
                    // Mevcut çizgi grafiklerini kaldır
                    formsPlot1.Plot.Remove(_tempPlot);
                    formsPlot1.Plot.Remove(_rpmPlot);
                    formsPlot1.Plot.Remove(_waterLevelPlot);

                    // Yeni verilerle çizgi grafiklerini yeniden oluştur ve formsPlot1.Plot'a ekle
                    _tempPlot = formsPlot1.Plot.Add.Scatter(timeData, tempData);
                    _tempPlot.Color = ScottPlot.Colors.Red;
                    _tempPlot.LegendText = "Sıcaklık";
                    _tempPlot.LineWidth = 2;

                    _rpmPlot = formsPlot1.Plot.Add.Scatter(timeData, rpmData);
                    _rpmPlot.Color = ScottPlot.Colors.Blue;
                    _rpmPlot.LegendText = "Devir";

                    _waterLevelPlot = formsPlot1.Plot.Add.Scatter(timeData, waterLevelData);
                    _waterLevelPlot.Color = ScottPlot.Colors.Green;
                    _waterLevelPlot.LegendText = "Su Seviyesi";

                    // Sonraki güncellemelerde eksen limitlerini otomatik olarak değiştirmeyin,
                    // kullanıcının yaptığı zoom ve kaydırmaları koruyun.
                    // Sadece yeni veri mevcut görünümün dışına taştığında X eksenini biraz kaydırabilirsiniz.
                    // Bu kısım, kullanıcı etkileşimini korumak için önemlidir.

                    // Opsiyonel: Eğer kullanıcı herhangi bir zoom veya kaydırma yapmadıysa ve son veri
                    // görünür alanın dışına çıktıysa, görünümü son veriye kaydırabiliriz.
                    var xRange = formsPlot1.Plot.Axes.Bottom.Range;
                    if (timeData.Any() && timeData.Last() > xRange.Max)
                    {
                        // Mevcut aralığı koruyarak sadece sonuna eklemek için
                        // formsPlot1.Plot.Axes.SetLimitsX(xRange.Min, timeData.Last() + xRange.Span * 0.05);
                        // Veya daha basitçe, tüm aralığı en yeni veriye göre güncelle:
                        formsPlot1.Plot.Axes.SetLimitsX(startTime.ToOADate(), endTime.ToOADate());
                        // Y ekseni için de benzer bir mantık düşünebilirsiniz veya AutoScaleY() çağırarak güncelleyebilirsiniz.
                        // formsPlot1.Plot.Axes.AutoScaleY(); 
                    }
                }

                formsPlot1.Refresh();
            });
        }

        private void ClearAllFieldsWithMessage(string message)
        {
            ClearBatchSpecificFieldsWithMessage(message);
            lblReceteAdi.Text = "---";
            lblOperator.Text = "---";
            lblMusteriNo.Text = "---";
            lblBatchNo.Text = "---";
            lblSiparisNo.Text = "---";
            lblCalisanAdim.Text = "---";
        }

        private void ClearBatchSpecificFieldsWithMessage(string message)
        {
            lstAlarmlar.DataSource = new List<string> { message };
            dgvAdimlar.DataSource = null;
            formsPlot1.Plot.Clear();
            formsPlot1.Plot.Title(message);
            formsPlot1.Refresh();
        }

        private void HighlightCurrentStep(int currentStepNumber)
        {
            foreach (DataGridViewRow row in dgvAdimlar.Rows)
            {
                if (row.Cells["Adım"].Value != null && Convert.ToInt32(row.Cells["Adım"].Value) == currentStepNumber)
                {
                    row.DefaultCellStyle.BackColor = Color.LightGreen;
                    row.DefaultCellStyle.Font = new Font(dgvAdimlar.Font, FontStyle.Bold);
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                    row.DefaultCellStyle.Font = new Font(dgvAdimlar.Font, FontStyle.Regular);
                }
            }
        }

        private string GetStepTypeName(ScadaRecipeStep step)
        {
            var stepTypes = new List<string>();
            short controlWord = step.StepDataWords[24];
            if ((controlWord & 1) != 0) stepTypes.Add("Su Alma");
            if ((controlWord & 2) != 0) stepTypes.Add("Isıtma");
            if ((controlWord & 4) != 0) stepTypes.Add("Çalışma");
            if ((controlWord & 8) != 0) stepTypes.Add("Dozaj");
            if ((controlWord & 16) != 0) stepTypes.Add("Boşaltma");
            if ((controlWord & 32) != 0) stepTypes.Add("Sıkma");
            return string.Join(" + ", stepTypes);
        }

        private Color GetTemperatureColor(int temp)
        {
            if (temp < 40) return Color.DodgerBlue;
            if (temp < 60) return Color.SeaGreen;
            if (temp < 90) return Color.Orange;
            return Color.Crimson;
        }

        private void progressTemp_Paint(object sender, PaintEventArgs e)
        {
            // Sender'ı bir Panel olarak alıyoruz
            Panel barPanel = sender as Panel;
            if (barPanel == null || barPanel.Tag == null) return; // Tag kontrolü eklendi

            // Tag'den anlık değeri alıyoruz (eğer short atadıysanız short, int atadıysanız int olarak çekin)
            int currentValue = Convert.ToInt32(barPanel.Tag);
            int maximumValue = 150; // Max değeri burada sabit tuttuk (önceki gibi 150)

            // Değerin ProgressBar aralığında olduğundan emin olalım
            currentValue = Math.Max(0, Math.Min(maximumValue, currentValue));

            int controlWidth = barPanel.Width;
            int controlHeight = barPanel.Height;

            // Tüm arka planı temizle (varsayılan çizim müdahalesi olmayacak)
            e.Graphics.FillRectangle(new SolidBrush(Color.WhiteSmoke), 0, 0, controlWidth, controlHeight);

            // Dolu olması gereken yüksekliği hesapla
            int filledHeight = (int)(controlHeight * ((double)currentValue / maximumValue));

            // Dolu alanı çizeceğimiz dikdörtgeni tanımla
            Rectangle filledRect = new Rectangle(
                0, // X başlangıcı: Kontrolün sol kenarından başla
                controlHeight - filledHeight, // Y başlangıcı: Aşağıdan yukarıya dolum için
                controlWidth, // Genişlik: Kontrolün tam genişliğini kullan
                filledHeight // Yükseklik: Hesaplanan dolu alan yüksekliği
            );

            // Dolu alanı çiz
            e.Graphics.FillRectangle(new SolidBrush(GetTemperatureColor(currentValue)), filledRect);

            // Kenarlık çiz
            using (Pen borderPen = new Pen(Color.LightGray, 1))
            {
                e.Graphics.DrawRectangle(borderPen, 0, 0, controlWidth - 1, controlHeight - 1);
            }
        }

        private void lblMakineAdi_Click(object sender, EventArgs e)
        {

        }

        private void SafeInvoke(Action action)
        {
            if (this.IsHandleCreated && !this.IsDisposed)
            {
                try { this.BeginInvoke(action); }
                catch (Exception) { /* Form kapatılırken oluşabilecek hataları yoksay */ }
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            if (_pollingService != null)
            {
                _pollingService.OnMachineDataRefreshed -= OnDataRefreshed;
                _pollingService.OnMachineConnectionStateChanged -= OnConnectionStateChanged;
            }
            _uiUpdateTimer?.Stop();
            _uiUpdateTimer?.Dispose();
            base.OnHandleDestroyed(e);
        }
    }
}