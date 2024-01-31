using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    public enum ExecuteType
    {
        BeforeExecute,
        AfterExecute,
    }
    [Serializable]
    public class SequenceItem
    {
        public string Name { set; get; }
        public ExecuteType ExecuteType { set; get; }

        public SequenceItem() : this("", ExecuteType.AfterExecute)
        {
        }

        public SequenceItem(string strName) : this(strName, ExecuteType.AfterExecute)
        {
        }

        public SequenceItem(string strName, ExecuteType type)
        {
            Name = strName;
            ExecuteType = type;
        }
    }

    [Serializable]
    public class InitializeSequence
    {
        [Serializable]
        [TypeConverter(typeof(ExecuteConditionConverter))]
        public class ExecuteCondition
        {
            public string Name { set; get; }
            public string Sub { set; get; }
            public ExecuteType ExecuteType { set; get; }
            public ExecuteCondition() : this("", "", ExecuteType.AfterExecute)
            {
            }

            public ExecuteCondition(string strName, string strSub, ExecuteType executeType)
            {
                Name = strName;
                Sub = strSub;
                ExecuteType = executeType;
            }
        }
        public string Owner { set; get; }
        public ExecuteCondition Condition { set; get; }
        [Browsable(false)]
        public List<SequenceItem> Sequence { set; get; }

        public InitializeSequence() : this("")
        {

        }

        public InitializeSequence(string strOwner)
        {
            Owner = strOwner;
            Sequence = new List<SequenceItem>();
            Condition = new ExecuteCondition();
        }

        public List<SequenceItem> DeepCopySequenceList()
        {
            List<SequenceItem> listRet = new List<SequenceItem>();

            foreach (SequenceItem item in Sequence)
            {
                listRet.Add(item);
            }

            return listRet;
        }
    }

    [Serializable]
    public class InitializeSequenceCollection : Collection<InitializeSequence>
    {

    }

    internal class ExecuteConditionConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                string text = value as string;
                string[] array = text.Split(',');
                InitializeSequence.ExecuteCondition Condition = new InitializeSequence.ExecuteCondition();
                Condition.Name = array[0];
                Condition.Sub = array[1];
                Condition.ExecuteType = (ExecuteType)Enum.Parse(typeof(ExecuteType), array[2]);

                return Condition;
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is InitializeSequence.ExecuteCondition)
            {
                InitializeSequence.ExecuteCondition Condition = value as InitializeSequence.ExecuteCondition;
                return string.Format("{0}, {1}, {2}",
                    Condition.Name,
                    Condition.Sub,
                    Condition.ExecuteType.ToString()
                    );
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
