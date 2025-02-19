namespace SLD200_MSL
{
    partial class LaserPitchMoveShotterControl
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
            this.baseButtonRun = new SLD200_MSL.BaseButton();
            this.baseGroupBoxParameter = new SLD200_MSL.BaseGroupBox();
            this.basePropertyGridParameter = new SLD200_MSL.BasePropertyGrid();
            this.baseGroupBoxParameter.SuspendLayout();
            this.SuspendLayout();
            // 
            // baseButtonRun
            // 
            this.baseButtonRun.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonRun.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonRun.Location = new System.Drawing.Point(322, 226);
            this.baseButtonRun.Name = "baseButtonRun";
            this.baseButtonRun.Size = new System.Drawing.Size(100, 30);
            this.baseButtonRun.TabIndex = 11;
            this.baseButtonRun.Text = "Run";
            this.baseButtonRun.UseVisualStyleBackColor = false;
            this.baseButtonRun.Click += new System.EventHandler(this.baseButtonRun_Click);
            // 
            // baseGroupBoxParameter
            // 
            this.baseGroupBoxParameter.Controls.Add(this.baseButtonRun);
            this.baseGroupBoxParameter.Controls.Add(this.basePropertyGridParameter);
            this.baseGroupBoxParameter.ForeColor = System.Drawing.Color.White;
            this.baseGroupBoxParameter.Location = new System.Drawing.Point(3, 3);
            this.baseGroupBoxParameter.Name = "baseGroupBoxParameter";
            this.baseGroupBoxParameter.Size = new System.Drawing.Size(428, 264);
            this.baseGroupBoxParameter.TabIndex = 10;
            this.baseGroupBoxParameter.TabStop = false;
            this.baseGroupBoxParameter.Text = " Parameter ";
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
            // LaserPitchMoveShotterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseGroupBoxParameter);
            this.Name = "LaserPitchMoveShotterControl";
            this.Size = new System.Drawing.Size(440, 269);
            this.baseGroupBoxParameter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private SLD200_MSL.BaseButton baseButtonRun;
        private SLD200_MSL.BaseGroupBox baseGroupBoxParameter;
        private SLD200_MSL.BasePropertyGrid basePropertyGridParameter;
    }
}
