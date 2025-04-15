using QMC.Common;
using QMC.Common.Interpolator;
using QMC.Common.Modules;
using QMC.Common.Motion.Ajin;
using QMC.Common.Parts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace QMC.Vision
{
    public delegate void LoadButtonClickEventHandler();
    public partial class _2DMappingFileControl : UserControl
    {
        static WorkStage workStage;
        private XyzyStage m_Owner;
        private PerspectiveProjectionInterpolator m_interpolator;
        public event LoadButtonClickEventHandler LoadButtonClick;
        public _2DMappingFileControl(XyzyStage xyzyStage)
        {
            string strFileName = "";

            m_Owner = xyzyStage as XyzyStage;

            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;

            foreach (Module module in m_collectionModules)
            {
                if (module.Name == "WorkStage")
                {
                    workStage = module as WorkStage;
                }
            }

            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();

            Update(m_Owner.Config);


            if (workStage.Config.ParamConfig.MapFile_Path != null)
            {
                strFileName = workStage.Config.ParamConfig.MapFile_Path;

                FileInfo fi = new FileInfo(strFileName);
                if (fi.Exists)
                {
                    this.baseTextFilePath.Text = strFileName;
                    this.m_Owner.Config.FileName = strFileName;

                    if (m_interpolator == null)
                        m_interpolator = new PerspectiveProjectionInterpolator();
                    m_interpolator.Load(strFileName);
                    XyCoordinate source = new XyCoordinate(-54, 108);
                    XyCoordinate target = new XyCoordinate();
                    m_interpolator.Interpolate(source, ref target);
                    if (m_Owner.Axes[XytStage.MotionKey.X.ToString()] != null && m_Owner.Axes[XytStage.MotionKey.Y.ToString()] != null)
                    {
                        m_interpolator.SetAxis(m_Owner.Axes[XytStage.MotionKey.X.ToString()], m_Owner.Axes[XytStage.MotionKey.Y.ToString()]);
                    }

                    //m_Owner.Config.Use2DMap = workStage.Config.ParamConfig.MapFileApply_WhenPgmStart;
                    m_Owner.Config.Use2DMap = Equipment.MapDataStatus_Activate;

                    if (m_Owner.Config.Use2DMap)
                    {
                        m_Owner.Interpolator = m_interpolator;
                        workStage.Stage.Interpolator = m_interpolator;
                    }
                    else
                    {
                        workStage.Stage.Interpolator = null;
                        m_Owner.Interpolator = null;
                    }

                    if (LoadButtonClick != null)
                    {
                        LoadButtonClick();
                    }

                    UpdateToggleButton(m_Owner.Config.Use2DMap);
                }
                else
                {
                    workStage.Stage.Interpolator = null;

                    string m_strTemp;
                    m_strTemp = "[ " + strFileName + " ] 경로에 맵 파일이 없습니다.";
                    MessageBox.Show(m_strTemp, "Information!!");
                }
            }

            this.Load += _2DMappingFileControl_Load;
        }

        private void _2DMappingFileControl_Load(object sender, EventArgs e)
        {

        }
        #region Method

        public void Update(XyzyStageConfig config)
        {
            m_Owner.Config = config;
            if (m_Owner.Config != null)
            {
                this.baseTextFilePath.Text = m_Owner.Config.FileName;
                UpdateToggleButton(m_Owner.Config.Use2DMap);
            }
        }

        private void baseBtn_Load_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            string strFileName = "";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                strFileName = dialog.FileName;
                this.baseTextFilePath.Text = strFileName;
                this.m_Owner.Config.FileName = strFileName;

                workStage.Config.ParamConfig.MapFile_Path = strFileName;


                string m_strRecipe = "";
                RecipeInfo m_recipeInfo = new RecipeInfo();
                m_recipeInfo = Equipment.GetCurrentRecipe();

                if (m_recipeInfo != null)
                {
                    m_strRecipe = m_recipeInfo.Name;

                    //DataManager.Instance.UpdateConfigData(workStage); // 참고 : param save
                    //Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                    Equipment.SaveConfig(m_strRecipe);                                            //  2022. 06. 30.  SCH : Recipe 에 따라 Config 파라미터를 변경하기 위해 이걸로 함.
                }
                else
                {
                    //DataManager.Instance.UpdateConfigData(workStage); // 참고 : param save
                    Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                }

                DataManager.Instance.ApplyConfigData(workStage);

                //  Config 창 데이터 갱신을 위해서
                Equipment.m_bRedraw_FormWorkStageParameterConfig = true;


                if (m_interpolator == null)
                    m_interpolator = new PerspectiveProjectionInterpolator();
                m_interpolator.Load(strFileName);
                XyCoordinate source = new XyCoordinate(-54, 108);
                XyCoordinate target = new XyCoordinate();
                m_interpolator.Interpolate(source, ref target);
                if (m_Owner.Axes[XytStage.MotionKey.X.ToString()] != null && m_Owner.Axes[XytStage.MotionKey.Y.ToString()] != null)
                {
                    m_interpolator.SetAxis(m_Owner.Axes[XytStage.MotionKey.X.ToString()], m_Owner.Axes[XytStage.MotionKey.Y.ToString()]);
                }
                if(m_Owner.Config.Use2DMap)
                    m_Owner.Interpolator= m_interpolator;

                if (LoadButtonClick != null)
                {
                    LoadButtonClick();
                }
            }
        }

        private void UpdateToggleButton(bool bOn)
        {
            if (bOn)
            {
                baseToggleBtn_Use.UpdateToggleStatus(true);                
            }
            else
            {
                baseToggleBtn_Use.UpdateToggleStatus(false);
            }
        }

        private void baseToggleBtn_Use_Click(object sender, EventArgs e)
        {
            uint nRet = 0;

            if (m_Owner.Config != null)
            {
                bool bOn = m_Owner.Config.Use2DMap;
                m_Owner.Config.Use2DMap = !bOn;

                if(m_Owner.Config.Use2DMap)
                {
                    m_Owner.Interpolator = m_interpolator;
                    workStage.Stage.Interpolator = m_interpolator;

                    workStage.Config.ParamConfig.MapFileApply_WhenPgmStart = true;
                    //Equipment.MapDataStatus_Activate = true;
                }
                else
                {
                    m_Owner.Interpolator = null;
                    workStage.Stage.Interpolator = null;

                    workStage.Config.ParamConfig.MapFileApply_WhenPgmStart = false;
                    //Equipment.MapDataStatus_Activate = false;
                }
                
                UpdateToggleButton(m_Owner.Config.Use2DMap);

                //  Ajin 2D Mapping Compensation Disable (All)
                for ( int iter = 0; iter < 16; iter++)
                {
                    nRet = AXM.AxmCompensationTwoDimEnable(iter, 0);
                }

                string m_strRecipe = "";
                RecipeInfo m_recipeInfo = new RecipeInfo();
                m_recipeInfo = Equipment.GetCurrentRecipe();

                if (m_recipeInfo != null)
                {
                    m_strRecipe = m_recipeInfo.Name;

                    //DataManager.Instance.UpdateConfigData(workStage); // 참고 : param save
                    //Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                    Equipment.SaveConfig(m_strRecipe);                                            //  2022. 06. 30.  SCH : Recipe 에 따라 Config 파라미터를 변경하기 위해 이걸로 함.
                }
                else
                {
                    //DataManager.Instance.UpdateConfigData(workStage); // 참고 : param save
                    Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                }

                //DataManager.Instance.ApplyConfigData(workStage);                   //  이게 있으니 Toggle 이 안됨

                //  Config 창 데이터 갱신을 위해서
                //Equipment.m_bRedraw_FormWorkStageParameterConfig = true;
            }
        }
        #endregion



    }
}
