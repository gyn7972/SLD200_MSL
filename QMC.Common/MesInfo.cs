using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    [Serializable]
    public class MesInfo
    {
        public Int64 WPSeq { get; set; }
        public string Floor { get; set; }
        public string Line { get; set; }
        public string Proc { get; set; }
        public string EquipCD { get; set; }

        public MesInfo() 
        {
            WPSeq = 0;
            Floor = "";
            Line = "";
            Proc = "";
            EquipCD = "";
        }
    }
}
