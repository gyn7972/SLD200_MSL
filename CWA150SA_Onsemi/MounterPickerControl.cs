using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QMC.Common;
namespace CWA150SA_Onsemi300
{
    public partial class MounterPickerControl : UserControl
    {
        public MounterPickerControl(Part part)
        {
            InitializeComponent();
        }

        public void SetGroupboxName(string Name)
        {
            GroupBox.Text = Name;
        }

        private void baseButtonVacuum_Click(object sender, EventArgs e)
        {

        }

        private void baseButtonBlow_Click(object sender, EventArgs e)
        {

        }
    }
}
