using System;
using System.IO;


namespace QMC.Common
{
    public static class Def
    {
        #region << file path >>
        public static string ParamPath => "D:\\CWA-150SA_Onsemi_Parameter";
        public static string RecipePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "recipes");
        public static string ConfigPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config");
        public static string LogPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
        public static string LotLogPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "lotfiles");
        public static string MotorConfigPath => Path.Combine(ConfigPath, "MotorPara.mot");

        public static string GlassCompenFile => "gridcompen.json";
        public static string GlassStageCompenFile => "gridStagecompen.json";
        public static string DmFile => "dm.json";
        public static string LotFile => "lotstate.json";
        //public static string LaserPwrRangeFile => "laserpwr.json";

        public static string CurDate => DateTime.Now.ToString("yy-MM-dd-HH-mm-ss");
        public static string CurTime => DateTime.Now.ToString("HH-mm-ss");

        public static bool IsSimulModeEnable => File.Exists(Path.Combine(Def.ConfigPath, $"simulation.ini"));


        /// <summary>
        /// Etamax 검사 데이터를 가져오기 위한 파라미터
        /// </summary>
        public static string NetworkDriveInfo { get { return Path.Combine(ParamPath, "NetworkDrive.ini"); } }
        /// <summary>
        /// Etamax 검사 데이터 파일을 가져오는 임시 위치 경로
        /// </summary>
        public static string WaferChipDataRootPath { get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WaferChipData"); } }
        /// <summary>
        /// Etamax 검사 데이터 파일을 저장하기 위한 경로
        /// </summary>
        public static string WaferChipDataStorePath { get { return Path.Combine(WaferChipDataRootPath, "Store"); } }
        #endregion
    }

    public struct RowColCnt 
    {
        public int Row { get; set; }
        public int Col { get; set; }

        public RowColCnt(int r, int c)
        {
            Row = r;
            Col = c;
        }
        public override string ToString()
        {
            return $"{Row:F3}, {Col:F3}";
        }
    }

    public enum MachineStatus
    {
        Idle = 0,
        Error = 1,
        Warning = 2,
        Run = 3,

    }

    //public enum MachineStateEnum
    //{
    //    AutoRun = 1,
    //    Stop = 2,
    //    Error = 3,
    //    Warning = 4,
    //    Cycle = 5,
    //}
}
