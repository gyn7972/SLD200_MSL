namespace QMC.Common.UI
{
    partial class SPCOptionControl
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
            this.baseGroupBoxSearchOption = new QMC.Common.UI.BaseGroupBox();
            this.baseGroupBox2 = new QMC.Common.UI.BaseGroupBox();
            this.baseButtonSaveData = new QMC.Common.UI.BaseButton();
            this.baseGroupBox1 = new QMC.Common.UI.BaseGroupBox();
            this.baseTextBox1 = new QMC.Common.UI.BaseTextBox();
            this.baseLabel1 = new QMC.Common.UI.BaseLabel();
            this.baseLabel2 = new QMC.Common.UI.BaseLabel();
            this.dateTimePickerStartTime = new System.Windows.Forms.DateTimePicker();
            this.baseLabelBarcode = new QMC.Common.UI.BaseLabel();
            this.dateTimePickerEndTime = new System.Windows.Forms.DateTimePicker();
            this.baseButtonSearch = new QMC.Common.UI.BaseButton();
            this.baseGroupBoxSearchOption.SuspendLayout();
            this.baseGroupBox2.SuspendLayout();
            this.baseGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // baseGroupBoxSearchOption
            // 
            this.baseGroupBoxSearchOption.Controls.Add(this.baseGroupBox2);
            this.baseGroupBoxSearchOption.Controls.Add(this.baseGroupBox1);
            this.baseGroupBoxSearchOption.ForeColor = System.Drawing.Color.White;
            this.baseGroupBoxSearchOption.Location = new System.Drawing.Point(3, 3);
            this.baseGroupBoxSearchOption.Name = "baseGroupBoxSearchOption";
            this.baseGroupBoxSearchOption.Size = new System.Drawing.Size(294, 784);
            this.baseGroupBoxSearchOption.TabIndex = 0;
            this.baseGroupBoxSearchOption.TabStop = false;
            this.baseGroupBoxSearchOption.Text = "Option";
            // 
            // baseGroupBox2
            // 
            this.baseGroupBox2.Controls.Add(this.baseButtonSaveData);
            this.baseGroupBox2.ForeColor = System.Drawing.Color.White;
            this.baseGroupBox2.Location = new System.Drawing.Point(6, 203);
            this.baseGroupBox2.Name = "baseGroupBox2";
            this.baseGroupBox2.Size = new System.Drawing.Size(282, 70);
            this.baseGroupBox2.TabIndex = 9;
            this.baseGroupBox2.TabStop = false;
            this.baseGroupBox2.Text = "DATA";
            // 
            // baseButtonSaveData
            // 
            this.baseButtonSaveData.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonSaveData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonSaveData.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonSaveData.Location = new System.Drawing.Point(19, 20);
            this.baseButtonSaveData.Name = "baseButtonSaveData";
            this.baseButtonSaveData.Size = new System.Drawing.Size(100, 38);
            this.baseButtonSaveData.TabIndex = 7;
            this.baseButtonSaveData.Text = "Data Save";
            this.baseButtonSaveData.UseVisualStyleBackColor = false;
            this.baseButtonSaveData.Click += new System.EventHandler(this.baseButtonSaveData_Click);
            // 
            // baseGroupBox1
            // 
            this.baseGroupBox1.Controls.Add(this.baseTextBox1);
            this.baseGroupBox1.Controls.Add(this.baseLabel1);
            this.baseGroupBox1.Controls.Add(this.baseLabel2);
            this.baseGroupBox1.Controls.Add(this.dateTimePickerStartTime);
            this.baseGroupBox1.Controls.Add(this.baseLabelBarcode);
            this.baseGroupBox1.Controls.Add(this.dateTimePickerEndTime);
            this.baseGroupBox1.Controls.Add(this.baseButtonSearch);
            this.baseGroupBox1.ForeColor = System.Drawing.Color.White;
            this.baseGroupBox1.Location = new System.Drawing.Point(6, 20);
            this.baseGroupBox1.Name = "baseGroupBox1";
            this.baseGroupBox1.Size = new System.Drawing.Size(282, 177);
            this.baseGroupBox1.TabIndex = 8;
            this.baseGroupBox1.TabStop = false;
            this.baseGroupBox1.Text = "Search";
            // 
            // baseTextBox1
            // 
            this.baseTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseTextBox1.ForeColor = System.Drawing.Color.White;
            this.baseTextBox1.Location = new System.Drawing.Point(122, 95);
            this.baseTextBox1.Name = "baseTextBox1";
            this.baseTextBox1.Size = new System.Drawing.Size(154, 21);
            this.baseTextBox1.TabIndex = 6;
            // 
            // baseLabel1
            // 
            this.baseLabel1.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabel1.ForeColor = System.Drawing.Color.White;
            this.baseLabel1.Location = new System.Drawing.Point(9, 20);
            this.baseLabel1.Name = "baseLabel1";
            this.baseLabel1.Size = new System.Drawing.Size(107, 23);
            this.baseLabel1.TabIndex = 0;
            this.baseLabel1.Text = "Start Time : ";
            // 
            // baseLabel2
            // 
            this.baseLabel2.AutoSize = true;
            this.baseLabel2.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabel2.ForeColor = System.Drawing.Color.White;
            this.baseLabel2.Location = new System.Drawing.Point(16, 55);
            this.baseLabel2.Name = "baseLabel2";
            this.baseLabel2.Size = new System.Drawing.Size(100, 16);
            this.baseLabel2.TabIndex = 1;
            this.baseLabel2.Text = "End Time : ";
            // 
            // dateTimePickerStartTime
            // 
            this.dateTimePickerStartTime.Location = new System.Drawing.Point(122, 18);
            this.dateTimePickerStartTime.Name = "dateTimePickerStartTime";
            this.dateTimePickerStartTime.Size = new System.Drawing.Size(154, 21);
            this.dateTimePickerStartTime.TabIndex = 2;
            // 
            // baseLabelBarcode
            // 
            this.baseLabelBarcode.AutoSize = true;
            this.baseLabelBarcode.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelBarcode.ForeColor = System.Drawing.Color.White;
            this.baseLabelBarcode.Location = new System.Drawing.Point(38, 97);
            this.baseLabelBarcode.Name = "baseLabelBarcode";
            this.baseLabelBarcode.Size = new System.Drawing.Size(78, 16);
            this.baseLabelBarcode.TabIndex = 5;
            this.baseLabelBarcode.Text = "Unit ID : ";
            // 
            // dateTimePickerEndTime
            // 
            this.dateTimePickerEndTime.Location = new System.Drawing.Point(122, 53);
            this.dateTimePickerEndTime.Name = "dateTimePickerEndTime";
            this.dateTimePickerEndTime.Size = new System.Drawing.Size(154, 21);
            this.dateTimePickerEndTime.TabIndex = 3;
            // 
            // baseButtonSearch
            // 
            this.baseButtonSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonSearch.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonSearch.Location = new System.Drawing.Point(176, 128);
            this.baseButtonSearch.Name = "baseButtonSearch";
            this.baseButtonSearch.Size = new System.Drawing.Size(100, 35);
            this.baseButtonSearch.TabIndex = 4;
            this.baseButtonSearch.Text = "Search";
            this.baseButtonSearch.UseVisualStyleBackColor = false;
            this.baseButtonSearch.Click += new System.EventHandler(this.baseButtonSearch_Click);
            // 
            // SPCOptionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseGroupBoxSearchOption);
            this.Name = "SPCOptionControl";
            this.Size = new System.Drawing.Size(300, 790);
            this.baseGroupBoxSearchOption.ResumeLayout(false);
            this.baseGroupBox2.ResumeLayout(false);
            this.baseGroupBox1.ResumeLayout(false);
            this.baseGroupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private BaseGroupBox baseGroupBoxSearchOption;
        private BaseTextBox baseTextBox1;
        private BaseLabel baseLabelBarcode;
        private BaseButton baseButtonSearch;
        private System.Windows.Forms.DateTimePicker dateTimePickerEndTime;
        private System.Windows.Forms.DateTimePicker dateTimePickerStartTime;
        private BaseLabel baseLabel2;
        private BaseLabel baseLabel1;
        private BaseGroupBox baseGroupBox2;
        private BaseButton baseButtonSaveData;
        private BaseGroupBox baseGroupBox1;
    }
}
