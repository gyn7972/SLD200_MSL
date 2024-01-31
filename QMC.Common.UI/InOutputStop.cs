
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
    public partial class InOutputStop : UserControl
    {
        #region Filed
        public Module m_Owner;
        #endregion

        #region Constructor
        public InOutputStop(Module module)
        {
            InitializeComponent();
            m_Owner = module;
        }
        #endregion

        #region Method
        private void baseToggleButtonInputStop_Click(object sender, EventArgs e)
        {
            //if(m_Owner is Loader)
            //{
            //    Loader loader = m_Owner as Loader;

            //    bool bOn = baseToggleButtonStop.GetButtonStatus();
            //    if (bOn && loader != null)
            //    {
            //        loader.InputStop = false;
            //        baseToggleButtonStop.UpdateToggleStatus(false);
            //    }
            //    else
            //    {
            //        loader.InputStop = true;
            //        baseToggleButtonStop.UpdateToggleStatus(true);
            //    }
            //}
            //else
            //{
            //    Unloader unLoader = m_Owner as Unloader;

            //    bool bOn = baseToggleButtonStop.GetButtonStatus();
            //    if (bOn && unLoader != null)
            //    {
            //        unLoader.OutputStop = false;
            //        baseToggleButtonStop.UpdateToggleStatus(false);
            //    }
            //    else
            //    {
            //        unLoader.OutputStop = true;
            //        baseToggleButtonStop.UpdateToggleStatus(true);
            //    }
            //}
        }
        public void UpdateState()
        {
            bool bOn = baseToggleButtonStop.GetButtonStatus();
            if (bOn)
            {
                baseToggleButtonStop.Text = "STOP";
                baseToggleButtonStop.ForeColor = Color.White;
                baseToggleButtonStop.BackColor = Color.Red;
            }
            else
            {
                baseToggleButtonStop.Text = "Runing";
                baseToggleButtonStop.ForeColor = Color.Black;
                baseToggleButtonStop.BackColor = Color.LightGreen;
            }
        }
        public void SetGroupBoxName(string strName)
        {
            baseGroupBoxName.Text = strName;
        }
        #endregion
    }
}
