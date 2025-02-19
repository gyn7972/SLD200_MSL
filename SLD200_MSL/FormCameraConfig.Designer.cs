namespace SLD200_MSL
{
    partial class FormCameraConfig
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
            this.CameraPropertyGridConfig_Upper = new SLD200_MSL.BasePropertyGrid();
            this.CameraPropertyGridConfig_Lower = new SLD200_MSL.BasePropertyGrid();
            this.buttonLoad = new SLD200_MSL.BaseButton();
            this.buttonSave = new SLD200_MSL.BaseButton();
            this.baseLabelCameraConfig_Upper = new SLD200_MSL.BaseLabel();
            this.baseLabelCameraConfig_Lower = new SLD200_MSL.BaseLabel();
            // 
            // CameraPropertyGridConfig_Upper 
            // 
            this.CameraPropertyGridConfig_Upper.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.CameraPropertyGridConfig_Upper.CategoryForeColor = System.Drawing.Color.Black;
            this.CameraPropertyGridConfig_Upper.CategorySplitterColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.CameraPropertyGridConfig_Upper.CommandsBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.CameraPropertyGridConfig_Upper.HelpBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.CameraPropertyGridConfig_Upper.HelpForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.CameraPropertyGridConfig_Upper.Location = new System.Drawing.Point(620, 158);
            this.CameraPropertyGridConfig_Upper.Name = "CameraPropertyGridConfig_Upper";
            this.CameraPropertyGridConfig_Upper.SelectedItemWithFocusBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.CameraPropertyGridConfig_Upper.Size = new System.Drawing.Size(56, 70);
            this.CameraPropertyGridConfig_Upper.TabIndex = 2;
            this.CameraPropertyGridConfig_Upper.ViewBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.CameraPropertyGridConfig_Upper.ViewBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            // 
            // CameraPropertyGridConfig_Lower
            // 
            this.CameraPropertyGridConfig_Lower.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.CameraPropertyGridConfig_Lower.CategoryForeColor = System.Drawing.Color.Black;
            this.CameraPropertyGridConfig_Lower.CategorySplitterColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.CameraPropertyGridConfig_Lower.CommandsBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.CameraPropertyGridConfig_Lower.HelpBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.CameraPropertyGridConfig_Lower.HelpForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.CameraPropertyGridConfig_Lower.Location = new System.Drawing.Point(620, 158);
            this.CameraPropertyGridConfig_Lower.Name = "CameraPropertyGridConfig_Lower";
            this.CameraPropertyGridConfig_Lower.SelectedItemWithFocusBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.CameraPropertyGridConfig_Lower.Size = new System.Drawing.Size(56, 70);
            this.CameraPropertyGridConfig_Lower.TabIndex = 2;
            this.CameraPropertyGridConfig_Lower.ViewBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.CameraPropertyGridConfig_Lower.ViewBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));

            // 
            // buttonLoad
            // 
            this.buttonLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.buttonLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLoad.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonLoad.Location = new System.Drawing.Point(472, 280);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(75, 23);
            this.buttonLoad.TabIndex = 5;
            this.buttonLoad.Text = "Load";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.buttonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSave.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonSave.Location = new System.Drawing.Point(553, 280);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 6;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);

            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "FormCameraConfig";
        }
        private BaseButton buttonLoad;
        private BaseButton buttonSave;
        private BasePropertyGrid CameraPropertyGridConfig_Upper;
        private BasePropertyGrid CameraPropertyGridConfig_Lower;
        private BaseLabel baseLabelCameraConfig_Upper;
        private BaseLabel baseLabelCameraConfig_Lower;
        #endregion
    }
}