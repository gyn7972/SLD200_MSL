namespace QMC.Vision
{
    partial class _2DMappingDataControl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.baseGroupBox1 = new QMC.Common.UI.BaseGroupBox();
            this.baseButtonSave = new QMC.Common.UI.BaseButton();
            this.baseButtonLoad = new QMC.Common.UI.BaseButton();
            this.baseButtonStop = new QMC.Common.UI.BaseButton();
            this.baseButtonMove = new QMC.Common.UI.BaseButton();
            this.baseDataGridView2DMapData = new QMC.Common.UI.BaseDataGridView();
            this.baseButtonOffsetMove = new QMC.Common.UI.BaseButton();
            this.baseGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.baseDataGridView2DMapData)).BeginInit();
            this.SuspendLayout();

            // 
            // _2DMappingDataControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseGroupBox1);
            this.Name = "_2DMappingDataControl";
            this.Size = new System.Drawing.Size(660, 680);
            this.baseGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.baseDataGridView2DMapData)).EndInit();
            this.ResumeLayout(false);
            // 
            // baseGroupBox1
            // 
            this.baseGroupBox1.Controls.Add(this.baseButtonOffsetMove);
            this.baseGroupBox1.Controls.Add(this.baseButtonSave);
            this.baseGroupBox1.Controls.Add(this.baseButtonLoad);
            this.baseGroupBox1.Controls.Add(this.baseButtonStop);
            this.baseGroupBox1.Controls.Add(this.baseButtonMove);
            this.baseGroupBox1.Controls.Add(this.baseDataGridView2DMapData);
            this.baseGroupBox1.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.baseGroupBox1.Name = "baseGroupBox1";
            this.baseGroupBox1.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseGroupBox1.Size = new System.Drawing.Size(658, 584);
            this.baseGroupBox1.TabIndex = 0;
            this.baseGroupBox1.TabStop = false;
            this.baseGroupBox1.Text = " 2D Mapping Data ";
            // 
            // baseDataGridView2DMapData
            // 
            this.baseDataGridView2DMapData.AllowUserToAddRows = false;
            this.baseDataGridView2DMapData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.baseDataGridView2DMapData.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseDataGridView2DMapData.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            //dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            //dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.baseDataGridView2DMapData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.baseDataGridView2DMapData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.baseDataGridView2DMapData.DefaultCellStyle = dataGridViewCellStyle5;
            this.baseDataGridView2DMapData.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            this.baseDataGridView2DMapData.Location = new System.Drawing.Point(10, 25);
            this.baseDataGridView2DMapData.MultiSelect = false;
            this.baseDataGridView2DMapData.Name = "baseDataGridView2DMapData";
            this.baseDataGridView2DMapData.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.baseDataGridView2DMapData.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.baseDataGridView2DMapData.RowHeadersVisible = false;
            this.baseDataGridView2DMapData.RowTemplate.Height = 23;
            this.baseDataGridView2DMapData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.baseDataGridView2DMapData.Size = new System.Drawing.Size(411, 550);
            this.baseDataGridView2DMapData.TabIndex = 0;
            // 
            // baseButtonLoad
            // 
            //this.baseButtonLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonLoad.BackColor = System.Drawing.Color.White;
            this.baseButtonLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            //this.baseButtonLoad.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonLoad.ForeColor = System.Drawing.Color.Black;
            //this.baseButtonLoad.Location = new System.Drawing.Point(6, 552);
            //this.baseButtonLoad.Location = new System.Drawing.Point(baseDataGridView2DMapData.Location.X, baseDataGridView2DMapData.Location.Y + baseDataGridView2DMapData.Size.Height + 10);
            this.baseButtonLoad.Location = new System.Drawing.Point(baseDataGridView2DMapData.Location.X + baseDataGridView2DMapData.Size.Width + 20, baseDataGridView2DMapData.Location.Y);
            this.baseButtonLoad.Name = "baseButtonLoad";
            this.baseButtonLoad.Size = new System.Drawing.Size(100, 30);
            this.baseButtonLoad.TabIndex = 3;
            this.baseButtonLoad.Text = "Load";
            this.baseButtonLoad.UseVisualStyleBackColor = false;
            this.baseButtonLoad.Click += new System.EventHandler(this.baseButtonLoad_Click);
            // 
            // baseButtonSave
            // 
            //this.baseButtonSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonSave.BackColor = System.Drawing.Color.White;
            this.baseButtonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            //this.baseButtonSave.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonSave.ForeColor = System.Drawing.Color.Black;
            //this.baseButtonSave.Location = new System.Drawing.Point(174, 552);
            this.baseButtonSave.Location = new System.Drawing.Point(baseButtonLoad.Location.X + baseButtonLoad.Size.Width + 5, baseButtonLoad.Location.Y);
            this.baseButtonSave.Name = "baseButtonSave";
            this.baseButtonSave.Size = new System.Drawing.Size(100, 30);
            this.baseButtonSave.TabIndex = 4;
            this.baseButtonSave.Text = "Save";
            this.baseButtonSave.UseVisualStyleBackColor = false;
            this.baseButtonSave.Click += new System.EventHandler(this.baseButtonSave_Click);
            // 
            // baseButtonMove
            // 
            //this.baseButtonMove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonMove.BackColor = System.Drawing.Color.White;
            this.baseButtonMove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            //this.baseButtonMove.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonMove.ForeColor = System.Drawing.Color.Black;
            //this.baseButtonMove.Location = new System.Drawing.Point(6, 590);
            this.baseButtonMove.Location = new System.Drawing.Point(baseButtonLoad.Location.X, baseButtonLoad.Location.Y + baseButtonLoad.Size.Height + 5);
            this.baseButtonMove.Name = "baseButtonMove";
            this.baseButtonMove.Size = new System.Drawing.Size(100, 30);
            this.baseButtonMove.TabIndex = 1;
            this.baseButtonMove.Text = "Move";
            this.baseButtonMove.UseVisualStyleBackColor = false;
            this.baseButtonMove.Click += new System.EventHandler(this.baseButton_Click);
            // 
            // baseButtonStop
            // 
            //this.baseButtonStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonStop.BackColor = System.Drawing.Color.White;
            this.baseButtonStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            //this.baseButtonStop.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonStop.ForeColor = System.Drawing.Color.Black;
            //this.baseButtonStop.Location = new System.Drawing.Point(174, 590);
            this.baseButtonStop.Location = new System.Drawing.Point(baseButtonMove.Location.X + baseButtonMove.Size.Width + 5, baseButtonMove.Location.Y);
            this.baseButtonStop.Name = "baseButtonStop";
            this.baseButtonStop.Size = new System.Drawing.Size(100, 30);
            this.baseButtonStop.TabIndex = 2;
            this.baseButtonStop.Text = "Stop";
            this.baseButtonStop.UseVisualStyleBackColor = false;
            this.baseButtonStop.Click += new System.EventHandler(this.baseButton_Click);
            // 
            // baseButtonOffsetMove
            // 
            //this.baseButtonOffsetMove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonOffsetMove.BackColor = System.Drawing.Color.White;
            this.baseButtonOffsetMove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            //this.baseButtonOffsetMove.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonOffsetMove.ForeColor = System.Drawing.Color.Black;
            //this.baseButtonOffsetMove.Location = new System.Drawing.Point(6, 629);
            this.baseButtonOffsetMove.Location = new System.Drawing.Point(baseButtonMove.Location.X, baseButtonMove.Location.Y + baseButtonMove.Size.Height + 5);
            this.baseButtonOffsetMove.Name = "baseButtonOffsetMove";
            this.baseButtonOffsetMove.Size = new System.Drawing.Size(205, 30);
            this.baseButtonOffsetMove.TabIndex = 5;
            this.baseButtonOffsetMove.Text = "Offset  Move";
            this.baseButtonOffsetMove.UseVisualStyleBackColor = false;
            this.baseButtonOffsetMove.Click += new System.EventHandler(this.baseButton_Click);
        }

        #endregion

        private Common.UI.BaseGroupBox baseGroupBox1;
        private Common.UI.BaseButton baseButtonStop;
        private Common.UI.BaseButton baseButtonMove;
        private Common.UI.BaseDataGridView baseDataGridView2DMapData;
        private Common.UI.BaseButton baseButtonSave;
        private Common.UI.BaseButton baseButtonLoad;
        private Common.UI.BaseButton baseButtonOffsetMove;
    }
}
