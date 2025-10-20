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
using QMC.Common.Recipe;
using OpenCvSharp.Dnn;

namespace SLD200.NewStyleForm.NewSubForm
{
    public partial class FormNewSub_Recipe_Vision : UserControl
    {
        private bool m_bFormVisible = false; // 실제 Show 상태 여부
        public bool m_bInitialized = false;

        static WorkStage workStage;
        static Vision vision;
        static JigAligner Owner;

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
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.UpdateStyles();

            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;
            foreach (Module module in m_collectionModules)
            {
                if (module.Name == "WorkStage")
                {
                    workStage = module as WorkStage;
                    Owner = workStage.jigAligner_LowRes;
                }
                else if (module.Name == "Vision")
                {
                    vision = module as Vision;
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // 엔터 또는 스페이스 키 눌렀을 때 무시
            if (keyData == Keys.Enter || keyData == Keys.Space)
                return true;

            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void FormNewSub_Recipe_Vision_Load(object sender, EventArgs e)
        {
            if (m_bInitialized)
                return;
            
            //GUI생성 완료 후 Data 및 Cintroller 업데이트!
            if (this.ImageViewer_RecipeVision_highs.IsHandleCreated)
            {
                this.ImageViewer_RecipeVision_highs.SizeMode = PictureBoxSizeMode.CenterImage;
                this.ImageViewer_RecipeVision_highs.SuspendDisplay();
                this.ImageViewer_RecipeVision_highs.Camera = workStage.jigAligner_HighRes.Camera; //Owner.Camera;
            }

            if (this.ImageViewer_RecipeVision_Lows.IsHandleCreated)
            {
                this.ImageViewer_RecipeVision_Lows.SizeMode = PictureBoxSizeMode.CenterImage;
                this.ImageViewer_RecipeVision_Lows.SuspendDisplay();
                this.ImageViewer_RecipeVision_Lows.Camera = Owner.Camera;
            }
            workStage.UpdateResultOveray += OnUpdateResultOverlay;

            RecipeVisionTimer = new System.Windows.Forms.Timer();
            RecipeVisionTimer.Interval = 200; // 200ms 간격으로 상태 확인
            RecipeVisionTimer.Tick += RecipeVisionTimer_Tick;
            RecipeVisionTimer.Start();

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

            InitSocketMarkCombo();
            InitPreAlignMarkCombo();

            InitRecipeUI_KeyPad();

            m_bInitialized = true;
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
                    ImageViewer_RecipeVision_Lows.ResultOverlays = workStage.CoarseCamResultOveray;
                }
            }
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
            //  Work Stage Position
            label_RecipeVision_EncPosition_STAGE_X.Text = string.Format("{0:F3}", workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X));
            label_RecipeVision_EncPosition_STAGE_Y.Text = string.Format("{0:F3}", workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y));
            
