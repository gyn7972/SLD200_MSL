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
    public delegate void AutoScaleButtonEventHandler();
    public partial class AutoScaleControl : UserControl
    {
        public event AutoScaleButtonEventHandler AutoScaleButtonClick;
        public double X { get; set; }
        public double Y { get; set; }
        public AutoScaleControl()
        {
            InitializeComponent();
            X = 0;
            Y = 0;
            this.baseTextBox1.Text = "0";
            SetScaleValue();
        }

        private void baseButtonAlign_Click(object sender, EventArgs e)
        {
            if (AutoScaleButtonClick != null)
            {
                AutoScaleButtonClick();
            }
        }
        private void SetScaleValue()
        {
            this.baseTextBoxX.Text = X.ToString();
            this.baseTextBoxY.Text = Y.ToString();
        }
        public void UpdateScaleValue(double dX, double dY)
        {
            X = dX;
            Y = dY;
            this.baseTextBoxX.Text = X.ToString();
            this.baseTextBoxY.Text = Y.ToString();
        }

        public double GetPitch()
        {
            double nPitch = 0;

            double.TryParse(this.baseTextBox1.Text, out nPitch);
            return nPitch;
        }
    }
}
