using SIOImport;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Core.Laser
{
    public class Serialsio: ISerial
    {
        /// <summary>
        /// 
        /// </summary>
        public uint Index { get; }

        /// <summary>
        /// 이름
        /// </summary>
        public string Name { get; }

        public bool IsReady
        {
            get { return this.IsOpen && !this.IsBusy; }
        }
        public bool IsBusy { get; set; }
        public bool IsError { get; set; }

        private bool IsOpen { get; set; }
        private bool IsWaitReceive { get; set; }

        private int Port;

        Queue<char> bufferQueue = new Queue<char>();

        Queue<StringBuilder> receiveBufferQueue = new Queue<StringBuilder>();

        //readonly char STX = (char)0x02;
        readonly char ETX = (char)0x0A;

        bool disposed = false;



        /// <summary>
        /// 생성자
        /// </summary>
        public Serialsio()
        {

        }

        /// <summary>
        /// 소멸자
        /// </summary>
        ~Serialsio()
        {
            if (this.disposed)
                return;
            this.Dispose(false);
        }
        /// <summary>
        /// IDisposable 인터페이스 구현
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// IDisposable 인터페이스 구현
        /// </summary>
        /// <param name="disposing"></param>
        protected void Dispose(bool disposing)
        {
            if (this.disposed)
                return;
            if (disposing)
            {
                this.IsOpen = false;
                this.IsWaitReceive = false;
                SIOWrap.sio_close(this.Port);
            }
            this.disposed = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <param name="baudrate"></param>
        /// <param name="parity"></param>
        /// <param name="databit"></param>
        /// <param name="stopbit"></param>
        /// <returns></returns>
        public bool Initialize(int port, int baudrate)
        {
            if(SIOWrap.SIO_OK != SIOWrap.sio_open(port))
            {
                //Logger.Log(Logger.Module.Global, Logger.Type.Error, $"fail to open barcode communication port: {port}");
                return false;
            }
            this.Port = port;
            SIOWrap.sio_ioctl(port, baudrate, SIOWrap.P_NONE | SIOWrap.BIT_8 | SIOWrap.STOP_1);
            return true;
        }

        public bool Write(string data)
        {

            byte[] bytes = Encoding.ASCII.GetBytes(data);
            try
            {
                if (data != null)
                {
                    SIOWrap.sio_flush(this.Port, SIOWrap.FLUSH_INPUT_OUTPUT);
                    this.receiveBufferQueue.Clear();
                    this.bufferQueue.Clear();
                    int ret = SIOWrap.sio_write(this.Port, bytes, bytes.Length);
                    this.IsWaitReceive = true;
                    return true;
                }
                else return false;

            }
            catch
            {
                return false;
            }
        }

        public string Read()
        {
            if( WaitData(out string recv, 1000) )
                return recv;
            else
                return "";
        }

        private StringBuilder BuildBuffer()
        {
            StringBuilder str = new StringBuilder();
            char c;
            int len = bufferQueue.Count;
            for (int j = 0; j < len; j++)
            {
                c = bufferQueue.Dequeue();
                str.Append(c);
            }
            return str;
        }

        private bool WaitData(out string recvData, long timeout)
        {
            long start;
            long ticks;
            int len;
            char recvChar;
            recvData = string.Empty;

            if (timeout < 0) timeout = 500;

            var sw = Stopwatch.StartNew();

            while (this.IsWaitReceive)
            {
                if (sw.ElapsedMilliseconds > timeout) return false;

                try
                {
                    len = SIOWrap.sio_iqueue(this.Port);
                    for (int i = 0; i < len; i++)
                    {
                        recvChar = Convert.ToChar(SIOWrap.sio_getch(this.Port));
                        if (Environment.NewLine.Contains(recvChar)) 
                            goto DataReceive;
                        this.bufferQueue.Enqueue(recvChar);
                    }
                }
                catch
                {
                    return false;
                }
            }

        DataReceive:
            recvData = BuildBuffer().ToString();
            return true;
        }


        
    }
}
