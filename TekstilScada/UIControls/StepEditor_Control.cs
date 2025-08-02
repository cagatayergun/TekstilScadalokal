// UI/Controls/RecipeStepEditors/StepEditor_Control.cs
using System;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;
using TekstilScada.Models;
using TekstilScada.Repositories;

namespace TekstilScada.UI.Controls.RecipeStepEditors
{
    public partial class StepEditor_Control : UserControl
    {
        private ScadaRecipeStep _step;
        public event EventHandler StepDataChanged;
        private bool _isUpdating = false; // Programatik değişikliklerde olayların tekrar tetiklenmesini önlemek için
        private TekstilScada.Models.Machine _machine; // YENİ: Hangi makine için çalıştığını bilmeli
        private readonly RecipeConfigurationRepository _configRepo = new RecipeConfigurationRepository();
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
            if (_step == null || _machine == null) return;

            int? stepTypeId = GetStepTypeIdFromControlWord(_step.StepDataWords[24]);
            if (!stepTypeId.HasValue) return;

            string layoutJson = _configRepo.GetLayoutJson(_machine.MachineSubType, stepTypeId.Value);
            if (string.IsNullOrEmpty(layoutJson))
            {
                layoutJson = _configRepo.GetLayoutJson("DEFAULT", stepTypeId.Value);
            }
            if (string.IsNullOrEmpty(layoutJson)) return;

            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var controlsData = JsonSerializer.Deserialize<List<ControlMetadata>>(layoutJson, options);

                foreach (var data in controlsData)
                {
                    Type controlType = Type.GetType(data.ControlType);
                    if (controlType == null) continue;

                    Control control = (Control)Activator.CreateInstance(controlType);

                    control.Name = data.Name;
                    control.Text = data.Text;
                    control.Location = new Point(int.Parse(data.Location.Split(',')[0].Trim()), int.Parse(data.Location.Split(',')[1].Trim()));
                    control.Size = new Size(int.Parse(data.Size.Split(',')[0].Trim()), int.Parse(data.Size.Split(',')[1].Trim()));

                    if (control is NumericUpDown num) num.Maximum = data.Maximum;

                    control.Tag = new PlcMapping { WordIndex = data.PLC_WordIndex, BitIndex = data.PLC_BitIndex };

                    if (control is NumericUpDown numeric) numeric.ValueChanged += OnDynamicControlValueChanged;
                    if (control is CheckBox chk) chk.CheckedChanged += OnDynamicControlValueChanged;

                    pnlParameters.Controls.Add(control);
                }
                LoadDataToDynamicControls();
            }
            catch (Exception ex)
            {
                pnlParameters.Controls.Add(new Label { Text = "Arayüz yüklenirken hata.", Dock = DockStyle.Fill });
                Console.WriteLine($"JSON Parse/UI Oluşturma Hatası: {ex.Message}");
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


        private void OnDynamicControlValueChanged(object sender, EventArgs e)
        {
            if (_isUpdating) return;
            Control control = sender as Control;
            if (control?.Tag is not PlcMapping mapping) return;

            if (mapping.WordIndex >= _step.StepDataWords.Length) return;

            if (control is NumericUpDown num)
            {
                _step.StepDataWords[mapping.WordIndex] = (short)num.Value;
            }
            else if (control is CheckBox chk)
            {
                SetBit(_step.StepDataWords, mapping.WordIndex, mapping.BitIndex, chk.Checked);
            }
            // YENİ: TextBox değeri değiştiğinde çalışacak mantık
            else if (control is TextBox txt && mapping.StringWordLength > 0)
            {
                // String'i istenen uzunlukta byte dizisine çevir
                int byteLength = mapping.StringWordLength * 2;
                byte[] stringBytes = new byte[byteLength];
                Encoding.ASCII.GetBytes(txt.Text, 0, Math.Min(txt.Text.Length, byteLength), stringBytes, 0);

                // Byte dizisini StepDataWords'e yaz
                for (int i = 0; i < mapping.StringWordLength; i++)
                {
                    int targetIndex = mapping.WordIndex + i;
                    if (targetIndex < _step.StepDataWords.Length)
                    {
                        _step.StepDataWords[targetIndex] = BitConverter.ToInt16(stringBytes, i * 2);
                    }
                }
            }
            StepDataChanged?.Invoke(this, EventArgs.Empty);
        }
        private void LoadDataToDynamicControls()
        {
            _isUpdating = true;
            foreach (Control control in pnlParameters.Controls)
            {
                if (control?.Tag is not PlcMapping mapping) continue;
                if (mapping.WordIndex >= _step.StepDataWords.Length) continue;

                if (control is NumericUpDown num)
                {
                    num.Value = _step.StepDataWords[mapping.WordIndex];
                }
                else if (control is CheckBox chk)
                {
                    chk.Checked = GetBit(_step.StepDataWords, mapping.WordIndex, mapping.BitIndex);
                }
                // YENİ: TextBox'ı dolduracak mantık
                else if (control is TextBox txt && mapping.StringWordLength > 0)
                {
                    List<byte> allBytes = new List<byte>();
                    for (int i = 0; i < mapping.StringWordLength; i++)
                    {
                        int sourceIndex = mapping.WordIndex + i;
                        if (sourceIndex < _step.StepDataWords.Length)
                        {
                            allBytes.AddRange(BitConverter.GetBytes(_step.StepDataWords[sourceIndex]));
                        }
                    }
                    txt.Text = Encoding.ASCII.GetString(allBytes.ToArray()).Trim('\0');
                    txt.MaxLength = mapping.StringWordLength * 2;
                }
            }
            _isUpdating = false;
        }

        private int? GetStepTypeIdFromControlWord(short controlWord)
        {
            for (int i = 0; i < 6; i++)
            {
                if ((controlWord & (1 << i)) != 0) return i + 1;
            }
            return null;
        }

        private bool GetBit(short[] data, int wordIndex, int bitIndex) => (data[wordIndex] & (1 << bitIndex)) != 0;

        private void SetBit(short[] data, int wordIndex, int bitIndex, bool value)
        {
            if (value) data[wordIndex] = (short)(data[wordIndex] | (1 << bitIndex));
            else data[wordIndex] = (short)(data[wordIndex] & ~(1 << bitIndex));
        }


    }

}
