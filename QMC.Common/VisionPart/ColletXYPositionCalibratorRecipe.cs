using QMC.Common.Vision.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]

public enum Polarity
{
    LightBlobs,
    DarkBlobs
}
namespace QMC.Common.VisionPart
{
    
    [Serializable]
    public class ColletXYPositionCalibratorRecipe
    {
        public IlluminationDataSet IlluminationDataSet { set; get; }

        [Browsable(false)]
        public Point InspectRoiStartLocation { set; get; }
        [Browsable(false)]
        public Point InspectRoiEndLocation { set; get; }
        [Browsable(false)]
        public Point TrainRoiStartLocation { set; get; }
        [Browsable(false)]
        public Point TrainRoiEndLocation { set; get; }

        public PathParameter PathParameter { set; get; }
        public XyztCoordinate CenterCoordinate { set; get; }
        public bool UseFinePathGenerator { set; get; }

        public double ColletDiameter { set; get; }

        public int HardThreshold { get; set; }

        public Polarity Porarity { get; set; }

        public ColletXYPositionCalibratorRecipe(Part part)
        {
            //IlluminationDataSets = new IlluminationDataList();
            IlluminationDataSet = new IlluminationDataSet(part.Name);
            PathParameter = new PathParameter();
            InspectRoiStartLocation = new Point(0, 0);
            InspectRoiEndLocation = new Point(100, 100);
            TrainRoiStartLocation = new Point(0, 0);
            TrainRoiEndLocation = new Point(100, 100);
            UseFinePathGenerator = false;
            this.ColletDiameter = 70.0;
            this.HardThreshold = 130;
        }

    }
}
