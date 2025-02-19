using QMC.Common.VisionPart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Modules
{
    [Serializable]
    public class DieTransferRecipe
    {
        //public ColletXYPositionCalibratorRecipe XYCalibratorRecipe { get; set; }
        public DieSearcherRecipe DieSearcherRecipe { get; set; }
        public IlluminationDataList IlluminationDataSets { get; set; }

        public DieTransferRecipe(DieTransfer dieTransfer)
        {
            init(dieTransfer);
        }
        public void UpdateIlluminationData(Part part)
        {
            if (part is DieSearcher)
            {
                IlluminationDataSet illuminationDataSet = IlluminationDataSets.GetIlluminationDataSet(part.Name);
                if (illuminationDataSet != null)
                {
                    DieSearcherRecipe.IlluminationDataSet = illuminationDataSet;
                }
                else
                {
                    if (DieSearcherRecipe.IlluminationDataSet == null)
                        DieSearcherRecipe.IlluminationDataSet = new IlluminationDataSet(part.Name);
                    IlluminationDataSets.Add(DieSearcherRecipe.IlluminationDataSet);
                }
            }
            else { }
        }
        public void init(DieTransfer dieTransfer)
        {
            if(IlluminationDataSets == null)
                IlluminationDataSets = new IlluminationDataList();
            //if (XYCalibratorRecipe == null)
            //    XYCalibratorRecipe = new ColletXYPositionCalibratorRecipe(dieTransfer.PositionCalibrator);
            if (DieSearcherRecipe == null)
                DieSearcherRecipe = new DieSearcherRecipe(dieTransfer.DieSearcher);

        }
    }
}
