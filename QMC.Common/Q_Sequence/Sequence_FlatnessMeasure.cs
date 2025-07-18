using QMC.Common.Modules;
using QMC.Common.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using static QMC.Common.Equipment;
using static QMC.Common.Modules.WorkStage;
using static QMC.Common.Q_Sequence.Sequence_LaserPowerMeasure;

namespace QMC.Common.Q_Sequence
{
    public class Sequence_FlatnessMeasure
    {
        public enum FlatnessMeasure_Step
        {
            None = 0,
            Start,                                  //  시작

            Socket_Pause_Check,

            FlatnessMeasure_StageZ_MovetoLaserHeightSensorPos,                              //  Stage Z, Laser Height Sensor 를 측정 위치로 이동
            FlatnessMeasure_StageZ_MovetoLaserHeightSensorPos_DoneCheck,                    //  Stage Z, Laser Height Sensor 를 측정 위치로 이동 완료 확인

            FlatnessMeasure_RemainedCheck,                                                  //  측정할 위치가 남아있는지 체크

            FlatnessMeasure_StageXY_MovetoFlatnessMeasurePos,                               //  Stage XY, Laser Height Sensor 를 측정 위치로 이동
            FlatnessMeasure_StageXY_MovetoFlatnessMeasurePos_DoneCheck,                     //  Stage XY, Laser Height Sensor 를 측정 위치로 이동 완료 확인

            FlatnessMeasure_StableTime,                                                     //  값을 읽기 위해 안정화 시간 대기

            FlatnessMeasure_LaserHeightValue_Get,                                           //  높이값 가져오기


            FlatnessMeasure_LaserHeightValue_Calc,                                          //  높이값 편차 계산. (최대, 최소, 평균)

            Complete,
        }
        public int m_nFlatnessMeasure_Step { set; get; }

        static QMC.Common.Modules.WorkStage workStage;
        static QMC.Common.Modules.Vision vision;

        protected Task m_taskTimer_Main_Tick = null;
        private bool isModuleClose = false;
        protected List<Task> listTask = new List<Task>();
        public bool _isMainStatusRunning = false; // 중복 실행 방지 플래그
        public bool m_MainTick_Start = false;

        #region Tick Count Check
        public enum TickType : int
        {
            TICK_NONE = 0,
            TICK_FLATNESS_MEASURE
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

        public Sequence_FlatnessMeasure()
        {

        }

        ~Sequence_FlatnessMeasure()
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
                else if (module.Name == "Vision")
                {
                    vision = module as QMC.Common.Modules.Vision;
                }
            }

