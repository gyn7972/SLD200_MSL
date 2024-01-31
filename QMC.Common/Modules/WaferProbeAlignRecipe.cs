using QMC.Common.Hmi;
using QMC.Common.Parts;
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
    public class WaferProbeAlignRecipe
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
        public VisionCalibratorRecipe VisionCalibratorRecipe_Upper { set; get; }
        public VisionCalibratorRecipe VisionCalibratorRecipe_Lower { set; get; }

        //public ScannerCompensatorRecipe scannerCompensatorRecipe { set; get; }

        public JigAlignerRecipe jigAlignerRecipe_Upper { set; get; }
        public JigAlignerRecipe jigAlignerRecipe_Lower { set; get; }

        public WaferProbeAlignRecipe(WaferProbeAlign waferProbeAlign)
        {
            //  요거 두개는 없어도 될 듯
            WorkingArea = new RecipeWorkingArea();
            SubMaterial = new RecipeSubMaterial();

            Init(waferProbeAlign);
        }

        public void Init(WaferProbeAlign waferProbeAlign)
        {
            if (VisionCalibratorRecipe_Upper == null)
            {
                VisionCalibratorRecipe_Upper = new VisionCalibratorRecipe(waferProbeAlign.visionCalibrator_Upper);
            }
            VisionCalibratorRecipe_Upper.Init(waferProbeAlign.visionCalibrator_Upper);

            if (VisionCalibratorRecipe_Lower == null)
            {
                VisionCalibratorRecipe_Lower = new VisionCalibratorRecipe(waferProbeAlign.visionCalibrator_Lower);
            }
            VisionCalibratorRecipe_Lower.Init(waferProbeAlign.visionCalibrator_Lower);

            //if (scannerCompensatorRecipe == null)
            //{
            //    scannerCompensatorRecipe = new ScannerCompensatorRecipe(waferProbeAlign.scannerCompensator);
            //}

            //scannerCompensatorRecipe.Init(waferProbeAlign.scannerCompensator);

            if (jigAlignerRecipe_Upper == null)
            {
                jigAlignerRecipe_Upper = new JigAlignerRecipe(waferProbeAlign.jigAligner_Upper);
            }
            jigAlignerRecipe_Upper.Init(waferProbeAlign.jigAligner_Upper);

            if (jigAlignerRecipe_Lower == null)
            {
                jigAlignerRecipe_Lower = new JigAlignerRecipe(waferProbeAlign.jigAligner_Lower);
            }
            jigAlignerRecipe_Lower.Init(waferProbeAlign.jigAligner_Lower);

            /*
            if (AlignerRecipe == null)
            {
                visionCalibratorRecipe = new VisionCalibratorRecipe(waferProbeAlign.visionCalibrator);
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
