using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Parts
{
    public class WaferProbeAlignParameterRecipe
    {
        //protected List<XyztPositionData> m_WaferProbeAlignPosition;
        //protected List<XyzztPositionData> m_WaferProbeAlignPosition;
        protected List<UvwzxyzPositionData> m_WaferProbeAlignPosition;

        /*
        public enum PositionVision1
        {
            Ready,

            NeedleChange,
            NeedleClean,
            PurgeShot,
            DummyShot,
            Scale,
            NeedleHeightCal,

            CamPos_LeftTopPCB,
            NeedlePos_LeftTopPCB,
            ContactSensor_LeftTopPCB
        }

        public enum PositionVision2
        {
            Ready,

            NeedleChange,
            NeedleClean,
            PurgeShot,
            DummyShot,
            Scale,
            NeedleHeightCal,

            CamPos_LeftTopPCB,
            NeedlePos_LeftTopPCB,
            ContactSensor_LeftTopPCB
        }

        [Browsable(false)]
        public List<XyzPositionData> InspVision1Positions
        {
            get { return m_InspVision1Position; }
            set { m_InspVision1Position = value; }
        }

        [Browsable(false)]
        public List<XyPositionData> InspVision2Positions
        {
            get { return m_InspVision2Position; }
            set { m_InspVision2Position = value; }
        }
        */


        public WaferProbeAlignParameterRecipe()
        {
            //m_InspVision1Position = new List<XyzPositionData>();
            //m_InspVision2Position = new List<XyPositionData>();

            Init();
        }
        //public List<ZPositionData> Positions { set; get; }

        protected void Init()
        {
            /*
            m_InspVision1Position.Clear();
            m_InspVision2Position.Clear();

            foreach (PositionVision1 key in Enum.GetValues(typeof(PositionVision1)))
            {
                XyzPositionData positionBase = new XyzPositionData();
                positionBase.Name = key.ToString();
                m_InspVision1Position.Add(positionBase);

                XyzPositionData positionTarget = new XyzPositionData();
                //positionTarget.Name = key.ToString();                     //  Offset 부분까지 Name 이 보이면 Position 별 구분이 잘 안됨.
                positionTarget.Type = TargetType.Offset;
                m_InspVision1Position.Add(positionTarget);
            }

            foreach (PositionVision2 key in Enum.GetValues(typeof(PositionVision2)))
            {
                XyPositionData positionBase = new XyPositionData();
                positionBase.Name = key.ToString();
                m_InspVision2Position.Add(positionBase);

                XyPositionData positionTarget = new XyPositionData();
                //positionTarget.Name = key.ToString();                     //  Offset 부분까지 Name 이 보이면 Position 별 구분이 잘 안됨.
                positionTarget.Type = TargetType.Offset;
                m_InspVision2Position.Add(positionTarget);
            }
            */
        }

        public List<string> GetWaferProbeAlignPositionList()
        {
            List<string> ret = new List<string>();
            //foreach (XyztPositionData position in m_WaferProbeAlignPosition)
            //foreach (XyzztPositionData position in m_WaferProbeAlignPosition)
            foreach (UvwzxyzPositionData position in m_WaferProbeAlignPosition)
            {
                if (position.Type == TargetType.Base)
                    ret.Add(position.Name);
            }
            return ret;
        }

        public int SetData(int nStartIndex, SettingParameterCollection parameters)
        {
            int i = nStartIndex;
            //foreach (XyztPositionData pos in m_WaferProbeAlignPosition)
            //foreach (XyzztPositionData pos in m_WaferProbeAlignPosition)
            foreach (UvwzxyzPositionData pos in m_WaferProbeAlignPosition)
            {
                if (i >= parameters.Count)
                    return i;
                pos.U = parameters[i++].DoubleValue;

                if (i >= parameters.Count)
                    return i;
                pos.V = parameters[i++].DoubleValue;

                if (i >= parameters.Count)
                    return i;
                pos.W = parameters[i++].DoubleValue;

                if (i >= parameters.Count)
                    return i;
                pos.EZ = parameters[i++].DoubleValue;

                if (i >= parameters.Count)
                    return i;
                pos.X = parameters[i++].DoubleValue;

                if (i >= parameters.Count)
                    return i;
                pos.Y = parameters[i++].DoubleValue;

                if (i >= parameters.Count)
                    return i;
                pos.VZ = parameters[i++].DoubleValue;
            }

            return i;
        }

        public SettingParameterCollection GetData()
        {
            SettingParameterCollection parameters = new SettingParameterCollection();

            //foreach (XyztPositionData pos in m_WaferProbeAlignPosition)
            //foreach (XyzztPositionData pos in m_WaferProbeAlignPosition)
            foreach (UvwzxyzPositionData pos in m_WaferProbeAlignPosition)
            {
                {
                    SettingParameter param = new SettingParameter(pos.Name, DataType.Double);
                    param.DoubleValue = pos.U;
                    param.Tag = WaferProbeAlignParameter.MotionKey.U.ToString();
                    param.Spare = pos.Type.ToString();
                    parameters.Add(param);
                }

                {
                    SettingParameter param = new SettingParameter(pos.Name, DataType.Double);
                    param.DoubleValue = pos.V;
                    param.Tag = WaferProbeAlignParameter.MotionKey.V.ToString();
                    param.Spare = pos.Type.ToString();
                    parameters.Add(param);
                }

                {
                    SettingParameter param = new SettingParameter(pos.Name, DataType.Double);
                    param.DoubleValue = pos.W;
                    param.Tag = WaferProbeAlignParameter.MotionKey.W.ToString();
                    param.Spare = pos.Type.ToString();
                    parameters.Add(param);
                }

                {
                    SettingParameter param = new SettingParameter(pos.Name, DataType.Double);
                    param.DoubleValue = pos.EZ;
                    param.Tag = WaferProbeAlignParameter.MotionKey.EZ.ToString();
                    param.Spare = pos.Type.ToString();
                    parameters.Add(param);
                }

                {
                    SettingParameter param = new SettingParameter(pos.Name, DataType.Double);
                    param.DoubleValue = pos.X;
                    param.Tag = WaferProbeAlignParameter.MotionKey.X.ToString();
                    param.Spare = pos.Type.ToString();
                    parameters.Add(param);
                }

                {
                    SettingParameter param = new SettingParameter(pos.Name, DataType.Double);
                    param.DoubleValue = pos.Y;
                    param.Tag = WaferProbeAlignParameter.MotionKey.Y.ToString();
                    param.Spare = pos.Type.ToString();
                    parameters.Add(param);
                }

                {
                    SettingParameter param = new SettingParameter(pos.Name, DataType.Double);
                    param.DoubleValue = pos.VZ;
                    param.Tag = WaferProbeAlignParameter.MotionKey.VZ.ToString();
                    param.Spare = pos.Type.ToString();
                    parameters.Add(param);
                }
            }

            return parameters;
        }
    }
}
