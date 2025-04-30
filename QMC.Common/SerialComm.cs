using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;
using System.Windows.Forms;

namespace QMC.Common
{
    public class SerialComm
    {
        protected SerialPort m_SerialPort;

        public string PortName
        {
            set
            {
                m_SerialPort.PortName = value;
            }
            get
            {
                return m_SerialPort.PortName;
            }
        }

        public int BaudRate
        {
            set
            {
                m_SerialPort.BaudRate = value;
            }
            get
            {
                return m_SerialPort.BaudRate;
            }
        }

        public int DataBits
        {
            set
            {
                m_SerialPort.DataBits = value;
            }
            get
            {
                return m_SerialPort.DataBits;
            }
        }

        public StopBits StopBits
        {
            set
            {
                m_SerialPort.StopBits = value;
            }
            get
            {
                return m_SerialPort.StopBits;
            }
        }

        public Parity Parity
        {
            set
            {
                m_SerialPort.Parity = value;
            }
            get
            {
                return m_SerialPort.Parity;
            }
        }

        public int BytesToRead
        {
            get
            {
                return m_SerialPort.BytesToRead;
            }
        }

		public Handshake Handshake
        {
            set
            {
				m_SerialPort.Handshake = value;
			}
			get
            {
				return m_SerialPort.Handshake;

			}
        }

        public int InterCharacterTimeout { set; get; }

        public SerialComm()
        {
            m_SerialPort = new SerialPort();
            InterCharacterTimeout = 500;
            //m_SerialPort.DataReceived += M_SerialPort_DataReceived;
        }

        //private void M_SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        //{
        //    //m_SerialPort.Read()
        //}

        public static List<string> GetPortNames()
        {
            return SerialPort.GetPortNames().ToList();
        }

        public void Open()
        {
			try
			{
				if (m_SerialPort != null && !m_SerialPort.IsOpen)
				{
					m_SerialPort.Open();
				}
			}
			catch (Exception ex)
			{
                Log.Write(ex);
                MessageBox.Show(ex.Message);
			}
		}

        public void Close()
        {
            if (m_SerialPort != null && m_SerialPort.IsOpen)
            {
                m_SerialPort.Close();
            }
        }

        public bool WaitRecv(int nTimeout)
        {
            DateTime startTime = DateTime.Now;
            while (BytesToRead <= 0)
            {
				TimeSpan timeSpan = DateTime.Now - startTime;
                if (timeSpan.TotalMilliseconds > nTimeout)
                {
                    return false;
                }
            }
            Thread.Sleep(10);

            return true;
        }
		public int SendFrame(string value, string stxString, string etxString)
		{
			return SendFrame(Encoding.ASCII.GetBytes(value), stxString, etxString);
		}
		public int SendFrame(string value, byte stxByte, byte etxByte)
		{
			if (etxByte == 0)
			{
				return -1;
			}
			byte[] stxBytes = ((stxByte == 0) ? null : new byte[1] { stxByte });
			return SendFrame(value, stxBytes, new byte[1] { etxByte });
		}
		public int SendFrame(string value, byte[] stxBytes, byte[] etxBytes)
		{
			return SendFrame(Encoding.ASCII.GetBytes(value), stxBytes, etxBytes);
		}
		public int SendFrame(byte[] value, string stxString, string etxString)
		{
			if (value == null || value.Length == 0)
			{
				return -1;
			}
			byte[] stxBytes;
			if (stxString == null)
			{
				stxBytes = null;
			}
			else
			{
				stxBytes = Encoding.ASCII.GetBytes(stxString);
			}
			byte[] etxBytes;
			if (etxString == null)
			{
				etxBytes = null;
			}
			else
			{
				etxBytes = Encoding.ASCII.GetBytes(etxString);
			}
			return SendFrame(value, stxBytes, etxBytes);
		}

		public int SendFrame(byte[] value, byte stxByte, byte etxByte)
		{
			if (etxByte == 0)
			{
				return -1;
			}
			byte[] stxBytes;
			if (stxByte != 0)
			{
				stxBytes = new byte[1] { stxByte };
			}
			else
			{
				stxBytes = null;
			}
			return SendFrame(value, stxBytes, new byte[1] { etxByte });
		}

		public int SendFrame(byte[] value, byte[] etxBytes)
		{
			return SendFrame(value, null, etxBytes);
		}
		public int SendFrame(string value, byte[] etxBytes)
		{
			return SendFrame(value, null, etxBytes);
		}
		public int SendFrame(string value, byte etxByte)
		{
			return SendFrame(value, new byte[1] { etxByte });
		}
		public int SendFrame(string value, string etxString)
		{
			return SendFrame(value, null, etxString);
		}
		public int SendFrame(byte[] value, string etxString)
		{
			byte[] etxBytes;
			if (etxString == null)
			{
				etxBytes = null;
			}
			else
			{
				etxBytes = Encoding.ASCII.GetBytes(etxString);
			}
			return SendFrame(value, etxBytes);
		}
		public int SendFrame(byte[] value, byte[] stxBytes, byte[] etxBytes)
		{
			if (value == null || value.Length == 0)
			{
				return -1;
			}
			int numStx;
			if (stxBytes == null)
			{
                numStx = 0;
			}
			else
			{
                numStx = stxBytes.Length;
			}
			
			
			int numEtx;
			if (etxBytes == null)
			{
                numEtx = 0;
			}
			else
			{
                numEtx = etxBytes.Length;
			}

			byte[] array = new byte[numStx + value.Length + numEtx];
			if (numStx != 0)
			{
				for (int i = 0; i < numStx; i++)
				{
					array[i] = stxBytes[i];
				}
			}
			Array.Copy(value, 0, array, numStx, value.Length);
			if (numEtx != 0)
			{
				for (int j = 0; j < numEtx; j++)
				{
					array[numStx + value.Length + j] = etxBytes[j];
				}
			}
			return SendBytes(array);
		}
        public int SendByte(byte value)
        {
            return SendBytes(new byte[1] { value });
        }
        public int SendBytes(byte[] value)
        {
            if (value == null || value.Length == 0)
            {
                return -1;
            }

            return OnSend(value);
        }

