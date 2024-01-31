using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    public class MaterialInfo
    {
        #region Property
        public string Name { set; get; }
        public int No { set; get; }

        // key : ActionParameter의 키로 ActorName_ParamterName 으로 사용.
        public Dictionary<string, SettingParameterCollection> Results { set; get; }
        #endregion

        #region Constructor
        public MaterialInfo() : this(DateTime.Now.ToString("yyyyMMddhhmmssfff"))
        {

        }
        public MaterialInfo(string strName)
        {
            Name = strName;
            Results = new Dictionary<string, SettingParameterCollection>();
        }
        #endregion

        public override string ToString()
        {
            return string.Format("Material [{0} : {1}]", No, Name);
        }

        public SettingParameterCollection GetResultData(string strResultID)
        {
            SettingParameterCollection parameters = null;
            if (Results.ContainsKey(strResultID))
            {
                parameters = Results[strResultID];
            }

            return parameters;
        }

        public void SetResultData(string strResultID, SettingParameterCollection parameters)
        {
            if (Results.ContainsKey(strResultID))
            {
                Results[strResultID] = parameters;
            }
            else
            {
                Results.Add(strResultID, parameters);
            }
        }
    }

    public class MaterialInfoCollection : Collection<MaterialInfo>
    {
    }

}
