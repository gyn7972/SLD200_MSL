using QMC.Common.Hmi;
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
    public class DieLoaderRecipe
    {
        [Serializable]
        [TypeConverter(typeof(NormalExpandableObjectConverter))]
        public class RecipeWorkingArea
        {
            public double Width { get; set; }
            public double Height { get; set; }

            public RecipeWorkingArea()
            {
                Width = 0;
                Height = 0;
            }
        }

        [Serializable]
        [TypeConverter(typeof(NormalExpandableObjectConverter))]
        public class RecipeSubMaterial
        {
            public double Width { get; set; }
            public double Height { get; set; }

            public SizeD Size
            {
                get 
                { 
                    return new SizeD(Width, Height); 
                }
                set 
                { 
                    Width = value.Width; 
                    Height = value.Height; 
                }
            }
            public double PitchX { get; set; }
            public double PitchY { get; set; }

            public RecipeSubMaterial()
            {
                Width = 0.14;
                Height = 0.1;
                PitchX = 0.3;
                PitchY = 0.2;
            }
        }
        [ReadOnly(true)]
        [TypeConverter(typeof(RecipeWorkingArea))]
        public RecipeWorkingArea WorkingArea { get; set; }
        [ReadOnly(true)]
        [TypeConverter(typeof(RecipeSubMaterial))]
        public RecipeSubMaterial SubMaterial { get; set; }

        [Browsable(false)]
        public RealTimeScannerRecipe ScannerRecipe { get; set; }
        [Browsable(false)]
        public IlluminationDataList IlluminationDataSets { get; set; }
        [Browsable(false)]
        public VisionCalibratorRecipe VisionCalibratorRecipe { get; set; }
        public ColletXYPositionCalibratorRecipe ColletXYPositionCalibratorRecipe { get; set; }

        public XyztCoordinate StartPosition { get; set; }
        public DieLoaderRecipe(DieLoader dieLoader)
        {
            Init(dieLoader);
        }

        public void Init(DieLoader dieLoader)
        {
            if(WorkingArea == null)
                WorkingArea = new RecipeWorkingArea();
            
            if (SubMaterial == null)
                SubMaterial = new RecipeSubMaterial();
            
            if (ScannerRecipe == null)
                ScannerRecipe = new RealTimeScannerRecipe(dieLoader.Scanner);
            
            if (IlluminationDataSets == null)
                IlluminationDataSets = new IlluminationDataList();
            
            if (VisionCalibratorRecipe == null)
                VisionCalibratorRecipe = new VisionCalibratorRecipe(dieLoader.VisionCalibrator);

            if (ColletXYPositionCalibratorRecipe == null)
                ColletXYPositionCalibratorRecipe = new ColletXYPositionCalibratorRecipe(dieLoader.XYCalibrator);
        }
        public void UpdateIlluminationData(Part part)
        {
            if(part is RealTimeScanner)
            {
                IlluminationDataSet illuminationDataSet = IlluminationDataSets.GetIlluminationDataSet(part.Name);
                if(illuminationDataSet != null)
                {
                    ScannerRecipe.IlluminationDataSet = illuminationDataSet;
                }
                else
                {
                    if (ScannerRecipe.IlluminationDataSet == null)
                        ScannerRecipe.IlluminationDataSet = new IlluminationDataSet(part.Name);
                    IlluminationDataSets.Add(ScannerRecipe.IlluminationDataSet);
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
        }


    }
}
