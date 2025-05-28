

using ACS.SPiiPlusNET;
using QMC.Common.Laser;
using QMC.Common.Motion.ACS.Motions;
using QMC.Common.Motion.Ajin.Motions;
using QMC.Common.Parts;
using QMC.Common.Vision.HIKVISION;
using QMC.Common.VisionPart;
using QMC.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using SerialCommHoneywellBarcodeReader;
using static QMC.Common.Parts.WorkStageParameter;
using Point = System.Drawing.Point;
using SerialCommLaserPowerMeter1;
using SerialCommLaserPowerMeter2;
using System.IO.Ports;
using MessageBox = System.Windows.Forms.MessageBox;
using static QMC.Common.Modules.Vision;
using static QMC.Common.Modules.WorkStage;


namespace QMC.Common.Modules
{
    [Serializable]
    public class Bds : Module
    {
        #region Define


        //#if true                                                                //  SLD-200C
#if SLD_200C       //  SLD-200U
        public enum nAxis                                                       //  SLD-200C 에서 사용하는 축 번호    
        {
            //  축 번호 변경 전 (MASK_Y:3)
            //  축 번호 변경 후 (MASK_Y:0)

            MASK_Y = 0,
        }
#else
        public enum nAxis                                                       //  SLD-200U 에서 사용하는 축 번호   
        {
            //  축 번호 변경 전 (MASK_Y:3)
            //  축 번호 변경 후 (MASK_Y:0)

            MASK_Y = 0,                                                         //  있긴 하지만 사용하지 않는 축. (SLD-200U 에서는 Mask Y 축이 없음)
        }
#endif


        #endregion


        #region Variables
       

        #endregion

        #region Field
        SettingParameterCollection PosParam_Bds;          //  2022. 04. 25.  SCH : 모터 위치 파라미터를 갖다쓰기 위해 선언해봄.
        //static Conveyor conveyor = new Conveyor("");            //  요거 다시해야 함. Conveyor.cs 에 정의된 변수에 접근할 수 있게... 어케 함? -_-
                                                                //  static 으로 선언하면 되긴 헌디.... 맞는건가 -_-
        public InterpolatorMotionFunction MC_Func = new InterpolatorMotionFunction();
        //public ACSSPiiPlusAxis ACS_Func = new ACSSPiiPlusAxis();
        #endregion

        #region Property
        public BdsConfig Config { set; get; }
        public BdsParameterConfig ParamConfig { set; get; }
        public BdsRecipe Recipe { set; get; }                

        public YStage Stage { set; get; }                         //  MSL SLD-200C, SLD-200U 의 Mask Y 축

        //20250527
        public SpiralLabRtc3D spiralLabRtc3D { get; private set; } = null; //  SpiralLab Rtc3D 객체
        static WorkStage workStage;

        //  레시피 변경 시 위치값을 갱신하기 위해
        public bool m_bParameterSetting_PosData_Reload { set; get; }            //  위치 데이터 다시 로드

        public bool m_bLog_1time;
        public bool m_bLog_1time2;

        //public bool ACS_Motion_isSimulationMode { set; get; }

        //  쓰레드로 변경 --> 변경 취소. 그냥 타이머 쓴다. Thread 쓰니까 뭐가 막 잘 안됨 ㅡㅡ
        public System.Windows.Forms.Timer timer_MainWork;
        public bool m_btimer_MainWork_Stop;

        public bool m_bBlink;

        public bool m_bAlignVisionThread_Use;                                                       //  2022. 04. 08.  SCH : Align Vision 을 Thread 로 할지 말지?

        //  단일 동작 중 안전센서를 터치할 경우 모터 Stop
        public bool m_bInManualMoving_SafetySensor_Detected = false;

        //  Cycle 동작 중 안전센서를 터치할 경우 모터 Stop
        public bool m_bInCycleMoving_SafetySensor_Detected = false;

        //  Cycle 동작 중 엘리베이터 Z축 오버 토크가 발생할 경우 모터 Stop
        public bool m_bInCycleMoving_ElevZOverTorque_Detected = false;


        public BdsParameter bdsParameter { set; get; }
        #endregion


        #region NewForm 을 위한 Teaching Position List 변수

        /// <summary>
        /// Config 에서 Teching Position List 가 추가되거나 삭제 되면 여기도 해줘야 함. (이 항목이 Position 배열의 Index 가 되기 때문에)
        /// </summary>
        /// 
        //  Vision Teaching Position List
        public enum BDS_TeachingPosList : int
        {
            BDS_NoneMarkPos = 0,
            BDS_Mask1Pos,
            BDS_Mask2Pos,
            BDS_Mask3Pos,
            BDS_Mask4Pos,
        }

