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
using QMC.Common;
using QMC.Common.Modules;
using MessageBoxOk = QMC.Core.MessageBoxOk;
using MessageBoxYesNo = QMC.Core.MessageBoxYesNo;
using static QMC.Common.Modules.WorkStage;
using Point = System.Drawing.Point;
using Bitmap = System.Drawing.Bitmap;

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
    #region Define
    public enum RecipeButton
    {
        Equipment, Module
    }
    #endregion

    #region FormRecipeMain
    public partial class FormRecipeMain : FormContentBase
    {
        #region Field
        public RecipeInfo CurrentRecipe { get; set; }

        public SettingParameterCollection ActionParameters { get; set; }

        public RecipeParameters RecipeParameters { get; set; }
        public RecipeParametersCollection RecipeParameterCollection { get; set; }
        private Module m_Module;
        public ModuleCollection m_Modules;
        public FormRecipePart m_FormRecipePart;

        //public FormRecipeModule FormRecipeModule { get; set; }

        static WorkStage workStage;


        private Dictionary<Part, Form> m_dicSubForms;
        #endregion

        #region Property
        public FormModuleCollection m_FormModuleCollection { get; set; }
        #endregion

        #region Constructor
        public FormRecipeMain()
            : base()
        {
            InitializeComponent();
       
            this.BackColor = Color.FromArgb(38, 38, 38);

            m_FormModuleCollection = new FormModuleCollection();
            m_FormModuleCollection.selectedModule += ShowSelectedModule;
            m_FormModuleCollection.selectedVisionModule += ShowSelectedModule;

            m_Modules = Equipment.Modules;
           
            RecipeParameterCollection = new RecipeParametersCollection();
            CreateModuleButton();

            if (Equipment.GetCurrentRecipe() != null)
            {
                CurrentRecipe = Equipment.GetCurrentRecipe();
                this.baseTextBoxRecipeName.Text = Equipment.GetCurrentRecipe().Name;

                Equipment.LoadConfig(Equipment.GetCurrentRecipe().Name);
            }

            this.flowLayoutPanelButton.Location = new Point(0, Configuration.PanelbuttonSize.Height);
            this.panelContent.Location = new Point(0, Configuration.PanelbuttonSize.Height * 2);
            this.panelContent.Size = new Size(Configuration.MainSize.Width, Configuration.ContentSize.Height - Configuration.PanelSize.Height * 2);

            //this.baseButtonAssign.Location = new Point(500, Configuration.TopSize.Height);
            this.Controls.Add(this.baseTextBoxRecipeName);
            //this.Controls.Add(this.baseButtonAssign);
            this.Controls.Add(this.baseButtonList);
            this.Controls.Add(this.baseButtonLoad);
            this.Controls.Add(this.baseButtonSave);
            this.Controls.Add(this.baseLabelRecipe);
            this.baseLabelRecipe.Location = new Point(10, 5);
            this.baseTextBoxRecipeName.Location = new Point(this.baseLabelRecipe.Size.Width + this.baseLabelRecipe.Location.X, 3);
            this.baseTextBoxRecipeName.Font = new Font(this.baseTextBoxRecipeName.Font.FontFamily, 15);
            this.baseTextBoxRecipeName.Multiline = false;
            this.baseTextBoxRecipeName.TabStop = false;

            this.baseButtonSave.Location = new Point(Configuration.MainSize.Width - Configuration.ContentLocation.X - Configuration.ButtonSize.Width, 3);
            this.baseButtonLoad.Location = new Point(this.baseButtonSave.Location.X - Configuration.ButtonSize.Width - 3, 3);
            this.baseButtonList.Location = new Point(this.baseButtonLoad.Location.X - Configuration.ButtonSize.Width - 3, 3);
            //this.baseButtonAssign.Location = new Point(this.baseButtonList.Location.X - Configuration.ButtonSize.Width - 3, 3);

            
        }
        #endregion

        #region EventHandler

        public void Button_Click(object sender, EventArgs e)
        {
            BaseButton button = sender as BaseButton;
            if (button != null)
                ShowSelectedModule((Module)button.Tag);
        }

        //public void UpdateParameter(RecipeParameters recipe)
        //{
        //    RecipeParameterCollection.Add(recipe);
        //    CurrentRecipe.RecipeParameterCollection = RecipeParameterCollection;
        //}

        private void baseButtonSave_Click(object sender, EventArgs e)
        {
            if (CurrentRecipe != null)
            {
                Equipment.UpdateRecipeData();
            }
            Equipment.SetCurrentRecipe(CurrentRecipe);
            Equipment.SaveRecipe();
        }

        private void baseButtonLoad_Click(object sender, EventArgs e)
        {
            if (m_FormRecipePart != null)
            {
                this.m_FormRecipePart.Close();
            }
            RecipeInfoCollection recipes = DataManager.Instance.Recipe;
            foreach (RecipeInfo recipe in recipes)
            {
                if (recipe.Name == this.baseTextBoxRecipeName.Text)
                {
                    CurrentRecipe = recipe;
                }

            }
            //CurrentRecipe = Equipment.GetCurrentRecipe();
        }

        private void baseButtonAssign_Click(object sender, EventArgs e)
        {
            Equipment.SetCurrentRecipe(CurrentRecipe);
        }

        private void baseButtonList_Click(object sender, EventArgs e)
        {
            string m_strBeforeRecipe = null;
            string m_strTemp = null;

            int m_nWaferImageOffset_X = 0;
            int m_nWaferImageOffset_Y = 0;

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

            m_strBeforeRecipe = CurrentRecipe.Name;                                                            //  현재 레시피

            RecipeInfoCollection recipes = DataManager.Instance.Recipe;
            FormRecipeList formRecipeList = new FormRecipeList(recipes);
            formRecipeList.BringToFront();
            formRecipeList.StartPosition = FormStartPosition.CenterScreen;
            if (formRecipeList.ShowDialog() == DialogResult.OK)
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Recipe Form, Button Click", "레시피 선택 완료");

                workStage.m_nReticleCheck_Step_forALIGN = (int)ReticleCheck_Step.None;                //  프로그램 시작 시, 레시피 변경 시 레티클 확인 Step 초기화 

                CurrentRecipe = formRecipeList.m_recipe;
                OnChangedCurrentRecipe();
                this.baseTextBoxRecipeName.Text = CurrentRecipe.Name;

                //  Config 파라미터 로드
                Equipment.LoadConfig(CurrentRecipe.Name);
                DataManager.Instance.ApplyConfigData(workStage);
                DataManager.Instance.UpdateConfigData(workStage);

                //  Config 화면에 데이터가 갱신되도록 하기 위해서. (각 Form 의 Timer 가 1초에 한번씩 이 변수를 체크해서 갱신해준다.) 크게 부하 받지는 않으니.... 꼼수..
                Equipment.m_bRedraw_FormWorkStageParameterConfig = true;

                if (m_FormRecipePart != null)
                {
                    m_FormRecipePart.Close();
                }
                //FormRecipePart = new FormRecipePart(Modules[i], CurrentRecipe);
                //FormRecipePart.parameterDelegate += UpdateParameter;
                ////   FormRecipePart = new FormRecipePart(Module);
                //ShowForm(FormRecipePart);

                if (CurrentRecipe != null)
                {
                    Equipment.UpdateRecipeData();
                }
                Equipment.SetCurrentRecipe(CurrentRecipe);
                Equipment.SaveRecipe();

                //workStage.m_bHighResCam_AlignPattern_Reset = true;
                //workStage.m_bLowResCam_AlignPattern_Reset = true;
                //workStage.m_bHighResCam_ReticleAlignPattern_Reset = true;
                //workStage.m_bLowResCam_ReticleAlignPattern_Reset = true;


                workStage.Machine_Parameter_Load();


                //  패턴 매칭 Train Image 설정
                string m_strFile = "";

                //workStage.m_bHighResCam_AlignPattern_Reset = false;
                //workStage.m_bLowResCam_AlignPattern_Reset = false;
                //workStage.m_bHighResCam_ReticleAlignPattern_Reset = false;
                //workStage.m_bLowResCam_ReticleAlignPattern_Reset = false;

                //  PAK
                m_strFile = string.Format("{0}\\{1}_PAK.jpg", ConfigManager.GetPatternImagePath(), CurrentRecipe.Name);
                if (File.Exists(m_strFile))
                {
                    if (workStage.jigAligner_HighRes != null)
                    {
                        workStage.jigAligner_HighRes.Recipe.PatternMatchingParameter.TrainImage = Bitmap.FromFile(m_strFile);

                        workStage.PatternMatchingImage_Loaded_HighRes = true;
                    }
                }
                else
                {
                    if (workStage.jigAligner_HighRes != null)
                    {
                        m_strFile = string.Format("{0}\\NoImage.jpg", ConfigManager.GetPatternImagePath());
                        if (File.Exists(m_strFile))
                        {
                            workStage.jigAligner_HighRes.Recipe.PatternMatchingParameter.TrainImage = Bitmap.FromFile(m_strFile);
                        }
                    }
                }

                //  Wafer
                m_strFile = string.Format("{0}\\{1}_Wafer.jpg", ConfigManager.GetPatternImagePath(), CurrentRecipe.Name);
                if (File.Exists(m_strFile))
                {
                    if (workStage.jigAligner_LowRes != null)
                    {
                        workStage.jigAligner_LowRes.Recipe.PatternMatchingParameter.TrainImage = Bitmap.FromFile(m_strFile);

                        workStage.PatternMatchingImage_Loaded_LowRes = true;
                    }
                }
                else
                {
                    if (workStage.jigAligner_LowRes != null)
                    {
                        m_strFile = string.Format("{0}\\NoImage.jpg", ConfigManager.GetPatternImagePath());
                        if (File.Exists(m_strFile))
                        {
                            workStage.jigAligner_LowRes.Recipe.PatternMatchingParameter.TrainImage = Bitmap.FromFile(m_strFile);
                        }
                    }
                }

                //  Reticle Upper
                m_strFile = string.Format("{0}\\Reticle_HighRes.jpg", ConfigManager.GetPatternImagePath());
                if (File.Exists(m_strFile))
                {
                    if (workStage.reticleAligner_HighRes != null)
                    {
                        workStage.reticleAligner_HighRes.Recipe.PatternMatchingParameter.TrainImage = Bitmap.FromFile(m_strFile);

                        workStage.PatternMatchingImage_Reticle_Loaded_HighRes = true;
                    }
                }
                else
                {
                    if (workStage.reticleAligner_HighRes != null)
                    {
                        m_strFile = string.Format("{0}\\NoImage.jpg", ConfigManager.GetPatternImagePath());
                        if (File.Exists(m_strFile))
                        {
                            workStage.reticleAligner_HighRes.Recipe.PatternMatchingParameter.TrainImage = Bitmap.FromFile(m_strFile);
                        }
                    }
                }

                //  Reticle Lower
                m_strFile = string.Format("{0}\\Reticle_LowRes.jpg", ConfigManager.GetPatternImagePath());
                if (File.Exists(m_strFile))
                {
                    if (workStage.reticleAligner_LowRes != null)
                    {
                        workStage.reticleAligner_LowRes.Recipe.PatternMatchingParameter.TrainImage = Bitmap.FromFile(m_strFile);

                        workStage.PatternMatchingImage_Reticle_Loaded_LowRes = true;
                    }
                }
                else
                {
                    if (workStage.reticleAligner_LowRes != null)
                    {
                        m_strFile = string.Format("{0}\\NoImage.jpg", ConfigManager.GetPatternImagePath());
                        if (File.Exists(m_strFile))
                        {
                            workStage.reticleAligner_LowRes.Recipe.PatternMatchingParameter.TrainImage = Bitmap.FromFile(m_strFile);
                        }
                    }
                }


                m_strTemp = string.Format("레시피 변경.    [변경 전 : \"{0}\",   변경 후 : \"{1}\"]",
                                            m_strBeforeRecipe, CurrentRecipe.Name);

                Log.Write("CWA150SA", Equipment.User_Name, "Recipe Form, Button Click", m_strTemp);


                //  오래된 Backup 파일 & 폴더 삭제 (3일 이전)
                workStage.Delete_Backup(@"C:\Program Files\QMC\SLD200_MSL\Config\Backup");
                workStage.Delete_Backup(@"C:\Program Files\QMC\SLD200_MSL\Recipe\Backup");

                workStage.m_bParameterSetting_PosData_Reload = true;

                //  레시피 변경 시 Offset 재설정
                m_nWaferImageOffset_X = workStage.Config.ParamConfig.ReticleGlass_WaferVision_Offset_X;
                m_nWaferImageOffset_Y = workStage.Config.ParamConfig.ReticleGlass_WaferVision_Offset_Y;

                m_strTemp = string.Format("설정값, 웨이퍼 카메라 Offset X : {0:0.000}, Offset Y: {1:0.000}",
                                        workStage.Config.ParamConfig.ReticleGlass_WaferVision_Offset_X,
                                        workStage.Config.ParamConfig.ReticleGlass_WaferVision_Offset_Y);
                Log.Write("CWA150SA", Equipment.User_Name, "레시피 변경 창", m_strTemp);


                //public int MAX_IMAGE_WIDTH = 2248;            //  현장에서 조정된 Size (Center Offset X : 100, Offset Y : 84)
                //public int MAX_IMAGE_HEIGHT = 1880;
                if ((workStage.Camera_LowRes != null) &&
                    ((m_nWaferImageOffset_X > 0) && (m_nWaferImageOffset_Y > 0)))             //  Wafer Image Offset 값 모두 0 이상일 때만 적용
                {
                    workStage.Camera_LowRes.MyConfig.OffsetX = (uint)workStage.Config.ParamConfig.ReticleGlass_WaferVision_Offset_X;
                    workStage.Camera_LowRes.MyConfig.OffsetY = (uint)workStage.Config.ParamConfig.ReticleGlass_WaferVision_Offset_Y;

                    workStage.Camera_LowRes.OffsetMove((uint)m_nWaferImageOffset_X, (uint)m_nWaferImageOffset_Y);

                    m_strTemp = string.Format("웨이퍼 카메라 Offset 설정 후 X : {0}, Y: {1}, 설정 파일 X : {2}, Y: {3}",
                                            workStage.Camera_LowRes.GetOffsetX(),
                                            workStage.Camera_LowRes.GetOffsetY(),
                                            workStage.Config.ParamConfig.ReticleGlass_WaferVision_Offset_X,
                                            workStage.Config.ParamConfig.ReticleGlass_WaferVision_Offset_Y);
                    Log.Write("CWA150SA", Equipment.User_Name, "레시피 변경 창", m_strTemp);
                }
                else
                {
                    Log.Write("CWA150SA", Equipment.User_Name, "레시피 변경 창", "카메라 Offset 설정 실패 (카메라가 null 이거나, Offset 값이 0)");

                    var mb1 = new MessageBoxOk();
                    mb1.ShowDialog("Information !", "Wafer 카메라 - 이미지 Offset 값이 0 입니다.     (0 < X < 200, 0 < Y < 168)\r\n\r\n[ Reticle Glass Center 조정 필요 ]");
                    return;
                }

                if (workStage.Config.ParamConfig.ReticleGlass_CenterCheck_forAlign)
                {
                    var mb1 = new MessageBoxOk();
                    mb1.ShowDialog("Information !", "레티클 글래스 센터 확인 작업을 진행해야 합니다.\r\n\r\n###   PAK 카드를 제거하세요.!!   ###");
                    return;
                }
            }
            else
            {
                Log.Write("CWA150SA", Equipment.User_Name, "Recipe Form, Button Click", "레시피 선택 취소");
            }
        }
        #endregion

        #region Method
        public void ShowSelectedModule(Module module)
        {
            m_Module = module;
            FormModuleRecipe formModule = new FormModuleRecipe(module);
            ShowSelectedModuleForm(formModule);
        }

        private void ShowSelectedModuleForm(Form form)
        {
            this.panelContent.Controls.Clear();
            form.TopLevel = false;
            this.panelContent.Controls.Add(form);
            form.BringToFront();
            form.Activate();
            form.Show();
        }

        public void CreateModuleButton()
        {
            this.flowLayoutPanelButton.Controls.Clear();
            BaseButton[] control = new BaseButton[m_Modules.Count];
            int locationX = Configuration.ButtonSize.Width;
            int locationY = Configuration.ButtonSize.Height;
            int i = 0;
            foreach (Module module in Equipment.Modules)
            {
                control[i] = new BaseButton();
                control[i].Parent = this;
                control[i].Font = new System.Drawing.Font("Tahoma", 9.0F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
                control[i].Size = new Size(Configuration.ButtonSize.Width, Configuration.ButtonSize.Height);
                control[i].Name = module.Name;
                control[i].Text = module.Name;
                control[i].Click += Button_Click;
                control[i].Tag = module;
                
                this.flowLayoutPanelButton.Controls.Add(control[i]);
                locationX += Configuration.ButtonSize.Width;
                i++;
            }
            
        }

        private void OnChangedCurrentRecipe()
        {
            Equipment.SetCurrentRecipe(CurrentRecipe);
            FormManager.FireUpdateRecipeEvent();
        }
        #endregion
    }
    #endregion
}
