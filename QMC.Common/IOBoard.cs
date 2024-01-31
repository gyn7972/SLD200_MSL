using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.ObjectModel;

namespace QMC.Common
{
	[Serializable]
    public abstract class IOBoard
    {
		protected uint m_nLastUid;
        public bool Simulated { set; get; }
		public bool IsOpen { set; get; }
		public List<IOModule> Modules { set; get; }
		public virtual IOBoadConfiguration Configuration { set; get; }

		public IOBoard()
		{
			Modules = new List<IOModule>();
            Configuration = new IOBoadConfiguration();
			m_nLastUid = 0;
		}

        public int Open()
		{
			int num = 0;
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
			foreach(IOModule current in Modules)
            {
				if ((num = current.Open()) != 0)
                {
					break;
				}
					
			}
			
			if (num == 0)
			{
				IsOpen = true;
			}
			return num;
		}

		/// <summary>
		/// 사용하기 위한 준비 작없을 수행합니다.
		/// </summary>
		/// <returns>작업에 대한 결과를 반환한다. 0이면 성공 그렇지 않으면 실패를 나타냅니다</returns>
		protected abstract int OnOpen();

		/// <summary>
		/// 사용하던 리소스를 반환하고 제어를 멈추도록 합니다.
		/// </summary>
		/// <returns>작업에 대한 결과를 반환한다. 0이면 성공 그렇지 않으면 실패를 나타냅니다</returns>
		public int Close()
		{
			int num = 0;
			if (!Simulated && (num = OnClose()) != 0)
			{
				return num;
			}
			foreach (IOModule module in Modules)
			{
				if ((num = module.Close()) != 0)
				{
					break;
				}
			}

			if (num == 0)
			{
				IsOpen = false;
			}
			return num;
		}
       
		/// <summary>
		/// 사용하던 리소스를 반환하고 제어를 멈추도록 합니다.
		/// </summary>
		/// <returns>작업에 대한 결과를 반환한다. 0이면 성공 그렇지 않으면 실패를 나타냅니다</returns>
		protected abstract int OnClose();

		
		public int Save(FileStream fs)
		{
			int ret = 0;
			if ((ret = this.Configuration.Save(fs)) != 0) return ret;

            foreach (IOModule module in Modules)
            {
                if ((ret = module.Save(fs)) != 0) return ret;
				foreach(IOPoint point in module.Points)
                {
					if((ret = point.Save(fs)) != 0) return ret;
                }
            }
 
            return ret;
		}
		public abstract int Load(FileStream fs);
		public int GetIOModuleCount()
		{
			return Modules.Count;
		}
		public void ClearModule()
		{
			Modules.Clear();
		}
		public uint GetMaxDioPointUid()
		{
			uint nMaxUid = 0;
			foreach (IOModule module in Modules)
			{
                foreach (IOPoint point in module.Points)
                {
					if (nMaxUid < point.UID)
						nMaxUid = point.UID;
				}

				
			}

			return nMaxUid;
		}
		private bool VerifyModuleIndex(int nIndex)
		{
			bool bRet = false;

			if (Modules.Count > nIndex && nIndex >= 0)
			{
				bRet = true;
			}

			return bRet;
		}

		public IOModule GetIOModule(int nIndex)
		{
			IOModule module = null;
			if (VerifyModuleIndex(nIndex))
			{
				module = Modules[nIndex];
			}

			return module;
		}
		

		public IOModule GetIOModule(uint nNo)
		{
			IOModule returnValue = null;
			foreach (IOModule module in Modules)
			{
				if (module.No == nNo)
				{
					returnValue = module;
					break;
				}
			}
			return returnValue;
		}

	}

	[Serializable]
	public class IOBoadConfiguration
    {
		public string ComponentUid { set; get; }

		public int No { set; get; }

		public bool Simulated { set; get; }

		public int ModuleCount { set; get; }


		public IOBoadConfiguration()
		{
			SetDefaultValues();
		}
		protected virtual void SetDefaultValues()
		{
			ComponentUid = "";
			No = 0;
			Simulated = false;
		}
		public int Save(FileStream fs)
		{
			int ret = 0;

			if ((ret = SaveManager.BinarySerialize(fs, this)) != 0) return ret;

			return ret;
		}


	}
	public class IOBoardConfigurationColletion : Collection<IOBoadConfiguration>
	{

	}

}
