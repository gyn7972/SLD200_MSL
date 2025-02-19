using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Parts
{
    public class UnloaderParameterRecipe
    {
        protected List<ZzxzxyPositionData> m_UnloaderPosition;

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


        public UnloaderParameterRecipe()
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

        public List<string> GetUnloaderPositionList()
        {
            List<string> ret = new List<string>();
            foreach (ZzxzxyPositionData position in m_UnloaderPosition)
            {
                if (position.Type == TargetType.Base)
                    ret.Add(position.Name);
            }
            return ret;
        }

        public int SetData(int nStartIndex, SettingParameterCollection parameters)
        {
            int i = nStartIndex;
            foreach (ZzxzxyPositionData pos in m_UnloaderPosition)
            {
                if (i >= parameters.Count)
                    return i;
                pos.Z0 = parameters[i++].DoubleValue;

                if (i >= parameters.Count)
                    return i;
                pos.Z1 = parameters[i++].DoubleValue;

                if (i >= parameters.Count)
                    return i;
                pos.TR_X = parameters[i++].DoubleValue;

                if (i >= parameters.Count)
                    return i;
                pos.TR_Z = parameters[i++].DoubleValue;
            }

            return i;
        }

        public SettingParameterCollection GetData()
        {
            SettingParameterCollection parameters = new SettingParameterCollection();

            foreach (ZzxzxyPositionData pos in m_UnloaderPosition)
            {
                {
                    SettingParameter param = new SettingParameter(pos.Name, DataType.Double);
                    param.DoubleValue = pos.Z0;
                    param.Tag = LoaderParameter.MotionKey.Z0.ToString();
                    param.Spare = pos.Type.ToString();
                    parameters.Add(param);
                }

                {
                    SettingParameter param = new SettingParameter(pos.Name, DataType.Double);
                    param.DoubleValue = pos.Z1;
                    param.Tag = LoaderParameter.MotionKey.Z1.ToString();
                    param.Spare = pos.Type.ToString();
                    parameters.Add(param);
                }

                {
                    SettingParameter param = new SettingParameter(pos.Name, DataType.Double);
                    param.DoubleValue = pos.TR_X;
                    param.Tag = LoaderParameter.MotionKey.TR_X.ToString();
                    param.Spare = pos.Type.ToString();
                    parameters.Add(param);
                }

                {
                    SettingParameter param = new SettingParameter(pos.Name, DataType.Double);
                    param.DoubleValue = pos.TR_Z;
                    param.Tag = LoaderParameter.MotionKey.TR_Z.ToString();
                    param.Spare = pos.Type.ToString();
                    parameters.Add(param);
                }
            }

            return parameters;
        }
    }
}
