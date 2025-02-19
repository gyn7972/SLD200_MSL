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
    public delegate void AlignButtonEventHandler();
    public partial class ResultAlignControl : UserControl
    {
        public event AlignButtonEventHandler AlignButtonClick;
        public ResultAlignControl()
        {
            InitializeComponent();
        }
        public double AlignResult { get; set; }
        private void baseButtonAlign_Click(object sender, EventArgs e)
        {
            if(AlignButtonClick != null)
            {
                AlignButtonClick();
                this.baseTextBoxAlign.Text = AlignResult.ToString();
            }
        }
    }
}
