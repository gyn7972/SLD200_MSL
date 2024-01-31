using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using QMC.Common;
using QMC.Common.Parts;
using QMC.Common.Vision.Cameras;
using QMC.Common.Vision.EureSys;
using QMC.Common.Vision.HIKVISION;
using QMC.Common.Vision.Optics.Leesos;
using QMC.Process.WaferProbeAlign.Parts;

namespace QMC.Process.WaferProbeAlign.Modules
{

    public class WaferProbeAlignProcessor : Module
    {
        public enum PositionKey
        {
            Load,
            Unload,
            Process,
        }

        //public HIKGigECamera Camera { set; get; }
        public GrabLinkMultiCamCamera Camera_Upper { set; get; }
        public GrabLinkMultiCamCamera Camera_Lower { set; get; }

        //public LaserProcessStage Stage { set; get; }

        //public LaserZ LaserZ { set; get; } 
        //public Scanner Scanner { set; get; }

        //public TwoPointAligner TwoPointAligner { set; get; }
        //public XyVisionCalibrator XyVisionCalibrator { set; get; }
        public WaferProbeAlignProcessorConfig Config { set; get; }
        public WaferProbeAlignProcessorRecipe Recipe { set; get; }
        public DigitalIlluminator Illuminator { set; get; }

        public LaserPowerCalibrator PowerCalibrator { set; get; }

        public DustCollector DustCollector { set; get; }

        //public Unloader Unloader { set; get; }

        public VisionScale Scale 
        { 
            set
            {
                Config.VisonCalibratorConfig.Scale = value;
            }
            get
            {
                return Config.VisonCalibratorConfig.Scale;
            }
        }
        public WaferProbeAlignProcessor(string strName) : base(strName)
        {
            Config = new WaferProbeAlignProcessorConfig();     
            Scale = new VisionScale();
            HasRecipe = true;
        }

        public override int Create()
        {
            int ret = 0;

            if ((ret = base.Create()) != 0) return ret;

            //Camera = new HIKGigECamera("LaserCamera");
            Camera_Upper = new GrabLinkMultiCamCamera("Upper Camera");
            Camera_Upper.Create();
            Camera_Upper.Owner = this;
            Parts.Add(Camera_Upper);

            //Camera_LowRes = new HIKGigECamera("LaserCamera Low-Res");                             //  하부 카메라
            Camera_Lower = new GrabLinkMultiCamCamera("Lower Camera");                              //  하부 카메라
            Camera_Lower.Create();
            Camera_Lower.Owner = this;
            Parts.Add(Camera_Lower);

            /*Stage = new LaserProcessStage("Stage");
            Stage.Create();
            Stage.Owner = this;
            Parts.Add(Stage);*/

            /*LaserZ = new LaserZ("LaserZ");
            LaserZ.Create();
            LaserZ.Owner = this;
            Parts.Add(LaserZ);*/

            /*TwoPointAligner = new TwoPointAligner("Aligner");
            TwoPointAligner.Create();
            TwoPointAligner.Owner = this;
            Parts.Add(TwoPointAligner);

            XyVisionCalibrator = new XyVisionCalibrator("VisionCalibrator");
            XyVisionCalibrator.Create();
            XyVisionCalibrator.Owner = this;
            Parts.Add(XyVisionCalibrator);*/

            Illuminator = new DigitalIlluminator("Illuminator");
            Illuminator.Create();
            Illuminator.Owner = this;
            Parts.Add(Illuminator);

            //Scanner = new Scanner("LaserScanner");
            //Scanner.Create();
            //Scanner.Owner = this;
            //Parts.Add(Scanner);

            PowerCalibrator = new LaserPowerCalibrator("LaserCalibrator");
            PowerCalibrator.Create();
            PowerCalibrator.Owner = this;
            Parts.Add(PowerCalibrator);

            DustCollector = new DustCollector("DustCollector");
            DustCollector.Create();
            DustCollector.Owner = this;
            Parts.Add(DustCollector);

            //PowerCalibrator.Scanner = Scanner;

            /*XyVisionCalibrator.Camera = Camera;
            XyVisionCalibrator.XyStage = Stage;
            TwoPointAligner.Stage = Stage;
            TwoPointAligner.Camera = Camera;
            TwoPointAligner.Illuminator = Illuminator;*/


            Recipe = new WaferProbeAlignProcessorRecipe(this);

            return ret;
        }

