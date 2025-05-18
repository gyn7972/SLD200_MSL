using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QMC.Common
{
    public delegate void PostAlarmEvent(Alarm alarm);
    public class AlarmManager
    {
        #region Singleton 
        private static AlarmManager g_AlarmManager;
        public static AlarmManager Instance
        {
            get
            {
                if (g_AlarmManager == null)
                    g_AlarmManager = new AlarmManager();
                return g_AlarmManager;
            }
        }
        #endregion

        private AlarmCollection m_Alarms;
        public AlarmCollection Alarms 
        { 
            get
            {
                return m_Alarms;
            }
        }

        public AlarmSaver Saver { set; get; }

        public event PostAlarmEvent PostAlarm;
        public AlarmManager()
        {
            m_Alarms = new AlarmCollection();
        }

        public bool IsAlarm { 
            get 
            {
                return m_Alarms.Where(t=>t.Grade.Equals("Error")).Count()> 0;
            }
        }

        // 알람 동시 발생으로 프로그램 다운 발생.
        // _lock을 사용하여 알람 리스트에 안전하게 추가하고,
        // PostAlarm 이벤트를 UI 스레드에서 실행하도록 수정.
        private readonly object _lock = new object();

        public void ShowAlarm(Alarm alarm)
        {
            lock (_lock)
            {
                // 1. 알람 리스트에 안전하게 추가
                m_Alarms.Add(alarm);
            }

            // 2. PostAlarm 이벤트 (UI 스레드에서 실행)
            if (PostAlarm != null)
            {
                if (Application.OpenForms.Count > 0)
                {
                    var form = Application.OpenForms[0];
                    if (form.InvokeRequired)
                    {
                        form.BeginInvoke(new Action(() => PostAlarm?.Invoke(alarm)));
                    }
                    else
                    {
                        PostAlarm?.Invoke(alarm);
                    }
                }
                else
                {
                    // UI 폼이 없으면 그냥 호출 (예: 콘솔 앱)
                    PostAlarm?.Invoke(alarm);
                }
            }

            // 3. saver에 알람 저장
            if (Saver != null)
            {
                Saver.AddAlarm(alarm);
                Saver.Close();
            }

            // 4. 마지막 알람 메세지를 Title bar 에 보이게


            //m_Alarms.Add(alarm);
            //if (PostAlarm != null)
            //{
            //    PostAlarm(alarm);
            //}
        }


    }
}
