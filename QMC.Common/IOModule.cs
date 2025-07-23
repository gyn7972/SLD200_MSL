using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Collections.ObjectModel;

namespace QMC.Common
{
	public delegate void DigitalEdgeDetectedCallback(int address, DigitalEdge edge);

	[Serializable]
	public abstract class IOModule
    {
		public uint No => Configuration.No;
		private IoModuleConfiguration m_Configuration;
		public IOBuffer InputBuffer { set; get; }
		public IOBuffer OutputBuffer { set; get; }
		public IOBuffer SimulationInBuffer { set; get; }
		public IOBuffer SimulationOutBuffer { set; get; }

		public IOBoard Board { set; get; }
		public virtual IoModuleConfiguration Configuration 
		{
			set
			{
				m_Configuration = value;
				InputBuffer.Count = m_Configuration.InputCount;
				OutputBuffer.Count = m_Configuration.OutputCount;
				SimulationInBuffer.Count = InputBuffer.Count;
				SimulationOutBuffer.Count = OutputBuffer.Count;
			}
			get
            {
				return m_Configuration;
			}
		}
		public List<IOPoint> Points { set; get; }

		public bool IsOpen { set; get; }
		public bool IsEnableOutput { set; get; }

		public virtual bool Simulated
		{
			get
			{
				bool result;
				if (!Configuration.Simulated)
				{
					result = Board.Simulated;
				}
				else
				{
					result = true;
				}
				return result;
			}
		}

		private int m_TickCount = 0;

		public IOModule()
        {
			Points = new List<IOPoint>();
			m_Configuration = new IoModuleConfiguration();
			m_TickCount = Environment.TickCount;
		}

		public uint GetMaxAxisUid()
		{
			uint nMaxUid = 0;
			foreach (IOPoint Points in Points)
			{
				if (nMaxUid < Points.UID)
					nMaxUid = Points.UID;
			}

			return nMaxUid;
		}

		private bool VerifyModuleIndex(int nIndex)
		{
			bool bRet = false;

			if (Points.Count > nIndex && nIndex >= 0)
			{
				bRet = true;
			}

			return bRet;
		}

		public IOPoint GetIOPoint(int nIndex)
		{
			IOPoint point = null;
			if (VerifyModuleIndex(nIndex))
			{
				point = Points[nIndex];
			}

			return point;
		}
		public int Read()
		{
			int result = 0;

			//if (Configuration.InputCount <= 0)
			//{
			//	return result;
			//}

			//2025-07-23 I/O Signal.
			//Configuration.RefreshTime = 10; // 10ms로 설정
            if ((double)(Environment.TickCount - m_TickCount) < Configuration.RefreshTime)
			{
				return result;
			}
			if ((result = ReadProcedure()) != 0)
			{
				return result;
			}
			m_TickCount = Environment.TickCount;
			return result;
		}

		private int ReadProcedure()
		{
			int result = 0;
			byte[] array = null;
			if (Simulated)
			{
				return result;
			}
			if (Configuration.InputCount > 0)
			{
				array = InputBuffer.GetValues();
				if ((result = OnRead(ref array)) != 0)
				{
					return result;
				}
				InputBuffer.SetValues(array);
			}
			else
			{
				array = OutputBuffer.GetValues();
				if ((result = OnReadOutput(ref array)) != 0)
				{
					return result;
				}
				OutputBuffer.SetValues(array);
			}
			return result;
		}

		protected abstract int OnRead(ref byte[] values);

		protected abstract int OnReadOutput(ref byte[] values);

		public int Write()
		{
			int num = 0;
			byte[] bytes = null;
			object syncRoot = OutputBuffer.SyncRoot;
			bool lockTaken = false;
			try
			{
				Monitor.Enter(syncRoot, ref lockTaken);
				bytes = OutputBuffer.GetValues();
			}
			finally
			{
				Monitor.Exit(syncRoot);
			}
			num = WriteProcedure(bytes);
			
			return num;
		}

