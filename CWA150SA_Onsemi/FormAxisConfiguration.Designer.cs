namespace CWA150SA_Onsemi300
{
    partial class FormAxisConfiguration
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

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new CWA150SA_Onsemi300.BaseDataGridView();
            this.propertyGrid = new CWA150SA_Onsemi300.BasePropertyGrid();
            this.buttonDelete = new CWA150SA_Onsemi300.BaseButton();
            this.buttonNew = new CWA150SA_Onsemi300.BaseButton();
            this.buttonSave = new CWA150SA_Onsemi300.BaseButton();
            this.buttonLoad = new CWA150SA_Onsemi300.BaseButton();
            this.timerServoStatus = new System.Windows.Forms.Timer(this.components);
            this.panelContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelContent
            // 
            this.panelContent.Controls.Add(this.dataGridView1);
            this.panelContent.Controls.Add(this.propertyGrid);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.RowTemplate.ReadOnly = true;
            this.dataGridView1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.Size = new System.Drawing.Size(240, 150);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            this.dataGridView1.CurrentCellChanged += new System.EventHandler(this.dataGridView1_CurrentCellChanged);
            // 
            // propertyGrid
            // 
            this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.propertyGrid.CategoryForeColor = System.Drawing.Color.Black;
            this.propertyGrid.CategorySplitterColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.propertyGrid.CommandsBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.propertyGrid.HelpBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.propertyGrid.HelpForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.propertyGrid.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.SelectedItemWithFocusBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.propertyGrid.Size = new System.Drawing.Size(130, 130);
            this.propertyGrid.TabIndex = 4;
            this.propertyGrid.ViewBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.propertyGrid.ViewBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid_PropertyValueChanged);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDelete.AutoSize = true;
            this.buttonDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.buttonDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDelete.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonDelete.Location = new System.Drawing.Point(84, 3);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 23);
            this.buttonDelete.TabIndex = 6;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonNew
            // 
            this.buttonNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonNew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.buttonNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonNew.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonNew.Location = new System.Drawing.Point(3, 3);
            this.buttonNew.Name = "buttonNew";
            this.buttonNew.Size = new System.Drawing.Size(75, 23);
            this.buttonNew.TabIndex = 7;
            this.buttonNew.Text = "New";
            this.buttonNew.UseVisualStyleBackColor = true;
            this.buttonNew.Click += new System.EventHandler(this.buttonNew_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.buttonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSave.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonSave.Location = new System.Drawing.Point(84, 32);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 8;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonLoad
            // 
            this.buttonLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.buttonLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLoad.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonLoad.Location = new System.Drawing.Point(3, 32);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(75, 23);
            this.buttonLoad.TabIndex = 9;
            this.buttonLoad.Text = "Load";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // timerServoStatus
            //             
            this.timerServoStatus.Interval = 1000;
            this.timerServoStatus.Tick += new System.EventHandler(this.timerServoStatus_Tick);
            this.timerServoStatus.Enabled = true;
            // 
            // FormAxisConfiguration
            // 
            this.ClientSize = new System.Drawing.Size(1924, 624);
            this.Name = "FormAxisConfiguration";
            this.Text = "Axis Configuration";
            this.panelContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BaseDataGridView dataGridView1;
        private BasePropertyGrid propertyGrid;
        private BaseButton buttonDelete;
        private BaseButton buttonNew;
        private BaseButton buttonSave;
        private BaseButton buttonLoad;
        private System.Windows.Forms.Timer timerServoStatus;
    }
}

