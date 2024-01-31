using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QMC.Common.Parts
{

    public class CarrierClamper : Part
    {
        public enum DioPointKey
        {
            Input_Carrier_Clamp,
            Input_Carrier_Unclamp,
            Output_Carrier_Clamp,
            Output_Carrier_Unclamp,
        }
        public enum AlarmKey
        {
            eStop = 1,
            eInitialize_Failed = -1,
            eClamp_Failed = -2,
            eUnClamp_Failed = -3,
        }
        public int ResponseTimeout { set; get; }
        public CarrierLocker Locker { set; get; }
        public CarrierClamper(string strName) : base(strName)
        {
            ResponseTimeout = 1000;
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

        public override int Initialize()
        {
            int ret = 0;
            if ((ret = base.Initialize()) != 0) return ret;
            if ((ret = Unclamp()) != 0) return ret;
            //if (Locker != null)
            //{
            //    if ((ret = Locker.Lock()) != 0) return ret;
            //}


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
                alarm.Code = (int)AlarmKey.eInitialize_Failed;
                alarm.Title = "초기화 실패";
                alarm.Cause = "Home 구동 I/O를 확인해 주세요.";
                alarm.Source = Name;
                alarm.Grade = "Error";
                m_dicAlarms.Add(alarm.Code, alarm);

                alarm = new Alarm();
                alarm.Code = (int)AlarmKey.eClamp_Failed;
                alarm.Title = "Clamp 실패";
                alarm.Cause = "Clamp 구동 I/O를 확인해 주세요.";
                alarm.Source = Name;
                alarm.Grade = "Error";
                m_dicAlarms.Add(alarm.Code, alarm);

                alarm = new Alarm();
                alarm.Code = (int)AlarmKey.eUnClamp_Failed;
                alarm.Title = "Unclamp 실패";
                alarm.Cause = "Clamp 구동 I/O를 확인해 주세요.";
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
        public virtual int Clamp()
        {
            int ret = 0;
            //if (m_Status == RunStatus.Stop)
            //{
            //    ret = 1;
            //    return ret;
            //}

            DioPoint dioClamp = m_dicDioPoints[DioPointKey.Output_Carrier_Clamp.ToString()];
            DioPoint dioUnclamp = m_dicDioPoints[DioPointKey.Output_Carrier_Unclamp.ToString()];

            if (dioUnclamp == null || dioClamp == null)
                return -1;

            if (Locker != null)
            {
                Locker.Init();
                Thread.Sleep(1);
            }

            while(true)
            {
                DioValue clamp = dioClamp.GetValue();
                DioValue unclamp = dioUnclamp.GetValue();
                if(clamp == DioValue.Off && unclamp == DioValue.Off)
                {
                    break;
                }
                if ((ret = dioClamp.Write(DioValue.Off)) != 0)
                {
                    Alarm alarm = GetAlarm((int)AlarmKey.eClamp_Failed);
                    AlarmManager.Instance.ShowAlarm(alarm);
                    return (int)AlarmKey.eClamp_Failed;
                }
                if ((ret = dioUnclamp.Write(DioValue.Off)) != 0)
                {
                    Alarm alarm = GetAlarm((int)AlarmKey.eClamp_Failed);
                    AlarmManager.Instance.ShowAlarm(alarm);
                    return (int)AlarmKey.eClamp_Failed;
                }
                Thread.Sleep(1);
            }
           
            DateTime startTime = DateTime.Now;
            while (true)
            {
                if ((ret = dioClamp.Write(DioValue.On)) != 0)
                {
                    Alarm alarm = GetAlarm((int)AlarmKey.eClamp_Failed);
                    AlarmManager.Instance.ShowAlarm(alarm);
                    return (int)AlarmKey.eClamp_Failed;
                }
                if (IsClamp())
                    break;
                if (ResponseTimeout > 0)
                {
                    TimeSpan processTime = DateTime.Now - startTime;
                    if (processTime.TotalMilliseconds > ResponseTimeout)
                    {
                        //Timeout
                        Alarm alarm = GetAlarm((int)AlarmKey.eClamp_Failed);
                        AlarmManager.Instance.ShowAlarm(alarm);
                        ret = (int)AlarmKey.eClamp_Failed;
                        break;
                    }
                }
                Thread.Sleep(1);
            }
            Thread.Sleep(500);
            if (Locker != null)
            {
                Locker.Unlock();
                Thread.Sleep(1);
            }
            return ret;
        }

        public virtual Task<int> BeginClamp()
        {
            return Task.Factory.StartNew(() =>
            {
                return Clamp();
            });
        }

        public virtual bool IsClamp()
        {
            bool bRet = false;
            DioPoint dioClamp = m_dicDioPoints[DioPointKey.Input_Carrier_Clamp.ToString()];
            DioPoint dioUnclamp = m_dicDioPoints[DioPointKey.Input_Carrier_Unclamp.ToString()];

            if (dioClamp == null || dioUnclamp == null)
                return bRet;

            DioValue on = dioClamp.GetValue();
            DioValue off = dioUnclamp.GetValue();

            if (on == DioValue.On && off == DioValue.Off)
            {
                bRet = true;
            }
            return bRet;
        }

        public virtual int Unclamp()
        {
            int ret = 0;
            //if (m_Status == RunStatus.Stop)
            //{
            //    ret = 1;
            //    return ret;
            //}
            DioPoint dioClamp = m_dicDioPoints[DioPointKey.Output_Carrier_Clamp.ToString()];
            DioPoint dioUnclamp = m_dicDioPoints[DioPointKey.Output_Carrier_Unclamp.ToString()];

            if (dioClamp == null || dioUnclamp == null)
                return -1;

            //공압 풀기.
            if (Locker != null)
            {
                Locker.Lock();
                Thread.Sleep(500);
            }

            while(true)
            {
                DioValue clamp = dioClamp.GetValue();
                DioValue unclamp = dioUnclamp.GetValue();
                if(clamp == DioValue.Off && unclamp == DioValue.Off)
                {
                    break;
                }
                if ((ret = dioClamp.Write(DioValue.Off)) != 0)
                {
                    Alarm alarm = GetAlarm((int)AlarmKey.eUnClamp_Failed);
                    AlarmManager.Instance.ShowAlarm(alarm);
                    return (int)AlarmKey.eUnClamp_Failed;
                }
                if ((ret = dioUnclamp.Write(DioValue.Off)) != 0)
                {
                    Alarm alarm = GetAlarm((int)AlarmKey.eUnClamp_Failed);
                    AlarmManager.Instance.ShowAlarm(alarm);
                    return (int)AlarmKey.eUnClamp_Failed;
                }
                Thread.Sleep(500);
            }

            DateTime startTime = DateTime.Now;
            while (true)
            {
                if ((ret = dioUnclamp.Write(DioValue.On)) != 0)
                {
                    Alarm alarm = GetAlarm((int)AlarmKey.eUnClamp_Failed);
                    AlarmManager.Instance.ShowAlarm(alarm);
                    return (int)AlarmKey.eUnClamp_Failed;
                }
                if (!IsClamp())
                    break;
                if (ResponseTimeout > 0)
                {
                    TimeSpan processTime = DateTime.Now - startTime;
                    if (processTime.TotalMilliseconds > ResponseTimeout)
                    {
                        //Timeout
                        Alarm alarm = GetAlarm((int)AlarmKey.eUnClamp_Failed);
                        AlarmManager.Instance.ShowAlarm(alarm);
                        ret = (int)AlarmKey.eUnClamp_Failed;
                        break;
                    }
                }
                Thread.Sleep(500);
            }
            Thread.Sleep(500);
            if (Locker != null)
            {
                if(!IsClamp())
                    Locker.Init();
                Thread.Sleep(500);
            }
            return ret;
        }


        public virtual Task<int> BeginUnclamp()
        {
            return Task.Factory.StartNew(() =>
            {
                return Unclamp();
            });
        }

        public int Lock()
        {
            int ret = 0;

            if (Locker != null)
                ret = Locker.Lock();

            return ret;
        }

        public int Unlock()
        {
            int ret = 0;

            if (Locker != null)
                ret = Locker.Unlock();

            return ret;
        }

        public bool IsLock()
        {
            bool ret = false;

            if (Locker != null)
                ret = Locker.IsLock();

            return ret;
        }



    }
}
