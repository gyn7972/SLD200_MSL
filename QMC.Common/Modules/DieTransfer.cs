using QMC.Common.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using QMC.Common.VisionPart;
using QMC.Common.Vision.EureSys;
using QMC.Common.Vision.HIKVISION;

namespace QMC.Common.Modules
{
    [Serializable]
    public class DieTransfer : Module
    {
        public LoadZ LoadZ { get; set; }        
        public UnloadZ UnloadZ { get; set; }
        public Turret Turret { get; set; }
        public RevisionZ RevisionZ { get; set; }
        public DieSearcher DieSearcher { get; set; }

        public ColletZCalibrator ColletZCalibrator { get; set; }

        public DieTransferConfig Config { get; set; }
        public DieTransferRecipe Recipe { get; set; }
        public ColletCountDataList ColletCounts { get; set; }

        public ColletXYPositionCalibrator PositionCalibrator { get; set; }

        //public HIKGigECamera Camera { set; get; }
        public GrabLinkMultiCamCamera Camera { get; set; }

        public AutoFocuser AutoFocuser { set; get; }
        public VisionScale Scale { set; get; }
        public List<Collet> Collets { get; set; }

        public DieLoader DieLoader { get; set; }
        public DieUnloader DieUnloader { get; set; }

        public DieTransfer(string strName) : base(strName)
        {
            Scale = new VisionScale();
        }

        #region Module
        public override int Create()
        {
            int ret = base.Create();

            //Camera = new HIKGigECamera("RevisionCamera");
            Camera = new GrabLinkMultiCamCamera("RevisionCamera");
            Camera.Create();
            Camera.Owner = this;
            Parts.Add(Camera);

            LoadZ = new LoadZ("LoadZ");
            LoadZ.Create();
            LoadZ.Owner = this;
            Parts.Add(LoadZ);

            UnloadZ = new UnloadZ("UnloadZ");
            UnloadZ.Create();
            UnloadZ.Owner = this;
            Parts.Add(UnloadZ);

            Turret = new Turret("Turret");
            Turret.Create();
            Turret.Owner = this;
            Parts.Add(Turret);

            RevisionZ = new RevisionZ("RevisionZ");
            RevisionZ.Create();
            RevisionZ.Owner = this;
            Parts.Add(RevisionZ);

            PositionCalibrator = new ColletXYPositionCalibrator("ColletCalibrator");
            PositionCalibrator.Create();
            PositionCalibrator.Owner = this;
            Parts.Add(PositionCalibrator);

            DieSearcher = new DieSearcher("DieSearcher");
            DieSearcher.Create();
            DieSearcher.Owner = this;            
            Parts.Add(DieSearcher);

            ColletZCalibrator = new ColletZCalibrator("ColletZCalibrator");
            ColletZCalibrator.Create();
            ColletZCalibrator.Owner = this;
            Parts.Add(ColletZCalibrator);

            AutoFocuser = new AutoFocuser("AutoFocuser");
            AutoFocuser.Create();
            AutoFocuser.Owner = this;
            Parts.Add(AutoFocuser);

            Config = new DieTransferConfig();
            Recipe = new DieTransferRecipe(this);

            DieSearcher.Turret = Turret;
            Turret.Config = this.Config.TurretConfig;
            DieSearcher.Camera = Camera;
            PositionCalibrator.Motion = Turret;
            //Camera.Config = Config.HIKGigECameraConfig;
            Camera.Config = Config.GrabLinkMultiCamCameraConfig;            
            LoadZ.LoadZConfig = this.Config.LoadZConfig;
            UnloadZ.UnLoadZConfig = this.Config.UnloadZConfig;
            DieSearcher.Recipe = Recipe.DieSearcherRecipe;
            PositionCalibrator.Camera = Camera;
            PositionCalibrator.XyCalibratorRecipe = Recipe.XYCalibratorRecipe;
            LoadZ.ColletZCalibrator = ColletZCalibrator;
            UnloadZ.ColletZCalibrator= ColletZCalibrator;
            PositionCalibrator.Collets = Collets;
            PositionCalibrator.Illuminator = CommonModule.Instance.Illuminator;
            DieSearcher.Illuminator = CommonModule.Instance.Illuminator;

            AutoFocuser.Camera = Camera;
            AutoFocuser.Config = Config.AutoFocuerConfig;
            AutoFocuser.Motion = this.RevisionZ;
            AutoFocuser.Illuminator = CommonModule.Instance.Illuminator;

            ColletCounts = new ColletCountDataList();
            for (int i = 0; i < Turret.TurretArmCount; i++)
            {
                ColletCountData countData = new ColletCountData(i + 1);
                ColletCounts.Add(countData);
            }
            int nColletIndex = 1;
            Collets = new List<Collet>();
            foreach(int nChannel in Config.CollectChannel)
            {
                Collet collet = new Collet("Collet"+nColletIndex.ToString());
                collet.Create();
                collet.Owner = this;
                collet.ArmIndex = nColletIndex - 1;
                collet.Channel = nColletIndex;
                if(nColletIndex <= 3)
                {
                    collet.Communicator = CommonModule.Instance.ColletGaugeCommunicator1;
                }
                else
                {
                    collet.Communicator = CommonModule.Instance.ColletGaugeCommunicator2;
                }
                Gripper gripper = new Gripper(collet.Name + "_Gripper");
                gripper.Create();
                gripper.Owner = this;

                
                collet.Gripper = gripper;
                Collets.Add(collet);
                Parts.Add(gripper);
                nColletIndex++;
            }

            Turret.Collets = Collets;




            return ret;
        }

