namespace SLD200_MSL
{
    partial class FormAlarm
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
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBoxRecovery = new System.Windows.Forms.GroupBox();
            this.panelComfirm = new System.Windows.Forms.Panel();
            this.groupBoxCellFocusOption = new System.Windows.Forms.GroupBox();
            this.radioButtonLastCell = new System.Windows.Forms.RadioButton();
            this.radioButtonUserSelectedCell = new System.Windows.Forms.RadioButton();
            this.groupBoxSelectedAlarmDetails = new System.Windows.Forms.GroupBox();
            this.baseTextBoxAlarmTitle = new SLD200_MSL.BaseTextBox();
            this.baseTextBoxCode = new SLD200_MSL.BaseTextBox();
            this.baseTextBoxGrade = new SLD200_MSL.BaseTextBox();
            this.baseTextBoxSource = new SLD200_MSL.BaseTextBox();
            this.baseTextBoxCause = new SLD200_MSL.BaseTextBox();
            this.baseLabelCode = new SLD200_MSL.BaseLabel();
            this.baseLabelCause = new SLD200_MSL.BaseLabel();
            this.baseLabelSource = new SLD200_MSL.BaseLabel();
            this.baseLabelGrade = new SLD200_MSL.BaseLabel();
            this.baseLabelAlarmTitle = new SLD200_MSL.BaseLabel();
            this.baseDataGridViewAlarm = new SLD200_MSL.BaseDataGridView();
            this.groupBoxRecovery.SuspendLayout();
            this.groupBoxCellFocusOption.SuspendLayout();
            this.groupBoxSelectedAlarmDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.baseDataGridViewAlarm)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxRecovery
            // 
            this.groupBoxRecovery.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxRecovery.Controls.Add(this.panelComfirm);
            this.groupBoxRecovery.ForeColor = System.Drawing.Color.White;
            this.groupBoxRecovery.Location = new System.Drawing.Point(1182, 20);
            this.groupBoxRecovery.Name = "groupBoxRecovery";
            this.groupBoxRecovery.Size = new System.Drawing.Size(198, 171);
            this.groupBoxRecovery.TabIndex = 2;
            this.groupBoxRecovery.TabStop = false;
            this.groupBoxRecovery.Text = "Recovery";
            // 
            // panelComfirm
            // 
            this.panelComfirm.Location = new System.Drawing.Point(17, 32);
            this.panelComfirm.Name = "panelComfirm";
            this.panelComfirm.Size = new System.Drawing.Size(153, 100);
            this.panelComfirm.TabIndex = 0;
            // 
            // groupBoxCellFocusOption
            // 
            this.groupBoxCellFocusOption.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxCellFocusOption.Controls.Add(this.radioButtonLastCell);
            this.groupBoxCellFocusOption.Controls.Add(this.radioButtonUserSelectedCell);
            this.groupBoxCellFocusOption.ForeColor = System.Drawing.Color.White;
            this.groupBoxCellFocusOption.Location = new System.Drawing.Point(1182, 206);
            this.groupBoxCellFocusOption.Name = "groupBoxCellFocusOption";
            this.groupBoxCellFocusOption.Size = new System.Drawing.Size(200, 100);
            this.groupBoxCellFocusOption.TabIndex = 3;
            this.groupBoxCellFocusOption.TabStop = false;
            this.groupBoxCellFocusOption.Text = "CellFocusOption";
            // 
            // radioButtonLastCell
            // 
            this.radioButtonLastCell.AutoSize = true;
            this.radioButtonLastCell.Location = new System.Drawing.Point(16, 66);
            this.radioButtonLastCell.Name = "radioButtonLastCell";
            this.radioButtonLastCell.Size = new System.Drawing.Size(69, 16);
            this.radioButtonLastCell.TabIndex = 1;
            this.radioButtonLastCell.TabStop = true;
            this.radioButtonLastCell.Text = "LastCell";
            this.radioButtonLastCell.UseVisualStyleBackColor = true;
            // 
            // radioButtonUserSelectedCell
            // 
            this.radioButtonUserSelectedCell.AutoSize = true;
            this.radioButtonUserSelectedCell.Location = new System.Drawing.Point(16, 31);
            this.radioButtonUserSelectedCell.Name = "radioButtonUserSelectedCell";
            this.radioButtonUserSelectedCell.Size = new System.Drawing.Size(120, 16);
            this.radioButtonUserSelectedCell.TabIndex = 0;
            this.radioButtonUserSelectedCell.TabStop = true;
            this.radioButtonUserSelectedCell.Text = "UserSelectedCell";
            this.radioButtonUserSelectedCell.UseVisualStyleBackColor = true;
            // 
            // groupBoxSelectedAlarmDetails
            // 
            this.groupBoxSelectedAlarmDetails.Controls.Add(this.baseTextBoxAlarmTitle);
            this.groupBoxSelectedAlarmDetails.Controls.Add(this.baseTextBoxCode);
            this.groupBoxSelectedAlarmDetails.Controls.Add(this.baseTextBoxGrade);
            this.groupBoxSelectedAlarmDetails.Controls.Add(this.baseTextBoxSource);
            this.groupBoxSelectedAlarmDetails.Controls.Add(this.baseTextBoxCause);
            this.groupBoxSelectedAlarmDetails.Controls.Add(this.baseLabelCode);
            this.groupBoxSelectedAlarmDetails.Controls.Add(this.groupBoxCellFocusOption);
            this.groupBoxSelectedAlarmDetails.Controls.Add(this.groupBoxRecovery);
            this.groupBoxSelectedAlarmDetails.Controls.Add(this.baseLabelCause);
            this.groupBoxSelectedAlarmDetails.Controls.Add(this.baseLabelSource);
            this.groupBoxSelectedAlarmDetails.Controls.Add(this.baseLabelGrade);
            this.groupBoxSelectedAlarmDetails.Controls.Add(this.baseLabelAlarmTitle);
            this.groupBoxSelectedAlarmDetails.ForeColor = System.Drawing.Color.White;
            this.groupBoxSelectedAlarmDetails.Location = new System.Drawing.Point(209, 3);
            this.groupBoxSelectedAlarmDetails.Name = "groupBoxSelectedAlarmDetails";
            this.groupBoxSelectedAlarmDetails.Size = new System.Drawing.Size(1390, 320);
            this.groupBoxSelectedAlarmDetails.TabIndex = 4;
            this.groupBoxSelectedAlarmDetails.TabStop = false;
            this.groupBoxSelectedAlarmDetails.Text = "SelectedAlarmDetails";
            // 
            // baseTextBoxAlarmTitle
            // 
            this.baseTextBoxAlarmTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.baseTextBoxAlarmTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxAlarmTitle.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxAlarmTitle.Location = new System.Drawing.Point(81, 34);
            this.baseTextBoxAlarmTitle.Multiline = true;
            this.baseTextBoxAlarmTitle.Name = "baseTextBoxAlarmTitle";
            this.baseTextBoxAlarmTitle.Size = new System.Drawing.Size(1081, 26);
            this.baseTextBoxAlarmTitle.TabIndex = 19;
            // 
            // baseTextBoxCode
            // 
            this.baseTextBoxCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.baseTextBoxCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxCode.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxCode.Location = new System.Drawing.Point(716, 88);
            this.baseTextBoxCode.Multiline = true;
            this.baseTextBoxCode.Name = "baseTextBoxCode";
            this.baseTextBoxCode.Size = new System.Drawing.Size(446, 27);
            this.baseTextBoxCode.TabIndex = 19;
            // 
            // baseTextBoxGrade
            // 
            this.baseTextBoxGrade.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.baseTextBoxGrade.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxGrade.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxGrade.Location = new System.Drawing.Point(81, 88);
            this.baseTextBoxGrade.Multiline = true;
            this.baseTextBoxGrade.Name = "baseTextBoxGrade";
            this.baseTextBoxGrade.Size = new System.Drawing.Size(574, 27);
            this.baseTextBoxGrade.TabIndex = 18;
            // 
            // baseTextBoxSource
            // 
            this.baseTextBoxSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.baseTextBoxSource.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxSource.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxSource.Location = new System.Drawing.Point(81, 146);
            this.baseTextBoxSource.Multiline = true;
            this.baseTextBoxSource.Name = "baseTextBoxSource";
            this.baseTextBoxSource.Size = new System.Drawing.Size(1081, 45);
            this.baseTextBoxSource.TabIndex = 17;
            // 
            // baseTextBoxCause
            // 
            this.baseTextBoxCause.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.baseTextBoxCause.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxCause.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxCause.Location = new System.Drawing.Point(81, 225);
            this.baseTextBoxCause.Multiline = true;
            this.baseTextBoxCause.Name = "baseTextBoxCause";
            this.baseTextBoxCause.Size = new System.Drawing.Size(1081, 63);
            this.baseTextBoxCause.TabIndex = 16;
            // 
            // baseLabelCode
            // 
            this.baseLabelCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.baseLabelCode.AutoSize = true;
            this.baseLabelCode.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelCode.ForeColor = System.Drawing.Color.White;
            this.baseLabelCode.Location = new System.Drawing.Point(660, 87);
            this.baseLabelCode.Name = "baseLabelCode";
            this.baseLabelCode.Size = new System.Drawing.Size(50, 16);
            this.baseLabelCode.TabIndex = 14;
            this.baseLabelCode.Text = "Code";
            // 
            // baseLabelCause
            // 
            this.baseLabelCause.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.baseLabelCause.AutoSize = true;
            this.baseLabelCause.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelCause.ForeColor = System.Drawing.Color.White;
            this.baseLabelCause.Location = new System.Drawing.Point(3, 225);
            this.baseLabelCause.Name = "baseLabelCause";
            this.baseLabelCause.Size = new System.Drawing.Size(59, 16);
            this.baseLabelCause.TabIndex = 7;
            this.baseLabelCause.Text = "Cause";
            // 
            // baseLabelSource
            // 
            this.baseLabelSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.baseLabelSource.AutoSize = true;
            this.baseLabelSource.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelSource.ForeColor = System.Drawing.Color.White;
            this.baseLabelSource.Location = new System.Drawing.Point(3, 146);
            this.baseLabelSource.Name = "baseLabelSource";
            this.baseLabelSource.Size = new System.Drawing.Size(65, 16);
            this.baseLabelSource.TabIndex = 6;
            this.baseLabelSource.Text = "Source";
            // 
            // baseLabelGrade
            // 
            this.baseLabelGrade.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.baseLabelGrade.AutoSize = true;
            this.baseLabelGrade.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelGrade.ForeColor = System.Drawing.Color.White;
            this.baseLabelGrade.Location = new System.Drawing.Point(3, 84);
            this.baseLabelGrade.Name = "baseLabelGrade";
            this.baseLabelGrade.Size = new System.Drawing.Size(56, 16);
            this.baseLabelGrade.TabIndex = 5;
            this.baseLabelGrade.Text = "Grade";
            // 
            // baseLabelAlarmTitle
            // 
            this.baseLabelAlarmTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.baseLabelAlarmTitle.AutoSize = true;
            this.baseLabelAlarmTitle.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelAlarmTitle.ForeColor = System.Drawing.Color.White;
            this.baseLabelAlarmTitle.Location = new System.Drawing.Point(3, 30);
            this.baseLabelAlarmTitle.Name = "baseLabelAlarmTitle";
            this.baseLabelAlarmTitle.Size = new System.Drawing.Size(41, 16);
            this.baseLabelAlarmTitle.TabIndex = 4;
            this.baseLabelAlarmTitle.Text = "Title";
            // 
            // baseDataGridViewAlarm
            // 
            this.baseDataGridViewAlarm.AllowUserToAddRows = false;
            this.baseDataGridViewAlarm.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.baseDataGridViewAlarm.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseDataGridViewAlarm.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.baseDataGridViewAlarm.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.baseDataGridViewAlarm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.baseDataGridViewAlarm.DefaultCellStyle = dataGridViewCellStyle5;
            this.baseDataGridViewAlarm.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            this.baseDataGridViewAlarm.Location = new System.Drawing.Point(210, 310);
            this.baseDataGridViewAlarm.Name = "baseDataGridViewAlarm";
            this.baseDataGridViewAlarm.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.baseDataGridViewAlarm.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.baseDataGridViewAlarm.RowHeadersVisible = false;
            this.baseDataGridViewAlarm.RowTemplate.Height = 23;
            this.baseDataGridViewAlarm.Size = new System.Drawing.Size(1390, 400);
            this.baseDataGridViewAlarm.TabIndex = 5;
            this.baseDataGridViewAlarm.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.baseDataGridViewAlarm_CellContentClick);
            // 
            // FormAlarm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1474, 708);
            this.Controls.Add(this.groupBoxSelectedAlarmDetails);
            this.Controls.Add(this.baseDataGridViewAlarm);
            this.Name = "FormAlarm";
            this.Text = "FormAlarm";
            this.Load += new System.EventHandler(this.FormAlarm_Load);
            this.VisibleChanged += new System.EventHandler(this.FormAlarm_VisibleChanged);
            this.Controls.SetChildIndex(this.panelContent, 0);
            this.Controls.SetChildIndex(this.baseDataGridViewAlarm, 0);
            this.Controls.SetChildIndex(this.groupBoxSelectedAlarmDetails, 0);
            this.groupBoxRecovery.ResumeLayout(false);
            this.groupBoxCellFocusOption.ResumeLayout(false);
            this.groupBoxCellFocusOption.PerformLayout();
            this.groupBoxSelectedAlarmDetails.ResumeLayout(false);
            this.groupBoxSelectedAlarmDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.baseDataGridViewAlarm)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxRecovery;
        private System.Windows.Forms.GroupBox groupBoxCellFocusOption;
        private System.Windows.Forms.GroupBox groupBoxSelectedAlarmDetails;
        private BaseLabel baseLabelCode;
        private BaseLabel baseLabelCause;
        private BaseLabel baseLabelSource;
        private BaseLabel baseLabelGrade;
        private BaseLabel baseLabelAlarmTitle;
        private BaseDataGridView baseDataGridViewAlarm;
        private System.Windows.Forms.RadioButton radioButtonLastCell;
        private System.Windows.Forms.RadioButton radioButtonUserSelectedCell;
        private BaseTextBox baseTextBoxAlarmTitle;
        private BaseTextBox baseTextBoxCode;
        private BaseTextBox baseTextBoxGrade;
        private BaseTextBox baseTextBoxSource;
        private BaseTextBox baseTextBoxCause;
        private System.Windows.Forms.Panel panelComfirm;
    }
}