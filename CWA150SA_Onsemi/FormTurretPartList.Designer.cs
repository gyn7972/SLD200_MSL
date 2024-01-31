namespace CWA150SA_Onsemi300
{
    partial class FormTurretPartList
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
            this.baseButtonCancel = new CWA150SA_Onsemi300.BaseButton();
            this.baseButtonOk = new CWA150SA_Onsemi300.BaseButton();
            this.baseTreeViewTurretPartList = new CWA150SA_Onsemi300.BaseTreeView();
            this.SuspendLayout();
            // 
            // baseButtonCancel
            // 
            this.baseButtonCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonCancel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonCancel.Location = new System.Drawing.Point(212, 389);
            this.baseButtonCancel.Name = "baseButtonCancel";
            this.baseButtonCancel.Size = new System.Drawing.Size(100, 30);
            this.baseButtonCancel.TabIndex = 1;
            this.baseButtonCancel.Text = "baseButton1";
            this.baseButtonCancel.UseVisualStyleBackColor = false;
            // 
            // baseButtonOk
            // 
            this.baseButtonOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonOk.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonOk.Location = new System.Drawing.Point(12, 389);
            this.baseButtonOk.Name = "baseButtonOk";
            this.baseButtonOk.Size = new System.Drawing.Size(100, 30);
            this.baseButtonOk.TabIndex = 1;
            this.baseButtonOk.Text = "baseButton1";
            this.baseButtonOk.UseVisualStyleBackColor = false;
            // 
            // baseTreeViewTurretPartList
            // 
            this.baseTreeViewTurretPartList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.baseTreeViewTurretPartList.Font = new System.Drawing.Font("Arial", 16F);
            this.baseTreeViewTurretPartList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            this.baseTreeViewTurretPartList.Location = new System.Drawing.Point(12, 12);
            this.baseTreeViewTurretPartList.Name = "baseTreeViewTurretPartList";
            this.baseTreeViewTurretPartList.Size = new System.Drawing.Size(300, 353);
            this.baseTreeViewTurretPartList.TabIndex = 0;
            // 
            // FormTurretPartList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 450);
            this.Controls.Add(this.baseButtonCancel);
            this.Controls.Add(this.baseButtonOk);
            this.Controls.Add(this.baseTreeViewTurretPartList);
            this.Name = "FormTurretPartList";
            this.Text = "FormTurretPartList";
            this.ResumeLayout(false);

        }

        #endregion

        private BaseTreeView baseTreeViewTurretPartList;
        private BaseButton baseButtonOk;
        private BaseButton baseButtonCancel;
    }
}