namespace SLD200_MSL
{
    partial class FormNew_Logout
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
            this.button_Exit = new System.Windows.Forms.Button();
            this.button_Logout = new System.Windows.Forms.Button();
            this.button_Login = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_Exit
            // 
            this.button_Exit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_Exit.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.button_Exit.Location = new System.Drawing.Point(1180, 379);
            this.button_Exit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_Exit.Name = "button_Exit";
            this.button_Exit.Size = new System.Drawing.Size(160, 90);
            this.button_Exit.TabIndex = 19;
            this.button_Exit.Text = "Exit";
            this.button_Exit.UseVisualStyleBackColor = true;
            this.button_Exit.Click += new System.EventHandler(this.button_Exit_Click);
            // 
            // button_Logout
            // 
            this.button_Logout.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_Logout.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.button_Logout.Location = new System.Drawing.Point(880, 379);
            this.button_Logout.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_Logout.Name = "button_Logout";
            this.button_Logout.Size = new System.Drawing.Size(160, 90);
            this.button_Logout.TabIndex = 18;
            this.button_Logout.Text = "Logout";
            this.button_Logout.UseVisualStyleBackColor = true;
            this.button_Logout.Click += new System.EventHandler(this.button_Logout_Click);
            // 
            // button_Login
            // 
            this.button_Login.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button_Login.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.button_Login.Location = new System.Drawing.Point(580, 379);
            this.button_Login.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_Login.Name = "button_Login";
            this.button_Login.Size = new System.Drawing.Size(160, 90);
            this.button_Login.TabIndex = 17;
            this.button_Login.Text = "Login";
            this.button_Login.UseVisualStyleBackColor = true;
            this.button_Login.Click += new System.EventHandler(this.button_Login_Click);
            // 
            // FormNew_Logout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1920, 877);
            this.ControlBox = false;
            this.Controls.Add(this.button_Exit);
            this.Controls.Add(this.button_Logout);
            this.Controls.Add(this.button_Login);
            this.Font = new System.Drawing.Font("Tahoma", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormNew_Logout";
            this.Text = "FormNew_Main";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_Exit;
        private System.Windows.Forms.Button button_Logout;
        private System.Windows.Forms.Button button_Login;
    }
}