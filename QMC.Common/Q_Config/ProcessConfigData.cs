using QMC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Q_Config
{
    public class ProcessConfigData
    {
        public int nSpeedAxisX { get; set; } = 100; // mm/s
        public int nSpeedAxisY { get; set; } = 100; // mm/s
        public int nAccelAxisX { get; set; } = 500; // mm/s^2
        public int nAccelAxisY { get; set; } = 500; // mm/s^2


        public ProcessConfigData()
        {
            // 기본값 설정
            nSpeedAxisX = 100; // mm/s
            nSpeedAxisY = 100; // mm/s
            nAccelAxisX = 500; // mm/s^2
            nAccelAxisY = 500; // mm/s^2
        }

        public bool SaveToIni(string path)
        {
            bool bRet = false;

            try
            {
                NativeMethods.WritePrivateProfileString("ProcessSpeed", "SpeedAxisX", nSpeedAxisX.ToString(), path);
                NativeMethods.WritePrivateProfileString("ProcessSpeed", "SpeedAxisY", nSpeedAxisY.ToString(), path);
                NativeMethods.WritePrivateProfileString("ProcessAccel", "AccelAxisX", nAccelAxisX.ToString(), path);
                NativeMethods.WritePrivateProfileString("ProcessAccel", "AccelAxisY", nAccelAxisY.ToString(), path);
                bRet = true;
            }
            catch (Exception ex)
            {
                bRet = false;
                Log.Write(ex);
            }

            return bRet;
        }

        public bool LoadFromIni(string path)
        {
            bool bRet = false;
            StringBuilder sb = new StringBuilder(255);
            try
            {
                string strValue = "";
                NativeMethods.GetPrivateProfileString("ProcessSpeed", "SpeedAxisX", "100", sb, sb.Capacity, path);
                nSpeedAxisX = int.Parse(strValue);
                NativeMethods.GetPrivateProfileString("ProcessSpeed", "SpeedAxisY", "100", sb, sb.Capacity, path);
                nSpeedAxisY = int.Parse(strValue);
                NativeMethods.GetPrivateProfileString("ProcessAccel", "AccelAxisX", "500", sb, sb.Capacity, path);
                nAccelAxisX = int.Parse(strValue);
                NativeMethods.GetPrivateProfileString("ProcessAccel", "AccelAxisY", "500", sb, sb.Capacity, path);
                nAccelAxisY = int.Parse(strValue);

                bRet = true;
            }
            catch (Exception ex)
            {
                bRet = false;
                Log.Write(ex);
            }
            return bRet;
        }
    }
}