        public struct stBDSAxesPos
        {
            public double Mask_Y;                           //  Mask Y
        }
        public stBDSAxesPos[] stBDSTeachingPos = new stBDSAxesPos[System.Enum.GetValues(typeof(BDS_TeachingPosList)).Length];

        public struct stBDSMoveProperties
        {
            public int Fine_Accel;                          //  Fine Acceleration
            public int Fine_SettleDelay;                    //  Fine Settle Delay
            public int Coarse_Accel;                        //  Coarse Acceleration
            public int Coarse_SettleDelay;                  //  Coarse Settle Delay
        }
        public stBDSMoveProperties[] stBDSPosMoveProperties = new stBDSMoveProperties[System.Enum.GetValues(typeof(BDS_TeachingPosList)).Length];

        #endregion


        public override void SetModuleScale(double dScaleX, double dScaleY, double dXaxisT, double dYaxisT, bool bInvertedX, bool bInvertedY)
        {
            //  요거 주석처리하면 안되는데... 이유가 뭘까

            throw new NotImplementedException();
        }

        #region Tick Count Check

        public int TickCount_MainCycle_Start { set; get; }
        public int TickCount_MainCycle_Current { set; get; }
        public int TickCount_MainCycle_Interval { set; get; }

        //System.Diagnostics.Stopwatch sw_DispenserMainCyc = new System.Diagnostics.Stopwatch();
        //System.Diagnostics.Stopwatch sw_DispenserSubCyc = new System.Diagnostics.Stopwatch();

        public enum TickType : int
        {
            TICK_HOME = 0,              //  0 : Initialize
            TICK_MAIN = 1,              //  1 : Main Cycle
            TICK_SUB = 2,               //  2 : Sub Cycle
            TICK_PAUSE = 3,             //  3 : Pause
            TICK_CHECK = 4,             //  4 : 체크용
            
            //TICK_LASER_INTERFACE = 3,   //  3 : Laser Interface Set
            //TICK_LASER_FOCUS = 4,       //  4 : Laser Focus Check Cycle
            //TICK_LASER_COMM = 5,        //  5 : Laser Comm. Cycle
            //TICK_POWERMETER_COMM = 6,   //  6 : Power Meter Comm. Cycle
            //TICK_ALIGN = 7,             //  7 : Wafer Align Cycle
            //TICK_RVA = 8,               //  8 : Beam Size Change Cycle
        }

        public int[,] TickCount_Cycle = new int[System.Enum.GetValues(typeof(TickType)).Length, 2];


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

        #region Initialize Cycle

        //  홈 실행 동작을 모듈에서 할지, Work Stage 에서 한꺼번에 할지 결정하고, 그에 따라 변수 및 함수를 정의한다.
        //public int m_nHomeStep { set; get; }                    //  Home Step
        //public int m_nHomeAxisCount { set; get; }
        //public bool m_bHomeOK { get; set; }
        //public enum Home_Step
        //{
        //    None = 0,
        //    Start,                                              //  시작

        //    //  알람이 발생한 축이 있을 경우, Servo Off --> Reset --> Servo On 해야 한다.
        //    AxisAlarmCheck,                                     //  서보 축 알람 체크
        //    AlarmAxisServoOff,                                  //  알람 축 서보 Off
        //    AlarmAxisServoOffCheck,                             //  알람 축 서보 Off 확인
        //    AlarmAxisAlarmResetOn,                              //  알람 축 리셋 신호 On
        //    AlarmAxisAlarmResetOff,                             //  알람 축 리셋 신호 Off (30ms delay 후 Off)
        //    AlarmAxisServoOn,                                   //  알람 축 서보 On
        //    AlarmAxisServoOnCheck,                              //  알람 축 서보 On 확인

        //    VisionY_HomeStart,                                  //  Vision Y 축 홈 실행
        //    VisionY_HomeCompleteCheck,                          //  Vision Y 축 홈 완료 체크

        //    VisionXZ_UVW_EZ_HomeStart,                          //  Vision XZ, UVW EZ 축 홈 실행
        //    VisionXZ_UVW_EZ_HomeCompleteCheck,                  //  Vision XZ, UVW EZ 축 홈 완료 체크

        //    ElevZ_Move_ReadyPos,                                //  Elev. Z 축, 대기 위치로 이동 
        //    ElevZ_Move_ReadyPos_DoneCheck,                      //  Elev. Z 축, 대기 위치로 이동 완료 확인

