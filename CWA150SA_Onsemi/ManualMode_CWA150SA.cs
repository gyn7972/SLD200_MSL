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
using SpiralLab.Sirius;
using ACS.SPiiPlusNET;
using System.Security.Permissions;
using QMC.Common.Motion.ACS.Motions;
using static QMC.Common.Modules.WaferProbeAlign;
using static QMC.Common.Equipment;
using QMC.Common.Hmi;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using MessageBox = System.Windows.Forms.MessageBox;
//using netDxf.Entities;

namespace CWA150SA_Onsemi300
{
    public partial class ManualMode_CWA150SA : UserControl
    {
        protected ModuleStateControl m_ModuleStateControl;
        private IlluminatorControl m_IlluminatorControl;
        //public ModulePositionControl m_WaferProbeAlignPosControl;
        public JogControl m_JogControl;

        public event JogButtonClickEventHandler JogButtonClick;
        public event JogButtonDownEventHandler JogButtonDown;
        public event JogButtonUpEventHandler JogButtonUp;
        public List<MotionAxis> AxisList { get; set; }
        public MotionAxis thetaValue { set; get; }
        public FormBaseConfiguration Configuration { get; set; }

        public AutoFocusControl m_AutoFocusControl;

        static WaferProbeAlign waferProbeAlign;

        MotionFunction MC_Func = new MotionFunction();

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

        public ManualMode_CWA150SA()
        {
            InitializeComponent();
            AxisList = new List<MotionAxis>();

            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;

            foreach (Module module in m_collectionModules)
            {
                if (module.Name == "WaferProbeAlign")
                {
                    waferProbeAlign = module as WaferProbeAlign;
                }
            }

            //m_ModuleStateControl = new ModuleStateControl(waferProbeAlign);

            //m_ModuleStateControl.Location = new Point(5, 0);
            //m_ModuleStateControl.Size = new System.Drawing.Size(175, 500);
            //this.Controls.Add(this.m_ModuleStateControl);

            //m_visionImageViewer_Upper.Location = new Point(m_ModuleStateControl.Location.X + m_ModuleStateControl.Size.Width + 10, m_ModuleStateControl.Location.Y);
            //m_visionImageViewer_Lower.Location = new Point(m_visionImageViewer_Upper.Location.X, m_visionImageViewer_Upper.Location.Y + m_visionImageViewer_Upper.Size.Height + 20);

            //baseLabel_ProbeCard_Camera.Location = new Point(m_visionImageViewer_Upper.Location.X + m_visionImageViewer_Upper.Size.Width + 2, m_visionImageViewer_Upper.Location.Y + 2);
            //baseLabel_ProbeCard_Camera1.Location = new Point(baseLabel_ProbeCard_Camera.Location.X, baseLabel_ProbeCard_Camera.Location.Y + baseLabel_ProbeCard_Camera.Size.Height - 1);
            //baseLabel_WaferChuck_Camera.Location = new Point(m_visionImageViewer_Lower.Location.X + m_visionImageViewer_Lower.Size.Width + 2, m_visionImageViewer_Lower.Location.Y + 2);
            //baseLabel_WaferChuck_Camera1.Location = new Point(baseLabel_WaferChuck_Camera.Location.X, baseLabel_WaferChuck_Camera.Location.Y + baseLabel_WaferChuck_Camera.Size.Height - 1);

            this.m_visionImageViewer_Upper.SizeMode = PictureBoxSizeMode.CenterImage;
            this.m_visionImageViewer_Upper.SuspendDisplay();
            this.m_visionImageViewer_Lower.SizeMode = PictureBoxSizeMode.CenterImage;
            this.m_visionImageViewer_Lower.SuspendDisplay();

            this.m_visionImageViewer_Upper.Camera = waferProbeAlign.Camera_Upper;
            this.m_visionImageViewer_Lower.Camera = waferProbeAlign.Camera_Lower;

            this.VisibleChanged += ManualMode_CWA150SA_VisibleChanged;

            //  Jog Control
            m_JogControl = new JogControl(waferProbeAlign.Stage);
            //m_JogControl.Location = new Point(m_visionImageViewer_Lower.Location.X + m_visionImageViewer_Lower.Size.Width + 30, m_visionImageViewer_Lower.Location.Y);
            m_JogControl.Location = new Point(m_visionImageViewer_Lower.Location.X + m_visionImageViewer_Lower.Size.Width + 30, baseLabel_ProbeCard_Camera1.Location.Y);
            this.Controls.Add(m_JogControl);            

            //  조명
            this.m_IlluminatorControl = new IlluminatorControl(waferProbeAlign.visionCalibrator_Upper.Recipe.IlluminationDataSet.ToList());
            //this.m_IlluminatorControl.Location = new Point(this.btnUpperCamera_Init.Location.X, m_visionImageViewer_Upper.Location.Y + m_visionImageViewer_Upper.Size.Height + 23);
            this.m_IlluminatorControl.Location = new Point(this.btnUpperCamera_Init.Location.X, groupBoxAlignCheckPosParameter.Location.Y);

            this.m_IlluminatorControl.Illuminator = waferProbeAlign.visionCalibrator_Upper.Illuminator;
            this.m_IlluminatorControl.IlluminatorControlButton_Click += m_IlluminatorControl_IlluminatorControlButton_Click;
            this.Controls.Add(m_IlluminatorControl);

            //  IO 상태 표시
            timer_IOStatus = new System.Windows.Forms.Timer();
            timer_IOStatus.Interval = 50;
            timer_IOStatus.Tick += new System.EventHandler(Timer_IOStatus);
            timer_IOStatus.Enabled = true;
        }

        private void ManualMode_CWA150SA_VisibleChanged(object sender, EventArgs e)
        {
            if (m_visionImageViewer_Upper != null)
            {
                if (this.Visible == true)
                {
                    m_visionImageViewer_Upper.StartUpdateTask();
                    //AddOverlay(visionImageViewer);
                }
                else
                {
                    m_visionImageViewer_Upper.StopUpdateTask();
                }
            }

            if (m_visionImageViewer_Lower != null)
            {
                if (this.Visible)
                {
                    m_visionImageViewer_Lower.StartUpdateTask();
                }
                else
                {
                    m_visionImageViewer_Lower.StopUpdateTask();
                }
            }
        }

