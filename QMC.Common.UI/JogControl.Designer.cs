
using System.Drawing;
using System.Windows.Forms;

namespace QMC.Common.UI
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.radioButtonContinuous = new System.Windows.Forms.RadioButton();
            this.radioButtonStep = new System.Windows.Forms.RadioButton();
            this.flowLayoutPanelJogButtonComb = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanelJogButtonAxisX = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanelJogButtonAxisY = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBoxJogControl = new System.Windows.Forms.GroupBox();
            this.baseGroupBox2 = new QMC.Common.UI.BaseGroupBox();
            this.buttonPercentPlus = new System.Windows.Forms.Button();
            this.buttonPercentMinus = new System.Windows.Forms.Button();
            this.baseTextBoxPercent = new QMC.Common.UI.BaseTextBox();
            this.baseGroupBox1 = new QMC.Common.UI.BaseGroupBox();
            this.baseTextBoxStep = new QMC.Common.UI.BaseTextBox();
            this.baseToggleButton0001 = new QMC.Common.UI.BaseToggleButton();
            this.baseToggleButton001 = new QMC.Common.UI.BaseToggleButton();
            this.baseToggleButton01 = new QMC.Common.UI.BaseToggleButton();
            this.baseToggleButton1 = new QMC.Common.UI.BaseToggleButton();
            this.dataGridViewJogControl = new QMC.Common.UI.BaseDataGridView();
            this.groupBoxJogControl.SuspendLayout();
            this.baseGroupBox2.SuspendLayout();
            this.baseGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewJogControl)).BeginInit();
            this.SuspendLayout();
            // 
            // radioButtonContinuous
            // 
            this.radioButtonContinuous.AutoSize = true;
            this.radioButtonContinuous.BackColor = System.Drawing.Color.Gainsboro;
            this.radioButtonContinuous.ForeColor = System.Drawing.Color.Black;
            this.radioButtonContinuous.Location = new System.Drawing.Point(6, 156);
            this.radioButtonContinuous.Name = "radioButtonContinuous";
            this.radioButtonContinuous.Size = new System.Drawing.Size(87, 16);
            this.radioButtonContinuous.TabIndex = 3;
            this.radioButtonContinuous.TabStop = true;
            this.radioButtonContinuous.Text = "Continuous";
            this.radioButtonContinuous.UseVisualStyleBackColor = false;
            this.radioButtonContinuous.CheckedChanged += new System.EventHandler(this.radioButtonContinuous_CheckedChanged);
            // 
            // radioButtonStep
            // 
            this.radioButtonStep.AutoSize = true;
            this.radioButtonStep.BackColor = System.Drawing.Color.Gainsboro;
            this.radioButtonStep.ForeColor = System.Drawing.Color.Black;
            this.radioButtonStep.Location = new System.Drawing.Point(6, 134);
            this.radioButtonStep.Name = "radioButtonStep";
            this.radioButtonStep.Size = new System.Drawing.Size(48, 16);
            this.radioButtonStep.TabIndex = 4;
            this.radioButtonStep.TabStop = true;
            this.radioButtonStep.Text = "Step";
            this.radioButtonStep.UseVisualStyleBackColor = false;
            this.radioButtonStep.CheckedChanged += new System.EventHandler(this.radioButtonStep_CheckedChanged);
            // 
            // flowLayoutPanelJogButtonComb
            // 
            this.flowLayoutPanelJogButtonComb.Location = new System.Drawing.Point(187, 207);
            this.flowLayoutPanelJogButtonComb.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.flowLayoutPanelJogButtonComb.Name = "flowLayoutPanelJogButtonComb";
            this.flowLayoutPanelJogButtonComb.Size = new System.Drawing.Size(168, 168);
            this.flowLayoutPanelJogButtonComb.TabIndex = 0;
            // 
            // flowLayoutPanelJogButtonAxisX
            // 
            this.flowLayoutPanelJogButtonAxisX.Location = new System.Drawing.Point(324, 178);
            this.flowLayoutPanelJogButtonAxisX.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.flowLayoutPanelJogButtonAxisX.Name = "flowLayoutPanelJogButtonAxisX";
            this.flowLayoutPanelJogButtonAxisX.Size = new System.Drawing.Size(157, 152);
            this.flowLayoutPanelJogButtonAxisX.TabIndex = 7;
            // 
            // flowLayoutPanelJogButtonAxisY
            // 
            this.flowLayoutPanelJogButtonAxisY.Location = new System.Drawing.Point(6, 207);
            this.flowLayoutPanelJogButtonAxisY.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.flowLayoutPanelJogButtonAxisY.Name = "flowLayoutPanelJogButtonAxisY";
            this.flowLayoutPanelJogButtonAxisY.Size = new System.Drawing.Size(167, 133);
            this.flowLayoutPanelJogButtonAxisY.TabIndex = 8;
            // 
            // groupBoxJogControl
            // 
            this.groupBoxJogControl.Controls.Add(this.baseGroupBox2);
            this.groupBoxJogControl.Controls.Add(this.baseGroupBox1);
            this.groupBoxJogControl.Controls.Add(this.flowLayoutPanelJogButtonComb);
            this.groupBoxJogControl.Controls.Add(this.flowLayoutPanelJogButtonAxisY);
            this.groupBoxJogControl.Controls.Add(this.flowLayoutPanelJogButtonAxisX);
            this.groupBoxJogControl.Controls.Add(this.dataGridViewJogControl);
            this.groupBoxJogControl.Controls.Add(this.radioButtonContinuous);
            this.groupBoxJogControl.Controls.Add(this.radioButtonStep);
            this.groupBoxJogControl.ForeColor = System.Drawing.Color.DarkBlue;
            this.groupBoxJogControl.Location = new System.Drawing.Point(0, 0);
            this.groupBoxJogControl.Name = "groupBoxJogControl";
            this.groupBoxJogControl.Size = new System.Drawing.Size(472, 380);
            this.groupBoxJogControl.TabIndex = 9;
            this.groupBoxJogControl.TabStop = false;
            this.groupBoxJogControl.Text = "Jog Control";
            // 
            // baseGroupBox2
            // 
            this.baseGroupBox2.Controls.Add(this.buttonPercentPlus);
            this.baseGroupBox2.Controls.Add(this.buttonPercentMinus);
            this.baseGroupBox2.Controls.Add(this.baseTextBoxPercent);
            this.baseGroupBox2.ForeColor = System.Drawing.Color.White;
            this.baseGroupBox2.Location = new System.Drawing.Point(400, 231);
            this.baseGroupBox2.Name = "baseGroupBox2";
            this.baseGroupBox2.Size = new System.Drawing.Size(66, 145);
            this.baseGroupBox2.TabIndex = 1;
            this.baseGroupBox2.TabStop = false;
            this.baseGroupBox2.Text = "Speed  (%)";
            // 
            // buttonPercentPlus
            // 
            this.buttonPercentPlus.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonPercentPlus.Image = global::QMC.Common.UI.Properties.Resources.Up;
            this.buttonPercentPlus.Location = new System.Drawing.Point(7, 33);
            this.buttonPercentPlus.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonPercentPlus.Name = "buttonPercentPlus";
            this.buttonPercentPlus.Size = new System.Drawing.Size(54, 32);
            this.buttonPercentPlus.TabIndex = 20;
            this.buttonPercentPlus.UseVisualStyleBackColor = false;
            this.buttonPercentPlus.Click += new System.EventHandler(this.basePercentButton_Click);
            // 
            // buttonPercentMinus
            // 
            this.buttonPercentMinus.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonPercentMinus.Image = global::QMC.Common.UI.Properties.Resources.Down;
            this.buttonPercentMinus.Location = new System.Drawing.Point(7, 104);
            this.buttonPercentMinus.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonPercentMinus.Name = "buttonPercentMinus";
            this.buttonPercentMinus.Size = new System.Drawing.Size(54, 32);
            this.buttonPercentMinus.TabIndex = 21;
            this.buttonPercentMinus.UseVisualStyleBackColor = false;
            this.buttonPercentMinus.Click += new System.EventHandler(this.basePercentButton_Click);
            // 
            // baseTextBoxPercent
            // 
            this.baseTextBoxPercent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxPercent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxPercent.Font = new System.Drawing.Font("굴림", 15F);
            this.baseTextBoxPercent.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxPercent.Location = new System.Drawing.Point(7, 73);
            this.baseTextBoxPercent.Name = "baseTextBoxPercent";
            this.baseTextBoxPercent.Size = new System.Drawing.Size(54, 23);
            this.baseTextBoxPercent.TabIndex = 10;
            this.baseTextBoxPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // baseGroupBox1
            // 
            this.baseGroupBox1.Controls.Add(this.baseTextBoxStep);
            this.baseGroupBox1.Controls.Add(this.baseToggleButton0001);
            this.baseGroupBox1.Controls.Add(this.baseToggleButton001);
            this.baseGroupBox1.Controls.Add(this.baseToggleButton01);
            this.baseGroupBox1.Controls.Add(this.baseToggleButton1);
            this.baseGroupBox1.Font = new System.Drawing.Font("굴림", 9F);
            this.baseGroupBox1.ForeColor = System.Drawing.Color.White;
            this.baseGroupBox1.Location = new System.Drawing.Point(400, 15);
            this.baseGroupBox1.Name = "baseGroupBox1";
            this.baseGroupBox1.Size = new System.Drawing.Size(66, 210);
            this.baseGroupBox1.TabIndex = 0;
            this.baseGroupBox1.TabStop = false;
            this.baseGroupBox1.Text = "Pitch  (mm)";
            // 
            // baseTextBoxStep
            // 
            this.baseTextBoxStep.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxStep.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxStep.Font = new System.Drawing.Font("굴림", 15F);
            this.baseTextBoxStep.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxStep.Location = new System.Drawing.Point(6, 33);
            this.baseTextBoxStep.Name = "baseTextBoxStep";
            this.baseTextBoxStep.Size = new System.Drawing.Size(54, 23);
            this.baseTextBoxStep.TabIndex = 22;
            this.baseTextBoxStep.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // baseToggleButton0001
            // 
            this.baseToggleButton0001.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseToggleButton0001.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButton0001.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseToggleButton0001.Location = new System.Drawing.Point(6, 164);
            this.baseToggleButton0001.Name = "baseToggleButton0001";
            this.baseToggleButton0001.Size = new System.Drawing.Size(54, 30);
            this.baseToggleButton0001.TabIndex = 16;
            this.baseToggleButton0001.Tag = "0.001";
            this.baseToggleButton0001.Text = "0.001";
            this.baseToggleButton0001.UseVisualStyleBackColor = false;
            this.baseToggleButton0001.Click += new System.EventHandler(this.baseToggleButton_Click);
            // 
            // baseToggleButton001
            // 
            this.baseToggleButton001.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseToggleButton001.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButton001.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseToggleButton001.Location = new System.Drawing.Point(6, 131);
            this.baseToggleButton001.Name = "baseToggleButton001";
            this.baseToggleButton001.Size = new System.Drawing.Size(54, 30);
            this.baseToggleButton001.TabIndex = 15;
            this.baseToggleButton001.Tag = "0.01";
            this.baseToggleButton001.Text = "0.01";
            this.baseToggleButton001.UseVisualStyleBackColor = false;
            this.baseToggleButton001.Click += new System.EventHandler(this.baseToggleButton_Click);
            // 
            // baseToggleButton01
            // 
            this.baseToggleButton01.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseToggleButton01.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButton01.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseToggleButton01.Location = new System.Drawing.Point(6, 98);
            this.baseToggleButton01.Name = "baseToggleButton01";
            this.baseToggleButton01.Size = new System.Drawing.Size(54, 30);
            this.baseToggleButton01.TabIndex = 14;
            this.baseToggleButton01.Tag = "0.1";
            this.baseToggleButton01.Text = "0.1";
            this.baseToggleButton01.UseVisualStyleBackColor = false;
            this.baseToggleButton01.Click += new System.EventHandler(this.baseToggleButton_Click);
            // 
            // baseToggleButton1
            // 
            this.baseToggleButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseToggleButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButton1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseToggleButton1.Location = new System.Drawing.Point(6, 65);
            this.baseToggleButton1.Name = "baseToggleButton1";
            this.baseToggleButton1.Size = new System.Drawing.Size(54, 30);
            this.baseToggleButton1.TabIndex = 13;
            this.baseToggleButton1.Tag = "1";
            this.baseToggleButton1.Text = "1";
            this.baseToggleButton1.UseVisualStyleBackColor = false;
            this.baseToggleButton1.Click += new System.EventHandler(this.baseToggleButton_Click);
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
            dataGridViewCellStyle1.Font = new System.Drawing.Font("굴림", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewJogControl.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewJogControl.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewJogControl.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewJogControl.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            this.dataGridViewJogControl.Location = new System.Drawing.Point(6, 16);
            this.dataGridViewJogControl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridViewJogControl.MultiSelect = false;
            this.dataGridViewJogControl.Name = "dataGridViewJogControl";
            this.dataGridViewJogControl.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("굴림", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewJogControl.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewJogControl.RowHeadersVisible = false;
            this.dataGridViewJogControl.RowHeadersWidth = 51;
            this.dataGridViewJogControl.RowTemplate.Height = 27;
            this.dataGridViewJogControl.Size = new System.Drawing.Size(388, 110);
            this.dataGridViewJogControl.TabIndex = 5;
            this.dataGridViewJogControl.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.daragridview_buttonCellClick);
            this.dataGridViewJogControl.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewJogControl_CellValueChanged);
            // 
            // JogControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxJogControl);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "JogControl";
            this.Size = new System.Drawing.Size(473, 382);
            this.groupBoxJogControl.ResumeLayout(false);
            this.groupBoxJogControl.PerformLayout();
            this.baseGroupBox2.ResumeLayout(false);
            this.baseGroupBox2.PerformLayout();
            this.baseGroupBox1.ResumeLayout(false);
            this.baseGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewJogControl)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.RadioButton radioButtonContinuous;
        private System.Windows.Forms.RadioButton radioButtonStep;
        public BaseDataGridView dataGridViewJogControl;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelJogButtonComb;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelJogButtonAxisX;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelJogButtonAxisY;
        private System.Windows.Forms.GroupBox groupBoxJogControl;
        private BaseGroupBox baseGroupBox2;
        private BaseTextBox baseTextBoxPercent;
        private BaseGroupBox baseGroupBox1;
        private BaseTextBox baseTextBoxStep;
        private BaseToggleButton baseToggleButton0001;
        private BaseToggleButton baseToggleButton001;
        private BaseToggleButton baseToggleButton01;
        private BaseToggleButton baseToggleButton1;
        private System.Windows.Forms.Button buttonPercentPlus;
        private System.Windows.Forms.Button buttonPercentMinus;
    }
}
