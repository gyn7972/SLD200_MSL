namespace CWA150SA_Onsemi300
{
    partial class FormModuleCollection
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
            this.panelModules = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panelModules
            // 
            this.panelModules.Location = new System.Drawing.Point(0, 0);
            this.panelModules.Name = "panelModules";
            this.panelModules.Size = new System.Drawing.Size(200, 100);
            this.panelModules.TabIndex = 0;
            // 
            // FormConfigurationModuleCollection
            // 
            this.ClientSize = new System.Drawing.Size(800, 457);
            this.Controls.Add(this.panelModules);
            this.Name = "FormConfigurationModuleCollection";
            this.Text = "FormConfigurationModuleCollection";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelModules;
    }
}