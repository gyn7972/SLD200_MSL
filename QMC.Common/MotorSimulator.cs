using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QMC.Common
{
    public class MotorSimulator
    {
        private Thread m_Monitor;
        private double m_dTargetPosition;
        private double m_dVelocity;
        private MotionAxis.FunctionID m_functionID;
        private bool m_bExit;
        private DateTime m_StartTime;
        public MotorState MotorState { set; get; }
        public MotorSimulator()
        {
            m_Monitor = null;
            m_bExit = false;
        }
        
        public int MovePosition(double dPosition, double dVelocity, double dAcceleration, double dDeceleration)
        {
            int ret = 0;

            if (m_Monitor != null && m_Monitor.IsAlive)
            {
                //return -1;
                m_bExit = true;
                m_Monitor.Join();
            }
                
            m_dTargetPosition = dPosition;
            MotorState.CommandPosition = dPosition;
            m_dVelocity = dVelocity;
            m_functionID = MotionAxis.FunctionID.MovePosition;
            m_bExit = false;
            m_Monitor = new Thread(() =>
            {
                try
                {
                    RunProc();
                }
                catch(Exception ex)
                {
                    Log.Write(ex);
                }
                
            });

            m_Monitor.Start();
            return ret;
        }

        public int MoveVelocity(double dVelocity, double dAcceleration, double dDeceleration)
        {
            int ret = 0;
            if (m_Monitor != null && m_Monitor.IsAlive)
                return -1;
            m_functionID = MotionAxis.FunctionID.MoveVelocity;
            m_dVelocity = dVelocity;
            m_bExit = false;
            m_Monitor = new Thread(() =>
            {
                try
                {
                    RunProc();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            });

            m_Monitor.Start();
            return ret;
        }

        public int Stop()
        {
            int ret = 0;

            //m_Monitor.Abort();

            return ret;
        }

        public int WaitDone()
        {
            int ret = 0;

            m_Monitor.Join();

            return ret;
        }

        private void RunProc()
        {
            m_StartTime = DateTime.Now;
            while (true)
            {
                if (m_bExit)
                    break;

                switch(m_functionID)
                {
                    case MotionAxis.FunctionID.MovePosition:
                        m_bExit = OnMovePosition();
                        break;
                    case MotionAxis.FunctionID.MoveVelocity:
                        m_bExit = OnMoveVelocity();
                        break;
                    default:
                        m_bExit = true;
                        break;
                }

                Thread.Sleep(10);
            }

        }

        private bool OnMovePosition()
        {
            bool bRet = false;

            TimeSpan durationTime = DateTime.Now - m_StartTime;
            double dPosition = MotorState.ActualPosition + (m_dVelocity * durationTime.TotalMilliseconds / 1000);

            if(m_dTargetPosition <= dPosition)
            {
                dPosition = m_dTargetPosition;
                bRet = true;
            }

            MotorState.ActualPosition = dPosition;

            return bRet;
        }

        private bool OnMoveVelocity()
        {
            bool bRet = false;

            TimeSpan durationTime = DateTime.Now - m_StartTime;
            MotorState.ActualPosition += m_dVelocity * durationTime.TotalMilliseconds / 1000;

            return bRet;
        }
    }
}
