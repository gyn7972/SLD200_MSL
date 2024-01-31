using QMC.Common;
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
    public partial class FormAddAlarm : Form
    {
        private Alarm m_Alarm;
        private AlarmCollection m_Alarms;
        private Part m_Part;

        public FormAddAlarm(Part part)
        {
            Part = part;
            //if(Part.Alarms ==null)
            //{
            //    Part.Alarms = new AlarmCollection();
            //}
            //else
            //{
            //    Alarms = Part.Alarms;
            //}
            InitializeComponent();
            this.BackColor = Color.FromArgb(38, 38, 38);
        }
        public Alarm Alarm
        {
            get { return m_Alarm; }
            set { m_Alarm = value; }
        }
        public Part Part
        {
            get { return m_Part; }
            set { m_Part = value; }
        }
        public AlarmCollection Alarms
        {
            get { return m_Alarms; }
            set { m_Alarms = value; }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            Alarm = new Alarm();
            Alarm.Title = baseTextBoxTitle.Text;
            //if(baseTextBoxCode.Text )
            Alarm.Code = int.Parse(baseTextBoxCode.Text);
            Alarm.Grade = baseTextBoxGrade.Text;
            Alarm.Source = baseTextBoxSource.Text;
            Alarm.Cause = baseTextBoxCause.Text;

            //Part.Alarms.Add(Alarm);

            DialogResult = DialogResult.OK;
        }
    }
}
