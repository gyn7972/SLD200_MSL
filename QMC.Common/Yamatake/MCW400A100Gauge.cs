using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Yamatake
{
    public class MCW400A100Gauge : Part
    {
        public int Channel { set; get; }
        public MCW400A100GaugeCommunicator Communicator { set; get; }

        public MCW400A100Gauge(string strName) : base(strName)
        {
        }

        public override int Create()
        {
            int ret = 0;

            return ret;
        }

        public override int Initialize()
        {
            int ret = 0;
            if (Communicator != null && !Communicator.IsOpen)
            {
                Communicator.Open();
            }

            return ret;
        }

        public override void Close()
        {
            if(Communicator != null && Communicator.IsOpen)
                Communicator.Close();
            base.Close();
        }

        public int GetPresentValue(ref double dPresentValue)
        {
            int ret = 0;

            if(Communicator == null || !Communicator.IsOpen)
            {
                //Open Error
                return -1;
            }

            ret = Communicator.Query(Channel, ref dPresentValue);

            return ret;
        }

        public int SetZero(int nChannel)
        {
            int ret = 0;
            if(Communicator == null || !Communicator.IsOpen)
            {
                return -1;
            }

            ret = Communicator.SetZero(nChannel);

            return ret;
        }
    }
}
