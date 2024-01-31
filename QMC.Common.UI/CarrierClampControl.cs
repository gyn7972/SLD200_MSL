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
    public partial class CarrierClampControl : UserControl
    {
        CarrierClamper m_Clamper;
        public enum ButtonType
        {
            Clamp,
            Unclamp,
        }
        public CarrierClampControl() : this(null)
        {
        }

        public CarrierClampControl(CarrierClamper clamp)
        {
            InitializeComponent();
            m_Clamper = clamp;
        }

        private void baseToggleButtonClamp_Click(object sender, EventArgs e)
        {
            if (m_Clamper != null)
            {
                Task<int> result = m_Clamper.BeginClamp(); // 참고: 무빙, 프로그래스 바 연동
                ProgressForm progressForm = new ProgressForm("Unload Zone Carrier Clamper", "Moving...", result);

                progressForm.ShowDialog();
                if (result.Result != 0)
                {
                    Alarm alarm = new Alarm();
                    alarm.Source = m_Clamper.Name + "/" + m_Clamper.Name;
                    alarm.Code = 1;
                    alarm.Grade = "Error";
                    alarm.GeneratedTime = DateTime.Now;
                    alarm.Cause = "Clamper Don't Move.";

                    AlarmManager.Instance.ShowAlarm(alarm);
                }
            }
        }

        private void baseToggleButtonUnclamp_Click(object sender, EventArgs e)
        {
            if (m_Clamper != null)
            {
                Task<int> result = m_Clamper.BeginUnclamp(); // 참고: 무빙, 프로그래스 바 연동
                ProgressForm progressForm = new ProgressForm("Unload Zone Carrier Clamper", "Moving...", result);

                progressForm.ShowDialog();
                if (result.Result != 0)
                {
                    Alarm alarm = new Alarm();
                    alarm.Source = m_Clamper.Name + "/" + m_Clamper.Name;
                    alarm.Code = 1;
                    alarm.Grade = "Error";
                    alarm.GeneratedTime = DateTime.Now;
                    alarm.Cause = "Clamper Don't Move.";

                    AlarmManager.Instance.ShowAlarm(alarm);
                }
            }
        }
        public void UpdateClampStatus()
        {
            if (m_Clamper == null)
                return;
            if (m_Clamper.IsClamp())
            {
                baseToggleButtonClamp.UpdateToggleStatus(true);
                baseToggleButtonUnclamp.UpdateToggleStatus(false);
            }
            else
            {
                baseToggleButtonClamp.UpdateToggleStatus(false);
                baseToggleButtonUnclamp.UpdateToggleStatus(true);
            }
        }

        public void SetEnable(bool enable)
        {
            this.baseToggleButtonClamp.Enabled = enable;
            this.baseToggleButtonUnclamp.Enabled = enable;
        }
    }
}
