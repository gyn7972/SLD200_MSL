namespace CWA150SA_Onsemi300
{
    partial class FormLogIn
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
            this.baseTextBoxExplain = new CWA150SA_Onsemi300.BaseTextBox();
            this.baseTextBoxMain = new CWA150SA_Onsemi300.BaseTextBox();
            this.baseTextBoxEnterPW = new CWA150SA_Onsemi300.BaseTextBox();
            this.baseTextBoxEnterID = new CWA150SA_Onsemi300.BaseTextBox();
            this.baseLabelEnterPW = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelEnterID = new CWA150SA_Onsemi300.BaseLabel();
            this.SuspendLayout();
            // 
            // baseButtonCancel
            // 
            this.baseButtonCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.baseButtonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonCancel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonCancel.Location = new System.Drawing.Point(206, 209);
            this.baseButtonCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseButtonCancel.Name = "baseButtonCancel";
            this.baseButtonCancel.Size = new System.Drawing.Size(48, 38);
            this.baseButtonCancel.TabIndex = 4;
            this.baseButtonCancel.Text = "baseButton1";
            this.baseButtonCancel.UseVisualStyleBackColor = false;
            this.baseButtonCancel.Click += new System.EventHandler(this.baseButtonCancel_Click);
            // 
            // baseButtonOk
            // 
            this.baseButtonOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonOk.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonOk.Location = new System.Drawing.Point(82, 209);
            this.baseButtonOk.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseButtonOk.Name = "baseButtonOk";
            this.baseButtonOk.Size = new System.Drawing.Size(48, 38);
            this.baseButtonOk.TabIndex = 3;
            this.baseButtonOk.Text = "baseButton1";
            this.baseButtonOk.UseVisualStyleBackColor = false;
            this.baseButtonOk.Click += new System.EventHandler(this.baseButtonOk_Click);
            // 
            // baseTextBoxExplain
            // 
            this.baseTextBoxExplain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxExplain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxExplain.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseTextBoxExplain.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxExplain.Location = new System.Drawing.Point(83, 71);
            this.baseTextBoxExplain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseTextBoxExplain.Name = "baseTextBoxExplain";
            this.baseTextBoxExplain.ReadOnly = true;
            this.baseTextBoxExplain.Size = new System.Drawing.Size(234, 14);
            this.baseTextBoxExplain.TabIndex = 7;
            this.baseTextBoxExplain.TabStop = false;
            this.baseTextBoxExplain.Text = "LogIn to Your Account";
            this.baseTextBoxExplain.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseTextBoxMain
            // 
            this.baseTextBoxMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxMain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxMain.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxMain.Location = new System.Drawing.Point(82, 43);
            this.baseTextBoxMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseTextBoxMain.Name = "baseTextBoxMain";
            this.baseTextBoxMain.Size = new System.Drawing.Size(137, 14);
            this.baseTextBoxMain.TabIndex = 2;
            this.baseTextBoxMain.TabStop = false;
            this.baseTextBoxMain.Text = "Log In";
            this.baseTextBoxMain.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseTextBoxEnterPW
            // 
            this.baseTextBoxEnterPW.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxEnterPW.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxEnterPW.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxEnterPW.Location = new System.Drawing.Point(140, 170);
            this.baseTextBoxEnterPW.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseTextBoxEnterPW.Name = "baseTextBoxEnterPW";
            this.baseTextBoxEnterPW.PasswordChar = '*';
            this.baseTextBoxEnterPW.Size = new System.Drawing.Size(186, 14);
            this.baseTextBoxEnterPW.TabIndex = 1;
            // 
            // baseTextBoxEnterID
            // 
            this.baseTextBoxEnterID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxEnterID.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxEnterID.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxEnterID.Location = new System.Drawing.Point(140, 135);
            this.baseTextBoxEnterID.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseTextBoxEnterID.Name = "baseTextBoxEnterID";
            this.baseTextBoxEnterID.Size = new System.Drawing.Size(186, 14);
            this.baseTextBoxEnterID.TabIndex = 0;
            // 
            // baseLabelEnterPW
            // 
            this.baseLabelEnterPW.Font = new System.Drawing.Font("Arial", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelEnterPW.ForeColor = System.Drawing.Color.White;
            this.baseLabelEnterPW.Location = new System.Drawing.Point(21, 168);
            this.baseLabelEnterPW.Name = "baseLabelEnterPW";
            this.baseLabelEnterPW.Size = new System.Drawing.Size(127, 20);
            this.baseLabelEnterPW.TabIndex = 0;
            this.baseLabelEnterPW.Text = "Password : ";
            // 
            // baseLabelEnterID
            // 
            this.baseLabelEnterID.Font = new System.Drawing.Font("Arial", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelEnterID.ForeColor = System.Drawing.Color.White;
            this.baseLabelEnterID.Location = new System.Drawing.Point(31, 117);
            this.baseLabelEnterID.Name = "baseLabelEnterID";
            this.baseLabelEnterID.Size = new System.Drawing.Size(103, 48);
            this.baseLabelEnterID.TabIndex = 0;
            this.baseLabelEnterID.Text = "ID : ";
            // 
            // FormLogIn
            // 
            this.AcceptButton = this.baseButtonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.baseButtonCancel;
            this.ClientSize = new System.Drawing.Size(791, 828);
            this.Controls.Add(this.baseButtonCancel);
            this.Controls.Add(this.baseButtonOk);
            this.Controls.Add(this.baseTextBoxExplain);
            this.Controls.Add(this.baseTextBoxMain);
            this.Controls.Add(this.baseTextBoxEnterPW);
            this.Controls.Add(this.baseTextBoxEnterID);
            this.Controls.Add(this.baseLabelEnterPW);
            this.Controls.Add(this.baseLabelEnterID);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormLogIn";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormLogIn";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormLogIn_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BaseLabel baseLabelEnterID;
        private BaseLabel baseLabelEnterPW;
        private BaseTextBox baseTextBoxEnterID;
        private BaseTextBox baseTextBoxEnterPW;
        private BaseTextBox baseTextBoxMain;
        private BaseTextBox baseTextBoxExplain;
        private BaseButton baseButtonOk;
        private BaseButton baseButtonCancel;
    }
}