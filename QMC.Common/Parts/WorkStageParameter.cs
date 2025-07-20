

using QMC.Common.Modules;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QMC.Common.Parts
{
    public class WorkStageParameter : MotionPart
    {
        #region Enum List

        #endregion

        #region => Axis Define 


#if SLD_200C
//#if false                                                               //  SLD-200U
        public enum AxisAjinEnum                                                       //  SLD-200C 에서 사용하는 축 번호    
        {
            //  축 번호 변경 전 (X:0,     Y:1,    Z:2,    MASK_Y:3)
            //  축 번호 변경 후 (X:3,     Y:4,    Z:5,    MASK_Y:0)

            X = 3,
            Y = 4,
            Z = 5,
            MASK_Y = 0,
        }
#else
        public enum AxisAjinEnum                                                       //  SLD-200U 에서 사용하는 축 번호   
        {
            //  축 번호 변경 전 (X:0,     Y:1,    Z:2,    MASK_Y:3)
            //  축 번호 변경 후 (X:3,     Y:4,    Z:5,    MASK_Y:0)

            X = 2,
            Y = 3,
            Z = 4,
            MASK_Y = 13, //  UV 에서는 없는 축이지만, CO2 와 프로그램을 통일하기 위해서 남겨둠. 실제로 사용하지는 않음.
        }
#endif


        #endregion

        public enum MotionKey
        {
            X,
            Y,
            Z,
            MASK_Y,

            Max,
        }

        public enum WorkTable
        {
            WorkTable_1,
            WorkTable_2
        }

        public enum LED_Light
        {
            Red = 0,
            Green,
            Blue,
        }

        public struct stWorkStageParam
        {
            public int[] nAxis;
            public double[] dTarget;
            public double[] dVel;
            public double[] dAcc;
            public double[] dDec;

            public double[] dOffset;
        }

        public enum DioPointKey
        {
            /// <summary>
            /// Input
            /// </summary>
            /// 
            //  OP S/W                          --> OperationButton 에서 처리
            //Input_OpSwitch_Start,               //  X000
            //Input_OpSwitch_Stop,                //  X001
            //Input_OpSwitch_Reset,               //  X002
            //Input_OpSwitch_EMG,                 //  X003

            //  CDA Panel
            Input_Main_CDA_Check,               //  X004
            Input_Main_Purge_Check,             //  X005
            Input_Ejector_1_Check,              //  X006
            Input_Ejector_2_Check,              //  X007
            Input_Ejector_3_Check,              //  X008
            Input_Ejector_4_Check,              //  X009
            Input_Ejector_5_Check,              //  X010

            //  Water Sol. Box
            Input_Scanner_Flow_Check,           //  X011
            Input_VarioScan_Flow_Check,         //  X012

            //  TOP Frame
            Input_Front_Door_Check,             //  X013
            Input_Front_SwingDoor_Check,        //  X014
            Input_Left_Door_Check,              //  X015
            Input_Rear_Door_Check,              //  X016    
            Input_Right_Door_Check,             //  X017

            //  BOTTOM Frame
            Input_Water_In_Leak_Check,          //  X018

            //  TOP Frame
            Input_Chiller_Run,                  //  X019                //  Input_Water_Box_Leak_Check --> Chiller Run 으로 변경

            //  LASER Base
            Input_Chiller_Alarm,                //  X020                //  Input_Laser_Leak_Check --> Chiller Alarm 으로 변경

            //  BDS
            Input_Mask_Leak_Check,              //  X021
            Input_BDS_PowerMeter_FW,            //  X022
            Input_BDS_PowerMeter_BW,            //  X023

            //  STAGE
            Input_Stage_Vacuum_Check,           //  X024
            Input_Laser_CalSheet_Vacuum_Check,  //  X025

            //  DUST Collector
            Input_DustCollector0_Fan_Run,       //  X026
            Input_DustCollector0_Fan_Fault,     //  X027
            Input_DustCollector1_Fan_Run,       //  X028
            Input_DustCollector1_Fan_Fault,     //  X029

            //  LASER Base
            Input_Laser_System_Fault,           //  X030



            /// <summary>
            /// Output
            /// </summary>
            /// 
            //  OP S/W                          --> OperationButton 에서 처리
            //Output_OpLamp_Start,                //  Y000
            //Output_OpLamp_Stop,                 //  Y001
            //Output_OpLamp_Reset,                //  Y002

            //  TOP Frame                       --> TowerLamp 에서 처리
            //Output_TowerLamp_Red,               //  Y003
            //Output_TowerLamp_Yellow,            //  Y004
            //Output_TowerLamp_Green,             //  Y005
            //Output_TowerLamp_Buzzer,            //  Y006

            //  Water Sol. Box
            Output_BeamDump_Coolant_Supply,     //  Y007

            //Output_BeamDump_Coolant_Return,     //  Y008
            Output_AirCurtainPurge,     //  Y008

            Output_Mask_Coolant_Supply,         //  Y009
            Output_Mask_Coolant_Return,         //  Y010
            Output_Scanner_Coolant_Supply,      //  Y011
            Output_Scanner_Coolant_Return,      //  Y012
            Output_VarioScan_Coolant_Supply,    //  Y013
            Output_VarioScan_Coolant_Return,    //  Y014

            //  Sol. Block
            Output_BDS_PowerMeter_FW,           //  Y015
            Output_BDS_PowerMeter_BW,           //  Y016
            Output_BDS_Purge,                   //  Y017
            Output_Laser_Purge,                 //  Y018
            Output_Scanner_Purge,               //  Y019
            Output_VarioScan_Purge,             //  Y020

            //  STAGE
            Output_Stage_Vacuum,                //  Y021
            Output_Laser_CalSheet_Vacuum,       //  Y022
            Output_Stage_Blow,                  //  Y023
            Output_Laser_CalSheet_Blow,         //  Y024

            //  LASER
            Output_Chiller_Run,                 //  Y025            //  Laser Shutter Command (예비용) 를 Chiller Run 으로 사용함.

            //  Dust Collector
            Output_DustCollector0_AirPulse_Run, //  Y026
            Output_DustCollector1_AirPulse_Run, //  Y027

            //  LASER
            Output_Laser_Enable,                //  Y028
        }


        public WorkStageParameterConfig Config { get; set; }
        public WorkStageParameterRecipe Recipe { get; set; }

        public stWorkStageParam stWorkStagePosParam;
        public stWorkStageParam stWorkStagePosParam2;
        public stWorkStageParam stWorkStagePosParam_Verify;             //  위치 좌표 무결성 검사
        public stWorkStageParam stStageLoadPosParam;
        public stWorkStageParam stStageCenterPosParam;
        public stWorkStageParam stHighVisionCenterPosParam;
        public stWorkStageParam stLowVisionCenterPosParam;


        public WorkStageParameter(string strName) : base(strName)
        {
            Config = new WorkStageParameterConfig();
            Recipe = new WorkStageParameterRecipe();
        }

        #region Part
        public override int Create()
        {
            int ret = base.Create();
            int count = 0;

            if (m_dicAxes == null)
                m_dicAxes = new Dictionary<string, MotionAxis>();

            m_dicAxes.Clear();
            foreach (MotionKey key in Enum.GetValues(typeof(MotionKey)))
            {
                m_dicAxes.Add(key.ToString(), null);
            }

            m_dicAxisDisplayType.Clear();

            m_dicAxisDisplayType.Add(MotionKey.X.ToString(), DisplayAxisType.CombinationHorizontal);
            m_dicAxisDisplayType.Add(MotionKey.Y.ToString(), DisplayAxisType.CombinationVertical);
            m_dicAxisDisplayType.Add(MotionKey.Z.ToString(), DisplayAxisType.Vertical);
            m_dicAxisDisplayType.Add(MotionKey.MASK_Y.ToString(), DisplayAxisType.Vertical2);

            //  IO 선언
            if (m_dicDioPoints == null)
                m_dicDioPoints = new Dictionary<string, DioPoint>();

            m_dicDioPoints.Clear();
            foreach (DioPointKey key in Enum.GetValues(typeof(DioPointKey)))
            {
                m_dicDioPoints.Add(key.ToString(), null);
            }

            return ret;
        }

        public override void Stop()
        {
            base.Stop();
            foreach (string key in m_dicAxes.Keys)
            {
                MotionAxis axis = m_dicAxes[key];
                if (axis != null)
                    axis.Stop();
            }
        }
        #endregion

        protected override int OnBeforeMove(Dictionary<string, MovingProjection> dicMovingProjection)
        {
            int ret = 0;
            //Interlock

            return ret;
        }
        protected MovingProjection GetDefaultMovingProjections(string strKey)
        {
            MovingProjection movingProjection = null;
            MotionAxis axis = m_dicAxes[strKey];
            if (axis != null)
            {
                movingProjection = axis.GetDefaultMovingProjection();
            }

            return movingProjection;
        }
        protected Dictionary<string, MovingProjection> GetDefaultMovingProjections()
        {
            Dictionary<string, MovingProjection> dicMovingProjection = new Dictionary<string, MovingProjection>();

            {
                string strKey = MotionKey.X.ToString();
                MovingProjection movingProjection = GetDefaultMovingProjection(strKey);
                if (movingProjection != null)
                {
                    dicMovingProjection.Add(strKey, movingProjection);
                }
            }
            {
                string strKey = MotionKey.Y.ToString();
                MovingProjection movingProjection = GetDefaultMovingProjection(strKey);
                if (movingProjection != null)
                {
                    dicMovingProjection.Add(strKey, movingProjection);
                }
            }
            {
                string strKey = MotionKey.Z.ToString();
                MovingProjection movingProjection = GetDefaultMovingProjection(strKey);
                if (movingProjection != null)
                {
                    dicMovingProjection.Add(strKey, movingProjection);
                }
            }
            {
                string strKey = MotionKey.MASK_Y.ToString();
                MovingProjection movingProjection = GetDefaultMovingProjection(strKey);
                if (movingProjection != null)
                {
                    dicMovingProjection.Add(strKey, movingProjection);
                }
            }

            return dicMovingProjection;
        }

        //protected Dictionary<string, MovingProjection> GetDefaultUVWMovingProjections()
        //{
        //    Dictionary<string, MovingProjection> dicMovingProjection = new Dictionary<string, MovingProjection>();

        //    {
        //        string strKey = MotionKey.U.ToString();
        //        MovingProjection movingProjection = GetDefaultMovingProjection(strKey);
        //        if (movingProjection != null)
        //        {
        //            dicMovingProjection.Add(strKey, movingProjection);
        //        }
        //    }
        //    {
        //        string strKey = MotionKey.V.ToString();
        //        MovingProjection movingProjection = GetDefaultMovingProjection(strKey);
        //        if (movingProjection != null)
        //        {
        //            dicMovingProjection.Add(strKey, movingProjection);
        //        }
        //    }
        //    {
        //        string strKey = MotionKey.W.ToString();
        //        MovingProjection movingProjection = GetDefaultMovingProjection(strKey);
        //        if (movingProjection != null)
        //        {
        //            dicMovingProjection.Add(strKey, movingProjection);
        //        }
        //    }

        //    return dicMovingProjection;
        //}

        //protected Dictionary<string, MovingProjection> GetDefaultXYMovingProjections()
        //{
        //    Dictionary<string, MovingProjection> dicMovingProjection = new Dictionary<string, MovingProjection>();            
        //    {
        //        string strKey = MotionKey.X.ToString();
        //        MovingProjection movingProjection = GetDefaultMovingProjection(strKey);
        //        if (movingProjection != null)
        //        {
        //            dicMovingProjection.Add(strKey, movingProjection);
        //        }
        //    }
        //    {
        //        string strKey = MotionKey.Y.ToString();
        //        MovingProjection movingProjection = GetDefaultMovingProjection(strKey);
        //        if (movingProjection != null)
        //        {
        //            dicMovingProjection.Add(strKey, movingProjection);
        //        }
        //    }

        //    return dicMovingProjection;
        //}

        //protected Dictionary<string, MovingProjection> GetDefaultZMovingProjections()
        //{
        //    Dictionary<string, MovingProjection> dicMovingProjection = new Dictionary<string, MovingProjection>();
        //    {
        //        string strKey = MotionKey.Z.ToString();
        //        MovingProjection movingProjection = GetDefaultMovingProjection(strKey);
        //        if (movingProjection != null)
        //        {
        //            dicMovingProjection.Add(strKey, movingProjection);
        //        }
        //    }
            
        //    return dicMovingProjection;
        //}

        //protected Dictionary<string, MovingProjection> GetDefaultElevatorZMovingProjections()
        //{
        //    Dictionary<string, MovingProjection> dicMovingProjection = new Dictionary<string, MovingProjection>();
        //    {
        //        string strKey = MotionKey.EZ.ToString();
        //        MovingProjection movingProjection = GetDefaultMovingProjection(strKey);
        //        if (movingProjection != null)
        //        {
        //            dicMovingProjection.Add(strKey, movingProjection);
        //        }
        //    }

        //    return dicMovingProjection;
        //}

        //protected Dictionary<string, MovingProjection> GetDefaultVisionZMovingProjections()
        //{
        //    Dictionary<string, MovingProjection> dicMovingProjection = new Dictionary<string, MovingProjection>();
        //    {
        //        string strKey = MotionKey.VZ.ToString();
        //        MovingProjection movingProjection = GetDefaultMovingProjection(strKey);
        //        if (movingProjection != null)
        //        {
        //            dicMovingProjection.Add(strKey, movingProjection);
        //        }
        //    }

        //    return dicMovingProjection;
        //}

        //public int UvwMovePosition(UvwCoordinate coordinate)
        //{
        //    Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultMovingProjections();

        //    dicMovingProjection[MotionKey.U.ToString()].Position = coordinate.U;
        //    dicMovingProjection[MotionKey.V.ToString()].Position = coordinate.V;
        //    dicMovingProjection[MotionKey.W.ToString()].Position = coordinate.W;

        //    return Move(dicMovingProjection);
        //}

        //public int MovePosition(XyCoordinate coordinate)
        //{
        //    Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultMovingProjections();

        //    dicMovingProjection[MotionKey.X.ToString()].Position = coordinate.X;
        //    dicMovingProjection[MotionKey.Y.ToString()].Position = coordinate.Y;
            
        //    return Move(dicMovingProjection);
        //}

        //public Task<int> UvwBeginMovePosition(UvwCoordinate coordinate)
        //{
        //    Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultMovingProjections();

        //    dicMovingProjection[MotionKey.U.ToString()].Position = coordinate.U;
        //    dicMovingProjection[MotionKey.V.ToString()].Position = coordinate.V;
        //    dicMovingProjection[MotionKey.W.ToString()].Position = coordinate.W;

        //    return BeginMove(dicMovingProjection);
        //}

        //public Task<int> BeginMovePosition(XyCoordinate coordinate)
        //{
        //    Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultMovingProjections();

        //    dicMovingProjection[MotionKey.X.ToString()].Position = coordinate.X;
        //    dicMovingProjection[MotionKey.Y.ToString()].Position = coordinate.Y;

        //    return BeginMove(dicMovingProjection);
        //}

        //public int MoveZPosition(double dPosition)
        //{
        //    Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultZMovingProjections();

        //    dicMovingProjection[MotionKey.Z.ToString()].Position = dPosition;

        //    return Move(dicMovingProjection);
        //}

        //public int MoveElevatorZPosition(double dPosition)
        //{
        //    Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultElevatorZMovingProjections();

        //    dicMovingProjection[MotionKey.EZ.ToString()].Position = dPosition;

        //    return Move(dicMovingProjection);
        //}

        //public int MoveVisionZPosition(double dPosition)
        //{
        //    Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultVisionZMovingProjections();

        //    dicMovingProjection[MotionKey.VZ.ToString()].Position = dPosition;

        //    return Move(dicMovingProjection);
        //}

        //public Task<int> BeginMoveElevatorZPosition(double dPosition)
        //{
        //    Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultElevatorZMovingProjections();

        //    dicMovingProjection[MotionKey.EZ.ToString()].Position = dPosition;
            
        //    return BeginMove(dicMovingProjection);
        //}

        //public Task<int> BeginMoveVisionZPosition(double dPosition)
        //{
        //    Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultVisionZMovingProjections();

        //    dicMovingProjection[MotionKey.VZ.ToString()].Position = dPosition;

        //    return BeginMove(dicMovingProjection);
        //}

        //public double GetCurrentEZPosition()
        //{
        //    double dPosition = 0;
        //    MotionAxis axis = m_dicAxes[MotionKey.EZ.ToString()];
        //    if(axis != null)
        //    {
        //        dPosition = axis.Motor.ActualPosition;
        //    }
        //    return dPosition;
        //}

        //public double GetCurrentVZPosition()
        //{
        //    double dPosition = 0;
        //    MotionAxis axis = m_dicAxes[MotionKey.VZ.ToString()];
        //    if (axis != null)
        //    {
        //        dPosition = axis.Motor.ActualPosition;
        //    }
        //    return dPosition;
        //}

        //public double GetCurrentUPosition()
        //{
        //    double dPosition = 0;
        //    MotionAxis axis = m_dicAxes[MotionKey.U.ToString()];
        //    if (axis != null)
        //    {
        //        dPosition = axis.Motor.ActualPosition;
        //    }
        //    return dPosition;
        //}

        //public double GetCurrentVPosition()
        //{
        //    double dPosition = 0;
        //    MotionAxis axis = m_dicAxes[MotionKey.V.ToString()];
        //    if (axis != null)
        //    {
        //        dPosition = axis.Motor.ActualPosition;
        //    }
        //    return dPosition;
        //}

        //public double GetCurrentWPosition()
        //{
        //    double dPosition = 0;
        //    MotionAxis axis = m_dicAxes[MotionKey.W.ToString()];
        //    if (axis != null)
        //    {
        //        dPosition = axis.Motor.ActualPosition;
        //    }
        //    return dPosition;
        //}

        //public double GetCurrentXPosition()
        //{
        //    double dPosition = 0;
        //    MotionAxis axis = m_dicAxes[MotionKey.X.ToString()];
        //    if (axis != null)
        //    {
        //        dPosition = axis.Motor.ActualPosition;
        //    }
        //    return dPosition;
        //}

        //public double GetCurrentYPosition()
        //{
        //    double dPosition = 0;
        //    MotionAxis axis = m_dicAxes[MotionKey.Y.ToString()];
        //    if (axis != null)
        //    {
        //        dPosition = axis.Motor.ActualPosition;
        //    }
        //    return dPosition;
        //}

        public stWorkStageParam GetPositionInformation(string m_strPosName)
        {
            stWorkStageParam m_stLaserPosParam = new stWorkStageParam();

            int count = System.Enum.GetValues(typeof(MotionKey)).Length;
            m_stLaserPosParam.nAxis = new int[count];
            m_stLaserPosParam.dTarget = new double[count];
            m_stLaserPosParam.dVel = new double[count];
            m_stLaserPosParam.dAcc = new double[count];
            m_stLaserPosParam.dDec = new double[count];
            m_stLaserPosParam.dOffset = new double[count];

            int ret = 0;
            int nPosCnt = 0;
            double[] m_dPos;
            m_dPos = new double[count];

            SettingParameterCollection parameters = new SettingParameterCollection();
            parameters = Config.GetData();

            //  Target Position 
            for (int i = 0; i < parameters.Count; i++)
            {
                if (parameters[i].Name == m_strPosName)
                {
#if true        //  요렇게 하던지.... 아니면...

                    //m_stLaserPosParam.dTarget[nPosCnt++] = m_dPos[nPosCnt] = Equipment.ToDouble(parameters[i++].Value);       //  X
                    //m_stLaserPosParam.dTarget[nPosCnt++] = m_dPos[nPosCnt] = Equipment.ToDouble(parameters[i++].Value);       //  Y
                    //m_stLaserPosParam.dTarget[nPosCnt++] = m_dPos[nPosCnt] = Equipment.ToDouble(parameters[i++].Value);       //  T
                    //m_stLaserPosParam.dTarget[nPosCnt] = m_dPos[nPosCnt] = Equipment.ToDouble(parameters[i++].Value);         //  Z

                    ////  Offset 값도 가져오자.
                    //nPosCnt = 0;
                    //m_stLaserPosParam.dOffset[nPosCnt++] = Equipment.ToDouble(parameters[i++].Value);                         //  X
                    //m_stLaserPosParam.dOffset[nPosCnt++] = Equipment.ToDouble(parameters[i++].Value);                         //  Y
                    //m_stLaserPosParam.dOffset[nPosCnt++] = Equipment.ToDouble(parameters[i++].Value);                         //  T
                    //m_stLaserPosParam.dOffset[nPosCnt] = Equipment.ToDouble(parameters[i++].Value);                           //  Z


                    //  다른 좋은 방법이 있겠지만 시간이 없으니 일단 이렇게 슥삭...
                    m_stLaserPosParam.dTarget[0] = m_dPos[0] = Equipment.ToDouble(parameters[i++].Value);       //  X
                    m_stLaserPosParam.dTarget[1] = m_dPos[1] = Equipment.ToDouble(parameters[i++].Value);       //  Y
                    m_stLaserPosParam.dTarget[2] = m_dPos[2] = Equipment.ToDouble(parameters[i++].Value);       //  Z
                    m_stLaserPosParam.dTarget[3] = m_dPos[3] = Equipment.ToDouble(parameters[i++].Value);       //  MASK_Y

                    //  Offset 값도 가져오자.
                    nPosCnt = 0;
                    m_stLaserPosParam.dOffset[0] = Equipment.ToDouble(parameters[i++].Value);                   //  X
                    m_stLaserPosParam.dOffset[1] = Equipment.ToDouble(parameters[i++].Value);                   //  Y
                    m_stLaserPosParam.dOffset[2] = Equipment.ToDouble(parameters[i++].Value);                   //  Z
                    m_stLaserPosParam.dOffset[3] = Equipment.ToDouble(parameters[i++].Value);                   //  MASK_Y


                    break;

#else           //  요렇게 하던지...

                    if (parameters[i].Tag == "X")
                    {
                        m_stDispParam.dTarget[0] = m_dPos[0] = Equipment.ToDouble(parameters[i].Value);      //  X
                        nPosCnt++;
                    }

                    if (parameters[i].Tag == "Y")
                    {
                        m_stDispParam.dTarget[1] = m_dPos[1] = Equipment.ToDouble(parameters[i].Value);      //  Y
                        nPosCnt++;
                    }

                    if (parameters[i].Tag == "T")
                    {
                        m_stDispParam.dTarget[2] = m_dPos[2] = Equipment.ToDouble(parameters[i].Value);      //  T
                        nPosCnt++;
                    }

                    if (parameters[i].Tag == "Z")
                    {
                        m_stDispParam.dTarget[3] = m_dPos[3] = Equipment.ToDouble(parameters[i].Value);      //  Z
                        nPosCnt++;
                    }

                    if (nPosCnt >= 4)
                    {
                        //  Offset 값도 가져오자.
                        if (parameters[i + 1].Tag == "X")
                        {
                            m_stDispParam.dOffset[0] = Equipment.ToDouble(parameters[i + 1].Value);      //  X
                        }

                        if (parameters[i + 2].Tag == "Y")
                        {
                            m_stDispParam.dOffset[1] = Equipment.ToDouble(parameters[i + 2].Value);      //  Y
                        }

                        if (parameters[i + 3].Tag == "T")
                        {
                            m_stDispParam.dOffset[2] = Equipment.ToDouble(parameters[i + 3].Value);      //  T
                        }

                        if (parameters[i + 4].Tag == "Z")
                        {
                            m_stDispParam.dOffset[3] = Equipment.ToDouble(parameters[i + 4].Value);      //  Z
                        }

                        break;
                    }
#endif
                }
            }

            count = 0;

            Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultMovingProjections();
            MovingProjection movingProjection = null;
            foreach (string axisKey in dicMovingProjection.Keys)
            {
                if (Axes.ContainsKey(axisKey))
                {
                    movingProjection = dicMovingProjection[axisKey];
                    MotionAxis axis = Axes[axisKey];
                    if (axis != null)
                    {
                        m_stLaserPosParam.nAxis[count] = axis.No;                  //  축 번호
                        m_stLaserPosParam.dVel[count] = movingProjection.Velocity;     //  속도
                        m_stLaserPosParam.dAcc[count] = movingProjection.Acceleration; //  가속도
                        m_stLaserPosParam.dDec[count] = movingProjection.Deceleration; //  감속도

                        //Limit 확인.
                        if (!axis.Motor.CheckLimit(m_dPos[count++]))
                        {
                            //ret = -1;
                            //break;
                        }
                    }
                }
            }

            return m_stLaserPosParam;
        }



        #region DI Functions (Laser Drilling)

        //public bool DI_OpSwitch_Start()
        //{
        //    bool bRet = false;

        //    DioPoint dioString = null;

        //    //  해당 채널 상태 리턴
        //    dioString = m_dicDioPoints[DioPointKey.Input_OpSwitch_Start.ToString()];

        //    if (dioString == null)
        //        return bRet;

        //    DioValue ioValue = dioString.GetValue();

        //    if (ioValue == DioValue.On)
        //        bRet = true;

        //    return bRet;
        //}

        //public bool DI_OpSwitch_Stop()
        //{
        //    bool bRet = false;

        //    DioPoint dioString = null;

        //    //  해당 채널 상태 리턴
        //    dioString = m_dicDioPoints[DioPointKey.Input_OpSwitch_Stop.ToString()];

        //    if (dioString == null)
        //        return bRet;

        //    DioValue ioValue = dioString.GetValue();

        //    if (ioValue == DioValue.On)
        //        bRet = true;

        //    return bRet;
        //}

        //public bool DI_OpSwitch_Reset()
        //{
        //    bool bRet = false;

        //    DioPoint dioString = null;

        //    //  해당 채널 상태 리턴
        //    dioString = m_dicDioPoints[DioPointKey.Input_OpSwitch_Reset.ToString()];

        //    if (dioString == null)
        //        return bRet;

        //    DioValue ioValue = dioString.GetValue();

        //    if (ioValue == DioValue.On)
        //        bRet = true;

        //    return bRet;
        //}

        //public bool DI_OpSwitch_EMG()
        //{
        //    bool bRet = false;

        //    DioPoint dioString = null;

        //    //  해당 채널 상태 리턴
        //    dioString = m_dicDioPoints[DioPointKey.Input_OpSwitch_EMG.ToString()];

        //    if (dioString == null)
        //        return bRet;

        //    DioValue ioValue = dioString.GetValue();

        //    if (ioValue == DioValue.On)
        //        bRet = true;

        //    return bRet;
        //}

        public bool DI_Main_CDA_Check()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Input_Main_CDA_Check.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_Main_Purge_Check()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Input_Main_Purge_Check.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_Ejector_Check(int m_nEjector)
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            switch( m_nEjector)
            {
                case 0: dioString = m_dicDioPoints[DioPointKey.Input_Ejector_1_Check.ToString()]; break;
                case 1: dioString = m_dicDioPoints[DioPointKey.Input_Ejector_2_Check.ToString()]; break;
                case 2: dioString = m_dicDioPoints[DioPointKey.Input_Ejector_3_Check.ToString()]; break;
                case 3: dioString = m_dicDioPoints[DioPointKey.Input_Ejector_4_Check.ToString()]; break;
                case 4: dioString = m_dicDioPoints[DioPointKey.Input_Ejector_5_Check.ToString()]; break;
            }            

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_Scanner_Flow_Check()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Input_Scanner_Flow_Check.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_VarioScan_Flow_Check()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Input_VarioScan_Flow_Check.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_Front_Door_Check()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Input_Front_Door_Check.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_Front_SwiingDoor_Check()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Input_Front_SwingDoor_Check.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_Left_Door_Check()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Input_Left_Door_Check.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_Rear_Door_Check()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Input_Rear_Door_Check.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_Right_Door_Check()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Input_Right_Door_Check.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_Water_In_Leak_Check()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Input_Water_In_Leak_Check.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_Chiller_Run()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Input_Chiller_Run.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_Chiller_Alarm_Check()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Input_Chiller_Alarm.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_Mask_Leak_Check()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Input_Mask_Leak_Check.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_BDS_PowerMeter_FW_Check()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Input_BDS_PowerMeter_FW.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_BDS_PowerMeter_BW_Check()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Input_BDS_PowerMeter_BW.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_Stage_Vacuum_Check()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Input_Stage_Vacuum_Check.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_Laser_CalSheet_Vacuum_Check()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Input_Laser_CalSheet_Vacuum_Check.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_DustCollector_Fan_Run(int m_nDustCollector)
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            switch( m_nDustCollector)
            {
                case 0: dioString = m_dicDioPoints[DioPointKey.Input_DustCollector0_Fan_Run.ToString()]; break;
                case 1: dioString = m_dicDioPoints[DioPointKey.Input_DustCollector1_Fan_Run.ToString()]; break;
            }
            
            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_DustCollector_Fan_Fault(int m_nDustCollector)
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            switch (m_nDustCollector)
            {
                case 0: dioString = m_dicDioPoints[DioPointKey.Input_DustCollector0_Fan_Fault.ToString()]; break;
                case 1: dioString = m_dicDioPoints[DioPointKey.Input_DustCollector1_Fan_Fault.ToString()]; break;
            }

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_Laser_System_Fault()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Input_Laser_System_Fault.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }
        #endregion


        #region DO Functions (Laser Drilling)

        //public int DO_OpLamp_Start(bool m_bOnOff)
        //{
        //    int nRet = 0;

        //    DioPoint dioString = null;

        //    //  해당 채널 출력 성공 여부 리턴
        //    dioString = m_dicDioPoints[DioPointKey.Output_OpLamp_Start.ToString()];

        //    if (dioString == null) return -1;

        //    if (m_bOnOff) nRet = dioString.Write(DioValue.On);
        //    else nRet = dioString.Write(DioValue.Off);

        //    if (nRet != 0) return nRet;

        //    return nRet;
        //}

        //public int DO_OpLamp_Stop(bool m_bOnOff)
        //{
        //    int nRet = 0;

        //    DioPoint dioString = null;

        //    //  해당 채널 출력 성공 여부 리턴
        //    dioString = m_dicDioPoints[DioPointKey.Output_OpLamp_Stop.ToString()];

        //    if (dioString == null) return -1;

        //    if (m_bOnOff) nRet = dioString.Write(DioValue.On);
        //    else nRet = dioString.Write(DioValue.Off);

        //    if (nRet != 0) return nRet;

        //    return nRet;
        //}

        //public int DO_OpLamp_Reset(bool m_bOnOff)
        //{
        //    int nRet = 0;

        //    DioPoint dioString = null;

        //    //  해당 채널 출력 성공 여부 리턴
        //    dioString = m_dicDioPoints[DioPointKey.Output_OpLamp_Reset.ToString()];

        //    if (dioString == null) return -1;

        //    if (m_bOnOff) nRet = dioString.Write(DioValue.On);
        //    else nRet = dioString.Write(DioValue.Off);

        //    if (nRet != 0) return nRet;

        //    return nRet;
        //}

        public int DO_BeamDump_Coolant_Supply(bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_BeamDump_Coolant_Supply.ToString()];

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        public int DO_AirCurtain_Purge(bool m_bOnOff)
        {
            int nRet = 0;
            DioPoint dioString = null;
            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_AirCurtainPurge.ToString()];
            if (dioString == null) return -1;

            //m_bOnOff 바뀌어 있음. :: 여기서 바꿔서 넣자. Is도 해줘야 하네.
            //2025.05.30
            if (m_bOnOff)
                m_bOnOff = false;
            else
                m_bOnOff = true;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        public int DO_Mask_Coolant_Supply(bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Mask_Coolant_Supply.ToString()];

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        public int DO_Mask_Coolant_Return(bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Mask_Coolant_Return.ToString()];

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        public int DO_Scanner_Coolant_Supply(bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Scanner_Coolant_Supply.ToString()];

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        public int DO_Scanner_Coolant_Return(bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Scanner_Coolant_Return.ToString()];

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        public int DO_VarioScan_Coolant_Supply(bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_VarioScan_Coolant_Supply.ToString()];

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        public int DO_VarioScan_Coolant_Return(bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_VarioScan_Coolant_Return.ToString()];

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        public int DO_BDS_PowerMeter_FW(bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_BDS_PowerMeter_FW.ToString()];

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        public int DO_BDS_PowerMeter_BW(bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_BDS_PowerMeter_BW.ToString()];

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        public int DO_BDS_Purge(bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_BDS_Purge.ToString()];

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        public int DO_Laser_Purge(bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Laser_Purge.ToString()];

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        public int DO_Scanner_Purge(bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Scanner_Purge.ToString()];

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        public int DO_VarioScan_Purge(bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_VarioScan_Purge.ToString()];

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        public int DO_Stage_Vacuum(bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Stage_Vacuum.ToString()];

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        public int DO_Laser_CalSheet_Vacuum(bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Laser_CalSheet_Vacuum.ToString()];

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        public int DO_Stage_Blow(bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Stage_Blow.ToString()];

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        public int DO_Laser_CalSheet_Blow(bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Laser_CalSheet_Blow.ToString()];

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        public int DO_Chiller_Run(bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Chiller_Run.ToString()];

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        public int DO_DustCollector_Fan_Run(int m_nPos, bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            switch (m_nPos)
            {
                case 0:
                    dioString = m_dicDioPoints[DioPointKey.Output_DustCollector0_AirPulse_Run.ToString()];
                    break;


                case 1:
                    dioString = m_dicDioPoints[DioPointKey.Output_DustCollector1_AirPulse_Run.ToString()];
                    break;
            }

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        public int DO_Laser_Enable(bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Laser_Enable.ToString()];

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }
        #endregion


        #region DO Status (Laser Drilling)

        //public bool IsDO_OpLamp_Start()
        //{
        //    bool bRet = false;

        //    DioPoint dioString = null;

        //    //  해당 출력 채널 상태 리턴
        //    dioString = m_dicDioPoints[DioPointKey.Output_OpLamp_Start.ToString()];

        //    if (dioString == null)
        //        return bRet;

        //    DioValue ioValue = dioString.GetValue();

        //    if (ioValue == DioValue.On)
        //        bRet = true;

        //    return bRet;
        //}

        //public bool IsDO_OpLamp_Stop()
        //{
        //    bool bRet = false;

        //    DioPoint dioString = null;

        //    //  해당 출력 채널 상태 리턴
        //    dioString = m_dicDioPoints[DioPointKey.Output_OpLamp_Stop.ToString()];

        //    if (dioString == null)
        //        return bRet;

        //    DioValue ioValue = dioString.GetValue();

        //    if (ioValue == DioValue.On)
        //        bRet = true;

        //    return bRet;
        //}

        //public bool IsDO_OpLamp_Reset()
        //{
        //    bool bRet = false;

        //    DioPoint dioString = null;

        //    //  해당 출력 채널 상태 리턴
        //    dioString = m_dicDioPoints[DioPointKey.Output_OpLamp_Reset.ToString()];

        //    if (dioString == null)
        //        return bRet;

        //    DioValue ioValue = dioString.GetValue();

        //    if (ioValue == DioValue.On)
        //        bRet = true;

        //    return bRet;
        //}

        public bool IsDO_BeamDump_Coolant_Supply()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_BeamDump_Coolant_Supply.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool IsDO_AirCurtain_Purge()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_AirCurtainPurge.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            // On/Off 신호가 바뀌어 있음. 여기서 뒤집어서 확인하자. 2025.05.30
            if (ioValue != DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool IsDO_Mask_Coolant_Supply()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Mask_Coolant_Supply.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool IsDO_Mask_Coolant_Return()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Mask_Coolant_Return.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool IsDO_Scanner_Coolant_Supply()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Scanner_Coolant_Supply.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool IsDO_Scanner_Coolant_Return()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Scanner_Coolant_Return.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool IsDO_VarioScan_Coolant_Supply()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_VarioScan_Coolant_Supply.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool IsDO_VarioScan_Coolant_Return()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_VarioScan_Coolant_Return.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool IsDO_BDS_PowerMeter_FW()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_BDS_PowerMeter_FW.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool IsDO_BDS_PowerMeter_BW()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_BDS_PowerMeter_BW.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool IsDO_BDS_Purge()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_BDS_Purge.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool IsDO_Laser_Purge()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Laser_Purge.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool IsDO_Scanner_Purge()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Scanner_Purge.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool IsDO_VarioScan_Purge()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_VarioScan_Purge.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool IsDO_Stage_Vacuum()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Stage_Vacuum.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool IsDO_Laser_CalSheet_Vacuum()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Laser_CalSheet_Vacuum.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool IsDO_Stage_Blow()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Stage_Blow.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool IsDO_Laser_CalSheet_Blow()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Laser_CalSheet_Blow.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool IsDO_Chiller_Run()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Chiller_Run.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool IsDO_DustCollector_AirPulse_Run(int m_nPos)
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            switch (m_nPos)
            {
                case 0:
                    dioString = m_dicDioPoints[DioPointKey.Output_DustCollector0_AirPulse_Run.ToString()];
                    break;


                case 1:
                    dioString = m_dicDioPoints[DioPointKey.Output_DustCollector1_AirPulse_Run.ToString()];
                    break;
            }

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool IsDO_Laser_Enable()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Laser_Enable.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }
        #endregion
    }
}
