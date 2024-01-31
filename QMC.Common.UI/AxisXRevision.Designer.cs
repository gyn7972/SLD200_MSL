namespace QMC.Common.UI
{
    partial class AxisXRevision
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
            this.components = new System.ComponentModel.Container();
            this.buttonUp = new System.Windows.Forms.Button();
            this.buttonDown = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.baseLabelAxisX = new BaseLabel();
            this.SuspendLayout();
            // 
            // buttonUp
            // 
            this.buttonUp.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonUp.Image = Properties.Resources.Right;
            this.buttonUp.Location = new System.Drawing.Point(130, 0);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(55, 57);
            this.buttonUp.TabIndex = 2;
            this.buttonUp.UseVisualStyleBackColor = false;
            this.buttonUp.Click += new System.EventHandler(this.buttonAxisXUp_Click);
            this.buttonUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXUp_MouseDown);
            this.buttonUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXUp_MouseUp);
            // 
            // buttonDown
            // 
            this.buttonDown.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonDown.Image = Properties.Resources.Left;
            this.buttonDown.Location = new System.Drawing.Point(0, 0);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(54, 57);
            this.buttonDown.TabIndex = 3;
            this.buttonDown.UseVisualStyleBackColor = false;
            this.buttonDown.Click += new System.EventHandler(this.buttonAxisXDown_Click);
            this.buttonDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDown_MouseDown);
            this.buttonDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDown_MouseUp);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // baseLabelAxisX
            // 
            this.baseLabelAxisX.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelAxisX.ForeColor = System.Drawing.Color.White;
            this.baseLabelAxisX.Location = new System.Drawing.Point(69, 17);
            this.baseLabelAxisX.Name = "baseLabelAxisX";
            this.baseLabelAxisX.Size = new System.Drawing.Size(116, 20);
            this.baseLabelAxisX.TabIndex = 4;
            this.baseLabelAxisX.Text = "baseLabel1";
            // 
            // AxisXRevision
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseLabelAxisX);
            this.Controls.Add(this.buttonUp);
            this.Controls.Add(this.buttonDown);
            this.Name = "AxisXRevision";
            this.Size = new System.Drawing.Size(190, 62);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonUp;
        private System.Windows.Forms.Button buttonDown;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private BaseLabel baseLabelAxisX;
    }
}
