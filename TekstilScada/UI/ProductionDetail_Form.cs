// UI/ProductionDetail_Form.cs
using ScottPlot; // ScottPlot'u kullanmak için
using System;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using TekstilScada.Core;
using TekstilScada.Core.Models;
using TekstilScada.Models;
using TekstilScada.Repositories;
using ChartLegend = System.Windows.Forms.DataVisualization.Charting.Legend;
namespace TekstilScada.UI
{
    public partial class ProductionDetail_Form : Form
    {
        private readonly ProductionReportItem _reportItem;
        private readonly ProductionRepository _productionRepo;
        private readonly ProcessLogRepository _processLogRepo;
        private readonly AlarmRepository _alarmRepo; // Alarmlar için eklendi
        private readonly RecipeRepository _recipeRepository; // YENİ

        public ProductionDetail_Form(ProductionReportItem reportItem, RecipeRepository recipeRepo, ProcessLogRepository processLogRepo, AlarmRepository alarmRepo)
        {
            InitializeComponent();
            _reportItem = reportItem;
            _productionRepo = new ProductionRepository();
            _processLogRepo = processLogRepo;
            _alarmRepo = alarmRepo;
            _recipeRepository = recipeRepo;
        }

        private void ProductionDetail_Form_Load(object sender, EventArgs e)
        {
            this.Text = $"Üretim Raporu Detayı - {_reportItem.BatchId}";

            // 1. Başlık bilgilerini doldur
            txtMachineName.Text = _reportItem.MachineName;
            txtRecipeName.Text = _reportItem.RecipeName;
            txtOperator.Text = _reportItem.OperatorName;
            txtStartTime.Text = _reportItem.StartTime.ToString("dd.MM.yyyy HH:mm:ss");
            txtStopTime.Text = _reportItem.EndTime.ToString("dd.MM.yyyy HH:mm:ss");
            txtTotalDuration.Text = _reportItem.CycleTime;

            // Diğer bilgileri de doldur
            txtCustomerNo.Text = _reportItem.MusteriNo;
            txtOrderNo.Text = _reportItem.SiparisNo;


            // 2. Adım detaylarını DataGridView'e yükle
            dgvStepDetails.DataSource = _productionRepo.GetProductionStepDetails(_reportItem.BatchId, _reportItem.MachineId);

            // 3. Alarm detaylarını yükle
            dgvAlarms.DataSource = _alarmRepo.GetAlarmDetailsForBatch(_reportItem.BatchId, _reportItem.MachineId);
            dgvStepDetails.CellFormatting += dgvStepDetails_CellFormatting;
            // 4. Zaman çizgisi grafiğini yükle
            // 1. Alarm ve Grafik Verilerini Yükle
        
            LoadTimelineChart(); // Mevcut zaman çizelgesi grafiğini yükle

            // 2. YENİ: Pasta Grafik Verilerini Hesapla ve Yükle
            LoadPieChart();

            
        }
        private void LoadPieChart()
        {
            // 1. HAZIR VERİLERİ AL
            double totalMachineAlarmSeconds = _reportItem.MachineAlarmDurationSeconds;
            double totalOperatorPauseSeconds = _reportItem.OperatorPauseDurationSeconds;
            double totalBatchSeconds = (_reportItem.EndTime - _reportItem.StartTime).TotalSeconds;

            // 2. AKTİF SÜREYİ HESAPLA
            double activeWorkingSeconds = totalBatchSeconds - totalMachineAlarmSeconds - totalOperatorPauseSeconds;
            if (activeWorkingSeconds < 0) activeWorkingSeconds = 0;

            // 3. GRAFİĞİ ÇİZ (Bu kod artık doğru ve basit)
            pieChartControl.Series.Clear();
            pieChartControl.Legends.Clear();

            Series series = new Series
            {
                Name = "Süre Dağılımı",
                IsVisibleInLegend = true,
                ChartType = SeriesChartType.Pie,
                Font = new System.Drawing.Font("Arial", 10f, System.Drawing.FontStyle.Bold),
                LabelForeColor = System.Drawing.Color.White
            };
            pieChartControl.Series.Add(series);

            if (activeWorkingSeconds > 1)
                series.Points.AddXY("Aktif Çalışma (dk)", Math.Round(activeWorkingSeconds / 60));
            if (totalMachineAlarmSeconds > 1)
                series.Points.AddXY("Makine Alarmı (dk)", Math.Round(totalMachineAlarmSeconds / 60));
            if (totalOperatorPauseSeconds > 1)
                series.Points.AddXY("Operatör Duraklatma (dk)", Math.Round(totalOperatorPauseSeconds / 60));

            series.IsValueShownAsLabel = true;
            series.Label = "#PERCENT{P0}";

            System.Windows.Forms.DataVisualization.Charting.Legend legend = new System.Windows.Forms.DataVisualization.Charting.Legend
            {
                Name = "Süreler",
                Docking = Docking.Bottom,
                Alignment = StringAlignment.Center
            };
            pieChartControl.Legends.Add(legend);
            series.LegendText = "#VALX";

            pieChartControl.Invalidate();
        }

