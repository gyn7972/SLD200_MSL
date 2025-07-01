using QMC.Common.Modules;
using QMC.Common.Parts;
using QMC.Common.Q_Config;
using SpiralLab.Sirius;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using static QMC.Common.Equipment;
using static QMC.Common.Modules.WorkStage;
using static QMC.Common.Q_Sequence.Sequence_LaserPowerMeasure;
using static QMC.Common.Q_Sequence.Sequence_VerifyScannerCameraOffset;

namespace QMC.Common.Q_Sequence
{
    public class Sequence_LaserPowerMeasure
    {
        public enum LaserPowerMeasure_Type
        {
            Top = 0,                                                //  Top Check
            Stage,                                                  //  Stage Check
        }

        public enum LaserPowerMeasure_Step
        {
            None = 0,
            Start = 1,

            WaterLine_Open,                                             //  수로 Open
            WaterLine_Open_Check,                                       //  수로 Open Delay

            LaserOn,
            LaserOn_Check,

            LaserPowerMeasure_Position, //Top / Stage

            TopCheck_LaserShutter_Close,                                          //  레이저 Shutter Close
            TopCheck_LaserShutter_Close_Check,                                    //  레이저 Shutter Close 확인
            
            LaserPowerMeasure_Start,                                   //  레이저 Power 측정 시작
            LaserPowerMeasure_Start_Check,                             //  레이저 Power 측정 시작 확인
            
            //StageCheck.
            LaserShutter_Open,                                          //  레이저 Shutter Open
            LaserShutter_Open_Check,                                    //  레이저 Shutter Open 확인
            
            Mask_Change,                                                //  Mask 변경 (CO2)
            Mask_Change_Check,                                          //  Mask 변경 확인 (CO2)

            BETA_Change,                                               //  BET A 변경 (CO2)
            BETA_Change_Check,                                         //  BET A 변경 확인 (CO2)

            Vario_Change,                                               // Vario 변경 (CO2)
            Vario_Change_Check,                                         // Vario 변경 확인 (CO2)

            StageZ_Move_ProcessPos,
            StageZ_Move_ProcessPos_Check,                              //  Stage Z 이동 확인

            StageXY_Move_PowerMeterPos,                              //  Stage XY 이동
            StageXY_Move_PowerMeterPos_Check,                        //  Stage XY 이동 확인

            Complete,                                                  //  완료
        }

        private SpiralLabScanner _scanner;
        private SpiralLabScanner.ScannerLaserSetting _setting = new SpiralLabScanner.ScannerLaserSetting();
        static QMC.Common.Modules.WorkStage workStage;
        static QMC.Common.Modules.Vision vision;
        static QMC.Common.Modules.Bds bds;

        private LaserPowerMeasure_Step m_LaserPowerMeasure_Step = LaserPowerMeasure_Step.None;

        #region Thread(Task)
        protected Task m_taskTimer_Main_Tick = null;
        private bool isModuleClose = false;
        protected List<Task> listTask = new List<Task>();
        public bool _isMainStatusRunning = false; // 중복 실행 방지 플래그
        public bool m_MainTick_Start = false;

        public Action<float> OnPowerMeasured;
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
                if (Equipment.AutoRunStatus || Equipment.SelectRunEnable_New || Equipment.SelectRunEnable)
                {
                }
                else
                {
                }
                SeqLaserPowerMeasure();
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

        public Sequence_LaserPowerMeasure()
        {
            
        }
        //소멸자
        ~Sequence_LaserPowerMeasure()
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

        #region Tick Count Check
        public enum TickType : int
        {
            TICK_NONE = 0,              
            TICK_LASER_POWER_MEASURE,
            TICK_POWER_MEASURE_START,
            TICK_POWER_MEASURE_LAST_SAVE
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
        private const int nLaserPowermeasureTimeout = 60000; // 5초
        #endregion


        public void InitSpiralLab(SpiralLabScanner spiralLabScanner)
        {
            _scanner = spiralLabScanner;
        }

