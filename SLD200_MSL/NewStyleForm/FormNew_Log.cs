using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QMC.Common;
using QMC.Common.UI;

namespace SLD200_MSL
{
    public partial class FormNew_Log : Form
    {
        private bool m_bFormVisible = false; // 실제 Show 상태 여부

        public FormNew_Log()
        {
            InitializeComponent();

            dataGridView_Log.AllowUserToResizeRows = false;
            dataGridView_Log.RowHeadersVisible = false;
            dataGridView_Log.ReadOnly = true;
            dataGridView_Log.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView_Log.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView_Log.AutoGenerateColumns = false;

            dataGridView_Log_LaserPower.AllowUserToResizeRows = false;
            dataGridView_Log_LaserPower.RowHeadersVisible = false;
            dataGridView_Log_LaserPower.ReadOnly = true;
            dataGridView_Log_LaserPower.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView_Log_LaserPower.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView_Log_LaserPower.AutoGenerateColumns = false;
            
            InitGrid();
            InitGrid_LaserPower();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (!this.Created)
                return;

            if (this.Visible && !m_bFormVisible)
            {
                m_bFormVisible = true;
                // OnShowRecipeForm();
            }
            else if (!this.Visible && m_bFormVisible)
            {
                m_bFormVisible = false;
                //OnHideRecipeForm();
            }
        }

        private void InitGrid()
        {
            dataGridView_Log.Columns.Clear(); // 중복 방지

            dataGridView_Log.AutoGenerateColumns = false;
            dataGridView_Log.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView_Log.ReadOnly = true;

            AddGridColumn("StartTime", "StartTime");
            AddGridColumn("EndTime", "EndTime");
            AddGridColumn("RecipeName", "RecipeName");
            AddGridColumn("DrawingName", "DrawingName");
            AddGridColumn("CompletedCount", "CompletedCount");
        }

        private void InitGrid_LaserPower()
        {
            dataGridView_Log_LaserPower.Columns.Clear(); // 중복 방지

            dataGridView_Log_LaserPower.AutoGenerateColumns = false;
            dataGridView_Log_LaserPower.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView_Log_LaserPower.ReadOnly = true;

            // 셀 크기 자동 조정
            dataGridView_Log_LaserPower.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None; // 중요
            dataGridView_Log_LaserPower.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView_Log_LaserPower.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            AddGridColumn_LaserPower_Fill("Time", "Time", 100);
            AddGridColumn_LaserPower_Fill("Target", "Target", 100);
            AddGridColumn_LaserPower_Fill("Type", "Type", 80);
            AddGridColumn_LaserPower_Fill("Value", "Value", 80);
            AddGridColumn_LaserPower_Fill("PowerPercent", "Power(%)", 90);
            AddGridColumn_LaserPower_Fill("Frequency", "Freq(kHz)", 90);
            AddGridColumn_LaserPower_Fill("PulseWidth", "PulseWidth(µs)", 100);
            AddGridColumn_LaserPower_Fill("DutyCycle", "DutyCycle(%)", 100);

        }


        // 기존 코드
        //private void InitGrid_LaserPower()
        //{
        //    dataGridView_Log_LaserPower.Columns.Clear(); // 중복 방지

        //    dataGridView_Log_LaserPower.AutoGenerateColumns = false;
        //    dataGridView_Log_LaserPower.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        //    dataGridView_Log_LaserPower.ReadOnly = true;

        //    AddGridColumn_LaserPower("Time", "Time");
        //    AddGridColumn_LaserPower("Target", "Target");
        //    AddGridColumn_LaserPower("Type", "Type");
        //    AddGridColumn_LaserPower("Value", "Value");
        //}


        private void AddGridColumn(string name, string headerText)
        {
            var column = new DataGridViewTextBoxColumn
            {
                Name = name,
                HeaderText = headerText,
                DataPropertyName = name,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };
            dataGridView_Log.Columns.Add(column);
        }

        private void AddGridColumn_LaserPower(string name, string headerText)
        {
            var column = new DataGridViewTextBoxColumn
            {
                Name = name,
                HeaderText = headerText,
                DataPropertyName = name,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };
            dataGridView_Log_LaserPower.Columns.Add(column);
        }
        private void AddGridColumn_LaserPower_Fill(string name, string header, int minWidth)
        {
            var col = new DataGridViewTextBoxColumn
            {
                Name = name,
                HeaderText = header,
                DataPropertyName = name,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                MinimumWidth = minWidth
            };
            dataGridView_Log_LaserPower.Columns.Add(col);
        }

        private void tabPage_LOT_Click(object sender, EventArgs e)
        {

        }

