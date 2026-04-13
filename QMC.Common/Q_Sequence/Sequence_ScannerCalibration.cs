using QMC.Common.Modules;
using QMC.Common.Parts;
using QMC.Common.Q_Config;
using QMC.Common.Vision.Tools;
using SpiralLab.Sirius;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using static QMC.Common.Equipment;
using static QMC.Common.Modules.WorkStage;
using static QMC.Common.Part;

namespace QMC.Common.Q_Sequence
{
    public class Sequence_ScannerCalibration
    {
        public enum ScannerCalibrationSeq_Step
        {
            None = 0,
            Start,

            Stage_Cal_Vacuum_On,
            Stage_Cal_Vacuum_On_Check,

            Laser_Off,
            Laser_Off_Check,

            LaserShutter_Close,
            LaserShutter_Close_Check,

            LaserFrequency_Change,
            LaserFrequency_ChangeCheck,

            LaserPower_Change,
            LaserPower_ChangeCheck,

            Scanner_Init,
            Scanner_Init_Check,

            DustCollector_On,
            DustCollector_Frequency_Set,
            DustCollector_On_Check,

            LaserShutter_Open,
            LaserShutter_Open_Check,

            WaterLine_Open,
            WaterLine_Open_Check,

            Mask_Change,
            Mask_Change_Check,

            BETA_Change,
            BETA_Change_Check,

            Vario_Change,
            Vario_Change_Check,

            StageXY_Move_CenterPos,
            StageXY_Move_CenterPos_Check,

            VerifyCalibrationAreaPos,

            MapDataChange_ScannerCalMap,
            MapDataFlagCheck_ScannerCalMap,

            StageZ_Move_LaserHeightSensorPos,
            StageZ_Move_LaserHeightSensorPos_DoneCheck,
            StageXY_Move_LaserHeightSensorPos,
            StageXY_Move_LaserHeightSensorPos_DoneCheck,

            StageZ_Move_LaserHeightSensor_CalPos,
            StageZ_Move_LaserHeightSensor_CalPos_DoneCheck,

            Move_LaserHeightSensorPos_StableTime,
            ScannerCalHeightValue_Get,
            ScannerCalHeight_ZOffset_Move,
            ScannerCalHeight_ZOffset_Move_DoneCheck,

            StageXY_Move_ScannerCalibrationPos,
            StageXY_Move_ScannerCalibrationPos_DoneCheck,

            CrossMark_MarkingStart,
            CrossMark_MarkingComplete,

            MapDataChange_FineCamMap,
            MapDataFlagCheck_FineCamMap,

            StageXY_Move_CrossMarkCenterPos,
            StageXY_Move_CrossMarkCenterPos_DoneCheck,

            ScannerCompensation_StartPosition_Set,

            VisionCalHeight_ZOffset_Move,
            VisionCalHeight_ZOffset_Move_DoneCheck,

            CrossMarkCenter_MarkFind_Ready,
            CrossMarkCenter_Find,
            CrossMarkCenter_Find_Wait,
            CrossMarkCenter_FindResultCheck,
            CrossMarkCenter_XYAlignData_Calc,
            CrossMarkCenter_XYAlign_CorrectionMove,
            CrossMarkCenter_XYAlign_CorrectionMove_DoneCheck,
            CrossMarkCenter_XYAlign_Retry,

            StageXY_Move_ScannerCalibration_RightTopPos,
            StageXY_Move_ScannerCalibration_RightTopPos_Check,

            ScannerCompensator_Start,                                   //  Scanner Compensator 시작
            ScannerCompensator_Wait,                                   //  Scanner Compensator 시작
            ScannerCompensator_Complete,                                //  Scanner Compensator 완료 확인

            ScannerCompensatedData_Check,                               //  Scanner 보정 데이터 확인 (오차 체크하여, 오차범위 이내이면 OK, 아니면 다시 보정)

            ScannerCompensatedData_Correction,                          //  Scanner 보정 데이터 보정 

            ScannerCompensatedData_CorrectionFile_Convert,              //  Scanner 보정 데이터 Convert (보정 데이터로 ct5 파일 생성)

            ScannerCompensatedData_CorrectionFile_Reset,                //  Scanner 보정 데이터 파일 리셋 (ct5 파일 다시 세팅)

            //  ct5 파일을 다시 세팅했으므로  "MapDataChange_ScannerCalMap" Step 부터 다시 진행한다. (Retry 회수 지정)


            Complete
        }

        static QMC.Common.Modules.WorkStage workStage;
        static QMC.Common.Modules.Vision vision;
        static QMC.Common.Modules.Bds bds;

        private ScannerCalConfigData m_scannerCalConfig;
        public ScannerCalibrationSeq_Step m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.None;

        public bool m_bScannerCalibration_Complete { get; set; }

        private Task _taskRunScannerCompensation;
        private int _scannerCalRetry = 1;        // 클래스 필드로 유지 (지역변수 X)
        private int _scannerCalRetryMax = 1;     // 필요시 config에서 주입

        // timeout 체크용
        private const int LaserScannerCalTimeout = 5000; // 실제값은 기존값/설정값으로 교체


        #region Thread(Task)
        protected Task m_taskTimer_Main_Tick = null;
        private bool isModuleClose = false;
        protected List<Task> listTask = new List<Task>();
        public bool _isMainStatusRunning = false;
        public bool m_MainTick_Start = false;
        private CancellationTokenSource m_mainTickCts = null;

        public bool IsMainTickAlive
        {
            get
            {
                var t = m_taskTimer_Main_Tick;
                return t != null && !(t.IsCanceled || t.IsCompleted || t.IsFaulted);
            }
        }

        private async void Timer_MainStatus_Tick(object sender, ElapsedEventArgs e)
        {
            if (_isMainStatusRunning)
                return;

            try
            {
                _isMainStatusRunning = true;

                if (m_MainTick_Start == false)
                    return;

                //TEST
                //if (!workStage.m_bHomeOK)
                //    return;

                SeqScannerCalibration();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            finally
            {
                _isMainStatusRunning = false;
            }
        }
        #endregion

        #region Task Utility
        private void CleanupCompletedTasks()
        {
            for (int i = listTask.Count - 1; i >= 0; i--)
            {
                var t = listTask[i];
                if (t == null || t.IsCompleted || t.IsCanceled || t.IsFaulted)
                {
                    try { t?.Dispose(); } catch { }
                    listTask.RemoveAt(i);
                }
            }
        }

        private void StartMainTickLoop()
        {
            try { m_mainTickCts?.Cancel(); } catch { }
            try { m_mainTickCts?.Dispose(); } catch { }

            m_mainTickCts = new CancellationTokenSource();
            var token = m_mainTickCts.Token;

            CleanupCompletedTasks();

            m_taskTimer_Main_Tick = Task.Factory.StartNew(() =>
            {
                var th = Thread.CurrentThread;
                if (th.Name == null)
                {
                    try { th.Name = "m_taskTimer_ScannerCalibration_Tick"; } catch { }
                }

                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        Thread.Sleep(20);

                        if (isModuleClose)
                            break;

                        Timer_MainStatus_Tick(null, null);
                    }
                    catch (Exception ex)
                    {
                        Log.Write(ex);
                    }
                }
            }, token, TaskCreationOptions.LongRunning, TaskScheduler.Default);

            listTask.Add(m_taskTimer_Main_Tick);

            m_taskTimer_Main_Tick.ContinueWith(t =>
            {
                if (t.IsFaulted)
                    Log.Write("ScannerCalibration", Equipment.User_Name, $"Main tick faulted: {t.Exception?.GetBaseException().Message}");
                else
                    Log.Write("ScannerCalibration", Equipment.User_Name, $"Main tick completed. Status={t.Status}");

                if (!isModuleClose && !(m_mainTickCts?.IsCancellationRequested ?? true))
                {
                    StartMainTickLoop();
                }
            }, TaskScheduler.Default);
        }

        public void EnsureMainTickRunning()
        {
            if (!IsMainTickAlive && !isModuleClose)
            {
                Log.Write("ScannerCalibration", Equipment.User_Name, "Main tick not alive. Restarting...");
                StartMainTickLoop();
            }
        }

        public void StopMainTickLoop()
        {
            try { m_mainTickCts?.Cancel(); } catch { }
        }
        #endregion

        public Sequence_ScannerCalibration()
        {
        }

        ~Sequence_ScannerCalibration()
        {
            isModuleClose = true;
            try { m_mainTickCts?.Cancel(); } catch { }

            foreach (var task in listTask)
            {
                try { task?.Wait(1000); } catch { }
                try { task?.Dispose(); } catch { }
            }

            listTask.Clear();
            m_taskTimer_Main_Tick = null;

            try { m_mainTickCts?.Dispose(); } catch { }
            m_mainTickCts = null;
        }

