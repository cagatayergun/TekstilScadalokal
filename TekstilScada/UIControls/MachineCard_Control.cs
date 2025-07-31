// UI/Controls/MachineCard_Control.cs
using System;
using System.Drawing;
using System.Windows.Forms;
using TekstilScada.Models;

namespace TekstilScada.UI.Controls
{
    public partial class MachineCard_Control : UserControl
    {
        public int MachineId { get; private set; }
        public string MachineUserDefinedId { get; private set; }
        public string MachineName { get; private set; }

        public event EventHandler DetailsRequested;
        public event EventHandler VncRequested;

        private readonly Color _colorConnected = Color.FromArgb(46, 204, 113);
        private readonly Color _colorDisconnected = Color.FromArgb(231, 76, 60);
        private readonly Color _colorConnecting = Color.FromArgb(241, 196, 15);
        private readonly Color _colorAlarm = Color.FromArgb(192, 57, 43);
        private readonly Color _colorPlay = Color.FromArgb(46, 204, 113); // Yeşil
        private readonly Color _colorPause = Color.FromArgb(241, 196, 15); // Sarı

        public MachineCard_Control(int machineId, string machineUserDefinedId, string machineName, int displayIndex)
        {
            InitializeComponent();
            this.MachineId = machineId;
            this.MachineUserDefinedId = machineUserDefinedId;
            this.MachineName = machineName;
            lblMachineNumber.Text = $"{displayIndex}.";
            UpdateView(new FullMachineStatus { ConnectionState = ConnectionStatus.Disconnected, MachineName = this.MachineName });
        }

        public void UpdateView(FullMachineStatus status)
        {
            if (this.IsDisposed || !this.IsHandleCreated) return;
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateView(status)));
                return;
            }

            switch (status.ConnectionState)
            {
                case ConnectionStatus.Connected:
                    picConnection.BackColor = _colorConnected;
                    break;
                case ConnectionStatus.Connecting:
                    picConnection.BackColor = _colorConnecting;
                    break;
                case ConnectionStatus.ConnectionLost:
                case ConnectionStatus.Disconnected:
                    picConnection.BackColor = _colorDisconnected;
                    ClearData();
                    return;
            }

            lblRecipeNameValue.Text = status.RecipeName;
            lblOperatorValue.Text = status.OperatorIsmi;
            lblStepValue.Text = status.AktifAdimAdi;
            lblMachineNameValue.Text = status.MachineName;
            lblMachineIdValue.Text = this.MachineUserDefinedId;

            // İKON GÖRÜNÜRLÜĞÜ VE RENKLERİ
            picPlay.Visible = status.IsInRecipeMode && !status.IsPaused;
            picPlay.BackColor = _colorPlay;

            picPause.Visible = status.IsPaused;
            picPause.BackColor = _colorPause;

            picAlarm.Visible = status.HasActiveAlarm;
            picAlarm.BackColor = status.HasActiveAlarm ? _colorAlarm : Color.Transparent;

            int progress = Math.Max(0, Math.Min(100, (int)status.ProsesYuzdesi));
            progressBar.Value = progress;
            lblPercentage.Text = $"{progress} %";
        }

        private void ClearData()
        {
            string noConnectionText = "---";
            lblRecipeNameValue.Text = noConnectionText;
            lblOperatorValue.Text = noConnectionText;
            lblStepValue.Text = noConnectionText;
            lblMachineNameValue.Text = this.MachineName;
            lblMachineIdValue.Text = this.MachineUserDefinedId;
            progressBar.Value = 0;
            lblPercentage.Text = "0 %";

            picPlay.Visible = false;
            picPause.Visible = false;
            picAlarm.Visible = false;
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            DetailsRequested?.Invoke(this, EventArgs.Empty);
        }

        private void btnVnc_Click(object sender, EventArgs e)
        {
            VncRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
