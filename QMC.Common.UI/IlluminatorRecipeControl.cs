using QMC.Common;
using QMC.Common.Modules;
using QMC.Common.VisionPart;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QMC.Common.UI
{
    public partial class IlluminatorRecipeControl : UserControl
    {
        protected IlluminationDataSet m_DataSet;
        protected RealTimeScanner m_Owner;
        public IlluminationDataList m_ListIlluminators;
        public IlluminationDataList ListIlluminators
        {
            get
            {
                return m_ListIlluminators;
            }
            set
            {
                m_ListIlluminators = value;
            }
        }
        public IlluminatorRecipeControl(IlluminationDataList listIlluminators)
        {
            InitializeComponent();
            m_ListIlluminators = listIlluminators;
            InitIlluminatorGridColumns();
            UpdateIlluminatorGridColumns();
        }

        private void InitIlluminatorGridColumns()
        {
            if (m_ListIlluminators == null)
            {
                return;
            }
            baseDataGridViewIlluminatorRecipe.Columns.Clear();
            baseDataGridViewIlluminatorRecipe.AutoGenerateColumns = false;
            baseDataGridViewIlluminatorRecipe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.Name = "Name";
                column.HeaderText = "Name";
                baseDataGridViewIlluminatorRecipe.Columns.Add(column);
            }
            if (m_ListIlluminators.Count > 0)
            {
                for (int i = 0; i < m_ListIlluminators[0].Values.Count; i++)
                {
                    IlluminationChannel channel = m_ListIlluminators[0].GetAt(i);
                    DataGridViewColumn column = new DataGridViewTextBoxColumn();
                    column.Name = channel.ChannelName;
                    column.HeaderText = channel.ChannelName;
                    baseDataGridViewIlluminatorRecipe.Columns.Add(column);
                }
            }
        }
        private void UpdateIlluminatorGridColumns()
        {
            if (m_ListIlluminators == null)
            {
                return;
            }
            if (m_ListIlluminators.Count > 0)
            {
                baseDataGridViewIlluminatorRecipe.Rows.Clear();
                for (int i = 0; i < m_ListIlluminators.Count; i++)
                {
                    int index = baseDataGridViewIlluminatorRecipe.Rows.Add();
                    baseDataGridViewIlluminatorRecipe.Rows[index].Cells[0].Value = m_ListIlluminators[i].Name;

                    for (int value = 0; value < m_ListIlluminators[i].Values.Count; value++)
                    {
                        IlluminationChannel data = m_ListIlluminators[i].GetAt(value);
                        baseDataGridViewIlluminatorRecipe.Rows[index].Cells[value + 1].Value = data.Value;
                    }
                }

            }

        }

    }
}
