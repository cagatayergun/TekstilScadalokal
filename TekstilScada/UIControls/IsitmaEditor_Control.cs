// UI/Controls/RecipeStepEditors/IsitmaEditor_Control.cs
using System;
using System.Windows.Forms;
using TekstilScada.Models;

namespace TekstilScada.UI.Controls.RecipeStepEditors
{
    public partial class IsitmaEditor_Control : UserControl
    {
        private IsitmaParams _params;
        public event EventHandler ValueChanged;

        public IsitmaEditor_Control()
        {
            InitializeComponent();
            numIsi.ValueChanged += OnValueChanged;
            numSure.ValueChanged += OnValueChanged;
            chkDirekBuhar.CheckedChanged += OnValueChanged;
            chkDolayliBuhar.CheckedChanged += OnValueChanged;
            chkTamburDur.CheckedChanged += OnValueChanged;
            chkAlarm.CheckedChanged += OnValueChanged;
        }

        public void LoadStep(ScadaRecipeStep step)
        {
            _params = new IsitmaParams(step.StepDataWords);

            numIsi.Value = _params.Isi / 10.0m;
            numSure.Value = _params.Sure;
            chkDirekBuhar.Checked = _params.DirekBuhar;
            chkDolayliBuhar.Checked = _params.DolayliBuhar;
            chkTamburDur.Checked = _params.TamburDur;
            chkAlarm.Checked = _params.Alarm;
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            if (_params == null) return;

            _params.Isi = (short)(numIsi.Value * 10);
            _params.Sure = (short)numSure.Value;
            _params.DirekBuhar = chkDirekBuhar.Checked;
            _params.DolayliBuhar = chkDolayliBuhar.Checked;
            _params.TamburDur = chkTamburDur.Checked;
            _params.Alarm = chkAlarm.Checked;

            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
