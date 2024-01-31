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
using static QMC.Common.Parts.Conveyor;

namespace QMC.Common.UI
{
    public delegate void ConveyorControlButtonClickHandler(ConveyorControl.ButtonType type);
    public partial class ConveyorControl : UserControl
    {
        public enum ButtonType
        {
            ConveyorRun,
            ConveyorStop,
            DirectionFoward,
            DirectionBackWard,
            StopperUp,
            StopperDown,
            WorkRun,
            Save,
        }
        public ConveyorControlButtonClickHandler ButtonClick;
        protected Conveyor m_Conveyor;
        
        public ConveyorControl() : this(null)
        { 

        }

        public ConveyorControl(Conveyor conveyor)
        {
            m_Conveyor = conveyor;
            InitializeComponent();
            Setdata();
        }

        private void Setdata()
        {
            baseTextBoxDelayTime.Text = m_Conveyor.Config.DelayDetectAfterStop.ToString();
        }

        public void SetEnable(bool enable)
        {
            this.baseToggleButtonRun.Enabled = enable;
            this.baseToggleButtonStop.Enabled = enable;
            this.baseToggleButtonStopperUp.Enabled = enable;
            this.baseToggleButtonStopperDown.Enabled = enable;
            this.baseToggleButtonFoward.Enabled = enable;
            this.baseToggleButtonBackward.Enabled = enable;
            this.baseButtonSave.Enabled = enable;
            this.baseButtonWorkRun.Enabled = enable;
            this.baseTextBoxDelayTime.Enabled = enable;
        }

        public void SetInvisibleStopperControl()
        {
            baseGroupBoxStopper.Visible = false;
            baseToggleButtonFoward.Visible = false;
            baseToggleButtonBackward.Visible = false;
        }

        private void baseToggleButtonRun_Click(object sender, EventArgs e)
        {
            if (m_Conveyor != null)
                m_Conveyor.Run();
        }

        private void baseToggleButtonStop_Click(object sender, EventArgs e)
        {
            if (m_Conveyor != null)
                m_Conveyor.Stop();
        }

        private void baseToggleButtonFoward_Click(object sender, EventArgs e)
        {
            if (m_Conveyor != null)
                m_Conveyor.SetDirection(Conveyor.Direction.Forword);
        }

        private void baseToggleButtonBackWard_Click(object sender, EventArgs e)
        {
            if (m_Conveyor != null)
                m_Conveyor.SetDirection(Conveyor.Direction.Backword);
        }

        private void baseToggleButtonStopperUp_Click(object sender, EventArgs e)
        {
            if(m_Conveyor != null && m_Conveyor.Stopper != null)
                m_Conveyor.Stopper.BeginUP();
        }

        private void baseToggleButtonStopperDown_Click(object sender, EventArgs e)
        {
            if (m_Conveyor != null && m_Conveyor.Stopper != null)
                m_Conveyor.Stopper.BeginDown();
        }

        private void baseButtonWorkRun_Click(object sender, EventArgs e)
        {
            int nDelay = 0;
            if(int.TryParse(baseTextBoxDelayTime.Text, out nDelay))
            {
                if(m_Conveyor != null)
                {
                    m_Conveyor.Config.DelayDetectAfterStop = nDelay;
                    Task<int> task = m_Conveyor.BeginWork();
                    ProgressForm progressForm = new ProgressForm("Conveyor", "Working...", task);
                    progressForm.StopProcess += ProgressForm_StopProcess;
                    progressForm.ShowDialog();

                    m_Conveyor.Stop();
                }
                
            }
            
        }

        private void ProgressForm_StopProcess(object target)
        {
            m_Conveyor.Stop();
        }

        private void baseButtonSave_Click(object sender, EventArgs e)
        {
            if(ButtonClick != null)
            { 
                UpdateData();
                ButtonClick(ButtonType.Save);
            }
        }
        private void UpdateData()
        {
            m_Conveyor.Config.DelayDetectAfterStop = Convert.ToInt32(baseTextBoxDelayTime.Text);
        }

        public void UpdateStatus()
        {
            UpdateStopperStatus();
            UpdateDirectionStatus();
            UpdateRunButtonStatus();
        }
        protected void UpdateStopperStatus()
        {
            if (m_Conveyor == null || m_Conveyor.Stopper == null)
                return;

            if (m_Conveyor.Stopper.IsUp())
            {
                baseToggleButtonStopperUp.UpdateToggleStatus(true);
                baseToggleButtonStopperDown.UpdateToggleStatus(false);
            }
            else
            {
                baseToggleButtonStopperUp.UpdateToggleStatus(false);
                baseToggleButtonStopperDown.UpdateToggleStatus(true);
            }
        }

        protected void UpdateDirectionStatus()
        {
            if (m_Conveyor.IsDirection())
            {
                baseToggleButtonFoward.UpdateToggleStatus(true);
                baseToggleButtonBackward.UpdateToggleStatus(false);
            }
            else
            {
                baseToggleButtonFoward.UpdateToggleStatus(false);
                baseToggleButtonBackward.UpdateToggleStatus(true);
            }
        }
        public void UpdateRunButtonStatus()
        {
            if (m_Conveyor.IsRun())
            {
                baseToggleButtonRun.UpdateToggleStatus(true);
                baseToggleButtonStop.UpdateToggleStatus(false);
            }
            else
            {
                baseToggleButtonRun.UpdateToggleStatus(false);
                baseToggleButtonStop.UpdateToggleStatus(true);
            }
        }

        public void SetDirectionStatus(bool bOn)
        {
            if (bOn)
            {
                baseToggleButtonFoward.UpdateToggleStatus(true);
                baseToggleButtonBackward.UpdateToggleStatus(false);
                m_Conveyor.SetDirection(Conveyor.Direction.Forword);
            }
            else
            {
                baseToggleButtonFoward.UpdateToggleStatus(false);
                baseToggleButtonBackward.UpdateToggleStatus(true);
                m_Conveyor.SetDirection(Conveyor.Direction.Backword);
            }
        }
    }
}
