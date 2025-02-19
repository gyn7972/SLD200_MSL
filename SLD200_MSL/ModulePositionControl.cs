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

namespace SLD200_MSL
{
    public delegate void PositionButtonClickHandler(ModulePositionControl.ButtonType type);
    public partial class ModulePositionControl : UserControl
    {
        #region Define
        public enum ButtonType
        {
            Save,
            Move,
        }
        #endregion

        #region Field
        public PositionButtonClickHandler ButtonClick;

        public XytPositionDataCollection m_XytPositionData;
        public XyzPositionDataCollection m_XyzPositionData;

        public List<XyzPositionData> m_ListXyzPosition;
        public List<XyztPositionData> m_ListXyztPosition;
        public List<XyzztPositionData> m_ListXyzztPosition;
        public List<UvwzxyzPositionData> m_ListUvwzxyzPosition;
        public List<XyzLDzzxzULzzxzPositionData> m_ListXyzLDzzxzULzzxzPosition;
        public List<XyzyPositionData> m_ListXyzyPosition;
        public List<ZzxzxyPositionData> m_ListZzxzxyPosition;
        public List<ZzxzPositionData> m_ListZzxzPosition;
        public List<YPositionData> m_ListYPosition;

        public XyPositionDataCollection m_XyPositionData;
        public XyztPositionDataCollection m_XyztPositionData;
        public XyzztPositionDataCollection m_XyzztPositionData;
        public XryztPositionDataCollection m_XryztPositionData;
        public UvwzxyzPositionDataCollection m_UvwzxyzPositionData;
        public XyzLDzzxzULzzxzPositionDataCollection m_XyzLDzzxzULzzxzPositionData;
        public XyzyPositionDataCollection m_XyzyPositionData;
        public ZzxzxyPositionDataCollection m_ZzxzxyPositionData;
        public ZzxzPositionDataCollection m_ZzxzPositionData;
        public ZPositionDataCollection m_ZPositionData;
        public YPositionDataCollection m_YPositionData;

        static WorkStage workStage;
        static Loader loader;

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
        #endregion

        #region Constructor
        public ModulePositionControl()
        {
            InitializeComponent();

            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;

            foreach (Module module1 in m_collectionModules)
            {
                //if (module1.Name == "WorkStage")
                if (module1.Name == "WorkStage")
                {
                    workStage = module1 as WorkStage;
                }

                if (module1.Name == "Loader")
                {
                    loader = module1 as Loader;
                }
            }
        }
        #endregion

        #region Method


        public void SetGroupboxName(string Name)
        {
            GroupBox.Text = Name;
        }

        public void SetPositionList(XyzLDzzxzULzzxzPositionDataCollection listPosition)
        {
            comboPosition.DisplayMember = "Name";
            m_XyzLDzzxzULzzxzPositionData = listPosition;

            comboPosition.DataSource = listPosition.GetPositionList();
            comboPosition.SelectedItem = null;
            UpdateDataGridViewColumns_XyzLDzzxzULzzxzPosition();
        }

        public void SetPositionList(XyzyPositionDataCollection listPosition)
        {
            comboPosition.DisplayMember = "Name";
            m_XyzyPositionData = listPosition;

            comboPosition.DataSource = listPosition.GetPositionList();
            comboPosition.SelectedItem = null;
            UpdateDataGridViewColumns_XyzyPosition();
        }

        public void SetPositionList(ZzxzxyPositionDataCollection listPosition)
        {
            comboPosition.DisplayMember = "Name";
            m_ZzxzxyPositionData = listPosition;

            comboPosition.DataSource = listPosition.GetPositionList();
            comboPosition.SelectedItem = null;
            UpdateDataGridViewColumns_ZzxzxyPosition();
        }
        
        public void SetPositionList(ZzxzPositionDataCollection listPosition)
        {
            comboPosition.DisplayMember = "Name";
            m_ZzxzPositionData = listPosition;

            comboPosition.DataSource = listPosition.GetPositionList();
            comboPosition.SelectedItem = null;
            UpdateDataGridViewColumns_ZzxzPosition();
        }

