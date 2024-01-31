namespace QMC.Common.UI
{
    partial class IlluminatorRecipeControl
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
            this.baseGroupBox1 = new QMC.Common.UI.BaseGroupBox();
            this.baseDataGridViewIlluminatorRecipe = new QMC.Common.UI.BaseDataGridView();
            this.baseGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.baseDataGridViewIlluminatorRecipe)).BeginInit();
            this.SuspendLayout();
            // 
            // baseGroupBox1
            // 
            this.baseGroupBox1.Controls.Add(this.baseDataGridViewIlluminatorRecipe);
            this.baseGroupBox1.ForeColor = System.Drawing.Color.White;
            this.baseGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.baseGroupBox1.Name = "baseGroupBox1";
            this.baseGroupBox1.Size = new System.Drawing.Size(290, 540);
            this.baseGroupBox1.TabIndex = 0;
            this.baseGroupBox1.TabStop = false;
            this.baseGroupBox1.Text = "Illuminator Recipe";
            // 
            // baseDataGridViewIlluminatorRecipe
            // 
            this.baseDataGridViewIlluminatorRecipe.AllowUserToAddRows = false;
            this.baseDataGridViewIlluminatorRecipe.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.baseDataGridViewIlluminatorRecipe.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseDataGridViewIlluminatorRecipe.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.baseDataGridViewIlluminatorRecipe.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.baseDataGridViewIlluminatorRecipe.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.baseDataGridViewIlluminatorRecipe.DefaultCellStyle = dataGridViewCellStyle2;
            this.baseDataGridViewIlluminatorRecipe.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            this.baseDataGridViewIlluminatorRecipe.Location = new System.Drawing.Point(7, 15);
            this.baseDataGridViewIlluminatorRecipe.Name = "baseDataGridViewIlluminatorRecipe";
            this.baseDataGridViewIlluminatorRecipe.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.baseDataGridViewIlluminatorRecipe.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.baseDataGridViewIlluminatorRecipe.RowHeadersVisible = false;
            this.baseDataGridViewIlluminatorRecipe.RowHeadersWidth = 51;
            this.baseDataGridViewIlluminatorRecipe.RowTemplate.Height = 23;
            this.baseDataGridViewIlluminatorRecipe.Size = new System.Drawing.Size(275, 520);
            this.baseDataGridViewIlluminatorRecipe.TabIndex = 0;
            // 
            // IlluminatorRecipeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseGroupBox1);
            this.Name = "IlluminatorRecipeControl";
            this.Size = new System.Drawing.Size(290, 540);
            this.baseGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.baseDataGridViewIlluminatorRecipe)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BaseGroupBox baseGroupBox1;
        private BaseDataGridView baseDataGridViewIlluminatorRecipe;
    }
}
