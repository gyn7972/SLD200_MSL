namespace CWA150SA_Onsemi
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
            this.baseGroupBox1 = new CWA150SA_Onsemi300.BaseGroupBox();
            this.baseTextBoxResult = new CWA150SA_Onsemi300.BaseTextBox();
            this.baseButtonRun = new CWA150SA_Onsemi300.BaseButton();
            this.baseLabel1 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // baseGroupBox1
            // 
            this.baseGroupBox1.Controls.Add(this.baseTextBoxResult);
            this.baseGroupBox1.Controls.Add(this.baseButtonRun);
            this.baseGroupBox1.Controls.Add(this.baseLabel1);
            this.baseGroupBox1.ForeColor = System.Drawing.Color.White;
            this.baseGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.baseGroupBox1.Name = "baseGroupBox1";
            this.baseGroupBox1.Size = new System.Drawing.Size(310, 54);
            this.baseGroupBox1.TabIndex = 0;
            this.baseGroupBox1.TabStop = false;
            this.baseGroupBox1.Text = "baseGroupBox1";
            // 
            // baseTextBoxResult
            // 
            this.baseTextBoxResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseTextBoxResult.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxResult.Location = new System.Drawing.Point(86, 21);
            this.baseTextBoxResult.Name = "baseTextBoxResult";
            this.baseTextBoxResult.Size = new System.Drawing.Size(119, 21);
            this.baseTextBoxResult.TabIndex = 2;
            // 
            // baseButtonRun
            // 
            this.baseButtonRun.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonRun.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonRun.Location = new System.Drawing.Point(218, 19);
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
            this.baseLabel1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabel1.ForeColor = System.Drawing.Color.White;
            this.baseLabel1.Location = new System.Drawing.Point(10, 20);
            this.baseLabel1.Name = "baseLabel1";
            this.baseLabel1.Size = new System.Drawing.Size(67, 19);
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

        private CWA150SA_Onsemi300.BaseGroupBox baseGroupBox1;
        private CWA150SA_Onsemi300.BaseTextBox baseTextBoxResult;
        private CWA150SA_Onsemi300.BaseButton baseButtonRun;
        private CWA150SA_Onsemi300.BaseLabel baseLabel1;
    }
}