        public bool IsCompleted { get; private set; } = false;
        public void Start()
        {
            IsCompleted = false;
            m_LaserPowerMeasure_Step = LaserPowerMeasure_Step.Start;
            m_MainTick_Start = true;
        }
        public LaserPowerMeasure_Step GetCurrentStep()
        {
            return m_LaserPowerMeasure_Step;
        }
        public void SetStep(LaserPowerMeasure_Step newStep)
        {
            m_LaserPowerMeasure_Step = newStep;
        }
        public bool IsStep(LaserPowerMeasure_Step checkStep)
        {
            return m_LaserPowerMeasure_Step == checkStep;
        }
        public void Reset()
        {
            IsCompleted = false;
            m_LaserPowerMeasure_Step = LaserPowerMeasure_Step.None;
            m_MainTick_Start = false;
        }
        int SeqLaserPowerMeasure()
        {
            int nRtn = 0;
            LaserPowerMeasure_Step nStep = LaserPowerMeasure_Step.None;

            if (workStage.rtc == null)
            {
                Log.Write("SeqLaserPowerMeasure", "workStage.rtc is not initialized.");
                return -1;
            }

            if (_scanner == null)
            {
                Log.Write("SeqLaserPowerMeasure", "SpiralLabScanner is not initialized.");
                return -1;
            }

            switch (m_LaserPowerMeasure_Step)
            {
                case LaserPowerMeasure_Step.None:
                    
                    break;

                case LaserPowerMeasure_Step.Start:
                    if (workStage.m_bHomeOK == false)
                    {
                        Log.Write("SeqLaserPowerMeasure", "WorkStage Home Position is not OK.");
                        return -1; // Home이 안되어 있으면 종료
                    }
                    else
                    {
                        _setting.LoadPowerMeterConfig();

                        TickCount_Start((int)TickType.TICK_LASER_POWER_MEASURE);
                        m_LaserPowerMeasure_Step = LaserPowerMeasure_Step.WaterLine_Open;
                    }
                    break;

                case LaserPowerMeasure_Step.WaterLine_Open:
                    if (WaterLine_Open() != 0)
                    {
                        if (TickCount_Elapsed((int)TickType.TICK_LASER_POWER_MEASURE) > nLaserPowermeasureTimeout)
                        {
                            Log.Write("SeqLaserPowerMeasure", "WaterLine Open Fail.");
                            return -1; // 수로 Open 실패
                        }
                    }
                    else
                    {
                        TickCount_Start((int)TickType.TICK_LASER_POWER_MEASURE);
                        m_LaserPowerMeasure_Step = LaserPowerMeasure_Step.WaterLine_Open_Check;
                    }
                    break;

                case LaserPowerMeasure_Step.WaterLine_Open_Check:
                    if (WaterLine_Open_Check() != 0)
                    {
                        if (TickCount_Elapsed((int)TickType.TICK_LASER_POWER_MEASURE) > nLaserPowermeasureTimeout)
                        {
                            Log.Write("SeqLaserPowerMeasure", "WaterLine Open Check Fail.");
                            return -1; // 수로 Open 확인 실패
                        }
                    }
                    else
                    {
                        TickCount_Start((int)TickType.TICK_LASER_POWER_MEASURE);
                        m_LaserPowerMeasure_Step = LaserPowerMeasure_Step.LaserOn;
                    }
                    break;

                case LaserPowerMeasure_Step.LaserOn:
                    if (LaserOn() != 0)
                    {
                        if (TickCount_Elapsed((int)TickType.TICK_LASER_POWER_MEASURE) > nLaserPowermeasureTimeout)
                        {
                            Log.Write("SeqLaserPowerMeasure", "Laser On Fail.");
                            return -1; // 레이저 On 실패
                        }
                    }
                    else
                    {
                        TickCount_Start((int)TickType.TICK_LASER_POWER_MEASURE);
                        m_LaserPowerMeasure_Step = LaserPowerMeasure_Step.LaserOn_Check;
                    }
                    break;

                case LaserPowerMeasure_Step.LaserOn_Check:
                    if (LaserOn_Check() != 0)
                    {
                        if (TickCount_Elapsed((int)TickType.TICK_LASER_POWER_MEASURE) > nLaserPowermeasureTimeout)
                        {
                            Log.Write("SeqLaserPowerMeasure", "Laser On Check Fail.");
                            return -1; // 레이저 On 확인 실패
                        }
                    }
                    else
                    {
                        TickCount_Start((int)TickType.TICK_LASER_POWER_MEASURE);
                        m_LaserPowerMeasure_Step = LaserPowerMeasure_Step.LaserPowerMeasure_Position;

                    }
                    break;

                case LaserPowerMeasure_Step.LaserPowerMeasure_Position:
                    if (LaserPowerMeasure_Position(ref nStep) != 0)
                    {
                        if (TickCount_Elapsed((int)TickType.TICK_LASER_POWER_MEASURE) > nLaserPowermeasureTimeout)
                        {

                            Log.Write("SeqLaserPowerMeasure", "Laser Power Measure Position Fail.");
                            return -1; // 레이저 Power 측정 위치 설정 실패
                        }
                    }
                    else
                    {
                        TickCount_Start((int)TickType.TICK_LASER_POWER_MEASURE);
                        m_LaserPowerMeasure_Step = nStep;
                    }
                       
                    break;

                case LaserPowerMeasure_Step.TopCheck_LaserShutter_Close:
                    if (TopCheck_LaserShutter_Close() != 0)
                    {
                        if (TickCount_Elapsed((int)TickType.TICK_LASER_POWER_MEASURE) > nLaserPowermeasureTimeout)
                        {
                            Log.Write("SeqLaserPowerMeasure", "Top Check Laser Shutter Close Fail.");
                            return -1; // Top Check 레이저 Shutter Close 실패
                        }
                    }
                    else
                    {
                        TickCount_Start((int)TickType.TICK_LASER_POWER_MEASURE);
                        m_LaserPowerMeasure_Step = LaserPowerMeasure_Step.TopCheck_LaserShutter_Close_Check;
                    }
                    break;

                case LaserPowerMeasure_Step.TopCheck_LaserShutter_Close_Check:
                    if (TopCheck_LaserShutter_Close_Check() != 0)
                    {
                        if (TickCount_Elapsed((int)TickType.TICK_LASER_POWER_MEASURE) > nLaserPowermeasureTimeout)
                        {
                            Log.Write("SeqLaserPowerMeasure", "Top Check Laser Shutter Close Check Fail.");
                            return -1; // Top Check 레이저 Shutter Close 확인 실패
                        }
                    }
                    else
                    {
                        TickCount_Start((int)TickType.TICK_LASER_POWER_MEASURE);
                        //TickCount_Start((int)TickType.TICK_POWER_MEASURE_START);
                        //TickCount_Start((int)TickType.TICK_POWER_MEASURE_LAST_SAVE);
                        m_LaserPowerMeasure_Step = LaserPowerMeasure_Step.LaserPowerMeasure_Start;
                    }
                    break;

                case LaserPowerMeasure_Step.LaserPowerMeasure_Start:
                    bool bComp = false;
                    if (LaserPowerMeasure_Start(ref bComp) != 0)
                    {
                        if (TickCount_Elapsed((int)TickType.TICK_LASER_POWER_MEASURE) > nLaserPowermeasureTimeout * 5)
                        {
                            Log.Write("SeqLaserPowerMeasure", "Top Check Laser Power Measure Start Fail.");
                            return -1; // Top Check 레이저 Power 측정 시작 실패
                        }
                    }
                    else if(bComp)
                    {
                        TickCount_Start((int)TickType.TICK_LASER_POWER_MEASURE);
                        m_LaserPowerMeasure_Step = LaserPowerMeasure_Step.Complete;
                    }
                    else
                    {
                        TickCount_Start((int)TickType.TICK_LASER_POWER_MEASURE);
                        m_LaserPowerMeasure_Step = LaserPowerMeasure_Step.LaserPowerMeasure_Start_Check;
                    }
                    break;

                case LaserPowerMeasure_Step.LaserPowerMeasure_Start_Check:
                    if (TopCheck_LaserPowerMeasure_Start_Check() != 0)
                    {
                        if (TickCount_Elapsed((int)TickType.TICK_LASER_POWER_MEASURE) > nLaserPowermeasureTimeout * 5)
                        {
                            Log.Write("SeqLaserPowerMeasure", "Top Check Laser Power Measure Start Check Fail.");
                            return -1; // Top Check 레이저 Power 측정 시작 확인 실패
                        }
                        
                    }
                    else
                    {
                        //TickCount_Start((int)TickType.TICK_LASER_POWER_MEASURE);
                        m_LaserPowerMeasure_Step = LaserPowerMeasure_Step.LaserPowerMeasure_Start;
                    }
                    break;

                case LaserPowerMeasure_Step.LaserShutter_Open:
                    if (LaserShutter_Open() != 0)
                    {
                        if (TickCount_Elapsed((int)TickType.TICK_LASER_POWER_MEASURE) > nLaserPowermeasureTimeout)
                        {
                            Log.Write("SeqLaserPowerMeasure", "Laser Shutter Open Fail.");
                            return -1; // 레이저 Shutter Open 실패
                        }
                    }
                    else
                    {
                        TickCount_Start((int)TickType.TICK_LASER_POWER_MEASURE);
                        m_LaserPowerMeasure_Step = LaserPowerMeasure_Step.LaserShutter_Open_Check;
                    }
                    break;

                case LaserPowerMeasure_Step.LaserShutter_Open_Check:
                    if (LaserShutter_Open_Check() != 0)
                    {
                        if (TickCount_Elapsed((int)TickType.TICK_LASER_POWER_MEASURE) > nLaserPowermeasureTimeout)
                        {
                            Log.Write("SeqLaserPowerMeasure", "Laser Shutter Open Check Fail.");
                            return -1; // 레이저 Shutter Open 확인 실패
                        }
                    }
                    else
                    {
                        if(Equipment.Machine_LaserType_CO2)
                        {
                            TickCount_Start((int)TickType.TICK_LASER_POWER_MEASURE);
                            m_LaserPowerMeasure_Step = LaserPowerMeasure_Step.Mask_Change;
                        }
                        else
                        {
                            TickCount_Start((int)TickType.TICK_LASER_POWER_MEASURE);
                            m_LaserPowerMeasure_Step = LaserPowerMeasure_Step.StageZ_Move_ProcessPos;
                        }
                    }
                    break;

                case LaserPowerMeasure_Step.Mask_Change:
                    if (Mask_Change() != 0)
                    {
                        if (TickCount_Elapsed((int)TickType.TICK_LASER_POWER_MEASURE) > nLaserPowermeasureTimeout)
                        {
                            Log.Write("SeqLaserPowerMeasure", "Mask Change Fail.");
                            return -1; // Mask 변경 실패
                        }
                    }
                    else
                    {
                        TickCount_Start((int)TickType.TICK_LASER_POWER_MEASURE);
                        m_LaserPowerMeasure_Step = LaserPowerMeasure_Step.Mask_Change_Check;
                    }
                    break;

                case LaserPowerMeasure_Step.Mask_Change_Check:
                    if (Mask_Change_Check() != 0)
                    {
                        if (TickCount_Elapsed((int)TickType.TICK_LASER_POWER_MEASURE) > nLaserPowermeasureTimeout)
                        {
                            Log.Write("SeqLaserPowerMeasure", "Mask Change Check Fail.");
                            return -1; // Mask 변경 확인 실패
                        }
                    }
                    else
                    {
                        TickCount_Start((int)TickType.TICK_LASER_POWER_MEASURE);
                        m_LaserPowerMeasure_Step = LaserPowerMeasure_Step.BETA_Change;
                    }
                    break;

                case LaserPowerMeasure_Step.BETA_Change:
                    if (BETA_Change() != 0)
                    {
                        if (TickCount_Elapsed((int)TickType.TICK_LASER_POWER_MEASURE) > nLaserPowermeasureTimeout)
                        {
                            Log.Write("SeqLaserPowerMeasure", "BETA Change Fail.");
                            return -1; // BET A 변경 실패
                        }
                    }
                    else
                    {
                        TickCount_Start((int)TickType.TICK_LASER_POWER_MEASURE);
                        m_LaserPowerMeasure_Step = LaserPowerMeasure_Step.BETA_Change_Check;
                    }
                    break;

                case LaserPowerMeasure_Step.BETA_Change_Check:
                    if (BETA_Change_Check() != 0)
                    {
                        if (TickCount_Elapsed((int)TickType.TICK_LASER_POWER_MEASURE) > nLaserPowermeasureTimeout)
                        {
                            Log.Write("SeqLaserPowerMeasure", "BETA Change Check Fail.");
                            return -1; // BET A 변경 확인 실패
                        }
                    }
                    else
                    {
                        TickCount_Start((int)TickType.TICK_LASER_POWER_MEASURE);
                        m_LaserPowerMeasure_Step = LaserPowerMeasure_Step.Vario_Change;
                    }
                    break;

                case LaserPowerMeasure_Step.Vario_Change:
                    if (Vario_Change() != 0)
                    {
                        if (TickCount_Elapsed((int)TickType.TICK_LASER_POWER_MEASURE) > nLaserPowermeasureTimeout)
                        {
                            Log.Write("SeqLaserPowerMeasure", "Vario Change Fail.");
                            return -1; // Vario 변경 실패
                        }
                    }
                    else
                    {
                        TickCount_Start((int)TickType.TICK_LASER_POWER_MEASURE);
                        m_LaserPowerMeasure_Step = LaserPowerMeasure_Step.Vario_Change_Check;
                    }
                    break;

                case LaserPowerMeasure_Step.Vario_Change_Check:
                    if (Vario_Change_Check() != 0)
                    {
                        if (TickCount_Elapsed((int)TickType.TICK_LASER_POWER_MEASURE) > nLaserPowermeasureTimeout)
                        {
                            Log.Write("SeqLaserPowerMeasure", "Vario Change Check Fail.");
                            return -1; // Vario 변경 확인 실패
                        }
                    }
                    else
                    {
                        TickCount_Start((int)TickType.TICK_LASER_POWER_MEASURE);
                        m_LaserPowerMeasure_Step = LaserPowerMeasure_Step.StageZ_Move_ProcessPos;
                    }
                    break;

                case LaserPowerMeasure_Step.StageZ_Move_ProcessPos:
                    if (StageZ_Move_ProcessPos() != 0)
                    {
                        if (TickCount_Elapsed((int)TickType.TICK_LASER_POWER_MEASURE) > nLaserPowermeasureTimeout)
                        {
                            Log.Write("SeqLaserPowerMeasure", "Stage Z Move Process Position Fail.");
                            return -1; // Stage Z 이동 실패
                        }
                    }
                    else
                    {
                        TickCount_Start((int)TickType.TICK_LASER_POWER_MEASURE);
                        m_LaserPowerMeasure_Step = LaserPowerMeasure_Step.StageZ_Move_ProcessPos_Check;
                    }
                    break;

                case LaserPowerMeasure_Step.StageZ_Move_ProcessPos_Check:
                    if (StageZ_Move_ProcessPos_Check() != 0)
                    {
                        if (TickCount_Elapsed((int)TickType.TICK_LASER_POWER_MEASURE) > nLaserPowermeasureTimeout)
                        {
                            Log.Write("SeqLaserPowerMeasure", "Stage Z Move Process Position Check Fail.");
                            return -1; // Stage Z 이동 확인 실패
                        }
                    }
                    else
                    {
                        TickCount_Start((int)TickType.TICK_LASER_POWER_MEASURE);
                        m_LaserPowerMeasure_Step = LaserPowerMeasure_Step.StageXY_Move_PowerMeterPos;
                    }
                    break;

                case LaserPowerMeasure_Step.StageXY_Move_PowerMeterPos:
                    if (StageXY_Move_PowerMeterPos() != 0)
                    {
                        if (TickCount_Elapsed((int)TickType.TICK_LASER_POWER_MEASURE) > nLaserPowermeasureTimeout)
                        {
                            Log.Write("SeqLaserPowerMeasure", "Stage XY Move Power Meter Position Fail.");
                            return -1; // Stage XY 이동 실패
                        }
                    }
                    else
                    {
                        TickCount_Start((int)TickType.TICK_LASER_POWER_MEASURE);
                        m_LaserPowerMeasure_Step = LaserPowerMeasure_Step.StageXY_Move_PowerMeterPos_Check;
                    }
                    break;

                case LaserPowerMeasure_Step.StageXY_Move_PowerMeterPos_Check:
                    if (StageXY_Move_PowerMeterPos_Check() != 0)
                    {
                        if (TickCount_Elapsed((int)TickType.TICK_LASER_POWER_MEASURE) > nLaserPowermeasureTimeout)
                        {
                            Log.Write("SeqLaserPowerMeasure", "Stage XY Move Power Meter Position Check Fail.");
                            return -1; // Stage XY 이동 확인 실패
                        }
                    }
                    else
                    {
                        TickCount_Start((int)TickType.TICK_LASER_POWER_MEASURE);
                        //TickCount_Start((int)TickType.TICK_POWER_MEASURE_START);
                        //TickCount_Start((int)TickType.TICK_POWER_MEASURE_LAST_SAVE);
                        m_LaserPowerMeasure_Step = LaserPowerMeasure_Step.LaserPowerMeasure_Start;
                    }
                    break;

                case LaserPowerMeasure_Step.Complete:

                    if (!workStage.rtc.CtlGetStatus(RtcStatus.Busy))
                    {
                        IsCompleted = true;
                        Log.Write("SeqLaserPowerMeasure", "Laser Power Measure Sequence Complete.");
                        m_LaserPowerMeasure_Step = LaserPowerMeasure_Step.None; // 완료 후 초기화
                        return 0; // 성공적으로 완료
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LASER_POWER_MEASURE) > nLaserPowermeasureTimeout)
                    {
                        IsCompleted = true;
                        Log.Write("SeqLaserPowerMeasure", "Laser Power Measure Sequence is still busy.");
                        m_LaserPowerMeasure_Step = LaserPowerMeasure_Step.None; // 완료 후 초기화
                        return -1; // 아직 Busy 상태
                    }
                    break;

                default:
                    Log.Write("SeqLaserPowerMeasure", "Unknown Step in Laser Power Measure Sequence.");
                    return -1; // 알 수 없는 단계
            }


