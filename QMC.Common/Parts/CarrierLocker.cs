using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QMC.Common.Parts
{
    public class CarrierLocker : Part
    {
        public enum DioPointKey
        {
            Input_Clamp_0_Lock,
            Input_Clamp_0_Unlock,
            Input_Clamp_1_Lock,
            Input_Clamp_1_Unlock,
            Output_Clamp_0_Lock,
            Output_Clamp_0_Unlock,
            Output_Clamp_1_Lock,
            Output_Clamp_1_Unlock,
        }
        public enum AlarmKey
        {           
            eStop = 1,
            eLock_Failed = -1,
            eUnlock_Failed = -2,
        }
        public int ResponseTimeout { set; get; }

        public CarrierLocker(string strName) : base(strName)
        {
            ResponseTimeout = 3000;
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

        protected override void InitAlarm()
        {
            base.InitAlarm();
            {
                Alarm alarm = new Alarm();
                alarm.Code = (int)AlarmKey.eLock_Failed;
                alarm.Title = "Carrier Lock 실패";
                alarm.Cause = "Carrier Lock 구동 I/O를 확인해 주세요.";
                alarm.Source = Name;
                alarm.Grade = "Error";
                m_dicAlarms.Add(alarm.Code, alarm);

                alarm = new Alarm();
                alarm.Code = (int)AlarmKey.eUnlock_Failed;
                alarm.Title = "Carrier Unlock 실패";
                alarm.Cause = "Carrier Unlock 구동 I/O를 확인해 주세요.";
                alarm.Source = Name;
                alarm.Grade = "Error";
                m_dicAlarms.Add(alarm.Code, alarm);

                alarm.Code = (int)AlarmKey.eStop;
                alarm.Title = "설비 정지";
                alarm.Cause = "설비 정지 신호가 On 되었습니다..";
                alarm.Source = Name;
                alarm.Grade = "Error";
                m_dicAlarms.Add(alarm.Code, alarm);
            }
        }


        public virtual int Lock()
        {
            int ret = 0;
            //if (m_Status == RunStatus.Stop)
            //{
            //    ret = 1;
            //    return ret;
            //}
            DioPoint dioClamp_0_On = m_dicDioPoints[DioPointKey.Output_Clamp_0_Lock.ToString()];
            DioPoint dioClamp_0_Off = m_dicDioPoints[DioPointKey.Output_Clamp_0_Unlock.ToString()];
            DioPoint dioClamp_1_On = m_dicDioPoints[DioPointKey.Output_Clamp_1_Lock.ToString()];
            DioPoint dioClamp_1_Off = m_dicDioPoints[DioPointKey.Output_Clamp_1_Unlock.ToString()];

            if (dioClamp_0_On == null || dioClamp_0_Off == null || dioClamp_1_On == null || dioClamp_1_Off == null)
                return -1;
            while(true)
            {
                DioValue clamp_0_on = dioClamp_0_On.GetValue();
                DioValue clamp_0_off = dioClamp_0_Off.GetValue();
                DioValue clamp_1_on = dioClamp_1_On.GetValue();
                DioValue clamp_1_off = dioClamp_1_Off.GetValue();

                if(clamp_0_on == DioValue.Off && clamp_0_off == DioValue.Off &&
                    clamp_1_on == DioValue.Off && clamp_1_on == DioValue.Off)
                {
                    break;
                }

                if ((ret = dioClamp_0_On.Write(DioValue.Off)) != 0)
                {
                    Alarm alarm = GetAlarm((int)AlarmKey.eLock_Failed);
                    AlarmManager.Instance.ShowAlarm(alarm);
                    return (int)AlarmKey.eLock_Failed;
                }
                if ((ret = dioClamp_0_Off.Write(DioValue.Off)) != 0)
                {
                    Alarm alarm = GetAlarm((int)AlarmKey.eLock_Failed);
                    AlarmManager.Instance.ShowAlarm(alarm);
                    return (int)AlarmKey.eLock_Failed;
                }
                if ((ret = dioClamp_1_On.Write(DioValue.Off)) != 0)
                {
                    Alarm alarm = GetAlarm((int)AlarmKey.eLock_Failed);
                    AlarmManager.Instance.ShowAlarm(alarm);
                    return (int)AlarmKey.eLock_Failed;
                }
                if ((ret = dioClamp_1_Off.Write(DioValue.Off)) != 0)
                {
                    Alarm alarm = GetAlarm((int)AlarmKey.eLock_Failed);
                    AlarmManager.Instance.ShowAlarm(alarm);
                    return (int)AlarmKey.eLock_Failed;
                }
                Thread.Sleep(1);
            }
            DateTime startTime = DateTime.Now;
            while (true)
            {
                if ((ret = dioClamp_0_On.Write(DioValue.On)) != 0)
                {
                    Alarm alarm = GetAlarm((int)AlarmKey.eLock_Failed);
                    AlarmManager.Instance.ShowAlarm(alarm);
                    return (int)AlarmKey.eLock_Failed;
                }
                if ((ret = dioClamp_1_On.Write(DioValue.On)) != 0)
                {
                    Alarm alarm = GetAlarm((int)AlarmKey.eLock_Failed);
                    AlarmManager.Instance.ShowAlarm(alarm);
                    return (int)AlarmKey.eLock_Failed;
                }
                if (IsLock())
                    break;
                if (ResponseTimeout > 0)
                {
                    TimeSpan processTime = DateTime.Now - startTime;
                    if (processTime.TotalMilliseconds > ResponseTimeout)
                    {
                        //Timeout
                        Alarm alarm = GetAlarm((int)AlarmKey.eLock_Failed);
                        AlarmManager.Instance.ShowAlarm(alarm);
                        ret = (int)AlarmKey.eLock_Failed;
                        break;
                    }
                }
                Thread.Sleep(1);
            }
            Thread.Sleep(1);
            if ((ret = dioClamp_0_On.Write(DioValue.On)) != 0)
            {
                Alarm alarm = GetAlarm((int)AlarmKey.eLock_Failed);
                AlarmManager.Instance.ShowAlarm(alarm);
                return (int)AlarmKey.eLock_Failed;
            }
            if ((ret = dioClamp_0_Off.Write(DioValue.On)) != 0)
            {
                Alarm alarm = GetAlarm((int)AlarmKey.eLock_Failed);
                AlarmManager.Instance.ShowAlarm(alarm);
                return (int)AlarmKey.eLock_Failed;
            }
            if ((ret = dioClamp_1_On.Write(DioValue.On)) != 0)
            {
                Alarm alarm = GetAlarm((int)AlarmKey.eLock_Failed);
                AlarmManager.Instance.ShowAlarm(alarm);
                return (int)AlarmKey.eLock_Failed;
            }
            if ((ret = dioClamp_1_Off.Write(DioValue.On)) != 0)
            {
                Alarm alarm = GetAlarm((int)AlarmKey.eLock_Failed);
                AlarmManager.Instance.ShowAlarm(alarm);
                return (int)AlarmKey.eLock_Failed;
            }

            return ret;
        }

        public virtual Task<int> BeginLock()
        {
            return Task.Factory.StartNew(() =>
            {
                return Lock();
            });
        }

        public virtual bool IsLock()
        {
            bool bRet = false;
            DioPoint dioClacmp_0_On = m_dicDioPoints[DioPointKey.Input_Clamp_0_Lock.ToString()];
            DioPoint dioClacmp_0_Off = m_dicDioPoints[DioPointKey.Input_Clamp_0_Unlock.ToString()];
            DioPoint dioClacmp_1_On = m_dicDioPoints[DioPointKey.Input_Clamp_1_Lock.ToString()];
            DioPoint dioClacmp_1_Off = m_dicDioPoints[DioPointKey.Input_Clamp_1_Unlock.ToString()];

            if (dioClacmp_0_On == null || dioClacmp_0_Off == null || dioClacmp_1_On == null || dioClacmp_1_Off == null)
                return bRet;

            DioValue Clamp_0_on = dioClacmp_0_On.GetValue();
            DioValue Clamp_0_off = dioClacmp_0_Off.GetValue();
            DioValue Clamp_1_on = dioClacmp_1_On.GetValue();
            DioValue Clamp_1_off = dioClacmp_1_Off.GetValue();

            if (Clamp_0_on == DioValue.On && Clamp_0_off == DioValue.Off && Clamp_1_on == DioValue.On && Clamp_1_off == DioValue.Off)
            {
                bRet = true;
            }
            return bRet;
        }

        /////
        ///
        public virtual int Unlock()
        {
            int ret = 0;
            //if (m_Status == RunStatus.Stop)
            //{
            //    ret = 1;
            //    return ret;
            //}
            DioPoint dioClamp_0_On = m_dicDioPoints[DioPointKey.Output_Clamp_0_Lock.ToString()];
            DioPoint dioClamp_0_Off = m_dicDioPoints[DioPointKey.Output_Clamp_0_Unlock.ToString()];
            DioPoint dioClamp_1_On = m_dicDioPoints[DioPointKey.Output_Clamp_1_Lock.ToString()];
            DioPoint dioClamp_1_Off = m_dicDioPoints[DioPointKey.Output_Clamp_1_Unlock.ToString()];

            if (dioClamp_0_On == null || dioClamp_0_Off == null || dioClamp_1_On == null || dioClamp_1_Off == null)
                return -1;

            while (true)
            {
                DioValue clamp_0_on = dioClamp_0_On.GetValue();
                DioValue clamp_0_off = dioClamp_0_Off.GetValue();
                DioValue clamp_1_on = dioClamp_1_On.GetValue();
                DioValue clamp_1_off = dioClamp_1_Off.GetValue();

                if (clamp_0_on == DioValue.Off && clamp_0_off == DioValue.Off &&
                    clamp_1_on == DioValue.Off && clamp_1_on == DioValue.Off)
                {
                    break;
                }

                if ((ret = dioClamp_0_On.Write(DioValue.Off)) != 0)
                {
                    Alarm alarm = GetAlarm((int)AlarmKey.eUnlock_Failed);
                    AlarmManager.Instance.ShowAlarm(alarm);
                    return ret;
                }
                if ((ret = dioClamp_0_Off.Write(DioValue.Off)) != 0)
                {
                    Alarm alarm = GetAlarm((int)AlarmKey.eUnlock_Failed);
                    AlarmManager.Instance.ShowAlarm(alarm);
                    return ret;
                }
                if ((ret = dioClamp_1_On.Write(DioValue.Off)) != 0)
                {
                    Alarm alarm = GetAlarm((int)AlarmKey.eUnlock_Failed);
                    AlarmManager.Instance.ShowAlarm(alarm);
                    return ret;
                }
                if ((ret = dioClamp_1_Off.Write(DioValue.Off)) != 0)
                {
                    Alarm alarm = GetAlarm((int)AlarmKey.eUnlock_Failed);
                    AlarmManager.Instance.ShowAlarm(alarm);
                    return ret;
                }
                Thread.Sleep(1);
            }
            DateTime startTime = DateTime.Now;
            while (true)
            {
                if ((ret = dioClamp_0_Off.Write(DioValue.On)) != 0)
                {
                    Alarm alarm = GetAlarm((int)AlarmKey.eUnlock_Failed);
                    AlarmManager.Instance.ShowAlarm(alarm);
                    return ret;
                }
                if ((ret = dioClamp_1_Off.Write(DioValue.On)) != 0)
                {
                    Alarm alarm = GetAlarm((int)AlarmKey.eUnlock_Failed);
                    AlarmManager.Instance.ShowAlarm(alarm);
                    return ret;
                }
                if (!IsLock())
                    break;
                if (ResponseTimeout > 0)
                {
                    TimeSpan processTime = DateTime.Now - startTime;
                    if (processTime.TotalMilliseconds > ResponseTimeout)
                    {
                        //Timeout
                        Alarm alarm = GetAlarm((int)AlarmKey.eUnlock_Failed);
                        AlarmManager.Instance.ShowAlarm(alarm);
                        ret = -1;
                        break;
                    }
                }
                Thread.Sleep(1);
            }
            return ret;
        }

        public virtual Task<int> BeginUnlock()
        {
            return Task.Factory.StartNew(() =>
            {
                return Unlock();
            });
        }

        public virtual int Init()
        {
            int ret = 0;
            //if (m_Status == RunStatus.Stop)
            //{
            //    ret = 1;
            //    return ret;
            //}
            DioPoint dioClamp_0_On = m_dicDioPoints[DioPointKey.Output_Clamp_0_Lock.ToString()];
            DioPoint dioClamp_0_Off = m_dicDioPoints[DioPointKey.Output_Clamp_0_Unlock.ToString()];
            DioPoint dioClamp_1_On = m_dicDioPoints[DioPointKey.Output_Clamp_1_Lock.ToString()];
            DioPoint dioClamp_1_Off = m_dicDioPoints[DioPointKey.Output_Clamp_1_Unlock.ToString()];

            if (dioClamp_0_On == null || dioClamp_0_Off == null || dioClamp_1_On == null || dioClamp_1_Off == null)
                return -1;
            while (true)
            {
                DioValue clamp_0_on = dioClamp_0_On.GetValue();
                DioValue clamp_0_off = dioClamp_0_Off.GetValue();
                DioValue clamp_1_on = dioClamp_1_On.GetValue();
                DioValue clamp_1_off = dioClamp_1_Off.GetValue();

                if (clamp_0_on == DioValue.Off && clamp_0_off == DioValue.Off &&
                    clamp_1_on == DioValue.Off && clamp_1_on == DioValue.Off)
                {
                    break;
                }

                if ((ret = dioClamp_0_On.Write(DioValue.Off)) != 0)
                {
                    Alarm alarm = GetAlarm((int)AlarmKey.eLock_Failed);
                    AlarmManager.Instance.ShowAlarm(alarm);
                    return ret;
                }
                if ((ret = dioClamp_0_Off.Write(DioValue.Off)) != 0)
                {
                    Alarm alarm = GetAlarm((int)AlarmKey.eLock_Failed);
                    AlarmManager.Instance.ShowAlarm(alarm);
                    return ret;
                }
                if ((ret = dioClamp_1_On.Write(DioValue.Off)) != 0)
                {
                    Alarm alarm = GetAlarm((int)AlarmKey.eLock_Failed);
                    AlarmManager.Instance.ShowAlarm(alarm);
                    return ret;
                }
                if ((ret = dioClamp_1_Off.Write(DioValue.Off)) != 0)
                {
                    Alarm alarm = GetAlarm((int)AlarmKey.eLock_Failed);
                    AlarmManager.Instance.ShowAlarm(alarm);
                    return ret;
                }
                Thread.Sleep(1);
            }
            DateTime startTime = DateTime.Now;
            while (true)
            {
                if ((ret = dioClamp_0_On.Write(DioValue.On)) != 0)
                {
                    Alarm alarm = GetAlarm((int)AlarmKey.eLock_Failed);
                    AlarmManager.Instance.ShowAlarm(alarm);
                    return ret;
                }
                if ((ret = dioClamp_1_On.Write(DioValue.On)) != 0)
                {
                    Alarm alarm = GetAlarm((int)AlarmKey.eLock_Failed);
                    AlarmManager.Instance.ShowAlarm(alarm);
                    return ret;
                }
                if (IsInit())
                    break;
                if (ResponseTimeout > 0)
                {
                    TimeSpan processTime = DateTime.Now - startTime;
                    if (processTime.TotalMilliseconds > ResponseTimeout)
                    {
                        //Timeout
                        Alarm alarm = GetAlarm((int)AlarmKey.eLock_Failed);
                        AlarmManager.Instance.ShowAlarm(alarm);
                        ret = -1;
                        break;
                    }
                }
                Thread.Sleep(1);
            }
            return ret;
        }


        public virtual bool IsInit() // IDLE = Lock
        {
            bool bRet = false;
            DioPoint dioClacmp_0_On = m_dicDioPoints[DioPointKey.Input_Clamp_0_Lock.ToString()];
            DioPoint dioClacmp_0_Off = m_dicDioPoints[DioPointKey.Input_Clamp_0_Unlock.ToString()];
            DioPoint dioClacmp_1_On = m_dicDioPoints[DioPointKey.Input_Clamp_1_Lock.ToString()];
            DioPoint dioClacmp_1_Off = m_dicDioPoints[DioPointKey.Input_Clamp_1_Unlock.ToString()];

            if (dioClacmp_0_On == null || dioClacmp_0_Off == null || dioClacmp_1_On == null || dioClacmp_1_Off == null)
                return bRet;

            DioValue Clamp_0_on = dioClacmp_0_On.GetValue();
            DioValue Clamp_0_off = dioClacmp_0_Off.GetValue();
            DioValue Clamp_1_on = dioClacmp_1_On.GetValue();
            DioValue Clamp_1_off = dioClacmp_1_Off.GetValue();

            if (Clamp_0_on == DioValue.On && Clamp_0_off == DioValue.Off && Clamp_1_on == DioValue.On && Clamp_1_off == DioValue.Off)
            {
                bRet = true;
            }
            return bRet;
        }
        ////////////////////////////
        ///
    }
}
