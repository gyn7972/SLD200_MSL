namespace SLD200_MSL
{
    partial class JigAlignerControl
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
            this.baseGroupBox1 = new SLD200_MSL.WATGroupBox();
            this.baseTextBoxResult = new SLD200_MSL.BaseTextBox();
            this.baseButtonRun = new SLD200_MSL.BaseButton();
            this.baseLabel1 = new SLD200_MSL.BaseLabel();
            this.baseGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // baseGroupBox1
            // 
            this.baseGroupBox1.BorderColor = System.Drawing.Color.Black;
            this.baseGroupBox1.Controls.Add(this.baseTextBoxResult);
            this.baseGroupBox1.Controls.Add(this.baseButtonRun);
            this.baseGroupBox1.Controls.Add(this.baseLabel1);
            this.baseGroupBox1.Font = new System.Drawing.Font("Tahoma", 9F);
            this.baseGroupBox1.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.baseGroupBox1.Name = "baseGroupBox1";
            this.baseGroupBox1.Size = new System.Drawing.Size(306, 54);
            this.baseGroupBox1.TabIndex = 0;
            this.baseGroupBox1.TabStop = false;
            this.baseGroupBox1.Text = "baseGroupBox1";
            // 
            // baseTextBoxResult
            // 
            this.baseTextBoxResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseTextBoxResult.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxResult.Location = new System.Drawing.Point(80, 22);
            this.baseTextBoxResult.Name = "baseTextBoxResult";
            this.baseTextBoxResult.Size = new System.Drawing.Size(119, 22);
            this.baseTextBoxResult.TabIndex = 2;
            // 
            // baseButtonRun
            // 
            this.baseButtonRun.BackColor = System.Drawing.Color.White;
            this.baseButtonRun.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.baseButtonRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonRun.ForeColor = System.Drawing.Color.Black;
            this.baseButtonRun.Location = new System.Drawing.Point(217, 20);
            this.baseButtonRun.Name = "baseButtonRun";
            this.baseButtonRun.Size = new System.Drawing.Size(81, 26);
            this.baseButtonRun.TabIndex = 1;
            this.baseButtonRun.Text = "Run";
            this.baseButtonRun.UseVisualStyleBackColor = false;
            this.baseButtonRun.Click += new System.EventHandler(this.baseButtonRun_Click);
            // 
            // baseLabel1
            // 
            this.baseLabel1.AutoSize = true;
            this.baseLabel1.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.baseLabel1.ForeColor = System.Drawing.Color.Black;
            this.baseLabel1.Location = new System.Drawing.Point(10, 23);
            this.baseLabel1.Name = "baseLabel1";
            this.baseLabel1.Size = new System.Drawing.Size(63, 18);
            this.baseLabel1.TabIndex = 0;
            this.baseLabel1.Text = "Angle : ";
            // 
            // JigAlignerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseGroupBox1);
            this.Name = "JigAlignerControl";
            this.Size = new System.Drawing.Size(310, 58);
            this.baseGroupBox1.ResumeLayout(false);
            this.baseGroupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        //private SLD200_MSL.BaseGroupBox baseGroupBox1;
        private WATGroupBox baseGroupBox1;
        private SLD200_MSL.BaseTextBox baseTextBoxResult;
        private SLD200_MSL.BaseButton baseButtonRun;
        private SLD200_MSL.BaseLabel baseLabel1;
    }
}
