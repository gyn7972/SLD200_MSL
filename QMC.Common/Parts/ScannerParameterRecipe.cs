using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace QMC.Process.WorkStage.Parts
namespace QMC.Common.Parts
{
    [Serializable]
    public class ScannerParameterRecipe
    {
        public string DocumentPath { set; get; }

        [Category("Laser Power")]
        public float Frequency { set; get; }
        [Category("Laser Power")]
        public float PulseWidth { set; get; }
        [Category("Laser Power")]
        public float Power { set; get; }

        public ScannerParameterRecipe()
        {
            //Test
            DocumentPath = @"D:\Temp\laser.sirius";
            Frequency = 100;
            PulseWidth = 2;
            Power = 150;

        }
    }
}
