using QMC.Common;
using QMC.Common.Modules;
using static QMC.Common.Equipment;
using static QMC.Common.Modules.WorkStage;
using static QMC.Common.Modules.Bds;
using static QMC.Common.Modules.Vision;
using QMC.Common.Parts;
using QMC.Common.Q_Config;
using static QMC.Common.Part;
using SpiralLab.Sirius;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using QMC.Common.Vision.Tools;
using System.Collections.Generic;
using System.Timers;


namespace QMC.Common.Q_Sequence
{
    public class Sequence_VerifyScannerCameraOffset
    {
        public enum VerifyScannerCameraOffset_Step
        {
            None = 0,
            Start,                                                      //  시작

            Stage_Cal_Vacuum_On,                                       //  Stage 진공 On
            Stage_Cal_Vacuum_On_Check,                                 //  Stage 진공 On 확인

            Laser_Off,                                                  //  레이저 Off 
            Laser_Off_Check,                                            //  레이저 Off Check

            LaserShutter_Close,                                         //  레이저 Shutter Close
            LaserShutter_Close_Check,                                   //  레이저 Shutter Close 확인

            LaserFrequency_Change,                                      //  레이저 Frequency 변경
            LaserFrequency_ChangeCheck,                                 //  레이저 Frequency 변경 확인

            LaserPower_Change,                                          //  레이저 Energy 변경
            LaserPower_ChangeCheck,                                     //  레이저 Energy 변경 확인

            DustCollector_On,                                           //  집진기 On
            DustCollector_Frequency_Set,
            DustCollector_On_Check,                                     //  집진기 On 확인

            LaserShutter_Open,                                          //  레이저 Shutter Open
            LaserShutter_Open_Check,                                    //  레이저 Shutter Open 확인

            WaterLine_Open,                                             //  수로 Open
            WaterLine_Open_Check,                                       //  수로 Open Delay

            Mask_Change,                                                //  Mask 변경 (CO2)
            Mask_Change_Check,                                          //  Mask 변경 확인 (CO2)

            BETA_Change,                                               //  BET A 변경 (CO2)
            BETA_Change_Check,                                         //  BET A 변경 확인 (CO2)

            Vario_Change,                                               // Vario 변경 (CO2)
            Vario_Change_Check,                                         // Vario 변경 확인 (CO2)

            StageXY_Move_CenterPos,
            StageXY_Move_CenterPos_Check,

            //이거는 우선.. 동작 처음 시작시 물어보고 진행하자. 
            //1.	AskUserForCalibrationPlateChange 사용자에게 캘판 변경 여부를 묻는 함수입니다.
            VerifyCalibrationAreaPos,           // 캘판 위치 확인

            MapDataChange_ScannerCalMap,                                //  Scanner Calibration 위치 Map Data 로 변경
            MapDataFlagCheck_ScannerCalMap,                             //  Scanner Calibration 위치 Map Data 로 변경되었는지 확인

            StageZ_Move_LaserHeightSensorPos,                           //  Scanner Calibration 을 진행할 위치를 Laser Height Sensor 위치로 이동
            StageZ_Move_LaserHeightSensorPos_DoneCheck,                 //  20 Scanner Calibration 을 진행할 위치를 Laser Height Sensor 위치로 이동 완료 확인
            StageXY_Move_LaserHeightSensorPos,                          //  Scanner Calibration 을 진행할 위치를 Laser Height Sensor 위치로 이동
            StageXY_Move_LaserHeightSensorPos_DoneCheck,                //  Scanner Calibration 을 진행할 위치를 Laser Height Sensor 위치로 이동 완료 확인

            StageZ_Move_LaserHeightSensor_CalPos,                       //  Scanner Calibration 을 진행할 위치를 Laser Height Sensor 위치로 이동 - 2차 이동
            StageZ_Move_LaserHeightSensor_CalPos_DoneCheck,     //  Scanner Calibration 을 진행할 위치를 Laser Height Sensor 위치로 이동 완료 확인 - 2차 이동

            Move_LaserHeightSensorPos_StableTime,                       //  Scanner Calibration 을 진행할 위치를 Laser Height Sensor 위치로 이동 후 안정화 시간 대기
            ScannerCalHeightValue_Get,                                  //  Scanner Calibration Height 측정   
            ScannerCalHeight_ZOffset_Move,                              //  Scanner Calibration 가공 높이 보정 이동
            ScannerCalHeight_ZOffset_Move_DoneCheck,                    //  Scanner Calibration 가공 높이 보정 이동 완료 확인

            StageXY_Move_ScannerCalibrationPos,                         //  Scanner Calibration 가공을 진행할 위치로 이동
            StageXY_Move_ScannerCalibrationPos_DoneCheck,               //  Scanner Calibration 가공을 진행할 위치로 이동 완료 확인

            CrossMark_MarkingStart,                                     //  Cross Mark 마킹 시작
            CrossMark_MarkingComplete,                                  //  Cross Mark 마킹 완료 확인

            MapDataChange_FineCamMap,                                   //  Fine Camera 위치 Map Data 로 변경
            MapDataFlagCheck_FineCamMap,                                //  Fine Camera 위치 Map Data 로 변경되었는지 확인

            StageXY_Move_CrossMarkCenterPos,                            //  가공된 영역의 Center 위치로 이동
            StageXY_Move_CrossMarkCenterPos_DoneCheck,                  //  가공된 영역의 Center 위치로 이동 완료 확인

            ScannerCompensation_StartPosition_Set,                      //  Scanner 보정 시작 위치 설정

            VisionCalHeight_ZOffset_Move,                              //  Vision 높이 보정 이동
            VisionCalHeight_ZOffset_Move_DoneCheck,                    //  Vision 높이 보정 이동 완료 확인

            CrossMarkCenter_MarkFind_Ready,                             //  Fine Camera 얼라인 마크 찾기 준비
            CrossMarkCenter_Find,                                       //  Cross Mark Center 찾기
            CrossMarkCenter_Find_Wait,                                  //  Cross Mark Center 찾기 대기
            CrossMarkCenter_FindResultCheck,                            //  Cross Mark Center 찾기 결과 확인
            CrossMarkCenter_XYAlignData_Calc,                           //  XY Align Data 계산
            CrossMarkCenter_XYAlign_CorrectionMove,                     //  XY 보정 이동 (카메라 Center 로)
            CrossMarkCenter_XYAlign_CorrectionMove_DoneCheck,           //  XY 보정 이동 완료 확인
            CrossMarkCenter_XYAlign_Retry,                              //  Mark 찾기부터 다시 시작

            Complete                                                    //  완료
        }

        static QMC.Common.Modules.WorkStage workStage;
        static QMC.Common.Modules.Vision vision;
        static QMC.Common.Modules.Bds bds;

        private ScannerCalConfigData m_scannerCalConfig;
        private VerifyScannerCameraOffset_Step m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.None;

        public bool m_bVerifyScannerCameraOffset_Complete { get; set; }

        #region Thread(Task)
        protected Task m_taskTimer_Main_Tick = null;
        private bool isModuleClose = false;
        protected List<Task> listTask = new List<Task>();
        public bool _isMainStatusRunning = false; // 중복 실행 방지 플래그
        public bool m_MainTick_Start = false;
        private async void Timer_MainStatus_Tick(object sender, ElapsedEventArgs e)
        {
            // 중복 실행 방지
            if (_isMainStatusRunning)
            {
                return;
            }

            try
            {
                _isMainStatusRunning = true;

                if (!m_MainTick_Start)
                {
                    return;
                }

                // Home 잡기 전에는 Device 알람 X
                if (!workStage.m_bHomeOK)
                {
                    return;
                }

                // 장비 구동 상태 체크 : true: 장비 구동 중, false: 장비 정지 중
                if (Equipment.AutoRunStatus)
                {
                }
                else
                {
                }

                SeqVerifyScannerCameraOffset();

            }
            catch (Exception ex)
            {
                Log.Write(ex);
                Console.WriteLine($"Error in Timer_Main Work_Elapsed: {ex.Message}");
            }
            finally
            {
                _isMainStatusRunning = false; // 플래그 해제
            }
        }


        #endregion


        //생성자
        public Sequence_VerifyScannerCameraOffset()
        {
            

        }
        //소멸자
        ~Sequence_VerifyScannerCameraOffset()
        {
            // 리소스 해제 로직이 필요하다면 여기에 작성
            isModuleClose = true;
            foreach (var task in listTask)
            {
                task.Wait();
                task.Dispose();

            }
            listTask.Clear();
            m_taskTimer_Main_Tick = null;
        }

        #region Tick Count Check
        public enum TickType : int
        {
            TICK_NONE = 0,              //  0 : NONE
            TICK_VERIFY_SCANNER_CAMERA_OFFSET, //  1 : TICK_VERIFY_SCANNER_CAMERA_OFFSET
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
        private const int nVerifyScannerCameraOffsetTimeout = 60000; // 5초
        #endregion



        public void Init()
        {
            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;
            foreach (Module module in m_collectionModules)
            {
                if (module.Name == "WorkStage")
                {
                    workStage = module as QMC.Common.Modules.WorkStage;
                }
                else if (module.Name == "BDS")
                {
                    bds = module as QMC.Common.Modules.Bds;
                }
                else if (module.Name == "Vision")
                {
                    vision = module as QMC.Common.Modules.Vision;
                }
            }

            m_scannerCalConfig = ScannerCalConfigData.LoadFromIni();

            m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.None;



            //장비 RUN 진행 시 프로그램 죽을때까지 돌아야함.
            m_taskTimer_Main_Tick = Task.Factory.StartNew(() =>
            {
                Thread.CurrentThread.Name = "m_taskTimer_VerifyScannerCameraOffset_Tick";

                while (true)
                {
                    Thread.Sleep(20);

                    if (isModuleClose)
                    {
                        break;
                    }

                    Timer_MainStatus_Tick(null, null);
                }
            });
            listTask.Add(m_taskTimer_Main_Tick);
        }

        public void Start()
        {
            m_scannerCalConfig = ScannerCalConfigData.LoadFromIni();
            m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.Start;
        }
        public VerifyScannerCameraOffset_Step GetCurrentStep()
        {
            return m_VerifyScannerCameraOffsetStep;
        }
        public void SetStep(VerifyScannerCameraOffset_Step newStep)
        {
            m_VerifyScannerCameraOffsetStep = newStep;
        }
        public void Reset()
        {
            m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.None;
        }
        public bool IsStep(VerifyScannerCameraOffset_Step checkStep)
        {
            return m_VerifyScannerCameraOffsetStep == checkStep;
        }

        double m_dScannerCalPosX_Last = 0.0;
        double m_dScannerCalPosY_Last = 0.0;
        double m_dCurrentCalPosX = 0.0;
        double m_dCurrentCalPosY = 0.0;
        double m_dZOffset_SocketHeightCheck = 0.0;

        private XyCoordinate xyInterpolatedCoordinate = new XyCoordinate(0.0, 0.0);
        private XyCoordinate result = new XyCoordinate(0.0, 0.0);

