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
            this.baseTextBox_SocketCountPerModule = new SLD200_MSL.BaseTextBox();
            this.baseLabel_SocketPerModule = new SLD200_MSL.BaseLabel();
            this.button10 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.SiriusViewer_Main = new SpiralLab.Sirius.SiriusViewerForm();
            this.button11 = new System.Windows.Forms.Button();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.button101 = new System.Windows.Forms.Button();
            this.groupBoxMain_ProcessingStatus.SuspendLayout();
            this.baseGroupBox_Progress.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Module_TargetCount)).BeginInit();
            this.baseGroupBox_WorkingTime.SuspendLayout();
            this.groupBoxMain_MaterialInformation.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxMain_VisionImage
            // 
            this.groupBoxMain_VisionImage.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxMain_VisionImage.Location = new System.Drawing.Point(30, 15);
            this.groupBoxMain_VisionImage.Name = "groupBoxMain_VisionImage";
            this.groupBoxMain_VisionImage.Size = new System.Drawing.Size(388, 305);
            this.groupBoxMain_VisionImage.TabIndex = 0;
            this.groupBoxMain_VisionImage.TabStop = false;
            this.groupBoxMain_VisionImage.Text = " Vision Image ";
            // 
            // groupBoxMain_ProcessingStatus
            // 
            this.groupBoxMain_ProcessingStatus.Controls.Add(this.baseGroupBox_Progress);
            this.groupBoxMain_ProcessingStatus.Controls.Add(this.baseGroupBox_WorkingTime);
            this.groupBoxMain_ProcessingStatus.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxMain_ProcessingStatus.Location = new System.Drawing.Point(468, 15);
            this.groupBoxMain_ProcessingStatus.Name = "groupBoxMain_ProcessingStatus";
            this.groupBoxMain_ProcessingStatus.Size = new System.Drawing.Size(1029, 305);
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
            this.baseGroupBox_Progress.Location = new System.Drawing.Point(479, 38);
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
            this.baseGroupBox_WorkingTime.Location = new System.Drawing.Point(17, 38);
            this.baseGroupBox_WorkingTime.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseGroupBox_WorkingTime.Name = "baseGroupBox_WorkingTime";
            this.baseGroupBox_WorkingTime.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseGroupBox_WorkingTime.Size = new System.Drawing.Size(426, 251);
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
            this.button_AverageOneCycleTime_Clear.Location = new System.Drawing.Point(348, 207);
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
            this.baseLabel_Average_OneCycleTime.Location = new System.Drawing.Point(228, 214);
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
            this.baseLabel_AverageOneCycle_Time.Location = new System.Drawing.Point(12, 214);
            this.baseLabel_AverageOneCycle_Time.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_AverageOneCycle_Time.Name = "baseLabel_AverageOneCycle_Time";
            this.baseLabel_AverageOneCycle_Time.Size = new System.Drawing.Size(171, 18);
            this.baseLabel_AverageOneCycle_Time.TabIndex = 116;
            this.baseLabel_AverageOneCycle_Time.Text = "Average One Cycle Time";
            // 
            // progressBar_TotalRemained_Time
            // 
            this.progressBar_TotalRemained_Time.Location = new System.Drawing.Point(12, 145);
            this.progressBar_TotalRemained_Time.Name = "progressBar_TotalRemained_Time";
            this.progressBar_TotalRemained_Time.Size = new System.Drawing.Size(403, 36);
            this.progressBar_TotalRemained_Time.TabIndex = 115;
            this.progressBar_TotalRemained_Time.Value = 50;
            // 
            // baseLabel_Total_RemainedTime
            // 
            this.baseLabel_Total_RemainedTime.AutoSize = true;
            this.baseLabel_Total_RemainedTime.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_Total_RemainedTime.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Total_RemainedTime.Location = new System.Drawing.Point(228, 121);
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
            this.baseLabel_TotalRunning_Time.Location = new System.Drawing.Point(12, 121);
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
            this.progressBar_OneCycle_Time.Size = new System.Drawing.Size(403, 36);
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
            this.groupBoxMain_MaterialInformation.Controls.Add(this.baseTextBox_SocketCountPerModule);
            this.groupBoxMain_MaterialInformation.Controls.Add(this.baseLabel_SocketPerModule);
            this.groupBoxMain_MaterialInformation.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxMain_MaterialInformation.Location = new System.Drawing.Point(1547, 15);
            this.groupBoxMain_MaterialInformation.Name = "groupBoxMain_MaterialInformation";
            this.groupBoxMain_MaterialInformation.Size = new System.Drawing.Size(350, 305);
            this.groupBoxMain_MaterialInformation.TabIndex = 2;
            this.groupBoxMain_MaterialInformation.TabStop = false;
            this.groupBoxMain_MaterialInformation.Text = " Material Information ";
            // 
            // baseTextBox_SocketCountPerModule
            // 
            this.baseTextBox_SocketCountPerModule.BackColor = System.Drawing.Color.White;
            this.baseTextBox_SocketCountPerModule.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseTextBox_SocketCountPerModule.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseTextBox_SocketCountPerModule.ForeColor = System.Drawing.Color.Black;
            this.baseTextBox_SocketCountPerModule.Location = new System.Drawing.Point(207, 38);
            this.baseTextBox_SocketCountPerModule.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseTextBox_SocketCountPerModule.Name = "baseTextBox_SocketCountPerModule";
            this.baseTextBox_SocketCountPerModule.ReadOnly = true;
            this.baseTextBox_SocketCountPerModule.Size = new System.Drawing.Size(127, 26);
            this.baseTextBox_SocketCountPerModule.TabIndex = 124;
            this.baseTextBox_SocketCountPerModule.Text = "0";
            this.baseTextBox_SocketCountPerModule.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseLabel_SocketPerModule
            // 
            this.baseLabel_SocketPerModule.AutoSize = true;
            this.baseLabel_SocketPerModule.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_SocketPerModule.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_SocketPerModule.Location = new System.Drawing.Point(15, 42);
            this.baseLabel_SocketPerModule.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_SocketPerModule.Name = "baseLabel_SocketPerModule";
            this.baseLabel_SocketPerModule.Size = new System.Drawing.Size(169, 18);
            this.baseLabel_SocketPerModule.TabIndex = 123;
            this.baseLabel_SocketPerModule.Text = "Socket count per Module";
            // 
            // button10
            // 
            this.button10.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button10.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.button10.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button10.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.button10.Location = new System.Drawing.Point(1733, 742);
            this.button10.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(165, 78);
            this.button10.TabIndex = 22;
            this.button10.Text = "Stop";
            this.button10.UseVisualStyleBackColor = false;
            // 
            // button9
            // 
            this.button9.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button9.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.button9.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button9.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.button9.Location = new System.Drawing.Point(1733, 546);
            this.button9.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(165, 78);
            this.button9.TabIndex = 21;
            this.button9.Text = "Pause";
            this.button9.UseVisualStyleBackColor = false;
            // 
            // button8
            // 
            this.button8.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button8.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.button8.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button8.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.button8.Location = new System.Drawing.Point(1733, 350);
            this.button8.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(165, 78);
            this.button8.TabIndex = 20;
            this.button8.Text = "Start";
            this.button8.UseVisualStyleBackColor = false;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.SiriusViewer_Main);
            this.groupBox5.Controls.Add(this.button11);
            this.groupBox5.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.Location = new System.Drawing.Point(30, 340);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox5.Size = new System.Drawing.Size(942, 519);
            this.groupBox5.TabIndex = 19;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Processing Data";
            // 
            // SiriusViewer_Main
            // 
            this.SiriusViewer_Main.AliasName = "NoName";
            this.SiriusViewer_Main.BackColor = System.Drawing.SystemColors.Control;
            this.SiriusViewer_Main.Document = null;
            this.SiriusViewer_Main.FileName = "NoName";
            this.SiriusViewer_Main.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SiriusViewer_Main.Index = ((uint)(0u));
            this.SiriusViewer_Main.Location = new System.Drawing.Point(16, 34);
            this.SiriusViewer_Main.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SiriusViewer_Main.Name = "SiriusViewer_Main";
            this.SiriusViewer_Main.Progress = 0;
            this.SiriusViewer_Main.Size = new System.Drawing.Size(771, 471);
            this.SiriusViewer_Main.TabIndex = 37;
            // 
            // button11
            // 
            this.button11.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button11.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.button11.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button11.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.button11.Location = new System.Drawing.Point(796, 32);
            this.button11.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(131, 63);
            this.button11.TabIndex = 5;
            this.button11.Text = "Open";
            this.button11.UseVisualStyleBackColor = false;
            // 
            // checkBox5
            // 
            this.checkBox5.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.checkBox5.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.checkBox5.Location = new System.Drawing.Point(1733, 644);
            this.checkBox5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(165, 78);
            this.checkBox5.TabIndex = 55;
            this.checkBox5.Text = "Cycle Stop";
            this.checkBox5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox5.UseVisualStyleBackColor = false;
            // 
            // button101
            // 
            this.button101.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button101.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.button101.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button101.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.button101.Location = new System.Drawing.Point(1733, 448);
            this.button101.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button101.Name = "button101";
            this.button101.Size = new System.Drawing.Size(165, 78);
            this.button101.TabIndex = 54;
            this.button101.Text = "Reset";
            this.button101.UseVisualStyleBackColor = false;
            // 
            // FormNew_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1920, 877);
            this.ControlBox = false;
            this.Controls.Add(this.checkBox5);
            this.Controls.Add(this.button101);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.groupBox5);
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
            this.groupBoxMain_ProcessingStatus.ResumeLayout(false);
            this.baseGroupBox_Progress.ResumeLayout(false);
            this.baseGroupBox_Progress.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Module_TargetCount)).EndInit();
            this.baseGroupBox_WorkingTime.ResumeLayout(false);
            this.baseGroupBox_WorkingTime.PerformLayout();
            this.groupBoxMain_MaterialInformation.ResumeLayout(false);
            this.groupBoxMain_MaterialInformation.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxMain_VisionImage;
        private System.Windows.Forms.GroupBox groupBoxMain_ProcessingStatus;
        private System.Windows.Forms.GroupBox groupBoxMain_MaterialInformation;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button button11;
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
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.Button button101;
    }
}