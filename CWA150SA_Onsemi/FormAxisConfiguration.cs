using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using QMC.Common;
using QMC.Common.Modules;
using QMC.Common.Motion.Ajin.Motions;
using ACS.SPiiPlusNET;
using QMC.Common.Motion.ACS.Motions;

namespace CWA150SA_Onsemi300
{
    #region FormAxisConfiguration
    public partial class FormAxisConfiguration : FormSubContentBase
    {
        #region Define
        static WaferProbeAlign waferProbeAlign;
        private MotorStates m_nMotorState;

        public Size m_imagesize = new Size(30, 25);
        public int m_nServoStatusIndex = 0;
        public int m_nAlarmIndex = 0;
        public enum AxisConfigurationColumnName
        {
            Name,
            Description_AxisNo,
            MotionType,            
            No,
            //AxisNo,
            //BoardNo,
            Simulated,
            ServoStatus,
            ServoAlarm,
            ServoOnOff,
            AlarmClear,
            Initial
        }
        #endregion

        #region Field
        private MotionAxisConfigurationCollection m_AxisConfigurationColletion;
        public MotionAxisConfiguration m_AxisConfigurations;
      //  DataGridViewComboBoxColumn m_ComboBoxColumn = new DataGridViewComboBoxColumn();
        #endregion

        #region Constructor
        public FormAxisConfiguration()
            :base(FormType.Content.ToString(), "Axis Configuration")
        {
            InitializeComponent();

            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;

            foreach (Module module in m_collectionModules)
            {
                if (module.Name == "WaferProbeAlign")
                {
                    waferProbeAlign = module as WaferProbeAlign;
                }
            }

            base.flowLayoutPanelButton.Controls.Add(this.buttonSave);
            base.flowLayoutPanelButton.Controls.Add(this.buttonLoad);
            base.flowLayoutPanelButton.Controls.Add(this.buttonDelete);
            base.flowLayoutPanelButton.Controls.Add(this.buttonNew);

            base.panelContent.Controls.Add(this.dataGridView1);
            base.panelContent.Controls.Add(this.propertyGrid);

            this.panelContent.Location = new Point(Configuration.ContentLocation.X + 10, this.Configuration.PanelSize.Height * 2);
            this.panelContent.Size = new Size(Configuration.MainSize.Width - this.panelContent.Location.X, Configuration.FormContentSize.Height - Configuration.PanelSize.Height * 2);
            this.flowLayoutPanelButton.FlowDirection = FlowDirection.RightToLeft;
          //  this.flowLayoutPanelButton.Size = Configuration.PanelbuttonSize;

            this.buttonDelete.Size = new System.Drawing.Size(Configuration.ButtonSize.Width, Configuration.ButtonSize.Height);
            this.buttonLoad.Size = new System.Drawing.Size(Configuration.ButtonSize.Width, Configuration.ButtonSize.Height);
            this.buttonSave.Size = new System.Drawing.Size(Configuration.ButtonSize.Width, Configuration.ButtonSize.Height);
            this.buttonNew.Size = new System.Drawing.Size(Configuration.ButtonSize.Width, Configuration.ButtonSize.Height);

            this.dataGridView1.Location = new System.Drawing.Point(0, this.propertyGrid.Location.Y);
            this.dataGridView1.Size = new System.Drawing.Size(Configuration.AxisConfigGridSize.Width, Configuration.AxisConfigGridSize.Height);

            this.propertyGrid.Location = new System.Drawing.Point(dataGridView1.Location.X + dataGridView1.Size.Width + Configuration.ControlGap, dataGridView1.Location.Y);
            this.propertyGrid.Size = new Size(Configuration.AxisConfigPropertySize.Width, Configuration.AxisConfigPropertySize.Height);

            DataGridViewColumnCreate();

            this.dataGridView1.DataError += DataGridView1_DataError;

            AjinAxlAxisConfiguration AxisConfiguration = new AjinAxlAxisConfiguration();
            propertyGrid.SelectedObject = AxisConfiguration;
            LoadAxisConfiguration();
        }

        private void DataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {

            }
            catch(Exception ex)
            {
                Log.Write("DataGridView Error", string.Format($"{ex.Message}"));
            }
        }
        #endregion

        #region Property
        public MotionAxisConfigurationCollection AxisConfigurationColletion
        {
            get { return this.m_AxisConfigurationColletion; }
            set { this.m_AxisConfigurationColletion = value; }
        }

        #endregion

