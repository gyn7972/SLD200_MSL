using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QMC.Common.UI
{
    public class BasePropertyGrid : PropertyGrid
    {

        public BasePropertyGrid()
        {
            this.BackColor = Color.FromArgb(90, 90, 90);
            this.ViewBackColor = Color.FromArgb(200, 200, 200);
            this.ViewBorderColor = Color.FromArgb(90, 90, 90);
            this.CategoryForeColor = Color.Black;
            this.CommandsBackColor = Color.FromArgb(90, 90, 90);
            this.HelpBackColor = Color.FromArgb(90, 90, 90);
            this.SelectedItemWithFocusBackColor = Color.FromArgb(90, 90, 90);
            this.CategorySplitterColor = Color.FromArgb(70, 70, 70);

            this.HelpForeColor = Color.FromArgb(245, 245, 245);
        }
    }
}
