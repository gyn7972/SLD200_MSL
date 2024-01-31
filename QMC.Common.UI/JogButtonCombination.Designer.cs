namespace QMC.Common.UI
{
    partial class JogButtonCombination
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
            this.buttonAxisXUpYDown = new System.Windows.Forms.Button();
            this.buttonAxisYDown = new System.Windows.Forms.Button();
            this.buttonAxisXUp = new System.Windows.Forms.Button();
            this.buttonAxisXDownYDown = new System.Windows.Forms.Button();
            this.buttonAxisXDown = new System.Windows.Forms.Button();
            this.buttonAxisXUpYUp = new System.Windows.Forms.Button();
            this.buttonAxisYUp = new System.Windows.Forms.Button();
            this.buttonAxisXDownYUp = new System.Windows.Forms.Button();
            this.baseLabelJogButtonComb = new QMC.Common.UI.BaseLabel();
            this.SuspendLayout();
            // 
            // buttonAxisXUpYDown
            // 
            this.buttonAxisXUpYDown.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonAxisXUpYDown.Image = global::QMC.Common.UI.Properties.Resources.RightDown;
            this.buttonAxisXUpYDown.Location = new System.Drawing.Point(95, 91);
            this.buttonAxisXUpYDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisXUpYDown.Name = "buttonAxisXUpYDown";
            this.buttonAxisXUpYDown.Size = new System.Drawing.Size(45, 45);
            this.buttonAxisXUpYDown.TabIndex = 4;
            this.buttonAxisXUpYDown.UseVisualStyleBackColor = false;
            this.buttonAxisXUpYDown.Click += new System.EventHandler(this.buttonAxisXUpYDown_Click);
            this.buttonAxisXUpYDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXUpYDown_MouseDown);
            this.buttonAxisXUpYDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXUpYDown_MouseUp);
            // 
            // buttonAxisYDown
            // 
            this.buttonAxisYDown.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonAxisYDown.Image = global::QMC.Common.UI.Properties.Resources.Down;
            this.buttonAxisYDown.Location = new System.Drawing.Point(49, 91);
            this.buttonAxisYDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisYDown.Name = "buttonAxisYDown";
            this.buttonAxisYDown.Size = new System.Drawing.Size(45, 45);
            this.buttonAxisYDown.TabIndex = 5;
            this.buttonAxisYDown.UseVisualStyleBackColor = false;
            this.buttonAxisYDown.Click += new System.EventHandler(this.buttonAxisYDown_Click);
            this.buttonAxisYDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisYDown_MouseDown);
            this.buttonAxisYDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisYDown_MouseUp);
            // 
            // buttonAxisXUp
            // 
            this.buttonAxisXUp.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonAxisXUp.Image = global::QMC.Common.UI.Properties.Resources.Right;
            this.buttonAxisXUp.Location = new System.Drawing.Point(95, 46);
            this.buttonAxisXUp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisXUp.Name = "buttonAxisXUp";
            this.buttonAxisXUp.Size = new System.Drawing.Size(45, 45);
            this.buttonAxisXUp.TabIndex = 6;
            this.buttonAxisXUp.UseVisualStyleBackColor = false;
            this.buttonAxisXUp.Click += new System.EventHandler(this.ButtonAxisXRight_Click);
            this.buttonAxisXUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXUp_MouseDown);
            this.buttonAxisXUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXUp_MouseUp);
            // 
            // buttonAxisXDownYDown
            // 
            this.buttonAxisXDownYDown.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonAxisXDownYDown.Image = global::QMC.Common.UI.Properties.Resources.LeftDown;
            this.buttonAxisXDownYDown.Location = new System.Drawing.Point(3, 91);
            this.buttonAxisXDownYDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisXDownYDown.Name = "buttonAxisXDownYDown";
            this.buttonAxisXDownYDown.Size = new System.Drawing.Size(45, 45);
            this.buttonAxisXDownYDown.TabIndex = 8;
            this.buttonAxisXDownYDown.UseVisualStyleBackColor = false;
            this.buttonAxisXDownYDown.Click += new System.EventHandler(this.buttonAxisXDownYDown_Click);
            this.buttonAxisXDownYDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDownYDown_MouseDown);
            this.buttonAxisXDownYDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDownYDown_MouseUp);
            // 
            // buttonAxisXDown
            // 
            this.buttonAxisXDown.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonAxisXDown.Image = global::QMC.Common.UI.Properties.Resources.Left;
            this.buttonAxisXDown.Location = new System.Drawing.Point(3, 46);
            this.buttonAxisXDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisXDown.Name = "buttonAxisXDown";
            this.buttonAxisXDown.Size = new System.Drawing.Size(45, 45);
            this.buttonAxisXDown.TabIndex = 10;
            this.buttonAxisXDown.UseVisualStyleBackColor = false;
            this.buttonAxisXDown.Click += new System.EventHandler(this.ButtonAxisXLeft_Click);
            this.buttonAxisXDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDown_MouseDown);
            this.buttonAxisXDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDown_MouseUp);
            // 
            // buttonAxisXUpYUp
            // 
            this.buttonAxisXUpYUp.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonAxisXUpYUp.Image = global::QMC.Common.UI.Properties.Resources.RightUp;
            this.buttonAxisXUpYUp.Location = new System.Drawing.Point(95, 2);
            this.buttonAxisXUpYUp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisXUpYUp.Name = "buttonAxisXUpYUp";
            this.buttonAxisXUpYUp.Size = new System.Drawing.Size(45, 45);
            this.buttonAxisXUpYUp.TabIndex = 7;
            this.buttonAxisXUpYUp.UseVisualStyleBackColor = false;
            this.buttonAxisXUpYUp.Click += new System.EventHandler(this.buttonAxisXUpYUp_Click);
            this.buttonAxisXUpYUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXUpYUp_MouseDown);
            this.buttonAxisXUpYUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXUpYUp_MouseUp);
            // 
            // buttonAxisYUp
            // 
            this.buttonAxisYUp.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonAxisYUp.Image = global::QMC.Common.UI.Properties.Resources.Up;
            this.buttonAxisYUp.Location = new System.Drawing.Point(49, 2);
            this.buttonAxisYUp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisYUp.Name = "buttonAxisYUp";
            this.buttonAxisYUp.Size = new System.Drawing.Size(45, 45);
            this.buttonAxisYUp.TabIndex = 9;
            this.buttonAxisYUp.UseVisualStyleBackColor = false;
            this.buttonAxisYUp.Click += new System.EventHandler(this.buttonAxisYUp_Click);
            this.buttonAxisYUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisYUp_MouseDown);
            this.buttonAxisYUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisYUp_MouseUp);
            // 
            // buttonAxisXDownYUp
            // 
            this.buttonAxisXDownYUp.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonAxisXDownYUp.Image = global::QMC.Common.UI.Properties.Resources.LeftUp;
            this.buttonAxisXDownYUp.Location = new System.Drawing.Point(3, 2);
            this.buttonAxisXDownYUp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisXDownYUp.Name = "buttonAxisXDownYUp";
            this.buttonAxisXDownYUp.Size = new System.Drawing.Size(45, 45);
            this.buttonAxisXDownYUp.TabIndex = 11;
            this.buttonAxisXDownYUp.UseVisualStyleBackColor = false;
            this.buttonAxisXDownYUp.Click += new System.EventHandler(this.buttonAxisXDownYUp_Click);
            this.buttonAxisXDownYUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDownYUp_MouseDown);
            this.buttonAxisXDownYUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDownYUp_MouseUp);
            // 
            // baseLabelJogButtonComb
            // 
            this.baseLabelJogButtonComb.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelJogButtonComb.ForeColor = System.Drawing.Color.White;
            this.baseLabelJogButtonComb.Location = new System.Drawing.Point(44, 140);
            this.baseLabelJogButtonComb.Name = "baseLabelJogButtonComb";
            this.baseLabelJogButtonComb.Size = new System.Drawing.Size(53, 16);
            this.baseLabelJogButtonComb.TabIndex = 12;
            this.baseLabelJogButtonComb.Text = "X , Y";
            this.baseLabelJogButtonComb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // JogButtonCombination
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseLabelJogButtonComb);
            this.Controls.Add(this.buttonAxisXUpYDown);
            this.Controls.Add(this.buttonAxisYDown);
            this.Controls.Add(this.buttonAxisXUp);
            this.Controls.Add(this.buttonAxisXUpYUp);
            this.Controls.Add(this.buttonAxisXDownYDown);
            this.Controls.Add(this.buttonAxisYUp);
            this.Controls.Add(this.buttonAxisXDown);
            this.Controls.Add(this.buttonAxisXDownYUp);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "JogButtonCombination";
            this.Size = new System.Drawing.Size(144, 159);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonAxisXUpYDown;
        private System.Windows.Forms.Button buttonAxisYDown;
        private System.Windows.Forms.Button buttonAxisXUp;
        private System.Windows.Forms.Button buttonAxisXDownYDown;
        private System.Windows.Forms.Button buttonAxisXDown;
        private System.Windows.Forms.Button buttonAxisXUpYUp;
        private System.Windows.Forms.Button buttonAxisYUp;
        private System.Windows.Forms.Button buttonAxisXDownYUp;
        private BaseLabel baseLabelJogButtonComb;
    }
}
