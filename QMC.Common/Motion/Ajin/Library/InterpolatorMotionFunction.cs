using QMC.Common;
using QMC.Common.Modules;
using QMC.Core;
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
            string iniPath = Path.Combine(ConfigManager.GetConfigPath(), "SoftLimit(Do not delete or modify).ini");
            InterpolatorMotionFunction.LoadSoftLimitsFromIni(iniPath, this);
        }

        // 축별 Lock 오브젝트 초기화
        private static readonly object[] _axisLocks = new object[32];
        static InterpolatorMotionFunction()
        {
            for (int i = 0; i < _axisLocks.Length; i++)
                _axisLocks[i] = new object();
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
        public override double MC_GetEncPos(int nAxis)
        {

            int ret = 0;
            double dPos = 0.0;
            double dCurrentX = 0;

            double dCurrentY = 0;
            if (Equipment.MapDataStatus_Activate && (workStage.Stage.Interpolator != null))

            {
                AXM.GetActualPosition(nAxis, ref dPos);
                string str = this.GetType().ToString();
                XyCoordinate source = new XyCoordinate();
                XyCoordinate dest = new XyCoordinate();

                if (nAxis == (int)WorkStage.nAxis.X)
                {
                    if (workStage.Stage.Interpolator != null)
                    {

                        //originPosition.X = MC_GetEncPos((int)WorkStage.nAxis.X);
                        //originPosition.Y = MC_GetEncPos((int)WorkStage.nAxis.Y);
                        AXM.GetActualPosition((int)WorkStage.nAxis.X, ref dCurrentX);
                        AXM.GetActualPosition((int)WorkStage.nAxis.Y, ref dCurrentY);

                        dest.X = dCurrentX;
                        dest.Y = dCurrentY;
                        if ((ret = workStage.Stage.Interpolator.ReverseInterpolate(dest, ref source)) != 0)
                        {
                            return ret;
                        }

                        dPos = source.X;
                    }
                    else
                    {
                        AXM.GetActualPosition(nAxis, ref dPos);
                    }
                }
                else if (nAxis == (int)WorkStage.nAxis.Y)
                {
                    if (workStage.Stage.Interpolator != null)
                    {
                        AXM.GetActualPosition((int)WorkStage.nAxis.X, ref dCurrentX);
                        AXM.GetActualPosition((int)WorkStage.nAxis.Y, ref dCurrentY);

                        dest.X = dCurrentX;
                        dest.Y = dCurrentY;
                        if ((ret = workStage.Stage.Interpolator.ReverseInterpolate(dest, ref source)) != 0)
                        {
                            return ret;
                        }

                        dPos = source.Y;
                    }
                    else
                    {
                        AXM.GetActualPosition(nAxis, ref dPos);
                    }
                }
                else
                {
                    AXM.GetActualPosition(nAxis, ref dPos);
                }
            }
            else
            {
                return base.MC_GetEncPos(nAxis);
            }

            return dPos;
        }
        public  override bool MC_MovePosition(int Axis, double position, double vel, double accel, double decel)
        {

            lock (_axisLocks[Axis])
            {
                try
                {
                    string msg;
                    if (!IsWithinSoftLimit(Axis, position, out msg))
                    {
                        Log.Write("SLD-200", "MC_MovePosition", msg);
                        //Equipment.AlarmPost(Equipment.AlarmKey.eSoftLimitExceeded);
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                }

                int elapsed = 0;
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
            }
            
            return false;            
        }
        public override bool MC_MoveRelPosition(int Axis, double position, double vel, double accel, double decel)
        {
            lock (_axisLocks[Axis])
            {
                try
                {
                    double current = MC_GetCmdPos(Axis);
                    double target = current + position;

                    string msg;
                    if (!IsWithinSoftLimit(Axis, target, out msg))
                    {
                        Log.Write("SLD-200", "MC_MoveRelPosition", msg);
                        //Equipment.AlarmPost(Equipment.AlarmKey.eSoftLimitExceeded);
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                }

                int elapsed = 0;
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
            }

            return false;
        }
        public bool MovePosition(XyCoordinate destPosition, double vel, double accel, double decel)
        {
            bool bRet = false;
            string m_strTemp = "";

            try
            {
                string msgX = "";
                string msgY = "";
                if (!IsWithinSoftLimit((int)WorkStage.nAxis.X, destPosition.X, out msgX) ||
                    !IsWithinSoftLimit((int)WorkStage.nAxis.Y, destPosition.Y, out msgY))
                {
                    if (!string.IsNullOrEmpty(msgX)) Log.Write("SLD-200", "Motion", msgX);
                    if (!string.IsNullOrEmpty(msgY)) Log.Write("SLD-200", "Motion", msgY);
                    //Equipment.AlarmPost(Equipment.AlarmKey.eSoftLimitExceeded);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }

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


        public class SoftLimitRange
        {
            public double Min { get; set; }
            public double Max { get; set; }

            public SoftLimitRange(double min, double max)
            {
                Min = min;
                Max = max;
            }
        }
        private readonly Dictionary<int, SoftLimitRange> _softLimits = new Dictionary<int, SoftLimitRange>();


        public class AxisInfo
        {
            public string Name { get; set; }        // 예: "Z0"
            public int AxisNumber { get; set; }     // 예: 6
            public string UnitName { get; set; }    // 예: "Loader"
        }

        public static class AxisMapProvider
        {
            public static List<AxisInfo> GetAllAxes()
            {
                var list = new List<AxisInfo>();

                // WorkStage 축
                foreach (WorkStage.nAxis axis in Enum.GetValues(typeof(WorkStage.nAxis)))
                {
                    list.Add(new AxisInfo
                    {
                        UnitName = "WorkStage",
                        Name = axis.ToString(),
                        AxisNumber = (int)axis
                    });
                }

                // Loader 축
                foreach (Loader.nAxis axis in Enum.GetValues(typeof(Loader.nAxis)))
                {
                    list.Add(new AxisInfo
                    {
                        UnitName = "Loader",
                        Name = axis.ToString(),
                        AxisNumber = (int)axis
                    });
                }

                // Unloader 축
                foreach (Unloader.nAxis axis in Enum.GetValues(typeof(Unloader.nAxis)))
                {
                    list.Add(new AxisInfo
                    {
                        UnitName = "Unloader",
                        Name = axis.ToString(),
                        AxisNumber = (int)axis
                    });
                }

                return list;
            }
        }

        public void SetSoftLimit(int axis, double min, double max)
        {
            _softLimits[axis] = new SoftLimitRange(min, max);
        }


        public static bool LoadSoftLimitsFromIni(string iniPath, InterpolatorMotionFunction motionFunc)
        {
            bool bRet = true;
            StringBuilder temp = new StringBuilder(255);
            string section = "SoftLimit";

            // 1. 폴더 확인 및 생성
            string folder = Path.GetDirectoryName(iniPath);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            // 2. 파일이 없으면 기본값 생성
            if (!File.Exists(iniPath))
            {
                var axisList = InterpolatorMotionFunction.AxisMapProvider.GetAllAxes();
                foreach (var axis in axisList)
                {
                    string keyMin = $"{axis.UnitName}_{axis.Name}_Min";
                    string keyMax = $"{axis.UnitName}_{axis.Name}_Max";

                    double minValue = -100.0;
                    double maxValue = 2000.0;

                    motionFunc.SetSoftLimit(axis.AxisNumber, minValue, maxValue);

                    NativeMethods.WritePrivateProfileString(section, keyMin, minValue.ToString("F3"), iniPath);
                    NativeMethods.WritePrivateProfileString(section, keyMax, maxValue.ToString("F3"), iniPath);

                    //Log.Write("SLD-200", "SoftLimit", $"[기본값 적용] {axis.UnitName}_{axis.Name} → Min={minValue:F3}, Max={maxValue:F3}");
                }

                //Log.Write("SLD-200", "SoftLimit", $"SoftLimit 파일이 없어 기본값으로 생성됨: {iniPath}");
                return true;
            }

            // 3. 기존 ini 파일에서 로드
            var loadedAxes = InterpolatorMotionFunction.AxisMapProvider.GetAllAxes();

            foreach (var axis in loadedAxes)
            {
                string keyMin = $"{axis.UnitName}_{axis.Name}_Min";
                string keyMax = $"{axis.UnitName}_{axis.Name}_Max";

                NativeMethods.GetPrivateProfileString(section, keyMin, "", temp, 255, iniPath);
                if (!double.TryParse(temp.ToString(), out double minValue))
                    continue;

                NativeMethods.GetPrivateProfileString(section, keyMax, "", temp, 255, iniPath);
                if (!double.TryParse(temp.ToString(), out double maxValue))
                    continue;

                motionFunc.SetSoftLimit(axis.AxisNumber, minValue, maxValue);
                //Log.Write("SLD-200", "SoftLimit", $"[{axis.UnitName}] {axis.Name}({axis.AxisNumber}) → Min={minValue:F3}, Max={maxValue:F3}");
            }

            return bRet;
        }


        private bool IsWithinSoftLimit(int axis, double targetPos, out string message)
        {
            message = "";
            if (_softLimits.TryGetValue(axis, out SoftLimitRange limits))
            {
                if (targetPos < limits.Min || targetPos > limits.Max)
                {
                    message = $"[SoftLimit] Axis({axis}): {targetPos:F3} → 제한 범위 초과 ({limits.Min:F3} ~ {limits.Max:F3})";
                    return false;
                }
            }
            return true;
        }
    }
}
