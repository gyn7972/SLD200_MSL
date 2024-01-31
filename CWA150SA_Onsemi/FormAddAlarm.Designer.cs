namespace CWA150SA_Onsemi300
{
    partial class FormAddAlarm
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
            this.baseLabelGrade = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelTitle = new CWA150SA_Onsemi300.BaseLabel();
            this.buttonCancel = new CWA150SA_Onsemi300.BaseButton();
            this.buttonApply = new CWA150SA_Onsemi300.BaseButton();
            this.baseLabelSource = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelCode = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelCause = new CWA150SA_Onsemi300.BaseLabel();
            this.baseTextBoxTitle = new CWA150SA_Onsemi300.BaseTextBox();
            this.baseTextBoxGrade = new CWA150SA_Onsemi300.BaseTextBox();
            this.baseTextBoxSource = new CWA150SA_Onsemi300.BaseTextBox();
            this.baseTextBoxCode = new CWA150SA_Onsemi300.BaseTextBox();
            this.baseTextBoxCause = new CWA150SA_Onsemi300.BaseTextBox();
            this.SuspendLayout();
            // 
            // baseLabelGrade
            // 
            this.baseLabelGrade.AutoSize = true;
            this.baseLabelGrade.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelGrade.ForeColor = System.Drawing.Color.White;
            this.baseLabelGrade.Location = new System.Drawing.Point(12, 85);
            this.baseLabelGrade.Name = "baseLabelGrade";
            this.baseLabelGrade.Size = new System.Drawing.Size(56, 16);
            this.baseLabelGrade.TabIndex = 7;
            this.baseLabelGrade.Text = "Grade";
            // 
            // baseLabelTitle
            // 
            this.baseLabelTitle.AutoSize = true;
            this.baseLabelTitle.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelTitle.ForeColor = System.Drawing.Color.White;
            this.baseLabelTitle.Location = new System.Drawing.Point(12, 19);
            this.baseLabelTitle.Name = "baseLabelTitle";
            this.baseLabelTitle.Size = new System.Drawing.Size(41, 16);
            this.baseLabelTitle.TabIndex = 6;
            this.baseLabelTitle.Text = "Title";
            // 
            // buttonCancel
            // 
            this.buttonCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonCancel.Location = new System.Drawing.Point(713, 41);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonApply
            // 
            this.buttonApply.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.buttonApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonApply.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonApply.Location = new System.Drawing.Point(713, 12);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(75, 23);
            this.buttonApply.TabIndex = 4;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // baseLabelSource
            // 
            this.baseLabelSource.AutoSize = true;
            this.baseLabelSource.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelSource.ForeColor = System.Drawing.Color.White;
            this.baseLabelSource.Location = new System.Drawing.Point(12, 156);
            this.baseLabelSource.Name = "baseLabelSource";
            this.baseLabelSource.Size = new System.Drawing.Size(65, 16);
            this.baseLabelSource.TabIndex = 8;
            this.baseLabelSource.Text = "Source";
            // 
            // baseLabelCode
            // 
            this.baseLabelCode.AutoSize = true;
            this.baseLabelCode.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelCode.ForeColor = System.Drawing.Color.White;
            this.baseLabelCode.Location = new System.Drawing.Point(12, 227);
            this.baseLabelCode.Name = "baseLabelCode";
            this.baseLabelCode.Size = new System.Drawing.Size(50, 16);
            this.baseLabelCode.TabIndex = 9;
            this.baseLabelCode.Text = "Code";
            // 
            // baseLabelCause
            // 
            this.baseLabelCause.AutoSize = true;
            this.baseLabelCause.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelCause.ForeColor = System.Drawing.Color.White;
            this.baseLabelCause.Location = new System.Drawing.Point(13, 287);
            this.baseLabelCause.Name = "baseLabelCause";
            this.baseLabelCause.Size = new System.Drawing.Size(59, 16);
            this.baseLabelCause.TabIndex = 10;
            this.baseLabelCause.Text = "Cause";
            // 
            // baseTextBoxTitle
            // 
            this.baseTextBoxTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxTitle.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxTitle.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxTitle.Location = new System.Drawing.Point(135, 19);
            this.baseTextBoxTitle.Multiline = true;
            this.baseTextBoxTitle.Name = "baseTextBoxTitle";
            this.baseTextBoxTitle.Size = new System.Drawing.Size(548, 45);
            this.baseTextBoxTitle.TabIndex = 11;
            // 
            // baseTextBoxGrade
            // 
            this.baseTextBoxGrade.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxGrade.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxGrade.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxGrade.Location = new System.Drawing.Point(135, 89);
            this.baseTextBoxGrade.Multiline = true;
            this.baseTextBoxGrade.Name = "baseTextBoxGrade";
            this.baseTextBoxGrade.Size = new System.Drawing.Size(548, 45);
            this.baseTextBoxGrade.TabIndex = 12;
            // 
            // baseTextBoxSource
            // 
            this.baseTextBoxSource.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxSource.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxSource.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxSource.Location = new System.Drawing.Point(135, 160);
            this.baseTextBoxSource.Multiline = true;
            this.baseTextBoxSource.Name = "baseTextBoxSource";
            this.baseTextBoxSource.Size = new System.Drawing.Size(548, 45);
            this.baseTextBoxSource.TabIndex = 13;
            // 
            // baseTextBoxCode
            // 
            this.baseTextBoxCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxCode.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxCode.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxCode.Location = new System.Drawing.Point(135, 227);
            this.baseTextBoxCode.Multiline = true;
            this.baseTextBoxCode.Name = "baseTextBoxCode";
            this.baseTextBoxCode.Size = new System.Drawing.Size(548, 45);
            this.baseTextBoxCode.TabIndex = 14;
            // 
            // baseTextBoxCause
            // 
            this.baseTextBoxCause.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxCause.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxCause.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxCause.Location = new System.Drawing.Point(135, 287);
            this.baseTextBoxCause.Multiline = true;
            this.baseTextBoxCause.Name = "baseTextBoxCause";
            this.baseTextBoxCause.Size = new System.Drawing.Size(548, 45);
            this.baseTextBoxCause.TabIndex = 15;
            // 
            // FormAddAlarm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 355);
            this.Controls.Add(this.baseTextBoxCause);
            this.Controls.Add(this.baseTextBoxCode);
            this.Controls.Add(this.baseTextBoxSource);
            this.Controls.Add(this.baseTextBoxGrade);
            this.Controls.Add(this.baseTextBoxTitle);
            this.Controls.Add(this.baseLabelCause);
            this.Controls.Add(this.baseLabelCode);
            this.Controls.Add(this.baseLabelSource);
            this.Controls.Add(this.baseLabelGrade);
            this.Controls.Add(this.baseLabelTitle);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonApply);
            this.Name = "FormAddAlarm";
            this.Text = "FormAddAlarm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BaseButton buttonCancel;
        private BaseButton buttonApply;
        private BaseLabel baseLabelTitle;
        private BaseLabel baseLabelGrade;
        private BaseLabel baseLabelSource;
        private BaseLabel baseLabelCode;
        private BaseLabel baseLabelCause;
        private BaseTextBox baseTextBoxTitle;
        private BaseTextBox baseTextBoxGrade;
        private BaseTextBox baseTextBoxSource;
        private BaseTextBox baseTextBoxCode;
        private BaseTextBox baseTextBoxCause;
    }
}