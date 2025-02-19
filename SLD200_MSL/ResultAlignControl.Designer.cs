namespace SLD200_MSL
{
    partial class ResultAlignControl
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
            this.baseGroupBoxResultAlign = new BaseGroupBox();
            this.baseButtonAlign = new BaseButton();
            this.baseTextBoxAlign = new BaseTextBox();
            this.baseLabelAlign = new BaseLabel();
            this.baseGroupBoxResultAlign.SuspendLayout();
            this.SuspendLayout();
            // 
            // baseGroupBoxResultAlign
            // 
            this.baseGroupBoxResultAlign.Controls.Add(this.baseButtonAlign);
            this.baseGroupBoxResultAlign.Controls.Add(this.baseTextBoxAlign);
            this.baseGroupBoxResultAlign.Controls.Add(this.baseLabelAlign);
            this.baseGroupBoxResultAlign.ForeColor = System.Drawing.Color.White;
            this.baseGroupBoxResultAlign.Location = new System.Drawing.Point(0, 0);
            this.baseGroupBoxResultAlign.Name = "baseGroupBoxResultAlign";
            this.baseGroupBoxResultAlign.Size = new System.Drawing.Size(300, 54);
            this.baseGroupBoxResultAlign.TabIndex = 0;
            this.baseGroupBoxResultAlign.TabStop = false;
            this.baseGroupBoxResultAlign.Text = "Result Align";
            // 
            // baseButtonAlign
            // 
            this.baseButtonAlign.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonAlign.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonAlign.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonAlign.Location = new System.Drawing.Point(194, 17);
            this.baseButtonAlign.Name = "baseButtonAlign";
            this.baseButtonAlign.Size = new System.Drawing.Size(100, 30);
            this.baseButtonAlign.TabIndex = 2;
            this.baseButtonAlign.Text = "Align";
            this.baseButtonAlign.UseVisualStyleBackColor = false;
            this.baseButtonAlign.Click += new System.EventHandler(this.baseButtonAlign_Click);
            // 
            // baseTextBoxAlign
            // 
            this.baseTextBoxAlign.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxAlign.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxAlign.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseTextBoxAlign.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxAlign.Location = new System.Drawing.Point(69, 22);
            this.baseTextBoxAlign.Name = "baseTextBoxAlign";
            this.baseTextBoxAlign.Size = new System.Drawing.Size(100, 19);
            this.baseTextBoxAlign.TabIndex = 1;
            // 
            // baseLabelAlign
            // 
            this.baseLabelAlign.AutoSize = true;
            this.baseLabelAlign.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelAlign.ForeColor = System.Drawing.Color.White;
            this.baseLabelAlign.Location = new System.Drawing.Point(6, 24);
            this.baseLabelAlign.Name = "baseLabelAlign";
            this.baseLabelAlign.Size = new System.Drawing.Size(58, 16);
            this.baseLabelAlign.TabIndex = 0;
            this.baseLabelAlign.Text = "Align :";
            // 
            // ResultAlignControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseGroupBoxResultAlign);
            this.Name = "ResultAlignControl";
            this.Size = new System.Drawing.Size(300, 54);
            this.baseGroupBoxResultAlign.ResumeLayout(false);
            this.baseGroupBoxResultAlign.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private BaseGroupBox baseGroupBoxResultAlign;
        private BaseButton baseButtonAlign;
        private BaseTextBox baseTextBoxAlign;
        private BaseLabel baseLabelAlign;
    }
}