        protected int OnSend(byte[] data)
        {
            int result = 0;
            if (m_SerialPort != null)
            {
                m_SerialPort.Write(data, 0, data.Length);
            }
            else
            {
                result = -1;
            }

            return result;
        }
		public int ReceiveByte(ref byte value, int timeout)
		{
			int ret = 0;
			byte[] data = null;
			TimeoutChecker timeoutChecker = new TimeoutChecker(timeout, autoStart: true);
			while (true)
			{
				if (0 < BytesToRead)
				{
                    if ((ret = OnRecieve(out data, 1)) != 0)
                    {
                        return ret;
                    }

                    value = data[0];
                    return ret;
                }
				if (timeoutChecker.IsCompleted || timeout == 0)
				{
					break;
				}
				Thread.Sleep(1);
			}
			return -1;
		}
		public int ReceiveFrame(out string value, string etxString)
		{
			value = string.Empty;
			if (string.IsNullOrEmpty(etxString))
			{
				return -1;
			}
			
			return ReceiveFrame(out value, (byte[])null, Encoding.ASCII.GetBytes(etxString));
		}

		public int ReceiveFrame(out string value, byte[] stxBytes, byte[] etxBytes)
		{
			int num = 0;
			value = string.Empty;
			if (etxBytes == null || etxBytes.Length < 1)
			{
				return -1;
			}
			
			if ((num = ReceiveFrame(out byte[] value2, stxBytes, etxBytes)) != 0)
			{
				return num;

			}
			if (value2 == null)
			{
				value = string.Empty;
			}
			else
			{
				value = Encoding.ASCII.GetString(value2);
			}
			return num;
		}

		public int ReceiveFrame(out byte[] value, byte etxByte)
        {
            value = null;
            if (etxByte == 0)
            {
                return -1;
            }
            return ReceiveFrame(out value, (byte)0, etxByte);
        }
        public int ReceiveFrame(out byte[] value, byte stxByte, byte etxByte)
        {
            value = null;
            if (etxByte == 0)
            {
                return -1;
            }
            byte[] stxBytes;
            if (stxByte != 0)
            {
                stxBytes = new byte[1] { stxByte };
            }
            else
            {
                stxBytes = null;
            }
            return ReceiveFrame(out value, stxBytes, new byte[1] { etxByte });
        }

        public int ReceiveFrame(out byte[] value, byte[] stxBytes, byte[] etxBytes)
		{
			int num = 0;
			string empty = string.Empty;
			bool flag = false;
			List<byte> listBytes = new List<byte>();
			value = null;
			if (etxBytes == null)
			{
				return -1;
			}
			if (etxBytes.Length < 1)
			{
				return -1;
			}
			int num2;
			if (stxBytes != null)
			{
				num2 = ((stxBytes.Length == 0) ? 1 : 0);
			}
			else
			{
				num2 = 1;
			}
			if (num2 != 0)
			{
				flag = true;
			}
			while (true)
			{
				byte value2 = 0;
				if ((num = ReceiveByte(ref value2, InterCharacterTimeout)) != 0)
				{
					value = new byte[listBytes.Count];
					listBytes.CopyTo(value, 0);
					return num;
				}
				listBytes.Add(value2);
				if (!flag)
				{
					if (listBytes.Count < stxBytes.Length)
					{
						continue;
					}
					bool flag2 = true;
					for (int i = 0; i < stxBytes.Length; i++)
					{
						if (listBytes[listBytes.Count - stxBytes.Length + i] != stxBytes[i])
						{
                            flag2 = false;
                        }						
					}
					if (!flag2)
					{
						continue;
					}
					
					flag = true;
                    listBytes.Clear();
					continue;
				}
				if (listBytes.Count < etxBytes.Length)
				{
					continue;
				}
				bool flag3 = true;
				for (int j = 0; j < etxBytes.Length; j++)
				{
					if (listBytes[listBytes.Count - etxBytes.Length + j] != etxBytes[j])
					{
                        flag3 = false;
                    }
					
				}
				if (flag3)
				{
					break;
				}
			}
			for (int k = 0; k < etxBytes.Length; k++)
			{
                listBytes.RemoveAt(listBytes.Count - 1);
			}
			
			value = new byte[listBytes.Count];
            listBytes.CopyTo(value, 0);
			return num;
		}

		protected int OnRecieve(out byte[] data, int count)
        {
            int result = 0;
            if (count < 1)
            {
                data = null;
                return -1;
            }

            data = new byte[count];
            int nRead = m_SerialPort.Read(data, 0, count);

            if (nRead != count)
            {
                return -1;
            }

            return result;

        }
    }
}
