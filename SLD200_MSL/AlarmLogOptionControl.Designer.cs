namespace SLD200_MSL
{
    partial class AlarmLogOptionControl
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
            this.baseGroupBox1 = new SLD200_MSL.BaseGroupBox();
            this.baseGroupBox3 = new SLD200_MSL.BaseGroupBox();
            this.baseButtonSaveData = new SLD200_MSL.BaseButton();
            this.baseGroupBox2 = new SLD200_MSL.BaseGroupBox();
            this.baseTextBoxAlarmCode = new SLD200_MSL.BaseTextBox();
            this.baseLabelBarcode = new SLD200_MSL.BaseLabel();
            this.baseLabel1 = new SLD200_MSL.BaseLabel();
            this.baseLabel2 = new SLD200_MSL.BaseLabel();
            this.dateTimePickerStartTime = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerEndTime = new System.Windows.Forms.DateTimePicker();
            this.baseButtonSearch = new SLD200_MSL.BaseButton();
            this.baseGroupBox1.SuspendLayout();
            this.baseGroupBox3.SuspendLayout();
            this.baseGroupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // baseGroupBox1
            // 
            this.baseGroupBox1.Controls.Add(this.baseGroupBox3);
            this.baseGroupBox1.Controls.Add(this.baseGroupBox2);
            this.baseGroupBox1.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox1.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox1.Location = new System.Drawing.Point(10, 12);
            this.baseGroupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.baseGroupBox1.Name = "baseGroupBox1";
            this.baseGroupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.baseGroupBox1.Size = new System.Drawing.Size(505, 1075);
            this.baseGroupBox1.TabIndex = 0;
            this.baseGroupBox1.TabStop = false;
            this.baseGroupBox1.Text = " Select Option ";
            // 
            // baseGroupBox3
            // 
            this.baseGroupBox3.Controls.Add(this.baseButtonSaveData);
            this.baseGroupBox3.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox3.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox3.Location = new System.Drawing.Point(15, 402);
            this.baseGroupBox3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.baseGroupBox3.Name = "baseGroupBox3";
            this.baseGroupBox3.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.baseGroupBox3.Size = new System.Drawing.Size(474, 160);
            this.baseGroupBox3.TabIndex = 10;
            this.baseGroupBox3.TabStop = false;
            this.baseGroupBox3.Text = " Data ";
            this.baseGroupBox3.Visible = false;
            // 
            // baseButtonSaveData
            // 
            this.baseButtonSaveData.BackColor = System.Drawing.Color.White;
            this.baseButtonSaveData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonSaveData.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseButtonSaveData.ForeColor = System.Drawing.Color.Black;
            this.baseButtonSaveData.Location = new System.Drawing.Point(16, 49);
            this.baseButtonSaveData.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.baseButtonSaveData.Name = "baseButtonSaveData";
            this.baseButtonSaveData.Size = new System.Drawing.Size(442, 93);
            this.baseButtonSaveData.TabIndex = 7;
            this.baseButtonSaveData.Text = "Data  Save";
            this.baseButtonSaveData.UseVisualStyleBackColor = false;
            this.baseButtonSaveData.Visible = false;
            this.baseButtonSaveData.Click += new System.EventHandler(this.baseButtonSaveData_Click);
            // 
            // baseGroupBox2
            // 
            this.baseGroupBox2.Controls.Add(this.baseTextBoxAlarmCode);
            this.baseGroupBox2.Controls.Add(this.baseLabelBarcode);
            this.baseGroupBox2.Controls.Add(this.baseLabel1);
            this.baseGroupBox2.Controls.Add(this.baseLabel2);
            this.baseGroupBox2.Controls.Add(this.dateTimePickerStartTime);
            this.baseGroupBox2.Controls.Add(this.dateTimePickerEndTime);
            this.baseGroupBox2.Controls.Add(this.baseButtonSearch);
            this.baseGroupBox2.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox2.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox2.Location = new System.Drawing.Point(15, 57);
            this.baseGroupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.baseGroupBox2.Name = "baseGroupBox2";
            this.baseGroupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.baseGroupBox2.Size = new System.Drawing.Size(474, 305);
            this.baseGroupBox2.TabIndex = 9;
            this.baseGroupBox2.TabStop = false;
            this.baseGroupBox2.Text = " Search ";
            // 
            // baseTextBoxAlarmCode
            // 
            this.baseTextBoxAlarmCode.BackColor = System.Drawing.Color.White;
            this.baseTextBoxAlarmCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseTextBoxAlarmCode.Font = new System.Drawing.Font("Tahoma", 11F);
            this.baseTextBoxAlarmCode.ForeColor = System.Drawing.Color.Black;
            this.baseTextBoxAlarmCode.Location = new System.Drawing.Point(156, 134);
            this.baseTextBoxAlarmCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.baseTextBoxAlarmCode.Name = "baseTextBoxAlarmCode";
            this.baseTextBoxAlarmCode.Size = new System.Drawing.Size(302, 34);
            this.baseTextBoxAlarmCode.TabIndex = 8;
            this.baseTextBoxAlarmCode.Visible = false;
            // 
            // baseLabelBarcode
            // 
            this.baseLabelBarcode.Font = new System.Drawing.Font("Tahoma", 11F);
            this.baseLabelBarcode.ForeColor = System.Drawing.Color.Black;
            this.baseLabelBarcode.Location = new System.Drawing.Point(11, 127);
            this.baseLabelBarcode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabelBarcode.Name = "baseLabelBarcode";
            this.baseLabelBarcode.Size = new System.Drawing.Size(142, 42);
            this.baseLabelBarcode.TabIndex = 7;
            this.baseLabelBarcode.Text = "Alarm Code :";
            this.baseLabelBarcode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.baseLabelBarcode.Visible = false;
            // 
            // baseLabel1
            // 
            this.baseLabel1.Font = new System.Drawing.Font("Tahoma", 11F);
            this.baseLabel1.ForeColor = System.Drawing.Color.Black;
            this.baseLabel1.Location = new System.Drawing.Point(11, 37);
            this.baseLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel1.Name = "baseLabel1";
            this.baseLabel1.Size = new System.Drawing.Size(142, 42);
            this.baseLabel1.TabIndex = 0;
            this.baseLabel1.Text = "Start Time :";
            this.baseLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // baseLabel2
            // 
            this.baseLabel2.Font = new System.Drawing.Font("Tahoma", 11F);
            this.baseLabel2.ForeColor = System.Drawing.Color.Black;
            this.baseLabel2.Location = new System.Drawing.Point(11, 82);
            this.baseLabel2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel2.Name = "baseLabel2";
            this.baseLabel2.Size = new System.Drawing.Size(142, 42);
            this.baseLabel2.TabIndex = 1;
            this.baseLabel2.Text = "End Time :";
            this.baseLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dateTimePickerStartTime
            // 
            this.dateTimePickerStartTime.CalendarFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePickerStartTime.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.dateTimePickerStartTime.Location = new System.Drawing.Point(156, 42);
            this.dateTimePickerStartTime.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dateTimePickerStartTime.Name = "dateTimePickerStartTime";
            this.dateTimePickerStartTime.Size = new System.Drawing.Size(302, 36);
            this.dateTimePickerStartTime.TabIndex = 2;
            // 
            // dateTimePickerEndTime
            // 
            this.dateTimePickerEndTime.CalendarFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePickerEndTime.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.dateTimePickerEndTime.Location = new System.Drawing.Point(156, 87);
            this.dateTimePickerEndTime.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dateTimePickerEndTime.Name = "dateTimePickerEndTime";
            this.dateTimePickerEndTime.Size = new System.Drawing.Size(302, 36);
            this.dateTimePickerEndTime.TabIndex = 3;
            // 
            // baseButtonSearch
            // 
            this.baseButtonSearch.BackColor = System.Drawing.Color.White;
            this.baseButtonSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonSearch.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseButtonSearch.ForeColor = System.Drawing.Color.Black;
            this.baseButtonSearch.Location = new System.Drawing.Point(156, 203);
            this.baseButtonSearch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.baseButtonSearch.Name = "baseButtonSearch";
            this.baseButtonSearch.Size = new System.Drawing.Size(302, 82);
            this.baseButtonSearch.TabIndex = 4;
            this.baseButtonSearch.Text = "Search";
            this.baseButtonSearch.UseVisualStyleBackColor = false;
            this.baseButtonSearch.Click += new System.EventHandler(this.baseButtonSearch_Click);
            // 
            // AlarmLogOptionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseGroupBox1);
            this.Font = new System.Drawing.Font("Tahoma", 9F);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "AlarmLogOptionControl";
            this.Size = new System.Drawing.Size(526, 1094);
            this.Load += new System.EventHandler(this.AlarmLogOptionControl_Load);
            this.baseGroupBox1.ResumeLayout(false);
            this.baseGroupBox3.ResumeLayout(false);
            this.baseGroupBox2.ResumeLayout(false);
            this.baseGroupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private BaseGroupBox baseGroupBox1;
        private BaseGroupBox baseGroupBox2;
        private BaseLabel baseLabel1;
        private BaseLabel baseLabel2;
        private System.Windows.Forms.DateTimePicker dateTimePickerStartTime;
        private System.Windows.Forms.DateTimePicker dateTimePickerEndTime;
        private BaseButton baseButtonSearch;
        private BaseGroupBox baseGroupBox3;
        private BaseButton baseButtonSaveData;
        private BaseTextBox baseTextBoxAlarmCode;
        private BaseLabel baseLabelBarcode;
    }
}