        public void SetPositionList(UvwzxyzPositionDataCollection listPosition)
        {
            comboPosition.DisplayMember = "Name";
            m_UvwzxyzPositionData = listPosition;

            comboPosition.DataSource = listPosition.GetPositionList();
            comboPosition.SelectedItem = null;
            UpdateDataGridViewColumns_UvwzxyzPosition();
        }
                
        public void SetPositionList(XyzztPositionDataCollection listPosition)
        {
            comboPosition.DisplayMember = "Name";
            m_XyzztPositionData = listPosition;

            comboPosition.DataSource = listPosition.GetPositionList();
            comboPosition.SelectedItem = null;
            UpdateDataGridViewColumns_XyzztPosition();
        }

        public void SetPositionList(XyztPositionDataCollection listPosition)
        {
            comboPosition.DisplayMember = "Name";
            m_XyztPositionData = listPosition;

            comboPosition.DataSource = listPosition.GetPositionList();
            comboPosition.SelectedItem = null;
            UpdateDataGridViewColumns_XyztPosition();
        }

        public void SetPositionList(XyzPositionDataCollection listPosition)
        {
            comboPosition.DisplayMember = "Name";
            m_XyzPositionData = listPosition;

            comboPosition.DataSource = m_XyzPositionData.GetPositionList();
            comboPosition.SelectedItem = null;
            UpdateDataGridViewColumns_XyzPosition();
        }

        public void SetPositionList(YPositionDataCollection listPosition)
        {
            comboPosition.DisplayMember = "Name";
            m_YPositionData = listPosition;

            comboPosition.DataSource = m_YPositionData.GetPositionList();
            comboPosition.SelectedItem = null;
            UpdateDataGridViewColumns_YPosition();
        }

        public void SetPositionList(List<XyzztPositionData> listPosition)
        {
            comboPosition.DisplayMember = "Name";
            m_ListXyzztPosition = listPosition;

            List<string> list = new List<string>();
            foreach (XyzztPositionData data in listPosition)
            {
                if (data.Type == TargetType.Base)
                {
                    list.Add(data.Name);
                }
            }

            comboPosition.DataSource = list;
            //comboPosition.DataSource = m_ListXyzPosition;
            comboPosition.SelectedItem = null;
            UpdateDataGridViewColumns_XyzztPosition();
        }

        public void SetPositionList(List<UvwzxyzPositionData> listPosition)
        {
            comboPosition.DisplayMember = "Name";
            m_ListUvwzxyzPosition = listPosition;

            List<string> list = new List<string>();
            foreach (UvwzxyzPositionData data in listPosition)
            {
                if (data.Type == TargetType.Base)
                {
                    list.Add(data.Name);
                }
            }

            comboPosition.DataSource = list;
            //comboPosition.DataSource = m_ListXyzPosition;
            comboPosition.SelectedItem = null;
            UpdateDataGridViewColumns_UvwzxyzPosition();
        }

        public void SetPositionList(List<XyzLDzzxzULzzxzPositionData> listPosition)
        {
            comboPosition.DisplayMember = "Name";
            m_ListXyzLDzzxzULzzxzPosition = listPosition;

            List<string> list = new List<string>();
            foreach (XyzLDzzxzULzzxzPositionData data in listPosition)
            {
                if (data.Type == TargetType.Base)
                {
                    list.Add(data.Name);
                }
            }

            comboPosition.DataSource = list;
            //comboPosition.DataSource = m_ListXyzPosition;
            comboPosition.SelectedItem = null;
            UpdateDataGridViewColumns_XyzLDzzxzULzzxzPosition();
        }

