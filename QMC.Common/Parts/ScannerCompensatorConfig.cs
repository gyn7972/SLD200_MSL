using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QMC.Common;
using QMC.Common.PathGenerators;
using QMC.Common.Vision.Tools;
using static QMC.Common.Parts.ScannerCompensator;

namespace QMC.Common.Parts
{
    [Serializable]
    public class ScannerCompensatorConfig
    {
        #region Field
        private double m_PitchDistanceX;
        private double m_PitchDistanceY;
        private Point m_Count;
        private ZigzagTwoDimensionPathGenerator.StartLocation m_StartLocation;
        private ZigzagTwoDimensionPathGenerator.Direction m_Direction;
        private SearchMethod m_SearchMethod;
        private int m_CenterCheckCount;
        #endregion

        #region Constructor
        public ScannerCompensatorConfig()
        {
            Init();
        }
        #endregion

        #region Property
        [Browsable(false)]
        public List<PositionOffset> XyGridSearchResults { get; set; }
        [Browsable(false)]
        public List<XyCoordinate> Positions { get; set; }
        [Browsable(false)]
        public XyzPositionDataCollection GridPositions { set; get; }

        [Category("GridXY")]
        [Browsable(true)]
        public double PitchDistanceX
        {
            get { return m_PitchDistanceX; }
            set { m_PitchDistanceX = value; }
        }
        [Category("GridXY")]
        [Browsable(true)]
        public double PitchDistanceY
        {
            get { return m_PitchDistanceY; }
            set { m_PitchDistanceY = value; }
        }

        [Category("GridXY")]
        [Browsable(true)]
        public Point Count
        {
            get { return m_Count; }
            set { m_Count = value; }
        }

        //TODO : 추후 PathGenerator로 변경한다면 Browsable true로 변경
        [Category("GridXY")]
        [Browsable(false)]
        public ZigzagTwoDimensionPathGenerator.StartLocation StartLocation
        {
            get { return this.m_StartLocation; }
            set { this.m_StartLocation = value; }
        }

        //TODO : 추후 PathGenerator로 변경한다면 Browsable true로 변경
        [Category("GridXY")]
        [Browsable(false)]
        public ZigzagTwoDimensionPathGenerator.Direction Direction
        {
            get { return m_Direction; }
            set { m_Direction = value; }
        }
        [Browsable(true)]
        public int MoveToDelay { get; set; }

        [Category("Blob")]
        [Browsable(true)]
        public SearchMethod SearchMethod
        {
            get { return m_SearchMethod; }
            set { m_SearchMethod = value; }
        }

        [Category("GridXY")]
        [Browsable(true)]
        public int CenterCheckCount
        {
            get { return m_CenterCheckCount; }
            set { m_CenterCheckCount = value; }
        }

        [Category("GridXY")]
        [Browsable(true)]
        public int AverageCount { get; set; }
        #endregion

        #region Method
        public void Init()
        {
            if (XyGridSearchResults == null)
                XyGridSearchResults = new List<PositionOffset>();

            if (Positions == null)
                Positions = new List<XyCoordinate>();

            if (GridPositions == null)
                GridPositions = new XyzPositionDataCollection();

            GridPositions.Clear();
            foreach (GridXyMotionPositionKeys key in Enum.GetValues(typeof(GridXyMotionPositionKeys)))
            {
                XyzPositionData positionBase = new XyzPositionData();
                positionBase.Name = key.ToString();
                GridPositions.Add(positionBase);

                XyzPositionData positionTarget = new XyzPositionData();
                positionTarget.Name = key.ToString();
                positionTarget.Type = TargetType.Offset;
                GridPositions.Add(positionTarget);
            }

            m_Direction = ZigzagTwoDimensionPathGenerator.Direction.Horizontal;
            m_PitchDistanceX = 0;
            m_PitchDistanceY = 0;
            m_Count = new Point();
            m_StartLocation = new ZigzagTwoDimensionPathGenerator.StartLocation();
            this.MoveToDelay = 100;
            this.SearchMethod = SearchMethod.PatternMatching;
            this.CenterCheckCount = 15;
            this.AverageCount = 5;
        }
        #endregion
    }
}
