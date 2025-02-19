namespace SLD200_MSL
{
    partial class ModulePositionControl
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
            //this.GroupBox = new System.Windows.Forms.GroupBox();
            this.GroupBox = new WATGroupBox();
            this.PositionInfoGrid = new SLD200_MSL.BaseDataGridView();
            this.baseButtonSave = new SLD200_MSL.BaseButton();
            this.baseButtonMove = new SLD200_MSL.BaseButton();
            this.comboPosition = new System.Windows.Forms.ComboBox();
            this.GroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PositionInfoGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // GroupBox
            // 
            this.GroupBox.Controls.Add(this.PositionInfoGrid);
            this.GroupBox.Controls.Add(this.baseButtonSave);
            this.GroupBox.Controls.Add(this.baseButtonMove);
            this.GroupBox.Controls.Add(this.comboPosition);
            this.GroupBox.Font = new System.Drawing.Font("Tahoma", 9F);
            this.GroupBox.ForeColor = System.Drawing.Color.Black;
            this.GroupBox.Location = new System.Drawing.Point(0, 0);
            this.GroupBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GroupBox.Name = "GroupBox";
            this.GroupBox.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GroupBox.Size = new System.Drawing.Size(456, 153);
            this.GroupBox.TabIndex = 0;
            this.GroupBox.TabStop = false;
            this.GroupBox.Text = "groupBox1";
            // 
            // PositionInfoGrid
            // 
            this.PositionInfoGrid.AllowUserToAddRows = false;
            this.PositionInfoGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.PositionInfoGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.PositionInfoGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.PositionInfoGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.PositionInfoGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.PositionInfoGrid.DefaultCellStyle = dataGridViewCellStyle2;
            this.PositionInfoGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            this.PositionInfoGrid.Location = new System.Drawing.Point(6, 42);
            this.PositionInfoGrid.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PositionInfoGrid.Name = "PositionInfoGrid";
            this.PositionInfoGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.PositionInfoGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.PositionInfoGrid.RowHeadersVisible = false;
            this.PositionInfoGrid.RowHeadersWidth = 51;
            this.PositionInfoGrid.RowTemplate.Height = 27;
            this.PositionInfoGrid.Size = new System.Drawing.Size(444, 63);
            this.PositionInfoGrid.TabIndex = 3;
            // 
            // baseButtonSave
            // 
            this.baseButtonSave.BackColor = System.Drawing.Color.White;
            this.baseButtonSave.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.baseButtonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonSave.ForeColor = System.Drawing.Color.Black;
            this.baseButtonSave.Location = new System.Drawing.Point(189, 110);
            this.baseButtonSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseButtonSave.Name = "baseButtonSave";
            this.baseButtonSave.Size = new System.Drawing.Size(88, 37);
            this.baseButtonSave.TabIndex = 2;
            this.baseButtonSave.Text = "Save";
            this.baseButtonSave.UseVisualStyleBackColor = false;
            this.baseButtonSave.Click += new System.EventHandler(this.baseButtonSave_Click);
            // 
            // baseButtonMove
            // 
            this.baseButtonMove.BackColor = System.Drawing.Color.White;
            this.baseButtonMove.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.baseButtonMove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonMove.ForeColor = System.Drawing.Color.Black;
            this.baseButtonMove.Location = new System.Drawing.Point(19, 110);
            this.baseButtonMove.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseButtonMove.Name = "baseButtonMove";
            this.baseButtonMove.Size = new System.Drawing.Size(88, 37);
            this.baseButtonMove.TabIndex = 1;
            this.baseButtonMove.Text = "Move";
            this.baseButtonMove.UseVisualStyleBackColor = false;
            this.baseButtonMove.Click += new System.EventHandler(this.baseButtonMove_Click);
            // 
            // comboPosition
            // 
            this.comboPosition.FormattingEnabled = true;
            this.comboPosition.Location = new System.Drawing.Point(6, 18);
            this.comboPosition.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboPosition.Name = "comboPosition";
            this.comboPosition.Size = new System.Drawing.Size(444, 22);
            this.comboPosition.TabIndex = 0;
            this.comboPosition.SelectedIndexChanged += new System.EventHandler(this.comboPosition_SelectedIndexChanged);
            // 
            // ModulePositionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.GroupBox);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ModulePositionControl";
            this.Size = new System.Drawing.Size(459, 155);
            this.GroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PositionInfoGrid)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        //private System.Windows.Forms.GroupBox GroupBox;
        private WATGroupBox GroupBox;
        private BaseButton baseButtonSave;
        private BaseButton baseButtonMove;
        private System.Windows.Forms.ComboBox comboPosition;
        private BaseDataGridView PositionInfoGrid;
    }
}
