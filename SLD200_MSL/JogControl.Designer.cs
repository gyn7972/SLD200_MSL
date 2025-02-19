namespace SLD200_MSL
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.timerJogControl = new System.Windows.Forms.Timer(this.components);
            this.groupBoxJogControl = new SLD200_MSL.WATGroupBox();
            this.flowLayoutPanelJogButtonComb_XZ_UL = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanelJogButtonComb_XY = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanelJogButtonAxisY2 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanelJogButtonComb_XZ_LD = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanelJogButtonAxisX = new System.Windows.Forms.FlowLayoutPanel();
            this.baseToggleButtonMultiplication = new SLD200_MSL.BaseToggleButton();
            this.baseToggleButtonDivision = new SLD200_MSL.BaseToggleButton();
            this.baseToggleButton0001 = new SLD200_MSL.BaseToggleButton();
            this.baseToggleButton001 = new SLD200_MSL.BaseToggleButton();
            this.baseToggleButton01 = new SLD200_MSL.BaseToggleButton();
            this.baseToggleButton1 = new SLD200_MSL.BaseToggleButton();
            this.flowLayoutPanelJogButtonComb_UVW = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanelJogButtonAxisY = new System.Windows.Forms.FlowLayoutPanel();
            this.dataGridViewJogControl = new SLD200_MSL.BaseDataGridView();
            this.radioButtonContinuous = new System.Windows.Forms.RadioButton();
            this.radioButtonStep = new System.Windows.Forms.RadioButton();
            this.groupBoxJogControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewJogControl)).BeginInit();
            this.SuspendLayout();
            // 
            // timerJogControl
            // 
            this.timerJogControl.Enabled = true;
            this.timerJogControl.Interval = 1000;
            this.timerJogControl.Tick += new System.EventHandler(this.TimerFunction);
            // 
            // groupBoxJogControl
            // 
            this.groupBoxJogControl.BorderColor = System.Drawing.Color.Black;
            this.groupBoxJogControl.Controls.Add(this.flowLayoutPanelJogButtonComb_XZ_UL);
            this.groupBoxJogControl.Controls.Add(this.flowLayoutPanelJogButtonComb_XY);
            this.groupBoxJogControl.Controls.Add(this.flowLayoutPanelJogButtonAxisY2);
            this.groupBoxJogControl.Controls.Add(this.flowLayoutPanelJogButtonComb_XZ_LD);
            this.groupBoxJogControl.Controls.Add(this.flowLayoutPanelJogButtonAxisX);
            this.groupBoxJogControl.Controls.Add(this.baseToggleButtonMultiplication);
            this.groupBoxJogControl.Controls.Add(this.baseToggleButtonDivision);
            this.groupBoxJogControl.Controls.Add(this.baseToggleButton0001);
            this.groupBoxJogControl.Controls.Add(this.baseToggleButton001);
            this.groupBoxJogControl.Controls.Add(this.baseToggleButton01);
            this.groupBoxJogControl.Controls.Add(this.baseToggleButton1);
            this.groupBoxJogControl.Controls.Add(this.flowLayoutPanelJogButtonComb_UVW);
            this.groupBoxJogControl.Controls.Add(this.flowLayoutPanelJogButtonAxisY);
            this.groupBoxJogControl.Controls.Add(this.dataGridViewJogControl);
            this.groupBoxJogControl.Controls.Add(this.radioButtonContinuous);
            this.groupBoxJogControl.Controls.Add(this.radioButtonStep);
            this.groupBoxJogControl.Font = new System.Drawing.Font("Tahoma", 9F);
            this.groupBoxJogControl.ForeColor = System.Drawing.Color.Black;
            this.groupBoxJogControl.Location = new System.Drawing.Point(0, 0);
            this.groupBoxJogControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBoxJogControl.Name = "groupBoxJogControl";
            this.groupBoxJogControl.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBoxJogControl.Size = new System.Drawing.Size(647, 474);
            this.groupBoxJogControl.TabIndex = 9;
            this.groupBoxJogControl.TabStop = false;
            this.groupBoxJogControl.Text = " Jog Control ";
            // 
            // flowLayoutPanelJogButtonComb_XZ_UL
            // 
            this.flowLayoutPanelJogButtonComb_XZ_UL.Location = new System.Drawing.Point(131, 245);
            this.flowLayoutPanelJogButtonComb_XZ_UL.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.flowLayoutPanelJogButtonComb_XZ_UL.Name = "flowLayoutPanelJogButtonComb_XZ_UL";
            this.flowLayoutPanelJogButtonComb_XZ_UL.Size = new System.Drawing.Size(138, 220);
            this.flowLayoutPanelJogButtonComb_XZ_UL.TabIndex = 23;
            this.flowLayoutPanelJogButtonComb_XZ_UL.Visible = false;
            // 
            // flowLayoutPanelJogButtonComb_XY
            // 
            this.flowLayoutPanelJogButtonComb_XY.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.flowLayoutPanelJogButtonComb_XY.Location = new System.Drawing.Point(81, 247);
            this.flowLayoutPanelJogButtonComb_XY.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.flowLayoutPanelJogButtonComb_XY.Name = "flowLayoutPanelJogButtonComb_XY";
            this.flowLayoutPanelJogButtonComb_XY.Size = new System.Drawing.Size(138, 220);
            this.flowLayoutPanelJogButtonComb_XY.TabIndex = 25;
            this.flowLayoutPanelJogButtonComb_XY.Visible = false;
            // 
            // flowLayoutPanelJogButtonAxisY2
            // 
            this.flowLayoutPanelJogButtonAxisY2.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.flowLayoutPanelJogButtonAxisY2.Location = new System.Drawing.Point(444, 247);
            this.flowLayoutPanelJogButtonAxisY2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.flowLayoutPanelJogButtonAxisY2.Name = "flowLayoutPanelJogButtonAxisY2";
            this.flowLayoutPanelJogButtonAxisY2.Size = new System.Drawing.Size(71, 220);
            this.flowLayoutPanelJogButtonAxisY2.TabIndex = 24;
            this.flowLayoutPanelJogButtonAxisY2.Visible = false;
            // 
            // flowLayoutPanelJogButtonComb_XZ_LD
            // 
            this.flowLayoutPanelJogButtonComb_XZ_LD.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.flowLayoutPanelJogButtonComb_XZ_LD.Location = new System.Drawing.Point(300, 247);
            this.flowLayoutPanelJogButtonComb_XZ_LD.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.flowLayoutPanelJogButtonComb_XZ_LD.Name = "flowLayoutPanelJogButtonComb_XZ_LD";
            this.flowLayoutPanelJogButtonComb_XZ_LD.Size = new System.Drawing.Size(138, 220);
            this.flowLayoutPanelJogButtonComb_XZ_LD.TabIndex = 22;
            this.flowLayoutPanelJogButtonComb_XZ_LD.Visible = false;
            // 
            // flowLayoutPanelJogButtonAxisX
            // 
            this.flowLayoutPanelJogButtonAxisX.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.flowLayoutPanelJogButtonAxisX.Location = new System.Drawing.Point(385, 247);
            this.flowLayoutPanelJogButtonAxisX.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.flowLayoutPanelJogButtonAxisX.Name = "flowLayoutPanelJogButtonAxisX";
            this.flowLayoutPanelJogButtonAxisX.Size = new System.Drawing.Size(157, 220);
            this.flowLayoutPanelJogButtonAxisX.TabIndex = 7;
            this.flowLayoutPanelJogButtonAxisX.Visible = false;
            // 
            // baseToggleButtonMultiplication
            // 
            this.baseToggleButtonMultiplication.BackColor = System.Drawing.Color.White;
            this.baseToggleButtonMultiplication.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.baseToggleButtonMultiplication.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButtonMultiplication.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseToggleButtonMultiplication.ForeColor = System.Drawing.Color.Black;
            this.baseToggleButtonMultiplication.Location = new System.Drawing.Point(148, 188);
            this.baseToggleButtonMultiplication.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.baseToggleButtonMultiplication.Name = "baseToggleButtonMultiplication";
            this.baseToggleButtonMultiplication.Size = new System.Drawing.Size(62, 37);
            this.baseToggleButtonMultiplication.TabIndex = 21;
            this.baseToggleButtonMultiplication.Text = "X 2";
            this.baseToggleButtonMultiplication.UseVisualStyleBackColor = false;
            this.baseToggleButtonMultiplication.Click += new System.EventHandler(this.baseToggleButtonMultiplication_Click);
            // 
            // baseToggleButtonDivision
            // 
            this.baseToggleButtonDivision.BackColor = System.Drawing.Color.White;
            this.baseToggleButtonDivision.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.baseToggleButtonDivision.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButtonDivision.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseToggleButtonDivision.ForeColor = System.Drawing.Color.Black;
            this.baseToggleButtonDivision.Location = new System.Drawing.Point(488, 188);
            this.baseToggleButtonDivision.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.baseToggleButtonDivision.Name = "baseToggleButtonDivision";
            this.baseToggleButtonDivision.Size = new System.Drawing.Size(62, 37);
            this.baseToggleButtonDivision.TabIndex = 20;
            this.baseToggleButtonDivision.Text = "/ 2";
            this.baseToggleButtonDivision.UseVisualStyleBackColor = false;
            this.baseToggleButtonDivision.Click += new System.EventHandler(this.baseToggleButtonDivision_Click);
            // 
            // baseToggleButton0001
            // 
            this.baseToggleButton0001.BackColor = System.Drawing.Color.White;
            this.baseToggleButton0001.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.baseToggleButton0001.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButton0001.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseToggleButton0001.ForeColor = System.Drawing.Color.Black;
            this.baseToggleButton0001.Location = new System.Drawing.Point(420, 188);
            this.baseToggleButton0001.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.baseToggleButton0001.Name = "baseToggleButton0001";
            this.baseToggleButton0001.Size = new System.Drawing.Size(62, 37);
            this.baseToggleButton0001.TabIndex = 19;
            this.baseToggleButton0001.Text = "0.001";
            this.baseToggleButton0001.UseVisualStyleBackColor = false;
            this.baseToggleButton0001.Click += new System.EventHandler(this.baseToggleButton0001_Click);
            // 
            // baseToggleButton001
            // 
            this.baseToggleButton001.BackColor = System.Drawing.Color.White;
            this.baseToggleButton001.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.baseToggleButton001.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButton001.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseToggleButton001.ForeColor = System.Drawing.Color.Black;
            this.baseToggleButton001.Location = new System.Drawing.Point(352, 188);
            this.baseToggleButton001.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.baseToggleButton001.Name = "baseToggleButton001";
            this.baseToggleButton001.Size = new System.Drawing.Size(62, 37);
            this.baseToggleButton001.TabIndex = 18;
            this.baseToggleButton001.Text = "0.01";
            this.baseToggleButton001.UseVisualStyleBackColor = false;
            this.baseToggleButton001.Click += new System.EventHandler(this.baseToggleButton001_Click);
            // 
            // baseToggleButton01
            // 
            this.baseToggleButton01.BackColor = System.Drawing.Color.White;
            this.baseToggleButton01.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.baseToggleButton01.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButton01.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseToggleButton01.ForeColor = System.Drawing.Color.Black;
            this.baseToggleButton01.Location = new System.Drawing.Point(284, 188);
            this.baseToggleButton01.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.baseToggleButton01.Name = "baseToggleButton01";
            this.baseToggleButton01.Size = new System.Drawing.Size(62, 37);
            this.baseToggleButton01.TabIndex = 17;
            this.baseToggleButton01.Text = "0.1";
            this.baseToggleButton01.UseVisualStyleBackColor = false;
            this.baseToggleButton01.Click += new System.EventHandler(this.baseToggleButton01_Click);
            // 
            // baseToggleButton1
            // 
            this.baseToggleButton1.BackColor = System.Drawing.Color.White;
            this.baseToggleButton1.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.baseToggleButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButton1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseToggleButton1.ForeColor = System.Drawing.Color.Black;
            this.baseToggleButton1.Location = new System.Drawing.Point(216, 188);
            this.baseToggleButton1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.baseToggleButton1.Name = "baseToggleButton1";
            this.baseToggleButton1.Size = new System.Drawing.Size(62, 37);
            this.baseToggleButton1.TabIndex = 16;
            this.baseToggleButton1.Text = "1";
            this.baseToggleButton1.UseVisualStyleBackColor = false;
            this.baseToggleButton1.Click += new System.EventHandler(this.baseToggleButton1_Click);
            // 
            // flowLayoutPanelJogButtonComb_UVW
            // 
            this.flowLayoutPanelJogButtonComb_UVW.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.flowLayoutPanelJogButtonComb_UVW.Location = new System.Drawing.Point(126, 247);
            this.flowLayoutPanelJogButtonComb_UVW.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.flowLayoutPanelJogButtonComb_UVW.Name = "flowLayoutPanelJogButtonComb_UVW";
            this.flowLayoutPanelJogButtonComb_UVW.Size = new System.Drawing.Size(168, 220);
            this.flowLayoutPanelJogButtonComb_UVW.TabIndex = 0;
            this.flowLayoutPanelJogButtonComb_UVW.Visible = false;
            // 
            // flowLayoutPanelJogButtonAxisY
            // 
            this.flowLayoutPanelJogButtonAxisY.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.flowLayoutPanelJogButtonAxisY.Location = new System.Drawing.Point(4, 247);
            this.flowLayoutPanelJogButtonAxisY.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.flowLayoutPanelJogButtonAxisY.Name = "flowLayoutPanelJogButtonAxisY";
            this.flowLayoutPanelJogButtonAxisY.Size = new System.Drawing.Size(71, 220);
            this.flowLayoutPanelJogButtonAxisY.TabIndex = 8;
            // 
            // dataGridViewJogControl
            // 
            this.dataGridViewJogControl.AllowUserToAddRows = false;
            this.dataGridViewJogControl.AllowUserToResizeRows = false;
            this.dataGridViewJogControl.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewJogControl.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.dataGridViewJogControl.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewJogControl.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewJogControl.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewJogControl.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewJogControl.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            this.dataGridViewJogControl.Location = new System.Drawing.Point(6, 26);
            this.dataGridViewJogControl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridViewJogControl.MultiSelect = false;
            this.dataGridViewJogControl.Name = "dataGridViewJogControl";
            this.dataGridViewJogControl.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewJogControl.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewJogControl.RowHeadersVisible = false;
            this.dataGridViewJogControl.RowHeadersWidth = 51;
            this.dataGridViewJogControl.RowTemplate.Height = 27;
            this.dataGridViewJogControl.Size = new System.Drawing.Size(626, 154);
            this.dataGridViewJogControl.TabIndex = 5;
            this.dataGridViewJogControl.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.daragridview_buttonCellClick);
            this.dataGridViewJogControl.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewJogControl_CellValueChanged);
            // 
            // radioButtonContinuous
            // 
            this.radioButtonContinuous.AutoSize = true;
            this.radioButtonContinuous.ForeColor = System.Drawing.Color.Black;
            this.radioButtonContinuous.Location = new System.Drawing.Point(6, 209);
            this.radioButtonContinuous.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioButtonContinuous.Name = "radioButtonContinuous";
            this.radioButtonContinuous.Size = new System.Drawing.Size(86, 18);
            this.radioButtonContinuous.TabIndex = 3;
            this.radioButtonContinuous.TabStop = true;
            this.radioButtonContinuous.Text = "Continuous";
            this.radioButtonContinuous.UseVisualStyleBackColor = true;
            this.radioButtonContinuous.CheckedChanged += new System.EventHandler(this.radioButtonContinuous_CheckedChanged);
            // 
            // radioButtonStep
            // 
            this.radioButtonStep.AutoSize = true;
            this.radioButtonStep.ForeColor = System.Drawing.Color.Black;
            this.radioButtonStep.Location = new System.Drawing.Point(6, 185);
            this.radioButtonStep.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioButtonStep.Name = "radioButtonStep";
            this.radioButtonStep.Size = new System.Drawing.Size(51, 18);
            this.radioButtonStep.TabIndex = 4;
            this.radioButtonStep.TabStop = true;
            this.radioButtonStep.Text = "Step";
            this.radioButtonStep.UseVisualStyleBackColor = true;
            this.radioButtonStep.CheckedChanged += new System.EventHandler(this.radioButtonStep_CheckedChanged);
            // 
            // JogControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxJogControl);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "JogControl";
            this.Size = new System.Drawing.Size(648, 476);
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
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelJogButtonAxisX;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelJogButtonAxisY;
        private System.Windows.Forms.Timer timerJogControl;
        private WATGroupBox groupBoxJogControl;
        private BaseToggleButton baseToggleButtonMultiplication;
        private BaseToggleButton baseToggleButtonDivision;
        private BaseToggleButton baseToggleButton0001;
        private BaseToggleButton baseToggleButton001;
        private BaseToggleButton baseToggleButton01;
        private BaseToggleButton baseToggleButton1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelJogButtonComb_XZ_UL;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelJogButtonComb_XZ_LD;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelJogButtonAxisY2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelJogButtonComb_XY;
    }
}
