namespace CWA150SA_Onsemi300
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
            this.buttonLogOut = new System.Windows.Forms.Button();
            this.buttonLogin = new System.Windows.Forms.Button();
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
            this.flowLayoutPanelStartAndStop.Location = new System.Drawing.Point(624, -1);
            this.flowLayoutPanelStartAndStop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.flowLayoutPanelStartAndStop.Name = "flowLayoutPanelStartAndStop";
            this.flowLayoutPanelStartAndStop.Size = new System.Drawing.Size(175, 80);
            this.flowLayoutPanelStartAndStop.TabIndex = 0;
            // 
            // buttonLogOut
            // 
            this.buttonLogOut.Image = global::CWA150SA_Onsemi.Properties.Resources.LogOffa;
            this.buttonLogOut.Location = new System.Drawing.Point(774, 5);
            this.buttonLogOut.Name = "buttonLogOut";
            this.buttonLogOut.Size = new System.Drawing.Size(143, 61);
            this.buttonLogOut.TabIndex = 1;
            this.buttonLogOut.Text = "Log Out";
            this.buttonLogOut.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.buttonLogOut.UseVisualStyleBackColor = true;
            this.buttonLogOut.Click += new System.EventHandler(this.buttonLogOut_Click);
            // 
            // buttonLogin
            // 
            this.buttonLogin.Image = global::CWA150SA_Onsemi.Properties.Resources.LogIna;
            this.buttonLogin.Location = new System.Drawing.Point(856, 2);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(143, 61);
            this.buttonLogin.TabIndex = 1;
            this.buttonLogin.Text = "Log In";
            this.buttonLogin.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonLogin.UseVisualStyleBackColor = true;
            this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
            // 
            // buttonExit
            // 
            this.buttonExit.Image = global::CWA150SA_Onsemi.Properties.Resources.ExitNormal;
            this.buttonExit.Location = new System.Drawing.Point(923, 5);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(143, 61);
            this.buttonExit.TabIndex = 1;
            this.buttonExit.Text = "Exit";
            this.buttonExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            this.buttonExit.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonExit_MouseDown);
            this.buttonExit.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonExit_MouseUp);
            // 
            // FormBottom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1078, 66);
            this.Controls.Add(this.flowLayoutPanelStartAndStop);
            this.Controls.Add(this.buttonLogOut);
            this.Controls.Add(this.buttonLogin);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.flowLayoutPanelBottom);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormBottom";
            this.Text = "FormBottom";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelBottom;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.Button buttonLogin;
        private System.Windows.Forms.Button buttonLogOut;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelStartAndStop;
    }
}