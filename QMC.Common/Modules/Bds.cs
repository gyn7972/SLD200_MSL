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


namespace QMC.Common.Modules
{
    [Serializable]
    public class Bds : Module
    {
        #region Define

        public enum nAxis
        {
            MASK_Y = 3,
        }
        #endregion


        #region Variables
       

        #endregion

        #region Field
        SettingParameterCollection PosParam_Bds;          //  2022. 04. 25.  SCH : 모터 위치 파라미터를 갖다쓰기 위해 선언해봄.
        //static Conveyor conveyor = new Conveyor("");            //  요거 다시해야 함. Conveyor.cs 에 정의된 변수에 접근할 수 있게... 어케 함? -_-
                                                                //  static 으로 선언하면 되긴 헌디.... 맞는건가 -_-
        public MotionFunction MC_Func = new MotionFunction();
        //public ACSSPiiPlusAxis ACS_Func = new ACSSPiiPlusAxis();
        #endregion

        #region Property
        public BdsConfig Config { set; get; }
        public BdsParameterConfig ParamConfig { set; get; }
        public BdsRecipe Recipe { set; get; }                

        public YStage Stage { set; get; }                         //  MSL SLD-200C, SLD-200U 의 Mask Y 축

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


        #region NewForm 을 위한 Teching Position List 변수

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
        public static stBDSAxesPos[] stBDSTeachingPos = new stBDSAxesPos[System.Enum.GetValues(typeof(BDS_TeachingPosList)).Length];


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

        #region Single Action (Stacker, Module Pickup Waiting Pos)

        public int m_nStacker_ModulePickupWaitingPos_Step { set; get; }             //  Stacker, Module Pickup Waiting Position Step

        public enum StackerModulePickupWaitingPos_Step
        {
            None = 0,

            Start,                                                          //  시작


            Process_Condition_Check,                                        //  동작 조건 체크 (Module Exist Sensor On Check, Transfer Cycle : None)


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


            Complete                                                        //  완료
        }
        #endregion


        #region Single Action (Loader Transfer)

        public int m_nLoader_Transfer_Step { set; get; }                                   //  Transfer Step

        public enum Loader_Transfer_Step
        {
            None = 0,

            Start,                                                          //  시작


            Process_Type_Check,                                             //  동작 타입 체크 (Stacker 에서 Module PickUp, M-Aligner 에서 Module PickUp, Work Stage 로 Module PutDown, M-Aligner 로 Module PutDown)


            /// <summary>
            /// Stacker 에서 Module Pick Up - 시작
            /// </summary>
            Stacker_ModulePickup_Condition_Check,                           //  Stacker 에서 Module Pick Up 조건 체크 (Stacker Module Exist Sensor On, Stacker Module Full Sensor On, Transfer Picker Vacuum Off Check, Stacker Cycle : None)

            StackerPickUp_TransferZ_Move_ReadyPos,                          //  Transfer Z 축, 대기 위치로 이동
            StackerPickUp_TransferZ_Move_ReadyPos_DoneCheck,                //  Transfer Z 축, 대기 위치로 이동 완료 확인

            StackerPickUp_TransferX_Move_StackerPos,                        //  Transfer X 축, Stacker 위치로 이동
            StackerPickUp_TransferX_Move_StackerPos_DoneCheck,              //  Transfer X 축, Stacker 위치로 이동 완료 확인

            StackerPickUp_TransferZ_Move_PickUpPos_1stStep,                 //  Transfer Z 축, Module Pick Up 위치로 이동 (1단계, 최종 위치에서 위로 10 mm)
            StackerPickUp_TransferZ_Move_PickUpPos_1stStep_DoneCheck,       //  Transfer Z 축, Module Pick Up 위치로 이동 완료 확인

            StackerPickUp_TransferZ_Move_PickUpPos_2ndStep,                 //  Transfer Z 축, Module Pick Up 위치로 이동 (2단계, 최종 위치)
            StackerPickUp_TransferZ_Move_PickUpPos_2ndStep_DoneCheck,       //  Transfer Z 축, Module Pick Up 위치로 이동 완료 확인
            
            StackerPickUp_Transfer_PickerVacuum_On,                         //  Transfer, Module Picker Vacuum On
            StackerPickUp_Transfer_PickerVacuum_OnCheck,                    //  Transfer, Module Picker Vacuum On 확인

            StackerPickUp_TransferZ_Move_ReadyPos2_1stStep,                 //  Transfer Z 축, 대기 위치로 이동 (1단계, 현재 위치에서 위로 10 mm)
            StackerPickUp_TransferZ_Move_ReadyPos2_1stStep_DoneCheck,       //  Transfer Z 축, 대기 위치로 이동 완료 확인

            StackerPickUp_TransferZ_Move_ReadyPos2_2ndStep,                 //  Transfer Z 축, 대기 위치로 이동 (2단계, 최종 위치)
            StackerPickUp_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck,       //  Transfer Z 축, 대기 위치로 이동 완료 확인
            /// <summary>
            /// Stacker 에서 Module Pick Up - 완료
            /// </summary>
            /// 


            /// <summary>
            /// M-Aligner 에서 Module Pick Up - 시작
            /// </summary>
            MAligner_ModulePickup_Condition_Check,                          //  M-Aligner 에서 Module Pick Up 조건 체크 (M-Align 완료, M-Aligner Vacuum On Check, Transfer Cycle : None, M-Align Cycle : None)

            MAligner_MAlign_NotComplete,                                    //  M-Aligner 에서 Align 이 완료되지 않은 상태일 경우 (M-Align Cyc. Call)
            MAligner_MAlign_Start,                                          //  M-Aligner 에서 Align 시작
            MAligner_MAlign_CompleteCheck,                                  //  M-Aligner 에서 Align 완료 확인 (M-Aligner Vacuum 이 Off 이면 Pick Up 하지 않음 --> 자재가 없다고 판단)

            MAlignerPickUp_TransferZ_Move_ReadyPos,                         //  Transfer Z 축, 대기 위치로 이동
            MAlignerPickUp_TransferZ_Move_ReadyPos_DoneCheck,               //  Transfer Z 축, 대기 위치로 이동 완료 확인

            MAlignerPickUp_TransferX_Move_MAlignerPos,                      //  Transfer X 축, M-Aligner 위치로 이동
            MAlignerPickUp_TransferX_Move_MAlignerPos_DoneCheck,            //  Transfer X 축, M-Aligner 위치로 이동 완료 확인

            MAlignerPickUp_TransferZ_Move_PickUpPos_1stStep,                //  Transfer Z 축, Module Pick Up 위치로 이동 (1단계, 최종 위치에서 위로 10 mm)
            MAlignerPickUp_TransferZ_Move_PickUpPos_1stStep_DoneCheck,      //  Transfer Z 축, Module Pick Up 위치로 이동 완료 확인

            MAlignerPickUp_TransferZ_Move_PickUpPos_2ndStep,                //  Transfer Z 축, Module Pick Up 위치로 이동 (2단계, 최종 위치)
            MAlignerPickUp_TransferZ_Move_PickUpPos_2ndStep_DoneCheck,      //  Transfer Z 축, Module Pick Up 위치로 이동 완료 확인

            MAlignerPickUp_Transfer_PickerVacuum_On,                        //  Transfer, Module Picker Vacuum On
            MAlignerPickUp_Transfer_PickerVacuum_OnCheck,                   //  Transfer, Module Picker Vacuum On 확인
            MAlignerPickUp_MAligner_Vacuum_Off,                             //  M-Aligner, Vacuum Off

                MAlignerPickUp_MAlignerXY_MoveType1_Widely,                 //  M-Aligner XY 축, Module 을 들어올리기 위해 열어주는 위치로 이동 (1mm 정도) - Type #1 or #2 둘 중에 하나만 사용
                MAlignerPickUp_MAlignerXY_MoveType1_Widely_DoneCheck,       //  M-Aligner XY 축, Module 을 들어올리기 위해 열어주는 위치로 이동 완료 확인

