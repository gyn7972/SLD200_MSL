using QMC.Common.Modules;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QMC.Common.Parts
{
    public class WaferProbeAlignParameter : MotionPart
    {
        #region Enum List

        #endregion

        #region => Axis Define 

        public enum AxisAjinEnum
        {
            U = 0,
            V,
            W,
            EZ,
            X,
            Y,
            VZ,

            Max,
        }
        //public enum AxisAcsEnum
        //{
        //    //--- ACS ---
        //    StageY = 0,
        //    StageX = 1,

        //    Max,
        //}
        #endregion

        public enum MotionKey
        {
            U,
            V,
            W,
            EZ,
            X,
            Y,
            VZ,
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

        public struct stWaferProbeAlignParam
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
            //  Front OP
            Input_OpSwitch_Start,               //  X000
            Input_OpSwitch_Stop,                //  X001
            Input_OpSwitch_Reset,               //  X002
            Input_OpSwitch_EMG,                 //  X003

            //  Util Panel
            Input_Main_CDACheck,                //  X004
            Input_Main_VacuumCheck,             //  X005

            //  Front OP
            Input_ThinChuck_VacuumCheck,        //  X006
            Input_Wafer_VacuumCheck,            //  X007
            Input_Probe_PackingCheck,           //  X008

            //  Thin Chuck
            Input_ThinChuck_Detect,             //  X016


            /// #1 호기 - 시작
            ///
            //  Top Cover
            Input_TopCover_Up,                  //  X017
            Input_TopCover_Down,                //  X018

            //  Probe card
            Input_Probe_BW_Detect,              //  X019
            ///
            /// #1 호기 - 끝


            /// #2 ~ #6 호기 - 시작
            /// 
            //  Probe-Card Clamp Module
            Input_Probe_LeftClampModule_FW,     //  X017
            Input_Probe_LeftClampModule_BW,     //  X018
            Input_Probe_RightClampModule_FW,    //  X019
            Input_Probe_RightClampModule_BW,    //  X020
            Input_Probe_UnpackingCyl_Down,      //  X021
            Input_Probe_UnpackingCyl_Up,        //  X022
            ///
            /// #2 ~ #6 호기 - 끝



            /// <summary>
            /// Output
            /// </summary>
            /// 
            //  Top Frame
            Output_OpLamp_Start,                //  Y000
            Output_OpLamp_Stop,                 //  Y001
            Output_OpLamp_Reset,                //  Y002

            //  상부 Frame
            //Output_TowerLamp_Red,             //  Tower Lamp 는 "TowerLamp.cs" 에서 처리한다.
            //Output_TowerLamp_Yellow,
            //Output_TowerLamp_Green,
            //Output_Buzzer,

            //  Lamp                            //  요것도 Tower Lamp 에서 하자
            //Output_Lamp0_Red,                   //  Y007
            //Output_Lamp0_Green,                 //  Y008
            //Output_Lamp0_Blue,                  //  Y009
            //Output_Lamp1_Red,                   //  Y010
            //Output_Lamp1_Green,                 //  Y011
            //Output_Lamp1_Blue,                  //  Y012
            //Output_Lamp2_Red,                   //  Y013
            //Output_Lamp2_Green,                 //  Y014
            //Output_Lamp2_Blue,                  //  Y015
            //Output_Lamp3_Red,                   //  Y016
            //Output_Lamp3_Green,                 //  Y017
            //Output_Lamp3_Blue,                  //  Y018

            //  SOL Block
            Output_Probe_Unpacking,             //  Y019
            Output_ThinChuck_StageCleaning,     //  Y020
            Output_ThinChuck_Vacuum,            //  Y021
            Output_Wafer_Vacuum,                //  Y022
            Output_Probe_Packing,               //  Y023


            /// #1 호기 - 시작
            ///
            Output_TopCover_Up,                 //  Y024
            Output_TopCover_Down,               //  Y025
            ///
            /// #1 호기 - 끝


            /// #2 ~ #6 호기 - 시작
            ///
            Output_Probe_ClampModule_Down,      //  Y024
            Output_Probe_ClampModule_FW,        //  Y025
            Output_Probe_ClampModule_BW,        //  Y026
            Output_Probe_UnpackingCyl_Down,     //  Y027
            Output_Probe_UnpackingCyl_Up,       //  Y028
            ///
            /// #2 ~ #6 호기 - 끝
        }


        public WaferProbeAlignParameterConfig Config { get; set; }
        public WaferProbeAlignParameterRecipe Recipe { get; set; }

        public stWaferProbeAlignParam stWaferProbeAlignPosParam;
        public stWaferProbeAlignParam stWaferProbeAlignPosParam2;
        public stWaferProbeAlignParam stStageLoadPosParam;
        public stWaferProbeAlignParam stStageCenterPosParam;
        public stWaferProbeAlignParam stHighVisionCenterPosParam;
        public stWaferProbeAlignParam stLowVisionCenterPosParam;


        public WaferProbeAlignParameter(string strName) : base(strName)
        {
            Config = new WaferProbeAlignParameterConfig();
            Recipe = new WaferProbeAlignParameterRecipe();
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

            m_dicAxisDisplayType.Add(MotionKey.U.ToString(), DisplayAxisType.UVW_Horizontal);
            m_dicAxisDisplayType.Add(MotionKey.V.ToString(), DisplayAxisType.UVW_Vertical);
            m_dicAxisDisplayType.Add(MotionKey.W.ToString(), DisplayAxisType.UVW_Vertical);
            m_dicAxisDisplayType.Add(MotionKey.EZ.ToString(), DisplayAxisType.Vertical);

            m_dicAxisDisplayType.Add(MotionKey.X.ToString(), DisplayAxisType.CombinationHorizontal);
            m_dicAxisDisplayType.Add(MotionKey.Y.ToString(), DisplayAxisType.CombinationVertical);
            m_dicAxisDisplayType.Add(MotionKey.VZ.ToString(), DisplayAxisType.Vertical);

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
                string strKey = MotionKey.U.ToString();
                MovingProjection movingProjection = GetDefaultMovingProjection(strKey);
                if (movingProjection != null)
                {
                    dicMovingProjection.Add(strKey, movingProjection);
                }
            }
            {
                string strKey = MotionKey.V.ToString();
                MovingProjection movingProjection = GetDefaultMovingProjection(strKey);
                if (movingProjection != null)
                {
                    dicMovingProjection.Add(strKey, movingProjection);
                }
            }
            {
                string strKey = MotionKey.W.ToString();
                MovingProjection movingProjection = GetDefaultMovingProjection(strKey);
                if (movingProjection != null)
                {
                    dicMovingProjection.Add(strKey, movingProjection);
                }
            }
            {
                string strKey = MotionKey.EZ.ToString();
                MovingProjection movingProjection = GetDefaultMovingProjection(strKey);
                if (movingProjection != null)
                {
                    dicMovingProjection.Add(strKey, movingProjection);
                }
            }
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
                string strKey = MotionKey.VZ.ToString();
                MovingProjection movingProjection = GetDefaultMovingProjection(strKey);
                if (movingProjection != null)
                {
                    dicMovingProjection.Add(strKey, movingProjection);
                }
            }

            return dicMovingProjection;
        }

        protected Dictionary<string, MovingProjection> GetDefaultUVWMovingProjections()
        {
            Dictionary<string, MovingProjection> dicMovingProjection = new Dictionary<string, MovingProjection>();

            {
                string strKey = MotionKey.U.ToString();
                MovingProjection movingProjection = GetDefaultMovingProjection(strKey);
                if (movingProjection != null)
                {
                    dicMovingProjection.Add(strKey, movingProjection);
                }
            }
            {
                string strKey = MotionKey.V.ToString();
                MovingProjection movingProjection = GetDefaultMovingProjection(strKey);
                if (movingProjection != null)
                {
                    dicMovingProjection.Add(strKey, movingProjection);
                }
            }
            {
                string strKey = MotionKey.W.ToString();
                MovingProjection movingProjection = GetDefaultMovingProjection(strKey);
                if (movingProjection != null)
                {
                    dicMovingProjection.Add(strKey, movingProjection);
                }
            }

            return dicMovingProjection;
        }

        protected Dictionary<string, MovingProjection> GetDefaultXYMovingProjections()
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

            return dicMovingProjection;
        }

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

        protected Dictionary<string, MovingProjection> GetDefaultElevatorZMovingProjections()
        {
            Dictionary<string, MovingProjection> dicMovingProjection = new Dictionary<string, MovingProjection>();
            {
                string strKey = MotionKey.EZ.ToString();
                MovingProjection movingProjection = GetDefaultMovingProjection(strKey);
                if (movingProjection != null)
                {
                    dicMovingProjection.Add(strKey, movingProjection);
                }
            }

            return dicMovingProjection;
        }

        protected Dictionary<string, MovingProjection> GetDefaultVisionZMovingProjections()
        {
            Dictionary<string, MovingProjection> dicMovingProjection = new Dictionary<string, MovingProjection>();
            {
                string strKey = MotionKey.VZ.ToString();
                MovingProjection movingProjection = GetDefaultMovingProjection(strKey);
                if (movingProjection != null)
                {
                    dicMovingProjection.Add(strKey, movingProjection);
                }
            }

            return dicMovingProjection;
        }

        public int UvwMovePosition(UvwCoordinate coordinate)
        {
            Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultMovingProjections();

            dicMovingProjection[MotionKey.U.ToString()].Position = coordinate.U;
            dicMovingProjection[MotionKey.V.ToString()].Position = coordinate.V;
            dicMovingProjection[MotionKey.W.ToString()].Position = coordinate.W;

            return Move(dicMovingProjection);
        }

        public int MovePosition(XyCoordinate coordinate)
        {
            Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultMovingProjections();

            dicMovingProjection[MotionKey.X.ToString()].Position = coordinate.X;
            dicMovingProjection[MotionKey.Y.ToString()].Position = coordinate.Y;
            
            return Move(dicMovingProjection);
        }

        public Task<int> UvwBeginMovePosition(UvwCoordinate coordinate)
        {
            Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultMovingProjections();

            dicMovingProjection[MotionKey.U.ToString()].Position = coordinate.U;
            dicMovingProjection[MotionKey.V.ToString()].Position = coordinate.V;
            dicMovingProjection[MotionKey.W.ToString()].Position = coordinate.W;

            return BeginMove(dicMovingProjection);
        }

        public Task<int> BeginMovePosition(XyCoordinate coordinate)
        {
            Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultMovingProjections();

            dicMovingProjection[MotionKey.X.ToString()].Position = coordinate.X;
            dicMovingProjection[MotionKey.Y.ToString()].Position = coordinate.Y;

            return BeginMove(dicMovingProjection);
        }

        //public int MoveZPosition(double dPosition)
        //{
        //    Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultZMovingProjections();

        //    dicMovingProjection[MotionKey.Z.ToString()].Position = dPosition;

        //    return Move(dicMovingProjection);
        //}

        public int MoveElevatorZPosition(double dPosition)
        {
            Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultElevatorZMovingProjections();

            dicMovingProjection[MotionKey.EZ.ToString()].Position = dPosition;

            return Move(dicMovingProjection);
        }

        public int MoveVisionZPosition(double dPosition)
        {
            Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultVisionZMovingProjections();

            dicMovingProjection[MotionKey.VZ.ToString()].Position = dPosition;

            return Move(dicMovingProjection);
        }

        public Task<int> BeginMoveElevatorZPosition(double dPosition)
        {
            Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultElevatorZMovingProjections();

            dicMovingProjection[MotionKey.EZ.ToString()].Position = dPosition;
            
            return BeginMove(dicMovingProjection);
        }

        public Task<int> BeginMoveVisionZPosition(double dPosition)
        {
            Dictionary<string, MovingProjection> dicMovingProjection = GetDefaultVisionZMovingProjections();

            dicMovingProjection[MotionKey.VZ.ToString()].Position = dPosition;

            return BeginMove(dicMovingProjection);
        }

        public double GetCurrentEZPosition()
        {
            double dPosition = 0;
            MotionAxis axis = m_dicAxes[MotionKey.EZ.ToString()];
            if(axis != null)
            {
                dPosition = axis.Motor.ActualPosition;
            }
            return dPosition;
        }

        public double GetCurrentVZPosition()
        {
            double dPosition = 0;
            MotionAxis axis = m_dicAxes[MotionKey.VZ.ToString()];
            if (axis != null)
            {
                dPosition = axis.Motor.ActualPosition;
            }
            return dPosition;
        }

        public double GetCurrentUPosition()
        {
            double dPosition = 0;
            MotionAxis axis = m_dicAxes[MotionKey.U.ToString()];
            if (axis != null)
            {
                dPosition = axis.Motor.ActualPosition;
            }
            return dPosition;
        }

        public double GetCurrentVPosition()
        {
            double dPosition = 0;
            MotionAxis axis = m_dicAxes[MotionKey.V.ToString()];
            if (axis != null)
            {
                dPosition = axis.Motor.ActualPosition;
            }
            return dPosition;
        }

        public double GetCurrentWPosition()
        {
            double dPosition = 0;
            MotionAxis axis = m_dicAxes[MotionKey.W.ToString()];
            if (axis != null)
            {
                dPosition = axis.Motor.ActualPosition;
            }
            return dPosition;
        }

        public double GetCurrentXPosition()
        {
            double dPosition = 0;
            MotionAxis axis = m_dicAxes[MotionKey.X.ToString()];
            if (axis != null)
            {
                dPosition = axis.Motor.ActualPosition;
            }
            return dPosition;
        }

        public double GetCurrentYPosition()
        {
            double dPosition = 0;
            MotionAxis axis = m_dicAxes[MotionKey.Y.ToString()];
            if (axis != null)
            {
                dPosition = axis.Motor.ActualPosition;
            }
            return dPosition;
        }




        public stWaferProbeAlignParam GetPositionInformation(string m_strPosName)
        {
            stWaferProbeAlignParam m_stLaserPosParam = new stWaferProbeAlignParam();

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

                    //m_stLaserPosParam.dTarget[nPosCnt++] = m_dPos[nPosCnt] = Convert.ToDouble(parameters[i++].Value);       //  X
                    //m_stLaserPosParam.dTarget[nPosCnt++] = m_dPos[nPosCnt] = Convert.ToDouble(parameters[i++].Value);       //  Y
                    //m_stLaserPosParam.dTarget[nPosCnt++] = m_dPos[nPosCnt] = Convert.ToDouble(parameters[i++].Value);       //  T
                    //m_stLaserPosParam.dTarget[nPosCnt] = m_dPos[nPosCnt] = Convert.ToDouble(parameters[i++].Value);         //  Z

                    ////  Offset 값도 가져오자.
                    //nPosCnt = 0;
                    //m_stLaserPosParam.dOffset[nPosCnt++] = Convert.ToDouble(parameters[i++].Value);                         //  X
                    //m_stLaserPosParam.dOffset[nPosCnt++] = Convert.ToDouble(parameters[i++].Value);                         //  Y
                    //m_stLaserPosParam.dOffset[nPosCnt++] = Convert.ToDouble(parameters[i++].Value);                         //  T
                    //m_stLaserPosParam.dOffset[nPosCnt] = Convert.ToDouble(parameters[i++].Value);                           //  Z


                    //  Z(2) 랑 T(3) 를 서로 바꿔야 한다. 데이터 순서는 X-Y-Z-T 인데, 모터 축 순서는 X-Y-T-Z 이기 때문에...
                    //  다른 좋은 방법이 있겠지만 시간이 없으니 일단 이렇게 슥삭...
                    m_stLaserPosParam.dTarget[0] = m_dPos[0] = Convert.ToDouble(parameters[i++].Value);       //  U
                    m_stLaserPosParam.dTarget[1] = m_dPos[1] = Convert.ToDouble(parameters[i++].Value);       //  V
                    m_stLaserPosParam.dTarget[2] = m_dPos[2] = Convert.ToDouble(parameters[i++].Value);       //  W
                    m_stLaserPosParam.dTarget[3] = m_dPos[3] = Convert.ToDouble(parameters[i++].Value);       //  EZ
                    m_stLaserPosParam.dTarget[4] = m_dPos[4] = Convert.ToDouble(parameters[i++].Value);       //  X
                    m_stLaserPosParam.dTarget[5] = m_dPos[5] = Convert.ToDouble(parameters[i++].Value);       //  Y
                    m_stLaserPosParam.dTarget[6] = m_dPos[6] = Convert.ToDouble(parameters[i++].Value);       //  VZ

                    //  Offset 값도 가져오자.
                    nPosCnt = 0;
                    m_stLaserPosParam.dOffset[0] = Convert.ToDouble(parameters[i++].Value);                   //  U
                    m_stLaserPosParam.dOffset[1] = Convert.ToDouble(parameters[i++].Value);                   //  V
                    m_stLaserPosParam.dOffset[2] = Convert.ToDouble(parameters[i++].Value);                   //  W
                    m_stLaserPosParam.dOffset[3] = Convert.ToDouble(parameters[i++].Value);                   //  EZ
                    m_stLaserPosParam.dOffset[4] = Convert.ToDouble(parameters[i++].Value);                   //  X
                    m_stLaserPosParam.dOffset[5] = Convert.ToDouble(parameters[i++].Value);                   //  Y
                    m_stLaserPosParam.dOffset[6] = Convert.ToDouble(parameters[i++].Value);                   //  VZ


                    break;

#else           //  요렇게 하던지...

                    if (parameters[i].Tag == "X")
                    {
                        m_stDispParam.dTarget[0] = m_dPos[0] = Convert.ToDouble(parameters[i].Value);      //  X
                        nPosCnt++;
                    }

                    if (parameters[i].Tag == "Y")
                    {
                        m_stDispParam.dTarget[1] = m_dPos[1] = Convert.ToDouble(parameters[i].Value);      //  Y
                        nPosCnt++;
                    }

                    if (parameters[i].Tag == "T")
                    {
                        m_stDispParam.dTarget[2] = m_dPos[2] = Convert.ToDouble(parameters[i].Value);      //  T
                        nPosCnt++;
                    }

                    if (parameters[i].Tag == "Z")
                    {
                        m_stDispParam.dTarget[3] = m_dPos[3] = Convert.ToDouble(parameters[i].Value);      //  Z
                        nPosCnt++;
                    }

                    if (nPosCnt >= 4)
                    {
                        //  Offset 값도 가져오자.
                        if (parameters[i + 1].Tag == "X")
                        {
                            m_stDispParam.dOffset[0] = Convert.ToDouble(parameters[i + 1].Value);      //  X
                        }

                        if (parameters[i + 2].Tag == "Y")
                        {
                            m_stDispParam.dOffset[1] = Convert.ToDouble(parameters[i + 2].Value);      //  Y
                        }

                        if (parameters[i + 3].Tag == "T")
                        {
                            m_stDispParam.dOffset[2] = Convert.ToDouble(parameters[i + 3].Value);      //  T
                        }

                        if (parameters[i + 4].Tag == "Z")
                        {
                            m_stDispParam.dOffset[3] = Convert.ToDouble(parameters[i + 4].Value);      //  Z
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



        #region DI Functions (공통)

        public bool DI_OpSwitch_Start()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Input_OpSwitch_Start.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_OpSwitch_Stop()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Input_OpSwitch_Stop.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_OpSwitch_Reset()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Input_OpSwitch_Reset.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_OpSwitch_EMG()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Input_OpSwitch_EMG.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_Main_CDACheck()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Input_Main_CDACheck.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_Main_VacuumCheck()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Input_Main_VacuumCheck.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_ThinChuck_VacuumCheck()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Input_ThinChuck_VacuumCheck.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_Wafer_VacuumCheck()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Input_Wafer_VacuumCheck.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_Probe_PackingCheck()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Input_Probe_PackingCheck.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_ThinChuck_Detect()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Input_ThinChuck_Detect.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }
        #endregion


        #region DI Functions (1호기)
        public bool DI_TopCover_Up()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Input_TopCover_Up.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_TopCover_Down()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Input_TopCover_Down.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_Probe_BW_Detect()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Input_Probe_BW_Detect.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return !bRet;
        }
        #endregion


        #region DI Functions (2 ~ 6호기)
        public bool DI_Probe_LeftClampModule_FW()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Input_Probe_LeftClampModule_FW.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_Probe_LeftClampModule_BW()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Input_Probe_LeftClampModule_BW.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_Probe_RightClampModule_FW()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Input_Probe_RightClampModule_FW.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_Probe_RightClampModule_BW()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Input_Probe_RightClampModule_BW.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_Probe_UnpackingCyl_Down()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Input_Probe_UnpackingCyl_Down.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_Probe_UnpackingCyl_Up()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Input_Probe_UnpackingCyl_Up.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }
        #endregion


        #region DO Functions (공통)

        public int DO_OpLamp_Start(bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_OpLamp_Start.ToString()];

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        public int DO_OpLamp_Stop(bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_OpLamp_Stop.ToString()];

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        public int DO_OpLamp_Reset(bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_OpLamp_Reset.ToString()];

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        public int DO_Probe_UnPacking(bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Probe_Unpacking.ToString()];

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        public int DO_ThinChuck_StageCleaning(bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_ThinChuck_StageCleaning.ToString()];

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        public int DO_ThinChuck_Vacuum(bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_ThinChuck_Vacuum.ToString()];

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        public int DO_Wafer_Vacuum(bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Wafer_Vacuum.ToString()];

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        public int DO_Probe_Packing(bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Probe_Packing.ToString()];

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        #endregion


        #region DO Functions (1 호기)
        public int DO_TopCover_Up(bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_TopCover_Up.ToString()];

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        public int DO_TopCover_Down(bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_TopCover_Down.ToString()];

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }
        #endregion


        #region DO Functions (2 ~ 6 호기)
        public int DO_ProbeClampModule_Down(bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Probe_ClampModule_Down.ToString()];

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        public int DO_ProbeClampModule_FW(bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Probe_ClampModule_FW.ToString()];

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        public int DO_ProbeClampModule_BW(bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Probe_ClampModule_BW.ToString()];

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        public int DO_Probe_UnpackingCyl_Down(bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Probe_UnpackingCyl_Down.ToString()];

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        public int DO_Probe_UnpackingCyl_Up(bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Probe_UnpackingCyl_Up.ToString()];

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }
        #endregion

        #region DO Status

        public bool IsDO_OpLamp_Start()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_OpLamp_Start.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool IsDO_OpLamp_Stop()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_OpLamp_Stop.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool IsDO_OpLamp_Reset()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_OpLamp_Reset.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        //public bool IsDO_LED_Light(int m_nCh, int m_nRGB)
        //{
        //    bool bRet = false;

        //    DioPoint dioString = null;

        //    //  해당 출력 채널 상태 리턴
        //    switch (m_nCh)
        //    {
        //        case 0:
        //            switch (m_nRGB)
        //            {
        //                case (int)LED_Light.Red:    dioString = m_dicDioPoints[DioPointKey.Output_Lamp0_Red.ToString()];    break;
        //                case (int)LED_Light.Green:  dioString = m_dicDioPoints[DioPointKey.Output_Lamp0_Green.ToString()];  break;
        //                case (int)LED_Light.Blue:   dioString = m_dicDioPoints[DioPointKey.Output_Lamp0_Blue.ToString()]; break;
        //            }

        //            break;


        //        case 1:
        //            switch (m_nRGB)
        //            {
        //                case (int)LED_Light.Red:    dioString = m_dicDioPoints[DioPointKey.Output_Lamp1_Red.ToString()];    break;
        //                case (int)LED_Light.Green:  dioString = m_dicDioPoints[DioPointKey.Output_Lamp1_Green.ToString()];  break;
        //                case (int)LED_Light.Blue:   dioString = m_dicDioPoints[DioPointKey.Output_Lamp1_Blue.ToString()]; break;
        //            }

        //            break;


        //        case 2:
        //            switch (m_nRGB)
        //            {
        //                case (int)LED_Light.Red:    dioString = m_dicDioPoints[DioPointKey.Output_Lamp2_Red.ToString()];    break;
        //                case (int)LED_Light.Green:  dioString = m_dicDioPoints[DioPointKey.Output_Lamp2_Green.ToString()];  break;
        //                case (int)LED_Light.Blue:   dioString = m_dicDioPoints[DioPointKey.Output_Lamp2_Blue.ToString()]; break;
        //            }

        //            break;


        //        case 3:
        //            switch (m_nRGB)
        //            {
        //                case (int)LED_Light.Red:    dioString = m_dicDioPoints[DioPointKey.Output_Lamp3_Red.ToString()];    break;
        //                case (int)LED_Light.Green:  dioString = m_dicDioPoints[DioPointKey.Output_Lamp3_Green.ToString()];  break;
        //                case (int)LED_Light.Blue:   dioString = m_dicDioPoints[DioPointKey.Output_Lamp3_Blue.ToString()]; break;
        //            }

        //            break;
        //    }

        //    if (dioString == null)
        //        return bRet;

        //    DioValue ioValue = dioString.GetValue();

        //    if (ioValue == DioValue.On)
        //        bRet = true;

        //    return bRet;
        //}

        public bool IsDO_Probe_Unpacking()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Probe_Unpacking.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool IsDO_ThinChuck_StageCleaning()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_ThinChuck_StageCleaning.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool IsDO_ThinChuck_Vacuum()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_ThinChuck_Vacuum.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool IsDO_Wafer_Vacuum()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Wafer_Vacuum.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool IsDO_Probe_Packing()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Probe_Packing.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool IsDO_TopCover_Up()          //  1호기 (Type-A)
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_TopCover_Up.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool IsDO_TopCover_Down()        //  1호기 (Type-A)
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_TopCover_Down.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool IsDO_Probe_ClampModule_Down()        //  2 ~ 6호기 (Type-B)
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Probe_ClampModule_Down.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool IsDO_Probe_ClampModule_FW()        //  2 ~ 6호기 (Type-B)
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Probe_ClampModule_FW.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool IsDO_Probe_ClampModule_BW()        //  2 ~ 6호기 (Type-B)
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Probe_ClampModule_BW.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool IsDO_Probe_UnpackingCyl_Down()        //  2 ~ 6호기 (Type-B)
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Probe_UnpackingCyl_Down.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool IsDO_Probe_UnpackingCyl_Up()        //  2 ~ 6호기 (Type-B)
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Probe_UnpackingCyl_Up.ToString()];

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
