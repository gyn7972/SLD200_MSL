using netDxf.Entities;
using QMC.Common;
using QMC.Common.Modules;
using QMC.Common.Motion.ACS.Motions;
using QMC.Common.Parts;
using QMC.Common.Vision.Optics;
using QMC.Common.Vision.Tools;
using QMC.Common.VisionPart;
using QMC.Core;
using SLD200_MSL;
using SpiralLab.Sirius;
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
using static QMC.Common.Equipment;
using static QMC.Common.Modules.Vision;
using static QMC.Common.Modules.WorkStage;
using Bitmap = System.Drawing.Bitmap;
using Image = System.Drawing.Image;
using Rectangle = System.Drawing.Rectangle;

namespace SLD200_MSL
{
    public partial class FormNew_Alarm : Form
    {        
        public AlarmCollection Alarms { get; set; }
        Alarm Alarm { get; set; }

        private Size ConfirmButton = new Size(220, 130);
        protected Size m_imagesize = new Size(30, 25);

        protected BaseButton m_BaseButton;

        public string m_path = "";

        //  2025. 04. 27.  SCH : 아래 코드가 원래 코드인데, 현재 디렉토리의 상위 디렉토리가 루트에 가까우면 null 이 반환될 수 있다. 그래서 생성자에서 예외처리해줌. 
        //public string m_path = System.IO.Directory.GetParent(System.Environment.CurrentDirectory).Parent.FullName;               

        public FormNew_Alarm()
        {
            InitializeComponent();


            // 현재 디렉터리 가져오기
            var currentDirectory = System.Environment.CurrentDirectory;

            // 상위 디렉터리 확인 및 처리
            var parentDirectory = System.IO.Directory.GetParent(currentDirectory)?.Parent;

            if (parentDirectory != null)
            {
                m_path = parentDirectory.FullName; // 상위 디렉터리의 상위 디렉터리 경로
            }
            else
            {
                m_path = currentDirectory; // 기본값으로 현재 디렉터리 경로 사용
            }


            this.StartPosition = FormStartPosition.CenterScreen;

            this.VisibleChanged += FormNew_Alarm_VisibleChanged;
        }

        private void FormNew_Alarm_Load(object sender, EventArgs e)
        {
            InitDataGridViewColumn();            

            if (Alarms != null && Alarms.Count > 0)
            {
                baseDataGridViewAlarm.DataSource = null;
                baseDataGridViewAlarm.DataSource = Alarms;
                BaseButton baseButton = new BaseButton();
                baseButton.Text = "Comfirm";
                baseButton.Size = ConfirmButton;
                baseButton.TextAlign = ContentAlignment.MiddleCenter;
                baseButton.FlatStyle = FlatStyle.Flat;
                baseButton.Click += ButtonComfirm_Click;

                this.panelComfirm.Controls.Add(baseButton);
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
                imageColumn.Width = 130;
                baseDataGridViewAlarm.Columns.Add(imageColumn);
            }

            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "GeneratedTime";
                column.Name = "Generated Time";
                column.Width = 250;
                baseDataGridViewAlarm.Columns.Add(column);
            }

            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Source";
                column.Name = "Source";
                column.Width = 250;
                baseDataGridViewAlarm.Columns.Add(column);
            }

            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Grade";
                column.Name = "Grade";
                column.Width = 200;
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

        private void ButtonComfirm_Click(object sender, EventArgs e)
        {
            if (Alarms != null && Alarms.Count > 0)
            {
                if (baseDataGridViewAlarm.SelectedCells != null)
                {
                    try
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
                    }catch(Exception ex)
                    {
                        Log.Write(ex);
                    }
                    
                    //알람 지우기
                }
            }
        }

        private void FormNew_Alarm_VisibleChanged(object sender, EventArgs e)
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
                        Image img = SLD200.Properties.Resources.AlarmInform;
                        Bitmap imgbitmap = new Bitmap(img);
                        img = FormMaintDigitalIO.resizeImage(imgbitmap, m_imagesize);
                        Alarms[i].StateImage = img;
                    }
                    if (Alarms[i].Grade == Alarm.AlarmType.Error.ToString())
                    {
                        Image img = SLD200.Properties.Resources.AlarmError;
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
                baseButton.Size = ConfirmButton;
                baseButton.TextAlign = ContentAlignment.MiddleCenter;
                baseButton.FlatStyle = FlatStyle.Flat;
                baseButton.Click += ButtonComfirm_Click;

                this.panelComfirm.Controls.Add(baseButton);
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

        private void button_Alarm_Buzz_Off_Click(object sender, EventArgs e)
        {
            CommonModule.Instance.TowerLamp.Buzzer_Off();
        }
    }
}