            MAlignerPickUp_TransferZ_Move_ReadyPos2_1stStep,                //  Transfer Z 축, 대기 위치로 이동 (1단계, 현재 위치에서 Aligner Pusher 를 벗어나는 높이까지)
            MAlignerPickUp_TransferZ_Move_ReadyPos2_1stStep_DoneCheck,      //  Transfer Z 축, 대기 위치로 이동 완료 확인 (여기서 Transfer Picker 의 Vacuum On 과 Aligner 의 Vacuum Off 를 동시에 확인)

                MAlignerPickUp_MAlignerXY_MoveType2_Widely,                 //  M-Aligner XY 축, Module 을 들어올린 후 열어주는 위치로 이동 (1mm 정도) - Type #2 or #1 둘 중에 하나만 사용
                MAlignerPickUp_MAlignerXY_MoveType2_Widely_DoneCheck,       //  M-Aligner XY 축, Module 을 들어올린 후 열어주는 위치로 이동 완료 확인

            MAlignerPickUp_TransferZ_Move_ReadyPos2_2ndStep,                //  Transfer Z 축, 대기 위치로 이동 (2단계, 최종 위치)
            MAlignerPickUp_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck,      //  Transfer Z 축, 대기 위치로 이동 완료 확인
            /// <summary>
            /// M-Aligner 에서 Module Pick Up - 완료
            /// </summary>


            /// <summary>
            /// Module 을 Work Stage 에 Put Down - 시작
            /// </summary>
            WorkStage_ModulePutdown_Condition_Check,                        //  Work Stage 에 Module Put Down 조건 체크 (Transfer Picker Vacuum On Check, Stage Vacuum Off Check, Drilling Cycle : None)

            WorkStagePutDown_TransferZ_Move_ReadyPos,                       //  Transfer Z 축, 대기 위치로 이동
            WorkStagePutDown_TransferZ_Move_ReadyPos_DoneCheck,             //  Transfer Z 축, 대기 위치로 이동 완료 확인

            WorkStagePutDown_WorkStageCycle_LoadingPos_Start,               //  Work Stage, Loading 위치로 이동 Cycle 시작
            WorkStagePutDown_TransferX_Move_LoadingPos,                     //  Transfer X 축, Work Stage Loading 위치로 이동
            WorkStagePutDown_TransferX_Move_LoadingPos_DoneCheck,           //  Transfer X 축, Work Stage Loading 위치로 이동 완료 확인 (Work Stage Loading 위치로 이동 Cycle 완료 확인 후, Transfer X 이동 완료 확인)

            WorkStagePutDown_TransferZ_Move_PutDownPos_1stStep,             //  Transfer Z 축, Module Put Down 위치로 이동 (1단계, 최종 위치에서 위로 10 mm)
            WorkStagePutDown_TransferZ_Move_PutDownPos_1stStep_DoneCheck,   //  Transfer Z 축, Module Put Down 위치로 이동 완료 확인

            WorkStagePutDown_TransferZ_Move_PutDownPos_2ndStep,             //  Transfer Z 축, Module Put Down 위치로 이동 (2단계, 최종 위치)
            WorkStagePutDown_TransferZ_Move_PutDownPos_2ndStep_DoneCheck,   //  Transfer Z 축, Module Put Down 위치로 이동 완료 확인

            WorkStagePutDown_WorkStage_Vacuum_On,                           //  Work Stage, Vacuum On
            WorkStagePutDown_Transfer_PickerVacuum_Off,                     //  Transfer, Module Picker Vacuum Off (and Blow On)
            WorkStagePutDown_Transfer_PickerVacuum_OffCheck,                //  Transfer, Module Picker Vacuum Off 확인 (and Work Stage Vacuum On 확인)

            WorkStagePutDown_TransferZ_Move_ReadyPos2_1stStep,              //  Transfer Z 축, 대기 위치로 이동 (1단계, 현재 위치에서 위로 10 mm)
            WorkStagePutDown_TransferZ_Move_ReadyPos2_1stStep_DoneCheck,    //  Transfer Z 축, 대기 위치로 이동 완료 확인 (then Picker Blow Off)

            WorkStagePutDown_TransferZ_Move_ReadyPos2_2ndStep,              //  Transfer Z 축, 대기 위치로 이동 (2단계, 최종 위치)
            WorkStagePutDown_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck,    //  Transfer Z 축, 대기 위치로 이동 완료 확인
            /// <summary>
            /// Module 을 Work Stage 에 Put Down - 완료
            /// </summary>


            /// <summary>
            /// Module 을 M-Aligner 에 Put Down - 시작
            /// </summary>
            MAligner_ModulePutdown_Condition_Check,                         //  M-Aligner 에 Module Put Down 조건 체크 (Transfer Picker Vacuum On Check, M-Aligner Vacuum Off Check, M-Align Cycle : None)

            MAlignerPutDown_TransferZ_Move_ReadyPos,                        //  Transfer Z 축, 대기 위치로 이동
            MAlignerPutDown_TransferZ_Move_ReadyPos_DoneCheck,              //  Transfer Z 축, 대기 위치로 이동 완료 확인

            MAlignerPutDown_MAlignerXY_Move_Widely,                         //  M-Aligner XY 축, Module 을 내려놓을 수 있을만큼 넓히기
            MAlignerPutDown_TransferX_Move_MAlignPos,                       //  Transfer X 축, M-Aligner Put Down 위치로 이동
            MAlignerPutDown_TransferX_Move_MAlignPos_DoneCheck,             //  Transfer X 축, M-Aligner Put Down 위치로 이동 완료 확인 (M-Aligner XY 축이 넓어진 후 이동 완료 확인)

            MAlignerPutDown_TransferZ_Move_PutDownPos_1stStep,              //  Transfer Z 축, Module Put Down 위치로 이동 (1단계, 최종 위치에서 위로 10 mm)
            MAlignerPutDown_TransferZ_Move_PutDownPos_1stStep_DoneCheck,    //  Transfer Z 축, Module Put Down 위치로 이동 완료 확인

            MAlignerPutDown_TransferZ_Move_PutDownPos_2ndStep,              //  Transfer Z 축, Module Put Down 위치로 이동 (2단계, 최종 위치)
            MAlignerPutDown_TransferZ_Move_PutDownPos_2ndStep_DoneCheck,    //  Transfer Z 축, Module Put Down 위치로 이동 완료 확인

            MAlignerPutDown_MAligner_Vacuum_On,                             //  M-Aligner, Vacuum On
            MAlignerPutDown_Transfer_PickerVacuum_Off,                      //  Transfer, Module Picker Vacuum Off (and Blow On)
            MAlignerPutDown_Transfer_PickerVacuum_OffCheck,                 //  Transfer, Module Picker Vacuum Off 확인 (and M-Aligner Vacuum On 확인)

            MAlignerPutDown_TransferZ_Move_ReadyPos2_1stStep,               //  Transfer Z 축, 대기 위치로 이동 (1단계, 현재 위치에서 위로 10 mm)
            MAlignerPutDown_TransferZ_Move_ReadyPos2_1stStep_DoneCheck,     //  Transfer Z 축, 대기 위치로 이동 완료 확인 (then Picker Blow Off)

            MAlignerPutDown_TransferZ_Move_ReadyPos2_2ndStep,               //  Transfer Z 축, 대기 위치로 이동 (2단계, 최종 위치)
            MAlignerPutDown_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck,     //  Transfer Z 축, 대기 위치로 이동 완료 확인

            MAlignerPutDown_MAlign_Start,                                   //  M-Aligner 에서 Align 시작
            MAlignerPutDown_MAlign_CompleteCheck,                           //  M-Aligner 에서 Align 완료 확인 (M-Aligner Vacuum 이 Off 이면 Pick Up 하지 않음 --> 자재가 없다고 판단)
            /// <summary>
            /// Module 을 M-Aligner 에 Put Down - 완료
            /// </summary>


