
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection; // Double Buffering için eklendi
using System.Windows.Forms;
using TekstilScada.Models;
using TekstilScada.Repositories;
using TekstilScada.Services;
using TekstilScada.UI.Controls;

namespace TekstilScada.UI.Views
{
    public partial class GenelBakis_Control : UserControl
    {
        private PlcPollingService _pollingService;
        private MachineRepository _machineRepository;
        private DashboardRepository _dashboardRepository;

        private readonly Dictionary<int, MachineStatusCard_Control> _machineCards = new Dictionary<int, MachineStatusCard_Control>();
        private System.Windows.Forms.Timer _refreshTimer;

        public GenelBakis_Control()
        {
            InitializeComponent();
            // HATA GİDERİLDİ: Akıcı bir güncelleme için Double Buffering'i aktif ediyoruz.
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            flpMachineGroups.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(flpMachineGroups, true, null);
        }

        public void InitializeControl(PlcPollingService pollingService, MachineRepository machineRepo, DashboardRepository dashboardRepo)
        {
            _pollingService = pollingService;
            _machineRepository = machineRepo;
            _dashboardRepository = dashboardRepo;
        }

        private void GenelBakis_Control_Load(object sender, EventArgs e)
        {
            if (this.DesignMode) return;

            BuildInitialLayout();

            _pollingService.OnMachineDataRefreshed += PollingService_OnMachineDataRefreshed;

            _refreshTimer = new System.Windows.Forms.Timer { Interval = 5000 };
            _refreshTimer.Tick += (s, a) => RefreshDashboard();
            _refreshTimer.Start();
        }

        private void BuildInitialLayout()
        {
            var allMachines = _machineRepository.GetAllEnabledMachines();
            _machineCards.Clear();
            foreach (var machine in allMachines)
            {
                var card = new MachineStatusCard_Control(machine);
                _machineCards.Add(machine.Id, card);
            }
            RefreshDashboard();
        }

        private void RefreshDashboard()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(RefreshDashboard));
                return;
            }

            SortAndRedrawMachineCards();
            UpdateKpiCards();
        }

        private void SortAndRedrawMachineCards()
        {
            // HATA GİDERİLDİ: Panelin yeniden çizimini geçici olarak durduruyoruz.
            flpMachineGroups.SuspendLayout();

            var allMachinesWithStatus = _machineRepository.GetAllEnabledMachines()
                .Select(m => new { Machine = m, Status = _pollingService.MachineDataCache.ContainsKey(m.Id) ? _pollingService.MachineDataCache[m.Id] : new FullMachineStatus() })
                .ToList();

            var sortedMachines = allMachinesWithStatus
                .OrderBy(x => GetStatusPriority(x.Status))
                .ThenByDescending(x => x.Machine.Id)
                .ToList();

            var groupedMachines = sortedMachines.GroupBy(x => x.Machine.MachineSubType);

            flpMachineGroups.Controls.Clear();

            foreach (var group in groupedMachines)
            {
                var groupPanel = new GroupBox
                {
                    Text = string.IsNullOrEmpty(group.Key) ? "Diğer Makineler" : group.Key,
                    AutoSize = true,
                    MinimumSize = new Size(flpMachineGroups.Width - 40, 0),
                    Font = new Font("Segoe UI", 12F, FontStyle.Bold)
                };

                var innerPanel = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    AutoSize = true,
                    Padding = new Padding(10)
                };

                foreach (var machineItem in group)
                {
                    if (_machineCards.ContainsKey(machineItem.Machine.Id))
                    {
                        innerPanel.Controls.Add(_machineCards[machineItem.Machine.Id]);
                    }
                }

                groupPanel.Controls.Add(innerPanel);
                flpMachineGroups.Controls.Add(groupPanel);
            }

            // HATA GİDERİLDİ: Tüm değişiklikler bittikten sonra panelin yeniden çizilmesine izin veriyoruz.
            flpMachineGroups.ResumeLayout(false);
            flpMachineGroups.PerformLayout();
        }

        private int GetStatusPriority(FullMachineStatus status)
        {
            if (status.HasActiveAlarm) return 0; // Alarm en öncelikli (0)
            if (status.IsInRecipeMode) return 1; // Çalışan ikinci (1)
            return 2;                            // Duran son (2)
        }

        private void PollingService_OnMachineDataRefreshed(int machineId, FullMachineStatus status)
        {
            if (_machineCards.TryGetValue(machineId, out var cardToUpdate))
            {
                cardToUpdate.UpdateStatus(status);
            }
        }

        private void UpdateKpiCards()
        {

            flpTopKpis.Controls.Clear();
            var allStatuses = _pollingService.MachineDataCache.Values;

            int totalMachines = allStatuses.Count;
            int runningMachines = allStatuses.Count(s => s.IsInRecipeMode && !s.HasActiveAlarm);
            int alarmMachines = allStatuses.Count(s => s.HasActiveAlarm);

            var kpi1 = new KpiCard_Control();
            kpi1.SetData("TOPLAM MAKİNE", totalMachines.ToString(), Color.FromArgb(41, 128, 185));

            var kpi2 = new KpiCard_Control();
            kpi2.SetData("AKTİF ÜRETİM", runningMachines.ToString(), Color.FromArgb(46, 204, 113));

            var kpi3 = new KpiCard_Control();
            kpi3.SetData("ALARM DURUMU", alarmMachines.ToString(), Color.FromArgb(231, 76, 60));

            flpTopKpis.Controls.Add(kpi1);
            flpTopKpis.Controls.Add(kpi2);
            flpTopKpis.Controls.Add(kpi3);

        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            if (_pollingService != null)
            {
                _pollingService.OnMachineDataRefreshed -= PollingService_OnMachineDataRefreshed;
            }
            _refreshTimer?.Stop();
            _refreshTimer?.Dispose();
            base.OnHandleDestroyed(e);
        }

    }
}

