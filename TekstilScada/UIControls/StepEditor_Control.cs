// UI/Controls/RecipeStepEditors/StepEditor_Control.cs
using System;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Windows.Forms;
using TekstilScada.Models;

namespace TekstilScada.UI.Controls.RecipeStepEditors
{
    public partial class StepEditor_Control : UserControl
    {
        private ScadaRecipeStep _step;
        public event EventHandler StepDataChanged;
        private bool _isUpdating = false; // Programatik değişikliklerde olayların tekrar tetiklenmesini önlemek için
        private TekstilScada.Models.Machine _machine; // YENİ: Hangi makine için çalıştığını bilmeli
        public StepEditor_Control()
        {
            InitializeComponent();
            // CheckBox'ların olaylarını bağla
            chkSuAlma.CheckedChanged += OnStepTypeChanged;
            chkIsitma.CheckedChanged += OnStepTypeChanged;
            chkCalisma.CheckedChanged += OnStepTypeChanged;
            chkDozaj.CheckedChanged += OnStepTypeChanged;
            chkBosaltma.CheckedChanged += OnStepTypeChanged;
            chkSikma.CheckedChanged += OnStepTypeChanged;

        }

        public void LoadStep(ScadaRecipeStep step, TekstilScada.Models.Machine machine)
        {
            _step = step;
            _machine = machine; // Makine bilgisini sakla
            _isUpdating = true; // Yükleme sırasında olayları durdur
            UpdateCheckboxesFromStepData();
            _isUpdating = false; // Yükleme bitti, olayları serbest bırak
            UpdateEditorPanels();
        }

        private void UpdateCheckboxesFromStepData()
        {
            if (_step == null) return;

            // YENİ MANTIK: Adım tiplerini Word 24'ten oku
            short controlWord = _step.StepDataWords[24];
            chkSuAlma.Checked = (controlWord & 1) != 0;      // Bit 0
            chkIsitma.Checked = (controlWord & 2) != 0;      // Bit 1
            chkCalisma.Checked = (controlWord & 4) != 0;     // Bit 2
            chkDozaj.Checked = (controlWord & 8) != 0;       // Bit 3
            chkBosaltma.Checked = (controlWord & 16) != 0;    // Bit 4
            chkSikma.Checked = (controlWord & 32) != 0;      // Bit 5
        }

