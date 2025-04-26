namespace SLD200_MSL
{
    partial class AlarmInfoControl
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
            this.groupBoxSelectedAlarmDetails = new System.Windows.Forms.GroupBox();
            this.baseTextBoxAlarmTitle = new SLD200_MSL.BaseTextBox();
            this.baseTextBoxCode = new SLD200_MSL.BaseTextBox();
            this.baseTextBoxGrade = new SLD200_MSL.BaseTextBox();
            this.baseTextBoxSource = new SLD200_MSL.BaseTextBox();
            this.baseTextBoxCause = new SLD200_MSL.BaseTextBox();
            this.baseLabelCode = new SLD200_MSL.BaseLabel();
            this.baseLabelCause = new SLD200_MSL.BaseLabel();
            this.baseLabelSource = new SLD200_MSL.BaseLabel();
            this.baseLabelGrade = new SLD200_MSL.BaseLabel();
            this.baseLabelAlarmTitle = new SLD200_MSL.BaseLabel();
            this.groupBoxSelectedAlarmDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxSelectedAlarmDetails
            // 
            this.groupBoxSelectedAlarmDetails.Controls.Add(this.baseTextBoxAlarmTitle);
            this.groupBoxSelectedAlarmDetails.Controls.Add(this.baseTextBoxCode);
            this.groupBoxSelectedAlarmDetails.Controls.Add(this.baseTextBoxGrade);
            this.groupBoxSelectedAlarmDetails.Controls.Add(this.baseTextBoxSource);
            this.groupBoxSelectedAlarmDetails.Controls.Add(this.baseTextBoxCause);
            this.groupBoxSelectedAlarmDetails.Controls.Add(this.baseLabelCode);
            this.groupBoxSelectedAlarmDetails.Controls.Add(this.baseLabelCause);
            this.groupBoxSelectedAlarmDetails.Controls.Add(this.baseLabelSource);
            this.groupBoxSelectedAlarmDetails.Controls.Add(this.baseLabelGrade);
            this.groupBoxSelectedAlarmDetails.Controls.Add(this.baseLabelAlarmTitle);
            this.groupBoxSelectedAlarmDetails.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.groupBoxSelectedAlarmDetails.ForeColor = System.Drawing.Color.Black;
            this.groupBoxSelectedAlarmDetails.Location = new System.Drawing.Point(10, 12);
            this.groupBoxSelectedAlarmDetails.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxSelectedAlarmDetails.Name = "groupBoxSelectedAlarmDetails";
            this.groupBoxSelectedAlarmDetails.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxSelectedAlarmDetails.Size = new System.Drawing.Size(1329, 408);
            this.groupBoxSelectedAlarmDetails.TabIndex = 5;
            this.groupBoxSelectedAlarmDetails.TabStop = false;
            this.groupBoxSelectedAlarmDetails.Text = " Selected Alarm Details ";
            // 
            // baseTextBoxAlarmTitle
            // 
            this.baseTextBoxAlarmTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.baseTextBoxAlarmTitle.BackColor = System.Drawing.Color.White;
            this.baseTextBoxAlarmTitle.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxAlarmTitle.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseTextBoxAlarmTitle.ForeColor = System.Drawing.Color.Black;
            this.baseTextBoxAlarmTitle.Location = new System.Drawing.Point(105, 68);
            this.baseTextBoxAlarmTitle.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.baseTextBoxAlarmTitle.Multiline = true;
            this.baseTextBoxAlarmTitle.Name = "baseTextBoxAlarmTitle";
            this.baseTextBoxAlarmTitle.Size = new System.Drawing.Size(1210, 49);
            this.baseTextBoxAlarmTitle.TabIndex = 19;
            // 
            // baseTextBoxCode
            // 
            this.baseTextBoxCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.baseTextBoxCode.BackColor = System.Drawing.Color.White;
            this.baseTextBoxCode.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxCode.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseTextBoxCode.ForeColor = System.Drawing.Color.Black;
            this.baseTextBoxCode.Location = new System.Drawing.Point(891, 131);
            this.baseTextBoxCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.baseTextBoxCode.Multiline = true;
            this.baseTextBoxCode.Name = "baseTextBoxCode";
            this.baseTextBoxCode.Size = new System.Drawing.Size(424, 49);
            this.baseTextBoxCode.TabIndex = 19;
            // 
            // baseTextBoxGrade
            // 
            this.baseTextBoxGrade.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.baseTextBoxGrade.BackColor = System.Drawing.Color.White;
            this.baseTextBoxGrade.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxGrade.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseTextBoxGrade.ForeColor = System.Drawing.Color.Black;
            this.baseTextBoxGrade.Location = new System.Drawing.Point(105, 131);
            this.baseTextBoxGrade.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.baseTextBoxGrade.Multiline = true;
            this.baseTextBoxGrade.Name = "baseTextBoxGrade";
            this.baseTextBoxGrade.Size = new System.Drawing.Size(612, 49);
            this.baseTextBoxGrade.TabIndex = 18;
            // 
            // baseTextBoxSource
            // 
            this.baseTextBoxSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.baseTextBoxSource.BackColor = System.Drawing.Color.White;
            this.baseTextBoxSource.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxSource.Font = new System.Drawing.Font("Tahoma", 12F);
            this.baseTextBoxSource.ForeColor = System.Drawing.Color.Black;
            this.baseTextBoxSource.Location = new System.Drawing.Point(105, 193);
            this.baseTextBoxSource.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.baseTextBoxSource.Multiline = true;
            this.baseTextBoxSource.Name = "baseTextBoxSource";
            this.baseTextBoxSource.Size = new System.Drawing.Size(1210, 82);
            this.baseTextBoxSource.TabIndex = 17;
            // 
            // baseTextBoxCause
            // 
            this.baseTextBoxCause.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.baseTextBoxCause.BackColor = System.Drawing.Color.White;
            this.baseTextBoxCause.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxCause.Font = new System.Drawing.Font("Tahoma", 12F);
            this.baseTextBoxCause.ForeColor = System.Drawing.Color.Black;
            this.baseTextBoxCause.Location = new System.Drawing.Point(105, 290);
            this.baseTextBoxCause.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.baseTextBoxCause.Multiline = true;
            this.baseTextBoxCause.Name = "baseTextBoxCause";
            this.baseTextBoxCause.Size = new System.Drawing.Size(1210, 100);
            this.baseTextBoxCause.TabIndex = 16;
            // 
            // baseLabelCode
            // 
            this.baseLabelCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.baseLabelCode.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabelCode.ForeColor = System.Drawing.Color.Black;
            this.baseLabelCode.Location = new System.Drawing.Point(780, 132);
            this.baseLabelCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabelCode.Name = "baseLabelCode";
            this.baseLabelCode.Size = new System.Drawing.Size(103, 40);
            this.baseLabelCode.TabIndex = 14;
            this.baseLabelCode.Text = "Code";
            this.baseLabelCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // baseLabelCause
            // 
            this.baseLabelCause.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.baseLabelCause.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabelCause.ForeColor = System.Drawing.Color.Black;
            this.baseLabelCause.Location = new System.Drawing.Point(4, 288);
            this.baseLabelCause.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabelCause.Name = "baseLabelCause";
            this.baseLabelCause.Size = new System.Drawing.Size(93, 40);
            this.baseLabelCause.TabIndex = 7;
            this.baseLabelCause.Text = "Cause";
            this.baseLabelCause.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // baseLabelSource
            // 
            this.baseLabelSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.baseLabelSource.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabelSource.ForeColor = System.Drawing.Color.Black;
            this.baseLabelSource.Location = new System.Drawing.Point(4, 191);
            this.baseLabelSource.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabelSource.Name = "baseLabelSource";
            this.baseLabelSource.Size = new System.Drawing.Size(93, 40);
            this.baseLabelSource.TabIndex = 6;
            this.baseLabelSource.Text = "Source";
            this.baseLabelSource.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // baseLabelGrade
            // 
            this.baseLabelGrade.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.baseLabelGrade.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabelGrade.ForeColor = System.Drawing.Color.Black;
            this.baseLabelGrade.Location = new System.Drawing.Point(4, 132);
            this.baseLabelGrade.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabelGrade.Name = "baseLabelGrade";
            this.baseLabelGrade.Size = new System.Drawing.Size(93, 40);
            this.baseLabelGrade.TabIndex = 5;
            this.baseLabelGrade.Text = "Grade";
            this.baseLabelGrade.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // baseLabelAlarmTitle
            // 
            this.baseLabelAlarmTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.baseLabelAlarmTitle.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabelAlarmTitle.ForeColor = System.Drawing.Color.Black;
            this.baseLabelAlarmTitle.Location = new System.Drawing.Point(4, 71);
            this.baseLabelAlarmTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabelAlarmTitle.Name = "baseLabelAlarmTitle";
            this.baseLabelAlarmTitle.Size = new System.Drawing.Size(93, 40);
            this.baseLabelAlarmTitle.TabIndex = 4;
            this.baseLabelAlarmTitle.Text = "Title";
            this.baseLabelAlarmTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // AlarmInfoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxSelectedAlarmDetails);
            this.Font = new System.Drawing.Font("Tahoma", 9F);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "AlarmInfoControl";
            this.Size = new System.Drawing.Size(1349, 430);
            this.groupBoxSelectedAlarmDetails.ResumeLayout(false);
            this.groupBoxSelectedAlarmDetails.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxSelectedAlarmDetails;
        private BaseTextBox baseTextBoxAlarmTitle;
        private BaseTextBox baseTextBoxCode;
        private BaseTextBox baseTextBoxGrade;
        private BaseTextBox baseTextBoxSource;
        private BaseTextBox baseTextBoxCause;
        private BaseLabel baseLabelCode;
        private BaseLabel baseLabelCause;
        private BaseLabel baseLabelSource;
        private BaseLabel baseLabelGrade;
        private BaseLabel baseLabelAlarmTitle;
    }
}
