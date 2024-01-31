namespace CWA150SA_Onsemi300
{
    partial class FormWaferProbeAlignParameterConfig
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
          
            this.SuspendLayout();
            this.LaserParameterGrid = new CWA150SA_Onsemi300.BasePropertyGrid();
            this.LaserPositionPropertyGrid = new CWA150SA_Onsemi300.BasePropertyGrid();
            this.LaserPositionGrid = new CWA150SA_Onsemi300.BaseDataGridView();
            this.baseLabelModuleConfiguration = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelModuleParameter = new CWA150SA_Onsemi300.BaseLabel();

            this.buttonLoad = new CWA150SA_Onsemi300.BaseButton();
            this.buttonSave = new CWA150SA_Onsemi300.BaseButton();

            this.buttonCommonParam_Save = new CWA150SA_Onsemi300.BaseButton();

            // 
            // buttonLoad
            // 
            this.buttonLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.buttonLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLoad.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonLoad.Location = new System.Drawing.Point(472, 280);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(75, 23);
            this.buttonLoad.TabIndex = 5;
            this.buttonLoad.Text = "Load";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.buttonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSave.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonSave.Location = new System.Drawing.Point(553, 280);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 6;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);

            // 
            // buttonCommonParam_Save
            // 
            this.buttonCommonParam_Save.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.buttonCommonParam_Save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCommonParam_Save.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonCommonParam_Save.Location = new System.Drawing.Point(553, 280);
            this.buttonCommonParam_Save.Name = "buttonCommonParam_Save";
            this.buttonCommonParam_Save.Size = new System.Drawing.Size(100, 20);
            this.buttonCommonParam_Save.TabIndex = 6;
            this.buttonCommonParam_Save.Text = "Common Parameter Save";
            this.buttonCommonParam_Save.UseVisualStyleBackColor = true;
            this.buttonCommonParam_Save.Click += ButtonCommonParam_Save_Click;

            //
            // PositionGrid
            //
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.LaserPositionGrid.AllowUserToAddRows = false;
            this.LaserPositionGrid.AllowUserToResizeColumns = false;
            this.LaserPositionGrid.AllowUserToResizeRows = false;
            this.LaserPositionGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.LaserPositionGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.LaserPositionGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.LaserPositionGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.LaserPositionGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.LaserPositionGrid.DefaultCellStyle = dataGridViewCellStyle2;
            this.LaserPositionGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            this.LaserPositionGrid.Location = new System.Drawing.Point(482, 179);
            this.LaserPositionGrid.Name = "PositionGrid";
            this.LaserPositionGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.LaserPositionGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.LaserPositionGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.LaserPositionGrid.RowHeadersVisible = false;
            this.LaserPositionGrid.RowTemplate.Height = 35;
            this.LaserPositionGrid.Name = "PositionGrid";
            this.LaserPositionGrid.TabIndex = 8;

            //
            //  PropertyGrid
            //
            this.LaserPositionPropertyGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.LaserPositionPropertyGrid.CategoryForeColor = System.Drawing.Color.Black;
            this.LaserPositionPropertyGrid.CategorySplitterColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.LaserPositionPropertyGrid.CommandsBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.LaserPositionPropertyGrid.HelpBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.LaserPositionPropertyGrid.HelpForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.LaserPositionPropertyGrid.Name = "PropertyGrid";
            this.LaserPositionPropertyGrid.SelectedItemWithFocusBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.LaserPositionPropertyGrid.TabIndex = 2;
            this.LaserPositionPropertyGrid.ViewBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.LaserPositionPropertyGrid.ViewBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.LaserPositionPropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.LaserPositionPropertyGrid_PropertyValueChanged);

            //
            //  ParameterGrid
            //
            this.LaserParameterGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.LaserParameterGrid.CategoryForeColor = System.Drawing.Color.Black;
            this.LaserParameterGrid.CategorySplitterColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.LaserParameterGrid.CommandsBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.LaserParameterGrid.HelpBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.LaserParameterGrid.HelpForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.LaserParameterGrid.Name = "ParameterGrid";
            this.LaserParameterGrid.SelectedItemWithFocusBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.LaserParameterGrid.TabIndex = 2;
            this.LaserParameterGrid.ViewBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.LaserParameterGrid.ViewBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.LaserParameterGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.LaserParameterGrid_PropertyValueChanged);

            this.ClientSize = new System.Drawing.Size(771, 513);


            //((System.ComponentModel.ISupportInitialize)(this.StageLoderConfigGrid)).EndInit();


            this.ResumeLayout(false);
        }        

        private BasePropertyGrid LaserParameterGrid;
        private BaseLabel baseLabelModuleConfiguration;
        private BaseLabel baseLabelModuleParameter;
        private BaseButton buttonLoad;
        private BaseButton buttonSave;
        private BaseButton buttonCommonParam_Save;
        private BaseDataGridView LaserPositionGrid;
        private BasePropertyGrid LaserPositionPropertyGrid;
        #endregion
    }
}