        private void baseButton_Log_Search_Click(object sender, EventArgs e)
        {
            DateTime startTime = dateTime_Log_Search_StartTime.Value;
            DateTime endTime = dateTime_Log_Search_EndTime.Value;

            SearchClick(startTime, endTime);
        }
        private void SearchClick(DateTime startTime, DateTime endTime)
        {
            dataGridView_Log.Rows.Clear();

            string logFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LotLog");

            for (var day = startTime.Date; day <= endTime.Date; day = day.AddDays(1))
            {
                string logFile = Path.Combine(logFolder, $"LotLog_{day:yyyyMMdd}.csv");
                if (!File.Exists(logFile)) continue;

                string[] lines = File.ReadAllLines(logFile, new UTF8Encoding(true));

                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length < 5) continue;

                    int rowIndex = dataGridView_Log.Rows.Add();
                    dataGridView_Log.Rows[rowIndex].Cells["StartTime"].Value = parts[0];
                    dataGridView_Log.Rows[rowIndex].Cells["EndTime"].Value = parts[1];
                    dataGridView_Log.Rows[rowIndex].Cells["RecipeName"].Value = parts[2];
                    dataGridView_Log.Rows[rowIndex].Cells["DrawingName"].Value = parts[3];
                    dataGridView_Log.Rows[rowIndex].Cells["CompletedCount"].Value = parts[4];
                }
            }
        }

        private void SearchClick_LaserPower(DateTime startTime, DateTime endTime)
        {
            dataGridView_Log_LaserPower.Rows.Clear();

            string logFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LaserPowerLog");

            for (var day = startTime.Date; day <= endTime.Date; day = day.AddDays(1))
            {
                string logFile = Path.Combine(logFolder, $"LaserPowerMeasureLog_{day:yyyyMMdd}.csv");
                if (!File.Exists(logFile))
                    continue;

                string[] lines = File.ReadAllLines(logFile, new UTF8Encoding(true));

                foreach (var line in lines)
                {
                    // 헤더 무시
                    if (line.StartsWith("Timestamp"))
                        continue;

                    var parts = line.Split(',');
                    if (parts.Length < 4)
                        continue;

                    string time = parts[0];
                    string target = parts[1];
                    string countType = parts[2];
                    string value = parts[3];

                    string powerPercent = parts.Length > 4 ? parts[4] : "";
                    string frequency = parts.Length > 5 ? parts[5] : "";
                    string pulseWidth = parts.Length > 6 ? parts[6] : "";
                    string dutyCycle = parts.Length > 7 ? parts[7] : "";

                    int row = dataGridView_Log_LaserPower.Rows.Add();
                    var rowCells = dataGridView_Log_LaserPower.Rows[row].Cells;

                    rowCells["Time"].Value = time;
                    rowCells["Target"].Value = target;
                    rowCells["Type"].Value = countType;
                    rowCells["Value"].Value = value;

                    rowCells["PowerPercent"].Value = powerPercent;
                    rowCells["Frequency"].Value = frequency;
                    rowCells["PulseWidth"].Value = pulseWidth;
                    rowCells["DutyCycle"].Value = dutyCycle;

                    if (countType.Equals("Average", StringComparison.OrdinalIgnoreCase))
                    {
                        dataGridView_Log_LaserPower.Rows[row].DefaultCellStyle.Font =
                            new Font(dataGridView_Log_LaserPower.Font, FontStyle.Bold);
                        dataGridView_Log_LaserPower.Rows[row].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                    }
                }
            }

            Log.Write("LaserPowerMeasure", "파워 측정 로그 조회 완료");
        }

        // 기존 코드
        //private void SearchClick_LaserPower(DateTime startTime, DateTime endTime)
        //{
        //    dataGridView_Log_LaserPower.Rows.Clear();

        //    string logFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LaserPowerLog");

        //    for (var day = startTime.Date; day <= endTime.Date; day = day.AddDays(1))
        //    {
        //        string logFile = Path.Combine(logFolder, $"LaserPowerMeasureLog_{day:yyyyMMdd}.csv");
        //        if (!File.Exists(logFile))
        //            continue;

        //        string[] lines = File.ReadAllLines(logFile, new UTF8Encoding(true));

        //        foreach (var line in lines)
        //        {
        //            var parts = line.Split(',');
        //            if (parts.Length < 4)
        //                continue;

        //            string time = parts[0];
        //            string target = parts[1];
        //            string countType = parts[2]; // Count_1, Count_2, ..., Average
        //            string value = parts[3];

        //            int row = dataGridView_Log_LaserPower.Rows.Add();
        //            var rowCells = dataGridView_Log_LaserPower.Rows[row].Cells;

        //            rowCells["Time"].Value = time;
        //            rowCells["Target"].Value = target;
        //            rowCells["Type"].Value = countType;
        //            rowCells["Value"].Value = value;

        //            // 평균값 강조 표시
        //            if (countType.Equals("Average", StringComparison.OrdinalIgnoreCase))
        //            {
        //                dataGridView_Log_LaserPower.Rows[row].DefaultCellStyle.Font =
        //                    new Font(dataGridView_Log_LaserPower.Font, FontStyle.Bold);
        //                dataGridView_Log_LaserPower.Rows[row].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
        //            }
        //        }
        //    }

        //    Log.Write("LaserPowerMeasure", "파워 측정 로그 조회 완료");
        //}


        private void baseButton_Log_LaserPower_Search_Click(object sender, EventArgs e)
        {
            DateTime startTime = dateTimePicker_Log_LaserPower_StartTime.Value;
            DateTime endTime = dateTimePicker_Log_LaserPower_EndTime.Value;

            SearchClick_LaserPower(startTime, endTime);
        }
    }
}
