using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QMC.Common.VisionPart;
using QMC.Common.Vision.EureSys;
using QMC.Common.Vision.Optics.Leesos;
using QMC.Core;
using QMC.Common;
using QMC.Common.Modules;
using QMC.Common.Vision;
using QMC.Common.Parts;
using System.Threading;
using MessageBox = System.Windows.Forms.MessageBox;
using static QMC.Common.Modules.WorkStage;

using SpiralLab.Sirius;

//using OpenTK;
//using OpenTK.Graphics.OpenGL;
//using SpiralLab.Sirius2;
//using SpiralLab.Sirius2.Laser;
//using SpiralLab.Sirius2.PowerMeter;
//using SpiralLab.Sirius2.Scanner;
//using SpiralLab.Sirius2.Scanner.Rtc;
//using SpiralLab.Sirius2.Winforms;
//using SpiralLab.Sirius2.Winforms.Entity;
//using SpiralLab.Sirius2.Winforms.Marker;
//using SpiralLab.Sirius2.Winforms.UI;

namespace SLD200_MSL
{
    public partial class FormRecipeChangeByComm : Form
    {
        static WorkStage workStage;



        public FormRecipeChangeByComm()
        {
            InitializeComponent();

            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;

            foreach (Module module in m_collectionModules)
            {
                //if (module.Name == "WorkStage")
                if (module.Name == "WorkStage")
                {
                    workStage = module as WorkStage;
                }
            }

            this.Load += FormRecipeChangeByComm_Load;
        }

        private void FormRecipeChangeByComm_Load(object sender, EventArgs e)
        {
            Recipe_CodeName_List_Load();
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public bool Recipe_CodeName_List_Load()
        {
            bool m_bRet = false;
            string strFIle = "";
            StringBuilder temp = new StringBuilder(255);

            strFIle = ConfigManager.GetConfigPath() + "\\Recipe CodeName List (Do not delete or modify).ini";

            RecipeInfoCollection recipes = DataManager.Instance.Recipe;

            dataGridView_RecipeCodeName.Rows.Clear();

            if (File.Exists(strFIle))
            {
                m_bRet = true;


                //  Recipe List를 읽어와서 ListBox에 표시 (CodeName 포함)
                for (int i = 0; i < recipes.Count; i++)
                {
                    NativeMethods.GetPrivateProfileString(recipes[i].Name, "CodeName", "", temp, 255, strFIle);

                    dataGridView_RecipeCodeName.Rows.Add(i + 1, recipes[i].Name, temp.ToString());
                }
            }
            else
            {
                //  Recipe List를 읽어와서 ListBox에 표시
                for (int i = 0; i < recipes.Count; i++)
                {
                    dataGridView_RecipeCodeName.Rows.Add(i + 1, recipes[i].Name, "");
                }
            }

            return m_bRet;
        }

        public void Recipe_CodeName_List_Save()
        {
            string strRecipeName = "";
            string strCodeName = "";

            string strFIle = "";
            strFIle = ConfigManager.GetConfigPath() + "\\Recipe CodeName List (Do not delete or modify).ini";

            if (File.Exists(strFIle) == false)
            {
                File.Create(strFIle);
                //return;
            }

            if (File.Exists(strFIle))
            {
                for(int i = 0; i < dataGridView_RecipeCodeName.Rows.Count - 1; i++ )
                {
                    strRecipeName = dataGridView_RecipeCodeName.Rows[i].Cells[1].Value.ToString();
                    strCodeName = dataGridView_RecipeCodeName.Rows[i].Cells[2].Value.ToString();

                    NativeMethods.WritePrivateProfileString(strRecipeName, "CodeName", strCodeName, strFIle);
                }

                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "저장하였습니다.");
                return;
            }
        }

        private void btn_ListSave_Click(object sender, EventArgs e)
        {
            Recipe_CodeName_List_Save();
        }

        private void btn_Reload_Click(object sender, EventArgs e)
        {
            Recipe_CodeName_List_Load(); 
        }
    }
}
