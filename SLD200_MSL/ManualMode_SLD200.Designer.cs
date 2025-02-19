using System.Drawing;
using System.Windows.Forms;

namespace SLD200_MSL
{
    partial class ManualMode_SLD200
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManualMode_SLD200));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.tabControl_ManualMode = new System.Windows.Forms.TabControl();
            this.tabPage_Laser = new System.Windows.Forms.TabPage();
            this.baseGroupBox_LaserControl = new SLD200_MSL.BaseGroupBox();
            this.button_Laser_Chiller2On = new System.Windows.Forms.Button();
            this.pictureBox_LaserControl_Button_Chiller2On = new System.Windows.Forms.PictureBox();
            this.button_Laser_Chiller1On = new System.Windows.Forms.Button();
            this.pictureBox_LaserControl_Button_Chiller1On = new System.Windows.Forms.PictureBox();
            this.button_Laser_ShutterOpen = new System.Windows.Forms.Button();
            this.pictureBox_LaserControl_Button_ShutterOpen = new System.Windows.Forms.PictureBox();
            this.button_Laser_PowerOn = new System.Windows.Forms.Button();
            this.pictureBox_LaserControl_Button_PowerOn = new System.Windows.Forms.PictureBox();
            this.baseGroupBox_LaserControl_Status = new SLD200_MSL.BaseGroupBox();
            this.baseLabel_Laser_Emission = new SLD200_MSL.BaseLabel();
            this.pictureBox_LaserControl_Status_Emission = new System.Windows.Forms.PictureBox();
            this.baseLabel_Laser_Shutter = new SLD200_MSL.BaseLabel();
            this.pictureBox_LaserControl_Status_Shutter = new System.Windows.Forms.PictureBox();
            this.baseLabel_Laser_Ready = new SLD200_MSL.BaseLabel();
            this.pictureBox_LaserControl_Status_Ready = new System.Windows.Forms.PictureBox();
            this.baseLabel_Laser_Fault = new SLD200_MSL.BaseLabel();
            this.pictureBox_LaserControl_Status_Fault = new System.Windows.Forms.PictureBox();
            this.baseGroupBox_DeviceControl = new SLD200_MSL.BaseGroupBox();
            this.button_Laser_TurnOffAllDevice = new System.Windows.Forms.Button();
            this.button_Laser_TurnOnAllDevice = new System.Windows.Forms.Button();
            this.tabPage_Motor = new System.Windows.Forms.TabPage();
            this.baseGroupBox_SingleRun = new SLD200_MSL.BaseGroupBox();
            this.button_Move_UnloadPos = new System.Windows.Forms.Button();
            this.button_Move_LoadPos = new System.Windows.Forms.Button();
            this.baseGroupBox_CycleRun = new SLD200_MSL.BaseGroupBox();
            this.button_Run_Unloading = new System.Windows.Forms.Button();
            this.button_Run_Loading = new System.Windows.Forms.Button();
            this.button_Run_Align = new System.Windows.Forms.Button();
            this.baseGroupBox_PickerSuction = new SLD200_MSL.BaseGroupBox();
            this.pictureBox_PickerStatus_ULSuction = new System.Windows.Forms.PictureBox();
            this.pictureBox_PickerStatus_LDSuction = new System.Windows.Forms.PictureBox();
            this.button_Picker_ULSuctionOn = new System.Windows.Forms.Button();
            this.pictureBox_PickerSignal_ULSuctionOn = new System.Windows.Forms.PictureBox();
            this.button_Picker_LDSuctionOn = new System.Windows.Forms.Button();
            this.pictureBox_PickerSignal_LDSuctionOn = new System.Windows.Forms.PictureBox();
            this.baseGroupBox_TableSuction = new SLD200_MSL.BaseGroupBox();
            this.pictureBox_TableStatus_AcrylSuction = new System.Windows.Forms.PictureBox();
            this.pictureBox_TableStatus_TableSuction = new System.Windows.Forms.PictureBox();
            this.button_Acryl_SuctionOn = new System.Windows.Forms.Button();
            this.pictureBox_TableSignal_AcrylSuction = new System.Windows.Forms.PictureBox();
            this.button_Table_SuctionOn = new System.Windows.Forms.Button();
            this.pictureBox_TableSignal_TableSuction = new System.Windows.Forms.PictureBox();
            this.tabPage_PowerMeter = new System.Windows.Forms.TabPage();
            this.baseGroupBox_PowerMeasurement = new SLD200_MSL.BaseGroupBox();
            this.listBox_PowerDataList = new System.Windows.Forms.ListBox();
            this.button_PowerMeasurement_Stop = new System.Windows.Forms.Button();
            this.button_PowerMeasurement_Start = new System.Windows.Forms.Button();
            this.tabPage_ScannerCal = new System.Windows.Forms.TabPage();
            this.baseGroupBox_ScannerCalInfo_Parameter = new SLD200_MSL.BaseGroupBox();
            this.baseGroupBox_ScannerCalInfo_Parameter_Laser = new SLD200_MSL.BaseGroupBox();
            this.baseGroupBox_ScannerCalInfo_Parameter_Vision = new SLD200_MSL.BaseGroupBox();
            this.baseGroupBox_ScannerCalInfo_Manual = new SLD200_MSL.BaseGroupBox();
            this.textBox_ManualCal_ScannerPos_Y = new System.Windows.Forms.TextBox();
            this.baseLabel_ManualCal_PosY = new SLD200_MSL.BaseLabel();
            this.textBox_ManualCal_ScannerPos_X = new System.Windows.Forms.TextBox();
            this.baseLabel_ManualCal_PosX = new SLD200_MSL.BaseLabel();
            this.baseLabel_ManualCal_ScannerPos = new SLD200_MSL.BaseLabel();
            this.baseLabel_ManualCal_1stThickness_Unit = new SLD200_MSL.BaseLabel();
            this.textBox_ManualCal_1stThickness = new System.Windows.Forms.TextBox();
            this.comboBox_ManualCal_Division = new System.Windows.Forms.ComboBox();
            this.baseLabel_ManualCal_1stThickness = new SLD200_MSL.BaseLabel();
            this.baseLabel_ManualCal_Division = new SLD200_MSL.BaseLabel();
            this.baseGroupBox_ScannerCalInfo_Auto = new SLD200_MSL.BaseGroupBox();
            this.textBox_AutoCal_ScannerPos_Y = new System.Windows.Forms.TextBox();
            this.baseLabel_AutoCal_PosY = new SLD200_MSL.BaseLabel();
            this.textBox_AutoCal_ScannerPos_X = new System.Windows.Forms.TextBox();
            this.baseLabel_AutoCal_PosX = new SLD200_MSL.BaseLabel();
            this.baseLabel_AutoCal_ScannerPos = new SLD200_MSL.BaseLabel();
            this.baseLabel_AutoCal_1stThickness_Unit = new SLD200_MSL.BaseLabel();
            this.textBox_AutoCal_1stThickness = new System.Windows.Forms.TextBox();
            this.comboBox_AutoCal_Division = new System.Windows.Forms.ComboBox();
            this.baseLabel_AutoCal_1stThickness = new SLD200_MSL.BaseLabel();
            this.baseLabel_AutoCal_Division = new SLD200_MSL.BaseLabel();
            this.baseGroupBox_ScannerCal_Measurement = new SLD200_MSL.BaseGroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.tabControl_ScannerCal_Camera = new System.Windows.Forms.TabControl();
            this.tabPage_LowRes = new System.Windows.Forms.TabPage();
            this.m_visionImageViewer_LowRes = new QMC.Common.Hmi.VisionImageViewer();
            this.tabPage_HighRes = new System.Windows.Forms.TabPage();
            this.m_visionImageViewer_HighRes = new QMC.Common.Hmi.VisionImageViewer();
            this.listBox_ScannerCal_MeasuredDataList = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tabPage_VisionSet = new System.Windows.Forms.TabPage();
            this.tabPage_OneHole = new System.Windows.Forms.TabPage();
            this.baseGroupBox_LaserFocusCheck = new SLD200_MSL.BaseGroupBox();
            this.baseGroupBox_LaserFocusCheck_Func = new SLD200_MSL.BaseGroupBox();
            this.numericUpDown_LaserFocusCheck_LinePos = new System.Windows.Forms.NumericUpDown();
            this.baseLabel_LaserFocusCheck_Arrow3 = new SLD200_MSL.BaseLabel();
            this.button_LaserFocusCheck_DrawnPosMove = new System.Windows.Forms.Button();
            this.textBox_LaserFocusCheck_AxisZFocusPos = new System.Windows.Forms.TextBox();
            this.baseLabel_LaserFocusCheck_AxisZFocusPos = new SLD200_MSL.BaseLabel();
            this.baseLabel_LaserFocusCheck_LinePosMove = new SLD200_MSL.BaseLabel();
            this.button_LaserFocusCheck_Start = new System.Windows.Forms.Button();
            this.baseGroupBox_LaserFocusCheck_PositionParameter = new SLD200_MSL.BaseGroupBox();
            this.button_LaserFocusCheck_ParameterVerification = new System.Windows.Forms.Button();
            this.baseLabel_LaserFocusCheck_Arrow2 = new SLD200_MSL.BaseLabel();
            this.button_LaserFocusCheck_GetXYPos = new System.Windows.Forms.Button();
            this.textBox_LaserFocusCheck_AxisYStartPos = new System.Windows.Forms.TextBox();
            this.baseLabel_LaserFocusCheck_AxisYStartPos = new SLD200_MSL.BaseLabel();
            this.textBox_LaserFocusCheck_AxisXStartPos = new System.Windows.Forms.TextBox();
            this.baseLabel_LaserFocusCheck_AxisXStartPos = new SLD200_MSL.BaseLabel();
            this.baseLabel_LaserFocusCheck_AxisXYRangeSet = new SLD200_MSL.BaseLabel();
            this.baseLabel_LaserFocusCheck_AxisZEndPos_AutoCalc = new SLD200_MSL.BaseLabel();
            this.baseLabel_LaserFocusCheck_Arrow = new SLD200_MSL.BaseLabel();
            this.button_LaserFocusCheck_GetZPos = new System.Windows.Forms.Button();
            this.textBox_LaserFocusCheck_AxisZEndPos = new System.Windows.Forms.TextBox();
            this.baseLabel_LaserFocusCheck_AxisZEndPos = new SLD200_MSL.BaseLabel();
            this.textBox_LaserFocusCheck_AxisZStartPos = new System.Windows.Forms.TextBox();
            this.baseLabel_LaserFocusCheck_AxisZStartPos = new SLD200_MSL.BaseLabel();
            this.baseLabel_LaserFocusCheck_AxisZRangeSet = new SLD200_MSL.BaseLabel();
            this.baseGroupBox_LaserFocusCheck_ShotParameter = new SLD200_MSL.BaseGroupBox();
            this.baseLabel_LaserFocusCheck_DrawLineNum_Unit = new SLD200_MSL.BaseLabel();
            this.textBox_LaserFocusCheck_DrawLineNum = new System.Windows.Forms.TextBox();
            this.baseLabel_LaserFocusCheck_DrawLineNum = new SLD200_MSL.BaseLabel();
            this.baseLabel_LaserFocusCheck_ShotPower_Unit = new SLD200_MSL.BaseLabel();
            this.textBox_LaserFocusCheck_ShotPower = new System.Windows.Forms.TextBox();
            this.baseLabel_LaserFocusCheck_ShotPower = new SLD200_MSL.BaseLabel();
            this.radioButton_LaserFocusCheck_DrawDirection_Ver = new System.Windows.Forms.RadioButton();
            this.radioButton_LaserFocusCheck_DrawDirection_Hor = new System.Windows.Forms.RadioButton();
            this.baseLabel_LaserFocusCheck_DrawDirection = new SLD200_MSL.BaseLabel();
            this.baseLabel_LaserFocusCheck_DrawLength_Unit = new SLD200_MSL.BaseLabel();
            this.textBox_LaserFocusCheck_DrawLength = new System.Windows.Forms.TextBox();
            this.baseLabel_LaserFocusCheck_DrawLength = new SLD200_MSL.BaseLabel();
            this.baseLabel_LaserFocusCheck_DrawSpeed_Unit = new SLD200_MSL.BaseLabel();
            this.textBox_LaserFocusCheck_DrawSpeed = new System.Windows.Forms.TextBox();
            this.baseLabel_LaserFocusCheck_DrawSpeed = new SLD200_MSL.BaseLabel();
            this.baseLabel_LaserFocusCheck_MovePitchXY_Unit = new SLD200_MSL.BaseLabel();
            this.textBox_LaserFocusCheck_MovePitchXY = new System.Windows.Forms.TextBox();
            this.baseLabel_LaserFocusCheck_MovePitchXY = new SLD200_MSL.BaseLabel();
            this.baseLabel_LaserFocusCheck_MovePitchZ_Unit = new SLD200_MSL.BaseLabel();
            this.textBox_LaserFocusCheck_MovePitchZ = new System.Windows.Forms.TextBox();
            this.baseLabel_LaserFocusCheck_MovePitchZ = new SLD200_MSL.BaseLabel();
            this.tabPage_DrillingParameter = new System.Windows.Forms.TabPage();
            this.baseGroupBox_DrillingToolParam = new SLD200_MSL.BaseGroupBox();
            this.baseLabel_DrillingTool_LaserOffDelay_Unit = new SLD200_MSL.BaseLabel();
            this.textBox_DrillingTool_LaserOffDelay = new System.Windows.Forms.TextBox();
            this.baseLabel_DrillingTool_LaserOffDelay = new SLD200_MSL.BaseLabel();
            this.baseLabel_DrillingTool_LaserOnDelay_Unit = new SLD200_MSL.BaseLabel();
            this.textBox_DrillingTool_LaserOnDelay = new System.Windows.Forms.TextBox();
            this.baseLabel_DrillingTool_LaserOnDelay = new SLD200_MSL.BaseLabel();
            this.baseLabel_DrillingTool_JumpDelay_Unit = new SLD200_MSL.BaseLabel();
            this.textBox_DrillingTool_JumpDelay = new System.Windows.Forms.TextBox();
            this.baseLabel_DrillingTool_JumpDelay = new SLD200_MSL.BaseLabel();
            this.baseLabel_DrillingTool_MarkDelay_Unit = new SLD200_MSL.BaseLabel();
            this.textBox_DrillingTool_MarkDelay = new System.Windows.Forms.TextBox();
            this.baseLabel_DrillingTool_MarkDelay = new SLD200_MSL.BaseLabel();
            this.baseLabel_DrillingTool_JumpSpeed_Unit = new SLD200_MSL.BaseLabel();
            this.textBox_DrillingTool_JumpSpeed = new System.Windows.Forms.TextBox();
            this.baseLabel_DrillingTool_JumpSpeed = new SLD200_MSL.BaseLabel();
            this.baseLabel_DrillingTool_MarkSpeed_Unit = new SLD200_MSL.BaseLabel();
            this.textBox_DrillingTool_MarkSpeed = new System.Windows.Forms.TextBox();
            this.baseLabel_DrillingTool_MarkSpeed = new SLD200_MSL.BaseLabel();
            this.baseLabel_DrillingTool_Freq_Unit = new SLD200_MSL.BaseLabel();
            this.textBox_DrillingTool_Frequency = new System.Windows.Forms.TextBox();
            this.baseLabel_DrillingTool_Freq = new SLD200_MSL.BaseLabel();
            this.baseLabel_DrillingTool_ZOffset_Unit = new SLD200_MSL.BaseLabel();
            this.textBox_DrillingTool_ZOffset = new System.Windows.Forms.TextBox();
            this.baseLabel_DrillingTool_ZOffset = new SLD200_MSL.BaseLabel();
            this.textBox_DrillingTool_RepeatCount = new System.Windows.Forms.TextBox();
            this.baseLabel_DrillingTool_RepeatCount = new SLD200_MSL.BaseLabel();
            this.baseLabel_DrillingTool_HoleSize_Unit = new SLD200_MSL.BaseLabel();
            this.textBox_DrillingTool_HoleSize = new System.Windows.Forms.TextBox();
            this.baseLabel_DrillingTool_HoleSize = new SLD200_MSL.BaseLabel();
            this.comboBox_DrillingTool_MaskNo = new System.Windows.Forms.ComboBox();
            this.baseLabel_DrillingTool_MaskNo = new SLD200_MSL.BaseLabel();
            this.comboBox_DrillingTool_Type = new System.Windows.Forms.ComboBox();
            this.baseLabel_DrillingTool_Type = new SLD200_MSL.BaseLabel();
            this.textBox_DrillingTool_No = new System.Windows.Forms.TextBox();
            this.baseLabel_DrillingTool_No = new SLD200_MSL.BaseLabel();
            this.baseGroupBox_DrillingToolList = new SLD200_MSL.BaseGroupBox();
            this.button_DrillingTool_Delete = new System.Windows.Forms.Button();
            this.button_DrillingTool_Add = new System.Windows.Forms.Button();
            this.treeView_DrillingTool = new System.Windows.Forms.TreeView();
            this.tabControl_ManualMode.SuspendLayout();
            this.tabPage_Laser.SuspendLayout();
            this.baseGroupBox_LaserControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_LaserControl_Button_Chiller2On)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_LaserControl_Button_Chiller1On)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_LaserControl_Button_ShutterOpen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_LaserControl_Button_PowerOn)).BeginInit();
            this.baseGroupBox_LaserControl_Status.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_LaserControl_Status_Emission)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_LaserControl_Status_Shutter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_LaserControl_Status_Ready)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_LaserControl_Status_Fault)).BeginInit();
            this.baseGroupBox_DeviceControl.SuspendLayout();
            this.tabPage_Motor.SuspendLayout();
            this.baseGroupBox_SingleRun.SuspendLayout();
            this.baseGroupBox_CycleRun.SuspendLayout();
            this.baseGroupBox_PickerSuction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_PickerStatus_ULSuction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_PickerStatus_LDSuction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_PickerSignal_ULSuctionOn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_PickerSignal_LDSuctionOn)).BeginInit();
            this.baseGroupBox_TableSuction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_TableStatus_AcrylSuction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_TableStatus_TableSuction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_TableSignal_AcrylSuction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_TableSignal_TableSuction)).BeginInit();
            this.tabPage_PowerMeter.SuspendLayout();
            this.baseGroupBox_PowerMeasurement.SuspendLayout();
            this.tabPage_ScannerCal.SuspendLayout();
            this.baseGroupBox_ScannerCalInfo_Parameter.SuspendLayout();
            this.baseGroupBox_ScannerCalInfo_Manual.SuspendLayout();
            this.baseGroupBox_ScannerCalInfo_Auto.SuspendLayout();
            this.baseGroupBox_ScannerCal_Measurement.SuspendLayout();
            this.tabControl_ScannerCal_Camera.SuspendLayout();
            this.tabPage_LowRes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_visionImageViewer_LowRes)).BeginInit();
            this.tabPage_HighRes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_visionImageViewer_HighRes)).BeginInit();
            this.tabPage_OneHole.SuspendLayout();
            this.baseGroupBox_LaserFocusCheck.SuspendLayout();
            this.baseGroupBox_LaserFocusCheck_Func.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_LaserFocusCheck_LinePos)).BeginInit();
            this.baseGroupBox_LaserFocusCheck_PositionParameter.SuspendLayout();
            this.baseGroupBox_LaserFocusCheck_ShotParameter.SuspendLayout();
            this.tabPage_DrillingParameter.SuspendLayout();
            this.baseGroupBox_DrillingToolParam.SuspendLayout();
            this.baseGroupBox_DrillingToolList.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // tabControl_ManualMode
            // 
            this.tabControl_ManualMode.Controls.Add(this.tabPage_Laser);
            this.tabControl_ManualMode.Controls.Add(this.tabPage_Motor);
            this.tabControl_ManualMode.Controls.Add(this.tabPage_PowerMeter);
            this.tabControl_ManualMode.Controls.Add(this.tabPage_ScannerCal);
            this.tabControl_ManualMode.Controls.Add(this.tabPage_VisionSet);
            this.tabControl_ManualMode.Controls.Add(this.tabPage_OneHole);
            this.tabControl_ManualMode.Controls.Add(this.tabPage_DrillingParameter);
            this.tabControl_ManualMode.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl_ManualMode.ItemSize = new System.Drawing.Size(150, 32);
            this.tabControl_ManualMode.Location = new System.Drawing.Point(3, 3);
            this.tabControl_ManualMode.Multiline = true;
            this.tabControl_ManualMode.Name = "tabControl_ManualMode";
            this.tabControl_ManualMode.SelectedIndex = 0;
            this.tabControl_ManualMode.Size = new System.Drawing.Size(1138, 764);
            this.tabControl_ManualMode.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl_ManualMode.TabIndex = 197;
            // 
            // tabPage_Laser
            // 
            this.tabPage_Laser.BackColor = System.Drawing.Color.Transparent;
            this.tabPage_Laser.Controls.Add(this.baseGroupBox_LaserControl);
            this.tabPage_Laser.Controls.Add(this.baseGroupBox_DeviceControl);
            this.tabPage_Laser.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tabPage_Laser.Location = new System.Drawing.Point(4, 36);
            this.tabPage_Laser.Name = "tabPage_Laser";
            this.tabPage_Laser.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Laser.Size = new System.Drawing.Size(1130, 724);
            this.tabPage_Laser.TabIndex = 0;
            this.tabPage_Laser.Text = "Laser";
            // 
            // baseGroupBox_LaserControl
            // 
            this.baseGroupBox_LaserControl.Controls.Add(this.button_Laser_Chiller2On);
            this.baseGroupBox_LaserControl.Controls.Add(this.pictureBox_LaserControl_Button_Chiller2On);
            this.baseGroupBox_LaserControl.Controls.Add(this.button_Laser_Chiller1On);
            this.baseGroupBox_LaserControl.Controls.Add(this.pictureBox_LaserControl_Button_Chiller1On);
            this.baseGroupBox_LaserControl.Controls.Add(this.button_Laser_ShutterOpen);
            this.baseGroupBox_LaserControl.Controls.Add(this.pictureBox_LaserControl_Button_ShutterOpen);
            this.baseGroupBox_LaserControl.Controls.Add(this.button_Laser_PowerOn);
            this.baseGroupBox_LaserControl.Controls.Add(this.pictureBox_LaserControl_Button_PowerOn);
            this.baseGroupBox_LaserControl.Controls.Add(this.baseGroupBox_LaserControl_Status);
            this.baseGroupBox_LaserControl.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_LaserControl.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox_LaserControl.Location = new System.Drawing.Point(11, 135);
            this.baseGroupBox_LaserControl.Name = "baseGroupBox_LaserControl";
            this.baseGroupBox_LaserControl.Size = new System.Drawing.Size(368, 287);
            this.baseGroupBox_LaserControl.TabIndex = 1;
            this.baseGroupBox_LaserControl.TabStop = false;
            this.baseGroupBox_LaserControl.Text = " [ Laser Control ] ";
            // 
            // button_Laser_Chiller2On
            // 
            this.button_Laser_Chiller2On.BackColor = System.Drawing.Color.White;
            this.button_Laser_Chiller2On.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_Laser_Chiller2On.FlatAppearance.BorderSize = 2;
            this.button_Laser_Chiller2On.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_Laser_Chiller2On.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_Laser_Chiller2On.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Laser_Chiller2On.ForeColor = System.Drawing.Color.Black;
            this.button_Laser_Chiller2On.Location = new System.Drawing.Point(226, 133);
            this.button_Laser_Chiller2On.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_Laser_Chiller2On.Name = "button_Laser_Chiller2On";
            this.button_Laser_Chiller2On.Size = new System.Drawing.Size(134, 27);
            this.button_Laser_Chiller2On.TabIndex = 201;
            this.button_Laser_Chiller2On.Text = "Chiller #2  On";
            this.button_Laser_Chiller2On.UseVisualStyleBackColor = false;
            // 
            // pictureBox_LaserControl_Button_Chiller2On
            // 
            this.pictureBox_LaserControl_Button_Chiller2On.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_LaserControl_Button_Chiller2On.Image")));
            this.pictureBox_LaserControl_Button_Chiller2On.Location = new System.Drawing.Point(198, 133);
            this.pictureBox_LaserControl_Button_Chiller2On.Name = "pictureBox_LaserControl_Button_Chiller2On";
            this.pictureBox_LaserControl_Button_Chiller2On.Size = new System.Drawing.Size(26, 26);
            this.pictureBox_LaserControl_Button_Chiller2On.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_LaserControl_Button_Chiller2On.TabIndex = 200;
            this.pictureBox_LaserControl_Button_Chiller2On.TabStop = false;
            // 
            // button_Laser_Chiller1On
            // 
            this.button_Laser_Chiller1On.BackColor = System.Drawing.Color.White;
            this.button_Laser_Chiller1On.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_Laser_Chiller1On.FlatAppearance.BorderSize = 2;
            this.button_Laser_Chiller1On.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_Laser_Chiller1On.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_Laser_Chiller1On.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Laser_Chiller1On.ForeColor = System.Drawing.Color.Black;
            this.button_Laser_Chiller1On.Location = new System.Drawing.Point(226, 101);
            this.button_Laser_Chiller1On.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_Laser_Chiller1On.Name = "button_Laser_Chiller1On";
            this.button_Laser_Chiller1On.Size = new System.Drawing.Size(134, 27);
            this.button_Laser_Chiller1On.TabIndex = 199;
            this.button_Laser_Chiller1On.Text = "Chiller #1  On";
            this.button_Laser_Chiller1On.UseVisualStyleBackColor = false;
            // 
            // pictureBox_LaserControl_Button_Chiller1On
            // 
            this.pictureBox_LaserControl_Button_Chiller1On.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_LaserControl_Button_Chiller1On.Image")));
            this.pictureBox_LaserControl_Button_Chiller1On.Location = new System.Drawing.Point(198, 101);
            this.pictureBox_LaserControl_Button_Chiller1On.Name = "pictureBox_LaserControl_Button_Chiller1On";
            this.pictureBox_LaserControl_Button_Chiller1On.Size = new System.Drawing.Size(26, 26);
            this.pictureBox_LaserControl_Button_Chiller1On.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_LaserControl_Button_Chiller1On.TabIndex = 198;
            this.pictureBox_LaserControl_Button_Chiller1On.TabStop = false;
            // 
            // button_Laser_ShutterOpen
            // 
            this.button_Laser_ShutterOpen.BackColor = System.Drawing.Color.White;
            this.button_Laser_ShutterOpen.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_Laser_ShutterOpen.FlatAppearance.BorderSize = 2;
            this.button_Laser_ShutterOpen.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_Laser_ShutterOpen.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_Laser_ShutterOpen.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Laser_ShutterOpen.ForeColor = System.Drawing.Color.Black;
            this.button_Laser_ShutterOpen.Location = new System.Drawing.Point(226, 69);
            this.button_Laser_ShutterOpen.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_Laser_ShutterOpen.Name = "button_Laser_ShutterOpen";
            this.button_Laser_ShutterOpen.Size = new System.Drawing.Size(134, 27);
            this.button_Laser_ShutterOpen.TabIndex = 197;
            this.button_Laser_ShutterOpen.Text = "Shutter  Open";
            this.button_Laser_ShutterOpen.UseVisualStyleBackColor = false;
            // 
            // pictureBox_LaserControl_Button_ShutterOpen
            // 
            this.pictureBox_LaserControl_Button_ShutterOpen.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_LaserControl_Button_ShutterOpen.Image")));
            this.pictureBox_LaserControl_Button_ShutterOpen.Location = new System.Drawing.Point(198, 69);
            this.pictureBox_LaserControl_Button_ShutterOpen.Name = "pictureBox_LaserControl_Button_ShutterOpen";
            this.pictureBox_LaserControl_Button_ShutterOpen.Size = new System.Drawing.Size(26, 26);
            this.pictureBox_LaserControl_Button_ShutterOpen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_LaserControl_Button_ShutterOpen.TabIndex = 196;
            this.pictureBox_LaserControl_Button_ShutterOpen.TabStop = false;
            // 
            // button_Laser_PowerOn
            // 
            this.button_Laser_PowerOn.BackColor = System.Drawing.Color.White;
            this.button_Laser_PowerOn.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_Laser_PowerOn.FlatAppearance.BorderSize = 2;
            this.button_Laser_PowerOn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_Laser_PowerOn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_Laser_PowerOn.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Laser_PowerOn.ForeColor = System.Drawing.Color.Black;
            this.button_Laser_PowerOn.Location = new System.Drawing.Point(226, 37);
            this.button_Laser_PowerOn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_Laser_PowerOn.Name = "button_Laser_PowerOn";
            this.button_Laser_PowerOn.Size = new System.Drawing.Size(134, 27);
            this.button_Laser_PowerOn.TabIndex = 195;
            this.button_Laser_PowerOn.Text = "Power  On";
            this.button_Laser_PowerOn.UseVisualStyleBackColor = false;
            // 
            // pictureBox_LaserControl_Button_PowerOn
            // 
            this.pictureBox_LaserControl_Button_PowerOn.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_LaserControl_Button_PowerOn.Image")));
            this.pictureBox_LaserControl_Button_PowerOn.Location = new System.Drawing.Point(198, 37);
            this.pictureBox_LaserControl_Button_PowerOn.Name = "pictureBox_LaserControl_Button_PowerOn";
            this.pictureBox_LaserControl_Button_PowerOn.Size = new System.Drawing.Size(26, 26);
            this.pictureBox_LaserControl_Button_PowerOn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_LaserControl_Button_PowerOn.TabIndex = 52;
            this.pictureBox_LaserControl_Button_PowerOn.TabStop = false;
            // 
            // baseGroupBox_LaserControl_Status
            // 
            this.baseGroupBox_LaserControl_Status.Controls.Add(this.baseLabel_Laser_Emission);
            this.baseGroupBox_LaserControl_Status.Controls.Add(this.pictureBox_LaserControl_Status_Emission);
            this.baseGroupBox_LaserControl_Status.Controls.Add(this.baseLabel_Laser_Shutter);
            this.baseGroupBox_LaserControl_Status.Controls.Add(this.pictureBox_LaserControl_Status_Shutter);
            this.baseGroupBox_LaserControl_Status.Controls.Add(this.baseLabel_Laser_Ready);
            this.baseGroupBox_LaserControl_Status.Controls.Add(this.pictureBox_LaserControl_Status_Ready);
            this.baseGroupBox_LaserControl_Status.Controls.Add(this.baseLabel_Laser_Fault);
            this.baseGroupBox_LaserControl_Status.Controls.Add(this.pictureBox_LaserControl_Status_Fault);
            this.baseGroupBox_LaserControl_Status.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_LaserControl_Status.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox_LaserControl_Status.Location = new System.Drawing.Point(8, 28);
            this.baseGroupBox_LaserControl_Status.Name = "baseGroupBox_LaserControl_Status";
            this.baseGroupBox_LaserControl_Status.Size = new System.Drawing.Size(133, 155);
            this.baseGroupBox_LaserControl_Status.TabIndex = 2;
            this.baseGroupBox_LaserControl_Status.TabStop = false;
            this.baseGroupBox_LaserControl_Status.Text = " Status ";
            // 
            // baseLabel_Laser_Emission
            // 
            this.baseLabel_Laser_Emission.AutoSize = true;
            this.baseLabel_Laser_Emission.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_Laser_Emission.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Laser_Emission.Location = new System.Drawing.Point(34, 104);
            this.baseLabel_Laser_Emission.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Laser_Emission.Name = "baseLabel_Laser_Emission";
            this.baseLabel_Laser_Emission.Size = new System.Drawing.Size(57, 16);
            this.baseLabel_Laser_Emission.TabIndex = 116;
            this.baseLabel_Laser_Emission.Text = "Emission";
            // 
            // pictureBox_LaserControl_Status_Emission
            // 
            this.pictureBox_LaserControl_Status_Emission.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_LaserControl_Status_Emission.Image")));
            this.pictureBox_LaserControl_Status_Emission.Location = new System.Drawing.Point(10, 102);
            this.pictureBox_LaserControl_Status_Emission.Name = "pictureBox_LaserControl_Status_Emission";
            this.pictureBox_LaserControl_Status_Emission.Size = new System.Drawing.Size(20, 20);
            this.pictureBox_LaserControl_Status_Emission.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_LaserControl_Status_Emission.TabIndex = 115;
            this.pictureBox_LaserControl_Status_Emission.TabStop = false;
            // 
            // baseLabel_Laser_Shutter
            // 
            this.baseLabel_Laser_Shutter.AutoSize = true;
            this.baseLabel_Laser_Shutter.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_Laser_Shutter.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Laser_Shutter.Location = new System.Drawing.Point(34, 78);
            this.baseLabel_Laser_Shutter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Laser_Shutter.Name = "baseLabel_Laser_Shutter";
            this.baseLabel_Laser_Shutter.Size = new System.Drawing.Size(83, 16);
            this.baseLabel_Laser_Shutter.TabIndex = 114;
            this.baseLabel_Laser_Shutter.Text = "Shutter Open";
            // 
            // pictureBox_LaserControl_Status_Shutter
            // 
            this.pictureBox_LaserControl_Status_Shutter.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_LaserControl_Status_Shutter.Image")));
            this.pictureBox_LaserControl_Status_Shutter.Location = new System.Drawing.Point(10, 76);
            this.pictureBox_LaserControl_Status_Shutter.Name = "pictureBox_LaserControl_Status_Shutter";
            this.pictureBox_LaserControl_Status_Shutter.Size = new System.Drawing.Size(20, 20);
            this.pictureBox_LaserControl_Status_Shutter.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_LaserControl_Status_Shutter.TabIndex = 113;
            this.pictureBox_LaserControl_Status_Shutter.TabStop = false;
            // 
            // baseLabel_Laser_Ready
            // 
            this.baseLabel_Laser_Ready.AutoSize = true;
            this.baseLabel_Laser_Ready.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_Laser_Ready.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Laser_Ready.Location = new System.Drawing.Point(34, 52);
            this.baseLabel_Laser_Ready.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Laser_Ready.Name = "baseLabel_Laser_Ready";
            this.baseLabel_Laser_Ready.Size = new System.Drawing.Size(42, 16);
            this.baseLabel_Laser_Ready.TabIndex = 112;
            this.baseLabel_Laser_Ready.Text = "Ready";
            // 
            // pictureBox_LaserControl_Status_Ready
            // 
            this.pictureBox_LaserControl_Status_Ready.Image = global::SLD200.Properties.Resources.DioEllipseOff;
            this.pictureBox_LaserControl_Status_Ready.Location = new System.Drawing.Point(10, 50);
            this.pictureBox_LaserControl_Status_Ready.Name = "pictureBox_LaserControl_Status_Ready";
            this.pictureBox_LaserControl_Status_Ready.Size = new System.Drawing.Size(20, 20);
            this.pictureBox_LaserControl_Status_Ready.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_LaserControl_Status_Ready.TabIndex = 111;
            this.pictureBox_LaserControl_Status_Ready.TabStop = false;
            // 
            // baseLabel_Laser_Fault
            // 
            this.baseLabel_Laser_Fault.AutoSize = true;
            this.baseLabel_Laser_Fault.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_Laser_Fault.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Laser_Fault.Location = new System.Drawing.Point(34, 26);
            this.baseLabel_Laser_Fault.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Laser_Fault.Name = "baseLabel_Laser_Fault";
            this.baseLabel_Laser_Fault.Size = new System.Drawing.Size(35, 16);
            this.baseLabel_Laser_Fault.TabIndex = 110;
            this.baseLabel_Laser_Fault.Text = "Fault";
            // 
            // pictureBox_LaserControl_Status_Fault
            // 
            this.pictureBox_LaserControl_Status_Fault.Image = global::SLD200.Properties.Resources.StopOff;
            this.pictureBox_LaserControl_Status_Fault.Location = new System.Drawing.Point(10, 24);
            this.pictureBox_LaserControl_Status_Fault.Name = "pictureBox_LaserControl_Status_Fault";
            this.pictureBox_LaserControl_Status_Fault.Size = new System.Drawing.Size(20, 20);
            this.pictureBox_LaserControl_Status_Fault.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_LaserControl_Status_Fault.TabIndex = 40;
            this.pictureBox_LaserControl_Status_Fault.TabStop = false;
            // 
            // baseGroupBox_DeviceControl
            // 
            this.baseGroupBox_DeviceControl.Controls.Add(this.button_Laser_TurnOffAllDevice);
            this.baseGroupBox_DeviceControl.Controls.Add(this.button_Laser_TurnOnAllDevice);
            this.baseGroupBox_DeviceControl.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_DeviceControl.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox_DeviceControl.Location = new System.Drawing.Point(11, 11);
            this.baseGroupBox_DeviceControl.Name = "baseGroupBox_DeviceControl";
            this.baseGroupBox_DeviceControl.Size = new System.Drawing.Size(368, 103);
            this.baseGroupBox_DeviceControl.TabIndex = 0;
            this.baseGroupBox_DeviceControl.TabStop = false;
            this.baseGroupBox_DeviceControl.Text = " [ Device Control ] ";
            // 
            // button_Laser_TurnOffAllDevice
            // 
            this.button_Laser_TurnOffAllDevice.BackColor = System.Drawing.Color.White;
            this.button_Laser_TurnOffAllDevice.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_Laser_TurnOffAllDevice.FlatAppearance.BorderSize = 2;
            this.button_Laser_TurnOffAllDevice.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_Laser_TurnOffAllDevice.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_Laser_TurnOffAllDevice.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Laser_TurnOffAllDevice.ForeColor = System.Drawing.Color.Black;
            this.button_Laser_TurnOffAllDevice.Location = new System.Drawing.Point(188, 31);
            this.button_Laser_TurnOffAllDevice.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_Laser_TurnOffAllDevice.Name = "button_Laser_TurnOffAllDevice";
            this.button_Laser_TurnOffAllDevice.Size = new System.Drawing.Size(172, 61);
            this.button_Laser_TurnOffAllDevice.TabIndex = 195;
            this.button_Laser_TurnOffAllDevice.Text = "Turn Off All Device";
            this.button_Laser_TurnOffAllDevice.UseVisualStyleBackColor = false;
            // 
            // button_Laser_TurnOnAllDevice
            // 
            this.button_Laser_TurnOnAllDevice.BackColor = System.Drawing.Color.White;
            this.button_Laser_TurnOnAllDevice.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_Laser_TurnOnAllDevice.FlatAppearance.BorderSize = 2;
            this.button_Laser_TurnOnAllDevice.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_Laser_TurnOnAllDevice.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_Laser_TurnOnAllDevice.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Laser_TurnOnAllDevice.ForeColor = System.Drawing.Color.Black;
            this.button_Laser_TurnOnAllDevice.Location = new System.Drawing.Point(8, 31);
            this.button_Laser_TurnOnAllDevice.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_Laser_TurnOnAllDevice.Name = "button_Laser_TurnOnAllDevice";
            this.button_Laser_TurnOnAllDevice.Size = new System.Drawing.Size(172, 61);
            this.button_Laser_TurnOnAllDevice.TabIndex = 194;
            this.button_Laser_TurnOnAllDevice.Text = "Turn On All Device";
            this.button_Laser_TurnOnAllDevice.UseVisualStyleBackColor = false;
            // 
            // tabPage_Motor
            // 
            this.tabPage_Motor.Controls.Add(this.baseGroupBox_SingleRun);
            this.tabPage_Motor.Controls.Add(this.baseGroupBox_CycleRun);
            this.tabPage_Motor.Controls.Add(this.baseGroupBox_PickerSuction);
            this.tabPage_Motor.Controls.Add(this.baseGroupBox_TableSuction);
            this.tabPage_Motor.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tabPage_Motor.Location = new System.Drawing.Point(4, 36);
            this.tabPage_Motor.Name = "tabPage_Motor";
            this.tabPage_Motor.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Motor.Size = new System.Drawing.Size(1130, 724);
            this.tabPage_Motor.TabIndex = 1;
            this.tabPage_Motor.Text = "Motor && Func.  ";
            this.tabPage_Motor.UseVisualStyleBackColor = true;
            // 
            // baseGroupBox_SingleRun
            // 
            this.baseGroupBox_SingleRun.Controls.Add(this.button_Move_UnloadPos);
            this.baseGroupBox_SingleRun.Controls.Add(this.button_Move_LoadPos);
            this.baseGroupBox_SingleRun.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_SingleRun.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox_SingleRun.Location = new System.Drawing.Point(243, 11);
            this.baseGroupBox_SingleRun.Name = "baseGroupBox_SingleRun";
            this.baseGroupBox_SingleRun.Size = new System.Drawing.Size(180, 150);
            this.baseGroupBox_SingleRun.TabIndex = 5;
            this.baseGroupBox_SingleRun.TabStop = false;
            this.baseGroupBox_SingleRun.Text = " [ Single Run ] ";
            // 
            // button_Move_UnloadPos
            // 
            this.button_Move_UnloadPos.BackColor = System.Drawing.Color.White;
            this.button_Move_UnloadPos.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_Move_UnloadPos.FlatAppearance.BorderSize = 2;
            this.button_Move_UnloadPos.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_Move_UnloadPos.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_Move_UnloadPos.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Move_UnloadPos.ForeColor = System.Drawing.Color.Black;
            this.button_Move_UnloadPos.Location = new System.Drawing.Point(9, 89);
            this.button_Move_UnloadPos.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_Move_UnloadPos.Name = "button_Move_UnloadPos";
            this.button_Move_UnloadPos.Size = new System.Drawing.Size(162, 51);
            this.button_Move_UnloadPos.TabIndex = 196;
            this.button_Move_UnloadPos.Text = "Move To Unload Pos.";
            this.button_Move_UnloadPos.UseVisualStyleBackColor = false;
            // 
            // button_Move_LoadPos
            // 
            this.button_Move_LoadPos.BackColor = System.Drawing.Color.White;
            this.button_Move_LoadPos.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_Move_LoadPos.FlatAppearance.BorderSize = 2;
            this.button_Move_LoadPos.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_Move_LoadPos.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_Move_LoadPos.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Move_LoadPos.ForeColor = System.Drawing.Color.Black;
            this.button_Move_LoadPos.Location = new System.Drawing.Point(9, 32);
            this.button_Move_LoadPos.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_Move_LoadPos.Name = "button_Move_LoadPos";
            this.button_Move_LoadPos.Size = new System.Drawing.Size(162, 51);
            this.button_Move_LoadPos.TabIndex = 195;
            this.button_Move_LoadPos.Text = "Move To Load Pos.";
            this.button_Move_LoadPos.UseVisualStyleBackColor = false;
            // 
            // baseGroupBox_CycleRun
            // 
            this.baseGroupBox_CycleRun.Controls.Add(this.button_Run_Unloading);
            this.baseGroupBox_CycleRun.Controls.Add(this.button_Run_Loading);
            this.baseGroupBox_CycleRun.Controls.Add(this.button_Run_Align);
            this.baseGroupBox_CycleRun.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_CycleRun.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox_CycleRun.Location = new System.Drawing.Point(243, 182);
            this.baseGroupBox_CycleRun.Name = "baseGroupBox_CycleRun";
            this.baseGroupBox_CycleRun.Size = new System.Drawing.Size(180, 207);
            this.baseGroupBox_CycleRun.TabIndex = 4;
            this.baseGroupBox_CycleRun.TabStop = false;
            this.baseGroupBox_CycleRun.Text = " [ Cycle Run ] ";
            // 
            // button_Run_Unloading
            // 
            this.button_Run_Unloading.BackColor = System.Drawing.Color.White;
            this.button_Run_Unloading.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_Run_Unloading.FlatAppearance.BorderSize = 2;
            this.button_Run_Unloading.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_Run_Unloading.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_Run_Unloading.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Run_Unloading.ForeColor = System.Drawing.Color.Black;
            this.button_Run_Unloading.Location = new System.Drawing.Point(9, 146);
            this.button_Run_Unloading.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_Run_Unloading.Name = "button_Run_Unloading";
            this.button_Run_Unloading.Size = new System.Drawing.Size(162, 51);
            this.button_Run_Unloading.TabIndex = 197;
            this.button_Run_Unloading.Text = "Module Unloading";
            this.button_Run_Unloading.UseVisualStyleBackColor = false;
            // 
            // button_Run_Loading
            // 
            this.button_Run_Loading.BackColor = System.Drawing.Color.White;
            this.button_Run_Loading.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_Run_Loading.FlatAppearance.BorderSize = 2;
            this.button_Run_Loading.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_Run_Loading.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_Run_Loading.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Run_Loading.ForeColor = System.Drawing.Color.Black;
            this.button_Run_Loading.Location = new System.Drawing.Point(9, 89);
            this.button_Run_Loading.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_Run_Loading.Name = "button_Run_Loading";
            this.button_Run_Loading.Size = new System.Drawing.Size(162, 51);
            this.button_Run_Loading.TabIndex = 196;
            this.button_Run_Loading.Text = "Module Loading";
            this.button_Run_Loading.UseVisualStyleBackColor = false;
            // 
            // button_Run_Align
            // 
            this.button_Run_Align.BackColor = System.Drawing.Color.White;
            this.button_Run_Align.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_Run_Align.FlatAppearance.BorderSize = 2;
            this.button_Run_Align.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_Run_Align.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_Run_Align.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Run_Align.ForeColor = System.Drawing.Color.Black;
            this.button_Run_Align.Location = new System.Drawing.Point(9, 32);
            this.button_Run_Align.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_Run_Align.Name = "button_Run_Align";
            this.button_Run_Align.Size = new System.Drawing.Size(162, 51);
            this.button_Run_Align.TabIndex = 195;
            this.button_Run_Align.Text = "Align";
            this.button_Run_Align.UseVisualStyleBackColor = false;
            // 
            // baseGroupBox_PickerSuction
            // 
            this.baseGroupBox_PickerSuction.Controls.Add(this.pictureBox_PickerStatus_ULSuction);
            this.baseGroupBox_PickerSuction.Controls.Add(this.pictureBox_PickerStatus_LDSuction);
            this.baseGroupBox_PickerSuction.Controls.Add(this.button_Picker_ULSuctionOn);
            this.baseGroupBox_PickerSuction.Controls.Add(this.pictureBox_PickerSignal_ULSuctionOn);
            this.baseGroupBox_PickerSuction.Controls.Add(this.button_Picker_LDSuctionOn);
            this.baseGroupBox_PickerSuction.Controls.Add(this.pictureBox_PickerSignal_LDSuctionOn);
            this.baseGroupBox_PickerSuction.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_PickerSuction.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox_PickerSuction.Location = new System.Drawing.Point(11, 182);
            this.baseGroupBox_PickerSuction.Name = "baseGroupBox_PickerSuction";
            this.baseGroupBox_PickerSuction.Size = new System.Drawing.Size(198, 150);
            this.baseGroupBox_PickerSuction.TabIndex = 3;
            this.baseGroupBox_PickerSuction.TabStop = false;
            this.baseGroupBox_PickerSuction.Text = " [ Picker Suction ] ";
            // 
            // pictureBox_PickerStatus_ULSuction
            // 
            this.pictureBox_PickerStatus_ULSuction.Image = global::SLD200.Properties.Resources.DioEllipseOff;
            this.pictureBox_PickerStatus_ULSuction.Location = new System.Drawing.Point(9, 89);
            this.pictureBox_PickerStatus_ULSuction.Name = "pictureBox_PickerStatus_ULSuction";
            this.pictureBox_PickerStatus_ULSuction.Size = new System.Drawing.Size(26, 26);
            this.pictureBox_PickerStatus_ULSuction.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_PickerStatus_ULSuction.TabIndex = 203;
            this.pictureBox_PickerStatus_ULSuction.TabStop = false;
            // 
            // pictureBox_PickerStatus_LDSuction
            // 
            this.pictureBox_PickerStatus_LDSuction.Image = global::SLD200.Properties.Resources.DioEllipseOff;
            this.pictureBox_PickerStatus_LDSuction.Location = new System.Drawing.Point(9, 32);
            this.pictureBox_PickerStatus_LDSuction.Name = "pictureBox_PickerStatus_LDSuction";
            this.pictureBox_PickerStatus_LDSuction.Size = new System.Drawing.Size(26, 26);
            this.pictureBox_PickerStatus_LDSuction.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_PickerStatus_LDSuction.TabIndex = 202;
            this.pictureBox_PickerStatus_LDSuction.TabStop = false;
            // 
            // button_Picker_ULSuctionOn
            // 
            this.button_Picker_ULSuctionOn.BackColor = System.Drawing.Color.White;
            this.button_Picker_ULSuctionOn.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_Picker_ULSuctionOn.FlatAppearance.BorderSize = 2;
            this.button_Picker_ULSuctionOn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_Picker_ULSuctionOn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_Picker_ULSuctionOn.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Picker_ULSuctionOn.ForeColor = System.Drawing.Color.Black;
            this.button_Picker_ULSuctionOn.Location = new System.Drawing.Point(37, 89);
            this.button_Picker_ULSuctionOn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_Picker_ULSuctionOn.Name = "button_Picker_ULSuctionOn";
            this.button_Picker_ULSuctionOn.Size = new System.Drawing.Size(152, 51);
            this.button_Picker_ULSuctionOn.TabIndex = 197;
            this.button_Picker_ULSuctionOn.Text = "UL Picker Suction  On";
            this.button_Picker_ULSuctionOn.UseVisualStyleBackColor = false;
            // 
            // pictureBox_PickerSignal_ULSuctionOn
            // 
            this.pictureBox_PickerSignal_ULSuctionOn.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_PickerSignal_ULSuctionOn.Image")));
            this.pictureBox_PickerSignal_ULSuctionOn.Location = new System.Drawing.Point(9, 114);
            this.pictureBox_PickerSignal_ULSuctionOn.Name = "pictureBox_PickerSignal_ULSuctionOn";
            this.pictureBox_PickerSignal_ULSuctionOn.Size = new System.Drawing.Size(26, 26);
            this.pictureBox_PickerSignal_ULSuctionOn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_PickerSignal_ULSuctionOn.TabIndex = 196;
            this.pictureBox_PickerSignal_ULSuctionOn.TabStop = false;
            // 
            // button_Picker_LDSuctionOn
            // 
            this.button_Picker_LDSuctionOn.BackColor = System.Drawing.Color.White;
            this.button_Picker_LDSuctionOn.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_Picker_LDSuctionOn.FlatAppearance.BorderSize = 2;
            this.button_Picker_LDSuctionOn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_Picker_LDSuctionOn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_Picker_LDSuctionOn.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Picker_LDSuctionOn.ForeColor = System.Drawing.Color.Black;
            this.button_Picker_LDSuctionOn.Location = new System.Drawing.Point(37, 32);
            this.button_Picker_LDSuctionOn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_Picker_LDSuctionOn.Name = "button_Picker_LDSuctionOn";
            this.button_Picker_LDSuctionOn.Size = new System.Drawing.Size(152, 51);
            this.button_Picker_LDSuctionOn.TabIndex = 195;
            this.button_Picker_LDSuctionOn.Text = "LD Picker Suction  On";
            this.button_Picker_LDSuctionOn.UseVisualStyleBackColor = false;
            // 
            // pictureBox_PickerSignal_LDSuctionOn
            // 
            this.pictureBox_PickerSignal_LDSuctionOn.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_PickerSignal_LDSuctionOn.Image")));
            this.pictureBox_PickerSignal_LDSuctionOn.Location = new System.Drawing.Point(9, 57);
            this.pictureBox_PickerSignal_LDSuctionOn.Name = "pictureBox_PickerSignal_LDSuctionOn";
            this.pictureBox_PickerSignal_LDSuctionOn.Size = new System.Drawing.Size(26, 26);
            this.pictureBox_PickerSignal_LDSuctionOn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_PickerSignal_LDSuctionOn.TabIndex = 52;
            this.pictureBox_PickerSignal_LDSuctionOn.TabStop = false;
            // 
            // baseGroupBox_TableSuction
            // 
            this.baseGroupBox_TableSuction.Controls.Add(this.pictureBox_TableStatus_AcrylSuction);
            this.baseGroupBox_TableSuction.Controls.Add(this.pictureBox_TableStatus_TableSuction);
            this.baseGroupBox_TableSuction.Controls.Add(this.button_Acryl_SuctionOn);
            this.baseGroupBox_TableSuction.Controls.Add(this.pictureBox_TableSignal_AcrylSuction);
            this.baseGroupBox_TableSuction.Controls.Add(this.button_Table_SuctionOn);
            this.baseGroupBox_TableSuction.Controls.Add(this.pictureBox_TableSignal_TableSuction);
            this.baseGroupBox_TableSuction.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_TableSuction.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox_TableSuction.Location = new System.Drawing.Point(11, 11);
            this.baseGroupBox_TableSuction.Name = "baseGroupBox_TableSuction";
            this.baseGroupBox_TableSuction.Size = new System.Drawing.Size(198, 150);
            this.baseGroupBox_TableSuction.TabIndex = 2;
            this.baseGroupBox_TableSuction.TabStop = false;
            this.baseGroupBox_TableSuction.Text = " [ Table Suction ] ";
            // 
            // pictureBox_TableStatus_AcrylSuction
            // 
            this.pictureBox_TableStatus_AcrylSuction.Image = global::SLD200.Properties.Resources.DioEllipseOff;
            this.pictureBox_TableStatus_AcrylSuction.Location = new System.Drawing.Point(9, 89);
            this.pictureBox_TableStatus_AcrylSuction.Name = "pictureBox_TableStatus_AcrylSuction";
            this.pictureBox_TableStatus_AcrylSuction.Size = new System.Drawing.Size(26, 26);
            this.pictureBox_TableStatus_AcrylSuction.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_TableStatus_AcrylSuction.TabIndex = 203;
            this.pictureBox_TableStatus_AcrylSuction.TabStop = false;
            // 
            // pictureBox_TableStatus_TableSuction
            // 
            this.pictureBox_TableStatus_TableSuction.Image = global::SLD200.Properties.Resources.DioEllipseOff;
            this.pictureBox_TableStatus_TableSuction.Location = new System.Drawing.Point(9, 32);
            this.pictureBox_TableStatus_TableSuction.Name = "pictureBox_TableStatus_TableSuction";
            this.pictureBox_TableStatus_TableSuction.Size = new System.Drawing.Size(26, 26);
            this.pictureBox_TableStatus_TableSuction.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_TableStatus_TableSuction.TabIndex = 202;
            this.pictureBox_TableStatus_TableSuction.TabStop = false;
            // 
            // button_Acryl_SuctionOn
            // 
            this.button_Acryl_SuctionOn.BackColor = System.Drawing.Color.White;
            this.button_Acryl_SuctionOn.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_Acryl_SuctionOn.FlatAppearance.BorderSize = 2;
            this.button_Acryl_SuctionOn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_Acryl_SuctionOn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_Acryl_SuctionOn.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Acryl_SuctionOn.ForeColor = System.Drawing.Color.Black;
            this.button_Acryl_SuctionOn.Location = new System.Drawing.Point(37, 89);
            this.button_Acryl_SuctionOn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_Acryl_SuctionOn.Name = "button_Acryl_SuctionOn";
            this.button_Acryl_SuctionOn.Size = new System.Drawing.Size(152, 51);
            this.button_Acryl_SuctionOn.TabIndex = 197;
            this.button_Acryl_SuctionOn.Text = "Acryl Suction  On";
            this.button_Acryl_SuctionOn.UseVisualStyleBackColor = false;
            // 
            // pictureBox_TableSignal_AcrylSuction
            // 
            this.pictureBox_TableSignal_AcrylSuction.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_TableSignal_AcrylSuction.Image")));
            this.pictureBox_TableSignal_AcrylSuction.Location = new System.Drawing.Point(9, 114);
            this.pictureBox_TableSignal_AcrylSuction.Name = "pictureBox_TableSignal_AcrylSuction";
            this.pictureBox_TableSignal_AcrylSuction.Size = new System.Drawing.Size(26, 26);
            this.pictureBox_TableSignal_AcrylSuction.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_TableSignal_AcrylSuction.TabIndex = 196;
            this.pictureBox_TableSignal_AcrylSuction.TabStop = false;
            // 
            // button_Table_SuctionOn
            // 
            this.button_Table_SuctionOn.BackColor = System.Drawing.Color.White;
            this.button_Table_SuctionOn.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_Table_SuctionOn.FlatAppearance.BorderSize = 2;
            this.button_Table_SuctionOn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_Table_SuctionOn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_Table_SuctionOn.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Table_SuctionOn.ForeColor = System.Drawing.Color.Black;
            this.button_Table_SuctionOn.Location = new System.Drawing.Point(37, 32);
            this.button_Table_SuctionOn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_Table_SuctionOn.Name = "button_Table_SuctionOn";
            this.button_Table_SuctionOn.Size = new System.Drawing.Size(152, 51);
            this.button_Table_SuctionOn.TabIndex = 195;
            this.button_Table_SuctionOn.Text = "Table Suction  On";
            this.button_Table_SuctionOn.UseVisualStyleBackColor = false;
            // 
            // pictureBox_TableSignal_TableSuction
            // 
            this.pictureBox_TableSignal_TableSuction.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_TableSignal_TableSuction.Image")));
            this.pictureBox_TableSignal_TableSuction.Location = new System.Drawing.Point(9, 57);
            this.pictureBox_TableSignal_TableSuction.Name = "pictureBox_TableSignal_TableSuction";
            this.pictureBox_TableSignal_TableSuction.Size = new System.Drawing.Size(26, 26);
            this.pictureBox_TableSignal_TableSuction.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_TableSignal_TableSuction.TabIndex = 52;
            this.pictureBox_TableSignal_TableSuction.TabStop = false;
            // 
            // tabPage_PowerMeter
            // 
            this.tabPage_PowerMeter.Controls.Add(this.baseGroupBox_PowerMeasurement);
            this.tabPage_PowerMeter.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tabPage_PowerMeter.Location = new System.Drawing.Point(4, 36);
            this.tabPage_PowerMeter.Name = "tabPage_PowerMeter";
            this.tabPage_PowerMeter.Size = new System.Drawing.Size(1130, 724);
            this.tabPage_PowerMeter.TabIndex = 2;
            this.tabPage_PowerMeter.Text = "Power Meter";
            this.tabPage_PowerMeter.UseVisualStyleBackColor = true;
            // 
            // baseGroupBox_PowerMeasurement
            // 
            this.baseGroupBox_PowerMeasurement.Controls.Add(this.listBox_PowerDataList);
            this.baseGroupBox_PowerMeasurement.Controls.Add(this.button_PowerMeasurement_Stop);
            this.baseGroupBox_PowerMeasurement.Controls.Add(this.button_PowerMeasurement_Start);
            this.baseGroupBox_PowerMeasurement.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_PowerMeasurement.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox_PowerMeasurement.Location = new System.Drawing.Point(11, 11);
            this.baseGroupBox_PowerMeasurement.Name = "baseGroupBox_PowerMeasurement";
            this.baseGroupBox_PowerMeasurement.Size = new System.Drawing.Size(263, 502);
            this.baseGroupBox_PowerMeasurement.TabIndex = 6;
            this.baseGroupBox_PowerMeasurement.TabStop = false;
            this.baseGroupBox_PowerMeasurement.Text = " [ Power Measurement ] ";
            // 
            // listBox_PowerDataList
            // 
            this.listBox_PowerDataList.FormattingEnabled = true;
            this.listBox_PowerDataList.ItemHeight = 18;
            this.listBox_PowerDataList.Location = new System.Drawing.Point(9, 32);
            this.listBox_PowerDataList.Name = "listBox_PowerDataList";
            this.listBox_PowerDataList.Size = new System.Drawing.Size(244, 400);
            this.listBox_PowerDataList.TabIndex = 197;
            // 
            // button_PowerMeasurement_Stop
            // 
            this.button_PowerMeasurement_Stop.BackColor = System.Drawing.Color.White;
            this.button_PowerMeasurement_Stop.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_PowerMeasurement_Stop.FlatAppearance.BorderSize = 2;
            this.button_PowerMeasurement_Stop.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_PowerMeasurement_Stop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_PowerMeasurement_Stop.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_PowerMeasurement_Stop.ForeColor = System.Drawing.Color.Black;
            this.button_PowerMeasurement_Stop.Location = new System.Drawing.Point(137, 441);
            this.button_PowerMeasurement_Stop.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_PowerMeasurement_Stop.Name = "button_PowerMeasurement_Stop";
            this.button_PowerMeasurement_Stop.Size = new System.Drawing.Size(116, 51);
            this.button_PowerMeasurement_Stop.TabIndex = 196;
            this.button_PowerMeasurement_Stop.Text = "Stop";
            this.button_PowerMeasurement_Stop.UseVisualStyleBackColor = false;
            // 
            // button_PowerMeasurement_Start
            // 
            this.button_PowerMeasurement_Start.BackColor = System.Drawing.Color.White;
            this.button_PowerMeasurement_Start.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_PowerMeasurement_Start.FlatAppearance.BorderSize = 2;
            this.button_PowerMeasurement_Start.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_PowerMeasurement_Start.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_PowerMeasurement_Start.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_PowerMeasurement_Start.ForeColor = System.Drawing.Color.Black;
            this.button_PowerMeasurement_Start.Location = new System.Drawing.Point(9, 441);
            this.button_PowerMeasurement_Start.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_PowerMeasurement_Start.Name = "button_PowerMeasurement_Start";
            this.button_PowerMeasurement_Start.Size = new System.Drawing.Size(116, 51);
            this.button_PowerMeasurement_Start.TabIndex = 195;
            this.button_PowerMeasurement_Start.Text = "Start";
            this.button_PowerMeasurement_Start.UseVisualStyleBackColor = false;
            // 
            // tabPage_ScannerCal
            // 
            this.tabPage_ScannerCal.Controls.Add(this.baseGroupBox_ScannerCalInfo_Parameter);
            this.tabPage_ScannerCal.Controls.Add(this.baseGroupBox_ScannerCalInfo_Manual);
            this.tabPage_ScannerCal.Controls.Add(this.baseGroupBox_ScannerCalInfo_Auto);
            this.tabPage_ScannerCal.Controls.Add(this.baseGroupBox_ScannerCal_Measurement);
            this.tabPage_ScannerCal.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tabPage_ScannerCal.Location = new System.Drawing.Point(4, 36);
            this.tabPage_ScannerCal.Name = "tabPage_ScannerCal";
            this.tabPage_ScannerCal.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_ScannerCal.Size = new System.Drawing.Size(1130, 724);
            this.tabPage_ScannerCal.TabIndex = 3;
            this.tabPage_ScannerCal.Text = "Scanner Cal.";
            this.tabPage_ScannerCal.UseVisualStyleBackColor = true;
            // 
            // baseGroupBox_ScannerCalInfo_Parameter
            // 
            this.baseGroupBox_ScannerCalInfo_Parameter.Controls.Add(this.baseGroupBox_ScannerCalInfo_Parameter_Laser);
            this.baseGroupBox_ScannerCalInfo_Parameter.Controls.Add(this.baseGroupBox_ScannerCalInfo_Parameter_Vision);
            this.baseGroupBox_ScannerCalInfo_Parameter.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_ScannerCalInfo_Parameter.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox_ScannerCalInfo_Parameter.Location = new System.Drawing.Point(523, 299);
            this.baseGroupBox_ScannerCalInfo_Parameter.Name = "baseGroupBox_ScannerCalInfo_Parameter";
            this.baseGroupBox_ScannerCalInfo_Parameter.Size = new System.Drawing.Size(409, 335);
            this.baseGroupBox_ScannerCalInfo_Parameter.TabIndex = 10;
            this.baseGroupBox_ScannerCalInfo_Parameter.TabStop = false;
            this.baseGroupBox_ScannerCalInfo_Parameter.Text = " [ Parameter ] ";
            // 
            // baseGroupBox_ScannerCalInfo_Parameter_Laser
            // 
            this.baseGroupBox_ScannerCalInfo_Parameter_Laser.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_ScannerCalInfo_Parameter_Laser.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox_ScannerCalInfo_Parameter_Laser.Location = new System.Drawing.Point(209, 28);
            this.baseGroupBox_ScannerCalInfo_Parameter_Laser.Name = "baseGroupBox_ScannerCalInfo_Parameter_Laser";
            this.baseGroupBox_ScannerCalInfo_Parameter_Laser.Size = new System.Drawing.Size(189, 263);
            this.baseGroupBox_ScannerCalInfo_Parameter_Laser.TabIndex = 12;
            this.baseGroupBox_ScannerCalInfo_Parameter_Laser.TabStop = false;
            this.baseGroupBox_ScannerCalInfo_Parameter_Laser.Text = " Laser ";
            // 
            // baseGroupBox_ScannerCalInfo_Parameter_Vision
            // 
            this.baseGroupBox_ScannerCalInfo_Parameter_Vision.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_ScannerCalInfo_Parameter_Vision.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox_ScannerCalInfo_Parameter_Vision.Location = new System.Drawing.Point(10, 28);
            this.baseGroupBox_ScannerCalInfo_Parameter_Vision.Name = "baseGroupBox_ScannerCalInfo_Parameter_Vision";
            this.baseGroupBox_ScannerCalInfo_Parameter_Vision.Size = new System.Drawing.Size(189, 263);
            this.baseGroupBox_ScannerCalInfo_Parameter_Vision.TabIndex = 11;
            this.baseGroupBox_ScannerCalInfo_Parameter_Vision.TabStop = false;
            this.baseGroupBox_ScannerCalInfo_Parameter_Vision.Text = " Vision ";
            // 
            // baseGroupBox_ScannerCalInfo_Manual
            // 
            this.baseGroupBox_ScannerCalInfo_Manual.Controls.Add(this.textBox_ManualCal_ScannerPos_Y);
            this.baseGroupBox_ScannerCalInfo_Manual.Controls.Add(this.baseLabel_ManualCal_PosY);
            this.baseGroupBox_ScannerCalInfo_Manual.Controls.Add(this.textBox_ManualCal_ScannerPos_X);
            this.baseGroupBox_ScannerCalInfo_Manual.Controls.Add(this.baseLabel_ManualCal_PosX);
            this.baseGroupBox_ScannerCalInfo_Manual.Controls.Add(this.baseLabel_ManualCal_ScannerPos);
            this.baseGroupBox_ScannerCalInfo_Manual.Controls.Add(this.baseLabel_ManualCal_1stThickness_Unit);
            this.baseGroupBox_ScannerCalInfo_Manual.Controls.Add(this.textBox_ManualCal_1stThickness);
            this.baseGroupBox_ScannerCalInfo_Manual.Controls.Add(this.comboBox_ManualCal_Division);
            this.baseGroupBox_ScannerCalInfo_Manual.Controls.Add(this.baseLabel_ManualCal_1stThickness);
            this.baseGroupBox_ScannerCalInfo_Manual.Controls.Add(this.baseLabel_ManualCal_Division);
            this.baseGroupBox_ScannerCalInfo_Manual.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_ScannerCalInfo_Manual.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox_ScannerCalInfo_Manual.Location = new System.Drawing.Point(523, 155);
            this.baseGroupBox_ScannerCalInfo_Manual.Name = "baseGroupBox_ScannerCalInfo_Manual";
            this.baseGroupBox_ScannerCalInfo_Manual.Size = new System.Drawing.Size(409, 128);
            this.baseGroupBox_ScannerCalInfo_Manual.TabIndex = 9;
            this.baseGroupBox_ScannerCalInfo_Manual.TabStop = false;
            this.baseGroupBox_ScannerCalInfo_Manual.Text = " [ Manual Cal. Info. ] ";
            // 
            // textBox_ManualCal_ScannerPos_Y
            // 
            this.textBox_ManualCal_ScannerPos_Y.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_ManualCal_ScannerPos_Y.Location = new System.Drawing.Point(308, 92);
            this.textBox_ManualCal_ScannerPos_Y.Name = "textBox_ManualCal_ScannerPos_Y";
            this.textBox_ManualCal_ScannerPos_Y.Size = new System.Drawing.Size(91, 26);
            this.textBox_ManualCal_ScannerPos_Y.TabIndex = 120;
            // 
            // baseLabel_ManualCal_PosY
            // 
            this.baseLabel_ManualCal_PosY.AutoSize = true;
            this.baseLabel_ManualCal_PosY.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_ManualCal_PosY.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_ManualCal_PosY.Location = new System.Drawing.Point(255, 95);
            this.baseLabel_ManualCal_PosY.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_ManualCal_PosY.Name = "baseLabel_ManualCal_PosY";
            this.baseLabel_ManualCal_PosY.Size = new System.Drawing.Size(51, 18);
            this.baseLabel_ManualCal_PosY.TabIndex = 119;
            this.baseLabel_ManualCal_PosY.Text = "Pos. Y";
            // 
            // textBox_ManualCal_ScannerPos_X
            // 
            this.textBox_ManualCal_ScannerPos_X.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_ManualCal_ScannerPos_X.Location = new System.Drawing.Point(308, 61);
            this.textBox_ManualCal_ScannerPos_X.Name = "textBox_ManualCal_ScannerPos_X";
            this.textBox_ManualCal_ScannerPos_X.Size = new System.Drawing.Size(91, 26);
            this.textBox_ManualCal_ScannerPos_X.TabIndex = 118;
            // 
            // baseLabel_ManualCal_PosX
            // 
            this.baseLabel_ManualCal_PosX.AutoSize = true;
            this.baseLabel_ManualCal_PosX.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_ManualCal_PosX.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_ManualCal_PosX.Location = new System.Drawing.Point(255, 64);
            this.baseLabel_ManualCal_PosX.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_ManualCal_PosX.Name = "baseLabel_ManualCal_PosX";
            this.baseLabel_ManualCal_PosX.Size = new System.Drawing.Size(50, 18);
            this.baseLabel_ManualCal_PosX.TabIndex = 117;
            this.baseLabel_ManualCal_PosX.Text = "Pos. X";
            // 
            // baseLabel_ManualCal_ScannerPos
            // 
            this.baseLabel_ManualCal_ScannerPos.AutoSize = true;
            this.baseLabel_ManualCal_ScannerPos.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_ManualCal_ScannerPos.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_ManualCal_ScannerPos.Location = new System.Drawing.Point(255, 32);
            this.baseLabel_ManualCal_ScannerPos.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_ManualCal_ScannerPos.Name = "baseLabel_ManualCal_ScannerPos";
            this.baseLabel_ManualCal_ScannerPos.Size = new System.Drawing.Size(93, 18);
            this.baseLabel_ManualCal_ScannerPos.TabIndex = 116;
            this.baseLabel_ManualCal_ScannerPos.Text = "Scanner Pos.";
            // 
            // baseLabel_ManualCal_1stThickness_Unit
            // 
            this.baseLabel_ManualCal_1stThickness_Unit.AutoSize = true;
            this.baseLabel_ManualCal_1stThickness_Unit.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_ManualCal_1stThickness_Unit.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_ManualCal_1stThickness_Unit.Location = new System.Drawing.Point(182, 64);
            this.baseLabel_ManualCal_1stThickness_Unit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_ManualCal_1stThickness_Unit.Name = "baseLabel_ManualCal_1stThickness_Unit";
            this.baseLabel_ManualCal_1stThickness_Unit.Size = new System.Drawing.Size(23, 18);
            this.baseLabel_ManualCal_1stThickness_Unit.TabIndex = 115;
            this.baseLabel_ManualCal_1stThickness_Unit.Text = "㎜";
            // 
            // textBox_ManualCal_1stThickness
            // 
            this.textBox_ManualCal_1stThickness.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_ManualCal_1stThickness.Location = new System.Drawing.Point(109, 61);
            this.textBox_ManualCal_1stThickness.Name = "textBox_ManualCal_1stThickness";
            this.textBox_ManualCal_1stThickness.Size = new System.Drawing.Size(72, 26);
            this.textBox_ManualCal_1stThickness.TabIndex = 114;
            // 
            // comboBox_ManualCal_Division
            // 
            this.comboBox_ManualCal_Division.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_ManualCal_Division.FormattingEnabled = true;
            this.comboBox_ManualCal_Division.Items.AddRange(new object[] {
            "5 X 5",
            "7 X 7",
            "9 X 9",
            "11 X 11",
            "13 X 13",
            "15 X 15"});
            this.comboBox_ManualCal_Division.Location = new System.Drawing.Point(109, 33);
            this.comboBox_ManualCal_Division.Name = "comboBox_ManualCal_Division";
            this.comboBox_ManualCal_Division.Size = new System.Drawing.Size(92, 26);
            this.comboBox_ManualCal_Division.TabIndex = 113;
            this.comboBox_ManualCal_Division.Text = "5 X 5";
            // 
            // baseLabel_ManualCal_1stThickness
            // 
            this.baseLabel_ManualCal_1stThickness.AutoSize = true;
            this.baseLabel_ManualCal_1stThickness.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_ManualCal_1stThickness.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_ManualCal_1stThickness.Location = new System.Drawing.Point(7, 64);
            this.baseLabel_ManualCal_1stThickness.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_ManualCal_1stThickness.Name = "baseLabel_ManualCal_1stThickness";
            this.baseLabel_ManualCal_1stThickness.Size = new System.Drawing.Size(100, 18);
            this.baseLabel_ManualCal_1stThickness.TabIndex = 112;
            this.baseLabel_ManualCal_1stThickness.Text = "1\'st Thickness";
            // 
            // baseLabel_ManualCal_Division
            // 
            this.baseLabel_ManualCal_Division.AutoSize = true;
            this.baseLabel_ManualCal_Division.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_ManualCal_Division.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_ManualCal_Division.Location = new System.Drawing.Point(44, 36);
            this.baseLabel_ManualCal_Division.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_ManualCal_Division.Name = "baseLabel_ManualCal_Division";
            this.baseLabel_ManualCal_Division.Size = new System.Drawing.Size(55, 18);
            this.baseLabel_ManualCal_Division.TabIndex = 111;
            this.baseLabel_ManualCal_Division.Text = "Division";
            // 
            // baseGroupBox_ScannerCalInfo_Auto
            // 
            this.baseGroupBox_ScannerCalInfo_Auto.Controls.Add(this.textBox_AutoCal_ScannerPos_Y);
            this.baseGroupBox_ScannerCalInfo_Auto.Controls.Add(this.baseLabel_AutoCal_PosY);
            this.baseGroupBox_ScannerCalInfo_Auto.Controls.Add(this.textBox_AutoCal_ScannerPos_X);
            this.baseGroupBox_ScannerCalInfo_Auto.Controls.Add(this.baseLabel_AutoCal_PosX);
            this.baseGroupBox_ScannerCalInfo_Auto.Controls.Add(this.baseLabel_AutoCal_ScannerPos);
            this.baseGroupBox_ScannerCalInfo_Auto.Controls.Add(this.baseLabel_AutoCal_1stThickness_Unit);
            this.baseGroupBox_ScannerCalInfo_Auto.Controls.Add(this.textBox_AutoCal_1stThickness);
            this.baseGroupBox_ScannerCalInfo_Auto.Controls.Add(this.comboBox_AutoCal_Division);
            this.baseGroupBox_ScannerCalInfo_Auto.Controls.Add(this.baseLabel_AutoCal_1stThickness);
            this.baseGroupBox_ScannerCalInfo_Auto.Controls.Add(this.baseLabel_AutoCal_Division);
            this.baseGroupBox_ScannerCalInfo_Auto.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_ScannerCalInfo_Auto.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox_ScannerCalInfo_Auto.Location = new System.Drawing.Point(523, 11);
            this.baseGroupBox_ScannerCalInfo_Auto.Name = "baseGroupBox_ScannerCalInfo_Auto";
            this.baseGroupBox_ScannerCalInfo_Auto.Size = new System.Drawing.Size(409, 128);
            this.baseGroupBox_ScannerCalInfo_Auto.TabIndex = 8;
            this.baseGroupBox_ScannerCalInfo_Auto.TabStop = false;
            this.baseGroupBox_ScannerCalInfo_Auto.Text = " [ Auto Cal. Info. ] ";
            // 
            // textBox_AutoCal_ScannerPos_Y
            // 
            this.textBox_AutoCal_ScannerPos_Y.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_AutoCal_ScannerPos_Y.Location = new System.Drawing.Point(308, 92);
            this.textBox_AutoCal_ScannerPos_Y.Name = "textBox_AutoCal_ScannerPos_Y";
            this.textBox_AutoCal_ScannerPos_Y.Size = new System.Drawing.Size(91, 26);
            this.textBox_AutoCal_ScannerPos_Y.TabIndex = 120;
            // 
            // baseLabel_AutoCal_PosY
            // 
            this.baseLabel_AutoCal_PosY.AutoSize = true;
            this.baseLabel_AutoCal_PosY.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_AutoCal_PosY.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_AutoCal_PosY.Location = new System.Drawing.Point(255, 95);
            this.baseLabel_AutoCal_PosY.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_AutoCal_PosY.Name = "baseLabel_AutoCal_PosY";
            this.baseLabel_AutoCal_PosY.Size = new System.Drawing.Size(51, 18);
            this.baseLabel_AutoCal_PosY.TabIndex = 119;
            this.baseLabel_AutoCal_PosY.Text = "Pos. Y";
            // 
            // textBox_AutoCal_ScannerPos_X
            // 
            this.textBox_AutoCal_ScannerPos_X.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_AutoCal_ScannerPos_X.Location = new System.Drawing.Point(308, 61);
            this.textBox_AutoCal_ScannerPos_X.Name = "textBox_AutoCal_ScannerPos_X";
            this.textBox_AutoCal_ScannerPos_X.Size = new System.Drawing.Size(91, 26);
            this.textBox_AutoCal_ScannerPos_X.TabIndex = 118;
            // 
            // baseLabel_AutoCal_PosX
            // 
            this.baseLabel_AutoCal_PosX.AutoSize = true;
            this.baseLabel_AutoCal_PosX.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_AutoCal_PosX.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_AutoCal_PosX.Location = new System.Drawing.Point(255, 64);
            this.baseLabel_AutoCal_PosX.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_AutoCal_PosX.Name = "baseLabel_AutoCal_PosX";
            this.baseLabel_AutoCal_PosX.Size = new System.Drawing.Size(50, 18);
            this.baseLabel_AutoCal_PosX.TabIndex = 117;
            this.baseLabel_AutoCal_PosX.Text = "Pos. X";
            // 
            // baseLabel_AutoCal_ScannerPos
            // 
            this.baseLabel_AutoCal_ScannerPos.AutoSize = true;
            this.baseLabel_AutoCal_ScannerPos.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_AutoCal_ScannerPos.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_AutoCal_ScannerPos.Location = new System.Drawing.Point(255, 32);
            this.baseLabel_AutoCal_ScannerPos.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_AutoCal_ScannerPos.Name = "baseLabel_AutoCal_ScannerPos";
            this.baseLabel_AutoCal_ScannerPos.Size = new System.Drawing.Size(93, 18);
            this.baseLabel_AutoCal_ScannerPos.TabIndex = 116;
            this.baseLabel_AutoCal_ScannerPos.Text = "Scanner Pos.";
            // 
            // baseLabel_AutoCal_1stThickness_Unit
            // 
            this.baseLabel_AutoCal_1stThickness_Unit.AutoSize = true;
            this.baseLabel_AutoCal_1stThickness_Unit.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_AutoCal_1stThickness_Unit.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_AutoCal_1stThickness_Unit.Location = new System.Drawing.Point(182, 64);
            this.baseLabel_AutoCal_1stThickness_Unit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_AutoCal_1stThickness_Unit.Name = "baseLabel_AutoCal_1stThickness_Unit";
            this.baseLabel_AutoCal_1stThickness_Unit.Size = new System.Drawing.Size(23, 18);
            this.baseLabel_AutoCal_1stThickness_Unit.TabIndex = 115;
            this.baseLabel_AutoCal_1stThickness_Unit.Text = "㎜";
            // 
            // textBox_AutoCal_1stThickness
            // 
            this.textBox_AutoCal_1stThickness.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_AutoCal_1stThickness.Location = new System.Drawing.Point(109, 61);
            this.textBox_AutoCal_1stThickness.Name = "textBox_AutoCal_1stThickness";
            this.textBox_AutoCal_1stThickness.Size = new System.Drawing.Size(72, 26);
            this.textBox_AutoCal_1stThickness.TabIndex = 114;
            // 
            // comboBox_AutoCal_Division
            // 
            this.comboBox_AutoCal_Division.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_AutoCal_Division.FormattingEnabled = true;
            this.comboBox_AutoCal_Division.Items.AddRange(new object[] {
            "5 X 5",
            "7 X 7",
            "9 X 9",
            "11 X 11",
            "13 X 13",
            "15 X 15"});
            this.comboBox_AutoCal_Division.Location = new System.Drawing.Point(109, 33);
            this.comboBox_AutoCal_Division.Name = "comboBox_AutoCal_Division";
            this.comboBox_AutoCal_Division.Size = new System.Drawing.Size(92, 26);
            this.comboBox_AutoCal_Division.TabIndex = 113;
            this.comboBox_AutoCal_Division.Text = "9 X 9";
            // 
            // baseLabel_AutoCal_1stThickness
            // 
            this.baseLabel_AutoCal_1stThickness.AutoSize = true;
            this.baseLabel_AutoCal_1stThickness.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_AutoCal_1stThickness.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_AutoCal_1stThickness.Location = new System.Drawing.Point(7, 64);
            this.baseLabel_AutoCal_1stThickness.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_AutoCal_1stThickness.Name = "baseLabel_AutoCal_1stThickness";
            this.baseLabel_AutoCal_1stThickness.Size = new System.Drawing.Size(100, 18);
            this.baseLabel_AutoCal_1stThickness.TabIndex = 112;
            this.baseLabel_AutoCal_1stThickness.Text = "1\'st Thickness";
            // 
            // baseLabel_AutoCal_Division
            // 
            this.baseLabel_AutoCal_Division.AutoSize = true;
            this.baseLabel_AutoCal_Division.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_AutoCal_Division.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_AutoCal_Division.Location = new System.Drawing.Point(44, 36);
            this.baseLabel_AutoCal_Division.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_AutoCal_Division.Name = "baseLabel_AutoCal_Division";
            this.baseLabel_AutoCal_Division.Size = new System.Drawing.Size(55, 18);
            this.baseLabel_AutoCal_Division.TabIndex = 111;
            this.baseLabel_AutoCal_Division.Text = "Division";
            // 
            // baseGroupBox_ScannerCal_Measurement
            // 
            this.baseGroupBox_ScannerCal_Measurement.Controls.Add(this.button3);
            this.baseGroupBox_ScannerCal_Measurement.Controls.Add(this.tabControl_ScannerCal_Camera);
            this.baseGroupBox_ScannerCal_Measurement.Controls.Add(this.listBox_ScannerCal_MeasuredDataList);
            this.baseGroupBox_ScannerCal_Measurement.Controls.Add(this.button1);
            this.baseGroupBox_ScannerCal_Measurement.Controls.Add(this.button2);
            this.baseGroupBox_ScannerCal_Measurement.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_ScannerCal_Measurement.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox_ScannerCal_Measurement.Location = new System.Drawing.Point(11, 11);
            this.baseGroupBox_ScannerCal_Measurement.Name = "baseGroupBox_ScannerCal_Measurement";
            this.baseGroupBox_ScannerCal_Measurement.Size = new System.Drawing.Size(489, 707);
            this.baseGroupBox_ScannerCal_Measurement.TabIndex = 7;
            this.baseGroupBox_ScannerCal_Measurement.TabStop = false;
            this.baseGroupBox_ScannerCal_Measurement.Text = " [ Scanner Calibration ] ";
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.White;
            this.button3.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button3.FlatAppearance.BorderSize = 2;
            this.button3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button3.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.Color.Black;
            this.button3.Location = new System.Drawing.Point(279, 650);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(116, 51);
            this.button3.TabIndex = 199;
            this.button3.Text = "Test";
            this.button3.UseVisualStyleBackColor = false;
            // 
            // tabControl_ScannerCal_Camera
            // 
            this.tabControl_ScannerCal_Camera.Controls.Add(this.tabPage_LowRes);
            this.tabControl_ScannerCal_Camera.Controls.Add(this.tabPage_HighRes);
            this.tabControl_ScannerCal_Camera.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl_ScannerCal_Camera.ItemSize = new System.Drawing.Size(202, 22);
            this.tabControl_ScannerCal_Camera.Location = new System.Drawing.Point(9, 32);
            this.tabControl_ScannerCal_Camera.Multiline = true;
            this.tabControl_ScannerCal_Camera.Name = "tabControl_ScannerCal_Camera";
            this.tabControl_ScannerCal_Camera.SelectedIndex = 0;
            this.tabControl_ScannerCal_Camera.Size = new System.Drawing.Size(471, 427);
            this.tabControl_ScannerCal_Camera.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl_ScannerCal_Camera.TabIndex = 198;
            // 
            // tabPage_LowRes
            // 
            this.tabPage_LowRes.Controls.Add(this.m_visionImageViewer_LowRes);
            this.tabPage_LowRes.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage_LowRes.Location = new System.Drawing.Point(4, 26);
            this.tabPage_LowRes.Name = "tabPage_LowRes";
            this.tabPage_LowRes.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_LowRes.Size = new System.Drawing.Size(463, 397);
            this.tabPage_LowRes.TabIndex = 0;
            this.tabPage_LowRes.Text = "Coarse Vision";
            this.tabPage_LowRes.UseVisualStyleBackColor = true;
            // 
            // m_visionImageViewer_LowRes
            // 
            this.m_visionImageViewer_LowRes.BackColor = System.Drawing.Color.Black;
            this.m_visionImageViewer_LowRes.Camera = null;
            this.m_visionImageViewer_LowRes.CameraSwitch = null;
            this.m_visionImageViewer_LowRes.FrameRate = 1D;
            this.m_visionImageViewer_LowRes.InputImage = null;
            this.m_visionImageViewer_LowRes.IsViewCustomizedImage = false;
            this.m_visionImageViewer_LowRes.Location = new System.Drawing.Point(0, 0);
            this.m_visionImageViewer_LowRes.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.m_visionImageViewer_LowRes.Name = "m_visionImageViewer_LowRes";
            this.m_visionImageViewer_LowRes.OperatingType = QMC.Common.Hmi.VisionImageViewer.OperatingTypes.Center;
            this.m_visionImageViewer_LowRes.Simulated = false;
            this.m_visionImageViewer_LowRes.Size = new System.Drawing.Size(463, 397);
            this.m_visionImageViewer_LowRes.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.m_visionImageViewer_LowRes.TabIndex = 106;
            this.m_visionImageViewer_LowRes.TabStop = false;
            this.m_visionImageViewer_LowRes.UpdateDelayTime = 160;
            this.m_visionImageViewer_LowRes.VisibleCrossLine = true;
            // 
            // tabPage_HighRes
            // 
            this.tabPage_HighRes.Controls.Add(this.m_visionImageViewer_HighRes);
            this.tabPage_HighRes.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage_HighRes.Location = new System.Drawing.Point(4, 26);
            this.tabPage_HighRes.Name = "tabPage_HighRes";
            this.tabPage_HighRes.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_HighRes.Size = new System.Drawing.Size(463, 397);
            this.tabPage_HighRes.TabIndex = 1;
            this.tabPage_HighRes.Text = "Fine Vision";
            this.tabPage_HighRes.UseVisualStyleBackColor = true;
            // 
            // m_visionImageViewer_HighRes
            // 
            this.m_visionImageViewer_HighRes.BackColor = System.Drawing.Color.Black;
            this.m_visionImageViewer_HighRes.Camera = null;
            this.m_visionImageViewer_HighRes.CameraSwitch = null;
            this.m_visionImageViewer_HighRes.FrameRate = 1D;
            this.m_visionImageViewer_HighRes.InputImage = null;
            this.m_visionImageViewer_HighRes.IsViewCustomizedImage = false;
            this.m_visionImageViewer_HighRes.Location = new System.Drawing.Point(0, 0);
            this.m_visionImageViewer_HighRes.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.m_visionImageViewer_HighRes.Name = "m_visionImageViewer_HighRes";
            this.m_visionImageViewer_HighRes.OperatingType = QMC.Common.Hmi.VisionImageViewer.OperatingTypes.Center;
            this.m_visionImageViewer_HighRes.Simulated = false;
            this.m_visionImageViewer_HighRes.Size = new System.Drawing.Size(463, 397);
            this.m_visionImageViewer_HighRes.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.m_visionImageViewer_HighRes.TabIndex = 107;
            this.m_visionImageViewer_HighRes.TabStop = false;
            this.m_visionImageViewer_HighRes.UpdateDelayTime = 160;
            this.m_visionImageViewer_HighRes.VisibleCrossLine = true;
            // 
            // listBox_ScannerCal_MeasuredDataList
            // 
            this.listBox_ScannerCal_MeasuredDataList.FormattingEnabled = true;
            this.listBox_ScannerCal_MeasuredDataList.ItemHeight = 18;
            this.listBox_ScannerCal_MeasuredDataList.Location = new System.Drawing.Point(9, 466);
            this.listBox_ScannerCal_MeasuredDataList.Name = "listBox_ScannerCal_MeasuredDataList";
            this.listBox_ScannerCal_MeasuredDataList.Size = new System.Drawing.Size(471, 166);
            this.listBox_ScannerCal_MeasuredDataList.TabIndex = 197;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.White;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button1.FlatAppearance.BorderSize = 2;
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(137, 650);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(116, 51);
            this.button1.TabIndex = 196;
            this.button1.Text = "Train";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.White;
            this.button2.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button2.FlatAppearance.BorderSize = 2;
            this.button2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button2.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.Black;
            this.button2.Location = new System.Drawing.Point(9, 650);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(116, 51);
            this.button2.TabIndex = 195;
            this.button2.Text = "Start Live";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // tabPage_VisionSet
            // 
            this.tabPage_VisionSet.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tabPage_VisionSet.Location = new System.Drawing.Point(4, 36);
            this.tabPage_VisionSet.Name = "tabPage_VisionSet";
            this.tabPage_VisionSet.Size = new System.Drawing.Size(1130, 724);
            this.tabPage_VisionSet.TabIndex = 4;
            this.tabPage_VisionSet.Text = "Vision Set.";
            this.tabPage_VisionSet.UseVisualStyleBackColor = true;
            // 
            // tabPage_OneHole
            // 
            this.tabPage_OneHole.Controls.Add(this.baseGroupBox_LaserFocusCheck);
            this.tabPage_OneHole.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tabPage_OneHole.Location = new System.Drawing.Point(4, 36);
            this.tabPage_OneHole.Name = "tabPage_OneHole";
            this.tabPage_OneHole.Size = new System.Drawing.Size(1130, 724);
            this.tabPage_OneHole.TabIndex = 5;
            this.tabPage_OneHole.Text = "One Hole";
            this.tabPage_OneHole.UseVisualStyleBackColor = true;
            // 
            // baseGroupBox_LaserFocusCheck
            // 
            this.baseGroupBox_LaserFocusCheck.Controls.Add(this.baseGroupBox_LaserFocusCheck_Func);
            this.baseGroupBox_LaserFocusCheck.Controls.Add(this.baseGroupBox_LaserFocusCheck_PositionParameter);
            this.baseGroupBox_LaserFocusCheck.Controls.Add(this.baseGroupBox_LaserFocusCheck_ShotParameter);
            this.baseGroupBox_LaserFocusCheck.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_LaserFocusCheck.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox_LaserFocusCheck.Location = new System.Drawing.Point(11, 11);
            this.baseGroupBox_LaserFocusCheck.Name = "baseGroupBox_LaserFocusCheck";
            this.baseGroupBox_LaserFocusCheck.Size = new System.Drawing.Size(520, 692);
            this.baseGroupBox_LaserFocusCheck.TabIndex = 9;
            this.baseGroupBox_LaserFocusCheck.TabStop = false;
            this.baseGroupBox_LaserFocusCheck.Text = " [ Laser Focus Check ] ";
            // 
            // baseGroupBox_LaserFocusCheck_Func
            // 
            this.baseGroupBox_LaserFocusCheck_Func.Controls.Add(this.numericUpDown_LaserFocusCheck_LinePos);
            this.baseGroupBox_LaserFocusCheck_Func.Controls.Add(this.baseLabel_LaserFocusCheck_Arrow3);
            this.baseGroupBox_LaserFocusCheck_Func.Controls.Add(this.button_LaserFocusCheck_DrawnPosMove);
            this.baseGroupBox_LaserFocusCheck_Func.Controls.Add(this.textBox_LaserFocusCheck_AxisZFocusPos);
            this.baseGroupBox_LaserFocusCheck_Func.Controls.Add(this.baseLabel_LaserFocusCheck_AxisZFocusPos);
            this.baseGroupBox_LaserFocusCheck_Func.Controls.Add(this.baseLabel_LaserFocusCheck_LinePosMove);
            this.baseGroupBox_LaserFocusCheck_Func.Controls.Add(this.button_LaserFocusCheck_Start);
            this.baseGroupBox_LaserFocusCheck_Func.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_LaserFocusCheck_Func.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox_LaserFocusCheck_Func.Location = new System.Drawing.Point(10, 458);
            this.baseGroupBox_LaserFocusCheck_Func.Name = "baseGroupBox_LaserFocusCheck_Func";
            this.baseGroupBox_LaserFocusCheck_Func.Size = new System.Drawing.Size(501, 165);
            this.baseGroupBox_LaserFocusCheck_Func.TabIndex = 123;
            this.baseGroupBox_LaserFocusCheck_Func.TabStop = false;
            this.baseGroupBox_LaserFocusCheck_Func.Text = " Function ";
            // 
            // numericUpDown_LaserFocusCheck_LinePos
            // 
            this.numericUpDown_LaserFocusCheck_LinePos.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDown_LaserFocusCheck_LinePos.Location = new System.Drawing.Point(59, 128);
            this.numericUpDown_LaserFocusCheck_LinePos.Name = "numericUpDown_LaserFocusCheck_LinePos";
            this.numericUpDown_LaserFocusCheck_LinePos.Size = new System.Drawing.Size(58, 23);
            this.numericUpDown_LaserFocusCheck_LinePos.TabIndex = 206;
            // 
            // baseLabel_LaserFocusCheck_Arrow3
            // 
            this.baseLabel_LaserFocusCheck_Arrow3.AutoSize = true;
            this.baseLabel_LaserFocusCheck_Arrow3.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_LaserFocusCheck_Arrow3.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LaserFocusCheck_Arrow3.Location = new System.Drawing.Point(214, 126);
            this.baseLabel_LaserFocusCheck_Arrow3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LaserFocusCheck_Arrow3.Name = "baseLabel_LaserFocusCheck_Arrow3";
            this.baseLabel_LaserFocusCheck_Arrow3.Size = new System.Drawing.Size(50, 25);
            this.baseLabel_LaserFocusCheck_Arrow3.TabIndex = 205;
            this.baseLabel_LaserFocusCheck_Arrow3.Text = "▶▶";
            // 
            // button_LaserFocusCheck_DrawnPosMove
            // 
            this.button_LaserFocusCheck_DrawnPosMove.BackColor = System.Drawing.Color.White;
            this.button_LaserFocusCheck_DrawnPosMove.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_LaserFocusCheck_DrawnPosMove.FlatAppearance.BorderSize = 2;
            this.button_LaserFocusCheck_DrawnPosMove.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_LaserFocusCheck_DrawnPosMove.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_LaserFocusCheck_DrawnPosMove.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_LaserFocusCheck_DrawnPosMove.ForeColor = System.Drawing.Color.Black;
            this.button_LaserFocusCheck_DrawnPosMove.Location = new System.Drawing.Point(120, 125);
            this.button_LaserFocusCheck_DrawnPosMove.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_LaserFocusCheck_DrawnPosMove.Name = "button_LaserFocusCheck_DrawnPosMove";
            this.button_LaserFocusCheck_DrawnPosMove.Size = new System.Drawing.Size(68, 29);
            this.button_LaserFocusCheck_DrawnPosMove.TabIndex = 204;
            this.button_LaserFocusCheck_DrawnPosMove.Text = "Move";
            this.button_LaserFocusCheck_DrawnPosMove.UseVisualStyleBackColor = false;
            // 
            // textBox_LaserFocusCheck_AxisZFocusPos
            // 
            this.textBox_LaserFocusCheck_AxisZFocusPos.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_LaserFocusCheck_AxisZFocusPos.Location = new System.Drawing.Point(395, 128);
            this.textBox_LaserFocusCheck_AxisZFocusPos.Name = "textBox_LaserFocusCheck_AxisZFocusPos";
            this.textBox_LaserFocusCheck_AxisZFocusPos.Size = new System.Drawing.Size(96, 23);
            this.textBox_LaserFocusCheck_AxisZFocusPos.TabIndex = 201;
            // 
            // baseLabel_LaserFocusCheck_AxisZFocusPos
            // 
            this.baseLabel_LaserFocusCheck_AxisZFocusPos.AutoSize = true;
            this.baseLabel_LaserFocusCheck_AxisZFocusPos.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_LaserFocusCheck_AxisZFocusPos.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LaserFocusCheck_AxisZFocusPos.Location = new System.Drawing.Point(283, 132);
            this.baseLabel_LaserFocusCheck_AxisZFocusPos.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LaserFocusCheck_AxisZFocusPos.Name = "baseLabel_LaserFocusCheck_AxisZFocusPos";
            this.baseLabel_LaserFocusCheck_AxisZFocusPos.Size = new System.Drawing.Size(107, 16);
            this.baseLabel_LaserFocusCheck_AxisZFocusPos.TabIndex = 200;
            this.baseLabel_LaserFocusCheck_AxisZFocusPos.Text = "Axis-Z Focus Pos.";
            // 
            // baseLabel_LaserFocusCheck_LinePosMove
            // 
            this.baseLabel_LaserFocusCheck_LinePosMove.AutoSize = true;
            this.baseLabel_LaserFocusCheck_LinePosMove.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_LaserFocusCheck_LinePosMove.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LaserFocusCheck_LinePosMove.Location = new System.Drawing.Point(9, 103);
            this.baseLabel_LaserFocusCheck_LinePosMove.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LaserFocusCheck_LinePosMove.Name = "baseLabel_LaserFocusCheck_LinePosMove";
            this.baseLabel_LaserFocusCheck_LinePosMove.Size = new System.Drawing.Size(166, 16);
            this.baseLabel_LaserFocusCheck_LinePosMove.TabIndex = 199;
            this.baseLabel_LaserFocusCheck_LinePosMove.Text = "Move to the Drawn Location";
            // 
            // button_LaserFocusCheck_Start
            // 
            this.button_LaserFocusCheck_Start.BackColor = System.Drawing.Color.White;
            this.button_LaserFocusCheck_Start.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_LaserFocusCheck_Start.FlatAppearance.BorderSize = 2;
            this.button_LaserFocusCheck_Start.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_LaserFocusCheck_Start.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_LaserFocusCheck_Start.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_LaserFocusCheck_Start.ForeColor = System.Drawing.Color.Black;
            this.button_LaserFocusCheck_Start.Location = new System.Drawing.Point(9, 26);
            this.button_LaserFocusCheck_Start.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_LaserFocusCheck_Start.Name = "button_LaserFocusCheck_Start";
            this.button_LaserFocusCheck_Start.Size = new System.Drawing.Size(187, 46);
            this.button_LaserFocusCheck_Start.TabIndex = 196;
            this.button_LaserFocusCheck_Start.Text = "Laser Focus Check Start";
            this.button_LaserFocusCheck_Start.UseVisualStyleBackColor = false;
            // 
            // baseGroupBox_LaserFocusCheck_PositionParameter
            // 
            this.baseGroupBox_LaserFocusCheck_PositionParameter.Controls.Add(this.button_LaserFocusCheck_ParameterVerification);
            this.baseGroupBox_LaserFocusCheck_PositionParameter.Controls.Add(this.baseLabel_LaserFocusCheck_Arrow2);
            this.baseGroupBox_LaserFocusCheck_PositionParameter.Controls.Add(this.button_LaserFocusCheck_GetXYPos);
            this.baseGroupBox_LaserFocusCheck_PositionParameter.Controls.Add(this.textBox_LaserFocusCheck_AxisYStartPos);
            this.baseGroupBox_LaserFocusCheck_PositionParameter.Controls.Add(this.baseLabel_LaserFocusCheck_AxisYStartPos);
            this.baseGroupBox_LaserFocusCheck_PositionParameter.Controls.Add(this.textBox_LaserFocusCheck_AxisXStartPos);
            this.baseGroupBox_LaserFocusCheck_PositionParameter.Controls.Add(this.baseLabel_LaserFocusCheck_AxisXStartPos);
            this.baseGroupBox_LaserFocusCheck_PositionParameter.Controls.Add(this.baseLabel_LaserFocusCheck_AxisXYRangeSet);
            this.baseGroupBox_LaserFocusCheck_PositionParameter.Controls.Add(this.baseLabel_LaserFocusCheck_AxisZEndPos_AutoCalc);
            this.baseGroupBox_LaserFocusCheck_PositionParameter.Controls.Add(this.baseLabel_LaserFocusCheck_Arrow);
            this.baseGroupBox_LaserFocusCheck_PositionParameter.Controls.Add(this.button_LaserFocusCheck_GetZPos);
            this.baseGroupBox_LaserFocusCheck_PositionParameter.Controls.Add(this.textBox_LaserFocusCheck_AxisZEndPos);
            this.baseGroupBox_LaserFocusCheck_PositionParameter.Controls.Add(this.baseLabel_LaserFocusCheck_AxisZEndPos);
            this.baseGroupBox_LaserFocusCheck_PositionParameter.Controls.Add(this.textBox_LaserFocusCheck_AxisZStartPos);
            this.baseGroupBox_LaserFocusCheck_PositionParameter.Controls.Add(this.baseLabel_LaserFocusCheck_AxisZStartPos);
            this.baseGroupBox_LaserFocusCheck_PositionParameter.Controls.Add(this.baseLabel_LaserFocusCheck_AxisZRangeSet);
            this.baseGroupBox_LaserFocusCheck_PositionParameter.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_LaserFocusCheck_PositionParameter.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox_LaserFocusCheck_PositionParameter.Location = new System.Drawing.Point(10, 200);
            this.baseGroupBox_LaserFocusCheck_PositionParameter.Name = "baseGroupBox_LaserFocusCheck_PositionParameter";
            this.baseGroupBox_LaserFocusCheck_PositionParameter.Size = new System.Drawing.Size(501, 239);
            this.baseGroupBox_LaserFocusCheck_PositionParameter.TabIndex = 122;
            this.baseGroupBox_LaserFocusCheck_PositionParameter.TabStop = false;
            this.baseGroupBox_LaserFocusCheck_PositionParameter.Text = " ( Step - 2 )   Position Param. ";
            // 
            // button_LaserFocusCheck_ParameterVerification
            // 
            this.button_LaserFocusCheck_ParameterVerification.BackColor = System.Drawing.Color.White;
            this.button_LaserFocusCheck_ParameterVerification.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_LaserFocusCheck_ParameterVerification.FlatAppearance.BorderSize = 2;
            this.button_LaserFocusCheck_ParameterVerification.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_LaserFocusCheck_ParameterVerification.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_LaserFocusCheck_ParameterVerification.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_LaserFocusCheck_ParameterVerification.ForeColor = System.Drawing.Color.Black;
            this.button_LaserFocusCheck_ParameterVerification.Location = new System.Drawing.Point(315, 181);
            this.button_LaserFocusCheck_ParameterVerification.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_LaserFocusCheck_ParameterVerification.Name = "button_LaserFocusCheck_ParameterVerification";
            this.button_LaserFocusCheck_ParameterVerification.Size = new System.Drawing.Size(176, 46);
            this.button_LaserFocusCheck_ParameterVerification.TabIndex = 206;
            this.button_LaserFocusCheck_ParameterVerification.Text = "Parameter Verification";
            this.button_LaserFocusCheck_ParameterVerification.UseVisualStyleBackColor = false;
            // 
            // baseLabel_LaserFocusCheck_Arrow2
            // 
            this.baseLabel_LaserFocusCheck_Arrow2.AutoSize = true;
            this.baseLabel_LaserFocusCheck_Arrow2.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_LaserFocusCheck_Arrow2.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LaserFocusCheck_Arrow2.Location = new System.Drawing.Point(261, 110);
            this.baseLabel_LaserFocusCheck_Arrow2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LaserFocusCheck_Arrow2.Name = "baseLabel_LaserFocusCheck_Arrow2";
            this.baseLabel_LaserFocusCheck_Arrow2.Size = new System.Drawing.Size(50, 25);
            this.baseLabel_LaserFocusCheck_Arrow2.TabIndex = 205;
            this.baseLabel_LaserFocusCheck_Arrow2.Text = "▶▶";
            // 
            // button_LaserFocusCheck_GetXYPos
            // 
            this.button_LaserFocusCheck_GetXYPos.BackColor = System.Drawing.Color.White;
            this.button_LaserFocusCheck_GetXYPos.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_LaserFocusCheck_GetXYPos.FlatAppearance.BorderSize = 2;
            this.button_LaserFocusCheck_GetXYPos.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_LaserFocusCheck_GetXYPos.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_LaserFocusCheck_GetXYPos.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_LaserFocusCheck_GetXYPos.ForeColor = System.Drawing.Color.Black;
            this.button_LaserFocusCheck_GetXYPos.Location = new System.Drawing.Point(113, 101);
            this.button_LaserFocusCheck_GetXYPos.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_LaserFocusCheck_GetXYPos.Name = "button_LaserFocusCheck_GetXYPos";
            this.button_LaserFocusCheck_GetXYPos.Size = new System.Drawing.Size(142, 46);
            this.button_LaserFocusCheck_GetXYPos.TabIndex = 204;
            this.button_LaserFocusCheck_GetXYPos.Text = "Get  XY - Start Pos.";
            this.button_LaserFocusCheck_GetXYPos.UseVisualStyleBackColor = false;
            // 
            // textBox_LaserFocusCheck_AxisYStartPos
            // 
            this.textBox_LaserFocusCheck_AxisYStartPos.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_LaserFocusCheck_AxisYStartPos.Location = new System.Drawing.Point(419, 138);
            this.textBox_LaserFocusCheck_AxisYStartPos.Name = "textBox_LaserFocusCheck_AxisYStartPos";
            this.textBox_LaserFocusCheck_AxisYStartPos.Size = new System.Drawing.Size(72, 23);
            this.textBox_LaserFocusCheck_AxisYStartPos.TabIndex = 203;
            // 
            // baseLabel_LaserFocusCheck_AxisYStartPos
            // 
            this.baseLabel_LaserFocusCheck_AxisYStartPos.AutoSize = true;
            this.baseLabel_LaserFocusCheck_AxisYStartPos.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_LaserFocusCheck_AxisYStartPos.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LaserFocusCheck_AxisYStartPos.Location = new System.Drawing.Point(312, 141);
            this.baseLabel_LaserFocusCheck_AxisYStartPos.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LaserFocusCheck_AxisYStartPos.Name = "baseLabel_LaserFocusCheck_AxisYStartPos";
            this.baseLabel_LaserFocusCheck_AxisYStartPos.Size = new System.Drawing.Size(102, 16);
            this.baseLabel_LaserFocusCheck_AxisYStartPos.TabIndex = 202;
            this.baseLabel_LaserFocusCheck_AxisYStartPos.Text = "Axis-Y Start Pos.";
            // 
            // textBox_LaserFocusCheck_AxisXStartPos
            // 
            this.textBox_LaserFocusCheck_AxisXStartPos.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_LaserFocusCheck_AxisXStartPos.Location = new System.Drawing.Point(419, 112);
            this.textBox_LaserFocusCheck_AxisXStartPos.Name = "textBox_LaserFocusCheck_AxisXStartPos";
            this.textBox_LaserFocusCheck_AxisXStartPos.Size = new System.Drawing.Size(72, 23);
            this.textBox_LaserFocusCheck_AxisXStartPos.TabIndex = 201;
            // 
            // baseLabel_LaserFocusCheck_AxisXStartPos
            // 
            this.baseLabel_LaserFocusCheck_AxisXStartPos.AutoSize = true;
            this.baseLabel_LaserFocusCheck_AxisXStartPos.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_LaserFocusCheck_AxisXStartPos.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LaserFocusCheck_AxisXStartPos.Location = new System.Drawing.Point(312, 115);
            this.baseLabel_LaserFocusCheck_AxisXStartPos.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LaserFocusCheck_AxisXStartPos.Name = "baseLabel_LaserFocusCheck_AxisXStartPos";
            this.baseLabel_LaserFocusCheck_AxisXStartPos.Size = new System.Drawing.Size(103, 16);
            this.baseLabel_LaserFocusCheck_AxisXStartPos.TabIndex = 200;
            this.baseLabel_LaserFocusCheck_AxisXStartPos.Text = "Axis-X Start Pos.";
            // 
            // baseLabel_LaserFocusCheck_AxisXYRangeSet
            // 
            this.baseLabel_LaserFocusCheck_AxisXYRangeSet.AutoSize = true;
            this.baseLabel_LaserFocusCheck_AxisXYRangeSet.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_LaserFocusCheck_AxisXYRangeSet.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LaserFocusCheck_AxisXYRangeSet.Location = new System.Drawing.Point(7, 115);
            this.baseLabel_LaserFocusCheck_AxisXYRangeSet.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LaserFocusCheck_AxisXYRangeSet.Name = "baseLabel_LaserFocusCheck_AxisXYRangeSet";
            this.baseLabel_LaserFocusCheck_AxisXYRangeSet.Size = new System.Drawing.Size(98, 16);
            this.baseLabel_LaserFocusCheck_AxisXYRangeSet.TabIndex = 199;
            this.baseLabel_LaserFocusCheck_AxisXYRangeSet.Text = "XY - Range Set.";
            // 
            // baseLabel_LaserFocusCheck_AxisZEndPos_AutoCalc
            // 
            this.baseLabel_LaserFocusCheck_AxisZEndPos_AutoCalc.AutoSize = true;
            this.baseLabel_LaserFocusCheck_AxisZEndPos_AutoCalc.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_LaserFocusCheck_AxisZEndPos_AutoCalc.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LaserFocusCheck_AxisZEndPos_AutoCalc.Location = new System.Drawing.Point(311, 82);
            this.baseLabel_LaserFocusCheck_AxisZEndPos_AutoCalc.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LaserFocusCheck_AxisZEndPos_AutoCalc.Name = "baseLabel_LaserFocusCheck_AxisZEndPos_AutoCalc";
            this.baseLabel_LaserFocusCheck_AxisZEndPos_AutoCalc.Size = new System.Drawing.Size(75, 16);
            this.baseLabel_LaserFocusCheck_AxisZEndPos_AutoCalc.TabIndex = 198;
            this.baseLabel_LaserFocusCheck_AxisZEndPos_AutoCalc.Text = "(Auto Calc.)";
            // 
            // baseLabel_LaserFocusCheck_Arrow
            // 
            this.baseLabel_LaserFocusCheck_Arrow.AutoSize = true;
            this.baseLabel_LaserFocusCheck_Arrow.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_LaserFocusCheck_Arrow.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LaserFocusCheck_Arrow.Location = new System.Drawing.Point(261, 34);
            this.baseLabel_LaserFocusCheck_Arrow.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LaserFocusCheck_Arrow.Name = "baseLabel_LaserFocusCheck_Arrow";
            this.baseLabel_LaserFocusCheck_Arrow.Size = new System.Drawing.Size(50, 25);
            this.baseLabel_LaserFocusCheck_Arrow.TabIndex = 197;
            this.baseLabel_LaserFocusCheck_Arrow.Text = "▶▶";
            // 
            // button_LaserFocusCheck_GetZPos
            // 
            this.button_LaserFocusCheck_GetZPos.BackColor = System.Drawing.Color.White;
            this.button_LaserFocusCheck_GetZPos.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_LaserFocusCheck_GetZPos.FlatAppearance.BorderSize = 2;
            this.button_LaserFocusCheck_GetZPos.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_LaserFocusCheck_GetZPos.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_LaserFocusCheck_GetZPos.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_LaserFocusCheck_GetZPos.ForeColor = System.Drawing.Color.Black;
            this.button_LaserFocusCheck_GetZPos.Location = new System.Drawing.Point(113, 25);
            this.button_LaserFocusCheck_GetZPos.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_LaserFocusCheck_GetZPos.Name = "button_LaserFocusCheck_GetZPos";
            this.button_LaserFocusCheck_GetZPos.Size = new System.Drawing.Size(142, 46);
            this.button_LaserFocusCheck_GetZPos.TabIndex = 196;
            this.button_LaserFocusCheck_GetZPos.Text = "Get  Z - Start Pos.";
            this.button_LaserFocusCheck_GetZPos.UseVisualStyleBackColor = false;
            // 
            // textBox_LaserFocusCheck_AxisZEndPos
            // 
            this.textBox_LaserFocusCheck_AxisZEndPos.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_LaserFocusCheck_AxisZEndPos.Location = new System.Drawing.Point(419, 62);
            this.textBox_LaserFocusCheck_AxisZEndPos.Name = "textBox_LaserFocusCheck_AxisZEndPos";
            this.textBox_LaserFocusCheck_AxisZEndPos.ReadOnly = true;
            this.textBox_LaserFocusCheck_AxisZEndPos.Size = new System.Drawing.Size(72, 23);
            this.textBox_LaserFocusCheck_AxisZEndPos.TabIndex = 126;
            // 
            // baseLabel_LaserFocusCheck_AxisZEndPos
            // 
            this.baseLabel_LaserFocusCheck_AxisZEndPos.AutoSize = true;
            this.baseLabel_LaserFocusCheck_AxisZEndPos.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_LaserFocusCheck_AxisZEndPos.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LaserFocusCheck_AxisZEndPos.Location = new System.Drawing.Point(312, 65);
            this.baseLabel_LaserFocusCheck_AxisZEndPos.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LaserFocusCheck_AxisZEndPos.Name = "baseLabel_LaserFocusCheck_AxisZEndPos";
            this.baseLabel_LaserFocusCheck_AxisZEndPos.Size = new System.Drawing.Size(95, 16);
            this.baseLabel_LaserFocusCheck_AxisZEndPos.TabIndex = 125;
            this.baseLabel_LaserFocusCheck_AxisZEndPos.Text = "Axis-Z End Pos.";
            // 
            // textBox_LaserFocusCheck_AxisZStartPos
            // 
            this.textBox_LaserFocusCheck_AxisZStartPos.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_LaserFocusCheck_AxisZStartPos.Location = new System.Drawing.Point(419, 36);
            this.textBox_LaserFocusCheck_AxisZStartPos.Name = "textBox_LaserFocusCheck_AxisZStartPos";
            this.textBox_LaserFocusCheck_AxisZStartPos.Size = new System.Drawing.Size(72, 23);
            this.textBox_LaserFocusCheck_AxisZStartPos.TabIndex = 123;
            // 
            // baseLabel_LaserFocusCheck_AxisZStartPos
            // 
            this.baseLabel_LaserFocusCheck_AxisZStartPos.AutoSize = true;
            this.baseLabel_LaserFocusCheck_AxisZStartPos.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_LaserFocusCheck_AxisZStartPos.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LaserFocusCheck_AxisZStartPos.Location = new System.Drawing.Point(312, 39);
            this.baseLabel_LaserFocusCheck_AxisZStartPos.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LaserFocusCheck_AxisZStartPos.Name = "baseLabel_LaserFocusCheck_AxisZStartPos";
            this.baseLabel_LaserFocusCheck_AxisZStartPos.Size = new System.Drawing.Size(102, 16);
            this.baseLabel_LaserFocusCheck_AxisZStartPos.TabIndex = 122;
            this.baseLabel_LaserFocusCheck_AxisZStartPos.Text = "Axis-Z Start Pos.";
            // 
            // baseLabel_LaserFocusCheck_AxisZRangeSet
            // 
            this.baseLabel_LaserFocusCheck_AxisZRangeSet.AutoSize = true;
            this.baseLabel_LaserFocusCheck_AxisZRangeSet.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_LaserFocusCheck_AxisZRangeSet.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LaserFocusCheck_AxisZRangeSet.Location = new System.Drawing.Point(7, 39);
            this.baseLabel_LaserFocusCheck_AxisZRangeSet.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LaserFocusCheck_AxisZRangeSet.Name = "baseLabel_LaserFocusCheck_AxisZRangeSet";
            this.baseLabel_LaserFocusCheck_AxisZRangeSet.Size = new System.Drawing.Size(90, 16);
            this.baseLabel_LaserFocusCheck_AxisZRangeSet.TabIndex = 116;
            this.baseLabel_LaserFocusCheck_AxisZRangeSet.Text = "Z - Range Set.";
            // 
            // baseGroupBox_LaserFocusCheck_ShotParameter
            // 
            this.baseGroupBox_LaserFocusCheck_ShotParameter.Controls.Add(this.baseLabel_LaserFocusCheck_DrawLineNum_Unit);
            this.baseGroupBox_LaserFocusCheck_ShotParameter.Controls.Add(this.textBox_LaserFocusCheck_DrawLineNum);
            this.baseGroupBox_LaserFocusCheck_ShotParameter.Controls.Add(this.baseLabel_LaserFocusCheck_DrawLineNum);
            this.baseGroupBox_LaserFocusCheck_ShotParameter.Controls.Add(this.baseLabel_LaserFocusCheck_ShotPower_Unit);
            this.baseGroupBox_LaserFocusCheck_ShotParameter.Controls.Add(this.textBox_LaserFocusCheck_ShotPower);
            this.baseGroupBox_LaserFocusCheck_ShotParameter.Controls.Add(this.baseLabel_LaserFocusCheck_ShotPower);
            this.baseGroupBox_LaserFocusCheck_ShotParameter.Controls.Add(this.radioButton_LaserFocusCheck_DrawDirection_Ver);
            this.baseGroupBox_LaserFocusCheck_ShotParameter.Controls.Add(this.radioButton_LaserFocusCheck_DrawDirection_Hor);
            this.baseGroupBox_LaserFocusCheck_ShotParameter.Controls.Add(this.baseLabel_LaserFocusCheck_DrawDirection);
            this.baseGroupBox_LaserFocusCheck_ShotParameter.Controls.Add(this.baseLabel_LaserFocusCheck_DrawLength_Unit);
            this.baseGroupBox_LaserFocusCheck_ShotParameter.Controls.Add(this.textBox_LaserFocusCheck_DrawLength);
            this.baseGroupBox_LaserFocusCheck_ShotParameter.Controls.Add(this.baseLabel_LaserFocusCheck_DrawLength);
            this.baseGroupBox_LaserFocusCheck_ShotParameter.Controls.Add(this.baseLabel_LaserFocusCheck_DrawSpeed_Unit);
            this.baseGroupBox_LaserFocusCheck_ShotParameter.Controls.Add(this.textBox_LaserFocusCheck_DrawSpeed);
            this.baseGroupBox_LaserFocusCheck_ShotParameter.Controls.Add(this.baseLabel_LaserFocusCheck_DrawSpeed);
            this.baseGroupBox_LaserFocusCheck_ShotParameter.Controls.Add(this.baseLabel_LaserFocusCheck_MovePitchXY_Unit);
            this.baseGroupBox_LaserFocusCheck_ShotParameter.Controls.Add(this.textBox_LaserFocusCheck_MovePitchXY);
            this.baseGroupBox_LaserFocusCheck_ShotParameter.Controls.Add(this.baseLabel_LaserFocusCheck_MovePitchXY);
            this.baseGroupBox_LaserFocusCheck_ShotParameter.Controls.Add(this.baseLabel_LaserFocusCheck_MovePitchZ_Unit);
            this.baseGroupBox_LaserFocusCheck_ShotParameter.Controls.Add(this.textBox_LaserFocusCheck_MovePitchZ);
            this.baseGroupBox_LaserFocusCheck_ShotParameter.Controls.Add(this.baseLabel_LaserFocusCheck_MovePitchZ);
            this.baseGroupBox_LaserFocusCheck_ShotParameter.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_LaserFocusCheck_ShotParameter.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox_LaserFocusCheck_ShotParameter.Location = new System.Drawing.Point(10, 31);
            this.baseGroupBox_LaserFocusCheck_ShotParameter.Name = "baseGroupBox_LaserFocusCheck_ShotParameter";
            this.baseGroupBox_LaserFocusCheck_ShotParameter.Size = new System.Drawing.Size(501, 150);
            this.baseGroupBox_LaserFocusCheck_ShotParameter.TabIndex = 121;
            this.baseGroupBox_LaserFocusCheck_ShotParameter.TabStop = false;
            this.baseGroupBox_LaserFocusCheck_ShotParameter.Text = " ( Step - 1 )   Laser Drawing Param. ";
            // 
            // baseLabel_LaserFocusCheck_DrawLineNum_Unit
            // 
            this.baseLabel_LaserFocusCheck_DrawLineNum_Unit.AutoSize = true;
            this.baseLabel_LaserFocusCheck_DrawLineNum_Unit.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_LaserFocusCheck_DrawLineNum_Unit.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LaserFocusCheck_DrawLineNum_Unit.Location = new System.Drawing.Point(454, 90);
            this.baseLabel_LaserFocusCheck_DrawLineNum_Unit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LaserFocusCheck_DrawLineNum_Unit.Name = "baseLabel_LaserFocusCheck_DrawLineNum_Unit";
            this.baseLabel_LaserFocusCheck_DrawLineNum_Unit.Size = new System.Drawing.Size(22, 16);
            this.baseLabel_LaserFocusCheck_DrawLineNum_Unit.TabIndex = 136;
            this.baseLabel_LaserFocusCheck_DrawLineNum_Unit.Text = "EA";
            // 
            // textBox_LaserFocusCheck_DrawLineNum
            // 
            this.textBox_LaserFocusCheck_DrawLineNum.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_LaserFocusCheck_DrawLineNum.Location = new System.Drawing.Point(380, 86);
            this.textBox_LaserFocusCheck_DrawLineNum.Name = "textBox_LaserFocusCheck_DrawLineNum";
            this.textBox_LaserFocusCheck_DrawLineNum.Size = new System.Drawing.Size(72, 23);
            this.textBox_LaserFocusCheck_DrawLineNum.TabIndex = 135;
            // 
            // baseLabel_LaserFocusCheck_DrawLineNum
            // 
            this.baseLabel_LaserFocusCheck_DrawLineNum.AutoSize = true;
            this.baseLabel_LaserFocusCheck_DrawLineNum.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_LaserFocusCheck_DrawLineNum.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LaserFocusCheck_DrawLineNum.Location = new System.Drawing.Point(276, 89);
            this.baseLabel_LaserFocusCheck_DrawLineNum.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LaserFocusCheck_DrawLineNum.Name = "baseLabel_LaserFocusCheck_DrawLineNum";
            this.baseLabel_LaserFocusCheck_DrawLineNum.Size = new System.Drawing.Size(94, 16);
            this.baseLabel_LaserFocusCheck_DrawLineNum.TabIndex = 134;
            this.baseLabel_LaserFocusCheck_DrawLineNum.Text = "Draw Line Num";
            // 
            // baseLabel_LaserFocusCheck_ShotPower_Unit
            // 
            this.baseLabel_LaserFocusCheck_ShotPower_Unit.AutoSize = true;
            this.baseLabel_LaserFocusCheck_ShotPower_Unit.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_LaserFocusCheck_ShotPower_Unit.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LaserFocusCheck_ShotPower_Unit.Location = new System.Drawing.Point(187, 121);
            this.baseLabel_LaserFocusCheck_ShotPower_Unit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LaserFocusCheck_ShotPower_Unit.Name = "baseLabel_LaserFocusCheck_ShotPower_Unit";
            this.baseLabel_LaserFocusCheck_ShotPower_Unit.Size = new System.Drawing.Size(21, 16);
            this.baseLabel_LaserFocusCheck_ShotPower_Unit.TabIndex = 133;
            this.baseLabel_LaserFocusCheck_ShotPower_Unit.Text = "％";
            // 
            // textBox_LaserFocusCheck_ShotPower
            // 
            this.textBox_LaserFocusCheck_ShotPower.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_LaserFocusCheck_ShotPower.Location = new System.Drawing.Point(113, 117);
            this.textBox_LaserFocusCheck_ShotPower.Name = "textBox_LaserFocusCheck_ShotPower";
            this.textBox_LaserFocusCheck_ShotPower.Size = new System.Drawing.Size(72, 23);
            this.textBox_LaserFocusCheck_ShotPower.TabIndex = 132;
            // 
            // baseLabel_LaserFocusCheck_ShotPower
            // 
            this.baseLabel_LaserFocusCheck_ShotPower.AutoSize = true;
            this.baseLabel_LaserFocusCheck_ShotPower.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_LaserFocusCheck_ShotPower.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LaserFocusCheck_ShotPower.Location = new System.Drawing.Point(7, 120);
            this.baseLabel_LaserFocusCheck_ShotPower.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LaserFocusCheck_ShotPower.Name = "baseLabel_LaserFocusCheck_ShotPower";
            this.baseLabel_LaserFocusCheck_ShotPower.Size = new System.Drawing.Size(73, 16);
            this.baseLabel_LaserFocusCheck_ShotPower.TabIndex = 131;
            this.baseLabel_LaserFocusCheck_ShotPower.Text = "Shot Power";
            // 
            // radioButton_LaserFocusCheck_DrawDirection_Ver
            // 
            this.radioButton_LaserFocusCheck_DrawDirection_Ver.AutoSize = true;
            this.radioButton_LaserFocusCheck_DrawDirection_Ver.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton_LaserFocusCheck_DrawDirection_Ver.Location = new System.Drawing.Point(171, 88);
            this.radioButton_LaserFocusCheck_DrawDirection_Ver.Name = "radioButton_LaserFocusCheck_DrawDirection_Ver";
            this.radioButton_LaserFocusCheck_DrawDirection_Ver.Size = new System.Drawing.Size(49, 20);
            this.radioButton_LaserFocusCheck_DrawDirection_Ver.TabIndex = 130;
            this.radioButton_LaserFocusCheck_DrawDirection_Ver.TabStop = true;
            this.radioButton_LaserFocusCheck_DrawDirection_Ver.Text = "Ver.";
            this.radioButton_LaserFocusCheck_DrawDirection_Ver.UseVisualStyleBackColor = true;
            // 
            // radioButton_LaserFocusCheck_DrawDirection_Hor
            // 
            this.radioButton_LaserFocusCheck_DrawDirection_Hor.AutoSize = true;
            this.radioButton_LaserFocusCheck_DrawDirection_Hor.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton_LaserFocusCheck_DrawDirection_Hor.Location = new System.Drawing.Point(118, 88);
            this.radioButton_LaserFocusCheck_DrawDirection_Hor.Name = "radioButton_LaserFocusCheck_DrawDirection_Hor";
            this.radioButton_LaserFocusCheck_DrawDirection_Hor.Size = new System.Drawing.Size(49, 20);
            this.radioButton_LaserFocusCheck_DrawDirection_Hor.TabIndex = 129;
            this.radioButton_LaserFocusCheck_DrawDirection_Hor.TabStop = true;
            this.radioButton_LaserFocusCheck_DrawDirection_Hor.Text = "Hor.";
            this.radioButton_LaserFocusCheck_DrawDirection_Hor.UseVisualStyleBackColor = true;
            // 
            // baseLabel_LaserFocusCheck_DrawDirection
            // 
            this.baseLabel_LaserFocusCheck_DrawDirection.AutoSize = true;
            this.baseLabel_LaserFocusCheck_DrawDirection.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_LaserFocusCheck_DrawDirection.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LaserFocusCheck_DrawDirection.Location = new System.Drawing.Point(7, 89);
            this.baseLabel_LaserFocusCheck_DrawDirection.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LaserFocusCheck_DrawDirection.Name = "baseLabel_LaserFocusCheck_DrawDirection";
            this.baseLabel_LaserFocusCheck_DrawDirection.Size = new System.Drawing.Size(91, 16);
            this.baseLabel_LaserFocusCheck_DrawDirection.TabIndex = 128;
            this.baseLabel_LaserFocusCheck_DrawDirection.Text = "Draw Direction";
            // 
            // baseLabel_LaserFocusCheck_DrawLength_Unit
            // 
            this.baseLabel_LaserFocusCheck_DrawLength_Unit.AutoSize = true;
            this.baseLabel_LaserFocusCheck_DrawLength_Unit.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_LaserFocusCheck_DrawLength_Unit.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LaserFocusCheck_DrawLength_Unit.Location = new System.Drawing.Point(454, 59);
            this.baseLabel_LaserFocusCheck_DrawLength_Unit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LaserFocusCheck_DrawLength_Unit.Name = "baseLabel_LaserFocusCheck_DrawLength_Unit";
            this.baseLabel_LaserFocusCheck_DrawLength_Unit.Size = new System.Drawing.Size(22, 16);
            this.baseLabel_LaserFocusCheck_DrawLength_Unit.TabIndex = 127;
            this.baseLabel_LaserFocusCheck_DrawLength_Unit.Text = "㎜";
            // 
            // textBox_LaserFocusCheck_DrawLength
            // 
            this.textBox_LaserFocusCheck_DrawLength.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_LaserFocusCheck_DrawLength.Location = new System.Drawing.Point(380, 55);
            this.textBox_LaserFocusCheck_DrawLength.Name = "textBox_LaserFocusCheck_DrawLength";
            this.textBox_LaserFocusCheck_DrawLength.Size = new System.Drawing.Size(72, 23);
            this.textBox_LaserFocusCheck_DrawLength.TabIndex = 126;
            // 
            // baseLabel_LaserFocusCheck_DrawLength
            // 
            this.baseLabel_LaserFocusCheck_DrawLength.AutoSize = true;
            this.baseLabel_LaserFocusCheck_DrawLength.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_LaserFocusCheck_DrawLength.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LaserFocusCheck_DrawLength.Location = new System.Drawing.Point(276, 58);
            this.baseLabel_LaserFocusCheck_DrawLength.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LaserFocusCheck_DrawLength.Name = "baseLabel_LaserFocusCheck_DrawLength";
            this.baseLabel_LaserFocusCheck_DrawLength.Size = new System.Drawing.Size(79, 16);
            this.baseLabel_LaserFocusCheck_DrawLength.TabIndex = 125;
            this.baseLabel_LaserFocusCheck_DrawLength.Text = "Draw Length";
            // 
            // baseLabel_LaserFocusCheck_DrawSpeed_Unit
            // 
            this.baseLabel_LaserFocusCheck_DrawSpeed_Unit.AutoSize = true;
            this.baseLabel_LaserFocusCheck_DrawSpeed_Unit.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_LaserFocusCheck_DrawSpeed_Unit.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LaserFocusCheck_DrawSpeed_Unit.Location = new System.Drawing.Point(454, 28);
            this.baseLabel_LaserFocusCheck_DrawSpeed_Unit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LaserFocusCheck_DrawSpeed_Unit.Name = "baseLabel_LaserFocusCheck_DrawSpeed_Unit";
            this.baseLabel_LaserFocusCheck_DrawSpeed_Unit.Size = new System.Drawing.Size(33, 16);
            this.baseLabel_LaserFocusCheck_DrawSpeed_Unit.TabIndex = 124;
            this.baseLabel_LaserFocusCheck_DrawSpeed_Unit.Text = "㎜/s";
            // 
            // textBox_LaserFocusCheck_DrawSpeed
            // 
            this.textBox_LaserFocusCheck_DrawSpeed.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_LaserFocusCheck_DrawSpeed.Location = new System.Drawing.Point(380, 24);
            this.textBox_LaserFocusCheck_DrawSpeed.Name = "textBox_LaserFocusCheck_DrawSpeed";
            this.textBox_LaserFocusCheck_DrawSpeed.Size = new System.Drawing.Size(72, 23);
            this.textBox_LaserFocusCheck_DrawSpeed.TabIndex = 123;
            // 
            // baseLabel_LaserFocusCheck_DrawSpeed
            // 
            this.baseLabel_LaserFocusCheck_DrawSpeed.AutoSize = true;
            this.baseLabel_LaserFocusCheck_DrawSpeed.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_LaserFocusCheck_DrawSpeed.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LaserFocusCheck_DrawSpeed.Location = new System.Drawing.Point(276, 27);
            this.baseLabel_LaserFocusCheck_DrawSpeed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LaserFocusCheck_DrawSpeed.Name = "baseLabel_LaserFocusCheck_DrawSpeed";
            this.baseLabel_LaserFocusCheck_DrawSpeed.Size = new System.Drawing.Size(77, 16);
            this.baseLabel_LaserFocusCheck_DrawSpeed.TabIndex = 122;
            this.baseLabel_LaserFocusCheck_DrawSpeed.Text = "Draw Speed";
            // 
            // baseLabel_LaserFocusCheck_MovePitchXY_Unit
            // 
            this.baseLabel_LaserFocusCheck_MovePitchXY_Unit.AutoSize = true;
            this.baseLabel_LaserFocusCheck_MovePitchXY_Unit.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_LaserFocusCheck_MovePitchXY_Unit.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LaserFocusCheck_MovePitchXY_Unit.Location = new System.Drawing.Point(187, 59);
            this.baseLabel_LaserFocusCheck_MovePitchXY_Unit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LaserFocusCheck_MovePitchXY_Unit.Name = "baseLabel_LaserFocusCheck_MovePitchXY_Unit";
            this.baseLabel_LaserFocusCheck_MovePitchXY_Unit.Size = new System.Drawing.Size(22, 16);
            this.baseLabel_LaserFocusCheck_MovePitchXY_Unit.TabIndex = 121;
            this.baseLabel_LaserFocusCheck_MovePitchXY_Unit.Text = "㎜";
            // 
            // textBox_LaserFocusCheck_MovePitchXY
            // 
            this.textBox_LaserFocusCheck_MovePitchXY.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_LaserFocusCheck_MovePitchXY.Location = new System.Drawing.Point(113, 55);
            this.textBox_LaserFocusCheck_MovePitchXY.Name = "textBox_LaserFocusCheck_MovePitchXY";
            this.textBox_LaserFocusCheck_MovePitchXY.Size = new System.Drawing.Size(72, 23);
            this.textBox_LaserFocusCheck_MovePitchXY.TabIndex = 120;
            // 
            // baseLabel_LaserFocusCheck_MovePitchXY
            // 
            this.baseLabel_LaserFocusCheck_MovePitchXY.AutoSize = true;
            this.baseLabel_LaserFocusCheck_MovePitchXY.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_LaserFocusCheck_MovePitchXY.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LaserFocusCheck_MovePitchXY.Location = new System.Drawing.Point(7, 58);
            this.baseLabel_LaserFocusCheck_MovePitchXY.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LaserFocusCheck_MovePitchXY.Name = "baseLabel_LaserFocusCheck_MovePitchXY";
            this.baseLabel_LaserFocusCheck_MovePitchXY.Size = new System.Drawing.Size(96, 16);
            this.baseLabel_LaserFocusCheck_MovePitchXY.TabIndex = 119;
            this.baseLabel_LaserFocusCheck_MovePitchXY.Text = "Move Pitch - XY";
            // 
            // baseLabel_LaserFocusCheck_MovePitchZ_Unit
            // 
            this.baseLabel_LaserFocusCheck_MovePitchZ_Unit.AutoSize = true;
            this.baseLabel_LaserFocusCheck_MovePitchZ_Unit.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_LaserFocusCheck_MovePitchZ_Unit.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LaserFocusCheck_MovePitchZ_Unit.Location = new System.Drawing.Point(187, 28);
            this.baseLabel_LaserFocusCheck_MovePitchZ_Unit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LaserFocusCheck_MovePitchZ_Unit.Name = "baseLabel_LaserFocusCheck_MovePitchZ_Unit";
            this.baseLabel_LaserFocusCheck_MovePitchZ_Unit.Size = new System.Drawing.Size(22, 16);
            this.baseLabel_LaserFocusCheck_MovePitchZ_Unit.TabIndex = 118;
            this.baseLabel_LaserFocusCheck_MovePitchZ_Unit.Text = "㎜";
            // 
            // textBox_LaserFocusCheck_MovePitchZ
            // 
            this.textBox_LaserFocusCheck_MovePitchZ.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_LaserFocusCheck_MovePitchZ.Location = new System.Drawing.Point(113, 24);
            this.textBox_LaserFocusCheck_MovePitchZ.Name = "textBox_LaserFocusCheck_MovePitchZ";
            this.textBox_LaserFocusCheck_MovePitchZ.Size = new System.Drawing.Size(72, 23);
            this.textBox_LaserFocusCheck_MovePitchZ.TabIndex = 117;
            // 
            // baseLabel_LaserFocusCheck_MovePitchZ
            // 
            this.baseLabel_LaserFocusCheck_MovePitchZ.AutoSize = true;
            this.baseLabel_LaserFocusCheck_MovePitchZ.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_LaserFocusCheck_MovePitchZ.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LaserFocusCheck_MovePitchZ.Location = new System.Drawing.Point(7, 27);
            this.baseLabel_LaserFocusCheck_MovePitchZ.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LaserFocusCheck_MovePitchZ.Name = "baseLabel_LaserFocusCheck_MovePitchZ";
            this.baseLabel_LaserFocusCheck_MovePitchZ.Size = new System.Drawing.Size(88, 16);
            this.baseLabel_LaserFocusCheck_MovePitchZ.TabIndex = 116;
            this.baseLabel_LaserFocusCheck_MovePitchZ.Text = "Move Pitch - Z";
            // 
            // tabPage_DrillingParameter
            // 
            this.tabPage_DrillingParameter.Controls.Add(this.baseGroupBox_DrillingToolParam);
            this.tabPage_DrillingParameter.Controls.Add(this.baseGroupBox_DrillingToolList);
            this.tabPage_DrillingParameter.Location = new System.Drawing.Point(4, 36);
            this.tabPage_DrillingParameter.Name = "tabPage_DrillingParameter";
            this.tabPage_DrillingParameter.Size = new System.Drawing.Size(1130, 724);
            this.tabPage_DrillingParameter.TabIndex = 6;
            this.tabPage_DrillingParameter.Text = "Drilling Param.";
            this.tabPage_DrillingParameter.UseVisualStyleBackColor = true;
            // 
            // baseGroupBox_DrillingToolParam
            // 
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_LaserOffDelay_Unit);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.textBox_DrillingTool_LaserOffDelay);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_LaserOffDelay);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_LaserOnDelay_Unit);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.textBox_DrillingTool_LaserOnDelay);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_LaserOnDelay);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_JumpDelay_Unit);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.textBox_DrillingTool_JumpDelay);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_JumpDelay);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_MarkDelay_Unit);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.textBox_DrillingTool_MarkDelay);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_MarkDelay);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_JumpSpeed_Unit);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.textBox_DrillingTool_JumpSpeed);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_JumpSpeed);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_MarkSpeed_Unit);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.textBox_DrillingTool_MarkSpeed);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_MarkSpeed);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_Freq_Unit);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.textBox_DrillingTool_Frequency);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_Freq);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_ZOffset_Unit);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.textBox_DrillingTool_ZOffset);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_ZOffset);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.textBox_DrillingTool_RepeatCount);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_RepeatCount);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_HoleSize_Unit);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.textBox_DrillingTool_HoleSize);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_HoleSize);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.comboBox_DrillingTool_MaskNo);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_MaskNo);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.comboBox_DrillingTool_Type);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_Type);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.textBox_DrillingTool_No);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_No);
            this.baseGroupBox_DrillingToolParam.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_DrillingToolParam.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox_DrillingToolParam.Location = new System.Drawing.Point(295, 11);
            this.baseGroupBox_DrillingToolParam.Name = "baseGroupBox_DrillingToolParam";
            this.baseGroupBox_DrillingToolParam.Size = new System.Drawing.Size(541, 502);
            this.baseGroupBox_DrillingToolParam.TabIndex = 10;
            this.baseGroupBox_DrillingToolParam.TabStop = false;
            this.baseGroupBox_DrillingToolParam.Text = " [ Drilling Tool Parameter ] ";
            // 
            // baseLabel_DrillingTool_LaserOffDelay_Unit
            // 
            this.baseLabel_DrillingTool_LaserOffDelay_Unit.AutoSize = true;
            this.baseLabel_DrillingTool_LaserOffDelay_Unit.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_LaserOffDelay_Unit.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_LaserOffDelay_Unit.Location = new System.Drawing.Point(487, 222);
            this.baseLabel_DrillingTool_LaserOffDelay_Unit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_LaserOffDelay_Unit.Name = "baseLabel_DrillingTool_LaserOffDelay_Unit";
            this.baseLabel_DrillingTool_LaserOffDelay_Unit.Size = new System.Drawing.Size(23, 18);
            this.baseLabel_DrillingTool_LaserOffDelay_Unit.TabIndex = 153;
            this.baseLabel_DrillingTool_LaserOffDelay_Unit.Text = "㎲";
            // 
            // textBox_DrillingTool_LaserOffDelay
            // 
            this.textBox_DrillingTool_LaserOffDelay.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_DrillingTool_LaserOffDelay.Location = new System.Drawing.Point(393, 219);
            this.textBox_DrillingTool_LaserOffDelay.Name = "textBox_DrillingTool_LaserOffDelay";
            this.textBox_DrillingTool_LaserOffDelay.Size = new System.Drawing.Size(92, 26);
            this.textBox_DrillingTool_LaserOffDelay.TabIndex = 152;
            // 
            // baseLabel_DrillingTool_LaserOffDelay
            // 
            this.baseLabel_DrillingTool_LaserOffDelay.AutoSize = true;
            this.baseLabel_DrillingTool_LaserOffDelay.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_LaserOffDelay.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_LaserOffDelay.Location = new System.Drawing.Point(281, 222);
            this.baseLabel_DrillingTool_LaserOffDelay.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_LaserOffDelay.Name = "baseLabel_DrillingTool_LaserOffDelay";
            this.baseLabel_DrillingTool_LaserOffDelay.Size = new System.Drawing.Size(110, 18);
            this.baseLabel_DrillingTool_LaserOffDelay.TabIndex = 151;
            this.baseLabel_DrillingTool_LaserOffDelay.Text = "Laser Off Delay";
            // 
            // baseLabel_DrillingTool_LaserOnDelay_Unit
            // 
            this.baseLabel_DrillingTool_LaserOnDelay_Unit.AutoSize = true;
            this.baseLabel_DrillingTool_LaserOnDelay_Unit.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_LaserOnDelay_Unit.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_LaserOnDelay_Unit.Location = new System.Drawing.Point(487, 191);
            this.baseLabel_DrillingTool_LaserOnDelay_Unit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_LaserOnDelay_Unit.Name = "baseLabel_DrillingTool_LaserOnDelay_Unit";
            this.baseLabel_DrillingTool_LaserOnDelay_Unit.Size = new System.Drawing.Size(23, 18);
            this.baseLabel_DrillingTool_LaserOnDelay_Unit.TabIndex = 150;
            this.baseLabel_DrillingTool_LaserOnDelay_Unit.Text = "㎲";
            // 
            // textBox_DrillingTool_LaserOnDelay
            // 
            this.textBox_DrillingTool_LaserOnDelay.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_DrillingTool_LaserOnDelay.Location = new System.Drawing.Point(393, 188);
            this.textBox_DrillingTool_LaserOnDelay.Name = "textBox_DrillingTool_LaserOnDelay";
            this.textBox_DrillingTool_LaserOnDelay.Size = new System.Drawing.Size(92, 26);
            this.textBox_DrillingTool_LaserOnDelay.TabIndex = 149;
            // 
            // baseLabel_DrillingTool_LaserOnDelay
            // 
            this.baseLabel_DrillingTool_LaserOnDelay.AutoSize = true;
            this.baseLabel_DrillingTool_LaserOnDelay.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_LaserOnDelay.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_LaserOnDelay.Location = new System.Drawing.Point(281, 191);
            this.baseLabel_DrillingTool_LaserOnDelay.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_LaserOnDelay.Name = "baseLabel_DrillingTool_LaserOnDelay";
            this.baseLabel_DrillingTool_LaserOnDelay.Size = new System.Drawing.Size(108, 18);
            this.baseLabel_DrillingTool_LaserOnDelay.TabIndex = 148;
            this.baseLabel_DrillingTool_LaserOnDelay.Text = "Laser On Delay";
            // 
            // baseLabel_DrillingTool_JumpDelay_Unit
            // 
            this.baseLabel_DrillingTool_JumpDelay_Unit.AutoSize = true;
            this.baseLabel_DrillingTool_JumpDelay_Unit.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_JumpDelay_Unit.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_JumpDelay_Unit.Location = new System.Drawing.Point(487, 160);
            this.baseLabel_DrillingTool_JumpDelay_Unit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_JumpDelay_Unit.Name = "baseLabel_DrillingTool_JumpDelay_Unit";
            this.baseLabel_DrillingTool_JumpDelay_Unit.Size = new System.Drawing.Size(23, 18);
            this.baseLabel_DrillingTool_JumpDelay_Unit.TabIndex = 147;
            this.baseLabel_DrillingTool_JumpDelay_Unit.Text = "㎲";
            // 
            // textBox_DrillingTool_JumpDelay
            // 
            this.textBox_DrillingTool_JumpDelay.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_DrillingTool_JumpDelay.Location = new System.Drawing.Point(393, 157);
            this.textBox_DrillingTool_JumpDelay.Name = "textBox_DrillingTool_JumpDelay";
            this.textBox_DrillingTool_JumpDelay.Size = new System.Drawing.Size(92, 26);
            this.textBox_DrillingTool_JumpDelay.TabIndex = 146;
            // 
            // baseLabel_DrillingTool_JumpDelay
            // 
            this.baseLabel_DrillingTool_JumpDelay.AutoSize = true;
            this.baseLabel_DrillingTool_JumpDelay.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_JumpDelay.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_JumpDelay.Location = new System.Drawing.Point(281, 160);
            this.baseLabel_DrillingTool_JumpDelay.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_JumpDelay.Name = "baseLabel_DrillingTool_JumpDelay";
            this.baseLabel_DrillingTool_JumpDelay.Size = new System.Drawing.Size(84, 18);
            this.baseLabel_DrillingTool_JumpDelay.TabIndex = 145;
            this.baseLabel_DrillingTool_JumpDelay.Text = "Jump Delay";
            // 
            // baseLabel_DrillingTool_MarkDelay_Unit
            // 
            this.baseLabel_DrillingTool_MarkDelay_Unit.AutoSize = true;
            this.baseLabel_DrillingTool_MarkDelay_Unit.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_MarkDelay_Unit.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_MarkDelay_Unit.Location = new System.Drawing.Point(487, 129);
            this.baseLabel_DrillingTool_MarkDelay_Unit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_MarkDelay_Unit.Name = "baseLabel_DrillingTool_MarkDelay_Unit";
            this.baseLabel_DrillingTool_MarkDelay_Unit.Size = new System.Drawing.Size(23, 18);
            this.baseLabel_DrillingTool_MarkDelay_Unit.TabIndex = 144;
            this.baseLabel_DrillingTool_MarkDelay_Unit.Text = "㎲";
            // 
            // textBox_DrillingTool_MarkDelay
            // 
            this.textBox_DrillingTool_MarkDelay.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_DrillingTool_MarkDelay.Location = new System.Drawing.Point(393, 126);
            this.textBox_DrillingTool_MarkDelay.Name = "textBox_DrillingTool_MarkDelay";
            this.textBox_DrillingTool_MarkDelay.Size = new System.Drawing.Size(92, 26);
            this.textBox_DrillingTool_MarkDelay.TabIndex = 143;
            // 
            // baseLabel_DrillingTool_MarkDelay
            // 
            this.baseLabel_DrillingTool_MarkDelay.AutoSize = true;
            this.baseLabel_DrillingTool_MarkDelay.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_MarkDelay.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_MarkDelay.Location = new System.Drawing.Point(281, 129);
            this.baseLabel_DrillingTool_MarkDelay.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_MarkDelay.Name = "baseLabel_DrillingTool_MarkDelay";
            this.baseLabel_DrillingTool_MarkDelay.Size = new System.Drawing.Size(81, 18);
            this.baseLabel_DrillingTool_MarkDelay.TabIndex = 142;
            this.baseLabel_DrillingTool_MarkDelay.Text = "Mark Delay";
            // 
            // baseLabel_DrillingTool_JumpSpeed_Unit
            // 
            this.baseLabel_DrillingTool_JumpSpeed_Unit.AutoSize = true;
            this.baseLabel_DrillingTool_JumpSpeed_Unit.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_JumpSpeed_Unit.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_JumpSpeed_Unit.Location = new System.Drawing.Point(487, 98);
            this.baseLabel_DrillingTool_JumpSpeed_Unit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_JumpSpeed_Unit.Name = "baseLabel_DrillingTool_JumpSpeed_Unit";
            this.baseLabel_DrillingTool_JumpSpeed_Unit.Size = new System.Drawing.Size(47, 18);
            this.baseLabel_DrillingTool_JumpSpeed_Unit.TabIndex = 141;
            this.baseLabel_DrillingTool_JumpSpeed_Unit.Text = "mm/s";
            // 
            // textBox_DrillingTool_JumpSpeed
            // 
            this.textBox_DrillingTool_JumpSpeed.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_DrillingTool_JumpSpeed.Location = new System.Drawing.Point(393, 95);
            this.textBox_DrillingTool_JumpSpeed.Name = "textBox_DrillingTool_JumpSpeed";
            this.textBox_DrillingTool_JumpSpeed.Size = new System.Drawing.Size(92, 26);
            this.textBox_DrillingTool_JumpSpeed.TabIndex = 140;
            // 
            // baseLabel_DrillingTool_JumpSpeed
            // 
            this.baseLabel_DrillingTool_JumpSpeed.AutoSize = true;
            this.baseLabel_DrillingTool_JumpSpeed.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_JumpSpeed.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_JumpSpeed.Location = new System.Drawing.Point(281, 98);
            this.baseLabel_DrillingTool_JumpSpeed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_JumpSpeed.Name = "baseLabel_DrillingTool_JumpSpeed";
            this.baseLabel_DrillingTool_JumpSpeed.Size = new System.Drawing.Size(88, 18);
            this.baseLabel_DrillingTool_JumpSpeed.TabIndex = 139;
            this.baseLabel_DrillingTool_JumpSpeed.Text = "Jump Speed";
            // 
            // baseLabel_DrillingTool_MarkSpeed_Unit
            // 
            this.baseLabel_DrillingTool_MarkSpeed_Unit.AutoSize = true;
            this.baseLabel_DrillingTool_MarkSpeed_Unit.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_MarkSpeed_Unit.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_MarkSpeed_Unit.Location = new System.Drawing.Point(487, 67);
            this.baseLabel_DrillingTool_MarkSpeed_Unit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_MarkSpeed_Unit.Name = "baseLabel_DrillingTool_MarkSpeed_Unit";
            this.baseLabel_DrillingTool_MarkSpeed_Unit.Size = new System.Drawing.Size(47, 18);
            this.baseLabel_DrillingTool_MarkSpeed_Unit.TabIndex = 138;
            this.baseLabel_DrillingTool_MarkSpeed_Unit.Text = "mm/s";
            // 
            // textBox_DrillingTool_MarkSpeed
            // 
            this.textBox_DrillingTool_MarkSpeed.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_DrillingTool_MarkSpeed.Location = new System.Drawing.Point(393, 64);
            this.textBox_DrillingTool_MarkSpeed.Name = "textBox_DrillingTool_MarkSpeed";
            this.textBox_DrillingTool_MarkSpeed.Size = new System.Drawing.Size(92, 26);
            this.textBox_DrillingTool_MarkSpeed.TabIndex = 137;
            // 
            // baseLabel_DrillingTool_MarkSpeed
            // 
            this.baseLabel_DrillingTool_MarkSpeed.AutoSize = true;
            this.baseLabel_DrillingTool_MarkSpeed.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_MarkSpeed.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_MarkSpeed.Location = new System.Drawing.Point(281, 67);
            this.baseLabel_DrillingTool_MarkSpeed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_MarkSpeed.Name = "baseLabel_DrillingTool_MarkSpeed";
            this.baseLabel_DrillingTool_MarkSpeed.Size = new System.Drawing.Size(85, 18);
            this.baseLabel_DrillingTool_MarkSpeed.TabIndex = 136;
            this.baseLabel_DrillingTool_MarkSpeed.Text = "Mark Speed";
            // 
            // baseLabel_DrillingTool_Freq_Unit
            // 
            this.baseLabel_DrillingTool_Freq_Unit.AutoSize = true;
            this.baseLabel_DrillingTool_Freq_Unit.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_Freq_Unit.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_Freq_Unit.Location = new System.Drawing.Point(487, 36);
            this.baseLabel_DrillingTool_Freq_Unit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_Freq_Unit.Name = "baseLabel_DrillingTool_Freq_Unit";
            this.baseLabel_DrillingTool_Freq_Unit.Size = new System.Drawing.Size(23, 18);
            this.baseLabel_DrillingTool_Freq_Unit.TabIndex = 135;
            this.baseLabel_DrillingTool_Freq_Unit.Text = "㎐";
            // 
            // textBox_DrillingTool_Frequency
            // 
            this.textBox_DrillingTool_Frequency.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_DrillingTool_Frequency.Location = new System.Drawing.Point(393, 33);
            this.textBox_DrillingTool_Frequency.Name = "textBox_DrillingTool_Frequency";
            this.textBox_DrillingTool_Frequency.Size = new System.Drawing.Size(92, 26);
            this.textBox_DrillingTool_Frequency.TabIndex = 134;
            // 
            // baseLabel_DrillingTool_Freq
            // 
            this.baseLabel_DrillingTool_Freq.AutoSize = true;
            this.baseLabel_DrillingTool_Freq.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_Freq.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_Freq.Location = new System.Drawing.Point(281, 36);
            this.baseLabel_DrillingTool_Freq.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_Freq.Name = "baseLabel_DrillingTool_Freq";
            this.baseLabel_DrillingTool_Freq.Size = new System.Drawing.Size(76, 18);
            this.baseLabel_DrillingTool_Freq.TabIndex = 133;
            this.baseLabel_DrillingTool_Freq.Text = "Frequency";
            // 
            // baseLabel_DrillingTool_ZOffset_Unit
            // 
            this.baseLabel_DrillingTool_ZOffset_Unit.AutoSize = true;
            this.baseLabel_DrillingTool_ZOffset_Unit.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_ZOffset_Unit.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_ZOffset_Unit.Location = new System.Drawing.Point(202, 191);
            this.baseLabel_DrillingTool_ZOffset_Unit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_ZOffset_Unit.Name = "baseLabel_DrillingTool_ZOffset_Unit";
            this.baseLabel_DrillingTool_ZOffset_Unit.Size = new System.Drawing.Size(23, 18);
            this.baseLabel_DrillingTool_ZOffset_Unit.TabIndex = 132;
            this.baseLabel_DrillingTool_ZOffset_Unit.Text = "㎜";
            // 
            // textBox_DrillingTool_ZOffset
            // 
            this.textBox_DrillingTool_ZOffset.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_DrillingTool_ZOffset.Location = new System.Drawing.Point(108, 188);
            this.textBox_DrillingTool_ZOffset.Name = "textBox_DrillingTool_ZOffset";
            this.textBox_DrillingTool_ZOffset.Size = new System.Drawing.Size(92, 26);
            this.textBox_DrillingTool_ZOffset.TabIndex = 131;
            // 
            // baseLabel_DrillingTool_ZOffset
            // 
            this.baseLabel_DrillingTool_ZOffset.AutoSize = true;
            this.baseLabel_DrillingTool_ZOffset.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_ZOffset.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_ZOffset.Location = new System.Drawing.Point(9, 191);
            this.baseLabel_DrillingTool_ZOffset.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_ZOffset.Name = "baseLabel_DrillingTool_ZOffset";
            this.baseLabel_DrillingTool_ZOffset.Size = new System.Drawing.Size(72, 18);
            this.baseLabel_DrillingTool_ZOffset.TabIndex = 130;
            this.baseLabel_DrillingTool_ZOffset.Text = "Z - Offset";
            // 
            // textBox_DrillingTool_RepeatCount
            // 
            this.textBox_DrillingTool_RepeatCount.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_DrillingTool_RepeatCount.Location = new System.Drawing.Point(108, 157);
            this.textBox_DrillingTool_RepeatCount.Name = "textBox_DrillingTool_RepeatCount";
            this.textBox_DrillingTool_RepeatCount.Size = new System.Drawing.Size(92, 26);
            this.textBox_DrillingTool_RepeatCount.TabIndex = 129;
            // 
            // baseLabel_DrillingTool_RepeatCount
            // 
            this.baseLabel_DrillingTool_RepeatCount.AutoSize = true;
            this.baseLabel_DrillingTool_RepeatCount.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_RepeatCount.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_RepeatCount.Location = new System.Drawing.Point(9, 160);
            this.baseLabel_DrillingTool_RepeatCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_RepeatCount.Name = "baseLabel_DrillingTool_RepeatCount";
            this.baseLabel_DrillingTool_RepeatCount.Size = new System.Drawing.Size(97, 18);
            this.baseLabel_DrillingTool_RepeatCount.TabIndex = 128;
            this.baseLabel_DrillingTool_RepeatCount.Text = "Repeat Count";
            // 
            // baseLabel_DrillingTool_HoleSize_Unit
            // 
            this.baseLabel_DrillingTool_HoleSize_Unit.AutoSize = true;
            this.baseLabel_DrillingTool_HoleSize_Unit.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_HoleSize_Unit.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_HoleSize_Unit.Location = new System.Drawing.Point(202, 129);
            this.baseLabel_DrillingTool_HoleSize_Unit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_HoleSize_Unit.Name = "baseLabel_DrillingTool_HoleSize_Unit";
            this.baseLabel_DrillingTool_HoleSize_Unit.Size = new System.Drawing.Size(23, 18);
            this.baseLabel_DrillingTool_HoleSize_Unit.TabIndex = 127;
            this.baseLabel_DrillingTool_HoleSize_Unit.Text = "㎜";
            // 
            // textBox_DrillingTool_HoleSize
            // 
            this.textBox_DrillingTool_HoleSize.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_DrillingTool_HoleSize.Location = new System.Drawing.Point(108, 126);
            this.textBox_DrillingTool_HoleSize.Name = "textBox_DrillingTool_HoleSize";
            this.textBox_DrillingTool_HoleSize.Size = new System.Drawing.Size(92, 26);
            this.textBox_DrillingTool_HoleSize.TabIndex = 126;
            // 
            // baseLabel_DrillingTool_HoleSize
            // 
            this.baseLabel_DrillingTool_HoleSize.AutoSize = true;
            this.baseLabel_DrillingTool_HoleSize.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_HoleSize.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_HoleSize.Location = new System.Drawing.Point(9, 129);
            this.baseLabel_DrillingTool_HoleSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_HoleSize.Name = "baseLabel_DrillingTool_HoleSize";
            this.baseLabel_DrillingTool_HoleSize.Size = new System.Drawing.Size(66, 18);
            this.baseLabel_DrillingTool_HoleSize.TabIndex = 125;
            this.baseLabel_DrillingTool_HoleSize.Text = "Hole Size";
            // 
            // comboBox_DrillingTool_MaskNo
            // 
            this.comboBox_DrillingTool_MaskNo.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_DrillingTool_MaskNo.FormattingEnabled = true;
            this.comboBox_DrillingTool_MaskNo.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4"});
            this.comboBox_DrillingTool_MaskNo.Location = new System.Drawing.Point(108, 95);
            this.comboBox_DrillingTool_MaskNo.Name = "comboBox_DrillingTool_MaskNo";
            this.comboBox_DrillingTool_MaskNo.Size = new System.Drawing.Size(92, 26);
            this.comboBox_DrillingTool_MaskNo.TabIndex = 124;
            this.comboBox_DrillingTool_MaskNo.Text = "0";
            // 
            // baseLabel_DrillingTool_MaskNo
            // 
            this.baseLabel_DrillingTool_MaskNo.AutoSize = true;
            this.baseLabel_DrillingTool_MaskNo.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_MaskNo.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_MaskNo.Location = new System.Drawing.Point(9, 97);
            this.baseLabel_DrillingTool_MaskNo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_MaskNo.Name = "baseLabel_DrillingTool_MaskNo";
            this.baseLabel_DrillingTool_MaskNo.Size = new System.Drawing.Size(70, 18);
            this.baseLabel_DrillingTool_MaskNo.TabIndex = 123;
            this.baseLabel_DrillingTool_MaskNo.Text = "Mask No.";
            // 
            // comboBox_DrillingTool_Type
            // 
            this.comboBox_DrillingTool_Type.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_DrillingTool_Type.FormattingEnabled = true;
            this.comboBox_DrillingTool_Type.Items.AddRange(new object[] {
            "Shot",
            "Circle",
            "Line"});
            this.comboBox_DrillingTool_Type.Location = new System.Drawing.Point(108, 64);
            this.comboBox_DrillingTool_Type.Name = "comboBox_DrillingTool_Type";
            this.comboBox_DrillingTool_Type.Size = new System.Drawing.Size(92, 26);
            this.comboBox_DrillingTool_Type.TabIndex = 122;
            this.comboBox_DrillingTool_Type.Text = "Circle";
            // 
            // baseLabel_DrillingTool_Type
            // 
            this.baseLabel_DrillingTool_Type.AutoSize = true;
            this.baseLabel_DrillingTool_Type.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_Type.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_Type.Location = new System.Drawing.Point(9, 66);
            this.baseLabel_DrillingTool_Type.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_Type.Name = "baseLabel_DrillingTool_Type";
            this.baseLabel_DrillingTool_Type.Size = new System.Drawing.Size(75, 18);
            this.baseLabel_DrillingTool_Type.TabIndex = 121;
            this.baseLabel_DrillingTool_Type.Text = "Tool Type";
            // 
            // textBox_DrillingTool_No
            // 
            this.textBox_DrillingTool_No.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_DrillingTool_No.Location = new System.Drawing.Point(108, 33);
            this.textBox_DrillingTool_No.Name = "textBox_DrillingTool_No";
            this.textBox_DrillingTool_No.ReadOnly = true;
            this.textBox_DrillingTool_No.Size = new System.Drawing.Size(92, 26);
            this.textBox_DrillingTool_No.TabIndex = 120;
            // 
            // baseLabel_DrillingTool_No
            // 
            this.baseLabel_DrillingTool_No.AutoSize = true;
            this.baseLabel_DrillingTool_No.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_No.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_No.Location = new System.Drawing.Point(9, 36);
            this.baseLabel_DrillingTool_No.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_No.Name = "baseLabel_DrillingTool_No";
            this.baseLabel_DrillingTool_No.Size = new System.Drawing.Size(64, 18);
            this.baseLabel_DrillingTool_No.TabIndex = 119;
            this.baseLabel_DrillingTool_No.Text = "Tool No.";
            // 
            // baseGroupBox_DrillingToolList
            // 
            this.baseGroupBox_DrillingToolList.Controls.Add(this.button_DrillingTool_Delete);
            this.baseGroupBox_DrillingToolList.Controls.Add(this.button_DrillingTool_Add);
            this.baseGroupBox_DrillingToolList.Controls.Add(this.treeView_DrillingTool);
            this.baseGroupBox_DrillingToolList.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_DrillingToolList.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox_DrillingToolList.Location = new System.Drawing.Point(11, 11);
            this.baseGroupBox_DrillingToolList.Name = "baseGroupBox_DrillingToolList";
            this.baseGroupBox_DrillingToolList.Size = new System.Drawing.Size(263, 502);
            this.baseGroupBox_DrillingToolList.TabIndex = 9;
            this.baseGroupBox_DrillingToolList.TabStop = false;
            this.baseGroupBox_DrillingToolList.Text = " [ Drilling Tool List ] ";
            // 
            // button_DrillingTool_Delete
            // 
            this.button_DrillingTool_Delete.BackColor = System.Drawing.Color.White;
            this.button_DrillingTool_Delete.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_DrillingTool_Delete.FlatAppearance.BorderSize = 2;
            this.button_DrillingTool_Delete.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_DrillingTool_Delete.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_DrillingTool_Delete.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_DrillingTool_Delete.ForeColor = System.Drawing.Color.Black;
            this.button_DrillingTool_Delete.Location = new System.Drawing.Point(137, 441);
            this.button_DrillingTool_Delete.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_DrillingTool_Delete.Name = "button_DrillingTool_Delete";
            this.button_DrillingTool_Delete.Size = new System.Drawing.Size(116, 51);
            this.button_DrillingTool_Delete.TabIndex = 198;
            this.button_DrillingTool_Delete.Text = "Delete Tool";
            this.button_DrillingTool_Delete.UseVisualStyleBackColor = false;
            // 
            // button_DrillingTool_Add
            // 
            this.button_DrillingTool_Add.BackColor = System.Drawing.Color.White;
            this.button_DrillingTool_Add.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_DrillingTool_Add.FlatAppearance.BorderSize = 2;
            this.button_DrillingTool_Add.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_DrillingTool_Add.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_DrillingTool_Add.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_DrillingTool_Add.ForeColor = System.Drawing.Color.Black;
            this.button_DrillingTool_Add.Location = new System.Drawing.Point(9, 441);
            this.button_DrillingTool_Add.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_DrillingTool_Add.Name = "button_DrillingTool_Add";
            this.button_DrillingTool_Add.Size = new System.Drawing.Size(116, 51);
            this.button_DrillingTool_Add.TabIndex = 197;
            this.button_DrillingTool_Add.Text = "Add Tool";
            this.button_DrillingTool_Add.UseVisualStyleBackColor = false;
            // 
            // treeView_DrillingTool
            // 
            this.treeView_DrillingTool.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.treeView_DrillingTool.Location = new System.Drawing.Point(9, 32);
            this.treeView_DrillingTool.Name = "treeView_DrillingTool";
            this.treeView_DrillingTool.Size = new System.Drawing.Size(244, 403);
            this.treeView_DrillingTool.TabIndex = 1;
            // 
            // ManualMode_SLD200
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.Controls.Add(this.tabControl_ManualMode);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ManualMode_SLD200";
            this.Size = new System.Drawing.Size(1910, 770);
            this.Load += new System.EventHandler(this.ManualMode_CWA150SA_Load);
            this.tabControl_ManualMode.ResumeLayout(false);
            this.tabPage_Laser.ResumeLayout(false);
            this.baseGroupBox_LaserControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_LaserControl_Button_Chiller2On)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_LaserControl_Button_Chiller1On)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_LaserControl_Button_ShutterOpen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_LaserControl_Button_PowerOn)).EndInit();
            this.baseGroupBox_LaserControl_Status.ResumeLayout(false);
            this.baseGroupBox_LaserControl_Status.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_LaserControl_Status_Emission)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_LaserControl_Status_Shutter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_LaserControl_Status_Ready)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_LaserControl_Status_Fault)).EndInit();
            this.baseGroupBox_DeviceControl.ResumeLayout(false);
            this.tabPage_Motor.ResumeLayout(false);
            this.baseGroupBox_SingleRun.ResumeLayout(false);
            this.baseGroupBox_CycleRun.ResumeLayout(false);
            this.baseGroupBox_PickerSuction.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_PickerStatus_ULSuction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_PickerStatus_LDSuction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_PickerSignal_ULSuctionOn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_PickerSignal_LDSuctionOn)).EndInit();
            this.baseGroupBox_TableSuction.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_TableStatus_AcrylSuction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_TableStatus_TableSuction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_TableSignal_AcrylSuction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_TableSignal_TableSuction)).EndInit();
            this.tabPage_PowerMeter.ResumeLayout(false);
            this.baseGroupBox_PowerMeasurement.ResumeLayout(false);
            this.tabPage_ScannerCal.ResumeLayout(false);
            this.baseGroupBox_ScannerCalInfo_Parameter.ResumeLayout(false);
            this.baseGroupBox_ScannerCalInfo_Manual.ResumeLayout(false);
            this.baseGroupBox_ScannerCalInfo_Manual.PerformLayout();
            this.baseGroupBox_ScannerCalInfo_Auto.ResumeLayout(false);
            this.baseGroupBox_ScannerCalInfo_Auto.PerformLayout();
            this.baseGroupBox_ScannerCal_Measurement.ResumeLayout(false);
            this.tabControl_ScannerCal_Camera.ResumeLayout(false);
            this.tabPage_LowRes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_visionImageViewer_LowRes)).EndInit();
            this.tabPage_HighRes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_visionImageViewer_HighRes)).EndInit();
            this.tabPage_OneHole.ResumeLayout(false);
            this.baseGroupBox_LaserFocusCheck.ResumeLayout(false);
            this.baseGroupBox_LaserFocusCheck_Func.ResumeLayout(false);
            this.baseGroupBox_LaserFocusCheck_Func.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_LaserFocusCheck_LinePos)).EndInit();
            this.baseGroupBox_LaserFocusCheck_PositionParameter.ResumeLayout(false);
            this.baseGroupBox_LaserFocusCheck_PositionParameter.PerformLayout();
            this.baseGroupBox_LaserFocusCheck_ShotParameter.ResumeLayout(false);
            this.baseGroupBox_LaserFocusCheck_ShotParameter.PerformLayout();
            this.tabPage_DrillingParameter.ResumeLayout(false);
            this.baseGroupBox_DrillingToolParam.ResumeLayout(false);
            this.baseGroupBox_DrillingToolParam.PerformLayout();
            this.baseGroupBox_DrillingToolList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.TabControl tabControl_ManualMode;
        private System.Windows.Forms.TabPage tabPage_Laser;
        private System.Windows.Forms.TabPage tabPage_Motor;
        private System.Windows.Forms.TabPage tabPage_PowerMeter;
        private System.Windows.Forms.TabPage tabPage_ScannerCal;
        private System.Windows.Forms.TabPage tabPage_VisionSet;
        private System.Windows.Forms.TabPage tabPage_OneHole;
        private BaseGroupBox baseGroupBox_DeviceControl;
        private Button button_Laser_TurnOffAllDevice;
        private Button button_Laser_TurnOnAllDevice;
        private BaseGroupBox baseGroupBox_LaserControl;
        private BaseGroupBox baseGroupBox_LaserControl_Status;
        private PictureBox pictureBox_LaserControl_Status_Fault;
        private BaseLabel baseLabel_Laser_Fault;
        private BaseLabel baseLabel_Laser_Ready;
        private PictureBox pictureBox_LaserControl_Status_Ready;
        private BaseLabel baseLabel_Laser_Shutter;
        private PictureBox pictureBox_LaserControl_Status_Shutter;
        private BaseLabel baseLabel_Laser_Emission;
        private PictureBox pictureBox_LaserControl_Status_Emission;
        private Button button_Laser_PowerOn;
        private PictureBox pictureBox_LaserControl_Button_PowerOn;
        private Button button_Laser_ShutterOpen;
        private PictureBox pictureBox_LaserControl_Button_ShutterOpen;
        private Button button_Laser_Chiller2On;
        private PictureBox pictureBox_LaserControl_Button_Chiller2On;
        private Button button_Laser_Chiller1On;
        private PictureBox pictureBox_LaserControl_Button_Chiller1On;
        private BaseGroupBox baseGroupBox_TableSuction;
        private Button button_Acryl_SuctionOn;
        private PictureBox pictureBox_TableSignal_AcrylSuction;
        private Button button_Table_SuctionOn;
        private PictureBox pictureBox_TableSignal_TableSuction;
        private PictureBox pictureBox_TableStatus_TableSuction;
        private PictureBox pictureBox_TableStatus_AcrylSuction;
        private BaseGroupBox baseGroupBox_PickerSuction;
        private PictureBox pictureBox_PickerStatus_ULSuction;
        private PictureBox pictureBox_PickerStatus_LDSuction;
        private Button button_Picker_ULSuctionOn;
        private PictureBox pictureBox_PickerSignal_ULSuctionOn;
        private Button button_Picker_LDSuctionOn;
        private PictureBox pictureBox_PickerSignal_LDSuctionOn;
        private BaseGroupBox baseGroupBox_CycleRun;
        private Button button_Run_Align;
        private Button button_Run_Unloading;
        private Button button_Run_Loading;
        private BaseGroupBox baseGroupBox_SingleRun;
        private Button button_Move_UnloadPos;
        private Button button_Move_LoadPos;
        private BaseGroupBox baseGroupBox_PowerMeasurement;
        private Button button_PowerMeasurement_Stop;
        private Button button_PowerMeasurement_Start;
        private ListBox listBox_PowerDataList;
        private BaseGroupBox baseGroupBox_ScannerCal_Measurement;
        private ListBox listBox_ScannerCal_MeasuredDataList;
        private Button button1;
        private Button button2;
        private TabControl tabControl_ScannerCal_Camera;
        private TabPage tabPage_LowRes;
        private QMC.Common.Hmi.VisionImageViewer m_visionImageViewer_LowRes;
        private QMC.Common.Hmi.VisionImageViewer m_visionImageViewer_HighRes;
        private Button button3;
        private BaseGroupBox baseGroupBox_ScannerCalInfo_Auto;
        private BaseLabel baseLabel_AutoCal_1stThickness;
        private BaseLabel baseLabel_AutoCal_Division;
        private ComboBox comboBox_AutoCal_Division;
        private BaseLabel baseLabel_AutoCal_1stThickness_Unit;
        private TextBox textBox_AutoCal_1stThickness;
        private TextBox textBox_AutoCal_ScannerPos_X;
        private BaseLabel baseLabel_AutoCal_PosX;
        private BaseLabel baseLabel_AutoCal_ScannerPos;
        private TextBox textBox_AutoCal_ScannerPos_Y;
        private BaseLabel baseLabel_AutoCal_PosY;
        private BaseGroupBox baseGroupBox_ScannerCalInfo_Manual;
        private TextBox textBox_ManualCal_ScannerPos_Y;
        private BaseLabel baseLabel_ManualCal_PosY;
        private TextBox textBox_ManualCal_ScannerPos_X;
        private BaseLabel baseLabel_ManualCal_PosX;
        private BaseLabel baseLabel_ManualCal_ScannerPos;
        private BaseLabel baseLabel_ManualCal_1stThickness_Unit;
        private TextBox textBox_ManualCal_1stThickness;
        private ComboBox comboBox_ManualCal_Division;
        private BaseLabel baseLabel_ManualCal_1stThickness;
        private BaseLabel baseLabel_ManualCal_Division;
        private BaseGroupBox baseGroupBox_ScannerCalInfo_Parameter;
        private BaseGroupBox baseGroupBox_ScannerCalInfo_Parameter_Laser;
        private BaseGroupBox baseGroupBox_ScannerCalInfo_Parameter_Vision;
        private BaseGroupBox baseGroupBox_LaserFocusCheck;
        private BaseGroupBox baseGroupBox_LaserFocusCheck_ShotParameter;
        private BaseLabel baseLabel_LaserFocusCheck_MovePitchXY_Unit;
        private TextBox textBox_LaserFocusCheck_MovePitchXY;
        private BaseLabel baseLabel_LaserFocusCheck_MovePitchXY;
        private BaseLabel baseLabel_LaserFocusCheck_MovePitchZ_Unit;
        private TextBox textBox_LaserFocusCheck_MovePitchZ;
        private BaseLabel baseLabel_LaserFocusCheck_MovePitchZ;
        private BaseLabel baseLabel_LaserFocusCheck_DrawSpeed_Unit;
        private TextBox textBox_LaserFocusCheck_DrawSpeed;
        private BaseLabel baseLabel_LaserFocusCheck_DrawSpeed;
        private BaseLabel baseLabel_LaserFocusCheck_DrawLength_Unit;
        private TextBox textBox_LaserFocusCheck_DrawLength;
        private BaseLabel baseLabel_LaserFocusCheck_DrawLength;
        private BaseLabel baseLabel_LaserFocusCheck_DrawDirection;
        private RadioButton radioButton_LaserFocusCheck_DrawDirection_Ver;
        private RadioButton radioButton_LaserFocusCheck_DrawDirection_Hor;
        private BaseLabel baseLabel_LaserFocusCheck_ShotPower_Unit;
        private TextBox textBox_LaserFocusCheck_ShotPower;
        private BaseLabel baseLabel_LaserFocusCheck_ShotPower;
        private BaseLabel baseLabel_LaserFocusCheck_DrawLineNum_Unit;
        private TextBox textBox_LaserFocusCheck_DrawLineNum;
        private BaseLabel baseLabel_LaserFocusCheck_DrawLineNum;
        private BaseGroupBox baseGroupBox_LaserFocusCheck_PositionParameter;
        private TextBox textBox_LaserFocusCheck_AxisZEndPos;
        private BaseLabel baseLabel_LaserFocusCheck_AxisZEndPos;
        private TextBox textBox_LaserFocusCheck_AxisZStartPos;
        private BaseLabel baseLabel_LaserFocusCheck_AxisZStartPos;
        private BaseLabel baseLabel_LaserFocusCheck_AxisZRangeSet;
        private Button button_LaserFocusCheck_GetZPos;
        private BaseLabel baseLabel_LaserFocusCheck_Arrow;
        private BaseLabel baseLabel_LaserFocusCheck_AxisZEndPos_AutoCalc;
        private BaseLabel baseLabel_LaserFocusCheck_Arrow2;
        private Button button_LaserFocusCheck_GetXYPos;
        private TextBox textBox_LaserFocusCheck_AxisYStartPos;
        private BaseLabel baseLabel_LaserFocusCheck_AxisYStartPos;
        private TextBox textBox_LaserFocusCheck_AxisXStartPos;
        private BaseLabel baseLabel_LaserFocusCheck_AxisXStartPos;
        private BaseLabel baseLabel_LaserFocusCheck_AxisXYRangeSet;
        private BaseGroupBox baseGroupBox_LaserFocusCheck_Func;
        private BaseLabel baseLabel_LaserFocusCheck_Arrow3;
        private Button button_LaserFocusCheck_DrawnPosMove;
        private TextBox textBox_LaserFocusCheck_AxisZFocusPos;
        private BaseLabel baseLabel_LaserFocusCheck_AxisZFocusPos;
        private BaseLabel baseLabel_LaserFocusCheck_LinePosMove;
        private Button button_LaserFocusCheck_Start;
        private Button button_LaserFocusCheck_ParameterVerification;
        private NumericUpDown numericUpDown_LaserFocusCheck_LinePos;
        private TabPage tabPage_DrillingParameter;
        private BaseGroupBox baseGroupBox_DrillingToolList;
        private Button button_DrillingTool_Delete;
        private Button button_DrillingTool_Add;
        private TreeView treeView_DrillingTool;
        private BaseGroupBox baseGroupBox_DrillingToolParam;
        private TextBox textBox_DrillingTool_No;
        private BaseLabel baseLabel_DrillingTool_No;
        private ComboBox comboBox_DrillingTool_Type;
        private BaseLabel baseLabel_DrillingTool_Type;
        private ComboBox comboBox_DrillingTool_MaskNo;
        private BaseLabel baseLabel_DrillingTool_MaskNo;
        private BaseLabel baseLabel_DrillingTool_HoleSize_Unit;
        private TextBox textBox_DrillingTool_HoleSize;
        private BaseLabel baseLabel_DrillingTool_HoleSize;
        private BaseLabel baseLabel_DrillingTool_ZOffset_Unit;
        private TextBox textBox_DrillingTool_ZOffset;
        private BaseLabel baseLabel_DrillingTool_ZOffset;
        private TextBox textBox_DrillingTool_RepeatCount;
        private BaseLabel baseLabel_DrillingTool_RepeatCount;
        private BaseLabel baseLabel_DrillingTool_Freq_Unit;
        private TextBox textBox_DrillingTool_Frequency;
        private BaseLabel baseLabel_DrillingTool_Freq;
        private BaseLabel baseLabel_DrillingTool_JumpSpeed_Unit;
        private TextBox textBox_DrillingTool_JumpSpeed;
        private BaseLabel baseLabel_DrillingTool_JumpSpeed;
        private BaseLabel baseLabel_DrillingTool_MarkSpeed_Unit;
        private TextBox textBox_DrillingTool_MarkSpeed;
        private BaseLabel baseLabel_DrillingTool_MarkSpeed;
        private BaseLabel baseLabel_DrillingTool_MarkDelay_Unit;
        private TextBox textBox_DrillingTool_MarkDelay;
        private BaseLabel baseLabel_DrillingTool_MarkDelay;
        private BaseLabel baseLabel_DrillingTool_LaserOffDelay_Unit;
        private TextBox textBox_DrillingTool_LaserOffDelay;
        private BaseLabel baseLabel_DrillingTool_LaserOffDelay;
        private BaseLabel baseLabel_DrillingTool_LaserOnDelay_Unit;
        private TextBox textBox_DrillingTool_LaserOnDelay;
        private BaseLabel baseLabel_DrillingTool_LaserOnDelay;
        private BaseLabel baseLabel_DrillingTool_JumpDelay_Unit;
        private TextBox textBox_DrillingTool_JumpDelay;
        private BaseLabel baseLabel_DrillingTool_JumpDelay;
        internal TabPage tabPage_HighRes;
    }
}
