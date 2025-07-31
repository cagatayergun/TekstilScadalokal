// UI/Controls/RecipeStepEditors/SuAlmaEditor_Control.cs
using System;
using System.Windows.Forms;
using TekstilScada.Models;

namespace TekstilScada.UI.Controls.RecipeStepEditors
{
    public partial class SuAlmaEditor_Control : UserControl
    {
        private SuAlmaParams _params;
        public event EventHandler ValueChanged;

        public SuAlmaEditor_Control()
        {
            InitializeComponent();
            numLitre.ValueChanged += OnValueChanged;
            chkSicakSu.CheckedChanged += OnValueChanged;
            chkSogukSu.CheckedChanged += OnValueChanged;
            chkYumusakSu.CheckedChanged += OnValueChanged;
            chkTamburDur.CheckedChanged += OnValueChanged;
            chkAlarm.CheckedChanged += OnValueChanged;
        }

        public void LoadStep(ScadaRecipeStep step)
        {
            _params = new SuAlmaParams(step.StepDataWords);

            numLitre.Value = _params.MiktarLitre;
            chkSicakSu.Checked = _params.SicakSu;
            chkSogukSu.Checked = _params.SogukSu;
            chkYumusakSu.Checked = _params.YumusakSu;
            chkTamburDur.Checked = _params.TamburDur;
            chkAlarm.Checked = _params.Alarm;
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            if (_params == null) return;

            _params.MiktarLitre = (short)numLitre.Value;
            _params.SicakSu = chkSicakSu.Checked;
            _params.SogukSu = chkSogukSu.Checked;
            _params.YumusakSu = chkYumusakSu.Checked;
            _params.TamburDur = chkTamburDur.Checked;
            _params.Alarm = chkAlarm.Checked;

            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}