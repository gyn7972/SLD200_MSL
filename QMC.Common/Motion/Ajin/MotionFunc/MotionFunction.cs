using QMC.Common;
using QMC.Common.Modules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows;

namespace QMC.Common.Motion.Ajin.Motions
{
    //public class MotionFunction : MotionAxis
    public class MotionFunction
    {
        static WaferProbeAlign waferProbeAlign;

        public MotionFunction()
        {
            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;

            foreach (Module module in m_collectionModules)
            {
                if (module.Name == "WaferProbeAlign")
                {
                    waferProbeAlign = module as WaferProbeAlign;
                }
            }
        }


        #region MOTOR VALUE SET GET

        public bool MC_IsServoOn(int nAxis)
        {
            bool m_bRet = false;

            AXM.GetAmpEnabled(nAxis, ref m_bRet);

            return m_bRet;
        }

        public bool MC_SetServoOnOff(int nAxis, bool m_bOnOff)
        {
            bool m_bRet = false;

            if (AXM.SetAmpEnabled(nAxis, m_bOnOff) != 0)
                m_bOnOff = true;

            return m_bRet;
        }

        public uint MC_GetStatus(int nAxis, int point)
        {
            //if (nAxis >= m_lAxisCounts) return 0;        
            //return SignalState[(id * 8) + point];

            return 1;
        }

        public bool MC_GetDone(int nAxis)
        {
            bool m_bRet = false;

            AXM.GetInMotion(nAxis, ref m_bRet);             //  구동 중이면 1, 정지 상태이면 0

            //if (id >= m_lAxisCounts) return 0;
            //return SignalState[(id * 8) + point];

            return !m_bRet;
        }

        public double MC_GetEncPos(int nAxis)
        {
            int ret = 0;
            double m_dPos = 0.0;
            double dCurrent = 0;


            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //  2D 맵핑을 사용할 경우
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///
            //double dCurrent_Other = 0;                          //  현재 축이 X 이면 Y, 현재 축이 Y 이면 X
            //XyCoordinate source = new XyCoordinate();
            //XyCoordinate dest = new XyCoordinate();

            //if (nAxis == (int)WaferProbeAlign.nAxis.X)
            //{
            //    if (waferProbeAlign.Stage.Interpolator != null)
            //    {
            //        //AXM.GetActualPosition(nAxis, ref dCurrent);
            //        AXM.GetCommandPosition(nAxis, ref dCurrent);
            //        AXM.GetCommandPosition((int)WaferProbeAlign.nAxis.Y, ref dCurrent_Other);              //  2023. 05. 24.  SCH : Reverse Interpolate 에 축 데이터를 넣을 때는 X, Y 값 모두를 넣어야 한다.
            //        dest.X = dCurrent;
            //        dest.Y = dCurrent_Other;

            //        if ((ret = waferProbeAlign.Stage.Interpolator.ReverseInterpolate(dest, ref source)) != 0)
            //        {
            //            return ret;
            //        }

            //        m_dPos = source.X;
            //    }
            //    else
            //    {
            //        AXM.GetActualPosition(nAxis, ref m_dPos);
            //        //AXM.GetCommandPosition(nAxis, ref m_dPos);
            //    }
            //}
            //else if (nAxis == (int)WaferProbeAlign.nAxis.Y)
            //{
            //    if (waferProbeAlign.Stage.Interpolator != null)
            //    {
            //        //AXM.GetActualPosition(nAxis, ref dCurrent);
            //        AXM.GetCommandPosition(nAxis, ref dCurrent);
            //        AXM.GetCommandPosition((int)WaferProbeAlign.nAxis.X, ref dCurrent_Other);              //  2023. 05. 24.  SCH : Reverse Interpolate 에 축 데이터를 넣을 때는 X, Y 값 모두를 넣어야 한다.
            //        dest.Y = dCurrent;
            //        dest.X = dCurrent_Other;

            //        if ((ret = waferProbeAlign.Stage.Interpolator.ReverseInterpolate(dest, ref source)) != 0)
            //        {
            //            return ret;
            //        }

            //        m_dPos = source.Y;
            //    }
            //    else
            //    {
            //        AXM.GetActualPosition(nAxis, ref m_dPos);
            //        //AXM.GetCommandPosition(nAxis, ref m_dPos);
            //    }
            //}
            //else
            //{
            //    AXM.GetActualPosition(nAxis, ref m_dPos);
            //    //AXM.GetCommandPosition(nAxis, ref m_dPos);
            //}

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //  2D 맵핑을 사용하지 않을 경우
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///
            AXM.GetActualPosition(nAxis, ref m_dPos);                 //  원래 것 (2D Mapping 사용하기 전

            //if (id >= m_lAxisCounts) return 0;
            //return SignalState[(id * 8) + point];

            return m_dPos;
        }

