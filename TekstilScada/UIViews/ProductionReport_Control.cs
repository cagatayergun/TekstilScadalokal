// UI/Views/ProductionReport_Control.cs
using System;
using System.Windows.Forms;
using TekstilScada.Core;
using TekstilScada.Models;
using TekstilScada.Repositories;
using TekstilScada.UI;
using static TekstilScada.Repositories.ProductionRepository;

namespace TekstilScada.UI.Views
{
    public partial class ProductionReport_Control : UserControl
    {
        private MachineRepository _machineRepository;
        private ProductionRepository _productionRepository;
        private RecipeRepository _recipeRepository; // YENİ
        private ProcessLogRepository _processLogRepo; // HATA GİDERİLDİ: Alan eklendi
        private AlarmRepository _alarmRepo; // HATA GİDERİLDİ: Alan eklendi
        public ProductionReport_Control()
        {
            InitializeComponent();
        }

        public void InitializeControl(MachineRepository machineRepo, ProductionRepository productionRepo, RecipeRepository recipeRepo, ProcessLogRepository processLogRepo , AlarmRepository alarmRepo)
        {
            _machineRepository = machineRepo;
            _productionRepository = productionRepo;
            _recipeRepository = recipeRepo; // YENİ
            _processLogRepo = processLogRepo; // Gelen parametre alana atandı
            _alarmRepo = alarmRepo;           // Gelen parametre alana atandı
        }

        private void ProductionReport_Control_Load(object sender, EventArgs e)
        {
            dtpStartTime.Value = DateTime.Today.AddDays(-7);
            dtpEndTime.Value = DateTime.Now;

            var machines = _machineRepository.GetAllMachines();
            machines.Insert(0, new Machine { Id = -1, MachineName = "TÜM MAKİNELER", MachineUserDefinedId = "" });
            cmbMachines.DataSource = machines;
            cmbMachines.DisplayMember = "DisplayInfo";
            cmbMachines.ValueMember = "Id";
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            var filters = new ReportFilters
            {
                StartTime = dtpStartTime.Value,
                EndTime = dtpEndTime.Value,
                MachineId = (int)cmbMachines.SelectedValue == -1 ? (int?)null : (int)cmbMachines.SelectedValue,
                BatchNo = txtBatchNo.Text,
                RecipeName = txtRecipeName.Text,
                SiparisNo = txtOrderNo.Text,
                MusteriNo = txtCustomerNo.Text,
                OperatorName = txtOperator.Text
            };

            try
            {
                this.Cursor = Cursors.WaitCursor;
                var reportData = _productionRepository.GetProductionReport(filters);
                dgvProductionReport.DataSource = null;
                dgvProductionReport.DataSource = reportData;
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

        private void dgvProductionReport_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedReportItem = dgvProductionReport.Rows[e.RowIndex].DataBoundItem as ProductionReportItem;
                if (selectedReportItem != null)
                {
                    // GÜNCELLENDİ: Detail form'a recipeRepository de gönderiliyor
                    var detailForm = new ProductionDetail_Form(selectedReportItem, _recipeRepository, _processLogRepo, _alarmRepo);
                    detailForm.Show();
                }
            }
        }

        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            ExcelExporter.ExportDataGridViewToExcel(dgvProductionReport);
        }
    }
}