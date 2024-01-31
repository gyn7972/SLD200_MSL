namespace QMC.Common.UI
{
    partial class ConveyorStateControl
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
            this.baseButtonConveyorRun = new QMC.Common.UI.BaseButton();
            this.baseGroupBoxStopper = new QMC.Common.UI.BaseGroupBox();
            this.baseButtonStopper = new QMC.Common.UI.BaseButton();
            this.baseLabelStopperDown = new QMC.Common.UI.BaseLabel();
            this.baseLabelStopperUp = new QMC.Common.UI.BaseLabel();
            this.pictureBoxStopperDown = new System.Windows.Forms.PictureBox();
            this.pictureBoxStopperUp = new System.Windows.Forms.PictureBox();
            this.baseGroupBoxClamper = new QMC.Common.UI.BaseGroupBox();
            this.baseButtonClamp = new QMC.Common.UI.BaseButton();
            this.baseLabel4 = new QMC.Common.UI.BaseLabel();
            this.baseLabel3 = new QMC.Common.UI.BaseLabel();
            this.pictureBoxUnclamp = new System.Windows.Forms.PictureBox();
            this.pictureBoxClamp = new System.Windows.Forms.PictureBox();
            this.baseLabel2 = new QMC.Common.UI.BaseLabel();
            this.baseLabel1 = new QMC.Common.UI.BaseLabel();
            this.pictureBoxRun = new System.Windows.Forms.PictureBox();
            this.pictureBoxCarrierCheck = new System.Windows.Forms.PictureBox();
            this.baseGroupBoxMain.SuspendLayout();
            this.baseGroupBoxStopper.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStopperDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStopperUp)).BeginInit();
            this.baseGroupBoxClamper.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxUnclamp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxClamp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRun)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCarrierCheck)).BeginInit();
            this.SuspendLayout();
            // 
            // baseGroupBoxMain
            // 
            this.baseGroupBoxMain.Controls.Add(this.baseButtonConveyorRun);
            this.baseGroupBoxMain.Controls.Add(this.baseGroupBoxStopper);
            this.baseGroupBoxMain.Controls.Add(this.baseGroupBoxClamper);
            this.baseGroupBoxMain.Controls.Add(this.baseLabel2);
            this.baseGroupBoxMain.Controls.Add(this.baseLabel1);
            this.baseGroupBoxMain.Controls.Add(this.pictureBoxRun);
            this.baseGroupBoxMain.Controls.Add(this.pictureBoxCarrierCheck);
            this.baseGroupBoxMain.ForeColor = System.Drawing.Color.White;
            this.baseGroupBoxMain.Location = new System.Drawing.Point(0, 3);
            this.baseGroupBoxMain.Name = "baseGroupBoxMain";
            this.baseGroupBoxMain.Size = new System.Drawing.Size(358, 125);
            this.baseGroupBoxMain.TabIndex = 0;
            this.baseGroupBoxMain.TabStop = false;
            this.baseGroupBoxMain.Text = "baseGroupBox1";
            // 
            // baseButtonConveyorRun
            // 
            this.baseButtonConveyorRun.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonConveyorRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonConveyorRun.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonConveyorRun.Location = new System.Drawing.Point(19, 97);
            this.baseButtonConveyorRun.Name = "baseButtonConveyorRun";
            this.baseButtonConveyorRun.Size = new System.Drawing.Size(100, 22);
            this.baseButtonConveyorRun.TabIndex = 12;
            this.baseButtonConveyorRun.Text = "Run";
            this.baseButtonConveyorRun.UseVisualStyleBackColor = false;
            this.baseButtonConveyorRun.Click += new System.EventHandler(this.baseButtonConveyorRun_Click);
            // 
            // baseGroupBoxStopper
            // 
            this.baseGroupBoxStopper.Controls.Add(this.baseButtonStopper);
            this.baseGroupBoxStopper.Controls.Add(this.baseLabelStopperDown);
            this.baseGroupBoxStopper.Controls.Add(this.baseLabelStopperUp);
            this.baseGroupBoxStopper.Controls.Add(this.pictureBoxStopperDown);
            this.baseGroupBoxStopper.Controls.Add(this.pictureBoxStopperUp);
            this.baseGroupBoxStopper.ForeColor = System.Drawing.Color.White;
            this.baseGroupBoxStopper.Location = new System.Drawing.Point(256, 12);
            this.baseGroupBoxStopper.Name = "baseGroupBoxStopper";
            this.baseGroupBoxStopper.Size = new System.Drawing.Size(97, 110);
            this.baseGroupBoxStopper.TabIndex = 11;
            this.baseGroupBoxStopper.TabStop = false;
            this.baseGroupBoxStopper.Text = "Stopper";
            // 
            // baseButtonStopper
            // 
            this.baseButtonStopper.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonStopper.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonStopper.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonStopper.Location = new System.Drawing.Point(6, 85);
            this.baseButtonStopper.Name = "baseButtonStopper";
            this.baseButtonStopper.Size = new System.Drawing.Size(89, 22);
            this.baseButtonStopper.TabIndex = 14;
            this.baseButtonStopper.Text = "Stopper";
            this.baseButtonStopper.UseVisualStyleBackColor = false;
            this.baseButtonStopper.Click += new System.EventHandler(this.baseButtonStopper_Click);
            // 
            // baseLabelStopperDown
            // 
            this.baseLabelStopperDown.AutoSize = true;
            this.baseLabelStopperDown.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelStopperDown.ForeColor = System.Drawing.Color.White;
            this.baseLabelStopperDown.Location = new System.Drawing.Point(42, 58);
            this.baseLabelStopperDown.Name = "baseLabelStopperDown";
            this.baseLabelStopperDown.Size = new System.Drawing.Size(52, 16);
            this.baseLabelStopperDown.TabIndex = 3;
            this.baseLabelStopperDown.Text = "Down";
            // 
            // baseLabelStopperUp
            // 
            this.baseLabelStopperUp.AutoSize = true;
            this.baseLabelStopperUp.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelStopperUp.ForeColor = System.Drawing.Color.White;
            this.baseLabelStopperUp.Location = new System.Drawing.Point(42, 25);
            this.baseLabelStopperUp.Name = "baseLabelStopperUp";
            this.baseLabelStopperUp.Size = new System.Drawing.Size(29, 16);
            this.baseLabelStopperUp.TabIndex = 2;
            this.baseLabelStopperUp.Text = "Up";
            // 
            // pictureBoxStopperDown
            // 
            this.pictureBoxStopperDown.Location = new System.Drawing.Point(8, 54);
            this.pictureBoxStopperDown.Name = "pictureBoxStopperDown";
            this.pictureBoxStopperDown.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxStopperDown.TabIndex = 1;
            this.pictureBoxStopperDown.TabStop = false;
            // 
            // pictureBoxStopperUp
            // 
            this.pictureBoxStopperUp.Location = new System.Drawing.Point(8, 21);
            this.pictureBoxStopperUp.Name = "pictureBoxStopperUp";
            this.pictureBoxStopperUp.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxStopperUp.TabIndex = 0;
            this.pictureBoxStopperUp.TabStop = false;
            // 
            // baseGroupBoxClamper
            // 
            this.baseGroupBoxClamper.Controls.Add(this.baseButtonClamp);
            this.baseGroupBoxClamper.Controls.Add(this.baseLabel4);
            this.baseGroupBoxClamper.Controls.Add(this.baseLabel3);
            this.baseGroupBoxClamper.Controls.Add(this.pictureBoxUnclamp);
            this.baseGroupBoxClamper.Controls.Add(this.pictureBoxClamp);
            this.baseGroupBoxClamper.ForeColor = System.Drawing.Color.White;
            this.baseGroupBoxClamper.Location = new System.Drawing.Point(125, 12);
            this.baseGroupBoxClamper.Name = "baseGroupBoxClamper";
            this.baseGroupBoxClamper.Size = new System.Drawing.Size(125, 110);
            this.baseGroupBoxClamper.TabIndex = 10;
            this.baseGroupBoxClamper.TabStop = false;
            this.baseGroupBoxClamper.Text = "Clamper";
            // 
            // baseButtonClamp
            // 
            this.baseButtonClamp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonClamp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonClamp.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonClamp.Location = new System.Drawing.Point(7, 85);
            this.baseButtonClamp.Name = "baseButtonClamp";
            this.baseButtonClamp.Size = new System.Drawing.Size(112, 22);
            this.baseButtonClamp.TabIndex = 13;
            this.baseButtonClamp.Text = "Clamp";
            this.baseButtonClamp.UseVisualStyleBackColor = false;
            this.baseButtonClamp.Click += new System.EventHandler(this.baseButtonClamp_Click);
            // 
            // baseLabel4
            // 
            this.baseLabel4.AutoSize = true;
            this.baseLabel4.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabel4.ForeColor = System.Drawing.Color.White;
            this.baseLabel4.Location = new System.Drawing.Point(42, 59);
            this.baseLabel4.Name = "baseLabel4";
            this.baseLabel4.Size = new System.Drawing.Size(77, 16);
            this.baseLabel4.TabIndex = 3;
            this.baseLabel4.Text = "Unclamp";
            // 
            // baseLabel3
            // 
            this.baseLabel3.AutoSize = true;
            this.baseLabel3.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabel3.ForeColor = System.Drawing.Color.White;
            this.baseLabel3.Location = new System.Drawing.Point(46, 24);
            this.baseLabel3.Name = "baseLabel3";
            this.baseLabel3.Size = new System.Drawing.Size(57, 16);
            this.baseLabel3.TabIndex = 2;
            this.baseLabel3.Text = "Clamp";
            // 
            // pictureBoxUnclamp
            // 
            this.pictureBoxUnclamp.Location = new System.Drawing.Point(6, 54);
            this.pictureBoxUnclamp.Name = "pictureBoxUnclamp";
            this.pictureBoxUnclamp.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxUnclamp.TabIndex = 1;
            this.pictureBoxUnclamp.TabStop = false;
            // 
            // pictureBoxClamp
            // 
            this.pictureBoxClamp.Location = new System.Drawing.Point(7, 21);
            this.pictureBoxClamp.Name = "pictureBoxClamp";
            this.pictureBoxClamp.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxClamp.TabIndex = 0;
            this.pictureBoxClamp.TabStop = false;
            // 
            // baseLabel2
            // 
            this.baseLabel2.AutoSize = true;
            this.baseLabel2.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabel2.ForeColor = System.Drawing.Color.White;
            this.baseLabel2.Location = new System.Drawing.Point(37, 58);
            this.baseLabel2.Name = "baseLabel2";
            this.baseLabel2.Size = new System.Drawing.Size(85, 32);
            this.baseLabel2.TabIndex = 9;
            this.baseLabel2.Text = "Conveyor\r\nRun\r\n";
            // 
            // baseLabel1
            // 
            this.baseLabel1.AutoSize = true;
            this.baseLabel1.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabel1.ForeColor = System.Drawing.Color.White;
            this.baseLabel1.Location = new System.Drawing.Point(36, 24);
            this.baseLabel1.Name = "baseLabel1";
            this.baseLabel1.Size = new System.Drawing.Size(61, 32);
            this.baseLabel1.TabIndex = 8;
            this.baseLabel1.Text = "Carrier\r\nCheck\r\n";
            // 
            // pictureBoxRun
            // 
            this.pictureBoxRun.Location = new System.Drawing.Point(6, 61);
            this.pictureBoxRun.Name = "pictureBoxRun";
            this.pictureBoxRun.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxRun.TabIndex = 1;
            this.pictureBoxRun.TabStop = false;
            // 
            // pictureBoxCarrierCheck
            // 
            this.pictureBoxCarrierCheck.Location = new System.Drawing.Point(6, 28);
            this.pictureBoxCarrierCheck.Name = "pictureBoxCarrierCheck";
            this.pictureBoxCarrierCheck.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxCarrierCheck.TabIndex = 0;
            this.pictureBoxCarrierCheck.TabStop = false;
            // 
            // ConveyorStateControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseGroupBoxMain);
            this.Name = "ConveyorStateControl";
            this.Size = new System.Drawing.Size(360, 128);
            this.baseGroupBoxMain.ResumeLayout(false);
            this.baseGroupBoxMain.PerformLayout();
            this.baseGroupBoxStopper.ResumeLayout(false);
            this.baseGroupBoxStopper.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStopperDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStopperUp)).EndInit();
            this.baseGroupBoxClamper.ResumeLayout(false);
            this.baseGroupBoxClamper.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxUnclamp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxClamp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRun)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCarrierCheck)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BaseGroupBox baseGroupBoxMain;
        private BaseGroupBox baseGroupBoxStopper;
        private QMC.Common.UI.BaseLabel baseLabelStopperDown;
        private QMC.Common.UI.BaseLabel baseLabelStopperUp;
        private System.Windows.Forms.PictureBox pictureBoxStopperDown;
        private System.Windows.Forms.PictureBox pictureBoxStopperUp;
        private BaseGroupBox baseGroupBoxClamper;
        private QMC.Common.UI.BaseLabel baseLabel4;
        private QMC.Common.UI.BaseLabel baseLabel3;
        private System.Windows.Forms.PictureBox pictureBoxUnclamp;
        private System.Windows.Forms.PictureBox pictureBoxClamp;
        private QMC.Common.UI.BaseLabel baseLabel2;
        private QMC.Common.UI.BaseLabel baseLabel1;
        private System.Windows.Forms.PictureBox pictureBoxRun;
        private System.Windows.Forms.PictureBox pictureBoxCarrierCheck;
        private BaseButton baseButtonConveyorRun;
        private BaseButton baseButtonStopper;
        private BaseButton baseButtonClamp;
    }
}