            return nRtn;
        }

        private int StageXY_Move_PowerMeterPos_Check()
        {
            int nRtn = 0;

            if (workStage.IsWorkStage_TeachingPositionsXY((int)WorkStage_TeachingPosList.STAGE_Scanner_PMPos))
            {
                nRtn = 0;
            }
            else
            {
                string strTemp = string.Format("Stage XY Move Center Position 실패");
                Log.Write("SeqLaserPowerMeasure", "StageXY_Move_PowerMeterPos_Check", strTemp);
                nRtn = -1;
                return workStage.AlarmPost(AlarmKey.eStageMoveFail);
            }

            return nRtn;
        }

        private int StageXY_Move_PowerMeterPos()
        {
            int nRtn = 0;

            workStage.MovetoWorkStage_TeachingPositionsXY((int)WorkStage_TeachingPosList.STAGE_Scanner_PMPos, Type_Motor_Speed.Coarse);

            return nRtn;
        }

        private int StageZ_Move_ProcessPos_Check()
        {
            int nRtn = 0;
            string strTemp = string.Empty;
            int nZPos = 0;
            nZPos = (int)QMC.Common.Modules.Vision.Vision_TeachingPosList.Vision_SafetyPos;

            if (workStage.IsWorkStage_TeachingPositionsZ((int)nZPos))
            {
                nRtn = 0;
            }
            else
            {
                strTemp = string.Format("Stage Z 축, Laser Height Check 높이로 이동 실패");
                Log.Write("SeqLaserPowerMeasure", "StageZ_Move_ProcessPos_Check", strTemp);

                nRtn = -1;
                return workStage.AlarmPost(WorkStage.AlarmKey.eZAxisFail);
            }

            return nRtn;
        }

