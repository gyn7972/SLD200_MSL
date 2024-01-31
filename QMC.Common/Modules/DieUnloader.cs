using QMC.Common.Parts;
using QMC.Common.Vision;
using QMC.Common.Vision.Cameras;
using QMC.Common.Vision.EureSys;
using QMC.Common.Vision.HIKVISION;
using QMC.Common.VisionPart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Modules
{
    public class DieUnloader : Module
    {
        //public XytStage Stage { set; get; }
        //public XyztStage Stage { set; get; }
        //public XyzztStage Stage { set; get; }
        public UvwzxyzStage Stage { set; get; }
        public DualGripper Gripper { set; get; }        
        public Aligner Aligner { set; get; }
        public MaterialCompansator MarterialCompansator { set; get; }
        public ColletXYPositionCalibrator XYCalibrator { set; get; }
        public List<XytPositionData> Positions { set; get; }
        public DieFinder DieFinder { set; get; }
        public DieUnloaderConfig Config { set; get; }
        public DieUnloaderRecipe Recipe { set; get; }
        public VisionScale Scale
        {
            get
            {
                if (Config.VisonCalibratorConfig.Scale == null)
                    Config.VisonCalibratorConfig.Scale = new VisionScale();
                return Config.VisonCalibratorConfig.Scale;
            }
        }

        public VisionCalibrator VisionCalibrator { set; get; }

        public DieTransfer DieTransfer { set; get; }

        public bool Simulated { set; get; }

        public HIKGigECamera Camera { set; get; }
        //public GrabLinkMultiCamCamera Camera { get; set; }
        public DieUnloader(string strName) : base(strName)
        {
            
            Positions = new List<XytPositionData>();         
        }

        #region Module
        public override int Create()
        {
            int ret = base.Create();

            //Stage = new XyztStage("TargetStage");
            //Stage = new XyzztStage("TargetStage");
            Stage = new UvwzxyzStage("TargetStage");
            Stage.Create();
            Stage.Owner = this;
            Parts.Add(Stage);

            Gripper = new DualGripper("StageGripper");
            Gripper.Create();
            Gripper.Owner = this;
            Parts.Add(Gripper);
            
            Camera = new HIKGigECamera("TargetCamera");
            //Camera = new GrabLinkMultiCamCamera("TargetCamera");
            Camera.Create();
            Camera.Owner = this;
            Parts.Add(Camera);

            Aligner = new Aligner("Aligner");
            Aligner.Create();
            Aligner.Owner = this;
            Parts.Add(Aligner);

            DieFinder = new DieFinder("DieFinder");
            DieFinder.Create();
            DieFinder.Owner = this;
            Parts.Add(DieFinder);

            MarterialCompansator = new MaterialCompansator("Material Comp");
            MarterialCompansator.Create();
            MarterialCompansator.Owner = this;
            Parts.Add(MarterialCompansator);

            XYCalibrator = new ColletXYPositionCalibrator("ColletCalibrator");
            XYCalibrator.Create();
            XYCalibrator.Owner = this;
            Parts.Add(XYCalibrator);

            VisionCalibrator = new VisionCalibrator("VisionCalibrator");
            VisionCalibrator.Create();
            VisionCalibrator.Owner = this;
            Parts.Add(VisionCalibrator);
            

            Config = new DieUnloaderConfig();
            Recipe = new DieUnloaderRecipe(this);
            //Recipe.AlignerRecipe
            //Stage.Gripper = Gripper;
            Aligner.Stage = Stage;
            Aligner.Camera = Camera;
            Aligner.Recipe = Recipe.AlignerRecipe;
            Aligner.Illuminator = CommonModule.Instance.Illuminator;
            DieFinder.Camera = Camera;
            DieFinder.Recipe = Recipe.DieFinderRecipe;
            DieFinder.Illuminator = CommonModule.Instance.Illuminator;

            XYCalibrator.Illuminator = CommonModule.Instance.Illuminator;
            MarterialCompansator.Illuminator = CommonModule.Instance.Illuminator;
            MarterialCompansator.Stage = Stage;
            //MarterialCompansator.Recipe = Recipe;
            MarterialCompansator.Camera = Camera;
            XYCalibrator.Camera = Camera;
            XYCalibrator.Motion = Stage;
            Camera.Config = Config.HIKGigECameraConfig; // 참고 데이터 연동
            //Camera.Config = Config.GrabLinkMultiCamCameraConfig; // 참고 데이터 연동
            VisionCalibrator.Config = this.Config.VisonCalibratorConfig;
            MarterialCompansator.Recipe = this.Recipe.MaterialCompansatorRecipe;
            XYCalibrator.XyCalibratorRecipe = this.Recipe.ColletXYPositionCalibratorRecipe;
            XYCalibrator.XYCalibratoeConfig = this.Config.XYPositionCalibratorConfig;
            //XYCalibrator.Illuminator = CommonModule.Instance.Illuminator;
            //XYCalibrator.XYCalibratoeConfig = this.Config.
            VisionCalibrator.Camera = Camera;
            VisionCalibrator.UvwzxyzStage = Stage;
            VisionCalibrator.Illuminator = CommonModule.Instance.Illuminator;
            return ret;
        }
      
        public List<string> GetPositionList()
        {
            List<string> ret = new List<string>();
            foreach (XytPositionData position in Config.Positions)
            {
                if (position.Type == TargetType.Base)
                    ret.Add(position.Name);
            }
            return ret;
        }
        public List<XytPositionData> GetPositionListData()
        {
            List<XytPositionData> ret = new List<XytPositionData>();
            foreach (XytPositionData position in Config.Positions)
            {
                if (position.Type == TargetType.Base)
                    ret.Add(position);
            }
            return ret;
        }

        public XytCoordinate GetCurrentPosition()
        {
            XytCoordinate current = new XytCoordinate();
            if (Stage != null)
            {
                double dPos = 0;
                Stage.Axes["X"].GetActualPosition(ref dPos);
                current.X = dPos;
                Stage.Axes["Y"].GetActualPosition(ref dPos);
                current.Y = dPos;
                Stage.Axes["T"].GetActualPosition(ref dPos);
                current.T = dPos;
            }

            return current;
        }

        //public override void SetRecipeData(SettingParameterCollection parameters)
        //{
        //    Recipe.SetData(parameters);
        //    UpdateRecipeData();
        //}
        //public override SettingParameterCollection GetRecipeData()
        //{
        //    return Recipe.GetData();
        //}

        public override void SetRecipeData(object recipeData)
        {
            DieUnloaderRecipe recipe = recipeData as DieUnloaderRecipe;
            if (recipe == null)
            {
                recipe = new DieUnloaderRecipe(this);
            }

            Recipe = recipe;
            Recipe.Init(this);

            foreach (Part part in Parts)
            {
                Recipe.UpdateIlluminationData(part);
            }

            base.SetRecipeData(recipeData);
        }
        public override object GetRecipeData()
        {
            return Recipe;
        }

        public override void SetConfigData(object configData)
        {
            DieUnloaderConfig config = configData as DieUnloaderConfig;
            if (config != null)
            {
                Config = config;
                Config.Init();

            }
        }


        public override object GetConfigData()
        {
            return Config;
        }

        public override void Close()
        {
            base.Close();
        }

        protected override int OnRun()
        {
            return base.OnRun();
        }

        public override void CreateRecipe(RecipeInfo recipe)
        {
            DieUnloaderRecipe dieUnloadRecipe = new DieUnloaderRecipe(this);
            recipe.Add(Name, dieUnloadRecipe);
        }
        #endregion

        public override List<IlluminationChannel> GetIlluminationChannel()
        {
            return Config.ListIlluminationChannel;
        }

        //public XyztCoordinate GetReferenceCoordinate()
        //{
        //    XyztCoordinate refer = Recipe.AlignerRecipe.AlignPositions.GetPositionCoordinate(AlignerRecipe.PositionAligns.Reference.ToString());
        //    return refer;
        //}

        public UvwzxyzCoordinate GetReferenceCoordinate()
        {
            UvwzxyzCoordinate refer = Recipe.AlignerRecipe.AlignPositions.GetPositionCoordinate(AlignerRecipe.PositionAligns.Reference.ToString());
            return refer;
        }

        #region IExecuter
        public override int Initialize()
        {
            int ret = 0;
            if ((ret = base.Initialize()) != 0) return ret;

            if ((ret = Stage.Initialize()) != 0) return ret;
            if ((ret = Camera.Initialize()) != 0) return ret;

            return ret;
        }

        public override void Stop()
        {
            base.Stop();
        }
        public override int OnWork()
        {
            int ret = 0;
            ret = base.Work();

            return ret;
        }

        public override void SetModuleScale(double dScaleX, double dScaleY, double dXaxisT, double dYaxisT, bool bInvertedX, bool bInvertedY)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
