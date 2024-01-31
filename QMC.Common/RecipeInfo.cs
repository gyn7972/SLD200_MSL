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
    public class RecipeInfo
    {
        private uint m_nNo;
        private string m_strName;
        private string m_strDescription;
        private DateTime m_dtEditTime;
        private string m_strEditedBY;

        private object m_lockRecipeParam;
        private Dictionary<string, RecipeParameters> m_dicRecipes;
        

        public RecipeInfo() : this(0, "", "")
        {
            m_lockRecipeParam = new object();
            m_dicRecipes = new Dictionary<string, RecipeParameters>();
        }

        public RecipeInfo(uint no, string name, string description)
        {
            m_nNo = no;
            m_strName = name;
            m_strDescription = description;
            m_dtEditTime = DateTime.Now;
            m_strEditedBY = string.Empty;
        }

        public string strEditTime()
        {
            string time = string.Empty;

            time = m_dtEditTime.ToString();

            return time;
        }

        public uint No
        {
            get { return m_nNo; }
            set { m_nNo = value; }
        }

        public string Name
        {
            get { return m_strName; }
            set { m_strName = value; }
        }

        public string Description
        {
            get { return m_strDescription; }
            set { m_strDescription = value; }
        }

        public DateTime EditTime
        {
            get { return m_dtEditTime; }
        }

        public string EditBy
        {
            get { return m_strEditedBY; }
            set { m_strEditedBY = value; }
        }
        public RecipeInfo DeepCopy()
        {
            RecipeInfo info = new RecipeInfo();
            info.No = this.No;
            info.Name = this.Name;
            info.Description = this.Description;
            info.EditBy = this.EditBy;
            info.m_dtEditTime = DateTime.Now;
            return info;
        }

        public void ApplyRecipeData(Module module)
        {
            if (m_dicRecipes != null && m_dicRecipes.ContainsKey(module.Name))
            {
                module.SetRecipeData(m_dicRecipes[module.Name].RecipeData);
            }
        }

        public void SaveRecipeData(Module module)
        {            
             if (m_dicRecipes != null)
            {
                if(m_dicRecipes.ContainsKey(module.Name))
                {
                    m_dicRecipes[module.Name].RecipeData = module.GetRecipeData();
                }                    
                else
                {
                    RecipeParameters parameters = new RecipeParameters();
                    parameters.Module = module.Name;
                    parameters.RecipeData = module.GetRecipeData();
                    m_dicRecipes.Add(module.Name, parameters);
                }
                    
            }
            
        }

        public void Add(string strModuleName, object recipeData)
        {
            if(m_dicRecipes.ContainsKey(strModuleName))
            {
                m_dicRecipes[strModuleName].RecipeData = recipeData;
            }
            else
            {
                m_dicRecipes.Add(strModuleName, new RecipeParameters(strModuleName, recipeData));
            }
        }
    }
    [Serializable]
    public class RecipeHeader
    {
        public string CurrentRecipeName { get; set; }
        public List<string> RecipeList { set; get; }

        public RecipeHeader()
        {
            RecipeList = new List<string>();
        }
    }
    [Serializable]
    public class RecipeInfoCollection : Collection<RecipeInfo>
    {
        private RecipeHeader m_Header;
        public RecipeHeader Header 
        { 
            get
            {
                m_Header.RecipeList.Clear();
                foreach(RecipeInfo recipeInfo in this)
                {
                    m_Header.RecipeList.Add(recipeInfo.Name);
                }
                return m_Header;
            }
            set
            {
                m_Header = value;
            }
        }

        public RecipeInfoCollection()
        {
            m_Header = new RecipeHeader();
        }
    }
}
