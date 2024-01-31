using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CWA150SA_Onsemi300;
using QMC.Common;
using QMC.Common.Motion.Ajin.IO;

namespace CWA150SA_Onsemi300
{
    #region Define

    public enum InterLockIoList
    {
        Name, Address, ModuleName, IoType
    }
    public enum buttonType
    {
        Cancel, Apply
    }
    #endregion

    #region Field

    public delegate void SaveInterlockIOEventHandler(buttonType type);
    #endregion
    public partial class FormInterLockIO : Form
    {
        #region Field

        public event SaveInterlockIOEventHandler m_SaveInterlockIO;
        private FormBaseConfiguration m_configuration = new FormBaseConfiguration();
        private List<IOPoint> m_datas;
        private int m_RowIndex = 0;
        private int m_findRowIndex = 0;
        private IOPoint m_selectedData = null;
        #endregion

        #region Property
        public FormBaseConfiguration Configuration 
        {
            get { return m_configuration; }
            set { m_configuration = value; }
        }

        public Interlock Interlock { get; set; }
        #endregion

        #region Constructor

        public FormInterLockIO()
        {
            InitializeComponent();

            #region ControlsConstructor

            this.Size = new Size(Configuration.ContentSize.Width / 2 + (Configuration.ContentSize.Width / 2) / 2 - Configuration.ButtonSize.Width -90, Configuration.PanelSize.Height + 5);
            this.BackColor = Color.FromArgb(38, 38, 38);


            this.flowLayoutPanelIntelockIO.Size = new Size(Configuration.ButtonSize.Width * 3, Configuration.ButtonSize.Height + 5);
            this.flowLayoutPanelIntelockIO.Location = new Point(this.Width - this.flowLayoutPanelIntelockIO.Width + 60, 2);
            this.flowLayoutPanelIntelockIO.FlowDirection = FlowDirection.RightToLeft;

            this.dataGridViewInterLockIO.Size = new Size(Configuration.ContentSize.Width / 2 + (Configuration.ContentSize.Width / 2) / 2 - Configuration.ButtonSize.Width - 65, Configuration.ContentSize.Height / 2 - Configuration.ButtonSize.Height * 2 - 30);
            this.dataGridViewInterLockIO.Location = new Point(19, Configuration.ButtonSize.Height + 10);
            this.dataGridViewInterLockIO.ReadOnly = true;

            this.baseLabelInterlockIO.Size = new Size(Configuration.ButtonSize.Width * 2, Configuration.ButtonSize.Height * 2);
            this.baseLabelInterlockIO.Location = new Point(this.dataGridViewInterLockIO.Location.X, this.flowLayoutPanelIntelockIO.Location.Y + 5);
            this.baseLabelInterlockIO.Text = "Value";
            this.baseLabelInterlockIO.TextAlign = ContentAlignment.MiddleLeft;

            this.comboBoxInterLockIO.Size = new Size(Configuration.ButtonSize.Width, Configuration.ButtonSize.Height);
            this.comboBoxInterLockIO.Location = new Point(this.baseLabelInterlockIO.Location.X + this.baseLabelInterlockIO.Width, this.flowLayoutPanelIntelockIO.Location.Y + 5);
            
            #endregion

            m_datas = new List<IOPoint>();
            CreatDataGridViewInterLockIOColums();
            UpdateDataGridViewData();
            CreatComboboxList();
            CreatInterlockButton();
            
            this.Load += FormInterLockIO_Load;
                
        }
        #endregion

        #region Event Handler

