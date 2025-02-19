using SLD200_MSL;
using QMC.Common;
using QMC.Common.Parts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SLD200_MSL
{
    #region Define
    public enum OffetGridColumns
    {
        X,
        Y,
        OffsetX,
        OffsetY,
    }
    public delegate void OffsetControlButtonClickEventHandler(string strName);
    #endregion

    public partial class ScannerCompensatorOffsetControl : UserControl
    {
        #region Define
        public event OffsetControlButtonClickEventHandler ButtonClick;
        #endregion

        #region Property
        public ScannerCompensatorConfig Config { get; set; }
        #endregion

        public ScannerCompensatorOffsetControl(ScannerCompensatorConfig config)
        {
            Config = config;
            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();

            UpdateDataGridColumns();
            UpdateGridData();
        }


        #region Method
        private void UpdateDataGridColumns()
        {
            OffsetGrid.Columns.Clear();
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.Name = "X";
                column.HeaderText = "X";
                column.DataPropertyName = "Postion";
                column.ReadOnly = true;
                OffsetGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.Name = "Y";
                column.HeaderText = "Y";
                column.DataPropertyName = "Postion.Y";
                column.ReadOnly = true;
                OffsetGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.Name = "OffsetX";
                column.HeaderText = "OffsetX";
                column.DataPropertyName = "Offset";
                OffsetGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.Name = "OffsetY";
                column.HeaderText = "OffsetY";
                column.DataPropertyName = "Offset.Y";
                OffsetGrid.Columns.Add(column);
            }
            //foreach (OffetGridColumns one in Enum.GetValues(typeof(OffetGridColumns)))
            //{
            //    switch (one)
            //    {
            //        case OffetGridColumns.X:
            //        case OffetGridColumns.Y:
            //        case OffetGridColumns.OffsetX:
            //        case OffetGridColumns.OffsetY:
            //            {
            //                OffsetGrid.Columns.Add(one.ToString(), one.ToString());
            //            }
            //            break;


            //    }
            //}
        }

        public void UpdateGridData()
        {
            this.OffsetGrid.Rows.Clear();

            if (Config != null)
            {
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
                    QMC.Common.Parts.PositionOffset positionoffset = Config.XyGridSearchResults[e.RowIndex];
                    switch ((OffetGridColumns)e.ColumnIndex)
                    {
                        case OffetGridColumns.X:
                            Config.XyGridSearchResults[e.RowIndex] =
                                new QMC.Common.Parts.PositionOffset(new XyCoordinate(dValue, positionoffset.Position.Y), new XyCoordinate(positionoffset.Offset.X, positionoffset.Offset.Y));
                            break;
                        case OffetGridColumns.Y:
                            Config.XyGridSearchResults[e.RowIndex] =
                                new QMC.Common.Parts.PositionOffset(new XyCoordinate(positionoffset.Position.X, dValue), new XyCoordinate(positionoffset.Offset.X, positionoffset.Offset.Y));
                            break;
                        case OffetGridColumns.OffsetX:
                            Config.XyGridSearchResults[e.RowIndex] =
                                new QMC.Common.Parts.PositionOffset(new XyCoordinate(positionoffset.Position.X, positionoffset.Position.Y), new XyCoordinate(dValue, positionoffset.Offset.Y));
                            break;
                        case OffetGridColumns.OffsetY:
                            Config.XyGridSearchResults[e.RowIndex] =
                                new QMC.Common.Parts.PositionOffset(new XyCoordinate(positionoffset.Position.X, positionoffset.Position.Y), new XyCoordinate(positionoffset.Offset.X, dValue));
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

        private void baseButtonSave_Click(object sender, EventArgs e)
        {
            BaseButton button = sender as BaseButton;
            if (ButtonClick != null)
            {
                ButtonClick(button.Text);
            }
        }

        private void baseButtonLoad_Click(object sender, EventArgs e)
        {
            BaseButton button = sender as BaseButton;
            if (ButtonClick != null)
            {
                ButtonClick(button.Text);
            }
        }
        #endregion
    }
}
