using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    [Serializable]
    public class Part : IExecuter
    {
        public enum RunMode
        {
            Auto,
            Manual,
            DryRun,
            Bypass,
        }

        public enum RunStatus
        {
            Run,
            Stop,
            CycleStop,
        }

        protected Dictionary<string, MotionAxis> m_dicAxes;
        protected Dictionary<string, DioPoint> m_dicDioPoints;
        protected Dictionary<string, DisplayAxisType> m_dicAxisDisplayType;
        protected Dictionary<int, Alarm> m_dicAlarms;
        protected RunStatus m_Status;
        public string Name { get; set; }

        public string LastError { set; get; }

        [Browsable(false)]
        public Dictionary<string, MotionAxis> Axes
        {
            set
            {
                m_dicAxes = value;
            }
            get
            {
                return m_dicAxes;
            }
        }

        [Browsable(false)]
        public Dictionary<string, DioPoint> DioPoints
        {
            set
            {
                m_dicDioPoints = value;
            }
            get
            {
                return m_dicDioPoints;
            }
        }

        [Browsable(false)]
        public Module Owner { set; get; }

        public Part(string strName)
        {
            Name = strName;
            Axes = new Dictionary<string, MotionAxis>();
            DioPoints = new Dictionary<string, DioPoint>();
            m_dicAxisDisplayType = new Dictionary<string, DisplayAxisType>();
            if (m_dicAlarms == null)
                m_dicAlarms = new Dictionary<int, Alarm>();
            InitAlarm();
        }
        protected virtual void InitAlarm()
        {
            Alarm alarm = new Alarm();
            alarm.Code = -999;
            alarm.Title = "Unknown Error";
            alarm.Cause = "";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);
        }

        protected Alarm GetAlarm(int nCode)
        {
            Alarm alarm = null;
            if (m_dicAlarms.ContainsKey(nCode))
            {
                alarm = m_dicAlarms[nCode];
            }
            else
            {
                alarm = m_dicAlarms[999];
            }

            return alarm;
        }
        #region IAutoRun
        public virtual int Initialize()
        {
            if (m_Status == RunStatus.Stop)
                return 1;

            return 0;
        }

        public virtual int OnPrepareToWork()
        {
            int ret = 0;
            SetRunStatus(RunStatus.Run);
            return ret;
        }

        public int Work()
        {
            int ret = 0;
            if ((ret = OnPrepareToWork()) != 0) return ret;
            if ((ret = OnWork()) != 0) return ret;
            if ((ret = OnAfterWork()) != 0) return ret;
            return ret;
        }

        public Task<int> BeginWork()
        {
            return Task.Factory.StartNew(() =>
            {
                return Work();
            });
        }

        public virtual int OnWork()
        {
            int ret = 0;

            return ret;
        }

        public virtual int OnAfterWork()
        {
            int ret = 0;

            return ret;
        }

        public virtual void Stop()
        {
            m_Status = RunStatus.Stop;
        }
        #endregion

        public virtual int Create()
        {
            return 0;
        }
        public virtual void Close()
        {

        }

        public void SetMotion(string key, MotionAxis axis)
        {
            if (m_dicAxes.ContainsKey(key))
            {
                m_dicAxes[key] = axis;

                if (m_dicAxes[key] != null)
                {
                    m_dicAxes[key].OnMoveInterpolation += OnMoveInterpolation;
                    m_dicAxes[key].OnStopJogVelocity += OnStopJogVelocity;
                    m_dicAxes[key].OnGetAcutualInterpolationPosition += OnGetAcutualInterpolationPosition;
                    m_dicAxes[key].OnGetCommandInterpolationPosition += OnGetCommandInterpolationPosition;
                }
            }
        }

        protected virtual int OnGetCommandInterpolationPosition(MotionAxis axis, ref double dPosition)
        {
            int ret = 0;

            return ret;
        }

        protected virtual int OnGetAcutualInterpolationPosition(MotionAxis axis, ref double dPosition)
        {
            int ret = 0;

            return ret;
        }

        protected virtual int OnStopJogVelocity(MotionAxis axis, int nVelPercent)
        {
            int ret = 0;

            return ret;
        }

        protected virtual int OnMoveInterpolation(MotionAxis axis, double dPosition, int nVelPercent)
        {
            int ret = 0;

            return ret;
        }

        public MotionAxis GetMotion(string key)
        {
            MotionAxis axis = null;
            if (m_dicAxes.ContainsKey(key))
                axis = m_dicAxes[key];

            return axis;
        }

        public void SetDioPoint(string key, DioPoint dio)
        {
            if (m_dicDioPoints.ContainsKey(key))
                m_dicDioPoints[key] = dio;
        }

        public DioPoint GetDioPoint(string key)
        {
            DioPoint point = null;
            if (m_dicDioPoints.ContainsKey(key))
                point = m_dicDioPoints[key];

            return point;
        }

        public void ClearMotion()
        {
            for (int i = 0; i < m_dicAxes.Keys.Count; i++)
            {
                string key = m_dicAxes.Keys.ElementAt(i);
                m_dicAxes[key] = null;
            }
        }

        public void ClearDioPoint()
        {
            //foreach (string key in m_dicDioPoints.Keys)
            //{
            //    m_dicDioPoints [key] = null;
            //}
            for (int i = 0; i < m_dicDioPoints.Keys.Count; i++)
            {
                string key = m_dicDioPoints.Keys.ElementAt(i);
                m_dicDioPoints[key] = null;
            }
        }
        public virtual List<MotionAxis> GetAxisList()
        {
            List<MotionAxis> listAxis = new List<MotionAxis>();
            foreach (string strKey in m_dicAxes.Keys)
            {
                MotionAxis axis = m_dicAxes[strKey];
                if (axis != null)
                {
                    axis.Configuration.DisplayAxisType = m_dicAxisDisplayType[strKey];
                    listAxis.Add(axis);
                }
            }

            return listAxis;
        }

        public List<DioPoint> GetDioPointList()
        {
            List<DioPoint> listPoints = new List<DioPoint>();
            foreach (DioPoint dioPoint in m_dicDioPoints.Values)
            {
                if (dioPoint != null)
                    listPoints.Add(dioPoint);
            }

            return listPoints;
        }

        public virtual void UpdateConfigData()
        {

        }

        public virtual void UpdateRecipeData()
        {

        }

        public virtual void SetRunStatus(RunStatus status)
        {
            m_Status = status;
        }
    }
}
