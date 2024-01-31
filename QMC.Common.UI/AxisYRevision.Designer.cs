namespace QMC.Common.UI
{
    partial class AxisYRevision
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
            this.buttonDown = new System.Windows.Forms.Button();
            this.buttonUp = new System.Windows.Forms.Button();
            this.baseLabelAxisYRevision = new BaseLabel();
            this.SuspendLayout();
            // 
            // buttonDown
            // 
            this.buttonDown.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonDown.Image = Properties.Resources.Down;
            this.buttonDown.Location = new System.Drawing.Point(3, 127);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(58, 56);
            this.buttonDown.TabIndex = 7;
            this.buttonDown.UseVisualStyleBackColor = false;
            this.buttonDown.Click += new System.EventHandler(this.buttonAxisYDown_Click);
            this.buttonDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisYDown_MouseDown);
            this.buttonDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisYDown_MouseUp);
            // 
            // buttonUp
            // 
            this.buttonUp.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonUp.Image = Properties.Resources.Up;
            this.buttonUp.Location = new System.Drawing.Point(3, 0);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(57, 59);
            this.buttonUp.TabIndex = 8;
            this.buttonUp.UseVisualStyleBackColor = false;
            this.buttonUp.Click += new System.EventHandler(this.buttonAxisYUp_Click);
            this.buttonUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisUp_MouseDown);
            this.buttonUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisUp_MouseUp);
            // 
            // baseLabelAxisYRevision
            // 
            this.baseLabelAxisYRevision.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelAxisYRevision.ForeColor = System.Drawing.Color.White;
            this.baseLabelAxisYRevision.Location = new System.Drawing.Point(7, 79);
            this.baseLabelAxisYRevision.Name = "baseLabelAxisYRevision";
            this.baseLabelAxisYRevision.Size = new System.Drawing.Size(116, 20);
            this.baseLabelAxisYRevision.TabIndex = 9;
            this.baseLabelAxisYRevision.Text = "baseLabel1";
            // 
            // AxisYRevision
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseLabelAxisYRevision);
            this.Controls.Add(this.buttonDown);
            this.Controls.Add(this.buttonUp);
            this.Name = "AxisYRevision";
            this.Size = new System.Drawing.Size(62, 187);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonDown;
        private System.Windows.Forms.Button buttonUp;
        private BaseLabel baseLabelAxisYRevision;
    }
}
