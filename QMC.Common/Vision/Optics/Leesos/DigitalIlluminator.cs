using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QMC.Common.Vision.Optics.Leesos
{
    [Serializable]
    public class DigitalIlluminator : Illuminator
    {
        public bool m_bIsOpen { get { return m_Communicator != null ? m_Communicator.IsOpen : false; } }

        public class VolumnData
        {
            public int Channel { set; get; }
            public int Volumn { set; get; }
            
            public VolumnData() : this(0, 0)
            {
            }

            public VolumnData(int nChannel, int nVolumn)
            {
                Channel = nChannel;
                Volumn = nVolumn;
            }
        }

        [NonSerialized]
        private DigitalIlluminatorCommunicator m_Communicator;
        private Task m_Task;
        private Queue<VolumnData> m_CommandQueue;
        private CancellationTokenSource m_Cts;
        private bool m_bStop;
        private object m_QueueLock;
        public DigitalIlluminatorConfig Config { get; set; }

        

        public DigitalIlluminator() : this("DigitalIlluminator")
        {
        }

        public DigitalIlluminator(string strName) : base(strName)
        {
            //m_Communicator = new DigitalIlluminatorCommunicator();
            Config = new DigitalIlluminatorConfig();
            m_CommandQueue = new Queue<VolumnData>();
            m_QueueLock = new object();
            m_bStop = false;
        }

        public override int Initialize()
        {
            int ret = 0;
            if ((ret = base.Initialize()) != 0) return ret;
            
            try
            {
                if (m_Communicator == null)
                    m_Communicator = new DigitalIlluminatorCommunicator();

                if (m_Communicator.IsOpen)
                {
                    m_Communicator.Close();
                }

                m_Communicator.PortName = Config.PortName;
                m_Communicator.BaudRate = Config.BaudRate;
                m_Communicator.DataBits = Config.DataBits;
                m_Communicator.Parity = Config.Parity;
                m_Communicator.StopBits = Config.StopBits;
                m_Communicator.Handshake = Config.Handshake;
                m_Communicator.ReplyTimeout = Config.TimeOut;

                m_Communicator.Open();

                if(m_Communicator.IsOpen)
                {
                    m_Cts = new CancellationTokenSource();
                    CancellationToken token = m_Cts.Token;
                    m_Task = Task.Factory.StartNew(() =>
                    {
                        while(true)
                        {
                            if (m_bStop)
                                break;

                            if(CommandQueueCount() > 0)
                            {
                                SendVolumn();
                            }

                            token.Register(() =>
                            {
                                m_bStop = true;
                            });
                            Thread.Sleep(100);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
                ret = -1;
            }


            return ret;
        }

        public override void Close()
        {
            if (m_Communicator == null)
                return;

            base.Close();
            m_bStop = true;
            m_Communicator.Close();
        }

        private void Enqueue(VolumnData data)
        {
            lock(m_QueueLock)
            {
                m_CommandQueue.Enqueue(data);
            }
        }

        private VolumnData Dequeue()
        {
            lock(m_QueueLock)
            {
                return m_CommandQueue.Dequeue();
            }
            
        }

        private int CommandQueueCount()
        {
            lock (m_QueueLock)
            {
                return m_CommandQueue.Count;
            }
        }

        private void SendVolumn()
        {
            Dictionary<int, VolumnData> SendDatas = new Dictionary<int, VolumnData>();
            lock(m_QueueLock)
            {
                foreach(VolumnData data in m_CommandQueue)
                {
                    if (SendDatas.ContainsKey(data.Channel))
                    {
                        SendDatas[data.Channel] = data;
                    }
                    else
                    {
                        SendDatas.Add(data.Channel, data);
                    }
                }

                m_CommandQueue.Clear();
            }

            foreach(VolumnData data in SendDatas.Values)
            {
                SendVolumn(data);
            }
        }

        private void SendVolumn(VolumnData data)
        {
            if (m_Communicator == null)
                return;

            if (m_Communicator.IsOpen)
            {
                if(m_Communicator.SetVolume(data.Volumn, data.Channel) != 0)
                {
                    //Error 표시
                    
                }
            }
        }
        public override int CheckPowerOn(int channel)
        {
            int ret = -1;

            if (m_Communicator == null)
                return ret;

            if (m_Communicator.IsOpen)
            {
                ret = m_Communicator.CheckPowerOn(channel);
            }
            return ret;
        }

        public override int SetVolume(int volume, int channel)
        {
            int ret = 0;
            Enqueue(new VolumnData(channel, volume));
            return ret;
        }

        public override int TurnOnOff(bool bOnOff, int channel)
        {
            DigitalIlluminatorCommunicator.Commands commands;
            if (bOnOff)
                commands = DigitalIlluminatorCommunicator.Commands.ON;
            else
                commands = DigitalIlluminatorCommunicator.Commands.OFF;

            return TurnOnOff(commands, channel);
        }
        public int TurnOnOff(DigitalIlluminatorCommunicator.Commands onoff, int channel)
        {
            int ret = -1;

            if (m_Communicator == null)
                return ret;

            if (m_Communicator.IsOpen)
            {
                ret = m_Communicator.TurnOnOff(onoff, channel);
            }
            return ret;
        }
    }
}
