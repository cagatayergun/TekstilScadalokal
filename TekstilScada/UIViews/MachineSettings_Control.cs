// UI/Views/MachineSettings_Control.cs
using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Windows.Forms;
using TekstilScada.Models;
using TekstilScada.Repositories;

namespace TekstilScada.UI.Views
{
    public partial class MachineSettings_Control : UserControl
    {
        public event EventHandler MachineListChanged;

        private readonly MachineRepository _repository;
        private List<TekstilScada.Models.Machine> _machines;
        private TekstilScada.Models.Machine _selectedMachine;

        public MachineSettings_Control()
        {
            InitializeComponent();
            _repository = new MachineRepository();
        }

        private void MachineSettings_Control_Load(object sender, EventArgs e)
        {
            cmbMachineType.Items.Add("BYMakinesi");
            cmbMachineType.Items.Add("Kurutma Makinesi");
            cmbMachineType.SelectedIndex = 0;

            RefreshMachineList();
        }

        private void RefreshMachineList()
        {
            try
            {
                _machines = _repository.GetAllMachines();
                dgvMachines.DataSource = null;
                dgvMachines.DataSource = _machines;
                if (dgvMachines.Columns["Id"] != null) dgvMachines.Columns["Id"].Visible = false;
                // GEREKSİZ SÜTUNLARI GİZLE
                if (dgvMachines.Columns["VncPassword"] != null) dgvMachines.Columns["VncPassword"].Visible = false;
                if (dgvMachines.Columns["FtpPassword"] != null) dgvMachines.Columns["FtpPassword"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Makineler yüklenirken hata oluştu: {ex.Message}", "Veritabanı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvMachines_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvMachines.SelectedRows.Count > 0)
            {
                _selectedMachine = dgvMachines.SelectedRows[0].DataBoundItem as TekstilScada.Models.Machine;
                if (_selectedMachine != null)
                {
                    PopulateFields(_selectedMachine);
                }
            }
        }

        private void PopulateFields(TekstilScada.Models.Machine machine)
        {
            txtMachineId.Text = machine.MachineUserDefinedId;
            txtMachineName.Text = machine.MachineName;
            txtIpAddress.Text = machine.IpAddress;
            txtPort.Text = machine.Port.ToString();
            txtVncAddress.Text = machine.VncAddress;
            chkIsEnabled.Checked = machine.IsEnabled;
            cmbMachineType.SelectedItem = machine.MachineType;
            // YENİ: FTP alanlarını doldur
            txtFtpUsername.Text = machine.FtpUsername;
            txtFtpPassword.Text = machine.FtpPassword;
            txtMachineSubType.Text = machine.MachineSubType;
        }

        private void ClearFields()
        {
            _selectedMachine = null;
            dgvMachines.ClearSelection();
            txtMachineId.Text = "";
            txtMachineName.Text = "";
            txtIpAddress.Text = "";
            txtPort.Text = "2004";
            txtVncAddress.Text = "";
            chkIsEnabled.Checked = true;
            cmbMachineType.SelectedIndex = 0;
            // YENİ: FTP alanlarını temizle
            txtFtpUsername.Text = "";
            txtFtpPassword.Text = "";
            txtMachineSubType.Text = "";
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMachineId.Text) || string.IsNullOrWhiteSpace(txtIpAddress.Text) || string.IsNullOrWhiteSpace(txtMachineSubType.Text))
            {
                MessageBox.Show("Makine ID ve IP Adresi alanları zorunludur.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (_selectedMachine == null) // Yeni Kayıt
                {
                    var newMachine = new TekstilScada.Models.Machine
                    {
                        MachineUserDefinedId = txtMachineId.Text,
                        MachineName = txtMachineName.Text,
                        IpAddress = txtIpAddress.Text,
                        Port = int.Parse(txtPort.Text),
                        VncAddress = txtVncAddress.Text,
                        IsEnabled = chkIsEnabled.Checked,
                        MachineType = cmbMachineType.SelectedItem.ToString(),
                        // YENİ: FTP alanlarını oku
                        FtpUsername = txtFtpUsername.Text,
                        FtpPassword = txtFtpPassword.Text,
                        MachineSubType = txtMachineSubType.Text
                    }; 
                    _repository.AddMachine(newMachine);
                    MessageBox.Show("Yeni makine başarıyla eklendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else // Güncelleme
                {
                    _selectedMachine.MachineUserDefinedId = txtMachineId.Text;
                    _selectedMachine.MachineName = txtMachineName.Text;
                    _selectedMachine.IpAddress = txtIpAddress.Text;
                    _selectedMachine.Port = int.Parse(txtPort.Text);
                    _selectedMachine.VncAddress = txtVncAddress.Text;
                    _selectedMachine.IsEnabled = chkIsEnabled.Checked;
                    _selectedMachine.MachineType = cmbMachineType.SelectedItem.ToString();
                    // YENİ: FTP alanlarını oku
                    _selectedMachine.FtpUsername = txtFtpUsername.Text;
                    _selectedMachine.FtpPassword = txtFtpPassword.Text;
                    _selectedMachine.MachineSubType = txtMachineSubType.Text;
                    _repository.UpdateMachine(_selectedMachine);
                  
                    MessageBox.Show("Makine bilgileri başarıyla güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                RefreshMachineList();
                ClearFields();
                MachineListChanged?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kayıt sırasında bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedMachine == null)
            {
                MessageBox.Show("Lütfen silmek için bir makine seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show($"'{_selectedMachine.MachineName}' makinesini silmek istediğinizden emin misiniz?", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    _repository.DeleteMachine(_selectedMachine.Id);
                    RefreshMachineList();
                    ClearFields();
                    MachineListChanged?.Invoke(this, EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Silme işlemi sırasında bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
