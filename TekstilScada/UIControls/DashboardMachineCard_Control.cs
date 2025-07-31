// ############################################################################
// ## YENİ DOSYA: UIControls/DashboardMachineCard_Control.cs
// ## AÇIKLAMA: Bu, sadece Genel Bakış sayfası için oluşturulmuş YENİ bir karttır.
// ## Orijinal MachineCard_Control'e dokunmaz.
// ############################################################################

using System;
using System.Drawing;
using System.Windows.Forms;
using TekstilScada.Models;

namespace TekstilScada.UIControls
{
    public partial class DashboardMachineCard_Control : UserControl
    {
        public DashboardMachineCard_Control()
        {
            InitializeComponent();
        }

        public void SetMachineName(string name)
        {
            lblMachineName.Text = name;
        }

      
    }
}