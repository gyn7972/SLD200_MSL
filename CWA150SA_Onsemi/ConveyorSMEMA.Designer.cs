namespace CWA150SA_Onsemi300
{
    partial class ConveyorSMEMA
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
            this.labelBoardAvailablePRE = new System.Windows.Forms.Label();
            this.labelMachineReadyPRE = new System.Windows.Forms.Label();
            this.groupBoxPOST = new System.Windows.Forms.GroupBox();
            this.labelBoardAvailablePOST = new System.Windows.Forms.Label();
            this.labelMachineReadyPOST = new System.Windows.Forms.Label();
            this.btnMaterialCheck = new CWA150SA_Onsemi300.BaseButton();
            this.pictureBoxPresenceDetector = new System.Windows.Forms.PictureBox();
            this.pictureBoxMachineReadyFromPRE = new System.Windows.Forms.PictureBox();
            this.pictureBoxBoardAvailableToPRE = new System.Windows.Forms.PictureBox();
            this.pictureBoxBoardAvailableToPOST = new System.Windows.Forms.PictureBox();
            this.pictureBoxMachineReadyFromPOST = new System.Windows.Forms.PictureBox();
            this.groupBoxPRE.SuspendLayout();
            this.groupBoxPOST.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPresenceDetector)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMachineReadyFromPRE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBoardAvailableToPRE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBoardAvailableToPOST)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMachineReadyFromPOST)).BeginInit();
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
            // 
            // groupBoxPOST
            // 
            this.groupBoxPOST.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.groupBoxPOST.Controls.Add(this.pictureBoxBoardAvailableToPOST);
            this.groupBoxPOST.Controls.Add(this.pictureBoxMachineReadyFromPOST);
            this.groupBoxPOST.Controls.Add(this.labelBoardAvailablePOST);
            this.groupBoxPOST.Controls.Add(this.labelMachineReadyPOST);
            this.groupBoxPOST.ForeColor = System.Drawing.Color.White;
            this.groupBoxPOST.Location = new System.Drawing.Point(260, 2);
            this.groupBoxPOST.Name = "groupBoxPOST";
            this.groupBoxPOST.Size = new System.Drawing.Size(250, 81);
            this.groupBoxPOST.TabIndex = 14;
            this.groupBoxPOST.TabStop = false;
            this.groupBoxPOST.Text = " SMEMA - POST ";
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
            // btnMaterialCheck
            // 
            this.btnMaterialCheck.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.btnMaterialCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMaterialCheck.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnMaterialCheck.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnMaterialCheck.Location = new System.Drawing.Point(72, 89);
            this.btnMaterialCheck.Name = "btnMaterialCheck";
            this.btnMaterialCheck.Size = new System.Drawing.Size(160, 34);
            this.btnMaterialCheck.TabIndex = 3;
            this.btnMaterialCheck.Text = "Material";
            this.btnMaterialCheck.UseVisualStyleBackColor = false;
            this.btnMaterialCheck.Click += new System.EventHandler(this.btnMaterialCheck_Click);
            // 
            // pictureBoxPresenceDetector
            // 
            this.pictureBoxPresenceDetector.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
            this.pictureBoxPresenceDetector.Location = new System.Drawing.Point(26, 94);
            this.pictureBoxPresenceDetector.Name = "pictureBoxPresenceDetector";
            this.pictureBoxPresenceDetector.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxPresenceDetector.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxPresenceDetector.TabIndex = 25;
            this.pictureBoxPresenceDetector.TabStop = false;
            // 
            // pictureBoxMachineReadyFromPRE
            // 
            this.pictureBoxMachineReadyFromPRE.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
            this.pictureBoxMachineReadyFromPRE.Location = new System.Drawing.Point(22, 20);
            this.pictureBoxMachineReadyFromPRE.Name = "pictureBoxMachineReadyFromPRE";
            this.pictureBoxMachineReadyFromPRE.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxMachineReadyFromPRE.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxMachineReadyFromPRE.TabIndex = 25;
            this.pictureBoxMachineReadyFromPRE.TabStop = false;
            // 
            // pictureBoxBoardAvailableToPRE
            // 
            this.pictureBoxBoardAvailableToPRE.Image = global::CWA150SA_Onsemi.Properties.Resources.DioRectangleOff;
            this.pictureBoxBoardAvailableToPRE.Location = new System.Drawing.Point(22, 48);
            this.pictureBoxBoardAvailableToPRE.Name = "pictureBoxBoardAvailableToPRE";
            this.pictureBoxBoardAvailableToPRE.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxBoardAvailableToPRE.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxBoardAvailableToPRE.TabIndex = 26;
            this.pictureBoxBoardAvailableToPRE.TabStop = false;
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
            // ConveyorSMEMA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.Controls.Add(this.pictureBoxPresenceDetector);
            this.Controls.Add(this.groupBoxPOST);
            this.Controls.Add(this.groupBoxPRE);
            this.Controls.Add(this.btnMaterialCheck);
            this.Name = "ConveyorSMEMA";
            this.Size = new System.Drawing.Size(514, 130);
            this.groupBoxPRE.ResumeLayout(false);
            this.groupBoxPRE.PerformLayout();
            this.groupBoxPOST.ResumeLayout(false);
            this.groupBoxPOST.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPresenceDetector)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMachineReadyFromPRE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBoardAvailableToPRE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBoardAvailableToPOST)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMachineReadyFromPOST)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxPRE;
        private BaseButton btnMaterialCheck;
        private System.Windows.Forms.Label labelBoardAvailablePRE;
        private System.Windows.Forms.Label labelMachineReadyPRE;
        private System.Windows.Forms.GroupBox groupBoxPOST;
        private System.Windows.Forms.Label labelBoardAvailablePOST;
        private System.Windows.Forms.Label labelMachineReadyPOST;
        private System.Windows.Forms.PictureBox pictureBoxPresenceDetector;
        private System.Windows.Forms.PictureBox pictureBoxMachineReadyFromPRE;
        private System.Windows.Forms.PictureBox pictureBoxBoardAvailableToPRE;
        private System.Windows.Forms.PictureBox pictureBoxBoardAvailableToPOST;
        private System.Windows.Forms.PictureBox pictureBoxMachineReadyFromPOST;
    }
}
