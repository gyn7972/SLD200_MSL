namespace QMC.Common.UI
{
    partial class JogButtonTheta
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
            this.buttonCW = new System.Windows.Forms.Button();
            this.buttonCCW = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.baseLabelTheta = new BaseLabel();
            this.SuspendLayout();
            // 
            // buttonCW
            // 
            this.buttonCW.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonCW.Image = Properties.Resources.CW;
            this.buttonCW.Location = new System.Drawing.Point(130, 0);
            this.buttonCW.Name = "buttonCW";
            this.buttonCW.Size = new System.Drawing.Size(55, 57);
            this.buttonCW.TabIndex = 7;
            this.buttonCW.UseVisualStyleBackColor = false;
            this.buttonCW.Click += new System.EventHandler(this.buttonThetaLeft_Click);
            this.buttonCW.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDown_MouseDown);
            this.buttonCW.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDown_MouseUp);
            // 
            // buttonCCW
            // 
            this.buttonCCW.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonCCW.Image = Properties.Resources.CCW;
            this.buttonCCW.Location = new System.Drawing.Point(0, 0);
            this.buttonCCW.Name = "buttonCCW";
            this.buttonCCW.Size = new System.Drawing.Size(54, 57);
            this.buttonCCW.TabIndex = 8;
            this.buttonCCW.UseVisualStyleBackColor = false;
            this.buttonCCW.Click += new System.EventHandler(this.buttonThetaRight_Click);
            this.buttonCCW.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXUp_MouseDown);
            this.buttonCCW.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXUp_MouseUp);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // baseLabelTheta
            // 
            this.baseLabelTheta.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelTheta.ForeColor = System.Drawing.Color.White;
            this.baseLabelTheta.Location = new System.Drawing.Point(60, 22);
            this.baseLabelTheta.Name = "baseLabelTheta";
            this.baseLabelTheta.Size = new System.Drawing.Size(116, 20);
            this.baseLabelTheta.TabIndex = 9;
            this.baseLabelTheta.Text = "baseLabel1";
            // 
            // JogButtonTheta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseLabelTheta);
            this.Controls.Add(this.buttonCW);
            this.Controls.Add(this.buttonCCW);
            this.Name = "JogButtonTheta";
            this.Size = new System.Drawing.Size(192, 64);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonCW;
        private System.Windows.Forms.Button buttonCCW;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private BaseLabel baseLabelTheta;
    }
}