        private void FormInterLockIO_Load(object sender, EventArgs e)
        {
            if (Interlock == null)
            {
                Interlock = new Interlock();
            }
            else
            {
                LoadModifyInfo();
            }
        }
        #region DataGridViewInterLockAxis_CurrentCellChanged
        private void DataGridViewInterLockAxis_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dataGridViewInterLockIO.RowCount > 0)
            {

                if (dataGridViewInterLockIO.SelectedCells != null)
                {
                    m_RowIndex = 0;
                }
                else
                {
                    m_RowIndex = dataGridViewInterLockIO.SelectedCells[0].RowIndex;
                    if (m_datas.Count <= m_RowIndex)
                    {
                        m_RowIndex = m_datas.Count - 1;
                    }
                    if (m_RowIndex >= 0)
                    {
                        m_selectedData = m_datas[m_RowIndex];
                    }
                }
            }
        }
        #endregion

        #region buttonApply_Click
        private void buttonIOApply_Click(object sender, EventArgs e)
        {
            m_RowIndex = dataGridViewInterLockIO.SelectedCells[0].RowIndex;
            if (m_datas[m_RowIndex] != null && Interlock.Value != null)
            {
                Interlock.Target = m_datas[m_RowIndex];
                if (comboBoxInterLockIO.SelectedItem != null)
                {
                    if (comboBoxInterLockIO.SelectedItem.ToString() == "On")
                    {
                        Interlock.Value.StringValue = "On";
                    }
                    else if (comboBoxInterLockIO.SelectedItem.ToString() == "Off")
                    {
                        Interlock.Value.StringValue = "Off";
                    }
                }
                Interlock.Condition = InterlockCondition.Equal;

                if (m_SaveInterlockIO != null)
                {
                    m_SaveInterlockIO(buttonType.Apply);
                }
            }
        }
        #endregion

        #region buttonCancel_Click
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (m_SaveInterlockIO != null)
            {
                m_SaveInterlockIO(buttonType.Cancel);
            }
        }
        #endregion

        #endregion

        #region Method

        #region CreatDataGridViewInterLockIOColums
        private void CreatDataGridViewInterLockIOColums()
        {
            dataGridViewInterLockIO.Columns.Clear();
            foreach (InterLockIoList one in Enum.GetValues(typeof(InterLockIoList)))
            {
                switch (one)
                {
                    case InterLockIoList.Name:
                    case InterLockIoList.ModuleName:
                    case InterLockIoList.Address:
                    case InterLockIoList.IoType:
                        {
                            dataGridViewInterLockIO.Columns.Add(one.ToString(), one.ToString());
                        }
                        break;
                }
            }
        }
        #endregion

        #region GetDataGridViewData
        private void UpdateDataGridViewData()
        {
            m_datas.Clear();
            foreach (AjinAxlIoBoard board in Equipment.IOBoards)
            {
                foreach (AjinAxlDioModule module in board.Modules)
                {
                    foreach (IOPoint point in module.Points)
                    {
                        m_datas.Add(point);
                    }
                }
            }

            dataGridViewInterLockIO.Rows.Clear();
            int nindex = m_datas.Count();
            for (int i = 0; i < nindex; i++)
            {
                dataGridViewInterLockIO.Rows.Add();
                this.dataGridViewInterLockIO[(int)InterLockIoList.Name, i].Value = m_datas[i].Configuration.Name;
                this.dataGridViewInterLockIO[(int)InterLockIoList.Address, i].Value = m_datas[i].Configuration.Address;
                this.dataGridViewInterLockIO[(int)InterLockIoList.IoType, i].Value = m_datas[i].Configuration.IoType;
                this.dataGridViewInterLockIO[(int)InterLockIoList.ModuleName, i].Value = m_datas[i].Name;
            }
        }
        #endregion


        #region CreatComboboxList
        public void CreatComboboxList()
        {
            comboBoxInterLockIO.Items.Clear();
            comboBoxInterLockIO.Items.Add("On");
            comboBoxInterLockIO.Items.Add("Off");
            comboBoxInterLockIO.SelectedIndex = 0;
        }
        #endregion

        #region CreatInterlockButton
        public void CreatInterlockButton()
        {
            Point buttonInterlockLocation = new Point();
            buttonInterlockLocation.X = 0;
            buttonInterlockLocation.Y = 0;
            foreach (buttonType buttonInterlock in Enum.GetValues(typeof(buttonType)))
            {
                BaseButton btn = new BaseButton();
                switch (buttonInterlock)
                {
                    case buttonType.Apply:
                        btn.Text = buttonInterlock.ToString();
                        btn.Name = string.Format("button", buttonInterlock.ToString());
                        btn.Size = new System.Drawing.Size(Configuration.ButtonSize.Width, Configuration.ButtonSize.Height);
                        btn.Location = buttonInterlockLocation;
                        btn.Click += buttonIOApply_Click;
                        break;
                    case buttonType.Cancel:
                        btn.Text = buttonInterlock.ToString();
                        btn.Name = string.Format("button", buttonInterlock.ToString());
                        btn.Size = new System.Drawing.Size(Configuration.ButtonSize.Width, Configuration.ButtonSize.Height);
                        btn.Location = buttonInterlockLocation;
                        btn.Click += buttonCancel_Click;
                        break;
                }
                flowLayoutPanelIntelockIO.Controls.Add(btn);
            }
        }
        #endregion

        public void LoadModifyInfo()
        {
            dataGridViewInterLockIO.ClearSelection();
            for (int i = 0; i < dataGridViewInterLockIO.RowCount; i++)
            {
                if (Interlock.Target.ToString() == this.dataGridViewInterLockIO[(int)InterLockIoList.Name, i].Value.ToString())
                {
                    m_findRowIndex = i;
                    dataGridViewInterLockIO[0, m_findRowIndex].Selected = true;
                    break;
                }
            }
            comboBoxInterLockIO.SelectedItem = Interlock.Value.StringValue;
        }
        #endregion
    }
}
