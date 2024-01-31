using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using QMC.Common;
using QMC.Common.Motion.ACS.Motions;

namespace CWA150SA_Onsemi300
{
    #region FormMotionBoardConfiguration

    public partial class FormMotionBoardConfiguration : FormSubContentBase
    {
        #region Define
        public enum MotionBoardConfigurationColumnName
        {
            Name,
            Description,
            BoardType,
            No,
            //AxisNo,
            AxisCount,
            Simulated
        }
        public enum Components
        {
            AjinAxis,
            AcsAxis
        }
        #endregion

        #region Field
        private MotionBoardConfigurationColletion m_MotionBoardConfigurationColletion;
        public MotionBoardConfiguration m_MotionBoardConfigration;
        DataGridViewComboBoxColumn ComboBoxColumn = new DataGridViewComboBoxColumn();
        #endregion

        #region Constructor
        public FormMotionBoardConfiguration()
            : base(FormType.Content.ToString(), "Motion Board Configuration")
        {
            InitializeComponent();
            base.flowLayoutPanelButton.Controls.Add(this.buttonSave);
            base.flowLayoutPanelButton.Controls.Add(this.buttonLoad);
            base.flowLayoutPanelButton.Controls.Add(this.buttonDelete);
            base.flowLayoutPanelButton.Controls.Add(this.buttonNew);
            this.flowLayoutPanelButton.Size = Configuration.PanelbuttonSize;

            base.panelContent.Controls.Add(this.dataGridViewMotionBoard);
            base.panelContent.Controls.Add(this.propertyGridMotionBoard);

            this.panelContent.Location = new Point(Configuration.ContentLocation.X + 10, this.Configuration.PanelSize.Height * 2);
            this.panelContent.Size = new Size(Configuration.MainSize.Width - this.panelContent.Location.X, Configuration.FormContentSize.Height - Configuration.PanelSize.Height * 2);
            this.flowLayoutPanelButton.FlowDirection = FlowDirection.RightToLeft;

            this.buttonDelete.Size = new System.Drawing.Size(Configuration.ButtonSize.Width, Configuration.ButtonSize.Height);
            this.buttonLoad.Size = new System.Drawing.Size(Configuration.ButtonSize.Width, Configuration.ButtonSize.Height);
            this.buttonSave.Size = new System.Drawing.Size(Configuration.ButtonSize.Width, Configuration.ButtonSize.Height);
            this.buttonNew.Size = new System.Drawing.Size(Configuration.ButtonSize.Width, Configuration.ButtonSize.Height);

            this.propertyGridMotionBoard.Location = new System.Drawing.Point(Configuration.ContentSize.Width / 2, 0);
            this.propertyGridMotionBoard.Size = new Size(Configuration.ContentSize.Width / 2 - 30, Configuration.ContentSize.Height - Configuration.PanelSize.Height * 4 - this.flowLayoutPanelButton.Height);

            this.dataGridViewMotionBoard.Location = new System.Drawing.Point(0, this.propertyGridMotionBoard.Location.Y);
            this.dataGridViewMotionBoard.Size = new System.Drawing.Size(Configuration.ContentSize.Width / 2 - Configuration.ContentLocation.X, this.propertyGridMotionBoard.Height);

            //    this.flowLayoutPanelButton.Location = new System.Drawing.Point(0, 0);
            //    this.flowLayoutPanelButton.Size = new System.Drawing.Size(Configuration.ContentSize.Width, Configuration.PanelSize.Height);


            DataGridViewColumnCreate();
            LoadBoardCollection();

        }
        #endregion

        #region Property
        public MotionBoardConfigurationColletion MotionBoardConfigurationColletion
        {
            get { return this.m_MotionBoardConfigurationColletion; }
            set { this.m_MotionBoardConfigurationColletion = value; }
        }
        #endregion

