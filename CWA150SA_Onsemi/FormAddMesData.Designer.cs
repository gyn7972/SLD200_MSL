namespace CWA150SA_Onsemi300
{
    partial class FormAddMesData
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.baseDataGridViewMES = new BaseDataGridView();
            this.baseButtonApply = new BaseButton();
            this.baseButtonCancel = new BaseButton();
            ((System.ComponentModel.ISupportInitialize)(this.baseDataGridViewMES)).BeginInit();
            this.SuspendLayout();
            // 
            // baseDataGridViewMES
            // 
            this.baseDataGridViewMES.AllowUserToAddRows = false;
            this.baseDataGridViewMES.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.baseDataGridViewMES.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseDataGridViewMES.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.baseDataGridViewMES.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.baseDataGridViewMES.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.baseDataGridViewMES.DefaultCellStyle = dataGridViewCellStyle5;
            this.baseDataGridViewMES.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            this.baseDataGridViewMES.Location = new System.Drawing.Point(12, 12);
            this.baseDataGridViewMES.Name = "baseDataGridViewMES";
            this.baseDataGridViewMES.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.baseDataGridViewMES.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.baseDataGridViewMES.RowHeadersVisible = false;
            this.baseDataGridViewMES.RowTemplate.Height = 23;
            this.baseDataGridViewMES.Size = new System.Drawing.Size(694, 426);
            this.baseDataGridViewMES.TabIndex = 0;
            // 
            // baseButtonApply
            // 
            this.baseButtonApply.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonApply.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonApply.Location = new System.Drawing.Point(714, 12);
            this.baseButtonApply.Name = "baseButtonApply";
            this.baseButtonApply.Size = new System.Drawing.Size(75, 23);
            this.baseButtonApply.TabIndex = 1;
            this.baseButtonApply.Text = "Apply";
            this.baseButtonApply.UseVisualStyleBackColor = false;
            this.baseButtonApply.Click += new System.EventHandler(this.baseButtonApply_Click);
            // 
            // baseButtonCancel
            // 
            this.baseButtonCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonCancel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonCancel.Location = new System.Drawing.Point(714, 41);
            this.baseButtonCancel.Name = "baseButtonCancel";
            this.baseButtonCancel.Size = new System.Drawing.Size(75, 23);
            this.baseButtonCancel.TabIndex = 2;
            this.baseButtonCancel.Text = "Cancel";
            this.baseButtonCancel.UseVisualStyleBackColor = false;
            this.baseButtonCancel.Click += new System.EventHandler(this.baseButtonCancel_Click);
            // 
            // FormAddMesData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.baseButtonCancel);
            this.Controls.Add(this.baseButtonApply);
            this.Controls.Add(this.baseDataGridViewMES);
            this.Name = "FormAddMesData";
            this.Text = "FormAddMesData";

            this.Load += new System.EventHandler(this.ListMesData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.baseDataGridViewMES)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BaseDataGridView baseDataGridViewMES;
        private BaseButton baseButtonApply;
        private BaseButton baseButtonCancel;
    }
}