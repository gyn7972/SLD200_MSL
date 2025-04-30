//using QMC.Core;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;


namespace QMC.Aux
{    public class BCR_NLV5201_Opticon
    {
        long timeoutMs;
        int portNo;
        int baudRate = B9600;
        int parity = P_NONE;
        int dataBit = BIT_8;
        int stopBit = STOP_1;
        readonly char STX = (char)0x02;
        readonly char ETX = (char)0x03;
        readonly char RETX = (char)0x0D;

        bool isOpen { get; set; }
        bool isWaitReceive { get; set; }
        object syncRoot;
        int receiveFailCnt;
        Queue<char> bufferQueue = new Queue<char>();
        Queue<StringBuilder> receiveBufferQueue = new Queue<StringBuilder>();

        public bool Open(int port, int timeout = 500)
        {
            isOpen = false;
            portNo = port;
            sio_ioctl(portNo, baudRate, parity | dataBit | stopBit);
            if (SIO_OK != sio_open(portNo))
            {
                //Logger.Log(Logger.Module.Global, Logger.Type.Error, $"fail to open barcode communication port: {port}");       //  SCH : Log 는 임시로 주석 처리함.
                return false;
            }


            if (timeout < 10) timeout = 500;
            this.timeoutMs = timeout;

            isWaitReceive = true;
            isOpen = true;
            return true;
        }
        public bool Close()
        {
            isOpen = false;
            isWaitReceive = false;
            sio_close(portNo);

            return true;
        }
        public bool IsOpen()
        {
            return isOpen;
        }

        public bool Read(out string data)
        {
            data = string.Empty;
            char command = Convert.ToChar(0x5A); // 0x02
           // lock (this.syncRoot)
           // {
                if (this.isOpen)
                {
                    this.SendData($"{STX}{command}{ETX}");
                    if (this.WaitData(out data, this.timeoutMs))
                    {
                        this.receiveFailCnt = 0;
                        return true;
                    }
                    else
                        this.receiveFailCnt++;
            //    }
                }
            return false;
        }
        public bool Read(string command, out string data)
        {
            data = string.Empty;
            lock (this.syncRoot)
            {
                if (this.isOpen)
                {
                    this.SendData($"{STX}{command}{ETX}");
                    if (this.WaitData(out data, this.timeoutMs))
                    {
                        this.receiveFailCnt = 0;
                        return true;
                    }
                    else
                        this.receiveFailCnt++;
                }
            }
            return false;
        }


        // Response: "ACK" (ASCII:0x06)
        // The scanner terminates transmission with the good-read buzzer.

        // Response: "NAK" (ASCII:0x15)
        // The scanner sends the data again.

        // Response: "DC1" (ASCII:0x11)
        // The scanner terminates transmission without the good-read or error buzzer.


        #region Serial_Functions
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
            start = ticks = DateTime.Now.Ticks;

            while (this.isWaitReceive)
            {
                ticks = DateTime.Now.Ticks;
                if (ticks > start + timeout * 10000) return false;

                try
                {
                    len = sio_iqueue(portNo);
                    for (int i = 0; i < len; i++)
                    {
                        recvChar = Convert.ToChar(sio_getch(portNo));
                        if (recvChar.Equals(RETX)) goto DataReceive;
                        this.bufferQueue.Enqueue(recvChar);
                    }
                }
                catch (System.Exception ex)
                {
                    //Log.Write(ex);
                    return false;
                }
            }

        DataReceive:
            recvData = BuildBuffer().ToString();
            return true;
        }
        private bool SendData(string data)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(data);
            try
            {
                if (data != null)
                {
                    sio_flush(portNo, FLUSH_INPUT_OUTPUT);
                    this.receiveBufferQueue.Clear();
                    this.bufferQueue.Clear();
                    int ret = sio_write(portNo, bytes, bytes.Length);
                    return true;
                }
                else return false;

            }
            catch (System.Exception ex)
            {
                //Log.Write(ex);
                return false;
            }

        }
        #endregion

        #region define PCOMM

        /// <summary>
        /// Baud-Rate Settings        
        /// 50 ~ 921600 bps
        /// </summary>
        public const int B50 = 0x00;
        public const int B75 = 0x01;
        public const int B110 = 0x02;
        public const int B134 = 0x03;
        public const int B150 = 0x04;
        public const int B300 = 0x05;
        public const int B600 = 0x06;
        public const int B1200 = 0x07;
        public const int B1800 = 0x08;
        public const int B2400 = 0x09;
        public const int B4800 = 0x0A;
        public const int B7200 = 0x0B;
        public const int B9600 = 0x0C;
        public const int B19200 = 0x0D;
        public const int B38400 = 0x0E;
        public const int B57600 = 0x0F;
        public const int B115200 = 0x10;
        public const int B230400 = 0x11;
        public const int B460800 = 0x12;
        public const int B921600 = 0x13;

        /// <summary>
        /// Mode Setting :: Word length        
        /// </summary>
        public const int BIT_5 = 0x00;
        public const int BIT_6 = 0x01;
        public const int BIT_7 = 0x02;
        public const int BIT_8 = 0x03;

        /// <summary>
        /// Mode Setting :: Stop Bit
        /// </summary>
        public const int STOP_1 = 0x00;
        public const int STOP_2 = 0x04;

        /// <summary>
        /// Mode Setting :: Perity Bit
        /// </summary>
        public const int P_EVEN = 0x18;
        public const int P_ODD = 0x08;
        public const int P_SPC = 0x38;
        public const int P_MRK = 0x28;
        public const int P_NONE = 0x00;

        /// <summary>
        /// Modem Control Settings
        /// </summary>
        public const int C_DTR = 0x01;
        public const int C_RTS = 0x02;


        /// <summary>
        /// Modem Line Status
        /// </summary>
        public const int S_CTS = 0x01;
        public const int S_DSR = 0x02;
        public const int S_RI = 0x04;
        public const int S_CD = 0x08;

        public const int SIO_OK = 0;
        public const int SIO_BADPORT = -1;	            // no such port or port not opened 
        public const int SIO_OUTCONTROL = -2;	        // can't control the board 
        public const int SIO_NODATA = -4;	            // no data to read or no buffer to write 
        public const int SIO_OPENFAIL = -5;	            // no such port or port has be opened 
        public const int SIO_RTS_BY_HW = -6;            // RTS can't set because H/W flowctrl 
        public const int SIO_BADPARM = -7;	            // bad parameter 
        public const int SIO_WIN32FAIL = -8;            // call win32 function fail, please call 
        public const int SIO_BOARDNOTSUPPORT = -9;	    // Does not support this board 
        public const int SIO_FAIL = -10;                // PComm function run result fail 
        public const int SIO_ABORTWRITE = -11;          // write has blocked, and user abort write 
        public const int SIO_WRITETIMEOUT = -12;        // write timeoue has happened 

        public const int SIOFT_OK = 0;
        public const int SIOFT_BADPORT = -1;	        // no such port or port not open 
        public const int SIOFT_TIMEOUT = -2;	        // protocol timeout 
        public const int SIOFT_ABORT = -3;	            // user key abort 
        public const int SIOFT_FUNC = -4;	            // func return abort 
        public const int SIOFT_FOPEN = -5;	            // can not open files 
        public const int SIOFT_CANABORT = -6;	        // Ymodem CAN signal abort 
        public const int SIOFT_PROTOCOL = -7;	        // Protocol checking error abort 
        public const int SIOFT_SKIP = -8;	            // Zmodem remote skip this send file 
        public const int SIOFT_LACKRBUF = -9;	        // Zmodem Recv-Buff size must >= 2K bytes 
        public const int SIOFT_WIN32FAIL = -10;	        // OS fail 
        public const int SIOFT_BOARDNOTSUPPORT = -11;   // Does not support board 


        public const int FLUSH_INPUT = 0;
        public const int FLUSH_OUPUT = 1;
        public const int FLUSH_INPUT_OUTPUT = 2;

        public const int FLOW_CONTROL_CTS = 0x01;
        public const int FLOW_CONTROL_RTS = 0x02;
        public const int FLOW_CONTROL_XON = 0x04;
        public const int FLOW_CONTROL_XOFF = 0x08;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct CHAR_PTR
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 5000)]
            public string buffer;
        }

