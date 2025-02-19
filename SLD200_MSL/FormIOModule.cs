using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using QMC.Common.Motion.Ajin.IO;
using System.IO;
using QMC.Common;

namespace SLD200_MSL
{
    public partial class FormIOModule : FormSubContentBase
    {                
        #region Define
        public enum IOModuleColumnList
        {
           No, ComponentUid, BoardNo, InputCount, OutputCount, RefreshTime, Remoted, Simulated
        }
        #endregion

        #region Field
        private List<AjinAxlDioModuleConfiguration> m_listIOModuleConfiguration;
        #endregion

        #region Property
        public List<AjinAxlDioModuleConfiguration> ListIOModuleConfiguration
        {
            get { return m_listIOModuleConfiguration; }
            set { m_listIOModuleConfiguration = value; }
        }

        #endregion

        #region Constructor
        public FormIOModule()
            :base(FormType.Content.ToString(), "IO Module")
        {
            InitializeComponent();

            this.BackColor = Configuration.PanelBackColor;

            base.flowLayoutPanelButton.Controls.Add(this.buttonSave);
            base.flowLayoutPanelButton.Controls.Add(this.buttonLoad);
            base.flowLayoutPanelButton.Controls.Add(this.buttonDelete);
            base.flowLayoutPanelButton.Controls.Add(this.buttonNew);
            base.flowLayoutPanelButton.BackColor = Configuration.PanelBackColor;

            base.panelContent.Controls.Add(this.dataGridViewIOModule);
            base.panelContent.Controls.Add(this.propertyGridIOModule);
            this.panelContent.Location = new Point(Configuration.ContentLocation.X+10, this.Configuration.PanelSize.Height*2);
            this.panelContent.Size = new Size(Configuration.MainSize.Width - this.panelContent.Location.X, Configuration.FormContentSize.Height - Configuration.PanelSize.Height * 2);
            base.panelContent.BackColor = Configuration.PanelBackColor; 

            this.flowLayoutPanelButton.FlowDirection = FlowDirection.RightToLeft;

            this.buttonDelete.Size = new System.Drawing.Size(Configuration.ButtonSize.Width, Configuration.ButtonSize.Height);
            this.buttonLoad.Size = new System.Drawing.Size(Configuration.ButtonSize.Width, Configuration.ButtonSize.Height);
            this.buttonSave.Size = new System.Drawing.Size(Configuration.ButtonSize.Width, Configuration.ButtonSize.Height);
            this.buttonNew.Size = new System.Drawing.Size(Configuration.ButtonSize.Width, Configuration.ButtonSize.Height);

            this.propertyGridIOModule.Location = new System.Drawing.Point(Configuration.PanelSize.Width / 2, 0);
            this.propertyGridIOModule.Size = new Size(Configuration.ContentSize.Width / 2 - 30, Configuration.ContentSize.Height - Configuration.PanelSize.Height * 4 - this.flowLayoutPanelButton.Height);

            this.dataGridViewIOModule.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewIOModule.Size = new System.Drawing.Size(Configuration.ContentSize.Width / 2 - Configuration.ContentLocation.X,this.propertyGridIOModule.Height);

            CreatGridViewColumn();
            this.ListIOModuleConfiguration = new List<AjinAxlDioModuleConfiguration>();
            AjinAxlDioModuleConfiguration ioModuleconfig = new AjinAxlDioModuleConfiguration();
            propertyGridIOModule.SelectedObject = ioModuleconfig;
            LoadModuleConfiguration();
        }
        #endregion