        #region Method
        public void DataGridViewColumnCreate()
        {

            foreach (AxisConfigurationColumnName item in Enum.GetValues(typeof(AxisConfigurationColumnName)))
            {
                if (item == AxisConfigurationColumnName.MotionType)
                {
                    DataGridViewComboBoxColumn ComboBoxColumn = new DataGridViewComboBoxColumn();
                    ComboBoxColumn.HeaderText = item.ToString();
                    ComboBoxColumn.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                    ComboBoxColumn.Name = item.ToString();
                    ComboBoxColumn.DefaultCellStyle.BackColor = Color.FromArgb(200, 200, 200);
                    ComboBoxColumn.FlatStyle = FlatStyle.Flat;
                
                    foreach (AjinAxlMotionType AjinAxlMotionType in Enum.GetValues(typeof(AjinAxlMotionType)))
                    {
                        ComboBoxColumn.Items.Add(AjinAxlMotionType.ToString());
                    }
                    dataGridView1.Columns.Add(ComboBoxColumn);
                }               
                else if (item == AxisConfigurationColumnName.Simulated)
                {
                    DataGridViewCheckBoxColumn CheckBoxColumn = new DataGridViewCheckBoxColumn();
                    CheckBoxColumn.HeaderText = item.ToString();
                    CheckBoxColumn.Name = item.ToString();
                    //ComboBoxColumn.FlatStyle = FlatStyle.Standard;
                    //ComboBoxColumn.DataGridView.BackgroundColor = Color.FromArgb(89, 89, 89);
                    dataGridView1.Columns.Add(CheckBoxColumn);
                }
                else if (item == AxisConfigurationColumnName.ServoStatus)
                {
                    DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
                    Image img = CWA150SA_Onsemi.Properties.Resources.AlarmEmpty;
                    Bitmap imgbitmap = new Bitmap(img);
                    img = resizeImage(imgbitmap, m_imagesize);
                    imageColumn.Image = img;
                    imageColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    imageColumn.Resizable = DataGridViewTriState.False;
                    imageColumn.Name = "ServoStatus";
                    imageColumn.HeaderText = "Servo";
                    imageColumn.Width = 30;
                    
                    m_nServoStatusIndex = dataGridView1.Columns.Add(imageColumn);
                }
                else if (item == AxisConfigurationColumnName.ServoAlarm)
                {
                    DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
                    Image img = CWA150SA_Onsemi.Properties.Resources.AlarmEmpty;
                    Bitmap imgbitmap = new Bitmap(img);
                    img = resizeImage(imgbitmap, m_imagesize);
                    imageColumn.Image = img;
                    imageColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    imageColumn.Resizable = DataGridViewTriState.False;
                    imageColumn.Name = "ServoAlarm";
                    imageColumn.HeaderText = "Alarm";
                    imageColumn.Width = 30;
                    m_nAlarmIndex = dataGridView1.Columns.Add(imageColumn);
                }
                else if (item == AxisConfigurationColumnName.ServoOnOff)
                {
                    DataGridViewColumn column = new DataGridViewTextBoxColumn();
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    column.Name = "ServoOnOff";
                    column.HeaderText = "Servo";
                    column.Width = 60;
                    dataGridView1.Columns.Add(column);
                }
                else if (item == AxisConfigurationColumnName.AlarmClear)
                {
                    DataGridViewColumn column = new DataGridViewTextBoxColumn();
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    column.Name = "AlarmClear";
                    column.HeaderText = "Alarm";
                    column.Width = 60;
                    dataGridView1.Columns.Add(column);
                }
                else if (item == AxisConfigurationColumnName.Initial)
                {
                    DataGridViewColumn column = new DataGridViewTextBoxColumn();
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    column.Name = "Initialize";
                    column.HeaderText = "Initialize";
                    column.Width = 60;
                    dataGridView1.Columns.Add(column);
                }
                else
                {
                    this.dataGridView1.DefaultCellStyle.BackColor = Color.FromArgb(200, 200, 200);
                    this.dataGridView1.BackgroundColor = Color.FromArgb(78, 78, 78);
                    this.dataGridView1.GridColor = Color.FromArgb(3,3,3);
                    this.dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(78, 78, 78);
                    this.dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(78, 78, 78);
                    this.dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                    this.dataGridView1.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                    this.dataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(78, 78, 78);
                    this.dataGridView1.RowHeadersDefaultCellStyle.ForeColor = Color.FromArgb(78, 78, 78);
                    dataGridView1.Columns.Add(item.ToString(), item.ToString());
                    
                }
            }
        }