        public override void SetConfigData(object configData)
        {
            DieTransferConfig config = configData as DieTransferConfig;

            if (config != null)
            {
                Config = config;
                Config.Init();
                if (Config.CollectChannel == null)
                {
                    Config.CollectChannel = new List<int> { 1, 2, 3, 1, 2, 3 };
                }
                    
                for (int i = 0; i < Config.CollectChannel.Count; i++)
                {
                    Collets[i].Channel = Config.CollectChannel[i];
                }

            }

        }

        public override object GetConfigData()
        {
            return Config;
        }

        public override void SetRecipeData(object recipeData)
        {
            DieTransferRecipe recipe = recipeData as DieTransferRecipe;
            if (recipe != null)
            {
                Recipe = recipe;
                Recipe.init(this);

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
            DieTransferRecipe dieTransferRecipe = new DieTransferRecipe(this);
            recipe.Add(Name, dieTransferRecipe);
        }
        #endregion

        #region IExecuter
        public override int Initialize()
        {
            int ret = 0;
            if((ret = base.Initialize()) != 0) return ret;

            if((ret = LoadZ.Initialize()) != 0) return ret;
            if ((ret = UnloadZ.Initialize()) != 0) return ret;
            if ((ret = Turret.Initialize()) != 0) return ret;
            if ((ret = RevisionZ.Initialize()) != 0) return ret;
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
        #endregion

        #region Part
        public override List<MotionAxis> GetAxisList()
        {
            List<MotionAxis> result = new List<MotionAxis>();
            foreach(Part part in Parts)
            {
                foreach(MotionAxis axis in part.GetAxisList())
                {
                    result.Add(axis);
                }
            }
            return result;
        }

        #endregion

        public override List<IlluminationChannel> GetIlluminationChannel()
        {
            return Config.ListIlluminationChannel;
        }

        public int GetColletIndex(Turret.PoistionKey position)
        {
            int index = 0;

            if(Turret != null)
            {
                index = Turret.GetColletIndex(position);
            }

            return index;
        }

        public Collet GetCollet(Turret.PoistionKey position)
        {
            int index = 0;
            Collet collet = null;
            if (Turret != null)
            {
                index = Turret.GetColletIndex(position) - 1;
                if (index >= 0 && index < Collets.Count)
                    collet = Collets[index];
            }

            return collet;
        }

        public override void SetModuleScale(double dScaleX, double dScaleY, double dXaxisT, double dYaxisT, bool bInvertedX, bool bInvertedY)
        {
            throw new NotImplementedException();
        }
    }
}
