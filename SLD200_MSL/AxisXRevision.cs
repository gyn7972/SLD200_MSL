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

namespace SLD200_MSL
{
    public partial class AxisXRevision : UserControl
    {
        public event JogButtonClickEventHandler JogButtonClick;
        public event JogButtonDownEventHandler JogButtonDown;
        public event JogButtonUpEventHandler JogButtonUp;
        public List<MotionAxis> AxisList { get; set; }
        public MotionAxis AxisValue { get; set; }       
        public AxisXRevision()
        {
            InitializeComponent();
            AxisList = new List<MotionAxis>();
            this.baseLabelAxisX.Size = new Size(this.buttonDown.Width, this.buttonDown.Height);
            this.baseLabelAxisX.Location = new Point(this.buttonDown.Location.X + this.buttonDown.Width, this.buttonUp.Location.Y);
            this.baseLabelAxisX.TextAlign = ContentAlignment.MiddleCenter;
            this.buttonUp.Location = new Point(this.baseLabelAxisX.Location.X + this.baseLabelAxisX.Width, this.baseLabelAxisX.Location.Y);
            //this.baseLabelAxisX.Font = new Font(baseLabelAxisX.Font.FontFamily, 9);
            this.baseLabelAxisX.Font = new Font("Tahoma", 9);
        }

        #region SetButtonName
        public void SetButtonName(string name)
        {
            this.baseLabelAxisX.Text = name;
        }
        #endregion

        #region buttonClickEvent
        private void buttonAxisXUp_Click(object sender, EventArgs e)
        {
            if (JogButtonClick != null)
            {
                JogButtonClick(JogControlButtonList.buttonRight, AxisList);
            }
        }

        private void buttonAxisXDown_Click(object sender, EventArgs e)
        {
            if (JogButtonClick != null)
            {
                JogButtonClick(JogControlButtonList.buttonLeft, AxisList);
            }
        }
        #endregion

        #region buttonDownEvent
        private void buttonAxisXUp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonDown(JogControlButtonList.buttonRight, AxisList);
            }
        }

        private void buttonAxisXDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonDown(JogControlButtonList.buttonLeft, AxisList);
            }
        }
        #endregion

        #region buttonUpEvent
        private void buttonAxisXUp_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonUp(JogControlButtonList.buttonRight, AxisList);
            }
        }

        private void buttonAxisXDown_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonUp(JogControlButtonList.buttonLeft, AxisList);
            }
        }
        #endregion

    }
}
