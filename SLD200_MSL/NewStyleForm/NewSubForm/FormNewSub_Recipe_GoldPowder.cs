using OpenCvSharp.Dnn;
using QMC.Common;
using QMC.Common.Global;
using QMC.Common.Modules;
using QMC.Common.Parts;
using QMC.Common.Recipe;
using QMC.Common.Vision.Tools;
using QMC.Common.VisionPart;
using QMC.Core;
using SLD200_MSL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using QMC.Common.Recipe;


namespace SLD200.NewStyleForm.NewSubForm
{
    public partial class FormNewSub_Recipe_GoldPowder : UserControl
    {
        private bool m_bFormVisible = false; // 실제 Show 상태 여부
        public bool m_bInitialized = false;

        private System.Windows.Forms.Timer timer_Status;

        static WorkStage workStage;
        static Vision vision;
        static JigAligner Owner;

        private VisionRecipeData m_recipe = null;
        private string m_recipePath = "";

        // (추가) 필드
        private int m_currentGoldPowderSocketIndex = 0;
        private int m_currentGoldPowderSocketIndexOffset = 0;
        //private int GoldPowderSocketTotal => Math.Max(1, m_recipe?.GoldPowderSocketPosList?.Count ?? 1);
        // GoldPowder 소켓 총수 (동적으로 갱신)
        private int m_goldPowderSocketTotal = 1;

        private int m_currentGoldPowderMarkIndex = 0;


        public FormNewSub_Recipe_GoldPowder()
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
                    Owner = workStage.jigAligner_HighRes;
                }
                else if(module.Name == "Vision")
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
        private void FormNewSub_Recipe_GoldPowder_Load(object sender, EventArgs e)
        {
            if(m_bInitialized)
                return;

            if (this.ImageViewer_Recipe_GoldPowder_highs.IsHandleCreated)
            {
                this.ImageViewer_Recipe_GoldPowder_highs.SizeMode = PictureBoxSizeMode.CenterImage;
                this.ImageViewer_Recipe_GoldPowder_highs.SuspendDisplay();
                this.ImageViewer_Recipe_GoldPowder_highs.Camera = workStage.jigAligner_HighRes.Camera; //Owner.Camera;
            }
            workStage.UpdateResultOveray += OnUpdateResultOverlay;

            // 타이머 초기화
            timer_Status = new System.Windows.Forms.Timer();
            timer_Status.Interval = 200; // 200ms 주기
            timer_Status.Tick += Timer_Status_Tick;
            timer_Status.Start();

            radioButton_Recipe_GoldPowder_CameraSelection_HighMag.Checked = true;
            radioButton_Recipe_GoldPowder_CameraSelection_LowMag.Visible = false;
            radioButton_Recipe_GoldPowder_CameraSelection_LowMag.Enabled = false;

            this.hScrollBar_Recipe_GoldPowder_Illuminator_IR.ValueChanged += new System.EventHandler(this.hScrollBarIlluminator_ValueChanged_IR);
            this.hScrollBar_Recipe_GoldPowder_Illuminator_Red.ValueChanged += new System.EventHandler(this.hScrollBarIlluminator_ValueChanged_Red);
            SetScroll();

            InitRecipeUI_KeyPad();

            this.RoiTrain = Owner.GetTrainRoi();
            this.RoiInspect = Owner.GetInspectRoi();
            this.m_RoiListControl = new SLD200_MSL.RoiListControl(RoiTrain, RoiInspect, workStage.Camera_HighRes.Resolution);
            this.m_RoiListControl.roiGoldpowderButtonClick += RoiGoldpowderButtonClick;
            this.m_RoiListControl.roiGoldpowderSaveButtonClick += RoiGoldpowderSaveButtonClick;

            m_bInitialized = true;
        }

        private void Timer_Status_Tick(object sender, EventArgs e)
        {
            try
            {
                // 타이머 중복 호출 방지
                timer_Status.Enabled = false;


            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            finally
            {
                timer_Status.Enabled = true;
            }
        }

        public void OnShow()
        {
            if (!m_bInitialized)
                return;
            LoadRecipe();

            // (추가) 마크 콤보 먼저 초기화
            InitGoldPowderMarkCombo();

            InitGoldPowderSocketCombo(); // (추가)

            // 없을 경우 최소 1개 확보 후 첫 소켓 UI 표시
            m_recipe?.EnsureGoldPowderSocketPosCount(m_goldPowderSocketTotal);
            m_recipe?.EnsureGoldPowderSocketPosCount_Offset(m_goldPowderSocketTotal);
            
            ApplyRecipeToUI();

            // (추가) 비어있는 소켓 포지션 자동 초기화 후 저장
            AutoInitializeEmptyGoldPowderPositionsAndSave();

            timer_Status.Start();
        }
        public void OnHide()
        {
            if (!m_bInitialized)
                return;
            UpdateRecipeFromUI();
            timer_Status.Stop();
        }

        // (추가) GoldPowder 마크 콤보 초기화/갱신
        

        private void OnUpdateResultOverlay(object sender, EventArgs e)
        {
            if (sender is QMC.Common.Vision.Cameras.Camera camera)
            {
                if (camera == workStage.Camera_HighRes)
                {
                    ImageViewer_Recipe_GoldPowder_highs.ResultOverlays = workStage.FineCamResultOveray;
                }
                //else if (camera == workStage.jigAligner_LowRes.Camera)
                //{
                //    ImageViewer_RecipeVision_Lows.ResultOverlays = workStage.CoarseCamResultOveray;
                //}
            }
        }

        // (추가) 소켓 콤보 초기화 호출 지점: LoadRecipe() 후 OnShow 또는 Load 완료 시
        // 콤보 초기화 갱신 (기존 메서드 수정)
        private void InitGoldPowderSocketCombo()
        {
            if (m_recipe == null) return;

            RefreshGoldPowderSocketTotal(); // 실제 소켓 수 먼저 갱신

            comboBox_Recipe_GoldPowder_Socket.Items.Clear();
            for (int i = 0; i < m_goldPowderSocketTotal; i++)
                comboBox_Recipe_GoldPowder_Socket.Items.Add((i + 1).ToString());

            if (comboBox_Recipe_GoldPowder_Socket.Items.Count > 0)
                comboBox_Recipe_GoldPowder_Socket.SelectedIndex =
                    Math.Min(m_currentGoldPowderSocketIndex, comboBox_Recipe_GoldPowder_Socket.Items.Count - 1);

            comboBox_Recipe_GoldPowder_Socket_Offset.Items.Clear();
            for (int i = 0; i < m_goldPowderSocketTotal; i++)
                comboBox_Recipe_GoldPowder_Socket_Offset.Items.Add((i + 1).ToString());

            if (comboBox_Recipe_GoldPowder_Socket_Offset.Items.Count > 0)
                comboBox_Recipe_GoldPowder_Socket_Offset.SelectedIndex =
                    Math.Min(m_currentGoldPowderSocketIndexOffset, comboBox_Recipe_GoldPowder_Socket_Offset.Items.Count - 1);
        }

        private void button_Recipe_GoldPowder_Save_Click(object sender, EventArgs e)
        {
            UpdateRecipeFromUI();
            SyncLegacyGoldPowderPosFromSocket0();       // 레거시 필드 보장
            SyncLegacyGoldPowderPosFromSocket0_Offset();
            SaveRecipe();

            

            //MessageBox.Show("GoldPowder Recipe 저장 완료", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LoadRecipe()
        {
            m_recipePath = Equipment.Current_Recipe;
            if (File.Exists(m_recipePath))
            {
                m_recipe = VisionRecipeData.LoadFromIni(m_recipePath);
            }
            else
            {
                //m_recipe = new VisionRecipeData(); // 디폴트 생성
                var mb1 = new QMC.Common.UI.MessageBoxOk();
                mb1.ShowDialog("Information", "불러오기에 [[실패]] 하였습니다.");
            }
        }

        private void SaveRecipe()
        {
            bool bRtn = false;
            if (m_recipe != null || m_recipePath != "")
            {
                bRtn = m_recipe.SaveToIni(m_recipePath);
            }

            var mb1 = new QMC.Common.UI.MessageBoxOk();
            if (!bRtn)
            {
                mb1.ShowDialog("Information", "저장에 [[실패]] 하였습니다.");
                return;
            }
            else
            {
                Equipment.stVisionRecipeSet = m_recipe;
                mb1.ShowDialog("Information", "저장하였습니다.");
            }

            Equipment.stVisionRecipeSet = VisionRecipeData.LoadFromIni(m_recipePath);
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

        }

        private void SetScroll()
        {
            //Channel 0: Red - high, 1: IR - high, 2: IR - Low
            //if (radioButton_Recipe_GoldPowder_CameraSelection_LowMag.Checked)
            //{
            //    hScrollBar_Recipe_GoldPowder_Illuminator_Red.Minimum = (int)workStage.Config.ListIlluminationChannel[0].Min;
            //    hScrollBar_Recipe_GoldPowder_Illuminator_Red.Maximum = (int)workStage.Config.ListIlluminationChannel[0].Max;
            //    hScrollBar_Recipe_GoldPowder_Illuminator_Red.Value = Equipment.stVisionRecipeSet.nSocketIlluminationRed;
            //    baseLabel_Recipe_GoldPowder_Min_Red.Text = hScrollBar_Recipe_GoldPowder_Illuminator_Red.Minimum.ToString();
            //    baseLabel_Recipe_GoldPowder_Max_Red.Text = hScrollBar_Recipe_GoldPowder_Illuminator_Red.Maximum.ToString();
            //    hScrollBar_Recipe_GoldPowder_Illuminator_IR.Minimum = (int)workStage.Config.ListIlluminationChannel[2].Min;
            //    hScrollBar_Recipe_GoldPowder_Illuminator_IR.Maximum = (int)workStage.Config.ListIlluminationChannel[2].Max;
            //    hScrollBar_Recipe_GoldPowder_Illuminator_IR.Value = Equipment.stVisionRecipeSet.nPreIlluminationIR;
            //    baseLabel_Recipe_GoldPowder_Min_IR.Text = hScrollBar_Recipe_GoldPowder_Illuminator_IR.Minimum.ToString();
            //    baseLabel_Recipe_GoldPowder_Max_IR.Text = hScrollBar_Recipe_GoldPowder_Illuminator_IR.Maximum.ToString();
            //}
            //else if (radioButton_Recipe_GoldPowder_CameraSelection_HighMag.Checked)
            if (radioButton_Recipe_GoldPowder_CameraSelection_HighMag.Checked)
            {
                hScrollBar_Recipe_GoldPowder_Illuminator_Red.Minimum = (int)workStage.Config.ListIlluminationChannel[0].Min;
                hScrollBar_Recipe_GoldPowder_Illuminator_Red.Maximum = (int)workStage.Config.ListIlluminationChannel[0].Max;
                hScrollBar_Recipe_GoldPowder_Illuminator_Red.Value = Equipment.stVisionRecipeSet.nGoldPowderIlluminationRed;
                baseLabel_Recipe_GoldPowder_Min_Red.Text = hScrollBar_Recipe_GoldPowder_Illuminator_Red.Minimum.ToString();
                baseLabel_Recipe_GoldPowder_Max_Red.Text = hScrollBar_Recipe_GoldPowder_Illuminator_Red.Maximum.ToString();

                hScrollBar_Recipe_GoldPowder_Illuminator_IR.Minimum = (int)workStage.Config.ListIlluminationChannel[1].Min;
                hScrollBar_Recipe_GoldPowder_Illuminator_IR.Maximum = (int)workStage.Config.ListIlluminationChannel[1].Max;
                hScrollBar_Recipe_GoldPowder_Illuminator_IR.Value = Equipment.stVisionRecipeSet.nGoldPowderIlluminationIR;
                baseLabel_Recipe_GoldPowder_Min_IR.Text = hScrollBar_Recipe_GoldPowder_Illuminator_IR.Minimum.ToString();
                baseLabel_Recipe_GoldPowder_Max_IR.Text = hScrollBar_Recipe_GoldPowder_Illuminator_IR.Maximum.ToString();
            }
            else
            {
                // 말이 되냐?
            }
            hScrollBar_Recipe_GoldPowder_Illuminator_Red.Refresh();
            hScrollBar_Recipe_GoldPowder_Illuminator_IR.Refresh();
        }
        private void hScrollBarIlluminator_ValueChanged_IR(object sender, System.EventArgs e)
        {
            if (radioButton_Recipe_GoldPowder_CameraSelection_LowMag.Checked)
            {
                //0:Red-High Mag, 1:IR-High Mag, 2:IR-Low Mag
                workStage.Config.ListIlluminationChannel[2].Value = hScrollBar_Recipe_GoldPowder_Illuminator_IR.Value;
                this.textBox_Recipe_GoldPowder_IlluminationValue_IR.Text = hScrollBar_Recipe_GoldPowder_Illuminator_IR.Value.ToString();
                //1:Red-High Mag, 2:IR-High Mag, 3:IR-Low Mag
                CommonModule.Instance.Illuminator.SetVolume(this.hScrollBar_Recipe_GoldPowder_Illuminator_IR.Value, 3);
            }
            else if (radioButton_Recipe_GoldPowder_CameraSelection_HighMag.Checked)
            {
                //0:Red-High Mag, 1:IR-High Mag, 2:IR-Low Mag
                workStage.Config.ListIlluminationChannel[1].Value = hScrollBar_Recipe_GoldPowder_Illuminator_IR.Value;
                this.textBox_Recipe_GoldPowder_IlluminationValue_IR.Text = hScrollBar_Recipe_GoldPowder_Illuminator_IR.Value.ToString();
                //1:Red-High Mag, 2:IR-High Mag, 3:IR-Low Mag
                CommonModule.Instance.Illuminator.SetVolume(this.hScrollBar_Recipe_GoldPowder_Illuminator_IR.Value, 2);
            }
        }

        private void hScrollBarIlluminator_ValueChanged_Red(object sender, System.EventArgs e)
        {
            if (radioButton_Recipe_GoldPowder_CameraSelection_HighMag.Checked)
            {
                //0:Red-High Mag, 1:IR-High Mag, 2:IR-Low Mag
                workStage.Config.ListIlluminationChannel[0].Value = hScrollBar_Recipe_GoldPowder_Illuminator_Red.Value;
                this.textBox_Recipe_GoldPowder_IlluminationValue_Red.Text = hScrollBar_Recipe_GoldPowder_Illuminator_Red.Value.ToString();
                //1:Red-High Mag, 2:IR-High Mag, 3:IR-Low Mag
                CommonModule.Instance.Illuminator.SetVolume(this.hScrollBar_Recipe_GoldPowder_Illuminator_Red.Value, 1);
            }
        }


        private double ParseDouble(string text)
        {
            return double.TryParse(text, out double result) ? result : 0.0;
        }

        private void button_Recipe_GoldPowder_CameraLive_Click(object sender, EventArgs e)
        {
            this.ImageViewer_Recipe_GoldPowder_highs.StartUpdateTask();
            if (workStage.Camera_HighRes != null)
                workStage.Camera_HighRes.StartLive();
        }

        private void button_Recipe_GoldPowder_CameraStop_Click(object sender, EventArgs e)
        {
            this.ImageViewer_Recipe_GoldPowder_highs.StopUpdateTask();
            if (workStage.Camera_HighRes != null)
                workStage.Camera_HighRes.StopLive();
        }

        private void checkBox_Recipe_GoldPowder_Illuminator_Red_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox_Recipe_GoldPowder_Illuminator_Red.Checked)
            {
                checkBox_Recipe_GoldPowder_Illuminator_Red.Text = "USE";
            }
            else
            {
                checkBox_Recipe_GoldPowder_Illuminator_Red.Text = "UnUSE";
            }
        }