        public void SetPositionList(List<XyzyPositionData> listPosition)
        {
            comboPosition.DisplayMember = "Name";
            m_ListXyzyPosition = listPosition;

            List<string> list = new List<string>();
            foreach (XyzyPositionData data in listPosition)
            {
                if (data.Type == TargetType.Base)
                {
                    list.Add(data.Name);
                }
            }

            comboPosition.DataSource = list;
            //comboPosition.DataSource = m_ListXyzPosition;
            comboPosition.SelectedItem = null;
            UpdateDataGridViewColumns_XyzyPosition();            
        }

        public void SetPositionList(List<ZzxzxyPositionData> listPosition)
        {
            comboPosition.DisplayMember = "Name";
            m_ListZzxzxyPosition = listPosition;

            List<string> list = new List<string>();
            foreach (ZzxzxyPositionData data in listPosition)
            {
                if (data.Type == TargetType.Base)
                {
                    list.Add(data.Name);
                }
            }

            comboPosition.DataSource = list;
            //comboPosition.DataSource = m_ListXyzPosition;
            comboPosition.SelectedItem = null;
            UpdateDataGridViewColumns_ZzxzxyPosition();
        }

        public void SetPositionList(List<ZzxzPositionData> listPosition)
        {
            comboPosition.DisplayMember = "Name";
            m_ListZzxzPosition = listPosition;

            List<string> list = new List<string>();
            foreach (ZzxzPositionData data in listPosition)
            {
                if (data.Type == TargetType.Base)
                {
                    list.Add(data.Name);
                }
            }

            comboPosition.DataSource = list;
            //comboPosition.DataSource = m_ListXyzPosition;
            comboPosition.SelectedItem = null;
            UpdateDataGridViewColumns_ZzxzPosition();
        }

        public void SetPositionList(List<XyztPositionData> listPosition)
        {
            comboPosition.DisplayMember = "Name";
            m_ListXyztPosition = listPosition;

            List<string> list = new List<string>();
            foreach (XyztPositionData data in listPosition)
            {
                if (data.Type == TargetType.Base)
                {
                    list.Add(data.Name);
                }
            }

            comboPosition.DataSource = list;
            //comboPosition.DataSource = m_ListXyzPosition;
            comboPosition.SelectedItem = null;
            UpdateDataGridViewColumns_XyztPosition();
        }

        public void SetPositionList(List<XyzPositionData> listPosition)
        {
            comboPosition.DisplayMember = "Name";
            m_ListXyzPosition = listPosition;

            List<string> list = new List<string>();
            foreach (XyzPositionData data in listPosition)
            {
                if (data.Type == TargetType.Base)
                {
                    list.Add(data.Name);
                }
            }

            comboPosition.DataSource = list;
            //comboPosition.DataSource = m_ListXyzPosition;
            comboPosition.SelectedItem = null;
            UpdateDataGridViewColumns_XyzPosition();
        }

        public void SetPositionList(List<YPositionData> listPosition)
        {
            comboPosition.DisplayMember = "Name";
            m_ListYPosition = listPosition;

            List<string> list = new List<string>();
            foreach (YPositionData data in listPosition)
            {
                if (data.Type == TargetType.Base)
                {
                    list.Add(data.Name);
                }
            }

            comboPosition.DataSource = list;
            //comboPosition.DataSource = m_ListXyzPosition;
            comboPosition.SelectedItem = null;
            UpdateDataGridViewColumns_YPosition();
        }

        public void SetPositionList(XyPositionDataCollection listPosition)
        {
            comboPosition.DisplayMember = "Name";
            m_XyPositionData = listPosition;
            comboPosition.SelectedItem = null;
            comboPosition.DataSource = m_XyPositionData.GetPositionList();

            UpdateDataGridViewColumns_XyPosition();
        }
        //public void SetPositionList(XyztPositionDataCollection listPosition)
        //{
        //    comboPosition.DisplayMember = "Name";
        //    m_XyztPositionData = listPosition;
        //    comboPosition.SelectedItem = null;
        //    comboPosition.DataSource = m_XyztPositionData.GetPositionList();

