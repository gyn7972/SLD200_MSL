using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    [Serializable]
    public class CalibrationPitch
    {
        public double pitchZ { get; set; }
        public CalibrationPitch()
        {
            pitchZ = 1;
        }
    }
}
