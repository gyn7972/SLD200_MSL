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
using static QMC.Common.Modules.WaferProbeAlign;

namespace CWA150SA_Onsemi300
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

        static WaferProbeAlign waferProbeAlign;


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

            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;

            foreach (Module module in m_collectionModules)
            {
                if (module.Name == "WaferProbeAlign")
                {
                    waferProbeAlign = module as WaferProbeAlign;
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

                waferProbeAlign.m_nReticleCheck_Step_forALIGN = (int)ReticleCheck_Step.None;                //  프로그램 시작 시, 레시피 변경 시 레티클 확인 Step 초기화 

                CurrentRecipe = formRecipeList.m_recipe;
                OnChangedCurrentRecipe();
                this.baseTextBoxRecipeName.Text = CurrentRecipe.Name;

                //  Config 파라미터 로드
                Equipment.LoadConfig(CurrentRecipe.Name);
                DataManager.Instance.ApplyConfigData(waferProbeAlign);
                DataManager.Instance.UpdateConfigData(waferProbeAlign);

                //  Config 화면에 데이터가 갱신되도록 하기 위해서. (각 Form 의 Timer 가 1초에 한번씩 이 변수를 체크해서 갱신해준다.) 크게 부하 받지는 않으니.... 꼼수..
                Equipment.m_bRedraw_FormWaferProbeAlignParameterConfig = true;

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

                waferProbeAlign.m_bUpperCam_AlignPattern_Reset = true;
                waferProbeAlign.m_bLowerCam_AlignPattern_Reset = true;


                waferProbeAlign.Machine_Parameter_Load();


                //  패턴 매칭 Train Image 설정
                string m_strFile = "";

                waferProbeAlign.PatternMatchingImage_Loaded_Upper = false;
                waferProbeAlign.PatternMatchingImage_Loaded_Lower = false;

                //  PAK
                m_strFile = string.Format("{0}\\{1}_PAK.jpg", ConfigManager.GetPatternImagePath(), CurrentRecipe.Name);
                if (File.Exists(m_strFile))
                {
                    if (waferProbeAlign.jigAligner_Upper != null)
                    {
                        waferProbeAlign.jigAligner_Upper.Recipe.PatternMatchingParameter.TrainImage = Bitmap.FromFile(m_strFile);
                        //waferProbeAlign.jigAligner_Upper.Recipe.PatternMatchingParameter.TrainImage.Load(m_strFile, VisionImage.FileFilter.jpg);
                        //pictureBoxTrainImage_Upper.Image = waferProbeAlign.jigAligner_Upper.Recipe.PatternMatchingParameter.TrainImage.GetImage();

                        waferProbeAlign.PatternMatchingImage_Loaded_Upper = true;
                    }
                }
                else
                {
                    if (waferProbeAlign.jigAligner_Upper != null)
                    {
                        m_strFile = string.Format("{0}\\NoImage.jpg", ConfigManager.GetPatternImagePath());
                        if (File.Exists(m_strFile))
                        {
                            waferProbeAlign.jigAligner_Upper.Recipe.PatternMatchingParameter.TrainImage = Bitmap.FromFile(m_strFile);
                            //pictureBoxTrainImage_Upper.Image = waferProbeAlign.jigAligner_Upper.Recipe.PatternMatchingParameter.TrainImage.GetImage();
                        }
                    }
                }

                //  Wafer
                m_strFile = string.Format("{0}\\{1}_Wafer.jpg", ConfigManager.GetPatternImagePath(), CurrentRecipe.Name);
                if (File.Exists(m_strFile))
                {
                    if (waferProbeAlign.jigAligner_Lower != null)
                    {
                        waferProbeAlign.jigAligner_Lower.Recipe.PatternMatchingParameter.TrainImage = Bitmap.FromFile(m_strFile);
                        //pictureBoxTrainImage_Lower.Image = waferProbeAlign.jigAligner_Lower.Recipe.PatternMatchingParameter.TrainImage.GetImage();

                        waferProbeAlign.PatternMatchingImage_Loaded_Lower = true;
                    }
                }
                else
                {
                    if (waferProbeAlign.jigAligner_Lower != null)
                    {
                        m_strFile = string.Format("{0}\\NoImage.jpg", ConfigManager.GetPatternImagePath());
                        if (File.Exists(m_strFile))
                        {
                            waferProbeAlign.jigAligner_Lower.Recipe.PatternMatchingParameter.TrainImage = Bitmap.FromFile(m_strFile);
                            //pictureBoxTrainImage_Lower.Image = waferProbeAlign.jigAligner_Lower.Recipe.PatternMatchingParameter.TrainImage.GetImage();
                        }
                    }
                }


                m_strTemp = string.Format("레시피 변경.    [변경 전 : \"{0}\",   변경 후 : \"{1}\"]",
                                            m_strBeforeRecipe, CurrentRecipe.Name);

                Log.Write("CWA150SA", Equipment.User_Name, "Recipe Form, Button Click", m_strTemp);


                waferProbeAlign.m_bWafer_ThetaAlign_OK = false;
                waferProbeAlign.m_bWafer_XYAlign_OK = false;
                waferProbeAlign.m_bProbeCard_TiltCheck_OK = false;
                waferProbeAlign.m_bProbeCard_XYAlign_OK = false;

                waferProbeAlign.m_bProbeCard_XYAlign_ErrorCheck_OK = false;         //  Probe Card XY Align Error Check OK
                waferProbeAlign.m_bWafer_XYAlign_ErrorCheck_OK = false;             //  Wafer XY Align Error Check OK
                waferProbeAlign.m_bWaferProbeAlign_ErrorCheck_Complete = false;
                waferProbeAlign.m_bWaferProbeAlign_ErrorCheck_All_OK = false;

                waferProbeAlign.m_dWafer_ProbeCard_PackingPos_Axis_U = -1;                  //  패킹할 때의 UVW Stage 좌표 (언패킹 시 사용한다.)
                waferProbeAlign.m_dWafer_ProbeCard_PackingPos_Axis_V = -1;                  //  패킹할 때의 UVW Stage 좌표 (언패킹 시 사용한다.)
                waferProbeAlign.m_dWafer_ProbeCard_PackingPos_Axis_W = -1;                  //  패킹할 때의 UVW Stage 좌표 (언패킹 시 사용한다.)

                waferProbeAlign.m_dWafer_ProbeCard_AlignPos_Axis_U = -1;                    //  얼라인 완료되었을때 UVW Stage 좌표 (패킹 시 사용한다.)
                waferProbeAlign.m_dWafer_ProbeCard_AlignPos_Axis_V = -1;                    //  얼라인 완료되었을때 UVW Stage 좌표 (패킹 시 사용한다.)
                waferProbeAlign.m_dWafer_ProbeCard_AlignPos_Axis_W = -1;                    //  얼라인 완료되었을때 UVW Stage 좌표 (패킹 시 사용한다.)

                waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Count_Total = 3;              //  오차 확인 위치 총 개수
                waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Count = 0;                    //  오차 확인 위치 카운트 (3개,   0: TOP, 1: MID, 2: BOT)

                for (int nPos = 0; nPos < 3; nPos++)
                {
                    waferProbeAlign.AlignmentErrorCheck_Position[nPos].X = 0.0;
                    waferProbeAlign.AlignmentErrorCheck_Position[nPos].Y = 0.0;

                    waferProbeAlign.ManualPacking_OffsetPosition_UVW[nPos] = 0.0;

                    for (int nSide = 0; nSide < 2; nSide++)
                    {
                        waferProbeAlign.AlignmentErrorCheck_Status[nPos, nSide] = false;

                        waferProbeAlign.AlignmentErrorCheck_MarkPosition[nPos, nSide].X = 0.0;
                        waferProbeAlign.AlignmentErrorCheck_MarkPosition[nPos, nSide].Y = 0.0;
                    }
                }

                waferProbeAlign.m_bCalibrationMode_PosData_Reload = true;
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