        private int StageZ_Move_ProcessPos()
        {
            int nRtn = 0;
            int nZPos = 0;
            
            nZPos = (int)QMC.Common.Modules.Vision.Vision_TeachingPosList.Vision_SafetyPos;
            workStage.MovetoWorkStage_TeachingPositionsZ(nZPos, Type_Motor_Speed.Fine);

            return nRtn;
        }

        private int Vario_Change_Check()
        {
            int nRtn = 0;

            if (bds.spiralLabVario.GetCurrentZOffset() == bds.spiralLabVario.fSetZOffset)
            {
                nRtn = 0;
            }
            else
            {
                nRtn = -1;
                return workStage.AlarmPost(WorkStage.AlarmKey.Vario_Scan_Fail);
            }

            return nRtn;
        }

        private int Vario_Change()
        {
            int nRtn = 0;

            if (true)
            {
                nRtn = 0;
            }
            else
            {
                bds.spiralLabVario.fSetZOffset = 0;
                bds.spiralLabVario.SetZOffset(bds.spiralLabVario.fSetZOffset);    // 설정값 받아와서 셋팅 필요.
            }

            return nRtn;
        }

        private int BETA_Change_Check(int nBETIndex = 0)
        {
            int nRtn = 0;
            string strTemp = string.Empty;
            if (nBETIndex < 0 || nBETIndex >= 5)                                    //  BET 배율은 총4개로 고정되어 있음.
            {
                nRtn = -1; // BET 변경 실패
                Log.Write("SeqLaserPowerMeasure", Equipment.User_Name, "BETA_Change_Check", "지정되지 않은 BET Index 입니다. (0 ~ 4)");
                return nRtn;
            }

            double dBET_Zoom = 0.0;
            double dBET_Mrad = 0.0;
            //  BET Zoom, Mrad 변경
            switch (nBETIndex)
            {
                case 0:         //  0.8x
                    dBET_Zoom = 0.8;
                    dBET_Mrad = Equipment.BET_0_8X_Mrad;
                    break;
                case 1:         //  0.9x
                    dBET_Zoom = 0.9;
                    dBET_Mrad = Equipment.BET_0_9X_Mrad;
                    break;
                case 2:         //  1.0x
                    dBET_Zoom = 1.0;
                    dBET_Mrad = Equipment.BET_1_0X_Mrad;
                    break;
                case 3:         //  1.1x
                    dBET_Zoom = 1.1;
                    dBET_Mrad = Equipment.BET_1_1X_Mrad;
                    break;
                case 4:         //  1.2x
                    dBET_Zoom = 1.2;
                    dBET_Mrad = Equipment.BET_1_2X_Mrad;
                    break;
            }

            if (((workStage.m_dBET_ZoomValue > (dBET_Zoom - 0.005)) && (workStage.m_dBET_ZoomValue < (dBET_Zoom + 0.005))) &&
                ((workStage.m_dBET_MradValue > (dBET_Mrad - 0.005)) && (workStage.m_dBET_MradValue < (dBET_Mrad + 0.005))))
            {
                strTemp = string.Format("BET Zoom ({0} / {1}), Mrad ({2} / {3}) 변경 성공", workStage.m_dBET_ZoomValue, dBET_Zoom, workStage.m_dBET_MradValue, dBET_Mrad);
                Log.Write("SeqLaserPowerMeasure", Equipment.User_Name, "BETA_Change_Check", strTemp);

                workStage.m_nBETChange_RetryCount = 0;
                nRtn = 0; // BET 변경 성공
            }
            else
            {
                if (workStage.m_nBETChange_RetryCount++ < 3)
                {
                    strTemp = string.Format("BET Zoom ({0} / {1}), Mrad ({2} / {3}) 변경 실패. 재시도 ({4}/{5})",
                        workStage.m_dBET_ZoomValue, dBET_Zoom, workStage.m_dBET_MradValue, dBET_Mrad, workStage.m_nBETChange_RetryCount, 3);
                    Log.Write("SeqLaserPowerMeasure", Equipment.User_Name, "BETA_Change_Check", strTemp);

                    Thread.Sleep(200);
                    nRtn = -1;
                }
                else
                {
                    strTemp = string.Format("BET Zoom ({0} / {1}), Mrad ({2} / {3}) 변경 실패. 재시도 ({4}/{5})",
                        workStage.m_dBET_ZoomValue, dBET_Zoom, workStage.m_dBET_MradValue, dBET_Mrad, workStage.m_nBETChange_RetryCount, 3);
                    Log.Write("VerifyScannerCameraOffset", Equipment.User_Name, "ScannerCalibration", strTemp);

                    nRtn = -1;
                    return workStage.AlarmPost(WorkStage.AlarmKey.eBETChangeFail);
                }
            }

            return nRtn;
        }

