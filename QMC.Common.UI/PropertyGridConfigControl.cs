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
    public partial class PropertyGridConfigControl : UserControl
    {
        public PropertyGridConfigControl()
        {
            InitializeComponent();
        }
        public void SetGroupBoxName(string strName)
        {
            baseGroupBoxMain.Text = strName;
        }
        public void SetData(object data)
        {
            basePropertyGrid.SelectedObject = data;
        }
    }
}
