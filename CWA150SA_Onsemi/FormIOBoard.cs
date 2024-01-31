using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QMC.Common.Motion.Ajin.IO;
using System.IO;
using QMC.Common;

namespace CWA150SA_Onsemi300
{
    public partial class FormIOBoard : FormSubContentBase
    {
        #region Define
        public enum IOBoardColumnList
        {
            ComponentUid, No, Simulated
        }
        #endregion

        #region Constructor

        #region Field
        List<AjinIoAxlBoardConfiguration> m_listIOBoardConfiguration;
        private IOBoardConfigurationColletion m_IOBoadConfigurationCollection;
        public IOBoadConfiguration m_IOBoadConfiguration;


        #endregion

        #region Property
        public List<AjinIoAxlBoardConfiguration> ListIOBoardConfiguration
        {
            get { return m_listIOBoardConfiguration; }
            set { m_listIOBoardConfiguration = value; }
        }
        public IOBoardConfigurationColletion IOBoardConfigurationColletion
        {
            get { return this.m_IOBoadConfigurationCollection; }
            set { m_IOBoadConfigurationCollection = value; }
        }
        #endregion

        public FormIOBoard()
            : base(FormType.Content.ToString(), "IO Board")
        {
            InitializeComponent();

            #region Controls
            base.flowLayoutPanelButton.Controls.Add(this.buttonSave);
            base.flowLayoutPanelButton.Controls.Add(this.buttonLoad);
            base.flowLayoutPanelButton.Controls.Add(this.buttonDelete);
            base.flowLayoutPanelButton.Controls.Add(this.buttonNew);

            base.panelContent.Controls.Add(this.dataGridViewIOBoard);
            base.panelContent.Controls.Add(this.propertyGridIOBoard);

            this.panelContent.Location = new Point(Configuration.ContentLocation.X + 10, this.Configuration.PanelSize.Height * 2);
            this.panelContent.Size = new Size(Configuration.MainSize.Width - this.panelContent.Location.X, Configuration.FormContentSize.Height - Configuration.PanelSize.Height * 2);
            this.flowLayoutPanelButton.FlowDirection = FlowDirection.RightToLeft;

            this.buttonDelete.Size = new System.Drawing.Size(Configuration.ButtonSize.Width, Configuration.ButtonSize.Height);
            this.buttonLoad.Size = new System.Drawing.Size(Configuration.ButtonSize.Width, Configuration.ButtonSize.Height);
            this.buttonSave.Size = new System.Drawing.Size(Configuration.ButtonSize.Width, Configuration.ButtonSize.Height);
            this.buttonNew.Size = new System.Drawing.Size(Configuration.ButtonSize.Width, Configuration.ButtonSize.Height);

            this.propertyGridIOBoard.Location = new System.Drawing.Point(Configuration.ContentSize.Width / 2, 0);
            this.propertyGridIOBoard.Size = new Size(Configuration.ContentSize.Width / 2 - 30, Configuration.ContentSize.Height - Configuration.PanelSize.Height * 4 - this.flowLayoutPanelButton.Height);
            this.dataGridViewIOBoard.Location = new System.Drawing.Point(0, this.propertyGridIOBoard.Location.Y);
            this.dataGridViewIOBoard.Size = new System.Drawing.Size(Configuration.ContentSize.Width / 2 - Configuration.ContentLocation.X, this.propertyGridIOBoard.Height);
            #endregion

            CreatGridViewColumn();
            this.ListIOBoardConfiguration = new List<AjinIoAxlBoardConfiguration>();
            AjinIoAxlBoardConfiguration ioBoardConfig = new AjinIoAxlBoardConfiguration();
            propertyGridIOBoard.SelectedObject = ioBoardConfig;
            LoadBoardCollection();
        }
        #endregion

        #region Method
        private void CreatGridViewColumn()
        {
            foreach (IOBoardColumnList one in Enum.GetValues(typeof(IOBoardColumnList)))
            {
                switch (one)
                {
                    case IOBoardColumnList.ComponentUid:
                    case IOBoardColumnList.No:
                        {
                            dataGridViewIOBoard.Columns.Add(one.ToString(), one.ToString());
                        }
                        break;
                    case IOBoardColumnList.Simulated:
                        {
                            DataGridViewCheckBoxColumn checkColumn = new DataGridViewCheckBoxColumn();
                            checkColumn.HeaderText = one.ToString();
                            dataGridViewIOBoard.Columns.Add(checkColumn);
                        }
                        break;
                }
            }
        }
        private void AddNewRow(IOBoadConfiguration info)
        {
            int nRow = this.dataGridViewIOBoard.Rows.Add();
            this.dataGridViewIOBoard[(int)IOBoardColumnList.ComponentUid, nRow].Value = info.ComponentUid;
            this.dataGridViewIOBoard[(int)IOBoardColumnList.Simulated, nRow].Value = info.Simulated;
            this.dataGridViewIOBoard[(int)IOBoardColumnList.No, nRow].Value = info.No;
        }
        #endregion

