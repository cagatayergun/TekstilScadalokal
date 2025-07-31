using System;
using System.Windows.Forms;
using TekstilScada.Models;
using TekstilScada.Repositories;

namespace TekstilScada.UI.Views
{
    public partial class OeeReport_Control : UserControl
    {
        private MachineRepository _machineRepository;
        private DashboardRepository _dashboardRepository;

        public OeeReport_Control()
        {
            InitializeComponent();
        }

        public void InitializeControl(MachineRepository machineRepo, DashboardRepository dashboardRepo)
        {
            _machineRepository = machineRepo;
            _dashboardRepository = dashboardRepo;
        }

        private void OeeReport_Control_Load(object sender, EventArgs e)
        {
            dtpStartTime.Value = DateTime.Today.AddDays(-7);
            dtpEndTime.Value = DateTime.Now;

            var machines = _machineRepository.GetAllMachines();
            machines.Insert(0, new Machine { Id = -1, MachineName = "TÜM MAKİNELER" });
            cmbMachines.DataSource = machines;
            cmbMachines.DisplayMember = "MachineName";
            cmbMachines.ValueMember = "Id";
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            DateTime startTime = dtpStartTime.Value;
            DateTime endTime = dtpEndTime.Value;
            int? machineId = (int)cmbMachines.SelectedValue == -1 ? (int?)null : (int)cmbMachines.SelectedValue;

            try
            {
                this.Cursor = Cursors.WaitCursor;
                var reportData = _dashboardRepository.GetOeeReport(startTime, endTime, machineId);
                dgvOeeReport.DataSource = null;
                dgvOeeReport.DataSource = reportData;
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
    }
}