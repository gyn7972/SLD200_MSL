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
    public partial class JogButtonTheta : UserControl
    {
        public event JogButtonClickEventHandler JogButtonClick;
        public event JogButtonDownEventHandler JogButtonDown;
        public event JogButtonUpEventHandler JogButtonUp;
        public List<MotionAxis> AxisList { get; set; }
        public MotionAxis thetaValue { set; get; }

        public JogButtonTheta()
        {
            InitializeComponent();
            AxisList = new List<MotionAxis>();
            this.baseLabelTheta.Size = new Size(this.buttonCCW.Width, this.buttonCCW.Height);
            this.baseLabelTheta.Location = new Point(this.buttonCCW.Location.X + this.buttonCCW.Width, this.buttonCCW.Location.Y);
            this.baseLabelTheta.TextAlign = ContentAlignment.MiddleCenter;
            this.buttonCW.Location = new Point(this.baseLabelTheta.Location.X + this.baseLabelTheta.Width, this.buttonCCW.Location.Y);
            this.baseLabelTheta.Font = new Font(baseLabelTheta.Font.FontFamily, 9);
        }

        #region SetButtonName
        public void SetButtonName(string name)
        {
            this.baseLabelTheta.Text = name;
        }
        #endregion

        #region buttonClickEvent

        private void buttonThetaLeft_Click(object sender, EventArgs e)
        {
            if (JogButtonClick != null)
            {
                JogButtonClick(JogControlButtonList.buttonCW, AxisList);
            }
        }

        private void buttonThetaRight_Click(object sender, EventArgs e)
        {
            if (JogButtonClick != null)
            {
                JogButtonClick(JogControlButtonList.buttonCCW, AxisList);
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

    }
}