        #region EventHandler
        private void ButtonNew_Click(object sender, EventArgs e)
        {
            AjinIoAxlBoardConfiguration info = new AjinIoAxlBoardConfiguration();
            this.ListIOBoardConfiguration.Add(info);
            AddNewRow(info);
            int RowIndex = 0;
            RowIndex = dataGridViewIOBoard.SelectedCells[0].RowIndex;
            if (ListIOBoardConfiguration.Count <= RowIndex)
            {
                RowIndex = ListIOBoardConfiguration.Count - 1;
            }
            if (RowIndex >= 0)
            {
                info = ListIOBoardConfiguration[RowIndex];
            }
            propertyGridIOBoard.SelectedObject = info;
        }
        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewIOBoard.RowCount > 0 && dataGridViewIOBoard.SelectedRows != null)
            {
                int rowindex = dataGridViewIOBoard.SelectedCells[0].RowIndex;
                this.ListIOBoardConfiguration.RemoveAt(rowindex);
                dataGridViewIOBoard.Rows.Remove(dataGridViewIOBoard.Rows[rowindex]);
            }
        }
        private void ButtonSave_Click(object sender, EventArgs e)
        {
            Equipment.SaveIOBoard(ListIOBoardConfiguration);
        }
        private void ButtonLoad_Click(object sender, EventArgs e)
        {
            LoadBoardCollection();
        }
        private void LoadBoardCollection()
        {
            dataGridViewIOBoard.Rows.Clear();
            Equipment.LoadIOBoards();
            ListIOBoardConfiguration = Equipment.GetIOConfigurationList();
            foreach (IOBoadConfiguration config in ListIOBoardConfiguration)
            {
                AddNewRow(config);
            }
        }
        private void DataGridViewIOBoard_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (ListIOBoardConfiguration.Count > 0 && ListIOBoardConfiguration.Count > e.RowIndex)
            {
                if (dataGridViewIOBoard[e.ColumnIndex, e.RowIndex].Value != null)
                {
                    string ColumnData = dataGridViewIOBoard[e.ColumnIndex, e.RowIndex].Value.ToString();
                    AjinIoAxlBoardConfiguration info = ListIOBoardConfiguration[e.RowIndex];

                    switch ((IOBoardColumnList)e.ColumnIndex)
                    {
                        case IOBoardColumnList.ComponentUid:
                            info.ComponentUid = ColumnData;
                            break;
                        case IOBoardColumnList.No:
                            info.No = int.Parse(ColumnData);
                            break;
                        case IOBoardColumnList.Simulated:
                            info.Simulated = Convert.ToBoolean(ColumnData);
                            break;
                        default:
                            break;
                    }
                    propertyGridIOBoard.SelectedObject = info;
                }
                else if (dataGridViewIOBoard[e.ColumnIndex, e.RowIndex].Value == null)
                {

                }
            }
        }
        private void DataGridViewIOBoard_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dataGridViewIOBoard.SelectedCells.Count > 0)
            {
                AjinIoAxlBoardConfiguration config = null;
                int RowIndex = 0;
                RowIndex = dataGridViewIOBoard.SelectedCells[0].RowIndex;
                if (ListIOBoardConfiguration.Count <= RowIndex)
                {
                    RowIndex = ListIOBoardConfiguration.Count - 1;
                }
                if (RowIndex >= 0)
                {
                    config = ListIOBoardConfiguration[RowIndex];
                }
                propertyGridIOBoard.SelectedObject = config;
            }
        }
        private void PropertyGridIOBoard_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (dataGridViewIOBoard.SelectedRows != null && ListIOBoardConfiguration.Count > 0 && ListIOBoardConfiguration.Count > dataGridViewIOBoard.SelectedCells[0].RowIndex)
            {
                AjinIoAxlBoardConfiguration config = null;
                bool CheckedData = default;
                string changedValue = string.Empty;
                config = ListIOBoardConfiguration[dataGridViewIOBoard.SelectedCells[0].RowIndex];
                if (e.ChangedItem.Label == IOBoardColumnList.Simulated.ToString())
                {
                    CheckedData = Convert.ToBoolean(e.ChangedItem.Value);
                }
                else
                {
                    changedValue = e.ChangedItem.Value.ToString();
                }
                switch (e.ChangedItem.Label)
                {
                    case "ComponentUid":
                        config.ComponentUid = changedValue;
                        break;
                    case "No":
                        config.No = int.Parse(changedValue);
                        break;
                    case "Simulated":
                        if (CheckedData == true)
                        {
                            config.Simulated = true;
                        }
                        else if (CheckedData == false)
                        {
                            config.Simulated = false;
                        }
                        break;
                    default:
                        break;
                }
                int nRow = dataGridViewIOBoard.SelectedCells[0].RowIndex;
                this.dataGridViewIOBoard[(int)IOBoardColumnList.ComponentUid, nRow].Value = config.ComponentUid;
                this.dataGridViewIOBoard[(int)IOBoardColumnList.Simulated, nRow].Value = config.Simulated;
                this.dataGridViewIOBoard[(int)IOBoardColumnList.No, nRow].Value = config.No;
            }
        }
        #endregion       
    }
}