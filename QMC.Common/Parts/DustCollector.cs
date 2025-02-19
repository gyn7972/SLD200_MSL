using QMC.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Process.WorkStage.Parts
{
    public class DustCollector : Part
    {
        public enum DioPointKey
        {
            Output_DustCollector_On,
        }
        public DustCollector(string strName) : base(strName)
        {
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
            ret = SetOnOff(false);
            return ret;
        }

        public override void Stop()
        {
            base.Stop();
            SetOnOff(false);
        }

        public override void Close()
        {
            base.Close();
        }

        public bool IsOn()
        {
            bool ret = false;

            DioPoint dioDustCollector = m_dicDioPoints[DioPointKey.Output_DustCollector_On.ToString()];
            if(dioDustCollector != null)
            {
                DioValue value = dioDustCollector.GetValue();
                if(value == DioValue.On)
                {
                    ret = true;
                }
            }

            return ret;
        }
        public int SetOnOff(bool bOn)
        {
            int ret = 0;

            DioPoint dioDustCollector = m_dicDioPoints[DioPointKey.Output_DustCollector_On.ToString()];
            if (dioDustCollector != null)
            {
                if (bOn)
                {
                    dioDustCollector.Write(DioValue.On);
                }
                else
                {
                    dioDustCollector.Write(DioValue.Off);
                }
            }
            else
            {
                Alarm alarm = new Alarm();
                alarm.Grade = "Error";
                alarm.Source = this.Name;
                alarm.Code = -10;
                alarm.Cause = "집진기 I/O 가 설정 되어 있지 않습니다. I/O 설정을 확인 해 주세요.";
                alarm.Title = "Invalid I/O";
                AlarmManager.Instance.ShowAlarm(alarm);
                ret = alarm.Code;
            }

            return ret;
        }
    }
}
