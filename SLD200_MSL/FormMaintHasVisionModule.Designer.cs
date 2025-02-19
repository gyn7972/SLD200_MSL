namespace SLD200_MSL
{
    partial class FormMaintHasVisionModule
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
            this.panelIO = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panelContent
            // 
            this.panelContent.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelContent_MouseDown);
            // 
            // panelGeneral
            // 
            this.panelGeneral.Location = new System.Drawing.Point(127, 192);
            this.panelGeneral.Name = "panelGeneral";
            this.panelGeneral.Size = new System.Drawing.Size(200, 100);
            this.panelGeneral.TabIndex = 2;
            // 
            // panelIO
            // 
            this.panelIO.Location = new System.Drawing.Point(418, 164);
            this.panelIO.Name = "panelIO";
            this.panelIO.Size = new System.Drawing.Size(200, 100);
            this.panelIO.TabIndex = 3;
            // 
            // FormMaintHasVisionModule
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panelIO);
            this.Controls.Add(this.panelGeneral);
            this.Name = "FormMaintHasVisionModule";
            this.Text = "FormMaintHasVisionModule";
            this.Controls.SetChildIndex(this.panelContent, 0);
            this.Controls.SetChildIndex(this.panelGeneral, 0);
            this.Controls.SetChildIndex(this.panelIO, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelGeneral;
        private System.Windows.Forms.Panel panelIO;
    }
}