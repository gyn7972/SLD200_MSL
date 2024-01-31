using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    [Serializable]
    public class MotorState
    {
        public MotorState() : this(string.Empty) { }

        public MotorState(int nNo) : this(nNo.ToString()) { }

        public MotorState(string strName)
        {
            this.Name = strName;
        }

        public string Name { set; get; }
        public double NegativePosition { set; get; }
        public double PositivePosition { set; get; }
        public double CommandPosition { set; get; }
        public double ActualPosition { set; get; }        
        public bool IsAmpEnable { set; get; }
        public bool IsAmpFault { set; get; }
        public bool IsHome { set; get; }
        public bool IsPositiveLimit { set; get; }
        public bool IsNegativeLimit { set; get; }
        public bool IsInPosition { set; get; }
        public bool IsMotionDone { set; get; }

        public AxisState MotionState { set; get; }

        public bool CheckLimit(double dPosition)
        {
            bool bRet = false;
            if(NegativePosition < dPosition && PositivePosition > dPosition)
            {
                bRet = true;
            }

            return bRet;
        }
    }
}
