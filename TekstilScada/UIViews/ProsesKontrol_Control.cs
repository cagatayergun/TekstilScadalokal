// UI/Views/ProsesKontrol_Control.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TekstilScada.Core;
using TekstilScada.Models;
using TekstilScada.Repositories;
using TekstilScada.Services;
using TekstilScada.UI.Controls;
using TekstilScada.UI.Controls.RecipeStepEditors;

namespace TekstilScada.UI.Views
{
    public partial class ProsesKontrol_Control : UserControl
    {
        private RecipeRepository _recipeRepository;
        private MachineRepository _machineRepository;
        private Dictionary<int, IPlcManager> _plcManagers;

        private List<ScadaRecipe> _recipeList;
        private ScadaRecipe _currentRecipe;

        // BYMakinesi editörünü ve bileşenlerini tutmak için değişkenler
        private SplitContainer _byMakinesiEditor;
        private DataGridView dgvRecipeSteps;
        private Panel pnlStepDetails;
        private Label lblStepDetailsTitle;
        private CostRepository _costRepository;
        private FtpSync_Form _ftpFormInstance; // YENİ EKLENEN SATIR
        public ProsesKontrol_Control()
        {
            InitializeComponent();
            _costRepository = new CostRepository(); // YENİ
            this.Load += ProsesKontrol_Control_Load;
            btnNewRecipe.Click += BtnNewRecipe_Click;
            btnDeleteRecipe.Click += BtnDeleteRecipe_Click;
            btnSaveRecipe.Click += BtnSaveRecipe_Click;
            btnSendToPlc.Click += BtnSendToPlc_Click;
            btnReadFromPlc.Click += BtnReadFromPlc_Click;
            lstRecipes.SelectedIndexChanged += LstRecipes_SelectedIndexChanged;
            cmbTargetMachine.SelectedIndexChanged += CmbTargetMachine_SelectedIndexChanged;
            btnFtpSync.Click += BtnFtpSync_Click;
            this.Load += ProsesKontrol_Control_Load;
        }

        public void InitializeControl(RecipeRepository recipeRepo, MachineRepository machineRepo, Dictionary<int, IPlcManager> plcManagers)
        {
            _recipeRepository = recipeRepo;
            _machineRepository = machineRepo;
            _plcManagers = plcManagers;

        }

        private void ProsesKontrol_Control_Load(object sender, EventArgs e)
        {
            LoadRecipeList();
            LoadMachineList();
            ApplyRolePermissions(); // YENİ: Yetki kontrolünü çağır
            ApplyPermissions(); // YENİ: Bu ekran için yetkileri uygula
            FtpTransferService.Instance.RecipeListChanged += OnRecipeListChanged;
        }

