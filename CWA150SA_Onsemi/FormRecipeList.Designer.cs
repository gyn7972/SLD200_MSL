namespace CWA150SA_Onsemi300
{
    partial class FormRecipeList
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
            this.flowLayoutPanelTop = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanelControlButton = new System.Windows.Forms.FlowLayoutPanel();
            this.baseButtonCancel = new CWA150SA_Onsemi300.BaseButton();
            this.baseDataGridViewRecipeList = new CWA150SA_Onsemi300.BaseDataGridView();
            this.baseButtonOk = new CWA150SA_Onsemi300.BaseButton();
            ((System.ComponentModel.ISupportInitialize)(this.baseDataGridViewRecipeList)).BeginInit();
            this.SuspendLayout();
            // 
            // flowLayoutPanelTop
            // 
            this.flowLayoutPanelTop.Location = new System.Drawing.Point(399, 125);
            this.flowLayoutPanelTop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.flowLayoutPanelTop.Name = "flowLayoutPanelTop";
            this.flowLayoutPanelTop.Size = new System.Drawing.Size(65, 22);
            this.flowLayoutPanelTop.TabIndex = 0;
            // 
            // flowLayoutPanelControlButton
            // 
            this.flowLayoutPanelControlButton.Location = new System.Drawing.Point(379, 216);
            this.flowLayoutPanelControlButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.flowLayoutPanelControlButton.Name = "flowLayoutPanelControlButton";
            this.flowLayoutPanelControlButton.Size = new System.Drawing.Size(56, 21);
            this.flowLayoutPanelControlButton.TabIndex = 1;
            // 
            // baseButtonCancel
            // 
            this.baseButtonCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonCancel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonCancel.Location = new System.Drawing.Point(517, 76);
            this.baseButtonCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseButtonCancel.Name = "baseButtonCancel";
            this.baseButtonCancel.Size = new System.Drawing.Size(117, 88);
            this.baseButtonCancel.TabIndex = 3;
            this.baseButtonCancel.Text = "Cancel";
            this.baseButtonCancel.UseVisualStyleBackColor = false;
            this.baseButtonCancel.Click += new System.EventHandler(this.baseButtonCancel_Click);
            // 
            // baseDataGridViewRecipeList
            // 
            this.baseDataGridViewRecipeList.AllowUserToAddRows = false;
            this.baseDataGridViewRecipeList.AllowUserToResizeColumns = false;
            this.baseDataGridViewRecipeList.AllowUserToResizeRows = false;
            this.baseDataGridViewRecipeList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.baseDataGridViewRecipeList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseDataGridViewRecipeList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.baseDataGridViewRecipeList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.baseDataGridViewRecipeList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.baseDataGridViewRecipeList.DefaultCellStyle = dataGridViewCellStyle2;
            this.baseDataGridViewRecipeList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            this.baseDataGridViewRecipeList.Location = new System.Drawing.Point(595, 226);
            this.baseDataGridViewRecipeList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseDataGridViewRecipeList.MultiSelect = false;
            this.baseDataGridViewRecipeList.Name = "baseDataGridViewRecipeList";
            this.baseDataGridViewRecipeList.ReadOnly = true;
            this.baseDataGridViewRecipeList.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.baseDataGridViewRecipeList.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.baseDataGridViewRecipeList.RowHeadersVisible = false;
            this.baseDataGridViewRecipeList.RowHeadersWidth = 51;
            this.baseDataGridViewRecipeList.RowTemplate.Height = 27;
            this.baseDataGridViewRecipeList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.baseDataGridViewRecipeList.Size = new System.Drawing.Size(61, 72);
            this.baseDataGridViewRecipeList.TabIndex = 2;
            this.baseDataGridViewRecipeList.DoubleClick += BaseDataGridViewRecipeList_DoubleClick;
            // 
            // baseButtonOk
            // 
            this.baseButtonOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonOk.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonOk.Location = new System.Drawing.Point(393, 72);
            this.baseButtonOk.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseButtonOk.Name = "baseButtonOk";
            this.baseButtonOk.Size = new System.Drawing.Size(117, 88);
            this.baseButtonOk.TabIndex = 4;
            this.baseButtonOk.Text = "Ok";
            this.baseButtonOk.UseVisualStyleBackColor = false;
            this.baseButtonOk.Click += new System.EventHandler(this.baseButtonOk_Click);
            // 
            // FormRecipeList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.baseButtonOk);
            this.Controls.Add(this.baseButtonCancel);
            this.Controls.Add(this.baseDataGridViewRecipeList);
            this.Controls.Add(this.flowLayoutPanelControlButton);
            this.Controls.Add(this.flowLayoutPanelTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormRecipeList";
            this.Text = "FormRecipeCreate";
            ((System.ComponentModel.ISupportInitialize)(this.baseDataGridViewRecipeList)).EndInit();
            this.ResumeLayout(false);

        }
               

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelTop;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelControlButton;
        private BaseDataGridView baseDataGridViewRecipeList;
        private BaseButton baseButtonCancel;
        private BaseButton baseButtonOk;
    }
}