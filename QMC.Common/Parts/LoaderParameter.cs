
using QMC.Common.Modules;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QMC.Common.Parts
{
    public class LoaderParameter : MotionPart
    {
        #region Enum List

        #endregion

        #region => Axis Define 


#if SLD_200C                                                                //  SLD-200C
//#if false                                                               //  SLD-200U
        public enum AxisAjinEnum                                                       //  SLD-200C 에서 사용하는 축 번호    
        {
            //  축 번호 변경 전 (Z0:4,    Z1:5,   TR_X:6,     TR_Z:7,     ALN_X:8,    ALN_Y:9)
            //  축 번호 변경 후 (Z0:6,    Z1:7,   TR_X:8,     TR_Z:9,     ALN_X:1,    ALN_Y:2)

            Z0 = 6,
            Z1 = 7,
            TR_X = 8,
            TR_Z = 9,
            ALN_X = 1,
            ALN_Y = 2,
        }
#else
        public enum AxisAjinEnum                                                       //  SLD-200U 에서 사용하는 축 번호   
        {
            //  축 번호 변경 전 (Z0:4,    Z1:5,   TR_X:6,     TR_Z:7,     ALN_X:8,    ALN_Y:9)
            //  축 번호 변경 후 (Z0:6,    Z1:7,   TR_X:8,     TR_Z:9,     ALN_X:1,    ALN_Y:2)

            Z0 = 5,
            Z1 = 6,
            TR_X = 7,
            TR_Z = 8,
            ALN_X = 0,
            ALN_Y = 1,
        }
#endif

        
        #endregion

        public enum MotionKey
        {
            Z0,
            Z1,
            TR_X,
            TR_Z,
            ALN_X,
            ALN_Y,

            Max,
        }

        public enum StackerTable
        {
            Stacker_0 = 0,
            Stacker_1
        }

        public enum PickerVacuumPos
        {
            Inner = 0,
            Outer
        }

        public enum MAlignerVacuumPos
        {
            Center = 0,
            Inner,
            Outer
        }

        public enum LED_Light
        {
            Red = 0,
            Green,
            Blue,
        }

        public struct stLoaderParam
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
            Input_Loader_Stacker0_MaterialCheck,        //  X032
            Input_Loader_Stacker0_FullCheck,            //  X033
            Input_Loader_Stacker1_MaterialCheck,        //  X034
            Input_Loader_Stacker1_FullCheck,            //  X035
            Input_Loader_Picker_InnerVacuumCheck,       //  X036
            Input_Loader_Picker_OuterVacuumCheck,       //  X037
            Input_Loader_Aligner_Center_VacuumCheck,    //  X038                    //  변경 (1개만 있던 것 -> 3개로 변경)
            Input_Loader_Ionizer0_AlarmCheck,           //  X039
            Input_Loader_Ionizer1_AlarmCheck,           //  X040
            Input_Loader_Front_DoorCheck,               //  X041
            Input_Loader_Right_DoorCheck,               //  X042
            Input_Loader_Aligner_Inner_VacuumCheck,     //  X043                    //  추가
            Input_Loader_Aligner_Outer_VacuumCheck,     //  X044                    //  추가



            /// <summary>
            /// Output
            /// </summary>
            /// 
            Output_Loader_Picker_InnerVacuum,           //  Y032
            Output_Loader_Picker_OuterVacuum,           //  Y033
            Output_Loader_Picker_Blow,                  //  Y034
            Output_Loader_Aligner_CenterVacuum,         //  Y035
            Output_Loader_Aligner_InnerVacuum,          //  Y036
            Output_Loader_Aligner_OuterVacuum,          //  Y037
            Output_Loader_Aligner_Center_Blow,          //  Y038                    //  변경 (1개만 있던 것 -> 3개로 변경)
            Output_Loader_Ionizer_On,                   //  Y039
            Output_Loader_Aligner_Inner_Blow,           //  Y040                    //  추가
            Output_Loader_Aligner_Outer_Blow,           //  Y041                    //  추가
        }


        public LoaderParameterConfig Config { get; set; }
        public LoaderParameterRecipe Recipe { get; set; }

        public stLoaderParam stLoaderPosParam;
        public stLoaderParam stLoaderPosParam2;
        public stLoaderParam stLoaderPosParam_Verify;             //  위치 좌표 무결성 검사

        public LoaderParameter(string strName) : base(strName)
        {
            Config = new LoaderParameterConfig();
            Recipe = new LoaderParameterRecipe();
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
            m_dicAxisDisplayType.Add(MotionKey.ALN_X.ToString(), DisplayAxisType.CombinationHorizontal);
            m_dicAxisDisplayType.Add(MotionKey.ALN_Y.ToString(), DisplayAxisType.CombinationVertical);

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
            {
                string strKey = MotionKey.ALN_X.ToString();
                MovingProjection movingProjection = GetDefaultMovingProjection(strKey);
                if (movingProjection != null)
                {
                    dicMovingProjection.Add(strKey, movingProjection);
                }
            }
            {
                string strKey = MotionKey.ALN_Y.ToString();
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

        public stLoaderParam GetPositionInformation(string m_strPosName)
        {
            stLoaderParam m_stLoaderPosParam = new stLoaderParam();

            int count = System.Enum.GetValues(typeof(MotionKey)).Length;
            m_stLoaderPosParam.nAxis = new int[count];
            m_stLoaderPosParam.dTarget = new double[count];
            m_stLoaderPosParam.dVel = new double[count];
            m_stLoaderPosParam.dAcc = new double[count];
            m_stLoaderPosParam.dDec = new double[count];
            m_stLoaderPosParam.dOffset = new double[count];

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
                    m_stLoaderPosParam.dTarget[0] = m_dPos[0] = Equipment.ToDouble(parameters[i++].Value);       //  Z0
                    m_stLoaderPosParam.dTarget[1] = m_dPos[1] = Equipment.ToDouble(parameters[i++].Value);       //  Z1
                    m_stLoaderPosParam.dTarget[2] = m_dPos[2] = Equipment.ToDouble(parameters[i++].Value);       //  TR_X
                    m_stLoaderPosParam.dTarget[3] = m_dPos[3] = Equipment.ToDouble(parameters[i++].Value);       //  TR_Z
                    m_stLoaderPosParam.dTarget[4] = m_dPos[4] = Equipment.ToDouble(parameters[i++].Value);       //  ALN_X
                    m_stLoaderPosParam.dTarget[5] = m_dPos[5] = Equipment.ToDouble(parameters[i++].Value);       //  ALN_Y

                    //  Offset 값도 가져오자.
                    nPosCnt = 0;
                    m_stLoaderPosParam.dOffset[0] = Equipment.ToDouble(parameters[i++].Value);                   //  Z0
                    m_stLoaderPosParam.dOffset[1] = Equipment.ToDouble(parameters[i++].Value);                   //  Z1
                    m_stLoaderPosParam.dOffset[2] = Equipment.ToDouble(parameters[i++].Value);                   //  TR_X
                    m_stLoaderPosParam.dOffset[3] = Equipment.ToDouble(parameters[i++].Value);                   //  TR_Z
                    m_stLoaderPosParam.dOffset[4] = Equipment.ToDouble(parameters[i++].Value);                   //  ALN_X
                    m_stLoaderPosParam.dOffset[5] = Equipment.ToDouble(parameters[i++].Value);                   //  ALN_Y

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
                        m_stLoaderPosParam.nAxis[count] = axis.No;                  //  축 번호
                        m_stLoaderPosParam.dVel[count] = movingProjection.Velocity;     //  속도
                        m_stLoaderPosParam.dAcc[count] = movingProjection.Acceleration; //  가속도
                        m_stLoaderPosParam.dDec[count] = movingProjection.Deceleration; //  감속도

                        //Limit 확인.
                        if (!axis.Motor.CheckLimit(m_dPos[count++]))
                        {
                            //ret = -1;
                            //break;
                        }
                    }
                }
            }

            return m_stLoaderPosParam;
        }




        #region DI Functions (Loader)

        public bool DI_Loader_Stacker_MaterialCheck(int m_nPos)
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            switch (m_nPos)
            {
                case 0:
                    dioString = m_dicDioPoints[DioPointKey.Input_Loader_Stacker0_MaterialCheck.ToString()];
                    break;


                case 1:
                    dioString = m_dicDioPoints[DioPointKey.Input_Loader_Stacker1_MaterialCheck.ToString()];
                    break;
            }

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_Loader_Stacker_FullCheck(int m_nPos)
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            switch (m_nPos)
            {
                case 0:
                    dioString = m_dicDioPoints[DioPointKey.Input_Loader_Stacker0_FullCheck.ToString()];
                    break;


                case 1:
                    dioString = m_dicDioPoints[DioPointKey.Input_Loader_Stacker1_FullCheck.ToString()];
                    break;
            }

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_Loader_Picker_VacuumCheck(int m_nPos)
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            switch (m_nPos)
            {
                case 0:
                    dioString = m_dicDioPoints[DioPointKey.Input_Loader_Picker_InnerVacuumCheck.ToString()];
                    break;


                case 1:
                    dioString = m_dicDioPoints[DioPointKey.Input_Loader_Picker_OuterVacuumCheck.ToString()];
                    break;
            }

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_Loader_Aligner_VacuumCheck(int m_nPos)
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            switch (m_nPos)
            {
                case 0:
                    dioString = m_dicDioPoints[DioPointKey.Input_Loader_Aligner_Center_VacuumCheck.ToString()];
                    break;


                case 1:
                    dioString = m_dicDioPoints[DioPointKey.Input_Loader_Aligner_Inner_VacuumCheck.ToString()];
                    break;


                case 2:
                    dioString = m_dicDioPoints[DioPointKey.Input_Loader_Aligner_Outer_VacuumCheck.ToString()];
                    break;
            }

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool DI_Loader_Ionizer_AlarmCheck(int m_nPos)
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 채널 상태 리턴
            switch (m_nPos)
            {
                case 0:
                    dioString = m_dicDioPoints[DioPointKey.Input_Loader_Ionizer0_AlarmCheck.ToString()];
                    break;


                case 1:
                    dioString = m_dicDioPoints[DioPointKey.Input_Loader_Ionizer1_AlarmCheck.ToString()];
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


        #region DO Functions (Loader)

        public int DO_Loader_Picker_Vacuum(int m_nPos, bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            switch (m_nPos)
            {
                case 0:
                    dioString = m_dicDioPoints[DioPointKey.Output_Loader_Picker_InnerVacuum.ToString()];
                    break;


                case 1:
                    dioString = m_dicDioPoints[DioPointKey.Output_Loader_Picker_OuterVacuum.ToString()];
                    break;
            }

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        public int DO_Loader_Picker_Blow(bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Loader_Picker_Blow.ToString()];

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        public int DO_Loader_Aligner_Vacuum(int m_nPos, bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            switch (m_nPos)
            {
                case 0:
                    dioString = m_dicDioPoints[DioPointKey.Output_Loader_Aligner_CenterVacuum.ToString()];
                    break;


                case 1:
                    dioString = m_dicDioPoints[DioPointKey.Output_Loader_Aligner_InnerVacuum.ToString()];
                    break;


                case 2:
                    dioString = m_dicDioPoints[DioPointKey.Output_Loader_Aligner_OuterVacuum.ToString()];
                    break;
            }

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        public int DO_Loader_Aligner_Blow(int m_nPos, bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            switch (m_nPos)
            {
                case 0:
                    dioString = m_dicDioPoints[DioPointKey.Output_Loader_Aligner_Center_Blow.ToString()];
                    break;


                case 1:
                    dioString = m_dicDioPoints[DioPointKey.Output_Loader_Aligner_Inner_Blow.ToString()];
                    break;


                case 2:
                    dioString = m_dicDioPoints[DioPointKey.Output_Loader_Aligner_Outer_Blow.ToString()];
                    break;
            }

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        public int DO_Loader_Ionizer(bool m_bOnOff)
        {
            int nRet = 0;

            DioPoint dioString = null;

            //  해당 채널 출력 성공 여부 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Loader_Ionizer_On.ToString()];

            if (dioString == null) return -1;

            if (m_bOnOff) nRet = dioString.Write(DioValue.On);
            else nRet = dioString.Write(DioValue.Off);

            if (nRet != 0) return nRet;

            return nRet;
        }

        #endregion


        #region DO Status (Loader)

        public bool IsDO_Loader_Picker_Vacuum(int m_nPos)
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            switch (m_nPos)
            {
                case 0:
                    dioString = m_dicDioPoints[DioPointKey.Output_Loader_Picker_InnerVacuum.ToString()];
                    break;


                case 1:
                    dioString = m_dicDioPoints[DioPointKey.Output_Loader_Picker_OuterVacuum.ToString()];
                    break;
            }

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool IsDO_Loader_Picker_Blow()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Loader_Picker_Blow.ToString()];

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool IsDO_Loader_Aligner_Vacuum(int m_nPos)
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            switch (m_nPos)
            {
                case 0:
                    dioString = m_dicDioPoints[DioPointKey.Output_Loader_Aligner_CenterVacuum.ToString()];
                    break;


                case 1:
                    dioString = m_dicDioPoints[DioPointKey.Output_Loader_Aligner_InnerVacuum.ToString()];
                    break;


                case 2:
                    dioString = m_dicDioPoints[DioPointKey.Output_Loader_Aligner_OuterVacuum.ToString()];
                    break;
            }

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool IsDO_Loader_Aligner_Blow(int m_nPos)
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            switch (m_nPos)
            {
                case 0:
                    dioString = m_dicDioPoints[DioPointKey.Output_Loader_Aligner_Center_Blow.ToString()];
                    break;


                case 1:
                    dioString = m_dicDioPoints[DioPointKey.Output_Loader_Aligner_Inner_Blow.ToString()];
                    break;


                case 2:
                    dioString = m_dicDioPoints[DioPointKey.Output_Loader_Aligner_Outer_Blow.ToString()];
                    break;
            }

            if (dioString == null)
                return bRet;

            DioValue ioValue = dioString.GetValue();

            if (ioValue == DioValue.On)
                bRet = true;

            return bRet;
        }

        public bool IsDO_Loader_Ionizer_On()
        {
            bool bRet = false;

            DioPoint dioString = null;

            //  해당 출력 채널 상태 리턴
            dioString = m_dicDioPoints[DioPointKey.Output_Loader_Ionizer_On.ToString()];

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
