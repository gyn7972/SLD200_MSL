using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;

namespace SerialCommChiller
{
    public class SerialChillerPort : Form
    {
        #region Ascii Code

        byte m_cPlus            = (byte)0x2B;
        byte m_cMinus           = (byte)0x2D;
        byte m_cDot             = (byte)0x2E;
        byte m_cNum0            = (byte)0x30;
        byte m_cNum1            = (byte)0x31;
        byte m_cNum2            = (byte)0x32;
        byte m_cNum3            = (byte)0x33;
        byte m_cNum4            = (byte)0x34;
        byte m_cNum5            = (byte)0x35;
        byte m_cNum6            = (byte)0x36;
        byte m_cNum7            = (byte)0x37;
        byte m_cNum8            = (byte)0x38;
        byte m_cNum9            = (byte)0x39;
        byte m_cSTX             = (byte)0x02;
        byte m_cETX             = (byte)0x13;

        #endregion

        #region Error Code

        byte m_cNoError         = (byte)0x00;
        byte m_cLevelWarning    = (byte)0x40;
        byte m_cLevelError      = (byte)0x20;
        byte m_cHighWarningTemp = (byte)0x10;
        byte m_cHighErrorTemp   = (byte)0x08;
        byte m_cLowWarningTemp  = (byte)0x04;
        byte m_cLowErrorTemp    = (byte)0x02;
        byte m_cFlowError       = (byte)0x01;

        #endregion


        public delegate void DataReceivedHandlerFunc(byte[] receiveData);
        public DataReceivedHandlerFunc DataReceivedHandler;

        public delegate void DisconnectedHandlerFunc();
        public DisconnectedHandlerFunc DisconnectedHandler;

        private SerialPort serialPort;

        public bool IsOpen
        {
            get
            {
                if (serialPort != null) return serialPort.IsOpen;
                return false;
            }
        }

        // serial port check
        private Thread threadCheckSerialOpen;
        private bool isThreadCheckSerialOpen = false;

        public SerialChillerPort()
        {
        }

        public bool OpenComm(string portName, int baudrate, int databits, StopBits stopbits, Parity parity, Handshake handshake)
        {
            try
            {
                serialPort = new SerialPort();

                serialPort.PortName = portName;
                serialPort.BaudRate = baudrate;
                serialPort.DataBits = databits;
                serialPort.StopBits = stopbits;
                serialPort.Parity = parity;
                serialPort.Handshake = handshake;

                serialPort.Encoding = new System.Text.ASCIIEncoding();
                serialPort.NewLine = "\r\n";
                serialPort.ErrorReceived += serialPort_ErrorReceived;
                serialPort.DataReceived += serialPort_DataReceived;

                serialPort.Open();

                StartCheckSerialOpenThread();
                return true;
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(ex.ToString());
                return false;
            }
        }

        public void CloseComm()
        {
            try
            {
                if (serialPort != null)
                {
                    StopCheckSerialOpenThread();
                    serialPort.Close();
                    serialPort = null;
                }
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(ex.ToString());
            }
        }

        public bool Send(string sendData)
        {
            try
            {
                if (serialPort != null && serialPort.IsOpen)
                {
                    serialPort.Write(sendData);
                    return true;
                }
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(ex.ToString());
            }
            return false;
        }

        public bool Send(byte[] sendData)
        {
            try
            {
                if (serialPort != null && serialPort.IsOpen)
                {
                    serialPort.Write(sendData, 0, sendData.Length);
                    return true;
                }
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(ex.ToString());
            }
            return false;
        }

        public bool Send(byte[] sendData, int offset, int count)
        {
            try
            {
                if (serialPort != null && serialPort.IsOpen)
                {
                    serialPort.Write(sendData, offset, count);
                    return true;
                }
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(ex.ToString());
            }
            return false;
        }

        private byte[] ReadSerialByteData()
        {
            serialPort.ReadTimeout = 100;
            byte[] bytesBuffer = new byte[serialPort.BytesToRead];
            int bufferOffset = 0;
            int bytesToRead = serialPort.BytesToRead;

            while (bytesToRead > 0)
            {
                try
                {
                    int readBytes = serialPort.Read(bytesBuffer, bufferOffset, bytesToRead - bufferOffset);
                    bytesToRead -= readBytes;
                    bufferOffset += readBytes;
                }
                catch (TimeoutException ex)
                {
                    //Debug.WriteLine(ex.ToString());
                }
            }

            return bytesBuffer;
        }

        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                byte[] bytesBuffer = ReadSerialByteData();
                string strBuffer = Encoding.ASCII.GetString(bytesBuffer);

                if (DataReceivedHandler != null)
                    DataReceivedHandler(bytesBuffer);

                //Debug.WriteLine("received(" + strBuffer.Length + ") : " + strBuffer);
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(ex.ToString());
            }
        }

        private void serialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            //Debug.WriteLine(e.ToString());
        }

        private void StartCheckSerialOpenThread()
        {
            isThreadCheckSerialOpen = true;
            threadCheckSerialOpen = new Thread(new ThreadStart(ThreadCheckSerialOpen));
            threadCheckSerialOpen.Start();
        }

        private void StopCheckSerialOpenThread()
        {
            if (isThreadCheckSerialOpen)
            {
                isThreadCheckSerialOpen = false;
                if (Thread.CurrentThread != threadCheckSerialOpen)
                    threadCheckSerialOpen.Join();
            }
        }

        private void ThreadCheckSerialOpen()
        {
            while (isThreadCheckSerialOpen)
            {
                Thread.Sleep(100);

                try
                {
                    if (serialPort == null || !serialPort.IsOpen)
                    {
                        //Debug.WriteLine("seriaport disconnected");
                        if (DisconnectedHandler != null)
                            DisconnectedHandler();
                        break;
                    }
                }
                catch (Exception ex)
                {
                    //Debug.WriteLine(ex.ToString());
                }
            }
        }
    }
}
