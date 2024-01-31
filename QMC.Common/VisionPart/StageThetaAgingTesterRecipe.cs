using QMC.Common.Modules;
using QMC.Common.Parts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.VisionPart
{
    [Serializable]
    public class StageThetaAgingTesterRecipe
    {
        #region Define
        [Serializable]
        public enum PositionKeys
        {
            First,
            Second,
        }

        #endregion        

        public StageThetaAgingTesterRecipe(Part part)
        {           

            InspectRoiStartLocation = new Point();
            InspectRoiEndLocation = new Point();
            TrainRoiStartLocation = new Point();
            TrainRoiEndLocation = new Point();

            RepeatCount = 0;
            MoveDelay = 100;
            ResultFilePath = string.Empty;

            //GetAxisInformation();
            Init(part);
        }

        #region Property

        [Browsable(false)]
        public Point InspectRoiStartLocation { set; get; }
        [Browsable(false)]
        public Point InspectRoiEndLocation { set; get; }
        [Browsable(false)]
        public Point TrainRoiStartLocation { set; get; }
        [Browsable(false)]
        public Point TrainRoiEndLocation { set; get; }
        [Browsable(false)]

        public XytPositionDataCollection Positions { set; get; }
        [Browsable(false)]
        public PatternMatchingParameters PatternMatchingParameter { set; get; }
        [Browsable(false)]
        public PatternMatchingParameters SecondPatternMatchingParameter { set; get; }
        [Browsable(false)]
        public IlluminationDataSet IlluminationDataSet { set; get; }

        public int RepeatCount { set; get; }

        [Category("X")]
        public double Velovity_X { set; get; }
        [Category("X")]
        public double Accelelation_X { set; get; }
        [Category("X")]
        public double Deceleration_X { set; get; }

        [Category("Y")]
        public double Velovity_Y { set; get; }
        [Category("Y")]
        public double Accelelation_Y { set; get; }
        [Category("Y")]
        public double Deceleration_Y { set; get; }

        [Category("T")]
        public double Velovity_T { set; get; }
        [Category("T")]
        public double Accelelation_T { set; get; }
        [Category("T")]
        public double Deceleration_T { set; get; }

        public int MoveDelay { set; get; }

        public string ResultFilePath { set; get; }

        #endregion


        #region Method

       

        public void Init(Part part)
        {
            if (PatternMatchingParameter == null)
            {
                PatternMatchingParameter = new PatternMatchingParameters();
            }

            if (SecondPatternMatchingParameter == null)
            {
                SecondPatternMatchingParameter = new PatternMatchingParameters();
            }

            if (IlluminationDataSet == null)
            {
                IlluminationDataSet = new IlluminationDataSet(part.Name);
            }

            if (Positions == null)
            {
                Positions = new XytPositionDataCollection();
                foreach (PositionKeys key in Enum.GetValues(typeof(PositionKeys)))
                {
                    XytPositionData positionBase = new XytPositionData();
                    positionBase.Name = key.ToString();
                    //positionBase.Coordinate = 
                    Positions.Add(positionBase);

                    XytPositionData positionTarget = new XytPositionData();
                    positionTarget.Name = key.ToString();
                    positionTarget.Type = TargetType.Offset;
                    Positions.Add(positionTarget);
                }
            }
        }

        public List<string> GetPositionList()
        {
            List<string> ret = new List<string>();
            foreach (XytPositionData position in Positions)
            {
                if (position.Type == TargetType.Base)
                    ret.Add(position.Name);
            }
            return ret;
        }

        public void SetPositionData(string strPosition, TargetType targetType, XytCoordinate coordinate)
        {
            foreach (XytPositionData position in Positions)
            {
                if (position.Name == strPosition && position.Type == targetType)
                {
                    position.X = coordinate.X;
                    position.Y = coordinate.Y;
                    position.T = coordinate.T;
                    break;
                }
            }
        }


        public XytCoordinate GetPositionData(string strPosition)
        {
            XytCoordinate coordinate = new XytCoordinate();

            foreach (XytPositionData position in Positions)
            {
                if (position.Name == strPosition)
                {
                    coordinate += position.Coordinate;
                    break;
                }
            }

            return coordinate;
        }
        #endregion
    }
}