        private int BETA_Change()
        {
            int nRtn = 0;

            int nBETIndex = _setting.BETIndex;  //0; // BET 배율 설정 필요.
            if (nBETIndex < 0 || nBETIndex >= 5)                                    //  BET 배율은 총4개로 고정되어 있음.
            {
                nRtn = -1; // BET 변경 실패
                Log.Write("SeqLaserPower", Equipment.User_Name, "BETA_Change", "지정되지 않은 BET Index 입니다. (0 ~ 5)");
            }
            else
            {
                workStage.LaserDrillingStepBETChange(nBETIndex);
            }

            return nRtn;
        }

        private int Mask_Change_Check()
        {
            int nRtn = 0;
            int nMaskIndex = (int)Bds.BDS_TeachingPosList.BDS_Mask4Pos;
            double dPosMaskY = bds.stBDSTeachingPos[nMaskIndex].Mask_Y;
            if (workStage.IsWorkStage_Positions(WorkStage.nAxis.MASK_Y, dPosMaskY))
            {
                nRtn = 0; // Mask 변경 확인 성공
            }
            else
            {
                nRtn = -1; // Mask 변경 확인 실패
            }

            return nRtn;
        }

        private int Mask_Change()
        {
            int nRtn = 0;

            //Powermeter 측정을 위한 Mask 변경
            int nMaskIndex = (int)_setting.MaskIndex;  //Bds.BDS_TeachingPosList.BDS_Mask4Pos;
            if(nMaskIndex < 0 || nMaskIndex >= bds.stBDSTeachingPos.Length)
            {
                Log.Write("SeqLaserPowerMeasure", Equipment.User_Name, "Mask_Change", "지정되지 않은 Mask Index 입니다. (0 ~ 4)");
                return -1; // Mask 변경 실패
            }

            double dPosMaskY = bds.stBDSTeachingPos[nMaskIndex].Mask_Y;
            Type_Motor_Speed speed = Type_Motor_Speed.Coarse;
            workStage.MovetoWorkStage_ABS_PositionsMaskY(dPosMaskY, speed);

            return nRtn;
        }

