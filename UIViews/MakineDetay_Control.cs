﻿// UI/Views/MakineDetay_Control.cs
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
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
        private Machine _machine;
        private System.Windows.Forms.Timer _uiUpdateTimer;
        private string _lastLoadedBatchId = null;
        private string _lastLoadedRecipeName = null;

        public MakineDetay_Control()
        {
            InitializeComponent();
            btnGeri.Click += (sender, args) => BackRequested?.Invoke(this, EventArgs.Empty);
            this.progressTemp.Paint += new System.Windows.Forms.PaintEventHandler(this.progressTemp_Paint);
        }

        public void InitializeControl(Machine machine, PlcPollingService service, ProcessLogRepository logRepo, AlarmRepository alarmRepo, RecipeRepository recipeRepo)
        {
            _machine = machine;
            _pollingService = service;
            _logRepository = logRepo;
            _alarmRepository = alarmRepo;
            _recipeRepository = recipeRepo;

            _uiUpdateTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            _uiUpdateTimer.Tick += (sender, args) => UpdateLiveGauges();
            _uiUpdateTimer.Start();
            _pollingService.OnMachineDataRefreshed += OnDataRefreshed;
            _pollingService.OnMachineConnectionStateChanged += OnConnectionStateChanged;

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
            if (machineId == _machine.Id)
            {
                SafeInvoke(() => UpdateUI(status));
            }
        }

        private void OnDataRefreshed(int machineId, FullMachineStatus status)
        {
            if (machineId == _machine.Id)
            {
                SafeInvoke(() => UpdateUI(status));
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
            lblMakineAdi.Text = status.MachineName;

            if (status.ConnectionState != ConnectionStatus.Connected)
            {
                ClearAllFields();
                lblMakineAdi.Text = _machine.MachineName;
                return;
            }

            if (status.BatchNumarasi != _lastLoadedBatchId || status.RecipeName != _lastLoadedRecipeName)
            {
                LoadBatchAndRecipeData(status);
                LoadTimelineChart(status.BatchNumarasi);
            }

            lblReceteAdi.Text = status.RecipeName;
            lblOperator.Text = status.OperatorIsmi;
            lblMusteriNo.Text = status.MusteriNumarasi;
            lblBatchNo.Text = status.BatchNumarasi;
            lblSiparisNo.Text = status.SiparisNumarasi;
            lblCalisanAdim.Text = $"#{status.AktifAdimNo} - {status.AktifAdimAdi}";
            HighlightCurrentStep(status.AktifAdimNo);
        }

        private void LoadBatchAndRecipeData(FullMachineStatus status)
        {
            _lastLoadedBatchId = status.BatchNumarasi;
            _lastLoadedRecipeName = status.RecipeName;

            lstAlarmlar.DataSource = null;
            if (!string.IsNullOrEmpty(status.BatchNumarasi))
            {
                var alarms = _alarmRepository.GetAlarmDetailsForBatch(status.BatchNumarasi, _machine.Id);
                lstAlarmlar.DataSource = alarms.Select(a => a.AlarmDescription).ToList();
            }

            dgvAdimlar.DataSource = null;
            if (!string.IsNullOrEmpty(status.RecipeName))
            {
                var recipe = _recipeRepository.GetAllRecipes().FirstOrDefault(r => r.RecipeName == status.RecipeName);
                if (recipe != null)
                {
                    var fullRecipe = _recipeRepository.GetRecipeById(recipe.Id);
                    dgvAdimlar.DataSource = fullRecipe.Steps.Select(s => new { Adım = s.StepNumber, Açıklama = GetStepTypeName(s) }).ToList();
                }
            }
        }

        private void HighlightCurrentStep(int currentStepNumber)
        {
            foreach (DataGridViewRow row in dgvAdimlar.Rows)
            {
                if (row.Cells["Adım"].Value != null && (int)row.Cells["Adım"].Value == currentStepNumber)
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

        private void ClearAllFields()
        {
            lblReceteAdi.Text = "---";
            lblOperator.Text = "---";
            lblMusteriNo.Text = "---";
            lblBatchNo.Text = "---";
            lblSiparisNo.Text = "---";
            lblCalisanAdim.Text = "---";
            gaugeRpm.Value = 0;
            gaugeRpm.Text = "0";
            progressTemp.Value = 0;
            lblTempValue.Text = "0 °C";
            waterTankGauge1.Value = 0;
            lstAlarmlar.DataSource = null;
            dgvAdimlar.DataSource = null;
        }

        private void LoadTimelineChart(string batchId)
        {
            SafeInvoke(() =>
            {
                formsPlot1.Plot.Clear();
                if (string.IsNullOrEmpty(batchId))
                {
                    formsPlot1.Plot.Title("Aktif bir Batch bulunamadı.");
                    formsPlot1.Refresh();
                    return;
                }

                try
                {
                    var dataPoints = _logRepository.GetLogsForBatch(_machine.Id, batchId);

                    if (dataPoints.Any())
                    {
                        double[] timeData = dataPoints.Select(p => p.Timestamp.ToOADate()).ToArray();
                        double[] tempData = dataPoints.Select(p => (double)p.Temperature).ToArray();

                        var tempPlot = formsPlot1.Plot.Add.Scatter(timeData, tempData);
                        tempPlot.Color = ScottPlot.Colors.Red;
                        tempPlot.LegendText = "Sıcaklık";
                        tempPlot.LineWidth = 2;

                        formsPlot1.Plot.Axes.DateTimeTicksBottom();
                        formsPlot1.Plot.Title($"{_machine.MachineName} - Proses Grafiği");
                        formsPlot1.Plot.ShowLegend(ScottPlot.Alignment.UpperLeft);
                        formsPlot1.Plot.Axes.AutoScale();
                    }
                    else
                    {
                        formsPlot1.Plot.Title("Bu Batch için henüz veri kaydedilmemiş.");
                    }
                    formsPlot1.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Grafik verileri yüklenirken hata oluştu: {ex.Message}", "Grafik Hatası");
                }
            });
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

        private void SafeInvoke(Action action)
        {
            if (this.IsHandleCreated && !this.IsDisposed)
            {
                try { this.BeginInvoke(action); }
                catch { }
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

        private void lblMakineAdi_Click(object sender, EventArgs e)
        {

        }
    }
}