/*
 * Purpose
 * 
 * Revision
 * 
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.IO;
using QMC.Common;


namespace QMC.Common.Motion.Ajin.IO
{
    [Serializable]
    public class AjinAxlIoBoard : IOBoard
    {
        #region Field
        #endregion

        #region Constructor
        public AjinAxlIoBoard() : base()
        {
            this.Configuration = new AjinIoAxlBoardConfiguration();
        }
        #endregion

        #region Method
        #endregion

        #region IoBoard Members
        public new AjinIoAxlBoardConfiguration Configuration
        {
            get { return base.Configuration as AjinIoAxlBoardConfiguration; }
            set { base.Configuration = value; }
        }

        protected override int OnClose()
        {
            int ret = 0;

            return ret;
        }

        protected override int OnOpen()
        {
            int ret = 0;

            //if ((ret = AXL.Open()) != 0) return ret;

            if(ret == 0) IsOpen = true;
            return ret;
        }

        public override int Load(FileStream fs)
        {
            int ret = 0;
            AjinIoAxlBoardConfiguration configuration = new AjinIoAxlBoardConfiguration();
            if ((ret = SaveManager.BinaryDeserialize<AjinIoAxlBoardConfiguration>(fs, out configuration)) != 0) return ret;
            this.Configuration = configuration;
            int nModuleCount = this.Configuration.ModuleCount;
            
            for (int i = 0; i < nModuleCount; i++)
            {
                AjinAxlDioModule module = new AjinAxlDioModule();
                module.Load(fs);                
                module.Board = this;
                this.Modules.Add(module);
            }
 
            return ret;
        }
        #endregion


    }

    [Serializable]
    public class AjinIoAxlBoardConfiguration : IOBoadConfiguration
    {
        #region Field
        #endregion

        #region Constructor
        public AjinIoAxlBoardConfiguration()
            : base()
        {
        }
        #endregion

        #region Property
        #endregion
    }
}
