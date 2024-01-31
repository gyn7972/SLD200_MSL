namespace QMC.Common.UI
{
    partial class LineSearchResultControl
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
            this.baseLabel1 = new QMC.Common.UI.BaseLabel();
            this.baseLabel2 = new QMC.Common.UI.BaseLabel();
            this.baseButtonLineSearch = new QMC.Common.UI.BaseButton();
            this.baseTextBoxResultCenterX = new QMC.Common.UI.BaseTextBox();
            this.baseTextBoxResultCenterY = new QMC.Common.UI.BaseTextBox();
            this.baseGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // baseGroupBox1
            // 
            this.baseGroupBox1.Controls.Add(this.baseTextBoxResultCenterY);
            this.baseGroupBox1.Controls.Add(this.baseTextBoxResultCenterX);
            this.baseGroupBox1.Controls.Add(this.baseButtonLineSearch);
            this.baseGroupBox1.Controls.Add(this.baseLabel2);
            this.baseGroupBox1.Controls.Add(this.baseLabel1);
            this.baseGroupBox1.ForeColor = System.Drawing.Color.White;
            this.baseGroupBox1.Location = new System.Drawing.Point(3, 3);
            this.baseGroupBox1.Name = "baseGroupBox1";
            this.baseGroupBox1.Size = new System.Drawing.Size(294, 86);
            this.baseGroupBox1.TabIndex = 0;
            this.baseGroupBox1.TabStop = false;
            this.baseGroupBox1.Text = "baseGroupBox1";
            // 
            // baseLabel1
            // 
            this.baseLabel1.AutoSize = true;
            this.baseLabel1.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabel1.ForeColor = System.Drawing.Color.White;
            this.baseLabel1.Location = new System.Drawing.Point(7, 21);
            this.baseLabel1.Name = "baseLabel1";
            this.baseLabel1.Size = new System.Drawing.Size(97, 16);
            this.baseLabel1.TabIndex = 0;
            this.baseLabel1.Text = "Center X : ";
            // 
            // baseLabel2
            // 
            this.baseLabel2.AutoSize = true;
            this.baseLabel2.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabel2.ForeColor = System.Drawing.Color.White;
            this.baseLabel2.Location = new System.Drawing.Point(6, 50);
            this.baseLabel2.Name = "baseLabel2";
            this.baseLabel2.Size = new System.Drawing.Size(96, 16);
            this.baseLabel2.TabIndex = 1;
            this.baseLabel2.Text = "Center Y : ";
            // 
            // baseButtonLineSearch
            // 
            this.baseButtonLineSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonLineSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonLineSearch.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonLineSearch.Location = new System.Drawing.Point(200, 20);
            this.baseButtonLineSearch.Name = "baseButtonLineSearch";
            this.baseButtonLineSearch.Size = new System.Drawing.Size(88, 58);
            this.baseButtonLineSearch.TabIndex = 2;
            this.baseButtonLineSearch.Text = "Search";
            this.baseButtonLineSearch.UseVisualStyleBackColor = false;
            // 
            // baseTextBoxResultCenterX
            // 
            this.baseTextBoxResultCenterX.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxResultCenterX.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxResultCenterX.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxResultCenterX.Location = new System.Drawing.Point(100, 25);
            this.baseTextBoxResultCenterX.Name = "baseTextBoxResultCenterX";
            this.baseTextBoxResultCenterX.Size = new System.Drawing.Size(94, 14);
            this.baseTextBoxResultCenterX.TabIndex = 3;
            // 
            // baseTextBoxResultCenterY
            // 
            this.baseTextBoxResultCenterY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxResultCenterY.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxResultCenterY.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxResultCenterY.Location = new System.Drawing.Point(100, 54);
            this.baseTextBoxResultCenterY.Name = "baseTextBoxResultCenterY";
            this.baseTextBoxResultCenterY.Size = new System.Drawing.Size(94, 14);
            this.baseTextBoxResultCenterY.TabIndex = 4;
            // 
            // LineSearchResultControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseGroupBox1);
            this.Name = "LineSearchResultControl";
            this.Size = new System.Drawing.Size(300, 94);
            this.baseGroupBox1.ResumeLayout(false);
            this.baseGroupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private BaseGroupBox baseGroupBox1;
        private BaseTextBox baseTextBoxResultCenterY;
        private BaseTextBox baseTextBoxResultCenterX;
        private BaseButton baseButtonLineSearch;
        private BaseLabel baseLabel2;
        private BaseLabel baseLabel1;
    }
}
