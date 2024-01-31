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

namespace QMC.Common.UI
{
    public delegate void JogButtonClickEventHandler(JogControlButtonList type, List<MotionAxis> axisList);
    public delegate void JogButtonDownEventHandler(JogControlButtonList type, List<MotionAxis> axisList);
    public delegate void JogButtonUpEventHandler(JogControlButtonList type, List<MotionAxis> axisList);
    public partial class AxisYRevision : UserControl
    {
        public event JogButtonClickEventHandler JogButtonClick;
        public event JogButtonDownEventHandler JogButtonDown;
        public event JogButtonUpEventHandler JogButtonUp;
        public List<MotionAxis> AxisList { get; set; }
        public MotionAxis AxisValue { get; set;  }
        public AxisYRevision()
        {
            InitializeComponent();
            AxisList = new List<MotionAxis>();
            this.baseLabelAxisYRevision.Size = new Size(this.buttonDown.Width, this.buttonDown.Height);
            this.baseLabelAxisYRevision.Location = new Point(this.buttonDown.Location.X, this.buttonUp.Location.Y + this.buttonUp.Height);
            this.baseLabelAxisYRevision.TextAlign = ContentAlignment.MiddleCenter;
            this.buttonDown.Location = new Point(this.baseLabelAxisYRevision.Location.X, this.baseLabelAxisYRevision.Location.Y + this.baseLabelAxisYRevision.Height);
            this.baseLabelAxisYRevision.Font = new Font(baseLabelAxisYRevision.Font.FontFamily, 9);
        }

        #region SetButtonName
        public void SetButtonName(string name)
        {
            this.baseLabelAxisYRevision.Text = name;            
        }
        #endregion

        #region buttonClickEvent
        private void buttonAxisYUp_Click(object sender, EventArgs e)
        {
            if (JogButtonClick != null)
            {
                JogButtonClick(JogControlButtonList.buttonUp, AxisList);
            }
        }

        private void buttonAxisYDown_Click(object sender, EventArgs e)
        {
            if (JogButtonClick != null)
            {
                JogButtonClick(JogControlButtonList.buttonDown, AxisList);
            }
        }
        #endregion

        #region buttonDownEvent

        private void buttonAxisUp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonDown(JogControlButtonList.buttonUp, AxisList);
            }
        }

        private void buttonAxisYDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonDown(JogControlButtonList.buttonDown, AxisList);
            }
        }
        #endregion

        #region buttonUpEvent
        private void buttonAxisUp_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonUp(JogControlButtonList.buttonUp, AxisList);
            }
        }

        private void buttonAxisYDown_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonUp(JogControlButtonList.buttonDown, AxisList);
            }
        }
        #endregion
    }
}
