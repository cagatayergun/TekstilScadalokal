// UI/Controls/AlarmBanner_Control.cs
using System;
using System.Drawing;
using System.Windows.Forms;
using TekstilScada.Models;

namespace TekstilScada.UI.Controls
{
    public partial class AlarmBanner_Control : UserControl
    {
        public AlarmBanner_Control()
        {
            InitializeComponent();
            this.Visible = false; // Başlangıçta gizli

            // DÜZELTME: Label'a yapılan tıklamaların, banner'ın kendi Click olayını tetiklemesini sağla.
            lblAlarmText.Click += (sender, e) => this.OnClick(e);
        }

        public void ShowAlarm(string machineName, AlarmDefinition alarmDef)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => ShowAlarm(machineName, alarmDef)));
                return;
            }

            if (alarmDef == null)
            {
                HideBanner();
                return;
            }

            lblAlarmText.Text = $"[{machineName}] - ALARM #{alarmDef.AlarmNumber}: {alarmDef.AlarmText}";

            // Önem derecesine göre renk ayarla
            switch (alarmDef.Severity)
            {
                case 4: // Kritik
                    this.BackColor = Color.FromArgb(192, 57, 43); // Koyu Kırmızı
                    break;
                case 3: // Yüksek
                    this.BackColor = Color.FromArgb(231, 76, 60); // Kırmızı
                    break;
                case 2: // Orta
                    this.BackColor = Color.FromArgb(230, 126, 34); // Turuncu
                    break;
                default: // Düşük (veya tanımsız)
                    this.BackColor = Color.FromArgb(241, 196, 15); // Sarı
                    break;
            }

            this.Visible = true;
        }

        public void HideBanner()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => HideBanner()));
                return;
            }
            this.Visible = false;
        }
    }
}