        void Timer_IOStatus(object sender, EventArgs e)
        {
            //  카메라 라이브 상태인지 표시
            if ((waferProbeAlign.jigAligner_Upper != null) && (waferProbeAlign.jigAligner_Lower != null))
            {
                if (waferProbeAlign.jigAligner_Upper.Camera.IsLiveOn && waferProbeAlign.jigAligner_Lower.Camera.IsLiveOn)
                {
                    btnUpperCamera_StartLive.BackColor = Color.LightGreen;
                }
                else
                {
                    btnUpperCamera_StartLive.BackColor = Color.LightGray;
                }
            }


            //  ProbeCard Clamp Type 에 따라 UI 변경
            if (Equipment.ProbeCard_ClampType == (int)WaferProbeAlign.nProbeClampType.Type_A)
            {
                if (tabControl_ProbeCard_ClampType.SelectedIndex != (int)WaferProbeAlign.nProbeClampType.Type_A)
                {
                    tabControl_ProbeCard_ClampType.SelectTab((int)WaferProbeAlign.nProbeClampType.Type_A);
                }
            }
            else if (Equipment.ProbeCard_ClampType == (int)WaferProbeAlign.nProbeClampType.Type_B)
            {
                if (tabControl_ProbeCard_ClampType.SelectedIndex != (int)WaferProbeAlign.nProbeClampType.Type_B)
                {
                    tabControl_ProbeCard_ClampType.SelectTab((int)WaferProbeAlign.nProbeClampType.Type_B);
                }
            }


            ///////////////////////////////////////////////////////////////////////////////////////
            //  Input
            //   

            if (waferProbeAlign.waferProbeAlignParameter.DI_Main_CDACheck())
                pictureBoxMainAirCheck.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOn;
            else
                pictureBoxMainAirCheck.Image = global::CWA150SA_Onsemi.Properties.Resources.StopOn;

            if (waferProbeAlign.waferProbeAlignParameter.DI_Main_VacuumCheck())
                pictureBoxMainVacuumCheck.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOn;
            else
                pictureBoxMainVacuumCheck.Image = global::CWA150SA_Onsemi.Properties.Resources.StopOn;

            if (waferProbeAlign.waferProbeAlignParameter.DI_ThinChuck_VacuumCheck())
                pictureBoxThinChuckVacuumCheck.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOn;
            else
                pictureBoxThinChuckVacuumCheck.Image = global::CWA150SA_Onsemi.Properties.Resources.StopOn;

            if (waferProbeAlign.waferProbeAlignParameter.DI_Wafer_VacuumCheck())
                pictureBoxWaferVacuumCheck.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOn;
            else
                pictureBoxWaferVacuumCheck.Image = global::CWA150SA_Onsemi.Properties.Resources.StopOn;

            if (waferProbeAlign.waferProbeAlignParameter.DI_Probe_PackingCheck())
                pictureBoxProbePackingCheck.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOn;
            else
                pictureBoxProbePackingCheck.Image = global::CWA150SA_Onsemi.Properties.Resources.StopOn;

            if (waferProbeAlign.waferProbeAlignParameter.DI_ThinChuck_Detect())
                pictureBoxThinChuckDetect.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOn;
            else
                pictureBoxThinChuckDetect.Image = global::CWA150SA_Onsemi.Properties.Resources.StopOn;

            if (Equipment.ProbeCard_ClampType == (int)WaferProbeAlign.nProbeClampType.Type_A)                                       //  1호기
            {
                if (waferProbeAlign.waferProbeAlignParameter.DI_Probe_BW_Detect())
                    pictureBoxProbeBWDetect.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOn;
                else
                    pictureBoxProbeBWDetect.Image = global::CWA150SA_Onsemi.Properties.Resources.StopOn;

                if (waferProbeAlign.waferProbeAlignParameter.DI_TopCover_Up())
                    pictureBoxTopCoverUp.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOn;
                else
                    pictureBoxTopCoverUp.Image = global::CWA150SA_Onsemi.Properties.Resources.StopOn;

                if (waferProbeAlign.waferProbeAlignParameter.DI_TopCover_Down())
                    pictureBoxTopCoverDown.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOn;
                else
                    pictureBoxTopCoverDown.Image = global::CWA150SA_Onsemi.Properties.Resources.StopOn;
            }
            else if (Equipment.ProbeCard_ClampType == (int)WaferProbeAlign.nProbeClampType.Type_B)                                  //  2 ~ 6호기
            {
                if (waferProbeAlign.waferProbeAlignParameter.DI_Probe_LeftClampModule_FW())
                    pictureBoxProbeLeftClamp_FW.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOn;
                else
                    pictureBoxProbeLeftClamp_FW.Image = global::CWA150SA_Onsemi.Properties.Resources.StopOn;

                if (waferProbeAlign.waferProbeAlignParameter.DI_Probe_LeftClampModule_BW())
                    pictureBoxProbeLeftClamp_BW.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOn;
                else
                    pictureBoxProbeLeftClamp_BW.Image = global::CWA150SA_Onsemi.Properties.Resources.StopOn;

                if (waferProbeAlign.waferProbeAlignParameter.DI_Probe_RightClampModule_FW())
                    pictureBoxProbeRightClamp_FW.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOn;
                else
                    pictureBoxProbeRightClamp_FW.Image = global::CWA150SA_Onsemi.Properties.Resources.StopOn;

                if (waferProbeAlign.waferProbeAlignParameter.DI_Probe_RightClampModule_BW())
                    pictureBoxProbeRightClamp_BW.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOn;
                else
                    pictureBoxProbeRightClamp_BW.Image = global::CWA150SA_Onsemi.Properties.Resources.StopOn;

                if (waferProbeAlign.waferProbeAlignParameter.DI_Probe_UnpackingCyl_Down())
                    pictureBoxProbeUnpackingCyl_Down.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOn;
                else
                    pictureBoxProbeUnpackingCyl_Down.Image = global::CWA150SA_Onsemi.Properties.Resources.StopOn;

                if (waferProbeAlign.waferProbeAlignParameter.DI_Probe_UnpackingCyl_Up())
                    pictureBoxProbeUnpackingCyl_Up.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOn;
                else
                    pictureBoxProbeUnpackingCyl_Up.Image = global::CWA150SA_Onsemi.Properties.Resources.StopOn;
            }


            ///////////////////////////////////////////////////////////////////////////////////////
            //  Output
            //

            if (waferProbeAlign.waferProbeAlignParameter.IsDO_ThinChuck_StageCleaning())
                baseButtonThinChuckStageCleaning.BackColor = Color.Lime;
            else
                baseButtonThinChuckStageCleaning.BackColor = Color.DimGray;

            if (waferProbeAlign.waferProbeAlignParameter.IsDO_ThinChuck_Vacuum())
                baseButtonThinChuckVacuum.BackColor = Color.Lime;
            else
                baseButtonThinChuckVacuum.BackColor = Color.DimGray;

            if (waferProbeAlign.waferProbeAlignParameter.IsDO_Wafer_Vacuum())
                baseButtonWaferVacuum.BackColor = Color.Lime;
            else
                baseButtonWaferVacuum.BackColor = Color.DimGray;

            if (Equipment.ProbeCard_ClampType == (int)WaferProbeAlign.nProbeClampType.Type_A)                                       //  1호기
            {
                if (waferProbeAlign.waferProbeAlignParameter.IsDO_TopCover_Up())
                    baseButtonTopCoverUp.BackColor = Color.Lime;
                else
                    baseButtonTopCoverUp.BackColor = Color.DimGray;

                if (waferProbeAlign.waferProbeAlignParameter.IsDO_TopCover_Down())
                    baseButtonTopCoverDown.BackColor = Color.Lime;
                else
                    baseButtonTopCoverDown.BackColor = Color.DimGray;
            }
            else if (Equipment.ProbeCard_ClampType == (int)WaferProbeAlign.nProbeClampType.Type_B)                                  //  2 ~ 6호기
            {
                if (waferProbeAlign.waferProbeAlignParameter.IsDO_Probe_ClampModule_Down())
                    baseButtonProbeClamp_Down.BackColor = Color.Lime;
                else
                    baseButtonProbeClamp_Down.BackColor = Color.DimGray;

                if (waferProbeAlign.waferProbeAlignParameter.IsDO_Probe_ClampModule_FW())
                    baseButtonProbeClamp_FW.BackColor = Color.Lime;
                else
                    baseButtonProbeClamp_FW.BackColor = Color.DimGray;

                if (waferProbeAlign.waferProbeAlignParameter.IsDO_Probe_ClampModule_BW())
                    baseButtonProbeClamp_BW.BackColor = Color.Lime;
                else
                    baseButtonProbeClamp_BW.BackColor = Color.DimGray;

                if (waferProbeAlign.waferProbeAlignParameter.IsDO_Probe_UnpackingCyl_Down())
                    baseButtonProbeUnpackingCyl_Down.BackColor = Color.Lime;
                else
                    baseButtonProbeUnpackingCyl_Down.BackColor = Color.DimGray;

                if (waferProbeAlign.waferProbeAlignParameter.IsDO_Probe_UnpackingCyl_Up())
                    baseButtonProbeUnpackingCyl_Up.BackColor = Color.Lime;
                else
                    baseButtonProbeUnpackingCyl_Up.BackColor = Color.DimGray;
            }

            if (waferProbeAlign.waferProbeAlignParameter.IsDO_Probe_Packing())
                baseButtonProbePacking.BackColor = Color.Lime;
            else
                baseButtonProbePacking.BackColor = Color.DimGray;

            if (waferProbeAlign.waferProbeAlignParameter.IsDO_Probe_Unpacking())
                baseButtonProbeUnpacking.BackColor = Color.Lime;
            else
                baseButtonProbeUnpacking.BackColor = Color.DimGray;



            ///////////////////////////////////////////////////////////////////////////////////////
            //  Etc
            //
            //  웨이퍼, 프로브 카드 틀어진 각도 보여주기
            lblUpperCamera_AlignData.Text = waferProbeAlign.m_strProbeCard_TiltData_for_Display;
            lblLowerCamera_AlignData.Text = waferProbeAlign.m_strWafer_TiltData_for_Display;
        }

        private void m_IlluminatorControl_IlluminatorControlButton_Click(IlluminatorControl.ButtonType type)
        {
            if (type == IlluminatorControl.ButtonType.Save)
            {
                //Module module = waferProbeAlign m_Owner.Owner as Module;               

                //string m_strRecipe = "";
                //RecipeInfo m_recipeInfo = new RecipeInfo();
                //m_recipeInfo = Equipment.GetCurrentRecipe();

                //if (m_recipeInfo != null)
                //{
                //    m_strRecipe = m_recipeInfo.Name;

                //    DataManager.Instance.UpdateConfigData(waferProbeAlign); // 참고 : param save
                //    //Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                //    Equipment.SaveConfig(m_strRecipe);                                            //  2022. 06. 30.  SCH : Recipe 에 따라 Config 파라미터를 변경하기 위해 이걸로 함.
                //}
                //else
                //{
                //    DataManager.Instance.UpdateConfigData(waferProbeAlign); // 참고 : param save
                //    Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                //}

                //DataManager.Instance.ApplyConfigData(waferProbeAlign);

                ////  Config 창 데이터 갱신을 위해서
                //Equipment.m_bRedraw_FormWaferProbeAlignParameterConfig = true;

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

        private void btnMoveLoadingPos_Click(object sender, EventArgs e)
        {
            MotorStates m_nMotorState_Y;
            MotorStates m_nMotorState_X;

            //if (!ACSSPiiPlusMotionBoard.Api.IsConnected)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "ACS 모션 제어기가 연결되지 않았습니다.");
            //    return;
            //}

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Stage 를 가공 위치로 보내시겠습니까?"))
                return;

            //m_nMotorState_Y = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY);
            //m_nMotorState_X = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX);

            //if (((m_nMotorState_Y & MotorStates.ACSC_MST_MOVE) != 0) || ((m_nMotorState_Y & MotorStates.ACSC_MST_INPOS) == 0) ||
            //    ((m_nMotorState_X & MotorStates.ACSC_MST_MOVE) != 0) || ((m_nMotorState_X & MotorStates.ACSC_MST_INPOS) == 0))
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Warning !", "Stage 가 이동중입니다.");
            //    return;
            //}

            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("LaserWorkHeight");
            waferProbeAlign.waferProbeAlignParameter.stStageLoadPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("Load");

            //ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                      //  '0' - Absolute position
            //                                    (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY,
            //                                    waferProbeAlign.waferProbeAlignParameter.stStageLoadPosParam.dTarget[(int)nAxis.Y]);                           //  Target position

            //ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                      //  '0' - Absolute position
            //                                    (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX,
            //                                    waferProbeAlign.waferProbeAlignParameter.stStageLoadPosParam.dTarget[(int)nAxis.X]);                           //  Target position

            //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.InspectionZ, waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.IZ],
            //                    10.0, 10.0 * 100.0, 10.0 * 100.0);
        }

        private void btnMoveUnloadingPos_Click(object sender, EventArgs e)
        {
            MotorStates m_nMotorState_Y;
            MotorStates m_nMotorState_X;

            //if (!ACSSPiiPlusMotionBoard.Api.IsConnected)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "ACS 모션 제어기가 연결되지 않았습니다.");
            //    return;
            //}

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Stage 를 투입&&배출 위치로 보내시겠습니까?"))
                return;

            //m_nMotorState_Y = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY);
            //m_nMotorState_X = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX);

            //if (((m_nMotorState_Y & MotorStates.ACSC_MST_MOVE) != 0) || ((m_nMotorState_Y & MotorStates.ACSC_MST_INPOS) == 0) ||
            //    ((m_nMotorState_X & MotorStates.ACSC_MST_MOVE) != 0) || ((m_nMotorState_X & MotorStates.ACSC_MST_INPOS) == 0))
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Warning !", "Stage 가 이동중입니다.");
            //    return;
            //}

            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("UnLoad");

            //ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                      //  '0' - Absolute position
            //                                    (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY,
            //                                    waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y]);                           //  Target position

            //ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                      //  '0' - Absolute position
            //                                    (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX,
            //                                    waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X]);                           //  Target position
        }

        private void btnCamMoveToScannerPos_Click(object sender, EventArgs e)
        {
            MotorStates m_nMotorState_Y;
            MotorStates m_nMotorState_X;

            double m_dCurPos_Y = 0.0;
            double m_dCurPos_X = 0.0;

            //if (!ACSSPiiPlusMotionBoard.Api.IsConnected)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "ACS 모션 제어기가 연결되지 않았습니다.");
            //    return;
            //}

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "카메라를 현재 Scanner Center 위치로 보내시겠습니까?"))
                return;

            //m_nMotorState_Y = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY);
            //m_nMotorState_X = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX);

            //if (((m_nMotorState_Y & MotorStates.ACSC_MST_MOVE) != 0) || ((m_nMotorState_Y & MotorStates.ACSC_MST_INPOS) == 0) ||
            //    ((m_nMotorState_X & MotorStates.ACSC_MST_MOVE) != 0) || ((m_nMotorState_X & MotorStates.ACSC_MST_INPOS) == 0))
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Warning !", "Stage 가 이동중입니다.");
            //    return;
            //}

            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("LaserWorkHeight");

            //m_dCurPos_X = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX);            //  현재 X 위치
            //m_dCurPos_Y = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY);            //  현재 Y 위치

            //waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.X] = m_dCurPos_X + waferProbeAlign.Config.ParamConfig.OffsetX_fromHighResCamera_toScanner;
            //waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.Y] = m_dCurPos_Y + waferProbeAlign.Config.ParamConfig.OffsetY_fromHighResCamera_toScanner;

            //ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                      //  '0' - Absolute position
            //                                    (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY,
            //                                    waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y]);                           //  Target position

            //ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                      //  '0' - Absolute position
            //                                    (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX,
            //                                    waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X]);                           //  Target position
        }

