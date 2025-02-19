namespace SLD200_MSL
{
    partial class VisionCalScaleControl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.Movedistance = new SLD200_MSL.BaseTextBox();
            this.CalibratorPositionGrid = new SLD200_MSL.BaseDataGridView();
            this.baseLabelMoveDistance = new SLD200_MSL.BaseLabel();
            this.baseGroupBoxScale = new SLD200_MSL.BaseGroupBox();
            this.baseTextYValue = new SLD200_MSL.BaseTextBox();
            this.baseTextXValue = new SLD200_MSL.BaseTextBox();
            this.baseTextXAxisValue = new SLD200_MSL.BaseTextBox();
            this.baseTextYAxisValue = new SLD200_MSL.BaseTextBox();
            this.checkBoxEnable = new System.Windows.Forms.CheckBox();
            this.checkBoxYInverted = new System.Windows.Forms.CheckBox();
            this.checkBoxXInverted = new System.Windows.Forms.CheckBox();
            this.baseLabelYAxis = new SLD200_MSL.BaseLabel();
            this.baseLabelXAxis = new SLD200_MSL.BaseLabel();
            this.baseLabelY = new SLD200_MSL.BaseLabel();
            this.baseLabelX = new SLD200_MSL.BaseLabel();
            this.baseGroupBox1 = new SLD200_MSL.BaseGroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.CalibratorPositionGrid)).BeginInit();
            this.baseGroupBoxScale.SuspendLayout();
            this.SuspendLayout();
            // 
            // Movedistance
            // 
            this.Movedistance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.Movedistance.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Movedistance.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Movedistance.ForeColor = System.Drawing.Color.White;
            this.Movedistance.Location = new System.Drawing.Point(220, 177);
            this.Movedistance.Name = "Movedistance";
            this.Movedistance.Size = new System.Drawing.Size(130, 17);
            this.Movedistance.TabIndex = 11;
            this.Movedistance.Text = "0";
            this.Movedistance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Movedistance.TextChanged += new System.EventHandler(this.baseTextBox1_TextChanged);
            // 
            // CalibratorPositionGrid
            // 
            this.CalibratorPositionGrid.AllowUserToAddRows = false;
            this.CalibratorPositionGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.CalibratorPositionGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.CalibratorPositionGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.CalibratorPositionGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.CalibratorPositionGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.CalibratorPositionGrid.DefaultCellStyle = dataGridViewCellStyle8;
            this.CalibratorPositionGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            this.CalibratorPositionGrid.Location = new System.Drawing.Point(12, 203);
            this.CalibratorPositionGrid.Name = "CalibratorPositionGrid";
            this.CalibratorPositionGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.CalibratorPositionGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.CalibratorPositionGrid.RowHeadersVisible = false;
            this.CalibratorPositionGrid.RowTemplate.Height = 23;
            this.CalibratorPositionGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.CalibratorPositionGrid.Size = new System.Drawing.Size(371, 102);
            this.CalibratorPositionGrid.TabIndex = 10;
            // 
            // baseLabelMoveDistance
            // 
            this.baseLabelMoveDistance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelMoveDistance.ForeColor = System.Drawing.Color.White;
            this.baseLabelMoveDistance.Location = new System.Drawing.Point(14, 175);
            this.baseLabelMoveDistance.Name = "baseLabelMoveDistance";
            this.baseLabelMoveDistance.Size = new System.Drawing.Size(200, 16);
            this.baseLabelMoveDistance.TabIndex = 8;
            this.baseLabelMoveDistance.Text = "Move Distance [mm] :";
            this.baseLabelMoveDistance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // baseGroupBoxScale
            // 
            this.baseGroupBoxScale.Controls.Add(this.baseTextYValue);
            this.baseGroupBoxScale.Controls.Add(this.baseTextXValue);
            this.baseGroupBoxScale.Controls.Add(this.baseTextXAxisValue);
            this.baseGroupBoxScale.Controls.Add(this.baseTextYAxisValue);
            this.baseGroupBoxScale.Controls.Add(this.checkBoxEnable);
            this.baseGroupBoxScale.Controls.Add(this.checkBoxYInverted);
            this.baseGroupBoxScale.Controls.Add(this.checkBoxXInverted);
            this.baseGroupBoxScale.Controls.Add(this.baseLabelYAxis);
            this.baseGroupBoxScale.Controls.Add(this.baseLabelXAxis);
            this.baseGroupBoxScale.Controls.Add(this.baseLabelY);
            this.baseGroupBoxScale.Controls.Add(this.baseLabelX);
            this.baseGroupBoxScale.ForeColor = System.Drawing.Color.White;
            this.baseGroupBoxScale.Location = new System.Drawing.Point(12, 19);
            this.baseGroupBoxScale.Name = "baseGroupBoxScale";
            this.baseGroupBoxScale.Size = new System.Drawing.Size(371, 150);
            this.baseGroupBoxScale.TabIndex = 0;
            this.baseGroupBoxScale.TabStop = false;
            this.baseGroupBoxScale.Text = " Scale ";
            // 
            // baseTextYValue
            // 
            this.baseTextYValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextYValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextYValue.Font = new System.Drawing.Font("Tahoma", 11F);
            this.baseTextYValue.ForeColor = System.Drawing.Color.White;
            this.baseTextYValue.Location = new System.Drawing.Point(151, 59);
            this.baseTextYValue.Name = "baseTextYValue";
            this.baseTextYValue.Size = new System.Drawing.Size(80, 17);
            this.baseTextYValue.TabIndex = 15;
            this.baseTextYValue.Text = "0";
            this.baseTextYValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // baseTextXValue
            // 
            this.baseTextXValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextXValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextXValue.Font = new System.Drawing.Font("Tahoma", 11F);
            this.baseTextXValue.ForeColor = System.Drawing.Color.White;
            this.baseTextXValue.Location = new System.Drawing.Point(151, 29);
            this.baseTextXValue.Name = "baseTextXValue";
            this.baseTextXValue.Size = new System.Drawing.Size(80, 17);
            this.baseTextXValue.TabIndex = 14;
            this.baseTextXValue.Text = "0";
            this.baseTextXValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // baseTextXAxisValue
            // 
            this.baseTextXAxisValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextXAxisValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextXAxisValue.Font = new System.Drawing.Font("Tahoma", 11F);
            this.baseTextXAxisValue.ForeColor = System.Drawing.Color.White;
            this.baseTextXAxisValue.Location = new System.Drawing.Point(151, 88);
            this.baseTextXAxisValue.Name = "baseTextXAxisValue";
            this.baseTextXAxisValue.Size = new System.Drawing.Size(80, 17);
            this.baseTextXAxisValue.TabIndex = 13;
            this.baseTextXAxisValue.Text = "0";
            this.baseTextXAxisValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // baseTextYAxisValue
            // 
            this.baseTextYAxisValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextYAxisValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextYAxisValue.Font = new System.Drawing.Font("Tahoma", 11F);
            this.baseTextYAxisValue.ForeColor = System.Drawing.Color.White;
            this.baseTextYAxisValue.Location = new System.Drawing.Point(151, 119);
            this.baseTextYAxisValue.Name = "baseTextYAxisValue";
            this.baseTextYAxisValue.Size = new System.Drawing.Size(80, 17);
            this.baseTextYAxisValue.TabIndex = 12;
            this.baseTextYAxisValue.Text = "0";
            this.baseTextYAxisValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // checkBoxEnable
            // 
            this.checkBoxEnable.AutoSize = true;
            this.checkBoxEnable.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.checkBoxEnable.Location = new System.Drawing.Point(250, 92);
            this.checkBoxEnable.Name = "checkBoxEnable";
            this.checkBoxEnable.Size = new System.Drawing.Size(70, 19);
            this.checkBoxEnable.TabIndex = 11;
            this.checkBoxEnable.Text = "Enable";
            this.checkBoxEnable.UseVisualStyleBackColor = true;
            this.checkBoxEnable.CheckedChanged += new System.EventHandler(this.checkBoxEnable_CheckedChanged);
            // 
            // checkBoxYInverted
            // 
            this.checkBoxYInverted.AutoSize = true;
            this.checkBoxYInverted.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.checkBoxYInverted.Location = new System.Drawing.Point(250, 62);
            this.checkBoxYInverted.Name = "checkBoxYInverted";
            this.checkBoxYInverted.Size = new System.Drawing.Size(85, 19);
            this.checkBoxYInverted.TabIndex = 10;
            this.checkBoxYInverted.Text = "InvertedY";
            this.checkBoxYInverted.UseVisualStyleBackColor = true;
            this.checkBoxYInverted.CheckedChanged += new System.EventHandler(this.checkBoxYInverted_CheckedChanged);
            // 
            // checkBoxXInverted
            // 
            this.checkBoxXInverted.AutoSize = true;
            this.checkBoxXInverted.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.checkBoxXInverted.Location = new System.Drawing.Point(250, 32);
            this.checkBoxXInverted.Name = "checkBoxXInverted";
            this.checkBoxXInverted.Size = new System.Drawing.Size(86, 19);
            this.checkBoxXInverted.TabIndex = 9;
            this.checkBoxXInverted.Text = "InvertedX";
            this.checkBoxXInverted.UseVisualStyleBackColor = true;
            this.checkBoxXInverted.CheckedChanged += new System.EventHandler(this.checkBoxXInverted_CheckedChanged);
            // 
            // baseLabelYAxis
            // 
            this.baseLabelYAxis.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.baseLabelYAxis.ForeColor = System.Drawing.Color.White;
            this.baseLabelYAxis.Location = new System.Drawing.Point(17, 119);
            this.baseLabelYAxis.Name = "baseLabelYAxis";
            this.baseLabelYAxis.Size = new System.Drawing.Size(120, 16);
            this.baseLabelYAxis.TabIndex = 3;
            this.baseLabelYAxis.Text = "Y Axis T [degree] :";
            this.baseLabelYAxis.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // baseLabelXAxis
            // 
            this.baseLabelXAxis.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.baseLabelXAxis.ForeColor = System.Drawing.Color.White;
            this.baseLabelXAxis.Location = new System.Drawing.Point(17, 89);
            this.baseLabelXAxis.Name = "baseLabelXAxis";
            this.baseLabelXAxis.Size = new System.Drawing.Size(120, 16);
            this.baseLabelXAxis.TabIndex = 2;
            this.baseLabelXAxis.Text = "X Axis T [degree] :";
            this.baseLabelXAxis.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // baseLabelY
            // 
            this.baseLabelY.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.baseLabelY.ForeColor = System.Drawing.Color.White;
            this.baseLabelY.Location = new System.Drawing.Point(18, 59);
            this.baseLabelY.Name = "baseLabelY";
            this.baseLabelY.Size = new System.Drawing.Size(120, 16);
            this.baseLabelY.TabIndex = 1;
            this.baseLabelY.Text = "Y [mm/pixel] :";
            this.baseLabelY.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // baseLabelX
            // 
            this.baseLabelX.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.baseLabelX.ForeColor = System.Drawing.Color.White;
            this.baseLabelX.Location = new System.Drawing.Point(17, 29);
            this.baseLabelX.Name = "baseLabelX";
            this.baseLabelX.Size = new System.Drawing.Size(120, 23);
            this.baseLabelX.TabIndex = 0;
            this.baseLabelX.Text = "X [mm/pixel] :";
            this.baseLabelX.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // baseGroupBox1
            // 
            this.baseGroupBox1.ForeColor = System.Drawing.Color.White;
            this.baseGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.baseGroupBox1.Name = "baseGroupBox1";
            this.baseGroupBox1.Size = new System.Drawing.Size(393, 313);
            this.baseGroupBox1.TabIndex = 12;
            this.baseGroupBox1.TabStop = false;
            this.baseGroupBox1.Text = " Vision Calibrator ";
            // 
            // VisionCalScaleControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Movedistance);
            this.Controls.Add(this.CalibratorPositionGrid);
            this.Controls.Add(this.baseLabelMoveDistance);
            this.Controls.Add(this.baseGroupBoxScale);
            this.Controls.Add(this.baseGroupBox1);
            this.Name = "VisionCalScaleControl";
            this.Size = new System.Drawing.Size(400, 320);
            ((System.ComponentModel.ISupportInitialize)(this.CalibratorPositionGrid)).EndInit();
            this.baseGroupBoxScale.ResumeLayout(false);
            this.baseGroupBoxScale.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BaseGroupBox baseGroupBoxScale;
        private System.Windows.Forms.CheckBox checkBoxEnable;
        private System.Windows.Forms.CheckBox checkBoxYInverted;
        private BaseLabel baseLabelYAxis;
        private BaseLabel baseLabelXAxis;
        private BaseLabel baseLabelY;
        private BaseLabel baseLabelX;
        private BaseLabel baseLabelMoveDistance;
        private BaseDataGridView CalibratorPositionGrid;
        private BaseTextBox Movedistance;
        private BaseGroupBox baseGroupBox1;
        private BaseTextBox baseTextYAxisValue;
        private BaseTextBox baseTextYValue;
        private BaseTextBox baseTextXValue;
        private BaseTextBox baseTextXAxisValue;
        private System.Windows.Forms.CheckBox checkBoxXInverted;
    }
}
