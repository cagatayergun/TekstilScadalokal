// UI/LiveEventPopup_Form.cs
using System;
using System.Drawing;
using System.Windows.Forms;
using TekstilScada.Services;

namespace TekstilScada.UI
{
    public partial class LiveEventPopup_Form : Form
    {
        private const int MAX_EVENTS = 100; // Ekranda gösterilecek maksimum olay sayısı

        public LiveEventPopup_Form()
        {
            InitializeComponent();
            // Olay dinleyicisine abone ol
            LiveEventAggregator.Instance.OnEventPublished += OnNewEventPublished;
        }

        private void OnNewEventPublished(LiveEvent liveEvent)
        {
            // Bu olay arka plan thread'inden gelebilir, bu yüzden Invoke kullanmak zorunludur.
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => AddEventToList(liveEvent)));
                return;
            }
            AddEventToList(liveEvent);
        }

        private void AddEventToList(LiveEvent liveEvent)
        {
            var item = new ListViewItem(liveEvent.Timestamp.ToString("HH:mm:ss"));
            item.SubItems.Add(liveEvent.Source);
            item.SubItems.Add(liveEvent.Message);

            // Olay türüne göre renklendir
            switch (liveEvent.Type)
            {
                case EventType.Alarm:
                    item.ForeColor = Color.White;
                    item.BackColor = Color.FromArgb(192, 57, 43); // Kırmızı
                    break;
                case EventType.Process:
                    item.ForeColor = Color.Black;
                    item.BackColor = Color.FromArgb(178, 235, 242); // Açık Mavi
                    break;
                case EventType.SystemSuccess:
                    item.ForeColor = Color.White;
                    item.BackColor = Color.FromArgb(39, 174, 96); // Yeşil
                    break;
                case EventType.SystemWarning:
                    item.ForeColor = Color.White;
                    item.BackColor = Color.FromArgb(243, 156, 18); // Turuncu
                    break;
                default:
                    item.ForeColor = Color.Black;
                    break;
            }

            // Yeni olayı listenin en üstüne ekle
            lstEvents.Items.Insert(0, item);

            // Eğer liste çok uzarsa, en eski olayı sil
            if (lstEvents.Items.Count > MAX_EVENTS)
            {
                lstEvents.Items.RemoveAt(MAX_EVENTS);
            }
        }

        // GÜNCELLENDİ: Formu kapatmak yerine gizle
        private void LiveEventPopup_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        // Form kapatıldığında olay dinleyicisinden aboneliği kaldır (bellek sızıntısını önler)
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            LiveEventAggregator.Instance.OnEventPublished -= OnNewEventPublished;
            base.OnFormClosed(e);
        }
    }
}