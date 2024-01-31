using QMC.Common.Yamatake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QMC.Common.Parts;

namespace QMC.Common
{
    public class Collet : MCW400A100Gauge
    {
        #region Field
        public int m_nArmIndex;
        private QmcLowPassFilter m_LPFTargetX;
        private QmcLowPassFilter m_LPFTargetY;
        private QmcLowPassFilter m_LPFTargetT;


        private QmcLowPassFilter m_LPFSourceX;
        private QmcLowPassFilter m_LPFSourceY;
        #endregion

        #region Property
        public int ArmIndex
        {
            get { return this.m_nArmIndex; }
            set
            {
                this.m_nArmIndex = value;
            }
        }
        public QmcLowPassFilter LPFTargetX
        {
            get { return this.m_LPFTargetX; }
        }
        public QmcLowPassFilter LPFTargetY
        {
            get { return this.m_LPFTargetY; }

        }

        public QmcLowPassFilter LPFTargetT
        {
            get { return this.m_LPFTargetT; }

        }
        public QmcLowPassFilter LPFSourceX
        {
            get { return this.m_LPFSourceX; }
        }
        public QmcLowPassFilter LPFSourceY
        {
            get { return this.m_LPFSourceY; }

        }

        public Gripper Gripper { set; get; }
        #endregion

        public Collet(string strName) : base(strName)
        {
            m_LPFTargetX = new QmcLowPassFilter();
            m_LPFTargetY = new QmcLowPassFilter();
            m_LPFTargetT = new QmcLowPassFilter();
            m_LPFSourceX = new QmcLowPassFilter();
            m_LPFSourceY = new QmcLowPassFilter();
            
        }

        public override int Create()
        {
            int ret = 0;
            if ((ret = base.Create()) != 0) return ret;

            return ret;
        }
        public override void Close()
        {
            base.Close();
        }

        public int Hold()
        {
            return Gripper.Hold();
        }

        public int Release()
        {
            return Gripper.Release();
        }

        public bool IsHold()
        {
            return Gripper.IsHold();
        }

        public Task<int> BeginHold()
        {
            return Gripper.BeginHold();
        }

        public Task<int> BeginRelease()
        {
            return Gripper.BeginRelease();
        }
    }
}
