using System.IO.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace QMC.Core.Laser
{
    public class SerialDefault: ISerial
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

        SerialPort serial;

        string receivedData;
        bool dataReceived;

        bool disposed = false;



        /// <summary>
        /// 생성자
        /// </summary>
        public SerialDefault()
        {

        }

        /// <summary>
        /// 소멸자
        /// </summary>
        ~SerialDefault()
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
            this.serial = new SerialPort();
            this.serial.PortName = $"COM{port}";
            this.serial.BaudRate = baudrate;
            this.serial.DataBits = 8;
            this.serial.StopBits = StopBits.One;
            this.serial.Parity = Parity.None;

            string[] portName = SerialPort.GetPortNames();
            bool isFind = false;
            for (int i = 0; i < portName.Length; i++)
            {
                if (portName[i] == this.serial.PortName)
                {
                    this.serial.Open();
                    this.dataReceived = false;
                    this.serial.DataReceived += new SerialDataReceivedEventHandler(OnReceiveData);
                    //Logger.Log(Logger.Module.Vision, Logger.Type.Trace, $"{this.Name} initialized: COM{port}");

                    isFind = true;
                }
            }
            return true;
        }

        private void OnReceiveData(object sender, SerialDataReceivedEventArgs e)
        {
            // int readByte = this.serial.ReadByte();
            try
            {
                this.serial.ReadTo(receivedData);
                dataReceived = true;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
                dataReceived = false;
            }
        }

        public bool Write(string data)
        {

            byte[] bytes = Encoding.ASCII.GetBytes(data);
            try
            {
                if (data != null)
                {
                    this.serial.Write(bytes, 0, bytes.Length);
                    //this.serial.Write(data);
                    IsWaitReceive = true;
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
            var sw = Stopwatch.StartNew();
            
            while (this.IsWaitReceive)
            {
                if (sw.ElapsedMilliseconds > 10000) 
                    return "";
                this.IsWaitReceive = false;
                return receivedData;
            }
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
    }
}
