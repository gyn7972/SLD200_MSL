using System.Drawing;
using System.Windows.Forms;

namespace SLD200_MSL
{
    partial class FormNew_CommunicationTerminal
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
            this.label_CommunicationTerminal_ReceivedData = new System.Windows.Forms.Label();
            this.label52 = new System.Windows.Forms.Label();
            this.textBox28 = new System.Windows.Forms.TextBox();
            this.label127 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label_Activated_Unit = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label_CommTerminal_LinkStatus = new System.Windows.Forms.Label();
            this.button_CommTerminal_Connect = new System.Windows.Forms.Button();
            this.button_CommTerminal_Disconnect = new System.Windows.Forms.Button();
            this.tabControl_CommTestFunction = new System.Windows.Forms.TabControl();
            this.tabPage_Illuminator = new System.Windows.Forms.TabPage();
            this.hScrollBarIlluminator = new System.Windows.Forms.HScrollBar();
            this.radioButton_IlluminatorChannel_1 = new System.Windows.Forms.RadioButton();
            this.radioButton_IlluminatorChannel_0 = new System.Windows.Forms.RadioButton();
            this.tabPage_PowerMeter_BDS = new System.Windows.Forms.TabPage();
            this.button_PowerMeter_BDS_ContinuousReading = new System.Windows.Forms.Button();
            this.button_PowerMeter_BDS_ReadOnce = new System.Windows.Forms.Button();
            this.tabPage_PowerMeter_Stage = new System.Windows.Forms.TabPage();
            this.button_PowerMeter_Stage_ContinuousReading = new System.Windows.Forms.Button();
            this.button_PowerMeter_Stage_ReadOnce = new System.Windows.Forms.Button();
            this.tabPage_MotorizedBeamExpander = new System.Windows.Forms.TabPage();
            this.button_BeamExpander_Mrad_InitCommand = new System.Windows.Forms.Button();
            this.button_BeamExpander_Zoom_InitCommand = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button_BeamExpander_Mrad_MoveCommand = new System.Windows.Forms.Button();
            this.textBox_BeamExpander_Mrad_Position = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.button_BeamExpander_Zoom_MoveCommand = new System.Windows.Forms.Button();
            this.textBox_BeamExpander_Zoom_Position = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tabPage_DustCollector_Upper = new System.Windows.Forms.TabPage();
            this.button_Test3 = new System.Windows.Forms.Button();
            this.button_Test2 = new System.Windows.Forms.Button();
            this.button_TEST1 = new System.Windows.Forms.Button();
            this.button_DustCollector_Upper_WriteCommand = new System.Windows.Forms.Button();
            this.textBox_DustCollector_Upper_Data = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_DustCollector_Upper_Address = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage_DustCollector_Lower = new System.Windows.Forms.TabPage();
            this.button_DustCollector_Lower_WriteCommand = new System.Windows.Forms.Button();
            this.textBox_DustCollector_Lower_Data = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_DustCollector_Lower_Address = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tabPage_ElectroPneumaticRegulator = new System.Windows.Forms.TabPage();
            this.button_ElectroPneumaticRegulator_SetPressure_Read = new System.Windows.Forms.Button();
            this.button_ElectroPneumaticRegulator_GetPressure = new System.Windows.Forms.Button();
            this.button_ElectroPneumaticRegulator_Pressure_Dec = new System.Windows.Forms.Button();
            this.button_ElectroPneumaticRegulator_Pressure_Inc = new System.Windows.Forms.Button();
            this.button_ElectroPneumaticRegulator_SetValue = new System.Windows.Forms.Button();
            this.textBox_ElectroPneumaticRegulator_SetValue = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tabPage_Laser = new System.Windows.Forms.TabPage();
            this.tabPage_LaserHeightSensor = new System.Windows.Forms.TabPage();
            this.button_LaserHeightSensor_ReadValue = new System.Windows.Forms.Button();
            this.button_LaserHeightSensor_MeasurementMode = new System.Windows.Forms.Button();
            this.button_LaserHeightSensor_SettingMode = new System.Windows.Forms.Button();
            this.baseLabel1Value = new SLD200_MSL.BaseLabel();
            this.baseLabelMax = new SLD200_MSL.BaseLabel();
            this.baseLabelMin = new SLD200_MSL.BaseLabel();
            this.button3 = new System.Windows.Forms.Button();
            this.tabControl_CommTestFunction.SuspendLayout();
            this.tabPage_Illuminator.SuspendLayout();
            this.tabPage_PowerMeter_BDS.SuspendLayout();
            this.tabPage_PowerMeter_Stage.SuspendLayout();
            this.tabPage_MotorizedBeamExpander.SuspendLayout();
            this.tabPage_DustCollector_Upper.SuspendLayout();
            this.tabPage_DustCollector_Lower.SuspendLayout();
            this.tabPage_ElectroPneumaticRegulator.SuspendLayout();
            this.tabPage_LaserHeightSensor.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_CommunicationTerminal_ReceivedData
            // 
            this.label_CommunicationTerminal_ReceivedData.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_CommunicationTerminal_ReceivedData.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_CommunicationTerminal_ReceivedData.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label_CommunicationTerminal_ReceivedData.ForeColor = System.Drawing.Color.Lime;
            this.label_CommunicationTerminal_ReceivedData.Location = new System.Drawing.Point(172, 124);
            this.label_CommunicationTerminal_ReceivedData.Margin = new System.Windows.Forms.Padding(6);
            this.label_CommunicationTerminal_ReceivedData.Name = "label_CommunicationTerminal_ReceivedData";
            this.label_CommunicationTerminal_ReceivedData.Size = new System.Drawing.Size(383, 246);
            this.label_CommunicationTerminal_ReceivedData.TabIndex = 18;
            this.label_CommunicationTerminal_ReceivedData.Text = "TEST";
            // 
            // label52
            // 
            this.label52.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.label52.Location = new System.Drawing.Point(15, 122);
            this.label52.Margin = new System.Windows.Forms.Padding(6);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(145, 24);
            this.label52.TabIndex = 17;
            this.label52.Text = "Received Message :";
            this.label52.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox28
            // 
            this.textBox28.Font = new System.Drawing.Font("Tahoma", 10F);
            this.textBox28.Location = new System.Drawing.Point(172, 95);
            this.textBox28.Margin = new System.Windows.Forms.Padding(6);
            this.textBox28.Name = "textBox28";
            this.textBox28.Size = new System.Drawing.Size(383, 24);
            this.textBox28.TabIndex = 24;
            this.textBox28.Text = "000.000";
            // 
            // label127
            // 
            this.label127.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.label127.Location = new System.Drawing.Point(15, 93);
            this.label127.Margin = new System.Windows.Forms.Padding(6);
            this.label127.Name = "label127";
            this.label127.Size = new System.Drawing.Size(145, 24);
            this.label127.TabIndex = 23;
            this.label127.Text = "Send Message :";
            this.label127.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button1.Location = new System.Drawing.Point(33, 311);
            this.button1.Margin = new System.Windows.Forms.Padding(6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(127, 59);
            this.button1.TabIndex = 25;
            this.button1.Text = "Query";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(15, 12);
            this.label1.Margin = new System.Windows.Forms.Padding(6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 24);
            this.label1.TabIndex = 26;
            this.label1.Text = "Selected Unit :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_Activated_Unit
            // 
            this.label_Activated_Unit.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_Activated_Unit.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_Activated_Unit.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label_Activated_Unit.ForeColor = System.Drawing.Color.Lime;
            this.label_Activated_Unit.Location = new System.Drawing.Point(172, 12);
            this.label_Activated_Unit.Margin = new System.Windows.Forms.Padding(6);
            this.label_Activated_Unit.Name = "label_Activated_Unit";
            this.label_Activated_Unit.Size = new System.Drawing.Size(383, 27);
            this.label_Activated_Unit.TabIndex = 27;
            this.label_Activated_Unit.Text = "TEST";
            this.label_Activated_Unit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(15, 43);
            this.label2.Margin = new System.Windows.Forms.Padding(6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(145, 24);
            this.label2.TabIndex = 28;
            this.label2.Text = "Link Status :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_CommTerminal_LinkStatus
            // 
            this.label_CommTerminal_LinkStatus.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_CommTerminal_LinkStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_CommTerminal_LinkStatus.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label_CommTerminal_LinkStatus.ForeColor = System.Drawing.Color.Lime;
            this.label_CommTerminal_LinkStatus.Location = new System.Drawing.Point(172, 43);
            this.label_CommTerminal_LinkStatus.Margin = new System.Windows.Forms.Padding(6);
            this.label_CommTerminal_LinkStatus.Name = "label_CommTerminal_LinkStatus";
            this.label_CommTerminal_LinkStatus.Size = new System.Drawing.Size(143, 27);
            this.label_CommTerminal_LinkStatus.TabIndex = 29;
            this.label_CommTerminal_LinkStatus.Text = "Not Connected";
            this.label_CommTerminal_LinkStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button_CommTerminal_Connect
            // 
            this.button_CommTerminal_Connect.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_CommTerminal_Connect.Location = new System.Drawing.Point(325, 43);
            this.button_CommTerminal_Connect.Margin = new System.Windows.Forms.Padding(6);
            this.button_CommTerminal_Connect.Name = "button_CommTerminal_Connect";
            this.button_CommTerminal_Connect.Size = new System.Drawing.Size(114, 27);
            this.button_CommTerminal_Connect.TabIndex = 30;
            this.button_CommTerminal_Connect.Text = "Connect";
            this.button_CommTerminal_Connect.UseVisualStyleBackColor = true;
            this.button_CommTerminal_Connect.Click += new System.EventHandler(this.button_CommTerminal_Connect_Click);
            // 
            // button_CommTerminal_Disconnect
            // 
            this.button_CommTerminal_Disconnect.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_CommTerminal_Disconnect.Location = new System.Drawing.Point(441, 43);
            this.button_CommTerminal_Disconnect.Margin = new System.Windows.Forms.Padding(6);
            this.button_CommTerminal_Disconnect.Name = "button_CommTerminal_Disconnect";
            this.button_CommTerminal_Disconnect.Size = new System.Drawing.Size(114, 27);
            this.button_CommTerminal_Disconnect.TabIndex = 31;
            this.button_CommTerminal_Disconnect.Text = "Disconnect";
            this.button_CommTerminal_Disconnect.UseVisualStyleBackColor = true;
            this.button_CommTerminal_Disconnect.Click += new System.EventHandler(this.button_CommTerminal_Disconnect_Click);
            // 
            // tabControl_CommTestFunction
            // 
            this.tabControl_CommTestFunction.Controls.Add(this.tabPage_Illuminator);
            this.tabControl_CommTestFunction.Controls.Add(this.tabPage_PowerMeter_BDS);
            this.tabControl_CommTestFunction.Controls.Add(this.tabPage_PowerMeter_Stage);
            this.tabControl_CommTestFunction.Controls.Add(this.tabPage_MotorizedBeamExpander);
            this.tabControl_CommTestFunction.Controls.Add(this.tabPage_DustCollector_Upper);
            this.tabControl_CommTestFunction.Controls.Add(this.tabPage_DustCollector_Lower);
            this.tabControl_CommTestFunction.Controls.Add(this.tabPage_ElectroPneumaticRegulator);
            this.tabControl_CommTestFunction.Controls.Add(this.tabPage_Laser);
            this.tabControl_CommTestFunction.Controls.Add(this.tabPage_LaserHeightSensor);
            this.tabControl_CommTestFunction.ItemSize = new System.Drawing.Size(140, 21);
            this.tabControl_CommTestFunction.Location = new System.Drawing.Point(33, 400);
            this.tabControl_CommTestFunction.Multiline = true;
            this.tabControl_CommTestFunction.Name = "tabControl_CommTestFunction";
            this.tabControl_CommTestFunction.SelectedIndex = 0;
            this.tabControl_CommTestFunction.Size = new System.Drawing.Size(522, 239);
            this.tabControl_CommTestFunction.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabControl_CommTestFunction.TabIndex = 32;
            // 
            // tabPage_Illuminator
            // 
            this.tabPage_Illuminator.Controls.Add(this.baseLabel1Value);
            this.tabPage_Illuminator.Controls.Add(this.baseLabelMax);
            this.tabPage_Illuminator.Controls.Add(this.baseLabelMin);
            this.tabPage_Illuminator.Controls.Add(this.hScrollBarIlluminator);
            this.tabPage_Illuminator.Controls.Add(this.radioButton_IlluminatorChannel_1);
            this.tabPage_Illuminator.Controls.Add(this.radioButton_IlluminatorChannel_0);
            this.tabPage_Illuminator.Location = new System.Drawing.Point(4, 67);
            this.tabPage_Illuminator.Name = "tabPage_Illuminator";
            this.tabPage_Illuminator.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Illuminator.Size = new System.Drawing.Size(514, 168);
            this.tabPage_Illuminator.TabIndex = 0;
            this.tabPage_Illuminator.Text = "Illuminator";
            this.tabPage_Illuminator.UseVisualStyleBackColor = true;
            // 
            // hScrollBarIlluminator
            // 
            this.hScrollBarIlluminator.Location = new System.Drawing.Point(10, 106);
            this.hScrollBarIlluminator.Name = "hScrollBarIlluminator";
            this.hScrollBarIlluminator.Size = new System.Drawing.Size(255, 28);
            this.hScrollBarIlluminator.TabIndex = 10;
            // 
            // radioButton_IlluminatorChannel_1
            // 
            this.radioButton_IlluminatorChannel_1.AutoSize = true;
            this.radioButton_IlluminatorChannel_1.Location = new System.Drawing.Point(10, 36);
            this.radioButton_IlluminatorChannel_1.Name = "radioButton_IlluminatorChannel_1";
            this.radioButton_IlluminatorChannel_1.Size = new System.Drawing.Size(202, 20);
            this.radioButton_IlluminatorChannel_1.TabIndex = 1;
            this.radioButton_IlluminatorChannel_1.Text = "Channel 1 (High Mag. Camera)";
            this.radioButton_IlluminatorChannel_1.UseVisualStyleBackColor = true;
            this.radioButton_IlluminatorChannel_1.CheckedChanged += new System.EventHandler(this.radioButton_IlluminatorChannel_1_CheckedChanged);
            // 
            // radioButton_IlluminatorChannel_0
            // 
            this.radioButton_IlluminatorChannel_0.AutoSize = true;
            this.radioButton_IlluminatorChannel_0.Checked = true;
            this.radioButton_IlluminatorChannel_0.Location = new System.Drawing.Point(10, 10);
            this.radioButton_IlluminatorChannel_0.Name = "radioButton_IlluminatorChannel_0";
            this.radioButton_IlluminatorChannel_0.Size = new System.Drawing.Size(200, 20);
            this.radioButton_IlluminatorChannel_0.TabIndex = 0;
            this.radioButton_IlluminatorChannel_0.TabStop = true;
            this.radioButton_IlluminatorChannel_0.Text = "Channel 0 (Low Mag. Camera)";
            this.radioButton_IlluminatorChannel_0.UseVisualStyleBackColor = true;
            this.radioButton_IlluminatorChannel_0.CheckedChanged += new System.EventHandler(this.radioButton_IlluminatorChannel_0_CheckedChanged);
            // 
            // tabPage_PowerMeter_BDS
            // 
            this.tabPage_PowerMeter_BDS.Controls.Add(this.button_PowerMeter_BDS_ContinuousReading);
            this.tabPage_PowerMeter_BDS.Controls.Add(this.button_PowerMeter_BDS_ReadOnce);
            this.tabPage_PowerMeter_BDS.Location = new System.Drawing.Point(4, 67);
            this.tabPage_PowerMeter_BDS.Name = "tabPage_PowerMeter_BDS";
            this.tabPage_PowerMeter_BDS.Size = new System.Drawing.Size(514, 168);
            this.tabPage_PowerMeter_BDS.TabIndex = 5;
            this.tabPage_PowerMeter_BDS.Text = "PowerMeter (BDS)";
            this.tabPage_PowerMeter_BDS.UseVisualStyleBackColor = true;
            // 
            // button_PowerMeter_BDS_ContinuousReading
            // 
            this.button_PowerMeter_BDS_ContinuousReading.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_PowerMeter_BDS_ContinuousReading.Location = new System.Drawing.Point(151, 10);
            this.button_PowerMeter_BDS_ContinuousReading.Margin = new System.Windows.Forms.Padding(6);
            this.button_PowerMeter_BDS_ContinuousReading.Name = "button_PowerMeter_BDS_ContinuousReading";
            this.button_PowerMeter_BDS_ContinuousReading.Size = new System.Drawing.Size(127, 47);
            this.button_PowerMeter_BDS_ContinuousReading.TabIndex = 27;
            this.button_PowerMeter_BDS_ContinuousReading.Text = "Continuous Reading";
            this.button_PowerMeter_BDS_ContinuousReading.UseVisualStyleBackColor = true;
            this.button_PowerMeter_BDS_ContinuousReading.Click += new System.EventHandler(this.button_PowerMeter_BDS_ContinuousReading_Click);
            // 
            // button_PowerMeter_BDS_ReadOnce
            // 
            this.button_PowerMeter_BDS_ReadOnce.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_PowerMeter_BDS_ReadOnce.Location = new System.Drawing.Point(10, 10);
            this.button_PowerMeter_BDS_ReadOnce.Margin = new System.Windows.Forms.Padding(6);
            this.button_PowerMeter_BDS_ReadOnce.Name = "button_PowerMeter_BDS_ReadOnce";
            this.button_PowerMeter_BDS_ReadOnce.Size = new System.Drawing.Size(127, 47);
            this.button_PowerMeter_BDS_ReadOnce.TabIndex = 26;
            this.button_PowerMeter_BDS_ReadOnce.Text = "Read Once";
            this.button_PowerMeter_BDS_ReadOnce.UseVisualStyleBackColor = true;
            this.button_PowerMeter_BDS_ReadOnce.Click += new System.EventHandler(this.button_PowerMeter_BDS_ReadOnce_Click);
            // 
            // tabPage_PowerMeter_Stage
            // 
            this.tabPage_PowerMeter_Stage.Controls.Add(this.button_PowerMeter_Stage_ContinuousReading);
            this.tabPage_PowerMeter_Stage.Controls.Add(this.button_PowerMeter_Stage_ReadOnce);
            this.tabPage_PowerMeter_Stage.Location = new System.Drawing.Point(4, 67);
            this.tabPage_PowerMeter_Stage.Name = "tabPage_PowerMeter_Stage";
            this.tabPage_PowerMeter_Stage.Size = new System.Drawing.Size(514, 168);
            this.tabPage_PowerMeter_Stage.TabIndex = 6;
            this.tabPage_PowerMeter_Stage.Text = "PowerMeter (Stage)";
            this.tabPage_PowerMeter_Stage.UseVisualStyleBackColor = true;
            // 
            // button_PowerMeter_Stage_ContinuousReading
            // 
            this.button_PowerMeter_Stage_ContinuousReading.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_PowerMeter_Stage_ContinuousReading.Location = new System.Drawing.Point(151, 10);
            this.button_PowerMeter_Stage_ContinuousReading.Margin = new System.Windows.Forms.Padding(6);
            this.button_PowerMeter_Stage_ContinuousReading.Name = "button_PowerMeter_Stage_ContinuousReading";
            this.button_PowerMeter_Stage_ContinuousReading.Size = new System.Drawing.Size(127, 47);
            this.button_PowerMeter_Stage_ContinuousReading.TabIndex = 29;
            this.button_PowerMeter_Stage_ContinuousReading.Text = "Continuous Reading";
            this.button_PowerMeter_Stage_ContinuousReading.UseVisualStyleBackColor = true;
            this.button_PowerMeter_Stage_ContinuousReading.Click += new System.EventHandler(this.button_PowerMeter_Stage_ContinuousReading_Click);
            // 
            // button_PowerMeter_Stage_ReadOnce
            // 
            this.button_PowerMeter_Stage_ReadOnce.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_PowerMeter_Stage_ReadOnce.Location = new System.Drawing.Point(10, 10);
            this.button_PowerMeter_Stage_ReadOnce.Margin = new System.Windows.Forms.Padding(6);
            this.button_PowerMeter_Stage_ReadOnce.Name = "button_PowerMeter_Stage_ReadOnce";
            this.button_PowerMeter_Stage_ReadOnce.Size = new System.Drawing.Size(127, 47);
            this.button_PowerMeter_Stage_ReadOnce.TabIndex = 28;
            this.button_PowerMeter_Stage_ReadOnce.Text = "Read Once";
            this.button_PowerMeter_Stage_ReadOnce.UseVisualStyleBackColor = true;
            this.button_PowerMeter_Stage_ReadOnce.Click += new System.EventHandler(this.button_PowerMeter_Stage_ReadOnce_Click);
            // 
            // tabPage_MotorizedBeamExpander
            // 
            this.tabPage_MotorizedBeamExpander.Controls.Add(this.button_BeamExpander_Mrad_InitCommand);
            this.tabPage_MotorizedBeamExpander.Controls.Add(this.button_BeamExpander_Zoom_InitCommand);
            this.tabPage_MotorizedBeamExpander.Controls.Add(this.button2);
            this.tabPage_MotorizedBeamExpander.Controls.Add(this.button_BeamExpander_Mrad_MoveCommand);
            this.tabPage_MotorizedBeamExpander.Controls.Add(this.textBox_BeamExpander_Mrad_Position);
            this.tabPage_MotorizedBeamExpander.Controls.Add(this.label9);
            this.tabPage_MotorizedBeamExpander.Controls.Add(this.button_BeamExpander_Zoom_MoveCommand);
            this.tabPage_MotorizedBeamExpander.Controls.Add(this.textBox_BeamExpander_Zoom_Position);
            this.tabPage_MotorizedBeamExpander.Controls.Add(this.label8);
            this.tabPage_MotorizedBeamExpander.Location = new System.Drawing.Point(4, 67);
            this.tabPage_MotorizedBeamExpander.Name = "tabPage_MotorizedBeamExpander";
            this.tabPage_MotorizedBeamExpander.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_MotorizedBeamExpander.Size = new System.Drawing.Size(514, 168);
            this.tabPage_MotorizedBeamExpander.TabIndex = 1;
            this.tabPage_MotorizedBeamExpander.Text = "Mot. Beam Expander";
            this.tabPage_MotorizedBeamExpander.UseVisualStyleBackColor = true;
            // 
            // button_BeamExpander_Mrad_InitCommand
            // 
            this.button_BeamExpander_Mrad_InitCommand.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_BeamExpander_Mrad_InitCommand.Location = new System.Drawing.Point(380, 55);
            this.button_BeamExpander_Mrad_InitCommand.Margin = new System.Windows.Forms.Padding(6);
            this.button_BeamExpander_Mrad_InitCommand.Name = "button_BeamExpander_Mrad_InitCommand";
            this.button_BeamExpander_Mrad_InitCommand.Size = new System.Drawing.Size(107, 34);
            this.button_BeamExpander_Mrad_InitCommand.TabIndex = 38;
            this.button_BeamExpander_Mrad_InitCommand.Text = "Mrad Init";
            this.button_BeamExpander_Mrad_InitCommand.UseVisualStyleBackColor = true;
            this.button_BeamExpander_Mrad_InitCommand.Click += new System.EventHandler(this.button_BeamExpander_Mrad_InitCommand_Click);
            // 
            // button_BeamExpander_Zoom_InitCommand
            // 
            this.button_BeamExpander_Zoom_InitCommand.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_BeamExpander_Zoom_InitCommand.Location = new System.Drawing.Point(380, 9);
            this.button_BeamExpander_Zoom_InitCommand.Margin = new System.Windows.Forms.Padding(6);
            this.button_BeamExpander_Zoom_InitCommand.Name = "button_BeamExpander_Zoom_InitCommand";
            this.button_BeamExpander_Zoom_InitCommand.Size = new System.Drawing.Size(107, 34);
            this.button_BeamExpander_Zoom_InitCommand.TabIndex = 37;
            this.button_BeamExpander_Zoom_InitCommand.Text = "Zoom Init";
            this.button_BeamExpander_Zoom_InitCommand.UseVisualStyleBackColor = true;
            this.button_BeamExpander_Zoom_InitCommand.Click += new System.EventHandler(this.button_BeamExpander_Zoom_InitCommand_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button2.Location = new System.Drawing.Point(197, 115);
            this.button2.Margin = new System.Windows.Forms.Padding(6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(171, 34);
            this.button2.TabIndex = 36;
            this.button2.Text = "Get Current Position";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button_BeamExpander_Mrad_MoveCommand
            // 
            this.button_BeamExpander_Mrad_MoveCommand.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_BeamExpander_Mrad_MoveCommand.Location = new System.Drawing.Point(261, 55);
            this.button_BeamExpander_Mrad_MoveCommand.Margin = new System.Windows.Forms.Padding(6);
            this.button_BeamExpander_Mrad_MoveCommand.Name = "button_BeamExpander_Mrad_MoveCommand";
            this.button_BeamExpander_Mrad_MoveCommand.Size = new System.Drawing.Size(107, 34);
            this.button_BeamExpander_Mrad_MoveCommand.TabIndex = 35;
            this.button_BeamExpander_Mrad_MoveCommand.Text = "Mrad Move";
            this.button_BeamExpander_Mrad_MoveCommand.UseVisualStyleBackColor = true;
            this.button_BeamExpander_Mrad_MoveCommand.Click += new System.EventHandler(this.button_BeamExpander_Mrad_MoveCommand_Click);
            // 
            // textBox_BeamExpander_Mrad_Position
            // 
            this.textBox_BeamExpander_Mrad_Position.Font = new System.Drawing.Font("Tahoma", 10F);
            this.textBox_BeamExpander_Mrad_Position.Location = new System.Drawing.Point(195, 55);
            this.textBox_BeamExpander_Mrad_Position.Margin = new System.Windows.Forms.Padding(6);
            this.textBox_BeamExpander_Mrad_Position.Name = "textBox_BeamExpander_Mrad_Position";
            this.textBox_BeamExpander_Mrad_Position.Size = new System.Drawing.Size(64, 24);
            this.textBox_BeamExpander_Mrad_Position.TabIndex = 34;
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(7, 54);
            this.label9.Margin = new System.Windows.Forms.Padding(6);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(187, 24);
            this.label9.TabIndex = 33;
            this.label9.Text = "Mrad Position (float, 4 digits) :";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button_BeamExpander_Zoom_MoveCommand
            // 
            this.button_BeamExpander_Zoom_MoveCommand.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_BeamExpander_Zoom_MoveCommand.Location = new System.Drawing.Point(261, 9);
            this.button_BeamExpander_Zoom_MoveCommand.Margin = new System.Windows.Forms.Padding(6);
            this.button_BeamExpander_Zoom_MoveCommand.Name = "button_BeamExpander_Zoom_MoveCommand";
            this.button_BeamExpander_Zoom_MoveCommand.Size = new System.Drawing.Size(107, 34);
            this.button_BeamExpander_Zoom_MoveCommand.TabIndex = 32;
            this.button_BeamExpander_Zoom_MoveCommand.Text = "Zoom Move";
            this.button_BeamExpander_Zoom_MoveCommand.UseVisualStyleBackColor = true;
            this.button_BeamExpander_Zoom_MoveCommand.Click += new System.EventHandler(this.button_BeamExpander_Zoom_MoveCommand_Click);
            // 
            // textBox_BeamExpander_Zoom_Position
            // 
            this.textBox_BeamExpander_Zoom_Position.Font = new System.Drawing.Font("Tahoma", 10F);
            this.textBox_BeamExpander_Zoom_Position.Location = new System.Drawing.Point(195, 9);
            this.textBox_BeamExpander_Zoom_Position.Margin = new System.Windows.Forms.Padding(6);
            this.textBox_BeamExpander_Zoom_Position.Name = "textBox_BeamExpander_Zoom_Position";
            this.textBox_BeamExpander_Zoom_Position.Size = new System.Drawing.Size(64, 24);
            this.textBox_BeamExpander_Zoom_Position.TabIndex = 31;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(7, 8);
            this.label8.Margin = new System.Windows.Forms.Padding(6);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(187, 24);
            this.label8.TabIndex = 30;
            this.label8.Text = "Zoom Position (float, 4 digits) :";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabPage_DustCollector_Upper
            // 
            this.tabPage_DustCollector_Upper.Controls.Add(this.button3);
            this.tabPage_DustCollector_Upper.Controls.Add(this.button_Test3);
            this.tabPage_DustCollector_Upper.Controls.Add(this.button_Test2);
            this.tabPage_DustCollector_Upper.Controls.Add(this.button_TEST1);
            this.tabPage_DustCollector_Upper.Controls.Add(this.button_DustCollector_Upper_WriteCommand);
            this.tabPage_DustCollector_Upper.Controls.Add(this.textBox_DustCollector_Upper_Data);
            this.tabPage_DustCollector_Upper.Controls.Add(this.label4);
            this.tabPage_DustCollector_Upper.Controls.Add(this.textBox_DustCollector_Upper_Address);
            this.tabPage_DustCollector_Upper.Controls.Add(this.label3);
            this.tabPage_DustCollector_Upper.Location = new System.Drawing.Point(4, 67);
            this.tabPage_DustCollector_Upper.Name = "tabPage_DustCollector_Upper";
            this.tabPage_DustCollector_Upper.Size = new System.Drawing.Size(514, 168);
            this.tabPage_DustCollector_Upper.TabIndex = 2;
            this.tabPage_DustCollector_Upper.Text = "DustCollector (Upper)";
            this.tabPage_DustCollector_Upper.UseVisualStyleBackColor = true;
            // 
            // button_Test3
            // 
            this.button_Test3.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_Test3.Location = new System.Drawing.Point(449, 29);
            this.button_Test3.Margin = new System.Windows.Forms.Padding(6);
            this.button_Test3.Name = "button_Test3";
            this.button_Test3.Size = new System.Drawing.Size(58, 47);
            this.button_Test3.TabIndex = 32;
            this.button_Test3.Text = "Test3";
            this.button_Test3.UseVisualStyleBackColor = true;
            this.button_Test3.Click += new System.EventHandler(this.button_Test3_Click);
            // 
            // button_Test2
            // 
            this.button_Test2.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_Test2.Location = new System.Drawing.Point(380, 29);
            this.button_Test2.Margin = new System.Windows.Forms.Padding(6);
            this.button_Test2.Name = "button_Test2";
            this.button_Test2.Size = new System.Drawing.Size(58, 47);
            this.button_Test2.TabIndex = 31;
            this.button_Test2.Text = "Test2";
            this.button_Test2.UseVisualStyleBackColor = true;
            this.button_Test2.Click += new System.EventHandler(this.button_Test2_Click);
            // 
            // button_TEST1
            // 
            this.button_TEST1.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_TEST1.Location = new System.Drawing.Point(310, 29);
            this.button_TEST1.Margin = new System.Windows.Forms.Padding(6);
            this.button_TEST1.Name = "button_TEST1";
            this.button_TEST1.Size = new System.Drawing.Size(58, 47);
            this.button_TEST1.TabIndex = 30;
            this.button_TEST1.Text = "Test1";
            this.button_TEST1.UseVisualStyleBackColor = true;
            this.button_TEST1.Click += new System.EventHandler(this.button_TEST1_Click);
            // 
            // button_DustCollector_Upper_WriteCommand
            // 
            this.button_DustCollector_Upper_WriteCommand.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_DustCollector_Upper_WriteCommand.Location = new System.Drawing.Point(177, 82);
            this.button_DustCollector_Upper_WriteCommand.Margin = new System.Windows.Forms.Padding(6);
            this.button_DustCollector_Upper_WriteCommand.Name = "button_DustCollector_Upper_WriteCommand";
            this.button_DustCollector_Upper_WriteCommand.Size = new System.Drawing.Size(107, 47);
            this.button_DustCollector_Upper_WriteCommand.TabIndex = 29;
            this.button_DustCollector_Upper_WriteCommand.Text = "Write";
            this.button_DustCollector_Upper_WriteCommand.UseVisualStyleBackColor = true;
            this.button_DustCollector_Upper_WriteCommand.Click += new System.EventHandler(this.button_DustCollector_Upper_Write_Click);
            // 
            // textBox_DustCollector_Upper_Data
            // 
            this.textBox_DustCollector_Upper_Data.Font = new System.Drawing.Font("Tahoma", 10F);
            this.textBox_DustCollector_Upper_Data.Location = new System.Drawing.Point(177, 41);
            this.textBox_DustCollector_Upper_Data.Margin = new System.Windows.Forms.Padding(6);
            this.textBox_DustCollector_Upper_Data.Name = "textBox_DustCollector_Upper_Data";
            this.textBox_DustCollector_Upper_Data.Size = new System.Drawing.Size(107, 24);
            this.textBox_DustCollector_Upper_Data.TabIndex = 28;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(10, 40);
            this.label4.Margin = new System.Windows.Forms.Padding(6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(163, 24);
            this.label4.TabIndex = 27;
            this.label4.Text = "Data (String, 4 digits) :";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox_DustCollector_Upper_Address
            // 
            this.textBox_DustCollector_Upper_Address.Font = new System.Drawing.Font("Tahoma", 10F);
            this.textBox_DustCollector_Upper_Address.Location = new System.Drawing.Point(177, 11);
            this.textBox_DustCollector_Upper_Address.Margin = new System.Windows.Forms.Padding(6);
            this.textBox_DustCollector_Upper_Address.Name = "textBox_DustCollector_Upper_Address";
            this.textBox_DustCollector_Upper_Address.Size = new System.Drawing.Size(107, 24);
            this.textBox_DustCollector_Upper_Address.TabIndex = 26;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(10, 10);
            this.label3.Margin = new System.Windows.Forms.Padding(6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(163, 24);
            this.label3.TabIndex = 25;
            this.label3.Text = "Address (String, 4 digits) :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabPage_DustCollector_Lower
            // 
            this.tabPage_DustCollector_Lower.Controls.Add(this.button_DustCollector_Lower_WriteCommand);
            this.tabPage_DustCollector_Lower.Controls.Add(this.textBox_DustCollector_Lower_Data);
            this.tabPage_DustCollector_Lower.Controls.Add(this.label5);
            this.tabPage_DustCollector_Lower.Controls.Add(this.textBox_DustCollector_Lower_Address);
            this.tabPage_DustCollector_Lower.Controls.Add(this.label6);
            this.tabPage_DustCollector_Lower.Location = new System.Drawing.Point(4, 67);
            this.tabPage_DustCollector_Lower.Name = "tabPage_DustCollector_Lower";
            this.tabPage_DustCollector_Lower.Size = new System.Drawing.Size(514, 168);
            this.tabPage_DustCollector_Lower.TabIndex = 3;
            this.tabPage_DustCollector_Lower.Text = "DustCollector (Lower)";
            this.tabPage_DustCollector_Lower.UseVisualStyleBackColor = true;
            // 
            // button_DustCollector_Lower_WriteCommand
            // 
            this.button_DustCollector_Lower_WriteCommand.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_DustCollector_Lower_WriteCommand.Location = new System.Drawing.Point(177, 82);
            this.button_DustCollector_Lower_WriteCommand.Margin = new System.Windows.Forms.Padding(6);
            this.button_DustCollector_Lower_WriteCommand.Name = "button_DustCollector_Lower_WriteCommand";
            this.button_DustCollector_Lower_WriteCommand.Size = new System.Drawing.Size(107, 47);
            this.button_DustCollector_Lower_WriteCommand.TabIndex = 34;
            this.button_DustCollector_Lower_WriteCommand.Text = "Write";
            this.button_DustCollector_Lower_WriteCommand.UseVisualStyleBackColor = true;
            this.button_DustCollector_Lower_WriteCommand.Click += new System.EventHandler(this.button_DustCollector_Lower_WriteCommand_Click);
            // 
            // textBox_DustCollector_Lower_Data
            // 
            this.textBox_DustCollector_Lower_Data.Font = new System.Drawing.Font("Tahoma", 10F);
            this.textBox_DustCollector_Lower_Data.Location = new System.Drawing.Point(177, 41);
            this.textBox_DustCollector_Lower_Data.Margin = new System.Windows.Forms.Padding(6);
            this.textBox_DustCollector_Lower_Data.Name = "textBox_DustCollector_Lower_Data";
            this.textBox_DustCollector_Lower_Data.Size = new System.Drawing.Size(107, 24);
            this.textBox_DustCollector_Lower_Data.TabIndex = 33;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(10, 40);
            this.label5.Margin = new System.Windows.Forms.Padding(6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(163, 24);
            this.label5.TabIndex = 32;
            this.label5.Text = "Data (String, 4 digits) :";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox_DustCollector_Lower_Address
            // 
            this.textBox_DustCollector_Lower_Address.Font = new System.Drawing.Font("Tahoma", 10F);
            this.textBox_DustCollector_Lower_Address.Location = new System.Drawing.Point(177, 11);
            this.textBox_DustCollector_Lower_Address.Margin = new System.Windows.Forms.Padding(6);
            this.textBox_DustCollector_Lower_Address.Name = "textBox_DustCollector_Lower_Address";
            this.textBox_DustCollector_Lower_Address.Size = new System.Drawing.Size(107, 24);
            this.textBox_DustCollector_Lower_Address.TabIndex = 31;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(10, 10);
            this.label6.Margin = new System.Windows.Forms.Padding(6);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(163, 24);
            this.label6.TabIndex = 30;
            this.label6.Text = "Address (String, 4 digits) :";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabPage_ElectroPneumaticRegulator
            // 
            this.tabPage_ElectroPneumaticRegulator.Controls.Add(this.button_ElectroPneumaticRegulator_SetPressure_Read);
            this.tabPage_ElectroPneumaticRegulator.Controls.Add(this.button_ElectroPneumaticRegulator_GetPressure);
            this.tabPage_ElectroPneumaticRegulator.Controls.Add(this.button_ElectroPneumaticRegulator_Pressure_Dec);
            this.tabPage_ElectroPneumaticRegulator.Controls.Add(this.button_ElectroPneumaticRegulator_Pressure_Inc);
            this.tabPage_ElectroPneumaticRegulator.Controls.Add(this.button_ElectroPneumaticRegulator_SetValue);
            this.tabPage_ElectroPneumaticRegulator.Controls.Add(this.textBox_ElectroPneumaticRegulator_SetValue);
            this.tabPage_ElectroPneumaticRegulator.Controls.Add(this.label7);
            this.tabPage_ElectroPneumaticRegulator.Location = new System.Drawing.Point(4, 67);
            this.tabPage_ElectroPneumaticRegulator.Name = "tabPage_ElectroPneumaticRegulator";
            this.tabPage_ElectroPneumaticRegulator.Size = new System.Drawing.Size(514, 168);
            this.tabPage_ElectroPneumaticRegulator.TabIndex = 4;
            this.tabPage_ElectroPneumaticRegulator.Text = "Electro Pneumatic Regulator";
            this.tabPage_ElectroPneumaticRegulator.UseVisualStyleBackColor = true;
            // 
            // button_ElectroPneumaticRegulator_SetPressure_Read
            // 
            this.button_ElectroPneumaticRegulator_SetPressure_Read.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_ElectroPneumaticRegulator_SetPressure_Read.Location = new System.Drawing.Point(152, 126);
            this.button_ElectroPneumaticRegulator_SetPressure_Read.Margin = new System.Windows.Forms.Padding(6);
            this.button_ElectroPneumaticRegulator_SetPressure_Read.Name = "button_ElectroPneumaticRegulator_SetPressure_Read";
            this.button_ElectroPneumaticRegulator_SetPressure_Read.Size = new System.Drawing.Size(151, 32);
            this.button_ElectroPneumaticRegulator_SetPressure_Read.TabIndex = 41;
            this.button_ElectroPneumaticRegulator_SetPressure_Read.Text = "Set Pressure Read";
            this.button_ElectroPneumaticRegulator_SetPressure_Read.UseVisualStyleBackColor = true;
            this.button_ElectroPneumaticRegulator_SetPressure_Read.Click += new System.EventHandler(this.button_ElectroPneumaticRegulator_SetPressure_Read_Click);
            // 
            // button_ElectroPneumaticRegulator_GetPressure
            // 
            this.button_ElectroPneumaticRegulator_GetPressure.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_ElectroPneumaticRegulator_GetPressure.Location = new System.Drawing.Point(152, 91);
            this.button_ElectroPneumaticRegulator_GetPressure.Margin = new System.Windows.Forms.Padding(6);
            this.button_ElectroPneumaticRegulator_GetPressure.Name = "button_ElectroPneumaticRegulator_GetPressure";
            this.button_ElectroPneumaticRegulator_GetPressure.Size = new System.Drawing.Size(151, 32);
            this.button_ElectroPneumaticRegulator_GetPressure.TabIndex = 40;
            this.button_ElectroPneumaticRegulator_GetPressure.Text = "Get Pressure";
            this.button_ElectroPneumaticRegulator_GetPressure.UseVisualStyleBackColor = true;
            this.button_ElectroPneumaticRegulator_GetPressure.Click += new System.EventHandler(this.button_ElectroPneumaticRegulator_GetPressure_Click);
            // 
            // button_ElectroPneumaticRegulator_Pressure_Dec
            // 
            this.button_ElectroPneumaticRegulator_Pressure_Dec.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_ElectroPneumaticRegulator_Pressure_Dec.Location = new System.Drawing.Point(8, 126);
            this.button_ElectroPneumaticRegulator_Pressure_Dec.Margin = new System.Windows.Forms.Padding(6);
            this.button_ElectroPneumaticRegulator_Pressure_Dec.Name = "button_ElectroPneumaticRegulator_Pressure_Dec";
            this.button_ElectroPneumaticRegulator_Pressure_Dec.Size = new System.Drawing.Size(126, 32);
            this.button_ElectroPneumaticRegulator_Pressure_Dec.TabIndex = 39;
            this.button_ElectroPneumaticRegulator_Pressure_Dec.Text = "Dec. Pressure";
            this.button_ElectroPneumaticRegulator_Pressure_Dec.UseVisualStyleBackColor = true;
            this.button_ElectroPneumaticRegulator_Pressure_Dec.Click += new System.EventHandler(this.button_ElectroPneumaticRegulator_Pressure_Dec_Click);
            // 
            // button_ElectroPneumaticRegulator_Pressure_Inc
            // 
            this.button_ElectroPneumaticRegulator_Pressure_Inc.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_ElectroPneumaticRegulator_Pressure_Inc.Location = new System.Drawing.Point(8, 91);
            this.button_ElectroPneumaticRegulator_Pressure_Inc.Margin = new System.Windows.Forms.Padding(6);
            this.button_ElectroPneumaticRegulator_Pressure_Inc.Name = "button_ElectroPneumaticRegulator_Pressure_Inc";
            this.button_ElectroPneumaticRegulator_Pressure_Inc.Size = new System.Drawing.Size(126, 32);
            this.button_ElectroPneumaticRegulator_Pressure_Inc.TabIndex = 38;
            this.button_ElectroPneumaticRegulator_Pressure_Inc.Text = "Inc. Pressure";
            this.button_ElectroPneumaticRegulator_Pressure_Inc.UseVisualStyleBackColor = true;
            this.button_ElectroPneumaticRegulator_Pressure_Inc.Click += new System.EventHandler(this.button_ElectroPneumaticRegulator_Pressure_Inc_Click);
            // 
            // button_ElectroPneumaticRegulator_SetValue
            // 
            this.button_ElectroPneumaticRegulator_SetValue.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_ElectroPneumaticRegulator_SetValue.Location = new System.Drawing.Point(339, 38);
            this.button_ElectroPneumaticRegulator_SetValue.Margin = new System.Windows.Forms.Padding(6);
            this.button_ElectroPneumaticRegulator_SetValue.Name = "button_ElectroPneumaticRegulator_SetValue";
            this.button_ElectroPneumaticRegulator_SetValue.Size = new System.Drawing.Size(107, 47);
            this.button_ElectroPneumaticRegulator_SetValue.TabIndex = 37;
            this.button_ElectroPneumaticRegulator_SetValue.Text = "Set Pressure";
            this.button_ElectroPneumaticRegulator_SetValue.UseVisualStyleBackColor = true;
            this.button_ElectroPneumaticRegulator_SetValue.Click += new System.EventHandler(this.button_ElectroPneumaticRegulator_SetValue_Click);
            // 
            // textBox_ElectroPneumaticRegulator_SetValue
            // 
            this.textBox_ElectroPneumaticRegulator_SetValue.Font = new System.Drawing.Font("Tahoma", 10F);
            this.textBox_ElectroPneumaticRegulator_SetValue.Location = new System.Drawing.Point(339, 11);
            this.textBox_ElectroPneumaticRegulator_SetValue.Margin = new System.Windows.Forms.Padding(6);
            this.textBox_ElectroPneumaticRegulator_SetValue.Name = "textBox_ElectroPneumaticRegulator_SetValue";
            this.textBox_ElectroPneumaticRegulator_SetValue.Size = new System.Drawing.Size(107, 24);
            this.textBox_ElectroPneumaticRegulator_SetValue.TabIndex = 36;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(10, 10);
            this.label7.Margin = new System.Windows.Forms.Padding(6);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(323, 24);
            this.label7.TabIndex = 35;
            this.label7.Text = "Set Pressure (Available Range : -1.3kPa ~ -80.0kPa) :";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabPage_Laser
            // 
            this.tabPage_Laser.Location = new System.Drawing.Point(4, 67);
            this.tabPage_Laser.Name = "tabPage_Laser";
            this.tabPage_Laser.Size = new System.Drawing.Size(514, 168);
            this.tabPage_Laser.TabIndex = 7;
            this.tabPage_Laser.Text = "Laser";
            this.tabPage_Laser.UseVisualStyleBackColor = true;
            // 
            // tabPage_LaserHeightSensor
            // 
            this.tabPage_LaserHeightSensor.Controls.Add(this.button_LaserHeightSensor_ReadValue);
            this.tabPage_LaserHeightSensor.Controls.Add(this.button_LaserHeightSensor_MeasurementMode);
            this.tabPage_LaserHeightSensor.Controls.Add(this.button_LaserHeightSensor_SettingMode);
            this.tabPage_LaserHeightSensor.Location = new System.Drawing.Point(4, 67);
            this.tabPage_LaserHeightSensor.Name = "tabPage_LaserHeightSensor";
            this.tabPage_LaserHeightSensor.Size = new System.Drawing.Size(514, 168);
            this.tabPage_LaserHeightSensor.TabIndex = 8;
            this.tabPage_LaserHeightSensor.Text = "Laser Height Sensor";
            this.tabPage_LaserHeightSensor.UseVisualStyleBackColor = true;
            // 
            // button_LaserHeightSensor_ReadValue
            // 
            this.button_LaserHeightSensor_ReadValue.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_LaserHeightSensor_ReadValue.Location = new System.Drawing.Point(323, 16);
            this.button_LaserHeightSensor_ReadValue.Margin = new System.Windows.Forms.Padding(6);
            this.button_LaserHeightSensor_ReadValue.Name = "button_LaserHeightSensor_ReadValue";
            this.button_LaserHeightSensor_ReadValue.Size = new System.Drawing.Size(167, 48);
            this.button_LaserHeightSensor_ReadValue.TabIndex = 41;
            this.button_LaserHeightSensor_ReadValue.Text = "Read Value";
            this.button_LaserHeightSensor_ReadValue.UseVisualStyleBackColor = true;
            this.button_LaserHeightSensor_ReadValue.Click += new System.EventHandler(this.button_LaserHeightSensor_ReadValue_Click);
            // 
            // button_LaserHeightSensor_MeasurementMode
            // 
            this.button_LaserHeightSensor_MeasurementMode.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_LaserHeightSensor_MeasurementMode.Location = new System.Drawing.Point(144, 16);
            this.button_LaserHeightSensor_MeasurementMode.Margin = new System.Windows.Forms.Padding(6);
            this.button_LaserHeightSensor_MeasurementMode.Name = "button_LaserHeightSensor_MeasurementMode";
            this.button_LaserHeightSensor_MeasurementMode.Size = new System.Drawing.Size(167, 48);
            this.button_LaserHeightSensor_MeasurementMode.TabIndex = 40;
            this.button_LaserHeightSensor_MeasurementMode.Text = "Measurement Mode";
            this.button_LaserHeightSensor_MeasurementMode.UseVisualStyleBackColor = true;
            this.button_LaserHeightSensor_MeasurementMode.Click += new System.EventHandler(this.button_LaserHeightSensor_MeasurementMode_Click);
            // 
            // button_LaserHeightSensor_SettingMode
            // 
            this.button_LaserHeightSensor_SettingMode.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_LaserHeightSensor_SettingMode.Location = new System.Drawing.Point(6, 16);
            this.button_LaserHeightSensor_SettingMode.Margin = new System.Windows.Forms.Padding(6);
            this.button_LaserHeightSensor_SettingMode.Name = "button_LaserHeightSensor_SettingMode";
            this.button_LaserHeightSensor_SettingMode.Size = new System.Drawing.Size(126, 48);
            this.button_LaserHeightSensor_SettingMode.TabIndex = 39;
            this.button_LaserHeightSensor_SettingMode.Text = "Setting Mode";
            this.button_LaserHeightSensor_SettingMode.UseVisualStyleBackColor = true;
            this.button_LaserHeightSensor_SettingMode.Click += new System.EventHandler(this.button_LaserHeightSensor_SettingMode_Click);
            // 
            // baseLabel1Value
            // 
            this.baseLabel1Value.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabel1Value.ForeColor = System.Drawing.Color.Black;
            this.baseLabel1Value.Location = new System.Drawing.Point(28, 80);
            this.baseLabel1Value.Name = "baseLabel1Value";
            this.baseLabel1Value.Size = new System.Drawing.Size(223, 23);
            this.baseLabel1Value.TabIndex = 9;
            this.baseLabel1Value.Text = "0";
            this.baseLabel1Value.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabelMax
            // 
            this.baseLabelMax.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.baseLabelMax.ForeColor = System.Drawing.Color.Black;
            this.baseLabelMax.Location = new System.Drawing.Point(212, 134);
            this.baseLabelMax.Name = "baseLabelMax";
            this.baseLabelMax.Size = new System.Drawing.Size(55, 23);
            this.baseLabelMax.TabIndex = 12;
            this.baseLabelMax.Text = "255";
            this.baseLabelMax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // baseLabelMin
            // 
            this.baseLabelMin.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.baseLabelMin.ForeColor = System.Drawing.Color.Black;
            this.baseLabelMin.Location = new System.Drawing.Point(18, 136);
            this.baseLabelMin.Name = "baseLabelMin";
            this.baseLabelMin.Size = new System.Drawing.Size(22, 23);
            this.baseLabelMin.TabIndex = 11;
            this.baseLabelMin.Text = "0";
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button3.Location = new System.Drawing.Point(380, 88);
            this.button3.Margin = new System.Windows.Forms.Padding(6);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(58, 47);
            this.button3.TabIndex = 33;
            this.button3.Text = "Test3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // FormNew_CommunicationTerminal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 650);
            this.Controls.Add(this.tabControl_CommTestFunction);
            this.Controls.Add(this.button_CommTerminal_Disconnect);
            this.Controls.Add(this.button_CommTerminal_Connect);
            this.Controls.Add(this.label_CommTerminal_LinkStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label_Activated_Unit);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox28);
            this.Controls.Add(this.label127);
            this.Controls.Add(this.label_CommunicationTerminal_ReceivedData);
            this.Controls.Add(this.label52);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormNew_CommunicationTerminal";
            this.Text = "QMC Communication Terminal";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormNew_CommunicationTerminal_FormClosing);
            this.Shown += new System.EventHandler(this.FormNew_CommunicationTerminal_Shown);
            this.tabControl_CommTestFunction.ResumeLayout(false);
            this.tabPage_Illuminator.ResumeLayout(false);
            this.tabPage_Illuminator.PerformLayout();
            this.tabPage_PowerMeter_BDS.ResumeLayout(false);
            this.tabPage_PowerMeter_Stage.ResumeLayout(false);
            this.tabPage_MotorizedBeamExpander.ResumeLayout(false);
            this.tabPage_MotorizedBeamExpander.PerformLayout();
            this.tabPage_DustCollector_Upper.ResumeLayout(false);
            this.tabPage_DustCollector_Upper.PerformLayout();
            this.tabPage_DustCollector_Lower.ResumeLayout(false);
            this.tabPage_DustCollector_Lower.PerformLayout();
            this.tabPage_ElectroPneumaticRegulator.ResumeLayout(false);
            this.tabPage_ElectroPneumaticRegulator.PerformLayout();
            this.tabPage_LaserHeightSensor.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label_CommunicationTerminal_ReceivedData;
        private Label label52;
        private TextBox textBox28;
        private Label label127;
        private Button button1;
        private Label label1;
        private Label label_Activated_Unit;
        private Label label2;
        private Label label_CommTerminal_LinkStatus;
        private Button button_CommTerminal_Connect;
        private Button button_CommTerminal_Disconnect;
        private TabControl tabControl_CommTestFunction;
        private TabPage tabPage_Illuminator;
        private TabPage tabPage_MotorizedBeamExpander;
        private TabPage tabPage_DustCollector_Upper;
        private TabPage tabPage_DustCollector_Lower;
        private TabPage tabPage_ElectroPneumaticRegulator;
        private RadioButton radioButton_IlluminatorChannel_1;
        private RadioButton radioButton_IlluminatorChannel_0;
        private BaseLabel baseLabel1Value;
        private BaseLabel baseLabelMax;
        private BaseLabel baseLabelMin;
        private HScrollBar hScrollBarIlluminator;
        private TabPage tabPage_PowerMeter_BDS;
        private TabPage tabPage_PowerMeter_Stage;
        private Button button_PowerMeter_BDS_ContinuousReading;
        private Button button_PowerMeter_BDS_ReadOnce;
        private Button button_PowerMeter_Stage_ContinuousReading;
        private Button button_PowerMeter_Stage_ReadOnce;
        private TextBox textBox_DustCollector_Upper_Address;
        private Label label3;
        private Button button_DustCollector_Upper_WriteCommand;
        private TextBox textBox_DustCollector_Upper_Data;
        private Label label4;
        private Button button_DustCollector_Lower_WriteCommand;
        private TextBox textBox_DustCollector_Lower_Data;
        private Label label5;
        private TextBox textBox_DustCollector_Lower_Address;
        private Label label6;
        private Button button_ElectroPneumaticRegulator_SetValue;
        private TextBox textBox_ElectroPneumaticRegulator_SetValue;
        private Label label7;
        private Button button_ElectroPneumaticRegulator_Pressure_Dec;
        private Button button_ElectroPneumaticRegulator_Pressure_Inc;
        private Button button_ElectroPneumaticRegulator_GetPressure;
        private Button button_ElectroPneumaticRegulator_SetPressure_Read;
        private TabPage tabPage_Laser;
        private TabPage tabPage_LaserHeightSensor;
        private Button button_LaserHeightSensor_SettingMode;
        private Button button_LaserHeightSensor_MeasurementMode;
        private Button button_LaserHeightSensor_ReadValue;
        private Button button_BeamExpander_Mrad_MoveCommand;
        private TextBox textBox_BeamExpander_Mrad_Position;
        private Label label9;
        private Button button_BeamExpander_Zoom_MoveCommand;
        private TextBox textBox_BeamExpander_Zoom_Position;
        private Label label8;
        private Button button2;
        private Button button_BeamExpander_Mrad_InitCommand;
        private Button button_BeamExpander_Zoom_InitCommand;
        private Button button_Test2;
        private Button button_TEST1;
        private Button button_Test3;
        private Button button3;
    }
}