            //  Scanner & Camera Position
            label_RecipeVision_EncPosition_SCANNER_Z.Text = string.Format("{0:F3}", workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Z));

            Motion_Status(); // 기존에 있던 리미트 감지 및 색상 갱신 함수 호출

        }

        public void OnShow()
        {
            //if (m_bFormVisible)
            //    return;
            //m_bFormVisible = true;

            InitPatternMatchingParameter();

            this.ImageViewer_RecipeVision_highs.ResumeDisplay();
            this.ImageViewer_RecipeVision_highs.StartUpdateTask();

            this.ImageViewer_RecipeVision_Lows.ResumeDisplay();
            this.ImageViewer_RecipeVision_Lows.StartUpdateTask();

            this.RecipeVisionTimer.Start();

            // === 현재 Recipe의 Vision 데이터 UI 반영 ===
            // 콤보 초기화 및 선택
            InitSocketMarkCombo();
            InitPreAlignMarkCombo();

            // 콤보에서 선택된 SocketMark를 UI에 표시
            this.ApplySocketMarkToUI();
            this.ApplyPreAlignMarkToUI();
        }

        public void OnHide()
        {
            //if (!m_bFormVisible)
            //    return;
            //m_bFormVisible = false;

            this.ImageViewer_RecipeVision_highs.SuspendDisplay();
            this.ImageViewer_RecipeVision_highs.StopUpdateTask();

            this.ImageViewer_RecipeVision_Lows.SuspendDisplay();
            this.ImageViewer_RecipeVision_Lows.StopUpdateTask();

            this.RecipeVisionTimer.Stop();
        }


        private void InitPatternMatchingParameter()
        {
            try
            {
                // Socket Align
                if (Equipment.stVisionRecipeSet.nSocketAlignType == 0)
                {
                    radioButton_Fiducial_Pattern.Checked = true;
                    radioButton_Fiducial_Circle.Checked = false;
                }
                else
                {
                    radioButton_Fiducial_Pattern.Checked = false;
                    radioButton_Fiducial_Circle.Checked = true;

                }

                if (Equipment.stVisionRecipeSet.nSocketMarkType == 0)
                {
                    radioButton_Fiducial_Type_Circle.Checked = true;
                    radioButton_Fiducial_Type_GoldPowder.Checked = false;
                }
                else
                {
                    radioButton_Fiducial_Type_Circle.Checked = false;
                    radioButton_Fiducial_Type_GoldPowder.Checked = true;
                }

                if (Equipment.stVisionRecipeSet.nSocketCircleColor == 0)
                {
                    radioButton_Fiducial_Black.Checked = true;
                    radioButton_Fiducial_White.Checked = false;
                    radioButton_Fiducial_Ignor.Checked = false;
                }
                else if(Equipment.stVisionRecipeSet.nSocketCircleColor == 1)
                {
                    radioButton_Fiducial_Black.Checked = false;
                    radioButton_Fiducial_White.Checked = true;
                    radioButton_Fiducial_Ignor.Checked = false;
                }
                else if (Equipment.stVisionRecipeSet.nSocketCircleColor == 2)
                {
                    radioButton_Fiducial_Black.Checked = false;
                    radioButton_Fiducial_White.Checked = false;
                    radioButton_Fiducial_Ignor.Checked = true;
                    
                }
                else
                {
                    radioButton_Fiducial_Black.Checked = true;
                    radioButton_Fiducial_White.Checked = false;
                    radioButton_Fiducial_Ignor.Checked = false;
                }
                textBox_Recipe_Fiducial_CircleSize.Text = Equipment.stVisionRecipeSet.dSocketCircleMarkRadius.ToString();

                //textBox_Recipe_Fiducial_CircleSpec.Text = Equipment.stVisionRecipeSet.dSocketCircleMarkSpec.ToString();
                //textBox_Recipe_Fiducial_CircleScore.Text = Equipment.stVisionRecipeSet.dSocketCircleMarkScore.ToString();
                // 내부 값 (0~1)을 퍼센트 문자열로 표시
                textBox_Recipe_Fiducial_CircleSpec.Text =
                    (Equipment.stVisionRecipeSet.dSocketCircleMarkSpec * 100).ToString("F2");
                // 내부 값 (0~1)을 퍼센트 문자열로 표시
                textBox_Recipe_Fiducial_CircleScore.Text =
                    (Equipment.stVisionRecipeSet.dSocketCircleMarkScore * 100).ToString("F2");


                checkBox_RecipeVision_Illuminator_Red.Checked = Equipment.stVisionRecipeSet.bSocketIlluminationRedUse;
                checkBox_RecipeVision_Illuminator_IR.Checked = Equipment.stVisionRecipeSet.bSocketIlluminationIRUse;

                textBox_RecipeVision_Camera_ExposureTime_High.Text = Equipment.stVisionRecipeSet.dSocketIlluminationExposureTime.ToString();

                textBox_RecipeVision_AxisZ_Setting.Text = Equipment.stVisionRecipeSet.dSocketAxisZ_Offset.ToString();

                // Pre Align
                if (Owner.Recipe != null)
                {
                    VisionImage visionImage = Owner.Recipe.PatternMatchingParameter.TrainImage;
                    if (visionImage != null)
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
                    if (Equipment.stVisionRecipeSet.PrePatternMatching != null)
                    {
                        basetextBox_RecipeVision_AngleTolerance.Text = Equipment.stVisionRecipeSet.PrePatternMatching.MaxTolerance.ToString();
                        basetextBox_RecipeVision_MaxInstance.Text = Equipment.stVisionRecipeSet.PrePatternMatching.MaxInstance.ToString();
                        basetextBox_RecipeVision_MinScore.Text = Equipment.stVisionRecipeSet.PrePatternMatching.MinScore.ToString();
                        baseToggleButton_RecipeVision_DuplicateCheck.UpdateToggleStatus(Equipment.stVisionRecipeSet.PrePatternMatching.DuplicateChecked);
                        baseToggleButton_RecipeVision_UseMaskImage.UpdateToggleStatus(Equipment.stVisionRecipeSet.PrePatternMatching.UseMaskImage);

                        if (Equipment.stVisionRecipeSet.LoadTrainImage() != null)
                        {
                            if (Equipment.stVisionRecipeSet.LoadTrainImage().GetImage() != null)
                            {
                                pictureBox_RecipeVision_TrainImage.Image = Equipment.stVisionRecipeSet.LoadTrainImage().GetImage();
                            }
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

                    RoiTrain.Parameter.StartLocation = Equipment.stVisionRecipeSet.pointPreTrainRoiStartLocation;
                    RoiTrain.Parameter.EndLocation = Equipment.stVisionRecipeSet.pointPreTrainRoiEndLocation;
                    RoiInspect.Parameter.StartLocation = Equipment.stVisionRecipeSet.pointPreInspectRoiStartLocation;
                    RoiInspect.Parameter.EndLocation = Equipment.stVisionRecipeSet.pointPreInspectRoiEndLocation;

                    Owner.Recipe.InspectRoiStartLocation = Equipment.stVisionRecipeSet.pointPreInspectRoiStartLocation; //RoiInspect.Parameter.StartLocation;
                    Owner.Recipe.InspectRoiEndLocation = Equipment.stVisionRecipeSet.pointPreInspectRoiEndLocation;     //RoiInspect.Parameter.EndLocation;
                    Owner.Recipe.TrainRoiStartLocation = Equipment.stVisionRecipeSet.pointPreTrainRoiStartLocation;     //RoiTrain.Parameter.StartLocation;
                    Owner.Recipe.TrainRoiEndLocation = Equipment.stVisionRecipeSet.pointPreTrainRoiEndLocation;         //RoiTrain.Parameter.EndLocation;
                }

                if (Equipment.stVisionRecipeSet.ePreAlgorithmType == VisionAlgorithmType.PatternMatching)
                {
                    radioButton_RecipeVision_Pattern.Checked = true;
                    radioButton_RecipeVision_Blob.Checked = false;
                }
                else
                {
                    radioButton_RecipeVision_Pattern.Checked = false;
                    radioButton_RecipeVision_Blob.Checked = true;
                }

                if (Equipment.stVisionRecipeSet.ePreMarkType == MarkTypeList.Cross)
                {
                    radioButton_RecipeVision_Type_Cross.Checked = true;
                    radioButton_RecipeVision_Circle.Checked = false;
                }
                else
                {
                    radioButton_RecipeVision_Type_Cross.Checked = false;
                    radioButton_RecipeVision_Circle.Checked = true;
                }

                if (Equipment.stVisionRecipeSet.nPreCircleColor == 0)
                {
                    radioButton_RecipeVision_Black.Checked = true;
                    radioButton_RecipeVision_White.Checked = false;
                    radioButton_RecipeVision_Ignore.Checked = false;
                }
                else if (Equipment.stVisionRecipeSet.nPreCircleColor == 1)
                {
                    radioButton_RecipeVision_Black.Checked = false;
                    radioButton_RecipeVision_White.Checked = true;
                    radioButton_RecipeVision_Ignore.Checked = false;
                }
                else if (Equipment.stVisionRecipeSet.nPreCircleColor == 2)
                {
                    radioButton_RecipeVision_Black.Checked = false;
                    radioButton_RecipeVision_White.Checked = false;
                    radioButton_RecipeVision_Ignore.Checked = true;
                }
                this.textBox_RecipeVision_Circle_Size.Text = Equipment.stVisionRecipeSet.dPreCircleMarkRadius.ToString();
                this.textBox_RecipeVision_Camera_ExposureTime_Low.Text = Equipment.stVisionRecipeSet.dPreAlignIlluminationExposureTime.ToString();

                //this.textBox_RecipeVision_Circle_Spec.Text = Equipment.stVisionRecipeSet.dPreCircleMarkSpec.ToString();
                //this.textBox_RecipeVision_Circle_Score.Text = Equipment.stVisionRecipeSet.dPreCircleMarkScore.ToString();
                // 내부 값 (0~1)을 퍼센트 문자열로 표시
                textBox_RecipeVision_Circle_Spec.Text =
                    (Equipment.stVisionRecipeSet.dPreCircleMarkSpec * 100).ToString("F2");
                // 내부 값 (0~1)을 퍼센트 문자열로 표시
                textBox_RecipeVision_Circle_Score.Text =
                    (Equipment.stVisionRecipeSet.dPreCircleMarkScore * 100).ToString("F2");



                this.radioButton_RecipeVision_Move_MoveMode_Fine.Checked = false;
                this.radioButton_RecipeVision_Move_MoveMode_Coarse.Checked = true;
                this.radioButton_RecipeVision_JogMove_Continuous.Checked = false;
                this.radioButton_RecipeVision_JogMove_Step.Checked = true;

                textBox_Recipe_RecipeVision_Illuminator_FineCamRed.Text = Equipment.stVisionRecipeSet.nSocketIlluminationRed.ToString();
                textBox_Recipe_RecipeVision_Illuminator_FineCamIR.Text = Equipment.stVisionRecipeSet.nSocketIlluminationIR.ToString();
                textBox_Recipe_RecipeVision_Illuminator_CoarseCamIR.Text = Equipment.stVisionRecipeSet.nPreIlluminationIR.ToString();
                textBox_Recipe_RecipeVision_Illuminator_CoarseCamRed.Text = Equipment.stVisionRecipeSet.nPreIlluminationRed.ToString();

                SetScroll();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
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
            else
            {
                //0:Red-High Mag, 1:IR-High Mag, 2:IR-Low Mag
                workStage.Config.ListIlluminationChannel[3].Value = hScrollBar_RecipeVision_Illuminator_Red.Value;
                this.textBox_RecipeVision_IlluminationValue_Red.Text = hScrollBar_RecipeVision_Illuminator_Red.Value.ToString();
                //1:Red-High Mag, 2:IR-High Mag, 3:IR-Low Mag
                CommonModule.Instance.Illuminator.SetVolume(this.hScrollBar_RecipeVision_Illuminator_Red.Value, 4);
            }

                this.textBox_RecipeVision_IlluminationValue_Red.Refresh();
        }

        private void SetScroll()
        {
            //Channel 0: Red - high, 1: IR - high, 2: IR - Low
            if (radioButton_RecipeVision_CameraSelection_LowMag.Checked)
            {
                hScrollBar_RecipeVision_Illuminator_Red.Minimum = (int)workStage.Config.ListIlluminationChannel[3].Min;
                hScrollBar_RecipeVision_Illuminator_Red.Maximum = (int)workStage.Config.ListIlluminationChannel[3].Max;
                hScrollBar_RecipeVision_Illuminator_Red.Value = Equipment.stVisionRecipeSet.nPreIlluminationRed;
                baseLabel_RecipeVision_Min_Red.Text = hScrollBar_RecipeVision_Illuminator_Red.Minimum.ToString();
                baseLabel_RecipeVision_Max_Red.Text = hScrollBar_RecipeVision_Illuminator_Red.Maximum.ToString();

                hScrollBar_RecipeVision_Illuminator_IR.Minimum = (int)workStage.Config.ListIlluminationChannel[2].Min;
                hScrollBar_RecipeVision_Illuminator_IR.Maximum = (int)workStage.Config.ListIlluminationChannel[2].Max;
                hScrollBar_RecipeVision_Illuminator_IR.Value = Equipment.stVisionRecipeSet.nPreIlluminationIR;
                baseLabel_RecipeVision_Min_IR.Text = hScrollBar_RecipeVision_Illuminator_IR.Minimum.ToString();
                baseLabel_RecipeVision_Max_IR.Text = hScrollBar_RecipeVision_Illuminator_IR.Maximum.ToString();
            }
            else if (radioButton_RecipeVision_CameraSelection_HighMag.Checked)
            {
                // 현재 선택된 마크 인덱스 가져오기
                int idx = comboBox_Recipe_Fiducial_MarkIndex.SelectedIndex;
                if (idx < 0 || idx >= Equipment.stVisionRecipeSet.SocketMarkList.Count)
                    return;

                var mark = Equipment.stVisionRecipeSet.SocketMarkList[idx];

                hScrollBar_RecipeVision_Illuminator_Red.Minimum = (int)workStage.Config.ListIlluminationChannel[0].Min;
                hScrollBar_RecipeVision_Illuminator_Red.Maximum = (int)workStage.Config.ListIlluminationChannel[0].Max;
                hScrollBar_RecipeVision_Illuminator_Red.Value = mark.IllumRed;
                baseLabel_RecipeVision_Min_Red.Text = hScrollBar_RecipeVision_Illuminator_Red.Minimum.ToString();
                baseLabel_RecipeVision_Max_Red.Text = hScrollBar_RecipeVision_Illuminator_Red.Maximum.ToString();

                hScrollBar_RecipeVision_Illuminator_IR.Minimum = (int)workStage.Config.ListIlluminationChannel[1].Min;
                hScrollBar_RecipeVision_Illuminator_IR.Maximum = (int)workStage.Config.ListIlluminationChannel[1].Max;
                hScrollBar_RecipeVision_Illuminator_IR.Value = mark.IllumIR;
                baseLabel_RecipeVision_Min_IR.Text = hScrollBar_RecipeVision_Illuminator_IR.Minimum.ToString();
                baseLabel_RecipeVision_Max_IR.Text = hScrollBar_RecipeVision_Illuminator_IR.Maximum.ToString();

                //hScrollBar_RecipeVision_Illuminator_Red.Minimum = (int)workStage.Config.ListIlluminationChannel[0].Min;
                //hScrollBar_RecipeVision_Illuminator_Red.Maximum = (int)workStage.Config.ListIlluminationChannel[0].Max;
                //hScrollBar_RecipeVision_Illuminator_Red.Value = Equipment.stVisionRecipeSet.nSocketIlluminationRed;
                //baseLabel_RecipeVision_Min_Red.Text = hScrollBar_RecipeVision_Illuminator_Red.Minimum.ToString();
                //baseLabel_RecipeVision_Max_Red.Text = hScrollBar_RecipeVision_Illuminator_Red.Maximum.ToString();

                //hScrollBar_RecipeVision_Illuminator_IR.Minimum = (int)workStage.Config.ListIlluminationChannel[1].Min;
                //hScrollBar_RecipeVision_Illuminator_IR.Maximum = (int)workStage.Config.ListIlluminationChannel[1].Max;
                //hScrollBar_RecipeVision_Illuminator_IR.Value = Equipment.stVisionRecipeSet.nSocketIlluminationIR;
                //baseLabel_RecipeVision_Min_IR.Text = hScrollBar_RecipeVision_Illuminator_IR.Minimum.ToString();
                //baseLabel_RecipeVision_Max_IR.Text = hScrollBar_RecipeVision_Illuminator_IR.Maximum.ToString();
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

            // PreAlignMarkList 기반으로 선택 마크를 가져온다
            int preAlignIdx = comboBox_Recipe_PreAlign_MarkIndex.SelectedIndex;
            if (preAlignIdx < 0 || preAlignIdx >= Equipment.stVisionRecipeSet.PreAlignMarkList.Count)
                return;
            var mark = Equipment.stVisionRecipeSet.PreAlignMarkList[preAlignIdx];

            // 컬러 선택
            if (radioButton_RecipeVision_Black.Checked)
                mark.CircleColor = 0;
            else if (radioButton_RecipeVision_White.Checked)
                mark.CircleColor = 1;
            else if (radioButton_RecipeVision_Ignore.Checked)
                mark.CircleColor = 2;

            if (radioButton_RecipeVision_Pattern.Checked)
            {
                // ROI
                mark.TrainRoiStart = RoiTrain.Parameter.StartLocation;
                mark.TrainRoiEnd = RoiTrain.Parameter.EndLocation;
                mark.InspectRoiStart = RoiInspect.Parameter.StartLocation;
                mark.InspectRoiEnd = RoiInspect.Parameter.EndLocation;

                // Illumination
                mark.IllumIR = hScrollBar_RecipeVision_Illuminator_IR.Value;
                mark.IllumRed = hScrollBar_RecipeVision_Illuminator_Red.Value;

                // PatternMatchingParameter (UI → mark)
                mark.PatternMatching.MaxTolerance = Equipment.ToDouble(basetextBox_RecipeVision_AngleTolerance.Text);
                mark.PatternMatching.MaxInstance = Equipment.ToInt(basetextBox_RecipeVision_MaxInstance.Text);
                mark.PatternMatching.MinTolerance = Equipment.ToDouble(basetextBox_RecipeVision_AngleTolerance.Text) * -1;
                mark.PatternMatching.MinScore = Equipment.ToDouble(basetextBox_RecipeVision_MinScore.Text);
                mark.PatternMatching.DuplicateChecked = baseToggleButton_RecipeVision_DuplicateCheck.GetButtonStatus();
                mark.PatternMatching.UseMaskImage = baseToggleButton_RecipeVision_UseMaskImage.GetButtonStatus();
                mark.PatternMatching.TrainImage = pictureBox_RecipeVision_TrainImage.Image;

                // AlgorithmType, MarkType
                mark.AlgorithmType = VisionAlgorithmType.PatternMatching;
                mark.MarkType = radioButton_RecipeVision_Type_Cross.Checked ? MarkTypeList.Cross : MarkTypeList.Circle;

                // 값 업데이트
                UpdateOwnerRecipe(Equipment.stVisionRecipeSet); // 필요 시 이 부분도 mark 단위로 전달

                // Search & Display (기존 로직 유지)
                PatternMatchingResult result = Owner.GetResult();
                if (result != null)
                    ImageViewer_RecipeVision_Lows.ResultOverlays.Clear();

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
                    if (IsPixel)
                        UpdataPositionData(result.Values[0].X, result.Values[0].Y, result.Values[0].R);
                    // (else 변환 좌표 처리 필요시 기존 로직 사용)
                }
                ImageViewer_RecipeVision_Lows.Display();
            }
            else if (radioButton_RecipeVision_Blob.Checked)
            {
                // Circle 파라미터 UI → mark 저장
                mark.CircleMarkRadius = Convert.ToDouble(textBox_RecipeVision_Circle_Size.Text);
                //mark.CircleMarkSpec = Convert.ToDouble(textBox_RecipeVision_Circle_Spec.Text);
                //mark.CircleMarkScore = Convert.ToDouble(textBox_RecipeVision_Circle_Score.Text);
                double percentValue = 0.0;
                if (double.TryParse(textBox_RecipeVision_Circle_Spec.Text, out percentValue))
                {
                    // UI에서 입력받은 %를 내부 0~1 값으로 변환
                    mark.CircleMarkSpec = percentValue / 100.0;
                }
                if (double.TryParse(textBox_RecipeVision_Circle_Score.Text, out percentValue))
                {
                    // UI에서 입력받은 %를 내부 0~1 값으로 변환
                    mark.CircleMarkScore = percentValue / 100.0;
                }

                mark.AlgorithmType = VisionAlgorithmType.CircleDetection;
                mark.MarkType = radioButton_RecipeVision_Type_Cross.Checked ? MarkTypeList.Cross : MarkTypeList.Circle;

                double dspec = mark.CircleMarkSpec;
                double dRadius = mark.CircleMarkRadius;
                double dScore = mark.CircleMarkScore;
                int nIsDarkCircleSearch = mark.CircleColor;

                PatternMatchingResult SearchResult = null;
                XyCoordinate PointCoordinate = new XyCoordinate();

                ImageViewer_RecipeVision_Lows.ResultOverlays.Clear();
                Owner.FindCircleDetection(dRadius, nIsDarkCircleSearch, dspec, dScore, out SearchResult, out PointCoordinate);
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
                    if (IsPixel)
                        UpdataPositionData(SearchResult.Values[0].X, SearchResult.Values[0].Y, SearchResult.Values[0].R);
                }
                ImageViewer_RecipeVision_Lows.Display();
            }

            Owner.Camera.StartLive();
        }

        //private void button_RecipeVision_Search_Click(object sender, EventArgs e)
        //{
        //    UpdataPositionData(0.0, 0.0, 0.0);

        //    if (ImageViewer_RecipeVision_Lows.Simulated)
        //    {
        //        Owner.Simulated = true;
        //        Owner.Camera.LatestImage = ImageViewer_RecipeVision_Lows.InputImage;
        //        Owner.TestImage = ImageViewer_RecipeVision_Lows.InputImage;
        //    }

        //    if (radioButton_RecipeVision_Black.Checked)
        //    {
        //        Equipment.stVisionRecipeSet.nPreCircleColor = 0;
        //    }
        //    else if (radioButton_RecipeVision_White.Checked)
        //    {
        //        Equipment.stVisionRecipeSet.nPreCircleColor = 1;
        //    }
        //    else if (radioButton_RecipeVision_Ignore.Checked)
        //    {
        //        Equipment.stVisionRecipeSet.nPreCircleColor = 2;
        //    }

        //    if (radioButton_RecipeVision_Pattern.Checked)
        //    {
        //        stVisionRecipeSet.pointPreTrainRoiStartLocation = RoiTrain.Parameter.StartLocation;
        //        stVisionRecipeSet.pointPreTrainRoiEndLocation = RoiTrain.Parameter.EndLocation;
        //        stVisionRecipeSet.pointPreInspectRoiStartLocation = RoiInspect.Parameter.StartLocation;
        //        stVisionRecipeSet.pointPreInspectRoiEndLocation = RoiInspect.Parameter.EndLocation;
        //        stVisionRecipeSet.nPreIlluminationIR = hScrollBar_RecipeVision_Illuminator_IR.Value;
        //        stVisionRecipeSet.nPreIlluminationRed = hScrollBar_RecipeVision_Illuminator_Red.Value;

        //        PatternMatchingParameter.MaxTolerance = Equipment.ToDouble(basetextBox_RecipeVision_AngleTolerance.Text);
        //        PatternMatchingParameter.MaxInstance = Equipment.ToInt(basetextBox_RecipeVision_MaxInstance.Text);
        //        PatternMatchingParameter.MinTolerance = Equipment.ToDouble(basetextBox_RecipeVision_AngleTolerance.Text) * -1;
        //        PatternMatchingParameter.MinScore = Equipment.ToDouble(basetextBox_RecipeVision_MinScore.Text);
        //        PatternMatchingParameter.DuplicateChecked = baseToggleButton_RecipeVision_DuplicateCheck.GetButtonStatus();
        //        PatternMatchingParameter.UseMaskImage = baseToggleButton_RecipeVision_UseMaskImage.GetButtonStatus();
        //        PatternMatchingParameter.TrainImage = pictureBox_RecipeVision_TrainImage.Image;

        //        stVisionRecipeSet.PrePatternMatching = PatternMatchingParameter;

        //        UpdateOwnerRecipe(stVisionRecipeSet);

        //        PatternMatchingResult result = Owner.GetResult();
        //        if (result != null)
        //        {
        //            ImageViewer_RecipeVision_Lows.ResultOverlays.Clear();
        //        }

        //        result = Owner.Search();
        //        if (result != null)
        //        {
        //            foreach (var overlay in result.ResultOverlays)
        //            {
        //                ImageViewer_RecipeVision_Lows.ResultOverlays.Add(overlay);
        //                overlay.Visible = true;
        //            }
        //        }

        //        if (result != null && result.Values.Count > 0)
        //        {
        //            if (IsPixel == true)
        //            {
        //                UpdataPositionData(result.Values[0].X, result.Values[0].Y, result.Values[0].R);
        //            }
        //            else
        //            {
        //                //PointD converted = new PointD((result.Values[0].X - this.m_Owner.Camera.Resolution.Width / 2) * ((WorkStage)this.m_Owner.Owner).Scale.X * (((WorkStage)this.m_Owner.Owner).Scale.InvertedX ? 1 : -1),
        //                //                              (result.Values[0].Y - this.m_Owner.Camera.Resolution.Height / 2) * ((WorkStage)this.m_Owner.Owner).Scale.Y * (((WorkStage)this.m_Owner.Owner).Scale.InvertedY ? 1 : -1));

        //                //PointD converted = new PointD((result.Values[0].X - workStage.scannerCompensator.Camera.Resolution.Width / 2) * (workStage.scannerCompensator.Owner).Scale.X * ((workStage.scannerCompensator.Owner).Scale.InvertedX ? 1 : -1),
        //                //                              (result.Values[0].Y - workStage.scannerCompensator.Camera.Resolution.Height / 2) * (workStage.scannerCompensator.Owner).Scale.Y * ((workStage.scannerCompensator.Owner).Scale.InvertedY ? 1 : -1));

        //                //UpdataPositionData(converted.X, converted.Y, result.Values[0].R);
        //            }
        //        }
        //        ImageViewer_RecipeVision_Lows.Display();
        //    }
        //    else if (radioButton_RecipeVision_Blob.Checked)
        //    {
        //        Equipment.stVisionRecipeSet.dPreCircleMarkSpec = Convert.ToDouble(textBox_RecipeVision_Circle_Spec.Text);
        //        Equipment.stVisionRecipeSet.dPreCircleMarkRadius = Convert.ToDouble(textBox_RecipeVision_Circle_Size.Text);
        //        Equipment.stVisionRecipeSet.dPreCircleMarkScore = Convert.ToDouble(textBox_RecipeVision_Circle_Score.Text);

        //        double dspec = Convert.ToDouble(textBox_RecipeVision_Circle_Spec.Text);    //0.5; //Spec Param 만들어야됨.
        //        double dRadius = Convert.ToDouble(textBox_RecipeVision_Circle_Size.Text);    //0.5; //Size Param 만들어야됨.
        //        double dScore = Convert.ToDouble(textBox_RecipeVision_Circle_Score.Text);    //0.5; //Score Param 만들어야됨.
        //        int nIsDarkCircleSearch = 0;
        //        if (radioButton_RecipeVision_Black.Checked)
        //        {
        //            nIsDarkCircleSearch = 0;
        //        }
        //        else if (radioButton_RecipeVision_White.Checked)
        //        {
        //            nIsDarkCircleSearch = 1;
        //        }
        //        else if (radioButton_RecipeVision_Ignore.Checked)
        //        {
        //            nIsDarkCircleSearch = 2;
        //        }

        //        PatternMatchingResult SearchResult = null;
        //        XyCoordinate PointCoordinate = new XyCoordinate();

        //        ImageViewer_RecipeVision_Lows.ResultOverlays.Clear();
        //        Owner.FindCircleDetection(dRadius, nIsDarkCircleSearch, dspec, dScore, out SearchResult, out PointCoordinate);
        //        if (SearchResult != null)
        //        {
        //            foreach (var overlay in SearchResult.ResultOverlays)
        //            {
        //                ImageViewer_RecipeVision_Lows.ResultOverlays.Add(overlay);
        //                overlay.Visible = true;
        //            }
        //        }


        //        if (SearchResult != null && SearchResult.Values.Count > 0)
        //        {
        //            if (IsPixel == true)
        //            {
        //                UpdataPositionData(SearchResult.Values[0].X, SearchResult.Values[0].Y, SearchResult.Values[0].R);
        //            }
        //        }
        //        ImageViewer_RecipeVision_Lows.Display();
        //    }

        //    Owner.Camera.StartLive();
        //}

        private void UpdateOwnerRecipe(VisionRecipeData data)
        {
            if (Owner is JigAligner)
            {
                JigAligner visionCompensator = Owner as JigAligner;
                visionCompensator.Recipe.PatternMatchingParameter = data.PrePatternMatching;
                visionCompensator.Recipe.TrainRoiStartLocation = data.pointPreTrainRoiStartLocation;
                visionCompensator.Recipe.TrainRoiEndLocation = data.pointPreTrainRoiEndLocation;
                visionCompensator.Recipe.InspectRoiStartLocation = data.pointPreInspectRoiStartLocation;
                visionCompensator.Recipe.InspectRoiEndLocation = data.pointPreInspectRoiEndLocation;
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
            int selectedIndex = comboBox_Recipe_Fiducial_MarkIndex.SelectedIndex;
            if (selectedIndex < 0)
            {
                MessageBox.Show("선택된 마크가 없습니다.", "저장 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 선택된 인덱스의 마크 가져오기 (부족하면 자동 생성)
            while (Equipment.stVisionRecipeSet.SocketMarkList.Count <= selectedIndex)
            {
                Equipment.stVisionRecipeSet.SocketMarkList.Add(new SocketMarkInfo());
            }

            var mark = Equipment.stVisionRecipeSet.SocketMarkList[selectedIndex];

            // 정렬 알고리즘 종류 선택
            if (radioButton_Fiducial_Pattern.Checked)
                mark.AlignType = (int)VisionAlgorithmType.PatternMatching;
            else if (radioButton_Fiducial_Circle.Checked)
                mark.AlignType = (int)VisionAlgorithmType.CircleDetection;

            // 마크 타입 선택
            if (radioButton_Fiducial_Type_GoldPowder.Checked)
                mark.MarkType = (int)MarkTypeList.GoldPowder;
            else if (radioButton_Fiducial_Type_Circle.Checked)
                mark.MarkType = (int)MarkTypeList.Circle;

            // 마크 색상 선택
            if (radioButton_Fiducial_Black.Checked)
                mark.MarkColor = 0;
            else if (radioButton_Fiducial_White.Checked)
                mark.MarkColor = 1;
            else if (radioButton_Fiducial_Ignor.Checked)
                mark.MarkColor = 2;
            else
                mark.MarkColor = 0;

            // 마크 속성 설정
            mark.MarkRadius = Convert.ToDouble(textBox_Recipe_Fiducial_CircleSize.Text);
            //mark.MarkSpec = Convert.ToDouble(textBox_Recipe_Fiducial_CircleSpec.Text);
            //mark.MarkScore = Convert.ToDouble(textBox_Recipe_Fiducial_CircleScore.Text);
            double percentValue = 0.0;
            if (double.TryParse(textBox_Recipe_Fiducial_CircleSpec.Text, out percentValue))
            {
                // UI에서 입력받은 %를 내부 0~1 값으로 변환
                mark.MarkSpec = percentValue / 100.0;
            }
            if (double.TryParse(textBox_Recipe_Fiducial_CircleScore.Text, out percentValue))
            {
                // UI에서 입력받은 %를 내부 0~1 값으로 변환
                mark.MarkScore = percentValue / 100.0;
            }

            // 조명 설정
            mark.UseRed = checkBox_RecipeVision_Illuminator_Red.Checked;
            mark.UseIR = checkBox_RecipeVision_Illuminator_IR.Checked;
            mark.ExposureTime = Convert.ToDouble(textBox_RecipeVision_Camera_ExposureTime_High.Text);
            mark.AxisZOffset = Convert.ToDouble(textBox_RecipeVision_AxisZ_Setting.Text);
            mark.IllumRed = Equipment.ToInt(textBox_Recipe_RecipeVision_Illuminator_FineCamRed.Text);
            mark.IllumIR = Equipment.ToInt(textBox_Recipe_RecipeVision_Illuminator_FineCamIR.Text);

            SaveSocketMarkFromUI();  // 이 함수 내부에서도 SelectedIndex를 사용해야 일관성 있음

            //PreAlign
            // ======= PreAlign 다중 마크 부분 =======

            // 현재 PreAlign 콤보 인덱스 사용 (※ 콤보박스 이름 다를 수 있음)
            int preAlignIndex = comboBox_Recipe_PreAlign_MarkIndex.SelectedIndex;
            if (preAlignIndex < 0)
            {
                MessageBox.Show("선택된 PreAlign 마크가 없습니다.", "저장 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            while (Equipment.stVisionRecipeSet.PreAlignMarkList.Count <= preAlignIndex)
                Equipment.stVisionRecipeSet.PreAlignMarkList.Add(new PreAlignMarkInfo());

            var markPreAlign = Equipment.stVisionRecipeSet.PreAlignMarkList[preAlignIndex];

            // ROI/조명 등 필요한 값 저장
            markPreAlign.TrainRoiStart = RoiTrain.Parameter.StartLocation;
            markPreAlign.TrainRoiEnd = RoiTrain.Parameter.EndLocation;
            markPreAlign.InspectRoiStart = RoiInspect.Parameter.StartLocation;
            markPreAlign.InspectRoiEnd = RoiInspect.Parameter.EndLocation;
            markPreAlign.IllumIR = Equipment.ToInt(textBox_Recipe_RecipeVision_Illuminator_CoarseCamIR.Text);
            markPreAlign.IllumRed = Equipment.ToInt(textBox_Recipe_RecipeVision_Illuminator_CoarseCamRed.Text);
            markPreAlign.ExposureTime = Equipment.ToDouble(textBox_RecipeVision_Camera_ExposureTime_Low.Text);

            // 알고리즘/마크타입/컬러/서클옵션 등
            markPreAlign.AlgorithmType = radioButton_RecipeVision_Pattern.Checked ? VisionAlgorithmType.PatternMatching : VisionAlgorithmType.CircleDetection;
            markPreAlign.MarkType = radioButton_RecipeVision_Type_Cross.Checked ? MarkTypeList.Cross : MarkTypeList.Circle;

            if (radioButton_RecipeVision_Black.Checked)
                markPreAlign.CircleColor = 0;
            else if (radioButton_RecipeVision_White.Checked)
                markPreAlign.CircleColor = 1;
            else if (radioButton_RecipeVision_Ignore.Checked)
                markPreAlign.CircleColor = 2;
            else
                markPreAlign.CircleColor = 0;

            markPreAlign.CircleMarkRadius = Convert.ToDouble(textBox_RecipeVision_Circle_Size.Text);
            //markPreAlign.CircleMarkSpec = Convert.ToDouble(textBox_RecipeVision_Circle_Spec.Text);
            //markPreAlign.CircleMarkScore = Convert.ToDouble(textBox_RecipeVision_Circle_Score.Text);
            double percentValue2 = 0.0;
            if (double.TryParse(textBox_RecipeVision_Circle_Spec.Text, out percentValue2))
            {
                // UI에서 입력받은 %를 내부 0~1 값으로 변환
                markPreAlign.CircleMarkSpec = percentValue2 / 100.0;
            }
            if (double.TryParse(textBox_RecipeVision_Circle_Score.Text, out percentValue2))
            {
                // UI에서 입력받은 %를 내부 0~1 값으로 변환
                markPreAlign.CircleMarkScore = percentValue2 / 100.0;
            }

            // 패턴매칭 파라미터
            if (markPreAlign.PatternMatching == null)
                markPreAlign.PatternMatching = new PatternMatchingParameters();

            markPreAlign.PatternMatching.MaxTolerance = Equipment.ToDouble(basetextBox_RecipeVision_AngleTolerance.Text);
            markPreAlign.PatternMatching.MaxInstance = Equipment.ToInt(basetextBox_RecipeVision_MaxInstance.Text);
            markPreAlign.PatternMatching.MinTolerance = Equipment.ToDouble(basetextBox_RecipeVision_AngleTolerance.Text) * -1;
            markPreAlign.PatternMatching.MinScore = Equipment.ToDouble(basetextBox_RecipeVision_MinScore.Text);
            markPreAlign.PatternMatching.DuplicateChecked = baseToggleButton_RecipeVision_DuplicateCheck.GetButtonStatus();
            markPreAlign.PatternMatching.UseMaskImage = baseToggleButton_RecipeVision_UseMaskImage.GetButtonStatus();
            markPreAlign.PatternMatching.TrainImage = pictureBox_RecipeVision_TrainImage.Image;

            // 이미지 저장
            Equipment.stVisionRecipeSet.SaveTrainImage(pictureBox_RecipeVision_TrainImage.Image);

            //기존 코드
            {
                //Equipment.stVisionRecipeSet.PrePatternMatching = PatternMatchingParameter;
                //Equipment.stVisionRecipeSet.pointPreTrainRoiStartLocation = RoiTrain.Parameter.StartLocation;
                //Equipment.stVisionRecipeSet.pointPreTrainRoiEndLocation = RoiTrain.Parameter.EndLocation;
                //Equipment.stVisionRecipeSet.pointPreInspectRoiStartLocation = RoiInspect.Parameter.StartLocation;
                //Equipment.stVisionRecipeSet.pointPreInspectRoiEndLocation = RoiInspect.Parameter.EndLocation;
                ////Equipment.stVisionRecipeSet.TrainImagePath = Equipment.stVisionRecipeSet.TrainImagePath;
                //Equipment.stVisionRecipeSet.SaveTrainImage(pictureBox_RecipeVision_TrainImage.Image);
                //if (this.radioButton_RecipeVision_Pattern.Checked)
                //{
                //    Equipment.stVisionRecipeSet.ePreAlgorithmType = VisionAlgorithmType.PatternMatching;
                //}
                //else if (this.radioButton_RecipeVision_Blob.Checked)
                //{
                //    Equipment.stVisionRecipeSet.ePreAlgorithmType = VisionAlgorithmType.CircleDetection;
                //}
                //if (this.radioButton_RecipeVision_Type_Cross.Checked)
                //{
                //    Equipment.stVisionRecipeSet.ePreMarkType = MarkTypeList.Cross;
                //}
                //else if (this.radioButton_RecipeVision_Circle.Checked)
                //{
                //    Equipment.stVisionRecipeSet.ePreMarkType = MarkTypeList.Circle;
                //}
                //if (radioButton_RecipeVision_Black.Checked)
                //{
                //    Equipment.stVisionRecipeSet.nPreCircleColor = 0;
                //}
                //else if (radioButton_RecipeVision_White.Checked)
                //{
                //    Equipment.stVisionRecipeSet.nPreCircleColor = 1;
                //}
                //else if (radioButton_RecipeVision_Ignore.Checked)
                //{
                //    Equipment.stVisionRecipeSet.nPreCircleColor = 2;
                //}
                //else
                //{
                //    Equipment.stVisionRecipeSet.nPreCircleColor = 0;
                //}
                //Equipment.stVisionRecipeSet.dPreCircleMarkRadius = Convert.ToDouble(textBox_RecipeVision_Circle_Size.Text);
                //Equipment.stVisionRecipeSet.dPreCircleMarkSpec = Convert.ToDouble(textBox_RecipeVision_Circle_Spec.Text);
                //Equipment.stVisionRecipeSet.dPreCircleMarkScore = Convert.ToDouble(textBox_RecipeVision_Circle_Score.Text);
                //Equipment.stVisionRecipeSet.nPreIlluminationIR = Equipment.ToInt(textBox_Recipe_RecipeVision_Illuminator_CoarseCamIR.Text);//workStage.Config.ListIlluminationChannel[2].Value;
                //Equipment.stVisionRecipeSet.nPreIlluminationRed = Equipment.ToInt(textBox_Recipe_RecipeVision_Illuminator_CoarseCamRed.Text);//workStage.Config.ListIlluminationChannel[2].Value;
                //Equipment.stVisionRecipeSet.dPreAlignIlluminationExposureTime = Equipment.ToDouble(textBox_RecipeVision_Camera_ExposureTime_Low.Text);

            }


            // ======= 저장 =======
            //Equipment.stVisionRecipeSet.SaveToIni(Equipment.Current_Recipe);
            if (Equipment.stVisionRecipeSet.SaveToIni(Equipment.Current_Recipe))
            {
                bRtn = true; //Ok.

                Equipment.stVisionRecipeSet = VisionRecipeData.LoadFromIni(Equipment.Current_Recipe);
                if (workStage.jigAligner_LowRes != null)
                {
                    workStage.jigAligner_LowRes.Recipe.PatternMatchingParameter.TrainImage = Equipment.stVisionRecipeSet.LoadTrainImage(); //Bitmap.FromFile(m_strFile);
                    workStage.jigAligner_LowRes.TrainImage = Equipment.stVisionRecipeSet.LoadTrainImage(); //이거 사용중.
                }

                if (Equipment.stVisionRecipeSet.PrePatternMatching != null)
                {
                    workStage.jigAligner_LowRes.Recipe.PatternMatchingParameter.MaxTolerance = Equipment.stVisionRecipeSet.PrePatternMatching.MaxTolerance;
                    workStage.jigAligner_LowRes.Recipe.PatternMatchingParameter.MaxInstance = Equipment.stVisionRecipeSet.PrePatternMatching.MaxInstance;
                    workStage.jigAligner_LowRes.Recipe.PatternMatchingParameter.MinScore = Equipment.stVisionRecipeSet.PrePatternMatching.MinScore;
                    workStage.jigAligner_LowRes.Recipe.PatternMatchingParameter.DuplicateChecked = Equipment.stVisionRecipeSet.PrePatternMatching.DuplicateChecked;
                    workStage.jigAligner_LowRes.Recipe.PatternMatchingParameter.UseMaskImage = Equipment.stVisionRecipeSet.PrePatternMatching.UseMaskImage;

                    if (workStage.jigAligner_LowRes.Recipe.PatternMatchingParameter.TrainImage != null)
                    {
                        workStage.jigAligner_LowRes.Recipe.PatternMatchingParameter.TrainImage = Equipment.stVisionRecipeSet.LoadTrainImage().GetImage();
                    }

                    workStage.jigAligner_LowRes.Recipe.TrainRoiStartLocation = Equipment.stVisionRecipeSet.pointPreTrainRoiStartLocation;
                    workStage.jigAligner_LowRes.Recipe.TrainRoiEndLocation = Equipment.stVisionRecipeSet.pointPreTrainRoiEndLocation;
                    workStage.jigAligner_LowRes.Recipe.InspectRoiStartLocation = Equipment.stVisionRecipeSet.pointPreInspectRoiStartLocation;
                    workStage.jigAligner_LowRes.Recipe.InspectRoiEndLocation = Equipment.stVisionRecipeSet.pointPreInspectRoiEndLocation;
                }

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

            //// 공통 MouseDown 핸들러
            //button_RecipeVision_X_Pos.MouseDown += button_RecipeVision_Axis_MouseDown;
            //button_RecipeVision_X_Neg.MouseDown += button_RecipeVision_Axis_MouseDown;
            //button_RecipeVision_Y_Pos.MouseDown += button_RecipeVision_Axis_MouseDown;
            //button_RecipeVision_Y_Neg.MouseDown += button_RecipeVision_Axis_MouseDown;
            //button_RecipeVision_Z_Pos.MouseDown += button_RecipeVision_Axis_MouseDown;
            //button_RecipeVision_Z_Neg.MouseDown += button_RecipeVision_Axis_MouseDown;

            //// 공통 MouseUp 핸들러
            //button_RecipeVision_X_Pos.MouseUp += button_RecipeVision_Axis_MouseUp;
            //button_RecipeVision_X_Neg.MouseUp += button_RecipeVision_Axis_MouseUp;
            //button_RecipeVision_Y_Pos.MouseUp += button_RecipeVision_Axis_MouseUp;
            //button_RecipeVision_Y_Neg.MouseUp += button_RecipeVision_Axis_MouseUp;
            //button_RecipeVision_Z_Pos.MouseUp += button_RecipeVision_Axis_MouseUp;
            //button_RecipeVision_Z_Neg.MouseUp += button_RecipeVision_Axis_MouseUp;
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
                    case "X": axis = (int)WorkStage.nAxis.X; break;
                    case "Y": axis = (int)WorkStage.nAxis.Y; break;
                    case "Z": axis = (int)WorkStage.nAxis.Z; break;
                    default: return;
                }

                // 속도/가감속 설정
                Type_Motor_Speed type_Motor_Speed;
                if (radioButton_RecipeVision_Move_MoveMode_Fine.Checked)
                {
                    type_Motor_Speed = Type_Motor_Speed.Fine;
                }
                else
                {
                    type_Motor_Speed = Type_Motor_Speed.Coarse;
                }
                velocity = Math.Abs(velocity);

                try
                {
                    if (radioButton_RecipeVision_JogMove_Continuous.Checked)
                    {
                        workStage.MovetoWorkStage_Jog_Positions((WorkStage.nAxis)axis, (int)direction, type_Motor_Speed);
                    }
                    else if (radioButton_RecipeVision_JogMove_Step.Checked)
                    {
                        string text = textBox_RecipeVision_JogMove_StepSize.Text;
                        distance = Math.Abs(Equipment.ToDouble(text));
                        workStage.MovetoWorkStage_Rel_Positions((WorkStage.nAxis)axis, distance, (int)direction, type_Motor_Speed);
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

        private async void button_RecipeVision_WorkStage_ToTempPos1_Move_Click(object sender, EventArgs e)
        {
            //  Temp1 위치로 이동
            //  현재 Fine Camera Center 위치를 Scanner Center 위치로 이동
            double lfTargetX = 0.0f;
            double lfTargetY = 0.0f;

            // 파일에 저장 해 놓자. // 가져 올 수 있는 Data 인지 확인하자.
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

            // 아래 코드 고민해 보자. IsMoving 함수 만들어서 사용해야 할듯.
            if (!workStage.IsWorkStageMoving(WorkStage.nAxis.X) ||
                !workStage.IsWorkStageMoving(WorkStage.nAxis.Y) ||
                !workStage.IsWorkStageMoving(WorkStage.nAxis.Z))
            {
                var mb1 = new QMC.Common.UI.MessageBoxOk();
                mb1.ShowDialog("Warning !", "Stage 가 이동중입니다.");
                return;
            }

            // Target 위치
            lfTargetX = Equipment.ToDouble(textBox_RecipeVision_WorkStage_TempPos1_StageX.Text);
            lfTargetY = Equipment.ToDouble(textBox_RecipeVision_WorkStage_TempPos1_StageY.Text);

            //  소프트웨어 리밋 체크
            //if (lfTargetX < X_Limit_Min || lfTargetX > X_Limit_Max ||
            //    lfTargetY < Y_Limit_Min || lfTargetY > Y_Limit_Max)
            //{
            //    string msg = $"이동하려는 위치가 소프트웨어 리밋을 벗어났습니다.\n\n" +
            //                 $"X 범위: {X_Limit_Min} ~ {X_Limit_Max}, 현재: {lfTargetX}\n" +
            //                 $"Y 범위: {Y_Limit_Min} ~ {Y_Limit_Max}, 현재: {lfTargetY}";
            //    var mb1 = new QMC.Common.UI.MessageBoxOk();
            //    mb1.ShowDialog("Software Limit", msg);
            //    return;
            //}

            //  속도 설정
            Type_Motor_Speed motor_Speed;
            if (radioButton_RecipeVision_Move_MoveMode_Fine.Checked)
            {
                motor_Speed = Type_Motor_Speed.Fine;
            }
            else
            {
                motor_Speed = Type_Motor_Speed.Coarse;
            }
            XyCoordinate xyInterpolatedCoordinate = new XyCoordinate();
            xyInterpolatedCoordinate.X = lfTargetX;
            xyInterpolatedCoordinate.Y = lfTargetY;
            workStage.MovetoWorkStage_ABS_PositionsXY(xyInterpolatedCoordinate, motor_Speed);

            bool bWaitPosX = false;
            bool bWaitPosY = false;

            try
            {
               // await 사용으로 UI 프리즈 없이 동작 //시컨스에서는 await 사용 안됨.
               bWaitPosX = await workStage.WaitUntilInPositionAsync(WorkStage.nAxis.X, xyInterpolatedCoordinate.X);
               bWaitPosY = await workStage.WaitUntilInPositionAsync(WorkStage.nAxis.Y, xyInterpolatedCoordinate.Y);

                if (!bWaitPosX)
                {
                    Log.Write("SLD-200", Equipment.User_Name, "Find Align Mark", "X축 이동 실패");
                    workStage.AlarmPost(WorkStage.AlarmKey.eStageMoveFail);
                }
                if (!bWaitPosY)
                {
                    Log.Write("SLD-200", Equipment.User_Name, "Find Align Mark", "Y축 이동 실패");
                    workStage.AlarmPost(WorkStage.AlarmKey.eStageMoveFail);
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
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
            workStage.SetLightingByChannel(Equipment.LightingChannel.CoarseCamIR, Equipment.stVisionRecipeSet.nPreIlluminationIR);
            workStage.SetLightingByChannel(Equipment.LightingChannel.CoarseCamRed, Equipment.stVisionRecipeSet.nPreIlluminationRed);
            Thread.Sleep(100);
            workStage.SetLightingByChannel(Equipment.LightingChannel.FineCamRed, 4000, true);
            workStage.SetLightingByChannel(Equipment.LightingChannel.FineCamIR, 0, false);

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

            //checkBox_RecipeVision_Illuminator_Red.Enabled = false;
            //checkBox_RecipeVision_Illuminator_IR.Enabled = false;
            textBox_RecipeVision_Camera_ExposureTime_High.Enabled = false;
            textBox_RecipeVision_AxisZ_Setting.Enabled = false;

            hScrollBar_RecipeVision_Illuminator_IR.Value = Equipment.stVisionRecipeSet.nPreIlluminationIR;
            hScrollBar_RecipeVision_Illuminator_Red.Value = Equipment.stVisionRecipeSet.nPreIlluminationRed;

            SetScroll();
        }

        private void radioButton_RecipeVision_CameraSelection_HighMag_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButton_RecipeVision_CameraSelection_HighMag.Checked)
                return;

            // 현재 선택된 마크 인덱스 가져오기
            int idx = comboBox_Recipe_Fiducial_MarkIndex.SelectedIndex;
            if (idx < 0 || idx >= Equipment.stVisionRecipeSet.SocketMarkList.Count)
                return;

            var mark = Equipment.stVisionRecipeSet.SocketMarkList[idx];

            // 기존 조명 OFF
            workStage.SetLightingByChannel(Equipment.LightingChannel.CoarseCamIR, 0, false);
            workStage.SetLightingByChannel(Equipment.LightingChannel.CoarseCamRed, 0, false);
            Thread.Sleep(100);

            // 선택된 마크의 조명 값으로 설정
            workStage.SetLightingByChannel(Equipment.LightingChannel.FineCamRed, mark.IllumRed);
            workStage.SetLightingByChannel(Equipment.LightingChannel.FineCamIR, mark.IllumIR);

            // 조명 관련 컨트롤 Enable
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

            checkBox_RecipeVision_Illuminator_Red.Enabled = true;
            checkBox_RecipeVision_Illuminator_IR.Enabled = true;
            textBox_RecipeVision_Camera_ExposureTime_High.Enabled = true;
            textBox_RecipeVision_AxisZ_Setting.Enabled = true;

            // 조명 슬라이더 값도 마크 기반으로 반영
            // 이거 죽는디?
            //hScrollBar_RecipeVision_Illuminator_IR.Value = mark.IllumIR;
            //hScrollBar_RecipeVision_Illuminator_Red.Value = mark.IllumRed;

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
            double dScore = 0.0;

            if (textBox_Recipe_Fiducial_CircleSize.Text.Length < 0)
            {
                MessageBox.Show("Fiducial Size 를 입력하세요.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (ImageViewer_RecipeVision_highs.Simulated)
            {
                //Owner.Simulated = true;
                workStage.Camera_HighRes.LatestImage = ImageViewer_RecipeVision_highs.InputImage;
            }

            dTargetSize_Radius = Equipment.ToDouble(textBox_Recipe_Fiducial_CircleSize.Text); //  Fiducial 마크 크기
            //dSpec = Equipment.ToDouble(textBox_Recipe_Fiducial_CircleSpec.Text); //  Fiducial 마크 Spec
            //dScore = Equipment.ToDouble(textBox_Recipe_Fiducial_CircleScore.Text); //  Fiducial 마크 Score
            double percentValue = 0.0;
            if (double.TryParse(textBox_Recipe_Fiducial_CircleSpec.Text, out percentValue))
            {
                // UI에서 입력받은 %를 내부 0~1 값으로 변환
                dSpec = percentValue / 100.0;
            }
            if (double.TryParse(textBox_Recipe_Fiducial_CircleScore.Text, out percentValue))
            {
                // UI에서 입력받은 %를 내부 0~1 값으로 변환
                dScore = percentValue / 100.0;
            }

            if (radioButton_Fiducial_Black.Checked)
            {
                nTargetColor = 0;
            }
            else if (radioButton_Fiducial_White.Checked)
            {
                nTargetColor = 1;
            }
            else if (radioButton_Fiducial_Ignor.Checked)
            {
                nTargetColor = 2;
            }
            else
            {
                nTargetColor = 0;
            }

            QMC_ImageProcessFindAlignResult result = new QMC_ImageProcessFindAlignResult();
            QMC_ImageProcessFindAlign aligner = new QMC_ImageProcessFindAlign();
            List<RectangleF> circlesResult = new List<RectangleF>();
            {
                int w = workStage.Camera_HighRes.Resolution.Width;
                int h = workStage.Camera_HighRes.Resolution.Height;
                nImage_Width = w;
                nImage_Height = h;
                double dRadius = 0.0;
                dRadius = dTargetSize_Radius / workStage.Config.ParamConfig.UpperVision_Scale_X;

                if (nTargetColor <= 1)
                {
                    result = aligner.FindCirclesWidthCircleBoundary(circlesResult,
                                                    workStage.Camera_HighRes.LatestImage.RawData, 
                                                    w, 
                                                    h, 
                                                    (int)dRadius, 
                                                    dSpec,
                                                    ref bFindCircle, 
                                                    0, 0, 
                                                    nTargetColor == 0, 
                                                    dScore, 
                                                    false);

                }
                else if(nTargetColor == 2)
                {
                    result = aligner.FindCircleForFR4(workStage.Camera_HighRes.LatestImage.RawData,
                                                      w,
                                                      h,
                                                      (int)dRadius,
                                                      dSpec,
                                                      dScore);
                    circlesResult.Clear();
                    foreach (var circle in result.Circles)
                    {
                        circlesResult.Add(circle.GetBoundery());
                        bFindCircle = true;
                    }
                }
                workStage.UpdateOverlay(result);
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
            }
            else
            {
                //MessageBox.Show("원 찾기 실패", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //listBox_Recipe_Fiducial_Result.Items.Clear();

                // 실패 시
                if (!result.Success)
                {
                    listBox_Recipe_Fiducial_Result.Items.Clear();
                    listBox_Recipe_Fiducial_Result.Items.Add("[원 찾기 실패]");
                    listBox_Recipe_Fiducial_Result.Items.Add("Reason : " + result.FailReason);
                    if (!string.IsNullOrEmpty(result.FailMessage))
                        listBox_Recipe_Fiducial_Result.Items.Add(result.FailMessage);

                    // 새 가이드 출력
                    if (!string.IsNullOrEmpty(result.UserGuide))
                    {
                        listBox_Recipe_Fiducial_Result.Items.Add("---- Guide ----");
                        foreach (var line in result.UserGuide.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                            listBox_Recipe_Fiducial_Result.Items.Add(line);
                    }
                    else if (!string.IsNullOrEmpty(result.Recommendation))
                    {
                        listBox_Recipe_Fiducial_Result.Items.Add("---- Recommendation ----");
                        foreach (var line in result.Recommendation.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                            listBox_Recipe_Fiducial_Result.Items.Add(line);
                    }

                    MessageBox.Show((result.UserGuide ?? result.FailMessage),
                        "원 찾기 실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    //listBox_Recipe_Fiducial_Result.Items.Clear();
                    //listBox_Recipe_Fiducial_Result.Items.Add("[원 찾기 실패]");
                    //listBox_Recipe_Fiducial_Result.Items.Add("Reason : " + result.FailReason);
                    //if (!string.IsNullOrEmpty(result.FailMessage))
                    //    listBox_Recipe_Fiducial_Result.Items.Add(result.FailMessage);
                    //if (!string.IsNullOrEmpty(result.Recommendation))
                    //{
                    //    listBox_Recipe_Fiducial_Result.Items.Add("---- 권고 ----");
                    //    foreach (var line in result.Recommendation.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                    //        listBox_Recipe_Fiducial_Result.Items.Add(line);
                    //}
                    //MessageBox.Show($"{result.FailMessage}\n\n{result.Recommendation}", "원 찾기 실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            //  원 찾기 후 다시 Live
            if (workStage.Camera_HighRes.Opened)
            {
                workStage.Camera_HighRes.StartLive();
            }
        }

        private void button_RecipeVision_Camera_ExposureTime_Click(object sender, EventArgs e)
        {
            double dExposureTime = Equipment.ToDouble(textBox_RecipeVision_Camera_ExposureTime_High.Text);
            if (workStage.jigAligner_HighRes.Camera.Opened)
            {

                workStage.jigAligner_HighRes.Camera.SetExposureTime(dExposureTime);
            }
        }

        private void button_RecipeVision_AxisZ_Setting_Click(object sender, EventArgs e)
        {

        }

        private void checkBox_RecipeVision_Illuminator_Red_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox_RecipeVision_Illuminator_Red.Checked)
            {
                checkBox_RecipeVision_Illuminator_Red.Text = "USE";
                hScrollBar_RecipeVision_Illuminator_Red.Enabled = true;
                textBox_RecipeVision_IlluminationValue_Red.Enabled = true;
            }
            else
            {
                checkBox_RecipeVision_Illuminator_Red.Text = "NOT USE";
                hScrollBar_RecipeVision_Illuminator_Red.Enabled = false;
                textBox_RecipeVision_IlluminationValue_Red.Enabled = false;
            }
        }

        private void checkBox_RecipeVision_Illuminator_IR_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_RecipeVision_Illuminator_IR.Checked)
            {
                checkBox_RecipeVision_Illuminator_IR.Text = "USE";
                hScrollBar_RecipeVision_Illuminator_IR.Enabled = true;
                textBox_RecipeVision_IlluminationValue_IR.Enabled = true;
            }
            else
            {
                checkBox_RecipeVision_Illuminator_IR.Text = "NOT USE";
                hScrollBar_RecipeVision_Illuminator_IR.Enabled = false;
                textBox_RecipeVision_IlluminationValue_IR.Enabled = false;
            }
        }

        private void button_RecipeVision_Camera_ExposureTime_Low_Click(object sender, EventArgs e)
        {
            double dExposureTime = Equipment.ToDouble(textBox_RecipeVision_Camera_ExposureTime_Low.Text);//Equipment.stVisionRecipeSet.dSocketIlluminationExposureTime;
            if (workStage.jigAligner_LowRes.Camera.Opened)
            {
                workStage.jigAligner_LowRes.Camera.SetExposureTime(dExposureTime);
            }
        }

        //comboBox_Recipe_Fiducial_MarkIndex
        private void InitSocketMarkCombo()
        {
            // 리스트가 비어 있다면 기존 호환용 마크 자동 등록
            if (Equipment.stVisionRecipeSet.SocketMarkList.Count == 0)
            {
                var defaultMark = new SocketMarkInfo();

                // 기존 필드 → 마이그레이션
                defaultMark.AlignType = Equipment.stVisionRecipeSet.nSocketAlignType;
                defaultMark.MarkType = Equipment.stVisionRecipeSet.nSocketMarkType;
                defaultMark.MarkColor = Equipment.stVisionRecipeSet.nSocketCircleColor;
                defaultMark.MarkRadius = Equipment.stVisionRecipeSet.dSocketCircleMarkRadius;
                defaultMark.MarkSpec = Equipment.stVisionRecipeSet.dSocketCircleMarkSpec;
                defaultMark.MarkScore = Equipment.stVisionRecipeSet.dSocketCircleMarkScore;
                defaultMark.UseIR = Equipment.stVisionRecipeSet.bSocketIlluminationIRUse;
                defaultMark.UseRed = Equipment.stVisionRecipeSet.bSocketIlluminationRedUse;
                defaultMark.ExposureTime = Equipment.stVisionRecipeSet.dSocketIlluminationExposureTime;
                defaultMark.AxisZOffset = Equipment.stVisionRecipeSet.dSocketAxisZ_Offset;
                defaultMark.IllumIR = Equipment.stVisionRecipeSet.nSocketIlluminationIR;
                defaultMark.IllumRed = Equipment.stVisionRecipeSet.nSocketIlluminationRed;

                Equipment.stVisionRecipeSet.SocketMarkList.Add(defaultMark);
            }

            // 콤보박스 채우기
            comboBox_Recipe_Fiducial_MarkIndex.Items.Clear();

            for (int i = 0; i < Equipment.stVisionRecipeSet.SocketMarkList.Count; i++)
                comboBox_Recipe_Fiducial_MarkIndex.Items.Add($"Mark {i + 1}");

            if (comboBox_Recipe_Fiducial_MarkIndex.Items.Count > 0)
                comboBox_Recipe_Fiducial_MarkIndex.SelectedIndex = 0;
        }

        private void comboBox_SocketMarkIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplySocketMarkToUI();
        }

        private void ApplySocketMarkToUI()
        {
            int idx = comboBox_Recipe_Fiducial_MarkIndex.SelectedIndex;
            if (idx < 0 || idx >= Equipment.stVisionRecipeSet.SocketMarkList.Count)
                return;

            var mark = Equipment.stVisionRecipeSet.SocketMarkList[idx];

            radioButton_Fiducial_Circle.Checked = mark.AlignType == 0;
            radioButton_Fiducial_Pattern.Checked = mark.AlignType == 2;

            radioButton_Fiducial_Type_Circle.Checked = mark.MarkType == 0;
            radioButton_Fiducial_Type_GoldPowder.Checked = mark.MarkType == 1;

            radioButton_Fiducial_White.Checked = mark.MarkColor == 0;
            radioButton_Fiducial_Black.Checked = mark.MarkColor == 1;
            radioButton_Fiducial_Ignor.Checked = mark.MarkColor == 2;

            textBox_Recipe_Fiducial_CircleSize.Text = mark.MarkRadius.ToString("F3");
            
            //textBox_Recipe_Fiducial_CircleSpec.Text = mark.MarkSpec.ToString("F3");
            //textBox_Recipe_Fiducial_CircleScore.Text = mark.MarkScore.ToString("F3");
            // 내부 값 (0~1)을 퍼센트 문자열로 표시
            textBox_Recipe_Fiducial_CircleSpec.Text =
                (mark.MarkSpec * 100).ToString("F2");
            // 내부 값 (0~1)을 퍼센트 문자열로 표시
            textBox_Recipe_Fiducial_CircleScore.Text =
                (mark.MarkScore * 100).ToString("F2");


            checkBox_RecipeVision_Illuminator_Red.Checked = mark.UseRed;
            checkBox_RecipeVision_Illuminator_IR.Checked = mark.UseIR;
            textBox_RecipeVision_Camera_ExposureTime_High.Text = mark.ExposureTime.ToString("F2");
            textBox_RecipeVision_AxisZ_Setting.Text = mark.AxisZOffset.ToString("F3");

            textBox_Recipe_RecipeVision_Illuminator_FineCamRed.Text = mark.IllumRed.ToString();
            textBox_Recipe_RecipeVision_Illuminator_FineCamIR.Text = mark.IllumIR.ToString();


            radioButton_RecipeVision_CameraSelection_HighMag_CheckedChanged(null, null); // HighMag 카메라 설정 적용

            this.Refresh();
        }

        private void SaveSocketMarkFromUI()
        {
            int idx = comboBox_Recipe_Fiducial_MarkIndex.SelectedIndex;
            if (idx < 0 || idx >= Equipment.stVisionRecipeSet.SocketMarkList.Count)
                return;

            var mark = Equipment.stVisionRecipeSet.SocketMarkList[idx];

            mark.AlignType = radioButton_Fiducial_Pattern.Checked ? 2 : 0;
            mark.MarkType = radioButton_Fiducial_Type_GoldPowder.Checked ? 1 : 0;

			if (radioButton_Fiducial_White.Checked)
                mark.MarkColor = 0;
            else if (radioButton_Fiducial_Black.Checked)
                mark.MarkColor = 1;
            else
                mark.MarkColor = 2;
                
            //if (radioButton_Fiducial_Black.Checked)
            //    mark.MarkColor = 0;
            //else if (radioButton_Fiducial_White.Checked)
            //    mark.MarkColor = 1;
            //else
            //    mark.MarkColor = 2;

            mark.MarkRadius = Equipment.ToDouble(textBox_Recipe_Fiducial_CircleSize.Text);
            //mark.MarkSpec = Equipment.ToDouble(textBox_Recipe_Fiducial_CircleSpec.Text);
            //mark.MarkScore = Equipment.ToDouble(textBox_Recipe_Fiducial_CircleScore.Text);
            double percentValue = 0.0;
            if (double.TryParse(textBox_Recipe_Fiducial_CircleSpec.Text, out percentValue))
            {
                // UI에서 입력받은 %를 내부 0~1 값으로 변환
                mark.MarkSpec = percentValue / 100.0;
            }
            if (double.TryParse(textBox_Recipe_Fiducial_CircleScore.Text, out percentValue))
            {
                // UI에서 입력받은 %를 내부 0~1 값으로 변환
                mark.MarkScore = percentValue / 100.0;
            }


            mark.UseRed = checkBox_RecipeVision_Illuminator_Red.Checked;
            mark.UseIR = checkBox_RecipeVision_Illuminator_IR.Checked;
            mark.ExposureTime = Equipment.ToDouble(textBox_RecipeVision_Camera_ExposureTime_High.Text);
            mark.AxisZOffset = Equipment.ToDouble(textBox_RecipeVision_AxisZ_Setting.Text);

            mark.IllumRed = Equipment.ToInt(textBox_Recipe_RecipeVision_Illuminator_FineCamRed.Text);
            mark.IllumIR = Equipment.ToInt(textBox_Recipe_RecipeVision_Illuminator_FineCamIR.Text);
        }

        private void button_Recipe_Fiducial_Mark_Add_Click(object sender, EventArgs e)
        {
            AddSocketMark();
        }

        private void button_Recipe_Fiducial_Mark_Delete_Click(object sender, EventArgs e)
        {
            int idx = comboBox_Recipe_Fiducial_MarkIndex.SelectedIndex;
            if (idx < 0)
            {
                MessageBox.Show("삭제할 마크가 선택되지 않았습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 인덱스는 1부터 표시 (사용자용)
            string msg = $"마크 {idx + 1}번을 삭제 하시겠습니까?";
            var mb1 = new QMC.Core.MessageBoxYesNo();
            if (DialogResult.Yes != mb1.ShowDialog("삭제 확인", msg))
                return;

            DeleteSocketMark();
        }

        private void AddSocketMark()
        {
            var newMark = new SocketMarkInfo();

            // 기본값 복사 (현재 마크 기준 or 초기값)
            if (comboBox_Recipe_Fiducial_MarkIndex.SelectedIndex >= 0)
            {
                var current = Equipment.stVisionRecipeSet.SocketMarkList[comboBox_Recipe_Fiducial_MarkIndex.SelectedIndex];
                newMark = current.Clone(); // ※ Clone() 구현되어 있어야 함
            }

            Equipment.stVisionRecipeSet.SocketMarkList.Add(newMark);
            InitSocketMarkCombo();
            comboBox_Recipe_Fiducial_MarkIndex.SelectedIndex = Equipment.stVisionRecipeSet.SocketMarkList.Count - 1;
        }

        private void DeleteSocketMark()
        {
            if (Equipment.stVisionRecipeSet.SocketMarkList.Count <= 1)
            {
                MessageBox.Show("최소 1개의 마크는 유지되어야 합니다.", "삭제 불가", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idx = comboBox_Recipe_Fiducial_MarkIndex.SelectedIndex;
            if (idx < 0 || idx >= Equipment.stVisionRecipeSet.SocketMarkList.Count)
                return;

            Equipment.stVisionRecipeSet.SocketMarkList.RemoveAt(idx);

            // 삭제 후 인덱스 조정
            InitSocketMarkCombo();
            comboBox_Recipe_Fiducial_MarkIndex.SelectedIndex = Math.Max(0, idx - 1);
        }

        // 이거 각각 폼에 만들어야함.
        private void InitRecipeUI_KeyPad()
        {
            RegisterKeyPadDoubleClickHandlers(this); // 폼 전체에 대해 수행
        }
        private void RegisterKeyPadDoubleClickHandlers(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                // 조건: 숫자 입력용 TextBox 또는 RichTextBox만
                bool isTargetTextBox = ctrl is TextBox || ctrl is RichTextBox;

                if (isTargetTextBox && ctrl.Tag?.ToString().Contains("KeyPad") == true)
                {
                    ctrl.DoubleClick -= textBox_DoubleClick_OpenKeyPad; // 중복 연결 방지
                    ctrl.DoubleClick += textBox_DoubleClick_OpenKeyPad;

                    // 키보드 입력 제한용 Validating 연결
                    ctrl.Validating -= textBox_Validate_KeyPadRange;
                    ctrl.Validating += textBox_Validate_KeyPadRange;
                }

                // 하위 컨트롤 재귀 탐색
                if (ctrl.HasChildren)
                    RegisterKeyPadDoubleClickHandlers(ctrl);
            }
        }
        private void textBox_DoubleClick_OpenKeyPad(object sender, EventArgs e)
        {
            if (sender is Control ctrl)
            {
                string currentText = ctrl.Text ?? "0";
                var dlg = new FormNew_KeyPad();
                dlg.StartPosition = FormStartPosition.CenterScreen;

                // Tag 파싱
                var meta = KeyPadMeta.ParseFromTag(ctrl.Tag?.ToString());

                dlg.MinValue = meta.Min;
                dlg.MaxValue = meta.Max;
                dlg.OriginValue = meta.Origin;

                if (double.TryParse(currentText, out double value))
                    dlg.SetInitialValue(value);
                else
                    dlg.SetInitialValue(0);

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    string result = dlg.EnteredValue.ToString(meta.Format);
                    ctrl.Text = result;
                }
            }
        }
        private void textBox_Validate_KeyPadRange(object sender, CancelEventArgs e)
        {
            if (sender is TextBox tb && tb.Tag != null)
            {
                var meta = KeyPadMeta.ParseFromTag(tb.Tag.ToString());

                if (double.TryParse(tb.Text, out double val))
                {
                    if (val < meta.Min)
                    {
                        tb.Text = meta.Min.ToString(meta.Format);
                        //MessageBox.Show($"최소값 {meta.Min}보다 작습니다."); // 또는 자동 보정만
                    }
                    else if (val > meta.Max)
                    {
                        tb.Text = meta.Max.ToString(meta.Format);
                        //MessageBox.Show($"최대값 {meta.Max}보다 큽니다.");
                    }
                    else
                    {
                        tb.Text = val.ToString(meta.Format);
                    }
                }
                else
                {
                    // 숫자 아님 → 초기화
                    tb.Text = meta.Min.ToString(meta.Format);
                }
            }
        }

        private void InitPreAlignMarkCombo()
        {
            // 리스트가 비어 있다면 기존 호환용 마크 자동 등록
            if (Equipment.stVisionRecipeSet.PreAlignMarkList.Count == 0)
            {
                var defaultMark = new PreAlignMarkInfo();

                // 기존 단일 값 → 마이그레이션 (예시)
                defaultMark.CircleColor = Equipment.stVisionRecipeSet.nPreCircleColor;
                defaultMark.CircleMarkRadius = Equipment.stVisionRecipeSet.dPreCircleMarkRadius;
                defaultMark.CircleMarkSpec = Equipment.stVisionRecipeSet.dPreCircleMarkSpec;
                defaultMark.CircleMarkScore = Equipment.stVisionRecipeSet.dPreCircleMarkScore;
                defaultMark.IllumIR = Equipment.stVisionRecipeSet.nPreIlluminationIR;
                defaultMark.IllumRed = Equipment.stVisionRecipeSet.nPreIlluminationRed;
                defaultMark.ExposureTime = Equipment.stVisionRecipeSet.dPreAlignIlluminationExposureTime;
                defaultMark.TrainRoiStart = Equipment.stVisionRecipeSet.pointPreTrainRoiStartLocation;
                defaultMark.TrainRoiEnd = Equipment.stVisionRecipeSet.pointPreTrainRoiEndLocation;
                defaultMark.InspectRoiStart = Equipment.stVisionRecipeSet.pointPreInspectRoiStartLocation;
                defaultMark.InspectRoiEnd = Equipment.stVisionRecipeSet.pointPreInspectRoiEndLocation;
                defaultMark.AlgorithmType = Equipment.stVisionRecipeSet.ePreAlgorithmType;
                defaultMark.MarkType = Equipment.stVisionRecipeSet.ePreMarkType;
                defaultMark.PatternMatching = Equipment.stVisionRecipeSet.PrePatternMatching;
                defaultMark.TrainImagePath = Equipment.stVisionRecipeSet.pointPreTrainImagePath;

                Equipment.stVisionRecipeSet.PreAlignMarkList.Add(defaultMark);
            }

            // 콤보박스 채우기
            comboBox_Recipe_PreAlign_MarkIndex.Items.Clear();

            for (int i = 0; i < Equipment.stVisionRecipeSet.PreAlignMarkList.Count; i++)
                comboBox_Recipe_PreAlign_MarkIndex.Items.Add($"PreAlign Mark {i + 1}");

            if (comboBox_Recipe_PreAlign_MarkIndex.Items.Count > 0)
                comboBox_Recipe_PreAlign_MarkIndex.SelectedIndex = 0;
        }

        private void button_Recipe_PreAlign_Mark_Add_Click(object sender, EventArgs e)
        {
            AddPreAlignMark();
        }

        private void button_Recipe_PreAlign_Mark_Delete_Click(object sender, EventArgs e)
        {
            DeletePreAlignMark();
        }

        private void AddPreAlignMark()
        {
            var newMark = new PreAlignMarkInfo();

            // 현재 마크 복사해서 추가 (편의성)
            if (comboBox_Recipe_PreAlign_MarkIndex.SelectedIndex >= 0)
            {
                var current = Equipment.stVisionRecipeSet.PreAlignMarkList[comboBox_Recipe_PreAlign_MarkIndex.SelectedIndex];
                newMark = current.Clone(); // Clone() 구현되어 있어야 함!
            }

            Equipment.stVisionRecipeSet.PreAlignMarkList.Add(newMark);

            // 콤보 및 인덱스 갱신
            InitPreAlignMarkCombo();
            comboBox_Recipe_PreAlign_MarkIndex.SelectedIndex = Equipment.stVisionRecipeSet.PreAlignMarkList.Count - 1;
        }

        private void DeletePreAlignMark()
        {
            if (Equipment.stVisionRecipeSet.PreAlignMarkList.Count <= 1)
            {
                MessageBox.Show("최소 1개의 마크는 유지되어야 합니다.", "삭제 불가", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idx = comboBox_Recipe_PreAlign_MarkIndex.SelectedIndex;
            if (idx < 0 || idx >= Equipment.stVisionRecipeSet.PreAlignMarkList.Count)
                return;

            Equipment.stVisionRecipeSet.PreAlignMarkList.RemoveAt(idx);

            // 콤보 및 인덱스 조정
            InitPreAlignMarkCombo();
            comboBox_Recipe_PreAlign_MarkIndex.SelectedIndex = Math.Max(0, idx - 1);
        }

        private void ApplyPreAlignMarkToUI()
        {
            int idx = comboBox_Recipe_PreAlign_MarkIndex.SelectedIndex;
            if (idx < 0 || idx >= Equipment.stVisionRecipeSet.PreAlignMarkList.Count)
                return;

            var mark = Equipment.stVisionRecipeSet.PreAlignMarkList[idx];

            // ROI/조명 등 필요한 UI 컨트롤 모두 적용
            RoiTrain.Parameter.StartLocation = mark.TrainRoiStart;
            RoiTrain.Parameter.EndLocation = mark.TrainRoiEnd;
            RoiInspect.Parameter.StartLocation = mark.InspectRoiStart;
            RoiInspect.Parameter.EndLocation = mark.InspectRoiEnd;
            hScrollBar_RecipeVision_Illuminator_IR.Value = mark.IllumIR;
            hScrollBar_RecipeVision_Illuminator_Red.Value = mark.IllumRed;

            textBox_Recipe_RecipeVision_Illuminator_CoarseCamRed.Text = mark.IllumRed.ToString();
            textBox_Recipe_RecipeVision_Illuminator_CoarseCamIR.Text = mark.IllumIR.ToString();


            // Circle 옵션
            radioButton_RecipeVision_Black.Checked = mark.CircleColor == 0;
            radioButton_RecipeVision_White.Checked = mark.CircleColor == 1;
            radioButton_RecipeVision_Ignore.Checked = mark.CircleColor == 2;

            textBox_RecipeVision_Circle_Size.Text = mark.CircleMarkRadius.ToString("F3");
            //textBox_RecipeVision_Circle_Spec.Text = mark.CircleMarkSpec.ToString("F3");
            //textBox_RecipeVision_Circle_Score.Text = mark.CircleMarkScore.ToString("F3");
            // 내부 값 (0~1)을 퍼센트 문자열로 표시
            textBox_RecipeVision_Circle_Spec.Text =
                (mark.CircleMarkSpec * 100).ToString("F2");
            // 내부 값 (0~1)을 퍼센트 문자열로 표시
            textBox_RecipeVision_Circle_Score.Text =
                (mark.CircleMarkScore * 100).ToString("F2");

            radioButton_RecipeVision_Pattern.Checked = mark.AlgorithmType == VisionAlgorithmType.PatternMatching;
            radioButton_RecipeVision_Blob.Checked = mark.AlgorithmType == VisionAlgorithmType.CircleDetection;

            radioButton_RecipeVision_Type_Cross.Checked = mark.MarkType == MarkTypeList.Cross;
            radioButton_RecipeVision_Circle.Checked = mark.MarkType == MarkTypeList.Circle;

            textBox_RecipeVision_Camera_ExposureTime_Low.Text = mark.ExposureTime.ToString("F2");

            // 학습 이미지, 패턴 파라미터 등 기타 필요한 UI 항목도 적용.
            if (mark.PatternMatching != null && mark.PatternMatching.TrainImage != null)
            {
                pictureBox_RecipeVision_TrainImage.Image = mark.PatternMatching.TrainImage.GetImage();
            }
                

            this.Refresh();
        }

        //private void SavePreAlignMarkFromUI()
        //{
        //    int idx = comboBox_Recipe_PreAlign_MarkIndex.SelectedIndex;
        //    if (idx < 0 || idx >= Equipment.stVisionRecipeSet.PreAlignMarkList.Count)
        //        return;

        //    var mark = Equipment.stVisionRecipeSet.PreAlignMarkList[idx];

        //    // ROI/조명 등 필요한 UI 컨트롤 값 저장
        //    mark.TrainRoiStart = RoiTrain.Parameter.StartLocation;
        //    mark.TrainRoiEnd = RoiTrain.Parameter.EndLocation;
        //    mark.InspectRoiStart = RoiInspect.Parameter.StartLocation;
        //    mark.InspectRoiEnd = RoiInspect.Parameter.EndLocation;
        //    mark.IllumIR = hScrollBar_RecipeVision_Illuminator_IR.Value;
        //    mark.IllumRed = hScrollBar_RecipeVision_Illuminator_Red.Value;

        //    // Circle 옵션
        //    if (radioButton_RecipeVision_Black.Checked)
        //        mark.CircleColor = 0;
        //    else if (radioButton_RecipeVision_White.Checked)
        //        mark.CircleColor = 1;
        //    else
        //        mark.CircleColor = 2;

        //    mark.CircleMarkSpec = Equipment.ToDouble(textBox_RecipeVision_Circle_Spec.Text);
        //    mark.CircleMarkRadius = Equipment.ToDouble(textBox_RecipeVision_Circle_Size.Text);
        //    mark.CircleMarkScore = Equipment.ToDouble(textBox_RecipeVision_Circle_Score.Text);

        //    mark.AlgorithmType = radioButton_RecipeVision_Pattern.Checked ? VisionAlgorithmType.PatternMatching : VisionAlgorithmType.CircleDetection;
        //    mark.MarkType = radioButton_RecipeVision_Type_Cross.Checked ? MarkTypeList.Cross : MarkTypeList.Circle;

        //    mark.ExposureTime = Equipment.ToDouble(textBox_RecipeVision_Camera_ExposureTime_Low.Text);

        //    // 학습 이미지, 패턴 파라미터 등 기타 필요한 데이터도 저장
        //    if (mark.PatternMatching == null)
        //        mark.PatternMatching = new PatternMatchingParameters();

        //    mark.PatternMatching.TrainImage = pictureBox_RecipeVision_TrainImage.Image;
        //    // ... 기타 PatternMatching 필드 저장도 필요시 추가

        //    // 만약 저장 직전에 값 동기화가 필요하면 여기에서 SavePreAlignMarkFromUI() 호출
        //}

        private void comboBox_Recipe_PreAlign_MarkIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyPreAlignMarkToUI();
        }

        private void button_Recipe_RecipeVision_Illuminator_FineCamRed_Click(object sender, EventArgs e)
        {
            if (radioButton_RecipeVision_CameraSelection_HighMag.Checked)
            {
                string strTemp = textBox_RecipeVision_IlluminationValue_Red.Text;
                textBox_Recipe_RecipeVision_Illuminator_FineCamRed.Text = strTemp;

            }
                
        }

        private void button_Recipe_RecipeVision_Illuminator_FineCamIR_Click(object sender, EventArgs e)
        {
            if (radioButton_RecipeVision_CameraSelection_HighMag.Checked)
            {
                string strTemp = textBox_RecipeVision_IlluminationValue_IR.Text;
                textBox_Recipe_RecipeVision_Illuminator_FineCamIR.Text = strTemp;
            }
                
        }

        private void button_Recipe_RecipeVision_Illuminator_CoarseCamRed_Click(object sender, EventArgs e)
        {
            if (radioButton_RecipeVision_CameraSelection_LowMag.Checked)
            {
                string strTemp = textBox_RecipeVision_IlluminationValue_Red.Text;
                textBox_Recipe_RecipeVision_Illuminator_CoarseCamRed.Text = strTemp;
            }
                
        }

        private void button_Recipe_RecipeVision_Illuminator_CoarseCamIR_Click(object sender, EventArgs e)
        {
            if (radioButton_RecipeVision_CameraSelection_LowMag.Checked)
            {
                string strTemp = textBox_RecipeVision_IlluminationValue_IR.Text;
                textBox_Recipe_RecipeVision_Illuminator_CoarseCamIR.Text = strTemp;
            }
                
        }

        private async void button_Recipe_Fiducial_Position_Move_Z_Click(object sender, EventArgs e)
        {
            try
            {
                if (workStage == null || vision == null)
                {
                    var mb = new QMC.Common.UI.MessageBoxOk();
                    mb.ShowDialog("Error !", "모듈이 초기화되지 않았습니다.");
                    return;
                }

                if (!workStage.m_bHomeOK)
                {
                    var mb = new QMC.Common.UI.MessageBoxOk();
                    mb.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                    return;
                }

                // 현재 마크 선택
                int markIndex = comboBox_Recipe_Fiducial_MarkIndex.SelectedIndex;
                if (markIndex < 0 || markIndex >= Equipment.stVisionRecipeSet.SocketMarkList.Count)
                {
                    var mb = new QMC.Common.UI.MessageBoxOk();
                    mb.ShowDialog("Warning !", "선택된 Fiducial Mark 가 없습니다.");
                    return;
                }
                var mark = Equipment.stVisionRecipeSet.SocketMarkList[markIndex];

                // 이동 중 여부 (간단 체크)
                if (!workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Z))
                {
                    var mb = new QMC.Common.UI.MessageBoxOk();
                    mb.ShowDialog("Warning !", "Z 축이 이동중입니다.");
                    return;
                }

                // 사용자 확인
                var mbAsk = new QMC.Common.UI.MessageBoxYesNo();
                if (DialogResult.Yes != mbAsk.ShowDialog("Question ?", $"선택 마크(Z Offset:{mark.AxisZOffset:F3}) 기준으로 Z 축을 이동하시겠습니까?"))
                    return;

                if (!workStage.IsInterlock_WorkStageZ_Enabled())
                {
                    var mb = new QMC.Common.UI.MessageBoxOk();
                    mb.ShowDialog("Warning !", "Z 인터락 조건이 만족되지 않았습니다.");
                    return;
                }

                // 기준 Teaching (Laser_FocusPos 사용 – 필요 시 Vision_SafetyPos 로 교체 가능)
                int teachIndex = (int)Vision.Vision_TeachingPosList.Laser_FocusPos;
                if (vision.stVisionTeachingPos == null ||
                    teachIndex < 0 ||
                    teachIndex >= vision.stVisionTeachingPos.Length)
                {
                    var mb = new QMC.Common.UI.MessageBoxOk();
                    mb.ShowDialog("Error !", "Vision Teaching Z 데이터를 읽을 수 없습니다.");
                    return;
                }

                double baseZ = vision.stVisionTeachingPos[teachIndex].Vision_Z;
                double offsetSocketHeight = workStage.m_dZOffset_SocketHeightCheck;
                double thickness = 0.0;
                if (Equipment.stLayerRecipeSet != null &&
                    Equipment.stLayerRecipeSet.Length > 0)
                {
                    thickness = Equipment.stLayerRecipeSet[0].ModuleInformation_Silicon_Thickness;
                }
                double markOffset = mark.AxisZOffset; // 선택된 마크 오프셋
                double targetZ = baseZ + offsetSocketHeight + thickness + markOffset;

                // 속도 선택
                Equipment.Type_Motor_Speed speedType =
                    radioButton_RecipeVision_Move_MoveMode_Fine.Checked
                        ? Equipment.Type_Motor_Speed.Fine
                        : Equipment.Type_Motor_Speed.Coarse;

                int axisZ = (int)WorkStage.nAxis.Z;
                double vel, acc;
                
                speedType = Equipment.Type_Motor_Speed.Fine;    // Z 축은 항상 정밀 속도로 이동하도록 고정 (2025-09-03)

                if (speedType == Equipment.Type_Motor_Speed.Fine)
                {
                    vel = Equipment.stAxisParam[axisZ].Common_Speed_Fine;
                    acc = Equipment.stAxisParam[axisZ].Common_Acceleration_Fine;
                }
                else
                {
                    vel = Equipment.stAxisParam[axisZ].Common_Speed_Coarse;
                    acc = Equipment.stAxisParam[axisZ].Common_Acceleration_Coarse;
                }

                // 이동 명령
                workStage.MC_Func.MC_MovePosition(axisZ, targetZ, vel, acc, acc);

                // 대기
                bool ok = await workStage.WaitUntilInPositionAsync(WorkStage.nAxis.Z, targetZ);
                if (!ok)
                {
                    Log.Write("RecipeVision", "ZMove", $"Z 이동 실패 Target:{targetZ:F4} Enc:{workStage.MC_Func.MC_GetEncPos(axisZ):F4}");
                    workStage.AlarmPost(WorkStage.AlarmKey.eZAxisFail);
                    var mb = new QMC.Common.UI.MessageBoxOk();
                    mb.ShowDialog("Warning !", "Z 축 이동 실패(Timeout)");
                    return;
                }

                Log.Write("RecipeVision", "ZMove", $"Z 이동 완료 Target:{targetZ:F4}");
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                var mb = new QMC.Common.UI.MessageBoxOk();
                mb.ShowDialog("Error !", "Z 이동 중 예외가 발생했습니다.");
            }
        }
    }
}
