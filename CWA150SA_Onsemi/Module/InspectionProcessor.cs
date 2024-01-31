using QMC.Common;
using QMC.Common.Modules;
using QMC.Common.Parts;
using QMC.Common.Vision.Cameras;
using QMC.Common.Vision.EureSys;
using QMC.Common.Vision.HIKVISION;
using QMC.Common.Vision.Optics;
using QMC.Common.VisionPart;
using System.Collections.Generic;

namespace QMC.Vision
{
    public class InspectionProcessor : Module
    {
        #region Define
        #endregion

        #region Field
        protected CoaxlinkCamera m_CoaxCam;
        protected GrabLinkMultiCamCamera m_GrablinkCam;
        protected HIKGigECamera m_HIKGigECamera;
        #endregion

        #region Property
        public InspectionProcessorConfig Config { set; get; }

        public InspectionProcessorRecipe Recipe { set; get; }

        public VisionCompensator VisionCompensator { set; get; }

        public VisionCalibrator VisionCalibrator { set; get; }
        //public VisionCompensatorACS VisionCompensator { set; get; }

        public Camera Camera { set; get; }

        public Illuminator Illuminator { set; get; }

        //public XytStage Stage { set; get; }
        //public XyztStage Stage { set; get; }
        //public XyzztStage Stage { set; get; }
        public UvwzxyzStage Stage { set; get; }

        public OneAxis ZAxis { set; get; }

        public AutoFocuser AutoFocuser { set; get; }

        public Aligner Aligner { set; get; }

        public StageThetaAgingTester StageThetaAgingTester { set; get; }


        public override List<MotionAxis> GetAxisList()
        {
            List<MotionAxis> listAxis = Stage.GetAxisList();

            //foreach (string strKey in m_dicAxes.Keys)
            //{
            //    MotionAxis axis = m_dicAxes[strKey];
            //    if (axis != null)
            //    {
            //        axis.Configuration.DisplayAxisType = m_dicAxisDisplayType[strKey];

            //        listAxis.Add(axis);
            //    }
            //}
            listAxis.AddRange(ZAxis.GetAxisList());
            return listAxis;
        }

       
        public VisionScale Scale
        {
            set
            {
                //this.Camera.Config.Scale = new PointD(value.X, value.Y);
                ////TODO : Scale 어떻게 할지.
                //Config.VisonCalibratorConfig.Scale = value;
                Config.VisionScale = value;
                //if (VisionCalibrator.Config!=null)
                //{
                //    VisionCalibrator.Config.Scale = value;
                //}
            }
            get
            {
                //if(Config.VisionScale == null)
                //    Config.VisionScale = new VisionScale();
                return Config.VisionScale;
                //if (VisionCalibrator != null)
                //{
                //    return VisionCalibrator.Config.Scale;
                //}
                //else
                //{
                //    return Config.VisionScale;
                //}
                //return new VisionScale(0.0069,0.0069);
                //return new VisionScale(this.Camera.Config.Scale.X, this.Camera.Config.Scale.Y);
                ////return Config.VisonCalibratorConfig.Scale;
            }
        }
        #endregion

        #region Constructor
        public InspectionProcessor(string strName) : base(strName)
        {
            Config = new InspectionProcessorConfig(Name);
            //Recipe = new InspectionProcessorRecipe(this);
        }
        #endregion

