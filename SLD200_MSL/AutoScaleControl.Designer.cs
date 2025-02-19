namespace SLD200_MSL
{
    partial class AutoScaleControl
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
            this.baseGroupBoxAutoScale = new SLD200_MSL.BaseGroupBox();
            this.baseTextBoxY = new SLD200_MSL.BaseTextBox();
            this.baseLabel1 = new SLD200_MSL.BaseLabel();
            this.baseButtonAlign = new SLD200_MSL.BaseButton();
            this.baseTextBoxX = new SLD200_MSL.BaseTextBox();
            this.baseLabelXValue = new SLD200_MSL.BaseLabel();
            this.baseGroupBoxAutoScale.SuspendLayout();
            this.SuspendLayout();
            // 
            // baseGroupBoxAutoScale
            // 
            this.baseGroupBoxAutoScale.Controls.Add(this.baseTextBoxY);
            this.baseGroupBoxAutoScale.Controls.Add(this.baseLabel1);
            this.baseGroupBoxAutoScale.Controls.Add(this.baseButtonAlign);
            this.baseGroupBoxAutoScale.Controls.Add(this.baseTextBoxX);
            this.baseGroupBoxAutoScale.Controls.Add(this.baseLabelXValue);
            this.baseGroupBoxAutoScale.ForeColor = System.Drawing.Color.White;
            this.baseGroupBoxAutoScale.Location = new System.Drawing.Point(0, 0);
            this.baseGroupBoxAutoScale.Name = "baseGroupBoxAutoScale";
            this.baseGroupBoxAutoScale.Size = new System.Drawing.Size(300, 85);
            this.baseGroupBoxAutoScale.TabIndex = 1;
            this.baseGroupBoxAutoScale.TabStop = false;
            this.baseGroupBoxAutoScale.Text = "Auto Scale";
            // 
            // baseTextBoxY
            // 
            this.baseTextBoxY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxY.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxY.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseTextBoxY.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxY.Location = new System.Drawing.Point(69, 54);
            this.baseTextBoxY.Name = "baseTextBoxY";
            this.baseTextBoxY.ReadOnly = true;
            this.baseTextBoxY.Size = new System.Drawing.Size(100, 19);
            this.baseTextBoxY.TabIndex = 4;
            // 
            // baseLabel1
            // 
            this.baseLabel1.AutoSize = true;
            this.baseLabel1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabel1.ForeColor = System.Drawing.Color.White;
            this.baseLabel1.Location = new System.Drawing.Point(25, 56);
            this.baseLabel1.Name = "baseLabel1";
            this.baseLabel1.Size = new System.Drawing.Size(30, 16);
            this.baseLabel1.TabIndex = 3;
            this.baseLabel1.Text = "Y :";
            // 
            // baseButtonAlign
            // 
            this.baseButtonAlign.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonAlign.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonAlign.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonAlign.Location = new System.Drawing.Point(194, 43);
            this.baseButtonAlign.Name = "baseButtonAlign";
            this.baseButtonAlign.Size = new System.Drawing.Size(100, 30);
            this.baseButtonAlign.TabIndex = 2;
            this.baseButtonAlign.Text = "Auto Scale";
            this.baseButtonAlign.UseVisualStyleBackColor = false;
            this.baseButtonAlign.Click += new System.EventHandler(this.baseButtonAlign_Click);
            // 
            // baseTextBoxX
            // 
            this.baseTextBoxX.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxX.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxX.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseTextBoxX.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxX.Location = new System.Drawing.Point(69, 22);
            this.baseTextBoxX.Name = "baseTextBoxX";
            this.baseTextBoxX.ReadOnly = true;
            this.baseTextBoxX.Size = new System.Drawing.Size(100, 19);
            this.baseTextBoxX.TabIndex = 1;
            // 
            // baseLabelXValue
            // 
            this.baseLabelXValue.AutoSize = true;
            this.baseLabelXValue.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelXValue.ForeColor = System.Drawing.Color.White;
            this.baseLabelXValue.Location = new System.Drawing.Point(25, 24);
            this.baseLabelXValue.Name = "baseLabelXValue";
            this.baseLabelXValue.Size = new System.Drawing.Size(31, 16);
            this.baseLabelXValue.TabIndex = 0;
            this.baseLabelXValue.Text = "X :";
            // 
            // AutoScaleControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseGroupBoxAutoScale);
            this.Name = "AutoScaleControl";
            this.Size = new System.Drawing.Size(300, 85);
            this.baseGroupBoxAutoScale.ResumeLayout(false);
            this.baseGroupBoxAutoScale.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private BaseGroupBox baseGroupBoxAutoScale;
        private BaseTextBox baseTextBoxY;
        private BaseLabel baseLabel1;
        private BaseButton baseButtonAlign;
        private BaseTextBox baseTextBoxX;
        private BaseLabel baseLabelXValue;
    }
}