        //    UVW_Move_ReadyPos,                                  //  UVW 축, 대기 위치로 이동
        //    UVW_Move_ReadyPos_DoneCheck,                        //  UVW 축, 대기 위치로 이동 완료 체크

        //    VisionXYZ_Move_ReadyPos,                            //  Vision XYZ 축, 대기 위치로 이동 
        //    VisionXYZ_Move_ReadyPos_DoneCheck,                  //  Vision XYZ 축, 대기 위치로 이동 완료 확인

        //    Complete                                            //  완료
        //}

        #endregion

        #region Single Action (Mask Pos)

        public int m_nLaserMask_Move_Step { set; get; }                         //  Laser Mask Move Step
        public enum LaserMask_Move_Step
        {
            None = 0,
            Start,                                                          //  시작

            LaserMask_Move_Condition_Check,                                 //  Laser Mask 이동 조건 체크 (Drilling Cycle : None, Laser Shutter Close, Laser Off [필수 아님]) 

            MaskY_Move_MaskPos,                                             //  이동해야 하는 Mask 위치까지 이동
            MaskY_Move_MaskPos_DoneCheck,                                   //  이동해야 하는 Mask 위치까지 이동 완료 체크

            Complete                                                        //  완료
        }
        #endregion


        #region Constructor
        public Bds(string strName) : base(strName)
        {
            bool ret = true;

            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;
            foreach (Module module in m_collectionModules)
            {
                if (module.Name == "WorkStage")
                {
                    workStage = module as WorkStage;
                }
            }

            ParamConfig = new BdsParameterConfig();
            Config = new BdsConfig();

            m_nLaserMask_Move_Step = (int)LaserMask_Move_Step.None;

            m_bInManualMoving_SafetySensor_Detected = false;
            m_bInCycleMoving_SafetySensor_Detected = false;

            //  타이머를 쓰레드로 변경 --> 다시 타이머 사용하기로...

            //  Main Work 타이머
            timer_MainWork = new System.Windows.Forms.Timer();
            timer_MainWork.Interval = 10;                                               //  50 이었는데 10으로 변경. (50은 너무 느린 감이 없지 않아 있음. 근데 10에서 잘 될란가...?)
            timer_MainWork.Tick += new System.EventHandler(Timer_MainWork_Func);

            m_btimer_MainWork_Stop = false;

            m_bLog_1time = false;
            m_bLog_1time2 = false;

            TickCount_MainCycle_Start = 0;
            TickCount_MainCycle_Current = 0;
            TickCount_MainCycle_Interval = 0;


            //  Teaching Data 저장 폴더 생성
            if (Directory.Exists(ConfigManager.GetTeachingDataPath()) == false)
            {
                Directory.CreateDirectory(ConfigManager.GetTeachingDataPath());
            }

            //  Teaching Position List 변수 초기화
            for (int i = 0; i < System.Enum.GetValues(typeof(BDS_TeachingPosList)).Length; i++)
            {
                stBDSTeachingPos[i].Mask_Y = 0;

                stBDSPosMoveProperties[i].Fine_Accel = 0;
                stBDSPosMoveProperties[i].Fine_SettleDelay = 0;
                stBDSPosMoveProperties[i].Coarse_Accel = 0;
                stBDSPosMoveProperties[i].Coarse_SettleDelay = 0;
            }

            Teaching_Position_Load();
        }                                                   
        #endregion

        #region IExecuter
        public override int Initialize()
        {
            return base.Initialize();
        }

        public override int OnPrepareToWork()
        {
            return base.OnPrepareToWork();
        }
        public override int OnAfterWork()
        {
            return base.OnAfterWork();
        }
        public override void Stop()
        {
            base.Stop();
        }
        public override int OnWork()
        {
            //dispenserParameter.stDispenserPosParam = dispenserParameter.GetPositionInformation("Ready");


            int ret = 0;
            ret = base.Work();

            return ret;
        }
        #endregion

        #region Module Members

        protected override int OnRun()
        {
            return base.OnRun();
        }

        public override int Create()
        {
            int ret = base.Create();

            Stage = new YStage("Stage");
            Stage.Create();
            Stage.Owner = this;
            Parts.Add(Stage);

            bdsParameter = new BdsParameter("BDS Parameter");
            bdsParameter.Create();
            bdsParameter.Owner = this;
            bdsParameter.Axes = Stage.Axes;
            Parts.Add(bdsParameter);

            //PosParam_Dispenser = GetConfigData();     //  요건 나중에
            Recipe = new BdsRecipe(this);

            return ret;
        }