        int m_nCrossMark_AlignMarkCount_Max = 3;                            //  얼라인 마크 검사 최대 회수. (평균 계산용)
        int m_nCrossMark_AlignMark_Count = 0;                               //  얼라인 마크 개수. (평균 계산용)
        PointD m_pCrossMark_AlignMarkPosition_Sum = new PointD(0, 0);       //  얼라인 마크 위치 누적. (평균 계산용)
        PointD m_pCrossMark_AlignMarkPosition_Average = new PointD(0, 0);   //  얼라인 마크 위치 누적. (평균 계산용)

        public XyCoordinate[] m_forAlign_Data = new XyCoordinate[System.Enum.GetValues(typeof(AlignParam)).Length];         //  Align 용 회전 중심 좌표 (1번 마크)

        double m_deltaX = 0.0;
        double m_deltaY = 0.0;

        int SeqVerifyScannerCameraOffset()
        {
            // Scanner Vision Offset Setting 사용 여부
            Equipment.Scanner_Vision_Offset_Setting_Use = true;

            int nRtn = 0;
            string strTemp = string.Empty;
            double lfVelocity = 0;
            double lfAccDec = 0;

            double m_dHeightOffsetVision = Equipment.Scanner_Calibration_VisionZOffset; //Height Offset -> Camera
            double m_dHeightOffsetScanner = 0.0; //Height Offset -> Laser
            bool bCalPosition = Equipment.Scanner_Calibration_Position_Enable; // true: cal판, false 중앙
            bool bCalChagne = Equipment.Scanner_Calibration_Change;    //캘리브레이션 변경 여부
            if (Equipment.AutoManualStatus &&
               (Equipment.AutoRunStatus || Equipment.AutoManualStatus))
            {
                bCalPosition = true;    // 캘리브레이션 위치 설정 여부 : 무조건 Cal판.
                bCalChagne = false;
            }

            switch ((int)m_VerifyScannerCameraOffsetStep)
            {
                case (int)VerifyScannerCameraOffset_Step.None:
                    {
                        //m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.Start;
                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.Start:
                    {
                        m_bVerifyScannerCameraOffset_Complete = false;

                        int ch1Val = Equipment.Scanner_Calibration_Illumination_Red_Value; //m_scannerCalConfig.Scanner_Calibration_Illumination_Red_Value;
                        int ch2Val = Equipment.Scanner_Calibration_Illumination_IR_Value; //m_scannerCalConfig.Scanner_Calibration_Illumination_IR_Value;
                        int exposureTime = Equipment.Scanner_Calibration_ExposureTime_High; //m_scannerCalConfig.Scanner_Calibration_ExposureTime_High;
                        workStage.SetLightingByChannel(LightingChannel.CoarseCamIR, 0, false);
                        workStage.SetLightingByChannel(LightingChannel.CoarseCamRed, 0, false);
                        Thread.Sleep(100);
                        workStage.SetLightingByChannel(LightingChannel.FineCamRed, ch1Val);
                        workStage.SetLightingByChannel(LightingChannel.FineCamIR, ch2Val);

                        // 카메라 노출 설정
                        workStage.jigAligner_HighRes.Camera.SetExposureTime(exposureTime);

                        m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.Stage_Cal_Vacuum_On;
                        Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", "Laser&Scanner Calibration Start");
                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.Stage_Cal_Vacuum_On:                                              //  레이저 Off
                    {
                        if (bCalPosition)
                        {
                            workStage.workStageParameter.DO_Laser_CalSheet_Vacuum(true);
                        }
                        else
                        {
                            workStage.workStageParameter.DO_Stage_Vacuum(true);
                        }
                        Thread.Sleep(100); // 진공이 안정화 될 때까지 잠시 대기

                        TickCount_Start((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET);
                        m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.Stage_Cal_Vacuum_On_Check;
                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.Stage_Cal_Vacuum_On_Check:                                              //  레이저 Off
                    {
                        bool bRtn= false;
                        if (bCalPosition)
                        {
                            bRtn = workStage.workStageParameter.DI_Laser_CalSheet_Vacuum_Check();
                        }
                        else
                        {
                            bRtn = bRtn = workStage.workStageParameter.DI_Stage_Vacuum_Check();
                        }

                        if (bRtn)
                        {
                            Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", "Stage 진공 On 확인");
                            m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.Laser_Off;
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET) > nVerifyScannerCameraOffsetTimeout)
                        {
                            Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", "Stage 진공 On 실패");
                            m_VerifyScannerCameraOffsetStep = (int)VerifyScannerCameraOffset_Step.None;
                            return workStage.AlarmPost(WorkStage.AlarmKey.StageCal_Vacuum_On_Fail);
                        }

                        TickCount_Start((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET);
                        m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.Laser_Off;
                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.Laser_Off:                                              //  레이저 Off
                    {
                        if (workStage.rtc != null)
                        {
                            workStage.rtc.CtlLaserOff();
                        }

                        TickCount_Start((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET);
                        m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.Laser_Off_Check;
                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.Laser_Off_Check:                                                  //  레이저 Off 확인
                    {
                        if (workStage.rtc != null)
                        {
                            if (workStage.rtc.CtlGetStatus(RtcStatus.NotBusy))
                            {
                                if (Equipment.Machine_LaserType_CO2)
                                {
                                    m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.DustCollector_On;
                                }
                                else
                                {
                                    m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.LaserPower_Change;
                                }
                            }
                            else if (TickCount_Elapsed((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET) > nVerifyScannerCameraOffsetTimeout)
                            {
                                Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", "Laser Off Check 실패.");

                                m_VerifyScannerCameraOffsetStep = (int)VerifyScannerCameraOffset_Step.None;
                                return workStage.AlarmPost(WorkStage.AlarmKey.eRTC_FAIL);
                            }
                        }
                        else
                        {
                            strTemp = string.Format("Laser Off Check 실패. (Laser Comm 열리지 않음)");
                            Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", strTemp);

                            m_VerifyScannerCameraOffsetStep = (int)VerifyScannerCameraOffset_Step.None;
                            return workStage.AlarmPost(WorkStage.AlarmKey.eLaserComm_NotOpen);
                        }
                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.LaserPower_Change:                        //  레이저 Power 변경
                    {
                        if ((workStage.m_rapidLxLaser_Comm != null) && (Equipment.Scanner_Calibration_LaserEnergy > 0.0))
                        {
                            if (workStage.m_rapidLxLaser_Comm.IsOpen)
                            {
                                workStage.RapidLxLaserComm_Laser_OutputEnergy_Set(Equipment.Scanner_Calibration_LaserEnergy);
                                TickCount_Start((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET);

                                strTemp = string.Format("Laser Power 변경 시작, Laser Power ({0:0.000})", 
                                                        Equipment.Scanner_Calibration_LaserEnergy);
                                Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", strTemp);

                                m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.LaserPower_ChangeCheck;
                            }
                            else
                            {
                                strTemp = string.Format("Laser Power 변경 실패. (Laser Comm 열리지 않음)");
                                Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", strTemp);

                                m_VerifyScannerCameraOffsetStep = (int)VerifyScannerCameraOffset_Step.None;
                                return workStage.AlarmPost(WorkStage.AlarmKey.LaserPowerChange_Fail);
                                
                            }
                        }
                        else
                        {
                            strTemp = string.Format("Laser Power 변경 실패. (Laser Comm 준비되지 않았거나, 변경 출력이 0)");
                            Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", strTemp);

                            m_VerifyScannerCameraOffsetStep = (int)VerifyScannerCameraOffset_Step.None;
                            return workStage.AlarmPost(WorkStage.AlarmKey.LaserPowerChange_Fail);
                        }
                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.LaserPower_ChangeCheck:
                    {
                        const double epsilon = 0.001;// 1e-6;
                        if (Math.Abs(Equipment.Scanner_Calibration_LaserEnergy - workStage.m_dLaserComm_ReadSetValue_EnergyPercent) < epsilon)
                        {
                            strTemp = string.Format("Laser Energy 변경 완료 확인 성공, Laser Energy ({0:0.000})", 
                                                    Equipment.Scanner_Calibration_LaserFrequency);
                            Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", strTemp);
                            m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.DustCollector_On;
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET) > nVerifyScannerCameraOffsetTimeout)
                        {
                            strTemp = string.Format("Laser Energy 변경 완료 확인 실패");
                            Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", strTemp);

                            m_VerifyScannerCameraOffsetStep = (int)VerifyScannerCameraOffset_Step.None;
                            return workStage.AlarmPost(WorkStage.AlarmKey.LaserPowerChange_Fail);
                        }
                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.DustCollector_On:                                                //  Dust Collector On
                    {
                        //Cal Pan에 하면 이거 할 필요가 있나?
                        workStage.DustCollector_On((int)nDustCollector.DustCollector_Upper);
                        Thread.Sleep(100);
                        workStage.DustCollector_On((int)nDustCollector.DustCollector_Lower);
                        Thread.Sleep(100);

                        TickCount_Start((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET);
                        m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.DustCollector_Frequency_Set;
                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.DustCollector_Frequency_Set:

                    if (TickCount_Elapsed((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET) > 1000)
                    {
                        //임의로 freq시 지정.
                        double m_dFreq_Upper = 30;
                        double m_dFreq_Lower = 30;

                        workStage.DustCollectorComm_Send_SetFrequency((int)WorkStage.nDustCollector.DustCollector_Upper, m_dFreq_Upper);
                        Thread.Sleep(100);
                        workStage.DustCollectorComm_Send_SetFrequency((int)WorkStage.nDustCollector.DustCollector_Lower, m_dFreq_Lower);
                        Thread.Sleep(100);

                        TickCount_Start((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET);
                        m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.DustCollector_On_Check;
                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.DustCollector_On_Check:
                    {
                        if ((workStage.workStageParameter.DI_DustCollector_Fan_Run((int)nDustCollector.DustCollector_Upper) &&
                            workStage.workStageParameter.DI_DustCollector_Fan_Run((int)nDustCollector.DustCollector_Lower)) ||
                            (TickCount_Elapsed((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET) > DustCollector_TurnOn_AfterStableTime))
                        {
                            Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", "집진기 On 확인");
                            m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.LaserShutter_Open;
                        }
                        else if (workStage.workStageParameter.DI_DustCollector_Fan_Fault((int)nDustCollector.DustCollector_Upper) ||
                            workStage.workStageParameter.DI_DustCollector_Fan_Fault((int)nDustCollector.DustCollector_Lower))
                        {
                            Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", "집진기 알람 발생");

                            m_VerifyScannerCameraOffsetStep = (int)VerifyScannerCameraOffset_Step.None;
                            return workStage.AlarmPost(AlarmKey.eDustCollectorFail);
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET) > 120000)
                        {
                            Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", "집진기 On 실패");

                            m_VerifyScannerCameraOffsetStep = (int)VerifyScannerCameraOffset_Step.None;
                            return workStage.AlarmPost(AlarmKey.eDustCollectorFail);
                        }
                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.LaserShutter_Open:    //  Shutter Open
                    {
                        //상부 파워메타로 Shutter 사용 중?
                        workStage.workStageParameter.DO_BDS_PowerMeter_BW(true);
                        workStage.workStageParameter.DO_BDS_PowerMeter_FW(false);

                        TickCount_Start((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET);
                        m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.LaserShutter_Open_Check;
                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.LaserShutter_Open_Check:                                         //  Shutter Open 확인
                    {
                        if (workStage.workStageParameter.DI_BDS_PowerMeter_BW_Check() && 
                            !workStage.workStageParameter.DI_BDS_PowerMeter_FW_Check())
                        {
                            m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.WaterLine_Open;
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET) > nVerifyScannerCameraOffsetTimeout)
                        {
                            strTemp = string.Format("Power Meter (Shutter) Open 실패");
                            Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", strTemp);

                            m_VerifyScannerCameraOffsetStep = (int)VerifyScannerCameraOffset_Step.None;
                            return workStage.AlarmPost(AlarmKey.eBeamShutterOpenFail);

                        }
                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.WaterLine_Open:                                                //  Water Line Open
                    {
                        workStage.workStageParameter.DO_BeamDump_Coolant_Supply(true);                    //  Laser Cooling Valve Open
                        workStage.workStageParameter.DO_Scanner_Coolant_Supply(true);                     //  Scanner Cooling Valve Open

                        if (Equipment.Machine_LaserType_CO2)
                        {
                            workStage.workStageParameter.DO_Mask_Coolant_Supply(true);                    //  Beam Mask 
                            workStage.workStageParameter.DO_VarioScan_Coolant_Supply(true);
                        }

                        TickCount_Start((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET);
                        m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.WaterLine_Open_Check;
                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.WaterLine_Open_Check:                                         //  Water Line Open 확인
                    {
                        if (workStage.workStageParameter.IsDO_BeamDump_Coolant_Supply() &&
                            workStage.workStageParameter.IsDO_Scanner_Coolant_Supply() &&
                            (!Equipment.Machine_LaserType_CO2 ||
                            Equipment.Machine_LaserType_CO2 &&
                            workStage.workStageParameter.IsDO_Mask_Coolant_Supply() &&
                            workStage.workStageParameter.IsDO_VarioScan_Coolant_Supply()))
                        {
                            TickCount_Start((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET);

                            if (Equipment.Machine_LaserType_CO2)
                            {
                                m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.Mask_Change;
                            }
                            else
                            {
                                m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.StageXY_Move_CenterPos;
                            }
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET) > nVerifyScannerCameraOffsetTimeout)
                        {
                            strTemp = string.Format("Water Supply Line Open 실패");
                            Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", strTemp);

                            m_VerifyScannerCameraOffsetStep = (int)VerifyScannerCameraOffset_Step.None;
                            return workStage.AlarmPost(AlarmKey.WaterLine_Open_Fail);

                        }
                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.Mask_Change:

                    //LaserDrillingStepMaskChange(out lfVelocity, out lfAccDec);
                    workStage.workStageParameter.stWorkStagePosParam = workStage.workStageParameter.GetPositionInformation("Processing");

                    //  속도 설정
                    lfVelocity = Equipment.stAxisParam[(int)Bds.nAxis.MASK_Y].Common_Speed_Coarse;
                    lfAccDec = Equipment.stAxisParam[(int)Bds.nAxis.MASK_Y].Common_Acceleration_Coarse;

                    // Recipe에서 설정한 Y축 Mask 위치로 이동 - 
                    workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.MASK_Y] =
                        bds.stBDSTeachingPos[(int)Equipment.Scanner_Calibration_MaskIndex].Mask_Y;

                    workStage.MC_Func.MC_MovePosition((int)Bds.nAxis.MASK_Y, workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.MASK_Y],
                                          lfVelocity, lfAccDec, lfAccDec);

                    TickCount_Start((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET);
                    m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.Mask_Change_Check;

                    break;

                case (int)VerifyScannerCameraOffset_Step.Mask_Change_Check:

                    if (workStage.MC_Func.MC_GetDone((int)Bds.nAxis.MASK_Y) &&
                        workStage.MC_Func.MC_PosTolerance((int)Bds.nAxis.MASK_Y, workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.MASK_Y]))
                    {
                        workStage.m_nBETChange_RetryCount = 0;
                        m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.BETA_Change;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET) > nVerifyScannerCameraOffsetTimeout)
                    {
                        Log.Write("VerifyScannerCameraOffset", Equipment.User_Name, "VerifyScannerCameraOffset", "Mask Y 축, Mask 로 이동 실패. (Timeout)");
                        //  알람 정지 (LED Bar - Red Blink)
                        m_VerifyScannerCameraOffsetStep = (int)VerifyScannerCameraOffset_Step.None;
                        return workStage.AlarmPost(AlarmKey.MaskY_Axis_Fail);
                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.BETA_Change:

                    //int m_nBETIndex = Equipment.stLayerRecipeSet[0].Miscellaneous_BETPositionIndex; // BET 배율 설정 필요.
                    int m_nBETIndex = Equipment.Scanner_Calibration_BETPositionIndex; // BET 배율 설정 필요.
                    if (m_nBETIndex < 0 || m_nBETIndex >= 5)                                    //  BET 배율은 총4개로 고정되어 있음.
                    {
                        Log.Write("VerifyScannerCameraOffset", Equipment.User_Name, "VerifyScannerCameraOffset", "지정되지 않은 BET Index 입니다. (0 ~ 4)");

                        m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.None;
                        return workStage.AlarmPost(AlarmKey.eBETIndexFail);
                    }
                    else
                    {
                        workStage.LaserDrillingStepBETChange(m_nBETIndex);

                        TickCount_Start((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET);
                        m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.BETA_Change_Check;
                    }

                    break;

                case (int)VerifyScannerCameraOffset_Step.BETA_Change_Check:

                    if (TickCount_Elapsed((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET) > 500)
                    {
                        m_nBETIndex = Equipment.Scanner_Calibration_BETPositionIndex; //Equipment.stLayerRecipeSet[0].Miscellaneous_BETPositionIndex;
                        double m_dBET_Zoom = 0.0;
                        double m_dBET_Mrad = 0.0;
                        //  BET Zoom, Mrad 변경
                        switch (m_nBETIndex)
                        {
                            case 0:         //  0.8x
                                m_dBET_Zoom = 0.8;
                                m_dBET_Mrad = Equipment.BET_0_8X_Mrad;
                                break;
                            case 1:         //  0.9x
                                m_dBET_Zoom = 0.9;
                                m_dBET_Mrad = Equipment.BET_0_9X_Mrad;
                                break;
                            case 2:         //  1.0x
                                m_dBET_Zoom = 1.0;
                                m_dBET_Mrad = Equipment.BET_1_0X_Mrad;
                                break;
                            case 3:         //  1.1x
                                m_dBET_Zoom = 1.1;
                                m_dBET_Mrad = Equipment.BET_1_1X_Mrad;
                                break;
                            case 4:         //  1.2x
                                m_dBET_Zoom = 1.2;
                                m_dBET_Mrad = Equipment.BET_1_2X_Mrad;
                                break;
                        }

                        if (((workStage.m_dBET_ZoomValue > (m_dBET_Zoom - 0.005)) && (workStage.m_dBET_ZoomValue < (m_dBET_Zoom + 0.005))) &&
                            ((workStage.m_dBET_MradValue > (m_dBET_Mrad - 0.005)) && (workStage.m_dBET_MradValue < (m_dBET_Mrad + 0.005))))
                        {
                            strTemp = string.Format("BET Zoom ({0} / {1}), Mrad ({2} / {3}) 변경 성공", workStage.m_dBET_ZoomValue, m_dBET_Zoom, workStage.m_dBET_MradValue, m_dBET_Mrad);
                            Log.Write("VerifyScannerCameraOffset", Equipment.User_Name, "VerifyScannerCameraOffset", strTemp);

                            TickCount_Start((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET);
                            m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.Vario_Change;
                        }
                        else
                        {
                            if (workStage.m_nBETChange_RetryCount++ < 3)
                            {
                                strTemp = string.Format("BET Zoom ({0} / {1}), Mrad ({2} / {3}) 변경 실패. 재시도 ({4}/{5})", 
                                    workStage.m_dBET_ZoomValue, m_dBET_Zoom, workStage.m_dBET_MradValue, m_dBET_Mrad, workStage.m_nBETChange_RetryCount, 3);
                                Log.Write("VerifyScannerCameraOffset", Equipment.User_Name, "ScannerCalibration", strTemp);

                                m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.BETA_Change;
                                Thread.Sleep(200);
                            }
                            else
                            {
                                strTemp = string.Format("BET Zoom ({0} / {1}), Mrad ({2} / {3}) 변경 실패. 재시도 ({4}/{5})",
                                    workStage.m_dBET_ZoomValue, m_dBET_Zoom, workStage.m_dBET_MradValue, m_dBET_Mrad, workStage.m_nBETChange_RetryCount, 3);
                                Log.Write("VerifyScannerCameraOffset", Equipment.User_Name, "ScannerCalibration", strTemp);

                                m_VerifyScannerCameraOffsetStep = (int)VerifyScannerCameraOffset_Step.None;
                                return workStage.AlarmPost(AlarmKey.eBETChangeFail);
                            }
                        }
                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.Vario_Change:

                    if(true)
                    {
                        m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.StageXY_Move_CenterPos;
                    }
                    else
                    {
                        bds.spiralLabVario.fSetZOffset = 0;
                        bds.spiralLabVario.SetZOffset(bds.spiralLabVario.fSetZOffset);    // 설정값 받아와서 셋팅 필요.

                        TickCount_Start((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET);
                        m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.Vario_Change_Check;
                    }

                    break;

                case (int)VerifyScannerCameraOffset_Step.Vario_Change_Check:

                    if (bds.spiralLabVario.GetCurrentZOffset() == bds.spiralLabVario.fSetZOffset)
                    {
                        TickCount_Start((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET);
                        m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.StageXY_Move_CenterPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET) > nVerifyScannerCameraOffsetTimeout)
                    {
                        Log.Write("VerifyScannerCameraOffset", Equipment.User_Name, "VerifyScannerCameraOffset", "Vario 이동 실패. (Timeout)");

                        m_VerifyScannerCameraOffsetStep = (int)VerifyScannerCameraOffset_Step.None;
                        return workStage.AlarmPost(AlarmKey.Vario_Scan_Fail);
                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.StageXY_Move_CenterPos:
                    workStage.MovetoWorkStage_TeachingPositionsXY((int)WorkStage_TeachingPosList.STAGE_ProcessingPos, Type_Motor_Speed.Coarse);
                    TickCount_Start((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET);
                    m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.StageXY_Move_CenterPos_Check;
                    break;

                case (int)VerifyScannerCameraOffset_Step.StageXY_Move_CenterPos_Check:

                    if (workStage.IsWorkStage_TeachingPositionsXY((int)WorkStage_TeachingPosList.STAGE_ProcessingPos))
                    {
                        m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.VerifyCalibrationAreaPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET) > nVerifyScannerCameraOffsetTimeout * 10)
                    {
                        strTemp = string.Format("Stage XY Move Center Position 실패");
                        Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", strTemp);
                        
                        m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.None;
                        return workStage.AlarmPost(AlarmKey.eStageMoveFail);
                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.VerifyCalibrationAreaPos:
                    {
                        // cal center 기준 위치로 계산하고 이동하자.
                        double dScannerCalTeachingPosX = 0.0;
                        double dScannerCalTeachingPosY = 0.0;
                        if (bCalPosition)
                        {
                            dScannerCalTeachingPosX = workStage.stWorkStageTeachingPos[(int)WorkStage_TeachingPosList.STAGE_Scanner_CalPos].Stage_X;
                            dScannerCalTeachingPosY = workStage.stWorkStageTeachingPos[(int)WorkStage_TeachingPosList.STAGE_Scanner_CalPos].Stage_Y;
                        }
                        else
                        {
                            dScannerCalTeachingPosX = workStage.stWorkStageTeachingPos[(int)WorkStage_TeachingPosList.STAGE_ProcessingPos].Stage_X;
                            dScannerCalTeachingPosY = workStage.stWorkStageTeachingPos[(int)WorkStage_TeachingPosList.STAGE_ProcessingPos].Stage_Y;
                        }

                        double dScannerCalAreaWidth = Equipment.Scanner_Calibration_CalAreaWidth;
                        double dScannerCalAreaheight = Equipment.Scanner_Calibration_CalAreaHeight;
                        double dCalPitchOffset = Equipment.Scanner_Calibration_CalPitch;
                        double AreaCenterX = dScannerCalTeachingPosX;
                        double AreaCenterY = dScannerCalTeachingPosY;

                        // [변경] X 영역은 왼쪽부터 시작, Y는 센터 기준 위쪽부터
                        double dScannerCalAreaPosX_Min = AreaCenterX - (dScannerCalAreaWidth / 2) + dCalPitchOffset;
                        double dScannerCalAreaPosX_Max = AreaCenterX + (dScannerCalAreaWidth / 2) - dCalPitchOffset;

                        double dScannerCalAreaPosY_Min = AreaCenterY - (dScannerCalAreaheight / 2) + dCalPitchOffset;
                        double dScannerCalAreaPosY_Max = AreaCenterY + (dScannerCalAreaheight / 2) - dCalPitchOffset;

                        // 현재 하고자 하는 캘 사이즈 계산을 위한 값
                        int nRow = Equipment.Scanner_Calibration_rowCount;
                        int nCol = Equipment.Scanner_Calibration_colCount;
                        float fRowInterval = (float)Equipment.Scanner_Calibration_rowInterval;
                        float fColInterval = (float)Equipment.Scanner_Calibration_colInterval;

                        if (Equipment.Scanner_Vision_Offset_Setting_Use == true)
                        {
                            nRow = 1;
                            nCol = 1;
                            fRowInterval = 1;
                            fColInterval = 1;
                        }

                        double dCalWidth = (nCol - 1) * fColInterval;
                        double dCalHeight = (nRow - 1) * fRowInterval;
                        //double dCurrentCalCenterX = dCalWidth / 2;
                        //double dCurrentCalCenterY = dCalHeight / 2;

                        Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset",
                                $"[Cal 영역] X Range = {dScannerCalAreaPosX_Min:F3} ~ {dScannerCalAreaPosX_Max:F3}, " +
                                $"Y Range = {dScannerCalAreaPosY_Min:F3} ~ {dScannerCalAreaPosY_Max:F3}");

                        Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset",
                                $"[Last 위치] X = {Equipment.Scanner_Calibration_PosX_Last:F3}, Y = {Equipment.Scanner_Calibration_PosY_Last:F3}");

                        if (bCalChagne)
                        {
                            m_dCurrentCalPosX = dScannerCalTeachingPosX;// m_dScannerCalPosX_Last;
                            m_dCurrentCalPosY = dScannerCalTeachingPosY;// - 10;
                            Equipment.Scanner_Calibration_Change = false;
                            bCalChagne = false;
                        }
                        else
                        {
                            m_dCurrentCalPosX = Equipment.Scanner_Calibration_PosX_Last - (dCalWidth + dCalPitchOffset);
                            m_dCurrentCalPosY = Equipment.Scanner_Calibration_PosY_Last;

                            if (m_dCurrentCalPosX < dScannerCalAreaPosX_Min)
                            {
                                // 다음 Y 줄로 이동
                                m_dCurrentCalPosX = dScannerCalAreaPosX_Max;
                                m_dCurrentCalPosY = Equipment.Scanner_Calibration_PosY_Last - dCalPitchOffset;

                                Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset",
                                    $"X 범위 초과로 다음 Y줄 이동 시도 → X: {m_dCurrentCalPosX:F3}, Y: {m_dCurrentCalPosY:F3}");

                                if (m_dCurrentCalPosY < dScannerCalAreaPosY_Min)
                                {
                                    strTemp = string.Format("캘판 범위 모두 처리 완료. 캘판을 교체해 주세요.");
                                    Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", strTemp);
                                    m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.None;
                                    return workStage.AlarmPost(AlarmKey.Scan_Area_Fail);
                                }
                            }
                        }

                        // 최종 위치가 유효한지 검사
                        if (m_dCurrentCalPosX < dScannerCalAreaPosX_Min || m_dCurrentCalPosX > dScannerCalAreaPosX_Max ||
                            m_dCurrentCalPosY < dScannerCalAreaPosY_Min || m_dCurrentCalPosY > dScannerCalAreaPosY_Max)
                        {
                            strTemp = string.Format("캘판 범위 벗어났습니다. 캘판을 교체해 주세요.");
                            Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", strTemp);
                            m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.None;
                            return workStage.AlarmPost(AlarmKey.Scan_Area_Fail);
                        }

                        Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset",
                            $"[현재 가공 위치] X = {m_dCurrentCalPosX:F3}, Y = {m_dCurrentCalPosY:F3}");

                        m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.MapDataChange_ScannerCalMap;

                        //기존 코드 주석
                        {
                            //// cal center 기준 위치로 계산하고 이동하자.
                            //double dScannerCalTeachingPosX = 0.0;
                            //double dScannerCalTeachingPosY = 0.0;
                            //if (bCalPosition)
                            //{
                            //    dScannerCalTeachingPosX = workStage.stWorkStageTeachingPos[(int)WorkStage_TeachingPosList.STAGE_Scanner_CalPos].Stage_X;
                            //    dScannerCalTeachingPosY = workStage.stWorkStageTeachingPos[(int)WorkStage_TeachingPosList.STAGE_Scanner_CalPos].Stage_Y;
                            //}
                            //else
                            //{
                            //    dScannerCalTeachingPosX = workStage.stWorkStageTeachingPos[(int)WorkStage_TeachingPosList.STAGE_ProcessingPos].Stage_X;
                            //    dScannerCalTeachingPosY = workStage.stWorkStageTeachingPos[(int)WorkStage_TeachingPosList.STAGE_ProcessingPos].Stage_Y;
                            //}

                            //double dScannerCalAreaWidth = Equipment.Scanner_Calibration_CalAreaWidth;
                            //double dScannerCalAreaheight = Equipment.Scanner_Calibration_CalAreaHeight;
                            //double dCalPitchOffset = Equipment.Scanner_Calibration_CalPitch;
                            //double AreaCenterX = dScannerCalTeachingPosX;
                            //double AreaCenterY = dScannerCalTeachingPosY;

                            //// [변경] X 영역은 왼쪽부터 시작, Y는 센터 기준 위쪽부터
                            //double dScannerCalAreaPosX_Min = AreaCenterX - (dScannerCalAreaWidth / 2) + dCalPitchOffset;
                            //double dScannerCalAreaPosX_Max = AreaCenterX + (dScannerCalAreaWidth / 2) - dCalPitchOffset;

                            //double dScannerCalAreaPosY_Min = AreaCenterY - (dScannerCalAreaheight / 2) + dCalPitchOffset;
                            //double dScannerCalAreaPosY_Max = AreaCenterY + (dScannerCalAreaheight / 2) - dCalPitchOffset;

                            //// 현재 하고자 하는 캘 사이즈 계산을 위한 값
                            //int nRow = Equipment.Scanner_Calibration_rowCount;
                            //int nCol = Equipment.Scanner_Calibration_colCount;
                            //float fRowInterval = (float)Equipment.Scanner_Calibration_rowInterval;
                            //float fColInterval = (float)Equipment.Scanner_Calibration_colInterval;

                            //if (Equipment.Scanner_Vision_Offset_Setting_Use == true)
                            //{
                            //    nRow = 1;
                            //    nCol = 1;
                            //    fRowInterval = 1;
                            //    fColInterval = 1;
                            //}

                            //double dCalWidth = (nCol - 1) * fColInterval;
                            //double dCalHeight = (nRow - 1) * fRowInterval;
                            //double dCurrentCalCenterX = dCalWidth / 2;
                            //double dCurrentCalCenterY = dCalHeight / 2;

                            //// [변경] 캘판 교체 시 시작 위치는 티칭 기준 중앙에서 왼쪽으로 반 너비만큼 이동
                            //if (bCalChagne) //캘판 교체시.
                            //{
                            //    m_dScannerCalPosX_Last = AreaCenterX + (dCalWidth / 2);  // [변경]
                            //    m_dScannerCalPosY_Last = AreaCenterY;                    // [변경]
                            //}

                            //// 영역 계산 로그
                            //Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset",
                            //    $"[Cal 영역] X Range = {dScannerCalAreaPosX_Min:F3} ~ {dScannerCalAreaPosX_Max:F3}, " +
                            //    $"Y Range = {dScannerCalAreaPosY_Min:F3} ~ {dScannerCalAreaPosY_Max:F3}");

                            //Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset",
                            //    $"[Last 위치] X = {m_dScannerCalPosX_Last:F3}, Y = {m_dScannerCalPosY_Last:F3}");

                            //// 캘 영역 벗어나는지 검사
                            //if (m_dScannerCalPosX_Last < dScannerCalAreaPosX_Min || m_dScannerCalPosX_Last > dScannerCalAreaPosX_Max ||
                            //    m_dScannerCalPosY_Last < dScannerCalAreaPosY_Min || m_dScannerCalPosY_Last > dScannerCalAreaPosY_Max)
                            //{
                            //    strTemp = string.Format("캘판 범위 벗어났습니다. 캘판을 교체해 주세요.");
                            //    Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", strTemp);
                            //    m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.None;
                            //    return workStage.AlarmPost(AlarmKey.ScannerCalibration_Fail);
                            //}
                            //else
                            //{
                            //    if (bCalChagne) // [변경] 캘판 교체 후 첫 위치는 중앙 티칭 위치에서 계산됨
                            //    {
                            //        m_dCurrentCalPosX = m_dScannerCalPosX_Last;
                            //        m_dCurrentCalPosY = m_dScannerCalPosY_Last;
                            //        Equipment.Scanner_Calibration_Change = false;
                            //        bCalChagne = false;
                            //    }
                            //    else
                            //    {
                            //        // [변경] 이후부터는 X축 방향으로만 피치 간격 이동
                            //        //m_dCurrentCalPosX = m_dScannerCalPosX_Last + dCalWidth + dCalPitchOffset; // X만 증가
                            //        m_dCurrentCalPosX = m_dScannerCalPosX_Last - (dCalWidth + dCalPitchOffset); // ➖ 방향
                            //        m_dCurrentCalPosY = m_dScannerCalPosY_Last; // Y 고정

                            //        Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset",
                            //        $"[현재 가공 위치] X = {m_dCurrentCalPosX:F3}, Y = {m_dCurrentCalPosY:F3}");
                            //    }

                            //    // [유지] 이동할 위치가 cal area를 벗어나는지 확인
                            //    if (m_dCurrentCalPosX < dScannerCalAreaPosX_Min || m_dCurrentCalPosX > dScannerCalAreaPosX_Max ||
                            //        m_dCurrentCalPosY < dScannerCalAreaPosY_Min || m_dCurrentCalPosY > dScannerCalAreaPosY_Max)
                            //    {
                            //        strTemp = string.Format("캘판 범위 벗어났습니다. 캘판을 교체해 주세요.");
                            //        Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", strTemp);
                            //        m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.None;
                            //        return workStage.AlarmPost(AlarmKey.ScannerCalibration_Fail);

                            //    }
                            //    else
                            //    {
                            //        //범위 안에 있다면, 다음 위치로 이동.
                            //        //이동할 위치를 가지고 가자.
                            //        //다음 캘리브레이션 위치로 이동.
                            //        // 정상동작 완료하고 Pos값 넣고 저장하자.... 아니지... Laser 쏘고 완료 되면 
                            //        // 저장이다. 한 번 Laser 발진 한 곳은 그냥 끝. 
                            //        // m_dScannerCalPosX_Last <- 이 위치가.. Vision cal 할 수 있는 위치가 되겠다..
                            //        //m_dScannerCalPosX_Last = m_dCurrentCalPosX;
                            //        //m_dScannerCalPosY_Last = m_dCurrentCalPosY;

                            //        m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.MapDataChange_ScannerCalMap;
                            //    }
                            //}
                        }
                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.MapDataChange_ScannerCalMap:
                    {
                        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        //  맵 데이터 변경 (기준위치 : Scanner)
                        //  기준위치로 보낼 때, 맵데이터를 변경한 후 보낸다.
                        //  그 외에는, 위치로 보낸 후 맵데이터를 변경한다.
                        //MapData_Apply((int)nMapData_Type.MapData_Stage_Scanner);            //그냥 이거 사용하면 되지 않나?
                        //MapData_Apply((int)nMapData_Type.MapData_Stage_CalPos_Scanner);
                        m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.MapDataFlagCheck_ScannerCalMap;
                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.MapDataFlagCheck_ScannerCalMap:                                   //  Scanner Calibration 위치 Map Data 로 변경되었는지 확인
                    {
                        m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.StageZ_Move_LaserHeightSensorPos;
                    }
                    break;

                // 가공 높이 측정 후 Mark 그려야 함.
                case (int)VerifyScannerCameraOffset_Step.StageZ_Move_LaserHeightSensorPos:
                    {
                        // Z-Axis :: CO2 -> 아크릴 높이 감안하여 cal 확인시에는 높이를 따로 둔다. ( stage쪽에서는 높이 다름 )
                        m_dZOffset_SocketHeightCheck = 0.0;
                        int nZPos = 0;
                        if(bCalPosition)
                        {
                            nZPos = (int)QMC.Common.Modules.Vision.Vision_TeachingPosList.Laser_Sensor_HeightCheck_CalPos;
                        }
                        else
                        {
                            nZPos = (int)QMC.Common.Modules.Vision.Vision_TeachingPosList.Laser_Sensor_HeightCheckPos;
                        }

                        workStage.MovetoWorkStage_TeachingPositionsZ(nZPos, Type_Motor_Speed.Fine);
                        TickCount_Start((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET);
                        m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.StageZ_Move_LaserHeightSensorPos_DoneCheck;
                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.StageZ_Move_LaserHeightSensorPos_DoneCheck:
                    {
                        int nZPos = 0;
                        if (bCalPosition)
                        {
                            nZPos = (int)QMC.Common.Modules.Vision.Vision_TeachingPosList.Laser_Sensor_HeightCheck_CalPos;
                        }
                        else
                        {
                            nZPos = (int)QMC.Common.Modules.Vision.Vision_TeachingPosList.Laser_Sensor_HeightCheckPos;
                        }

                        if (workStage.IsWorkStage_TeachingPositionsZ((int)nZPos))
                        {
                            m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.StageXY_Move_LaserHeightSensorPos;
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET) >= nVerifyScannerCameraOffsetTimeout)
                        {
                            strTemp = string.Format("Stage Z 축, Laser Height Check 높이로 이동 실패");
                            Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", strTemp);
                            m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.None;
                            return workStage.AlarmPost(AlarmKey.eZAxisFail);
                        }
                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.StageXY_Move_LaserHeightSensorPos:
                    {
                        result.X = 0.0;
                        result.Y = 0.0;
                        result.X += m_dCurrentCalPosX;
                        result.Y += m_dCurrentCalPosY;
                        //  좌표계 변환 (Scanner 위치 --> Fine Camera 위치)
                        result.X -= Equipment.stOffsetDistance.FromScannerToFineCam.X;
                        result.Y -= Equipment.stOffsetDistance.FromScannerToFineCam.Y;
                        //  좌표계 변환 (Fine Camera 위치 --> Laser Height Sensor 위치)
                        result.X += Equipment.stOffsetDistance.FromFineCamToLaserHeightSensor.X;
                        result.Y += Equipment.stOffsetDistance.FromFineCamToLaserHeightSensor.Y;
                        //  좌표계 변환 (Laser Height Sensor 위치 --> 높이 측정 위치에 XY Offset 반영)
                        result.X += Equipment.stLayerRecipeSet[0].ProcessOption_SocketHeightCheckPos_OffsetX;
                        result.Y += Equipment.stLayerRecipeSet[0].ProcessOption_SocketHeightCheckPos_OffsetY;

                        xyInterpolatedCoordinate = result;
                        //  속도 설정
                        if (false)
                        {
                            workStage.MovetoWorkStage_ABS_PositionsXY(xyInterpolatedCoordinate, Type_Motor_Speed.Process);
                        }
                        else
                        {
                            workStage.MovetoWorkStage_ABS_PositionsXY(xyInterpolatedCoordinate, Type_Motor_Speed.Coarse);
                        }
                        //workStage.MovetoWorkStage_ABS_PositionsXY(xyInterpolatedCoordinate, Type_Motor_Speed.Coarse);
                        TickCount_Start((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET);
                        m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.StageXY_Move_LaserHeightSensorPos_DoneCheck;
                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.StageXY_Move_LaserHeightSensorPos_DoneCheck:
                    {
                        if (workStage.IsWorkStage_Positions(WorkStage.nAxis.X, xyInterpolatedCoordinate.X) &&
                           workStage.IsWorkStage_Positions(WorkStage.nAxis.Y, xyInterpolatedCoordinate.Y))
                        {
                            TickCount_Start((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET);
                            m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.Move_LaserHeightSensorPos_StableTime;
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET) > nVerifyScannerCameraOffsetTimeout * 5)
                        {
                            strTemp = string.Format("Stage XY축, 가공 Center 위치로 이동 실패. (Timeout)");
                            Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", strTemp);
                            m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.None;
                            return workStage.AlarmPost(AlarmKey.eStageMoveFail);
                        }
                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.Move_LaserHeightSensorPos_StableTime:
                    {
                        if (TickCount_Elapsed((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET) > 1000)
                        {
                            m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.ScannerCalHeightValue_Get;
                        }

                        //주석
                        {
                            //if (Equipment.Machine_LaserHeightCheckStableTime_Enable)
                            //{
                            //    if (TickCount_Elapsed((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET) > Equipment.Machine_LaserHeightCheckStableTime)
                            //    {

                            //        workStage.m_bSensorRequestPending = true;
                            //        workStage.m_bSensorResponseReady = false;
                            //        TickCount_Start((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET);
                            //        m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.ScannerCalHeightValue_Get;
                            //    }
                            //}
                            //else //Enable ; false 시에 안정화 시간 없이 값 읽어옴.
                            //{
                            //    workStage.m_bSensorRequestPending = true;
                            //    workStage.m_bSensorResponseReady = false;
                            //    TickCount_Start((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET);
                            //    m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.ScannerCalHeightValue_Get;
                            //}
                        }

                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.ScannerCalHeightValue_Get:
                    {
                        // Manual도 측정해야 함.
                        //if (!workStage.m_bSensorResponseReady)
                        //{
                        //    // 아직 응답 안옴 → 대기 or 타임아웃 처리
                        //    if (TickCount_Elapsed((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET) > 1000)
                        //    {
                        //        //알람 처리 해야 할 수도.
                        //        m_dZOffset_SocketHeightCheck = 0.0;
                        //    }
                        //    break;
                        //}
                        //workStage.m_bSensorResponseReady = false; // 응답 소비 완료

                        //  Laser Focus 위치에서 Laser Height 값과, 현재 Laser Height Sensor 값의 차이만큼 가공 높이 보정
                        //m_dZOffset_SocketHeightCheck = Equipment.LaserHeightSensor_ReferenceValue_atScannerFocusPosition - m_dLaserHeightSensorSocket_Value;
                        if ((workStage.m_dLaserHeightSensorSocket_Value < -4.5) || 
                            (workStage.m_dLaserHeightSensorSocket_Value > 5.5) || 
                            (workStage.m_dLaserHeightSensorSocket_Value < -99.9))
                        {
                            // Laser Height Sensor 값이 비정상적인 경우 알람 발생해야 할 것 같은데..
                            strTemp = string.Format("Laser Height Sensor 값이 비정상적입니다. (측정값: {0:F3})", workStage.m_dLaserHeightSensorSocket_Value);
                            Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", strTemp);
                            //여기 들어오면 티칭 위치 바꿔야 함.
                            m_dZOffset_SocketHeightCheck = 0.0;
                        }
                        else
                        {
                            m_dZOffset_SocketHeightCheck = workStage.m_dLaserHeightSensorSocket_Value - Equipment.LaserHeightSensor_ReferenceValue_atScannerFocusPosition;
                        }

                        Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", $"변위Data: Z={m_dZOffset_SocketHeightCheck}");
                        m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.ScannerCalHeight_ZOffset_Move;
                    }
                    break;

                // 십자가를 그릴 가공 높이로 이동 + Offset 
                case (int)VerifyScannerCameraOffset_Step.ScannerCalHeight_ZOffset_Move:
                    {
                        //  Scanner Cal 진행 시 Z Offset 값이 있으면 적용하자.
                        m_dHeightOffsetScanner = 0;
                        int nZPos = 0;
                        if (bCalPosition)
                        {
                            nZPos = (int)QMC.Common.Modules.Vision.Vision_TeachingPosList.Laser_Sensor_HeightCheck_CalPos;
                        }
                        else
                        {
                            nZPos = (int)QMC.Common.Modules.Vision.Vision_TeachingPosList.Laser_Sensor_HeightCheckPos;
                        }

                        double dPosZ = vision.stVisionTeachingPos[nZPos].Vision_Z + m_dZOffset_SocketHeightCheck + m_dHeightOffsetScanner;

                        if(false)
                        {
                            workStage.MovetoWorkStage_ABS_PositionsZ(dPosZ, Type_Motor_Speed.Fine);
                        }

                        Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset",
                        $"LaserZAxis: {dPosZ}" +
                            $"LaserFocusPos={vision.stVisionTeachingPos[(int)QMC.Common.Modules.Vision.Vision_TeachingPosList.Laser_Sensor_HeightCheck_CalPos].Vision_Z}" +
                            $"HeightCheck={m_dZOffset_SocketHeightCheck}" +
                            $"OffsetPos={m_dHeightOffsetScanner}");
                        TickCount_Start((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET);
                        m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.ScannerCalHeight_ZOffset_Move_DoneCheck;
                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.ScannerCalHeight_ZOffset_Move_DoneCheck:
                    {
                        if(false)
                        {
                            int nZPos = 0;
                            if (bCalPosition)
                            {
                                nZPos = (int)QMC.Common.Modules.Vision.Vision_TeachingPosList.Laser_Sensor_HeightCheck_CalPos;
                            }
                            else
                            {
                                nZPos = (int)QMC.Common.Modules.Vision.Vision_TeachingPosList.Laser_Sensor_HeightCheckPos;
                            }

                            double dPosZ = vision.stVisionTeachingPos[nZPos].Vision_Z + m_dZOffset_SocketHeightCheck + m_dHeightOffsetScanner;

                            if (workStage.IsWorkStage_Positions(WorkStage.nAxis.Z, dPosZ))
                            {
                                double targetZ = workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.Z];
                                Log.Write("VerifyScannerCameraOffset", Equipment.User_Name, "VerifyScannerCameraOffset",
                                    $"Stage Z 축, Z Offset 이동 완료 확인 (Target Z: {targetZ:F3})");

                                m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.StageXY_Move_ScannerCalibrationPos;
                            }
                            else if (TickCount_Elapsed((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET) > nVerifyScannerCameraOffsetTimeout)
                            {
                                strTemp = string.Format("Stage Z 축, Socket 가공 Focus 조정 실패. (Timeout)");
                                Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", strTemp);

                                m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.None;
                                return workStage.AlarmPost(AlarmKey.eZAxisFail);
                            }
                        }

                        m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.StageXY_Move_ScannerCalibrationPos;
                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.StageXY_Move_ScannerCalibrationPos:
                    {
                        xyInterpolatedCoordinate.X = m_dCurrentCalPosX;
                        xyInterpolatedCoordinate.Y = m_dCurrentCalPosY;

                        // 선택 기능 넣어야 겠다. 
                        if (bCalPosition)
                        {
                            // 캘판 위에서 캘할때!
                            workStage.MapData_Apply((int)nMapData_Type.MapData_Stage_CalPos_Scanner);
                        }
                        else
                        {
                            workStage.MapData_Apply((int)nMapData_Type.MapData_Stage_Scanner);
                        }

                        //  속도 설정
                        if (false)
                        {
                            workStage.MovetoWorkStage_ABS_PositionsXY(xyInterpolatedCoordinate, Type_Motor_Speed.Process);
                        }
                        else
                        {
                            workStage.MovetoWorkStage_ABS_PositionsXY(xyInterpolatedCoordinate, Type_Motor_Speed.Coarse);
                        }
                        //workStage.MovetoWorkStage_ABS_PositionsXY(xyInterpolatedCoordinate, Type_Motor_Speed.Coarse);

                        TickCount_Start((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET);
                        m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.StageXY_Move_ScannerCalibrationPos_DoneCheck;
                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.StageXY_Move_ScannerCalibrationPos_DoneCheck:
                    {
                        if (workStage.IsWorkStage_Positions(WorkStage.nAxis.X, xyInterpolatedCoordinate.X) &&
                           workStage.IsWorkStage_Positions(WorkStage.nAxis.Y, xyInterpolatedCoordinate.Y))
                        {
                            Thread.Sleep(500); // 안정화 시간으로 500msec 줘보자. (비교)
                            m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.CrossMark_MarkingStart;
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET) > nVerifyScannerCameraOffsetTimeout * 10)
                        {
                            strTemp = string.Format("Stage XY 축, Stage Center 위치로 이동 실패. (Timeout)");
                            Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", strTemp);
                            m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.None;
                            return workStage.AlarmPost(AlarmKey.eStageMoveFail);
                        }
                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.CrossMark_MarkingStart:
                    {
                        //  Cross Mark Marking 시작. //  Cross Mark Marking 함수 넣자.
                        //float fieldsize = Equipment.Scanner_Calibration_FieldSize; //정사각형 Cal. X,Y Size 동일.
                        int nRow = Equipment.Scanner_Calibration_rowCount;
                        int nCol = Equipment.Scanner_Calibration_colCount;
                        float fRowInterval = (float)Equipment.Scanner_Calibration_rowInterval;
                        float fColInterval = (float)Equipment.Scanner_Calibration_colInterval;
                        double dMarkLength = Equipment.Scanner_Calibration_CrossMarkLength;

                        if (Equipment.Scanner_Vision_Offset_Setting_Use == true)
                        {
                            // 1x1로 설정.: Laser <-> Scanner Offset Setting
                            nRow = 1;
                            nCol = 1;
                            fRowInterval = 1;
                            fColInterval = 1;
                        }

                        //Laser & Scanner 준비 상태 확인 필요. (발진 가능 여부 및 셋팅 값)
                        if (Equipment.Machine_LaserType_CO2)
                        {
                            if (Equipment.Scanner_Vision_Offset_Setting_Use == true)
                            {
                                //걍 무조건 Closs로
                                workStage.DrawCalibrationCrosses(nRow, nCol, fRowInterval, fColInterval, dMarkLength);
                            }
                            //else
                            //{
                            //    if (Equipment.Scanner_Calibration_MarkType_Cross)
                            //    {
                            //        workStage.DrawCalibrationCrosses(nRow, nCol, fRowInterval, fColInterval, dMarkLength);
                            //    }
                            //    else //Circle
                            //    {
                            //        workStage.DrawCalibrationArc(nRow, nCol, fRowInterval, fColInterval);
                            //    }
                            //}
                        }
                        else //UV
                        {
                            if (Equipment.Scanner_Vision_Offset_Setting_Use == true)
                            {
                                //걍 무조건 Closs로
                                workStage.DrawCalibrationCrosses(nRow, nCol, fRowInterval, fColInterval, dMarkLength);
                            }
                            //else
                            //{
                            //    if (Equipment.Scanner_Calibration_MarkType_Cross)
                            //    {
                            //        workStage.DrawCalibrationCrosses(nRow, nCol, fRowInterval, fColInterval, dMarkLength);
                            //    }
                            //    else //Circle
                            //    {
                            //        workStage.DrawCalibrationArc(nRow, nCol, fRowInterval, fColInterval);
                            //    }
                            //}
                        }

                        TickCount_Start((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET);
                        m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.CrossMark_MarkingComplete;
                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.CrossMark_MarkingComplete:
                    {
                        if (workStage.rtc.CtlGetStatus(RtcStatus.NotBusy))
                        {
                            Equipment.Scanner_Calibration_PosX_Last = m_dCurrentCalPosX;
                            Equipment.Scanner_Calibration_PosY_Last = m_dCurrentCalPosY;
                            workStage.Scanner_Calibration_Option_Save();
                            //m_dScannerCalPosX_Last = m_dCurrentCalPosX;
                            //m_dScannerCalPosY_Last = m_dCurrentCalPosY;
                            //workStage.Scanner_Calibration_Option_Save();

                            Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", "VerifyScannerCameraOffset, Cross Mark 가공 완료");
                            m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.MapDataChange_FineCamMap;
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET) > nVerifyScannerCameraOffsetTimeout)
                        {
                            strTemp = string.Format("Cross Mark Marking 실패");
                            Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", strTemp);
                            m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.None;
                            return workStage.AlarmPost(AlarmKey.eRTC_FAIL);

                        }
                    }
                    break;
                // 여기부터는 Fine Camera Seq.
                case (int)VerifyScannerCameraOffset_Step.MapDataChange_FineCamMap:
                    {
                        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        //  맵 데이터 변경 (기준위치 : Scanner)
                        //  기준위치로 보낼 때, 맵데이터를 변경한 후 보낸다.
                        //  그 외에는, 위치로 보낸 후 맵데이터를 변경한다.
                        //MapData_Apply((int)nMapData_Type.MapData_Stage_CalPos_FineCam);

                        m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.MapDataFlagCheck_FineCamMap;
                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.MapDataFlagCheck_FineCamMap: //  Fine Camera 위치 Map Data 로 변경되었는지 확인
                    {
                        m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.StageXY_Move_CrossMarkCenterPos;
                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.StageXY_Move_CrossMarkCenterPos:
                    {
                        xyInterpolatedCoordinate.X =
                            workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X) - Equipment.stOffsetDistance.FromScannerToFineCam.X;
                        xyInterpolatedCoordinate.Y =
                            workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y) - Equipment.stOffsetDistance.FromScannerToFineCam.Y;

                        if (bCalPosition)
                        {
                            workStage.MapData_Apply((int)nMapData_Type.MapData_Stage_CalPos_FineCam);
                        }
                        else
                        {
                            workStage.MapData_Apply((int)nMapData_Type.MapData_Stage_FineCam);
                        }

                        //  속도 설정
                        if (false)
                        {
                            workStage.MovetoWorkStage_ABS_PositionsXY(xyInterpolatedCoordinate, Type_Motor_Speed.Process);
                        }
                        else
                        {
                            workStage.MovetoWorkStage_ABS_PositionsXY(xyInterpolatedCoordinate, Type_Motor_Speed.Coarse);
                        }
                        //workStage.MovetoWorkStage_ABS_PositionsXY(xyInterpolatedCoordinate, Type_Motor_Speed.Coarse);

                        TickCount_Start((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET);
                        m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.StageXY_Move_CrossMarkCenterPos_DoneCheck;
                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.StageXY_Move_CrossMarkCenterPos_DoneCheck:
                    {
                        if (workStage.IsWorkStage_Positions(WorkStage.nAxis.X, xyInterpolatedCoordinate.X) &&
                           workStage.IsWorkStage_Positions(WorkStage.nAxis.Y, xyInterpolatedCoordinate.Y))
                        {
                            Log.Write("VerifyScannerCameraOffset", "Scanner Calibration", "Stage XY축, 가공 Center 위치로 이동 완료.");
                            Thread.Sleep(500); // 안정화 시간으로 500msec 줘보자. (비교)

                            TickCount_Start((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET);
                            m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.VisionCalHeight_ZOffset_Move;
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET) > nVerifyScannerCameraOffsetTimeout * 5)
                        {
                            strTemp = string.Format("Stage XY축, 가공 Center 위치로 이동 실패. (Timeout)");
                            Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", strTemp);
                            m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.None;
                            return workStage.AlarmPost(AlarmKey.eStageMoveFail);
                        }
                    }
                    break;

                // Z축 Vision 위치로 이동
                case (int)VerifyScannerCameraOffset_Step.VisionCalHeight_ZOffset_Move:
                    {
                        // Vision 진행 시 Z Offset 값이 있으면 적용하자.
                        m_dHeightOffsetVision = Equipment.Scanner_Calibration_VisionZOffset;

                        //Laser_Sensor_HeightCheck_CalPos
                        //double dPosZ = vision.stVisionTeachingPos[(int)QMC.Common.Modules.Vision.Vision_TeachingPosList.Laser_Sensor_HeightCheck_CalPos].Vision_Z +
                        //    m_dZOffset_SocketHeightCheck + m_dHeightOffsetVision;
                        int nZPos = 0;
                        if (bCalPosition)
                        {
                            nZPos = (int)QMC.Common.Modules.Vision.Vision_TeachingPosList.Laser_Sensor_HeightCheck_CalPos;
                        }
                        else
                        {
                            nZPos = (int)QMC.Common.Modules.Vision.Vision_TeachingPosList.Laser_Sensor_HeightCheckPos;
                        }

                        double dPosZ = vision.stVisionTeachingPos[nZPos].Vision_Z + m_dHeightOffsetVision;

                        workStage.MovetoWorkStage_ABS_PositionsZ(dPosZ, Type_Motor_Speed.Fine);

                        Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset",
                        $"VisionZAxis: {dPosZ}" +
                            $"VisionFocusPos={vision.stVisionTeachingPos[(int)QMC.Common.Modules.Vision.Vision_TeachingPosList.Laser_Sensor_HeightCheck_CalPos].Vision_Z}" +
                            $"HeightCheck={m_dZOffset_SocketHeightCheck}" +
                            $"OffsetPos={m_dHeightOffsetVision}");
                        TickCount_Start((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET);
                        m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.VisionCalHeight_ZOffset_Move_DoneCheck;
                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.VisionCalHeight_ZOffset_Move_DoneCheck:
                    {
                        int nZPos = 0;
                        if (bCalPosition)
                        {
                            nZPos = (int)QMC.Common.Modules.Vision.Vision_TeachingPosList.Laser_Sensor_HeightCheck_CalPos;
                        }
                        else
                        {
                            nZPos = (int)QMC.Common.Modules.Vision.Vision_TeachingPosList.Laser_Sensor_HeightCheckPos;
                        }

                        double dPosZ = vision.stVisionTeachingPos[nZPos].Vision_Z + m_dHeightOffsetVision;

                        if (workStage.IsWorkStage_Positions(WorkStage.nAxis.Z, dPosZ))
                        {
                            Log.Write("VerifyScannerCameraOffset", Equipment.User_Name, "VerifyScannerCameraOffset",
                                $"Stage Z 축, Z Offset 이동 완료 확인 (Target Z: {dPosZ:F3})");

                            m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.ScannerCompensation_StartPosition_Set;
                        }
                        else if (workStage.TickCount_Elapsed((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET) > nVerifyScannerCameraOffsetTimeout * 5)
                        {
                            strTemp = string.Format("Stage Z 축, Vision Focus 조정 실패. (Timeout)");
                            Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", strTemp);
                            m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.None;
                            return workStage.AlarmPost(AlarmKey.eZAxisFail);
                        }
                    }
                    break;

                //여기부터 카메라로 + Mark 찾아야함.
                //  Scanner 보정 시작 위치 설정
                case (int)VerifyScannerCameraOffset_Step.ScannerCompensation_StartPosition_Set:
                    {
                        // Todo : 조명 디버깅 필요 
                        int ch1Val = Equipment.Scanner_Calibration_Illumination_Red_Value;
                        int ch2Val = Equipment.Scanner_Calibration_Illumination_IR_Value;
                        int exposureTime = Equipment.Scanner_Calibration_ExposureTime_High;
                        workStage.SetLightingByChannel(Equipment.LightingChannel.CoarseCamIR, 0, false);
                        workStage.SetLightingByChannel(Equipment.LightingChannel.CoarseCamRed, 0, false);
                        Thread.Sleep(100);
                        workStage.SetLightingByChannel(Equipment.LightingChannel.FineCamRed, ch1Val);
                        workStage.SetLightingByChannel(Equipment.LightingChannel.FineCamIR, ch2Val);

                        // 카메라 노출 설정
                        workStage.jigAligner_HighRes.Camera.SetExposureTime(exposureTime);

                        XyzCoordinate currentPos = new XyzCoordinate();
                        if (workStage.scannerCompensator == null || workStage.scannerCompensator.Stage == null)
                        {
                            Log.Write("VerifyScannerCameraOffset", "ScannerCompensator or Stage is null");
                            m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.None;
                            return workStage.AlarmPost(AlarmKey.ScannerCalibration_Fail);
                        }

                        // 1. 현재 위치 획득
                        if (workStage.scannerCompensator.Stage.GetCommandPosition(ref currentPos) != 0)
                        {
                            strTemp = string.Format("Stage 현재 위치를 가져올 수 없습니다.");
                            Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", strTemp);
                            m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.None;
                            return workStage.AlarmPost(AlarmKey.ScannerCalibration_Fail);
                        }
                        // 2. Config에 현재 위치 저장
                        workStage.scannerCompensator.Config.GridPositions[(int)ScannerCompensator.GridXyMotionPositionKeys.StartPosition].Coordinate = currentPos;
                        // 3. 로그 출력 및 다음 단계
                        Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", $"Scanner 보정 시작 위치 저장 완료: X={currentPos.X:0.000}, Y={currentPos.Y:0.000}, Z={currentPos.Z:0.000}");

                        m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.CrossMarkCenter_MarkFind_Ready;
                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.CrossMarkCenter_MarkFind_Ready:
                    {
                        m_nCrossMark_AlignMarkCount_Max = 3;                            //  얼라인 마크 검사 최대 회수. (평균 계산용)
                        m_nCrossMark_AlignMark_Count = 0;                               //  얼라인 마크 개수. (평균 계산용)
                        m_pCrossMark_AlignMarkPosition_Sum = new PointD(0, 0);          //  얼라인 마크 위치 누적. (평균 계산용)
                        m_pCrossMark_AlignMarkPosition_Average = new PointD(0, 0);      //  얼라인 마크 위치 누적. (평균 계산용)

                        m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.CrossMarkCenter_Find;
                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.CrossMarkCenter_Find:
                    {
                        Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", "CrossMarkCenter_Find, Center 검출 시작");

                        Equipment.Vision_SpiralMove_Use = false;                                        //  돌면서 찾지 않음.
                        Equipment.MachineStop_byUser = false;
                        int result = 0;

                        if (workStage.scannerCompensator == null || workStage.scannerCompensator.Stage == null)
                        {
                            strTemp = string.Format("scannerCompensator 또는 Stage가 null입니다.");
                            Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", strTemp);
                            m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.None;
                            return workStage.AlarmPost(AlarmKey.ScannerCalibration_Fail);
                        }

                        //Center 찾고 진행.
                        result = workStage.scannerCompensator.RunSearchMark();
                        if (result == 1)
                        {
                            double markPositionX = workStage.scannerCompensator.ResultPosition.X;
                            double markPositionY = workStage.scannerCompensator.ResultPosition.Y;

                            Equipment.MachineStop_byUser = false;
                            Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset",
                                        $"마크 위치: X={markPositionX}, Y={markPositionY}");

                            // 다음 단계로 진행
                            TickCount_Start((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET);
                            m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.CrossMarkCenter_FindResultCheck;
                        }
                        else
                        {
                            strTemp = string.Format("마크를 찾을 수 없습니다.");
                            Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", strTemp);
                            m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.None;
                            return workStage.AlarmPost(AlarmKey.Mark_Search_Fail);
                        }
                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.CrossMarkCenter_Find_Wait:
                    {
                        m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.CrossMarkCenter_FindResultCheck;
                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.CrossMarkCenter_FindResultCheck:
                    {
                        Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", "CrossMarkCenter_FindResultCheck, Center 검출 확인");

                        // scannerCompensator에서 이전 검색 결과 가져오기
                        PatternMatchingResult result = workStage.scannerCompensator.GetResult();
                        if (result == null || result.Values.Count == 0)
                        {
                            strTemp = string.Format("반복하여 마크 서치 중 - NG (마크를 찾을 수 없음)");
                            Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", strTemp);
                            //m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.None;
                            //return workStage.AlarmPost(AlarmKey.Mark_Search_Fail);
                            m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.CrossMarkCenter_Find;

                        }
                        else
                        {
                            // 첫 번째 마크 위치 가져오기
                            //double markX = result.Values[0].X;
                            //double markY = result.Values[0].Y;
                            //ResultPosition <- 모터 값으로 가져와서 계산.
                            double markX = workStage.scannerCompensator.ResultPosition.X;
                            double markY = workStage.scannerCompensator.ResultPosition.Y;
                            if (markX == 0 || markY == 0)
                            {
                                strTemp = string.Format("반복하여 마크 서치 중 - NG (위치값 0)");
                                Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", strTemp);
                                //m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.None;
                                //return workStage.AlarmPost(AlarmKey.Mark_Search_Fail);
                                m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.CrossMarkCenter_Find;

                            }
                            else
                            {
                                m_pCrossMark_AlignMarkPosition_Sum.X += markX;
                                m_pCrossMark_AlignMarkPosition_Sum.Y += markY;

                                // 평균 계산을 위해 반복
                                m_nCrossMark_AlignMark_Count++;
                                if (m_nCrossMark_AlignMark_Count < m_nCrossMark_AlignMarkCount_Max)
                                {
                                    m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.CrossMarkCenter_Find;
                                }
                                else
                                {
                                    m_pCrossMark_AlignMarkPosition_Average.X = m_pCrossMark_AlignMarkPosition_Sum.X / m_nCrossMark_AlignMark_Count;
                                    m_pCrossMark_AlignMarkPosition_Average.Y = m_pCrossMark_AlignMarkPosition_Sum.Y / m_nCrossMark_AlignMark_Count;

                                    //300um 허용 오차 범위 내인지 확인
                                    //너무 크다. 바꾸자. 10um
                                    if (Math.Abs(m_pCrossMark_AlignMarkPosition_Average.X - markX) <= 0.01 &&
                                        Math.Abs(m_pCrossMark_AlignMarkPosition_Average.Y - markY) <= 0.01)
                                    {
                                        Log.Write("VerifyScannerCameraOffset", Equipment.User_Name, "VerifyScannerCameraOffset", "Cross Mark 평균값 OK");
                                        m_forAlign_Data[(int)AlignParam.RESULTPOS_FIRSTMARK].X = m_pCrossMark_AlignMarkPosition_Average.X;
                                        m_forAlign_Data[(int)AlignParam.RESULTPOS_FIRSTMARK].Y = m_pCrossMark_AlignMarkPosition_Average.Y;

                                        strTemp = string.Format("AvgX: {0:0.000}, Avg Y: {1:0.000}, LastX: {2:0.000}, LastY: {3:0.000}",
                                            m_pCrossMark_AlignMarkPosition_Average.X,
                                            m_pCrossMark_AlignMarkPosition_Average.Y,
                                            markX,
                                            markY);
                                        Log.Write("VerifyScannerCameraOffset", Equipment.User_Name, "VerifyScannerCameraOffset", strTemp);
                                    }
                                    else
                                    {
                                        Log.Write("VerifyScannerCameraOffset", Equipment.User_Name, "VerifyScannerCameraOffset", "Cross Mark 평균값 NG");
                                        strTemp = string.Format("AvgX: {0:0.000}, Avg Y: {1:0.000}, LastX: {2:0.000}, LastY: {3:0.000}",
                                            m_pCrossMark_AlignMarkPosition_Average.X,
                                            m_pCrossMark_AlignMarkPosition_Average.Y,
                                            markX,
                                            markY);
                                        Log.Write("VerifyScannerCameraOffset", Equipment.User_Name, "VerifyScannerCameraOffset", strTemp);

                                        m_forAlign_Data[(int)AlignParam.RESULTPOS_FIRSTMARK].X = markX;
                                        m_forAlign_Data[(int)AlignParam.RESULTPOS_FIRSTMARK].Y = markY;
                                    }

                                    m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.CrossMarkCenter_XYAlignData_Calc;
                                }
                            }
                        }

                        // 2분 동안 마크를 찾지 못할 경우
                        if (TickCount_Elapsed((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET) >= nVerifyScannerCameraOffsetTimeout * 2) 
                        {
                            workStage.MC_Func.MC_MotorStop((int)WorkStage.nAxis.X, 2000);
                            workStage.MC_Func.MC_MotorStop((int)WorkStage.nAxis.Y, 2000);
                            workStage.MC_Func.MC_MotorStop((int)WorkStage.nAxis.Z, 2000);

                            strTemp = string.Format("작업 중지. (Cross Mark 검출 시간 초과)");
                            Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", strTemp);
                            m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.None;
                            return workStage.AlarmPost(AlarmKey.ScannerCalibration_Fail);
                        }

                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.CrossMarkCenter_XYAlignData_Calc:
                    {
                        Log.Write("VerifyScannerCameraOffset", Equipment.User_Name, "Scanner Calibration", "Cross Mark XY 보정량 계산");

                        PatternMatchingResult result = workStage.scannerCompensator.GetResult();
                        if (result == null || result.Values.Count == 0)
                        {
                            strTemp = string.Format("Cross Mark 데이터를 가져오지 못했습니다.");
                            Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", strTemp);
                            m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.None;
                            return workStage.AlarmPost(AlarmKey.ScannerCalibration_Fail);
                        }
                        else
                        {
                            //현재 위치 포지션과 비교해서 해야 하는거 아닌가?
                            double dCurrentMotorPosX = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X);
                            double dCurrentMotorPosY = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y);

                            // Delta 계산
                            m_deltaX = dCurrentMotorPosX - m_pCrossMark_AlignMarkPosition_Average.X;
                            m_deltaY = dCurrentMotorPosY - m_pCrossMark_AlignMarkPosition_Average.Y;

                            // 허용 오차 값 가져오기
                            double allowableXY = 0.3;   //3.0; //Config.ParamConfig.ReticleAutoCal_UpperVision_Allowable_XY;

                            // 판정: 허용 오차 범위 내인지 확인
                            if (Math.Abs(m_deltaX) <= allowableXY &&
                                Math.Abs(m_deltaY) <= allowableXY)
                            {
                                Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset",
                                          $"Cross Mark XY 위치가 카메라 센터에서 오차범위 이내에 있음 (DeltaX: {m_deltaX}, DeltaY: {m_deltaY})");

                                if (Equipment.Scanner_Vision_Offset_Setting_Use == true)
                                {
                                    Equipment.Scanner_Vision_Offset_Setting_X = m_deltaX * 1;
                                    Equipment.Scanner_Vision_Offset_Setting_Y = m_deltaY * -1;

                                    // 수동 확인 용.
                                    if(false)
                                    {
                                        MessageBox.Show("OK: Cross Mark XY 위치.\n" +
                                        $"DeltaX: {m_deltaX}, DeltaY: {m_deltaY}", "Completed",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }

                                    // 자동일때.
                                    //  Scanner <-> Vision Offset data에 위에서 구한 Offset 적용해야함.
                                    // 적용 전 로그
                                    Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", $"[Before Offset Apply] " +
                                        $"FromScannerToFineCam.X: {Equipment.stOffsetDistance.FromScannerToFineCam.X:F6}, " +
                                        $"Y: {Equipment.stOffsetDistance.FromScannerToFineCam.Y:F6}");
                                    Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", $"[Apply Offset]" +
                                        $" Setting X: {Equipment.Scanner_Vision_Offset_Setting_X:F6}, " +
                                        $"Y: {Equipment.Scanner_Vision_Offset_Setting_Y:F6}");

                                    //  Scanner <-> FineCam Offset 적용 <- 검증 후에 적용하자.
                                    Equipment.stOffsetDistance.FromScannerToFineCam.X += Equipment.Scanner_Vision_Offset_Setting_X;
                                    Equipment.stOffsetDistance.FromScannerToFineCam.Y += Equipment.Scanner_Vision_Offset_Setting_Y;
                                    Equipment.Scanner_FineCam_Offset_Save();

                                    // 적용 후 로그
                                    Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", $"[After Offset Apply] " +
                                        $"FromScannerToFineCam.X: {Equipment.stOffsetDistance.FromScannerToFineCam.X:F6}, " +
                                        $"Y: {Equipment.stOffsetDistance.FromScannerToFineCam.Y:F6}");

                                    m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.Complete;
                                }
                            }
                            else
                            {
                                strTemp = string.Format($"Cross Mark XY 위치가 허용 오차를 벗어남 (DeltaX: {m_deltaX}, DeltaY: {m_deltaY}, Allowable: {allowableXY})");
                                Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", strTemp);

                                m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.None;
                                return workStage.AlarmPost(AlarmKey.Mark_Search_Error_Range_Fail);
                            }
                        }
                    }
                    break;

                //  XY 보정 이동 (카메라 Center 로)
                case (int)VerifyScannerCameraOffset_Step.CrossMarkCenter_XYAlign_CorrectionMove:
                    {
                        Equipment.Scanner_Vision_Offset_Setting_X = m_deltaX;
                        Equipment.Scanner_Vision_Offset_Setting_Y = m_deltaY;

                        //deltaX, deltaY값이 1mm 이상이면 실패 인터락 추가 해줘.
                        // 2mm 이상일 경우 실패 인터락 처리
                        if (Math.Abs(m_deltaX) > 1.0 || Math.Abs(m_deltaY) > 1.0)
                        {
                            strTemp = string.Format($"Cross Mark XY위치가 허용 오차를 벗어남 (DeltaX: {m_deltaX}, DeltaY: {m_deltaY})");
                            Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", strTemp);

                            m_VerifyScannerCameraOffsetStep = (int)VerifyScannerCameraOffset_Step.None;
                            return workStage.AlarmPost(AlarmKey.Mark_Search_Error_Range_Fail);
                        }
                        else
                        {
                            // 이거 검증 다시 필요!
                            xyInterpolatedCoordinate.X = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X) + m_deltaX; // (m_deltaX * -1); //X는 -
                            xyInterpolatedCoordinate.Y = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y) + m_deltaY;

                            if (bCalPosition)
                            {
                                workStage.MapData_Apply((int)nMapData_Type.MapData_Stage_CalPos_FineCam);
                            }
                            else
                            {
                                workStage.MapData_Apply((int)nMapData_Type.MapData_Stage_FineCam);
                            }

                            //  속도 설정
                            if (false)
                            {
                                workStage.MovetoWorkStage_ABS_PositionsXY(xyInterpolatedCoordinate, Type_Motor_Speed.Process);
                            }
                            else
                            {
                                workStage.MovetoWorkStage_ABS_PositionsXY(xyInterpolatedCoordinate, Type_Motor_Speed.Coarse);
                            }
                            //workStage.MovetoWorkStage_ABS_PositionsXY(xyInterpolatedCoordinate, Type_Motor_Speed.Coarse);

                            TickCount_Start((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET);
                            m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.CrossMarkCenter_XYAlign_CorrectionMove_DoneCheck;
                        }
                    }
                    break;
                case (int)VerifyScannerCameraOffset_Step.CrossMarkCenter_XYAlign_CorrectionMove_DoneCheck:
                    {
                        if (workStage.IsWorkStage_Positions(WorkStage.nAxis.X, xyInterpolatedCoordinate.X) &&
                           workStage.IsWorkStage_Positions(WorkStage.nAxis.Y, xyInterpolatedCoordinate.Y))
                        {
                            Log.Write("VerifyScannerCameraOffset", "Scanner Calibration", "Stage XY축, 보정 Center 위치로 이동 완료.");

                            TickCount_Start((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET);
                            m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.CrossMarkCenter_XYAlign_Retry;
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_VERIFY_SCANNER_CAMERA_OFFSET) > nVerifyScannerCameraOffsetTimeout)
                        {
                            strTemp = string.Format("Stage XY축, 보정 Center 위치로 이동 실패. (Timeout)");
                            Log.Write("VerifyScannerCameraOffset", "Scanner Calibration", strTemp);
                            m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.None;
                            return workStage.AlarmPost(AlarmKey.eStageMoveFail);
                        }
                    }
                    break;
                //  Mark 찾기부터 다시 시작
                case (int)VerifyScannerCameraOffset_Step.CrossMarkCenter_XYAlign_Retry:
                    {
                        if (Equipment.Scanner_Vision_Offset_Setting_Use == true)
                        {
                            m_deltaX = Equipment.Scanner_Vision_Offset_Setting_X;
                            m_deltaY = Equipment.Scanner_Vision_Offset_Setting_Y;

                            //MessageBox.Show("OK: Cross Mark XY 위치.\n" +
                            //    "DeltaX: {deltaX}, DeltaY: {deltaY}", "Completed",
                            //    MessageBoxButtons.OK, MessageBoxIcon.Information);

                            m_VerifyScannerCameraOffsetStep = VerifyScannerCameraOffset_Step.Complete;
                        }
                    }
                    break;

                case (int)VerifyScannerCameraOffset_Step.Complete:
                    {
                        strTemp = string.Format("VerifyScannerCameraOffset_Step 완료");
                        Log.Write("VerifyScannerCameraOffset", "VerifyScannerCameraOffset", strTemp);
                        
                        if(false)
                        {
                            MessageBox.Show(strTemp, "Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        m_bVerifyScannerCameraOffset_Complete = true;
                        m_VerifyScannerCameraOffsetStep = (int)VerifyScannerCameraOffset_Step.None;
                    }
                    break;
            }

            return nRtn;
        }
    }




}
