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

using SpiralLab.Sirius;

//using OpenTK;
//using OpenTK.Graphics.OpenGL;
//using SpiralLab.Sirius2;
//using SpiralLab.Sirius2.Laser;
//using SpiralLab.Sirius2.PowerMeter;
//using SpiralLab.Sirius2.Scanner;
//using SpiralLab.Sirius2.Scanner.Rtc;
//using SpiralLab.Sirius2.Winforms;
//using SpiralLab.Sirius2.Winforms.Entity;
//using SpiralLab.Sirius2.Winforms.Marker;
//using SpiralLab.Sirius2.Winforms.UI;

namespace SLD200_MSL
{
    //  2022. 03. 25.  SCH : Configuration --> Module --> WorkStage 이 여기로 오는군... 
    public partial class FormWorkStageParameterConfig : FormSubContentBase
    {
        //private WorkStageParameter m_Owner;
        Module m_Module { get; set; }

        public ScannerParameter m_Scanner;

        private bool m_bCommonParam_Save { get; set; }
        private int m_nCommonParam_Save_Count { get; set; }

        //  for Parameter Load
        public Timer timer_ParamLoad;

        public FormWorkStageParameterConfig(Module module)
            : base(FormType.Content.ToString(), module.Name)
        {
            InitializeComponent();
            //m_Owner = part as WorkStageParameter;
            m_Module = module;
            m_Scanner = (Part)m_Module as ScannerParameter;

            this.BackColor = Configuration.PanelBackColor;

            this.panelContent.Location = new Point(Configuration.ContentLocation.X, Configuration.PanelbuttonSize.Height);
            this.panelContent.BackColor = Configuration.PanelBackColor;


            this.buttonLoad.Size = Configuration.ButtonSize;
            this.buttonLoad.Location = new Point(Configuration.PanelSize.Width - Configuration.ControlsLocation.Width - Configuration.ButtonSize.Width * 2 - Configuration.ContentLocation.X, 3);
            this.buttonSave.Size = Configuration.ButtonSize;
            this.buttonSave.Location = new Point(Configuration.PanelSize.Width - Configuration.ButtonSize.Width - Configuration.ControlsLocation.Width, 3);

            this.LaserPositionGrid.Size = new Size((Configuration.MainSize.Width / 2) - 40, Configuration.ContentSize.Height - Configuration.PanelSize.Height * 7 - 172);
            this.LaserPositionGrid.Location = new Point(Configuration.ContentLocation.X, Configuration.ContentLocation.Y + Configuration.PanelSize.Height * 1 + Configuration.ButtonSize.Height);

            this.LaserPositionPropertyGrid.Size = new Size((Configuration.MainSize.Width / 2) - 40, 220);
            this.LaserPositionPropertyGrid.Location = new Point(Configuration.ContentLocation.X, LaserPositionGrid.Location.Y + LaserPositionGrid.Size.Height + 10);

            this.LaserParameterGrid.Size = new Size((Configuration.MainSize.Width / 2) - 10, Configuration.ContentSize.Height - Configuration.PanelSize.Height * 7 + 58);
            this.LaserParameterGrid.Location = new Point(LaserPositionPropertyGrid.Location.X + LaserPositionPropertyGrid.Size.Width + 20, Configuration.ContentLocation.Y + Configuration.PanelSize.Height * 1 + Configuration.ButtonSize.Height);

            SetLabelColumnWidth(LaserParameterGrid, 10);

            //this.baseLabelModuleConfiguration.Size = new Size(this.LaserPositionGrid.Width + LaserPositionPropertyGrid.Size.Width, Configuration.ButtonSize.Height);
            this.baseLabelModuleConfiguration.Size = new Size(this.LaserPositionGrid.Width, Configuration.ButtonSize.Height);
            this.baseLabelModuleConfiguration.Location = new Point(this.LaserPositionGrid.Location.X, this.flowLayoutPanelButton.Location.Y + this.flowLayoutPanelButton.Height);
            this.baseLabelModuleConfiguration.Text = "Position";
            this.baseLabelModuleConfiguration.ForeColor = Color.Black;
            this.baseLabelModuleConfiguration.TextAlign = ContentAlignment.MiddleCenter;

            //this.baseLabelModuleParameter.Size = new Size(this.LaserParameterGrid.Width, Configuration.ButtonSize.Height);
            this.baseLabelModuleParameter.Size = new Size((int)((double)this.LaserParameterGrid.Width * (2.0 / 4.0)), Configuration.ButtonSize.Height);
            this.baseLabelModuleParameter.Location = new Point(this.LaserParameterGrid.Location.X + 20, this.flowLayoutPanelButton.Location.Y + this.flowLayoutPanelButton.Height);
            this.baseLabelModuleParameter.Text = "General";
            this.baseLabelModuleParameter.ForeColor = Color.Black;
            this.baseLabelModuleParameter.TextAlign = ContentAlignment.MiddleRight;

            this.buttonCommonParam_Save.Size = new Size(Configuration.ButtonSize.Width + 30, Configuration.ButtonSize.Height - 5);
            this.buttonCommonParam_Save.Location = new Point(baseLabelModuleParameter.Location.X + baseLabelModuleParameter.Size.Width + 264, baseLabelModuleParameter.Location.Y);

            LaserPositionGrid.SelectionChanged += LaserPositionGrid_SelectionChanged;

            this.panelContent.Hide();

            this.Controls.Add(this.LaserPositionGrid);
            this.Controls.Add(this.LaserPositionPropertyGrid);
            this.Controls.Add(this.LaserParameterGrid);
            this.Controls.Add(this.baseLabelModuleConfiguration);
            this.Controls.Add(this.baseLabelModuleParameter);
            //List<StageLoader> list = new List<StageLoader>();
            //SetGridConfigData(module);
          
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonCommonParam_Save);

