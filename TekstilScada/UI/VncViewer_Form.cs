using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using VncSharpCore; // RemoteDesktop sınıfı için

namespace TekstilScada.UI
{
    public partial class VncViewer_Form : Form
    {
        private readonly string _address;
        private readonly string _password;
        private int _port = 5900; // Varsayılan port
        private bool _isClosingInitiated = false; // Form kapatma işleminin başlatılıp başlatılmadığını izle

        public VncViewer_Form(string address, string password)
        {
            InitializeComponent();

            // Adres ve Port ayrıştırma işlemi
            if (address.Contains(":"))
            {
                var parts = address.Split(':');
                _address = parts[0];
                // Port ayrıştırma başarılı olmazsa _port varsayılan değeri (5900) korur
                if (parts.Length > 1 && !int.TryParse(parts[1], out _port))
                {
                    // Hata ayıklama çıktısı ve kullanıcıya uyarı
                    System.Diagnostics.Debug.WriteLine($"Uyarı: Geçersiz port numarası algılandı: '{parts[1]}'. Varsayılan port (5900) kullanılacak.");
                    MessageBox.Show("Geçersiz port numarası. Varsayılan port (5900) kullanılacak.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _port = 5900; // Hata durumunda _port'u açıkça varsayılana ayarla
                }
            }
            else
            {
                _address = address;
            }

            _password = password;

            // Olayları (events) bağlayalım
            // Bu abonelikler, remoteDesktop1 kontrolü Form ile birlikte dispose edildiğinde otomatik olarak kaldırılacaktır.
            // Ancak, açıkça belirtmek iyi bir pratiktir, özellikle karmaşık senaryolarda sızıntıları önler.
            remoteDesktop1.ConnectComplete += VncControl_ConnectComplete;
            remoteDesktop1.ConnectionLost += VncControl_ConnectionLost;
            remoteDesktop1.GetPassword = () => _password; // Şifre delegate'ini burada ayarla
        }

        private async void VncViewer_Form_Load(object sender, EventArgs e)
        {
            this.Text = $"{_address}:{_port} - Bağlanılıyor...";
            try
            {
                // Bağlantıyı arka planda başlatıyoruz.
                // remoteDesktop1'in Connect metodu çağrıldığında, onun içindeki tüm UI etkileşimleri
                // (SetState, SetupDesktop, Invalidate vb.) RemoteDesktop.cs'de Invoke ile korunmalıdır.
                await Task.Run(() => remoteDesktop1.Connect(_address));
            }
            catch (Exception ex)
            {
                // Bağlantı başlatılırken bir hata oluşursa:
                System.Diagnostics.Debug.WriteLine($"VNC bağlantı başlatma hatası: {ex.Message}");
                MessageBox.Show($"VNC bağlantısı başlatılırken hata oluştu: {ex.Message}", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Hata durumunda formu kapatma işlemine başla
                _isClosingInitiated = true; // Kapanma işleminin bu hata nedeniyle başladığını işaretle
                this.Close();
            }
        }

        private void VncControl_ConnectComplete(object sender, ConnectEventArgs e)
        {
            // UI güncellemeleri için Invoke kullanmak her zaman en güvenlisidir.
            // Bu metot, RemoteDesktop içinden BeginInvoke ile çağrıldığından
            // bu kontrol genellikle true olacaktır.
            if (InvokeRequired)
            {
                Invoke(new Action(() => VncControl_ConnectComplete(sender, e)));
                return;
            }
            this.Text = $"{_address}:{_port} - Bağlandı: {e.DesktopName}";
        }

        private void VncControl_ConnectionLost(object sender, EventArgs e)
        {
            // UI güncellemeleri için Invoke kullanmak her zaman en güvenlisidir.
            // Bu metot, RemoteDesktop içinden BeginInvoke ile çağrıldığından
            // bu kontrol genellikle true olacaktır.
            if (InvokeRequired)
            {
                Invoke(new Action(() => VncControl_ConnectionLost(sender, e)));
                return;
            }

            System.Diagnostics.Debug.WriteLine("VNC bağlantısı kesildi veya kaybedildi.");
            MessageBox.Show("VNC bağlantısı kesildi veya kaybedildi.", "Bağlantı Kesildi", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            // Eğer form zaten kapanmıyorsa veya kapatma işlemi zaten bu olaydan başlatılmadıysa
            // formu kapatma işlemine başla. Bu, Disconnect döngülerini önler.
            if (!_isClosingInitiated)
            {
                _isClosingInitiated = true;
                this.Close();
            }
        }

        private void VncViewer_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            // _isClosingInitiated bayrağı, VncControl_ConnectionLost'tan veya
            // VncViewer_Form_Load'daki bir hatadan `this.Close()` çağrıldığında `true` olur.
            // Eğer kapanma nedeni `None` değilse (yani bir kullanıcı eylemi veya sistem kapatmasıysa)
            // ve zaten kapatma süreci başlatılmışsa, tekrar `Disconnect()` çağırmadan çık.
            if (_isClosingInitiated && e.CloseReason != CloseReason.None)
            {
                // Debug.WriteLine($"FormClosing: Kapatma zaten başlatılmış ({e.CloseReason}). Disconnect tekrar çağrılmıyor.");
                return;
            }

            // Eğer buraya gelinirse, ya kapanma işlemi ilk kez başlatılıyor ya da
            // _isClosingInitiated olmasına rağmen neden None.
            // Kapatma işleminin başlatıldığını işaretle.
            _isClosingInitiated = true;

            // Form kapanırken bağlantıyı kes
            if (remoteDesktop1.IsConnected)
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("VncViewer_Form_FormClosing: Bağlantı kesiliyor...");
                    remoteDesktop1.Disconnect();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"VncViewer_Form_FormClosing: Bağlantı kesilirken hata: {ex.Message}");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("VncViewer_Form_FormClosing: Bağlantı zaten kesik.");
            }

            // Olay aboneliklerini iptal et. Bu, dispose döngüsünde RemoteDesktop kontrolü ile birlikte
            // otomatik olarak yapılacaktır, ancak açıkça belirtmek iyi bir pratiktir.
            remoteDesktop1.ConnectComplete -= VncControl_ConnectComplete;
            remoteDesktop1.ConnectionLost -= VncControl_ConnectionLost;
            System.Diagnostics.Debug.WriteLine("VncViewer_Form_FormClosing: Event abonelikleri kaldırıldı.");
        }

       
    }
}