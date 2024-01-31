/*
 * Purpose
 * 
 * Revision
 * 
 */
using QMC.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;


namespace QMC.Common.Motion.Ajin.IO
{    
    public class AjinAxlAioModule : AioModule
    {
        #region Field
        private AXT_MODULE m_ModuleType;
        #endregion

        #region Constructor
        public AjinAxlAioModule()
        {
            this.Configuration = new AjinAxlAioModuleConfiguration();
        }
        #endregion

        #region Property
        public AXT_MODULE ModuleType
        {
            get { return this.m_ModuleType; }
        }
        #endregion

        #region Method
        #endregion

        #region AioModule Members
        public new AjinAxlAioModuleConfiguration Configuration
        {
            get { return base.Configuration as AjinAxlAioModuleConfiguration; }
            set { base.Configuration = value; }
        }

        public override int Load(FileStream fs)
        {
            return 0;
        }

        protected override int OnClose()
        {
            int ret = 0;
            return ret;
        }

        protected override int OnOpen()
        {
            int ret = 0;
            AjinAxlIoBoard board = this.Board as AjinAxlIoBoard;
            int boardId, moduleId;

            if (board == null)
                throw new ArgumentNullException("AjinAioModule.Board", "Board type is not AjinIoBoard");

            // get module information
            boardId = moduleId = 0;
            this.m_ModuleType = AXT_MODULE.AXT_SIO_DI32;
            if ((ret = AXA.GetModuleInformation((int)this.Configuration.No, ref boardId, ref moduleId, ref this.m_ModuleType)) != 0) return ret;

            // set channel range
            foreach (AioPoint point in this.Points)
            {
                if ((ret = AXA.SetOutputRange(point.Configuration.Address, point.Configuration.AnalogMinimum, point.Configuration.AnalogMaximum)) != 0) return ret;
            }

            return ret;
        }

        protected override int OnRead(ref byte[] values)
        {
            int ret = 0;


            return ret;
        }

        protected override int OnReadOutput(ref byte[] values)
        {
            int ret = 0;


            return ret;
        }

        protected override int OnWrite(byte[] values)
        {
            int ret = 0;

            foreach (AioPoint point in this.Points)
            {
                if (point.Configuration.IoType != IoType.Output) continue;

                if ((ret = AXA.WriteOutput(point.Configuration.Address + this.Configuration.StartChannel, point.AnalogValue)) != 0) return ret;
            }

            return ret;
        }
        #endregion
    }

}
