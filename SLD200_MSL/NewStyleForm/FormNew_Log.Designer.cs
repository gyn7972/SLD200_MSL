namespace SLD200_MSL
{
    partial class FormNew_Log
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TabControl tabControl_Log;
        private System.Windows.Forms.TabPage tabPage_LOT;
        private System.Windows.Forms.DataGridView dataGridView_Log;
        private System.Windows.Forms.Label label_LotDesc;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl_Log = new System.Windows.Forms.TabControl();
            this.tabPage_LOT = new System.Windows.Forms.TabPage();
            this.label_LotDesc = new System.Windows.Forms.Label();
            this.dataGridView_Log = new System.Windows.Forms.DataGridView();
            this.tabPage_AutoCross = new System.Windows.Forms.TabPage();
            this.baseGroupBox_Log_Search = new SLD200_MSL.BaseGroupBox();
            this.baseLabel_Log_Search_StartTime = new SLD200_MSL.BaseLabel();
            this.baseLabel_Log_Search_EndTime = new SLD200_MSL.BaseLabel();
            this.dateTime_Log_Search_StartTime = new System.Windows.Forms.DateTimePicker();
            this.dateTime_Log_Search_EndTime = new System.Windows.Forms.DateTimePicker();
            this.baseButton_Log_Search = new SLD200_MSL.BaseButton();
            this.tabControl_Log.SuspendLayout();
            this.tabPage_LOT.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Log)).BeginInit();
            this.baseGroupBox_Log_Search.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl_Log
            // 
            this.tabControl_Log.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tabControl_Log.Controls.Add(this.tabPage_LOT);
            this.tabControl_Log.Controls.Add(this.tabPage_AutoCross);
            this.tabControl_Log.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.tabControl_Log.ItemSize = new System.Drawing.Size(170, 40);
            this.tabControl_Log.Location = new System.Drawing.Point(10, 10);
            this.tabControl_Log.Name = "tabControl_Log";
            this.tabControl_Log.SelectedIndex = 0;
            this.tabControl_Log.Size = new System.Drawing.Size(1901, 857);
            this.tabControl_Log.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl_Log.TabIndex = 0;
            // 
            // tabPage_LOT
            // 
            this.tabPage_LOT.Controls.Add(this.baseGroupBox_Log_Search);
            this.tabPage_LOT.Controls.Add(this.label_LotDesc);
            this.tabPage_LOT.Controls.Add(this.dataGridView_Log);
            this.tabPage_LOT.Location = new System.Drawing.Point(4, 44);
            this.tabPage_LOT.Name = "tabPage_LOT";
            this.tabPage_LOT.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_LOT.Size = new System.Drawing.Size(1893, 809);
            this.tabPage_LOT.TabIndex = 0;
            this.tabPage_LOT.Text = "LOT";
            this.tabPage_LOT.UseVisualStyleBackColor = true;
            this.tabPage_LOT.Click += new System.EventHandler(this.tabPage_LOT_Click);
            // 
            // label_LotDesc
            // 
            this.label_LotDesc.AutoSize = true;
            this.label_LotDesc.Location = new System.Drawing.Point(584, 94);
            this.label_LotDesc.Name = "label_LotDesc";
            this.label_LotDesc.Size = new System.Drawing.Size(405, 19);
            this.label_LotDesc.TabIndex = 0;
            this.label_LotDesc.Text = "Lot : 시작 시간, 완료 시간, 레시피명, 도면명, 가공 완료 개수";
            // 
            // dataGridView_Log
            // 
            this.dataGridView_Log.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Log.Location = new System.Drawing.Point(7, 122);
            this.dataGridView_Log.Name = "dataGridView_Log";
            this.dataGridView_Log.Size = new System.Drawing.Size(1880, 694);
            this.dataGridView_Log.TabIndex = 1;
            // 
            // tabPage_AutoCross
            // 
            this.tabPage_AutoCross.Location = new System.Drawing.Point(4, 44);
            this.tabPage_AutoCross.Name = "tabPage_AutoCross";
            this.tabPage_AutoCross.Size = new System.Drawing.Size(1893, 809);
            this.tabPage_AutoCross.TabIndex = 1;
            this.tabPage_AutoCross.Text = "AutoCross";
            this.tabPage_AutoCross.UseVisualStyleBackColor = true;
            // 
            // baseGroupBox_Log_Search
            // 
            this.baseGroupBox_Log_Search.Controls.Add(this.baseLabel_Log_Search_StartTime);
            this.baseGroupBox_Log_Search.Controls.Add(this.baseLabel_Log_Search_EndTime);
            this.baseGroupBox_Log_Search.Controls.Add(this.dateTime_Log_Search_StartTime);
            this.baseGroupBox_Log_Search.Controls.Add(this.dateTime_Log_Search_EndTime);
            this.baseGroupBox_Log_Search.Controls.Add(this.baseButton_Log_Search);
            this.baseGroupBox_Log_Search.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.baseGroupBox_Log_Search.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox_Log_Search.Location = new System.Drawing.Point(7, 8);
            this.baseGroupBox_Log_Search.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.baseGroupBox_Log_Search.Name = "baseGroupBox_Log_Search";
            this.baseGroupBox_Log_Search.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.baseGroupBox_Log_Search.Size = new System.Drawing.Size(570, 105);
            this.baseGroupBox_Log_Search.TabIndex = 10;
            this.baseGroupBox_Log_Search.TabStop = false;
            this.baseGroupBox_Log_Search.Text = " Search ";
            // 
            // baseLabel_Log_Search_StartTime
            // 
            this.baseLabel_Log_Search_StartTime.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabel_Log_Search_StartTime.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Log_Search_StartTime.Location = new System.Drawing.Point(11, 25);
            this.baseLabel_Log_Search_StartTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Log_Search_StartTime.Name = "baseLabel_Log_Search_StartTime";
            this.baseLabel_Log_Search_StartTime.Size = new System.Drawing.Size(114, 27);
            this.baseLabel_Log_Search_StartTime.TabIndex = 0;
            this.baseLabel_Log_Search_StartTime.Text = "Start Time :";
            this.baseLabel_Log_Search_StartTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // baseLabel_Log_Search_EndTime
            // 
            this.baseLabel_Log_Search_EndTime.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabel_Log_Search_EndTime.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Log_Search_EndTime.Location = new System.Drawing.Point(11, 63);
            this.baseLabel_Log_Search_EndTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Log_Search_EndTime.Name = "baseLabel_Log_Search_EndTime";
            this.baseLabel_Log_Search_EndTime.Size = new System.Drawing.Size(114, 27);
            this.baseLabel_Log_Search_EndTime.TabIndex = 1;
            this.baseLabel_Log_Search_EndTime.Text = "End Time :";
            this.baseLabel_Log_Search_EndTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dateTime_Log_Search_StartTime
            // 
            this.dateTime_Log_Search_StartTime.CalendarFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTime_Log_Search_StartTime.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.dateTime_Log_Search_StartTime.Location = new System.Drawing.Point(126, 25);
            this.dateTime_Log_Search_StartTime.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dateTime_Log_Search_StartTime.Name = "dateTime_Log_Search_StartTime";
            this.dateTime_Log_Search_StartTime.Size = new System.Drawing.Size(302, 27);
            this.dateTime_Log_Search_StartTime.TabIndex = 2;
            // 
            // dateTime_Log_Search_EndTime
            // 
            this.dateTime_Log_Search_EndTime.CalendarFont = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTime_Log_Search_EndTime.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.dateTime_Log_Search_EndTime.Location = new System.Drawing.Point(126, 63);
            this.dateTime_Log_Search_EndTime.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dateTime_Log_Search_EndTime.Name = "dateTime_Log_Search_EndTime";
            this.dateTime_Log_Search_EndTime.Size = new System.Drawing.Size(302, 27);
            this.dateTime_Log_Search_EndTime.TabIndex = 3;
            // 
            // baseButton_Log_Search
            // 
            this.baseButton_Log_Search.BackColor = System.Drawing.Color.White;
            this.baseButton_Log_Search.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButton_Log_Search.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.baseButton_Log_Search.ForeColor = System.Drawing.Color.Black;
            this.baseButton_Log_Search.Location = new System.Drawing.Point(436, 25);
            this.baseButton_Log_Search.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.baseButton_Log_Search.Name = "baseButton_Log_Search";
            this.baseButton_Log_Search.Size = new System.Drawing.Size(128, 65);
            this.baseButton_Log_Search.TabIndex = 4;
            this.baseButton_Log_Search.Text = "Search";
            this.baseButton_Log_Search.UseVisualStyleBackColor = false;
            this.baseButton_Log_Search.Click += new System.EventHandler(this.baseButton_Log_Search_Click);
            // 
            // FormNew_Log
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1920, 877);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl_Log);
            this.Font = new System.Drawing.Font("Tahoma", 15F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormNew_Log";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "Log GUI";
            this.tabControl_Log.ResumeLayout(false);
            this.tabPage_LOT.ResumeLayout(false);
            this.tabPage_LOT.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Log)).EndInit();
            this.baseGroupBox_Log_Search.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage_AutoCross;
        private BaseGroupBox baseGroupBox_Log_Search;
        private BaseLabel baseLabel_Log_Search_StartTime;
        private BaseLabel baseLabel_Log_Search_EndTime;
        private System.Windows.Forms.DateTimePicker dateTime_Log_Search_StartTime;
        private System.Windows.Forms.DateTimePicker dateTime_Log_Search_EndTime;
        private BaseButton baseButton_Log_Search;
    }
}