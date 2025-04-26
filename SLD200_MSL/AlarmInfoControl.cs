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
    public partial class AlarmInfoControl : UserControl
    {
        public AlarmInfoControl()
        {
            InitializeComponent();
            Init();
        }
        private void Init()
        {
            baseTextBoxAlarmTitle.Text = string.Empty;
            baseTextBoxCause.Text = string.Empty;
            baseTextBoxCode.Text = string.Empty;
            baseTextBoxGrade.Text = string.Empty;
            baseTextBoxSource.Text = string.Empty;
        }
        public void SetInfo(string strTitle, string strCause, string strCode, string strGrade, string strSource)
        {
            baseTextBoxAlarmTitle.Text = strTitle;

            baseTextBoxCause.Text = strCause;
            baseTextBoxCode.Text = strCode;
            baseTextBoxGrade.Text = strGrade;
            baseTextBoxSource.Text = strSource;
        }
    }
}