        public override int Initialize()
        {
            int ret = 0;

            //ret = Scanner.Initialize();

            //if (!Conveyor.IsInputCarrier())
            //    this.IsReady = true;
            ret = base.Initialize();

            return ret;
        }

        public int OpticalInit()
        {
            int ret = 0;
            //if ((ret = Scanner.Initialize()) != 0) return ret;
            if ((ret = Camera_Upper.Initialize()) != 0) return ret;
            if ((ret = Camera_Lower.Initialize()) != 0) return ret;

            return ret;
        }

        public override void SetModuleScale(double dScaleX, double dScaleY, double dXaxisT, double dYaxisT, bool bInvertedX, bool bInvertedY)
        {
            throw new NotImplementedException();
        }

        public override void Start()
        {
            base.Start();
            /*if(!Conveyor.IsInputCarrier())
            {
                IsReady = true;
            }*/
        }

        public override void SetConfigData(object parameter)
        {
            Config = parameter as WaferProbeAlignProcessorConfig;
            if(Config == null)
                Config = new WaferProbeAlignProcessorConfig();
            Config.Init();
            Illuminator.Config = Config.IlluminatorConfig;
            //XyVisionCalibrator.Config = Config.VisonCalibratorConfig;
            Camera_Upper.Config = Config.CameraConfig_Upper;
            Camera_Lower.Config = Config.CameraConfig_Lower;
            //Scanner.Config = Config.ScannerConfig;
        }

        public override void UpdateConfigData()
        {
            base.UpdateConfigData();

            Illuminator.Initialize();
        }

        public override List<MotionAxis> GetAxisList()
        {
            List<MotionAxis> listAxes = new List<MotionAxis>();

            foreach (var part in Parts)
            {
                foreach (var axis in part.GetAxisList())
                {
                    listAxes.Add(axis);
                }
            }
            return listAxes;
        }

        public override object GetConfigData()
        {
            return Config;
        }

        public override object GetRecipeData()
        {
            return Recipe;
        }

        public override void SetRecipeData(object recipeData)
        {
            Recipe = recipeData as WaferProbeAlignProcessorRecipe;
            if (Recipe == null)
                Recipe = new WaferProbeAlignProcessorRecipe(this);
            Recipe.Init(this);

            //TwoPointAligner.Recipe = Recipe.TwoPointAlignerRecipe;

            //Scanner.Recipe = Recipe.ScannerRecipe;
            base.SetRecipeData(recipeData);
        }

        public override void SaveConfigData()
        {
            DataManager.Instance.Config.UpdateData(this);
            Equipment.SaveConfig();
        }

        public override void SaveRecipeData()
        {
            Equipment.UpdateRecipeData();
            Equipment.SaveRecipe();
        }

        public int PrepareToLoad()
        {
            int ret = 0;
            
            /*if((ret = LaserZ.MovePosition(0)) != 0)
            {
                //Error
                return ret;
            }*/

            XyztCoordinate xyzCoordinate = Config.GetPositionData(WaferProbeAlignProcessorConfig.PositionKey.Load.ToString());
            
            /*if((ret = Stage.MovePosition((XyCoordinate)xyzCoordinate))!= 0)
            {
                //Error
                return ret;
            }*/

            return ret;
        }

        public int PrepareToUnload()
        {
            int ret = 0;

            /*if ((ret = LaserZ.MovePosition(0)) != 0)
            {
                //Error
                return ret;
            }*/

            XyztCoordinate xyztCoordinate = Config.GetPositionData(WaferProbeAlignProcessorConfig.PositionKey.Unload.ToString());

            /*if ((ret = Stage.MovePosition((XyCoordinate)xyzCoordinate)) != 0)
            {
                //Error
                return ret;
            }*/

            return ret;
        }

        public int PrepareToEtchingProcess()
        {
            int ret = 0;

            /*if ((ret = LaserZ.MovePosition(0)) != 0)
            {
                //Error
                return ret;
            }*/

            XyztCoordinate xyztCoordinate = Config.GetPositionData(WaferProbeAlignProcessorConfig.PositionKey.Etching.ToString());

            /*if ((ret = Stage.MovePosition((XyCoordinate)xyzCoordinate)) != 0)
            {
                //Error
                return ret;
            }

            if ((ret = LaserZ.MovePosition(xyzCoordinate.Z)) != 0)
            {
                //Error
                return ret;
            }*/

            return ret;
        }

