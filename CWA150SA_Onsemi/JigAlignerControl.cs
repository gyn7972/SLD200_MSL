using QMC.Common;
using QMC.Common.Parts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CWA150SA_Onsemi
{
    #region Define
    public delegate void RunButtonEventHandler(out double result);
    #endregion

    public partial class JigAlignerControl : UserControl
    {
        #region Define
        public event RunButtonEventHandler RunButtonClick;
        #endregion

        #region Field
        private JigAligner m_Owner;
        #endregion

        #region Constructor
        public JigAlignerControl(Part part)
        {
            InitializeComponent();

            m_Owner = part as JigAligner;
        }
        #endregion

        #region Event Handler
        private void baseButtonRun_Click(object sender, EventArgs e)
        {
            double result = 0.0;

            if (RunButtonClick != null)
            {
                RunButtonClick(out result);
                this.baseTextBoxResult.Text = result.ToString();
            }
        }

        public void SetGroupBoxName(string name)
        {
            if (baseGroupBox1 == null)
                return;

            baseGroupBox1.Text = name;
        }
        #endregion
    }
}
