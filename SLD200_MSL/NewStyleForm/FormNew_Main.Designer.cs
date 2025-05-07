namespace SLD200_MSL
{
    partial class FormNew_Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.SiriusViewer_Main = new SpiralLab.Sirius.SiriusViewerForm();
            this.groupBoxMain_ModuleProcessingStatus = new System.Windows.Forms.GroupBox();
            this.checkBox_Main_AlignStartSocket_SelectMode = new System.Windows.Forms.CheckBox();
            this.baseLabel_SocketStatus_NG = new SLD200_MSL.BaseLabel();
            this.baseLabel_SocketStatus_OK = new SLD200_MSL.BaseLabel();
            this.baseLabel_SocketStatus_Processing = new SLD200_MSL.BaseLabel();
            this.baseLabel_SocketStatus_Ready = new SLD200_MSL.BaseLabel();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox_Socket_Before = new System.Windows.Forms.PictureBox();
            this.pictureBox_ModuleProcessingStatus = new System.Windows.Forms.PictureBox();
            this.groupBoxMain_ProcessingStatus = new System.Windows.Forms.GroupBox();
            this.baseGroupBox_Progress = new SLD200_MSL.WATGroupBox();
            this.baseTextBox2 = new SLD200_MSL.BaseTextBox();
            this.baseTextBox_TotalSocketCount = new SLD200_MSL.BaseTextBox();
            this.baseLabel_SocketCount = new SLD200_MSL.BaseLabel();
            this.numericUpDown_Module_TargetCount = new System.Windows.Forms.NumericUpDown();
            this.baseLabel_ModuleCount_Target = new SLD200_MSL.BaseLabel();
            this.button_PNLCount_Clear = new System.Windows.Forms.Button();
            this.baseLabel_PNLCount_NG = new SLD200_MSL.BaseLabel();
            this.baseTextBox_Module_NGCount = new SLD200_MSL.BaseTextBox();
            this.baseTextBox_Module_TotalCount = new SLD200_MSL.BaseTextBox();
            this.baseLabel_ModuleCount = new SLD200_MSL.BaseLabel();
            this.baseLabel_PNLCount_Total = new SLD200_MSL.BaseLabel();
            this.baseGroupBox_WorkingTime = new SLD200_MSL.WATGroupBox();
            this.button_AverageOneCycleTime_Clear = new System.Windows.Forms.Button();
            this.baseLabel_Average_OneCycleTime = new SLD200_MSL.BaseLabel();
            this.baseLabel_AverageOneCycle_Time = new SLD200_MSL.BaseLabel();
            this.progressBar_TotalRemained_Time = new System.Windows.Forms.ProgressBar();
            this.baseLabel_Total_RemainedTime = new SLD200_MSL.BaseLabel();
            this.baseLabel_TotalRunning_Time = new SLD200_MSL.BaseLabel();
            this.progressBar_OneCycle_Time = new System.Windows.Forms.ProgressBar();
            this.baseLabel_CurrentOneCycle_TotalTime = new SLD200_MSL.BaseLabel();
            this.baseLabel_CurrentOneCycle_ElapsedTime = new SLD200_MSL.BaseLabel();
            this.baseLabel_CurrentOneCycle_Time = new SLD200_MSL.BaseLabel();
            this.groupBox18 = new System.Windows.Forms.GroupBox();
            this.listView_Main_FiducialAlignData = new System.Windows.Forms.ListView();
            this.groupBoxMain_MaterialInformation = new System.Windows.Forms.GroupBox();
            this.baseTextBox_Socket_Index = new SLD200_MSL.BaseTextBox();
            this.baseTextBox_SocketCountPerModule = new SLD200_MSL.BaseTextBox();
            this.baseLabel_SocketPerModule = new SLD200_MSL.BaseLabel();
            this.baseLabel1 = new SLD200_MSL.BaseLabel();
            this.button_Main_Stop = new System.Windows.Forms.Button();
            this.button_Main_Pause = new System.Windows.Forms.Button();
            this.button_Main_Start = new System.Windows.Forms.Button();
            this.groupBox_ProcessingData = new System.Windows.Forms.GroupBox();
            this.button_Main_RecipeOpen = new System.Windows.Forms.Button();
            this.checkBox_Main_CycleStop = new System.Windows.Forms.CheckBox();
            this.button_Main_Reset = new System.Windows.Forms.Button();
            this.button_Main_Home = new System.Windows.Forms.Button();
            this.button_Main_RtcInit = new System.Windows.Forms.Button();
            this.checkBox_Test_DryRun = new System.Windows.Forms.CheckBox();
            this.checkBox_Main_Loader_LPort_Pause = new System.Windows.Forms.CheckBox();
            this.checkBox_Main_Loader_RPort_Pause = new System.Windows.Forms.CheckBox();
            this.button_Main_CameraInit = new System.Windows.Forms.Button();
            this.checkBox_Main_SocketStop = new System.Windows.Forms.CheckBox();
            this.buttonForceMaterialOut = new System.Windows.Forms.Button();
            this.groupBox_FineCam = new System.Windows.Forms.GroupBox();
            this.ImageViewer_Main_highs = new QMC.Common.Hmi.VisionImageViewer();
            this.groupBox_CoarseCam = new System.Windows.Forms.GroupBox();
            this.ImageViewer_Main_Rows = new QMC.Common.Hmi.VisionImageViewer();
            this.checkBox_Main_AutoRun = new System.Windows.Forms.CheckBox();
            this.button_TEST12 = new System.Windows.Forms.Button();
            this.button_TestbyUser_LPort_Start = new System.Windows.Forms.Button();
            this.label_Title_Stacker_LPort = new System.Windows.Forms.Label();
            this.label_Title_Stacker_RPort = new System.Windows.Forms.Label();
            this.groupBox_Main_DiviceStatus = new System.Windows.Forms.GroupBox();
            this.baseLabel_Main_Divice_Status_Illuminator = new SLD200_MSL.BaseLabel();
            this.pictureBox_Main_DiviceStatus_Illuminator = new System.Windows.Forms.PictureBox();
            this.baseLabel_Main_Divice_Status_CameraPre = new SLD200_MSL.BaseLabel();
            this.pictureBox_Main_DiviceStatus_CameraPre = new System.Windows.Forms.PictureBox();
            this.baseLabel_Main_Divice_Status_CameraFine = new SLD200_MSL.BaseLabel();
            this.pictureBox_Main_DiviceStatus_CameraFine = new System.Windows.Forms.PictureBox();
            this.baseLabel_Main_Divice_Status_heightsensor = new SLD200_MSL.BaseLabel();
            this.pictureBox_Main_DiviceStatus_HeightSensor = new System.Windows.Forms.PictureBox();
            this.baseLabel_Main_Divice_Status_ElectroRequlator = new SLD200_MSL.BaseLabel();
            this.pictureBox_Main_DiviceStatus_ElectroRegulator = new System.Windows.Forms.PictureBox();
            this.baseLabel_Main_Divice_Status_BeamExpander = new SLD200_MSL.BaseLabel();
            this.pictureBox_Main_DiviceStatus_BeamExpander = new System.Windows.Forms.PictureBox();
            this.baseLabel_Main_Divice_Status_Chiller = new SLD200_MSL.BaseLabel();
            this.pictureBox_Main_DiviceStatus_Chiller = new System.Windows.Forms.PictureBox();
            this.baseLabel_Main_Divice_Status_Stage = new SLD200_MSL.BaseLabel();
            this.pictureBox_Main_DiviceStatus_Powermeter_Stage = new System.Windows.Forms.PictureBox();
            this.baseLabel_Main_Divice_Status_PowermeterBds = new SLD200_MSL.BaseLabel();
            this.pictureBox_Main_DiviceStatus_Powermeter_bds = new System.Windows.Forms.PictureBox();
            this.baseLabel_Main_Divice_Status_DustcollectorLower = new SLD200_MSL.BaseLabel();
            this.pictureBox_Main_DiviceStatus_DustCollector_Lower = new System.Windows.Forms.PictureBox();
            this.baseLabel_Main_Divice_Status_DustcollectorUpper = new SLD200_MSL.BaseLabel();
            this.pictureBox_Main_DiviceStatus_DustCollector_Upper = new System.Windows.Forms.PictureBox();
            this.baseLabel_Main_Divice_Status_Scanner = new SLD200_MSL.BaseLabel();
            this.pictureBox_Main_DiviceStatus_Scanner = new System.Windows.Forms.PictureBox();
            this.baseLabel_Main_Divice_Status_Motion = new SLD200_MSL.BaseLabel();
            this.pictureBox_Main_DiviceStatus_Motion = new System.Windows.Forms.PictureBox();
            this.baseLabel_Main_Divice_Status_IO = new SLD200_MSL.BaseLabel();
            this.pictureBox_Main_DiviceStatus_IO = new System.Windows.Forms.PictureBox();
            this.baseLabel_Main_Divice_Status_Laser = new SLD200_MSL.BaseLabel();
            this.pictureBox_Main_DiviceStatus_Laser = new System.Windows.Forms.PictureBox();
            this.label_Main_LaserStatus = new System.Windows.Forms.Label();
            this.baseTextBox_DryRun_ProcessingTime = new SLD200_MSL.BaseTextBox();
            this.baseLabel5 = new SLD200_MSL.BaseLabel();
            this.checkBox_Test_LaserDrillingCycle = new System.Windows.Forms.CheckBox();
            this.button_Main_ManualStart = new System.Windows.Forms.Button();
            this.groupBoxMain_ModuleProcessingStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Socket_Before)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_ModuleProcessingStatus)).BeginInit();
            this.groupBoxMain_ProcessingStatus.SuspendLayout();
            this.baseGroupBox_Progress.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Module_TargetCount)).BeginInit();
            this.baseGroupBox_WorkingTime.SuspendLayout();
            this.groupBox18.SuspendLayout();
            this.groupBoxMain_MaterialInformation.SuspendLayout();
            this.groupBox_ProcessingData.SuspendLayout();
            this.groupBox_FineCam.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImageViewer_Main_highs)).BeginInit();
            this.groupBox_CoarseCam.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImageViewer_Main_Rows)).BeginInit();
            this.groupBox_Main_DiviceStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Main_DiviceStatus_Illuminator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Main_DiviceStatus_CameraPre)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Main_DiviceStatus_CameraFine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Main_DiviceStatus_HeightSensor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Main_DiviceStatus_ElectroRegulator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Main_DiviceStatus_BeamExpander)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Main_DiviceStatus_Chiller)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Main_DiviceStatus_Powermeter_Stage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Main_DiviceStatus_Powermeter_bds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Main_DiviceStatus_DustCollector_Lower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Main_DiviceStatus_DustCollector_Upper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Main_DiviceStatus_Scanner)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Main_DiviceStatus_Motion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Main_DiviceStatus_IO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Main_DiviceStatus_Laser)).BeginInit();
            this.SuspendLayout();
            // 
            // SiriusViewer_Main
            // 
            this.SiriusViewer_Main.AliasName = "NoName";
            this.SiriusViewer_Main.BackColor = System.Drawing.SystemColors.Control;
            this.SiriusViewer_Main.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SiriusViewer_Main.Document = null;
            this.SiriusViewer_Main.FileName = "NoName";
            this.SiriusViewer_Main.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SiriusViewer_Main.Index = ((uint)(0u));
            this.SiriusViewer_Main.Location = new System.Drawing.Point(11, 32);
            this.SiriusViewer_Main.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SiriusViewer_Main.Name = "SiriusViewer_Main";
            this.SiriusViewer_Main.Progress = 0;
            this.SiriusViewer_Main.Size = new System.Drawing.Size(690, 538);
            this.SiriusViewer_Main.TabIndex = 34;
            // 
            // groupBoxMain_ModuleProcessingStatus
            // 
            this.groupBoxMain_ModuleProcessingStatus.Controls.Add(this.checkBox_Main_AlignStartSocket_SelectMode);
            this.groupBoxMain_ModuleProcessingStatus.Controls.Add(this.baseLabel_SocketStatus_NG);
            this.groupBoxMain_ModuleProcessingStatus.Controls.Add(this.baseLabel_SocketStatus_OK);
            this.groupBoxMain_ModuleProcessingStatus.Controls.Add(this.baseLabel_SocketStatus_Processing);
            this.groupBoxMain_ModuleProcessingStatus.Controls.Add(this.baseLabel_SocketStatus_Ready);
            this.groupBoxMain_ModuleProcessingStatus.Controls.Add(this.pictureBox3);
            this.groupBoxMain_ModuleProcessingStatus.Controls.Add(this.pictureBox2);
            this.groupBoxMain_ModuleProcessingStatus.Controls.Add(this.pictureBox1);
            this.groupBoxMain_ModuleProcessingStatus.Controls.Add(this.pictureBox_Socket_Before);
            this.groupBoxMain_ModuleProcessingStatus.Controls.Add(this.pictureBox_ModuleProcessingStatus);
            this.groupBoxMain_ModuleProcessingStatus.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxMain_ModuleProcessingStatus.Location = new System.Drawing.Point(12, 9);
            this.groupBoxMain_ModuleProcessingStatus.Name = "groupBoxMain_ModuleProcessingStatus";
            this.groupBoxMain_ModuleProcessingStatus.Size = new System.Drawing.Size(365, 250);
            this.groupBoxMain_ModuleProcessingStatus.TabIndex = 0;
            this.groupBoxMain_ModuleProcessingStatus.TabStop = false;
            this.groupBoxMain_ModuleProcessingStatus.Text = " Module Status ";
            // 
            // checkBox_Main_AlignStartSocket_SelectMode
            // 
            this.checkBox_Main_AlignStartSocket_SelectMode.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.checkBox_Main_AlignStartSocket_SelectMode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.checkBox_Main_AlignStartSocket_SelectMode.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_Main_AlignStartSocket_SelectMode.Location = new System.Drawing.Point(256, 137);
            this.checkBox_Main_AlignStartSocket_SelectMode.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox_Main_AlignStartSocket_SelectMode.Name = "checkBox_Main_AlignStartSocket_SelectMode";
            this.checkBox_Main_AlignStartSocket_SelectMode.Size = new System.Drawing.Size(97, 53);
            this.checkBox_Main_AlignStartSocket_SelectMode.TabIndex = 148;
            this.checkBox_Main_AlignStartSocket_SelectMode.Text = "Align Start Socket Select";
            this.checkBox_Main_AlignStartSocket_SelectMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox_Main_AlignStartSocket_SelectMode.UseVisualStyleBackColor = false;
            // 
            // baseLabel_SocketStatus_NG
            // 
            this.baseLabel_SocketStatus_NG.AutoSize = true;
            this.baseLabel_SocketStatus_NG.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_SocketStatus_NG.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_SocketStatus_NG.Location = new System.Drawing.Point(281, 108);
            this.baseLabel_SocketStatus_NG.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_SocketStatus_NG.Name = "baseLabel_SocketStatus_NG";
            this.baseLabel_SocketStatus_NG.Size = new System.Drawing.Size(28, 18);
            this.baseLabel_SocketStatus_NG.TabIndex = 122;
            this.baseLabel_SocketStatus_NG.Text = "NG";
            this.baseLabel_SocketStatus_NG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel_SocketStatus_OK
            // 
            this.baseLabel_SocketStatus_OK.AutoSize = true;
            this.baseLabel_SocketStatus_OK.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_SocketStatus_OK.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_SocketStatus_OK.Location = new System.Drawing.Point(281, 85);
            this.baseLabel_SocketStatus_OK.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_SocketStatus_OK.Name = "baseLabel_SocketStatus_OK";
            this.baseLabel_SocketStatus_OK.Size = new System.Drawing.Size(69, 18);
            this.baseLabel_SocketStatus_OK.TabIndex = 121;
            this.baseLabel_SocketStatus_OK.Text = "Complete";
            this.baseLabel_SocketStatus_OK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel_SocketStatus_Processing
            // 
            this.baseLabel_SocketStatus_Processing.AutoSize = true;
            this.baseLabel_SocketStatus_Processing.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_SocketStatus_Processing.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_SocketStatus_Processing.Location = new System.Drawing.Point(281, 62);
            this.baseLabel_SocketStatus_Processing.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_SocketStatus_Processing.Name = "baseLabel_SocketStatus_Processing";
            this.baseLabel_SocketStatus_Processing.Size = new System.Drawing.Size(76, 18);
            this.baseLabel_SocketStatus_Processing.TabIndex = 120;
            this.baseLabel_SocketStatus_Processing.Text = "Processing";
            this.baseLabel_SocketStatus_Processing.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel_SocketStatus_Ready
            // 
            this.baseLabel_SocketStatus_Ready.AutoSize = true;
            this.baseLabel_SocketStatus_Ready.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_SocketStatus_Ready.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_SocketStatus_Ready.Location = new System.Drawing.Point(281, 39);
            this.baseLabel_SocketStatus_Ready.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_SocketStatus_Ready.Name = "baseLabel_SocketStatus_Ready";
            this.baseLabel_SocketStatus_Ready.Size = new System.Drawing.Size(49, 18);
            this.baseLabel_SocketStatus_Ready.TabIndex = 119;
            this.baseLabel_SocketStatus_Ready.Text = "Ready";
            this.baseLabel_SocketStatus_Ready.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Red;
            this.pictureBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox3.Location = new System.Drawing.Point(254, 107);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(24, 20);
            this.pictureBox3.TabIndex = 4;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Green;
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Location = new System.Drawing.Point(254, 84);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(24, 20);
            this.pictureBox2.TabIndex = 3;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Yellow;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(254, 61);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(24, 20);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox_Socket_Before
            // 
            this.pictureBox_Socket_Before.BackColor = System.Drawing.Color.LightGray;
            this.pictureBox_Socket_Before.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_Socket_Before.Location = new System.Drawing.Point(254, 38);
            this.pictureBox_Socket_Before.Name = "pictureBox_Socket_Before";
            this.pictureBox_Socket_Before.Size = new System.Drawing.Size(24, 20);
            this.pictureBox_Socket_Before.TabIndex = 1;
            this.pictureBox_Socket_Before.TabStop = false;
            // 
            // pictureBox_ModuleProcessingStatus
            // 
            this.pictureBox_ModuleProcessingStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_ModuleProcessingStatus.Location = new System.Drawing.Point(11, 38);
            this.pictureBox_ModuleProcessingStatus.Name = "pictureBox_ModuleProcessingStatus";
            this.pictureBox_ModuleProcessingStatus.Size = new System.Drawing.Size(237, 198);
            this.pictureBox_ModuleProcessingStatus.TabIndex = 0;
            this.pictureBox_ModuleProcessingStatus.TabStop = false;
            this.pictureBox_ModuleProcessingStatus.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PictureBox_ModuleProcessingStatus_MouseClick);
            // 
            // groupBoxMain_ProcessingStatus
            // 
            this.groupBoxMain_ProcessingStatus.Controls.Add(this.baseGroupBox_Progress);
            this.groupBoxMain_ProcessingStatus.Controls.Add(this.baseGroupBox_WorkingTime);
            this.groupBoxMain_ProcessingStatus.Controls.Add(this.groupBox18);
            this.groupBoxMain_ProcessingStatus.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxMain_ProcessingStatus.Location = new System.Drawing.Point(412, 9);
            this.groupBoxMain_ProcessingStatus.Name = "groupBoxMain_ProcessingStatus";
            this.groupBoxMain_ProcessingStatus.Size = new System.Drawing.Size(900, 250);
            this.groupBoxMain_ProcessingStatus.TabIndex = 1;
            this.groupBoxMain_ProcessingStatus.TabStop = false;
            this.groupBoxMain_ProcessingStatus.Text = " Processing Status (Time) ";
            // 
            // baseGroupBox_Progress
            // 
            this.baseGroupBox_Progress.BorderColor = System.Drawing.Color.DarkGray;
            this.baseGroupBox_Progress.Controls.Add(this.baseTextBox2);
            this.baseGroupBox_Progress.Controls.Add(this.baseTextBox_TotalSocketCount);
            this.baseGroupBox_Progress.Controls.Add(this.baseLabel_SocketCount);
            this.baseGroupBox_Progress.Controls.Add(this.numericUpDown_Module_TargetCount);
            this.baseGroupBox_Progress.Controls.Add(this.baseLabel_ModuleCount_Target);
            this.baseGroupBox_Progress.Controls.Add(this.button_PNLCount_Clear);
            this.baseGroupBox_Progress.Controls.Add(this.baseLabel_PNLCount_NG);
            this.baseGroupBox_Progress.Controls.Add(this.baseTextBox_Module_NGCount);
            this.baseGroupBox_Progress.Controls.Add(this.baseTextBox_Module_TotalCount);
            this.baseGroupBox_Progress.Controls.Add(this.baseLabel_ModuleCount);
            this.baseGroupBox_Progress.Controls.Add(this.baseLabel_PNLCount_Total);
            this.baseGroupBox_Progress.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_Progress.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox_Progress.Location = new System.Drawing.Point(459, 33);
            this.baseGroupBox_Progress.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseGroupBox_Progress.Name = "baseGroupBox_Progress";
            this.baseGroupBox_Progress.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseGroupBox_Progress.Size = new System.Drawing.Size(426, 115);
            this.baseGroupBox_Progress.TabIndex = 193;
            this.baseGroupBox_Progress.TabStop = false;
            this.baseGroupBox_Progress.Text = " [ Progress ] ";
            // 
            // baseTextBox2
            // 
            this.baseTextBox2.BackColor = System.Drawing.Color.White;
            this.baseTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseTextBox2.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseTextBox2.ForeColor = System.Drawing.Color.Black;
            this.baseTextBox2.Location = new System.Drawing.Point(277, 80);
            this.baseTextBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseTextBox2.Name = "baseTextBox2";
            this.baseTextBox2.ReadOnly = true;
            this.baseTextBox2.Size = new System.Drawing.Size(77, 26);
            this.baseTextBox2.TabIndex = 136;
            this.baseTextBox2.Text = "0 (0)";
            this.baseTextBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseTextBox_TotalSocketCount
            // 
            this.baseTextBox_TotalSocketCount.BackColor = System.Drawing.Color.White;
            this.baseTextBox_TotalSocketCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseTextBox_TotalSocketCount.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseTextBox_TotalSocketCount.ForeColor = System.Drawing.Color.Black;
            this.baseTextBox_TotalSocketCount.Location = new System.Drawing.Point(196, 80);
            this.baseTextBox_TotalSocketCount.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseTextBox_TotalSocketCount.Name = "baseTextBox_TotalSocketCount";
            this.baseTextBox_TotalSocketCount.ReadOnly = true;
            this.baseTextBox_TotalSocketCount.Size = new System.Drawing.Size(77, 26);
            this.baseTextBox_TotalSocketCount.TabIndex = 135;
            this.baseTextBox_TotalSocketCount.Text = "0 (0)";
            this.baseTextBox_TotalSocketCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseLabel_SocketCount
            // 
            this.baseLabel_SocketCount.AutoSize = true;
            this.baseLabel_SocketCount.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_SocketCount.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_SocketCount.Location = new System.Drawing.Point(10, 85);
            this.baseLabel_SocketCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_SocketCount.Name = "baseLabel_SocketCount";
            this.baseLabel_SocketCount.Size = new System.Drawing.Size(51, 18);
            this.baseLabel_SocketCount.TabIndex = 134;
            this.baseLabel_SocketCount.Text = "Socket";
            // 
            // numericUpDown_Module_TargetCount
            // 
            this.numericUpDown_Module_TargetCount.BackColor = System.Drawing.Color.White;
            this.numericUpDown_Module_TargetCount.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDown_Module_TargetCount.Location = new System.Drawing.Point(75, 45);
            this.numericUpDown_Module_TargetCount.Name = "numericUpDown_Module_TargetCount";
            this.numericUpDown_Module_TargetCount.Size = new System.Drawing.Size(98, 26);
            this.numericUpDown_Module_TargetCount.TabIndex = 133;
            this.numericUpDown_Module_TargetCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseLabel_ModuleCount_Target
            // 
            this.baseLabel_ModuleCount_Target.AutoSize = true;
            this.baseLabel_ModuleCount_Target.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.baseLabel_ModuleCount_Target.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_ModuleCount_Target.Location = new System.Drawing.Point(101, 20);
            this.baseLabel_ModuleCount_Target.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_ModuleCount_Target.Name = "baseLabel_ModuleCount_Target";
            this.baseLabel_ModuleCount_Target.Size = new System.Drawing.Size(45, 16);
            this.baseLabel_ModuleCount_Target.TabIndex = 131;
            this.baseLabel_ModuleCount_Target.Text = "Target";
            // 
            // button_PNLCount_Clear
            // 
            this.button_PNLCount_Clear.BackColor = System.Drawing.Color.White;
            this.button_PNLCount_Clear.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_PNLCount_Clear.FlatAppearance.BorderSize = 2;
            this.button_PNLCount_Clear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_PNLCount_Clear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_PNLCount_Clear.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.button_PNLCount_Clear.ForeColor = System.Drawing.Color.Black;
            this.button_PNLCount_Clear.Location = new System.Drawing.Point(359, 44);
            this.button_PNLCount_Clear.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_PNLCount_Clear.Name = "button_PNLCount_Clear";
            this.button_PNLCount_Clear.Size = new System.Drawing.Size(58, 29);
            this.button_PNLCount_Clear.TabIndex = 130;
            this.button_PNLCount_Clear.Text = "Clear";
            this.button_PNLCount_Clear.UseVisualStyleBackColor = false;
            // 
            // baseLabel_PNLCount_NG
            // 
            this.baseLabel_PNLCount_NG.AutoSize = true;
            this.baseLabel_PNLCount_NG.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.baseLabel_PNLCount_NG.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_PNLCount_NG.Location = new System.Drawing.Point(302, 20);
            this.baseLabel_PNLCount_NG.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_PNLCount_NG.Name = "baseLabel_PNLCount_NG";
            this.baseLabel_PNLCount_NG.Size = new System.Drawing.Size(23, 16);
            this.baseLabel_PNLCount_NG.TabIndex = 121;
            this.baseLabel_PNLCount_NG.Text = "NG";
            // 
            // baseTextBox_Module_NGCount
            // 
            this.baseTextBox_Module_NGCount.BackColor = System.Drawing.Color.White;
            this.baseTextBox_Module_NGCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseTextBox_Module_NGCount.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseTextBox_Module_NGCount.ForeColor = System.Drawing.Color.Black;
            this.baseTextBox_Module_NGCount.Location = new System.Drawing.Point(277, 45);
            this.baseTextBox_Module_NGCount.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseTextBox_Module_NGCount.Name = "baseTextBox_Module_NGCount";
            this.baseTextBox_Module_NGCount.ReadOnly = true;
            this.baseTextBox_Module_NGCount.Size = new System.Drawing.Size(77, 26);
            this.baseTextBox_Module_NGCount.TabIndex = 120;
            this.baseTextBox_Module_NGCount.Text = "0";
            this.baseTextBox_Module_NGCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseTextBox_Module_TotalCount
            // 
            this.baseTextBox_Module_TotalCount.BackColor = System.Drawing.Color.White;
            this.baseTextBox_Module_TotalCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseTextBox_Module_TotalCount.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseTextBox_Module_TotalCount.ForeColor = System.Drawing.Color.Black;
            this.baseTextBox_Module_TotalCount.Location = new System.Drawing.Point(196, 45);
            this.baseTextBox_Module_TotalCount.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseTextBox_Module_TotalCount.Name = "baseTextBox_Module_TotalCount";
            this.baseTextBox_Module_TotalCount.ReadOnly = true;
            this.baseTextBox_Module_TotalCount.Size = new System.Drawing.Size(77, 26);
            this.baseTextBox_Module_TotalCount.TabIndex = 119;
            this.baseTextBox_Module_TotalCount.Text = "0";
            this.baseTextBox_Module_TotalCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseLabel_ModuleCount
            // 
            this.baseLabel_ModuleCount.AutoSize = true;
            this.baseLabel_ModuleCount.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_ModuleCount.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_ModuleCount.Location = new System.Drawing.Point(10, 51);
            this.baseLabel_ModuleCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_ModuleCount.Name = "baseLabel_ModuleCount";
            this.baseLabel_ModuleCount.Size = new System.Drawing.Size(54, 18);
            this.baseLabel_ModuleCount.TabIndex = 118;
            this.baseLabel_ModuleCount.Text = "Module";
            this.baseLabel_ModuleCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel_PNLCount_Total
            // 
            this.baseLabel_PNLCount_Total.AutoSize = true;
            this.baseLabel_PNLCount_Total.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.baseLabel_PNLCount_Total.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_PNLCount_Total.Location = new System.Drawing.Point(216, 20);
            this.baseLabel_PNLCount_Total.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_PNLCount_Total.Name = "baseLabel_PNLCount_Total";
            this.baseLabel_PNLCount_Total.Size = new System.Drawing.Size(36, 16);
            this.baseLabel_PNLCount_Total.TabIndex = 117;
            this.baseLabel_PNLCount_Total.Text = "Total";
            // 
            // baseGroupBox_WorkingTime
            // 
            this.baseGroupBox_WorkingTime.BorderColor = System.Drawing.Color.DarkGray;
            this.baseGroupBox_WorkingTime.Controls.Add(this.button_AverageOneCycleTime_Clear);
            this.baseGroupBox_WorkingTime.Controls.Add(this.baseLabel_Average_OneCycleTime);
            this.baseGroupBox_WorkingTime.Controls.Add(this.baseLabel_AverageOneCycle_Time);
            this.baseGroupBox_WorkingTime.Controls.Add(this.progressBar_TotalRemained_Time);
            this.baseGroupBox_WorkingTime.Controls.Add(this.baseLabel_Total_RemainedTime);
            this.baseGroupBox_WorkingTime.Controls.Add(this.baseLabel_TotalRunning_Time);
            this.baseGroupBox_WorkingTime.Controls.Add(this.progressBar_OneCycle_Time);
            this.baseGroupBox_WorkingTime.Controls.Add(this.baseLabel_CurrentOneCycle_TotalTime);
            this.baseGroupBox_WorkingTime.Controls.Add(this.baseLabel_CurrentOneCycle_ElapsedTime);
            this.baseGroupBox_WorkingTime.Controls.Add(this.baseLabel_CurrentOneCycle_Time);
            this.baseGroupBox_WorkingTime.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_WorkingTime.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox_WorkingTime.Location = new System.Drawing.Point(14, 33);
            this.baseGroupBox_WorkingTime.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseGroupBox_WorkingTime.Name = "baseGroupBox_WorkingTime";
            this.baseGroupBox_WorkingTime.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseGroupBox_WorkingTime.Size = new System.Drawing.Size(427, 202);
            this.baseGroupBox_WorkingTime.TabIndex = 192;
            this.baseGroupBox_WorkingTime.TabStop = false;
            this.baseGroupBox_WorkingTime.Text = " [ Working Time ] ";
            // 
            // button_AverageOneCycleTime_Clear
            // 
            this.button_AverageOneCycleTime_Clear.BackColor = System.Drawing.Color.White;
            this.button_AverageOneCycleTime_Clear.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_AverageOneCycleTime_Clear.FlatAppearance.BorderSize = 2;
            this.button_AverageOneCycleTime_Clear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_AverageOneCycleTime_Clear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_AverageOneCycleTime_Clear.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_AverageOneCycleTime_Clear.ForeColor = System.Drawing.Color.Black;
            this.button_AverageOneCycleTime_Clear.Location = new System.Drawing.Point(348, 159);
            this.button_AverageOneCycleTime_Clear.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_AverageOneCycleTime_Clear.Name = "button_AverageOneCycleTime_Clear";
            this.button_AverageOneCycleTime_Clear.Size = new System.Drawing.Size(67, 33);
            this.button_AverageOneCycleTime_Clear.TabIndex = 129;
            this.button_AverageOneCycleTime_Clear.Text = "Clear";
            this.button_AverageOneCycleTime_Clear.UseVisualStyleBackColor = false;
            // 
            // baseLabel_Average_OneCycleTime
            // 
            this.baseLabel_Average_OneCycleTime.AutoSize = true;
            this.baseLabel_Average_OneCycleTime.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_Average_OneCycleTime.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Average_OneCycleTime.Location = new System.Drawing.Point(228, 166);
            this.baseLabel_Average_OneCycleTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Average_OneCycleTime.Name = "baseLabel_Average_OneCycleTime";
            this.baseLabel_Average_OneCycleTime.Size = new System.Drawing.Size(66, 18);
            this.baseLabel_Average_OneCycleTime.TabIndex = 117;
            this.baseLabel_Average_OneCycleTime.Text = "00:00:00";
            // 
            // baseLabel_AverageOneCycle_Time
            // 
            this.baseLabel_AverageOneCycle_Time.AutoSize = true;
            this.baseLabel_AverageOneCycle_Time.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_AverageOneCycle_Time.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_AverageOneCycle_Time.Location = new System.Drawing.Point(12, 166);
            this.baseLabel_AverageOneCycle_Time.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_AverageOneCycle_Time.Name = "baseLabel_AverageOneCycle_Time";
            this.baseLabel_AverageOneCycle_Time.Size = new System.Drawing.Size(171, 18);
            this.baseLabel_AverageOneCycle_Time.TabIndex = 116;
            this.baseLabel_AverageOneCycle_Time.Text = "Average One Cycle Time";
            // 
            // progressBar_TotalRemained_Time
            // 
            this.progressBar_TotalRemained_Time.Location = new System.Drawing.Point(12, 121);
            this.progressBar_TotalRemained_Time.Name = "progressBar_TotalRemained_Time";
            this.progressBar_TotalRemained_Time.Size = new System.Drawing.Size(403, 21);
            this.progressBar_TotalRemained_Time.TabIndex = 115;
            this.progressBar_TotalRemained_Time.Value = 50;
            // 
            // baseLabel_Total_RemainedTime
            // 
            this.baseLabel_Total_RemainedTime.AutoSize = true;
            this.baseLabel_Total_RemainedTime.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_Total_RemainedTime.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Total_RemainedTime.Location = new System.Drawing.Point(228, 97);
            this.baseLabel_Total_RemainedTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Total_RemainedTime.Name = "baseLabel_Total_RemainedTime";
            this.baseLabel_Total_RemainedTime.Size = new System.Drawing.Size(66, 18);
            this.baseLabel_Total_RemainedTime.TabIndex = 114;
            this.baseLabel_Total_RemainedTime.Text = "00:00:00";
            // 
            // baseLabel_TotalRunning_Time
            // 
            this.baseLabel_TotalRunning_Time.AutoSize = true;
            this.baseLabel_TotalRunning_Time.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_TotalRunning_Time.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_TotalRunning_Time.Location = new System.Drawing.Point(12, 97);
            this.baseLabel_TotalRunning_Time.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_TotalRunning_Time.Name = "baseLabel_TotalRunning_Time";
            this.baseLabel_TotalRunning_Time.Size = new System.Drawing.Size(135, 18);
            this.baseLabel_TotalRunning_Time.TabIndex = 113;
            this.baseLabel_TotalRunning_Time.Text = "Total Running Time";
            // 
            // progressBar_OneCycle_Time
            // 
            this.progressBar_OneCycle_Time.Location = new System.Drawing.Point(12, 58);
            this.progressBar_OneCycle_Time.Name = "progressBar_OneCycle_Time";
            this.progressBar_OneCycle_Time.Size = new System.Drawing.Size(403, 21);
            this.progressBar_OneCycle_Time.TabIndex = 112;
            this.progressBar_OneCycle_Time.Value = 50;
            // 
            // baseLabel_CurrentOneCycle_TotalTime
            // 
            this.baseLabel_CurrentOneCycle_TotalTime.AutoSize = true;
            this.baseLabel_CurrentOneCycle_TotalTime.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_CurrentOneCycle_TotalTime.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_CurrentOneCycle_TotalTime.Location = new System.Drawing.Point(346, 34);
            this.baseLabel_CurrentOneCycle_TotalTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_CurrentOneCycle_TotalTime.Name = "baseLabel_CurrentOneCycle_TotalTime";
            this.baseLabel_CurrentOneCycle_TotalTime.Size = new System.Drawing.Size(66, 18);
            this.baseLabel_CurrentOneCycle_TotalTime.TabIndex = 111;
            this.baseLabel_CurrentOneCycle_TotalTime.Text = "00:00:00";
            // 
            // baseLabel_CurrentOneCycle_ElapsedTime
            // 
            this.baseLabel_CurrentOneCycle_ElapsedTime.AutoSize = true;
            this.baseLabel_CurrentOneCycle_ElapsedTime.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_CurrentOneCycle_ElapsedTime.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_CurrentOneCycle_ElapsedTime.Location = new System.Drawing.Point(228, 34);
            this.baseLabel_CurrentOneCycle_ElapsedTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_CurrentOneCycle_ElapsedTime.Name = "baseLabel_CurrentOneCycle_ElapsedTime";
            this.baseLabel_CurrentOneCycle_ElapsedTime.Size = new System.Drawing.Size(66, 18);
            this.baseLabel_CurrentOneCycle_ElapsedTime.TabIndex = 110;
            this.baseLabel_CurrentOneCycle_ElapsedTime.Text = "00:00:00";
            // 
            // baseLabel_CurrentOneCycle_Time
            // 
            this.baseLabel_CurrentOneCycle_Time.AutoSize = true;
            this.baseLabel_CurrentOneCycle_Time.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_CurrentOneCycle_Time.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_CurrentOneCycle_Time.Location = new System.Drawing.Point(12, 34);
            this.baseLabel_CurrentOneCycle_Time.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_CurrentOneCycle_Time.Name = "baseLabel_CurrentOneCycle_Time";
            this.baseLabel_CurrentOneCycle_Time.Size = new System.Drawing.Size(112, 18);
            this.baseLabel_CurrentOneCycle_Time.TabIndex = 109;
            this.baseLabel_CurrentOneCycle_Time.Text = "One Cycle Time";
            // 
            // groupBox18
            // 
            this.groupBox18.Controls.Add(this.listView_Main_FiducialAlignData);
            this.groupBox18.Font = new System.Drawing.Font("Tahoma", 10F);
            this.groupBox18.Location = new System.Drawing.Point(459, 160);
            this.groupBox18.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.groupBox18.Name = "groupBox18";
            this.groupBox18.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox18.Size = new System.Drawing.Size(426, 75);
            this.groupBox18.TabIndex = 89;
            this.groupBox18.TabStop = false;
            this.groupBox18.Text = " Fiducial Align Data ";
            // 
            // listView_Main_FiducialAlignData
            // 
            this.listView_Main_FiducialAlignData.FullRowSelect = true;
            this.listView_Main_FiducialAlignData.HideSelection = false;
            this.listView_Main_FiducialAlignData.Location = new System.Drawing.Point(12, 26);
            this.listView_Main_FiducialAlignData.Name = "listView_Main_FiducialAlignData";
            this.listView_Main_FiducialAlignData.Size = new System.Drawing.Size(405, 39);
            this.listView_Main_FiducialAlignData.TabIndex = 0;
            this.listView_Main_FiducialAlignData.UseCompatibleStateImageBehavior = false;
            // 
            // groupBoxMain_MaterialInformation
            // 
            this.groupBoxMain_MaterialInformation.Controls.Add(this.baseTextBox_Socket_Index);
            this.groupBoxMain_MaterialInformation.Controls.Add(this.baseTextBox_SocketCountPerModule);
            this.groupBoxMain_MaterialInformation.Controls.Add(this.baseLabel_SocketPerModule);
            this.groupBoxMain_MaterialInformation.Controls.Add(this.baseLabel1);
            this.groupBoxMain_MaterialInformation.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxMain_MaterialInformation.Location = new System.Drawing.Point(1347, 9);
            this.groupBoxMain_MaterialInformation.Name = "groupBoxMain_MaterialInformation";
            this.groupBoxMain_MaterialInformation.Size = new System.Drawing.Size(323, 112);
            this.groupBoxMain_MaterialInformation.TabIndex = 2;
            this.groupBoxMain_MaterialInformation.TabStop = false;
            this.groupBoxMain_MaterialInformation.Text = " Material Information ";
            // 
            // baseTextBox_Socket_Index
            // 
            this.baseTextBox_Socket_Index.BackColor = System.Drawing.Color.White;
            this.baseTextBox_Socket_Index.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseTextBox_Socket_Index.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseTextBox_Socket_Index.ForeColor = System.Drawing.Color.Black;
            this.baseTextBox_Socket_Index.Location = new System.Drawing.Point(208, 73);
            this.baseTextBox_Socket_Index.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseTextBox_Socket_Index.Name = "baseTextBox_Socket_Index";
            this.baseTextBox_Socket_Index.ReadOnly = true;
            this.baseTextBox_Socket_Index.Size = new System.Drawing.Size(98, 26);
            this.baseTextBox_Socket_Index.TabIndex = 140;
            this.baseTextBox_Socket_Index.Text = "0";
            this.baseTextBox_Socket_Index.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseTextBox_SocketCountPerModule
            // 
            this.baseTextBox_SocketCountPerModule.BackColor = System.Drawing.Color.White;
            this.baseTextBox_SocketCountPerModule.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseTextBox_SocketCountPerModule.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseTextBox_SocketCountPerModule.ForeColor = System.Drawing.Color.Black;
            this.baseTextBox_SocketCountPerModule.Location = new System.Drawing.Point(208, 38);
            this.baseTextBox_SocketCountPerModule.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseTextBox_SocketCountPerModule.Name = "baseTextBox_SocketCountPerModule";
            this.baseTextBox_SocketCountPerModule.ReadOnly = true;
            this.baseTextBox_SocketCountPerModule.Size = new System.Drawing.Size(98, 26);
            this.baseTextBox_SocketCountPerModule.TabIndex = 124;
            this.baseTextBox_SocketCountPerModule.Text = "0";
            this.baseTextBox_SocketCountPerModule.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseLabel_SocketPerModule
            // 
            this.baseLabel_SocketPerModule.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_SocketPerModule.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_SocketPerModule.Location = new System.Drawing.Point(15, 37);
            this.baseLabel_SocketPerModule.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_SocketPerModule.Name = "baseLabel_SocketPerModule";
            this.baseLabel_SocketPerModule.Size = new System.Drawing.Size(185, 26);
            this.baseLabel_SocketPerModule.TabIndex = 123;
            this.baseLabel_SocketPerModule.Text = "Socket count per Module";
            this.baseLabel_SocketPerModule.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // baseLabel1
            // 
            this.baseLabel1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel1.ForeColor = System.Drawing.Color.Black;
            this.baseLabel1.Location = new System.Drawing.Point(15, 72);
            this.baseLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel1.Name = "baseLabel1";
            this.baseLabel1.Size = new System.Drawing.Size(185, 26);
            this.baseLabel1.TabIndex = 139;
            this.baseLabel1.Text = "Selected Socket Index :";
            this.baseLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button_Main_Stop
            // 
            this.button_Main_Stop.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_Main_Stop.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.button_Main_Stop.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_Main_Stop.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.button_Main_Stop.Location = new System.Drawing.Point(1726, 520);
            this.button_Main_Stop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_Main_Stop.Name = "button_Main_Stop";
            this.button_Main_Stop.Size = new System.Drawing.Size(178, 73);
            this.button_Main_Stop.TabIndex = 22;
            this.button_Main_Stop.Text = "Stop";
            this.button_Main_Stop.UseVisualStyleBackColor = false;
            this.button_Main_Stop.Click += new System.EventHandler(this.button_Main_Stop_Click);
            // 
            // button_Main_Pause
            // 
            this.button_Main_Pause.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_Main_Pause.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.button_Main_Pause.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_Main_Pause.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.button_Main_Pause.Location = new System.Drawing.Point(1726, 599);
            this.button_Main_Pause.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_Main_Pause.Name = "button_Main_Pause";
            this.button_Main_Pause.Size = new System.Drawing.Size(178, 73);
            this.button_Main_Pause.TabIndex = 21;
            this.button_Main_Pause.Text = "Pause";
            this.button_Main_Pause.UseVisualStyleBackColor = false;
            this.button_Main_Pause.Click += new System.EventHandler(this.button_Main_Pause_Click);
            // 
            // button_Main_Start
            // 
            this.button_Main_Start.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_Main_Start.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.button_Main_Start.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_Main_Start.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.button_Main_Start.Location = new System.Drawing.Point(1726, 444);
            this.button_Main_Start.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_Main_Start.Name = "button_Main_Start";
            this.button_Main_Start.Size = new System.Drawing.Size(178, 73);
            this.button_Main_Start.TabIndex = 20;
            this.button_Main_Start.Text = "Start";
            this.button_Main_Start.UseVisualStyleBackColor = false;
            this.button_Main_Start.Click += new System.EventHandler(this.button_Main_Start_Click);
            // 
            // groupBox_ProcessingData
            // 
            this.groupBox_ProcessingData.Controls.Add(this.SiriusViewer_Main);
            this.groupBox_ProcessingData.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_ProcessingData.Location = new System.Drawing.Point(12, 285);
            this.groupBox_ProcessingData.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox_ProcessingData.Name = "groupBox_ProcessingData";
            this.groupBox_ProcessingData.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox_ProcessingData.Size = new System.Drawing.Size(711, 581);
            this.groupBox_ProcessingData.TabIndex = 19;
            this.groupBox_ProcessingData.TabStop = false;
            this.groupBox_ProcessingData.Text = "Processing Data";
            // 
            // button_Main_RecipeOpen
            // 
            this.button_Main_RecipeOpen.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_Main_RecipeOpen.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.button_Main_RecipeOpen.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_Main_RecipeOpen.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.button_Main_RecipeOpen.Location = new System.Drawing.Point(1503, 124);
            this.button_Main_RecipeOpen.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_Main_RecipeOpen.Name = "button_Main_RecipeOpen";
            this.button_Main_RecipeOpen.Size = new System.Drawing.Size(167, 44);
            this.button_Main_RecipeOpen.TabIndex = 5;
            this.button_Main_RecipeOpen.Text = "Recipe Open";
            this.button_Main_RecipeOpen.UseVisualStyleBackColor = false;
            this.button_Main_RecipeOpen.Click += new System.EventHandler(this.button_Main_RecipeOpen_Click);
            // 
            // checkBox_Main_CycleStop
            // 
            this.checkBox_Main_CycleStop.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.checkBox_Main_CycleStop.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.checkBox_Main_CycleStop.Location = new System.Drawing.Point(1726, 816);
            this.checkBox_Main_CycleStop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox_Main_CycleStop.Name = "checkBox_Main_CycleStop";
            this.checkBox_Main_CycleStop.Size = new System.Drawing.Size(178, 50);
            this.checkBox_Main_CycleStop.TabIndex = 55;
            this.checkBox_Main_CycleStop.Text = "Cycle Stop";
            this.checkBox_Main_CycleStop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox_Main_CycleStop.UseVisualStyleBackColor = false;
            this.checkBox_Main_CycleStop.CheckedChanged += new System.EventHandler(this.checkBox_Main_CycleStop_CheckedChanged);
            // 
            // button_Main_Reset
            // 
            this.button_Main_Reset.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_Main_Reset.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.button_Main_Reset.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_Main_Reset.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.button_Main_Reset.Location = new System.Drawing.Point(1726, 676);
            this.button_Main_Reset.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_Main_Reset.Name = "button_Main_Reset";
            this.button_Main_Reset.Size = new System.Drawing.Size(178, 73);
            this.button_Main_Reset.TabIndex = 54;
            this.button_Main_Reset.Text = "Reset";
            this.button_Main_Reset.UseVisualStyleBackColor = false;
            this.button_Main_Reset.Click += new System.EventHandler(this.button_Main_Reset_Click);
            // 
            // button_Main_Home
            // 
            this.button_Main_Home.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_Main_Home.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.button_Main_Home.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_Main_Home.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.button_Main_Home.Location = new System.Drawing.Point(1726, 15);
            this.button_Main_Home.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_Main_Home.Name = "button_Main_Home";
            this.button_Main_Home.Size = new System.Drawing.Size(178, 78);
            this.button_Main_Home.TabIndex = 56;
            this.button_Main_Home.Text = "Machine\r\nInitialize";
            this.button_Main_Home.UseVisualStyleBackColor = false;
            this.button_Main_Home.Click += new System.EventHandler(this.button_Main_Home_Click);
            // 
            // button_Main_RtcInit
            // 
            this.button_Main_RtcInit.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_Main_RtcInit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.button_Main_RtcInit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_Main_RtcInit.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.button_Main_RtcInit.Location = new System.Drawing.Point(1726, 100);
            this.button_Main_RtcInit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_Main_RtcInit.Name = "button_Main_RtcInit";
            this.button_Main_RtcInit.Size = new System.Drawing.Size(178, 63);
            this.button_Main_RtcInit.TabIndex = 57;
            this.button_Main_RtcInit.Text = "Scanner Board\r\nOpen";
            this.button_Main_RtcInit.UseVisualStyleBackColor = false;
            this.button_Main_RtcInit.Click += new System.EventHandler(this.button_Main_RtcInit_Click);
            // 
            // checkBox_Test_DryRun
            // 
            this.checkBox_Test_DryRun.AutoSize = true;
            this.checkBox_Test_DryRun.Location = new System.Drawing.Point(1833, 248);
            this.checkBox_Test_DryRun.Name = "checkBox_Test_DryRun";
            this.checkBox_Test_DryRun.Size = new System.Drawing.Size(69, 18);
            this.checkBox_Test_DryRun.TabIndex = 59;
            this.checkBox_Test_DryRun.Text = "Dry Run";
            this.checkBox_Test_DryRun.UseVisualStyleBackColor = true;
            // 
            // checkBox_Main_Loader_LPort_Pause
            // 
            this.checkBox_Main_Loader_LPort_Pause.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.checkBox_Main_Loader_LPort_Pause.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.checkBox_Main_Loader_LPort_Pause.Location = new System.Drawing.Point(1476, 297);
            this.checkBox_Main_Loader_LPort_Pause.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox_Main_Loader_LPort_Pause.Name = "checkBox_Main_Loader_LPort_Pause";
            this.checkBox_Main_Loader_LPort_Pause.Size = new System.Drawing.Size(94, 62);
            this.checkBox_Main_Loader_LPort_Pause.TabIndex = 147;
            this.checkBox_Main_Loader_LPort_Pause.Text = "Loader L-Port Pause";
            this.checkBox_Main_Loader_LPort_Pause.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox_Main_Loader_LPort_Pause.UseVisualStyleBackColor = false;
            this.checkBox_Main_Loader_LPort_Pause.CheckedChanged += new System.EventHandler(this.checkBox_Main_Loader_LPort_Pause_CheckedChanged);
            // 
            // checkBox_Main_Loader_RPort_Pause
            // 
            this.checkBox_Main_Loader_RPort_Pause.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.checkBox_Main_Loader_RPort_Pause.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.checkBox_Main_Loader_RPort_Pause.Location = new System.Drawing.Point(1576, 297);
            this.checkBox_Main_Loader_RPort_Pause.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox_Main_Loader_RPort_Pause.Name = "checkBox_Main_Loader_RPort_Pause";
            this.checkBox_Main_Loader_RPort_Pause.Size = new System.Drawing.Size(94, 62);
            this.checkBox_Main_Loader_RPort_Pause.TabIndex = 148;
            this.checkBox_Main_Loader_RPort_Pause.Text = "Loader R-Port Pause";
            this.checkBox_Main_Loader_RPort_Pause.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox_Main_Loader_RPort_Pause.UseVisualStyleBackColor = false;
            this.checkBox_Main_Loader_RPort_Pause.CheckedChanged += new System.EventHandler(this.checkBox_Main_Loader_RPort_Pause_CheckedChanged);
            // 
            // button_Main_CameraInit
            // 
            this.button_Main_CameraInit.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_Main_CameraInit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.button_Main_CameraInit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_Main_CameraInit.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.button_Main_CameraInit.Location = new System.Drawing.Point(1726, 170);
            this.button_Main_CameraInit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_Main_CameraInit.Name = "button_Main_CameraInit";
            this.button_Main_CameraInit.Size = new System.Drawing.Size(178, 63);
            this.button_Main_CameraInit.TabIndex = 153;
            this.button_Main_CameraInit.Text = "Camera Open";
            this.button_Main_CameraInit.UseVisualStyleBackColor = false;
            this.button_Main_CameraInit.Click += new System.EventHandler(this.button_Main_CameraInit_Click);
            // 
            // checkBox_Main_SocketStop
            // 
            this.checkBox_Main_SocketStop.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.checkBox_Main_SocketStop.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.checkBox_Main_SocketStop.Location = new System.Drawing.Point(1726, 755);
            this.checkBox_Main_SocketStop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox_Main_SocketStop.Name = "checkBox_Main_SocketStop";
            this.checkBox_Main_SocketStop.Size = new System.Drawing.Size(178, 50);
            this.checkBox_Main_SocketStop.TabIndex = 154;
            this.checkBox_Main_SocketStop.Text = "Socket Stop";
            this.checkBox_Main_SocketStop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox_Main_SocketStop.UseVisualStyleBackColor = false;
            this.checkBox_Main_SocketStop.CheckedChanged += new System.EventHandler(this.checkBox_Main_SocketStop_CheckedChanged);
            // 
            // buttonForceMaterialOut
            // 
            this.buttonForceMaterialOut.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.buttonForceMaterialOut.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.buttonForceMaterialOut.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonForceMaterialOut.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.buttonForceMaterialOut.Location = new System.Drawing.Point(1576, 366);
            this.buttonForceMaterialOut.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonForceMaterialOut.Name = "buttonForceMaterialOut";
            this.buttonForceMaterialOut.Size = new System.Drawing.Size(94, 62);
            this.buttonForceMaterialOut.TabIndex = 158;
            this.buttonForceMaterialOut.Text = "강제배출";
            this.buttonForceMaterialOut.UseVisualStyleBackColor = false;
            this.buttonForceMaterialOut.Click += new System.EventHandler(this.buttonForceMaterialOut_Click);
            // 
            // groupBox_FineCam
            // 
            this.groupBox_FineCam.Controls.Add(this.ImageViewer_Main_highs);
            this.groupBox_FineCam.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_FineCam.Location = new System.Drawing.Point(744, 285);
            this.groupBox_FineCam.Name = "groupBox_FineCam";
            this.groupBox_FineCam.Size = new System.Drawing.Size(315, 286);
            this.groupBox_FineCam.TabIndex = 191;
            this.groupBox_FineCam.TabStop = false;
            this.groupBox_FineCam.Text = " Fine Camera ";
            // 
            // ImageViewer_Main_highs
            // 
            this.ImageViewer_Main_highs.BackColor = System.Drawing.Color.Black;
            this.ImageViewer_Main_highs.Camera = null;
            this.ImageViewer_Main_highs.CameraSwitch = null;
            this.ImageViewer_Main_highs.FrameRate = 1D;
            this.ImageViewer_Main_highs.InputImage = null;
            this.ImageViewer_Main_highs.IsViewCustomizedImage = false;
            this.ImageViewer_Main_highs.Location = new System.Drawing.Point(10, 28);
            this.ImageViewer_Main_highs.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ImageViewer_Main_highs.Name = "ImageViewer_Main_highs";
            this.ImageViewer_Main_highs.OperatingType = QMC.Common.Hmi.VisionImageViewer.OperatingTypes.Center;
            this.ImageViewer_Main_highs.Simulated = false;
            this.ImageViewer_Main_highs.Size = new System.Drawing.Size(295, 247);
            this.ImageViewer_Main_highs.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ImageViewer_Main_highs.TabIndex = 189;
            this.ImageViewer_Main_highs.TabStop = false;
            this.ImageViewer_Main_highs.UpdateDelayTime = 160;
            this.ImageViewer_Main_highs.VisibleCrossLine = true;
            // 
            // groupBox_CoarseCam
            // 
            this.groupBox_CoarseCam.Controls.Add(this.ImageViewer_Main_Rows);
            this.groupBox_CoarseCam.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_CoarseCam.Location = new System.Drawing.Point(744, 580);
            this.groupBox_CoarseCam.Name = "groupBox_CoarseCam";
            this.groupBox_CoarseCam.Size = new System.Drawing.Size(315, 286);
            this.groupBox_CoarseCam.TabIndex = 192;
            this.groupBox_CoarseCam.TabStop = false;
            this.groupBox_CoarseCam.Text = " Coarse Camera ";
            // 
            // ImageViewer_Main_Rows
            // 
            this.ImageViewer_Main_Rows.BackColor = System.Drawing.Color.Black;
            this.ImageViewer_Main_Rows.Camera = null;
            this.ImageViewer_Main_Rows.CameraSwitch = null;
            this.ImageViewer_Main_Rows.FrameRate = 1D;
            this.ImageViewer_Main_Rows.InputImage = null;
            this.ImageViewer_Main_Rows.IsViewCustomizedImage = false;
            this.ImageViewer_Main_Rows.Location = new System.Drawing.Point(10, 28);
            this.ImageViewer_Main_Rows.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ImageViewer_Main_Rows.Name = "ImageViewer_Main_Rows";
            this.ImageViewer_Main_Rows.OperatingType = QMC.Common.Hmi.VisionImageViewer.OperatingTypes.Center;
            this.ImageViewer_Main_Rows.Simulated = false;
            this.ImageViewer_Main_Rows.Size = new System.Drawing.Size(295, 247);
            this.ImageViewer_Main_Rows.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ImageViewer_Main_Rows.TabIndex = 190;
            this.ImageViewer_Main_Rows.TabStop = false;
            this.ImageViewer_Main_Rows.UpdateDelayTime = 160;
            this.ImageViewer_Main_Rows.VisibleCrossLine = true;
            // 
            // checkBox_Main_AutoRun
            // 
            this.checkBox_Main_AutoRun.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBox_Main_AutoRun.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.checkBox_Main_AutoRun.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.checkBox_Main_AutoRun.Location = new System.Drawing.Point(1726, 368);
            this.checkBox_Main_AutoRun.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox_Main_AutoRun.Name = "checkBox_Main_AutoRun";
            this.checkBox_Main_AutoRun.Size = new System.Drawing.Size(178, 73);
            this.checkBox_Main_AutoRun.TabIndex = 191;
            this.checkBox_Main_AutoRun.Text = "AutoRun";
            this.checkBox_Main_AutoRun.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox_Main_AutoRun.UseVisualStyleBackColor = false;
            this.checkBox_Main_AutoRun.CheckedChanged += new System.EventHandler(this.checkBox_Main_AutoRun_CheckedChanged);
            // 
            // button_TEST12
            // 
            this.button_TEST12.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_TEST12.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.button_TEST12.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_TEST12.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.button_TEST12.Location = new System.Drawing.Point(1065, 842);
            this.button_TEST12.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_TEST12.Name = "button_TEST12";
            this.button_TEST12.Size = new System.Drawing.Size(57, 24);
            this.button_TEST12.TabIndex = 192;
            this.button_TEST12.Text = "TEST";
            this.button_TEST12.UseVisualStyleBackColor = false;
            this.button_TEST12.Click += new System.EventHandler(this.button_TEST12_Click);
            // 
            // button_TestbyUser_LPort_Start
            // 
            this.button_TestbyUser_LPort_Start.BackColor = System.Drawing.Color.DarkGray;
            this.button_TestbyUser_LPort_Start.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_TestbyUser_LPort_Start.Location = new System.Drawing.Point(1476, 366);
            this.button_TestbyUser_LPort_Start.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.button_TestbyUser_LPort_Start.Name = "button_TestbyUser_LPort_Start";
            this.button_TestbyUser_LPort_Start.Size = new System.Drawing.Size(94, 62);
            this.button_TestbyUser_LPort_Start.TabIndex = 193;
            this.button_TestbyUser_LPort_Start.Text = "LD L-Port Start\r\n(by User)";
            this.button_TestbyUser_LPort_Start.UseVisualStyleBackColor = false;
            this.button_TestbyUser_LPort_Start.Click += new System.EventHandler(this.button_TestbyUser_LPort_Start_Click);
            // 
            // label_Title_Stacker_LPort
            // 
            this.label_Title_Stacker_LPort.BackColor = System.Drawing.Color.Black;
            this.label_Title_Stacker_LPort.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_Title_Stacker_LPort.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Title_Stacker_LPort.ForeColor = System.Drawing.Color.Lime;
            this.label_Title_Stacker_LPort.Location = new System.Drawing.Point(1348, 170);
            this.label_Title_Stacker_LPort.Name = "label_Title_Stacker_LPort";
            this.label_Title_Stacker_LPort.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.label_Title_Stacker_LPort.Size = new System.Drawing.Size(322, 40);
            this.label_Title_Stacker_LPort.TabIndex = 205;
            this.label_Title_Stacker_LPort.Text = "R Stacker 자재 유/무";
            this.label_Title_Stacker_LPort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_Title_Stacker_RPort
            // 
            this.label_Title_Stacker_RPort.BackColor = System.Drawing.Color.Black;
            this.label_Title_Stacker_RPort.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_Title_Stacker_RPort.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Title_Stacker_RPort.ForeColor = System.Drawing.Color.Lime;
            this.label_Title_Stacker_RPort.Location = new System.Drawing.Point(1348, 219);
            this.label_Title_Stacker_RPort.Name = "label_Title_Stacker_RPort";
            this.label_Title_Stacker_RPort.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.label_Title_Stacker_RPort.Size = new System.Drawing.Size(322, 40);
            this.label_Title_Stacker_RPort.TabIndex = 206;
            this.label_Title_Stacker_RPort.Text = "L Stacker 자재 유/무";
            this.label_Title_Stacker_RPort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox_Main_DiviceStatus
            // 
            this.groupBox_Main_DiviceStatus.Controls.Add(this.baseLabel_Main_Divice_Status_Illuminator);
            this.groupBox_Main_DiviceStatus.Controls.Add(this.pictureBox_Main_DiviceStatus_Illuminator);
            this.groupBox_Main_DiviceStatus.Controls.Add(this.baseLabel_Main_Divice_Status_CameraPre);
            this.groupBox_Main_DiviceStatus.Controls.Add(this.pictureBox_Main_DiviceStatus_CameraPre);
            this.groupBox_Main_DiviceStatus.Controls.Add(this.baseLabel_Main_Divice_Status_CameraFine);
            this.groupBox_Main_DiviceStatus.Controls.Add(this.pictureBox_Main_DiviceStatus_CameraFine);
            this.groupBox_Main_DiviceStatus.Controls.Add(this.baseLabel_Main_Divice_Status_heightsensor);
            this.groupBox_Main_DiviceStatus.Controls.Add(this.pictureBox_Main_DiviceStatus_HeightSensor);
            this.groupBox_Main_DiviceStatus.Controls.Add(this.baseLabel_Main_Divice_Status_ElectroRequlator);
            this.groupBox_Main_DiviceStatus.Controls.Add(this.pictureBox_Main_DiviceStatus_ElectroRegulator);
            this.groupBox_Main_DiviceStatus.Controls.Add(this.baseLabel_Main_Divice_Status_BeamExpander);
            this.groupBox_Main_DiviceStatus.Controls.Add(this.pictureBox_Main_DiviceStatus_BeamExpander);
            this.groupBox_Main_DiviceStatus.Controls.Add(this.baseLabel_Main_Divice_Status_Chiller);
            this.groupBox_Main_DiviceStatus.Controls.Add(this.pictureBox_Main_DiviceStatus_Chiller);
            this.groupBox_Main_DiviceStatus.Controls.Add(this.baseLabel_Main_Divice_Status_Stage);
            this.groupBox_Main_DiviceStatus.Controls.Add(this.pictureBox_Main_DiviceStatus_Powermeter_Stage);
            this.groupBox_Main_DiviceStatus.Controls.Add(this.baseLabel_Main_Divice_Status_PowermeterBds);
            this.groupBox_Main_DiviceStatus.Controls.Add(this.pictureBox_Main_DiviceStatus_Powermeter_bds);
            this.groupBox_Main_DiviceStatus.Controls.Add(this.baseLabel_Main_Divice_Status_DustcollectorLower);
            this.groupBox_Main_DiviceStatus.Controls.Add(this.pictureBox_Main_DiviceStatus_DustCollector_Lower);
            this.groupBox_Main_DiviceStatus.Controls.Add(this.baseLabel_Main_Divice_Status_DustcollectorUpper);
            this.groupBox_Main_DiviceStatus.Controls.Add(this.pictureBox_Main_DiviceStatus_DustCollector_Upper);
            this.groupBox_Main_DiviceStatus.Controls.Add(this.baseLabel_Main_Divice_Status_Scanner);
            this.groupBox_Main_DiviceStatus.Controls.Add(this.pictureBox_Main_DiviceStatus_Scanner);
            this.groupBox_Main_DiviceStatus.Controls.Add(this.baseLabel_Main_Divice_Status_Motion);
            this.groupBox_Main_DiviceStatus.Controls.Add(this.pictureBox_Main_DiviceStatus_Motion);
            this.groupBox_Main_DiviceStatus.Controls.Add(this.baseLabel_Main_Divice_Status_IO);
            this.groupBox_Main_DiviceStatus.Controls.Add(this.pictureBox_Main_DiviceStatus_IO);
            this.groupBox_Main_DiviceStatus.Controls.Add(this.baseLabel_Main_Divice_Status_Laser);
            this.groupBox_Main_DiviceStatus.Controls.Add(this.pictureBox_Main_DiviceStatus_Laser);
            this.groupBox_Main_DiviceStatus.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold);
            this.groupBox_Main_DiviceStatus.Location = new System.Drawing.Point(1117, 676);
            this.groupBox_Main_DiviceStatus.Name = "groupBox_Main_DiviceStatus";
            this.groupBox_Main_DiviceStatus.Size = new System.Drawing.Size(553, 190);
            this.groupBox_Main_DiviceStatus.TabIndex = 207;
            this.groupBox_Main_DiviceStatus.TabStop = false;
            this.groupBox_Main_DiviceStatus.Text = " Device Status ";
            // 
            // baseLabel_Main_Divice_Status_Illuminator
            // 
            this.baseLabel_Main_Divice_Status_Illuminator.AutoSize = true;
            this.baseLabel_Main_Divice_Status_Illuminator.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_Main_Divice_Status_Illuminator.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Main_Divice_Status_Illuminator.Location = new System.Drawing.Point(398, 158);
            this.baseLabel_Main_Divice_Status_Illuminator.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Main_Divice_Status_Illuminator.Name = "baseLabel_Main_Divice_Status_Illuminator";
            this.baseLabel_Main_Divice_Status_Illuminator.Size = new System.Drawing.Size(75, 18);
            this.baseLabel_Main_Divice_Status_Illuminator.TabIndex = 239;
            this.baseLabel_Main_Divice_Status_Illuminator.Text = "Illuminator";
            // 
            // pictureBox_Main_DiviceStatus_Illuminator
            // 
            this.pictureBox_Main_DiviceStatus_Illuminator.Image = global::SLD200.Properties.Resources.DioEllipseOff;
            this.pictureBox_Main_DiviceStatus_Illuminator.Location = new System.Drawing.Point(369, 155);
            this.pictureBox_Main_DiviceStatus_Illuminator.Name = "pictureBox_Main_DiviceStatus_Illuminator";
            this.pictureBox_Main_DiviceStatus_Illuminator.Size = new System.Drawing.Size(25, 25);
            this.pictureBox_Main_DiviceStatus_Illuminator.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Main_DiviceStatus_Illuminator.TabIndex = 240;
            this.pictureBox_Main_DiviceStatus_Illuminator.TabStop = false;
            // 
            // baseLabel_Main_Divice_Status_CameraPre
            // 
            this.baseLabel_Main_Divice_Status_CameraPre.AutoSize = true;
            this.baseLabel_Main_Divice_Status_CameraPre.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_Main_Divice_Status_CameraPre.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Main_Divice_Status_CameraPre.Location = new System.Drawing.Point(398, 127);
            this.baseLabel_Main_Divice_Status_CameraPre.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Main_Divice_Status_CameraPre.Name = "baseLabel_Main_Divice_Status_CameraPre";
            this.baseLabel_Main_Divice_Status_CameraPre.Size = new System.Drawing.Size(80, 18);
            this.baseLabel_Main_Divice_Status_CameraPre.TabIndex = 237;
            this.baseLabel_Main_Divice_Status_CameraPre.Text = "CameraPre";
            // 
            // pictureBox_Main_DiviceStatus_CameraPre
            // 
            this.pictureBox_Main_DiviceStatus_CameraPre.Image = global::SLD200.Properties.Resources.DioEllipseOff;
            this.pictureBox_Main_DiviceStatus_CameraPre.Location = new System.Drawing.Point(369, 124);
            this.pictureBox_Main_DiviceStatus_CameraPre.Name = "pictureBox_Main_DiviceStatus_CameraPre";
            this.pictureBox_Main_DiviceStatus_CameraPre.Size = new System.Drawing.Size(25, 25);
            this.pictureBox_Main_DiviceStatus_CameraPre.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Main_DiviceStatus_CameraPre.TabIndex = 238;
            this.pictureBox_Main_DiviceStatus_CameraPre.TabStop = false;
            // 
            // baseLabel_Main_Divice_Status_CameraFine
            // 
            this.baseLabel_Main_Divice_Status_CameraFine.AutoSize = true;
            this.baseLabel_Main_Divice_Status_CameraFine.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_Main_Divice_Status_CameraFine.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Main_Divice_Status_CameraFine.Location = new System.Drawing.Point(398, 96);
            this.baseLabel_Main_Divice_Status_CameraFine.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Main_Divice_Status_CameraFine.Name = "baseLabel_Main_Divice_Status_CameraFine";
            this.baseLabel_Main_Divice_Status_CameraFine.Size = new System.Drawing.Size(85, 18);
            this.baseLabel_Main_Divice_Status_CameraFine.TabIndex = 235;
            this.baseLabel_Main_Divice_Status_CameraFine.Text = "CameraFine";
            // 
            // pictureBox_Main_DiviceStatus_CameraFine
            // 
            this.pictureBox_Main_DiviceStatus_CameraFine.Image = global::SLD200.Properties.Resources.DioEllipseOff;
            this.pictureBox_Main_DiviceStatus_CameraFine.Location = new System.Drawing.Point(369, 93);
            this.pictureBox_Main_DiviceStatus_CameraFine.Name = "pictureBox_Main_DiviceStatus_CameraFine";
            this.pictureBox_Main_DiviceStatus_CameraFine.Size = new System.Drawing.Size(25, 25);
            this.pictureBox_Main_DiviceStatus_CameraFine.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Main_DiviceStatus_CameraFine.TabIndex = 236;
            this.pictureBox_Main_DiviceStatus_CameraFine.TabStop = false;
            // 
            // baseLabel_Main_Divice_Status_heightsensor
            // 
            this.baseLabel_Main_Divice_Status_heightsensor.AutoSize = true;
            this.baseLabel_Main_Divice_Status_heightsensor.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_Main_Divice_Status_heightsensor.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Main_Divice_Status_heightsensor.Location = new System.Drawing.Point(398, 65);
            this.baseLabel_Main_Divice_Status_heightsensor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Main_Divice_Status_heightsensor.Name = "baseLabel_Main_Divice_Status_heightsensor";
            this.baseLabel_Main_Divice_Status_heightsensor.Size = new System.Drawing.Size(93, 18);
            this.baseLabel_Main_Divice_Status_heightsensor.TabIndex = 233;
            this.baseLabel_Main_Divice_Status_heightsensor.Text = "HeightSensor";
            // 
            // pictureBox_Main_DiviceStatus_HeightSensor
            // 
            this.pictureBox_Main_DiviceStatus_HeightSensor.Image = global::SLD200.Properties.Resources.DioEllipseOff;
            this.pictureBox_Main_DiviceStatus_HeightSensor.Location = new System.Drawing.Point(369, 62);
            this.pictureBox_Main_DiviceStatus_HeightSensor.Name = "pictureBox_Main_DiviceStatus_HeightSensor";
            this.pictureBox_Main_DiviceStatus_HeightSensor.Size = new System.Drawing.Size(25, 25);
            this.pictureBox_Main_DiviceStatus_HeightSensor.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Main_DiviceStatus_HeightSensor.TabIndex = 234;
            this.pictureBox_Main_DiviceStatus_HeightSensor.TabStop = false;
            // 
            // baseLabel_Main_Divice_Status_ElectroRequlator
            // 
            this.baseLabel_Main_Divice_Status_ElectroRequlator.AutoSize = true;
            this.baseLabel_Main_Divice_Status_ElectroRequlator.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_Main_Divice_Status_ElectroRequlator.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Main_Divice_Status_ElectroRequlator.Location = new System.Drawing.Point(398, 34);
            this.baseLabel_Main_Divice_Status_ElectroRequlator.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Main_Divice_Status_ElectroRequlator.Name = "baseLabel_Main_Divice_Status_ElectroRequlator";
            this.baseLabel_Main_Divice_Status_ElectroRequlator.Size = new System.Drawing.Size(112, 18);
            this.baseLabel_Main_Divice_Status_ElectroRequlator.TabIndex = 231;
            this.baseLabel_Main_Divice_Status_ElectroRequlator.Text = "ElectroRegulator";
            // 
            // pictureBox_Main_DiviceStatus_ElectroRegulator
            // 
            this.pictureBox_Main_DiviceStatus_ElectroRegulator.Image = global::SLD200.Properties.Resources.DioEllipseOff;
            this.pictureBox_Main_DiviceStatus_ElectroRegulator.Location = new System.Drawing.Point(369, 31);
            this.pictureBox_Main_DiviceStatus_ElectroRegulator.Name = "pictureBox_Main_DiviceStatus_ElectroRegulator";
            this.pictureBox_Main_DiviceStatus_ElectroRegulator.Size = new System.Drawing.Size(25, 25);
            this.pictureBox_Main_DiviceStatus_ElectroRegulator.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Main_DiviceStatus_ElectroRegulator.TabIndex = 232;
            this.pictureBox_Main_DiviceStatus_ElectroRegulator.TabStop = false;
            // 
            // baseLabel_Main_Divice_Status_BeamExpander
            // 
            this.baseLabel_Main_Divice_Status_BeamExpander.AutoSize = true;
            this.baseLabel_Main_Divice_Status_BeamExpander.Enabled = false;
            this.baseLabel_Main_Divice_Status_BeamExpander.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_Main_Divice_Status_BeamExpander.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Main_Divice_Status_BeamExpander.Location = new System.Drawing.Point(184, 158);
            this.baseLabel_Main_Divice_Status_BeamExpander.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Main_Divice_Status_BeamExpander.Name = "baseLabel_Main_Divice_Status_BeamExpander";
            this.baseLabel_Main_Divice_Status_BeamExpander.Size = new System.Drawing.Size(107, 18);
            this.baseLabel_Main_Divice_Status_BeamExpander.TabIndex = 229;
            this.baseLabel_Main_Divice_Status_BeamExpander.Text = "BeamExpander";
            // 
            // pictureBox_Main_DiviceStatus_BeamExpander
            // 
            this.pictureBox_Main_DiviceStatus_BeamExpander.Enabled = false;
            this.pictureBox_Main_DiviceStatus_BeamExpander.Image = global::SLD200.Properties.Resources.DioEllipseOff;
            this.pictureBox_Main_DiviceStatus_BeamExpander.Location = new System.Drawing.Point(155, 155);
            this.pictureBox_Main_DiviceStatus_BeamExpander.Name = "pictureBox_Main_DiviceStatus_BeamExpander";
            this.pictureBox_Main_DiviceStatus_BeamExpander.Size = new System.Drawing.Size(25, 25);
            this.pictureBox_Main_DiviceStatus_BeamExpander.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Main_DiviceStatus_BeamExpander.TabIndex = 230;
            this.pictureBox_Main_DiviceStatus_BeamExpander.TabStop = false;
            // 
            // baseLabel_Main_Divice_Status_Chiller
            // 
            this.baseLabel_Main_Divice_Status_Chiller.AutoSize = true;
            this.baseLabel_Main_Divice_Status_Chiller.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_Main_Divice_Status_Chiller.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Main_Divice_Status_Chiller.Location = new System.Drawing.Point(43, 158);
            this.baseLabel_Main_Divice_Status_Chiller.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Main_Divice_Status_Chiller.Name = "baseLabel_Main_Divice_Status_Chiller";
            this.baseLabel_Main_Divice_Status_Chiller.Size = new System.Drawing.Size(44, 18);
            this.baseLabel_Main_Divice_Status_Chiller.TabIndex = 227;
            this.baseLabel_Main_Divice_Status_Chiller.Text = "Chiller";
            // 
            // pictureBox_Main_DiviceStatus_Chiller
            // 
            this.pictureBox_Main_DiviceStatus_Chiller.Image = global::SLD200.Properties.Resources.DioEllipseOff;
            this.pictureBox_Main_DiviceStatus_Chiller.Location = new System.Drawing.Point(14, 155);
            this.pictureBox_Main_DiviceStatus_Chiller.Name = "pictureBox_Main_DiviceStatus_Chiller";
            this.pictureBox_Main_DiviceStatus_Chiller.Size = new System.Drawing.Size(25, 25);
            this.pictureBox_Main_DiviceStatus_Chiller.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Main_DiviceStatus_Chiller.TabIndex = 228;
            this.pictureBox_Main_DiviceStatus_Chiller.TabStop = false;
            // 
            // baseLabel_Main_Divice_Status_Stage
            // 
            this.baseLabel_Main_Divice_Status_Stage.AutoSize = true;
            this.baseLabel_Main_Divice_Status_Stage.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_Main_Divice_Status_Stage.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Main_Divice_Status_Stage.Location = new System.Drawing.Point(184, 127);
            this.baseLabel_Main_Divice_Status_Stage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Main_Divice_Status_Stage.Name = "baseLabel_Main_Divice_Status_Stage";
            this.baseLabel_Main_Divice_Status_Stage.Size = new System.Drawing.Size(130, 18);
            this.baseLabel_Main_Divice_Status_Stage.TabIndex = 225;
            this.baseLabel_Main_Divice_Status_Stage.Text = "Powermeter_stage";
            // 
            // pictureBox_Main_DiviceStatus_Powermeter_Stage
            // 
            this.pictureBox_Main_DiviceStatus_Powermeter_Stage.Image = global::SLD200.Properties.Resources.DioEllipseOff;
            this.pictureBox_Main_DiviceStatus_Powermeter_Stage.Location = new System.Drawing.Point(155, 124);
            this.pictureBox_Main_DiviceStatus_Powermeter_Stage.Name = "pictureBox_Main_DiviceStatus_Powermeter_Stage";
            this.pictureBox_Main_DiviceStatus_Powermeter_Stage.Size = new System.Drawing.Size(25, 25);
            this.pictureBox_Main_DiviceStatus_Powermeter_Stage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Main_DiviceStatus_Powermeter_Stage.TabIndex = 226;
            this.pictureBox_Main_DiviceStatus_Powermeter_Stage.TabStop = false;
            // 
            // baseLabel_Main_Divice_Status_PowermeterBds
            // 
            this.baseLabel_Main_Divice_Status_PowermeterBds.AutoSize = true;
            this.baseLabel_Main_Divice_Status_PowermeterBds.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_Main_Divice_Status_PowermeterBds.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Main_Divice_Status_PowermeterBds.Location = new System.Drawing.Point(184, 96);
            this.baseLabel_Main_Divice_Status_PowermeterBds.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Main_Divice_Status_PowermeterBds.Name = "baseLabel_Main_Divice_Status_PowermeterBds";
            this.baseLabel_Main_Divice_Status_PowermeterBds.Size = new System.Drawing.Size(117, 18);
            this.baseLabel_Main_Divice_Status_PowermeterBds.TabIndex = 223;
            this.baseLabel_Main_Divice_Status_PowermeterBds.Text = "Powermeter_bds";
            // 
            // pictureBox_Main_DiviceStatus_Powermeter_bds
            // 
            this.pictureBox_Main_DiviceStatus_Powermeter_bds.Image = global::SLD200.Properties.Resources.DioEllipseOff;
            this.pictureBox_Main_DiviceStatus_Powermeter_bds.Location = new System.Drawing.Point(155, 93);
            this.pictureBox_Main_DiviceStatus_Powermeter_bds.Name = "pictureBox_Main_DiviceStatus_Powermeter_bds";
            this.pictureBox_Main_DiviceStatus_Powermeter_bds.Size = new System.Drawing.Size(25, 25);
            this.pictureBox_Main_DiviceStatus_Powermeter_bds.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Main_DiviceStatus_Powermeter_bds.TabIndex = 224;
            this.pictureBox_Main_DiviceStatus_Powermeter_bds.TabStop = false;
            // 
            // baseLabel_Main_Divice_Status_DustcollectorLower
            // 
            this.baseLabel_Main_Divice_Status_DustcollectorLower.AutoSize = true;
            this.baseLabel_Main_Divice_Status_DustcollectorLower.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_Main_Divice_Status_DustcollectorLower.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Main_Divice_Status_DustcollectorLower.Location = new System.Drawing.Point(184, 65);
            this.baseLabel_Main_Divice_Status_DustcollectorLower.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Main_Divice_Status_DustcollectorLower.Name = "baseLabel_Main_Divice_Status_DustcollectorLower";
            this.baseLabel_Main_Divice_Status_DustcollectorLower.Size = new System.Drawing.Size(128, 18);
            this.baseLabel_Main_Divice_Status_DustcollectorLower.TabIndex = 221;
            this.baseLabel_Main_Divice_Status_DustcollectorLower.Text = "DustcollectorLower";
            // 
            // pictureBox_Main_DiviceStatus_DustCollector_Lower
            // 
            this.pictureBox_Main_DiviceStatus_DustCollector_Lower.Image = global::SLD200.Properties.Resources.DioEllipseOff;
            this.pictureBox_Main_DiviceStatus_DustCollector_Lower.Location = new System.Drawing.Point(155, 62);
            this.pictureBox_Main_DiviceStatus_DustCollector_Lower.Name = "pictureBox_Main_DiviceStatus_DustCollector_Lower";
            this.pictureBox_Main_DiviceStatus_DustCollector_Lower.Size = new System.Drawing.Size(25, 25);
            this.pictureBox_Main_DiviceStatus_DustCollector_Lower.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Main_DiviceStatus_DustCollector_Lower.TabIndex = 222;
            this.pictureBox_Main_DiviceStatus_DustCollector_Lower.TabStop = false;
            // 
            // baseLabel_Main_Divice_Status_DustcollectorUpper
            // 
            this.baseLabel_Main_Divice_Status_DustcollectorUpper.AutoSize = true;
            this.baseLabel_Main_Divice_Status_DustcollectorUpper.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_Main_Divice_Status_DustcollectorUpper.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Main_Divice_Status_DustcollectorUpper.Location = new System.Drawing.Point(184, 34);
            this.baseLabel_Main_Divice_Status_DustcollectorUpper.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Main_Divice_Status_DustcollectorUpper.Name = "baseLabel_Main_Divice_Status_DustcollectorUpper";
            this.baseLabel_Main_Divice_Status_DustcollectorUpper.Size = new System.Drawing.Size(129, 18);
            this.baseLabel_Main_Divice_Status_DustcollectorUpper.TabIndex = 219;
            this.baseLabel_Main_Divice_Status_DustcollectorUpper.Text = "DustcollectorUpper";
            // 
            // pictureBox_Main_DiviceStatus_DustCollector_Upper
            // 
            this.pictureBox_Main_DiviceStatus_DustCollector_Upper.Image = global::SLD200.Properties.Resources.DioEllipseOff;
            this.pictureBox_Main_DiviceStatus_DustCollector_Upper.Location = new System.Drawing.Point(155, 31);
            this.pictureBox_Main_DiviceStatus_DustCollector_Upper.Name = "pictureBox_Main_DiviceStatus_DustCollector_Upper";
            this.pictureBox_Main_DiviceStatus_DustCollector_Upper.Size = new System.Drawing.Size(25, 25);
            this.pictureBox_Main_DiviceStatus_DustCollector_Upper.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Main_DiviceStatus_DustCollector_Upper.TabIndex = 220;
            this.pictureBox_Main_DiviceStatus_DustCollector_Upper.TabStop = false;
            // 
            // baseLabel_Main_Divice_Status_Scanner
            // 
            this.baseLabel_Main_Divice_Status_Scanner.AutoSize = true;
            this.baseLabel_Main_Divice_Status_Scanner.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_Main_Divice_Status_Scanner.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Main_Divice_Status_Scanner.Location = new System.Drawing.Point(43, 127);
            this.baseLabel_Main_Divice_Status_Scanner.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Main_Divice_Status_Scanner.Name = "baseLabel_Main_Divice_Status_Scanner";
            this.baseLabel_Main_Divice_Status_Scanner.Size = new System.Drawing.Size(60, 18);
            this.baseLabel_Main_Divice_Status_Scanner.TabIndex = 217;
            this.baseLabel_Main_Divice_Status_Scanner.Text = "Scanner";
            // 
            // pictureBox_Main_DiviceStatus_Scanner
            // 
            this.pictureBox_Main_DiviceStatus_Scanner.Image = global::SLD200.Properties.Resources.DioEllipseOff;
            this.pictureBox_Main_DiviceStatus_Scanner.Location = new System.Drawing.Point(14, 124);
            this.pictureBox_Main_DiviceStatus_Scanner.Name = "pictureBox_Main_DiviceStatus_Scanner";
            this.pictureBox_Main_DiviceStatus_Scanner.Size = new System.Drawing.Size(25, 25);
            this.pictureBox_Main_DiviceStatus_Scanner.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Main_DiviceStatus_Scanner.TabIndex = 218;
            this.pictureBox_Main_DiviceStatus_Scanner.TabStop = false;
            // 
            // baseLabel_Main_Divice_Status_Motion
            // 
            this.baseLabel_Main_Divice_Status_Motion.AutoSize = true;
            this.baseLabel_Main_Divice_Status_Motion.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_Main_Divice_Status_Motion.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Main_Divice_Status_Motion.Location = new System.Drawing.Point(43, 34);
            this.baseLabel_Main_Divice_Status_Motion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Main_Divice_Status_Motion.Name = "baseLabel_Main_Divice_Status_Motion";
            this.baseLabel_Main_Divice_Status_Motion.Size = new System.Drawing.Size(51, 18);
            this.baseLabel_Main_Divice_Status_Motion.TabIndex = 215;
            this.baseLabel_Main_Divice_Status_Motion.Text = "Motion";
            // 
            // pictureBox_Main_DiviceStatus_Motion
            // 
            this.pictureBox_Main_DiviceStatus_Motion.Image = global::SLD200.Properties.Resources.DioEllipseOff;
            this.pictureBox_Main_DiviceStatus_Motion.Location = new System.Drawing.Point(14, 31);
            this.pictureBox_Main_DiviceStatus_Motion.Name = "pictureBox_Main_DiviceStatus_Motion";
            this.pictureBox_Main_DiviceStatus_Motion.Size = new System.Drawing.Size(25, 25);
            this.pictureBox_Main_DiviceStatus_Motion.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Main_DiviceStatus_Motion.TabIndex = 216;
            this.pictureBox_Main_DiviceStatus_Motion.TabStop = false;
            // 
            // baseLabel_Main_Divice_Status_IO
            // 
            this.baseLabel_Main_Divice_Status_IO.AutoSize = true;
            this.baseLabel_Main_Divice_Status_IO.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_Main_Divice_Status_IO.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Main_Divice_Status_IO.Location = new System.Drawing.Point(43, 65);
            this.baseLabel_Main_Divice_Status_IO.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Main_Divice_Status_IO.Name = "baseLabel_Main_Divice_Status_IO";
            this.baseLabel_Main_Divice_Status_IO.Size = new System.Drawing.Size(25, 18);
            this.baseLabel_Main_Divice_Status_IO.TabIndex = 213;
            this.baseLabel_Main_Divice_Status_IO.Text = "IO";
            // 
            // pictureBox_Main_DiviceStatus_IO
            // 
            this.pictureBox_Main_DiviceStatus_IO.Image = global::SLD200.Properties.Resources.DioEllipseOff;
            this.pictureBox_Main_DiviceStatus_IO.Location = new System.Drawing.Point(14, 62);
            this.pictureBox_Main_DiviceStatus_IO.Name = "pictureBox_Main_DiviceStatus_IO";
            this.pictureBox_Main_DiviceStatus_IO.Size = new System.Drawing.Size(25, 25);
            this.pictureBox_Main_DiviceStatus_IO.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Main_DiviceStatus_IO.TabIndex = 214;
            this.pictureBox_Main_DiviceStatus_IO.TabStop = false;
            // 
            // baseLabel_Main_Divice_Status_Laser
            // 
            this.baseLabel_Main_Divice_Status_Laser.AutoSize = true;
            this.baseLabel_Main_Divice_Status_Laser.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_Main_Divice_Status_Laser.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Main_Divice_Status_Laser.Location = new System.Drawing.Point(43, 96);
            this.baseLabel_Main_Divice_Status_Laser.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Main_Divice_Status_Laser.Name = "baseLabel_Main_Divice_Status_Laser";
            this.baseLabel_Main_Divice_Status_Laser.Size = new System.Drawing.Size(43, 18);
            this.baseLabel_Main_Divice_Status_Laser.TabIndex = 130;
            this.baseLabel_Main_Divice_Status_Laser.Text = "Laser";
            // 
            // pictureBox_Main_DiviceStatus_Laser
            // 
            this.pictureBox_Main_DiviceStatus_Laser.Image = global::SLD200.Properties.Resources.DioEllipseOff;
            this.pictureBox_Main_DiviceStatus_Laser.Location = new System.Drawing.Point(14, 93);
            this.pictureBox_Main_DiviceStatus_Laser.Name = "pictureBox_Main_DiviceStatus_Laser";
            this.pictureBox_Main_DiviceStatus_Laser.Size = new System.Drawing.Size(25, 25);
            this.pictureBox_Main_DiviceStatus_Laser.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Main_DiviceStatus_Laser.TabIndex = 212;
            this.pictureBox_Main_DiviceStatus_Laser.TabStop = false;
            // 
            // label_Main_LaserStatus
            // 
            this.label_Main_LaserStatus.BackColor = System.Drawing.Color.Black;
            this.label_Main_LaserStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_Main_LaserStatus.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_Main_LaserStatus.ForeColor = System.Drawing.Color.Lime;
            this.label_Main_LaserStatus.Location = new System.Drawing.Point(1726, 298);
            this.label_Main_LaserStatus.Name = "label_Main_LaserStatus";
            this.label_Main_LaserStatus.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.label_Main_LaserStatus.Size = new System.Drawing.Size(178, 68);
            this.label_Main_LaserStatus.TabIndex = 208;
            this.label_Main_LaserStatus.Text = "레이저 상태";
            this.label_Main_LaserStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseTextBox_DryRun_ProcessingTime
            // 
            this.baseTextBox_DryRun_ProcessingTime.BackColor = System.Drawing.Color.White;
            this.baseTextBox_DryRun_ProcessingTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseTextBox_DryRun_ProcessingTime.Font = new System.Drawing.Font("Tahoma", 10F);
            this.baseTextBox_DryRun_ProcessingTime.ForeColor = System.Drawing.Color.Black;
            this.baseTextBox_DryRun_ProcessingTime.Location = new System.Drawing.Point(1876, 271);
            this.baseTextBox_DryRun_ProcessingTime.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseTextBox_DryRun_ProcessingTime.Name = "baseTextBox_DryRun_ProcessingTime";
            this.baseTextBox_DryRun_ProcessingTime.Size = new System.Drawing.Size(26, 24);
            this.baseTextBox_DryRun_ProcessingTime.TabIndex = 151;
            this.baseTextBox_DryRun_ProcessingTime.Text = "5";
            this.baseTextBox_DryRun_ProcessingTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseLabel5
            // 
            this.baseLabel5.Font = new System.Drawing.Font("Tahoma", 9F);
            this.baseLabel5.ForeColor = System.Drawing.Color.Black;
            this.baseLabel5.Location = new System.Drawing.Point(1689, 269);
            this.baseLabel5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel5.Name = "baseLabel5";
            this.baseLabel5.Size = new System.Drawing.Size(188, 26);
            this.baseLabel5.TabIndex = 150;
            this.baseLabel5.Text = "(Dry Run Processing Time (sec) :";
            this.baseLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkBox_Test_LaserDrillingCycle
            // 
            this.checkBox_Test_LaserDrillingCycle.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.checkBox_Test_LaserDrillingCycle.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.checkBox_Test_LaserDrillingCycle.Location = new System.Drawing.Point(1476, 498);
            this.checkBox_Test_LaserDrillingCycle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox_Test_LaserDrillingCycle.Name = "checkBox_Test_LaserDrillingCycle";
            this.checkBox_Test_LaserDrillingCycle.Size = new System.Drawing.Size(194, 52);
            this.checkBox_Test_LaserDrillingCycle.TabIndex = 209;
            this.checkBox_Test_LaserDrillingCycle.Text = "Laser Drilling Cycle Enable   (Manual)";
            this.checkBox_Test_LaserDrillingCycle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox_Test_LaserDrillingCycle.UseVisualStyleBackColor = false;
            this.checkBox_Test_LaserDrillingCycle.CheckedChanged += new System.EventHandler(this.checkBox_Test_LaserDrillingCycle_CheckedChanged);
            // 
            // button_Main_ManualStart
            // 
            this.button_Main_ManualStart.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_Main_ManualStart.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.button_Main_ManualStart.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_Main_ManualStart.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Main_ManualStart.Location = new System.Drawing.Point(1476, 564);
            this.button_Main_ManualStart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_Main_ManualStart.Name = "button_Main_ManualStart";
            this.button_Main_ManualStart.Size = new System.Drawing.Size(194, 55);
            this.button_Main_ManualStart.TabIndex = 210;
            this.button_Main_ManualStart.Text = "Laser Drilling Cycle Start    (manual)";
            this.button_Main_ManualStart.UseVisualStyleBackColor = false;
            this.button_Main_ManualStart.Click += new System.EventHandler(this.button_Main_ManualStart_Click);
            // 
            // FormNew_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1920, 877);
            this.ControlBox = false;
            this.Controls.Add(this.button_Main_ManualStart);
            this.Controls.Add(this.checkBox_Test_LaserDrillingCycle);
            this.Controls.Add(this.label_Main_LaserStatus);
            this.Controls.Add(this.groupBox_Main_DiviceStatus);
            this.Controls.Add(this.label_Title_Stacker_RPort);
            this.Controls.Add(this.label_Title_Stacker_LPort);
            this.Controls.Add(this.button_TestbyUser_LPort_Start);
            this.Controls.Add(this.groupBox_CoarseCam);
            this.Controls.Add(this.groupBox_FineCam);
            this.Controls.Add(this.button_TEST12);
            this.Controls.Add(this.button_Main_RecipeOpen);
            this.Controls.Add(this.checkBox_Main_AutoRun);
            this.Controls.Add(this.buttonForceMaterialOut);
            this.Controls.Add(this.checkBox_Main_SocketStop);
            this.Controls.Add(this.button_Main_CameraInit);
            this.Controls.Add(this.baseTextBox_DryRun_ProcessingTime);
            this.Controls.Add(this.baseLabel5);
            this.Controls.Add(this.checkBox_Main_Loader_RPort_Pause);
            this.Controls.Add(this.checkBox_Main_Loader_LPort_Pause);
            this.Controls.Add(this.checkBox_Test_DryRun);
            this.Controls.Add(this.button_Main_RtcInit);
            this.Controls.Add(this.button_Main_Home);
            this.Controls.Add(this.checkBox_Main_CycleStop);
            this.Controls.Add(this.button_Main_Reset);
            this.Controls.Add(this.button_Main_Stop);
            this.Controls.Add(this.button_Main_Pause);
            this.Controls.Add(this.button_Main_Start);
            this.Controls.Add(this.groupBox_ProcessingData);
            this.Controls.Add(this.groupBoxMain_MaterialInformation);
            this.Controls.Add(this.groupBoxMain_ProcessingStatus);
            this.Controls.Add(this.groupBoxMain_ModuleProcessingStatus);
            this.Font = new System.Drawing.Font("Tahoma", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormNew_Main";
            this.Text = "FormNew_Main";
            this.Shown += new System.EventHandler(this.FormNew_Main_Shown);
            this.groupBoxMain_ModuleProcessingStatus.ResumeLayout(false);
            this.groupBoxMain_ModuleProcessingStatus.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Socket_Before)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_ModuleProcessingStatus)).EndInit();
            this.groupBoxMain_ProcessingStatus.ResumeLayout(false);
            this.baseGroupBox_Progress.ResumeLayout(false);
            this.baseGroupBox_Progress.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Module_TargetCount)).EndInit();
            this.baseGroupBox_WorkingTime.ResumeLayout(false);
            this.baseGroupBox_WorkingTime.PerformLayout();
            this.groupBox18.ResumeLayout(false);
            this.groupBoxMain_MaterialInformation.ResumeLayout(false);
            this.groupBoxMain_MaterialInformation.PerformLayout();
            this.groupBox_ProcessingData.ResumeLayout(false);
            this.groupBox_FineCam.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ImageViewer_Main_highs)).EndInit();
            this.groupBox_CoarseCam.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ImageViewer_Main_Rows)).EndInit();
            this.groupBox_Main_DiviceStatus.ResumeLayout(false);
            this.groupBox_Main_DiviceStatus.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Main_DiviceStatus_Illuminator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Main_DiviceStatus_CameraPre)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Main_DiviceStatus_CameraFine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Main_DiviceStatus_HeightSensor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Main_DiviceStatus_ElectroRegulator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Main_DiviceStatus_BeamExpander)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Main_DiviceStatus_Chiller)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Main_DiviceStatus_Powermeter_Stage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Main_DiviceStatus_Powermeter_bds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Main_DiviceStatus_DustCollector_Lower)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Main_DiviceStatus_DustCollector_Upper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Main_DiviceStatus_Scanner)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Main_DiviceStatus_Motion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Main_DiviceStatus_IO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Main_DiviceStatus_Laser)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxMain_ModuleProcessingStatus;
        private System.Windows.Forms.GroupBox groupBoxMain_ProcessingStatus;
        private System.Windows.Forms.GroupBox groupBoxMain_MaterialInformation;
        private System.Windows.Forms.Button button_Main_Stop;
        private System.Windows.Forms.Button button_Main_Pause;
        private System.Windows.Forms.Button button_Main_Start;
        private System.Windows.Forms.GroupBox groupBox_ProcessingData;
        private System.Windows.Forms.Button button_Main_RecipeOpen;
        private WATGroupBox baseGroupBox_Progress;
        private System.Windows.Forms.NumericUpDown numericUpDown_Module_TargetCount;
        private BaseLabel baseLabel_ModuleCount_Target;
        private System.Windows.Forms.Button button_PNLCount_Clear;
        private BaseLabel baseLabel_PNLCount_NG;
        private BaseTextBox baseTextBox_Module_NGCount;
        private BaseTextBox baseTextBox_Module_TotalCount;
        private BaseLabel baseLabel_ModuleCount;
        private BaseLabel baseLabel_PNLCount_Total;
        private WATGroupBox baseGroupBox_WorkingTime;
        private System.Windows.Forms.Button button_AverageOneCycleTime_Clear;
        private BaseLabel baseLabel_Average_OneCycleTime;
        private BaseLabel baseLabel_AverageOneCycle_Time;
        private System.Windows.Forms.ProgressBar progressBar_TotalRemained_Time;
        private BaseLabel baseLabel_Total_RemainedTime;
        private BaseLabel baseLabel_TotalRunning_Time;
        private System.Windows.Forms.ProgressBar progressBar_OneCycle_Time;
        private BaseLabel baseLabel_CurrentOneCycle_TotalTime;
        private BaseLabel baseLabel_CurrentOneCycle_ElapsedTime;
        private BaseLabel baseLabel_CurrentOneCycle_Time;
        private BaseTextBox baseTextBox_SocketCountPerModule;
        private BaseLabel baseLabel_SocketPerModule;
        private BaseTextBox baseTextBox2;
        private BaseTextBox baseTextBox_TotalSocketCount;
        private BaseLabel baseLabel_SocketCount;
        public SpiralLab.Sirius.SiriusViewerForm SiriusViewer_Main;
        private System.Windows.Forms.CheckBox checkBox_Main_CycleStop;
        private System.Windows.Forms.Button button_Main_Reset;
        private System.Windows.Forms.Button button_Main_Home;
        private System.Windows.Forms.Button button_Main_RtcInit;
        private BaseTextBox baseTextBox_Socket_Index;
        private BaseLabel baseLabel1;
        private System.Windows.Forms.CheckBox checkBox_Test_DryRun;
        private System.Windows.Forms.CheckBox checkBox_Main_Loader_LPort_Pause;
        private System.Windows.Forms.CheckBox checkBox_Main_Loader_RPort_Pause;
        private BaseTextBox baseTextBox_DryRun_ProcessingTime;
        private BaseLabel baseLabel5;
        private System.Windows.Forms.Button button_Main_CameraInit;
        private System.Windows.Forms.CheckBox checkBox_Main_SocketStop;
        private System.Windows.Forms.GroupBox groupBox18;
        private System.Windows.Forms.ListView listView_Main_FiducialAlignData;
        private System.Windows.Forms.PictureBox pictureBox_ModuleProcessingStatus;
        private System.Windows.Forms.PictureBox pictureBox_Socket_Before;
        private BaseLabel baseLabel_SocketStatus_NG;
        private BaseLabel baseLabel_SocketStatus_OK;
        private BaseLabel baseLabel_SocketStatus_Processing;
        private BaseLabel baseLabel_SocketStatus_Ready;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonForceMaterialOut;
        private QMC.Common.Hmi.VisionImageViewer ImageViewer_Main_Rows;
        private QMC.Common.Hmi.VisionImageViewer ImageViewer_Main_highs;
        private System.Windows.Forms.GroupBox groupBox_FineCam;
        private System.Windows.Forms.GroupBox groupBox_CoarseCam;
        private System.Windows.Forms.CheckBox checkBox_Main_AutoRun;
        private System.Windows.Forms.Button button_TEST12;
        private System.Windows.Forms.Button button_TestbyUser_LPort_Start;
        private System.Windows.Forms.CheckBox checkBox_Main_AlignStartSocket_SelectMode;
        private System.Windows.Forms.Label label_Title_Stacker_LPort;
        private System.Windows.Forms.Label label_Title_Stacker_RPort;
        private System.Windows.Forms.GroupBox groupBox_Main_DiviceStatus;
        private BaseLabel baseLabel_Main_Divice_Status_Laser;
        private System.Windows.Forms.PictureBox pictureBox_Main_DiviceStatus_Laser;
        private BaseLabel baseLabel_Main_Divice_Status_Stage;
        private System.Windows.Forms.PictureBox pictureBox_Main_DiviceStatus_Powermeter_Stage;
        private BaseLabel baseLabel_Main_Divice_Status_PowermeterBds;
        private System.Windows.Forms.PictureBox pictureBox_Main_DiviceStatus_Powermeter_bds;
        private BaseLabel baseLabel_Main_Divice_Status_DustcollectorLower;
        private System.Windows.Forms.PictureBox pictureBox_Main_DiviceStatus_DustCollector_Lower;
        private BaseLabel baseLabel_Main_Divice_Status_DustcollectorUpper;
        private System.Windows.Forms.PictureBox pictureBox_Main_DiviceStatus_DustCollector_Upper;
        private BaseLabel baseLabel_Main_Divice_Status_Scanner;
        private System.Windows.Forms.PictureBox pictureBox_Main_DiviceStatus_Scanner;
        private BaseLabel baseLabel_Main_Divice_Status_Motion;
        private System.Windows.Forms.PictureBox pictureBox_Main_DiviceStatus_Motion;
        private BaseLabel baseLabel_Main_Divice_Status_IO;
        private System.Windows.Forms.PictureBox pictureBox_Main_DiviceStatus_IO;
        private BaseLabel baseLabel_Main_Divice_Status_BeamExpander;
        private System.Windows.Forms.PictureBox pictureBox_Main_DiviceStatus_BeamExpander;
        private BaseLabel baseLabel_Main_Divice_Status_Chiller;
        private System.Windows.Forms.PictureBox pictureBox_Main_DiviceStatus_Chiller;
        private BaseLabel baseLabel_Main_Divice_Status_Illuminator;
        private System.Windows.Forms.PictureBox pictureBox_Main_DiviceStatus_Illuminator;
        private BaseLabel baseLabel_Main_Divice_Status_CameraPre;
        private System.Windows.Forms.PictureBox pictureBox_Main_DiviceStatus_CameraPre;
        private BaseLabel baseLabel_Main_Divice_Status_CameraFine;
        private System.Windows.Forms.PictureBox pictureBox_Main_DiviceStatus_CameraFine;
        private BaseLabel baseLabel_Main_Divice_Status_heightsensor;
        private System.Windows.Forms.PictureBox pictureBox_Main_DiviceStatus_HeightSensor;
        private BaseLabel baseLabel_Main_Divice_Status_ElectroRequlator;
        private System.Windows.Forms.PictureBox pictureBox_Main_DiviceStatus_ElectroRegulator;
        private System.Windows.Forms.Label label_Main_LaserStatus;
        private System.Windows.Forms.CheckBox checkBox_Test_LaserDrillingCycle;
        private System.Windows.Forms.Button button_Main_ManualStart;
    }
}