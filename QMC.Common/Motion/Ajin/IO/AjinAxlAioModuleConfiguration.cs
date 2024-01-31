using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QMC.Common;

namespace QMC.Common.Motion.Ajin.IO
{
    [Serializable]
    public class AjinAxlAioModuleConfiguration : AioModuleConfiguration
    {
        #region Field
        private int m_StartChannel;
        #endregion

        #region Constructor
        public AjinAxlAioModuleConfiguration()
        {
        }
        #endregion

        #region Property
        /// <summary>
        /// 모듈의 시작 채널 값을 가져오거나 설정한다.
        /// </summary>
        [Category("AjinAxlAioModule")]
        [DefaultValue(0)]
        public int StartChannel
        {
            get { return this.m_StartChannel; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("StartChannel");
                this.m_StartChannel = value;
            }
        }
        #endregion

        #region Configuration Members
        protected override void SetDefaultValues()
        {
            base.SetDefaultValues();

            this.StartChannel = 0;
        }
        #endregion
    }
}
