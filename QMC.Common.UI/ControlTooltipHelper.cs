using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QMC.Common.UI
{
    public class ControlTooltipHelper
    {
        private ToolTip _toolTip;

        public ControlTooltipHelper()
        {
            _toolTip = new ToolTip
            {
                AutoPopDelay = 5000,
                InitialDelay = 300,
                ReshowDelay = 100,
                ShowAlways = true
            };
        }

        public void AddTooltip(Control control, string text)
        {
            if (control != null && !string.IsNullOrWhiteSpace(text))
            {
                _toolTip.SetToolTip(control, text);
            }
        }

        public void AddTooltips(Dictionary<Control, string> tooltips)
        {
            foreach (var kv in tooltips)
            {
                AddTooltip(kv.Key, kv.Value);
            }
        }
    }
}
