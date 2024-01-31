namespace QMC.Common.UI
{
    partial class FormSelectIO
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
            this.baseLabel = new QMC.Common.UI.BaseLabel();
            this.panelContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelContent
            // 
            this.panelContent.Controls.Add(this.baseLabel);
            // 
            // baseLabel
            // 
            this.baseLabel.AutoSize = true;
            this.baseLabel.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabel.ForeColor = System.Drawing.Color.White;
            this.baseLabel.Location = new System.Drawing.Point(56, 113);
            this.baseLabel.Name = "baseLabel";
            this.baseLabel.Size = new System.Drawing.Size(100, 16);
            this.baseLabel.TabIndex = 2;
            this.baseLabel.Text = "baseLabel1";
            // 
            // FormSelectIO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 634);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormSelectIO";
            this.Text = "FormSelectIO";
            this.panelContent.ResumeLayout(false);
            this.panelContent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private BaseLabel baseLabel;
    }
}