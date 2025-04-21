using QMC.Common.Modules;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QMC.Common.Parts
{
    public class UnloaderParameter : MotionPart
    {
        #region Enum List

        #endregion

        #region => Axis Define 


#if true                                                                //  SLD-200C
//#if false                                                               //  SLD-200U
        public enum AxisAjinEnum                                                       //  SLD-200C 에서 사용하는 축 번호    
        {
            //  축 번호 변경 전 (Z0:10,   Z1:11,  TR_X:12,    TR_Z:13)
            //  축 번호 변경 후 (Z0:10,   Z1:11,  TR_X:12,    TR_Z:13) - 변동 없음

            Z0 = 10,
            Z1,
            TR_X,
            TR_Z,

            Max,
        }
#else
        public enum AxisAjinEnum                                                       //  SLD-200U 에서 사용하는 축 번호   
        {
            //  축 번호 변경 전 (Z0:10,   Z1:11,  TR_X:12,    TR_Z:13)
            //  축 번호 변경 후 (Z0:10,   Z1:11,  TR_X:12,    TR_Z:13) - 변동 없음

            Z0 = 9,
            Z1,
            TR_X,
            TR_Z,

            Max,
        }
#endif

        
        #endregion

        public enum MotionKey
        {
            Z0,
            Z1,
            TR_X,
            TR_Z,

            Max,
        }

        public enum StackerTable
        {
            Stacker_0,
            Stacker_1
        }

        public enum PickerVacuumPos
        {
            Inner = 0,
            Outer
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

        public struct stUnloaderParam
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
            Input_Unloader_NG_Stacker_FullCheck,          //  X048
            Input_Unloader_Stacker0_MaterialCheck,        //  X049
            Input_Unloader_Stacker0_FullCheck,            //  X050
            Input_Unloader_Stacker1_MaterialCheck,        //  X051
            Input_Unloader_Stacker1_FullCheck,            //  X052
            Input_Unloader_Picker_InnerVacuumCheck,       //  X053
            Input_Unloader_Picker_OuterVacuumCheck,       //  X054
            Input_Unloader_Front_DoorCheck,               //  X055
            Input_Unloader_Left_DoorCheck,                //  X056



            /// <summary>
            /// Output
            /// </summary>
            /// 
            Output_Unloader_Picker_InnerVacuum,           //  Y048
            Output_Unloader_Picker_OuterVacuum,           //  Y049
            Output_Unloader_Picker_Blow,                  //  Y050
        }


        public UnloaderParameterConfig Config { get; set; }
        public UnloaderParameterRecipe Recipe { get; set; }

        public stUnloaderParam stUnloaderPosParam;
        public stUnloaderParam stUnloaderPosParam2;
        public stUnloaderParam stUnloaderPosParam_Verify;             //  위치 좌표 무결성 검사

        public UnloaderParameter(string strName) : base(strName)
        {
            Config = new UnloaderParameterConfig();
            Recipe = new UnloaderParameterRecipe();
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

            m_dicAxisDisplayType.Add(MotionKey.Z0.ToString(), DisplayAxisType.CombinationPicker);
            m_dicAxisDisplayType.Add(MotionKey.Z1.ToString(), DisplayAxisType.CombinationPicker);
            m_dicAxisDisplayType.Add(MotionKey.TR_X.ToString(), DisplayAxisType.CombinationPicker);
            m_dicAxisDisplayType.Add(MotionKey.TR_Z.ToString(), DisplayAxisType.CombinationPicker);

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
                string strKey = MotionKey.Z0.ToString();
                MovingProjection movingProjection = GetDefaultMovingProjection(strKey);
                if (movingProjection != null)
                {
                    dicMovingProjection.Add(strKey, movingProjection);
                }
            }
            {
                string strKey = MotionKey.Z1.ToString();
                MovingProjection movingProjection = GetDefaultMovingProjection(strKey);
                if (movingProjection != null)
                {
                    dicMovingProjection.Add(strKey, movingProjection);
                }
            }
            {
                string strKey = MotionKey.TR_X.ToString();
                MovingProjection movingProjection = GetDefaultMovingProjection(strKey);
                if (movingProjection != null)
                {
                    dicMovingProjection.Add(strKey, movingProjection);
                }
            }
            {
                string strKey = MotionKey.TR_Z.ToString();
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

        public stUnloaderParam GetPositionInformation(string m_strPosName)
        {
            stUnloaderParam m_stUnloaderPosParam = new stUnloaderParam();

            int count = System.Enum.GetValues(typeof(MotionKey)).Length;
            m_stUnloaderPosParam.nAxis = new int[count];
            m_stUnloaderPosParam.dTarget = new double[count];
            m_stUnloaderPosParam.dVel = new double[count];
            m_stUnloaderPosParam.dAcc = new double[count];
            m_stUnloaderPosParam.dDec = new double[count];
            m_stUnloaderPosParam.dOffset = new double[count];

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


                    //  다른 좋은 방법이 있겠지만 시간이 없으니 일단 이렇게 슥삭...
                    m_stUnloaderPosParam.dTarget[0] = m_dPos[0] = Convert.ToDouble(parameters[i++].Value);       //  Z0
                    m_stUnloaderPosParam.dTarget[1] = m_dPos[1] = Convert.ToDouble(parameters[i++].Value);       //  Z1
                    m_stUnloaderPosParam.dTarget[2] = m_dPos[2] = Convert.ToDouble(parameters[i++].Value);       //  TR_X
                    m_stUnloaderPosParam.dTarget[3] = m_dPos[3] = Convert.ToDouble(parameters[i++].Value);       //  TR_Z

                    //  Offset 값도 가져오자.
                    nPosCnt = 0;
                    m_stUnloaderPosParam.dOffset[0] = Convert.ToDouble(parameters[i++].Value);                   //  Z0
                    m_stUnloaderPosParam.dOffset[1] = Convert.ToDouble(parameters[i++].Value);                   //  Z1
                    m_stUnloaderPosParam.dOffset[2] = Convert.ToDouble(parameters[i++].Value);                   //  TR_X
                    m_stUnloaderPosParam.dOffset[3] = Convert.ToDouble(parameters[i++].Value);                   //  TR_Z

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
                        m_stUnloaderPosParam.nAxis[count] = axis.No;                  //  축 번호
                        m_stUnloaderPosParam.dVel[count] = movingProjection.Velocity;     //  속도
                        m_stUnloaderPosParam.dAcc[count] = movingProjection.Acceleration; //  가속도
                        m_stUnloaderPosParam.dDec[count] = movingProjection.Deceleration; //  감속도

                        //Limit 확인.
                        if (!axis.Motor.CheckLimit(m_dPos[count++]))
                        {
                            //ret = -1;
                            //break;
                        }
                    }
                }
            }

            return m_stUnloaderPosParam;
        }




        #region DI Functions (Unloader)

        public bool DI_Unloader_NG_Stacker_FullCheck()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Input_Unloader_NG_Stacker_FullCheck.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_Unloader_Stacker_MaterialCheck(int m_nPos)
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            switch (m_nPos)
            {
                case 0:
                    dioString = m_dicDioPoints[DioPointKey.Input_Unloader_Stacker0_MaterialCheck.ToString()];
                    break;


                case 1:
                    dioString = m_dicDioPoints[DioPointKey.Input_Unloader_Stacker1_MaterialCheck.ToString()];
                    break;
            }

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_Unloader_Stacker_FullCheck(int m_nPos)
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            switch (m_nPos)
            {
                case 0:
                    dioString = m_dicDioPoints[DioPointKey.Input_Unloader_Stacker0_FullCheck.ToString()];
                    break;


                case 1:
                    dioString = m_dicDioPoints[DioPointKey.Input_Unloader_Stacker1_FullCheck.ToString()];
                    break;
            }

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_Unloader_Picker_VacuumCheck(int m_nPos)
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            switch (m_nPos)
            {
                case 0:
                    dioString = m_dicDioPoints[DioPointKey.Input_Unloader_Picker_InnerVacuumCheck.ToString()];
                    break;


                case 1:
                    dioString = m_dicDioPoints[DioPointKey.Input_Unloader_Picker_OuterVacuumCheck.ToString()];
                    break;
            }

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }
        #endregion


        #region DO Functions (Unloader)

        public int DO_Unloader_Picker_Vacuum(int m_nPos, bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            switch (m_nPos)
            {
                case 0:
                    dioString = m_dicDioPoints[DioPointKey.Output_Unloader_Picker_InnerVacuum.ToString()];
                    break;


                case 1:
                    dioString = m_dicDioPoints[DioPointKey.Output_Unloader_Picker_OuterVacuum.ToString()];
                    break;
            }

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        public int DO_Unloader_Picker_Blow(bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Unloader_Picker_Blow.ToString()];

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }
        #endregion


        #region DO Status (Unloader)

        public bool IsDO_Unloader_Picker_Vacuum(int m_nPos)
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            switch (m_nPos)
            {
                case 0:
                    dioString = m_dicDioPoints[DioPointKey.Output_Unloader_Picker_InnerVacuum.ToString()];
                    break;


                case 1:
                    dioString = m_dicDioPoints[DioPointKey.Output_Unloader_Picker_OuterVacuum.ToString()];
                    break;
            }

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool IsDO_Unloader_Picker_Blow()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Unloader_Picker_Blow.ToString()];

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
