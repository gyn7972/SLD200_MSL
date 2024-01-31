using System.Windows.Forms;

namespace QMC.Common.UI
{
    partial class AutoFocusControl
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
            this.baseGroupBoxMain = new BaseGroupBox();
            this.baseButtonAutoFocus = new BaseButton();
            this.baseTextBoxValue = new BaseTextBox();
            this.baseTextBoxPosZ = new BaseTextBox();
            this.baseLabel1 = new BaseLabel();
            this.baseLabelPosZ = new BaseLabel();
            this.baseGroupBoxMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // baseGroupBoxMain
            // 
            this.baseGroupBoxMain.Controls.Add(this.baseButtonAutoFocus);
            this.baseGroupBoxMain.Controls.Add(this.baseTextBoxValue);
            this.baseGroupBoxMain.Controls.Add(this.baseTextBoxPosZ);
            this.baseGroupBoxMain.Controls.Add(this.baseLabel1);
            this.baseGroupBoxMain.Controls.Add(this.baseLabelPosZ);
            this.baseGroupBoxMain.ForeColor = System.Drawing.Color.White;
            this.baseGroupBoxMain.Location = new System.Drawing.Point(0, 0);
            this.baseGroupBoxMain.Name = "baseGroupBoxMain";
            this.baseGroupBoxMain.Size = new System.Drawing.Size(131, 178);
            this.baseGroupBoxMain.TabIndex = 0;
            this.baseGroupBoxMain.TabStop = false;
            this.baseGroupBoxMain.Text = "AutoFocus";
            // 
            // baseButtonAutoFocus
            // 
            this.baseButtonAutoFocus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonAutoFocus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonAutoFocus.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonAutoFocus.Location = new System.Drawing.Point(7, 139);
            this.baseButtonAutoFocus.Name = "baseButtonAutoFocus";
            this.baseButtonAutoFocus.Size = new System.Drawing.Size(114, 30);
            this.baseButtonAutoFocus.TabIndex = 4;
            this.baseButtonAutoFocus.Text = "AutoFocus";
            this.baseButtonAutoFocus.UseVisualStyleBackColor = false;
            this.baseButtonAutoFocus.Click += new System.EventHandler(this.baseButtonAutoFocus_Click);
            // 
            // baseTextBoxValue
            // 
            this.baseTextBoxValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxValue.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseTextBoxValue.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxValue.Location = new System.Drawing.Point(7, 96);
            this.baseTextBoxValue.Name = "baseTextBoxValue";
            this.baseTextBoxValue.Size = new System.Drawing.Size(114, 20);
            this.baseTextBoxValue.TabIndex = 3;
            // 
            // baseTextBoxPosZ
            // 
            this.baseTextBoxPosZ.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxPosZ.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxPosZ.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseTextBoxPosZ.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxPosZ.Location = new System.Drawing.Point(7, 44);
            this.baseTextBoxPosZ.Name = "baseTextBoxPosZ";
            this.baseTextBoxPosZ.Size = new System.Drawing.Size(114, 20);
            this.baseTextBoxPosZ.TabIndex = 2;
            // 
            // baseLabel1
            // 
            this.baseLabel1.AutoSize = true;
            this.baseLabel1.Font = new System.Drawing.Font("돋움", 10F, System.Drawing.FontStyle.Bold);
            this.baseLabel1.ForeColor = System.Drawing.Color.White;
            this.baseLabel1.Location = new System.Drawing.Point(10, 76);
            this.baseLabel1.Name = "baseLabel1";
            this.baseLabel1.Size = new System.Drawing.Size(53, 17);
            this.baseLabel1.TabIndex = 1;
            this.baseLabel1.Text = "Value";
            // 
            // baseLabelPosZ
            // 
            this.baseLabelPosZ.AutoSize = true;
            this.baseLabelPosZ.Font = new System.Drawing.Font("돋움", 10F, System.Drawing.FontStyle.Bold);
            this.baseLabelPosZ.ForeColor = System.Drawing.Color.White;
            this.baseLabelPosZ.Location = new System.Drawing.Point(10, 24);
            this.baseLabelPosZ.Name = "baseLabelPosZ";
            this.baseLabelPosZ.Size = new System.Drawing.Size(19, 17);
            this.baseLabelPosZ.TabIndex = 0;
            this.baseLabelPosZ.Text = "Z";
            // 
            // AutoFocusControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseGroupBoxMain);
            this.Name = "AutoFocusControl";
            this.Size = new System.Drawing.Size(131, 178);
            this.baseGroupBoxMain.ResumeLayout(false);
            this.baseGroupBoxMain.PerformLayout();
            this.ResumeLayout(false);

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();

        }

        #endregion

        private BaseGroupBox baseGroupBoxMain;
        private BaseButton baseButtonAutoFocus;
        private BaseTextBox baseTextBoxValue;
        private BaseTextBox baseTextBoxPosZ;
        private BaseLabel baseLabel1;
        private BaseLabel baseLabelPosZ;
    }
}
