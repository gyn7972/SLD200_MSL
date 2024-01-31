
namespace QMC.Common.UI
{
    partial class FormContentBase
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
            this.panelContent = new DoubleBufferPanel();
            this.flowLayoutPanelButton = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // panelContent
            // 
        //    this.panelContent.Location = new System.Drawing.Point(0, 0);
            this.panelContent.Name = "panelContent";
      //      this.panelContent.Size = new System.Drawing.Size(200, 100);
            this.panelContent.TabIndex = 0;
            // 
            // flowLayoutPanelButton
            // 
       //     this.flowLayoutPanelButton.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelButton.Name = "flowLayoutPanelButton";
      //      this.flowLayoutPanelButton.Size = new System.Drawing.Size(200, 100);
            this.flowLayoutPanelButton.TabIndex = 1;
            // 
            // FormContentBase
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      //      this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.flowLayoutPanelButton);
            this.Controls.Add(this.panelContent);
            this.Name = "FormContentBase";
            this.Text = "FormContentBase";
            this.ResumeLayout(false);

        }

        #endregion

        public DoubleBufferPanel panelContent;
        protected System.Windows.Forms.FlowLayoutPanel flowLayoutPanelButton;
    }
}