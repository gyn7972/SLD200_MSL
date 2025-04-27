using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    public static class Log
    {
        public static void Write(string strClass, string strSource, string strMessage)
        {
            LogManager.Instance.Write(LogLevel.Normal, strClass, strSource, strMessage);
        }
        public static void Write(string strClass, string strOperator, string strSource, string strMessage)
        {
            LogManager.Instance.Write(LogLevel.Normal, strClass, strOperator, strSource, strMessage);
        }
        public static void Write(LogLevel level, string strClass, string strSource, string strMessage)
        {
            LogManager.Instance.Write(level, strClass, strSource, strMessage);
        }
        public static void Write(string strClass, string strMessage)
        {
            LogManager.Instance.Write(LogLevel.Normal, strClass, strMessage);
        }
        public static void Write(Part part, string strMessage)
        {
            LogManager.Instance.Write(LogLevel.Normal, part.Name, part.Name, strMessage);
        }
        public static void Write(LogLevel level, Part part, string strMessage)
        {
            LogManager.Instance.Write(level, part.Name, part.Name, strMessage);
        }
        public static void Write(LogLevel level, string strClass, Part part, string strMessage)
        {
            LogManager.Instance.Write(level, strClass, part.Name, strMessage);
        }
        public static void Write(Exception ex)
        {
            LogManager.Instance.Write(LogLevel.Highest, "ProgramExeption", ex.Source);
            LogManager.Instance.Write(LogLevel.Highest, "ProgramExeption", ex.Message);
            LogManager.Instance.Write(LogLevel.Highest, "ProgramExeption", ex.StackTrace);
            
        }
        public static void WriteWorkLog(string strMessage)
        {
            LogManager.Instance.WriteWorkLog(strMessage);
        }
    }
}
