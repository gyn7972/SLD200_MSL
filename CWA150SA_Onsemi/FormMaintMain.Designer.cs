namespace CWA150SA_Onsemi300
{
    partial class FormMaintMain
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
            // 
            // panelContent
            // 
            this.panelContent.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelContent_MouseDown);
            // 
            // FormMaintMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1870, 1046);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormMaintMain";
            this.Text = "FormMaint";
            this.ResumeLayout(false);

        }

        #endregion
    }
}