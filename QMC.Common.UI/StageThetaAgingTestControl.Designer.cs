namespace QMC.Common.UI
{
    partial class StageThetaAgingTestControl
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
            this.baseButtonRun = new BaseButton();
            this.baseGroupBox1 = new BaseGroupBox();
            this.basePropertyGrid1 = new BasePropertyGrid();
            this.baseGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // baseButtonRun
            // 
            this.baseButtonRun.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonRun.FlatAppearance.BorderSize = 0;
            this.baseButtonRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonRun.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonRun.Location = new System.Drawing.Point(4, 287);
            this.baseButtonRun.Name = "baseButtonRun";
            this.baseButtonRun.Size = new System.Drawing.Size(283, 32);
            this.baseButtonRun.TabIndex = 0;
            this.baseButtonRun.Text = "Run";
            this.baseButtonRun.UseVisualStyleBackColor = false;
            this.baseButtonRun.Click += new System.EventHandler(this.baseButtonRun_Click);
            // 
            // baseGroupBox1
            // 
            this.baseGroupBox1.Controls.Add(this.basePropertyGrid1);
            this.baseGroupBox1.Controls.Add(this.baseButtonRun);
            this.baseGroupBox1.ForeColor = System.Drawing.Color.White;
            this.baseGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.baseGroupBox1.Name = "baseGroupBox1";
            this.baseGroupBox1.Size = new System.Drawing.Size(290, 325);
            this.baseGroupBox1.TabIndex = 0;
            this.baseGroupBox1.TabStop = false;
            this.baseGroupBox1.Text = "Stage Theta Aging Test";
            // 
            // basePropertyGrid1
            // 
            this.basePropertyGrid1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.basePropertyGrid1.CategoryForeColor = System.Drawing.Color.Black;
            this.basePropertyGrid1.CategorySplitterColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.basePropertyGrid1.CommandsBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.basePropertyGrid1.HelpBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.basePropertyGrid1.HelpForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.basePropertyGrid1.Location = new System.Drawing.Point(4, 17);
            this.basePropertyGrid1.Name = "basePropertyGrid1";
            this.basePropertyGrid1.SelectedItemWithFocusBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.basePropertyGrid1.Size = new System.Drawing.Size(282, 264);
            this.basePropertyGrid1.TabIndex = 1;
            this.basePropertyGrid1.ViewBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.basePropertyGrid1.ViewBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            // 
            // StageThetaAgingTestControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseGroupBox1);
            this.Name = "StageThetaAgingTestControl";
            this.Size = new System.Drawing.Size(290, 325);
            this.baseGroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private BaseButton baseButtonRun;
        private BaseGroupBox baseGroupBox1;
        private BasePropertyGrid basePropertyGrid1;
    }
}