        #region Method
        public void DataGridViewColumnCreate()
        {
            foreach (MotionBoardConfigurationColumnName item in Enum.GetValues(typeof(MotionBoardConfigurationColumnName)))
            {
                if (item == MotionBoardConfigurationColumnName.BoardType)
                {
                    DataGridViewComboBoxColumn ComboBoxColumn = new DataGridViewComboBoxColumn();
                    ComboBoxColumn.HeaderText = item.ToString();
                    ComboBoxColumn.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                    ComboBoxColumn.DefaultCellStyle.BackColor = Color.FromArgb(200, 200, 200);
                    ComboBoxColumn.FlatStyle = FlatStyle.Flat;
                    ComboBoxColumn.Name = item.ToString();
                    foreach (MotionBoardType MotionBoardType in Enum.GetValues(typeof(MotionBoardType)))
                    {
                        ComboBoxColumn.Items.Add(MotionBoardType.ToString());
                    }

                    dataGridViewMotionBoard.Columns.Add(ComboBoxColumn);
                }
                else if (item == MotionBoardConfigurationColumnName.Simulated)
                {
                    DataGridViewCheckBoxColumn CheckBoxColumn = new DataGridViewCheckBoxColumn();
                    CheckBoxColumn.HeaderText = item.ToString();
                    CheckBoxColumn.Name = item.ToString();
                    dataGridViewMotionBoard.Columns.Add(CheckBoxColumn);
                }
                else
                {
                    dataGridViewMotionBoard.Columns.Add(item.ToString(), item.ToString());
                }
            }
        }

        private void UpdateGridView(int nRow)
        {
            UpdateGridView(nRow, this.MotionBoardConfigurationColletion[nRow]);
        }

        protected void UpdateGridView(int nRow, MotionBoardConfiguration MotionBoardConfigration)
        {
            this.dataGridViewMotionBoard[(int)MotionBoardConfigurationColumnName.Name, nRow].Value = MotionBoardConfigration.Name;
            this.dataGridViewMotionBoard[(int)MotionBoardConfigurationColumnName.Description, nRow].Value = MotionBoardConfigration.Description;
            this.dataGridViewMotionBoard[(int)MotionBoardConfigurationColumnName.No, nRow].Value = MotionBoardConfigration.No.ToString();
            //this.dataGridViewMotionBoard[(int)MotionBoardConfigurationColumnName.AxisNo, nRow].Value = MotionBoardConfigration.AxisNo.ToString();
            this.dataGridViewMotionBoard[(int)MotionBoardConfigurationColumnName.AxisCount, nRow].Value = MotionBoardConfigration.AxisCount.ToString();
            this.dataGridViewMotionBoard[(int)MotionBoardConfigurationColumnName.Simulated, nRow].Value = MotionBoardConfigration.Simulated;
            this.dataGridViewMotionBoard[(int)MotionBoardConfigurationColumnName.BoardType, nRow].Value = MotionBoardConfigration.BoardType.ToString();
        }
        #endregion

        #region Event Handler

        #region NewRow
        private void buttonNew_Click(object sender, EventArgs e)
        {
            int nRow = dataGridViewMotionBoard.Rows.Add();
            MotionBoardConfiguration MotionBoardConfigration = new MotionBoardConfiguration();
            //  MotionBoardConfigration.GetDeepCopy();
            this.MotionBoardConfigurationColletion.Add(MotionBoardConfigration);
            UpdateGridView(nRow, MotionBoardConfigration);
        }

        #endregion