        public int RunAlignProcess()
        {
            int ret = 0;

            /*if(Carrier == null)
            {
                //error
                //return -1;
                //Test를 위해 잠시 주석
                Carrier = new Carrier();
            }

            for(int i = 0; i < Carrier.UnitCount; i++)
            {
                if (m_Status == RunStatus.Stop)
                {
                    ret = 1;
                    return ret;
                }

                SmtUnit unit = Carrier.ArrSmtUnit[i];
                if(unit.UseWork)
                {
                    if ((ret = OnAlign(i, unit)) != 0) return ret;
                }
            }

            ret = TwoPointAligner.TurnOffIlluminator();*/

            return ret;
        }

        public int RunAlignProcess(int nIndex)
        {
            int ret = 0;
            
/*            if (Carrier == null)
            {
                //error
                //return -1;
                //Test를 위해 잠시 주석
                Carrier = new Carrier();
            }

            for (int i = 0; i < Carrier.UnitCount; i++)
            {
                SmtUnit unit = Carrier.ArrSmtUnit[i];
                if (nIndex == i && unit.UseWork)
                {
                    if ((ret = OnAlign(i, unit)) != 0) return ret;
                    break;
                }
            }

            ret = TwoPointAligner.TurnOffIlluminator();*/

            return ret;
        }

        public int OnAlign(int index, SmtUnit unit)
        {
            int ret = 0;

            /*TwoPointAligner.SetAlignPosition(index);
            if ((ret = TwoPointAligner.Work()) != 0)
            {
                unit.Result = SmtUnit.ResultKey.Ng;
                unit.UseWork = false;
                ret = 0;
                //return ret;
            }
            unit.EtchingPosition = TwoPointAligner.Result;*/

            return ret;
        }
        public int RunEtchingProcess()
        {
            int ret = 0;
            if (m_Status == RunStatus.Stop)
            {
                ret = 1;
                return ret;
            }

            XyztCoordinate coordinate = Config.GetPositionData(WaferProbeAlignProcessorConfig.PositionKey.Etching.ToString());

            /*if (LaserZ != null)
            {
                if ((ret = LaserZ.MovePosition(coordinate.Z)) != 0) return ret;
            }*/

            //Scanner.EnableLaserEmission(true);

            //  Drilling~

            /*for (int i = 0; i < Carrier.UnitCount; i++)
            {
                if (m_Status == RunStatus.Stop)
                {
                    ret = 1;
                    return ret;
                }

                SmtUnit unit = Carrier.ArrSmtUnit[i];
                if (unit.UseWork && unit.WorkStatus != SmtUnit.UnitWorkStateKey.Complete)
                {
                    unit.WorkStatus = SmtUnit.UnitWorkStateKey.Work;
                    if ((ret = Stage.MovePosition((XyCoordinate)(unit.GetEtchingPosition(Config.GetOffsetVisionToScanner(i))))) != 0) return ret;
                    //if ((ret = Stage.MovePosition((XyCoordinate)(unit.EtchingPosition))) != 0) return ret;
                    Scanner.EtchingAngle = unit.EtchingPosition.T;
                    if ((ret = Scanner.Work()) != 0) return ret;
                    //Thread.Sleep(1000);
                    unit.WorkStatus = SmtUnit.UnitWorkStateKey.Complete;
                    unit.ProcessTime = DateTime.Now;
                    Equipment.ProductData.TotalCount++;
                    Equipment.ProductData.OKCount++;
                }

                
            }*/

            //Scanner.EnableLaserEmission(false);

            //Carrier.CurrentState = Carrier.CarrierWorkStateKey.Complete;

            return ret;
        }

