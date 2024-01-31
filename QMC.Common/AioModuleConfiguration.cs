using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    public class AioModuleConfiguration : IoModuleConfiguration
    {
        public AioModuleConfiguration() : base()
        {
            
        }

        public uint ItemByte { set; get; }

        protected override void SetDefaultValues()
        {
            base.SetDefaultValues();

            this.ItemByte = 2; //Default 2Byte
        }
    }
}
