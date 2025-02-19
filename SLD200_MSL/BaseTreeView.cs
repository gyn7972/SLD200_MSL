using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SLD200_MSL
{
    public class BaseTreeView : TreeView
    {
        private Font font = new Font("Tahoma",16f,FontStyle.Regular);
        public BaseTreeView()
        {
            this.BackColor = Color.FromArgb(200,200,200);
            this.ForeColor = Color.FromArgb(3, 3, 3);

            this.Font = font;


        }
    }
}
