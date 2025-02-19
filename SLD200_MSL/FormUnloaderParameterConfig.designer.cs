namespace SLD200_MSL
{
    partial class FormUnloaderParameterConfig
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
            // 
            // FormLoaderDrillingParameterConfig
            // 
            //this.ClientSize = new System.Drawing.Size(610, 360);
            this.Name = "FormUnloaderParameterConfig";
            this.ResumeLayout(false);



            this.SuspendLayout();
            this.UnloaderParameterGrid = new SLD200_MSL.BasePropertyGrid();
            this.UnloaderPositionPropertyGrid = new SLD200_MSL.BasePropertyGrid();
            this.UnloaderPositionGrid = new SLD200_MSL.BaseDataGridView();
            this.baseLabelModuleConfiguration = new SLD200_MSL.BaseLabel();
            this.baseLabelModuleParameter = new SLD200_MSL.BaseLabel();

            this.buttonLoad = new SLD200_MSL.BaseButton();
            this.buttonSave = new SLD200_MSL.BaseButton();

            this.buttonCommonParam_Save = new SLD200_MSL.BaseButton();

            // 
            // buttonLoad
            // 
            //this.buttonLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.buttonLoad.BackColor = System.Drawing.Color.White;
            this.buttonLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            //this.buttonLoad.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonLoad.ForeColor = System.Drawing.Color.Black;
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
            //this.buttonSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.buttonSave.BackColor = System.Drawing.Color.White;
            this.buttonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            //this.buttonSave.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonSave.ForeColor = System.Drawing.Color.Black;
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
            this.UnloaderPositionGrid.AllowUserToAddRows = false;
            this.UnloaderPositionGrid.AllowUserToResizeColumns = false;
            this.UnloaderPositionGrid.AllowUserToResizeRows = false;
            this.UnloaderPositionGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.UnloaderPositionGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.UnloaderPositionGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.UnloaderPositionGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.UnloaderPositionGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.UnloaderPositionGrid.DefaultCellStyle = dataGridViewCellStyle2;
            this.UnloaderPositionGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            this.UnloaderPositionGrid.Location = new System.Drawing.Point(482, 179);
            this.UnloaderPositionGrid.Name = "PositionGrid";
            this.UnloaderPositionGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.UnloaderPositionGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.UnloaderPositionGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.UnloaderPositionGrid.RowHeadersVisible = false;
            this.UnloaderPositionGrid.RowTemplate.Height = 35;
            this.UnloaderPositionGrid.Name = "PositionGrid";
            this.UnloaderPositionGrid.TabIndex = 8;

            //
            //  PropertyGrid
            //
            this.UnloaderPositionPropertyGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.UnloaderPositionPropertyGrid.CategoryForeColor = System.Drawing.Color.Black;
            this.UnloaderPositionPropertyGrid.CategorySplitterColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.UnloaderPositionPropertyGrid.CommandsBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.UnloaderPositionPropertyGrid.HelpBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.UnloaderPositionPropertyGrid.HelpForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.UnloaderPositionPropertyGrid.Name = "PropertyGrid";
            this.UnloaderPositionPropertyGrid.SelectedItemWithFocusBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.UnloaderPositionPropertyGrid.TabIndex = 2;
            this.UnloaderPositionPropertyGrid.ViewBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.UnloaderPositionPropertyGrid.ViewBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.UnloaderPositionPropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.UnloaderPositionPropertyGrid_PropertyValueChanged);

            //
            //  ParameterGrid
            //
            this.UnloaderParameterGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.UnloaderParameterGrid.CategoryForeColor = System.Drawing.Color.Black;
            this.UnloaderParameterGrid.CategorySplitterColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.UnloaderParameterGrid.CommandsBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.UnloaderParameterGrid.HelpBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.UnloaderParameterGrid.HelpForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.UnloaderParameterGrid.Name = "ParameterGrid";
            this.UnloaderParameterGrid.SelectedItemWithFocusBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.UnloaderParameterGrid.TabIndex = 2;
            this.UnloaderParameterGrid.ViewBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.UnloaderParameterGrid.ViewBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.UnloaderParameterGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.UnloaderParameterGrid_PropertyValueChanged);

            this.ClientSize = new System.Drawing.Size(771, 513);
        }        

        private BasePropertyGrid UnloaderParameterGrid;
        private BaseLabel baseLabelModuleConfiguration;
        private BaseLabel baseLabelModuleParameter;
        private BaseButton buttonLoad;
        private BaseButton buttonSave;
        private BaseButton buttonCommonParam_Save;
        private BaseDataGridView UnloaderPositionGrid;
        private BasePropertyGrid UnloaderPositionPropertyGrid;
        #endregion
    }
}