            //장비 RUN 진행 시 프로그램 죽을때까지 돌아야함.
            m_taskTimer_Main_Tick = Task.Factory.StartNew(() =>
            {
                Thread.CurrentThread.Name = "m_taskTimer_FlatnessMeasure_Tick";

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
                SeqFlatnessMeasurementFunc();
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


        #region Flatness Measurement 

        public bool IsCompleted { get; private set; } = false;
        public void Start()
        {
            IsCompleted = false;
            m_nFlatnessMeasure_Step = (int)LaserPowerMeasure_Step.Start;
            m_MainTick_Start = true;
        }
        public void Reset()
        {
            IsCompleted = false;
            m_MainTick_Start = false;
            m_nFlatnessMeasure_Step = (int)LaserPowerMeasure_Step.None;
        }


        public int m_nFlatnessMeasure_Count { set; get; } = 0; //  Flatness Measure Count
        public int m_nFlatnessMeasure_Type { set; get; } = 0; //  Flatness Measure Type (0:Work Stage, 1: Cal.Plate, 2: User1, 3: User2, 4: User3)
        public int m_nHeightValue_OK_Count { set; get; } = 0; //  Data OK Count
        public double m_dHeightValue_Min_Value { set; get; } = 0; //  Height Value Min
        public double m_dHeightValue_Max_Value { set; get; } = 0; //  Height Value Max
        public double m_dHeightValue_Sum { set; get; } = 0; //  Height Value Avg Min
        public double m_dHeightValue_Avg { set; get; } = 0; //  Height Value Avg
        XyCoordinate m_xyCoordinate = new XyCoordinate(0, 0);
        private int m_nStage_RetryCount = 0; //  Stage XY 축 이동 재시도 횟수

        int SeqFlatnessMeasurementFunc()
        {
            int nRtn = 0;
            string m_strTemp = "";

            double lfVelocity = 0.0;
            double lfAccDec = 0.0;

            double m_dDeviation = 0.0;

            if (workStage.m_SocketLaserHeightSensor == null)
            {
                m_nFlatnessMeasure_Step = (int)FlatnessMeasure_Step.None;
                return -1;
            }

            if (!workStage.m_SocketLaserHeightSensor.isConnected)
            {
                m_nFlatnessMeasure_Step = (int)FlatnessMeasure_Step.None;
                return -1;
            }

            switch (m_nFlatnessMeasure_Step)
            {
                case (int)FlatnessMeasure_Step.Start:

                    Log.Write("FlatnessMeasure", Equipment.User_Name, "측정 시작.");

                    m_nFlatnessMeasure_Count = 0;

                    if ((m_nFlatnessMeasure_Type >= 0) && (m_nFlatnessMeasure_Type <= 4))
                    {
                        m_nHeightValue_OK_Count = 0;                            //  Data OK Count
                        m_dHeightValue_Min_Value = double.MaxValue;             //  Height Value Min
                        m_dHeightValue_Max_Value = double.MinValue;             //  Height Value Max
                        m_dHeightValue_Sum = 0.0;                               //  Height Value Sum
                        m_dHeightValue_Avg = 0.0;                               //  Height Value Avg

                        m_nFlatnessMeasure_Step = (int)FlatnessMeasure_Step.Socket_Pause_Check;
                    }
                    else
                    {
                        Log.Write("FlatnessMeasure", Equipment.User_Name, "Flatness Measurement Type 을 선택하지 않음.");

                        m_nFlatnessMeasure_Step = (int)FlatnessMeasure_Step.None;
                        MessageBox.Show("Flatness Measurement Type 을 선택하지 않음.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;

                case (int)FlatnessMeasure_Step.Socket_Pause_Check:
                    m_nFlatnessMeasure_Step = (int)FlatnessMeasure_Step.FlatnessMeasure_StageZ_MovetoLaserHeightSensorPos;
                    break;

                case (int)FlatnessMeasure_Step.FlatnessMeasure_StageZ_MovetoLaserHeightSensorPos:

                    if (StageZ_Move_ProcessPos() != 0)
                    {
                        if (TickCount_Elapsed((int)TickType.TICK_FLATNESS_MEASURE) > nLaserPowermeasureTimeout)
                        {
                            Log.Write("FlatnessMeasure", "Stage Z Move Process Position Fail.");
                            return workStage.AlarmPost(WorkStage.AlarmKey.eZAxisFail); // Stage Z 이동 실패
                        }
                    }
                    else
                    {
                        TickCount_Start((int)TickType.TICK_FLATNESS_MEASURE);
                        m_nFlatnessMeasure_Step = (int)FlatnessMeasure_Step.FlatnessMeasure_StageZ_MovetoLaserHeightSensorPos_DoneCheck;
                    }
                    break;

                case (int)FlatnessMeasure_Step.FlatnessMeasure_StageZ_MovetoLaserHeightSensorPos_DoneCheck:               //  Stage Z, Laser Height Sensor 를 측정 위치로 이동 완료 확인

                    if (StageZ_Move_ProcessPos_Check() != 0)
                    {
                        if (TickCount_Elapsed((int)TickType.TICK_FLATNESS_MEASURE) > nLaserPowermeasureTimeout)
                        {
                            Log.Write("FlatnessMeasure", "Stage Z Move Process Position Check Fail.");
                            return workStage.AlarmPost(WorkStage.AlarmKey.eZAxisFail); // Stage Z 이동 확인 실패
                        }
                    }
                    else
                    {
                        TickCount_Start((int)TickType.TICK_FLATNESS_MEASURE);
                        m_nFlatnessMeasure_Step = (int)FlatnessMeasure_Step.FlatnessMeasure_RemainedCheck;
                    }
                    break;


                case (int)FlatnessMeasure_Step.FlatnessMeasure_RemainedCheck: //  남아있는 얼라인 위치가 있는지 확인 (4개의 얼라인 위치를 모두 확인)

                    if (m_nFlatnessMeasure_Count < 9) //  최대 9 포인트로 고정되어 있음.
                    {
                        if (Equipment.Machine_HeightMeasure_Enable)
                        {
                            Log.Write("FlatnessMeasure", Equipment.User_Name, "Machine_HeightMeasure_Enable. 측정 위치 이동.");
                            m_nFlatnessMeasure_Step = (int)FlatnessMeasure_Step.FlatnessMeasure_StageXY_MovetoFlatnessMeasurePos;
                        }
                        else
                        {
                            //  측정 위치값이 있는지 체크
                            if ((Equipment.stFlatMeasurePos[m_nFlatnessMeasure_Type].StagePos[m_nFlatnessMeasure_Count].X != 0.0) &&
                                (Equipment.stFlatMeasurePos[m_nFlatnessMeasure_Type].StagePos[m_nFlatnessMeasure_Count].Y != 0.0))
                            {
                                Log.Write("FlatnessMeasure", Equipment.User_Name, "최대 측정 회수 이내. 측정 위치값 있음.");

                                //  측정 위치로 이동
                                m_nFlatnessMeasure_Step = (int)FlatnessMeasure_Step.FlatnessMeasure_StageXY_MovetoFlatnessMeasurePos;
                            }
                            else
                            {
                                Log.Write("FlatnessMeasure", Equipment.User_Name, "최대 측정 회수 이내. 측정 위치값 없음. 다음 Position 체크");

                                //  측정 위치가 없으면 다음 포인트로 이동
                                m_nFlatnessMeasure_Count++;
                                m_nFlatnessMeasure_Step = (int)FlatnessMeasure_Step.FlatnessMeasure_RemainedCheck;
                            }
                        }
                    }
                    else
                    {
                        Log.Write("FlatnessMeasure", Equipment.User_Name, "최대 측정 회수 초과. 결과 계산.");
                        m_nFlatnessMeasure_Step = (int)FlatnessMeasure_Step.Complete;
                    }
                    break;


                case (int)FlatnessMeasure_Step.FlatnessMeasure_StageXY_MovetoFlatnessMeasurePos:                 //  Stage XY, Laser Height Sensor 를 측정 위치로 이동

                    double dPosX = 0.0;
                    double dPosY = 0.0;

                    if(Equipment.Machine_HeightMeasure_Enable)
                    {
                        dPosX = Equipment.Machine_HeightMeasure_PosX;
                        dPosY = Equipment.Machine_HeightMeasure_PosY;
                    }
                    else
                    {
                        dPosX = Equipment.stFlatMeasurePos[m_nFlatnessMeasure_Type].StagePos[m_nFlatnessMeasure_Count].X;
                        dPosY = Equipment.stFlatMeasurePos[m_nFlatnessMeasure_Type].StagePos[m_nFlatnessMeasure_Count].Y;
                    }

                    if (StageXY_Move_HeightCheckPos(dPosX, dPosY) != 0)
                    {
                        if (TickCount_Elapsed((int)TickType.TICK_FLATNESS_MEASURE) > nLaserPowermeasureTimeout)
                        {
                            Log.Write("FlatnessMeasure", Equipment.User_Name, "Stage XY Move Power Meter Position Fail.");
                            return workStage.AlarmPost(WorkStage.AlarmKey.eStageMoveFail); // Stage XY 이동 실패
                        }
                    }
                    else
                    {
                        TickCount_Start((int)TickType.TICK_FLATNESS_MEASURE);
                        m_nFlatnessMeasure_Step = (int)FlatnessMeasure_Step.FlatnessMeasure_StageXY_MovetoFlatnessMeasurePos_DoneCheck;
                    }
                    break;


                case (int)FlatnessMeasure_Step.FlatnessMeasure_StageXY_MovetoFlatnessMeasurePos_DoneCheck:                            //  Stage XY, Laser Height Sensor 를 측정 위치로 이동 완료 확인

                    int tempStep = m_nFlatnessMeasure_Step;
                    if (workStage.CheckAxesMotionDoneWithRetry(
                        m_xyCoordinate.X,                                                             /// <param name="targetX">X 목표 위치. 사용하지 않으면 null</param>
                        m_xyCoordinate.Y,                                                             /// <param name="targetY">Y 목표 위치. 사용하지 않으면 null</param>
                        null,               // Z 없음                                                           /// <param name="targetZ">Z 목표 위치. 사용하지 않으면 null</param>
                        60000,                                                                                  /// <param name="timeoutMs">타임아웃 (ms)</param>
                        ref m_nStage_RetryCount,                                                                /// <param name="retryCount">ref 재시도 횟수 변수</param>
                        3,                                                                                      /// <param name="maxRetry">최대 재시도 횟수</param>
                        ref tempStep,
                        (int)FlatnessMeasure_Step.FlatnessMeasure_StageXY_MovetoFlatnessMeasurePos))                         /// <param name="jumpBackStep">재시도 시 되돌아갈 Step</param>
                    {
                        TickCount_Start((int)TickType.TICK_FLATNESS_MEASURE);
                        m_strTemp = "Stage XY 축, Laser Height Check 위치로 이동 완료";
                        Log.Write("FlatnessMeasure", Equipment.User_Name, m_strTemp);
                        m_nFlatnessMeasure_Step = (int)FlatnessMeasure_Step.FlatnessMeasure_StableTime;
                    }
                    else
                    {
                        m_nFlatnessMeasure_Step = tempStep;  // 다시 반영
                    }
                    break;


                case (int)FlatnessMeasure_Step.FlatnessMeasure_StableTime:                                     //  값을 읽기 위해 안정화 시간 대기

                    if (Equipment.Machine_LaserHeightCheckStableTime_Enable)
                    {
                        if (TickCount_Elapsed((int)TickType.TICK_FLATNESS_MEASURE) > Equipment.Machine_LaserHeightCheckStableTime)
                        {
                            workStage.m_bSensorRequestPending = true;
                            workStage.m_bSensorResponseReady = false;
                            Thread.Sleep(100);
                            TickCount_Start((int)TickType.TICK_FLATNESS_MEASURE);
                            Log.Write("FlatnessMeasure", Equipment.User_Name, "Stage XY축, Laser Height Check 위치로 이동 후 안정화 시간.");
                            m_nFlatnessMeasure_Step = (int)FlatnessMeasure_Step.FlatnessMeasure_LaserHeightValue_Get;
                        }
                    }
                    else    //  바로 값 가져오기
                    {
                        workStage.m_bSensorRequestPending = true;
                        workStage.m_bSensorResponseReady = false;
                        Thread.Sleep(100);
                        TickCount_Start((int)TickType.TICK_FLATNESS_MEASURE);
                        Log.Write("FlatnessMeasure", Equipment.User_Name, "Stage XY축, Laser Height Check 위치로 이동 후 안정화 시간 없이 바로 데이터 Get.");
                        m_nFlatnessMeasure_Step = (int)FlatnessMeasure_Step.FlatnessMeasure_LaserHeightValue_Get;
                    }
                    break;


                case (int)FlatnessMeasure_Step.FlatnessMeasure_LaserHeightValue_Get:                                                    //  높이값 가져오기

                    // 응답이 아직 안 왔으면 기다림
                    if (!workStage.m_bSensorResponseReady)
                    {
                        if (TickCount_Elapsed((int)TickType.TICK_FLATNESS_MEASURE) > 1000)
                        {
                            Log.Write("FlatnessMeasure", Equipment.User_Name, $"센서 응답 Timeout (1000ms)");
                            //AlarmPost(AlarmKey.eSensor_Height_Timeout);

                            // 강제 fallback 진입: 센서 응답 없는 상태로 시뮬레이션 강제 진행
                            workStage.m_bSensorResponseReady = true;
                        }
                        break;
                    }

                    if (workStage.m_bSensorResponseReady)
                    {
                        Log.Write("FlatnessMeasure", Equipment.User_Name, $"센서 값 받아오기.");
                    }

                    //  Laser 값 저장
                    Equipment.stFlatMeasurePos[m_nFlatnessMeasure_Type].LaserHeightValue[m_nFlatnessMeasure_Count] = 
                    workStage.m_dLaserHeightSensorSocket_Value;

                    if ((workStage.m_dLaserHeightSensorSocket_Value < -4.5) || 
                        (workStage.m_dLaserHeightSensorSocket_Value > 5.5) || 
                        (workStage.m_dLaserHeightSensorSocket_Value < -99.9) ) //  잘못된 값은 사용하지 않는다.
                    {
                        m_strTemp = string.Format("데이터값 NG, Flatness Measurement Type Index ({0}), Position Index ({1}), Laser Height Value ({2:0.000})",
                                                    m_nFlatnessMeasure_Type, m_nFlatnessMeasure_Count, workStage.m_dLaserHeightSensorSocket_Value);

                        Log.Write("FlatnessMeasure", Equipment.User_Name, m_strTemp);
                        m_nFlatnessMeasure_Count++;     //  다음 위치
                        m_nFlatnessMeasure_Step = (int)FlatnessMeasure_Step.FlatnessMeasure_RemainedCheck;
                    }
                    else
                    {
                        m_strTemp = string.Format("데이터값 OK, Flatness Measurement Type Index ({0}), Position Index ({1}), Laser Height Value ({2:0.000})",
                                                    m_nFlatnessMeasure_Type, m_nFlatnessMeasure_Count, workStage.m_dLaserHeightSensorSocket_Value);

                        Log.Write("FlatnessMeasure", Equipment.User_Name, m_strTemp);

                        m_nFlatnessMeasure_Count++;     //  다음위치
                        m_nHeightValue_OK_Count++;                                                                                  //  Data OK Count

                        m_dHeightValue_Min_Value = Math.Min(m_dHeightValue_Min_Value, workStage.m_dLaserHeightSensorSocket_Value);            //  Height Value Min
                        m_dHeightValue_Max_Value = Math.Max(m_dHeightValue_Max_Value, workStage.m_dLaserHeightSensorSocket_Value);            //  Height Value Max
                        m_dHeightValue_Sum += workStage.m_dLaserHeightSensorSocket_Value;                                                     //  Height Value Sum

                        m_nFlatnessMeasure_Step = (int)FlatnessMeasure_Step.FlatnessMeasure_RemainedCheck;
                    }
                    break;

                case (int)FlatnessMeasure_Step.FlatnessMeasure_LaserHeightValue_Calc:

                    if (m_nHeightValue_OK_Count > 0)
                    {
                        m_dHeightValue_Avg = m_dHeightValue_Sum / (double)m_nHeightValue_OK_Count;                     //  Height Value Avg

                        m_nFlatnessMeasure_Step = (int)FlatnessMeasure_Step.Complete;
                    }
                    else
                    {
                        m_nFlatnessMeasure_Step = (int)FlatnessMeasure_Step.Complete;
                    }
                    break;

                case (int)FlatnessMeasure_Step.Complete:

                    Log.Write("FlatnessMeasure", Equipment.User_Name, "완료");
                    m_strTemp = "===  Flatness Measurement 완료  ===";
                    if (m_nHeightValue_OK_Count > 0)
                    {
                        m_strTemp += string.Format("\r\n\r\n   - Flatness Measurement Type Index ({0})\r\n   - 측정 위치 개수 ({1})\r\n   - 정상 측정 개수 ({2})\r\n\r\n",
                                                    m_nFlatnessMeasure_Type, m_nFlatnessMeasure_Count, m_nHeightValue_OK_Count);

                        m_dHeightValue_Avg = m_dHeightValue_Sum / (double)m_nHeightValue_OK_Count;                     //  Height Value Avg

                        //  측정된 높이값 보여주기
                        for (int i = 0; i < 9; i++)
                        {
                            if (Equipment.stFlatMeasurePos[m_nFlatnessMeasure_Type].LaserHeightValue[i] > -10.0)
                            {
                                m_strTemp += string.Format("   - Pos.{0} : {1:0.000}\r\n", i + 1, Equipment.stFlatMeasurePos[m_nFlatnessMeasure_Type].LaserHeightValue[i]);
                            }
                            else                                                                                        //  값이 -4.5 이하로 내려가면 측정 실패
                            {
                                m_strTemp += string.Format("   - Pos.{0} : Failed\r\n", i + 1);
                            }
                        }

                        m_dDeviation = Math.Abs(m_dHeightValue_Max_Value - m_dHeightValue_Min_Value);
                        m_strTemp += string.Format("\r\n   - Min. ({0:0.000})\r\n   - Max. ({1:0.000})\r\n   - Average ({2:0.000}\r\n\r\n   - Deviation ({3:0.000})",
                                                    m_dHeightValue_Min_Value, m_dHeightValue_Max_Value, m_dHeightValue_Avg, m_dDeviation);
                        Log.Write("FlatnessMeasure", Equipment.User_Name, m_strTemp);
                    }
                    else
                    {
                        m_strTemp += string.Format("\r\n\n   - 측정된 위치값이 없음.");
                        Log.Write("FlatnessMeasure", Equipment.User_Name, m_strTemp);
                    }

                    m_nFlatnessMeasure_Step = (int)FlatnessMeasure_Step.None;

                    if (!Equipment.AutoRunStatus && !Equipment.SelectRunEnable_New && !Equipment.SelectRunEnable)
                    {
                        MessageBox.Show(m_strTemp, "Information!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;
            }


            return nRtn;
        }

        #endregion


        private int StageXY_Move_HeightCheckPos_Check()
        {
            int nRtn = 0;
            if (workStage.IsWorkStage_Positions(WorkStage.nAxis.X, m_xyCoordinate.X) &&
                workStage.IsWorkStage_Positions(WorkStage.nAxis.Y, m_xyCoordinate.Y)  )
            {
                //  XY 축이 지정된 위치로 이동 완료
                nRtn = 0;
            }
            else
            {
                string strTemp = string.Format("Stage XY Move Center Position 실패");
                Log.Write("FlatnessMeasure", "StageXY_Move_HeightCheckPos_Check", strTemp);
                nRtn = -1;
                return workStage.AlarmPost(AlarmKey.eStageMoveFail);
            }

            return nRtn;
        }

        private int StageXY_Move_HeightCheckPos(double dPosX, double dPosY)
        {
            int nRtn = 0;
            m_xyCoordinate = new XyCoordinate(dPosX, dPosY);
            workStage.MovetoWorkStage_ABS_PositionsXY(m_xyCoordinate, Type_Motor_Speed.Coarse);
            return nRtn;
        }

        private int StageZ_Move_ProcessPos_Check()
        {
            int nRtn = 0;
            string strTemp = string.Empty;
            int nZPos = 0;
            nZPos = (int)QMC.Common.Modules.Vision.Vision_TeachingPosList.Laser_Sensor_HeightCheckPos;

            if (workStage.IsWorkStage_TeachingPositionsZ((int)nZPos))
            {
                nRtn = 0;
            }
            else
            {
                strTemp = string.Format("Stage Z 축, Laser_Sensor_HeightCheckPos 높이로 이동 실패");
                Log.Write("FlatnessMeasure", Equipment.User_Name, strTemp);

                nRtn = -1;
                return workStage.AlarmPost(WorkStage.AlarmKey.eZAxisFail);
            }

            return nRtn;
        }
        private int StageZ_Move_ProcessPos()
        {
            int nRtn = 0;
            int nZPos = 0;

            nZPos = (int)QMC.Common.Modules.Vision.Vision_TeachingPosList.Laser_Sensor_HeightCheckPos;
            workStage.MovetoWorkStage_TeachingPositionsZ(nZPos, Type_Motor_Speed.Fine);

            return nRtn;
        }


    }
}
