using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Parts
{
    [Serializable]
    public class NeedleBlockConfig
    {
        protected ZPositionDataCollection m_CapPosition;
        protected XyPositionDataCollection m_NeedleBlockPosition;
        protected ZPositionDataCollection m_NeedlePosition;
        protected List<ColletOffset> m_ColletOffsets;

        public enum PositionCap
        {
            Ready,
            Detach,
            ResetPosition,
        }

        public enum PositionNeedleBlock
        {
            Center,
            ChangeNeedle,
            XYCalibratePosition,
        }

        public enum PositionNeedle
        {
            GoBack,
        }

        [Browsable(false)]
        public ZPositionDataCollection CapPositions
        {
            get { return m_CapPosition; }
            set { m_CapPosition = value; }
        }
        [Browsable(false)]
        public XyPositionDataCollection NeedleBlockPosition
        {
            get { return m_NeedleBlockPosition; }
            set { m_NeedleBlockPosition = value; }
        }
        [Browsable(false)]
        public ZPositionDataCollection NeedlePosition
        {
            get { return m_NeedlePosition; }
            set { m_NeedlePosition = value; }
        }
        [Browsable(false)]
        public List<ColletOffset> ColletOffsets
        {
            get { return m_ColletOffsets; }
            set { m_ColletOffsets = value; }
        }
        [Category("NeedleBlock")]
        public double ResetPositionInterval { set; get; }
        [Category("NeedleBlock")]
        public double Radius { set; get; }

        [Category("Needle")]
        public int NeedleLifeCycleCount { set; get; }

        [Category("Needle")]
        public double AfterResetPosition { set; get; }

        [Category("Needle")]
        public bool UsePickNonSyncMode { set; get; }

        [Category("Needle")]
        public int BeforeMoveColletDelay { set; get; }

        [Category("Needle")]
        public int BeforeMoveEjectorPinDelay { set; get; }

        public NeedleBlockConfig()
        {
            m_CapPosition = new ZPositionDataCollection();
            m_NeedleBlockPosition = new XyPositionDataCollection();
            m_NeedlePosition = new ZPositionDataCollection();

            ResetPositionInterval = 0;
            NeedleLifeCycleCount = 0;
            AfterResetPosition = 0;
            BeforeMoveColletDelay = 0;
            BeforeMoveEjectorPinDelay = 0;
            Radius = 0;
            Init();

        }
        //public List<ZPositionData> Positions { set; get; }

        public void Init()
        {
            m_CapPosition.Clear();

            foreach (PositionCap key in Enum.GetValues(typeof(PositionCap)))
            {
                ZPositionData positionBase = new ZPositionData();
                positionBase.Name = key.ToString();
                m_CapPosition.Add(positionBase);

                ZPositionData positionTarget = new ZPositionData();
                positionTarget.Name = key.ToString();
                positionTarget.Type = TargetType.Offset;
                m_CapPosition.Add(positionTarget);
            }

            m_NeedleBlockPosition.Clear();
            foreach (PositionNeedleBlock key in Enum.GetValues(typeof(PositionNeedleBlock)))
            {
                XyPositionData positionBase = new XyPositionData();
                positionBase.Name = key.ToString();
                m_NeedleBlockPosition.Add(positionBase);

                XyPositionData positionTarget = new XyPositionData();
                positionTarget.Name = key.ToString();
                positionTarget.Type = TargetType.Offset;
                m_NeedleBlockPosition.Add(positionTarget);
            }

            m_NeedlePosition.Clear();
            foreach (PositionNeedle key in Enum.GetValues(typeof(PositionNeedle)))
            {
                ZPositionData positionBase = new ZPositionData();
                positionBase.Name = key.ToString();
                m_NeedlePosition.Add(positionBase);

                ZPositionData positionTarget = new ZPositionData();
                positionTarget.Name = key.ToString();
                positionTarget.Type = TargetType.Offset;
                m_NeedlePosition.Add(positionTarget);
            }
        }

        public List<string> GetCapPositionList()
        {
            List<string> ret = new List<string>();
            foreach (ZPositionData position in m_CapPosition)
            {
                if (position.Type == TargetType.Base)
                    ret.Add(position.Name);
            }
            return ret;
        }

        public List<string> GetNeedleBlockPositionList()
        {
            List<string> ret = new List<string>();
            foreach (XyPositionData position in m_NeedleBlockPosition)
            {
                if (position.Type == TargetType.Base)
                    ret.Add(position.Name);
            }
            return ret;
        }

        public List<string> GetNeedlePositionList()
        {
            List<string> ret = new List<string>();
            foreach (ZPositionData position in m_NeedlePosition)
            {
                if (position.Type == TargetType.Base)
                    ret.Add(position.Name);
            }
            return ret;
        }
        public void SetNeedleBlockPosition(string strPositionKey, TargetType targetType, XyCoordinate coordinate)
        {
            foreach (XyPositionData position in m_NeedleBlockPosition)
            {
                if (position.Name == strPositionKey && position.Type == targetType)
                {
                    position.X = coordinate.X;
                    position.Y = coordinate.Y;
                    break;
                }
            }
        }
        public void SetCapPosition(string strPositionKey, TargetType targetType, double dPosition)
        {
            foreach (ZPositionData position in m_CapPosition)
            {
                if (position.Name == strPositionKey && position.Type == targetType)
                {
                    position.Z = dPosition;
                    break;
                }
            }
        }

        public void SetNeedlePosition(string strPositionKey, TargetType targetType, double dPosition)
        {
            foreach (ZPositionData position in m_NeedlePosition)
            {
                if (position.Name == strPositionKey && position.Type == targetType)
                {
                    position.Z = dPosition;
                    break;
                }
            }
        }

        public double GetCapPosition(string strPositionKey, TargetType targetType)
        {
            double dPosition = 0;
            foreach (ZPositionData position in m_CapPosition)
            {
                if (position.Name == strPositionKey && position.Type == targetType)
                {
                    dPosition = position.Z;
                    break;
                }
            }
            return dPosition;
        }
        public double GetNeedlePosition(string strPositionKey, TargetType targetType)
        {
            double dPosition = 0;
            foreach (ZPositionData position in m_NeedlePosition)
            {
                if (position.Name == strPositionKey && position.Type == targetType)
                {
                    dPosition = position.Z;
                    break;
                }
            }
            return dPosition;
        }

        public XyCoordinate GetNeedleBlockPosition(string strPositionKey, TargetType targetType)
        {
            XyCoordinate coordinate = new XyCoordinate();
            foreach (XyPositionData position in m_NeedleBlockPosition)
            {
                if (position.Name == strPositionKey && position.Type == targetType)
                {
                    coordinate = position.Coordinate;
                    break;
                }
            }
            return coordinate;
        }


    }
}
