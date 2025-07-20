using QMC.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace SLD200_MSL
{
    public partial class FormNew_AlarmLog : Form
    {
        public FormNew_AlarmLog()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            //m_AlarmLogOptionControl = new AlarmLogOptionControl();
            m_AlarmLogOptionControl.SearchClick += SearchClick;
            m_AlarmLogOptionControl.DataSaveClick += SaveDataClick;

            m_DataGridViewAlarmLog.AllowUserToResizeRows = false;
            m_DataGridViewAlarmLog.RowHeadersVisible = false;
            m_DataGridViewAlarmLog.ReadOnly = true;
            m_DataGridViewAlarmLog.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            m_DataGridViewAlarmLog.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            m_DataGridViewAlarmLog.AutoGenerateColumns = false;

            InitGrid();
            m_DataGridViewAlarmLog.CellClick += baseDataGridViewAlarm_CellClick;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // 엔터 또는 스페이스 키 눌렀을 때 무시
            if (keyData == Keys.Enter || keyData == Keys.Space)
                return true;

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void InitGrid()
        {
            m_DataGridViewAlarmLog.Columns.Clear(); // 중복 방지

            m_DataGridViewAlarmLog.AutoGenerateColumns = false;
            m_DataGridViewAlarmLog.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            m_DataGridViewAlarmLog.ReadOnly = true;

            AddGridColumn("Date", "Date");
            AddGridColumn("Title", "Title");
            AddGridColumn("Grade", "Grade");
            AddGridColumn("Source", "Source");
            AddGridColumn("Cause", "Cause");
            AddGridColumn("Code", "Code");
        }

        private void AddGridColumn(string name, string headerText)
        {
            var column = new DataGridViewTextBoxColumn
            {
                Name = name,
                HeaderText = headerText,
                DataPropertyName = name
            };
            m_DataGridViewAlarmLog.Columns.Add(column);
        }

        private void SearchClick(DateTime startTime, DateTime endTime)
        {
            m_DataGridViewAlarmLog.Rows.Clear();

            string logFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AlarmLog");

            for (var day = startTime.Date; day <= endTime.Date; day = day.AddDays(1))
            {
                string logFile = Path.Combine(logFolder, $"AlarmLog_{day:yyyyMMdd}.csv");
                if (!File.Exists(logFile)) continue;

                try
                {
                    using (var fs = new FileStream(logFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (var reader = new StreamReader(fs, new UTF8Encoding(true)))
                    {
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            var parts = line.Split(',');
                            if (parts.Length < 6) continue;

                            try
                            {
                                int rowIndex = m_DataGridViewAlarmLog.Rows.Add();
                                m_DataGridViewAlarmLog.Rows[rowIndex].SetValues(parts);
                            }
                            catch (Exception exRow)
                            {
                                Log.Write("AlarmLogViewer", $"Row 추가 실패: {exRow.Message}");
                            }
                        }
                    }
                }
                catch (Exception exFile)
                {
                    Log.Write("AlarmLogViewer", $"파일 열기 실패: {exFile.Message}");
                }
            }
        }

        private void SaveDataClick()
        {
            if (m_DataGridViewAlarmLog.Rows.Count == 0) return;

            using (var dialog = new SaveFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                DefaultExt = "csv",
                Filter = "CSV files (*.csv)|*.csv"
            })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter sw = new StreamWriter(dialog.FileName, false, new UTF8Encoding(true)))
                    {
                        sw.WriteLine("Date,Title,Grade,Source,Cause,Code");
                        foreach (DataGridViewRow row in m_DataGridViewAlarmLog.Rows)
                        {
                            if (row.IsNewRow) continue;

                            string[] values = new string[6];
                            for (int i = 0; i < 6; i++)
                                values[i] = row.Cells[i].Value?.ToString() ?? "";

                            sw.WriteLine(string.Join(",", values));
                        }
                    }
                }
            }
        }

        private void baseDataGridViewAlarm_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= m_DataGridViewAlarmLog.Rows.Count) return;

            var row = m_DataGridViewAlarmLog.Rows[e.RowIndex];
            string strTitle = row.Cells["Title"].Value?.ToString() ?? "";
            string strCause = row.Cells["Cause"].Value?.ToString() ?? "";
            string strCode = row.Cells["Code"].Value?.ToString() ?? "";
            string strGrade = row.Cells["Grade"].Value?.ToString() ?? "";
            string strSource = row.Cells["Source"].Value?.ToString() ?? "";

            m_AlarmInfoControl.SetInfo(strTitle, strCause, strCode, strGrade, strSource);
        }
    }
}
