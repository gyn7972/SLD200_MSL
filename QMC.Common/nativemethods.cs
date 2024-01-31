using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

#pragma warning disable CS1591

namespace QMC.Core
{
    /// <summary>
    /// P/Invoke 용 네이티브 코드 집합
    /// 2020.1. hcchoi
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    public static class NativeMethods
    {
        /// <summary>
        /// 콘솔창 핸들러
        /// </summary>
        /// <param name="HandlerRoutine"></param>
        /// <param name="Add"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate HandlerRoutine, bool Add);
        /// <summary>
        /// 콘솔창 핸들러 델리게이트
        /// </summary>
        /// <param name="CtrlType"></param>
        /// <returns></returns>
        public delegate Boolean ConsoleCtrlDelegate(CtrlTypes CtrlType);
        /// <summary>
        /// 콘솔창 핸들러에 사용하는 이벤트 종류
        /// </summary>
        public enum CtrlTypes : uint
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        }

        /// <summary>
        /// 표준 핸들 얻기
        /// </summary>
        /// <param name="nStdHandle"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetStdHandle(int nStdHandle);

        /// <summary>
        /// 콘솔 모드 얻기
        /// </summary>
        /// <param name="hConsoleHandle"></param>
        /// <param name="lpMode"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        /// <summary>
        /// 콘솔 모드 열거
        /// </summary>
        [Flags]
        public enum ConsoleModes : uint
        {
            ENABLE_PROCESSED_INPUT = 0x0001,
            ENABLE_LINE_INPUT = 0x0002,
            ENABLE_ECHO_INPUT = 0x0004,
            ENABLE_WINDOW_INPUT = 0x0008,
            ENABLE_MOUSE_INPUT = 0x0010,
            ENABLE_INSERT_MODE = 0x0020,
            ENABLE_QUICK_EDIT_MODE = 0x0040,
            ENABLE_EXTENDED_FLAGS = 0x0080,
            ENABLE_AUTO_POSITION = 0x0100,
            ENABLE_PROCESSED_OUTPUT = 0x0001,
            ENABLE_WRAP_AT_EOL_OUTPUT = 0x0002,
            ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004,
            DISABLE_NEWLINE_AUTO_RETURN = 0x0008,
            ENABLE_LVB_GRID_WORLDWIDE = 0x0010
        }

        /// <summary>
        /// 콘솔 모드 설정
        /// </summary>
        /// <param name="hConsoleHandle"></param>
        /// <param name="dwMode"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        /// <summary>
        /// DLL 파일 접근 경로설정
        /// </summary>
        /// <param name="lpPathName"></param>
        /// <returns></returns>

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetDllDirectory(string lpPathName);

        /// <summary>
        /// Ini 파일에서 읽기
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="def"></param>
        /// <param name="retVal"></param>
        /// <param name="size"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        /// <summary>
        /// Ini 파일에 쓰기
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern long WritePrivateProfileString(string Section, string key, string val, string filePath);
    }
}
