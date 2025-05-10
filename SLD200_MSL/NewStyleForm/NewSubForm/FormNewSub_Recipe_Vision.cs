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
using static QMC.Common.Equipment;
using System.Security.Cryptography;
using MessageBoxOk = QMC.Core.MessageBoxOk;

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

            //Size 축소 / 확대 안되게 하기 위한 코드.
            this.AutoScaleMode = AutoScaleMode.None;

            this.Load += FormNewSub_Recipe_Vision_Load; // 👈 여기서 Load 이벤트 연결

            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;
            foreach (Module module in m_collectionModules)
            {
                if (module.Name == "WorkStage")
                {
                    workStage = module as WorkStage;
                    Owner = workStage.jigAligner_LowRes;
                    workStage.UpdateResultOveray += workStage_UpdateResultOveray;
                }
            }
        }

        private void workStage_UpdateResultOveray(object sender, EventArgs e)
        {
            if (sender is QMC.Common.Vision.Cameras.Camera camera)
            {
                if (camera == workStage.Camera_HighRes)
                {
                    ImageViewer_RecipeVision_highs.ResultOverlays = workStage.FineCamResultOveray;
                }
                else if (camera == workStage.jigAligner_LowRes.Camera)
                {
                    ImageViewer_RecipeVision_Rows.ResultOverlays = workStage.CoarseCamResultOveray;
                }
            }
        }

        private void FormNewSub_Recipe_Vision_Load(object sender, EventArgs e)
        {
            this.AutoScaleMode = AutoScaleMode.None;

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
            }

            workStage.UpdateResultOveray += OnUpdateResultOverlay;

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

            if (this.ImageViewer_RecipeVision_Lows.IsHandleCreated)
            {
                this.ImageViewer_RecipeVision_Lows.SizeMode = PictureBoxSizeMode.CenterImage;
                this.ImageViewer_RecipeVision_Lows.SuspendDisplay();
                this.ImageViewer_RecipeVision_Lows.StopUpdateTask();

                this.ImageViewer_RecipeVision_Lows.Camera = Owner.Camera;

                this.ImageViewer_RecipeVision_Lows.ResumeDisplay();
                this.ImageViewer_RecipeVision_Lows.StartUpdateTask();
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

            this.hScrollBar_RecipeVision_Illuminator_IR.ValueChanged += new System.EventHandler(this.hScrollBarIlluminator_ValueChanged_IR);
            this.hScrollBar_RecipeVision_Illuminator_Red.ValueChanged += new System.EventHandler(this.hScrollBarIlluminator_ValueChanged_Red);
            SetScroll();

            IsPixel = true;

            InitPatternMatchingParameter();

            InitializeJogButtons();

            Temp_Position_Load();
        }

        private void OnUpdateResultOverlay(object sender, EventArgs e)
        {
            if (sender is QMC.Common.Vision.Cameras.Camera camera)
            {
                if (camera == workStage.Camera_HighRes)
                {
                    ImageViewer_RecipeVision_highs.ResultOverlays = workStage.FineCamResultOveray;
                }
                else if (camera == workStage.jigAligner_LowRes.Camera)
                {
                    ImageViewer_RecipeVision_Lows.ResultOverlays = workStage.CoarseCamResultOveray;
                }
            }
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

            InitPatternMatchingParameter();

            this.ImageViewer_RecipeVision_highs.ResumeDisplay();
            this.ImageViewer_RecipeVision_highs.StartUpdateTask();

            this.ImageViewer_RecipeVision_Lows.ResumeDisplay();
            this.ImageViewer_RecipeVision_Lows.StartUpdateTask();

            this.RecipeVisionTimer.Start();
        }

        public void OnHide()
        {
            if (!m_bFormVisible)
                return;

            m_bFormVisible = false;

            this.ImageViewer_RecipeVision_highs.SuspendDisplay();
            this.ImageViewer_RecipeVision_highs.StopUpdateTask();

            this.ImageViewer_RecipeVision_Lows.SuspendDisplay();
            this.ImageViewer_RecipeVision_Lows.StopUpdateTask();

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
                if(Equipment.stVisionRecipeSet.PatternMatching != null)
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
                }

                RoiTrain.Parameter.StartLocation = Equipment.stVisionRecipeSet.TrainRoiStartLocation;
                RoiTrain.Parameter.EndLocation = Equipment.stVisionRecipeSet.TrainRoiEndLocation;
                RoiInspect.Parameter.StartLocation = Equipment.stVisionRecipeSet.InspectRoiStartLocation;
                RoiInspect.Parameter.EndLocation = Equipment.stVisionRecipeSet.InspectRoiEndLocation;

                Owner.Recipe.InspectRoiStartLocation = Equipment.stVisionRecipeSet.InspectRoiStartLocation; //RoiInspect.Parameter.StartLocation;
                Owner.Recipe.InspectRoiEndLocation = Equipment.stVisionRecipeSet.InspectRoiEndLocation;     //RoiInspect.Parameter.EndLocation;
                Owner.Recipe.TrainRoiStartLocation = Equipment.stVisionRecipeSet.TrainRoiStartLocation;     //RoiTrain.Parameter.StartLocation;
                Owner.Recipe.TrainRoiEndLocation = Equipment.stVisionRecipeSet.TrainRoiEndLocation;         //RoiTrain.Parameter.EndLocation;
            }

            if (Equipment.stVisionRecipeSet.PatternShape == PatternShapeType.Cross)
            {
                radioButton_RecipeVision_Cross.Checked = true;
                radioButton_RecipeVision_Circle.Checked = false;
            }
            else
            {
                radioButton_RecipeVision_Cross.Checked = false;
                radioButton_RecipeVision_Circle.Checked = true;
            }

            if (Equipment.stVisionRecipeSet.AlgorithmType == VisionAlgorithmType.PatternMatching)
            {
                radioButton_RecipeVision_Pattern.Checked = true;
                radioButton_RecipeVision_Blob.Checked = false;
            }
            else
            {
                radioButton_RecipeVision_Pattern.Checked = false;
                radioButton_RecipeVision_Blob.Checked = true;
            }

            if (Equipment.stVisionRecipeSet.bCircleDetectionColor == true)
            {
                radioButton_RecipeVision_White.Checked = false;
                radioButton_RecipeVision_Black.Checked = true;
            }
            else
            {
                radioButton_RecipeVision_White.Checked = true;
                radioButton_RecipeVision_Black.Checked = false;
            }

            this.textBox_RecipeVision_Circle_Spec.Text = Equipment.stVisionRecipeSet.dCircleSpec.ToString();
            this.textBox_RecipeVision_Circle_Size.Text = Equipment.stVisionRecipeSet.dCircleDetectionSizeW.ToString();

            this.radioButton_RecipeVision_Move_MoveMode_Fine.Checked = false;
            this.radioButton_RecipeVision_Move_MoveMode_Coarse.Checked = true;
            this.radioButton_RecipeVision_JogMove_Continuous.Checked = false;
            this.radioButton_RecipeVision_JogMove_Step.Checked = true;

            this.radioButton_RecipeVision_Circle.Checked = true;

            //workStage.Config.ListIlluminationChannel[0].Value = Equipment.Scanner_Calibration_Illumination_channel_01_Value; //RED
            //workStage.Config.ListIlluminationChannel[1].Value = Equipment.Scanner_Calibration_Illumination_channel_02_Value; //IR

            comboBox_Recipe_Fiducial_Miscellaneous_FiducialAlignType.SelectedIndex = Equipment.stVisionRecipeSet.Miscellaneous_FiducialAlignType;
            comboBox_Recipe_Fiducial_Miscellaneous_FiducialMarkType.SelectedIndex = Equipment.stVisionRecipeSet.Miscellaneous_FiducialMarkType;
            textBox_Recipe_Fiducial_CircleSpec.Text = Equipment.stVisionRecipeSet.Miscellaneous_FiducialMarkSpec.ToString();
            textBox_Recipe_Fiducial_CircleSize.Text = Equipment.stVisionRecipeSet.Miscellaneous_FiducialMarkRadius.ToString();


            textBox_Recipe_RecipeVision_Illuminator_FineCamRed.Text = Equipment.stVisionRecipeSet.nFiduciallluminationRed.ToString();
            textBox_Recipe_RecipeVision_Illuminator_FineCamIR.Text = Equipment.stVisionRecipeSet.nFiduciallluminationIR.ToString();
            textBox_Recipe_RecipeVision_Illuminator_CoarseCamIR.Text = Equipment.stVisionRecipeSet.nPreAlignlluminationIR.ToString();

            SetScroll();
        }

        private void button_RecipeVision_CameraLive_Click(object sender, EventArgs e)
        {
            if (!Owner.Camera.Opened || !workStage.Camera_HighRes.Opened)
            {
                var mb1 = new QMC.Common.UI.MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 카메라를 연결해야 해야 합니다.");
                return;
            }

            if (radioButton_RecipeVision_CameraSelection_LowMag.Checked)
            {
                ImageViewer_RecipeVision_Lows.Simulated = false;
                Owner.Simulated = false;

                this.ImageViewer_RecipeVision_Lows.StartUpdateTask();
                this.ImageViewer_RecipeVision_Lows.Visible = true;

                if (Owner.Camera != null)
                    Owner.Camera.StartLive();
            }
            else if (radioButton_RecipeVision_CameraSelection_HighMag.Checked)
            {
                ImageViewer_RecipeVision_highs.Simulated = false;
                Owner.Simulated = false;
                this.ImageViewer_RecipeVision_highs.StartUpdateTask();
                this.ImageViewer_RecipeVision_highs.Visible = true;
                if (workStage.Camera_HighRes != null)
                    workStage.Camera_HighRes.StartLive();
            }
            else
            {
                var mb1 = new QMC.Common.UI.MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 카메라를 선택해야 합니다.");
                return;
            }
        }

        private void button_RecipeVision_CameraStop_Click(object sender, EventArgs e)
        {
            if (!Owner.Camera.Opened || !workStage.Camera_HighRes.Opened)
            {
                var mb1 = new QMC.Common.UI.MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 카메라를 연결해야 해야 합니다.");
                return;
            }

            if (radioButton_RecipeVision_CameraSelection_LowMag.Checked)
            {
                this.ImageViewer_RecipeVision_Lows.StopUpdateTask();
                if (Owner.Camera != null)
                    Owner.Camera.StopLive();
            }
            else if(radioButton_RecipeVision_CameraSelection_HighMag.Checked)
            {
                this.ImageViewer_RecipeVision_highs.StopUpdateTask();
                if (workStage.Camera_HighRes != null)
                    workStage.Camera_HighRes.StopLive();
            }
            else
            {
                var mb1 = new QMC.Common.UI.MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 카메라를 선택해야 합니다.");
                return;
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
            this.ImageViewer_RecipeVision_Lows.NormalOverlays.Add(RoiTrain.Parameter.Overlay);
            foreach (var overlay in result.ResultOverlays)
            {
                this.ImageViewer_RecipeVision_Lows.NormalOverlays.Remove(overlay);
            }
            this.ImageViewer_RecipeVision_Lows.Display();

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
            this.ImageViewer_RecipeVision_Lows.NormalOverlays.Add(RoiInspect.Parameter.Overlay);
            foreach (var overlay in result.ResultOverlays)
            {
                this.ImageViewer_RecipeVision_Lows.NormalOverlays.Remove(overlay);
            }
            this.ImageViewer_RecipeVision_Lows.Display();

            this.m_RoiListControl.RoiAlignClickNew();
        }

        private void RoiTrainButtonClick(RoiVisionTool roiVisionTool)
        {
            if (Owner != null)
            {
                RoiTrain.Parameter.CenterLocation = roiVisionTool.Parameter.CenterLocation;
                RoiTrain.Parameter.Size = roiVisionTool.Parameter.Size;

                this.ImageViewer_RecipeVision_Lows.NormalOverlays.Add(RoiTrain.Parameter.Overlay);
                RoiTrain.Parameter.Overlay.Visible = true;
                this.ImageViewer_RecipeVision_Lows.Display();

            }
        }

        private void RoiInspectButtonClick(RoiVisionTool roiVisionTool)
        {
            if (Owner != null)
            {
                RoiInspect.Parameter.CenterLocation = roiVisionTool.Parameter.CenterLocation;
                RoiInspect.Parameter.Size = roiVisionTool.Parameter.Size;

                this.ImageViewer_RecipeVision_Lows.NormalOverlays.Add(RoiInspect.Parameter.Overlay);
                RoiInspect.Parameter.Overlay.Visible = true;
                this.ImageViewer_RecipeVision_Lows.Display();
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
                this.ImageViewer_RecipeVision_Lows.Display();
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



            this.ImageViewer_RecipeVision_Lows.Display();
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
                this.ImageViewer_RecipeVision_Lows.Display();
                return;
            }
            RoiInspect = roiVisionTool;
            recipe.InspectRoiStartLocation = RoiInspect.Parameter.StartLocation;
            recipe.InspectRoiEndLocation = RoiInspect.Parameter.EndLocation;

            this.ImageViewer_RecipeVision_Lows.Display();


        }

        private void button_RecipeVision_Train_Set_Click(object sender, EventArgs e)
        {
            if (Owner != null)
            {
                RoiVisionTool roiVisionTool = m_RoiListControl.RoiTrainVisionTool;

                RoiTrain.Parameter.CenterLocation = roiVisionTool.Parameter.CenterLocation;
                RoiTrain.Parameter.Size = roiVisionTool.Parameter.Size;

                this.ImageViewer_RecipeVision_Lows.NormalOverlays.Add(RoiTrain.Parameter.Overlay);
                RoiTrain.Parameter.Overlay.Visible = true;
                this.ImageViewer_RecipeVision_Lows.Display();
            }

            string m_strFile = "";
            string m_strRecipe = "";
            RecipeInfo m_recipeInfo = new RecipeInfo();
            m_recipeInfo = Equipment.GetCurrentRecipe();

            Owner.Camera.LatestImage = this.ImageViewer_RecipeVision_Lows.InputImage;

            if (this.ImageViewer_RecipeVision_Lows.Simulated)
            {
                Owner.Simulated = true;
                Owner.TestImage = this.ImageViewer_RecipeVision_Lows.InputImage;
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

        private void hScrollBarIlluminator_ValueChanged_IR(object sender, System.EventArgs e)
        {
            if (radioButton_RecipeVision_CameraSelection_LowMag.Checked)
            {
                //0:Red-High Mag, 1:IR-High Mag, 2:IR-Low Mag
                workStage.Config.ListIlluminationChannel[2].Value = hScrollBar_RecipeVision_Illuminator_IR.Value;
                this.textBox_RecipeVision_IlluminationValue_IR.Text = hScrollBar_RecipeVision_Illuminator_IR.Value.ToString();
                //1:Red-High Mag, 2:IR-High Mag, 3:IR-Low Mag
                CommonModule.Instance.Illuminator.SetVolume(this.hScrollBar_RecipeVision_Illuminator_IR.Value, 3);
            }
            else if (radioButton_RecipeVision_CameraSelection_HighMag.Checked)
            {
                //0:Red-High Mag, 1:IR-High Mag, 2:IR-Low Mag
                workStage.Config.ListIlluminationChannel[1].Value = hScrollBar_RecipeVision_Illuminator_IR.Value;
                this.textBox_RecipeVision_IlluminationValue_IR.Text = hScrollBar_RecipeVision_Illuminator_IR.Value.ToString();
                //1:Red-High Mag, 2:IR-High Mag, 3:IR-Low Mag
                CommonModule.Instance.Illuminator.SetVolume(this.hScrollBar_RecipeVision_Illuminator_IR.Value, 2);
            }

            this.textBox_RecipeVision_IlluminationValue_IR.Refresh();
            this.textBox_RecipeVision_IlluminationValue_Red.Refresh();
        }

        private void hScrollBarIlluminator_ValueChanged_Red(object sender, System.EventArgs e)
        {
            if(radioButton_RecipeVision_CameraSelection_HighMag.Checked)
            {
                //0:Red-High Mag, 1:IR-High Mag, 2:IR-Low Mag
                workStage.Config.ListIlluminationChannel[0].Value = hScrollBar_RecipeVision_Illuminator_Red.Value;
                this.textBox_RecipeVision_IlluminationValue_Red.Text = hScrollBar_RecipeVision_Illuminator_Red.Value.ToString();
                //1:Red-High Mag, 2:IR-High Mag, 3:IR-Low Mag
                CommonModule.Instance.Illuminator.SetVolume(this.hScrollBar_RecipeVision_Illuminator_Red.Value, 1);
            }
            
            this.textBox_RecipeVision_IlluminationValue_Red.Refresh();
        }

        private void SetScroll()
        {
            //Channel 0: Red - high, 1: IR - high, 2: IR - Low
            if (radioButton_RecipeVision_CameraSelection_LowMag.Checked)
            {
                hScrollBar_RecipeVision_Illuminator_Red.Minimum = (int)workStage.Config.ListIlluminationChannel[0].Min;
                hScrollBar_RecipeVision_Illuminator_Red.Maximum = (int)workStage.Config.ListIlluminationChannel[0].Max;
                hScrollBar_RecipeVision_Illuminator_Red.Value = Equipment.stVisionRecipeSet.nFiduciallluminationRed;
                baseLabel_RecipeVision_Min_Red.Text = hScrollBar_RecipeVision_Illuminator_Red.Minimum.ToString();
                baseLabel_RecipeVision_Max_Red.Text = hScrollBar_RecipeVision_Illuminator_Red.Maximum.ToString();

                hScrollBar_RecipeVision_Illuminator_IR.Minimum = (int)workStage.Config.ListIlluminationChannel[2].Min;
                hScrollBar_RecipeVision_Illuminator_IR.Maximum = (int)workStage.Config.ListIlluminationChannel[2].Max;
                hScrollBar_RecipeVision_Illuminator_IR.Value = Equipment.stVisionRecipeSet.nPreAlignlluminationIR;
                baseLabel_RecipeVision_Min_IR.Text = hScrollBar_RecipeVision_Illuminator_IR.Minimum.ToString();
                baseLabel_RecipeVision_Max_IR.Text = hScrollBar_RecipeVision_Illuminator_IR.Maximum.ToString();
            }
            else if (radioButton_RecipeVision_CameraSelection_HighMag.Checked)
            {
                hScrollBar_RecipeVision_Illuminator_Red.Minimum = (int)workStage.Config.ListIlluminationChannel[0].Min;
                hScrollBar_RecipeVision_Illuminator_Red.Maximum = (int)workStage.Config.ListIlluminationChannel[0].Max;
                hScrollBar_RecipeVision_Illuminator_Red.Value = Equipment.stVisionRecipeSet.nFiduciallluminationRed;
                baseLabel_RecipeVision_Min_Red.Text = hScrollBar_RecipeVision_Illuminator_Red.Minimum.ToString();
                baseLabel_RecipeVision_Max_Red.Text = hScrollBar_RecipeVision_Illuminator_Red.Maximum.ToString();

                hScrollBar_RecipeVision_Illuminator_IR.Minimum = (int)workStage.Config.ListIlluminationChannel[1].Min;
                hScrollBar_RecipeVision_Illuminator_IR.Maximum = (int)workStage.Config.ListIlluminationChannel[1].Max;
                hScrollBar_RecipeVision_Illuminator_IR.Value = Equipment.stVisionRecipeSet.nFiduciallluminationIR;
                baseLabel_RecipeVision_Min_IR.Text = hScrollBar_RecipeVision_Illuminator_IR.Minimum.ToString();
                baseLabel_RecipeVision_Max_IR.Text = hScrollBar_RecipeVision_Illuminator_IR.Maximum.ToString();
            }
            else
            {
                // 말이 되냐?
            }
            hScrollBar_RecipeVision_Illuminator_Red.Refresh();
            hScrollBar_RecipeVision_Illuminator_IR.Refresh();
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
        }

        private void button_RecipeVision_Search_Click(object sender, EventArgs e)
        {
            UpdataPositionData(0.0, 0.0, 0.0);

            if (ImageViewer_RecipeVision_Lows.Simulated)
            {
                Owner.Simulated = true;
                Owner.Camera.LatestImage = ImageViewer_RecipeVision_Lows.InputImage;
                Owner.TestImage = ImageViewer_RecipeVision_Lows.InputImage;
            }

            if (radioButton_RecipeVision_Black.Checked)
            {
                Equipment.stVisionRecipeSet.bCircleDetectionColor = true;
            }
            else if (radioButton_RecipeVision_White.Checked)
            {
                Equipment.stVisionRecipeSet.bCircleDetectionColor = false;
            }
            if (radioButton_RecipeVision_Pattern.Checked)
            {
                stVisionRecipeSet.TrainRoiStartLocation = RoiTrain.Parameter.StartLocation;
                stVisionRecipeSet.TrainRoiEndLocation = RoiTrain.Parameter.EndLocation;
                stVisionRecipeSet.InspectRoiStartLocation = RoiInspect.Parameter.StartLocation;
                stVisionRecipeSet.InspectRoiEndLocation = RoiInspect.Parameter.EndLocation;
                stVisionRecipeSet.nPreAlignlluminationIR = hScrollBar_RecipeVision_Illuminator_IR.Value;

                PatternMatchingParameter.MaxTolerance = Equipment.ToDouble(basetextBox_RecipeVision_AngleTolerance.Text);
                PatternMatchingParameter.MaxInstance = Equipment.ToInt(basetextBox_RecipeVision_MaxInstance.Text);
                PatternMatchingParameter.MinTolerance = Equipment.ToDouble(basetextBox_RecipeVision_AngleTolerance.Text) * -1;
                PatternMatchingParameter.MinScore = Equipment.ToDouble(basetextBox_RecipeVision_MinScore.Text);
                PatternMatchingParameter.DuplicateChecked = baseToggleButton_RecipeVision_DuplicateCheck.GetButtonStatus();
                PatternMatchingParameter.UseMaskImage = baseToggleButton_RecipeVision_UseMaskImage.GetButtonStatus();
                PatternMatchingParameter.TrainImage = pictureBox_RecipeVision_TrainImage.Image;

                stVisionRecipeSet.PatternMatching = PatternMatchingParameter;

                UpdateOwnerRecipe(stVisionRecipeSet);

                PatternMatchingResult result = Owner.GetResult();
                if (result != null)
                {
                    ImageViewer_RecipeVision_Lows.ResultOverlays.Clear();
                }

                result = Owner.Search();
                if (result != null)
                {
                    foreach (var overlay in result.ResultOverlays)
                    {
                        ImageViewer_RecipeVision_Lows.ResultOverlays.Add(overlay);
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
                ImageViewer_RecipeVision_Lows.Display();
            }
            else if(radioButton_RecipeVision_Blob.Checked)
            {
                Equipment.stVisionRecipeSet.dCircleSpec = Convert.ToDouble(textBox_RecipeVision_Circle_Spec.Text);
                Equipment.stVisionRecipeSet.dCircleDetectionSizeW = Convert.ToDouble(textBox_RecipeVision_Circle_Size.Text);
                double dspec = Convert.ToDouble(textBox_RecipeVision_Circle_Spec.Text);    //0.5; //Spec Param 만들어야됨.
                double dRadius = Convert.ToDouble(textBox_RecipeVision_Circle_Size.Text);    //0.5; //Size Param 만들어야됨.
                bool bIsDarkCircleSearch = radioButton_RecipeVision_Black.Checked;
                if(radioButton_RecipeVision_Black.Checked)
                {
                    bIsDarkCircleSearch = true;
                }
                else
                {
                    bIsDarkCircleSearch = false;
                }
                
                PatternMatchingResult SearchResult = null;
                XyCoordinate PointCoordinate = new XyCoordinate();

                ImageViewer_RecipeVision_Lows.ResultOverlays.Clear();

                Owner.FindCircleDetection(dRadius, bIsDarkCircleSearch, dspec, out SearchResult, out PointCoordinate);
                if (SearchResult != null)
                {
                    foreach (var overlay in SearchResult.ResultOverlays)
                    {
                        ImageViewer_RecipeVision_Lows.ResultOverlays.Add(overlay);
                        overlay.Visible = true;
                    }
                }

                if (SearchResult != null && SearchResult.Values.Count > 0)
                {
                    if (IsPixel == true)
                    {
                        UpdataPositionData(SearchResult.Values[0].X, SearchResult.Values[0].Y, SearchResult.Values[0].R);
                    }
                }
                ImageViewer_RecipeVision_Lows.Display();
            }

            Owner.Camera.StartLive();
        }

        private void UpdateOwnerRecipe(VisionRecipeData data)
        {
            if (Owner is JigAligner)
            {
                JigAligner visionCompensator = Owner as JigAligner;
                visionCompensator.Recipe.PatternMatchingParameter = data.PatternMatching;
                visionCompensator.Recipe.TrainRoiStartLocation = data.TrainRoiStartLocation;
                visionCompensator.Recipe.TrainRoiEndLocation = data.TrainRoiEndLocation;
                visionCompensator.Recipe.InspectRoiStartLocation = data.InspectRoiStartLocation;
                visionCompensator.Recipe.InspectRoiEndLocation = data.InspectRoiEndLocation;
                Owner = visionCompensator;
                Equipment.SaveRecipe();
            }
        }

        //private void UpdateOwnerRecipe(PatternMatchingParameters parameters)
        //{
        //    if (Owner is JigAligner)
        //    {
        //        JigAligner visionCompensator = Owner as JigAligner;
        //        visionCompensator.Recipe.PatternMatchingParameter = parameters;
        //        Owner = visionCompensator;
        //    }
        //    Equipment.SaveRecipe();
        //}

        private void checkBox_RecipeVision_CirclePos_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button_RecipeVision_Vision_Save_Click(object sender, EventArgs e)
        {
            bool bRtn = false;

            //Socket
            Equipment.stVisionRecipeSet.Miscellaneous_FiducialAlignType = comboBox_Recipe_Fiducial_Miscellaneous_FiducialAlignType.SelectedIndex;
            Equipment.stVisionRecipeSet.Miscellaneous_FiducialMarkType = comboBox_Recipe_Fiducial_Miscellaneous_FiducialMarkType.SelectedIndex;
            Equipment.stVisionRecipeSet.Miscellaneous_FiducialMarkRadius = Convert.ToDouble(textBox_Recipe_Fiducial_CircleSize.Text);
            Equipment.stVisionRecipeSet.Miscellaneous_FiducialMarkSpec = Convert.ToDouble(textBox_Recipe_Fiducial_CircleSpec.Text);

            //PreAlign
            Equipment.stVisionRecipeSet.PatternMatching = PatternMatchingParameter;
            Equipment.stVisionRecipeSet.TrainRoiStartLocation = RoiTrain.Parameter.StartLocation;
            Equipment.stVisionRecipeSet.TrainRoiEndLocation = RoiTrain.Parameter.EndLocation;
            Equipment.stVisionRecipeSet.InspectRoiStartLocation = RoiInspect.Parameter.StartLocation;
            Equipment.stVisionRecipeSet.InspectRoiEndLocation = RoiInspect.Parameter.EndLocation;
            //Equipment.stVisionRecipeSet.TrainImagePath = Equipment.stVisionRecipeSet.TrainImagePath;
            Equipment.stVisionRecipeSet.SaveTrainImage(pictureBox_RecipeVision_TrainImage.Image);

            if (this.radioButton_RecipeVision_Pattern.Checked)
            {
                Equipment.stVisionRecipeSet.AlgorithmType = VisionAlgorithmType.PatternMatching;
            }
            else if (this.radioButton_RecipeVision_Blob.Checked)
            {
                Equipment.stVisionRecipeSet.AlgorithmType = VisionAlgorithmType.CircleDetection;
            }

            if (this.radioButton_RecipeVision_Cross.Checked)
            {
                Equipment.stVisionRecipeSet.PatternShape = PatternShapeType.Cross;
            }
            else if (this.radioButton_RecipeVision_Circle.Checked)
            {
                Equipment.stVisionRecipeSet.PatternShape = PatternShapeType.Circle;
            }

            if(radioButton_RecipeVision_Black.Checked)
            {
                Equipment.stVisionRecipeSet.bCircleDetectionColor = true;
            }
            else if (radioButton_RecipeVision_White.Checked)
            {
                Equipment.stVisionRecipeSet.bCircleDetectionColor = false;
            }

            Equipment.stVisionRecipeSet.dCircleDetectionSizeW = Convert.ToDouble(textBox_RecipeVision_Circle_Size.Text);
            Equipment.stVisionRecipeSet.dCircleSpec = Convert.ToDouble(textBox_RecipeVision_Circle_Spec.Text);

            //Illuminator
            //if (radioButton_RecipeVision_CameraSelection_LowMag.Checked)
            //{
            //    //Equipment.stVisionRecipeSet.nPreAlignlluminationIR = hScrollBar_RecipeVision_Illuminator_IR.Value;
            //    Equipment.stVisionRecipeSet.nPreAlignlluminationIR = workStage.Config.ListIlluminationChannel[2].Value;
            //}
            //else if (radioButton_RecipeVision_CameraSelection_HighMag.Checked)
            //{
            //    //Equipment.stVisionRecipeSet.nFiduciallluminationIR = hScrollBar_RecipeVision_Illuminator_IR.Value;
            //    Equipment.stVisionRecipeSet.nFiduciallluminationIR = workStage.Config.ListIlluminationChannel[1].Value;
            //    //Equipment.stVisionRecipeSet.nFiduciallluminationRed = hScrollBar_RecipeVision_Illuminator_Red.Value;
            //    Equipment.stVisionRecipeSet.nFiduciallluminationRed = workStage.Config.ListIlluminationChannel[0].Value;
            //}

            Equipment.stVisionRecipeSet.nFiduciallluminationRed = Equipment.ToInt(textBox_Recipe_RecipeVision_Illuminator_FineCamRed.Text);// workStage.Config.ListIlluminationChannel[0].Value;
            Equipment.stVisionRecipeSet.nFiduciallluminationIR = Equipment.ToInt(textBox_Recipe_RecipeVision_Illuminator_FineCamIR.Text);//workStage.Config.ListIlluminationChannel[1].Value;
            Equipment.stVisionRecipeSet.nPreAlignlluminationIR = Equipment.ToInt(textBox_Recipe_RecipeVision_Illuminator_CoarseCamIR.Text);//workStage.Config.ListIlluminationChannel[2].Value;

            //Equipment.stVisionRecipeSet.SaveToIni(Equipment.Current_Recipe);
            if (Equipment.stVisionRecipeSet.SaveToIni(Equipment.Current_Recipe))
            {
                bRtn = true; //Ok.
                UpdateOwnerRecipe(Equipment.stVisionRecipeSet);
            }
            else
            {
                var mb = new MessageBoxOk();
                mb.ShowDialog("Error!", "Data가 저장되지 않았습니다. 레시피를 불러온 후 진행 바랍니다.");
            }
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


        private void radioButton_RecipeVision_CameraSelection_LowMag_CheckedChanged(object sender, EventArgs e)
        {
            CommonModule.Instance.Illuminator.TurnOnOff(true, 1);       //  Fine Cam Red 조명
            Thread.Sleep(1);
            CommonModule.Instance.Illuminator.TurnOnOff(true, 2);       //  Fine Cam IR 조명
            Thread.Sleep(1);
            CommonModule.Instance.Illuminator.TurnOnOff(false, 3);      //  Coarse Cam IR 조명은 일단 Off (Coarse Cam 으로 얼라인을 할 때만 켜도록 한다)

            hScrollBar_RecipeVision_Illuminator_IR.Enabled = true;
            textBox_RecipeVision_IlluminationValue_IR.Enabled = true;
            button_RecipeVision_Illumin_value_IR.Enabled = true;
            baseLabel_RecipeVision_Max_IR.Enabled = true;
            baseLabel_RecipeVision_Min_IR.Enabled = true;
            label_RecipeVision_Light_IR.Enabled = true;

            hScrollBar_RecipeVision_Illuminator_IR.Value = Equipment.stVisionRecipeSet.nPreAlignlluminationIR;

            hScrollBar_RecipeVision_Illuminator_Red.Enabled = false;
            textBox_RecipeVision_IlluminationValue_Red.Enabled = false;
            button_RecipeVision_Illumin_value_Red.Enabled = false;
            baseLabel_RecipeVision_Max_Red.Enabled = false;
            baseLabel_RecipeVision_Min_Red.Enabled = false;
            label_RecipeVision_Light_Red.Enabled = false;

            SetScroll();
        }

        private void radioButton_RecipeVision_CameraSelection_HighMag_CheckedChanged(object sender, EventArgs e)
        {
            CommonModule.Instance.Illuminator.TurnOnOff(true, 1);       //  Fine Cam Red 조명
            Thread.Sleep(1);
            CommonModule.Instance.Illuminator.TurnOnOff(true, 2);       //  Fine Cam IR 조명
            Thread.Sleep(1);
            CommonModule.Instance.Illuminator.TurnOnOff(false, 3);      //  Coarse Cam IR 조명은 일단 Off (Coarse Cam 으로 얼라인을 할 때만 켜도록 한다)

            hScrollBar_RecipeVision_Illuminator_IR.Enabled = true;
            textBox_RecipeVision_IlluminationValue_IR.Enabled = true;
            button_RecipeVision_Illumin_value_IR.Enabled = true;
            baseLabel_RecipeVision_Max_IR.Enabled = true;
            baseLabel_RecipeVision_Min_IR.Enabled = true;
            label_RecipeVision_Light_IR.Enabled = true;

            hScrollBar_RecipeVision_Illuminator_Red.Enabled = true;
            textBox_RecipeVision_IlluminationValue_Red.Enabled = true;
            button_RecipeVision_Illumin_value_Red.Enabled = true;
            baseLabel_RecipeVision_Max_Red.Enabled = true;
            baseLabel_RecipeVision_Min_Red.Enabled = true;
            label_RecipeVision_Light_Red.Enabled = true;

            hScrollBar_RecipeVision_Illuminator_IR.Value = Equipment.stVisionRecipeSet.nFiduciallluminationIR;
            hScrollBar_RecipeVision_Illuminator_Red.Value = Equipment.stVisionRecipeSet.nFiduciallluminationRed;

            SetScroll();
        }

        private void button_Recipe_Fiducial_Search_Click(object sender, EventArgs e)
        {
            //  원 찾기
            bool bFindCircle = false;
            int nImage_Width = 0;
            int nImage_Height = 0;
            double dTargetSize_Radius = 0.0;
            int nTargetColor = 0;
            double dSpec = 0.0;

            if (textBox_Recipe_Fiducial_CircleSize.Text.Length < 0)
            {
                MessageBox.Show("Fiducial Size 를 입력하세요.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            dSpec = Equipment.ToDouble(textBox_Recipe_Fiducial_CircleSpec.Text); //  Fiducial 마크 Spec
            dTargetSize_Radius = Equipment.ToDouble(textBox_Recipe_Fiducial_CircleSize.Text); //  Fiducial 마크 크기
            nTargetColor = comboBox_Recipe_Fiducial_CicleColor.SelectedIndex;          //  Fiducial 마크 색깔 //  0: Black, 1: White

            //detectedCircles.Clear();
            QMC_ImageProcessFindAlign aligner = new QMC_ImageProcessFindAlign();
            List<RectangleF> circlesResult = new List<RectangleF>();
            {
                int w = workStage.Camera_HighRes.Resolution.Width;
                int h = workStage.Camera_HighRes.Resolution.Height;
                nImage_Width = w;
                nImage_Height = h;
                double m_dradius = 0.0;
                m_dradius = dTargetSize_Radius / workStage.Config.ParamConfig.UpperVision_Scale_X;

                // Bitmap을 byte 배열로 변환
                //byte[] pixelData = aligner.ConvertBitmapToByteArray(bm_Temp);      
                //aligner.FindCirclesWidthCircleBoundary(circlesResult, pixelData, w, h);
                //aligner.FindCirclesWidthCircleBoundary(circlesResult, 
                //                                    bm_RawData, w, h, (int)m_dradius, 0.08, 
                //                                    ref bFindCircle, 0, 0, nTargetColor == 0);
                aligner.FindCirclesWidthCircleBoundary(circlesResult,
                                                    workStage.Camera_HighRes.LatestImage.RawData, 
                                                    w, h, (int)m_dradius, dSpec,
                                                    ref bFindCircle, 0, 0, nTargetColor == 0);
            }

            if (bFindCircle && (circlesResult.Count > 0))
            {
                //detectedCircles.Clear();
                double dCenterPosX = 0.0, dCenterPosY = 0.0;
                //  좌표 표시
                listBox_Recipe_Fiducial_Result.Items.Clear();
                for (int i = 0; i < circlesResult.Count; i++)
                {
                    listBox_Recipe_Fiducial_Result.Items.Add((i + 1) + ".  Left-Top X : " + circlesResult[i].X);
                    listBox_Recipe_Fiducial_Result.Items.Add((i + 1) + ".  Left-Top Y : " + circlesResult[i].Y);
                    listBox_Recipe_Fiducial_Result.Items.Add((i + 1) + ".  Width : " + circlesResult[i].Width);
                    listBox_Recipe_Fiducial_Result.Items.Add((i + 1) + ".  Height : " + circlesResult[i].Height);

                    dCenterPosX = circlesResult[i].X + (circlesResult[i].Width / 2);
                    dCenterPosY = circlesResult[i].Y + (circlesResult[i].Height / 2);
                    listBox_Recipe_Fiducial_Result.Items.Add((i + 1) + ".  Center X : " + dCenterPosX);
                    listBox_Recipe_Fiducial_Result.Items.Add((i + 1) + ".  Center Y: " + dCenterPosY);
                }

                foreach (var circle in circlesResult)
                {
                    float ratioX = (float)ImageViewer_RecipeVision_highs.Width / nImage_Width;
                    float ratioY = (float)ImageViewer_RecipeVision_highs.Height / nImage_Height;
                    float ratio = Math.Min(ratioX, ratioY);
                    int newX = (int)(circle.X * ratio);
                    int newY = (int)(circle.Y * ratio);
                    int newWidth = (int)(circle.Width * ratio);
                    int newHeight = (int)(circle.Height * ratio);
                    //detectedCircles.Add(new Rectangle(newX, newY, newWidth, newHeight));
                    //ImageViewer_RecipeVision_highs.Invalidate(); // PictureBox를 다시 그리도록 요청 오버레이로 넘겨야 할듯.
                }
            }
            else
            {
                MessageBox.Show("원 찾기 실패", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //detectedCircles.Clear();
                listBox_Recipe_Fiducial_Result.Items.Clear();
            }

            //  원 찾기 후 다시 Live
            if (workStage.Camera_HighRes.Opened)
            {
                workStage.Camera_HighRes.StartLive();
            }
        }
    }
}
