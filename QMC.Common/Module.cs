using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QMC.Common
{
    [Serializable]
    public abstract class Module : Part
    {
        private Thread m_workThread;

        private bool m_bExit;

        protected List<Part> m_Parts;

        public bool HasRecipe { set; get; }


        [Browsable(false)]
        public List<Part> Parts
        { get { return m_Parts; } }
        public Module(string strName) : base(strName)
        {
            m_bExit = false;
            m_Parts = new List<Part>();
        }

        protected void OnMainProcedure()
        {
            while (true)
            {
                if (m_bExit)
                {
                    break;
                }
                if (OnRun() != 0) break;
                Thread.Sleep(1);
            }
        }
        protected virtual int OnRun()
        {
            int ret = 0;

            //  2022. 04. 13.  SCH : Module 별로 이 함수를 오버라이딩 하여 Cycle 동작.

            return ret;
        }

        /*public void Start()
        {
            m_workThread = new Thread(new ThreadStart(OnMainProcedure));
            m_workThread.Start();
        }*/
        public virtual void Start()
        {
            SetRunStatus(RunStatus.Run);
            //SetRunMode(RunMode.Auto);
            m_bExit = false;
            m_workThread = new Thread(new ThreadStart(OnMainProcedure));
            m_workThread.Start();
            //IsReady = true;
        }

        #region Auto Run

        public override void Stop()
        {
            m_bExit = true;
            foreach(Part part in m_Parts)
            {
                part.Stop();
            }
        }

        #endregion

        #region Part
        public override int Create()
        {
            int ret = base.Create();

            foreach(Part part in m_Parts)
            {
                if ((ret = part.Create()) != 0) break;
            }

            return ret;
        }

        public DioPointCollection GetAllDioPoints()
        {
            DioPointCollection dioPoints = new DioPointCollection();

            dioPoints.Add(m_dicDioPoints.Values.ToList());

            foreach(Part part in m_Parts)
            {
                dioPoints.Add(part.GetDioPointList());
            }

            return dioPoints;
        }

        public override void Close()
        {
            base.Close();
            foreach (Part part in m_Parts)
            {
                part.Close();
            }
        }
        public override void UpdateConfigData()
        {
            foreach (Part part in Parts)
            {
                part.UpdateConfigData();
            }
        }
        public override void UpdateRecipeData()
        {
            foreach (Part part in Parts)
            {
                part.UpdateRecipeData();
            }
        }

        #endregion
        public virtual List<IlluminationChannel> GetIlluminationChannel()
        {
            return null;
        }
        public virtual void SetConfigData(object parameter)
        {

        }

        public virtual object GetConfigData()
        {
            return null;
        }

        public virtual void SetRecipeData(object recipeData)
        {
            UpdateRecipeData();
        }

        public virtual object GetRecipeData()
        {
            return null;
        }

        public virtual object GetConfigData_2nd()           //  2022. 04. 25.  SCH : Inspection Vision 에 Rivet 과 Mount 2가지 비전을 하나의 클래스에서 하기 때문에, 
                                                                                //                      Vision 검사 위치를 각각 가져오도록 가상함수 하나 더 선언 하였음.
        {
            return null;
        }
        
        /*public void SaveConfigData()
        {
            DataManager.Instance.Config.UpdateData(this);
            Equipment.SaveConfig();
        }*/
        public virtual void SaveConfigData()
        {
            DataManager.Instance.Config.UpdateData(this);
            Equipment.SaveConfig();
        }

        /*public void SaveRecipeData()
        {
            RecipeInfo recipe = Equipment.GetCurrentRecipe();
            recipe.Add(this.Name, GetRecipeData());
            Equipment.SaveRecipe();
        }*/
        public virtual void SaveRecipeData()
        {
            RecipeInfo recipe = Equipment.GetCurrentRecipe();
            recipe.Add(this.Name, GetRecipeData());
            Equipment.SaveRecipe();
        }

        public virtual void CreateRecipe(RecipeInfo recipe)
        {

        }

        public override void SetRunStatus(RunStatus status)
        {
            base.SetRunStatus(status);
            foreach(Part part in Parts)
            {
                part.SetRunStatus(status);
            }
        }

        public abstract void SetModuleScale(double dScaleX, double dScaleY, double dXaxisT, double dYaxisT, bool bInvertedX, bool bInvertedY);
    }

    public class ModuleCollection : Collection<Module>
    {
        public List<string> GetModuleList()
        {
            List<string> listTitle = new List<string>();
            foreach (Module module in this)
            {
                listTitle.Add(module.Name);
            }

            return listTitle;
        }
    }
}