        #region Tick Count
        public enum TickType : int
        {
            TICK_NONE = 0,
            TICK_SCANNER_CALIBRATION,
        }

        public int[,] TickCount_Cycle = new int[System.Enum.GetValues(typeof(TickType)).Length, 2];

        public void TickCount_Start(int index)
        {
            TickCount_Cycle[index, 0] = Environment.TickCount;
        }

        public int TickCount_Elapsed(int index)
        {
            TickCount_Cycle[index, 1] = Environment.TickCount;
            return TickCount_Cycle[index, 1] - TickCount_Cycle[index, 0];
        }

        private const int nScannerCalibrationTimeout = 60000;
        #endregion

        #region Members
        double m_dScannerCalPosX_Last = 0.0;
        double m_dScannerCalPosY_Last = 0.0;
        double m_dCurrentCalPosX = 0.0;
        double m_dCurrentCalPosY = 0.0;
        double m_dZOffset_SocketHeightCheck = 0.0;

        private XyCoordinate xyInterpolatedCoordinate = new XyCoordinate(0.0, 0.0);

        int m_nCrossMark_AlignMark_Count = 0;
        int m_nCrossMark_AlignMarkCount_Max = 3;
        PointD m_pCrossMark_AlignMarkPosition_Sum = new PointD(0, 0);
        PointD m_pCrossMark_AlignMarkPosition_Average = new PointD(0, 0);

        public XyCoordinate[] m_forAlign_Data = new XyCoordinate[System.Enum.GetValues(typeof(AlignParam)).Length];

        double m_deltaX = 0.0;
        double m_deltaY = 0.0;

        private bool bIsLeftToRight = false;
        private int m_nBETChange_RetryCount = 0;
        #endregion

        public void Init()
        {
            ModuleCollection modules = Equipment.Modules;
            foreach (Module module in modules)
            {
                if (module.Name == "WorkStage")
                    workStage = module as QMC.Common.Modules.WorkStage;
                else if (module.Name == "BDS")
                    bds = module as QMC.Common.Modules.Bds;
                else if (module.Name == "Vision")
                    vision = module as QMC.Common.Modules.Vision;
            }

            m_scannerCalConfig = ScannerCalConfigData.LoadFromIni();
            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.None;
            StartMainTickLoop();
        }

        public void Start()
        {
            m_scannerCalConfig = ScannerCalConfigData.LoadFromIni();
            m_bScannerCalibration_Complete = false;
            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.Start;
            m_MainTick_Start = true;
        }

        public ScannerCalibrationSeq_Step GetCurrentStep() => m_ScannerCalibrationStep;
        public void SetStep(ScannerCalibrationSeq_Step step) => m_ScannerCalibrationStep = step;

        public void Reset()
        {
            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.None;
            m_bScannerCalibration_Complete = false;
            m_MainTick_Start = false;
        }

        private int AlarmAndStop(AlarmKey key, string msg)
        {
            Log.Write("ScannerCalibration", "ScannerCalibration", msg);
            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.None;
            m_MainTick_Start = false;
            return workStage.AlarmPost(key);
        }

        private bool IsStageXYArrived(XyCoordinate pos)
        {
            return workStage.IsWorkStage_Positions(WorkStage.nAxis.X, pos.X) &&
                   workStage.IsWorkStage_Positions(WorkStage.nAxis.Y, pos.Y);
        }

        private int SeqScannerCalibration()
        {
            int nRtn = 0;
            string strTemp = string.Empty;
            double lfVelocity = 0.0;
            double lfAccDec = 0.0;

            double m_dHeightOffsetVision = Equipment.Scanner_Calibration_VisionZOffset;
            double m_dHeightOffsetScanner = 0.0;

            bool bCalPosition = Equipment.Scanner_Calibration_Position_Enable;
            bool bCalChagne = Equipment.Scanner_Calibration_Change;

            if (Equipment.AutoManualStatus &&
                (Equipment.AutoRunStatus || Equipment.SelectRunEnable_New || Equipment.SemiAutoEnable))
            {
                bCalPosition = true;
                bCalChagne = false;
            }

            switch (m_ScannerCalibrationStep)
            {
                case ScannerCalibrationSeq_Step.None:
                    break;

                case ScannerCalibrationSeq_Step.Start:
                    {
                        m_bScannerCalibration_Complete = false;

                        int ch1Val = Equipment.Scanner_Calibration_Illumination_Red_Value;
                        int ch2Val = Equipment.Scanner_Calibration_Illumination_IR_Value;
                        int exposureTime = Equipment.Scanner_Calibration_ExposureTime_High;

                        workStage.SetLightingByChannel(LightingChannel.CoarseCamIR, 0, false);
                        workStage.SetLightingByChannel(LightingChannel.CoarseCamRed, 0, false);
                        Thread.Sleep(100);
                        workStage.SetLightingByChannel(LightingChannel.FineCamRed, ch1Val);
                        workStage.SetLightingByChannel(LightingChannel.FineCamIR, ch2Val);

                        workStage.jigAligner_HighRes.Camera.SetExposureTime(exposureTime);

                        m_nCrossMark_AlignMarkCount_Max = 3;
                        m_nCrossMark_AlignMark_Count = 0;
                        m_pCrossMark_AlignMarkPosition_Sum = new PointD(0, 0);
                        m_pCrossMark_AlignMarkPosition_Average = new PointD(0, 0);

                        Log.Write("ScannerCalibration", "ScannerCalibration", "Laser&Scanner Calibration Start");
                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.Stage_Cal_Vacuum_On;
                    }
                    break;

                case ScannerCalibrationSeq_Step.Stage_Cal_Vacuum_On:
                    {
                        if (bCalPosition)
                            workStage.workStageParameter.DO_Laser_CalSheet_Vacuum(true);
                        else
                            workStage.workStageParameter.DO_Stage_Vacuum(true);

                        Thread.Sleep(100);
                        TickCount_Start((int)TickType.TICK_SCANNER_CALIBRATION);
                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.Stage_Cal_Vacuum_On_Check;
                    }
                    break;

                case ScannerCalibrationSeq_Step.Stage_Cal_Vacuum_On_Check:
                    {
                        bool ok = false;

                        if (bCalPosition)
                        {
                            if (workStage.workStageParameter.DI_Laser_CalSheet_Vacuum_Check() ||
                                workStage.workStageParameter.IsDO_Laser_CalSheet_Vacuum())
                                ok = true;
                        }
                        else
                        {
                            ok = workStage.workStageParameter.DI_Stage_Vacuum_Check();
                        }

                        if (ok)
                        {
                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.Laser_Off;
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_SCANNER_CALIBRATION) > nScannerCalibrationTimeout)
                        {
                            return AlarmAndStop(AlarmKey.StageCal_Vacuum_On_Fail, "Stage 진공 On 실패");
                        }
                    }
                    break;

                case ScannerCalibrationSeq_Step.Laser_Off:
                    {
                        if (workStage.rtc != null)
                            workStage.rtc.CtlLaserOff();

                        TickCount_Start((int)TickType.TICK_SCANNER_CALIBRATION);
                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.Laser_Off_Check;
                    }
                    break;

                case ScannerCalibrationSeq_Step.Laser_Off_Check:
                    {
                        if (workStage.rtc == null)
                            return AlarmAndStop(AlarmKey.eLaserComm_NotOpen, "Laser Comm 열리지 않음");

                        if (workStage.rtc.CtlGetStatus(RtcStatus.NotBusy))
                        {
                            if (Equipment.Machine_LaserType_CO2)
                                m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.DustCollector_On;
                            else
                                m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.LaserFrequency_Change;
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_SCANNER_CALIBRATION) > nScannerCalibrationTimeout)
                        {
                            return AlarmAndStop(AlarmKey.eRTC_FAIL, "Laser Off Check 실패");
                        }
                    }
                    break;

                case ScannerCalibrationSeq_Step.LaserShutter_Close:
                    {
                        workStage.workStageParameter.DO_BDS_PowerMeter_FW(true);
                        Thread.Sleep(200);
                        workStage.workStageParameter.DO_BDS_PowerMeter_BW(false);

                        TickCount_Start((int)TickType.TICK_SCANNER_CALIBRATION);
                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.LaserShutter_Close_Check;
                    }
                    break;

