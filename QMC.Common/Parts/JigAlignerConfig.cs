using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QMC.Common.VisionPart.VisionCalibratorConfig;

namespace QMC.Common.Parts
{
    [Serializable]
    public class JigAlignerConfig
    {
        #region Define
        [Serializable]
        public enum PositionVisionCal
        {
            Start,
        }
        #endregion

        #region Field
        public JigAligner m_Owner;
        #endregion

        #region Property
        public double MoveDistance { get; set; }

        public XyzPositionDataCollection AlignPositions { set; get; }
        #endregion

        #region Constructor
        public JigAlignerConfig()
        {
            Init();
        }
        #endregion

        #region Method
        public void Init()
        {

            if (AlignPositions == null)
            {
                AlignPositions = new XyzPositionDataCollection();
                AlignPositions.Clear();

                foreach (PositionVisionCal key in Enum.GetValues(typeof(PositionVisionCal)))
                {
                    XyzPositionData positionBase = new XyzPositionData();
                    positionBase.Name = key.ToString();
                    AlignPositions.Add(positionBase);

                    XyzPositionData positionTarget = new XyzPositionData();
                    positionTarget.Name = key.ToString();
                    positionTarget.Type = TargetType.Offset;
                    AlignPositions.Add(positionTarget);
                }
            }

        }

        public void SetAlignPosition(string strPositionKey, TargetType targetType, XyzCoordinate coordinate)
        {
            foreach (XyzPositionData position in AlignPositions)
            {
                if (position.Name == strPositionKey && position.Type == targetType)
                {
                    position.X = coordinate.X;
                    position.Y = coordinate.Y;
                    position.Z = coordinate.Z;
                    //position.T = coordinate.T;
                    break;
                }
            }
        }

        public XyzCoordinate GetCalibratorPosition(string strPositionKey, TargetType targetType)
        {
            XyzCoordinate coordinate = new XyzCoordinate();
            foreach (XyzPositionData position in AlignPositions)
            {
                if (position.Name == strPositionKey && position.Type == targetType)
                {
                    coordinate = position.Coordinate;
                    break;
                }
            }
            return coordinate;
        }
        #endregion
    }
}
