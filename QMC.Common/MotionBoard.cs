using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    [Serializable]
    public abstract class MotionBoard
    {
        #region Field
        protected uint m_nLastUid;
        #endregion

        #region Property
        protected Dictionary<uint, MotionAxis> MotionTable { set; get; }
        protected List<MotionAxis> Axes { set; get; }

        public virtual MotionBoardConfiguration Configuration { set; get; }

        public bool IsOpen { get; protected set; }

        public bool Simulated => Configuration.Simulated;
        public string Name => Configuration.Name;
        public string Description => Configuration.Description;
        #endregion

        #region Constructor
        public MotionBoard()
        {
            Axes = new List<MotionAxis>();
            Configuration = new MotionBoardConfiguration();
            MotionTable = new Dictionary<uint, MotionAxis>();
            m_nLastUid = 0;
        }
        #endregion

        #region Method
        public int Close()
        {
            int result = 0;
            //if (!IsOpen)
            //{
            //	return result;
            //}
            if (!Simulated)
            {
                if ((result = OnClose()) != 0)
                {
                    return result;
                }
            }

            Equipment.AjinBoard_Opened = false;
            IsOpen = false;

            return result;
        }


        protected abstract int OnClose();


        public int Open()
        {
            int result = 0;
            if (IsOpen)
            {
                return result;
            }
            if (!Simulated)
            {
                if ((result = OnOpen()) != 0)
                {
                    return result;
                }
            }

            foreach (MotionAxis axis in Axes)
            {
                axis.Open();
                axis.SetEnable(true);
            }

            Equipment.AjinBoard_Opened = true;
            IsOpen = true;

            return result;
        }

        protected abstract int OnOpen();

        public int Save(FileStream fs)
        {
            int ret = 0;
            this.Configuration.AxisCount = this.Axes.Count;
            if ((ret = this.Configuration.Save(fs)) != 0) return ret;

            foreach (MotionAxis axis in Axes)
            {
                if ((ret = axis.Save(fs)) != 0) return ret;
            }

            return ret;
        }

        public abstract int Load(FileStream fs);

        public abstract int Load(MotionBoardConfiguration configuration, FileStream fs);

        public int AddAxis(MotionAxis axis)
        {
            int ret = 0;
            if (!MotionTable.ContainsKey((uint)axis.No))
            {
                Axes.Add(axis);
                MotionTable.Add((uint)axis.No, axis);
                m_nLastUid++;
            }
            else
            {
                ret = -1;
            }


            return ret;
        }

        public MotionAxis GetAxis(int nIndex)
        {
            MotionAxis axis = null;
            if (VerifyAxisIndex(nIndex))
            {
                axis = Axes[nIndex];
            }

            return axis;
        }

        public MotionAxis GetAxis(uint nUID)
        {
            MotionAxis axis = null;
            if (MotionTable.ContainsKey(nUID))
            {
                axis = MotionTable[nUID];
            }

            return axis;
        }

        private bool VerifyAxisIndex(int nIndex)
        {
            bool bRet = false;

            if (Axes.Count > nIndex && nIndex >= 0)
            {
                bRet = true;
            }

            return bRet;
        }

        public void ClearAxis()
        {
            Axes.Clear();
        }

        public int GetAxisCount()
        {
            return Axes.Count;
        }

        public uint GetMaxAxisUid()
        {
            uint nMaxUid = 0;
            foreach (MotionAxis axis in Axes)
            {
                if (nMaxUid < axis.UID)
                    nMaxUid = axis.UID;
            }

            return nMaxUid;
        }

        public int RemoveAtAxis(int nIndex)
        {
            int ret = 0;

            if (nIndex < Axes.Count && nIndex >= 0)
            {
                Axes.RemoveAt(nIndex);
            }

            return ret;
        }

        #endregion
    }
    [Serializable]
    public class MotionBoardConfiguration
    {
        #region Field
        private IPAddress m_IPAddress;
        #endregion

        public bool Simulated { set; get; }
        public int No { set; get; }
        //public int AxisNo { set; get; }
        public int AxisCount { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public MotionBoardType BoardType { set; get; }

        [Category("ACS")]
        [TypeConverter(typeof(Converter))]
        public IPAddress Address
        {
            get { return m_IPAddress; }
            set { m_IPAddress = value; }
        }

        [Category("ACS")]
        public int Port { get; set; }

        public MotionBoardConfiguration()
        {
            SetDefaultValues();
        }

        protected virtual void SetDefaultValues()
        {
            No = 0;
            //AxisNo = 0;
            AxisCount = 0;
            Simulated = false;
            Name = "";
            Description = "";
            BoardType = MotionBoardType.Unknown;
            this.Address = null;
            this.Port = 0;
        }

        public int Save(FileStream fs)
        {
            int ret = 0;

            if ((ret = SaveManager.BinarySerialize(fs, this)) != 0) return ret;

            return ret;
        }

        public static int Load(FileStream fs, out MotionBoardConfiguration configuration)
        {
            int ret = 0;
            //configuration = new MotionBoardConfiguration();

            if ((ret = SaveManager.BinaryDeserialize<MotionBoardConfiguration>(fs, out configuration)) != 0) return ret;

            return ret;
        }
    }
    public class MotionBoardConfigurationColletion : Collection<MotionBoardConfiguration>
    {

    }

    public class Converter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string)) return true;
            return base.CanConvertFrom(context, sourceType);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
                return IPAddress.Parse((string)value);
            return base.ConvertFrom(context, culture, value);
        }
    }
}