            UpdateDataGridViewWorkStagePosition();
            SetGridConfigData(module);

            //  Parameter Load 하는 타이머
            timer_ParamLoad = new Timer();
            timer_ParamLoad.Interval = 100;
            timer_ParamLoad.Tick += new System.EventHandler(Timer_ParamLoadFunc);
            timer_ParamLoad.Enabled = true;


            //this.Load += FormDispenserParameterConfig_Load;           

            m_bCommonParam_Save = false;
            m_nCommonParam_Save_Count = 0;


            WorkStage workStage = m_Module as WorkStage;

            if (!workStage.Machine_Parameter_Exist())
            {
                buttonCommonParam_Save.Visible = true;
            }
            else
            {
                buttonCommonParam_Save.Visible = false;

                workStage.Machine_Parameter_Load();
            }
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
            if (Equipment.m_bRedraw_FormWorkStageParameterConfig)
            {
                Equipment.m_bRedraw_FormWorkStageParameterConfig = false;

                SetGridConfigData(m_Module);
            }


            if (m_bCommonParam_Save)
            {
                if (m_nCommonParam_Save_Count++ < 2)
                {
                    WorkStage workStage = m_Module as WorkStage;

                    workStage.Machine_Parameter_Save();
                }
                else
                {
                    m_bCommonParam_Save = false;
                    m_nCommonParam_Save_Count = 0;

                    buttonCommonParam_Save.Visible = false;
                }
            }
        }

        private void UpdateDataGridViewWorkStagePosition()
        {
            LaserPositionGrid.Columns.Clear();
            LaserPositionGrid.AutoGenerateColumns = false;

            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Name";
                column.Name = "Position";
                LaserPositionGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Type";
                column.Name = "Target";
                LaserPositionGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "X";
                column.Name = "X [mm]";
                LaserPositionGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Y";
                column.Name = "Y [mm]";
                LaserPositionGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Z";
                column.Name = "Scanner Z [mm]";
                LaserPositionGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "MASK_Y";
                column.Name = "Mask Y [mm]";
                LaserPositionGrid.Columns.Add(column);
            }

            //  첫번째 Column 의 크기만 크게 변경하기 위해 추가됨
            DataGridViewColumn column0 = LaserPositionGrid.Columns[0];
            column0.Width = 220;
        }

        private void LaserPositionGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (LaserPositionGrid.SelectedCells.Count > 0)
            {
                int nIndex = LaserPositionGrid.SelectedCells[0].RowIndex;
                WorkStage workStage = m_Module as WorkStage;

                if (workStage != null && nIndex >= 0)
                {
                    //XyzPositionData data = dispenserAndScale.dispenserParameter.Config.DispenserPositions[nIndex];
                    //XyztPositionData data = workStage.Config.Positions[nIndex];
                    //XyzztPositionData data = workStage.Config.Positions[nIndex];
                    //UvwzxyzPositionData data = workStage.Config.Positions[nIndex];

                    //XyzyPositionData data = workStage.Config.Positions[nIndex];
                    //XyzyPositionData data = workStage.workStageParameter.Config.WorkStagePositions[nIndex];
                    XyzyPositionData data = workStage.Config.ParamConfig.WorkStagePositions[nIndex];

                    LaserPositionPropertyGrid.SelectedObject = data;
                }
            }

            //GridConfigDataRefresh();
            //this.Invalidate();
        }

        //private void SetGridConfigData()
        //{
        //    DispenserPositionGrid.DataSource = m_Owner.Config.DispenserPositions;
        //    DispenserParameterGrid.SelectedObject = m_Owner.Config;
        //}

        public void SetGridConfigData(Module module)
        {
            WorkStage workStage = module as WorkStage;

            //LaserPositionGrid.DataSource = workStage.Config.Positions;
            LaserPositionGrid.DataSource = workStage.Config.ParamConfig.WorkStagePositions;
            LaserParameterGrid.SelectedObject = workStage.workStageParameter.Config ;
        }

        private void LaserParameterGrid_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {

        }

        private void LaserPositionPropertyGrid_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
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

            WorkStage workStage = m_Module as WorkStage;

            workStage.Machine_Parameter_Load();

            //  요걸 해 줘야 화면의 데이터가 바뀐다.
            //LaserPositionGrid.DataSource = workStage.Config.Positions;
            LaserPositionGrid.DataSource = workStage.Config.ParamConfig.WorkStagePositions;
            LaserParameterGrid.SelectedObject = workStage.workStageParameter.Config;
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

            WorkStage workStage = m_Module as WorkStage;
            workStage.Machine_Parameter_Save();

            Equipment.m_bRedraw_FormWorkStageParameterConfig = true;
            workStage.m_bParameterSetting_PosData_Reload = true;

            //if (dispenserScale != null)
            //{
            //    DataManager.Instance.UpdateConfigData(dispenserScale); // 참고 : param save
            //    Equipment.SaveConfig();
            //}
        }

        private void GridConfigDataRefresh()
        {
            for (int i = 0; i < LaserPositionGrid.RowCount; i++)
            {
                if (i % 2 == 0)
                {
                    LaserPositionGrid.Rows[i].DefaultCellStyle.BackColor = Color.LightSkyBlue;
                }
                else
                {
                    LaserPositionGrid.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
                }
            }
        }
    }
}
