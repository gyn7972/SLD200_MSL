using QMC.Common.VisionPart;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Modules
{
    [Serializable]
    public class DieUnloaderRecipe
    {
        [Browsable(false)]
        public AlignerRecipe AlignerRecipe { get; set; }
        [Browsable(false)]
        public MaterialCompansatorRecipe MaterialCompansatorRecipe { get; set; }
        [Browsable(false)]
        public ColletXYPositionCalibratorRecipe ColletXYPositionCalibratorRecipe { get; set; }
        [Browsable(false)]
        public IlluminationDataList IlluminationDataSets { get; set; }
        public VisionCalibratorRecipe VisionCalibratorRecipe { get; set; }
        public DieFinderRecipe DieFinderRecipe { get; set; }
        public XyztCoordinate StartPosition { get; set; }

        public DieUnloaderRecipe(DieUnloader dieUnloader)
        {
            //AlignerRecipe = new AlignerRecipe(dieUnloader.Aligner);
            //MaterialCompansatorRecipe = new MaterialCompansatorRecipe(dieUnloader.MarterialCompansator); 
            //ColletXYPositionCalibratorRecipe = new ColletXYPositionCalibratorRecipe(dieUnloader.ColletCalibrator);
            //StartPosition = new XyztCoordinate();
            Init(dieUnloader);
        }

        public void Init(DieUnloader dieUnloader)
        {
            if (AlignerRecipe == null)
            {
                AlignerRecipe = new AlignerRecipe(dieUnloader.Aligner);
            }
            if (MaterialCompansatorRecipe == null)
            {
                MaterialCompansatorRecipe = new MaterialCompansatorRecipe(dieUnloader.MarterialCompansator);
            }
            if (ColletXYPositionCalibratorRecipe == null)
            {
                ColletXYPositionCalibratorRecipe = new ColletXYPositionCalibratorRecipe(dieUnloader.XYCalibrator);
            }
            if (VisionCalibratorRecipe == null)
            {
                VisionCalibratorRecipe = new VisionCalibratorRecipe(dieUnloader.VisionCalibrator);
            }
            if(IlluminationDataSets == null)
            {
                IlluminationDataSets = new IlluminationDataList();
            }

            if(DieFinderRecipe == null)
            {
                DieFinderRecipe = new DieFinderRecipe(dieUnloader.DieFinder);
            }    

            StartPosition = new XyztCoordinate();
        }
        public void UpdateIlluminationData(Part part)
        {
            if (part is Aligner)
            {
                IlluminationDataSet illuminationDataSet = IlluminationDataSets.GetIlluminationDataSet(part.Name);
                if (illuminationDataSet != null)
                {
                    AlignerRecipe.IlluminationDataSet = illuminationDataSet;
                }
                else
                {
                    if (AlignerRecipe.IlluminationDataSet == null)
                        AlignerRecipe.IlluminationDataSet = new IlluminationDataSet(part.Name);
                    IlluminationDataSets.Add(AlignerRecipe.IlluminationDataSet);
                }
            }
            else if (part is MaterialCompansator)
            {
                IlluminationDataSet illuminationDataSet = IlluminationDataSets.GetIlluminationDataSet(part.Name);
                if (illuminationDataSet != null)
                {
                    MaterialCompansatorRecipe.IlluminationDataSet = illuminationDataSet;
                }
                else
                {
                    if (MaterialCompansatorRecipe.IlluminationDataSet == null)
                        MaterialCompansatorRecipe.IlluminationDataSet = new IlluminationDataSet(part.Name);
                    IlluminationDataSets.Add(MaterialCompansatorRecipe.IlluminationDataSet);
                }
            }
            else if (part is ColletXYPositionCalibrator)
            {
                IlluminationDataSet illuminationDataSet = IlluminationDataSets.GetIlluminationDataSet(part.Name);
                if (illuminationDataSet != null)
                {
                    ColletXYPositionCalibratorRecipe.IlluminationDataSet = illuminationDataSet;
                }
                else
                {
                    if (ColletXYPositionCalibratorRecipe.IlluminationDataSet == null)
                        ColletXYPositionCalibratorRecipe.IlluminationDataSet = new IlluminationDataSet(part.Name);
                    IlluminationDataSets.Add(ColletXYPositionCalibratorRecipe.IlluminationDataSet);
                }
            }
            else if(part is VisionCalibrator)
            {
                IlluminationDataSet illuminationDataSet = IlluminationDataSets.GetIlluminationDataSet(part.Name);
                if (illuminationDataSet != null)
                {
                    VisionCalibratorRecipe.IlluminationDataSet = illuminationDataSet;
                }
                else
                {
                    if (VisionCalibratorRecipe.IlluminationDataSet == null)
                        VisionCalibratorRecipe.IlluminationDataSet = new IlluminationDataSet(part.Name);
                    IlluminationDataSets.Add(VisionCalibratorRecipe.IlluminationDataSet);
                }


            }
        }
    }
}