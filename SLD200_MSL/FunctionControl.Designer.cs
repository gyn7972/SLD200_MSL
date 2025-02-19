namespace SLD200_MSL
{
    partial class FunctionControl
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

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBoxFunctionControl = new System.Windows.Forms.GroupBox();
            this.groupBoxActionParameter = new System.Windows.Forms.GroupBox();
            this.dataGridViewActionParameter = new SLD200_MSL.BaseDataGridView();
            this.baseButtonSave = new SLD200_MSL.BaseButton();
            this.baseButtonSetCurrent = new SLD200_MSL.BaseButton();
            this.groupBoxAction = new System.Windows.Forms.GroupBox();
            this.baseButtonActionRun = new SLD200_MSL.BaseButton();
            this.listBoxAction = new SLD200_MSL.BaseListBox();
            this.groupBoxFunction = new System.Windows.Forms.GroupBox();
            this.listBoxFunction = new SLD200_MSL.BaseListBox();
            this.baseButtonRun = new SLD200_MSL.BaseButton();
            this.groupBoxFunctionControl.SuspendLayout();
            this.groupBoxActionParameter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewActionParameter)).BeginInit();
            this.groupBoxAction.SuspendLayout();
            this.groupBoxFunction.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxFunctionControl
            // 
            this.groupBoxFunctionControl.Controls.Add(this.groupBoxActionParameter);
            this.groupBoxFunctionControl.Controls.Add(this.groupBoxAction);
            this.groupBoxFunctionControl.Controls.Add(this.groupBoxFunction);
            this.groupBoxFunctionControl.ForeColor = System.Drawing.Color.White;
            this.groupBoxFunctionControl.Location = new System.Drawing.Point(4, 3);
            this.groupBoxFunctionControl.Name = "groupBoxFunctionControl";
            this.groupBoxFunctionControl.Size = new System.Drawing.Size(793, 284);
            this.groupBoxFunctionControl.TabIndex = 6;
            this.groupBoxFunctionControl.TabStop = false;
            this.groupBoxFunctionControl.Text = " FunctionControl ";
            // 
            // groupBoxActionParameter
            // 
            this.groupBoxActionParameter.Controls.Add(this.dataGridViewActionParameter);
            this.groupBoxActionParameter.Controls.Add(this.baseButtonSave);
            this.groupBoxActionParameter.Controls.Add(this.baseButtonSetCurrent);
            this.groupBoxActionParameter.ForeColor = System.Drawing.Color.White;
            this.groupBoxActionParameter.Location = new System.Drawing.Point(363, 16);
            this.groupBoxActionParameter.Name = "groupBoxActionParameter";
            this.groupBoxActionParameter.Size = new System.Drawing.Size(425, 262);
            this.groupBoxActionParameter.TabIndex = 15;
            this.groupBoxActionParameter.TabStop = false;
            this.groupBoxActionParameter.Text = " Action Parameter ";
            // 
            // dataGridViewActionParameter
            // 
            this.dataGridViewActionParameter.AllowUserToAddRows = false;
            this.dataGridViewActionParameter.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewActionParameter.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.dataGridViewActionParameter.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewActionParameter.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewActionParameter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewActionParameter.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewActionParameter.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            this.dataGridViewActionParameter.Location = new System.Drawing.Point(6, 18);
            this.dataGridViewActionParameter.MultiSelect = false;
            this.dataGridViewActionParameter.Name = "dataGridViewActionParameter";
            this.dataGridViewActionParameter.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewActionParameter.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewActionParameter.RowHeadersVisible = false;
            this.dataGridViewActionParameter.RowHeadersWidth = 51;
            this.dataGridViewActionParameter.RowTemplate.Height = 23;
            this.dataGridViewActionParameter.Size = new System.Drawing.Size(416, 194);
            this.dataGridViewActionParameter.TabIndex = 9;
            this.dataGridViewActionParameter.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewActionParameter_CellContentClick);
            this.dataGridViewActionParameter.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridViewActionParameter_CellFormatting);
            // 
            // baseButtonSave
            // 
            this.baseButtonSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonSave.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonSave.Location = new System.Drawing.Point(342, 214);
            this.baseButtonSave.Name = "baseButtonSave";
            this.baseButtonSave.Size = new System.Drawing.Size(80, 42);
            this.baseButtonSave.TabIndex = 8;
            this.baseButtonSave.Text = "Save";
            this.baseButtonSave.UseVisualStyleBackColor = false;
            this.baseButtonSave.Click += new System.EventHandler(this.baseButtonSave_Click);
            // 
            // baseButtonSetCurrent
            // 
            this.baseButtonSetCurrent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonSetCurrent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonSetCurrent.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonSetCurrent.Location = new System.Drawing.Point(256, 214);
            this.baseButtonSetCurrent.Name = "baseButtonSetCurrent";
            this.baseButtonSetCurrent.Size = new System.Drawing.Size(80, 42);
            this.baseButtonSetCurrent.TabIndex = 7;
            this.baseButtonSetCurrent.Text = "SetCurrent";
            this.baseButtonSetCurrent.UseVisualStyleBackColor = false;
            this.baseButtonSetCurrent.Click += new System.EventHandler(this.baseButtonSetCurrent_Click);
            // 
            // groupBoxAction
            // 
            this.groupBoxAction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxAction.Controls.Add(this.baseButtonActionRun);
            this.groupBoxAction.Controls.Add(this.listBoxAction);
            this.groupBoxAction.ForeColor = System.Drawing.Color.White;
            this.groupBoxAction.Location = new System.Drawing.Point(176, 20);
            this.groupBoxAction.Name = "groupBoxAction";
            this.groupBoxAction.Size = new System.Drawing.Size(183, 258);
            this.groupBoxAction.TabIndex = 14;
            this.groupBoxAction.TabStop = false;
            this.groupBoxAction.Text = " Action ";
            // 
            // baseButtonActionRun
            // 
            this.baseButtonActionRun.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonActionRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonActionRun.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonActionRun.Location = new System.Drawing.Point(97, 213);
            this.baseButtonActionRun.Name = "baseButtonActionRun";
            this.baseButtonActionRun.Size = new System.Drawing.Size(80, 42);
            this.baseButtonActionRun.TabIndex = 12;
            this.baseButtonActionRun.Text = "Run";
            this.baseButtonActionRun.UseVisualStyleBackColor = false;
            this.baseButtonActionRun.Click += new System.EventHandler(this.baseButtonActionRun_Click);
            // 
            // listBoxAction
            // 
            this.listBoxAction.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.listBoxAction.Font = new System.Drawing.Font("Tahoma", 15F);
            this.listBoxAction.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(79)))), ((int)(((byte)(79)))));
            this.listBoxAction.FormattingEnabled = true;
            this.listBoxAction.ItemHeight = 20;
            this.listBoxAction.Location = new System.Drawing.Point(6, 17);
            this.listBoxAction.Name = "listBoxAction";
            this.listBoxAction.Size = new System.Drawing.Size(171, 184);
            this.listBoxAction.TabIndex = 10;
            this.listBoxAction.SelectedIndexChanged += new System.EventHandler(this.listBoxAction_SelectedIndexChanged);
            // 
            // groupBoxFunction
            // 
            this.groupBoxFunction.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxFunction.Controls.Add(this.listBoxFunction);
            this.groupBoxFunction.Controls.Add(this.baseButtonRun);
            this.groupBoxFunction.ForeColor = System.Drawing.Color.White;
            this.groupBoxFunction.Location = new System.Drawing.Point(6, 19);
            this.groupBoxFunction.Name = "groupBoxFunction";
            this.groupBoxFunction.Size = new System.Drawing.Size(164, 258);
            this.groupBoxFunction.TabIndex = 13;
            this.groupBoxFunction.TabStop = false;
            this.groupBoxFunction.Text = " Function ";
            // 
            // listBoxFunction
            // 
            this.listBoxFunction.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.listBoxFunction.Font = new System.Drawing.Font("Tahoma", 15F);
            this.listBoxFunction.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(79)))), ((int)(((byte)(79)))));
            this.listBoxFunction.FormattingEnabled = true;
            this.listBoxFunction.ItemHeight = 20;
            this.listBoxFunction.Location = new System.Drawing.Point(9, 17);
            this.listBoxFunction.Name = "listBoxFunction";
            this.listBoxFunction.Size = new System.Drawing.Size(147, 184);
            this.listBoxFunction.TabIndex = 11;
            this.listBoxFunction.SelectedIndexChanged += new System.EventHandler(this.listBoxFunction_SelectedIndexChanged);
            // 
            // baseButtonRun
            // 
            this.baseButtonRun.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonRun.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonRun.Location = new System.Drawing.Point(76, 214);
            this.baseButtonRun.Name = "baseButtonRun";
            this.baseButtonRun.Size = new System.Drawing.Size(80, 42);
            this.baseButtonRun.TabIndex = 6;
            this.baseButtonRun.Text = "Run";
            this.baseButtonRun.UseVisualStyleBackColor = false;
            this.baseButtonRun.Click += new System.EventHandler(this.baseButtonRun_Click);
            // 
            // FunctionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxFunctionControl);
            this.Name = "FunctionControl";
            this.Size = new System.Drawing.Size(800, 295);
            this.groupBoxFunctionControl.ResumeLayout(false);
            this.groupBoxActionParameter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewActionParameter)).EndInit();
            this.groupBoxAction.ResumeLayout(false);
            this.groupBoxFunction.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBoxFunctionControl;
        private BaseButton baseButtonSave;
        private BaseButton baseButtonSetCurrent;
        private BaseButton baseButtonRun;
        private BaseDataGridView dataGridViewActionParameter;
        private BaseListBox listBoxFunction;
        private BaseListBox listBoxAction;
        private System.Windows.Forms.GroupBox groupBoxAction;
        private BaseButton baseButtonActionRun;
        private System.Windows.Forms.GroupBox groupBoxFunction;
        private System.Windows.Forms.GroupBox groupBoxActionParameter;
    }
}
