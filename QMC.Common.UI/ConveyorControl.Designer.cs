namespace QMC.Common.UI
{
    partial class ConveyorControl
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
            this.baseGroupBoxMain = new QMC.Common.UI.BaseGroupBox();
            this.baseGroupBox1 = new QMC.Common.UI.BaseGroupBox();
            this.baseButtonSave = new QMC.Common.UI.BaseButton();
            this.baseButtonWorkRun = new QMC.Common.UI.BaseButton();
            this.baseTextBoxDelayTime = new QMC.Common.UI.BaseTextBox();
            this.baseLabel2 = new QMC.Common.UI.BaseLabel();
            this.baseLabel1 = new QMC.Common.UI.BaseLabel();
            this.baseGroupBoxStopper = new QMC.Common.UI.BaseGroupBox();
            this.baseToggleButtonStopperDown = new QMC.Common.UI.BaseToggleButton();
            this.baseToggleButtonStopperUp = new QMC.Common.UI.BaseToggleButton();
            this.baseGroupBoxDirection = new QMC.Common.UI.BaseGroupBox();
            this.baseToggleButtonBackward = new QMC.Common.UI.BaseToggleButton();
            this.baseToggleButtonFoward = new QMC.Common.UI.BaseToggleButton();
            this.baseGroupBoxRunStop = new QMC.Common.UI.BaseGroupBox();
            this.baseToggleButtonStop = new QMC.Common.UI.BaseToggleButton();
            this.baseToggleButtonRun = new QMC.Common.UI.BaseToggleButton();
            this.baseGroupBoxMain.SuspendLayout();
            this.baseGroupBox1.SuspendLayout();
            this.baseGroupBoxStopper.SuspendLayout();
            this.baseGroupBoxDirection.SuspendLayout();
            this.baseGroupBoxRunStop.SuspendLayout();
            this.SuspendLayout();
            // 
            // baseGroupBoxMain
            // 
            this.baseGroupBoxMain.Controls.Add(this.baseGroupBox1);
            this.baseGroupBoxMain.Controls.Add(this.baseGroupBoxStopper);
            this.baseGroupBoxMain.Controls.Add(this.baseGroupBoxDirection);
            this.baseGroupBoxMain.Controls.Add(this.baseGroupBoxRunStop);
            this.baseGroupBoxMain.ForeColor = System.Drawing.Color.White;
            this.baseGroupBoxMain.Location = new System.Drawing.Point(0, 0);
            this.baseGroupBoxMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseGroupBoxMain.Name = "baseGroupBoxMain";
            this.baseGroupBoxMain.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseGroupBoxMain.Size = new System.Drawing.Size(497, 96);
            this.baseGroupBoxMain.TabIndex = 0;
            this.baseGroupBoxMain.TabStop = false;
            this.baseGroupBoxMain.Text = "Conveyor";
            // 
            // baseGroupBox1
            // 
            this.baseGroupBox1.Controls.Add(this.baseButtonSave);
            this.baseGroupBox1.Controls.Add(this.baseButtonWorkRun);
            this.baseGroupBox1.Controls.Add(this.baseTextBoxDelayTime);
            this.baseGroupBox1.Controls.Add(this.baseLabel2);
            this.baseGroupBox1.Controls.Add(this.baseLabel1);
            this.baseGroupBox1.ForeColor = System.Drawing.Color.White;
            this.baseGroupBox1.Location = new System.Drawing.Point(316, 11);
            this.baseGroupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseGroupBox1.Name = "baseGroupBox1";
            this.baseGroupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseGroupBox1.Size = new System.Drawing.Size(175, 80);
            this.baseGroupBox1.TabIndex = 3;
            this.baseGroupBox1.TabStop = false;
            this.baseGroupBox1.Text = "Work";
            // 
            // baseButtonSave
            // 
            this.baseButtonSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonSave.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonSave.Location = new System.Drawing.Point(7, 48);
            this.baseButtonSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseButtonSave.Name = "baseButtonSave";
            this.baseButtonSave.Size = new System.Drawing.Size(74, 24);
            this.baseButtonSave.TabIndex = 4;
            this.baseButtonSave.Text = "Save";
            this.baseButtonSave.UseVisualStyleBackColor = false;
            this.baseButtonSave.Click += new System.EventHandler(this.baseButtonSave_Click);
            // 
            // baseButtonWorkRun
            // 
            this.baseButtonWorkRun.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonWorkRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonWorkRun.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonWorkRun.Location = new System.Drawing.Point(88, 48);
            this.baseButtonWorkRun.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseButtonWorkRun.Name = "baseButtonWorkRun";
            this.baseButtonWorkRun.Size = new System.Drawing.Size(74, 24);
            this.baseButtonWorkRun.TabIndex = 3;
            this.baseButtonWorkRun.Text = "Run";
            this.baseButtonWorkRun.UseVisualStyleBackColor = false;
            this.baseButtonWorkRun.Click += new System.EventHandler(this.baseButtonWorkRun_Click);
            // 
            // baseTextBoxDelayTime
            // 
            this.baseTextBoxDelayTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxDelayTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxDelayTime.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxDelayTime.Location = new System.Drawing.Point(101, 30);
            this.baseTextBoxDelayTime.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseTextBoxDelayTime.Name = "baseTextBoxDelayTime";
            this.baseTextBoxDelayTime.Size = new System.Drawing.Size(60, 14);
            this.baseTextBoxDelayTime.TabIndex = 2;
            // 
            // baseLabel2
            // 
            this.baseLabel2.AutoSize = true;
            this.baseLabel2.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
            this.baseLabel2.ForeColor = System.Drawing.Color.White;
            this.baseLabel2.Location = new System.Drawing.Point(5, 31);
            this.baseLabel2.Name = "baseLabel2";
            this.baseLabel2.Size = new System.Drawing.Size(76, 12);
            this.baseLabel2.TabIndex = 1;
            this.baseLabel2.Text = "Delay Time";
            // 
            // baseLabel1
            // 
            this.baseLabel1.AutoSize = true;
            this.baseLabel1.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Bold);
            this.baseLabel1.ForeColor = System.Drawing.Color.White;
            this.baseLabel1.Location = new System.Drawing.Point(5, 16);
            this.baseLabel1.Name = "baseLabel1";
            this.baseLabel1.Size = new System.Drawing.Size(96, 12);
            this.baseLabel1.TabIndex = 0;
            this.baseLabel1.Text = "Carrier Detect";
            // 
            // baseGroupBoxStopper
            // 
            this.baseGroupBoxStopper.Controls.Add(this.baseToggleButtonStopperDown);
            this.baseGroupBoxStopper.Controls.Add(this.baseToggleButtonStopperUp);
            this.baseGroupBoxStopper.ForeColor = System.Drawing.Color.White;
            this.baseGroupBoxStopper.Location = new System.Drawing.Point(212, 11);
            this.baseGroupBoxStopper.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseGroupBoxStopper.Name = "baseGroupBoxStopper";
            this.baseGroupBoxStopper.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseGroupBoxStopper.Size = new System.Drawing.Size(98, 80);
            this.baseGroupBoxStopper.TabIndex = 2;
            this.baseGroupBoxStopper.TabStop = false;
            this.baseGroupBoxStopper.Text = "Stopper";
            // 
            // baseToggleButtonStopperDown
            // 
            this.baseToggleButtonStopperDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseToggleButtonStopperDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButtonStopperDown.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseToggleButtonStopperDown.Location = new System.Drawing.Point(6, 48);
            this.baseToggleButtonStopperDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseToggleButtonStopperDown.Name = "baseToggleButtonStopperDown";
            this.baseToggleButtonStopperDown.Size = new System.Drawing.Size(88, 24);
            this.baseToggleButtonStopperDown.TabIndex = 1;
            this.baseToggleButtonStopperDown.Text = "Down";
            this.baseToggleButtonStopperDown.UseVisualStyleBackColor = false;
            this.baseToggleButtonStopperDown.Click += new System.EventHandler(this.baseToggleButtonStopperDown_Click);
            // 
            // baseToggleButtonStopperUp
            // 
            this.baseToggleButtonStopperUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseToggleButtonStopperUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButtonStopperUp.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseToggleButtonStopperUp.Location = new System.Drawing.Point(6, 19);
            this.baseToggleButtonStopperUp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseToggleButtonStopperUp.Name = "baseToggleButtonStopperUp";
            this.baseToggleButtonStopperUp.Size = new System.Drawing.Size(88, 24);
            this.baseToggleButtonStopperUp.TabIndex = 0;
            this.baseToggleButtonStopperUp.Text = "Up";
            this.baseToggleButtonStopperUp.UseVisualStyleBackColor = false;
            this.baseToggleButtonStopperUp.Click += new System.EventHandler(this.baseToggleButtonStopperUp_Click);
            // 
            // baseGroupBoxDirection
            // 
            this.baseGroupBoxDirection.Controls.Add(this.baseToggleButtonBackward);
            this.baseGroupBoxDirection.Controls.Add(this.baseToggleButtonFoward);
            this.baseGroupBoxDirection.ForeColor = System.Drawing.Color.White;
            this.baseGroupBoxDirection.Location = new System.Drawing.Point(108, 11);
            this.baseGroupBoxDirection.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseGroupBoxDirection.Name = "baseGroupBoxDirection";
            this.baseGroupBoxDirection.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseGroupBoxDirection.Size = new System.Drawing.Size(98, 80);
            this.baseGroupBoxDirection.TabIndex = 1;
            this.baseGroupBoxDirection.TabStop = false;
            this.baseGroupBoxDirection.Text = "Direction";
            // 
            // baseToggleButtonBackward
            // 
            this.baseToggleButtonBackward.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseToggleButtonBackward.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButtonBackward.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseToggleButtonBackward.Location = new System.Drawing.Point(6, 49);
            this.baseToggleButtonBackward.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseToggleButtonBackward.Name = "baseToggleButtonBackward";
            this.baseToggleButtonBackward.Size = new System.Drawing.Size(88, 24);
            this.baseToggleButtonBackward.TabIndex = 1;
            this.baseToggleButtonBackward.Text = "Backward";
            this.baseToggleButtonBackward.UseVisualStyleBackColor = false;
            this.baseToggleButtonBackward.Click += new System.EventHandler(this.baseToggleButtonBackWard_Click);
            // 
            // baseToggleButtonFoward
            // 
            this.baseToggleButtonFoward.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseToggleButtonFoward.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButtonFoward.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseToggleButtonFoward.Location = new System.Drawing.Point(6, 19);
            this.baseToggleButtonFoward.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseToggleButtonFoward.Name = "baseToggleButtonFoward";
            this.baseToggleButtonFoward.Size = new System.Drawing.Size(88, 24);
            this.baseToggleButtonFoward.TabIndex = 0;
            this.baseToggleButtonFoward.Text = "Foward";
            this.baseToggleButtonFoward.UseVisualStyleBackColor = false;
            this.baseToggleButtonFoward.Click += new System.EventHandler(this.baseToggleButtonFoward_Click);
            // 
            // baseGroupBoxRunStop
            // 
            this.baseGroupBoxRunStop.Controls.Add(this.baseToggleButtonStop);
            this.baseGroupBoxRunStop.Controls.Add(this.baseToggleButtonRun);
            this.baseGroupBoxRunStop.ForeColor = System.Drawing.Color.White;
            this.baseGroupBoxRunStop.Location = new System.Drawing.Point(5, 11);
            this.baseGroupBoxRunStop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseGroupBoxRunStop.Name = "baseGroupBoxRunStop";
            this.baseGroupBoxRunStop.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseGroupBoxRunStop.Size = new System.Drawing.Size(98, 80);
            this.baseGroupBoxRunStop.TabIndex = 0;
            this.baseGroupBoxRunStop.TabStop = false;
            this.baseGroupBoxRunStop.Text = "Run";
            // 
            // baseToggleButtonStop
            // 
            this.baseToggleButtonStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseToggleButtonStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButtonStop.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseToggleButtonStop.Location = new System.Drawing.Point(6, 49);
            this.baseToggleButtonStop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseToggleButtonStop.Name = "baseToggleButtonStop";
            this.baseToggleButtonStop.Size = new System.Drawing.Size(88, 24);
            this.baseToggleButtonStop.TabIndex = 1;
            this.baseToggleButtonStop.Text = "Stop";
            this.baseToggleButtonStop.UseVisualStyleBackColor = false;
            this.baseToggleButtonStop.Click += new System.EventHandler(this.baseToggleButtonStop_Click);
            // 
            // baseToggleButtonRun
            // 
            this.baseToggleButtonRun.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseToggleButtonRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButtonRun.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseToggleButtonRun.Location = new System.Drawing.Point(5, 19);
            this.baseToggleButtonRun.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseToggleButtonRun.Name = "baseToggleButtonRun";
            this.baseToggleButtonRun.Size = new System.Drawing.Size(88, 24);
            this.baseToggleButtonRun.TabIndex = 0;
            this.baseToggleButtonRun.Text = "Run";
            this.baseToggleButtonRun.UseVisualStyleBackColor = false;
            this.baseToggleButtonRun.Click += new System.EventHandler(this.baseToggleButtonRun_Click);
            // 
            // ConveyorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseGroupBoxMain);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ConveyorControl";
            this.Size = new System.Drawing.Size(499, 95);
            this.baseGroupBoxMain.ResumeLayout(false);
            this.baseGroupBox1.ResumeLayout(false);
            this.baseGroupBox1.PerformLayout();
            this.baseGroupBoxStopper.ResumeLayout(false);
            this.baseGroupBoxDirection.ResumeLayout(false);
            this.baseGroupBoxRunStop.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private BaseGroupBox baseGroupBoxMain;
        private BaseGroupBox baseGroupBox1;
        private BaseButton baseButtonWorkRun;
        private BaseTextBox baseTextBoxDelayTime;
        private BaseLabel baseLabel2;
        private BaseLabel baseLabel1;
        private BaseGroupBox baseGroupBoxStopper;
        private BaseToggleButton baseToggleButtonStopperDown;
        private BaseToggleButton baseToggleButtonStopperUp;
        private BaseGroupBox baseGroupBoxDirection;
        private BaseToggleButton baseToggleButtonBackward;
        private BaseToggleButton baseToggleButtonFoward;
        private BaseGroupBox baseGroupBoxRunStop;
        private BaseToggleButton baseToggleButtonStop;
        private BaseToggleButton baseToggleButtonRun;
        private BaseButton baseButtonSave;
    }
}