        #region Module Members
        public override int Create()
        {
            int ret = 0;

            if ((ret = base.Create()) != 0) return ret;

            //this.Camera = new CoaxlinkCamera("Camera");
            //this.Camera.Create();
            //this.Camera.Owner = this;
            
            m_GrablinkCam = new GrabLinkMultiCamCamera("GCam");
            m_GrablinkCam.Create();
            m_GrablinkCam.Owner = this;
            Parts.Add(m_GrablinkCam);

            m_CoaxCam = new CoaxlinkCamera("CCam");
            m_CoaxCam.Create();
            m_CoaxCam.Owner = this;
            Parts.Add(m_CoaxCam);
            //Hik
            m_HIKGigECamera = new HIKGigECamera("HIKCam");
            m_HIKGigECamera.Create();
            m_HIKGigECamera.Owner = this;
            Parts.Add(m_HIKGigECamera);


            //this.Stage = new XyztStage("Stage");
            //this.Stage = new XyzztStage("Stage");
            this.Stage = new UvwzxyzStage("Stage");
            this.Stage.Create();
            this.Stage.Owner = this;
            this.Stage.Camera = Camera;
            Parts.Add(Stage);

            this.ZAxis = new OneAxis("ZAxis");
            this.ZAxis.Create();
            this.ZAxis.Owner = this;
            Parts.Add(this.ZAxis);

            this.Aligner = new Aligner("Aligner");
            this.Aligner.Create(); 
            this.Aligner.Owner = this;
            this.Aligner.Camera = Camera;
            this.Aligner.Stage= this.Stage;
            this.Aligner.Illuminator = CommonModule.Instance.Illuminator;
            this.Parts.Add(Aligner);


            // this.VisionCompensator = new VisionCompensatorACS("VisionCompensator");
            this.VisionCompensator = new VisionCompensator("VisionCompensator");
            this.VisionCompensator.Create();
            this.VisionCompensator.Stage = this.Stage;
            this.VisionCompensator.Camera= Camera;
            this.VisionCompensator.Scale = this.Scale;
            this.VisionCompensator.Owner = this;
            this.VisionCompensator.Illuminator= CommonModule.Instance.Illuminator;
            this.VisionCompensator.AutoFocusEventEvent += AutoFocusEvent;
            this.VisionCompensator.MoveZAxisEvent += MoveZAxisEvent;
            Parts.Add(VisionCompensator);

            this.VisionCalibrator = new VisionCalibrator("VisionCalibrator");
            this.VisionCalibrator.Create();
            this.VisionCalibrator.UvwzxyzStage = this.Stage;
            this.VisionCalibrator.Camera = Camera;
            this.VisionCalibrator.Owner = this;
            this.VisionCalibrator.Illuminator = CommonModule.Instance.Illuminator;
            Parts.Add(VisionCalibrator);
            

            this.StageThetaAgingTester = new StageThetaAgingTester("StageThetaAgingTester");
            this.StageThetaAgingTester.Create();
            this.StageThetaAgingTester.Stage = this.Stage;
            this.StageThetaAgingTester.Camera = Camera;
            this.StageThetaAgingTester.Owner = this;
            this.StageThetaAgingTester.Scale = this.Scale;
            this.StageThetaAgingTester.Illuminator = CommonModule.Instance.Illuminator;
            Parts.Add(StageThetaAgingTester);

            this.AutoFocuser = new AutoFocuser("AutoFocuser");
            this.AutoFocuser.Create();
            this.AutoFocuser.Camera = Camera;
            this.AutoFocuser.Motion = this.ZAxis;
            this.AutoFocuser.Illuminator = CommonModule.Instance.Illuminator;
            this.AutoFocuser.Camera = Camera;
            this.AutoFocuser.UpdateAutoFocusEvent += UpdateAutoFocusConfigEvent;
            Parts.Add(this.AutoFocuser);

            Recipe = new InspectionProcessorRecipe(this);
            this.VisionCompensator.Recipe = Recipe.VisionCompensatorRecipe;
            this.VisionCalibrator.Recipe = Recipe.VisionCalibratorRecipe;

            Illuminator =CommonModule.Instance.Illuminator;

            return ret;
        }

        public override void Close()
        {
            base.Close();

            if(Camera!=null)
                Camera.Close();
            Aligner.Close();
        }

        public override int Initialize()
        {
            int ret = 0;

            if ((ret = base.Initialize()) != 0) return ret;

            if (Aligner.Camera is CoaxlinkCamera)
            {
                CoaxlinkCamera serverCamera = Aligner.Camera as CoaxlinkCamera;
    
            }

            return ret;
        }

