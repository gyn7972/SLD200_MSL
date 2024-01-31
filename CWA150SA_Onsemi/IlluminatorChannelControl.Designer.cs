namespace CWA150SA_Onsemi300
{
    partial class IlluminatorChannelControl
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
            this.baseGroupBoxIlluminator = new CWA150SA_Onsemi300.BaseGroupBox();
            this.baseButtonClear = new CWA150SA_Onsemi300.BaseButton();
            this.baseButtonRemove = new CWA150SA_Onsemi300.BaseButton();
            this.baseButtonAdd = new CWA150SA_Onsemi300.BaseButton();
            this.baseDataGridViewIlluminator = new CWA150SA_Onsemi300.BaseDataGridView();
            this.baseGroupBoxIlluminator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.baseDataGridViewIlluminator)).BeginInit();
            this.SuspendLayout();
            // 
            // baseGroupBoxIlluminator
            // 
            this.baseGroupBoxIlluminator.Controls.Add(this.baseButtonClear);
            this.baseGroupBoxIlluminator.Controls.Add(this.baseButtonRemove);
            this.baseGroupBoxIlluminator.Controls.Add(this.baseButtonAdd);
            this.baseGroupBoxIlluminator.Controls.Add(this.baseDataGridViewIlluminator);
            this.baseGroupBoxIlluminator.ForeColor = System.Drawing.Color.White;
            this.baseGroupBoxIlluminator.Location = new System.Drawing.Point(0, 0);
            this.baseGroupBoxIlluminator.Name = "baseGroupBoxIlluminator";
            this.baseGroupBoxIlluminator.Size = new System.Drawing.Size(295, 280);
            this.baseGroupBoxIlluminator.TabIndex = 0;
            this.baseGroupBoxIlluminator.TabStop = false;
            this.baseGroupBoxIlluminator.Text = " Illuminator Channel ";
            // 
            // baseButtonClear
            // 
            this.baseButtonClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonClear.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonClear.Location = new System.Drawing.Point(189, 100);
            this.baseButtonClear.Name = "baseButtonClear";
            this.baseButtonClear.Size = new System.Drawing.Size(100, 30);
            this.baseButtonClear.TabIndex = 3;
            this.baseButtonClear.Text = "Clear";
            this.baseButtonClear.UseVisualStyleBackColor = false;
            this.baseButtonClear.Click += new System.EventHandler(this.baseButtonClear_Click);
            // 
            // baseButtonRemove
            // 
            this.baseButtonRemove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonRemove.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonRemove.Location = new System.Drawing.Point(189, 60);
            this.baseButtonRemove.Name = "baseButtonRemove";
            this.baseButtonRemove.Size = new System.Drawing.Size(100, 30);
            this.baseButtonRemove.TabIndex = 2;
            this.baseButtonRemove.Text = "Remove";
            this.baseButtonRemove.UseVisualStyleBackColor = false;
            this.baseButtonRemove.Click += new System.EventHandler(this.baseButtonRemove_Click);
            // 
            // baseButtonAdd
            // 
            this.baseButtonAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonAdd.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonAdd.Location = new System.Drawing.Point(189, 20);
            this.baseButtonAdd.Name = "baseButtonAdd";
            this.baseButtonAdd.Size = new System.Drawing.Size(100, 30);
            this.baseButtonAdd.TabIndex = 1;
            this.baseButtonAdd.Text = "Add";
            this.baseButtonAdd.UseVisualStyleBackColor = false;
            this.baseButtonAdd.Click += new System.EventHandler(this.baseButtonAdd_Click);
            // 
            // baseDataGridViewIlluminator
            // 
            this.baseDataGridViewIlluminator.AllowUserToAddRows = false;
            this.baseDataGridViewIlluminator.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.baseDataGridViewIlluminator.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseDataGridViewIlluminator.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.baseDataGridViewIlluminator.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.baseDataGridViewIlluminator.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.baseDataGridViewIlluminator.DefaultCellStyle = dataGridViewCellStyle2;
            this.baseDataGridViewIlluminator.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            this.baseDataGridViewIlluminator.Location = new System.Drawing.Point(4, 20);
            this.baseDataGridViewIlluminator.Name = "baseDataGridViewIlluminator";
            this.baseDataGridViewIlluminator.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.baseDataGridViewIlluminator.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.baseDataGridViewIlluminator.RowHeadersVisible = false;
            this.baseDataGridViewIlluminator.RowHeadersWidth = 51;
            this.baseDataGridViewIlluminator.RowTemplate.Height = 23;
            this.baseDataGridViewIlluminator.Size = new System.Drawing.Size(179, 250);
            this.baseDataGridViewIlluminator.TabIndex = 0;
            // 
            // IlluminatorChannelControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseGroupBoxIlluminator);
            this.Name = "IlluminatorChannelControl";
            this.Size = new System.Drawing.Size(298, 280);
            this.baseGroupBoxIlluminator.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.baseDataGridViewIlluminator)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BaseGroupBox baseGroupBoxIlluminator;
        private BaseButton baseButtonClear;
        private BaseButton baseButtonRemove;
        private BaseButton baseButtonAdd;
        private BaseDataGridView baseDataGridViewIlluminator;
    }
}
