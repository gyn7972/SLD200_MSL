namespace QMC.Common.UI
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
            this.baseGroupBoxAutoScale = new QMC.Common.UI.BaseGroupBox();
            this.baseTextBoxY = new QMC.Common.UI.BaseTextBox();
            this.baseLabel1 = new QMC.Common.UI.BaseLabel();
            this.baseButtonAlign = new QMC.Common.UI.BaseButton();
            this.baseTextBoxX = new QMC.Common.UI.BaseTextBox();
            this.baseLabelXValue = new QMC.Common.UI.BaseLabel();
            this.baseTextBox1 = new QMC.Common.UI.BaseTextBox();
            this.baseLabel2 = new QMC.Common.UI.BaseLabel();
            this.baseGroupBox1 = new QMC.Common.UI.BaseGroupBox();
            this.baseGroupBox2 = new QMC.Common.UI.BaseGroupBox();
            this.baseGroupBoxAutoScale.SuspendLayout();
            this.baseGroupBox1.SuspendLayout();
            this.baseGroupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // baseGroupBoxAutoScale
            // 
            this.baseGroupBoxAutoScale.Controls.Add(this.baseGroupBox2);
            this.baseGroupBoxAutoScale.Controls.Add(this.baseGroupBox1);
            this.baseGroupBoxAutoScale.Controls.Add(this.baseButtonAlign);
            this.baseGroupBoxAutoScale.ForeColor = System.Drawing.Color.White;
            this.baseGroupBoxAutoScale.Location = new System.Drawing.Point(0, 0);
            this.baseGroupBoxAutoScale.Name = "baseGroupBoxAutoScale";
            this.baseGroupBoxAutoScale.Size = new System.Drawing.Size(300, 180);
            this.baseGroupBoxAutoScale.TabIndex = 1;
            this.baseGroupBoxAutoScale.TabStop = false;
            this.baseGroupBoxAutoScale.Text = "Auto Scale";
            // 
            // baseTextBoxY
            // 
            this.baseTextBoxY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxY.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxY.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseTextBoxY.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxY.Location = new System.Drawing.Point(75, 55);
            this.baseTextBoxY.Name = "baseTextBoxY";
            this.baseTextBoxY.ReadOnly = true;
            this.baseTextBoxY.Size = new System.Drawing.Size(100, 19);
            this.baseTextBoxY.TabIndex = 4;
            // 
            // baseLabel1
            // 
            this.baseLabel1.AutoSize = true;
            this.baseLabel1.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabel1.ForeColor = System.Drawing.Color.White;
            this.baseLabel1.Location = new System.Drawing.Point(19, 58);
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
            this.baseButtonAlign.Location = new System.Drawing.Point(229, 18);
            this.baseButtonAlign.Name = "baseButtonAlign";
            this.baseButtonAlign.Size = new System.Drawing.Size(61, 155);
            this.baseButtonAlign.TabIndex = 2;
            this.baseButtonAlign.Text = "Auto Scale";
            this.baseButtonAlign.UseVisualStyleBackColor = false;
            this.baseButtonAlign.Click += new System.EventHandler(this.baseButtonAlign_Click);
            // 
            // baseTextBoxX
            // 
            this.baseTextBoxX.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxX.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxX.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseTextBoxX.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxX.Location = new System.Drawing.Point(75, 23);
            this.baseTextBoxX.Name = "baseTextBoxX";
            this.baseTextBoxX.ReadOnly = true;
            this.baseTextBoxX.Size = new System.Drawing.Size(100, 19);
            this.baseTextBoxX.TabIndex = 1;
            // 
            // baseLabelXValue
            // 
            this.baseLabelXValue.AutoSize = true;
            this.baseLabelXValue.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelXValue.ForeColor = System.Drawing.Color.White;
            this.baseLabelXValue.Location = new System.Drawing.Point(19, 26);
            this.baseLabelXValue.Name = "baseLabelXValue";
            this.baseLabelXValue.Size = new System.Drawing.Size(31, 16);
            this.baseLabelXValue.TabIndex = 0;
            this.baseLabelXValue.Text = "X :";
            // 
            // baseTextBox1
            // 
            this.baseTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBox1.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseTextBox1.ForeColor = System.Drawing.Color.White;
            this.baseTextBox1.Location = new System.Drawing.Point(75, 22);
            this.baseTextBox1.Name = "baseTextBox1";
            this.baseTextBox1.Size = new System.Drawing.Size(100, 19);
            this.baseTextBox1.TabIndex = 6;
            // 
            // baseLabel2
            // 
            this.baseLabel2.AutoSize = true;
            this.baseLabel2.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabel2.ForeColor = System.Drawing.Color.White;
            this.baseLabel2.Location = new System.Drawing.Point(11, 22);
            this.baseLabel2.Name = "baseLabel2";
            this.baseLabel2.Size = new System.Drawing.Size(66, 16);
            this.baseLabel2.TabIndex = 5;
            this.baseLabel2.Text = "Pitch : ";
            // 
            // baseGroupBox1
            // 
            this.baseGroupBox1.Controls.Add(this.baseTextBox1);
            this.baseGroupBox1.Controls.Add(this.baseLabel2);
            this.baseGroupBox1.ForeColor = System.Drawing.Color.White;
            this.baseGroupBox1.Location = new System.Drawing.Point(18, 18);
            this.baseGroupBox1.Name = "baseGroupBox1";
            this.baseGroupBox1.Size = new System.Drawing.Size(193, 57);
            this.baseGroupBox1.TabIndex = 7;
            this.baseGroupBox1.TabStop = false;
            this.baseGroupBox1.Text = "Move Pitch";
            // 
            // baseGroupBox2
            // 
            this.baseGroupBox2.Controls.Add(this.baseLabelXValue);
            this.baseGroupBox2.Controls.Add(this.baseTextBoxX);
            this.baseGroupBox2.Controls.Add(this.baseTextBoxY);
            this.baseGroupBox2.Controls.Add(this.baseLabel1);
            this.baseGroupBox2.ForeColor = System.Drawing.Color.White;
            this.baseGroupBox2.Location = new System.Drawing.Point(18, 81);
            this.baseGroupBox2.Name = "baseGroupBox2";
            this.baseGroupBox2.Size = new System.Drawing.Size(200, 92);
            this.baseGroupBox2.TabIndex = 8;
            this.baseGroupBox2.TabStop = false;
            this.baseGroupBox2.Text = "Result ";
            // 
            // AutoScaleControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseGroupBoxAutoScale);
            this.Name = "AutoScaleControl";
            this.Size = new System.Drawing.Size(300, 180);
            this.baseGroupBoxAutoScale.ResumeLayout(false);
            this.baseGroupBox1.ResumeLayout(false);
            this.baseGroupBox1.PerformLayout();
            this.baseGroupBox2.ResumeLayout(false);
            this.baseGroupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private BaseGroupBox baseGroupBoxAutoScale;
        private BaseTextBox baseTextBoxY;
        private BaseLabel baseLabel1;
        private BaseButton baseButtonAlign;
        private BaseTextBox baseTextBoxX;
        private BaseLabel baseLabelXValue;
        private BaseGroupBox baseGroupBox2;
        private BaseGroupBox baseGroupBox1;
        private BaseTextBox baseTextBox1;
        private BaseLabel baseLabel2;
    }
}
