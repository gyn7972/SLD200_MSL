namespace SLD200_MSL
{
    partial class JogButtonXY
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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.baseLabelX = new SLD200_MSL.BaseLabel();
            this.baseLabelY = new SLD200_MSL.BaseLabel();
            this.buttonAxisXLeft = new System.Windows.Forms.Button();
            this.buttonAxisXRight = new System.Windows.Forms.Button();
            this.buttonAxisYFwd = new System.Windows.Forms.Button();
            this.buttonAxisYBwd = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // baseLabelX
            // 
            this.baseLabelX.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelX.ForeColor = System.Drawing.Color.White;
            this.baseLabelX.Location = new System.Drawing.Point(55, 55);
            this.baseLabelX.Name = "baseLabelX";
            this.baseLabelX.Size = new System.Drawing.Size(64, 16);
            this.baseLabelX.TabIndex = 9;
            this.baseLabelX.Text = "Axis X";
            // 
            // baseLabelY
            // 
            this.baseLabelY.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelY.ForeColor = System.Drawing.Color.White;
            this.baseLabelY.Location = new System.Drawing.Point(55, 77);
            this.baseLabelY.Name = "baseLabelY";
            this.baseLabelY.Size = new System.Drawing.Size(64, 16);
            this.baseLabelY.TabIndex = 18;
            this.baseLabelY.Text = "Axis Y";
            // 
            // buttonAxisXLeft
            // 
            this.buttonAxisXLeft.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonAxisXLeft.Image = global::SLD200.Properties.Resources.Left;
            this.buttonAxisXLeft.Location = new System.Drawing.Point(4, 51);
            this.buttonAxisXLeft.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisXLeft.Name = "buttonAxisXLeft";
            this.buttonAxisXLeft.Size = new System.Drawing.Size(47, 46);
            this.buttonAxisXLeft.TabIndex = 16;
            this.buttonAxisXLeft.UseVisualStyleBackColor = false;
            this.buttonAxisXLeft.Click += new System.EventHandler(this.buttonMoveLeft_Click);
            this.buttonAxisXLeft.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXLeft_MouseDown);
            this.buttonAxisXLeft.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXLeft_MouseUp);
            // 
            // buttonAxisXRight
            // 
            this.buttonAxisXRight.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonAxisXRight.Image = global::SLD200.Properties.Resources.Right;
            this.buttonAxisXRight.Location = new System.Drawing.Point(122, 51);
            this.buttonAxisXRight.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisXRight.Name = "buttonAxisXRight";
            this.buttonAxisXRight.Size = new System.Drawing.Size(48, 46);
            this.buttonAxisXRight.TabIndex = 11;
            this.buttonAxisXRight.UseVisualStyleBackColor = false;
            this.buttonAxisXRight.Click += new System.EventHandler(this.buttonMoveRight_Click);
            this.buttonAxisXRight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXRight_MouseDown);
            this.buttonAxisXRight.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXRight_MouseUp);
            // 
            // buttonAxisYFwd
            // 
            this.buttonAxisYFwd.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonAxisYFwd.Image = global::SLD200.Properties.Resources.Down;
            this.buttonAxisYFwd.Location = new System.Drawing.Point(63, 99);
            this.buttonAxisYFwd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisYFwd.Name = "buttonAxisYFwd";
            this.buttonAxisYFwd.Size = new System.Drawing.Size(47, 46);
            this.buttonAxisYFwd.TabIndex = 12;
            this.buttonAxisYFwd.UseVisualStyleBackColor = false;
            this.buttonAxisYFwd.Click += new System.EventHandler(this.buttonMoveForward_Click);
            this.buttonAxisYFwd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisYFwd_MouseDown);
            this.buttonAxisYFwd.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisYFwd_MouseUp);
            // 
            // buttonAxisYBwd
            // 
            this.buttonAxisYBwd.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonAxisYBwd.Image = global::SLD200.Properties.Resources.Up;
            this.buttonAxisYBwd.Location = new System.Drawing.Point(63, 4);
            this.buttonAxisYBwd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisYBwd.Name = "buttonAxisYBwd";
            this.buttonAxisYBwd.Size = new System.Drawing.Size(47, 46);
            this.buttonAxisYBwd.TabIndex = 15;
            this.buttonAxisYBwd.UseVisualStyleBackColor = false;
            this.buttonAxisYBwd.Click += new System.EventHandler(this.buttonMoveBackward_Click);
            this.buttonAxisYBwd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisYBwd_MouseDown);
            this.buttonAxisYBwd.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisYBwd_MouseUp);
            // 
            // JogButtonXY
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.Controls.Add(this.baseLabelY);
            this.Controls.Add(this.buttonAxisXRight);
            this.Controls.Add(this.buttonAxisYFwd);
            this.Controls.Add(this.buttonAxisYBwd);
            this.Controls.Add(this.buttonAxisXLeft);
            this.Controls.Add(this.baseLabelX);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "JogButtonXY";
            this.Size = new System.Drawing.Size(175, 148);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private BaseLabel baseLabelX;
        private System.Windows.Forms.Button buttonAxisXRight;
        private System.Windows.Forms.Button buttonAxisYFwd;
        private System.Windows.Forms.Button buttonAxisYBwd;
        private System.Windows.Forms.Button buttonAxisXLeft;
        private BaseLabel baseLabelY;
    }
}
