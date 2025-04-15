namespace SLD200_MSL
{
    partial class JogButtonCombinationXZ
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
            this.buttonAxisYDown = new System.Windows.Forms.Button();
            this.buttonAxisXUp = new System.Windows.Forms.Button();
            this.buttonAxisYUp = new System.Windows.Forms.Button();
            this.buttonAxisXDown = new System.Windows.Forms.Button();
            this.buttonAxisZ0Down = new System.Windows.Forms.Button();
            this.buttonAxisZ0Up = new System.Windows.Forms.Button();
            this.buttonAxisZ1Down = new System.Windows.Forms.Button();
            this.buttonAxisZ1Up = new System.Windows.Forms.Button();
            this.baseLabel_Stacker0 = new SLD200_MSL.BaseLabel();
            this.baseLabel_Stacker1 = new SLD200_MSL.BaseLabel();
            this.baseLabelJogButtonComb_XZ = new SLD200_MSL.BaseLabel();
            this.baseLabelJogButtonComb_Title = new SLD200_MSL.BaseLabel();
            this.baseLabelJogButtonComb = new SLD200_MSL.BaseLabel();
            this.SuspendLayout();
            // 
            // buttonAxisYDown
            // 
            this.buttonAxisYDown.BackColor = System.Drawing.Color.Lavender;
            this.buttonAxisYDown.Image = global::SLD200.Properties.Resources.Z_Down_Small;
            this.buttonAxisYDown.Location = new System.Drawing.Point(72, 53);
            this.buttonAxisYDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisYDown.Name = "buttonAxisYDown";
            this.buttonAxisYDown.Size = new System.Drawing.Size(58, 47);
            this.buttonAxisYDown.TabIndex = 5;
            this.buttonAxisYDown.UseVisualStyleBackColor = false;
            this.buttonAxisYDown.Click += new System.EventHandler(this.buttonAxisYDown_Click);
            this.buttonAxisYDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisYDown_MouseDown);
            this.buttonAxisYDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisYDown_MouseUp);
            // 
            // buttonAxisXUp
            // 
            this.buttonAxisXUp.BackColor = System.Drawing.Color.Lavender;
            this.buttonAxisXUp.Image = global::SLD200.Properties.Resources.Right;
            this.buttonAxisXUp.Location = new System.Drawing.Point(138, 26);
            this.buttonAxisXUp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisXUp.Name = "buttonAxisXUp";
            this.buttonAxisXUp.Size = new System.Drawing.Size(60, 54);
            this.buttonAxisXUp.TabIndex = 6;
            this.buttonAxisXUp.UseVisualStyleBackColor = false;
            this.buttonAxisXUp.Click += new System.EventHandler(this.ButtonAxisXRight_Click);
            this.buttonAxisXUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXUp_MouseDown);
            this.buttonAxisXUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXUp_MouseUp);
            // 
            // buttonAxisYUp
            // 
            this.buttonAxisYUp.BackColor = System.Drawing.Color.Lavender;
            this.buttonAxisYUp.Image = global::SLD200.Properties.Resources.Z_Up_Small;
            this.buttonAxisYUp.Location = new System.Drawing.Point(72, 7);
            this.buttonAxisYUp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisYUp.Name = "buttonAxisYUp";
            this.buttonAxisYUp.Size = new System.Drawing.Size(58, 47);
            this.buttonAxisYUp.TabIndex = 9;
            this.buttonAxisYUp.UseVisualStyleBackColor = false;
            this.buttonAxisYUp.Click += new System.EventHandler(this.buttonAxisYUp_Click);
            this.buttonAxisYUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisYUp_MouseDown);
            this.buttonAxisYUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisYUp_MouseUp);
            // 
            // buttonAxisXDown
            // 
            this.buttonAxisXDown.BackColor = System.Drawing.Color.Lavender;
            this.buttonAxisXDown.Image = global::SLD200.Properties.Resources.Left;
            this.buttonAxisXDown.Location = new System.Drawing.Point(3, 26);
            this.buttonAxisXDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisXDown.Name = "buttonAxisXDown";
            this.buttonAxisXDown.Size = new System.Drawing.Size(60, 54);
            this.buttonAxisXDown.TabIndex = 10;
            this.buttonAxisXDown.UseVisualStyleBackColor = false;
            this.buttonAxisXDown.Click += new System.EventHandler(this.ButtonAxisXLeft_Click);
            this.buttonAxisXDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDown_MouseDown);
            this.buttonAxisXDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDown_MouseUp);
            // 
            // buttonAxisZ0Down
            // 
            this.buttonAxisZ0Down.BackColor = System.Drawing.Color.Lavender;
            this.buttonAxisZ0Down.Image = global::SLD200.Properties.Resources.UVW_VW_DOWN_Edited;
            this.buttonAxisZ0Down.Location = new System.Drawing.Point(138, 162);
            this.buttonAxisZ0Down.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisZ0Down.Name = "buttonAxisZ0Down";
            this.buttonAxisZ0Down.Size = new System.Drawing.Size(60, 54);
            this.buttonAxisZ0Down.TabIndex = 15;
            this.buttonAxisZ0Down.UseVisualStyleBackColor = false;
            this.buttonAxisZ0Down.Click += new System.EventHandler(this.buttonAxisZ0Down_Click);
            this.buttonAxisZ0Down.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisZ0Down_MouseDown);
            this.buttonAxisZ0Down.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisZ0Down_MouseUp);
            // 
            // buttonAxisZ0Up
            // 
            this.buttonAxisZ0Up.BackColor = System.Drawing.Color.Lavender;
            this.buttonAxisZ0Up.Image = global::SLD200.Properties.Resources.UVW_VW_UP_Edited;
            this.buttonAxisZ0Up.Location = new System.Drawing.Point(138, 109);
            this.buttonAxisZ0Up.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisZ0Up.Name = "buttonAxisZ0Up";
            this.buttonAxisZ0Up.Size = new System.Drawing.Size(60, 54);
            this.buttonAxisZ0Up.TabIndex = 16;
            this.buttonAxisZ0Up.UseVisualStyleBackColor = false;
            this.buttonAxisZ0Up.Click += new System.EventHandler(this.buttonAxisZ0Up_Click);
            this.buttonAxisZ0Up.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisZ0Up_MouseDown);
            this.buttonAxisZ0Up.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisZ0Up_MouseUp);
            // 
            // buttonAxisZ1Down
            // 
            this.buttonAxisZ1Down.BackColor = System.Drawing.Color.Lavender;
            this.buttonAxisZ1Down.Image = global::SLD200.Properties.Resources.UVW_VW_DOWN_Edited;
            this.buttonAxisZ1Down.Location = new System.Drawing.Point(26, 162);
            this.buttonAxisZ1Down.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisZ1Down.Name = "buttonAxisZ1Down";
            this.buttonAxisZ1Down.Size = new System.Drawing.Size(60, 54);
            this.buttonAxisZ1Down.TabIndex = 17;
            this.buttonAxisZ1Down.UseVisualStyleBackColor = false;
            this.buttonAxisZ1Down.Click += new System.EventHandler(this.buttonAxisZ1Down_Click);
            this.buttonAxisZ1Down.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisZ1Down_MouseDown);
            this.buttonAxisZ1Down.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisZ1Down_MouseUp);
            // 
            // buttonAxisZ1Up
            // 
            this.buttonAxisZ1Up.BackColor = System.Drawing.Color.Lavender;
            this.buttonAxisZ1Up.Image = global::SLD200.Properties.Resources.UVW_VW_UP_Edited;
            this.buttonAxisZ1Up.Location = new System.Drawing.Point(26, 109);
            this.buttonAxisZ1Up.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisZ1Up.Name = "buttonAxisZ1Up";
            this.buttonAxisZ1Up.Size = new System.Drawing.Size(60, 54);
            this.buttonAxisZ1Up.TabIndex = 18;
            this.buttonAxisZ1Up.UseVisualStyleBackColor = false;
            this.buttonAxisZ1Up.Click += new System.EventHandler(this.buttonAxisZ1Up_Click);
            this.buttonAxisZ1Up.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisZ1Up_MouseDown);
            this.buttonAxisZ1Up.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisZ1Up_MouseUp);
            // 
            // baseLabel_Stacker0
            // 
            this.baseLabel_Stacker0.BackColor = System.Drawing.Color.LightGray;
            this.baseLabel_Stacker0.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_Stacker0.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Stacker0.Location = new System.Drawing.Point(117, 112);
            this.baseLabel_Stacker0.Name = "baseLabel_Stacker0";
            this.baseLabel_Stacker0.Size = new System.Drawing.Size(21, 102);
            this.baseLabel_Stacker0.TabIndex = 20;
            this.baseLabel_Stacker0.Text = "S\r\nT\r\nA\r\nC\r\nK\r\n\r\n[0]";
            this.baseLabel_Stacker0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel_Stacker1
            // 
            this.baseLabel_Stacker1.BackColor = System.Drawing.Color.LightGray;
            this.baseLabel_Stacker1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_Stacker1.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Stacker1.Location = new System.Drawing.Point(4, 112);
            this.baseLabel_Stacker1.Name = "baseLabel_Stacker1";
            this.baseLabel_Stacker1.Size = new System.Drawing.Size(21, 102);
            this.baseLabel_Stacker1.TabIndex = 19;
            this.baseLabel_Stacker1.Text = "S\r\nT\r\nA\r\nC\r\nK\r\n\r\n[1]";
            this.baseLabel_Stacker1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabelJogButtonComb_XZ
            // 
            this.baseLabelJogButtonComb_XZ.BackColor = System.Drawing.Color.LightSkyBlue;
            this.baseLabelJogButtonComb_XZ.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.baseLabelJogButtonComb_XZ.ForeColor = System.Drawing.Color.Black;
            this.baseLabelJogButtonComb_XZ.Location = new System.Drawing.Point(145, 5);
            this.baseLabelJogButtonComb_XZ.Name = "baseLabelJogButtonComb_XZ";
            this.baseLabelJogButtonComb_XZ.Size = new System.Drawing.Size(46, 20);
            this.baseLabelJogButtonComb_XZ.TabIndex = 14;
            this.baseLabelJogButtonComb_XZ.Text = "[XZ]";
            this.baseLabelJogButtonComb_XZ.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabelJogButtonComb_Title
            // 
            this.baseLabelJogButtonComb_Title.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.baseLabelJogButtonComb_Title.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.baseLabelJogButtonComb_Title.ForeColor = System.Drawing.Color.Black;
            this.baseLabelJogButtonComb_Title.Location = new System.Drawing.Point(1, 5);
            this.baseLabelJogButtonComb_Title.Name = "baseLabelJogButtonComb_Title";
            this.baseLabelJogButtonComb_Title.Size = new System.Drawing.Size(57, 20);
            this.baseLabelJogButtonComb_Title.TabIndex = 13;
            this.baseLabelJogButtonComb_Title.Text = "Transfer";
            this.baseLabelJogButtonComb_Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabelJogButtonComb
            // 
            this.baseLabelJogButtonComb.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelJogButtonComb.ForeColor = System.Drawing.Color.White;
            this.baseLabelJogButtonComb.Location = new System.Drawing.Point(62, 197);
            this.baseLabelJogButtonComb.Name = "baseLabelJogButtonComb";
            this.baseLabelJogButtonComb.Size = new System.Drawing.Size(53, 20);
            this.baseLabelJogButtonComb.TabIndex = 12;
            this.baseLabelJogButtonComb.Text = "X , Y";
            this.baseLabelJogButtonComb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // JogButtonCombinationXZ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseLabel_Stacker0);
            this.Controls.Add(this.baseLabel_Stacker1);
            this.Controls.Add(this.buttonAxisZ0Down);
            this.Controls.Add(this.buttonAxisZ0Up);
            this.Controls.Add(this.buttonAxisZ1Down);
            this.Controls.Add(this.buttonAxisZ1Up);
            this.Controls.Add(this.baseLabelJogButtonComb_XZ);
            this.Controls.Add(this.baseLabelJogButtonComb_Title);
            this.Controls.Add(this.baseLabelJogButtonComb);
            this.Controls.Add(this.buttonAxisYDown);
            this.Controls.Add(this.buttonAxisXUp);
            this.Controls.Add(this.buttonAxisYUp);
            this.Controls.Add(this.buttonAxisXDown);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "JogButtonCombinationXZ";
            this.Size = new System.Drawing.Size(200, 220);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonAxisYDown;
        private System.Windows.Forms.Button buttonAxisXUp;
        private System.Windows.Forms.Button buttonAxisXDown;
        private System.Windows.Forms.Button buttonAxisYUp;
        private BaseLabel baseLabelJogButtonComb;
        private BaseLabel baseLabelJogButtonComb_Title;
        private BaseLabel baseLabelJogButtonComb_XZ;
        private System.Windows.Forms.Button buttonAxisZ0Down;
        private System.Windows.Forms.Button buttonAxisZ0Up;
        private System.Windows.Forms.Button buttonAxisZ1Down;
        private System.Windows.Forms.Button buttonAxisZ1Up;
        private BaseLabel baseLabel_Stacker1;
        private BaseLabel baseLabel_Stacker0;
    }
}
