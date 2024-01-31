namespace CWA150SA_Onsemi300
{
    partial class IlluminatorControl
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
            this.baseLabel1Value = new CWA150SA_Onsemi300.BaseLabel();
            this.baseGroupBoxIlluminatorControl = new CWA150SA_Onsemi300.BaseGroupBox();
            this.baseLabelMax = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelMin = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelIlluminator = new CWA150SA_Onsemi300.BaseLabel();
            this.comboBoxIllumanatorList = new System.Windows.Forms.ComboBox();
            this.hScrollBarIlluminator = new System.Windows.Forms.HScrollBar();
            this.baseButtonAllOff = new CWA150SA_Onsemi300.BaseButton();
            this.IlluminatorGrid = new CWA150SA_Onsemi300.BaseDataGridView();
            this.baseButtonSave = new CWA150SA_Onsemi300.BaseButton();
            this.baseGroupBoxIlluminatorControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IlluminatorGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // baseLabel1Value
            // 
            this.baseLabel1Value.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabel1Value.ForeColor = System.Drawing.Color.White;
            this.baseLabel1Value.Location = new System.Drawing.Point(26, 171);
            this.baseLabel1Value.Name = "baseLabel1Value";
            this.baseLabel1Value.Size = new System.Drawing.Size(223, 23);
            this.baseLabel1Value.TabIndex = 1;
            this.baseLabel1Value.Text = "0";
            this.baseLabel1Value.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseGroupBoxIlluminatorControl
            // 
            this.baseGroupBoxIlluminatorControl.Controls.Add(this.baseLabelMax);
            this.baseGroupBoxIlluminatorControl.Controls.Add(this.baseLabelMin);
            this.baseGroupBoxIlluminatorControl.Controls.Add(this.baseLabelIlluminator);
            this.baseGroupBoxIlluminatorControl.Controls.Add(this.comboBoxIllumanatorList);
            this.baseGroupBoxIlluminatorControl.Controls.Add(this.hScrollBarIlluminator);
            this.baseGroupBoxIlluminatorControl.Controls.Add(this.baseButtonAllOff);
            this.baseGroupBoxIlluminatorControl.Controls.Add(this.IlluminatorGrid);
            this.baseGroupBoxIlluminatorControl.Controls.Add(this.baseButtonSave);
            this.baseGroupBoxIlluminatorControl.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseGroupBoxIlluminatorControl.ForeColor = System.Drawing.Color.White;
            this.baseGroupBoxIlluminatorControl.Location = new System.Drawing.Point(0, 0);
            this.baseGroupBoxIlluminatorControl.Name = "baseGroupBoxIlluminatorControl";
            this.baseGroupBoxIlluminatorControl.Size = new System.Drawing.Size(270, 300);
            this.baseGroupBoxIlluminatorControl.TabIndex = 5;
            this.baseGroupBoxIlluminatorControl.TabStop = false;
            this.baseGroupBoxIlluminatorControl.Text = " [ Illuminator ] ";
            // 
            // baseLabelMax
            // 
            this.baseLabelMax.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.baseLabelMax.ForeColor = System.Drawing.Color.White;
            this.baseLabelMax.Location = new System.Drawing.Point(210, 225);
            this.baseLabelMax.Name = "baseLabelMax";
            this.baseLabelMax.Size = new System.Drawing.Size(55, 23);
            this.baseLabelMax.TabIndex = 8;
            this.baseLabelMax.Text = "255";
            this.baseLabelMax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // baseLabelMin
            // 
            this.baseLabelMin.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.baseLabelMin.ForeColor = System.Drawing.Color.White;
            this.baseLabelMin.Location = new System.Drawing.Point(16, 227);
            this.baseLabelMin.Name = "baseLabelMin";
            this.baseLabelMin.Size = new System.Drawing.Size(22, 23);
            this.baseLabelMin.TabIndex = 7;
            this.baseLabelMin.Text = "0";
            // 
            // baseLabelIlluminator
            // 
            this.baseLabelIlluminator.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.baseLabelIlluminator.ForeColor = System.Drawing.Color.White;
            this.baseLabelIlluminator.Location = new System.Drawing.Point(6, 146);
            this.baseLabelIlluminator.Name = "baseLabelIlluminator";
            this.baseLabelIlluminator.Size = new System.Drawing.Size(99, 23);
            this.baseLabelIlluminator.TabIndex = 6;
            this.baseLabelIlluminator.Text = "Illuminator :";
            // 
            // comboBoxIllumanatorList
            // 
            this.comboBoxIllumanatorList.FormattingEnabled = true;
            this.comboBoxIllumanatorList.Location = new System.Drawing.Point(111, 143);
            this.comboBoxIllumanatorList.Name = "comboBoxIllumanatorList";
            this.comboBoxIllumanatorList.Size = new System.Drawing.Size(154, 27);
            this.comboBoxIllumanatorList.TabIndex = 2;
            this.comboBoxIllumanatorList.SelectedIndexChanged += new System.EventHandler(this.comboBoxIllumanatorList_SelectedIndexChanged);
            // 
            // hScrollBarIlluminator
            // 
            this.hScrollBarIlluminator.Location = new System.Drawing.Point(8, 197);
            this.hScrollBarIlluminator.Name = "hScrollBarIlluminator";
            this.hScrollBarIlluminator.Size = new System.Drawing.Size(255, 28);
            this.hScrollBarIlluminator.TabIndex = 5;
            // 
            // baseButtonAllOff
            // 
            this.baseButtonAllOff.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonAllOff.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonAllOff.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonAllOff.Location = new System.Drawing.Point(161, 257);
            this.baseButtonAllOff.Name = "baseButtonAllOff";
            this.baseButtonAllOff.Size = new System.Drawing.Size(100, 35);
            this.baseButtonAllOff.TabIndex = 4;
            this.baseButtonAllOff.Text = "All Off";
            this.baseButtonAllOff.UseVisualStyleBackColor = false;
            this.baseButtonAllOff.Click += new System.EventHandler(this.baseButtonAllOff_Click);
            // 
            // IlluminatorGrid
            // 
            this.IlluminatorGrid.AllowUserToAddRows = false;
            this.IlluminatorGrid.AllowUserToResizeColumns = false;
            this.IlluminatorGrid.AllowUserToResizeRows = false;
            this.IlluminatorGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.IlluminatorGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.IlluminatorGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.IlluminatorGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.IlluminatorGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.IlluminatorGrid.DefaultCellStyle = dataGridViewCellStyle2;
            this.IlluminatorGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            this.IlluminatorGrid.Location = new System.Drawing.Point(5, 22);
            this.IlluminatorGrid.Name = "IlluminatorGrid";
            this.IlluminatorGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.IlluminatorGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.IlluminatorGrid.RowHeadersVisible = false;
            this.IlluminatorGrid.RowHeadersWidth = 51;
            this.IlluminatorGrid.RowTemplate.Height = 23;
            this.IlluminatorGrid.Size = new System.Drawing.Size(260, 117);
            this.IlluminatorGrid.TabIndex = 0;
            this.IlluminatorGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.IlluminatorGrid_CellClick);
            // 
            // baseButtonSave
            // 
            this.baseButtonSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonSave.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonSave.Location = new System.Drawing.Point(9, 257);
            this.baseButtonSave.Name = "baseButtonSave";
            this.baseButtonSave.Size = new System.Drawing.Size(100, 35);
            this.baseButtonSave.TabIndex = 3;
            this.baseButtonSave.Text = "Save";
            this.baseButtonSave.UseVisualStyleBackColor = false;
            this.baseButtonSave.Click += new System.EventHandler(this.baseButtonSave_Click);
            // 
            // IlluminatorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseLabel1Value);
            this.Controls.Add(this.baseGroupBoxIlluminatorControl);
            this.Name = "IlluminatorControl";
            this.Size = new System.Drawing.Size(270, 305);
            this.baseGroupBoxIlluminatorControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.IlluminatorGrid)).EndInit();
            this.ResumeLayout(false);

        }


        #endregion

        private BaseDataGridView IlluminatorGrid;
        private BaseLabel baseLabel1Value;
        private System.Windows.Forms.ComboBox comboBoxIllumanatorList;
        private BaseButton baseButtonSave;
        private BaseButton baseButtonAllOff;
        private BaseGroupBox baseGroupBoxIlluminatorControl;
        private BaseLabel baseLabelIlluminator;
        private System.Windows.Forms.HScrollBar hScrollBarIlluminator;
        private BaseLabel baseLabelMax;
        private BaseLabel baseLabelMin;
    }
}
