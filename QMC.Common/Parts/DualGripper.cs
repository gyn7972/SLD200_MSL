using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QMC.Common.Parts
{
    public class DualGripper : Gripper
    {
        public new enum DioPointKey
        {
            Input_Vacuum_On,
            Output_Eject_On,
            Output_Vacuum1_On,
            Output_Vacuum2_On,
        }

        public DualGripper(string strName) : base(strName)
        {
        }

        #region Part
        public override int Create()
        {
            int ret = base.Create();

            if (m_dicDioPoints == null)
                m_dicDioPoints = new Dictionary<string, DioPoint>();

            m_dicDioPoints.Clear();
            foreach (DioPointKey key in Enum.GetValues(typeof(DioPointKey)))
            {
                m_dicDioPoints.Add(key.ToString(), null);
            }

            return ret;
        }

        public override void Close()
        {
            base.Close();
        }
        #endregion

        public override bool IsHold()
        {
            bool bRet = false;

            DioValue on = m_dicDioPoints[DioPointKey.Input_Vacuum_On.ToString()].GetValue();
            
            if (on == DioValue.On)
            {
                bRet = true;
            }

            return bRet;
        }

        public override bool IsRelease()
        {
            bool bRet = false;

            DioValue on = m_dicDioPoints[DioPointKey.Input_Vacuum_On.ToString()].GetValue();

            if (on == DioValue.Off)
            {
                bRet = true;
            }

            return bRet;
        }
        public override int Hold()
        {
            int ret = 0;
            if ((ret = m_dicDioPoints[DioPointKey.Output_Eject_On.ToString()].Write(DioValue.On)) != 0) return ret;
            if ((ret = m_dicDioPoints[DioPointKey.Output_Vacuum1_On.ToString()].Write(DioValue.On)) != 0) return ret;
            if ((ret = m_dicDioPoints[DioPointKey.Output_Vacuum2_On.ToString()].Write(DioValue.On)) != 0) return ret;

            DateTime startTime = DateTime.Now;
            while (true)
            {
                if (IsHold())
                    break;
                TimeSpan processTime = DateTime.Now - startTime;
                if (ResponseTimeout != 0 && processTime.TotalSeconds >= ResponseTimeout)
                {
                    //Timeout
                    ret = -1;
                    break;
                }
                Thread.Sleep(1);
            }

            return ret;
        }

        public override Task<int> BeginHold()
        {
            return Task.Factory.StartNew(() =>
            {
                return Hold();
            });
        }

        public override int Release()
        {
            int ret = 0;
            if ((ret = m_dicDioPoints[DioPointKey.Output_Eject_On.ToString()].Write(DioValue.Off)) != 0) return ret;
            if ((ret = m_dicDioPoints[DioPointKey.Output_Vacuum1_On.ToString()].Write(DioValue.Off)) != 0) return ret;
            if ((ret = m_dicDioPoints[DioPointKey.Output_Vacuum2_On.ToString()].Write(DioValue.Off)) != 0) return ret;

            DateTime startTime = DateTime.Now;
            while (true)
            {
                if (IsRelease())
                    break;
                TimeSpan processTime = DateTime.Now - startTime;
                if (ResponseTimeout != 0 && processTime.TotalSeconds >= ResponseTimeout)
                {
                    //Timeout
                    ret = -1;
                    break;
                }
                Thread.Sleep(1);
            }
            return ret;
        }

        public override Task<int> BeginRelease()
        {
            return Task.Factory.StartNew(() =>
            {
                return Release();
            });
        }
    }
}
