// UI/Views/AlarmReport_Control.cs
using System;
using System.Windows.Forms;
using TekstilScada.Core;
using TekstilScada.Models;
using TekstilScada.Repositories;

namespace TekstilScada.UI.Views
{
    // DÜZELTME: Sınıfın bir UserControl'den türediğini belirtiyoruz.
    public partial class AlarmReport_Control : UserControl
    {
        private MachineRepository _machineRepository;
        private AlarmRepository _alarmRepository;

        public AlarmReport_Control()
        {
            InitializeComponent();
        }

        public void InitializeControl(MachineRepository machineRepo, AlarmRepository alarmRepo)
        {
            _machineRepository = machineRepo;
            _alarmRepository = alarmRepo;
        }
        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            ExcelExporter.ExportDataGridViewToExcel(dgvAlarmReport);
        }
        private void AlarmReport_Control_Load(object sender, EventArgs e)
        {
            dtpStartTime.Value = DateTime.Today;
            dtpEndTime.Value = DateTime.Today.AddDays(1).AddSeconds(-1);

            var machines = _machineRepository.GetAllMachines();
            machines.Insert(0, new Machine { Id = -1, MachineName = "TÜM MAKİNELER" });
            cmbMachines.DataSource = machines;
            cmbMachines.DisplayMember = "MachineName";
            cmbMachines.ValueMember = "Id";
        }

        private async void btnGenerateReport_Click(object sender, EventArgs e)
        {
            DateTime startTime = dtpStartTime.Value;
            DateTime endTime = dtpEndTime.Value;
            int? machineId = (int)cmbMachines.SelectedValue == -1 ? (int?)null : (int)cmbMachines.SelectedValue;
            btnGenerateReport.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                var reportData = _alarmRepository.GetAlarmReport(startTime, endTime, machineId);
                dgvAlarmReport.DataSource = null;
                dgvAlarmReport.DataSource = reportData;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Rapor oluşturulurken bir hata oluştu: {ex.Message}", "Hata");
            }
            finally
            {
                this.Cursor = Cursors.Default;
                btnGenerateReport.Enabled = true;
            }
        }
    }
}