        public void UpdateGridView(int nRow)
        {

            this.dataGridView1[(int)AxisConfigurationColumnName.Name, nRow].Value = this.AxisConfigurationColletion[nRow].Name;
          //  this.dataGridView1[(int)AxisConfigurationColumnName.BoardNo, nRow].Value = this.AxisConfigurationColletion[nRow].BoardNo;
            this.dataGridView1[(int)AxisConfigurationColumnName.No, nRow].Value = this.AxisConfigurationColletion[nRow].No.ToString();
            //this.dataGridView1[(int)AxisConfigurationColumnName.AxisNo, nRow].Value = this.AxisConfigurationColletion[nRow].AxisNo.ToString();
            this.dataGridView1[(int)AxisConfigurationColumnName.Description_AxisNo, nRow].Value = this.AxisConfigurationColletion[nRow].Description;
            this.dataGridView1[(int)AxisConfigurationColumnName.Simulated, nRow].Value = Convert.ToBoolean(this.AxisConfigurationColletion[nRow].Simulated);
            this.dataGridView1[(int)AxisConfigurationColumnName.MotionType, nRow].Value = this.AxisConfigurationColletion[nRow].BoardType.ToString();
            DataGridViewButtonCell ServoOnOff = new DataGridViewButtonCell();
            ServoOnOff.Value = "On/Off";
            this.dataGridView1.Rows[nRow].Cells[7] = ServoOnOff;
            //this.dataGridView1.Rows[nRow].Cells[8] = ServoOnOff;

            DataGridViewButtonCell AlarmClear = new DataGridViewButtonCell();
            AlarmClear.Value = "Clear";
            this.dataGridView1.Rows[nRow].Cells[8] = AlarmClear;
            //this.dataGridView1.Rows[nRow].Cells[9] = AlarmClear;

            DataGridViewButtonCell Initialize= new DataGridViewButtonCell();
            Initialize.Value = "Initialize";
            this.dataGridView1.Rows[nRow].Cells[9] = Initialize;
            //this.dataGridView1.Rows[nRow].Cells[10] = Initialize;

        }


        private void LoadAxisConfiguration()
        {
            dataGridView1.Rows.Clear();
            AxisConfigurationColletion = Equipment.GetAxisConfigurationList();
            foreach (MotionAxisConfiguration config in AxisConfigurationColletion)
            {
                int nRow = dataGridView1.Rows.Add();
                UpdateGridView(nRow);
            }
        }
     
        #endregion

        #region Event Handler

        #region Save
        private void buttonSave_Click(object sender, EventArgs e)
        {
            Equipment.SaveMotionAxis(AxisConfigurationColletion);
            //Equipment.GetAxi
            

        }

        #endregion

