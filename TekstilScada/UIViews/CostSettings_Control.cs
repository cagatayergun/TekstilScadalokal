using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TekstilScada.Core;
using TekstilScada.Models;
using TekstilScada.Properties;
using TekstilScada.Repositories;

namespace TekstilScada.UI.Views
{
    public partial class CostSettings_Control : UserControl
    {
        private readonly CostRepository _repository;
        private List<CostParameter> _parameters;

        public CostSettings_Control()
        {
            LanguageManager.LanguageChanged += LanguageManager_LanguageChanged;
            InitializeComponent();
            _repository = new CostRepository();
        }

        private void CostSettings_Control_Load(object sender, EventArgs e)
        {
            LoadParameters();
        }
        private void LanguageManager_LanguageChanged(object sender, EventArgs e)
        {
            ApplyLocalization();

        }
        public void ApplyLocalization()
        {
            btnSave.Text = Resources.Save;
           
            //btnSave.Text = Resources.Save;


        }
        private void LoadParameters()
        {
            try
            {
                _parameters = _repository.GetAllParameters();
                dgvCostParameters.DataSource = _parameters;
                if (dgvCostParameters.Columns["Id"] != null) dgvCostParameters.Columns["Id"].Visible = false;

                // YENİ: Kolon başlıklarını ve formatlarını düzenle
                if (dgvCostParameters.Columns["ParameterName"] != null) dgvCostParameters.Columns["ParameterName"].HeaderText = "Parametre";
                if (dgvCostParameters.Columns["CostValue"] != null) dgvCostParameters.Columns["CostValue"].HeaderText = "Birim Maliyet";
                if (dgvCostParameters.Columns["Unit"] != null) dgvCostParameters.Columns["Unit"].HeaderText = "Birim";
                if (dgvCostParameters.Columns["Multiplier"] != null) dgvCostParameters.Columns["Multiplier"].HeaderText = "Çarpan";
                if (dgvCostParameters.Columns["CurrencySymbol"] != null) dgvCostParameters.Columns["CurrencySymbol"].HeaderText = "Para Birimi";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Maliyet parametreleri yüklenirken hata oluştu: {ex.Message}", "Veritabanı Hatası");
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Değişiklikleri DataGridView'den al
                var updatedParameters = (List<CostParameter>)dgvCostParameters.DataSource;
                _repository.UpdateParameters(updatedParameters);
                MessageBox.Show("Maliyet parametreleri başarıyla güncellendi.", "Başarılı");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Parametreler kaydedilirken hata oluştu: {ex.Message}", "Hata");
            }
        }
    }
}