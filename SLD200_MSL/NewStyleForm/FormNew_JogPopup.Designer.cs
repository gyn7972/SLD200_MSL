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
            this.SiriusViewer_JogPopup = new SpiralLab.Sirius.SiriusViewerForm();
            this.tabControl_JogPopup.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl_JogPopup
            // 
            this.tabControl_JogPopup.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tabControl_JogPopup.Controls.Add(this.tabPage_Loader);
            this.tabControl_JogPopup.Controls.Add(this.tabPage_Stage);
            this.tabControl_JogPopup.Controls.Add(this.tabPage_Unloader);
            this.tabControl_JogPopup.Dock = System.Windows.Forms.DockStyle.Left;
            this.tabControl_JogPopup.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.tabControl_JogPopup.Location = new System.Drawing.Point(0, 0);
            this.tabControl_JogPopup.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl_JogPopup.Name = "tabControl_JogPopup";
            this.tabControl_JogPopup.SelectedIndex = 0;
            this.tabControl_JogPopup.Size = new System.Drawing.Size(496, 461);
            this.tabControl_JogPopup.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl_JogPopup.TabIndex = 0;
            // 
            // tabPage_Loader
            // 
            this.tabPage_Loader.Location = new System.Drawing.Point(4, 31);
            this.tabPage_Loader.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage_Loader.Name = "tabPage_Loader";
            this.tabPage_Loader.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage_Loader.Size = new System.Drawing.Size(488, 426);
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
            this.tabPage_Stage.Size = new System.Drawing.Size(488, 426);
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
            this.tabPage_Unloader.Size = new System.Drawing.Size(488, 426);
            this.tabPage_Unloader.TabIndex = 2;
            this.tabPage_Unloader.Text = "Unloader";
            this.tabPage_Unloader.UseVisualStyleBackColor = true;
            // 
            // SiriusViewer_JogPopup
            // 
            this.SiriusViewer_JogPopup.AliasName = "NoName";
            this.SiriusViewer_JogPopup.BackColor = System.Drawing.SystemColors.Control;
            this.SiriusViewer_JogPopup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SiriusViewer_JogPopup.Document = null;
            this.SiriusViewer_JogPopup.FileName = "NoName";
            this.SiriusViewer_JogPopup.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SiriusViewer_JogPopup.Index = ((uint)(0u));
            this.SiriusViewer_JogPopup.Location = new System.Drawing.Point(500, 31);
            this.SiriusViewer_JogPopup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SiriusViewer_JogPopup.Name = "SiriusViewer_JogPopup";
            this.SiriusViewer_JogPopup.Progress = 0;
            this.SiriusViewer_JogPopup.Size = new System.Drawing.Size(447, 426);
            this.SiriusViewer_JogPopup.TabIndex = 35;
            // 
            // FormNew_JogPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(956, 461);
            this.Controls.Add(this.SiriusViewer_JogPopup);
            this.Controls.Add(this.tabControl_JogPopup);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormNew_JogPopup";
            this.Text = "Jog Popup";
            this.tabControl_JogPopup.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl_JogPopup;
        private System.Windows.Forms.TabPage tabPage_Loader;
        private System.Windows.Forms.TabPage tabPage_Stage;
        private System.Windows.Forms.TabPage tabPage_Unloader;
        public SpiralLab.Sirius.SiriusViewerForm SiriusViewer_JogPopup;
    }
}