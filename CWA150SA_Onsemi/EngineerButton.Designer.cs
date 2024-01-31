namespace CWA150SA_Onsemi300
{
    partial class EngineerButton
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
            this.gruopBoxEngineer = new System.Windows.Forms.GroupBox();
            this.baseButtonEngineer = new CWA150SA_Onsemi300.BaseButton();
            this.gruopBoxEngineer.SuspendLayout();
            this.SuspendLayout();
            // 
            // gruopBoxEngineer
            // 
            this.gruopBoxEngineer.Controls.Add(this.baseButtonEngineer);
            this.gruopBoxEngineer.Location = new System.Drawing.Point(3, 2);
            this.gruopBoxEngineer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gruopBoxEngineer.Name = "gruopBoxEngineer";
            this.gruopBoxEngineer.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gruopBoxEngineer.Size = new System.Drawing.Size(111, 45);
            this.gruopBoxEngineer.TabIndex = 0;
            this.gruopBoxEngineer.TabStop = false;
            this.gruopBoxEngineer.Text = " Engineer ";
            // 
            // baseButtonEngineer
            // 
            this.baseButtonEngineer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonEngineer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonEngineer.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonEngineer.Location = new System.Drawing.Point(5, 17);
            this.baseButtonEngineer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseButtonEngineer.Name = "baseButtonEngineer";
            this.baseButtonEngineer.Size = new System.Drawing.Size(101, 24);
            this.baseButtonEngineer.TabIndex = 0;
            this.baseButtonEngineer.Text = "Engineer";
            this.baseButtonEngineer.UseVisualStyleBackColor = false;
            this.baseButtonEngineer.Click += new System.EventHandler(this.baseButton1_Click);
            // 
            // EngineerButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gruopBoxEngineer);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "EngineerButton";
            this.Size = new System.Drawing.Size(116, 50);
            this.gruopBoxEngineer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gruopBoxEngineer;
        private BaseButton baseButtonEngineer;
    }
}
