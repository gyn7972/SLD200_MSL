using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Global
{
    public static class AutoRunTracker
    {
        private static readonly object m_lock = new object();
        private static DateTime? m_start;
        private static DateTime? m_end;

        public static void OnAutoStart()
        {
            lock (m_lock)
            {
                m_start = DateTime.Now;
                m_end = null; // 진행 중
            }
        }

        public static void OnAutoStop()
        {
            lock (m_lock)
            {
                m_end = DateTime.Now;
            }
        }

        public static DateTime GetStartOrNow()
        {
            lock (m_lock) { return m_start ?? DateTime.Now; }
        }

        public static DateTime GetEndOrNow()
        {
            lock (m_lock) { return m_end ?? DateTime.Now; }
        }
    }

}
