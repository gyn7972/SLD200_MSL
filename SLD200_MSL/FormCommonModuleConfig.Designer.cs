namespace SLD200_MSL
{
    partial class FormCommonModuleConfig
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
            this.baseLabelModuleConfiguration = new SLD200_MSL.BaseLabel();
            // 
            // baseLabelModuleConfiguration
            // 
            this.baseLabelModuleConfiguration.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelModuleConfiguration.ForeColor = System.Drawing.Color.White;
            this.baseLabelModuleConfiguration.Location = new System.Drawing.Point(0, 0);
            this.baseLabelModuleConfiguration.Name = "baseLabelModuleConfiguration";
            this.baseLabelModuleConfiguration.Size = new System.Drawing.Size(100, 23);
            this.baseLabelModuleConfiguration.TabIndex = 0;

            //this.panelContent.Location = new System.Drawing.Point(85, 143);
            this.panelContent.Size = new System.Drawing.Size(853, 426);
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "FormCommonModuleConfig";

            this.ClientSize = new System.Drawing.Size(1192, 676);
        }
        private BaseLabel baseLabelModuleConfiguration;
        #endregion
    }
}