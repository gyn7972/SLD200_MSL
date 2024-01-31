using QMC.Common.Vision;
using QMC.Common.Vision.Cameras;
using QMC.Common.Vision.Cognex;
using QMC.Common.Vision.Optics;
using QMC.Common.Vision.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.VisionPart
{
    public delegate void UpdatePatternMatchingResult(PatternMatchingResult result);
    [Serializable]
    public class VisionPart : Part
    {
        
        public Camera Camera { set; get; }
        public Illuminator Illuminator { set; get; }
       
        public event UpdatePatternMatchingResult UpdateResult;
        
        public VisionPart(string strName) : base(strName)
        {
            
        }

        public override int Create()
        {
            int ret = 0;

            return ret;
        }

        public override void Close()
        {
            base.Close();
        }

        protected int OnSetIllumination(IlluminationDataSet illuminationDataSet, bool bOn)
        {
            int ret = 0;
            if (Illuminator == null)
            {
                return -1;
            }

            foreach (IlluminationChannel illumination in illuminationDataSet.Values)
            {
                if (bOn)
                {
                    if ((ret = Illuminator.TurnOnOff(true, illumination.Channel)) != 0) return ret;
                    if ((ret = Illuminator.SetVolume(illumination.Value, illumination.Channel)) != 0) return ret;
                }
                else
                {
                    if ((ret = Illuminator.TurnOnOff(false, illumination.Channel)) != 0) return ret;
                }

            }

            return ret;
        }

        public void FireUpdateResult(PatternMatchingResult result)
        {
            UpdateResult?.BeginInvoke(result, null, null);
        }

    }
}
