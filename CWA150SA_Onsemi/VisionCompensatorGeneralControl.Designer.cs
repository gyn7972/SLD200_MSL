namespace CWA150SA_Onsemi300
{
    partial class VisionCompensatorGeneralControl
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
            this.baseGroupBoxGeneral = new CWA150SA_Onsemi300.BaseGroupBox();
            this.baseTextBoxDcc = new CWA150SA_Onsemi300.BaseTextBox();
            this.baseLabel4 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseTextBoxAcc = new CWA150SA_Onsemi300.BaseTextBox();
            this.baseLabel3 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseTextBoxVelocity = new CWA150SA_Onsemi300.BaseTextBox();
            this.baseButtonRun = new CWA150SA_Onsemi300.BaseButton();
            this.baseGroupBoxParameter = new CWA150SA_Onsemi300.BaseGroupBox();
            this.basePropertyGridParameter = new CWA150SA_Onsemi300.BasePropertyGrid();
            this.baseToggleButtonInvertedY = new CWA150SA_Onsemi300.BaseToggleButton();
            this.baseToggleButtonInvertedX = new CWA150SA_Onsemi300.BaseToggleButton();
            this.baseLabel2 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel1 = new CWA150SA_Onsemi300.BaseLabel();
            this.comboBoxOperate = new System.Windows.Forms.ComboBox();
            this.baseGroupBoxVerification = new CWA150SA_Onsemi300.BaseGroupBox();
            this.baseLabelVerificationX = new CWA150SA_Onsemi300.BaseLabel();
            this.baseTextBoxVerificationPitchY = new CWA150SA_Onsemi300.BaseTextBox();
            this.baseLabelVerificationY = new CWA150SA_Onsemi300.BaseLabel();
            this.baseTextBoxVerificationPitchX = new CWA150SA_Onsemi300.BaseTextBox();
            this.baseGroupBoxMeasureMent = new CWA150SA_Onsemi300.BaseGroupBox();
            this.baseTextBoxMeasurePitchY = new CWA150SA_Onsemi300.BaseTextBox();
            this.baseTextBoxMeasurePitchX = new CWA150SA_Onsemi300.BaseTextBox();
            this.baseLabelMeasurementX = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelMeasurementY = new CWA150SA_Onsemi300.BaseLabel();
            this.baseGroupBoxGeneral.SuspendLayout();
            this.baseGroupBoxParameter.SuspendLayout();
            this.baseGroupBoxVerification.SuspendLayout();
            this.baseGroupBoxMeasureMent.SuspendLayout();
            this.SuspendLayout();
            // 
            // baseGroupBoxGeneral
            // 
            this.baseGroupBoxGeneral.Controls.Add(this.baseTextBoxDcc);
            this.baseGroupBoxGeneral.Controls.Add(this.baseLabel4);
            this.baseGroupBoxGeneral.Controls.Add(this.baseTextBoxAcc);
            this.baseGroupBoxGeneral.Controls.Add(this.baseLabel3);
            this.baseGroupBoxGeneral.Controls.Add(this.baseTextBoxVelocity);
            this.baseGroupBoxGeneral.Controls.Add(this.baseButtonRun);
            this.baseGroupBoxGeneral.Controls.Add(this.baseGroupBoxParameter);
            this.baseGroupBoxGeneral.Controls.Add(this.baseToggleButtonInvertedY);
            this.baseGroupBoxGeneral.Controls.Add(this.baseToggleButtonInvertedX);
            this.baseGroupBoxGeneral.Controls.Add(this.baseLabel2);
            this.baseGroupBoxGeneral.Controls.Add(this.baseLabel1);
            this.baseGroupBoxGeneral.Controls.Add(this.comboBoxOperate);
            this.baseGroupBoxGeneral.Controls.Add(this.baseGroupBoxVerification);
            this.baseGroupBoxGeneral.Controls.Add(this.baseGroupBoxMeasureMent);
            this.baseGroupBoxGeneral.ForeColor = System.Drawing.Color.White;
            this.baseGroupBoxGeneral.Location = new System.Drawing.Point(0, 0);
            this.baseGroupBoxGeneral.Name = "baseGroupBoxGeneral";
            this.baseGroupBoxGeneral.Size = new System.Drawing.Size(440, 472);
            this.baseGroupBoxGeneral.TabIndex = 0;
            this.baseGroupBoxGeneral.TabStop = false;
            this.baseGroupBoxGeneral.Text = " General ";
            // 
            // baseTextBoxDcc
            // 
            this.baseTextBoxDcc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxDcc.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxDcc.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxDcc.Location = new System.Drawing.Point(350, 136);
            this.baseTextBoxDcc.Name = "baseTextBoxDcc";
            this.baseTextBoxDcc.Size = new System.Drawing.Size(84, 14);
            this.baseTextBoxDcc.TabIndex = 15;
            this.baseTextBoxDcc.TextChanged += new System.EventHandler(this.baseTextBoxDcc_TextChanged);
            // 
            // baseLabel4
            // 
            this.baseLabel4.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.baseLabel4.ForeColor = System.Drawing.Color.White;
            this.baseLabel4.Location = new System.Drawing.Point(227, 132);
            this.baseLabel4.Name = "baseLabel4";
            this.baseLabel4.Size = new System.Drawing.Size(117, 20);
            this.baseLabel4.TabIndex = 14;
            this.baseLabel4.Text = "Dcc";
            this.baseLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // baseTextBoxAcc
            // 
            this.baseTextBoxAcc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxAcc.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxAcc.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxAcc.Location = new System.Drawing.Point(350, 106);
            this.baseTextBoxAcc.Name = "baseTextBoxAcc";
            this.baseTextBoxAcc.Size = new System.Drawing.Size(84, 14);
            this.baseTextBoxAcc.TabIndex = 13;
            this.baseTextBoxAcc.TextChanged += new System.EventHandler(this.baseTextBoxAcc_TextChanged);
            // 
            // baseLabel3
            // 
            this.baseLabel3.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.baseLabel3.ForeColor = System.Drawing.Color.White;
            this.baseLabel3.Location = new System.Drawing.Point(227, 102);
            this.baseLabel3.Name = "baseLabel3";
            this.baseLabel3.Size = new System.Drawing.Size(117, 20);
            this.baseLabel3.TabIndex = 12;
            this.baseLabel3.Text = "Acc";
            this.baseLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // baseTextBoxVelocity
            // 
            this.baseTextBoxVelocity.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxVelocity.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxVelocity.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxVelocity.Location = new System.Drawing.Point(350, 74);
            this.baseTextBoxVelocity.Name = "baseTextBoxVelocity";
            this.baseTextBoxVelocity.Size = new System.Drawing.Size(84, 14);
            this.baseTextBoxVelocity.TabIndex = 11;
            this.baseTextBoxVelocity.TextChanged += new System.EventHandler(this.baseTextBoxVelocity_TextChanged);
            // 
            // baseButtonRun
            // 
            this.baseButtonRun.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonRun.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonRun.Location = new System.Drawing.Point(337, 437);
            this.baseButtonRun.Name = "baseButtonRun";
            this.baseButtonRun.Size = new System.Drawing.Size(100, 30);
            this.baseButtonRun.TabIndex = 9;
            this.baseButtonRun.Text = "Run";
            this.baseButtonRun.UseVisualStyleBackColor = false;
            this.baseButtonRun.Click += new System.EventHandler(this.baseButtonRun_Click);
            // 
            // baseGroupBoxParameter
            // 
            this.baseGroupBoxParameter.Controls.Add(this.basePropertyGridParameter);
            this.baseGroupBoxParameter.ForeColor = System.Drawing.Color.White;
            this.baseGroupBoxParameter.Location = new System.Drawing.Point(6, 205);
            this.baseGroupBoxParameter.Name = "baseGroupBoxParameter";
            this.baseGroupBoxParameter.Size = new System.Drawing.Size(428, 226);
            this.baseGroupBoxParameter.TabIndex = 8;
            this.baseGroupBoxParameter.TabStop = false;
            this.baseGroupBoxParameter.Text = "Parameter";
            this.baseGroupBoxParameter.Enter += new System.EventHandler(this.baseGroupBoxParameter_Enter);
            // 
            // basePropertyGridParameter
            // 
            this.basePropertyGridParameter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.basePropertyGridParameter.CategoryForeColor = System.Drawing.Color.Black;
            this.basePropertyGridParameter.CategorySplitterColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.basePropertyGridParameter.CommandsBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.basePropertyGridParameter.HelpBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.basePropertyGridParameter.HelpForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.basePropertyGridParameter.Location = new System.Drawing.Point(6, 20);
            this.basePropertyGridParameter.Name = "basePropertyGridParameter";
            this.basePropertyGridParameter.SelectedItemWithFocusBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.basePropertyGridParameter.Size = new System.Drawing.Size(416, 200);
            this.basePropertyGridParameter.TabIndex = 0;
            this.basePropertyGridParameter.ViewBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.basePropertyGridParameter.ViewBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            // 
            // baseToggleButtonInvertedY
            // 
            this.baseToggleButtonInvertedY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseToggleButtonInvertedY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButtonInvertedY.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseToggleButtonInvertedY.Location = new System.Drawing.Point(335, 166);
            this.baseToggleButtonInvertedY.Name = "baseToggleButtonInvertedY";
            this.baseToggleButtonInvertedY.Size = new System.Drawing.Size(100, 30);
            this.baseToggleButtonInvertedY.TabIndex = 7;
            this.baseToggleButtonInvertedY.Text = "InvertedY";
            this.baseToggleButtonInvertedY.UseVisualStyleBackColor = false;
            this.baseToggleButtonInvertedY.Click += new System.EventHandler(this.baseToggleButtonInvertedY_Click);
            // 
            // baseToggleButtonInvertedX
            // 
            this.baseToggleButtonInvertedX.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseToggleButtonInvertedX.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButtonInvertedX.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseToggleButtonInvertedX.Location = new System.Drawing.Point(229, 166);
            this.baseToggleButtonInvertedX.Name = "baseToggleButtonInvertedX";
            this.baseToggleButtonInvertedX.Size = new System.Drawing.Size(100, 30);
            this.baseToggleButtonInvertedX.TabIndex = 6;
            this.baseToggleButtonInvertedX.Text = "InvertedX";
            this.baseToggleButtonInvertedX.UseVisualStyleBackColor = false;
            this.baseToggleButtonInvertedX.Click += new System.EventHandler(this.baseToggleButtonInvertedX_Click);
            // 
            // baseLabel2
            // 
            this.baseLabel2.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.baseLabel2.ForeColor = System.Drawing.Color.White;
            this.baseLabel2.Location = new System.Drawing.Point(227, 70);
            this.baseLabel2.Name = "baseLabel2";
            this.baseLabel2.Size = new System.Drawing.Size(117, 20);
            this.baseLabel2.TabIndex = 5;
            this.baseLabel2.Text = "Velocity";
            this.baseLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // baseLabel1
            // 
            this.baseLabel1.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.baseLabel1.ForeColor = System.Drawing.Color.White;
            this.baseLabel1.Location = new System.Drawing.Point(227, 30);
            this.baseLabel1.Name = "baseLabel1";
            this.baseLabel1.Size = new System.Drawing.Size(117, 20);
            this.baseLabel1.TabIndex = 4;
            this.baseLabel1.Text = "Operate";
            this.baseLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxOperate
            // 
            this.comboBoxOperate.FormattingEnabled = true;
            this.comboBoxOperate.Location = new System.Drawing.Point(350, 30);
            this.comboBoxOperate.Name = "comboBoxOperate";
            this.comboBoxOperate.Size = new System.Drawing.Size(85, 20);
            this.comboBoxOperate.TabIndex = 2;
            this.comboBoxOperate.SelectedIndexChanged += new System.EventHandler(this.comboBoxOperate_SelectedIndexChanged);
            // 
            // baseGroupBoxVerification
            // 
            this.baseGroupBoxVerification.Controls.Add(this.baseLabelVerificationX);
            this.baseGroupBoxVerification.Controls.Add(this.baseTextBoxVerificationPitchY);
            this.baseGroupBoxVerification.Controls.Add(this.baseLabelVerificationY);
            this.baseGroupBoxVerification.Controls.Add(this.baseTextBoxVerificationPitchX);
            this.baseGroupBoxVerification.ForeColor = System.Drawing.Color.White;
            this.baseGroupBoxVerification.Location = new System.Drawing.Point(6, 111);
            this.baseGroupBoxVerification.Name = "baseGroupBoxVerification";
            this.baseGroupBoxVerification.Size = new System.Drawing.Size(215, 85);
            this.baseGroupBoxVerification.TabIndex = 1;
            this.baseGroupBoxVerification.TabStop = false;
            this.baseGroupBoxVerification.Text = "Verification";
            // 
            // baseLabelVerificationX
            // 
            this.baseLabelVerificationX.AutoSize = true;
            this.baseLabelVerificationX.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Bold);
            this.baseLabelVerificationX.ForeColor = System.Drawing.Color.White;
            this.baseLabelVerificationX.Location = new System.Drawing.Point(6, 27);
            this.baseLabelVerificationX.Name = "baseLabelVerificationX";
            this.baseLabelVerificationX.Size = new System.Drawing.Size(110, 12);
            this.baseLabelVerificationX.TabIndex = 12;
            this.baseLabelVerificationX.Text = "Pitch Distance X[mm]";
            // 
            // baseTextBoxVerificationPitchY
            // 
            this.baseTextBoxVerificationPitchY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxVerificationPitchY.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxVerificationPitchY.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxVerificationPitchY.Location = new System.Drawing.Point(141, 55);
            this.baseTextBoxVerificationPitchY.Name = "baseTextBoxVerificationPitchY";
            this.baseTextBoxVerificationPitchY.Size = new System.Drawing.Size(68, 14);
            this.baseTextBoxVerificationPitchY.TabIndex = 12;
            this.baseTextBoxVerificationPitchY.TextChanged += new System.EventHandler(this.baseTextBoxVerificationPitchY_TextChanged);
            // 
            // baseLabelVerificationY
            // 
            this.baseLabelVerificationY.AutoSize = true;
            this.baseLabelVerificationY.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Bold);
            this.baseLabelVerificationY.ForeColor = System.Drawing.Color.White;
            this.baseLabelVerificationY.Location = new System.Drawing.Point(6, 55);
            this.baseLabelVerificationY.Name = "baseLabelVerificationY";
            this.baseLabelVerificationY.Size = new System.Drawing.Size(110, 12);
            this.baseLabelVerificationY.TabIndex = 11;
            this.baseLabelVerificationY.Text = "Pitch Distance Y[mm]";
            // 
            // baseTextBoxVerificationPitchX
            // 
            this.baseTextBoxVerificationPitchX.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxVerificationPitchX.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxVerificationPitchX.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxVerificationPitchX.Location = new System.Drawing.Point(141, 23);
            this.baseTextBoxVerificationPitchX.Name = "baseTextBoxVerificationPitchX";
            this.baseTextBoxVerificationPitchX.Size = new System.Drawing.Size(68, 14);
            this.baseTextBoxVerificationPitchX.TabIndex = 11;
            this.baseTextBoxVerificationPitchX.TextChanged += new System.EventHandler(this.baseTextBoxVerificationPitchX_TextChanged);
            // 
            // baseGroupBoxMeasureMent
            // 
            this.baseGroupBoxMeasureMent.Controls.Add(this.baseTextBoxMeasurePitchY);
            this.baseGroupBoxMeasureMent.Controls.Add(this.baseTextBoxMeasurePitchX);
            this.baseGroupBoxMeasureMent.Controls.Add(this.baseLabelMeasurementX);
            this.baseGroupBoxMeasureMent.Controls.Add(this.baseLabelMeasurementY);
            this.baseGroupBoxMeasureMent.ForeColor = System.Drawing.Color.White;
            this.baseGroupBoxMeasureMent.Location = new System.Drawing.Point(6, 17);
            this.baseGroupBoxMeasureMent.Name = "baseGroupBoxMeasureMent";
            this.baseGroupBoxMeasureMent.Size = new System.Drawing.Size(215, 85);
            this.baseGroupBoxMeasureMent.TabIndex = 0;
            this.baseGroupBoxMeasureMent.TabStop = false;
            this.baseGroupBoxMeasureMent.Text = "MeasureMent";
            // 
            // baseTextBoxMeasurePitchY
            // 
            this.baseTextBoxMeasurePitchY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxMeasurePitchY.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxMeasurePitchY.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxMeasurePitchY.Location = new System.Drawing.Point(141, 53);
            this.baseTextBoxMeasurePitchY.Name = "baseTextBoxMeasurePitchY";
            this.baseTextBoxMeasurePitchY.Size = new System.Drawing.Size(68, 14);
            this.baseTextBoxMeasurePitchY.TabIndex = 10;
            this.baseTextBoxMeasurePitchY.TextChanged += new System.EventHandler(this.baseTextBoxMeasurePitchY_TextChanged);
            // 
            // baseTextBoxMeasurePitchX
            // 
            this.baseTextBoxMeasurePitchX.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxMeasurePitchX.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxMeasurePitchX.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxMeasurePitchX.Location = new System.Drawing.Point(141, 23);
            this.baseTextBoxMeasurePitchX.Name = "baseTextBoxMeasurePitchX";
            this.baseTextBoxMeasurePitchX.Size = new System.Drawing.Size(68, 14);
            this.baseTextBoxMeasurePitchX.TabIndex = 9;
            this.baseTextBoxMeasurePitchX.TextChanged += new System.EventHandler(this.baseTextBoxMeasurePitchX_TextChanged);
            // 
            // baseLabelMeasurementX
            // 
            this.baseLabelMeasurementX.AutoSize = true;
            this.baseLabelMeasurementX.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Bold);
            this.baseLabelMeasurementX.ForeColor = System.Drawing.Color.White;
            this.baseLabelMeasurementX.Location = new System.Drawing.Point(6, 25);
            this.baseLabelMeasurementX.Name = "baseLabelMeasurementX";
            this.baseLabelMeasurementX.Size = new System.Drawing.Size(110, 12);
            this.baseLabelMeasurementX.TabIndex = 8;
            this.baseLabelMeasurementX.Text = "Pitch Distance X[mm]";
            // 
            // baseLabelMeasurementY
            // 
            this.baseLabelMeasurementY.AutoSize = true;
            this.baseLabelMeasurementY.Font = new System.Drawing.Font("Arial", 7F, System.Drawing.FontStyle.Bold);
            this.baseLabelMeasurementY.ForeColor = System.Drawing.Color.White;
            this.baseLabelMeasurementY.Location = new System.Drawing.Point(6, 55);
            this.baseLabelMeasurementY.Name = "baseLabelMeasurementY";
            this.baseLabelMeasurementY.Size = new System.Drawing.Size(110, 12);
            this.baseLabelMeasurementY.TabIndex = 0;
            this.baseLabelMeasurementY.Text = "Pitch Distance Y[mm]";
            // 
            // VisionCompensatorGeneralControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseGroupBoxGeneral);
            this.Name = "VisionCompensatorGeneralControl";
            this.Size = new System.Drawing.Size(440, 475);
            this.baseGroupBoxGeneral.ResumeLayout(false);
            this.baseGroupBoxGeneral.PerformLayout();
            this.baseGroupBoxParameter.ResumeLayout(false);
            this.baseGroupBoxVerification.ResumeLayout(false);
            this.baseGroupBoxVerification.PerformLayout();
            this.baseGroupBoxMeasureMent.ResumeLayout(false);
            this.baseGroupBoxMeasureMent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private BaseGroupBox baseGroupBoxMeasureMent;
        private BaseTextBox baseTextBoxMeasurePitchY;
        private BaseTextBox baseTextBoxMeasurePitchX;
        private BaseLabel baseLabelMeasurementX;
        private BaseLabel baseLabelMeasurementY;
        private BaseGroupBox baseGroupBoxVerification;
        private BaseLabel baseLabelVerificationX;
        private BaseTextBox baseTextBoxVerificationPitchY;
        private BaseLabel baseLabelVerificationY;
        private BaseTextBox baseTextBoxVerificationPitchX;
        private System.Windows.Forms.ComboBox comboBoxOperate;
        private BaseLabel baseLabel1;
        private BaseLabel baseLabel2;
        private BaseToggleButton baseToggleButtonInvertedX;
        private BaseToggleButton baseToggleButtonInvertedY;
        private BaseGroupBox baseGroupBoxParameter;
        private BasePropertyGrid basePropertyGridParameter;
        private BaseButton baseButtonRun;
        private BaseGroupBox baseGroupBoxGeneral;
        private BaseTextBox baseTextBoxDcc;
        private BaseLabel baseLabel4;
        private BaseTextBox baseTextBoxAcc;
        private BaseLabel baseLabel3;
        private BaseTextBox baseTextBoxVelocity;
    }
}
