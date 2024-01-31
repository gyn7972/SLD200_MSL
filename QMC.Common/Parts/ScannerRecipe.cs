using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Process.WaferProbeAlign.Parts
{
    [Serializable]
    public class ScannerRecipe
    {
        public string DocumentPath { set; get; }

        [Category("Laser Power")]
        public float Frequency { set; get; }
        [Category("Laser Power")]
        public float PulseWidth { set; get; }
        [Category("Laser Power")]
        public float Power { set; get; }

        public ScannerRecipe()
        {
            //Test
            DocumentPath = @"D:\Temp\laser.sirius";
            Frequency = 100;
            PulseWidth = 2;
            Power = 150;

        }
    }
}