        #region NewRow
        private void buttonNew_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add();
            AjinAxlAxisConfiguration AxisConfiguration = new AjinAxlAxisConfiguration();
            AxisConfiguration.UID = Equipment.GetAxisUID();
            // AxisConfiguration.GetDeepCopy();
            this.AxisConfigurationColletion.Add(AxisConfiguration);
        }

        #endregion

        #region Delete
        private void buttonDelete_Click(object sender, EventArgs e)
        {          
            if (dataGridView1.RowCount > 0 && dataGridView1.SelectedRows != null)
            {
                int rowindex = dataGridView1.SelectedCells[0].RowIndex;
                this.AxisConfigurationColletion.RemoveAt(rowindex);
                dataGridView1.Rows.Remove(dataGridView1.Rows[rowindex]);
            }
        }

        #endregion

        #region Load
        private void buttonLoad_Click(object sender, EventArgs e)
        {
            Equipment.LoadMotionBoards();
            LoadAxisConfiguration();
        }

        #endregion

        #region CellChanges

        #region CellValueChanged
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (AxisConfigurationColletion.Count > 0)
            {
                object ChangeData = dataGridView1[e.ColumnIndex, e.RowIndex].Value;
                MotionAxisConfiguration Axis = AxisConfigurationColletion[e.RowIndex];

                switch ((AxisConfigurationColumnName)e.ColumnIndex)
                {
                    case AxisConfigurationColumnName.MotionType: 
                        AxisConfigurationColletion[e.RowIndex].BoardType = (MotionBoardType)Enum.Parse(typeof(MotionBoardType),(string)ChangeData);
                        propertyGrid.SelectedObject = Axis;
                        break;
                    //case AxisConfigurationColumnName.BoardNo:
                    //    AxisConfigurationColletion[e.RowIndex].BoardNo = (int)ChangeData;
                    //    break;
                    case AxisConfigurationColumnName.Description_AxisNo:
                        AxisConfigurationColletion[e.RowIndex].Description = (string)ChangeData;
                        propertyGrid.SelectedObject = Axis;
                        break;
                    case AxisConfigurationColumnName.Name:
                        AxisConfigurationColletion[e.RowIndex].Name = (string)ChangeData;
                        propertyGrid.SelectedObject = Axis;
                        break;
                    case AxisConfigurationColumnName.No:
                        AxisConfigurationColletion[e.RowIndex].No = int.Parse((string)ChangeData);
                        propertyGrid.SelectedObject = Axis;
                        break;
                    //case AxisConfigurationColumnName.AxisNo:
                    //    AxisConfigurationColletion[e.RowIndex].AxisNo = int.Parse((string)ChangeData);
                    //    propertyGrid.SelectedObject = Axis;
                    //    break;
                    case AxisConfigurationColumnName.Simulated:
                        AxisConfigurationColletion[e.RowIndex].Simulated = Convert.ToBoolean(ChangeData);
                        propertyGrid.SelectedObject = Axis;
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        #region CurrentCellChanged
        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int RowIndex = 0;
                RowIndex = dataGridView1.SelectedCells[0].RowIndex;
                if (AxisConfigurationColletion.Count <= RowIndex)
                {
                    //RowIndex = AxisConfigurationColletion.Count - 1;
                    return;
                }
                if (RowIndex >= 0)
                {
                    m_AxisConfigurations = AxisConfigurationColletion[RowIndex];
                    propertyGrid.SelectedObject = m_AxisConfigurations;
                }
            }
        }
        #endregion

        #endregion

        #endregion

        #region Form Members

        #endregion

        #region PropertyValueChanged

        private void propertyGrid_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            string strtitle = e.ChangedItem.Label;
            if (dataGridView1.RowCount == 0)
            {
                AxisConfigurationColletion = new MotionAxisConfigurationCollection();
                return;
            }
            //if(e.ChangedItem.Label == dataGridViewMotionBoard[)
            PropertyGrid propertyGrid = sender as PropertyGrid;
            if (propertyGrid != null)
            {
                AjinAxlAxisConfiguration configuration = propertyGrid.SelectedObject as AjinAxlAxisConfiguration;
                int nRow = dataGridView1.SelectedCells[0].RowIndex;
                UpdateGridView(nRow);
            }
        }
        public static Image resizeImage(Image image, Size size)
        {
            return (Image)new Bitmap(image, size);
        }

        public void ClickedServoOnOffButton(int nAxisNo)
        {
            MotionAxis axis = Equipment.GetAxis(nAxisNo);
            bool bEnable = false;

            if (axis != null)
            {
                axis.GetEnable(ref bEnable);
                if (bEnable == false)
                {
                    axis.SetEnable(true);
                }
                else
                {
                    axis.SetEnable(false);
                }
            }
        }
        public void ClickedServoAlarmClearButton(int nAixsNo)
        {
            MotionAxis axis = Equipment.GetAxis(nAixsNo);
            if (axis != null)
            {
                axis.Reset();
            }
        }
        public void ClickedServoInitializeButton(int nAixsNo)
        {
            MotionAxis axis = Equipment.GetAxis(nAixsNo);
            if (axis != null)
            {
                axis.Initialize();
            }
        }
        #endregion

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            if (senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewButtonCell &&
                e.RowIndex >= 0)
            {
                //int nAxisNo = Convert.ToInt32(this.dataGridView1[(int)AxisConfigurationColumnName.No, e.RowIndex].Value);
                int nAxisNo = Convert.ToInt32(this.dataGridView1[(int)AxisConfigurationColumnName.Description_AxisNo, e.RowIndex].Value);
                if (senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "On/Off")
                {
                    ClickedServoOnOffButton(nAxisNo);
                }
                else if (senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Clear")
                {
                    ClickedServoAlarmClearButton(nAxisNo);
                }
                else if (senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Initialize")
                {
                    ClickedServoInitializeButton(nAxisNo);
                }
                else { }
            }
        }

        private void timerServoStatus_Tick(object sender, EventArgs e)
        {
            //return;
            timerServoStatus.Stop();
            foreach (MotionAxisConfiguration axisConfig in m_AxisConfigurationColletion)
            {
                MotionAxis axis = Equipment.GetAxis(axisConfig.No);

                if(axis == null)
                {
                    Console.WriteLine("Axis is Null!!!");
                }
                else
                {
                    bool bAmp = true;
                    bool bEnable = false;

                    if (axis.Board.Configuration.BoardType == MotionBoardType.Ajin)
                    {
                        axis.GetAmpFault(ref bAmp);
                        if (bAmp)
                        {
                            Image img = CWA150SA_Onsemi.Properties.Resources.AlarmError;
                            Bitmap imgbitmap = new Bitmap(img);
                            img = resizeImage(imgbitmap, m_imagesize);
                            dataGridView1.Rows[(int)axisConfig.No].Cells[6].Value = img;
                        }
                        else
                        {
                            Image img = CWA150SA_Onsemi.Properties.Resources.AlarmEmpty;
                            Bitmap imgbitmap = new Bitmap(img);
                            img = resizeImage(imgbitmap, m_imagesize);
                            dataGridView1.Rows[(int)axisConfig.No].Cells[6].Value = img;
                        }

                        axis.GetEnable(ref bEnable);
                        if (bEnable == false)
                        {

                            Image img = CWA150SA_Onsemi.Properties.Resources.AlarmEmpty;
                            Bitmap imgbitmap = new Bitmap(img);
                            img = resizeImage(imgbitmap, m_imagesize);
                            dataGridView1.Rows[(int)axisConfig.No].Cells[5].Value = img;
                        }
                        else
                        {
                            Image img = CWA150SA_Onsemi.Properties.Resources.AlarmInform;
                            Bitmap imgbitmap = new Bitmap(img);
                            img = resizeImage(imgbitmap, m_imagesize);
                            dataGridView1.Rows[(int)axisConfig.No].Cells[5].Value = img;
                        }
                    }
                    //else if (ACSSPiiPlusMotionBoard.Api.IsConnected && (axis.Board.Configuration.BoardType == MotionBoardType.ACS))
                    else if (ACSSPiiPlusMotionBoard.Api.IsConnected && (axis.Board.Configuration.BoardType == MotionBoardType.ACS))
                    {
                        SafetyControlMasks m_safetyControlMask;
                        //m_safetyControlMask = ACSSPiiPlusMotionBoard.Api.GetFault((Axis)axis.No);
                        m_safetyControlMask = ACSSPiiPlusMotionBoard.Api.GetFault((Axis)axis.No);

                        if (m_safetyControlMask == SafetyControlMasks.ACSC_SAFETY_DRIVE)
                        {
                            Image img = CWA150SA_Onsemi.Properties.Resources.AlarmError;
                            Bitmap imgbitmap = new Bitmap(img);
                            img = resizeImage(imgbitmap, m_imagesize);
                            dataGridView1.Rows[(int)axisConfig.No].Cells[6].Value = img;
                        }
                        else
                        {
                            Image img = CWA150SA_Onsemi.Properties.Resources.AlarmEmpty;
                            Bitmap imgbitmap = new Bitmap(img);
                            img = resizeImage(imgbitmap, m_imagesize);
                            dataGridView1.Rows[(int)axisConfig.No].Cells[6].Value = img;
                        }

                        //m_nMotorState = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)axis.No);
                        m_nMotorState = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)axis.No);

                        // Returned value is integer, you need to use bitmaks 
                        //if ((m_nMotorState & MotorStates.ACSC_MST_MOVE) != 0) lblMoving.Image = Properties.Resources.On; else lblMoving.Image = Properties.Resources.Off;
                        //if ((m_nMotorState & MotorStates.ACSC_MST_INPOS) != 0) lblInPos.Image = Properties.Resources.On; else lblInPos.Image = Properties.Resources.Off;
                        //if ((m_nMotorState & MotorStates.ACSC_MST_ACC) != 0) lblAcc.Image = Properties.Resources.On; else lblAcc.Image = Properties.Resources.Off;
                        if ((m_nMotorState & MotorStates.ACSC_MST_ENABLE) != 0)
                        {
                            Image img = CWA150SA_Onsemi.Properties.Resources.AlarmInform;
                            Bitmap imgbitmap = new Bitmap(img);
                            img = resizeImage(imgbitmap, m_imagesize);
                            dataGridView1.Rows[(int)axisConfig.No].Cells[5].Value = img;
                        }
                        else
                        {
                            Image img = CWA150SA_Onsemi.Properties.Resources.AlarmEmpty;
                            Bitmap imgbitmap = new Bitmap(img);
                            img = resizeImage(imgbitmap, m_imagesize);
                            dataGridView1.Rows[(int)axisConfig.No].Cells[5].Value = img;
                        }
                    }
                }
            }
            timerServoStatus.Start();
        }
    }
    #endregion
}
