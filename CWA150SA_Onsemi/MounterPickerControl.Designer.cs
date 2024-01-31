namespace CWA150SA_Onsemi300
{
    partial class MounterPickerControl
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
            this.GroupBox = new System.Windows.Forms.GroupBox();
            this.baseButtonBlow = new CWA150SA_Onsemi300.BaseButton();
            this.baseButtonVacuum = new CWA150SA_Onsemi300.BaseButton();
            this.GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroupBox
            // 
            this.GroupBox.Controls.Add(this.baseButtonBlow);
            this.GroupBox.Controls.Add(this.baseButtonVacuum);
            this.GroupBox.ForeColor = System.Drawing.Color.White;
            this.GroupBox.Location = new System.Drawing.Point(3, 2);
            this.GroupBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GroupBox.Name = "GroupBox";
            this.GroupBox.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GroupBox.Size = new System.Drawing.Size(225, 119);
            this.GroupBox.TabIndex = 0;
            this.GroupBox.TabStop = false;
            this.GroupBox.Text = "groupBox1";
            // 
            // baseButtonBlow
            // 
            this.baseButtonBlow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonBlow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonBlow.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonBlow.Location = new System.Drawing.Point(122, 35);
            this.baseButtonBlow.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseButtonBlow.Name = "baseButtonBlow";
            this.baseButtonBlow.Size = new System.Drawing.Size(88, 37);
            this.baseButtonBlow.TabIndex = 2;
            this.baseButtonBlow.Text = "Blow";
            this.baseButtonBlow.UseVisualStyleBackColor = false;
            this.baseButtonBlow.Click += new System.EventHandler(this.baseButtonBlow_Click);
            // 
            // baseButtonVacuum
            // 
            this.baseButtonVacuum.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonVacuum.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonVacuum.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonVacuum.Location = new System.Drawing.Point(17, 35);
            this.baseButtonVacuum.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseButtonVacuum.Name = "baseButtonVacuum";
            this.baseButtonVacuum.Size = new System.Drawing.Size(88, 37);
            this.baseButtonVacuum.TabIndex = 1;
            this.baseButtonVacuum.Text = "Vacuum";
            this.baseButtonVacuum.UseVisualStyleBackColor = false;
            this.baseButtonVacuum.Click += new System.EventHandler(this.baseButtonVacuum_Click);
            // 
            // MounterPickerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.GroupBox);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MounterPickerControl";
            this.Size = new System.Drawing.Size(230, 126);
            this.GroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.GroupBox GroupBox;
        private BaseButton baseButtonBlow;
        private BaseButton baseButtonVacuum;
    }
}