        #region Method
        private void CreatGridViewColumn()
        {
            foreach (IOModuleColumnList one in Enum.GetValues(typeof(IOModuleColumnList)))
            {
                switch (one)
                {
                    case IOModuleColumnList.No:
                    case IOModuleColumnList.ComponentUid:
                    case IOModuleColumnList.BoardNo:
                    case IOModuleColumnList.InputCount:
                    case IOModuleColumnList.OutputCount:
                    case IOModuleColumnList.RefreshTime:

                        {
                            dataGridViewIOModule.Columns.Add(one.ToString(), one.ToString());
                        }
                        break;
                    case IOModuleColumnList.Simulated:
                    case IOModuleColumnList.Remoted:
                        {
                            DataGridViewCheckBoxColumn checkColumn = new DataGridViewCheckBoxColumn();
                            checkColumn.HeaderText = one.ToString();
                            dataGridViewIOModule.Columns.Add(checkColumn);
                        }
                        break;

                }
            }
        }
        private void AddNewRow(AjinAxlDioModuleConfiguration info)
        {
            int nRow = this.dataGridViewIOModule.Rows.Add();
            this.dataGridViewIOModule[(int)IOModuleColumnList.Simulated, nRow].Value = info.Simulated;
            this.dataGridViewIOModule[(int)IOModuleColumnList.No, nRow].Value = info.No;
            this.dataGridViewIOModule[(int)IOModuleColumnList.InputCount, nRow].Value = info.InputCount;
            this.dataGridViewIOModule[(int)IOModuleColumnList.OutputCount, nRow].Value = info.OutputCount;
            this.dataGridViewIOModule[(int)IOModuleColumnList.RefreshTime, nRow].Value = info.RefreshTime;
            this.dataGridViewIOModule[(int)IOModuleColumnList.Remoted, nRow].Value = info.Remoted;
            this.dataGridViewIOModule[(int)IOModuleColumnList.BoardNo, nRow].Value = info.BoardNo;
            this.dataGridViewIOModule[(int)IOModuleColumnList.ComponentUid, nRow].Value = info.ComponentUid;
        }
        private void LoadModuleConfiguration()
        {
            dataGridViewIOModule.Rows.Clear();
            Equipment.LoadIOBoards();
            ListIOModuleConfiguration = Equipment.GetIOModuleConfigurationList();
            foreach (AjinAxlDioModuleConfiguration config in ListIOModuleConfiguration)
            {
                AddNewRow(config);
            }
        }

        #endregion

        #region EventHandler
        private void ButtonNew_Click(object sender, EventArgs e)
        {
            AjinAxlDioModuleConfiguration info = new AjinAxlDioModuleConfiguration();
            AddNewRow(info);
            this.ListIOModuleConfiguration.Add(info);            
            int RowIndex = 0;
            RowIndex = dataGridViewIOModule.SelectedCells[0].RowIndex;
            if (ListIOModuleConfiguration.Count <= RowIndex)
            {
                RowIndex = ListIOModuleConfiguration.Count - 1;
            }

            if (RowIndex >= 0)
                info = ListIOModuleConfiguration[RowIndex];

            propertyGridIOModule.SelectedObject = info;
        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewIOModule.RowCount > 0 && dataGridViewIOModule.SelectedRows != null)
            {
                int rowindex = dataGridViewIOModule.SelectedCells[0].RowIndex;
                this.ListIOModuleConfiguration.RemoveAt(rowindex);
                dataGridViewIOModule.Rows.Remove(dataGridViewIOModule.Rows[rowindex]);
            }
        }

        private void ButtonLoad_Click(object sender, EventArgs e)
        {
            Equipment.LoadIOBoards();
            LoadModuleConfiguration();
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            Equipment.SaveIOModule(ListIOModuleConfiguration);
        }

