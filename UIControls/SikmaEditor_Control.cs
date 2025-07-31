// UI/Controls/RecipeStepEditors/SikmaEditor_Control.cs
using System;
using System.Windows.Forms;
using TekstilScada.Models;

namespace TekstilScada.UI.Controls.RecipeStepEditors
{
    public partial class SikmaEditor_Control : UserControl
    {
        private SikmaParams _params;
        public event EventHandler ValueChanged;

        public SikmaEditor_Control()
        {
            InitializeComponent();
            numSikmaDevri.ValueChanged += OnValueChanged;
            numSikmaSure.ValueChanged += OnValueChanged;
        }

        public void LoadStep(ScadaRecipeStep step)
        {
            _params = new SikmaParams(step.StepDataWords);

            numSikmaDevri.Value = _params.SikmaDevri;
            numSikmaSure.Value = _params.SikmaSure;
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            if (_params == null) return;

            _params.SikmaDevri = (short)numSikmaDevri.Value;
            _params.SikmaSure = (short)numSikmaSure.Value;

            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}