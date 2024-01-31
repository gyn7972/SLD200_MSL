namespace CWA150SA_Onsemi300
{
    partial class ReelFeeder
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
            this.groupBoxCompletedReelMotor = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonCCW = new System.Windows.Forms.Button();
            this.buttonCW = new System.Windows.Forms.Button();
            this.groupBoxCoverTapeMotor = new System.Windows.Forms.GroupBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBoxChipFeedingMotor = new System.Windows.Forms.GroupBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.groupBoxCompletedReelMotor.SuspendLayout();
            this.groupBoxCoverTapeMotor.SuspendLayout();
            this.groupBoxChipFeedingMotor.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonStop
            // 
            this.buttonStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.buttonStop.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
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
            // groupBoxCompletedReelMotor
            // 
            this.groupBoxCompletedReelMotor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.groupBoxCompletedReelMotor.Controls.Add(this.textBox1);
            this.groupBoxCompletedReelMotor.Controls.Add(this.buttonCCW);
            this.groupBoxCompletedReelMotor.Controls.Add(this.buttonStop);
            this.groupBoxCompletedReelMotor.Controls.Add(this.buttonCW);
            this.groupBoxCompletedReelMotor.ForeColor = System.Drawing.Color.White;
            this.groupBoxCompletedReelMotor.Location = new System.Drawing.Point(3, 149);
            this.groupBoxCompletedReelMotor.Name = "groupBoxCompletedReelMotor";
            this.groupBoxCompletedReelMotor.Size = new System.Drawing.Size(223, 125);
            this.groupBoxCompletedReelMotor.TabIndex = 9;
            this.groupBoxCompletedReelMotor.TabStop = false;
            this.groupBoxCompletedReelMotor.Text = " Completed Reel Motor ";
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
            // groupBoxCoverTapeMotor
            // 
            this.groupBoxCoverTapeMotor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.groupBoxCoverTapeMotor.Controls.Add(this.textBox2);
            this.groupBoxCoverTapeMotor.Controls.Add(this.button1);
            this.groupBoxCoverTapeMotor.Controls.Add(this.button2);
            this.groupBoxCoverTapeMotor.Controls.Add(this.button3);
            this.groupBoxCoverTapeMotor.ForeColor = System.Drawing.Color.White;
            this.groupBoxCoverTapeMotor.Location = new System.Drawing.Point(3, 3);
            this.groupBoxCoverTapeMotor.Name = "groupBoxCoverTapeMotor";
            this.groupBoxCoverTapeMotor.Size = new System.Drawing.Size(223, 125);
            this.groupBoxCoverTapeMotor.TabIndex = 10;
            this.groupBoxCoverTapeMotor.TabStop = false;
            this.groupBoxCoverTapeMotor.Text = " Cover Tape Motor ";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(12, 92);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(118, 21);
            this.textBox2.TabIndex = 9;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.button1.Image = global::CWA150SA_Onsemi.Properties.Resources.CCW;
            this.button1.Location = new System.Drawing.Point(12, 26);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(56, 56);
            this.button1.TabIndex = 8;
            this.button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.button2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(146, 26);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(67, 56);
            this.button2.TabIndex = 8;
            this.button2.Text = "Stop";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.button3.Image = global::CWA150SA_Onsemi.Properties.Resources.CW;
            this.button3.Location = new System.Drawing.Point(74, 26);
            this.button3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(56, 56);
            this.button3.TabIndex = 7;
            this.button3.UseVisualStyleBackColor = false;
            // 
            // groupBoxChipFeedingMotor
            // 
            this.groupBoxChipFeedingMotor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.groupBoxChipFeedingMotor.Controls.Add(this.textBox3);
            this.groupBoxChipFeedingMotor.Controls.Add(this.button4);
            this.groupBoxChipFeedingMotor.Controls.Add(this.button5);
            this.groupBoxChipFeedingMotor.Controls.Add(this.button6);
            this.groupBoxChipFeedingMotor.ForeColor = System.Drawing.Color.White;
            this.groupBoxChipFeedingMotor.Location = new System.Drawing.Point(244, 149);
            this.groupBoxChipFeedingMotor.Name = "groupBoxChipFeedingMotor";
            this.groupBoxChipFeedingMotor.Size = new System.Drawing.Size(223, 125);
            this.groupBoxChipFeedingMotor.TabIndex = 11;
            this.groupBoxChipFeedingMotor.TabStop = false;
            this.groupBoxChipFeedingMotor.Text = " Chip Feeding Motor ";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(12, 92);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(118, 21);
            this.textBox3.TabIndex = 9;
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.button4.Image = global::CWA150SA_Onsemi.Properties.Resources.CCW;
            this.button4.Location = new System.Drawing.Point(12, 26);
            this.button4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(56, 56);
            this.button4.TabIndex = 8;
            this.button4.UseVisualStyleBackColor = false;
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.button5.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button5.ForeColor = System.Drawing.Color.White;
            this.button5.Location = new System.Drawing.Point(146, 26);
            this.button5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(67, 56);
            this.button5.TabIndex = 8;
            this.button5.Text = "Stop";
            this.button5.UseVisualStyleBackColor = false;
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.button6.Image = global::CWA150SA_Onsemi.Properties.Resources.CW;
            this.button6.Location = new System.Drawing.Point(74, 26);
            this.button6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(56, 56);
            this.button6.TabIndex = 7;
            this.button6.UseVisualStyleBackColor = false;
            // 
            // ReelFeeder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.Controls.Add(this.groupBoxChipFeedingMotor);
            this.Controls.Add(this.groupBoxCoverTapeMotor);
            this.Controls.Add(this.groupBoxCompletedReelMotor);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ReelFeeder";
            this.Size = new System.Drawing.Size(471, 277);
            this.groupBoxCompletedReelMotor.ResumeLayout(false);
            this.groupBoxCompletedReelMotor.PerformLayout();
            this.groupBoxCoverTapeMotor.ResumeLayout(false);
            this.groupBoxCoverTapeMotor.PerformLayout();
            this.groupBoxChipFeedingMotor.ResumeLayout(false);
            this.groupBoxChipFeedingMotor.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonCW;
        private System.Windows.Forms.Button buttonCCW;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.GroupBox groupBoxCompletedReelMotor;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBoxCoverTapeMotor;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.GroupBox groupBoxChipFeedingMotor;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
    }
}
