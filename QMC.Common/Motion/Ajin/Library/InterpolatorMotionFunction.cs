using QMC.Common;
using QMC.Common.Modules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading;
using System.Windows;

namespace QMC.Common.Motion.Ajin.Motions
{
    public class InterpolatorMotionFunction : MotionFunction
    {
        public InterpolatorMotionFunction() : base()
        {
        }


        public override double MC_GetCmdPos(int nAxis)
        {
            //if (workStage.Config.ParamConfig.MapFileApply_WhenPgmStart &&(workStage.Stage.Interpolator != null))
            if (Equipment.MapDataStatus_Activate &&(workStage.Stage.Interpolator != null))
            {
                if (nAxis == (int)WorkStage.nAxis.X || nAxis == (int)WorkStage.nAxis.Y)
                {
                    double dCmdPosX = base.MC_GetCmdPos((int)WorkStage.nAxis.X);
                    double dCmdPosY = base.MC_GetCmdPos((int)WorkStage.nAxis.Y);
                    XyCoordinate originPosition = new XyCoordinate(dCmdPosX, dCmdPosY);
                    XyCoordinate reverseInterpolatedCoord = new XyCoordinate();
                    workStage.Stage.Interpolator.ReverseInterpolate(originPosition, ref reverseInterpolatedCoord);
                    if (nAxis == (int)WorkStage.nAxis.X)
                    {
                        return reverseInterpolatedCoord.X;
                    }
                    else if (nAxis == (int)WorkStage.nAxis.Y)
                    {
                        return reverseInterpolatedCoord.Y;
                    }
                }
            }
            return base.MC_GetCmdPos(nAxis);
        }
        public  override bool MC_MovePosition(int Axis, double position, double vel, double accel, double decel)
        {
            //if (workStage.Config.ParamConfig.MapFileApply_WhenPgmStart && (workStage.Stage.Interpolator != null))
            if (Equipment.MapDataStatus_Activate && (workStage.Stage.Interpolator != null))
            {
                if (Axis == (int)WorkStage.nAxis.X || Axis == (int)WorkStage.nAxis.Y)
                {
                    XyCoordinate originPosition = new XyCoordinate();
                    originPosition.X = MC_GetCmdPos((int)WorkStage.nAxis.X);
                    originPosition.Y = MC_GetCmdPos((int)WorkStage.nAxis.Y);
                    XyCoordinate destPosition = new XyCoordinate();

                    if (Axis == (int)WorkStage.nAxis.X)
                    {
                        originPosition.X = position;
                    }
                    else if (Axis == (int)WorkStage.nAxis.Y)
                    {
                        originPosition.Y = position;
                    }
                    workStage.Stage.Interpolator.Interpolate(originPosition, ref destPosition);

                    if (Axis == (int)WorkStage.nAxis.X)
                    {
                        bool bRet = base.MC_MovePosition((int)WorkStage.nAxis.X, destPosition.X, vel, accel, decel);
                        bRet &= base.MC_MovePosition((int)WorkStage.nAxis.Y, destPosition.Y, vel, accel, decel);
                        while (!base.MC_GetInposition((int)WorkStage.nAxis.Y))
                        {
                            Thread.Sleep(1);
                        }
                        return bRet;
                    }
                    else if (Axis == (int)WorkStage.nAxis.Y)
                    {
                        bool bRet = base.MC_MovePosition((int)WorkStage.nAxis.X, destPosition.X, vel, accel, decel);
                        bRet &= base.MC_MovePosition((int)WorkStage.nAxis.Y, destPosition.Y, vel, accel, decel);
                        while (!base.MC_GetInposition((int)WorkStage.nAxis.X))
                        {
                            Thread.Sleep(1);
                        }
                        return bRet;
                    }
                }
                else
                {
                    return base.MC_MovePosition(Axis, position, vel, accel, decel);
                }
            }
            else
            {
                return base.MC_MovePosition(Axis, position, vel, accel, decel);
            }
            return false;
            
        }

