using QMC.Common;
using QMC.Common.Modules;
using QMC.Common.Parts;
using QMC.Common.Recipe;
using QMC.Common.VisionPart;
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

namespace SLD200.NewStyleForm.NewSubForm
{
    public partial class FormNewSub_Recipe_GoldPowder : UserControl
    {
        private bool m_bFormVisible = false; // 실제 Show 상태 여부
        public bool m_bInitialized = false;

        private System.Windows.Forms.Timer timer_Status;

        static WorkStage workStage;
        static JigAligner Owner;

        private VisionRecipeData m_recipe = null;
        private string m_recipePath = "";

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
            }
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
            ApplyRecipeToUI();
            timer_Status.Start();
        }
        public void OnHide()
        {
            if (!m_bInitialized)
                return;
            UpdateRecipeFromUI();
            timer_Status.Stop();
        }

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

        private void button_Recipe_GoldPowder_Save_Click(object sender, EventArgs e)
        {
            UpdateRecipeFromUI();
            SaveRecipe();

            MessageBox.Show("GoldPowder Recipe 저장 완료", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            }
            else
            {
                mb1.ShowDialog("Information", "저장하였습니다.");
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
                hScrollBar_Recipe_GoldPowder_Illuminator_Red.Value = Equipment.stVisionRecipeSet.nSocketIlluminationRed;
                baseLabel_Recipe_GoldPowder_Min_Red.Text = hScrollBar_Recipe_GoldPowder_Illuminator_Red.Minimum.ToString();
                baseLabel_Recipe_GoldPowder_Max_Red.Text = hScrollBar_Recipe_GoldPowder_Illuminator_Red.Maximum.ToString();

                hScrollBar_Recipe_GoldPowder_Illuminator_IR.Minimum = (int)workStage.Config.ListIlluminationChannel[1].Min;
                hScrollBar_Recipe_GoldPowder_Illuminator_IR.Maximum = (int)workStage.Config.ListIlluminationChannel[1].Max;
                hScrollBar_Recipe_GoldPowder_Illuminator_IR.Value = Equipment.stVisionRecipeSet.nSocketIlluminationIR;
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



        private void ApplyRecipeToUI()
        {
            if (m_recipe == null || 
                m_recipePath == "")
            {
                return;
            }

            radioButton_Recipe_GoldPowder_CameraSelection_HighMag.Checked = true;

            // 정렬 방식
            if (m_recipe.dGoldPowderAlignType == 0)
            {
                radioButton_Recipe_GoldPowder_Fiducial_Pattern.Checked = true;
                radioButton_Recipe_GoldPowder_Fiducial_Circle.Checked = false;
            }
            else
            {
                radioButton_Recipe_GoldPowder_Fiducial_Pattern.Checked = false;
                radioButton_Recipe_GoldPowder_Fiducial_Circle.Checked = true;
            }



            // 마크 타입
            if (m_recipe.dGoldPowderMarkType == 0)
            {
                radioButton_Recipe_GoldPowder_Fiducial_Type_GoldPowder.Checked = true;
                radioButton_Recipe_GoldPowder_Fiducial_Type_Circle.Checked = false;
                
            }
            else
            {
                radioButton_Recipe_GoldPowder_Fiducial_Circle.Checked = true;
                radioButton_Recipe_GoldPowder_Fiducial_Type_GoldPowder.Checked = false;
            }

            // 마크 색상
            if (m_recipe.bGoldPowderCircleColor)
            {
                radioButton_Recipe_GoldPowder_Fiducial_White.Checked = false;
                radioButton_Recipe_GoldPowder_Fiducial_Black.Checked = true;
            }
            else
            {
                radioButton_Recipe_GoldPowder_Fiducial_White.Checked = true;
                radioButton_Recipe_GoldPowder_Fiducial_Black.Checked = false;
            }

            // 마크 스펙
            textBox_Recipe_GoldPowder_Fiducial_CircleSpec.Text = m_recipe.dGoldPowderCircleMarkSpec.ToString("F3");
            textBox_Recipe_GoldPowder_Fiducial_CircleSize.Text = m_recipe.dGoldPowderCircleMarkRadius.ToString("F3");
            textBox_Recipe_GoldPowder_Fiducial_CircleScore.Text = m_recipe.dGoldPowderCircleMarkScore.ToString("F3");
            textBox_Recipe_GoldPowder_Fiducial_MaxInstance.Text = m_recipe.dGoldPowderCircleMarkMaxInstance.ToString("F3");
            textBox_Recipe_GoldPowder_Fiducial_FindCount.Text = m_recipe.dGoldPowderCircleMarkFindCount.ToString("F3");

            // 조명 사용 여부
            checkBox_Recipe_GoldPowder_Illuminator_Red.Checked = m_recipe.bGoldPowderIlluminationRedUse;
            if (checkBox_Recipe_GoldPowder_Illuminator_Red.Checked)
            {
                checkBox_Recipe_GoldPowder_Illuminator_Red.Text = "USE";
            }
            else
            {
                checkBox_Recipe_GoldPowder_Illuminator_Red.Text = "UnUSE";
            }
            
            checkBox_Recipe_GoldPowder_Illuminator_IR.Checked = m_recipe.bGoldPowderIlluminationIRUse;
            if (checkBox_Recipe_GoldPowder_Illuminator_IR.Checked)
            {
                checkBox_Recipe_GoldPowder_Illuminator_IR.Text = "USE";
            }
            else
            {
                checkBox_Recipe_GoldPowder_Illuminator_IR.Text = "UnUSE";
            }

            // 노출 시간
            textBox_Recipe_GoldPowder_Camera_ExposureTime.Text = m_recipe.dGoldPowderIlluminationExposureTime.ToString("F1");

            // Z축 오프셋
            textBox_Recipe_GoldPowder_AxisZ_Setting.Text = m_recipe.dGoldPowderAxisZ_Offset.ToString("F3");

            // 조명 세기
            textBox_Recipe_GoldPowder_Illuminator_FineCamIR.Text = m_recipe.nGoldPowderIlluminationIR.ToString();
            textBox_Recipe_GoldPowder_Illuminator_FineCamRed.Text = m_recipe.nGoldPowderIlluminationRed.ToString();

            SetScroll();
        }

        private void UpdateRecipeFromUI()
        {
            if (m_recipe == null)
                return;

            try
            {
                // 정렬 방식
                m_recipe.dGoldPowderAlignType = radioButton_Recipe_GoldPowder_Fiducial_Pattern.Checked ? 0 : 1;

                // 마크 타입
                m_recipe.dGoldPowderMarkType = radioButton_Recipe_GoldPowder_Fiducial_Type_Circle.Checked ? 0 : 1;

                // 마크 색상
                m_recipe.bGoldPowderCircleColor = radioButton_Recipe_GoldPowder_Fiducial_White.Checked ? false : true;

                // Z 오프셋 및 노출 시간
                m_recipe.dGoldPowderAxisZ_Offset = ParseDouble(textBox_Recipe_GoldPowder_AxisZ_Setting.Text);
                m_recipe.dGoldPowderIlluminationExposureTime = ParseDouble(textBox_Recipe_GoldPowder_Camera_ExposureTime.Text);

                // 조명 사용 여부
                m_recipe.bGoldPowderIlluminationIRUse = checkBox_Recipe_GoldPowder_Illuminator_IR.Checked;
                m_recipe.bGoldPowderIlluminationRedUse = checkBox_Recipe_GoldPowder_Illuminator_Red.Checked;

                // 조명 세기
                m_recipe.nGoldPowderIlluminationIR = Equipment.ToInt(textBox_Recipe_GoldPowder_Illuminator_FineCamIR.Text);
                m_recipe.nGoldPowderIlluminationRed = Equipment.ToInt(textBox_Recipe_GoldPowder_Illuminator_FineCamRed.Text);

                // 마크 조건
                m_recipe.dGoldPowderCircleMarkRadius = ParseDouble(textBox_Recipe_GoldPowder_Fiducial_CircleSize.Text);
                m_recipe.dGoldPowderCircleMarkSpec = ParseDouble(textBox_Recipe_GoldPowder_Fiducial_CircleSpec.Text);
                m_recipe.dGoldPowderCircleMarkScore = ParseDouble(textBox_Recipe_GoldPowder_Fiducial_CircleScore.Text);
                m_recipe.dGoldPowderCircleMarkMaxInstance = Equipment.ToInt(textBox_Recipe_GoldPowder_Fiducial_MaxInstance.Text);
                m_recipe.dGoldPowderCircleMarkFindCount = Equipment.ToInt(textBox_Recipe_GoldPowder_Fiducial_FindCount.Text);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                //MessageBox.Show("UI 값 중 잘못된 항목이 있습니다.\r\n숫자 형식을 확인하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            double dSpec = 0.0;
            double dScore = 0.0;
            int nMaxInstance = 0;
            int nFindCount = 0;

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


            dSpec = Equipment.ToDouble(textBox_Recipe_GoldPowder_Fiducial_CircleSpec.Text); //  Fiducial 마크 Spec
            dTargetSize_Radius = Equipment.ToDouble(textBox_Recipe_GoldPowder_Fiducial_CircleSize.Text); //  Fiducial 마크 크기
            dScore = Equipment.ToDouble(textBox_Recipe_GoldPowder_Fiducial_CircleScore.Text); //  Fiducial 마크 Score
            nMaxInstance = Equipment.ToInt(textBox_Recipe_GoldPowder_Fiducial_MaxInstance.Text); //  Fiducial 마크 최대 개수
            nFindCount = Equipment.ToInt(textBox_Recipe_GoldPowder_Fiducial_FindCount.Text); //  Fiducial 마크 찾기 개수

            if (radioButton_Recipe_GoldPowder_Fiducial_White.Checked)
            {
                nTargetColor = 1;          //  Fiducial 마크 색깔 //  0: Black, 1: White
            }
            else if (radioButton_Recipe_GoldPowder_Fiducial_Black.Checked)
            {
                nTargetColor = 0;          //  Fiducial 마크 색깔 //  0: Black, 1: White
            }

            QMC_ImageProcessFindAlign aligner = new QMC_ImageProcessFindAlign();
            List<RectangleF> circlesResult = new List<RectangleF>();
            {
                int w = workStage.Camera_HighRes.Resolution.Width;
                int h = workStage.Camera_HighRes.Resolution.Height;
                nImage_Width = w;
                nImage_Height = h;
                double m_dradius = 0.0;
                m_dradius = dTargetSize_Radius / workStage.Config.ParamConfig.UpperVision_Scale_X;

                //dScore
                QMC_ImageProcessFindAlignResult result = aligner.FindMetalPowderForAutoTreshold(circlesResult,
                                                    workStage.Camera_HighRes.LatestImage.RawData,
                                                    w, h, (int)m_dradius, dScore, dSpec);
                if(circlesResult.Count > 3 )
                {
                    bFindCircle = true;
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
                MessageBox.Show("원 찾기 실패", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                listBox_Recipe_GoldPowder_Fiducial_Result.Items.Clear();
            }

            //  원 찾기 후 다시 Live
            if (workStage.Camera_HighRes.Opened)
            {
                workStage.Camera_HighRes.StartLive();
            }
        }
    }
}
