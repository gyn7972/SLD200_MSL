using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CWA150SA_Onsemi300
{
    public partial class SPCOptionControl : UserControl
    {
        public delegate void SearchButtonClick(string strUnitID, DateTime startTime, DateTime endTime);
        public delegate void SaveDataButtonClick();


        #region Constructor
        public SPCOptionControl()
        {
            InitializeComponent();
        }
        #endregion

        public event SearchButtonClick SearchClick;
        public event SaveDataButtonClick DataSaveClick;

        private void baseButtonSearch_Click(object sender, EventArgs e)
        {
            string strUnitID = baseTextBox1.Text;
            DateTime startTime = dateTimePickerStartTime.Value;
            DateTime endTime = dateTimePickerEndTime.Value;
            
            if (SearchClick != null)
                SearchClick(strUnitID, startTime, endTime);
        }

        private void baseButtonSaveData_Click(object sender, EventArgs e)
        {
            if (DataSaveClick != null)
                DataSaveClick();
        }
    }
}
