using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QMC.Common;
namespace CWA150SA_Onsemi300
{
    public delegate void GripperButtonClickHandler(GripperContorl.ButtonType type);
    public partial class GripperContorl : UserControl
    {
        public enum ButtonType
        {
            Hold,
            Release,
        }
        public GripperButtonClickHandler ButtonClick;
        public GripperContorl(Module module)
        {
            InitializeComponent();
        }
        private void OnButtonClick(ButtonType type)
        {

        }
        public void SetGroupBoxName(string name)
        {
            baseGripperGroupBox.Text = name;
        }

        private void baseToggleButtonHold_Click(object sender, EventArgs e)
        {
            if (ButtonClick != null)
            {
                ButtonClick(ButtonType.Hold);
            }
        }

        public void SetButtonStatus(ButtonType type, bool bOn)
        {
            if(type == ButtonType.Hold)
            {
               baseToggleButtonHold.UpdateToggleStatus(bOn);
            }
            else if(type == ButtonType.Release)
            {
                baseToggleButtonRelease.UpdateToggleStatus(bOn);
            }
            else
            {

            }
        }

        private void baseToggleButtonRelease_Click(object sender, EventArgs e)
        {
            if (ButtonClick != null)
            {
                ButtonClick(ButtonType.Release);
            }
        }
    }
}
