// UI/Controls/KurutmaReçete_Control.cs
using System;
using System.Windows.Forms;
using TekstilScada.Models;

namespace TekstilScada.UI.Controls
{
    public partial class KurutmaReçete_Control : UserControl
    {
        private ScadaRecipeStep _recipeStep;
        public event EventHandler ValueChanged;

        public KurutmaReçete_Control()
        {
            InitializeComponent();
            numSicaklik.ValueChanged += OnValueChanged;
            numNem.ValueChanged += OnValueChanged;
            numZaman.ValueChanged += OnValueChanged;
            numCalismaDevri.ValueChanged += OnValueChanged;
            numSogutmaZamani.ValueChanged += OnValueChanged;
            chkNemAktif.CheckedChanged += OnValueChanged;
            chkZamanAktif.CheckedChanged += OnValueChanged;
        }

        public void LoadRecipe(ScadaRecipe recipe)
        {
            if (recipe != null && recipe.Steps.Count > 0)
            {
                _recipeStep = recipe.Steps[0];

                // Değerleri PLC hafıza haritasına göre kontrollerden oku
                // Word0 = Sıcaklık, Word1 = Nem, Word2 = Zaman
                // Word3 = Çalışma Devri, Word4 = Soğutma Zamanı
                numSicaklik.Value = _recipeStep.StepDataWords[0];
                numNem.Value = _recipeStep.StepDataWords[1];
                numZaman.Value = _recipeStep.StepDataWords[2];
                numCalismaDevri.Value = _recipeStep.StepDataWords[3];
                numSogutmaZamani.Value = _recipeStep.StepDataWords[4];

                // Kontrol bitlerini oku (Word 5)
                short controlWord = _recipeStep.StepDataWords[5];
                chkNemAktif.Checked = (controlWord & 1) != 0;      // Bit 0
                chkZamanAktif.Checked = (controlWord & 2) != 0;    // Bit 1
            }
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            if (_recipeStep == null) return;

            // Değişiklikleri anında _recipeStep nesnesine kaydet
            _recipeStep.StepDataWords[0] = (short)numSicaklik.Value;
            _recipeStep.StepDataWords[1] = (short)numNem.Value;
            _recipeStep.StepDataWords[2] = (short)numZaman.Value;
            _recipeStep.StepDataWords[3] = (short)numCalismaDevri.Value;
            _recipeStep.StepDataWords[4] = (short)numSogutmaZamani.Value;

            // Kontrol bitlerini yaz (Word 5)
            short controlWord = 0;
            if (chkNemAktif.Checked) controlWord |= 1;  // Bit 0
            if (chkZamanAktif.Checked) controlWord |= 2; // Bit 1
            _recipeStep.StepDataWords[5] = controlWord;

            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
