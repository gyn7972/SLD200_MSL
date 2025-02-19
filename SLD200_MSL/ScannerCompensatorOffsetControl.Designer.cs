namespace SLD200_MSL
{
    partial class ScannerCompensatorOffsetControl
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
            this.baseGroupBox1 = new SLD200_MSL.BaseGroupBox();
            this.OffsetGrid = new SLD200_MSL.BaseDataGridView();
            this.baseButtonLoad = new SLD200_MSL.BaseButton();
            this.baseButtonSave = new SLD200_MSL.BaseButton();
            this.baseGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OffsetGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // baseGroupBox1
            // 
            this.baseGroupBox1.Controls.Add(this.OffsetGrid);
            this.baseGroupBox1.Controls.Add(this.baseButtonLoad);
            this.baseGroupBox1.Controls.Add(this.baseButtonSave);
            this.baseGroupBox1.ForeColor = System.Drawing.Color.White;
            this.baseGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.baseGroupBox1.Name = "baseGroupBox1";
            this.baseGroupBox1.Size = new System.Drawing.Size(300, 380);
            this.baseGroupBox1.TabIndex = 1;
            this.baseGroupBox1.TabStop = false;
            this.baseGroupBox1.Text = " Compensator Offset ";
            // 
            // OffsetGrid
            // 
            this.OffsetGrid.AllowUserToAddRows = false;
            this.OffsetGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.OffsetGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.OffsetGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.OffsetGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.OffsetGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.OffsetGrid.DefaultCellStyle = dataGridViewCellStyle2;
            this.OffsetGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            this.OffsetGrid.Location = new System.Drawing.Point(6, 56);
            this.OffsetGrid.Name = "OffsetGrid";
            this.OffsetGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.OffsetGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.OffsetGrid.RowHeadersVisible = false;
            this.OffsetGrid.RowTemplate.Height = 23;
            this.OffsetGrid.Size = new System.Drawing.Size(288, 318);
            this.OffsetGrid.TabIndex = 2;
            // 
            // baseButtonLoad
            // 
            this.baseButtonLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonLoad.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonLoad.Location = new System.Drawing.Point(194, 20);
            this.baseButtonLoad.Name = "baseButtonLoad";
            this.baseButtonLoad.Size = new System.Drawing.Size(100, 30);
            this.baseButtonLoad.TabIndex = 1;
            this.baseButtonLoad.Text = "Load";
            this.baseButtonLoad.UseVisualStyleBackColor = false;
            // 
            // baseButtonSave
            // 
            this.baseButtonSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonSave.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonSave.Location = new System.Drawing.Point(88, 20);
            this.baseButtonSave.Name = "baseButtonSave";
            this.baseButtonSave.Size = new System.Drawing.Size(100, 30);
            this.baseButtonSave.TabIndex = 0;
            this.baseButtonSave.Text = "Save";
            this.baseButtonSave.UseVisualStyleBackColor = false;
            // 
            // ScannerCompensatorOffsetControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseGroupBox1);
            this.Name = "ScannerCompensatorOffsetControl";
            this.Size = new System.Drawing.Size(300, 381);
            this.baseGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.OffsetGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private SLD200_MSL.BaseGroupBox baseGroupBox1;
        private SLD200_MSL.BaseDataGridView OffsetGrid;
        private SLD200_MSL.BaseButton baseButtonLoad;
        private SLD200_MSL.BaseButton baseButtonSave;
    }
}
