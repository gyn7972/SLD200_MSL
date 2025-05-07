using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QMC.Common;
using QMC.Common.Modules;
using QMC.Common.Parts;
using QMC.Common.Vision;
using QMC.Common.Vision.Tools;
using QMC.Common.VisionPart;
using QMC.Core;
using QMC.Common.Hmi;
using QMC.Common.UI;
using SLD200_MSL;
using static OpenCvSharp.LineIterator;
using static QMC.Common.Vision.Tools.PatternMatchingResult;
using System.Threading;
using System.IO;

namespace SLD200.NewStyleForm.NewSubForm
{
    public partial class FormNewSub_Recipe_Vision : UserControl
    {
        private bool m_bFormVisible = false; // 실제 Show 상태 여부

        static WorkStage workStage;
        static JigAligner Owner;

        //static Vision vision;

        private int nMarkType = 0; //0:Cross, 1:Circle 등
        private int nSerchType = 0; //0:PatternMatching, 1:Blob, 2:CircleA 등
        public bool IsPixel { get; set; }

        #region Property
        public RoiVisionTool RoiTrain { get; set; }
        public RoiVisionTool RoiInspect { get; set; }
        public PatternMatchingParameters PatternMatchingParameter { set; get; }
        public BlobVisionToolParameter BlobParameter { get; set; }
        #endregion

        private SLD200_MSL.RoiListControl m_RoiListControl;

        private System.Windows.Forms.Timer RecipeVisionTimer;

        public FormNewSub_Recipe_Vision()
        {
            InitializeComponent();
            this.Load += FormNewSub_Recipe_Vision_Load; // 👈 여기서 Load 이벤트 연결

            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;
            foreach (Module module in m_collectionModules)
            {
                if (module.Name == "WorkStage")
                {
                    workStage = module as WorkStage;
                    Owner = workStage.jigAligner_LowRes;
                }
            }
        }

        private void FormNewSub_Recipe_Vision_Load(object sender, EventArgs e)
        {
            //GUI생성 완료 후 Data 및 Cintroller 업데이트!
            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;
            foreach (Module module in m_collectionModules)
            {
                if (module.Name == "WorkStage")
                {
                    workStage = module as WorkStage;
                    Owner = workStage.jigAligner_LowRes;
                }

                //if (module.Name == "Vision")
                //{
                //    vision = module as Vision;
                //}
            }

            RecipeVisionTimer = new System.Windows.Forms.Timer();
            RecipeVisionTimer.Interval = 200; // 200ms 간격으로 상태 확인
            RecipeVisionTimer.Tick += RecipeVisionTimer_Tick;
            RecipeVisionTimer.Start();

            if (this.ImageViewer_RecipeVision_highs.IsHandleCreated)
            {
                this.ImageViewer_RecipeVision_highs.SizeMode = PictureBoxSizeMode.CenterImage;
                this.ImageViewer_RecipeVision_highs.SuspendDisplay();
                this.ImageViewer_RecipeVision_highs.StopUpdateTask();

                this.ImageViewer_RecipeVision_highs.Camera = workStage.jigAligner_HighRes.Camera; //Owner.Camera;

                this.ImageViewer_RecipeVision_highs.ResumeDisplay();
                this.ImageViewer_RecipeVision_highs.StartUpdateTask();
            }

            if (this.ImageViewer_RecipeVision_Rows.IsHandleCreated)
            {
                this.ImageViewer_RecipeVision_Rows.SizeMode = PictureBoxSizeMode.CenterImage;
                this.ImageViewer_RecipeVision_Rows.SuspendDisplay();
                this.ImageViewer_RecipeVision_Rows.StopUpdateTask();

                this.ImageViewer_RecipeVision_Rows.Camera = Owner.Camera;

                this.ImageViewer_RecipeVision_Rows.ResumeDisplay();
                this.ImageViewer_RecipeVision_Rows.StartUpdateTask();
            }

            this.pictureBox_RecipeVision_TrainImage.BackColor = Color.SpringGreen;
            this.RoiTrain = Owner.GetTrainRoi();
            this.RoiInspect = Owner.GetInspectRoi();
            this.m_RoiListControl = new SLD200_MSL.RoiListControl(RoiTrain, RoiInspect, Owner.Camera.Resolution);
            this.m_RoiListControl.roiTrainButtonClick += RoiTrainButtonClick;
            this.m_RoiListControl.roiAlignButtonClick += RoiInspectButtonClick;
            this.m_RoiListControl.roiTrainSaveButtonClick += RoiTrainSaveButtonClick;
            this.m_RoiListControl.roiAlignSaveButtonClick += RoiInspectSaveButtonClick;

            PatternMatchingParameter = new PatternMatchingParameters();
            PatternMatchingParameter = Owner.Recipe.PatternMatchingParameter;

            this.hScrollBar_RecipeVision_Illuminator.ValueChanged += new System.EventHandler(this.hScrollBarIlluminator_ValueChanged);
            SetScroll(2);

            IsPixel = true;

            InitPatternMatchingParameter();

            InitializeJogButtons();

            Temp_Position_Load();

            radioButton_RecipeVision_Move_MoveMode_Fine.Checked = false;
            radioButton_RecipeVision_Move_MoveMode_Coarse.Checked = true;
            radioButton_RecipeVision_JogMove_Continuous.Checked = false;
            radioButton_RecipeVision_JogMove_Step.Checked = true;

            radioButton_RecipeVision_Circle.Checked = true;

        }

        private void RecipeVisionTimer_Tick(object sender, EventArgs e)
        {
            //this.ImageViewer_RecipeVision_highs.Camera = workStage.jigAligner_HighRes.Camera; //Owner.Camera;

            //  Work Stage Position
            label_RecipeVision_EncPosition_STAGE_X.Text = string.Format("{0:F3}", workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X));
            label_RecipeVision_EncPosition_STAGE_Y.Text = string.Format("{0:F3}", workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y));
            
