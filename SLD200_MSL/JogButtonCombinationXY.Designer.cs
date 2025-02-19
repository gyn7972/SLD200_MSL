namespace SLD200_MSL
{
    partial class JogButtonCombinationXY
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
            this.buttonAxisXUpYUp = new System.Windows.Forms.Button();
            this.buttonAxisXDownYDown = new System.Windows.Forms.Button();
            this.buttonAxisYUp = new System.Windows.Forms.Button();
            this.buttonAxisXDown = new System.Windows.Forms.Button();
            this.buttonAxisXDownYUp = new System.Windows.Forms.Button();
            this.baseLabelJogButtonComb_XY = new SLD200_MSL.BaseLabel();
            this.baseLabelJogButtonComb_Title = new SLD200_MSL.BaseLabel();
            this.baseLabelJogButtonComb = new SLD200_MSL.BaseLabel();
            this.SuspendLayout();
            // 
            // buttonAxisXUpYDown
            // 
            this.buttonAxisXUpYDown.BackColor = System.Drawing.Color.DimGray;
            this.buttonAxisXUpYDown.Image = global::SLD200.Properties.Resources.RightDown;
            this.buttonAxisXUpYDown.Location = new System.Drawing.Point(148, 148);
            this.buttonAxisXUpYDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisXUpYDown.Name = "buttonAxisXUpYDown";
            this.buttonAxisXUpYDown.Size = new System.Drawing.Size(69, 69);
            this.buttonAxisXUpYDown.TabIndex = 4;
            this.buttonAxisXUpYDown.UseVisualStyleBackColor = false;
            this.buttonAxisXUpYDown.Click += new System.EventHandler(this.buttonAxisXUpYDown_Click);
            this.buttonAxisXUpYDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXUpYDown_MouseDown);
            this.buttonAxisXUpYDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXUpYDown_MouseUp);
            // 
            // buttonAxisYDown
            // 
            this.buttonAxisYDown.BackColor = System.Drawing.Color.DimGray;
            this.buttonAxisYDown.Image = global::SLD200.Properties.Resources.Down;
            this.buttonAxisYDown.Location = new System.Drawing.Point(75, 148);
            this.buttonAxisYDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisYDown.Name = "buttonAxisYDown";
            this.buttonAxisYDown.Size = new System.Drawing.Size(69, 69);
            this.buttonAxisYDown.TabIndex = 5;
            this.buttonAxisYDown.UseVisualStyleBackColor = false;
            this.buttonAxisYDown.Click += new System.EventHandler(this.buttonAxisYDown_Click);
            this.buttonAxisYDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisYDown_MouseDown);
            this.buttonAxisYDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisYDown_MouseUp);
            // 
            // buttonAxisXUp
            // 
            this.buttonAxisXUp.BackColor = System.Drawing.Color.DimGray;
            this.buttonAxisXUp.Image = global::SLD200.Properties.Resources.Right;
            this.buttonAxisXUp.Location = new System.Drawing.Point(148, 75);
            this.buttonAxisXUp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisXUp.Name = "buttonAxisXUp";
            this.buttonAxisXUp.Size = new System.Drawing.Size(69, 69);
            this.buttonAxisXUp.TabIndex = 6;
            this.buttonAxisXUp.UseVisualStyleBackColor = false;
            this.buttonAxisXUp.Click += new System.EventHandler(this.ButtonAxisXRight_Click);
            this.buttonAxisXUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXUp_MouseDown);
            this.buttonAxisXUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXUp_MouseUp);
            // 
            // buttonAxisXUpYUp
            // 
            this.buttonAxisXUpYUp.BackColor = System.Drawing.Color.DimGray;
            this.buttonAxisXUpYUp.Image = global::SLD200.Properties.Resources.RightUp;
            this.buttonAxisXUpYUp.Location = new System.Drawing.Point(148, 2);
            this.buttonAxisXUpYUp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisXUpYUp.Name = "buttonAxisXUpYUp";
            this.buttonAxisXUpYUp.Size = new System.Drawing.Size(69, 69);
            this.buttonAxisXUpYUp.TabIndex = 7;
            this.buttonAxisXUpYUp.UseVisualStyleBackColor = false;
            this.buttonAxisXUpYUp.Click += new System.EventHandler(this.buttonAxisXUpYUp_Click);
            this.buttonAxisXUpYUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXUpYUp_MouseDown);
            this.buttonAxisXUpYUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXUpYUp_MouseUp);
            // 
            // buttonAxisXDownYDown
            // 
            this.buttonAxisXDownYDown.BackColor = System.Drawing.Color.DimGray;
            this.buttonAxisXDownYDown.Image = global::SLD200.Properties.Resources.LeftDown;
            this.buttonAxisXDownYDown.Location = new System.Drawing.Point(3, 148);
            this.buttonAxisXDownYDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisXDownYDown.Name = "buttonAxisXDownYDown";
            this.buttonAxisXDownYDown.Size = new System.Drawing.Size(69, 69);
            this.buttonAxisXDownYDown.TabIndex = 8;
            this.buttonAxisXDownYDown.UseVisualStyleBackColor = false;
            this.buttonAxisXDownYDown.Click += new System.EventHandler(this.buttonAxisXDownYDown_Click);
            this.buttonAxisXDownYDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDownYDown_MouseDown);
            this.buttonAxisXDownYDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDownYDown_MouseUp);
            // 
            // buttonAxisYUp
            // 
            this.buttonAxisYUp.BackColor = System.Drawing.Color.DimGray;
            this.buttonAxisYUp.Image = global::SLD200.Properties.Resources.Up;
            this.buttonAxisYUp.Location = new System.Drawing.Point(75, 2);
            this.buttonAxisYUp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisYUp.Name = "buttonAxisYUp";
            this.buttonAxisYUp.Size = new System.Drawing.Size(69, 69);
            this.buttonAxisYUp.TabIndex = 9;
            this.buttonAxisYUp.UseVisualStyleBackColor = false;
            this.buttonAxisYUp.Click += new System.EventHandler(this.buttonAxisYUp_Click);
            this.buttonAxisYUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisYUp_MouseDown);
            this.buttonAxisYUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisYUp_MouseUp);
            // 
            // buttonAxisXDown
            // 
            this.buttonAxisXDown.BackColor = System.Drawing.Color.DimGray;
            this.buttonAxisXDown.Image = global::SLD200.Properties.Resources.Left;
            this.buttonAxisXDown.Location = new System.Drawing.Point(3, 75);
            this.buttonAxisXDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisXDown.Name = "buttonAxisXDown";
            this.buttonAxisXDown.Size = new System.Drawing.Size(69, 69);
            this.buttonAxisXDown.TabIndex = 10;
            this.buttonAxisXDown.UseVisualStyleBackColor = false;
            this.buttonAxisXDown.Click += new System.EventHandler(this.ButtonAxisXLeft_Click);
            this.buttonAxisXDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDown_MouseDown);
            this.buttonAxisXDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDown_MouseUp);
            // 
            // buttonAxisXDownYUp
            // 
            this.buttonAxisXDownYUp.BackColor = System.Drawing.Color.DimGray;
            this.buttonAxisXDownYUp.Image = global::SLD200.Properties.Resources.LeftUp;
            this.buttonAxisXDownYUp.Location = new System.Drawing.Point(3, 2);
            this.buttonAxisXDownYUp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisXDownYUp.Name = "buttonAxisXDownYUp";
            this.buttonAxisXDownYUp.Size = new System.Drawing.Size(69, 69);
            this.buttonAxisXDownYUp.TabIndex = 11;
            this.buttonAxisXDownYUp.UseVisualStyleBackColor = false;
            this.buttonAxisXDownYUp.Click += new System.EventHandler(this.buttonAxisXDownYUp_Click);
            this.buttonAxisXDownYUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDownYUp_MouseDown);
            this.buttonAxisXDownYUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDownYUp_MouseUp);
            // 
            // baseLabelJogButtonComb_XY
            // 
            this.baseLabelJogButtonComb_XY.BackColor = System.Drawing.Color.LightSkyBlue;
            this.baseLabelJogButtonComb_XY.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.baseLabelJogButtonComb_XY.ForeColor = System.Drawing.Color.Black;
            this.baseLabelJogButtonComb_XY.Location = new System.Drawing.Point(85, 110);
            this.baseLabelJogButtonComb_XY.Name = "baseLabelJogButtonComb_XY";
            this.baseLabelJogButtonComb_XY.Size = new System.Drawing.Size(49, 26);
            this.baseLabelJogButtonComb_XY.TabIndex = 14;
            this.baseLabelJogButtonComb_XY.Text = "[XY]";
            this.baseLabelJogButtonComb_XY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabelJogButtonComb_Title
            // 
            this.baseLabelJogButtonComb_Title.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.baseLabelJogButtonComb_Title.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.baseLabelJogButtonComb_Title.ForeColor = System.Drawing.Color.Black;
            this.baseLabelJogButtonComb_Title.Location = new System.Drawing.Point(85, 82);
            this.baseLabelJogButtonComb_Title.Name = "baseLabelJogButtonComb_Title";
            this.baseLabelJogButtonComb_Title.Size = new System.Drawing.Size(49, 26);
            this.baseLabelJogButtonComb_Title.TabIndex = 13;
            this.baseLabelJogButtonComb_Title.Text = "Table";
            this.baseLabelJogButtonComb_Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabelJogButtonComb
            // 
            this.baseLabelJogButtonComb.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelJogButtonComb.ForeColor = System.Drawing.Color.White;
            this.baseLabelJogButtonComb.Location = new System.Drawing.Point(72, 209);
            this.baseLabelJogButtonComb.Name = "baseLabelJogButtonComb";
            this.baseLabelJogButtonComb.Size = new System.Drawing.Size(53, 20);
            this.baseLabelJogButtonComb.TabIndex = 12;
            this.baseLabelJogButtonComb.Text = "X , Y";
            this.baseLabelJogButtonComb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // JogButtonCombinationXY
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseLabelJogButtonComb_XY);
            this.Controls.Add(this.baseLabelJogButtonComb_Title);
            this.Controls.Add(this.baseLabelJogButtonComb);
            this.Controls.Add(this.buttonAxisXUpYDown);
            this.Controls.Add(this.buttonAxisYDown);
            this.Controls.Add(this.buttonAxisXUp);
            this.Controls.Add(this.buttonAxisXUpYUp);
            this.Controls.Add(this.buttonAxisXDownYDown);
            this.Controls.Add(this.buttonAxisYUp);
            this.Controls.Add(this.buttonAxisXDown);
            this.Controls.Add(this.buttonAxisXDownYUp);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "JogButtonCombinationXY";
            this.Size = new System.Drawing.Size(220, 220);
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
        public BaseLabel baseLabelJogButtonComb_Title;
        private BaseLabel baseLabelJogButtonComb_XY;
    }
}
