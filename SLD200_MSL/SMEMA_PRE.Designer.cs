namespace SLD200_MSL
{
    partial class SMEMA_PRE
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
            this.groupBoxPRE = new System.Windows.Forms.GroupBox();
            this.pictureBoxBoardAvailableToPRE = new System.Windows.Forms.PictureBox();
            this.pictureBoxMachineReadyFromPRE = new System.Windows.Forms.PictureBox();
            this.labelBoardAvailablePRE = new System.Windows.Forms.Label();
            this.labelMachineReadyPRE = new System.Windows.Forms.Label();
            this.groupBoxPRE.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBoardAvailableToPRE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMachineReadyFromPRE)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxPRE
            // 
            this.groupBoxPRE.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.groupBoxPRE.Controls.Add(this.pictureBoxBoardAvailableToPRE);
            this.groupBoxPRE.Controls.Add(this.pictureBoxMachineReadyFromPRE);
            this.groupBoxPRE.Controls.Add(this.labelBoardAvailablePRE);
            this.groupBoxPRE.Controls.Add(this.labelMachineReadyPRE);
            this.groupBoxPRE.ForeColor = System.Drawing.Color.White;
            this.groupBoxPRE.Location = new System.Drawing.Point(4, 2);
            this.groupBoxPRE.Name = "groupBoxPRE";
            this.groupBoxPRE.Size = new System.Drawing.Size(250, 81);
            this.groupBoxPRE.TabIndex = 0;
            this.groupBoxPRE.TabStop = false;
            this.groupBoxPRE.Text = " SMEMA - PRE ";
            // 
            // pictureBoxBoardAvailableToPRE
            // 
            this.pictureBoxBoardAvailableToPRE.Image = global::SLD200.Properties.Resources.DioRectangleOff;
            this.pictureBoxBoardAvailableToPRE.Location = new System.Drawing.Point(22, 48);
            this.pictureBoxBoardAvailableToPRE.Name = "pictureBoxBoardAvailableToPRE";
            this.pictureBoxBoardAvailableToPRE.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxBoardAvailableToPRE.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxBoardAvailableToPRE.TabIndex = 26;
            this.pictureBoxBoardAvailableToPRE.TabStop = false;
            // 
            // pictureBoxMachineReadyFromPRE
            // 
            this.pictureBoxMachineReadyFromPRE.Image = global::SLD200.Properties.Resources.DioEllipseOff;
            this.pictureBoxMachineReadyFromPRE.Location = new System.Drawing.Point(22, 20);
            this.pictureBoxMachineReadyFromPRE.Name = "pictureBoxMachineReadyFromPRE";
            this.pictureBoxMachineReadyFromPRE.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxMachineReadyFromPRE.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxMachineReadyFromPRE.TabIndex = 25;
            this.pictureBoxMachineReadyFromPRE.TabStop = false;
            // 
            // labelBoardAvailablePRE
            // 
            this.labelBoardAvailablePRE.AutoSize = true;
            this.labelBoardAvailablePRE.Location = new System.Drawing.Point(135, 54);
            this.labelBoardAvailablePRE.Name = "labelBoardAvailablePRE";
            this.labelBoardAvailablePRE.Size = new System.Drawing.Size(93, 12);
            this.labelBoardAvailablePRE.TabIndex = 12;
            this.labelBoardAvailablePRE.Text = "Board Available";
            // 
            // labelMachineReadyPRE
            // 
            this.labelMachineReadyPRE.AutoSize = true;
            this.labelMachineReadyPRE.Location = new System.Drawing.Point(66, 26);
            this.labelMachineReadyPRE.Name = "labelMachineReadyPRE";
            this.labelMachineReadyPRE.Size = new System.Drawing.Size(162, 12);
            this.labelMachineReadyPRE.TabIndex = 10;
            this.labelMachineReadyPRE.Text = "Machine Ready To Receive";
            this.labelMachineReadyPRE.Click += new System.EventHandler(this.labelMachineReadyPRE_Click);
            // 
            // SMEMA_PRE
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.Controls.Add(this.groupBoxPRE);
            this.Name = "SMEMA_PRE";
            this.Size = new System.Drawing.Size(258, 86);
            this.groupBoxPRE.ResumeLayout(false);
            this.groupBoxPRE.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBoardAvailableToPRE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMachineReadyFromPRE)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxPRE;
        private System.Windows.Forms.Label labelBoardAvailablePRE;
        private System.Windows.Forms.Label labelMachineReadyPRE;
        private System.Windows.Forms.PictureBox pictureBoxMachineReadyFromPRE;
        private System.Windows.Forms.PictureBox pictureBoxBoardAvailableToPRE;
    }
}
