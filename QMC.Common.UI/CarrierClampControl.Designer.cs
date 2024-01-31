namespace QMC.Common.UI
{
    partial class CarrierClampControl
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
            this.baseToggleButtonUnclamp = new QMC.Common.UI.BaseToggleButton();
            this.baseToggleButtonClamp = new QMC.Common.UI.BaseToggleButton();
            this.baseGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // baseGroupBox1
            // 
            this.baseGroupBox1.Controls.Add(this.baseToggleButtonUnclamp);
            this.baseGroupBox1.Controls.Add(this.baseToggleButtonClamp);
            this.baseGroupBox1.ForeColor = System.Drawing.Color.White;
            this.baseGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.baseGroupBox1.Name = "baseGroupBox1";
            this.baseGroupBox1.Size = new System.Drawing.Size(227, 62);
            this.baseGroupBox1.TabIndex = 0;
            this.baseGroupBox1.TabStop = false;
            this.baseGroupBox1.Text = "Clamper";
            // 
            // baseToggleButtonUnclamp
            // 
            this.baseToggleButtonUnclamp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseToggleButtonUnclamp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButtonUnclamp.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseToggleButtonUnclamp.Location = new System.Drawing.Point(123, 21);
            this.baseToggleButtonUnclamp.Name = "baseToggleButtonUnclamp";
            this.baseToggleButtonUnclamp.Size = new System.Drawing.Size(100, 30);
            this.baseToggleButtonUnclamp.TabIndex = 1;
            this.baseToggleButtonUnclamp.Text = "Unclamp";
            this.baseToggleButtonUnclamp.UseVisualStyleBackColor = false;
            this.baseToggleButtonUnclamp.Click += new System.EventHandler(this.baseToggleButtonUnclamp_Click);
            // 
            // baseToggleButtonClamp
            // 
            this.baseToggleButtonClamp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseToggleButtonClamp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButtonClamp.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseToggleButtonClamp.Location = new System.Drawing.Point(7, 21);
            this.baseToggleButtonClamp.Name = "baseToggleButtonClamp";
            this.baseToggleButtonClamp.Size = new System.Drawing.Size(100, 30);
            this.baseToggleButtonClamp.TabIndex = 0;
            this.baseToggleButtonClamp.Text = "Clamp";
            this.baseToggleButtonClamp.UseVisualStyleBackColor = false;
            this.baseToggleButtonClamp.Click += new System.EventHandler(this.baseToggleButtonClamp_Click);
            // 
            // CarrierClampControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseGroupBox1);
            this.Name = "CarrierClampControl";
            this.Size = new System.Drawing.Size(227, 65);
            this.baseGroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private BaseGroupBox baseGroupBox1;
        private BaseToggleButton baseToggleButtonUnclamp;
        private BaseToggleButton baseToggleButtonClamp;
    }
}
