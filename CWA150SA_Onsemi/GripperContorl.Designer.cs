namespace CWA150SA_Onsemi300
{
    partial class GripperContorl
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
            this.baseGripperGroupBox = new CWA150SA_Onsemi300.BaseGroupBox();
            this.baseToggleButtonRelease = new CWA150SA_Onsemi300.BaseToggleButton();
            this.baseToggleButtonHold = new CWA150SA_Onsemi300.BaseToggleButton();
            this.baseGripperGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // baseGripperGroupBox
            // 
            this.baseGripperGroupBox.Controls.Add(this.baseToggleButtonRelease);
            this.baseGripperGroupBox.Controls.Add(this.baseToggleButtonHold);
            this.baseGripperGroupBox.ForeColor = System.Drawing.Color.White;
            this.baseGripperGroupBox.Location = new System.Drawing.Point(4, 3);
            this.baseGripperGroupBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseGripperGroupBox.Name = "baseGripperGroupBox";
            this.baseGripperGroupBox.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseGripperGroupBox.Size = new System.Drawing.Size(251, 99);
            this.baseGripperGroupBox.TabIndex = 0;
            this.baseGripperGroupBox.TabStop = false;
            this.baseGripperGroupBox.Text = "baseGroupBox1";
            // 
            // baseToggleButtonRelease
            // 
            this.baseToggleButtonRelease.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseToggleButtonRelease.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButtonRelease.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseToggleButtonRelease.Location = new System.Drawing.Point(133, 27);
            this.baseToggleButtonRelease.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseToggleButtonRelease.Name = "baseToggleButtonRelease";
            this.baseToggleButtonRelease.Size = new System.Drawing.Size(101, 40);
            this.baseToggleButtonRelease.TabIndex = 3;
            this.baseToggleButtonRelease.Text = "Release";
            this.baseToggleButtonRelease.UseVisualStyleBackColor = false;
            this.baseToggleButtonRelease.Click += new System.EventHandler(this.baseToggleButtonRelease_Click);
            // 
            // baseToggleButtonHold
            // 
            this.baseToggleButtonHold.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseToggleButtonHold.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButtonHold.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseToggleButtonHold.Location = new System.Drawing.Point(18, 27);
            this.baseToggleButtonHold.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseToggleButtonHold.Name = "baseToggleButtonHold";
            this.baseToggleButtonHold.Size = new System.Drawing.Size(101, 40);
            this.baseToggleButtonHold.TabIndex = 2;
            this.baseToggleButtonHold.Text = "Hold";
            this.baseToggleButtonHold.UseVisualStyleBackColor = false;
            this.baseToggleButtonHold.Click += new System.EventHandler(this.baseToggleButtonHold_Click);
            // 
            // GripperContorl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseGripperGroupBox);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "GripperContorl";
            this.Size = new System.Drawing.Size(258, 105);
            this.baseGripperGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private BaseGroupBox baseGripperGroupBox;
        private BaseToggleButton baseToggleButtonRelease;
        private BaseToggleButton baseToggleButtonHold;
    }
}