            Complete                                                        //  완료
        }
        #endregion


        #region Constructor
        public Bds(string strName) : base(strName)
        {
            bool ret = true;

            ParamConfig = new BdsParameterConfig();
            Config = new BdsConfig();
            //SetDispenserWork((int)DispenserWorkStatus.WORK_NONE);

            //WorkStageIndex = -1;
            //m_bLaserGetStatus_Run = false;

            //Cepheus_laser = new MyCepheusLaser();

            //m_nHomeStep = (int)Home_Step.None;
            m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;
            m_nStacker_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;

            m_bInManualMoving_SafetySensor_Detected = false;
            m_bInCycleMoving_SafetySensor_Detected = false;

            //  타이머를 쓰레드로 변경 --> 다시 타이머 사용하기로...

            //  Main Work 타이머
            timer_MainWork = new System.Windows.Forms.Timer();
            timer_MainWork.Interval = 1;                                               //  50 이었는데 10으로 변경. (50은 너무 느린 감이 없지 않아 있음. 근데 10에서 잘 될란가...?)
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
            }
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
            //    Config.ParamConfig.DriveLimit_ElevZ_when_WaferAlign = Convert.ToDouble(temp.ToString());

            //    //  구동 제한 - Vision Y 축이, Elevator Z 축과 충돌하지 않는 최대 위치
            //    NativeMethods.GetPrivateProfileString("Drive_Limit", "Vision_Y", "20.0", temp, 255, strFIle);
            //    Config.ParamConfig.DriveLimit_VisionY_NotConflictWithElevZ = Convert.ToDouble(temp.ToString());

            //    //  구동 제한 - UVW 스테이지, U 축 얼라인 제한 위치 (마이너스 방향)
            //    NativeMethods.GetPrivateProfileString("Align_Limit", "UVW_U_Minus", "-1.915", temp, 255, strFIle);
            //    Config.ParamConfig.AlignLimit_UVW_U_Minus = Convert.ToDouble(temp.ToString());

            //    //  구동 제한 - UVW 스테이지, U 축 얼라인 제한 위치 (플러스 방향)
            //    NativeMethods.GetPrivateProfileString("Align_Limit", "UVW_U_Plus", "0.085", temp, 255, strFIle);
            //    Config.ParamConfig.AlignLimit_UVW_U_Plus = Convert.ToDouble(temp.ToString());

            //    //  구동 제한 - UVW 스테이지, V 축 얼라인 제한 위치 (마이너스 방향)
            //    NativeMethods.GetPrivateProfileString("Align_Limit", "UVW_V_Minus", "-0.805", temp, 255, strFIle);
            //    Config.ParamConfig.AlignLimit_UVW_V_Minus = Convert.ToDouble(temp.ToString());

            //    //  구동 제한 - UVW 스테이지, V 축 얼라인 제한 위치 (플러스 방향)
            //    NativeMethods.GetPrivateProfileString("Align_Limit", "UVW_V_Plus", "1.195", temp, 255, strFIle);
            //    Config.ParamConfig.AlignLimit_UVW_V_Plus = Convert.ToDouble(temp.ToString());

            //    //  구동 제한 - UVW 스테이지, W 축 얼라인 제한 위치 (마이너스 방향)
            //    NativeMethods.GetPrivateProfileString("Align_Limit", "UVW_W_Minus", "-0.075", temp, 255, strFIle);
            //    Config.ParamConfig.AlignLimit_UVW_W_Minus = Convert.ToDouble(temp.ToString());

            //    //  구동 제한 - UVW 스테이지, W 축 얼라인 제한 위치 (플러스 방향)
            //    NativeMethods.GetPrivateProfileString("Align_Limit", "UVW_W_Plus", "1.925", temp, 255, strFIle);
            //    Config.ParamConfig.AlignLimit_UVW_W_Plus = Convert.ToDouble(temp.ToString());

            //    //  비전 스케일 - Manual Scale Usage
            //    NativeMethods.GetPrivateProfileString("Vision_Scale", "Manual_Scale_Use", "True", temp, 255, strFIle);
            //    Config.ParamConfig.ManualScale_Usage = temp.ToString() == "False" ? false : true;

            //    //  비전 스케일 - Lower Vision Scale X (mm)
            //    NativeMethods.GetPrivateProfileString("Vision_Scale", "Lower_Scale_X", "0.001726468", temp, 255, strFIle);
            //    Config.ParamConfig.LowerVision_Scale_X = Convert.ToDouble(temp.ToString());

            //    //  비전 스케일 - Lower Vision Scale Y (mm)
            //    NativeMethods.GetPrivateProfileString("Vision_Scale", "Lower_Scale_Y", "0.001726468", temp, 255, strFIle);
            //    Config.ParamConfig.LowerVision_Scale_Y = Convert.ToDouble(temp.ToString());

            //    //  비전 스케일 - Lower Vision Scale Invert X
            //    NativeMethods.GetPrivateProfileString("Vision_Scale", "Lower_Scale_X_Invert", "False", temp, 255, strFIle);
            //    Config.ParamConfig.LowerVision_ScaleInvert_X = temp.ToString() == "False" ? false : true;

            //    //  비전 스케일 - Lower Vision Scale Invert Y
            //    NativeMethods.GetPrivateProfileString("Vision_Scale", "Lower_Scale_Y_Invert", "False", temp, 255, strFIle);
            //    Config.ParamConfig.LowerVision_ScaleInvert_Y = temp.ToString() == "False" ? false : true;

            //    //  비전 스케일 - Upper Vision Scale X (mm)
            //    NativeMethods.GetPrivateProfileString("Vision_Scale", "Upper_Scale_X", "0.001726468", temp, 255, strFIle);
            //    Config.ParamConfig.UpperVision_Scale_X = Convert.ToDouble(temp.ToString());

            //    //  비전 스케일 - Upper Vision Scale Y (mm)
            //    NativeMethods.GetPrivateProfileString("Vision_Scale", "Upper_Scale_Y", "0.001726468", temp, 255, strFIle);
            //    Config.ParamConfig.UpperVision_Scale_Y = Convert.ToDouble(temp.ToString());

            //    //  비전 스케일 - Upper Vision Scale Invert X
            //    NativeMethods.GetPrivateProfileString("Vision_Scale", "Upper_Scale_X_Invert", "False", temp, 255, strFIle);
            //    Config.ParamConfig.UpperVision_ScaleInvert_X = temp.ToString() == "False" ? false : true;

            //    //  비전 스케일 - Upper Vision Scale Invert Y
            //    NativeMethods.GetPrivateProfileString("Vision_Scale", "Upper_Scale_Y_Invert", "False", temp, 255, strFIle);
            //    Config.ParamConfig.UpperVision_ScaleInvert_Y = temp.ToString() == "False" ? false : true;

            //    //  레티클 글래스 - 얼라인 조명 밝기값 (상부 카메라)
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass", "Upper_Vision_LightValue", "602", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleAlign_UpperVision_LightValue = Convert.ToInt16(temp.ToString());

            //    //  레티클 글래스 - 얼라인 조명 밝기값 (하부 카메라)
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass", "Lower_Vision_LightValue", "964", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleAlign_LowerVision_LightValue = Convert.ToInt16(temp.ToString());

            //    //  얼라인 - 얼라인 시 Theta 축 회전 속도 (mm/s)
            //    NativeMethods.GetPrivateProfileString("Vision_Align", "Theta_Rotation_Speed", "5.0", temp, 255, strFIle);
            //    Config.ParamConfig.Align_Theta_Velocity = Convert.ToDouble(temp.ToString());

            //    //  얼라인 - 얼라인 시 Theta 축 회전 가속도 (mm/s²)
            //    NativeMethods.GetPrivateProfileString("Vision_Align", "Theta_Rotation_Accel", "100.0", temp, 255, strFIle);
            //    Config.ParamConfig.Align_Theta_Accel = Convert.ToDouble(temp.ToString());

