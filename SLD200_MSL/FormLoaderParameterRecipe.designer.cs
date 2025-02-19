namespace SLD200_MSL
{
    partial class FormVisionParameterRecipe
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
            this.baseLabelRecipeProperty = new SLD200_MSL.BaseLabel();
            this.SuspendLayout();
            // 
            // baseLabelRecipeProperty
            // 
            this.baseLabelRecipeProperty.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelRecipeProperty.ForeColor = System.Drawing.Color.White;
            this.baseLabelRecipeProperty.Location = new System.Drawing.Point(0, 0);
            this.baseLabelRecipeProperty.Name = "baseLabelRecipeProperty";
            this.baseLabelRecipeProperty.Size = new System.Drawing.Size(100, 23);
            this.baseLabelRecipeProperty.TabIndex = 0;
            // 
            // FormWorkStageParameterRecipe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Name = "FormWorkStageParameterRecipe";
            this.Text = "FormStageLoaderRecipe";
            this.ResumeLayout(false);

        }

        #endregion
        private BaseLabel baseLabelRecipeProperty;
    }
}