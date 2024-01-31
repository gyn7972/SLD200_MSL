using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using QMC.Common;
using QMC.Common.Parts;

namespace QMC.Common.UI
{
    public delegate void SpcControlButtonClickHandler(XRsChartControl.ButtonType type);
    public delegate void SpcControlComboboxSelectedChangedHandler(int index);
    public delegate void SpcControlComboboxSTDEVSelectedChangedHandler(int index);
    public partial class XRsChartControl : UserControl
    {
        #region Define
        public enum ButtonType
        {
            Apply,
        }

        public SpcControlButtonClickHandler ButtonClick { get; set; }
        public SpcControlComboboxSelectedChangedHandler ComboboxSelectedChanged { get; set; }
        public SpcControlComboboxSTDEVSelectedChangedHandler ComboboxSTDEVSelectedChanged { get; set; }
        #endregion

        #region Field
        public Series Series_1;
        public Series Series_2;

        public Title m_TitleX;
        public Title m_TitleRs;

        public List<double> m_Datas;
        public double m_USL;
        public double m_LSL;
        public double m_UCL;
        public double m_LCL;
        public double m_UCLRs;
        #endregion

        #region Constructor
        public XRsChartControl()
        {
            InitializeComponent();
            Init();
        }
        #endregion

        #region Method
        public void Init()
        {
            // 타이틀 객체 생성
            m_TitleX = new Title();
            m_TitleX.Text = "X Chart";
            m_TitleX.ForeColor = Color.Black;
            m_TitleX.Font = new Font("맑은고딕", 25, FontStyle.Bold);
            chartX.Titles.Add(m_TitleX);

            m_TitleRs = new Title();
            m_TitleRs.Text = "Rs Chart";
            m_TitleRs.ForeColor = Color.Black;
            m_TitleRs.Font = new Font("맑은고딕", 25, FontStyle.Bold);
            chartRs.Titles.Add(m_TitleRs);

            if (this.comboBoxVIewItem != null)
            {
                this.comboBoxVIewItem.SelectedIndex = 0;
            }

            if (this.comboBoxStandardDeviation != null)
            {
                this.comboBoxStandardDeviation.SelectedIndex = 0;
            }

            //if (m_SPC.Config != null)
            //{
            //    this.baseTextBoxUSL.Text = m_SPC.Config.USL.ToString();
            //    this.baseTextBoxLSL.Text = m_SPC.Config.LSL.ToString();
            //}

            //m_USL = 3;
            //m_LSL = 1;
        }
        public void UpdateChartXControl(List<double> datas)
        {
            if (datas != null)
            {
                UpdateDataChartXData(datas);
            }
        }

        protected void UpdateDataChartXData(List<double> datas)
        {
            if (chartX != null)
            {
                chartX.Series.Clear();

                // 객체 생성
                Series USL = chartX.Series.Add("USL");
                Series LSL = chartX.Series.Add("LSL");
                Series UCL = chartX.Series.Add("UCL");
                Series LCL = chartX.Series.Add("LCL");
                Series data = chartX.Series.Add(this.comboBoxVIewItem.SelectedItem.ToString());

                // 범례 설정
                USL.LegendText = "USL";
                LSL.LegendText = "LSL";
                UCL.LegendText = "UCL";
                LCL.LegendText = "LCL";
                data.LegendText = this.comboBoxVIewItem.SelectedItem.ToString();

                // 차트 종류 설정
                USL.ChartType = SeriesChartType.Line;   // 선
                LSL.ChartType = SeriesChartType.Line;   // 선
                UCL.ChartType = SeriesChartType.Line;   // 선
                LCL.ChartType = SeriesChartType.Line;   // 선
                data.ChartType = SeriesChartType.Line;   // 선

                data.MarkerSize = 2;

                // 차트 색상 설정
                USL.Color = Color.Red;
                LSL.Color = Color.Red;
                UCL.Color = Color.Blue;
                LCL.Color = Color.Blue;
                data.Color = Color.LimeGreen;
                data.MarkerColor = Color.Blue;
                data.MarkerStyle = MarkerStyle.Square;

                // 차트 굵기 설정
                USL.BorderWidth = 3;
                LSL.BorderWidth = 3;
                UCL.BorderWidth = 3;
                LCL.BorderWidth = 3;
                data.BorderWidth = 3;

                // 범례1 데이터
                //DB 연결해서... Force, Position값 넣어주면됨.. 
                //아래는 테스트용..
                for (int i = 0; i < datas.Count; i++)
                {
                    USL.Points.AddXY(i, m_USL);
                    LSL.Points.AddXY(i, m_LSL);
                    UCL.Points.AddXY(i, m_UCL);
                    LCL.Points.AddXY(i, m_LCL);
                    data.Points.AddXY(i, datas[i]);
                }
            }
        }

