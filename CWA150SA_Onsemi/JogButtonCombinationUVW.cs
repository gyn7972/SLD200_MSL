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
    public delegate void JogCombButtonClickEventHandler(JogControlButtonList type, List<MotionAxis> AxisList);
    public delegate void JogCombButtonDownEventHandler(JogControlButtonList type, List<MotionAxis> AxisList);
    public delegate void JogCombButtonUpEventHandler(JogControlButtonList type, List<MotionAxis> AxisList);
    public partial class JogButtonCombinationUVW : UserControl
    {
        //public List<MotionAxis> CombValueList = new List<MotionAxis>();
        public MotionAxis HorizontalAxis { get; set; }
        public MotionAxis VerticalAxis1 { get; set;}
        public MotionAxis VerticalAxis2 { get; set; }
        public event JogCombButtonClickEventHandler JogButtonUVWClick;
        public event JogCombButtonDownEventHandler JogButtonUVWDown;
        public event JogCombButtonUpEventHandler JogButtonUVWUp;
        
        public List<MotionAxis> AxisList { get; set; }
        public JogButtonCombinationUVW()
        {
            InitializeComponent();
            this.baseLabelJogButtonComb.Size = new Size(this.Size.Width, this.Height - this.buttonAxisXDown.Height * 3-10);
            this.baseLabelJogButtonComb.Location = new Point(0, this.buttonAxisXDownYDown.Location.Y + this.buttonAxisXDownYDown.Height+2);
            this.baseLabelJogButtonComb.TextAlign = ContentAlignment.MiddleCenter;
            this.baseLabelJogButtonComb.Text = string.Empty;
            this.baseLabelJogButtonComb.Font = new Font(baseLabelJogButtonComb.Font.FontFamily, 9);

            //this.Size = new Size(this.buttonAxisXDown.Width * 3 + 14, this.buttonAxisXDown.Height * 3 +17 );
            this.Size = new Size(this.Width, this.Height );

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
            if (JogButtonUVWClick != null)
            {
                JogButtonUVWClick(JogControlButtonList.combButtonLeft, AxisList);
            }
        }
        private void ButtonAxisXRight_Click(object sender, EventArgs e)
        {
            if (JogButtonUVWClick != null)
            {
                JogButtonUVWClick(JogControlButtonList.combButtonRight, AxisList);
            }
        }
        

        private void buttonAxisYUp_Click(object sender, EventArgs e)
        {
            if (JogButtonUVWClick != null)
            {
                JogButtonUVWClick(JogControlButtonList.combButtonUp, AxisList);
            }
        }
        private void buttonAxisYDown_Click(object sender, EventArgs e)
        {
            if (JogButtonUVWClick != null)
            {
                JogButtonUVWClick(JogControlButtonList.combButtonDown, AxisList);
            }
        }

        private void buttonAxisXDownYDown_Click(object sender, EventArgs e)
        {
            if (JogButtonUVWClick != null)
            {
                JogButtonUVWClick(JogControlButtonList.buttonAxisXDownYDown, AxisList);
            }
        }

        private void buttonAxisXUpYDown_Click(object sender, EventArgs e)
        {
            if (JogButtonUVWClick != null)
            {
                JogButtonUVWClick(JogControlButtonList.buttonAxisXUpYDown, AxisList);
            }
        }

        private void buttonAxisXDownYUp_Click(object sender, EventArgs e)
        {
            if (JogButtonUVWClick != null)
            {
                JogButtonUVWClick(JogControlButtonList.buttonAxisXDownYUp, AxisList);
            }
        }        

        private void buttonAxisXUpYUp_Click(object sender, EventArgs e)
        {
            if (JogButtonUVWClick != null)
            {
                JogButtonUVWClick(JogControlButtonList.buttonAxisXUpYUp, AxisList);
            }
        }
        #endregion

        #region buttonDownEvent
        private void buttonAxisXDownYDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonUVWDown(JogControlButtonList.buttonAxisXDownYDown, AxisList);
            }
        }

        private void buttonAxisYUp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonUVWDown(JogControlButtonList.combButtonUp, AxisList);
            }
        }

        private void buttonAxisXUpYDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonUVWDown(JogControlButtonList.buttonAxisXUpYDown, AxisList);
            }
        }

        private void buttonAxisXDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonUVWDown(JogControlButtonList.combButtonLeft, AxisList);
            }
        }

        private void buttonAxisXUp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonUVWDown(JogControlButtonList.combButtonRight, AxisList);
            }
        }

        private void buttonAxisXDownYUp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonUVWDown(JogControlButtonList.buttonAxisXDownYUp, AxisList);
            }
        }

        private void buttonAxisYDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonUVWDown(JogControlButtonList.combButtonDown, AxisList);
            }
        }

        private void buttonAxisXUpYUp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonUVWDown(JogControlButtonList.buttonAxisXUpYUp, AxisList);
            }
        }
        #endregion

        #region buttonUpEvent
        private void buttonAxisXDownYDown_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonUVWUp(JogControlButtonList.buttonAxisXDownYDown, AxisList);
            }
        }

        private void buttonAxisYUp_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonUVWUp(JogControlButtonList.combButtonUp, AxisList);
            }
        }

        private void buttonAxisXUpYDown_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonUVWUp(JogControlButtonList.buttonAxisXUpYDown, AxisList);
            }
        }

        private void buttonAxisXDown_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonUVWUp(JogControlButtonList.combButtonLeft, AxisList);
            }
        }

        private void buttonAxisXUp_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonUVWUp(JogControlButtonList.combButtonRight, AxisList);
            }
        }

        private void buttonAxisXDownYUp_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonUVWUp(JogControlButtonList.buttonAxisXDownYUp, AxisList);
            }
        }

        private void buttonAxisYDown_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonUVWUp(JogControlButtonList.combButtonDown, AxisList);
            }
        }

        private void buttonAxisXUpYUp_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonUVWUp(JogControlButtonList.buttonAxisXUpYUp, AxisList);
            }
        }
        #endregion

        private void buttonAxisXYCCW_Click(object sender, EventArgs e)
        {
            if (JogButtonUVWClick != null)
            {
                JogButtonUVWClick(JogControlButtonList.buttonCCW, AxisList);
            }
        }

        private void buttonAxisXYCW_Click(object sender, EventArgs e)
        {
            if (JogButtonUVWClick != null)
            {
                JogButtonUVWClick(JogControlButtonList.buttonCW, AxisList);
            }
        }

        private void baseLabelJogButtonComb_U_Click(object sender, EventArgs e)
        {

        }

        private void buttonAxisXYCCW_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonUVWDown(JogControlButtonList.buttonCCW, AxisList);
            }
        }

        private void buttonAxisXYCCW_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonUVWUp(JogControlButtonList.combButtonUp, AxisList);
            }
        }

        private void buttonAxisXYCW_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonUVWDown(JogControlButtonList.buttonCW, AxisList);
            }
        }
    }
}
