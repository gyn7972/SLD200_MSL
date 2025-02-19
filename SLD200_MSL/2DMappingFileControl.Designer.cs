namespace QMC.Vision
{
    partial class _2DMappingFileControl
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
            this.baseGroupBox1 = new QMC.Common.UI.BaseGroupBox();
            this.baseToggleBtn_Use = new QMC.Common.UI.BaseToggleButton();
            this.baseBtn_Load = new QMC.Common.UI.BaseButton();
            this.baseLabel1 = new QMC.Common.UI.BaseLabel();
            this.baseTextFilePath = new QMC.Common.UI.BaseTextBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.baseGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // baseGroupBox1
            // 
            this.baseGroupBox1.Controls.Add(this.baseToggleBtn_Use);
            this.baseGroupBox1.Controls.Add(this.baseBtn_Load);
            this.baseGroupBox1.Controls.Add(this.baseLabel1);
            this.baseGroupBox1.Controls.Add(this.baseTextFilePath);
            this.baseGroupBox1.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.baseGroupBox1.Name = "baseGroupBox1";
            this.baseGroupBox1.Size = new System.Drawing.Size(440, 102);
            this.baseGroupBox1.TabIndex = 0;
            this.baseGroupBox1.TabStop = false;
            this.baseGroupBox1.Text = " 2D Mapping ";
            // 
            // baseToggleBtn_Use
            // 
            this.baseToggleBtn_Use.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseToggleBtn_Use.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleBtn_Use.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseToggleBtn_Use.Font = new System.Drawing.Font("Tahoma", 12.0F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseToggleBtn_Use.Location = new System.Drawing.Point(9, 65);
            this.baseToggleBtn_Use.Name = "baseToggleBtn_Use";
            this.baseToggleBtn_Use.Size = new System.Drawing.Size(422, 30);
            this.baseToggleBtn_Use.TabIndex = 3;
            this.baseToggleBtn_Use.Text = " Use / Not Used ";
            this.baseToggleBtn_Use.UseVisualStyleBackColor = false;
            this.baseToggleBtn_Use.Click += new System.EventHandler(this.baseToggleBtn_Use_Click);
            // 
            // baseBtn_Load
            // 
            this.baseBtn_Load.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseBtn_Load.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseBtn_Load.Font = new System.Drawing.Font("Tahoma", 11.0F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseBtn_Load.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseBtn_Load.Location = new System.Drawing.Point(338, 20);
            this.baseBtn_Load.Name = "baseBtn_Load";
            this.baseBtn_Load.Size = new System.Drawing.Size(90, 30);
            this.baseBtn_Load.TabIndex = 2;
            this.baseBtn_Load.Text = " Load ";
            this.baseBtn_Load.UseVisualStyleBackColor = false;
            this.baseBtn_Load.Click += new System.EventHandler(this.baseBtn_Load_Click);
            // 
            // baseLabel1
            // 
            this.baseLabel1.AutoSize = true;
            this.baseLabel1.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.baseLabel1.ForeColor = System.Drawing.Color.Black;
            this.baseLabel1.Location = new System.Drawing.Point(6, 29);
            this.baseLabel1.Name = "baseLabel1";
            this.baseLabel1.Size = new System.Drawing.Size(85, 16);
            this.baseLabel1.TabIndex = 1;
            this.baseLabel1.Text = " 2D Map : ";
            // 
            // baseTextFilePath
            // 
            this.baseTextFilePath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextFilePath.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextFilePath.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseTextFilePath.Font = new System.Drawing.Font("Tahoma", 9.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseTextFilePath.Location = new System.Drawing.Point(97, 21);
            this.baseTextFilePath.Name = "baseTextFilePath";
            this.baseTextFilePath.Multiline = true;
            this.baseTextFilePath.Size = new System.Drawing.Size(230, 28);
            this.baseTextFilePath.TabIndex = 0;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // _2DMappingFileControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseGroupBox1);
            this.Name = "_2DMappingFileControl";
            this.Size = new System.Drawing.Size(442, 112);
            this.baseGroupBox1.ResumeLayout(false);
            this.baseGroupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Common.UI.BaseGroupBox baseGroupBox1;
        private Common.UI.BaseToggleButton baseToggleBtn_Use;
        private Common.UI.BaseButton baseBtn_Load;
        private Common.UI.BaseLabel baseLabel1;
        private Common.UI.BaseTextBox baseTextFilePath;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
    }
}
