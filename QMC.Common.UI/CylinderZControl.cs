using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QMC.Common.UI
{
    public delegate void CylinderZButtonClickHandler(CylinderZControl.ButtonType type);

    public partial class CylinderZControl : UserControl
    {
        public enum ButtonType
        {
            Up,
            Down,
        }
        public event CylinderZButtonClickHandler ButtonClick;
        public CylinderZControl()
        {
            InitializeComponent();
        }

        private void baseToggleButtonUp_Click(object sender, EventArgs e)
        {
            if(ButtonClick != null)
            {
                baseToggleButtonUp.UpdateToggleStatus(true);
                baseToggleButtonDown.UpdateToggleStatus(false);
                ButtonClick(ButtonType.Up);
            }
        }

        private void baseToggleButtonDown_Click(object sender, EventArgs e)
        {
            if (ButtonClick != null)
            {
                baseToggleButtonUp.UpdateToggleStatus(false);
                baseToggleButtonDown.UpdateToggleStatus(true);
                ButtonClick(ButtonType.Down);
            }
        }
        public void UpdateCylinderZStatus(bool bOn)
        {
            if(bOn)
            {
                baseToggleButtonUp.UpdateToggleStatus(true);
                baseToggleButtonDown.UpdateToggleStatus(false);
            }
            else
            {
                baseToggleButtonUp.UpdateToggleStatus(false);
                baseToggleButtonDown.UpdateToggleStatus(true);
            }
        }
    }
}
