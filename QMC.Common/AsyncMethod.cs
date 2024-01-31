using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QMC.Common
{
    public delegate int IntResultDelegate();
    public delegate void VoidResultDelegate();
    public class AsyncMethod
    {
        private Delegate m_RunProcedure;
        private object[] m_RunArguments;
        private Delegate m_StopProcedure;
        private object[] m_StopArguments;
        private AsyncResult m_Result;
        private Task<int> m_Task;
        private CancellationTokenSource m_tokenSourceCancel;
        

        //public Function Func { set; get; }

        public double Timeout { set; get; }

        public bool IsStop
        {
            get
            {
                bool bRet = false;
                if(m_Task.Status == TaskStatus.Faulted || m_Task.Status == TaskStatus.Canceled || m_Task.Status == TaskStatus.RanToCompletion)
                {
                    bRet = true;
                }
                return bRet;
            }

        }

        public AsyncMethod()
        {
            m_RunProcedure = null;
            m_RunArguments = null;
            m_StopProcedure = null;
            m_StopArguments = null;
            m_Result = null;
            //Func = null;
            Timeout = 0;
            m_Task = null;
            
        }

        public void SetRunProcedure(Delegate procedure, object[] arguments)
        {
            m_RunProcedure = procedure;
            m_RunArguments = arguments;
        }

        public void SetStopProcedure(Delegate procedure, object[] arguments)
        {
            m_StopProcedure = procedure;
            m_StopArguments = arguments;
        }

        public AsyncResult Run()
        {
            if(Timeout == 0)
            {
                m_tokenSourceCancel = new CancellationTokenSource();
            }
            else
            {
                m_tokenSourceCancel = new CancellationTokenSource(TimeSpan.FromMilliseconds(Timeout));
            }
            
            CancellationToken tokenCancel = m_tokenSourceCancel.Token;

            m_Result = new AsyncResult(this);
            m_Task = Task.Factory.StartNew<int>(() =>
            {
                tokenCancel.Register(() =>
                {
                    if(m_Result.Result == null)
                        m_Result.Result = -5;
                    if(m_StopProcedure != null)
                        m_StopProcedure.DynamicInvoke(m_StopArguments);
                });

                Execute();
                
                m_tokenSourceCancel.Dispose();
                return 0;
            });            

            return m_Result;
        }


        void Execute()
        {
            //if (m_RunProcedure == null && Func == null)
            //{
            //    m_Result.Result = -10;
            //    return;
            //}

            //if (m_RunProcedure != null && m_Result != null)
            //{
            //    object obj = m_RunProcedure.DynamicInvoke(m_RunArguments);
            //    if (m_Result.Result == null)
            //        m_Result.Result = obj;
            //}

            //if (Func != null)
            //{
            //    m_Result.Result = Func.Execute();
            //}
        }

        //void DoMonitoring()
        //{
        //    TimeSpan executeTime;
        //    while (true)
        //    {
        //        if (m_bStopMonitoring)
        //            break;

        //        executeTime = DateTime.Now - m_StartTime;
        //        if (executeTime.TotalMilliseconds >= Timeout)
        //        {
        //            //Timeout 발생.
        //        }
        //        Thread.Sleep(1);
        //    }
        //}
        public void Stop()
        {
            m_Result.Result = 1; //User Stop
            m_tokenSourceCancel.Cancel();
            
        }


        public void Wait()
        {
            m_Task.Wait();
        }
    }
}