        private int LaserShutter_Open_Check()
        {
            int nRtn = 0;

            if (workStage.workStageParameter.DI_BDS_PowerMeter_BW_Check() &&
                !workStage.workStageParameter.DI_BDS_PowerMeter_FW_Check())
            {
                nRtn = 0;
            }
            else
            {
                nRtn = -1; // 레이저 Power 측정 위치 설정 실패
            }

            return nRtn;
        }

        private int LaserShutter_Open()
        {
            int nRtn = 0;

            workStage.workStageParameter.DO_BDS_PowerMeter_BW(true);
            workStage.workStageParameter.DO_BDS_PowerMeter_FW(false);

            return nRtn;
        }

        private int TopCheck_LaserPowerMeasure_Start_Check()
        {
            int nRtn = 0;



            return nRtn;
        }

        private int _powerMeasureLogIndex = 0;
        private const int _powerMeasureInitialDelayMs = 20000;  // 20초 대기 시간
        private bool _isPowerMeasureDelayed = false;            // 지연 완료 여부
        private bool _isPowerMeasureOnes = false;            // 지연 완료 여부
        public int _powerMeasureLogIntervalMs = 5000;   // 3초 간격
        public int _powerMeasureLogTotalTimeMs = 60000; // 총 30초
        private int LaserPowerMeasure_Start(ref bool bComp)
        {
            int nRtn = 0;
            bool result = false;
            string strTemp = string.Empty;

            if (_powerMeasureLogIndex == 0 && !_isPowerMeasureDelayed && !_isPowerMeasureOnes)
            {
                if (!Equipment.Machine_LaserType_CO2) // UV인 경우.
                {
                    if (_setting.PowerPercent < 0 || _setting.PowerPercent > 100)
                    {
                        strTemp = string.Format("PowerPercent는 0에서 100 사이의 값이어야 합니다.");
                        Log.Write("LaserPowerMeasure", "LaserPowerMeasure_Start", strTemp);
                        return -1;
                    }

                    result = SetLaserPower(_setting.PowerPercent);
                    if (!result)
                    {
                        strTemp = string.Format("레이저 파워 변경-Error");
                        Log.Write("LaserPowerMeasure", "LaserPowerMeasure_Start", strTemp);
                        return -1;
                    }
                }
                else
                {
                    if (Equipment.Machine_LaserType_CO2)
                    {
                        if (!(_setting.DutyCycle >= 2.5f && _setting.DutyCycle < 20.0f))
                        {
                            strTemp = $"DutyCycle은 2.5% 이상, 20% 미만이어야 합니다.\r\n현재 설정: {_setting.DutyCycle:F2}%";
                            Log.Write("LaserPowerMeasure", "LaserPowerMeasure_Start", strTemp);
                            return -1;
                            //mb.ShowDialog("Error!", strTemp);
                            //return;
                        }
                    }
                }

                float duration = (float)_powerMeasureLogTotalTimeMs * 0.9f;
                result = _scanner.LaserOn(duration, _setting);

                Log.Write("LaserPowerMeasure", "LaserPowerMeasure_Start", "파워 측정 시작: 20초 대기 후 5초 간격으로 측정 시작");
                _isPowerMeasureOnes = true;
                TickCount_Start((int)TickType.TICK_POWER_MEASURE_LAST_SAVE);  // 최초 시작 시간
            }

            // 아직 20초 대기 중이면 대기
            if (!_isPowerMeasureDelayed)
            {
                if (TickCount_Elapsed((int)TickType.TICK_POWER_MEASURE_LAST_SAVE) < _powerMeasureInitialDelayMs)
                {
                    bComp = false;
                    return 0; // 아직 대기 중
                }

                // 20초 경과 후, 측정 시작
                Log.Write("LaserPowerMeasure", "LaserPowerMeasure_Start", "20초 경과: 파워 측정 시작 (5초 간격)");
                TickCount_Start((int)TickType.TICK_POWER_MEASURE_START);  // 측정 시작 타이머
                _isPowerMeasureDelayed = true;

                _isPowerMeasureOnes = false;
            }

            // 30초 이후 종료 (20초 대기 + 10초 측정)
            if (TickCount_Elapsed((int)TickType.TICK_POWER_MEASURE_LAST_SAVE) >= _powerMeasureLogTotalTimeMs * 0.8)
            {
                string position = (_setting.PowerMeterType == 0) ? "Top" : "Stage";
                SavePowerMeasureLogList(position);
                Log.Write("LaserPowerMeasure", "LaserPowerMeasure_Start", $"{position} 위치에서 파워 측정 완료");

                _measuredPowerList.Clear();
                bComp = true;

                // 모든 상태 초기화
                _powerMeasureLogIndex = 0;
                _isPowerMeasureDelayed = false;
                return 0;
            }

            // 5초 간격 측정
            if (TickCount_Elapsed((int)TickType.TICK_POWER_MEASURE_START) >= _powerMeasureLogIntervalMs)
            {
                double power = (_setting.PowerMeterType == 0) ? workStage.m_dPowerMeterBDS_Value : workStage.m_dPowerMeterStage_Value;

                AppendPowerMeasure(power);
                OnPowerMeasured?.Invoke((float)power);

                _powerMeasureLogIndex++;
                TickCount_Start((int)TickType.TICK_POWER_MEASURE_START); // 다음 5초 타이머 리셋
            }

            //기존 코드
            {
                // 처음 진입 시 초기화
                //if (_powerMeasureLogIndex == 0)
                //{
                //    Log.Write("LaserPowerMeasure", "TopCheck_LaserPowerMeasure_Start", "파워 측정 시작: 30초 동안 3초 간격 10회 측정");
                //}

                //// 30초가 지났으면 완료 처리
                //if (TickCount_Elapsed((int)TickType.TICK_POWER_MEASURE_LAST_SAVE) >= _powerMeasureLogTotalTimeMs)
                //{
                //    if (m_nType == 0) {
                //        SavePowerMeasureLogList("Top"); // 또는 "Stage"
                //        Log.Write("LaserPowerMeasure", "TopCheck_LaserPowerMeasure_Start", "Top 위치에서 파워 측정 완료");
                //    }
                //    else
                //    {
                //        SavePowerMeasureLogList("Stage"); // 또는 "Stage"
                //        Log.Write("LaserPowerMeasure", "TopCheck_LaserPowerMeasure_Start", "Stage 위치에서 파워 측정 완료");
                //    }
                //    _measuredPowerList.Clear();     // 다음 측정을 위한 초기화

                //    bComp = true;
                //    _powerMeasureLogIndex = 0; // 다음 측정 위해 초기화
                //    return 0; // 성공 완료
                //}

                //// 3초마다 측정 저장
                //if (TickCount_Elapsed((int)TickType.TICK_POWER_MEASURE_START) >= _powerMeasureLogIntervalMs)
                //{
                //    double power = 0.0;
                //    if (m_nType == 0)
                //    {
                //        power = workStage.m_dPowerMeterBDS_Value;
                //    }
                //    else
                //    {
                //        power = workStage.m_dPowerMeterStage_Value;
                //    }

                //    AppendPowerMeasure(power); // 매 측정마다 누적

                //    OnPowerMeasured?.Invoke((float)power);

                //    bComp = false;
                //    _powerMeasureLogIndex++;
                //}
            }

            return nRtn;
        }

