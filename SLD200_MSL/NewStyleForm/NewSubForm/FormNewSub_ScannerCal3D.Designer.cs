namespace SLD200.NewStyleForm.NewSubForm
{
    partial class FormNewSub_ScannerCal3D
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

            if (disposing)
            {
                if (m_correction3DRtcForm != null)
                {
                    m_correction3DRtcForm.Dispose();
                    m_correction3DRtcForm = null;
                }

                if (m_correction3DRtc != null)
                {
                    m_correction3DRtc = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.panelPreviewHost = new System.Windows.Forms.Panel();
            this.groupBoxSetting = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxBasePlane = new System.Windows.Forms.ComboBox();
            this.checkBoxDuplicateCheck = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoSort = new System.Windows.Forms.CheckBox();
            this.buttonBrowseTarget = new System.Windows.Forms.Button();
            this.textBoxTargetFile = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonBrowseSource = new System.Windows.Forms.Button();
            this.textBoxSourceFile = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxOutputFileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxPlaneList = new System.Windows.Forms.GroupBox();
            this.buttonValidate = new System.Windows.Forms.Button();
            this.buttonSortByZ = new System.Windows.Forms.Button();
            this.buttonAutoFillZ = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonAddFiles = new System.Windows.Forms.Button();
            this.dataGridViewPlaneFiles = new System.Windows.Forms.DataGridView();
            this.groupBoxSelectedInfo = new System.Windows.Forms.GroupBox();
            this.labelSelectedGridMatch = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.labelSelectedPointCount = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.labelSelectedMaxY = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.labelSelectedMinY = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.labelSelectedMaxX = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.labelSelectedMinX = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.labelSelectedFilePath = new System.Windows.Forms.Label();
            this.groupBoxLog = new System.Windows.Forms.GroupBox();
            this.richTextBoxLog = new System.Windows.Forms.RichTextBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.groupBoxSetting.SuspendLayout();
            this.groupBoxPlaneList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPlaneFiles)).BeginInit();
            this.groupBoxSelectedInfo.SuspendLayout();
            this.groupBoxLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelPreviewHost
            // 
            this.panelPreviewHost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelPreviewHost.Location = new System.Drawing.Point(12, 12);
            this.panelPreviewHost.Name = "panelPreviewHost";
            this.panelPreviewHost.Size = new System.Drawing.Size(560, 520);
            this.panelPreviewHost.TabIndex = 0;
            // 
            // groupBoxSetting
            // 
            this.groupBoxSetting.Controls.Add(this.label4);
            this.groupBoxSetting.Controls.Add(this.comboBoxBasePlane);
            this.groupBoxSetting.Controls.Add(this.checkBoxDuplicateCheck);
            this.groupBoxSetting.Controls.Add(this.checkBoxAutoSort);
            this.groupBoxSetting.Controls.Add(this.buttonBrowseTarget);
            this.groupBoxSetting.Controls.Add(this.textBoxTargetFile);
            this.groupBoxSetting.Controls.Add(this.label3);
            this.groupBoxSetting.Controls.Add(this.buttonBrowseSource);
            this.groupBoxSetting.Controls.Add(this.textBoxSourceFile);
            this.groupBoxSetting.Controls.Add(this.label2);
            this.groupBoxSetting.Controls.Add(this.textBoxOutputFileName);
            this.groupBoxSetting.Controls.Add(this.label1);
            this.groupBoxSetting.Location = new System.Drawing.Point(578, 12);
            this.groupBoxSetting.Name = "groupBoxSetting";
            this.groupBoxSetting.Size = new System.Drawing.Size(610, 128);
            this.groupBoxSetting.TabIndex = 1;
            this.groupBoxSetting.TabStop = false;
            this.groupBoxSetting.Text = "기본 설정";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 20);
            this.label4.TabIndex = 11;
            this.label4.Text = "Base Plane";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxBasePlane
            // 
            this.comboBoxBasePlane.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBasePlane.FormattingEnabled = true;
            this.comboBoxBasePlane.Location = new System.Drawing.Point(106, 96);
            this.comboBoxBasePlane.Name = "comboBoxBasePlane";
            this.comboBoxBasePlane.Size = new System.Drawing.Size(220, 20);
            this.comboBoxBasePlane.TabIndex = 10;
            // 
            // checkBoxDuplicateCheck
            // 
            this.checkBoxDuplicateCheck.AutoSize = true;
            this.checkBoxDuplicateCheck.Checked = true;
            this.checkBoxDuplicateCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDuplicateCheck.Location = new System.Drawing.Point(470, 99);
            this.checkBoxDuplicateCheck.Name = "checkBoxDuplicateCheck";
            this.checkBoxDuplicateCheck.Size = new System.Drawing.Size(115, 16);
            this.checkBoxDuplicateCheck.TabIndex = 9;
            this.checkBoxDuplicateCheck.Text = "Duplicate Check";
            this.checkBoxDuplicateCheck.UseVisualStyleBackColor = true;
            // 
            // checkBoxAutoSort
            // 
            this.checkBoxAutoSort.AutoSize = true;
            this.checkBoxAutoSort.Checked = true;
            this.checkBoxAutoSort.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAutoSort.Location = new System.Drawing.Point(367, 99);
            this.checkBoxAutoSort.Name = "checkBoxAutoSort";
            this.checkBoxAutoSort.Size = new System.Drawing.Size(74, 16);
            this.checkBoxAutoSort.TabIndex = 8;
            this.checkBoxAutoSort.Text = "Auto Sort";
            this.checkBoxAutoSort.UseVisualStyleBackColor = true;
            // 
            // buttonBrowseTarget
            // 
            this.buttonBrowseTarget.Location = new System.Drawing.Point(530, 58);
            this.buttonBrowseTarget.Name = "buttonBrowseTarget";
            this.buttonBrowseTarget.Size = new System.Drawing.Size(70, 23);
            this.buttonBrowseTarget.TabIndex = 7;
            this.buttonBrowseTarget.Text = "Browse";
            this.buttonBrowseTarget.UseVisualStyleBackColor = true;
            // 
            // textBoxTargetFile
            // 
            this.textBoxTargetFile.Location = new System.Drawing.Point(106, 60);
            this.textBoxTargetFile.Name = "textBoxTargetFile";
            this.textBoxTargetFile.Size = new System.Drawing.Size(418, 21);
            this.textBoxTargetFile.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Target 3D File";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonBrowseSource
            // 
            this.buttonBrowseSource.Location = new System.Drawing.Point(530, 24);
            this.buttonBrowseSource.Name = "buttonBrowseSource";
            this.buttonBrowseSource.Size = new System.Drawing.Size(70, 23);
            this.buttonBrowseSource.TabIndex = 4;
            this.buttonBrowseSource.Text = "Browse";
            this.buttonBrowseSource.UseVisualStyleBackColor = true;
            // 
            // textBoxSourceFile
            // 
            this.textBoxSourceFile.Location = new System.Drawing.Point(106, 26);
            this.textBoxSourceFile.Name = "textBoxSourceFile";
            this.textBoxSourceFile.Size = new System.Drawing.Size(418, 21);
            this.textBoxSourceFile.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Source CT5";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxOutputFileName
            // 
            this.textBoxOutputFileName.Location = new System.Drawing.Point(367, 96);
            this.textBoxOutputFileName.Name = "textBoxOutputFileName";
            this.textBoxOutputFileName.Size = new System.Drawing.Size(0, 21);
            this.textBoxOutputFileName.TabIndex = 1;
            this.textBoxOutputFileName.Visible = false;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 0);
            this.label1.TabIndex = 0;
            // 
            // groupBoxPlaneList
            // 
            this.groupBoxPlaneList.Controls.Add(this.buttonValidate);
            this.groupBoxPlaneList.Controls.Add(this.buttonSortByZ);
            this.groupBoxPlaneList.Controls.Add(this.buttonAutoFillZ);
            this.groupBoxPlaneList.Controls.Add(this.buttonRemove);
            this.groupBoxPlaneList.Controls.Add(this.buttonAddFiles);
            this.groupBoxPlaneList.Controls.Add(this.dataGridViewPlaneFiles);
            this.groupBoxPlaneList.Location = new System.Drawing.Point(578, 146);
            this.groupBoxPlaneList.Name = "groupBoxPlaneList";
            this.groupBoxPlaneList.Size = new System.Drawing.Size(610, 386);
            this.groupBoxPlaneList.TabIndex = 2;
            this.groupBoxPlaneList.TabStop = false;
            this.groupBoxPlaneList.Text = "2D Plane File List";
            // 
            // buttonValidate
            // 
            this.buttonValidate.Location = new System.Drawing.Point(524, 22);
            this.buttonValidate.Name = "buttonValidate";
            this.buttonValidate.Size = new System.Drawing.Size(76, 25);
            this.buttonValidate.TabIndex = 5;
            this.buttonValidate.Text = "Validate";
            this.buttonValidate.UseVisualStyleBackColor = true;
            // 
            // buttonSortByZ
            // 
            this.buttonSortByZ.Location = new System.Drawing.Point(442, 22);
            this.buttonSortByZ.Name = "buttonSortByZ";
            this.buttonSortByZ.Size = new System.Drawing.Size(76, 25);
            this.buttonSortByZ.TabIndex = 4;
            this.buttonSortByZ.Text = "Sort by Z";
            this.buttonSortByZ.UseVisualStyleBackColor = true;
            // 
            // buttonAutoFillZ
            // 
            this.buttonAutoFillZ.Location = new System.Drawing.Point(360, 22);
            this.buttonAutoFillZ.Name = "buttonAutoFillZ";
            this.buttonAutoFillZ.Size = new System.Drawing.Size(76, 25);
            this.buttonAutoFillZ.TabIndex = 3;
            this.buttonAutoFillZ.Text = "Auto Fill Z";
            this.buttonAutoFillZ.UseVisualStyleBackColor = true;
            // 
            // buttonRemove
            // 
            this.buttonRemove.Location = new System.Drawing.Point(278, 22);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(76, 25);
            this.buttonRemove.TabIndex = 2;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            // 
            // buttonAddFiles
            // 
            this.buttonAddFiles.Location = new System.Drawing.Point(196, 22);
            this.buttonAddFiles.Name = "buttonAddFiles";
            this.buttonAddFiles.Size = new System.Drawing.Size(76, 25);
            this.buttonAddFiles.TabIndex = 1;
            this.buttonAddFiles.Text = "Add Files";
            this.buttonAddFiles.UseVisualStyleBackColor = true;
            // 
            // dataGridViewPlaneFiles
            // 
            this.dataGridViewPlaneFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPlaneFiles.Location = new System.Drawing.Point(10, 55);
            this.dataGridViewPlaneFiles.Name = "dataGridViewPlaneFiles";
            this.dataGridViewPlaneFiles.RowTemplate.Height = 23;
            this.dataGridViewPlaneFiles.Size = new System.Drawing.Size(590, 321);
            this.dataGridViewPlaneFiles.TabIndex = 0;
            // 
            // groupBoxSelectedInfo
            // 
            this.groupBoxSelectedInfo.Controls.Add(this.labelSelectedGridMatch);
            this.groupBoxSelectedInfo.Controls.Add(this.label15);
            this.groupBoxSelectedInfo.Controls.Add(this.labelSelectedPointCount);
            this.groupBoxSelectedInfo.Controls.Add(this.label13);
            this.groupBoxSelectedInfo.Controls.Add(this.labelSelectedMaxY);
            this.groupBoxSelectedInfo.Controls.Add(this.label11);
            this.groupBoxSelectedInfo.Controls.Add(this.labelSelectedMinY);
            this.groupBoxSelectedInfo.Controls.Add(this.label9);
            this.groupBoxSelectedInfo.Controls.Add(this.labelSelectedMaxX);
            this.groupBoxSelectedInfo.Controls.Add(this.label7);
            this.groupBoxSelectedInfo.Controls.Add(this.labelSelectedMinX);
            this.groupBoxSelectedInfo.Controls.Add(this.label5);
            this.groupBoxSelectedInfo.Controls.Add(this.labelSelectedFilePath);
            this.groupBoxSelectedInfo.Location = new System.Drawing.Point(12, 538);
            this.groupBoxSelectedInfo.Name = "groupBoxSelectedInfo";
            this.groupBoxSelectedInfo.Size = new System.Drawing.Size(1176, 74);
            this.groupBoxSelectedInfo.TabIndex = 3;
            this.groupBoxSelectedInfo.TabStop = false;
            this.groupBoxSelectedInfo.Text = "Selected File Info";
            // 
            // labelSelectedGridMatch
            // 
            this.labelSelectedGridMatch.AutoSize = true;
            this.labelSelectedGridMatch.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.labelSelectedGridMatch.Location = new System.Drawing.Point(1050, 48);
            this.labelSelectedGridMatch.Name = "labelSelectedGridMatch";
            this.labelSelectedGridMatch.Size = new System.Drawing.Size(21, 12);
            this.labelSelectedGridMatch.TabIndex = 12;
            this.labelSelectedGridMatch.Text = "-";
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(960, 45);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(85, 18);
            this.label15.TabIndex = 11;
            this.label15.Text = "Grid Match";
            // 
            // labelSelectedPointCount
            // 
            this.labelSelectedPointCount.AutoSize = true;
            this.labelSelectedPointCount.Location = new System.Drawing.Point(860, 48);
            this.labelSelectedPointCount.Name = "labelSelectedPointCount";
            this.labelSelectedPointCount.Size = new System.Drawing.Size(9, 12);
            this.labelSelectedPointCount.TabIndex = 10;
            this.labelSelectedPointCount.Text = "-";
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(770, 45);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(85, 18);
            this.label13.TabIndex = 9;
            this.label13.Text = "Point Count";
            // 
            // labelSelectedMaxY
            // 
            this.labelSelectedMaxY.AutoSize = true;
            this.labelSelectedMaxY.Location = new System.Drawing.Point(670, 48);
            this.labelSelectedMaxY.Name = "labelSelectedMaxY";
            this.labelSelectedMaxY.Size = new System.Drawing.Size(9, 12);
            this.labelSelectedMaxY.TabIndex = 8;
            this.labelSelectedMaxY.Text = "-";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(580, 45);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(85, 18);
            this.label11.TabIndex = 7;
            this.label11.Text = "Max Y";
            // 
            // labelSelectedMinY
            // 
            this.labelSelectedMinY.AutoSize = true;
            this.labelSelectedMinY.Location = new System.Drawing.Point(480, 48);
            this.labelSelectedMinY.Name = "labelSelectedMinY";
            this.labelSelectedMinY.Size = new System.Drawing.Size(9, 12);
            this.labelSelectedMinY.TabIndex = 6;
            this.labelSelectedMinY.Text = "-";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(390, 45);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 18);
            this.label9.TabIndex = 5;
            this.label9.Text = "Min Y";
            // 
            // labelSelectedMaxX
            // 
            this.labelSelectedMaxX.AutoSize = true;
            this.labelSelectedMaxX.Location = new System.Drawing.Point(290, 48);
            this.labelSelectedMaxX.Name = "labelSelectedMaxX";
            this.labelSelectedMaxX.Size = new System.Drawing.Size(9, 12);
            this.labelSelectedMaxX.TabIndex = 4;
            this.labelSelectedMaxX.Text = "-";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(200, 45);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(85, 18);
            this.label7.TabIndex = 3;
            this.label7.Text = "Max X";
            // 
            // labelSelectedMinX
            // 
            this.labelSelectedMinX.AutoSize = true;
            this.labelSelectedMinX.Location = new System.Drawing.Point(100, 48);
            this.labelSelectedMinX.Name = "labelSelectedMinX";
            this.labelSelectedMinX.Size = new System.Drawing.Size(9, 12);
            this.labelSelectedMinX.TabIndex = 2;
            this.labelSelectedMinX.Text = "-";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(10, 45);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 18);
            this.label5.TabIndex = 1;
            this.label5.Text = "Min X";
            // 
            // labelSelectedFilePath
            // 
            this.labelSelectedFilePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelSelectedFilePath.Location = new System.Drawing.Point(10, 20);
            this.labelSelectedFilePath.Name = "labelSelectedFilePath";
            this.labelSelectedFilePath.Size = new System.Drawing.Size(1150, 20);
            this.labelSelectedFilePath.TabIndex = 0;
            this.labelSelectedFilePath.Text = "-";
            this.labelSelectedFilePath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBoxLog
            // 
            this.groupBoxLog.Controls.Add(this.richTextBoxLog);
            this.groupBoxLog.Location = new System.Drawing.Point(12, 618);
            this.groupBoxLog.Name = "groupBoxLog";
            this.groupBoxLog.Size = new System.Drawing.Size(1176, 131);
            this.groupBoxLog.TabIndex = 4;
            this.groupBoxLog.TabStop = false;
            this.groupBoxLog.Text = "Log";
            // 
            // richTextBoxLog
            // 
            this.richTextBoxLog.Location = new System.Drawing.Point(10, 20);
            this.richTextBoxLog.Name = "richTextBoxLog";
            this.richTextBoxLog.ReadOnly = true;
            this.richTextBoxLog.Size = new System.Drawing.Size(1150, 101);
            this.richTextBoxLog.TabIndex = 0;
            this.richTextBoxLog.Text = "";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(1088, 755);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(100, 32);
            this.buttonSave.TabIndex = 5;
            this.buttonSave.Text = "Build && Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            // 
            // FormNewSub_ScannerCal3D
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 799);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.groupBoxLog);
            this.Controls.Add(this.groupBoxSelectedInfo);
            this.Controls.Add(this.groupBoxPlaneList);
            this.Controls.Add(this.groupBoxSetting);
            this.Controls.Add(this.panelPreviewHost);
            this.Name = "FormNewSub_ScannerCal3D";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Scanner 3D Calibration Builder";
            this.groupBoxSetting.ResumeLayout(false);
            this.groupBoxSetting.PerformLayout();
            this.groupBoxPlaneList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPlaneFiles)).EndInit();
            this.groupBoxSelectedInfo.ResumeLayout(false);
            this.groupBoxSelectedInfo.PerformLayout();
            this.groupBoxLog.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelPreviewHost;
        private System.Windows.Forms.GroupBox groupBoxSetting;
        private System.Windows.Forms.Button buttonBrowseTarget;
        private System.Windows.Forms.TextBox textBoxTargetFile;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonBrowseSource;
        private System.Windows.Forms.TextBox textBoxSourceFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxOutputFileName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBoxDuplicateCheck;
        private System.Windows.Forms.CheckBox checkBoxAutoSort;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxBasePlane;
        private System.Windows.Forms.GroupBox groupBoxPlaneList;
        private System.Windows.Forms.Button buttonValidate;
        private System.Windows.Forms.Button buttonSortByZ;
        private System.Windows.Forms.Button buttonAutoFillZ;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonAddFiles;
        private System.Windows.Forms.DataGridView dataGridViewPlaneFiles;
        private System.Windows.Forms.GroupBox groupBoxSelectedInfo;
        private System.Windows.Forms.Label labelSelectedGridMatch;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label labelSelectedPointCount;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label labelSelectedMaxY;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label labelSelectedMinY;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label labelSelectedMaxX;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelSelectedMinX;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelSelectedFilePath;
        private System.Windows.Forms.GroupBox groupBoxLog;
        private System.Windows.Forms.RichTextBox richTextBoxLog;
        private System.Windows.Forms.Button buttonSave;
    }
}