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

namespace QMC.Core.Laser.winforms
{
    public partial class FormPowerMap : Form
    {
        IPowerMap map;

        public FormPowerMap(IPowerMap map)
        {
            InitializeComponent();
            this.map = map;
            this.Shown += FormPowerMap_Shown;
        }

        private void FormPowerMap_Shown(object sender, EventArgs e)
        {
            foreach (var data in this.map.Data)
            {
                string name = data.Key;
                Dictionary<double, double> setXAndWatt = data.Value;
                this.chart1.Series.Add(name);
                this.chart1.Series[name].XValueType = ChartValueType.Double;
                this.chart1.Series[name].YValueType = ChartValueType.Double;
                foreach (var xAndWatt in setXAndWatt)
                {
                    this.chart1.Series[name].Points.AddXY(xAndWatt.Key, xAndWatt.Value);
                }
                this.chart1.Series[name].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                this.chart1.Series[name].MarkerStyle = MarkerStyle.Square;
                this.chart1.Series[name].BorderWidth = 2;
            }
        }
    }
}