        private void DataGridViewIOModule_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (ListIOModuleConfiguration.Count > 0 && ListIOModuleConfiguration.Count > e.RowIndex)
            {
                if (dataGridViewIOModule[e.ColumnIndex, e.RowIndex].Value != null)
                {
                    string ColumnData = dataGridViewIOModule[e.ColumnIndex, e.RowIndex].Value.ToString();
                    AjinAxlDioModuleConfiguration info = ListIOModuleConfiguration[e.RowIndex];

                    switch ((IOModuleColumnList)e.ColumnIndex)
                    {
                        case IOModuleColumnList.No:                            
                            info.No = uint.Parse(ColumnData);
                            break;
                        case IOModuleColumnList.InputCount:
                            info.InputCount = int.Parse(ColumnData);
                            break;
                        case IOModuleColumnList.OutputCount:
                            info.OutputCount = int.Parse(ColumnData);
                            break;
                        case IOModuleColumnList.RefreshTime:
                            info.RefreshTime = int.Parse(ColumnData);
                            break;
                        case IOModuleColumnList.BoardNo:
                            info.BoardNo = int.Parse(ColumnData);
                            break;
                        case IOModuleColumnList.ComponentUid:
                            info.ComponentUid = ColumnData;
                            break;
                        case IOModuleColumnList.Simulated:
                            info.Simulated = Convert.ToBoolean(ColumnData);
                            break;
                        case IOModuleColumnList.Remoted:
                            info.Remoted = Convert.ToBoolean(ColumnData);
                            break;

                        default:
                            break;
                    }
                    propertyGridIOModule.SelectedObject = info;
                //    ListIOModuleConfiguration[e.RowIndex] = info;
                }

                else if(dataGridViewIOModule[e.ColumnIndex, e.RowIndex].Value == null)
                {

                }
            }
        }


        private void DataGridViewIOModule_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dataGridViewIOModule.SelectedCells.Count > 0)
            {
                AjinAxlDioModuleConfiguration config = null;
                int RowIndex = 0;
                RowIndex = dataGridViewIOModule.SelectedCells[0].RowIndex;
                if (ListIOModuleConfiguration.Count <= RowIndex)
                {
                    RowIndex = ListIOModuleConfiguration.Count - 1;
                }

                if (RowIndex >= 0)
                    config = ListIOModuleConfiguration[RowIndex];

                propertyGridIOModule.SelectedObject = config;
            }
        }

        private void PropertyGridIOModule_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (dataGridViewIOModule.SelectedRows != null && ListIOModuleConfiguration.Count > 0 && ListIOModuleConfiguration.Count > dataGridViewIOModule.SelectedCells[0].RowIndex)
            {
                AjinAxlDioModuleConfiguration config = null;
                bool CheckedData = default;
                string changedValue = string.Empty;
                config = ListIOModuleConfiguration[dataGridViewIOModule.SelectedCells[0].RowIndex];
                if (e.ChangedItem.Label == IOModuleColumnList.Simulated.ToString() || e.ChangedItem.Label == IOModuleColumnList.Remoted.ToString())
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
                    case "BoardNo":
                        config.BoardNo = int.Parse(changedValue);
                        break;
                    case "No":
                        config.No = uint.Parse(changedValue);
                        break;
                    case "OutputCount":
                        config.OutputCount = int.Parse(changedValue);
                        break;
                    case "InputCount":
                        config.InputCount = int.Parse(changedValue);
                        break;
                    case "RefresTime":
                        config.RefreshTime = int.Parse(changedValue);
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
                    case "Remoted":
                        if (CheckedData == true)
                        {
                            config.Remoted = true;
                        }
                        else if (CheckedData == false)
                        {
                            config.Remoted = false;
                        }
                        break;
                    default:
                        break;
                }
                int nRow = dataGridViewIOModule.SelectedCells[0].RowIndex;
                this.dataGridViewIOModule[(int)IOModuleColumnList.ComponentUid, nRow].Value = config.ComponentUid;
                this.dataGridViewIOModule[(int)IOModuleColumnList.BoardNo, nRow].Value = config.BoardNo;
                this.dataGridViewIOModule[(int)IOModuleColumnList.Simulated, nRow].Value = config.Simulated;
                this.dataGridViewIOModule[(int)IOModuleColumnList.No, nRow].Value = config.No.ToString();
                this.dataGridViewIOModule[(int)IOModuleColumnList.InputCount, nRow].Value = config.InputCount.ToString();
                this.dataGridViewIOModule[(int)IOModuleColumnList.OutputCount, nRow].Value = config.OutputCount.ToString();
                this.dataGridViewIOModule[(int)IOModuleColumnList.RefreshTime, nRow].Value = config.RefreshTime.ToString();
                this.dataGridViewIOModule[(int)IOModuleColumnList.Remoted, nRow].Value = config.Remoted;

            }
        }
        #endregion
        
    }
}
