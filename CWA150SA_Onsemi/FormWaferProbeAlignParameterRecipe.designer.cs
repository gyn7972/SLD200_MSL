namespace CWA150SA_Onsemi300
{
    partial class FormWaferProbeAlignParameterRecipe
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
            this.baseLabelRecipeProperty = new CWA150SA_Onsemi300.BaseLabel();
            this.SuspendLayout();
            // 
            // baseLabelRecipeProperty
            // 
            this.baseLabelRecipeProperty.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelRecipeProperty.ForeColor = System.Drawing.Color.White;
            this.baseLabelRecipeProperty.Location = new System.Drawing.Point(0, 0);
            this.baseLabelRecipeProperty.Name = "baseLabelRecipeProperty";
            this.baseLabelRecipeProperty.Size = new System.Drawing.Size(100, 23);
            this.baseLabelRecipeProperty.TabIndex = 0;
            // 
            // FormWaferProbeAlignParameterRecipe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Name = "FormWaferProbeAlignParameterRecipe";
            this.Text = "FormStageLoaderRecipe";
            this.ResumeLayout(false);

        }

        #endregion
        private BaseLabel baseLabelRecipeProperty;
    }
}