        public void InitRtc3DModule()
        {
            if(workStage.rtc != null)
            {
                spiralLabRtc3D = new SpiralLabRtc3D("Scanner3D", workStage.rtc);
                spiralLabRtc3D.Create();
                spiralLabRtc3D.Owner = this;
                Parts.Add(spiralLabRtc3D);

                Log.Write("SLD-200", "InitRtc3DModule", "Scanner3D 모듈 초기화 완료");
            }
        }

        public override void SetConfigData(object configData)
        {
            Config = configData as BdsConfig;

            if (Config == null)
                Config = new BdsConfig();

            Config.Init();

            if (Config.ParamConfig != null)
            {
                ParamConfig = Config.ParamConfig;

                bdsParameter.Config = ParamConfig;
            }

            Stage.Config = this.Config.StageConfig;

            //Stage.UpdateDirection();                              //  Z 축 방향 바꾸기? (주석 처리)
            //jigAligner.Config = Config.JigAlignerConfig;
        }

        public override object GetConfigData()
        {
            return Config;
        }

        public override void UpdateConfigData()
        {
            //jigAligner.Config = Config.JigAlignerConfig;

            base.UpdateConfigData();
        }

        public override void SetRecipeData(object recipeData)
        {
            BdsRecipe recipe = recipeData as BdsRecipe;
            if (recipe == null)
            {
                recipe = new BdsRecipe(this);
            }


            Recipe = recipe;
            Recipe.Init(this);

            base.SetRecipeData(recipeData);
        }

        public override object GetRecipeData()
        {
            return Recipe;
        }

        public override void UpdateRecipeData()
        {
            base.UpdateRecipeData();
        }

        public override void Close()
        {
            base.Close();

            if (Stage != null)
            {
                Stage.Close();
            }

            //if (ACS_Motion != null)
            //{
            //    ACS_Motion.CloseComm();
            //}
        }
        #endregion


        #region Teaching Position List Save / Load

