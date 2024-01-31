using QMC.Common.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Parts
{
    public class PositionResetter : Part
    {
        public PositionResetter(string strName) : base(strName)
        {

        }
        public override int Create()
        {
            int ret = base.Create();

           

            return ret;
        }

        public override void Close()
        {
            base.Close();
        }

        public override void UpdateConfigData() //참고 : Override
        {
            DieLoader dieLoader = Owner as DieLoader;
            
        }
    }
}
