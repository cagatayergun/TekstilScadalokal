// UI/Views/AlarmSettings_Control.cs
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TekstilScada.Models;
using TekstilScada.Repositories;

namespace TekstilScada.UI.Views
{
    public partial class AlarmSettings_Control : UserControl
    {
        private readonly AlarmRepository _repository;
        private List<AlarmDefinition> _definitions;
        private AlarmDefinition _selectedDefinition;

        public AlarmSettings_Control()
        {
            InitializeComponent();
            _repository = new AlarmRepository();
        }

        private void AlarmSettings_Control_Load(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void RefreshList()
        {
            try
            {
                _definitions = _repository.GetAllAlarmDefinitions();
                dgvAlarms.DataSource = null;
                dgvAlarms.DataSource = _definitions;
                if (dgvAlarms.Columns["Id"] != null) dgvAlarms.Columns["Id"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Alarm tanımları yüklenirken hata oluştu: {ex.Message}", "Veritabanı Hatası");
            }
        }

        private void dgvAlarms_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvAlarms.SelectedRows.Count > 0)
            {
                _selectedDefinition = dgvAlarms.SelectedRows[0].DataBoundItem as AlarmDefinition;
                if (_selectedDefinition != null)
                {
                    PopulateFields(_selectedDefinition);
                }
            }
        }

        private void PopulateFields(AlarmDefinition def)
        {
            numAlarmNo.Value = def.AlarmNumber;
            txtAlarmText.Text = def.AlarmText;
            numSeverity.Value = def.Severity;
            txtCategory.Text = def.Category;
        }

        private void ClearFields()
        {
            _selectedDefinition = null;
            dgvAlarms.ClearSelection();
            numAlarmNo.Value = 0;
            txtAlarmText.Text = "";
            numSeverity.Value = 1;
            txtCategory.Text = "";
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAlarmText.Text) || numAlarmNo.Value == 0)
            {
                MessageBox.Show("Alarm Numarası ve Alarm Metni zorunludur.", "Eksik Bilgi");
                return;
            }

            try
            {
                if (_selectedDefinition == null) // Yeni Kayıt
                {
                    var newDef = new AlarmDefinition
                    {
                        AlarmNumber = (int)numAlarmNo.Value,
                        AlarmText = txtAlarmText.Text,
                        Severity = (int)numSeverity.Value,
                        Category = txtCategory.Text
                    };
                    _repository.AddAlarmDefinition(newDef);
                }
                else // Güncelleme
                {
                    _selectedDefinition.AlarmNumber = (int)numAlarmNo.Value;
                    _selectedDefinition.AlarmText = txtAlarmText.Text;
                    _selectedDefinition.Severity = (int)numSeverity.Value;
                    _selectedDefinition.Category = txtCategory.Text;
                    _repository.UpdateAlarmDefinition(_selectedDefinition);
                }
                RefreshList();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kayıt sırasında hata: {ex.Message}", "Hata");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_selectedDefinition == null)
            {
                MessageBox.Show("Lütfen silmek için bir alarm tanımı seçin.", "Uyarı");
                return;
            }
            var result = MessageBox.Show($"'{_selectedDefinition.AlarmText}' alarm tanımını silmek istediğinizden emin misiniz?", "Onay", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                try
                {
                    _repository.DeleteAlarmDefinition(_selectedDefinition.Id);
                    RefreshList();
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Silme sırasında hata: {ex.Message}", "Hata");
                }
            }
        }
    }
}