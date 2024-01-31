using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QMC.Common;

namespace CWA150SA_Onsemi300
{
    
    public partial class FormDigitalIO : FormSubContentBase
    {
        #region Define
        public enum DigitalIOColumnList
        {   
            Name, Description, ModuleNo, Address, Enabled, IoType, Locator, ModuleUid, PartUid, Tag, Simulated
        }
        #endregion

        #region Field

        List<IOPointConfiguration> m_listDigitalIOConfiguration;
        #endregion
        
        #region Property

        public List<IOPointConfiguration> ListDigitalIOConfiguration
        {
            get { return m_listDigitalIOConfiguration; }
            set { m_listDigitalIOConfiguration = value; }
        }
        #endregion

        #region Constructor
        public FormDigitalIO()
            :base(FormType.Content.ToString(), "Digital IO")
        {
            InitializeComponent();
            base.flowLayoutPanelButton.Controls.Add(this.buttonSave);
            base.flowLayoutPanelButton.Controls.Add(this.buttonLoad);
            base.flowLayoutPanelButton.Controls.Add(this.buttonDelete);
            base.flowLayoutPanelButton.Controls.Add(this.buttonNew);

            base.panelContent.Controls.Add(this.dataGridViewDigital);
            this.panelContent.Location = new Point(Configuration.ContentLocation.X+10, this.Configuration.PanelSize.Height*2);
            this.panelContent.Size = new System.Drawing.Size(Configuration.PanelbuttonSize.Width - Configuration.ContentLocation.X, Configuration.FormContentSize.Height - Configuration.PanelSize.Height*2);
            this.flowLayoutPanelButton.FlowDirection = FlowDirection.RightToLeft;
            //this.flowLayoutPanelButton.Size = new System.Drawing.Size(this.Configuration.PanelSize.Width - this.dataGridViewDigital.Width, Configuration.PanelSize.Height);
            //this.flowLayoutPanelButton.Location = new Point(this.Configuration.PanelSize.Width - this.flowLayoutPanelButton.Width-10, this.Configuration.PanelSize.Height);

            this.buttonDelete.Size = Configuration.ButtonSize;
            this.buttonDelete.Size = Configuration.ButtonSize;
            this.buttonLoad.Size = Configuration.ButtonSize;
            this.buttonSave.Size = Configuration.ButtonSize;
            this.buttonNew.Size = Configuration.ButtonSize;


            this.dataGridViewDigital.Location = new System.Drawing.Point(0, 0);
            
            this.dataGridViewDigital.Size = new System.Drawing.Size(this.panelContent.Size.Width-10, Configuration.ContentSize.Height - Configuration.PanelSize.Height * 4 - this.flowLayoutPanelButton.Height);
           
            CreatGridViewColumn();
            this.ListDigitalIOConfiguration = new List<IOPointConfiguration>();
            LoadModuleConfiguration();
        }

        #endregion

        #region Method
        private void CreatGridViewColumn()
        {
            foreach (DigitalIOColumnList one in Enum.GetValues(typeof(DigitalIOColumnList)))
            {
                switch (one)
                {
                    case DigitalIOColumnList.Name:
                    case DigitalIOColumnList.Description:
                    case DigitalIOColumnList.Address:
                    case DigitalIOColumnList.ModuleUid:
                    case DigitalIOColumnList.Locator:
                    case DigitalIOColumnList.PartUid:
                    case DigitalIOColumnList.Tag:
                    case DigitalIOColumnList.ModuleNo:
                        //case DigitalIOColumnList.ModuleNo:
                        {
                            dataGridViewDigital.Columns.Add(one.ToString(), one.ToString());
                        }
                        break;

                    case DigitalIOColumnList.IoType:
                        {
                            DataGridViewComboBoxColumn comboColumn = new DataGridViewComboBoxColumn();
                            comboColumn.HeaderText = one.ToString();
                            comboColumn.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                            comboColumn.DefaultCellStyle.BackColor = Color.FromArgb(200, 200, 200);
                            comboColumn.FlatStyle = FlatStyle.Flat;
                            dataGridViewDigital.Columns.Add(comboColumn);
                            comboColumn.Name = one.ToString();
                            foreach (IoType IoType in Enum.GetValues(typeof(IoType)))
                            {
                                comboColumn.Items.Add(IoType.ToString());
                            }
                        }
                        break;
                    case DigitalIOColumnList.Enabled:
                    case DigitalIOColumnList.Simulated:
                        {
                            DataGridViewCheckBoxColumn checkColumn = new DataGridViewCheckBoxColumn();
                            checkColumn.HeaderText = one.ToString();
                            dataGridViewDigital.Columns.Add(checkColumn);
                            break;
                        }

                }
            }
        }
        private void AddNewRow(IOPointConfiguration info)
        {
            int nRow = this.dataGridViewDigital.Rows.Add();
            this.dataGridViewDigital[(int)DigitalIOColumnList.Enabled, nRow].Value = info.Enabled;
            this.dataGridViewDigital[(int)DigitalIOColumnList.Description, nRow].Value = info.Description;
            this.dataGridViewDigital[(int)DigitalIOColumnList.ModuleNo, nRow].Value = info.ModuleNo;
            this.dataGridViewDigital[(int)DigitalIOColumnList.Address, nRow].Value = info.Address;
            this.dataGridViewDigital[(int)DigitalIOColumnList.IoType, nRow].Value = info.IoType.ToString();
            this.dataGridViewDigital[(int)DigitalIOColumnList.ModuleUid, nRow].Value = info.ModuleUid;
            this.dataGridViewDigital[(int)DigitalIOColumnList.PartUid, nRow].Value = info.PartUid;
            this.dataGridViewDigital[(int)DigitalIOColumnList.Locator, nRow].Value = info.Locator;
            this.dataGridViewDigital[(int)DigitalIOColumnList.Tag, nRow].Value = info.Tag;
            this.dataGridViewDigital[(int)DigitalIOColumnList.Simulated, nRow].Value = info.Simulated;
            //this.dataGridViewDigital[(int)DigitalIOColumnList.ModuleNo, nRow].Value = info.ModuleNo;
            this.dataGridViewDigital[(int)DigitalIOColumnList.Name, nRow].Value = info.Name;
        }
        private void LoadModuleConfiguration()
        {
            dataGridViewDigital.Rows.Clear();
            ListDigitalIOConfiguration = Equipment.GetIOPointConfigurationList();
            foreach (IOPointConfiguration config in ListDigitalIOConfiguration)
            {
                AddNewRow(config);
            }
        }
        #endregion