        private int TopCheck_LaserShutter_Close_Check()
        {
            int nRtn = 0;

            if (!workStage.workStageParameter.DI_BDS_PowerMeter_BW_Check() &&
                workStage.workStageParameter.DI_BDS_PowerMeter_FW_Check())
            {
                nRtn = 0;
            }
            else
            {
                nRtn = -1; // 레이저 Power 측정 위치 설정 실패
            }

            return nRtn;
        }

        private int TopCheck_LaserShutter_Close()
        {
            int nRtn = 0;

            workStage.workStageParameter.DO_BDS_PowerMeter_BW(false);
            workStage.workStageParameter.DO_BDS_PowerMeter_FW(true);

            return nRtn;
        }

        private int LaserPowerMeasure_Position(ref LaserPowerMeasure_Step nStep)
        {
            int nRtn = 0;

            //Top이냐 Stage냐에 따라 Power Meter 위치가 다름
            if(Equipment.Machine_LaserType_CO2)
            {
                _setting.PowerMeterType = (int)LaserPowerMeasure_Type.Stage;
            }

            switch(_setting.PowerMeterType)
            {
                case (int)LaserPowerMeasure_Type.Top:
                    {
                        nStep = LaserPowerMeasure_Step.TopCheck_LaserShutter_Close;
                        nRtn = 0;
                    }
                    break;
                case (int)LaserPowerMeasure_Type.Stage:
                    {
                        nStep = LaserPowerMeasure_Step.LaserShutter_Open;
                        nRtn = 0;
                    }
                    break;
                default:
                    {
                        nRtn = -1; // 알 수 없는 Power Meter 위치
                        Log.Write("SeqLaserPowerMeasure", "LaserPowerMeasure_Position", "알 수 없는 Power Meter 위치입니다.");
                    }
                    break;
            }

            return nRtn;
        }

        private int LaserOn_Check()
        {
            int nRtn = 0;



            return nRtn;
        }

        private int LaserOn()
        {
            int nRtn = 0;



            return nRtn;
        }

        private int WaterLine_Open_Check()
        {
            int nRtn = 0;

            if (workStage.workStageParameter.IsDO_BeamDump_Coolant_Supply() &&
                workStage.workStageParameter.IsDO_Scanner_Coolant_Supply() &&
                (!Equipment.Machine_LaserType_CO2 ||
                Equipment.Machine_LaserType_CO2 &&
                workStage.workStageParameter.IsDO_Mask_Coolant_Supply() &&
                workStage.workStageParameter.IsDO_VarioScan_Coolant_Supply()))
            {
                nRtn = 0;
            }
            else
            {
                nRtn = -1; // 수로 Open 확인 실패
            }

            return nRtn;
        }

