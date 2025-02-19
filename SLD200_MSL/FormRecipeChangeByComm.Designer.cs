namespace SLD200_MSL
{
    partial class FormRecipeChangeByComm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_Close = new System.Windows.Forms.Button();
            this.baseLabel_RecipeList = new SLD200_MSL.BaseLabel();
            this.btn_Reload = new System.Windows.Forms.Button();
            this.btn_ListSave = new System.Windows.Forms.Button();
            this.dataGridView_RecipeCodeName = new System.Windows.Forms.DataGridView();
            this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RecipeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CodeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_RecipeCodeName)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_Close
            // 
            this.btn_Close.BackColor = System.Drawing.Color.DarkGray;
            this.btn_Close.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btn_Close.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn_Close.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btn_Close.Font = new System.Drawing.Font("나눔바른고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Close.ForeColor = System.Drawing.Color.DarkRed;
            this.btn_Close.Location = new System.Drawing.Point(743, 388);
            this.btn_Close.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(143, 58);
            this.btn_Close.TabIndex = 127;
            this.btn_Close.Text = "창 닫기";
            this.btn_Close.UseVisualStyleBackColor = false;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // baseLabel_RecipeList
            // 
            this.baseLabel_RecipeList.BackColor = System.Drawing.Color.Black;
            this.baseLabel_RecipeList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabel_RecipeList.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_RecipeList.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel_RecipeList.Location = new System.Drawing.Point(12, 12);
            this.baseLabel_RecipeList.Name = "baseLabel_RecipeList";
            this.baseLabel_RecipeList.Size = new System.Drawing.Size(711, 30);
            this.baseLabel_RecipeList.TabIndex = 217;
            this.baseLabel_RecipeList.Text = "레시피 && 코드네임 리스트";
            this.baseLabel_RecipeList.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_Reload
            // 
            this.btn_Reload.BackColor = System.Drawing.Color.DarkGray;
            this.btn_Reload.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btn_Reload.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn_Reload.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btn_Reload.Font = new System.Drawing.Font("나눔바른고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Reload.ForeColor = System.Drawing.Color.DarkRed;
            this.btn_Reload.Location = new System.Drawing.Point(743, 48);
            this.btn_Reload.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_Reload.Name = "btn_Reload";
            this.btn_Reload.Size = new System.Drawing.Size(143, 58);
            this.btn_Reload.TabIndex = 218;
            this.btn_Reload.Text = "리스트 갱신";
            this.btn_Reload.UseVisualStyleBackColor = false;
            this.btn_Reload.Click += new System.EventHandler(this.btn_Reload_Click);
            // 
            // btn_ListSave
            // 
            this.btn_ListSave.BackColor = System.Drawing.Color.DarkGray;
            this.btn_ListSave.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btn_ListSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn_ListSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btn_ListSave.Font = new System.Drawing.Font("나눔바른고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_ListSave.ForeColor = System.Drawing.Color.DarkRed;
            this.btn_ListSave.Location = new System.Drawing.Point(743, 112);
            this.btn_ListSave.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_ListSave.Name = "btn_ListSave";
            this.btn_ListSave.Size = new System.Drawing.Size(143, 58);
            this.btn_ListSave.TabIndex = 219;
            this.btn_ListSave.Text = "리스트 저장";
            this.btn_ListSave.UseVisualStyleBackColor = false;
            this.btn_ListSave.Click += new System.EventHandler(this.btn_ListSave_Click);
            // 
            // dataGridView_RecipeCodeName
            // 
            this.dataGridView_RecipeCodeName.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_RecipeCodeName.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.No,
            this.RecipeName,
            this.CodeName});
            this.dataGridView_RecipeCodeName.Location = new System.Drawing.Point(12, 48);
            this.dataGridView_RecipeCodeName.Name = "dataGridView_RecipeCodeName";
            this.dataGridView_RecipeCodeName.RowTemplate.Height = 23;
            this.dataGridView_RecipeCodeName.Size = new System.Drawing.Size(711, 398);
            this.dataGridView_RecipeCodeName.TabIndex = 220;
            // 
            // No
            // 
            this.No.HeaderText = "순번";
            this.No.Name = "No";
            this.No.Width = 70;
            // 
            // RecipeName
            // 
            this.RecipeName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.RecipeName.HeaderText = "레시피";
            this.RecipeName.Name = "RecipeName";
            // 
            // CodeName
            // 
            this.CodeName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CodeName.HeaderText = "레시피 대응 코드네임";
            this.CodeName.Name = "CodeName";
            // 
            // FormRecipeChangeByComm
            // 
            this.BackColor = System.Drawing.Color.DarkSlateGray;
            this.ClientSize = new System.Drawing.Size(899, 458);
            this.Controls.Add(this.dataGridView_RecipeCodeName);
            this.Controls.Add(this.btn_ListSave);
            this.Controls.Add(this.btn_Reload);
            this.Controls.Add(this.baseLabel_RecipeList);
            this.Controls.Add(this.btn_Close);
            this.ForeColor = System.Drawing.Color.Coral;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormRecipeChangeByComm";
            this.Text = "레시피 & 코드네임 리스트";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_RecipeCodeName)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_Close;
        private BaseLabel baseLabel_RecipeList;
        private System.Windows.Forms.Button btn_Reload;
        private System.Windows.Forms.Button btn_ListSave;
        private System.Windows.Forms.DataGridView dataGridView_RecipeCodeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn No;
        private System.Windows.Forms.DataGridViewTextBoxColumn RecipeName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CodeName;
    }
}

