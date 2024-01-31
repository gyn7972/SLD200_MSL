using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QMC.Common.Parts;
using QMC.Common.Vision;
using QMC.Common.Vision.Cameras;
using QMC.Common.Vision.EureSys;
using QMC.Common.Vision.HIKVISION;
using QMC.Common.Vision.Optics;
using QMC.Common.VisionPart;

namespace QMC.Common.Modules
{
    public class DieLoader : Module
    {
        public LoadingQueue LoadingQueue { set; get; }

        public DieLoaderConfig Config { set; get; }
        public DieLoaderRecipe Recipe { set; get; }

        //public XytStage Stage { set; get; }
        //public XyztStage Stage { set; get; }
        //public XyzztStage Stage { set; get; }
        public UvwzxyzStage Stage { set; get; }

        public Gripper Gripper { set; get; }

        //public HIKGigECamera Camera { set; get; }
        public GrabLinkMultiCamCamera Camera { get; set; }

        public Illuminator Illuminator { set; get; }

        public NeedleBlock NeedleBlock { set; get; }

        public RealTimeScanner Scanner { set; get; }
        public VisionCalibrator VisionCalibrator { set; get; }

        public PickupAgent PickupAgent { set; get; }
        public VisionImage TestImage { set; get; }
        public VisionImage TrainImage { set; get; }
        public ColletXYPositionCalibrator XYCalibrator { get; set; }
        public PositionResetter PositionResetter { set; get; }

        public DieTransfer DieTransfer { set; get; }

        public VisionScale Scale
        {
            get
            {
                if (Config.VisonCalibratorConfig.Scale == null)
                    Config.VisonCalibratorConfig.Scale = new VisionScale();
                return Config.VisonCalibratorConfig.Scale;
            }
        }
        public bool Simulated { set; get; }

        public double Radius 
        { 
            set
            {
                Config.Radius = value;
            }
            get
            {
                return Config.Radius;
            }
        }

        

        public DieLoader(string strName) : base(strName)
        {

            //TestImage = new VisionImage();
            //Config = new DieLoaderConfig();
            //Recipe = new DieLoaderRecipe();
            //Scale = new VisionScale();
              TestImage = new VisionImage();
            TrainImage = new VisionImage();    
            
        }
        
