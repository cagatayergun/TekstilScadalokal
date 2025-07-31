using System.Drawing;
using System.Windows.Forms;

namespace TekstilScada.UI.Controls
{
    public partial class KpiCard_Control : UserControl
    {
        public KpiCard_Control()
        {
            InitializeComponent();
        }

        public void SetData(string title, string value, Color backgroundColor)
        {
            lblKpiTitle.Text = title;
            lblKpiValue.Text = value;
            this.BackColor = backgroundColor;
        }
    }
}