#if WIN32
        public const string DLL_FILE_NAME = "PComm32.dll";
#else
        public const string DLL_FILE_NAME = "PComm64.dll";
#endif

        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_open(int port);
        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_ioctl(int port, int baud, int mode);
        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_write(int port, string buf, int len);
        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_write(int port, byte[] buf, int len);
        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_write(int port, char[] buf, int len);

        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_read(int port, ref StringBuilder buf, int len);
        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_close(int port);
        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_iqueue(int port);
        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_oqueue(int port);

        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_getch(int port);
        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_putch(int port, int ch);
        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_flush(int port, int flush_mode);
        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_lstatus(int port);          // Get line status, such as CTS, DSR, DCD, RI.
        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_lctrl(int port, int mode);   // Set both the DTR and RTS state.

        public delegate void CNT_IRQ(int port, int count);
        public delegate void MODEM_IRQ(int port);
        public delegate void BREAK_IRQ(int port);
        public delegate void TX_EMPTY_IRQ(int port);

        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_cnt_irq(int port, CNT_IRQ cirq);
        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_modem_irq(int port, MODEM_IRQ mirq);
        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_break_irq(int port, BREAK_IRQ birq);
        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_Tx_empty_irq(int port, TX_EMPTY_IRQ tirq);

        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_break(int port, int tictime);    // Send out a break signal. This function will block until the time is over.
        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_break_ex(int port, int mstime);
        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_flowctrl(int port, int mode);    // Set hardware and/or software flow control.
        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_Tx_hold(int port);               // Check the reason why data could not be transmitted.

        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_getbaud(int port);
        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_getmode(int port);
        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_DTR(int port, int mode);
        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_RTS(int port, int mode);
        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_baud(int port, long baudrate);
        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_data_status(int port);            // Check if any error encountered when receiving data.

        public delegate void TERM_IRQ(int port, char code);
        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_term_irq(int port, TERM_IRQ tirq);

        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_linput(int port, ref StringBuilder buf, int lne, int term);
        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_putb_x(int port, ref StringBuilder buf, int len, int tick);
        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_putb_x_ex(int port, ref StringBuilder buf, int len, int tms);
        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_view(int port, ref StringBuilder buf, int len);
        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_TxLowWater(int port, int size);
        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_AbortWrite(int port);
        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_SetWriteTimeouts(int port, UInt32 TotalTimeouts);
        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_GetWriteTimeouts(int port, ref UInt32 TotalTimeouts);
        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_SetReadTimeouts(int port, UInt32 TotalTimeouts, UInt32 IntervalTimeouts);
        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_GetReadTimeouts(int port, ref UInt32 TotalTimeouts, ref UInt32 IntervalTimeouts);
        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_AbortRead(int port);

        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_ActXoff(int port);
        [DllImport(DLL_FILE_NAME)]
        public static extern int sio_ActXon(int port);
        #endregion

    }
}
