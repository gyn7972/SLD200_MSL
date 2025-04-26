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
        public event PostAlarmEvent PostAlarm;
        public AlarmManager()
        {
            m_Alarms = new AlarmCollection();
        }

        public bool IsAlarm { get 
            {
                return m_Alarms.Count > 0;
            }
        }
        public void ShowAlarm(Alarm alarm)
        {
            m_Alarms.Add(alarm);
            if (PostAlarm != null)
            {
                PostAlarm(alarm);
            }
        }


    }
}
