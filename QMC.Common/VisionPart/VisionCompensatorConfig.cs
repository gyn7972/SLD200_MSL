using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.VisionPart
{
    #region VisionCompensatorConfig
    [Serializable]
    public class VisionCompensatorConfig
    {
        #region Property
        public List<PositionOffset> XyGridSearchResults { get; set; }
        public List<XyCoordinate> Positions { get; set; }
        public VisionCompensatorParameter Parameter { get; set; }
        public string SaveFilePath { set; get; }
        public string FailImageSavePath { set; get; }
        #endregion

        #region Constructor
        public VisionCompensatorConfig()
        {
            Init();

            //if (XyGridSearchResults == null)
            //    XyGridSearchResults = new List<PositionOffset>();

            //if (Parameter == null)
            //    Parameter = new VisionCompensatorParameter();

            //if(Positions == null)
            //    Positions = new List<XyCoordinate>();
        }
        #endregion

        #region Method
        public void Init()
        {
            if (XyGridSearchResults == null)
                XyGridSearchResults = new List<PositionOffset>();

            if (Parameter == null)
                Parameter = new VisionCompensatorParameter();

            if (Positions == null)
                Positions = new List<XyCoordinate>();

            Parameter.UpdateGridPosition();

            if (string.IsNullOrEmpty(SaveFilePath))
            {
                SaveFilePath = "D:\\QMCStageMap.csv";
            }
        }

        public void SetPosition(string strPosition, TargetType type, UvwzxyzCoordinate coordinate)
        {
            bool bFind = false;
            foreach (var position in Parameter.GridPositions)
            {
                if (position.Name == strPosition && position.Type == type)
                {
                    position.Coordinate = coordinate;
                    bFind = true;
                    break;
                }
            }

            if (!bFind)
            {
                UvwzxyzPositionData positionData = new UvwzxyzPositionData();
                positionData.Coordinate = coordinate;
                positionData.Type = type;
                positionData.Name = strPosition;

                Parameter.GridPositions.Add(positionData);
            }
        }

        public UvwzxyzCoordinate GetPosition(string strPosition)
        {
            UvwzxyzCoordinate coordinate = new UvwzxyzCoordinate();

            foreach (var position in Parameter.GridPositions)
            {
                if (position.Name == strPosition)
                {
                    coordinate += position.Coordinate;
                }
            }

            return coordinate;
        }
        #endregion
    }
    #endregion
}
