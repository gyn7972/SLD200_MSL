using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CWA150SA_Onsemi300
{
    public class BaseTextBox : TextBox
    {
        public BaseTextBox()
        {
            this.BackColor = System.Drawing.Color.FromArgb(78, 78, 78);
            this.ForeColor = System.Drawing.Color.White;
            this.BorderStyle = BorderStyle.None;

            this.Height = 20;
            this.KeyPress += BaseTextBox_KeyPress;
        }

        private void BaseTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back) || e.KeyChar == '.'))
            //{
            //    e.Handled = true;
            //}
        }
    }
}
