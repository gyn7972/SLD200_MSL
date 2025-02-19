using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SLD200_MSL
{
    public partial class NormalDataConfigControl : UserControl
    {
        public NormalDataConfigControl()
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

        public void SetGroupBoxSize(int nWitdh, int nHight)
        {
            baseGroupBoxMain.Size = new Size(nWitdh, nHight);
        }

        public void SetDataGridSize(int nWitdh, int nHight)
        {
            basePropertyGrid.Size = new Size(nWitdh, nHight);
        }
    }
}
