namespace CWA150SA_Onsemi300
{
    partial class RotaryMotor
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
            this.buttonStop = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.groupBoxRotaryMotor = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonCCW = new System.Windows.Forms.Button();
            this.buttonCW = new System.Windows.Forms.Button();
            this.groupBoxRotaryMotor.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonStop
            // 
            this.buttonStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.buttonStop.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonStop.ForeColor = System.Drawing.Color.White;
            this.buttonStop.Location = new System.Drawing.Point(146, 26);
            this.buttonStop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(67, 56);
            this.buttonStop.TabIndex = 8;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = false;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // groupBoxRotaryMotor
            // 
            this.groupBoxRotaryMotor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.groupBoxRotaryMotor.Controls.Add(this.textBox1);
            this.groupBoxRotaryMotor.Controls.Add(this.buttonCCW);
            this.groupBoxRotaryMotor.Controls.Add(this.buttonStop);
            this.groupBoxRotaryMotor.Controls.Add(this.buttonCW);
            this.groupBoxRotaryMotor.ForeColor = System.Drawing.Color.White;
            this.groupBoxRotaryMotor.Location = new System.Drawing.Point(3, 3);
            this.groupBoxRotaryMotor.Name = "groupBoxRotaryMotor";
            this.groupBoxRotaryMotor.Size = new System.Drawing.Size(225, 125);
            this.groupBoxRotaryMotor.TabIndex = 9;
            this.groupBoxRotaryMotor.TabStop = false;
            this.groupBoxRotaryMotor.Text = " Rotary Motor ";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 92);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(118, 21);
            this.textBox1.TabIndex = 9;
            // 
            // buttonCCW
            // 
            this.buttonCCW.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonCCW.Image = global::CWA150SA_Onsemi.Properties.Resources.CCW;
            this.buttonCCW.Location = new System.Drawing.Point(12, 26);
            this.buttonCCW.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonCCW.Name = "buttonCCW";
            this.buttonCCW.Size = new System.Drawing.Size(56, 56);
            this.buttonCCW.TabIndex = 8;
            this.buttonCCW.UseVisualStyleBackColor = false;
            this.buttonCCW.Click += new System.EventHandler(this.buttonCCW_Click);
            this.buttonCCW.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXUp_MouseDown);
            this.buttonCCW.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXUp_MouseUp);
            // 
            // buttonCW
            // 
            this.buttonCW.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonCW.Image = global::CWA150SA_Onsemi.Properties.Resources.CW;
            this.buttonCW.Location = new System.Drawing.Point(74, 26);
            this.buttonCW.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonCW.Name = "buttonCW";
            this.buttonCW.Size = new System.Drawing.Size(56, 56);
            this.buttonCW.TabIndex = 7;
            this.buttonCW.UseVisualStyleBackColor = false;
            this.buttonCW.Click += new System.EventHandler(this.buttonCW_Click);
            this.buttonCW.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDown_MouseDown);
            this.buttonCW.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDown_MouseUp);
            // 
            // RotaryMotor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxRotaryMotor);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "RotaryMotor";
            this.Size = new System.Drawing.Size(230, 130);
            this.groupBoxRotaryMotor.ResumeLayout(false);
            this.groupBoxRotaryMotor.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonCW;
        private System.Windows.Forms.Button buttonCCW;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.GroupBox groupBoxRotaryMotor;
        private System.Windows.Forms.TextBox textBox1;
    }
}
