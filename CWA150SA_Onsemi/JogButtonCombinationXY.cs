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
    public delegate void JogCombXYButtonClickEventHandler(JogControlButtonList type, List<MotionAxis> AxisList);
    public delegate void JogCombXYButtonDownEventHandler(JogControlButtonList type, List<MotionAxis> AxisList);
    public delegate void JogCombXYButtonUpEventHandler(JogControlButtonList type, List<MotionAxis> AxisList);
    public partial class JogButtonCombinationXY : UserControl
    {
        //public List<MotionAxis> CombValueList = new List<MotionAxis>();
        public MotionAxis HorizontalAxis { get; set; }
        public MotionAxis VerticalAxis { get; set;}
        public event JogCombXYButtonClickEventHandler JogButtonXYClick;
        public event JogCombXYButtonDownEventHandler JogButtonXYDown;
        public event JogCombXYButtonUpEventHandler JogButtonXYUp;
        
        public List<MotionAxis> AxisList { get; set; }
        public JogButtonCombinationXY()
        {
            InitializeComponent();
            this.baseLabelJogButtonComb.Size = new Size(this.Size.Width, this.Height - this.buttonAxisXDown.Height * 3-10);
            this.baseLabelJogButtonComb.Location = new Point(0, this.buttonAxisXDownYDown.Location.Y + this.buttonAxisXDownYDown.Height+2);
            this.baseLabelJogButtonComb.TextAlign = ContentAlignment.MiddleCenter;
            this.baseLabelJogButtonComb.Text = string.Empty;
            this.baseLabelJogButtonComb.Font = new Font(baseLabelJogButtonComb.Font.FontFamily, 9);

            this.Size = new Size(this.buttonAxisXDown.Width * 3 + 14, this.buttonAxisXDown.Height * 3 +17 );

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
            if (JogButtonXYClick != null)
            {
                JogButtonXYClick(JogControlButtonList.combButtonLeft, AxisList);
            }
        }
        private void ButtonAxisXRight_Click(object sender, EventArgs e)
        {
            if (JogButtonXYClick != null)
            {
                JogButtonXYClick(JogControlButtonList.combButtonRight, AxisList);
            }
        }
        

        private void buttonAxisYUp_Click(object sender, EventArgs e)
        {
            if (JogButtonXYClick != null)
            {
                JogButtonXYClick(JogControlButtonList.combButtonUp, AxisList);
            }
        }
        private void buttonAxisYDown_Click(object sender, EventArgs e)
        {
            if (JogButtonXYClick != null)
            {
                JogButtonXYClick(JogControlButtonList.combButtonDown, AxisList);
            }
        }

        private void buttonAxisXDownYDown_Click(object sender, EventArgs e)
        {
            if (JogButtonXYClick != null)
            {
                JogButtonXYClick(JogControlButtonList.buttonAxisXDownYDown, AxisList);
            }
        }

        private void buttonAxisXUpYDown_Click(object sender, EventArgs e)
        {
            if (JogButtonXYClick != null)
            {
                JogButtonXYClick(JogControlButtonList.buttonAxisXUpYDown, AxisList);
            }
        }

        private void buttonAxisXDownYUp_Click(object sender, EventArgs e)
        {
            if (JogButtonXYClick != null)
            {
                JogButtonXYClick(JogControlButtonList.buttonAxisXDownYUp, AxisList);
            }
        }        

        private void buttonAxisXUpYUp_Click(object sender, EventArgs e)
        {
            if (JogButtonXYClick != null)
            {
                JogButtonXYClick(JogControlButtonList.buttonAxisXUpYUp, AxisList);
            }
        }
        #endregion

        #region buttonDownEvent
        private void buttonAxisXDownYDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonXYDown(JogControlButtonList.buttonAxisXDownYDown, AxisList);
            }
        }

        private void buttonAxisYUp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonXYDown(JogControlButtonList.combButtonUp, AxisList);
            }
        }

        private void buttonAxisXUpYDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonXYDown(JogControlButtonList.buttonAxisXUpYDown, AxisList);
            }
        }

        private void buttonAxisXDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonXYDown(JogControlButtonList.combButtonLeft, AxisList);
            }
        }

        private void buttonAxisXUp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonXYDown(JogControlButtonList.combButtonRight, AxisList);
            }
        }

        private void buttonAxisXDownYUp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonXYDown(JogControlButtonList.buttonAxisXDownYUp, AxisList);
            }
        }

        private void buttonAxisYDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonXYDown(JogControlButtonList.combButtonDown, AxisList);
            }
        }

        private void buttonAxisXUpYUp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonXYDown(JogControlButtonList.buttonAxisXUpYUp, AxisList);
            }
        }
        #endregion

        #region buttonUpEvent
        private void buttonAxisXDownYDown_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonXYUp(JogControlButtonList.buttonAxisXDownYDown, AxisList);
            }
        }

        private void buttonAxisYUp_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonXYUp(JogControlButtonList.combButtonUp, AxisList);
            }
        }

        private void buttonAxisXUpYDown_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonXYUp(JogControlButtonList.buttonAxisXUpYDown, AxisList);
            }
        }

        private void buttonAxisXDown_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonXYUp(JogControlButtonList.combButtonLeft, AxisList);
            }
        }

        private void buttonAxisXUp_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonXYUp(JogControlButtonList.combButtonRight, AxisList);
            }
        }

        private void buttonAxisXDownYUp_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonXYUp(JogControlButtonList.buttonAxisXDownYUp, AxisList);
            }
        }

        private void buttonAxisYDown_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonXYUp(JogControlButtonList.combButtonDown, AxisList);
            }
        }

        private void buttonAxisXUpYUp_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonXYUp(JogControlButtonList.buttonAxisXUpYUp, AxisList);
            }
        }
        #endregion
    }
}