        private int WaterLine_Open()
        {
            int nRtn = 0;

            workStage.workStageParameter.DO_BeamDump_Coolant_Supply(true);                    //  Laser Cooling Valve Open
            workStage.workStageParameter.DO_Scanner_Coolant_Supply(true);                     //  Scanner Cooling Valve Open

            if (Equipment.Machine_LaserType_CO2)
            {
                workStage.workStageParameter.DO_Mask_Coolant_Supply(true);                    //  Beam Mask 
                workStage.workStageParameter.DO_VarioScan_Coolant_Supply(true);
            }

            return nRtn;
        }

        private List<double> _measuredPowerList = new List<double>();

        public void SavePowerMeasureLogList(string targetType)
        {
            string logFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LaserPowerLog");
            if (!Directory.Exists(logFolder))
                Directory.CreateDirectory(logFolder);

            string logFile = Path.Combine(logFolder, $"LaserPowerMeasureLog_{DateTime.Now:yyyyMMdd}.csv");
            List<string> lines = new List<string>();

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // 측정 설정값
            float powerPercent = _setting.PowerPercent;
            float frequency = _setting.Frequency;
            float pulseWidth = _setting.PulseWidth;
            float dutyCycle = _setting.DutyCycle;

            // 기존 로그 파일이 없으면 헤더 추가
            if (!File.Exists(logFile))
            {
                string header = "Timestamp,TargetType,Label,Value,PowerPercent,Frequency,PulseWidth,DutyCycle";
                lines.Add(header);
            }

            // 10개 측정값
            for (int i = 0; i < _measuredPowerList.Count; i++)
            {
                string line = $"{timestamp},{targetType},Count_{i + 1},{_measuredPowerList[i]:F2}," +
                              $"{powerPercent:F1},{frequency:F1},{pulseWidth:F2},{dutyCycle:F2}";
                lines.Add(line);
            }

            // 평균값
            if (_measuredPowerList.Count > 0)
            {
                double avg = _measuredPowerList.Average();
                string avgLine = $"{timestamp},{targetType},Average,{avg:F2}," +
                                 $"{powerPercent:F1},{frequency:F1},{pulseWidth:F2},{dutyCycle:F2}";
                lines.Add(avgLine);
            }

            try
            {
                // 파일 공유 설정 적용 → UI 등에서 동시에 읽어도 예외 안 남
                using (FileStream fs = new FileStream(logFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                using (StreamWriter writer = new StreamWriter(fs, new UTF8Encoding(true)))
                {
                    foreach (string line in lines)
                        writer.WriteLine(line);
                }
                //File.AppendAllLines(logFile, lines, new UTF8Encoding(true));
                Log.Write("LaserPowerMeasure", $"파워 측정 로그 {lines.Count}줄 저장 완료 (대상: {targetType})");
            }
            catch (Exception ex)
            {
                Log.Write("LaserPowerMeasure", $"파워 측정 로그 저장 실패: {ex.Message}");
            }
        }

        // 기존 코드 (주석 처리)
        //public void SavePowerMeasureLogList(string targetType)
        //{
        //    string logFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LaserPowerLog");
        //    if (!Directory.Exists(logFolder))
        //        Directory.CreateDirectory(logFolder);

        //    string logFile = Path.Combine(logFolder, $"LaserPowerMeasureLog_{DateTime.Now:yyyyMMdd}.csv");
        //    List<string> lines = new List<string>();

        //    string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        //    // 10개 측정값 저장
        //    for (int i = 0; i < _measuredPowerList.Count; i++)
        //    {
        //        string line = $"{timestamp},{targetType},Count_{i + 1},{_measuredPowerList[i]:F2}";
        //        lines.Add(line);
        //    }

        //    // 평균값 저장
        //    if (_measuredPowerList.Count > 0)
        //    {
        //        double avg = _measuredPowerList.Average();
        //        string avgLine = $"{timestamp},{targetType},Average,{avg:F2}";
        //        lines.Add(avgLine);
        //    }

        //    try
        //    {
        //        File.AppendAllLines(logFile, lines, new UTF8Encoding(true));
        //        Log.Write("LaserPowerMeasure", $"파워 측정 로그 {lines.Count}줄 저장 완료 (대상: {targetType})");
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Write("LaserPowerMeasure", $"파워 측정 로그 저장 실패: {ex.Message}");
        //    }
        //}

        private void AppendPowerMeasure(double currentPower)
        {
            _measuredPowerList.Add(currentPower);
        }

        public void TestLog()
        {
            // 예시 데이터: 10개의 측정값 생성
            List<double> testPowers = new List<double> { 123.4, 125.1, 121.0, 124.8, 122.9, 126.5, 120.4, 124.1, 123.9, 122.7 };

            // 저장할 대상
            string targetType = "TestTarget";

            // 내부 리스트에 저장
            _measuredPowerList = testPowers;

            // 저장 함수 호출
            SavePowerMeasureLogList(targetType);
        }

        public bool SetLaserPower(float powerPercent)
        {
            bool bSuccess = false;
            string strTemp = string.Empty;
            if ((workStage.m_rapidLxLaser_Comm != null))
            {
                if (workStage.m_rapidLxLaser_Comm.IsOpen)
                {
                    workStage.RapidLxLaserComm_Laser_OutputEnergy_Set(powerPercent);

                    strTemp = string.Format("Laser Power 변경 시작, Laser Power ({0:0.000})",
                                            powerPercent);
                    Log.Write("SLD-200", "SetLaserPower", strTemp);
                    bSuccess = true;
                }
                else
                {
                    strTemp = string.Format("Laser Power 변경 실패. (Laser Comm 열리지 않음)");
                    Log.Write("SLD-200", "SetLaserPower", strTemp);
                }
            }

            return bSuccess;
        }
    }
}