        #region EventHandler
        private void ButtonNew_Click(object sender, EventArgs e)
        {
            DioPointConfiguration info = new DioPointConfiguration();
            info.UID = Equipment.GetAxisUID();
            this.ListDigitalIOConfiguration.Add(info);
            AddNewRow(info);
            int RowIndex = 0;
            RowIndex = dataGridViewDigital.SelectedCells[0].RowIndex;
            if (ListDigitalIOConfiguration.Count <= RowIndex)
            {
                RowIndex = ListDigitalIOConfiguration.Count - 1;
            }

            if (RowIndex >= 0)
            {
                info = ListDigitalIOConfiguration[RowIndex] as DioPointConfiguration;
            }
        }
        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewDigital.RowCount > 0 && dataGridViewDigital.SelectedRows != null)
            {
                int rowindex = dataGridViewDigital.SelectedCells[0].RowIndex;
                this.ListDigitalIOConfiguration.RemoveAt(rowindex);
                dataGridViewDigital.Rows.Remove(dataGridViewDigital.Rows[rowindex]);
            }
        }
        private void ButtonSave_Click(object sender, EventArgs e)
        {
            Equipment.SaveIOPoints(ListDigitalIOConfiguration);
        }
        private void ButtonLoad_Click(object sender, EventArgs e)
        {
            Equipment.LoadIOBoards();
            LoadModuleConfiguration();
        }
        private void DataGridViewDigitalIO_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (ListDigitalIOConfiguration.Count > 0 && ListDigitalIOConfiguration.Count > e.RowIndex)
            {
                if (dataGridViewDigital[e.ColumnIndex, e.RowIndex].Value != null)
                {
                    string ColumnData = dataGridViewDigital[e.ColumnIndex, e.RowIndex].Value.ToString();
                    IOPointConfiguration info = ListDigitalIOConfiguration[e.RowIndex];
                    switch ((DigitalIOColumnList)e.ColumnIndex)
                    {
                        case DigitalIOColumnList.PartUid:
                            info.PartUid = ColumnData;
                            break;
                        case DigitalIOColumnList.Tag:
                            info.Tag = ColumnData;
                            break;
                        case DigitalIOColumnList.Description:
                            info.Description = ColumnData;
                            break;
                        case DigitalIOColumnList.Name:
                            info.Name = ColumnData;
                            break;
                        case DigitalIOColumnList.ModuleUid:
                            info.ModuleUid = ColumnData;
                            break;
                        case DigitalIOColumnList.Locator:
                            info.Locator = ColumnData;
                            break;
                        //case DigitalIOColumnList.Label:
                        //    info.Label = ColumnData;
                        //    break;
                        case DigitalIOColumnList.Address:
                            info.Address = int.Parse(ColumnData);
                            break;
                        case DigitalIOColumnList.ModuleNo:
                            info.ModuleNo = uint.Parse(ColumnData);
                            break;
                        case DigitalIOColumnList.Simulated:
                            info.Simulated = Convert.ToBoolean(ColumnData);
                            break;
                        case DigitalIOColumnList.Enabled:
                            info.Enabled = Convert.ToBoolean(ColumnData);
                            break;
                        case DigitalIOColumnList.IoType:
                            info.IoType = (IoType)Enum.Parse(typeof(IoType), (string)ColumnData);
                            break;
                        default:
                            break;
                    }
                }
                else if (dataGridViewDigital[e.ColumnIndex, e.RowIndex].Value == null)
                {

                }
            }
        }
        private void DataGridViewDigitalIO_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dataGridViewDigital.SelectedCells.Count > 0)
            {
                IOPointConfiguration config = null;
                int RowIndex = 0;
                RowIndex = dataGridViewDigital.SelectedCells[0].RowIndex;
                if (ListDigitalIOConfiguration.Count <= RowIndex)
                {
                    RowIndex = ListDigitalIOConfiguration.Count - 1;
                }
                if (RowIndex >= 0)
                    config = ListDigitalIOConfiguration[RowIndex];
            }
        }
        #endregion
    }
}
