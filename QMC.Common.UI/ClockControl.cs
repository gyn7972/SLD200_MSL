using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QMC.Common.UI
{
    public partial class ClockControl : UserControl
    {
        #region Field
        #endregion

        #region Constructor
        
        public ClockControl()
        {
            InitializeComponent();
            //m_bExit = false;
            this.baseLabelDate.ForeColor = Color.White;
            this.baseLabelDate.BackColor = Color.FromArgb(78, 78, 78);

        }
        #endregion

        #region Method
        public void ChangeRealTime()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(delegate ()
                {
                    this.baseLabelDate.Text = DateTime.Now.ToShortDateString();
                    this.baseLabelTime.Text = DateTime.Now.ToShortTimeString();
                }));
            }
            else
            {
                this.baseLabelDate.Text = DateTime.Now.ToShortDateString();
                this.baseLabelTime.Text = DateTime.Now.ToShortTimeString();
            }
        }
        #endregion
    }
}