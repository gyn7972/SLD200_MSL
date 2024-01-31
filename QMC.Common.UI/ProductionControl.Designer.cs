namespace QMC.Common.UI
{
    partial class ProductionControl
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
            this.baseGroupBoxProduction = new QMC.Common.UI.BaseGroupBox();
            this.baseLabel1 = new QMC.Common.UI.BaseLabel();
            this.baseButtonResetProductionInformation = new QMC.Common.UI.BaseButton();
            this.baseLabelNG = new QMC.Common.UI.BaseLabel();
            this.baseLabelTitleNG = new QMC.Common.UI.BaseLabel();
            this.baseLabelOK = new QMC.Common.UI.BaseLabel();
            this.baseLabelTitleOK = new QMC.Common.UI.BaseLabel();
            this.baseLabelTactTime = new QMC.Common.UI.BaseLabel();
            this.baseLabelTitleTactTime = new QMC.Common.UI.BaseLabel();
            this.baseLabelCount = new QMC.Common.UI.BaseLabel();
            this.baseLabelTitleCount = new QMC.Common.UI.BaseLabel();
            this.baseGroupBoxProduction.SuspendLayout();
            this.SuspendLayout();
            // 
            // baseGroupBoxProduction
            // 
            this.baseGroupBoxProduction.Controls.Add(this.baseLabel1);
            this.baseGroupBoxProduction.Controls.Add(this.baseButtonResetProductionInformation);
            this.baseGroupBoxProduction.Controls.Add(this.baseLabelNG);
            this.baseGroupBoxProduction.Controls.Add(this.baseLabelTitleNG);
            this.baseGroupBoxProduction.Controls.Add(this.baseLabelOK);
            this.baseGroupBoxProduction.Controls.Add(this.baseLabelTitleOK);
            this.baseGroupBoxProduction.Controls.Add(this.baseLabelTactTime);
            this.baseGroupBoxProduction.Controls.Add(this.baseLabelTitleTactTime);
            this.baseGroupBoxProduction.Controls.Add(this.baseLabelCount);
            this.baseGroupBoxProduction.Controls.Add(this.baseLabelTitleCount);
            this.baseGroupBoxProduction.ForeColor = System.Drawing.Color.White;
            this.baseGroupBoxProduction.Location = new System.Drawing.Point(0, 0);
            this.baseGroupBoxProduction.Name = "baseGroupBoxProduction";
            this.baseGroupBoxProduction.Size = new System.Drawing.Size(360, 144);
            this.baseGroupBoxProduction.TabIndex = 0;
            this.baseGroupBoxProduction.TabStop = false;
            this.baseGroupBoxProduction.Text = "Production";
            // 
            // baseLabel1
            // 
            this.baseLabel1.AutoSize = true;
            this.baseLabel1.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabel1.ForeColor = System.Drawing.Color.White;
            this.baseLabel1.Location = new System.Drawing.Point(244, 116);
            this.baseLabel1.Name = "baseLabel1";
            this.baseLabel1.Size = new System.Drawing.Size(31, 16);
            this.baseLabel1.TabIndex = 11;
            this.baseLabel1.Text = "ms";
            // 
            // baseButtonResetProductionInformation
            // 
            this.baseButtonResetProductionInformation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonResetProductionInformation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonResetProductionInformation.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonResetProductionInformation.Location = new System.Drawing.Point(244, 13);
            this.baseButtonResetProductionInformation.Name = "baseButtonResetProductionInformation";
            this.baseButtonResetProductionInformation.Size = new System.Drawing.Size(104, 93);
            this.baseButtonResetProductionInformation.TabIndex = 1;
            this.baseButtonResetProductionInformation.Text = "Reset";
            this.baseButtonResetProductionInformation.UseVisualStyleBackColor = false;
            this.baseButtonResetProductionInformation.Click += new System.EventHandler(this.baseButtonResetProductionInformation_Click);
            // 
            // baseLabelNG
            // 
            this.baseLabelNG.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabelNG.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelNG.ForeColor = System.Drawing.Color.White;
            this.baseLabelNG.Location = new System.Drawing.Point(106, 79);
            this.baseLabelNG.Name = "baseLabelNG";
            this.baseLabelNG.Size = new System.Drawing.Size(132, 25);
            this.baseLabelNG.TabIndex = 8;
            this.baseLabelNG.Text = "0";
            this.baseLabelNG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabelTitleNG
            // 
            this.baseLabelTitleNG.AutoSize = true;
            this.baseLabelTitleNG.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelTitleNG.ForeColor = System.Drawing.Color.White;
            this.baseLabelTitleNG.Location = new System.Drawing.Point(6, 81);
            this.baseLabelTitleNG.Name = "baseLabelTitleNG";
            this.baseLabelTitleNG.Size = new System.Drawing.Size(32, 16);
            this.baseLabelTitleNG.TabIndex = 9;
            this.baseLabelTitleNG.Text = "NG";
            // 
            // baseLabelOK
            // 
            this.baseLabelOK.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabelOK.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelOK.ForeColor = System.Drawing.Color.White;
            this.baseLabelOK.Location = new System.Drawing.Point(106, 46);
            this.baseLabelOK.Name = "baseLabelOK";
            this.baseLabelOK.Size = new System.Drawing.Size(132, 25);
            this.baseLabelOK.TabIndex = 6;
            this.baseLabelOK.Text = "0";
            this.baseLabelOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabelTitleOK
            // 
            this.baseLabelTitleOK.AutoSize = true;
            this.baseLabelTitleOK.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelTitleOK.ForeColor = System.Drawing.Color.White;
            this.baseLabelTitleOK.Location = new System.Drawing.Point(7, 49);
            this.baseLabelTitleOK.Name = "baseLabelTitleOK";
            this.baseLabelTitleOK.Size = new System.Drawing.Size(31, 16);
            this.baseLabelTitleOK.TabIndex = 7;
            this.baseLabelTitleOK.Text = "OK";
            // 
            // baseLabelTactTime
            // 
            this.baseLabelTactTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabelTactTime.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelTactTime.ForeColor = System.Drawing.Color.White;
            this.baseLabelTactTime.Location = new System.Drawing.Point(106, 112);
            this.baseLabelTactTime.Name = "baseLabelTactTime";
            this.baseLabelTactTime.Size = new System.Drawing.Size(132, 25);
            this.baseLabelTactTime.TabIndex = 2;
            this.baseLabelTactTime.Text = "0";
            this.baseLabelTactTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabelTitleTactTime
            // 
            this.baseLabelTitleTactTime.AutoSize = true;
            this.baseLabelTitleTactTime.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelTitleTactTime.ForeColor = System.Drawing.Color.White;
            this.baseLabelTitleTactTime.Location = new System.Drawing.Point(6, 113);
            this.baseLabelTitleTactTime.Name = "baseLabelTitleTactTime";
            this.baseLabelTitleTactTime.Size = new System.Drawing.Size(87, 16);
            this.baseLabelTitleTactTime.TabIndex = 3;
            this.baseLabelTitleTactTime.Text = "Tact Time";
            // 
            // baseLabelCount
            // 
            this.baseLabelCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabelCount.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelCount.ForeColor = System.Drawing.Color.White;
            this.baseLabelCount.Location = new System.Drawing.Point(106, 13);
            this.baseLabelCount.Name = "baseLabelCount";
            this.baseLabelCount.Size = new System.Drawing.Size(132, 25);
            this.baseLabelCount.TabIndex = 1;
            this.baseLabelCount.Text = "0";
            this.baseLabelCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabelTitleCount
            // 
            this.baseLabelTitleCount.AutoSize = true;
            this.baseLabelTitleCount.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelTitleCount.ForeColor = System.Drawing.Color.White;
            this.baseLabelTitleCount.Location = new System.Drawing.Point(6, 17);
            this.baseLabelTitleCount.Name = "baseLabelTitleCount";
            this.baseLabelTitleCount.Size = new System.Drawing.Size(56, 16);
            this.baseLabelTitleCount.TabIndex = 1;
            this.baseLabelTitleCount.Text = "Count";
            // 
            // ProductionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseGroupBoxProduction);
            this.Name = "ProductionControl";
            this.Size = new System.Drawing.Size(360, 148);
            this.baseGroupBoxProduction.ResumeLayout(false);
            this.baseGroupBoxProduction.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private BaseButton baseButtonResetProductionInformation;
        private BaseLabel baseLabelNG;
        private BaseLabel baseLabelTitleNG;
        private BaseLabel baseLabelOK;
        private BaseLabel baseLabelTitleOK;
        private BaseLabel baseLabelTactTime;
        private BaseLabel baseLabelTitleTactTime;
        private BaseLabel baseLabelCount;
        private BaseLabel baseLabelTitleCount;
        protected BaseGroupBox baseGroupBoxProduction;
        protected BaseLabel baseLabel1;
    }
}
