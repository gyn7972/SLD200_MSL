namespace SLD200_MSL
{
    partial class NeedleCalibrationJogControl
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
            this.buttonAxisXRight = new System.Windows.Forms.Button();
            this.buttonAxisYFwd = new System.Windows.Forms.Button();
            this.buttonAxisYBwd = new System.Windows.Forms.Button();
            this.buttonAxisXLeft = new System.Windows.Forms.Button();
            this.baseLabelY = new SLD200_MSL.BaseLabel();
            this.baseLabelX = new SLD200_MSL.BaseLabel();
            this.baseToggleButton01 = new SLD200_MSL.BaseToggleButton();
            this.baseToggleButton001 = new SLD200_MSL.BaseToggleButton();
            this.baseToggleButton0001 = new SLD200_MSL.BaseToggleButton();
            this.SuspendLayout();
            // 
            // buttonAxisXRight
            // 
            this.buttonAxisXRight.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonAxisXRight.Image = global::SLD200.Properties.Resources.Right;
            this.buttonAxisXRight.Location = new System.Drawing.Point(122, 51);
            this.buttonAxisXRight.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisXRight.Name = "buttonAxisXRight";
            this.buttonAxisXRight.Size = new System.Drawing.Size(48, 46);
            this.buttonAxisXRight.TabIndex = 17;
            this.buttonAxisXRight.UseVisualStyleBackColor = false;
            this.buttonAxisXRight.Click += new System.EventHandler(this.buttonAxisXRight_Click);
            // 
            // buttonAxisYFwd
            // 
            this.buttonAxisYFwd.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonAxisYFwd.Image = global::SLD200.Properties.Resources.Down;
            this.buttonAxisYFwd.Location = new System.Drawing.Point(63, 99);
            this.buttonAxisYFwd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisYFwd.Name = "buttonAxisYFwd";
            this.buttonAxisYFwd.Size = new System.Drawing.Size(47, 46);
            this.buttonAxisYFwd.TabIndex = 18;
            this.buttonAxisYFwd.UseVisualStyleBackColor = false;
            this.buttonAxisYFwd.Click += new System.EventHandler(this.buttonAxisYFwd_Click);
            // 
            // buttonAxisYBwd
            // 
            this.buttonAxisYBwd.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonAxisYBwd.Image = global::SLD200.Properties.Resources.Up;
            this.buttonAxisYBwd.Location = new System.Drawing.Point(63, 4);
            this.buttonAxisYBwd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisYBwd.Name = "buttonAxisYBwd";
            this.buttonAxisYBwd.Size = new System.Drawing.Size(47, 46);
            this.buttonAxisYBwd.TabIndex = 19;
            this.buttonAxisYBwd.UseVisualStyleBackColor = false;
            this.buttonAxisYBwd.Click += new System.EventHandler(this.buttonAxisYBwd_Click);
            // 
            // buttonAxisXLeft
            // 
            this.buttonAxisXLeft.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonAxisXLeft.Image = global::SLD200.Properties.Resources.Left;
            this.buttonAxisXLeft.Location = new System.Drawing.Point(4, 51);
            this.buttonAxisXLeft.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisXLeft.Name = "buttonAxisXLeft";
            this.buttonAxisXLeft.Size = new System.Drawing.Size(47, 46);
            this.buttonAxisXLeft.TabIndex = 20;
            this.buttonAxisXLeft.UseVisualStyleBackColor = false;
            this.buttonAxisXLeft.Click += new System.EventHandler(this.buttonAxisXLeft_Click);
            // 
            // baseLabelY
            // 
            this.baseLabelY.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelY.ForeColor = System.Drawing.Color.White;
            this.baseLabelY.Location = new System.Drawing.Point(55, 77);
            this.baseLabelY.Name = "baseLabelY";
            this.baseLabelY.Size = new System.Drawing.Size(64, 16);
            this.baseLabelY.TabIndex = 22;
            this.baseLabelY.Text = "Axis Y";
            // 
            // baseLabelX
            // 
            this.baseLabelX.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelX.ForeColor = System.Drawing.Color.White;
            this.baseLabelX.Location = new System.Drawing.Point(55, 55);
            this.baseLabelX.Name = "baseLabelX";
            this.baseLabelX.Size = new System.Drawing.Size(64, 16);
            this.baseLabelX.TabIndex = 21;
            this.baseLabelX.Text = "Axis X";
            // 
            // baseToggleButton01
            // 
            this.baseToggleButton01.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseToggleButton01.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButton01.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseToggleButton01.Location = new System.Drawing.Point(190, 4);
            this.baseToggleButton01.Name = "baseToggleButton01";
            this.baseToggleButton01.Size = new System.Drawing.Size(74, 30);
            this.baseToggleButton01.TabIndex = 23;
            this.baseToggleButton01.Text = "0.1 mm";
            this.baseToggleButton01.UseVisualStyleBackColor = false;
            this.baseToggleButton01.Click += new System.EventHandler(this.baseToggleButton01_Click);
            // 
            // baseToggleButton001
            // 
            this.baseToggleButton001.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseToggleButton001.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButton001.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseToggleButton001.Location = new System.Drawing.Point(190, 55);
            this.baseToggleButton001.Name = "baseToggleButton001";
            this.baseToggleButton001.Size = new System.Drawing.Size(74, 30);
            this.baseToggleButton001.TabIndex = 24;
            this.baseToggleButton001.Text = "0.01 mm";
            this.baseToggleButton001.UseVisualStyleBackColor = false;
            this.baseToggleButton001.Click += new System.EventHandler(this.baseToggleButton001_Click);
            // 
            // baseToggleButton0001
            // 
            this.baseToggleButton0001.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseToggleButton0001.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButton0001.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseToggleButton0001.Location = new System.Drawing.Point(190, 106);
            this.baseToggleButton0001.Name = "baseToggleButton0001";
            this.baseToggleButton0001.Size = new System.Drawing.Size(74, 30);
            this.baseToggleButton0001.TabIndex = 25;
            this.baseToggleButton0001.Text = "0.001 mm";
            this.baseToggleButton0001.UseVisualStyleBackColor = false;
            this.baseToggleButton0001.Click += new System.EventHandler(this.baseToggleButton0001_Click);
            // 
            // NeedleCalibrationJogControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.Controls.Add(this.baseToggleButton0001);
            this.Controls.Add(this.baseToggleButton001);
            this.Controls.Add(this.baseToggleButton01);
            this.Controls.Add(this.baseLabelY);
            this.Controls.Add(this.baseLabelX);
            this.Controls.Add(this.buttonAxisXRight);
            this.Controls.Add(this.buttonAxisYFwd);
            this.Controls.Add(this.buttonAxisYBwd);
            this.Controls.Add(this.buttonAxisXLeft);
            this.Name = "NeedleCalibrationJogControl";
            this.Size = new System.Drawing.Size(267, 148);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonAxisXRight;
        private System.Windows.Forms.Button buttonAxisYFwd;
        private System.Windows.Forms.Button buttonAxisYBwd;
        private System.Windows.Forms.Button buttonAxisXLeft;
        private BaseLabel baseLabelY;
        private BaseLabel baseLabelX;
        private BaseToggleButton baseToggleButton01;
        private BaseToggleButton baseToggleButton001;
        private BaseToggleButton baseToggleButton0001;
    }
}
