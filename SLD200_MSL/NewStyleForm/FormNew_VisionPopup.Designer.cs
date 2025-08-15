using System.Drawing;
using System.Windows.Forms;

namespace SLD200_MSL
{
    partial class FormNew_VisionPopup
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
            this.panel4 = new System.Windows.Forms.Panel();
            this.button41 = new System.Windows.Forms.Button();
            this.button42 = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnTest = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textBox_VisionPopup_LaserHeightValue = new System.Windows.Forms.Label();
            this.button_VisionPopup_LaserHeightCheck_Start = new System.Windows.Forms.Button();
            this.button_CurrentLightValue_toAlignLightValue = new System.Windows.Forms.Button();
            this.button_CurrentZPos_toLaserFocus = new System.Windows.Forms.Button();
            this.button_CurrentZPos_toFineCamFocus = new System.Windows.Forms.Button();
            this.tabControl_MarkFindType = new System.Windows.Forms.TabControl();
            this.tabPage_MarkFind_PatternMatching = new System.Windows.Forms.TabPage();
            this.listBox_VisionPopup_PM_Result = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button34 = new System.Windows.Forms.Button();
            this.button_VisionPopup_Search = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.button_VisionPopup_PM_Grab = new System.Windows.Forms.Button();
            this.pictureBox_VisionPopup_PM_ImageDisplay = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.tabPage_MarkFind_CircleFind = new System.Windows.Forms.TabPage();
            this.comboBox_VisionPopup_FiducialColor = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox_VisionPopup_FiducialSize_Width = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.button_VisionPopup_FindMetalPowder_Search = new System.Windows.Forms.Button();
            this.listBox_FindCircle_Result = new System.Windows.Forms.ListBox();
            this.button_VisionPopup_FindCircle_GrabImage = new System.Windows.Forms.Button();
            this.pictureBox_ImageDisplay = new System.Windows.Forms.PictureBox();
            this.button_VisionPopup_FindCircle_LoadImage = new System.Windows.Forms.Button();
            this.button_VisionPopup_FindCircle_Search = new System.Windows.Forms.Button();
            this.tabPage_Socket_AlignTest = new System.Windows.Forms.TabPage();
            this.button_Test_MoveTo_CorrectedPos = new System.Windows.Forms.Button();
            this.button_Test_MoveTo_FiducialPos = new System.Windows.Forms.Button();
            this.comboBox_Config_VisionPopup_AlignTest_SelectedSocket_FiducialList = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_Config_VisionPopup_AlignTest_Socket_CenterY = new System.Windows.Forms.TextBox();
            this.textBox_Config_VisionPopup_AlignTest_Socket_CenterX = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button_Test_SocketAlign_Start = new System.Windows.Forms.Button();
            this.comboBox_Config_VisionPopup_AlignTest_SocketList = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage_ScannerCal = new System.Windows.Forms.TabPage();
            this.btnTrain = new System.Windows.Forms.Button();
            this.pictureBox_ImageDisplayScannerCal = new System.Windows.Forms.PictureBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button_Scanner_FineCam_OffsetChange = new System.Windows.Forms.Button();
            this.button_Scanner_FineCam_OffsetCheck = new System.Windows.Forms.Button();
            this.btnCamera_StartLive = new System.Windows.Forms.Button();
            this.btnCamera_Init = new System.Windows.Forms.Button();
            this.groupBox19 = new System.Windows.Forms.GroupBox();
            this.groupBox20 = new System.Windows.Forms.GroupBox();
            this.button_KeypadCall_VisionPopup_JogMove_StepSize = new System.Windows.Forms.Button();
            this.textBox_VisionPopup_JogMove_StepSize = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.radioButton_VisionPopup_JogMove_Step = new System.Windows.Forms.RadioButton();
            this.radioButton_VisionPopup_JogMove_Continuous = new System.Windows.Forms.RadioButton();
            this.groupBox22 = new System.Windows.Forms.GroupBox();
            this.radioButton_VisionPopup_Move_MoveMode_Coarse = new System.Windows.Forms.RadioButton();
            this.radioButton_VisionPopup_Move_MoveMode_Fine = new System.Windows.Forms.RadioButton();
            this.groupBox21 = new System.Windows.Forms.GroupBox();
            this.labelLED_VisionZ_NOT = new System.Windows.Forms.Label();
            this.labelLED_VisionZ_POT = new System.Windows.Forms.Label();
            this.button_VisionPopup_Z_Neg = new System.Windows.Forms.Button();
            this.button_VisionPopup_Z_Pos = new System.Windows.Forms.Button();
            this.groupBox58 = new System.Windows.Forms.GroupBox();
            this.labelLED_VisionX_NOT = new System.Windows.Forms.Label();
            this.labelLED_VisionX_POT = new System.Windows.Forms.Label();
            this.labelLED_VisionY_NOT = new System.Windows.Forms.Label();
            this.labelLED_VisionY_POT = new System.Windows.Forms.Label();
            this.button_VisionPopup_X_Neg = new System.Windows.Forms.Button();
            this.button_VisionPopup_X_Pos = new System.Windows.Forms.Button();
            this.button_VisionPopup_Y_Neg = new System.Windows.Forms.Button();
            this.button_VisionPopup_Y_Pos = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_VisionPopup_WorkStage_CurrentFineCamPos_To_LaserSensorPos = new System.Windows.Forms.Button();
            this.button_VisionPopup_WorkStage_CurrentLaserSensorPos_To_FineCamPos = new System.Windows.Forms.Button();
            this.button_VisionPopup_WorkStage_CurrentCoarseCamPos_To_FineCamPos = new System.Windows.Forms.Button();
            this.button_VisionPopup_WorkStage_CurrentFineCamPos_To_CoarseCamPos = new System.Windows.Forms.Button();
            this.button_VisionPopup_WorkStage_CurrentFineCamPos_To_ScannerPos = new System.Windows.Forms.Button();
            this.button_VisionPopup_WorkStage_CurrentScannerPos_To_FineCamPos = new System.Windows.Forms.Button();
            this.button_VisionPopup_WorkStage_StageCenter_To_ScannerCenter = new System.Windows.Forms.Button();
            this.radioButton_WorkStageMove_toCalibration = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButton_VisionPopup_WorkStageMove_Relative = new System.Windows.Forms.RadioButton();
            this.radioButton_VisionPopup_WorkStageMove_Absolute = new System.Windows.Forms.RadioButton();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.radioButton_WorkStageMove_toProcess = new System.Windows.Forms.RadioButton();
            this.radioButton_WorkStageMove_toLowMagCamera = new System.Windows.Forms.RadioButton();
            this.radioButton_WorkStageMove_toHighMagCamera = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioButton_VisionPopup_DisplayMode_Live = new System.Windows.Forms.RadioButton();
            this.radioButton_VisionPopup_DisplayMode_Capture = new System.Windows.Forms.RadioButton();
            this.groupBox87 = new System.Windows.Forms.GroupBox();
            this.textBox_IlluminationValue_Red = new System.Windows.Forms.TextBox();
            this.baseLabelMax_Red = new SLD200_MSL.BaseLabel();
            this.baseLabelMin_Red = new SLD200_MSL.BaseLabel();
            this.hScrollBarIlluminator_Red = new System.Windows.Forms.HScrollBar();
            this.baseLabel_Red = new SLD200_MSL.BaseLabel();
            this.baseLabel_IR = new SLD200_MSL.BaseLabel();
            this.baseLabelMax_IR = new SLD200_MSL.BaseLabel();
            this.baseLabelMin_IR = new SLD200_MSL.BaseLabel();
            this.hScrollBarIlluminator_IR = new System.Windows.Forms.HScrollBar();
            this.textBox_IlluminationValue_IR = new System.Windows.Forms.TextBox();
            this.groupBox83 = new System.Windows.Forms.GroupBox();
            this.radioButton_VisionPopup_CameraSelection_HighMag = new System.Windows.Forms.RadioButton();
            this.radioButton_VisionPopup_CameraSelection_LowMag = new System.Windows.Forms.RadioButton();
            this.groupBox78 = new System.Windows.Forms.GroupBox();
            this.checkBox7 = new System.Windows.Forms.CheckBox();
            this.checkBox9 = new System.Windows.Forms.CheckBox();
            this.checkBox10 = new System.Windows.Forms.CheckBox();
            this.checkBox11 = new System.Windows.Forms.CheckBox();
            this.checkBox12 = new System.Windows.Forms.CheckBox();
            this.groupBox36 = new System.Windows.Forms.GroupBox();
            this.button46 = new System.Windows.Forms.Button();
            this.button49 = new System.Windows.Forms.Button();
            this.textBox16 = new System.Windows.Forms.TextBox();
            this.label70 = new System.Windows.Forms.Label();
            this.textBox17 = new System.Windows.Forms.TextBox();
            this.label71 = new System.Windows.Forms.Label();
            this.m_visionImageViewer_LowRes = new QMC.Common.Hmi.VisionImageViewer();
            this.m_visionImageViewer_HighRes = new QMC.Common.Hmi.VisionImageViewer();
            this.button44 = new System.Windows.Forms.Button();
            this.button43 = new System.Windows.Forms.Button();
            this.button47 = new System.Windows.Forms.Button();
            this.button48 = new System.Windows.Forms.Button();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabControl_MarkFindType.SuspendLayout();
            this.tabPage_MarkFind_PatternMatching.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_VisionPopup_PM_ImageDisplay)).BeginInit();
            this.tabPage_MarkFind_CircleFind.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_ImageDisplay)).BeginInit();
            this.tabPage_Socket_AlignTest.SuspendLayout();
            this.tabPage_ScannerCal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_ImageDisplayScannerCal)).BeginInit();
            this.groupBox19.SuspendLayout();
            this.groupBox20.SuspendLayout();
            this.groupBox22.SuspendLayout();
            this.groupBox21.SuspendLayout();
            this.groupBox58.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox87.SuspendLayout();
            this.groupBox83.SuspendLayout();
            this.groupBox78.SuspendLayout();
            this.groupBox36.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_visionImageViewer_LowRes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_visionImageViewer_HighRes)).BeginInit();
            this.SuspendLayout();
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.button41);
            this.panel4.Controls.Add(this.button42);
            this.panel4.Controls.Add(this.panel3);
            this.panel4.Controls.Add(this.button44);
            this.panel4.Controls.Add(this.button43);
            this.panel4.Controls.Add(this.button47);
            this.panel4.Controls.Add(this.button48);
            this.panel4.Font = new System.Drawing.Font("Arial", 8.25F);
            this.panel4.Location = new System.Drawing.Point(10, 10);
            this.panel4.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1354, 710);
            this.panel4.TabIndex = 44;
            // 
            // button41
            // 
            this.button41.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button41.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.button41.Location = new System.Drawing.Point(1254, 5);
            this.button41.Margin = new System.Windows.Forms.Padding(6);
            this.button41.Name = "button41";
            this.button41.Size = new System.Drawing.Size(100, 45);
            this.button41.TabIndex = 34;
            this.button41.Text = "New";
            this.button41.UseVisualStyleBackColor = true;
            // 
            // button42
            // 
            this.button42.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button42.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.button42.Location = new System.Drawing.Point(1254, 61);
            this.button42.Margin = new System.Windows.Forms.Padding(6);
            this.button42.Name = "button42";
            this.button42.Size = new System.Drawing.Size(100, 45);
            this.button42.TabIndex = 33;
            this.button42.Text = "Open";
            this.button42.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.btnTest);
            this.panel3.Controls.Add(this.groupBox4);
            this.panel3.Controls.Add(this.button_CurrentLightValue_toAlignLightValue);
            this.panel3.Controls.Add(this.button_CurrentZPos_toLaserFocus);
            this.panel3.Controls.Add(this.button_CurrentZPos_toFineCamFocus);
            this.panel3.Controls.Add(this.tabControl_MarkFindType);
            this.panel3.Controls.Add(this.button_Scanner_FineCam_OffsetChange);
            this.panel3.Controls.Add(this.button_Scanner_FineCam_OffsetCheck);
            this.panel3.Controls.Add(this.btnCamera_StartLive);
            this.panel3.Controls.Add(this.btnCamera_Init);
            this.panel3.Controls.Add(this.groupBox19);
            this.panel3.Controls.Add(this.groupBox1);
            this.panel3.Controls.Add(this.groupBox3);
            this.panel3.Controls.Add(this.groupBox87);
            this.panel3.Controls.Add(this.groupBox83);
            this.panel3.Controls.Add(this.groupBox78);
            this.panel3.Controls.Add(this.groupBox36);
            this.panel3.Controls.Add(this.m_visionImageViewer_LowRes);
            this.panel3.Controls.Add(this.m_visionImageViewer_HighRes);
            this.panel3.Location = new System.Drawing.Point(6, 5);
            this.panel3.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1236, 699);
            this.panel3.TabIndex = 41;
            // 
            // btnTest
            // 
            this.btnTest.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnTest.Location = new System.Drawing.Point(975, 10);
            this.btnTest.Margin = new System.Windows.Forms.Padding(6);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(60, 64);
            this.btnTest.TabIndex = 139;
            this.btnTest.Text = "TEST";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.textBox_VisionPopup_LaserHeightValue);
            this.groupBox4.Controls.Add(this.button_VisionPopup_LaserHeightCheck_Start);
            this.groupBox4.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.groupBox4.Location = new System.Drawing.Point(784, 309);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.groupBox4.Size = new System.Drawing.Size(123, 93);
            this.groupBox4.TabIndex = 138;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = " Height Check ";
            // 
            // textBox_VisionPopup_LaserHeightValue
            // 
            this.textBox_VisionPopup_LaserHeightValue.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.textBox_VisionPopup_LaserHeightValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.textBox_VisionPopup_LaserHeightValue.Font = new System.Drawing.Font("Tahoma", 10F);
            this.textBox_VisionPopup_LaserHeightValue.ForeColor = System.Drawing.Color.Lime;
            this.textBox_VisionPopup_LaserHeightValue.Location = new System.Drawing.Point(10, 57);
            this.textBox_VisionPopup_LaserHeightValue.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.textBox_VisionPopup_LaserHeightValue.Name = "textBox_VisionPopup_LaserHeightValue";
            this.textBox_VisionPopup_LaserHeightValue.Size = new System.Drawing.Size(103, 27);
            this.textBox_VisionPopup_LaserHeightValue.TabIndex = 133;
            this.textBox_VisionPopup_LaserHeightValue.Text = "000.000";
            this.textBox_VisionPopup_LaserHeightValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_VisionPopup_LaserHeightCheck_Start
            // 
            this.button_VisionPopup_LaserHeightCheck_Start.BackColor = System.Drawing.Color.White;
            this.button_VisionPopup_LaserHeightCheck_Start.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_VisionPopup_LaserHeightCheck_Start.FlatAppearance.BorderSize = 2;
            this.button_VisionPopup_LaserHeightCheck_Start.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_VisionPopup_LaserHeightCheck_Start.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_VisionPopup_LaserHeightCheck_Start.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_VisionPopup_LaserHeightCheck_Start.ForeColor = System.Drawing.Color.Black;
            this.button_VisionPopup_LaserHeightCheck_Start.Location = new System.Drawing.Point(10, 25);
            this.button_VisionPopup_LaserHeightCheck_Start.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_VisionPopup_LaserHeightCheck_Start.Name = "button_VisionPopup_LaserHeightCheck_Start";
            this.button_VisionPopup_LaserHeightCheck_Start.Size = new System.Drawing.Size(103, 30);
            this.button_VisionPopup_LaserHeightCheck_Start.TabIndex = 132;
            this.button_VisionPopup_LaserHeightCheck_Start.Text = "Start";
            this.button_VisionPopup_LaserHeightCheck_Start.UseVisualStyleBackColor = false;
            this.button_VisionPopup_LaserHeightCheck_Start.Click += new System.EventHandler(this.button_VisionPopup_LaserHeightCheck_Start_Click);
            // 
            // button_CurrentLightValue_toAlignLightValue
            // 
            this.button_CurrentLightValue_toAlignLightValue.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_CurrentLightValue_toAlignLightValue.Location = new System.Drawing.Point(985, 160);
            this.button_CurrentLightValue_toAlignLightValue.Margin = new System.Windows.Forms.Padding(6);
            this.button_CurrentLightValue_toAlignLightValue.Name = "button_CurrentLightValue_toAlignLightValue";
            this.button_CurrentLightValue_toAlignLightValue.Size = new System.Drawing.Size(118, 64);
            this.button_CurrentLightValue_toAlignLightValue.TabIndex = 137;
            this.button_CurrentLightValue_toAlignLightValue.Text = "Current Light Value\r\nto Align Light Value";
            this.button_CurrentLightValue_toAlignLightValue.UseVisualStyleBackColor = true;
            this.button_CurrentLightValue_toAlignLightValue.Click += new System.EventHandler(this.button_CurrentLightValue_toAlignLightValue_Click);
            // 
            // button_CurrentZPos_toLaserFocus
            // 
            this.button_CurrentZPos_toLaserFocus.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_CurrentZPos_toLaserFocus.Location = new System.Drawing.Point(1108, 160);
            this.button_CurrentZPos_toLaserFocus.Margin = new System.Windows.Forms.Padding(6);
            this.button_CurrentZPos_toLaserFocus.Name = "button_CurrentZPos_toLaserFocus";
            this.button_CurrentZPos_toLaserFocus.Size = new System.Drawing.Size(118, 64);
            this.button_CurrentZPos_toLaserFocus.TabIndex = 136;
            this.button_CurrentZPos_toLaserFocus.Text = "Current Z Pos.\r\nto Laser focus";
            this.button_CurrentZPos_toLaserFocus.UseVisualStyleBackColor = true;
            this.button_CurrentZPos_toLaserFocus.Click += new System.EventHandler(this.button_CurrentZPos_toLaserFocus_Click);
            // 
            // button_CurrentZPos_toFineCamFocus
            // 
            this.button_CurrentZPos_toFineCamFocus.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_CurrentZPos_toFineCamFocus.Location = new System.Drawing.Point(985, 92);
            this.button_CurrentZPos_toFineCamFocus.Margin = new System.Windows.Forms.Padding(6);
            this.button_CurrentZPos_toFineCamFocus.Name = "button_CurrentZPos_toFineCamFocus";
            this.button_CurrentZPos_toFineCamFocus.Size = new System.Drawing.Size(241, 64);
            this.button_CurrentZPos_toFineCamFocus.TabIndex = 135;
            this.button_CurrentZPos_toFineCamFocus.Text = "Current Z Pos. to Fine Cam. ,\r\nLaser and Height Sensor focus";
            this.button_CurrentZPos_toFineCamFocus.UseVisualStyleBackColor = true;
            this.button_CurrentZPos_toFineCamFocus.Click += new System.EventHandler(this.button_CurrentZPos_toFineCamFocus_Click);
            // 
            // tabControl_MarkFindType
            // 
            this.tabControl_MarkFindType.Controls.Add(this.tabPage_MarkFind_PatternMatching);
            this.tabControl_MarkFindType.Controls.Add(this.tabPage_MarkFind_CircleFind);
            this.tabControl_MarkFindType.Controls.Add(this.tabPage_Socket_AlignTest);
            this.tabControl_MarkFindType.Controls.Add(this.tabPage_ScannerCal);
            this.tabControl_MarkFindType.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.tabControl_MarkFindType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tabControl_MarkFindType.ItemSize = new System.Drawing.Size(130, 21);
            this.tabControl_MarkFindType.Location = new System.Drawing.Point(513, 9);
            this.tabControl_MarkFindType.Name = "tabControl_MarkFindType";
            this.tabControl_MarkFindType.SelectedIndex = 0;
            this.tabControl_MarkFindType.Size = new System.Drawing.Size(457, 283);
            this.tabControl_MarkFindType.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl_MarkFindType.TabIndex = 134;
            // 
            // tabPage_MarkFind_PatternMatching
            // 
            this.tabPage_MarkFind_PatternMatching.Controls.Add(this.listBox_VisionPopup_PM_Result);
            this.tabPage_MarkFind_PatternMatching.Controls.Add(this.button1);
            this.tabPage_MarkFind_PatternMatching.Controls.Add(this.button34);
            this.tabPage_MarkFind_PatternMatching.Controls.Add(this.button_VisionPopup_Search);
            this.tabPage_MarkFind_PatternMatching.Controls.Add(this.groupBox5);
            this.tabPage_MarkFind_PatternMatching.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.tabPage_MarkFind_PatternMatching.Location = new System.Drawing.Point(4, 25);
            this.tabPage_MarkFind_PatternMatching.Name = "tabPage_MarkFind_PatternMatching";
            this.tabPage_MarkFind_PatternMatching.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_MarkFind_PatternMatching.Size = new System.Drawing.Size(449, 254);
            this.tabPage_MarkFind_PatternMatching.TabIndex = 0;
            this.tabPage_MarkFind_PatternMatching.Text = "Pattern Matching";
            this.tabPage_MarkFind_PatternMatching.UseVisualStyleBackColor = true;
            // 
            // listBox_VisionPopup_PM_Result
            // 
            this.listBox_VisionPopup_PM_Result.FormattingEnabled = true;
            this.listBox_VisionPopup_PM_Result.ItemHeight = 16;
            this.listBox_VisionPopup_PM_Result.Location = new System.Drawing.Point(310, 108);
            this.listBox_VisionPopup_PM_Result.Name = "listBox_VisionPopup_PM_Result";
            this.listBox_VisionPopup_PM_Result.Size = new System.Drawing.Size(127, 100);
            this.listBox_VisionPopup_PM_Result.TabIndex = 75;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button1.Location = new System.Drawing.Point(336, 59);
            this.button1.Margin = new System.Windows.Forms.Padding(6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 42);
            this.button1.TabIndex = 70;
            this.button1.Text = "Image Teaching";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button34
            // 
            this.button34.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button34.Location = new System.Drawing.Point(336, 13);
            this.button34.Margin = new System.Windows.Forms.Padding(6);
            this.button34.Name = "button34";
            this.button34.Size = new System.Drawing.Size(101, 42);
            this.button34.TabIndex = 68;
            this.button34.Text = "Image Teaching";
            this.button34.UseVisualStyleBackColor = true;
            this.button34.Click += new System.EventHandler(this.button34_Click);
            // 
            // button_VisionPopup_Search
            // 
            this.button_VisionPopup_Search.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_VisionPopup_Search.Location = new System.Drawing.Point(310, 203);
            this.button_VisionPopup_Search.Margin = new System.Windows.Forms.Padding(6);
            this.button_VisionPopup_Search.Name = "button_VisionPopup_Search";
            this.button_VisionPopup_Search.Size = new System.Drawing.Size(127, 42);
            this.button_VisionPopup_Search.TabIndex = 69;
            this.button_VisionPopup_Search.Text = "Search";
            this.button_VisionPopup_Search.UseVisualStyleBackColor = true;
            this.button_VisionPopup_Search.Click += new System.EventHandler(this.button_VisionPopup_Search_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.button_VisionPopup_PM_Grab);
            this.groupBox5.Controls.Add(this.pictureBox_VisionPopup_PM_ImageDisplay);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.richTextBox1);
            this.groupBox5.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.groupBox5.Location = new System.Drawing.Point(5, 5);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.groupBox5.Size = new System.Drawing.Size(296, 244);
            this.groupBox5.TabIndex = 67;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = " Teaching Image ";
            // 
            // button_VisionPopup_PM_Grab
            // 
            this.button_VisionPopup_PM_Grab.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_VisionPopup_PM_Grab.Location = new System.Drawing.Point(1, 191);
            this.button_VisionPopup_PM_Grab.Margin = new System.Windows.Forms.Padding(6);
            this.button_VisionPopup_PM_Grab.Name = "button_VisionPopup_PM_Grab";
            this.button_VisionPopup_PM_Grab.Size = new System.Drawing.Size(98, 42);
            this.button_VisionPopup_PM_Grab.TabIndex = 76;
            this.button_VisionPopup_PM_Grab.Text = "Grab";
            this.button_VisionPopup_PM_Grab.UseVisualStyleBackColor = true;
            this.button_VisionPopup_PM_Grab.Click += new System.EventHandler(this.button_VisionPopup_PM_Grab_Click);
            // 
            // pictureBox_VisionPopup_PM_ImageDisplay
            // 
            this.pictureBox_VisionPopup_PM_ImageDisplay.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_VisionPopup_PM_ImageDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_VisionPopup_PM_ImageDisplay.Location = new System.Drawing.Point(105, 54);
            this.pictureBox_VisionPopup_PM_ImageDisplay.Margin = new System.Windows.Forms.Padding(6);
            this.pictureBox_VisionPopup_PM_ImageDisplay.Name = "pictureBox_VisionPopup_PM_ImageDisplay";
            this.pictureBox_VisionPopup_PM_ImageDisplay.Size = new System.Drawing.Size(180, 180);
            this.pictureBox_VisionPopup_PM_ImageDisplay.TabIndex = 56;
            this.pictureBox_VisionPopup_PM_ImageDisplay.TabStop = false;
            this.pictureBox_VisionPopup_PM_ImageDisplay.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_VisionPopup_PM_ImageDisplay_paint);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label2.Location = new System.Drawing.Point(-3, 54);
            this.label2.Margin = new System.Windows.Forms.Padding(3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 27);
            this.label2.TabIndex = 45;
            this.label2.Text = "Teaching Image :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label1.Location = new System.Drawing.Point(-3, 25);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 27);
            this.label1.TabIndex = 44;
            this.label1.Text = "Image File Name :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(105, 23);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(6);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(180, 27);
            this.richTextBox1.TabIndex = 23;
            this.richTextBox1.Text = "";
            // 
            // tabPage_MarkFind_CircleFind
            // 
            this.tabPage_MarkFind_CircleFind.Controls.Add(this.comboBox_VisionPopup_FiducialColor);
            this.tabPage_MarkFind_CircleFind.Controls.Add(this.label8);
            this.tabPage_MarkFind_CircleFind.Controls.Add(this.textBox_VisionPopup_FiducialSize_Width);
            this.tabPage_MarkFind_CircleFind.Controls.Add(this.label7);
            this.tabPage_MarkFind_CircleFind.Controls.Add(this.button_VisionPopup_FindMetalPowder_Search);
            this.tabPage_MarkFind_CircleFind.Controls.Add(this.listBox_FindCircle_Result);
            this.tabPage_MarkFind_CircleFind.Controls.Add(this.button_VisionPopup_FindCircle_GrabImage);
            this.tabPage_MarkFind_CircleFind.Controls.Add(this.pictureBox_ImageDisplay);
            this.tabPage_MarkFind_CircleFind.Controls.Add(this.button_VisionPopup_FindCircle_LoadImage);
            this.tabPage_MarkFind_CircleFind.Controls.Add(this.button_VisionPopup_FindCircle_Search);
            this.tabPage_MarkFind_CircleFind.Location = new System.Drawing.Point(4, 25);
            this.tabPage_MarkFind_CircleFind.Name = "tabPage_MarkFind_CircleFind";
            this.tabPage_MarkFind_CircleFind.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_MarkFind_CircleFind.Size = new System.Drawing.Size(449, 254);
            this.tabPage_MarkFind_CircleFind.TabIndex = 1;
            this.tabPage_MarkFind_CircleFind.Text = "Find Circle";
            this.tabPage_MarkFind_CircleFind.UseVisualStyleBackColor = true;
            // 
            // comboBox_VisionPopup_FiducialColor
            // 
            this.comboBox_VisionPopup_FiducialColor.Font = new System.Drawing.Font("Tahoma", 10F);
            this.comboBox_VisionPopup_FiducialColor.FormattingEnabled = true;
            this.comboBox_VisionPopup_FiducialColor.Items.AddRange(new object[] {
            "Black",
            "White"});
            this.comboBox_VisionPopup_FiducialColor.Location = new System.Drawing.Point(372, 29);
            this.comboBox_VisionPopup_FiducialColor.Name = "comboBox_VisionPopup_FiducialColor";
            this.comboBox_VisionPopup_FiducialColor.Size = new System.Drawing.Size(64, 24);
            this.comboBox_VisionPopup_FiducialColor.TabIndex = 79;
            this.comboBox_VisionPopup_FiducialColor.Text = "Black";
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label8.Location = new System.Drawing.Point(251, 29);
            this.label8.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 24);
            this.label8.TabIndex = 78;
            this.label8.Text = "Radius";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox_VisionPopup_FiducialSize_Width
            // 
            this.textBox_VisionPopup_FiducialSize_Width.Font = new System.Drawing.Font("Tahoma", 10F);
            this.textBox_VisionPopup_FiducialSize_Width.Location = new System.Drawing.Point(304, 29);
            this.textBox_VisionPopup_FiducialSize_Width.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_VisionPopup_FiducialSize_Width.Name = "textBox_VisionPopup_FiducialSize_Width";
            this.textBox_VisionPopup_FiducialSize_Width.Size = new System.Drawing.Size(56, 24);
            this.textBox_VisionPopup_FiducialSize_Width.TabIndex = 77;
            this.textBox_VisionPopup_FiducialSize_Width.Text = "1.000";
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label7.Location = new System.Drawing.Point(247, 7);
            this.label7.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(189, 18);
            this.label7.TabIndex = 76;
            this.label7.Text = "Target Size (mm)      Color";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button_VisionPopup_FindMetalPowder_Search
            // 
            this.button_VisionPopup_FindMetalPowder_Search.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_VisionPopup_FindMetalPowder_Search.Location = new System.Drawing.Point(252, 102);
            this.button_VisionPopup_FindMetalPowder_Search.Margin = new System.Windows.Forms.Padding(6);
            this.button_VisionPopup_FindMetalPowder_Search.Name = "button_VisionPopup_FindMetalPowder_Search";
            this.button_VisionPopup_FindMetalPowder_Search.Size = new System.Drawing.Size(186, 39);
            this.button_VisionPopup_FindMetalPowder_Search.TabIndex = 75;
            this.button_VisionPopup_FindMetalPowder_Search.Text = "Search  [Metal Powder]";
            this.button_VisionPopup_FindMetalPowder_Search.UseVisualStyleBackColor = true;
            this.button_VisionPopup_FindMetalPowder_Search.Click += new System.EventHandler(this.button_VisionPopup_FindMetalPowder_Search_Click);
            // 
            // listBox_FindCircle_Result
            // 
            this.listBox_FindCircle_Result.FormattingEnabled = true;
            this.listBox_FindCircle_Result.ItemHeight = 16;
            this.listBox_FindCircle_Result.Location = new System.Drawing.Point(252, 147);
            this.listBox_FindCircle_Result.Name = "listBox_FindCircle_Result";
            this.listBox_FindCircle_Result.Size = new System.Drawing.Size(186, 100);
            this.listBox_FindCircle_Result.TabIndex = 74;
            // 
            // button_VisionPopup_FindCircle_GrabImage
            // 
            this.button_VisionPopup_FindCircle_GrabImage.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_VisionPopup_FindCircle_GrabImage.Location = new System.Drawing.Point(10, 10);
            this.button_VisionPopup_FindCircle_GrabImage.Margin = new System.Windows.Forms.Padding(6);
            this.button_VisionPopup_FindCircle_GrabImage.Name = "button_VisionPopup_FindCircle_GrabImage";
            this.button_VisionPopup_FindCircle_GrabImage.Size = new System.Drawing.Size(107, 42);
            this.button_VisionPopup_FindCircle_GrabImage.TabIndex = 73;
            this.button_VisionPopup_FindCircle_GrabImage.Text = "Grab Image";
            this.button_VisionPopup_FindCircle_GrabImage.UseVisualStyleBackColor = true;
            this.button_VisionPopup_FindCircle_GrabImage.Click += new System.EventHandler(this.button_VisionPopup_FindCircle_GrabImage_Click);
            // 
            // pictureBox_ImageDisplay
            // 
            this.pictureBox_ImageDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_ImageDisplay.Location = new System.Drawing.Point(10, 61);
            this.pictureBox_ImageDisplay.Name = "pictureBox_ImageDisplay";
            this.pictureBox_ImageDisplay.Size = new System.Drawing.Size(223, 187);
            this.pictureBox_ImageDisplay.TabIndex = 72;
            this.pictureBox_ImageDisplay.TabStop = false;
            this.pictureBox_ImageDisplay.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_ImageDisplay_Paint);
            // 
            // button_VisionPopup_FindCircle_LoadImage
            // 
            this.button_VisionPopup_FindCircle_LoadImage.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_VisionPopup_FindCircle_LoadImage.Location = new System.Drawing.Point(126, 10);
            this.button_VisionPopup_FindCircle_LoadImage.Margin = new System.Windows.Forms.Padding(6);
            this.button_VisionPopup_FindCircle_LoadImage.Name = "button_VisionPopup_FindCircle_LoadImage";
            this.button_VisionPopup_FindCircle_LoadImage.Size = new System.Drawing.Size(107, 42);
            this.button_VisionPopup_FindCircle_LoadImage.TabIndex = 71;
            this.button_VisionPopup_FindCircle_LoadImage.Text = "Load Image";
            this.button_VisionPopup_FindCircle_LoadImage.UseVisualStyleBackColor = true;
            this.button_VisionPopup_FindCircle_LoadImage.Click += new System.EventHandler(this.button_VisionPopup_FindCircle_LoadImage_Click);
            // 
            // button_VisionPopup_FindCircle_Search
            // 
            this.button_VisionPopup_FindCircle_Search.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_VisionPopup_FindCircle_Search.Location = new System.Drawing.Point(252, 61);
            this.button_VisionPopup_FindCircle_Search.Margin = new System.Windows.Forms.Padding(6);
            this.button_VisionPopup_FindCircle_Search.Name = "button_VisionPopup_FindCircle_Search";
            this.button_VisionPopup_FindCircle_Search.Size = new System.Drawing.Size(186, 39);
            this.button_VisionPopup_FindCircle_Search.TabIndex = 70;
            this.button_VisionPopup_FindCircle_Search.Text = "Search      [Fiducial]";
            this.button_VisionPopup_FindCircle_Search.UseVisualStyleBackColor = true;
            this.button_VisionPopup_FindCircle_Search.Click += new System.EventHandler(this.button_VisionPopup_FindCircle_Search_Click);
            // 
            // tabPage_Socket_AlignTest
            // 
            this.tabPage_Socket_AlignTest.Controls.Add(this.button_Test_MoveTo_CorrectedPos);
            this.tabPage_Socket_AlignTest.Controls.Add(this.button_Test_MoveTo_FiducialPos);
            this.tabPage_Socket_AlignTest.Controls.Add(this.comboBox_Config_VisionPopup_AlignTest_SelectedSocket_FiducialList);
            this.tabPage_Socket_AlignTest.Controls.Add(this.label6);
            this.tabPage_Socket_AlignTest.Controls.Add(this.textBox_Config_VisionPopup_AlignTest_Socket_CenterY);
            this.tabPage_Socket_AlignTest.Controls.Add(this.textBox_Config_VisionPopup_AlignTest_Socket_CenterX);
            this.tabPage_Socket_AlignTest.Controls.Add(this.label5);
            this.tabPage_Socket_AlignTest.Controls.Add(this.label4);
            this.tabPage_Socket_AlignTest.Controls.Add(this.button_Test_SocketAlign_Start);
            this.tabPage_Socket_AlignTest.Controls.Add(this.comboBox_Config_VisionPopup_AlignTest_SocketList);
            this.tabPage_Socket_AlignTest.Controls.Add(this.label3);
            this.tabPage_Socket_AlignTest.Location = new System.Drawing.Point(4, 25);
            this.tabPage_Socket_AlignTest.Name = "tabPage_Socket_AlignTest";
            this.tabPage_Socket_AlignTest.Size = new System.Drawing.Size(449, 254);
            this.tabPage_Socket_AlignTest.TabIndex = 2;
            this.tabPage_Socket_AlignTest.Text = "Align Test";
            this.tabPage_Socket_AlignTest.UseVisualStyleBackColor = true;
            // 
            // button_Test_MoveTo_CorrectedPos
            // 
            this.button_Test_MoveTo_CorrectedPos.BackColor = System.Drawing.Color.White;
            this.button_Test_MoveTo_CorrectedPos.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_Test_MoveTo_CorrectedPos.FlatAppearance.BorderSize = 2;
            this.button_Test_MoveTo_CorrectedPos.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_Test_MoveTo_CorrectedPos.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_Test_MoveTo_CorrectedPos.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_Test_MoveTo_CorrectedPos.ForeColor = System.Drawing.Color.Black;
            this.button_Test_MoveTo_CorrectedPos.Location = new System.Drawing.Point(223, 138);
            this.button_Test_MoveTo_CorrectedPos.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_Test_MoveTo_CorrectedPos.Name = "button_Test_MoveTo_CorrectedPos";
            this.button_Test_MoveTo_CorrectedPos.Size = new System.Drawing.Size(124, 66);
            this.button_Test_MoveTo_CorrectedPos.TabIndex = 139;
            this.button_Test_MoveTo_CorrectedPos.Text = "Move to\r\nCorrected Pos.\r\n(after Align)";
            this.button_Test_MoveTo_CorrectedPos.UseVisualStyleBackColor = false;
            this.button_Test_MoveTo_CorrectedPos.Click += new System.EventHandler(this.button_Test_MoveTo_CorrectedPos_Click);
            // 
            // button_Test_MoveTo_FiducialPos
            // 
            this.button_Test_MoveTo_FiducialPos.BackColor = System.Drawing.Color.White;
            this.button_Test_MoveTo_FiducialPos.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_Test_MoveTo_FiducialPos.FlatAppearance.BorderSize = 2;
            this.button_Test_MoveTo_FiducialPos.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_Test_MoveTo_FiducialPos.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_Test_MoveTo_FiducialPos.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_Test_MoveTo_FiducialPos.ForeColor = System.Drawing.Color.Black;
            this.button_Test_MoveTo_FiducialPos.Location = new System.Drawing.Point(97, 138);
            this.button_Test_MoveTo_FiducialPos.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_Test_MoveTo_FiducialPos.Name = "button_Test_MoveTo_FiducialPos";
            this.button_Test_MoveTo_FiducialPos.Size = new System.Drawing.Size(118, 66);
            this.button_Test_MoveTo_FiducialPos.TabIndex = 138;
            this.button_Test_MoveTo_FiducialPos.Text = "Move to\r\nFiducial Pos.\r\n(before Align)";
            this.button_Test_MoveTo_FiducialPos.UseVisualStyleBackColor = false;
            this.button_Test_MoveTo_FiducialPos.Click += new System.EventHandler(this.button_Test_MoveTo_FiducialPos_Click);
            // 
            // comboBox_Config_VisionPopup_AlignTest_SelectedSocket_FiducialList
            // 
            this.comboBox_Config_VisionPopup_AlignTest_SelectedSocket_FiducialList.Font = new System.Drawing.Font("Tahoma", 10F);
            this.comboBox_Config_VisionPopup_AlignTest_SelectedSocket_FiducialList.FormattingEnabled = true;
            this.comboBox_Config_VisionPopup_AlignTest_SelectedSocket_FiducialList.Location = new System.Drawing.Point(10, 156);
            this.comboBox_Config_VisionPopup_AlignTest_SelectedSocket_FiducialList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBox_Config_VisionPopup_AlignTest_SelectedSocket_FiducialList.Name = "comboBox_Config_VisionPopup_AlignTest_SelectedSocket_FiducialList";
            this.comboBox_Config_VisionPopup_AlignTest_SelectedSocket_FiducialList.Size = new System.Drawing.Size(80, 24);
            this.comboBox_Config_VisionPopup_AlignTest_SelectedSocket_FiducialList.TabIndex = 137;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label6.Location = new System.Drawing.Point(7, 135);
            this.label6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 19);
            this.label6.TabIndex = 136;
            this.label6.Text = "Fiducial No. :";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox_Config_VisionPopup_AlignTest_Socket_CenterY
            // 
            this.textBox_Config_VisionPopup_AlignTest_Socket_CenterY.Font = new System.Drawing.Font("Tahoma", 10F);
            this.textBox_Config_VisionPopup_AlignTest_Socket_CenterY.Location = new System.Drawing.Point(128, 71);
            this.textBox_Config_VisionPopup_AlignTest_Socket_CenterY.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_Config_VisionPopup_AlignTest_Socket_CenterY.Name = "textBox_Config_VisionPopup_AlignTest_Socket_CenterY";
            this.textBox_Config_VisionPopup_AlignTest_Socket_CenterY.Size = new System.Drawing.Size(72, 24);
            this.textBox_Config_VisionPopup_AlignTest_Socket_CenterY.TabIndex = 135;
            this.textBox_Config_VisionPopup_AlignTest_Socket_CenterY.Text = "0.000";
            // 
            // textBox_Config_VisionPopup_AlignTest_Socket_CenterX
            // 
            this.textBox_Config_VisionPopup_AlignTest_Socket_CenterX.Font = new System.Drawing.Font("Tahoma", 10F);
            this.textBox_Config_VisionPopup_AlignTest_Socket_CenterX.Location = new System.Drawing.Point(128, 43);
            this.textBox_Config_VisionPopup_AlignTest_Socket_CenterX.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_Config_VisionPopup_AlignTest_Socket_CenterX.Name = "textBox_Config_VisionPopup_AlignTest_Socket_CenterX";
            this.textBox_Config_VisionPopup_AlignTest_Socket_CenterX.Size = new System.Drawing.Size(72, 24);
            this.textBox_Config_VisionPopup_AlignTest_Socket_CenterX.TabIndex = 134;
            this.textBox_Config_VisionPopup_AlignTest_Socket_CenterX.Text = "0.000";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label5.Location = new System.Drawing.Point(7, 70);
            this.label5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(123, 24);
            this.label5.TabIndex = 133;
            this.label5.Text = "Socket Center Y :";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label4.Location = new System.Drawing.Point(7, 42);
            this.label4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 24);
            this.label4.TabIndex = 132;
            this.label4.Text = "Socket Center X :";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_Test_SocketAlign_Start
            // 
            this.button_Test_SocketAlign_Start.BackColor = System.Drawing.Color.White;
            this.button_Test_SocketAlign_Start.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_Test_SocketAlign_Start.FlatAppearance.BorderSize = 2;
            this.button_Test_SocketAlign_Start.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_Test_SocketAlign_Start.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_Test_SocketAlign_Start.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_Test_SocketAlign_Start.ForeColor = System.Drawing.Color.Black;
            this.button_Test_SocketAlign_Start.Location = new System.Drawing.Point(330, 8);
            this.button_Test_SocketAlign_Start.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_Test_SocketAlign_Start.Name = "button_Test_SocketAlign_Start";
            this.button_Test_SocketAlign_Start.Size = new System.Drawing.Size(108, 54);
            this.button_Test_SocketAlign_Start.TabIndex = 131;
            this.button_Test_SocketAlign_Start.Text = "Socket Align\r\nStart";
            this.button_Test_SocketAlign_Start.UseVisualStyleBackColor = false;
            this.button_Test_SocketAlign_Start.Click += new System.EventHandler(this.button_Test_SocketAlign_Start_Click);
            // 
            // comboBox_Config_VisionPopup_AlignTest_SocketList
            // 
            this.comboBox_Config_VisionPopup_AlignTest_SocketList.Font = new System.Drawing.Font("Tahoma", 10F);
            this.comboBox_Config_VisionPopup_AlignTest_SocketList.FormattingEnabled = true;
            this.comboBox_Config_VisionPopup_AlignTest_SocketList.Location = new System.Drawing.Point(105, 8);
            this.comboBox_Config_VisionPopup_AlignTest_SocketList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBox_Config_VisionPopup_AlignTest_SocketList.Name = "comboBox_Config_VisionPopup_AlignTest_SocketList";
            this.comboBox_Config_VisionPopup_AlignTest_SocketList.Size = new System.Drawing.Size(95, 24);
            this.comboBox_Config_VisionPopup_AlignTest_SocketList.TabIndex = 118;
            this.comboBox_Config_VisionPopup_AlignTest_SocketList.SelectedIndexChanged += new System.EventHandler(this.comboBox_Config_VisionPopup_AlignTest_SocketList_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label3.Location = new System.Drawing.Point(7, 6);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 24);
            this.label3.TabIndex = 75;
            this.label3.Text = "Socket No. :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabPage_ScannerCal
            // 
            this.tabPage_ScannerCal.Controls.Add(this.btnTrain);
            this.tabPage_ScannerCal.Controls.Add(this.pictureBox_ImageDisplayScannerCal);
            this.tabPage_ScannerCal.Controls.Add(this.button2);
            this.tabPage_ScannerCal.Location = new System.Drawing.Point(4, 25);
            this.tabPage_ScannerCal.Name = "tabPage_ScannerCal";
            this.tabPage_ScannerCal.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_ScannerCal.Size = new System.Drawing.Size(449, 254);
            this.tabPage_ScannerCal.TabIndex = 3;
            this.tabPage_ScannerCal.Text = "Scanner Cal.";
            this.tabPage_ScannerCal.UseVisualStyleBackColor = true;
            // 
            // btnTrain
            // 
            this.btnTrain.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnTrain.Location = new System.Drawing.Point(10, 8);
            this.btnTrain.Margin = new System.Windows.Forms.Padding(6);
            this.btnTrain.Name = "btnTrain";
            this.btnTrain.Size = new System.Drawing.Size(107, 42);
            this.btnTrain.TabIndex = 76;
            this.btnTrain.Text = "Train";
            this.btnTrain.UseVisualStyleBackColor = true;
            this.btnTrain.Click += new System.EventHandler(this.btnTrain_Click);
            // 
            // pictureBox_ImageDisplayScannerCal
            // 
            this.pictureBox_ImageDisplayScannerCal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_ImageDisplayScannerCal.Location = new System.Drawing.Point(10, 59);
            this.pictureBox_ImageDisplayScannerCal.Name = "pictureBox_ImageDisplayScannerCal";
            this.pictureBox_ImageDisplayScannerCal.Size = new System.Drawing.Size(223, 187);
            this.pictureBox_ImageDisplayScannerCal.TabIndex = 75;
            this.pictureBox_ImageDisplayScannerCal.TabStop = false;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button2.Location = new System.Drawing.Point(252, 8);
            this.button2.Margin = new System.Windows.Forms.Padding(6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(186, 42);
            this.button2.TabIndex = 74;
            this.button2.Text = "Search      [Fiducial]";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button_Scanner_FineCam_OffsetChange
            // 
            this.button_Scanner_FineCam_OffsetChange.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_Scanner_FineCam_OffsetChange.Location = new System.Drawing.Point(1108, 228);
            this.button_Scanner_FineCam_OffsetChange.Margin = new System.Windows.Forms.Padding(6);
            this.button_Scanner_FineCam_OffsetChange.Name = "button_Scanner_FineCam_OffsetChange";
            this.button_Scanner_FineCam_OffsetChange.Size = new System.Drawing.Size(118, 64);
            this.button_Scanner_FineCam_OffsetChange.TabIndex = 133;
            this.button_Scanner_FineCam_OffsetChange.Text = "Scanner  ↔ \r\nFine Cam.\r\nOffset Change";
            this.button_Scanner_FineCam_OffsetChange.UseVisualStyleBackColor = true;
            this.button_Scanner_FineCam_OffsetChange.Click += new System.EventHandler(this.button_Scanner_FineCam_OffsetChange_Click);
            // 
            // button_Scanner_FineCam_OffsetCheck
            // 
            this.button_Scanner_FineCam_OffsetCheck.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_Scanner_FineCam_OffsetCheck.Location = new System.Drawing.Point(985, 228);
            this.button_Scanner_FineCam_OffsetCheck.Margin = new System.Windows.Forms.Padding(6);
            this.button_Scanner_FineCam_OffsetCheck.Name = "button_Scanner_FineCam_OffsetCheck";
            this.button_Scanner_FineCam_OffsetCheck.Size = new System.Drawing.Size(118, 64);
            this.button_Scanner_FineCam_OffsetCheck.TabIndex = 132;
            this.button_Scanner_FineCam_OffsetCheck.Text = "Scanner  ↔ \r\nFine Cam.\r\nOffset Check";
            this.button_Scanner_FineCam_OffsetCheck.UseVisualStyleBackColor = true;
            this.button_Scanner_FineCam_OffsetCheck.Click += new System.EventHandler(this.button_Scanner_FineCam_OffsetCheck_Click);
            // 
            // btnCamera_StartLive
            // 
            this.btnCamera_StartLive.BackColor = System.Drawing.Color.White;
            this.btnCamera_StartLive.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.btnCamera_StartLive.FlatAppearance.BorderSize = 2;
            this.btnCamera_StartLive.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnCamera_StartLive.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnCamera_StartLive.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnCamera_StartLive.ForeColor = System.Drawing.Color.Black;
            this.btnCamera_StartLive.Location = new System.Drawing.Point(1146, 9);
            this.btnCamera_StartLive.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnCamera_StartLive.Name = "btnCamera_StartLive";
            this.btnCamera_StartLive.Size = new System.Drawing.Size(80, 54);
            this.btnCamera_StartLive.TabIndex = 131;
            this.btnCamera_StartLive.Text = "Live\r\nStart";
            this.btnCamera_StartLive.UseVisualStyleBackColor = false;
            this.btnCamera_StartLive.Click += new System.EventHandler(this.btnCamera_StartLive_Click);
            // 
            // btnCamera_Init
            // 
            this.btnCamera_Init.BackColor = System.Drawing.Color.White;
            this.btnCamera_Init.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.btnCamera_Init.FlatAppearance.BorderSize = 2;
            this.btnCamera_Init.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnCamera_Init.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnCamera_Init.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.btnCamera_Init.ForeColor = System.Drawing.Color.Black;
            this.btnCamera_Init.Location = new System.Drawing.Point(1055, 9);
            this.btnCamera_Init.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnCamera_Init.Name = "btnCamera_Init";
            this.btnCamera_Init.Size = new System.Drawing.Size(80, 54);
            this.btnCamera_Init.TabIndex = 130;
            this.btnCamera_Init.Text = "Camera\r\nInitialize";
            this.btnCamera_Init.UseVisualStyleBackColor = false;
            this.btnCamera_Init.Click += new System.EventHandler(this.btnCamera_Init_Click);
            // 
            // groupBox19
            // 
            this.groupBox19.Controls.Add(this.groupBox20);
            this.groupBox19.Controls.Add(this.groupBox22);
            this.groupBox19.Controls.Add(this.groupBox21);
            this.groupBox19.Controls.Add(this.groupBox58);
            this.groupBox19.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.groupBox19.Location = new System.Drawing.Point(930, 309);
            this.groupBox19.Margin = new System.Windows.Forms.Padding(5);
            this.groupBox19.Name = "groupBox19";
            this.groupBox19.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox19.Size = new System.Drawing.Size(294, 327);
            this.groupBox19.TabIndex = 110;
            this.groupBox19.TabStop = false;
            this.groupBox19.Text = " Jog Move ";
            // 
            // groupBox20
            // 
            this.groupBox20.Controls.Add(this.button_KeypadCall_VisionPopup_JogMove_StepSize);
            this.groupBox20.Controls.Add(this.textBox_VisionPopup_JogMove_StepSize);
            this.groupBox20.Controls.Add(this.label10);
            this.groupBox20.Controls.Add(this.radioButton_VisionPopup_JogMove_Step);
            this.groupBox20.Controls.Add(this.radioButton_VisionPopup_JogMove_Continuous);
            this.groupBox20.Font = new System.Drawing.Font("Tahoma", 10F);
            this.groupBox20.Location = new System.Drawing.Point(10, 83);
            this.groupBox20.Margin = new System.Windows.Forms.Padding(5, 2, 5, 0);
            this.groupBox20.Name = "groupBox20";
            this.groupBox20.Padding = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.groupBox20.Size = new System.Drawing.Size(275, 78);
            this.groupBox20.TabIndex = 75;
            this.groupBox20.TabStop = false;
            this.groupBox20.Text = " Move Mode ";
            // 
            // button_KeypadCall_VisionPopup_JogMove_StepSize
            // 
            this.button_KeypadCall_VisionPopup_JogMove_StepSize.Font = new System.Drawing.Font("Tahoma", 10F);
            this.button_KeypadCall_VisionPopup_JogMove_StepSize.Location = new System.Drawing.Point(233, 45);
            this.button_KeypadCall_VisionPopup_JogMove_StepSize.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.button_KeypadCall_VisionPopup_JogMove_StepSize.Name = "button_KeypadCall_VisionPopup_JogMove_StepSize";
            this.button_KeypadCall_VisionPopup_JogMove_StepSize.Size = new System.Drawing.Size(35, 25);
            this.button_KeypadCall_VisionPopup_JogMove_StepSize.TabIndex = 76;
            this.button_KeypadCall_VisionPopup_JogMove_StepSize.Text = "#";
            this.button_KeypadCall_VisionPopup_JogMove_StepSize.UseVisualStyleBackColor = true;
            this.button_KeypadCall_VisionPopup_JogMove_StepSize.Visible = false;
            // 
            // textBox_VisionPopup_JogMove_StepSize
            // 
            this.textBox_VisionPopup_JogMove_StepSize.Font = new System.Drawing.Font("Tahoma", 10F);
            this.textBox_VisionPopup_JogMove_StepSize.Location = new System.Drawing.Point(162, 45);
            this.textBox_VisionPopup_JogMove_StepSize.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox_VisionPopup_JogMove_StepSize.Name = "textBox_VisionPopup_JogMove_StepSize";
            this.textBox_VisionPopup_JogMove_StepSize.Size = new System.Drawing.Size(71, 24);
            this.textBox_VisionPopup_JogMove_StepSize.TabIndex = 75;
            this.textBox_VisionPopup_JogMove_StepSize.Text = "1.000";
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label10.Location = new System.Drawing.Point(88, 44);
            this.label10.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(73, 24);
            this.label10.TabIndex = 74;
            this.label10.Text = "Step Size :";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // radioButton_VisionPopup_JogMove_Step
            // 
            this.radioButton_VisionPopup_JogMove_Step.Checked = true;
            this.radioButton_VisionPopup_JogMove_Step.Font = new System.Drawing.Font("Tahoma", 10F);
            this.radioButton_VisionPopup_JogMove_Step.Location = new System.Drawing.Point(11, 44);
            this.radioButton_VisionPopup_JogMove_Step.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioButton_VisionPopup_JogMove_Step.Name = "radioButton_VisionPopup_JogMove_Step";
            this.radioButton_VisionPopup_JogMove_Step.Size = new System.Drawing.Size(59, 24);
            this.radioButton_VisionPopup_JogMove_Step.TabIndex = 6;
            this.radioButton_VisionPopup_JogMove_Step.TabStop = true;
            this.radioButton_VisionPopup_JogMove_Step.Text = "Step";
            this.radioButton_VisionPopup_JogMove_Step.UseVisualStyleBackColor = true;
            // 
            // radioButton_VisionPopup_JogMove_Continuous
            // 
            this.radioButton_VisionPopup_JogMove_Continuous.Font = new System.Drawing.Font("Tahoma", 10F);
            this.radioButton_VisionPopup_JogMove_Continuous.Location = new System.Drawing.Point(11, 21);
            this.radioButton_VisionPopup_JogMove_Continuous.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioButton_VisionPopup_JogMove_Continuous.Name = "radioButton_VisionPopup_JogMove_Continuous";
            this.radioButton_VisionPopup_JogMove_Continuous.Size = new System.Drawing.Size(107, 24);
            this.radioButton_VisionPopup_JogMove_Continuous.TabIndex = 5;
            this.radioButton_VisionPopup_JogMove_Continuous.Text = "Continuous";
            this.radioButton_VisionPopup_JogMove_Continuous.UseVisualStyleBackColor = true;
            // 
            // groupBox22
            // 
            this.groupBox22.Controls.Add(this.radioButton_VisionPopup_Move_MoveMode_Coarse);
            this.groupBox22.Controls.Add(this.radioButton_VisionPopup_Move_MoveMode_Fine);
            this.groupBox22.Font = new System.Drawing.Font("Tahoma", 10F);
            this.groupBox22.Location = new System.Drawing.Point(10, 23);
            this.groupBox22.Margin = new System.Windows.Forms.Padding(5);
            this.groupBox22.Name = "groupBox22";
            this.groupBox22.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox22.Size = new System.Drawing.Size(275, 50);
            this.groupBox22.TabIndex = 109;
            this.groupBox22.TabStop = false;
            this.groupBox22.Text = " Move Speed ";
            // 
            // radioButton_VisionPopup_Move_MoveMode_Coarse
            // 
            this.radioButton_VisionPopup_Move_MoveMode_Coarse.Checked = true;
            this.radioButton_VisionPopup_Move_MoveMode_Coarse.Font = new System.Drawing.Font("Tahoma", 10F);
            this.radioButton_VisionPopup_Move_MoveMode_Coarse.Location = new System.Drawing.Point(144, 20);
            this.radioButton_VisionPopup_Move_MoveMode_Coarse.Margin = new System.Windows.Forms.Padding(5);
            this.radioButton_VisionPopup_Move_MoveMode_Coarse.Name = "radioButton_VisionPopup_Move_MoveMode_Coarse";
            this.radioButton_VisionPopup_Move_MoveMode_Coarse.Size = new System.Drawing.Size(85, 24);
            this.radioButton_VisionPopup_Move_MoveMode_Coarse.TabIndex = 6;
            this.radioButton_VisionPopup_Move_MoveMode_Coarse.TabStop = true;
            this.radioButton_VisionPopup_Move_MoveMode_Coarse.Text = "Coarse";
            this.radioButton_VisionPopup_Move_MoveMode_Coarse.UseVisualStyleBackColor = true;
            // 
            // radioButton_VisionPopup_Move_MoveMode_Fine
            // 
            this.radioButton_VisionPopup_Move_MoveMode_Fine.Font = new System.Drawing.Font("Tahoma", 10F);
            this.radioButton_VisionPopup_Move_MoveMode_Fine.Location = new System.Drawing.Point(38, 20);
            this.radioButton_VisionPopup_Move_MoveMode_Fine.Margin = new System.Windows.Forms.Padding(5);
            this.radioButton_VisionPopup_Move_MoveMode_Fine.Name = "radioButton_VisionPopup_Move_MoveMode_Fine";
            this.radioButton_VisionPopup_Move_MoveMode_Fine.Size = new System.Drawing.Size(80, 24);
            this.radioButton_VisionPopup_Move_MoveMode_Fine.TabIndex = 5;
            this.radioButton_VisionPopup_Move_MoveMode_Fine.Text = "Fine";
            this.radioButton_VisionPopup_Move_MoveMode_Fine.UseVisualStyleBackColor = true;
            // 
            // groupBox21
            // 
            this.groupBox21.Controls.Add(this.labelLED_VisionZ_NOT);
            this.groupBox21.Controls.Add(this.labelLED_VisionZ_POT);
            this.groupBox21.Controls.Add(this.button_VisionPopup_Z_Neg);
            this.groupBox21.Controls.Add(this.button_VisionPopup_Z_Pos);
            this.groupBox21.Font = new System.Drawing.Font("Tahoma", 10F);
            this.groupBox21.Location = new System.Drawing.Point(205, 173);
            this.groupBox21.Margin = new System.Windows.Forms.Padding(5);
            this.groupBox21.Name = "groupBox21";
            this.groupBox21.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox21.Size = new System.Drawing.Size(80, 145);
            this.groupBox21.TabIndex = 65;
            this.groupBox21.TabStop = false;
            this.groupBox21.Text = " Vision ";
            // 
            // labelLED_VisionZ_NOT
            // 
            this.labelLED_VisionZ_NOT.BackColor = System.Drawing.Color.LightGray;
            this.labelLED_VisionZ_NOT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelLED_VisionZ_NOT.Font = new System.Drawing.Font("Tahoma", 10F);
            this.labelLED_VisionZ_NOT.Location = new System.Drawing.Point(3, 126);
            this.labelLED_VisionZ_NOT.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.labelLED_VisionZ_NOT.Name = "labelLED_VisionZ_NOT";
            this.labelLED_VisionZ_NOT.Size = new System.Drawing.Size(10, 10);
            this.labelLED_VisionZ_NOT.TabIndex = 73;
            this.labelLED_VisionZ_NOT.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelLED_VisionZ_NOT.Visible = false;
            // 
            // labelLED_VisionZ_POT
            // 
            this.labelLED_VisionZ_POT.BackColor = System.Drawing.Color.LightGray;
            this.labelLED_VisionZ_POT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelLED_VisionZ_POT.Font = new System.Drawing.Font("Tahoma", 10F);
            this.labelLED_VisionZ_POT.Location = new System.Drawing.Point(3, 25);
            this.labelLED_VisionZ_POT.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.labelLED_VisionZ_POT.Name = "labelLED_VisionZ_POT";
            this.labelLED_VisionZ_POT.Size = new System.Drawing.Size(10, 10);
            this.labelLED_VisionZ_POT.TabIndex = 72;
            this.labelLED_VisionZ_POT.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelLED_VisionZ_POT.Visible = false;
            // 
            // button_VisionPopup_Z_Neg
            // 
            this.button_VisionPopup_Z_Neg.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_VisionPopup_Z_Neg.Location = new System.Drawing.Point(13, 82);
            this.button_VisionPopup_Z_Neg.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_VisionPopup_Z_Neg.Name = "button_VisionPopup_Z_Neg";
            this.button_VisionPopup_Z_Neg.Size = new System.Drawing.Size(55, 55);
            this.button_VisionPopup_Z_Neg.TabIndex = 67;
            this.button_VisionPopup_Z_Neg.Text = "-Z";
            this.button_VisionPopup_Z_Neg.UseVisualStyleBackColor = true;
            this.button_VisionPopup_Z_Neg.Click += new System.EventHandler(this.button_VisionPopup_Z_Neg_Click);
            this.button_VisionPopup_Z_Neg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_VisionPopup_Z_Neg_MouseDown);
            this.button_VisionPopup_Z_Neg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button_VisionPopup_Axis_MouseUp);
            // 
            // button_VisionPopup_Z_Pos
            // 
            this.button_VisionPopup_Z_Pos.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_VisionPopup_Z_Pos.Location = new System.Drawing.Point(13, 24);
            this.button_VisionPopup_Z_Pos.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_VisionPopup_Z_Pos.Name = "button_VisionPopup_Z_Pos";
            this.button_VisionPopup_Z_Pos.Size = new System.Drawing.Size(55, 55);
            this.button_VisionPopup_Z_Pos.TabIndex = 66;
            this.button_VisionPopup_Z_Pos.Text = "+Z";
            this.button_VisionPopup_Z_Pos.UseVisualStyleBackColor = true;
            this.button_VisionPopup_Z_Pos.Click += new System.EventHandler(this.button_VisionPopup_Z_Pos_Click);
            this.button_VisionPopup_Z_Pos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_VisionPopup_Z_Pos_MouseDown);
            this.button_VisionPopup_Z_Pos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button_VisionPopup_Axis_MouseUp);
            // 
            // groupBox58
            // 
            this.groupBox58.Controls.Add(this.labelLED_VisionX_NOT);
            this.groupBox58.Controls.Add(this.labelLED_VisionX_POT);
            this.groupBox58.Controls.Add(this.labelLED_VisionY_NOT);
            this.groupBox58.Controls.Add(this.labelLED_VisionY_POT);
            this.groupBox58.Controls.Add(this.button_VisionPopup_X_Neg);
            this.groupBox58.Controls.Add(this.button_VisionPopup_X_Pos);
            this.groupBox58.Controls.Add(this.button_VisionPopup_Y_Neg);
            this.groupBox58.Controls.Add(this.button_VisionPopup_Y_Pos);
            this.groupBox58.Font = new System.Drawing.Font("Tahoma", 10F);
            this.groupBox58.Location = new System.Drawing.Point(10, 173);
            this.groupBox58.Margin = new System.Windows.Forms.Padding(5);
            this.groupBox58.Name = "groupBox58";
            this.groupBox58.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox58.Size = new System.Drawing.Size(185, 145);
            this.groupBox58.TabIndex = 64;
            this.groupBox58.TabStop = false;
            this.groupBox58.Text = " Stage ";
            // 
            // labelLED_VisionX_NOT
            // 
            this.labelLED_VisionX_NOT.BackColor = System.Drawing.Color.LightGray;
            this.labelLED_VisionX_NOT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelLED_VisionX_NOT.Font = new System.Drawing.Font("Tahoma", 10F);
            this.labelLED_VisionX_NOT.Location = new System.Drawing.Point(8, 43);
            this.labelLED_VisionX_NOT.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.labelLED_VisionX_NOT.Name = "labelLED_VisionX_NOT";
            this.labelLED_VisionX_NOT.Size = new System.Drawing.Size(10, 10);
            this.labelLED_VisionX_NOT.TabIndex = 71;
            this.labelLED_VisionX_NOT.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelLED_VisionX_NOT.Visible = false;
            // 
            // labelLED_VisionX_POT
            // 
            this.labelLED_VisionX_POT.BackColor = System.Drawing.Color.Red;
            this.labelLED_VisionX_POT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelLED_VisionX_POT.Font = new System.Drawing.Font("Tahoma", 10F);
            this.labelLED_VisionX_POT.Location = new System.Drawing.Point(167, 43);
            this.labelLED_VisionX_POT.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.labelLED_VisionX_POT.Name = "labelLED_VisionX_POT";
            this.labelLED_VisionX_POT.Size = new System.Drawing.Size(10, 10);
            this.labelLED_VisionX_POT.TabIndex = 70;
            this.labelLED_VisionX_POT.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelLED_VisionX_POT.Visible = false;
            // 
            // labelLED_VisionY_NOT
            // 
            this.labelLED_VisionY_NOT.BackColor = System.Drawing.Color.LightGray;
            this.labelLED_VisionY_NOT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelLED_VisionY_NOT.Font = new System.Drawing.Font("Tahoma", 10F);
            this.labelLED_VisionY_NOT.Location = new System.Drawing.Point(55, 125);
            this.labelLED_VisionY_NOT.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.labelLED_VisionY_NOT.Name = "labelLED_VisionY_NOT";
            this.labelLED_VisionY_NOT.Size = new System.Drawing.Size(10, 10);
            this.labelLED_VisionY_NOT.TabIndex = 69;
            this.labelLED_VisionY_NOT.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelLED_VisionY_NOT.Visible = false;
            // 
            // labelLED_VisionY_POT
            // 
            this.labelLED_VisionY_POT.BackColor = System.Drawing.Color.LightGray;
            this.labelLED_VisionY_POT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelLED_VisionY_POT.Font = new System.Drawing.Font("Tahoma", 10F);
            this.labelLED_VisionY_POT.Location = new System.Drawing.Point(55, 26);
            this.labelLED_VisionY_POT.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.labelLED_VisionY_POT.Name = "labelLED_VisionY_POT";
            this.labelLED_VisionY_POT.Size = new System.Drawing.Size(10, 10);
            this.labelLED_VisionY_POT.TabIndex = 68;
            this.labelLED_VisionY_POT.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelLED_VisionY_POT.Visible = false;
            // 
            // button_VisionPopup_X_Neg
            // 
            this.button_VisionPopup_X_Neg.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_VisionPopup_X_Neg.Location = new System.Drawing.Point(7, 53);
            this.button_VisionPopup_X_Neg.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_VisionPopup_X_Neg.Name = "button_VisionPopup_X_Neg";
            this.button_VisionPopup_X_Neg.Size = new System.Drawing.Size(55, 55);
            this.button_VisionPopup_X_Neg.TabIndex = 59;
            this.button_VisionPopup_X_Neg.Text = "-X";
            this.button_VisionPopup_X_Neg.UseVisualStyleBackColor = true;
            this.button_VisionPopup_X_Neg.Click += new System.EventHandler(this.button_VisionPopup_X_Neg_Click);
            this.button_VisionPopup_X_Neg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_VisionPopup_X_Neg_MouseDown);
            this.button_VisionPopup_X_Neg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button_VisionPopup_Axis_MouseUp);
            // 
            // button_VisionPopup_X_Pos
            // 
            this.button_VisionPopup_X_Pos.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_VisionPopup_X_Pos.Location = new System.Drawing.Point(123, 53);
            this.button_VisionPopup_X_Pos.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_VisionPopup_X_Pos.Name = "button_VisionPopup_X_Pos";
            this.button_VisionPopup_X_Pos.Size = new System.Drawing.Size(55, 55);
            this.button_VisionPopup_X_Pos.TabIndex = 58;
            this.button_VisionPopup_X_Pos.Text = "+X";
            this.button_VisionPopup_X_Pos.UseVisualStyleBackColor = true;
            this.button_VisionPopup_X_Pos.Click += new System.EventHandler(this.button_VisionPopup_X_Pos_Click);
            this.button_VisionPopup_X_Pos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_VisionPopup_X_Pos_MouseDown);
            this.button_VisionPopup_X_Pos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button_VisionPopup_Axis_MouseUp);
            // 
            // button_VisionPopup_Y_Neg
            // 
            this.button_VisionPopup_Y_Neg.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_VisionPopup_Y_Neg.Location = new System.Drawing.Point(65, 82);
            this.button_VisionPopup_Y_Neg.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_VisionPopup_Y_Neg.Name = "button_VisionPopup_Y_Neg";
            this.button_VisionPopup_Y_Neg.Size = new System.Drawing.Size(55, 55);
            this.button_VisionPopup_Y_Neg.TabIndex = 57;
            this.button_VisionPopup_Y_Neg.Text = "-Y";
            this.button_VisionPopup_Y_Neg.UseVisualStyleBackColor = true;
            this.button_VisionPopup_Y_Neg.Click += new System.EventHandler(this.button_VisionPopup_Y_Neg_Click);
            this.button_VisionPopup_Y_Neg.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_VisionPopup_Y_Neg_MouseDown);
            this.button_VisionPopup_Y_Neg.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button_VisionPopup_Axis_MouseUp);
            // 
            // button_VisionPopup_Y_Pos
            // 
            this.button_VisionPopup_Y_Pos.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_VisionPopup_Y_Pos.Location = new System.Drawing.Point(65, 24);
            this.button_VisionPopup_Y_Pos.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_VisionPopup_Y_Pos.Name = "button_VisionPopup_Y_Pos";
            this.button_VisionPopup_Y_Pos.Size = new System.Drawing.Size(55, 55);
            this.button_VisionPopup_Y_Pos.TabIndex = 56;
            this.button_VisionPopup_Y_Pos.Text = "+Y";
            this.button_VisionPopup_Y_Pos.UseVisualStyleBackColor = true;
            this.button_VisionPopup_Y_Pos.Click += new System.EventHandler(this.button_VisionPopup_Y_Pos_Click);
            this.button_VisionPopup_Y_Pos.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_VisionPopup_Y_Pos_MouseDown);
            this.button_VisionPopup_Y_Pos.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button_VisionPopup_Axis_MouseUp);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_VisionPopup_WorkStage_CurrentFineCamPos_To_LaserSensorPos);
            this.groupBox1.Controls.Add(this.button_VisionPopup_WorkStage_CurrentLaserSensorPos_To_FineCamPos);
            this.groupBox1.Controls.Add(this.button_VisionPopup_WorkStage_CurrentCoarseCamPos_To_FineCamPos);
            this.groupBox1.Controls.Add(this.button_VisionPopup_WorkStage_CurrentFineCamPos_To_CoarseCamPos);
            this.groupBox1.Controls.Add(this.button_VisionPopup_WorkStage_CurrentFineCamPos_To_ScannerPos);
            this.groupBox1.Controls.Add(this.button_VisionPopup_WorkStage_CurrentScannerPos_To_FineCamPos);
            this.groupBox1.Controls.Add(this.button_VisionPopup_WorkStage_StageCenter_To_ScannerCenter);
            this.groupBox1.Controls.Add(this.radioButton_WorkStageMove_toCalibration);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.radioButton_WorkStageMove_toProcess);
            this.groupBox1.Controls.Add(this.radioButton_WorkStageMove_toLowMagCamera);
            this.groupBox1.Controls.Add(this.radioButton_WorkStageMove_toHighMagCamera);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(515, 422);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.groupBox1.Size = new System.Drawing.Size(392, 267);
            this.groupBox1.TabIndex = 64;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Work Stage Switching Move ";
            // 
            // button_VisionPopup_WorkStage_CurrentFineCamPos_To_LaserSensorPos
            // 
            this.button_VisionPopup_WorkStage_CurrentFineCamPos_To_LaserSensorPos.Font = new System.Drawing.Font("Tahoma", 9F);
            this.button_VisionPopup_WorkStage_CurrentFineCamPos_To_LaserSensorPos.Location = new System.Drawing.Point(124, 206);
            this.button_VisionPopup_WorkStage_CurrentFineCamPos_To_LaserSensorPos.Margin = new System.Windows.Forms.Padding(5);
            this.button_VisionPopup_WorkStage_CurrentFineCamPos_To_LaserSensorPos.Name = "button_VisionPopup_WorkStage_CurrentFineCamPos_To_LaserSensorPos";
            this.button_VisionPopup_WorkStage_CurrentFineCamPos_To_LaserSensorPos.Size = new System.Drawing.Size(113, 52);
            this.button_VisionPopup_WorkStage_CurrentFineCamPos_To_LaserSensorPos.TabIndex = 72;
            this.button_VisionPopup_WorkStage_CurrentFineCamPos_To_LaserSensorPos.Text = "Fine Cam. Pos.\r\nMove To\r\nLaser Sensor Pos.";
            this.button_VisionPopup_WorkStage_CurrentFineCamPos_To_LaserSensorPos.UseVisualStyleBackColor = true;
            this.button_VisionPopup_WorkStage_CurrentFineCamPos_To_LaserSensorPos.Click += new System.EventHandler(this.button_VisionPopup_WorkStage_CurrentFineCamPos_To_LaserSensorPos_Click);
            // 
            // button_VisionPopup_WorkStage_CurrentLaserSensorPos_To_FineCamPos
            // 
            this.button_VisionPopup_WorkStage_CurrentLaserSensorPos_To_FineCamPos.Font = new System.Drawing.Font("Tahoma", 9F);
            this.button_VisionPopup_WorkStage_CurrentLaserSensorPos_To_FineCamPos.Location = new System.Drawing.Point(10, 206);
            this.button_VisionPopup_WorkStage_CurrentLaserSensorPos_To_FineCamPos.Margin = new System.Windows.Forms.Padding(5);
            this.button_VisionPopup_WorkStage_CurrentLaserSensorPos_To_FineCamPos.Name = "button_VisionPopup_WorkStage_CurrentLaserSensorPos_To_FineCamPos";
            this.button_VisionPopup_WorkStage_CurrentLaserSensorPos_To_FineCamPos.Size = new System.Drawing.Size(113, 52);
            this.button_VisionPopup_WorkStage_CurrentLaserSensorPos_To_FineCamPos.TabIndex = 71;
            this.button_VisionPopup_WorkStage_CurrentLaserSensorPos_To_FineCamPos.Text = "Laser Sensor Pos.\r\nMove To\r\nFine Cam. Pos.";
            this.button_VisionPopup_WorkStage_CurrentLaserSensorPos_To_FineCamPos.UseVisualStyleBackColor = true;
            this.button_VisionPopup_WorkStage_CurrentLaserSensorPos_To_FineCamPos.Click += new System.EventHandler(this.button_VisionPopup_WorkStage_CurrentLaserSensorPos_To_FineCamPos_Click);
            // 
            // button_VisionPopup_WorkStage_CurrentCoarseCamPos_To_FineCamPos
            // 
            this.button_VisionPopup_WorkStage_CurrentCoarseCamPos_To_FineCamPos.Font = new System.Drawing.Font("Tahoma", 9F);
            this.button_VisionPopup_WorkStage_CurrentCoarseCamPos_To_FineCamPos.Location = new System.Drawing.Point(271, 153);
            this.button_VisionPopup_WorkStage_CurrentCoarseCamPos_To_FineCamPos.Margin = new System.Windows.Forms.Padding(5);
            this.button_VisionPopup_WorkStage_CurrentCoarseCamPos_To_FineCamPos.Name = "button_VisionPopup_WorkStage_CurrentCoarseCamPos_To_FineCamPos";
            this.button_VisionPopup_WorkStage_CurrentCoarseCamPos_To_FineCamPos.Size = new System.Drawing.Size(111, 52);
            this.button_VisionPopup_WorkStage_CurrentCoarseCamPos_To_FineCamPos.TabIndex = 70;
            this.button_VisionPopup_WorkStage_CurrentCoarseCamPos_To_FineCamPos.Text = "Coarse Cam. Pos.\r\nMove To\r\nFine Cam. Pos.";
            this.button_VisionPopup_WorkStage_CurrentCoarseCamPos_To_FineCamPos.UseVisualStyleBackColor = true;
            this.button_VisionPopup_WorkStage_CurrentCoarseCamPos_To_FineCamPos.Click += new System.EventHandler(this.button_VisionPopup_WorkStage_CurrentCoarseCamPos_To_FineCamPos_Click);
            // 
            // button_VisionPopup_WorkStage_CurrentFineCamPos_To_CoarseCamPos
            // 
            this.button_VisionPopup_WorkStage_CurrentFineCamPos_To_CoarseCamPos.Font = new System.Drawing.Font("Tahoma", 9F);
            this.button_VisionPopup_WorkStage_CurrentFineCamPos_To_CoarseCamPos.Location = new System.Drawing.Point(271, 206);
            this.button_VisionPopup_WorkStage_CurrentFineCamPos_To_CoarseCamPos.Margin = new System.Windows.Forms.Padding(5);
            this.button_VisionPopup_WorkStage_CurrentFineCamPos_To_CoarseCamPos.Name = "button_VisionPopup_WorkStage_CurrentFineCamPos_To_CoarseCamPos";
            this.button_VisionPopup_WorkStage_CurrentFineCamPos_To_CoarseCamPos.Size = new System.Drawing.Size(111, 52);
            this.button_VisionPopup_WorkStage_CurrentFineCamPos_To_CoarseCamPos.TabIndex = 69;
            this.button_VisionPopup_WorkStage_CurrentFineCamPos_To_CoarseCamPos.Text = "Fine Cam. Pos.\r\nMove To\r\nCoarse Cam. Pos.";
            this.button_VisionPopup_WorkStage_CurrentFineCamPos_To_CoarseCamPos.UseVisualStyleBackColor = true;
            this.button_VisionPopup_WorkStage_CurrentFineCamPos_To_CoarseCamPos.Click += new System.EventHandler(this.button_VisionPopup_WorkStage_CurrentFineCamPos_To_CoarseCamPos_Click);
            // 
            // button_VisionPopup_WorkStage_CurrentFineCamPos_To_ScannerPos
            // 
            this.button_VisionPopup_WorkStage_CurrentFineCamPos_To_ScannerPos.Font = new System.Drawing.Font("Tahoma", 9F);
            this.button_VisionPopup_WorkStage_CurrentFineCamPos_To_ScannerPos.Location = new System.Drawing.Point(124, 148);
            this.button_VisionPopup_WorkStage_CurrentFineCamPos_To_ScannerPos.Margin = new System.Windows.Forms.Padding(5);
            this.button_VisionPopup_WorkStage_CurrentFineCamPos_To_ScannerPos.Name = "button_VisionPopup_WorkStage_CurrentFineCamPos_To_ScannerPos";
            this.button_VisionPopup_WorkStage_CurrentFineCamPos_To_ScannerPos.Size = new System.Drawing.Size(113, 52);
            this.button_VisionPopup_WorkStage_CurrentFineCamPos_To_ScannerPos.TabIndex = 68;
            this.button_VisionPopup_WorkStage_CurrentFineCamPos_To_ScannerPos.Text = "Fine Cam. Pos.\r\nMove To\r\nScanner Pos.";
            this.button_VisionPopup_WorkStage_CurrentFineCamPos_To_ScannerPos.UseVisualStyleBackColor = true;
            this.button_VisionPopup_WorkStage_CurrentFineCamPos_To_ScannerPos.Click += new System.EventHandler(this.button_VisionPopup_WorkStage_CurrentFineCamPos_To_ScannerPos_Click);
            // 
            // button_VisionPopup_WorkStage_CurrentScannerPos_To_FineCamPos
            // 
            this.button_VisionPopup_WorkStage_CurrentScannerPos_To_FineCamPos.Font = new System.Drawing.Font("Tahoma", 9F);
            this.button_VisionPopup_WorkStage_CurrentScannerPos_To_FineCamPos.Location = new System.Drawing.Point(10, 148);
            this.button_VisionPopup_WorkStage_CurrentScannerPos_To_FineCamPos.Margin = new System.Windows.Forms.Padding(5);
            this.button_VisionPopup_WorkStage_CurrentScannerPos_To_FineCamPos.Name = "button_VisionPopup_WorkStage_CurrentScannerPos_To_FineCamPos";
            this.button_VisionPopup_WorkStage_CurrentScannerPos_To_FineCamPos.Size = new System.Drawing.Size(113, 52);
            this.button_VisionPopup_WorkStage_CurrentScannerPos_To_FineCamPos.TabIndex = 67;
            this.button_VisionPopup_WorkStage_CurrentScannerPos_To_FineCamPos.Text = "Scanner Pos.\r\nMove To\r\nFine Cam. Pos.";
            this.button_VisionPopup_WorkStage_CurrentScannerPos_To_FineCamPos.UseVisualStyleBackColor = true;
            this.button_VisionPopup_WorkStage_CurrentScannerPos_To_FineCamPos.Click += new System.EventHandler(this.button_VisionPopup_WorkStage_CurrentScannerPos_To_FineCamPos_Click);
            // 
            // button_VisionPopup_WorkStage_StageCenter_To_ScannerCenter
            // 
            this.button_VisionPopup_WorkStage_StageCenter_To_ScannerCenter.Font = new System.Drawing.Font("Tahoma", 9F);
            this.button_VisionPopup_WorkStage_StageCenter_To_ScannerCenter.Location = new System.Drawing.Point(10, 90);
            this.button_VisionPopup_WorkStage_StageCenter_To_ScannerCenter.Margin = new System.Windows.Forms.Padding(5);
            this.button_VisionPopup_WorkStage_StageCenter_To_ScannerCenter.Name = "button_VisionPopup_WorkStage_StageCenter_To_ScannerCenter";
            this.button_VisionPopup_WorkStage_StageCenter_To_ScannerCenter.Size = new System.Drawing.Size(227, 52);
            this.button_VisionPopup_WorkStage_StageCenter_To_ScannerCenter.TabIndex = 66;
            this.button_VisionPopup_WorkStage_StageCenter_To_ScannerCenter.Text = "Stage Center    Move To\r\nScanner Center";
            this.button_VisionPopup_WorkStage_StageCenter_To_ScannerCenter.UseVisualStyleBackColor = true;
            this.button_VisionPopup_WorkStage_StageCenter_To_ScannerCenter.Click += new System.EventHandler(this.button_VisionPopup_WorkStage_StageCenter_To_ScannerCenter_Click);
            // 
            // radioButton_WorkStageMove_toCalibration
            // 
            this.radioButton_WorkStageMove_toCalibration.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButton_WorkStageMove_toCalibration.Font = new System.Drawing.Font("Tahoma", 10F);
            this.radioButton_WorkStageMove_toCalibration.Location = new System.Drawing.Point(172, 243);
            this.radioButton_WorkStageMove_toCalibration.Margin = new System.Windows.Forms.Padding(6);
            this.radioButton_WorkStageMove_toCalibration.Name = "radioButton_WorkStageMove_toCalibration";
            this.radioButton_WorkStageMove_toCalibration.Size = new System.Drawing.Size(120, 45);
            this.radioButton_WorkStageMove_toCalibration.TabIndex = 11;
            this.radioButton_WorkStageMove_toCalibration.Text = "to Calibration";
            this.radioButton_WorkStageMove_toCalibration.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButton_WorkStageMove_toCalibration.UseVisualStyleBackColor = true;
            this.radioButton_WorkStageMove_toCalibration.Visible = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButton_VisionPopup_WorkStageMove_Relative);
            this.groupBox2.Controls.Add(this.radioButton_VisionPopup_WorkStageMove_Absolute);
            this.groupBox2.Font = new System.Drawing.Font("Tahoma", 10F);
            this.groupBox2.Location = new System.Drawing.Point(12, 25);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.groupBox2.Size = new System.Drawing.Size(225, 54);
            this.groupBox2.TabIndex = 65;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = " Coordinate ";
            // 
            // radioButton_VisionPopup_WorkStageMove_Relative
            // 
            this.radioButton_VisionPopup_WorkStageMove_Relative.Font = new System.Drawing.Font("Tahoma", 10F);
            this.radioButton_VisionPopup_WorkStageMove_Relative.Location = new System.Drawing.Point(128, 21);
            this.radioButton_VisionPopup_WorkStageMove_Relative.Margin = new System.Windows.Forms.Padding(6);
            this.radioButton_VisionPopup_WorkStageMove_Relative.Name = "radioButton_VisionPopup_WorkStageMove_Relative";
            this.radioButton_VisionPopup_WorkStageMove_Relative.Size = new System.Drawing.Size(90, 27);
            this.radioButton_VisionPopup_WorkStageMove_Relative.TabIndex = 6;
            this.radioButton_VisionPopup_WorkStageMove_Relative.Text = "Relative";
            this.radioButton_VisionPopup_WorkStageMove_Relative.UseVisualStyleBackColor = true;
            // 
            // radioButton_VisionPopup_WorkStageMove_Absolute
            // 
            this.radioButton_VisionPopup_WorkStageMove_Absolute.Checked = true;
            this.radioButton_VisionPopup_WorkStageMove_Absolute.Font = new System.Drawing.Font("Tahoma", 10F);
            this.radioButton_VisionPopup_WorkStageMove_Absolute.Location = new System.Drawing.Point(21, 21);
            this.radioButton_VisionPopup_WorkStageMove_Absolute.Margin = new System.Windows.Forms.Padding(6);
            this.radioButton_VisionPopup_WorkStageMove_Absolute.Name = "radioButton_VisionPopup_WorkStageMove_Absolute";
            this.radioButton_VisionPopup_WorkStageMove_Absolute.Size = new System.Drawing.Size(90, 27);
            this.radioButton_VisionPopup_WorkStageMove_Absolute.TabIndex = 5;
            this.radioButton_VisionPopup_WorkStageMove_Absolute.TabStop = true;
            this.radioButton_VisionPopup_WorkStageMove_Absolute.Text = "Absolute";
            this.radioButton_VisionPopup_WorkStageMove_Absolute.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Font = new System.Drawing.Font("Tahoma", 10F);
            this.checkBox1.Location = new System.Drawing.Point(247, 37);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(125, 38);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.Text = "with\r\nCamera Seletion";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // radioButton_WorkStageMove_toProcess
            // 
            this.radioButton_WorkStageMove_toProcess.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButton_WorkStageMove_toProcess.Font = new System.Drawing.Font("Tahoma", 10F);
            this.radioButton_WorkStageMove_toProcess.Location = new System.Drawing.Point(40, 243);
            this.radioButton_WorkStageMove_toProcess.Margin = new System.Windows.Forms.Padding(6);
            this.radioButton_WorkStageMove_toProcess.Name = "radioButton_WorkStageMove_toProcess";
            this.radioButton_WorkStageMove_toProcess.Size = new System.Drawing.Size(120, 45);
            this.radioButton_WorkStageMove_toProcess.TabIndex = 10;
            this.radioButton_WorkStageMove_toProcess.Text = "to Process";
            this.radioButton_WorkStageMove_toProcess.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButton_WorkStageMove_toProcess.UseVisualStyleBackColor = true;
            this.radioButton_WorkStageMove_toProcess.Visible = false;
            // 
            // radioButton_WorkStageMove_toLowMagCamera
            // 
            this.radioButton_WorkStageMove_toLowMagCamera.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButton_WorkStageMove_toLowMagCamera.Checked = true;
            this.radioButton_WorkStageMove_toLowMagCamera.Font = new System.Drawing.Font("Tahoma", 10F);
            this.radioButton_WorkStageMove_toLowMagCamera.Location = new System.Drawing.Point(40, 243);
            this.radioButton_WorkStageMove_toLowMagCamera.Margin = new System.Windows.Forms.Padding(6);
            this.radioButton_WorkStageMove_toLowMagCamera.Name = "radioButton_WorkStageMove_toLowMagCamera";
            this.radioButton_WorkStageMove_toLowMagCamera.Size = new System.Drawing.Size(120, 45);
            this.radioButton_WorkStageMove_toLowMagCamera.TabIndex = 8;
            this.radioButton_WorkStageMove_toLowMagCamera.TabStop = true;
            this.radioButton_WorkStageMove_toLowMagCamera.Text = "to Low Mag.";
            this.radioButton_WorkStageMove_toLowMagCamera.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButton_WorkStageMove_toLowMagCamera.UseVisualStyleBackColor = true;
            this.radioButton_WorkStageMove_toLowMagCamera.Visible = false;
            // 
            // radioButton_WorkStageMove_toHighMagCamera
            // 
            this.radioButton_WorkStageMove_toHighMagCamera.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButton_WorkStageMove_toHighMagCamera.Font = new System.Drawing.Font("Tahoma", 10F);
            this.radioButton_WorkStageMove_toHighMagCamera.Location = new System.Drawing.Point(172, 243);
            this.radioButton_WorkStageMove_toHighMagCamera.Margin = new System.Windows.Forms.Padding(6);
            this.radioButton_WorkStageMove_toHighMagCamera.Name = "radioButton_WorkStageMove_toHighMagCamera";
            this.radioButton_WorkStageMove_toHighMagCamera.Size = new System.Drawing.Size(120, 45);
            this.radioButton_WorkStageMove_toHighMagCamera.TabIndex = 9;
            this.radioButton_WorkStageMove_toHighMagCamera.Text = "to High Mag.";
            this.radioButton_WorkStageMove_toHighMagCamera.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButton_WorkStageMove_toHighMagCamera.UseVisualStyleBackColor = true;
            this.radioButton_WorkStageMove_toHighMagCamera.Visible = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioButton_VisionPopup_DisplayMode_Live);
            this.groupBox3.Controls.Add(this.radioButton_VisionPopup_DisplayMode_Capture);
            this.groupBox3.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.groupBox3.Location = new System.Drawing.Point(641, 309);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.groupBox3.Size = new System.Drawing.Size(123, 93);
            this.groupBox3.TabIndex = 62;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = " Display Mode ";
            // 
            // radioButton_VisionPopup_DisplayMode_Live
            // 
            this.radioButton_VisionPopup_DisplayMode_Live.Checked = true;
            this.radioButton_VisionPopup_DisplayMode_Live.Font = new System.Drawing.Font("Tahoma", 10F);
            this.radioButton_VisionPopup_DisplayMode_Live.Location = new System.Drawing.Point(13, 25);
            this.radioButton_VisionPopup_DisplayMode_Live.Margin = new System.Windows.Forms.Padding(6);
            this.radioButton_VisionPopup_DisplayMode_Live.Name = "radioButton_VisionPopup_DisplayMode_Live";
            this.radioButton_VisionPopup_DisplayMode_Live.Size = new System.Drawing.Size(75, 27);
            this.radioButton_VisionPopup_DisplayMode_Live.TabIndex = 5;
            this.radioButton_VisionPopup_DisplayMode_Live.TabStop = true;
            this.radioButton_VisionPopup_DisplayMode_Live.Text = "Live";
            this.radioButton_VisionPopup_DisplayMode_Live.UseVisualStyleBackColor = true;
            this.radioButton_VisionPopup_DisplayMode_Live.CheckedChanged += new System.EventHandler(this.radioButton_VisionPopup_DisplayMode_Live_CheckedChanged);
            // 
            // radioButton_VisionPopup_DisplayMode_Capture
            // 
            this.radioButton_VisionPopup_DisplayMode_Capture.Font = new System.Drawing.Font("Tahoma", 10F);
            this.radioButton_VisionPopup_DisplayMode_Capture.Location = new System.Drawing.Point(13, 55);
            this.radioButton_VisionPopup_DisplayMode_Capture.Margin = new System.Windows.Forms.Padding(6);
            this.radioButton_VisionPopup_DisplayMode_Capture.Name = "radioButton_VisionPopup_DisplayMode_Capture";
            this.radioButton_VisionPopup_DisplayMode_Capture.Size = new System.Drawing.Size(75, 27);
            this.radioButton_VisionPopup_DisplayMode_Capture.TabIndex = 6;
            this.radioButton_VisionPopup_DisplayMode_Capture.Text = "Capture";
            this.radioButton_VisionPopup_DisplayMode_Capture.UseVisualStyleBackColor = true;
            this.radioButton_VisionPopup_DisplayMode_Capture.CheckedChanged += new System.EventHandler(this.radioButton_VisionPopup_DisplayMode_Capture_CheckedChanged);
            // 
            // groupBox87
            // 
            this.groupBox87.Controls.Add(this.textBox_IlluminationValue_Red);
            this.groupBox87.Controls.Add(this.baseLabelMax_Red);
            this.groupBox87.Controls.Add(this.baseLabelMin_Red);
            this.groupBox87.Controls.Add(this.hScrollBarIlluminator_Red);
            this.groupBox87.Controls.Add(this.baseLabel_Red);
            this.groupBox87.Controls.Add(this.baseLabel_IR);
            this.groupBox87.Controls.Add(this.baseLabelMax_IR);
            this.groupBox87.Controls.Add(this.baseLabelMin_IR);
            this.groupBox87.Controls.Add(this.hScrollBarIlluminator_IR);
            this.groupBox87.Controls.Add(this.textBox_IlluminationValue_IR);
            this.groupBox87.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.groupBox87.Location = new System.Drawing.Point(162, 533);
            this.groupBox87.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox87.Name = "groupBox87";
            this.groupBox87.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.groupBox87.Size = new System.Drawing.Size(328, 139);
            this.groupBox87.TabIndex = 61;
            this.groupBox87.TabStop = false;
            this.groupBox87.Text = " Illumination Brightness (%) ";
            // 
            // textBox_IlluminationValue_Red
            // 
            this.textBox_IlluminationValue_Red.Font = new System.Drawing.Font("Tahoma", 10F);
            this.textBox_IlluminationValue_Red.Location = new System.Drawing.Point(272, 105);
            this.textBox_IlluminationValue_Red.Name = "textBox_IlluminationValue_Red";
            this.textBox_IlluminationValue_Red.Size = new System.Drawing.Size(46, 24);
            this.textBox_IlluminationValue_Red.TabIndex = 40;
            this.textBox_IlluminationValue_Red.Text = "000";
            this.textBox_IlluminationValue_Red.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseLabelMax_Red
            // 
            this.baseLabelMax_Red.Font = new System.Drawing.Font("Tahoma", 10F);
            this.baseLabelMax_Red.ForeColor = System.Drawing.Color.Black;
            this.baseLabelMax_Red.Location = new System.Drawing.Point(225, 87);
            this.baseLabelMax_Red.Name = "baseLabelMax_Red";
            this.baseLabelMax_Red.Size = new System.Drawing.Size(45, 15);
            this.baseLabelMax_Red.TabIndex = 39;
            this.baseLabelMax_Red.Text = "255";
            this.baseLabelMax_Red.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // baseLabelMin_Red
            // 
            this.baseLabelMin_Red.Font = new System.Drawing.Font("Tahoma", 10F);
            this.baseLabelMin_Red.ForeColor = System.Drawing.Color.Black;
            this.baseLabelMin_Red.Location = new System.Drawing.Point(46, 87);
            this.baseLabelMin_Red.Name = "baseLabelMin_Red";
            this.baseLabelMin_Red.Size = new System.Drawing.Size(17, 15);
            this.baseLabelMin_Red.TabIndex = 38;
            this.baseLabelMin_Red.Text = "0";
            this.baseLabelMin_Red.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // hScrollBarIlluminator_Red
            // 
            this.hScrollBarIlluminator_Red.Location = new System.Drawing.Point(46, 105);
            this.hScrollBarIlluminator_Red.Name = "hScrollBarIlluminator_Red";
            this.hScrollBarIlluminator_Red.Size = new System.Drawing.Size(223, 26);
            this.hScrollBarIlluminator_Red.TabIndex = 37;
            // 
            // baseLabel_Red
            // 
            this.baseLabel_Red.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.baseLabel_Red.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Red.Location = new System.Drawing.Point(7, 106);
            this.baseLabel_Red.Name = "baseLabel_Red";
            this.baseLabel_Red.Size = new System.Drawing.Size(35, 23);
            this.baseLabel_Red.TabIndex = 36;
            this.baseLabel_Red.Text = "Red";
            this.baseLabel_Red.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // baseLabel_IR
            // 
            this.baseLabel_IR.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.baseLabel_IR.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_IR.Location = new System.Drawing.Point(7, 46);
            this.baseLabel_IR.Name = "baseLabel_IR";
            this.baseLabel_IR.Size = new System.Drawing.Size(35, 23);
            this.baseLabel_IR.TabIndex = 35;
            this.baseLabel_IR.Text = "IR";
            this.baseLabel_IR.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // baseLabelMax_IR
            // 
            this.baseLabelMax_IR.Font = new System.Drawing.Font("Tahoma", 10F);
            this.baseLabelMax_IR.ForeColor = System.Drawing.Color.Black;
            this.baseLabelMax_IR.Location = new System.Drawing.Point(225, 27);
            this.baseLabelMax_IR.Name = "baseLabelMax_IR";
            this.baseLabelMax_IR.Size = new System.Drawing.Size(45, 15);
            this.baseLabelMax_IR.TabIndex = 32;
            this.baseLabelMax_IR.Text = "255";
            this.baseLabelMax_IR.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // baseLabelMin_IR
            // 
            this.baseLabelMin_IR.Font = new System.Drawing.Font("Tahoma", 10F);
            this.baseLabelMin_IR.ForeColor = System.Drawing.Color.Black;
            this.baseLabelMin_IR.Location = new System.Drawing.Point(46, 27);
            this.baseLabelMin_IR.Name = "baseLabelMin_IR";
            this.baseLabelMin_IR.Size = new System.Drawing.Size(17, 15);
            this.baseLabelMin_IR.TabIndex = 31;
            this.baseLabelMin_IR.Text = "0";
            this.baseLabelMin_IR.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // hScrollBarIlluminator_IR
            // 
            this.hScrollBarIlluminator_IR.Location = new System.Drawing.Point(46, 45);
            this.hScrollBarIlluminator_IR.Name = "hScrollBarIlluminator_IR";
            this.hScrollBarIlluminator_IR.Size = new System.Drawing.Size(223, 26);
            this.hScrollBarIlluminator_IR.TabIndex = 30;
            // 
            // textBox_IlluminationValue_IR
            // 
            this.textBox_IlluminationValue_IR.Font = new System.Drawing.Font("Tahoma", 10F);
            this.textBox_IlluminationValue_IR.Location = new System.Drawing.Point(272, 45);
            this.textBox_IlluminationValue_IR.Name = "textBox_IlluminationValue_IR";
            this.textBox_IlluminationValue_IR.Size = new System.Drawing.Size(46, 24);
            this.textBox_IlluminationValue_IR.TabIndex = 27;
            this.textBox_IlluminationValue_IR.Text = "000";
            this.textBox_IlluminationValue_IR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox83
            // 
            this.groupBox83.Controls.Add(this.radioButton_VisionPopup_CameraSelection_HighMag);
            this.groupBox83.Controls.Add(this.radioButton_VisionPopup_CameraSelection_LowMag);
            this.groupBox83.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.groupBox83.Location = new System.Drawing.Point(515, 309);
            this.groupBox83.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox83.Name = "groupBox83";
            this.groupBox83.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.groupBox83.Size = new System.Drawing.Size(123, 93);
            this.groupBox83.TabIndex = 55;
            this.groupBox83.TabStop = false;
            this.groupBox83.Text = " Camera ";
            // 
            // radioButton_VisionPopup_CameraSelection_HighMag
            // 
            this.radioButton_VisionPopup_CameraSelection_HighMag.Checked = true;
            this.radioButton_VisionPopup_CameraSelection_HighMag.Font = new System.Drawing.Font("Tahoma", 10F);
            this.radioButton_VisionPopup_CameraSelection_HighMag.Location = new System.Drawing.Point(13, 55);
            this.radioButton_VisionPopup_CameraSelection_HighMag.Margin = new System.Windows.Forms.Padding(6);
            this.radioButton_VisionPopup_CameraSelection_HighMag.Name = "radioButton_VisionPopup_CameraSelection_HighMag";
            this.radioButton_VisionPopup_CameraSelection_HighMag.Size = new System.Drawing.Size(86, 27);
            this.radioButton_VisionPopup_CameraSelection_HighMag.TabIndex = 6;
            this.radioButton_VisionPopup_CameraSelection_HighMag.TabStop = true;
            this.radioButton_VisionPopup_CameraSelection_HighMag.Text = "FineCam";
            this.radioButton_VisionPopup_CameraSelection_HighMag.UseVisualStyleBackColor = true;
            this.radioButton_VisionPopup_CameraSelection_HighMag.CheckedChanged += new System.EventHandler(this.radioButton_VisionPopup_CameraSelection_HighMag_CheckedChanged);
            // 
            // radioButton_VisionPopup_CameraSelection_LowMag
            // 
            this.radioButton_VisionPopup_CameraSelection_LowMag.Font = new System.Drawing.Font("Tahoma", 10F);
            this.radioButton_VisionPopup_CameraSelection_LowMag.Location = new System.Drawing.Point(13, 25);
            this.radioButton_VisionPopup_CameraSelection_LowMag.Margin = new System.Windows.Forms.Padding(6);
            this.radioButton_VisionPopup_CameraSelection_LowMag.Name = "radioButton_VisionPopup_CameraSelection_LowMag";
            this.radioButton_VisionPopup_CameraSelection_LowMag.Size = new System.Drawing.Size(101, 27);
            this.radioButton_VisionPopup_CameraSelection_LowMag.TabIndex = 5;
            this.radioButton_VisionPopup_CameraSelection_LowMag.Text = "CoarseCam";
            this.radioButton_VisionPopup_CameraSelection_LowMag.UseVisualStyleBackColor = true;
            this.radioButton_VisionPopup_CameraSelection_LowMag.CheckedChanged += new System.EventHandler(this.radioButton_VisionPopup_CameraSelection_LowMag_CheckedChanged);
            // 
            // groupBox78
            // 
            this.groupBox78.Controls.Add(this.checkBox7);
            this.groupBox78.Controls.Add(this.checkBox9);
            this.groupBox78.Controls.Add(this.checkBox10);
            this.groupBox78.Controls.Add(this.checkBox11);
            this.groupBox78.Controls.Add(this.checkBox12);
            this.groupBox78.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.groupBox78.Location = new System.Drawing.Point(11, 423);
            this.groupBox78.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox78.Name = "groupBox78";
            this.groupBox78.Padding = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.groupBox78.Size = new System.Drawing.Size(139, 213);
            this.groupBox78.TabIndex = 56;
            this.groupBox78.TabStop = false;
            this.groupBox78.Text = " Display Items ";
            // 
            // checkBox7
            // 
            this.checkBox7.Font = new System.Drawing.Font("Tahoma", 10F);
            this.checkBox7.Location = new System.Drawing.Point(12, 176);
            this.checkBox7.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.Size = new System.Drawing.Size(120, 27);
            this.checkBox7.TabIndex = 11;
            this.checkBox7.Text = "Teaching Box";
            this.checkBox7.UseVisualStyleBackColor = true;
            // 
            // checkBox9
            // 
            this.checkBox9.Checked = true;
            this.checkBox9.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox9.Font = new System.Drawing.Font("Tahoma", 10F);
            this.checkBox9.Location = new System.Drawing.Point(12, 140);
            this.checkBox9.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.checkBox9.Name = "checkBox9";
            this.checkBox9.Size = new System.Drawing.Size(120, 27);
            this.checkBox9.TabIndex = 10;
            this.checkBox9.Text = "Search Position";
            this.checkBox9.UseVisualStyleBackColor = true;
            // 
            // checkBox10
            // 
            this.checkBox10.Checked = true;
            this.checkBox10.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox10.Font = new System.Drawing.Font("Tahoma", 10F);
            this.checkBox10.Location = new System.Drawing.Point(12, 104);
            this.checkBox10.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.checkBox10.Name = "checkBox10";
            this.checkBox10.Size = new System.Drawing.Size(120, 27);
            this.checkBox10.TabIndex = 9;
            this.checkBox10.Text = "Search Box";
            this.checkBox10.UseVisualStyleBackColor = true;
            // 
            // checkBox11
            // 
            this.checkBox11.Checked = true;
            this.checkBox11.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox11.Font = new System.Drawing.Font("Tahoma", 10F);
            this.checkBox11.Location = new System.Drawing.Point(12, 68);
            this.checkBox11.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.checkBox11.Name = "checkBox11";
            this.checkBox11.Size = new System.Drawing.Size(120, 27);
            this.checkBox11.TabIndex = 8;
            this.checkBox11.Text = "Center Postion";
            this.checkBox11.UseVisualStyleBackColor = true;
            // 
            // checkBox12
            // 
            this.checkBox12.Checked = true;
            this.checkBox12.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox12.Font = new System.Drawing.Font("Tahoma", 10F);
            this.checkBox12.Location = new System.Drawing.Point(12, 32);
            this.checkBox12.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.checkBox12.Name = "checkBox12";
            this.checkBox12.Size = new System.Drawing.Size(120, 27);
            this.checkBox12.TabIndex = 7;
            this.checkBox12.Text = "Cross Line";
            this.checkBox12.UseVisualStyleBackColor = true;
            // 
            // groupBox36
            // 
            this.groupBox36.Controls.Add(this.button46);
            this.groupBox36.Controls.Add(this.button49);
            this.groupBox36.Controls.Add(this.textBox16);
            this.groupBox36.Controls.Add(this.label70);
            this.groupBox36.Controls.Add(this.textBox17);
            this.groupBox36.Controls.Add(this.label71);
            this.groupBox36.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.groupBox36.Location = new System.Drawing.Point(176, 423);
            this.groupBox36.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox36.Name = "groupBox36";
            this.groupBox36.Size = new System.Drawing.Size(314, 92);
            this.groupBox36.TabIndex = 58;
            this.groupBox36.TabStop = false;
            this.groupBox36.Text = " Vision Scale (um/pixel) ";
            // 
            // button46
            // 
            this.button46.Font = new System.Drawing.Font("Tahoma", 10F);
            this.button46.Location = new System.Drawing.Point(264, 56);
            this.button46.Name = "button46";
            this.button46.Size = new System.Drawing.Size(40, 27);
            this.button46.TabIndex = 26;
            this.button46.Text = "#";
            this.button46.UseVisualStyleBackColor = true;
            // 
            // button49
            // 
            this.button49.Font = new System.Drawing.Font("Tahoma", 10F);
            this.button49.Location = new System.Drawing.Point(264, 25);
            this.button49.Name = "button49";
            this.button49.Size = new System.Drawing.Size(40, 27);
            this.button49.TabIndex = 25;
            this.button49.Text = "#";
            this.button49.UseVisualStyleBackColor = true;
            // 
            // textBox16
            // 
            this.textBox16.Font = new System.Drawing.Font("Tahoma", 10F);
            this.textBox16.Location = new System.Drawing.Point(158, 57);
            this.textBox16.Name = "textBox16";
            this.textBox16.Size = new System.Drawing.Size(100, 24);
            this.textBox16.TabIndex = 24;
            this.textBox16.Text = "000.000";
            // 
            // label70
            // 
            this.label70.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label70.Location = new System.Drawing.Point(82, 56);
            this.label70.Margin = new System.Windows.Forms.Padding(3);
            this.label70.Name = "label70";
            this.label70.Size = new System.Drawing.Size(70, 27);
            this.label70.TabIndex = 23;
            this.label70.Text = "Y Axis :";
            this.label70.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox17
            // 
            this.textBox17.Font = new System.Drawing.Font("Tahoma", 10F);
            this.textBox17.Location = new System.Drawing.Point(158, 27);
            this.textBox17.Name = "textBox17";
            this.textBox17.Size = new System.Drawing.Size(100, 24);
            this.textBox17.TabIndex = 22;
            this.textBox17.Text = "000.000";
            // 
            // label71
            // 
            this.label71.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label71.Location = new System.Drawing.Point(82, 25);
            this.label71.Margin = new System.Windows.Forms.Padding(3);
            this.label71.Name = "label71";
            this.label71.Size = new System.Drawing.Size(70, 27);
            this.label71.TabIndex = 21;
            this.label71.Text = "X Axis :";
            this.label71.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_visionImageViewer_LowRes
            // 
            this.m_visionImageViewer_LowRes.BackColor = System.Drawing.Color.Black;
            this.m_visionImageViewer_LowRes.Camera = null;
            this.m_visionImageViewer_LowRes.CameraSwitch = null;
            this.m_visionImageViewer_LowRes.FrameRate = 1D;
            this.m_visionImageViewer_LowRes.InputImage = null;
            this.m_visionImageViewer_LowRes.IsViewCustomizedImage = false;
            this.m_visionImageViewer_LowRes.Location = new System.Drawing.Point(10, 10);
            this.m_visionImageViewer_LowRes.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.m_visionImageViewer_LowRes.Name = "m_visionImageViewer_LowRes";
            this.m_visionImageViewer_LowRes.OperatingType = QMC.Common.Hmi.VisionImageViewer.OperatingTypes.Center;
            this.m_visionImageViewer_LowRes.Simulated = false;
            this.m_visionImageViewer_LowRes.Size = new System.Drawing.Size(479, 400);
            this.m_visionImageViewer_LowRes.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.m_visionImageViewer_LowRes.TabIndex = 108;
            this.m_visionImageViewer_LowRes.TabStop = false;
            this.m_visionImageViewer_LowRes.UpdateDelayTime = 160;
            this.m_visionImageViewer_LowRes.VisibleCrossLine = true;
            // 
            // m_visionImageViewer_HighRes
            // 
            this.m_visionImageViewer_HighRes.BackColor = System.Drawing.Color.Black;
            this.m_visionImageViewer_HighRes.Camera = null;
            this.m_visionImageViewer_HighRes.CameraSwitch = null;
            this.m_visionImageViewer_HighRes.FrameRate = 1D;
            this.m_visionImageViewer_HighRes.InputImage = null;
            this.m_visionImageViewer_HighRes.IsViewCustomizedImage = false;
            this.m_visionImageViewer_HighRes.Location = new System.Drawing.Point(10, 10);
            this.m_visionImageViewer_HighRes.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.m_visionImageViewer_HighRes.Name = "m_visionImageViewer_HighRes";
            this.m_visionImageViewer_HighRes.OperatingType = QMC.Common.Hmi.VisionImageViewer.OperatingTypes.Center;
            this.m_visionImageViewer_HighRes.Simulated = false;
            this.m_visionImageViewer_HighRes.Size = new System.Drawing.Size(479, 400);
            this.m_visionImageViewer_HighRes.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.m_visionImageViewer_HighRes.TabIndex = 107;
            this.m_visionImageViewer_HighRes.TabStop = false;
            this.m_visionImageViewer_HighRes.UpdateDelayTime = 160;
            this.m_visionImageViewer_HighRes.VisibleCrossLine = true;
            // 
            // button44
            // 
            this.button44.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button44.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.button44.Location = new System.Drawing.Point(1254, 227);
            this.button44.Margin = new System.Windows.Forms.Padding(6);
            this.button44.Name = "button44";
            this.button44.Size = new System.Drawing.Size(100, 45);
            this.button44.TabIndex = 31;
            this.button44.Text = "Save As";
            this.button44.UseVisualStyleBackColor = true;
            // 
            // button43
            // 
            this.button43.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button43.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.button43.Location = new System.Drawing.Point(1254, 283);
            this.button43.Margin = new System.Windows.Forms.Padding(6);
            this.button43.Name = "button43";
            this.button43.Size = new System.Drawing.Size(100, 45);
            this.button43.TabIndex = 32;
            this.button43.Text = "Cancel";
            this.button43.UseVisualStyleBackColor = true;
            // 
            // button47
            // 
            this.button47.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button47.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.button47.Location = new System.Drawing.Point(1254, 172);
            this.button47.Margin = new System.Windows.Forms.Padding(6);
            this.button47.Name = "button47";
            this.button47.Size = new System.Drawing.Size(100, 45);
            this.button47.TabIndex = 30;
            this.button47.Text = "Save";
            this.button47.UseVisualStyleBackColor = true;
            this.button47.Click += new System.EventHandler(this.button47_Click);
            // 
            // button48
            // 
            this.button48.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button48.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.button48.Location = new System.Drawing.Point(1254, 116);
            this.button48.Margin = new System.Windows.Forms.Padding(6);
            this.button48.Name = "button48";
            this.button48.Size = new System.Drawing.Size(100, 45);
            this.button48.TabIndex = 29;
            this.button48.Text = "Apply";
            this.button48.UseVisualStyleBackColor = true;
            // 
            // FormNew_VisionPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1373, 729);
            this.Controls.Add(this.panel4);
            this.Font = new System.Drawing.Font("Tahoma", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormNew_VisionPopup";
            this.Text = "Vision Popup Dialog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormNew_VisionPopup_FormClosing);
            this.Shown += new System.EventHandler(this.FormNew_VisionPopup_Shown);
            this.VisibleChanged += new System.EventHandler(this.FormNew_VisionPopup_VisibleChanged);
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.tabControl_MarkFindType.ResumeLayout(false);
            this.tabPage_MarkFind_PatternMatching.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_VisionPopup_PM_ImageDisplay)).EndInit();
            this.tabPage_MarkFind_CircleFind.ResumeLayout(false);
            this.tabPage_MarkFind_CircleFind.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_ImageDisplay)).EndInit();
            this.tabPage_Socket_AlignTest.ResumeLayout(false);
            this.tabPage_Socket_AlignTest.PerformLayout();
            this.tabPage_ScannerCal.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_ImageDisplayScannerCal)).EndInit();
            this.groupBox19.ResumeLayout(false);
            this.groupBox20.ResumeLayout(false);
            this.groupBox20.PerformLayout();
            this.groupBox22.ResumeLayout(false);
            this.groupBox21.ResumeLayout(false);
            this.groupBox58.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox87.ResumeLayout(false);
            this.groupBox87.PerformLayout();
            this.groupBox83.ResumeLayout(false);
            this.groupBox78.ResumeLayout(false);
            this.groupBox36.ResumeLayout(false);
            this.groupBox36.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_visionImageViewer_LowRes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_visionImageViewer_HighRes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel4;
        private Button button41;
        private Button button42;
        private Panel panel3;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private RadioButton radioButton_VisionPopup_WorkStageMove_Relative;
        private RadioButton radioButton_VisionPopup_WorkStageMove_Absolute;
        private CheckBox checkBox1;
        private GroupBox groupBox3;
        private RadioButton radioButton_VisionPopup_DisplayMode_Live;
        private RadioButton radioButton_VisionPopup_DisplayMode_Capture;
        private GroupBox groupBox87;
        private TextBox textBox_IlluminationValue_IR;
        private GroupBox groupBox83;
        private RadioButton radioButton_VisionPopup_CameraSelection_HighMag;
        private RadioButton radioButton_VisionPopup_CameraSelection_LowMag;
        private GroupBox groupBox78;
        private CheckBox checkBox7;
        private CheckBox checkBox9;
        private CheckBox checkBox10;
        private CheckBox checkBox11;
        private CheckBox checkBox12;
        private GroupBox groupBox36;
        private Button button46;
        private Button button49;
        private TextBox textBox16;
        private Label label70;
        private TextBox textBox17;
        private Label label71;
        private Button button44;
        private Button button43;
        private Button button47;
        private Button button48;
        private QMC.Common.Hmi.VisionImageViewer m_visionImageViewer_HighRes;
        private QMC.Common.Hmi.VisionImageViewer m_visionImageViewer_LowRes;
        private BaseLabel baseLabelMax_IR;
        private BaseLabel baseLabelMin_IR;
        private HScrollBar hScrollBarIlluminator_IR;
        private HScrollBar hScrollBarIlluminator_Red;
        private GroupBox groupBox19;
        private GroupBox groupBox20;
        private Button button_KeypadCall_VisionPopup_JogMove_StepSize;
        private TextBox textBox_VisionPopup_JogMove_StepSize;
        private Label label10;
        private RadioButton radioButton_VisionPopup_JogMove_Step;
        private RadioButton radioButton_VisionPopup_JogMove_Continuous;
        private GroupBox groupBox21;
        private Label labelLED_VisionZ_NOT;
        private Label labelLED_VisionZ_POT;
        private Button button_VisionPopup_Z_Neg;
        private Button button_VisionPopup_Z_Pos;
        private GroupBox groupBox58;
        private Label labelLED_VisionX_NOT;
        private Label labelLED_VisionX_POT;
        private Label labelLED_VisionY_NOT;
        private Label labelLED_VisionY_POT;
        private Button button_VisionPopup_X_Neg;
        private Button button_VisionPopup_X_Pos;
        private Button button_VisionPopup_Y_Neg;
        private Button button_VisionPopup_Y_Pos;
        private GroupBox groupBox22;
        private RadioButton radioButton_VisionPopup_Move_MoveMode_Coarse;
        private RadioButton radioButton_VisionPopup_Move_MoveMode_Fine;
        private Button btnCamera_StartLive;
        private Button btnCamera_Init;
        public Button button_Scanner_FineCam_OffsetCheck;
        public Button button_Scanner_FineCam_OffsetChange;
        private TabControl tabControl_MarkFindType;
        private TabPage tabPage_MarkFind_PatternMatching;
        private TabPage tabPage_MarkFind_CircleFind;
        private GroupBox groupBox5;
        private PictureBox pictureBox_VisionPopup_PM_ImageDisplay;
        private Label label2;
        private Label label1;
        private RichTextBox richTextBox1;
        private Button button34;
        private Button button_VisionPopup_Search;
        private Button button_VisionPopup_FindCircle_Search;
        private Button button_VisionPopup_FindCircle_LoadImage;
        private PictureBox pictureBox_ImageDisplay;
        private Button button_VisionPopup_FindCircle_GrabImage;
        private ListBox listBox_FindCircle_Result;
        private Button button_CurrentZPos_toLaserFocus;
        private Button button_CurrentZPos_toFineCamFocus;
        private Button button_CurrentLightValue_toAlignLightValue;
        private TabPage tabPage_Socket_AlignTest;
        private Label label3;
        private ComboBox comboBox_Config_VisionPopup_AlignTest_SocketList;
        private Button button_Test_SocketAlign_Start;
        private TextBox textBox_Config_VisionPopup_AlignTest_Socket_CenterY;
        private TextBox textBox_Config_VisionPopup_AlignTest_Socket_CenterX;
        private Label label5;
        private Label label4;
        private Button button_Test_MoveTo_FiducialPos;
        private ComboBox comboBox_Config_VisionPopup_AlignTest_SelectedSocket_FiducialList;
        private Label label6;
        private RadioButton radioButton_WorkStageMove_toCalibration;
        private RadioButton radioButton_WorkStageMove_toProcess;
        private RadioButton radioButton_WorkStageMove_toLowMagCamera;
        private RadioButton radioButton_WorkStageMove_toHighMagCamera;
        private Button button_VisionPopup_WorkStage_CurrentCoarseCamPos_To_FineCamPos;
        private Button button_VisionPopup_WorkStage_CurrentFineCamPos_To_CoarseCamPos;
        private Button button_VisionPopup_WorkStage_CurrentFineCamPos_To_ScannerPos;
        private Button button_VisionPopup_WorkStage_CurrentScannerPos_To_FineCamPos;
        private Button button_VisionPopup_WorkStage_StageCenter_To_ScannerCenter;
        private Button button_VisionPopup_WorkStage_CurrentFineCamPos_To_LaserSensorPos;
        private Button button_VisionPopup_WorkStage_CurrentLaserSensorPos_To_FineCamPos;
        private Button button_VisionPopup_FindMetalPowder_Search;
        private Button button_Test_MoveTo_CorrectedPos;
        private GroupBox groupBox4;
        private Button button_VisionPopup_LaserHeightCheck_Start;
        private Label textBox_VisionPopup_LaserHeightValue;
        private Button btnTest;
        private TabPage tabPage_ScannerCal;
        private Button btnTrain;
        private PictureBox pictureBox_ImageDisplayScannerCal;
        private Button button2;
        private Button button1;
        private ListBox listBox_VisionPopup_PM_Result;
        private Button button_VisionPopup_PM_Grab;
        private BaseLabel baseLabel_Red;
        private BaseLabel baseLabel_IR;
        private BaseLabel baseLabelMax_Red;
        private BaseLabel baseLabelMin_Red;
        private TextBox textBox_IlluminationValue_Red;
        private Label label8;
        private TextBox textBox_VisionPopup_FiducialSize_Width;
        private Label label7;
        private ComboBox comboBox_VisionPopup_FiducialColor;
    }
}