            //    //  얼라인 - 얼라인 시 Theta 축 회전 감속도 (mm/s²)
            //    NativeMethods.GetPrivateProfileString("Vision_Align", "Theta_Rotation_Decel", "100.0", temp, 255, strFIle);
            //    Config.ParamConfig.Align_Theta_Decel = Convert.ToDouble(temp.ToString());

            //    //  얼라인 - 얼라인 재시도 회수
            //    NativeMethods.GetPrivateProfileString("Vision_Align", "Align_Retry", "10", temp, 255, strFIle);
            //    Config.ParamConfig.Align_Retries = Convert.ToInt16(temp.ToString());

            //    //  얼라인 - 얼라인 각도 Invert
            //    NativeMethods.GetPrivateProfileString("Vision_Scale", "Align_Angle_Invert", "True", temp, 255, strFIle);
            //    Config.ParamConfig.Align_AngleInvert = temp.ToString() == "False" ? false : true;

            //    //  얼라인 - 얼라인 각도 계산 시 Atan 함수 사용
            //    NativeMethods.GetPrivateProfileString("Vision_Scale", "Align_Calc_AtanFunc", "True", temp, 255, strFIle);
            //    Config.ParamConfig.Align_ThetaCalcFunction_Atan = temp.ToString() == "False" ? false : true;

            //    //  얼라인 스테이지 - Theta 회전 반경 (mm)
            //    NativeMethods.GetPrivateProfileString("Align_Stage", "Turning_Radius", "70.0", temp, 255, strFIle);
            //    Config.ParamConfig.Align_Theta_From_RotCenter_To_UVW_Distance = Convert.ToDouble(temp.ToString());

            //    //  얼라인 스테이지 - Theta 1˚ 회전을 위한 UVW 각 축 이동량 (mm)
            //    NativeMethods.GetPrivateProfileString("Align_Stage", "Movement_Amount_1Deg_Rotation", "1.221668451", temp, 255, strFIle);
            //    Config.ParamConfig.Align_Theta_Movement_MM_Per_1Deg = Convert.ToDouble(temp.ToString());

            //    //  얼라인 이미지 저장 여부
            //    NativeMethods.GetPrivateProfileString("Align_Image", "Image_Save_Use", "True", temp, 255, strFIle);
            //    Config.ParamConfig.AlignImageSave_Usage = temp.ToString() == "False" ? false : true;

            //    //  얼라인 이미지 저장 위치 용량 부족 경고 기준치 (GB)
            //    NativeMethods.GetPrivateProfileString("Align_Image", "Image_Save_DriveSpace_Warning_Value", "10", temp, 255, strFIle);
            //    Config.ParamConfig.AlignImageSaveFolder_WarningSpace = Convert.ToDouble(temp.ToString());

            //    //  프로브 카드 클램프 타입 1 일 경우, 업다운 실린더 동작 대기 시간
            //    NativeMethods.GetPrivateProfileString("Machine_Type", "ProbeCard_ClampTypeB_CylUpDown_StableTime", "1000", temp, 255, strFIle);
            //    Config.ParamConfig.ProbeCard_ClampTypeB_CylUpDown_StableTime = Convert.ToInt16(temp.ToString());

            //    //  레티클 글래스 - 레시피 변경 시, 레티클 글래스 센터를 확인해야 작업 진행 가능
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass", "ReticleGlass_CenterCheck_forAlign", "False", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleGlass_CenterCheck_forAlign = temp.ToString() == "False" ? false : true;

            //    //  레티클 글래스 - 레티클 글래스를 확인 시 비전 카메라와 엘리베이터의 충돌 방지를 위한 Vision Y 축 이동 한계 위치. (mm, 대부분 90.0 이내)
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass", "ReticleGlass_Vision_Y_Limit", "90", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleGlass_Vision_Y_Limit = Convert.ToDouble(temp.ToString());

            //    //  PAK, 웨이퍼 Gate - Center 가 일치할 때의 Offset X
            //    NativeMethods.GetPrivateProfileString("PAK_Gate_Center", "PAK_WaferGate_Centering_Offset_X", "0", temp, 255, strFIle);
            //    Config.ParamConfig.PAK_WaferGate_Centering_Offset_X = Convert.ToDouble(temp.ToString());

            //    //  PAK, 웨이퍼 Gate - Center 가 일치할 때의 Offset Y
            //    NativeMethods.GetPrivateProfileString("PAK_Gate_Center", "PAK_WaferGate_Centering_Offset_Y", "0", temp, 255, strFIle);
            //    Config.ParamConfig.PAK_WaferGate_Centering_Offset_Y = Convert.ToDouble(temp.ToString());

            //    //  자동 로그아웃 기능 사용 여부
            //    NativeMethods.GetPrivateProfileString("Auto_LogOut", "Auto_LogOut_Usage", "False", temp, 255, strFIle);
            //    Config.ParamConfig.Auto_LogOut_Usage = temp.ToString() == "False" ? false : true;

            //    //  자동 로그아웃 설정 시간
            //    NativeMethods.GetPrivateProfileString("Auto_LogOut", "Auto_LogOut_Time", "10", temp, 255, strFIle);
            //    Config.ParamConfig.Auto_LogOut_Time = Convert.ToDouble(temp.ToString());

            //    //  얼라인 작업 시작 시 & 패킹 작업 완료시 Leak Check 기능 사용 여부
            //    NativeMethods.GetPrivateProfileString("Packing_LeakCheck", "AlignPacking_LeakCheck_Usage", "False", temp, 255, strFIle);
            //    Config.ParamConfig.AlignPacking_OP_LeakCheck_Usage = temp.ToString() == "False" ? false : true;

            //    //  패킹 작업 완료시 Leak Check 설정 시간 (sec)
            //    NativeMethods.GetPrivateProfileString("Packing_LeakCheck", "Packing_LeakCheck_Time", "30", temp, 255, strFIle);
            //    Config.ParamConfig.Packing_OP_LeakCheck_Time = Convert.ToDouble(temp.ToString());

            //    //  패킹 작업 완료 후, 제품 언로딩을 위해 엘리베이터를 내리는 거리 (mm)
            //    NativeMethods.GetPrivateProfileString("ElevatorZ", "DownDistance_AfterPacking", "20", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_ProbeCard_Down_Distance_After_Packing = Convert.ToDouble(temp.ToString());

            //    //  전면 안전센서 사용 여부
            //    NativeMethods.GetPrivateProfileString("Interlock", "Area_Sensor_Usage", "False", temp, 255, strFIle);
            //    Config.ParamConfig.AreaSensor_Usage = temp.ToString() == "False" ? false : true;

            //    //  전면 안전센서 감지 시 Servo Off 여부
            //    NativeMethods.GetPrivateProfileString("Interlock", "AreaSensor_ServoOff_Usage", "True", temp, 255, strFIle);
            //    Config.ParamConfig.AreaSensor_ServoOff_Usage = temp.ToString() == "False" ? false : true;

            //    ////  전면 안전센서 해제 후 다시 동작시키기 위해 대기하는 시간 (sec)
            //    //NativeMethods.GetPrivateProfileString("Interlock", "Area_Sensor_Release_Pause_Time", "3", temp, 255, strFIle);
            //    //Config.ParamConfig.AreaSensor_Off_Pause_Time = Convert.ToDouble(temp.ToString());

            //    //  수동패킹 모드 사용 권한 설정
            //    NativeMethods.GetPrivateProfileString("Manual_Packing", "Enable_Admin_Only", "True", temp, 255, strFIle);
            //    Config.ParamConfig.ManualPacking_Only_Admin = temp.ToString() == "False" ? false : true;



            //    //  사용 옵션과 안정화 시간

            //    //  사용 옵션 - 웨이퍼 얼라인 시작 전 PAK 공압 라인 막힘 검사 사용 여부
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "Pak_AirLineCheck_Usage", "False", temp, 255, strFIle);
            //    Config.ParamConfig.Pak_AirLineCheck_Usage = temp.ToString() == "False" ? false : true;