                case ScannerCalibrationSeq_Step.LaserShutter_Close_Check:
                    {
                        if (!workStage.workStageParameter.DI_BDS_PowerMeter_BW_Check() &&
                             workStage.workStageParameter.DI_BDS_PowerMeter_FW_Check())
                        {
                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.LaserFrequency_Change;
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_SCANNER_CALIBRATION) > nScannerCalibrationTimeout)
                        {
                            return AlarmAndStop(AlarmKey.eBeamShutterCloseFail, "Shutter Close 실패");
                        }
                    }
                    break;

                case ScannerCalibrationSeq_Step.LaserFrequency_Change:
                    {
                        if (workStage.m_rapidLxLaser_Comm == null || !workStage.m_rapidLxLaser_Comm.IsOpen)
                            return AlarmAndStop(AlarmKey.eLaserComm_NotOpen, "Laser Frequency 변경 실패 - Laser Comm 미연결");

                        if (Equipment.Scanner_Calibration_LaserFrequency <= 0.0)
                            return AlarmAndStop(AlarmKey.ScannerCalibration_Fail, "Laser Frequency 값이 0 이하");

                        workStage.RapidLxLaserComm_Laser_AmplifierRR_Set(Equipment.Scanner_Calibration_LaserFrequency);
                        TickCount_Start((int)TickType.TICK_SCANNER_CALIBRATION);
                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.LaserFrequency_ChangeCheck;
                    }
                    break;

                case ScannerCalibrationSeq_Step.LaserFrequency_ChangeCheck:
                    {
                        const double epsilon = 0.1;
                        if (Math.Abs(Equipment.Scanner_Calibration_LaserFrequency - workStage.m_dLaserComm_ReadSetValue_Amplifier) < epsilon)
                        {
                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.LaserPower_Change;
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_SCANNER_CALIBRATION) > nScannerCalibrationTimeout)
                        {
                            return AlarmAndStop(AlarmKey.ScannerCalibration_Fail, "Laser Frequency 변경 완료 확인 실패");
                        }
                    }
                    break;

                case ScannerCalibrationSeq_Step.LaserPower_Change:
                    {
                        if (workStage.m_rapidLxLaser_Comm == null || !workStage.m_rapidLxLaser_Comm.IsOpen)
                            return AlarmAndStop(AlarmKey.eLaserComm_NotOpen, "Laser Power 변경 실패 - Laser Comm 미연결");

                        if (Equipment.Scanner_Calibration_LaserEnergy <= 0.0)
                            return AlarmAndStop(AlarmKey.LaserPowerChange_Fail, "Laser Energy 값이 0 이하");

                        workStage.RapidLxLaserComm_Laser_OutputEnergy_Set(Equipment.Scanner_Calibration_LaserEnergy);
                        TickCount_Start((int)TickType.TICK_SCANNER_CALIBRATION);
                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.LaserPower_ChangeCheck;
                    }
                    break;

                case ScannerCalibrationSeq_Step.LaserPower_ChangeCheck:
                    {
                        const double epsilon = 0.001;
                        if (Math.Abs(Equipment.Scanner_Calibration_LaserEnergy - workStage.m_dLaserComm_ReadSetValue_EnergyPercent) < epsilon)
                        {
                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.DustCollector_On;
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_SCANNER_CALIBRATION) > nScannerCalibrationTimeout)
                        {
                            return AlarmAndStop(AlarmKey.LaserPowerChange_Fail, "Laser Energy 변경 완료 확인 실패");
                        }
                    }
                    break;

                case ScannerCalibrationSeq_Step.DustCollector_On:
                    {
                        workStage.DustCollector_On((int)nDustCollector.DustCollector_Upper);
                        Thread.Sleep(100);
                        workStage.DustCollector_On((int)nDustCollector.DustCollector_Lower);
                        Thread.Sleep(100);

                        TickCount_Start((int)TickType.TICK_SCANNER_CALIBRATION);
                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.DustCollector_Frequency_Set;
                    }
                    break;

                case ScannerCalibrationSeq_Step.DustCollector_Frequency_Set:
                    {
                        if (TickCount_Elapsed((int)TickType.TICK_SCANNER_CALIBRATION) > 1000)
                        {
                            workStage.DustCollectorComm_Send_SetFrequency((int)nDustCollector.DustCollector_Upper, 30);
                            Thread.Sleep(100);
                            workStage.DustCollectorComm_Send_SetFrequency((int)nDustCollector.DustCollector_Lower, 30);
                            Thread.Sleep(100);

                            TickCount_Start((int)TickType.TICK_SCANNER_CALIBRATION);
                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.DustCollector_On_Check;
                        }
                    }
                    break;

                case ScannerCalibrationSeq_Step.DustCollector_On_Check:
                    {
                        if ((workStage.workStageParameter.DI_DustCollector_Fan_Run((int)nDustCollector.DustCollector_Upper) &&
                             workStage.workStageParameter.DI_DustCollector_Fan_Run((int)nDustCollector.DustCollector_Lower)) ||
                            (TickCount_Elapsed((int)TickType.TICK_SCANNER_CALIBRATION) > DustCollector_TurnOn_AfterStableTime))
                        {
                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.Scanner_Init;
                        }
                        else if (workStage.workStageParameter.DI_DustCollector_Fan_Fault((int)nDustCollector.DustCollector_Upper) ||
                                 workStage.workStageParameter.DI_DustCollector_Fan_Fault((int)nDustCollector.DustCollector_Lower))
                        {
                            return AlarmAndStop(AlarmKey.eDustCollectorFail, "집진기 알람 발생");
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_SCANNER_CALIBRATION) > 120000)
                        {
                            return AlarmAndStop(AlarmKey.eDustCollectorFail, "집진기 On 실패");
                        }
                    }
                    break;

                case ScannerCalibrationSeq_Step.Scanner_Init:
                    {
                        if (Equipment.Machine_LaserType_CO2)
                        {
                            if (Equipment.ScannerMode_Change_byUser == (int)RtcMode.RTC_RTC6_COMPLETE)
                            {
                                TickCount_Start((int)TickType.TICK_SCANNER_CALIBRATION);
                                workStage.workStageParameter.DO_Laser_Enable(false);
                                Equipment.ScannerMode_Change_byUser = (int)RtcMode.RTC_RTC6;
                            }
                        }
                        else
                        {
                            if (Equipment.ScannerMode_Change_byUser == (int)RtcMode.RTC_RTC6_COMPLETE)
                            {
                                TickCount_Start((int)TickType.TICK_SCANNER_CALIBRATION);
                                Equipment.ScannerMode_Change_byUser = (int)RtcMode.RTC_RTC6;
                            }
                        }

                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.Scanner_Init_Check;
                    }
                    break;

                case ScannerCalibrationSeq_Step.Scanner_Init_Check:
                    {
                        if (Equipment.ScannerMode_Change_byUser == (int)RtcMode.RTC_RTC6_COMPLETE &&
                            Equipment._InitDeviceStatus.Scanner &&
                            TickCount_Elapsed((int)TickType.TICK_SCANNER_CALIBRATION) > 5000)
                        {
                            if (Equipment.Machine_LaserType_CO2)
                            {
                                workStage.workStageParameter.DO_Laser_Enable(true);
                                Thread.Sleep(100);
                            }

                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.LaserShutter_Open;
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_SCANNER_CALIBRATION) > 120000)
                        {
                            return AlarmAndStop(AlarmKey.InitFail_Scanner, "Scanner init 실패");
                        }
                    }
                    break;

                case ScannerCalibrationSeq_Step.LaserShutter_Open:
                    {
                        workStage.workStageParameter.DO_BDS_PowerMeter_BW(true);
                        Thread.Sleep(200);
                        workStage.workStageParameter.DO_BDS_PowerMeter_FW(false);

                        TickCount_Start((int)TickType.TICK_SCANNER_CALIBRATION);
                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.LaserShutter_Open_Check;
                    }
                    break;

                case ScannerCalibrationSeq_Step.LaserShutter_Open_Check:
                    {
                        if (workStage.workStageParameter.DI_BDS_PowerMeter_BW_Check() &&
                            !workStage.workStageParameter.DI_BDS_PowerMeter_FW_Check())
                        {
                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.WaterLine_Open;
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_SCANNER_CALIBRATION) > nScannerCalibrationTimeout)
                        {
                            return AlarmAndStop(AlarmKey.eBeamShutterOpenFail, "Shutter Open 실패");
                        }
                    }
                    break;

                case ScannerCalibrationSeq_Step.WaterLine_Open:
                    {
                        workStage.workStageParameter.DO_BeamDump_Coolant_Supply(true);
                        workStage.workStageParameter.DO_Scanner_Coolant_Supply(true);

                        if (Equipment.Machine_LaserType_CO2)
                        {
                            workStage.workStageParameter.DO_Mask_Coolant_Supply(true);
                            workStage.workStageParameter.DO_VarioScan_Coolant_Supply(true);
                        }

                        TickCount_Start((int)TickType.TICK_SCANNER_CALIBRATION);
                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.WaterLine_Open_Check;
                    }
                    break;

                case ScannerCalibrationSeq_Step.WaterLine_Open_Check:
                    {
                        bool ok = workStage.workStageParameter.IsDO_BeamDump_Coolant_Supply() &&
                                  workStage.workStageParameter.IsDO_Scanner_Coolant_Supply();

                        if (Equipment.Machine_LaserType_CO2)
                        {
                            ok = ok &&
                                 workStage.workStageParameter.IsDO_Mask_Coolant_Supply() &&
                                 workStage.workStageParameter.IsDO_VarioScan_Coolant_Supply();
                        }

                        if (ok)
                        {
                            if (Equipment.Machine_LaserType_CO2)
                                m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.Mask_Change;
                            else
                                m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.StageXY_Move_CenterPos;
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_SCANNER_CALIBRATION) > 5000)
                        {
                            return AlarmAndStop(AlarmKey.WaterLine_Open_Fail, "Water Supply Line Open 실패");
                        }
                    }
                    break;

                case ScannerCalibrationSeq_Step.Mask_Change:
                    {
                        workStage.workStageParameter.stWorkStagePosParam = workStage.workStageParameter.GetPositionInformation("Processing");

                        lfVelocity = Equipment.stAxisParam[(int)Bds.nAxis.MASK_Y].Common_Speed_Coarse;
                        lfAccDec = Equipment.stAxisParam[(int)Bds.nAxis.MASK_Y].Common_Acceleration_Coarse;

                        workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.MASK_Y] =
                            bds.stBDSTeachingPos[(int)Equipment.stLayerRecipeSet[(int)Equipment.LayerList.Hole1].Miscellaneous_MaskIndex].Mask_Y;

                        workStage.MC_Func.MC_MovePosition(
                            (int)Bds.nAxis.MASK_Y,
                            workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.MASK_Y],
                            lfVelocity, lfAccDec, lfAccDec);

                        TickCount_Start((int)TickType.TICK_SCANNER_CALIBRATION);
                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.Mask_Change_Check;
                    }
                    break;

                case ScannerCalibrationSeq_Step.Mask_Change_Check:
                    {
                        if (workStage.MC_Func.MC_GetDone((int)Bds.nAxis.MASK_Y) &&
                            workStage.MC_Func.MC_PosTolerance((int)Bds.nAxis.MASK_Y,
                                workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.MASK_Y]))
                        {
                            m_nBETChange_RetryCount = 0;
                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.BETA_Change;
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_SCANNER_CALIBRATION) > 60000)
                        {
                            return AlarmAndStop(AlarmKey.MaskY_Axis_Fail, "Mask Y 이동 실패");
                        }
                    }
                    break;

                case ScannerCalibrationSeq_Step.BETA_Change:
                    {
                        int betIndex = Equipment.stLayerRecipeSet[0].Miscellaneous_BETPositionIndex;
                        if (betIndex < 0 || betIndex >= 5)
                            return AlarmAndStop(AlarmKey.eBETIndexFail, "BET Index 범위 오류");

                        workStage.LaserDrillingStepBETChange(betIndex);

                        TickCount_Start((int)TickType.TICK_SCANNER_CALIBRATION);
                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.BETA_Change_Check;
                    }
                    break;

                case ScannerCalibrationSeq_Step.BETA_Change_Check:
                    {
                        if (TickCount_Elapsed((int)TickType.TICK_SCANNER_CALIBRATION) <= 500)
                            break;

                        int betIndex = Equipment.stLayerRecipeSet[0].Miscellaneous_BETPositionIndex;
                        double targetZoom = 0.0;
                        double targetMrad = 0.0;

                        switch (betIndex)
                        {
                            case 0: targetZoom = 0.8; targetMrad = Equipment.BET_0_8X_Mrad; break;
                            case 1: targetZoom = 0.9; targetMrad = Equipment.BET_0_9X_Mrad; break;
                            case 2: targetZoom = 1.0; targetMrad = Equipment.BET_1_0X_Mrad; break;
                            case 3: targetZoom = 1.1; targetMrad = Equipment.BET_1_1X_Mrad; break;
                            case 4: targetZoom = 1.2; targetMrad = Equipment.BET_1_2X_Mrad; break;
                        }

                        bool ok = (workStage.m_dBET_ZoomValue > (targetZoom - 0.005)) &&
                                  (workStage.m_dBET_ZoomValue < (targetZoom + 0.005)) &&
                                  (workStage.m_dBET_MradValue > (targetMrad - 0.005)) &&
                                  (workStage.m_dBET_MradValue < (targetMrad + 0.005));

                        if (ok)
                        {
                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.Vario_Change;
                        }
                        else
                        {
                            if (m_nBETChange_RetryCount++ < 3)
                            {
                                m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.BETA_Change;
                                Thread.Sleep(200);
                            }
                            else
                            {
                                return AlarmAndStop(AlarmKey.eBETChangeFail, "BET 변경 실패");
                            }
                        }
                    }
                    break;

                case ScannerCalibrationSeq_Step.Vario_Change:
                    {
                        if (bds.spiralLabVario != null)
                        {
                            // 여기에서.. 현재 지정한 Z축으로 이동해한다. 
                            // 0 기준으로 이동해야 한다. 
                            bds.spiralLabVario.fSetZOffset = (float)Equipment.Scanner_Calibration_VisionZOffset;
                            bds.spiralLabVario.SetZOffset(bds.spiralLabVario.fSetZOffset);
                        }

                        TickCount_Start((int)TickType.TICK_SCANNER_CALIBRATION);
                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.Vario_Change_Check;
                    }
                    break;

                case ScannerCalibrationSeq_Step.Vario_Change_Check:
                    {
                        if (bds.spiralLabVario == null ||
                            bds.spiralLabVario.GetCurrentZOffset() == bds.spiralLabVario.fSetZOffset)
                        {
                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.StageXY_Move_CenterPos;
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_SCANNER_CALIBRATION) > 60000)
                        {
                            return AlarmAndStop(AlarmKey.Vario_Scan_Fail, "Vario 이동 실패");
                        }
                    }
                    break;

                case ScannerCalibrationSeq_Step.StageXY_Move_CenterPos:
                    {
                        int posIndex = (int)WorkStage_TeachingPosList.STAGE_ProcessingPos;
                        workStage.MapData_Apply((int)WorkStage.nMapData_Type.MapData_Stage_Scanner);
                        workStage.MovetoWorkStage_TeachingPositionsXY(posIndex, Type_Motor_Speed.Coarse);

                        TickCount_Start((int)TickType.TICK_SCANNER_CALIBRATION);
                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.StageXY_Move_CenterPos_Check;
                    }
                    break;

                case ScannerCalibrationSeq_Step.StageXY_Move_CenterPos_Check:
                    {
                        if (workStage.IsWorkStage_TeachingPositionsXY((int)WorkStage_TeachingPosList.STAGE_ProcessingPos))
                        {
                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.VerifyCalibrationAreaPos;
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_SCANNER_CALIBRATION) > 50000)
                        {
                            return AlarmAndStop(AlarmKey.eStageMoveFail, "Stage Center 이동 실패");
                        }
                    }
                    break;

                case ScannerCalibrationSeq_Step.VerifyCalibrationAreaPos:
                    {
                        double dScannerCalTeachingPosX;
                        double dScannerCalTeachingPosY;

                        if (bCalPosition)
                        {
                            dScannerCalTeachingPosX = workStage.stWorkStageTeachingPos[(int)WorkStage_TeachingPosList.STAGE_Scanner_CalPos].Stage_X;
                            dScannerCalTeachingPosY = workStage.stWorkStageTeachingPos[(int)WorkStage_TeachingPosList.STAGE_Scanner_CalPos].Stage_Y;
                        }
                        else
                        {
                            if (Equipment.stLayerRecipeSet[0].ChuckMSL_Enable)
                            {
                                dScannerCalTeachingPosX = Equipment.StageOffset_forDrilling_X_MSL;
                                dScannerCalTeachingPosY = Equipment.StageOffset_forDrilling_Y_MSL;
                            }
                            else
                            {
                                dScannerCalTeachingPosX = workStage.stWorkStageTeachingPos[(int)WorkStage_TeachingPosList.STAGE_ProcessingPos].Stage_X;
                                dScannerCalTeachingPosY = workStage.stWorkStageTeachingPos[(int)WorkStage_TeachingPosList.STAGE_ProcessingPos].Stage_Y;
                            }
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
                        //double dScannerCalAreaPosX_Min = dScannerCalTeachingPosX - Equipment.Scanner_Calibration_X_Distance;
                        //double dScannerCalAreaPosX_Max = dScannerCalTeachingPosX + Equipment.Scanner_Calibration_X_Distance;
                        //double dScannerCalAreaPosY_Min = dScannerCalTeachingPosY - Equipment.Scanner_Calibration_Y_Distance;
                        //double dScannerCalAreaPosY_Max = dScannerCalTeachingPosY + Equipment.Scanner_Calibration_Y_Distance;

                        int nRow = Equipment.Scanner_Calibration_rowCount;
                        int nCol = Equipment.Scanner_Calibration_colCount;
                        float fRowInterval = (float)Equipment.Scanner_Calibration_rowInterval;
                        float fColInterval = (float)Equipment.Scanner_Calibration_colInterval;

                        if (Equipment.Scanner_Vision_Offset_Setting_Use)
                        {
                            nRow = 1;
                            nCol = 1;
                            fRowInterval = 1;
                            fColInterval = 1;
                        }

                        double dCalWidth = (nCol - 1) * fColInterval;

                        if (bCalChagne)
                        {
                            if (bCalPosition)
                            {
                                Equipment.Scanner_Calibration_PosX_Last = 0.0;
                                Equipment.Scanner_Calibration_PosY_Last = 0.0;

                                m_dCurrentCalPosX = dScannerCalAreaPosX_Max;
                                m_dCurrentCalPosY = dScannerCalAreaPosY_Max;
                                bIsLeftToRight = true;
                            }
                            else
                            {
                                m_dCurrentCalPosX = dScannerCalTeachingPosX;
                                m_dCurrentCalPosY = dScannerCalTeachingPosY;
                                bIsLeftToRight = true;
                            }

                            Equipment.Scanner_Calibration_Change = false;
                        }
                        else
                        {
                            if (bCalPosition)
                            {
                                m_dCurrentCalPosX = Equipment.Scanner_Calibration_PosX_Last - (dCalWidth + dCalPitchOffset);
                                m_dCurrentCalPosY = Equipment.Scanner_Calibration_PosY_Last;
                            }
                            else
                            {
                                m_dCurrentCalPosX = dScannerCalTeachingPosX;
                                m_dCurrentCalPosY = dScannerCalTeachingPosY;
                            }

                            if (bCalPosition && m_dCurrentCalPosX < dScannerCalAreaPosX_Min)
                            {
                                m_dCurrentCalPosY = Equipment.Scanner_Calibration_PosY_Last - dCalPitchOffset;
                                bIsLeftToRight = !bIsLeftToRight;
                                m_dCurrentCalPosX = bIsLeftToRight ? dScannerCalAreaPosX_Min : dScannerCalAreaPosX_Max;

                                if (m_dCurrentCalPosY < dScannerCalAreaPosY_Min)
                                {
                                    return AlarmAndStop(AlarmKey.Scan_Area_Fail, "캘판 범위 모두 처리 완료");
                                }
                            }
                        }

                        double epsilon = 2.0;
                        if (bCalPosition)
                        {
                            if (m_dCurrentCalPosX < dScannerCalAreaPosX_Min - epsilon ||
                                m_dCurrentCalPosX > dScannerCalAreaPosX_Max + epsilon ||
                                m_dCurrentCalPosY < dScannerCalAreaPosY_Min - epsilon ||
                                m_dCurrentCalPosY > dScannerCalAreaPosY_Max + epsilon)
                            {
                                return AlarmAndStop(AlarmKey.Scan_Area_Fail, "캘판 범위 벗어남");
                            }
                        }

                        Equipment.Scanner_Calibration_PosX_Last = m_dCurrentCalPosX;
                        Equipment.Scanner_Calibration_PosY_Last = m_dCurrentCalPosY;

                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.MapDataChange_ScannerCalMap;
                    }
                    break;

                case ScannerCalibrationSeq_Step.MapDataChange_ScannerCalMap:
                    {
                        if (bCalPosition)
                            workStage.MapData_Apply((int)nMapData_Type.MapData_Stage_CalPos_Scanner);
                        else
                            workStage.MapData_Apply((int)nMapData_Type.MapData_Stage_Scanner);

                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.MapDataFlagCheck_ScannerCalMap;
                    }
                    break;

                case ScannerCalibrationSeq_Step.MapDataFlagCheck_ScannerCalMap:
                    {
                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.StageZ_Move_LaserHeightSensorPos;
                    }
                    break;

                case ScannerCalibrationSeq_Step.StageZ_Move_LaserHeightSensorPos:
                    {
                        int nZPos;
                        if (bCalPosition)
                            nZPos = (int)QMC.Common.Modules.Vision.Vision_TeachingPosList.Laser_Sensor_HeightCheck_CalPos;
                        else
                            nZPos = (int)QMC.Common.Modules.Vision.Vision_TeachingPosList.Laser_Sensor_HeightCheckPos;

                        double dPosZ = vision.stVisionTeachingPos[nZPos].Vision_Z;
                        workStage.MovetoWorkStage_ABS_PositionsZ(dPosZ, Type_Motor_Speed.Fine);

                        TickCount_Start((int)TickType.TICK_SCANNER_CALIBRATION);
                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.StageZ_Move_LaserHeightSensorPos_DoneCheck;
                    }
                    break;

                case ScannerCalibrationSeq_Step.StageZ_Move_LaserHeightSensorPos_DoneCheck:
                    {
                        int nZPos = bCalPosition
                            ? (int)QMC.Common.Modules.Vision.Vision_TeachingPosList.Laser_Sensor_HeightCheck_CalPos
                            : (int)QMC.Common.Modules.Vision.Vision_TeachingPosList.Laser_Sensor_HeightCheckPos;

                        double dPosZ = vision.stVisionTeachingPos[nZPos].Vision_Z;
                        if (workStage.IsWorkStage_Positions(WorkStage.nAxis.Z, dPosZ))
                        {
                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.StageXY_Move_LaserHeightSensorPos;
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_SCANNER_CALIBRATION) > nScannerCalibrationTimeout * 5)
                        {
                            return AlarmAndStop(AlarmKey.eZAxisFail, "Height Sensor Z 이동 실패");
                        }
                    }
                    break;

                case ScannerCalibrationSeq_Step.StageXY_Move_LaserHeightSensorPos:
                    {
                        xyInterpolatedCoordinate.X = m_dCurrentCalPosX;
                        xyInterpolatedCoordinate.Y = m_dCurrentCalPosY;
                        workStage.MovetoWorkStage_ABS_PositionsXY(xyInterpolatedCoordinate, Type_Motor_Speed.Coarse);

                        TickCount_Start((int)TickType.TICK_SCANNER_CALIBRATION);
                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.StageXY_Move_LaserHeightSensorPos_DoneCheck;
                    }
                    break;

                case ScannerCalibrationSeq_Step.StageXY_Move_LaserHeightSensorPos_DoneCheck:
                    {
                        if (IsStageXYArrived(xyInterpolatedCoordinate))
                        {
                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.StageZ_Move_LaserHeightSensor_CalPos;
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_SCANNER_CALIBRATION) > nScannerCalibrationTimeout)
                        {
                            return AlarmAndStop(AlarmKey.eStageMoveFail, "Height Sensor XY 이동 실패");
                        }
                    }
                    break;

                case ScannerCalibrationSeq_Step.StageZ_Move_LaserHeightSensor_CalPos:
                    {
                        TickCount_Start((int)TickType.TICK_SCANNER_CALIBRATION);
                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.StageZ_Move_LaserHeightSensor_CalPos_DoneCheck;
                    }
                    break;

                case ScannerCalibrationSeq_Step.StageZ_Move_LaserHeightSensor_CalPos_DoneCheck:
                    {
                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.Move_LaserHeightSensorPos_StableTime;
                    }
                    break;

                case ScannerCalibrationSeq_Step.Move_LaserHeightSensorPos_StableTime:
                    {
                        if (TickCount_Elapsed((int)TickType.TICK_SCANNER_CALIBRATION) > 500)
                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.ScannerCalHeightValue_Get;
                    }
                    break;

                case ScannerCalibrationSeq_Step.ScannerCalHeightValue_Get:
                    {
                        if ((workStage.m_dLaserHeightSensorSocket_Value < -4.5) ||
                            (workStage.m_dLaserHeightSensorSocket_Value > 5.5) ||
                            (workStage.m_dLaserHeightSensorSocket_Value < -99.9))
                        {
                            m_dZOffset_SocketHeightCheck = 0.0;
                        }
                        else
                        {
                            m_dZOffset_SocketHeightCheck =
                                workStage.m_dLaserHeightSensorSocket_Value -
                                Equipment.LaserHeightSensor_ReferenceValue_atScannerFocusPosition;
                        }

                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.ScannerCalHeight_ZOffset_Move;
                    }
                    break;

                case ScannerCalibrationSeq_Step.ScannerCalHeight_ZOffset_Move:
                    {
                        int basePos = bCalPosition
                            ? (int)QMC.Common.Modules.Vision.Vision_TeachingPosList.Laser_Sensor_HeightCheck_CalPos
                            : (int)QMC.Common.Modules.Vision.Vision_TeachingPosList.Laser_Sensor_HeightCheckPos;

                        double dPosZ =
                            vision.stVisionTeachingPos[basePos].Vision_Z +
                            m_dZOffset_SocketHeightCheck +
                            m_dHeightOffsetScanner;

                        workStage.MovetoWorkStage_ABS_PositionsZ(dPosZ, Type_Motor_Speed.Fine);

                        TickCount_Start((int)TickType.TICK_SCANNER_CALIBRATION);
                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.ScannerCalHeight_ZOffset_Move_DoneCheck;
                    }
                    break;

                case ScannerCalibrationSeq_Step.ScannerCalHeight_ZOffset_Move_DoneCheck:
                    {
                        int basePos = bCalPosition
                            ? (int)QMC.Common.Modules.Vision.Vision_TeachingPosList.Laser_Sensor_HeightCheck_CalPos
                            : (int)QMC.Common.Modules.Vision.Vision_TeachingPosList.Laser_Sensor_HeightCheckPos;

                        double dPosZ =
                            vision.stVisionTeachingPos[basePos].Vision_Z +
                            m_dZOffset_SocketHeightCheck +
                            m_dHeightOffsetScanner;

                        if (workStage.IsWorkStage_Positions(WorkStage.nAxis.Z, dPosZ))
                        {
                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.StageXY_Move_ScannerCalibrationPos;
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_SCANNER_CALIBRATION) > nScannerCalibrationTimeout)
                        {
                            return AlarmAndStop(AlarmKey.eZAxisFail, "Scanner Calibration 높이 이동 실패");
                        }
                    }
                    break;

                case ScannerCalibrationSeq_Step.StageXY_Move_ScannerCalibrationPos:
                    {
                        if (bCalPosition)
                            workStage.MapData_Apply((int)nMapData_Type.MapData_Stage_CalPos_Scanner);
                        else
                            workStage.MapData_Apply((int)nMapData_Type.MapData_Stage_Scanner);

                        xyInterpolatedCoordinate.X = m_dCurrentCalPosX;
                        xyInterpolatedCoordinate.Y = m_dCurrentCalPosY;

                        workStage.MovetoWorkStage_ABS_PositionsXY(xyInterpolatedCoordinate, Type_Motor_Speed.Coarse);

                        TickCount_Start((int)TickType.TICK_SCANNER_CALIBRATION);
                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.StageXY_Move_ScannerCalibrationPos_DoneCheck;
                    }
                    break;

                case ScannerCalibrationSeq_Step.StageXY_Move_ScannerCalibrationPos_DoneCheck:
                    {
                        if (IsStageXYArrived(xyInterpolatedCoordinate))
                        {
                            Thread.Sleep(500);
                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.CrossMark_MarkingStart;
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_SCANNER_CALIBRATION) > nScannerCalibrationTimeout * 10)
                        {
                            return AlarmAndStop(AlarmKey.eStageMoveFail, "Scanner Calibration 위치 이동 실패");
                        }
                    }
                    break;

                case ScannerCalibrationSeq_Step.CrossMark_MarkingStart:
                    {
                        int nRow = Equipment.Scanner_Calibration_rowCount;
                        int nCol = Equipment.Scanner_Calibration_colCount;
                        float fRowInterval = (float)Equipment.Scanner_Calibration_rowInterval;
                        float fColInterval = (float)Equipment.Scanner_Calibration_colInterval;
                        double dMarkLength = Equipment.Scanner_Calibration_CrossMarkLength;

                        //Laser & Scanner 준비 상태 확인 필요. (발진 가능 여부 및 셋팅 값)
                        if (Equipment.Machine_LaserType_CO2)
                        {
                            if (Equipment.Scanner_Calibration_MarkType_Cross)
                            {
                                bds.spiralLabScanner.DrawCalibrationCrosses(nRow, nCol, fRowInterval, fColInterval, dMarkLength);
                            }
                            else //Circle
                            {
                                bds.spiralLabScanner.DrawCalibrationArc(nRow, nCol, fRowInterval, fColInterval);
                            }
                        }
                        else //UV
                        {
                            if (Equipment.Scanner_Calibration_MarkType_Cross)
                            {
                                bds.spiralLabScanner.DrawCalibrationCrosses(nRow, nCol, fRowInterval, fColInterval, dMarkLength);
                            }
                            else //Circle
                            {
                                bds.spiralLabScanner.DrawCalibrationArc(nRow, nCol, fRowInterval, fColInterval);
                            }
                        }

                        TickCount_Start((int)TickType.TICK_SCANNER_CALIBRATION);
                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.CrossMark_MarkingComplete;
                    }
                    break;

                case ScannerCalibrationSeq_Step.CrossMark_MarkingComplete:
                    {
                        if (workStage.rtc.CtlGetStatus(RtcStatus.NotBusy))
                        {
                            m_dScannerCalPosX_Last = m_dCurrentCalPosX;
                            m_dScannerCalPosY_Last = m_dCurrentCalPosY;
                            workStage.Scanner_Calibration_Option_Save();

                            Log.Write("SLD-200", "Scanner Calibration", "Scanner Calibration, Cross Mark 가공 완료");
                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.MapDataChange_FineCamMap;
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_SCANNER_CALIBRATION) > nScannerCalibrationTimeout)
                        {
                            strTemp = string.Format("Cross Mark Marking 실패");
                            Log.Write("SLD-200", "Scanner Calibration", strTemp);
                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.None;
                            return AlarmAndStop(AlarmKey.ScannerCalibration_Fail, "Cross Mark 마킹 완료 대기 실패");
                        }
                    }
                    break;

                case ScannerCalibrationSeq_Step.MapDataChange_FineCamMap:
                    {
                        if (bCalPosition)
                            workStage.MapData_Apply((int)nMapData_Type.MapData_Stage_CalPos_FineCam);
                        else
                            workStage.MapData_Apply((int)nMapData_Type.MapData_Stage_FineCam);

                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.MapDataFlagCheck_FineCamMap;
                    }
                    break;

                case ScannerCalibrationSeq_Step.MapDataFlagCheck_FineCamMap:
                    {
                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.StageXY_Move_CrossMarkCenterPos;
                    }
                    break;

                case ScannerCalibrationSeq_Step.StageXY_Move_CrossMarkCenterPos:
                    {
                        xyInterpolatedCoordinate.X = m_dCurrentCalPosX;
                        xyInterpolatedCoordinate.Y = m_dCurrentCalPosY;
                        workStage.MovetoWorkStage_ABS_PositionsXY(xyInterpolatedCoordinate, Type_Motor_Speed.Process);

                        TickCount_Start((int)TickType.TICK_SCANNER_CALIBRATION);
                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.StageXY_Move_CrossMarkCenterPos_DoneCheck;
                    }
                    break;

                case ScannerCalibrationSeq_Step.StageXY_Move_CrossMarkCenterPos_DoneCheck:
                    {
                        if (IsStageXYArrived(xyInterpolatedCoordinate))
                        {
                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.ScannerCompensation_StartPosition_Set;
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_SCANNER_CALIBRATION) > nScannerCalibrationTimeout)
                        {
                            return AlarmAndStop(AlarmKey.eStageMoveFail, "Cross Mark Center 위치 이동 실패");
                        }
                    }
                    break;

                case ScannerCalibrationSeq_Step.ScannerCompensation_StartPosition_Set:
                    {
                        if (workStage.scannerCompensator == null || workStage.scannerCompensator.Stage == null)
                            return AlarmAndStop(AlarmKey.ScannerCalibration_Fail, "scannerCompensator 또는 Stage가 null");

                        XyzCoordinate currentPos = new XyzCoordinate();
                        if (workStage.scannerCompensator.Stage.GetCommandPosition(ref currentPos) != 0)
                            return AlarmAndStop(AlarmKey.ScannerCalibration_Fail, "Stage 현재 위치를 가져올 수 없음");

                        workStage.scannerCompensator.Config.GridPositions[(int)ScannerCompensator.GridXyMotionPositionKeys.StartPosition].Coordinate = currentPos;
                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.VisionCalHeight_ZOffset_Move;
                    }
                    break;

                case ScannerCalibrationSeq_Step.VisionCalHeight_ZOffset_Move:
                    {
                        int nZPos = bCalPosition
                            ? (int)QMC.Common.Modules.Vision.Vision_TeachingPosList.Laser_Sensor_HeightCheck_CalPos
                            : (int)QMC.Common.Modules.Vision.Vision_TeachingPosList.Laser_Sensor_HeightCheckPos;

                        double dPosZ = vision.stVisionTeachingPos[nZPos].Vision_Z + m_dHeightOffsetVision;
                        workStage.MovetoWorkStage_ABS_PositionsZ(dPosZ, Type_Motor_Speed.Fine);

                        TickCount_Start((int)TickType.TICK_SCANNER_CALIBRATION);
                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.VisionCalHeight_ZOffset_Move_DoneCheck;
                    }
                    break;

                case ScannerCalibrationSeq_Step.VisionCalHeight_ZOffset_Move_DoneCheck:
                    {
                        int nZPos = bCalPosition
                            ? (int)QMC.Common.Modules.Vision.Vision_TeachingPosList.Laser_Sensor_HeightCheck_CalPos
                            : (int)QMC.Common.Modules.Vision.Vision_TeachingPosList.Laser_Sensor_HeightCheckPos;

                        double dPosZ = vision.stVisionTeachingPos[nZPos].Vision_Z + m_dHeightOffsetVision;

                        if (workStage.IsWorkStage_Positions(WorkStage.nAxis.Z, dPosZ))
                        {
                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.CrossMarkCenter_MarkFind_Ready;
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_SCANNER_CALIBRATION) > nScannerCalibrationTimeout * 5)
                        {
                            return AlarmAndStop(AlarmKey.eZAxisFail, "Vision Focus Z 이동 실패");
                        }
                    }
                    break;

                case ScannerCalibrationSeq_Step.CrossMarkCenter_MarkFind_Ready:
                    {
                        m_nCrossMark_AlignMarkCount_Max = 3;
                        m_nCrossMark_AlignMark_Count = 0;
                        m_pCrossMark_AlignMarkPosition_Sum = new PointD(0, 0);
                        m_pCrossMark_AlignMarkPosition_Average = new PointD(0, 0);

                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.CrossMarkCenter_Find;
                    }
                    break;

                case ScannerCalibrationSeq_Step.CrossMarkCenter_Find:
                    {
                        if (workStage.scannerCompensator == null || workStage.scannerCompensator.Stage == null)
                            return AlarmAndStop(AlarmKey.ScannerCalibration_Fail, "scannerCompensator 또는 Stage가 null");

                        Equipment.Vision_SpiralMove_Use = false;
                        Equipment.MachineStop_byUser = false;

                        int result = workStage.scannerCompensator.RunSearchMark();
                        if (result == 1)
                        {
                            TickCount_Start((int)TickType.TICK_SCANNER_CALIBRATION);
                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.CrossMarkCenter_FindResultCheck;
                        }
                        else
                        {
                            return AlarmAndStop(AlarmKey.Mark_Search_Fail, "마크를 찾을 수 없음");
                        }
                    }
                    break;

                case ScannerCalibrationSeq_Step.CrossMarkCenter_Find_Wait:
                    {
                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.CrossMarkCenter_FindResultCheck;
                    }
                    break;

                case ScannerCalibrationSeq_Step.CrossMarkCenter_FindResultCheck:
                    {
                        PatternMatchingResult result = workStage.scannerCompensator.GetResult();
                        if (result == null || result.Values.Count == 0)
                        {
                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.CrossMarkCenter_Find;
                            break;
                        }

                        double markX = workStage.scannerCompensator.ResultPosition.X;
                        double markY = workStage.scannerCompensator.ResultPosition.Y;

                        if (markX == 0 || markY == 0)
                        {
                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.CrossMarkCenter_Find;
                            break;
                        }

                        m_pCrossMark_AlignMarkPosition_Sum.X += markX;
                        m_pCrossMark_AlignMarkPosition_Sum.Y += markY;
                        m_nCrossMark_AlignMark_Count++;

                        if (m_nCrossMark_AlignMark_Count < m_nCrossMark_AlignMarkCount_Max)
                        {
                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.CrossMarkCenter_Find;
                        }
                        else
                        {
                            m_pCrossMark_AlignMarkPosition_Average.X =
                                m_pCrossMark_AlignMarkPosition_Sum.X / m_nCrossMark_AlignMark_Count;
                            m_pCrossMark_AlignMarkPosition_Average.Y =
                                m_pCrossMark_AlignMarkPosition_Sum.Y / m_nCrossMark_AlignMark_Count;

                            if (TickCount_Elapsed((int)TickType.TICK_SCANNER_CALIBRATION) >= nScannerCalibrationTimeout * 6)
                            {
                                workStage.MC_Func.MC_MotorStop((int)WorkStage.nAxis.X, 2000);
                                workStage.MC_Func.MC_MotorStop((int)WorkStage.nAxis.Y, 2000);
                                workStage.MC_Func.MC_MotorStop((int)WorkStage.nAxis.Z, 2000);
                                return AlarmAndStop(AlarmKey.Mark_Search_Fail, "Cross Mark 검출 시간 초과");
                            }

                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.CrossMarkCenter_XYAlignData_Calc;
                        }
                    }
                    break;

                case ScannerCalibrationSeq_Step.CrossMarkCenter_XYAlignData_Calc:
                    {
                        PatternMatchingResult result = workStage.scannerCompensator.GetResult();
                        if (result == null || result.Values.Count == 0)
                            return AlarmAndStop(AlarmKey.ScannerCalibration_Fail, "Cross Mark 데이터 없음");

                        double currentX = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X);
                        double currentY = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y);

                        m_deltaX = currentX - m_pCrossMark_AlignMarkPosition_Average.X;
                        m_deltaY = currentY - m_pCrossMark_AlignMarkPosition_Average.Y;

                        const double allowableXY = 1.0;

                        if (Math.Abs(m_deltaX) <= allowableXY &&
                            Math.Abs(m_deltaY) <= allowableXY)
                        {
                            Equipment.Scanner_Vision_Offset_Setting_X = m_deltaX;
                            Equipment.Scanner_Vision_Offset_Setting_Y = m_deltaY;

                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.CrossMarkCenter_XYAlign_CorrectionMove;
                        }
                        else
                        {
                            return AlarmAndStop(AlarmKey.Mark_Search_Error_Range_Fail,
                                $"Cross Mark XY 허용오차 초과 (DeltaX:{m_deltaX}, DeltaY:{m_deltaY})");
                        }
                    }
                    break;

                case ScannerCalibrationSeq_Step.CrossMarkCenter_XYAlign_CorrectionMove:
                    {
                        if (Math.Abs(m_deltaX) > 1.0 || Math.Abs(m_deltaY) > 1.0)
                        {
                            return AlarmAndStop(AlarmKey.Mark_Search_Error_Range_Fail,
                                $"Cross Mark XY 허용오차 초과 (DeltaX:{m_deltaX}, DeltaY:{m_deltaY})");
                        }

                        xyInterpolatedCoordinate.X = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X) + (m_deltaX * -1);
                        xyInterpolatedCoordinate.Y = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y) + m_deltaY;

                        if (bCalPosition)
                            workStage.MapData_Apply((int)nMapData_Type.MapData_Stage_CalPos_FineCam);
                        else
                            workStage.MapData_Apply((int)nMapData_Type.MapData_Stage_FineCam);

                        workStage.MovetoWorkStage_ABS_PositionsXY(xyInterpolatedCoordinate, Type_Motor_Speed.Process);

                        TickCount_Start((int)TickType.TICK_SCANNER_CALIBRATION);
                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.CrossMarkCenter_XYAlign_CorrectionMove_DoneCheck;
                    }
                    break;

                case ScannerCalibrationSeq_Step.CrossMarkCenter_XYAlign_CorrectionMove_DoneCheck:
                    {
                        if (IsStageXYArrived(xyInterpolatedCoordinate))
                        {
                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.CrossMarkCenter_XYAlign_Retry;
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_SCANNER_CALIBRATION) > nScannerCalibrationTimeout)
                        {
                            return AlarmAndStop(AlarmKey.eStageMoveFail, "Cross Mark 보정 위치 이동 실패");
                        }
                    }
                    break;

                case ScannerCalibrationSeq_Step.CrossMarkCenter_XYAlign_Retry:
                    {
                        const double TOLERANCE = 0.002;

                        double offsetX = Equipment.Scanner_Vision_Offset_Setting_X;
                        double offsetY = Equipment.Scanner_Vision_Offset_Setting_Y;

                        if (bCalPosition)
                        {
                            if (Math.Abs(offsetX) >= TOLERANCE || Math.Abs(offsetY) >= TOLERANCE)
                                m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.Start;
                            else
                                m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.StageXY_Move_ScannerCalibration_RightTopPos;
                        }
                        else
                        {
                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.StageXY_Move_ScannerCalibration_RightTopPos;
                        }
                    }
                    break;

                //  가공된 영역의 우측 상단 모서리 위치로 이동 (이동 거리, X: Pitch * (Row - 1) / 2, Y: Pitch * (Col - 1) / 2)
                case ScannerCalibrationSeq_Step.StageXY_Move_ScannerCalibration_RightTopPos:
                    {
                        if (workStage.scannerCompensator == null || workStage.scannerCompensator.Stage == null)
                        {
                            strTemp = string.Format("ScannerCompensator or Stage is null");
                            Log.Write("SLD-200", "Scanner Calibration", strTemp);
                            MessageBox.Show(strTemp, "Error");
                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.None;
                            break;
                        }

                        //Data 불러와서 넣어야함.
                        System.Drawing.Point point = new System.Drawing.Point(0, 0);
                        point.X = Equipment.Scanner_Calibration_colCount;
                        point.Y = Equipment.Scanner_Calibration_rowCount;
                        workStage.scannerCompensator.Config.Count = point;
                        workStage.scannerCompensator.Config.PitchDistanceX = Equipment.Scanner_Calibration_colInterval;
                        workStage.scannerCompensator.Config.PitchDistanceY = Equipment.Scanner_Calibration_rowInterval;

                        double dCurrentPosX = workStage.MC_Func.MC_GetEncPos((int)nAxis.X);
                        double dCurrentPosY = workStage.MC_Func.MC_GetEncPos((int)nAxis.Y);

                        // 우측 상단 기준으로 StartPosition 계산
                        double startX = dCurrentPosX - ((workStage.scannerCompensator.Config.Count.X - 1) * workStage.scannerCompensator.Config.PitchDistanceX) / 2.0;
                        double startY = dCurrentPosY - ((workStage.scannerCompensator.Config.Count.Y - 1) * workStage.scannerCompensator.Config.PitchDistanceY) / 2.0;
                        double startZ = 0.0;

                        workStage.scannerCompensator.Config.GridPositions[(int)ScannerCompensator.GridXyMotionPositionKeys.StartPosition].X = startX;
                        workStage.scannerCompensator.Config.GridPositions[(int)ScannerCompensator.GridXyMotionPositionKeys.StartPosition].Y = startY;
                        workStage.scannerCompensator.Config.GridPositions[(int)ScannerCompensator.GridXyMotionPositionKeys.StartPosition].Z = startZ;

                        // 1. 기준 좌표 획득
                        XyzCoordinate startPos =
                            workStage.scannerCompensator.Config.GridPositions[(int)ScannerCompensator.GridXyMotionPositionKeys.StartPosition].Coordinate;

                        xyInterpolatedCoordinate.X = startPos.X;
                        xyInterpolatedCoordinate.Y = startPos.Y;

                        if (bCalPosition)
                        {
                            workStage.MapData_Apply((int)nMapData_Type.MapData_Stage_CalPos_FineCam);
                        }
                        else
                        {
                            workStage.MapData_Apply((int)nMapData_Type.MapData_Stage_FineCam);
                        }

                        //  속도 설정
                        if (true)
                        {
                            workStage.MovetoWorkStage_ABS_PositionsXY(xyInterpolatedCoordinate, Type_Motor_Speed.Process);
                        }
                        else
                        {
                            workStage.MovetoWorkStage_ABS_PositionsXY(xyInterpolatedCoordinate, Type_Motor_Speed.Coarse);
                        }

                        TickCount_Start((int)TickType.TICK_SCANNER_CALIBRATION);
                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.StageXY_Move_ScannerCalibration_RightTopPos_Check;
                    }
                    break;

                case ScannerCalibrationSeq_Step.StageXY_Move_ScannerCalibration_RightTopPos_Check:
                    {
                        if (workStage.IsWorkStage_Positions(nAxis.X, xyInterpolatedCoordinate.X) &&
                           workStage.IsWorkStage_Positions(nAxis.Y, xyInterpolatedCoordinate.Y))
                        {
                            Log.Write("SLD-200", "Scanner Calibration", "Stage XY축, 우측 상단 위치로 이동 완료.");

                            TickCount_Start((int)TickType.TICK_SCANNER_CALIBRATION);
                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.ScannerCompensator_Start;
                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_SCANNER_CALIBRATION) > nScannerCalibrationTimeout)
                        {
                            strTemp = string.Format("Stage XY축, 우측 상단 위치로 이동 실패. (Timeout)");
                            Log.Write("SLD-200", "Scanner Calibration", strTemp);
                            MessageBox.Show(strTemp, "Error");
                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.None;

                            return workStage.AlarmPost(AlarmKey.ScannerCalibration_Fail);
                        }
                    }
                    break;

                case ScannerCalibrationSeq_Step.ScannerCompensator_Start:
                    {
                        if (workStage.scannerCompensator == null)
                        {
                            string msg = "ScannerCompensator is null";
                            Log.Write("SLD-200", "Scanner Calibration", msg);
                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.None;
                            return workStage.AlarmPost(WorkStage.AlarmKey.ScannerCalibration_Fail);
                        }

                        Log.Write("SLD-200", "Scanner Calibration", "ScannerCompensator 보정 시작 (Sequence)");

                        _taskRunScannerCompensation = Task.Factory.StartNew(() =>
                        {
                            workStage.scannerCompensator.SetRunStatus(RunStatus.Run);
                            int nRet = workStage.scannerCompensator.OnWork();
                            if (nRet != 0)
                            {
                                m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.None;
                                workStage.AlarmPost(WorkStage.AlarmKey.ScannerCalibration_Fail);
                            }
                        });

                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.ScannerCompensator_Wait;
                    }
                    break;

                case ScannerCalibrationSeq_Step.ScannerCompensator_Wait:
                    {
                        if (_taskRunScannerCompensation == null)
                        {
                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.None;
                            return workStage.AlarmPost(WorkStage.AlarmKey.ScannerCalibration_Fail);
                        }

                        if (!_taskRunScannerCompensation.IsCompleted)
                            break;

                        try
                        {
                            _taskRunScannerCompensation.Wait(); // 예외 전파 확인
                        }
                        catch (Exception ex)
                        {
                            Log.Write(ex);
                            _taskRunScannerCompensation.Dispose();
                            _taskRunScannerCompensation = null;
                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.None;
                            return workStage.AlarmPost(WorkStage.AlarmKey.ScannerCalibration_Fail);
                        }

                        _taskRunScannerCompensation.Dispose();
                        _taskRunScannerCompensation = null;

                        Log.Write("SLD-200", "Scanner Calibration", "ScannerCompensator 보정 완료");
                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.ScannerCompensator_Complete;
                    }
                    break;

                case ScannerCalibrationSeq_Step.ScannerCompensator_Complete:
                    {
                        Log.Write("SLD-200", "Scanner Calibration", "Scanner 보정 데이터 수집 완료");
                        workStage.TickCount_Start((int)TickType.TICK_SCANNER_CALIBRATION);
                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.ScannerCompensatedData_Check;
                    }
                    break;

                case ScannerCalibrationSeq_Step.ScannerCompensatedData_Check:
                    {
                        if (Equipment.Scanner_Calibration_Convert == 1)
                        {
                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.ScannerCompensatedData_Correction;
                        }
                        else if (Equipment.Scanner_Calibration_Convert == -1)
                        {
                            Log.Write("SLD-200", "Scanner Calibration", "Scanner 보정 데이터 확인 실패");
                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.None;
                            return workStage.AlarmPost(WorkStage.AlarmKey.ScannerCalibration_Fail);
                        }
                        else if (workStage.TickCount_Elapsed((int)TickType.TICK_SCANNER_CALIBRATION) > LaserScannerCalTimeout * 60)
                        {
                            Log.Write("SLD-200", "Scanner Calibration", "Scanner 보정 데이터 확인 실패. (Timeout)");
                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.None;
                            return workStage.AlarmPost(WorkStage.AlarmKey.ScannerCalibration_Timeout);
                        }
                    }
                    break;

                case ScannerCalibrationSeq_Step.ScannerCompensatedData_Correction:
                    {
                        // GUI 수동 보정 단계라면 여기서 상태만 다음으로
                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.ScannerCompensatedData_CorrectionFile_Convert;
                    }
                    break;

                case ScannerCalibrationSeq_Step.ScannerCompensatedData_CorrectionFile_Convert:
                    {
                        // GUI 수동 convert 단계
                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.ScannerCompensatedData_CorrectionFile_Reset;
                    }
                    break;

                case ScannerCalibrationSeq_Step.ScannerCompensatedData_CorrectionFile_Reset:
                    {
                        // retry 제어 (지역변수 말고 필드 사용!)
                        if (_scannerCalRetry < _scannerCalRetryMax)
                        {
                            _scannerCalRetry++;
                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.MapDataChange_ScannerCalMap;
                        }
                        else
                        {
                            m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.Complete;
                        }
                    }
                    break;


                case ScannerCalibrationSeq_Step.Complete:
                    {
                        Log.Write("ScannerCalibration", "ScannerCalibration", "Scanner Calibration Complete");
                        m_bScannerCalibration_Complete = true;
                        m_ScannerCalibrationStep = ScannerCalibrationSeq_Step.None;
                        m_MainTick_Start = false;
                    }
                    break;
            }

            return nRtn;
        }
    }
}