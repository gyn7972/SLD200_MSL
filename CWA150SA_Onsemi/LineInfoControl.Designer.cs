namespace CWA150SA_Onsemi300
{ 
    partial class LineInfoControl
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
            this.baseGroupBoxMain = new BaseGroupBox();
            this.checkBoxUseMesInterlock = new System.Windows.Forms.CheckBox();
            this.baseDataGridViewLineInfo = new BaseDataGridView();
            this.baseButtonWPseg = new BaseButton();
            this.baseGroupBoxMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.baseDataGridViewLineInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // baseGroupBoxMain
            // 
            this.baseGroupBoxMain.Controls.Add(this.checkBoxUseMesInterlock);
            this.baseGroupBoxMain.Controls.Add(this.baseDataGridViewLineInfo);
            this.baseGroupBoxMain.Controls.Add(this.baseButtonWPseg);
            this.baseGroupBoxMain.ForeColor = System.Drawing.Color.White;
            this.baseGroupBoxMain.Location = new System.Drawing.Point(0, 0);
            this.baseGroupBoxMain.Name = "baseGroupBoxMain";
            this.baseGroupBoxMain.Size = new System.Drawing.Size(215, 549);
            this.baseGroupBoxMain.TabIndex = 0;
            this.baseGroupBoxMain.TabStop = false;
            this.baseGroupBoxMain.Text = "Line Info";
            // 
            // checkBoxUseMesInterlock
            // 
            this.checkBoxUseMesInterlock.AutoSize = true;
            this.checkBoxUseMesInterlock.Location = new System.Drawing.Point(6, 498);
            this.checkBoxUseMesInterlock.Name = "checkBoxUseMesInterlock";
            this.checkBoxUseMesInterlock.Size = new System.Drawing.Size(128, 16);
            this.checkBoxUseMesInterlock.TabIndex = 2;
            this.checkBoxUseMesInterlock.Text = "Use MES Interlock";
            this.checkBoxUseMesInterlock.UseVisualStyleBackColor = true;
            this.checkBoxUseMesInterlock.CheckedChanged += new System.EventHandler(this.checkBoxUseMesInterlock_CheckedChanged);
            // 
            // baseDataGridViewLineInfo
            // 
            this.baseDataGridViewLineInfo.AllowUserToAddRows = false;
            this.baseDataGridViewLineInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.baseDataGridViewLineInfo.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseDataGridViewLineInfo.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.baseDataGridViewLineInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.baseDataGridViewLineInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.baseDataGridViewLineInfo.DefaultCellStyle = dataGridViewCellStyle2;
            this.baseDataGridViewLineInfo.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            this.baseDataGridViewLineInfo.Location = new System.Drawing.Point(6, 56);
            this.baseDataGridViewLineInfo.Name = "baseDataGridViewLineInfo";
            this.baseDataGridViewLineInfo.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.baseDataGridViewLineInfo.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.baseDataGridViewLineInfo.RowHeadersVisible = false;
            this.baseDataGridViewLineInfo.RowTemplate.Height = 23;
            this.baseDataGridViewLineInfo.Size = new System.Drawing.Size(203, 433);
            this.baseDataGridViewLineInfo.TabIndex = 1;
            // 
            // baseButtonWPseg
            // 
            this.baseButtonWPseg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonWPseg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonWPseg.ForeColor = System.Drawing.SystemColors.Window;
            this.baseButtonWPseg.Location = new System.Drawing.Point(109, 20);
            this.baseButtonWPseg.Name = "baseButtonWPseg";
            this.baseButtonWPseg.Size = new System.Drawing.Size(100, 30);
            this.baseButtonWPseg.TabIndex = 0;
            this.baseButtonWPseg.Text = "Set LineInfo";
            this.baseButtonWPseg.UseVisualStyleBackColor = false;
            this.baseButtonWPseg.Click += new System.EventHandler(this.baseButtonWPseg_Click);
            // 
            // LineInfoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseGroupBoxMain);
            this.Name = "LineInfoControl";
            this.Size = new System.Drawing.Size(215, 549);
            this.baseGroupBoxMain.ResumeLayout(false);
            this.baseGroupBoxMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.baseDataGridViewLineInfo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BaseGroupBox baseGroupBoxMain;
        private BaseButton baseButtonWPseg;
        private BaseDataGridView baseDataGridViewLineInfo;
        private System.Windows.Forms.CheckBox checkBoxUseMesInterlock;
    }
}
