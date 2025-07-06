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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.comboBox_BETPositionIndex = new System.Windows.Forms.ComboBox();
            this.comboBox_MaskIndex = new System.Windows.Forms.ComboBox();
            this.label_Recipe_TabRecipe_Miscellaneous_BETPosition = new System.Windows.Forms.Label();
            this.label_Recipe_TabRecipe_Miscellaneous_Mask = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button_Param_Save = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSettings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridViewSettings
            // 
            this.dataGridViewSettings.AllowUserToAddRows = false;
            this.dataGridViewSettings.AllowUserToDeleteRows = false;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.Format = "N0";
            dataGridViewCellStyle3.NullValue = "0";
            this.dataGridViewSettings.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewSettings.BackgroundColor = System.Drawing.SystemColors.Menu;
            this.dataGridViewSettings.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridViewSettings.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
            this.dataGridViewSettings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.Format = "N0";
            dataGridViewCellStyle4.NullValue = "0";
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewSettings.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewSettings.Location = new System.Drawing.Point(7, 44);
            this.dataGridViewSettings.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dataGridViewSettings.Name = "dataGridViewSettings";
            this.dataGridViewSettings.RowHeadersWidth = 62;
            this.dataGridViewSettings.Size = new System.Drawing.Size(322, 178);
            this.dataGridViewSettings.TabIndex = 0;
            this.dataGridViewSettings.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewSettings_CellValueChanged);
            this.dataGridViewSettings.CurrentCellChanged += new System.EventHandler(this.dataGridViewSettings_CurrentCellChanged);
            // 
            // buttonApplyAndFire
            // 
            this.buttonApplyAndFire.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonApplyAndFire.Location = new System.Drawing.Point(7, 76);
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
            this.labelDuration.Location = new System.Drawing.Point(75, 287);
            this.labelDuration.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDuration.Name = "labelDuration";
            this.labelDuration.Size = new System.Drawing.Size(106, 16);
            this.labelDuration.TabIndex = 1;
            this.labelDuration.Text = "Duration (sec):";
            // 
            // numericUpDownDuration
            // 
            this.numericUpDownDuration.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownDuration.Location = new System.Drawing.Point(185, 285);
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
            30,
            0,
            0,
            0});
            // 
            // buttonStop
            // 
            this.buttonStop.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonStop.Location = new System.Drawing.Point(145, 76);
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
            this.button_MeasureReady.Location = new System.Drawing.Point(7, 20);
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
            this.labelTargetType.Location = new System.Drawing.Point(4, 17);
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
            this.comboBoxTargetType.Location = new System.Drawing.Point(98, 14);
            this.comboBoxTargetType.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxTargetType.Name = "comboBoxTargetType";
            this.comboBoxTargetType.Size = new System.Drawing.Size(130, 24);
            this.comboBoxTargetType.TabIndex = 7;
            this.comboBoxTargetType.SelectedIndexChanged += new System.EventHandler(this.comboBoxTargetType_SelectedIndexChanged);
            // 
            // button_SeqStart
            // 
            this.button_SeqStart.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_SeqStart.Location = new System.Drawing.Point(7, 20);
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
            this.button_SeqStop.Location = new System.Drawing.Point(145, 20);
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
            this.listBox_PowerLog.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.listBox_PowerLog.ItemHeight = 16;
            this.listBox_PowerLog.Location = new System.Drawing.Point(588, 12);
            this.listBox_PowerLog.Name = "listBox_PowerLog";
            this.listBox_PowerLog.Size = new System.Drawing.Size(240, 260);
            this.listBox_PowerLog.TabIndex = 10;
            // 
            // button_Test
            // 
            this.button_Test.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Test.Location = new System.Drawing.Point(1, 283);
            this.button_Test.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_Test.Name = "button_Test";
            this.button_Test.Size = new System.Drawing.Size(70, 24);
            this.button_Test.TabIndex = 11;
            this.button_Test.Text = "Test";
            this.button_Test.UseVisualStyleBackColor = true;
            this.button_Test.Visible = false;
            this.button_Test.Click += new System.EventHandler(this.button_Test_Click);
            // 
            // comboBox_BETPositionIndex
            // 
            this.comboBox_BETPositionIndex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_BETPositionIndex.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_BETPositionIndex.FormattingEnabled = true;
            this.comboBox_BETPositionIndex.Items.AddRange(new object[] {
            "0.8x",
            "0.9x",
            "1.0x",
            "1.1x",
            "1.2x"});
            this.comboBox_BETPositionIndex.Location = new System.Drawing.Point(185, 256);
            this.comboBox_BETPositionIndex.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBox_BETPositionIndex.Name = "comboBox_BETPositionIndex";
            this.comboBox_BETPositionIndex.Size = new System.Drawing.Size(73, 24);
            this.comboBox_BETPositionIndex.TabIndex = 232;
            this.comboBox_BETPositionIndex.SelectedIndexChanged += new System.EventHandler(this.comboBox_BETPositionIndex_SelectedIndexChanged);
            // 
            // comboBox_MaskIndex
            // 
            this.comboBox_MaskIndex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_MaskIndex.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_MaskIndex.FormattingEnabled = true;
            this.comboBox_MaskIndex.Items.AddRange(new object[] {
            "None Mask Position",
            "#1 Mask Position",
            "#2 Mask Position",
            "#3 Mask Position",
            "#4 Mask Position"});
            this.comboBox_MaskIndex.Location = new System.Drawing.Point(185, 229);
            this.comboBox_MaskIndex.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBox_MaskIndex.Name = "comboBox_MaskIndex";
            this.comboBox_MaskIndex.Size = new System.Drawing.Size(144, 24);
            this.comboBox_MaskIndex.TabIndex = 231;
            this.comboBox_MaskIndex.SelectedIndexChanged += new System.EventHandler(this.comboBox_MaskIndex_SelectedIndexChanged);
            // 
            // label_Recipe_TabRecipe_Miscellaneous_BETPosition
            // 
            this.label_Recipe_TabRecipe_Miscellaneous_BETPosition.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.label_Recipe_TabRecipe_Miscellaneous_BETPosition.Location = new System.Drawing.Point(7, 255);
            this.label_Recipe_TabRecipe_Miscellaneous_BETPosition.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.label_Recipe_TabRecipe_Miscellaneous_BETPosition.Name = "label_Recipe_TabRecipe_Miscellaneous_BETPosition";
            this.label_Recipe_TabRecipe_Miscellaneous_BETPosition.Size = new System.Drawing.Size(174, 25);
            this.label_Recipe_TabRecipe_Miscellaneous_BETPosition.TabIndex = 230;
            this.label_Recipe_TabRecipe_Miscellaneous_BETPosition.Text = "BET Zoom Position :";
            this.label_Recipe_TabRecipe_Miscellaneous_BETPosition.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_Recipe_TabRecipe_Miscellaneous_Mask
            // 
            this.label_Recipe_TabRecipe_Miscellaneous_Mask.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.label_Recipe_TabRecipe_Miscellaneous_Mask.Location = new System.Drawing.Point(7, 229);
            this.label_Recipe_TabRecipe_Miscellaneous_Mask.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.label_Recipe_TabRecipe_Miscellaneous_Mask.Name = "label_Recipe_TabRecipe_Miscellaneous_Mask";
            this.label_Recipe_TabRecipe_Miscellaneous_Mask.Size = new System.Drawing.Size(174, 25);
            this.label_Recipe_TabRecipe_Miscellaneous_Mask.TabIndex = 229;
            this.label_Recipe_TabRecipe_Miscellaneous_Mask.Text = "Mask Position :";
            this.label_Recipe_TabRecipe_Miscellaneous_Mask.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_Param_Save);
            this.groupBox1.Controls.Add(this.dataGridViewSettings);
            this.groupBox1.Controls.Add(this.comboBox_BETPositionIndex);
            this.groupBox1.Controls.Add(this.labelDuration);
            this.groupBox1.Controls.Add(this.button_Test);
            this.groupBox1.Controls.Add(this.comboBox_MaskIndex);
            this.groupBox1.Controls.Add(this.labelTargetType);
            this.groupBox1.Controls.Add(this.comboBoxTargetType);
            this.groupBox1.Controls.Add(this.numericUpDownDuration);
            this.groupBox1.Controls.Add(this.label_Recipe_TabRecipe_Miscellaneous_BETPosition);
            this.groupBox1.Controls.Add(this.label_Recipe_TabRecipe_Miscellaneous_Mask);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(337, 315);
            this.groupBox1.TabIndex = 233;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Parameter";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button_SeqStart);
            this.groupBox2.Controls.Add(this.button_SeqStop);
            this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.groupBox2.Location = new System.Drawing.Point(355, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(227, 81);
            this.groupBox2.TabIndex = 234;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Auto";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button_MeasureReady);
            this.groupBox3.Controls.Add(this.buttonApplyAndFire);
            this.groupBox3.Controls.Add(this.buttonStop);
            this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.groupBox3.Location = new System.Drawing.Point(355, 139);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(227, 132);
            this.groupBox3.TabIndex = 235;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Manual";
            // 
            // button_Param_Save
            // 
            this.button_Param_Save.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Param_Save.Location = new System.Drawing.Point(259, 14);
            this.button_Param_Save.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_Param_Save.Name = "button_Param_Save";
            this.button_Param_Save.Size = new System.Drawing.Size(70, 24);
            this.button_Param_Save.TabIndex = 233;
            this.button_Param_Save.Text = "SAVE";
            this.button_Param_Save.UseVisualStyleBackColor = true;
            this.button_Param_Save.Click += new System.EventHandler(this.button_Param_Save_Click);
            // 
            // FormNewSub_LaserPowerMeasure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(839, 331);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.listBox_PowerLog);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormNewSub_LaserPowerMeasure";
            this.Text = "Laser Power Measure";
            this.VisibleChanged += new System.EventHandler(this.FormNewSub_LaserPowerMeasure_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSettings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button button_MeasureReady;
        private System.Windows.Forms.Button button_SeqStart;
        private System.Windows.Forms.Button button_SeqStop;
        private System.Windows.Forms.Button button_Test;
        private System.Windows.Forms.ComboBox comboBox_BETPositionIndex;
        private System.Windows.Forms.ComboBox comboBox_MaskIndex;
        private System.Windows.Forms.Label label_Recipe_TabRecipe_Miscellaneous_BETPosition;
        private System.Windows.Forms.Label label_Recipe_TabRecipe_Miscellaneous_Mask;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button_Param_Save;
    }
}