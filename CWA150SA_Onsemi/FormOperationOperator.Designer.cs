namespace CWA150SA_Onsemi300
{
    partial class FormOperationOperator
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
            this.panelGeneral = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panelGeneral
            // 
            this.panelGeneral.Location = new System.Drawing.Point(338, 62);
            this.panelGeneral.Name = "panelGeneral";
            this.panelGeneral.Size = new System.Drawing.Size(200, 100);
            this.panelGeneral.TabIndex = 2;
            // 
            // FormOperationOperator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panelGeneral);
            this.Name = "FormOperationOperator";
            this.Text = "FormOperationOperator";
            this.Controls.SetChildIndex(this.panelContent, 0);
            this.Controls.SetChildIndex(this.panelGeneral, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelGeneral;
    }
}