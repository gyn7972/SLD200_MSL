namespace SLD200_MSL
{
    partial class FormBottom
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
            this.flowLayoutPanelBottom = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanelStartAndStop = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // flowLayoutPanelBottom
            // 
            this.flowLayoutPanelBottom.Location = new System.Drawing.Point(0, -1);
            this.flowLayoutPanelBottom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.flowLayoutPanelBottom.Name = "flowLayoutPanelBottom";
            this.flowLayoutPanelBottom.Size = new System.Drawing.Size(551, 67);
            this.flowLayoutPanelBottom.TabIndex = 0;
            // 
            // flowLayoutPanelStartAndStop
            // 
            this.flowLayoutPanelStartAndStop.Location = new System.Drawing.Point(1465, -1);
            this.flowLayoutPanelStartAndStop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.flowLayoutPanelStartAndStop.Name = "flowLayoutPanelStartAndStop";
            this.flowLayoutPanelStartAndStop.Size = new System.Drawing.Size(175, 80);
            this.flowLayoutPanelStartAndStop.TabIndex = 0;
            // 
            // buttonExit
            // 
            this.buttonExit.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonExit.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.buttonExit.Location = new System.Drawing.Point(1764, 5);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(143, 61);
            this.buttonExit.TabIndex = 1;
            this.buttonExit.Text = "Logout";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Visible = false;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // FormBottom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1919, 101);
            this.Controls.Add(this.flowLayoutPanelStartAndStop);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.flowLayoutPanelBottom);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormBottom";
            this.Text = "FormBottom";
            this.ResumeLayout(false);

        }

        #endregion

        //private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelBottom;
        public System.Windows.Forms.FlowLayoutPanel flowLayoutPanelBottom;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelStartAndStop;
    }
}