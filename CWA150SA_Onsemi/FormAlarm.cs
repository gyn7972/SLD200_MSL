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
    public partial class FormAlarm : FormSubContentBase
    {
        public AlarmCollection Alarms { get; set; }
        Alarm Alarm { get; set; }

        protected Size m_imagesize = new Size(30, 25);

        //public string m_path = System.IO.Directory.GetParent(System.Environment.CurrentDirectory).Parent.FullName;
        public FormAlarm()
            : base(FormType.withButton.ToString(), "Alarm Viewer")

        {
            InitializeComponent();
            //this.Size

            this.flowLayoutPanelButton.Size = new System.Drawing.Size(0, 0);
            this.baseLabelTitle.Location = new Point((Configuration.ContentSize.Width / 2) - (Configuration.ButtonSize.Width / 2), 0);
            this.panelContent.Location = new Point(Configuration.ContentLocation.X, Configuration.PanelbuttonSize.Height);
            this.panelContent.Size = new Size(Configuration.PanelbuttonSize.Width, Configuration.FormWithbuttonSize.Height);


            this.panelContent.Controls.Add(this.groupBoxSelectedAlarmDetails);
            this.panelContent.Controls.Add(this.baseDataGridViewAlarm);
            this.groupBoxRecovery.Controls.Add(this.panelComfirm);

            this.groupBoxSelectedAlarmDetails.Size = Configuration.AlarmGroupBoxSize;
            this.groupBoxSelectedAlarmDetails.Location = new Point(Configuration.ContentLocation.X, 0);

            this.baseDataGridViewAlarm.Location = new Point(Configuration.ContentLocation.X, this.groupBoxSelectedAlarmDetails.Location.Y + this.groupBoxSelectedAlarmDetails.Size.Height + 5);
            this.baseDataGridViewAlarm.Size = new Size(Configuration.AlarmdataGridViewSize.Width, Configuration.AlarmdataGridViewSize.Height);
            this.VisibleChanged += FormAlarm_VisibleChanged;
        }

        private void FormAlarm_Load(object sender, EventArgs e)
        {
            InitDataGridViewColumn();
            if (Alarms != null && Alarms.Count > 0)
            {
                baseDataGridViewAlarm.DataSource = null;
                baseDataGridViewAlarm.DataSource = Alarms;
                BaseButton baseButton = new BaseButton();
                baseButton.Text = "Comfirm";
                baseButton.Size = Configuration.ButtonSize;
                baseButton.TextAlign = ContentAlignment.MiddleCenter;
                baseButton.FlatStyle = FlatStyle.Flat;
                baseButton.Click += ButtonComfirm_Click;

                this.panelComfirm.Controls.Add(baseButton);
            }
        }
        private void ButtonComfirm_Click(object sender, EventArgs e)
        {
            if (Alarms != null && Alarms.Count > 0)
            {
                if (baseDataGridViewAlarm.SelectedCells != null)
                {
                    Alarm alarm = baseDataGridViewAlarm.Rows[baseDataGridViewAlarm.SelectedCells[0].RowIndex].DataBoundItem as Alarm;
                    foreach (Alarm alarm1 in Alarms)
                    {
                        if (alarm == alarm1)
                        {
                            
                            Alarms.Remove(alarm1);
                            baseDataGridViewAlarm.DataSource = null;
                            baseDataGridViewAlarm.DataSource = Alarms;
                            if (Alarms.Count > 0)
                            {
                                baseDataGridViewAlarm.Rows[0].Cells[1].Selected = true;
                            }
                            break;
                        }
                    }
                    //알람 지우기
                }
            }
        }
        public void InitDataGridViewColumn()
        {
            baseDataGridViewAlarm.Columns.Clear();
            baseDataGridViewAlarm.AutoGenerateColumns = false;
            baseDataGridViewAlarm.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.None);
            //  baseDataGridViewAlarm.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            // 알람이 들어왔을때는 위에꺼
            baseDataGridViewAlarm.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            {
                DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
                imageColumn.DataPropertyName = "StateImage";
                imageColumn.Name = "State";
                imageColumn.Width = 30;
                baseDataGridViewAlarm.Columns.Add(imageColumn);
            }

            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "GeneratedTime";
                column.Name = "GeneratedTime";
                column.Width = 155;
                baseDataGridViewAlarm.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Source";
                column.Name = "Source";
                column.Width = 100;
                baseDataGridViewAlarm.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Grade";
                column.Name = "Grade";
                column.Width = 100;
                baseDataGridViewAlarm.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Title";
                column.Name = "Title";
                column.Width = 1000;
                baseDataGridViewAlarm.Columns.Add(column);
            }
        }
        private void baseDataGridViewAlarm_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //if(e.ColumnIndex <0|| e.RowIndex <0)
            //{
            //    return;
            //}
            //Alarm = baseDataGridViewAlarm.Rows[e.RowIndex].DataBoundItem as Alarm;

            //baseTextBoxAlarmTitle.Text = Alarm.Title;
            //baseTextBoxCause.Text = Alarm.Cause;
            //baseTextBoxCode.Text = Alarm.Code.ToString();
            //baseTextBoxGrade.Text = Alarm.Grade.ToString();//이넘?
            //baseTextBoxSource.Text = Alarm.Source.ToString();

        }
        private void FormAlarm_VisibleChanged(object sender, EventArgs e)
        {
            //InitDataGridViewColumn();
            if (Alarms != null && Alarms.Count > 0)
            {
                baseDataGridViewAlarm.DataSource = null;
                baseDataGridViewAlarm.DataSource = Alarms;
                for (int i = 0; i < Alarms.Count; i++)
                {

                    if (Alarms[i].Grade == Alarm.AlarmType.Inform.ToString())
                    {
                        Image img = CWA150SA_Onsemi.Properties.Resources.AlarmInform;
                        Bitmap imgbitmap = new Bitmap(img);
                        img = FormMaintDigitalIO.resizeImage(imgbitmap, m_imagesize);
                        Alarms[i].StateImage = img;
                    }
                    if (Alarms[i].Grade == Alarm.AlarmType.Error.ToString())
                    {
                        Image img = CWA150SA_Onsemi.Properties.Resources.AlarmError;
                        Bitmap imgbitmap = new Bitmap(img);
                        img = FormMaintDigitalIO.resizeImage(imgbitmap, m_imagesize);
                        Alarms[i].StateImage = img;
                    }
                    /*
                    if (Alarms[i].Grade == Alarm.AlarmType.Warning.ToString())
                    {
                        Image img = Properties.Resources.AlarmWarning;
                        Bitmap imgbitmap = new Bitmap(img);
                        img = FormMaintDigitalIO.resizeImage(imgbitmap, m_imagesize);
                        Alarms[i].StateImage = img;
                    }
                    */
                }

                BaseButton baseButton = new BaseButton();
                baseButton.Text = "Comfirm";
                baseButton.Size = Configuration.ButtonSize;
                baseButton.TextAlign = ContentAlignment.MiddleCenter;
                baseButton.FlatStyle = FlatStyle.Flat;
                baseButton.Click += ButtonComfirm_Click;

                this.panelComfirm.Controls.Add(baseButton);
            }
        }

        private void baseDataGridViewAlarm_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
            {
                return;
            }
            Alarm = baseDataGridViewAlarm.Rows[e.RowIndex].DataBoundItem as Alarm;

            baseTextBoxAlarmTitle.Text = Alarm.Title;
            baseTextBoxCause.Text = Alarm.Cause;
            baseTextBoxCode.Text = Alarm.Code.ToString();
            baseTextBoxGrade.Text = Alarm.Grade.ToString();//이넘?
            baseTextBoxSource.Text = Alarm.Source.ToString();
        }
        private void baseDataGridViewAlarm_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {

            Alarm = baseDataGridViewAlarm.Rows[0].DataBoundItem as Alarm;
            baseTextBoxAlarmTitle.Text = Alarm.Title;
            baseTextBoxCause.Text = Alarm.Cause;
            baseTextBoxCode.Text = Alarm.Code.ToString();
            baseTextBoxGrade.Text = Alarm.Grade.ToString();//이넘?
            baseTextBoxSource.Text = Alarm.Source.ToString();
        }

        private void baseDataGridViewAlarm_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (Alarms.Count == 0)
            {
                baseTextBoxAlarmTitle.Text = "";
                baseTextBoxCause.Text = "";
                baseTextBoxCode.Text = "";
                baseTextBoxGrade.Text = "";//이넘?
                baseTextBoxSource.Text = "";
            }
        }
    }
}
