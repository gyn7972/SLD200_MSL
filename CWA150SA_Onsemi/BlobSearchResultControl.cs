using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CWA150SA_Onsemi
{
    public delegate void SearchButtonClickHandler(Control control);
    
    public partial class BlobSearchResultControl : UserControl
    {
        #region Define
        public event SearchButtonClickHandler SearchClick;
        #endregion

        #region Field

        #endregion

        #region Property
        public bool IsPixel
        {
            get { return this.radioButtonPixel.Checked; }
            set
            {
                if (value == true)
                {
                    this.radioButtonPixel.Checked = value;
                }
                else
                {
                    this.radioButtonPixel.Checked = false;
                }
            }
        }
        #endregion

        #region Constructor
        public BlobSearchResultControl()
        {
            InitializeComponent();

            this.radioButtonPixel.Checked = true;
        }
        #endregion

        #region Method
        public void UpdataPositionData(double x, double y, double area)
        {
            this.baseTextBoxResultX.Text = x.ToString();
            this.baseTextBoxResultY.Text = y.ToString();
            this.baseTextBoxResultArea.Text = area.ToString();

        }

        public void UpdataPositionData(double x, double y)
        {
            this.UpdataPositionData(x, y, 0.0);
        }
        #endregion

        #region Event Handler
        private void baseButtonSearch_Click(object sender, EventArgs e)
        {
            if (this.SearchClick != null)
            {
                this.SearchClick(this);
            }
        }
        #endregion


    }
}
