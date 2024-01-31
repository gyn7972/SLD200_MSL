using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CWA150SA_Onsemi300
{
    public class BaseButton : Button
    {
        FormBaseConfiguration Configuration { get; set; }
        public BaseButton()
        {
            Configuration = new FormBaseConfiguration();    
            this.BackColor = Color.FromArgb(78,78,78);
            this.FlatStyle = FlatStyle.Flat;
            this.ForeColor = Color.WhiteSmoke;
            this.Size = Configuration.ButtonSize;
           // this.FlatStyle = FlatStyle.Standard;
            //this.Click += BaseButton_Click;
        }

        private void BaseButton_Click(object sender, EventArgs e)
        {
            BaseButton baseButton = sender as BaseButton;
            this.BackColor = Color.FromArgb(78, 78, 78);
            this.FlatStyle = FlatStyle.Flat;
            this.ForeColor = Color.WhiteSmoke;
            baseButton.BackColor = Color.FromArgb(200,200,200);
            
        }
       
        
    }

    
}
