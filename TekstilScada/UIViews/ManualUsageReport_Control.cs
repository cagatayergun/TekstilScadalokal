using System;
using System.Collections.Generic; // List için eklendi
using System.Windows.Forms;
using TekstilScada.Core;
using TekstilScada.Models;
using TekstilScada.Repositories;

namespace TekstilScada.UI.Views
{
    public partial class ManualUsageReport_Control : UserControl
    {
        private MachineRepository _machineRepository;
        private ProcessLogRepository _processLogRepository;

        public ManualUsageReport_Control()
        {
            InitializeComponent();
        }

        public void InitializeControl(MachineRepository machineRepo, ProcessLogRepository processLogRepo)
        {
            _machineRepository = machineRepo;
            _processLogRepository = processLogRepo;
        }

        private void ManualUsageReport_Control_Load(object sender, EventArgs e)
        {
            dtpStartTime.Value = DateTime.Today;
            dtpEndTime.Value = DateTime.Today.AddDays(1).AddSeconds(-1);

            var machines = _machineRepository.GetAllMachines();
            cmbMachines.DataSource = machines;
            cmbMachines.DisplayMember = "DisplayInfo";
            cmbMachines.ValueMember = "Id";
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            DateTime startTime = dtpStartTime.Value;
            DateTime endTime = dtpEndTime.Value;
            var selectedMachine = cmbMachines.SelectedItem as Machine;

            if (selectedMachine == null) return;

            try
            {
                this.Cursor = Cursors.WaitCursor;
                var summary = _processLogRepository.GetManualConsumptionSummary(selectedMachine.Id, selectedMachine.MachineName, startTime, endTime);
                
                var reportData = new List<ManualConsumptionSummary>();
                if (summary != null)
                {
                    reportData.Add(summary);
                }

                dgvManualUsage.DataSource = null;
                dgvManualUsage.DataSource = reportData;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Rapor oluşturulurken bir hata oluştu: {ex.Message}", "Hata");
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            ExcelExporter.ExportDataGridViewToExcel(dgvManualUsage);
        }
    }
}