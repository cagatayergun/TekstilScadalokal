// UI/Controls/RecipeStepEditors/DozajEditor_Control.cs
using System;
using System.Windows.Forms;
using TekstilScada.Models;

namespace TekstilScada.UI.Controls.RecipeStepEditors
{
    public partial class DozajEditor_Yikama_Control : UserControl
    {
        private DozajParams _params;
        public event EventHandler ValueChanged;

        public DozajEditor_Yikama_Control()
        {
            InitializeComponent();
            txtKimyasal.TextChanged += OnValueChanged;
            numTankSu.ValueChanged += OnValueChanged;
            numCozmeSure.ValueChanged += OnValueChanged;
            numDozajSure.ValueChanged += OnValueChanged;
            numDozajLitre.ValueChanged += OnValueChanged;
            chkAnaTankMakSu.CheckedChanged += OnValueChanged;
            chkAnaTankTemizSu.CheckedChanged += OnValueChanged;
            chkTank1Su.CheckedChanged += OnValueChanged;
            chkTank1Dozaj.CheckedChanged += OnValueChanged;
            chkTamburDur.CheckedChanged += OnValueChanged;
            chkAlarm.CheckedChanged += OnValueChanged;
        }

        public void LoadStep(ScadaRecipeStep step)
        {
            _params = new DozajParams(step.StepDataWords);

            txtKimyasal.Text = _params.Kimyasal;
            numTankSu.Value = _params.TankSu;
            numCozmeSure.Value = _params.CozmeSure;
            numDozajSure.Value = _params.DozajSure;
            numDozajLitre.Value = _params.DozajLitre;
            chkAnaTankMakSu.Checked = _params.AnaTankMakSu;
            chkAnaTankTemizSu.Checked = _params.AnaTankTemizSu;
            chkTank1Su.Checked = _params.Tank1Su;
            chkTank1Dozaj.Checked = _params.Tank1Dozaj;
            chkTamburDur.Checked = _params.TamburDur;
            chkAlarm.Checked = _params.Alarm;
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            if (_params == null) return;

            _params.Kimyasal = txtKimyasal.Text;
            _params.TankSu = (short)numTankSu.Value;
            _params.CozmeSure = (short)numCozmeSure.Value;
            _params.DozajSure = (short)numDozajSure.Value;
            _params.DozajLitre = (short)numDozajLitre.Value;
            _params.AnaTankMakSu = chkAnaTankMakSu.Checked;
            _params.AnaTankTemizSu = chkAnaTankTemizSu.Checked;
            _params.Tank1Su = chkTank1Su.Checked;
            _params.Tank1Dozaj = chkTank1Dozaj.Checked;
            _params.TamburDur = chkTamburDur.Checked;
            _params.Alarm = chkAlarm.Checked;

            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