		public int Write(byte[] bytes)
		{
			int num = 0;
			if ((num = WriteProcedure(bytes)) != 0)
			{
				return num;
			}
			object syncRoot = OutputBuffer.SyncRoot;
			bool lockTaken = false;
			try
			{
				Monitor.Enter(syncRoot, ref lockTaken);
				OutputBuffer.SetValues(bytes);
			}
			finally
			{
				if (lockTaken)
				{
					Monitor.Exit(syncRoot);
				}
			}
			return num;
		}

		private int WriteProcedure(byte[] bytes)
		{
			int result = 0;

			if (Configuration.OutputCount <= 0)
			{
				return result;
			}
			//if (!IsEnableOutput)
			//{
			//	return result;
			//}
			if (Simulated)
			{
				return result;
			}
			if ((result = OnWrite(bytes)) != 0)
			{
				return result;
			}
			return result;
		}

		protected abstract int OnWrite(byte[] values);

		internal virtual int ApplyConfiguration()
		{
			InputBuffer.Count = Configuration.InputCount;
			OutputBuffer.Count = Configuration.OutputCount;
			SimulationInBuffer.Count = InputBuffer.Count;
			SimulationOutBuffer.Count = OutputBuffer.Count;
			return 0;
		}

		public int Open()
		{
			int num = 0;
			if ((num = ApplyConfiguration()) != 0)
			{
				return num;
			}
			
			if (IsOpen)
			{
				if ((num = Close()) != 0)
				{
					return num;
				}
			}
			if (!Simulated)
			{
				if ((num = OnOpen()) != 0)
				{
					return num;
				}
			}
			
			
			return num;
		}

		protected abstract int OnOpen();
		public int GetIOPointCount()
		{
			return Points.Count;
		}		

		public int Close()
		{
			int num = 0;
			
			if (!Simulated)
			{
				if ((num = OnClose()) != 0)
				{
					return num;
				}
			}
			
			IsOpen = false;
			
			return num;
		}
		public abstract int Load(FileStream fs);
		public int Save(FileStream fs)
		{
			int ret = 0;

			ret = SaveManager.BinarySerialize(fs, this.Configuration);

			return ret;
		}
		protected abstract int OnClose();
	}

	[Serializable]
    public abstract class DioModule : IOModule
    {
		[Serializable]
		internal class InterruptSpecification
		{
			private ActiveLevel m_Level;

			private DigitalEdgeDetectedCallback m_DownEdgeDetected;

			private DigitalEdgeDetectedCallback m_UpEdgeDetected;

			public ActiveLevel Level
			{
				get
				{
					return m_Level;
				}
				set
				{
					m_Level = value;
				}
			}

			public DigitalEdgeDetectedCallback DownEdgeDetected
			{
				get
				{
					return m_DownEdgeDetected;
				}
				set
				{
					m_DownEdgeDetected = value;
				}
			}

			public DigitalEdgeDetectedCallback UpEdgeDetected
			{
				get
				{
					return m_UpEdgeDetected;
				}
				set
				{
					m_UpEdgeDetected = value;
				}
			}
		}
		[Serializable]
		internal class InterruptTable
		{
			private InterruptSpecification[] m_Specifications;

			public int Count
			{
				get
				{
					return (m_Specifications != null) ? m_Specifications.Length : 0;
				}
				set
				{
					if (value < 0)
					{
						throw new ArgumentOutOfRangeException("Count < 0");
					}
					if (m_Specifications == null)
					{
						m_Specifications = new InterruptSpecification[0];
					}
					if (m_Specifications.Length == value)
					{
						return;
					}
					InterruptSpecification[] specifications = m_Specifications;
					m_Specifications = new InterruptSpecification[value];
					if (m_Specifications.Length < value)
					{
						Array.Copy(specifications, m_Specifications, specifications.Length);
					}
					else if (value < m_Specifications.Length)
					{
						Array.Copy(specifications, m_Specifications, m_Specifications.Length);
					}
					for (int i = 0; i < m_Specifications.Length; i++)
					{
						m_Specifications[i] = new InterruptSpecification();
					}
				}
			}	


