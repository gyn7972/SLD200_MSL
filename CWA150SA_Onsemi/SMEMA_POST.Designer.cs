namespace CWA150SA_Onsemi300
{
    partial class SMEMA_POST
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
            this.groupBoxPOST = new System.Windows.Forms.GroupBox();
            this.pictureBoxBoardAvailableToPOST = new System.Windows.Forms.PictureBox();
            this.pictureBoxMachineReadyFromPOST = new System.Windows.Forms.PictureBox();
            this.labelBoardAvailablePOST = new System.Windows.Forms.Label();
            this.labelMachineReadyPOST = new System.Windows.Forms.Label();
            this.groupBoxPOST.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBoardAvailableToPOST)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMachineReadyFromPOST)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxPOST
            // 
            this.groupBoxPOST.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.groupBoxPOST.Controls.Add(this.pictureBoxBoardAvailableToPOST);
            this.groupBoxPOST.Controls.Add(this.pictureBoxMachineReadyFromPOST);
            this.groupBoxPOST.Controls.Add(this.labelBoardAvailablePOST);
            this.groupBoxPOST.Controls.Add(this.labelMachineReadyPOST);
            this.groupBoxPOST.ForeColor = System.Drawing.Color.White;
            this.groupBoxPOST.Location = new System.Drawing.Point(4, 2);
            this.groupBoxPOST.Name = "groupBoxPOST";
            this.groupBoxPOST.Size = new System.Drawing.Size(250, 81);
            this.groupBoxPOST.TabIndex = 14;
            this.groupBoxPOST.TabStop = false;
            this.groupBoxPOST.Text = " SMEMA - POST ";
            // 
            // pictureBoxBoardAvailableToPOST
            // 
            this.pictureBoxBoardAvailableToPOST.Image = global::CWA150SA_Onsemi.Properties.Resources.DioRectangleOff;
            this.pictureBoxBoardAvailableToPOST.Location = new System.Drawing.Point(22, 48);
            this.pictureBoxBoardAvailableToPOST.Name = "pictureBoxBoardAvailableToPOST";
            this.pictureBoxBoardAvailableToPOST.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxBoardAvailableToPOST.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxBoardAvailableToPOST.TabIndex = 28;
            this.pictureBoxBoardAvailableToPOST.TabStop = false;
            // 
            // pictureBoxMachineReadyFromPOST
            // 
            this.pictureBoxMachineReadyFromPOST.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
            this.pictureBoxMachineReadyFromPOST.Location = new System.Drawing.Point(22, 20);
            this.pictureBoxMachineReadyFromPOST.Name = "pictureBoxMachineReadyFromPOST";
            this.pictureBoxMachineReadyFromPOST.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxMachineReadyFromPOST.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxMachineReadyFromPOST.TabIndex = 27;
            this.pictureBoxMachineReadyFromPOST.TabStop = false;
            // 
            // labelBoardAvailablePOST
            // 
            this.labelBoardAvailablePOST.AutoSize = true;
            this.labelBoardAvailablePOST.Location = new System.Drawing.Point(135, 54);
            this.labelBoardAvailablePOST.Name = "labelBoardAvailablePOST";
            this.labelBoardAvailablePOST.Size = new System.Drawing.Size(93, 12);
            this.labelBoardAvailablePOST.TabIndex = 12;
            this.labelBoardAvailablePOST.Text = "Board Available";
            // 
            // labelMachineReadyPOST
            // 
            this.labelMachineReadyPOST.AutoSize = true;
            this.labelMachineReadyPOST.Location = new System.Drawing.Point(66, 26);
            this.labelMachineReadyPOST.Name = "labelMachineReadyPOST";
            this.labelMachineReadyPOST.Size = new System.Drawing.Size(162, 12);
            this.labelMachineReadyPOST.TabIndex = 10;
            this.labelMachineReadyPOST.Text = "Machine Ready To Receive";
            // 
            // SMEMA_POST
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.Controls.Add(this.groupBoxPOST);
            this.Name = "SMEMA_POST";
            this.Size = new System.Drawing.Size(258, 86);
            this.groupBoxPOST.ResumeLayout(false);
            this.groupBoxPOST.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBoardAvailableToPOST)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMachineReadyFromPOST)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBoxPOST;
        private System.Windows.Forms.Label labelBoardAvailablePOST;
        private System.Windows.Forms.Label labelMachineReadyPOST;
        private System.Windows.Forms.PictureBox pictureBoxBoardAvailableToPOST;
        private System.Windows.Forms.PictureBox pictureBoxMachineReadyFromPOST;
    }
}
