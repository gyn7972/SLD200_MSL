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
using QMC.Common.Parts;
using QMC.Common;

namespace CWA150SA_Onsemi300
{
    public partial class FormSPCMain : FormSubContentBase
    {
        protected TabControl TabControlSPC = new TabControl();
        protected TabPage TabPageData = new TabPage("Data");
        protected TabPage TabPageChart = new TabPage("Chart");

        protected SPCOptionControl m_SPCOptionControl;

        protected DataGridView m_DataGridViewSearchValue;

        protected BaseButton m_CsvSaveButton;

        protected XRsChartControl m_SpcControl;
        protected Timer m_Timer;

        public FormSPCMain()
            : base(FormType.withButton.ToString(), "Monitering")
        {
            InitializeComponent();
            flowLayoutPanelButton.Visible = false;
            flowLayoutPanelButton.Size = new Size();
            panelContent.Visible = false;
            panelContent.Size = new System.Drawing.Size();
            baseLabelTitle.Visible = false;
            baseLabelTitle.Size = new Size();

            m_SPCOptionControl = new SPCOptionControl();
            m_SPCOptionControl.Location = new Point(Configuration.ContentLocation.X, Configuration.ContentLocation.Y - 20);
            m_SPCOptionControl.SearchClick += SearchClick;
            m_SPCOptionControl.DataSaveClick += SaveDataClick;
            this.Controls.Add(m_SPCOptionControl);

            TabControlSPC = new TabControl();
            TabControlSPC.Size = new Size(Configuration.ContentSize.Width - m_SPCOptionControl.Size.Width - m_SPCOptionControl.Location.X - Configuration.ControlGap, Configuration.ContentSize.Height - Configuration.ControlGap * 2);
            TabControlSPC.Location = new Point(m_SPCOptionControl.Location.X + m_SPCOptionControl.Size.Width + Configuration.ControlGap, m_SPCOptionControl.Location.Y);
            TabControlSPC.ItemSize = new Size(100, 25);
            
            TabPageData = new TabPage("Data");
            TabPageChart = new TabPage("Chart");

            #region TabPage - Data
            m_DataGridViewSearchValue = new DataGridView();
            m_DataGridViewSearchValue.Size = new Size(TabControlSPC.Size.Width, TabControlSPC.Size.Height);
            m_DataGridViewSearchValue.AllowUserToResizeRows = false;
            m_DataGridViewSearchValue.RowHeadersVisible = false;
            m_DataGridViewSearchValue.ReadOnly = true;
            m_DataGridViewSearchValue.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            #region 주석
            //DataGridViewTextBoxColumn textboxColumn;
            //textboxColumn = new DataGridViewTextBoxColumn();
            //textboxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //textboxColumn.FillWeight = 45;
            //textboxColumn.ReadOnly = true;
            //textboxColumn.HeaderText = "Index";
            //m_DataGridViewSearchValue.Columns.Add(textboxColumn);
            //textboxColumn = new DataGridViewTextBoxColumn();
            //textboxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //textboxColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            //textboxColumn.ReadOnly = true;
            //textboxColumn.HeaderText = "CarrierID";
            //m_DataGridViewSearchValue.Columns.Add(textboxColumn);
            //textboxColumn = new DataGridViewTextBoxColumn();
            //textboxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //textboxColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            //textboxColumn.ReadOnly = true;
            //textboxColumn.HeaderText = "UnitID";
            //m_DataGridViewSearchValue.Columns.Add(textboxColumn);
            //textboxColumn = new DataGridViewTextBoxColumn();
            //textboxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //textboxColumn.FillWeight = 60;
            //textboxColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            //textboxColumn.ReadOnly = true;
            //textboxColumn.HeaderText = "Location NO.";
            //m_DataGridViewSearchValue.Columns.Add(textboxColumn);
            //textboxColumn = new DataGridViewTextBoxColumn();
            //textboxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //textboxColumn.FillWeight = 70;
            //textboxColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            //textboxColumn.ReadOnly = true;
            //textboxColumn.HeaderText = "Vf1";
            //m_DataGridViewSearchValue.Columns.Add(textboxColumn);
            //textboxColumn = new DataGridViewTextBoxColumn();
            //textboxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //textboxColumn.FillWeight = 70;
            //textboxColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            //textboxColumn.ReadOnly = true;
            //textboxColumn.HeaderText = "Vf2";
            //m_DataGridViewSearchValue.Columns.Add(textboxColumn);
            //textboxColumn = new DataGridViewTextBoxColumn();
            //textboxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //textboxColumn.FillWeight = 70;
            //textboxColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            //textboxColumn.ReadOnly = true;
            //textboxColumn.HeaderText = "NTC";
            //m_DataGridViewSearchValue.Columns.Add(textboxColumn);
            #endregion

            m_CsvSaveButton = new BaseButton();
            m_CsvSaveButton.Size = new Size(100, 70);
            m_CsvSaveButton.Location = new Point(m_DataGridViewSearchValue.Location.X,
                                            m_DataGridViewSearchValue.Location.Y + m_DataGridViewSearchValue.Height + Configuration.ControlGap);

            TabPageData.Controls.Add(m_DataGridViewSearchValue);
            #endregion

            m_SpcControl = new XRsChartControl();
            //m_SpcControl.Location = new Point(Configuration.ContentLocation.X, Configuration.ContentLocation.Y);
            m_SpcControl.Size = TabControlSPC.Size;
            TabPageChart.Controls.Add(this.m_SpcControl);

            TabControlSPC.Controls.Add(TabPageData);
            TabControlSPC.Controls.Add(TabPageChart);

            this.Controls.Add(TabControlSPC);
        }

        /// <summary>
        /// TODO : DataGridView Value 값 넣기. (어떤 형식의 값 넣을지)
        /// </summary>
        protected void SetData(object datas)
        {
            m_DataGridViewSearchValue.DataSource = null;
            m_DataGridViewSearchValue.DataSource = datas;
        }

        /// <summary>
        /// TODO : DataGridView Value 값 넣기. (어떤 형식의 값 넣을지)
        /// </summary>
        protected object GetCurrentData()
        {
            return m_DataGridViewSearchValue.DataSource;
            //m_DataGridViewSearchValue.DataSource = null;
            //m_DataGridViewSearchValue.DataSource = datas;
        }

        /// <summary>
        /// Search Click시 발생.
        /// </summary>
        protected void SearchClick(string strUnitID, DateTime startTime, DateTime endTime)
        {
            OnSearchClick(strUnitID, startTime, endTime);
        }

        protected virtual void OnSearchClick(string strUnitID, DateTime startTime, DateTime endTime)
        {

        }

        protected void SaveDataClick()
        {
            OnSaveDataClick();
        }

        protected virtual void OnSaveDataClick()
        {

        }
    }
}