        public bool Teaching_Position_Load()
        {
            string strTemp = "";

            bool m_bRet = true;
            string strFIle = "";
            StringBuilder temp = new StringBuilder(255);

            strFIle = ConfigManager.GetTeachingDataPath() + "\\BDS_TeachingPosition.ini";

            if (File.Exists(strFIle) == false)
            {
                MessageBox.Show("BDS Teaching Position 파일이 없습니다.\r\n\r\n[Default 값으로 설정됩니다.]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return false;
            }

            //  Position 데이터 로드
            for (int i = 0; i < System.Enum.GetValues(typeof(BDS_TeachingPosList)).Length; i++)
            {
                strTemp = string.Format("PosIndex_{0}", i);

                //  Mask Y
                NativeMethods.GetPrivateProfileString(strTemp, "MaskY", "0", temp, 255, strFIle);
                stBDSTeachingPos[i].Mask_Y = Equipment.ToDouble(temp.ToString());
            }

            return m_bRet;
        }

        public void Teaching_Position_Save()
        {
            string strTemp = "";

            string strFIle = "";
            strFIle = ConfigManager.GetTeachingDataPath() + "\\BDS_TeachingPosition.ini";

            if (File.Exists(strFIle) == false)
            {
                File.Create(strFIle);

                MessageBox.Show("BDS Teaching Position 파일을 생성하였습니다. 다시 시도하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //  Position Parameter 저장
            for (int i = 0; i < System.Enum.GetValues(typeof(BDS_TeachingPosList)).Length; i++)
            {
                strTemp = string.Format("PosIndex_{0}", i);

                //  Mask Y
                NativeMethods.WritePrivateProfileString(strTemp, "MaskY", stBDSTeachingPos[i].Mask_Y.ToString(), strFIle);
            }

            MessageBox.Show("Teaching Position 을 저장하였습니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion


        #region Teaching Position Move Properties Save / Load

        public bool Move_Properties_Load()
        {
            string strTemp = "";

            bool m_bRet = true;
            string strFIle = "";
            StringBuilder temp = new StringBuilder(255);

            strFIle = ConfigManager.GetTeachingDataPath() + "\\BDS_MoveProperties.ini";

            if (File.Exists(strFIle) == false)
            {
                MessageBox.Show("BDS Move Properties 파일이 없습니다.\r\n\r\n[Default 값으로 설정됩니다.]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return false;
            }

            //  Position 데이터 로드
            for (int i = 0; i < System.Enum.GetValues(typeof(BDS_TeachingPosList)).Length; i++)
            {
                strTemp = string.Format("PosIndex_{0}", i);

                //  Fine Accel
                NativeMethods.GetPrivateProfileString(strTemp, "Fine_Accel", "20", temp, 255, strFIle);
                stBDSPosMoveProperties[i].Fine_Accel = Equipment.ToInt(temp.ToString());
                //  Fine Settle Delay
                NativeMethods.GetPrivateProfileString(strTemp, "Fine_SettleDelay", "200", temp, 255, strFIle);
                stBDSPosMoveProperties[i].Fine_SettleDelay = Equipment.ToInt(temp.ToString());
                //  Coarse Accel
                NativeMethods.GetPrivateProfileString(strTemp, "Coarse_Accel", "200", temp, 255, strFIle);
                stBDSPosMoveProperties[i].Coarse_Accel = Equipment.ToInt(temp.ToString());
                //  Coarse Settle Delay
                NativeMethods.GetPrivateProfileString(strTemp, "Coarse_SettleDelay", "20", temp, 255, strFIle);
                stBDSPosMoveProperties[i].Coarse_SettleDelay = Equipment.ToInt(temp.ToString());
            }

            return m_bRet;
        }

        public void Move_Properties_Save()
        {
            string strTemp = "";

            string strFIle = "";
            strFIle = ConfigManager.GetTeachingDataPath() + "\\BDS_MoveProperties.ini";

            if (File.Exists(strFIle) == false)
            {
                File.Create(strFIle);

                MessageBox.Show("BDS Move Properties 파일을 생성하였습니다. 다시 시도하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //  Position Parameter 저장
            for (int i = 0; i < System.Enum.GetValues(typeof(BDS_TeachingPosList)).Length; i++)
            {
                strTemp = string.Format("PosIndex_{0}", i);

                //  Fine Accel
                NativeMethods.WritePrivateProfileString(strTemp, "Fine_Accel", stBDSPosMoveProperties[i].Fine_Accel.ToString(), strFIle);
                //  Fine Settle Delay
                NativeMethods.WritePrivateProfileString(strTemp, "Fine_SettleDelay", stBDSPosMoveProperties[i].Fine_SettleDelay.ToString(), strFIle);
                //  Coarse Accel
                NativeMethods.WritePrivateProfileString(strTemp, "Coarse_Accel", stBDSPosMoveProperties[i].Coarse_Accel.ToString(), strFIle);
                //  Coarse Settle Delay
                NativeMethods.WritePrivateProfileString(strTemp, "Coarse_SettleDelay", stBDSPosMoveProperties[i].Coarse_SettleDelay.ToString(), strFIle);
            }

            //MessageBox.Show("Move Properties 를 저장하였습니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion


        #region Event Handler

        private void Timer_MainWork_Func(object sender, EventArgs e)
        {
            //  동시에 진행되지 않는 함수들만 동일한 타이머로 한다.

            m_btimer_MainWork_Stop = false;
            timer_MainWork.Enabled = false; 

            if (!m_btimer_MainWork_Stop)
            {
                timer_MainWork.Enabled = true;
            }
        }

        private void Timer_ProductAlign_Func(object sender, EventArgs e)
        {
            if (!m_bAlignVisionThread_Use)
            {
                
            }
        }

        public void forThread_MainWorkCycle()
        {
            //  Main-Work Cycle

        }

        public void forThread_SubWorkCycle()
        {
            //  Sub-Work Cycle

        }

        public void forThread_AlignVisionCycle()
        {
            //Run_ProductAlign_Func();            
        }


        #endregion

        #region Method

        public List<string> GetPositionList()
        {
            List<string> ret = new List<string>();

            foreach (YPositionData position in Config.Positions)
            {
                if (position.Type == TargetType.Base)
                    ret.Add(position.Name);
            }
            return ret;
        }

        #endregion

        public bool Machine_Parameter_Exist()
        {
            bool m_bRet = false;
            string strFIle = "";

            strFIle = ConfigManager.GetConfigPath() + "\\Common Setting (Do not delete or modify).ini";

            if (File.Exists(strFIle))
            {
                m_bRet = true;
            }

            return m_bRet;
        }

        public bool Machine_Parameter_Load()
        {
            bool m_bRet = false;
            string strFIle = "";
            StringBuilder temp = new StringBuilder(255);

            strFIle = ConfigManager.GetConfigPath() + "\\Common Setting (Do not delete or modify).ini";

            return m_bRet;
        }

        public void Machine_Parameter_Save()
        {
            string strFIle = "";
            strFIle = ConfigManager.GetConfigPath() + "\\Common Setting (Do not delete or modify).ini";
        }
    }
}