        private void btnScannerMoveToCamPos_Click(object sender, EventArgs e)
        {
            MotorStates m_nMotorState_Y;
            MotorStates m_nMotorState_X;

            double m_dCurPos_Y = 0.0;
            double m_dCurPos_X = 0.0;

            //if (!ACSSPiiPlusMotionBoard.Api.IsConnected)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "ACS 모션 제어기가 연결되지 않았습니다.");
            //    return;
            //}

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Scanner Center 를 카메라가 보고 있는 위치로 보내시겠습니까?"))
                return;

            //m_nMotorState_Y = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY);
            //m_nMotorState_X = ACSSPiiPlusMotionBoard.Api.GetMotorState((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX);

            //if (((m_nMotorState_Y & MotorStates.ACSC_MST_MOVE) != 0) || ((m_nMotorState_Y & MotorStates.ACSC_MST_INPOS) == 0) ||
            //    ((m_nMotorState_X & MotorStates.ACSC_MST_MOVE) != 0) || ((m_nMotorState_X & MotorStates.ACSC_MST_INPOS) == 0))
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Warning !", "Stage 가 이동중입니다.");
            //    return;
            //}

            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("LaserWorkHeight");

            //m_dCurPos_X = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX);            //  현재 X 위치
            //m_dCurPos_Y = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY);            //  현재 Y 위치

            //waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.X] = m_dCurPos_X - waferProbeAlign.Config.ParamConfig.OffsetX_fromHighResCamera_toScanner;
            //waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.Y] = m_dCurPos_Y - waferProbeAlign.Config.ParamConfig.OffsetY_fromHighResCamera_toScanner;

            //ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                      //  '0' - Absolute position
            //                                    (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY,
            //                                    waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y]);                           //  Target position

            //ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                      //  '0' - Absolute position
            //                                    (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX,
            //                                    waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X]);                           //  Target position
        }

        private void lblMainVacuumCheck_Click(object sender, EventArgs e)
        {

        }

        private void groupBoxDetectStatus_Enter(object sender, EventArgs e)
        {

        }

        private void btnUpperCamera_Init_Click(object sender, EventArgs e)
        {
            //  카메라 초기화

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            Task<int> task = Task.Factory.StartNew<int>(() =>
            {
                waferProbeAlign.Camera_Upper.SetRunStatus(Part.RunStatus.Run);
                waferProbeAlign.Camera_Lower.SetRunStatus(Part.RunStatus.Run);
                waferProbeAlign.Camera_Upper.Initialize();
                waferProbeAlign.Camera_Lower.Initialize();
                return 0;
            });

            ProgressForm ProgressForm = new ProgressForm(waferProbeAlign.Name, "Camera Initializing...", task, waferProbeAlign.Camera_Upper);
            ProgressForm.StopProcess += ProgressForm_StopProcess;
            ProgressForm.StartPosition = FormStartPosition.CenterScreen;
            ProgressForm.ShowDialog();

            waferProbeAlign.Camera_Upper.Initialize();
            waferProbeAlign.Camera_Lower.Initialize();
        }

        private void ProgressForm_StopProcess(object target)
        {
            Part part = target as Part;
            if (part != null)
            {
                //part.Stop();
            }
        }

        private void baseButtonThinChuckStageCleaning_Click(object sender, EventArgs e)
        {
            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (waferProbeAlign.waferProbeAlignParameter.IsDO_ThinChuck_StageCleaning())
                waferProbeAlign.waferProbeAlignParameter.DO_ThinChuck_StageCleaning(false);
            else
                waferProbeAlign.waferProbeAlignParameter.DO_ThinChuck_StageCleaning(true);
        }

        private void baseButtonThinChuckVacuum_Click(object sender, EventArgs e)
        {
            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (waferProbeAlign.waferProbeAlignParameter.IsDO_ThinChuck_Vacuum())
                waferProbeAlign.waferProbeAlignParameter.DO_ThinChuck_Vacuum(false);
            else
                waferProbeAlign.waferProbeAlignParameter.DO_ThinChuck_Vacuum(true);
        }

        private void baseButtonWaferVacuum_Click(object sender, EventArgs e)
        {
            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (waferProbeAlign.waferProbeAlignParameter.IsDO_Wafer_Vacuum())
                waferProbeAlign.waferProbeAlignParameter.DO_Wafer_Vacuum(false);
            else
                waferProbeAlign.waferProbeAlignParameter.DO_Wafer_Vacuum(true);
        }

        private void baseButtonTopCoverUp_Click(object sender, EventArgs e)
        {
            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (waferProbeAlign.waferProbeAlignParameter.IsDO_TopCover_Up())
            {
                waferProbeAlign.waferProbeAlignParameter.DO_TopCover_Up(false);
            }
            else
            {
                waferProbeAlign.waferProbeAlignParameter.DO_TopCover_Up(true);
                waferProbeAlign.waferProbeAlignParameter.DO_TopCover_Down(false);
            }
        }

        private void baseButtonTopCoverDown_Click(object sender, EventArgs e)
        {
            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (!waferProbeAlign.waferProbeAlignParameter.DI_Probe_BW_Detect())
            {
                var mb = new MessageBoxOk();
                mb.ShowDialog("Information !", "프로브 카드 트레이가 로딩 위치에 있습니다.");
                return;
            }

            if (waferProbeAlign.waferProbeAlignParameter.IsDO_TopCover_Down())
            {
                waferProbeAlign.waferProbeAlignParameter.DO_TopCover_Down(false);
            }
            else
            {
                waferProbeAlign.waferProbeAlignParameter.DO_TopCover_Down(true);
                waferProbeAlign.waferProbeAlignParameter.DO_TopCover_Up(false);
            }
        }

        private void baseButtonProbePacking_Click(object sender, EventArgs e)
        {
            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (waferProbeAlign.waferProbeAlignParameter.IsDO_Probe_Packing())
            {
                waferProbeAlign.waferProbeAlignParameter.DO_Probe_Packing(false);
            }
            else
            {
                waferProbeAlign.waferProbeAlignParameter.DO_Probe_Packing(true);
                waferProbeAlign.waferProbeAlignParameter.DO_Probe_UnPacking(false);
            }
        }

        private void baseButtonProbeUnpacking_Click(object sender, EventArgs e)
        {
            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (waferProbeAlign.waferProbeAlignParameter.IsDO_Probe_Unpacking())
            {
                waferProbeAlign.waferProbeAlignParameter.DO_Probe_UnPacking(false);
            }
            else
            {
                waferProbeAlign.waferProbeAlignParameter.DO_Probe_UnPacking(true);
                waferProbeAlign.waferProbeAlignParameter.DO_Probe_Packing(false);
            }
        }

