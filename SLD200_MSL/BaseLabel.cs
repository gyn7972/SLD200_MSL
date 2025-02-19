using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SLD200_MSL
{
    public class BaseLabel : Label
    {
        private Font font = new Font("Tahoma", 12f, FontStyle.Bold);
        
        public BaseLabel()
        {
            this.ForeColor = System.Drawing.Color.White;
            this.Font = font;
        }
    }
}
