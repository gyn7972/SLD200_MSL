using QMC.Common.Modules;
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
    public partial class FormAddMesData : Form
    {
        private MesInfo m_info;
        private List<MesInfo> listMesInfo;
        private MesDataManager m_DataManager;

        public FormAddMesData()
        {
            InitializeComponent();
            //m_DataManager = new MesDataManager();
            //try
            //{
            //    if (CommonModule.Instance.Config.UseMes)
            //        m_DataManager.Open();
            //}
            //catch { }
            //baseDataGridViewMES.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //baseDataGridViewMES.ReadOnly = true;
        }

        private void baseButtonApply_Click(object sender, EventArgs e)
        {
            if (baseDataGridViewMES.Rows.Count == 0)
            {
                return;
            }

            int nRow = baseDataGridViewMES.SelectedCells[0].RowIndex;
            if (nRow >= 0)
            {
                if (listMesInfo != null)
                {
                    m_info = listMesInfo[nRow];
                }

                DialogResult = DialogResult.OK;
            }
            else
            {
                DialogResult = DialogResult.Cancel;
            }
        }

        private void baseButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void ListMesData_Load(object sender, EventArgs e)
        {
            baseDataGridViewMES.DataSource = null;
            //if (CommonModule.Instance.Config.UseMes)
            //{
            //    listMesInfo = m_DataManager.GetLineInfo();
            //}
            //if (listMesInfo != null)
            //{
            //    baseDataGridViewMES.DataSource = listMesInfo;
            //}
            //채워야함... 
        }
        public void SetMesInfo(MesInfo mesInfo)
        {
            m_info = mesInfo;
        }

        public MesInfo GetMesInfo()
        {
            return m_info;
        }
        protected override void OnClosed(EventArgs e)
        {
            m_DataManager.Close();
        }
    }
}
