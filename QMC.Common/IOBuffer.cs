using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QMC.Common
{
	[Serializable]
    public abstract class IOBuffer
    {
        private object m_SyncRoot;

        public IOBuffer()
        {
            m_SyncRoot = new object();
        }

        public abstract int Count { get; set; }
        public object SyncRoot
        {
            get
            {
                return m_SyncRoot;
            }
            private set
            {
                m_SyncRoot = value;
            }
        }

		public abstract void SetValues(byte[] values);
		public abstract byte[] GetValues();

	}

	[Serializable]
    public class DioBuffer : IOBuffer
    {
        private byte[] m_Data;

		public override int Count
		{
			get
			{
				object syncRoot = base.SyncRoot;
				bool lockTaken = false;
				try
				{
					Monitor.Enter(syncRoot, ref lockTaken);
					return m_Data.Length;
				}
				finally
				{
					if (lockTaken)
					{
						Monitor.Exit(syncRoot);
					}
				}
			}
			set
			{
				object syncRoot = base.SyncRoot;
				bool lockTaken = false;
				try
				{
					Monitor.Enter(syncRoot, ref lockTaken);
					if (m_Data.Length < value)
					{
						byte[] array = new byte[value];
						Array.Copy(m_Data, array, m_Data.Length);
						m_Data = array;
					}
					else
					{
						if (m_Data.Length <= value)
						{
							return;
						}
						
						byte[] array2 = new byte[value];
						Array.Copy(m_Data, array2, array2.Length);
						m_Data = array2;
						return;
						
					}
				}
				finally
				{
					if (lockTaken)
					{
						Monitor.Exit(syncRoot);
					}
				}
			}
		}
		public DioBuffer()
			: this(1)
		{
		}
		public DioBuffer(int count)
		{
			m_Data = new byte[count];
		}

		private void GetIndex(int address, ref int byteIndex, ref int bitIndex)
		{
			byteIndex = address / 8;
			bitIndex = address - byteIndex * 8;
		}

		public bool GetValue(int nAddress)
		{
			int byteIndex = 0;
			int bitIndex = 0;
			byte[] array = null;
			byte b = 0;
			bool flag = false;
			GetIndex(nAddress, ref byteIndex, ref bitIndex);
			array = GetValues();
			b = array[byteIndex];
			int num;
			if ((b & (1 << bitIndex)) != 0)
			{
				num = 1;
			}
			else
			{
				num = 0;
			}
			flag = (byte)num != 0;
			
			return flag;
		}

		public void SetValue(int nAddress, bool value)
		{
			int byteIndex = 0;
			int bitIndex = 0;
			BitArray bitArray = null;
			GetIndex(nAddress, ref byteIndex, ref bitIndex);
			object syncRoot = base.SyncRoot;
			bool lockTaken = false;
			try
			{
				Monitor.Enter(syncRoot, ref lockTaken);
				bitArray = new BitArray(m_Data);
				bitArray[byteIndex * 8 + bitIndex] = value;
				bitArray.CopyTo(m_Data, 0);
			}
			finally
			{
				if (lockTaken)
				{
					Monitor.Exit(syncRoot);
				}
			}
		}

		public void SetValue(int nAddress, DioValue value)
		{
			bool value2;
			if (value != DioValue.On)
			{
				value2 = false;
			}
			else
			{
				value2 = true;
			}
			SetValue(nAddress, value2);
		}

		public override byte[] GetValues()
		{
			object syncRoot = SyncRoot;
			bool lockTaken = false;
			try
			{
				Monitor.Enter(syncRoot, ref lockTaken);
				return m_Data;
			}
			finally
			{
				if (lockTaken)
				{
					Monitor.Exit(syncRoot);
				}
			}
		}

		public override void SetValues(byte[] values)
		{
			object syncRoot = SyncRoot;
			bool lockTaken = false;
			try
			{
				Monitor.Enter(syncRoot, ref lockTaken);
				Array.Copy(values, m_Data, values.Length);
			}
			finally
			{
				if (lockTaken)
				{
					Monitor.Exit(syncRoot);
				}
			}
		}
	}


	public class AioBuffer : IOBuffer
	{
		private uint m_ItemByte;

		private int[] m_Data;

		public uint ItemByte
		{
			get
			{
				return m_ItemByte;
			}
			internal set
			{
				m_ItemByte = value;
			}
		}

		public override int Count
		{
			get
			{
				lock (base.SyncRoot)
				{
					return m_Data.Length;
				}
			}
			set
			{
				object syncRoot = base.SyncRoot;
				bool lockTaken = false;
				try
				{
					Monitor.Enter(syncRoot, ref lockTaken);
					if (m_Data.Length < value)
					{
						int[] array = new int[value];
						Array.Copy(m_Data, array, m_Data.Length);
						m_Data = array;
					}
					else
					{
						if (m_Data.Length <= value)
						{
							return;
						}
						int[] array2 = new int[value];
						Array.Copy(m_Data, array2, array2.Length);
						m_Data = array2;
						return;
					}
				}
				finally
				{
					if (lockTaken)
					{
						Monitor.Exit(syncRoot);
					}
				}
			}
		}

		public AioBuffer(int count)
		{
			ItemByte = 2;
			m_Data = new int[count];
		}

		public AioBuffer()
			: this(1)
		{
		}


		public int GetValue(int index)
		{
			object syncRoot = base.SyncRoot;
			bool lockTaken = false;
			try
			{
				Monitor.Enter(syncRoot, ref lockTaken);
				return m_Data[index];
			}
			finally
			{
				if (lockTaken)
				{
					Monitor.Exit(syncRoot);
				}
			}
		}

		public void SetValue(int index, int value)
		{
			object syncRoot = base.SyncRoot;
			bool lockTaken = false;
			try
			{
				Monitor.Enter(syncRoot, ref lockTaken);
				m_Data[index] = value;
			}
			finally
			{
				if (lockTaken)
				{
					Monitor.Exit(syncRoot);
				}
			}
		}

		public override byte[] GetValues()
		{
			object syncRoot = SyncRoot;
			bool lockTaken = false;
			try
			{
				Monitor.Enter(syncRoot, ref lockTaken);
				byte[] array = new byte[m_Data.Length * (int)ItemByte];
				for (int i = 0; i < m_Data.Length; i++)
				{
					if (ItemByte == 4)
					{
						array[i * (int)ItemByte + 3] = (byte)((uint)(m_Data[i] >> 24) & 0xFFu);
						array[i * (int)ItemByte + 2] = (byte)((uint)(m_Data[i] >> 16) & 0xFFu);
					}
					array[i * (int)ItemByte + 1] = (byte)((uint)(m_Data[i] >> 8) & 0xFFu);
					array[i * (int)ItemByte] = (byte)((uint)m_Data[i] & 0xFFu);
				}

				return array;
			}
			finally
			{
				if (lockTaken)
				{
					Monitor.Exit(syncRoot);
				}
			}

			
		}

		public override void SetValues(byte[] values)
		{
			object syncRoot = SyncRoot;
			bool lockTaken = false;
			try
			{
				Monitor.Enter(syncRoot, ref lockTaken);
				for (int i = 0; i < m_Data.Length; i++)
				{
					if (ItemByte == 4)
					{
						m_Data[i] = (int)(((values[i * (int)ItemByte + 3] << 24) & 0xFF000000u) + ((values[i * (int)ItemByte + 2] << 16) & 0xFF0000) + ((values[i * (int)ItemByte + 1] << 8) & 0xFF00) + values[i * (int)ItemByte]);
					}
					else
					{
						m_Data[i] = (short)((values[i * (int)ItemByte + 1] << 8) & 0xFF00) + values[i * (int)ItemByte];
					}
				}
			}
			finally
			{
				if (lockTaken)
				{
					Monitor.Exit(syncRoot);
				}
			}
		}
	}

}
