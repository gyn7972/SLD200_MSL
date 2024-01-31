namespace QMC.Common.UI
{
    partial class CarrierLockControl
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
            this.baseGroupBox1 = new QMC.Common.UI.BaseGroupBox();
            this.baseToggleButtonUnlock = new QMC.Common.UI.BaseToggleButton();
            this.baseToggleButtonLock = new QMC.Common.UI.BaseToggleButton();
            this.baseGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // baseGroupBox1
            // 
            this.baseGroupBox1.Controls.Add(this.baseToggleButtonUnlock);
            this.baseGroupBox1.Controls.Add(this.baseToggleButtonLock);
            this.baseGroupBox1.ForeColor = System.Drawing.Color.White;
            this.baseGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.baseGroupBox1.Name = "baseGroupBox1";
            this.baseGroupBox1.Size = new System.Drawing.Size(235, 61);
            this.baseGroupBox1.TabIndex = 0;
            this.baseGroupBox1.TabStop = false;
            this.baseGroupBox1.Text = "Carrier Lock";
            // 
            // baseToggleButtonUnlock
            // 
            this.baseToggleButtonUnlock.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseToggleButtonUnlock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButtonUnlock.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseToggleButtonUnlock.Location = new System.Drawing.Point(121, 20);
            this.baseToggleButtonUnlock.Name = "baseToggleButtonUnlock";
            this.baseToggleButtonUnlock.Size = new System.Drawing.Size(100, 30);
            this.baseToggleButtonUnlock.TabIndex = 1;
            this.baseToggleButtonUnlock.Text = "Unlock";
            this.baseToggleButtonUnlock.UseVisualStyleBackColor = false;
            this.baseToggleButtonUnlock.Click += new System.EventHandler(this.baseToggleButtonUnlock_Click);
            // 
            // baseToggleButtonLock
            // 
            this.baseToggleButtonLock.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseToggleButtonLock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButtonLock.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseToggleButtonLock.Location = new System.Drawing.Point(15, 20);
            this.baseToggleButtonLock.Name = "baseToggleButtonLock";
            this.baseToggleButtonLock.Size = new System.Drawing.Size(100, 30);
            this.baseToggleButtonLock.TabIndex = 0;
            this.baseToggleButtonLock.Text = "Lock";
            this.baseToggleButtonLock.UseVisualStyleBackColor = false;
            this.baseToggleButtonLock.Click += new System.EventHandler(this.baseToggleButtonLock_Click);
            // 
            // CarrierLockControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseGroupBox1);
            this.Name = "CarrierLockControl";
            this.Size = new System.Drawing.Size(235, 61);
            this.baseGroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private BaseGroupBox baseGroupBox1;
        private BaseToggleButton baseToggleButtonUnlock;
        private BaseToggleButton baseToggleButtonLock;
    }
}
