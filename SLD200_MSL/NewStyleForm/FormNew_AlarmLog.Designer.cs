using System.Drawing;
using System.Windows.Forms;

namespace SLD200_MSL
{
    partial class FormNew_AlarmLog
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.m_DataGridViewAlarmLog = new QMC.Common.UI.BaseDataGridView();
            this.m_AlarmLogOptionControl = new SLD200_MSL.AlarmLogOptionControl();
            this.m_AlarmInfoControl = new SLD200_MSL.AlarmInfoControl();
            ((System.ComponentModel.ISupportInitialize)(this.m_DataGridViewAlarmLog)).BeginInit();
            this.SuspendLayout();
            // 
            // m_DataGridViewAlarmLog
            // 
            this.m_DataGridViewAlarmLog.AllowUserToAddRows = false;
            this.m_DataGridViewAlarmLog.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.m_DataGridViewAlarmLog.BackgroundColor = System.Drawing.Color.White;
            this.m_DataGridViewAlarmLog.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 12F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_DataGridViewAlarmLog.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.m_DataGridViewAlarmLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 12F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.m_DataGridViewAlarmLog.DefaultCellStyle = dataGridViewCellStyle2;
            this.m_DataGridViewAlarmLog.Font = new System.Drawing.Font("Tahoma", 12F);
            this.m_DataGridViewAlarmLog.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            this.m_DataGridViewAlarmLog.Location = new System.Drawing.Point(555, 13);
            this.m_DataGridViewAlarmLog.Margin = new System.Windows.Forms.Padding(4);
            this.m_DataGridViewAlarmLog.Name = "m_DataGridViewAlarmLog";
            this.m_DataGridViewAlarmLog.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 12F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_DataGridViewAlarmLog.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.m_DataGridViewAlarmLog.RowHeadersVisible = false;
            this.m_DataGridViewAlarmLog.RowHeadersWidth = 62;
            this.m_DataGridViewAlarmLog.RowTemplate.Height = 23;
            this.m_DataGridViewAlarmLog.Size = new System.Drawing.Size(1331, 397);
            this.m_DataGridViewAlarmLog.TabIndex = 6;
            // 
            // m_AlarmLogOptionControl
            // 
            this.m_AlarmLogOptionControl.Font = new System.Drawing.Font("Tahoma", 9F);
            this.m_AlarmLogOptionControl.Location = new System.Drawing.Point(0, 0);
            this.m_AlarmLogOptionControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.m_AlarmLogOptionControl.Name = "m_AlarmLogOptionControl";
            this.m_AlarmLogOptionControl.Size = new System.Drawing.Size(526, 877);
            this.m_AlarmLogOptionControl.TabIndex = 7;
            // 
            // m_AlarmInfoControl
            // 
            this.m_AlarmInfoControl.Font = new System.Drawing.Font("Tahoma", 9F);
            this.m_AlarmInfoControl.Location = new System.Drawing.Point(546, 442);
            this.m_AlarmInfoControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.m_AlarmInfoControl.Name = "m_AlarmInfoControl";
            this.m_AlarmInfoControl.Size = new System.Drawing.Size(1349, 430);
            this.m_AlarmInfoControl.TabIndex = 8;
            // 
            // FormNew_AlarmLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1920, 877);
            this.ControlBox = false;
            this.Controls.Add(this.m_AlarmInfoControl);
            this.Controls.Add(this.m_AlarmLogOptionControl);
            this.Controls.Add(this.m_DataGridViewAlarmLog);
            this.Font = new System.Drawing.Font("Tahoma", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormNew_AlarmLog";
            this.Text = "Alarm Dialog";
            ((System.ComponentModel.ISupportInitialize)(this.m_DataGridViewAlarmLog)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private QMC.Common.UI.BaseDataGridView m_DataGridViewAlarmLog;
        private AlarmLogOptionControl m_AlarmLogOptionControl;
        private AlarmInfoControl m_AlarmInfoControl;
    }
}