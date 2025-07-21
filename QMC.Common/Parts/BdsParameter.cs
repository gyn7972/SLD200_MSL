

//using ADLINKImport;
using QMC.Common.Modules;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QMC.Common.Parts
{
    public class BdsParameter : MotionPart
    {
        #region Enum List

        #endregion

        #region => Axis Define 


#if SLD_200C                                                            //  SLD-200C
//#if false                                                               //  SLD-200U
        public enum AxisAjinEnum                                                       //  SLD-200C 에서 사용하는 축 번호    
        {
            //  축 번호 변경 전 (MASK_Y:3)
            //  축 번호 변경 후 (MASK_Y:0)

            MASK_Y = 0,
        }
#else
        public enum AxisAjinEnum                                                       //  SLD-200U 에서 사용하는 축 번호   
        {
            //  축 번호 변경 전 (MASK_Y:3)
            //  축 번호 변경 후 (MASK_Y:0)

            MASK_Y = 13,                                                                 //  SLD-200U 에서는 없는 축. 
        }
#endif

        
        #endregion

        public enum MotionKey
        {
            MASK_Y,

            Max,
        }

        public enum StackerTable
        {
            Stacker_0,
            Stacker_1
        }

        public enum LED_Light
        {
            Red = 0,
            Green,
            Blue,
        }

        public struct stBdsParam
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
            Input_BDS_Mask_LeakCheck,                   //  X021
            Input_BDS_PowerMeter_FW,                    //  X022
            Input_BDS_PowerMeter_BW,                    //  X023  


            /// <summary>
            /// Output
            /// </summary>
            /// 
            Output_BDS_MaskCoolant_Supply,              //  Y009  
            Output_BDS_MaskCoolant_Return,              //  Y010
            Output_BDS_PowerMeter_FW,                   //  Y015
            Output_BDS_PowerMeter_BW,                   //  Y016
            Output_BDS_Purge,                           //  Y017
        }


        public BdsParameterConfig Config { get; set; }
        public BdsParameterRecipe Recipe { get; set; }

        public stBdsParam stBdsPosParam;
        public stBdsParam stBdsPosParam2;
        public stBdsParam stBdsPosParam_Verify;             //  위치 좌표 무결성 검사

        public BdsParameter(string strName) : base(strName)
        {
            Config = new BdsParameterConfig();
            Recipe = new BdsParameterRecipe();
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

        public stBdsParam GetPositionInformation(string m_strPosName)
        {
            stBdsParam m_stBdsPosParam = new stBdsParam();

            int count = System.Enum.GetValues(typeof(MotionKey)).Length;
            m_stBdsPosParam.nAxis = new int[count];
            m_stBdsPosParam.dTarget = new double[count];
            m_stBdsPosParam.dVel = new double[count];
            m_stBdsPosParam.dAcc = new double[count];
            m_stBdsPosParam.dDec = new double[count];
            m_stBdsPosParam.dOffset = new double[count];

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
                    m_stBdsPosParam.dTarget[0] = m_dPos[0] = Equipment.ToDouble(parameters[i++].Value);       //  Z0

                    //  Offset 값도 가져오자.
                    nPosCnt = 0;
                    m_stBdsPosParam.dOffset[0] = Equipment.ToDouble(parameters[i++].Value);                   //  Z0

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
                        m_stBdsPosParam.nAxis[count] = axis.No;                  //  축 번호
                        m_stBdsPosParam.dVel[count] = movingProjection.Velocity;     //  속도
                        m_stBdsPosParam.dAcc[count] = movingProjection.Acceleration; //  가속도
                        m_stBdsPosParam.dDec[count] = movingProjection.Deceleration; //  감속도

                        //Limit 확인.
                        if (!axis.Motor.CheckLimit(m_dPos[count++]))
                        {
                            //ret = -1;
                            //break;
                        }
                    }
                }
            }

            return m_stBdsPosParam;
        }




        #region DI Functions (BDS)

        public bool DI_BDS_Mask_LeakCheck()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Input_BDS_Mask_LeakCheck.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_BDS_PowerMeter_FW()
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

        public bool DI_BDS_PowerMeter_BW()
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

        #endregion


        #region DO Functions (BDS)

        public int DO_BDS_MaskCoolant_Supply(bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_BDS_MaskCoolant_Supply.ToString()];

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        public int DO_BDS_MaskCoolant_Return(bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_BDS_MaskCoolant_Return.ToString()];

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

        #endregion



        #region DO Status (BDS)

        public bool IsDO_BDS_MaskCoolant_Supply()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_BDS_MaskCoolant_Supply.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool IsDO_BDS_MaskCoolant_Return()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_BDS_MaskCoolant_Return.ToString()];

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

        #endregion
    }
}
