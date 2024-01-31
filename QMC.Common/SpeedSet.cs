using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    public class SpeedSet
    {
        public string Name { set; get; }
        public double Velocity { set; get; }
        public double Acceleration { set; get; }
        public double Deceleration { set; get; }

        public SpeedSet()
        {
            Name = "";
            Velocity = 0; 
            Acceleration = 0; 
            Deceleration = 0; 
        }
    }
}
