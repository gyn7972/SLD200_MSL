
namespace CWA150SA_Onsemi300
{
    partial class FormIOBoard
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
            this.propertyGridIOBoard = new CWA150SA_Onsemi300.BasePropertyGrid();
            this.buttonNew = new CWA150SA_Onsemi300.BaseButton();
            this.buttonDelete = new CWA150SA_Onsemi300.BaseButton();
            this.buttonSave = new CWA150SA_Onsemi300.BaseButton();
            this.dataGridViewIOBoard = new CWA150SA_Onsemi300.BaseDataGridView();
            this.buttonLoad = new CWA150SA_Onsemi300.BaseButton();
            this.panelContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewIOBoard)).BeginInit();
            this.SuspendLayout();
            // 
            // panelContent
            // 
            this.panelContent.Controls.Add(this.dataGridViewIOBoard);
            this.panelContent.Controls.Add(this.propertyGridIOBoard);
            // 
            // propertyGridIOBoard
            // 
            this.propertyGridIOBoard.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGridIOBoard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.propertyGridIOBoard.CategoryForeColor = System.Drawing.Color.Black;
            this.propertyGridIOBoard.CategorySplitterColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.propertyGridIOBoard.CommandsBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.propertyGridIOBoard.HelpBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.propertyGridIOBoard.HelpForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.propertyGridIOBoard.Location = new System.Drawing.Point(0, 0);
            this.propertyGridIOBoard.Name = "propertyGridIOBoard";
            this.propertyGridIOBoard.SelectedItemWithFocusBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.propertyGridIOBoard.Size = new System.Drawing.Size(130, 130);
            this.propertyGridIOBoard.TabIndex = 1;
            this.propertyGridIOBoard.ViewBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.propertyGridIOBoard.ViewBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.propertyGridIOBoard.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.PropertyGridIOBoard_PropertyValueChanged);
            // 
            // buttonNew
            // 
            this.buttonNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonNew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.buttonNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonNew.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonNew.Location = new System.Drawing.Point(0, 0);
            this.buttonNew.Name = "buttonNew";
            this.buttonNew.Size = new System.Drawing.Size(75, 23);
            this.buttonNew.TabIndex = 2;
            this.buttonNew.Text = "New";
            this.buttonNew.UseVisualStyleBackColor = true;
            this.buttonNew.Click += new System.EventHandler(this.ButtonNew_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.buttonDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDelete.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonDelete.Location = new System.Drawing.Point(0, 0);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 23);
            this.buttonDelete.TabIndex = 2;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.ButtonDelete_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.buttonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSave.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonSave.Location = new System.Drawing.Point(0, 0);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 2;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.ButtonSave_Click);
            // 
            // dataGridViewIOBoard
            // 
            this.dataGridViewIOBoard.AllowUserToAddRows = false;
            this.dataGridViewIOBoard.AllowUserToResizeRows = false;
            this.dataGridViewIOBoard.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewIOBoard.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewIOBoard.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.dataGridViewIOBoard.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewIOBoard.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewIOBoard.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewIOBoard.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewIOBoard.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            this.dataGridViewIOBoard.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewIOBoard.Name = "dataGridViewIOBoard";
            this.dataGridViewIOBoard.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewIOBoard.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewIOBoard.RowHeadersVisible = false;
            this.dataGridViewIOBoard.RowHeadersWidth = 51;
            this.dataGridViewIOBoard.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewIOBoard.RowTemplate.Height = 23;
            this.dataGridViewIOBoard.Size = new System.Drawing.Size(240, 150);
            this.dataGridViewIOBoard.TabIndex = 0;
            this.dataGridViewIOBoard.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewIOBoard_CellValueChanged);
            this.dataGridViewIOBoard.CurrentCellChanged += new System.EventHandler(this.DataGridViewIOBoard_CurrentCellChanged);
            // 
            // buttonLoad
            // 
            this.buttonLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.buttonLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLoad.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonLoad.Location = new System.Drawing.Point(0, 0);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(75, 23);
            this.buttonLoad.TabIndex = 2;
            this.buttonLoad.Text = "Load";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.ButtonLoad_Click);
            // 
            // FormIOBoardm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "FormIOBoard";
            this.Text = "FormIOBoard";
            this.panelContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewIOBoard)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BasePropertyGrid propertyGridIOBoard;
        private BaseButton buttonNew;
        private BaseButton buttonDelete;
        private BaseButton buttonSave;
        private BaseDataGridView dataGridViewIOBoard;
        private BaseButton buttonLoad;
    }
}