        private void OnStepTypeChanged(object sender, EventArgs e)
        {
            if (_isUpdating) return; // Eğer programatik bir değişiklikse, hiçbir şey yapma

            var changedCheckbox = sender as CheckBox;
            if (changedCheckbox == null) return;

            // YENİ MANTIK: Maksimum 2 seçim kuralı
            var checkedBoxes = pnlStepTypes.Controls.OfType<CheckBox>().Count(c => c.Checked);
            if (checkedBoxes > 2)
            {
                MessageBox.Show("Bir adımda en fazla 2 farklı işlem türü seçebilirsiniz.", "Kural İhlali", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _isUpdating = true; // Olay döngüsünü kırmak için
                changedCheckbox.Checked = false; // Yapılan son seçimi geri al
                _isUpdating = false;
                return;
            }

            UpdateStepDataFromCheckboxes(changedCheckbox);
            UpdateEditorPanels();
            StepDataChanged?.Invoke(this, EventArgs.Empty);
        }

        private void UpdateStepDataFromCheckboxes(CheckBox chk)
        {
            if (_step == null) return;

            // YENİ MANTIK: Seçimlere göre Word 24'ü güncelle
            short controlWord = 0;
            if (chkSuAlma.Checked) controlWord |= 1;
            if (chkIsitma.Checked) controlWord |= 2;
            if (chkCalisma.Checked) controlWord |= 4;
            if (chkDozaj.Checked) controlWord |= 8;
            if (chkBosaltma.Checked) controlWord |= 16;
            if (chkSikma.Checked) controlWord |= 32;
            _step.StepDataWords[24] = controlWord;

            // Eğer bir adım türü seçimi kaldırılıyorsa, ilgili parametre word'lerini sıfırla
            if (!chk.Checked)
            {
                if (chk == chkSuAlma) { _step.StepDataWords[0] = 0; _step.StepDataWords[1] = 0; }
                if (chk == chkIsitma) { _step.StepDataWords[2] = 0; _step.StepDataWords[3] = 0; _step.StepDataWords[4] = 0; }
                if (chk == chkCalisma) { _step.StepDataWords[14] = 0; _step.StepDataWords[15] = 0; _step.StepDataWords[16] = 0; _step.StepDataWords[17] = 0; _step.StepDataWords[18] = 0; }
                if (chk == chkDozaj) { _step.StepDataWords[5] = 0; _step.StepDataWords[6] = 0; _step.StepDataWords[7] = 0; _step.StepDataWords[11] = 0; _step.StepDataWords[20] = 0; _step.StepDataWords[21] = 0; _step.StepDataWords[22] = 0; _step.StepDataWords[23] = 0; }
                if (chk == chkBosaltma) { _step.StepDataWords[10] = 0; _step.StepDataWords[12] = 0; _step.StepDataWords[13] = 0; _step.StepDataWords[15] = 0; }
                if (chk == chkSikma) { _step.StepDataWords[8] = 0; _step.StepDataWords[9] = 0; }
            }
        }

        private void UpdateEditorPanels()
        {
            pnlParameters.Controls.Clear();

            // Hangi CheckBox işaretliyse, ilgili editörü panele yükle
            if (chkSuAlma.Checked)
            {
                var editor = new SuAlmaEditor_Control();
                editor.LoadStep(_step);
                editor.ValueChanged += (s, ev) => StepDataChanged?.Invoke(this, EventArgs.Empty);
                editor.Dock = DockStyle.Top;
                pnlParameters.Controls.Add(editor);
            }
            if (chkIsitma.Checked)
            {
                var editor = new IsitmaEditor_Control();
                editor.LoadStep(_step);
                editor.ValueChanged += (s, ev) => StepDataChanged?.Invoke(this, EventArgs.Empty);
                editor.Dock = DockStyle.Top;
                pnlParameters.Controls.Add(editor);
            }
            if (chkCalisma.Checked)
            {
                var editor = new CalismaEditor_Control();
                editor.LoadStep(_step);
                editor.ValueChanged += (s, ev) => StepDataChanged?.Invoke(this, EventArgs.Empty);
                editor.Dock = DockStyle.Top;
                pnlParameters.Controls.Add(editor);
            }
            if (chkDozaj.Checked)
            {
                pnlParameters.Controls.Clear();
                if (_machine == null) return; // Makine seçilmemişse hiçbir şey yapma

                // Hangi CheckBox işaretliyse, makine alt tipine göre ilgili editörü yükle
                if (chkDozaj.Checked)
                {
                    // MAKİNE ALT TİPİNE GÖRE EDİTÖR SEÇİMİ
                    if (_machine.MachineSubType?.ToUpper() == "BOYAMA")
                    {
                        var editor = new DozajEditor_Boyama_Control(); // Detaylı editör
                        editor.LoadStep(_step);
                        editor.ValueChanged += (s, ev) => StepDataChanged?.Invoke(this, EventArgs.Empty);
                        editor.Dock = DockStyle.Top;
                        pnlParameters.Controls.Add(editor);
                    }
                    else if (_machine.MachineSubType?.ToUpper() == "YIKAMA")
                    {
                        var editor = new DozajEditor_Yikama_Control(); // Basit editör
                        editor.LoadStep(_step);
                        editor.ValueChanged += (s, ev) => StepDataChanged?.Invoke(this, EventArgs.Empty);
                        editor.Dock = DockStyle.Top;
                        pnlParameters.Controls.Add(editor);
                    }
                    else // Alt tip belirtilmemişse veya eşleşmiyorsa, varsayılanı yükle
                    {
                        var editor = new DozajEditor_Control(); // Orijinal, varsayılan editör
                        editor.LoadStep(_step);
                        editor.ValueChanged += (s, ev) => StepDataChanged?.Invoke(this, EventArgs.Empty);
                        editor.Dock = DockStyle.Top;
                        pnlParameters.Controls.Add(editor);
                    }
                }
            }
            if (chkBosaltma.Checked)
            {
                var editor = new BosaltmaEditor_Control();
                editor.LoadStep(_step);
                editor.ValueChanged += (s, ev) => StepDataChanged?.Invoke(this, EventArgs.Empty);
                editor.Dock = DockStyle.Top;
                pnlParameters.Controls.Add(editor);
            }
            if (chkSikma.Checked)
            {
                var editor = new SikmaEditor_Control();
                editor.LoadStep(_step);
                editor.ValueChanged += (s, ev) => StepDataChanged?.Invoke(this, EventArgs.Empty);
                editor.Dock = DockStyle.Top;
                pnlParameters.Controls.Add(editor);
            }

        }
        public void SetReadOnly(bool isReadOnly)
        {
            // Bu metot, içindeki tüm alt kontrollere ulaşarak
            // düzenleme yapılmasını engeller veya izin verir.
            SetControlsState(this.Controls, !isReadOnly);
        }

        private void SetControlsState(Control.ControlCollection controls, bool enabled)
        {
            foreach (Control control in controls)
            {
                // Sadece kullanıcı girişi alan kontrolleri hedef alıyoruz.
                // Butonlar veya labellar etkilenmez.
                if (control is NumericUpDown || control is TextBox || control is CheckBox || control is ComboBox)
                {
                    control.Enabled = enabled;
                }

                // Panel veya GroupBox gibi taşıyıcıların içindeki kontrollere de ulaşmak için
                // kendini tekrar çağıran (recursive) bir yapı kullanıyoruz.
                if (control.HasChildren)
                {
                    SetControlsState(control.Controls, enabled);
                }
            }
        }
    }

}
