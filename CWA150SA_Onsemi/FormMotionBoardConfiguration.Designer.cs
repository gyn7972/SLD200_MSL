namespace CWA150SA_Onsemi300
{
    partial class FormMotionBoardConfiguration
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
            this.buttonLoad = new BaseButton();
            this.buttonSave = new BaseButton();
            this.buttonNew = new BaseButton();
            this.buttonDelete = new BaseButton();
            this.propertyGridMotionBoard = new BasePropertyGrid();
            this.dataGridViewMotionBoard = new BaseDataGridView();
            this.panelContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMotionBoard)).BeginInit();
            this.SuspendLayout();
            // 
            // panelContent
            // 
            this.panelContent.Controls.Add(this.dataGridViewMotionBoard);
            this.panelContent.Controls.Add(this.propertyGridMotionBoard);
            // 
            // buttonLoad
            // 
            this.buttonLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLoad.Location = new System.Drawing.Point(3, 32);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(75, 23);
            this.buttonLoad.TabIndex = 15;
            this.buttonLoad.Text = "Load";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.Location = new System.Drawing.Point(84, 32);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 14;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonNew
            // 
            this.buttonNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonNew.Location = new System.Drawing.Point(3, 3);
            this.buttonNew.Name = "buttonNew";
            this.buttonNew.Size = new System.Drawing.Size(75, 23);
            this.buttonNew.TabIndex = 13;
            this.buttonNew.Text = "New";
            this.buttonNew.UseVisualStyleBackColor = true;
            this.buttonNew.Click += new System.EventHandler(this.buttonNew_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDelete.AutoSize = true;
            this.buttonDelete.Location = new System.Drawing.Point(84, 3);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 23);
            this.buttonDelete.TabIndex = 12;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // propertyGridMotionBoard
            // 
            this.propertyGridMotionBoard.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGridMotionBoard.Location = new System.Drawing.Point(0, 0);
            this.propertyGridMotionBoard.Name = "propertyGridMotionBoard";
            this.propertyGridMotionBoard.Size = new System.Drawing.Size(130, 130);
            this.propertyGridMotionBoard.TabIndex = 11;
            this.propertyGridMotionBoard.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGridMotionBoard_PropertyValueChanged_1);
            // 
            // dataGridViewMotionBoard
            // 
            this.dataGridViewMotionBoard.AllowUserToAddRows = false;
            this.dataGridViewMotionBoard.AllowUserToDeleteRows = false;
            this.dataGridViewMotionBoard.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewMotionBoard.ColumnHeadersHeight = 29;
            this.dataGridViewMotionBoard.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewMotionBoard.Name = "dataGridViewMotionBoard";
            this.dataGridViewMotionBoard.RowHeadersVisible = false;
            this.dataGridViewMotionBoard.RowHeadersWidth = 51;
            this.dataGridViewMotionBoard.RowTemplate.Height = 23;
            this.dataGridViewMotionBoard.Size = new System.Drawing.Size(240, 150);
            this.dataGridViewMotionBoard.TabIndex = 10;
            this.dataGridViewMotionBoard.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewMotionBoard_CellValueChanged);
            this.dataGridViewMotionBoard.CurrentCellChanged += new System.EventHandler(this.dataGridViewMotionBoard_CurrentCellChanged);
            // 
            // FormMotionBoardConfiguration
            // 
            this.Name = "FormMotionBoardConfiguration";
            this.Text = "FormMotionBoardConfiguration";
            this.panelContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMotionBoard)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BaseButton buttonLoad;
        private BaseButton buttonSave;
        private BaseButton buttonNew;
        private BaseButton buttonDelete;
        private BasePropertyGrid propertyGridMotionBoard;
        private BaseDataGridView dataGridViewMotionBoard;
    }
}