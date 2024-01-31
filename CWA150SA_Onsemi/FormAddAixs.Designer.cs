namespace CWA150SA_Onsemi300
{
    partial class FormAddAixs
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
            this.dataGridViewMotion = new CWA150SA_Onsemi300.BaseDataGridView();
            this.buttonCancel = new CWA150SA_Onsemi300.BaseButton();
            this.buttonApply = new CWA150SA_Onsemi300.BaseButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMotion)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewMotion
            // 
            this.dataGridViewMotion.AllowUserToAddRows = false;
            this.dataGridViewMotion.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dataGridViewMotion.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.dataGridViewMotion.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewMotion.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewMotion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewMotion.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewMotion.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            this.dataGridViewMotion.Location = new System.Drawing.Point(12, 12);
            this.dataGridViewMotion.Name = "dataGridViewMotion";
            this.dataGridViewMotion.ReadOnly = true;
            this.dataGridViewMotion.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewMotion.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewMotion.RowHeadersVisible = false;
            this.dataGridViewMotion.RowHeadersWidth = 51;
            this.dataGridViewMotion.RowTemplate.Height = 23;
            this.dataGridViewMotion.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewMotion.Size = new System.Drawing.Size(695, 400);
            this.dataGridViewMotion.TabIndex = 0;
            // 
            // buttonCancel
            // 
            this.buttonCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonCancel.Location = new System.Drawing.Point(713, 41);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonApply
            // 
            this.buttonApply.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.buttonApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonApply.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonApply.Location = new System.Drawing.Point(713, 12);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(75, 23);
            this.buttonApply.TabIndex = 1;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // FormAddAixs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(799, 450);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.dataGridViewMotion);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormAddAixs";
            this.Text = "FormAddAixs";
            this.Load += new System.EventHandler(this.FormAddAixs_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMotion)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BaseDataGridView dataGridViewMotion;
        private BaseButton buttonApply;
        private BaseButton buttonCancel;
    }
}