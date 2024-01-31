using QMC.Common;
using QMC.Common.VisionPart;
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

namespace CWA150SA_Onsemi300
{
    #region Define
    public delegate void OffsetConfigControlButtonClickEventHandler(string strName);
    #endregion

    #region VisionCompensatorOffsetConfigControl
    public partial class VisionCompensatorOffsetConfigControl : UserControl
    {
        #region Define
        public event OffsetControlButtonClickEventHandler ConfigButtonClick;
        #endregion

        #region Property
        public VisionCompensatorConfig Config { get; set; }
        #endregion

        #region Constructor
        public VisionCompensatorOffsetConfigControl(VisionCompensatorConfig config)
        {
            Config = config;
            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();

            UpdateDataGridColumns();
            UpdateGridData();
        }
        #endregion

        #region Method
        private void UpdateDataGridColumns()
        {
            OffsetGrid.Columns.Clear();
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.Name = "X";
                column.HeaderText = "X";
                //column.DataPropertyName = "X";
                column.ReadOnly = true;
                OffsetGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.Name = "Y";
                column.HeaderText = "Y";
                //column.DataPropertyName = "Y";
                column.ReadOnly = true;
                OffsetGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.Name = "OffsetX";
                column.HeaderText = "OffsetX";
                //column.DataPropertyName = "X";
                OffsetGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.Name = "OffsetY";
                column.HeaderText = "OffsetY";
                //column.DataPropertyName = "Y";
                OffsetGrid.Columns.Add(column);
            }
        }

        private void UpdateGridData()
        {
            this.OffsetGrid.Rows.Clear();

            if (Config == null)
            {
                return;
            }

            if (Config.XyGridSearchResults.Count > 0)
            {
                for (int i = 0; i < Config.XyGridSearchResults.Count; i++)
                {
                    this.OffsetGrid.Rows.Add();
                    OffsetGrid[0, i].Value = Config.XyGridSearchResults[i].Position.X;
                    OffsetGrid[1, i].Value = Config.XyGridSearchResults[i].Position.Y;
                    OffsetGrid[2, i].Value = Config.XyGridSearchResults[i].Offset.X;
                    OffsetGrid[3, i].Value = Config.XyGridSearchResults[i].Offset.Y;
                }
            }
        }
        #endregion

        #region Event Handler
        private void DataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (Config.XyGridSearchResults.Count > 0 && Config.XyGridSearchResults.Count > e.RowIndex)
            {
                if (OffsetGrid[e.ColumnIndex, e.RowIndex].Value != null)
                {
                    string ColumnData = OffsetGrid[e.ColumnIndex, e.RowIndex].Value.ToString();
                    double dValue = 0;
                    double.TryParse(ColumnData, out dValue);
                    QMC.Common.VisionPart.PositionOffset positionoffset = Config.XyGridSearchResults[e.RowIndex];
                    switch ((OffetGridColumns)e.ColumnIndex)
                    {
                        case OffetGridColumns.X:
                            Config.XyGridSearchResults[e.RowIndex] =
                                new QMC.Common.VisionPart.PositionOffset(new XyCoordinate(dValue, positionoffset.Position.Y), new XyCoordinate(positionoffset.Offset.X, positionoffset.Offset.Y));
                            break;
                        case OffetGridColumns.Y:
                            Config.XyGridSearchResults[e.RowIndex] =
                                new QMC.Common.VisionPart.PositionOffset(new XyCoordinate(positionoffset.Position.X, dValue), new XyCoordinate(positionoffset.Offset.X, positionoffset.Offset.Y));
                            break;
                        case OffetGridColumns.OffsetX:
                            Config.XyGridSearchResults[e.RowIndex] =
                                new QMC.Common.VisionPart.PositionOffset(new XyCoordinate(positionoffset.Position.X, positionoffset.Position.Y), new XyCoordinate(dValue, positionoffset.Offset.Y));
                            break;
                        case OffetGridColumns.OffsetY:
                            Config.XyGridSearchResults[e.RowIndex] =
                                new QMC.Common.VisionPart.PositionOffset(new XyCoordinate(positionoffset.Position.X, positionoffset.Position.Y), new XyCoordinate(positionoffset.Offset.X, dValue));
                            break;

                        default:
                            break;
                    }
                }
            }
            else if (OffsetGrid[e.ColumnIndex, e.RowIndex].Value == null)
            {

            }
        }
        private void buttonLoad_Click(object sender, EventArgs e)
        {
            StringBuilder builder = null;

            if (openFileDialog.ShowDialog() != DialogResult.OK) return;
            if (File.Exists(openFileDialog.FileName) == false)
            {
                return;
            }

            builder = this.FileLoad(openFileDialog.FileName);
            this.GridLoad(this.OffsetGrid, builder);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            StringBuilder builder = null;
            BaseDataGridView grid = null;
            string path = string.Empty;

            if (saveFileDialog.ShowDialog() != DialogResult.OK) return;

            builder = this.GridSave(this.OffsetGrid);

            path = Path.Combine(Path.GetDirectoryName(saveFileDialog.FileName), Path.GetFileNameWithoutExtension(saveFileDialog.FileName) + ".csv");
            this.FileSave(path, builder);
        }
        #endregion

        #region Method
        private void GridLoad(BaseDataGridView grid, StringBuilder builder)
        {
            string[] line = builder.ToString().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            double positionX = 0D, positionY = 0D, offsetX = 0D, offsetY = 0D;
            List<DataGridViewRow> rows = new List<DataGridViewRow>();
            UpdateDataGridColumns();
            for (int i = 0; i < line.Length; i++)
            {
                DataGridViewRow row = null;
                string[] token = line[i].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                if (token.Length != grid.Columns.Count) return;

                if (double.TryParse(token[0], out positionX) == false) continue;
                if (double.TryParse(token[1], out positionY) == false) continue;
                if (double.TryParse(token[2], out offsetX) == false) continue;
                if (double.TryParse(token[3], out offsetY) == false) continue;
                row = grid.GetNewRow();
                row.SetValues(positionX, positionY, offsetX, offsetY);
                Config.XyGridSearchResults.Clear();
                Config.XyGridSearchResults.Add(new QMC.Common.VisionPart.PositionOffset(new XyCoordinate(positionX, positionY), new XyCoordinate(offsetX, offsetY)));
                rows.Add(row);
            }
            grid.Rows.Clear();
            grid.Rows.AddRange(rows.ToArray());
        }


        private StringBuilder GridSave(BaseDataGridView grid)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"{grid.Columns[0].HeaderText},{grid.Columns[1].HeaderText},{grid.Columns[2].HeaderText},{grid.Columns[3].HeaderText}");
            for (int i = 0; i < grid.Rows.Count; i++)
                builder.AppendLine($"{grid.Rows[i].Cells[0].Value.ToString()},{grid.Rows[i].Cells[1].Value.ToString()},{grid.Rows[i].Cells[2].Value.ToString()},{grid.Rows[i].Cells[3].Value.ToString()}");

            return builder;
        }
        private void FileSave(string path, StringBuilder builder)
        {
            try
            {
                File.WriteAllText(path, builder.ToString());
            }
            catch (Exception ex)
            {

            }
        }
        private StringBuilder FileLoad(string path)
        {
            StringBuilder builder = new StringBuilder();

            try
            {
                builder = new StringBuilder(File.ReadAllText(openFileDialog.FileName));
            }
            catch (Exception ex)
            {

            }

            return builder;
        }
        #endregion
    }
    #endregion
}
