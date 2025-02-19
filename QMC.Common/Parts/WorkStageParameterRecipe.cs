using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Parts
{
    public class WorkStageParameterRecipe
    {
        protected List<XyzyPositionData> m_WorkStagePosition;

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


        public WorkStageParameterRecipe()
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

        public List<string> GetWorkStagePositionList()
        {
            List<string> ret = new List<string>();
            foreach (XyzyPositionData position in m_WorkStagePosition)
            {
                if (position.Type == TargetType.Base)
                    ret.Add(position.Name);
            }
            return ret;
        }

        public int SetData(int nStartIndex, SettingParameterCollection parameters)
        {
            int i = nStartIndex;
            foreach (XyzyPositionData pos in m_WorkStagePosition)
            {
                if (i >= parameters.Count)
                    return i;
                pos.X = parameters[i++].DoubleValue;

                if (i >= parameters.Count)
                    return i;
                pos.Y = parameters[i++].DoubleValue;

                if (i >= parameters.Count)
                    return i;
                pos.Z = parameters[i++].DoubleValue;

                if (i >= parameters.Count)
                    return i;
                pos.MASK_Y = parameters[i++].DoubleValue;
            }

            return i;
        }

        public SettingParameterCollection GetData()
        {
            SettingParameterCollection parameters = new SettingParameterCollection();

            foreach (XyzyPositionData pos in m_WorkStagePosition)
            {
                {
                    SettingParameter param = new SettingParameter(pos.Name, DataType.Double);
                    param.DoubleValue = pos.X;
                    param.Tag = WorkStageParameter.MotionKey.X.ToString();
                    param.Spare = pos.Type.ToString();
                    parameters.Add(param);
                }

                {
                    SettingParameter param = new SettingParameter(pos.Name, DataType.Double);
                    param.DoubleValue = pos.Y;
                    param.Tag = WorkStageParameter.MotionKey.Y.ToString();
                    param.Spare = pos.Type.ToString();
                    parameters.Add(param);
                }

                {
                    SettingParameter param = new SettingParameter(pos.Name, DataType.Double);
                    param.DoubleValue = pos.Z;
                    param.Tag = WorkStageParameter.MotionKey.Z.ToString();
                    param.Spare = pos.Type.ToString();
                    parameters.Add(param);
                }

                {
                    SettingParameter param = new SettingParameter(pos.Name, DataType.Double);
                    param.DoubleValue = pos.MASK_Y;
                    param.Tag = WorkStageParameter.MotionKey.MASK_Y.ToString();
                    param.Spare = pos.Type.ToString();
                    parameters.Add(param);
                }
            }

            return parameters;
        }
    }
}
