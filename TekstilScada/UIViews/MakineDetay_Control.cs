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
                    progressTemp.Value = Math.Min(progressTemp.Maximum, status.AnlikSicaklik);
                    lblTempValue.Text = $"{status.AnlikSicaklik} °C";
                    lblTempValue.ForeColor = GetTemperatureColor(status.AnlikSicaklik);
                    progressTemp.Invalidate();
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

                formsPlot1.Plot.Axes.DateTimeTicksBottom();
                formsPlot1.Plot.ShowLegend(ScottPlot.Alignment.UpperLeft);
                formsPlot1.Plot.Axes.AutoScale();
                formsPlot1.Refresh();
            });
        }

        private void LoadTimelineChartForLive()
        {
            SafeInvoke(() =>
            {
                formsPlot1.Plot.Clear();
                DateTime startTime = DateTime.Now.AddMinutes(-30);
                DateTime endTime = DateTime.Now;

                // --- DEĞİŞİKLİK BURADA ---
                // process_data_log yerine manual_mode_log tablosundan veri çekiyoruz.
                var dataPoints = _logRepository.GetManualLogs(_machine.Id, startTime, endTime);

                if (!dataPoints.Any())
                {
                    formsPlot1.Plot.Title("Canlı veri akışı bekleniyor...");
                    formsPlot1.Refresh();
                    return;
                }

                formsPlot1.Plot.Title($"{_machine.MachineName} - Canlı Proses Verileri (Son 30 Dakika)");
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

                formsPlot1.Plot.Axes.DateTimeTicksBottom();
                formsPlot1.Plot.ShowLegend(ScottPlot.Alignment.UpperLeft);
                formsPlot1.Plot.Axes.SetLimitsX(startTime.ToOADate(), endTime.AddMinutes(1).ToOADate());
                formsPlot1.Plot.Axes.AutoScaleY();
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
            ProgressBar bar = sender as ProgressBar;
            if (bar == null || bar.Maximum == 0) return;

            Rectangle rec = e.ClipRectangle;
            int barHeight = (int)(e.ClipRectangle.Height * ((double)bar.Value / bar.Maximum));
            rec.Y = e.ClipRectangle.Height - barHeight;
            rec.Height = barHeight;

            e.Graphics.FillRectangle(new SolidBrush(GetTemperatureColor(bar.Value)), rec);
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