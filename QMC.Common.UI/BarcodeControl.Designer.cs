namespace QMC.Common.UI
{
    partial class BarcodeControl
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
            this.baseGroupBoxBarcode = new QMC.Common.UI.BaseGroupBox();
            this.baseButtonRead = new QMC.Common.UI.BaseButton();
            this.baseTextBoxResult = new QMC.Common.UI.BaseTextBox();
            this.baseGroupBoxBarcode.SuspendLayout();
            this.SuspendLayout();
            // 
            // baseGroupBoxBarcode
            // 
            this.baseGroupBoxBarcode.Controls.Add(this.baseButtonRead);
            this.baseGroupBoxBarcode.Controls.Add(this.baseTextBoxResult);
            this.baseGroupBoxBarcode.ForeColor = System.Drawing.Color.White;
            this.baseGroupBoxBarcode.Location = new System.Drawing.Point(0, 0);
            this.baseGroupBoxBarcode.Name = "baseGroupBoxBarcode";
            this.baseGroupBoxBarcode.Size = new System.Drawing.Size(355, 50);
            this.baseGroupBoxBarcode.TabIndex = 0;
            this.baseGroupBoxBarcode.TabStop = false;
            this.baseGroupBoxBarcode.Text = "Barcode";
            // 
            // baseButtonRead
            // 
            this.baseButtonRead.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonRead.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonRead.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonRead.Location = new System.Drawing.Point(260, 20);
            this.baseButtonRead.Name = "baseButtonRead";
            this.baseButtonRead.Size = new System.Drawing.Size(86, 21);
            this.baseButtonRead.TabIndex = 2;
            this.baseButtonRead.Text = "Read";
            this.baseButtonRead.UseVisualStyleBackColor = false;
            this.baseButtonRead.Click += new System.EventHandler(this.baseButtonRead_Click);
            // 
            // baseTextBoxResult
            // 
            this.baseTextBoxResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseTextBoxResult.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxResult.Location = new System.Drawing.Point(6, 20);
            this.baseTextBoxResult.Name = "baseTextBoxResult";
            this.baseTextBoxResult.ReadOnly = true;
            this.baseTextBoxResult.Size = new System.Drawing.Size(248, 21);
            this.baseTextBoxResult.TabIndex = 1;
            // 
            // BarcodeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseGroupBoxBarcode);
            this.Name = "BarcodeControl";
            this.Size = new System.Drawing.Size(355, 50);
            this.baseGroupBoxBarcode.ResumeLayout(false);
            this.baseGroupBoxBarcode.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private BaseGroupBox baseGroupBoxBarcode;
        private BaseButton baseButtonRead;
        private BaseTextBox baseTextBoxResult;
    }
}
