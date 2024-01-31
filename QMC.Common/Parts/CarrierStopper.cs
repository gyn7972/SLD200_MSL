using QMC.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QMC.Common.Parts
{
    public class CarrierStopper : Part
    {
        public enum DioPointKey
        {
            Input_Stopper_Up,
            Input_Stopper_Down,
            Output_Stopper_Up,
            Output_Stopper_Down,
        }
        public int ResponseTimeout { set; get; }
        public CarrierStopper(string strName) : base(strName)
        {
            ResponseTimeout = 0;
        }
        public override int Create()
        {
            int ret = base.Create();

            if (m_dicDioPoints == null)
                m_dicDioPoints = new Dictionary<string, DioPoint>();

            m_dicDioPoints.Clear();
            foreach (DioPointKey key in Enum.GetValues(typeof(DioPointKey)))
            {
                m_dicDioPoints.Add(key.ToString(), null);
            }

            return ret;
        }
        public override void Close()
        {
            base.Close();
        }

        public virtual bool IsUp()
        {
            bool bRet = false;
            DioValue on, off;
            DioPoint dioInputStopperUp = m_dicDioPoints[DioPointKey.Input_Stopper_Up.ToString()];
            DioPoint dioInputStopperDown = m_dicDioPoints[DioPointKey.Input_Stopper_Down.ToString()];
            if (dioInputStopperUp != null)
            {
                on = dioInputStopperUp.GetValue();
            }                
            else
            {
                on = DioValue.On;
            }                

            if (dioInputStopperDown != null)
            {
                off = dioInputStopperDown.GetValue();
            }                
            else
            {
                off = DioValue.Off;
            }

            if (on == DioValue.On && off == DioValue.Off)
            {
                bRet = true;
            }


            return bRet;
        }

        public virtual bool IsDown()
        {
            bool bRet = false;
            DioValue on, off;
            DioPoint dioInputStopperUp = m_dicDioPoints[DioPointKey.Input_Stopper_Up.ToString()];
            DioPoint dioInputStopperDown = m_dicDioPoints[DioPointKey.Input_Stopper_Down.ToString()];
            if (dioInputStopperUp != null)
            {
                on = dioInputStopperUp.GetValue();
            }
            else
            {
                on = DioValue.Off;
            }

            if (dioInputStopperDown != null)
            {
                off = dioInputStopperDown.GetValue();
            }
            else
            {
                off = DioValue.On;
            }

            if (on == DioValue.Off && off == DioValue.On)
            {
                bRet = true;
            }


            return bRet;
        }
        public virtual int Up()
        {
            int ret = 0;
            DioPoint dioOutputStopperUp = m_dicDioPoints[DioPointKey.Output_Stopper_Up.ToString()];
            DioPoint dioOutputStopperDown = m_dicDioPoints[DioPointKey.Output_Stopper_Down.ToString()];
            if(dioOutputStopperUp != null && dioOutputStopperDown != null)
            {
                if ((ret = dioOutputStopperUp.Write(DioValue.On)) != 0) return ret;
                Thread.Sleep(10);
                if ((ret = dioOutputStopperDown.Write(DioValue.Off)) != 0) return ret;
            }
            else
            {
                ret = -1;
                return ret;
            }
            
            

            DateTime startTime = DateTime.Now;
            while (true)
            {
                if (IsUp())
                    break;
                if(ResponseTimeout > 0)
                {
                    TimeSpan processTime = DateTime.Now - startTime;
                    if (processTime.TotalMilliseconds >= ResponseTimeout)
                    {
                        //Timeout
                        ret = -1;
                        break;
                    }
                }
                Thread.Sleep(1);
            }

            return ret;
        }

        public virtual Task<int> BeginUP()
        {
            return Task.Factory.StartNew(() =>
            {
                return Up();
            });
        }

        public virtual int Down()
        {
            int ret = 0;

            DioPoint dioOutputStopperUp = m_dicDioPoints[DioPointKey.Output_Stopper_Up.ToString()];
            DioPoint dioOutputStopperDown = m_dicDioPoints[DioPointKey.Output_Stopper_Down.ToString()];

            if (dioOutputStopperUp != null && dioOutputStopperDown != null)
            {
                if ((ret = dioOutputStopperUp.Write(DioValue.Off)) != 0) return ret;
                if ((ret = dioOutputStopperDown.Write(DioValue.On)) != 0) return ret;
            }
            else
            {
                ret = -1;
                return ret;
            }

            DateTime startTime = DateTime.Now;
            while (true)
            {
                if (IsDown())
                    break;
                if (ResponseTimeout > 0)
                {
                    TimeSpan processTime = DateTime.Now - startTime;
                    if (processTime.TotalMilliseconds >= ResponseTimeout)
                    {
                        //Timeout
                        ret = -1;
                        break;
                    }
                }
                Thread.Sleep(1);
            }
            return ret;
        }

        public virtual Task<int> BeginDown()
        {
            return Task.Factory.StartNew(() =>
            {
                return Down();
            });
        }

    }
}
