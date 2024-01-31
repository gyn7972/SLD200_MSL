using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    [Serializable]
    [TypeConverterAttribute(typeof(BehaviorStateConverter))]
    public class BehaviorState 
    {
        #region Field
        private string m_strName;
        private int m_nCode;
        #endregion
        
        #region Property
        public string Name 
        { 
            set
            {
                if(!ReadOnly)
                {
                    m_strName = value;
                }
            }
            get
            {
                return m_strName;
            }
        }
        public int Code
        {
            set
            {
                if (!ReadOnly)
                {
                    m_nCode = value;
                }
            }
            get
            {
                return m_nCode;
            }
        }
        [Browsable(false)]
        public bool ReadOnly { set; get; }
        #endregion

        #region Constructor
        public BehaviorState() : this("Empty", 0)
        {
        }

        public BehaviorState(string strName, int code)
        {
            Name = strName;
            Code = code;
            ReadOnly = false;
        }
        #endregion

        #region Method

        public override string ToString()
        {
            string strValue;
            strValue = string.Format("{0}({1})", this.Name, this.Code);
            return base.ToString();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion

    }

    [Serializable]
    public class BehaviorStateCollection : Collection<BehaviorState>
    {
        public BehaviorStateCollection()
        {
            SetDefaultValue();
        }
        private void SetDefaultValue()
        {
            {
                BehaviorState state = new BehaviorState("Idle", 0);
                state.ReadOnly = true;
                Add(state);
            }
            {
                BehaviorState state = new BehaviorState("Alarm", -1);
                state.ReadOnly = true;
                Add(state);
            }
            {
                BehaviorState state = new BehaviorState("Ready", 1);
                state.ReadOnly = true;
                Add(state);
            }
            {
                BehaviorState state = new BehaviorState("Running", 2);
                Add(state);
            }
        }

        protected override void RemoveItem(int index)
        {
            if(index < this.Count && index >= 0)
            {
                BehaviorState state = this[index];
                if(!state.ReadOnly)
                {
                    base.RemoveItem(index);
                }
            }
        }


        protected override void SetItem(int index, BehaviorState item)
        {
            if(index >= 0 && index < this.Count)
            {
                if (!this[index].ReadOnly)
                {
                    base.SetItem(index, item);
                }
            }
        }
    }

    internal class BehaviorStateConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(String))
                return true;
            return base.CanConvertTo(context, destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(String))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                string s = value as string;
                string[] token;
                token = s.Split(',');
                BehaviorState BehaviorState = new BehaviorState();
                BehaviorState.Name = token[0];
                BehaviorState.Code = int.Parse(token[1]);
                return BehaviorState;
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(String) && value is BehaviorState)
            {
                BehaviorState BehaviorState = value as BehaviorState;
                return string.Format("{0}, {1}",
                    BehaviorState.Name,
                    BehaviorState.Code
                    );
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