        // FormLoad metodundan LoadPieChart çağrısını parametresiz yap
     


        private void dgvStepDetails_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Sadece "DeflectionTime" veya "Sapma" isimli sütun için çalış
            if (dgvStepDetails.Columns[e.ColumnIndex].DataPropertyName == "DeflectionTime" && e.Value != null)
            {
                string deflectionValue = e.Value.ToString();
                if (deflectionValue.StartsWith("+"))
                {
                    e.CellStyle.BackColor = System.Drawing.Color.FromArgb(255, 204, 203); // Açık Kırmızı
                    e.CellStyle.ForeColor = System.Drawing.Color.DarkRed;
                }
                else if (deflectionValue.StartsWith("-"))
                {
                    e.CellStyle.BackColor = System.Drawing.Color.FromArgb(204, 255, 204); // Açık Yeşil
                    e.CellStyle.ForeColor = System.Drawing.Color.DarkGreen;
                }
            }
        }
        private void LoadTimelineChart()
        {
            var dataPoints = _processLogRepo.GetLogsForBatch(_reportItem.MachineId, _reportItem.BatchId);
            formsPlot1.Plot.Clear();
            if (dataPoints.Any())
            {
                double[] timeData = dataPoints.Select(p => p.Timestamp.ToOADate()).ToArray();
                // DEĞİŞİKLİK: Sıcaklık verisini 10'a böl
                double[] tempData = dataPoints.Select(p => (double)p.Temperature / 10.0).ToArray();


                var tempPlot = formsPlot1.Plot.Add.Scatter(timeData, tempData);
                tempPlot.Color = ScottPlot.Colors.Red;
                tempPlot.LegendText = "Sıcaklık";
                // 2. YENİ: Teorik Veri Grafiğini Çiz
                var recipe = _recipeRepository.GetAllRecipes().FirstOrDefault(r => r.RecipeName == _reportItem.RecipeName);
                if (recipe != null)
                {
                    var fullRecipe = _recipeRepository.GetRecipeById(recipe.Id);
                    // RampCalculator'ı kullanarak teorik veriyi oluştur
                    var (theoTimestamps, theoTemperatures) = RampCalculator.GenerateTheoreticalRamp(fullRecipe, _reportItem.StartTime);

                    if (theoTimestamps.Any())
                    {
                        var theoPlot = formsPlot1.Plot.Add.Scatter(theoTimestamps, theoTemperatures);
                        theoPlot.Color = ScottPlot.Colors.Blue;
                        theoPlot.LegendText = "Teorik Sıcaklık";
                        theoPlot.LineStyle.Pattern = ScottPlot.LinePattern.Dashed;
                        theoPlot.LineWidth = 2;
                    }
                }

                formsPlot1.Plot.Axes.DateTimeTicksBottom();
                formsPlot1.Plot.Title($"{_reportItem.MachineName} - Proses Grafiği");
                formsPlot1.Plot.ShowLegend(ScottPlot.Alignment.UpperLeft);
                
        // --- YENİ KOD: GRAFİĞİ OTOMATİK YAKINLAŞTIRMA ---
        // AutoScale() yerine, eksen limitlerini manuel olarak belirliyoruz.
        DateTime startTime = _reportItem.StartTime;
        // Eğer üretim bitmemişse, bitiş zamanı olarak şimdiki zamanı al
        DateTime endTime = (_reportItem.EndTime == DateTime.MinValue) ? DateTime.Now : _reportItem.EndTime;

        // Grafiğin kenarlara yapışmaması için küçük bir boşluk (marj) ekleyelim
        startTime = startTime.AddMinutes(-5);
        endTime = endTime.AddMinutes(5);

        // X ekseninin (zaman) sınırlarını ayarla
        formsPlot1.Plot.Axes.SetLimitsX(startTime.ToOADate(), endTime.ToOADate());
        // Y eksenini (sıcaklık) ise kendi verisine göre otomatik ayarlamasını söyle
        formsPlot1.Plot.Axes.AutoScaleY(); 
                formsPlot1.Refresh();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            ExcelExporter.ExportProductionDetailToExcel(_reportItem, dgvStepDetails, dgvAlarms,formsPlot1);
            //ExportProductionDetailToExcel(ProductionReportItem headerData, DataGridView dgvSteps, DataGridView dgvAlarms)
        }
    }
}