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
    public partial class JogButtonXY : UserControl
    {
        public event JogButtonClickEventHandler JogButtonClick;
        public event JogButtonDownEventHandler JogButtonDown;
        public event JogButtonUpEventHandler JogButtonUp;
        public List<MotionAxis> AxisList { get; set; }
        public MotionAxis thetaValue { set; get; }

        public JogButtonXY()
        {
            InitializeComponent();
            AxisList = new List<MotionAxis>();
            //this.baseLabelX.Size = new Size(this.buttonCCW.Width, this.buttonCCW.Height);
            //this.baseLabelX.Location = new Point(this.buttonCCW.Location.X + this.buttonCCW.Width, this.buttonCCW.Location.Y);
            this.baseLabelX.TextAlign = ContentAlignment.MiddleCenter;
            this.baseLabelY.TextAlign = ContentAlignment.MiddleCenter;
            //this.buttonCW.Location = new Point(this.baseLabelX.Location.X + this.baseLabelX.Width, this.buttonCCW.Location.Y);
            this.baseLabelX.Font = new Font(baseLabelX.Font.FontFamily, 9);
            this.baseLabelY.Font = new Font(baseLabelX.Font.FontFamily, 9);
        }

        #region SetButtonName
        public void SetButtonName(string name)
        {
            this.baseLabelX.Text = name;
        }
        #endregion

        #region buttonClickEvent

        private void buttonMoveLeft_Click(object sender, EventArgs e)
        {
            if (JogButtonClick != null)
            {
                JogButtonClick(JogControlButtonList.buttonLeft, AxisList);
            }
        }

        private void buttonMoveRight_Click(object sender, EventArgs e)
        {
            if (JogButtonClick != null)
            {
                JogButtonClick(JogControlButtonList.buttonRight, AxisList);
            }
        }

        private void buttonMoveForward_Click(object sender, EventArgs e)
        {
            if (JogButtonClick != null)
            {
                JogButtonClick(JogControlButtonList.buttonFwd, AxisList);
            }
        }

        private void buttonMoveBackward_Click(object sender, EventArgs e)
        {
            if (JogButtonClick != null)
            {
                JogButtonClick(JogControlButtonList.buttonBwd, AxisList);
            }
        }

        private void buttonMoveUp_Click(object sender, EventArgs e)
        {
            if (JogButtonClick != null)
            {
                JogButtonClick(JogControlButtonList.buttonUp, AxisList);
            }
        }

        private void buttonMoveDown_Click(object sender, EventArgs e)
        {
            if (JogButtonClick != null)
            {
                JogButtonClick(JogControlButtonList.buttonDown, AxisList);
            }
        }

        #endregion

        #region buttonDownEvent

        private void buttonAxisXUp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonDown(JogControlButtonList.buttonCW, AxisList);
            }
        }

        private void buttonAxisXDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonDown(JogControlButtonList.buttonCCW, AxisList);
            }
        }

        #endregion

        #region buttonUpEvent

        private void buttonAxisXUp_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonUp(JogControlButtonList.buttonCW, AxisList);
            }
        }

        private void buttonAxisXDown_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonUp(JogControlButtonList.buttonCCW, AxisList);
            }
        }

        #endregion

        private void buttonAxisXLeft_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonDown(JogControlButtonList.buttonLeft, AxisList);
            }
        }

        private void buttonAxisXLeft_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonUp(JogControlButtonList.buttonLeft, AxisList);
            }
        }

        private void buttonAxisXRight_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonDown(JogControlButtonList.buttonRight, AxisList);
            }
        }

        private void buttonAxisXRight_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonUp(JogControlButtonList.buttonRight, AxisList);
            }
        }

        private void buttonAxisYFwd_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonDown(JogControlButtonList.buttonFwd, AxisList);
            }
        }

        private void buttonAxisYFwd_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonUp(JogControlButtonList.buttonFwd, AxisList);
            }
        }

        private void buttonAxisYBwd_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonDown(JogControlButtonList.buttonBwd, AxisList);
            }
        }

        private void buttonAxisYBwd_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonUp(JogControlButtonList.buttonBwd, AxisList);
            }
        }
    }
}
