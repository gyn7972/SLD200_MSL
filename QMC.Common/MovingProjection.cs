using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    public class MovingProjection
    {
        private double m_Position;

        private double m_Velocity;

        private double m_Acceleration;

        private double m_Deceleration;

        private double m_dTimeout;

        [Browsable(false)]
        public double Position
        { 
            get { return m_Position; } 
            set { m_Position = value; } 
        }

        public double Velocity
        {
            get { return m_Velocity; }
            set { m_Velocity = value; }
        }

        public double Acceleration
        {
            get { return m_Acceleration; }
            set { m_Acceleration = value; }
        }

        public double Deceleration
        {
            get { return m_Deceleration; }
            set { m_Deceleration = value; }
        }

        public double Timeout
        {
            get { return m_dTimeout; }
            set { m_dTimeout = value; }
        }
        public MovingProjection()
        {
            m_Position = 0;
            m_Velocity = 0;
            m_Acceleration = 0;
            m_Deceleration= 0;
            Timeout = 5000;
        }

        public MovingProjection(double dPosition, double dVelocity, double dAcceleration, double dDeceleration)
        {
            m_Position = dPosition;
            m_Velocity = dVelocity;
            m_Acceleration = dAcceleration;
            m_Deceleration = dDeceleration;
            Timeout = 5000;
        }
    }
}
