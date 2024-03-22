using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QMC.Common.Parts
{
    public delegate void OperationButtonsClickEventHandler(OperationButtons.ButtonType type);
    public class OperationButtons : Part
    {
        public enum ButtonType
        {
            Reset,
        }
        public OperationButtonsClickEventHandler ButtonClick;
        public enum DioPointKey
        {
            Input_Start,
            Input_Stop,
            Input_Reset,
            Input_EMG,
            Output_Start,
            Output_Stop,
            Output_Reset,
        }

        public enum AlarmKey
        {
            eStart_Failed = -1,
            eStop_Failed = -2,
            eReset_Failed = -3,
            eEMG_Failed = -4,
        }

        protected override void InitAlarm()
        {
            base.InitAlarm();
            {
                Alarm alarm = new Alarm();
                alarm.Code = (int)AlarmKey.eStart_Failed;
                alarm.Title = "Operation Start Button 작동 Error";
                alarm.Cause = "Operation Start Button  I/O를 확인해 주세요.";
                alarm.Source = Name;
                alarm.Grade = "Error";
                m_dicAlarms.Add(alarm.Code, alarm);
            }
            {
                Alarm alarm = new Alarm();
                alarm.Code = (int)AlarmKey.eStop_Failed;
                alarm.Title = "Operation Stop Button 작동 Error";
                alarm.Cause = "Operation Stop Button  I/O를 확인해 주세요.";
                alarm.Source = Name;
                alarm.Grade = "Error";
                m_dicAlarms.Add(alarm.Code, alarm);
            }
            {
                Alarm alarm = new Alarm();
                alarm.Code = (int)AlarmKey.eReset_Failed;
                alarm.Title = "Operation Reset Button 작동 Error";
                alarm.Cause = "Operation Reset Button  I/O를 확인해 주세요.";
                alarm.Source = Name;
                alarm.Grade = "Error";
                m_dicAlarms.Add(alarm.Code, alarm);
            }
            {
                Alarm alarm = new Alarm();
                alarm.Code = (int)AlarmKey.eEMG_Failed;
                alarm.Title = "Operation EMG Button 작동 Error";
                alarm.Cause = "Operation EMG Button  I/O를 확인해 주세요.";
                alarm.Source = Name;
                alarm.Grade = "Error";
                m_dicAlarms.Add(alarm.Code, alarm);
            }
        }

        protected bool m_bExit;
        protected Thread m_workThread;
        public OperationButtonsConfig Config;
        public OperationButtons(string strname) : base(strname)
        {
            Config = new OperationButtonsConfig();

        }

        public override int Create()
        {
            int ret = 0;

            if (m_dicDioPoints == null)
                m_dicDioPoints = new Dictionary<string, DioPoint>();

            m_dicDioPoints.Clear();
            foreach (DioPointKey key in Enum.GetValues(typeof(DioPointKey)))
            {
                m_dicDioPoints.Add(key.ToString(), null);
            }

            m_bExit = false;
            m_workThread = new Thread(WorkThreadProc);
            m_workThread.Start();

            return base.Create();
        }

        public override void Close()
        {
            m_bExit=true;
            m_workThread.Join();
            Start(false);
            Stop(false);
            base.Close();
        }
        private void WorkThreadProc()
        {
            bool bEmg = false;
            Thread.Sleep(5000);
            while (true)
            {
                if (m_bExit)
                    break;

                if (IsEMG())
                {
                    //Alarm
                    if (!bEmg)
                    {

                        bEmg = true;
                        OnEmg();
                    }
                }
                else
                {
                    bEmg = false;
                }

                if (IsStart())
                {
                    OnStart();
                }

                if (IsStop())
                {
                    OnStop();
                }

                if(IsReset())
                {
                    OnReset();
                }

                Thread.Sleep(10);
            }
        }

        public int Start(bool bOn)
        {
            int ret = 0;
            //if (m_Status == RunStatus.Stop)
            //{
            //    ret = 1;
            //    return ret;
            //}
            DioPoint dioStart = m_dicDioPoints[DioPointKey.Output_Start.ToString()];

            if (dioStart == null)
                return -1;
            DioValue dvOn = DioValue.Off;
            if (bOn)
            {
                dvOn = DioValue.On;
            }

            if ((ret = dioStart.Write(dvOn)) != 0)
            {
                Alarm alarm = GetAlarm((int)AlarmKey.eStop_Failed);
                AlarmManager.Instance.ShowAlarm(alarm);
                return ret;
            }
            return ret;
        }
        public bool IsStart()
        {
            bool bRet = false;
            DioPoint dioStart = m_dicDioPoints[DioPointKey.Input_Start.ToString()];

            if (dioStart == null)
                return bRet;

            DioValue start = dioStart.GetValue();

            if (start == DioValue.On)
            {
                bRet = true;
            }
            return bRet;
        }

        public int Stop(bool bOn)
        {
            int ret = 0;
            DioPoint dioStop = m_dicDioPoints[DioPointKey.Output_Stop.ToString()];

            if (dioStop == null)
                return -1;

            DioValue dvOff = DioValue.Off;
            if (bOn)
            {
                dvOff = DioValue.On;
            }

            if ((ret = dioStop.Write(dvOff)) != 0)
            {
                Alarm alarm = GetAlarm((int)AlarmKey.eStop_Failed);
                AlarmManager.Instance.ShowAlarm(alarm);
                return ret;
            }
            return ret;
        }
        public bool IsStop()
        {
            bool bRet = false;
            DioPoint dioStop = m_dicDioPoints[DioPointKey.Input_Stop.ToString()];

            if (dioStop == null)
                return bRet;

            DioValue stop = dioStop.GetValue();

            if (stop == DioValue.On)
            {
                bRet = true;
            }
            return bRet;
        }

        public int Reset(bool bOn)
        {
            int ret = 0;
            DioPoint dioReset = m_dicDioPoints[DioPointKey.Output_Reset.ToString()];

            if (dioReset == null)
                return -1;

            DioValue dvReset = DioValue.Off;
            if (bOn)
            {
                dvReset = DioValue.On;
            }

            if ((ret = dioReset.Write(dvReset)) != 0)
            {
                Alarm alarm = GetAlarm((int)AlarmKey.eReset_Failed);
                AlarmManager.Instance.ShowAlarm(alarm);
                return ret;
            }
            return ret;
        }
        public bool IsReset()
        {
            bool bRet = false;
            DioPoint dioReset = m_dicDioPoints[DioPointKey.Input_Reset.ToString()];

            if (dioReset == null)
                return bRet;

            DioValue dvReset = dioReset.GetValue();


            if (dvReset == DioValue.On)
            {
                bRet = true;
            }
            return bRet;
        }

        public bool IsEMG()
        {
            bool bRet = false;
            DioPoint dioReset = m_dicDioPoints[DioPointKey.Input_EMG.ToString()];

            if (dioReset == null)
                return bRet;

            DioValue dvReset = dioReset.GetValue();


            if (dvReset == DioValue.Off)
            {
                bRet = true;
            }
            return bRet;
        }

        public virtual void OnStart()
        {

        }

        public virtual void OnStop()
        {

        }
        public virtual void OnReset()
        {
            //if(ButtonClick != null)
            //{
            //    ButtonClick(ButtonType.Reset);
            //}
        }

        public virtual void OnEmg()
        {

        }

        public virtual void Reset()
        {

        }
    }
}
