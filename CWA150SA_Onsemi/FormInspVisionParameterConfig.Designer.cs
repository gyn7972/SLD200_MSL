namespace MicroLedDieTransfers2
{
    partial class FormInspVisionParameterConfig
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
            this.InspVisionParameterGrid = new MicroLedDieTransfers2.BasePropertyGrid();
            this.InspVision1PositionPropertyGrid = new MicroLedDieTransfers2.BasePropertyGrid();
            this.InspVision1PositionGrid = new MicroLedDieTransfers2.BaseDataGridView();
            this.InspVision2PositionPropertyGrid = new MicroLedDieTransfers2.BasePropertyGrid();
            this.InspVision2PositionGrid = new MicroLedDieTransfers2.BaseDataGridView();
            this.baseLabelModuleConfiguration = new MicroLedDieTransfers2.BaseLabel();
            this.baseLabelModuleParameter = new MicroLedDieTransfers2.BaseLabel();

            this.buttonLoad = new MicroLedDieTransfers2.BaseButton();
            this.buttonSave = new MicroLedDieTransfers2.BaseButton();

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
            // InspVision1PositionGrid
            //
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.InspVision1PositionGrid.AllowUserToAddRows = false;
            this.InspVision1PositionGrid.AllowUserToResizeColumns = false;
            this.InspVision1PositionGrid.AllowUserToResizeRows = false;
            this.InspVision1PositionGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.InspVision1PositionGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.InspVision1PositionGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.InspVision1PositionGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.InspVision1PositionGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.InspVision1PositionGrid.DefaultCellStyle = dataGridViewCellStyle2;
            this.InspVision1PositionGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            this.InspVision1PositionGrid.Location = new System.Drawing.Point(482, 179);
            this.InspVision1PositionGrid.Name = "InspVision1PositionGrid";
            this.InspVision1PositionGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.InspVision1PositionGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.InspVision1PositionGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.InspVision1PositionGrid.RowHeadersVisible = false;
            this.InspVision1PositionGrid.RowTemplate.Height = 35;
            this.InspVision1PositionGrid.Name = "InspVision1PositionGrid";
            this.InspVision1PositionGrid.TabIndex = 8;

            //
            //  InspVision1PositionPropertyGrid
            //
            this.InspVision1PositionPropertyGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.InspVision1PositionPropertyGrid.CategoryForeColor = System.Drawing.Color.Black;
            this.InspVision1PositionPropertyGrid.CategorySplitterColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.InspVision1PositionPropertyGrid.CommandsBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.InspVision1PositionPropertyGrid.HelpBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.InspVision1PositionPropertyGrid.HelpForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.InspVision1PositionPropertyGrid.Name = "InspVision1PositionPropertyGrid";
            this.InspVision1PositionPropertyGrid.SelectedItemWithFocusBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.InspVision1PositionPropertyGrid.TabIndex = 2;
            this.InspVision1PositionPropertyGrid.ViewBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.InspVision1PositionPropertyGrid.ViewBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.InspVision1PositionPropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.InspVision1PositionPropertyGrid_PropertyValueChanged);

            //
            // InspVision2PositionGrid
            //
            this.InspVision2PositionGrid.AllowUserToAddRows = false;
            this.InspVision2PositionGrid.AllowUserToResizeColumns = false;
            this.InspVision2PositionGrid.AllowUserToResizeRows = false;
            this.InspVision2PositionGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.InspVision2PositionGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.InspVision2PositionGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.InspVision2PositionGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.InspVision2PositionGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.InspVision2PositionGrid.DefaultCellStyle = dataGridViewCellStyle2;
            this.InspVision2PositionGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            this.InspVision2PositionGrid.Location = new System.Drawing.Point(482, 179);
            this.InspVision2PositionGrid.Name = "InspVision2PositionGrid";
            this.InspVision2PositionGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.InspVision2PositionGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.InspVision2PositionGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.InspVision2PositionGrid.RowHeadersVisible = false;
            this.InspVision2PositionGrid.RowTemplate.Height = 35;
            this.InspVision2PositionGrid.Name = "InspVision2PositionGrid";
            this.InspVision2PositionGrid.TabIndex = 8;

            //
            //  InspVision2PositionPropertyGrid
            //
            this.InspVision2PositionPropertyGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.InspVision2PositionPropertyGrid.CategoryForeColor = System.Drawing.Color.Black;
            this.InspVision2PositionPropertyGrid.CategorySplitterColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.InspVision2PositionPropertyGrid.CommandsBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.InspVision2PositionPropertyGrid.HelpBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.InspVision2PositionPropertyGrid.HelpForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.InspVision2PositionPropertyGrid.Name = "InspVision2PositionPropertyGrid";
            this.InspVision2PositionPropertyGrid.SelectedItemWithFocusBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.InspVision2PositionPropertyGrid.TabIndex = 2;
            this.InspVision2PositionPropertyGrid.ViewBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.InspVision2PositionPropertyGrid.ViewBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.InspVision2PositionPropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.InspVision2PositionPropertyGrid_PropertyValueChanged);

            //
            //  InspVisionParameterGrid
            //
            this.InspVisionParameterGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.InspVisionParameterGrid.CategoryForeColor = System.Drawing.Color.Black;
            this.InspVisionParameterGrid.CategorySplitterColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.InspVisionParameterGrid.CommandsBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.InspVisionParameterGrid.HelpBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.InspVisionParameterGrid.HelpForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.InspVisionParameterGrid.Name = "InspVisionParameterGrid";
            this.InspVisionParameterGrid.SelectedItemWithFocusBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.InspVisionParameterGrid.TabIndex = 2;
            this.InspVisionParameterGrid.ViewBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.InspVisionParameterGrid.ViewBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.InspVisionParameterGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.InspVisionParameterGrid_PropertyValueChanged);

            this.ClientSize = new System.Drawing.Size(771, 513);


            //((System.ComponentModel.ISupportInitialize)(this.StageLoderConfigGrid)).EndInit();


            this.ResumeLayout(false);
        }

        private BasePropertyGrid InspVisionParameterGrid;
        private BaseLabel baseLabelModuleConfiguration;
        private BaseLabel baseLabelModuleParameter;
        private BaseButton buttonLoad;
        private BaseButton buttonSave;
        private BaseDataGridView InspVision1PositionGrid;
        private BasePropertyGrid InspVision1PositionPropertyGrid;
        private BaseDataGridView InspVision2PositionGrid;
        private BasePropertyGrid InspVision2PositionPropertyGrid;
        #endregion
    }
}