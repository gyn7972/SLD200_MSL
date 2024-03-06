using QMC.Common;
using QMC.Common.Modules;
using QMC.Common.Vision.Optics;
using QMC.Core;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CWA150SA_Onsemi300
{
    #region Define
    public delegate void IlluminatorControlEventHandler();
    public delegate void IlluminatorControlButtonEventHandler(IlluminatorControl.ButtonType type);
    #endregion

    #region IlluminatorControl
    public partial class IlluminatorControl : UserControl
    {
        #region Define
        public enum ButtonType
        {
            Save,
            AllOff,
        }
        public event IlluminatorControlEventHandler IlluminatorControl_ValueChange;
        public event IlluminatorControlButtonEventHandler IlluminatorControlButton_Click;
        #endregion

        protected WaferProbeAlign waferProbeAlign;

        #region Field
        public IlluminationDataSet m_CurrentDataSet;
        private IlluminationDataList m_ListIlluminators;
        private int m_nCurrentRow;
        private int m_nCurrentColum;
        #endregion

        #region Property
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public Illuminator Illuminator { set; get; }
        public int Value { get; set; }

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
        #endregion

        #region Constructor
        public IlluminatorControl(IlluminationDataList listIlluminators)
        { 
            InitializeComponent();
            m_ListIlluminators = listIlluminators;
            m_nCurrentRow = 0;
			if(m_ListIlluminators != null && m_ListIlluminators.Count > 0)
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


            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;

            foreach (Module module in m_collectionModules)
            {
                if (module.Name == "WaferProbeAlign")
                {
                    waferProbeAlign = module as WaferProbeAlign;
                }
            }
        }
        #endregion

        #region Event Handler
        private void IlluminatorGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex > 0)
            {
                string strTemp = IlluminatorGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                m_ListIlluminators[e.RowIndex].Values[e.ColumnIndex - 1].Value = Convert.ToInt32(strTemp);
            }
            SetScroll();
        }
        
        private void baseButtonSave_Click(object sender, EventArgs e)
        {
            if(IlluminatorControlButton_Click != null )
            {
                IlluminatorControlButton_Click(ButtonType.Save);

                MessageBoxYesNo mb = new MessageBoxYesNo();

                if (mb.ShowDialog("얼라인 조명값", "현재 조명값을 얼라인 조명값으로 사용하시겠습니까?") == DialogResult.Yes)
                {
                    foreach (IlluminationChannel channel in m_CurrentDataSet.Values)
                    {
                        if (channel.ChannelName == "Lower")
                        {
                            waferProbeAlign.Config.ParamConfig.Align_LowerVision_LightValue = channel.Value;
                        }
                        else if (channel.ChannelName == "Upper")
                        {
                            waferProbeAlign.Config.ParamConfig.Align_UpperVision_LightValue = channel.Value;
                        }
                    }

                    string m_strRecipe = "";
                    RecipeInfo m_recipeInfo = new RecipeInfo();
                    m_recipeInfo = Equipment.GetCurrentRecipe();

                    if (m_recipeInfo != null)
                    {
                        m_strRecipe = m_recipeInfo.Name;

                        DataManager.Instance.UpdateConfigData(waferProbeAlign); // 참고 : param save
                        //Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                        Equipment.SaveConfig(m_strRecipe);                                            //  2022. 06. 30.  SCH : Recipe 에 따라 Config 파라미터를 변경하기 위해 이걸로 함.
                    }
                    else
                    {
                        DataManager.Instance.UpdateConfigData(waferProbeAlign); // 참고 : param save
                        Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                    }

                    DataManager.Instance.ApplyConfigData(waferProbeAlign);

                    //  Config 창 데이터 갱신을 위해서
                    Equipment.m_bRedraw_FormWaferProbeAlignParameterConfig = true;
                }
            }
        }

        private void baseButtonAllOff_Click(object sender, EventArgs e)
        {
            if(Illuminator != null)
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

        public void Illuminator_ValueChanged_Call()
        {
            if (IlluminatorControl_ValueChange != null)
            {
                IlluminatorControl_ValueChange();
            }
        }

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
            if(m_ListIlluminators != null)
            {
                if (e.ColumnIndex >= 1)
                {
                    m_nCurrentColum = e.ColumnIndex - 1; //Name Colum 하나 빼고..
                    if(e.RowIndex >= 0)
                    {
                        m_nCurrentRow = e.RowIndex;
                        m_CurrentDataSet = m_ListIlluminators[m_nCurrentRow];
                        comboBoxIllumanatorList.SelectedIndex = m_nCurrentColum;
                        SetScroll();
                        IlluminationChannel channel = m_CurrentDataSet.Values[m_nCurrentColum];
                        if(Illuminator ==null )
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
            if(m_ListIlluminators != null)
            { 
                int nIndex = comboBoxIllumanatorList.SelectedIndex;
                m_nCurrentColum = nIndex;
                SetScroll();
            }
        }
        #endregion

        #region Method


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

        public void SetIlluminatorGridColumns(int nLow, int nHigh, int nRed)                    //  2023. 09. 07.  SCH : 조명 컨트롤러의 값이 정상적으로 표시되지 않아서 강제로 넣어줌.
        {
            if (m_ListIlluminators == null)
            {
                return;
            }

            if (m_ListIlluminators.Count >= 1)
            {

                if (m_ListIlluminators[0].Values.Count >= 1)
                {
                    if (m_ListIlluminators[0].Values[0].ChannelName == "Low")
                    {
                        m_ListIlluminators[0].Values[0].Value = nLow;
                    }
                    else if (m_ListIlluminators[0].Values[0].ChannelName == "High")
                    {
                        m_ListIlluminators[0].Values[0].Value = nHigh;
                    }
                    else if (m_ListIlluminators[0].Values[0].ChannelName == "Red")
                    {
                        m_ListIlluminators[0].Values[0].Value = nRed;
                    }
                }

                if (m_ListIlluminators[0].Values.Count >= 2)
                {
                    if (m_ListIlluminators[0].Values[1].ChannelName == "Low")
                    {
                        m_ListIlluminators[0].Values[1].Value = nLow;
                    }
                    else if (m_ListIlluminators[0].Values[1].ChannelName == "High")
                    {
                        m_ListIlluminators[0].Values[1].Value = nHigh;
                    }
                    else if (m_ListIlluminators[0].Values[1].ChannelName == "Red")
                    {
                        m_ListIlluminators[0].Values[1].Value = nRed;
                    }
                }

                if (m_ListIlluminators[0].Values.Count >= 3)
                {
                    if (m_ListIlluminators[0].Values[2].ChannelName == "Low")
                    {
                        m_ListIlluminators[0].Values[2].Value = nLow;
                    }
                    else if (m_ListIlluminators[0].Values[2].ChannelName == "High")
                    {
                        m_ListIlluminators[0].Values[2].Value = nHigh;
                    }
                    else if (m_ListIlluminators[0].Values[2].ChannelName == "Red")
                    {
                        m_ListIlluminators[0].Values[2].Value = nRed;
                    }
                }

                UpdateIlluminatorGridColumns();
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

        private void SetScroll()
        {
            if (m_CurrentDataSet != null && m_nCurrentColum >= 0 && m_nCurrentColum < m_CurrentDataSet.Values.Count)
            {
                MinValue = (int)m_CurrentDataSet.Values[m_nCurrentColum].Min;
                MaxValue = (int)m_CurrentDataSet.Values[m_nCurrentColum].Max;

                hScrollBarIlluminator.Minimum = (int)m_CurrentDataSet.Values[m_nCurrentColum].Min;
                hScrollBarIlluminator.Maximum = (int)m_CurrentDataSet.Values[m_nCurrentColum].Max;
                if(hScrollBarIlluminator.Minimum <= m_CurrentDataSet.Values[m_nCurrentColum].Value && m_CurrentDataSet.Values[m_nCurrentColum].Value <= hScrollBarIlluminator.Maximum)
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
        #endregion

        private void IlluminatorControl_VisibleChanged(object sender, EventArgs e)
        {
            //  조명 컨트롤러의 변경된 값이, 모든 조명 컨트롤러에 적용되게 하기 위해서 추가됨.

            m_CurrentDataSet.Values[0].Value = waferProbeAlign.Config.ParamConfig.Align_UpperVision_LightValue;
            m_CurrentDataSet.Values[1].Value = waferProbeAlign.Config.ParamConfig.Align_LowerVision_LightValue;

            SetScroll();
            UpdateIlluminatorGridColumns();

            if (Illuminator != null)
            {
                Illuminator.SetVolume(0, m_CurrentDataSet.Values[0].Value);
                Illuminator.SetVolume(1, m_CurrentDataSet.Values[1].Value);
            }
        }
    }
    #endregion
}
