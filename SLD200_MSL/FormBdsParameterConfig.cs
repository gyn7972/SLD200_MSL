using System;
using System.IO;
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
using QMC.Common.Modules;
using QMC.Process.WorkStage.Parts;
using Timer = System.Windows.Forms.Timer;
using Point = System.Drawing.Point;
using System.Reflection;
using Module = QMC.Common.Module;
using System.Threading;

namespace SLD200_MSL
{
    //  2022. 03. 25.  SCH : Configuration --> Module --> Bds 가 여기로 오는군... 
    public partial class FormBdsParameterConfig : FormSubContentBase
    {
        //private LoaderParameter m_Owner;
        Module m_Module { get; set; }

        private bool m_bCommonParam_Save { get; set; }
        private int m_nCommonParam_Save_Count { get; set; }

        //  for Parameter Load
        public Timer timer_ParamLoad;

        public FormBdsParameterConfig(Module module)
            : base(FormType.Content.ToString(), module.Name)
        {
            InitializeComponent();
            //m_Owner = part as WorkStageParameter;
            m_Module = module;

            this.BackColor = Configuration.PanelBackColor;

            this.panelContent.Location = new Point(Configuration.ContentLocation.X, Configuration.PanelbuttonSize.Height);
                        
            this.buttonLoad.Size = Configuration.ButtonSize;
            this.buttonLoad.Location = new Point(Configuration.PanelSize.Width - Configuration.ControlsLocation.Width - Configuration.ButtonSize.Width * 2 - Configuration.ContentLocation.X, 3);
            this.buttonSave.Size = Configuration.ButtonSize;
            this.buttonSave.Location = new Point(Configuration.PanelSize.Width - Configuration.ButtonSize.Width - Configuration.ControlsLocation.Width, 3);

            this.BdsPositionGrid.Size = new Size((Configuration.MainSize.Width / 2) - 40, Configuration.ContentSize.Height - Configuration.PanelSize.Height * 7 - 172);
            this.BdsPositionGrid.Location = new Point(Configuration.ContentLocation.X, Configuration.ContentLocation.Y + Configuration.PanelSize.Height * 1 + Configuration.ButtonSize.Height);

            this.BdsPositionPropertyGrid.Size = new Size((Configuration.MainSize.Width / 2) - 40, 220);
            this.BdsPositionPropertyGrid.Location = new Point(Configuration.ContentLocation.X, BdsPositionGrid.Location.Y + BdsPositionGrid.Size.Height + 10);

            this.BdsParameterGrid.Size = new Size((Configuration.MainSize.Width / 2) - 10, Configuration.ContentSize.Height - Configuration.PanelSize.Height * 7 + 58);
            this.BdsParameterGrid.Location = new Point(BdsPositionPropertyGrid.Location.X + BdsPositionPropertyGrid.Size.Width + 20, Configuration.ContentLocation.Y + Configuration.PanelSize.Height * 1 + Configuration.ButtonSize.Height);

            SetLabelColumnWidth(BdsParameterGrid, 10);

            //this.baseLabelModuleConfiguration.Size = new Size(this.LaserPositionGrid.Width + LaserPositionPropertyGrid.Size.Width, Configuration.ButtonSize.Height);
            this.baseLabelModuleConfiguration.Size = new Size(this.BdsPositionGrid.Width, Configuration.ButtonSize.Height);
            this.baseLabelModuleConfiguration.Location = new Point(this.BdsPositionGrid.Location.X, this.flowLayoutPanelButton.Location.Y + this.flowLayoutPanelButton.Height);
            this.baseLabelModuleConfiguration.Text = "Bds Position";
            this.baseLabelModuleConfiguration.ForeColor = Color.Black;
            this.baseLabelModuleConfiguration.TextAlign = ContentAlignment.MiddleCenter;

            //this.baseLabelModuleParameter.Size = new Size(this.LaserParameterGrid.Width, Configuration.ButtonSize.Height);
            this.baseLabelModuleParameter.Size = new Size((int)((double)this.BdsParameterGrid.Width * (2.0 / 4.0)), Configuration.ButtonSize.Height);
            this.baseLabelModuleParameter.Location = new Point(this.BdsParameterGrid.Location.X + 20, this.flowLayoutPanelButton.Location.Y + this.flowLayoutPanelButton.Height);
            this.baseLabelModuleParameter.Text = "General";
            this.baseLabelModuleParameter.ForeColor = Color.Black;
            this.baseLabelModuleParameter.TextAlign = ContentAlignment.MiddleRight;

            this.buttonCommonParam_Save.Size = new Size(Configuration.ButtonSize.Width + 30, Configuration.ButtonSize.Height - 5);
            this.buttonCommonParam_Save.Location = new Point(baseLabelModuleParameter.Location.X + baseLabelModuleParameter.Size.Width + 264, baseLabelModuleParameter.Location.Y);

            BdsPositionGrid.SelectionChanged += BdsPositionGrid_SelectionChanged;

            this.panelContent.Hide();

            this.Controls.Add(this.BdsPositionGrid);
            this.Controls.Add(this.BdsPositionPropertyGrid);
            this.Controls.Add(this.BdsParameterGrid);
            this.Controls.Add(this.baseLabelModuleConfiguration);
            this.Controls.Add(this.baseLabelModuleParameter);
            //List<StageLoader> list = new List<StageLoader>();
            //SetGridConfigData(module);
          
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonCommonParam_Save);

