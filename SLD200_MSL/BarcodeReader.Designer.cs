namespace SLD200_MSL
{
    partial class BarcodeReader
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
            this.buttonStop = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBoxBarcodeReader = new System.Windows.Forms.GroupBox();
            this.txtBarcodeData = new System.Windows.Forms.TextBox();
            this.btnBarcodeTrigger = new System.Windows.Forms.Button();
            this.groupBoxBarcodeReader.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonStop
            // 
            this.buttonStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.buttonStop.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonStop.ForeColor = System.Drawing.Color.White;
            this.buttonStop.Location = new System.Drawing.Point(73, 34);
            this.buttonStop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(77, 38);
            this.buttonStop.TabIndex = 8;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = false;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(50, 95);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(127, 21);
            this.textBox1.TabIndex = 9;
            // 
            // groupBoxBarcodeReader
            // 
            this.groupBoxBarcodeReader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.groupBoxBarcodeReader.Controls.Add(this.txtBarcodeData);
            this.groupBoxBarcodeReader.Controls.Add(this.btnBarcodeTrigger);
            this.groupBoxBarcodeReader.ForeColor = System.Drawing.Color.White;
            this.groupBoxBarcodeReader.Location = new System.Drawing.Point(3, 3);
            this.groupBoxBarcodeReader.Name = "groupBoxBarcodeReader";
            this.groupBoxBarcodeReader.Size = new System.Drawing.Size(371, 125);
            this.groupBoxBarcodeReader.TabIndex = 10;
            this.groupBoxBarcodeReader.TabStop = false;
            this.groupBoxBarcodeReader.Text = " Barcode Reader ";
            // 
            // txtBarcodeData
            // 
            this.txtBarcodeData.Location = new System.Drawing.Point(108, 28);
            this.txtBarcodeData.Name = "txtBarcodeData";
            this.txtBarcodeData.Size = new System.Drawing.Size(242, 21);
            this.txtBarcodeData.TabIndex = 9;
            // 
            // btnBarcodeTrigger
            // 
            this.btnBarcodeTrigger.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnBarcodeTrigger.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnBarcodeTrigger.ForeColor = System.Drawing.Color.White;
            this.btnBarcodeTrigger.Location = new System.Drawing.Point(15, 28);
            this.btnBarcodeTrigger.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnBarcodeTrigger.Name = "btnBarcodeTrigger";
            this.btnBarcodeTrigger.Size = new System.Drawing.Size(87, 38);
            this.btnBarcodeTrigger.TabIndex = 8;
            this.btnBarcodeTrigger.Text = "Trigger";
            this.btnBarcodeTrigger.UseVisualStyleBackColor = false;
            // 
            // BarcodeReader
            // 
            this.Controls.Add(this.groupBoxBarcodeReader);
            this.Name = "BarcodeReader";
            this.Size = new System.Drawing.Size(377, 131);
            this.groupBoxBarcodeReader.ResumeLayout(false);
            this.groupBoxBarcodeReader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBoxBarcodeReader;
        private System.Windows.Forms.TextBox txtBarcodeData;
        private System.Windows.Forms.Button btnBarcodeTrigger;
    }
}
