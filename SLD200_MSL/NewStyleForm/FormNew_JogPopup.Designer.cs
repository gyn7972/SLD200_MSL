namespace SLD200.NewStyleForm
{
    partial class FormNew_JogPopup
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
            this.tabControl_JogPopup = new System.Windows.Forms.TabControl();
            this.tabPage_Loader = new System.Windows.Forms.TabPage();
            this.tabPage_Stage = new System.Windows.Forms.TabPage();
            this.tabPage_Unloader = new System.Windows.Forms.TabPage();
            this.tabControl_JogPopup.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl_JogPopup
            // 
            this.tabControl_JogPopup.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tabControl_JogPopup.Controls.Add(this.tabPage_Loader);
            this.tabControl_JogPopup.Controls.Add(this.tabPage_Stage);
            this.tabControl_JogPopup.Controls.Add(this.tabPage_Unloader);
            this.tabControl_JogPopup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_JogPopup.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.tabControl_JogPopup.Location = new System.Drawing.Point(0, 0);
            this.tabControl_JogPopup.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl_JogPopup.Name = "tabControl_JogPopup";
            this.tabControl_JogPopup.SelectedIndex = 0;
            this.tabControl_JogPopup.Size = new System.Drawing.Size(710, 598);
            this.tabControl_JogPopup.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl_JogPopup.TabIndex = 0;
            // 
            // tabPage_Loader
            // 
            this.tabPage_Loader.Location = new System.Drawing.Point(4, 31);
            this.tabPage_Loader.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage_Loader.Name = "tabPage_Loader";
            this.tabPage_Loader.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage_Loader.Size = new System.Drawing.Size(702, 563);
            this.tabPage_Loader.TabIndex = 0;
            this.tabPage_Loader.Text = "Loader";
            this.tabPage_Loader.UseVisualStyleBackColor = true;
            // 
            // tabPage_Stage
            // 
            this.tabPage_Stage.Location = new System.Drawing.Point(4, 31);
            this.tabPage_Stage.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage_Stage.Name = "tabPage_Stage";
            this.tabPage_Stage.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage_Stage.Size = new System.Drawing.Size(702, 563);
            this.tabPage_Stage.TabIndex = 1;
            this.tabPage_Stage.Text = "Stage";
            this.tabPage_Stage.UseVisualStyleBackColor = true;
            // 
            // tabPage_Unloader
            // 
            this.tabPage_Unloader.Location = new System.Drawing.Point(4, 31);
            this.tabPage_Unloader.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage_Unloader.Name = "tabPage_Unloader";
            this.tabPage_Unloader.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage_Unloader.Size = new System.Drawing.Size(702, 563);
            this.tabPage_Unloader.TabIndex = 2;
            this.tabPage_Unloader.Text = "Unloader";
            this.tabPage_Unloader.UseVisualStyleBackColor = true;
            // 
            // FormNew_JogPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(710, 598);
            this.Controls.Add(this.tabControl_JogPopup);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormNew_JogPopup";
            this.Text = "FormNew_JogPopup";
            this.tabControl_JogPopup.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl_JogPopup;
        private System.Windows.Forms.TabPage tabPage_Loader;
        private System.Windows.Forms.TabPage tabPage_Stage;
        private System.Windows.Forms.TabPage tabPage_Unloader;
    }
}