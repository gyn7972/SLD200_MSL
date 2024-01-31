using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QMC.Common.Hmi;
using QMC.Common.VisionPart;

namespace QMC.Common.Parts
{
    [Serializable]
    public class JigAlignerRecipe
    {
        #region Define
        [Serializable]
        public enum Direction
        {
            /// <summary>
            /// 반시계 방향.
            /// </summary>
            CounterClockWise,

            /// <summary>
            /// 시계 방향.
            /// </summary>
            ClockWise,
        }
        [Serializable]
        public enum PathType
        {
            Continuous,
            StepByStep,
        }
        [Serializable]
        public enum PositionAligns
        {
            Reference,
            First,
            Second,
            Third,
            Forth
        }
        #endregion

        #region Field
        [NonSerialized]
        public JigAligner m_Owner;
        #endregion

        #region Constructor
        public JigAlignerRecipe(Part part)
        {
            if (part != null && part is JigAligner)
            {
                m_Owner = part as JigAligner;
            }

            MoveToDelay = 0;

            Init(part);
        }
        #endregion

        #region Property
        public PatternMatchingParameters PatternMatchingParameter { get; set; }

        [Browsable(false)]
        public Point InspectRoiStartLocation { set; get; }
        [Browsable(false)]
        public Point InspectRoiEndLocation { set; get; }
        [Browsable(false)]
        public Point TrainRoiStartLocation { set; get; }
        [Browsable(false)]
        public Point TrainRoiEndLocation { set; get; }
        [Browsable(false)]
        public IlluminationDataSet IlluminationDataSet { set; get; }
        [Browsable(true),ReadOnly(false)]
        public PointD ReferencePosition { set; get; }
        //public XyzCoordinate ReferencePosition { set; get; }
        [Browsable(true), ReadOnly(false)]
        public PointD SecondPositionDistance { set; get; }
        //public XyzCoordinate SecondPositionDistance { set; get; }
        public int MoveToDelay { set; get; }

        [TypeConverter(typeof(NormalExpandableObjectConverter))]
        public PathGenerator pathGenerator { set; get; }
        #endregion

        #region Method
        public void Init(Part part)
        {
            if (PatternMatchingParameter == null)
                PatternMatchingParameter = new PatternMatchingParameters();

            if (TrainRoiStartLocation == null)
                TrainRoiStartLocation = new Point();

            if (TrainRoiEndLocation == null)
                TrainRoiEndLocation = new Point();

            if (InspectRoiStartLocation == null)
                InspectRoiStartLocation = new Point();

            if (InspectRoiEndLocation == null)
                InspectRoiEndLocation = new Point();

            if (IlluminationDataSet == null)
                IlluminationDataSet = new IlluminationDataSet(part.Name);

            if (ReferencePosition == null)
                ReferencePosition = new PointD();
            //ReferencePosition = new XyzCoordinate();

            if (SecondPositionDistance == null)
                SecondPositionDistance = new PointD();
            //SecondPositionDistance = new XyzCoordinate();

            if (pathGenerator == null)
                pathGenerator = new PathGenerator();
        }

        public XyCoordinate GetSecondPosition()
        {
            XyCoordinate secondPos = new XyCoordinate(ReferencePosition.X + SecondPositionDistance.X, ReferencePosition.Y + SecondPositionDistance.Y);
            return secondPos;
        }
        #endregion
    }
}
