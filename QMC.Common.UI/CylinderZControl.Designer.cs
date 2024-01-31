namespace QMC.Common.UI
{
    partial class CylinderZControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.baseGroupBoxMain = new QMC.Common.UI.BaseGroupBox();
            this.baseToggleButtonDown = new QMC.Common.UI.BaseToggleButton();
            this.baseToggleButtonUp = new QMC.Common.UI.BaseToggleButton();
            this.baseGroupBoxMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // baseGroupBoxMain
            // 
            this.baseGroupBoxMain.Controls.Add(this.baseToggleButtonDown);
            this.baseGroupBoxMain.Controls.Add(this.baseToggleButtonUp);
            this.baseGroupBoxMain.ForeColor = System.Drawing.Color.White;
            this.baseGroupBoxMain.Location = new System.Drawing.Point(0, 0);
            this.baseGroupBoxMain.Name = "baseGroupBoxMain";
            this.baseGroupBoxMain.Size = new System.Drawing.Size(227, 62);
            this.baseGroupBoxMain.TabIndex = 0;
            this.baseGroupBoxMain.TabStop = false;
            this.baseGroupBoxMain.Text = "Cylinder Z";
            // 
            // baseToggleButtonDown
            // 
            this.baseToggleButtonDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseToggleButtonDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButtonDown.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseToggleButtonDown.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseToggleButtonDown.Location = new System.Drawing.Point(123, 21);
            this.baseToggleButtonDown.Name = "baseToggleButtonDown";
            this.baseToggleButtonDown.Size = new System.Drawing.Size(100, 30);
            this.baseToggleButtonDown.TabIndex = 1;
            this.baseToggleButtonDown.Text = "Down";
            this.baseToggleButtonDown.UseVisualStyleBackColor = false;
            this.baseToggleButtonDown.Click += new System.EventHandler(this.baseToggleButtonDown_Click);
            // 
            // baseToggleButtonUp
            // 
            this.baseToggleButtonUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseToggleButtonUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButtonUp.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseToggleButtonUp.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseToggleButtonUp.Location = new System.Drawing.Point(7, 21);
            this.baseToggleButtonUp.Name = "baseToggleButtonUp";
            this.baseToggleButtonUp.Size = new System.Drawing.Size(100, 30);
            this.baseToggleButtonUp.TabIndex = 0;
            this.baseToggleButtonUp.Text = "Up";
            this.baseToggleButtonUp.UseVisualStyleBackColor = false;
            this.baseToggleButtonUp.Click += new System.EventHandler(this.baseToggleButtonUp_Click);
            // 
            // CylinderZControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseGroupBoxMain);
            this.Name = "CylinderZControl";
            this.Size = new System.Drawing.Size(227, 65);
            this.baseGroupBoxMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private BaseGroupBox baseGroupBoxMain;
        private BaseToggleButton baseToggleButtonUp;
        private BaseToggleButton baseToggleButtonDown;
    }
}
