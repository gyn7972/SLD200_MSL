namespace SLD200_MSL
{
    partial class ConveyorStopper
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
            this.groupBoxStopper = new System.Windows.Forms.GroupBox();
            this.StopperUp = new SLD200_MSL.BaseButton();
            this.StopperDown = new SLD200_MSL.BaseButton();
            this.groupBoxCarrierLock = new System.Windows.Forms.GroupBox();
            this.CarrierLock = new SLD200_MSL.BaseButton();
            this.CarrierUnlock = new SLD200_MSL.BaseButton();
            this.pictureBoxPresenceDetector = new System.Windows.Forms.PictureBox();
            this.btnMaterialCheck = new SLD200_MSL.BaseButton();
            this.groupBoxStopper.SuspendLayout();
            this.groupBoxCarrierLock.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPresenceDetector)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxStopper
            // 
            this.groupBoxStopper.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.groupBoxStopper.Controls.Add(this.pictureBoxPresenceDetector);
            this.groupBoxStopper.Controls.Add(this.StopperUp);
            this.groupBoxStopper.Controls.Add(this.btnMaterialCheck);
            this.groupBoxStopper.Controls.Add(this.StopperDown);
            this.groupBoxStopper.ForeColor = System.Drawing.Color.White;
            this.groupBoxStopper.Location = new System.Drawing.Point(3, 3);
            this.groupBoxStopper.Name = "groupBoxStopper";
            this.groupBoxStopper.Size = new System.Drawing.Size(190, 125);
            this.groupBoxStopper.TabIndex = 0;
            this.groupBoxStopper.TabStop = false;
            this.groupBoxStopper.Text = " Stopper ";
            // 
            // StopperUp
            // 
            this.StopperUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.StopperUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StopperUp.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.StopperUp.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.StopperUp.Location = new System.Drawing.Point(111, 27);
            this.StopperUp.Name = "StopperUp";
            this.StopperUp.Size = new System.Drawing.Size(70, 34);
            this.StopperUp.TabIndex = 3;
            this.StopperUp.Text = "Up";
            this.StopperUp.UseVisualStyleBackColor = false;
            this.StopperUp.Click += new System.EventHandler(this.StopperUp_Click);
            // 
            // StopperDown
            // 
            this.StopperDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.StopperDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StopperDown.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.StopperDown.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.StopperDown.Location = new System.Drawing.Point(111, 77);
            this.StopperDown.Name = "StopperDown";
            this.StopperDown.Size = new System.Drawing.Size(70, 34);
            this.StopperDown.TabIndex = 2;
            this.StopperDown.Text = "Down";
            this.StopperDown.UseVisualStyleBackColor = false;
            this.StopperDown.Click += new System.EventHandler(this.StopperDown_Click);
            // 
            // groupBoxCarrierLock
            // 
            this.groupBoxCarrierLock.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.groupBoxCarrierLock.Controls.Add(this.CarrierLock);
            this.groupBoxCarrierLock.Controls.Add(this.CarrierUnlock);
            this.groupBoxCarrierLock.ForeColor = System.Drawing.Color.White;
            this.groupBoxCarrierLock.Location = new System.Drawing.Point(199, 3);
            this.groupBoxCarrierLock.Name = "groupBoxCarrierLock";
            this.groupBoxCarrierLock.Size = new System.Drawing.Size(152, 125);
            this.groupBoxCarrierLock.TabIndex = 4;
            this.groupBoxCarrierLock.TabStop = false;
            this.groupBoxCarrierLock.Text = " Carrier ";
            // 
            // CarrierLock
            // 
            this.CarrierLock.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.CarrierLock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CarrierLock.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.CarrierLock.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.CarrierLock.Location = new System.Drawing.Point(10, 27);
            this.CarrierLock.Name = "CarrierLock";
            this.CarrierLock.Size = new System.Drawing.Size(133, 34);
            this.CarrierLock.TabIndex = 3;
            this.CarrierLock.Text = "Clamp Up (Lock)";
            this.CarrierLock.UseVisualStyleBackColor = false;
            this.CarrierLock.Click += new System.EventHandler(this.CarrierLock_Click);
            // 
            // CarrierUnlock
            // 
            this.CarrierUnlock.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.CarrierUnlock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CarrierUnlock.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.CarrierUnlock.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.CarrierUnlock.Location = new System.Drawing.Point(10, 77);
            this.CarrierUnlock.Name = "CarrierUnlock";
            this.CarrierUnlock.Size = new System.Drawing.Size(133, 34);
            this.CarrierUnlock.TabIndex = 2;
            this.CarrierUnlock.Text = "Clamp Down";
            this.CarrierUnlock.UseVisualStyleBackColor = false;
            this.CarrierUnlock.Click += new System.EventHandler(this.CarrierUnlock_Click);
            // 
            // pictureBoxPresenceDetector
            // 
            this.pictureBoxPresenceDetector.Image = global::SLD200.Properties.Resources.DioEllipseOff;
            this.pictureBoxPresenceDetector.Location = new System.Drawing.Point(10, 67);
            this.pictureBoxPresenceDetector.Name = "pictureBoxPresenceDetector";
            this.pictureBoxPresenceDetector.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxPresenceDetector.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxPresenceDetector.TabIndex = 27;
            this.pictureBoxPresenceDetector.TabStop = false;
            // 
            // btnMaterialCheck
            // 
            this.btnMaterialCheck.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.btnMaterialCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMaterialCheck.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnMaterialCheck.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnMaterialCheck.Location = new System.Drawing.Point(10, 27);
            this.btnMaterialCheck.Name = "btnMaterialCheck";
            this.btnMaterialCheck.Size = new System.Drawing.Size(81, 34);
            this.btnMaterialCheck.TabIndex = 26;
            this.btnMaterialCheck.Text = "Material";
            this.btnMaterialCheck.UseVisualStyleBackColor = false;
            // 
            // ConveyorStopper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.Controls.Add(this.groupBoxCarrierLock);
            this.Controls.Add(this.groupBoxStopper);
            this.Name = "ConveyorStopper";
            this.Size = new System.Drawing.Size(354, 130);
            this.groupBoxStopper.ResumeLayout(false);
            this.groupBoxCarrierLock.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPresenceDetector)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxStopper;
        private BaseButton StopperUp;
        private BaseButton StopperDown;
        private System.Windows.Forms.GroupBox groupBoxCarrierLock;
        private BaseButton CarrierLock;
        private BaseButton CarrierUnlock;
        private System.Windows.Forms.PictureBox pictureBoxPresenceDetector;
        private BaseButton btnMaterialCheck;
    }
}
