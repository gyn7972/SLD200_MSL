namespace SLD200_MSL
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
            this.panelModules = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // panelModules
            // 
            this.panelModules.Font = new System.Drawing.Font("Tahoma", 9F);
            this.panelModules.Location = new System.Drawing.Point(12, 12);
            this.panelModules.Name = "panelModules";
            this.panelModules.Size = new System.Drawing.Size(200, 100);
            this.panelModules.TabIndex = 0;
            // 
            // FormModuleCollection
            // 
            this.ClientSize = new System.Drawing.Size(800, 570);
            this.Controls.Add(this.panelModules);
            this.Font = new System.Drawing.Font("Tahoma", 9F);
            this.Name = "FormModuleCollection";
            this.Text = "FormConfigurationModuleCollection";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel panelModules;
    }
}