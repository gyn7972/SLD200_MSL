using QMC.Common.Vision.Optics.Leesos;
using QMC.Common.Opticon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QMC.Common.Parts;
using QMC.Common.Yamatake;

namespace QMC.Common.Modules
{
    #region CommonModule
    public class CommonModule : Module
    {
        #region Singleton 
        private static CommonModule g_Common;
        public static CommonModule Instance
        {
            get
            {
                if (g_Common == null)
                    g_Common = new CommonModule("Common");
                return g_Common;
            }
        }
        #endregion
        public DigitalIlluminator Illuminator { set; get; }
        //public OpticonBarcodeReader BarcodeReader { set; get; }
        public MCW400A100GaugeCommunicator ColletGaugeCommunicator1 { set; get; }
        public MCW400A100GaugeCommunicator ColletGaugeCommunicator2 { set; get; }

        public TowerLamp TowerLamp { set; get; }
        public bool TowerLamp_BuzzerStop { set; get; } = false;

        public CommonConfig Config { set; get; }

        public OperationButtons OperationButtons { set; get; }

        public CommonModule(string strName) : base(strName)
        {
            Config = new CommonConfig();
        }

        public override int Create()
        {
            int ret = 0;

            Illuminator = new DigitalIlluminator("Illuminator");
            Illuminator.Create();
            Illuminator.Owner = this;
            Parts.Add(Illuminator);

            Illuminator.Config = Config.IlluminatorConfig;

            TowerLamp = new TowerLamp("TowerLamp");
            TowerLamp.Create();
            TowerLamp.Owner = this;
            Parts.Add(TowerLamp);

            OperationButtons = new OperationButtons("OperationButtons");
            OperationButtons.Create();
            OperationButtons.Owner = this;
            Parts.Add(OperationButtons);

            /*BarcodeReader = new OpticonBarcodeReader("Barcode");
            BarcodeReader.Create();
            BarcodeReader.Owner = this;
            Parts.Add(BarcodeReader);
            BarcodeReader.Config = Config.BarcodeReaderConfig;*/

            return ret;
        }

        public override void Close()
        {
            if (Illuminator != null)
            {
                Illuminator.TurnOnOff(false, 1);
                Illuminator.TurnOnOff(false, 2);
                Illuminator.Close();
            }

            if (TowerLamp != null)
            {
                TowerLamp.Buzzer_Off();
                TowerLamp.Red_Off();
                TowerLamp.Green_Off();
                TowerLamp.Yellow_Off();
                TowerLamp.Close();
            }

            //if (BarcodeReader != null)
            //    BarcodeReader.Close();
        }

        public override void SetConfigData(object parameter)
        {
            Config = parameter as CommonConfig;
            if (Config == null)
            {
                Config = new CommonConfig();
            }

            Config.Init();

            Illuminator.Config = Config.IlluminatorConfig;
            //BarcodeReader.Config = Config.BarcodeReaderConfig;
        }

        public override object GetConfigData()
        {
            return Config;
        }

        public override int Initialize()
        {
            int ret = 0;
            if ((ret = base.Initialize()) != 0) return ret;

            foreach (Part part in Parts)
            {
                if (part.Name != "Illuminator")                                 //  조명을 여기서 초기화 하지 않도록 한다.
                {
                    if ((ret = part.Initialize()) != 0) return ret;
                }
            }

            return ret;
        }

        public override void SetModuleScale(double dScaleX, double dScaleY, double dXaxisT, double dYaxisT, bool bInvertedX, bool bInvertedY)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
}
