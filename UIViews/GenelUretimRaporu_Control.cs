using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using TekstilScada.Models;
using TekstilScada.Repositories;

namespace TekstilScada.UI.Views
{
    public partial class GenelUretimRaporu_Control : UserControl
    {
        private MachineRepository _machineRepository;
        private ProductionRepository _productionRepository;
        private DataTable _reportData;
        private ListBox _activeSourceListBox; // Hangi gruptan seçim yapıldığını takip etmek için

        public GenelUretimRaporu_Control()
        {
            InitializeComponent();
        }

        public void InitializeControl(MachineRepository machineRepo, ProductionRepository productionRepo)
        {
            _machineRepository = machineRepo;
            _productionRepository = productionRepo;
        }

        private void GenelUretimRaporu_Control_Load(object sender, EventArgs e)
        {
            dtpStartTime.Value = DateTime.Today;
            dtpEndTime.Value = DateTime.Today.AddDays(1).AddSeconds(-1);
            LoadMachineLists();
        }

        private void LoadMachineLists()
        {
            var allMachines = _machineRepository.GetAllMachines();

            // Makineleri alt tiplerine göre grupla
            var groupedMachines = allMachines
                .Where(m => !string.IsNullOrEmpty(m.MachineSubType))
                .GroupBy(m => m.MachineSubType);

            flpMachineGroups.Controls.Clear();

            foreach (var group in groupedMachines)
            {
                // Her grup için bir GroupBox oluştur
                var groupBox = new GroupBox
                {
                    Text = group.Key,
                    Width = 250,
                    Height = 220
                };

                // Her grubun içine bir ListBox oluştur
                var listBox = new ListBox
                {
                    DataSource = group.ToList(),
                    DisplayMember = "MachineName",
                    Dock = DockStyle.Fill,
                    SelectionMode = SelectionMode.MultiExtended
                };

                // Olayları bağla
                listBox.Enter += (s, a) => { _activeSourceListBox = s as ListBox; };
                listBox.DoubleClick += (s, a) => { AddSelectedItems(); };

                groupBox.Controls.Add(listBox);
                flpMachineGroups.Controls.Add(groupBox);
            }
        }

        private void btnRaporOlustur_Click(object sender, EventArgs e)
        {
            var selectedMachines = listBoxSeciliMakineler.Items.Cast<Machine>().Select(m => m.MachineName).ToList();
            if (!selectedMachines.Any())
            {
                MessageBox.Show("Lütfen en az bir makine seçin.", "Uyarı");
                return;
            }

            try
            {
                this.Cursor = Cursors.WaitCursor;
                _reportData = _productionRepository.GetGeneralProductionReport(dtpStartTime.Value, dtpEndTime.Value, selectedMachines);
                dgvReport.DataSource = _reportData;
                FilterGridByConsumptionType();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Rapor oluşturulurken hata: {ex.Message}", "Hata");
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void radioConsumption_CheckedChanged(object sender, EventArgs e)
        {
            FilterGridByConsumptionType();
        }

        private void FilterGridByConsumptionType()
        {
            if (dgvReport.DataSource == null || dgvReport.Columns.Count == 0) return;

            // Önce tüm tüketim kolonlarını gizle
            if (dgvReport.Columns.Contains("TotalWater")) dgvReport.Columns["TotalWater"].Visible = false;
            if (dgvReport.Columns.Contains("TotalElectricity")) dgvReport.Columns["TotalElectricity"].Visible = false;
            if (dgvReport.Columns.Contains("TotalSteam")) dgvReport.Columns["TotalSteam"].Visible = false;

            // Sadece seçili olanı göster
            if (radioSu.Checked && dgvReport.Columns.Contains("TotalWater")) dgvReport.Columns["TotalWater"].Visible = true;
            if (radioElektrik.Checked && dgvReport.Columns.Contains("TotalElectricity")) dgvReport.Columns["TotalElectricity"].Visible = true;
            if (radioBuhar.Checked && dgvReport.Columns.Contains("TotalSteam")) dgvReport.Columns["TotalSteam"].Visible = true;
        }

        // --- Makine Ekleme/Çıkarma Butonları ---

        private void AddSelectedItems()
        {
            if (_activeSourceListBox == null || _activeSourceListBox.SelectedItems.Count == 0) return;

            foreach (var item in _activeSourceListBox.SelectedItems)
            {
                if (!listBoxSeciliMakineler.Items.Contains(item))
                {
                    listBoxSeciliMakineler.Items.Add(item);
                }
            }
            listBoxSeciliMakineler.DisplayMember = "DisplayInfo"; // Görünümü düzelt
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddSelectedItems();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            // Seçili olanları ters sırada silmek, index kaymasını önler
            for (int i = listBoxSeciliMakineler.SelectedIndices.Count - 1; i >= 0; i--)
            {
                listBoxSeciliMakineler.Items.RemoveAt(listBoxSeciliMakineler.SelectedIndices[i]);
            }
        }

        private void btnAddAll_Click(object sender, EventArgs e)
        {
            listBoxSeciliMakineler.Items.Clear();
            foreach (var groupBox in flpMachineGroups.Controls.OfType<GroupBox>())
            {
                var listBox = groupBox.Controls.OfType<ListBox>().FirstOrDefault();
                if (listBox != null)
                {
                    foreach (var item in listBox.Items)
                    {
                        if (!listBoxSeciliMakineler.Items.Contains(item))
                        {
                            listBoxSeciliMakineler.Items.Add(item);
                        }
                    }
                }
            }
            listBoxSeciliMakineler.DisplayMember = "DisplayInfo";
        }

        private void btnRemoveAll_Click(object sender, EventArgs e)
        {
            listBoxSeciliMakineler.Items.Clear();
        }
    }
}