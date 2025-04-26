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
    public partial class AlarmLogOptionControl : UserControl
    {
        public delegate void SearchButtonClick(DateTime startTime, DateTime endTime);
        public delegate void SaveDataButtonClick();
        public AlarmLogOptionControl()
        {
            InitializeComponent();
        }
        public event SearchButtonClick SearchClick;
        public event SaveDataButtonClick DataSaveClick;

        private void baseButtonSearch_Click(object sender, EventArgs e)
        {
            DateTime startTime = dateTimePickerStartTime.Value;
            DateTime endTime = dateTimePickerEndTime.Value;
            
            if (SearchClick != null)
                SearchClick(startTime, endTime);
        }

        private void baseButtonSaveData_Click(object sender, EventArgs e)
        {
            if (DataSaveClick != null)
                DataSaveClick();
        }

        private void AlarmLogOptionControl_Load(object sender, EventArgs e)
        {

        }
    }
}
