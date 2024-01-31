using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QMC.Common
{
    public class AsyncResult
    {
        public object Result { set; get; }

        //private Thread m_Worker;
        private AsyncMethod m_Worker;
        
        public bool IsStop
        {
            get
            {
                return m_Worker.IsStop;
            }
        }

        public AsyncResult(AsyncMethod worker)
        {
            Result = null;
            m_Worker = worker;            
        }

        public void Stop()
        {
            m_Worker.Stop();
        }

        public void WaitDone()
        {
            m_Worker.Wait();
        }
        
    }

    public class AsyncResultCollection : Collection<AsyncResult>
    {
        public bool IsStop
        {
            get
            {
                bool bResult = true;

                foreach(AsyncResult result in this)
                {
                    bResult &= result.IsStop;
                }

                return bResult;
            }
        }
        public int Wait()
        {
            int ret = 0;
            //bool bResult = false;
            while (true)
            {
                if (IsStop)
                    break;

                Thread.Sleep(1);
            }

            return ret;
        }
    }
}