        private void btnMainWork_Start_Click(object sender, EventArgs e)
        {
            //  Wafer Align

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (Equipment.ProbeCard_ClampType == (int)WaferProbeAlign.nProbeClampType.Type_B)       //  실내 조명이 설치되는 2호기 부터는 Start 시 실내 조명을 Off 시킨다.
            {
                CommonModule.Instance.TowerLamp.Lamp0_Off();
                CommonModule.Instance.TowerLamp.Lamp1_Off();
            }

            waferProbeAlign.m_bProbeCard_TiltCheck_Only = false;               //  ProbeCard Tilt Check Only
            waferProbeAlign.m_bWafer_Align_Only = false;                       //  Wafer Align Only

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  레티클 글래스 확인
            if (waferProbeAlign.Config.ParamConfig.ReticleGlass_CenterCheck_forAlign)
            {
                if (waferProbeAlign.m_nReticleCheck_Step_forALIGN == (int)ReticleCheck_Step.None)
                {
                    var mb1 = new MessageBoxOk();
                    mb1.ShowDialog("Information !", "PAK 카메라 - 레티클 글래스 센터 확인 작업을 진행해야 합니다.\r\n\r\n##  PAK 카드를 제거하세요.!!  ##");
                    return;
                }
                if (waferProbeAlign.m_nReticleCheck_Step_forALIGN == (int)ReticleCheck_Step.UpperCam_Complete)
                {
                    var mb1 = new MessageBoxOk();
                    mb1.ShowDialog("Information !", "웨이퍼 카메라 - 레티클 글래스 센터 확인 작업을 진행해야 합니다.");
                    return;
                }
                if (waferProbeAlign.m_nReticleCheck_Step_forALIGN != (int)ReticleCheck_Step.LowerCam_Complete)
                {
                    var mb1 = new MessageBoxOk();
                    mb1.ShowDialog("Information !", "레티클 글래스 센터 확인 작업을 진행해야 합니다.");
                    return;
                }
            }

            //  Probe Card 유무 확인
            if (!waferProbeAlign.waferProbeAlignParameter.DI_Probe_BW_Detect())
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Probe-Card 가 없습니다.");
                return;
            }

            //  Probe Card Locking 확인
            if (waferProbeAlign.waferProbeAlignParameter.DI_TopCover_Up() || !waferProbeAlign.waferProbeAlignParameter.DI_TopCover_Down())
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Probe-Card Top Cover 상태를 확인하십시오.\r\n\r\n[Top Cover Down Check]");
                return;
            }

