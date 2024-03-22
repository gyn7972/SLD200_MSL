namespace CWA150SA_Onsemi300
{
    partial class FormRecipeMain
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
            this.baseTextBoxRecipeName = new CWA150SA_Onsemi300.BaseTextBox();
            this.baseButtonLoad = new CWA150SA_Onsemi300.BaseButton();
            this.baseButtonSave = new CWA150SA_Onsemi300.BaseButton();
            this.baseButtonAssign = new CWA150SA_Onsemi300.BaseButton();
            this.baseButtonList = new CWA150SA_Onsemi300.BaseButton();
            this.baseLabelRecipe = new CWA150SA_Onsemi300.BaseLabel();
            this.panelContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelContent
            // 
            this.panelContent.Controls.Add(this.baseLabelRecipe);
            this.panelContent.Controls.Add(this.baseButtonList);
            this.panelContent.Controls.Add(this.baseButtonAssign);
            this.panelContent.Controls.Add(this.baseButtonSave);
            this.panelContent.Controls.Add(this.baseButtonLoad);
            this.panelContent.Controls.Add(this.baseTextBoxRecipeName);
            // 
            // baseTextBoxRecipeName
            // 
            this.baseTextBoxRecipeName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxRecipeName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxRecipeName.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxRecipeName.Location = new System.Drawing.Point(153, 352);
            this.baseTextBoxRecipeName.Multiline = true;
            this.baseTextBoxRecipeName.Name = "baseTextBoxRecipeName";
            this.baseTextBoxRecipeName.ReadOnly = true;
            this.baseTextBoxRecipeName.Size = new System.Drawing.Size(281, 30);
            this.baseTextBoxRecipeName.TabIndex = 0;
            // 
            // baseButtonLoad
            // 
            this.baseButtonLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonLoad.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonLoad.Location = new System.Drawing.Point(567, 430);
            this.baseButtonLoad.Name = "baseButtonLoad";
            this.baseButtonLoad.Size = new System.Drawing.Size(100, 30);
            this.baseButtonLoad.TabIndex = 1;
            this.baseButtonLoad.Text = "Load";
            this.baseButtonLoad.UseVisualStyleBackColor = false;
            this.baseButtonLoad.Click += new System.EventHandler(this.baseButtonLoad_Click);
            // 
            // baseButtonSave
            // 
            this.baseButtonSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonSave.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonSave.Location = new System.Drawing.Point(683, 430);
            this.baseButtonSave.Name = "baseButtonSave";
            this.baseButtonSave.Size = new System.Drawing.Size(100, 30);
            this.baseButtonSave.TabIndex = 2;
            this.baseButtonSave.Text = "Save";
            this.baseButtonSave.UseVisualStyleBackColor = false;
            this.baseButtonSave.Click += new System.EventHandler(this.baseButtonSave_Click);
            // 
            // baseButtonAssign
            // 
            this.baseButtonAssign.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonAssign.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonAssign.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonAssign.Location = new System.Drawing.Point(459, 430);
            this.baseButtonAssign.Name = "baseButtonAssign";
            this.baseButtonAssign.Size = new System.Drawing.Size(100, 30);
            this.baseButtonAssign.TabIndex = 3;
            this.baseButtonAssign.Text = "Assign";
            this.baseButtonAssign.UseVisualStyleBackColor = false;
            this.baseButtonAssign.Click += new System.EventHandler(this.baseButtonAssign_Click);
            // 
            // baseButtonList
            // 
            this.baseButtonList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonList.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonList.Location = new System.Drawing.Point(353, 430);
            this.baseButtonList.Name = "baseButtonList";
            this.baseButtonList.Size = new System.Drawing.Size(100, 30);
            this.baseButtonList.TabIndex = 4;
            this.baseButtonList.Text = "List";
            this.baseButtonList.UseVisualStyleBackColor = false;
            this.baseButtonList.Click += new System.EventHandler(this.baseButtonList_Click);
            // 
            // baseLabelRecipe
            // 
            this.baseLabelRecipe.AutoSize = true;
            this.baseLabelRecipe.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelRecipe.ForeColor = System.Drawing.Color.White;
            this.baseLabelRecipe.Location = new System.Drawing.Point(33, 352);
            this.baseLabelRecipe.Name = "baseLabelRecipe";
            this.baseLabelRecipe.Size = new System.Drawing.Size(110, 19);
            this.baseLabelRecipe.TabIndex = 5;
            this.baseLabelRecipe.Text = "Recipe Name";
            // 
            // FormRecipeMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1942, 993);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormRecipeMain";
            this.Text = "FormRecipe";
            this.panelContent.ResumeLayout(false);
            this.panelContent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private BaseButton baseButtonList;
        private BaseButton baseButtonAssign;
        private BaseButton baseButtonSave;
        private BaseButton baseButtonLoad;
        private BaseTextBox baseTextBoxRecipeName;
        private BaseLabel baseLabelRecipe;
    }
}