			public InterruptSpecification this[int index] => m_Specifications[index];

			public InterruptTable()
			{
				Count = 0;
			}
		}
		public virtual bool SupportsInterrupt => false;
		private InterruptTable Interrupts { set; get; }

		public DioModule() : base()
        {
			InputBuffer = new DioBuffer();
			OutputBuffer = new DioBuffer();
			SimulationInBuffer = new DioBuffer();
			SimulationOutBuffer = new DioBuffer();
			Interrupts = new InterruptTable();
		}

        public DioValue GetValue(bool bSimulator, int nAddress, IoType ioType)
        {
			DioBuffer dioBuffer = null;
			if (bSimulator)
			{
				DioBuffer dioBuffer2;
				if (ioType != IoType.Input)
				{
					dioBuffer2 = this.SimulationOutBuffer as DioBuffer;
				}
				else
				{
					dioBuffer2 = this.SimulationInBuffer as DioBuffer;
				}
				dioBuffer = dioBuffer2;
			}
			else
			{
				dioBuffer = ((ioType == IoType.Input) ? this.InputBuffer : this.OutputBuffer) as DioBuffer;
			}
			int result;
			if (!dioBuffer.GetValue(nAddress))
			{
				result = 0;
			}
			else
			{
				result = 1;
			}

			return (DioValue)result;
		}
		public int SetValue(bool bSimulator, int nAddress, bool bValue)
        {
			DioValue value = DioValue.On;
			if (bValue)
				value = DioValue.On;
			else
				value = DioValue.Off;

			return SetValue(bSimulator, nAddress, value);	

		}
		public int SetValue(bool bSimulator, int nAddress, DioValue value)
        {
			int ret = 0;
			if(!bSimulator)
            {
				DioBuffer dioBuffer = this.OutputBuffer as DioBuffer;
				if(dioBuffer != null)
					dioBuffer.SetValue(nAddress, value);
			}
				

			return ret;
		}

		public List<DioPoint> GetPoints()
		{
			List<DioPoint> listPoints = new List<DioPoint>();
			for (int i = 0; i < base.Points.Count; i++)
			{
				listPoints.Add(base.Points[i] as DioPoint);
			}

			return listPoints;
		}
		private void CheckInterruptValidity(int address)
		{
			if (!SupportsInterrupt)
			{
				throw new InvalidOperationException();
			}
			if (!base.IsOpen)
			{
				throw new ApplicationException();
			}
			if (address < 0 || Interrupts.Count <= address)
			{
				throw new ArgumentOutOfRangeException();
			}
		}

		public int SetInterrupt(DioPoint point, DigitalEdge edge, DigitalEdgeDetectedCallback callback)
		{
			if (point == null)
			{
				throw new ArgumentNullException();
			}
			DioPointConfiguration configuration = point.Configuration as DioPointConfiguration;
			return SetInterrupt(point.Configuration.Address, configuration.Level, edge, callback);
		}

		
		public int SetInterrupt(int address, ActiveLevel level, DigitalEdge edge, DigitalEdgeDetectedCallback callback)
		{
			int num = 0;
			if (callback == null)
			{
				throw new ArgumentNullException();
			}
			CheckInterruptValidity(address);
			InterruptSpecification interruptSpecification = Interrupts[address];
			interruptSpecification.Level = level;
			if (edge == DigitalEdge.Down)
			{
				interruptSpecification.DownEdgeDetected = callback;
			}
			else
			{
				interruptSpecification.UpEdgeDetected = callback;
			}
			if ((num = OnSetInterrupt(address, edge)) != 0)
			{
				return num;
			}
			return num;
		}

		protected virtual int OnSetInterrupt(int address, DigitalEdge edge)
		{
			return 0;
		}