            //  Wafer 유무 확인
            if (!waferProbeAlign.waferProbeAlignParameter.DI_Wafer_VacuumCheck())
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer Vacuum 이 감지되지 않습니다.");
                return;
            }

            //  Thin Chuck 유무 확인
            if (!waferProbeAlign.waferProbeAlignParameter.DI_ThinChuck_Detect())
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Thin-Chuck 이 감지되지 않습니다.");
                return;
            }

            //  Thin Chuck Vacuum 확인
            if (!waferProbeAlign.waferProbeAlignParameter.DI_ThinChuck_VacuumCheck())
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Thin-Chuck Vacuum 이 감지되지 않습니다.");
                return;
            }

            //  Inter-Lock
            //if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
            //    return;
            //}
            //if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
            //    return;
            //}
            if ((waferProbeAlign.m_nWaferProbeAlign_MainStep == (int)WaferProbeAlign_Step.None) && (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None))
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            //  이미지 저장소 용량 체크
            if (waferProbeAlign.Config.ParamConfig.AlignImageSaveFolder_FreeSpaceCheck_Usage)
            {
                double m_dSpace = 0.0;
                double m_dWarningSpace = 0.0;
                m_dSpace = waferProbeAlign.GetDriveSpace("D");

                m_dWarningSpace = waferProbeAlign.Config.ParamConfig.AlignImageSaveFolder_WarningSpace <= 0 ? 10.0 : waferProbeAlign.Config.ParamConfig.AlignImageSaveFolder_WarningSpace;
                if (m_dSpace <= m_dWarningSpace)
                {
                    string m_strWarningMessage;
                    m_strWarningMessage = string.Format("D 드라이브 남은 용량이 {0:0.00} GB 이하입니다. \r\n과거 얼라인 이미지 또는 불필요한 데이터를 삭제하여 공간을 확보하십시오.", m_dWarningSpace);

                    var mb1 = new MessageBoxOk();
                    mb1.ShowDialog("Information !", m_strWarningMessage);
                }
            }

            if (waferProbeAlign.m_nWaferProbeAlign_MainStep == (int)WaferProbeAlign.WaferProbeAlign_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Wafer - ProbeCard 정렬을 시작하시겠습니까?"))
                    return;

                waferProbeAlign.m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign.WaferProbeAlign_Step.Start;
                waferProbeAlign.timer_MainWork.Enabled = true;
            }
            else
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Wafer - ProbeCard 정렬을 중지하시겠습니까?"))
                    return;

                Equipment.MachineStop_byUser = true;

                waferProbeAlign.timer_MainWork.Enabled = false;
                waferProbeAlign.m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign.WaferProbeAlign_Step.None;
            }
        }

        private void btnUpperCamera_StartLive_Click(object sender, EventArgs e)
        {
            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (waferProbeAlign.Camera_Upper != null)
            {
                waferProbeAlign.Camera_Upper.StartLive();
            }

            if (waferProbeAlign.Camera_Lower != null)
            {
                waferProbeAlign.Camera_Lower.StartLive();
            }
        }

        private void btnPacking_Click(object sender, EventArgs e)
        {
            //  Wafer - ProbeCard Packing

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Probe Card 유무 확인
            if (!waferProbeAlign.waferProbeAlignParameter.DI_Probe_BW_Detect())
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Probe-Card 가 없습니다.");
                return;
            }

            //  Probe Card Locking 확인
            if (waferProbeAlign.waferProbeAlignParameter.DI_TopCover_Up() || !waferProbeAlign.waferProbeAlignParameter.DI_TopCover_Down())
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Probe-Card Top Cover 상태를 확인하십시오.\r\n\r\n[Top Cover Down Check]");
                return;
            }

            //  Wafer 유무 확인
            if (waferProbeAlign.Config.ParamConfig.Wafer_VacuumSignal_Usage && !waferProbeAlign.waferProbeAlignParameter.DI_Wafer_VacuumCheck())
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer Vacuum 이 감지되지 않습니다.");
                return;
            }

            //  Thin Chuck 유무 확인
            if (!waferProbeAlign.waferProbeAlignParameter.DI_ThinChuck_Detect())
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Thin-Chuck 이 감지되지 않습니다.");
                return;
            }

            //  Thin Chuck Vacuum 확인
            if (waferProbeAlign.Config.ParamConfig.ThinChuck_VacuumSignal_Usage && !waferProbeAlign.waferProbeAlignParameter.DI_ThinChuck_VacuumCheck())
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Thin-Chuck Vacuum 이 감지되지 않습니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            //if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
            //    return;
            //}
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            //  Manual Packing 일 경우
            if (waferProbeAlign.m_nManualPacking_Step == (int)ManualPackingStep.STEP1_OK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "수동 패킹 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nManualPacking_Step == (int)ManualPackingStep.STEP2_OK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "수동 패킹 페이지의 패킹 버튼으로 패킹 작업을 진행해야 합니다.");
                return;
            }

            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step == (int)WaferProbeAlign.WaferProbeCard_Packing_Step.None)
            {
                var mb = new MessageBoxYesNo();

                if (waferProbeAlign.m_bWafer_ThetaAlign_OK && waferProbeAlign.m_bWafer_XYAlign_OK)
                {
                    if (DialogResult.Yes != mb.ShowDialog("Question ?", "Wafer - ProbeCard Packing 을 시작하시겠습니까?"))
                        return;
                }
                else
                {
                    if (DialogResult.Yes != mb.ShowDialog("Question ?", "###  먼저 Wafer Align 을 진행해야 합니다.  ###\r\n\r\nWafer Align 을 하지 않고 Wafer - ProbeCard Packing 을 시작하시겠습니까?"))
                        return;
                }

                waferProbeAlign.m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeAlign.WaferProbeCard_Packing_Step.Start;
                waferProbeAlign.timer_MainWork.Enabled = true;
            }
            else
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Wafer - ProbeCard Packing 을 중지하시겠습니까?"))
                    return;

                waferProbeAlign.timer_MainWork.Enabled = false;
                waferProbeAlign.m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeAlign.WaferProbeCard_Packing_Step.None;
            }
        }

        private void btnLoadingPos_Wafer_GO_Click(object sender, EventArgs e)
        {
            //  Wafer Loading 위치로 이동

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  레티클 글래스 확인
            if (waferProbeAlign.Config.ParamConfig.ReticleGlass_CenterCheck_forAlign)
            {
                if (waferProbeAlign.m_nReticleCheck_Step_forALIGN == (int)ReticleCheck_Step.None)
                {
                    var mb1 = new MessageBoxOk();
                    mb1.ShowDialog("Information !", "PAK 카메라 - 레티클 글래스 센터 확인 작업을 진행해야 합니다.\r\n\r\n##  PAK 카드를 제거하세요.!!  ##");
                    return;
                }
                if (waferProbeAlign.m_nReticleCheck_Step_forALIGN == (int)ReticleCheck_Step.UpperCam_Complete)
                {
                    var mb1 = new MessageBoxOk();
                    mb1.ShowDialog("Information !", "웨이퍼 카메라 - 레티클 글래스 센터 확인 작업을 진행해야 합니다.");
                    return;
                }
                if (waferProbeAlign.m_nReticleCheck_Step_forALIGN != (int)ReticleCheck_Step.LowerCam_Complete)
                {
                    var mb1 = new MessageBoxOk();
                    mb1.ShowDialog("Information !", "레티클 글래스 센터 확인 작업을 진행해야 합니다.");
                    return;
                }
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            //if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
            //    return;
            //}
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            if (waferProbeAlign.m_nWafer_Loading_Ready_Step == (int)WaferProbeAlign.WaferLoading_Ready_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Wafer 로딩 대기 위치로 이동하시겠습니까?"))
                    return;

                waferProbeAlign.m_nWafer_Loading_Ready_Step = (int)WaferProbeAlign.WaferLoading_Ready_Step.Start;
                waferProbeAlign.timer_SubWork.Enabled = true;
            }
            else
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Wafer 로딩 대기 위치로 이동을 중지하시겠습니까?"))
                    return;

                waferProbeAlign.timer_SubWork.Enabled = false;
                waferProbeAlign.m_nWafer_Loading_Ready_Step = (int)WaferProbeAlign.WaferLoading_Ready_Step.None;
            }
        }

        private void btnLoadingPos_ProbeCard_GO_Click(object sender, EventArgs e)
        {
            //  Probe-Card Loading 위치로 이동

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            //if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
            //    return;
            //}
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step == (int)WaferProbeAlign.ProbeCard_Loading_Ready_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Probe-Card 로딩 대기 위치로 이동하시겠습니까?"))
                    return;

                waferProbeAlign.m_nProbeCard_Loading_Ready_Step = (int)WaferProbeAlign.ProbeCard_Loading_Ready_Step.Start;
                waferProbeAlign.timer_SubWork.Enabled = true;
            }
            else
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Probe-Card 로딩 대기 위치로 이동을 중지하시겠습니까?"))
                    return;

                waferProbeAlign.timer_SubWork.Enabled = false;
                waferProbeAlign.m_nProbeCard_Loading_Ready_Step = (int)WaferProbeAlign.ProbeCard_Loading_Ready_Step.None;
            }
        }

        private void btn_ProbeCard_Locking_Click(object sender, EventArgs e)
        {
            //  Probe-Card Locking

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            //if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
            //    return;
            //}
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            if (waferProbeAlign.m_nProbeCard_Locking_Step == (int)WaferProbeAlign.ProbeCard_Locking_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Probe-Card 고정 작업을 진행하시겠습니까?"))
                    return;

                waferProbeAlign.m_nProbeCard_Locking_Step = (int)WaferProbeAlign.ProbeCard_Locking_Step.Start;
                waferProbeAlign.timer_SubWork.Enabled = true;
            }
            else
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Probe-Card 고정 작업을 중지하시겠습니까?"))
                    return;

                waferProbeAlign.timer_SubWork.Enabled = false;
                waferProbeAlign.m_nProbeCard_Locking_Step = (int)WaferProbeAlign.ProbeCard_Locking_Step.None;
            }
        }

        private void btnSafetyPos_CamXY_GO_Click(object sender, EventArgs e)
        {
            //  Camera XY 안전위치로 이동

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            //if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
            //    return;
            //}
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            if (waferProbeAlign.m_nSafetyPos_Move_Step == (int)WaferProbeAlign.SafetyPos_Move_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "안전 위치로 이동하시겠습니까?"))
                    return;

                waferProbeAlign.m_nSafetyPos_Move_Step = (int)WaferProbeAlign.SafetyPos_Move_Step.Start;
                waferProbeAlign.timer_SubWork.Enabled = true;
            }
            else
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "안전 위치로 이동 동작을 중지하시겠습니까?"))
                    return;

                waferProbeAlign.timer_SubWork.Enabled = false;
                waferProbeAlign.m_nSafetyPos_Move_Step = (int)WaferProbeAlign.SafetyPos_Move_Step.None;
            }
        }

        private void btnReticlePos_UpperCam_GO_Click(object sender, EventArgs e)
        {
            //  상부 카메라로 Reticle Glass 를 확인하는 위치로 이동

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            //if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
            //    return;
            //}
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step == (int)WaferProbeAlign.ReticleCheck_UpperCam_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Reticle Glass 확인 위치로 이동하시겠습니까?\r\n\r\n[Upper Camera]\r\n\r\n##  프로브 카드는 반드시 제거해야 합니다. [충돌 경고]  ##"))
                    return;

                waferProbeAlign.m_nReticleCheck_UpperCam_Step = (int)WaferProbeAlign.ReticleCheck_UpperCam_Step.Start;
                waferProbeAlign.timer_ReticleGlass_Check.Enabled = true;
            }
            else
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Reticle Glass 확인 위치로 이동을 중지하시겠습니까?\r\n\r\n[Upper Camera]"))
                    return;

                waferProbeAlign.timer_ReticleGlass_Check.Enabled = false;
                waferProbeAlign.m_nReticleCheck_UpperCam_Step = (int)WaferProbeAlign.ReticleCheck_UpperCam_Step.None;
            }
        }

        private void btnReticlePos_LowerCam_GO_Click(object sender, EventArgs e)
        {
            //  하부 카메라로 Reticle Glass 를 확인하는 위치로 이동

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            //if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
            //    return;
            //}
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step == (int)WaferProbeAlign.ReticleCheck_LowerCam_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Reticle Glass 확인 위치로 이동하시겠습니까?\r\n\r\n[Lower Camera]"))
                    return;

                waferProbeAlign.m_nReticleCheck_LowerCam_Step = (int)WaferProbeAlign.ReticleCheck_LowerCam_Step.Start;
                waferProbeAlign.timer_ReticleGlass_Check.Enabled = true;
            }
            else
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Reticle Glass 확인 위치로 이동을 중지하시겠습니까?\r\n\r\n[Lower Camera]"))
                    return;

                waferProbeAlign.timer_ReticleGlass_Check.Enabled = false;
                waferProbeAlign.m_nReticleCheck_LowerCam_Step = (int)WaferProbeAlign.ReticleCheck_LowerCam_Step.None;
            }
        }

        private void btnWaferProbeCardUnpackingReady_Click(object sender, EventArgs e)
        {
            //  Wafer, Probe-Card Unpacking 대기 위치로 이동

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            //if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
            //    return;
            //}
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step == (int)WaferProbeAlign.WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Wafer, Probe-Card Unpacking 대기 위치로 이동하시겠습니까?"))
                    return;

                waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeAlign.WaferProbeCard_Unpacking_Ready_Step.Start;
                waferProbeAlign.timer_SubWork.Enabled = true;
            }
            else
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Wafer, Probe-Card Unpacking 대기 위치로 이동을 중지하시겠습니까?"))
                    return;

                waferProbeAlign.timer_SubWork.Enabled = false;
                waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step = (int)WaferProbeAlign.WaferProbeCard_Unpacking_Ready_Step.None;
            }
        }

        private void btnWaferProbeCardUnpacking_Click(object sender, EventArgs e)
        {
            //  Wafer, Probe-Card Unpacking

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            //if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
            //    return;
            //}
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step == (int)WaferProbeAlign.WaferProbeCard_Unpacking_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Wafer, Probe-Card Unpacking 작업을 진행하시겠습니까?"))
                    return;

                waferProbeAlign.m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeAlign.WaferProbeCard_Unpacking_Step.Start;
                waferProbeAlign.timer_MainWork.Enabled = true;
            }
            else
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Wafer, Probe-Card Unpacking 작업을 중지하시겠습니까?"))
                    return;

                waferProbeAlign.timer_MainWork.Enabled = false;
                waferProbeAlign.m_nWaferProbeCard_Unpacking_Step = (int)WaferProbeAlign.WaferProbeCard_Unpacking_Step.None;
            }
        }

        private void baseButton_Y_Pos_GO1_Click(object sender, EventArgs e)
        {
            //  얼라인 확인위치 이동 (TOP)

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            if (waferProbeAlign.Camera_Upper != null)
            {
                waferProbeAlign.Camera_Upper.StartLive();
                waferProbeAlign.visionCalibrator_Upper.Illuminator.SetVolume(waferProbeAlign.Config.ParamConfig.Align_UpperVision_LightValue, 1);
                waferProbeAlign.visionCalibrator_Upper.Illuminator.TurnOnOff(true, 1);       //  Probe Card 조명
            }

            if (waferProbeAlign.Camera_Lower != null)
            {
                waferProbeAlign.Camera_Lower.StartLive();
                waferProbeAlign.visionCalibrator_Upper.Illuminator.SetVolume(waferProbeAlign.Config.ParamConfig.Align_LowerVision_LightValue, 2);
                waferProbeAlign.visionCalibrator_Upper.Illuminator.TurnOnOff(true, 2);       //  Wafer 조명
            }

            double lfTargetPos_Y = 0.0;

            double lfVelocity = 0.0;
            double lfAccDec = 0.0f;

            int m_nIndex = -1;

            for (int i = 0; i < waferProbeAlign.Config.Positions.Count; i++)
            {
                //  웨이퍼 얼라인 상태를 확인하는 위치 (TOP)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Ver_Top")
                {
                    m_nIndex = i;
                    break;
                }
            }

            if (m_nIndex != -1)
            {
                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Top");

                if (Equipment.AjinBoard_Opened)
                {
                    var mb = new MessageBoxYesNo();
                    if (DialogResult.Yes != mb.ShowDialog("Question ?", "Vision Y축을 TOP 위치로 보내시겠습니까?"))
                        return;

                    lfTargetPos_Y = waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y];

                    lfVelocity = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.Y.ToString()].Configuration.Velocity;
                    lfAccDec = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.Y.ToString()].Configuration.Acceleration;

                    //if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                    //{
                    //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                    //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY,
                    //                            lfTargetPos_Y);

                    //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                    //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX,
                    //                            lfTargetPos_X);
                    //}

                    //  Vision XY 축 이동 시 Elev. Z 축과 충돌하는지 체크
                    if (MC_Func.MC_GetEncPos((int)nAxis.EZ) >= waferProbeAlign.Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign)
                    {
                        MessageBox.Show("Elev. Z 축이 Vision XY 축과 충돌 위치에 있습니다.", "Warning!!");
                    }
                    else if (waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.EZ] >= waferProbeAlign.Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign)
                    {
                        MessageBox.Show("Elev. Z 축이 Vision XY 축과 충돌하는 위치로 이동하려고 하였습니다.", "Warning!!");
                    }
                    else
                    {
                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X, waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X], lfVelocity, lfAccDec, lfAccDec);
                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y, lfTargetPos_Y, lfVelocity, lfAccDec, lfAccDec);

                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.EZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.EZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.EZ]);
                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.VZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.VZ],
                                            waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.VZ]);
                    }
                }
                else
                {
                    var mb1 = new MessageBoxOk();
                    mb1.ShowDialog("Information !", "Ajin 제어기가 연결되지 않았습니다.");
                    return;
                }
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Config 창에 \"AlignPosition_Ver_Top\" 항목이 없습니다.\r\n\r\n(이 메세지가 보이면 안됨)");
                return;
            }
        }

        private void baseButton_Y_Pos_GO2_Click(object sender, EventArgs e)
        {
            //  얼라인 확인위치 이동 (MID)

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            if (waferProbeAlign.Camera_Upper != null)
            {
                waferProbeAlign.Camera_Upper.StartLive();
                waferProbeAlign.visionCalibrator_Upper.Illuminator.SetVolume(waferProbeAlign.Config.ParamConfig.Align_UpperVision_LightValue, 1);
                waferProbeAlign.visionCalibrator_Upper.Illuminator.TurnOnOff(true, 1);       //  Probe Card 조명
            }

            if (waferProbeAlign.Camera_Lower != null)
            {
                waferProbeAlign.Camera_Lower.StartLive();
                waferProbeAlign.visionCalibrator_Upper.Illuminator.SetVolume(waferProbeAlign.Config.ParamConfig.Align_LowerVision_LightValue, 2);
                waferProbeAlign.visionCalibrator_Upper.Illuminator.TurnOnOff(true, 2);       //  Wafer 조명
            }

            double lfTargetPos_Y = 0.0;

            double lfVelocity = 0.0;
            double lfAccDec = 0.0f;

            int m_nIndex = -1;

            for (int i = 0; i < waferProbeAlign.Config.Positions.Count; i++)
            {
                //  웨이퍼 얼라인 상태를 확인하는 위치 (MID)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Ver_Middle")
                {
                    m_nIndex = i;
                    break;
                }
            }

            if (m_nIndex != -1)
            {
                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Middle");

                if (Equipment.AjinBoard_Opened)
                {
                    var mb = new MessageBoxYesNo();
                    if (DialogResult.Yes != mb.ShowDialog("Question ?", "Vision Y축을 MID 위치로 보내시겠습니까?"))
                        return;

                    lfTargetPos_Y = waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y];

                    lfVelocity = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.Y.ToString()].Configuration.Velocity;
                    lfAccDec = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.Y.ToString()].Configuration.Acceleration;

                    //if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                    //{
                    //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                    //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY,
                    //                            lfTargetPos_Y);

                    //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                    //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX,
                    //                            lfTargetPos_X);
                    //}

                    //  Vision XY 축 이동 시 Elev. Z 축과 충돌하는지 체크
                    if (MC_Func.MC_GetEncPos((int)nAxis.EZ) >= waferProbeAlign.Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign)
                    {
                        MessageBox.Show("Elev. Z 축이 Vision XY 축과 충돌 위치에 있습니다.", "Warning!!");
                    }
                    else if (waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.EZ] >= waferProbeAlign.Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign)
                    {
                        MessageBox.Show("Elev. Z 축이 Vision XY 축과 충돌하는 위치로 이동하려고 하였습니다.", "Warning!!");
                    }
                    else
                    {
                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X, waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X], lfVelocity, lfAccDec, lfAccDec);
                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y, lfTargetPos_Y, lfVelocity, lfAccDec, lfAccDec);

                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ],
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.EZ],
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.EZ],
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.EZ]);
                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ],
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.VZ],
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.VZ],
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.VZ]);
                    }
                }
                else
                {
                    var mb1 = new MessageBoxOk();
                    mb1.ShowDialog("Information !", "Ajin 제어기가 연결되지 않았습니다.");
                    return;
                }
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Config 창에 \"AlignPosition_Ver_Middle\" 항목이 없습니다.\r\n\r\n(이 메세지가 보이면 안됨)");
                return;
            }
        }

        private void baseButton_Y_Pos_GO3_Click(object sender, EventArgs e)
        {
            //  얼라인 확인위치 이동 (BOT)

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            if (waferProbeAlign.Camera_Upper != null)
            {
                waferProbeAlign.Camera_Upper.StartLive();
                waferProbeAlign.visionCalibrator_Upper.Illuminator.SetVolume(waferProbeAlign.Config.ParamConfig.Align_UpperVision_LightValue, 1);
                waferProbeAlign.visionCalibrator_Upper.Illuminator.TurnOnOff(true, 1);       //  Probe Card 조명
            }

            if (waferProbeAlign.Camera_Lower != null)
            {
                waferProbeAlign.Camera_Lower.StartLive();
                waferProbeAlign.visionCalibrator_Upper.Illuminator.SetVolume(waferProbeAlign.Config.ParamConfig.Align_LowerVision_LightValue, 2);
                waferProbeAlign.visionCalibrator_Upper.Illuminator.TurnOnOff(true, 2);       //  Wafer 조명
            }

            double lfTargetPos_Y = 0.0;

            double lfVelocity = 0.0;
            double lfAccDec = 0.0f;

            int m_nIndex = -1;

            for (int i = 0; i < waferProbeAlign.Config.Positions.Count; i++)
            {
                //  웨이퍼 얼라인 상태를 확인하는 위치 (BOT)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Ver_Bottom")
                {
                    m_nIndex = i;
                    break;
                }
            }

            if (m_nIndex != -1)
            {
                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Bottom");
                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam2 = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("AlignPosition_Ver_Top");

                if (Equipment.AjinBoard_Opened)
                {
                    var mb = new MessageBoxYesNo();
                    if (DialogResult.Yes != mb.ShowDialog("Question ?", "Vision Y축을 BOT 위치로 보내시겠습니까?"))
                        return;

                    lfTargetPos_Y = waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.Y];

                    lfVelocity = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.Y.ToString()].Configuration.Velocity;
                    lfAccDec = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.Y.ToString()].Configuration.Acceleration;

                    //if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                    //{
                    //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                    //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY,
                    //                            lfTargetPos_Y);

                    //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                    //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX,
                    //                            lfTargetPos_X);
                    //}

                    //  Vision XY 축 이동 시 Elev. Z 축과 충돌하는지 체크
                    if (MC_Func.MC_GetEncPos((int)nAxis.EZ) >= waferProbeAlign.Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign)
                    {
                        MessageBox.Show("Elev. Z 축이 Vision XY 축과 충돌 위치에 있습니다.", "Warning!!");
                    }
                    else if (waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.EZ] >= waferProbeAlign.Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign)
                    {
                        MessageBox.Show("Elev. Z 축이 Vision XY 축과 충돌하는 위치로 이동하려고 하였습니다.", "Warning!!");
                    }
                    else
                    {
                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X, waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam2.dTarget[(int)nAxis.X], lfVelocity, lfAccDec, lfAccDec);
                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.Y, lfTargetPos_Y, lfVelocity, lfAccDec, lfAccDec);

                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ],
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.EZ],
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.EZ],
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.EZ]);
                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ],
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.VZ],
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.VZ],
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.VZ]);
                    }
                }
                else
                {
                    var mb1 = new MessageBoxOk();
                    mb1.ShowDialog("Information !", "Ajin 제어기가 연결되지 않았습니다.");
                    return;
                }
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Config 창에 \"AlignPosition_Ver_Bottom\" 항목이 없습니다.\r\n\r\n(이 메세지가 보이면 안됨)");
                return;
            }
        }

        private void baseButton_X_Pos_GO1_Click(object sender, EventArgs e)
        {
            //  얼라인 확인위치 이동 (LEFT)

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            if (waferProbeAlign.Camera_Upper != null)
            {
                waferProbeAlign.Camera_Upper.StartLive();
                waferProbeAlign.visionCalibrator_Upper.Illuminator.SetVolume(waferProbeAlign.Config.ParamConfig.Align_UpperVision_LightValue, 1);
                waferProbeAlign.visionCalibrator_Upper.Illuminator.TurnOnOff(true, 1);       //  Probe Card 조명
            }

            if (waferProbeAlign.Camera_Lower != null)
            {
                waferProbeAlign.Camera_Lower.StartLive();
                waferProbeAlign.visionCalibrator_Upper.Illuminator.SetVolume(waferProbeAlign.Config.ParamConfig.Align_LowerVision_LightValue, 2);
                waferProbeAlign.visionCalibrator_Upper.Illuminator.TurnOnOff(true, 2);       //  Wafer 조명
            }

            double lfTargetPos_X = 0.0;

            double lfVelocity = 0.0;
            double lfAccDec = 0.0f;

            int m_nIndex = -1;

            for (int i = 0; i < waferProbeAlign.Config.Positions.Count; i++)
            {
                //  웨이퍼 얼라인 상태를 확인하는 위치 (LEFT)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Hor_Left")
                {
                    m_nIndex = i;
                    break;
                }
            }

            if (m_nIndex != -1)
            {
                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("AlignPosition_Hor_Left");

                if (Equipment.AjinBoard_Opened)
                {
                    var mb = new MessageBoxYesNo();
                    if (DialogResult.Yes != mb.ShowDialog("Question ?", "Vision X축을 LEFT 위치로 보내시겠습니까?"))
                        return;

                    lfTargetPos_X = waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X];

                    lfVelocity = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.X.ToString()].Configuration.Velocity;
                    lfAccDec = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.X.ToString()].Configuration.Acceleration;

                    //if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                    //{
                    //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                    //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY,
                    //                            lfTargetPos_Y);

                    //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                    //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX,
                    //                            lfTargetPos_X);
                    //}

                    //  Vision XY 축 이동 시 Elev. Z 축과 충돌하는지 체크
                    if (MC_Func.MC_GetEncPos((int)nAxis.EZ) >= waferProbeAlign.Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign)
                    {
                        MessageBox.Show("Elev. Z 축이 Vision XY 축과 충돌 위치에 있습니다.", "Warning!!");
                    }
                    else if (waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.EZ] >= waferProbeAlign.Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign)
                    {
                        MessageBox.Show("Elev. Z 축이 Vision XY 축과 충돌하는 위치로 이동하려고 하였습니다.", "Warning!!");
                    }
                    else
                    {
                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X, lfTargetPos_X, lfVelocity, lfAccDec, lfAccDec);

                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ],
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.EZ],
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.EZ],
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.EZ]);
                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ],
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.VZ],
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.VZ],
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.VZ]);
                    }
                }
                else
                {
                    var mb1 = new MessageBoxOk();
                    mb1.ShowDialog("Information !", "Ajin 제어기가 연결되지 않았습니다.");
                    return;
                }
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Config 창에 \"AlignPosition_Hor_Left\" 항목이 없습니다.\r\n\r\n(이 메세지가 보이면 안됨)");
                return;
            }
        }

        private void baseButton_X_Pos_GO2_Click(object sender, EventArgs e)
        {
            //  얼라인 확인위치 이동 (CENTER)

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            if (waferProbeAlign.Camera_Upper != null)
            {
                waferProbeAlign.Camera_Upper.StartLive();
                waferProbeAlign.visionCalibrator_Upper.Illuminator.SetVolume(waferProbeAlign.Config.ParamConfig.Align_UpperVision_LightValue, 1);
                waferProbeAlign.visionCalibrator_Upper.Illuminator.TurnOnOff(true, 1);       //  Probe Card 조명
            }

            if (waferProbeAlign.Camera_Lower != null)
            {
                waferProbeAlign.Camera_Lower.StartLive();
                waferProbeAlign.visionCalibrator_Upper.Illuminator.SetVolume(waferProbeAlign.Config.ParamConfig.Align_LowerVision_LightValue, 2);
                waferProbeAlign.visionCalibrator_Upper.Illuminator.TurnOnOff(true, 2);       //  Wafer 조명
            }

            double lfTargetPos_X = 0.0;

            double lfVelocity = 0.0;
            double lfAccDec = 0.0f;

            int m_nIndex = -1;

            for (int i = 0; i < waferProbeAlign.Config.Positions.Count; i++)
            {
                //  웨이퍼 얼라인 상태를 확인하는 위치 (CENTER)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Hor_Center")
                {
                    m_nIndex = i;
                    break;
                }
            }

            if (m_nIndex != -1)
            {
                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("AlignPosition_Hor_Center");

                if (Equipment.AjinBoard_Opened)
                {
                    var mb = new MessageBoxYesNo();
                    if (DialogResult.Yes != mb.ShowDialog("Question ?", "Vision X축을 CENTER 위치로 보내시겠습니까?"))
                        return;

                    lfTargetPos_X = waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X];

                    lfVelocity = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.X.ToString()].Configuration.Velocity;
                    lfAccDec = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.X.ToString()].Configuration.Acceleration;

                    //if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                    //{
                    //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                    //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY,
                    //                            lfTargetPos_Y);

                    //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                    //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX,
                    //                            lfTargetPos_X);
                    //}

                    //  Vision XY 축 이동 시 Elev. Z 축과 충돌하는지 체크
                    if (MC_Func.MC_GetEncPos((int)nAxis.EZ) >= waferProbeAlign.Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign)
                    {
                        MessageBox.Show("Elev. Z 축이 Vision XY 축과 충돌 위치에 있습니다.", "Warning!!");
                    }
                    else if (waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.EZ] >= waferProbeAlign.Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign)
                    {
                        MessageBox.Show("Elev. Z 축이 Vision XY 축과 충돌하는 위치로 이동하려고 하였습니다.", "Warning!!");
                    }
                    else
                    {
                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X, lfTargetPos_X, lfVelocity, lfAccDec, lfAccDec);

                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ],
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.EZ],
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.EZ],
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.EZ]);
                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ],
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.VZ],
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.VZ],
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.VZ]);
                    }
                }
                else
                {
                    var mb1 = new MessageBoxOk();
                    mb1.ShowDialog("Information !", "Ajin 제어기가 연결되지 않았습니다.");
                    return;
                }
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Config 창에 \"AlignPosition_Hor_Center\" 항목이 없습니다.\r\n\r\n(이 메세지가 보이면 안됨)");
                return;
            }
        }

        private void baseButton_X_Pos_GO3_Click(object sender, EventArgs e)
        {
            //  얼라인 확인위치 이동 (RIGHT)

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            if (waferProbeAlign.Camera_Upper != null)
            {
                waferProbeAlign.Camera_Upper.StartLive();
                waferProbeAlign.visionCalibrator_Upper.Illuminator.SetVolume(waferProbeAlign.Config.ParamConfig.Align_UpperVision_LightValue, 1);
                waferProbeAlign.visionCalibrator_Upper.Illuminator.TurnOnOff(true, 1);       //  Probe Card 조명
            }

            if (waferProbeAlign.Camera_Lower != null)
            {
                waferProbeAlign.Camera_Lower.StartLive();
                waferProbeAlign.visionCalibrator_Upper.Illuminator.SetVolume(waferProbeAlign.Config.ParamConfig.Align_LowerVision_LightValue, 2);
                waferProbeAlign.visionCalibrator_Upper.Illuminator.TurnOnOff(true, 2);       //  Wafer 조명
            }

            double lfTargetPos_X = 0.0;

            double lfVelocity = 0.0;
            double lfAccDec = 0.0f;

            int m_nIndex = -1;

            for (int i = 0; i < waferProbeAlign.Config.Positions.Count; i++)
            {
                //  웨이퍼 얼라인 상태를 확인하는 위치 (CENTER)
                if (waferProbeAlign.Config.Positions[i].Name == "AlignPosition_Hor_Right")
                {
                    m_nIndex = i;
                    break;
                }
            }

            if (m_nIndex != -1)
            {
                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("AlignPosition_Hor_Right");

                if (Equipment.AjinBoard_Opened)
                {
                    var mb = new MessageBoxYesNo();
                    if (DialogResult.Yes != mb.ShowDialog("Question ?", "Vision X축을 RIGHT 위치로 보내시겠습니까?"))
                        return;

                    lfTargetPos_X = waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.X];

                    lfVelocity = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.X.ToString()].Configuration.Velocity;
                    lfAccDec = waferProbeAlign.waferProbeAlignParameter.Axes[WaferProbeAlignParameter.MotionKey.X.ToString()].Configuration.Acceleration;

                    //if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                    //{
                    //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                    //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageY,
                    //                            lfTargetPos_Y);

                    //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,
                    //                            (Axis)WaferProbeAlignParameter.AxisAcsEnum.StageX,
                    //                            lfTargetPos_X);
                    //}

                    //  Vision XY 축 이동 시 Elev. Z 축과 충돌하는지 체크
                    if (MC_Func.MC_GetEncPos((int)nAxis.EZ) >= waferProbeAlign.Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign)
                    {
                        MessageBox.Show("Elev. Z 축이 Vision XY 축과 충돌 위치에 있습니다.", "Warning!!");
                    }
                    else if (waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)WaferProbeAlign.nAxis.EZ] >= waferProbeAlign.Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign)
                    {
                        MessageBox.Show("Elev. Z 축이 Vision XY 축과 충돌하는 위치로 이동하려고 하였습니다.", "Warning!!");
                    }
                    else
                    {
                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.X, lfTargetPos_X, lfVelocity, lfAccDec, lfAccDec);

                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.EZ],
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.EZ],
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.EZ],
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.EZ]);
                        MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.VZ,
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dTarget[(int)nAxis.VZ],
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dVel[(int)nAxis.VZ],
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dAcc[(int)nAxis.VZ],
                                                waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.dDec[(int)nAxis.VZ]);
                    }
                }
                else
                {
                    var mb1 = new MessageBoxOk();
                    mb1.ShowDialog("Information !", "Ajin 제어기가 연결되지 않았습니다.");
                    return;
                }
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Config 창에 \"AlignPosition_Hor_Right\" 항목이 없습니다.\r\n\r\n(이 메세지가 보이면 안됨)");
                return;
            }
        }

        private void btnProbeCardAlignOnly_Start_Click(object sender, EventArgs e)
        {
            //  Probe Card Align Only

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            waferProbeAlign.m_bProbeCard_TiltCheck_Only = true;               //  ProbeCard Tilt Check Only
            waferProbeAlign.m_bWafer_Align_Only = false;                       //  Wafer Align Only

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Probe Card 유무 확인
            if (!waferProbeAlign.waferProbeAlignParameter.DI_Probe_BW_Detect())
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Probe-Card 가 없습니다.");
                return;
            }

            //  Probe Card Locking 확인
            if (waferProbeAlign.waferProbeAlignParameter.DI_TopCover_Up() || !waferProbeAlign.waferProbeAlignParameter.DI_TopCover_Down())
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Probe-Card Top Cover 상태를 확인하십시오.\r\n\r\n[Top Cover Down Check]");
                return;
            }

            //  Inter-Lock
            //if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
            //    return;
            //}
            //if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
            //    return;
            //}
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            if (waferProbeAlign.m_nWaferProbeAlign_MainStep == (int)WaferProbeAlign.WaferProbeAlign_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "ProbeCard 정렬 상태를 확인하시겠습니까?"))
                    return;

                waferProbeAlign.m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign.WaferProbeAlign_Step.Start;
                waferProbeAlign.timer_MainWork.Enabled = true;
            }
            else
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "ProbeCard 정렬 상태를 확인을 중지하시겠습니까?"))
                    return;

                Equipment.MachineStop_byUser = true;

                waferProbeAlign.timer_MainWork.Enabled = false;
                waferProbeAlign.m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign.WaferProbeAlign_Step.None;
            }
        }

        private void btnWaferAlignOnly_Start_Click(object sender, EventArgs e)
        {
            //  Wafer Align Only

            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            waferProbeAlign.m_bProbeCard_TiltCheck_Only = false;               //  ProbeCard Tilt Check Only
            waferProbeAlign.m_bWafer_Align_Only = true;                        //  Wafer Align Only

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Wafer 유무 확인
            if (!waferProbeAlign.waferProbeAlignParameter.DI_Wafer_VacuumCheck())
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer Vacuum 이 감지되지 않습니다.");
                return;
            }

            //  Thin Chuck 유무 확인
            if (!waferProbeAlign.waferProbeAlignParameter.DI_ThinChuck_Detect())
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Thin-Chuck 이 감지되지 않습니다.");
                return;
            }

            //  Thin Chuck Vacuum 확인
            if (!waferProbeAlign.waferProbeAlignParameter.DI_ThinChuck_VacuumCheck())
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Thin-Chuck Vacuum 이 감지되지 않습니다.");
                return;
            }

            //  Inter-Lock
            //if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
            //    return;
            //}
            //if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
            //    return;
            //}
            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            if (waferProbeAlign.m_nWaferProbeAlign_MainStep == (int)WaferProbeAlign.WaferProbeAlign_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Wafer 정렬을 시작하시겠습니까?"))
                    return;

                waferProbeAlign.m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign.WaferProbeAlign_Step.Start;
                waferProbeAlign.timer_MainWork.Enabled = true;
            }
            else
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Wafer 정렬을 중지하시겠습니까?"))
                    return;

                Equipment.MachineStop_byUser = true;

                waferProbeAlign.timer_MainWork.Enabled = false;
                waferProbeAlign.m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign.WaferProbeAlign_Step.None;
            }
        }

        private void btnAlignErrorCheck_Click(object sender, EventArgs e)
        {
            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (Equipment.ProbeCard_ClampType == (int)WaferProbeAlign.nProbeClampType.Type_B)       //  실내 조명이 설치되는 2호기 부터는 Start 시 실내 조명을 Off 시킨다.
            {
                CommonModule.Instance.TowerLamp.Lamp0_Off();
                CommonModule.Instance.TowerLamp.Lamp1_Off();
            }

            if (!waferProbeAlign.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  Probe Card 유무 확인
            if (!waferProbeAlign.waferProbeAlignParameter.DI_Probe_BW_Detect())
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Probe-Card 가 없습니다.");
                return;
            }

            //  Probe Card Locking 확인
            if (waferProbeAlign.waferProbeAlignParameter.DI_TopCover_Up() || !waferProbeAlign.waferProbeAlignParameter.DI_TopCover_Down())
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Probe-Card Top Cover 상태를 확인하십시오.\r\n\r\n[Top Cover Down Check]");
                return;
            }

            //  Wafer 유무 확인
            if (waferProbeAlign.Config.ParamConfig.Wafer_VacuumSignal_Usage && !waferProbeAlign.waferProbeAlignParameter.DI_Wafer_VacuumCheck())
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer Vacuum 이 감지되지 않습니다.");
                return;
            }

            //  Thin Chuck 유무 확인
            if (!waferProbeAlign.waferProbeAlignParameter.DI_ThinChuck_Detect())
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Thin-Chuck 이 감지되지 않습니다.");
                return;
            }

            //  Thin Chuck Vacuum 확인
            if (waferProbeAlign.Config.ParamConfig.ThinChuck_VacuumSignal_Usage && !waferProbeAlign.waferProbeAlignParameter.DI_ThinChuck_VacuumCheck())
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Thin-Chuck Vacuum 이 감지되지 않습니다.");
                return;
            }

            //  Inter-Lock
            if (waferProbeAlign.m_nWaferProbeAlign_MainStep != (int)WaferProbeAlign_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 작업 진행중입니다.");
                return;
            }
            //if (waferProbeAlign.m_nFindAlignMark_Step != (int)FindAlignMark_Step.None)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "마크를 찾는 중입니다.");
            //    return;
            //}
            //if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step != (int)WaferProbeAlignErrorCheck_Step.None)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "Wafer - ProbeCard 정렬 상태 확인중입니다.");
            //    return;
            //}
            if (waferProbeAlign.m_nReticleCheck_UpperCam_Step != (int)ReticleCheck_UpperCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "상부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nReticleCheck_LowerCam_Step != (int)ReticleCheck_LowerCam_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "하부 카메라 레티클 글래스 센터 확인 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nSafetyPos_Move_Step != (int)SafetyPos_Move_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "안전 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_Loading_Ready_Step != (int)WaferLoading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step != (int)WaferProbeCard_Packing_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Step != (int)WaferProbeCard_Unpacking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nWaferProbeCard_Unpacking_Ready_Step != (int)WaferProbeCard_Unpacking_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "웨이퍼 - 프로브 카드 언패킹 준비 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Locking_Step != (int)ProbeCard_Locking_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 고정 작업 진행중입니다.");
                return;
            }
            if (waferProbeAlign.m_nProbeCard_Loading_Ready_Step != (int)ProbeCard_Loading_Ready_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "프로브 카드 로딩 위치로 이동중입니다.");
                return;
            }
            if (waferProbeAlign.m_nPAK_AirLine_Check_Step != (int)PAK_AirLine_Check_Step.None)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PAK 공압 관로 상태 확인 작업 진행중입니다.");
                return;
            }

            if (waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step == (int)WaferProbeAlign.WaferProbeAlignErrorCheck_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Wafer - ProbeCard 얼라인 에러 검사를 시작하시겠습니까?"))
                    return;

                waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlign.WaferProbeAlignErrorCheck_Step.Start;
                waferProbeAlign.timer_SubWork.Enabled = true;
            }
            else
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Wafer - ProbeCard 얼라인 에러 검사를 중지하시겠습니까?"))
                    return;

                Equipment.MachineStop_byUser = true;

                waferProbeAlign.timer_SubWork.Enabled = false;
                waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Step = (int)WaferProbeAlign.WaferProbeAlignErrorCheck_Step.None;
            }
        }

        private void baseButtonProbeClamp_Down_Click(object sender, EventArgs e)
        {
            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (waferProbeAlign.waferProbeAlignParameter.IsDO_Probe_ClampModule_Down())
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "프로브 카드 고정 실린더 올림");

                waferProbeAlign.waferProbeAlignParameter.DO_ProbeClampModule_Down(false);
            }
            else
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "프로브 카드 고정 실린더 내림");

                waferProbeAlign.waferProbeAlignParameter.DO_ProbeClampModule_Down(true);
            }
        }

        private void baseButtonProbeClamp_FW_Click(object sender, EventArgs e)
        {
            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (waferProbeAlign.waferProbeAlignParameter.IsDO_Probe_ClampModule_Down())
            {
                var mb = new MessageBoxOk();
                mb.ShowDialog("Information !", "프로브 카드 클램프가 내려와 있습니다.");
                return;
            }

            if (waferProbeAlign.waferProbeAlignParameter.DI_Probe_UnpackingCyl_Down() || !waferProbeAlign.waferProbeAlignParameter.DI_Probe_UnpackingCyl_Up())
            {
                var mb = new MessageBoxOk();
                mb.ShowDialog("Information !", "프로브 카드 언패킹 실린더가 내려와 있습니다.");
                return;
            }

            if (waferProbeAlign.waferProbeAlignParameter.IsDO_Probe_ClampModule_FW())
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "프로브 카드 클램프 실린더 닫기 신호 Off");

                waferProbeAlign.waferProbeAlignParameter.DO_ProbeClampModule_FW(false);
            }
            else
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "프로브 카드 클램프 실린더 닫기");

                waferProbeAlign.waferProbeAlignParameter.DO_ProbeClampModule_FW(true);
                waferProbeAlign.waferProbeAlignParameter.DO_ProbeClampModule_BW(false);
            }
        }

        private void baseButtonProbeClamp_BW_Click(object sender, EventArgs e)
        {
            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (waferProbeAlign.waferProbeAlignParameter.IsDO_Probe_ClampModule_Down())
            {
                var mb = new MessageBoxOk();
                mb.ShowDialog("Information !", "프로브 카드 클램프가 내려와 있습니다.");
                return;
            }

            if (waferProbeAlign.waferProbeAlignParameter.DI_Probe_UnpackingCyl_Down() || !waferProbeAlign.waferProbeAlignParameter.DI_Probe_UnpackingCyl_Up())
            {
                var mb = new MessageBoxOk();
                mb.ShowDialog("Information !", "프로브 카드 언패킹 실린더가 내려와 있습니다.");
                return;
            }

            if (waferProbeAlign.waferProbeAlignParameter.IsDO_Probe_ClampModule_BW())
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "프로브 카드 클램프 실린더 열기 신호 Off");

                waferProbeAlign.waferProbeAlignParameter.DO_ProbeClampModule_BW(false);
            }
            else
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "프로브 카드 클램프 실린더 열기");

                waferProbeAlign.waferProbeAlignParameter.DO_ProbeClampModule_BW(true);
                waferProbeAlign.waferProbeAlignParameter.DO_ProbeClampModule_FW(false);
            }
        }

        private void baseButtonProbeUnpackingCyl_Up_Click(object sender, EventArgs e)
        {
            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (waferProbeAlign.waferProbeAlignParameter.IsDO_Probe_UnpackingCyl_Up())
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "프로브 카드 언패킹 실린더 올림 신호 Off");

                waferProbeAlign.waferProbeAlignParameter.DO_Probe_UnpackingCyl_Up(false);
            }
            else
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "프로브 카드 언패킹 실린더 올림");

                waferProbeAlign.waferProbeAlignParameter.DO_Probe_UnpackingCyl_Up(true);
                waferProbeAlign.waferProbeAlignParameter.DO_Probe_UnpackingCyl_Down(false);
            }
        }

        private void baseButtonProbeUnpackingCyl_Down_Click(object sender, EventArgs e)
        {
            if (Equipment.User_Mode == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
                return;
            }

            if (waferProbeAlign.waferProbeAlignParameter.IsDO_Probe_UnpackingCyl_Down())
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "프로브 카드 언패킹 실린더 내림 신호 Off");

                waferProbeAlign.waferProbeAlignParameter.DO_Probe_UnpackingCyl_Down(false);
            }
            else
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Button Click", "프로브 카드 언패킹 실린더 내림");

                waferProbeAlign.waferProbeAlignParameter.DO_Probe_UnpackingCyl_Down(true);
                waferProbeAlign.waferProbeAlignParameter.DO_Probe_UnpackingCyl_Up(false);
            }
        }
    }
}
