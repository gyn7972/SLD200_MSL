using QMC.Common.Opticon;
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
    public partial class BarcodeControl : UserControl
    {
        #region Field
        public OpticonBarcodeReader m_Owner;
        #endregion

        #region Constructor
        public BarcodeControl(OpticonBarcodeReader opticonBarcodeReader)
        {
            InitializeComponent();
            m_Owner = opticonBarcodeReader;
        }
        #endregion

        #region Method
        public void SetGroupBox(string name)
        {
            this.baseGroupBoxBarcode.Text = name;
        }
        #endregion

        #region Event Handler
        private void baseButtonRead_Click(object sender, EventArgs e)
        {
            this.baseTextBoxResult.ResetText();
            if (m_Owner == null)
            {
                this.baseTextBoxResult.Text = "Owner is Null";
                return;
            }

            string result = string.Empty;

            m_Owner.Read(out result);
            if (result == String.Empty)
            {
                this.baseTextBoxResult.Text = "Result is Null";
            }
            else
            {
                this.baseTextBoxResult.Text = result;
            }
        }
        #endregion
    }
}
