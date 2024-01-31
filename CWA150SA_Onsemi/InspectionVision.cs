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
    public partial class InspectionVision : UserControl
    {
        public InspectionVision()
        {
            InitializeComponent();
        }

        #region SetButtonName
        public void SetButtonName(string name)
        {
            //this.baseLabelTheta.Text = name;
        }
        #endregion

        #region buttonClickEvent
        private void buttonStop_Click(object sender, EventArgs e)
        {

        }
        #endregion
    }
}
