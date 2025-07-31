// UI/Controls/RecipeStepEditors/BosaltmaEditor_Control.cs
using System;
using System.Windows.Forms;
using TekstilScada.Models;

namespace TekstilScada.UI.Controls.RecipeStepEditors
{
    public partial class BosaltmaEditor_Control : UserControl
    {
        private BosaltmaParams _params;
        public event EventHandler ValueChanged;

        public BosaltmaEditor_Control()
        {
            InitializeComponent();
            numSagSolSure.ValueChanged += OnValueChanged;
            numBeklemeZamani.ValueChanged += OnValueChanged;
            numCalismaDevri.ValueChanged += OnValueChanged;
            chkTamburDur.CheckedChanged += OnValueChanged;
            chkAlarm.CheckedChanged += OnValueChanged;
        }

        public void LoadStep(ScadaRecipeStep step)
        {
            _params = new BosaltmaParams(step.StepDataWords);

            numSagSolSure.Value = _params.SagSolSure;
            numBeklemeZamani.Value = _params.BeklemeZamani;
            numCalismaDevri.Value = _params.CalismaDevri;
            chkTamburDur.Checked = _params.TamburDur;
            chkAlarm.Checked = _params.Alarm;
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            if (_params == null) return;

            _params.SagSolSure = (short)numSagSolSure.Value;
            _params.BeklemeZamani = (short)numBeklemeZamani.Value;
            _params.CalismaDevri = (short)numCalismaDevri.Value;
            _params.TamburDur = chkTamburDur.Checked;
            _params.Alarm = chkAlarm.Checked;

            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}