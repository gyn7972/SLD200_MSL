using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SLD200_MSL
{
    public class BaseListBox :ListBox
    {
        private Font font = new Font("Tahoma", 15f, FontStyle.Regular);
        public BaseListBox()
        {
            this.Font = font;
            this.BackColor = Color.FromArgb(200,200,200);
            this.ForeColor = Color.FromArgb(79, 79, 79);
        }
    }
}