		public int ResetInterrupt(int address, DigitalEdge edge)
		{
			int num = 0;
			CheckInterruptValidity(address);
			InterruptSpecification interruptSpecification = Interrupts[address];
			if (edge == DigitalEdge.Down)
			{
				interruptSpecification.DownEdgeDetected = null;
			}
			else
			{
				interruptSpecification.UpEdgeDetected = null;
			}
			if ((num = OnResetInterrupt(address, edge)) != 0)
			{
				return num;
			}
			return num;
		}

		protected virtual int OnResetInterrupt(int address, DigitalEdge edge)
		{
			return 0;
		}

		protected void FireInterrupt(int address, DigitalEdge edge)
		{
			DigitalEdgeDetectedCallback digitalEdgeDetectedCallback = null;
			CheckInterruptValidity(address);
			InterruptSpecification interruptSpecification = Interrupts[address];
			int num = 0;
			
			if (edge == DigitalEdge.Up)
			{
				num = ((interruptSpecification.Level == ActiveLevel.Low) ? 1 : 0);
			}
			else
			{
				num = (interruptSpecification.Level == ActiveLevel.High) ? 1 : 0;
			}
		
			if (num != 0)
			{
				digitalEdgeDetectedCallback = interruptSpecification.DownEdgeDetected;
			}
			else
			{
				digitalEdgeDetectedCallback = interruptSpecification.UpEdgeDetected;
			}
			if (digitalEdgeDetectedCallback != null)
			{
				digitalEdgeDetectedCallback(address, edge);
			}			
		}

	}

	public abstract class AioModule : IOModule
	{
		protected internal new AioBuffer InputBuffer
		{
			get
			{
				return base.InputBuffer as AioBuffer;
			}
			set
			{
				base.InputBuffer = value;
			}
		}

		protected internal new AioBuffer OutputBuffer
		{
			get
			{
				return base.OutputBuffer as AioBuffer;
			}
			set
			{
				base.OutputBuffer = value;
			}
		}

		protected internal new AioBuffer SimulationInBuffer
		{
			get
			{
				return base.SimulationInBuffer as AioBuffer;
			}
			set
			{
				base.SimulationInBuffer = value;
			}
		}

		protected internal new AioBuffer SimulationOutBuffer
		{
			get
			{
				return base.SimulationOutBuffer as AioBuffer;
			}
			set
			{
				base.SimulationOutBuffer = value;
			}
		}

		public new AioModuleConfiguration Configuration
		{
			get
			{
				return base.Configuration as AioModuleConfiguration;
			}
			set
			{
				base.Configuration = value;
			}
		}

		/// <summary>
		/// AioModule 클래스의 새 인스턴스를 초기화합니다.
		/// </summary>
		public AioModule()
		{
			InputBuffer = new AioBuffer();
			OutputBuffer = new AioBuffer();
			SimulationInBuffer = new AioBuffer();
			SimulationOutBuffer = new AioBuffer();
			Configuration = new AioModuleConfiguration();
		}

		/// <summary>
		/// 소유하고 있는 아날로그 접점을 반환합니다.
		/// </summary>
		/// <returns>AIO 접점의 컬렉션입니다.</returns>
		public List<AioPoint> GetPoints()
		{
			List<AioPoint> list = new List<AioPoint>();
			for (int i = 0; i < base.Points.Count; i++)
			{
				list.Add(base.Points[i] as AioPoint);
			}
			return list;
		
		}

		internal override int ApplyConfiguration()
		{
			int num = 0;
			if ((num = base.ApplyConfiguration()) != 0)
			{
				return num;
			}
			uint aioItemSize2 = (InputBuffer.ItemByte = (OutputBuffer.ItemByte = Configuration.ItemByte));
			aioItemSize2 = (SimulationInBuffer.ItemByte = (SimulationOutBuffer.ItemByte = Configuration.ItemByte));
			return num;
		}
	}
}