        public void UpdateChartRsControl(List<double> datas)
        {
            if (datas != null)
            {
                UpdateDataChartRsData(datas);
            }
        }

        protected void UpdateDataChartRsData(List<double> datas)
        {
            if (chartRs != null)
            {
                chartRs.Series.Clear();

                // 객체 생성
                Series USL = chartRs.Series.Add("UCL");
                Series data = chartRs.Series.Add(this.comboBoxVIewItem.SelectedItem.ToString());

                // 범례 설정
                USL.LegendText = "UCL";
                data.LegendText = this.comboBoxVIewItem.SelectedItem.ToString();

                // 차트 종류 설정
                USL.ChartType = SeriesChartType.Line;   // 선
                data.ChartType = SeriesChartType.StepLine;   // 선

                // 차트 색상 설정
                USL.Color = Color.Red;
                data.Color = Color.LimeGreen;

                // 차트 굵기 설정
                USL.BorderWidth = 3;
                data.BorderWidth = 3;

                // 범례1 데이터
                //DB 연결해서... Force, Position값 넣어주면됨.. 
                //아래는 테스트용..
                for (int i = 0; i < datas.Count; i++)
                {
                    USL.Points.AddXY(i, m_UCLRs);
                    data.Points.AddXY(i, datas[i]);
                }
            }
        }

        public void InitSeries_1()
        {
            Series_1 = chartX.Series.Add("Series_1");
        }

        private void UpdateBaseLine()
        {



        }

        public void UpdateCP(double value)
        {
            this.baseTextBoxCP.Text = value.ToString();
        }

        public void UpdateCPK(double value)
        {
            this.baseTextBoxCPK.Text = value.ToString();
        }

        public void UpdateSTDEV(double value)
        {
            this.baseTextBoxStandardDeviation.Text = value.ToString();
        }

        public void SetItem(Enum items)
        {

            //this.comboBoxVIewItem.Items.Add(items);

        }

        public int GetSelectedItemCombo()
        {
            int ret = 0;

            if (this.comboBoxVIewItem != null)
            {
                ret = this.comboBoxVIewItem.SelectedIndex;
            }

            return ret;
        }

        public int GetSelectedSTDEVItemCombo()
        {
            int ret = 0;

            if (this.comboBoxVIewItem != null)
            {
                ret = this.comboBoxStandardDeviation.SelectedIndex;
            }

            return ret;
        }

        public double GetUSL()
        {
            double ret = 0;

            if (this.baseTextBoxUSL.Text != string.Empty)
                ret = double.Parse(this.baseTextBoxUSL.Text);

            return ret;
        }

        public double GetLSL()
        {
            double ret = 0;

            if (this.baseTextBoxLSL.Text != string.Empty)
                ret = double.Parse(this.baseTextBoxLSL.Text);

            return ret;
        }

        public void SetUSLSL(double usl, double lsl)
        {
            this.baseTextBoxUSL.Text = usl.ToString();
            this.baseTextBoxLSL.Text = lsl.ToString();
        }

        public void SetUCLCL(double ucl, double lcl)
        {
            this.m_UCL = ucl;
            this.m_LCL = lcl;
        }
        #endregion

        #region Event Handler
        private void baseButtonApply_Click(object sender, EventArgs e)
        {
            if (ButtonClick != null)
            {
                //m_SPC.Config.USL = Convert.ToDouble(baseTextBoxUSL.Text);
                //m_SPC.Config.LSL = Convert.ToDouble(baseTextBoxLSL.Text);
                //m_SPC.Config.CpkLimit = Convert.ToDouble(baseTextBoxMaxCount.Text);
                ButtonClick(ButtonType.Apply);
            }
        }
        #endregion

        private void comboBoxVIewItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboboxSelectedChanged != null)
                ComboboxSelectedChanged(GetSelectedItemCombo());
        }

        private void comboBoxStandardDeviation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboboxSTDEVSelectedChanged != null)
                ComboboxSTDEVSelectedChanged(GetSelectedSTDEVItemCombo());
        }
    }
}