        //    UpdateDataGridViewColumns_XyztPosition();
        //}
        public void SetPositionList(XryztPositionDataCollection listPosition)
        {
            comboPosition.DisplayMember = "Name";
            m_XryztPositionData = listPosition;
            comboPosition.SelectedItem = null;
            comboPosition.DataSource = m_XryztPositionData.GetPositionList();

            UpdateDataGridViewColumns_XryztPosition();
        }
        public void SetPositionList(ZPositionDataCollection listPosition)
        {
            comboPosition.DisplayMember = "Name";
            m_ZPositionData = listPosition;
            comboPosition.SelectedItem = null;
            comboPosition.DataSource = m_ZPositionData.GetPositionList();

            UpdateDataGridViewColumns_ZPosition();
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

            //  첫번째 Column 의 크기만 크게 변경하기 위해 추가됨
            DataGridViewColumn column0 = PositionInfoGrid.Columns[0];
            column0.Width = 120;
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

            //  첫번째 Column 의 크기만 크게 변경하기 위해 추가됨
            //DataGridViewColumn column0 = PositionInfoGrid.Columns[0];
            //column0.Width = 120;
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

            //  첫번째 Column 의 크기만 크게 변경하기 위해 추가됨
            DataGridViewColumn column0 = PositionInfoGrid.Columns[0];
            column0.Width = 120;
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

            //  첫번째 Column 의 크기만 크게 변경하기 위해 추가됨
            DataGridViewColumn column0 = PositionInfoGrid.Columns[0];
            column0.Width = 120;
        }

        private void UpdateDataGridViewColumns_YPosition()
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
                column.DataPropertyName = "MASK_Y";
                column.Name = "MASK_Y";
                PositionInfoGrid.Columns.Add(column);
            }