        public bool MC_SetEncPos(int nAxis, double dPos)
        {
            int nRet = 0;

            nRet = AXM.SetActualPosition(nAxis, dPos);

            //if (id >= m_lAxisCounts) return 0;
            //return SignalState[(id * 8) + point];

            if (nRet != 0)
                return false;

            return true;
        }

        public bool MC_SetCmdPos(int nAxis, double dPos)
        {
            int nRet = 0;

            nRet = AXM.SetCommandPosition(nAxis, dPos);

            //if (id >= m_lAxisCounts) return 0;
            //return SignalState[(id * 8) + point];

            if (nRet != 0)
                return false;

            return true;
        }

        public double MC_GetCmdPos(int nAxis)
        {
            int ret = 0;
            double m_dPos = 0.0;
            double dCurrent = 0;

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //  2D 맵핑을 사용할 경우
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///
            //XyCoordinate source = new XyCoordinate();
            //XyCoordinate dest = new XyCoordinate();

            //if (nAxis == (int)WaferProbeAlign.nAxis.X)
            //{
            //    if (waferProbeAlign.Stage.Interpolator != null)
            //    {
            //        AXM.GetCommandPosition(nAxis, ref dCurrent);
            //        dest.X = dCurrent;

            //        if ((ret = waferProbeAlign.Stage.Interpolator.ReverseInterpolate(dest, ref source)) != 0)
            //        {
            //            return ret;
            //        }

            //        m_dPos = source.X;
            //    }
            //    else
            //    {
            //        AXM.GetCommandPosition(nAxis, ref m_dPos);
            //    }
            //}
            //else if (nAxis == (int)WaferProbeAlign.nAxis.Y)
            //{
            //    if (waferProbeAlign.Stage.Interpolator != null)
            //    {
            //        AXM.GetCommandPosition(nAxis, ref dCurrent);
            //        dest.Y = dCurrent;

            //        if ((ret = waferProbeAlign.Stage.Interpolator.ReverseInterpolate(dest, ref source)) != 0)
            //        {
            //            return ret;
            //        }

            //        m_dPos = source.Y;
            //    }
            //    else
            //    {
            //        AXM.GetCommandPosition(nAxis, ref m_dPos);
            //    }
            //}
            //else
            //{
            //    AXM.GetCommandPosition(nAxis, ref m_dPos);
            //}

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //  2D 맵핑을 사용하지 않을 경우
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///
            AXM.GetCommandPosition(nAxis, ref m_dPos);            //  원래 것 (2D Mapping 사용하기 전

            //if (id >= m_lAxisCounts) return 0;
            //return SignalState[(id * 8) + point];

            return m_dPos;
        }

        public bool MC_GetInposition(int nAxis)
        {
            bool m_bRet = false;

            AXM.GetInPositionValue(nAxis, ref m_bRet);

            return m_bRet;
        }

