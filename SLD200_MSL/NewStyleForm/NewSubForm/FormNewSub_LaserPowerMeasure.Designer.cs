namespace SLD200.NewStyleForm.NewSubForm
{
    partial class FormNewSub_LaserPowerMeasure
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dataGridViewSettings;
        private System.Windows.Forms.Button buttonApplyAndFire;
        private System.Windows.Forms.Label labelDuration;
        private System.Windows.Forms.NumericUpDown numericUpDownDuration;
        private System.Windows.Forms.ComboBox comboBoxTargetType;
        private System.Windows.Forms.Label labelTargetType;
        private System.Windows.Forms.ListBox listBox_PowerLog;

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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridViewSettings = new System.Windows.Forms.DataGridView();
            this.buttonApplyAndFire = new System.Windows.Forms.Button();
            this.labelDuration = new System.Windows.Forms.Label();
            this.numericUpDownDuration = new System.Windows.Forms.NumericUpDown();
            this.buttonStop = new System.Windows.Forms.Button();
            this.button_MeasureReady = new System.Windows.Forms.Button();
            this.labelTargetType = new System.Windows.Forms.Label();
            this.comboBoxTargetType = new System.Windows.Forms.ComboBox();
            this.button_SeqStart = new System.Windows.Forms.Button();
            this.button_SeqStop = new System.Windows.Forms.Button();
            this.listBox_PowerLog = new System.Windows.Forms.ListBox();
            this.button_Test = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSettings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewSettings
            // 
            this.dataGridViewSettings.AllowUserToAddRows = false;
            this.dataGridViewSettings.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.Format = "N0";
            dataGridViewCellStyle1.NullValue = "0";
            this.dataGridViewSettings.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewSettings.BackgroundColor = System.Drawing.SystemColors.Menu;
            this.dataGridViewSettings.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridViewSettings.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            this.dataGridViewSettings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.Format = "N0";
            dataGridViewCellStyle2.NullValue = "0";
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewSettings.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewSettings.Location = new System.Drawing.Point(14, 11);
            this.dataGridViewSettings.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dataGridViewSettings.Name = "dataGridViewSettings";
            this.dataGridViewSettings.RowHeadersWidth = 62;
            this.dataGridViewSettings.Size = new System.Drawing.Size(322, 196);
            this.dataGridViewSettings.TabIndex = 0;
            // 
            // buttonApplyAndFire
            // 
            this.buttonApplyAndFire.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonApplyAndFire.Location = new System.Drawing.Point(344, 187);
            this.buttonApplyAndFire.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonApplyAndFire.Name = "buttonApplyAndFire";
            this.buttonApplyAndFire.Size = new System.Drawing.Size(130, 50);
            this.buttonApplyAndFire.TabIndex = 3;
            this.buttonApplyAndFire.Text = "Apply && Fire";
            this.buttonApplyAndFire.UseVisualStyleBackColor = true;
            this.buttonApplyAndFire.Click += new System.EventHandler(this.buttonApplyAndFire_Click);
            // 
            // labelDuration
            // 
            this.labelDuration.AutoSize = true;
            this.labelDuration.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDuration.Location = new System.Drawing.Point(90, 216);
            this.labelDuration.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDuration.Name = "labelDuration";
            this.labelDuration.Size = new System.Drawing.Size(102, 16);
            this.labelDuration.TabIndex = 1;
            this.labelDuration.Text = "Duration (ms):";
            // 
            // numericUpDownDuration
            // 
            this.numericUpDownDuration.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownDuration.Location = new System.Drawing.Point(200, 214);
            this.numericUpDownDuration.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.numericUpDownDuration.Maximum = new decimal(new int[] {
            1661992959,
            1808227885,
            5,
            0});
            this.numericUpDownDuration.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownDuration.Name = "numericUpDownDuration";
            this.numericUpDownDuration.Size = new System.Drawing.Size(136, 23);
            this.numericUpDownDuration.TabIndex = 2;
            this.numericUpDownDuration.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // buttonStop
            // 
            this.buttonStop.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonStop.Location = new System.Drawing.Point(482, 187);
            this.buttonStop.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(70, 50);
            this.buttonStop.TabIndex = 4;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // button_MeasureReady
            // 
            this.button_MeasureReady.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_MeasureReady.Location = new System.Drawing.Point(344, 131);
            this.button_MeasureReady.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_MeasureReady.Name = "button_MeasureReady";
            this.button_MeasureReady.Size = new System.Drawing.Size(130, 50);
            this.button_MeasureReady.TabIndex = 5;
            this.button_MeasureReady.Text = "Measure Ready";
            this.button_MeasureReady.UseVisualStyleBackColor = true;
            this.button_MeasureReady.Click += new System.EventHandler(this.button_MeasureReady_Click);
            // 
            // labelTargetType
            // 
            this.labelTargetType.AutoSize = true;
            this.labelTargetType.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.labelTargetType.Location = new System.Drawing.Point(344, 11);
            this.labelTargetType.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelTargetType.Name = "labelTargetType";
            this.labelTargetType.Size = new System.Drawing.Size(90, 16);
            this.labelTargetType.TabIndex = 6;
            this.labelTargetType.Text = "Target Type:";
            // 
            // comboBoxTargetType
            // 
            this.comboBoxTargetType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTargetType.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.comboBoxTargetType.FormattingEnabled = true;
            this.comboBoxTargetType.Items.AddRange(new object[] {
            "Top",
            "Stage"});
            this.comboBoxTargetType.Location = new System.Drawing.Point(344, 29);
            this.comboBoxTargetType.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxTargetType.Name = "comboBoxTargetType";
            this.comboBoxTargetType.Size = new System.Drawing.Size(130, 24);
            this.comboBoxTargetType.TabIndex = 7;
            this.comboBoxTargetType.SelectedIndexChanged += new System.EventHandler(this.comboBoxTargetType_SelectedIndexChanged);
            // 
            // button_SeqStart
            // 
            this.button_SeqStart.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_SeqStart.Location = new System.Drawing.Point(344, 58);
            this.button_SeqStart.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_SeqStart.Name = "button_SeqStart";
            this.button_SeqStart.Size = new System.Drawing.Size(130, 50);
            this.button_SeqStart.TabIndex = 8;
            this.button_SeqStart.Text = "Start";
            this.button_SeqStart.UseVisualStyleBackColor = true;
            this.button_SeqStart.Click += new System.EventHandler(this.button_SeqStart_Click);
            // 
            // button_SeqStop
            // 
            this.button_SeqStop.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_SeqStop.Location = new System.Drawing.Point(482, 58);
            this.button_SeqStop.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_SeqStop.Name = "button_SeqStop";
            this.button_SeqStop.Size = new System.Drawing.Size(70, 50);
            this.button_SeqStop.TabIndex = 9;
            this.button_SeqStop.Text = "Stop";
            this.button_SeqStop.UseVisualStyleBackColor = true;
            this.button_SeqStop.Click += new System.EventHandler(this.button_SeqStop_Click);
            // 
            // listBox_PowerLog
            // 
            this.listBox_PowerLog.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.listBox_PowerLog.ItemHeight = 16;
            this.listBox_PowerLog.Location = new System.Drawing.Point(559, 11);
            this.listBox_PowerLog.Name = "listBox_PowerLog";
            this.listBox_PowerLog.Size = new System.Drawing.Size(240, 228);
            this.listBox_PowerLog.TabIndex = 10;
            // 
            // button_Test
            // 
            this.button_Test.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Test.Location = new System.Drawing.Point(482, 11);
            this.button_Test.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_Test.Name = "button_Test";
            this.button_Test.Size = new System.Drawing.Size(70, 24);
            this.button_Test.TabIndex = 11;
            this.button_Test.Text = "Test";
            this.button_Test.UseVisualStyleBackColor = true;
            this.button_Test.Click += new System.EventHandler(this.button_Test_Click);
            // 
            // FormNewSub_LaserPowerMeasure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(822, 249);
            this.Controls.Add(this.button_Test);
            this.Controls.Add(this.button_SeqStop);
            this.Controls.Add(this.button_SeqStart);
            this.Controls.Add(this.button_MeasureReady);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonApplyAndFire);
            this.Controls.Add(this.numericUpDownDuration);
            this.Controls.Add(this.labelDuration);
            this.Controls.Add(this.dataGridViewSettings);
            this.Controls.Add(this.labelTargetType);
            this.Controls.Add(this.comboBoxTargetType);
            this.Controls.Add(this.listBox_PowerLog);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormNewSub_LaserPowerMeasure";
            this.Text = "Laser Power Measure";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSettings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button button_MeasureReady;
        private System.Windows.Forms.Button button_SeqStart;
        private System.Windows.Forms.Button button_SeqStop;
        private System.Windows.Forms.Button button_Test;
    }
}