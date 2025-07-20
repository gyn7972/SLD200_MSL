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

            dataGridView_Log_AutoCross.AllowUserToResizeRows = false;
            dataGridView_Log_AutoCross.RowHeadersVisible = false;
            dataGridView_Log_AutoCross.ReadOnly = true;
            dataGridView_Log_AutoCross.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView_Log_AutoCross.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView_Log_AutoCross.AutoGenerateColumns = false;

            dataGridView_Log_Height.AllowUserToResizeRows = false;
            dataGridView_Log_Height.RowHeadersVisible = false;
            dataGridView_Log_Height.ReadOnly = true;
            dataGridView_Log_Height.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView_Log_Height.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView_Log_Height.AutoGenerateColumns = false;


            InitGrid();
            InitGrid_LaserPower();
            InitGrid_AutoCross();
            InitGrid_HeightMeasure();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // 엔터 또는 스페이스 키 눌렀을 때 무시
            if (keyData == Keys.Enter || keyData == Keys.Space)
                return true;

            return base.ProcessCmdKey(ref msg, keyData);
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
            AddGridColumn("NG_Count", "NG_Count");
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

            if(Equipment.Machine_LaserType_CO2)
            {
                AddGridColumn_LaserPower_Fill("Time", "Time", 100);
                AddGridColumn_LaserPower_Fill("Target", "Target", 100);
                AddGridColumn_LaserPower_Fill("Type", "Type", 80);
                AddGridColumn_LaserPower_Fill("Frequency", "Freq(kHz)", 90);
                AddGridColumn_LaserPower_Fill("PulseWidth", "PulseWidth(µs)", 100);
                AddGridColumn_LaserPower_Fill("DutyCycle", "DutyCycle(%)", 100);
                AddGridColumn_LaserPower_Fill("Value", "Value(W)", 80);
            }
            else
            {
                AddGridColumn_LaserPower_Fill("Time", "Time", 100);
                AddGridColumn_LaserPower_Fill("Target", "Target", 100);
                AddGridColumn_LaserPower_Fill("Type", "Type", 80);
                AddGridColumn_LaserPower_Fill("Frequency", "Freq(kHz)", 90);
                AddGridColumn_LaserPower_Fill("PulseWidth", "PulseWidth(µs)", 100);
                AddGridColumn_LaserPower_Fill("PowerPercent", "Power(%)", 90);
                AddGridColumn_LaserPower_Fill("Value", "Value(W)", 80);
            }
        }

        private void InitGrid_AutoCross()
        {
            dataGridView_Log_AutoCross.Columns.Clear(); // 중복 방지

            dataGridView_Log_AutoCross.AutoGenerateColumns = false;
            dataGridView_Log_AutoCross.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView_Log_AutoCross.ReadOnly = true;

            // 셀 크기 자동 조정
            dataGridView_Log_AutoCross.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None; // 중요
            dataGridView_Log_AutoCross.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView_Log_AutoCross.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            AddGridColumn_AutoCross_Fill("Time", "Time", 100);
            AddGridColumn_AutoCross_Fill("Result", "Result", 80);
            AddGridColumn_AutoCross_Fill("BeforeX", "BeforeX", 100);
            AddGridColumn_AutoCross_Fill("BeforeY", "BeforeY", 100);
            AddGridColumn_AutoCross_Fill("OffsetX", "OffsetX", 100);
            AddGridColumn_AutoCross_Fill("OffsetY", "OffsetY", 100);
            AddGridColumn_AutoCross_Fill("AfterX", "AfterX", 100);
            AddGridColumn_AutoCross_Fill("AfterY", "AfterY", 100);

        }

        private void InitGrid_HeightMeasure()
        {
            dataGridView_Log_Height.Columns.Clear(); // 중복 방지

            dataGridView_Log_Height.AutoGenerateColumns = false;
            dataGridView_Log_Height.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView_Log_Height.ReadOnly = true;

            // 셀 크기 자동 조정
            dataGridView_Log_Height.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dataGridView_Log_Height.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView_Log_Height.DefaultCellStyle.WrapMode = DataGridViewTriState.False;

            AddGridColumn_HeightMeasure_Fill("Time", "Time", 150);
            AddGridColumn_HeightMeasure_Fill("Target", "Target", 100);
            AddGridColumn_HeightMeasure_Fill("StagePosZ", "StagePosZ", 100);
            AddGridColumn_HeightMeasure_Fill("HeightOffset", "HeightOffset", 100);
            AddGridColumn_HeightMeasure_Fill("ModulePosZ", "ModulePosZ", 100);
            AddGridColumn_HeightMeasure_Fill("ModuleHeight", "ModuleHeight", 100);
        }


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

        private void AddGridColumn_AutoCross_Fill(string name, string header, int minWidth)
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
            dataGridView_Log_AutoCross.Columns.Add(col);
        }

        private void AddGridColumn_HeightMeasure_Fill(string name, string header, int minWidth)
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
            dataGridView_Log_Height.Columns.Add(col);
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

                using (var fs = new FileStream(logFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = new StreamReader(fs, new UTF8Encoding(true)))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var parts = line.Split(',');
                        if (parts.Length < 6) continue;

                        int rowIndex = dataGridView_Log.Rows.Add();
                        dataGridView_Log.Rows[rowIndex].Cells["StartTime"].Value = parts[0];
                        dataGridView_Log.Rows[rowIndex].Cells["EndTime"].Value = parts[1];
                        dataGridView_Log.Rows[rowIndex].Cells["RecipeName"].Value = parts[2];
                        dataGridView_Log.Rows[rowIndex].Cells["DrawingName"].Value = parts[3];
                        dataGridView_Log.Rows[rowIndex].Cells["CompletedCount"].Value = parts[4];
                        dataGridView_Log.Rows[rowIndex].Cells["NG_Count"].Value = parts[5];
                    }
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
                if (!File.Exists(logFile)) continue;

                using (var fs = new FileStream(logFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = new StreamReader(fs, new UTF8Encoding(true)))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (line.StartsWith("Timestamp")) continue;

                        var parts = line.Split(',');
                        if (parts.Length < 4) continue;

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

                        if(Equipment.Machine_LaserType_CO2)
                        {
                            rowCells["Time"].Value = time;
                            rowCells["Target"].Value = target;
                            rowCells["Type"].Value = countType;
                            rowCells["Value"].Value = value;
                            rowCells["Frequency"].Value = frequency;
                            rowCells["PulseWidth"].Value = pulseWidth;
                            rowCells["DutyCycle"].Value = dutyCycle;
                        }
                        else
                        {
                            rowCells["Time"].Value = time;
                            rowCells["Target"].Value = target;
                            rowCells["Type"].Value = countType;
                            rowCells["Value"].Value = value;
                            rowCells["PowerPercent"].Value = powerPercent;
                            rowCells["Frequency"].Value = frequency;
                            rowCells["PulseWidth"].Value = pulseWidth;
                        }
                            

                        if (countType.Equals("Average", StringComparison.OrdinalIgnoreCase))
                        {
                            dataGridView_Log_LaserPower.Rows[row].DefaultCellStyle.Font =
                                new Font(dataGridView_Log_LaserPower.Font, FontStyle.Bold);
                            dataGridView_Log_LaserPower.Rows[row].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                        }

                    }
                }
            }

            Log.Write("LaserPowerMeasure", "파워 측정 로그 조회 완료");
        }

        private void SearchClick_AutoCross(DateTime startTime, DateTime endTime)
        {
            dataGridView_Log_AutoCross.Rows.Clear();

            string logFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ScannerCameraOffsetLog");

            for (var day = startTime.Date; day <= endTime.Date; day = day.AddDays(1))
            {
                string logFile = Path.Combine(logFolder, $"ScannerCameraOffsetLog_{day:yyyyMMdd}.csv");
                if (!File.Exists(logFile)) continue;

                using (var fs = new FileStream(logFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = new StreamReader(fs, new UTF8Encoding(true)))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (line.StartsWith("Time")) continue;

                        var parts = line.Split(',');
                        if (parts.Length < 8) continue;

                        string time = parts[0];
                        string result = parts[1];
                        string beforeX = parts[2];
                        string beforeY = parts[3];
                        string offsetX = parts[4];
                        string offsetY = parts[5];
                        string afterX = parts[6];
                        string afterY = parts[7];

                        int row = dataGridView_Log_AutoCross.Rows.Add();
                        var rowCells = dataGridView_Log_AutoCross.Rows[row].Cells;

                        rowCells["Time"].Value = time;
                        rowCells["Result"].Value = result;
                        rowCells["BeforeX"].Value = beforeX;
                        rowCells["BeforeY"].Value = beforeY;
                        rowCells["OffsetX"].Value = offsetX;
                        rowCells["OffsetY"].Value = offsetY;
                        rowCells["AfterX"].Value = afterX;
                        rowCells["AfterY"].Value = afterY;

                        if (result.Equals("OK", StringComparison.OrdinalIgnoreCase))
                            dataGridView_Log_AutoCross.Rows[row].DefaultCellStyle.BackColor = Color.LightGreen;
                        else if (result.Equals("NG", StringComparison.OrdinalIgnoreCase))
                            dataGridView_Log_AutoCross.Rows[row].DefaultCellStyle.BackColor = Color.LightCoral;
                    }
                }
            }

            Log.Write("ScannerCameraOffset", "자동 교차 오프셋 보정 로그 조회 완료");
        }
        private void SearchClick_HeightMeasure(DateTime startTime, DateTime endTime)
        {
            dataGridView_Log_Height.Rows.Clear();

            string logFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HeightMeasureLog");

            for (var day = startTime.Date; day <= endTime.Date; day = day.AddDays(1))
            {
                string logFile = Path.Combine(logFolder, $"HeightMeasureLog_{day:yyyyMMdd}.csv");
                if (!File.Exists(logFile)) continue;

                using (var fs = new FileStream(logFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = new StreamReader(fs, new UTF8Encoding(true)))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (line.StartsWith("Timestamp")) continue;

                        var parts = line.Split(',');
                        if (parts.Length < 6) continue;

                        int row = dataGridView_Log_Height.Rows.Add();
                        var rowCells = dataGridView_Log_Height.Rows[row].Cells;

                        rowCells["Time"].Value = parts[0];
                        rowCells["Target"].Value = parts[1];
                        rowCells["StagePosZ"].Value = parts[2];
                        rowCells["HeightOffset"].Value = parts[3];
                        rowCells["ModulePosZ"].Value = parts[4];
                        rowCells["ModuleHeight"].Value = parts[5];
                    }
                }
            }

            Log.Write("SocketHeight", "Height 측정 로그 조회 완료");
        }

        private void baseButton_Log_LaserPower_Search_Click(object sender, EventArgs e)
        {
            DateTime startTime = dateTimePicker_Log_LaserPower_StartTime.Value;
            DateTime endTime = dateTimePicker_Log_LaserPower_EndTime.Value;

            SearchClick_LaserPower(startTime, endTime);
        }

        private void baseButton_Log_AutoCross_Search_Click(object sender, EventArgs e)
        {
            DateTime startTime = dateTimePicker_Log_AutoCross_StartTime.Value;
            DateTime endTime = dateTimePicker_Log_AutoCross_EndTime.Value;

            SearchClick_AutoCross(startTime, endTime);
        }

        private void baseButton_Log_Height_Search_Click(object sender, EventArgs e)
        {
            DateTime startTime = dateTimePicker_Log_Height_StartTime.Value;
            DateTime endTime = dateTimePicker_Log_Height_EndTime.Value;

            SearchClick_HeightMeasure(startTime, endTime);
        }
    }
}
