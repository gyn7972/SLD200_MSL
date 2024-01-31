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
    public delegate void AlignButtonEventHandler();
    public partial class ResultAlignControl : UserControl
    {
        public event AlignButtonEventHandler AlignButtonClick;
        public ResultAlignControl()
        {
            InitializeComponent();
        }
        public double AlignResult { get; set; }

        public int AlignNo { set; get; }
        private void baseButtonAlign_Click(object sender, EventArgs e)
        {
            if(AlignButtonClick != null)
            {
                //AlignNo = int.Parse(baseTextBoxAlignNo.Text);
                AlignButtonClick();
                this.baseTextBoxAlign.Text = AlignResult.ToString();
            }
        }
    }
}
