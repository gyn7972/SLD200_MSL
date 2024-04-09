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
            this.button_Lamp0 = new System.Windows.Forms.Button();
            this.button_Lamp1 = new System.Windows.Forms.Button();
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
            // buttonLogOut
            // 
            this.buttonLogOut.Image = global::CWA150SA_Onsemi.Properties.Resources.LogOffa;
            this.buttonLogOut.Location = new System.Drawing.Point(1615, 5);
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
            this.buttonLogin.Location = new System.Drawing.Point(1697, 2);
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
            this.buttonExit.Location = new System.Drawing.Point(1764, 5);
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
            // button_Lamp0
            // 
            this.button_Lamp0.BackColor = System.Drawing.Color.LightGray;
            this.button_Lamp0.Font = new System.Drawing.Font("나눔바른고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Lamp0.ForeColor = System.Drawing.Color.DarkRed;
            this.button_Lamp0.Location = new System.Drawing.Point(1004, 9);
            this.button_Lamp0.Name = "button_Lamp0";
            this.button_Lamp0.Size = new System.Drawing.Size(183, 81);
            this.button_Lamp0.TabIndex = 10;
            this.button_Lamp0.Text = "내부   조명";
            this.button_Lamp0.UseVisualStyleBackColor = false;
            this.button_Lamp0.Click += new System.EventHandler(this.button_Lamp1_Click);
            // 
            // button_Lamp1
            // 
            this.button_Lamp1.BackColor = System.Drawing.Color.LightGray;
            this.button_Lamp1.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Lamp1.ForeColor = System.Drawing.Color.DarkRed;
            this.button_Lamp1.Location = new System.Drawing.Point(1109, 9);
            this.button_Lamp1.Name = "button_Lamp1";
            this.button_Lamp1.Size = new System.Drawing.Size(150, 81);
            this.button_Lamp1.TabIndex = 11;
            this.button_Lamp1.Text = "실내 조명 2";
            this.button_Lamp1.UseVisualStyleBackColor = false;
            this.button_Lamp1.Visible = false;
            this.button_Lamp1.Click += new System.EventHandler(this.button_Lamp2_Click);
            // 
            // FormBottom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1919, 101);
            this.Controls.Add(this.button_Lamp0);
            this.Controls.Add(this.button_Lamp1);
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
        private System.Windows.Forms.Button button_Lamp0;
        private System.Windows.Forms.Button button_Lamp1;
    }
}