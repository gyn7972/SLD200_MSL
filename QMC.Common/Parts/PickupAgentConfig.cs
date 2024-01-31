using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Parts
{
    [Serializable]
    public class PickupAgentConfig
    {
        [Serializable]
        public enum NeedleAfterFinish
        {
            GoBack,
            GoHome
        }
        private NeedleAfterFinish m_eNeedleAfterFinish;

        [Browsable(false)]
        public List<ActionItem> ActionItems { get; set; }

        [Browsable(false)]
        public NeedleAfterFinish AfterFinsh
        {
            get { return m_eNeedleAfterFinish; }
            set { m_eNeedleAfterFinish = value; }
        }

        public PickupAgentConfig()
        {
            ActionItems = new List<ActionItem>();
            m_eNeedleAfterFinish = NeedleAfterFinish.GoBack;
        }

    }
    [Serializable]
    public class ActionItem
    {
        #region Define
        public enum AXIS
        {
            Collet,
            Needle,
            VacuumOn,
            VacuumOff,
        }
        public enum ACTION_TYPE
        {
            WaitBeforeAction,
            WithBeforeAction,
            OverrideMotion,
            OverridePosition,
            OverrideVelocity
        }


        #endregion

        #region Field
        private double m_dDistance;
        private double m_dAcc;
        private double m_dDec;
        private double m_dVel;
        private double m_dActionForRemainDistance;
        private double m_dActionForMoveDistance;
        private double m_dTolerance;
        private double m_dBeforeDelay;
        private AXIS m_eAxis;
        private ACTION_TYPE m_eType;
        private bool m_MoveColletPickupPosition;
        private bool m_MoveNeedleGobackPosition;
        private bool m_MoveColletHomePosition;
        #endregion

        public ActionItem()
        {
            this.m_dDistance = 0;
            this.m_dAcc = 0;
            this.m_dDec = 0;
            this.m_dVel = 0;
            this.m_dActionForRemainDistance = 0;
            this.m_dBeforeDelay = 0;
            this.m_eAxis = AXIS.Collet;
            this.m_eType = ACTION_TYPE.WaitBeforeAction;
            this.m_MoveColletPickupPosition = false;
        }

        #region Peoperty

        [Category("Action")]
        public double Distance
        {
            get { return m_dDistance; }
            set { m_dDistance = value; }
        }

        [Category("Action")]
        public double Acc
        {
            get { return m_dAcc; }
            set { m_dAcc = value; }
        }

        [Category("Action")]
        public double Dec
        {
            get { return m_dDec; }
            set { m_dDec = value; }
        }

        [Category("Action")]
        public double Velocity
        {
            get { return m_dVel; }
            set { m_dVel = value; }
        }

        [Category("Action")]
        public double ActionForMoveDistance
        {
            get { return m_dActionForMoveDistance; }
            set { m_dActionForMoveDistance = value; }
        }

        [Category("Action")]
        public double ActionForRemainDistance
        {
            get { return m_dActionForRemainDistance; }
            set { m_dActionForRemainDistance = value; }
        }

        [Category("Action")]
        public double Tolerance
        {
            get { return m_dTolerance; }
            set { m_dTolerance = value; }
        }

        [Category("Action")]
        public double BeforeDelay
        {
            get { return m_dBeforeDelay; }
            set { m_dBeforeDelay = value; }
        }

        [Category("Axis")]
        public AXIS Axis
        {
            get { return m_eAxis; }
            set { m_eAxis = value; }
        }

        [Category("Axis")]
        public ACTION_TYPE ActionType
        {
            get { return m_eType; }
            set { m_eType = value; }
        }

        [Category("Axis")]
        public bool MoveColletPickupPosition
        {
            get { return this.m_MoveColletPickupPosition; }
            set { this.m_MoveColletPickupPosition = value; }
        }

        [Category("Axis")]
        public bool MoveNeedleGobackPosition
        {
            get { return this.m_MoveNeedleGobackPosition; }
            set { this.m_MoveNeedleGobackPosition = value; }
        }

        [Category("Axis")]
        public bool MoveColletHomePosition
        {
            get { return this.m_MoveColletHomePosition; }
            set { this.m_MoveColletHomePosition = value; }
        }


        public int SetData(int i, SettingParameterCollection parameters)
        {
            int ret = 0;

            if (parameters.Count <= i) return ret = i;
            Axis = (AXIS)Enum.Parse(typeof(AXIS), parameters[i++].StringValue);
            if (parameters.Count <= i) return ret = i;
            Distance = parameters[i++].DoubleValue;
            if (parameters.Count <= i) return ret = i;
            Acc = parameters[i++].DoubleValue;
            if (parameters.Count <= i) return ret = i;
            Dec = parameters[i++].DoubleValue;
            if (parameters.Count <= i) return ret = i;
            Velocity = parameters[i++].DoubleValue;
            if (parameters.Count <= i) return ret = i;
            ActionForRemainDistance = parameters[i++].DoubleValue;
            if (parameters.Count <= i) return ret = i;
            ActionForMoveDistance = parameters[i++].DoubleValue;
            if (parameters.Count <= i) return ret = i;
            Tolerance = parameters[i++].DoubleValue;
            if (parameters.Count <= i) return ret = i;
            BeforeDelay = parameters[i++].DoubleValue;
            if (parameters.Count <= i) return ret = i;
            ActionType = (ACTION_TYPE)Enum.Parse(typeof(ACTION_TYPE), parameters[i++].StringValue);
            if (parameters.Count <= i) return ret = i;
            MoveColletPickupPosition = parameters[i++].BoolValue;
            if (parameters.Count <= i) return ret = i;
            MoveNeedleGobackPosition = parameters[i++].BoolValue;
            if (parameters.Count <= i) return ret = i;
            MoveColletHomePosition = parameters[i++].BoolValue;

            ret = i;
            return ret;
        }

        public SettingParameterCollection GetData()
        {
            SettingParameterCollection ret = new SettingParameterCollection();

            string strName = m_eAxis.ToString();
            {
                SettingParameter parameter = new SettingParameter();
                parameter.Name = strName;
                parameter.StringValue = strName;
                ret.Add(parameter);
            }
            {
                SettingParameter parameter = new SettingParameter();
                parameter.Name = strName;
                parameter.DoubleValue = m_dDistance;
                ret.Add(parameter);
            }
            {
                SettingParameter parameter = new SettingParameter();
                parameter.Name = strName;
                parameter.DoubleValue = m_dAcc;
                ret.Add(parameter);
            }
            {
                SettingParameter parameter = new SettingParameter();
                parameter.Name = strName;
                parameter.DoubleValue = m_dDec;
                ret.Add(parameter);
            }
            {
                SettingParameter parameter = new SettingParameter();
                parameter.Name = strName;
                parameter.DoubleValue = m_dVel;
                ret.Add(parameter);
            }
            {
                SettingParameter parameter = new SettingParameter();
                parameter.Name = strName;
                parameter.DoubleValue = m_dActionForRemainDistance;
                ret.Add(parameter);
            }
            {
                SettingParameter parameter = new SettingParameter();
                parameter.Name = strName;
                parameter.DoubleValue = m_dActionForMoveDistance;
                ret.Add(parameter);
            }
            {
                SettingParameter parameter = new SettingParameter();
                parameter.Name = strName;
                parameter.DoubleValue = m_dTolerance;
                ret.Add(parameter);
            }
            {
                SettingParameter parameter = new SettingParameter();
                parameter.Name = strName;
                parameter.DoubleValue = m_dBeforeDelay;
                ret.Add(parameter);
            }
            {
                SettingParameter parameter = new SettingParameter();
                parameter.Name = strName;
                parameter.StringValue = m_eType.ToString();
                ret.Add(parameter);
            }
            {
                SettingParameter parameter = new SettingParameter();
                parameter.Name = strName;
                parameter.BoolValue = m_MoveColletPickupPosition;
                ret.Add(parameter);
            }
            {
                SettingParameter parameter = new SettingParameter();
                parameter.Name = strName;
                parameter.BoolValue = m_MoveNeedleGobackPosition;
                ret.Add(parameter);
            }
            {
                SettingParameter parameter = new SettingParameter();
                parameter.Name = strName;
                parameter.BoolValue = m_MoveColletHomePosition;
                ret.Add(parameter);
            }



            return ret;
        }
        #endregion
    }
}

