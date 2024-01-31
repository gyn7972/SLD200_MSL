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
    public partial class LineInfoControl : UserControl
    {
        private FormAddMesData m_FormAddMesData = new FormAddMesData();
        public MesInfo m_MesInfo;
        private CommonModule m_Common;
        private string m_strModuleName;
        public LineInfoControl(Module module)
        {
            InitializeComponent();
            InitGridView();
            m_strModuleName = module.Name;
            if (m_strModuleName == "Common")
            {
                m_Common = module as CommonModule;
            }
            else if (m_strModuleName == "WireBonder")
            {
            }
        }
        private void baseButtonWPseg_Click(object sender, EventArgs e)
        {
            m_FormAddMesData.BringToFront();
            m_FormAddMesData.StartPosition = FormStartPosition.CenterScreen;
            if (m_FormAddMesData.ShowDialog() == DialogResult.OK)
            {
                //추가 했을때
                MesInfo info = m_FormAddMesData.GetMesInfo();
                m_MesInfo.WPSeq = info.WPSeq;
                m_MesInfo.EquipCD = info.EquipCD;
                m_MesInfo.Proc = info.Proc;
                m_MesInfo.Floor = info.Floor;
                m_MesInfo.Line = info.Line;
                UpdateGridView();
            }
        }
        public void SetMesInfo(MesInfo mesInfo)
        {
            m_MesInfo = mesInfo;
            UpdateGridView();
        }
        private void InitGridView()

        {
            baseDataGridViewLineInfo.Columns.Clear();
            baseDataGridViewLineInfo.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            baseDataGridViewLineInfo.ReadOnly = true;
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.Name = "Name";
                column.HeaderText = "Name";
                column.Width = 100;
                baseDataGridViewLineInfo.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.Name = "Value";
                column.HeaderText = "Value";
                column.Width = 100;
                baseDataGridViewLineInfo.Columns.Add(column);
            }

        }
        private void UpdateGridView()
        {
            //if (baseDataGridViewLineInfo.Rows.Count == 0)
            //{
            //    for (int i = 0; i < 5; i++)
            //    {
            //        baseDataGridViewLineInfo.Rows.Add();
            //    }
            //    this.baseDataGridViewLineInfo["Name", 0].Value = "WPSeq";
            //    this.baseDataGridViewLineInfo["Name", 1].Value = "Floor";
            //    this.baseDataGridViewLineInfo["Name", 2].Value = "Line";
            //    this.baseDataGridViewLineInfo["Name", 3].Value = "Proc";
            //    this.baseDataGridViewLineInfo["Name", 4].Value = "EquipCD";
            //}
            //if (m_MesInfo != null)
            //{
            //    this.baseDataGridViewLineInfo["Value", 0].Value = m_MesInfo.WPSeq;
            //    this.baseDataGridViewLineInfo["Value", 1].Value = m_MesInfo.Floor;
            //    this.baseDataGridViewLineInfo["Value", 2].Value = m_MesInfo.Line;
            //    this.baseDataGridViewLineInfo["Value", 3].Value = m_MesInfo.Proc;
            //    this.baseDataGridViewLineInfo["Value", 4].Value = m_MesInfo.EquipCD;
            //}

            //if (m_Common != null)
            //{
            //    //checkBoxUseMesInterlock.Checked = m_Common.Config.UseMesInterlock;
            //    //checkBoxUseMes.Checked = m_Common.Config.UseMes;
            //    //checkBoxUseBarcodeDB.Checked = m_Common.Config.UseBarcodeDB;
            //}
            //else if (m_Wirebonder != null)
            //{
            //    checkBoxUseMesInterlock.Checked = m_Wirebonder.Config.UseMesInterlock;
            //    checkBoxUseBarcodeDB.Checked = m_Wirebonder.Config.UseBarcodeDB;
            //    checkBoxUseMes.Checked = m_Wirebonder.Config.UseMes;
            //}
        }

        private void checkBoxUseMesInterlock_CheckedChanged(object sender, EventArgs e)
        {
            //if (m_Common != null)
            //{
            //    m_Common.Config.UseMesInterlock = checkBoxUseMesInterlock.Checked;
            //}
            //else if (m_Wirebonder != null)
            //{
            //    m_Wirebonder.Config.UseMesInterlock = checkBoxUseMesInterlock.Checked;
            //}
        }

        private void checkBoxUseMes_CheckedChanged(object sender, EventArgs e)
        {
            //if (m_Common != null)
            //{
            //    m_Common.Config.UseMes = checkBoxUseMes.Checked;
            //}
            //else if (m_Wirebonder != null)
            //{
            //    m_Wirebonder.Config.UseMes = checkBoxUseMes.Checked;
            //}
        }

        private void checkBoxUseBarcodeDB_CheckedChanged(object sender, EventArgs e)
        {
            //if (m_Common != null)
            //{
            //    m_Common.Config.UseBarcodeDB = checkBoxUseBarcodeDB.Checked;
            //}
            //else if (m_Wirebonder != null)
            //{
            //    m_Wirebonder.Config.UseBarcodeDB = checkBoxUseBarcodeDB.Checked;
            //}
        }
    }
}
