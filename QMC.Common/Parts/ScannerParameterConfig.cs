using QMC.Common;
using QMC.Common.Hmi;
using QMC.Common.Parts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace QMC.Process.WorkStage.Parts
namespace QMC.Common.Parts
{
    [Serializable]

    public class ScannerParameterConfig
    {
        private List<XyCoordinate> m_offsetVisionToScanner;
        public enum ScannerModel
        {
            RTC4,
            RTC5,
            RTC6,
        }

        [Category("Scanner")]
        public ScannerModel Model { set; get; }
        [Category("Scanner")]
        public double Fov { set; get; }
        [Category("Scanner")]
        public string CorrectionFile { set; get; }

        [Category("Laser")]
        public int LaserMaxPower { set; get; }

        [Category("Laser Calibration")]
        public float VerifyRange { set; get; }

        [TypeConverter(typeof(XyPositionExpandableObjectConverter))]
        public List<XyCoordinate> OffsetVisionToScanner 
        { 
            set
            {
                m_offsetVisionToScanner = value;
            }
            get
            {
                return m_offsetVisionToScanner;
            }
        }

        public double EtchingOffsetY { set; get; }

        public bool SimulationMode { set; get; }

        public LaserConfig LaserConfig { set; get; }
        public ScannerParameterConfig()
        {
            Model = ScannerModel.RTC4;
            LaserMaxPower = 200;
            Fov = 128.82;
            CorrectionFile = "D:\\Temp\\Cor_1to1.ctb";

            //OffsetVisionToScanner = new XyCoordinate(147.54, -1.159);
            OffsetVisionToScanner = new List<XyCoordinate>();
            OffsetVisionToScanner.Add(new XyCoordinate(147.64,  -1.223));
            OffsetVisionToScanner.Add(new XyCoordinate(147.599, -1.248));
            OffsetVisionToScanner.Add(new XyCoordinate(147.566, -1.253));
            OffsetVisionToScanner.Add(new XyCoordinate(147.582, -1.275));
            OffsetVisionToScanner.Add(new XyCoordinate(147.612, -1.252));
            OffsetVisionToScanner.Add(new XyCoordinate(147.637, -1.224));
            OffsetVisionToScanner.Add(new XyCoordinate(147.607, -1.234));
            OffsetVisionToScanner.Add(new XyCoordinate(147.567, -1.244));
            OffsetVisionToScanner.Add(new XyCoordinate(147.587, -1.253));
            OffsetVisionToScanner.Add(new XyCoordinate(147.617, -1.264));

            EtchingOffsetY = -0.68;
            VerifyRange = 0.5f;
            SimulationMode = false;
        }

    }
}
