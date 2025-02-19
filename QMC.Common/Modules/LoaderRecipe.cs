using QMC.Common.Hmi;
using QMC.Common.Parts;
using QMC.Common.VisionPart;
using QMC.Process.WorkStage.Parts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Modules
{
    [Serializable]
    public class LoaderRecipe
    {
        #region RecipeWorkingArea
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
        #endregion

        #region RecipeSubMaterial
        [Serializable]
        [TypeConverter(typeof(NormalExpandableObjectConverter))]
        public class RecipeSubMaterial
        {
            public double Width { get; set; }
            public double Height { get; set; }
            public double PitchX { get; set; }
            public double PitchY { get; set; }

            public RecipeSubMaterial()
            {
                Width = 0;
                Height = 0;
                PitchX = 0;
                PitchY = 0;
            }
        }
        #endregion

        [ReadOnly(true)]
        [TypeConverter(typeof(RecipeWorkingArea))]
        public RecipeWorkingArea WorkingArea { get; set; }
        [ReadOnly(true)]
        [TypeConverter(typeof(RecipeSubMaterial))]
        public RecipeSubMaterial SubMaterial { get; set; }
        public VisionCalibratorRecipe VisionCalibratorRecipe_HighRes { set; get; }
        public VisionCalibratorRecipe VisionCalibratorRecipe_LowRes { set; get; }

        public RealTimeScannerRecipe ScannerRecipe { get; set; }
        //public ScannerCompensatorRecipe scannerCompensatorRecipe { set; get; }

        public JigAlignerRecipe jigAlignerRecipe_HighRes { set; get; }
        public JigAlignerRecipe jigAlignerRecipe_LowRes { set; get; }
        public JigAlignerRecipe reticleAlignerRecipe_HighRes { set; get; }
        public JigAlignerRecipe reticleAlignerRecipe_LowRes { set; get; }

        public LoaderRecipe(Loader loader)
        {
            //  요거 두개는 없어도 될 듯
            WorkingArea = new RecipeWorkingArea();
            SubMaterial = new RecipeSubMaterial();

            ScannerRecipe = new RealTimeScannerRecipe(loader);

            Init(loader);
        }

        public void Init(Loader loader)
        {
            /*
            if (AlignerRecipe == null)
            {
                visionCalibratorRecipe = new VisionCalibratorRecipe(workStage.visionCalibrator);
            }
            if (MaterialCompansatorRecipe == null)
            {
                MaterialCompansatorRecipe = new MaterialCompansatorRecipe(dieUnloader.MarterialCompansator);
            }
            if (ColletXYPositionCalibratorRecipe == null)
            {
                ColletXYPositionCalibratorRecipe = new ColletXYPositionCalibratorRecipe(dieUnloader.XYCalibrator);
            }
            if (IlluminationDataSets == null)
            {
                IlluminationDataSets = new IlluminationDataList();
            }

            if (DieFinderRecipe == null)
            {
                DieFinderRecipe = new DieFinderRecipe(dieUnloader.DieFinder);
            }
            StartPosition = new XyztCoordinate();
            if (PathParameter == null)
            {
                PathParameter = new UnLoaderPathParameter();
            }
            */
        }

        public void SetData(SettingParameterCollection parameters)
        {
            int i = 0;
            WorkingArea.Width = parameters[i++].DoubleValue;
            WorkingArea.Height = parameters[i++].DoubleValue;

            SubMaterial.Width = parameters[i++].DoubleValue;
            SubMaterial.Height = parameters[i++].DoubleValue;
            SubMaterial.PitchX = parameters[i++].DoubleValue;
            SubMaterial.PitchY = parameters[i++].DoubleValue;
        }

        public SettingParameterCollection GetData()
        {
            SettingParameterCollection parameters = new SettingParameterCollection();

            {
                SettingParameter parameter = new SettingParameter();
                parameter.Name = "WorkingArea.Width";
                parameter.DoubleValue = WorkingArea.Width;
                parameters.Add(parameter);
            }

            {
                SettingParameter parameter = new SettingParameter();
                parameter.Name = "WorkingArea.Height";
                parameter.DoubleValue = WorkingArea.Height;
                parameters.Add(parameter);
            }

            {
                SettingParameter parameter = new SettingParameter();
                parameter.Name = "SubMaterial.Width";
                parameter.DoubleValue = SubMaterial.Width;
                parameters.Add(parameter);
            }

            {
                SettingParameter parameter = new SettingParameter();
                parameter.Name = "SubMaterial.Height";
                parameter.DoubleValue = SubMaterial.Height;
                parameters.Add(parameter);
            }

            {
                SettingParameter parameter = new SettingParameter();
                parameter.Name = "SubMaterial.PitchX";
                parameter.DoubleValue = SubMaterial.PitchX;
                parameters.Add(parameter);
            }

            {
                SettingParameter parameter = new SettingParameter();
                parameter.Name = "SubMaterial.PitchY";
                parameter.DoubleValue = SubMaterial.PitchY;
                parameters.Add(parameter);
            }


            return parameters;
        }
    }
}
