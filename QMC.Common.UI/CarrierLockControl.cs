using QMC.Common.Parts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QMC.Common.UI
{
    public partial class CarrierLockControl : UserControl
    {
        CarrierLocker m_Locker;
        public CarrierLockControl() : this(null)
        {
        }

        public CarrierLockControl(CarrierLocker locker)
        {
            InitializeComponent();
            m_Locker = locker;
        }

        private void baseToggleButtonLock_Click(object sender, EventArgs e)
        {
            if(m_Locker != null)
            {
                Task<int> result = m_Locker.BeginLock(); // 참고: 무빙, 프로그래스 바 연동
                ProgressForm progressForm = new ProgressForm("Unload Zone", "Locker Moving...", result);

                progressForm.ShowDialog();
                if (result.Result != 0)
                {
                    Alarm alarm = new Alarm();
                    alarm.Source = m_Locker.Name + "/" + m_Locker.Name;
                    alarm.Code = 1;
                    alarm.Grade = "Error";
                    alarm.GeneratedTime = DateTime.Now;
                    alarm.Cause = "Locker Don't Move.";

                    AlarmManager.Instance.ShowAlarm(alarm);
                }
            }
        }

        private void baseToggleButtonUnlock_Click(object sender, EventArgs e)
        {
            if (m_Locker != null)
            {
                Task<int> result = m_Locker.BeginUnlock(); // 참고: 무빙, 프로그래스 바 연동
                ProgressForm progressForm = new ProgressForm("Unload Zone", "Locker Moving...", result);

                progressForm.ShowDialog();
                if (result.Result != 0)
                {
                    Alarm alarm = new Alarm();
                    alarm.Source = m_Locker.Name + "/" + m_Locker.Name;
                    alarm.Code = 1;
                    alarm.Grade = "Error";
                    alarm.GeneratedTime = DateTime.Now;
                    alarm.Cause = "Locker Don't Move.";

                    AlarmManager.Instance.ShowAlarm(alarm);
                }
            }
        }
        public void UpdateCarrierLockStatus()
        {
            if (m_Locker == null)
                return;

            if (m_Locker.IsLock())
            {
                baseToggleButtonLock.UpdateToggleStatus(true);
                baseToggleButtonUnlock.UpdateToggleStatus(false);
            }
            else
            {
                baseToggleButtonLock.UpdateToggleStatus(false);
                baseToggleButtonUnlock.UpdateToggleStatus(true);
            }
        }

       
    }
}
