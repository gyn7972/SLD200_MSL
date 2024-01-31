using Matrox.MatroxImagingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Vision.Matrox.Tools
{
    public class MSystem
    {
        private static MSystem _instance;
        public static MSystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MSystem();
                }
                return _instance;
            }
        }
        private MIL_ID m_MilApplication;
        private MIL_ID m_MilSystem;
        public MIL_ID MilApplication    // Application identifier.
        { 
            get
            {
                return m_MilApplication;
            }
        }      
        public MIL_ID MilSystem         // System identifier.
        {
            get
            {
                return m_MilSystem;
            }
        }

        public MSystem()
        {
            m_MilApplication = MIL.M_NULL;
            m_MilSystem = MIL.M_NULL;
            Init();
        }

        private void Init()
        {
            MIL.MappAlloc(MIL.M_DEFAULT, ref m_MilApplication);
            MIL.MsysAlloc(MilApplication, MIL.M_SYSTEM_HOST, MIL.M_DEFAULT, MIL.M_DEFAULT, ref m_MilSystem);
        }

        public void Close()
        {
            if (MilSystem != MIL.M_NULL)
                MIL.MsysFree(MilSystem);
            if (MilApplication != MIL.M_NULL)
                MIL.MappFree(MilApplication);
        }
    }
}
