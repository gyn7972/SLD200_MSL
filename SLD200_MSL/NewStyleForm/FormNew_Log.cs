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

            InitGrid();
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

        private void AddGridColumn(string name, string headerText)
        {
            var column = new DataGridViewTextBoxColumn
            {
                Name = name,
                HeaderText = headerText,
                DataPropertyName = name
            };
            dataGridView_Log.Columns.Add(column);
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

    }
}