            //    //  사용 옵션 - 웨이퍼 얼라인 시작 전 PAK 공압 라인 막힘 검사 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "Pak_AirLineCheck_Time", "2000", temp, 255, strFIle);
            //    Config.ParamConfig.Pak_AirLineCheck_Time = Convert.ToInt16(temp.ToString());

            //    //  사용 옵션 - Packing 공압 신호 사용 여부
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "Packing_VacuumSignal_Usage", "False", temp, 255, strFIle);
            //    Config.ParamConfig.Packing_VacuumSignal_Usage = temp.ToString() == "False" ? false : true;

            //    //  사용 옵션 - Packing 공압 신호를 사용하지 않을 경우, 대기 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "Packing_VacuumSignal_Time", "5000", temp, 255, strFIle);
            //    Config.ParamConfig.Packing_VacuumSignal_Time = Convert.ToInt16(temp.ToString());

            //    //  사용 옵션 - Packing 공압 신호를 사용할 경우, 추가 가압 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "Packing_VacuumSignal_AfterTime", "5000", temp, 255, strFIle);
            //    Config.ParamConfig.Packing_VacuumSignal_AfterTime = Convert.ToInt16(temp.ToString());

            //    //  사용 옵션 - Wafer 공압 체크 사용 여부
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "Wafer_VacuumSignal_Usage", "False", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_VacuumSignal_Usage = temp.ToString() == "False" ? false : true;

            //    //  사용 옵션 - Wafer 공압을 사용하지 않을 경우, 대기 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "Wafer_VacuumSignal_Time", "1000", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_VacuumSignal_Time = Convert.ToInt16(temp.ToString());

            //    //  사용 옵션 - Thin-Chuck 감지 센서 사용 여부
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "ThinChuck_DetectSignal_Usage", "True", temp, 255, strFIle);
            //    Config.ParamConfig.ThinChuck_DetectSignal_Usage = temp.ToString() == "False" ? false : true;

            //    //  사용 옵션 - Thin-Chuck 공압 체크 사용 여부
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "ThinChuck_VacuumSignal_Usage", "True", temp, 255, strFIle);
            //    Config.ParamConfig.ThinChuck_VacuumSignal_Usage = temp.ToString() == "False" ? false : true;

            //    //  사용 옵션 - Thin-Chuck 공압을 사용하지 않을 경우, 대기 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "ThinChuck_VacuumSignal_Time", "1000", temp, 255, strFIle);
            //    Config.ParamConfig.ThinChuck_VacuumSignal_Time = Convert.ToInt16(temp.ToString());

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
            //    Config.ParamConfig.StableTime_after_PackingSignal_On = Convert.ToInt16(temp.ToString());

            //    //  안정화 시간 - Packing 공압 신호 On 후 Thin-Chuck 공압을 해제하기 위해 대기하는 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Stable_Time", "StableTime_after_PackingSignal_On_before_ThinChuck_Vacuum_Off", "100", temp, 255, strFIle);
            //    Config.ParamConfig.StableTime_after_PackingSignal_On_before_ThinChuck_Vacuum_Off = Convert.ToInt16(temp.ToString());

            //    //  안정화 시간 - Packing 공압 신호 On 후 Wafer 공압을 해제하기 위해 대기하는 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Stable_Time", "StableTime_after_PackingSignal_On_before_Wafer_Vacuum_Off", "0", temp, 255, strFIle);
            //    Config.ParamConfig.StableTime_after_PackingSignal_On_before_Wafer_Vacuum_Off = Convert.ToInt16(temp.ToString());

            //    //  안정화 시간 - Packing 작업 중, Wafer 공압 해제 후 안정화(대기) 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Stable_Time", "StableTime_after_WaferVacuumSignal_Off", "300", temp, 255, strFIle);
            //    Config.ParamConfig.StableTime_after_WaferVacuumSignal_Off = Convert.ToInt16(temp.ToString());

            //    //  안정화 시간 - Packing 을 위해 Elev. Z 축이 프로브 카드 위치까지 이동한 후 안정화 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Stable_Time", "StableTime_before_PackingSignal_On", "100", temp, 255, strFIle);
            //    Config.ParamConfig.StableTime_before_PackingSignal_On = Convert.ToInt16(temp.ToString());

            //    //  안정화 시간 - Packing 작업 중, Thin-Chuck 공압 해제 후 안정화(대기) 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Stable_Time", "StableTime_after_ThinChuckVacuumSignal_Off", "300", temp, 255, strFIle);
            //    Config.ParamConfig.StableTime_after_ThinChuckVacuumSignal_Off = Convert.ToInt16(temp.ToString());

            //    //  안정화 시간 - Wafer 얼라인 시, 마크 위치 이동 후 안정화 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Stable_Time", "WaferAlign_Move_StableTime", "500", temp, 255, strFIle);
            //    Config.ParamConfig.WaferAlign_Move_StableTime = Convert.ToInt16(temp.ToString());

            //    //  안정화 시간 - Unpacking 작업 중, Unpacking 공압 신호 On 후 Elev. Z 축을 내리기 시작할 때까지 대기 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Stable_Time", "StableTime_after_UnpackingSignal_On", "100", temp, 255, strFIle);
            //    Config.ParamConfig.StableTime_after_UnpackingSignal_On = Convert.ToInt16(temp.ToString());



            //    //  Offset && Delay - 웨이퍼 && 프로브카드 Packing 시, 저속 이동 거리 (mm)
            //    NativeMethods.GetPrivateProfileString("Offset_Delay", "Wafer_ProbeCard_PackingOffset_Distance", "10", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_ProbeCard_PackingOffset_Distance = Convert.ToDouble(temp.ToString());

            //    //  Offset && Delay - 웨이퍼 && 프로브카드 Unpacking 시, 저속 이동 거리 (mm)
            //    NativeMethods.GetPrivateProfileString("Offset_Delay", "Wafer_ProbeCard_UnpackingOffset_Distance", "10", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_ProbeCard_UnpackingOffset_Distance = Convert.ToDouble(temp.ToString());

            //    //  Offset && Delay - 웨이퍼 && 프로브카드 UnPacking 시, UnPacking 신호 인가 후 이동하는 거리 (mm)
            //    NativeMethods.GetPrivateProfileString("Offset_Delay", "Wafer_ProbeCard_UnPackingOffset_Distance", "5", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_ProbeCard_UnPackingOffset_Distance = Convert.ToDouble(temp.ToString());

            //    //  Offset && Delay - 웨이퍼 && 프로브카드 UnPacking 시, UnPacking 을 위한 Elevator Z 축 이동 거리 (기준 높이 : Packing 위치) (mm)
            //    NativeMethods.GetPrivateProfileString("Offset_Delay", "Wafer_ProbeCard_UnPackingStartOffset_Distance", "-1.5", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_ProbeCard_UnPackingStartOffset_Distance = Convert.ToDouble(temp.ToString());

            //    //  Offset && Delay - 웨이퍼 && 프로브카드 Manual Packing 시, 엘리베이터 Z 축의 1단계 Offset 거리 (mm, > 0)
            //    NativeMethods.GetPrivateProfileString("Offset_Delay", "Wafer_ProbeCard_ManualPacking_ElevZ_Offset_Distance", "30", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_ProbeCard_ManualPacking_ElevZ_Offset_Distance = Convert.ToDouble(temp.ToString());

            //    //  Offset && Delay - 웨이퍼 && 프로브카드 Manual Packing 시, 엘리베이터 Z 축의 1단계 Offset 이동 방법 (2 Step 이동 or 이동 후 대기)
            //    NativeMethods.GetPrivateProfileString("Offset_Delay", "Wafer_ProbeCard_ManualPacking_ElevZ_OffsetMove_Concept", "False", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_ProbeCard_ManualPacking_1st_Step_ElevZ_OffsetMove_Concept = temp.ToString() == "False" ? false : true;

            //    //  Offset && Delay - 웨이퍼 && 프로브카드 안전 분리 동작 시, 씬-척 낙하 방지를 위해 엘리베이터 Z 축을 올리는 위치. (Packing 위치 대비 Offset 거리 (mm, > 0)
            //    NativeMethods.GetPrivateProfileString("Offset_Delay", "Wafer_ProbeCard_SafelyUnpacking_ElevZ_Offset_Distance", "30", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_ProbeCard_SafelyUnpacking_ElevZ_Offset_Distance = Convert.ToDouble(temp.ToString());



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
            //    Config.ParamConfig.ReticleGlass_Vision_X_Pos = Convert.ToDouble(temp.ToString());

            //    //  레티클 글래스 - 레티클 글래스 확인 위치 (VIsion Y)                  ## 임의 변경 금지 ##
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass", "ReticleGlass_Vision_Y_Pos", "87", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleGlass_Vision_Y_Pos = Convert.ToDouble(temp.ToString());

            //    //  레티클 글래스 - 레티클 글래스 확인 위치 (VIsion Z)                  ## 임의 변경 금지 ##
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass", "ReticleGlass_Vision_Z_Pos", "2.73", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleGlass_Vision_Z_Pos = Convert.ToDouble(temp.ToString());

            //    //  레티클 글래스 - 레티클 글래스 확인 위치 (Elev. Z - PAK 카메라)      ## 임의 변경 금지 ##
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass", "ReticleGlass_Elev_Z_PAK_Pos", "0.0", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleGlass_Elev_Z_PAK_Pos = Convert.ToDouble(temp.ToString());

            //    //  레티클 글래스 - 레티클 글래스 확인 위치 (Elev. Z - Wafer 카메라)    ## 임의 변경 금지 ##
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass", "ReticleGlass_Elev_Z_Wafer_Pos", "0.0", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleGlass_Elev_Z_Wafer_Pos = Convert.ToDouble(temp.ToString());

            //    //  레티클 글래스 - 레티클 글래스 Wafer 이미지 Offset X        ## 임의 변경 금지 ##
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass", "ReticleGlass_WaferVision_Offset_X", "0", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleGlass_WaferVision_Offset_X = Convert.ToInt16(temp.ToString());

            //    //  레티클 글래스 - 레티클 글래스 Wafer 이미지 Offset Y        ## 임의 변경 금지 ##
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass", "ReticleGlass_WaferVision_Offset_Y", "0", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleGlass_WaferVision_Offset_Y = Convert.ToInt16(temp.ToString());



            //    //  레티클 글래스 자동 보정 - 사용 여부 (False : 사용 안함)
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass_AutoCal", "ReticleAutoCal_Usage", "False", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleAutoCal_Usage = temp.ToString() == "False" ? false : true;

            //    //  레티클 글래스 자동 보정 - 상부 비전 허용 오차 (XY, mm)
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass_AutoCal", "ReticleAutoCal_UpperVision_Allowable_XY", "0.005", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleAutoCal_UpperVision_Allowable_XY = Convert.ToDouble(temp.ToString());

            //    //  레티클 글래스 자동 보정 - 하부 비전 허용 오차 (XY, mm)
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass_AutoCal", "ReticleAutoCal_LowerVision_Allowable_XY", "0.008", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleAutoCal_LowerVision_Allowable_XY = Convert.ToDouble(temp.ToString());

            //    //  레티클 글래스 자동 보정 - 상부 카메라 얼라인 마크의 위치 평균을 계산하기 위한 측정 회수
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass_AutoCal", "ReticleAutoCal_Upper_AlignMarkCount_forAverage", "1", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleAutoCal_Upper_AlignMarkCount_forAverage = Convert.ToInt16(temp.ToString());

            //    //  레티클 글래스 자동 보정 - 하부 카메라 얼라인 마크의 위치 평균을 계산하기 위한 측정 회수
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass_AutoCal", "ReticleAutoCal_Lower_AlignMarkCount_forAverage", "1", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleAutoCal_Lower_AlignMarkCount_forAverage = Convert.ToInt16(temp.ToString());

            //    //  레티클 글래스 자동 보정 - 마크 위치 평균값 신뢰 공차 (mm)
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass_AutoCal", "ReticleAutoCal_Vision_AverageCheck_Range", "0.05", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleAutoCal_Vision_AverageCheck_Range = Convert.ToDouble(temp.ToString());

            //    //  레티클 글래스 자동 보정 - 얼라인 재시도 회수
            //    NativeMethods.GetPrivateProfileString("Reticle_Glass_AutoCal", "ReticleAutoCal_Align_Retries", "10", temp, 255, strFIle);
            //    Config.ParamConfig.ReticleAutoCal_Align_Retries = Convert.ToInt16(temp.ToString());



            //    //  PAK 얼라인 - 조명값 조정 크기 1단계
            //    NativeMethods.GetPrivateProfileString("PAK_Align", "PAKAlign_LightValue_Step1", "30", temp, 255, strFIle);
            //    Config.ParamConfig.PAKAlign_LightValue_Step1 = Convert.ToInt16(temp.ToString());

            //    //  PAK 얼라인 - 조명값 조정 크기 2단계
            //    NativeMethods.GetPrivateProfileString("PAK_Align", "PAKAlign_LightValue_Step2", "10", temp, 255, strFIle);
            //    Config.ParamConfig.PAKAlign_LightValue_Step2 = Convert.ToInt16(temp.ToString());



            //    //  인터락 - Z-Slip 기능 - 엘리베이터 Z 축의 Torque 값이 설정치를 초과할 경우 긴급정지 여부
            //    NativeMethods.GetPrivateProfileString("Interlock", "ElevZ_ESTOP_by_Torque_Usage", "False", temp, 255, strFIle);
            //    Config.ParamConfig.ElevZ_ESTOP_by_Torque_Usage = temp.ToString() == "False" ? false : true;

            //    //  인터락 - Z-Slip 기능 - 엘리베이터 Z 축을 긴급정지시키기 위한 Torque 기준값
            //    NativeMethods.GetPrivateProfileString("Interlock", "ElevZ_ESTOP_Torque_Value", "300", temp, 255, strFIle);
            //    Config.ParamConfig.ElevZ_ESTOP_Torque_Value = Convert.ToDouble(temp.ToString());



            //    //  사용 옵션 - Wafer 얼라인 후 얼라인 위치 정확성 검증 시 Wafer TOP, MID, BOTTOM 위치별 오차 비교 여부. (false : 사용 안함)
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "Wafer_Gate_PosMarginErrorCheck_After_Wafer_Align_Usage", "False", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_Gate_PosMarginErrorCheck_After_Wafer_Align_Usage = temp.ToString() == "False" ? false : true;

            //    //  사용 옵션 - Wafer 얼라인 후 얼라인 위치 정확성 검증 시 Wafer TOP, MID, BOTTOM 위치별 허용 오차 범위 (mm)
            //    NativeMethods.GetPrivateProfileString("Operation_Options", "Wafer_Gate_PosMarginErrorRange_After_Wafer_Align", "0.05", temp, 255, strFIle);
            //    Config.ParamConfig.Wafer_Gate_PosMarginErrorRange_After_Wafer_Align = Convert.ToDouble(temp.ToString());



            //    //  오프셋 - 웨이퍼 && 프로브카드 Packing 시, Latch 상태 검사 여부. (false : 검사 안함)
            //    NativeMethods.GetPrivateProfileString("Offset_Delay", "ProbeCard_LatchStatusCheck_Usage", "False", temp, 255, strFIle);
            //    Config.ParamConfig.ProbeCard_LatchStatusCheck_Usage = temp.ToString() == "False" ? false : true;

            //    //  오프셋 - 웨이퍼 && 프로브카드 Packing 시, Latch 상태 검사를 위한 Offset 거리. (mm, 기준 : Packing 위치)
            //    NativeMethods.GetPrivateProfileString("Offset_Delay", "ProbeCard_LatchStatusCheck_OffsetDistance_from_PackingPos", "20", temp, 255, strFIle);
            //    Config.ParamConfig.ProbeCard_LatchStatusCheck_OffsetDistance_from_PackingPos = Convert.ToDouble(temp.ToString());

            //    //  오프셋 - 웨이퍼 && 프로브카드 Packing 시, Latch 상태 검사를 위한 Offset 거리 이동 후 대기시간. (ms)
            //    NativeMethods.GetPrivateProfileString("Offset_Delay", "ProbeCard_LatchStatusCheck_OffsetDistance_StableTime", "1000", temp, 255, strFIle);
            //    Config.ParamConfig.ProbeCard_LatchStatusCheck_OffsetDistance_StableTime = Convert.ToInt16(temp.ToString());



            //    //  Dummy Wafer Packing 검증 - 메세지 팝업 사용 여부. (False : 사용 안함)
            //    NativeMethods.GetPrivateProfileString("Dummy_Wafer_Packing", "DummyWaferPacking_Message_Usage", "False", temp, 255, strFIle);
            //    Config.ParamConfig.DummyWaferPacking_Message_Usage = temp.ToString() == "False" ? false : true;

            //    //  Dummy Wafer Packing 검증 - 메세지 팝업 주기 모드 선택. (True : 레시피 변경 시, False : 패킹 작업 누적 회수 도달 시)
            //    NativeMethods.GetPrivateProfileString("Dummy_Wafer_Packing", "DummyWaferPacking_MessagePopup_Mode", "False", temp, 255, strFIle);
            //    Config.ParamConfig.DummyWaferPacking_MessagePopup_Mode = temp.ToString() == "False" ? false : true;

            //    //  Dummy Wafer Packing 검증 - 패킹 작업 누적 회수. (메세지 팝업 주기를 \"False\" 로 할 경우, 이 회수만큼 패킹을 진행하면 검증 요청 메세지창 팝업)
            //    NativeMethods.GetPrivateProfileString("Dummy_Wafer_Packing", "DummyWaferPacking_MessagePopup_Count", "100", temp, 255, strFIle);
            //    Config.ParamConfig.DummyWaferPacking_MessagePopup_Count = Convert.ToInt16(temp.ToString());



            //    //  패킹 방법 - 패킹 모드 선택 (True : 패킹 공압 On 후 Elev. Z 를 단계적으로 올려서 패킹, False : 패킹 높이까지 씬-척을 올려서 패킹)
            //    NativeMethods.GetPrivateProfileString("Wafer_Packing_Mode", "PackingConcept_Mode", "False", temp, 255, strFIle);
            //    Config.ParamConfig.PackingConcept_StepUp_Mode = temp.ToString() == "False" ? false : true;

            //    //  패킹 방법 - 패킹 모드 True 선택 시, 패킹 시작 오프셋. (mm, 패킹 높이에서 이 값만큼 떨어진 위치까지 Elev. Z 를 올린 후 시작)
            //    NativeMethods.GetPrivateProfileString("Wafer_Packing_Mode", "PackingConcept_StepMode_StartOffset", "5", temp, 255, strFIle);
            //    Config.ParamConfig.PackingConcept_StepMode_StartOffset = Convert.ToDouble(temp.ToString());

            //    //  패킹 방법 - 패킹 모드 True 선택 시, Elev. Z 축을 상숭 시키는 단위 거리. (mm)
            //    NativeMethods.GetPrivateProfileString("Wafer_Packing_Mode", "PackingConcept_StepMode_MoveOffset", "0.1", temp, 255, strFIle);
            //    Config.ParamConfig.PackingConcept_StepMode_MoveOffset = Convert.ToDouble(temp.ToString());

            //    //  패킹 방법 - 패킹 모드 True 선택 시, Elev. Z 축을 Step 만큼 올린 후 패킹 공압을 체크하기 위해 대기하는 시간 (ms)
            //    NativeMethods.GetPrivateProfileString("Wafer_Packing_Mode", "PackingConcept_StepMode_PackingPressure_CheckTime", "500", temp, 255, strFIle);
            //    Config.ParamConfig.PackingConcept_StepMode_PackingPressure_CheckTime = Convert.ToDouble(temp.ToString());

            //    //  패킹 방법 - 패킹 모드 True 선택 시, Elev. Z 축에 설정된 패킹 높이보다 추가로 더 올리는 거리. (mm)
            //    NativeMethods.GetPrivateProfileString("Wafer_Packing_Mode", "PackingConcept_StepMode_PackingPos_AddOffset", "0.1", temp, 255, strFIle);
            //    Config.ParamConfig.PackingConcept_StepMode_PackingPos_AddOffset = Convert.ToDouble(temp.ToString());

            //    //  패킹 방법 - 패킹 방법 - 패킹 모드 True 선택 시, Elev. Z 축을 상승 시키는 방법 선택. (True : 저속으로 연속 이동, False : 상승 단위 거리만큼 이동)
            //    NativeMethods.GetPrivateProfileString("Wafer_Packing_Mode", "PackingConcept_StepMode_StepUpMethod_Continuous", "False", temp, 255, strFIle);
            //    Config.ParamConfig.PackingConcept_StepMode_StepUpMethod_Continuous = temp.ToString() == "False" ? false : true;

            //    //  패킹 방법 - 패킹 모드 True 선택 시, Elev. Z 축을 저속으로 상승 시키면서 패킹 공압을 확인하는 모드일 경우 저속 이동 속도 (mm/s)
            //    NativeMethods.GetPrivateProfileString("Wafer_Packing_Mode", "PackingConcept_StepMode_StepUpMethod_Continuous_MoveSpeed", "0.5", temp, 255, strFIle);
            //    Config.ParamConfig.PackingConcept_StepMode_StepUpMethod_Continuous_MoveSpeed = Convert.ToDouble(temp.ToString());

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
            //        Config.Positions[m_nIndex_Ready].U = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_Ready", "Ready_V", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Ready].V = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_Ready", "Ready_W", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Ready].W = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_Ready", "Ready_EZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Ready].EZ = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_Ready", "Ready_X", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Ready].X = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_Ready", "Ready_Y", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Ready].Y = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_Ready", "Ready_VZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Ready].VZ = Convert.ToDouble(temp.ToString());
            //    }

            //    //  Load 좌표 로드
            //    if (m_nIndex_Load != -1)
            //    {
            //        NativeMethods.GetPrivateProfileString("PositionData_Load", "Load_U", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Load].U = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_Load", "Load_V", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Load].V = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_Load", "Load_W", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Load].W = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_Load", "Load_EZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Load].EZ = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_Load", "Load_X", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Load].X = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_Load", "Load_Y", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Load].Y = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_Load", "Load_VZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Load].VZ = Convert.ToDouble(temp.ToString());
            //    }

            //    //  UnLoad 좌표 로드
            //    if (m_nIndex_UnLoad != -1)
            //    {
            //        NativeMethods.GetPrivateProfileString("PositionData_UnLoad", "UnLoad_U", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_UnLoad].U = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_UnLoad", "UnLoad_V", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_UnLoad].V = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_UnLoad", "UnLoad_W", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_UnLoad].W = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_UnLoad", "UnLoad_EZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_UnLoad].EZ = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_UnLoad", "UnLoad_X", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_UnLoad].X = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_UnLoad", "UnLoad_Y", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_UnLoad].Y = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_UnLoad", "UnLoad_VZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_UnLoad].VZ = Convert.ToDouble(temp.ToString());
            //    }

            //    //  Upper Cam 좌표 로드
            //    if (m_nIndex_Reticle_UpperCam != -1)
            //    {
            //        NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "UpperCam_U", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Reticle_UpperCam].U = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "UpperCam_V", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Reticle_UpperCam].V = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "UpperCam_W", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Reticle_UpperCam].W = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "UpperCam_EZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Reticle_UpperCam].EZ = Convert.ToDouble(temp.ToString());
            //        //NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "UpperCam_X", "0.0", temp, 255, strFIle);
            //        //Config.Positions[m_nIndex_Reticle_UpperCam].X = Convert.ToDouble(temp.ToString());
            //        //NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "UpperCam_Y", "0.0", temp, 255, strFIle);
            //        //Config.Positions[m_nIndex_Reticle_UpperCam].Y = Convert.ToDouble(temp.ToString());
            //        //NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "UpperCam_VZ", "0.0", temp, 255, strFIle);
            //        //Config.Positions[m_nIndex_Reticle_UpperCam].VZ = Convert.ToDouble(temp.ToString());

            //        Config.Positions[m_nIndex_Reticle_UpperCam].X = Config.ParamConfig.ReticleGlass_Vision_X_Pos;
            //        Config.Positions[m_nIndex_Reticle_UpperCam].Y = Config.ParamConfig.ReticleGlass_Vision_Y_Pos;
            //        Config.Positions[m_nIndex_Reticle_UpperCam].VZ = Config.ParamConfig.ReticleGlass_Vision_Z_Pos;
            //    }

            //    //  Lower Cam 좌표 로드
            //    if (m_nIndex_Reticle_LowerCam != -1)
            //    {
            //        NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "LowerCam_U", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Reticle_LowerCam].U = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "LowerCam_V", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Reticle_LowerCam].V = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "LowerCam_W", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Reticle_LowerCam].W = Convert.ToDouble(temp.ToString());
            //        NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "LowerCam_EZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_Reticle_LowerCam].EZ = Convert.ToDouble(temp.ToString());
            //        //NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "LowerCam_X", "0.0", temp, 255, strFIle);
            //        //Config.Positions[m_nIndex_Reticle_LowerCam].X = Convert.ToDouble(temp.ToString());
            //        //NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "LowerCam_Y", "0.0", temp, 255, strFIle);
            //        //Config.Positions[m_nIndex_Reticle_LowerCam].Y = Convert.ToDouble(temp.ToString());
            //        //NativeMethods.GetPrivateProfileString("PositionData_ReticleGlass", "LowerCam_VZ", "0.0", temp, 255, strFIle);
            //        //Config.Positions[m_nIndex_Reticle_LowerCam].VZ = Convert.ToDouble(temp.ToString());

            //        Config.Positions[m_nIndex_Reticle_LowerCam].X = Config.ParamConfig.ReticleGlass_Vision_X_Pos;
            //        Config.Positions[m_nIndex_Reticle_LowerCam].Y = Config.ParamConfig.ReticleGlass_Vision_Y_Pos;
            //        Config.Positions[m_nIndex_Reticle_LowerCam].VZ = Config.ParamConfig.ReticleGlass_Vision_Z_Pos;
            //    }


            //    //  Top 위치의 ElevZ 좌표, VisionZ 좌표 로드
            //    if (m_nIndex_AlignPos_Top != -1)
            //    {
            //        NativeMethods.GetPrivateProfileString("PositionData_Align", "ElevZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_AlignPos_Top].EZ = Convert.ToDouble(temp.ToString()) != 0.0 ? Convert.ToDouble(temp.ToString()) : Config.Positions[m_nIndex_AlignPos_Top].EZ;
            //        NativeMethods.GetPrivateProfileString("PositionData_Align", "VisionZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_AlignPos_Top].VZ = Convert.ToDouble(temp.ToString()) != 0.0 ? Convert.ToDouble(temp.ToString()) : Config.Positions[m_nIndex_AlignPos_Top].VZ;
            //    }

            //    //  Mid 위치의 ElevZ 좌표, VisionZ 좌표 로드
            //    if (m_nIndex_AlignPos_Mid != -1)
            //    {
            //        NativeMethods.GetPrivateProfileString("PositionData_Align", "ElevZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_AlignPos_Mid].EZ = Convert.ToDouble(temp.ToString()) != 0.0 ? Convert.ToDouble(temp.ToString()) : Config.Positions[m_nIndex_AlignPos_Mid].EZ;
            //        NativeMethods.GetPrivateProfileString("PositionData_Align", "VisionZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_AlignPos_Mid].VZ = Convert.ToDouble(temp.ToString()) != 0.0 ? Convert.ToDouble(temp.ToString()) : Config.Positions[m_nIndex_AlignPos_Mid].VZ;
            //    }

            //    //  Bottom 위치의 ElevZ 좌표, VisionZ 좌표 로드
            //    if (m_nIndex_AlignPos_Bottom != -1)
            //    {
            //        NativeMethods.GetPrivateProfileString("PositionData_Align", "ElevZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_AlignPos_Bottom].EZ = Convert.ToDouble(temp.ToString()) != 0.0 ? Convert.ToDouble(temp.ToString()) : Config.Positions[m_nIndex_AlignPos_Bottom].EZ;
            //        NativeMethods.GetPrivateProfileString("PositionData_Align", "VisionZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_AlignPos_Bottom].VZ = Convert.ToDouble(temp.ToString()) != 0.0 ? Convert.ToDouble(temp.ToString()) : Config.Positions[m_nIndex_AlignPos_Bottom].VZ;
            //    }

            //    //  Left 위치의 ElevZ 좌표, VisionZ 좌표 로드
            //    if (m_nIndex_AlignPos_Left != -1)
            //    {
            //        NativeMethods.GetPrivateProfileString("PositionData_Align", "ElevZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_AlignPos_Left].EZ = Convert.ToDouble(temp.ToString()) != 0.0 ? Convert.ToDouble(temp.ToString()) : Config.Positions[m_nIndex_AlignPos_Left].EZ;
            //        NativeMethods.GetPrivateProfileString("PositionData_Align", "VisionZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_AlignPos_Left].VZ = Convert.ToDouble(temp.ToString()) != 0.0 ? Convert.ToDouble(temp.ToString()) : Config.Positions[m_nIndex_AlignPos_Left].VZ;
            //    }

            //    //  Center 위치의 ElevZ 좌표, VisionZ 좌표 로드
            //    if (m_nIndex_AlignPos_Center != -1)
            //    {
            //        NativeMethods.GetPrivateProfileString("PositionData_Align", "ElevZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_AlignPos_Center].EZ = Convert.ToDouble(temp.ToString()) != 0.0 ? Convert.ToDouble(temp.ToString()) : Config.Positions[m_nIndex_AlignPos_Center].EZ;
            //        NativeMethods.GetPrivateProfileString("PositionData_Align", "VisionZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_AlignPos_Center].VZ = Convert.ToDouble(temp.ToString()) != 0.0 ? Convert.ToDouble(temp.ToString()) : Config.Positions[m_nIndex_AlignPos_Center].VZ;
            //    }

            //    //  Right 위치의 ElevZ 좌표, VisionZ 좌표 로드
            //    if (m_nIndex_AlignPos_Right != -1)
            //    {
            //        NativeMethods.GetPrivateProfileString("PositionData_Align", "ElevZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_AlignPos_Right].EZ = Convert.ToDouble(temp.ToString()) != 0.0 ? Convert.ToDouble(temp.ToString()) : Config.Positions[m_nIndex_AlignPos_Right].EZ;
            //        NativeMethods.GetPrivateProfileString("PositionData_Align", "VisionZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_AlignPos_Right].VZ = Convert.ToDouble(temp.ToString()) != 0.0 ? Convert.ToDouble(temp.ToString()) : Config.Positions[m_nIndex_AlignPos_Right].VZ;
            //    }

            //    //  Packing 시 ElevZ 좌표 로드
            //    if (m_nIndex_PackingPos_ElevZ != -1)
            //    {
            //        NativeMethods.GetPrivateProfileString("PositionData_Packing", "ElevZ", "0.0", temp, 255, strFIle);
            //        Config.Positions[m_nIndex_PackingPos_ElevZ].EZ = Convert.ToDouble(temp.ToString()) != 0.0 ? Convert.ToDouble(temp.ToString()) : Config.Positions[m_nIndex_PackingPos_ElevZ].EZ;
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
    }
}