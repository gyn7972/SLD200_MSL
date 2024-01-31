using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    [Serializable]
    public enum InterlockType
    {
        BeforeExecute,
        Executing
    }
    [Serializable]
    public enum InterlockCondition
    {
        Equal,
        GreaterThanOrEqual,
        LessThanOrEqual,
    }
    [Serializable]
    public class Interlock
    {
        static uint m_nLastUid;

        private uint m_nUid;
        public uint Uid 
        { 
            get
            {
                return m_nUid;
            }
        }
        public IActor Target { set; get; }
        public InterlockCondition Condition { set; get; }
        public SettingParameter Value { set; get; }
        public InterlockType InterlockType { set; get; }

        public string TargetPartName { set; get; }
        public int SelectedFunctionIndex { set; get; }
        public int SelectedParameterIndex { set; get; }
        public Interlock()
        {
            m_nUid = m_nLastUid++;
            Target = null;
            Condition = InterlockCondition.Equal;
            Value = new SettingParameter();
            InterlockType = InterlockType.BeforeExecute;
        }
    }
    [Serializable]
    public class InterlockCollection : Collection<Interlock>
    {
     
    }
}
