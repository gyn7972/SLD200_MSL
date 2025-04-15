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
using System.Windows;
using System.Numerics;
using QMC.Core;
using QMC.Common;
using QMC.Common.Parts;
using QMC.Common.Modules;
using QMC.Common.Motion.Ajin.Motions;
using ACS.SPiiPlusNET;
using System.Security.Permissions;
using QMC.Common.Motion.ACS.Motions;
using static QMC.Common.Modules.WorkStage;
using static QMC.Common.Equipment;
using QMC.Common.Hmi;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using MessageBox = System.Windows.Forms.MessageBox;
//using netDxf.Entities;

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
    public partial class ManualMode_SLD200 : UserControl
    {
        protected ModuleStateControl m_ModuleStateControl;
        private IlluminatorControl m_IlluminatorControl;
        //public ModulePositionControl m_WorkStagePosControl;
        public JogControl m_JogControl;

        public event JogButtonClickEventHandler JogButtonClick;
        public event JogButtonDownEventHandler JogButtonDown;
        public event JogButtonUpEventHandler JogButtonUp;
        public List<MotionAxis> AxisList { get; set; }
        public MotionAxis thetaValue { set; get; }
        public FormBaseConfiguration Configuration { get; set; }

        public AutoFocusControl m_AutoFocusControl;

        static WorkStage workStage;

        MotionFunction MC_Func = new InterpolatorMotionFunction();

        public System.Windows.Forms.Timer timer_IOStatus;

        #region Tick Count Check
        //System.Diagnostics.Stopwatch sw_User = new System.Diagnostics.Stopwatch();

        public enum TickType : int
        {
            TICK_USER = 0,          //  0 : User
            TICK_USER2 = 1,         //  0 : User2
            TICK_USER3 = 2,         //  0 : User3
        }

        public int[,] TickCount_Cycle = new int[10, 2];          //  0 : User
                                                                 //  1 : 
                                                                 //  2 : 

        public void TickCount_Start(int m_nIndex)
        {
            TickCount_Cycle[m_nIndex, 0] = Environment.TickCount;
        }
        public int TickCount_Elapsed(int m_nIndex)
        {
            int TickCount_Elapsed = 0;
            TickCount_Cycle[m_nIndex, 1] = Environment.TickCount;
            TickCount_Elapsed = TickCount_Cycle[m_nIndex, 1] - TickCount_Cycle[m_nIndex, 0];

            return TickCount_Elapsed;
        }
        #endregion

        static int marginWidth = 10;
        static int marginHeight = 10;
        static int btnWidth = 50;
        static int btnHeight = 50;

        //  Test용 변수
        bool m_bRun;
        string m_strTemp;

        public ManualMode_SLD200()
        {
            InitializeComponent();
            AxisList = new List<MotionAxis>();

            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;

            foreach (Module module in m_collectionModules)
            {
                //if (module.Name == "WorkStage")
                if (module.Name == "WorkStage")
                {
                    workStage = module as WorkStage;
                }
            }

            this.VisibleChanged += ManualMode_CWA150SA_VisibleChanged;

            //  조명
            //this.m_IlluminatorControl = new IlluminatorControl(workStage.visionCalibrator_Upper.Recipe.IlluminationDataSet.ToList());
            //this.m_IlluminatorControl.Location = new Point(this.btnUpperCamera_Init.Location.X, m_visionImageViewer_Upper.Location.Y + m_visionImageViewer_Upper.Size.Height + 23);
            //this.m_IlluminatorControl.Illuminator = workStage.visionCalibrator_Upper.Illuminator;
            //this.m_IlluminatorControl.IlluminatorControlButton_Click += m_IlluminatorControl_IlluminatorControlButton_Click;
            //this.Controls.Add(m_IlluminatorControl);

            //  IO 상태 표시
            timer_IOStatus = new System.Windows.Forms.Timer();
            //timer_IOStatus.Interval = 50;
            timer_IOStatus.Interval = 1;
            timer_IOStatus.Tick += new System.EventHandler(Timer_IOStatus);
            //timer_IOStatus.Enabled = true;
        }

        private void ManualMode_CWA150SA_VisibleChanged(object sender, EventArgs e)
        {
            
        }

        void Timer_IOStatus(object sender, EventArgs e)
        {
            timer_IOStatus.Enabled = false;
            

            ///////////////////////////////////////////////////////////////////////////////////////
            //  Input
            //   

            //if (workStage.workStageParameter.DI_Main_CDACheck())
            //    pictureBoxMainAirCheck.Image = global::SLD200.Properties.Resources.DioEllipseOn;
            //else
            //    pictureBoxMainAirCheck.Image = global::SLD200.Properties.Resources.StopOn;           


            ///////////////////////////////////////////////////////////////////////////////////////
            //  Output
            //

            //if (workStage.workStageParameter.IsDO_ThinChuck_StageCleaning())
            //    baseButtonThinChuckStageCleaning.BackColor = Color.Lime;
            //else
            //    baseButtonThinChuckStageCleaning.BackColor = Color.DimGray;
            

            ///////////////////////////////////////////////////////////////////////////////////////
            //  Etc
            //


            timer_IOStatus.Enabled = true;
        }

        private void m_IlluminatorControl_IlluminatorControlButton_Click(IlluminatorControl.ButtonType type)
        {
            if (type == IlluminatorControl.ButtonType.Save)
            {
                //Module module = workStage m_Owner.Owner as Module;               

                //string m_strRecipe = "";
                //RecipeInfo m_recipeInfo = new RecipeInfo();
                //m_recipeInfo = Equipment.GetCurrentRecipe();

                //if (m_recipeInfo != null)
                //{
                //    m_strRecipe = m_recipeInfo.Name;

                //    DataManager.Instance.UpdateConfigData(workStage); // 참고 : param save
                //    //Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                //    Equipment.SaveConfig(m_strRecipe);                                            //  2022. 06. 30.  SCH : Recipe 에 따라 Config 파라미터를 변경하기 위해 이걸로 함.
                //}
                //else
                //{
                //    DataManager.Instance.UpdateConfigData(workStage); // 참고 : param save
                //    Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                //}

                //DataManager.Instance.ApplyConfigData(workStage);

                ////  Config 창 데이터 갱신을 위해서
                //Equipment.m_bRedraw_FormWorkStageParameterConfig = true;

                //Equipment.UpdateRecipeData();
                //Equipment.SaveRecipe();
            }
            else if (type == IlluminatorControl.ButtonType.AllOff)
            {

            }
            else { }
        }

        #region buttonDownEvent

        private void buttonAxisXUp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonDown(JogControlButtonList.buttonCW, AxisList);
            }
        }

        private void buttonAxisXDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonDown(JogControlButtonList.buttonCCW, AxisList);
            }
        }

        #endregion

        #region buttonUpEvent

        private void buttonAxisXUp_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonUp(JogControlButtonList.buttonCW, AxisList);
            }
        }

        private void buttonAxisXDown_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonUp(JogControlButtonList.buttonCCW, AxisList);
            }
        }

        #endregion


        private void ManualMode_CWA150SA_Load(object sender, EventArgs e)
        {

        }
        
        private void ProgressForm_StopProcess(object target)
        {
            Part part = target as Part;
            if (part != null)
            {
                //part.Stop();
            }
        }
    }
}
