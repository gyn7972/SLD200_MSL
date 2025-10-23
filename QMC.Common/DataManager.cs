using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    public class DataManager
    {
        #region Singleton 
        private static DataManager g_DataManager;
        public static DataManager Instance
        {
            get
            {
                if (g_DataManager == null)
                    g_DataManager = new DataManager();
                return g_DataManager;
            }
        }
        #endregion

        #region Field
        protected ConfigParameters m_Config;
        protected RecipeInfoCollection m_Recipe;
        protected Dictionary<string, SettingParameterCollection> m_dicOffsetValues;
        protected object m_SyncRootOffsetValues;
        #endregion

        #region Property
        public ConfigParameters Config
        {
            get
            {
                return m_Config;
            }
            set
            {
                m_Config = value;
            }
        }

        public RecipeInfoCollection Recipe
        {
            get
            {
                return m_Recipe;
            }
            set
            {
                m_Recipe = value;
            }
        }

        #endregion

        public DataManager()
        { 
            m_Config = new ConfigParameters();
            m_Recipe = new RecipeInfoCollection();

            m_dicOffsetValues = new Dictionary<string, SettingParameterCollection>();
            m_SyncRootOffsetValues = new object();
        }

        public void SetOffsetData(string strKey, SettingParameterCollection parameters)
        {
            lock(m_SyncRootOffsetValues)
            {
                if (m_dicOffsetValues.ContainsKey(strKey))
                {
                    m_dicOffsetValues[strKey] = parameters;
                }
                else
                {
                    m_dicOffsetValues.Add(strKey, parameters);
                }
            }            
        }

        public void ApplyConfigData(Module module)
        {
            Config.ApplyData(module);
        }

        public void UpdateConfigData(Module module)
        {
            Config.UpdateData(module);
        }
        public void ApplyRecipeData(Module module)
        {
            //Recipe.ApplyData(module);
        }

        public SettingParameterCollection GetOffsetData(string strKey)
        {
            SettingParameterCollection parameters = null;

            lock(m_SyncRootOffsetValues)
            {
                if (m_dicOffsetValues.ContainsKey(strKey))
                {
                    parameters = m_dicOffsetValues[strKey];
                }
            }
            

            return parameters;
        }

        public void  UpdateConfigParameters(ModuleCollection modules)
        {
            foreach(Module module in modules)
            {
                ApplyConfigData(module);
                module.UpdateConfigData();
            }
        }


    }
    [Serializable]
    public class DataKey
    {
        public string ModuleName { set; get; }
        public string PartName { set; get; }
        public string FunctionName { set; get; }
        public string ActionName { set; get; }
        public DataKey()
        {
            ModuleName = "";
            PartName = "";
            FunctionName = "";
            ActionName = "";
        }

        public override int GetHashCode()
        {
            return ModuleName.GetHashCode() + PartName.GetHashCode() + FunctionName.GetHashCode() + ActionName.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            bool bRet = false;

            DataKey target = obj as DataKey;
            if (target != null)
            {
                if (this.ModuleName == target.ModuleName && this.PartName == target.PartName && this.FunctionName == target.FunctionName && this.ActionName == target.ActionName)
                    bRet = true;
            }


            return bRet;
        }
    }
    [Serializable]
    public class ConfigParameters  
    {
        private object m_SysncRoot;
        
        private Dictionary<string, object> m_dicParameters;
        public ConfigParameters()
        {
            m_SysncRoot = new object();
            m_dicParameters = new Dictionary<string, object>();
        }

        public void ApplyData(Module module)
        {
            lock (m_SysncRoot)
            {
                if (m_dicParameters.ContainsKey(module.Name))
                {
                    object configData = m_dicParameters[module.Name];
                    module.SetConfigData(configData);
                }
            }
        }

        public void UpdateData(Module module)
        {
            lock (m_SysncRoot)
            {
                object configData = module.GetConfigData();
                if (m_dicParameters.ContainsKey(module.Name))
                {
                    m_dicParameters[module.Name] = configData;
                }
                else
                {
                    m_dicParameters.Add(module.Name, configData);
                }
            }
        }

    }

}