        #region Delete
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewMotionBoard.Rows.Count > 0)
            {
                int nRow = dataGridViewMotionBoard.SelectedCells[0].RowIndex;
                if (nRow >= 0 && nRow < MotionBoardConfigurationColletion.Count)
                {
                    if (MotionBoardConfigurationColletion.Count == 0 || dataGridViewMotionBoard.Rows.Count == 0)
                    {
                        MotionBoardConfiguration MotionBoardConfigration = new MotionBoardConfiguration();
                        propertyGridMotionBoard.SelectedObject = MotionBoardConfigration;
                        return;
                    }

                    MotionBoardConfigurationColletion.RemoveAt(nRow);
                    dataGridViewMotionBoard.Rows.Remove(dataGridViewMotionBoard.Rows[nRow]);
                }
            }

        }

        #endregion

        #region Save
        private void buttonSave_Click(object sender, EventArgs e)
        {
            Equipment.SaveMotionBoard(MotionBoardConfigurationColletion);
        }
        #endregion
        private void LoadBoardCollection()
        {
            dataGridViewMotionBoard.Rows.Clear();
            //Equipment.LoadMotionBoards();
            MotionBoardConfigurationColletion = Equipment.GetMotionConfigurationList();
            foreach (MotionBoardConfiguration config in MotionBoardConfigurationColletion)
            {
                int nRow = dataGridViewMotionBoard.Rows.Add();
                UpdateGridView(nRow, config);
            }
        }
        #region Load
        private void buttonLoad_Click(object sender, EventArgs e)
        {
            LoadBoardCollection();
        }

        #endregion

        #region CellChanges

        #region CurrentCellChanged
        private void dataGridViewMotionBoard_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dataGridViewMotionBoard.SelectedCells.Count > 0)
            {
                int RowIndex = 0;
                RowIndex = dataGridViewMotionBoard.SelectedCells[0].RowIndex;
                if (MotionBoardConfigurationColletion.Count <= RowIndex)
                {
                    return;
                }
                if (RowIndex >= 0)
                {
                    m_MotionBoardConfigration = MotionBoardConfigurationColletion[RowIndex];
                    propertyGridMotionBoard.SelectedObject = m_MotionBoardConfigration;
                }
            }
        }

        #endregion

        #region CellValueChanged
        private void dataGridViewMotionBoard_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (MotionBoardConfigurationColletion.Count > 0)
            {

                object ChangeData = dataGridViewMotionBoard[e.ColumnIndex, e.RowIndex].Value;

                switch ((MotionBoardConfigurationColumnName)e.ColumnIndex)
                {
                    case MotionBoardConfigurationColumnName.AxisCount:
                        MotionBoardConfigurationColletion[e.RowIndex].AxisCount = int.Parse((string)ChangeData);
                        break;
                    case MotionBoardConfigurationColumnName.BoardType:
                        MotionBoardConfigurationColletion[e.RowIndex].BoardType = (MotionBoardType)Enum.Parse(typeof(MotionBoardType), (string)ChangeData);
                        break;
                    case MotionBoardConfigurationColumnName.Description:
                        MotionBoardConfigurationColletion[e.RowIndex].Description = (string)ChangeData;
                        break;
                    case MotionBoardConfigurationColumnName.Name:
                        MotionBoardConfigurationColletion[e.RowIndex].Name = (string)ChangeData;
                        break;
                    case MotionBoardConfigurationColumnName.No:
                        MotionBoardConfigurationColletion[e.RowIndex].No = int.Parse((string)ChangeData);
                        break;
                    //case MotionBoardConfigurationColumnName.AxisNo:
                    //    MotionBoardConfigurationColletion[e.RowIndex].AxisNo = int.Parse((string)ChangeData);
                    //    break;
                    case MotionBoardConfigurationColumnName.Simulated:
                        MotionBoardConfigurationColletion[e.RowIndex].Simulated = Convert.ToBoolean(ChangeData);
                        break;
                    default:
                        break;
                }
                MotionBoardConfiguration motion = MotionBoardConfigurationColletion[e.RowIndex];
                propertyGridMotionBoard.SelectedObject = motion;
            }
        }

        #endregion

        #endregion

        #region PropertyValueChanged
        private void propertyGridMotionBoard_PropertyValueChanged_1(object sender, PropertyValueChangedEventArgs e)
        {
            string strtitle = e.ChangedItem.Label;
            if (dataGridViewMotionBoard.RowCount == 0)
            {
                MotionBoardConfigurationColletion = new MotionBoardConfigurationColletion();
                return;
            }
            //if(e.ChangedItem.Label == dataGridViewMotionBoard[)
            PropertyGrid propertyGrid = sender as PropertyGrid;
            if (propertyGrid != null)
            {
                MotionBoardConfiguration configuration = propertyGrid.SelectedObject as MotionBoardConfiguration;
                int nRow = dataGridViewMotionBoard.SelectedCells[0].RowIndex;
                UpdateGridView(nRow);
            }
        }

        #endregion
        #endregion
    }
    #endregion
}
