using QMC.Common.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Parts
{
    [Serializable]
    public class UnloadZ : LoadZ
    {
        public UnloadZConfig UnLoadZConfig { get; set; }
        public UnloadZ(string strName) : base(strName)
        {
            PitchCount = 3;
            UnLoadZConfig = new UnloadZConfig();
        }
        public override void UpdateConfigData() //참고 : Override
        {
            DieTransfer dieTransfer = Owner as DieTransfer;
            if (Owner != null) // 참고 : Loader인지 Unloader인지 parsing
            {
                UnLoadZConfig = dieTransfer.Config.UnloadZConfig;
                
                if (UnLoadZConfig.m_listCalibrationpitch == null)
                    UnLoadZConfig.m_listCalibrationpitch = new List<CalibrationPitch>();

                if (UnLoadZConfig.m_ListOffsetZ != null)
                    UnLoadZConfig.m_ListOffsetZ = new List<ColletZOffset>();
            }
        }
    }
}