        #region IExecuter
        public override int Initialize()
        {
            int ret = 0;
            if ((ret = base.Initialize()) != 0) return ret;
            if ((ret = Camera.Initialize()) != 0) return ret;
            if ((ret = NeedleBlock.Initialize()) != 0) return ret;
            if ((ret = Stage.Initialize()) != 0) return ret;

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
        #endregion

        #region Module
        protected override int OnRun()
        {
            return base.OnRun();
        }
        public override int Create()
        {
            int ret = base.Create();

            //Stage = new XytStage("Stage");
            //Stage = new XyztStage("Stage");
            //Stage = new XyzztStage("Stage");
            Stage = new UvwzxyzStage("Stage");
            Stage.Create();
            Stage.Owner = this;
            Parts.Add(Stage);

            Gripper = new Gripper("StageGripper");
            Gripper.Create();
            Gripper.Owner = this;
            Parts.Add(Gripper);

            //Camera = new HIKGigECamera("SourceCamera");
            Camera = new GrabLinkMultiCamCamera("SourceCamera");
            Camera.Create();
            Camera.Owner = this;
            Parts.Add(Camera);

            //hIKGigECamera = new HIKGigECamera("vision Camera");
            //hIKGigECamera.Create();
            //hIKGigECamera.Owner = this;
            //Parts.Add(hIKGigECamera);

            NeedleBlock = new NeedleBlock("NeedleBlock");
            NeedleBlock.Create();
            NeedleBlock.Owner = this;
            Parts.Add(NeedleBlock);

            Scanner = new RealTimeScanner("RealTimeScanner");
            Scanner.Create();
            Scanner.Owner = this;
            Parts.Add(Scanner);

            PickupAgent = new PickupAgent("PickupAgent");
            PickupAgent.Create();
            PickupAgent.Owner = this;
            Parts.Add(PickupAgent);

            VisionCalibrator = new VisionCalibrator("VisionCalibrator");
            VisionCalibrator.Create();
            VisionCalibrator.Owner = this;
            
            Parts.Add(VisionCalibrator);

            XYCalibrator = new ColletXYPositionCalibrator("ColletCalibrator");
            XYCalibrator.Create();
            XYCalibrator.Owner = this;
            Parts.Add(XYCalibrator);

            PositionResetter = new PositionResetter("Resetter");
            PositionResetter.Create();
            PositionResetter.Owner = this;
            Parts.Add(PositionResetter);

            Config = new DieLoaderConfig();
            Recipe = new DieLoaderRecipe(this);

            Illuminator = CommonModule.Instance.Illuminator;

            this.LoadingQueue = Equipment.LoadingQueue;

            NeedleBlock.Stage = Stage;
            
            Stage.NeedleBlock = NeedleBlock;
            Scanner.Stage = Stage;
            Scanner.Camera = Camera;
            Scanner.RecipeSubMaterial = Recipe.SubMaterial;
            Scanner.LoadingQueue = LoadingQueue;
            //Stage.Gripper = Gripper;
            //Camera.Config = Config.HIKGigECameraConfig;
            Camera.Config = Config.GrabLinkMultiCamCameraConfig;
            NeedleBlock.Config = this.Config.NeedleBlockConfig;
            NeedleBlock.Camera = Camera;
            PickupAgent.Config = this.Config.PickUpAgentConfig;
            VisionCalibrator.UvwzxyzStage = Stage;
            VisionCalibrator.Camera = Camera;
            VisionCalibrator.Illuminator = Illuminator;
            VisionCalibrator.Config = this.Config.VisonCalibratorConfig;
            XYCalibrator.Recipe = this.Recipe.ColletXYPositionCalibratorRecipe;
            //XYCalibrator.DieLoaderConfig = this.Config;
            XYCalibrator.Camera = Camera; 
            XYCalibrator.Illuminator = this.Illuminator;
            XYCalibrator.Motion = Stage;
            XYCalibrator.XyCalibratorRecipe = this.Recipe.ColletXYPositionCalibratorRecipe;
            //Camera.Config = Config.HIKGigECameraConfig;
            Camera.Config = Config.GrabLinkMultiCamCameraConfig;
            //  Config.HIKGigECameraConfig = Camera.Config;
            Scanner.Illuminator = Illuminator;

            

            return ret;
        }

        public override void Close()
        {
            base.Close();
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

        public override void SetRecipeData(object recipeData)
        {
            DieLoaderRecipe recipe = recipeData as DieLoaderRecipe;
            if (recipe != null)
            {
                Recipe = recipe;
                //Recipe.Init(this);
                if (Recipe.IlluminationDataSets == null)
                    Recipe.IlluminationDataSets = new IlluminationDataList();

                foreach (Part part in Parts)
                {
                    Recipe.UpdateIlluminationData(part);
                }

                base.SetRecipeData(recipeData);
            }
        }
        public override object GetRecipeData()
        {
            return Recipe;
        }

        public override void SetConfigData(object configData)
        {
            DieLoaderConfig config = configData as DieLoaderConfig;

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
        #endregion

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

        
        public bool ScanPositionCheckInterlock(XyCoordinate position)
        {
            int ret = 0;
            XyCoordinate capCurrentCoordinate = new XyCoordinate();
            CircleD stageCircle;
            CircleD capCircle;

            if ((ret = NeedleBlock.GetCommandPosition(ref capCurrentCoordinate)) != 0) return false;

            stageCircle = new CircleD(position, this.Radius);
            capCircle = new CircleD(capCurrentCoordinate, NeedleBlock.Radius);

            if (stageCircle.Contains(capCircle) != ShapeLocation.Inner) return false;

            return true;
        }

        //public bool CheckMotionLimit(XyztCoordinate position)
        //{
        //    bool bRet = true;

        //    bRet &= Stage.CheckLimit(XyztStage.MotionKey.X, position.X);
        //    bRet &= Stage.CheckLimit(XyztStage.MotionKey.Y, position.Y);
        //    bRet &= Stage.CheckLimit(XyztStage.MotionKey.Z, position.Z);
        //    bRet &= Stage.CheckLimit(XyztStage.MotionKey.T, position.T);

        //    return bRet;
        //}

        public bool CheckMotionLimit(UvwzxyzCoordinate position)
        {
            bool bRet = true;

            bRet &= Stage.CheckLimit(UvwzxyzStage.MotionKey.U, position.U);
            bRet &= Stage.CheckLimit(UvwzxyzStage.MotionKey.V, position.V);
            bRet &= Stage.CheckLimit(UvwzxyzStage.MotionKey.W, position.W);
            bRet &= Stage.CheckLimit(UvwzxyzStage.MotionKey.X, position.X);
            bRet &= Stage.CheckLimit(UvwzxyzStage.MotionKey.Y, position.Y);
            bRet &= Stage.CheckLimit(UvwzxyzStage.MotionKey.EZ, position.EZ);
            bRet &= Stage.CheckLimit(UvwzxyzStage.MotionKey.VZ, position.VZ);

            return bRet;
        }

        public DieLoadSubstrate GetMaterial()
        {
            DieLoadSubstrate substrate = new DieLoadSubstrate();

            return substrate;
        }
        
        public RealTimeScanParameter GetRealTimeScanParamter(Collet collet)
        {
            int ret = 0;
            PointD offsetPixel = new PointD();
            XyCoordinate offset = new XyCoordinate();
            RealTimeScanParameter parameter = null;
            
            if (Scanner != null)
            {
                parameter = new RealTimeScanParameter();
                XytCoordinate currentPosition = new XytCoordinate();
                PointD pitchPixel = new PointD();
                PointD referencePixel = new PointD();

                if ((ret = this.Stage.GetCommandPosition(ref currentPosition)) != 0) return parameter;

                VisionScale.ConvertPixel<XyCoordinate>(this.Scale, this.Camera.Resolution, new XyCoordinate(this.Recipe.SubMaterial.PitchX, this.Recipe.SubMaterial.PitchY), out pitchPixel);

                //if (this.ColletXyPositionCalibrator != null && collet != null)
                //{
                //    if ((ret = this.ColletXyPositionCalibrator.GetColletXyPosition(collet, false, ref offset)) != 0)
                //    {
                //        if ((ret = this.Alarms[AlarmKeys.NotFoundColletOffset].Post(this)) != 0) return parameter;
                //    }
                //}

                if (collet != null)
                {
                    VisionScale.ConvertPixel<XyCoordinate>(this.Scale, this.Camera.Resolution, offset, out offsetPixel);
                    referencePixel = new PointD(this.Camera.Resolution.Width - offsetPixel.X, this.Camera.Resolution.Height - offsetPixel.Y);
                    parameter.Resolution = new PointD(this.Camera.Resolution.Width, this.Camera.Resolution.Height);
                }
                else
                {
                    referencePixel = new PointD(this.Camera.Resolution.Width / 2, this.Camera.Resolution.Height / 2);
                    parameter.Resolution = new PointD(this.Camera.Resolution.Width / 2, this.Camera.Resolution.Height / 2);
                }

                // Scan Current Row
                parameter.CurrentPosition = currentPosition;
                parameter.Pitch = new SizeD(Math.Abs(this.Camera.Resolution.Width / 2 - pitchPixel.X), Math.Abs(this.Camera.Resolution.Height / 2 - pitchPixel.Y));
                parameter.Reference = referencePixel;

                if (0 < this.LoadingQueue.Count())
                {
                    parameter.SearchNextRow = false;
                }
            }

            return parameter;
        }

        public override void CreateRecipe(RecipeInfo recipe)
        {
            DieLoaderRecipe dieLoaderRecipe = new DieLoaderRecipe(this);
            recipe.Add(Name, dieLoaderRecipe);
        }

        public override List<IlluminationChannel> GetIlluminationChannel()
        {
            return Config.ListIlluminationChannel;
        }

        public override void SetModuleScale(double dScaleX, double dScaleY, double dXaxisT, double dYaxisT, bool bInvertedX, bool bInvertedY)
        {
            throw new NotImplementedException();
        }
    }
}
