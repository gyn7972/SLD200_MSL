using QMC.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QMC.Common.Vision.Optics;
using System.Threading;

namespace SLD200_MSL
{
    public partial class IlluminatorChannelControl : UserControl
    {
        protected List<IlluminationChannel> m_listIlluminationChannel;
        public List<IlluminationChannel> listIlluminationChannel
        { 
            get
            {
                return m_listIlluminationChannel;
            }
            set
            {
                m_listIlluminationChannel = value;
            }
        }


        public IlluminatorChannelControl()
        {
            listIlluminationChannel = new List<IlluminationChannel> ();
       
            InitializeComponent();
            InitdataGridViewParameterColumns();

            this.Load += IlluminatorChannelControl_Load;
        }

        private void IlluminatorChannelControl_Load(object sender, EventArgs e)
        {
            UpdateGridConfigData();
        }

        private void InitdataGridViewParameterColumns()
        {
            baseDataGridViewIlluminator.Columns.Clear();
            baseDataGridViewIlluminator.AutoGenerateColumns = false;
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.Name = "ChannelName";
                column.HeaderText = "ChannelName";
                column.DataPropertyName = "ChannelName";
                column.Tag = "ChannelName";
                baseDataGridViewIlluminator.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.Name = "Channel";
                column.HeaderText = "Channel";
                column.DataPropertyName = "Channel";
                column.Tag = "Channel";
                baseDataGridViewIlluminator.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.Name = "Min";
                column.HeaderText = "Min";
                column.DataPropertyName = "Min";
                column.Tag = "Min";
                baseDataGridViewIlluminator.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.Name = "Max";
                column.HeaderText = "Max";
                column.DataPropertyName = "Max";
                column.Tag = "Max";
                baseDataGridViewIlluminator.Columns.Add(column);
            }

        }

        public void UpdateGridConfigData()
        {
            if(baseDataGridViewIlluminator != null)
            {
                BindingSource bindingSource = new BindingSource();
                bindingSource.DataSource = listIlluminationChannel;
                baseDataGridViewIlluminator.DataSource = bindingSource;
            }
        }

         private void baseButtonAdd_Click(object sender, EventArgs e)
        {
            if(m_listIlluminationChannel != null)
            {
                IlluminationChannel channel = new IlluminationChannel("");
                channel.Channel = 0;
                m_listIlluminationChannel.Add(channel);
                UpdateGridConfigData();
            }
        }

        private void baseButtonRemove_Click(object sender, EventArgs e)
        {
            if (baseDataGridViewIlluminator.SelectedCells.Count > 0 && baseDataGridViewIlluminator != null )
            {
                int nIndex = baseDataGridViewIlluminator.SelectedCells[0].RowIndex;
                m_listIlluminationChannel.RemoveAt(nIndex);
                UpdateGridConfigData();
            }
            
        }

        private void baseButtonClear_Click(object sender, EventArgs e)
        {
            if(m_listIlluminationChannel != null)
            {
                m_listIlluminationChannel.Clear();
                UpdateGridConfigData();
            }
        }
    }
}
