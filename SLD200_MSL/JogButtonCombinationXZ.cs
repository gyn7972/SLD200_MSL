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
    public delegate void JogCombXZButtonClickEventHandler(JogControlButtonList type, List<MotionAxis> AxisList);
    public delegate void JogCombXZButtonDownEventHandler(JogControlButtonList type, List<MotionAxis> AxisList);
    public delegate void JogCombXZButtonUpEventHandler(JogControlButtonList type, List<MotionAxis> AxisList);
    public partial class JogButtonCombinationXZ : UserControl
    {
        //public List<MotionAxis> CombValueList = new List<MotionAxis>();
        public MotionAxis HorizontalAxis { get; set; }
        public MotionAxis VerticalAxis { get; set;}
        public MotionAxis Z0Axis { get; set; }
        public MotionAxis Z1Axis { get; set; }

        public event JogCombXZButtonClickEventHandler JogButtonXZClick;
        public event JogCombXZButtonDownEventHandler JogButtonXZDown;
        public event JogCombXZButtonUpEventHandler JogButtonXZUp;
        
        public List<MotionAxis> AxisList { get; set; }
        public JogButtonCombinationXZ()
        {
            InitializeComponent();
            this.baseLabelJogButtonComb.Size = new Size(this.Size.Width, this.Height - this.buttonAxisXDown.Height * 3-10);
            this.baseLabelJogButtonComb.Location = new Point(0, this.buttonAxisYUp.Location.Y + this.buttonAxisYUp.Height+2);
            this.baseLabelJogButtonComb.TextAlign = ContentAlignment.MiddleCenter;
            this.baseLabelJogButtonComb.Text = string.Empty;
            this.baseLabelJogButtonComb.Font = new Font(baseLabelJogButtonComb.Font.FontFamily, 9);

            this.baseLabelJogButtonComb.Visible = false;

            //this.Size = new Size(this.buttonAxisXDown.Width * 3 + 14, this.buttonAxisXDown.Height * 3 +17 );

            AxisList = new List<MotionAxis>();
        }

        #region SetButtonName
        public void SetButtonName(string strName1, string strName2)
        {
            this.baseLabelJogButtonComb.Text = string.Format("{0}, {1}", strName1, strName2);
        }
        #endregion

        #region buttonClickEvent

        private void ButtonAxisXLeft_Click(object sender, EventArgs e)
        {
            if (JogButtonXZClick != null)
            {
                JogButtonXZClick(JogControlButtonList.combButtonLeft, AxisList);
            }
        }
        private void ButtonAxisXRight_Click(object sender, EventArgs e)
        {
            if (JogButtonXZClick != null)
            {
                JogButtonXZClick(JogControlButtonList.combButtonRight, AxisList);
            }
        }
        

        private void buttonAxisYUp_Click(object sender, EventArgs e)
        {
            if (JogButtonXZClick != null)
            {
                JogButtonXZClick(JogControlButtonList.combButtonUp, AxisList);
            }
        }
        private void buttonAxisYDown_Click(object sender, EventArgs e)
        {
            if (JogButtonXZClick != null)
            {
                JogButtonXZClick(JogControlButtonList.combButtonDown, AxisList);
            }
        }
        private void buttonAxisXDownYDown_Click(object sender, EventArgs e)
        {
            if (JogButtonXZClick != null)
            {
                JogButtonXZClick(JogControlButtonList.buttonAxisXDownYDown, AxisList);
            }
        }

        private void buttonAxisXUpYDown_Click(object sender, EventArgs e)
        {
            if (JogButtonXZClick != null)
            {
                JogButtonXZClick(JogControlButtonList.buttonAxisXUpYDown, AxisList);
            }
        }

        private void buttonAxisXDownYUp_Click(object sender, EventArgs e)
        {
            if (JogButtonXZClick != null)
            {
                JogButtonXZClick(JogControlButtonList.buttonAxisXDownYUp, AxisList);
            }
        }

        private void buttonAxisXUpYUp_Click(object sender, EventArgs e)
        {
            if (JogButtonXZClick != null)
            {
                JogButtonXZClick(JogControlButtonList.buttonAxisXUpYUp, AxisList);
            }
        }
        #endregion

        #region buttonDownEvent
        private void buttonAxisYUp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonXZDown(JogControlButtonList.combButtonUp, AxisList);
            }
        }

        private void buttonAxisXDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonXZDown(JogControlButtonList.combButtonLeft, AxisList);
            }
        }

        private void buttonAxisXUp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonXZDown(JogControlButtonList.combButtonRight, AxisList);
            }
        }

        private void buttonAxisYDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonXZDown(JogControlButtonList.combButtonDown, AxisList);
            }
        }
        private void buttonAxisXDownYDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonXZDown(JogControlButtonList.buttonAxisXDownYDown, AxisList);
            }
        }
        private void buttonAxisXUpYDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonXZDown(JogControlButtonList.buttonAxisXUpYDown, AxisList);
            }
        }
        private void buttonAxisXDownYUp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonXZDown(JogControlButtonList.buttonAxisXDownYUp, AxisList);
            }
        }
        private void buttonAxisXUpYUp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonXZDown(JogControlButtonList.buttonAxisXUpYUp, AxisList);
            }
        }
        #endregion

        #region buttonUpEvent
        private void buttonAxisYUp_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonXZUp(JogControlButtonList.combButtonUp, AxisList);
            }
        }

        private void buttonAxisXDown_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonXZUp(JogControlButtonList.combButtonLeft, AxisList);
            }
        }

        private void buttonAxisXUp_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonXZUp(JogControlButtonList.combButtonRight, AxisList);
            }
        }

        private void buttonAxisYDown_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonXZUp(JogControlButtonList.combButtonDown, AxisList);
            }
        }
        private void buttonAxisXDownYDown_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonXZUp(JogControlButtonList.buttonAxisXDownYDown, AxisList);
            }
        }
        private void buttonAxisXUpYDown_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonXZUp(JogControlButtonList.buttonAxisXUpYDown, AxisList);
            }
        }
        private void buttonAxisXDownYUp_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonXZUp(JogControlButtonList.buttonAxisXDownYUp, AxisList);
            }
        }
        private void buttonAxisXUpYUp_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonXZUp(JogControlButtonList.buttonAxisXUpYUp, AxisList);
            }
        }
        #endregion
    }
}
