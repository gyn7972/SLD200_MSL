using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    public class LoadingQueue
    {
        private Queue<Unit> m_queue;
        private object m_queuelock;
        public LoadingQueue()
        {
            m_queue = new Queue<Unit>();
            m_queuelock = new object();
        }

        public void Enqueue(Unit unit)
        {
            lock(m_queuelock)
            {
                m_queue.Enqueue(unit);
            }
            
        }

        public Unit DeQueue()
        {
            lock (m_queuelock)
            {
                if(m_queue.Count > 0)
                    return m_queue.Dequeue();
                else
                    return null;
            }            
        }

        public Unit Front()
        {
            lock( m_queuelock)
            {
                return m_queue.FirstOrDefault();
            }            
        }

        public void Clear()
        {
            lock(m_queuelock)
            {
                m_queue.Clear();
            }
        }

        public int Count()
        {
            int count = 0;
            lock(m_queuelock)
            {
                count = m_queue.Count;
            }
            return count;
        }

        public List<Unit> ToList()
        {
            List<Unit> list = null;
            lock (m_queuelock)
            {
                list = m_queue.ToList();
            }

            return list;
        }
    }
}
