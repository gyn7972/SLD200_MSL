namespace CWA150SA_Onsemi300
{
    partial class VisionCompensatorParameterControl
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
            this.baseGroupBoxParameter = new CWA150SA_Onsemi300.BaseGroupBox();
            this.baseGroupBoxGrid = new CWA150SA_Onsemi300.BaseGroupBox();
            this.baseToggleButtonInvertedY = new CWA150SA_Onsemi300.BaseToggleButton();
            this.baseToggleButtonInvertedX = new CWA150SA_Onsemi300.BaseToggleButton();
            this.basePropertyGridParameter = new CWA150SA_Onsemi300.BasePropertyGrid();
            this.baseGroupBoxParameter.SuspendLayout();
            this.baseGroupBoxGrid.SuspendLayout();
            this.SuspendLayout();
            // 
            // baseGroupBoxParameter
            // 
            this.baseGroupBoxParameter.Controls.Add(this.baseGroupBoxGrid);
            this.baseGroupBoxParameter.Controls.Add(this.basePropertyGridParameter);
            this.baseGroupBoxParameter.ForeColor = System.Drawing.Color.White;
            this.baseGroupBoxParameter.Location = new System.Drawing.Point(0, 0);
            this.baseGroupBoxParameter.Name = "baseGroupBoxParameter";
            this.baseGroupBoxParameter.Size = new System.Drawing.Size(440, 340);
            this.baseGroupBoxParameter.TabIndex = 0;
            this.baseGroupBoxParameter.TabStop = false;
            this.baseGroupBoxParameter.Text = " Parameter ";
            // 
            // baseGroupBoxGrid
            // 
            this.baseGroupBoxGrid.Controls.Add(this.baseToggleButtonInvertedY);
            this.baseGroupBoxGrid.Controls.Add(this.baseToggleButtonInvertedX);
            this.baseGroupBoxGrid.ForeColor = System.Drawing.Color.White;
            this.baseGroupBoxGrid.Location = new System.Drawing.Point(212, 267);
            this.baseGroupBoxGrid.Name = "baseGroupBoxGrid";
            this.baseGroupBoxGrid.Size = new System.Drawing.Size(222, 61);
            this.baseGroupBoxGrid.TabIndex = 10;
            this.baseGroupBoxGrid.TabStop = false;
            this.baseGroupBoxGrid.Text = "GridXY Inverted";
            // 
            // baseToggleButtonInvertedY
            // 
            this.baseToggleButtonInvertedY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseToggleButtonInvertedY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButtonInvertedY.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseToggleButtonInvertedY.Location = new System.Drawing.Point(114, 20);
            this.baseToggleButtonInvertedY.Name = "baseToggleButtonInvertedY";
            this.baseToggleButtonInvertedY.Size = new System.Drawing.Size(100, 30);
            this.baseToggleButtonInvertedY.TabIndex = 9;
            this.baseToggleButtonInvertedY.Text = "InvertedY";
            this.baseToggleButtonInvertedY.UseVisualStyleBackColor = false;
            // 
            // baseToggleButtonInvertedX
            // 
            this.baseToggleButtonInvertedX.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseToggleButtonInvertedX.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButtonInvertedX.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseToggleButtonInvertedX.Location = new System.Drawing.Point(8, 20);
            this.baseToggleButtonInvertedX.Name = "baseToggleButtonInvertedX";
            this.baseToggleButtonInvertedX.Size = new System.Drawing.Size(100, 30);
            this.baseToggleButtonInvertedX.TabIndex = 8;
            this.baseToggleButtonInvertedX.Text = "InvertedX";
            this.baseToggleButtonInvertedX.UseVisualStyleBackColor = false;
            // 
            // basePropertyGridParameter
            // 
            this.basePropertyGridParameter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.basePropertyGridParameter.CategoryForeColor = System.Drawing.Color.Black;
            this.basePropertyGridParameter.CategorySplitterColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.basePropertyGridParameter.CommandsBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.basePropertyGridParameter.HelpBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.basePropertyGridParameter.HelpForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.basePropertyGridParameter.Location = new System.Drawing.Point(6, 20);
            this.basePropertyGridParameter.Name = "basePropertyGridParameter";
            this.basePropertyGridParameter.SelectedItemWithFocusBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.basePropertyGridParameter.Size = new System.Drawing.Size(428, 230);
            this.basePropertyGridParameter.TabIndex = 0;
            this.basePropertyGridParameter.ViewBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.basePropertyGridParameter.ViewBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            // 
            // VisionCompensatorParameterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseGroupBoxParameter);
            this.Name = "VisionCompensatorParameterControl";
            this.Size = new System.Drawing.Size(440, 340);
            this.baseGroupBoxParameter.ResumeLayout(false);
            this.baseGroupBoxGrid.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private BaseGroupBox baseGroupBoxParameter;
        private BasePropertyGrid basePropertyGridParameter;
        private BaseGroupBox baseGroupBoxGrid;
        private BaseToggleButton baseToggleButtonInvertedY;
        private BaseToggleButton baseToggleButtonInvertedX;
    }
}