        public bool MC_PosTolerance(int nAxis, double m_dTargetPos)
        {
            bool m_bRet = false;
            double m_dCurPos = 0.0;
            double m_dTol = 0.5;            //  Tolerance : +- 0.02mm

            m_dCurPos = MC_GetEncPos(nAxis);

            if ((m_dCurPos >= (m_dTargetPos - m_dTol)) &&
                (m_dCurPos <= (m_dTargetPos + m_dTol)))
                m_bRet = true;

            //if (id >= m_lAxisCounts) return 0;
            //return SignalState[(id * 8) + point];

            return m_bRet;
        }

        public bool MC_IsAlarm(int nAxis)
        {
            bool m_bRet = false;

            AXM.GetAmpFaultValue(nAxis, ref m_bRet);

            return m_bRet;
        }

        public bool MC_AlarmReset(int nAxis, bool m_bOnOff)
        {
            bool m_bRet = false;

            AXM.AlarmReset(nAxis, m_bOnOff);

            return m_bRet;
        }

        public bool MC_isLimit_Neg( int nAxis)
        {
            bool m_bValue = false;
            AXM.GetNegativeLimitValue(nAxis, ref m_bValue);
            return m_bValue;
        }

        public bool MC_isLimit_Pos(int nAxis)
        {
            bool m_bValue = false;
            AXM.GetPositiveLimitValue(nAxis, ref m_bValue);
            return m_bValue;
        }

        /*
        public bool IsAlarmClear(int nID)
        {
            if (nID >= m_lAxisCounts) return false;
            return Convert.ToBoolean(GetStatus(nID, 1));
        }

        public void ALARM_CLEAR(int iAxis, uint uStatus)
        {
            if (IsInit == false) return;
            CAXM.AxmSignalServoAlarmReset(iAxis, 1);
        }

        public bool IsLimit_PLUS(int nID)
        {
            if (nID >= m_lAxisCounts) return false;
            return Convert.ToBoolean(GetStatus(nID, 2));
        }

        public bool IsLimit_MINUS(int nID)
        {
            if (nID >= m_lAxisCounts) return false;
            return Convert.ToBoolean(GetStatus(nID, 3));
        }

        public bool IsAlarm(int nID)
        {
            if (nID >= m_lAxisCounts) return false;
            return Convert.ToBoolean(GetStatus(nID, 4));
        }

        public bool IsEmg(int nID)
        {
            if (nID >= m_lAxisCounts) return false;
            return Convert.ToBoolean(GetStatus(nID, 5));
        }

        public bool IsHome(int nID)
        {
            bool bRet = false;
            if (nID >= m_lAxisCounts) return bRet;
            uint duState = 0;
            uint duRetCode = CAXM.AxmHomeReadSignal(nID, ref duState);
            if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS) bRet = Convert.ToBoolean(duState);

            return bRet;
        }
        */
        public bool MC_IsBusy(int nID)
        {
            //if (nID >= m_lAxisCounts) return false;
            //return Convert.ToBoolean(GetStatus(nID, 7));

            return true;
        }

        public bool MC_JogMove(int nMAxis, double dVelocity, double dAccel, double dDecel)
        {
            uint duRetCode = 0;

            //if (IsInit == false) return;

            duRetCode = AXM.AxmMoveVel(nMAxis, dVelocity, dAccel, dDecel);
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                return false;

            return true;
        }

        public void MC_JogStop(int nMAxis)
        {
            //if (IsInit == false) return;
            AXM.AxmMoveSStop(nMAxis);
        }

        public void MC_EStop(int nMAxis)
        {
            //if (IsInit == false) return;
            AXM.AxmMoveEStop(nMAxis);
        }

        public uint MC_TriggerSetParam(int nAxis, double dTrigTime, uint uTrigLevel, uint uSelect, uint uInterrupt )
        {
            uint duRetCode = 0;

            duRetCode = AXM.AxmTriggerSetTimeLevel(nAxis, dTrigTime, uTrigLevel, uSelect, uInterrupt);

            return duRetCode;
        }

        public uint MC_TriggerSetBlock(int nAxis, double dStartPos, double dEndPos, double dPeriodPos)
        {
            uint duRetCode = 0;

            duRetCode = AXM.AxmTriggerSetBlock(nAxis, dStartPos, dEndPos, dPeriodPos);

            return duRetCode;
        }

