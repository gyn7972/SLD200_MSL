using QMC.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SLD200_MSL
{
    public partial class FormAlarmLog : FormSubContentBase
    {
        protected DataGridView m_DataGridViewAlarmLog;
        protected AlarmLogOptionControl m_AlarmLogOptionControl;
        protected AlarmInfoControl m_AlarmInfoControl;
        Alarm Alarm { get; set; }
        public FormAlarmLog()
            : base(FormType.withButton.ToString(), "AlarmLog")
        {
            InitializeComponent();

            flowLayoutPanelButton.Visible = false;
            flowLayoutPanelButton.Size = new Size();
            panelContent.Visible = false;
            panelContent.Size = new System.Drawing.Size();
            baseLabelTitle.Visible = false;
            baseLabelTitle.Size = new Size();

            m_AlarmLogOptionControl = new AlarmLogOptionControl();
            m_AlarmLogOptionControl.Location = new Point(Configuration.ContentLocation.X, Configuration.ContentLocation.Y - 20);
            m_AlarmLogOptionControl.SearchClick += SearchClick;
            m_AlarmLogOptionControl.DataSaveClick += SaveDataClick;
            this.Controls.Add(m_AlarmLogOptionControl);


            m_DataGridViewAlarmLog = new DataGridView();
            m_DataGridViewAlarmLog.Size = new Size(Configuration.ContentSize.Width - m_AlarmLogOptionControl.Size.Width - Configuration.ControlGap_2 * 2, m_AlarmLogOptionControl.Size.Height - m_AlarmLogOptionControl.Size.Height / 3);
            m_DataGridViewAlarmLog.AllowUserToResizeRows = false;
            m_DataGridViewAlarmLog.RowHeadersVisible = false;
            m_DataGridViewAlarmLog.ReadOnly = true;
            m_DataGridViewAlarmLog.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            m_DataGridViewAlarmLog.Location = new Point(m_AlarmLogOptionControl.Location.X + m_AlarmLogOptionControl.Size.Width + Configuration.ControlGap, m_AlarmLogOptionControl.Location.Y);
            this.Controls.Add(m_DataGridViewAlarmLog);
            m_AlarmInfoControl = new AlarmInfoControl();
            m_AlarmInfoControl.Location = new Point(m_DataGridViewAlarmLog.Location.X, m_DataGridViewAlarmLog.Location.Y + m_DataGridViewAlarmLog.Size.Height + Configuration.ControlGap);
            this.Controls.Add(m_AlarmInfoControl);
            this.m_DataGridViewAlarmLog.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.baseDataGridViewAlarm_CellClick);
            InitGrid();
        }

        private void baseDataGridViewAlarm_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
            {
                return;
            }
            if(e.RowIndex < m_DataGridViewAlarmLog.Rows.Count-1)
            {
                string strTitle = this.m_DataGridViewAlarmLog["Title", e.RowIndex].Value.ToString();
                string strCause = this.m_DataGridViewAlarmLog["Cause", e.RowIndex].Value.ToString();
                string strCode = this.m_DataGridViewAlarmLog["Code", e.RowIndex].Value.ToString();
                string strGrade = this.m_DataGridViewAlarmLog["Grade", e.RowIndex].Value.ToString();
                string strSource = this.m_DataGridViewAlarmLog["Source", e.RowIndex].Value.ToString();
                m_AlarmInfoControl.SetInfo(strTitle, strCause, strCode, strGrade, strSource);
            }

        }

        protected void SetData(List<Alarm> listAlarm)
        {
            m_DataGridViewAlarmLog.DataSource = null;
            //m_DataGridViewAlarmLog.DataSource = datas;
            if (listAlarm.Count != 0)
            {
                for (int i = 0; i < listAlarm.Count; i++)
                {
                    m_DataGridViewAlarmLog.Rows.Add();

                    this.m_DataGridViewAlarmLog["Date", i].Value = listAlarm[i].GeneratedTime;
                    this.m_DataGridViewAlarmLog["Title", i].Value = listAlarm[i].Title;
                    this.m_DataGridViewAlarmLog["Grade", i].Value = listAlarm[i].Grade;
                    this.m_DataGridViewAlarmLog["Source", i].Value = listAlarm[i].Source;
                    this.m_DataGridViewAlarmLog["Cause", i].Value = listAlarm[i].Cause;
                    this.m_DataGridViewAlarmLog["Code", i].Value = listAlarm[i].Code;
                }
            }
        }
        protected object GetCurrentData()
        {
            return m_DataGridViewAlarmLog.DataSource;
        }
        protected void SearchClick(DateTime startTime, DateTime endTime)
        {
            OnSearchClick(startTime, endTime);
        }

        protected void OnSearchClick(DateTime startTime, DateTime endTime)
        {
            AlarmSaver alarmSaver = AlarmManager.Instance.Saver;
            List<Alarm> Alarms = alarmSaver.GetAlarms(startTime, endTime);
            SetData(Alarms);
        }

        protected void SaveDataClick()
        {
            OnSaveDataClick();
        }

        protected void OnSaveDataClick()
        {
            //List<Alarm> datas = this.GetCurrentData() as List<Alarm>;
            //SaveFileDialog dialog = new SaveFileDialog();
            //dialog.InitialDirectory = Environment.SpecialFolder.Desktop.ToString();
            //dialog.DefaultExt = "csv";
            //dialog.Filter = "CSV files(*.csv)|*.csv";

            //if (dialog.ShowDialog() == DialogResult.OK)
            //{
            //    string path = dialog.FileName;

            //    using (StreamWriter sw = new StreamWriter(path, false, Encoding.Default))
            //    {
            //        sw.WriteLine("Date,Location NO,CarrierID,UnitID,Force,Depth");

            //        foreach (var data in datas)
            //        {
            //            sw.WriteLine(data.SaveCSVFormat());
            //        }
            //    }
            //}
        }
        public void InitGrid()
        {
            m_DataGridViewAlarmLog.AutoGenerateColumns = false;
            m_DataGridViewAlarmLog.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            m_DataGridViewAlarmLog.ReadOnly = true;
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.Name = "Date";
                column.HeaderText = "Date";
                column.DataPropertyName = "Date";
                m_DataGridViewAlarmLog.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.Name = "Title";
                column.HeaderText = "Title";
                column.DataPropertyName = "Title";
                m_DataGridViewAlarmLog.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.Name = "Grade";
                column.HeaderText = "Grade";
                column.DataPropertyName = "Grade";
                m_DataGridViewAlarmLog.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.Name = "Source";
                column.HeaderText = "Source";
                column.DataPropertyName = "Source";
                m_DataGridViewAlarmLog.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.Name = "Cause";
                column.HeaderText = "Cause";
                column.DataPropertyName = "Cause";
                m_DataGridViewAlarmLog.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.Name = "Code";
                column.HeaderText = "Code";
                column.DataPropertyName = "Code";
                m_DataGridViewAlarmLog.Columns.Add(column);
            }

        }
        private void UpdateGridView(List <Alarm> listAlarm)
        {
            
        }

    }
}