        public override void SetConfigData(object parameter)
        {
            Config = parameter as InspectionProcessorConfig;
            if (Config == null)
                Config = new InspectionProcessorConfig(Name);

            Config.Init(Name);
            
            if(VisionCompensator != null)
            {
                if (VisionCompensator.Recipe.IlluminationDataSet == null)
                    VisionCompensator.Recipe.IlluminationDataSet = new IlluminationDataSet(Name);
                VisionCompensator.Recipe.IlluminationDataSet.SetIlluminationChannel(Config.ListIlluminationChannel);
            }

            if(Aligner != null)
            {
                if(Aligner.Recipe.IlluminationDataSet==null)
                    Aligner.Recipe.IlluminationDataSet = new IlluminationDataSet(Name);
                Aligner.Recipe.IlluminationDataSet.SetIlluminationChannel(Config.ListIlluminationChannel);
            }

            if(this.VisionCalibrator != null)
            {
                if (VisionCalibrator.Recipe.IlluminationDataSet == null)
                    VisionCalibrator.Recipe.IlluminationDataSet = new IlluminationDataSet(Name);
                VisionCalibrator.Recipe.IlluminationDataSet.SetIlluminationChannel(Config.ListIlluminationChannel);
            }

            if(this.StageThetaAgingTester != null)
            {
                if (StageThetaAgingTester.Recipe.IlluminationDataSet == null)
                    StageThetaAgingTester.Recipe.IlluminationDataSet = new IlluminationDataSet(Name);
                StageThetaAgingTester.Recipe.IlluminationDataSet.SetIlluminationChannel(Config.ListIlluminationChannel);
            }

            VisionCompensator.Config = this.Config.VisionCompensatorConfig;
            VisionCalibrator.Config = this.Config.VisionCalibratorConfig;
            
            Stage.Config = this.Config.StageConfig;

            if (Camera != null)
                Camera.Config = this.Config.CameraConfig;

            if (Config.CameraType == CameraType.GrablinkCamera)
            {
                Camera = m_GrablinkCam;
                if(this.Config.CameraConfig is GrabLinkMultiCamCameraConfig)
                {
                    Camera.Config = this.Config.CameraConfig;
                }
                else
                {
                    this.Config.CameraConfig = Camera.Config;
                }
            }
            else if(Config.CameraType == CameraType.HikCamera)
            {
                Camera = m_HIKGigECamera;
                if(this.Config.CameraConfig is HIKGigECameraConfig)
                {
                    Camera.Config = this.Config.CameraConfig;
                }
                else
                {
                    this.Config.CameraConfig= Camera.Config;
                }
            }
            else if(Config.CameraType == CameraType.CoaxlinkCamera)
            {
                Camera = m_CoaxCam;
                if (this.Config.CameraConfig is CoaxlinkCameraConfig)
                {
                    Camera.Config = this.Config.CameraConfig;
                }
                else
                {
                    this.Config.CameraConfig = Camera.Config;
                }
            }
            else
            {

            }

            this.Aligner.Camera = Camera;
            this.VisionCompensator.Camera = Camera;
            this.VisionCalibrator.Camera = Camera;
            this.Aligner.Scale = this.Scale;
            this.VisionCompensator.Scale= this.Scale;
            this.StageThetaAgingTester.Camera = Camera;
            this.StageThetaAgingTester.Scale = this.Scale;
            this.AutoFocuser.Config = Config.AutoFocuserConfig;
            this.AutoFocuser.Camera = Camera;
            //Illuminator.Config = Config.IlluminatorConfig;
        }

        public override object GetConfigData()
        {
            return Config;
        }

        public override void UpdateConfigData()
        {
            base.UpdateConfigData();

            //Camera.Config = Config.CameraConfig;
            this.AutoFocuser.Config = Config.AutoFocuserConfig;
        }

