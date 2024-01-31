using QMC.Common.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.VisionPart
{
    [Serializable]
    public class VisionCalibratorConfig
    {
        #region Define
        [Serializable]
        public enum PositionVisionCal
        {
            Start,
        }
        #endregion

        #region Property
        public double X
        {
            get
            {
                return Scale.X;
            }
            set
            {
                Scale.X = value;
            }
        }
        
        public double Y
        {
            get
            {
                return Scale.Y;
            }
            set
            {
                Scale.Y = value;
            }
        }
        
        public double XAxisT
        {
            get
            {
                return Scale.XAxisT;
            }
            set
            {
                Scale.XAxisT = value;
            }
        }
        
        public double YAxisT
        {
            get
            {
                return Scale.YAxisT;
            }
            set
            {
                Scale.YAxisT = value;
            }
        }
        

        
        public bool XInveted
        {
            get
            {
                return Scale.InvertedX;
            }
            set
            {
                Scale.InvertedX = value;
            }
        }
        
        public bool YInveted
        {
            get
            {
                return Scale.InvertedY;
            }
            set
            {
                Scale.InvertedY = value;
            }
        }
        
        public bool Enable
        {
            get
            {
                return Scale.UsedScaleT;
            }
            set
            {
                Scale.UsedScaleT = value;
            }
        }

        public double MoveDistance { get; set; }

        //public XyztPositionDataCollection VisionCalPositions { set; get; }
        //public XyzztPositionDataCollection VisionCalPositions { set; get; }
        public UvwzxyzPositionDataCollection VisionCalPositions { set; get; }

        public VisionScale Scale { get; set; }
        #endregion

        #region Constructor
        public VisionCalibratorConfig()
        {
            Init();
            X = 0;
            Y = 0;
            XAxisT = 0;
            YAxisT = 0;
            MoveDistance = 0;
            
            XInveted = false;
            YInveted = false;
            Enable = false;
        }
        #endregion

        #region Method
        public void Init()
        {
            if(Scale == null)
                Scale = new VisionScale();

            if(VisionCalPositions == null)
            {
                //VisionCalPositions = new XyztPositionDataCollection();
                //VisionCalPositions = new XyzztPositionDataCollection();
                VisionCalPositions = new UvwzxyzPositionDataCollection();
                VisionCalPositions.Clear();

                foreach (PositionVisionCal key in Enum.GetValues(typeof(PositionVisionCal)))
                {
                    //XyztPositionData positionBase = new XyztPositionData();
                    //XyzztPositionData positionBase = new XyzztPositionData();
                    UvwzxyzPositionData positionBase = new UvwzxyzPositionData();

                    positionBase.Name = key.ToString();
                    VisionCalPositions.Add(positionBase);

                    //XyztPositionData positionTarget = new XyztPositionData();
                    //XyzztPositionData positionTarget = new XyzztPositionData();
                    UvwzxyzPositionData positionTarget = new UvwzxyzPositionData();

                    positionTarget.Name = key.ToString();
                    positionTarget.Type = TargetType.Offset;
                    VisionCalPositions.Add(positionTarget);
                }
            }
            
        }

        //public void SetCalibratorPosition(string strPositionKey, TargetType targetType, XyztCoordinate coordinate)
        //{
        //    foreach (XyztPositionData position in VisionCalPositions)
        //    {
        //        if (position.Name == strPositionKey && position.Type == targetType)
        //        {
        //            position.X = coordinate.X;
        //            position.Y = coordinate.Y;
        //            position.Z = coordinate.Z;
        //            position.T = coordinate.T;
        //            break;
        //        }
        //    }
        //}

        //public void SetCalibratorPosition(string strPositionKey, TargetType targetType, XyzztCoordinate coordinate)
        //{
        //    foreach (XyzztPositionData position in VisionCalPositions)
        //    {
        //        if (position.Name == strPositionKey && position.Type == targetType)
        //        {
        //            position.X = coordinate.X;
        //            position.Y = coordinate.Y;
        //            position.IZ = coordinate.IZ;
        //            position.SZ = coordinate.SZ;
        //            position.T = coordinate.T;
        //            break;
        //        }
        //    }
        //}

        public void SetCalibratorPosition(string strPositionKey, TargetType targetType, UvwzxyzCoordinate coordinate)
        {
            foreach (UvwzxyzPositionData position in VisionCalPositions)
            {
                if (position.Name == strPositionKey && position.Type == targetType)
                {
                    position.U = coordinate.U;
                    position.V = coordinate.V;
                    position.W = coordinate.W;
                    position.EZ = coordinate.EZ;
                    position.X = coordinate.X;
                    position.Y = coordinate.Y;
                    position.VZ = coordinate.VZ;
                    break;
                }
            }
        }

        //public XyztCoordinate GetCalibratorPosition(string strPositionKey, TargetType targetType)
        //{
        //    XyztCoordinate coordinate = new XyztCoordinate();
        //    foreach (XyztPositionData position in VisionCalPositions)
        //    {
        //        if (position.Name == strPositionKey && position.Type == targetType)
        //        {
        //            coordinate = position.Coordinate;
        //            break;
        //        }
        //    }
        //    return coordinate;
        //}

        //public XyzztCoordinate GetCalibratorPosition(string strPositionKey, TargetType targetType)
        //{
        //    XyzztCoordinate coordinate = new XyzztCoordinate();
        //    foreach (XyzztPositionData position in VisionCalPositions)
        //    {
        //        if (position.Name == strPositionKey && position.Type == targetType)
        //        {
        //            coordinate = position.Coordinate;
        //            break;
        //        }
        //    }
        //    return coordinate;
        //}

        public UvwzxyzCoordinate GetCalibratorPosition(string strPositionKey, TargetType targetType)
        {
            UvwzxyzCoordinate coordinate = new UvwzxyzCoordinate();
            foreach (UvwzxyzPositionData position in VisionCalPositions)
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
