namespace CWA150SA_Onsemi300
{
    partial class FormUserRegistration
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
            this.listView_Operator = new System.Windows.Forms.ListView();
            this.groupBox_Operator_Information = new System.Windows.Forms.GroupBox();
            this.checkBox_Password_Visible = new System.Windows.Forms.CheckBox();
            this.radioButton_Administrator = new System.Windows.Forms.RadioButton();
            this.radioButton_Operator = new System.Windows.Forms.RadioButton();
            this.baseLabel_Authority = new CWA150SA_Onsemi300.BaseLabel();
            this.btn_User_Del = new System.Windows.Forms.Button();
            this.btn_User_Add = new System.Windows.Forms.Button();
            this.tb_Password = new System.Windows.Forms.TextBox();
            this.baseLabel_Password = new CWA150SA_Onsemi300.BaseLabel();
            this.tb_ID = new System.Windows.Forms.TextBox();
            this.baseLabel_ID = new CWA150SA_Onsemi300.BaseLabel();
            this.tb_Name = new System.Windows.Forms.TextBox();
            this.baseLabel_Name = new CWA150SA_Onsemi300.BaseLabel();
            this.groupBox_Operator_Information.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView_Operator
            // 
            this.listView_Operator.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.listView_Operator.HideSelection = false;
            this.listView_Operator.Location = new System.Drawing.Point(12, 12);
            this.listView_Operator.Name = "listView_Operator";
            this.listView_Operator.Size = new System.Drawing.Size(714, 426);
            this.listView_Operator.TabIndex = 0;
            this.listView_Operator.UseCompatibleStateImageBehavior = false;
            this.listView_Operator.SelectedIndexChanged += new System.EventHandler(this.listView_Operator_SelectedIndexChanged);
            // 
            // groupBox_Operator_Information
            // 
            this.groupBox_Operator_Information.BackColor = System.Drawing.Color.Lavender;
            this.groupBox_Operator_Information.Controls.Add(this.checkBox_Password_Visible);
            this.groupBox_Operator_Information.Controls.Add(this.radioButton_Administrator);
            this.groupBox_Operator_Information.Controls.Add(this.radioButton_Operator);
            this.groupBox_Operator_Information.Controls.Add(this.baseLabel_Authority);
            this.groupBox_Operator_Information.Controls.Add(this.btn_User_Del);
            this.groupBox_Operator_Information.Controls.Add(this.btn_User_Add);
            this.groupBox_Operator_Information.Controls.Add(this.tb_Password);
            this.groupBox_Operator_Information.Controls.Add(this.baseLabel_Password);
            this.groupBox_Operator_Information.Controls.Add(this.tb_ID);
            this.groupBox_Operator_Information.Controls.Add(this.baseLabel_ID);
            this.groupBox_Operator_Information.Controls.Add(this.tb_Name);
            this.groupBox_Operator_Information.Controls.Add(this.baseLabel_Name);
            this.groupBox_Operator_Information.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_Operator_Information.ForeColor = System.Drawing.Color.Black;
            this.groupBox_Operator_Information.Location = new System.Drawing.Point(755, 12);
            this.groupBox_Operator_Information.Name = "groupBox_Operator_Information";
            this.groupBox_Operator_Information.Size = new System.Drawing.Size(272, 304);
            this.groupBox_Operator_Information.TabIndex = 199;
            this.groupBox_Operator_Information.TabStop = false;
            this.groupBox_Operator_Information.Text = " [ 작업자 정보 ] ";
            // 
            // checkBox_Password_Visible
            // 
            this.checkBox_Password_Visible.AutoSize = true;
            this.checkBox_Password_Visible.Location = new System.Drawing.Point(85, 193);
            this.checkBox_Password_Visible.Name = "checkBox_Password_Visible";
            this.checkBox_Password_Visible.Size = new System.Drawing.Size(130, 23);
            this.checkBox_Password_Visible.TabIndex = 6;
            this.checkBox_Password_Visible.Text = "비밀번호 보이기";
            this.checkBox_Password_Visible.UseVisualStyleBackColor = true;
            this.checkBox_Password_Visible.CheckedChanged += new System.EventHandler(this.checkBox_Password_Visible_CheckedChanged);
            // 
            // radioButton_Administrator
            // 
            this.radioButton_Administrator.AutoSize = true;
            this.radioButton_Administrator.Location = new System.Drawing.Point(191, 40);
            this.radioButton_Administrator.Name = "radioButton_Administrator";
            this.radioButton_Administrator.Size = new System.Drawing.Size(69, 23);
            this.radioButton_Administrator.TabIndex = 2;
            this.radioButton_Administrator.TabStop = true;
            this.radioButton_Administrator.Text = "관리자";
            this.radioButton_Administrator.UseVisualStyleBackColor = true;
            // 
            // radioButton_Operator
            // 
            this.radioButton_Operator.AutoSize = true;
            this.radioButton_Operator.Location = new System.Drawing.Point(104, 40);
            this.radioButton_Operator.Name = "radioButton_Operator";
            this.radioButton_Operator.Size = new System.Drawing.Size(69, 23);
            this.radioButton_Operator.TabIndex = 1;
            this.radioButton_Operator.TabStop = true;
            this.radioButton_Operator.Text = "작업자";
            this.radioButton_Operator.UseVisualStyleBackColor = true;
            // 
            // baseLabel_Authority
            // 
            this.baseLabel_Authority.BackColor = System.Drawing.Color.Black;
            this.baseLabel_Authority.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabel_Authority.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_Authority.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel_Authority.Location = new System.Drawing.Point(13, 35);
            this.baseLabel_Authority.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Authority.Name = "baseLabel_Authority";
            this.baseLabel_Authority.Size = new System.Drawing.Size(71, 32);
            this.baseLabel_Authority.TabIndex = 208;
            this.baseLabel_Authority.Text = "권한";
            this.baseLabel_Authority.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_User_Del
            // 
            this.btn_User_Del.BackColor = System.Drawing.Color.LightGray;
            this.btn_User_Del.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btn_User_Del.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn_User_Del.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btn_User_Del.Font = new System.Drawing.Font("나눔바른고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_User_Del.ForeColor = System.Drawing.Color.DarkRed;
            this.btn_User_Del.Location = new System.Drawing.Point(142, 239);
            this.btn_User_Del.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_User_Del.Name = "btn_User_Del";
            this.btn_User_Del.Size = new System.Drawing.Size(118, 55);
            this.btn_User_Del.TabIndex = 8;
            this.btn_User_Del.Text = "삭제";
            this.btn_User_Del.UseVisualStyleBackColor = false;
            this.btn_User_Del.Click += new System.EventHandler(this.btn_User_Del_Click);
            // 
            // btn_User_Add
            // 
            this.btn_User_Add.BackColor = System.Drawing.Color.LightGray;
            this.btn_User_Add.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btn_User_Add.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn_User_Add.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btn_User_Add.Font = new System.Drawing.Font("나눔바른고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_User_Add.ForeColor = System.Drawing.Color.DarkRed;
            this.btn_User_Add.Location = new System.Drawing.Point(13, 239);
            this.btn_User_Add.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_User_Add.Name = "btn_User_Add";
            this.btn_User_Add.Size = new System.Drawing.Size(118, 55);
            this.btn_User_Add.TabIndex = 7;
            this.btn_User_Add.Text = "추가";
            this.btn_User_Add.UseVisualStyleBackColor = false;
            this.btn_User_Add.Click += new System.EventHandler(this.btn_User_Add_Click);
            // 
            // tb_Password
            // 
            this.tb_Password.BackColor = System.Drawing.Color.Cornsilk;
            this.tb_Password.Font = new System.Drawing.Font("나눔바른고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_Password.Location = new System.Drawing.Point(85, 155);
            this.tb_Password.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_Password.Name = "tb_Password";
            this.tb_Password.PasswordChar = '*';
            this.tb_Password.Size = new System.Drawing.Size(175, 32);
            this.tb_Password.TabIndex = 5;
            this.tb_Password.Text = "12345";
            this.tb_Password.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseLabel_Password
            // 
            this.baseLabel_Password.BackColor = System.Drawing.Color.Black;
            this.baseLabel_Password.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabel_Password.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_Password.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel_Password.Location = new System.Drawing.Point(13, 156);
            this.baseLabel_Password.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Password.Name = "baseLabel_Password";
            this.baseLabel_Password.Size = new System.Drawing.Size(71, 32);
            this.baseLabel_Password.TabIndex = 203;
            this.baseLabel_Password.Text = "비밀번호";
            this.baseLabel_Password.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tb_ID
            // 
            this.tb_ID.BackColor = System.Drawing.Color.Cornsilk;
            this.tb_ID.Font = new System.Drawing.Font("나눔바른고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_ID.Location = new System.Drawing.Point(85, 111);
            this.tb_ID.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_ID.Name = "tb_ID";
            this.tb_ID.Size = new System.Drawing.Size(175, 32);
            this.tb_ID.TabIndex = 4;
            this.tb_ID.Text = "ABCDE";
            this.tb_ID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseLabel_ID
            // 
            this.baseLabel_ID.BackColor = System.Drawing.Color.Black;
            this.baseLabel_ID.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabel_ID.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_ID.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel_ID.Location = new System.Drawing.Point(13, 112);
            this.baseLabel_ID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_ID.Name = "baseLabel_ID";
            this.baseLabel_ID.Size = new System.Drawing.Size(71, 32);
            this.baseLabel_ID.TabIndex = 201;
            this.baseLabel_ID.Text = "아이디";
            this.baseLabel_ID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tb_Name
            // 
            this.tb_Name.BackColor = System.Drawing.Color.Cornsilk;
            this.tb_Name.Font = new System.Drawing.Font("나눔바른고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_Name.Location = new System.Drawing.Point(85, 77);
            this.tb_Name.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_Name.Name = "tb_Name";
            this.tb_Name.Size = new System.Drawing.Size(175, 32);
            this.tb_Name.TabIndex = 3;
            this.tb_Name.Text = "김 아무개";
            this.tb_Name.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseLabel_Name
            // 
            this.baseLabel_Name.BackColor = System.Drawing.Color.Black;
            this.baseLabel_Name.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabel_Name.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_Name.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel_Name.Location = new System.Drawing.Point(13, 78);
            this.baseLabel_Name.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Name.Name = "baseLabel_Name";
            this.baseLabel_Name.Size = new System.Drawing.Size(71, 32);
            this.baseLabel_Name.TabIndex = 199;
            this.baseLabel_Name.Text = "이름";
            this.baseLabel_Name.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormUserRegistration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lavender;
            this.ClientSize = new System.Drawing.Size(1040, 448);
            this.Controls.Add(this.groupBox_Operator_Information);
            this.Controls.Add(this.listView_Operator);
            this.Name = "FormUserRegistration";
            this.Text = "User Registration";
            this.groupBox_Operator_Information.ResumeLayout(false);
            this.groupBox_Operator_Information.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView_Operator;
        private System.Windows.Forms.GroupBox groupBox_Operator_Information;
        public System.Windows.Forms.TextBox tb_Password;
        private BaseLabel baseLabel_Password;
        public System.Windows.Forms.TextBox tb_ID;
        private BaseLabel baseLabel_ID;
        public System.Windows.Forms.TextBox tb_Name;
        private BaseLabel baseLabel_Name;
        private System.Windows.Forms.Button btn_User_Del;
        private System.Windows.Forms.Button btn_User_Add;
        public System.Windows.Forms.RadioButton radioButton_Administrator;
        public System.Windows.Forms.RadioButton radioButton_Operator;
        private BaseLabel baseLabel_Authority;
        public System.Windows.Forms.CheckBox checkBox_Password_Visible;
    }
}