        public override void SetRecipeData(object recipeData)
        {
            Recipe = recipeData as InspectionProcessorRecipe;
            if (Recipe == null)
                Recipe = new InspectionProcessorRecipe(this);
            Recipe.Init(this);

            if(Aligner != null)
            {
                Aligner.Recipe = Recipe.AlignerRecipe;
                if (Aligner.Recipe == null)
                {
                    Aligner.Recipe = new AlignerRecipe(Aligner);
                }
            }
            if(VisionCompensator != null)
            {
                VisionCompensator.Recipe = Recipe.VisionCompensatorRecipe;
                if (VisionCompensator.Recipe == null)
                {
                    VisionCompensator.Recipe = new VisionCompensatorRecipe(VisionCompensator.Name);
                }
            }
            if (VisionCalibrator != null)
            {
                VisionCalibrator.Recipe = Recipe.VisionCalibratorRecipe;
                if (VisionCalibrator.Recipe == null)
                {
                    VisionCalibrator.Recipe = new VisionCalibratorRecipe(VisionCalibrator);
                }
            }

            if(StageThetaAgingTester != null)
            {
                StageThetaAgingTester.Recipe = Recipe.StageThetaAgingTesterRecipe;
                if(StageThetaAgingTester.Recipe == null)
                {
                    StageThetaAgingTester.Recipe = new StageThetaAgingTesterRecipe(StageThetaAgingTester);
                }
            }

            base.SetRecipeData(recipeData);
        }

        public override object GetRecipeData()
        {
            return Recipe;
        }

        //public override void UpdateRecipeData()
        //{
        //    base.UpdateRecipeData();
        //}

        //public override void SaveRecipeData()
        //{
        //    EquipmentVision.UpdateRecipeData();
        //    EquipmentVision.SaveRecipe();
        //}

        //public override void SaveConfigData()
        //{
        //    DataManager.Instance.Config.UpdateData(this);
        //    EquipmentVision.SaveConfig();
        //}

        public override void SaveConfigData()
        {
            DataManager.Instance.Config.UpdateData(this);
            EquipmentVision.SaveConfig();
        }

        public override void SaveRecipeData()
        {
            RecipeInfo recipe = EquipmentVision.GetCurrentRecipe();
            recipe.Add(this.Name, GetRecipeData());
            EquipmentVision.SaveRecipe();

        }


        protected override int OnRun()
        {
            int ret = 0;
            string result = string.Empty;

            if ((ret = base.OnRun()) != 0)
            {
                return ret;
            }

            return ret;
        }

        public override void SetModuleScale(double dScaleX, double dScaleY, double dXaxisT, double dYaxisT, bool bInvertedX, bool bInvertedY)
        {
            this.Scale.X = dScaleX;
            this.Scale.Y = dScaleY;
            this.Scale.XAxisT = dXaxisT;
            this.Scale.YAxisT = dYaxisT;
            this.Scale.InvertedX = bInvertedX;
            this.Scale.InvertedY = bInvertedY;
        }
        #endregion

        #region Method
        #endregion

        public void AutoFocusEvent(out AutoFocusResult result)
        {
            result = null;

            if(this.AutoFocuser != null)
            {
                this.AutoFocuser.OnWork();

                if(this.AutoFocuser.Result != null)
                {
                    result = this.AutoFocuser.Result;
                }
            }
        }

        public void MoveZAxisEvent(double position)
        {
            if (this.ZAxis != null)
            {
                this.ZAxis.Move(position);
            }
        }

        public void UpdateAutoFocusConfigEvent(ref AutoFocuserConfig config)
        {
            if(this.AutoFocuser != null)
            {
                if(this.Config.AutoFocuserConfig!=null)
                {
                    config = this.Config.AutoFocuserConfig;
                }
                else
                {
                    this.Config.AutoFocuserConfig = new AutoFocuserConfig();
                    config = this.Config.AutoFocuserConfig;
                }
            }
        }
    }
}
