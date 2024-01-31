using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Motion.ACS.Motion
{
    public class AxisParam
    {
        public double Velocity { get; set; }
        public double Acceleration { get; set; }
        public double Deceleration { get; set; }
        public double KillDeceleration { get; set; }
        public double Jerk { get; set; }

        public AxisParam()
        {
            Velocity = 0.0;
            Acceleration = 0.0;
            Deceleration = 0.0;
            KillDeceleration = 0.0;
            Jerk = 0.0;
        }
    }
}