        public int RunEtchingProcess(int nIndex)
        {
            int ret = 0;

            XyztCoordinate coordinate = Config.GetPositionData(WaferProbeAlignProcessorConfig.PositionKey.Etching.ToString());

/*            if(LaserZ != null)
            {
                if ((ret = LaserZ.MovePosition(coordinate.Z)) != 0) return ret;
            }*/

            //Scanner.EnableLaserEmission(true);

            /*for (int i = 0; i < Carrier.UnitCount; i++)
            {
                SmtUnit unit = Carrier.ArrSmtUnit[i];
                
                if (nIndex == i && unit.UseWork)
                {
                    unit.WorkStatus = SmtUnit.UnitWorkStateKey.Work;
                    if ((ret = Stage.MovePosition((XyCoordinate)(unit.GetEtchingPosition(Config.GetOffsetVisionToScanner(i)))))!= 0) return ret;
                    //if ((ret = Stage.MovePosition((XyCoordinate)(unit.EtchingPosition))) != 0) return ret;
                    Scanner.EtchingAngle = unit.EtchingPosition.T;
                    if ((ret = Scanner.Work()) != 0) return ret;
                    unit.WorkStatus = SmtUnit.UnitWorkStateKey.Complete;
                    unit.ProcessTime = DateTime.Now;
                    break;
                }
            }*/

            //Scanner.EnableLaserEmission(false);

            return ret;
        }

        public Task<int> BeginInput()
        {
            Task<int> task = Task.Factory.StartNew(() =>
            {
                return Input();
            });

            return task;
        }
        public int Input()
        {
            int ret = 0;

            /*if ((ret = Conveyor.MoveToZone()) != 0)
            {
                return ret;
            }

            if (CarrierClamper != null)
            {
                if ((ret = CarrierClamper.Clamp()) != 0)
                {
                    return ret;
                }
            }

            Conveyor.Stop();*/

            return ret;
        }

        public int Output()
        {
            int ret = 0;

            /*Unloader.IsReady = false;
            if((ret = PrepareToUnload()) != 0) return ret;
            Task<int> task = null;
            if(Unloader != null)
                task = Unloader.BeginInput();

            if (CarrierClamper != null)
            {
                if ((ret = CarrierClamper.Unclamp()) != 0)
                {
                    return ret;
                }
            }

            if ((ret = Conveyor.MoveToOut()) != 0)
            {
                return ret;
            }

            if(task != null)
                task.Wait();

            Conveyor.Stop();

            Unloader.Carrier = Carrier;
            Carrier = null;
            this.IsReady = true;

            Unloader.UpdateBarcodeDB();
            Unloader.SendMesResult();*/

            return ret;
        }

        public int MoveAlignPosition(int nIndex)
        {
            int ret = 0;

            /*if(Carrier != null)
            {
                XyCoordinate coordinate = (XyCoordinate)Carrier.ArrSmtUnit[nIndex].EtchingPosition;
                coordinate.Y += Config.ScannerConfig.EtchingOffsetY;
                Stage.BeginMovePosition(coordinate);
            }*/

            return ret;
        }

        public int RunProcess()
        {
            int ret = 0;
            //DateTime startTime = DateTime.Now;

            /*if (Carrier != null)
            {
                Carrier.CurrentState = Carrier.CarrierWorkStateKey.Work;
            }
            
            if ((ret = RunAlignProcess()) != 0) return ret;
            if ((ret = RunEtchingProcess()) != 0) return ret;

            if(Carrier != null)
            {
                Carrier.CurrentState = Carrier.CarrierWorkStateKey.Complete;
            }*/

            //TimeSpan time = DateTime.Now - startTime;
            //EquipmentSLE100.ProductData.TactTime = time.TotalMilliseconds;
            
            return ret;
        }

        protected override int OnRun()
        {
            int ret = base.OnRun();

            /*if(Carrier != null)
            {
                if(Carrier.CurrentState == Carrier.CarrierWorkStateKey.None || Carrier.CurrentState == Carrier.CarrierWorkStateKey.Work)
                {
                    IsReady = false;
                    if ((ret = RunProcess()) != 0) return ret;
                    
                }
                else if(Carrier.CurrentState == Carrier.CarrierWorkStateKey.Complete)
                {
                    if(Unloader.IsReady && Unloader.Carrier == null)
                    {
                        if ((ret = Output()) != 0) return ret;
                        IsReady = true;
                    }                    
                }                
            }*/

            return ret;
        }

        public int EnableDustCollector(bool bEnable)
        {
            int ret = 0;

            if(DustCollector != null)
            {
                ret = DustCollector.SetOnOff(bEnable);
            }

            return ret;
        }

    }
}
