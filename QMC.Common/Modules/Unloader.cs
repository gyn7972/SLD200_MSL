

using Cognex.VisionPro.Implementation.Internal;
using QMC.Common.Motion.Ajin.Motions;
using QMC.Common.Parts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using static QMC.Common.Equipment;
using static QMC.Common.Modules.Loader;
using static QMC.Common.Modules.Unloader;
using static QMC.Common.Modules.WorkStage;
using MessageBox = System.Windows.Forms.MessageBox;


namespace QMC.Common.Modules
{
    [Serializable]
    public class Unloader : Module
    {
        #region Define

        //#if true                                                                //  SLD-200C
#if SLD_200C                                                                //  SLD-200U
        public enum nAxis                                                       //  SLD-200C 에서 사용하는 축 번호    
        {
            //  축 번호 변경 전 (Z0:10,   Z1:11,  TR_X:12,    TR_Z:13)
            //  축 번호 변경 후 (Z0:10,   Z1:11,  TR_X:12,    TR_Z:13) - 변동 없음

            Z0 = 10,
            Z1,
            TR_X,
            TR_Z,
        }
#else
        public enum nAxis                                                       //  SLD-200U 에서 사용하는 축 번호   
        {
            //  축 번호 변경 전 (Z0:10,   Z1:11,  TR_X:12,    TR_Z:13)
            //  축 번호 변경 후 (Z0:10,   Z1:11,  TR_X:12,    TR_Z:13) - 변동 없음

            Z0 = 9,
            Z1,
            TR_X,
            TR_Z,
        }
#endif


        #endregion


        //  다른 모듈에 접근하기 위함
        static WorkStage workStage;
        static Loader loader;

        public enum AlarmKey
        {
            FirstAlarm = 5000,

            Drilling_NotCompleted,                          //  드릴링 가공 진행중입니다.
            UL_Transfer_Picker_Module_Exist,                //  언로더 트랜스퍼 Picker 에 모듈이 존재합니다.
            WorkStage_Module_NotExist,                      //  언로더 WorkStage 에 모듈이 존재하지 않습니다.
            UL_Transfer_Picker_Module_NotExist,             //  언로더 트랜스퍼 Picker 에 모듈이 없습니다.
            UL_Stacker0_Running,                            //  언로더 스태커0 가동중입니다.
            UL_Stacker1_Running,                            //  언로더 스태커1 가동중입니다.
            UL_NGPort_Full,                                 //  언로더 NG 포트가 가득 차 있습니다.

            UL_Staker0_Too_Many_Module,
            UL_Staker0_MoveZ_Timeout,
            UL_Staker0_Z_Full_Sensor_On_Fail,
            UL_Staker0_Z_Full_Sensor_On_Z_Move_Fail,
            UL_Staker1_Too_Many_Module,
            UL_Staker1_MoveZ_Timeout,
            UL_Staker1_Z_Full_Sensor_On_Fail,
            UL_Staker1_Z_Full_Sensor_On_Z_Move_Fail,
            UL_Staker1_Z_Full_Sensor_On_Z_Move_ToBottom_Fail,

            UL_Transfer_Error,
            UL_Transfer_Z_Move_To_Ready_Pos_Fail,
            UL_Transfer_X_Move_To_Ready_Pos,
            UL_Transfer_Z_Move_To_WorkStage_PickUp_Pos,
            UL_WorkStage_Move_To_Unloading_Pos,
            UL_Transfer_X_Move_To_WorkStage_Pos,
            UL_WorkStage_Vacuum_Off,
            UL_Transfer_Picker_Vacuum_On_Check,

            WorkStage_DustCollector_Off_Fail,

            LastAlarm = 5999,
        }
        #region Variables

        #endregion

        #region Field
        SettingParameterCollection PosParam_Unloader;          //  2022. 04. 25.  SCH : 모터 위치 파라미터를 갖다쓰기 위해 선언해봄.
        //static Conveyor conveyor = new Conveyor("");            //  요거 다시해야 함. Conveyor.cs 에 정의된 변수에 접근할 수 있게... 어케 함? -_-
                                                                //  static 으로 선언하면 되긴 헌디.... 맞는건가 -_-
        public InterpolatorMotionFunction MC_Func = new InterpolatorMotionFunction();
        //public ACSSPiiPlusAxis ACS_Func = new ACSSPiiPlusAxis();
        #endregion

        #region Property
        public UnloaderConfig Config { set; get; }
        public UnloaderParameterConfig ParamConfig { set; get; }
        public UnloaderRecipe Recipe { set; get; }                

        public ZzxzStage Stage { set; get; }                         //  MSL SLD-200C, SLD-200U 

        //  레시피 변경 시 위치값을 갱신하기 위해
        public bool m_bParameterSetting_PosData_Reload { set; get; }            //  위치 데이터 다시 로드

        public bool m_bLog_1time;
        public bool m_bLog_1time2;

        //public bool ACS_Motion_isSimulationMode { set; get; }

        //  쓰레드로 변경 --> 변경 취소. 그냥 타이머 쓴다. Thread 쓰니까 뭐가 막 잘 안됨 ㅡㅡ
        //public System.Windows.Forms.Timer timer_UnloaderWork;
        public System.Timers.Timer timer_UnloaderWork;
        protected Task m_taskTimer_UnloaderWork_Tick = null;
        public bool m_btimer_UnloaderWork_Stop;

        public bool m_bBlink;

        public bool m_bAlignVisionThread_Use;                                                       //  2022. 04. 08.  SCH : Align Vision 을 Thread 로 할지 말지?

        //  단일 동작 중 안전센서를 터치할 경우 모터 Stop
        public bool m_bInManualMoving_SafetySensor_Detected = false;

        //  Cycle 동작 중 안전센서를 터치할 경우 모터 Stop
        public bool m_bInCycleMoving_SafetySensor_Detected = false;

        //  Cycle 동작 중 엘리베이터 Z축 오버 토크가 발생할 경우 모터 Stop
        public bool m_bInCycleMoving_ElevZOverTorque_Detected = false;


        public UnloaderParameter unloaderParameter { set; get; }
        #endregion


        #region NewForm 을 위한 Teching Position List 변수

        /// <summary>
        /// Loader Module 에 선언함
        /// </summary>
        /// 

        #endregion

        protected override void InitAlarm()
        {
            Alarm alarm = new Alarm();
            alarm.Code = (int)AlarmKey.UL_Staker0_Too_Many_Module;
            alarm.Title = "Unloader";
            alarm.Cause = "언로더에 R-Port에 자재가 너무 많이 적재 되어 있습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);

            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.UL_Staker0_MoveZ_Timeout;
            alarm.Title = "Unloader";
            alarm.Cause = "Unloader Right stacker 동작 시 타임아웃 발생하였습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);

            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.UL_Staker1_MoveZ_Timeout;
            alarm.Title = "Unloader";
            alarm.Cause = "Unloader Left stacker 동작 시 타임아웃 발생하였습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);

            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.UL_Staker0_Z_Full_Sensor_On_Fail;
            alarm.Title = "Unloader";
            alarm.Cause = "언로더에 R-Port에 만재 센서가 감지되지 않았습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);

            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.UL_Staker0_Z_Full_Sensor_On_Z_Move_Fail;
            alarm.Title = "Unloader";
            alarm.Cause = "언로더에 R-Port에 만재 센서감지 위치까지 이동하지 못하였습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);

            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.UL_Staker1_Too_Many_Module;
            alarm.Title = "Unloader";
            alarm.Cause = "언로더에 L-Port에 만재 센서감지 위치까지 이동하지 못하였습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);


            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.UL_Staker1_Z_Full_Sensor_On_Fail;
            alarm.Title = "Unloader";
            alarm.Cause = "언로더에 L-Port에 만재 센서가 감지되지 않았습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);

            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.UL_Staker1_Z_Full_Sensor_On_Z_Move_Fail;
            alarm.Title = "Unloader";
            alarm.Cause = "언로더에 L-Port에 만재 센서감지 위치까지 이동하지 못하였습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);

            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.UL_Staker1_Z_Full_Sensor_On_Z_Move_ToBottom_Fail;
            alarm.Title = "Unloader";
            alarm.Cause = "언로더에 L-Port에 Bottom 포지션까지 이동에 실패 하였습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);

            //
            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.UL_Transfer_Error;
            alarm.Title = "Unloader Transfer";
            alarm.Cause = "언로더에 트랜스퍼 Error (로그 확인 후 알람 생성 및 추가 바람)";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);

            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.UL_Transfer_Z_Move_To_Ready_Pos_Fail;
            alarm.Title = "Unloader Transfer";
            alarm.Cause = "언로더에 트랜스퍼 Z축대기 위치로 이동하는데 실패 하였습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);

            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.UL_Transfer_X_Move_To_Ready_Pos;
            alarm.Title = "Unloader Transfer";
            alarm.Cause = "언로더에 트랜스퍼 X축대기 위치로 이동하는데 실패 하였습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);

            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.UL_Transfer_Z_Move_To_WorkStage_PickUp_Pos;
            alarm.Title = "Unloader Transfer";
            alarm.Cause = "언로더에 트랜스퍼 Z축 WorkStage PickUp 위치로 이동하는데 실패 하였습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);

            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.UL_WorkStage_Move_To_Unloading_Pos;
            alarm.Title = "Unloader Transfer";
            alarm.Cause = "언로더에 트랜스퍼 WorkStage Unloading 위치로 이동하는데 실패 하였습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);

            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.UL_Transfer_X_Move_To_WorkStage_Pos;
            alarm.Title = "Unloader Transfer";
            alarm.Cause = "언로더에 트랜스퍼 X축 WorkStage PickUp 위치로 이동하는데 실패 하였습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);

            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.UL_WorkStage_Vacuum_Off;
            alarm.Title = "Unloader Transfer";
            alarm.Cause = "언로더에 트랜스퍼 WorkStage Vacuum Off 에 실패 하였습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);


            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.UL_Transfer_Picker_Vacuum_On_Check;
            alarm.Title = "Unloader Transfer";
            alarm.Cause = "언로더에 트랜스퍼 Picker Vacuum On Check 에 실패 하였습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);


            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.Drilling_NotCompleted;
            alarm.Title = "Unloader Transfer";
            alarm.Cause = "드릴링 가공 진행중입니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";

            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.UL_Transfer_Picker_Module_Exist;
            alarm.Title = "Unloader Transfer";
            alarm.Cause = "언로더 트랜스퍼 Picker 에 모듈이 존재합니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";

            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.WorkStage_Module_NotExist;
            alarm.Title = "Unloader Transfer";
            alarm.Cause = "WorkStage 에 Unloading 할 모듈이 존재하지 않습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";

            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.UL_Transfer_Picker_Module_NotExist;
            alarm.Title = "Unloader Transfer";
            alarm.Cause = "언로더 트랜스퍼 Picker 에 모듈이 없습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";

            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.UL_Stacker0_Running;
            alarm.Title = "Unloader Transfer";
            alarm.Cause = "언로더 스태커0 가동중입니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";

            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.UL_Stacker1_Running;
            alarm.Title = "Unloader Transfer";
            alarm.Cause = "언로더 스태커1 가동중입니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";

            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.UL_NGPort_Full;
            alarm.Title = "Unloader Transfer";
            alarm.Cause = "언로더 NG 포트가 가득 차 있습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";

            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.WorkStage_DustCollector_Off_Fail;
            alarm.Title = "Unloader";
            alarm.Cause = "집진기가 Off 되지 않았습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
        }

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

            TICK_ULSZ0 = 5,             //  5 : Unloader Stacker Z0
            TICK_ULSZ1 = 6,             //  6 : Unloader Stacker Z1
            TICK_ULTR = 7,              //  7 : Unloader Transfer

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


        #region Single Action (Stacker, Module Putdown Waiting Pos)

        public int m_nStacker0_ModulePutdownWaitingPos_Step { set; get; }             //  Stacker, Module Putdown Waiting Position Step
        public int m_nStacker1_ModulePutdownWaitingPos_Step { set; get; }             //  Stacker, Module Putdown Waiting Position Step


        //  자동 운전을 위한 변수
        public bool m_bStacker0_Complete { set; get; }                                  //  Stacker0 동작 완료 여부 (Transfer 가 Stacker0 에 Module 을 PutDown 해도 되는지 확인하는 Flag)
        public bool m_bStacker1_Complete { set; get; }                                  //  Stacker1 동작 완료 여부 (Transfer 가 Stacker1 에 Module 을 PutDown 해도 되는지 확인하는 Flag)


        public enum StackerModulePutdownWaitingPos_Step
        {
            None = 0,

            Start,                                                          //  시작


            Process_Condition_Check,                                        //  동작 조건 체크 (Transfer Cycle : None)


            //  Full Sensor 감지 상태일 경우 (Off 될 때 까지 내림 -> Off 되면 Stop -> 느리게 Up -> On 되면 Stop -> 느리게 Down -> Off 되면 Stop -> 더 느리게 Up -> On 되면 Stop -> 완료)
            StackerZ_MoveType1_FastDown,                                    //  Stacker Z 축, 빠르게 내림 (최 하단까지)
            StackerZ_MoveType1_FastDown_DoneCheck,                          //  Stacker Z 축, 빠르게 내림, 이동 완료 확인

            StackerZ_MoveType1_SlowUp,                                      //  Stacker Z 축, 느리게 올림 (최 상단까지)
            StackerZ_MoveType1_SlowUp_DoneCheck,                            //  Stacker Z 축, 느리게 올림, 이동 완료 확인

            StackerZ_MoveType1_Slow2Down,                                   //  Stacker Z 축, 더 느리게 내림 (최 하단까지)
            StackerZ_MoveType1_Slow2Down_DoneCheck,                         //  Stacker Z 축, 더 느리게 내림, 이동 완료 확인

            StackerZ_MoveType1_Slow3Up,                                     //  Stacker Z 축, 더더 느리게 올림 (최 상단까지)
            StackerZ_MoveType1_Slow3Up_DoneCheck,                           //  Stacker Z 축, 더더 느리게 올림, 이동 완료 확인


            //  Full Sensor 감지되지 않는 상태일 경우 (Up -> On 되면 Stop -> 느리게 Down -> Off 되면 Stop -> 더 느리게 Up -> On 되면 Stop -> 완료)
            StackerZ_MoveType2_FastUp,                                      //  Stacker Z 축, 빠르게 올림 (최 상단까지)
            StackerZ_MoveType2_FastUp_DoneCheck,                            //  Stacker Z 축, 빠르게 올림, 이동 완료 확인

            StackerZ_MoveType2_Slow2Down,                                   //  Stacker Z 축, 더 느리게 내림 (최 하단까지)
            StackerZ_MoveType2_Slow2Down_DoneCheck,                         //  Stacker Z 축, 더 느리게 내림, 이동 완료 확인

            StackerZ_MoveType2_Slow3Up,                                     //  Stacker Z 축, 더더 느리게 올림 (최 상단까지)
            StackerZ_MoveType2_Slow3Up_DoneCheck,                           //  Stacker Z 축, 더더 느리게 올림, 이동 완료 확인


            //  마지막 위치 이동 후 추가 이동
            StackerZ_Move_OverDistance,                                     //  Stacker Z 축, 최종 위치에서 추가로 이동 (아래로 5mm 더 내림)
            StackerZ_Move_OverDistance_DoneCheck,                           //  Stacker Z 축, 최종 위치에서 추가로 이동 완료 확인


            Complete                                                        //  완료
        }
        #endregion


        #region Single Action (Unloader Transfer)


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        ///     Stop -> Start 시 동작 Sequence 를 재설정 하기 위한 변수
        /// 
        /// </summary>
        /// 
        public int m_nUnloader_Transfer_Restart_MoveType { set; get; }                      //  어떤 작업을 하다가 멈춘 것인지?
                                                                                            //      1. Work Stage 에서 Module Pick Up
                                                                                            //      2. Stacker0 에 Module Put Down
                                                                                            //      3. Stacker1 에 Module Put Down

        public bool m_bUnloader_WorkStage_PickUp_Retry { set; get; }

        public int m_nUL_RESTORE_Transfer_Step { set; get; } = 0;
        public int m_nUL_RESTORE_Transfer_MoveType { set; get; } = 0;
        public bool m_bUL_RESTORE_Transfer_fromWorkStage_Module_PickUp_Complete_Flag { set; get; } = false;
        public bool m_bUL_RESTORE_LD_Transfer_toWorkStage_Module_PutDown_Complete { set; get; } = false;
        public int m_nUL_RESTORE_MainWork_Cycle_Step { set; get; } = 0;
        public int m_nUL_RESTORE_DryRun_Cycle_Step { set; get; } = 0;
        public int m_nUL_RESTORE_LaserDrilling_Cycle_Step { set; get; } = 0;
        public bool m_bUL_RESTORE_MainWork_Cycle_Complete { set; get; } = false;
        public int m_nUL_RESTORE_MainWork_Cycle_ResultOKNG { set; get; } = (int)WorkStage.MainCycle_Result.None;
        public bool m_bUL_RESTORE_MainWorkCycle_ResultOK_toRPort { set; get; } = false;                         //  OK 인 Module 을 R-Port 로 가져갈 것인지 L-Port 로 가져갈 것인지

        public bool m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete { set; get; } = false;      //  Work Stage 에서 Module Pick Up 완료 여부
        public bool m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePutDowntoStacker0_Complete { set; get; } = false;         //  Stacker0 에 Module Put Down 완료 여부
        public bool m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePutDowntoStacker1_Complete { set; get; } = false;        //  Stacker1 에 Module Put Down 완료 여부
        public bool m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePutDowntoNG_Complete { set; get; } = false;              //  NG-Port 에 Module Put Down 완료 여부        

        /// <summary>
        /// 
        ///     Stop -> Start 시 동작 Sequence 를 재설정 하기 위한 변수
        /// 
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        //  자동 운전을 위한 변수
        public bool m_bAUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete { set; get; }   //  Work Stage 에서 Module Pick Up 완료 여부
        public bool m_bAUTORUN_Unloader_Transfer_ModulePutDowntoStacker0_Complete { set; get; }     //  Stacker0 에 Module Put Down 완료 여부
        public bool m_bAUTORUN_Unloader_Transfer_ModulePutDowntoStacker1_Complete { set; get; }     //  Stacker1 에 Module Put Down 완료 여부
        public bool m_bAUTORUN_Unloader_Transfer_ModulePutDowntoNG_Complete { set; get; }           //  NG-Port 에 Module Put Down 완료 여부        


        public int m_nUnloader_Transfer_Step { set; get; }                                   //  Transfer Step
        public int m_nUnloaderTransferMoveType { set; get; }                  //  Transfer Move Type
        public bool m_bUL_Transfer_fromWorkStage_Module_PickUp_Complete_Flag { set; get; } = false;      //  Work Stage 에서 Module Pick Up 완료 여부
        public enum UnloaderTransferMoveType : int
        {
            Cycle_None = -1,

            Cycle_Transfer_ReadyPos = 0,                                    //  Transfer Ready Position

            Cycle_WorkStage_PickUp,                                         //  Module Pick Up Cycle (Work Stage)

            Cycle_Stacker0_PutDown,                                         //  Module Put Down Cycle (Stacker 0)
            Cycle_Stacker1_PutDown,                                         //  Module Put Down Cycle (Stacker 1)
            Cycle_NG_PutDown,                                               //  Module Put Down Cycle (NG)
        }


        public enum Unloader_Transfer_Step
        {
            None = 0,
            Start,                                                          //  시작

            Process_Type_Check,                                             //  동작 타입 체크 (Work Stage 에서 Module PickUp, NG-Box 로 Module Drop, Stacker 로 Module PutDown)


            /// <summary>
            /// Transfer 대기 위치로 이동 - 시작
            /// </summary>
            Transfer_Move_Condition_Check,                                  //  Stacker0 에서 Module Pick Up 조건 체크 (Stacker0 Module Exist Sensor On, Stacker0 Module Full Sensor On, Transfer Picker Vacuum Off Check, Stacker0 Cycle : None)

            TransferZ_Move_ReadyPos,                                        //  Transfer Z 축, 대기 위치로 이동
            TransferZ_Move_ReadyPos_DoneCheck,                              //  Transfer Z 축, 대기 위치로 이동 완료 확인

            TransferX_Move_ReadyPos,                                        //  Transfer X 축, 대기 위치로 이동
            TransferX_Move_ReadyPos_DoneCheck,                              //  Transfer X 축, 대기 위치로 이동 완료 확인
            /// <summary>
            /// Transfer 대기 위치로 이동 - 완료
            /// </summary>


            /// <summary>
            /// Work Stage 에서 Module Pick Up - 시작
            /// </summary>
            WorkStage_ModulePickup_Condition_Check,                         //  Work Stage 에서 Module Pick Up 조건 체크 (Work Stage Vacuum On Check, Transfer Picker Vacuum Off Check, Stacker Cycle : None, Drilling Cycle : None)

            WorkStagePickUp_TransferZ_Move_ReadyPos,                        //  Transfer Z 축, 대기 위치로 이동
            WorkStagePickUp_TransferZ_Move_ReadyPos_DoneCheck,              //  Transfer Z 축, 대기 위치로 이동 완료 확인

            WorkStagePickUp_WorkStageCycle_UnloadingPos_Start,              //  Work Stage, Unloading 위치로 이동 Cycle 시작
            WorkStagePickUp_WorkStageCycle_UnloadingPos_CompleteCheck,      //  Work Stage, Unloading 위치로 이동 Cycle 완료 체크

            WorkStagePickUp_TransferX_Move_WorkStagePos,                    //  Transfer X 축, Work Stage 위치로 이동

            WorkStagePickUp_DustCol_Off,
            WorkStagePickUp_DustCol_Off_check,

            WorkStagePickUp_TransferX_Move_WorkStagePos_DoneCheck,          //  Transfer X 축, Work Stage 위치로 이동 완료 확인 (Work Stage Unloading 위치로 이동 Cycle 완료 확인 후, Transfer X 이동 완료 확인)

            WorkStagePickUp_TransferZ_Move_PickUpPos_1stStep,               //  Transfer Z 축, Module Pick Up 위치로 이동 (1단계, 최종 위치에서 위로 10 mm)
            WorkStagePickUp_TransferZ_Move_PickUpPos_1stStep_DoneCheck,     //  Transfer Z 축, Module Pick Up 위치로 이동 완료 확인

            WorkStagePickUp_TransferZ_Move_PickUpPos_2ndStep,               //  Transfer Z 축, Module Pick Up 위치로 이동 (2단계, 최종 위치)
            WorkStagePickUp_TransferZ_Move_PickUpPos_2ndStep_DoneCheck,     //  Transfer Z 축, Module Pick Up 위치로 이동 완료 확인

            WorkStagePickUp_Transfer_PickerVacuum_On,                       //  Transfer, Module Picker Vacuum On
            WorkStagePickUp_WorkStage_Vacuum_Off,                           //  Work Stage, Vacuum Off (and Blow On)
            WorkStagePickUp_Transfer_PickerVacuum_OnCheck,                  //  Transfer, Module Picker Vacuum On 확인 (and Work Stage Vacuum Off 확인)

            WorkStagePickUp_TransferZ_Move_ReadyPos2_1stStep,               //  Transfer Z 축, 대기 위치로 이동 (1단계, 현재 위치에서 위로 10 mm)
            WorkStagePickUp_TransferZ_Move_ReadyPos2_1stStep_DoneCheck,     //  Transfer Z 축, 대기 위치로 이동 완료 확인 (여기서 Transfer Picker 의 Vacuum On 과 Work Stage 의 Vacuum Off 를 동시에 확인)

            WorkStagePickUp_TransferZ_Move_ReadyPos2_2ndStep,               //  Transfer Z 축, 대기 위치로 이동 (2단계, 최종 위치)
            WorkStagePickUp_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck,     //  Transfer Z 축, 대기 위치로 이동 완료 확인
            /// <summary>
            /// Work Stage 에서 Module Pick Up - 완료
            /// </summary>
            /// 


            /// <summary>
            /// Module 을 Stacker0 에 Put Down - 시작
            /// </summary>
            Stacker0_ModulePutdown_Condition_Check,                         //  Stacker0 에 Module Put Down 조건 체크 (Stacker Module Full Sensor On Check, Transfer Picker Vacuum On Check, Stage Vacuum Off Check, Drilling Cycle : None, Stacker Cycle : None)

            Stacker0PutDown_TransferZ_Move_ReadyPos,                        //  Transfer Z 축, 대기 위치로 이동
            Stacker0PutDown_TransferZ_Move_ReadyPos_DoneCheck,              //  Transfer Z 축, 대기 위치로 이동 완료 확인

            Stacker0PutDown_TransferX_Move_StackerPos,                      //  Transfer X 축, Stacker 위치로 이동
            Stacker0PutDown_TransferX_Move_StackerPos_DoneCheck,            //  Transfer X 축, Stacker 위치로 이동 완료 확인

            Stacker0PutDown_TransferZ_Move_PutDownPos_1stStep,              //  Transfer Z 축, Module Put Down 위치로 이동 (1단계, 최종 위치에서 위로 10 mm)
            Stacker0PutDown_TransferZ_Move_PutDownPos_1stStep_DoneCheck,    //  Transfer Z 축, Module Put Down 위치로 이동 완료 확인

            Stacker0PutDown_TransferZ_Move_PutDownPos_2ndStep,              //  Transfer Z 축, Module Put Down 위치로 이동 (2단계, 최종 위치)
            Stacker0PutDown_TransferZ_Move_PutDownPos_2ndStep_DoneCheck,    //  Transfer Z 축, Module Put Down 위치로 이동 완료 확인

            Stacker0PutDown_Transfer_PickerVacuum_Off,                      //  Transfer, Module Picker Vacuum Off (and Blow On)
            Stacker0PutDown_Transfer_PickerVacuum_OffCheck,                 //  Transfer, Module Picker Vacuum Off 확인 (then Blow Off)

            Stacker0PutDown_TransferZ_Move_ReadyPos2_1stStep,               //  Transfer Z 축, 대기 위치로 이동 (1단계, 현재 위치에서 위로 10 mm)
            Stacker0PutDown_TransferZ_Move_ReadyPos2_1stStep_DoneCheck,     //  Transfer Z 축, 대기 위치로 이동 완료 확인 (then Picker Blow Off)

            Stacker0PutDown_TransferZ_Move_ReadyPos2_2ndStep,               //  Transfer Z 축, 대기 위치로 이동 (2단계, 최종 위치)
            Stacker0PutDown_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck,     //  Transfer Z 축, 대기 위치로 이동 완료 확인
            /// <summary>
            /// Module 을 Stacker0 에 Put Down - 완료
            /// </summary>


            /// <summary>
            /// Module 을 Stacker1 에 Put Down - 시작
            /// </summary>
            Stacker1_ModulePutdown_Condition_Check,                         //  Stacker1 에 Module Put Down 조건 체크 (Stacker Module Full Sensor On Check, Transfer Picker Vacuum On Check, Stage Vacuum Off Check, Drilling Cycle : None, Stacker Cycle : None)

            Stacker1PutDown_TransferZ_Move_ReadyPos,                        //  Transfer Z 축, 대기 위치로 이동
            Stacker1PutDown_TransferZ_Move_ReadyPos_DoneCheck,              //  Transfer Z 축, 대기 위치로 이동 완료 확인

            Stacker1PutDown_TransferX_Move_StackerPos,                      //  Transfer X 축, Stacker 위치로 이동
            Stacker1PutDown_TransferX_Move_StackerPos_DoneCheck,            //  Transfer X 축, Stacker 위치로 이동 완료 확인

            Stacker1PutDown_TransferZ_Move_PutDownPos_1stStep,              //  Transfer Z 축, Module Put Down 위치로 이동 (1단계, 최종 위치에서 위로 10 mm)
            Stacker1PutDown_TransferZ_Move_PutDownPos_1stStep_DoneCheck,    //  Transfer Z 축, Module Put Down 위치로 이동 완료 확인

            Stacker1PutDown_TransferZ_Move_PutDownPos_2ndStep,              //  Transfer Z 축, Module Put Down 위치로 이동 (2단계, 최종 위치)
            Stacker1PutDown_TransferZ_Move_PutDownPos_2ndStep_DoneCheck,    //  Transfer Z 축, Module Put Down 위치로 이동 완료 확인

            Stacker1PutDown_Transfer_PickerVacuum_Off,                      //  Transfer, Module Picker Vacuum Off (and Blow On)
            Stacker1PutDown_Transfer_PickerVacuum_OffCheck,                 //  Transfer, Module Picker Vacuum Off 확인 (then Blow Off)

            Stacker1PutDown_TransferZ_Move_ReadyPos2_1stStep,               //  Transfer Z 축, 대기 위치로 이동 (1단계, 현재 위치에서 위로 10 mm)
            Stacker1PutDown_TransferZ_Move_ReadyPos2_1stStep_DoneCheck,     //  Transfer Z 축, 대기 위치로 이동 완료 확인 (then Picker Blow Off)

            Stacker1PutDown_TransferZ_Move_ReadyPos2_2ndStep,               //  Transfer Z 축, 대기 위치로 이동 (2단계, 최종 위치)
            Stacker1PutDown_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck,     //  Transfer Z 축, 대기 위치로 이동 완료 확인
            /// <summary>
            /// Module 을 Stacker1 에 Put Down - 완료
            /// </summary>


            /// <summary>
            /// Module 을 NG-Stacker 에 Drop - 시작
            /// </summary>
            NgStacker_ModuleDrop_Condition_Check,                           //  NG-Stacker 에 Module Dop 조건 체크 (Transfer Picker Vacuum On Check, NG Stacker Full Sensor Off Check)

            NgStackerDrop_TransferZ_Move_ReadyPos,                          //  Transfer Z 축, 대기 위치로 이동
            NgStackerDrop_TransferZ_Move_ReadyPos_DoneCheck,                //  Transfer Z 축, 대기 위치로 이동 완료 확인

            NgStackerDrop_TransferX_Move_NgStackerPos,                      //  Transfer X 축, NG-Stacker 위치로 이동
            NgStackerDrop_TransferX_Move_NgStackerPos_DoneCheck,            //  Transfer X 축, NG-Stacker 위치로 이동 완료 확인

            NgStackerDrop_TransferZ_Move_DropPos,                           //  Transfer Z 축, Module Drop 위치로 이동
            NgStackerDrop_TransferZ_Move_DropPos_DoneCheck,                 //  Transfer Z 축, Module Drop 위치로 이동 완료 확인

            NgStackerDrop_Transfer_PickerVacuum_Off,                        //  Transfer, Module Picker Vacuum Off (and Blow On)
            NgStackerDrop_Transfer_PickerVacuum_OffCheck,                   //  Transfer, Module Picker Vacuum Off 확인 (the Blow Off)

            NgStackerDrop_TransferZ_Move_ReadyPos2,                         //  Transfer Z 축, 대기 위치로 이동
            NgStackerDrop_TransferZ_Move_ReadyPos2_DoneCheck,               //  Transfer Z 축, 대기 위치로 이동 완료 확인
            /// <summary>
            /// Module 을 NG-Stacker 에 Drop - 완료
            /// </summary>


            Complete                                                        //  완료
        }
        #endregion



        #region Constructor
        public Unloader(string strName) : base(strName)
        {
            bool ret = true;

            ParamConfig = new UnloaderParameterConfig();
            Config = new UnloaderConfig();
            //SetDispenserWork((int)DispenserWorkStatus.WORK_NONE);

            //WorkStageIndex = -1;
            //m_bLaserGetStatus_Run = false;

            //Cepheus_laser = new MyCepheusLaser();

            //m_nHomeStep = (int)Home_Step.None;
            m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.None;
            m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.None;
            m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.None;

            m_bStacker0_Complete = false;
            m_bStacker1_Complete = false;

            m_bAUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete = false;                    //  Work Stage 에서 Module Pick Up 완료 여부
            m_bAUTORUN_Unloader_Transfer_ModulePutDowntoStacker0_Complete = false;                      //  Stacker0 에 Module Put Down 완료 여부
            m_bAUTORUN_Unloader_Transfer_ModulePutDowntoStacker1_Complete = false;                      //  Stacker1 에 Module Put Down 완료 여부
            m_bAUTORUN_Unloader_Transfer_ModulePutDowntoNG_Complete = false;                            //  NG-Port 에 Module Put Down 완료 여부

            m_bUL_Transfer_fromWorkStage_Module_PickUp_Complete_Flag = false;

            m_nUL_RESTORE_Transfer_Step = 0;
            m_nUL_RESTORE_Transfer_MoveType = 0;
            m_bUL_RESTORE_Transfer_fromWorkStage_Module_PickUp_Complete_Flag = false;
            m_bUL_RESTORE_LD_Transfer_toWorkStage_Module_PutDown_Complete = false;
            m_nUL_RESTORE_MainWork_Cycle_Step = 0;
            m_nUL_RESTORE_DryRun_Cycle_Step = 0;
            m_nUL_RESTORE_LaserDrilling_Cycle_Step = 0;
            m_bUL_RESTORE_MainWork_Cycle_Complete = false;
            m_nUL_RESTORE_MainWork_Cycle_ResultOKNG = (int)WorkStage.MainCycle_Result.None;
            m_bUL_RESTORE_MainWorkCycle_ResultOK_toRPort = false;                               //  OK 인 Module 을 R-Port 로 가져갈 것인지 L-Port 로 가져갈 것인지

            m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete = false;         //  Work Stage 에서 Module Pick Up 완료 여부
            m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePutDowntoStacker0_Complete = false;           //  Stacker0 에 Module Put Down 완료 여부
            m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePutDowntoStacker1_Complete = false;           //  Stacker1 에 Module Put Down 완료 여부
            m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePutDowntoNG_Complete = false;                 //  NG-Port 에 Module Put Down 완료 여부        

            m_bUnloader_WorkStage_PickUp_Retry = false;

            m_bInManualMoving_SafetySensor_Detected = false;
            m_bInCycleMoving_SafetySensor_Detected = false;

            //  타이머를 쓰레드로 변경 --> 다시 타이머 사용하기로...

            //  Unloader Work 타이머
            timer_UnloaderWork = new System.Timers.Timer(10);
            //timer_UnloaderWork.Elapsed += Timer_UnloaderWork_Tick;
            timer_UnloaderWork.AutoReset = true; // 반복 실행
            timer_UnloaderWork.Enabled = false; // 초기

            m_btimer_UnloaderWork_Stop = false;

            m_bLog_1time = false;
            m_bLog_1time2 = false;

            TickCount_MainCycle_Start = 0;
            TickCount_MainCycle_Current = 0;
            TickCount_MainCycle_Interval = 0;

            //for (int i = 0; i < 2; i++)
            //{
            //    m_dCmdPos[i] = 0.0;
            //    m_dActPos[i] = 0.0;
            //}
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


        #region Action

        public Action<Unloader_Transfer_Step> ActionUnloaderTransferStep;

        #endregion


        #region Module Members

        protected override int OnRun()
        {
            return base.OnRun();
        }

        public override int Create()
        {
            int ret = base.Create();

            Stage = new ZzxzStage("Stage");
            Stage.Create();
            Stage.Owner = this;
            Parts.Add(Stage);

            unloaderParameter = new UnloaderParameter("Unloader Parameter");
            unloaderParameter.Create();
            unloaderParameter.Owner = this;
            unloaderParameter.Axes = Stage.Axes;
            Parts.Add(unloaderParameter);

            //PosParam_Dispenser = GetConfigData();     //  요건 나중에

            Recipe = new UnloaderRecipe(this);
            m_taskTimer_UnloaderWork_Tick = Task.Factory.StartNew(() =>
            {
                Thread.CurrentThread.Name = "m_taskTimer_UnloaderWork_Tick";
                while (true)
                {
                    Thread.Sleep(1);
                    if (IsAlarm())
                    {
                        continue;
                    }
                    if (m_IsModuleClose)
                    {
                        break;
                    }
                    Timer_UnloaderWork_Tick(null, null);

                }
            }); ;

            
            return ret;
        }

        protected bool IsAlarm()
        {

            bool bIsAlarm = false;
            try
            {
                var v = AlarmManager.Instance.Alarms;
                var alarmList = v.Where(t => t.Code >= (int)AlarmKey.FirstAlarm && t.Code <= (int)AlarmKey.LastAlarm);
                bIsAlarm = alarmList.Any();

            }catch(Exception ex)
            {
                Log.Write(ex);
            }
            return bIsAlarm;
        }

        public override void SetConfigData(object configData)
        {
            Config = configData as UnloaderConfig;

            if (Config == null)
                Config = new UnloaderConfig();

            Config.Init();

            if (Config.ParamConfig != null)
            {
                ParamConfig = Config.ParamConfig;

                unloaderParameter.Config = ParamConfig;
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
            UnloaderRecipe recipe = recipeData as UnloaderRecipe;
            if (recipe == null)
            {
                recipe = new UnloaderRecipe(this);
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
            m_IsModuleClose = true;
            if (m_taskTimer_UnloaderWork_Tick != null)
            {
                m_taskTimer_UnloaderWork_Tick.Wait();
                m_taskTimer_UnloaderWork_Tick.Dispose();
                m_taskTimer_UnloaderWork_Tick = null;
            }
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

        public void Module_Allocation()
        {
            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;

            foreach (Module module in m_collectionModules)
            {
                if (module.Name == "WorkStage")
                {
                    workStage = module as WorkStage;
                }

                if (module.Name == "Loader")
                {
                    loader = module as Loader;
                }                

                //if (module.Name == "BDS")
                //{
                //    bds = module as Bds;
                //}

                //if (module.Name == "Vision")
                //{
                //    vision = module as Vision;
                //}
            }
        }


        #region Stop 후 Start 시 동작 Sequence 를 재설정 하기 위한 함수


        public void Unloader_CurrentStatus_Save_StopedByTimeout()
        {
            //  Unloader 상태
            m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete = m_bAUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete;     //  Work Stage 에서 Module Pick Up 완료 여부
            m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePutDowntoStacker0_Complete = m_bAUTORUN_Unloader_Transfer_ModulePutDowntoStacker0_Complete;         //  Stacker0 에 Module Put Down 완료 여부
            m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePutDowntoStacker1_Complete = m_bAUTORUN_Unloader_Transfer_ModulePutDowntoStacker1_Complete;         //  Stacker1 에 Module Put Down 완료 여부
            m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePutDowntoNG_Complete = m_bAUTORUN_Unloader_Transfer_ModulePutDowntoNG_Complete;                     //  NG-Port 에 Module Put Down 완료 여부        
            m_nUL_RESTORE_Transfer_Step = m_nUnloader_Transfer_Step;                                                                                          //  Unloader Transfer Step
            m_nUL_RESTORE_Transfer_MoveType = m_nUnloaderTransferMoveType;                                                                                    //  Unloader Transfer Move Type
            m_bUL_RESTORE_Transfer_fromWorkStage_Module_PickUp_Complete_Flag = m_bUL_Transfer_fromWorkStage_Module_PickUp_Complete_Flag;                      //  Unloader 가 Work Stage 에서 Module Pick Up 완료 여부
            m_bUL_RESTORE_LD_Transfer_toWorkStage_Module_PutDown_Complete = loader.m_bAUTORUN_Loader_Transfer_ModulePutDowntoWorkStage_Complete;                       //  Loader 가 Work Stage 에 Module Put Down 완료 여부
            m_nUL_RESTORE_MainWork_Cycle_Step = workStage.m_nMainWork_Step;                                                                                            //  Main Work Cycle Step
            m_nUL_RESTORE_DryRun_Cycle_Step = workStage.m_nDryRun_Step;                                                                                                //  Dry Run Cycle Step
            m_nUL_RESTORE_LaserDrilling_Cycle_Step = workStage.m_nLaserDrilling_MainStep;                                                                              //  Laser Drilling Cycle Step
            m_bUL_RESTORE_MainWork_Cycle_Complete = workStage.m_bMainWorkCycle_Complete;                                                                               //  Main Work Cycle 완료 여부
            m_nUL_RESTORE_MainWork_Cycle_ResultOKNG = workStage.m_nMainWorkCycle_ResultOKNG;                                                                           //  Main Work Cycle 결과 (OK, NG) : OK 인 경우에만 R-Port 로 가져감
            m_bUL_RESTORE_MainWorkCycle_ResultOK_toRPort = workStage.m_bMainWorkCycle_ResultOK_toRPort;                                                                //  OK 인 Module 을 R-Port 로 가져갈 것인지 L-Port 로 가져갈 것인지
        }

        public void Unloader_Transfer_Restart_MoveType_Check()
        {
            //  Stop 했을 때의 조건들을 조합하여 시작 조건 결정

            //  1. Unloader Transfer Cycle 의 Step
            //  2. Transfer 의 Move Type
            //  3. Module Pick Up 완료 여부
            //  4. Loader 가 Stage 에 Module 을 내려놨는지 여부
            //  5. Main Work Cycle 의 동작 여부
            //  6. Main Work Cycle 의 완료 여부
            //  7. Main Work Cycle 이 완료되었다면, 양불 결과


            //int m_nRESTORE_UL_Transfer_Cycle_Step = 0;
            //int m_nRESTORE_UL_Transfer_MoveType = 0;
            //bool m_bRESTORE_UL_Transfer_fromWorkStage_Module_PickUp_Complete = false;
            //bool m_bRESTORE_LD_Transfer_toWorkStage_Module_PutDown_Complete = false;
            //int m_nRESTORE_MainWork_Cycle_Step = 0;
            //bool m_bRESTORE_MainWork_Cycle_Complete = false;
            //int m_nRESTORE_MainWork_Cycle_ResultOKNG = (int)WorkStage.MainCycle_Result.None;
            //bool m_bRESTORE_Unloader_Transfer_ModulePickUpfromWorkStage_Complete;   //  Work Stage 에서 Module Pick Up 완료 여부
            //bool m_bRESTORE_Unloader_Transfer_ModulePutDowntoStacker0_Complete;     //  Stacker0 에 Module Put Down 완료 여부
            //bool m_bRESTORE_Unloader_Transfer_ModulePutDowntoStacker1_Complete;     //  Stacker1 에 Module Put Down 완료 여부
            //bool m_bRESTORE_Unloader_Transfer_ModulePutDowntoNG_Complete;           //  NG-Port 에 Module Put Down 완료 여부        

            //  Auto Run 시 동작 조건을 결정하는 변수들. 위 Restore 조건으로 이 변수들의 값을 결정해서 Restart 하도록 한다.
            //bool m_bUnloader_Transfer_ModulePickUpfromWorkStage_Complete;   //  Work Stage 에서 Module Pick Up 완료 여부
            //bool m_bUnloader_Transfer_ModulePutDowntoStacker0_Complete;     //  Stacker0 에 Module Put Down 완료 여부
            //bool m_bUnloader_Transfer_ModulePutDowntoStacker1_Complete;     //  Stacker1 에 Module Put Down 완료 여부
            //bool m_bUnloader_Transfer_ModulePutDowntoNG_Complete;           //  NG-Port 에 Module Put Down 완료 여부


            //  1. Laser Drilling 이 진행중이면? Unloader 모든 동작 Stop
            if (m_nUL_RESTORE_LaserDrilling_Cycle_Step != (int)WorkStage.LaserDrilling_Step.None)
            {
                m_bAUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete = false;                                //  Work Stage 에서 Module Pick Up 완료 여부
                m_bAUTORUN_Unloader_Transfer_ModulePutDowntoStacker0_Complete = false;                                  //  Stacker0 에 Module Put Down 완료 여부
                m_bAUTORUN_Unloader_Transfer_ModulePutDowntoStacker1_Complete = false;                                  //  Stacker1 에 Module Put Down 완료 여부
                m_bAUTORUN_Unloader_Transfer_ModulePutDowntoNG_Complete = false;                                        //  NG-Port 에 Module Put Down 완료 여부        

                m_bStacker0_Complete = true;
                m_bStacker1_Complete = true;
            }
            //  2. Main Work Cycle 이 진행중이면? Unloader 모든 동작 Stop
            else if (m_nUL_RESTORE_MainWork_Cycle_Step != (int)WorkStage.MainWork_Step.None)
            {
                m_bAUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete = false;                                //  Work Stage 에서 Module Pick Up 완료 여부
                m_bAUTORUN_Unloader_Transfer_ModulePutDowntoStacker0_Complete = false;                                  //  Stacker0 에 Module Put Down 완료 여부
                m_bAUTORUN_Unloader_Transfer_ModulePutDowntoStacker1_Complete = false;                                  //  Stacker1 에 Module Put Down 완료 여부
                m_bAUTORUN_Unloader_Transfer_ModulePutDowntoNG_Complete = false;                                        //  NG-Port 에 Module Put Down 완료 여부        

                m_bStacker0_Complete = true;
                m_bStacker1_Complete = true;
            }
            //  3. Unloader Transfer Cycle 이 None 상태이면, 동작이 없던 상태이므로 기존 조건들 그대로 적용하여 Start 한다. 
            else if (m_nUL_RESTORE_Transfer_Step == (int)Unloader_Transfer_Step.None)
            {
                m_bAUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete = m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete;      //  Work Stage 에서 Module Pick Up 완료 여부
                m_bAUTORUN_Unloader_Transfer_ModulePutDowntoStacker0_Complete = m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePutDowntoStacker0_Complete;          //  Stacker0 에 Module Put Down 완료 여부
                m_bAUTORUN_Unloader_Transfer_ModulePutDowntoStacker1_Complete = m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePutDowntoStacker1_Complete;          //  Stacker1 에 Module Put Down 완료 여부
                m_bAUTORUN_Unloader_Transfer_ModulePutDowntoNG_Complete = m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePutDowntoNG_Complete;                      //  NG-Port 에 Module Put Down 완료 여부        
            }
            //  4. Unloader Transfer Cycle 이 None 이 아니면, 뭔가 동작을 하던 상황
            else
            {
                //  4-1. Unloader Transfer Cycle 이 Work Stage 에서 Module Pick Up 인 경우
                if (m_nUL_RESTORE_Transfer_MoveType == (int)UnloaderTransferMoveType.Cycle_WorkStage_PickUp)
                {
                    //  4-1-1. Work Stage 에서 Module Pick Up 완료했을 경우 --> Port 에 Module 을 내려놓는 Cycle 진행해야 한다.
                    if (m_bUL_RESTORE_Transfer_fromWorkStage_Module_PickUp_Complete_Flag)
                    {
                        //  4-1-1-1. Pick Up 한 Module 이 양품일 경우
                        if (m_nUL_RESTORE_MainWork_Cycle_ResultOKNG == (int)WorkStage.MainCycle_Result.OK)
                        {
                            workStage.m_bMainWorkCycle_ResultOK_toRPort = m_bUL_RESTORE_MainWorkCycle_ResultOK_toRPort;

                            //  4-1-1-1-1. R-Port 에 내려놓을지, L-Port 에 내려놓을지 결정해야 한다.
                            if (m_bUL_RESTORE_MainWorkCycle_ResultOK_toRPort)           //  R-Port 로
                            {
                                //m_bAUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete = m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete;     //  Work Stage 에서 Module Pick Up 완료 여부
                                //m_bAUTORUN_Unloader_Transfer_ModulePutDowntoStacker0_Complete = m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePutDowntoStacker0_Complete;         //  Stacker0 에 Module Put Down 완료 여부
                                m_bAUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete = true;                                                                             //  Work Stage 에서 Module Pick Up 완료 여부
                                m_bAUTORUN_Unloader_Transfer_ModulePutDowntoStacker0_Complete = true;                                                                               //  Stacker0 에 Module Put Down 완료 여부
                                m_bAUTORUN_Unloader_Transfer_ModulePutDowntoStacker1_Complete = false;                                                                              //  Stacker1 에 Module Put Down 완료 여부
                                m_bAUTORUN_Unloader_Transfer_ModulePutDowntoNG_Complete = false;                                                                                    //  NG-Port 에 Module Put Down 완료 여부        
                            }
                            else                                                        //  L-Port 로                            
                            {
                                //m_bAUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete = m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete;     //  Work Stage 에서 Module Pick Up 완료 여부
                                m_bAUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete = true;                                                                             //  Work Stage 에서 Module Pick Up 완료 여부
                                m_bAUTORUN_Unloader_Transfer_ModulePutDowntoStacker0_Complete = false;                                                                              //  Stacker0 에 Module Put Down 완료 여부
                                //m_bAUTORUN_Unloader_Transfer_ModulePutDowntoStacker1_Complete = m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePutDowntoStacker1_Complete;         //  Stacker1 에 Module Put Down 완료 여부
                                m_bAUTORUN_Unloader_Transfer_ModulePutDowntoStacker1_Complete = true;                                                                               //  Stacker1 에 Module Put Down 완료 여부
                                m_bAUTORUN_Unloader_Transfer_ModulePutDowntoNG_Complete = false;                                                                                    //  NG-Port 에 Module Put Down 완료 여부        
                            }
                        }
                        //  4-1-1-2. Pick Up 한 Module 이 불량일 경우
                        else if (m_nUL_RESTORE_MainWork_Cycle_ResultOKNG == (int)WorkStage.MainCycle_Result.NG)
                        {
                            //m_bAUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete = m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete;         //  Work Stage 에서 Module Pick Up 완료 여부
                            m_bAUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete = true;                                                                                 //  Work Stage 에서 Module Pick Up 완료 여부
                            m_bAUTORUN_Unloader_Transfer_ModulePutDowntoStacker0_Complete = false;                                                                                  //  Stacker0 에 Module Put Down 완료 여부
                            m_bAUTORUN_Unloader_Transfer_ModulePutDowntoStacker1_Complete = false;                                                                                  //  Stacker1 에 Module Put Down 완료 여부
                            //m_bAUTORUN_Unloader_Transfer_ModulePutDowntoNG_Complete = m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePutDowntoNG_Complete;                         //  NG-Port 에 Module Put Down 완료 여부        
                            m_bAUTORUN_Unloader_Transfer_ModulePutDowntoNG_Complete = true;                                                                                         //  NG-Port 에 Module Put Down 완료 여부        
                        }
                    }
                    //  4-1-2. Work Stage 에서 Module Pick Up 완료하지 못했을 경우 --> 다시 PickUp 진행해야 한다.
                    else
                    {
                        //m_bAUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete = m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete;             //  Work Stage 에서 Module Pick Up 완료 여부
                        m_bAUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete = false;                                                                                    //  Work Stage 에서 Module Pick Up 완료 여부
                        m_bAUTORUN_Unloader_Transfer_ModulePutDowntoStacker0_Complete = false;                                                                                      //  Stacker0 에 Module Put Down 완료 여부
                        m_bAUTORUN_Unloader_Transfer_ModulePutDowntoStacker1_Complete = false;                                                                                      //  Stacker1 에 Module Put Down 완료 여부
                        m_bAUTORUN_Unloader_Transfer_ModulePutDowntoNG_Complete = false;                                                                                            //  NG-Port 에 Module Put Down 완료 여부        

                        //  주의!
                        //  "loader.m_bAUTORUN_Loader_Transfer_ModulePutDowntoWorkStage_Complete" 가 "true" 가 되어야 한다.
                    }
                }
                //  4-2. Unloader Transfer Cycle 이 Stacker0 에 Module Put Down 인 경우
                else if (m_nUL_RESTORE_Transfer_MoveType == (int)UnloaderTransferMoveType.Cycle_Stacker0_PutDown)
                {
                    //  R-Port 에 내려놓는 양품만 진행됨
                    //m_bAUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete = m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete;                 //  Work Stage 에서 Module Pick Up 완료 여부
                    //m_bAUTORUN_Unloader_Transfer_ModulePutDowntoStacker0_Complete = m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePutDowntoStacker0_Complete;                     //  Stacker0 에 Module Put Down 완료 여부
                    m_bAUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete = true;                                                                                         //  Work Stage 에서 Module Pick Up 완료 여부
                    m_bAUTORUN_Unloader_Transfer_ModulePutDowntoStacker0_Complete = true;                                                                                           //  Stacker0 에 Module Put Down 완료 여부
                    m_bAUTORUN_Unloader_Transfer_ModulePutDowntoStacker1_Complete = false;                                                                                          //  Stacker1 에 Module Put Down 완료 여부
                    m_bAUTORUN_Unloader_Transfer_ModulePutDowntoNG_Complete = false;                                                                                                //  NG-Port 에 Module Put Down 완료 여부        
                }
                //  4-3. Unloader Transfer Cycle 이 Stacker1 에 Module Put Down 인 경우
                else if (m_nUL_RESTORE_Transfer_MoveType == (int)UnloaderTransferMoveType.Cycle_Stacker1_PutDown)
                {
                    //  L-Port 에 내려놓는 양품만 진행됨
                    //m_bAUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete = m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete;                 //  Work Stage 에서 Module Pick Up 완료 여부
                    m_bAUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete = true;                                                                                         //  Work Stage 에서 Module Pick Up 완료 여부
                    m_bAUTORUN_Unloader_Transfer_ModulePutDowntoStacker0_Complete = false;                                                                                          //  Stacker0 에 Module Put Down 완료 여부
                    //m_bAUTORUN_Unloader_Transfer_ModulePutDowntoStacker1_Complete = m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePutDowntoStacker1_Complete;                     //  Stacker1 에 Module Put Down 완료 여부
                    m_bAUTORUN_Unloader_Transfer_ModulePutDowntoStacker1_Complete = true;                                                                                           //  Stacker1 에 Module Put Down 완료 여부
                    m_bAUTORUN_Unloader_Transfer_ModulePutDowntoNG_Complete = false;                                                                                                //  NG-Port 에 Module Put Down 완료 여부        
                }
                //  4-4. Unloader Transfer Cycle 이 NG-Stacker 에 Module Drop 인 경우
                else if (m_nUL_RESTORE_Transfer_MoveType == (int)UnloaderTransferMoveType.Cycle_NG_PutDown)
                {
                    //  NG-Port 에 내려놓는 불량만 진행됨
                    //m_bAUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete = m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete;                 //  Work Stage 에서 Module Pick Up 완료 여부
                    m_bAUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete = true;                                                                                         //  Work Stage 에서 Module Pick Up 완료 여부
                    m_bAUTORUN_Unloader_Transfer_ModulePutDowntoStacker0_Complete = false;                                                                                          //  Stacker0 에 Module Put Down 완료 여부
                    m_bAUTORUN_Unloader_Transfer_ModulePutDowntoStacker1_Complete = false;                                                                                          //  Stacker1 에 Module Put Down 완료 여부
                    //m_bAUTORUN_Unloader_Transfer_ModulePutDowntoNG_Complete = m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePutDowntoNG_Complete;                                 //  NG-Port 에 Module Put Down 완료 여부        
                    m_bAUTORUN_Unloader_Transfer_ModulePutDowntoNG_Complete = true;                                                                                                 //  NG-Port 에 Module Put Down 완료 여부        
                }
            }
            

            //  5. Stop 후 Start 할 때는 Stacker 는 다시 동작시킬 필요 없다. 
            m_bStacker0_Complete = true;            //  true : 동작 안함,   false : 동작함
            m_bStacker1_Complete = true;            //  true : 동작 안함,   false : 동작함
        }

        #endregion

        #region Stacker Move Function (Module PickUp & PutDown 높이로 이동 -> 이건 Loader Unloader 에서 하도록 해야 할듯???)

        int Run_Stacker0Module_PutdownWaitingPos_Func()
        {
            int ret = 0;
            bool m_bRet = false;
            string m_strTemp;

            double m_dSpeed_Stacker_Fast = 0.0;
            double m_dSpeed_Stacker_Slow = 0.0;
            double m_dSpeed_Stacker_MoreSlow = 0.0;
            double m_dSpeedMag_forAccDec = 0.0;


            //  운전 중 Door 를 열면 장비 Stop
            if (m_nStacker0_ModulePutdownWaitingPos_Step >= (int)StackerModulePutdownWaitingPos_Step.Start)
            {
                //if (Config.ParamConfig.AreaSensor_Usage && (waferProbeAlignParameter.DI_AreaSensor_Detect() || waferProbeAlignParameter.DI_AlignJig_Detect()))
                //{
                //    Log.Write("CWA150SA", Equipment.User_Name, "Machine Initialize", "안전 센서 감지로 인한 장비 Stop");

                //    m_bInCycleMoving_SafetySensor_Detected = true;

                //    //  알람 정지 (LED Bar - Red Blink)
                //    Equipment.MachineStop_byAlarm = true;

                //    timer_Motion_Home.Enabled = false;
                //    m_btimer_Motion_Home_Stop = true;

                //    m_nHomeStep = (int)Home_Step.None;

                //    //MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.U, 500);
                //    //MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.V, 500);
                //    //MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.W, 500);
                //    //MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.EZ, 500);
                //    //MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.X, 500);
                //    //MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.Y, 500);
                //    //MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.VZ, 500);

                //    for (int i = 0; i < (int)AxisAjinEnum.Max; i++)
                //    {
                //        MC_Func.MC_MotorStop(i, 2000);
                //        //MC_Func.MC_EStop(i);
                //    }

                //    if (!Equipment.User_QMC_Engineer)                   //  QMC 관리자가 아닐 경우에만 Home Flag 를 false 로
                //    {
                //        m_bHomeOK = false;                              //  안전센서 감지 시 무조건 장비 초기화 해야 함
                //    }

                //    if (!Equipment.User_QMC_Engineer && Config.ParamConfig.AreaSensor_ServoOff_Usage)           //  안전센서 감지 시 Servo Off 할 경우
                //    {
                //        for (int i = 0; i < (int)AxisAjinEnum.Max; i++)
                //        {
                //            MC_Func.MC_SetServoOnOff(i, false);
                //        }
                //    }
                //}
            }

            //  자동운전 시, Stacker0 동작 조건 : TR Cycle (None), Stacker0 Cycle (None), TR 이 Module 을 내려놨을 때
            if (Equipment.AutoRunStatus &&

                m_nUnloader_Transfer_Step == (int)Unloader_Transfer_Step.None &&
                m_nStacker0_ModulePutdownWaitingPos_Step == (int)StackerModulePutdownWaitingPos_Step.None &&

                !m_bStacker0_Complete)                                                //  Stacker0 동작 완료되지 않은 상태 (TR 이 Module 을 내려놓은 후 false 로 변경됨)
            {
                m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.Start;
            }

            switch (m_nStacker0_ModulePutdownWaitingPos_Step)
            {
                case (int)StackerModulePutdownWaitingPos_Step.Start:
                    Log.Write("SLD-200", Equipment.User_Name, "UL Stacker0 Work Pos. Set", "시작");

                    Equipment.MachineStop_byAlarm = false;

                    //  Unlaoder Stacker Z 축 모터 전체 Stop
                    MC_Func.MC_MotorStop((int)UnloaderParameter.AxisAjinEnum.Z0, 2000);
                    m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.Process_Condition_Check;
                    break;

                case (int)StackerModulePutdownWaitingPos_Step.Process_Condition_Check:
                    Log.Write("SLD-200", Equipment.User_Name, "UL Stacker0 Work Pos. Set", "동작 조건 확인");

                    if (m_nUnloader_Transfer_Step > (int)Unloader_Transfer_Step.None)                                                       //  Transfer Cycle 이 동작중
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Stacker0 Work Pos. Set", "Transfer 가 동작중이므로 Stacker 동작 중지.");

                        //  일단 Out. (Transfer 동작이 완료되면 진행하도록 대기할 것인지는 테스트 하면서 결정하기로 함)
                        m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.None;
                    }
                    else //if (unloaderParameter.DI_Unloader_Stacker_MaterialCheck((int)UnloaderParameter.StackerTable.Stacker_0))               //  우측 Port 에 Module 이 감지되어 있을 때만 진행
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Stacker0 Work Pos. Set", "Stacker_0 Module Exist 센서 감지됨");
                        if (!unloaderParameter.DI_Unloader_Stacker_FullCheck((int)UnloaderParameter.StackerTable.Stacker_0))             //  감지 시 Off
                        {
                            //  Full Sensor 감지 상태일 경우 (Off 될 때 까지 내림 -> Off 되면 Stop -> 느리게 Up -> On 되면 Stop -> 느리게 Down -> Off 되면 Stop -> 더 느리게 Up -> On 되면 Stop -> 완료)
                            m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType1_FastDown;
                        }
                        else
                        {
                            //  Full Sensor 감지되지 않는 상태일 경우 (Up -> On 되면 Stop -> 느리게 Down -> Off 되면 Stop -> 더 느리게 Up -> On 되면 Stop -> 완료)
                            m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType2_FastUp;
                        }
                    }
                    break;


                /// <summary>
                /// Full Sensor 감지 상태일 경우 - 시작
                /// </summary>
                case (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType1_FastDown:                            //  Stacker Z 축, 빠르게 내림 (최 하단까지)

                    StackerModulePutdownWaitingPos_Step_StackerZ_MoveType1_FastDown(out m_dSpeed_Stacker_Fast, out m_dSpeedMag_forAccDec);
                    m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType1_FastDown_DoneCheck;
                    break;

                case (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType1_FastDown_DoneCheck:                       //  Stacker Z 축, 빠르게 내림, 이동 완료 확인

                    if (unloaderParameter.DI_Unloader_Stacker_FullCheck((int)UnloaderParameter.StackerTable.Stacker_0))          //  Full 감지 센서가 Off 되면 Stop           //  감지 시 Off
                    {
                        MC_Func.MC_MotorStop((int)nAxis.Z0, 2000);

                        m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType1_SlowUp;
                    }
                    else if (MC_Func.MC_GetDone((int)nAxis.Z0) && 
                        MC_Func.MC_PosTolerance((int)nAxis.Z0, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Stacker0 Work Pos. Set", "Stacker0 Z 축, Bottom 위치까지 이동 완료");

                        if (!unloaderParameter.DI_Unloader_Stacker_FullCheck((int)UnloaderParameter.StackerTable.Stacker_0))         //  감지 시 Off
                        {
                            m_strTemp = "Stacker0 Z 축, Bottom 위치까지 이동했으나 Full 센서 On 상태";
                            Log.Write("SLD-200", Equipment.User_Name, "Unload_StackerModulePutdownWaitingPos_Step", m_strTemp);
                            return AlarmPost(AlarmKey.UL_Staker0_Too_Many_Module);
                        }
                        else
                        {
                            m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType1_SlowUp;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULSZ0) > 60000)
                    {
                        m_strTemp = "Stacker0 Z 축, Full 센서가 Off 되는 위치까지 이동 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unload_StackerModulePutdownWaitingPos_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Staker0_Too_Many_Module);
                    }
                    break;


                case (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType1_SlowUp:                               //  Stacker Z 축, 느리게 올림 (최 상단까지)

                    if (MC_Func.MC_GetDone((int)nAxis.Z0))
                    {
                        StackerModulePutdownWaitingPos_Step_StackerZ_MoveType1_SlowUp(out m_dSpeed_Stacker_Slow, out m_dSpeedMag_forAccDec);

                        m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType1_SlowUp_DoneCheck;
                    }
                    break;


                case (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType1_SlowUp_DoneCheck:                          //  Stacker Z 축, 느리게 올림, 이동 완료 확인

                    if (!unloaderParameter.DI_Unloader_Stacker_FullCheck((int)UnloaderParameter.StackerTable.Stacker_0))         //  감지 시 Off
                    {
                        MC_Func.MC_MotorStop((int)nAxis.Z0, 2000);

                        m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType1_Slow2Down;
                    }
                    else if (MC_Func.MC_GetDone((int)nAxis.Z0) && 
                        MC_Func.MC_PosTolerance((int)nAxis.Z0, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Stacker0 Work Pos. Set", "Stacker0 Z 축, Top 위치까지 이동 완료");

                        if (unloaderParameter.DI_Unloader_Stacker_FullCheck((int)UnloaderParameter.StackerTable.Stacker_0))            //  감지 시 Off
                        {
                            m_strTemp = "Stacker0 Z 축, Top 위치까지 이동했으나 Full 센서 Off 상태";
                            Log.Write("SLD-200", Equipment.User_Name, "Unload_StackerModulePutdownWaitingPos_Step", m_strTemp);
                            return AlarmPost(AlarmKey.UL_Staker0_Too_Many_Module);
                        }
                        else
                        {
                            m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType1_Slow2Down;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULSZ0) > 60000)
                    {
                        m_strTemp = "Stacker0 Z 축, Full 센서가 On 되는 위치까지 이동 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unload_StackerModulePutdownWaitingPos_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Staker0_Z_Full_Sensor_On_Fail);
                    }
                    break;


                case (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType1_Slow2Down:                            //  Stacker Z 축, 더 느리게 내림 (최 하단까지)

                    if (MC_Func.MC_GetDone((int)nAxis.Z0))
                    {
                        StackerModulePutdownWaitingPos_Step_StackerZ_MoveType1_Slow2Down(out m_dSpeed_Stacker_MoreSlow, out m_dSpeedMag_forAccDec);

                        TickCount_Start((int)TickType.TICK_ULSZ0);

                        m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType1_Slow2Down_DoneCheck;
                    }
                    break;

                case (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType1_Slow2Down_DoneCheck:                       //  Stacker Z 축, 더 느리게 내림, 이동 완료 확인

                    if (unloaderParameter.DI_Unloader_Stacker_FullCheck((int)UnloaderParameter.StackerTable.Stacker_0))            //  감지 시 Off
                    {
                        MC_Func.MC_MotorStop((int)nAxis.Z0, 1000);

                        //m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType1_Slow3Up;
                        //m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.Complete;                           //  다시 올리지 않고 완료. (Full 센서가 감지되지 않는 위치에서 Unloading 하도록 한다.)
                        m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_Move_OverDistance;
                    }
                    else if (MC_Func.MC_GetDone((int)nAxis.Z0) && MC_Func.MC_PosTolerance((int)nAxis.Z0, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Stacker0 Work Pos. Set", "Stacker0 Z 축, Bottom 위치까지 이동 완료");

                        if (!unloaderParameter.DI_Unloader_Stacker_FullCheck((int)UnloaderParameter.StackerTable.Stacker_0))         //  감지 시 Off
                        {
                            m_strTemp = "Stacker0 Z 축, Bottom 위치까지 이동했으나 Full 센서 On 상태";
                            Log.Write("SLD-200", Equipment.User_Name, "Unload_StackerModulePutdownWaitingPos_Step", m_strTemp);
                            return AlarmPost(AlarmKey.UL_Staker0_Too_Many_Module);
                        }
                        else
                        {
                            //m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.Complete;
                            m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_Move_OverDistance;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULSZ0) > 60000)
                    {
                        m_strTemp = "Stacker0 Z 축, Full 센서가 Off 되는 위치까지 이동 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unload_StackerModulePutdownWaitingPos_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Staker0_Too_Many_Module);

                    }
                    break;


                case (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType1_Slow3Up:                               //  Stacker Z 축, 더더 느리게 올림 (최 상단까지)

                    if (MC_Func.MC_GetDone((int)nAxis.Z0))
                    {
                        StackerModulePutdownWaitingPos_Step_StackerZ_MoveType1_Slow3Up(out m_dSpeed_Stacker_MoreSlow, out m_dSpeedMag_forAccDec);

                        m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType1_Slow3Up_DoneCheck;
                    }
                    break;


                case (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType1_Slow3Up_DoneCheck:                          //  Stacker Z 축, 더더 느리게 올림, 이동 완료 확인

                    if (!unloaderParameter.DI_Unloader_Stacker_FullCheck((int)UnloaderParameter.StackerTable.Stacker_0))         //  감지 시 Off
                    {
                        MC_Func.MC_MotorStop((int)nAxis.Z0, 2000);

                        //m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.Complete;
                        m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_Move_OverDistance;
                    }
                    else if (MC_Func.MC_GetDone((int)nAxis.Z0) 
                        && MC_Func.MC_PosTolerance((int)nAxis.Z0, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Stacker0 Work Pos. Set", "Stacker0 Z 축, Top 위치까지 이동 완료");

                        if (unloaderParameter.DI_Unloader_Stacker_FullCheck((int)UnloaderParameter.StackerTable.Stacker_0))            //  감지 시 Off
                        {
                            m_strTemp = "Stacker0 Z 축, Top 위치까지 이동했으나 Full 센서 Off 상태";
                            Log.Write("SLD-200", Equipment.User_Name, "Unload_StackerModulePutdownWaitingPos_Step", m_strTemp);
                            return AlarmPost(AlarmKey.UL_Staker0_Too_Many_Module);
                        }
                        else
                        {
                            //m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.Complete;
                            m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_Move_OverDistance;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULSZ0) > 60000)
                    {
                        m_strTemp = "Stacker0 Z 축, Full 센서가 On 되는 위치까지 이동 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unload_StackerModulePutdownWaitingPos_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Staker0_Too_Many_Module);
                    }
                    break;
                /// <summary>
                /// Full Sensor 감지 상태일 경우 - 완료
                /// </summary>



                /// <summary>
                /// Full Sensor 감지되지 않는 상태일 경우 - 시작
                /// </summary>
                case (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType2_FastUp:                               //  Stacker Z 축, 빠르게 올림 (최 상단까지)

                    if (MC_Func.MC_GetDone((int)nAxis.Z0))
                    {
                        StackerModulePutdownWaitingPos_Step_StackerZ_MoveType2_FastUp(out m_dSpeed_Stacker_Fast, out m_dSpeedMag_forAccDec);

                        m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType2_FastUp_DoneCheck;
                    }
                    break;


                case (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType2_FastUp_DoneCheck:                          //  Stacker Z 축, 빠르게 올림, 이동 완료 확인

                    if (!unloaderParameter.DI_Unloader_Stacker_FullCheck((int)UnloaderParameter.StackerTable.Stacker_0))         //  감지 시 Off
                    {
                        MC_Func.MC_MotorStop((int)nAxis.Z0, 2000);

                        m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType2_Slow2Down;
                    }
                    else if (MC_Func.MC_GetDone((int)nAxis.Z0) && 
                        MC_Func.MC_PosTolerance((int)nAxis.Z0, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Stacker0 Work Pos. Set", "Stacker0 Z 축, Top 위치까지 이동 완료");

                        if (unloaderParameter.DI_Unloader_Stacker_FullCheck((int)UnloaderParameter.StackerTable.Stacker_0))            //  감지 시 Off
                        {
                            m_strTemp = "Stacker0 Z 축, Top 위치까지 이동했으나 Full 센서 Off 상태";
                            Log.Write("SLD-200", Equipment.User_Name, "Unload_StackerModulePutdownWaitingPos_Step", m_strTemp);
                            return AlarmPost(AlarmKey.UL_Staker0_Z_Full_Sensor_On_Fail);
                        }
                        else
                        {
                            m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType2_Slow2Down;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULSZ0) > 60000)
                    {
                        m_strTemp = "Stacker0 Z 축, Full 센서가 On 되는 위치까지 이동 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unload_StackerModulePutdownWaitingPos_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Staker0_Z_Full_Sensor_On_Fail);
                    }
                    break;


                case (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType2_Slow2Down:                            //  Stacker Z 축, 더 느리게 내림 (최 하단까지)

                    if (MC_Func.MC_GetDone((int)nAxis.Z0))
                    {
                        StackerModulePutdownWaitingPos_Step_StackerZ_MoveType2_Slow2Down(out m_dSpeed_Stacker_MoreSlow, out m_dSpeedMag_forAccDec);

                        m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType2_Slow2Down_DoneCheck;
                    }
                    break;


                case (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType2_Slow2Down_DoneCheck:                       //  Stacker Z 축, 더 느리게 내림, 이동 완료 확인

                    if (unloaderParameter.DI_Unloader_Stacker_FullCheck((int)UnloaderParameter.StackerTable.Stacker_0))            //  감지 시 Off
                    {
                        MC_Func.MC_MotorStop((int)nAxis.Z0, 1000);

                        //m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType2_Slow3Up;
                        //m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.Complete;                           //  다시 올리지 않고 완료. (Full 센서가 감지되지 않는 위치에서 Unloading 하도록 한다.)
                        m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_Move_OverDistance;
                    }
                    else if (MC_Func.MC_GetDone((int)nAxis.Z0) && MC_Func.MC_PosTolerance((int)nAxis.Z0, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Stacker0 Work Pos. Set", "Stacker0 Z 축, Bottom 위치까지 이동 완료");

                        if (!unloaderParameter.DI_Unloader_Stacker_FullCheck((int)UnloaderParameter.StackerTable.Stacker_0))         //  감지 시 Off
                        {
                            m_strTemp = "Stacker0 Z 축, Bottom 위치까지 이동했으나 Full 센서 On 상태";
                            Log.Write("SLD-200", Equipment.User_Name, "Unload_StackerModulePutdownWaitingPos_Step", m_strTemp);
                            return AlarmPost(AlarmKey.UL_Staker0_Too_Many_Module);
                        }
                        else
                        {
                            //m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.Complete;
                            m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_Move_OverDistance;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULSZ0) > 60000)
                    {
                        m_strTemp = "Stacker0 Z 축, Full 센서가 Off 되는 위치까지 이동 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unload_StackerModulePutdownWaitingPos_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Staker0_Too_Many_Module);
                    }
                    break;


                case (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType2_Slow3Up:                               //  Stacker Z 축, 더더 느리게 올림 (최 상단까지)

                    if (MC_Func.MC_GetDone((int)nAxis.Z0))
                    {
                        StackerModulePutdownWaitingPos_Step_StackerZ_MoveType2_Slow3Up(out m_dSpeed_Stacker_MoreSlow, out m_dSpeedMag_forAccDec);

                        TickCount_Start((int)TickType.TICK_ULSZ0);

                        m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType2_Slow3Up_DoneCheck;
                    }
                    break;


                case (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType2_Slow3Up_DoneCheck:                          //  Stacker Z 축, 더더 느리게 올림, 이동 완료 확인

                    if (!unloaderParameter.DI_Unloader_Stacker_FullCheck((int)UnloaderParameter.StackerTable.Stacker_0))         //  감지 시 Off
                    {
                        MC_Func.MC_MotorStop((int)nAxis.Z0, 2000);

                        //m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.Complete;
                        m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_Move_OverDistance;
                    }
                    else if (MC_Func.MC_GetDone((int)nAxis.Z0) && 
                        MC_Func.MC_PosTolerance((int)nAxis.Z0, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Stacker0 Work Pos. Set", "Stacker0 Z 축, Top 위치까지 이동 완료");

                        if (unloaderParameter.DI_Unloader_Stacker_FullCheck((int)UnloaderParameter.StackerTable.Stacker_0))            //  감지 시 Off
                        {
                            m_strTemp = "Stacker0 Z 축, Top 위치까지 이동했으나 Full 센서 Off 상태";
                            Log.Write("SLD-200", Equipment.User_Name, "Unload_StackerModulePutdownWaitingPos_Step", m_strTemp);
                            return AlarmPost(AlarmKey.UL_Staker0_Z_Full_Sensor_On_Fail);
                        }
                        else
                        {
                            //m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.Complete;
                            m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_Move_OverDistance;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULSZ0) > 60000)
                    {
                        m_strTemp = "Stacker0 Z 축, Full 센서가 On 되는 위치까지 이동 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unload_StackerModulePutdownWaitingPos_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Staker0_Z_Full_Sensor_On_Z_Move_Fail);
                    }
                    break;
                /// <summary>
                /// Full Sensor 감지되지 않는 상태일 경우 - 완료
                /// </summary>
                /// 


                /// <summary>
                /// 최종 위치 이동 후 추가 이동 - 시작
                /// </summary>
                case (int)StackerModulePutdownWaitingPos_Step.StackerZ_Move_OverDistance:                               //  Stacker Z 축, 최종 감지 위치에서 추가로 이동

                    if (MC_Func.MC_GetDone((int)nAxis.Z0))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Stacker0 Work Pos. Set", "Stacker0 Z 축, 최종 위치에서 추가 이동 시작. (아래로 3mm)");

                        unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Stacker0_Top");

                        //  Target Position 변경 : 현재 위치에서 추가 이동
                        unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0] = MC_Func.MC_GetEncPos((int)nAxis.Z0) - 3.0;

                        if (unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0] < loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_RPort_ReadyPos].UL_Stacker_Z0)
                        {
                            unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_RPort_ReadyPos].UL_Stacker_Z0;
                        }

                        //  속도 (기본 속도 / 4)
                        //m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z0].Common_MoveSpeed / 4.0;
                        m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z0].Common_Speed_Fine / 4.0;

                        //  가감속 배율
                        m_dSpeedMag_forAccDec = 2.0;

                        MC_Func.MC_MovePosition((int)nAxis.Z0,
                                            unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
                                            m_dSpeed_Stacker_MoreSlow,
                                            m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec,
                                            m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec);

                        TickCount_Start((int)TickType.TICK_ULSZ0);

                        m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_Move_OverDistance_DoneCheck;
                    }
                    break;


                case (int)StackerModulePutdownWaitingPos_Step.StackerZ_Move_OverDistance_DoneCheck:                          //  Stacker Z 축, 최종 감지 위치에서 추가로 이동 완료 확인

                    //if (MC_Func.MC_GetDone((int)nAxis.Z0) && MC_Func.MC_PosTolerance((int)nAxis.Z0, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0]))
                    if (MC_Func.MC_GetDone((int)nAxis.Z0))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Stacker0 Work Pos. Set", "Stacker0 Z 축, Top 위치 아래 3mm 까지 이동 완료");

                        m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.Complete;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULSZ0) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Stacker0 Work Pos. Set", "Stacker0 Z 축, Top 위치 아래 3mm 까지 이동 실패. (Timeout)");

                        return AlarmPost(AlarmKey.UL_Staker0_MoveZ_Timeout);

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;
                        //return AlarmPost(AlarmKey.UL_Stacker0_FullSensor_Off_MoveFail);
                        m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.None;

                        MessageBox.Show("LD Stacker0 Z 축, Top 위치 Over 까지 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                /// <summary>
                /// 최종 위치 이동 후 추가 이동 - 완료
                /// </summary>


                case (int)StackerModulePutdownWaitingPos_Step.Complete:

                    m_strTemp = "===  UL Stacker0 작업위치 이동 완료  ===";
                    Log.Write("SLD-200", Equipment.User_Name, "Unload_StackerModulePutdownWaitingPos_Step", m_strTemp);

                    m_bStacker0_Complete = true;
                    m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.None;
                    break;
            }
            return 0;
        }

        private void StackerModulePutdownWaitingPos_Step_StackerZ_MoveType2_Slow3Up(out double m_dSpeed_Stacker_MoreSlow, out double m_dSpeedMag_forAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "UL Stacker0 Work Pos. Set", "Stacker0 Z 축, Full 센서가 On 되는 위치까지 이동 시작 (저속 / 2)");

            unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Stacker0_Top");

            //  Target Position 변경 : 맨 위로 올라가는 위치
            unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_RPort_TopPos].UL_Stacker_Z0;

            //  속도 (기본 속도 / 4)
            //m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z0].Common_MoveSpeed / 4.0;
            m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z0].Common_Speed_Fine / 4.0;

            //  가감속 배율
            m_dSpeedMag_forAccDec = 2.0;

            MC_Func.MC_MovePosition((int)nAxis.Z0,
                                unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
                                m_dSpeed_Stacker_MoreSlow,
                                m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec,
                                m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec);
        }

        private void StackerModulePutdownWaitingPos_Step_StackerZ_MoveType2_Slow2Down(out double m_dSpeed_Stacker_MoreSlow, out double m_dSpeedMag_forAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "UL Stacker0 Work Pos. Set", "Stacker0 Z 축, Full 센서가 Off 되는 위치까지 이동 시작 (저속)");

            unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Stacker0_Bottom");

            //  Target Position 변경 : 맨 아래로 내려가는 위치
            unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_RPort_ReadyPos].UL_Stacker_Z0;

            //  속도 (기본 속도 / 3)
            //m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z0].Common_MoveSpeed / 3.0;
            m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z0].Common_Speed_Fine / 3.0;

            //  가감속 배율
            m_dSpeedMag_forAccDec = 2.0;

            MC_Func.MC_MovePosition((int)nAxis.Z0,
                                unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
                                m_dSpeed_Stacker_MoreSlow,
                                m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec,
                                m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec);

            //MC_Func.MC_MovePosition((int)UnloaderParameter.AxisAjinEnum.Z0,
            //                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dVel[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dAcc[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dDec[(int)UnloaderParameter.MotionKey.Z0]);

            TickCount_Start((int)TickType.TICK_ULSZ0);
        }

        private void StackerModulePutdownWaitingPos_Step_StackerZ_MoveType2_FastUp(out double m_dSpeed_Stacker_Fast, out double m_dSpeedMag_forAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "UL Stacker0 Work Pos. Set", "Stacker0 Z 축, Full 센서가 On 되는 위치까지 이동 시작 (고속)");

            unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Stacker0_Top");

            //  Target Position 변경 : 맨 위로 올라가는 위치
            unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_RPort_TopPos].UL_Stacker_Z0;

            //  속도 (기본 속도)
            m_dSpeed_Stacker_Fast = Equipment.stAxisParam[(int)nAxis.Z0].Common_Speed_Fine;

            //  가감속 배율
            m_dSpeedMag_forAccDec = 2.0;

            MC_Func.MC_MovePosition((int)nAxis.Z0,
                                unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
                                m_dSpeed_Stacker_Fast,
                                m_dSpeed_Stacker_Fast * m_dSpeedMag_forAccDec,
                                m_dSpeed_Stacker_Fast * m_dSpeedMag_forAccDec);

            //MC_Func.MC_MovePosition((int)UnloaderParameter.AxisAjinEnum.Z0,
            //                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dVel[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dAcc[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dDec[(int)UnloaderParameter.MotionKey.Z0]);

            TickCount_Start((int)TickType.TICK_ULSZ0);
        }

        private void StackerModulePutdownWaitingPos_Step_StackerZ_MoveType1_Slow3Up(out double m_dSpeed_Stacker_MoreSlow, out double m_dSpeedMag_forAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "UL Stacker0 Work Pos. Set", "Stacker0 Z 축, Full 센서가 On 되는 위치까지 이동 시작 (저속 / 2)");

            unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Stacker0_Top");

            //  Target Position 변경 : 맨 위로 올라가는 위치
            unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_RPort_TopPos].UL_Stacker_Z0;

            //  속도 (기본 속도 / 4)
            //m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z0].Common_MoveSpeed / 4.0;
            m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z0].Common_Speed_Fine / 4.0;

            //  가감속 배율
            m_dSpeedMag_forAccDec = 2.0;

            MC_Func.MC_MovePosition((int)nAxis.Z0,
                                unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
                                m_dSpeed_Stacker_MoreSlow,
                                m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec,
                                m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec);

            //MC_Func.MC_MovePosition((int)UnloaderParameter.AxisAjinEnum.Z0,
            //                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dVel[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dAcc[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dDec[(int)UnloaderParameter.MotionKey.Z0]);

            TickCount_Start((int)TickType.TICK_ULSZ0);
        }

        private void StackerModulePutdownWaitingPos_Step_StackerZ_MoveType1_Slow2Down(out double m_dSpeed_Stacker_MoreSlow, out double m_dSpeedMag_forAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "UL Stacker0 Work Pos. Set", "Stacker0 Z 축, Full 센서가 Off 되는 위치까지 이동 시작 (저속)");

            unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Stacker0_Bottom");

            //  Target Position 변경 : 맨 아래로 내려가는 위치
            unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_RPort_ReadyPos].UL_Stacker_Z0;

            //  속도 (기본 속도 / 3)
            //m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z0].Common_MoveSpeed / 3.0;
            m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z0].Common_Speed_Fine / 3.0;

            //  가감속 배율
            m_dSpeedMag_forAccDec = 2.0;

            MC_Func.MC_MovePosition((int)nAxis.Z0,
                                unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
                                m_dSpeed_Stacker_MoreSlow,
                                m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec,
                                m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec);
        }

        private void StackerModulePutdownWaitingPos_Step_StackerZ_MoveType1_SlowUp(out double m_dSpeed_Stacker_Slow, out double m_dSpeedMag_forAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "UL Stacker0 Work Pos. Set", "Stacker0 Z 축, Full 센서가 On 되는 위치까지 이동 시작 (중속)");

            unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Stacker0_Top");

            //  Target Position 변경 : 맨 위로 올라가는 위치
            unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_RPort_TopPos].UL_Stacker_Z0;

            //  속도 (기본 속도 / 2)
            m_dSpeed_Stacker_Slow = Equipment.stAxisParam[(int)nAxis.Z0].Common_Speed_Fine / 2.0;

            //  가감속 배율
            m_dSpeedMag_forAccDec = 2.0;

            MC_Func.MC_MovePosition((int)nAxis.Z0,
                                unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
                                m_dSpeed_Stacker_Slow,
                                m_dSpeed_Stacker_Slow * m_dSpeedMag_forAccDec,
                                m_dSpeed_Stacker_Slow * m_dSpeedMag_forAccDec);

            //MC_Func.MC_MovePosition((int)UnloaderParameter.AxisAjinEnum.Z0,
            //                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dVel[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dAcc[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dDec[(int)UnloaderParameter.MotionKey.Z0]);

            TickCount_Start((int)TickType.TICK_ULSZ0);
        }

        private void StackerModulePutdownWaitingPos_Step_StackerZ_MoveType1_FastDown(out double m_dSpeed_Stacker_Fast, out double m_dSpeedMag_forAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "UL Stacker0 Work Pos. Set", "Stacker0 Z 축, Full 센서가 Off 되는 위치까지 이동 시작 (고속)");

            unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Stacker0_Bottom");

            //  Target Position 변경 : 맨 아래로 내려가는 위치
            unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_RPort_ReadyPos].UL_Stacker_Z0;

            //  속도 (기본 속도)
            m_dSpeed_Stacker_Fast = Equipment.stAxisParam[(int)nAxis.Z0].Common_Speed_Fine;

            //  가감속 배율
            m_dSpeedMag_forAccDec = 2.0;

            MC_Func.MC_MovePosition((int)nAxis.Z0,
                                unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
                                m_dSpeed_Stacker_Fast,
                                m_dSpeed_Stacker_Fast * m_dSpeedMag_forAccDec,
                                m_dSpeed_Stacker_Fast * m_dSpeedMag_forAccDec);


            TickCount_Start((int)TickType.TICK_ULSZ0);
        }

        protected int AlarmPost(AlarmKey AlarmCode)
        {
            
            Alarm alarm = GetAlarm((int)AlarmCode);
            if (alarm.Grade.Equals("Error"))
            {
                this.m_UnloaderWork_Start = false;
            }
            //MessageBox.Show(alarm.Cause);
            AlarmManager.Instance.ShowAlarm(alarm);
            return alarm.Code;
        }
        int  Run_Stacker1Module_PutdownWaitingPos_Func()
        {
            int ret = 0;
            bool m_bRet = false;
            string m_strTemp;

            double m_dSpeed_Stacker_Fast = 0.0;
            double m_dSpeed_Stacker_Slow = 0.0;
            double m_dSpeed_Stacker_MoreSlow = 0.0;
            double m_dSpeedMag_forAccDec = 0.0;


            //  운전 중 Door 를 열면 장비 Stop
            if (m_nStacker1_ModulePutdownWaitingPos_Step >= (int)StackerModulePutdownWaitingPos_Step.Start)
            {
                
            }


            //  자동운전 시, Stacker1 동작 조건 : TR Cycle (None), Stacker0 Cycle (None), TR 이 Module 을 내려놨을 때
            if (Equipment.AutoRunStatus &&

                m_nUnloader_Transfer_Step == (int)Unloader_Transfer_Step.None &&
                m_nStacker1_ModulePutdownWaitingPos_Step == (int)StackerModulePutdownWaitingPos_Step.None &&

                !m_bStacker1_Complete)                                                //  Stacker1 동작 완료되지 않은 상태 (TR 이 Module 을 내려놓은 후 false 로 변경됨)
            {
                m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.Start;
            }


            switch (m_nStacker1_ModulePutdownWaitingPos_Step)
            {
                case (int)StackerModulePutdownWaitingPos_Step.Start:
                    Log.Write("SLD-200", Equipment.User_Name, "UL Stacker1 Work Pos. Set", "시작");

                    Equipment.MachineStop_byAlarm = false;
                    
                    MC_Func.MC_MotorStop((int)UnloaderParameter.AxisAjinEnum.Z1, 2000);
                    m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.Process_Condition_Check;
                    break;


                case (int)StackerModulePutdownWaitingPos_Step.Process_Condition_Check:
                    Log.Write("SLD-200", Equipment.User_Name, "UL Stacker1 Work Pos. Set", "동작 조건 확인");

                    if (m_nUnloader_Transfer_Step > (int)Unloader_Transfer_Step.None)                                                       //  Transfer Cycle 이 동작중
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Stacker1 Work Pos. Set", "Transfer 가 동작중이므로 Stacker 동작 중지.");

                        //  일단 Out. (Transfer 동작이 완료되면 진행하도록 대기할 것인지는 테스트 하면서 결정하기로 함)
                        m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.None;
                    }
                    else //if (unloaderParameter.DI_Unloader_Stacker_MaterialCheck((int)UnloaderParameter.StackerTable.Stacker_1))               //  좌측 Port 에 Module 이 감지되어 있을 때만 진행
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Stacker1 Work Pos. Set", "Stacker_1 Module Exist 센서 감지됨");

                        if (!unloaderParameter.DI_Unloader_Stacker_FullCheck((int)UnloaderParameter.StackerTable.Stacker_1))         //  감지 시 Off
                        {
                            //  Full Sensor 감지 상태일 경우 (Off 될 때 까지 내림 -> Off 되면 Stop -> 느리게 Up -> On 되면 Stop -> 느리게 Down -> Off 되면 Stop -> 더 느리게 Up -> On 되면 Stop -> 완료)
                            m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType1_FastDown;
                        }
                        else
                        {
                            //  Full Sensor 감지되지 않는 상태일 경우 (Up -> On 되면 Stop -> 느리게 Down -> Off 되면 Stop -> 더 느리게 Up -> On 되면 Stop -> 완료)
                            m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType2_FastUp;
                        }
                    }
                    break;


                /// <summary>
                /// Full Sensor 감지 상태일 경우 - 시작
                /// </summary>
                case (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType1_FastDown:                            //  Stacker Z 축, 빠르게 내림 (최 하단까지)

                    if (MC_Func.MC_GetDone((int)nAxis.Z1))
                    {
                        StackerModulePutdownWaitingPos_Step_tackerZ_MoveType1_FastDown(out m_dSpeed_Stacker_Fast, out m_dSpeedMag_forAccDec);
                        m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType1_FastDown_DoneCheck;
                    }
                    break;


                case (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType1_FastDown_DoneCheck:                       //  Stacker Z 축, 빠르게 내림, 이동 완료 확인

                    if (unloaderParameter.DI_Unloader_Stacker_FullCheck((int)UnloaderParameter.StackerTable.Stacker_1))          //  Full 감지 센서가 Off 되면 Stop           //  감지 시 Off
                    {
                        MC_Func.MC_MotorStop((int)nAxis.Z1, 2000);

                        m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType1_SlowUp;
                    }
                    else if (MC_Func.MC_GetDone((int)nAxis.Z1) && 
                        MC_Func.MC_PosTolerance((int)nAxis.Z1, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z1]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Stacker1 Work Pos. Set", "Stacker1 Z 축, Bottom 위치까지 이동 완료");

                        if (!unloaderParameter.DI_Unloader_Stacker_FullCheck((int)UnloaderParameter.StackerTable.Stacker_1))         //  감지 시 Off
                        {
                            m_strTemp = "Stacker1 Z 축, Bottom 위치까지 이동했으나 Full 센서 On 상태";
                            Log.Write("SLD-200", Equipment.User_Name, "Unload_StackerModulePutdownWaitingPos_Step", m_strTemp);
                            return AlarmPost(AlarmKey.UL_Staker1_Too_Many_Module);
                        }
                        else
                        {
                            m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType1_SlowUp;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULSZ1) > 60000)
                    {
                        m_strTemp = "Stacker1 Z 축, Full 센서가 Off 되는 위치까지 이동 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unload_StackerModulePutdownWaitingPos_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Staker1_Z_Full_Sensor_On_Z_Move_Fail);
                    }
                    break;


                case (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType1_SlowUp:                               //  Stacker Z 축, 느리게 올림 (최 상단까지)

                    if (MC_Func.MC_GetDone((int)nAxis.Z1))
                    {
                        StackerModulePutdownWaitingPosStepStackerZMoveType1SlowUp(out m_dSpeed_Stacker_Slow, out m_dSpeedMag_forAccDec);
                        TickCount_Start((int)TickType.TICK_ULSZ1);
                        m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType1_SlowUp_DoneCheck;
                    }
                    break;


                case (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType1_SlowUp_DoneCheck:                          //  Stacker Z 축, 느리게 올림, 이동 완료 확인

                    if (!unloaderParameter.DI_Unloader_Stacker_FullCheck((int)UnloaderParameter.StackerTable.Stacker_1))         //  감지 시 Off
                    {
                        MC_Func.MC_MotorStop((int)nAxis.Z1, 2000);

                        m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType1_Slow2Down;
                    }
                    else if (MC_Func.MC_GetDone((int)nAxis.Z1) && MC_Func.MC_PosTolerance((int)nAxis.Z1, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z1]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Stacker1 Work Pos. Set", "Stacker1 Z 축, Top 위치까지 이동 완료");

                        if (unloaderParameter.DI_Unloader_Stacker_FullCheck((int)UnloaderParameter.StackerTable.Stacker_1))            //  감지 시 Off
                        {
                            m_strTemp = "Stacker1 Z 축, Top 위치까지 이동했으나 Full 센서 Off 상태)";
                            Log.Write("SLD-200", Equipment.User_Name, "Unload_StackerModulePutdownWaitingPos_Step", m_strTemp);
                            return AlarmPost(AlarmKey.UL_Staker1_Z_Full_Sensor_On_Z_Move_Fail);
                        }
                        else
                        {
                            m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType1_Slow2Down;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULSZ1) > 60000)
                    {
                        m_strTemp = "Stacker1 Z 축, Full 센서가 On 되는 위치까지 이동 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unload_StackerModulePutdownWaitingPos_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Staker1_Z_Full_Sensor_On_Z_Move_Fail);
                    }
                    break;


                case (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType1_Slow2Down:                            //  Stacker Z 축, 더 느리게 내림 (최 하단까지)

                    if (MC_Func.MC_GetDone((int)nAxis.Z1))
                    {
                        StackerModulePutdownWaitingPos_Step_tackerZ_MoveType1_Slow2Down(out m_dSpeed_Stacker_MoreSlow, out m_dSpeedMag_forAccDec);

                        m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType1_Slow2Down_DoneCheck;
                    }
                    break;


                case (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType1_Slow2Down_DoneCheck:                       //  Stacker Z 축, 더 느리게 내림, 이동 완료 확인

                    if (unloaderParameter.DI_Unloader_Stacker_FullCheck((int)UnloaderParameter.StackerTable.Stacker_1))            //  감지 시 Off
                    {
                        MC_Func.MC_MotorStop((int)nAxis.Z1, 1000);

                        //m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType1_Slow3Up;
                        //m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.Complete;                           //  다시 올리지 않고 완료. (Full 센서가 감지되지 않는 위치에서 Unloading 하도록 한다.)
                        m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_Move_OverDistance;
                    }
                    else if (MC_Func.MC_GetDone((int)nAxis.Z1) && 
                        MC_Func.MC_PosTolerance((int)nAxis.Z1, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z1]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Stacker1 Work Pos. Set", "Stacker1 Z 축, Bottom 위치까지 이동 완료");

                        if (!unloaderParameter.DI_Unloader_Stacker_FullCheck((int)UnloaderParameter.StackerTable.Stacker_1))         //  감지 시 Off
                        {
                            m_strTemp = "Stacker1 Z 축, Bottom 위치까지 이동했으나 Full 센서 On 상태";
                            Log.Write("SLD-200", Equipment.User_Name, "Unload_StackerModulePutdownWaitingPos_Step", m_strTemp);
                            return AlarmPost(AlarmKey.UL_Staker1_Z_Full_Sensor_On_Z_Move_Fail);
                        }
                        else
                        {
                            //m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.Complete;
                            m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_Move_OverDistance;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULSZ1) > 60000)
                    {
                        m_strTemp = "Stacker1 Z 축, Full 센서가 Off 되는 위치까지 이동 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unload_StackerModulePutdownWaitingPos_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Staker1_Z_Full_Sensor_On_Z_Move_Fail);
                    }
                    break;


                case (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType1_Slow3Up:                               //  Stacker Z 축, 더더 느리게 올림 (최 상단까지)

                    if (MC_Func.MC_GetDone((int)nAxis.Z1))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Stacker1 Work Pos. Set", "Stacker1 Z 축, Full 센서가 On 되는 위치까지 이동 시작 (저속 / 2)");

                        unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Stacker1_Top");

                        //  Target Position 변경 : 맨 위로 올라가는 위치
                        unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z1] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_LPort_TopPos].UL_Stacker_Z1;

                        //  속도 (기본 속도 / 4)
                        //m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z0].Common_MoveSpeed / 4.0;
                        m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z1].Common_Speed_Fine / 4.0;

                        //  가감속 배율
                        m_dSpeedMag_forAccDec = 2.0;

                        MC_Func.MC_MovePosition((int)nAxis.Z1,
                                            unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z1],
                                            m_dSpeed_Stacker_MoreSlow,
                                            m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec,
                                            m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec);

                        //MC_Func.MC_MovePosition((int)UnloaderParameter.AxisAjinEnum.Z0,
                        //                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
                        //                    unloaderParameter.stUnloaderPosParam.dVel[(int)UnloaderParameter.MotionKey.Z0],
                        //                    unloaderParameter.stUnloaderPosParam.dAcc[(int)UnloaderParameter.MotionKey.Z0],
                        //                    unloaderParameter.stUnloaderPosParam.dDec[(int)UnloaderParameter.MotionKey.Z0]);

                        TickCount_Start((int)TickType.TICK_ULSZ1);

                        m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType1_Slow3Up_DoneCheck;
                    }
                    break;


                case (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType1_Slow3Up_DoneCheck:                          //  Stacker Z 축, 더더 느리게 올림, 이동 완료 확인

                    if (!unloaderParameter.DI_Unloader_Stacker_FullCheck((int)UnloaderParameter.StackerTable.Stacker_1))         //  감지 시 Off
                    {
                        MC_Func.MC_MotorStop((int)nAxis.Z1, 2000);

                        //m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.Complete;
                        m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_Move_OverDistance;
                    }
                    else if (MC_Func.MC_GetDone((int)nAxis.Z1) && 
                        MC_Func.MC_PosTolerance((int)nAxis.Z1, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z1]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Stacker1 Work Pos. Set", "Stacker1 Z 축, Top 위치까지 이동 완료");

                        if (unloaderParameter.DI_Unloader_Stacker_FullCheck((int)UnloaderParameter.StackerTable.Stacker_1))            //  감지 시 Off
                        {
                            m_strTemp = "Stacker1 Z 축, Top 위치까지 이동했으나 Full 센서 Off 상태";
                            Log.Write("SLD-200", Equipment.User_Name, "Unload_StackerModulePutdownWaitingPos_Step", m_strTemp);
                            return AlarmPost(AlarmKey.UL_Staker1_Z_Full_Sensor_On_Fail);
                        }
                        else
                        {
                            //m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.Complete;
                            m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_Move_OverDistance;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULSZ1) > 60000)
                    {
                        m_strTemp = "Stacker1 Z 축, Full 센서가 On 되는 위치까지 이동 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unload_StackerModulePutdownWaitingPos_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Staker1_Z_Full_Sensor_On_Z_Move_Fail);
                    }
                    break;
                /// <summary>
                /// Full Sensor 감지 상태일 경우 - 완료
                /// </summary>



                /// <summary>
                /// Full Sensor 감지되지 않는 상태일 경우 - 시작
                /// </summary>
                case (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType2_FastUp:                               //  Stacker Z 축, 빠르게 올림 (최 상단까지)

                    if (MC_Func.MC_GetDone((int)nAxis.Z1))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Stacker1 Work Pos. Set", "Stacker1 Z 축, Full 센서가 On 되는 위치까지 이동 시작 (고속)");

                        unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Stacker1_Top");

                        //  Target Position 변경 : 맨 위로 올라가는 위치
                        unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z1] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_LPort_TopPos].UL_Stacker_Z1;

                        //  속도 (기본 속도)
                        m_dSpeed_Stacker_Fast = Equipment.stAxisParam[(int)nAxis.Z1].Common_Speed_Fine;

                        //  가감속 배율
                        m_dSpeedMag_forAccDec = 2.0;

                        MC_Func.MC_MovePosition((int)nAxis.Z1,
                                            unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z1],
                                            m_dSpeed_Stacker_Fast,
                                            m_dSpeed_Stacker_Fast * m_dSpeedMag_forAccDec,
                                            m_dSpeed_Stacker_Fast * m_dSpeedMag_forAccDec);

                        //MC_Func.MC_MovePosition((int)UnloaderParameter.AxisAjinEnum.Z0,
                        //                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
                        //                    unloaderParameter.stUnloaderPosParam.dVel[(int)UnloaderParameter.MotionKey.Z0],
                        //                    unloaderParameter.stUnloaderPosParam.dAcc[(int)UnloaderParameter.MotionKey.Z0],
                        //                    unloaderParameter.stUnloaderPosParam.dDec[(int)UnloaderParameter.MotionKey.Z0]);

                        TickCount_Start((int)TickType.TICK_ULSZ1);

                        m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType2_FastUp_DoneCheck;
                    }
                    break;


                case (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType2_FastUp_DoneCheck:                          //  Stacker Z 축, 빠르게 올림, 이동 완료 확인

                    if (!unloaderParameter.DI_Unloader_Stacker_FullCheck((int)UnloaderParameter.StackerTable.Stacker_1))         //  감지 시 Off
                    {
                        MC_Func.MC_MotorStop((int)nAxis.Z1, 2000);

                        m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType2_Slow2Down;
                    }
                    else if (MC_Func.MC_GetDone((int)nAxis.Z1) && 
                        MC_Func.MC_PosTolerance((int)nAxis.Z1, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z1]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Stacker1 Work Pos. Set", "Stacker1 Z 축, Top 위치까지 이동 완료");

                        if (unloaderParameter.DI_Unloader_Stacker_FullCheck((int)UnloaderParameter.StackerTable.Stacker_1))            //  감지 시 Off
                        {
                            m_strTemp = "Stacker1 Z 축, Top 위치까지 이동했으나 Full 센서 Off 상태";
                            Log.Write("SLD-200", Equipment.User_Name, "Unload_StackerModulePutdownWaitingPos_Step", m_strTemp);
                            return AlarmPost(AlarmKey.UL_Staker1_Z_Full_Sensor_On_Fail);
                        }
                        else
                        {
                            m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType2_Slow2Down;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULSZ1) > 60000)
                    {
                        m_strTemp = "Stacker1 Z 축, Full 센서가 On 되는 위치까지 이동 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unload_StackerModulePutdownWaitingPos_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Staker0_Z_Full_Sensor_On_Z_Move_Fail);
                    }
                    break;


                case (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType2_Slow2Down:                            //  Stacker Z 축, 더 느리게 내림 (최 하단까지)

                    if (MC_Func.MC_GetDone((int)nAxis.Z1))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Stacker1 Work Pos. Set", "Stacker1 Z 축, Full 센서가 Off 되는 위치까지 이동 시작 (저속)");

                        unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Stacker1_Bottom");

                        //  Target Position 변경 : 맨 아래로 내려가는 위치
                        unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z1] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_LPort_ReadyPos].UL_Stacker_Z1;

                        //  속도 (기본 속도 / 3)
                        //m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z0].Common_MoveSpeed / 3.0;
                        m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z1].Common_Speed_Fine / 3.0;

                        //  가감속 배율
                        m_dSpeedMag_forAccDec = 2.0;

                        MC_Func.MC_MovePosition((int)nAxis.Z1,
                                            unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z1],
                                            m_dSpeed_Stacker_MoreSlow,
                                            m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec,
                                            m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec);

                        //MC_Func.MC_MovePosition((int)UnloaderParameter.AxisAjinEnum.Z0,
                        //                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
                        //                    unloaderParameter.stUnloaderPosParam.dVel[(int)UnloaderParameter.MotionKey.Z0],
                        //                    unloaderParameter.stUnloaderPosParam.dAcc[(int)UnloaderParameter.MotionKey.Z0],
                        //                    unloaderParameter.stUnloaderPosParam.dDec[(int)UnloaderParameter.MotionKey.Z0]);

                        TickCount_Start((int)TickType.TICK_ULSZ1);

                        m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType2_Slow2Down_DoneCheck;
                    }
                    break;


                case (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType2_Slow2Down_DoneCheck:                       //  Stacker Z 축, 더 느리게 내림, 이동 완료 확인

                    if (unloaderParameter.DI_Unloader_Stacker_FullCheck((int)UnloaderParameter.StackerTable.Stacker_1))            //  감지 시 Off
                    {
                        MC_Func.MC_MotorStop((int)nAxis.Z1, 1000);

                        //m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType2_Slow3Up;
                        //m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.Complete;                           //  다시 올리지 않고 완료. (Full 센서가 감지되지 않는 위치에서 Unloading 하도록 한다.)
                        m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_Move_OverDistance;
                    }
                    else if (MC_Func.MC_GetDone((int)nAxis.Z1) && MC_Func.MC_PosTolerance((int)nAxis.Z1, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z1]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Stacker1 Work Pos. Set", "Stacker1 Z 축, Bottom 위치까지 이동 완료");

                        if (!unloaderParameter.DI_Unloader_Stacker_FullCheck((int)UnloaderParameter.StackerTable.Stacker_1))         //  감지 시 Off
                        {
                            m_strTemp = "Stacker1 Z 축, Bottom 위치까지 이동했으나 Full 센서 On 상태";
                            Log.Write("SLD-200", Equipment.User_Name, "Unload_StackerModulePutdownWaitingPos_Step", m_strTemp);
                            return AlarmPost(AlarmKey.UL_Staker1_Too_Many_Module);
                        }
                        else
                        {
                            //m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.Complete;
                            m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_Move_OverDistance;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULSZ1) > 60000)
                    {
                        m_strTemp = "Stacker1 Z 축, Full 센서가 Off 되는 위치까지 이동 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unload_StackerModulePutdownWaitingPos_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Staker1_Z_Full_Sensor_On_Z_Move_ToBottom_Fail);
                    }
                    break;


                case (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType2_Slow3Up:                               //  Stacker Z 축, 더더 느리게 올림 (최 상단까지)

                    if (MC_Func.MC_GetDone((int)nAxis.Z1))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Stacker1 Work Pos. Set", "Stacker1 Z 축, Full 센서가 On 되는 위치까지 이동 시작 (저속 / 2)");

                        unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Stacker1_Top");

                        //  Target Position 변경 : 맨 위로 올라가는 위치
                        unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z1] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_LPort_TopPos].UL_Stacker_Z1;

                        //  속도 (기본 속도 / 4)
                        //m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z0].Common_MoveSpeed / 4.0;
                        m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z1].Common_Speed_Fine / 4.0;

                        //  가감속 배율
                        m_dSpeedMag_forAccDec = 2.0;

                        MC_Func.MC_MovePosition((int)nAxis.Z1,
                                            unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z1],
                                            m_dSpeed_Stacker_MoreSlow,
                                            m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec,
                                            m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec);

                        //MC_Func.MC_MovePosition((int)UnloaderParameter.AxisAjinEnum.Z0,
                        //                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
                        //                    unloaderParameter.stUnloaderPosParam.dVel[(int)UnloaderParameter.MotionKey.Z0],
                        //                    unloaderParameter.stUnloaderPosParam.dAcc[(int)UnloaderParameter.MotionKey.Z0],
                        //                    unloaderParameter.stUnloaderPosParam.dDec[(int)UnloaderParameter.MotionKey.Z0]);

                        TickCount_Start((int)TickType.TICK_ULSZ1);

                        m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType2_Slow3Up_DoneCheck;
                    }
                    break;


                case (int)StackerModulePutdownWaitingPos_Step.StackerZ_MoveType2_Slow3Up_DoneCheck:                          //  Stacker Z 축, 더더 느리게 올림, 이동 완료 확인

                    if (!unloaderParameter.DI_Unloader_Stacker_FullCheck((int)UnloaderParameter.StackerTable.Stacker_1))         //  감지 시 Off
                    {
                        MC_Func.MC_MotorStop((int)nAxis.Z1, 2000);

                        //m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.Complete;
                        m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_Move_OverDistance;
                    }
                    else if (MC_Func.MC_GetDone((int)nAxis.Z1) && 
                        MC_Func.MC_PosTolerance((int)nAxis.Z1, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z1]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Stacker1 Work Pos. Set", "Stacker1 Z 축, Top 위치까지 이동 완료");

                        if (unloaderParameter.DI_Unloader_Stacker_FullCheck((int)UnloaderParameter.StackerTable.Stacker_1))            //  감지 시 Off
                        {
                            m_strTemp = "Stacker1 Z 축, Top 위치까지 이동했으나 Full 센서 Off 상태";
                            Log.Write("SLD-200", Equipment.User_Name, "Unload_StackerModulePutdownWaitingPos_Step", m_strTemp);
                            return AlarmPost(AlarmKey.UL_Staker1_Z_Full_Sensor_On_Fail);
                        }
                        else
                        {
                            //m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.Complete;
                            m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_Move_OverDistance;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULSZ1) > 60000)
                    {
                        m_strTemp = "Stacker1 Z 축, Full 센서가 On 되는 위치까지 이동 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unload_StackerModulePutdownWaitingPos_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Staker1_Z_Full_Sensor_On_Z_Move_Fail);
                    }
                    break;
                /// <summary>
                /// Full Sensor 감지되지 않는 상태일 경우 - 완료
                /// </summary>
                /// 


                /// <summary>
                /// 최종 위치 이동 후 추가 이동 - 시작
                /// </summary>
                case (int)StackerModulePutdownWaitingPos_Step.StackerZ_Move_OverDistance:                               //  Stacker Z 축, 최종 감지 위치에서 추가로 이동

                    if (MC_Func.MC_GetDone((int)nAxis.Z1))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Stacker1 Work Pos. Set", "Stacker1 Z 축, 최종 위치에서 추가 이동 시작. (아래로 3mm)");

                        unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Stacker1_Top");

                        //  Target Position 변경 : 현재 위치에서 추가 이동
                        unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z1] = MC_Func.MC_GetEncPos((int)nAxis.Z1) - 3.0;

                        if (unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z1] < loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_LPort_ReadyPos].UL_Stacker_Z1)
                        {
                            unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z1] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_LPort_ReadyPos].UL_Stacker_Z1;
                        }

                        //  속도 (기본 속도 / 4)
                        //m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z0].Common_MoveSpeed / 4.0;
                        m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z1].Common_Speed_Fine / 4.0;

                        //  가감속 배율
                        m_dSpeedMag_forAccDec = 2.0;

                        MC_Func.MC_MovePosition((int)nAxis.Z1,
                                            unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z1],
                                            m_dSpeed_Stacker_MoreSlow,
                                            m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec,
                                            m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec);

                        TickCount_Start((int)TickType.TICK_ULSZ1);

                        m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.StackerZ_Move_OverDistance_DoneCheck;
                    }
                    break;


                case (int)StackerModulePutdownWaitingPos_Step.StackerZ_Move_OverDistance_DoneCheck:                          //  Stacker Z 축, 최종 감지 위치에서 추가로 이동 완료 확인

                    //if (MC_Func.MC_GetDone((int)nAxis.Z0) && MC_Func.MC_PosTolerance((int)nAxis.Z0, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0]))
                    if (MC_Func.MC_GetDone((int)nAxis.Z1))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Stacker1 Work Pos. Set", "Stacker1 Z 축, Top 위치 아래 3mm 까지 이동 완료");

                        m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.Complete;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULSZ1) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Stacker1 Work Pos. Set", "Stacker1 Z 축, Top 위치 아래 3mm 까지 이동 실패. (Timeout)");

                        return AlarmPost(AlarmKey.UL_Staker1_MoveZ_Timeout);

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;
                        //return AlarmPost(AlarmKey.UL_Stacker0_FullSensor_Off_MoveFail);
                        m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.None;

                        MessageBox.Show("LD Stacker0 Z 축, Top 위치 Over 까지 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                /// <summary>
                /// 최종 위치 이동 후 추가 이동 - 완료
                /// </summary>


                case (int)StackerModulePutdownWaitingPos_Step.Complete:

                    m_strTemp = "===  UL Stacker1 작업위치 이동 완료  ===";
                    Log.Write("SLD-200", Equipment.User_Name, "Unload_StackerModulePutdownWaitingPos_Step", m_strTemp);

                    m_bStacker1_Complete = true;

                    m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.None;
                    break;
            }
            return 0;
        }

        private void StackerModulePutdownWaitingPos_Step_tackerZ_MoveType1_Slow2Down(out double m_dSpeed_Stacker_MoreSlow, out double m_dSpeedMag_forAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "UL Stacker1 Work Pos. Set", "Stacker1 Z 축, Full 센서가 Off 되는 위치까지 이동 시작 (저속)");

            unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Stacker1_Bottom");

            //  Target Position 변경 : 맨 아래로 내려가는 위치
            unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z1] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_LPort_ReadyPos].UL_Stacker_Z1;

            //  속도 (기본 속도 / 3)
            //m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z0].Common_MoveSpeed / 3.0;
            m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z1].Common_Speed_Fine / 3.0;

            //  가감속 배율
            m_dSpeedMag_forAccDec = 2.0;

            MC_Func.MC_MovePosition((int)nAxis.Z1,
                                unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z1],
                                m_dSpeed_Stacker_MoreSlow,
                                m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec,
                                m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec);

            //MC_Func.MC_MovePosition((int)UnloaderParameter.AxisAjinEnum.Z0,
            //                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dVel[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dAcc[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dDec[(int)UnloaderParameter.MotionKey.Z0]);

            TickCount_Start((int)TickType.TICK_ULSZ1);
        }

        private void StackerModulePutdownWaitingPos_Step_tackerZ_MoveType1_FastDown(out double m_dSpeed_Stacker_Fast, out double m_dSpeedMag_forAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "UL Stacker1 Work Pos. Set", "Stacker1 Z 축, Full 센서가 Off 되는 위치까지 이동 시작 (고속)");

            unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Stacker1_Bottom");

            //  Target Position 변경 : 맨 아래로 내려가는 위치
            unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z1] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_LPort_ReadyPos].UL_Stacker_Z1;

            //  속도 (기본 속도)
            m_dSpeed_Stacker_Fast = Equipment.stAxisParam[(int)nAxis.Z1].Common_Speed_Fine;

            //  가감속 배율
            m_dSpeedMag_forAccDec = 2.0;

            MC_Func.MC_MovePosition((int)nAxis.Z1,
                                unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z1],
                                m_dSpeed_Stacker_Fast,
                                m_dSpeed_Stacker_Fast * m_dSpeedMag_forAccDec,
                                m_dSpeed_Stacker_Fast * m_dSpeedMag_forAccDec);
            TickCount_Start((int)TickType.TICK_ULSZ1);

        }

        private void StackerModulePutdownWaitingPosStepStackerZMoveType1SlowUp(out double m_dSpeed_Stacker_Slow, out double m_dSpeedMag_forAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "UL Stacker1 Work Pos. Set", "Stacker1 Z 축, Full 센서가 On 되는 위치까지 이동 시작 (중속)");

            unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Stacker1_Top");

            //  Target Position 변경 : 맨 위로 올라가는 위치
            unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z1] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_LPort_TopPos].UL_Stacker_Z1;

            //  속도 (기본 속도 / 2)
            m_dSpeed_Stacker_Slow = Equipment.stAxisParam[(int)nAxis.Z1].Common_Speed_Fine / 2.0;

            //  가감속 배율
            m_dSpeedMag_forAccDec = 2.0;

            MC_Func.MC_MovePosition((int)nAxis.Z1,
                                unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z1],
                                m_dSpeed_Stacker_Slow,
                                m_dSpeed_Stacker_Slow * m_dSpeedMag_forAccDec,
                                m_dSpeed_Stacker_Slow * m_dSpeedMag_forAccDec);

            //MC_Func.MC_MovePosition((int)UnloaderParameter.AxisAjinEnum.Z0,
            //                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dVel[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dAcc[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dDec[(int)UnloaderParameter.MotionKey.Z0]);
        }
        #endregion

        #region Transfer Cycle Function (Module PickUp & PutDown 위치로 이동)

        
        int Run_Transfer_Cycle_Func()
        {
            int ret = 0;
            bool m_bRet = false;
            string m_strTemp;

            double m_dSpeed = 0.0;
            double m_dAccDec = 0.0;


            //  운전 중 Door 를 열면 장비 Stop
            if (m_nUnloader_Transfer_Step >= (int)Unloader_Transfer_Step.Start)
            {
               
            }

            //  자동운전 시, Transfer 동작 조건
            //1. Work Stage 에서 Module Pick Up Cycle
            //2. Stacker0 에 Module Put Down Cycle
            //3. Stacker1 에 Module Put Down Cycle
            //4. NG-Port 에 Module Drop Cycle

            if (Equipment.AutoRunStatus &&

                !Equipment.SocketStopped &&                                             //  Socket Stop 시 동작 안되도록

                !Equipment.CycleStopped_UnloaderTransfer &&                             //  Cycle Stop 시 동작 안되도록

                !Equipment.MachineStop_byTimeout_Unloader &&                            //  Unloader 가 Time out 으로 멈추면 동작 안되도록

                m_nUnloader_Transfer_Step == (int)Unloader_Transfer_Step.None)
            {
                //  Work Stage 에서 Module 을 Pick Up 하기 위한 조건
                if (!m_bAUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete &&
                    (workStage.m_nLaserDrilling_MainStep == (int)WorkStage.LaserDrilling_Step.None) &&
                    (workStage.m_nWorkStage_Move_Step == (int)WorkStage.WorkStage_Move_Step.None) &&
                    loader.m_bAUTORUN_Loader_Transfer_ModulePutDowntoWorkStage_Complete &&
                    workStage.m_bMainWorkCycle_Complete)                                                //  Laser Drilling Main Cycle 이 완료되었을 경우
                {
                    m_nUnloaderTransferMoveType = (int)UnloaderTransferMoveType.Cycle_WorkStage_PickUp;            //  Work Stage 에서 Module Pick Up Cycle
                    m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Start;
                }
                //  Stacker0 에 Module 을 Put Down 하기 위한 조건
                else if (m_bAUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete &&
                    (m_nStacker0_ModulePutdownWaitingPos_Step == (int)StackerModulePutdownWaitingPos_Step.None) &&
                    (workStage.m_nMainWorkCycle_ResultOKNG == (int)WorkStage.MainCycle_Result.OK) &&
                    workStage.m_bMainWorkCycle_ResultOK_toRPort &&
                    m_bStacker0_Complete)
                {
                    //  우측 Port (Stacker0) 에 내려놔야 하는데 Full 상태이면? 좌측 Port 에 내려놓도록
                    if (!unloaderParameter.DI_Unloader_Stacker_FullCheck((int)UnloaderParameter.StackerTable.Stacker_0))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Stacker0 이 Full 상태이므로 Stacker1 에 Put Down 시도합니다.");

                        if (!unloaderParameter.DI_Unloader_Stacker_FullCheck((int)UnloaderParameter.StackerTable.Stacker_1))
                        {
                            Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Stacker0, Stacker1 모두 Full 상태이므로 알람.");

                            //  알람 정지 (LED Bar - Red Blink)
                            Equipment.MachineStop_byAlarm = true;

                            return AlarmPost(AlarmKey.UL_Staker0_Too_Many_Module);
                        }
                        else
                        {
                            workStage.m_bMainWorkCycle_ResultOK_toRPort = false;
                        }
                    }
                    else
                    {
                        m_nUnloaderTransferMoveType = (int)UnloaderTransferMoveType.Cycle_Stacker0_PutDown;            //  Stacker0 에 Module Put Down Cycle
                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Start;
                    }
                }
                //  Stacker1 에 Module 을 Put Down 하기 위한 조건
                else if (m_bAUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete &&
                    (m_nStacker1_ModulePutdownWaitingPos_Step == (int)StackerModulePutdownWaitingPos_Step.None) &&
                    (workStage.m_nMainWorkCycle_ResultOKNG == (int)WorkStage.MainCycle_Result.OK) &&
                    !workStage.m_bMainWorkCycle_ResultOK_toRPort &&
                    m_bStacker1_Complete)
                {
                    //  좌측 Port (Stacker1) 에 내려놔야 하는데 Full 상태이면? 우측 Port 에 내려놓도록
                    if (!unloaderParameter.DI_Unloader_Stacker_FullCheck((int)UnloaderParameter.StackerTable.Stacker_1))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Stacker1 이 Full 상태이므로 Stacker0 에 Put Down 시도합니다.");

                        if (!unloaderParameter.DI_Unloader_Stacker_FullCheck((int)UnloaderParameter.StackerTable.Stacker_1))
                        {
                            Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Stacker0, Stacker1 모두 Full 상태이므로 알람.");

                            //  알람 정지 (LED Bar - Red Blink)
                            Equipment.MachineStop_byAlarm = true;

                            return AlarmPost(AlarmKey.UL_Staker1_Too_Many_Module);
                        }
                        else
                        {
                            workStage.m_bMainWorkCycle_ResultOK_toRPort = true;
                        }
                    }
                    else
                    {
                        m_nUnloaderTransferMoveType = (int)UnloaderTransferMoveType.Cycle_Stacker1_PutDown;            //  Stacker1 에 Module Put Down Cycle
                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Start;
                    }
                }
                //  NG-Port 에 Module 을 Drop 하기 위한 조건
                else if (m_bAUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete &&
                        (workStage.m_nMainWorkCycle_ResultOKNG == (int)WorkStage.MainCycle_Result.NG))  ////!workStage.m_bMainWorkCycle_ResultOK)
                {
                    m_nUnloaderTransferMoveType = (int)UnloaderTransferMoveType.Cycle_NG_PutDown;            //  NG-Port 에 Module Drop Cycle
                    m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Start;
                }
            }

            switch (m_nUnloader_Transfer_Step)
            {
                case (int)Unloader_Transfer_Step.Start:
                    Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "시작");

                    Equipment.MachineStop_byAlarm = false;

                    //  Unlaoder Transfer XZ 축 모터 전체 Stop
                    MC_Func.MC_MotorStop((int)UnloaderParameter.AxisAjinEnum.TR_X, 2000);
                    MC_Func.MC_MotorStop((int)UnloaderParameter.AxisAjinEnum.TR_Z, 2000);

                    m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Process_Type_Check;
                    break;


                case (int)Unloader_Transfer_Step.Process_Type_Check:
                    Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "동작 타입 확인");

                    switch (m_nUnloaderTransferMoveType)
                    {
                        case (int)UnloaderTransferMoveType.Cycle_Transfer_ReadyPos:
                            Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Ready Position Move");
                            m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Transfer_Move_Condition_Check;
                            break;

                        case (int)UnloaderTransferMoveType.Cycle_WorkStage_PickUp:
                            Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Work Stage 에서 Module Pick Up");
                            m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.WorkStage_ModulePickup_Condition_Check;
                            break;

                        case (int)UnloaderTransferMoveType.Cycle_Stacker0_PutDown:
                            Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Stacker0 에 Module Put Down");
                            m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Stacker0_ModulePutdown_Condition_Check;
                            break;

                        case (int)UnloaderTransferMoveType.Cycle_Stacker1_PutDown:
                            Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Stacker1 에 Module Put Down");
                            m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Stacker1_ModulePutdown_Condition_Check;
                            break;

                        case (int)UnloaderTransferMoveType.Cycle_NG_PutDown:
                            Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "NG-Port 에 Module Drop");
                            m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.NgStacker_ModuleDrop_Condition_Check;
                            break;

                        default:  //  Error
                            m_strTemp = "동작 타입 목록에 없음";
                            Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                            return AlarmPost(AlarmKey.UL_Transfer_Error);
                            //Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "동작 타입 목록에 없음");
                            //m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Complete;
                            break;
                    }
                    break;

                /// <summary>
                /// Transfer 대기 위치로 이동 - 시작
                /// </summary>
                case (int)Unloader_Transfer_Step.Transfer_Move_Condition_Check:                            //  Stacker0 에서 Module Pick Up 조건 체크 (Stacker0 Module Exist Sensor On, Stacker0 Module Full Sensor On, Transfer Picker Vacuum Off Check, Stacker0 Cycle : None)

                    Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "TR 축, Ready Position Move 조건 체크");
                    m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.TransferZ_Move_ReadyPos;
             
                    break;


                case (int)Unloader_Transfer_Step.TransferZ_Move_ReadyPos:                            //  Transfer Z 축, 대기 위치로 이동

                    Unloader_Transfer_Step_TransferZ_Move_ReadyPos(out m_dSpeed, out m_dAccDec);

                    m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.TransferZ_Move_ReadyPos_DoneCheck;
                    break;


                case (int)Unloader_Transfer_Step.TransferZ_Move_ReadyPos_DoneCheck:                       //  Transfer Z 축, 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 완료");

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.TransferX_Move_ReadyPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULTR) > 60000)
                    {
                        m_strTemp = "Transfer Z 축, 대기 위치로 이동 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Transfer_Z_Move_To_Ready_Pos_Fail);
                    }
                    break;

                case (int)Unloader_Transfer_Step.TransferX_Move_ReadyPos:                            //  Transfer X 축, 대기 위치로 이동

                    Unloader_Transfer_Step_TransferX_Move_ReadyPos(out m_dSpeed, out m_dAccDec);

                    m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.TransferX_Move_ReadyPos_DoneCheck;
                    break;


                case (int)Unloader_Transfer_Step.TransferX_Move_ReadyPos_DoneCheck:                       //  Transfer X 축, Stacker 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_X) && 
                        MC_Func.MC_PosTolerance((int)nAxis.TR_X, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_X]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer X 축, 대기 위치로 이동 완료");

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Complete;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULTR) > 60000)
                    {
                        m_strTemp = "Transfer X 축, 대기 위치로 이동 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Transfer_X_Move_To_Ready_Pos);
                    }
                    break;
                /// <summary>
                /// Transfer 대기 위치로 이동 - 완료
                /// </summary>

                /// <summary>
                /// Work Stage 에서 Module Pick Up - 시작
                /// </summary>
                case (int)Unloader_Transfer_Step.WorkStage_ModulePickup_Condition_Check:                            //  Work Stage 에서 Module Pick Up 조건 체크 (Work Stage Vacuum On Check, Transfer Picker Vacuum Off Check, Stacker Cycle : None, Drilling Cycle : None)

                    Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "TR 축, Work Stage 에서 Module Pickup 조건 체크");

                    if (workStage.m_nLaserDrilling_MainStep > (int)WorkStage.LaserDrilling_Step.None)
                    {
                        Unloader_CurrentStatus_Save_StopedByTimeout();

                        m_strTemp = "Laser Drilling 중.";
                        Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                        return AlarmPost(AlarmKey.Drilling_NotCompleted);
                    }
                    else if (unloaderParameter.DI_Unloader_Picker_VacuumCheck((int)UnloaderParameter.PickerVacuumPos.Inner) ||
                            unloaderParameter.DI_Unloader_Picker_VacuumCheck((int)UnloaderParameter.PickerVacuumPos.Outer))
                    {
                        Unloader_CurrentStatus_Save_StopedByTimeout();

                        m_strTemp = "TR Picker 에 자재 있음.";
                        Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Transfer_Picker_Module_Exist);
                    }
                    else if (Equipment.Machine_VacuumSensor_Enable && !workStage.workStageParameter.DI_Stage_Vacuum_Check() && 
                        !m_bUnloader_WorkStage_PickUp_Retry)                                                                        //  Work Stage 에서 Module Pick Up 실패 시 재시도 할 경우, Stage Vacuum 이 파기된 상태이므로 체크하지 않는다.
                    {
                        Unloader_CurrentStatus_Save_StopedByTimeout();

                        m_strTemp = "Work Stage 에 자재 없음.";
                        Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                        return AlarmPost(AlarmKey.WorkStage_Module_NotExist);
                    }
                    else
                    {
                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.WorkStagePickUp_TransferZ_Move_ReadyPos;
                    }
                    break;


                case (int)Unloader_Transfer_Step.WorkStagePickUp_TransferZ_Move_ReadyPos:                            //  Transfer Z 축, 대기 위치로 이동

                    Unloader_Transfer_Step_WorkStagePickUp_TransferZ_Move_ReadyPos(out m_dSpeed, out m_dAccDec);

                    m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.WorkStagePickUp_TransferZ_Move_ReadyPos_DoneCheck;
                    break;


                case (int)Unloader_Transfer_Step.WorkStagePickUp_TransferZ_Move_ReadyPos_DoneCheck:                       //  Transfer Z 축, 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && 
                        MC_Func.MC_PosTolerance((int)nAxis.TR_Z, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 완료");

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.WorkStagePickUp_WorkStageCycle_UnloadingPos_Start;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULTR) > 60000)
                    {
                        m_strTemp = "대기 위치로 이동 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Transfer_Z_Move_To_WorkStage_PickUp_Pos);

                    }
                    break;
                case (int)Unloader_Transfer_Step.WorkStagePickUp_WorkStageCycle_UnloadingPos_Start:                            //  Work Stage, Unloading 위치로 이동 Cycle 시작

                    //if (!workStage.m_bWorkStageMove_Complete)
                    if ((workStage.m_nWorkStagePosition == (int)WorkStage.WorkStagePosition.WorkStage_UnloadingZone) &&
                            (workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X) > (workStage.stWorkStageTeachingPos[(int)WorkStage.WorkStage_TeachingPosList.STAGE_UnloadingPos].Stage_X - 0.1)) &&
                            (workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X) < (workStage.stWorkStageTeachingPos[(int)WorkStage.WorkStage_TeachingPosList.STAGE_UnloadingPos].Stage_X + 0.1)) &&
                            (workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y) > (workStage.stWorkStageTeachingPos[(int)WorkStage.WorkStage_TeachingPosList.STAGE_UnloadingPos].Stage_Y - 0.1)) &&
                            (workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y) < (workStage.stWorkStageTeachingPos[(int)WorkStage.WorkStage_TeachingPosList.STAGE_UnloadingPos].Stage_Y + 0.1)))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Work Stage, Module Unloading 위치로 이동이 완료된 상태이므로 Pick Up 진행");

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.WorkStagePickUp_TransferX_Move_WorkStagePos;
                    }
                    else if ((workStage.m_nWorkStage_Move_Step == (int)WorkStage.WorkStage_Move_Step.None) &&
                            (workStage.m_nLaserDrilling_MainStep == (int)WorkStage.LaserDrilling_Step.None) &&
                            (workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X) > (workStage.stWorkStageTeachingPos[(int)WorkStage.WorkStage_TeachingPosList.STAGE_UnloadingPos].Stage_X - 0.1)) &&
                            (workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X) < (workStage.stWorkStageTeachingPos[(int)WorkStage.WorkStage_TeachingPosList.STAGE_UnloadingPos].Stage_X + 0.1)) &&
                            (workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y) > (workStage.stWorkStageTeachingPos[(int)WorkStage.WorkStage_TeachingPosList.STAGE_UnloadingPos].Stage_Y - 0.1)) &&
                            (workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y) < (workStage.stWorkStageTeachingPos[(int)WorkStage.WorkStage_TeachingPosList.STAGE_UnloadingPos].Stage_Y + 0.1)))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Work Stage, Module Unloading 위치에 있으므로 Pick Up 진행, (위치 및 로딩 조건 OK)");

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.WorkStagePickUp_TransferX_Move_WorkStagePos;
                    }
                    else
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Work Stage, Module Unloading 위치로 이동 시작");

                        TickCount_Start((int)TickType.TICK_ULTR);

                        //  Work Stage 이동 시작
                        workStage.m_nWorkStageMoveType = (int)WorkStage.WorkStageMoveType.MoveTo_UnloadingPos;
                        workStage.m_nWorkStage_Move_Step = (int)WorkStage.WorkStage_Move_Step.Start;

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.WorkStagePickUp_WorkStageCycle_UnloadingPos_CompleteCheck;
                    }
                    break;


                case (int)Unloader_Transfer_Step.WorkStagePickUp_WorkStageCycle_UnloadingPos_CompleteCheck:                       //  Work Stage, Unloading 위치로 이동 Cycle 완료 체크

                    if (MC_Func.MC_GetDone((int)WorkStage.nAxis.X) && MC_Func.MC_GetDone((int)WorkStage.nAxis.Y) &&
                        (workStage.m_nWorkStagePosition == (int)WorkStage.WorkStagePosition.WorkStage_UnloadingZone) && 
                        (workStage.m_nWorkStage_Move_Step == (int)WorkStage.WorkStage_Move_Step.None))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Work Stage, Module Unloading 위치로 이동 완료");

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.WorkStagePickUp_TransferX_Move_WorkStagePos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULTR) > 60000)
                    {
                        m_strTemp = "Work Stage, Module Unloading 위치로 이동 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_WorkStage_Move_To_Unloading_Pos);

                    }
                    break;


                case (int)Unloader_Transfer_Step.WorkStagePickUp_TransferX_Move_WorkStagePos:                            //  Transfer X 축, Work Stage 위치로 이동

                    Unloader_Transfer_Step_WorkStagePickUp_TransferX_Move_WorkStagePos(out m_dSpeed, out m_dAccDec);

                    //m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.WorkStagePickUp_TransferX_Move_WorkStagePos_DoneCheck;
                    m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.WorkStagePickUp_DustCol_Off;
                    break;

                    //WorkStagePickUp_DustCol_Off,
                    //WorkStagePickUp_DustCol_Off_check,
                case (int)Unloader_Transfer_Step.WorkStagePickUp_DustCol_Off:

                    workStage.DustCollector_Off((int)nDustCollector.DustCollector_Lower);
                    Thread.Sleep(200);

                    TickCount_Start((int)TickType.TICK_ULTR);
                    m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.WorkStagePickUp_DustCol_Off_check;

                    break;

                case (int)Unloader_Transfer_Step.WorkStagePickUp_DustCol_Off_check:

                    if (!workStage.workStageParameter.DI_DustCollector_Fan_Run((int)nDustCollector.DustCollector_Lower))
                    {
                        Log.Write("SLD-200", "Auto Run", "집진기 Remote Mode, Off 완료");

                        TickCount_Start((int)TickType.TICK_ULTR);
                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.WorkStagePickUp_TransferX_Move_WorkStagePos_DoneCheck;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULTR) > 60000 * 2)
                    {
                        Log.Write("SLD-200", "Auto Run", "집진기 Off 실패 (Timeout)");

                        return AlarmPost(AlarmKey.WorkStage_DustCollector_Off_Fail);
                    }

                    break;


                case (int)Unloader_Transfer_Step.WorkStagePickUp_TransferX_Move_WorkStagePos_DoneCheck:                       //  Transfer X 축, Work Stage 위치로 이동 완료 확인 (Work Stage Unloading 위치로 이동 Cycle 완료 확인 후, Transfer X 이동 완료 확인)

                    if (MC_Func.MC_GetDone((int)nAxis.TR_X) && 
                        MC_Func.MC_PosTolerance((int)nAxis.TR_X, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_X]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer X 축, Work Stage 위치로 이동 완료");


                        //  집진기가 Off 되었는지 확인한 후 다음 Step 을 진행한다.
                        if (Equipment.stLayerRecipeSet[0].DustCollectorRemoteMode_Use)
                        {
                            if (!workStage.workStageParameter.DI_DustCollector_Fan_Run((int)nDustCollector.DustCollector_Lower))
                            {
                                Log.Write("SLD-200", "Auto Run", "집진기 Remote Mode, Off 완료");

                                m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.WorkStagePickUp_TransferZ_Move_PickUpPos_1stStep;
                            }
                            else if (workStage.workStageParameter.DI_DustCollector_Fan_Run((int)nDustCollector.DustCollector_Lower))
                            {
                                //  집진기가 Off 되기를 기다리고 있는데 Off 되지 않는 경우, 집진기 Off Command 를 다시 보낸다.
                                
                                workStage.DustCollector_Off((int)nDustCollector.DustCollector_Lower);
                                Thread.Sleep(200);

                            }
                            else if (TickCount_Elapsed((int)TickType.TICK_ULTR) > 60000)
                            {
                                Log.Write("SLD-200", "Auto Run", "집진기 Off 실패 (Timeout)");

                                //  알람 정지 (LED Bar - Red Blink)
                                Equipment.MachineStop_byAlarm = true;

                                return AlarmPost(AlarmKey.WorkStage_DustCollector_Off_Fail);
                            }
                        }
                        else
                        {
                            m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.WorkStagePickUp_TransferZ_Move_PickUpPos_1stStep;
                        }
                    }
                    else if (Equipment.stLayerRecipeSet[0].DustCollectorRemoteMode_Use &&
                            workStage.workStageParameter.DI_DustCollector_Fan_Run((int)nDustCollector.DustCollector_Lower))
                    {
                        //  집진기가 Off 되기를 기다리고 있는데 Off 되지 않는 경우, 집진기 Off Command 를 다시 보낸다.
                        Thread.Sleep(200);
                        //workStage.DustCollector_Off((int)nDustCollector.DustCollector_Upper);           //  사실 상부 집진은 끌 필요가 없긴 한데... 걍 끄지 뭐
                        workStage.DustCollector_Off((int)nDustCollector.DustCollector_Lower);
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULTR) > 60000 * 2)
                    {
                        m_strTemp = "Transfer X 축, Work Stage 위치로 이동 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Transfer_X_Move_To_WorkStage_Pos);
                    }
                    break;


                case (int)Unloader_Transfer_Step.WorkStagePickUp_TransferZ_Move_PickUpPos_1stStep:                            //  Transfer Z 축, Module Pick Up 위치로 이동 (1단계, 최종 위치에서 위로 10 mm)

                    Unloader_Transfer_Step_WorkStagePickUp_TransferZ_Move_PickUpPos_1stStep(out m_dSpeed, out m_dAccDec);

                    m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.WorkStagePickUp_TransferZ_Move_PickUpPos_1stStep_DoneCheck;
                    break;


                case (int)Unloader_Transfer_Step.WorkStagePickUp_TransferZ_Move_PickUpPos_1stStep_DoneCheck:                       //  Transfer Z 축, Module Pick Up 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && 
                        MC_Func.MC_PosTolerance((int)nAxis.TR_Z, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, Module Pickup 대기 위치로 이동 완료");

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.WorkStagePickUp_TransferZ_Move_PickUpPos_2ndStep;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULTR) > 60000)
                    {
                        m_strTemp = "Transfer Z 축, Module Pickup 대기 위치로 이동 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Transfer_Z_Move_To_WorkStage_PickUp_Pos);
                    }
                    break;


                case (int)Unloader_Transfer_Step.WorkStagePickUp_TransferZ_Move_PickUpPos_2ndStep:                            //  Transfer Z 축, Module Pick Up 위치로 이동 (2단계, 최종 위치)

                    Unloader_Transfer_Step_WorkStagePickUp_TransferZ_Move_PickUpPos_2ndStep(out m_dSpeed, out m_dAccDec);

                    m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.WorkStagePickUp_TransferZ_Move_PickUpPos_2ndStep_DoneCheck;
                    break;


                case (int)Unloader_Transfer_Step.WorkStagePickUp_TransferZ_Move_PickUpPos_2ndStep_DoneCheck:                       //  Transfer Z 축, Module Pick Up 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && 
                        MC_Func.MC_PosTolerance((int)nAxis.TR_Z, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, Module Pickup 위치로 이동 완료");

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.WorkStagePickUp_Transfer_PickerVacuum_On;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULTR) > 60000)
                    {
                        m_strTemp = "Transfer Z 축, Module Pickup 위치로 이동 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Transfer_Z_Move_To_WorkStage_PickUp_Pos);
                    }
                    break;


                case (int)Unloader_Transfer_Step.WorkStagePickUp_Transfer_PickerVacuum_On:                                     //  Transfer, Module Picker Vacuum On

                    Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer 축, Module Picker Vacuum On");

                    unloaderParameter.DO_Unloader_Picker_Blow(false);
                    unloaderParameter.DO_Unloader_Picker_Vacuum((int)LoaderParameter.PickerVacuumPos.Inner, true);
                    unloaderParameter.DO_Unloader_Picker_Vacuum((int)LoaderParameter.PickerVacuumPos.Outer, true);

                    m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.WorkStagePickUp_WorkStage_Vacuum_Off;
                    break;


                case (int)Unloader_Transfer_Step.WorkStagePickUp_WorkStage_Vacuum_Off:                                     //  Work Stage, Vacuum Off (and Blow On)

                    Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Work Stage, Module Vacuum Off");

                    workStage.workStageParameter.DO_Stage_Vacuum(false);
                    Thread.Sleep(10); //  진공이 동작하는 경우가 있어서, Off 코드 추가
                    workStage.workStageParameter.DO_Stage_Blow(true);                   //  Blow On

                    // Todo : 수정 필요
                    //  Stage Vacuum Off 시, 진공레귤레이터도 함께 동작시켜야 한다.
                    Thread.Sleep(10);
                    workStage.ElectroPneumaticRegulatorComm_Pressure_Set(-1.3);

                    TickCount_Start((int)TickType.TICK_ULTR);

                    m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.WorkStagePickUp_Transfer_PickerVacuum_OnCheck;
                    break;


                case (int)Unloader_Transfer_Step.WorkStagePickUp_Transfer_PickerVacuum_OnCheck:                                //  Transfer, Module Picker Vacuum On 확인 (and Work Stage Vacuum Off 확인)

                    //  Stage Vacuum 을 Off 했는데, 진공이 동작하는 경우가 있어서, Off 코드 추가
                    workStage.workStageParameter.DO_Stage_Vacuum(false);
                    Thread.Sleep(10); //  진공이 동작하는 경우가 있어서, Off 코드 추가
                    workStage.workStageParameter.DO_Stage_Blow(true);                   //  Blow On

                    if (((!Equipment.Machine_VacuumSensor_Enable && (TickCount_Elapsed((int)TickType.TICK_ULTR) > Equipment.Machine_SignalHoldTime)) ||

                        (Equipment.Machine_VacuumSensor_Enable && (unloaderParameter.DI_Unloader_Picker_VacuumCheck((int)UnloaderParameter.PickerVacuumPos.Inner) ||
                                                                    unloaderParameter.DI_Unloader_Picker_VacuumCheck((int)UnloaderParameter.PickerVacuumPos.Outer)) &&

                        (!Equipment.Machine_VacuumStableTime_Enable ||
                        (Equipment.Machine_VacuumStableTime_Enable && (TickCount_Elapsed((int)TickType.TICK_ULTR) > Equipment.Machine_VacuumStableTime))))) )//&&

                        //!workStage.workStageParameter.DI_Stage_Vacuum_Check())
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Work Stage, Module Vacuum Off 완료");

                        //  Stage Vacuum 을 Off 했는데, 진공이 동작하는 경우가 있어서, Off 코드 추가
                        workStage.workStageParameter.DO_Stage_Vacuum(false);
                        Thread.Sleep(10); //  진공이 동작하는 경우가 있어서, Off 코드 추가
                        workStage.workStageParameter.DO_Stage_Blow(true);                   //  Blow On

                        Thread.Sleep(10);
                        //  Stage Vacuum Off 시, 진공레귤레이터도 함께 동작시켜야 한다. (안꺼질 때가 있어서 한번 더)
                        Thread.Sleep(10); //  진공이 동작하는 경우가 있어서, Off 코드 추가
                        workStage.ElectroPneumaticRegulatorComm_Pressure_Set(-1.3);

                        //////////////////////////////////////////////////////////////////////////////////////////
                        //  복원 지점 체크용 (Work Stage 에서 Module Pick Up 완료)
                        //
                        m_bUL_Transfer_fromWorkStage_Module_PickUp_Complete_Flag = true;
                        //
                        //  복원 지점 체크용 (Work Stage 에서 Module Pick Up 완료)
                        //////////////////////////////////////////////////////////////////////////////////////////

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.WorkStagePickUp_TransferZ_Move_ReadyPos2_1stStep;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULTR) > 10000)
                    {
                        m_strTemp = "Work Stage, Module Vacuum Off 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_WorkStage_Vacuum_Off);
                    }
                    break;


                case (int)Unloader_Transfer_Step.WorkStagePickUp_TransferZ_Move_ReadyPos2_1stStep:                            //  Transfer Z 축, 대기 위치로 이동 (1단계, 현재 위치에서 위로 10 mm)
                    Unloader_Transfer_Step_WorkStagePickUp_TransferZ_Move_ReadyPos2_1stStep(out m_dSpeed, out m_dAccDec);

                    //  Stage Vacuum 을 Off 했는데, 진공이 동작하는 경우가 있어서, Off 코드 추가
                    workStage.workStageParameter.DO_Stage_Vacuum(false);
                    Thread.Sleep(10); //  진공이 동작하는 경우가 있어서, Off 코드 추가
                    workStage.workStageParameter.DO_Stage_Blow(true);                   //  Blow On

                    m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.WorkStagePickUp_TransferZ_Move_ReadyPos2_1stStep_DoneCheck;
                    break;


                case (int)Unloader_Transfer_Step.WorkStagePickUp_TransferZ_Move_ReadyPos2_1stStep_DoneCheck:                       //  Transfer Z 축, 대기 위치로 이동 완료 확인 (여기서 Transfer Picker 의 Vacuum On 과 Work Stage 의 Vacuum Off 를 동시에 확인)

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && 
                        MC_Func.MC_PosTolerance((int)nAxis.TR_Z, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, 대기 위치로 1단계 이동 완료");

                        //  Stage Vacuum 을 Off 했는데, 진공이 동작하는 경우가 있어서, Off 코드 추가
                        workStage.workStageParameter.DO_Stage_Vacuum(false);
                        Thread.Sleep(10); //  진공이 동작하는 경우가 있어서, Off 코드 추가
                        workStage.workStageParameter.DO_Stage_Blow(false);                   //  Blow Off

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.WorkStagePickUp_TransferZ_Move_ReadyPos2_2ndStep;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULTR) > 60000)
                    {
                        m_strTemp = "Transfer Z 축, 대기 위치로 1단계 이동 실패. (Timeout))";
                        Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Transfer_Z_Move_To_Ready_Pos_Fail);
                    }
                    break;


                case (int)Unloader_Transfer_Step.WorkStagePickUp_TransferZ_Move_ReadyPos2_2ndStep:                            //  Transfer Z 축, 대기 위치로 이동 (2단계, 최종 위치)

                    Unloader_Transfer_Step_WorkStagePickUp_TransferZ_Move_ReadyPos2_2ndStep(out m_dSpeed, out m_dAccDec);

                    m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.WorkStagePickUp_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck;
                    break;


                case (int)Unloader_Transfer_Step.WorkStagePickUp_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck:                       //  Transfer Z 축, 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && 
                        MC_Func.MC_PosTolerance((int)nAxis.TR_Z, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, 대기 위치로 2단계 이동 완료");

                        workStage.workStageParameter.DO_Stage_Blow(false);                   //  Blow Off

                        if (Equipment.Machine_VacuumSensor_Enable)
                        {
                            //  여기서 Module 을 정상적으로 들어올렸는지 다시 체크
                            if (unloaderParameter.DI_Unloader_Picker_VacuumCheck((int)UnloaderParameter.PickerVacuumPos.Inner) ||
                                unloaderParameter.DI_Unloader_Picker_VacuumCheck((int)UnloaderParameter.PickerVacuumPos.Outer))
                            {
                                Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer 축, Module Picker Vacuum On 확인");

                                m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Complete;
                            }
                            else
                            {
                                //////////////////////////////////////////////////////////////////////////////////////////
                                //  복원 지점 체크용 (Work Stage 에서 Module Pick Up 실패) - Picker 공압이 형성되지  않음
                                //
                                m_bUL_Transfer_fromWorkStage_Module_PickUp_Complete_Flag = false;
                                //
                                //  복원 지점 체크용 (Work Stage 에서 Module Pick Up 실패)
                                //////////////////////////////////////////////////////////////////////////////////////////

                                m_bUnloader_WorkStage_PickUp_Retry = true;

                                Unloader_CurrentStatus_Save_StopedByTimeout();

                                m_strTemp = "Transfer 축, Module Picker Vacuum On 확인 실패)";
                                Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                                return AlarmPost(AlarmKey.UL_Transfer_Picker_Vacuum_On_Check);


                                Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer 축, Module Picker Vacuum On 확인 실패");
                                //////////////////////////////////////////////////////////////////////////////////////////
                                //  복원 지점 체크용 (Work Stage 에서 Module Pick Up 실패) - Picker 공압이 형성되지  않음
                                //
                                m_bUL_Transfer_fromWorkStage_Module_PickUp_Complete_Flag = false;
                                //
                                //  복원 지점 체크용 (Work Stage 에서 Module Pick Up 실패)
                                //////////////////////////////////////////////////////////////////////////////////////////
                                //  알람 정지 (LED Bar - Red Blink)
                                Equipment.MachineStop_byAlarm = true;
                                //////////////////////////////////////////////////////////////////////////////////////////
                                //  재시작 위치 저장용
                                //
                                m_bUnloader_WorkStage_PickUp_Retry = true;
                                Equipment.MachineStop_byTimeout_Unloader = true;
                                Unloader_CurrentStatus_Save_StopedByTimeout();
                                //
                                //  재시작 위치 저장용
                                //////////////////////////////////////////////////////////////////////////////////////////
                                return AlarmPost(AlarmKey.UL_Transfer_Picker_Vacuum_On_Check);
                                m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.None;
                                MessageBox.Show("Module Picker Vacuum On 확인 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Complete;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULTR) > 60000)
                    {
                        m_strTemp = "Transfer Z 축, 대기 위치로 2단계 이동 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Transfer_Z_Move_To_Ready_Pos_Fail);

                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, 대기 위치로 2단계 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        return AlarmPost(AlarmKey.UL_Transfer_Z_Move_To_Ready_Pos_Fail);
                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, 대기 위치로 2단계 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                /// <summary>
                /// Work Stage 에서 Module Pick Up - 완료
                /// </summary>



                /// <summary>
                /// Module 을 Stacker0 에 Put Down - 시작
                /// </summary>
                case (int)Unloader_Transfer_Step.Stacker0_ModulePutdown_Condition_Check:                            //  Stacker0 에 Module Put Down 조건 체크 (Stacker Module Full Sensor On Check, Transfer Picker Vacuum On Check, Stage Vacuum Off Check, Drilling Cycle : None, Stacker Cycle : None)

                    Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "TR 축, Stacker0 에 Module PutDown 조건 체크");

                    if (!unloaderParameter.DI_Unloader_Stacker_FullCheck((int)UnloaderParameter.StackerTable.Stacker_0))            //  감지 시 Off
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Stacker0 의 Full 감지 센서에 Module 이 감지됨.");

                        if (m_bStacker0_Complete)
                        {
                            Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Stacker0 의 Full 감지 센서는 감지됨. Module PutDown 조건은 OK");

                            m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Stacker0PutDown_TransferZ_Move_ReadyPos;
                        }
                        else
                        {
                            m_strTemp = "Stacker0 의 Full 감지 센서에 Module 이 감지되지 않고, Module PutDown 조건도 NG.";
                            Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                            return AlarmPost(AlarmKey.UL_Transfer_Error);

                            Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Stacker0 의 Full 감지 센서에 Module 이 감지되지 않고, Module PutDown 조건도 NG.");
                            //  Out.
                            m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.None;
                        }
                    }
                    else if (Equipment.Machine_VacuumSensor_Enable && !unloaderParameter.DI_Unloader_Picker_VacuumCheck((int)UnloaderParameter.PickerVacuumPos.Inner) &&
                                                                    !unloaderParameter.DI_Unloader_Picker_VacuumCheck((int)UnloaderParameter.PickerVacuumPos.Outer))
                    {
                        Unloader_CurrentStatus_Save_StopedByTimeout();

                        m_strTemp = "Picker 에 자재가 감지되지 않음.";
                        Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Transfer_Picker_Module_NotExist);

                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Picker 에 자재가 감지되지 않음.");

                        //  Out.

                        //////////////////////////////////////////////////////////////////////////////////////////
                        //  재시작 위치 저장용
                        //
                        Equipment.MachineStop_byTimeout_Unloader = true;
                        Unloader_CurrentStatus_Save_StopedByTimeout();
                        //
                        //  재시작 위치 저장용
                        //////////////////////////////////////////////////////////////////////////////////////////

                        return AlarmPost(AlarmKey.UL_Transfer_Picker_Module_NotExist);
                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.None;
                    }
                    else if (m_nStacker0_ModulePutdownWaitingPos_Step > (int)StackerModulePutdownWaitingPos_Step.None)                                                       //  Stacker0 이 동작중
                    {
                        Unloader_CurrentStatus_Save_StopedByTimeout();

                        m_strTemp = "Stacker0 이 동작중이므로 Module PutDown 동작 중지.";
                        Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Stacker0_Running);

                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Stacker0 이 동작중이므로 Module PutDown 동작 중지.");

                        //  Out. (Stacker 동작이 완료되면 진행하도록 대기할 것인지는 테스트 하면서 결정하기로 함)
                        //////////////////////////////////////////////////////////////////////////////////////////
                        //  재시작 위치 저장용
                        //
                        Equipment.MachineStop_byTimeout_Unloader = true;
                        Unloader_CurrentStatus_Save_StopedByTimeout();
                        //
                        //  재시작 위치 저장용
                        //////////////////////////////////////////////////////////////////////////////////////////

                        return AlarmPost(AlarmKey.UL_Stacker0_Running);
                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.None;
                    }
                    else
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Stacker0 Module PutDown 조건 OK");
                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Stacker0PutDown_TransferZ_Move_ReadyPos;
                    }
                    break;


                case (int)Unloader_Transfer_Step.Stacker0PutDown_TransferZ_Move_ReadyPos:                            //  Transfer Z 축, 대기 위치로 이동

                    Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 시작");

                    unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Transfer_To_R_Port");

                    //  Target Position 변경 : 대기 위치
                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_TR_SafetyPos].UL_Transfer_Z;

                    //  속도
                    m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Coarse;

                    //  가감속
                    m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Coarse;

                    MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                        unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z],
                                        m_dSpeed,
                                        m_dAccDec,
                                        m_dAccDec);
                    TickCount_Start((int)TickType.TICK_ULTR);

                    m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Stacker0PutDown_TransferZ_Move_ReadyPos_DoneCheck;
                    break;


                case (int)Unloader_Transfer_Step.Stacker0PutDown_TransferZ_Move_ReadyPos_DoneCheck:                       //  Transfer Z 축, 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 완료");

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Stacker0PutDown_TransferX_Move_StackerPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULTR) > 60000)
                    {
                        m_strTemp = "Transfer Z 축, 대기 위치로 이동 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Transfer_Error);

                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, 대기 위치로 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Unloader_Transfer_Step.Stacker0PutDown_TransferX_Move_StackerPos:                            //  Transfer X 축, Stacker 위치로 이동

                    Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer X 축, Stacker0 위치로 이동 시작");

                    unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Transfer_To_R_Port");

                    //  Target Position 변경 : Stacker0 위치
                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_X] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_TR_RPortPos].UL_Transfer_X;

                    //  속도
                    m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_X].Common_Speed_Coarse;

                    //  가감속
                    m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_X].Common_Acceleration_Coarse;

                    MC_Func.MC_MovePosition((int)nAxis.TR_X,
                                        unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_X],
                                        m_dSpeed,
                                        m_dAccDec,
                                        m_dAccDec);
                    TickCount_Start((int)TickType.TICK_ULTR);

                    m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Stacker0PutDown_TransferX_Move_StackerPos_DoneCheck;
                    break;


                case (int)Unloader_Transfer_Step.Stacker0PutDown_TransferX_Move_StackerPos_DoneCheck:                       //  Transfer X 축, Stacker 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_X) && MC_Func.MC_PosTolerance((int)nAxis.TR_X, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_X]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer X 축, Stacker0 위치로 이동 완료");

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Stacker0PutDown_TransferZ_Move_PutDownPos_1stStep;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULTR) > 60000)
                    {
                        m_strTemp = "Transfer X 축, Stacker0 위치로 이동 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Transfer_Error);

                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer X 축, Stacker0 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.None;

                        MessageBox.Show("Transfer X 축, Stacker0 위치로 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Unloader_Transfer_Step.Stacker0PutDown_TransferZ_Move_PutDownPos_1stStep:                            //  Transfer Z 축, Module Put Down 위치로 이동 (1단계, 최종 위치에서 위로 10 mm)

                    Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, Module Put Down 대기 위치로 이동 시작. (10mm 위)");

                    unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Transfer_To_R_Port");

                    //  Target Position 변경 : Module Pickup 대기 위치
                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_TR_RPortPos].UL_Transfer_Z + 10.0;

                    //  속도
                    m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Coarse;

                    //  가감속
                    m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Coarse;

                    MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                        unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z],
                                        m_dSpeed,
                                        m_dAccDec,
                                        m_dAccDec);
                    TickCount_Start((int)TickType.TICK_ULTR);

                    m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Stacker0PutDown_TransferZ_Move_PutDownPos_1stStep_DoneCheck;
                    break;


                case (int)Unloader_Transfer_Step.Stacker0PutDown_TransferZ_Move_PutDownPos_1stStep_DoneCheck:                       //  Transfer Z 축, Module Put Down 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, Module Put Down 대기 위치로 이동 완료");

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Stacker0PutDown_TransferZ_Move_PutDownPos_2ndStep;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULTR) > 60000)
                    {
                        m_strTemp = "Transfer Z 축, Module Put Down 대기 위치로 이동 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Transfer_Error);

                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, Module Put Down 대기 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, Module Put Down 대기 위치로 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Unloader_Transfer_Step.Stacker0PutDown_TransferZ_Move_PutDownPos_2ndStep:                            //  Transfer Z 축, Module Put Down 위치로 이동 (2단계, 최종 위치)

                    Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, Module Put Down 위치로 이동 시작.");

                    unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Transfer_To_R_Port");

                    //  Target Position 변경 : Module Put Down 위치
                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_TR_RPortPos].UL_Transfer_Z;

                    //  속도
                    m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Fine;

                    //  가감속
                    m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Fine;

                    MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                        unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z],
                                        m_dSpeed,
                                        m_dAccDec,
                                        m_dAccDec);
                    TickCount_Start((int)TickType.TICK_ULTR);

                    m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Stacker0PutDown_TransferZ_Move_PutDownPos_2ndStep_DoneCheck;
                    break;


                case (int)Unloader_Transfer_Step.Stacker0PutDown_TransferZ_Move_PutDownPos_2ndStep_DoneCheck:                       //  Transfer Z 축, Module Put Down 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, Module Put Down 위치로 이동 완료");

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Stacker0PutDown_Transfer_PickerVacuum_Off;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULTR) > 60000)
                    {
                        m_strTemp = "Transfer Z 축, Module Put Down 위치로 이동 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Transfer_Error);

                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, Module Put Down 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, Module Put Down 위치로 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Unloader_Transfer_Step.Stacker0PutDown_Transfer_PickerVacuum_Off:                            //  Transfer, Module Picker Vacuum Off (and Blow On)

                    Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer 축, Module Picker Vacuum Off");

                    unloaderParameter.DO_Unloader_Picker_Vacuum((int)UnloaderParameter.PickerVacuumPos.Inner, false);
                    unloaderParameter.DO_Unloader_Picker_Vacuum((int)UnloaderParameter.PickerVacuumPos.Outer, false);
                    unloaderParameter.DO_Unloader_Picker_Blow(true);

                    TickCount_Start((int)TickType.TICK_ULTR);

                    m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Stacker0PutDown_Transfer_PickerVacuum_OffCheck;
                    break;


                case (int)Unloader_Transfer_Step.Stacker0PutDown_Transfer_PickerVacuum_OffCheck:                       //  Transfer, Module Picker Vacuum Off 확인 (then Blow Off)

                    if (!unloaderParameter.DI_Unloader_Picker_VacuumCheck((int)UnloaderParameter.PickerVacuumPos.Inner) &&
                        !unloaderParameter.DI_Unloader_Picker_VacuumCheck((int)UnloaderParameter.PickerVacuumPos.Outer))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer 축, Module Picker Vacuum Off 완료");

                        unloaderParameter.DO_Unloader_Picker_Blow(false);

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Stacker0PutDown_TransferZ_Move_ReadyPos2_1stStep;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULTR) > 10000)
                    {
                        unloaderParameter.DO_Unloader_Picker_Blow(false);

                        m_strTemp = "Transfer 축, Module Picker Vacuum Off 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Transfer_Error);

                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer 축, Module Picker Vacuum Off 실패. (Timeout)");

                        unloaderParameter.DO_Unloader_Picker_Blow(false);

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.None;

                        MessageBox.Show("Transfer 축, Module Picker Vacuum Off 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Unloader_Transfer_Step.Stacker0PutDown_TransferZ_Move_ReadyPos2_1stStep:                            //  Transfer Z 축, 대기 위치로 이동 (1단계, 현재 위치에서 위로 10 mm)

                    Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 시작. (1단계, 현재 위치에서 10mm 위)");

                    unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Transfer_To_R_Port");

                    //  Target Position 변경 : 대기 위치 1단계
                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_TR_RPortPos].UL_Transfer_Z + 10.0;

                    //  속도
                    m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Fine;

                    //  가감속
                    m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Fine;

                    MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                        unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z],
                                        m_dSpeed,
                                        m_dAccDec,
                                        m_dAccDec);
                    TickCount_Start((int)TickType.TICK_ULTR);

                    m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Stacker0PutDown_TransferZ_Move_ReadyPos2_1stStep_DoneCheck;
                    break;


                case (int)Unloader_Transfer_Step.Stacker0PutDown_TransferZ_Move_ReadyPos2_1stStep_DoneCheck:                       //  Transfer Z 축, 대기 위치로 이동 완료 확인 (then Picker Blow Off)

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && 
                        MC_Func.MC_PosTolerance((int)nAxis.TR_Z, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, 대기 위치로 1단계 이동 완료");

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Stacker0PutDown_TransferZ_Move_ReadyPos2_2ndStep;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULTR) > 60000)
                    {
                        m_strTemp = "Transfer Z 축, 대기 위치로 1단계 이동 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Transfer_Error);

                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, 대기 위치로 1단계 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, 대기 위치로 1단계 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Unloader_Transfer_Step.Stacker0PutDown_TransferZ_Move_ReadyPos2_2ndStep:                            //  Transfer Z 축, 대기 위치로 이동 (2단계, 최종 위치)

                    Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 시작. (2단계, 최종 위치)");

                    unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Transfer_To_R_Port");

                    //  Target Position 변경 : 대기 위치 2단계 (최종 위치)
                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_TR_SafetyPos].UL_Transfer_Z;

                    //  속도
                    m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Coarse;

                    //  가감속
                    m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Coarse;

                    MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                        unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z],
                                        m_dSpeed,
                                        m_dAccDec,
                                        m_dAccDec);
                    TickCount_Start((int)TickType.TICK_ULTR);

                    m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Stacker0PutDown_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck;
                    break;


                case (int)Unloader_Transfer_Step.Stacker0PutDown_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck:                       //  Transfer Z 축, 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, 대기 위치로 2단계 이동 완료");

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Complete;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULTR) > 60000)
                    {
                        m_strTemp = "Transfer Z 축, 대기 위치로 2단계 이동 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Transfer_Error);

                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, 대기 위치로 2단계 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, 대기 위치로 2단계 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                /// <summary>
                /// Module 을 Stacker0 에 Put Down - 완료
                /// </summary>



                /// <summary>
                /// Module 을 Stacker1 에 Put Down - 시작
                /// </summary>
                case (int)Unloader_Transfer_Step.Stacker1_ModulePutdown_Condition_Check:                            //  Stacker1 에 Module Put Down 조건 체크 (Stacker Module Full Sensor On Check, Transfer Picker Vacuum On Check, Stage Vacuum Off Check, Drilling Cycle : None, Stacker Cycle : None)

                    Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "TR 축, Stacker1 에 Module PutDown 조건 체크");

                    if (!unloaderParameter.DI_Unloader_Stacker_FullCheck((int)UnloaderParameter.StackerTable.Stacker_1))            //  감지 시 Off
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Stacker1 의 Full 감지 센서에 Module 이 감지됨.");

                        if (m_bStacker1_Complete)
                        {
                            Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Stacker1 의 Full 감지 센서는 감지됨. Module PutDown 조건은 OK");

                            m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Stacker1PutDown_TransferZ_Move_ReadyPos;
                        }
                        else
                        {
                            m_strTemp = "Stacker1 의 Full 감지 센서에 Module 이 감지되지 않고, Module PutDown 조건도 NG.";
                            Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                            return AlarmPost(AlarmKey.UL_Transfer_Error);

                            Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Stacker1 의 Full 감지 센서에 Module 이 감지되지 않고, Module PutDown 조건도 NG.");
                            //  Out.
                            m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.None;
                        }
                    }
                    else if (Equipment.Machine_VacuumSensor_Enable && !unloaderParameter.DI_Unloader_Picker_VacuumCheck((int)UnloaderParameter.PickerVacuumPos.Inner) &&
                                                                    !unloaderParameter.DI_Unloader_Picker_VacuumCheck((int)UnloaderParameter.PickerVacuumPos.Outer))
                    {
                        Unloader_CurrentStatus_Save_StopedByTimeout();

                        m_strTemp = "Picker 에 자재가 감지되지 않음.";
                        Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Transfer_Picker_Module_NotExist);

                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Picker 에 자재가 감지되지 않음.");

                        //  Out.

                        //////////////////////////////////////////////////////////////////////////////////////////
                        //  재시작 위치 저장용
                        //
                        Equipment.MachineStop_byTimeout_Unloader = true;
                        Unloader_CurrentStatus_Save_StopedByTimeout();
                        //
                        //  재시작 위치 저장용
                        //////////////////////////////////////////////////////////////////////////////////////////

                        return AlarmPost(AlarmKey.UL_Transfer_Picker_Module_NotExist);
                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.None;
                    }
                    else if (m_nStacker1_ModulePutdownWaitingPos_Step > (int)StackerModulePutdownWaitingPos_Step.None)                                                       //  Stacker1 이 동작중
                    {
                        Unloader_CurrentStatus_Save_StopedByTimeout();

                        m_strTemp = "Stacker1 이 동작중이므로 Module PutDown 동작 중지.";
                        Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Transfer_Error);

                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Stacker1 이 동작중이므로 Module PutDown 동작 중지.");

                        //  Out. (Stacker 동작이 완료되면 진행하도록 대기할 것인지는 테스트 하면서 결정하기로 함)

                        //////////////////////////////////////////////////////////////////////////////////////////
                        //  재시작 위치 저장용
                        //
                        Equipment.MachineStop_byTimeout_Unloader = true;
                        Unloader_CurrentStatus_Save_StopedByTimeout();
                        //
                        //  재시작 위치 저장용
                        //////////////////////////////////////////////////////////////////////////////////////////

                        return AlarmPost(AlarmKey.UL_Stacker1_Running);
                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.None;
                    }
                    else
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Stacker1 Module PutDown 조건 OK");

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Stacker1PutDown_TransferZ_Move_ReadyPos;
                    }
                    break;


                case (int)Unloader_Transfer_Step.Stacker1PutDown_TransferZ_Move_ReadyPos:                            //  Transfer Z 축, 대기 위치로 이동

                    Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 시작");

                    unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Transfer_To_L_Port");

                    //  Target Position 변경 : 대기 위치
                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_TR_SafetyPos].UL_Transfer_Z;

                    //  속도
                    m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Coarse;

                    //  가감속
                    m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Coarse;

                    MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                        unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z],
                                        m_dSpeed,
                                        m_dAccDec,
                                        m_dAccDec);
                    TickCount_Start((int)TickType.TICK_ULTR);

                    m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Stacker1PutDown_TransferZ_Move_ReadyPos_DoneCheck;
                    break;


                case (int)Unloader_Transfer_Step.Stacker1PutDown_TransferZ_Move_ReadyPos_DoneCheck:                       //  Transfer Z 축, 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && 
                        MC_Func.MC_PosTolerance((int)nAxis.TR_Z, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 완료");

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Stacker1PutDown_TransferX_Move_StackerPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULTR) > 60000)
                    {
                        m_strTemp = "Transfer Z 축, 대기 위치로 이동 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Transfer_Error);

                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, 대기 위치로 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Unloader_Transfer_Step.Stacker1PutDown_TransferX_Move_StackerPos:                            //  Transfer X 축, Stacker 위치로 이동

                    Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer X 축, Stacker1 위치로 이동 시작");

                    unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Transfer_To_L_Port");

                    //  Target Position 변경 : Stacker1 위치
                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_X] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_TR_LPortPos].UL_Transfer_X;

                    //  속도
                    m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_X].Common_Speed_Coarse;

                    //  가감속
                    m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_X].Common_Acceleration_Coarse;

                    MC_Func.MC_MovePosition((int)nAxis.TR_X,
                                        unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_X],
                                        m_dSpeed,
                                        m_dAccDec,
                                        m_dAccDec);
                    TickCount_Start((int)TickType.TICK_ULTR);

                    m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Stacker1PutDown_TransferX_Move_StackerPos_DoneCheck;
                    break;


                case (int)Unloader_Transfer_Step.Stacker1PutDown_TransferX_Move_StackerPos_DoneCheck:                       //  Transfer X 축, Stacker 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_X) && 
                        MC_Func.MC_PosTolerance((int)nAxis.TR_X, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_X]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer X 축, Stacker1 위치로 이동 완료");

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Stacker1PutDown_TransferZ_Move_PutDownPos_1stStep;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULTR) > 60000)
                    {
                        m_strTemp = "Transfer X 축, Stacker1 위치로 이동 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Transfer_Error);

                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer X 축, Stacker1 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.None;

                        MessageBox.Show("Transfer X 축, Stacker1 위치로 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Unloader_Transfer_Step.Stacker1PutDown_TransferZ_Move_PutDownPos_1stStep:                            //  Transfer Z 축, Module Put Down 위치로 이동 (1단계, 최종 위치에서 위로 10 mm)

                    Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, Module Put Down 대기 위치로 이동 시작. (10mm 위)");

                    unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Transfer_To_L_Port");

                    //  Target Position 변경 : Module Pickup 대기 위치
                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_TR_LPortPos].UL_Transfer_Z + 10.0;

                    //  속도
                    m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Coarse;

                    //  가감속
                    m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Coarse;

                    MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                        unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z],
                                        m_dSpeed,
                                        m_dAccDec,
                                        m_dAccDec);
                    TickCount_Start((int)TickType.TICK_ULTR);

                    m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Stacker1PutDown_TransferZ_Move_PutDownPos_1stStep_DoneCheck;
                    break;


                case (int)Unloader_Transfer_Step.Stacker1PutDown_TransferZ_Move_PutDownPos_1stStep_DoneCheck:                       //  Transfer Z 축, Module Put Down 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, Module Put Down 대기 위치로 이동 완료");

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Stacker1PutDown_TransferZ_Move_PutDownPos_2ndStep;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULTR) > 60000)
                    {
                        m_strTemp = "Transfer Z 축, Module Put Down 대기 위치로 이동 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Transfer_Error);

                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, Module Put Down 대기 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, Module Put Down 대기 위치로 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Unloader_Transfer_Step.Stacker1PutDown_TransferZ_Move_PutDownPos_2ndStep:                            //  Transfer Z 축, Module Put Down 위치로 이동 (2단계, 최종 위치)

                    Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, Module Put Down 위치로 이동 시작.");

                    unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Transfer_To_L_Port");

                    //  Target Position 변경 : Module Put Down 위치
                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_TR_LPortPos].UL_Transfer_Z;

                    //  속도
                    m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Fine;

                    //  가감속
                    m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Fine;

                    MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                        unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z],
                                        m_dSpeed,
                                        m_dAccDec,
                                        m_dAccDec);
                    TickCount_Start((int)TickType.TICK_ULTR);

                    m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Stacker1PutDown_TransferZ_Move_PutDownPos_2ndStep_DoneCheck;
                    break;


                case (int)Unloader_Transfer_Step.Stacker1PutDown_TransferZ_Move_PutDownPos_2ndStep_DoneCheck:                       //  Transfer Z 축, Module Put Down 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, Module Put Down 위치로 이동 완료");

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Stacker1PutDown_Transfer_PickerVacuum_Off;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULTR) > 60000)
                    {
                        m_strTemp = "Transfer Z 축, Module Put Down 위치로 이동 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Transfer_Error);

                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, Module Put Down 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, Module Put Down 위치로 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Unloader_Transfer_Step.Stacker1PutDown_Transfer_PickerVacuum_Off:                            //  Transfer, Module Picker Vacuum Off (and Blow On)

                    Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer 축, Module Picker Vacuum Off");

                    unloaderParameter.DO_Unloader_Picker_Vacuum((int)UnloaderParameter.PickerVacuumPos.Inner, false);
                    unloaderParameter.DO_Unloader_Picker_Vacuum((int)UnloaderParameter.PickerVacuumPos.Outer, false);
                    unloaderParameter.DO_Unloader_Picker_Blow(true);

                    TickCount_Start((int)TickType.TICK_ULTR);

                    m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Stacker1PutDown_Transfer_PickerVacuum_OffCheck;
                    break;


                case (int)Unloader_Transfer_Step.Stacker1PutDown_Transfer_PickerVacuum_OffCheck:                       //  Transfer, Module Picker Vacuum Off 확인 (then Blow Off)

                    if (!unloaderParameter.DI_Unloader_Picker_VacuumCheck((int)UnloaderParameter.PickerVacuumPos.Inner) &&
                        !unloaderParameter.DI_Unloader_Picker_VacuumCheck((int)UnloaderParameter.PickerVacuumPos.Outer))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer 축, Module Picker Vacuum Off 완료");

                        unloaderParameter.DO_Unloader_Picker_Blow(false);

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Stacker1PutDown_TransferZ_Move_ReadyPos2_1stStep;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULTR) > 10000)
                    {
                        unloaderParameter.DO_Unloader_Picker_Blow(false);

                        m_strTemp = "Transfer 축, Module Picker Vacuum Off 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Transfer_Error);

                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer 축, Module Picker Vacuum Off 실패. (Timeout)");

                        unloaderParameter.DO_Unloader_Picker_Blow(false);

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.None;

                        MessageBox.Show("Transfer 축, Module Picker Vacuum Off 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Unloader_Transfer_Step.Stacker1PutDown_TransferZ_Move_ReadyPos2_1stStep:                            //  Transfer Z 축, 대기 위치로 이동 (1단계, 현재 위치에서 위로 10 mm)

                    Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 시작. (1단계, 현재 위치에서 10mm 위)");

                    unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Transfer_To_L_Port");

                    //  Target Position 변경 : 대기 위치 1단계
                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_TR_LPortPos].UL_Transfer_Z + 10.0;

                    //  속도
                    m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Fine;

                    //  가감속
                    m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Fine;

                    MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                        unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z],
                                        m_dSpeed,
                                        m_dAccDec,
                                        m_dAccDec);
                    TickCount_Start((int)TickType.TICK_ULTR);

                    m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Stacker1PutDown_TransferZ_Move_ReadyPos2_1stStep_DoneCheck;
                    break;


                case (int)Unloader_Transfer_Step.Stacker1PutDown_TransferZ_Move_ReadyPos2_1stStep_DoneCheck:                       //  Transfer Z 축, 대기 위치로 이동 완료 확인 (then Picker Blow Off)

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && 
                        MC_Func.MC_PosTolerance((int)nAxis.TR_Z, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, 대기 위치로 1단계 이동 완료");

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Stacker1PutDown_TransferZ_Move_ReadyPos2_2ndStep;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULTR) > 60000)
                    {
                        m_strTemp = "Transfer Z 축, 대기 위치로 1단계 이동 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Transfer_Error);

                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, 대기 위치로 1단계 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, 대기 위치로 1단계 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Unloader_Transfer_Step.Stacker1PutDown_TransferZ_Move_ReadyPos2_2ndStep:                            //  Transfer Z 축, 대기 위치로 이동 (2단계, 최종 위치)

                    Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 시작. (2단계, 최종 위치)");

                    unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Transfer_To_L_Port");

                    //  Target Position 변경 : 대기 위치 2단계 (최종 위치)
                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_TR_SafetyPos].UL_Transfer_Z;

                    //  속도
                    m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Coarse;

                    //  가감속
                    m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Coarse;

                    MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                        unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z],
                                        m_dSpeed,
                                        m_dAccDec,
                                        m_dAccDec);
                    TickCount_Start((int)TickType.TICK_ULTR);

                    m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Stacker1PutDown_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck;
                    break;


                case (int)Unloader_Transfer_Step.Stacker1PutDown_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck:                       //  Transfer Z 축, 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, 대기 위치로 2단계 이동 완료");

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Complete;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULTR) > 60000)
                    {
                        m_strTemp = "Transfer Z 축, 대기 위치로 2단계 이동 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Transfer_Error);

                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, 대기 위치로 2단계 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, 대기 위치로 2단계 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                /// <summary>
                /// Module 을 Stacker1 에 Put Down - 완료
                /// </summary>



                /// <summary>
                /// Module 을 NG-Stacker 에 Drop - 시작
                /// </summary>
                case (int)Unloader_Transfer_Step.NgStacker_ModuleDrop_Condition_Check:                            //  NG-Stacker 에 Module Drop 조건 체크 (Transfer Picker Vacuum On Check, NG Stacker Full Sensor Off Check)

                    Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "TR 축, NG-Port 에 Module Drop 조건 체크");

                    if (!workStage.m_bMainWorkCycle_DryRun && Equipment.Machine_VacuumSensor_Enable && !unloaderParameter.DI_Unloader_Picker_VacuumCheck((int)UnloaderParameter.PickerVacuumPos.Inner) && 
                                                                                                        !unloaderParameter.DI_Unloader_Picker_VacuumCheck((int)UnloaderParameter.PickerVacuumPos.Outer))
                    {
                        Unloader_CurrentStatus_Save_StopedByTimeout();

                        m_strTemp = "Picker 에 Module 이 감지되지 않음.";
                        Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Transfer_Picker_Module_NotExist);

                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Picker 에 Module 이 감지되지 않음.");

                        //  Out.

                        //////////////////////////////////////////////////////////////////////////////////////////
                        //  재시작 위치 저장용
                        //
                        Equipment.MachineStop_byTimeout_Unloader = true;
                        Unloader_CurrentStatus_Save_StopedByTimeout();
                        //
                        //  재시작 위치 저장용
                        //////////////////////////////////////////////////////////////////////////////////////////

                        return AlarmPost(AlarmKey.UL_Transfer_Picker_Module_NotExist);
                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.None;
                    }
                    else if (!unloaderParameter.DI_Unloader_NG_Stacker_FullCheck())              //  감지 시 Off
                    {
                        Unloader_CurrentStatus_Save_StopedByTimeout();

                        m_strTemp = "NG-Port Full.";
                        Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Transfer_Picker_Module_NotExist);

                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "NG-Port Full.");

                        //  Out.

                        //////////////////////////////////////////////////////////////////////////////////////////
                        //  재시작 위치 저장용
                        //
                        Equipment.MachineStop_byTimeout_Unloader = true;
                        Unloader_CurrentStatus_Save_StopedByTimeout();
                        //
                        //  재시작 위치 저장용
                        //////////////////////////////////////////////////////////////////////////////////////////

                        return AlarmPost(AlarmKey.UL_Transfer_Picker_Module_NotExist);
                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.None;
                    }
                    else
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "TR 축, NG-Port 에 Module Drop 조건 OK");

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.NgStackerDrop_TransferZ_Move_ReadyPos;
                    }
                    break;


                case (int)Unloader_Transfer_Step.NgStackerDrop_TransferZ_Move_ReadyPos:                            //  Transfer Z 축, 대기 위치로 이동

                    Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 시작");

                    unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Transfer_To_F_Port");

                    //  Target Position 변경 : 대기 위치
                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_TR_SafetyPos].UL_Transfer_Z;

                    //  속도
                    m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Coarse;

                    //  가감속
                    m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Coarse;

                    MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                        unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z],
                                        m_dSpeed,
                                        m_dAccDec,
                                        m_dAccDec);

                    TickCount_Start((int)TickType.TICK_ULTR);

                    m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.NgStackerDrop_TransferZ_Move_ReadyPos_DoneCheck;
                    break;


                case (int)Unloader_Transfer_Step.NgStackerDrop_TransferZ_Move_ReadyPos_DoneCheck:                       //  Transfer Z 축, 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && 
                        MC_Func.MC_PosTolerance((int)nAxis.TR_Z, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 완료");

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.NgStackerDrop_TransferX_Move_NgStackerPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULTR) > 60000)
                    {
                        m_strTemp = "Transfer Z 축, 대기 위치로 이동 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Transfer_Picker_Module_NotExist);

                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, 대기 위치로 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Unloader_Transfer_Step.NgStackerDrop_TransferX_Move_NgStackerPos:                            //  Transfer X 축, NG-Stacker 위치로 이동

                    Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer X 축, NG-Port Module Drop 위치로 이동 시작");

                    unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Transfer_To_F_Port");

                    //  Target Position 변경 : M-Aligner 위치
                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_X] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_TR_FPortPos].UL_Transfer_X;

                    //  속도
                    m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_X].Common_Speed_Coarse;

                    //  가감속
                    m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_X].Common_Acceleration_Coarse;

                    MC_Func.MC_MovePosition((int)nAxis.TR_X,
                                        unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_X],
                                        m_dSpeed,
                                        m_dAccDec,
                                        m_dAccDec);

                    TickCount_Start((int)TickType.TICK_ULTR);

                    m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.NgStackerDrop_TransferX_Move_NgStackerPos_DoneCheck;
                    break;


                case (int)Unloader_Transfer_Step.NgStackerDrop_TransferX_Move_NgStackerPos_DoneCheck:                       //  Transfer X 축, NG-Stacker 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_X) && 
                        MC_Func.MC_PosTolerance((int)nAxis.TR_X, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_X]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer X 축, NG-Port Module Drop 위치로 이동 완료");

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.NgStackerDrop_TransferZ_Move_DropPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULTR) > 60000)
                    {
                        m_strTemp = "Transfer X 축, NG-Port Module Drop 위치로 이동 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Transfer_Error);

                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer X 축, NG-Port Module Drop 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.None;

                        MessageBox.Show("Transfer X 축, NG-Port Module Drop 위치로 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Unloader_Transfer_Step.NgStackerDrop_TransferZ_Move_DropPos:                            //  Transfer Z 축, Module Drop 위치로 이동

                    Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, Module Drop 위치로 이동 시작.");

                    unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Transfer_To_F_Port");

                    //  Target Position 변경 : Module Pickup 위치
                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_TR_FPortPos].UL_Transfer_Z;

                    //  속도
                    m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Coarse;

                    //  가감속
                    m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Coarse;

                    MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                        unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z],
                                        m_dSpeed,
                                        m_dAccDec,
                                        m_dAccDec);

                    TickCount_Start((int)TickType.TICK_ULTR);

                    m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.NgStackerDrop_TransferZ_Move_DropPos_DoneCheck;
                    break;


                case (int)Unloader_Transfer_Step.NgStackerDrop_TransferZ_Move_DropPos_DoneCheck:                       //  Transfer Z 축, Module Drop 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, Module Drop 위치로 이동 완료");

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.NgStackerDrop_Transfer_PickerVacuum_Off;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULTR) > 60000)
                    {
                        m_strTemp = "Transfer Z 축, Module Drop 위치로 이동 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Transfer_Error);

                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, Module Drop 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, Module Drop 위치로 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Unloader_Transfer_Step.NgStackerDrop_Transfer_PickerVacuum_Off:                            //  Transfer, Module Picker Vacuum Off (and Blow On)

                    Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer 축, Module Picker Vacuum Off");

                    unloaderParameter.DO_Unloader_Picker_Vacuum((int)UnloaderParameter.PickerVacuumPos.Inner, false);
                    unloaderParameter.DO_Unloader_Picker_Vacuum((int)UnloaderParameter.PickerVacuumPos.Outer, false);
                    unloaderParameter.DO_Unloader_Picker_Blow(true);

                    TickCount_Start((int)TickType.TICK_ULTR);

                    m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.NgStackerDrop_Transfer_PickerVacuum_OffCheck;
                    break;


                case (int)Unloader_Transfer_Step.NgStackerDrop_Transfer_PickerVacuum_OffCheck:                       //  Transfer, Module Picker Vacuum Off 확인 (the Blow Off)

                    if (!unloaderParameter.DI_Unloader_Picker_VacuumCheck((int)UnloaderParameter.PickerVacuumPos.Inner) &&
                        !unloaderParameter.DI_Unloader_Picker_VacuumCheck((int)UnloaderParameter.PickerVacuumPos.Outer))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer 축, Module Picker Vacuum Off 완료");

                        unloaderParameter.DO_Unloader_Picker_Blow(false);

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.NgStackerDrop_TransferZ_Move_ReadyPos2;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULTR) > 10000)
                    {
                        unloaderParameter.DO_Unloader_Picker_Blow(false);

                        m_strTemp = "Transfer 축, Module Picker Vacuum Off 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Transfer_Error);

                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer 축, Module Picker Vacuum Off 실패. (Timeout)");

                        unloaderParameter.DO_Unloader_Picker_Blow(false);

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.None;

                        MessageBox.Show("Transfer 축, Module Picker Vacuum Off 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Unloader_Transfer_Step.NgStackerDrop_TransferZ_Move_ReadyPos2:                            //  Transfer Z 축, 대기 위치로 이동

                    Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 시작.");

                    unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Transfer_To_L_Port");

                    //  Target Position 변경 : 대기 위치 2단계 (최종 위치)
                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_TR_SafetyPos].UL_Transfer_Z;

                    //  속도
                    m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Coarse;

                    //  가감속
                    m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Coarse;

                    MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                        unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z],
                                        m_dSpeed,
                                        m_dAccDec,
                                        m_dAccDec);

                    TickCount_Start((int)TickType.TICK_ULTR);

                    m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.NgStackerDrop_TransferZ_Move_ReadyPos2_DoneCheck;
                    break;


                case (int)Unloader_Transfer_Step.NgStackerDrop_TransferZ_Move_ReadyPos2_DoneCheck:                       //  Transfer Z 축, 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && 
                        MC_Func.MC_PosTolerance((int)nAxis.TR_Z, unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 완료");

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.Complete;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ULTR) > 60000)
                    {
                        m_strTemp = "Transfer Z 축, 대기 위치로 이동 실패. (Timeout)";
                        Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                        return AlarmPost(AlarmKey.UL_Transfer_Error);

                        Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, 대기 위치로 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                /// <summary>
                /// Module 을 NG-Stacker 에 Drop - 완료
                /// </summary>



                case (int)Unloader_Transfer_Step.Complete:

                    Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "완료");

                    switch (m_nUnloaderTransferMoveType)
                    {
                        case (int)UnloaderTransferMoveType.Cycle_Transfer_ReadyPos:
                            Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Ready Position 이동 완료");
                            break;

                        case (int)UnloaderTransferMoveType.Cycle_WorkStage_PickUp:
                            Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Work Stage 에서 Module Pick Up 완료");

                            loader.m_bAUTORUN_Loader_Transfer_ModulePutDowntoWorkStage_Complete = false;                                                //  Unloader 에서 Work Stage 의 Module 을 가져갔으므로 false 로 만들어 줌. 
                            m_bAUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete = true;

                            workStage.m_bMainWorkCycle_Complete = false;
                            break;

                        case (int)UnloaderTransferMoveType.Cycle_Stacker0_PutDown:
                            Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Stacker0 에 Module Put Down 완료");

                            m_bAUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete = false;
                            //workStage.m_bMainWorkCycle_Complete = false;

                            workStage.m_nMainWorkCycle_ResultOKNG = (int)WorkStage.MainCycle_Result.None;       //  모듈을 Unloading 했으니 결과데이터 초기화
                            m_bStacker0_Complete = false;                                                       //  Module 내려놨으면 Stacker 높이 재조정해야 함.

                            //  Cycle Stop 이면?              --> Unloader 에게 Cycle Stop 은 Module 을 OK 또는 NG 위치에 내려놓으면 Stop
                            if (Equipment.CycleStop)
                            {
                                //  Loader 와 Work Stage 모두 Cycle Stop 되었을 때만 Unloader 를 cycle Stop 처리 한다.
                                if (Equipment.CycleStopped_LoaderTransfer && Equipment.CycleStopped_MainWork)
                                {
                                    //  Unloader Transfer 돌아가지 않게
                                    Equipment.CycleStopped_UnloaderTransfer = true;
                                }
                            }
                            break;

                        case (int)UnloaderTransferMoveType.Cycle_Stacker1_PutDown:
                            Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Stacker1 에 Module Put Down 완료");

                            m_bAUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete = false;
                            //workStage.m_bMainWorkCycle_Complete = false;

                            workStage.m_nMainWorkCycle_ResultOKNG = (int)WorkStage.MainCycle_Result.None;       //  모듈을 Unloading 했으니 결과데이터 초기화
                            m_bStacker1_Complete = false;                                                       //  Module 내려놨으면 Stacker 높이 재조정해야 함.

                            //  Cycle Stop 이면?              --> Unloader 에게 Cycle Stop 은 Module 을 OK 또는 NG 위치에 내려놓으면 Stop
                            if (Equipment.CycleStop)
                            {
                                //  Loader 와 Work Stage 모두 Cycle Stop 되었을 때만 Unloader 를 cycle Stop 처리 한다.
                                if (Equipment.CycleStopped_LoaderTransfer && Equipment.CycleStopped_MainWork)
                                {
                                    //  Unloader Transfer 돌아가지 않게
                                    Equipment.CycleStopped_UnloaderTransfer = true;
                                }
                            }
                            break;

                        case (int)UnloaderTransferMoveType.Cycle_NG_PutDown:
                            Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "NG-Port 에 Module Drop 완료");

                            m_bAUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete = false;
                            //workStage.m_bMainWorkCycle_Complete = false;

                            workStage.m_nMainWorkCycle_ResultOKNG = (int)WorkStage.MainCycle_Result.None;       //  모듈을 Unloading 했으니 결과데이터 초기화



                            //  Cycle Stop 이면?              --> Unloader 에게 Cycle Stop 은 Module 을 OK 또는 NG 위치에 내려놓으면 Stop
                            if (Equipment.CycleStop)
                            {
                                //  Loader 와 Work Stage 모두 Cycle Stop 되었을 때만 Unloader 를 cycle Stop 처리 한다.
                                if (Equipment.CycleStopped_LoaderTransfer && Equipment.CycleStopped_MainWork)
                                {
                                    //  Unloader Transfer 돌아가지 않게
                                    Equipment.CycleStopped_UnloaderTransfer = true;
                                }
                            }
                            break;

                        default:            //  Error
                            m_strTemp = "동작 타입 목록에 없음";
                            Log.Write("SLD-200", Equipment.User_Name, "Unloader_Transfer_Step", m_strTemp);
                            return AlarmPost(AlarmKey.UL_Transfer_Error);

                            Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "동작 타입 목록에 없음");
                            break;
                    }

                    m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.None;
                    break;
            }

            return 0;
        }

        private void Unloader_Transfer_Step_WorkStagePickUp_TransferZ_Move_ReadyPos2_2ndStep(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 시작. (2단계, 최종 위치)");

            unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Transfer_To_L_Port");

            //  Target Position 변경 : 대기 위치 2단계 (최종 위치)
            unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_TR_SafetyPos].UL_Transfer_Z;

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Coarse;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Coarse;

            MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_ULTR);
        }

        private void Unloader_Transfer_Step_WorkStagePickUp_TransferZ_Move_ReadyPos2_1stStep(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 시작. (1단계, 현재 위치에서 10mm 위)");

            unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Transfer_To_L_Port");

            //  Target Position 변경 : 현재 위치 에서 10 mm 위, 1단계 --> 현재 위치에서 10mm 올리던 것을, Table 위치에서 10mm 올리는 것으로 변경
            //unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z] = MC_Func.MC_GetEncPos((int)nAxis.TR_Z) + 10.0;
            unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_TR_WorkTablePos].UL_Transfer_Z + 10.0;

            //  Dry Run 모드이면 10mm 더 위로
            if (workStage.m_bMainWorkCycle_DryRun)
            {
                unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z] += 10.0;

                if (unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z] > 0)
                {
                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z] = 0;
                }
            }

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Fine;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Fine;

            MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_ULTR);
        }

        private void Unloader_Transfer_Step_WorkStagePickUp_TransferZ_Move_PickUpPos_2ndStep(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, Module Pickup 위치로 이동 시작.");

            unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Transfer_To_WorkStage");

            //  Target Position 변경 : Module Pickup 위치
            unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_TR_WorkTablePos].UL_Transfer_Z;

            //  Dry Run 모드이면 10mm 더 위로
            if (workStage.m_bMainWorkCycle_DryRun)
            {
                unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z] += 10.0;

                if (unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z] > 0)
                {
                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z] = 0;
                }
            }

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Fine;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Fine;

            MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_ULTR);
        }

        private void Unloader_Transfer_Step_WorkStagePickUp_TransferZ_Move_PickUpPos_1stStep(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, Module Pickup 대기 위치로 이동 시작. (10mm 위)");

            unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Transfer_To_WorkStage");

            //  Target Position 변경 : Module Pickup 대기 위치
            unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_TR_WorkTablePos].UL_Transfer_Z + 10.0;

            //  Dry Run 모드이면 10mm 더 위로
            if (workStage.m_bMainWorkCycle_DryRun)
            {
                unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z] += 10.0;

                if (unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z] > 0)
                {
                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z] = 0;
                }
            }

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Coarse;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Coarse;

            MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_ULTR);
        }

        private void Unloader_Transfer_Step_WorkStagePickUp_TransferX_Move_WorkStagePos(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer X 축, Work Stagre 위치로 이동 시작");

            unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Transfer_To_WorkStage");

            //  Target Position 변경 : M-Aligner 위치
            unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_X] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_TR_WorkTablePos].UL_Transfer_X;

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_X].Common_Speed_Coarse;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_X].Common_Acceleration_Coarse;

            MC_Func.MC_MovePosition((int)nAxis.TR_X,
                                unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_X],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_ULTR);
        }

        private void Unloader_Transfer_Step_WorkStagePickUp_TransferZ_Move_ReadyPos(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 시작");

            unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Transfer_To_WorkStage");

            //  Target Position 변경 : 대기 위치
            unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_TR_SafetyPos].UL_Transfer_Z;

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Coarse;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Coarse;

            MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_ULTR);
        }

        private void Unloader_Transfer_Step_TransferX_Move_ReadyPos(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer X 축, 대기 위치로 이동 시작");

            unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Transfer_To_R_Port");

            //  Target Position 변경 : Stacker0 위치
            unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_X] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_TR_SafetyPos].LD_Transfer_X;

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_X].Common_Speed_Coarse;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_X].Common_Acceleration_Coarse;

            MC_Func.MC_MovePosition((int)nAxis.TR_X,
                                unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_X],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);
            TickCount_Start((int)TickType.TICK_ULTR);
        }

        private void Unloader_Transfer_Step_TransferZ_Move_ReadyPos(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "UL Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 시작");

            unloaderParameter.stUnloaderPosParam = unloaderParameter.GetPositionInformation("Transfer_To_R_Port");

            //  Target Position 변경 : 대기 위치
            unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z] = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_TR_SafetyPos].LD_Transfer_Z;

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Coarse;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Coarse;

            MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.TR_Z],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);
            TickCount_Start((int)TickType.TICK_ULTR);
        }

        #endregion

        #region Event Handler


        //Timer_UnloaderWork_Tick
        public bool m_UnloaderWork_Start = false;
        public bool _isUnloaderWorkRunning = false; // 중복 실행 방지 플래그
        private bool m_IsModuleClose = false;
        private int m_nStacker0_ModulePutdownWaitingPos_Step_Recovery;
        private int m_nStacker1_ModulePutdownWaitingPos_Step_Recovery;
        private int m_nUnloader_Transfer_Step_Recovery;

        private void Timer_UnloaderWork_Tick(object sender, ElapsedEventArgs e)
        {
            // 중복 실행 방지
            if (_isUnloaderWorkRunning)
            {
                //Console.WriteLine("Scanner Calibration is already running. Skipping this call.");
                return;
            }

            try
            {
                _isUnloaderWorkRunning = true;

                // Scanner Calibration이 활성화되지 않은 경우 종료
                if (!m_UnloaderWork_Start)
                {
                    //Console.WriteLine("UnloadTransfer is not started.");
                    return;
                }

                //  Work Stage 에서 Module 을 Pick Up 완료
                Equipment.m_bMainProcessStatus_UL_Module_WorkStagePickUp_Complete = m_bAUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete;

                //  Port 에 Module 을 Put Down 완료
                Equipment.m_bMainProcessStatus_UL_Module_PortPutDown_Complete = !m_bAUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete;

                //  메인 화면 갱신용 변수
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                
                int ret = Run_Stacker0Module_PutdownWaitingPos_Func();
                if (ret != 0)
                {
                    m_nStacker0_ModulePutdownWaitingPos_Step_Recovery = SetRecoveryStaker0(m_nStacker0_ModulePutdownWaitingPos_Step);
                }

                ret = Run_Stacker1Module_PutdownWaitingPos_Func();
                if (ret != 0)
                {
                    m_nStacker1_ModulePutdownWaitingPos_Step_Recovery = SetRecoveryStaker1(m_nStacker1_ModulePutdownWaitingPos_Step);
                }

                ret = Run_Transfer_Cycle_Func();
                if (ret != 0)
                {
                    m_nUnloader_Transfer_Step_Recovery = SetRecoveryTransfer(m_nUnloader_Transfer_Step);
                }
                
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            finally
            {
                _isUnloaderWorkRunning = false; // 플래그 해제
            }
        }

        private int SetRecoveryTransfer(int step)
        {
            if(step <= (int)Unloader_Transfer_Step.None)
            {
                m_nUnloader_Transfer_Step_Recovery = (int)Unloader_Transfer_Step.None;
            }
            else if (step <= (int)Unloader_Transfer_Step.Transfer_Move_Condition_Check)
            {
                m_nUnloader_Transfer_Step_Recovery = (int)Unloader_Transfer_Step.Start;
            }
            else if (step <= (int)Unloader_Transfer_Step.Transfer_Move_Condition_Check)
            {
                m_nUnloader_Transfer_Step_Recovery = (int)Unloader_Transfer_Step.Transfer_Move_Condition_Check;
            }
            else if (step <= (int)Unloader_Transfer_Step.TransferZ_Move_ReadyPos_DoneCheck)
            {
                m_nUnloader_Transfer_Step_Recovery = (int)Unloader_Transfer_Step.Transfer_Move_Condition_Check;
            }
            else if (step <= (int)Unloader_Transfer_Step.TransferX_Move_ReadyPos_DoneCheck)
            {
                m_nUnloader_Transfer_Step_Recovery = (int)Unloader_Transfer_Step.TransferX_Move_ReadyPos;
            }
            else if (step <= (int)Unloader_Transfer_Step.WorkStage_ModulePickup_Condition_Check)
            {
                m_nUnloader_Transfer_Step_Recovery = (int)Unloader_Transfer_Step.WorkStage_ModulePickup_Condition_Check;
            }
            else if (step <= (int)Unloader_Transfer_Step.WorkStagePickUp_TransferZ_Move_ReadyPos_DoneCheck)
            {
                m_nUnloader_Transfer_Step_Recovery = (int)Unloader_Transfer_Step.WorkStagePickUp_TransferZ_Move_ReadyPos;
            }
            else if (step <= (int)Unloader_Transfer_Step.WorkStagePickUp_WorkStageCycle_UnloadingPos_CompleteCheck)
            {
                m_nUnloader_Transfer_Step_Recovery = (int)Unloader_Transfer_Step.WorkStagePickUp_WorkStageCycle_UnloadingPos_Start;
            }
            else if (step <= (int)Unloader_Transfer_Step.WorkStagePickUp_TransferX_Move_WorkStagePos_DoneCheck)
            {
                m_nUnloader_Transfer_Step_Recovery = (int)Unloader_Transfer_Step.WorkStagePickUp_TransferX_Move_WorkStagePos;
            }
            else if (step <= (int)Unloader_Transfer_Step.WorkStagePickUp_TransferZ_Move_PickUpPos_1stStep_DoneCheck)
            {
                m_nUnloader_Transfer_Step_Recovery = (int)Unloader_Transfer_Step.WorkStagePickUp_TransferZ_Move_PickUpPos_1stStep;
            }
            else if (step <= (int)Unloader_Transfer_Step.WorkStagePickUp_TransferZ_Move_PickUpPos_2ndStep_DoneCheck)
            {
                m_nUnloader_Transfer_Step_Recovery = (int)Unloader_Transfer_Step.WorkStagePickUp_TransferZ_Move_PickUpPos_2ndStep;
            }
            else if (step <= (int)Unloader_Transfer_Step.WorkStagePickUp_Transfer_PickerVacuum_OnCheck)
            {
                m_nUnloader_Transfer_Step_Recovery = (int)Unloader_Transfer_Step.WorkStagePickUp_Transfer_PickerVacuum_On;
            }

            else if (step <= (int)Unloader_Transfer_Step.WorkStagePickUp_TransferZ_Move_ReadyPos2_1stStep_DoneCheck)
            {
                m_nUnloader_Transfer_Step_Recovery = (int)Unloader_Transfer_Step.WorkStagePickUp_TransferZ_Move_ReadyPos2_1stStep;
            }
            else if (step <= (int)Unloader_Transfer_Step.WorkStagePickUp_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck)
            {
                m_nUnloader_Transfer_Step_Recovery = (int)Unloader_Transfer_Step.WorkStagePickUp_TransferZ_Move_ReadyPos2_2ndStep;
            }
            else if (step <= (int)Unloader_Transfer_Step.Stacker0_ModulePutdown_Condition_Check)
            {
                m_nUnloader_Transfer_Step_Recovery = (int)Unloader_Transfer_Step.Stacker0_ModulePutdown_Condition_Check;
            }

            else if (step <= (int)Unloader_Transfer_Step.Stacker0PutDown_TransferZ_Move_ReadyPos_DoneCheck)
            {
                m_nUnloader_Transfer_Step_Recovery = (int)Unloader_Transfer_Step.Stacker0PutDown_TransferZ_Move_ReadyPos;
            }
            else if (step <= (int)Unloader_Transfer_Step.Stacker0PutDown_TransferX_Move_StackerPos_DoneCheck)
            {
                m_nUnloader_Transfer_Step_Recovery = (int)Unloader_Transfer_Step.Stacker0PutDown_TransferX_Move_StackerPos;
            }
            else if (step <= (int)Unloader_Transfer_Step.Stacker0PutDown_TransferZ_Move_PutDownPos_1stStep_DoneCheck)
            {
                m_nUnloader_Transfer_Step_Recovery = (int)Unloader_Transfer_Step.Stacker0PutDown_TransferZ_Move_PutDownPos_1stStep;
            }
            else if (step <= (int)Unloader_Transfer_Step.Stacker0PutDown_TransferZ_Move_PutDownPos_2ndStep_DoneCheck)
            {
                m_nUnloader_Transfer_Step_Recovery = (int)Unloader_Transfer_Step.Stacker0PutDown_TransferZ_Move_PutDownPos_2ndStep;
            }

            else if (step <= (int)Unloader_Transfer_Step.Stacker0PutDown_Transfer_PickerVacuum_OffCheck)
            {
                m_nUnloader_Transfer_Step_Recovery = (int)Unloader_Transfer_Step.Stacker0PutDown_Transfer_PickerVacuum_Off;
            }


            else if (step <= (int)Unloader_Transfer_Step.Stacker0PutDown_TransferZ_Move_ReadyPos2_1stStep_DoneCheck)
            {
                m_nUnloader_Transfer_Step_Recovery = (int)Unloader_Transfer_Step.Stacker0PutDown_TransferZ_Move_ReadyPos2_1stStep;
            }

            else if (step <= (int)Unloader_Transfer_Step.Stacker0PutDown_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck)
            {
                m_nUnloader_Transfer_Step_Recovery = (int)Unloader_Transfer_Step.Stacker0PutDown_TransferZ_Move_ReadyPos2_2ndStep;
            }
            else if (step <= (int)Unloader_Transfer_Step.Stacker1_ModulePutdown_Condition_Check)
            {
                m_nUnloader_Transfer_Step_Recovery = (int)Unloader_Transfer_Step.Stacker1_ModulePutdown_Condition_Check;
            }

            else if (step <= (int)Unloader_Transfer_Step.Stacker1PutDown_TransferZ_Move_ReadyPos_DoneCheck)
            {
                m_nUnloader_Transfer_Step_Recovery = (int)Unloader_Transfer_Step.Stacker1PutDown_TransferZ_Move_ReadyPos;
            }
            else if (step <= (int)Unloader_Transfer_Step.Stacker1PutDown_TransferX_Move_StackerPos_DoneCheck)
            {
                m_nUnloader_Transfer_Step_Recovery = (int)Unloader_Transfer_Step.Stacker1PutDown_TransferX_Move_StackerPos;
            }

            else if (step <= (int)Unloader_Transfer_Step.Stacker1PutDown_TransferZ_Move_PutDownPos_1stStep_DoneCheck)
            {
                m_nUnloader_Transfer_Step_Recovery = (int)Unloader_Transfer_Step.Stacker1PutDown_TransferZ_Move_PutDownPos_1stStep;
            }

            else if (step <= (int)Unloader_Transfer_Step.Stacker1PutDown_TransferZ_Move_PutDownPos_2ndStep_DoneCheck)
            {
                m_nUnloader_Transfer_Step_Recovery = (int)Unloader_Transfer_Step.Stacker1PutDown_TransferZ_Move_PutDownPos_2ndStep;
            }
            else if (step <= (int)Unloader_Transfer_Step.Stacker1PutDown_Transfer_PickerVacuum_OffCheck)
            {
                m_nUnloader_Transfer_Step_Recovery = (int)Unloader_Transfer_Step.Stacker1PutDown_Transfer_PickerVacuum_Off;
            }

            else if (step <= (int)Unloader_Transfer_Step.Stacker1PutDown_TransferZ_Move_ReadyPos2_1stStep_DoneCheck)
            {
                m_nUnloader_Transfer_Step_Recovery = (int)Unloader_Transfer_Step.Stacker1PutDown_TransferZ_Move_ReadyPos2_1stStep;
            }

            else if (step <= (int)Unloader_Transfer_Step.Stacker1PutDown_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck)
            {
                m_nUnloader_Transfer_Step_Recovery = (int)Unloader_Transfer_Step.Stacker1PutDown_TransferZ_Move_ReadyPos2_2ndStep;
            }

            else if (step <= (int)Unloader_Transfer_Step.NgStacker_ModuleDrop_Condition_Check)
            {
                m_nUnloader_Transfer_Step_Recovery = (int)Unloader_Transfer_Step.NgStacker_ModuleDrop_Condition_Check;
            }

            else if (step <= (int)Unloader_Transfer_Step.NgStackerDrop_TransferZ_Move_ReadyPos_DoneCheck)
            {
                m_nUnloader_Transfer_Step_Recovery = (int)Unloader_Transfer_Step.NgStackerDrop_TransferZ_Move_ReadyPos;
            }

            else if (step <= (int)Unloader_Transfer_Step.NgStackerDrop_TransferX_Move_NgStackerPos_DoneCheck)
            {
                m_nUnloader_Transfer_Step_Recovery = (int)Unloader_Transfer_Step.NgStackerDrop_TransferX_Move_NgStackerPos;
            }


            else if (step <= (int)Unloader_Transfer_Step.NgStackerDrop_TransferZ_Move_DropPos_DoneCheck)
            {
                m_nUnloader_Transfer_Step_Recovery = (int)Unloader_Transfer_Step.NgStackerDrop_TransferZ_Move_DropPos;
            }
            else if (step <= (int)Unloader_Transfer_Step.NgStackerDrop_Transfer_PickerVacuum_OffCheck)
            {
                m_nUnloader_Transfer_Step_Recovery = (int)Unloader_Transfer_Step.NgStackerDrop_Transfer_PickerVacuum_Off;
            }

            else if (step <= (int)Unloader_Transfer_Step.NgStackerDrop_TransferZ_Move_ReadyPos2_DoneCheck)
            {
                m_nUnloader_Transfer_Step_Recovery = (int)Unloader_Transfer_Step.NgStackerDrop_TransferZ_Move_ReadyPos2;
            }
            return m_nUnloader_Transfer_Step_Recovery;
        }

        private int SetRecoveryStaker0(int step)
        {

            m_nStacker0_ModulePutdownWaitingPos_Step_Recovery = (int)StackerModulePutdownWaitingPos_Step.Start;
            return m_nStacker0_ModulePutdownWaitingPos_Step_Recovery;
        }
        private int SetRecoveryStaker1(int step)
        {
            m_nStacker1_ModulePutdownWaitingPos_Step_Recovery = (int)StackerModulePutdownWaitingPos_Step.Start;
            return m_nStacker1_ModulePutdownWaitingPos_Step_Recovery;


        }
        private void Timer_UnloaderWork_Func(object sender, EventArgs e)
        {
            //  동시에 진행되지 않는 함수들만 동일한 타이머로 한다.

            //m_btimer_UnloaderWork_Stop = false;
            timer_UnloaderWork.Enabled = false;


            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //  메인 화면 갱신용 변수

            //  Work Stage 에서 Module 을 Pick Up 완료
            Equipment.m_bMainProcessStatus_UL_Module_WorkStagePickUp_Complete = m_bAUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete;

            //  Port 에 Module 을 Put Down 완료
            Equipment.m_bMainProcessStatus_UL_Module_PortPutDown_Complete = !m_bAUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete;

            //  메인 화면 갱신용 변수
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            Run_Stacker0Module_PutdownWaitingPos_Func();
            Run_Stacker1Module_PutdownWaitingPos_Func();

            Run_Transfer_Cycle_Func();

            //if (!m_btimer_LoaderWork_Stop)
            {
                timer_UnloaderWork.Enabled = true;
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

            foreach (ZzxzPositionData position in Config.Positions)
            {
                if (position.Type == TargetType.Base)
                    ret.Add(position.Name);
            }
            return ret;
        }

        #endregion


        //        public XytCoordinate GetCurrentPosition()
        //        {
        //            XytCoordinate current = new XytCoordinate();
        //            if (Stage != null)
        //            {
        //                current.X = Stage.Axes["X"].Motor.ActualPosition;
        //                current.Y = Stage.Axes["Y"].Motor.ActualPosition;
        //                current.T = Stage.Axes["T"].Motor.ActualPosition;
        //            }
        //
        //            return current;
        //        }


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

            //if (File.Exists(strFIle))
            //{
            //    m_bRet = true;

                                
            //    //  구동 제한 - Wafer Align 시, Elevator Z 축이 올라갈 수 있는 최대 높이 위치
            //    NativeMethods.GetPrivateProfileString("Drive_Limit", "Elev_Z", "58.0", temp, 255, strFIle);
            //    Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign = Equipment.ToDouble(temp.ToString());

            //    //  구동 제한 - Vision Y 축이, Elevator Z 축과 충돌하지 않는 최대 위치
            //    NativeMethods.GetPrivateProfileString("Drive_Limit", "Vision_Y", "20.0", temp, 255, strFIle);
            //    Config.ParamConfig.DriveLimit_VisionY_NotConflictWithElevZ = Equipment.ToDouble(temp.ToString());

            //    //  구동 제한 - UVW 스테이지, U 축 얼라인 제한 위치 (마이너스 방향)
            //    NativeMethods.GetPrivateProfileString("Align_Limit", "UVW_U_Minus", "-1.915", temp, 255, strFIle);
            //    Config.ParamConfig.AlignLimit_UVW_U_Minus = Equipment.ToDouble(temp.ToString());

            //    //  구동 제한 - UVW 스테이지, U 축 얼라인 제한 위치 (플러스 방향)
            //    NativeMethods.GetPrivateProfileString("Align_Limit", "UVW_U_Plus", "0.085", temp, 255, strFIle);
            //    Config.ParamConfig.AlignLimit_UVW_U_Plus = Equipment.ToDouble(temp.ToString());

            //    //  구동 제한 - UVW 스테이지, V 축 얼라인 제한 위치 (마이너스 방향)
            //    NativeMethods.GetPrivateProfileString("Align_Limit", "UVW_V_Minus", "-0.805", temp, 255, strFIle);
            //    Config.ParamConfig.AlignLimit_UVW_V_Minus = Equipment.ToDouble(temp.ToString());

            //    //  구동 제한 - UVW 스테이지, V 축 얼라인 제한 위치 (플러스 방향)
            //    NativeMethods.GetPrivateProfileString("Align_Limit", "UVW_V_Plus", "1.195", temp, 255, strFIle);
            //    Config.ParamConfig.AlignLimit_UVW_V_Plus = Equipment.ToDouble(temp.ToString());

            //    //  구동 제한 - UVW 스테이지, W 축 얼라인 제한 위치 (마이너스 방향)
            //    NativeMethods.GetPrivateProfileString("Align_Limit", "UVW_W_Minus", "-0.075", temp, 255, strFIle);
            //    Config.ParamConfig.AlignLimit_UVW_W_Minus = Equipment.ToDouble(temp.ToString());

            //    //  구동 제한 - UVW 스테이지, W 축 얼라인 제한 위치 (플러스 방향)
            //    NativeMethods.GetPrivateProfileString("Align_Limit", "UVW_W_Plus", "1.925", temp, 255, strFIle);
            //    Config.ParamConfig.AlignLimit_UVW_W_Plus = Equipment.ToDouble(temp.ToString());

            //    //  비전 스케일 - Manual Scale Usage
            //    NativeMethods.GetPrivateProfileString("Vision_Scale", "Manual_Scale_Use", "True", temp, 255, strFIle);
            //    Config.ParamConfig.ManualScale_Usage = temp.ToString() == "False" ? false : true;

            //    //  비전 스케일 - Lower Vision Scale X (mm)
            //    NativeMethods.GetPrivateProfileString("Vision_Scale", "Lower_Scale_X", "0.001726468", temp, 255, strFIle);
            //    Config.ParamConfig.LowerVision_Scale_X = Equipment.ToDouble(temp.ToString());

            //    //  비전 스케일 - Lower Vision Scale Y (mm)
            //    NativeMethods.GetPrivateProfileString("Vision_Scale", "Lower_Scale_Y", "0.001726468", temp, 255, strFIle);
            //    Config.ParamConfig.LowerVision_Scale_Y = Equipment.ToDouble(temp.ToString());

            //    //  비전 스케일 - Lower Vision Scale Invert X
            //    NativeMethods.GetPrivateProfileString("Vision_Scale", "Lower_Scale_X_Invert", "False", temp, 255, strFIle);
            //    Config.ParamConfig.LowerVision_ScaleInvert_X = temp.ToString() == "False" ? false : true;

            //    //  비전 스케일 - Lower Vision Scale Invert Y
            //    NativeMethods.GetPrivateProfileString("Vision_Scale", "Lower_Scale_Y_Invert", "False", temp, 255, strFIle);
            //    Config.ParamConfig.LowerVision_ScaleInvert_Y = temp.ToString() == "False" ? false : true;

            //    //  비전 스케일 - Upper Vision Scale X (mm)
            //    NativeMethods.GetPrivateProfileString("Vision_Scale", "Upper_Scale_X", "0.001726468", temp, 255, strFIle);
            //    Config.ParamConfig.UpperVision_Scale_X = Equipment.ToDouble(temp.ToString());

            //    //  비전 스케일 - Upper Vision Scale Y (mm)
            //    NativeMethods.GetPrivateProfileString("Vision_Scale", "Upper_Scale_Y", "0.001726468", temp, 255, strFIle);
            //    Config.ParamConfig.UpperVision_Scale_Y = Equipment.ToDouble(temp.ToString());

            //    //  비전 스케일 - Upper Vision Scale Invert X
            //    NativeMethods.GetPrivateProfileString("Vision_Scale", "Upper_Scale_X_Invert", "False", temp, 255, strFIle);
            //    Config.ParamConfig.UpperVision_ScaleInvert_X = temp.ToString() == "False" ? false : true;

            //    //  비전 스케일 - Upper Vision Scale Invert Y
            //    NativeMethods.GetPrivateProfileString("Vision_Scale", "Upper_Scale_Y_Invert", "False", temp, 255, strFIle);
            //    Config.ParamConfig.UpperVision_ScaleInvert_Y = temp.ToString() == "False" ? false : true;

            //    //  레티클 글래스 - 얼라인 조명 밝기값 (상부 카메라)
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass", "Upper_Vision_LightValue", "602", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleAlign_UpperVision_LightValue = Equipment.ToInt(temp.ToString());

            //    //  레티클 글래스 - 얼라인 조명 밝기값 (하부 카메라)
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass", "Lower_Vision_LightValue", "964", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleAlign_LowerVision_LightValue = Equipment.ToInt(temp.ToString());

            //    //  얼라인 - 얼라인 시 Theta 축 회전 속도 (mm/s)
            //    NativeMethods.GetPrivateProfileString("Vision_Align", "Theta_Rotation_Speed", "5.0", temp, 255, strFIle);
            //    Config.ParamConfig.Align_Theta_Velocity = Equipment.ToDouble(temp.ToString());

            //    //  얼라인 - 얼라인 시 Theta 축 회전 가속도 (mm/s²)
            //    NativeMethods.GetPrivateProfileString("Vision_Align", "Theta_Rotation_Accel", "100.0", temp, 255, strFIle);
            //    Config.ParamConfig.Align_Theta_Accel = Equipment.ToDouble(temp.ToString());

            //    //  얼라인 - 얼라인 시 Theta 축 회전 감속도 (mm/s²)
            //    NativeMethods.GetPrivateProfileString("Vision_Align", "Theta_Rotation_Decel", "100.0", temp, 255, strFIle);
            //    Config.ParamConfig.Align_Theta_Decel = Equipment.ToDouble(temp.ToString());

            //    //  얼라인 - 얼라인 재시도 회수
            //    NativeMethods.GetPrivateProfileString("Vision_Align", "Align_Retry", "10", temp, 255, strFIle);
            //    Config.ParamConfig.Align_Retries = Equipment.ToInt(temp.ToString());

            //    //  얼라인 - 얼라인 각도 Invert
            //    NativeMethods.GetPrivateProfileString("Vision_Scale", "Align_Angle_Invert", "True", temp, 255, strFIle);
            //    Config.ParamConfig.Align_AngleInvert = temp.ToString() == "False" ? false : true;

            //    //  얼라인 - 얼라인 각도 계산 시 Atan 함수 사용
            //    NativeMethods.GetPrivateProfileString("Vision_Scale", "Align_Calc_AtanFunc", "True", temp, 255, strFIle);
            //    Config.ParamConfig.Align_ThetaCalcFunction_Atan = temp.ToString() == "False" ? false : true;

            //    //  얼라인 스테이지 - Theta 회전 반경 (mm)
            //    NativeMethods.GetPrivateProfileString("Align_Stage", "Turning_Radius", "70.0", temp, 255, strFIle);
            //    Config.ParamConfig.Align_Theta_From_RotCenter_To_UVW_Distance = Equipment.ToDouble(temp.ToString());

            //    //  얼라인 스테이지 - Theta 1˚ 회전을 위한 UVW 각 축 이동량 (mm)
            //    NativeMethods.GetPrivateProfileString("Align_Stage", "Movement_Amount_1Deg_Rotation", "1.221668451", temp, 255, strFIle);
            //    Config.ParamConfig.Align_Theta_Movement_MM_Per_1Deg = Equipment.ToDouble(temp.ToString());

            //    //  얼라인 이미지 저장 여부
            //    NativeMethods.GetPrivateProfileString("Align_Image", "Image_Save_Use", "True", temp, 255, strFIle);
            //    Config.ParamConfig.AlignImageSave_Usage = temp.ToString() == "False" ? false : true;

            //    //  얼라인 이미지 저장 위치 용량 부족 경고 기준치 (GB)
            //    NativeMethods.GetPrivateProfileString("Align_Image", "Image_Save_DriveSpace_Warning_Value", "10", temp, 255, strFIle);
            //    Config.ParamConfig.AlignImageSaveFolder_WarningSpace = Equipment.ToDouble(temp.ToString());

            //    //  프로브 카드 클램프 타입 1 일 경우, 업다운 실린더 동작 대기 시간
            //    NativeMethods.GetPrivateProfileString("Machine_Type", "ProbeCard_ClampTypeB_CylUpDown_StableTime", "1000", temp, 255, strFIle);
            //    Config.ParamConfig.ProbeCard_ClampTypeB_CylUpDown_StableTime = Equipment.ToInt(temp.ToString());

            //    //  레티클 글래스 - 레시피 변경 시, 레티클 글래스 센터를 확인해야 작업 진행 가능
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass", "ReticleGlass_CenterCheck_forAlign", "False", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleGlass_CenterCheck_forAlign = temp.ToString() == "False" ? false : true;

            //    //  레티클 글래스 - 레티클 글래스를 확인 시 비전 카메라와 엘리베이터의 충돌 방지를 위한 Vision Y 축 이동 한계 위치. (mm, 대부분 90.0 이내)
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass", "ReticleGlass_Vision_Y_Limit", "90", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleGlass_Vision_Y_Limit = Equipment.ToDouble(temp.ToString());

            //    //  PAK, 웨이퍼 Gate - Center 가 일치할 때의 Offset X
            //    NativeMethods.GetPrivateProfileString("PAK_Gate_Center", "PAK_WaferGate_Centering_Offset_X", "0", temp, 255, strFIle);
            //    Config.ParamConfig.PAK_WaferGate_Centering_Offset_X = Equipment.ToDouble(temp.ToString());

            //    //  PAK, 웨이퍼 Gate - Center 가 일치할 때의 Offset Y
            //    NativeMethods.GetPrivateProfileString("PAK_Gate_Center", "PAK_WaferGate_Centering_Offset_Y", "0", temp, 255, strFIle);
            //    Config.ParamConfig.PAK_WaferGate_Centering_Offset_Y = Equipment.ToDouble(temp.ToString());

            //    //  자동 로그아웃 기능 사용 여부
            //    NativeMethods.GetPrivateProfileString("Auto_LogOut", "Auto_LogOut_Usage", "False", temp, 255, strFIle);
            //    Config.ParamConfig.Auto_LogOut_Usage = temp.ToString() == "False" ? false : true;

            //    //  자동 로그아웃 설정 시간
            //    NativeMethods.GetPrivateProfileString("Auto_LogOut", "Auto_LogOut_Time", "10", temp, 255, strFIle);
            //    Config.ParamConfig.Auto_LogOut_Time = Equipment.ToDouble(temp.ToString());

            //    //  얼라인 작업 시작 시 & 패킹 작업 완료시 Leak Check 기능 사용 여부
            //    NativeMethods.GetPrivateProfileString("Packing_LeakCheck", "AlignPacking_LeakCheck_Usage", "False", temp, 255, strFIle);
            //    Config.ParamConfig.AlignPacking_OP_LeakCheck_Usage = temp.ToString() == "False" ? false : true;

            //    //  패킹 작업 완료시 Leak Check 설정 시간 (sec)
            //    NativeMethods.GetPrivateProfileString("Packing_LeakCheck", "Packing_LeakCheck_Time", "30", temp, 255, strFIle);
            //    Config.ParamConfig.Packing_OP_LeakCheck_Time = Equipment.ToDouble(temp.ToString());

            //    //  패킹 작업 완료 후, 제품 언로딩을 위해 엘리베이터를 내리는 거리 (mm)
            //    NativeMethods.GetPrivateProfileString("ElevatorZ", "DownDistance_AfterPacking", "20", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_ProbeCard_Down_Distance_After_Packing = Equipment.ToDouble(temp.ToString());

            //    //  전면 안전센서 사용 여부
            //    NativeMethods.GetPrivateProfileString("Interlock", "Area_Sensor_Usage", "False", temp, 255, strFIle);
            //    Config.ParamConfig.AreaSensor_Usage = temp.ToString() == "False" ? false : true;

            //    //  전면 안전센서 감지 시 Servo Off 여부
            //    NativeMethods.GetPrivateProfileString("Interlock", "AreaSensor_ServoOff_Usage", "True", temp, 255, strFIle);
            //    Config.ParamConfig.AreaSensor_ServoOff_Usage = temp.ToString() == "False" ? false : true;

            //    ////  전면 안전센서 해제 후 다시 동작시키기 위해 대기하는 시간 (sec)
            //    //NativeMethods.GetPrivateProfileString("Interlock", "Area_Sensor_Release_Pause_Time", "3", temp, 255, strFIle);
            //    //Config.ParamConfig.AreaSensor_Off_Pause_Time = Equipment.ToDouble(temp.ToString());

            //    //  수동패킹 모드 사용 권한 설정
            //    NativeMethods.GetPrivateProfileString("Manual_Packing", "Enable_Admin_Only", "True", temp, 255, strFIle);
            //    Config.ParamConfig.ManualPacking_Only_Admin = temp.ToString() == "False" ? false : true;



            //    //  사용 옵션과 안정화 시간

            //    //  사용 옵션 - 웨이퍼 얼라인 시작 전 PAK 공압 라인 막힘 검사 사용 여부
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "Pak_AirLineCheck_Usage", "False", temp, 255, strFIle);
            //    Config.ParamConfig.Pak_AirLineCheck_Usage = temp.ToString() == "False" ? false : true;

            //    //  사용 옵션 - 웨이퍼 얼라인 시작 전 PAK 공압 라인 막힘 검사 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "Pak_AirLineCheck_Time", "2000", temp, 255, strFIle);
            //    Config.ParamConfig.Pak_AirLineCheck_Time = Equipment.ToInt(temp.ToString());

            //    //  사용 옵션 - Packing 공압 신호 사용 여부
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "Packing_VacuumSignal_Usage", "False", temp, 255, strFIle);
            //    Config.ParamConfig.Packing_VacuumSignal_Usage = temp.ToString() == "False" ? false : true;

            //    //  사용 옵션 - Packing 공압 신호를 사용하지 않을 경우, 대기 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "Packing_VacuumSignal_Time", "5000", temp, 255, strFIle);
            //    Config.ParamConfig.Packing_VacuumSignal_Time = Equipment.ToInt(temp.ToString());

            //    //  사용 옵션 - Packing 공압 신호를 사용할 경우, 추가 가압 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "Packing_VacuumSignal_AfterTime", "5000", temp, 255, strFIle);
            //    Config.ParamConfig.Packing_VacuumSignal_AfterTime = Equipment.ToInt(temp.ToString());

            //    //  사용 옵션 - Wafer 공압 체크 사용 여부
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "Wafer_VacuumSignal_Usage", "False", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_VacuumSignal_Usage = temp.ToString() == "False" ? false : true;

            //    //  사용 옵션 - Wafer 공압을 사용하지 않을 경우, 대기 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "Wafer_VacuumSignal_Time", "1000", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_VacuumSignal_Time = Equipment.ToInt(temp.ToString());

            //    //  사용 옵션 - Thin-Chuck 감지 센서 사용 여부
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "ThinChuck_DetectSignal_Usage", "True", temp, 255, strFIle);
            //    Config.ParamConfig.ThinChuck_DetectSignal_Usage = temp.ToString() == "False" ? false : true;

            //    //  사용 옵션 - Thin-Chuck 공압 체크 사용 여부
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "ThinChuck_VacuumSignal_Usage", "True", temp, 255, strFIle);
            //    Config.ParamConfig.ThinChuck_VacuumSignal_Usage = temp.ToString() == "False" ? false : true;

            //    //  사용 옵션 - Thin-Chuck 공압을 사용하지 않을 경우, 대기 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "ThinChuck_VacuumSignal_Time", "1000", temp, 255, strFIle);
            //    Config.ParamConfig.ThinChuck_VacuumSignal_Time = Equipment.ToInt(temp.ToString());

            //    //  사용 옵션 - Wafer 얼라인 동작 중, 비전 사용 여부. (false : 사용 안함)
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "Wafer_Align_Cam_Usage", "True", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_Align_Cam_Usage = temp.ToString() == "False" ? false : true;

            //    //  사용 옵션 - Wafer 얼라인 후 Packing 작업 자동 시작 여부. (false : 얼라인 완료 후 Packing 작업 대기)
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "Packing_AutoStart_After_Wafer_Align_Usage", "False", temp, 255, strFIle);
            //    Config.ParamConfig.Packing_AutoStart_After_Wafer_Align_Usage = temp.ToString() == "False" ? false : true;

            //    //  사용 옵션 - Wafer 얼라인 후 얼라인 위치 정확성 검증 여부. (false : 사용 안함) 
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "Wafer_Align_ErrorCheck_After_Wafer_Align_Usage", "True", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_Align_ErrorCheck_After_Wafer_Align_Usage = temp.ToString() == "False" ? false : true;

            //    //  사용 옵션 - 실내 조명을 상시 On 상태로 할 것인지 여부. (false : Align 시 Off 되고, 자재를 Loading 할 때 On)
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "Indoor_Light_AlwaysOn_Usage", "False", temp, 255, strFIle);
            //    Config.ParamConfig.Indoor_Light_AlwaysOn_Usage = temp.ToString() == "False" ? false : true;



            //    //  안정화 시간 - Packing 공압 신호 On 후 대기 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Stable_Time", "StableTime_after_PackingSignal_On", "4000", temp, 255, strFIle);
            //    Config.ParamConfig.StableTime_after_PackingSignal_On = Equipment.ToInt(temp.ToString());

            //    //  안정화 시간 - Packing 공압 신호 On 후 Thin-Chuck 공압을 해제하기 위해 대기하는 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Stable_Time", "StableTime_after_PackingSignal_On_before_ThinChuck_Vacuum_Off", "100", temp, 255, strFIle);
            //    Config.ParamConfig.StableTime_after_PackingSignal_On_before_ThinChuck_Vacuum_Off = Equipment.ToInt(temp.ToString());

            //    //  안정화 시간 - Packing 공압 신호 On 후 Wafer 공압을 해제하기 위해 대기하는 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Stable_Time", "StableTime_after_PackingSignal_On_before_Wafer_Vacuum_Off", "0", temp, 255, strFIle);
            //    Config.ParamConfig.StableTime_after_PackingSignal_On_before_Wafer_Vacuum_Off = Equipment.ToInt(temp.ToString());

            //    //  안정화 시간 - Packing 작업 중, Wafer 공압 해제 후 안정화(대기) 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Stable_Time", "StableTime_after_WaferVacuumSignal_Off", "300", temp, 255, strFIle);
            //    Config.ParamConfig.StableTime_after_WaferVacuumSignal_Off = Equipment.ToInt(temp.ToString());

            //    //  안정화 시간 - Packing 을 위해 Elev. Z 축이 프로브 카드 위치까지 이동한 후 안정화 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Stable_Time", "StableTime_before_PackingSignal_On", "100", temp, 255, strFIle);
            //    Config.ParamConfig.StableTime_before_PackingSignal_On = Equipment.ToInt(temp.ToString());

            //    //  안정화 시간 - Packing 작업 중, Thin-Chuck 공압 해제 후 안정화(대기) 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Stable_Time", "StableTime_after_ThinChuckVacuumSignal_Off", "300", temp, 255, strFIle);
            //    Config.ParamConfig.StableTime_after_ThinChuckVacuumSignal_Off = Equipment.ToInt(temp.ToString());

            //    //  안정화 시간 - Wafer 얼라인 시, 마크 위치 이동 후 안정화 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Stable_Time", "WaferAlign_Move_StableTime", "500", temp, 255, strFIle);
            //    Config.ParamConfig.WaferAlign_Move_StableTime = Equipment.ToInt(temp.ToString());

            //    //  안정화 시간 - Unpacking 작업 중, Unpacking 공압 신호 On 후 Elev. Z 축을 내리기 시작할 때까지 대기 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Stable_Time", "StableTime_after_UnpackingSignal_On", "100", temp, 255, strFIle);
            //    Config.ParamConfig.StableTime_after_UnpackingSignal_On = Equipment.ToInt(temp.ToString());



            //    //  Offset && Delay - 웨이퍼 && 프로브카드 Packing 시, 저속 이동 거리 (mm)
            //    NativeMethods.GetPrivateProfileString("Offset_Delay", "Wafer_ProbeCard_PackingOffset_Distance", "10", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_ProbeCard_PackingOffset_Distance = Equipment.ToDouble(temp.ToString());

            //    //  Offset && Delay - 웨이퍼 && 프로브카드 Unpacking 시, 저속 이동 거리 (mm)
            //    NativeMethods.GetPrivateProfileString("Offset_Delay", "Wafer_ProbeCard_UnpackingOffset_Distance", "10", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_ProbeCard_UnpackingOffset_Distance = Equipment.ToDouble(temp.ToString());

            //    //  Offset && Delay - 웨이퍼 && 프로브카드 UnPacking 시, UnPacking 신호 인가 후 이동하는 거리 (mm)
            //    NativeMethods.GetPrivateProfileString("Offset_Delay", "Wafer_ProbeCard_UnPackingOffset_Distance", "5", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_ProbeCard_UnPackingOffset_Distance = Equipment.ToDouble(temp.ToString());

            //    //  Offset && Delay - 웨이퍼 && 프로브카드 UnPacking 시, UnPacking 을 위한 Elevator Z 축 이동 거리 (기준 높이 : Packing 위치) (mm)
            //    NativeMethods.GetPrivateProfileString("Offset_Delay", "Wafer_ProbeCard_UnPackingStartOffset_Distance", "-1.5", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_ProbeCard_UnPackingStartOffset_Distance = Equipment.ToDouble(temp.ToString());

            //    //  Offset && Delay - 웨이퍼 && 프로브카드 Manual Packing 시, 엘리베이터 Z 축의 1단계 Offset 거리 (mm, > 0)
            //    NativeMethods.GetPrivateProfileString("Offset_Delay", "Wafer_ProbeCard_ManualPacking_ElevZ_Offset_Distance", "30", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_ProbeCard_ManualPacking_ElevZ_Offset_Distance = Equipment.ToDouble(temp.ToString());

            //    //  Offset && Delay - 웨이퍼 && 프로브카드 Manual Packing 시, 엘리베이터 Z 축의 1단계 Offset 이동 방법 (2 Step 이동 or 이동 후 대기)
            //    NativeMethods.GetPrivateProfileString("Offset_Delay", "Wafer_ProbeCard_ManualPacking_ElevZ_OffsetMove_Concept", "False", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_ProbeCard_ManualPacking_1st_Step_ElevZ_OffsetMove_Concept = temp.ToString() == "False" ? false : true;

            //    //  Offset && Delay - 웨이퍼 && 프로브카드 안전 분리 동작 시, 씬-척 낙하 방지를 위해 엘리베이터 Z 축을 올리는 위치. (Packing 위치 대비 Offset 거리 (mm, > 0)
            //    NativeMethods.GetPrivateProfileString("Offset_Delay", "Wafer_ProbeCard_SafelyUnpacking_ElevZ_Offset_Distance", "30", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_ProbeCard_SafelyUnpacking_ElevZ_Offset_Distance = Equipment.ToDouble(temp.ToString());



            //    //  카메라 설정 - 시리얼 넘버 사용 여부 (False : Config 에 세팅된 시리얼 넘버 사용)
            //    NativeMethods.GetPrivateProfileString("Camera_SerialNumber", "Camera_SerialNumber_Type", "False", temp, 255, strFIle);
            //    Config.ParamConfig.Camera_SerialNumber_Type = temp.ToString() == "False" ? false : true;

            //    //  카메라 설정 - PAK 카메라 시리얼 넘버 (Upper Camera)
            //    NativeMethods.GetPrivateProfileString("Camera_SerialNumber", "Camera_SerialNumber_PAK", "", temp, 255, strFIle);
            //    Config.ParamConfig.Camera_SerialNumber_PAK = temp.ToString();

            //    //  카메라 설정 - Wafer 카메라 시리얼 넘버 (Lower Camera)
            //    NativeMethods.GetPrivateProfileString("Camera_SerialNumber", "Camera_SerialNumber_Wafer", "", temp, 255, strFIle);
            //    Config.ParamConfig.Camera_SerialNumber_Wafer = temp.ToString();



            //    //  레티클 글래스 - 레티클 글래스 확인 위치 (VIsion X)                  ## 임의 변경 금지 ##
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass", "ReticleGlass_Vision_X_Pos", "164", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleGlass_Vision_X_Pos = Equipment.ToDouble(temp.ToString());

            //    //  레티클 글래스 - 레티클 글래스 확인 위치 (VIsion Y)                  ## 임의 변경 금지 ##
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass", "ReticleGlass_Vision_Y_Pos", "87", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleGlass_Vision_Y_Pos = Equipment.ToDouble(temp.ToString());

            //    //  레티클 글래스 - 레티클 글래스 확인 위치 (VIsion Z)                  ## 임의 변경 금지 ##
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass", "ReticleGlass_Vision_Z_Pos", "2.73", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleGlass_Vision_Z_Pos = Equipment.ToDouble(temp.ToString());

            //    //  레티클 글래스 - 레티클 글래스 확인 위치 (Elev. Z - PAK 카메라)      ## 임의 변경 금지 ##
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass", "ReticleGlass_Elev_Z_PAK_Pos", "0.0", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleGlass_Elev_Z_PAK_Pos = Equipment.ToDouble(temp.ToString());

            //    //  레티클 글래스 - 레티클 글래스 확인 위치 (Elev. Z - Wafer 카메라)    ## 임의 변경 금지 ##
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass", "ReticleGlass_Elev_Z_Wafer_Pos", "0.0", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleGlass_Elev_Z_Wafer_Pos = Equipment.ToDouble(temp.ToString());

            //    //  레티클 글래스 - 레티클 글래스 Wafer 이미지 Offset X        ## 임의 변경 금지 ##
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass", "ReticleGlass_WaferVision_Offset_X", "0", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleGlass_WaferVision_Offset_X = Equipment.ToInt(temp.ToString());

            //    //  레티클 글래스 - 레티클 글래스 Wafer 이미지 Offset Y        ## 임의 변경 금지 ##
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass", "ReticleGlass_WaferVision_Offset_Y", "0", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleGlass_WaferVision_Offset_Y = Equipment.ToInt(temp.ToString());



            //    //  레티클 글래스 자동 보정 - 사용 여부 (False : 사용 안함)
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass_AutoCal", "ReticleAutoCal_Usage", "False", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleAutoCal_Usage = temp.ToString() == "False" ? false : true;

            //    //  레티클 글래스 자동 보정 - 상부 비전 허용 오차 (XY, mm)
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass_AutoCal", "ReticleAutoCal_UpperVision_Allowable_XY", "0.005", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleAutoCal_UpperVision_Allowable_XY = Equipment.ToDouble(temp.ToString());

            //    //  레티클 글래스 자동 보정 - 하부 비전 허용 오차 (XY, mm)
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass_AutoCal", "ReticleAutoCal_LowerVision_Allowable_XY", "0.008", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleAutoCal_LowerVision_Allowable_XY = Equipment.ToDouble(temp.ToString());

            //    //  레티클 글래스 자동 보정 - 상부 카메라 얼라인 마크의 위치 평균을 계산하기 위한 측정 회수
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass_AutoCal", "ReticleAutoCal_Upper_AlignMarkCount_forAverage", "1", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleAutoCal_Upper_AlignMarkCount_forAverage = Equipment.ToInt(temp.ToString());

            //    //  레티클 글래스 자동 보정 - 하부 카메라 얼라인 마크의 위치 평균을 계산하기 위한 측정 회수
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass_AutoCal", "ReticleAutoCal_Lower_AlignMarkCount_forAverage", "1", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleAutoCal_Lower_AlignMarkCount_forAverage = Equipment.ToInt(temp.ToString());

            //    //  레티클 글래스 자동 보정 - 마크 위치 평균값 신뢰 공차 (mm)
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass_AutoCal", "ReticleAutoCal_Vision_AverageCheck_Range", "0.05", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleAutoCal_Vision_AverageCheck_Range = Equipment.ToDouble(temp.ToString());

            //    //  레티클 글래스 자동 보정 - 얼라인 재시도 회수
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass_AutoCal", "ReticleAutoCal_Align_Retries", "10", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleAutoCal_Align_Retries = Equipment.ToInt(temp.ToString());



            //    //  PAK 얼라인 - 조명값 조정 크기 1단계
            //    NativeMethods.GetPrivateProfileString("PAK_Align", "PAKAlign_LightValue_Step1", "30", temp, 255, strFIle);
            //    Config.ParamConfig.PAKAlign_LightValue_Step1 = Equipment.ToInt(temp.ToString());

            //    //  PAK 얼라인 - 조명값 조정 크기 2단계
            //    NativeMethods.GetPrivateProfileString("PAK_Align", "PAKAlign_LightValue_Step2", "10", temp, 255, strFIle);
            //    Config.ParamConfig.PAKAlign_LightValue_Step2 = Equipment.ToInt(temp.ToString());



            //    //  인터락 - Z-Slip 기능 - 엘리베이터 Z 축의 Torque 값이 설정치를 초과할 경우 긴급정지 여부
            //    NativeMethods.GetPrivateProfileString("Interlock", "ElevZ_ESTOP_by_Torque_Usage", "False", temp, 255, strFIle);
            //    Config.ParamConfig.ElevZ_ESTOP_by_Torque_Usage = temp.ToString() == "False" ? false : true;

            //    //  인터락 - Z-Slip 기능 - 엘리베이터 Z 축을 긴급정지시키기 위한 Torque 기준값
            //    NativeMethods.GetPrivateProfileString("Interlock", "ElevZ_ESTOP_Torque_Value", "300", temp, 255, strFIle);
            //    Config.ParamConfig.ElevZ_ESTOP_Torque_Value = Equipment.ToDouble(temp.ToString());



            //    //  사용 옵션 - Wafer 얼라인 후 얼라인 위치 정확성 검증 시 Wafer TOP, MID, BOTTOM 위치별 오차 비교 여부. (false : 사용 안함)
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "Wafer_Gate_PosMarginErrorCheck_After_Wafer_Align_Usage", "False", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_Gate_PosMarginErrorCheck_After_Wafer_Align_Usage = temp.ToString() == "False" ? false : true;

            //    //  사용 옵션 - Wafer 얼라인 후 얼라인 위치 정확성 검증 시 Wafer TOP, MID, BOTTOM 위치별 허용 오차 범위 (mm)
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "Wafer_Gate_PosMarginErrorRange_After_Wafer_Align", "0.05", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_Gate_PosMarginErrorRange_After_Wafer_Align = Equipment.ToDouble(temp.ToString());



            //    //  오프셋 - 웨이퍼 && 프로브카드 Packing 시, Latch 상태 검사 여부. (false : 검사 안함)
            //    NativeMethods.GetPrivateProfileString("Offset_Delay", "ProbeCard_LatchStatusCheck_Usage", "False", temp, 255, strFIle);
            //    Config.ParamConfig.ProbeCard_LatchStatusCheck_Usage = temp.ToString() == "False" ? false : true;

            //    //  오프셋 - 웨이퍼 && 프로브카드 Packing 시, Latch 상태 검사를 위한 Offset 거리. (mm, 기준 : Packing 위치)
            //    NativeMethods.GetPrivateProfileString("Offset_Delay", "ProbeCard_LatchStatusCheck_OffsetDistance_from_PackingPos", "20", temp, 255, strFIle);
            //    Config.ParamConfig.ProbeCard_LatchStatusCheck_OffsetDistance_from_PackingPos = Equipment.ToDouble(temp.ToString());

            //    //  오프셋 - 웨이퍼 && 프로브카드 Packing 시, Latch 상태 검사를 위한 Offset 거리 이동 후 대기시간. (ms)
            //    NativeMethods.GetPrivateProfileString("Offset_Delay", "ProbeCard_LatchStatusCheck_OffsetDistance_StableTime", "1000", temp, 255, strFIle);
            //    Config.ParamConfig.ProbeCard_LatchStatusCheck_OffsetDistance_StableTime = Equipment.ToInt(temp.ToString());



            //    //  Dummy Wafer Packing 검증 - 메세지 팝업 사용 여부. (False : 사용 안함)
            //    NativeMethods.GetPrivateProfileString("Dummy_Wafer_Packing", "DummyWaferPacking_Message_Usage", "False", temp, 255, strFIle);
            //    Config.ParamConfig.DummyWaferPacking_Message_Usage = temp.ToString() == "False" ? false : true;

            //    //  Dummy Wafer Packing 검증 - 메세지 팝업 주기 모드 선택. (True : 레시피 변경 시, False : 패킹 작업 누적 회수 도달 시)
            //    NativeMethods.GetPrivateProfileString("Dummy_Wafer_Packing", "DummyWaferPacking_MessagePopup_Mode", "False", temp, 255, strFIle);
            //    Config.ParamConfig.DummyWaferPacking_MessagePopup_Mode = temp.ToString() == "False" ? false : true;

            //    //  Dummy Wafer Packing 검증 - 패킹 작업 누적 회수. (메세지 팝업 주기를 \"False\" 로 할 경우, 이 회수만큼 패킹을 진행하면 검증 요청 메세지창 팝업)
            //    NativeMethods.GetPrivateProfileString("Dummy_Wafer_Packing", "DummyWaferPacking_MessagePopup_Count", "100", temp, 255, strFIle);
            //    Config.ParamConfig.DummyWaferPacking_MessagePopup_Count = Equipment.ToInt(temp.ToString());



            //    //  패킹 방법 - 패킹 모드 선택 (True : 패킹 공압 On 후 Elev. Z 를 단계적으로 올려서 패킹, False : 패킹 높이까지 씬-척을 올려서 패킹)
            //    NativeMethods.GetPrivateProfileString("Wafer_Packing_Mode", "PackingConcept_Mode", "False", temp, 255, strFIle);
            //    Config.ParamConfig.PackingConcept_StepUp_Mode = temp.ToString() == "False" ? false : true;

            //    //  패킹 방법 - 패킹 모드 True 선택 시, 패킹 시작 오프셋. (mm, 패킹 높이에서 이 값만큼 떨어진 위치까지 Elev. Z 를 올린 후 시작)
            //    NativeMethods.GetPrivateProfileString("Wafer_Packing_Mode", "PackingConcept_StepMode_StartOffset", "5", temp, 255, strFIle);
            //    Config.ParamConfig.PackingConcept_StepMode_StartOffset = Equipment.ToDouble(temp.ToString());

            //    //  패킹 방법 - 패킹 모드 True 선택 시, Elev. Z 축을 상숭 시키는 단위 거리. (mm)
            //    NativeMethods.GetPrivateProfileString("Wafer_Packing_Mode", "PackingConcept_StepMode_MoveOffset", "0.1", temp, 255, strFIle);
            //    Config.ParamConfig.PackingConcept_StepMode_MoveOffset = Equipment.ToDouble(temp.ToString());

            //    //  패킹 방법 - 패킹 모드 True 선택 시, Elev. Z 축을 Step 만큼 올린 후 패킹 공압을 체크하기 위해 대기하는 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Wafer_Packing_Mode", "PackingConcept_StepMode_PackingPressure_CheckTime", "500", temp, 255, strFIle);
            //    Config.ParamConfig.PackingConcept_StepMode_PackingPressure_CheckTime = Equipment.ToDouble(temp.ToString());

            //    //  패킹 방법 - 패킹 모드 True 선택 시, Elev. Z 축에 설정된 패킹 높이보다 추가로 더 올리는 거리. (mm)
            //    NativeMethods.GetPrivateProfileString("Wafer_Packing_Mode", "PackingConcept_StepMode_PackingPos_AddOffset", "0.1", temp, 255, strFIle);
            //    Config.ParamConfig.PackingConcept_StepMode_PackingPos_AddOffset = Equipment.ToDouble(temp.ToString());

            //    //  패킹 방법 - 패킹 방법 - 패킹 모드 True 선택 시, Elev. Z 축을 상승 시키는 방법 선택. (True : 저속으로 연속 이동, False : 상승 단위 거리만큼 이동)
            //    NativeMethods.GetPrivateProfileString("Wafer_Packing_Mode", "PackingConcept_StepMode_StepUpMethod_Continuous", "False", temp, 255, strFIle);
            //    Config.ParamConfig.PackingConcept_StepMode_StepUpMethod_Continuous = temp.ToString() == "False" ? false : true;

            //    //  패킹 방법 - 패킹 모드 True 선택 시, Elev. Z 축을 저속으로 상승 시키면서 패킹 공압을 확인하는 모드일 경우 저속 이동 속도 (mm/s)
            //    NativeMethods.GetPrivateProfileString("Wafer_Packing_Mode", "PackingConcept_StepMode_StepUpMethod_Continuous_MoveSpeed", "0.5", temp, 255, strFIle);
            //    Config.ParamConfig.PackingConcept_StepMode_StepUpMethod_Continuous_MoveSpeed = Equipment.ToDouble(temp.ToString());

            //    //  패킹 방법 - 패킹 모드 True 선택 시, Elev. Z 축을 상승 시키면서 패킹 공압을 체크할 때 별도의 공압센서를 사용할 것인지 여부 선택. (True : 사용, False : 사용하지 않음)
            //    NativeMethods.GetPrivateProfileString("Wafer_Packing_Mode", "PackingConcept_StepMode_PackingVacuumSensor_2EA_Usage", "False", temp, 255, strFIle);
            //    Config.ParamConfig.PackingConcept_StepMode_PackingVacuumSensor_2EA_Usage = temp.ToString() == "False" ? false : true;



            //    /// Position 로드
            //    /// 
            //    int m_nIndex_Ready = -1;
            //    int m_nIndex_Load = -1;
            //    int m_nIndex_UnLoad = -1;
            //    int m_nIndex_Reticle_UpperCam = -1;
            //    int m_nIndex_Reticle_LowerCam = -1;

            //    int m_nIndex_AlignPos_Top = -1;
            //    int m_nIndex_AlignPos_Mid = -1;
            //    int m_nIndex_AlignPos_Bottom = -1;
            //    int m_nIndex_AlignPos_Left = -1;
            //    int m_nIndex_AlignPos_Center = -1;
            //    int m_nIndex_AlignPos_Right = -1;

            //    int m_nIndex_PackingPos_ElevZ = -1;


            //    for (int i = 0; i < Config.Positions.Count; i++)
            //    {
            //        //  Ready
            //        if (Config.Positions[i].Name == "Ready")
            //        {
            //            m_nIndex_Ready = i;
            //        }

            //        //  Load
            //        if (Config.Positions[i].Name == "Load")
            //        {
            //            m_nIndex_Load = i;
            //        }

            //        //  UnLoad
            //        if (Config.Positions[i].Name == "UnLoad")
            //        {
            //            m_nIndex_UnLoad = i;
            //        }

            //        //  Reticle Glass 를 보는 Upper Camera 위치 Index
            //        if (Config.Positions[i].Name == "ReticleGlass_UpperCamera")
            //        {
            //            m_nIndex_Reticle_UpperCam = i;
            //        }

            //        //  Reticle Glass 를 보는 Lower Camera 위치 Index
            //        if (Config.Positions[i].Name == "ReticleGlass_LowerCamera")
            //        {
            //            m_nIndex_Reticle_LowerCam = i;
            //        }

            //        //  Align Position (Top)
            //        if (Config.Positions[i].Name == "AlignPosition_Ver_Top")
            //        {
            //            m_nIndex_AlignPos_Top = i;
            //        }

            //        //  Align Position (Mid)
            //        if (Config.Positions[i].Name == "AlignPosition_Ver_Middle")
            //        {
            //            m_nIndex_AlignPos_Mid = i;
            //        }

            //        //  Align Position (Bottom)
            //        if (Config.Positions[i].Name == "AlignPosition_Ver_Bottom")
            //        {
            //            m_nIndex_AlignPos_Bottom = i;
            //        }

            //        //  Align Position (Left)
            //        if (Config.Positions[i].Name == "AlignPosition_Hor_Left")
            //        {
            //            m_nIndex_AlignPos_Left = i;
            //        }

            //        //  Align Position (Center)
            //        if (Config.Positions[i].Name == "AlignPosition_Hor_Center")
            //        {
            //            m_nIndex_AlignPos_Center = i;
            //        }

            //        //  Align Position (Right)
            //        if (Config.Positions[i].Name == "AlignPosition_Hor_Right")
            //        {
            //            m_nIndex_AlignPos_Right = i;
            //        }

            //        //  Wafer Packing (ElevZ)
            //        if (Config.Positions[i].Name == "ProbeWafer_Packing")
            //        {
            //            m_nIndex_PackingPos_ElevZ = i;
            //        }

            //        if ((m_nIndex_Ready != -1) && (m_nIndex_Load != -1) && (m_nIndex_UnLoad != -1) && (m_nIndex_Reticle_UpperCam != -1) && (m_nIndex_Reticle_LowerCam != -1) &&
            //            (m_nIndex_AlignPos_Top != -1) && (m_nIndex_AlignPos_Mid != -1) && (m_nIndex_AlignPos_Bottom != -1) &&
            //            (m_nIndex_AlignPos_Left != -1) && (m_nIndex_AlignPos_Center != -1) && (m_nIndex_AlignPos_Right != -1) &&
            //            (m_nIndex_PackingPos_ElevZ != -1))
            //        {
            //            break;
            //        }
            //    }

            //    //  Ready 좌표 로드
            //    if (m_nIndex_Ready != -1)
            //    {
            //        NativeMethods.GetPrivateProfileString("PositionData_Ready", "Ready_U", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Ready].U = Equipment.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_Ready", "Ready_V", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Ready].V = Equipment.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_Ready", "Ready_W", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Ready].W = Equipment.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_Ready", "Ready_EZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Ready].EZ = Equipment.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_Ready", "Ready_X", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Ready].X = Equipment.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_Ready", "Ready_Y", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Ready].Y = Equipment.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_Ready", "Ready_VZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Ready].VZ = Equipment.ToDouble(temp.ToString());
            //    }

            //    //  Load 좌표 로드
            //    if (m_nIndex_Load != -1)
            //    {
            //        NativeMethods.GetPrivateProfileString("PositionData_Load", "Load_U", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Load].U = Equipment.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_Load", "Load_V", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Load].V = Equipment.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_Load", "Load_W", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Load].W = Equipment.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_Load", "Load_EZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Load].EZ = Equipment.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_Load", "Load_X", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Load].X = Equipment.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_Load", "Load_Y", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Load].Y = Equipment.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_Load", "Load_VZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Load].VZ = Equipment.ToDouble(temp.ToString());
            //    }

            //    //  UnLoad 좌표 로드
            //    if (m_nIndex_UnLoad != -1)
            //    {
            //        NativeMethods.GetPrivateProfileString("PositionData_UnLoad", "UnLoad_U", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_UnLoad].U = Equipment.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_UnLoad", "UnLoad_V", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_UnLoad].V = Equipment.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_UnLoad", "UnLoad_W", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_UnLoad].W = Equipment.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_UnLoad", "UnLoad_EZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_UnLoad].EZ = Equipment.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_UnLoad", "UnLoad_X", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_UnLoad].X = Equipment.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_UnLoad", "UnLoad_Y", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_UnLoad].Y = Equipment.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_UnLoad", "UnLoad_VZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_UnLoad].VZ = Equipment.ToDouble(temp.ToString());
            //    }

            //    //  Upper Cam 좌표 로드
            //    if (m_nIndex_Reticle_UpperCam != -1)
            //    {
            //        NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "UpperCam_U", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Reticle_UpperCam].U = Equipment.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "UpperCam_V", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Reticle_UpperCam].V = Equipment.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "UpperCam_W", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Reticle_UpperCam].W = Equipment.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "UpperCam_EZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Reticle_UpperCam].EZ = Equipment.ToDouble(temp.ToString());
            //        //NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "UpperCam_X", "0.0", temp, 255, strFIle);
            //        //Config.Positions[m_nIndex_Reticle_UpperCam].X = Equipment.ToDouble(temp.ToString());
            //        //NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "UpperCam_Y", "0.0", temp, 255, strFIle);
            //        //Config.Positions[m_nIndex_Reticle_UpperCam].Y = Equipment.ToDouble(temp.ToString());
            //        //NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "UpperCam_VZ", "0.0", temp, 255, strFIle);
            //        //Config.Positions[m_nIndex_Reticle_UpperCam].VZ = Equipment.ToDouble(temp.ToString());

            //        Config.Positions[m_nIndex_Reticle_UpperCam].X = Config.ParamConfig.ReticleGlass_Vision_X_Pos;
            //        Config.Positions[m_nIndex_Reticle_UpperCam].Y = Config.ParamConfig.ReticleGlass_Vision_Y_Pos;
            //        Config.Positions[m_nIndex_Reticle_UpperCam].VZ = Config.ParamConfig.ReticleGlass_Vision_Z_Pos;
            //    }

            //    //  Lower Cam 좌표 로드
            //    if (m_nIndex_Reticle_LowerCam != -1)
            //    {
            //        NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "LowerCam_U", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Reticle_LowerCam].U = Equipment.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "LowerCam_V", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Reticle_LowerCam].V = Equipment.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "LowerCam_W", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Reticle_LowerCam].W = Equipment.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "LowerCam_EZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Reticle_LowerCam].EZ = Equipment.ToDouble(temp.ToString());
            //        //NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "LowerCam_X", "0.0", temp, 255, strFIle);
            //        //Config.Positions[m_nIndex_Reticle_LowerCam].X = Equipment.ToDouble(temp.ToString());
            //        //NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "LowerCam_Y", "0.0", temp, 255, strFIle);
            //        //Config.Positions[m_nIndex_Reticle_LowerCam].Y = Equipment.ToDouble(temp.ToString());
            //        //NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "LowerCam_VZ", "0.0", temp, 255, strFIle);
            //        //Config.Positions[m_nIndex_Reticle_LowerCam].VZ = Equipment.ToDouble(temp.ToString());

            //        Config.Positions[m_nIndex_Reticle_LowerCam].X = Config.ParamConfig.ReticleGlass_Vision_X_Pos;
            //        Config.Positions[m_nIndex_Reticle_LowerCam].Y = Config.ParamConfig.ReticleGlass_Vision_Y_Pos;
            //        Config.Positions[m_nIndex_Reticle_LowerCam].VZ = Config.ParamConfig.ReticleGlass_Vision_Z_Pos;
            //    }


            //    //  Top 위치의 ElevZ 좌표, VisionZ 좌표 로드
            //    if (m_nIndex_AlignPos_Top != -1)
            //    {
            //        NativeMethods.GetPrivateProfileString("PositionData_Align", "ElevZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_AlignPos_Top].EZ = Equipment.ToDouble(temp.ToString()) != 0.0 ? Equipment.ToDouble(temp.ToString()) : Config.Positions[m_nIndex_AlignPos_Top].EZ;
            //        NativeMethods.GetPrivateProfileString("PositionData_Align", "VisionZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_AlignPos_Top].VZ = Equipment.ToDouble(temp.ToString()) != 0.0 ? Equipment.ToDouble(temp.ToString()) : Config.Positions[m_nIndex_AlignPos_Top].VZ;
            //    }

            //    //  Mid 위치의 ElevZ 좌표, VisionZ 좌표 로드
            //    if (m_nIndex_AlignPos_Mid != -1)
            //    {
            //        NativeMethods.GetPrivateProfileString("PositionData_Align", "ElevZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_AlignPos_Mid].EZ = Equipment.ToDouble(temp.ToString()) != 0.0 ? Equipment.ToDouble(temp.ToString()) : Config.Positions[m_nIndex_AlignPos_Mid].EZ;
            //        NativeMethods.GetPrivateProfileString("PositionData_Align", "VisionZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_AlignPos_Mid].VZ = Equipment.ToDouble(temp.ToString()) != 0.0 ? Equipment.ToDouble(temp.ToString()) : Config.Positions[m_nIndex_AlignPos_Mid].VZ;
            //    }

            //    //  Bottom 위치의 ElevZ 좌표, VisionZ 좌표 로드
            //    if (m_nIndex_AlignPos_Bottom != -1)
            //    {
            //        NativeMethods.GetPrivateProfileString("PositionData_Align", "ElevZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_AlignPos_Bottom].EZ = Equipment.ToDouble(temp.ToString()) != 0.0 ? Equipment.ToDouble(temp.ToString()) : Config.Positions[m_nIndex_AlignPos_Bottom].EZ;
            //        NativeMethods.GetPrivateProfileString("PositionData_Align", "VisionZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_AlignPos_Bottom].VZ = Equipment.ToDouble(temp.ToString()) != 0.0 ? Equipment.ToDouble(temp.ToString()) : Config.Positions[m_nIndex_AlignPos_Bottom].VZ;
            //    }

            //    //  Left 위치의 ElevZ 좌표, VisionZ 좌표 로드
            //    if (m_nIndex_AlignPos_Left != -1)
            //    {
            //        NativeMethods.GetPrivateProfileString("PositionData_Align", "ElevZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_AlignPos_Left].EZ = Equipment.ToDouble(temp.ToString()) != 0.0 ? Equipment.ToDouble(temp.ToString()) : Config.Positions[m_nIndex_AlignPos_Left].EZ;
            //        NativeMethods.GetPrivateProfileString("PositionData_Align", "VisionZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_AlignPos_Left].VZ = Equipment.ToDouble(temp.ToString()) != 0.0 ? Equipment.ToDouble(temp.ToString()) : Config.Positions[m_nIndex_AlignPos_Left].VZ;
            //    }

            //    //  Center 위치의 ElevZ 좌표, VisionZ 좌표 로드
            //    if (m_nIndex_AlignPos_Center != -1)
            //    {
            //        NativeMethods.GetPrivateProfileString("PositionData_Align", "ElevZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_AlignPos_Center].EZ = Equipment.ToDouble(temp.ToString()) != 0.0 ? Equipment.ToDouble(temp.ToString()) : Config.Positions[m_nIndex_AlignPos_Center].EZ;
            //        NativeMethods.GetPrivateProfileString("PositionData_Align", "VisionZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_AlignPos_Center].VZ = Equipment.ToDouble(temp.ToString()) != 0.0 ? Equipment.ToDouble(temp.ToString()) : Config.Positions[m_nIndex_AlignPos_Center].VZ;
            //    }

            //    //  Right 위치의 ElevZ 좌표, VisionZ 좌표 로드
            //    if (m_nIndex_AlignPos_Right != -1)
            //    {
            //        NativeMethods.GetPrivateProfileString("PositionData_Align", "ElevZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_AlignPos_Right].EZ = Equipment.ToDouble(temp.ToString()) != 0.0 ? Equipment.ToDouble(temp.ToString()) : Config.Positions[m_nIndex_AlignPos_Right].EZ;
            //        NativeMethods.GetPrivateProfileString("PositionData_Align", "VisionZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_AlignPos_Right].VZ = Equipment.ToDouble(temp.ToString()) != 0.0 ? Equipment.ToDouble(temp.ToString()) : Config.Positions[m_nIndex_AlignPos_Right].VZ;
            //    }

            //    //  Packing 시 ElevZ 좌표 로드
            //    if (m_nIndex_PackingPos_ElevZ != -1)
            //    {
            //        NativeMethods.GetPrivateProfileString("PositionData_Packing", "ElevZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_PackingPos_ElevZ].EZ = Equipment.ToDouble(temp.ToString()) != 0.0 ? Equipment.ToDouble(temp.ToString()) : Config.Positions[m_nIndex_PackingPos_ElevZ].EZ;
            //    }


            //    //stCameraParam stCamera = new stCameraParam();
            //    stCamera = Machine_Parameter_Camera_Setting_Load();

            //    Equipment.PAKCamera_SerialNumber = stCamera.Upper_SerialNumber;
            //    Equipment.PAKCamera_Width = stCamera.Upper_Width;
            //    Equipment.PAKCamera_Height = stCamera.Upper_Height;
            //    Equipment.WaferCamera_SerialNumber = stCamera.Lower_SerialNumber;
            //    Equipment.WaferCamera_Width = stCamera.Lower_Width;
            //    Equipment.WaferCamera_Height = stCamera.Lower_Height;

            //    //Camera_Upper.MyConfig.SerialNumber = stCamera.Upper_SerialNumber;
            //    //Camera_Upper.MyConfig.Resolution = new System.Drawing.Size(stCamera.Upper_Width, stCamera.Upper_Height);
            //    //Camera_Upper.MyConfig.CameraResolution = new System.Drawing.Size(stCamera.Upper_Width, stCamera.Upper_Height);

            //    //Camera_Lower.MyConfig.SerialNumber = stCamera.Lower_SerialNumber;
            //    //Camera_Lower.MyConfig.Resolution = new System.Drawing.Size(stCamera.Lower_Width, stCamera.Lower_Height);
            //    //Camera_Lower.MyConfig.CameraResolution = new System.Drawing.Size(stCamera.Lower_Width, stCamera.Lower_Height);


            //    //  Wafer 이미지 Offset 값이 0 이면? --> 최대 해상도에서 현재 해상도 차이의 1/2 로 설정
            //    if (Camera_Lower.Resolution.Width == MAX_IMAGE_WIDTH)
            //    {
            //        Config.ParamConfig.ReticleGlass_WaferVision_Offset_X = 0;
            //    }
            //    //else if (Config.ParamConfig.ReticleGlass_WaferVision_Offset_X == 0)
            //    //{
            //    //    m_nDiffX = MAX_IMAGE_WIDTH - Camera_Lower.Resolution.Width;

            //    //    Config.ParamConfig.ReticleGlass_WaferVision_Offset_X = m_nDiffX > 0 ? m_nDiffX / 2 : 0;
            //    //}

            //    if (Camera_Lower.Resolution.Height == MAX_IMAGE_HEIGHT)
            //    {
            //        Config.ParamConfig.ReticleGlass_WaferVision_Offset_Y = 0;
            //    }
            //    //else if (Config.ParamConfig.ReticleGlass_WaferVision_Offset_Y == 0)
            //    //{
            //    //    m_nDiffY = MAX_IMAGE_HEIGHT - Camera_Lower.Resolution.Height;

            //    //    Config.ParamConfig.ReticleGlass_WaferVision_Offset_Y = m_nDiffY > 0 ? m_nDiffY / 2 : 0;
            //    //}

            //    //public int MAX_IMAGE_WIDTH = 2248;            //  현장에서 조정된 Size (Center Offset X : 100, Offset Y : 84)
            //    //public int MAX_IMAGE_HEIGHT = 1880;
            //    //if ((Camera_Lower.Resolution.Width == MAX_IMAGE_WIDTH) || (Camera_Lower.Resolution.Height == MAX_IMAGE_HEIGHT))
            //    //{
            //    //    Log.Write("CWA150SA", Equipment.User_Name, "메인 화면", "레시피 변경, Wafer 카메라 해상도 최대");
            //    //    MessageBox.Show("Wafer 카메라 해상도가 최대입니다.\r\n\r\n[레티클 얼라인을 위해서는 Wafer 카메라의 이미지 해상도를 변경해야 합니다.]\r\n[Width : 2248,\tHeight : 1880]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    //}
            //    //else if ((Config.ParamConfig.ReticleGlass_WaferVision_Offset_X == 0) || (Config.ParamConfig.ReticleGlass_WaferVision_Offset_Y == 0))
            //    //{
            //    //    Log.Write("CWA150SA", Equipment.User_Name, "메인 화면", "레시피 변경, Wafer 카메라 이미지 Offset 값 0");
            //    //    MessageBox.Show("Wafer 카메라의 이미지 Offset 값은 0 이 될 수 없습니다.\r\n\r\n[default X : 100,\tY : 84]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    //}


            //    ////  카메라 초기화 시 이미지 Offset 설정
            //    //Camera_Lower.MyConfig.OffsetX = (uint)Config.ParamConfig.ReticleGlass_WaferVision_Offset_X;
            //    //Camera_Lower.MyConfig.OffsetY = (uint)Config.ParamConfig.ReticleGlass_WaferVision_Offset_Y;



            //    string m_strRecipe = "";
            //    RecipeInfo m_recipeInfo = new RecipeInfo();
            //    m_recipeInfo = Equipment.GetCurrentRecipe();

            //    if (m_recipeInfo != null)
            //    {
            //        m_strRecipe = m_recipeInfo.Name;

            //        //DataManager.Instance.UpdateConfigData(m_Module); // 참고 : param save
            //        //Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
            //        Equipment.SaveConfig(m_strRecipe);                                            //  2022. 06. 30.  SCH : Recipe 에 따라 Config 파라미터를 변경하기 위해 이걸로 함.
            //    }
            //    else
            //    {
            //        //DataManager.Instance.UpdateConfigData(m_Module); // 참고 : param save
            //        Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
            //    }

            //    DataManager.Instance.ApplyConfigData(this);

            //    //  Config 창 데이터 갱신을 위해서
            //    Equipment.m_bRedraw_FormWorkStageParameterConfig = true;
            //}

            return m_bRet;
        }

        public void Machine_Parameter_Save()
        {
            string strFIle = "";
            strFIle = ConfigManager.GetConfigPath() + "\\Common Setting (Do not delete or modify).ini";

            //if (File.Exists(strFIle) == false)
            //{
            //    //File.Create(strFIle);
            //    return;
            //}


            ///// Config 저장
            ///// 

            ////  구동 제한 - Wafer Align 시, Elevator Z 축이 올라갈 수 있는 최대 높이 위치
            //NativeMethods.WritePrivateProfileString("Drive_Limit", "Elev_Z", Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign.ToString(), strFIle);

            ////  구동 제한 - Vision Y 축이, Elevator Z 축과 충돌하지 않는 최대 위치
            //NativeMethods.WritePrivateProfileString("Drive_Limit", "Vision_Y", Config.ParamConfig.DriveLimit_VisionY_NotConflictWithElevZ.ToString(), strFIle);

            ////  구동 제한 - UVW 스테이지, U 축 얼라인 제한 위치 (마이너스 방향)
            //NativeMethods.WritePrivateProfileString("Align_Limit", "UVW_U_Minus", Config.ParamConfig.AlignLimit_UVW_U_Minus.ToString(), strFIle);

            ////  구동 제한 - UVW 스테이지, U 축 얼라인 제한 위치 (플러스 방향)
            //NativeMethods.WritePrivateProfileString("Align_Limit", "UVW_U_Plus", Config.ParamConfig.AlignLimit_UVW_U_Plus.ToString(), strFIle);

            ////  구동 제한 - UVW 스테이지, V 축 얼라인 제한 위치 (마이너스 방향)
            //NativeMethods.WritePrivateProfileString("Align_Limit", "UVW_V_Minus", Config.ParamConfig.AlignLimit_UVW_V_Minus.ToString(), strFIle);

            ////  구동 제한 - UVW 스테이지, V 축 얼라인 제한 위치 (플러스 방향)
            //NativeMethods.WritePrivateProfileString("Align_Limit", "UVW_V_Plus", Config.ParamConfig.AlignLimit_UVW_V_Plus.ToString(), strFIle);

            ////  구동 제한 - UVW 스테이지, W 축 얼라인 제한 위치 (마이너스 방향)
            //NativeMethods.WritePrivateProfileString("Align_Limit", "UVW_W_Minus", Config.ParamConfig.AlignLimit_UVW_W_Minus.ToString(), strFIle);

            ////  구동 제한 - UVW 스테이지, W 축 얼라인 제한 위치 (플러스 방향)
            //NativeMethods.WritePrivateProfileString("Align_Limit", "UVW_W_Plus", Config.ParamConfig.AlignLimit_UVW_W_Plus.ToString(), strFIle);

            ////  비전 스케일 - Manual Scale Usage
            //NativeMethods.WritePrivateProfileString("Vision_Scale", "Manual_Scale_Use", Config.ParamConfig.ManualScale_Usage.ToString(), strFIle);

            ////  비전 스케일 - Lower Vision Scale X (mm)
            //NativeMethods.WritePrivateProfileString("Vision_Scale", "Lower_Scale_X", Config.ParamConfig.LowerVision_Scale_X.ToString(), strFIle);

            ////  비전 스케일 - Lower Vision Scale Y (mm)
            //NativeMethods.WritePrivateProfileString("Vision_Scale", "Lower_Scale_Y", Config.ParamConfig.LowerVision_Scale_Y.ToString(), strFIle);

            ////  비전 스케일 - Lower Vision Scale Invert X
            //NativeMethods.WritePrivateProfileString("Vision_Scale", "Lower_Scale_X_Invert", Config.ParamConfig.LowerVision_ScaleInvert_X.ToString(), strFIle);

            ////  비전 스케일 - Lower Vision Scale Invert Y
            //NativeMethods.WritePrivateProfileString("Vision_Scale", "Lower_Scale_Y_Invert", Config.ParamConfig.LowerVision_ScaleInvert_Y.ToString(), strFIle);

            ////  비전 스케일 - Upper Vision Scale X (mm)
            //NativeMethods.WritePrivateProfileString("Vision_Scale", "Upper_Scale_X", Config.ParamConfig.UpperVision_Scale_X.ToString(), strFIle);

            ////  비전 스케일 - Upper Vision Scale Y (mm)
            //NativeMethods.WritePrivateProfileString("Vision_Scale", "Upper_Scale_Y", Config.ParamConfig.UpperVision_Scale_Y.ToString(), strFIle);

            ////  비전 스케일 - Upper Vision Scale Invert X
            //NativeMethods.WritePrivateProfileString("Vision_Scale", "Upper_Scale_X_Invert", Config.ParamConfig.UpperVision_ScaleInvert_X.ToString(), strFIle);

            ////  비전 스케일 - Upper Vision Scale Invert Y
            //NativeMethods.WritePrivateProfileString("Vision_Scale", "Upper_Scale_Y_Invert", Config.ParamConfig.UpperVision_ScaleInvert_Y.ToString(), strFIle);

            ////  레티클 글래스 - 얼라인 조명 밝기값 (상부 카메라)
            //NativeMethods.WritePrivateProfileString("Reticle_Glass", "Upper_Vision_LightValue", Config.ParamConfig.ReticleAlign_UpperVision_LightValue.ToString(), strFIle);

            ////  레티클 글래스 - 얼라인 조명 밝기값 (하부 카메라)
            //NativeMethods.WritePrivateProfileString("Reticle_Glass", "Lower_Vision_LightValue", Config.ParamConfig.ReticleAlign_LowerVision_LightValue.ToString(), strFIle);

            ////  얼라인 - 얼라인 시 Theta 축 회전 속도 (mm/s)
            //NativeMethods.WritePrivateProfileString("Vision_Align", "Theta_Rotation_Speed", Config.ParamConfig.Align_Theta_Velocity.ToString(), strFIle);

            ////  얼라인 - 얼라인 시 Theta 축 회전 가속도 (mm/s²)
            //NativeMethods.WritePrivateProfileString("Vision_Align", "Theta_Rotation_Accel", Config.ParamConfig.Align_Theta_Accel.ToString(), strFIle);

            ////  얼라인 - 얼라인 시 Theta 축 회전 감속도 (mm/s²)
            //NativeMethods.WritePrivateProfileString("Vision_Align", "Theta_Rotation_Decel", Config.ParamConfig.Align_Theta_Decel.ToString(), strFIle);

            ////  얼라인 - 얼라인 재시도 회수
            //NativeMethods.WritePrivateProfileString("Vision_Align", "Align_Retry", Config.ParamConfig.Align_Retries.ToString(), strFIle);

            ////  얼라인 - 얼라인 각도 Invert
            //NativeMethods.WritePrivateProfileString("Vision_Scale", "Align_Angle_Invert", Config.ParamConfig.Align_AngleInvert.ToString(), strFIle);

            ////  얼라인 - 얼라인 각도 계산 시 Atan 함수 사용
            //NativeMethods.WritePrivateProfileString("Vision_Scale", "Align_Calc_AtanFunc", Config.ParamConfig.Align_ThetaCalcFunction_Atan.ToString(), strFIle);

            ////  얼라인 스테이지 - Theta 회전 반경 (mm)
            //NativeMethods.WritePrivateProfileString("Align_Stage", "Turning_Radius", Config.ParamConfig.Align_Theta_From_RotCenter_To_UVW_Distance.ToString(), strFIle);

            ////  얼라인 스테이지 - Theta 1˚ 회전을 위한 UVW 각 축 이동량 (mm)
            //NativeMethods.WritePrivateProfileString("Align_Stage", "Movement_Amount_1Deg_Rotation", Config.ParamConfig.Align_Theta_Movement_MM_Per_1Deg.ToString(), strFIle);

            ////  얼라인 이미지 저장 여부
            //NativeMethods.WritePrivateProfileString("Align_Image", "Image_Save_Use", Config.ParamConfig.AlignImageSave_Usage.ToString(), strFIle);

            ////  얼라인 이미지 저장 위치 용량 부족 경고 기준치 (GB)
            //NativeMethods.WritePrivateProfileString("Align_Image", "Image_Save_DriveSpace_Warning_Value", Config.ParamConfig.AlignImageSaveFolder_WarningSpace.ToString(), strFIle);

            ////  프로브 카드 클램프 타입 1 일 경우, 업다운 실린더 동작 대기 시간
            //NativeMethods.WritePrivateProfileString("Machine_Type", "ProbeCard_ClampTypeB_CylUpDown_StableTime", Config.ParamConfig.ProbeCard_ClampTypeB_CylUpDown_StableTime.ToString(), strFIle);

            ////  레티클 글래스 - 레시피 변경 시, 레티클 글래스 센터를 확인해야 작업 진행 가능
            //NativeMethods.WritePrivateProfileString("Reticle_Glass", "ReticleGlass_CenterCheck_forAlign", Config.ParamConfig.ReticleGlass_CenterCheck_forAlign.ToString(), strFIle);

            ////  레티클 글래스 - 레티클 글래스를 확인 시 비전 카메라와 엘리베이터의 충돌 방지를 위한 Vision Y 축 이동 한계 위치. (mm, 대부분 90.0 이내)
            //NativeMethods.WritePrivateProfileString("Reticle_Glass", "ReticleGlass_Vision_Y_Limit", Config.ParamConfig.ReticleGlass_Vision_Y_Limit.ToString(), strFIle);

            ////  PAK, 웨이퍼 Gate - Center 가 일치할 때의 Offset X
            //NativeMethods.WritePrivateProfileString("PAK_Gate_Center", "PAK_WaferGate_Centering_Offset_X", Config.ParamConfig.PAK_WaferGate_Centering_Offset_X.ToString(), strFIle);

            ////  PAK, 웨이퍼 Gate - Center 가 일치할 때의 Offset Y
            //NativeMethods.WritePrivateProfileString("PAK_Gate_Center", "PAK_WaferGate_Centering_Offset_Y", Config.ParamConfig.PAK_WaferGate_Centering_Offset_Y.ToString(), strFIle);

            ////  자동 로그아웃 기능 사용 여부
            //NativeMethods.WritePrivateProfileString("Auto_LogOut", "Auto_LogOut_Usage", Config.ParamConfig.Auto_LogOut_Usage.ToString(), strFIle);

            ////  자동 로그아웃 설정 시간
            //NativeMethods.WritePrivateProfileString("Auto_LogOut", "Auto_LogOut_Time", Config.ParamConfig.Auto_LogOut_Time.ToString(), strFIle);

            ////  패킹 작업 완료시 Leak Check 기능 사용 여부
            //NativeMethods.WritePrivateProfileString("Packing_LeakCheck", "AlignPacking_LeakCheck_Usage", Config.ParamConfig.AlignPacking_OP_LeakCheck_Usage.ToString(), strFIle);

            ////  패킹 작업 완료시 Leak Check 설정 시간 (sec)
            //NativeMethods.WritePrivateProfileString("Packing_LeakCheck", "Packing_LeakCheck_Time", Config.ParamConfig.Packing_OP_LeakCheck_Time.ToString(), strFIle);

            ////  패킹 작업 완료 후, 제품 언로딩을 위해 엘리베이터를 내리는 거리 (mm)
            //NativeMethods.WritePrivateProfileString("ElevatorZ", "DownDistance_AfterPacking", Config.ParamConfig.Wafer_ProbeCard_Down_Distance_After_Packing.ToString(), strFIle);

            ////  전면 안전센서 사용 여부
            //NativeMethods.WritePrivateProfileString("Interlock", "Area_Sensor_Usage", Config.ParamConfig.AreaSensor_Usage.ToString(), strFIle);

            ////  전면 안전센서 감지 시 Servo Off 여부
            //NativeMethods.WritePrivateProfileString("Interlock", "AreaSensor_ServoOff_Usage", Config.ParamConfig.AreaSensor_ServoOff_Usage.ToString(), strFIle);

            //////  전면 안전센서 해제 후 다시 동작시키기 위해 대기하는 시간 (sec)
            ////NativeMethods.WritePrivateProfileString("Interlock", "Area_Sensor_Release_Pause_Time", Config.ParamConfig.AreaSensor_Off_Pause_Time.ToString(), strFIle);

            ////  수동패킹 모드 사용 권한 설정
            //NativeMethods.WritePrivateProfileString("Manual_Packing", "Enable_Admin_Only", Config.ParamConfig.ManualPacking_Only_Admin.ToString(), strFIle);



            ////  사용 옵션과 안정화 시간

            ////  사용 옵션 - 웨이퍼 얼라인 시작 전 PAK 공압 라인 막힘 검사 사용 여부
            //NativeMethods.WritePrivateProfileString("Operation_Options", "Pak_AirLineCheck_Usage", Config.ParamConfig.Pak_AirLineCheck_Usage.ToString(), strFIle);

            ////  사용 옵션 - 웨이퍼 얼라인 시작 전 PAK 공압 라인 막힘 검사 시간 (ms)
            //NativeMethods.WritePrivateProfileString("Operation_Options", "Pak_AirLineCheck_Time", Config.ParamConfig.Pak_AirLineCheck_Time.ToString(), strFIle);

            ////  사용 옵션 - Packing 공압 신호 사용 여부
            //NativeMethods.WritePrivateProfileString("Operation_Options", "Packing_VacuumSignal_Usage", Config.ParamConfig.Packing_VacuumSignal_Usage.ToString(), strFIle);

            ////  사용 옵션 - Packing 공압 신호를 사용하지 않을 경우, 대기 시간 (ms)
            //NativeMethods.WritePrivateProfileString("Operation_Options", "Packing_VacuumSignal_Time", Config.ParamConfig.Packing_VacuumSignal_Time.ToString(), strFIle);

            ////  사용 옵션 - Packing 공압 신호를 사용할 경우, 추가 가압 시간 (ms)
            //NativeMethods.WritePrivateProfileString("Operation_Options", "Packing_VacuumSignal_AfterTime", Config.ParamConfig.Packing_VacuumSignal_AfterTime.ToString(), strFIle);

            ////  사용 옵션 - Wafer 공압 체크 사용 여부
            //NativeMethods.WritePrivateProfileString("Operation_Options", "Wafer_VacuumSignal_Usage", Config.ParamConfig.Wafer_VacuumSignal_Usage.ToString(), strFIle);

            ////  사용 옵션 - Wafer 공압을 사용하지 않을 경우, 대기 시간 (ms)
            //NativeMethods.WritePrivateProfileString("Operation_Options", "Wafer_VacuumSignal_Time", Config.ParamConfig.Wafer_VacuumSignal_Time.ToString(), strFIle);

            ////  사용 옵션 - Thin-Chuck 감지 센서 사용 여부
            //NativeMethods.WritePrivateProfileString("Operation_Options", "ThinChuck_DetectSignal_Usage", Config.ParamConfig.ThinChuck_DetectSignal_Usage.ToString(), strFIle);

            ////  사용 옵션 - Thin-Chuck 공압 체크 사용 여부
            //NativeMethods.WritePrivateProfileString("Operation_Options", "ThinChuck_VacuumSignal_Usage", Config.ParamConfig.ThinChuck_VacuumSignal_Usage.ToString(), strFIle);

            ////  사용 옵션 - Thin-Chuck 공압을 사용하지 않을 경우, 대기 시간 (ms)
            //NativeMethods.WritePrivateProfileString("Operation_Options", "ThinChuck_VacuumSignal_Time", Config.ParamConfig.ThinChuck_VacuumSignal_Time.ToString(), strFIle);

            ////  사용 옵션 - Wafer 얼라인 동작 중, 비전 사용 여부. (false : 사용 안함)
            //NativeMethods.WritePrivateProfileString("Operation_Options", "Wafer_Align_Cam_Usage", Config.ParamConfig.Wafer_Align_Cam_Usage.ToString(), strFIle);

            ////  사용 옵션 - Wafer 얼라인 후 Packing 작업 자동 시작 여부. (false : 얼라인 완료 후 Packing 작업 대기)
            //NativeMethods.WritePrivateProfileString("Operation_Options", "Packing_AutoStart_After_Wafer_Align_Usage", Config.ParamConfig.Packing_AutoStart_After_Wafer_Align_Usage.ToString(), strFIle);

            ////  사용 옵션 - Wafer 얼라인 후 얼라인 위치 정확성 검증 여부. (false : 사용 안함) 
            //NativeMethods.WritePrivateProfileString("Operation_Options", "Wafer_Align_ErrorCheck_After_Wafer_Align_Usage", Config.ParamConfig.Wafer_Align_ErrorCheck_After_Wafer_Align_Usage.ToString(), strFIle);

            ////  사용 옵션 - 실내 조명을 상시 On 상태로 할 것인지 여부. (false : Align 시 Off 되고, 자재를 Loading 할 때 On)
            //NativeMethods.WritePrivateProfileString("Operation_Options", "Indoor_Light_AlwaysOn_Usage", Config.ParamConfig.Indoor_Light_AlwaysOn_Usage.ToString(), strFIle);



            ////  안정화 시간 - Packing 공압 신호 On 후 대기 시간 (ms)
            //NativeMethods.WritePrivateProfileString("Stable_Time", "StableTime_after_PackingSignal_On", Config.ParamConfig.StableTime_after_PackingSignal_On.ToString(), strFIle);

            ////  안정화 시간 - Packing 공압 신호 On 후 Thin-Chuck 공압을 해제하기 위해 대기하는 시간 (ms)
            //NativeMethods.WritePrivateProfileString("Stable_Time", "StableTime_after_PackingSignal_On_before_ThinChuck_Vacuum_Off", Config.ParamConfig.StableTime_after_PackingSignal_On_before_ThinChuck_Vacuum_Off.ToString(), strFIle);

            ////  안정화 시간 - Packing 공압 신호 On 후 Wafer 공압을 해제하기 위해 대기하는 시간 (ms)
            //NativeMethods.WritePrivateProfileString("Stable_Time", "StableTime_after_PackingSignal_On_before_Wafer_Vacuum_Off", Config.ParamConfig.StableTime_after_PackingSignal_On_before_Wafer_Vacuum_Off.ToString(), strFIle);

            ////  안정화 시간 - Packing 작업 중, Wafer 공압 해제 후 안정화(대기) 시간 (ms)
            //NativeMethods.WritePrivateProfileString("Stable_Time", "StableTime_after_WaferVacuumSignal_Off", Config.ParamConfig.StableTime_after_WaferVacuumSignal_Off.ToString(), strFIle);

            ////  안정화 시간 - Packing 을 위해 Elev. Z 축이 프로브 카드 위치까지 이동한 후 안정화 시간 (ms)
            //NativeMethods.WritePrivateProfileString("Stable_Time", "StableTime_before_PackingSignal_On", Config.ParamConfig.StableTime_before_PackingSignal_On.ToString(), strFIle);

            ////  안정화 시간 - Packing 작업 중, Thin-Chuck 공압 해제 후 안정화(대기) 시간 (ms)
            //NativeMethods.WritePrivateProfileString("Stable_Time", "StableTime_after_ThinChuckVacuumSignal_Off", Config.ParamConfig.StableTime_after_ThinChuckVacuumSignal_Off.ToString(), strFIle);

            ////  안정화 시간 - Wafer 얼라인 시, 마크 위치 이동 후 안정화 시간 (ms)
            //NativeMethods.WritePrivateProfileString("Stable_Time", "WaferAlign_Move_StableTime", Config.ParamConfig.WaferAlign_Move_StableTime.ToString(), strFIle);

            ////  안정화 시간 - Unpacking 작업 중, Unpacking 공압 신호 On 후 Elev. Z 축을 내리기 시작할 때까지 대기 시간 (ms)
            //NativeMethods.WritePrivateProfileString("Stable_Time", "StableTime_after_UnpackingSignal_On", Config.ParamConfig.StableTime_after_UnpackingSignal_On.ToString(), strFIle);



            ////  Offset && Delay - 웨이퍼 && 프로브카드 Packing 시, 저속 이동 거리 (mm)
            //NativeMethods.WritePrivateProfileString("Offset_Delay", "Wafer_ProbeCard_PackingOffset_Distance", Config.ParamConfig.Wafer_ProbeCard_PackingOffset_Distance.ToString(), strFIle);

            ////  Offset && Delay - 웨이퍼 && 프로브카드 Unpacking 시, 저속 이동 거리 (mm)
            //NativeMethods.WritePrivateProfileString("Offset_Delay", "Wafer_ProbeCard_UnpackingOffset_Distance", Config.ParamConfig.Wafer_ProbeCard_UnpackingOffset_Distance.ToString(), strFIle);

            ////  Offset && Delay - 웨이퍼 && 프로브카드 UnPacking 시, UnPacking 신호 인가 후 이동하는 거리 (mm)
            //NativeMethods.WritePrivateProfileString("Offset_Delay", "Wafer_ProbeCard_UnPackingOffset_Distance", Config.ParamConfig.Wafer_ProbeCard_UnPackingOffset_Distance.ToString(), strFIle);

            ////  Offset && Delay - 웨이퍼 && 프로브카드 UnPacking 시, UnPacking 을 위한 Elevator Z 축 이동 거리 (기준 높이 : Packing 위치) (mm)
            //NativeMethods.WritePrivateProfileString("Offset_Delay", "Wafer_ProbeCard_UnPackingStartOffset_Distance", Config.ParamConfig.Wafer_ProbeCard_UnPackingStartOffset_Distance.ToString(), strFIle);

            ////  Offset && Delay - 웨이퍼 && 프로브카드 Manual Packing 시, 엘리베이터 Z 축의 1단계 Offset 거리 (mm, > 0)
            //NativeMethods.WritePrivateProfileString("Offset_Delay", "Wafer_ProbeCard_ManualPacking_ElevZ_Offset_Distance", Config.ParamConfig.Wafer_ProbeCard_ManualPacking_ElevZ_Offset_Distance.ToString(), strFIle);

            ////  Offset && Delay - 웨이퍼 && 프로브카드 Manual Packing 시, 엘리베이터 Z 축의 1단계 Offset 이동 방법 (2 Step 이동 or 이동 후 대기)
            //NativeMethods.WritePrivateProfileString("Offset_Delay", "Wafer_ProbeCard_ManualPacking_ElevZ_OffsetMove_Concept", Config.ParamConfig.Wafer_ProbeCard_ManualPacking_1st_Step_ElevZ_OffsetMove_Concept.ToString(), strFIle);

            ////  Offset && Delay - 웨이퍼 && 프로브카드 안전 분리 동작 시, 씬-척 낙하 방지를 위해 엘리베이터 Z 축을 올리는 위치. (Packing 위치 대비 Offset 거리 (mm, > 0)
            //NativeMethods.WritePrivateProfileString("Offset_Delay", "Wafer_ProbeCard_SafelyUnpacking_ElevZ_Offset_Distance", Config.ParamConfig.Wafer_ProbeCard_SafelyUnpacking_ElevZ_Offset_Distance.ToString(), strFIle);



            ////  카메라 설정 - 시리얼 넘버 사용 여부 (False : Config 에 세팅된 시리얼 넘버 사용)
            //NativeMethods.WritePrivateProfileString("Camera_SerialNumber", "Camera_SerialNumber_Type", Config.ParamConfig.Camera_SerialNumber_Type.ToString(), strFIle);

            ////  카메라 설정 - PAK 카메라 시리얼 넘버 (Upper Camera)
            //NativeMethods.WritePrivateProfileString("Camera_SerialNumber", "Camera_SerialNumber_PAK", Config.ParamConfig.Camera_SerialNumber_PAK.ToString(), strFIle);

            ////  카메라 설정 - Wafer 카메라 시리얼 넘버 (Lower Camera)
            //NativeMethods.WritePrivateProfileString("Camera_SerialNumber", "Camera_SerialNumber_Wafer", Config.ParamConfig.Camera_SerialNumber_Wafer.ToString(), strFIle);



            ////  레티클 글래스 - 레티클 글래스 확인 위치 (VIsion X)                  ## 임의 변경 금지 ##
            //NativeMethods.WritePrivateProfileString("Reticle_Glass", "ReticleGlass_Vision_X_Pos", Config.ParamConfig.ReticleGlass_Vision_X_Pos.ToString(), strFIle);

            ////  레티클 글래스 - 레티클 글래스 확인 위치 (VIsion Y)                  ## 임의 변경 금지 ##
            //NativeMethods.WritePrivateProfileString("Reticle_Glass", "ReticleGlass_Vision_Y_Pos", Config.ParamConfig.ReticleGlass_Vision_Y_Pos.ToString(), strFIle);

            ////  레티클 글래스 - 레티클 글래스 확인 위치 (VIsion Z)                  ## 임의 변경 금지 ##
            //NativeMethods.WritePrivateProfileString("Reticle_Glass", "ReticleGlass_Vision_Z_Pos", Config.ParamConfig.ReticleGlass_Vision_Z_Pos.ToString(), strFIle);

            ////  레티클 글래스 - 레티클 글래스 확인 위치 (Elev. Z - PAK 카메라)      ## 임의 변경 금지 ##
            //NativeMethods.WritePrivateProfileString("Reticle_Glass", "ReticleGlass_Elev_Z_PAK_Pos", Config.ParamConfig.ReticleGlass_Elev_Z_PAK_Pos.ToString(), strFIle);

            ////  레티클 글래스 - 레티클 글래스 확인 위치 (Elev. Z - Wafer 카메라)    ## 임의 변경 금지 ##
            //NativeMethods.WritePrivateProfileString("Reticle_Glass", "ReticleGlass_Elev_Z_Wafer_Pos", Config.ParamConfig.ReticleGlass_Elev_Z_Wafer_Pos.ToString(), strFIle);

            ////  레티클 글래스 - 레티클 글래스 확인 위치 (VIsion Z)                  ## 임의 변경 금지 ##
            //NativeMethods.WritePrivateProfileString("Reticle_Glass", "ReticleGlass_WaferVision_Offset_X", Config.ParamConfig.ReticleGlass_WaferVision_Offset_X.ToString(), strFIle);

            ////  레티클 글래스 - 레티클 글래스 확인 위치 (VIsion Z)                  ## 임의 변경 금지 ##
            //NativeMethods.WritePrivateProfileString("Reticle_Glass", "ReticleGlass_WaferVision_Offset_Y", Config.ParamConfig.ReticleGlass_WaferVision_Offset_Y.ToString(), strFIle);




            ////  레티클 글래스 자동 보정 - 사용 여부 (False : 사용 안함)
            //NativeMethods.WritePrivateProfileString("Reticle_Glass_AutoCal", "ReticleAutoCal_Usage", Config.ParamConfig.ReticleAutoCal_Usage.ToString(), strFIle);

            ////  레티클 글래스 자동 보정 - 상부 비전 허용 오차 (XY, mm)
            //NativeMethods.WritePrivateProfileString("Reticle_Glass_AutoCal", "ReticleAutoCal_UpperVision_Allowable_XY", Config.ParamConfig.ReticleAutoCal_UpperVision_Allowable_XY.ToString(), strFIle);

            ////  레티클 글래스 자동 보정 - 하부 비전 허용 오차 (XY, mm)
            //NativeMethods.WritePrivateProfileString("Reticle_Glass_AutoCal", "ReticleAutoCal_LowerVision_Allowable_XY", Config.ParamConfig.ReticleAutoCal_LowerVision_Allowable_XY.ToString(), strFIle);

            ////  레티클 글래스 자동 보정 - 상부 카메라 얼라인 마크의 위치 평균을 계산하기 위한 측정 회수
            //NativeMethods.WritePrivateProfileString("Reticle_Glass_AutoCal", "ReticleAutoCal_Upper_AlignMarkCount_forAverage", Config.ParamConfig.ReticleAutoCal_Upper_AlignMarkCount_forAverage.ToString(), strFIle);

            ////  레티클 글래스 자동 보정 - 하부 카메라 얼라인 마크의 위치 평균을 계산하기 위한 측정 회수
            //NativeMethods.WritePrivateProfileString("Reticle_Glass_AutoCal", "ReticleAutoCal_Lower_AlignMarkCount_forAverage", Config.ParamConfig.ReticleAutoCal_Lower_AlignMarkCount_forAverage.ToString(), strFIle);

            ////  레티클 글래스 자동 보정 - 마크 위치 평균값 신뢰 공차 (mm)
            //NativeMethods.WritePrivateProfileString("Reticle_Glass_AutoCal", "ReticleAutoCal_Vision_AverageCheck_Range", Config.ParamConfig.ReticleAutoCal_Vision_AverageCheck_Range.ToString(), strFIle);

            ////  레티클 글래스 자동 보정 - 얼라인 재시도 회수
            //NativeMethods.WritePrivateProfileString("Reticle_Glass_AutoCal", "ReticleAutoCal_Align_Retries", Config.ParamConfig.ReticleAutoCal_Align_Retries.ToString(), strFIle);



            ////  PAK 얼라인 - 조명값 조정 크기 1단계
            //NativeMethods.WritePrivateProfileString("PAK_Align", "PAKAlign_LightValue_Step1", Config.ParamConfig.PAKAlign_LightValue_Step1.ToString(), strFIle);

            ////  PAK 얼라인 - 조명값 조정 크기 2단계
            //NativeMethods.WritePrivateProfileString("PAK_Align", "PAKAlign_LightValue_Step2", Config.ParamConfig.PAKAlign_LightValue_Step2.ToString(), strFIle);



            ////  인터락 - Z-Slip 기능 - 엘리베이터 Z 축의 Torque 값이 설정치를 초과할 경우 긴급정지 여부
            //NativeMethods.WritePrivateProfileString("Interlock", "ElevZ_ESTOP_by_Torque_Usage", Config.ParamConfig.ElevZ_ESTOP_by_Torque_Usage.ToString(), strFIle);

            ////  인터락 - Z-Slip 기능 - 엘리베이터 Z 축을 긴급정지시키기 위한 Torque 기준값
            //NativeMethods.WritePrivateProfileString("Interlock", "ElevZ_ESTOP_Torque_Value", Config.ParamConfig.ElevZ_ESTOP_Torque_Value.ToString(), strFIle);



            ////  사용 옵션 - Wafer 얼라인 후 얼라인 위치 정확성 검증 시 Wafer TOP, MID, BOTTOM 위치별 오차 비교 여부. (false : 사용 안함)
            //NativeMethods.WritePrivateProfileString("Operation_Options", "Wafer_Gate_PosMarginErrorCheck_After_Wafer_Align_Usage", Config.ParamConfig.Wafer_Gate_PosMarginErrorCheck_After_Wafer_Align_Usage.ToString(), strFIle);

            ////  사용 옵션 - Wafer 얼라인 후 얼라인 위치 정확성 검증 시 Wafer TOP, MID, BOTTOM 위치별 허용 오차 범위 (mm)
            //NativeMethods.WritePrivateProfileString("Operation_Options", "Wafer_Gate_PosMarginErrorRange_After_Wafer_Align", Config.ParamConfig.Wafer_Gate_PosMarginErrorRange_After_Wafer_Align.ToString(), strFIle);



            ////  오프셋 - 웨이퍼 && 프로브카드 Packing 시, Latch 상태 검사 여부. (false : 검사 안함)
            //NativeMethods.WritePrivateProfileString("Offset_Delay", "ProbeCard_LatchStatusCheck_Usage", Config.ParamConfig.ProbeCard_LatchStatusCheck_Usage.ToString(), strFIle);

            ////  오프셋 - 웨이퍼 && 프로브카드 Packing 시, Latch 상태 검사를 위한 Offset 거리 (mm, 기준 : Packing 위치)
            //NativeMethods.WritePrivateProfileString("Offset_Delay", "ProbeCard_LatchStatusCheck_OffsetDistance_from_PackingPos", Config.ParamConfig.ProbeCard_LatchStatusCheck_OffsetDistance_from_PackingPos.ToString(), strFIle);

            ////  오프셋 - 웨이퍼 && 프로브카드 Packing 시, Latch 상태 검사를 위한 Offset 거리 이동 후 대기시간. (ms)
            //NativeMethods.WritePrivateProfileString("Offset_Delay", "ProbeCard_LatchStatusCheck_OffsetDistance_StableTime", Config.ParamConfig.ProbeCard_LatchStatusCheck_OffsetDistance_StableTime.ToString(), strFIle);



            ////  Dummy Wafer Packing 검증 - 메세지 팝업 사용 여부. (False : 사용 안함)
            //NativeMethods.WritePrivateProfileString("Dummy_Wafer_Packing", "DummyWaferPacking_Message_Usage", Config.ParamConfig.DummyWaferPacking_Message_Usage.ToString(), strFIle);

            ////  Dummy Wafer Packing 검증 - 메세지 팝업 주기 모드 선택. (True : 레시피 변경 시, False : 패킹 작업 누적 회수 도달 시)
            //NativeMethods.WritePrivateProfileString("Dummy_Wafer_Packing", "DummyWaferPacking_MessagePopup_Mode", Config.ParamConfig.DummyWaferPacking_MessagePopup_Mode.ToString(), strFIle);

            ////  Dummy Wafer Packing 검증 - 패킹 작업 누적 회수. (메세지 팝업 주기를 \"False\" 로 할 경우, 이 회수만큼 패킹을 진행하면 검증 요청 메세지창 팝업)
            //NativeMethods.WritePrivateProfileString("Dummy_Wafer_Packing", "DummyWaferPacking_MessagePopup_Count", Config.ParamConfig.DummyWaferPacking_MessagePopup_Count.ToString(), strFIle);



            ////  패킹 방법 - 패킹 모드 선택 (True : 패킹 공압 On 후 Elev. Z 를 단계적으로 올려서 패킹, False : 패킹 높이까지 씬-척을 올려서 패킹)
            //NativeMethods.WritePrivateProfileString("Wafer_Packing_Mode", "PackingConcept_Mode", Config.ParamConfig.PackingConcept_StepUp_Mode.ToString(), strFIle);

            ////  패킹 방법 - 패킹 모드 True 선택 시, 패킹 시작 오프셋. (mm, 패킹 높이에서 이 값만큼 떨어진 위치까지 Elev. Z 를 올린 후 시작)
            //NativeMethods.WritePrivateProfileString("Wafer_Packing_Mode", "PackingConcept_StepMode_StartOffset", Config.ParamConfig.PackingConcept_StepMode_StartOffset.ToString(), strFIle);

            ////  패킹 방법 - 패킹 모드 True 선택 시, Elev. Z 축을 상숭 시키는 단위 거리. (mm)
            //NativeMethods.WritePrivateProfileString("Wafer_Packing_Mode", "PackingConcept_StepMode_MoveOffset", Config.ParamConfig.PackingConcept_StepMode_MoveOffset.ToString(), strFIle);

            ////  패킹 방법 - 패킹 모드 True 선택 시, Elev. Z 축을 Step 만큼 올린 후 패킹 공압을 체크하기 위해 대기하는 시간 (ms)
            //NativeMethods.WritePrivateProfileString("Wafer_Packing_Mode", "PackingConcept_StepMode_PackingPressure_CheckTime", Config.ParamConfig.PackingConcept_StepMode_PackingPressure_CheckTime.ToString(), strFIle);

            ////  패킹 방법 - 패킹 모드 True 선택 시, Elev. Z 축에 설정된 패킹 높이보다 추가로 더 올리는 거리. (mm)
            //NativeMethods.WritePrivateProfileString("Wafer_Packing_Mode", "PackingConcept_StepMode_PackingPos_AddOffset", Config.ParamConfig.PackingConcept_StepMode_PackingPos_AddOffset.ToString(), strFIle);

            ////  패킹 방법 - 패킹 방법 - 패킹 모드 True 선택 시, Elev. Z 축을 상승 시키는 방법 선택. (True : 저속으로 연속 이동, False : 상승 단위 거리만큼 이동)
            //NativeMethods.WritePrivateProfileString("Wafer_Packing_Mode", "PackingConcept_StepMode_StepUpMethod_Continuous", Config.ParamConfig.PackingConcept_StepMode_StepUpMethod_Continuous.ToString(), strFIle);

            ////  패킹 방법 - 패킹 모드 True 선택 시, Elev. Z 축을 저속으로 상승 시키면서 패킹 공압을 확인하는 모드일 경우 저속 이동 속도 (mm/s)
            //NativeMethods.WritePrivateProfileString("Wafer_Packing_Mode", "PackingConcept_StepMode_StepUpMethod_Continuous_MoveSpeed", Config.ParamConfig.PackingConcept_StepMode_StepUpMethod_Continuous_MoveSpeed.ToString(), strFIle);

            ////  패킹 방법 - 패킹 모드 True 선택 시, Elev. Z 축을 상승 시키면서 패킹 공압을 체크할 때 별도의 공압센서를 사용할 것인지 여부 선택. (True : 사용, False : 사용하지 않음)
            //NativeMethods.WritePrivateProfileString("Wafer_Packing_Mode", "PackingConcept_StepMode_PackingVacuumSensor_2EA_Usage", Config.ParamConfig.PackingConcept_StepMode_PackingVacuumSensor_2EA_Usage.ToString(), strFIle);



            ///// Position 저장
            ///// 
            //int m_nIndex_Ready = -1;
            //int m_nIndex_Load = -1;
            //int m_nIndex_UnLoad = -1;
            //int m_nIndex_Reticle_UpperCam = -1;
            //int m_nIndex_Reticle_LowerCam = -1;

            //int m_nIndex_AlignPos_Top = -1;
            //int m_nIndex_Packing_ElevZ = -1;

            //for (int i = 0; i < Config.Positions.Count; i++)
            //{
            //    //  Ready
            //    if (Config.Positions[i].Name == "Ready")
            //    {
            //        m_nIndex_Ready = i;
            //    }

            //    //  Load
            //    if (Config.Positions[i].Name == "Load")
            //    {
            //        m_nIndex_Load = i;
            //    }

            //    //  UnLoad
            //    if (Config.Positions[i].Name == "UnLoad")
            //    {
            //        m_nIndex_UnLoad = i;
            //    }

            //    //  Reticle Glass 를 보는 Upper Camera 위치 Index
            //    if (Config.Positions[i].Name == "ReticleGlass_UpperCamera")
            //    {
            //        m_nIndex_Reticle_UpperCam = i;
            //    }

            //    //  Reticle Glass 를 보는 Lower Camera 위치 Index
            //    if (Config.Positions[i].Name == "ReticleGlass_LowerCamera")
            //    {
            //        m_nIndex_Reticle_LowerCam = i;
            //    }

            //    //  Align 위치 (Top)
            //    if (Config.Positions[i].Name == "AlignPosition_Ver_Top")
            //    {
            //        m_nIndex_AlignPos_Top = i;
            //    }

            //    //  Wafer Packing 높이 (Elevator Z)
            //    if (Config.Positions[i].Name == "ProbeWafer_Packing")
            //    {
            //        m_nIndex_Packing_ElevZ = i;
            //    }


            //    if ((m_nIndex_Ready != -1) && (m_nIndex_Load != -1) && (m_nIndex_UnLoad != -1) && (m_nIndex_Reticle_UpperCam != -1) && (m_nIndex_Reticle_LowerCam != -1) &&
            //        (m_nIndex_AlignPos_Top != -1) && (m_nIndex_Packing_ElevZ != -1))
            //    {
            //        break;
            //    }
            //}

            ////  Ready 좌표 저장
            //if (m_nIndex_Ready != -1)
            //{
            //    NativeMethods.WritePrivateProfileString("PositionData_Ready", "Ready_U", Config.Positions[m_nIndex_Ready].U.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_Ready", "Ready_V", Config.Positions[m_nIndex_Ready].V.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_Ready", "Ready_W", Config.Positions[m_nIndex_Ready].W.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_Ready", "Ready_EZ", Config.Positions[m_nIndex_Ready].EZ.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_Ready", "Ready_X", Config.Positions[m_nIndex_Ready].X.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_Ready", "Ready_Y", Config.Positions[m_nIndex_Ready].Y.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_Ready", "Ready_VZ", Config.Positions[m_nIndex_Ready].VZ.ToString(), strFIle);
            //}

            ////  Load 좌표 저장
            //if (m_nIndex_Load != -1)
            //{
            //    NativeMethods.WritePrivateProfileString("PositionData_Load", "Load_U", Config.Positions[m_nIndex_Load].U.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_Load", "Load_V", Config.Positions[m_nIndex_Load].V.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_Load", "Load_W", Config.Positions[m_nIndex_Load].W.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_Load", "Load_EZ", Config.Positions[m_nIndex_Load].EZ.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_Load", "Load_X", Config.Positions[m_nIndex_Load].X.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_Load", "Load_Y", Config.Positions[m_nIndex_Load].Y.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_Load", "Load_VZ", Config.Positions[m_nIndex_Load].VZ.ToString(), strFIle);
            //}

            ////  UnLoad 좌표 저장
            //if (m_nIndex_UnLoad != -1)
            //{
            //    NativeMethods.WritePrivateProfileString("PositionData_UnLoad", "UnLoad_U", Config.Positions[m_nIndex_UnLoad].U.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_UnLoad", "UnLoad_V", Config.Positions[m_nIndex_UnLoad].V.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_UnLoad", "UnLoad_W", Config.Positions[m_nIndex_UnLoad].W.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_UnLoad", "UnLoad_EZ", Config.Positions[m_nIndex_UnLoad].EZ.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_UnLoad", "UnLoad_X", Config.Positions[m_nIndex_UnLoad].X.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_UnLoad", "UnLoad_Y", Config.Positions[m_nIndex_UnLoad].Y.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_UnLoad", "UnLoad_VZ", Config.Positions[m_nIndex_UnLoad].VZ.ToString(), strFIle);
            //}

            ////  Upper Cam 좌표 저장
            //if (m_nIndex_Reticle_UpperCam != -1)
            //{
            //    NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "UpperCam_U", Config.Positions[m_nIndex_Reticle_UpperCam].U.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "UpperCam_V", Config.Positions[m_nIndex_Reticle_UpperCam].V.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "UpperCam_W", Config.Positions[m_nIndex_Reticle_UpperCam].W.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "UpperCam_EZ", Config.Positions[m_nIndex_Reticle_UpperCam].EZ.ToString(), strFIle);
            //    //NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "UpperCam_X", Config.Positions[m_nIndex_Reticle_UpperCam].X.ToString(), strFIle);
            //    //NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "UpperCam_Y", Config.Positions[m_nIndex_Reticle_UpperCam].Y.ToString(), strFIle);
            //    //NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "UpperCam_VZ", Config.Positions[m_nIndex_Reticle_UpperCam].VZ.ToString(), strFIle);
            //}

            ////  Lower Cam 좌표 저장
            //if (m_nIndex_Reticle_LowerCam != -1)
            //{
            //    NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "LowerCam_U", Config.Positions[m_nIndex_Reticle_LowerCam].U.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "LowerCam_V", Config.Positions[m_nIndex_Reticle_LowerCam].V.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "LowerCam_W", Config.Positions[m_nIndex_Reticle_LowerCam].W.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "LowerCam_EZ", Config.Positions[m_nIndex_Reticle_LowerCam].EZ.ToString(), strFIle);
            //    //NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "LowerCam_X", Config.Positions[m_nIndex_Reticle_LowerCam].X.ToString(), strFIle);
            //    //NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "LowerCam_Y", Config.Positions[m_nIndex_Reticle_LowerCam].Y.ToString(), strFIle);
            //    //NativeMethods.WritePrivateProfileString("PositionData_ReticleGlass", "LowerCam_VZ", Config.Positions[m_nIndex_Reticle_LowerCam].VZ.ToString(), strFIle);
            //}


            ////  Top 위치의 ElevZ 좌표, VisionZ 좌표 저장
            //if (m_nIndex_AlignPos_Top != -1)
            //{
            //    NativeMethods.WritePrivateProfileString("PositionData_Align", "ElevZ", Config.Positions[m_nIndex_AlignPos_Top].EZ.ToString(), strFIle);
            //    NativeMethods.WritePrivateProfileString("PositionData_Align", "VisionZ", Config.Positions[m_nIndex_AlignPos_Top].VZ.ToString(), strFIle);
            //}

            ////  Packing 시 ElevZ 좌표 저장
            //if (m_nIndex_Packing_ElevZ != -1)
            //{
            //    NativeMethods.WritePrivateProfileString("PositionData_Packing", "ElevZ", Config.Positions[m_nIndex_Packing_ElevZ].EZ.ToString(), strFIle);
            //}
        }


        //  로그 자동 삭제
        public void Delete_Backup(string folderDir)
        {
            try
            {
                int deleteDay = 3;

                DirectoryInfo di = new DirectoryInfo(folderDir);
                if (di.Exists)
                {
                    DirectoryInfo[] dirInfo = di.GetDirectories();
                    FileInfo[] fileInfo = di.GetFiles();

                    string IDate = DateTime.Today.AddDays(-deleteDay).ToString("yyyyMMdd");

                    //  폴더가 있으면 삭제
                    foreach( DirectoryInfo dir in dirInfo)
                    {
                        if (IDate.CompareTo(dir.LastWriteTime.ToString("yyyyMMdd")) > 0)
                        {
                            dir.Attributes = FileAttributes.Normal;
                            dir.Delete(true);
                        }
                    }

                    //  파일이 있으면 삭제
                    foreach( FileInfo fi in fileInfo)
                    {
                        if (IDate.CompareTo(fi.LastWriteTime.ToString("yyyyMMdd")) > 0)
                        {
                            fi.Attributes = FileAttributes.Normal;
                            fi.Delete();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        public void SetRecovery()
        {
            SetRecoveryStaker0(m_nStacker0_ModulePutdownWaitingPos_Step);
            m_nStacker0_ModulePutdownWaitingPos_Step = m_nStacker0_ModulePutdownWaitingPos_Step_Recovery;

            SetRecoveryStaker1(m_nStacker1_ModulePutdownWaitingPos_Step);
            m_nStacker1_ModulePutdownWaitingPos_Step = m_nStacker1_ModulePutdownWaitingPos_Step_Recovery;

            SetRecoveryTransfer(m_nUnloader_Transfer_Step);
            m_nUnloader_Transfer_Step = m_nUnloader_Transfer_Step_Recovery;
        }

        public void ResetRecovery()
        {
            m_nStacker0_ModulePutdownWaitingPos_Step_Recovery = 0;
            m_nStacker1_ModulePutdownWaitingPos_Step_Recovery = 0;
            m_nUnloader_Transfer_Step_Recovery = 0;
        }


        //motion 함수 


        public bool IsInterlock_UnloaderPortR_Enabled()
        {
            bool bRtn = false;
            string strTemp = "";

            if (!workStage.m_bHomeOK)
            {
                strTemp = string.Format("IsInterlock_UnloaderPortR_Enabled [Fail]: 장비 초기화 후 구동");
                Log.Write("SLD-200", Equipment.User_Name, strTemp);
                return bRtn;
            }

            if (!MC_Func.MC_GetDone((int)Unloader.nAxis.Z0) ||
                !MC_Func.MC_GetInposition((int)Unloader.nAxis.Z0))
            {
                strTemp = string.Format("IsInterlock_UnloaderPortR_Enabled [Fail]: LoaderPortR Axis이 이동중입니다.");
                Log.Write("SLD-200", Equipment.User_Name, strTemp);
                return bRtn;
            }

            // Todo: 구영남 - 여기 설정값 셋팅 연결 필요.
            //double dLoaderTransferX = 100;
            //double dLoaderTransferZ = 100;
            //double dCurPositionLoaderTransferX = MC_Func.MC_GetEncPos((int)Loader.nAxis.TR_X);
            //double dCurPositionLoaderTransferZ = MC_Func.MC_GetEncPos((int)Loader.nAxis.TR_Z);
            //if (dCurPositionLoaderTransferX < dLoaderTransferX)
            //{
            //    if (dCurPositionLoaderTransferZ > dLoaderTransferZ)
            //    {
            //        strTemp = string.Format("IsInterlock_LoaderPortL_Enabled [Fail]: Loader Z축 설정보다 내려와 있습니다.");
            //        Log.Write("SLD-200", Equipment.User_Name, strTemp);
            //        return bRtn = false;
            //    }
            //}

            bRtn = true;
            return bRtn;
        }
        public bool MovetoUnloader_TeachingPositionsPortR(int nTeachingPos, Type_Motor_Speed typeSpeed)
        {
            // string strTemp = "";
            bool bRtn = false;
            double dVelocity = 0.0;
            double dAcc = 0.0;
            try
            {
                if (IsInterlock_UnloaderPortR_Enabled())
                {
                    if (IsUnloader_TeachingPositionsPortR(nTeachingPos) == false)
                    {
                        switch (typeSpeed)
                        {
                            case Type_Motor_Speed.Fine:
                                dVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.Z0].Jog_Speed_Fine;
                                dAcc = Equipment.stAxisParam[(int)Unloader.nAxis.Z0].Common_Acceleration_Fine;
                                break;
                            case Type_Motor_Speed.Coarse:
                                dVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.Z0].Jog_Speed_Coarse;
                                dAcc = Equipment.stAxisParam[(int)Unloader.nAxis.Z0].Common_Acceleration_Coarse;
                                break;
                            default:
                                dVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.Z0].Jog_Speed_Fine;
                                dAcc = Equipment.stAxisParam[(int)Unloader.nAxis.Z0].Common_Acceleration_Fine;
                                break;
                        }

                        MC_Func.MC_MovePosition((int)Unloader.nAxis.Z0, loader.stLDULTeachingPos[nTeachingPos].UL_Stacker_Z0,
                                                dVelocity, dAcc, dAcc);
                    }

                    bRtn = true;
                }
                //strTemp = string.Format("Move_to_WorkStage_TeachingPositions 이동");
                //Log.Write("SLD-200", Equipment.User_Name, strTemp);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }

            return bRtn;
        }
        public bool IsUnloader_TeachingPositionsPortR(int nTeachingPos)
        {
            bool bRtn = false;

            if (MC_Func.MC_GetDone((int)Unloader.nAxis.Z0) &&
                MC_Func.MC_PosTolerance((int)Unloader.nAxis.Z0, loader.stLDULTeachingPos[nTeachingPos].UL_Stacker_Z0))
            {
                bRtn = true;
            }

            return bRtn;
        }

        public bool IsInterlock_UnloaderPortL_Enabled()
        {
            bool bRtn = false;
            string strTemp = "";

            if (!workStage.m_bHomeOK)
            {
                strTemp = string.Format("IsInterlock_UnloaderPortL_Enabled [Fail]: 장비 초기화 후 구동");
                Log.Write("SLD-200", Equipment.User_Name, strTemp);
                return bRtn;
            }

            if (!MC_Func.MC_GetDone((int)Unloader.nAxis.Z1) ||
                !MC_Func.MC_GetInposition((int)Unloader.nAxis.Z1))
            {
                strTemp = string.Format("IsInterlock_UnloaderPortL_Enabled [Fail]: UnloaderPortL Axis이 이동중입니다.");
                Log.Write("SLD-200", Equipment.User_Name, strTemp);
                return bRtn;
            }

            // Todo: 구영남 - 여기 설정값 셋팅 연결 필요.
            //double dLoaderTransferX = 100;
            //double dLoaderTransferZ = 100;
            //double dCurPositionLoaderTransferX = MC_Func.MC_GetEncPos((int)Loader.nAxis.TR_X);
            //double dCurPositionLoaderTransferZ = MC_Func.MC_GetEncPos((int)Loader.nAxis.TR_Z);
            //if (dCurPositionLoaderTransferX < dLoaderTransferX)
            //{
            //    if (dCurPositionLoaderTransferZ > dLoaderTransferZ)
            //    {
            //        strTemp = string.Format("IsInterlock_LoaderPortL_Enabled [Fail]: Loader Z축 설정보다 내려와 있습니다.");
            //        Log.Write("SLD-200", Equipment.User_Name, strTemp);
            //        return bRtn = false;
            //    }
            //}

            bRtn = true;
            return bRtn;
        }
        public bool MovetoUnloader_TeachingPositionsPortL(int nTeachingPos, Type_Motor_Speed typeSpeed)
        {
            // string strTemp = "";
            bool bRtn = false;
            double dVelocity = 0.0;
            double dAcc = 0.0;
            try
            {
                if (IsInterlock_UnloaderPortL_Enabled())
                {
                    if (IsUnloader_TeachingPositionsPortL(nTeachingPos) == false)
                    {
                        switch (typeSpeed)
                        {
                            case Type_Motor_Speed.Fine:
                                dVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.Z1].Jog_Speed_Fine;
                                dAcc = Equipment.stAxisParam[(int)Unloader.nAxis.Z1].Common_Acceleration_Fine;
                                break;
                            case Type_Motor_Speed.Coarse:
                                dVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.Z1].Jog_Speed_Coarse;
                                dAcc = Equipment.stAxisParam[(int)Unloader.nAxis.Z1].Common_Acceleration_Coarse;
                                break;
                            default:
                                dVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.Z1].Jog_Speed_Fine;
                                dAcc = Equipment.stAxisParam[(int)Unloader.nAxis.Z1].Common_Acceleration_Fine;
                                break;
                        }

                        MC_Func.MC_MovePosition((int)Unloader.nAxis.Z1, loader.stLDULTeachingPos[nTeachingPos].UL_Stacker_Z1,
                                                dVelocity, dAcc, dAcc);
                    }

                    bRtn = true;
                }
                //strTemp = string.Format("Move_to_WorkStage_TeachingPositions 이동");
                //Log.Write("SLD-200", Equipment.User_Name, strTemp);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }

            return bRtn;
        }
        public bool IsUnloader_TeachingPositionsPortL(int nTeachingPos)
        {
            bool bRtn = false;

            if (MC_Func.MC_GetDone((int)Unloader.nAxis.Z1) &&
                MC_Func.MC_PosTolerance((int)Unloader.nAxis.Z1, loader.stLDULTeachingPos[nTeachingPos].UL_Stacker_Z1))
            {
                bRtn = true;
            }

            return bRtn;
        }

        public bool IsInterlock_UnloaderTransferZ_Enabled()
        {
            bool bRtn = false;
            string strTemp = "";

            if (!workStage.m_bHomeOK)
            {
                strTemp = string.Format("IsInterlock_UnloaderTransferZ_Enabled [Fail]: 장비 초기화 후 구동");
                Log.Write("SLD-200", Equipment.User_Name, strTemp);
                return bRtn;
            }

            if (!MC_Func.MC_GetDone((int)Unloader.nAxis.TR_Z) ||
                !MC_Func.MC_GetInposition((int)Unloader.nAxis.TR_Z))
            {
                strTemp = string.Format("IsInterlock_UnloaderTransferZ_Enabled [Fail]: UnloaderTransferZ Axis이 이동중입니다.");
                Log.Write("SLD-200", Equipment.User_Name, strTemp);
                return bRtn;
            }

            if (!MC_Func.MC_GetDone((int)Unloader.nAxis.TR_X) ||
                !MC_Func.MC_GetInposition((int)Unloader.nAxis.TR_X))
            {
                strTemp = string.Format("IsInterlock_UnloaderTransferZ_Enabled [Fail]: UnloaderTransferX Axis이 이동중입니다.");
                Log.Write("SLD-200", Equipment.User_Name, strTemp);
                return bRtn;
            }

            bRtn = true;
            return bRtn;
        }
        public bool MovetoUnloader_TeachingPositionsTransferZ(int nTeachingPos, Type_Motor_Speed typeSpeed, bool bSynchronous = false)
        {
            string strTemp = "";
            bool bRtn = false;
            double dVelocity = 0.0;
            double dAcc = 0.0;
            try
            {
                if (IsInterlock_UnloaderTransferZ_Enabled())
                {
                    if (IsUnloader_TeachingPositionsTransferZ(nTeachingPos) == false)
                    {
                        switch (typeSpeed)
                        {
                            case Type_Motor_Speed.Fine:
                                dVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.TR_Z].Jog_Speed_Fine;
                                dAcc = Equipment.stAxisParam[(int)Unloader.nAxis.TR_Z].Common_Acceleration_Fine;
                                break;
                            case Type_Motor_Speed.Coarse:
                                dVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.TR_Z].Jog_Speed_Coarse;
                                dAcc = Equipment.stAxisParam[(int)Unloader.nAxis.TR_Z].Common_Acceleration_Coarse;
                                break;
                            default:
                                dVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.TR_Z].Jog_Speed_Fine;
                                dAcc = Equipment.stAxisParam[(int)Unloader.nAxis.TR_Z].Common_Acceleration_Fine;
                                break;
                        }

                        MC_Func.MC_MovePosition((int)Unloader.nAxis.TR_Z, loader.stLDULTeachingPos[nTeachingPos].UL_Transfer_Z,
                                                dVelocity, dAcc, dAcc);

                        if (bSynchronous)
                        {
                            bool bTimeout = false;
                            DateTime StartTime = DateTime.Now;
                            TimeSpan ProcessTime;
                            while (true)
                            {
                                if (IsUnloader_TeachingPositionsTransferZ(nTeachingPos))
                                    break;

                                //Config.TimeOut
                                if (2000 > 0) // 2000 정도면 2초?
                                {
                                    ProcessTime = DateTime.Now - StartTime;
                                    if (ProcessTime.TotalMilliseconds >= 2000)
                                    {
                                        bTimeout = true;
                                        break;
                                    }
                                }
                                Thread.Sleep(1);
                            }

                            if (bTimeout)
                            {
                                strTemp = string.Format("MovetoUnloader_TeachingPositionsTransferZ [Fail]: UnloaderTransferZ Axis이 이동 실패.");
                                Log.Write("SLD-200", Equipment.User_Name, strTemp);

                                Alarm alarm = new Alarm();
                                alarm.Title = "Unloader TransferZ Timeout";
                                alarm.Code = -100;
                                alarm.Grade = "Stop";
                                alarm.Source = this.Name;
                                alarm.Cause = "Unloader TransferZ Timeout이 발생했습니다. Unloader TransferZ을 확인해주세요.";
                                //AlarmPost(AlarmKey.LoaderTransferZTimeout);

                                return bRtn = false;
                            }
                        }
                    }

                    bRtn = true;
                }
                //strTemp = string.Format("Move_to_WorkStage_TeachingPositions 이동");
                //Log.Write("SLD-200", Equipment.User_Name, strTemp);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }

            return bRtn;
        }
        public bool IsUnloader_TeachingPositionsTransferZ(int nTeachingPos)
        {
            bool bRtn = false;

            if (MC_Func.MC_GetDone((int)Unloader.nAxis.TR_Z) &&
                MC_Func.MC_PosTolerance((int)Unloader.nAxis.TR_Z, loader.stLDULTeachingPos[nTeachingPos].UL_Transfer_Z))
            {
                bRtn = true;
            }

            return bRtn;
        }

        public bool IsInterlock_UnloaderTransferX_Enabled()
        {
            bool bRtn = false;
            string strTemp = "";

            if (!workStage.m_bHomeOK)
            {
                strTemp = string.Format("IsInterlock_UnloaderTransferX_Enabled [Fail]: 장비 초기화 후 구동");
                Log.Write("SLD-200", Equipment.User_Name, strTemp);
                return bRtn;
            }

            if (!MC_Func.MC_GetDone((int)Unloader.nAxis.TR_X) ||
                !MC_Func.MC_GetInposition((int)Unloader.nAxis.TR_X))
            {
                strTemp = string.Format("IsInterlock_UnloaderTransferX_Enabled [Fail]: UnloaderTransferX Axis이 이동중입니다.");
                Log.Write("SLD-200", Equipment.User_Name, strTemp);
                return bRtn;
            }

            double dTargetZ = loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_TR_SafetyPos].UL_Transfer_Z;
            if (MC_Func.MC_GetDone((int)Unloader.nAxis.TR_Z) &&
                MC_Func.MC_PosTolerance((int)Unloader.nAxis.TR_Z, dTargetZ))
            {
                strTemp = string.Format("IsInterlock_UnloaderTransferX_Enabled [Fail]: UnloaderTransferZ Axis이 Safety Pos 아닙니다.");
                Log.Write("SLD-200", Equipment.User_Name, strTemp);
                return bRtn;
            }

            // Todo: 구영남 - 여기 설정값 셋팅 연결 필요.
            //double dLoaderTransferX = 100;
            //double dLoaderTransferZ = 100;
            //double dCurPositionLoaderTransferX = MC_Func.MC_GetEncPos((int)Loader.nAxis.TR_X);
            //double dCurPositionLoaderTransferZ = MC_Func.MC_GetEncPos((int)Loader.nAxis.TR_Z);
            //if (dCurPositionLoaderTransferX < dLoaderTransferX)
            //{
            //    if (dCurPositionLoaderTransferZ > dLoaderTransferZ)
            //    {
            //        strTemp = string.Format("IsInterlock_LoaderPortL_Enabled [Fail]: Loader Z축 설정보다 내려와 있습니다.");
            //        Log.Write("SLD-200", Equipment.User_Name, strTemp);
            //        return bRtn = false;
            //    }
            //}

            bRtn = true;
            return bRtn;
        }
        public bool MovetoUnloader_TeachingPositionsTransferX(int nTeachingPos, Type_Motor_Speed typeSpeed, bool bSynchronous = false)
        {
            string strTemp = "";
            bool bRtn = false;
            double dVelocity = 0.0;
            double dAcc = 0.0;
            try
            {
                //Loader Z-Axis를 무조건 safety Pos 으로 보내고 이동.
                if (MovetoUnloader_TeachingPositionsTransferZ((int)LDUL_TeachingPosList.UL_TR_SafetyPos, typeSpeed, true))
                {
                    if (IsInterlock_UnloaderTransferX_Enabled())
                    {
                        if (IsUnloader_TeachingPositionsTransferX(nTeachingPos) == false)
                        {
                            switch (typeSpeed)
                            {
                                case Type_Motor_Speed.Fine:
                                    dVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.TR_X].Jog_Speed_Fine;
                                    dAcc = Equipment.stAxisParam[(int)Unloader.nAxis.TR_X].Common_Acceleration_Fine;
                                    break;
                                case Type_Motor_Speed.Coarse:
                                    dVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.TR_X].Jog_Speed_Coarse;
                                    dAcc = Equipment.stAxisParam[(int)Unloader.nAxis.TR_X].Common_Acceleration_Coarse;
                                    break;
                                default:
                                    dVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.TR_X].Jog_Speed_Fine;
                                    dAcc = Equipment.stAxisParam[(int)Unloader.nAxis.TR_X].Common_Acceleration_Fine;
                                    break;
                            }

                            MC_Func.MC_MovePosition((int)Unloader.nAxis.TR_X, loader.stLDULTeachingPos[nTeachingPos].UL_Transfer_X,
                                                    dVelocity, dAcc, dAcc);
                        }

                        if (bSynchronous)
                        {
                            bool bTimeout = false;
                            DateTime StartTime = DateTime.Now;
                            TimeSpan ProcessTime;
                            while (true)
                            {
                                if (IsUnloader_TeachingPositionsTransferX(nTeachingPos))
                                    break;

                                //Config.TimeOut
                                if (2000 > 0) // 2000 정도면 2초?
                                {
                                    ProcessTime = DateTime.Now - StartTime;
                                    if (ProcessTime.TotalMilliseconds >= 2000)
                                    {
                                        bTimeout = true;
                                        break;
                                    }
                                }
                                Thread.Sleep(1);
                            }

                            if (bTimeout)
                            {
                                strTemp = string.Format("MovetoUnloader_TeachingPositionsTransferX [Fail]: UnloaderTransferX Axis이 이동 실패.");
                                Log.Write("SLD-200", Equipment.User_Name, strTemp);

                                Alarm alarm = new Alarm();
                                alarm.Title = "Loader TransferX Timeout";
                                alarm.Code = -100;
                                alarm.Grade = "Stop";
                                alarm.Source = this.Name;
                                alarm.Cause = "Unloader TransferX Timeout이 발생했습니다. Unloader TransferX을 확인해주세요.";
                                //AlarmPost(AlarmKey.LoaderTransferZTimeout);

                                return bRtn = false;
                            }
                        }

                        bRtn = true;
                    }
                }
                else
                {
                    strTemp = string.Format("MovetoLoader_TeachingPositionsTransferX [Fail]: LoaderTransferZ Axis이 Safety Pos 이동 실패.");
                    Log.Write("SLD-200", Equipment.User_Name, strTemp);
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }

            return bRtn;
        }
        public bool IsUnloader_TeachingPositionsTransferX(int nTeachingPos)
        {
            bool bRtn = false;

            if (MC_Func.MC_GetDone((int)Unloader.nAxis.TR_X) &&
                MC_Func.MC_PosTolerance((int)Unloader.nAxis.TR_X, loader.stLDULTeachingPos[nTeachingPos].UL_Transfer_X))
            {
                bRtn = true;
            }

            return bRtn;
        }
    }
}