using ACS.SPiiPlusNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Motion.ACS.Motion
{
    public class ACS_Test
    {
        private Api m_Api;
        private bool m_bConnected = false;
        private int m_nTotalAxis = 0;
        private Axis[] m_arrAxisList = null;
        private int m_nTotalBuffer = 0;
        private MotorStates m_nMotorState;
        private double m_lfRPos, m_lfFPos, m_lfPE, m_lfFVEL;
        private ProgramStates m_nProgramState;
        private object m_objReadVar = null;
        private Array m_arrReadVector = null;
        private const int MAX_AXIS_COUNT = 32;
        private const int MAX_BUFFER_CNT = 64;

        private const int MAX_UI_LIMIT_CNT = 8;
        private const int MAX_UI_IO_CNT = 8;
        private int m_nValues, m_nOutputState;
        private int m_nAxisCount;
        public ACS_Test()
        {
            m_Api = new Api();
            m_nAxisCount = 2;
        }

        public int X_Move(double dTargetPos)
        {
            int ret = 0;
            double lfTargetPos = 0.0f;
            try
            {
                m_Api.ToPoint(
                    0,                          // '0' - Absolute position
                    (Axis)1,  // Axis number
                    dTargetPos                 // Target position
                    );

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return ret;
        }

        public int Y_Move(double dTargetPos)
        {
            int ret = 0;
            double lfTargetPos = 0.0f;
            try
            {
                m_Api.ToPoint(
                    0,                          // '0' - Absolute position
                    (Axis)1,  // Axis number
                    dTargetPos                 // Target position
                    );

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return ret;
        }


    }
}