            //  첫번째 Column 의 크기만 크게 변경하기 위해 추가됨
            DataGridViewColumn column0 = PositionInfoGrid.Columns[0];
            column0.Width = 120;
        }

        private void UpdateDataGridViewColumns_XyzztPosition()
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
                column.DataPropertyName = "IZ";
                column.Name = "IZ";
                PositionInfoGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "SZ";
                column.Name = "SZ";
                PositionInfoGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "T";
                column.Name = "T";
                PositionInfoGrid.Columns.Add(column);
            }

            //  첫번째 Column 의 크기만 크게 변경하기 위해 추가됨
            DataGridViewColumn column0 = PositionInfoGrid.Columns[0];
            column0.Width = 120;
        }

        private void UpdateDataGridViewColumns_UvwzxyzPosition()
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
                column.DataPropertyName = "U";
                column.Name = "U";
                PositionInfoGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "V";
                column.Name = "V";
                PositionInfoGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "W";
                column.Name = "W";
                PositionInfoGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "EZ";
                column.Name = "EZ";
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
                column.DataPropertyName = "VZ";
                column.Name = "VZ";
                PositionInfoGrid.Columns.Add(column);
            }

            //  첫번째 Column 의 크기만 크게 변경하기 위해 추가됨
            DataGridViewColumn column0 = PositionInfoGrid.Columns[0];
            column0.Width = 120;
        }

        private void UpdateDataGridViewColumns_XyzLDzzxzULzzxzPosition()
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
                column.DataPropertyName = "LD_SZ0";
                column.Name = "LD_SZ0";
                PositionInfoGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "LD_SZ1";
                column.Name = "LD_SZ1";
                PositionInfoGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "LD_TRX";
                column.Name = "LD_TRX";
                PositionInfoGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "LD_TRZ";
                column.Name = "LD_TRZ";
                PositionInfoGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "UL_SZ0";
                column.Name = "UL_SZ0";
                PositionInfoGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "UL_SZ1";
                column.Name = "UL_SZ1";
                PositionInfoGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "UL_TRX";
                column.Name = "UL_TRX";
                PositionInfoGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "UL_TRZ";
                column.Name = "UL_TRZ";
                PositionInfoGrid.Columns.Add(column);
            }

            //  첫번째 Column 의 크기만 크게 변경하기 위해 추가됨
            DataGridViewColumn column0 = PositionInfoGrid.Columns[0];
            column0.Width = 120;
        }

        private void UpdateDataGridViewColumns_XyzyPosition()
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
                column.DataPropertyName = "MASK_Y";
                column.Name = "MASK_Y";
                PositionInfoGrid.Columns.Add(column);
            }

            //  첫번째 Column 의 크기만 크게 변경하기 위해 추가됨
            DataGridViewColumn column0 = PositionInfoGrid.Columns[0];
            column0.Width = 120;
        }

        private void UpdateDataGridViewColumns_ZzxzxyPosition()
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
                column.DataPropertyName = "Z0";
                column.Name = "Z0";
                PositionInfoGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Z1";
                column.Name = "Z1";
                PositionInfoGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "TR_X";
                column.Name = "TR_X";
                PositionInfoGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "TR_Z";
                column.Name = "TR_Z";
                PositionInfoGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "ALN_X";
                column.Name = "ALN_X";
                PositionInfoGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "ALN_Y";
                column.Name = "ALN_Y";
                PositionInfoGrid.Columns.Add(column);
            }

            //  첫번째 Column 의 크기만 크게 변경하기 위해 추가됨
            DataGridViewColumn column0 = PositionInfoGrid.Columns[0];
            column0.Width = 120;
        }

        private void UpdateDataGridViewColumns_ZzxzPosition()
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
                column.DataPropertyName = "Z0";
                column.Name = "Z0";
                PositionInfoGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Z1";
                column.Name = "Z1";
                PositionInfoGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "TR_X";
                column.Name = "TR_X";
                PositionInfoGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "TR_Z";
                column.Name = "TR_Z";
                PositionInfoGrid.Columns.Add(column);
            }

            //  첫번째 Column 의 크기만 크게 변경하기 위해 추가됨
            DataGridViewColumn column0 = PositionInfoGrid.Columns[0];
            column0.Width = 120;
        }

        private void UpdateDataGridViewColumns_XryztPosition()
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
                column.DataPropertyName = "R";
                column.Name = "R";
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

            //  첫번째 Column 의 크기만 크게 변경하기 위해 추가됨
            DataGridViewColumn column0 = PositionInfoGrid.Columns[0];
            column0.Width = 120;
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

            //  첫번째 Column 의 크기만 크게 변경하기 위해 추가됨
            DataGridViewColumn column0 = PositionInfoGrid.Columns[0];
            column0.Width = 120;

        }
        #endregion

        #region Event Handler
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
        private void comboPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string data = comboPosition.SelectedItem as string;

            if (m_XytPositionData != null)
            {
                PositionInfoGrid.DataSource = m_XytPositionData.GetPositionDatas(SelectedPosition);
            }
            else if (m_XyzPositionData != null)
            {
                PositionInfoGrid.DataSource = m_XyzPositionData.GetPositionDatas(SelectedPosition);
            }
            else if (m_XyPositionData != null)
            {
                PositionInfoGrid.DataSource = m_XyPositionData.GetPositionDatas(SelectedPosition);
            }
            else if (m_XyztPositionData != null)
            {
                PositionInfoGrid.DataSource = m_XyztPositionData.GetPositionDatas(SelectedPosition);
            }
            else if (m_ZPositionData != null)
            {
                PositionInfoGrid.DataSource = m_ZPositionData.GetPositionDatas(SelectedPosition);
            }
            else if (m_XryztPositionData != null)
            {
                PositionInfoGrid.DataSource = m_XryztPositionData.GetPositionDatas(SelectedPosition);
            }
            else if (m_ListXyzPosition != null)
            {
                List<XyzPositionData> list = new List<XyzPositionData>();
                foreach (XyzPositionData data in m_ListXyzPosition)
                {
                    if (data.Name == SelectedPosition)
                    {
                        list.Add(data);
                    }
                }

                PositionInfoGrid.DataSource = list;
            }
        }
        #endregion
    }
}
