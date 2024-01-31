namespace CWA150SA_Onsemi300
{
    partial class JogControl
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.radioButtonContinuous = new System.Windows.Forms.RadioButton();
            this.radioButtonStep = new System.Windows.Forms.RadioButton();
            this.flowLayoutPanelJogButtonComb_UVW = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanelJogButtonAxisX = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanelJogButtonAxisY = new System.Windows.Forms.FlowLayoutPanel();
            this.timerJogControl = new System.Windows.Forms.Timer(this.components);
            this.groupBoxJogControl = new System.Windows.Forms.GroupBox();
            this.baseToggleButtonMultiplication = new CWA150SA_Onsemi300.BaseToggleButton();
            this.baseToggleButtonDivision = new CWA150SA_Onsemi300.BaseToggleButton();
            this.baseToggleButton0001 = new CWA150SA_Onsemi300.BaseToggleButton();
            this.baseToggleButton001 = new CWA150SA_Onsemi300.BaseToggleButton();
            this.baseToggleButton01 = new CWA150SA_Onsemi300.BaseToggleButton();
            this.baseToggleButton1 = new CWA150SA_Onsemi300.BaseToggleButton();
            this.dataGridViewJogControl = new CWA150SA_Onsemi300.BaseDataGridView();
            this.flowLayoutPanelJogButtonComb_XY = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBoxJogControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewJogControl)).BeginInit();
            this.SuspendLayout();
            // 
            // radioButtonContinuous
            // 
            this.radioButtonContinuous.AutoSize = true;
            this.radioButtonContinuous.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.radioButtonContinuous.Location = new System.Drawing.Point(6, 222);
            this.radioButtonContinuous.Name = "radioButtonContinuous";
            this.radioButtonContinuous.Size = new System.Drawing.Size(87, 16);
            this.radioButtonContinuous.TabIndex = 3;
            this.radioButtonContinuous.TabStop = true;
            this.radioButtonContinuous.Text = "Continuous";
            this.radioButtonContinuous.UseVisualStyleBackColor = true;
            this.radioButtonContinuous.CheckedChanged += new System.EventHandler(this.radioButtonContinuous_CheckedChanged);
            // 
            // radioButtonStep
            // 
            this.radioButtonStep.AutoSize = true;
            this.radioButtonStep.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.radioButtonStep.Location = new System.Drawing.Point(6, 200);
            this.radioButtonStep.Name = "radioButtonStep";
            this.radioButtonStep.Size = new System.Drawing.Size(48, 16);
            this.radioButtonStep.TabIndex = 4;
            this.radioButtonStep.TabStop = true;
            this.radioButtonStep.Text = "Step";
            this.radioButtonStep.UseVisualStyleBackColor = true;
            this.radioButtonStep.CheckedChanged += new System.EventHandler(this.radioButtonStep_CheckedChanged);
            // 
            // flowLayoutPanelJogButtonComb_UVW
            // 
            this.flowLayoutPanelJogButtonComb_UVW.Location = new System.Drawing.Point(98, 238);
            this.flowLayoutPanelJogButtonComb_UVW.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.flowLayoutPanelJogButtonComb_UVW.Name = "flowLayoutPanelJogButtonComb_UVW";
            this.flowLayoutPanelJogButtonComb_UVW.Size = new System.Drawing.Size(168, 223);
            this.flowLayoutPanelJogButtonComb_UVW.TabIndex = 0;
            // 
            // flowLayoutPanelJogButtonAxisX
            // 
            this.flowLayoutPanelJogButtonAxisX.Location = new System.Drawing.Point(272, 222);
            this.flowLayoutPanelJogButtonAxisX.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.flowLayoutPanelJogButtonAxisX.Name = "flowLayoutPanelJogButtonAxisX";
            this.flowLayoutPanelJogButtonAxisX.Size = new System.Drawing.Size(157, 178);
            this.flowLayoutPanelJogButtonAxisX.TabIndex = 7;
            // 
            // flowLayoutPanelJogButtonAxisY
            // 
            this.flowLayoutPanelJogButtonAxisY.Location = new System.Drawing.Point(2, 238);
            this.flowLayoutPanelJogButtonAxisY.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.flowLayoutPanelJogButtonAxisY.Name = "flowLayoutPanelJogButtonAxisY";
            this.flowLayoutPanelJogButtonAxisY.Size = new System.Drawing.Size(162, 223);
            this.flowLayoutPanelJogButtonAxisY.TabIndex = 8;
            // 
            // timerJogControl
            // 
            this.timerJogControl.Enabled = true;
            this.timerJogControl.Interval = 1000;
            this.timerJogControl.Tick += new System.EventHandler(this.TimerFunction);
            // 
            // groupBoxJogControl
            // 
            this.groupBoxJogControl.Controls.Add(this.baseToggleButtonMultiplication);
            this.groupBoxJogControl.Controls.Add(this.baseToggleButtonDivision);
            this.groupBoxJogControl.Controls.Add(this.baseToggleButton0001);
            this.groupBoxJogControl.Controls.Add(this.baseToggleButton001);
            this.groupBoxJogControl.Controls.Add(this.baseToggleButton01);
            this.groupBoxJogControl.Controls.Add(this.baseToggleButton1);
            this.groupBoxJogControl.Controls.Add(this.flowLayoutPanelJogButtonComb_UVW);
            this.groupBoxJogControl.Controls.Add(this.flowLayoutPanelJogButtonAxisY);
            this.groupBoxJogControl.Controls.Add(this.flowLayoutPanelJogButtonAxisX);
            this.groupBoxJogControl.Controls.Add(this.dataGridViewJogControl);
            this.groupBoxJogControl.Controls.Add(this.radioButtonContinuous);
            this.groupBoxJogControl.Controls.Add(this.radioButtonStep);
            this.groupBoxJogControl.Controls.Add(this.flowLayoutPanelJogButtonComb_XY);
            this.groupBoxJogControl.ForeColor = System.Drawing.Color.White;
            this.groupBoxJogControl.Location = new System.Drawing.Point(0, 0);
            this.groupBoxJogControl.Name = "groupBoxJogControl";
            this.groupBoxJogControl.Size = new System.Drawing.Size(748, 473);
            this.groupBoxJogControl.TabIndex = 9;
            this.groupBoxJogControl.TabStop = false;
            this.groupBoxJogControl.Text = " Jog Control ";
            // 
            // baseToggleButtonMultiplication
            // 
            this.baseToggleButtonMultiplication.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseToggleButtonMultiplication.FlatAppearance.BorderSize = 0;
            this.baseToggleButtonMultiplication.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButtonMultiplication.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseToggleButtonMultiplication.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseToggleButtonMultiplication.Location = new System.Drawing.Point(192, 194);
            this.baseToggleButtonMultiplication.Name = "baseToggleButtonMultiplication";
            this.baseToggleButtonMultiplication.Size = new System.Drawing.Size(75, 41);
            this.baseToggleButtonMultiplication.TabIndex = 21;
            this.baseToggleButtonMultiplication.Text = "X 2";
            this.baseToggleButtonMultiplication.UseVisualStyleBackColor = false;
            this.baseToggleButtonMultiplication.Click += new System.EventHandler(this.baseToggleButtonMultiplication_Click);
            // 
            // baseToggleButtonDivision
            // 
            this.baseToggleButtonDivision.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseToggleButtonDivision.FlatAppearance.BorderSize = 0;
            this.baseToggleButtonDivision.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButtonDivision.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseToggleButtonDivision.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseToggleButtonDivision.Location = new System.Drawing.Point(633, 194);
            this.baseToggleButtonDivision.Name = "baseToggleButtonDivision";
            this.baseToggleButtonDivision.Size = new System.Drawing.Size(75, 41);
            this.baseToggleButtonDivision.TabIndex = 20;
            this.baseToggleButtonDivision.Text = "/ 2";
            this.baseToggleButtonDivision.UseVisualStyleBackColor = false;
            this.baseToggleButtonDivision.Click += new System.EventHandler(this.baseToggleButtonDivision_Click);
            // 
            // baseToggleButton0001
            // 
            this.baseToggleButton0001.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseToggleButton0001.FlatAppearance.BorderSize = 0;
            this.baseToggleButton0001.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButton0001.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseToggleButton0001.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseToggleButton0001.Location = new System.Drawing.Point(540, 194);
            this.baseToggleButton0001.Name = "baseToggleButton0001";
            this.baseToggleButton0001.Size = new System.Drawing.Size(68, 41);
            this.baseToggleButton0001.TabIndex = 19;
            this.baseToggleButton0001.Text = "0.001";
            this.baseToggleButton0001.UseVisualStyleBackColor = false;
            this.baseToggleButton0001.Click += new System.EventHandler(this.baseToggleButton0001_Click);
            // 
            // baseToggleButton001
            // 
            this.baseToggleButton001.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseToggleButton001.FlatAppearance.BorderSize = 0;
            this.baseToggleButton001.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButton001.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseToggleButton001.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseToggleButton001.Location = new System.Drawing.Point(455, 194);
            this.baseToggleButton001.Name = "baseToggleButton001";
            this.baseToggleButton001.Size = new System.Drawing.Size(68, 41);
            this.baseToggleButton001.TabIndex = 18;
            this.baseToggleButton001.Text = "0.01";
            this.baseToggleButton001.UseVisualStyleBackColor = false;
            this.baseToggleButton001.Click += new System.EventHandler(this.baseToggleButton001_Click);
            // 
            // baseToggleButton01
            // 
            this.baseToggleButton01.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseToggleButton01.FlatAppearance.BorderSize = 0;
            this.baseToggleButton01.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButton01.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseToggleButton01.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseToggleButton01.Location = new System.Drawing.Point(370, 194);
            this.baseToggleButton01.Name = "baseToggleButton01";
            this.baseToggleButton01.Size = new System.Drawing.Size(68, 41);
            this.baseToggleButton01.TabIndex = 17;
            this.baseToggleButton01.Text = "0.1";
            this.baseToggleButton01.UseVisualStyleBackColor = false;
            this.baseToggleButton01.Click += new System.EventHandler(this.baseToggleButton01_Click);
            // 
            // baseToggleButton1
            // 
            this.baseToggleButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseToggleButton1.FlatAppearance.BorderSize = 0;
            this.baseToggleButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButton1.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseToggleButton1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseToggleButton1.Location = new System.Drawing.Point(285, 194);
            this.baseToggleButton1.Name = "baseToggleButton1";
            this.baseToggleButton1.Size = new System.Drawing.Size(68, 41);
            this.baseToggleButton1.TabIndex = 16;
            this.baseToggleButton1.Text = "1";
            this.baseToggleButton1.UseVisualStyleBackColor = false;
            this.baseToggleButton1.Click += new System.EventHandler(this.baseToggleButton1_Click);
            // 
            // dataGridViewJogControl
            // 
            this.dataGridViewJogControl.AllowUserToAddRows = false;
            this.dataGridViewJogControl.AllowUserToResizeRows = false;
            this.dataGridViewJogControl.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewJogControl.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.dataGridViewJogControl.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewJogControl.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewJogControl.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewJogControl.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewJogControl.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            this.dataGridViewJogControl.Location = new System.Drawing.Point(6, 16);
            this.dataGridViewJogControl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridViewJogControl.MultiSelect = false;
            this.dataGridViewJogControl.Name = "dataGridViewJogControl";
            this.dataGridViewJogControl.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewJogControl.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewJogControl.RowHeadersVisible = false;
            this.dataGridViewJogControl.RowHeadersWidth = 51;
            this.dataGridViewJogControl.RowTemplate.Height = 27;
            this.dataGridViewJogControl.Size = new System.Drawing.Size(736, 172);
            this.dataGridViewJogControl.TabIndex = 5;
            this.dataGridViewJogControl.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.daragridview_buttonCellClick);
            this.dataGridViewJogControl.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewJogControl_CellValueChanged);
            // 
            // flowLayoutPanelJogButtonComb_XY
            // 
            this.flowLayoutPanelJogButtonComb_XY.Location = new System.Drawing.Point(192, 238);
            this.flowLayoutPanelJogButtonComb_XY.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.flowLayoutPanelJogButtonComb_XY.Name = "flowLayoutPanelJogButtonComb_XY";
            this.flowLayoutPanelJogButtonComb_XY.Size = new System.Drawing.Size(168, 178);
            this.flowLayoutPanelJogButtonComb_XY.TabIndex = 1;
            // 
            // JogControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxJogControl);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "JogControl";
            this.Size = new System.Drawing.Size(750, 473);
            this.groupBoxJogControl.ResumeLayout(false);
            this.groupBoxJogControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewJogControl)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.RadioButton radioButtonContinuous;
        private System.Windows.Forms.RadioButton radioButtonStep;
        public BaseDataGridView dataGridViewJogControl;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelJogButtonComb_UVW;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelJogButtonComb_XY;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelJogButtonAxisX;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelJogButtonAxisY;
        private System.Windows.Forms.Timer timerJogControl;
        private System.Windows.Forms.GroupBox groupBoxJogControl;
        private BaseToggleButton baseToggleButtonMultiplication;
        private BaseToggleButton baseToggleButtonDivision;
        private BaseToggleButton baseToggleButton0001;
        private BaseToggleButton baseToggleButton001;
        private BaseToggleButton baseToggleButton01;
        private BaseToggleButton baseToggleButton1;
    }
}