            //  Scanner & Camera Position
            label_RecipeVision_EncPosition_SCANNER_Z.Text = string.Format("{0:F3}", workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Z));

            Motion_Status(); // 기존에 있던 리미트 감지 및 색상 갱신 함수 호출

        }

        public void OnShow()
        {
            if (m_bFormVisible)
                return;

            m_bFormVisible = true;

            this.ImageViewer_RecipeVision_highs.ResumeDisplay();
            this.ImageViewer_RecipeVision_highs.StartUpdateTask();

            this.ImageViewer_RecipeVision_Rows.ResumeDisplay();
            this.ImageViewer_RecipeVision_Rows.StartUpdateTask();

            this.RecipeVisionTimer.Start();
        }

        public void OnHide()
        {
            if (!m_bFormVisible)
                return;

            m_bFormVisible = false;

            this.ImageViewer_RecipeVision_highs.SuspendDisplay();
            this.ImageViewer_RecipeVision_highs.StopUpdateTask();

            this.ImageViewer_RecipeVision_Rows.SuspendDisplay();
            this.ImageViewer_RecipeVision_Rows.StopUpdateTask();

            this.RecipeVisionTimer.Stop();
        }

        private void InitPatternMatchingParameter()
        {
            if (Owner.Recipe != null)
            {
                VisionImage visionImage = Owner.Recipe.PatternMatchingParameter.TrainImage;
                if(visionImage != null)
                {
                    this.pictureBox_RecipeVision_TrainImage.Image = visionImage.GetImage();
                }
            }
            else
            {
                this.pictureBox_RecipeVision_TrainImage.Image = null;
            }

            if (PatternMatchingParameter != null)
            {
                basetextBox_RecipeVision_AngleTolerance.Text = Equipment.stVisionRecipeSet.PatternMatching.MaxTolerance.ToString();
                basetextBox_RecipeVision_MaxInstance.Text = Equipment.stVisionRecipeSet.PatternMatching.MaxInstance.ToString();
                basetextBox_RecipeVision_MinScore.Text = Equipment.stVisionRecipeSet.PatternMatching.MinScore.ToString();
                baseToggleButton_RecipeVision_DuplicateCheck.UpdateToggleStatus(Equipment.stVisionRecipeSet.PatternMatching.DuplicateChecked);
                baseToggleButton_RecipeVision_UseMaskImage.UpdateToggleStatus(Equipment.stVisionRecipeSet.PatternMatching.UseMaskImage);
                if (Equipment.stVisionRecipeSet.LoadTrainImage().GetImage() != null)
                {
                    pictureBox_RecipeVision_TrainImage.Image = Equipment.stVisionRecipeSet.LoadTrainImage().GetImage();
                }
                

                PatternMatchingParameter.MaxTolerance = Equipment.ToDouble(basetextBox_RecipeVision_AngleTolerance.Text);
                PatternMatchingParameter.MaxInstance = Equipment.ToInt(basetextBox_RecipeVision_MaxInstance.Text);
                PatternMatchingParameter.MinTolerance = Equipment.ToDouble(basetextBox_RecipeVision_AngleTolerance.Text) * -1;
                PatternMatchingParameter.MinScore = Equipment.ToDouble(basetextBox_RecipeVision_MinScore.Text);
                PatternMatchingParameter.DuplicateChecked = baseToggleButton_RecipeVision_DuplicateCheck.GetButtonStatus();
                PatternMatchingParameter.UseMaskImage = baseToggleButton_RecipeVision_UseMaskImage.GetButtonStatus();
                PatternMatchingParameter.TrainImage = pictureBox_RecipeVision_TrainImage.Image;
                Owner.Recipe.PatternMatchingParameter = PatternMatchingParameter;

                Owner.Recipe.InspectRoiStartLocation = Equipment.stVisionRecipeSet.InspectRoiStartLocation; //RoiInspect.Parameter.StartLocation;
                Owner.Recipe.InspectRoiEndLocation = Equipment.stVisionRecipeSet.InspectRoiEndLocation;     //RoiInspect.Parameter.EndLocation;
                Owner.Recipe.TrainRoiStartLocation = Equipment.stVisionRecipeSet.TrainRoiStartLocation;     //RoiTrain.Parameter.StartLocation;
                Owner.Recipe.TrainRoiEndLocation = Equipment.stVisionRecipeSet.TrainRoiEndLocation;         //RoiTrain.Parameter.EndLocation;

                //basetextBox_RecipeVision_AngleTolerance.Text = Owner.Recipe.PatternMatchingParameter.MaxTolerance.ToString();
                //PatternMatchingParameter.MaxTolerance = Owner.Recipe.PatternMatchingParameter.MaxTolerance;

                //basetextBox_RecipeVision_MaxInstance.Text = Owner.Recipe.PatternMatchingParameter.MaxInstance.ToString();
                //PatternMatchingParameter.MaxInstance = Owner.Recipe.PatternMatchingParameter.MaxInstance;

                //basetextBox_RecipeVision_MinScore.Text = Owner.Recipe.PatternMatchingParameter.MinScore.ToString();
                //PatternMatchingParameter.MinScore = Owner.Recipe.PatternMatchingParameter.MinScore;

                //bool bOn = Owner.Recipe.PatternMatchingParameter.DuplicateChecked;
                //baseToggleButton_RecipeVision_DuplicateCheck.UpdateToggleStatus(bOn);
                //PatternMatchingParameter.DuplicateChecked = bOn;

                //bOn = Owner.Recipe.PatternMatchingParameter.UseMaskImage;
                //baseToggleButton_RecipeVision_UseMaskImage.UpdateToggleStatus(bOn);
                //PatternMatchingParameter.UseMaskImage = bOn;
            }

            this.radioButton_RecipeVision_Pattern.Checked = true;   //무조건 무조건이야~
            this.radioButton_RecipeVision_Blob.Checked = false;
            this.radioButton_RecipeVision_Cross.Checked = false;
            this.radioButton_RecipeVision_Circle.Checked = true;

            this.radioButton_RecipeVision_Light_IR.Checked = true;
            this.radioButton_RecipeVision_Light_Red.Checked = false;
            //workStage.Config.ListIlluminationChannel[0].Value = Equipment.Scanner_Calibration_Illumination_channel_01_Value; //RED
            //workStage.Config.ListIlluminationChannel[1].Value = Equipment.Scanner_Calibration_Illumination_channel_02_Value; //IR

            this.radioButton_RecipeVision_Light_IR.Checked = true;
            this.radioButton_RecipeVision_Light_Red.Checked = false;
            //workStage.Config.ListIlluminationChannel[0].Value = Equipment.Scanner_Calibration_Illumination_channel_01_Value; //RED
            //workStage.Config.ListIlluminationChannel[1].Value = Equipment.Scanner_Calibration_Illumination_channel_02_Value; //IR



        }

        private void button_RecipeVision_CameraLive_Click(object sender, EventArgs e)
        {
            if (!Owner.Camera.Opened)
            {
                var mb1 = new QMC.Common.UI.MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 카메라를 연결해야 해야 합니다.");
                return;
            }

            ImageViewer_RecipeVision_Rows.Simulated = false;
            Owner.Simulated = false;

            this.ImageViewer_RecipeVision_Rows.StartUpdateTask();
            this.ImageViewer_RecipeVision_Rows.Visible = true;

            if (Owner.Camera != null)
            {
                Owner.Camera.StartLive();
            }
        }

        private void button_RecipeVision_CameraStop_Click(object sender, EventArgs e)
        {
            if (!Owner.Camera.Opened)
            {
                var mb1 = new QMC.Common.UI.MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 카메라를 연결해야 해야 합니다.");
                return;
            }

            this.ImageViewer_RecipeVision_Rows.StopUpdateTask();

            if (Owner.Camera != null)
            {
                Owner.Camera.StopLive();
            }
        }

        private void radioButton_RecipeVision_Cross_CheckedChanged(object sender, EventArgs e)
        {
            nMarkType = 0;  //Cross
        }

        private void radioButton_RecipeVision_Circle_CheckedChanged(object sender, EventArgs e)
        {
            nMarkType = 1;  //Circle
        }

        private void radioButton_RecipeVision_Pattern_CheckedChanged(object sender, EventArgs e)
        {
            nSerchType = 0; //PatternMatching
        }

        private void radioButton_RecipeVision_Blob_CheckedChanged(object sender, EventArgs e)
        {
            nSerchType = 1; //Blob
        }

        private void button_RecipeVision_Train_Click(object sender, EventArgs e)
        {
            JigAlignerRecipe recipe = Owner.Recipe; 
            RoiTrain.Parameter.StartLocation = recipe.TrainRoiStartLocation;
            RoiTrain.Parameter.EndLocation = recipe.TrainRoiEndLocation;

            RoiInspect.Parameter.Overlay.Visible = false;
            RoiTrain.Parameter.Overlay.Visible = true;

            PatternMatchingResult result = Owner.GetResult();
            this.ImageViewer_RecipeVision_Rows.NormalOverlays.Add(RoiTrain.Parameter.Overlay);
            foreach (var overlay in result.ResultOverlays)
            {
                this.ImageViewer_RecipeVision_Rows.NormalOverlays.Remove(overlay);
            }
            this.ImageViewer_RecipeVision_Rows.Display();

            m_RoiListControl.RoiTrainClickNew();
        }

        private void button_RecipeVision_Inspect_Click(object sender, EventArgs e)
        {
            JigAlignerRecipe recipe = Owner.Recipe;
            RoiInspect.Parameter.StartLocation = recipe.InspectRoiStartLocation;
            RoiInspect.Parameter.EndLocation = recipe.InspectRoiEndLocation;

            RoiTrain.Parameter.Overlay.Visible = false;
            RoiInspect.Parameter.Overlay.Visible = true;

            PatternMatchingResult result = Owner.GetResult();
            this.ImageViewer_RecipeVision_Rows.NormalOverlays.Add(RoiInspect.Parameter.Overlay);
            foreach (var overlay in result.ResultOverlays)
            {
                this.ImageViewer_RecipeVision_Rows.NormalOverlays.Remove(overlay);
            }
            this.ImageViewer_RecipeVision_Rows.Display();

            this.m_RoiListControl.RoiAlignClickNew();
        }

        private void RoiTrainButtonClick(RoiVisionTool roiVisionTool)
        {
            if (Owner != null)
            {
                RoiTrain.Parameter.CenterLocation = roiVisionTool.Parameter.CenterLocation;
                RoiTrain.Parameter.Size = roiVisionTool.Parameter.Size;

                this.ImageViewer_RecipeVision_Rows.NormalOverlays.Add(RoiTrain.Parameter.Overlay);
                RoiTrain.Parameter.Overlay.Visible = true;
                this.ImageViewer_RecipeVision_Rows.Display();

            }
        }

        private void RoiInspectButtonClick(RoiVisionTool roiVisionTool)
        {
            if (Owner != null)
            {
                RoiInspect.Parameter.CenterLocation = roiVisionTool.Parameter.CenterLocation;
                RoiInspect.Parameter.Size = roiVisionTool.Parameter.Size;

                this.ImageViewer_RecipeVision_Rows.NormalOverlays.Add(RoiInspect.Parameter.Overlay);
                RoiInspect.Parameter.Overlay.Visible = true;
                this.ImageViewer_RecipeVision_Rows.Display();
            }
        }

        private void RoiTrainSaveButtonClick(RoiVisionTool roiVisionTool, bool bOk)
        {
            JigAlignerRecipe recipe = Owner.Recipe;

            if (!bOk)
            {
                RoiTrain.Parameter.StartLocation = recipe.TrainRoiStartLocation;
                RoiTrain.Parameter.EndLocation = recipe.TrainRoiEndLocation;
                RoiTrain.Parameter.Overlay.Visible = false;
                this.ImageViewer_RecipeVision_Rows.Display();
                return;
            }

            RoiTrain = roiVisionTool;
            RoiTrain.Parameter.Overlay.Visible = false;

            recipe.TrainRoiStartLocation = RoiTrain.Parameter.StartLocation;
            recipe.TrainRoiEndLocation = RoiTrain.Parameter.EndLocation;

            //Box_Setup_ScannerCal_ImageViewer.Display();
            //if (m_JogControl != null)
            //{
            //    m_JogControl.Show();
            //    m_JogControl.BringToFront();
            //    m_TrainImageControl.Show();
            //    m_TrainImageControl.BringToFront();
            //}
            //SetTrainImage(Owner.Recipe.PatternMatchingParameter.TrainImage);



            this.ImageViewer_RecipeVision_Rows.Display();
        }

        private void SetTrainImage(VisionImage image)
        {
            this.pictureBox_RecipeVision_TrainImage.Image = image.GetImage();
            this.pictureBox_RecipeVision_TrainImage.BringToFront();
            this.pictureBox_RecipeVision_TrainImage.Refresh();
        }

        private void RoiInspectSaveButtonClick(RoiVisionTool roiVisionTool, bool bOk)
        {
            JigAlignerRecipe recipe = Owner.Recipe;
            if (!bOk)
            {
                RoiInspect.Parameter.StartLocation = recipe.InspectRoiStartLocation;
                RoiInspect.Parameter.EndLocation = recipe.InspectRoiEndLocation;
                this.ImageViewer_RecipeVision_Rows.Display();
                return;
            }
            RoiInspect = roiVisionTool;
            recipe.InspectRoiStartLocation = RoiInspect.Parameter.StartLocation;
            recipe.InspectRoiEndLocation = RoiInspect.Parameter.EndLocation;

            this.ImageViewer_RecipeVision_Rows.Display();


        }

        private void button_RecipeVision_Train_Set_Click(object sender, EventArgs e)
        {
            if (Owner != null)
            {
                RoiVisionTool roiVisionTool = m_RoiListControl.RoiTrainVisionTool;

                RoiTrain.Parameter.CenterLocation = roiVisionTool.Parameter.CenterLocation;
                RoiTrain.Parameter.Size = roiVisionTool.Parameter.Size;

                this.ImageViewer_RecipeVision_Rows.NormalOverlays.Add(RoiTrain.Parameter.Overlay);
                RoiTrain.Parameter.Overlay.Visible = true;
                this.ImageViewer_RecipeVision_Rows.Display();
            }

            string m_strFile = "";
            string m_strRecipe = "";
            RecipeInfo m_recipeInfo = new RecipeInfo();
            m_recipeInfo = Equipment.GetCurrentRecipe();

            Owner.Camera.LatestImage = this.ImageViewer_RecipeVision_Rows.InputImage;

            if (this.ImageViewer_RecipeVision_Rows.Simulated)
            {
                Owner.Simulated = true;
                Owner.TestImage = this.ImageViewer_RecipeVision_Rows.InputImage;
            }
            else
            {
                Owner.Simulated = false;
            }

            Owner.Train();
            SetTrainImage(Owner.TrainImage);

            //  train 할 때 레시피를 저장해줘야 정상적으로 패턴 이미지가 변경된다.
            if (m_recipeInfo != null)
            {
                Equipment.UpdateRecipeData();
            }
            Equipment.SetCurrentRecipe(m_recipeInfo);
            Equipment.SaveRecipe();
            FormManager.FireUpdateRecipeEvent();

            Equipment.ApplyRecipeData();

            if (Owner.Name == "JigAligner (Coarse)")
            {
                m_strFile = string.Format("{0}\\PreAlign.bmp", ConfigManager.GetPatternImagePath());
                Owner.TrainImage.Save(m_strFile, QMC.Common.Vision.VisionImage.FileFilter.bmp);
            }
        }

        private void hScrollBarIlluminator_ValueChanged(object sender, System.EventArgs e)
        {
            //  고해상도 카메라에는 Red Ring 과 IR 조명이 달려 있음
            if (radioButton_RecipeVision_Light_IR.Checked)
            {
                workStage.Config.ListIlluminationChannel[1].Value = hScrollBar_RecipeVision_Illuminator.Value;
                this.textBox_RecipeVision_IlluminationValue.Text = hScrollBar_RecipeVision_Illuminator.Value.ToString();
                CommonModule.Instance.Illuminator.SetVolume(this.hScrollBar_RecipeVision_Illuminator.Value, 2);

                Owner.Illuminator.SetVolume(workStage.Config.ListIlluminationChannel[1].Value, 2);

            }
            //else        //  Red Ring
            //{
            //    workStage.Config.ListIlluminationChannel[0].Value = hScrollBar_RecipeVision_Illuminator.Value;
            //    this.textBox_RecipeVision_IlluminationValue.Text = hScrollBar_RecipeVision_Illuminator.Value.ToString();
            //    CommonModule.Instance.Illuminator.SetVolume(this.hScrollBar_RecipeVision_Illuminator.Value, 1);

            //    Owner.Illuminator.SetVolume(workStage.Config.ListIlluminationChannel[0].Value, 1);
            //}

            this.textBox_RecipeVision_IlluminationValue.Refresh();
        }

        private void SetScroll(int nChannel)
        {
            hScrollBar_RecipeVision_Illuminator.Minimum = (int)workStage.Config.ListIlluminationChannel[nChannel].Min;
            hScrollBar_RecipeVision_Illuminator.Maximum = (int)workStage.Config.ListIlluminationChannel[nChannel].Max;

            baseLabel_RecipeVision_Min.Text = hScrollBar_RecipeVision_Illuminator.Minimum.ToString();
            baseLabel_RecipeVision_Max.Text = hScrollBar_RecipeVision_Illuminator.Maximum.ToString();
        }


        private void radioButton_RecipeVision_Light_IR_CheckedChanged(object sender, EventArgs e)
        {
            SetScroll(1);

            //  조명값 변경
            //CommonModule.Instance.Illuminator.SetVolume(workStage.Config.ListIlluminationChannel[1].Value, 2);
            //CommonModule.Instance.Illuminator.TurnOnOff(true, 2);
            //CommonModule.Instance.Illuminator.SetVolume(workStage.Config.ListIlluminationChannel[0].Value, 1);
            //CommonModule.Instance.Illuminator.TurnOnOff(true, 1);

            hScrollBar_RecipeVision_Illuminator.Value = 400; // workStage.Config.ListIlluminationChannel[1].Value;                //  고해상도 카메라 IR 조명 (2번, Index 는 1번)
            this.textBox_RecipeVision_IlluminationValue.Text = hScrollBar_RecipeVision_Illuminator.Value.ToString();

            //  Low Mag Camera 조명 끄기
            CommonModule.Instance.Illuminator.TurnOnOff(true, 3);

        }

        private void radioButton_RecipeVision_Light_Red_CheckedChanged(object sender, EventArgs e)
        {
            return;

            SetScroll(0);

            //  조명값 변경
            //CommonModule.Instance.Illuminator.SetVolume(workStage.Config.ListIlluminationChannel[0].Value, 1);
            //CommonModule.Instance.Illuminator.TurnOnOff(true, 1);
            //CommonModule.Instance.Illuminator.SetVolume(workStage.Config.ListIlluminationChannel[1].Value, 2);
            //CommonModule.Instance.Illuminator.TurnOnOff(true, 2);

            hScrollBar_RecipeVision_Illuminator.Value = 400;// workStage.Config.ListIlluminationChannel[0].Value;                //  저해상도 카메라 IR 조명 (3번, Index 는 2번)
            this.textBox_RecipeVision_IlluminationValue.Text = hScrollBar_RecipeVision_Illuminator.Value.ToString();

            //  Low Mag Camera 조명 끄기
            CommonModule.Instance.Illuminator.TurnOnOff(false, 3);

        }

        private void baseToggleButton_RecipeVision_DuplicateCheck_Click(object sender, EventArgs e)
        {
            bool bOn = baseToggleButton_RecipeVision_DuplicateCheck.GetButtonStatus();
            if (bOn)
            {
                baseToggleButton_RecipeVision_DuplicateCheck.UpdateToggleStatus(false);
            }
            else
            {
                baseToggleButton_RecipeVision_DuplicateCheck.UpdateToggleStatus(true);
            }
        }

        private void baseToggleButton_RecipeVision_UseMaskImage_Click(object sender, EventArgs e)
        {
            bool bOn = baseToggleButton_RecipeVision_UseMaskImage.GetButtonStatus();
            if (bOn)
            {
                baseToggleButton_RecipeVision_UseMaskImage.UpdateToggleStatus(false);
            }
            else
            {
                baseToggleButton_RecipeVision_UseMaskImage.UpdateToggleStatus(true);
            }
        }

        private void radioButton_RecipeVision_Pixel_CheckedChanged(object sender, EventArgs e)
        {
            IsPixel = true;
        }

        private void radioButton_RecipeVision_mm_CheckedChanged(object sender, EventArgs e)
        {
            IsPixel = false;
        }

        public void UpdataPositionData(double x, double y, double t, double circlex = 0.0, double circley = 0.0)
        {
            baseTextBox_RecipeVision_PositionX.Text = x.ToString();
            baseTextBox_RecipeVision_PositionY.Text = y.ToString();
            baseTextBox_RecipeVision_PositionT.Text = t.ToString();

            if (checkBox_RecipeVision_CirclePos.Checked)
            {
                label_RecipeVision_CirclePosX.Text = circlex.ToString();
                label_RecipeVision_CirclePosY.Text = circley.ToString();
            }
            else
            {
                label_RecipeVision_CirclePosX.Text = "---";
                label_RecipeVision_CirclePosY.Text = "---";
            }

            baseTextBox_RecipeVision_PositionX.Refresh();
            baseTextBox_RecipeVision_PositionY.Refresh();
            baseTextBox_RecipeVision_PositionT.Refresh();
            label_RecipeVision_CirclePosX.Refresh();
            label_RecipeVision_CirclePosY.Refresh();

        }

        private void button_RecipeVision_Search_Click(object sender, EventArgs e)
        {
            UpdataPositionData(0.0, 0.0, 0.0);

            if (ImageViewer_RecipeVision_Rows.Simulated)
            {
                Owner.Simulated = true;
                Owner.Camera.LatestImage = ImageViewer_RecipeVision_Rows.InputImage;
                Owner.TestImage = ImageViewer_RecipeVision_Rows.InputImage;
            }

            if (Equipment.Scanner_Calibration_UseBlobVisionTool)
            {
                if (IsPixel == true)
                {
                    PatternMatchingParameter.MaxTolerance = 0;
                    PatternMatchingParameter.MaxInstance = Equipment.ToInt(basetextBox_RecipeVision_MaxInstance.Text);
                    PatternMatchingParameter.MinTolerance = 0;
                    PatternMatchingParameter.MinScore = Equipment.ToDouble(basetextBox_RecipeVision_MinScore.Text);
                    PatternMatchingParameter.DuplicateChecked = baseToggleButton_RecipeVision_DuplicateCheck.GetButtonStatus();
                    PatternMatchingParameter.UseMaskImage = baseToggleButton_RecipeVision_UseMaskImage.GetButtonStatus();

                    PatternMatchingResult result = Owner.GetResult();
                    if (result != null)
                    {
                        ImageViewer_RecipeVision_Rows.ResultOverlays.Clear();
                    }

                    result = Owner.Search();
                    if (result != null)
                    {
                        foreach (var overlay in result.ResultOverlays)
                        {
                            ImageViewer_RecipeVision_Rows.ResultOverlays.Add(overlay);
                            overlay.Visible = true;
                        }
                    }

                    //BlobResult result = workStage.scannerCompensator.Blob();
                    bool bFind = false;
                    QMC_ImageProcessFindAlign qip = new QMC_ImageProcessFindAlign();
                    List<RectangleF> Fiducial_circlesResult = new List<RectangleF>();

                    VisionScale m_TempScale = new VisionScale();
                    m_TempScale.X = workStage.Config.ParamConfig.UpperVision_Scale_X;
                    m_TempScale.Y = workStage.Config.ParamConfig.UpperVision_Scale_Y;
                    m_TempScale.InvertedX = workStage.Config.ParamConfig.UpperVision_ScaleInvert_X;
                    m_TempScale.InvertedY = workStage.Config.ParamConfig.UpperVision_ScaleInvert_Y;

                    double pixelR = (Equipment.Scanner_Calibration_CrossMarkLength / 2) / (m_TempScale.X);
                    if (ImageViewer_RecipeVision_Rows.Simulated)
                    {
                        qip.FindCirclesWidthCircleBoundary(Fiducial_circlesResult, ImageViewer_RecipeVision_Rows.InputImage.RawData
                            , ImageViewer_RecipeVision_Rows.InputImage.Header.Width
                            , ImageViewer_RecipeVision_Rows.InputImage.Header.Height
                            , (int)pixelR, 0.5, ref bFind, (int)result.Values[0].X, (int)result.Values[0].Y);
                        //1000, 1 -> 엄청느린값 // 원의 반지름의 값이랑 오차범위
                        //센터점 전달해서 찾기로, 센터 못찾으면 그냥 센터로. 
                    }
                    else
                    {
                        qip.FindCirclesWidthCircleBoundary(Fiducial_circlesResult, Owner.Camera.LatestImage.RawData
                            , Owner.Camera.LatestImage.Header.Width
                            , Owner.Camera.LatestImage.Header.Height
                            , (int)pixelR, 0.5, ref bFind, (int)result.Values[0].X, (int)result.Values[0].Y);
                        //1000, 1 -> 엄청느린값 // 원의 반지름의 값이랑 오차범위
                        //센터점 전달해서 찾기로, 센터 못찾으면 그냥 센터로. 
                    }

                    if (Fiducial_circlesResult.Count > 0)
                    {
                        double x = Fiducial_circlesResult[0].X + (Fiducial_circlesResult[0].Width / 2);
                        double y = Fiducial_circlesResult[0].Y + (Fiducial_circlesResult[0].Height / 2);

                        PatternMatchingResultValue pmrv = new PatternMatchingResultValue();
                        pmrv.X = x;
                        pmrv.Y = y;

                        //비교를 위해서 우선 막아놈.
                        //result.Values[0] = pmrv;
                        RectangleFrameVisionImageOverlay overlay = new RectangleFrameVisionImageOverlay("FindCircle",
                            new Point((int)Fiducial_circlesResult[0].Left, (int)Fiducial_circlesResult[0].Top)
                            , new Point((int)(Fiducial_circlesResult[0].Right), (int)Fiducial_circlesResult[0].Bottom));

                        ImageViewer_RecipeVision_Rows.ResultOverlays.Add(overlay);
                        overlay.Visible = true;

                        UpdataPositionData(result.Values[0].X, result.Values[0].Y, result.Values[0].R, pmrv.X, pmrv.Y);
                    }
                }
                else
                {
                    //PointD converted = new PointD((result.Values[0].X - this.m_Owner.Camera.Resolution.Width / 2) * ((WorkStage)this.m_Owner.Owner).Scale.X * (((WorkStage)this.m_Owner.Owner).Scale.InvertedX ? 1 : -1),
                    //                              (result.Values[0].Y - this.m_Owner.Camera.Resolution.Height / 2) * ((WorkStage)this.m_Owner.Owner).Scale.Y * (((WorkStage)this.m_Owner.Owner).Scale.InvertedY ? 1 : -1));

                    //PointD converted = new PointD((result.Values[0].X - workStage.scannerCompensator.Camera.Resolution.Width / 2) * (workStage.scannerCompensator.Owner).Scale.X * ((workStage.scannerCompensator.Owner).Scale.InvertedX ? 1 : -1),
                    //                              (result.Values[0].Y - workStage.scannerCompensator.Camera.Resolution.Height / 2) * (workStage.scannerCompensator.Owner).Scale.Y * ((workStage.scannerCompensator.Owner).Scale.InvertedY ? 1 : -1));

                    //UpdataPositionData(converted.X, converted.Y, result.Values[0].R);
                }

            }
            else //Scanner_Calibration_UsePatternMatching
            {
                //jigAligner_LowRes.Recipe.PatternMatchingParameter = stVisionRecipeSet.PatternMatching;
                //jigAligner_LowRes.Recipe.InspectRoiStartLocation = stVisionRecipeSet.InspectRoiStartLocation;
                //jigAligner_LowRes.Recipe.InspectRoiEndLocation = stVisionRecipeSet.InspectRoiEndLocation;
                //jigAligner_LowRes.Recipe.TrainRoiStartLocation = stVisionRecipeSet.TrainRoiStartLocation;
                //jigAligner_LowRes.Recipe.TrainRoiEndLocation = stVisionRecipeSet.TrainRoiEndLocation;

                PatternMatchingParameter.MaxTolerance = Equipment.ToDouble(basetextBox_RecipeVision_AngleTolerance.Text);
                PatternMatchingParameter.MaxInstance = Equipment.ToInt(basetextBox_RecipeVision_MaxInstance.Text);
                PatternMatchingParameter.MinTolerance = Equipment.ToDouble(basetextBox_RecipeVision_AngleTolerance.Text) * -1;
                PatternMatchingParameter.MinScore = Equipment.ToDouble(basetextBox_RecipeVision_MinScore.Text);
                PatternMatchingParameter.DuplicateChecked = baseToggleButton_RecipeVision_DuplicateCheck.GetButtonStatus();
                PatternMatchingParameter.UseMaskImage = baseToggleButton_RecipeVision_UseMaskImage.GetButtonStatus();
                PatternMatchingParameter.TrainImage = pictureBox_RecipeVision_TrainImage.Image;
                Owner.Recipe.PatternMatchingParameter = PatternMatchingParameter;

                Owner.Recipe.InspectRoiStartLocation = RoiInspect.Parameter.StartLocation;
                Owner.Recipe.InspectRoiEndLocation = RoiInspect.Parameter.EndLocation;
                Owner.Recipe.TrainRoiStartLocation = RoiTrain.Parameter.StartLocation;
                Owner.Recipe.TrainRoiEndLocation = RoiTrain.Parameter.EndLocation;

                PatternMatchingResult result = Owner.GetResult();
                if (result != null)
                {
                    ImageViewer_RecipeVision_Rows.ResultOverlays.Clear();
                }

                result = Owner.Search();
                if (result != null)
                {
                    foreach (var overlay in result.ResultOverlays)
                    {
                        ImageViewer_RecipeVision_Rows.ResultOverlays.Add(overlay);
                        overlay.Visible = true;
                    }
                }

                if (result != null && result.Values.Count > 0)
                {
                    if (IsPixel == true)
                    {
                        UpdataPositionData(result.Values[0].X, result.Values[0].Y, result.Values[0].R);
                    }
                    else
                    {
                        //PointD converted = new PointD((result.Values[0].X - this.m_Owner.Camera.Resolution.Width / 2) * ((WorkStage)this.m_Owner.Owner).Scale.X * (((WorkStage)this.m_Owner.Owner).Scale.InvertedX ? 1 : -1),
                        //                              (result.Values[0].Y - this.m_Owner.Camera.Resolution.Height / 2) * ((WorkStage)this.m_Owner.Owner).Scale.Y * (((WorkStage)this.m_Owner.Owner).Scale.InvertedY ? 1 : -1));

                        //PointD converted = new PointD((result.Values[0].X - workStage.scannerCompensator.Camera.Resolution.Width / 2) * (workStage.scannerCompensator.Owner).Scale.X * ((workStage.scannerCompensator.Owner).Scale.InvertedX ? 1 : -1),
                        //                              (result.Values[0].Y - workStage.scannerCompensator.Camera.Resolution.Height / 2) * (workStage.scannerCompensator.Owner).Scale.Y * ((workStage.scannerCompensator.Owner).Scale.InvertedY ? 1 : -1));

                        //UpdataPositionData(converted.X, converted.Y, result.Values[0].R);
                    }
                }
                ImageViewer_RecipeVision_Rows.Display();
                UpdateOwnerRecipe(PatternMatchingParameter);
            }

            Owner.Camera.StartLive();
        }

        private void UpdateOwnerRecipe(PatternMatchingParameters parameters)
        {
            if (Owner is JigAligner)
            {
                JigAligner visionCompensator = Owner as JigAligner;
                visionCompensator.Recipe.PatternMatchingParameter = parameters;
                Owner = visionCompensator;
            }
            Equipment.SaveRecipe();
        }

        private void checkBox_RecipeVision_CirclePos_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button_RecipeVision_Vision_Save_Click(object sender, EventArgs e)
        {
            Equipment.stVisionRecipeSet.PatternMatching = PatternMatchingParameter;
            Equipment.stVisionRecipeSet.TrainRoiStartLocation = RoiTrain.Parameter.StartLocation;
            Equipment.stVisionRecipeSet.TrainRoiEndLocation = RoiTrain.Parameter.EndLocation;
            Equipment.stVisionRecipeSet.InspectRoiStartLocation = RoiInspect.Parameter.StartLocation;
            Equipment.stVisionRecipeSet.InspectRoiEndLocation = RoiInspect.Parameter.EndLocation;
            Equipment.stVisionRecipeSet.IlluminationIR = hScrollBar_RecipeVision_Illuminator.Value;
            //Equipment.stVisionRecipeSet.TrainImagePath = Equipment.stVisionRecipeSet.TrainImagePath;
            Equipment.stVisionRecipeSet.SaveTrainImage(pictureBox_RecipeVision_TrainImage.Image);

            Equipment.stVisionRecipeSet.SaveToIni(Equipment.Current_Recipe);

            UpdateOwnerRecipe(PatternMatchingParameter);

            //workStage.jigAligner_LowRes.Recipe.PatternMatchingParameter = PatternMatchingParameter;
            //Equipment.SaveRecipe();

            //Equipment.Scanner_Calibration_BlobVisionToolParameter.RepeatCount = BlobParameter.RepeatCount;
            //Equipment.Scanner_Calibration_BlobVisionToolParameter.HasChanged = BlobParameter.HasChanged;

            //workStage.scannerCompensator.Recipe.PatternMatchingParameter = PatternMatchingParameter;
            //workStage.scannerCompensator.Recipe.BlobParameter = BlobParameter;
            //Equipment.SaveRecipe();

            //workStage.Scanner_Calibration_Vision_Save();
        }

        //Jog Move
        private void InitializeJogButtons()
        {
            // Tag 설정
            button_RecipeVision_X_Pos.Tag = "X,+1";
            button_RecipeVision_X_Neg.Tag = "X,-1";
            button_RecipeVision_Y_Pos.Tag = "Y,+1";
            button_RecipeVision_Y_Neg.Tag = "Y,-1";
            button_RecipeVision_Z_Pos.Tag = "Z,+1";
            button_RecipeVision_Z_Neg.Tag = "Z,-1";

            // 공통 MouseDown 핸들러
            button_RecipeVision_X_Pos.MouseDown += button_RecipeVision_Axis_MouseDown;
            button_RecipeVision_X_Neg.MouseDown += button_RecipeVision_Axis_MouseDown;
            button_RecipeVision_Y_Pos.MouseDown += button_RecipeVision_Axis_MouseDown;
            button_RecipeVision_Y_Neg.MouseDown += button_RecipeVision_Axis_MouseDown;
            button_RecipeVision_Z_Pos.MouseDown += button_RecipeVision_Axis_MouseDown;
            button_RecipeVision_Z_Neg.MouseDown += button_RecipeVision_Axis_MouseDown;

            // 공통 MouseUp 핸들러
            button_RecipeVision_X_Pos.MouseUp += button_RecipeVision_Axis_MouseUp;
            button_RecipeVision_X_Neg.MouseUp += button_RecipeVision_Axis_MouseUp;
            button_RecipeVision_Y_Pos.MouseUp += button_RecipeVision_Axis_MouseUp;
            button_RecipeVision_Y_Neg.MouseUp += button_RecipeVision_Axis_MouseUp;
            button_RecipeVision_Z_Pos.MouseUp += button_RecipeVision_Axis_MouseUp;
            button_RecipeVision_Z_Neg.MouseUp += button_RecipeVision_Axis_MouseUp;
        }

        private void Motion_Status()
        {
            //  Limit
            if (Equipment.AjinBoard_Opened)
            {
                //  Vision
                if (workStage.MC_Func.MC_isLimit_Neg((int)Vision.nAxis.X))
                {
                    button_RecipeVision_X_Neg.BackColor = Color.Red;
                    button_RecipeVision_X_Neg.ForeColor = Color.White;
                }
                else
                {
                    button_RecipeVision_X_Neg.BackColor = Color.White;
                    button_RecipeVision_X_Neg.ForeColor = Color.Black;
                }

                if (workStage.MC_Func.MC_isLimit_Pos((int)Vision.nAxis.X))
                {
                    button_RecipeVision_X_Pos.BackColor = Color.Red;
                    button_RecipeVision_X_Pos.ForeColor = Color.White;
                }
                else
                {
                    button_RecipeVision_X_Pos.BackColor = Color.White;
                    button_RecipeVision_X_Pos.ForeColor = Color.Black;
                }

                if (workStage.MC_Func.MC_isLimit_Neg((int)Vision.nAxis.Y))
                {
                    button_RecipeVision_Y_Neg.BackColor = Color.Red;
                    button_RecipeVision_Y_Neg.ForeColor = Color.White;
                }
                else
                {
                    button_RecipeVision_Y_Neg.BackColor = Color.White;
                    button_RecipeVision_Y_Neg.ForeColor = Color.Black;
                }

                if (workStage.MC_Func.MC_isLimit_Pos((int)Vision.nAxis.Y))
                {
                    button_RecipeVision_Y_Pos.BackColor = Color.Red;
                    button_RecipeVision_Y_Pos.ForeColor = Color.White;
                }
                else
                {
                    button_RecipeVision_Y_Pos.BackColor = Color.White;
                    button_RecipeVision_Y_Pos.ForeColor = Color.Black;
                }

                if (workStage.MC_Func.MC_isLimit_Neg((int)Vision.nAxis.Z))
                {
                    button_RecipeVision_Z_Neg.BackColor = Color.Red;
                    button_RecipeVision_Z_Neg.ForeColor = Color.White;
                }
                else
                {
                    button_RecipeVision_Z_Neg.BackColor = Color.White;
                    button_RecipeVision_Z_Neg.ForeColor = Color.Black;
                }

                if (workStage.MC_Func.MC_isLimit_Pos((int)Vision.nAxis.Z))
                {
                    button_RecipeVision_Z_Pos.BackColor = Color.Red;
                    button_RecipeVision_Z_Pos.ForeColor = Color.White;
                }
                else
                {
                    button_RecipeVision_Z_Pos.BackColor = Color.White;
                    button_RecipeVision_Z_Pos.ForeColor = Color.Black;
                }
            }
        }

        private void button_RecipeVision_Axis_MouseUp(object sender, MouseEventArgs e)
        {
            if (!this.radioButton_RecipeVision_JogMove_Continuous.Checked)
                return;

            if (Equipment.AjinBoard_Opened)
            {
                workStage.MC_Func.MC_JogStop((int)Vision.nAxis.X);
                workStage.MC_Func.MC_JogStop((int)Vision.nAxis.Y);
                workStage.MC_Func.MC_JogStop((int)Vision.nAxis.Z);
            }
        }

        private void button_RecipeVision_Axis_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Button btn && Equipment.AjinBoard_Opened)
            {
                string[] tagParts = btn.Tag?.ToString()?.Split(',');
                if (tagParts == null || tagParts.Length != 2)
                    return;

                string axisName = tagParts[0].ToUpper();   // "X", "Y", "Z"
                double direction = Convert.ToDouble(tagParts[1]);  // +1.0 or -1.0

                int axis = -1;
                double velocity = 0.0;
                double accdec = 0.0;
                double distance = 0.0;

                // 축 번호 결정
                switch (axisName)
                {
                    case "X": axis = (int)Vision.nAxis.X; break;
                    case "Y": axis = (int)Vision.nAxis.Y; break;
                    case "Z": axis = (int)Vision.nAxis.Z; break;
                    default: return;
                }

                // 속도/가감속 설정
                if (radioButton_RecipeVision_Move_MoveMode_Fine.Checked)
                {
                    velocity = Equipment.stAxisParam[axis].Jog_Speed_Fine;
                    accdec = Equipment.stAxisParam[axis].Common_Acceleration_Fine;
                }
                else
                {
                    velocity = Equipment.stAxisParam[axis].Jog_Speed_Coarse;
                    accdec = Equipment.stAxisParam[axis].Common_Acceleration_Coarse;
                }

                velocity = Math.Abs(velocity);

                try
                {
                    if (radioButton_RecipeVision_JogMove_Continuous.Checked)
                    {
                        workStage.MC_Func.MC_JogMove(axis, velocity * direction, accdec, accdec);
                    }
                    else if (radioButton_RecipeVision_JogMove_Step.Checked)
                    {
                        string text = textBox_RecipeVision_JogMove_StepSize.Text;
                        distance = Math.Abs(Equipment.ToDouble(text));
                        workStage.MC_Func.MC_MoveRelPosition(axis, distance * direction, velocity, accdec, accdec);
                    }
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button_RecipeVision_WorkStage_GetCurrentPos_ToTempPos1_Click(object sender, EventArgs e)
        {
            textBox_RecipeVision_WorkStage_TempPos1_StageX.Text = string.Format("{0:0.000}", workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X).ToString());
            textBox_RecipeVision_WorkStage_TempPos1_StageY.Text = string.Format("{0:0.000}", workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y).ToString());

            Temp_Position_Save();
        }

        private void button_RecipeVision_WorkStage_ToTempPos1_Move_Click(object sender, EventArgs e)
        {
            //  Temp1 위치로 이동
            //  현재 Fine Camera Center 위치를 Scanner Center 위치로 이동
            double lfTargetX = 0.0f;
            double lfTargetY = 0.0f;
            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;

            // 파일에 저장 해 놓자.
            double X_Limit_Min = 5.0;
            double X_Limit_Max = 800.0;
            double Y_Limit_Min = 5.0;
            double Y_Limit_Max = 500.0;


            if (Equipment.ToDouble(textBox_RecipeVision_WorkStage_TempPos1_StageX.Text) == 0.0 && 
                Equipment.ToDouble(textBox_RecipeVision_WorkStage_TempPos1_StageY.Text) == 0.0)
            {
                var mb1 = new QMC.Common.UI.MessageBoxOk();
                mb1.ShowDialog("Warning !", "Temp1 위치가 설정되어 있지 않습니다.");
                return;
            }

            var mb = new QMC.Common.UI.MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Temp1 위치로 이동하시겠습니까?"))
                return;

            if (!workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.X) || 
                !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Y) || 
                !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Z) ||
                !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.X) || 
                !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Y) || 
                !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Z))
            {
                var mb1 = new QMC.Common.UI.MessageBoxOk();
                mb1.ShowDialog("Warning !", "Stage 가 이동중입니다.");
                return;
            }


            //Todo: Z축 이동시 Interlock 체크 - 코드 삽입 할것.1!!!
            ////  StageZ 한계위치 설정되어 있는지 체크
            //if (laserDrilling.Config.ParamConfig.Interlock_StageZ_UpperPos <= 0.0)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Warning !", "Stage Z축 한계 높이가 설정되어 있지 않습니다.\r\n\r\n(Config -> [17] Interlock  확인)");
            //    return;
            //}
            ////  StageZ 한계위치를 초과하여 이동하는지 체크
            //if (laserDrilling.Config.ParamConfig.Interlock_StageZ_UpperPos < laserDrilling.laserDrillingParameter.stLaserDrillingPosParam.dTarget[(int)WorkStageParameter.MotionKey.Z])
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Warning !", "Stage Z축 한계 높이를 초과하여 이동하려고 하였습니다.\r\n\r\n[ Cancel ]");
            //    return;
            //}
            ////  맵 데이터를 이원화 할 경우
            //if (laserDrilling.Config.ParamConfig.ScannerCamera_MapData_Div)
            //{
            //    laserDrilling.MapData_Change((int)LaserDrilling.MapDataType.MAPDATASTATUS_SCANNER);
            //}

            //  Target 위치
            lfTargetX = Equipment.ToDouble(textBox_RecipeVision_WorkStage_TempPos1_StageX.Text);
            lfTargetY = Equipment.ToDouble(textBox_RecipeVision_WorkStage_TempPos1_StageY.Text);

            //  소프트웨어 리밋 체크
            if (lfTargetX < X_Limit_Min || lfTargetX > X_Limit_Max ||
                lfTargetY < Y_Limit_Min || lfTargetY > Y_Limit_Max)
            {
                string msg = $"이동하려는 위치가 소프트웨어 리밋을 벗어났습니다.\n\n" +
                             $"X 범위: {X_Limit_Min} ~ {X_Limit_Max}, 현재: {lfTargetX}\n" +
                             $"Y 범위: {Y_Limit_Min} ~ {Y_Limit_Max}, 현재: {lfTargetY}";
                var mb1 = new QMC.Common.UI.MessageBoxOk();
                mb1.ShowDialog("Software Limit", msg);
                return;
            }

            //  속도 설정
            if (radioButton_RecipeVision_Move_MoveMode_Fine.Checked)
            {
                lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Fine;
                lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Acceleration_Fine;
            }
            else
            {
                lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Coarse;
                lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Acceleration_Coarse;
            }

            XyCoordinate xyInterpolatedCoordinate = new XyCoordinate();
            xyInterpolatedCoordinate.X = lfTargetX;
            xyInterpolatedCoordinate.Y = lfTargetY;

            workStage.MC_Func.MovePosition(xyInterpolatedCoordinate, lfVelocity, lfAccDec, lfAccDec);

            int nWait = 0;
            while (true)
            {
                if (workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.X)
                    && workStage.MC_Func.MC_PosTolerance((int)WorkStage.nAxis.X, xyInterpolatedCoordinate.X))
                {
                    break;
                }
                Thread.Sleep(1);
                nWait++;
                if (nWait == 1000)
                {
                    break;
                }

            }

            nWait = 0;
            while (true)
            {
                if (workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Y)
                    && workStage.MC_Func.MC_PosTolerance((int)WorkStage.nAxis.Y, xyInterpolatedCoordinate.Y))
                {
                    break;
                }
                Thread.Sleep(1);
                nWait++;
                if (nWait == 1000)
                {
                    break;
                }
            }
        }


        public void Temp_Position_Save()
        {
            string strTemp = "";

            string strFIle = "";
            strFIle = ConfigManager.GetTeachingDataPath() + "\\RecipeVision_TempPosition.ini";

            if (File.Exists(strFIle) == false)
            {
                File.Create(strFIle);

                MessageBox.Show("RecipeVision Temp Position 파일을 생성하였습니다. 다시 시도하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //  Temp Position 저장

            //  Temp1 Stage X
            NativeMethods.WritePrivateProfileString("TempPos1", "StageX", textBox_RecipeVision_WorkStage_TempPos1_StageX.Text.ToString(), strFIle);
            //  Temp1 Stage Y
            NativeMethods.WritePrivateProfileString("TempPos1", "StageY", textBox_RecipeVision_WorkStage_TempPos1_StageY.Text.ToString(), strFIle);

        }

        public bool Temp_Position_Load()
        {
            string strTemp = "";

            bool m_bRet = true;
            string strFIle = "";
            StringBuilder temp = new StringBuilder(255);
            strFIle = ConfigManager.GetTeachingDataPath() + "\\RecipeVision_TempPosition.ini";

            if (File.Exists(strFIle) == false)
            {
                MessageBox.Show("RecipeVision Temp Position 파일이 없습니다.\r\n\r\n[Default 값으로 설정됩니다.]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return false;
            }

            //  Temp Position 데이터 로드
            //  Temp1 Stage X
            NativeMethods.GetPrivateProfileString("TempPos1", "StageX", "0", temp, 255, strFIle);
            textBox_RecipeVision_WorkStage_TempPos1_StageX.Text = temp.ToString();
            //  Temp1 Stage Y
            NativeMethods.GetPrivateProfileString("TempPos1", "StageY", "0", temp, 255, strFIle);
            textBox_RecipeVision_WorkStage_TempPos1_StageY.Text = temp.ToString();

            return m_bRet;
        }

    }
}
