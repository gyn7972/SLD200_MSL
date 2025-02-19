namespace SLD200_MSL
{
    public partial class FormSubContentBase
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
            this.panelContent = new SLD200_MSL.DoubleBufferPanel();
            this.flowLayoutPanelButton = new System.Windows.Forms.FlowLayoutPanel();
            this.baseLabelTitle = new SLD200_MSL.BaseLabel();
            this.SuspendLayout();
            // 
            // panelContent
            // 
            this.panelContent.Location = new System.Drawing.Point(32, 121);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(200, 100);
            this.panelContent.TabIndex = 0;
            // 
            // flowLayoutPanelButton
            // 
            this.flowLayoutPanelButton.Location = new System.Drawing.Point(12, 12);
            this.flowLayoutPanelButton.Name = "flowLayoutPanelButton";
            this.flowLayoutPanelButton.Size = new System.Drawing.Size(200, 100);
            this.flowLayoutPanelButton.TabIndex = 1;
            // 
            // baseLabelTitle
            // 
            this.baseLabelTitle.AutoSize = true;
            this.baseLabelTitle.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelTitle.ForeColor = System.Drawing.Color.White;
            this.baseLabelTitle.Location = new System.Drawing.Point(90, 150);
            this.baseLabelTitle.Name = "baseLabelTitle";
            this.baseLabelTitle.Size = new System.Drawing.Size(64, 12);
            this.baseLabelTitle.TabIndex = 2;
            this.baseLabelTitle.Text = "baseLabel";
            // 
            // FormSubContentBase
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.flowLayoutPanelButton);
            this.Name = "FormSubContentBase";
            this.Text = "FormSubContentBase";
            this.ResumeLayout(false);

        }

        #endregion

        protected DoubleBufferPanel panelContent;
        protected System.Windows.Forms.FlowLayoutPanel flowLayoutPanelButton;
        public BaseLabel baseLabelTitle;
    }
}