            UpdateDataGridViewBdsPosition();
            SetGridConfigData(module);

            //  Parameter Load 하는 타이머
            timer_ParamLoad = new Timer();
            timer_ParamLoad.Interval = 100;
            timer_ParamLoad.Tick += new System.EventHandler(Timer_ParamLoadFunc);
            timer_ParamLoad.Enabled = true;


            //this.Load += FormDispenserParameterConfig_Load;           
        }

        private void BdsPositionGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (BdsPositionGrid.SelectedCells.Count > 0)
            {
                int nIndex = BdsPositionGrid.SelectedCells[0].RowIndex;
                Bds bds = m_Module as Bds;

                if (bds != null && nIndex >= 0)
                {
                    //ZzxzxyPositionData data = loader.Config.Positions[nIndex];
                    YPositionData data = bds.bdsParameter.Config.BdsPositions[nIndex];
                    //ZzxzxyPositionData data = loader.ParamConfig.LoaderPositions[nIndex];

                    BdsPositionPropertyGrid.SelectedObject = data;
                }
            }

            //GridConfigDataRefresh();
            //this.Invalidate();
        }

        public static void SetLabelColumnWidth(PropertyGrid grid, int width)
        {
            FieldInfo fi = grid.GetType().BaseType.GetField("gridView", BindingFlags.Instance | BindingFlags.NonPublic);
            object view = fi.GetValue(grid);
            MethodInfo mi = view.GetType().GetMethod("MoveSplitterTo", BindingFlags.Instance | BindingFlags.NonPublic);

            mi.Invoke(view, new object[] { width });
        }

        private void ButtonCommonParam_Save_Click(object sender, System.EventArgs e)
        {
            WorkStage workStage = m_Module as WorkStage;

            string strFIle = "";
            strFIle = ConfigManager.GetConfigPath() + "\\Common Setting (Do not delete or modify).ini";

            if (File.Exists(strFIle) == false)
            {
                File.Create(strFIle);
            }

            workStage.Machine_Parameter_Save();

            m_bCommonParam_Save = true;
            m_nCommonParam_Save_Count = 0;
        }

        void Timer_ParamLoadFunc(object sender, EventArgs e)
        {
            if (Equipment.m_bRedraw_FormBdsParameterConfig)
            {
                Equipment.m_bRedraw_FormBdsParameterConfig = false;

                SetGridConfigData(m_Module);
            }


            if (m_bCommonParam_Save)
            {
                if (m_nCommonParam_Save_Count++ < 2)
                {
                    Bds bds = m_Module as Bds;

                    bds.Machine_Parameter_Save();
                }
                else
                {
                    m_bCommonParam_Save = false;
                    m_nCommonParam_Save_Count = 0;

                    buttonCommonParam_Save.Visible = false;
                }
            }
        }

        private void UpdateDataGridViewBdsPosition()
        {
            BdsPositionGrid.Columns.Clear();
            BdsPositionGrid.AutoGenerateColumns = false;

            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Name";
                column.Name = "Position";
                BdsPositionGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Type";
                column.Name = "Target";
                BdsPositionGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Mask Y";
                column.Name = "Mask Y [mm]";
                BdsPositionGrid.Columns.Add(column);
            }

            //  첫번째 Column 의 크기만 크게 변경하기 위해 추가됨
            DataGridViewColumn column0 = BdsPositionGrid.Columns[0];
            column0.Width = 180;
        }
                
        //private void SetGridConfigData()
        //{
        //    DispenserPositionGrid.DataSource = m_Owner.Config.DispenserPositions;
        //    DispenserParameterGrid.SelectedObject = m_Owner.Config;
        //}

        public void SetGridConfigData(Module module)
        {
            Bds bds = module as Bds;

            //DispenserPositionGrid.DataSource = m_Owner.Config.DispenserPositions;
            //DispenserParameterGrid.SelectedObject = m_Owner.Config;

            //LoaderPositionGrid.DataSource = loader.Config.Positions;
            BdsPositionGrid.DataSource = bds.bdsParameter.Config.BdsPositions;
            //LoaderPositionGrid.DataSource = loader.ParamConfig.LoaderPositions;
            BdsParameterGrid.SelectedObject = bds.bdsParameter.Config;
        }

        private void BdsParameterGrid_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {

        }

        private void BdsPositionPropertyGrid_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {

        }

        static int m_nCnt = 0;

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            string m_strRecipe = "";
            RecipeInfo m_recipeInfo = new RecipeInfo();
            m_recipeInfo = Equipment.GetCurrentRecipe();

            if (m_recipeInfo != null)
            {
                m_strRecipe = m_recipeInfo.Name;

                //Equipment.LoadConfig(); // 참고 : param load                                //  2022. 06. 30.  SCH : 원래 이건데...
                Equipment.LoadConfig(m_strRecipe); // 참고 : param load                       //  2022. 06. 30.  SCH : Recipe 에 따라 Config 파라미터를 변경하기 위해 이걸로 함.
                DataManager.Instance.ApplyConfigData(m_Module);
            }
            else
            {
                Equipment.LoadConfig(); // 참고 : param load                                //  2022. 06. 30.  SCH : 원래 이건데...
                //Equipment.LoadConfig(m_strRecipe); // 참고 : param load                       //  2022. 06. 30.  SCH : Recipe 에 따라 Config 파라미터를 변경하기 위해 이걸로 함.
                DataManager.Instance.ApplyConfigData(m_Module);
            }

            Bds bds = m_Module as Bds;

            //  요걸 해 줘야 화면의 데이터가 바뀐다.
            //LoaderPositionGrid.DataSource = loader.Config.Positions;
            BdsPositionGrid.DataSource = bds.bdsParameter.Config.BdsPositions;
            //LoaderPositionGrid.DataSource = loader.ParamConfig.LoaderPositions;
            BdsParameterGrid.SelectedObject = bds.bdsParameter.Config;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            string m_strRecipe = "";
            RecipeInfo m_recipeInfo = new RecipeInfo();
            m_recipeInfo = Equipment.GetCurrentRecipe();

            if (m_recipeInfo != null)
            {
                m_strRecipe = m_recipeInfo.Name;

                DataManager.Instance.UpdateConfigData(m_Module); // 참고 : param save
                                                                 //Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                Equipment.SaveConfig(m_strRecipe);                                            //  2022. 06. 30.  SCH : Recipe 에 따라 Config 파라미터를 변경하기 위해 이걸로 함.
            }
            else
            {
                DataManager.Instance.UpdateConfigData(m_Module); // 참고 : param save
                Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
            }

            m_Module.Initialize();

            Bds bds = m_Module as Bds;

            Equipment.m_bRedraw_FormBdsParameterConfig = true;
            bds.m_bParameterSetting_PosData_Reload = true;

            //if (dispenserScale != null)
            //{
            //    DataManager.Instance.UpdateConfigData(dispenserScale); // 참고 : param save
            //    Equipment.SaveConfig();
            //}
        }

        private void GridConfigDataRefresh()
        {
            for (int i = 0; i < BdsPositionGrid.RowCount; i++)
            {
                if (i % 2 == 0)
                {
                    BdsPositionGrid.Rows[i].DefaultCellStyle.BackColor = Color.LightSkyBlue;
                }
                else
                {
                    BdsPositionGrid.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
                }
            }
        }
    }
}
