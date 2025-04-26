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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.m_DataGridViewAlarmLog = new QMC.Common.UI.BaseDataGridView();
            this.alarmInfoControl1 = new SLD200_MSL.AlarmInfoControl();
            this.alarmLogOptionControl1 = new SLD200_MSL.AlarmLogOptionControl();
            ((System.ComponentModel.ISupportInitialize)(this.m_DataGridViewAlarmLog)).BeginInit();
            this.SuspendLayout();
            // 
            // m_DataGridViewAlarmLog
            // 
            this.m_DataGridViewAlarmLog.AllowUserToAddRows = false;
            this.m_DataGridViewAlarmLog.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.m_DataGridViewAlarmLog.BackgroundColor = System.Drawing.Color.White;
            this.m_DataGridViewAlarmLog.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Tahoma", 12F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_DataGridViewAlarmLog.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.m_DataGridViewAlarmLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Tahoma", 12F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.m_DataGridViewAlarmLog.DefaultCellStyle = dataGridViewCellStyle5;
            this.m_DataGridViewAlarmLog.Font = new System.Drawing.Font("Tahoma", 12F);
            this.m_DataGridViewAlarmLog.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            this.m_DataGridViewAlarmLog.Location = new System.Drawing.Point(564, 13);
            this.m_DataGridViewAlarmLog.Margin = new System.Windows.Forms.Padding(4);
            this.m_DataGridViewAlarmLog.Name = "m_DataGridViewAlarmLog";
            this.m_DataGridViewAlarmLog.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Tahoma", 12F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_DataGridViewAlarmLog.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.m_DataGridViewAlarmLog.RowHeadersVisible = false;
            this.m_DataGridViewAlarmLog.RowHeadersWidth = 62;
            this.m_DataGridViewAlarmLog.RowTemplate.Height = 23;
            this.m_DataGridViewAlarmLog.Size = new System.Drawing.Size(1331, 397);
            this.m_DataGridViewAlarmLog.TabIndex = 6;
            // 
            // alarmInfoControl1
            // 
            this.alarmInfoControl1.Font = new System.Drawing.Font("Tahoma", 9F);
            this.alarmInfoControl1.Location = new System.Drawing.Point(553, 437);
            this.alarmInfoControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.alarmInfoControl1.Name = "alarmInfoControl1";
            this.alarmInfoControl1.Size = new System.Drawing.Size(1349, 426);
            this.alarmInfoControl1.TabIndex = 8;
            // 
            // alarmLogOptionControl1
            // 
            this.alarmLogOptionControl1.Font = new System.Drawing.Font("Tahoma", 9F);
            this.alarmLogOptionControl1.Location = new System.Drawing.Point(0, 0);
            this.alarmLogOptionControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.alarmLogOptionControl1.Name = "alarmLogOptionControl1";
            this.alarmLogOptionControl1.Size = new System.Drawing.Size(526, 1094);
            this.alarmLogOptionControl1.TabIndex = 9;
            // 
            // FormNew_AlarmLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1920, 877);
            this.ControlBox = false;
            this.Controls.Add(this.alarmLogOptionControl1);
            this.Controls.Add(this.alarmInfoControl1);
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
        private AlarmInfoControl alarmInfoControl1;
        private AlarmLogOptionControl alarmLogOptionControl1;
    }
}