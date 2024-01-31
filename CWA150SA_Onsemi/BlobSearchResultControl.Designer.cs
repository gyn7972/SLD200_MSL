namespace CWA150SA_Onsemi
{
    partial class BlobSearchResultControl
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
            this.baseGroupBoxBlobResult = new CWA150SA_Onsemi300.BaseGroupBox();
            this.baseButtonSearch = new CWA150SA_Onsemi300.BaseButton();
            this.baseGroupBox2 = new CWA150SA_Onsemi300.BaseGroupBox();
            this.baseLabelResultArea = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelResultX = new CWA150SA_Onsemi300.BaseLabel();
            this.baseTextBoxResultArea = new CWA150SA_Onsemi300.BaseTextBox();
            this.baseLabelResultY = new CWA150SA_Onsemi300.BaseLabel();
            this.baseTextBoxResultY = new CWA150SA_Onsemi300.BaseTextBox();
            this.baseTextBoxResultX = new CWA150SA_Onsemi300.BaseTextBox();
            this.baseGroupBox1 = new CWA150SA_Onsemi300.BaseGroupBox();
            this.radioButtonMillimeter = new System.Windows.Forms.RadioButton();
            this.radioButtonPixel = new System.Windows.Forms.RadioButton();
            this.baseGroupBoxBlobResult.SuspendLayout();
            this.baseGroupBox2.SuspendLayout();
            this.baseGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // baseGroupBoxBlobResult
            // 
            this.baseGroupBoxBlobResult.Controls.Add(this.baseButtonSearch);
            this.baseGroupBoxBlobResult.Controls.Add(this.baseGroupBox2);
            this.baseGroupBoxBlobResult.Controls.Add(this.baseGroupBox1);
            this.baseGroupBoxBlobResult.ForeColor = System.Drawing.Color.White;
            this.baseGroupBoxBlobResult.Location = new System.Drawing.Point(0, 0);
            this.baseGroupBoxBlobResult.Name = "baseGroupBoxBlobResult";
            this.baseGroupBoxBlobResult.Size = new System.Drawing.Size(286, 117);
            this.baseGroupBoxBlobResult.TabIndex = 0;
            this.baseGroupBoxBlobResult.TabStop = false;
            this.baseGroupBoxBlobResult.Text = " Blob Result ";
            // 
            // baseButtonSearch
            // 
            this.baseButtonSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonSearch.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonSearch.Location = new System.Drawing.Point(188, 79);
            this.baseButtonSearch.Name = "baseButtonSearch";
            this.baseButtonSearch.Size = new System.Drawing.Size(90, 30);
            this.baseButtonSearch.TabIndex = 10;
            this.baseButtonSearch.Text = "Search";
            this.baseButtonSearch.UseVisualStyleBackColor = false;
            this.baseButtonSearch.Click += new System.EventHandler(this.baseButtonSearch_Click);
            // 
            // baseGroupBox2
            // 
            this.baseGroupBox2.Controls.Add(this.baseLabelResultArea);
            this.baseGroupBox2.Controls.Add(this.baseLabelResultX);
            this.baseGroupBox2.Controls.Add(this.baseTextBoxResultArea);
            this.baseGroupBox2.Controls.Add(this.baseLabelResultY);
            this.baseGroupBox2.Controls.Add(this.baseTextBoxResultY);
            this.baseGroupBox2.Controls.Add(this.baseTextBoxResultX);
            this.baseGroupBox2.ForeColor = System.Drawing.Color.White;
            this.baseGroupBox2.Location = new System.Drawing.Point(8, 17);
            this.baseGroupBox2.Name = "baseGroupBox2";
            this.baseGroupBox2.Size = new System.Drawing.Size(171, 93);
            this.baseGroupBox2.TabIndex = 9;
            this.baseGroupBox2.TabStop = false;
            this.baseGroupBox2.Text = " Unit ";
            // 
            // baseLabelResultArea
            // 
            this.baseLabelResultArea.AutoSize = true;
            this.baseLabelResultArea.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabelResultArea.ForeColor = System.Drawing.Color.White;
            this.baseLabelResultArea.Location = new System.Drawing.Point(5, 68);
            this.baseLabelResultArea.Name = "baseLabelResultArea";
            this.baseLabelResultArea.Size = new System.Drawing.Size(69, 16);
            this.baseLabelResultArea.TabIndex = 2;
            this.baseLabelResultArea.Text = "Area[px] :";
            // 
            // baseLabelResultX
            // 
            this.baseLabelResultX.AutoSize = true;
            this.baseLabelResultX.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabelResultX.ForeColor = System.Drawing.Color.White;
            this.baseLabelResultX.Location = new System.Drawing.Point(50, 18);
            this.baseLabelResultX.Name = "baseLabelResultX";
            this.baseLabelResultX.Size = new System.Drawing.Size(24, 16);
            this.baseLabelResultX.TabIndex = 0;
            this.baseLabelResultX.Text = "X :";
            // 
            // baseTextBoxResultArea
            // 
            this.baseTextBoxResultArea.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxResultArea.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxResultArea.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxResultArea.Location = new System.Drawing.Point(76, 70);
            this.baseTextBoxResultArea.Name = "baseTextBoxResultArea";
            this.baseTextBoxResultArea.Size = new System.Drawing.Size(88, 14);
            this.baseTextBoxResultArea.TabIndex = 5;
            // 
            // baseLabelResultY
            // 
            this.baseLabelResultY.AutoSize = true;
            this.baseLabelResultY.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabelResultY.ForeColor = System.Drawing.Color.White;
            this.baseLabelResultY.Location = new System.Drawing.Point(51, 43);
            this.baseLabelResultY.Name = "baseLabelResultY";
            this.baseLabelResultY.Size = new System.Drawing.Size(23, 16);
            this.baseLabelResultY.TabIndex = 1;
            this.baseLabelResultY.Text = "Y :";
            // 
            // baseTextBoxResultY
            // 
            this.baseTextBoxResultY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxResultY.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxResultY.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxResultY.Location = new System.Drawing.Point(76, 45);
            this.baseTextBoxResultY.Name = "baseTextBoxResultY";
            this.baseTextBoxResultY.Size = new System.Drawing.Size(88, 14);
            this.baseTextBoxResultY.TabIndex = 4;
            // 
            // baseTextBoxResultX
            // 
            this.baseTextBoxResultX.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxResultX.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxResultX.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxResultX.Location = new System.Drawing.Point(76, 20);
            this.baseTextBoxResultX.Name = "baseTextBoxResultX";
            this.baseTextBoxResultX.Size = new System.Drawing.Size(88, 14);
            this.baseTextBoxResultX.TabIndex = 3;
            // 
            // baseGroupBox1
            // 
            this.baseGroupBox1.Controls.Add(this.radioButtonMillimeter);
            this.baseGroupBox1.Controls.Add(this.radioButtonPixel);
            this.baseGroupBox1.ForeColor = System.Drawing.Color.White;
            this.baseGroupBox1.Location = new System.Drawing.Point(188, 17);
            this.baseGroupBox1.Name = "baseGroupBox1";
            this.baseGroupBox1.Size = new System.Drawing.Size(89, 58);
            this.baseGroupBox1.TabIndex = 6;
            this.baseGroupBox1.TabStop = false;
            this.baseGroupBox1.Text = " Unit ";
            // 
            // radioButtonMillimeter
            // 
            this.radioButtonMillimeter.AutoSize = true;
            this.radioButtonMillimeter.Location = new System.Drawing.Point(7, 37);
            this.radioButtonMillimeter.Name = "radioButtonMillimeter";
            this.radioButtonMillimeter.Size = new System.Drawing.Size(75, 16);
            this.radioButtonMillimeter.TabIndex = 8;
            this.radioButtonMillimeter.TabStop = true;
            this.radioButtonMillimeter.Text = "Millimeter";
            this.radioButtonMillimeter.UseVisualStyleBackColor = true;
            // 
            // radioButtonPixel
            // 
            this.radioButtonPixel.AutoSize = true;
            this.radioButtonPixel.Location = new System.Drawing.Point(7, 17);
            this.radioButtonPixel.Name = "radioButtonPixel";
            this.radioButtonPixel.Size = new System.Drawing.Size(51, 16);
            this.radioButtonPixel.TabIndex = 7;
            this.radioButtonPixel.TabStop = true;
            this.radioButtonPixel.Text = "Pixel";
            this.radioButtonPixel.UseVisualStyleBackColor = true;
            // 
            // BlobSearchResultControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseGroupBoxBlobResult);
            this.Name = "BlobSearchResultControl";
            this.Size = new System.Drawing.Size(288, 118);
            this.baseGroupBoxBlobResult.ResumeLayout(false);
            this.baseGroupBox2.ResumeLayout(false);
            this.baseGroupBox2.PerformLayout();
            this.baseGroupBox1.ResumeLayout(false);
            this.baseGroupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private CWA150SA_Onsemi300.BaseGroupBox baseGroupBoxBlobResult;
        private CWA150SA_Onsemi300.BaseGroupBox baseGroupBox1;
        private System.Windows.Forms.RadioButton radioButtonMillimeter;
        private System.Windows.Forms.RadioButton radioButtonPixel;
        private CWA150SA_Onsemi300.BaseTextBox baseTextBoxResultArea;
        private CWA150SA_Onsemi300.BaseTextBox baseTextBoxResultY;
        private CWA150SA_Onsemi300.BaseTextBox baseTextBoxResultX;
        private CWA150SA_Onsemi300.BaseLabel baseLabelResultArea;
        private CWA150SA_Onsemi300.BaseLabel baseLabelResultY;
        private CWA150SA_Onsemi300.BaseLabel baseLabelResultX;
        private CWA150SA_Onsemi300.BaseGroupBox baseGroupBox2;
        private CWA150SA_Onsemi300.BaseButton baseButtonSearch;
    }
}