        private void ApplyPermissions()
        {
            // Reçete kaydetme yetkisi kontrolü
            btnSaveRecipe.Enabled = PermissionService.CanEditRecipes;

            // Reçete silme yetkisi kontrolü (sadece Admin silebilir)
            btnDeleteRecipe.Enabled = PermissionService.CanDeleteRecipes;

            // PLC'ye gönderme yetkisi kontrolü
            btnSendToPlc.Enabled = PermissionService.CanTransferToPlc;
            btnReadFromPlc.Enabled = PermissionService.CanTransferToPlc;
            btnFtpSync.Enabled = PermissionService.CanTransferToPlc;

            // Reçete Adı metin kutusunu sadece yetkisi olanlar düzenleyebilir
            txtRecipeName.ReadOnly = !PermissionService.CanEditRecipes;
        }
        private void ApplyRolePermissions()
        {
            // Sadece Admin ve Muhendis (Mühendis) rolleri kaydedebilir.
            btnSaveRecipe.Enabled = CurrentUser.HasRole("Admin") || CurrentUser.HasRole("Muhendis");


        }
        // YENİ EKLENEN METOT: Sinyal geldiğinde bu metot çalışacak
        private void OnRecipeListChanged(object sender, EventArgs e)
        {
            // Farklı bir thread'den gelebileceği için Invoke kullanarak UI'ı güvenli şekilde güncelle
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => LoadRecipeList()));
            }
            else
            {
                LoadRecipeList();
            }
        }
         
             
        private void LoadMachineList()
        {
            var machines = _machineRepository.GetAllEnabledMachines();
            cmbTargetMachine.DataSource = machines;
            cmbTargetMachine.DisplayMember = "DisplayInfo";
            cmbTargetMachine.ValueMember = "Id";
        }

        private void LoadRecipeList()
        {
            try
            {
                // 1. Daha önce hangi öğenin seçili olduğunu hatırla
                int selectedId = (lstRecipes.SelectedItem as ScadaRecipe)?.Id ?? -1;

                // 2. Listeyi veritabanından yeniden yükle
                _recipeList = _recipeRepository.GetAllRecipes();
                lstRecipes.DataSource = null;
                lstRecipes.DataSource = _recipeList;
                lstRecipes.DisplayMember = "RecipeName";
                lstRecipes.ValueMember = "Id";

                // 3. Eski seçili öğeyi tekrar seçmeye çalış
                if (selectedId != -1)
                {
                    // --- YENİ GÜVENLİK KONTROLÜ ---
                    // 'selectedId'nin yeni yüklenen '_recipeList' içinde hala var olup olmadığını kontrol et.
                    var itemExists = _recipeList.Any(r => r.Id == selectedId);
                    if (itemExists)
                    {
                        // Eğer hala varsa, güvenli bir şekilde seç.
                        lstRecipes.SelectedValue = selectedId;
                    }
                    // Eğer yoksa, hiçbir şey yapma ve hatayı önle.
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Reçeteler yüklenirken hata oluştu: {ex.Message}", "Veritabanı Hatası");
            }
        }

        private void BtnNewRecipe_Click(object sender, EventArgs e)
        {
            _currentRecipe = new ScadaRecipe { RecipeName = "YENİ REÇETE" };

            var selectedMachine = cmbTargetMachine.SelectedItem as Machine;
            int stepCount = (selectedMachine != null && selectedMachine.MachineType == "Kurutma Makinesi") ? 1 : 98;

            _currentRecipe.Steps.Clear();
            for (int i = 1; i <= stepCount; i++)
            {
                _currentRecipe.Steps.Add(new ScadaRecipeStep { StepNumber = i });
            }
            DisplayCurrentRecipe();
        }

        private void LstRecipes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstRecipes.SelectedItem is ScadaRecipe selected)
            {
                try
                {
                    _currentRecipe = _recipeRepository.GetRecipeById(selected.Id);
                    DisplayCurrentRecipe();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Reçete detayları yüklenirken hata oluştu: {ex.Message}", "Veritabanı Hatası");
                }
            }
        }

        private void CmbTargetMachine_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayCurrentRecipe();
        }

        private void DisplayCurrentRecipe()
        {
            if (_currentRecipe != null)
            {
                txtRecipeName.Text = _currentRecipe.RecipeName;
                LoadEditorForSelectedMachine();
            }
            else
            {
                txtRecipeName.Text = "";
                pnlEditorArea.Controls.Clear();
            }
        }

        private void LoadEditorForSelectedMachine()
        {
            pnlEditorArea.Controls.Clear();
            var selectedMachine = cmbTargetMachine.SelectedItem as Machine;

            if (selectedMachine == null) return;

            if (selectedMachine.MachineType == "Kurutma Makinesi")
            {
                var editor = new KurutmaReçete_Control();
                editor.LoadRecipe(_currentRecipe);
                editor.ValueChanged += (s, ev) => { /* Değişiklikleri kaydetmek için event'i dinle */ };
                editor.Dock = DockStyle.Fill;
                pnlEditorArea.Controls.Add(editor);
            }
            else // Varsayılan olarak BYMakinesi
            {
                InitializeBYMakinesiEditor();
                PopulateStepsGridView();
                pnlEditorArea.Controls.Add(_byMakinesiEditor);
            }
        }

        private void InitializeBYMakinesiEditor()
        {
            _byMakinesiEditor = new SplitContainer();
            dgvRecipeSteps = new DataGridView();
            pnlStepDetails = new Panel();
            lblStepDetailsTitle = new Label();

            _byMakinesiEditor.Dock = DockStyle.Fill;
            _byMakinesiEditor.SplitterDistance = 40;

            _byMakinesiEditor.Panel1.Controls.Add(dgvRecipeSteps);
            _byMakinesiEditor.Panel2.Controls.Add(pnlStepDetails);

            dgvRecipeSteps.Dock = DockStyle.Fill;
            dgvRecipeSteps.AllowUserToAddRows = false;
            dgvRecipeSteps.AllowUserToDeleteRows = false;
            dgvRecipeSteps.MultiSelect = false;
            dgvRecipeSteps.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRecipeSteps.CellClick += DgvRecipeSteps_CellClick;

            pnlStepDetails.Dock = DockStyle.Fill;
            pnlStepDetails.BorderStyle = BorderStyle.FixedSingle;
            pnlStepDetails.Controls.Add(lblStepDetailsTitle);

            lblStepDetailsTitle.Dock = DockStyle.Top;
            lblStepDetailsTitle.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            lblStepDetailsTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lblStepDetailsTitle.Text = "Adım Detayları";

            SetupStepsGridView();
        }

        private void SetupStepsGridView()
        {
            if (dgvRecipeSteps == null) return;
            dgvRecipeSteps.DataSource = null;
            dgvRecipeSteps.Rows.Clear();
            dgvRecipeSteps.Columns.Clear();
            dgvRecipeSteps.AutoGenerateColumns = false;

            dgvRecipeSteps.Columns.Add(new DataGridViewTextBoxColumn { Name = "StepNumber", HeaderText = "Adım No", DataPropertyName = "StepNumber", Width = 40 });
            dgvRecipeSteps.Columns.Add(new DataGridViewTextBoxColumn { Name = "StepType", HeaderText = "Adım Tipi", Width = 300 });
        }

        private void PopulateStepsGridView()
        {
            if (_currentRecipe == null || _currentRecipe.Steps == null || dgvRecipeSteps == null) return;
            dgvRecipeSteps.Rows.Clear();
            foreach (var step in _currentRecipe.Steps)
            {
                string stepTypeName = GetStepTypeName(step);
                dgvRecipeSteps.Rows.Add(step.StepNumber, stepTypeName);
            }
        }

        private string GetStepTypeName(ScadaRecipeStep step)
        {
            var stepTypes = new List<string>();
            short controlWord = step.StepDataWords[24];
            if ((controlWord & 1) != 0) stepTypes.Add("Su Alma");
            if ((controlWord & 2) != 0) stepTypes.Add("Isıtma");
            if ((controlWord & 4) != 0) stepTypes.Add("Çalışma");
            if ((controlWord & 8) != 0) stepTypes.Add("Dozaj");
            if ((controlWord & 16) != 0) stepTypes.Add("Boşaltma");
            if ((controlWord & 32) != 0) stepTypes.Add("Sıkma");
            return string.Join(" + ", stepTypes);
        }

        private void DgvRecipeSteps_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || _currentRecipe == null || pnlStepDetails == null) return;

            pnlStepDetails.Controls.Clear();
            pnlStepDetails.Controls.Add(lblStepDetailsTitle);

            int selectedIndex = e.RowIndex;
            if (selectedIndex < _currentRecipe.Steps.Count)
            {
                var selectedStep = _currentRecipe.Steps[selectedIndex];
                var selectedMachine = cmbTargetMachine.SelectedItem as Machine; // YENİ
                lblStepDetailsTitle.Text = $"Adım Detayları - Adım No: {selectedStep.StepNumber}";

                var mainEditor = new StepEditor_Control();
                mainEditor.LoadStep(selectedStep, selectedMachine);
                mainEditor.StepDataChanged += (s, ev) => {
                    if (dgvRecipeSteps.Rows.Count > selectedIndex)
                    {
                        dgvRecipeSteps.Rows[selectedIndex].Cells["StepType"].Value = GetStepTypeName(selectedStep);
                    }
                };
                mainEditor.Dock = DockStyle.Fill;
                pnlStepDetails.Controls.Add(mainEditor);
                mainEditor.BringToFront();
            }
        }

        private void BtnFtpSync_Click(object sender, EventArgs e)
        {
            // 1. Eğer form daha önce açılmış ve hala açıksa, yeni bir tane açma, eskisini öne getir.
            if (_ftpFormInstance != null && !_ftpFormInstance.IsDisposed)
            {
                _ftpFormInstance.BringToFront();
                return;
            }

            // 2. Formu oluştur.
            _ftpFormInstance = new FtpSync_Form(_machineRepository, _recipeRepository);

            // 3. Form kapandığında, referansı temizle ki tekrar açılabilsin.
            _ftpFormInstance.FormClosed += (s, args) => _ftpFormInstance = null;

            // 4. Formu ShowDialog() yerine Show() ile açarak arka planda çalışmaya izin ver.
            _ftpFormInstance.Show(this);

            // DİKKAT: LoadRecipeList() satırını buradan siliyoruz.
            // Çünkü Show() komutu beklemeyeceği için, liste yenileme işlemini
            // arka plan servisinden bir sinyal geldiğinde yapacağız. (Bir sonraki adımda anlatılıyor)
        }

        private async void BtnSendToPlc_Click(object sender, EventArgs e)
        {
            if (_currentRecipe == null) { MessageBox.Show("Lütfen PLC'ye göndermek için bir reçete seçin veya oluşturun.", "Uyarı"); return; }
            var selectedMachine = cmbTargetMachine.SelectedItem as Machine;
            if (selectedMachine == null) { MessageBox.Show("Lütfen bir hedef makine seçin.", "Uyarı"); return; }

            if (_plcManagers == null || !_plcManagers.TryGetValue(selectedMachine.Id, out var plcManager))
            {
                MessageBox.Show($"'{selectedMachine.MachineName}' için aktif bir PLC bağlantısı bulunamadı.", "Bağlantı Hatası");
                return;
            }

            int? recipeSlot = null;

            if (selectedMachine.MachineType == "Kurutma Makinesi")
            {
                string input = ShowInputDialog("Lütfen PLC'ye kaydedilecek reçete numarasını girin (1-20):");
                if (int.TryParse(input, out int slot) && slot >= 1 && slot <= 20)
                {
                    recipeSlot = slot;
                }
                else
                {
                    if (!string.IsNullOrEmpty(input))
                    {
                        MessageBox.Show("Geçersiz reçete numarası girdiniz.", "Hata");
                    }
                    return;
                }
            }

            btnSendToPlc.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                var result = await plcManager.WriteRecipeToPlcAsync(_currentRecipe, recipeSlot);

                if (result.IsSuccess) { MessageBox.Show($"'{_currentRecipe.RecipeName}' reçetesi, '{selectedMachine.MachineName}' makinesine başarıyla gönderildi.", "Başarılı"); }
                else { MessageBox.Show($"Reçete gönderilirken hata oluştu: {result.Message}", "Hata"); }
            }
            catch (NotImplementedException)
            {
                MessageBox.Show($"'{selectedMachine.MachineType}' tipi için reçete gönderme özelliği henüz tamamlanmadı.", "Geliştirme Aşamasında");
            }
            catch (Exception ex) { MessageBox.Show($"Beklenmedik bir hata oluştu: {ex.Message}", "Sistem Hatası"); }
            finally
            {
                this.Cursor = Cursors.Default;
                btnSendToPlc.Enabled = true;
            }
        }

        public static string ShowInputDialog(string text)
        {
            Form prompt = new Form()
            {
                Width = 400,
                Height = 180,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Giriş Gerekli",
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = "Reçete No (1-20):", Width = 300 };
            NumericUpDown inputBox = new NumericUpDown() { Left = 50, Top = 50, Width = 300, Minimum = 1, Maximum = 20 };
            Button confirmation = new Button() { Text = "Tamam", Left = 250, Width = 100, Top = 90, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(inputBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? inputBox.Value.ToString() : "";
        }

        private async void BtnReadFromPlc_Click(object sender, EventArgs e)
        {
            var selectedMachine = cmbTargetMachine.SelectedItem as Machine;
            if (selectedMachine == null) { MessageBox.Show("Lütfen bir hedef makine seçin.", "Uyarı"); return; }

            if (_plcManagers == null || !_plcManagers.TryGetValue(selectedMachine.Id, out var plcManager))
            {
                MessageBox.Show($"'{selectedMachine.MachineName}' için aktif bir PLC bağlantısı bulunamadı.", "Bağlantı Hatası");
                return;
            }

            btnReadFromPlc.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                var result = await plcManager.ReadRecipeFromPlcAsync();
                if (result.IsSuccess)
                {
                    var recipeFromPlc = new ScadaRecipe { RecipeName = $"PLC_{selectedMachine.MachineUserDefinedId}_{DateTime.Now:HHmm}" };

                    if (selectedMachine.MachineType == "Kurutma Makinesi")
                    {
                        var step = new ScadaRecipeStep { StepNumber = 1 };
                        // Kurutma makinesi 5 word + 1 kontrol word'ü okur
                        Array.Copy(result.Content, 0, step.StepDataWords, 0, Math.Min(result.Content.Length, 6));
                        recipeFromPlc.Steps.Add(step);
                    }
                    else
                    {
                        for (int i = 0; i < 98; i++)
                        {
                            var step = new ScadaRecipeStep { StepNumber = i + 1 };
                            Array.Copy(result.Content, i * 25, step.StepDataWords, 0, 25);
                            recipeFromPlc.Steps.Add(step);
                        }
                    }

                    _currentRecipe = recipeFromPlc;
                    DisplayCurrentRecipe();
                    MessageBox.Show($"'{selectedMachine.MachineName}' makinesindeki reçete başarıyla okundu.\nLütfen yeni bir isim verip kaydedin.", "Başarılı");
                }
                else { MessageBox.Show($"Reçete okunurken hata oluştu: {result.Message}", "Hata"); }
            }
            catch (NotImplementedException)
            {
                MessageBox.Show($"'{selectedMachine.MachineType}' tipi için reçete okuma özelliği henüz tamamlanmadı.", "Geliştirme Aşamasında");
            }
            catch (Exception ex) { MessageBox.Show($"Beklenmedik bir hata oluştu: {ex.Message}", "Sistem Hatası"); }
            finally
            {
                this.Cursor = Cursors.Default;
                btnReadFromPlc.Enabled = true;
            }
        }

        private void BtnSaveRecipe_Click(object sender, EventArgs e)
        {
            if (_currentRecipe == null) { MessageBox.Show("Kaydedilecek bir reçete yok.", "Uyarı"); return; }
            if (string.IsNullOrWhiteSpace(txtRecipeName.Text)) { MessageBox.Show("Reçete adı boş olamaz.", "Uyarı"); return; }
            _currentRecipe.RecipeName = txtRecipeName.Text;
            try
            {
                _recipeRepository.SaveRecipe(_currentRecipe);
                MessageBox.Show("Reçete başarıyla kaydedildi.", "Başarılı");
                LoadRecipeList();
            }
            catch (Exception ex) { MessageBox.Show($"Reçete kaydedilirken bir hata oluştu: {ex.Message}", "Hata"); }
        }

        private void BtnDeleteRecipe_Click(object sender, EventArgs e)
        {
            // 1. Listeden seçili olan tüm reçeteleri al.
            var selectedRecipes = lstRecipes.SelectedItems.Cast<ScadaRecipe>().ToList();

            // 2. Hiçbir reçete seçilmediyse uyarı ver ve metottan çık.
            if (!selectedRecipes.Any())
            {
                MessageBox.Show("Lütfen silmek için listeden en az bir reçete seçin.", "Uyarı");
                return;
            }

            // 3. Kullanıcıdan toplu silme için onay al.
            var result = MessageBox.Show(
                $"{selectedRecipes.Count} adet reçeteyi kalıcı olarak silmek istediğinizden emin misiniz?\nBu işlem geri alınamaz.",
                "Toplu Silme Onayı",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            // 4. Kullanıcı "Evet" derse silme işlemine başla.
            if (result == DialogResult.Yes)
            {
                try
                {
                    // Seçilen her bir reçete için döngü kur ve sil.
                    foreach (var recipeToDelete in selectedRecipes)
                    {
                        _recipeRepository.DeleteRecipe(recipeToDelete.Id);
                    }

                    MessageBox.Show($"{selectedRecipes.Count} adet reçete başarıyla silindi.", "İşlem Tamamlandı");

                    // 5. Mevcut reçete ekranını temizle ve listeyi yenile.
                    _currentRecipe = null;
                    DisplayCurrentRecipe();
                    LoadRecipeList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Reçeteler silinirken bir hata oluştu: {ex.Message}", "Hata");
                }
            }
        }
        private void btnCalculateCost_Click(object sender, EventArgs e)
        {
            if (_currentRecipe == null)
            {
                MessageBox.Show("Lütfen maliyetini hesaplamak için bir reçete seçin veya oluşturun.", "Uyarı");
                return;
            }

            _currentRecipe.RecipeName = txtRecipeName.Text;

            try
            {
                var costParams = _costRepository.GetAllParameters();
                // GÜNCELLENDİ: Yeni metottan 3 değer alınıyor
                var (totalCost, currencySymbol, breakdown) = RecipeCostCalculator.Calculate(_currentRecipe, costParams);

                // GÜNCELLENDİ: Sonuç para birimi sembolü ile birlikte gösteriliyor
                lblTotalCost.Text = $"{totalCost:F2} {currencySymbol}";

                // Detaylı döküm tooltip'e yazdırılıyor
                ToolTip toolTip = new ToolTip();
                toolTip.SetToolTip(pnlCost, breakdown);
                toolTip.SetToolTip(lblTotalCost, breakdown);
                toolTip.SetToolTip(lblCostTitle, breakdown);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Maliyet hesaplanırken bir hata oluştu: {ex.Message}", "Hata");
            }
        }
    }
}
