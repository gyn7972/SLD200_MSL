using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    [Serializable]
    public class LPMDispenserCPKConfig
    {
        #region Define
        public enum InspectItem
        {
            Rivet1,
            Rivet2,
            EtchingWidth,
            EtchingHeight,
        }
        #endregion

        #region Property
        public double[] USL;
        public double[] LSL;
        #endregion

        #region Constructor
        public LPMDispenserCPKConfig()
        {
            int measureCount = Enum.GetValues(typeof(InspectItem)).Length;

            USL = new double[measureCount];
            LSL = new double[measureCount];

            Init();
        }
        #endregion

        #region Method
        public void Init()
        {

        }
        #endregion
    }

    [Serializable]
    public class LPMMounterCPKConfig
    {
        #region Define
        public enum InspectItem
        {
          MounterShiftX,
          MounterShiftY
        }
        #endregion

        #region Property
        public double[] USL;
        public double[] LSL;
        #endregion

        #region Constructor
        public LPMMounterCPKConfig()
        {
            int measureCount = Enum.GetValues(typeof(InspectItem)).Length;

            USL = new double[measureCount];
            LSL = new double[measureCount];

            Init();
        }
        #endregion

        #region Method
        public void Init()
        {

        }
        #endregion
    }
}
