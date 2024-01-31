using QMC.Common.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Parts
{
    public class PickupAgent : Part
    {
        public PickupAgentConfig Config { get; set; }
        public PickupAgent(string strName) : base(strName)
        {
            Config = new PickupAgentConfig();
        }

        #region Module
        public override int Create()
        {
            int ret = base.Create();

            return ret;
        }

        public override void Close()
        {
            base.Close();
        }

        public override void UpdateConfigData()
        {
            if(Owner is DieLoader)
            {
                DieLoader dieLoader = (DieLoader)Owner;
                this.Config = dieLoader.Config.PickUpAgentConfig;
            }
        }
        #endregion
    }
}