        public override bool MC_MoveRelPosition(int Axis, double position, double vel, double accel, double decel)
        {
            //if (workStage.Config.ParamConfig.MapFileApply_WhenPgmStart && (workStage.Stage.Interpolator != null))
            if (Equipment.MapDataStatus_Activate && (workStage.Stage.Interpolator != null))
            {
                if (Axis == (int)WorkStage.nAxis.X || Axis == (int)WorkStage.nAxis.Y)
                {
                    XyCoordinate originPosition = new XyCoordinate();
                    originPosition.X = MC_GetCmdPos((int)WorkStage.nAxis.X);
                    originPosition.Y = MC_GetCmdPos((int)WorkStage.nAxis.Y);
                    XyCoordinate destPosition = new XyCoordinate();
                    if (Axis == (int)WorkStage.nAxis.X)
                    {
                        originPosition.X += position;
                    }
                    else if (Axis == (int)WorkStage.nAxis.Y)
                    {
                        originPosition.Y += position;
                    }
                    workStage.Stage.Interpolator.Interpolate(originPosition, ref destPosition);
                    if (Axis == (int)WorkStage.nAxis.X)
                    {
                        bool bRet = base.MC_MovePosition((int)WorkStage.nAxis.X, destPosition.X, vel, accel, decel);
                        bRet &= base.MC_MovePosition((int)WorkStage.nAxis.Y, destPosition.Y, vel, accel, decel);
                        while (!base.MC_GetInposition((int)WorkStage.nAxis.Y))
                        {
                            Thread.Sleep(1);
                        }

                        while (!base.MC_GetInposition((int)WorkStage.nAxis.X))
                        {
                            Thread.Sleep(1);
                        }
                        return bRet;
                    }
                    else if (Axis == (int)WorkStage.nAxis.Y)
                    {
                        bool bRet = base.MC_MovePosition((int)WorkStage.nAxis.X, destPosition.X, vel, accel, decel);
                        bRet &= base.MC_MovePosition((int)WorkStage.nAxis.Y, destPosition.Y, vel, accel, decel);
                        while (!base.MC_GetInposition((int)WorkStage.nAxis.Y))
                        {
                            Thread.Sleep(1);
                        }

                        while (!base.MC_GetInposition((int)WorkStage.nAxis.X))
                        {
                            Thread.Sleep(1);
                        }

                        return bRet;
                    }
                }
                else
                {
                    return base.MC_MoveRelPosition(Axis, position, vel, accel, decel);
                }
            }
            else
            {
                return base.MC_MoveRelPosition(Axis, position, vel, accel, decel);
            }

            return false;
        }
        public bool MovePosition(XyCoordinate destPosition, double vel, double accel, double decel)
        {
            bool bRet = false;
            string m_strTemp = "";

            //if (workStage.Config.ParamConfig.MapFileApply_WhenPgmStart && (workStage.Stage.Interpolator != null))
            if (Equipment.MapDataStatus_Activate && (workStage.Stage.Interpolator != null))
            {
                //m_strTemp = string.Format("Interpolator Activated {0}", workStage.Stage.Interpolator.ToString());
                //Log.Write("SLD-200", "MapData", m_strTemp);

                XyCoordinate destPositionInterpolated = new XyCoordinate();

                workStage.Stage.Interpolator.Interpolate(destPosition, ref destPositionInterpolated);
                bRet = base.MC_MovePosition((int)WorkStage.nAxis.X, destPositionInterpolated.X, vel, accel, decel);
                bRet &= base.MC_MovePosition((int)WorkStage.nAxis.Y, destPositionInterpolated.Y, vel, accel, decel);
            }
            else
            {
                //m_strTemp = string.Format("Interpolator Deactivated");
                //Log.Write("SLD-200", "MapData", m_strTemp);

                base.MC_MovePosition((int)WorkStage.nAxis.X, destPosition.X, vel, accel, decel);
                base.MC_MovePosition((int)WorkStage.nAxis.Y, destPosition.Y, vel, accel, decel);
            }
            return bRet;

        }
    }
}