        private void checkBox_Recipe_GoldPowder_Illuminator_IR_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Recipe_GoldPowder_Illuminator_IR.Checked)
            {
                checkBox_Recipe_GoldPowder_Illuminator_IR.Text = "USE";
            }
            else
            {
                checkBox_Recipe_GoldPowder_Illuminator_IR.Text = "UnUSE";
            }
        }

        private void button_Recipe_GoldPowder_Fiducial_Search_Click(object sender, EventArgs e)
        {
            //  원 찾기
            bool bFindCircle = false;
            int nImage_Width = 0;
            int nImage_Height = 0;
            double dTargetSize_Radius = 0.0;
            int nTargetColor = 0;
            bool bTargetColor = false;
            double dSpec = 0.0;
            double dScore = 0.0;
            int nMaxInstance = 0;
            int nFindCount = 0;

            if (Equipment.Machine_LaserType_CO2)
            {
                workStage.m_bCO2_MultyMode = true;    // 386 Model시에 적용.
            }
            else
            {
                workStage.m_bCO2_MultyMode = false;
            }

            if (textBox_Recipe_GoldPowder_Fiducial_CircleSize.Text.Length < 0)
            {
                MessageBox.Show("Fiducial Size 를 입력하세요.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (ImageViewer_Recipe_GoldPowder_highs.Simulated)
            {
                //Owner.Simulated = true;
                workStage.Camera_HighRes.LatestImage = ImageViewer_Recipe_GoldPowder_highs.InputImage;
            }

            dTargetSize_Radius = Equipment.ToDouble(textBox_Recipe_GoldPowder_Fiducial_CircleSize.Text); //  Fiducial 마크 크기
            nMaxInstance = Equipment.ToInt(textBox_Recipe_GoldPowder_Fiducial_MaxInstance.Text); //  Fiducial 마크 최대 개수
            nFindCount = Equipment.ToInt(textBox_Recipe_GoldPowder_Fiducial_FindCount.Text); //  Fiducial 마크 찾기 개수

            //dSpec = Equipment.ToDouble(textBox_Recipe_GoldPowder_Fiducial_CircleSpec.Text); //  Fiducial 마크 Spec
            //dScore = Equipment.ToDouble(textBox_Recipe_GoldPowder_Fiducial_CircleScore.Text); //  Fiducial 마크 Score
            double percentValue = 0.0;
            if (double.TryParse(textBox_Recipe_GoldPowder_Fiducial_CircleSpec.Text, out percentValue))
            {
                // UI에서 입력받은 %를 내부 0~1 값으로 변환
                dSpec = percentValue / 100.0;
            }

            if (double.TryParse(textBox_Recipe_GoldPowder_Fiducial_CircleScore.Text, out percentValue))
            {
                // UI에서 입력받은 %를 내부 0~1 값으로 변환
                dScore = percentValue / 100.0;
            }

            if (radioButton_Recipe_GoldPowder_Fiducial_White.Checked)
            {
                nTargetColor = 1;          //  Fiducial 마크 색깔 //  0: Black, 1: White
                bTargetColor = false;
            }
            else if (radioButton_Recipe_GoldPowder_Fiducial_Black.Checked)
            {
                nTargetColor = 0;          //  Fiducial 마크 색깔 //  0: Black, 1: White
                bTargetColor = true;
            }

            QMC_ImageProcessFindAlignResult result = new QMC_ImageProcessFindAlignResult();
            QMC_ImageProcessFindAlign aligner = new QMC_ImageProcessFindAlign();
            
            List<RectangleF> circlesResult = new List<RectangleF>();
            {
                int w = workStage.Camera_HighRes.Resolution.Width;
                int h = workStage.Camera_HighRes.Resolution.Height;
                nImage_Width = w;
                nImage_Height = h;
                double m_dradius = 0.0;
                m_dradius = dTargetSize_Radius / workStage.Config.ParamConfig.UpperVision_Scale_X;

                //Rectangle rectangle = new Rectangle(0, 0, w, h);
                //rectangle.X = Equipment.ToInt(textBox_Recipe_Goldpowder_ROI_START_X.Text);
                //rectangle.Y = Equipment.ToInt(textBox_Recipe_Goldpowder_ROI_START_Y.Text);
                //rectangle.Width = Equipment.ToInt(textBox_Recipe_Goldpowder_ROI_END_X.Text);
                //rectangle.Height = Equipment.ToInt(textBox_Recipe_Goldpowder_ROI_END_Y.Text);
                // 교체
                int sx = Equipment.ToInt(textBox_Recipe_Goldpowder_ROI_START_X.Text);
                int sy = Equipment.ToInt(textBox_Recipe_Goldpowder_ROI_START_Y.Text);
                int ex = Equipment.ToInt(textBox_Recipe_Goldpowder_ROI_END_X.Text);
                int ey = Equipment.ToInt(textBox_Recipe_Goldpowder_ROI_END_Y.Text);

                // 좌표 정규화 + 영상 경계 클램프
                int left = Math.Max(0, Math.Min(sx, ex));
                int top = Math.Max(0, Math.Min(sy, ey));
                int right = Math.Min(w, Math.Max(sx, ex));
                int bottom = Math.Min(h, Math.Max(sy, ey));

                // 폭/높이 계산 (음수 방지)
                int roiW = Math.Max(0, right - left);
                int roiH = Math.Max(0, bottom - top);

                // 최종 ROI
                Rectangle rectangle = new Rectangle(left, top, roiW, roiH);
                {
                    if(workStage.m_bCO2_MultyMode)
                    {
                        result = aligner.FindCirclesWidthCircleBoundaryMultipleCircles(
                                        circlesResult,
                                        workStage.Camera_HighRes.LatestImage.RawData,
                                        w, h,
                                        (int)m_dradius,
                                        dSpec,
                                        nMaxInstance,          // 최대 20개 원 탐색
                                        bTargetColor,                  // 검은 원 :: true
                                        dScore,
                                        false,
                                        rectangle
                                    );
                    }
                    else
                    {
                        result = aligner.FindGoldPowderForAutoTreshold(circlesResult,
                                                            workStage.Camera_HighRes.LatestImage.RawData,
                                                            w, h, (int)m_dradius, dScore, dSpec, nMaxInstance);
                    }
                        
                    if (circlesResult.Count >= 1)
                    {
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
                listBox_Recipe_GoldPowder_Fiducial_Result.Items.Clear();
                for (int i = 0; i < circlesResult.Count; i++)
                {
                    listBox_Recipe_GoldPowder_Fiducial_Result.Items.Add((i + 1) + ".  Left-Top X : " + circlesResult[i].X);
                    listBox_Recipe_GoldPowder_Fiducial_Result.Items.Add((i + 1) + ".  Left-Top Y : " + circlesResult[i].Y);
                    listBox_Recipe_GoldPowder_Fiducial_Result.Items.Add((i + 1) + ".  Width : " + circlesResult[i].Width);
                    listBox_Recipe_GoldPowder_Fiducial_Result.Items.Add((i + 1) + ".  Height : " + circlesResult[i].Height);

                    dCenterPosX = circlesResult[i].X + (circlesResult[i].Width / 2);
                    dCenterPosY = circlesResult[i].Y + (circlesResult[i].Height / 2);
                    listBox_Recipe_GoldPowder_Fiducial_Result.Items.Add((i + 1) + ".  Center X : " + dCenterPosX);
                    listBox_Recipe_GoldPowder_Fiducial_Result.Items.Add((i + 1) + ".  Center Y: " + dCenterPosY);
                }
            }
            else
            {
                var mb = new MessageBoxOk();
                mb.ShowDialog("Information !", "원 찾기 실패");
                //MessageBox.Show("원 찾기 실패", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                listBox_Recipe_GoldPowder_Fiducial_Result.Items.Clear();
            }

            //  원 찾기 후 다시 Live
            if (workStage.Camera_HighRes.Opened)
            {
                workStage.Camera_HighRes.StartLive();
            }
        }

        private void button_Recipe_GoldPowder_Camera_ExposureTime_Click(object sender, EventArgs e)
        {
            double dExposureTime = Equipment.ToDouble(textBox_Recipe_GoldPowder_Camera_ExposureTime.Text);
            if (workStage.jigAligner_HighRes.Camera.Opened)
            {
                workStage.jigAligner_HighRes.Camera.SetExposureTime(dExposureTime);
            }
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

        private void button_Recipe_GoldPowder_Illuminator_FineCamRed_Click(object sender, EventArgs e)
        {
            string strTemp = textBox_Recipe_GoldPowder_IlluminationValue_Red.Text;
            textBox_Recipe_GoldPowder_Illuminator_FineCamRed.Text = strTemp;
        }

        private void button_Recipe_GoldPowder_Illuminator_FineCamIR_Click(object sender, EventArgs e)
        {
            string strTemp = textBox_Recipe_GoldPowder_IlluminationValue_IR.Text;
            textBox_Recipe_GoldPowder_Illuminator_FineCamIR.Text = strTemp;
        }

        private void button_Recipe_GoldPowder_Position_X1_Click(object sender, EventArgs e)
        {
            CaptureStageXY(out double x, out double y);
            SetPositionUIAndRecipe(m_currentGoldPowderSocketIndex, 1, x, y);
        }

        private void button_Recipe_GoldPowder_Position_X2_Click(object sender, EventArgs e)
        {
            CaptureStageXY(out double x, out double y);
            SetPositionUIAndRecipe(m_currentGoldPowderSocketIndex, 2, x, y);
        }

        private void button_Recipe_GoldPowder_Position_X3_Click(object sender, EventArgs e)
        {
            CaptureStageXY(out double x, out double y);
            SetPositionUIAndRecipe(m_currentGoldPowderSocketIndex, 3, x, y);
        }

        private void button_Recipe_GoldPowder_Position_X4_Click(object sender, EventArgs e)
        {
            CaptureStageXY(out double x, out double y);
            SetPositionUIAndRecipe(m_currentGoldPowderSocketIndex, 4, x, y);
        }

        private void button_Recipe_GoldPowder_AxisZ_Setting_Click(object sender, EventArgs e)
        {

        }

        
        private void comboBox_Recipe_GoldPowder_Socket_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_recipe == null) return;

            m_currentGoldPowderSocketIndex = comboBox_Recipe_GoldPowder_Socket.SelectedIndex;
            ApplyGoldPowderSocketPosToUI(m_currentGoldPowderSocketIndex);
        }


        // (추가) UI -> 소켓 데이터
        private void UpdateGoldPowderSocketPosFromUI(int socketIndex)
        {
            if (m_recipe == null) 
                return;

            if (socketIndex < 0) 
                return;

            m_recipe.EnsureGoldPowderSocketPosCount(socketIndex + 1);

            double x1 = Equipment.ToDouble(textBox_Recipe_GoldPowder_Position_X1.Text);
            double y1 = Equipment.ToDouble(textBox_Recipe_GoldPowder_Position_Y1.Text);
            double x2 = Equipment.ToDouble(textBox_Recipe_GoldPowder_Position_X2.Text);
            double y2 = Equipment.ToDouble(textBox_Recipe_GoldPowder_Position_Y2.Text);
            double x3 = Equipment.ToDouble(textBox_Recipe_GoldPowder_Position_X3.Text);
            double y3 = Equipment.ToDouble(textBox_Recipe_GoldPowder_Position_Y3.Text);
            double x4 = Equipment.ToDouble(textBox_Recipe_GoldPowder_Position_X4.Text);
            double y4 = Equipment.ToDouble(textBox_Recipe_GoldPowder_Position_Y4.Text);

            m_recipe.SetGoldPowderPos(socketIndex, 1, x1, y1);
            m_recipe.SetGoldPowderPos(socketIndex, 2, x2, y2);
            m_recipe.SetGoldPowderPos(socketIndex, 3, x3, y3);
            m_recipe.SetGoldPowderPos(socketIndex, 4, x4, y4);
        }

        private void UpdateGoldPowderSocketPosFromUI_Offset(int socketIndex)
        {
            if (m_recipe == null) return;
            if (socketIndex < 0) return;
            m_recipe.EnsureGoldPowderSocketPosCount_Offset(socketIndex + 1);

            double x1 = Equipment.ToDouble(textBox_Recipe_GoldPowder_Position_X1_Offset.Text);
            double y1 = Equipment.ToDouble(textBox_Recipe_GoldPowder_Position_Y1_Offset.Text);
            double x2 = Equipment.ToDouble(textBox_Recipe_GoldPowder_Position_X2_Offset.Text);
            double y2 = Equipment.ToDouble(textBox_Recipe_GoldPowder_Position_Y2_Offset.Text);
            double x3 = Equipment.ToDouble(textBox_Recipe_GoldPowder_Position_X3_Offset.Text);
            double y3 = Equipment.ToDouble(textBox_Recipe_GoldPowder_Position_Y3_Offset.Text);
            double x4 = Equipment.ToDouble(textBox_Recipe_GoldPowder_Position_X4_Offset.Text);
            double y4 = Equipment.ToDouble(textBox_Recipe_GoldPowder_Position_Y4_Offset.Text);

            m_recipe.SetGoldPowderPos_Offset(socketIndex, 1, x1, y1);
            m_recipe.SetGoldPowderPos_Offset(socketIndex, 2, x2, y2);
            m_recipe.SetGoldPowderPos_Offset(socketIndex, 3, x3, y3);
            m_recipe.SetGoldPowderPos_Offset(socketIndex, 4, x4, y4);
        }

        // (추가) 소켓 데이터 -> UI
        private void ApplyGoldPowderSocketPosToUI(int socketIndex)
        {
            if (m_recipe == null) return;
            var gp = m_recipe.GetGoldPowderSocketPos(socketIndex);
            if (gp == null) return;

            textBox_Recipe_GoldPowder_Position_X1.Text = gp.X[0].ToString("F3");
            textBox_Recipe_GoldPowder_Position_Y1.Text = gp.Y[0].ToString("F3");
            textBox_Recipe_GoldPowder_Position_X2.Text = gp.X[1].ToString("F3");
            textBox_Recipe_GoldPowder_Position_Y2.Text = gp.Y[1].ToString("F3");
            textBox_Recipe_GoldPowder_Position_X3.Text = gp.X[2].ToString("F3");
            textBox_Recipe_GoldPowder_Position_Y3.Text = gp.Y[2].ToString("F3");
            textBox_Recipe_GoldPowder_Position_X4.Text = gp.X[3].ToString("F3");
            textBox_Recipe_GoldPowder_Position_Y4.Text = gp.Y[3].ToString("F3");
        }

        private void ApplyGoldPowderSocketPosToUI_Offset(int SocketIndex)
        {
            if (m_recipe == null) 
                return;

            var gp = m_recipe.GetGoldPowderSocketPos_Offset(SocketIndex);
            if (gp == null) 
                return;

            textBox_Recipe_GoldPowder_Position_X1_Offset.Text = gp.X[0].ToString("F3");
            textBox_Recipe_GoldPowder_Position_Y1_Offset.Text = gp.Y[0].ToString("F3");
            textBox_Recipe_GoldPowder_Position_X2_Offset.Text = gp.X[1].ToString("F3");
            textBox_Recipe_GoldPowder_Position_Y2_Offset.Text = gp.Y[1].ToString("F3");
            textBox_Recipe_GoldPowder_Position_X3_Offset.Text = gp.X[2].ToString("F3");
            textBox_Recipe_GoldPowder_Position_Y3_Offset.Text = gp.Y[2].ToString("F3");
            textBox_Recipe_GoldPowder_Position_X4_Offset.Text = gp.X[3].ToString("F3");
            textBox_Recipe_GoldPowder_Position_Y4_Offset.Text = gp.Y[3].ToString("F3");
        }

        // (선택) 저장 전에 소켓[0]을 기존 전역 변수로 동기화 (레거시 호환)
        private void SyncLegacyGoldPowderPosFromSocket0()
        {
            var gp0 = m_recipe.GetGoldPowderSocketPos(0);
            if (gp0 == null) return;
            m_recipe.dGoldPowderPos1X = gp0.X[0]; m_recipe.dGoldPowderPos1Y = gp0.Y[0];
            m_recipe.dGoldPowderPos2X = gp0.X[1]; m_recipe.dGoldPowderPos2Y = gp0.Y[1];
            m_recipe.dGoldPowderPos3X = gp0.X[2]; m_recipe.dGoldPowderPos3Y = gp0.Y[2];
            m_recipe.dGoldPowderPos4X = gp0.X[3]; m_recipe.dGoldPowderPos4Y = gp0.Y[3];
        }

        private void SyncLegacyGoldPowderPosFromSocket0_Offset()
        {
            var gp0 = m_recipe.GetGoldPowderSocketPos_Offset(0);
            if (gp0 == null) return;
            m_recipe.dGoldPowderPos1XOffset = gp0.X[0]; m_recipe.dGoldPowderPos1YOffset = gp0.Y[0];
            m_recipe.dGoldPowderPos2XOffset = gp0.X[1]; m_recipe.dGoldPowderPos2YOffset = gp0.Y[1];
            m_recipe.dGoldPowderPos3XOffset = gp0.X[2]; m_recipe.dGoldPowderPos3YOffset = gp0.Y[2];
            m_recipe.dGoldPowderPos4XOffset = gp0.X[3]; m_recipe.dGoldPowderPos4YOffset = gp0.Y[3];
        }

        // 실제 장비/공정 상태로부터 소켓 총수를 계산
        private void RefreshGoldPowderSocketTotal()
        {
            int count = 0;
            try
            {
                if (workStage != null)
                {
                    // 우선순위: drilling 전체 > drilling 진행 수 > marking > 기타
                    if(workStage.m_stLaserDrilling_SocketData[0].nGroup_Num > 0)
                        count = workStage.m_stLaserDrilling_SocketData[0].nGroup_Num;
                }

                //if (count <= 0)
                //{
                //    // 글로벌 매니저 (있다면) 활용
                //    try
                //    {
                //        var mgr = QMC.Common.Global.DrillingProcessManager.Instance;
                //        if (mgr != null)
                //            count = mgr.GetTotalSocketCount(true); // onlyUsedSockets=true
                //    }
                //    catch { /* 싱글톤 미초기화 상황 대비 */ }
                //}
            }
            catch { }

            if (count <= 0)
                count = 1; // 최소 1 보장

            m_goldPowderSocketTotal = count;

            // 레시피 리스트에도 최소 count 개 확보
            if (m_recipe != null)
                m_recipe.EnsureGoldPowderSocketPosCount(m_goldPowderSocketTotal);

            // 선택 인덱스 범위 보정
            if (m_currentGoldPowderSocketIndex >= m_goldPowderSocketTotal)
                m_currentGoldPowderSocketIndex = m_goldPowderSocketTotal - 1;

            // 레시피 리스트에도 최소 count 개 확보
            if (m_recipe != null)
                m_recipe.EnsureGoldPowderSocketPosCount_Offset(m_goldPowderSocketTotal);

            // 선택 인덱스 범위 보정
            if (m_currentGoldPowderSocketIndexOffset >= m_goldPowderSocketTotal)
                m_currentGoldPowderSocketIndexOffset = m_goldPowderSocketTotal - 1;
        }

        // (추가) 캡처/공통 유틸 메서드들 : 클래스 내부 아무 private 메서드 영역에 추가
        private void CaptureStageXY(out double x, out double y)
        {
            x = workStage?.MC_Func?.MC_GetEncPos((int)WorkStage.nAxis.X) ?? 0.0;
            y = workStage?.MC_Func?.MC_GetEncPos((int)WorkStage.nAxis.Y) ?? 0.0;

            XyCoordinate xyCoordinate = new XyCoordinate(x, y);
            if (workStage.m_bPreAlignCompleted)
            {
                //역치환
                workStage.ConvertPreAlignData(xyCoordinate, false);
            }
            else
            {
               var mb = new MessageBoxOk();
                mb.ShowDialog("Warning!", "Pre-Align Data가 설정되지 않았습니다.");
            }

            x = xyCoordinate.X;
            y = xyCoordinate.Y;

        }
        private string FormatPos(double v) => v.ToString("0.000");

        private void SetPositionUIAndRecipe(int socketIndex, int posIndex, double x, double y)
        {
            // UI
            switch (posIndex)
            {
                case 1:
                    textBox_Recipe_GoldPowder_Position_X1.Text = FormatPos(x);
                    textBox_Recipe_GoldPowder_Position_Y1.Text = FormatPos(y);
                    break;
                case 2:
                    textBox_Recipe_GoldPowder_Position_X2.Text = FormatPos(x);
                    textBox_Recipe_GoldPowder_Position_Y2.Text = FormatPos(y);
                    break;
                case 3:
                    textBox_Recipe_GoldPowder_Position_X3.Text = FormatPos(x);
                    textBox_Recipe_GoldPowder_Position_Y3.Text = FormatPos(y);
                    break;
                case 4:
                    textBox_Recipe_GoldPowder_Position_X4.Text = FormatPos(x);
                    textBox_Recipe_GoldPowder_Position_Y4.Text = FormatPos(y);
                    break;
            }
            // 레시피(소켓 구조)
            m_recipe?.SetGoldPowderPos(socketIndex, posIndex, x, y);
            // 레거시 첫 소켓 동기화 (선택)
            if (socketIndex == 0)
                SyncLegacyGoldPowderPosFromSocket0();
        }

        private void SetPositionUIAndRecipe_Offset(int socketIndex, int posIndex, double x, double y)
        {
            // UI
            switch (posIndex)
            {
                case 1:
                    textBox_Recipe_GoldPowder_Position_X1_Offset.Text = FormatPos(x);
                    textBox_Recipe_GoldPowder_Position_Y1_Offset.Text = FormatPos(y);
                    break;
                case 2:
                    textBox_Recipe_GoldPowder_Position_X2_Offset.Text = FormatPos(x);
                    textBox_Recipe_GoldPowder_Position_Y2_Offset.Text = FormatPos(y);
                    break;
                case 3:
                    textBox_Recipe_GoldPowder_Position_X3_Offset.Text = FormatPos(x);
                    textBox_Recipe_GoldPowder_Position_Y3_Offset.Text = FormatPos(y);
                    break;
                case 4:
                    textBox_Recipe_GoldPowder_Position_X4_Offset.Text = FormatPos(x);
                    textBox_Recipe_GoldPowder_Position_Y4_Offset.Text = FormatPos(y);
                    break;
            }
            // 레시피(소켓 구조)
            m_recipe?.SetGoldPowderPos_Offset(socketIndex, posIndex, x, y);
            // 레거시 첫 소켓 동기화 (선택)
            if (socketIndex == 0)
                SyncLegacyGoldPowderPosFromSocket0_Offset();
        }

        private void button_Recipe_GoldPowder_Position_Init_Click(object sender, EventArgs e)
        {
            try
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question?", $"소켓 초기화 하시겠습니까?"))
                    return;

                bool ok = InitializeGoldPowderMotorPositions_FromDrawing(m_currentGoldPowderSocketIndex, overwrite: true, showMessage: true);
                if (ok)
                {
                    UpdateGoldPowderSocketPosFromUI(m_currentGoldPowderSocketIndex);
                    if (m_currentGoldPowderSocketIndex == 0)
                        SyncLegacyGoldPowderPosFromSocket0();
                }
                else
                {
                    MessageBox.Show("초기화 실패 또는 기존 데이터가 이미 존재합니다.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                MessageBox.Show("포지션 초기화 중 오류가 발생했습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //try
            //{
            //    string strTemp = string.Empty;
            //    var alignPositions = HoleAlignHelper.CalculateAlignmentPoints(m_currentGoldPowderSocketIndex, workStage.m_stLaserDrilling_SocketData);
            //    if (alignPositions == null || alignPositions.CornerPoints == null || alignPositions.CornerPoints.Count < 4)
            //    {
            //        Log.Write("Goldpowder", "Position_Init", "CornerPoints 부족 또는 null");
            //        MessageBox.Show("정렬 포인트 계산 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;
            //    }

            //    for (int i = 0; i < 4; i++)
            //    {
            //        // (패치) 도면 좌표 -> 장비(FineCam 기준) 좌표 변환 로직 강화
            //        double dTargetX = alignPositions.CornerPoints[i].X;
            //        double dTargetY = alignPositions.CornerPoints[i].Y;

            //        strTemp = string.Format(
            //            "Fiducial Mark No: {0}, Drawing TargetX: {1:F4}, TargetY: {2:F4}",
            //            i+1,
            //            dTargetX, dTargetY
            //        );
            //        Log.Write("Goldpowder", "Result", strTemp);

            //        XyCoordinate xyDraw = new XyCoordinate(dTargetX, dTargetY);

            //        // 2) FineCam 보정/보간 포함된 변환
            //        XyCoordinate xyConverted = workStage.ConvertPointFineCam(xyDraw);

            //        // 3) 변환값 검증 (이상치 필터 – 필요 시 조건 수정)
            //        if (Math.Abs(xyConverted.X) < 0.0001 && Math.Abs(xyConverted.Y) < 0.0001)
            //        {
            //            Log.Write("Goldpowder", "Result",
            //                $"[Warn] ConvertPointFineCam 결과가 (0,0)에 근접. 입력(Draw:{xyDraw.X:F4},{xyDraw.Y:F4})");
            //            // 필요 시 fallback 로직 (예: 그대로 사용)
            //            xyConverted = xyDraw;
            //        }
            //        // UI + Recipe 반영 (i:0~3 -> Pos1~Pos4)
            //        SetPositionUIAndRecipe(m_currentGoldPowderSocketIndex, i + 1, xyConverted.X, xyConverted.Y);

            //        string logMsg =
            //            $"[Socket {m_currentGoldPowderSocketIndex + 1}, Pos{i + 1}] Calc Alignment\n" +
            //            $"X={xyConverted.X:F4}  Y={xyConverted.Y:F4}";

            //        Log.Write("Goldpowder", "button_Recipe_GoldPowder_Position_Init_Click", logMsg);

            //        //여기서 도면 Data를 장비 Pos 값으로 변경하자.
            //        //xyCoordinateAlign = workStage.ConvertPointFineCam(new XyCoordinate(dX, dY));
            //        //xyInterpolatedCoordinate = xyCoordinateAlign;

            //    }

            //    // 현재 소켓 저장 구조 재확인 / 레거시 동기화
            //    UpdateGoldPowderSocketPosFromUI(m_currentGoldPowderSocketIndex);
            //    if (m_currentGoldPowderSocketIndex == 0)
            //        SyncLegacyGoldPowderPosFromSocket0();

            //    MessageBox.Show("도면 기준 4포인트를 적용했습니다.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            //catch (Exception ex)
            //{
            //    Log.Write(ex);
            //    MessageBox.Show("포지션 초기화 중 오류가 발생했습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        // (추가) GoldPowder 소켓 포지션 비어있을 때 자동 초기화 & 저장 기능 관련 유틸리티 메서드들

        // 모든 값이 0이면 비어있다고 판단
        private bool IsGoldPowderSocketPosEmpty(int socketIndex)
        {
            if (m_recipe == null) return true;
            var gp = m_recipe.GetGoldPowderSocketPos(socketIndex);
            if (gp == null) return true;

            bool anyNonZero = false;
            for (int i = 0; i < 4; i++)
            {
                if (Math.Abs(gp.X[i]) > double.Epsilon || Math.Abs(gp.Y[i]) > double.Epsilon)
                {
                    anyNonZero = true;
                    break;
                }
            }
            return !anyNonZero;
        }

        private bool IsGoldPowderSocketPosEmpty_Offset(int socketIndex)
        {
            if (m_recipe == null) return true;
            var gp = m_recipe.GetGoldPowderSocketPos_Offset(socketIndex);
            if (gp == null) return true;

            bool anyNonZero = false;
            for (int i = 0; i < 4; i++)
            {
                if (Math.Abs(gp.X[i]) > double.Epsilon || Math.Abs(gp.Y[i]) > double.Epsilon)
                {
                    anyNonZero = true;
                    break;
                }
            }
            return !anyNonZero;
        }

        // 기존 버튼 로직을 재사용할 수 있도록 함수로 분리
        private bool InitializeGoldPowderMotorPositions_FromDrawing(int socketIndex, bool overwrite = false, bool showMessage = true)
        {
            if (m_recipe == null || workStage == null) return false;

            // 덮어쓰기 금지이며 이미 데이터가 있으면 스킵
            if (!overwrite && !IsGoldPowderSocketPosEmpty(socketIndex))
                return false;

            try
            {
                var alignPositions = HoleAlignHelper.CalculateAlignmentPoints(socketIndex, workStage.m_stLaserDrilling_SocketData);
                if (alignPositions == null || alignPositions.CornerPoints == null || alignPositions.CornerPoints.Count < 4)
                {
                    Log.Write("Goldpowder", "AutoInit", $"Socket {socketIndex + 1}: CornerPoints 부족/NULL");
                    return false;
                }

                for (int i = 0; i < 4; i++)
                {
                    double dTargetX = alignPositions.CornerPoints[i].X;
                    double dTargetY = alignPositions.CornerPoints[i].Y;

                    XyCoordinate xyDraw = new XyCoordinate(dTargetX, dTargetY);
                    XyCoordinate xyConverted;
                    try
                    {
                        //Stage 좌표계로 변환 (FineCam 기준 보정/보간 포함)
                        xyConverted = workStage.ConvertPointFineCam(xyDraw);
                        if (Math.Abs(xyConverted.X) < 0.0001 && Math.Abs(xyConverted.Y) < 0.0001)
                        {
                            Log.Write("Goldpowder", "AutoInit",
                                $"[Warn] ConvertPointFineCam (0,0) 근접 → 원본 사용. Draw({xyDraw.X:F4},{xyDraw.Y:F4})");
                            xyConverted = xyDraw;
                        }

                        //도면 좌표계 사용
                        //xyConverted = xyDraw;
                    }
                    catch
                    {
                        xyConverted = xyDraw;
                    }

                    m_recipe.SetGoldPowderPos(socketIndex, i + 1, xyConverted.X, xyConverted.Y);

                    Log.Write("Goldpowder", "AutoInit",
                        $"Socket {socketIndex + 1} Pos{i + 1}: Draw({xyDraw.X:F4},{xyDraw.Y:F4}) -> Stage({xyConverted.X:F4},{xyConverted.Y:F4})");
                }

                if (socketIndex == 0)
                    SyncLegacyGoldPowderPosFromSocket0();

                // 현재 선택된 소켓이면 UI 갱신
                if (socketIndex == m_currentGoldPowderSocketIndex)
                    ApplyGoldPowderSocketPosToUI(socketIndex);

                if (showMessage)
                    MessageBox.Show($"Socket {socketIndex + 1} 포지션 자동 초기화 완료", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return true;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }
        }

        // 레시피 로드 후 비어있는 소켓 자동 초기화 & 저장
        private void AutoInitializeEmptyGoldPowderPositionsAndSave()
        {
            if (m_recipe == null) return;

            bool changed = false;
            m_recipe.EnsureGoldPowderSocketPosCount(m_goldPowderSocketTotal);
            m_recipe.EnsureGoldPowderSocketPosCount_Offset(m_goldPowderSocketTotal);

            for (int s = 0; s < m_goldPowderSocketTotal; s++)
            {
                if (IsGoldPowderSocketPosEmpty(s))
                {
                    bool initOk = InitializeGoldPowderMotorPositions_FromDrawing(s, overwrite: false, showMessage: false);
                    if (initOk)
                        changed = true;
                }

                if(IsGoldPowderSocketPosEmpty_Offset(s))
                {

                }
            }

            if (changed)
            {
                // 자동 초기화된 내용 저장
                try
                {
                    SyncLegacyGoldPowderPosFromSocket0();
                    SyncLegacyGoldPowderPosFromSocket0_Offset();
                    SaveRecipe();
                    Log.Write("Goldpowder", "AutoInit", "빈 GoldPowder 소켓 포지션 자동 초기화 & 저장 완료");
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                }
            }
        }

        // (추가) 공통: 축 완료 대기 (Done & Inposition) - 타임아웃(ms)
        private bool WaitAxisDone(WorkStage.nAxis axis, int timeoutMs)
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            while (sw.ElapsedMilliseconds < timeoutMs)
            {
                bool done = workStage.MC_Func.MC_GetDone((int)axis);
                bool inpos = workStage.MC_Func.MC_GetInposition((int)axis);
                if (done && inpos) return true;
                Thread.Sleep(50);
            }
            return false;
        }

        // (추가) Z 세이프티 위치 선이동
        private bool MoveStageZToSafety()
        {
            try
            {
                if (workStage == null || vision == null) return false;
                if (!workStage.IsInterlock_WorkStageZ_Enabled())
                    return false;

                // 필요 시 Vision_TeachingPosList.Vision_SafetyPos 로 교체 가능
                int index = (int)Vision.Vision_TeachingPosList.Laser_FocusPos;

                if (vision.stVisionTeachingPos == null ||
                    index < 0 || index >= vision.stVisionTeachingPos.Length)
                {
                    Log.Write("Goldpowder", "MoveStageZToSafety", "Vision Teaching Pos 범위 오류");
                    return false;
                }

                double baseZ = vision.stVisionTeachingPos[index].Vision_Z;
                double offsetSocket = workStage.m_dZOffset_SocketHeightCheck;
                double thickness = 0.0;
                if (Equipment.stLayerRecipeSet != null &&
                    Equipment.stLayerRecipeSet.Length > 0)
                {
                    thickness = Equipment.stLayerRecipeSet[0].ModuleInformation_GoldPowder_Thickness;
                }
                double targetZ = baseZ + offsetSocket + thickness;

                int axisZ = (int)WorkStage.nAxis.Z;
                double vel = Equipment.stAxisParam[axisZ].Common_Speed_Fine;
                double acc = Equipment.stAxisParam[axisZ].Common_Acceleration_Fine;

                workStage.MC_Func.MC_MovePosition(axisZ, targetZ, vel, acc, acc);

                // 기존 제공 함수 활용 (Task<bool> 반환 가정)
                // 내부에서 타임아웃이 처리되지 않는다면 별도 타임아웃 로직 래핑 가능
                var waitTask = workStage.WaitUntilInPositionAsync(WorkStage.nAxis.Z, targetZ);
                if (!waitTask.Result)
                {
                    Log.Write("Goldpowder", "MoveStageZToSafety",
                        $"Z 이동 실패 Target:{targetZ:F4} Enc:{workStage.MC_Func.MC_GetEncPos(axisZ):F4}");
                    var mb = new MessageBoxOk();
                    mb.ShowDialog("Warning !", "Z축 이동 실패");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return false;
            }
        }

        // (추가) GoldPowder 포인트 이동 공통
        private void MoveGoldPowderPoint(int pointNo, string xText, string yText)
        {
            double targetX = Equipment.ToDouble(xText);
            double targetY = Equipment.ToDouble(yText);

            if (!workStage.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            if (targetX == 0.0 && targetY == 0.0)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "위치가 설정되어 있지 않습니다.");
                return;
            }

            if (!workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.X) ||
                !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Y) ||
                !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Z))
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Stage 가 이동중입니다.");
                return;
            }

            // 사용자 확인
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", $"포인트 {pointNo} 위치로 이동하시겠습니까?"))
                return;

            // 1) Z 세이프티 선이동
            if (!MoveStageZToSafety())
                return;

            // 2) XY 이동
            double vel = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Coarse;
            double acc = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Acceleration_Coarse;

            XyCoordinate xy = new XyCoordinate { X = targetX, Y = targetY };

            // PreAlign Data 적용/미적용 :: 이 위치에서 변경되면 안됨!!
            //if (Equipment.Machine_PreAlign_First_Enable && workStage.m_bPreAlignCompleted)
            if (workStage.m_bPreAlignCompleted)
            {
                xy = workStage.ConvertPreAlignData(new XyCoordinate(xy.X, xy.Y));
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PreAlign Data 가 적용되지 않았습니다.");
            }

            workStage.MC_Func.MovePosition(xy, vel, acc, acc);
        }

        private void button_Recipe_GoldPowder_Position_Move_XY1_Click(object sender, EventArgs e)
        {
            MoveGoldPowderPoint(1,
                                textBox_Recipe_GoldPowder_Position_X1.Text,
                                textBox_Recipe_GoldPowder_Position_Y1.Text);
            //기존코드
            {
                //double lfTargetX = 0.0f;
                //double lfTargetY = 0.0f;
                //double lfVelocity = 0.0f;
                //double lfAccDec = 0.0f;

                //if (!workStage.m_bHomeOK)
                //{
                //    var mb1 = new MessageBoxOk();
                //    mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                //    return;
                //}

                //if (Equipment.ToDouble(textBox_Recipe_GoldPowder_Position_X1.Text) == 0.0 && Equipment.ToDouble(textBox_Recipe_GoldPowder_Position_Y1.Text) == 0.0)
                //{
                //    var mb1 = new MessageBoxOk();
                //    mb1.ShowDialog("Warning !", "위치가 설정되어 있지 않습니다.");
                //    return;
                //}

                //var mb = new MessageBoxYesNo();
                //if (DialogResult.Yes != mb.ShowDialog("Question ?", "위치로 이동하시겠습니까?"))
                //    return;

                //if (!workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.X) || !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Y) || !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Z) ||
                //    !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.X) || !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Y) || !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Z))
                //{
                //    var mb1 = new MessageBoxOk();
                //    mb1.ShowDialog("Warning !", "Stage 가 이동중입니다.");
                //    return;
                //}

                ////  Target 위치
                //lfTargetX = Equipment.ToDouble(textBox_Recipe_GoldPowder_Position_X1.Text);
                //lfTargetY = Equipment.ToDouble(textBox_Recipe_GoldPowder_Position_Y1.Text);

                //lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Coarse;
                //lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Acceleration_Coarse;

                //XyCoordinate xyInterpolatedCoordinate = new XyCoordinate();
                //xyInterpolatedCoordinate.X = lfTargetX;
                //xyInterpolatedCoordinate.Y = lfTargetY;
                //workStage.MC_Func.MovePosition(xyInterpolatedCoordinate, lfVelocity, lfAccDec, lfAccDec);
            }
            
        }

        private void button_Recipe_GoldPowder_Position_Move_XY2_Click(object sender, EventArgs e)
        {
            MoveGoldPowderPoint(2,
                                textBox_Recipe_GoldPowder_Position_X2.Text,
                                textBox_Recipe_GoldPowder_Position_Y2.Text);
        }

        private void button_Recipe_GoldPowder_Position_Move_XY3_Click(object sender, EventArgs e)
        {
            MoveGoldPowderPoint(3,
                                textBox_Recipe_GoldPowder_Position_X3.Text,
                                textBox_Recipe_GoldPowder_Position_Y3.Text);
        }

        private void button_Recipe_GoldPowder_Position_Move_XY4_Click(object sender, EventArgs e)
        {
            MoveGoldPowderPoint(4,
                                textBox_Recipe_GoldPowder_Position_X4.Text,
                                textBox_Recipe_GoldPowder_Position_Y4.Text);
        }

        private void button_Recipe_GoldPowder_Position_Init_All_Click(object sender, EventArgs e)
        {
            // 사용자 확인
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question?", $"전체 초기화 하시겠습니까?"))
                return;

            try
            {
                bool anyInit = false;
                for (int s = 0; s < m_goldPowderSocketTotal; s++)
                {
                    bool initOk = InitializeGoldPowderMotorPositions_FromDrawing(s, overwrite: true, showMessage: false);
                    if (initOk)
                        anyInit = true;
                }
                if (anyInit)
                {
                    UpdateGoldPowderSocketPosFromUI(m_currentGoldPowderSocketIndex);
                    if (m_currentGoldPowderSocketIndex == 0)
                        SyncLegacyGoldPowderPosFromSocket0();
                    MessageBox.Show("전체 소켓 포지션을 초기화했습니다.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("초기화 실패 또는 기존 데이터가 이미 존재합니다.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                MessageBox.Show("포지션 초기화 중 오류가 발생했습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox_Recipe_GoldPowder_Socket_Offset_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_recipe == null) return;

            m_currentGoldPowderSocketIndexOffset = comboBox_Recipe_GoldPowder_Socket_Offset.SelectedIndex;
            ApplyGoldPowderSocketPosToUI_Offset(m_currentGoldPowderSocketIndexOffset);
        }

        private void button_Recipe_GoldPowder_Position_Move_XY1_Offset_Click(object sender, EventArgs e)
        {
            int nPositionIndex = 0;
            XyCoordinate xyCoordinate;
            GetGoldPowderDrawingPositions_FromDrawing(nPositionIndex, out xyCoordinate);
            
            string strX = textBox_Recipe_GoldPowder_Position_X1_Offset.Text;
            string strY = textBox_Recipe_GoldPowder_Position_Y1_Offset.Text;
            double dX = Equipment.ToDouble(strX);
            double dY = Equipment.ToDouble(strY);
            xyCoordinate.X += dX;
            xyCoordinate.Y += dY;
            xyCoordinate = xyCoordinate;

            XyCoordinate xyConverted;
            xyConverted = workStage.ConvertPointFineCam(xyCoordinate);
            if (Math.Abs(xyConverted.X) < 0.0001 && Math.Abs(xyConverted.Y) < 0.0001)
            {
                Log.Write("Goldpowder", "MoveGoldPowderPointOffset",
                    $"[Warn] ConvertPointFineCam (0,0) Draw({xyCoordinate.X:F4},{xyCoordinate.Y:F4})" +
                    $" -> Stage({xyConverted.X:F4},{xyConverted.Y:F4})");
            }

            // PreAlign Data 적용/미적용 :: 이 위치에서 변경되면 안됨!!
            //if (Equipment.Machine_PreAlign_First_Enable && workStage.m_bPreAlignCompleted)
             if (workStage.m_bPreAlignCompleted == false && workStage.m_bAlignCompleted == false && workStage.m_bSocketAlign_OK == false)
            {
                xyConverted = workStage.ConvertPreAlignData(new XyCoordinate(xyConverted.X, xyConverted.Y));
                Log.Write("Goldpowder", "MoveGoldPowderPointOffset",
                    $"[Warn] PreAlign : Stage({xyConverted.X:F4},{xyConverted.Y:F4})");
            }
            else if (workStage.m_bPreAlignCompleted || workStage.m_bAlignCompleted || workStage.m_bSocketAlign_OK)
            {
                //여기서 얼라인 안하는게 맞음!!
                //xyConverted = workStage.ConvertPreAlignData(new XyCoordinate(xyConverted.X, xyConverted.Y));
                Log.Write("Goldpowder", "MoveGoldPowderPointOffset",
                    $"[Warn] PreAlign/Align/SocketAlign 완료 : Stage({xyConverted.X:F4},{xyConverted.Y:F4})");
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PreAlign Data 가 적용되지 않았습니다.");
            }

            TargetXyCoordinate[nPositionIndex] = xyConverted;
            MoveGoldPowderPointOffset(nPositionIndex, xyConverted);
        }
        private void button_Recipe_GoldPowder_Position_Move_XY2_Offset_Click(object sender, EventArgs e)
        {
            int nPositionIndex = 1;
            XyCoordinate xyCoordinate;
            GetGoldPowderDrawingPositions_FromDrawing(nPositionIndex, out xyCoordinate);

            string strX = textBox_Recipe_GoldPowder_Position_X2_Offset.Text;
            string strY = textBox_Recipe_GoldPowder_Position_Y2_Offset.Text;
            double dX = Equipment.ToDouble(strX);
            double dY = Equipment.ToDouble(strY);
            xyCoordinate.X += dX;
            xyCoordinate.Y += dY;
            xyCoordinate = xyCoordinate;

            XyCoordinate xyConverted;
            xyConverted = workStage.ConvertPointFineCam(xyCoordinate);
            if (Math.Abs(xyConverted.X) < 0.0001 && Math.Abs(xyConverted.Y) < 0.0001)
            {
                Log.Write("Goldpowder", "MoveGoldPowderPointOffset",
                    $"[Warn] ConvertPointFineCam (0,0) Draw({xyCoordinate.X:F4},{xyCoordinate.Y:F4})" +
                    $" -> Stage({xyConverted.X:F4},{xyConverted.Y:F4})");
            }

            // PreAlign Data 적용/미적용 :: 이 위치에서 변경되면 안됨!!
            //if (Equipment.Machine_PreAlign_First_Enable && workStage.m_bPreAlignCompleted)
            if (workStage.m_bPreAlignCompleted == false && workStage.m_bAlignCompleted == false && workStage.m_bSocketAlign_OK == false)
            {
                xyConverted = workStage.ConvertPreAlignData(new XyCoordinate(xyConverted.X, xyConverted.Y));
                Log.Write("Goldpowder", "MoveGoldPowderPointOffset",
                    $"[Warn] PreAlign : Stage({xyConverted.X:F4},{xyConverted.Y:F4})");
            }
            else if (workStage.m_bPreAlignCompleted || workStage.m_bAlignCompleted || workStage.m_bSocketAlign_OK)
            {
                //여기서 얼라인 안하는게 맞음!!
                //xyConverted = workStage.ConvertPreAlignData(new XyCoordinate(xyConverted.X, xyConverted.Y));
                Log.Write("Goldpowder", "MoveGoldPowderPointOffset",
                    $"[Warn] PreAlign/Align/SocketAlign 완료 : Stage({xyConverted.X:F4},{xyConverted.Y:F4})");
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PreAlign Data 가 적용되지 않았습니다.");
            }

            TargetXyCoordinate[nPositionIndex] = xyConverted;
            MoveGoldPowderPointOffset(nPositionIndex, xyConverted);

        }

        private void button_Recipe_GoldPowder_Position_Move_XY3_Offset_Click(object sender, EventArgs e)
        {
            int nPositionIndex = 2;
            XyCoordinate xyCoordinate;
            GetGoldPowderDrawingPositions_FromDrawing(nPositionIndex, out xyCoordinate);

            string strX = textBox_Recipe_GoldPowder_Position_X3_Offset.Text;
            string strY = textBox_Recipe_GoldPowder_Position_Y3_Offset.Text;
            double dX = Equipment.ToDouble(strX);
            double dY = Equipment.ToDouble(strY);
            xyCoordinate.X += dX;
            xyCoordinate.Y += dY;
            xyCoordinate = xyCoordinate;

            XyCoordinate xyConverted;
            xyConverted = workStage.ConvertPointFineCam(xyCoordinate);
            if (Math.Abs(xyConverted.X) < 0.0001 && Math.Abs(xyConverted.Y) < 0.0001)
            {
                Log.Write("Goldpowder", "MoveGoldPowderPointOffset",
                    $"[Warn] ConvertPointFineCam (0,0) Draw({xyCoordinate.X:F4},{xyCoordinate.Y:F4})" +
                    $" -> Stage({xyConverted.X:F4},{xyConverted.Y:F4})");
            }

            // PreAlign Data 적용/미적용 :: 이 위치에서 변경되면 안됨!!
            //if (Equipment.Machine_PreAlign_First_Enable && workStage.m_bPreAlignCompleted)
            if (workStage.m_bPreAlignCompleted == false && workStage.m_bAlignCompleted == false && workStage.m_bSocketAlign_OK == false)
            {
                xyConverted = workStage.ConvertPreAlignData(new XyCoordinate(xyConverted.X, xyConverted.Y));
                Log.Write("Goldpowder", "MoveGoldPowderPointOffset",
                    $"[Warn] PreAlign : Stage({xyConverted.X:F4},{xyConverted.Y:F4})");
            }
            else if (workStage.m_bPreAlignCompleted || workStage.m_bAlignCompleted || workStage.m_bSocketAlign_OK)
            {
                //여기서 얼라인 안하는게 맞음!!
                //xyConverted = workStage.ConvertPreAlignData(new XyCoordinate(xyConverted.X, xyConverted.Y));
                Log.Write("Goldpowder", "MoveGoldPowderPointOffset",
                    $"[Warn] PreAlign/Align/SocketAlign 완료 : Stage({xyConverted.X:F4},{xyConverted.Y:F4})");
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PreAlign Data 가 적용되지 않았습니다.");
            }

            TargetXyCoordinate[nPositionIndex] = xyConverted;
            MoveGoldPowderPointOffset(nPositionIndex, xyConverted);

        }

        private void button_Recipe_GoldPowder_Position_Move_XY4_Offset_Click(object sender, EventArgs e)
        {
            int nPositionIndex = 3;
            XyCoordinate xyCoordinate;
            GetGoldPowderDrawingPositions_FromDrawing(nPositionIndex, out xyCoordinate);

            string strX = textBox_Recipe_GoldPowder_Position_X4_Offset.Text;
            string strY = textBox_Recipe_GoldPowder_Position_Y4_Offset.Text;
            double dX = Equipment.ToDouble(strX);
            double dY = Equipment.ToDouble(strY);
            xyCoordinate.X += dX;
            xyCoordinate.Y += dY;
            xyCoordinate = xyCoordinate;
            XyCoordinate xyConverted;
            xyConverted = workStage.ConvertPointFineCam(xyCoordinate);
            if (Math.Abs(xyConverted.X) < 0.0001 && Math.Abs(xyConverted.Y) < 0.0001)
            {
                Log.Write("Goldpowder", "MoveGoldPowderPointOffset",
                    $"[Warn] ConvertPointFineCam (0,0) Draw({xyCoordinate.X:F4},{xyCoordinate.Y:F4})" +
                    $" -> Stage({xyConverted.X:F4},{xyConverted.Y:F4})");
            }

            // PreAlign Data 적용/미적용 :: 이 위치에서 변경되면 안됨!!
            //if (Equipment.Machine_PreAlign_First_Enable && workStage.m_bPreAlignCompleted)
            if (workStage.m_bPreAlignCompleted == false && workStage.m_bAlignCompleted == false && workStage.m_bSocketAlign_OK == false)
            {
                xyConverted = workStage.ConvertPreAlignData(new XyCoordinate(xyConverted.X, xyConverted.Y));
                Log.Write("Goldpowder", "MoveGoldPowderPointOffset",
                    $"[Warn] PreAlign : Stage({xyConverted.X:F4},{xyConverted.Y:F4})");
            }
            else if (workStage.m_bPreAlignCompleted || workStage.m_bAlignCompleted || workStage.m_bSocketAlign_OK)
            {
                //여기서 얼라인 안하는게 맞음!!
                //xyConverted = workStage.ConvertPreAlignData(new XyCoordinate(xyConverted.X, xyConverted.Y));
                Log.Write("Goldpowder", "MoveGoldPowderPointOffset",
                    $"[Warn] PreAlign/Align/SocketAlign 완료 : Stage({xyConverted.X:F4},{xyConverted.Y:F4})");
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "PreAlign Data 가 적용되지 않았습니다.");
            }

            TargetXyCoordinate[nPositionIndex] = xyConverted;
            MoveGoldPowderPointOffset(nPositionIndex, xyConverted);
        }

        private void MoveGoldPowderPointOffset(int pointNo, XyCoordinate xyCoordinate)
        {
            if (!workStage.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            if (xyCoordinate.X == 0.0 && xyCoordinate.Y == 0.0)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "위치가 설정되어 있지 않습니다.");
                return;
            }

            if (!workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.X) ||
                !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Y) ||
                !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Z))
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Stage 가 이동중입니다.");
                return;
            }

            // 사용자 확인
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", $"포인트 {pointNo + 1} 위치로 이동하시겠습니까?"))
                return;

            // 1) Z 세이프티 선이동
            if (!MoveStageZToSafety())
                return;

            // 2) XY 이동
            double vel = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Coarse;
            double acc = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Acceleration_Coarse;

            workStage.MC_Func.MovePosition(xyCoordinate, vel, acc, acc);
        }
        private void GetGoldPowderDrawingPositions_FromDrawing(int positionIndex, out XyCoordinate xyCoordinate)
        {
            xyCoordinate = new XyCoordinate();
            if (m_recipe == null || workStage == null) 
                return;
            
            try
            {
                //여기서 얼라인 완료 여부를 확인하자...?
                var alignPositions = HoleAlignHelper.CalculateAlignmentPoints(m_currentGoldPowderSocketIndexOffset, workStage.m_stLaserDrilling_SocketData);
                if (alignPositions == null || alignPositions.CornerPoints == null || alignPositions.CornerPoints.Count < 4)
                {
                    var mb1 = new MessageBoxOk();
                    mb1.ShowDialog("Information !", $"Socket {m_currentGoldPowderSocketIndexOffset + 1}: CornerPoints 부족/NULL");
                    Log.Write("Goldpowder", "AutoInit", $"Socket {m_currentGoldPowderSocketIndexOffset + 1}: CornerPoints 부족/NULL");

                    return;
                }

                double dTargetX = alignPositions.CornerPoints[positionIndex].X;
                double dTargetY = alignPositions.CornerPoints[positionIndex].Y;

                XyCoordinate xyDraw = new XyCoordinate(dTargetX, dTargetY);
                Log.Write("Goldpowder", "SocketDrawPosition",
                    $"Socket {m_currentGoldPowderSocketIndexOffset + 1} Pos{positionIndex + 1}: Draw({xyDraw.X:F4},{xyDraw.Y:F4})");

                xyCoordinate = xyDraw;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                return;
            }
        }

        public XyCoordinate[] TargetXyCoordinate = new XyCoordinate[4];
        private void button_Recipe_GoldPowder_Position_Cal_XY1_Offset_Click(object sender, EventArgs e)
        {
            XyCoordinate xyCurrentPos = new XyCoordinate();
            xyCurrentPos.X = workStage?.MC_Func?.MC_GetEncPos((int)WorkStage.nAxis.X) ?? 0.0;
            xyCurrentPos.Y = workStage?.MC_Func?.MC_GetEncPos((int)WorkStage.nAxis.Y) ?? 0.0;

            //XyCoordinate xyCurrentDrawPos = new XyCoordinate();
            //xyCurrentDrawPos = workStage.ConvertPointFineCam(xyCurrentPos);
            //GetGoldPowderDrawingPositions_FromDrawing(0, out TargetXyCoordinate[0]);
            //XyCoordinate xyTargetPos = new XyCoordinate();
            //xyTargetPos = workStage.ConvertPointFineCam(TargetXyCoordinate[0]);
            //double dX = Equipment.ToDouble(textBox_Recipe_GoldPowder_Position_X1_Offset.Text);
            //double dY = Equipment.ToDouble(textBox_Recipe_GoldPowder_Position_Y1_Offset.Text);
            //xyTargetPos.X += dX;
            //xyTargetPos.Y += dY;
            ////도면
            //XyCoordinate xyCalPos = new XyCoordinate();
            //xyCalPos = xyCurrentDrawPos - xyTargetDrawPos;
            //모터
            XyCoordinate xyCalPos = new XyCoordinate();
            xyCalPos = xyCurrentPos - TargetXyCoordinate[0];
            textBox_Recipe_GoldPowder_Position_X1_Offset.Text = FormatPos(xyCalPos.X * -1);
            textBox_Recipe_GoldPowder_Position_Y1_Offset.Text = FormatPos(xyCalPos.Y * -1);
        }

        private void button_Recipe_GoldPowder_Position_Cal_XY2_Offset_Click(object sender, EventArgs e)
        {
            XyCoordinate xyCurrentPos = new XyCoordinate();
            xyCurrentPos.X = workStage?.MC_Func?.MC_GetEncPos((int)WorkStage.nAxis.X) ?? 0.0;
            xyCurrentPos.Y = workStage?.MC_Func?.MC_GetEncPos((int)WorkStage.nAxis.Y) ?? 0.0;

            //XyCoordinate xyCurrentDrawPos = new XyCoordinate();
            //xyCurrentDrawPos = workStage.ConvertPointFineCam(xyCurrentPos);
            //GetGoldPowderDrawingPositions_FromDrawing(1, out TargetXyCoordinate[1]);
            //XyCoordinate xyTargetPos = new XyCoordinate();
            //xyTargetPos = workStage.ConvertPointFineCam(TargetXyCoordinate[1]);
            //double dX = Equipment.ToDouble(textBox_Recipe_GoldPowder_Position_X1_Offset.Text);
            //double dY = Equipment.ToDouble(textBox_Recipe_GoldPowder_Position_Y1_Offset.Text);
            //xyTargetPos.X += dX;
            //xyTargetPos.Y += dY;
            ////도면
            //XyCoordinate xyCalPos = new XyCoordinate();
            //xyCalPos = xyCurrentDrawPos - xyTargetDrawPos;
            //모터
            XyCoordinate xyCalPos = new XyCoordinate();
            //xyCalPos = xyCurrentPos - xyTargetPos;
            xyCalPos = xyCurrentPos - TargetXyCoordinate[1];
            textBox_Recipe_GoldPowder_Position_X2_Offset.Text = FormatPos(xyCalPos.X * -1);
            textBox_Recipe_GoldPowder_Position_Y2_Offset.Text = FormatPos(xyCalPos.Y * -1);
        }

        private void button_Recipe_GoldPowder_Position_Cal_XY3_Offset_Click(object sender, EventArgs e)
        {
            XyCoordinate xyCurrentPos = new XyCoordinate();
            xyCurrentPos.X = workStage?.MC_Func?.MC_GetEncPos((int)WorkStage.nAxis.X) ?? 0.0;
            xyCurrentPos.Y = workStage?.MC_Func?.MC_GetEncPos((int)WorkStage.nAxis.Y) ?? 0.0;

            //XyCoordinate xyCurrentDrawPos = new XyCoordinate();
            //xyCurrentDrawPos = workStage.ConvertPointFineCam(xyCurrentPos);
            //GetGoldPowderDrawingPositions_FromDrawing(2, out TargetXyCoordinate[2]);
            //XyCoordinate xyTargetPos = new XyCoordinate();
            //xyTargetPos = workStage.ConvertPointFineCam(TargetXyCoordinate[2]);
            //double dX = Equipment.ToDouble(textBox_Recipe_GoldPowder_Position_X1_Offset.Text);
            //double dY = Equipment.ToDouble(textBox_Recipe_GoldPowder_Position_Y1_Offset.Text);
            //xyTargetPos.X += dX;
            //xyTargetPos.Y += dY;
            ////도면
            //XyCoordinate xyCalPos = new XyCoordinate();
            //xyCalPos = xyCurrentDrawPos - xyTargetDrawPos;
            //모터
            XyCoordinate xyCalPos = new XyCoordinate();
            //xyCalPos = xyCurrentPos - xyTargetPos;
            xyCalPos = xyCurrentPos - TargetXyCoordinate[2];
            textBox_Recipe_GoldPowder_Position_X3_Offset.Text = FormatPos(xyCalPos.X * -1);
            textBox_Recipe_GoldPowder_Position_Y3_Offset.Text = FormatPos(xyCalPos.Y * -1);
        }

        private void button_Recipe_GoldPowder_Position_Cal_XY4_Offset_Click(object sender, EventArgs e)
        {
            XyCoordinate xyCurrentPos = new XyCoordinate();
            xyCurrentPos.X = workStage?.MC_Func?.MC_GetEncPos((int)WorkStage.nAxis.X) ?? 0.0;
            xyCurrentPos.Y = workStage?.MC_Func?.MC_GetEncPos((int)WorkStage.nAxis.Y) ?? 0.0;

            //XyCoordinate xyCurrentDrawPos = new XyCoordinate();
            //xyCurrentDrawPos = workStage.ConvertPointFineCam(xyCurrentPos);
            //GetGoldPowderDrawingPositions_FromDrawing(3, out TargetXyCoordinate[3]);
            //XyCoordinate xyTargetPos = new XyCoordinate();
            //xyTargetPos = workStage.ConvertPointFineCam(TargetXyCoordinate[3]);
            //double dX = Equipment.ToDouble(textBox_Recipe_GoldPowder_Position_X1_Offset.Text);
            //double dY = Equipment.ToDouble(textBox_Recipe_GoldPowder_Position_Y1_Offset.Text);
            //xyTargetPos.X += dX;
            //xyTargetPos.Y += dY;
            ////도면
            //XyCoordinate xyCalPos = new XyCoordinate();
            //xyCalPos = xyCurrentDrawPos - xyTargetDrawPos;
            //모터
            XyCoordinate xyCalPos = new XyCoordinate();
            //xyCalPos = xyCurrentPos - xyTargetPos;
            xyCalPos = xyCurrentPos - TargetXyCoordinate[3];
            textBox_Recipe_GoldPowder_Position_X4_Offset.Text = FormatPos(xyCalPos.X * -1);
            textBox_Recipe_GoldPowder_Position_Y4_Offset.Text = FormatPos(xyCalPos.Y * -1);
        }

        private void button_Recipe_GoldPowder_Position_Apply_All_Click(object sender, EventArgs e)
        {
            //1번 소켓의 4개의 Offset값을 전 소켓에 동일하게 적용.
            if (m_recipe == null) return;
            try
            {
                string strX1 = textBox_Recipe_GoldPowder_Position_X1_Offset.Text;
                string strY1 = textBox_Recipe_GoldPowder_Position_Y1_Offset.Text;
                double dX1 = Equipment.ToDouble(strX1);
                double dY1 = Equipment.ToDouble(strY1);
                string strX2 = textBox_Recipe_GoldPowder_Position_X2_Offset.Text;
                string strY2 = textBox_Recipe_GoldPowder_Position_Y2_Offset.Text;
                double dX2 = Equipment.ToDouble(strX2);
                double dY2 = Equipment.ToDouble(strY2);
                string strX3 = textBox_Recipe_GoldPowder_Position_X3_Offset.Text;
                string strY3 = textBox_Recipe_GoldPowder_Position_Y3_Offset.Text;
                double dX3 = Equipment.ToDouble(strX3);
                double dY3 = Equipment.ToDouble(strY3);
                string strX4 = textBox_Recipe_GoldPowder_Position_X4_Offset.Text;
                string strY4 = textBox_Recipe_GoldPowder_Position_Y4_Offset.Text;
                double dX4 = Equipment.ToDouble(strX4);
                double dY4 = Equipment.ToDouble(strY4);
                for (int s = 0; s < m_goldPowderSocketTotal; s++)
                {
                    m_recipe.SetGoldPowderPos_Offset(s, 1, dX1, dY1);
                    m_recipe.SetGoldPowderPos_Offset(s, 2, dX2, dY2);
                    m_recipe.SetGoldPowderPos_Offset(s, 3, dX3, dY3);
                    m_recipe.SetGoldPowderPos_Offset(s, 4, dX4, dY4);
                }
                // 현재 선택된 소켓이면 UI 갱신
                ApplyGoldPowderSocketPosToUI_Offset(m_currentGoldPowderSocketIndexOffset);
                MessageBox.Show("모든 소켓에 Offset 값을 적용했습니다.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                MessageBox.Show("Offset 값 적용 중 오류가 발생했습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void button_Recipe_GoldPowder_Position_Move_Z_Click(object sender, EventArgs e)
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
                //int markIndex = comboBox_Recipe_Fiducial_MarkIndex.SelectedIndex;
                //if (markIndex < 0 || markIndex >= Equipment.stVisionRecipeSet.SocketMarkList.Count)
                //{
                //    var mb = new QMC.Common.UI.MessageBoxOk();
                //    mb.ShowDialog("Warning !", "선택된 Fiducial Mark 가 없습니다.");
                //    return;
                //}
                //var mark = Equipment.stVisionRecipeSet.SocketMarkList[markIndex];

                // 이동 중 여부 (간단 체크)
                if (!workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Z))
                {
                    var mb = new QMC.Common.UI.MessageBoxOk();
                    mb.ShowDialog("Warning !", "Z 축이 이동중입니다.");
                    return;
                }

                // 사용자 확인
                var mbAsk = new QMC.Common.UI.MessageBoxYesNo();
                //if (DialogResult.Yes != mbAsk.ShowDialog("Question ?", $"선택 마크(Z Offset:{mark.AxisZOffset:F3}) 기준으로 Z 축을 이동하시겠습니까?"))
                //    return;
                if (DialogResult.Yes != mbAsk.ShowDialog("Question ?", $"Z 축을 이동하시겠습니까?"))
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
                    thickness = Equipment.stLayerRecipeSet[0].ModuleInformation_GoldPowder_Thickness;
                }
                //double markOffset = mark.AxisZOffset; // 선택된 마크 오프셋
                string strAxisZ = textBox_Recipe_GoldPowder_AxisZ_Setting.Text;
                double markOffset = Equipment.ToDouble(strAxisZ);
                double targetZ = baseZ + offsetSocketHeight + thickness + markOffset;

                // 속도 선택
                Equipment.Type_Motor_Speed speedType = Equipment.Type_Motor_Speed.Fine;
                int axisZ = (int)WorkStage.nAxis.Z;
                double vel = 0.0, acc = 0.0;
                speedType = Equipment.Type_Motor_Speed.Fine;    // Z 축은 항상 정밀 속도로 이동하도록 고정 (2025-09-03)
                if (speedType == Equipment.Type_Motor_Speed.Fine)
                {
                    vel = Equipment.stAxisParam[axisZ].Common_Speed_Fine;
                    acc = Equipment.stAxisParam[axisZ].Common_Acceleration_Fine;
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

        //Vision ROI 및 마크 추가의 건
        public RoiVisionTool RoiTrain { get; set; }
        public RoiVisionTool RoiInspect { get; set; }
        private SLD200_MSL.RoiListControl m_RoiListControl;



        private void button_Recipe_Goldpowder_ROI_Click(object sender, EventArgs e)
        {
            Point startLocation = new Point();
            Point endLocation = new Point();
            startLocation.X = Equipment.ToInt(textBox_Recipe_Goldpowder_ROI_START_X.Text);
            startLocation.Y = Equipment.ToInt(textBox_Recipe_Goldpowder_ROI_START_Y.Text);
            endLocation.X = Equipment.ToInt(textBox_Recipe_Goldpowder_ROI_END_X.Text);
            endLocation.Y = Equipment.ToInt(textBox_Recipe_Goldpowder_ROI_END_Y.Text);
            
            RoiInspect.Parameter.StartLocation = startLocation;
            RoiInspect.Parameter.EndLocation = endLocation;     //recipe.InspectRoiEndLocation;
            RoiTrain.Parameter.Overlay.Visible = false;
            RoiInspect.Parameter.Overlay.Visible = true;

            this.ImageViewer_Recipe_GoldPowder_highs.NormalOverlays.Add(RoiInspect.Parameter.Overlay);
            this.ImageViewer_Recipe_GoldPowder_highs.Display();
            this.m_RoiListControl.RoiGoldpowderClickNew();

            // (추가) 현재 선택 마크에 ROI 반영
            if (m_recipe != null 
                && m_currentGoldPowderMarkIndex >= 0 
                && m_currentGoldPowderMarkIndex < m_recipe.GoldPowderMarkList.Count)
            {
                var mark = m_recipe.GoldPowderMarkList[m_currentGoldPowderMarkIndex];
                mark.InspectRoiStart = startLocation;
                mark.InspectRoiEnd = endLocation;
            }
        }

        private void RoiGoldpowderButtonClick(RoiVisionTool roiVisionTool)
        {
            //if (Owner != null)
            {
                RoiInspect.Parameter.CenterLocation = roiVisionTool.Parameter.CenterLocation;
                RoiInspect.Parameter.Size = roiVisionTool.Parameter.Size;

                this.ImageViewer_Recipe_GoldPowder_highs.NormalOverlays.Add(RoiInspect.Parameter.Overlay);
                RoiInspect.Parameter.Overlay.Visible = true;
                this.ImageViewer_Recipe_GoldPowder_highs.Display();
            }
        }

        private void RoiGoldpowderSaveButtonClick(RoiVisionTool roiVisionTool, bool bOk)
        {
            if (bOk == true)
            {


                textBox_Recipe_Goldpowder_ROI_START_X.Text = roiVisionTool.Parameter.StartLocation.X.ToString();
                textBox_Recipe_Goldpowder_ROI_START_Y.Text = roiVisionTool.Parameter.StartLocation.Y.ToString();
                textBox_Recipe_Goldpowder_ROI_END_X.Text = roiVisionTool.Parameter.EndLocation.X.ToString();
                textBox_Recipe_Goldpowder_ROI_END_Y.Text = roiVisionTool.Parameter.EndLocation.Y.ToString();

                // (추가) 현재 선택 마크에 ROI 반영
                if (m_recipe != null 
                    && m_currentGoldPowderMarkIndex >= 0 
                    && m_currentGoldPowderMarkIndex < m_recipe.GoldPowderMarkList.Count)
                {
                    var mark = m_recipe.GoldPowderMarkList[m_currentGoldPowderMarkIndex];
                    mark.InspectRoiStart = roiVisionTool.Parameter.StartLocation;
                    mark.InspectRoiEnd = roiVisionTool.Parameter.EndLocation;
                }
            }
            else
            {
                textBox_Recipe_Goldpowder_ROI_START_X.Text = "000";
                textBox_Recipe_Goldpowder_ROI_START_Y.Text = "000";
                textBox_Recipe_Goldpowder_ROI_END_X.Text = "000";
                textBox_Recipe_Goldpowder_ROI_END_Y.Text = "000";

                // (추가) 현재 선택 마크 ROI 초기화
                if (m_recipe != null 
                    && m_currentGoldPowderMarkIndex >= 0 
                    && m_currentGoldPowderMarkIndex < m_recipe.GoldPowderMarkList.Count)
                {
                    var mark = m_recipe.GoldPowderMarkList[m_currentGoldPowderMarkIndex];
                    mark.InspectRoiStart = new Point(0, 0);
                    mark.InspectRoiEnd = new Point(0, 0);
                }
            }

            this.ImageViewer_Recipe_GoldPowder_highs.Display();

        }

        private void InitGoldPowderMarkCombo()
        {
            if (m_recipe == null) 
                return;

            // 마크 리스트 최소 1개 보장
            if (m_recipe.GoldPowderMarkList == null)
            {
                m_recipe.GoldPowderMarkList = new List<GoldPowderMarkInfo>();
            }
            if (m_recipe.GoldPowderMarkList.Count == 0)
                m_recipe.GoldPowderMarkList.Add(new GoldPowderMarkInfo());

            var cb = comboBox_Recipe_Goldpowder_MarkIndex;
            if (cb == null)
                return;

            int keepIndex = Math.Max(0, Math.Min(m_currentGoldPowderMarkIndex, m_recipe.GoldPowderMarkList.Count - 1));

            // 초기화 중에는 이벤트 분리하여 0번 마크가 0으로 덮이는 문제 방지
            cb.SelectedIndexChanged -= comboBox_Recipe_Goldpowder_MarkIndex_SelectedIndexChanged;

            cb.Items.Clear();
            for (int i = 0; i < m_recipe.GoldPowderMarkList.Count; i++)
                cb.Items.Add($"Mark {i + 1}");

            cb.SelectedIndex = keepIndex;
            m_currentGoldPowderMarkIndex = keepIndex;

            // 초기화 완료 후 이벤트 재연결
            cb.SelectedIndexChanged += comboBox_Recipe_Goldpowder_MarkIndex_SelectedIndexChanged;
        }

        // (추가) 선택된 마크 → UI 반영
        private void ApplyGoldPowderMarkToUI(int index)
        {
            if (m_recipe == null) 
                return;

            if (index < 0 || index >= m_recipe.GoldPowderMarkList.Count) 
                return;

            var mark = m_recipe.GoldPowderMarkList[index];

            // 정렬 방식: 0 = Pattern, 그 외 = Circle (기존 UI 로직 유지)
            if (mark.AlignType == 0)
            {
                radioButton_Recipe_GoldPowder_Fiducial_Pattern.Checked = true;
                radioButton_Recipe_GoldPowder_Fiducial_Circle.Checked = false;
            }
            else
            {
                radioButton_Recipe_GoldPowder_Fiducial_Pattern.Checked = false;
                radioButton_Recipe_GoldPowder_Fiducial_Circle.Checked = true;
            }

            // 마크 타입: 0 = GoldPowder, 1 = Circle (기존 UI 로직 유지)
            if (mark.MarkType == 0)
            {
                radioButton_Recipe_GoldPowder_Fiducial_Type_GoldPowder.Checked = true;
                radioButton_Recipe_GoldPowder_Fiducial_Type_Circle.Checked = false;
            }
            else
            {
                radioButton_Recipe_GoldPowder_Fiducial_Type_GoldPowder.Checked = false;
                radioButton_Recipe_GoldPowder_Fiducial_Type_Circle.Checked = true;
            }

            // 색상: true = Black, false = White
            if (mark.CircleColor)
            {
                radioButton_Recipe_GoldPowder_Fiducial_White.Checked = false;
                radioButton_Recipe_GoldPowder_Fiducial_Black.Checked = true;
            }
            else
            {
                radioButton_Recipe_GoldPowder_Fiducial_White.Checked = true;
                radioButton_Recipe_GoldPowder_Fiducial_Black.Checked = false;
            }

            // 마크 스펙/스코어/사이즈
            textBox_Recipe_GoldPowder_Fiducial_CircleSize.Text = mark.CircleMarkRadius.ToString("F3");
            textBox_Recipe_GoldPowder_Fiducial_CircleSpec.Text = (mark.CircleMarkSpec * 100.0).ToString("F2");
            textBox_Recipe_GoldPowder_Fiducial_CircleScore.Text = (mark.CircleMarkScore * 100.0).ToString("F2");

            // 최대 개수/찾기 개수
            textBox_Recipe_GoldPowder_Fiducial_MaxInstance.Text = mark.CircleMarkMaxInstance.ToString();
            textBox_Recipe_GoldPowder_Fiducial_FindCount.Text = mark.CircleMarkFindCount.ToString();

            // 조명 사용 여부
            checkBox_Recipe_GoldPowder_Illuminator_IR.Checked = mark.UseIR;
            checkBox_Recipe_GoldPowder_Illuminator_Red.Checked = mark.UseRed;
            checkBox_Recipe_GoldPowder_Illuminator_IR.Text = mark.UseIR ? "USE" : "UnUSE";
            checkBox_Recipe_GoldPowder_Illuminator_Red.Text = mark.UseRed ? "USE" : "UnUSE";

            // 조명 세기 텍스트
            textBox_Recipe_GoldPowder_Illuminator_FineCamIR.Text = mark.IllumIR.ToString();
            textBox_Recipe_GoldPowder_Illuminator_FineCamRed.Text = mark.IllumRed.ToString();

            // 노출/축Z
            textBox_Recipe_GoldPowder_Camera_ExposureTime.Text = mark.ExposureTime.ToString("F1");
            textBox_Recipe_GoldPowder_AxisZ_Setting.Text = mark.AxisZOffset.ToString("F3");

            // (추가) ROI 텍스트 박스 반영
            textBox_Recipe_Goldpowder_ROI_START_X.Text = mark.InspectRoiStart.X.ToString();
            textBox_Recipe_Goldpowder_ROI_START_Y.Text = mark.InspectRoiStart.Y.ToString();
            textBox_Recipe_Goldpowder_ROI_END_X.Text = mark.InspectRoiEnd.X.ToString();
            textBox_Recipe_Goldpowder_ROI_END_Y.Text = mark.InspectRoiEnd.Y.ToString();

            // (선택) 뷰어 오버레이 파라미터도 동기화
            if (RoiInspect != null)
            {
                RoiInspect.Parameter.StartLocation = mark.InspectRoiStart;
                RoiInspect.Parameter.EndLocation = mark.InspectRoiEnd;
            }

            // 스크롤바 세팅을 위해 Equipment.stVisionRecipeSet 값을 동기화
            Equipment.stVisionRecipeSet.nGoldPowderIlluminationRed = mark.IllumRed;
            Equipment.stVisionRecipeSet.nGoldPowderIlluminationIR = mark.IllumIR;
            SetScroll();

            // 선택 마크를 레거시 단일 변수에도 반영(후방 호환)
            SyncLegacyGoldPowderFromMark(mark);
        }

        // (추가) UI → 선택된 마크 반영
        private void UpdateGoldPowderMarkFromUI(int index)
        {
            if (m_recipe == null) 
                return;

            if (index < 0 || index >= m_recipe.GoldPowderMarkList.Count) 
                return;

            var mark = m_recipe.GoldPowderMarkList[index];

            // 정렬 방식 (UI 로직 유지: Pattern=0, Circle=기타)
            mark.AlignType = radioButton_Recipe_GoldPowder_Fiducial_Pattern.Checked ? 0 : 1;

            // 마크 타입
            mark.MarkType = radioButton_Recipe_GoldPowder_Fiducial_Type_Circle.Checked ? 1 : 0;

            // 색상
            mark.CircleColor = radioButton_Recipe_GoldPowder_Fiducial_White.Checked ? false : true;

            // 노출/축Z
            mark.AxisZOffset = Equipment.ToDouble(textBox_Recipe_GoldPowder_AxisZ_Setting.Text);
            mark.ExposureTime = Equipment.ToDouble(textBox_Recipe_GoldPowder_Camera_ExposureTime.Text);

            // 조명 사용 여부
            mark.UseIR = checkBox_Recipe_GoldPowder_Illuminator_IR.Checked;
            mark.UseRed = checkBox_Recipe_GoldPowder_Illuminator_Red.Checked;

            // 조명 세기
            mark.IllumIR = Equipment.ToInt(textBox_Recipe_GoldPowder_Illuminator_FineCamIR.Text);
            mark.IllumRed = Equipment.ToInt(textBox_Recipe_GoldPowder_Illuminator_FineCamRed.Text);

            // 마크 조건
            mark.CircleMarkRadius = Equipment.ToDouble(textBox_Recipe_GoldPowder_Fiducial_CircleSize.Text);
            mark.CircleMarkMaxInstance = Equipment.ToInt(textBox_Recipe_GoldPowder_Fiducial_MaxInstance.Text);
            mark.CircleMarkFindCount = Equipment.ToInt(textBox_Recipe_GoldPowder_Fiducial_FindCount.Text);

            double percentValue;
            if (double.TryParse(textBox_Recipe_GoldPowder_Fiducial_CircleSpec.Text, out percentValue))
                mark.CircleMarkSpec = percentValue / 100.0;
            if (double.TryParse(textBox_Recipe_GoldPowder_Fiducial_CircleScore.Text, out percentValue))
                mark.CircleMarkScore = percentValue / 100.0;

            // (추가) ROI 텍스트 박스 → 마크 반영
            int sX = Equipment.ToInt(textBox_Recipe_Goldpowder_ROI_START_X.Text);
            int sY = Equipment.ToInt(textBox_Recipe_Goldpowder_ROI_START_Y.Text);
            int eX = Equipment.ToInt(textBox_Recipe_Goldpowder_ROI_END_X.Text);
            int eY = Equipment.ToInt(textBox_Recipe_Goldpowder_ROI_END_Y.Text);
            mark.InspectRoiStart = new Point(sX, sY);
            mark.InspectRoiEnd = new Point(eX, eY);

            // (선택) 뷰어 오버레이 파라미터도 동기화
            if (RoiInspect != null)
            {
                RoiInspect.Parameter.StartLocation = mark.InspectRoiStart;
                RoiInspect.Parameter.EndLocation = mark.InspectRoiEnd;
            }

            // 선택 마크를 레거시 단일 변수에도 반영(후방 호환)
            SyncLegacyGoldPowderFromMark(mark);
        }

        // (추가) 선택 마크 → 레거시 단일 변수 동기화
        private void SyncLegacyGoldPowderFromMark(GoldPowderMarkInfo mark)
        {
            if (mark == null || m_recipe == null) return;
            m_recipe.dGoldPowderAlignType = mark.AlignType;
            m_recipe.dGoldPowderMarkType = mark.MarkType;
            m_recipe.bGoldPowderCircleColor = mark.CircleColor;
            m_recipe.dGoldPowderCircleMarkRadius = mark.CircleMarkRadius;
            m_recipe.dGoldPowderCircleMarkSpec = mark.CircleMarkSpec;
            m_recipe.dGoldPowderCircleMarkScore = mark.CircleMarkScore;
            m_recipe.nGoldPowderIlluminationIR = mark.IllumIR;
            m_recipe.nGoldPowderIlluminationRed = mark.IllumRed;
            m_recipe.bGoldPowderIlluminationIRUse = mark.UseIR;
            m_recipe.bGoldPowderIlluminationRedUse = mark.UseRed;
            m_recipe.dGoldPowderIlluminationExposureTime = mark.ExposureTime;
            m_recipe.dGoldPowderAxisZ_Offset = mark.AxisZOffset;
            m_recipe.nGoldPowderCircleMarkMaxInstance = mark.CircleMarkMaxInstance;
            m_recipe.nGoldPowderCircleMarkFindCount = mark.CircleMarkFindCount;
        }

        private void ApplyRecipeToUI()
        {
            if (m_recipe == null || m_recipePath == "")
                return;

            radioButton_Recipe_GoldPowder_CameraSelection_HighMag.Checked = true;

            // (변경) 선택된 마크 기준으로 UI 반영
            ApplyGoldPowderMarkToUI(m_currentGoldPowderMarkIndex);

            // 최초 호출 시 현재 소켓 UI 로드
            ApplyGoldPowderSocketPosToUI(m_currentGoldPowderSocketIndex);
            ApplyGoldPowderSocketPosToUI_Offset(m_currentGoldPowderSocketIndexOffset);
        }

        //private void ApplyRecipeToUI()
        //{
        //    if (m_recipe == null || m_recipePath == "")
        //        return;

        //    radioButton_Recipe_GoldPowder_CameraSelection_HighMag.Checked = true;

        //    // 정렬 방식
        //    if (m_recipe.dGoldPowderAlignType == 0)
        //    {
        //        radioButton_Recipe_GoldPowder_Fiducial_Pattern.Checked = true;
        //        radioButton_Recipe_GoldPowder_Fiducial_Circle.Checked = false;
        //    }
        //    else
        //    {
        //        radioButton_Recipe_GoldPowder_Fiducial_Pattern.Checked = false;
        //        radioButton_Recipe_GoldPowder_Fiducial_Circle.Checked = true;
        //    }

        //    // 마크 타입
        //    if (m_recipe.dGoldPowderMarkType == 0)
        //    {
        //        radioButton_Recipe_GoldPowder_Fiducial_Type_GoldPowder.Checked = true;
        //        radioButton_Recipe_GoldPowder_Fiducial_Type_Circle.Checked = false;

        //    }
        //    else
        //    {
        //        radioButton_Recipe_GoldPowder_Fiducial_Circle.Checked = true;
        //        radioButton_Recipe_GoldPowder_Fiducial_Type_GoldPowder.Checked = false;
        //    }

        //    // 마크 색상
        //    if (m_recipe.bGoldPowderCircleColor)
        //    {
        //        radioButton_Recipe_GoldPowder_Fiducial_White.Checked = false;
        //        radioButton_Recipe_GoldPowder_Fiducial_Black.Checked = true;
        //    }
        //    else
        //    {
        //        radioButton_Recipe_GoldPowder_Fiducial_White.Checked = true;
        //        radioButton_Recipe_GoldPowder_Fiducial_Black.Checked = false;
        //    }

        //    // 마크 스펙
        //    textBox_Recipe_GoldPowder_Fiducial_CircleSize.Text = m_recipe.dGoldPowderCircleMarkRadius.ToString("F3");
        //    textBox_Recipe_GoldPowder_Fiducial_MaxInstance.Text = m_recipe.nGoldPowderCircleMarkMaxInstance.ToString();
        //    textBox_Recipe_GoldPowder_Fiducial_FindCount.Text = m_recipe.nGoldPowderCircleMarkFindCount.ToString();
        //    //textBox_Recipe_GoldPowder_Fiducial_CircleSpec.Text = m_recipe.dGoldPowderCircleMarkSpec.ToString("F3");
        //    //textBox_Recipe_GoldPowder_Fiducial_CircleScore.Text = m_recipe.dGoldPowderCircleMarkScore.ToString("F3");
        //    // 내부 값 (0~1)을 퍼센트 문자열로 표시
        //    textBox_Recipe_GoldPowder_Fiducial_CircleSpec.Text =
        //        (m_recipe.dGoldPowderCircleMarkSpec * 100).ToString("F2");
        //    // 내부 값 (0~1)을 퍼센트 문자열로 표시
        //    textBox_Recipe_GoldPowder_Fiducial_CircleScore.Text =
        //        (m_recipe.dGoldPowderCircleMarkScore * 100).ToString("F2");


        //    // 조명 사용 여부
        //    checkBox_Recipe_GoldPowder_Illuminator_Red.Checked = m_recipe.bGoldPowderIlluminationRedUse;
        //    if (checkBox_Recipe_GoldPowder_Illuminator_Red.Checked)
        //    {
        //        checkBox_Recipe_GoldPowder_Illuminator_Red.Text = "USE";
        //    }
        //    else
        //    {
        //        checkBox_Recipe_GoldPowder_Illuminator_Red.Text = "UnUSE";
        //    }

        //    checkBox_Recipe_GoldPowder_Illuminator_IR.Checked = m_recipe.bGoldPowderIlluminationIRUse;
        //    if (checkBox_Recipe_GoldPowder_Illuminator_IR.Checked)
        //    {
        //        checkBox_Recipe_GoldPowder_Illuminator_IR.Text = "USE";
        //    }
        //    else
        //    {
        //        checkBox_Recipe_GoldPowder_Illuminator_IR.Text = "UnUSE";
        //    }

        //    // 노출 시간
        //    textBox_Recipe_GoldPowder_Camera_ExposureTime.Text = m_recipe.dGoldPowderIlluminationExposureTime.ToString("F1");

        //    // Z축 오프셋
        //    textBox_Recipe_GoldPowder_AxisZ_Setting.Text = m_recipe.dGoldPowderAxisZ_Offset.ToString("F3");

        //    // 조명 세기
        //    Equipment.stVisionRecipeSet.nGoldPowderIlluminationRed = m_recipe.nGoldPowderIlluminationRed;
        //    Equipment.stVisionRecipeSet.nGoldPowderIlluminationIR = m_recipe.nGoldPowderIlluminationIR;
        //    textBox_Recipe_GoldPowder_Illuminator_FineCamRed.Text = m_recipe.nGoldPowderIlluminationRed.ToString();
        //    textBox_Recipe_GoldPowder_Illuminator_FineCamIR.Text = m_recipe.nGoldPowderIlluminationIR.ToString();

        //    // 최초 호출 시 현재 소켓 UI 로드
        //    ApplyGoldPowderSocketPosToUI(m_currentGoldPowderSocketIndex);
        //    ApplyGoldPowderSocketPosToUI_Offset(m_currentGoldPowderSocketIndexOffset);

        //    //textBox_Recipe_GoldPowder_Position_X1.Text = m_recipe.dGoldPowderPos1X.ToString("F3");
        //    //textBox_Recipe_GoldPowder_Position_Y1.Text = m_recipe.dGoldPowderPos1Y.ToString("F3");
        //    //textBox_Recipe_GoldPowder_Position_X2.Text = m_recipe.dGoldPowderPos2X.ToString("F3");
        //    //textBox_Recipe_GoldPowder_Position_Y2.Text = m_recipe.dGoldPowderPos2Y.ToString("F3");
        //    //textBox_Recipe_GoldPowder_Position_X3.Text = m_recipe.dGoldPowderPos3X.ToString("F3");
        //    //textBox_Recipe_GoldPowder_Position_Y3.Text = m_recipe.dGoldPowderPos3Y.ToString("F3");
        //    //textBox_Recipe_GoldPowder_Position_X4.Text = m_recipe.dGoldPowderPos4X.ToString("F3");
        //    //textBox_Recipe_GoldPowder_Position_Y4.Text = m_recipe.dGoldPowderPos4Y.ToString("F3");

        //    //// (수정) 기존 ApplyRecipeToUI 끝부분 GoldPowder 포지션 설정 부분 교체
        //    //// 기존 전역 변수 -> 최초 소켓[0] 동기화 후 UI 반영
        //    //textBox_Recipe_GoldPowder_Position_X1.Text = m_recipe.GoldPowderSocketPosList.Count > 0 ?
        //    //    m_recipe.GoldPowderSocketPosList[0].X[0].ToString("F3") : m_recipe.dGoldPowderPos1X.ToString("F3");
        //    //// 나머지 동일하게 호출 대신 아래 한줄로 대체:
        //    //ApplyGoldPowderSocketPosToUI(m_currentGoldPowderSocketIndex);

        //    //// (수정) UpdateRecipeFromUI 끝부분 전역 포지션 저장 직후 추가
        //    //UpdateGoldPowderSocketPosFromUI(m_currentGoldPowderSocketIndex);


        //    SetScroll();
        //}

        private void UpdateRecipeFromUI()
        {
            if (m_recipe == null)
                return;

            try
            {
                // (변경) UI → 선택된 마크 갱신
                UpdateGoldPowderMarkFromUI(m_currentGoldPowderMarkIndex);

                // 현재 소켓 포지션 구조에 반영
                UpdateGoldPowderSocketPosFromUI(m_currentGoldPowderSocketIndex);
                UpdateGoldPowderSocketPosFromUI_Offset(m_currentGoldPowderSocketIndexOffset);

                // 레거시 필드 (첫 소켓) 동기화
                SyncLegacyGoldPowderPosFromSocket0();
                SyncLegacyGoldPowderPosFromSocket0_Offset();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }
        //private void UpdateRecipeFromUI()
        //{
        //    if (m_recipe == null)
        //        return;

        //    try
        //    {
        //        // 정렬 방식
        //        m_recipe.dGoldPowderAlignType = radioButton_Recipe_GoldPowder_Fiducial_Pattern.Checked ? 0 : 1;

        //        // 마크 타입
        //        m_recipe.dGoldPowderMarkType = radioButton_Recipe_GoldPowder_Fiducial_Type_Circle.Checked ? 0 : 1;

        //        // 마크 색상
        //        m_recipe.bGoldPowderCircleColor = radioButton_Recipe_GoldPowder_Fiducial_White.Checked ? false : true;

        //        // Z 오프셋 및 노출 시간
        //        m_recipe.dGoldPowderAxisZ_Offset = Equipment.ToDouble(textBox_Recipe_GoldPowder_AxisZ_Setting.Text);
        //        m_recipe.dGoldPowderIlluminationExposureTime = Equipment.ToDouble(textBox_Recipe_GoldPowder_Camera_ExposureTime.Text);

        //        // 조명 사용 여부
        //        m_recipe.bGoldPowderIlluminationIRUse = checkBox_Recipe_GoldPowder_Illuminator_IR.Checked;
        //        m_recipe.bGoldPowderIlluminationRedUse = checkBox_Recipe_GoldPowder_Illuminator_Red.Checked;

        //        // 조명 세기
        //        m_recipe.nGoldPowderIlluminationIR = Equipment.ToInt(textBox_Recipe_GoldPowder_Illuminator_FineCamIR.Text);
        //        m_recipe.nGoldPowderIlluminationRed = Equipment.ToInt(textBox_Recipe_GoldPowder_Illuminator_FineCamRed.Text);

        //        // 마크 조건
        //        m_recipe.dGoldPowderCircleMarkRadius = Equipment.ToDouble(textBox_Recipe_GoldPowder_Fiducial_CircleSize.Text);
        //        m_recipe.nGoldPowderCircleMarkMaxInstance = Equipment.ToInt(textBox_Recipe_GoldPowder_Fiducial_MaxInstance.Text);
        //        m_recipe.nGoldPowderCircleMarkFindCount = Equipment.ToInt(textBox_Recipe_GoldPowder_Fiducial_FindCount.Text);
        //        //m_recipe.dGoldPowderCircleMarkSpec = ParseDouble(textBox_Recipe_GoldPowder_Fiducial_CircleSpec.Text);
        //        //m_recipe.dGoldPowderCircleMarkScore = ParseDouble(textBox_Recipe_GoldPowder_Fiducial_CircleScore.Text);
        //        double percentValue = 0.0;
        //        if (double.TryParse(textBox_Recipe_GoldPowder_Fiducial_CircleSpec.Text, out percentValue))
        //        {
        //            // UI에서 입력받은 %를 내부 0~1 값으로 변환
        //            m_recipe.dGoldPowderCircleMarkSpec = percentValue / 100.0;
        //        }
        //        if (double.TryParse(textBox_Recipe_GoldPowder_Fiducial_CircleScore.Text, out percentValue))
        //        {
        //            // UI에서 입력받은 %를 내부 0~1 값으로 변환
        //            m_recipe.dGoldPowderCircleMarkScore = percentValue / 100.0;
        //        }

        //        //m_recipe.dGoldPowderPos1X = Equipment.ToDouble(textBox_Recipe_GoldPowder_Position_X1.Text);
        //        //m_recipe.dGoldPowderPos1Y = Equipment.ToDouble(textBox_Recipe_GoldPowder_Position_Y1.Text);
        //        //m_recipe.dGoldPowderPos2X = Equipment.ToDouble(textBox_Recipe_GoldPowder_Position_X2.Text);
        //        //m_recipe.dGoldPowderPos2Y = Equipment.ToDouble(textBox_Recipe_GoldPowder_Position_Y2.Text);
        //        //m_recipe.dGoldPowderPos3X = Equipment.ToDouble(textBox_Recipe_GoldPowder_Position_X3.Text);
        //        //m_recipe.dGoldPowderPos3Y = Equipment.ToDouble(textBox_Recipe_GoldPowder_Position_Y3.Text);
        //        //m_recipe.dGoldPowderPos4X = Equipment.ToDouble(textBox_Recipe_GoldPowder_Position_X4.Text);
        //        //m_recipe.dGoldPowderPos4Y = Equipment.ToDouble(textBox_Recipe_GoldPowder_Position_Y4.Text);

        //        // 현재 소켓 포지션 구조에 반영
        //        UpdateGoldPowderSocketPosFromUI(m_currentGoldPowderSocketIndex);
        //        UpdateGoldPowderSocketPosFromUI_Offset(m_currentGoldPowderSocketIndexOffset);
        //        // 레거시 필드 (첫 소켓) 동기화
        //        SyncLegacyGoldPowderPosFromSocket0();
        //        SyncLegacyGoldPowderPosFromSocket0_Offset();

        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Write(ex);
        //        //MessageBox.Show("UI 값 중 잘못된 항목이 있습니다.\r\n숫자 형식을 확인하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        private void comboBox_Recipe_Goldpowder_MarkIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_recipe == null) 
                return;

            // 변경사항 먼저 반영
            UpdateGoldPowderMarkFromUI(m_currentGoldPowderMarkIndex);

            // 선택 인덱스 갱신
            m_currentGoldPowderMarkIndex = Math.Max(0, comboBox_Recipe_Goldpowder_MarkIndex.SelectedIndex);

            // 선택된 마크로 UI 새로고침
            ApplyGoldPowderMarkToUI(m_currentGoldPowderMarkIndex);
        }

        private void button_Recipe_Goldpowder_Mark_Add_Click(object sender, EventArgs e)
        {
            if (m_recipe == null) 
                return;

            // 선택 변경 전 현재 UI 값을 커밋 (기존 SelectedIndexChanged의 선반영 동작 유지)
            UpdateGoldPowderMarkFromUI(m_currentGoldPowderMarkIndex);

            // 현재 마크를 복제해서 추가(초기값 유지)
            GoldPowderMarkInfo newMark;
            if (m_recipe.GoldPowderMarkList.Count > 0)
            {
                var curr = m_recipe.GoldPowderMarkList[Math.Max(0, m_currentGoldPowderMarkIndex)];
                newMark = curr.Clone();
            }
            else
            {
                newMark = new GoldPowderMarkInfo();
            }

            m_recipe.GoldPowderMarkList.Add(newMark);

            // 콤보 갱신(이때 SelectedIndexChanged는 일시 분리됨)
            InitGoldPowderMarkCombo();

            // 마지막 항목 선택 후 UI 반영
            comboBox_Recipe_Goldpowder_MarkIndex.SelectedIndex = m_recipe.GoldPowderMarkList.Count - 1;
            ApplyGoldPowderMarkToUI(comboBox_Recipe_Goldpowder_MarkIndex.SelectedIndex);
        }

        private void button_Recipe_Goldpowder_Mark_Delete_Click(object sender, EventArgs e)
        {
            if (m_recipe == null) 
                return;

            if (m_recipe.GoldPowderMarkList.Count <= 1)
            {
                MessageBox.Show("최소 1개 마크는 유지되어야 합니다.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 선택 변경 전 현재 UI 값을 커밋
            UpdateGoldPowderMarkFromUI(m_currentGoldPowderMarkIndex);

            int idx = Math.Max(0, comboBox_Recipe_Goldpowder_MarkIndex.SelectedIndex);
            m_recipe.GoldPowderMarkList.RemoveAt(idx);

            // 인덱스 보정 및 콤보 갱신
            m_currentGoldPowderMarkIndex = Math.Max(0, Math.Min(idx, m_recipe.GoldPowderMarkList.Count - 1));
            InitGoldPowderMarkCombo();
            comboBox_Recipe_Goldpowder_MarkIndex.SelectedIndex = m_currentGoldPowderMarkIndex;

            // 선택 마크로 UI 반영
            ApplyGoldPowderMarkToUI(m_currentGoldPowderMarkIndex);
        }
    }
}
