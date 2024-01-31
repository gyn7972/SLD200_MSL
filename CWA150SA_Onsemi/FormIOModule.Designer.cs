
namespace CWA150SA_Onsemi300
{
    partial class FormIOModule
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.buttonDelete = new CWA150SA_Onsemi300.BaseButton();
            this.buttonLoad = new CWA150SA_Onsemi300.BaseButton();
            this.buttonSave = new CWA150SA_Onsemi300.BaseButton();
            this.buttonNew = new CWA150SA_Onsemi300.BaseButton();
            this.propertyGridIOModule = new CWA150SA_Onsemi300.BasePropertyGrid();
            this.dataGridViewIOModule = new CWA150SA_Onsemi300.BaseDataGridView();
            this.panelContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewIOModule)).BeginInit();
            this.SuspendLayout();
            // 
            // panelContent
            // 
            this.panelContent.Controls.Add(this.propertyGridIOModule);
            this.panelContent.Controls.Add(this.dataGridViewIOModule);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.buttonDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDelete.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonDelete.Location = new System.Drawing.Point(84, 3);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 23);
            this.buttonDelete.TabIndex = 5;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.ButtonDelete_Click);
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
            this.buttonLoad.TabIndex = 6;
            this.buttonLoad.Text = "Load";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.ButtonLoad_Click);
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
            this.buttonSave.TabIndex = 7;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.ButtonSave_Click);
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
            this.buttonNew.TabIndex = 8;
            this.buttonNew.Text = "New";
            this.buttonNew.UseVisualStyleBackColor = true;
            this.buttonNew.Click += new System.EventHandler(this.ButtonNew_Click);
            // 
            // propertyGridIOModule
            // 
            this.propertyGridIOModule.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGridIOModule.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.propertyGridIOModule.CategoryForeColor = System.Drawing.Color.Black;
            this.propertyGridIOModule.CategorySplitterColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.propertyGridIOModule.CommandsBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.propertyGridIOModule.HelpBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.propertyGridIOModule.HelpForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.propertyGridIOModule.Location = new System.Drawing.Point(0, 0);
            this.propertyGridIOModule.Name = "propertyGridIOModule";
            this.propertyGridIOModule.SelectedItemWithFocusBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.propertyGridIOModule.Size = new System.Drawing.Size(130, 130);
            this.propertyGridIOModule.TabIndex = 4;
            this.propertyGridIOModule.ViewBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.propertyGridIOModule.ViewBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.propertyGridIOModule.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.PropertyGridIOModule_PropertyValueChanged);
            // 
            // dataGridViewIOModule
            // 
            this.dataGridViewIOModule.AllowUserToAddRows = false;
            this.dataGridViewIOModule.AllowUserToDeleteRows = false;
            this.dataGridViewIOModule.AllowUserToResizeRows = false;
            this.dataGridViewIOModule.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewIOModule.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewIOModule.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.dataGridViewIOModule.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewIOModule.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewIOModule.ColumnHeadersHeight = 29;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewIOModule.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewIOModule.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            this.dataGridViewIOModule.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewIOModule.Name = "dataGridViewIOModule";
            this.dataGridViewIOModule.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewIOModule.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewIOModule.RowHeadersVisible = false;
            this.dataGridViewIOModule.RowHeadersWidth = 51;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewIOModule.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewIOModule.RowTemplate.Height = 23;
            this.dataGridViewIOModule.Size = new System.Drawing.Size(240, 150);
            this.dataGridViewIOModule.TabIndex = 3;
            this.dataGridViewIOModule.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewIOModule_CellValueChanged);
            this.dataGridViewIOModule.CurrentCellChanged += new System.EventHandler(this.DataGridViewIOModule_CurrentCellChanged);
            // 
            // FormIOModule
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "FormIOModule";
            this.Text = "FormIOModule";
            this.panelContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewIOModule)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BaseButton buttonDelete;
        private BaseButton buttonLoad;
        private BaseButton buttonSave;
        private BaseButton buttonNew;
        private BasePropertyGrid propertyGridIOModule;
        private BaseDataGridView dataGridViewIOModule;
    }
}