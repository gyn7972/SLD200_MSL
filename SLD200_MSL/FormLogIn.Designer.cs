namespace SLD200_MSL
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
            this.baseButtonCancel = new SLD200_MSL.BaseButton();
            this.baseButtonOk = new SLD200_MSL.BaseButton();
            this.baseTextBoxExplain = new SLD200_MSL.BaseTextBox();
            this.baseTextBoxMain = new SLD200_MSL.BaseTextBox();
            this.baseTextBoxEnterPW = new SLD200_MSL.BaseTextBox();
            this.baseTextBoxEnterID = new SLD200_MSL.BaseTextBox();
            this.baseLabelEnterPW = new SLD200_MSL.BaseLabel();
            this.baseLabelEnterID = new SLD200_MSL.BaseLabel();
            this.SuspendLayout();
            // 
            // baseButtonCancel
            // 
            this.baseButtonCancel.BackColor = System.Drawing.Color.White;
            this.baseButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.baseButtonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonCancel.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseButtonCancel.ForeColor = System.Drawing.Color.Black;
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
            this.baseButtonOk.BackColor = System.Drawing.Color.White;
            this.baseButtonOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonOk.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseButtonOk.ForeColor = System.Drawing.Color.Black;
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
            this.baseTextBoxExplain.BackColor = System.Drawing.SystemColors.Control;
            this.baseTextBoxExplain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxExplain.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.baseTextBoxExplain.ForeColor = System.Drawing.Color.Black;
            this.baseTextBoxExplain.Location = new System.Drawing.Point(83, 71);
            this.baseTextBoxExplain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseTextBoxExplain.Name = "baseTextBoxExplain";
            this.baseTextBoxExplain.ReadOnly = true;
            this.baseTextBoxExplain.Size = new System.Drawing.Size(234, 15);
            this.baseTextBoxExplain.TabIndex = 7;
            this.baseTextBoxExplain.TabStop = false;
            this.baseTextBoxExplain.Text = "LogIn to Your Account";
            this.baseTextBoxExplain.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseTextBoxMain
            // 
            this.baseTextBoxMain.BackColor = System.Drawing.SystemColors.Control;
            this.baseTextBoxMain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxMain.Font = new System.Drawing.Font("Tahoma", 9F);
            this.baseTextBoxMain.ForeColor = System.Drawing.Color.Black;
            this.baseTextBoxMain.Location = new System.Drawing.Point(83, 43);
            this.baseTextBoxMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseTextBoxMain.Name = "baseTextBoxMain";
            this.baseTextBoxMain.Size = new System.Drawing.Size(137, 15);
            this.baseTextBoxMain.TabIndex = 2;
            this.baseTextBoxMain.TabStop = false;
            this.baseTextBoxMain.Text = "Log In";
            this.baseTextBoxMain.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseTextBoxEnterPW
            // 
            this.baseTextBoxEnterPW.BackColor = System.Drawing.Color.White;
            this.baseTextBoxEnterPW.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxEnterPW.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseTextBoxEnterPW.ForeColor = System.Drawing.Color.Black;
            this.baseTextBoxEnterPW.Location = new System.Drawing.Point(140, 170);
            this.baseTextBoxEnterPW.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseTextBoxEnterPW.Name = "baseTextBoxEnterPW";
            this.baseTextBoxEnterPW.PasswordChar = '*';
            this.baseTextBoxEnterPW.Size = new System.Drawing.Size(134, 20);
            this.baseTextBoxEnterPW.TabIndex = 1;
            this.baseTextBoxEnterPW.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseTextBoxEnterID
            // 
            this.baseTextBoxEnterID.BackColor = System.Drawing.Color.White;
            this.baseTextBoxEnterID.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxEnterID.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseTextBoxEnterID.ForeColor = System.Drawing.Color.Black;
            this.baseTextBoxEnterID.Location = new System.Drawing.Point(140, 135);
            this.baseTextBoxEnterID.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseTextBoxEnterID.Name = "baseTextBoxEnterID";
            this.baseTextBoxEnterID.Size = new System.Drawing.Size(134, 20);
            this.baseTextBoxEnterID.TabIndex = 0;
            this.baseTextBoxEnterID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseLabelEnterPW
            // 
            this.baseLabelEnterPW.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabelEnterPW.ForeColor = System.Drawing.Color.Black;
            this.baseLabelEnterPW.Location = new System.Drawing.Point(21, 168);
            this.baseLabelEnterPW.Name = "baseLabelEnterPW";
            this.baseLabelEnterPW.Size = new System.Drawing.Size(127, 20);
            this.baseLabelEnterPW.TabIndex = 0;
            this.baseLabelEnterPW.Text = "Password : ";
            // 
            // baseLabelEnterID
            // 
            this.baseLabelEnterID.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabelEnterID.ForeColor = System.Drawing.Color.Black;
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
            this.BackColor = System.Drawing.SystemColors.Control;
            this.CancelButton = this.baseButtonCancel;
            this.ClientSize = new System.Drawing.Size(394, 315);
            this.Controls.Add(this.baseButtonCancel);
            this.Controls.Add(this.baseButtonOk);
            this.Controls.Add(this.baseTextBoxExplain);
            this.Controls.Add(this.baseTextBoxMain);
            this.Controls.Add(this.baseTextBoxEnterPW);
            this.Controls.Add(this.baseTextBoxEnterID);
            this.Controls.Add(this.baseLabelEnterPW);
            this.Controls.Add(this.baseLabelEnterID);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
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