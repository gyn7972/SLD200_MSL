namespace SLD200_MSL
{
    partial class FormAddRecipeInfo
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
            this.baseLabelDescription = new SLD200_MSL.BaseLabel();
            this.baseLabelRecipeName = new SLD200_MSL.BaseLabel();
            this.baseTextBoxDescription = new SLD200_MSL.BaseTextBox();
            this.baseTextBoxCreateRecipeName = new SLD200_MSL.BaseTextBox();
            this.baseButtonCancel = new SLD200_MSL.BaseButton();
            this.baseButtonAdd = new SLD200_MSL.BaseButton();
            this.SuspendLayout();
            // 
            // baseLabelDescription
            // 
            this.baseLabelDescription.AutoSize = true;
            this.baseLabelDescription.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelDescription.ForeColor = System.Drawing.Color.White;
            this.baseLabelDescription.Location = new System.Drawing.Point(75, 91);
            this.baseLabelDescription.Name = "baseLabelDescription";
            this.baseLabelDescription.Size = new System.Drawing.Size(137, 20);
            this.baseLabelDescription.TabIndex = 2;
            this.baseLabelDescription.Text = "Description : ";
            // 
            // baseLabelRecipeName
            // 
            this.baseLabelRecipeName.AutoSize = true;
            this.baseLabelRecipeName.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelRecipeName.ForeColor = System.Drawing.Color.White;
            this.baseLabelRecipeName.Location = new System.Drawing.Point(75, 47);
            this.baseLabelRecipeName.Name = "baseLabelRecipeName";
            this.baseLabelRecipeName.Size = new System.Drawing.Size(157, 20);
            this.baseLabelRecipeName.TabIndex = 2;
            this.baseLabelRecipeName.Text = "Recipe Name : ";
            // 
            // baseTextBoxDescription
            // 
            this.baseTextBoxDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxDescription.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxDescription.Location = new System.Drawing.Point(229, 89);
            this.baseTextBoxDescription.Name = "baseTextBoxDescription";
            this.baseTextBoxDescription.Size = new System.Drawing.Size(196, 18);
            this.baseTextBoxDescription.TabIndex = 1;
            // 
            // baseTextBoxCreateRecipeName
            // 
            this.baseTextBoxCreateRecipeName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxCreateRecipeName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxCreateRecipeName.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxCreateRecipeName.Location = new System.Drawing.Point(229, 43);
            this.baseTextBoxCreateRecipeName.Name = "baseTextBoxCreateRecipeName";
            this.baseTextBoxCreateRecipeName.Size = new System.Drawing.Size(196, 18);
            this.baseTextBoxCreateRecipeName.TabIndex = 1;
            // 
            // baseButtonCancel
            // 
            this.baseButtonCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonCancel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonCancel.Location = new System.Drawing.Point(279, 150);
            this.baseButtonCancel.Name = "baseButtonCancel";
            this.baseButtonCancel.Size = new System.Drawing.Size(134, 57);
            this.baseButtonCancel.TabIndex = 0;
            this.baseButtonCancel.Text = "baseButton1";
            this.baseButtonCancel.UseVisualStyleBackColor = false;
            this.baseButtonCancel.Click += new System.EventHandler(this.baseButtonCancel_Click);
            // 
            // baseButtonAdd
            // 
            this.baseButtonAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonAdd.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonAdd.Location = new System.Drawing.Point(86, 150);
            this.baseButtonAdd.Name = "baseButtonAdd";
            this.baseButtonAdd.Size = new System.Drawing.Size(134, 57);
            this.baseButtonAdd.TabIndex = 0;
            this.baseButtonAdd.Text = "baseButton1";
            this.baseButtonAdd.UseVisualStyleBackColor = false;
            this.baseButtonAdd.Click += new System.EventHandler(this.baseButtonOK_Click);
            // 
            // FormAddRecipeInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(488, 219);
            this.Controls.Add(this.baseLabelDescription);
            this.Controls.Add(this.baseLabelRecipeName);
            this.Controls.Add(this.baseTextBoxDescription);
            this.Controls.Add(this.baseTextBoxCreateRecipeName);
            this.Controls.Add(this.baseButtonCancel);
            this.Controls.Add(this.baseButtonAdd);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormAddRecipeInfo";
            this.Text = "FormCreateRecipeName";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BaseButton baseButtonAdd;
        private BaseButton baseButtonCancel;
        private BaseTextBox baseTextBoxCreateRecipeName;
        private BaseLabel baseLabelRecipeName;
        private BaseTextBox baseTextBoxDescription;
        private BaseLabel baseLabelDescription;
    }
}