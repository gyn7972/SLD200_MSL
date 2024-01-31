using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Xml;
using System.Windows.Forms.VisualStyles;
using System.Net.Sockets;
using SIOImport;

namespace QMC.Core.Laser
{
    public class PowerMeter_CoherentPm1019C : IPowerMeter
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public bool IsReady 
        {
            get { return this.IsOpen && !this.IsBusy; }
        }
        public bool IsBusy { get; set; }
        public bool IsError { get; set; }

        public bool IsOpen { get; set; }


        private ISerial serial;

        private long    timeoutMs;
        private int     portNo;
        private double  MeasureWatt = 0;

        //readonly char STX = (char)0x02;
        readonly char ETX = (char)0x0A;


        object syncRoot;
        int receiveFailCnt;
        bool disposed = false;

        /// <summary>
        /// 
        /// </summary>
        public PowerMeter_CoherentPm1019C()
        { }

        #region 소멸자및 자원 해제
        /// <summary>
        /// 소멸자
        /// </summary>
        ~PowerMeter_CoherentPm1019C()
        {
            if (this.disposed)
                return;
            this.Dispose(false);
        }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <param name="disposing"></param>
        protected void Dispose(bool disposing)
        {
            if (this.disposed)
                return;
            if (disposing)
            {
            }
            this.disposed = true;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Initialze(int portNo)
        {
            bool success = true;
            //XmlDocument xmlDoc = new XmlDocument();
            //xmlDoc.Load(Define.ConfigAuxiliaryFilePath);
            //var nodes = xmlDoc.SelectNodes("/config/auxseq/powermeter");
            //int portNo = int.Parse(nodes[0].Attributes["port"].Value.ToString());

            //serial = new SerialDefault();
            //success &= serial.Initialize(3, 9600);

            serial = new Serialsio();
            success &= serial.Initialize(portNo, SIOWrap.B9600);

            if (success)
            {
                IsOpen = true;
            }
            return success;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="watt"></param>
        /// <returns></returns>
        public bool Power(out double watt)
        {
            double checkNum = 0;
            watt = 0;
            
            CommandPacket pk = MakePacket(CommandType.GetData);
            this.serial.Write(pk.cmd);
            var recvData = this.serial.Read();
            string[] input = recvData.Split(',');
            if (string.IsNullOrEmpty(input[0]) == false)
            {
                if (double.TryParse(input[0], out checkNum) == true)
                {
                    if (checkNum == 0) return false;
                    watt = checkNum;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            CommandPacket pk = MakePacket(CommandType.Start);
            return this.serial.Write(pk.cmd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Stop()
        {
            this.IsBusy = false;
            CommandPacket pk = MakePacket(CommandType.Stop);
            return this.serial.Write(pk.cmd);
        }

        private bool Reset()
        {
            CommandPacket pk = MakePacket(CommandType.Reset);
            if (this.IsOpen)
            {
                this.serial.Write(pk.cmd);
                this.IsError = false;
                return true;
            }
            this.IsError = true;
            return false;
        }

        public enum CommandType
        {
            Start,          //dst
            Stop,           //dsp
            Reset,          //*rst
            GetData,        //pw?
            GetMaxRange,    //rmx
            GetMinRange,    //rmi
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CommandPacket
        {
            public string cmd;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public CommandPacket MakePacket(CommandType cmd)
        {
            CommandPacket packet = new CommandPacket();
            switch (cmd)
            {
                case CommandType.Start:         packet.cmd = "dst"; break;
                case CommandType.Stop:          packet.cmd = "dsp"; break;
                case CommandType.Reset:         packet.cmd = "*rst"; break;
                case CommandType.GetData:       packet.cmd = "pw?"; break;
                case CommandType.GetMaxRange:   packet.cmd = "rmx"; break;
                case CommandType.GetMinRange:   packet.cmd = "rmi"; break;
            }
            packet.cmd += Environment.NewLine;
            return packet;
        }
    }
}
