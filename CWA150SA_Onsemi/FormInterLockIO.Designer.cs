namespace CWA150SA_Onsemi300
{
    partial class FormInterLockIO
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
            this.dataGridViewInterLockIO = new System.Windows.Forms.DataGridView();
            this.comboBoxInterLockIO = new System.Windows.Forms.ComboBox();
            this.flowLayoutPanelIntelockIO = new System.Windows.Forms.FlowLayoutPanel();
            this.baseLabelInterlockIO = new CWA150SA_Onsemi300.BaseLabel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInterLockIO)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewInterLockIO
            // 
            this.dataGridViewInterLockIO.AllowUserToAddRows = false;
            this.dataGridViewInterLockIO.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewInterLockIO.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewInterLockIO.Location = new System.Drawing.Point(12, 60);
            this.dataGridViewInterLockIO.MultiSelect = false;
            this.dataGridViewInterLockIO.Name = "dataGridViewInterLockIO";
            this.dataGridViewInterLockIO.RowHeadersVisible = false;
            this.dataGridViewInterLockIO.RowHeadersWidth = 51;
            this.dataGridViewInterLockIO.RowTemplate.Height = 27;
            this.dataGridViewInterLockIO.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewInterLockIO.Size = new System.Drawing.Size(885, 527);
            this.dataGridViewInterLockIO.TabIndex = 0;
            // 
            // comboBoxInterLockIO
            // 
            this.comboBoxInterLockIO.FormattingEnabled = true;
            this.comboBoxInterLockIO.Location = new System.Drawing.Point(690, 593);
            this.comboBoxInterLockIO.Name = "comboBoxInterLockIO";
            this.comboBoxInterLockIO.Size = new System.Drawing.Size(187, 23);
            this.comboBoxInterLockIO.TabIndex = 2;
            // 
            // flowLayoutPanelIntelockIO
            // 
            this.flowLayoutPanelIntelockIO.Location = new System.Drawing.Point(337, 12);
            this.flowLayoutPanelIntelockIO.Name = "flowLayoutPanelIntelockIO";
            this.flowLayoutPanelIntelockIO.Size = new System.Drawing.Size(277, 39);
            this.flowLayoutPanelIntelockIO.TabIndex = 11;
            // 
            // baseLabelInterlockIO
            // 
            this.baseLabelInterlockIO.AutoSize = true;
            this.baseLabelInterlockIO.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelInterlockIO.ForeColor = System.Drawing.Color.White;
            this.baseLabelInterlockIO.Location = new System.Drawing.Point(29, 12);
            this.baseLabelInterlockIO.Name = "baseLabelInterlockIO";
            this.baseLabelInterlockIO.Size = new System.Drawing.Size(116, 20);
            this.baseLabelInterlockIO.TabIndex = 10;
            this.baseLabelInterlockIO.Text = "baseLabel1";
            // 
            // FormInterLockIO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Controls.Add(this.flowLayoutPanelIntelockIO);
            this.Controls.Add(this.baseLabelInterlockIO);
            this.Controls.Add(this.comboBoxInterLockIO);
            this.Controls.Add(this.dataGridViewInterLockIO);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormInterLockIO";
            this.Text = "FormInteLockIO";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInterLockIO)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewInterLockIO;
        private System.Windows.Forms.ComboBox comboBoxInterLockIO;
        private BaseLabel baseLabelInterlockIO;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelIntelockIO;
    }
}