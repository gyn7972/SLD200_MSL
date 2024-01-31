namespace CWA150SA_Onsemi300
{
    partial class FormAxisMotion
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
            this.ButtonStateReset = new BaseButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TextBoxState = new System.Windows.Forms.TextBox();
            this.TextBoxAmpFault = new System.Windows.Forms.TextBox();
            this.TextBoxInPosition = new System.Windows.Forms.TextBox();
            this.TextBoxMotionDone = new System.Windows.Forms.TextBox();
            this.TextBoxAmpOn = new System.Windows.Forms.TextBox();
            this.TextBoxNegativeLimit = new System.Windows.Forms.TextBox();
            this.TextBoxStateHome = new System.Windows.Forms.TextBox();
            this.TextBoxPositiveLimit = new System.Windows.Forms.TextBox();
            this.ButtonAmpOff = new BaseButton();
            this.ButtonAmpClear = new BaseButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TextBoxSetPostion = new System.Windows.Forms.TextBox();
            this.ButtonSetPosition = new BaseButton();
            this.TextBoxPositionError = new System.Windows.Forms.TextBox();
            this.TextBoxActualPosition = new System.Windows.Forms.TextBox();
            this.TextBoxCommandPosition = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.TextBoxPreciseVelocity = new System.Windows.Forms.TextBox();
            this.ComboBoxIndexSeach = new System.Windows.Forms.CheckBox();
            this.CheckBoxPreciseVelocity = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.TextBoxHomingState = new System.Windows.Forms.TextBox();
            this.TextBoxEscapeDistance = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.TextBoxHomingVelocity = new System.Windows.Forms.TextBox();
            this.TextBoxHomingAcceleration = new System.Windows.Forms.TextBox();
            this.TextBoxHomingDeceleration = new System.Windows.Forms.TextBox();
            this.TextBoxHomingnegativeLimit = new System.Windows.Forms.TextBox();
            this.TextBoxHomingPositiveLimit = new System.Windows.Forms.TextBox();
            this.ButtonHomingStart = new BaseButton();
            this.ComboBoxHomingMethod = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.CheckBoxJogByVelocityMove = new System.Windows.Forms.CheckBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.TextBoxAccDecMove = new System.Windows.Forms.TextBox();
            this.TextBoxVelocityMove = new System.Windows.Forms.TextBox();
            this.ButtonPositive = new BaseButton();
            this.ButtonPositiveStep = new BaseButton();
            this.ButtonNegative = new BaseButton();
            this.ButtonNegativeStep = new BaseButton();
            this.ButtonRepaet = new BaseButton();
            this.ButtonMoveDistance = new BaseButton();
            this.ButtonModifyVeloity = new BaseButton();
            this.ButtonMoveVelocity = new BaseButton();
            this.ButtonModify = new BaseButton();
            this.ButtonMove = new BaseButton();
            this.TextBoxPositionMove = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.StopEmergency = new BaseButton();
            this.ButtonStop = new BaseButton();
            this.TextBoxStopDeceleration = new System.Windows.Forms.TextBox();
            this.CheckBoxStopDeceleration = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonStateReset
            // 
            this.ButtonStateReset.Location = new System.Drawing.Point(183, 20);
            this.ButtonStateReset.Name = "ButtonStateReset";
            this.ButtonStateReset.Size = new System.Drawing.Size(75, 23);
            this.ButtonStateReset.TabIndex = 0;
            this.ButtonStateReset.Text = "Reset";
            this.ButtonStateReset.UseVisualStyleBackColor = true;
            this.ButtonStateReset.Click += new System.EventHandler(this.ButtonStateReset_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.TextBoxState);
            this.groupBox1.Controls.Add(this.TextBoxAmpFault);
            this.groupBox1.Controls.Add(this.TextBoxInPosition);
            this.groupBox1.Controls.Add(this.TextBoxMotionDone);
            this.groupBox1.Controls.Add(this.TextBoxAmpOn);
            this.groupBox1.Controls.Add(this.TextBoxNegativeLimit);
            this.groupBox1.Controls.Add(this.TextBoxStateHome);
            this.groupBox1.Controls.Add(this.TextBoxPositiveLimit);
            this.groupBox1.Controls.Add(this.ButtonAmpOff);
            this.groupBox1.Controls.Add(this.ButtonAmpClear);
            this.groupBox1.Controls.Add(this.ButtonStateReset);
            this.groupBox1.Location = new System.Drawing.Point(3, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(268, 137);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Status";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "State";
            // 
            // TextBoxState
            // 
            this.TextBoxState.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxState.BackColor = System.Drawing.SystemColors.Info;
            this.TextBoxState.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxState.Location = new System.Drawing.Point(56, 20);
            this.TextBoxState.Name = "TextBoxState";
            this.TextBoxState.ReadOnly = true;
            this.TextBoxState.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TextBoxState.Size = new System.Drawing.Size(121, 21);
            this.TextBoxState.TabIndex = 5;
            this.TextBoxState.Text = "Idle";
            this.TextBoxState.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TextBoxAmpFault
            // 
            this.TextBoxAmpFault.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxAmpFault.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.TextBoxAmpFault.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxAmpFault.Location = new System.Drawing.Point(95, 105);
            this.TextBoxAmpFault.Name = "TextBoxAmpFault";
            this.TextBoxAmpFault.ReadOnly = true;
            this.TextBoxAmpFault.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TextBoxAmpFault.Size = new System.Drawing.Size(82, 21);
            this.TextBoxAmpFault.TabIndex = 5;
            this.TextBoxAmpFault.Text = "Amp Fault";
            this.TextBoxAmpFault.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TextBoxInPosition
            // 
            this.TextBoxInPosition.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxInPosition.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.TextBoxInPosition.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxInPosition.Location = new System.Drawing.Point(6, 105);
            this.TextBoxInPosition.Name = "TextBoxInPosition";
            this.TextBoxInPosition.ReadOnly = true;
            this.TextBoxInPosition.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TextBoxInPosition.Size = new System.Drawing.Size(82, 21);
            this.TextBoxInPosition.TabIndex = 8;
            this.TextBoxInPosition.Text = "InPosition";
            this.TextBoxInPosition.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TextBoxMotionDone
            // 
            this.TextBoxMotionDone.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxMotionDone.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.TextBoxMotionDone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxMotionDone.Location = new System.Drawing.Point(6, 76);
            this.TextBoxMotionDone.Name = "TextBoxMotionDone";
            this.TextBoxMotionDone.ReadOnly = true;
            this.TextBoxMotionDone.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TextBoxMotionDone.Size = new System.Drawing.Size(82, 21);
            this.TextBoxMotionDone.TabIndex = 7;
            this.TextBoxMotionDone.Text = "MotionDone";
            this.TextBoxMotionDone.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TextBoxAmpOn
            // 
            this.TextBoxAmpOn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxAmpOn.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.TextBoxAmpOn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxAmpOn.Location = new System.Drawing.Point(95, 76);
            this.TextBoxAmpOn.Name = "TextBoxAmpOn";
            this.TextBoxAmpOn.ReadOnly = true;
            this.TextBoxAmpOn.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TextBoxAmpOn.Size = new System.Drawing.Size(82, 21);
            this.TextBoxAmpOn.TabIndex = 6;
            this.TextBoxAmpOn.Text = "AmpOn";
            this.TextBoxAmpOn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TextBoxNegativeLimit
            // 
            this.TextBoxNegativeLimit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxNegativeLimit.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.TextBoxNegativeLimit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxNegativeLimit.Location = new System.Drawing.Point(6, 49);
            this.TextBoxNegativeLimit.Name = "TextBoxNegativeLimit";
            this.TextBoxNegativeLimit.ReadOnly = true;
            this.TextBoxNegativeLimit.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TextBoxNegativeLimit.Size = new System.Drawing.Size(82, 21);
            this.TextBoxNegativeLimit.TabIndex = 5;
            this.TextBoxNegativeLimit.Text = "-Limit";
            this.TextBoxNegativeLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TextBoxStateHome
            // 
            this.TextBoxStateHome.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxStateHome.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.TextBoxStateHome.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxStateHome.Location = new System.Drawing.Point(95, 49);
            this.TextBoxStateHome.Name = "TextBoxStateHome";
            this.TextBoxStateHome.ReadOnly = true;
            this.TextBoxStateHome.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TextBoxStateHome.Size = new System.Drawing.Size(82, 21);
            this.TextBoxStateHome.TabIndex = 4;
            this.TextBoxStateHome.Text = "Home";
            this.TextBoxStateHome.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TextBoxPositiveLimit
            // 
            this.TextBoxPositiveLimit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxPositiveLimit.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.TextBoxPositiveLimit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxPositiveLimit.Location = new System.Drawing.Point(183, 49);
            this.TextBoxPositiveLimit.Name = "TextBoxPositiveLimit";
            this.TextBoxPositiveLimit.ReadOnly = true;
            this.TextBoxPositiveLimit.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TextBoxPositiveLimit.Size = new System.Drawing.Size(75, 21);
            this.TextBoxPositiveLimit.TabIndex = 3;
            this.TextBoxPositiveLimit.Text = "+Limit";
            this.TextBoxPositiveLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ButtonAmpOff
            // 
            this.ButtonAmpOff.Location = new System.Drawing.Point(183, 76);
            this.ButtonAmpOff.Name = "ButtonAmpOff";
            this.ButtonAmpOff.Size = new System.Drawing.Size(75, 23);
            this.ButtonAmpOff.TabIndex = 2;
            this.ButtonAmpOff.Text = "Amp Off";
            this.ButtonAmpOff.UseVisualStyleBackColor = true;
            this.ButtonAmpOff.Click += new System.EventHandler(this.ButtonAmpOff_Click);
            // 
            // ButtonAmpClear
            // 
            this.ButtonAmpClear.Location = new System.Drawing.Point(183, 105);
            this.ButtonAmpClear.Name = "ButtonAmpClear";
            this.ButtonAmpClear.Size = new System.Drawing.Size(75, 23);
            this.ButtonAmpClear.TabIndex = 1;
            this.ButtonAmpClear.Text = "Amp Clear";
            this.ButtonAmpClear.UseVisualStyleBackColor = true;
            this.ButtonAmpClear.Click += new System.EventHandler(this.ButtonAmpClear_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.TextBoxSetPostion);
            this.groupBox2.Controls.Add(this.ButtonSetPosition);
            this.groupBox2.Controls.Add(this.TextBoxPositionError);
            this.groupBox2.Controls.Add(this.TextBoxActualPosition);
            this.groupBox2.Controls.Add(this.TextBoxCommandPosition);
            this.groupBox2.Location = new System.Drawing.Point(277, 1);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(217, 137);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Position";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 12);
            this.label4.TabIndex = 13;
            this.label4.Text = "Position Error";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 12);
            this.label3.TabIndex = 12;
            this.label3.Text = "Actual";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "Command";
            // 
            // TextBoxSetPostion
            // 
            this.TextBoxSetPostion.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxSetPostion.BackColor = System.Drawing.SystemColors.Window;
            this.TextBoxSetPostion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxSetPostion.Location = new System.Drawing.Point(6, 106);
            this.TextBoxSetPostion.Name = "TextBoxSetPostion";
            this.TextBoxSetPostion.ReadOnly = true;
            this.TextBoxSetPostion.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TextBoxSetPostion.Size = new System.Drawing.Size(86, 21);
            this.TextBoxSetPostion.TabIndex = 10;
            this.TextBoxSetPostion.Text = "0";
            this.TextBoxSetPostion.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ButtonSetPosition
            // 
            this.ButtonSetPosition.Location = new System.Drawing.Point(98, 106);
            this.ButtonSetPosition.Name = "ButtonSetPosition";
            this.ButtonSetPosition.Size = new System.Drawing.Size(113, 23);
            this.ButtonSetPosition.TabIndex = 9;
            this.ButtonSetPosition.Text = "Set Position";
            this.ButtonSetPosition.UseVisualStyleBackColor = true;
            this.ButtonSetPosition.Click += new System.EventHandler(this.ButtonSetPosition_Click);
            // 
            // TextBoxPositionError
            // 
            this.TextBoxPositionError.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxPositionError.BackColor = System.Drawing.SystemColors.Info;
            this.TextBoxPositionError.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxPositionError.Location = new System.Drawing.Point(98, 79);
            this.TextBoxPositionError.Name = "TextBoxPositionError";
            this.TextBoxPositionError.ReadOnly = true;
            this.TextBoxPositionError.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TextBoxPositionError.Size = new System.Drawing.Size(113, 21);
            this.TextBoxPositionError.TabIndex = 8;
            this.TextBoxPositionError.Text = "Idle";
            this.TextBoxPositionError.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TextBoxActualPosition
            // 
            this.TextBoxActualPosition.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxActualPosition.BackColor = System.Drawing.SystemColors.Info;
            this.TextBoxActualPosition.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxActualPosition.Location = new System.Drawing.Point(98, 49);
            this.TextBoxActualPosition.Name = "TextBoxActualPosition";
            this.TextBoxActualPosition.ReadOnly = true;
            this.TextBoxActualPosition.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TextBoxActualPosition.Size = new System.Drawing.Size(113, 21);
            this.TextBoxActualPosition.TabIndex = 7;
            this.TextBoxActualPosition.Text = "Idle";
            this.TextBoxActualPosition.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TextBoxCommandPosition
            // 
            this.TextBoxCommandPosition.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxCommandPosition.BackColor = System.Drawing.SystemColors.Info;
            this.TextBoxCommandPosition.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxCommandPosition.Location = new System.Drawing.Point(98, 20);
            this.TextBoxCommandPosition.Name = "TextBoxCommandPosition";
            this.TextBoxCommandPosition.ReadOnly = true;
            this.TextBoxCommandPosition.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TextBoxCommandPosition.Size = new System.Drawing.Size(113, 21);
            this.TextBoxCommandPosition.TabIndex = 6;
            this.TextBoxCommandPosition.Text = "Idle";
            this.TextBoxCommandPosition.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.TextBoxPreciseVelocity);
            this.groupBox3.Controls.Add(this.ComboBoxIndexSeach);
            this.groupBox3.Controls.Add(this.CheckBoxPreciseVelocity);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.TextBoxHomingState);
            this.groupBox3.Controls.Add(this.TextBoxEscapeDistance);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.TextBoxHomingVelocity);
            this.groupBox3.Controls.Add(this.TextBoxHomingAcceleration);
            this.groupBox3.Controls.Add(this.TextBoxHomingDeceleration);
            this.groupBox3.Controls.Add(this.TextBoxHomingnegativeLimit);
            this.groupBox3.Controls.Add(this.TextBoxHomingPositiveLimit);
            this.groupBox3.Controls.Add(this.ButtonHomingStart);
            this.groupBox3.Controls.Add(this.ComboBoxHomingMethod);
            this.groupBox3.Location = new System.Drawing.Point(3, 144);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(491, 179);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Homing";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label13.Location = new System.Drawing.Point(465, 46);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(16, 12);
            this.label13.TabIndex = 33;
            this.label13.Text = "%";
            // 
            // TextBoxPreciseVelocity
            // 
            this.TextBoxPreciseVelocity.Location = new System.Drawing.Point(352, 40);
            this.TextBoxPreciseVelocity.Name = "TextBoxPreciseVelocity";
            this.TextBoxPreciseVelocity.Size = new System.Drawing.Size(107, 21);
            this.TextBoxPreciseVelocity.TabIndex = 32;
            this.TextBoxPreciseVelocity.Text = "0";
            this.TextBoxPreciseVelocity.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ComboBoxIndexSeach
            // 
            this.ComboBoxIndexSeach.AutoSize = true;
            this.ComboBoxIndexSeach.Location = new System.Drawing.Point(232, 69);
            this.ComboBoxIndexSeach.Name = "ComboBoxIndexSeach";
            this.ComboBoxIndexSeach.Size = new System.Drawing.Size(99, 16);
            this.ComboBoxIndexSeach.TabIndex = 31;
            this.ComboBoxIndexSeach.Text = "Index Search";
            this.ComboBoxIndexSeach.UseVisualStyleBackColor = true;
            // 
            // CheckBoxPreciseVelocity
            // 
            this.CheckBoxPreciseVelocity.AutoSize = true;
            this.CheckBoxPreciseVelocity.Location = new System.Drawing.Point(232, 42);
            this.CheckBoxPreciseVelocity.Name = "CheckBoxPreciseVelocity";
            this.CheckBoxPreciseVelocity.Size = new System.Drawing.Size(116, 16);
            this.CheckBoxPreciseVelocity.TabIndex = 30;
            this.CheckBoxPreciseVelocity.Text = "Precise Velocity";
            this.CheckBoxPreciseVelocity.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(251, 123);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(80, 12);
            this.label12.TabIndex = 29;
            this.label12.Text = "Homing State";
            // 
            // TextBoxHomingState
            // 
            this.TextBoxHomingState.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxHomingState.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.TextBoxHomingState.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxHomingState.Location = new System.Drawing.Point(352, 121);
            this.TextBoxHomingState.Name = "TextBoxHomingState";
            this.TextBoxHomingState.ReadOnly = true;
            this.TextBoxHomingState.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TextBoxHomingState.Size = new System.Drawing.Size(133, 21);
            this.TextBoxHomingState.TabIndex = 28;
            this.TextBoxHomingState.Text = "Idle";
            this.TextBoxHomingState.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TextBoxEscapeDistance
            // 
            this.TextBoxEscapeDistance.Location = new System.Drawing.Point(337, 13);
            this.TextBoxEscapeDistance.Name = "TextBoxEscapeDistance";
            this.TextBoxEscapeDistance.Size = new System.Drawing.Size(148, 21);
            this.TextBoxEscapeDistance.TabIndex = 27;
            this.TextBoxEscapeDistance.Text = "0";
            this.TextBoxEscapeDistance.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(230, 17);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(101, 12);
            this.label11.TabIndex = 26;
            this.label11.Text = "Escape Distance";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 151);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(85, 12);
            this.label10.TabIndex = 25;
            this.label10.Text = "Negative Limit";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 124);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 12);
            this.label9.TabIndex = 24;
            this.label9.Text = "Positive Limit";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 97);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(75, 12);
            this.label8.TabIndex = 23;
            this.label8.Text = "Deceleration";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 70);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 12);
            this.label7.TabIndex = 22;
            this.label7.Text = "Acceleration";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 12);
            this.label6.TabIndex = 21;
            this.label6.Text = "Velocity";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 14;
            this.label5.Text = "Method";
            // 
            // TextBoxHomingVelocity
            // 
            this.TextBoxHomingVelocity.Location = new System.Drawing.Point(95, 40);
            this.TextBoxHomingVelocity.Name = "TextBoxHomingVelocity";
            this.TextBoxHomingVelocity.Size = new System.Drawing.Size(118, 21);
            this.TextBoxHomingVelocity.TabIndex = 20;
            this.TextBoxHomingVelocity.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TextBoxHomingAcceleration
            // 
            this.TextBoxHomingAcceleration.Location = new System.Drawing.Point(95, 67);
            this.TextBoxHomingAcceleration.Name = "TextBoxHomingAcceleration";
            this.TextBoxHomingAcceleration.Size = new System.Drawing.Size(118, 21);
            this.TextBoxHomingAcceleration.TabIndex = 19;
            this.TextBoxHomingAcceleration.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TextBoxHomingDeceleration
            // 
            this.TextBoxHomingDeceleration.Location = new System.Drawing.Point(95, 94);
            this.TextBoxHomingDeceleration.Name = "TextBoxHomingDeceleration";
            this.TextBoxHomingDeceleration.Size = new System.Drawing.Size(118, 21);
            this.TextBoxHomingDeceleration.TabIndex = 18;
            this.TextBoxHomingDeceleration.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TextBoxHomingnegativeLimit
            // 
            this.TextBoxHomingnegativeLimit.Location = new System.Drawing.Point(95, 148);
            this.TextBoxHomingnegativeLimit.Name = "TextBoxHomingnegativeLimit";
            this.TextBoxHomingnegativeLimit.Size = new System.Drawing.Size(118, 21);
            this.TextBoxHomingnegativeLimit.TabIndex = 17;
            this.TextBoxHomingnegativeLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TextBoxHomingPositiveLimit
            // 
            this.TextBoxHomingPositiveLimit.Location = new System.Drawing.Point(95, 121);
            this.TextBoxHomingPositiveLimit.Name = "TextBoxHomingPositiveLimit";
            this.TextBoxHomingPositiveLimit.Size = new System.Drawing.Size(118, 21);
            this.TextBoxHomingPositiveLimit.TabIndex = 16;
            this.TextBoxHomingPositiveLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ButtonHomingStart
            // 
            this.ButtonHomingStart.Location = new System.Drawing.Point(352, 148);
            this.ButtonHomingStart.Name = "ButtonHomingStart";
            this.ButtonHomingStart.Size = new System.Drawing.Size(133, 23);
            this.ButtonHomingStart.TabIndex = 15;
            this.ButtonHomingStart.Text = "Homing Start";
            this.ButtonHomingStart.UseVisualStyleBackColor = true;
            this.ButtonHomingStart.Click += new System.EventHandler(this.ButtonHomingStart_Click);
            // 
            // ComboBoxHomingMethod
            // 
            this.ComboBoxHomingMethod.FormattingEnabled = true;
            this.ComboBoxHomingMethod.Location = new System.Drawing.Point(95, 14);
            this.ComboBoxHomingMethod.Name = "ComboBoxHomingMethod";
            this.ComboBoxHomingMethod.Size = new System.Drawing.Size(118, 20);
            this.ComboBoxHomingMethod.TabIndex = 12;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.CheckBoxJogByVelocityMove);
            this.groupBox4.Controls.Add(this.label16);
            this.groupBox4.Controls.Add(this.label15);
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Controls.Add(this.TextBoxAccDecMove);
            this.groupBox4.Controls.Add(this.TextBoxVelocityMove);
            this.groupBox4.Controls.Add(this.ButtonPositive);
            this.groupBox4.Controls.Add(this.ButtonPositiveStep);
            this.groupBox4.Controls.Add(this.ButtonNegative);
            this.groupBox4.Controls.Add(this.ButtonNegativeStep);
            this.groupBox4.Controls.Add(this.ButtonRepaet);
            this.groupBox4.Controls.Add(this.ButtonMoveDistance);
            this.groupBox4.Controls.Add(this.ButtonModifyVeloity);
            this.groupBox4.Controls.Add(this.ButtonMoveVelocity);
            this.groupBox4.Controls.Add(this.ButtonModify);
            this.groupBox4.Controls.Add(this.ButtonMove);
            this.groupBox4.Controls.Add(this.TextBoxPositionMove);
            this.groupBox4.Location = new System.Drawing.Point(3, 329);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(491, 161);
            this.groupBox4.TabIndex = 14;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Move";
            // 
            // CheckBoxJogByVelocityMove
            // 
            this.CheckBoxJogByVelocityMove.AutoSize = true;
            this.CheckBoxJogByVelocityMove.Location = new System.Drawing.Point(232, 109);
            this.CheckBoxJogByVelocityMove.Name = "CheckBoxJogByVelocityMove";
            this.CheckBoxJogByVelocityMove.Size = new System.Drawing.Size(144, 16);
            this.CheckBoxJogByVelocityMove.TabIndex = 42;
            this.CheckBoxJogByVelocityMove.Text = "Jog by velocity move";
            this.CheckBoxJogByVelocityMove.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(6, 80);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(55, 12);
            this.label16.TabIndex = 41;
            this.label16.Text = "Acc/Dec";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 51);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(50, 12);
            this.label15.TabIndex = 40;
            this.label15.Text = "Velocity";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 24);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(50, 12);
            this.label14.TabIndex = 39;
            this.label14.Text = "P osition";
            // 
            // TextBoxAccDecMove
            // 
            this.TextBoxAccDecMove.Location = new System.Drawing.Point(82, 76);
            this.TextBoxAccDecMove.Name = "TextBoxAccDecMove";
            this.TextBoxAccDecMove.Size = new System.Drawing.Size(161, 21);
            this.TextBoxAccDecMove.TabIndex = 38;
            this.TextBoxAccDecMove.Text = "0";
            this.TextBoxAccDecMove.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TextBoxVelocityMove
            // 
            this.TextBoxVelocityMove.Location = new System.Drawing.Point(82, 48);
            this.TextBoxVelocityMove.Name = "TextBoxVelocityMove";
            this.TextBoxVelocityMove.Size = new System.Drawing.Size(161, 21);
            this.TextBoxVelocityMove.TabIndex = 37;
            this.TextBoxVelocityMove.Text = "0";
            this.TextBoxVelocityMove.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ButtonPositive
            // 
            this.ButtonPositive.Location = new System.Drawing.Point(122, 105);
            this.ButtonPositive.Name = "ButtonPositive";
            this.ButtonPositive.Size = new System.Drawing.Size(103, 22);
            this.ButtonPositive.TabIndex = 32;
            this.ButtonPositive.Text = "Positive (+)";
            this.ButtonPositive.UseVisualStyleBackColor = true;
            this.ButtonPositive.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ButtonPositive_MouseDown);
            this.ButtonPositive.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ButtonPositive_MouseUp);
            // 
            // ButtonPositiveStep
            // 
            this.ButtonPositiveStep.Location = new System.Drawing.Point(122, 133);
            this.ButtonPositiveStep.Name = "ButtonPositiveStep";
            this.ButtonPositiveStep.Size = new System.Drawing.Size(103, 22);
            this.ButtonPositiveStep.TabIndex = 36;
            this.ButtonPositiveStep.Text = "Step (+)";
            this.ButtonPositiveStep.UseVisualStyleBackColor = true;
            this.ButtonPositiveStep.Click += new System.EventHandler(this.ButtonPositiveStep_Click);
            // 
            // ButtonNegative
            // 
            this.ButtonNegative.Location = new System.Drawing.Point(6, 105);
            this.ButtonNegative.Name = "ButtonNegative";
            this.ButtonNegative.Size = new System.Drawing.Size(104, 22);
            this.ButtonNegative.TabIndex = 31;
            this.ButtonNegative.Text = "Negative (-)";
            this.ButtonNegative.UseVisualStyleBackColor = true;
            this.ButtonNegative.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ButtonNegative_MouseDown);
            this.ButtonNegative.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ButtonNegative_MouseUp);
            // 
            // ButtonNegativeStep
            // 
            this.ButtonNegativeStep.Location = new System.Drawing.Point(6, 133);
            this.ButtonNegativeStep.Name = "ButtonNegativeStep";
            this.ButtonNegativeStep.Size = new System.Drawing.Size(104, 22);
            this.ButtonNegativeStep.TabIndex = 35;
            this.ButtonNegativeStep.Text = "Step (-)";
            this.ButtonNegativeStep.UseVisualStyleBackColor = true;
            this.ButtonNegativeStep.Click += new System.EventHandler(this.ButtonNegativeStep_Click);
            // 
            // ButtonRepaet
            // 
            this.ButtonRepaet.Location = new System.Drawing.Point(378, 75);
            this.ButtonRepaet.Name = "ButtonRepaet";
            this.ButtonRepaet.Size = new System.Drawing.Size(103, 22);
            this.ButtonRepaet.TabIndex = 34;
            this.ButtonRepaet.Text = "Repeat";
            this.ButtonRepaet.UseVisualStyleBackColor = true;
            this.ButtonRepaet.Click += new System.EventHandler(this.ButtonRepaet_Click);
            // 
            // ButtonMoveDistance
            // 
            this.ButtonMoveDistance.Location = new System.Drawing.Point(262, 75);
            this.ButtonMoveDistance.Name = "ButtonMoveDistance";
            this.ButtonMoveDistance.Size = new System.Drawing.Size(104, 22);
            this.ButtonMoveDistance.TabIndex = 33;
            this.ButtonMoveDistance.Text = "Move Distance";
            this.ButtonMoveDistance.UseVisualStyleBackColor = true;
            this.ButtonMoveDistance.Click += new System.EventHandler(this.ButtonMoveDistance_Click);
            // 
            // ButtonModifyVeloity
            // 
            this.ButtonModifyVeloity.Location = new System.Drawing.Point(378, 47);
            this.ButtonModifyVeloity.Name = "ButtonModifyVeloity";
            this.ButtonModifyVeloity.Size = new System.Drawing.Size(103, 22);
            this.ButtonModifyVeloity.TabIndex = 32;
            this.ButtonModifyVeloity.Text = "Modify Velocity";
            this.ButtonModifyVeloity.UseVisualStyleBackColor = true;
            this.ButtonModifyVeloity.Click += new System.EventHandler(this.ButtonModifyVeloity_Click);
            // 
            // ButtonMoveVelocity
            // 
            this.ButtonMoveVelocity.Location = new System.Drawing.Point(262, 47);
            this.ButtonMoveVelocity.Name = "ButtonMoveVelocity";
            this.ButtonMoveVelocity.Size = new System.Drawing.Size(104, 22);
            this.ButtonMoveVelocity.TabIndex = 31;
            this.ButtonMoveVelocity.Text = "Move Velocity";
            this.ButtonMoveVelocity.UseVisualStyleBackColor = true;
            this.ButtonMoveVelocity.Click += new System.EventHandler(this.ButtonMoveVelocity_Click);
            // 
            // ButtonModify
            // 
            this.ButtonModify.Location = new System.Drawing.Point(378, 19);
            this.ButtonModify.Name = "ButtonModify";
            this.ButtonModify.Size = new System.Drawing.Size(103, 22);
            this.ButtonModify.TabIndex = 30;
            this.ButtonModify.Text = "Modify";
            this.ButtonModify.UseVisualStyleBackColor = true;
            this.ButtonModify.Click += new System.EventHandler(this.ButtonModify_Click);
            // 
            // ButtonMove
            // 
            this.ButtonMove.Location = new System.Drawing.Point(262, 19);
            this.ButtonMove.Name = "ButtonMove";
            this.ButtonMove.Size = new System.Drawing.Size(104, 22);
            this.ButtonMove.TabIndex = 29;
            this.ButtonMove.Text = "Move";
            this.ButtonMove.UseVisualStyleBackColor = true;
            this.ButtonMove.Click += new System.EventHandler(this.ButtonMove_Click);
            // 
            // TextBoxPositionMove
            // 
            this.TextBoxPositionMove.Location = new System.Drawing.Point(82, 20);
            this.TextBoxPositionMove.Name = "TextBoxPositionMove";
            this.TextBoxPositionMove.Size = new System.Drawing.Size(161, 21);
            this.TextBoxPositionMove.TabIndex = 28;
            this.TextBoxPositionMove.Text = "0";
            this.TextBoxPositionMove.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.StopEmergency);
            this.groupBox5.Controls.Add(this.ButtonStop);
            this.groupBox5.Controls.Add(this.TextBoxStopDeceleration);
            this.groupBox5.Controls.Add(this.CheckBoxStopDeceleration);
            this.groupBox5.Location = new System.Drawing.Point(3, 496);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(491, 50);
            this.groupBox5.TabIndex = 15;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Stop";
            // 
            // StopEmergency
            // 
            this.StopEmergency.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.StopEmergency.ForeColor = System.Drawing.Color.Red;
            this.StopEmergency.Location = new System.Drawing.Point(374, 12);
            this.StopEmergency.Name = "StopEmergency";
            this.StopEmergency.Size = new System.Drawing.Size(111, 30);
            this.StopEmergency.TabIndex = 46;
            this.StopEmergency.Text = "E-Stop";
            this.StopEmergency.UseVisualStyleBackColor = true;
            this.StopEmergency.Click += new System.EventHandler(this.StopEmergency_Click);
            // 
            // ButtonStop
            // 
            this.ButtonStop.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ButtonStop.Location = new System.Drawing.Point(255, 12);
            this.ButtonStop.Name = "ButtonStop";
            this.ButtonStop.Size = new System.Drawing.Size(111, 30);
            this.ButtonStop.TabIndex = 45;
            this.ButtonStop.Text = "Stop";
            this.ButtonStop.UseVisualStyleBackColor = true;
            this.ButtonStop.Click += new System.EventHandler(this.ButtonStop_Click);
            // 
            // TextBoxStopDeceleration
            // 
            this.TextBoxStopDeceleration.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBoxStopDeceleration.BackColor = System.Drawing.SystemColors.Window;
            this.TextBoxStopDeceleration.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TextBoxStopDeceleration.Location = new System.Drawing.Point(108, 15);
            this.TextBoxStopDeceleration.Name = "TextBoxStopDeceleration";
            this.TextBoxStopDeceleration.ReadOnly = true;
            this.TextBoxStopDeceleration.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TextBoxStopDeceleration.Size = new System.Drawing.Size(135, 21);
            this.TextBoxStopDeceleration.TabIndex = 44;
            this.TextBoxStopDeceleration.Text = "100";
            this.TextBoxStopDeceleration.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // CheckBoxStopDeceleration
            // 
            this.CheckBoxStopDeceleration.AutoSize = true;
            this.CheckBoxStopDeceleration.Location = new System.Drawing.Point(8, 20);
            this.CheckBoxStopDeceleration.Name = "CheckBoxStopDeceleration";
            this.CheckBoxStopDeceleration.Size = new System.Drawing.Size(94, 16);
            this.CheckBoxStopDeceleration.TabIndex = 43;
            this.CheckBoxStopDeceleration.Text = "Deceleration";
            this.CheckBoxStopDeceleration.UseVisualStyleBackColor = true;
            // 
            // FormAxisMotion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 554);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox4);
            this.Name = "FormAxisMotion";
            this.Text = "MotionAxis";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private BaseButton ButtonStateReset;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TextBoxState;
        private System.Windows.Forms.TextBox TextBoxAmpFault;
        private System.Windows.Forms.TextBox TextBoxInPosition;
        private System.Windows.Forms.TextBox TextBoxMotionDone;
        private System.Windows.Forms.TextBox TextBoxAmpOn;
        private System.Windows.Forms.TextBox TextBoxNegativeLimit;
        private System.Windows.Forms.TextBox TextBoxStateHome;
        private System.Windows.Forms.TextBox TextBoxPositiveLimit;
        private BaseButton ButtonAmpOff;
        private BaseButton ButtonAmpClear;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TextBoxSetPostion;
        private BaseButton ButtonSetPosition;
        private System.Windows.Forms.TextBox TextBoxPositionError;
        private System.Windows.Forms.TextBox TextBoxActualPosition;
        private System.Windows.Forms.TextBox TextBoxCommandPosition;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox TextBoxEscapeDistance;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TextBoxHomingVelocity;
        private System.Windows.Forms.TextBox TextBoxHomingAcceleration;
        private System.Windows.Forms.TextBox TextBoxHomingDeceleration;
        private System.Windows.Forms.TextBox TextBoxHomingnegativeLimit;
        private System.Windows.Forms.TextBox TextBoxHomingPositiveLimit;
        private BaseButton ButtonHomingStart;
        private System.Windows.Forms.ComboBox ComboBoxHomingMethod;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox TextBoxPreciseVelocity;
        private System.Windows.Forms.CheckBox ComboBoxIndexSeach;
        private System.Windows.Forms.CheckBox CheckBoxPreciseVelocity;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox TextBoxHomingState;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox TextBoxAccDecMove;
        private System.Windows.Forms.TextBox TextBoxVelocityMove;
        private BaseButton ButtonPositive;
        private BaseButton ButtonPositiveStep;
        private BaseButton ButtonNegative;
        private BaseButton ButtonNegativeStep;
        private BaseButton ButtonRepaet;
        private BaseButton ButtonMoveDistance;
        private BaseButton ButtonModifyVeloity;
        private BaseButton ButtonMoveVelocity;
        private BaseButton ButtonModify;
        private BaseButton ButtonMove;
        private System.Windows.Forms.TextBox TextBoxPositionMove;
        private System.Windows.Forms.CheckBox CheckBoxJogByVelocityMove;
        private System.Windows.Forms.GroupBox groupBox5;
        private BaseButton StopEmergency;
        private BaseButton ButtonStop;
        private System.Windows.Forms.TextBox TextBoxStopDeceleration;
        private System.Windows.Forms.CheckBox CheckBoxStopDeceleration;
    }
}