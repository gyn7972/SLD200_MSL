namespace QMC.Common.UI
{
    partial class XRsChartControl
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.baseGroupBoxMain = new QMC.Common.UI.BaseGroupBox();
            this.baseTextBoxMaxCount = new QMC.Common.UI.BaseTextBox();
            this.baseLabel3 = new QMC.Common.UI.BaseLabel();
            this.baseGroupBox2 = new QMC.Common.UI.BaseGroupBox();
            this.baseLabelStdev = new QMC.Common.UI.BaseLabel();
            this.baseTextBoxUSL = new QMC.Common.UI.BaseTextBox();
            this.baseLabel1 = new QMC.Common.UI.BaseLabel();
            this.baseTextBoxLSL = new QMC.Common.UI.BaseTextBox();
            this.baseLabel2 = new QMC.Common.UI.BaseLabel();
            this.comboBoxStandardDeviation = new System.Windows.Forms.ComboBox();
            this.comboBoxVIewItem = new System.Windows.Forms.ComboBox();
            this.baseLabelItem = new QMC.Common.UI.BaseLabel();
            this.baseButtonApply = new QMC.Common.UI.BaseButton();
            this.baseLabel7 = new QMC.Common.UI.BaseLabel();
            this.baseLabel4 = new QMC.Common.UI.BaseLabel();
            this.baseTextBoxStandardDeviation = new QMC.Common.UI.BaseTextBox();
            this.baseLabel5 = new QMC.Common.UI.BaseLabel();
            this.baseTextBoxCPK = new QMC.Common.UI.BaseTextBox();
            this.baseLabel6 = new QMC.Common.UI.BaseLabel();
            this.baseTextBoxCP = new QMC.Common.UI.BaseTextBox();
            this.chartRs = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartX = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.baseGroupBoxMain.SuspendLayout();
            this.baseGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartRs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartX)).BeginInit();
            this.SuspendLayout();
            // 
            // baseGroupBoxMain
            // 
            this.baseGroupBoxMain.Controls.Add(this.baseTextBoxMaxCount);
            this.baseGroupBoxMain.Controls.Add(this.baseLabel3);
            this.baseGroupBoxMain.Controls.Add(this.baseGroupBox2);
            this.baseGroupBoxMain.Controls.Add(this.baseLabel7);
            this.baseGroupBoxMain.Controls.Add(this.baseLabel4);
            this.baseGroupBoxMain.Controls.Add(this.baseTextBoxStandardDeviation);
            this.baseGroupBoxMain.Controls.Add(this.baseLabel5);
            this.baseGroupBoxMain.Controls.Add(this.baseTextBoxCPK);
            this.baseGroupBoxMain.Controls.Add(this.baseLabel6);
            this.baseGroupBoxMain.Controls.Add(this.baseTextBoxCP);
            this.baseGroupBoxMain.Controls.Add(this.chartRs);
            this.baseGroupBoxMain.Controls.Add(this.chartX);
            this.baseGroupBoxMain.ForeColor = System.Drawing.Color.White;
            this.baseGroupBoxMain.Location = new System.Drawing.Point(0, 3);
            this.baseGroupBoxMain.Name = "baseGroupBoxMain";
            this.baseGroupBoxMain.Size = new System.Drawing.Size(940, 775);
            this.baseGroupBoxMain.TabIndex = 0;
            this.baseGroupBoxMain.TabStop = false;
            this.baseGroupBoxMain.Text = "SPC";
            // 
            // baseTextBoxMaxCount
            // 
            this.baseTextBoxMaxCount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxMaxCount.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxMaxCount.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseTextBoxMaxCount.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxMaxCount.Location = new System.Drawing.Point(803, 121);
            this.baseTextBoxMaxCount.Name = "baseTextBoxMaxCount";
            this.baseTextBoxMaxCount.Size = new System.Drawing.Size(100, 19);
            this.baseTextBoxMaxCount.TabIndex = 5;
            this.baseTextBoxMaxCount.Visible = false;
            // 
            // baseLabel3
            // 
            this.baseLabel3.AutoSize = true;
            this.baseLabel3.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabel3.ForeColor = System.Drawing.Color.White;
            this.baseLabel3.Location = new System.Drawing.Point(745, 143);
            this.baseLabel3.Name = "baseLabel3";
            this.baseLabel3.Size = new System.Drawing.Size(96, 16);
            this.baseLabel3.TabIndex = 6;
            this.baseLabel3.Text = "Max Count";
            this.baseLabel3.Visible = false;
            // 
            // baseGroupBox2
            // 
            this.baseGroupBox2.Controls.Add(this.baseLabelStdev);
            this.baseGroupBox2.Controls.Add(this.baseTextBoxUSL);
            this.baseGroupBox2.Controls.Add(this.baseLabel1);
            this.baseGroupBox2.Controls.Add(this.baseTextBoxLSL);
            this.baseGroupBox2.Controls.Add(this.baseLabel2);
            this.baseGroupBox2.Controls.Add(this.comboBoxStandardDeviation);
            this.baseGroupBox2.Controls.Add(this.comboBoxVIewItem);
            this.baseGroupBox2.Controls.Add(this.baseLabelItem);
            this.baseGroupBox2.Controls.Add(this.baseButtonApply);
            this.baseGroupBox2.ForeColor = System.Drawing.Color.White;
            this.baseGroupBox2.Location = new System.Drawing.Point(15, 18);
            this.baseGroupBox2.Name = "baseGroupBox2";
            this.baseGroupBox2.Size = new System.Drawing.Size(187, 165);
            this.baseGroupBox2.TabIndex = 21;
            this.baseGroupBox2.TabStop = false;
            this.baseGroupBox2.Text = "Parameter";
            // 
            // baseLabelStdev
            // 
            this.baseLabelStdev.AutoSize = true;
            this.baseLabelStdev.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelStdev.ForeColor = System.Drawing.Color.White;
            this.baseLabelStdev.Location = new System.Drawing.Point(6, 50);
            this.baseLabelStdev.Name = "baseLabelStdev";
            this.baseLabelStdev.Size = new System.Drawing.Size(61, 16);
            this.baseLabelStdev.TabIndex = 17;
            this.baseLabelStdev.Text = "STDEV";
            // 
            // baseTextBoxUSL
            // 
            this.baseTextBoxUSL.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxUSL.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxUSL.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseTextBoxUSL.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxUSL.Location = new System.Drawing.Point(71, 75);
            this.baseTextBoxUSL.Name = "baseTextBoxUSL";
            this.baseTextBoxUSL.Size = new System.Drawing.Size(100, 19);
            this.baseTextBoxUSL.TabIndex = 1;
            // 
            // baseLabel1
            // 
            this.baseLabel1.AutoSize = true;
            this.baseLabel1.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabel1.ForeColor = System.Drawing.Color.White;
            this.baseLabel1.Location = new System.Drawing.Point(6, 76);
            this.baseLabel1.Name = "baseLabel1";
            this.baseLabel1.Size = new System.Drawing.Size(40, 16);
            this.baseLabel1.TabIndex = 2;
            this.baseLabel1.Text = "USL";
            // 
            // baseTextBoxLSL
            // 
            this.baseTextBoxLSL.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxLSL.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxLSL.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseTextBoxLSL.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxLSL.Location = new System.Drawing.Point(71, 100);
            this.baseTextBoxLSL.Name = "baseTextBoxLSL";
            this.baseTextBoxLSL.Size = new System.Drawing.Size(100, 19);
            this.baseTextBoxLSL.TabIndex = 3;
            // 
            // baseLabel2
            // 
            this.baseLabel2.AutoSize = true;
            this.baseLabel2.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabel2.ForeColor = System.Drawing.Color.White;
            this.baseLabel2.Location = new System.Drawing.Point(6, 101);
            this.baseLabel2.Name = "baseLabel2";
            this.baseLabel2.Size = new System.Drawing.Size(38, 16);
            this.baseLabel2.TabIndex = 4;
            this.baseLabel2.Text = "LSL";
            // 
            // comboBoxStandardDeviation
            // 
            this.comboBoxStandardDeviation.FormattingEnabled = true;
            this.comboBoxStandardDeviation.Items.AddRange(new object[] {
            "OverAll",
            "ReadIn"});
            this.comboBoxStandardDeviation.Location = new System.Drawing.Point(71, 48);
            this.comboBoxStandardDeviation.Name = "comboBoxStandardDeviation";
            this.comboBoxStandardDeviation.Size = new System.Drawing.Size(100, 20);
            this.comboBoxStandardDeviation.TabIndex = 14;
            this.comboBoxStandardDeviation.SelectedIndexChanged += new System.EventHandler(this.comboBoxStandardDeviation_SelectedIndexChanged);
            // 
            // comboBoxVIewItem
            // 
            this.comboBoxVIewItem.FormattingEnabled = true;
            this.comboBoxVIewItem.Items.AddRange(new object[] {
            "Vf1",
            "Vf2",
            "NTC"});
            this.comboBoxVIewItem.Location = new System.Drawing.Point(71, 22);
            this.comboBoxVIewItem.Name = "comboBoxVIewItem";
            this.comboBoxVIewItem.Size = new System.Drawing.Size(100, 20);
            this.comboBoxVIewItem.TabIndex = 15;
            this.comboBoxVIewItem.SelectedIndexChanged += new System.EventHandler(this.comboBoxVIewItem_SelectedIndexChanged);
            // 
            // baseLabelItem
            // 
            this.baseLabelItem.AutoSize = true;
            this.baseLabelItem.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelItem.ForeColor = System.Drawing.Color.White;
            this.baseLabelItem.Location = new System.Drawing.Point(6, 26);
            this.baseLabelItem.Name = "baseLabelItem";
            this.baseLabelItem.Size = new System.Drawing.Size(46, 16);
            this.baseLabelItem.TabIndex = 16;
            this.baseLabelItem.Text = "ITEM";
            // 
            // baseButtonApply
            // 
            this.baseButtonApply.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonApply.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonApply.Location = new System.Drawing.Point(9, 126);
            this.baseButtonApply.Name = "baseButtonApply";
            this.baseButtonApply.Size = new System.Drawing.Size(162, 30);
            this.baseButtonApply.TabIndex = 7;
            this.baseButtonApply.Text = "Apply";
            this.baseButtonApply.UseVisualStyleBackColor = false;
            this.baseButtonApply.Click += new System.EventHandler(this.baseButtonApply_Click);
            // 
            // baseLabel7
            // 
            this.baseLabel7.AutoSize = true;
            this.baseLabel7.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabel7.ForeColor = System.Drawing.Color.White;
            this.baseLabel7.Location = new System.Drawing.Point(208, 67);
            this.baseLabel7.Name = "baseLabel7";
            this.baseLabel7.Size = new System.Drawing.Size(0, 16);
            this.baseLabel7.TabIndex = 19;
            // 
            // baseLabel4
            // 
            this.baseLabel4.AutoSize = true;
            this.baseLabel4.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabel4.ForeColor = System.Drawing.Color.White;
            this.baseLabel4.Location = new System.Drawing.Point(745, 75);
            this.baseLabel4.Name = "baseLabel4";
            this.baseLabel4.Size = new System.Drawing.Size(61, 16);
            this.baseLabel4.TabIndex = 18;
            this.baseLabel4.Text = "STDEV";
            // 
            // baseTextBoxStandardDeviation
            // 
            this.baseTextBoxStandardDeviation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxStandardDeviation.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxStandardDeviation.Enabled = false;
            this.baseTextBoxStandardDeviation.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseTextBoxStandardDeviation.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxStandardDeviation.Location = new System.Drawing.Point(812, 73);
            this.baseTextBoxStandardDeviation.Name = "baseTextBoxStandardDeviation";
            this.baseTextBoxStandardDeviation.Size = new System.Drawing.Size(100, 19);
            this.baseTextBoxStandardDeviation.TabIndex = 13;
            // 
            // baseLabel5
            // 
            this.baseLabel5.AutoSize = true;
            this.baseLabel5.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabel5.ForeColor = System.Drawing.Color.White;
            this.baseLabel5.Location = new System.Drawing.Point(765, 48);
            this.baseLabel5.Name = "baseLabel5";
            this.baseLabel5.Size = new System.Drawing.Size(41, 16);
            this.baseLabel5.TabIndex = 12;
            this.baseLabel5.Text = "CPK";
            // 
            // baseTextBoxCPK
            // 
            this.baseTextBoxCPK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxCPK.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxCPK.Enabled = false;
            this.baseTextBoxCPK.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseTextBoxCPK.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxCPK.Location = new System.Drawing.Point(812, 48);
            this.baseTextBoxCPK.Name = "baseTextBoxCPK";
            this.baseTextBoxCPK.Size = new System.Drawing.Size(100, 19);
            this.baseTextBoxCPK.TabIndex = 11;
            // 
            // baseLabel6
            // 
            this.baseLabel6.AutoSize = true;
            this.baseLabel6.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabel6.ForeColor = System.Drawing.Color.White;
            this.baseLabel6.Location = new System.Drawing.Point(776, 23);
            this.baseLabel6.Name = "baseLabel6";
            this.baseLabel6.Size = new System.Drawing.Size(30, 16);
            this.baseLabel6.TabIndex = 10;
            this.baseLabel6.Text = "CP";
            // 
            // baseTextBoxCP
            // 
            this.baseTextBoxCP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxCP.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxCP.Enabled = false;
            this.baseTextBoxCP.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseTextBoxCP.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxCP.Location = new System.Drawing.Point(812, 23);
            this.baseTextBoxCP.Name = "baseTextBoxCP";
            this.baseTextBoxCP.Size = new System.Drawing.Size(100, 19);
            this.baseTextBoxCP.TabIndex = 9;
            // 
            // chartRs
            // 
            chartArea1.Name = "ChartArea1";
            this.chartRs.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartRs.Legends.Add(legend1);
            this.chartRs.Location = new System.Drawing.Point(15, 473);
            this.chartRs.Name = "chartRs";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.MarkerBorderWidth = 30;
            series1.MarkerColor = System.Drawing.Color.Blue;
            series1.MarkerSize = 10;
            series1.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Square;
            series1.Name = "Series1";
            series1.YValuesPerPoint = 2;
            this.chartRs.Series.Add(series1);
            this.chartRs.Size = new System.Drawing.Size(906, 250);
            this.chartRs.TabIndex = 8;
            this.chartRs.Text = "000000000000000000000000000000000000000000000000000000000000000000000000000000000" +
    "00000000000000000000000000000000000000000000000000000000000000000000000000000000" +
    "0000000000000000000.";
            // 
            // chartX
            // 
            chartArea2.Name = "ChartArea1";
            this.chartX.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartX.Legends.Add(legend2);
            this.chartX.Location = new System.Drawing.Point(15, 189);
            this.chartX.Name = "chartX";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "Legend1";
            series2.MarkerBorderWidth = 30;
            series2.MarkerColor = System.Drawing.Color.Blue;
            series2.MarkerSize = 10;
            series2.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Square;
            series2.Name = "X";
            series2.YValuesPerPoint = 2;
            this.chartX.Series.Add(series2);
            this.chartX.Size = new System.Drawing.Size(906, 250);
            this.chartX.TabIndex = 0;
            this.chartX.Text = "chart1";
            // 
            // XRsChartControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.Controls.Add(this.baseGroupBoxMain);
            this.Name = "XRsChartControl";
            this.Size = new System.Drawing.Size(940, 780);
            this.Tag = "";
            this.baseGroupBoxMain.ResumeLayout(false);
            this.baseGroupBoxMain.PerformLayout();
            this.baseGroupBox2.ResumeLayout(false);
            this.baseGroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartRs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartX)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BaseGroupBox baseGroupBoxMain;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartX;
        private BaseLabel baseLabel3;
        private BaseTextBox baseTextBoxMaxCount;
        private BaseLabel baseLabel2;
        private BaseTextBox baseTextBoxLSL;
        private BaseLabel baseLabel1;
        private BaseTextBox baseTextBoxUSL;
        private BaseButton baseButtonApply;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartRs;
        private System.Windows.Forms.ComboBox comboBoxStandardDeviation;
        private BaseTextBox baseTextBoxStandardDeviation;
        private BaseLabel baseLabel5;
        private BaseTextBox baseTextBoxCPK;
        private BaseLabel baseLabel6;
        private BaseTextBox baseTextBoxCP;
        private BaseLabel baseLabelItem;
        private System.Windows.Forms.ComboBox comboBoxVIewItem;
        private BaseLabel baseLabel4;
        private BaseLabel baseLabelStdev;
        private BaseLabel baseLabel7;
        private BaseGroupBox baseGroupBox2;
    }
}