        public bool MC_SetGantryStatus(int nMaster, int nSlave)
        {
            bool m_bRet = false;
            int m_nRet = 0;

            AXM.GantryHomingMethods m_nGantryHomeMethod = AXM.GantryHomingMethods.OnlyMaster ;
            double m_dSlaveOffset = 0.0;
            double m_dSlaveOffsetRange = 0.0;
            bool m_bGantryOn = false;

            m_nRet = AXM.SetGantryEnable(nMaster, nSlave, m_nGantryHomeMethod, m_dSlaveOffset, m_dSlaveOffsetRange);

            if (m_nRet != 0)
            {
                return false;
            }

            return m_bRet;
        }

        public bool MC_GetGantryStatus(int nMAxis)
        {
            bool m_bRet = false;
            int m_nRet = 0;

            GenericUriParserOptions gantryHomeMethod = 0 ;
            double  m_dSlaveOffset = 0.0;
            double  m_dSlaveOffsetRange = 0.0 ;
            bool    m_bGantryOn = false ;

            m_nRet = AXM.GetGantryEnable(nMAxis, ref gantryHomeMethod, ref m_dSlaveOffset, ref m_dSlaveOffsetRange, ref m_bGantryOn);

            if ( m_nRet != 0 )
            {
                return false;
            }

            m_bRet = m_bGantryOn;

            return m_bRet;
        }

        public bool MC_SetHomeMethod(int selAXis)
        {
            uint duRetCode = 0;

            if ( selAXis == 13 )            //   Mounter Z0
            {
                duRetCode = (uint)AXM.SetHomeMethod(selAXis, Directions.Ccw, HomeSignals.ZPhase, ZPhaseMethods.None, 1000, 1.0);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    //Debug.WriteLine(String.Format("AxmHomeSetStart return error[Code:{0:d}]", duRetCode));
                    return false;
                }
            }
            else if ( selAXis == 12 )       //  Mounter T0
            {
                duRetCode = (uint)AXM.SetHomeMethod(selAXis, Directions.Ccw, HomeSignals.ZPhase, ZPhaseMethods.None, 1000, 0.0);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    //Debug.WriteLine(String.Format("AxmHomeSetStart return error[Code:{0:d}]", duRetCode));
                    return false;
                }
            }

