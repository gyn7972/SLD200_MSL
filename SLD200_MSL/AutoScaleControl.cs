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
    public delegate void AutoScaleButtonEventHandler();
    public partial class AutoScaleControl : UserControl
    {
        public event AutoScaleButtonEventHandler AutoScaleButtonClick;
        public double X { get; set; }
        public double Y { get; set; }
        public AutoScaleControl()
        {
            InitializeComponent();
            SetScaleValue();
        }

        private void baseButtonAlign_Click(object sender, EventArgs e)
        {
            if(AutoScaleButtonClick != null)
            {
                AutoScaleButtonClick();
            }
        }
        private void SetScaleValue()
        {
            if (X != null & Y != null)
            {
                this.baseTextBoxX.Text = X.ToString();
                this.baseTextBoxY.Text = Y.ToString();
            }
        }
        public void UpdateScaleValue(double dX, double dY)
        {
            X = dX;
            Y = dY;
            this.baseTextBoxX.Text = X.ToString();
            this.baseTextBoxY.Text = Y.ToString();

        }
    }
}
