namespace CWA150SA_Onsemi300
{
    partial class FormOperationGeneral
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
            this.flowLayoutPanelModuleList = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // flowLayoutPanelModuleList
            // 
            this.flowLayoutPanelModuleList.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelModuleList.Name = "flowLayoutPanelModuleList";
            this.flowLayoutPanelModuleList.Size = new System.Drawing.Size(200, 100);
            this.flowLayoutPanelModuleList.TabIndex = 2;
            // 
            // FormOperationGeneral
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.flowLayoutPanelModuleList);
            this.Name = "FormOperationGeneral";
            this.Text = "FormOperationGeneral";
            this.Controls.SetChildIndex(this.panelContent, 0);
            this.Controls.SetChildIndex(this.flowLayoutPanelModuleList, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelModuleList;
    }
}