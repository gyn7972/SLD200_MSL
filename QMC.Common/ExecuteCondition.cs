using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    [Serializable]
    public enum ExecuteConditionType
    {
        Equal,
        GreaterThanOrEqual,
        LessThanOrEqual,

    }
    [Serializable]
    public class ExecuteCondition
    {
        public string Name { set; get; }
        public Part Part { set; get; }
        public ExecuteConditionType ConditionType { set; get; }
        public BehaviorState State { set; get; }
        public ExecuteCondition() : this("")
        {

        }

        public ExecuteCondition(string strName)
        {
            Name = strName;
            ConditionType = ExecuteConditionType.Equal;
            Part = null;
            State = null;
        }

        public bool IsExecutable
        {
            get
            {
                bool bRet = false;
                //if(ConditionType == ExecuteConditionType.Equal)
                //{
                //    if (Part.CurrentState.Code == State.Code)
                //        bRet = true;
                //}
                //else if(ConditionType == ExecuteConditionType.GreaterThanOrEqual)
                //{
                //    if (Part.CurrentState.Code >= State.Code)
                //        bRet = true;
                //}
                //else
                //{
                //    if (Part.CurrentState.Code <= State.Code)
                //        bRet = true;
                //}
                
                return bRet;
            }
        }

        public ExecuteCondition DeepCopy()
        {
            ExecuteCondition condition = new ExecuteCondition();
            condition.Name = Name;
            condition.Part = Part;
            condition.ConditionType = ConditionType;
            condition.State = State;
            return condition;
        }
    }

    [Serializable]
    public class ExecuteConditionCollection : Collection<ExecuteCondition>
    {
        public bool IsExecutable
        {
            get
            {
                bool bRet = true;

                foreach (ExecuteCondition condition in this)
                {
                    bRet &= condition.IsExecutable;
                }

                return bRet;
            }
        }
        public ExecuteConditionCollection DeepCopy()
        {
            ExecuteConditionCollection conditions = new ExecuteConditionCollection();

            foreach (ExecuteCondition condition in this)
            {
                conditions.Add(condition.DeepCopy());
            }

            return conditions;
        }
    }
}
