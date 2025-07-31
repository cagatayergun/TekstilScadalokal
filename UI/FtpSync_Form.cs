using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TekstilScada.Models;
using TekstilScada.Repositories;
using TekstilScada.Services;

namespace TekstilScada.UI
{
    public partial class FtpSync_Form : Form
    {
        private readonly MachineRepository _machineRepository;
        private readonly RecipeRepository _recipeRepository;
        private readonly FtpTransferService _transferService;

        public FtpSync_Form(MachineRepository machineRepo, RecipeRepository recipeRepo)
        {
            InitializeComponent();
            _machineRepository = machineRepo;
            _recipeRepository = recipeRepo;
            _transferService = FtpTransferService.Instance; // Singleton servisi al
        }

        private void FtpSync_Form_Load(object sender, EventArgs e)
        {
            // Makineleri ve reçeteleri yükle
            LoadMachines();
            LoadLocalRecipes();

            // İlerleme tablosunu ayarla
            SetupTransfersGrid();

            // Mevcut işleri listele ve ilerleme durumlarını dinlemeye başla
            dgvTransfers.DataSource = _transferService.Jobs;
            _transferService.Jobs.ListChanged += Jobs_ListChanged;
        }

        private void LoadMachines()
        {
            // Sadece FTP kullanıcı adı tanımlanmış makineleri listele
            var machines = _machineRepository.GetAllEnabledMachines()
                .Where(m => !string.IsNullOrEmpty(m.FtpUsername)).ToList();
            cmbMachines.DataSource = machines;
            cmbMachines.DisplayMember = "DisplayInfo";
            cmbMachines.ValueMember = "Id";
        }

        private void LoadLocalRecipes()
        {
            lstLocalRecipes.DataSource = _recipeRepository.GetAllRecipes();
            lstLocalRecipes.DisplayMember = "RecipeName";
            lstLocalRecipes.ValueMember = "Id";
        }

        private async void LoadHmiRecipes()
        {
            var selectedMachine = cmbMachines.SelectedItem as Machine;
            if (selectedMachine == null)
            {
                lstHmiRecipes.DataSource = null;
                return;
            }

            // YENİ: Makinenin FTP bilgilerinin dolu olup olmadığını kontrol et
            if (string.IsNullOrEmpty(selectedMachine.IpAddress) || string.IsNullOrEmpty(selectedMachine.FtpUsername))
            {
                MessageBox.Show("Seçilen makine için FTP IP adresi veya kullanıcı adı tanımlanmamış.", "Eksik Bilgi");
                lstHmiRecipes.DataSource = null;
                return;
            }

            btnRefreshHmi.Enabled = false;
            lstHmiRecipes.DataSource = new List<string> { "Yükleniyor..." };

            try
            {
                var ftpService = new FtpService(selectedMachine.IpAddress, selectedMachine.FtpUsername, selectedMachine.FtpPassword);
                var files = await ftpService.ListDirectoryAsync("/");
                var recipeFiles = files
                    .Where(f => f.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                    .OrderBy(f => f)
                    .ToList();
                lstHmiRecipes.DataSource = recipeFiles;
            }
            catch (Exception ex)
            {
                // YENİ: Hata mesajını daha anlaşılır hale getir
                string errorMessage = $"'{selectedMachine.MachineName}' makinesinin FTP sunucusuna bağlanılamadı.\n\n" +
                                      "Olası Nedenler:\n" +
                                      "- Makine kapalı veya ağa bağlı değil.\n" +
                                      "- FTP kullanıcı adı veya şifresi yanlış.\n" +
                                      "- Ağ güvenlik duvarı bağlantıyı engelliyor.\n\n" +
                                      $"Teknik Detay: {ex.Message}";
                MessageBox.Show(errorMessage, "FTP Bağlantı Hatası");
                lstHmiRecipes.DataSource = null;
            }
            finally
            {
                btnRefreshHmi.Enabled = true;
            }
        }

        private void SetupTransfersGrid()
        {
            dgvTransfers.AutoGenerateColumns = false;
            dgvTransfers.Columns.Clear();

            dgvTransfers.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MakineAdi", HeaderText = "Makine", FillWeight = 150 });
            dgvTransfers.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ReceteAdi", HeaderText = "Reçete/Dosya", FillWeight = 200 });
            dgvTransfers.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "IslemTipi", HeaderText = "İşlem" });
            dgvTransfers.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Durum", HeaderText = "Durum" });
            dgvTransfers.Columns.Add(new DataGridViewProgressBarColumn { DataPropertyName = "Ilerleme", HeaderText = "İlerleme" });
            dgvTransfers.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "HataMesaji", HeaderText = "Hata", FillWeight = 250 });
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            var selectedRecipes = lstLocalRecipes.SelectedItems.Cast<ScadaRecipe>().ToList();
            var selectedMachine = cmbMachines.SelectedItem as Machine;

            if (!selectedRecipes.Any() || selectedMachine == null)
            {
                MessageBox.Show("Lütfen en az bir reçete ve bir hedef makine seçin.", "Uyarı");
                return;
            }
            _transferService.QueueSendJobs(selectedRecipes, selectedMachine);
        }

        private void btnReceive_Click(object sender, EventArgs e)
        {
            var selectedFiles = lstHmiRecipes.SelectedItems.Cast<string>().ToList();
            var selectedMachine = cmbMachines.SelectedItem as Machine;

            if (!selectedFiles.Any() || selectedMachine == null)
            {
                MessageBox.Show("Lütfen en az bir HMI dosyası ve bir kaynak makine seçin.", "Uyarı");
                return;
            }
            _transferService.QueueReceiveJobs(selectedFiles, selectedMachine);
        }

        private void cmbMachines_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadHmiRecipes();
        }

        private void btnRefreshHmi_Click(object sender, EventArgs e)
        {
            LoadHmiRecipes();
        }

        private void Jobs_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (this.IsDisposed || !this.IsHandleCreated) return;

            if (dgvTransfers.InvokeRequired)
            {
                dgvTransfers.Invoke(new Action(() => dgvTransfers.Refresh()));
            }
            else
            {
                dgvTransfers.Refresh();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _transferService.Jobs.ListChanged -= Jobs_ListChanged;
            base.OnFormClosing(e);
        }
    }

    // DataGridView için özel ProgressBar kolonu
    public class DataGridViewProgressBarColumn : DataGridViewTextBoxColumn
    {
        public DataGridViewProgressBarColumn()
        {
            this.CellTemplate = new DataGridViewProgressBarCell();
        }
    }

    public class DataGridViewProgressBarCell : DataGridViewTextBoxCell
    {
        protected override void Paint(Graphics g, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            base.Paint(g, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts & ~DataGridViewPaintParts.ContentForeground);

            int progressVal = (value == null) ? 0 : (int)value;
            float percentage = ((float)progressVal / 100.0f);

            if (percentage > 0.0)
            {
                g.FillRectangle(new SolidBrush(Color.FromArgb(180, 220, 180)), cellBounds.X + 2, cellBounds.Y + 2, Convert.ToInt32((percentage * cellBounds.Width - 4)), cellBounds.Height - 4);
            }

            string text = progressVal.ToString() + "%";
            SizeF textSize = g.MeasureString(text, cellStyle.Font);
            float textX = cellBounds.X + (cellBounds.Width - textSize.Width) / 2;
            float textY = cellBounds.Y + (cellBounds.Height - textSize.Height) / 2;
            g.DrawString(text, cellStyle.Font, Brushes.Black, textX, textY);
        }
    }
}
