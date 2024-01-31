using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Parts
{
    [Serializable]
    public class ConveyorConfig
    {
        [Category("Stopper")]
        public int ResponseTimeout { set; get; }

        [Category("Clamper")]
        public int ClamperResponseTimeout { set; get; }

        [Category("Conveyor")]
        public bool EnableStopper { set; get; }

        [Category("Conveyor")]
        public bool EnableClamper { set; get; }

        [Category("Conveyor")]
        public DioValue PositiveDirection { set; get; }

        [Category("Conveyor")]
        public int DelayDetectAfterStop { set; get; }

        [Category("Conveyor")]
        public int DetectCarrierTimeOut { set; get; }
       

        public ConveyorConfig()
        {
            ResponseTimeout = 0;
            ClamperResponseTimeout = 0;
            EnableStopper = false;
            EnableClamper = false;
            PositiveDirection = DioValue.On;
            DelayDetectAfterStop = 100;
            DetectCarrierTimeOut = 10000;
        }
    }
}
