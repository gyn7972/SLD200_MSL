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
using QMC.Common.Modules;
using QMC.Common.Parts;

namespace QMC.Common.UI
{
    public delegate void PositionButtonClickHandler(ModulePositionControl.ButtonType type);
    public partial class ModulePositionControl : UserControl
    {
        public enum ButtonType
        {
            Save,
            Move,
        }
        public event PositionButtonClickHandler ButtonClick;

        public XytPositionDataCollection m_XytPositionData;
        public XyzPositionDataCollection m_XyzPositionData;
        public XyPositionDataCollection m_XyPositionData;
        public XyztPositionDataCollection m_XyztPositionData;
        public ZPositionDataCollection m_ZPositionData;
        public string SelectedPosition//컨트롤이 가지고있는 값 받아올때는 이렇게.......
        {
            get
            {
                string strResult = "";
                if (comboPosition.SelectedIndex >= 0)
                    strResult = comboPosition.Text;

                return strResult;
            }
        }
        public ModulePositionControl()
        {
            InitializeComponent();
        }

        public void SetGroupboxName(string Name)
        {
            GroupBox.Text = Name;
        }

        public void SetPositionList(XytPositionDataCollection listPosition)
        {
            comboPosition.DisplayMember = "Name";
            m_XytPositionData = listPosition;

            comboPosition.DataSource = listPosition.GetPositionList();            
            comboPosition.SelectedItem = null;
            UpdateDataGridViewColumns_XytPosition();
        }
        public void SetPositionList(XyzPositionDataCollection listPosition)
        {
            comboPosition.DisplayMember = "Name";
            m_XyzPositionData = listPosition;

            comboPosition.DataSource = m_XyzPositionData.GetPositionList();
            comboPosition.SelectedItem = null;
            UpdateDataGridViewColumns_XyzPosition();
        }
        public void SetPositionList(XyPositionDataCollection listPosition)
        {
            comboPosition.DisplayMember = "Name";
            m_XyPositionData = listPosition;
            comboPosition.SelectedItem = null;
            comboPosition.DataSource = m_XyPositionData.GetPositionList();

            UpdateDataGridViewColumns_XyPosition();
        }
        public void SetPositionList(XyztPositionDataCollection listPosition)
        {
            comboPosition.DisplayMember = "Name";
            m_XyztPositionData = listPosition;
            comboPosition.SelectedItem = null;
            comboPosition.DataSource = m_XyztPositionData.GetPositionList();

            UpdateDataGridViewColumns_XyztPosition();
        }
        public void SetPositionList(ZPositionDataCollection listPosition)
        {
            comboPosition.DisplayMember = "Name";
            m_ZPositionData = listPosition;
            comboPosition.SelectedItem = null;
            comboPosition.DataSource = m_ZPositionData.GetPositionList();

            UpdateDataGridViewColumns_ZPosition();
        }

        private void baseButtonMove_Click(object sender, EventArgs e)
        {
            if (ButtonClick != null)
            {
                ButtonClick(ButtonType.Move);
            }
        }

        private void baseButtonSave_Click(object sender, EventArgs e)
        {
            if (ButtonClick != null)
            {
                ButtonClick(ButtonType.Save);
                PositionInfoGrid.Invalidate();
            }
        }

        private void UpdateDataGridViewColumns_XytPosition()
        {
            PositionInfoGrid.Columns.Clear();
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Name";
                column.Name = "Name";
                column.HeaderText = "Name";
                PositionInfoGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Type";
                column.Name = "Type";
                column.HeaderText = "Type";
                PositionInfoGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "X";
                column.Name = "X";
                column.HeaderText = "X";
                PositionInfoGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Y";
                column.Name = "Y";
                column.HeaderText = "Y";
                PositionInfoGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "T";
                column.Name = "T";
                column.HeaderText = "T";
                PositionInfoGrid.Columns.Add(column);
            }            
        }
        private void UpdateDataGridViewColumns_XyzPosition()
        {
            PositionInfoGrid.Columns.Clear();
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Name";
                column.Name = "Name";
                PositionInfoGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Type";
                column.Name = "Type";
                PositionInfoGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "X";
                column.Name = "X";
                PositionInfoGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Y";
                column.Name = "Y";
                PositionInfoGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Z";
                column.Name = "Z";
                PositionInfoGrid.Columns.Add(column);
            }
        }
        private void UpdateDataGridViewColumns_XyPosition()
        {
            PositionInfoGrid.Columns.Clear();
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Name";
                column.Name = "Name";
                PositionInfoGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Type";
                column.Name = "Type";
                PositionInfoGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "X";
                column.Name = "X";
                PositionInfoGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Y";
                column.Name = "Y";
                PositionInfoGrid.Columns.Add(column);
            }
        }
        private void UpdateDataGridViewColumns_XyztPosition()
        {
            PositionInfoGrid.Columns.Clear();
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Name";
                column.Name = "Name";
                PositionInfoGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Type";
                column.Name = "Type";
                PositionInfoGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "X";
                column.Name = "X";
                PositionInfoGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Y";
                column.Name = "Y";
                PositionInfoGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Z";
                column.Name = "Z";
                PositionInfoGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "T";
                column.Name = "T";
                PositionInfoGrid.Columns.Add(column);
            }
        }
        private void UpdateDataGridViewColumns_ZPosition()
        {
            PositionInfoGrid.Columns.Clear();
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Name";
                column.Name = "Name";
                PositionInfoGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Type";
                column.Name = "Type";
                PositionInfoGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Z";
                column.Name = "Z";
                PositionInfoGrid.Columns.Add(column);
            }

        }

        private void comboPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            string data = SelectedPosition as string;
            if (m_XytPositionData != null)
            {
                PositionInfoGrid.DataSource = m_XytPositionData.GetPositionDatas(data);
            }
            else if (m_XyzPositionData != null)
            {
                PositionInfoGrid.DataSource = m_XyzPositionData.GetPositionDatas(data);
            }
            else if (m_XyPositionData != null)
            {
                PositionInfoGrid.DataSource = m_XyPositionData.GetPositionDatas(data);
            }
            else if (m_XyztPositionData != null)
            {
                PositionInfoGrid.DataSource = m_XyztPositionData.GetPositionDatas(data);
            }
            else if (m_ZPositionData != null)
            {
                PositionInfoGrid.DataSource = m_ZPositionData.GetPositionDatas(data);
            }
        }

    }
}
