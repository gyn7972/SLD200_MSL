namespace CWA150SA_Onsemi300
{
    partial class FormAddIO
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
            this.buttonCancel = new CWA150SA_Onsemi300.BaseButton();
            this.buttonApply = new CWA150SA_Onsemi300.BaseButton();
            this.dataGridViewIO = new CWA150SA_Onsemi300.BaseDataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewIO)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonCancel.Location = new System.Drawing.Point(713, 41);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 4;
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
            this.buttonApply.TabIndex = 3;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // dataGridViewIO
            // 
            this.dataGridViewIO.AllowUserToAddRows = false;
            this.dataGridViewIO.AllowUserToResizeColumns = false;
            this.dataGridViewIO.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewIO.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridViewIO.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.dataGridViewIO.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewIO.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewIO.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewIO.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewIO.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            this.dataGridViewIO.Location = new System.Drawing.Point(12, 41);
            this.dataGridViewIO.Name = "dataGridViewIO";
            this.dataGridViewIO.ReadOnly = true;
            this.dataGridViewIO.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewIO.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewIO.RowHeadersVisible = false;
            this.dataGridViewIO.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            this.dataGridViewIO.RowTemplate.Height = 23;
            this.dataGridViewIO.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewIO.Size = new System.Drawing.Size(695, 400);
            this.dataGridViewIO.TabIndex = 1;
            // 
            // FormAddIO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(799, 450);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.dataGridViewIO);
            this.Name = "FormAddIO";
            this.Text = "FormAddIO";
            this.Load += new System.EventHandler(this.FormAddIO_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewIO)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BaseDataGridView dataGridViewIO;
        private BaseButton buttonCancel;
        private BaseButton buttonApply;
    }
}