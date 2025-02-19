namespace SLD200_MSL
{
    partial class JogButtonCombinationUVW
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
            this.buttonAxisXYCW = new System.Windows.Forms.Button();
            this.buttonAxisXYCCW = new System.Windows.Forms.Button();
            this.buttonAxisXUpYDown = new System.Windows.Forms.Button();
            this.buttonAxisYDown = new System.Windows.Forms.Button();
            this.buttonAxisXUp = new System.Windows.Forms.Button();
            this.buttonAxisXUpYUp = new System.Windows.Forms.Button();
            this.buttonAxisXDownYDown = new System.Windows.Forms.Button();
            this.buttonAxisYUp = new System.Windows.Forms.Button();
            this.buttonAxisXDown = new System.Windows.Forms.Button();
            this.buttonAxisXDownYUp = new System.Windows.Forms.Button();
            this.baseLabelJogButtonComb_VW = new SLD200_MSL.BaseLabel();
            this.baseLabelJogButtonComb_U = new SLD200_MSL.BaseLabel();
            this.baseLabelJogButtonComb = new SLD200_MSL.BaseLabel();
            this.SuspendLayout();
            // 
            // buttonAxisXYCW
            // 
            this.buttonAxisXYCW.BackColor = System.Drawing.Color.Lavender;
            this.buttonAxisXYCW.Image = global::SLD200.Properties.Resources.UVW_CW;
            this.buttonAxisXYCW.Location = new System.Drawing.Point(2, 146);
            this.buttonAxisXYCW.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisXYCW.Name = "buttonAxisXYCW";
            this.buttonAxisXYCW.Size = new System.Drawing.Size(68, 68);
            this.buttonAxisXYCW.TabIndex = 16;
            this.buttonAxisXYCW.UseVisualStyleBackColor = false;
            this.buttonAxisXYCW.Click += new System.EventHandler(this.buttonAxisXYCW_Click);
            this.buttonAxisXYCW.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXYCW_MouseDown);
            this.buttonAxisXYCW.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXYCCW_MouseUp);
            // 
            // buttonAxisXYCCW
            // 
            this.buttonAxisXYCCW.BackColor = System.Drawing.Color.Lavender;
            this.buttonAxisXYCCW.Image = global::SLD200.Properties.Resources.UVW_CCW;
            this.buttonAxisXYCCW.Location = new System.Drawing.Point(2, 2);
            this.buttonAxisXYCCW.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisXYCCW.Name = "buttonAxisXYCCW";
            this.buttonAxisXYCCW.Size = new System.Drawing.Size(68, 68);
            this.buttonAxisXYCCW.TabIndex = 15;
            this.buttonAxisXYCCW.UseVisualStyleBackColor = false;
            this.buttonAxisXYCCW.Click += new System.EventHandler(this.buttonAxisXYCCW_Click);
            this.buttonAxisXYCCW.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXYCCW_MouseDown);
            this.buttonAxisXYCCW.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXYCCW_MouseUp);
            // 
            // buttonAxisXUpYDown
            // 
            this.buttonAxisXUpYDown.BackColor = System.Drawing.Color.Lavender;
            this.buttonAxisXUpYDown.Image = global::SLD200.Properties.Resources.UVW_RightDown_Edited;
            this.buttonAxisXUpYDown.Location = new System.Drawing.Point(232, 146);
            this.buttonAxisXUpYDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisXUpYDown.Name = "buttonAxisXUpYDown";
            this.buttonAxisXUpYDown.Size = new System.Drawing.Size(68, 68);
            this.buttonAxisXUpYDown.TabIndex = 4;
            this.buttonAxisXUpYDown.UseVisualStyleBackColor = false;
            this.buttonAxisXUpYDown.Click += new System.EventHandler(this.buttonAxisXUpYDown_Click);
            this.buttonAxisXUpYDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXUpYDown_MouseDown);
            this.buttonAxisXUpYDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXUpYDown_MouseUp);
            // 
            // buttonAxisYDown
            // 
            this.buttonAxisYDown.BackColor = System.Drawing.Color.Lavender;
            this.buttonAxisYDown.Image = global::SLD200.Properties.Resources.UVW_VW_DOWN_Edited;
            this.buttonAxisYDown.Location = new System.Drawing.Point(158, 146);
            this.buttonAxisYDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisYDown.Name = "buttonAxisYDown";
            this.buttonAxisYDown.Size = new System.Drawing.Size(68, 68);
            this.buttonAxisYDown.TabIndex = 5;
            this.buttonAxisYDown.UseVisualStyleBackColor = false;
            this.buttonAxisYDown.Click += new System.EventHandler(this.buttonAxisYDown_Click);
            this.buttonAxisYDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisYDown_MouseDown);
            this.buttonAxisYDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisYDown_MouseUp);
            // 
            // buttonAxisXUp
            // 
            this.buttonAxisXUp.BackColor = System.Drawing.Color.Lavender;
            this.buttonAxisXUp.Image = global::SLD200.Properties.Resources.UVW_U_RIGHT_Edited;
            this.buttonAxisXUp.Location = new System.Drawing.Point(232, 74);
            this.buttonAxisXUp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisXUp.Name = "buttonAxisXUp";
            this.buttonAxisXUp.Size = new System.Drawing.Size(68, 68);
            this.buttonAxisXUp.TabIndex = 6;
            this.buttonAxisXUp.UseVisualStyleBackColor = false;
            this.buttonAxisXUp.Click += new System.EventHandler(this.ButtonAxisXRight_Click);
            this.buttonAxisXUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXUp_MouseDown);
            this.buttonAxisXUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXUp_MouseUp);
            // 
            // buttonAxisXUpYUp
            // 
            this.buttonAxisXUpYUp.BackColor = System.Drawing.Color.Lavender;
            this.buttonAxisXUpYUp.Image = global::SLD200.Properties.Resources.UVW_RightUp_Edited;
            this.buttonAxisXUpYUp.Location = new System.Drawing.Point(232, 2);
            this.buttonAxisXUpYUp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisXUpYUp.Name = "buttonAxisXUpYUp";
            this.buttonAxisXUpYUp.Size = new System.Drawing.Size(68, 68);
            this.buttonAxisXUpYUp.TabIndex = 7;
            this.buttonAxisXUpYUp.UseVisualStyleBackColor = false;
            this.buttonAxisXUpYUp.Click += new System.EventHandler(this.buttonAxisXUpYUp_Click);
            this.buttonAxisXUpYUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXUpYUp_MouseDown);
            this.buttonAxisXUpYUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXUpYUp_MouseUp);
            // 
            // buttonAxisXDownYDown
            // 
            this.buttonAxisXDownYDown.BackColor = System.Drawing.Color.Lavender;
            this.buttonAxisXDownYDown.Image = global::SLD200.Properties.Resources.UVW_LeftDown_Edited;
            this.buttonAxisXDownYDown.Location = new System.Drawing.Point(84, 146);
            this.buttonAxisXDownYDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisXDownYDown.Name = "buttonAxisXDownYDown";
            this.buttonAxisXDownYDown.Size = new System.Drawing.Size(68, 68);
            this.buttonAxisXDownYDown.TabIndex = 8;
            this.buttonAxisXDownYDown.UseVisualStyleBackColor = false;
            this.buttonAxisXDownYDown.Click += new System.EventHandler(this.buttonAxisXDownYDown_Click);
            this.buttonAxisXDownYDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDownYDown_MouseDown);
            this.buttonAxisXDownYDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDownYDown_MouseUp);
            // 
            // buttonAxisYUp
            // 
            this.buttonAxisYUp.BackColor = System.Drawing.Color.Lavender;
            this.buttonAxisYUp.Image = global::SLD200.Properties.Resources.UVW_VW_UP_Edited;
            this.buttonAxisYUp.Location = new System.Drawing.Point(158, 2);
            this.buttonAxisYUp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisYUp.Name = "buttonAxisYUp";
            this.buttonAxisYUp.Size = new System.Drawing.Size(68, 68);
            this.buttonAxisYUp.TabIndex = 9;
            this.buttonAxisYUp.UseVisualStyleBackColor = false;
            this.buttonAxisYUp.Click += new System.EventHandler(this.buttonAxisYUp_Click);
            this.buttonAxisYUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisYUp_MouseDown);
            this.buttonAxisYUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisYUp_MouseUp);
            // 
            // buttonAxisXDown
            // 
            this.buttonAxisXDown.BackColor = System.Drawing.Color.Lavender;
            this.buttonAxisXDown.Image = global::SLD200.Properties.Resources.UVW_U_LEFT_Edited;
            this.buttonAxisXDown.Location = new System.Drawing.Point(84, 74);
            this.buttonAxisXDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisXDown.Name = "buttonAxisXDown";
            this.buttonAxisXDown.Size = new System.Drawing.Size(68, 68);
            this.buttonAxisXDown.TabIndex = 10;
            this.buttonAxisXDown.UseVisualStyleBackColor = false;
            this.buttonAxisXDown.Click += new System.EventHandler(this.ButtonAxisXLeft_Click);
            this.buttonAxisXDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDown_MouseDown);
            this.buttonAxisXDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDown_MouseUp);
            // 
            // buttonAxisXDownYUp
            // 
            this.buttonAxisXDownYUp.BackColor = System.Drawing.Color.Lavender;
            this.buttonAxisXDownYUp.Image = global::SLD200.Properties.Resources.UVW_LeftUp_Edited;
            this.buttonAxisXDownYUp.Location = new System.Drawing.Point(84, 2);
            this.buttonAxisXDownYUp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisXDownYUp.Name = "buttonAxisXDownYUp";
            this.buttonAxisXDownYUp.Size = new System.Drawing.Size(68, 68);
            this.buttonAxisXDownYUp.TabIndex = 11;
            this.buttonAxisXDownYUp.UseVisualStyleBackColor = false;
            this.buttonAxisXDownYUp.Click += new System.EventHandler(this.buttonAxisXDownYUp_Click);
            this.buttonAxisXDownYUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDownYUp_MouseDown);
            this.buttonAxisXDownYUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDownYUp_MouseUp);
            // 
            // baseLabelJogButtonComb_VW
            // 
            this.baseLabelJogButtonComb_VW.BackColor = System.Drawing.Color.LightSkyBlue;
            this.baseLabelJogButtonComb_VW.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabelJogButtonComb_VW.ForeColor = System.Drawing.Color.Black;
            this.baseLabelJogButtonComb_VW.Location = new System.Drawing.Point(160, 116);
            this.baseLabelJogButtonComb_VW.Name = "baseLabelJogButtonComb_VW";
            this.baseLabelJogButtonComb_VW.Size = new System.Drawing.Size(64, 25);
            this.baseLabelJogButtonComb_VW.TabIndex = 14;
            this.baseLabelJogButtonComb_VW.Text = "VW  [↕]";
            this.baseLabelJogButtonComb_VW.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabelJogButtonComb_U
            // 
            this.baseLabelJogButtonComb_U.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.baseLabelJogButtonComb_U.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabelJogButtonComb_U.ForeColor = System.Drawing.Color.Black;
            this.baseLabelJogButtonComb_U.Location = new System.Drawing.Point(160, 75);
            this.baseLabelJogButtonComb_U.Name = "baseLabelJogButtonComb_U";
            this.baseLabelJogButtonComb_U.Size = new System.Drawing.Size(64, 40);
            this.baseLabelJogButtonComb_U.TabIndex = 13;
            this.baseLabelJogButtonComb_U.Text = "U\r\n[↔]";
            this.baseLabelJogButtonComb_U.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.baseLabelJogButtonComb_U.Click += new System.EventHandler(this.baseLabelJogButtonComb_U_Click);
            // 
            // baseLabelJogButtonComb
            // 
            this.baseLabelJogButtonComb.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelJogButtonComb.ForeColor = System.Drawing.Color.White;
            this.baseLabelJogButtonComb.Location = new System.Drawing.Point(3, 100);
            this.baseLabelJogButtonComb.Name = "baseLabelJogButtonComb";
            this.baseLabelJogButtonComb.Size = new System.Drawing.Size(53, 16);
            this.baseLabelJogButtonComb.TabIndex = 12;
            this.baseLabelJogButtonComb.Text = "X , Y";
            this.baseLabelJogButtonComb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // JogButtonCombinationUVW
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonAxisXYCW);
            this.Controls.Add(this.buttonAxisXYCCW);
            this.Controls.Add(this.baseLabelJogButtonComb_VW);
            this.Controls.Add(this.baseLabelJogButtonComb_U);
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
            this.Name = "JogButtonCombinationUVW";
            this.Size = new System.Drawing.Size(305, 220);
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
        private BaseLabel baseLabelJogButtonComb_U;
        private BaseLabel baseLabelJogButtonComb_VW;
        private System.Windows.Forms.Button buttonAxisXYCCW;
        private System.Windows.Forms.Button buttonAxisXYCW;
    }
}
