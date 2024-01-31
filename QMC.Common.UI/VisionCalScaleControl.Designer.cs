namespace QMC.Common.UI
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.Movedistance = new BaseTextBox();
            this.CalibratorPositionGrid = new BaseDataGridView();
            this.baseLabelMoveDistance = new BaseLabel();
            this.baseGroupBoxScale = new BaseGroupBox();
            this.checkBoxEnable = new System.Windows.Forms.CheckBox();
            this.checkBoxYInverted = new System.Windows.Forms.CheckBox();
            this.checkBoxXInverted = new System.Windows.Forms.CheckBox();
            this.baseLabelYAxisValue = new BaseLabel();
            this.baseLabelXAxisValue = new BaseLabel();
            this.baseLabelYValue = new BaseLabel();
            this.baseLabelXValue = new BaseLabel();
            this.baseLabelYAxis = new BaseLabel();
            this.baseLabelXAxis = new BaseLabel();
            this.baseLabelY = new BaseLabel();
            this.baseLabelX = new BaseLabel();
            this.baseGroupBox1 = new BaseGroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.CalibratorPositionGrid)).BeginInit();
            this.baseGroupBoxScale.SuspendLayout();
            this.SuspendLayout();
            // 
            // Movedistance
            // 
            this.Movedistance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.Movedistance.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Movedistance.Font = new System.Drawing.Font("굴림", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Movedistance.ForeColor = System.Drawing.Color.White;
            this.Movedistance.Location = new System.Drawing.Point(211, 172);
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
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.CalibratorPositionGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.CalibratorPositionGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.CalibratorPositionGrid.DefaultCellStyle = dataGridViewCellStyle2;
            this.CalibratorPositionGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            this.CalibratorPositionGrid.Location = new System.Drawing.Point(3, 199);
            this.CalibratorPositionGrid.Name = "CalibratorPositionGrid";
            this.CalibratorPositionGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.CalibratorPositionGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.CalibratorPositionGrid.RowHeadersVisible = false;
            this.CalibratorPositionGrid.RowTemplate.Height = 23;
            this.CalibratorPositionGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.CalibratorPositionGrid.Size = new System.Drawing.Size(373, 102);
            this.CalibratorPositionGrid.TabIndex = 10;
            // 
            // baseLabelMoveDistance
            // 
            this.baseLabelMoveDistance.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelMoveDistance.ForeColor = System.Drawing.Color.White;
            this.baseLabelMoveDistance.Location = new System.Drawing.Point(5, 171);
            this.baseLabelMoveDistance.Name = "baseLabelMoveDistance";
            this.baseLabelMoveDistance.Size = new System.Drawing.Size(200, 16);
            this.baseLabelMoveDistance.TabIndex = 8;
            this.baseLabelMoveDistance.Text = "Move Distance [mm] :";
            this.baseLabelMoveDistance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // baseGroupBoxScale
            // 
            this.baseGroupBoxScale.Controls.Add(this.checkBoxEnable);
            this.baseGroupBoxScale.Controls.Add(this.checkBoxYInverted);
            this.baseGroupBoxScale.Controls.Add(this.checkBoxXInverted);
            this.baseGroupBoxScale.Controls.Add(this.baseLabelYAxisValue);
            this.baseGroupBoxScale.Controls.Add(this.baseLabelXAxisValue);
            this.baseGroupBoxScale.Controls.Add(this.baseLabelYValue);
            this.baseGroupBoxScale.Controls.Add(this.baseLabelXValue);
            this.baseGroupBoxScale.Controls.Add(this.baseLabelYAxis);
            this.baseGroupBoxScale.Controls.Add(this.baseLabelXAxis);
            this.baseGroupBoxScale.Controls.Add(this.baseLabelY);
            this.baseGroupBoxScale.Controls.Add(this.baseLabelX);
            this.baseGroupBoxScale.ForeColor = System.Drawing.Color.White;
            this.baseGroupBoxScale.Location = new System.Drawing.Point(5, 16);
            this.baseGroupBoxScale.Name = "baseGroupBoxScale";
            this.baseGroupBoxScale.Size = new System.Drawing.Size(370, 150);
            this.baseGroupBoxScale.TabIndex = 0;
            this.baseGroupBoxScale.TabStop = false;
            this.baseGroupBoxScale.Text = "Scale";
            // 
            // checkBoxEnable
            // 
            this.checkBoxEnable.AutoSize = true;
            this.checkBoxEnable.Font = new System.Drawing.Font("굴림", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
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
            this.checkBoxYInverted.Font = new System.Drawing.Font("굴림", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
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
            this.checkBoxXInverted.Font = new System.Drawing.Font("굴림", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.checkBoxXInverted.Location = new System.Drawing.Point(250, 32);
            this.checkBoxXInverted.Name = "checkBoxXInverted";
            this.checkBoxXInverted.Size = new System.Drawing.Size(86, 19);
            this.checkBoxXInverted.TabIndex = 9;
            this.checkBoxXInverted.Text = "InvertedX";
            this.checkBoxXInverted.UseVisualStyleBackColor = true;
            this.checkBoxXInverted.CheckedChanged += new System.EventHandler(this.checkBoxXInverted_CheckedChanged);
            // 
            // baseLabelYAxisValue
            // 
            this.baseLabelYAxisValue.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelYAxisValue.ForeColor = System.Drawing.Color.White;
            this.baseLabelYAxisValue.Location = new System.Drawing.Point(151, 119);
            this.baseLabelYAxisValue.Name = "baseLabelYAxisValue";
            this.baseLabelYAxisValue.Size = new System.Drawing.Size(80, 23);
            this.baseLabelYAxisValue.TabIndex = 7;
            this.baseLabelYAxisValue.Text = "0";
            this.baseLabelYAxisValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // baseLabelXAxisValue
            // 
            this.baseLabelXAxisValue.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelXAxisValue.ForeColor = System.Drawing.Color.White;
            this.baseLabelXAxisValue.Location = new System.Drawing.Point(151, 89);
            this.baseLabelXAxisValue.Name = "baseLabelXAxisValue";
            this.baseLabelXAxisValue.Size = new System.Drawing.Size(80, 23);
            this.baseLabelXAxisValue.TabIndex = 6;
            this.baseLabelXAxisValue.Text = "0";
            this.baseLabelXAxisValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // baseLabelYValue
            // 
            this.baseLabelYValue.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelYValue.ForeColor = System.Drawing.Color.White;
            this.baseLabelYValue.Location = new System.Drawing.Point(151, 59);
            this.baseLabelYValue.Name = "baseLabelYValue";
            this.baseLabelYValue.Size = new System.Drawing.Size(80, 23);
            this.baseLabelYValue.TabIndex = 5;
            this.baseLabelYValue.Text = "0";
            this.baseLabelYValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // baseLabelXValue
            // 
            this.baseLabelXValue.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelXValue.ForeColor = System.Drawing.Color.White;
            this.baseLabelXValue.Location = new System.Drawing.Point(151, 29);
            this.baseLabelXValue.Name = "baseLabelXValue";
            this.baseLabelXValue.Size = new System.Drawing.Size(80, 23);
            this.baseLabelXValue.TabIndex = 4;
            this.baseLabelXValue.Text = "0";
            this.baseLabelXValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // baseLabelYAxis
            // 
            this.baseLabelYAxis.Font = new System.Drawing.Font("돋움", 8F, System.Drawing.FontStyle.Bold);
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
            this.baseLabelXAxis.Font = new System.Drawing.Font("돋움", 8F, System.Drawing.FontStyle.Bold);
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
            this.baseLabelY.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
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
            this.baseLabelX.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
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
            this.baseGroupBox1.Size = new System.Drawing.Size(380, 305);
            this.baseGroupBox1.TabIndex = 12;
            this.baseGroupBox1.TabStop = false;
            this.baseGroupBox1.Text = "Vision Calibrator";
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
            this.Size = new System.Drawing.Size(380, 305);
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
        private System.Windows.Forms.CheckBox checkBoxXInverted;
        private BaseLabel baseLabelYAxisValue;
        private BaseLabel baseLabelXAxisValue;
        private BaseLabel baseLabelYValue;
        private BaseLabel baseLabelXValue;
        private BaseLabel baseLabelYAxis;
        private BaseLabel baseLabelXAxis;
        private BaseLabel baseLabelY;
        private BaseLabel baseLabelX;
        private BaseLabel baseLabelMoveDistance;
        private BaseDataGridView CalibratorPositionGrid;
        private BaseTextBox Movedistance;
        private BaseGroupBox baseGroupBox1;
    }
}
