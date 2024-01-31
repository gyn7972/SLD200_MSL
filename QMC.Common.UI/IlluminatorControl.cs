using QMC.Common;
using QMC.Common.Modules;
using QMC.Common.Vision.Optics;
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
    public delegate void IlluminatorControlEventHandler();
    public delegate void IlluminatorControlButtonEventHandler(IlluminatorControl.ButtonType type);
    public partial class IlluminatorControl : UserControl
    {
        public enum ButtonType
        {
            Save,
            AllOff,
        }
        public event IlluminatorControlEventHandler IlluminatorControl_ValueChange;
        public event IlluminatorControlButtonEventHandler IlluminatorControlButton_Click;


        public int MinValue { get; set; }
        public int MaxValue { get; set; }

        public Illuminator Illuminator { set; get; }
        public int Value { get; set; }
        public IlluminationDataSet m_CurrentDataSet;
        private IlluminationDataList m_ListIlluminators;
        private int m_nCurrentRow;
        private int m_nCurrentColum;
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

        public IlluminatorControl(IlluminationDataList listIlluminators)
        {
            InitializeComponent();
            SetIlluminationDataList(listIlluminators);
        }

        public void SetIlluminationDataList(IlluminationDataList listIlluminators)
        {
            m_ListIlluminators = listIlluminators;
            m_nCurrentRow = 0;
            if (m_ListIlluminators != null && m_ListIlluminators.Count > 0)
            {
                m_CurrentDataSet = m_ListIlluminators[m_nCurrentRow];
            }

            hScrollBarIlluminator.LargeChange = 1;
            InitIlluminatorGridColumns();
            UpdateIlluminatorGridColumns();
            UpdateIlluminatorComboBox();
            if (m_ListIlluminators != null)
            {
                int nIndex = comboBoxIllumanatorList.SelectedIndex;
                m_nCurrentColum = nIndex;
                SetScroll();
            }
            //SetScroll(0);
            IlluminatorGrid.CellEndEdit += IlluminatorGrid_CellEndEdit;
            hScrollBarIlluminator.ValueChanged += HScrollBarIlluminator_ValueChanged;
        }


        private void IlluminatorGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex > 0)
            {
                string strTemp = IlluminatorGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                m_ListIlluminators[e.RowIndex].Values[e.ColumnIndex - 1].Value = Convert.ToInt32(strTemp);
            }
            SetScroll();
        }

        private void InitIlluminatorGridColumns()
        {
            if (m_ListIlluminators == null)
            {
                return;
            }
            IlluminatorGrid.Columns.Clear();
            IlluminatorGrid.AutoGenerateColumns = false;
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.Name = "Name";
                column.HeaderText = "Name";
                IlluminatorGrid.Columns.Add(column);
            }
            if (m_ListIlluminators.Count > 0)
            {
                for (int i = 0; i < m_ListIlluminators[0].Values.Count; i++)
                {
                    IlluminationChannel channel = m_ListIlluminators[0].GetAt(i);
                    DataGridViewColumn column = new DataGridViewTextBoxColumn();
                    column.Name = channel.ChannelName;
                    column.HeaderText = channel.ChannelName;
                    IlluminatorGrid.Columns.Add(column);
                }
            }
        }

        private void UpdateIlluminatorGridColumns()
        {
            if (m_ListIlluminators == null)
            {
                return;
            }

            try
            {
                if (m_ListIlluminators.Count > 0)
                {
                    IlluminatorGrid.Rows.Clear();
                    for (int i = 0; i < m_ListIlluminators.Count; i++)
                    {
                        int index = IlluminatorGrid.Rows.Add();
                        IlluminatorGrid.Rows[index].Cells[0].Value = m_ListIlluminators[i].Name;

                        for (int value = 0; value < m_ListIlluminators[i].Values.Count; value++)
                        {
                            IlluminationChannel data = m_ListIlluminators[i].GetAt(value);
                            IlluminatorGrid.Rows[index].Cells[value + 1].Value = data.Value;
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
        }

        private void UpdateIlluminatorComboBox()
        {
            if (m_ListIlluminators != null && m_ListIlluminators.Count > 0)
            {
                comboBoxIllumanatorList.DisplayMember = "ChannelName";
                comboBoxIllumanatorList.DataSource = m_ListIlluminators[0].Values;
            }
        }

        private void baseButtonSave_Click(object sender, EventArgs e)
        {
            if (IlluminatorControlButton_Click != null)
            {
                IlluminatorControlButton_Click(ButtonType.Save);
            }
        }

        private void baseButtonAllOff_Click(object sender, EventArgs e)
        {
            if (Illuminator != null)
            {
                foreach (IlluminationChannel channel in m_CurrentDataSet.Values)
                {
                    Illuminator.TurnOnOff(false, channel.Channel);
                }
            }


            if (IlluminatorControlButton_Click != null)
            {
                IlluminatorControlButton_Click(ButtonType.AllOff);
            }
        }

        //private void hScrollBarIlluminator_Scroll(object sender, ScrollEventArgs e)
        //{
        //if (m_CurrentDataSet != null && m_nCurrentColum >= 0 && m_nCurrentColum < m_CurrentDataSet.Values.Count)
        //{
        //    this.baseLabel1Value.Text = this.hScrollBarIlluminator.Value.ToString();
        //    m_CurrentDataSet.Values[m_nCurrentColum].Value = this.hScrollBarIlluminator.Value;
        //    m_ListIlluminators[m_nCurrentRow] = m_CurrentDataSet;
        //    UpdateIlluminatorGridColumns();

        //    if (IlluminatorControl_ValueChange != null)
        //    {
        //        IlluminatorControl_ValueChange();

        //    }

        //    foreach(IlluminationChannel channel in m_CurrentDataSet.Values)
        //    {

        //    }

        //}
        //}

        private void HScrollBarIlluminator_ValueChanged(object sender, System.EventArgs e)
        {
            if (m_CurrentDataSet != null && m_nCurrentColum >= 0 && m_nCurrentColum < m_CurrentDataSet.Values.Count)
            {
                this.baseLabel1Value.Text = this.hScrollBarIlluminator.Value.ToString();
                m_CurrentDataSet.Values[m_nCurrentColum].Value = this.hScrollBarIlluminator.Value;
                m_ListIlluminators[m_nCurrentRow] = m_CurrentDataSet;
                UpdateIlluminatorGridColumns();

                if (IlluminatorControl_ValueChange != null)
                {
                    IlluminatorControl_ValueChange();

                }
                if (Illuminator != null)
                {
                    IlluminationChannel channel = m_CurrentDataSet.Values[m_nCurrentColum];
                    Illuminator.SetVolume(channel.Value, channel.Channel);
                }

            }
        }


        private void IlluminatorGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (m_ListIlluminators != null)
            {
                if (e.ColumnIndex >= 1)
                {
                    m_nCurrentColum = e.ColumnIndex - 1; //Name Colum 하나 빼고..
                    if (e.RowIndex >= 0)
                    {
                        m_nCurrentRow = e.RowIndex;
                        m_CurrentDataSet = m_ListIlluminators[m_nCurrentRow];
                        comboBoxIllumanatorList.SelectedIndex = m_nCurrentColum;
                        SetScroll();
                        IlluminationChannel channel = m_CurrentDataSet.Values[m_nCurrentColum];
                        if (Illuminator == null)
                        {
                            return;
                        }
                        Illuminator.TurnOnOff(true, channel.Channel);
                    }
                }
            }
        }

        private void comboBoxIllumanatorList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_ListIlluminators != null)
            {
                int nIndex = comboBoxIllumanatorList.SelectedIndex;
                m_nCurrentColum = nIndex;
                SetScroll();
            }
        }
        private void SetScroll()
        {
            if (m_CurrentDataSet != null && m_nCurrentColum >= 0 && m_nCurrentColum < m_CurrentDataSet.Values.Count)
            {
                MinValue = (int)m_CurrentDataSet.Values[m_nCurrentColum].Min;
                MaxValue = (int)m_CurrentDataSet.Values[m_nCurrentColum].Max;

                hScrollBarIlluminator.Minimum = (int)m_CurrentDataSet.Values[m_nCurrentColum].Min;
                hScrollBarIlluminator.Maximum = (int)m_CurrentDataSet.Values[m_nCurrentColum].Max;
                if (hScrollBarIlluminator.Minimum <= m_CurrentDataSet.Values[m_nCurrentColum].Value && m_CurrentDataSet.Values[m_nCurrentColum].Value <= hScrollBarIlluminator.Maximum)
                {
                    hScrollBarIlluminator.Value = m_CurrentDataSet.Values[m_nCurrentColum].Value;
                }
                else
                {
                    m_CurrentDataSet.Values[m_nCurrentColum].Value = (int)m_CurrentDataSet.Values[m_nCurrentColum].Max;
                    MessageBox.Show("Out Of Range");
                    SetScroll();
                    UpdateIlluminatorGridColumns();
                }
                baseLabelMin.Text = MinValue.ToString();
                baseLabelMax.Text = MaxValue.ToString();
                this.baseLabel1Value.Text = this.hScrollBarIlluminator.Value.ToString();
            }
        }

        public IlluminationDataSet GetCurrentValue()
        {
            return m_CurrentDataSet;
        }
    }
}
