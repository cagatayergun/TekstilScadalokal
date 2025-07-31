// UI/Controls/RecipeStepEditors/CalismaEditor_Control.cs
using System;
using System.Windows.Forms;
using TekstilScada.Models;

namespace TekstilScada.UI.Controls.RecipeStepEditors
{
    public partial class CalismaEditor_Control : UserControl
    {
        private CalismaParams _params;
        public event EventHandler ValueChanged;

        public CalismaEditor_Control()
        {
            InitializeComponent();
            numSagSolSure.ValueChanged += OnValueChanged;
            numBeklemeSuresi.ValueChanged += OnValueChanged;
            numCalismaDevri.ValueChanged += OnValueChanged;
            numCalismaSuresi.ValueChanged += OnValueChanged;
            chkIsiKontrol.CheckedChanged += OnValueChanged;
            chkAlarm.CheckedChanged += OnValueChanged;
        }

        public void LoadStep(ScadaRecipeStep step)
        {
            _params = new CalismaParams(step.StepDataWords);

            numSagSolSure.Value = _params.SagSolSure;
            numBeklemeSuresi.Value = _params.BeklemeSuresi;
            numCalismaDevri.Value = _params.CalismaDevri;
            numCalismaSuresi.Value = _params.CalismaSuresi;
            chkIsiKontrol.Checked = _params.IsiKontrol;
            chkAlarm.Checked = _params.Alarm;
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            if (_params == null) return;

            _params.SagSolSure = (short)numSagSolSure.Value;
            _params.BeklemeSuresi = (short)numBeklemeSuresi.Value;
            _params.CalismaDevri = (short)numCalismaDevri.Value;
            _params.CalismaSuresi = (short)numCalismaSuresi.Value;
            _params.IsiKontrol = chkIsiKontrol.Checked;
            _params.Alarm = chkAlarm.Checked;

            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}