            return true;
        }

        public bool MC_SetHomeVelocity(int selAXis)
        {
            uint duRetCode = 0;

            if (selAXis == 13)            //   Mounter Z0
            {
                duRetCode = (uint)AXM.SetHomeVelocity(selAXis, 5.0, 2.0, 1.0, 0.5, 50.0, 50.0);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    //Debug.WriteLine(String.Format("AxmHomeSetStart return error[Code:{0:d}]", duRetCode));
                    return false;
                }
            }
            else if (selAXis == 12)       //  Mounter T0
            {
                duRetCode = (uint)AXM.SetHomeVelocity(selAXis, 50.0, 5.0, 0.5, 0.1, 100.0, 100.0);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    //Debug.WriteLine(String.Format("AxmHomeSetStart return error[Code:{0:d}]", duRetCode));
                    return false;
                }
            }

            return true;
        }

        public bool MC_HomeSearch(int selAxis)
        {
            uint duRetCode = 0;
            //++ 지정한 축에 원점검색을 진행합니다.
            duRetCode = (uint)AXM.SetHomeStart(selAxis);
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                //Debug.WriteLine(String.Format("AxmHomeSetStart return error[Code:{0:d}]", duRetCode));
                return false;
            }
            
            return true;
        }

        public bool MC_GetHoming(int m_nAxis)
        {
            bool m_lIsBusy = true;
            AXT_MOTION_HOME_RESULT uHomeResult = 0;

            //if (!m_bMC_Opened)
            //    return FALSE;

            AXM.GetHomeResult( m_nAxis, ref uHomeResult);

            if (uHomeResult == AXT_MOTION_HOME_RESULT.HOME_SUCCESS)
                m_lIsBusy = false;

            return m_lIsBusy;
        }

        public uint MC_HomeGetResult(int nAxisNo, ref string strResult)
        {
            uint nRet = 0;
            string strTemp = "";
            uint duState = 0;
            AXT_MOTION_HOME_RESULT result = 0;
            nRet = (uint)AXM.GetHomeResult(nAxisNo, ref result);
            strTemp = TranslateHomeResult(duState);
            if (strResult != strTemp)
                strResult = strTemp;
            return nRet;
        }

        public string TranslateHomeResult(uint duHomeResult)
        {
            string m_strResult = "";
            switch (duHomeResult)
            {
                case (uint)AXT_MOTION_HOME_RESULT.HOME_SUCCESS: m_strResult = ("[01H] HOME_SUCCESS"); break;
                case (uint)AXT_MOTION_HOME_RESULT.HOME_SEARCHING: m_strResult = ("([02H] HOME_SEARCHING"); break;
                case (uint)AXT_MOTION_HOME_RESULT.HOME_ERR_GNT_RANGE: m_strResult = ("[10H] HOME_ERR_GNT_RANGE"); break;
                case (uint)AXT_MOTION_HOME_RESULT.HOME_ERR_USER_BREAK: m_strResult = ("[11H] HOME_ERR_USER_BREAK"); break;
                case (uint)AXT_MOTION_HOME_RESULT.HOME_ERR_VELOCITY: m_strResult = ("[12H] HOME_ERR_VELOCITY"); break;
                case (uint)AXT_MOTION_HOME_RESULT.HOME_ERR_AMP_FAULT: m_strResult = ("[13H] HOME_ERR_AMP_FAULT"); break;
                case (uint)AXT_MOTION_HOME_RESULT.HOME_ERR_NEG_LIMIT: m_strResult = ("[14H] HOME_ERR_NEG_LIMIT"); break;
                case (uint)AXT_MOTION_HOME_RESULT.HOME_ERR_POS_LIMIT: m_strResult = ("[15H] HOME_ERR_POS_LIMIT"); break;
                case (uint)AXT_MOTION_HOME_RESULT.HOME_ERR_NOT_DETECT: m_strResult = ("[16H] HOME_ERR_NOT_DETECT"); break;
                case (uint)AXT_MOTION_HOME_RESULT.HOME_ERR_UNKNOWN: m_strResult = ("[FFH] HOME_ERR_UNKNOWN"); break;
            }
            return m_strResult;
        }

        /*
        public uint HomeGetRate(int nAxisNo, ref uint upHomeMainStepNumber, ref uint upHomeStepNumber)
        {
            uint nRet = 0;
            nRet = CAXM.AxmHomeGetRate(nAxisNo, ref upHomeMainStepNumber, ref upHomeStepNumber);

            return nRet;
        }


        //++ 지정한 축의 원점신호의 상태를 확인합니다.
        public uint GetHomeResult(int selAxis)
        {
            uint duState = 0;
            uint duRetCode = CAXM.AxmHomeGetResult(selAxis, ref duState);

            return duState;
        }
        */
        public bool MC_MotorStop(int Axis, double decel)
        {
            int nRetCode = 0;
            nRetCode = AXM.Stop(Axis, decel);
            if (nRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
            //    Debug.WriteLine(String.Format("AxmMoveSStop return error[Code:{0:d}]", duRetCode));
                return false;
            }

            return true;
        }

        // ++ =======================================================================
        // >> rdoAbs_CheckedChanged(...) : "Absolute/Relative"버튼 클릭시 호출되는 핸들러 함수.
        //  - 지정한 축의 구동 좌표계를 설정합니다.
        //  - 설정된 좌표계에 따라 구동 함수들에 전달되는 dPos의 값은 절대위치 또는 
        //    상대 이동량이 됩니다.
        //  - Ex1) 현재 Command위치 100으로 가정
        //     1) AxmMotSetAbsRelMode(0, POS_ABS_MODE)
        //     2) AxmMoveStartPos(0, 200, ...)
        //     -> Command위치 기준 200 위치로 이동, 즉 (+)방향으로 100만큼 이동함
        //  - Ex2) 현재 Command위치 100으로 가정
        //     1) AxmMotSetAbsRelMode(0, POS_REL_MODE)
        //     2) AxmMoveStartPos(0, 200, ...)
        //     -> Command위치 기준 300 위치로 이동, 즉 (+)방향으로 200만큼 이동함
        // --------------------------------------------------------------------------

        /*
        public bool SetAbsRel_Mode(int AxisNo, AXT_MOTION_ABSREL pos)
        {
            uint duRetCode = 0;

            //++ 지정 축의 구동 좌표계를 설정합니다. 
            // dwAbsRelMode : (0)POS_ABS_MODE - 현재 위치와 상관없이 지정한 위치로 절대좌표 이동합니다.
            //                (1)POS_REL_MODE - 현재 위치에서 지정한 양만큼 상대좌표 이동합니다.
            duRetCode = CAXM.AxmMotSetAbsRelMode(AxisNo, Convert.ToUInt32(pos));
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                Debug.WriteLine(String.Format("AxmMotSetAbsRelMode return error[Code:{0:d}]", duRetCode));
                return false;
            }

            return true;

        }
        */

        //public bool AxisMoveStartPos(int Axis, double position, double vel, double accel, double decel)
        public bool MC_MovePosition(int Axis, double position, double vel, double accel, double decel)
        {
            int nRetCode = 0;
            double m_dTarget = 0.0;
            double m_dCurrent_Others = 0.0;                             //  2023. 05. 24.  SCH : XY 축 모션 이동 시 2D Mapping 데이터를 가져오기 위해 구동 축 외 다른 축의 현재 위치값이 있어야 한다.
            XyCoordinate OriginPosition = new XyCoordinate();
            XyCoordinate InterpolatedPosition = new XyCoordinate();

            if (Axis == (int)WaferProbeAlign.nAxis.X)
            {
                if (waferProbeAlign.Stage.Interpolator != null)
                {
                    OriginPosition.X = position;
                    OriginPosition.Y = MC_GetEncPos( (int)WaferProbeAlign.nAxis.Y);

                    waferProbeAlign.Stage.Interpolator.Interpolate(OriginPosition, ref InterpolatedPosition);
                    m_dTarget = InterpolatedPosition.X;
                }
                else
                {
                    m_dTarget = position;
                }
            }
            else if (Axis == (int)WaferProbeAlign.nAxis.Y)
            {
                if (waferProbeAlign.Stage.Interpolator != null)
                {
                    OriginPosition.X = MC_GetEncPos((int)WaferProbeAlign.nAxis.X);
                    OriginPosition.Y = position;

                    waferProbeAlign.Stage.Interpolator.Interpolate(OriginPosition, ref InterpolatedPosition);
                    m_dTarget = InterpolatedPosition.Y;
                }
                else
                {
                    m_dTarget = position;
                }
            }
            else
            {
                m_dTarget = position;
            }

            //nRetCode = AXM.MovePosition(Axis, position, vel, accel, decel);
            nRetCode = AXM.MovePosition(Axis, m_dTarget, vel, accel, decel);

            if (nRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
            //    Debug.WriteLine(String.Format("AxmMoveStartPos return error[Code:{0:d}]", duRetCode));
                return false;
            }

            return true;
        }

        public bool MC_MoveRelPosition(int Axis, double position, double vel, double accel, double decel)
        {
            int nRetCode = 0;
            double dCurPos = 0.0;
            double m_dTarget = 0.0;
            double m_dCurrent_Others = 0.0;                             //  2023. 05. 24.  SCH : XY 축 모션 이동 시 2D Mapping 데이터를 가져오기 위해 구동 축 외 다른 축의 현재 위치값이 있어야 한다.
            XyCoordinate OriginPosition = new XyCoordinate();
            XyCoordinate InterpolatedPosition = new XyCoordinate();

            dCurPos = MC_GetEncPos(Axis);
            m_dTarget = dCurPos + position;

            if (Axis == (int)WaferProbeAlign.nAxis.X)
            {
                if (waferProbeAlign.Stage.Interpolator != null)
                {
                    OriginPosition.X = m_dTarget;
                    OriginPosition.Y = MC_GetEncPos((int)WaferProbeAlign.nAxis.Y);

                    waferProbeAlign.Stage.Interpolator.Interpolate(OriginPosition, ref InterpolatedPosition);
                    m_dTarget = InterpolatedPosition.X;
                }
            }
            else if (Axis == (int)WaferProbeAlign.nAxis.Y)
            {
                if (waferProbeAlign.Stage.Interpolator != null)
                {
                    OriginPosition.X = MC_GetEncPos((int)WaferProbeAlign.nAxis.X);
                    OriginPosition.Y = m_dTarget;

                    waferProbeAlign.Stage.Interpolator.Interpolate(OriginPosition, ref InterpolatedPosition);
                    m_dTarget = InterpolatedPosition.Y;
                }
            }

            //nRetCode = AXM.MovePosition(Axis, dCurPos + position, vel, accel, decel);
            nRetCode = AXM.MovePosition(Axis, m_dTarget, vel, accel, decel);

            if (nRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                //    Debug.WriteLine(String.Format("AxmMoveStartPos return error[Code:{0:d}]", duRetCode));
                return false;
            }

            return true;
        }

        public int MC_SetGroupAxis(int nCoord, int[] selAxis)
        {
            AXM.ClearPath(nCoord);                  //  연속 보간 동작을 쓸 지도 모르니 일단 clear 해두자.
            AXM.SetPathAxisMap(nCoord, selAxis);
            //++ 지정한 좌표계에 구동모드을 설정합니다.(절대구동/상대구동)
            return AXM.SetPathAbsRelMode(nCoord, 0);            //  mode 0 : ABS            1 : REL
        }

        public bool MC_LineMovePosition(int nCoord, int[] selAxis, double[] selPos, double vel, double acc, double decel)
        {
            int nRetCode = 0;

            MC_SetGroupAxis( nCoord, selAxis);

            nRetCode = AXM.MoveLine(nCoord, selAxis, selPos, vel, acc, decel);
            if (nRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                //    Debug.WriteLine(String.Format("AxmMoveStartPos return error[Code:{0:d}]", duRetCode));
                return false;
            }

            return true;
        }

        #endregion

        #region Motion Jerk

        public bool MC_SetAccJerk(int nMAxis, double m_dAccJerk)
        {
            int m_nRet = 0;

            m_nRet = AXM.SetAccelerationJerk(nMAxis, m_dAccJerk);

            if (m_nRet != 0)
            {
                return false;
            }

            return true;
        }

        public bool MC_SetDecJerk(int nMAxis, double m_dAccJerk)
        {
            int m_nRet = 0;

            m_nRet = AXM.SetDecelerationJerk(nMAxis, m_dAccJerk);

            if (m_nRet != 0)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region TASK

        /*
        private void MakeTask()
        {
            ReadMotionSignalStateToken = new CancellationTokenSource();
            ReadMotionSignalStateTask = new Task(ReadStateSignalWork, ReadMotionSignalStateToken.Token);
        }

        void ReadStateSignalWork()
        {
            Thread.CurrentThread.Name = $"ReadStateSignalWork";
            if (IsInit == false) return;
            while (ReadMotionSignalStateToken.Token.IsCancellationRequested == false)
            {
                uint duRetCode = 0;
                uint duState2 = 0;
                int iCheck = 0;
                for (int n = 0; n < m_lAxisCounts; ++n)
                {
                    for (uint uBitNo = 0; uBitNo < 2; uBitNo++)
                    {
                        //++ 범용입력 신호의(Bit00-Bit04) 상태를 확인합니다.
                        //duRetCode = CAXM.AxmSignalReadInputBit(n, (int)uBitNo, ref duState1);
                        //if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS) SignalInput[uBitNo].Checked = Convert.ToBoolean(duState1);
                        //++ 범용출력 신호의(Bit00-Bit04) 상태를 확인합니다.
                        duRetCode = CAXM.AxmSignalReadOutputBit(n, (int)uBitNo, ref duState2);
                        if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                            SignalState[(int)uBitNo + (n * 8)] = duState2;//0 : servo on 1: 
                        else
                            SignalState[(int)uBitNo + (n * 8)] = 0;
                    }

                    //++ 지정 축의 Mechanical Signal Data(현재 기계적인 신호상태)를 확인합니다.
                    // ※ [CAUTION] 각 제품별로 하드웨어적인 신호가 다르기 때문에 매뉴얼 및 AXHS.xxx파일을 참고하십시요.
                    duRetCode = CAXM.AxmStatusReadMechanical(n, ref duState2);

                    {
                        for (int iIndex = 0; iIndex < 5; iIndex++)
                        {
                            if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                            {
                                switch (iIndex)
                                {
                                    case 0:
                                        iCheck = ((int)duState2 >> iDigitMech[0] & 0x1);
                                        break;
                                    case 1:
                                        iCheck = ((int)duState2 >> iDigitMech[1] & 0x1);
                                        break;
                                    case 2:
                                        iCheck = ((int)duState2 >> iDigitMech[2] & 0x1);
                                        break;
                                    case 3:
                                        iCheck = ((int)duState2 >> iDigitMech[4] & 0x1);
                                        break;
                                    case 4:
                                        iCheck = ((int)duState2 >> iDigitMech[5] & 0x1);
                                        break;
                                }
                            }
                            else
                            {
                                switch (iIndex)
                                {
                                    case 0:
                                        iCheck = 0;
                                        break;
                                    case 1:
                                        iCheck = 0;
                                        break;
                                    case 2:
                                        iCheck = 1;
                                        break;
                                    case 3:
                                        iCheck = 0;
                                        break;
                                    case 4:
                                        iCheck = 1;
                                        break;
                                }
                            }


                            SignalState[iIndex + 2 + (n * 8)] = (uint)iCheck;
                        }
                    }

                    duRetCode = CAXM.AxmStatusReadMotion(n, ref duState2);
                    if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                    {
                        iCheck = ((int)duState2 >> iDigitDrive[0] & 0x1);
                        SignalState[(n * 8) + 7] = (uint)iCheck;
                    }

                    double dCmdPos = 0.0, dCmdVel = 0.0, dActPos = 0.0;

                    //++ 지정한 축의 지령(Command)위치를 반환합니다.
                    CAXM.AxmStatusGetCmdPos(n, ref dCmdPos);
                    //++ 지정한 축의 실제(Feedback)위치를 반환합니다.
                    CAXM.AxmStatusGetActPos(n, ref dActPos);
                    //++ 지정한 축의 구동 속도를 반환합니다.
                    CAXM.AxmStatusReadVel(n, ref dCmdVel);

                    CMDPOS[n] = dCmdPos;
                    ACTPOS[n] = dActPos;
                    CMDVEL[n] = dCmdVel;


                }
                Thread.Sleep(100);
                //Task.Delay(100);
            }
        }

        public void StartMotion()
        {
            if (IsInit)
                ReadMotionSignalStateTask.Start();
        }

        public void StopMotion()
        {
            ReadMotionSignalStateToken.Cancel();
            ReadMotionSignalStateTask.Wait();

            ReadMotionSignalStateTask.Dispose();
            ReadMotionSignalStateToken.Dispose();

            ReadMotionSignalStateTask = null;
            ReadMotionSignalStateToken = null;

            MakeTask();
        }

        */

        #endregion

    }
}
