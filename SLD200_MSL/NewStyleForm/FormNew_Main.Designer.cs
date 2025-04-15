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
            this.SiriusViewer_Main = new SpiralLab.Sirius.SiriusViewerForm();
            this.groupBoxMain_VisionImage = new System.Windows.Forms.GroupBox();
            this.groupBoxMain_ProcessingStatus = new System.Windows.Forms.GroupBox();
            this.baseGroupBox_Progress = new SLD200_MSL.WATGroupBox();
            this.baseTextBox2 = new SLD200_MSL.BaseTextBox();
            this.baseTextBox_TotalSocketCount = new SLD200_MSL.BaseTextBox();
            this.baseLabel_SocketCount = new SLD200_MSL.BaseLabel();
            this.numericUpDown_Module_TargetCount = new System.Windows.Forms.NumericUpDown();
            this.baseLabel_ModuleCount_Target = new SLD200_MSL.BaseLabel();
            this.button_PNLCount_Clear = new System.Windows.Forms.Button();
            this.baseLabel_PNLCount_NG = new SLD200_MSL.BaseLabel();
            this.baseTextBox1 = new SLD200_MSL.BaseTextBox();
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
            this.groupBoxMain_MaterialInformation = new System.Windows.Forms.GroupBox();
            this.baseTextBox_Socket_Index = new SLD200_MSL.BaseTextBox();
            this.baseTextBox_SocketCountPerModule = new SLD200_MSL.BaseTextBox();
            this.baseLabel_SocketPerModule = new SLD200_MSL.BaseLabel();
            this.baseLabel1 = new SLD200_MSL.BaseLabel();
            this.button_Main_Stop = new System.Windows.Forms.Button();
            this.button_Main_Pause = new System.Windows.Forms.Button();
            this.button_Main_Start = new System.Windows.Forms.Button();
            this.groupBox_ProcessingData = new System.Windows.Forms.GroupBox();
            this.button_TEST_RTCInit = new System.Windows.Forms.Button();
            this.button_TEST_RotOffset = new System.Windows.Forms.Button();
            this.button_Main_RecipeOpen = new System.Windows.Forms.Button();
            this.checkBox_Main_CycleStop = new System.Windows.Forms.CheckBox();
            this.button_Main_Reset = new System.Windows.Forms.Button();
            this.button_Main_Home = new System.Windows.Forms.Button();
            this.button_Main_RtcInit = new System.Windows.Forms.Button();
            this.button_Main_AutoRun = new System.Windows.Forms.Button();
            this.checkBox_Test_DryRun = new System.Windows.Forms.CheckBox();
            this.checkBox_Test_SocketAlign_UserOffset = new System.Windows.Forms.CheckBox();
            this.baseTextBox_Test_SocketAlign_OffsetX = new SLD200_MSL.BaseTextBox();
            this.baseLabel2 = new SLD200_MSL.BaseLabel();
            this.baseTextBox_Test_SocketAlign_OffsetY = new SLD200_MSL.BaseTextBox();
            this.baseLabel3 = new SLD200_MSL.BaseLabel();
            this.baseTextBox_Test_SocketAlign_Theta = new SLD200_MSL.BaseTextBox();
            this.baseLabel4 = new SLD200_MSL.BaseLabel();
            this.groupBoxMain_ProcessingStatus.SuspendLayout();
            this.baseGroupBox_Progress.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Module_TargetCount)).BeginInit();
            this.baseGroupBox_WorkingTime.SuspendLayout();
            this.groupBoxMain_MaterialInformation.SuspendLayout();
            this.groupBox_ProcessingData.SuspendLayout();
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
            this.SiriusViewer_Main.Size = new System.Drawing.Size(827, 538);
            this.SiriusViewer_Main.TabIndex = 34;
            // 
            // groupBoxMain_VisionImage
            // 
            this.groupBoxMain_VisionImage.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxMain_VisionImage.Location = new System.Drawing.Point(12, 9);
            this.groupBoxMain_VisionImage.Name = "groupBoxMain_VisionImage";
            this.groupBoxMain_VisionImage.Size = new System.Drawing.Size(388, 250);
            this.groupBoxMain_VisionImage.TabIndex = 0;
            this.groupBoxMain_VisionImage.TabStop = false;
            this.groupBoxMain_VisionImage.Text = " Vision Image ";
            // 
            // groupBoxMain_ProcessingStatus
            // 
            this.groupBoxMain_ProcessingStatus.Controls.Add(this.baseGroupBox_Progress);
            this.groupBoxMain_ProcessingStatus.Controls.Add(this.baseGroupBox_WorkingTime);
            this.groupBoxMain_ProcessingStatus.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxMain_ProcessingStatus.Location = new System.Drawing.Point(424, 9);
            this.groupBoxMain_ProcessingStatus.Name = "groupBoxMain_ProcessingStatus";
            this.groupBoxMain_ProcessingStatus.Size = new System.Drawing.Size(900, 250);
            this.groupBoxMain_ProcessingStatus.TabIndex = 1;
            this.groupBoxMain_ProcessingStatus.TabStop = false;
            this.groupBoxMain_ProcessingStatus.Text = " Processing Status ";
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
            this.baseGroupBox_Progress.Controls.Add(this.baseTextBox1);
            this.baseGroupBox_Progress.Controls.Add(this.baseTextBox_Module_TotalCount);
            this.baseGroupBox_Progress.Controls.Add(this.baseLabel_ModuleCount);
            this.baseGroupBox_Progress.Controls.Add(this.baseLabel_PNLCount_Total);
            this.baseGroupBox_Progress.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_Progress.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox_Progress.Location = new System.Drawing.Point(459, 33);
            this.baseGroupBox_Progress.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseGroupBox_Progress.Name = "baseGroupBox_Progress";
            this.baseGroupBox_Progress.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseGroupBox_Progress.Size = new System.Drawing.Size(426, 139);
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
            this.baseTextBox2.Location = new System.Drawing.Point(277, 101);
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
            this.baseTextBox_TotalSocketCount.Location = new System.Drawing.Point(196, 101);
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
            this.baseLabel_SocketCount.Location = new System.Drawing.Point(10, 106);
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
            this.numericUpDown_Module_TargetCount.Location = new System.Drawing.Point(75, 60);
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
            this.baseLabel_ModuleCount_Target.Location = new System.Drawing.Point(101, 35);
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
            this.button_PNLCount_Clear.Location = new System.Drawing.Point(359, 59);
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
            this.baseLabel_PNLCount_NG.Location = new System.Drawing.Point(302, 35);
            this.baseLabel_PNLCount_NG.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_PNLCount_NG.Name = "baseLabel_PNLCount_NG";
            this.baseLabel_PNLCount_NG.Size = new System.Drawing.Size(23, 16);
            this.baseLabel_PNLCount_NG.TabIndex = 121;
            this.baseLabel_PNLCount_NG.Text = "NG";
            // 
            // baseTextBox1
            // 
            this.baseTextBox1.BackColor = System.Drawing.Color.White;
            this.baseTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseTextBox1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseTextBox1.ForeColor = System.Drawing.Color.Black;
            this.baseTextBox1.Location = new System.Drawing.Point(277, 60);
            this.baseTextBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseTextBox1.Name = "baseTextBox1";
            this.baseTextBox1.ReadOnly = true;
            this.baseTextBox1.Size = new System.Drawing.Size(77, 26);
            this.baseTextBox1.TabIndex = 120;
            this.baseTextBox1.Text = "0";
            this.baseTextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseTextBox_Module_TotalCount
            // 
            this.baseTextBox_Module_TotalCount.BackColor = System.Drawing.Color.White;
            this.baseTextBox_Module_TotalCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseTextBox_Module_TotalCount.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseTextBox_Module_TotalCount.ForeColor = System.Drawing.Color.Black;
            this.baseTextBox_Module_TotalCount.Location = new System.Drawing.Point(196, 60);
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
            this.baseLabel_ModuleCount.Location = new System.Drawing.Point(10, 66);
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
            this.baseLabel_PNLCount_Total.Location = new System.Drawing.Point(216, 35);
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
            // groupBoxMain_MaterialInformation
            // 
            this.groupBoxMain_MaterialInformation.Controls.Add(this.baseTextBox_Socket_Index);
            this.groupBoxMain_MaterialInformation.Controls.Add(this.baseTextBox_SocketCountPerModule);
            this.groupBoxMain_MaterialInformation.Controls.Add(this.baseLabel_SocketPerModule);
            this.groupBoxMain_MaterialInformation.Controls.Add(this.baseLabel1);
            this.groupBoxMain_MaterialInformation.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxMain_MaterialInformation.Location = new System.Drawing.Point(1348, 9);
            this.groupBoxMain_MaterialInformation.Name = "groupBoxMain_MaterialInformation";
            this.groupBoxMain_MaterialInformation.Size = new System.Drawing.Size(323, 186);
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
            this.baseTextBox_Socket_Index.Location = new System.Drawing.Point(208, 86);
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
            this.baseLabel1.Location = new System.Drawing.Point(15, 85);
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
            this.button_Main_Stop.Location = new System.Drawing.Point(1733, 740);
            this.button_Main_Stop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_Main_Stop.Name = "button_Main_Stop";
            this.button_Main_Stop.Size = new System.Drawing.Size(165, 78);
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
            this.button_Main_Pause.Location = new System.Drawing.Point(1733, 554);
            this.button_Main_Pause.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_Main_Pause.Name = "button_Main_Pause";
            this.button_Main_Pause.Size = new System.Drawing.Size(165, 78);
            this.button_Main_Pause.TabIndex = 21;
            this.button_Main_Pause.Text = "Pause";
            this.button_Main_Pause.UseVisualStyleBackColor = false;
            // 
            // button_Main_Start
            // 
            this.button_Main_Start.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_Main_Start.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.button_Main_Start.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_Main_Start.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.button_Main_Start.Location = new System.Drawing.Point(1733, 368);
            this.button_Main_Start.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_Main_Start.Name = "button_Main_Start";
            this.button_Main_Start.Size = new System.Drawing.Size(165, 78);
            this.button_Main_Start.TabIndex = 20;
            this.button_Main_Start.Text = "Start";
            this.button_Main_Start.UseVisualStyleBackColor = false;
            this.button_Main_Start.Click += new System.EventHandler(this.button_Main_Start_Click);
            // 
            // groupBox_ProcessingData
            // 
            this.groupBox_ProcessingData.Controls.Add(this.button_TEST_RTCInit);
            this.groupBox_ProcessingData.Controls.Add(this.button_TEST_RotOffset);
            this.groupBox_ProcessingData.Controls.Add(this.button_Main_RecipeOpen);
            this.groupBox_ProcessingData.Controls.Add(this.SiriusViewer_Main);
            this.groupBox_ProcessingData.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_ProcessingData.Location = new System.Drawing.Point(12, 285);
            this.groupBox_ProcessingData.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox_ProcessingData.Name = "groupBox_ProcessingData";
            this.groupBox_ProcessingData.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox_ProcessingData.Size = new System.Drawing.Size(1041, 581);
            this.groupBox_ProcessingData.TabIndex = 19;
            this.groupBox_ProcessingData.TabStop = false;
            this.groupBox_ProcessingData.Text = "Processing Data";
            // 
            // button_TEST_RTCInit
            // 
            this.button_TEST_RTCInit.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_TEST_RTCInit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.button_TEST_RTCInit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_TEST_RTCInit.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.button_TEST_RTCInit.Location = new System.Drawing.Point(860, 492);
            this.button_TEST_RTCInit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_TEST_RTCInit.Name = "button_TEST_RTCInit";
            this.button_TEST_RTCInit.Size = new System.Drawing.Size(167, 78);
            this.button_TEST_RTCInit.TabIndex = 59;
            this.button_TEST_RTCInit.Text = "테스트 : RTC Init";
            this.button_TEST_RTCInit.UseVisualStyleBackColor = false;
            this.button_TEST_RTCInit.Click += new System.EventHandler(this.button_TEST_RTCInit_Click);
            // 
            // button_TEST_RotOffset
            // 
            this.button_TEST_RotOffset.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_TEST_RotOffset.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.button_TEST_RotOffset.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_TEST_RotOffset.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.button_TEST_RotOffset.Location = new System.Drawing.Point(860, 379);
            this.button_TEST_RotOffset.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_TEST_RotOffset.Name = "button_TEST_RotOffset";
            this.button_TEST_RotOffset.Size = new System.Drawing.Size(167, 78);
            this.button_TEST_RotOffset.TabIndex = 58;
            this.button_TEST_RotOffset.Text = "테스트 : Rot, Offset 이동";
            this.button_TEST_RotOffset.UseVisualStyleBackColor = false;
            this.button_TEST_RotOffset.Click += new System.EventHandler(this.button_TEST_RotOffset_Click);
            // 
            // button_Main_RecipeOpen
            // 
            this.button_Main_RecipeOpen.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_Main_RecipeOpen.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.button_Main_RecipeOpen.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_Main_RecipeOpen.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.button_Main_RecipeOpen.Location = new System.Drawing.Point(860, 30);
            this.button_Main_RecipeOpen.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_Main_RecipeOpen.Name = "button_Main_RecipeOpen";
            this.button_Main_RecipeOpen.Size = new System.Drawing.Size(167, 78);
            this.button_Main_RecipeOpen.TabIndex = 5;
            this.button_Main_RecipeOpen.Text = "Recipe Open";
            this.button_Main_RecipeOpen.UseVisualStyleBackColor = false;
            this.button_Main_RecipeOpen.Click += new System.EventHandler(this.button_Main_RecipeOpen_Click);
            // 
            // checkBox_Main_CycleStop
            // 
            this.checkBox_Main_CycleStop.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.checkBox_Main_CycleStop.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.checkBox_Main_CycleStop.Location = new System.Drawing.Point(1733, 647);
            this.checkBox_Main_CycleStop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox_Main_CycleStop.Name = "checkBox_Main_CycleStop";
            this.checkBox_Main_CycleStop.Size = new System.Drawing.Size(165, 78);
            this.checkBox_Main_CycleStop.TabIndex = 55;
            this.checkBox_Main_CycleStop.Text = "Cycle Stop";
            this.checkBox_Main_CycleStop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox_Main_CycleStop.UseVisualStyleBackColor = false;
            // 
            // button_Main_Reset
            // 
            this.button_Main_Reset.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_Main_Reset.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.button_Main_Reset.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_Main_Reset.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.button_Main_Reset.Location = new System.Drawing.Point(1733, 461);
            this.button_Main_Reset.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_Main_Reset.Name = "button_Main_Reset";
            this.button_Main_Reset.Size = new System.Drawing.Size(165, 78);
            this.button_Main_Reset.TabIndex = 54;
            this.button_Main_Reset.Text = "Reset";
            this.button_Main_Reset.UseVisualStyleBackColor = false;
            // 
            // button_Main_Home
            // 
            this.button_Main_Home.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_Main_Home.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.button_Main_Home.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_Main_Home.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.button_Main_Home.Location = new System.Drawing.Point(1709, 15);
            this.button_Main_Home.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_Main_Home.Name = "button_Main_Home";
            this.button_Main_Home.Size = new System.Drawing.Size(189, 78);
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
            this.button_Main_RtcInit.Location = new System.Drawing.Point(1709, 113);
            this.button_Main_RtcInit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_Main_RtcInit.Name = "button_Main_RtcInit";
            this.button_Main_RtcInit.Size = new System.Drawing.Size(189, 78);
            this.button_Main_RtcInit.TabIndex = 57;
            this.button_Main_RtcInit.Text = "Scanner Board\r\nInitialize";
            this.button_Main_RtcInit.UseVisualStyleBackColor = false;
            this.button_Main_RtcInit.Click += new System.EventHandler(this.button_Main_RtcInit_Click);
            // 
            // button_Main_AutoRun
            // 
            this.button_Main_AutoRun.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_Main_AutoRun.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.button_Main_AutoRun.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_Main_AutoRun.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.button_Main_AutoRun.Location = new System.Drawing.Point(1733, 254);
            this.button_Main_AutoRun.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_Main_AutoRun.Name = "button_Main_AutoRun";
            this.button_Main_AutoRun.Size = new System.Drawing.Size(165, 78);
            this.button_Main_AutoRun.TabIndex = 58;
            this.button_Main_AutoRun.Text = "Auto Run";
            this.button_Main_AutoRun.UseVisualStyleBackColor = false;
            this.button_Main_AutoRun.Click += new System.EventHandler(this.button_Main_AutoRun_Click);
            // 
            // checkBox_Test_DryRun
            // 
            this.checkBox_Test_DryRun.AutoSize = true;
            this.checkBox_Test_DryRun.Checked = true;
            this.checkBox_Test_DryRun.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Test_DryRun.Location = new System.Drawing.Point(1733, 226);
            this.checkBox_Test_DryRun.Name = "checkBox_Test_DryRun";
            this.checkBox_Test_DryRun.Size = new System.Drawing.Size(69, 18);
            this.checkBox_Test_DryRun.TabIndex = 59;
            this.checkBox_Test_DryRun.Text = "Dry Run";
            this.checkBox_Test_DryRun.UseVisualStyleBackColor = true;
            // 
            // checkBox_Test_SocketAlign_UserOffset
            // 
            this.checkBox_Test_SocketAlign_UserOffset.AutoSize = true;
            this.checkBox_Test_SocketAlign_UserOffset.Checked = true;
            this.checkBox_Test_SocketAlign_UserOffset.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Test_SocketAlign_UserOffset.Font = new System.Drawing.Font("Tahoma", 10F);
            this.checkBox_Test_SocketAlign_UserOffset.Location = new System.Drawing.Point(1441, 368);
            this.checkBox_Test_SocketAlign_UserOffset.Name = "checkBox_Test_SocketAlign_UserOffset";
            this.checkBox_Test_SocketAlign_UserOffset.Size = new System.Drawing.Size(262, 21);
            this.checkBox_Test_SocketAlign_UserOffset.TabIndex = 136;
            this.checkBox_Test_SocketAlign_UserOffset.Text = "Test : 얼라인 결과에 Offset 이동하여 가공";
            this.checkBox_Test_SocketAlign_UserOffset.UseVisualStyleBackColor = true;
            // 
            // baseTextBox_Test_SocketAlign_OffsetX
            // 
            this.baseTextBox_Test_SocketAlign_OffsetX.BackColor = System.Drawing.Color.White;
            this.baseTextBox_Test_SocketAlign_OffsetX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseTextBox_Test_SocketAlign_OffsetX.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseTextBox_Test_SocketAlign_OffsetX.ForeColor = System.Drawing.Color.Black;
            this.baseTextBox_Test_SocketAlign_OffsetX.Location = new System.Drawing.Point(1606, 395);
            this.baseTextBox_Test_SocketAlign_OffsetX.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseTextBox_Test_SocketAlign_OffsetX.Name = "baseTextBox_Test_SocketAlign_OffsetX";
            this.baseTextBox_Test_SocketAlign_OffsetX.Size = new System.Drawing.Size(88, 26);
            this.baseTextBox_Test_SocketAlign_OffsetX.TabIndex = 142;
            this.baseTextBox_Test_SocketAlign_OffsetX.Text = "0";
            this.baseTextBox_Test_SocketAlign_OffsetX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseLabel2
            // 
            this.baseLabel2.Font = new System.Drawing.Font("Tahoma", 10F);
            this.baseLabel2.ForeColor = System.Drawing.Color.Black;
            this.baseLabel2.Location = new System.Drawing.Point(1524, 394);
            this.baseLabel2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel2.Name = "baseLabel2";
            this.baseLabel2.Size = new System.Drawing.Size(78, 26);
            this.baseLabel2.TabIndex = 141;
            this.baseLabel2.Text = "Offset X :";
            this.baseLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // baseTextBox_Test_SocketAlign_OffsetY
            // 
            this.baseTextBox_Test_SocketAlign_OffsetY.BackColor = System.Drawing.Color.White;
            this.baseTextBox_Test_SocketAlign_OffsetY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseTextBox_Test_SocketAlign_OffsetY.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseTextBox_Test_SocketAlign_OffsetY.ForeColor = System.Drawing.Color.Black;
            this.baseTextBox_Test_SocketAlign_OffsetY.Location = new System.Drawing.Point(1606, 423);
            this.baseTextBox_Test_SocketAlign_OffsetY.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseTextBox_Test_SocketAlign_OffsetY.Name = "baseTextBox_Test_SocketAlign_OffsetY";
            this.baseTextBox_Test_SocketAlign_OffsetY.Size = new System.Drawing.Size(88, 26);
            this.baseTextBox_Test_SocketAlign_OffsetY.TabIndex = 144;
            this.baseTextBox_Test_SocketAlign_OffsetY.Text = "0";
            this.baseTextBox_Test_SocketAlign_OffsetY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseLabel3
            // 
            this.baseLabel3.Font = new System.Drawing.Font("Tahoma", 10F);
            this.baseLabel3.ForeColor = System.Drawing.Color.Black;
            this.baseLabel3.Location = new System.Drawing.Point(1524, 422);
            this.baseLabel3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel3.Name = "baseLabel3";
            this.baseLabel3.Size = new System.Drawing.Size(78, 26);
            this.baseLabel3.TabIndex = 143;
            this.baseLabel3.Text = "Offset Y :";
            this.baseLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // baseTextBox_Test_SocketAlign_Theta
            // 
            this.baseTextBox_Test_SocketAlign_Theta.BackColor = System.Drawing.Color.White;
            this.baseTextBox_Test_SocketAlign_Theta.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseTextBox_Test_SocketAlign_Theta.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseTextBox_Test_SocketAlign_Theta.ForeColor = System.Drawing.Color.Black;
            this.baseTextBox_Test_SocketAlign_Theta.Location = new System.Drawing.Point(1606, 451);
            this.baseTextBox_Test_SocketAlign_Theta.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseTextBox_Test_SocketAlign_Theta.Name = "baseTextBox_Test_SocketAlign_Theta";
            this.baseTextBox_Test_SocketAlign_Theta.Size = new System.Drawing.Size(88, 26);
            this.baseTextBox_Test_SocketAlign_Theta.TabIndex = 146;
            this.baseTextBox_Test_SocketAlign_Theta.Text = "0";
            this.baseTextBox_Test_SocketAlign_Theta.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseLabel4
            // 
            this.baseLabel4.Font = new System.Drawing.Font("Tahoma", 10F);
            this.baseLabel4.ForeColor = System.Drawing.Color.Black;
            this.baseLabel4.Location = new System.Drawing.Point(1524, 450);
            this.baseLabel4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel4.Name = "baseLabel4";
            this.baseLabel4.Size = new System.Drawing.Size(78, 26);
            this.baseLabel4.TabIndex = 145;
            this.baseLabel4.Text = "Theta :";
            this.baseLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FormNew_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1920, 877);
            this.ControlBox = false;
            this.Controls.Add(this.baseTextBox_Test_SocketAlign_Theta);
            this.Controls.Add(this.baseLabel4);
            this.Controls.Add(this.baseTextBox_Test_SocketAlign_OffsetY);
            this.Controls.Add(this.baseLabel3);
            this.Controls.Add(this.baseTextBox_Test_SocketAlign_OffsetX);
            this.Controls.Add(this.baseLabel2);
            this.Controls.Add(this.checkBox_Test_SocketAlign_UserOffset);
            this.Controls.Add(this.checkBox_Test_DryRun);
            this.Controls.Add(this.button_Main_AutoRun);
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
            this.Controls.Add(this.groupBoxMain_VisionImage);
            this.Font = new System.Drawing.Font("Tahoma", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormNew_Main";
            this.Text = "FormNew_Main";
            this.Shown += new System.EventHandler(this.FormNew_Main_Shown);
            this.groupBoxMain_ProcessingStatus.ResumeLayout(false);
            this.baseGroupBox_Progress.ResumeLayout(false);
            this.baseGroupBox_Progress.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Module_TargetCount)).EndInit();
            this.baseGroupBox_WorkingTime.ResumeLayout(false);
            this.baseGroupBox_WorkingTime.PerformLayout();
            this.groupBoxMain_MaterialInformation.ResumeLayout(false);
            this.groupBoxMain_MaterialInformation.PerformLayout();
            this.groupBox_ProcessingData.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxMain_VisionImage;
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
        private BaseTextBox baseTextBox1;
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
        private System.Windows.Forms.Button button_TEST_RotOffset;
        private System.Windows.Forms.Button button_Main_AutoRun;
        private System.Windows.Forms.Button button_TEST_RTCInit;
        private System.Windows.Forms.CheckBox checkBox_Test_DryRun;
        private System.Windows.Forms.CheckBox checkBox_Test_SocketAlign_UserOffset;
        private BaseTextBox baseTextBox_Test_SocketAlign_OffsetX;
        private BaseLabel baseLabel2;
        private BaseTextBox baseTextBox_Test_SocketAlign_OffsetY;
        private BaseLabel baseLabel3;
        private BaseTextBox baseTextBox_Test_SocketAlign_Theta;
        private BaseLabel baseLabel4;
    }
}