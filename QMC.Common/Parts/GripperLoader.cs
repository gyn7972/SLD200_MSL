using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QMC.Common.Parts
{
    public class GripperLoader : Part
    {
        public enum DioPointKey
        {
            Input_MaterialCheck,
            Input_Vacuum_On,
            Input_Vacuum_Off,
            Output_Vacuum_On,
            Output_Vacuum_Off,
            Output_MaterialFix,
        }
        public int ResponseTimeout { set; get; }
        public GripperLoader(string strName) : base( strName)
        {
            ResponseTimeout = 0;
        }
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

        public virtual bool IsHold()
        {
            bool bRet = false;
            DioPoint dioVacuumOn = m_dicDioPoints[DioPointKey.Input_Vacuum_On.ToString()];
            DioPoint dioVacuumOff = m_dicDioPoints[DioPointKey.Input_Vacuum_Off.ToString()];

            if(dioVacuumOn == null || dioVacuumOff == null)
                return bRet;

            DioValue on = dioVacuumOn.GetValue();
            DioValue off = dioVacuumOff.GetValue();

            if (on == DioValue.On && off == DioValue.Off)
            {
                bRet = true;
            }


            return bRet;
        }

        public virtual bool IsRelease()
        {
            bool bRet = false;
            DioPoint dioVacuumOn = m_dicDioPoints[DioPointKey.Input_Vacuum_On.ToString()];
            DioPoint dioVacuumOff = m_dicDioPoints[DioPointKey.Input_Vacuum_Off.ToString()];

            if (dioVacuumOn == null || dioVacuumOff == null)
                return bRet;

            DioValue on = dioVacuumOn.GetValue();
            DioValue off = dioVacuumOff.GetValue();

            if (on == DioValue.Off && off == DioValue.On)
            {
                bRet = true;
            }


            return bRet;
        }
        public virtual int Hold()
        {
            int ret = 0;
            DioPoint dioVacuumOn = m_dicDioPoints[DioPointKey.Output_Vacuum_On.ToString()];
            DioPoint dioVacuumOff = m_dicDioPoints[DioPointKey.Output_Vacuum_Off.ToString()];

            if (dioVacuumOn == null || dioVacuumOff == null)
                return -1;

            if ((ret = dioVacuumOn.Write(DioValue.On)) != 0) return ret;
            if ((ret = dioVacuumOff.Write(DioValue.Off)) != 0) return ret;

            DateTime startTime = DateTime.Now;
            while (true)
            {
                if (IsHold())
                    break;
                TimeSpan processTime = DateTime.Now - startTime;
                if (processTime.TotalSeconds >= ResponseTimeout)
                {
                    //Timeout
                    ret = -1;
                    break;
                }
                Thread.Sleep(1);
            }

            return ret;
        }

        public virtual Task<int> BeginHold()
        {
            return Task.Factory.StartNew(() =>
            {
                return Hold();
            });
        }

        public virtual int Release()
        {
            int ret = 0;
            DioPoint dioVacuumOn = m_dicDioPoints[DioPointKey.Output_Vacuum_On.ToString()];
            DioPoint dioVacuumOff = m_dicDioPoints[DioPointKey.Output_Vacuum_Off.ToString()];

            if (dioVacuumOn == null || dioVacuumOff == null)
                return -1;

            if ((ret = dioVacuumOn.Write(DioValue.Off)) != 0) return ret;
            if ((ret = dioVacuumOff.Write(DioValue.On)) != 0) return ret;

            DateTime startTime = DateTime.Now;
            while (true)
            {
                if (IsRelease())
                    break;
                TimeSpan processTime = DateTime.Now - startTime;
                if (processTime.TotalSeconds >= ResponseTimeout)
                {
                    //Timeout
                    ret = -1;
                    break;
                }
                Thread.Sleep(1);
            }
            return ret;
        }

        public virtual Task<int> BeginRelease()
        {
            return Task.Factory.StartNew(() =>
            {
                return Release();
            });
        }
        public override int Initialize()
        {
            return Release();
        }

        public virtual int MaterialHold()
        {
            int ret = 0;
            DioPoint dioFix = m_dicDioPoints[DioPointKey.Output_MaterialFix.ToString()];

            if (dioFix == null)
                return ret = -1;

            if ((ret = dioFix.Write(DioValue.On)) != 0) return ret;

            return ret;
        }
        public virtual int MaterialRelease()
        {
            int ret = 0;
            DioPoint dioFix = m_dicDioPoints[DioPointKey.Output_MaterialFix.ToString()];

            if (dioFix == null)
                return ret = -1;

            if ((ret = dioFix.Write(DioValue.Off)) != 0) return ret;

            return ret;
        }
    }
}
