using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QMC.Common.Parts
{
    public class Conveyor : Part
    {
        public enum DioPointKey
        {
            Output_Conveyor_Run,
            Output_Conveyor_Direction,
            Input_Detect_Tray,
            Input_Smema_Ready,
            Output_Smema_Ready,
        }

        public enum Direction
        {
            Forword,
            Backword,
        }

        protected Task<int> m_AutoTask;

        public CarrierStopper Stopper { get; set; }
        public ConveyorConfig Config { set; get; }

        public bool IsFinishedNextStep { set; get; }
        public CarrierClamper Clamper { get; set; }

        public bool ReadyInput { set; get; }
        public bool ReadyOutput { set; get; }
        public bool ReadyBeforeStep { set; get; }
        public bool ReadyNextStep { set; get; }
        public Carrier Carrier { get; set; }
        public Conveyor(string strName) : base(strName)
        {
            Config = new ConveyorConfig();
            ReadyInput = false;
            ReadyOutput = false;
            ReadyBeforeStep = false;
            ReadyNextStep = false;
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

        public override int Initialize()
        {
            int ret = 0;

            Stop();

            return base.Initialize();
        }

        public override void Close()
        {
            base.Close();
        }

        public override void UpdateConfigData()
        {
            base.UpdateConfigData();
            if(Config.EnableStopper)
            {
                if(Stopper != null)
                    Stopper.ResponseTimeout = Config.ResponseTimeout;
            }
            else
            {
                Stopper = null;
            }

            if (Config.EnableClamper)
            {
                if (Clamper != null)
                    Clamper.ResponseTimeout = Config.ClamperResponseTimeout;
            }
            else
            {
                Clamper = null;
            }
        }

        public override void UpdateRecipeData()
        {
            base.UpdateRecipeData();
        }

        public void AutoWork()
        {
            if(m_AutoTask == null || m_AutoTask.Status == TaskStatus.RanToCompletion)
            {
                m_AutoTask = BeginWork();
            }
        }
        
        public override int OnWork()
        {
            int ret = 0;

            DioPoint dioDetect = m_dicDioPoints[DioPointKey.Input_Detect_Tray.ToString()];
            if(dioDetect == null)
            {
                return -1;
            }

            try
            {
                if (dioDetect.GetValue() == DioValue.On)
                {
                    ReadyInput = false;
                    ReadyOutput = true;
                    while(true)
                    {
                        if (ReadyNextStep)
                            break;
                        Thread.Sleep(1);
                    }
                    if (Clamper != null)
                    {
                        Clamper.Unclamp();
                    }
                    if ((ret = MoveToOut()) != 0) return ret;
                    ReadyOutput = false;
                }
                else
                {
                    ReadyInput = true;
                    ReadyOutput = false;
                    while (true)
                    {
                        if (ReadyBeforeStep)
                            break;
                        Thread.Sleep(1);
                    }
                    if ((ret = MoveToZone()) != 0) return ret;
                    if(Clamper != null)
                    {
                        Clamper.Clamp();
                    }
                    ReadyInput = false;
                    Stop();
                }
            }
            catch(Exception ex)
            {
                LastError = ex.Message;
                return -1;
            }
            finally
            {
                
            }
            
            return ret;
        }

        public int MoveToZone()
        {
            int ret = 0;

            DioPoint dioDetect = m_dicDioPoints[DioPointKey.Input_Detect_Tray.ToString()];

            if (Stopper != null)
            {
                Stopper.Up();
            }

            if ((ret = Run()) != 0) return ret;

            if (dioDetect != null)
            {
                bool bTimeout = false;
                DateTime StartTime = DateTime.Now;
                TimeSpan ProcessTime;
                while (true)
                {
                    if (dioDetect.GetValue() == DioValue.On)
                    {
                        break;
                    }

                    if (Config.DetectCarrierTimeOut > 0)
                    {
                        ProcessTime = DateTime.Now - StartTime;
                        if (ProcessTime.TotalMilliseconds >= Config.DetectCarrierTimeOut)
                        {
                            bTimeout = true;
                            break;
                        }
                    }

                    Thread.Sleep(1);
                }

                if (bTimeout)
                {
                    this.SmemaOff();
                    this.Stop();

                    Alarm alarm = new Alarm();
                    alarm.Title = "Conveyor Timeout";
                    alarm.Code = -100;
                    alarm.Grade = "Stop";
                    alarm.Source = this.Name;
                    alarm.Cause = "Conveyor의 Timeout이 발생했습니다. Conveyor를 확인해주세요.";

                    AlarmManager.Instance.ShowAlarm(alarm);
                    return -100;
                }

                if (Config.DelayDetectAfterStop > 0)
                {
                    StartTime = DateTime.Now;
                    while (true)
                    {
                        ProcessTime = DateTime.Now - StartTime;
                        if (ProcessTime.TotalMilliseconds >= Config.DelayDetectAfterStop)
                        {
                            bTimeout = true;
                            break;
                        }
                        Thread.Sleep(1);
                    }
                }

            }

            return ret;
        }

        public bool IsDirection()
        {
            bool bRet = false;
            DioPoint dioDirection = m_dicDioPoints[DioPointKey.Output_Conveyor_Direction.ToString()];
            if(dioDirection != null)
            {
                if(dioDirection.GetValue() == Config.PositiveDirection)
                    bRet = true;
            }
            return bRet;
        }

        public int MoveToOut()
        {
            int ret = 0;

            if (Stopper != null)
            {
                Stopper.Down();
            }

            if ((ret = Run()) != 0) return ret;

            return ret;
        }

        public virtual int Run()
        {
            int ret = 0;
            if((ret = SetDirection(Direction.Forword)) != 0) return ret;
            DioPoint dioRun = m_dicDioPoints[DioPointKey.Output_Conveyor_Run.ToString()];
            
            if (dioRun == null)
                return ret = -1;

            if ((ret = dioRun.Write(DioValue.On)) != 0) return ret;

            return ret;
        }

        public Task<int> BeginRun()
        {
            return Task.Factory.StartNew(() =>
            {
                return Run();
            });
        }

        public override void Stop()
        {
            base.Stop();
            
            DioPoint dioRun = m_dicDioPoints[DioPointKey.Output_Conveyor_Run.ToString()];

            if (dioRun == null)
                return;

            dioRun.Write(DioValue.Off);
        }

        public Task<int> BeginStop()
        {
            return Task.Factory.StartNew(() =>
            {
                int ret = 0;
                Stop();
                return ret;
            });
        }

        public int SetDirection(Direction direction)
        {
            int ret = 0;

            DioPoint dioDirection = m_dicDioPoints[DioPointKey.Output_Conveyor_Direction.ToString()];

            if (dioDirection == null)
                return ret = -1;

            if(direction == Direction.Forword)
            {
                if ((ret = dioDirection.Write(Config.PositiveDirection)) != 0) return ret;
            }
            else
            {
                DioValue value = Config.PositiveDirection == DioValue.On ? DioValue.Off : DioValue.On;
                if ((ret = dioDirection.Write(value)) != 0) return ret;
            }
            

            return ret;

            
        }
        
        public bool IsInputCarrier()
        {
            bool bRet = false;

            //TO DO : Simulate용 삭제 필요.
            if (m_dicDioPoints.Count == 0) return bRet;

            DioPoint dioDetect = m_dicDioPoints[DioPointKey.Input_Detect_Tray.ToString()];
            if (dioDetect != null)
            {
                if(dioDetect.GetValue() == DioValue.On)
                {
                    bRet = true;
                }
            }

            return bRet;
        }

        public virtual bool IsRun()
        {
            bool ret = false;

            //TO DO : Simulate용 삭제 필요.
            if (m_dicDioPoints.Count == 0) return ret;

            DioPoint dioRun = m_dicDioPoints[DioPointKey.Output_Conveyor_Run.ToString()];


            if (dioRun == null)
                return ret;

            DioValue on = dioRun.GetValue();
            if (on == DioValue.On)
            {
                return true;
            }

            return ret;
        }

        public int SmemaOff()
        {
            int ret = 0;
            DioPoint dioSmema = m_dicDioPoints[DioPointKey.Output_Smema_Ready.ToString()];
            if(dioSmema != null)
            {
                if ((ret = dioSmema.Write(DioValue.Off)) != 0)
                {
                    return ret;
                }
            }
            
            return ret;
        }

        public int SmemaOn()
        {
            int ret = 0;
            DioPoint dioSmema = m_dicDioPoints[DioPointKey.Output_Smema_Ready.ToString()];
            if(dioSmema != null)
            {
                if ((ret = dioSmema.Write(DioValue.On)) != 0)
                {
                    return ret;
                }
            }
            
            return ret;
        }

        public virtual bool IsSmemaReady()
        {
            bool ret = false;

            if (m_dicDioPoints.Count == 0) return ret;

            DioPoint dioSmemaReady = m_dicDioPoints[DioPointKey.Input_Smema_Ready.ToString()];

            if (dioSmemaReady == null)
                return ret;

            DioValue on = dioSmemaReady.GetValue();
            if (on == DioValue.On)
            {
                return true;
            }
            return ret;
        }

    }
}
