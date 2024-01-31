using QMC.Common;
using QMC.Common.Motion.Ajin.Motions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CWA150SA_Onsemi300
{
    public partial class FormAxisMotion : Form
    {
        private AjinAxlAxis m_Axis;
        private System.Windows.Forms.Timer m_timer;
        private Thread m_ThreadRepeat;
        private bool m_bRepeatStop;
        public FormAxisMotion(AjinAxlAxis axis)
        {
            InitializeComponent();

            m_Axis = axis;
            m_bRepeatStop = false;
            m_timer = new System.Windows.Forms.Timer();
            m_timer.Tick += timer_Tick;
            m_timer.Interval = 100;
            m_timer.Start();

            //this.Text = string.Format("Board : {0}, Axis : {1}", m_Axis.BoardNo, m_Axis.No);
        }

        

        private void UpdateUI(MotorState motor)
        {
            TextBoxAmpOn.BackColor = motor.IsAmpEnable ? Color.FromArgb(0, 255, 0) : SystemColors.ScrollBar;
            TextBoxAmpFault.BackColor = motor.IsAmpFault ? Color.FromArgb(0, 255, 0) : SystemColors.ScrollBar;
            TextBoxInPosition.BackColor = motor.IsInPosition ? Color.FromArgb(0, 255, 0) : SystemColors.ScrollBar;
            TextBoxMotionDone.BackColor = motor.IsMotionDone ? Color.FromArgb(0, 255, 0) : SystemColors.ScrollBar;
            TextBoxNegativeLimit.BackColor = motor.IsNegativeLimit ? Color.FromArgb(0, 255, 0) : SystemColors.ScrollBar;
            TextBoxPositiveLimit.BackColor = motor.IsPositiveLimit ? Color.FromArgb(0, 255, 0) : SystemColors.ScrollBar;
            TextBoxStateHome.BackColor = motor.IsHome ? Color.FromArgb(0, 255, 0) : SystemColors.ScrollBar;
            TextBoxState.Text = motor.MotionState.ToString();
            TextBoxCommandPosition.Text = motor.CommandPosition.ToString();
            TextBoxActualPosition.Text = motor.ActualPosition.ToString();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            UpdateUI(m_Axis.Motor);
            
        }

        private void ButtonStateReset_Click(object sender, EventArgs e)
        {
            StopRepeatThread();
            m_Axis.Reset();
        }

        private void ButtonAmpOff_Click(object sender, EventArgs e)
        {
            StopRepeatThread();
            m_Axis.SetEnable(!m_Axis.Motor.IsAmpEnable);   
        }

        private void ButtonAmpClear_Click(object sender, EventArgs e)
        {
            StopRepeatThread();
            m_Axis.Clear();
        }

        private void ButtonSetPosition_Click(object sender, EventArgs e)
        {
            StopRepeatThread();
            m_Axis.SetPosition(0);
        }

        private void ButtonHomingStart_Click(object sender, EventArgs e)
        {
            StopRepeatThread();
            m_Axis.Homing();
        }

        private void ButtonStop_Click(object sender, EventArgs e)
        {
            StopRepeatThread();
            m_Axis.Stop();            
        }

        private void StopEmergency_Click(object sender, EventArgs e)
        {
            StopRepeatThread();
            m_Axis.StopEmergency();
        }

        private void ButtonPositiveStep_Click(object sender, EventArgs e)
        {
            StopRepeatThread();
            double dStepUnit = double.Parse(TextBoxPositionMove.Text);
            double dCurrent = m_Axis.Motor.CommandPosition;

            m_Axis.MovePosition(dCurrent + dStepUnit);
        }

        private void ButtonNegativeStep_Click(object sender, EventArgs e)
        {
            StopRepeatThread();
            double dStepUnit = double.Parse(TextBoxPositionMove.Text);
            double dCurrent = m_Axis.Motor.CommandPosition;

            m_Axis.MovePosition(dCurrent - dStepUnit);
        }

        private void ButtonNegative_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                StopRepeatThread();
                double dVelocity = double.Parse(TextBoxVelocityMove.Text) * -1;
                double dAccel = double.Parse(TextBoxAccDecMove.Text);
                m_Axis.MoveVelocity(dVelocity, dAccel, dAccel);
            }
        }

        private void ButtonNegative_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                double dDeccel = double.Parse(TextBoxAccDecMove.Text);
                m_Axis.Stop(dDeccel);
            }
        }

        private void ButtonPositive_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                StopRepeatThread();
                double dVelocity = double.Parse(TextBoxVelocityMove.Text);
                double dAccel = double.Parse(TextBoxAccDecMove.Text);
                m_Axis.MoveVelocity(dVelocity, dAccel, dAccel);
            }
        }

        private void ButtonPositive_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                double dDeccel = double.Parse(TextBoxAccDecMove.Text);
                m_Axis.Stop(dDeccel);
            }
        }

        private void ButtonMove_Click(object sender, EventArgs e)
        {
            StopRepeatThread();
            double dTargetPosition = double.Parse(TextBoxPositionMove.Text);
            double dVelocity = double.Parse(TextBoxVelocityMove.Text);
            double dAccel = double.Parse(TextBoxAccDecMove.Text);
            m_Axis.MovePosition(dTargetPosition, dVelocity, dAccel, dAccel);
        }

        private void ButtonModify_Click(object sender, EventArgs e)
        {
            StopRepeatThread();
            double dTargetPosition = double.Parse(TextBoxPositionMove.Text);
            double dVelocity = double.Parse(TextBoxVelocityMove.Text);
            double dAccel = double.Parse(TextBoxAccDecMove.Text);
            m_Axis.ModifyPosition(dTargetPosition, dVelocity, dAccel, dAccel);
        }

        private void ButtonMoveVelocity_Click(object sender, EventArgs e)
        {
            StopRepeatThread();
            double dVelocity = double.Parse(TextBoxVelocityMove.Text);
            double dAccel = double.Parse(TextBoxAccDecMove.Text);
            m_Axis.MoveVelocity(dVelocity, dAccel, dAccel);
        }

        private void ButtonModifyVeloity_Click(object sender, EventArgs e)
        {
            StopRepeatThread();
            double dVelocity = double.Parse(TextBoxVelocityMove.Text);
            double dAccel = double.Parse(TextBoxAccDecMove.Text);
            m_Axis.ModifyVelocity(dVelocity, dAccel, dAccel);
        }

        private void ButtonMoveDistance_Click(object sender, EventArgs e)
        {
            StopRepeatThread();
            double dDistance = double.Parse(TextBoxPositionMove.Text);
            double dVelocity = double.Parse(TextBoxVelocityMove.Text);
            double dAccel = double.Parse(TextBoxAccDecMove.Text);
            m_Axis.MoveDistance(dDistance, dVelocity, dAccel, dAccel);
        }

        private void ButtonRepaet_Click(object sender, EventArgs e)
        {
            StopRepeatThread();
            double dPosition = double.Parse(TextBoxPositionMove.Text);
            double dVelocity = double.Parse(TextBoxVelocityMove.Text) * -1;
            double dAccel = double.Parse(TextBoxAccDecMove.Text);

            m_ThreadRepeat = new Thread(() =>
            {
                bool bDirection = true;
                double dOriginPosition = m_Axis.Motor.ActualPosition;
                while(true)
                {
                    if (m_bRepeatStop)
                        break;
                    if(bDirection)
                    {
                        m_Axis.MovePosition(dPosition, dVelocity, dAccel, dAccel);
                    }
                    else
                    {
                        m_Axis.MovePosition(dOriginPosition, dVelocity, dAccel, dAccel);
                    }

                    Thread.Sleep(1000);
                    while(true)
                    {
                        if (m_bRepeatStop)
                            return;
                        if (m_Axis.Motor.IsInPosition && m_Axis.Motor.IsMotionDone)
                            break;
                        Thread.Sleep(100);
                    }
                }
            });
            m_ThreadRepeat.Start();
        }

        private void StopRepeatThread()
        {
            if (m_ThreadRepeat != null)
            {
                m_bRepeatStop = true;
                m_ThreadRepeat.Join();
            }
        }
    }
}
