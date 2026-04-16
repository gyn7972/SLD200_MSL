using QMC.Common.Modules;
using QMC.Common;
using QMC.Common.Q_Config;
using QMC.Common.Vision.Tools;
using QMC.Common.VisionPart;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QMC.Common.Parts;
using QMC.Common.Vision;
using SLD200_MSL;
using QMC.Common.UI;
using static OpenCvSharp.LineIterator;

namespace SLD200.NewStyleForm
{
    public partial class FormNew_VerifyScannerCameraOffset : Form
    {
        private bool m_bFormVisible = false; // 실제 Show 상태 여부
        private bool m_bInitialized = false;

        static WorkStage workStage;

        private ScannerCalConfigData _scannerCalConfig;
        public PatternMatchingParameters PatternMatchingParameter = new PatternMatchingParameters();
        public BlobVisionToolParameter BlobParameter = new BlobVisionToolParameter();
        public RoiVisionTool RoiTrain;
        public RoiVisionTool RoiInspect;
        private SLD200_MSL.RoiListControl m_RoiListControl;
        public bool IsPixel = true;


        public FormNew_VerifyScannerCameraOffset()
        {
            InitializeComponent();

            this.AutoScaleMode = AutoScaleMode.None;

            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;
            foreach (Module module in m_collectionModules)
            {
                if (module.Name == "WorkStage")
                {
                    workStage = module as WorkStage;
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

        private void FormNew_VerifyScannerCameraOffset_Load(object sender, EventArgs e)
        {
            this.AutoScaleMode = AutoScaleMode.None;

            _scannerCalConfig = ScannerCalConfigData.LoadFromIni();
            ApplyConfigToUI(); // 다음 단계에서 이 함수 구현 예정

            // Vision Offset 표시
            if (label_VerifyScannerCameraOffset_OffsetX.Text != Equipment.stOffsetDistance.FromScannerToFineCam.X.ToString())
            {
                label_VerifyScannerCameraOffset_OffsetX.Text = Equipment.stOffsetDistance.FromScannerToFineCam.X.ToString();
            }
            if (label_VerifyScannerCameraOffset_OffsetY.Text != Equipment.stOffsetDistance.FromScannerToFineCam.Y.ToString())
            {
                label_VerifyScannerCameraOffset_OffsetY.Text = Equipment.stOffsetDistance.FromScannerToFineCam.Y.ToString();
            }

            // ROI 설정
            this.RoiTrain = workStage.scannerCompensator.GetTrainRoi();
            this.RoiInspect = workStage.scannerCompensator.GetInspectRoi();

            ScannerCompensatorRecipe recipe = workStage.scannerCompensator.Recipe;

            Point roi = new Point(0, 0);
            roi.X = (int)_scannerCalConfig.Scanner_Calibration_TrainRoiStartLocation_X;
            roi.Y = (int)_scannerCalConfig.Scanner_Calibration_TrainRoiStartLocation_Y;
            recipe.TrainRoiStartLocation = roi;
            RoiTrain.Parameter.StartLocation = roi;
            roi.X = (int)_scannerCalConfig.Scanner_Calibration_TrainRoiEndLocation_X;
            roi.Y = (int)_scannerCalConfig.Scanner_Calibration_TrainRoiEndLocation_Y;
            recipe.TrainRoiEndLocation = roi;
            RoiTrain.Parameter.EndLocation = roi;
            roi.X = (int)_scannerCalConfig.Scanner_Calibration_InspectionRoiStartLocation_X;
            roi.Y = (int)_scannerCalConfig.Scanner_Calibration_InspectionRoiStartLocation_Y;
            recipe.InspectRoiStartLocation = roi;
            RoiInspect.Parameter.StartLocation = roi;
            roi.X = (int)_scannerCalConfig.Scanner_Calibration_InspectionRoiEndLocation_X;
            roi.Y = (int)_scannerCalConfig.Scanner_Calibration_InspectionRoiEndLocation_Y;
            recipe.InspectRoiEndLocation = roi;
            RoiInspect.Parameter.EndLocation = roi;

            // 트레인 이미지
            if (recipe != null)
            {
                VisionImage visionImage = recipe.PatternMatchingParameter.TrainImage;
                pictureBox_VerifyScannerCameraOffset_TrainImage.Image = visionImage?.GetImage();
            }
            else
            {
                pictureBox_VerifyScannerCameraOffset_TrainImage.Image = null;
            }

            PatternMatchingParameter = new PatternMatchingParameters();
            if (recipe != null)
            {
                this.SetPatternMatchingData(recipe.PatternMatchingParameter);
                BlobParameter = recipe.BlobParameter;
            }

            InitPatternMatchingParameter();
            InitBlobParameter();

            radioButton_VerifyScannerCameraOffset_Pattern.Checked = _scannerCalConfig.Scanner_Calibration_UsePatternMatching;
            radioButton_VerifyScannerCameraOffset_Blob.Checked = _scannerCalConfig.Scanner_Calibration_UseBlobVisionTool;
            radioButton_VerifyScannerCameraOffset_Cross.Checked = _scannerCalConfig.Scanner_Calibration_MarkType_Cross;
            radioButton_VerifyScannerCameraOffset_Circle.Checked = _scannerCalConfig.Scanner_Calibration_MarkType_Circlle;


            this.hScrollBar_VerifyScannerCameraOffset_Illuminator.ValueChanged += new System.EventHandler(this.hScrollBarIlluminator_ValueChanged);
            SetScroll(0);

            radioButton_VerifyScannerCameraOffset_Light_IR.Checked = true;
            radioButton_VerifyScannerCameraOffset_Light_Red.Checked = false;
            workStage.Config.ListIlluminationChannel[0].Value = _scannerCalConfig.Scanner_Calibration_Illumination_Red_Value; // RED
            workStage.Config.ListIlluminationChannel[1].Value = _scannerCalConfig.Scanner_Calibration_Illumination_IR_Value; // IR

            checkBox_VerifyScannerCameraOffset_Position.Checked = true;

            Box_VerifyScannerCameraOffset_ImageViewer.SizeMode = PictureBoxSizeMode.CenterImage;
            Box_VerifyScannerCameraOffset_ImageViewer.SuspendDisplay();
            Box_VerifyScannerCameraOffset_ImageViewer.Camera = workStage.scannerCompensator.Camera;

            m_RoiListControl = new SLD200_MSL.RoiListControl(RoiTrain, RoiInspect, workStage.scannerCompensator.Camera.Resolution);
            m_RoiListControl.roiTrainButtonClick += RoiTrainButtonClick;
            m_RoiListControl.roiAlignButtonClick += RoiInspectButtonClick;
            m_RoiListControl.roiTrainSaveButtonClick += RoiTrainSaveButtonClick;
            m_RoiListControl.roiAlignSaveButtonClick += RoiInspectSaveButtonClick;

            this.Refresh();
        }

        private void ApplyConfigToUI()
        {
            try
            {
                // Illuminator
                textBox_VerifyScannerCameraOffset_Illuminator_FineCamRed.Text =
                    _scannerCalConfig.Scanner_Calibration_Illumination_Red_Value.ToString();
                textBox_VerifyScannerCameraOffset_Illuminator_FineCamIR.Text =
                    _scannerCalConfig.Scanner_Calibration_Illumination_IR_Value.ToString();
                textBox_VerifyScannerCameraOffset_Illuminator_Camera_ExposureTime_High.Text =
                    _scannerCalConfig.Scanner_Calibration_ExposureTime_High.ToString();

                // Laser 설정
                textBox_VerifyScannerCameraOffset_LaserFrequency.Text =
                    _scannerCalConfig.Scanner_Calibration_LaserFrequency.ToString("F2");
                textBox_VerifyScannerCameraOffset_PulseWidth.Text =
                    _scannerCalConfig.Scanner_Calibration_LaserPulseWidth.ToString("F2");
                textBox_VerifyScannerCameraOffset_LaserEnergy.Text =
                    _scannerCalConfig.Scanner_Calibration_LaserEnergy.ToString("F2");
                textBox_VerifyScannerCameraOffset_CrossMarkLength.Text =
                    _scannerCalConfig.Scanner_Calibration_CrossMarkLength.ToString("F2");
                textBox_VerifyScannerCameraOffset_MarkingSpeed.Text =
                    _scannerCalConfig.Scanner_Calibration_LaserMarkSpeed.ToString("F2");
                textBox_VerifyScannerCameraOffset_JumpSpeed.Text =
                    _scannerCalConfig.Scanner_Calibration_LaserJumpSpeed.ToString("F2");
                textBox_VerifyScannerCameraOffset_LaserOnDelay.Text =
                    _scannerCalConfig.Scanner_Calibration_LaserOnDelay.ToString("F0");
                textBox_VerifyScannerCameraOffset_LaserOffDelay.Text =
                    _scannerCalConfig.Scanner_Calibration_LaserOffDelay.ToString("F0");
                textBox_VerifyScannerCameraOffset_MarkDelay.Text =
                    _scannerCalConfig.Scanner_Calibration_MarkDelay.ToString("F0");
                textBox_VerifyScannerCameraOffset_JumpDelay.Text =
                    _scannerCalConfig.Scanner_Calibration_JumpDelay.ToString("F0");
                textBox_VerifyScannerCameraOffset_PolygonDelay.Text =
                    _scannerCalConfig.Scanner_Calibration_PolygonDelay.ToString("F0");

                // Calibration Area
                textBox_VerifyScannerCameraOffset_CalAreaWidth.Text =
                    _scannerCalConfig.Scanner_Calibration_CalAreaWidth.ToString("F2");
                textBox_VerifyScannerCameraOffset_CalAreaHeight.Text =
                    _scannerCalConfig.Scanner_Calibration_CalAreaHeight.ToString("F2");
                textBox_VerifyScannerCameraOffset_CalPitch.Text =
                    _scannerCalConfig.Scanner_Calibration_CalPitch.ToString("F2");

                // Z 오프셋
                textBox_VerifyScannerCameraOffset_VisionZOffset.Text =
                    _scannerCalConfig.Scanner_Calibration_VisionZOffset.ToString("F3");

                // 마지막 위치
                label_VerifyScannerCameraOffset_LastPosX_Disp.Text =
                    _scannerCalConfig.Scanner_VerifyCameraOffset_PosX_Last.ToString("F3");
                label_VerifyScannerCameraOffset_LastPosY_Disp.Text =
                    _scannerCalConfig.Scanner_VerifyCameraOffset_PosY_Last.ToString("F3");

                // 마크 타입
                radioButton_VerifyScannerCameraOffset_Cross.Checked =
                    _scannerCalConfig.Scanner_Calibration_MarkType_Cross;
                radioButton_VerifyScannerCameraOffset_Circle.Checked =
                    _scannerCalConfig.Scanner_Calibration_MarkType_Circlle;

                // 매칭 타입
                radioButton_VerifyScannerCameraOffset_Pattern.Checked =
                    _scannerCalConfig.Scanner_Calibration_UsePatternMatching;
                radioButton_VerifyScannerCameraOffset_Blob.Checked =
                    _scannerCalConfig.Scanner_Calibration_UseBlobVisionTool;

                // 상태 및 기타 설정
                checkBox_VerifyScannerCameraOffset_Position.Checked =
                    _scannerCalConfig.Scanner_Calibration_Position_Enable;

                // ※ 나머지 값 (예: Change, Convert, Offset_X/Y, 경로, 필드사이즈 등)은 별도 TextBox/Control이 없으면 UI에 표시할 필요 없음
                // 하지만 필요하다면 추가적으로 label 또는 textbox 만들어 표시 가능

            }
            catch (Exception ex)
            {
                MessageBox.Show("설정값을 UI에 적용하는 도중 오류가 발생했습니다.\n" + ex.Message, "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ApplyUIToConfig()
        {
            try
            {
                // Illuminator
                _scannerCalConfig.Scanner_Calibration_Illumination_Red_Value = int.Parse(textBox_VerifyScannerCameraOffset_Illuminator_FineCamRed.Text);
                _scannerCalConfig.Scanner_Calibration_Illumination_IR_Value = int.Parse(textBox_VerifyScannerCameraOffset_Illuminator_FineCamIR.Text);
                _scannerCalConfig.Scanner_Calibration_ExposureTime_High = int.Parse(textBox_VerifyScannerCameraOffset_Illuminator_Camera_ExposureTime_High.Text);

                // Laser
                _scannerCalConfig.Scanner_Calibration_LaserFrequency = double.Parse(textBox_VerifyScannerCameraOffset_LaserFrequency.Text);
                _scannerCalConfig.Scanner_Calibration_LaserPulseWidth = double.Parse(textBox_VerifyScannerCameraOffset_PulseWidth.Text);
                _scannerCalConfig.Scanner_Calibration_LaserEnergy = double.Parse(textBox_VerifyScannerCameraOffset_LaserEnergy.Text);
                _scannerCalConfig.Scanner_Calibration_CrossMarkLength = double.Parse(textBox_VerifyScannerCameraOffset_CrossMarkLength.Text);
                _scannerCalConfig.Scanner_Calibration_LaserMarkSpeed = double.Parse(textBox_VerifyScannerCameraOffset_MarkingSpeed.Text);
                _scannerCalConfig.Scanner_Calibration_LaserJumpSpeed = double.Parse(textBox_VerifyScannerCameraOffset_JumpSpeed.Text);
                _scannerCalConfig.Scanner_Calibration_LaserOnDelay = double.Parse(textBox_VerifyScannerCameraOffset_LaserOnDelay.Text);
                _scannerCalConfig.Scanner_Calibration_LaserOffDelay = double.Parse(textBox_VerifyScannerCameraOffset_LaserOffDelay.Text);
                _scannerCalConfig.Scanner_Calibration_MarkDelay= double.Parse(textBox_VerifyScannerCameraOffset_MarkDelay.Text);
                _scannerCalConfig.Scanner_Calibration_JumpDelay= double.Parse(textBox_VerifyScannerCameraOffset_JumpDelay.Text);
                _scannerCalConfig.Scanner_Calibration_PolygonDelay = double.Parse(textBox_VerifyScannerCameraOffset_PolygonDelay.Text);

                // Calibration Area
                _scannerCalConfig.Scanner_Calibration_CalAreaWidth = double.Parse(textBox_VerifyScannerCameraOffset_CalAreaWidth.Text);
                _scannerCalConfig.Scanner_Calibration_CalAreaHeight = double.Parse(textBox_VerifyScannerCameraOffset_CalAreaHeight.Text);
                _scannerCalConfig.Scanner_Calibration_CalPitch = double.Parse(textBox_VerifyScannerCameraOffset_CalPitch.Text);

                // Z Offset
                _scannerCalConfig.Scanner_Calibration_VisionZOffset = double.Parse(textBox_VerifyScannerCameraOffset_VisionZOffset.Text);

                // Mark type
                _scannerCalConfig.Scanner_Calibration_MarkType_Cross = radioButton_VerifyScannerCameraOffset_Cross.Checked;
                _scannerCalConfig.Scanner_Calibration_MarkType_Circlle = radioButton_VerifyScannerCameraOffset_Circle.Checked;

                // Matching type
                _scannerCalConfig.Scanner_Calibration_UsePatternMatching = radioButton_VerifyScannerCameraOffset_Pattern.Checked;
                _scannerCalConfig.Scanner_Calibration_UseBlobVisionTool = radioButton_VerifyScannerCameraOffset_Blob.Checked;

                // Position, Status, Offset, Path
                _scannerCalConfig.Scanner_Calibration_Position_Enable = checkBox_VerifyScannerCameraOffset_Position.Checked;
                _scannerCalConfig.Scanner_Calibration_Change = true;
                _scannerCalConfig.Scanner_Calibration_Convert = 1;
                _scannerCalConfig.Scanner_Vision_Offset_Setting_Use = true;
                _scannerCalConfig.Scanner_Vision_Offset_Setting_X = 0.0;
                _scannerCalConfig.Scanner_Vision_Offset_Setting_Y = 0.0;
                _scannerCalConfig.Scanner_Calibration_srcFilePath = string.Empty;
                _scannerCalConfig.Scanner_Calibration_targetFilePath = string.Empty;
                _scannerCalConfig.Scanner_Calibration_FieldSize = 0.0;
                _scannerCalConfig.Scanner_Calibration_rowInterval = 1;
                _scannerCalConfig.Scanner_Calibration_colInterval = 1;
                _scannerCalConfig.Scanner_Calibration_rowCount = 1;
                _scannerCalConfig.Scanner_Calibration_colCount = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("UI 데이터를 설정값에 적용하는 중 오류가 발생했습니다.\n" + ex.Message, "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SetPatternMatchingData(PatternMatchingParameters parameter)
        {
            PatternMatchingParameter = parameter;
            InitPatternMatchingParameter();
        }

        private void InitPatternMatchingParameter()
        {
            if (PatternMatchingParameter != null)
            {
                basetextBox_VerifyScannerCameraOffset_AngleTolerance.Text = _scannerCalConfig.Scanner_Calibration_PatternMatchingParameters.MaxTolerance.ToString();
                PatternMatchingParameter.MaxTolerance = _scannerCalConfig.Scanner_Calibration_PatternMatchingParameters.MaxTolerance;

                basetextBox_VerifyScannerCameraOffset_MaxInstance.Text = _scannerCalConfig.Scanner_Calibration_PatternMatchingParameters.MaxInstance.ToString();
                PatternMatchingParameter.MaxInstance = _scannerCalConfig.Scanner_Calibration_PatternMatchingParameters.MaxInstance;

                basetextBox_VerifyScannerCameraOffset_MinScore.Text = _scannerCalConfig.Scanner_Calibration_PatternMatchingParameters.MinScore.ToString();
                PatternMatchingParameter.MinScore = _scannerCalConfig.Scanner_Calibration_PatternMatchingParameters.MinScore;

                bool bOn = _scannerCalConfig.Scanner_Calibration_PatternMatchingParameters.DuplicateChecked;
                baseToggleButton_VerifyScannerCameraOffset_DuplicateCheck.UpdateToggleStatus(bOn);
                PatternMatchingParameter.DuplicateChecked = bOn;

                bOn = _scannerCalConfig.Scanner_Calibration_PatternMatchingParameters.UseMaskImage;
                baseToggleButton_VerifyScannerCameraOffset_UseMaskImage.UpdateToggleStatus(bOn);
                PatternMatchingParameter.UseMaskImage = bOn;
            }
        }
        private void InitBlobParameter()
        {
            if (BlobParameter != null)
            {
                BlobParameter.HardThreshold = _scannerCalConfig.Scanner_Calibration_BlobVisionToolParameter.HardThreshold;
                BlobParameter.MinPixels = _scannerCalConfig.Scanner_Calibration_BlobVisionToolParameter.MinPixels;
                BlobParameter.RepeatCount = _scannerCalConfig.Scanner_Calibration_BlobVisionToolParameter.RepeatCount;
                BlobParameter.Polarity = _scannerCalConfig.Scanner_Calibration_BlobVisionToolParameter.Polarity;
                BlobParameter.HasChanged = _scannerCalConfig.Scanner_Calibration_BlobVisionToolParameter.HasChanged;
            }
        }
        private void RoiTrainButtonClick(RoiVisionTool roiVisionTool)
        {
            if (workStage.scannerCompensator != null)
            {
                RoiTrain.Parameter.CenterLocation = roiVisionTool.Parameter.CenterLocation;
                RoiTrain.Parameter.Size = roiVisionTool.Parameter.Size;

                Box_VerifyScannerCameraOffset_ImageViewer.NormalOverlays.Add(RoiTrain.Parameter.Overlay);
                RoiTrain.Parameter.Overlay.Visible = true;
                Box_VerifyScannerCameraOffset_ImageViewer.Display();
            }
        }

        public void RoiInspectButtonClick(RoiVisionTool roiVisionTool)
        {
            if (workStage.scannerCompensator != null)
            {
                RoiInspect.Parameter.CenterLocation = roiVisionTool.Parameter.CenterLocation;
                RoiInspect.Parameter.Size = roiVisionTool.Parameter.Size;

                Box_VerifyScannerCameraOffset_ImageViewer.NormalOverlays.Add(RoiInspect.Parameter.Overlay);
                RoiInspect.Parameter.Overlay.Visible = true;
                Box_VerifyScannerCameraOffset_ImageViewer.Display();
            }
        }
        private void RoiTrainSaveButtonClick(RoiVisionTool roiVisionTool, bool bOk)
        {
            ScannerCompensatorRecipe recipe = workStage.scannerCompensator.Recipe;

            if (!bOk)
            {
                RoiTrain.Parameter.StartLocation = recipe.TrainRoiStartLocation;
                RoiTrain.Parameter.EndLocation = recipe.TrainRoiEndLocation;
                RoiTrain.Parameter.Overlay.Visible = false;
                Box_VerifyScannerCameraOffset_ImageViewer.Display();
                return;
            }

            RoiTrain = roiVisionTool;
            RoiTrain.Parameter.Overlay.Visible = false;

            recipe.TrainRoiStartLocation = RoiTrain.Parameter.StartLocation;
            recipe.TrainRoiEndLocation = RoiTrain.Parameter.EndLocation;

            SetTrainImage(workStage.scannerCompensator.Recipe.PatternMatchingParameter.TrainImage);

            Box_VerifyScannerCameraOffset_ImageViewer.Display();

            _scannerCalConfig.Scanner_Calibration_TrainRoiStartLocation_X = RoiTrain.Parameter.StartLocation.X;
            _scannerCalConfig.Scanner_Calibration_TrainRoiStartLocation_Y = RoiTrain.Parameter.StartLocation.Y;
            _scannerCalConfig.Scanner_Calibration_TrainRoiEndLocation_X = RoiTrain.Parameter.EndLocation.X;
            _scannerCalConfig.Scanner_Calibration_TrainRoiEndLocation_Y = RoiTrain.Parameter.EndLocation.Y;

            _scannerCalConfig.SaveToIni();
        }

        private void RoiInspectSaveButtonClick(RoiVisionTool roiVisionTool, bool bOk)
        {
            ScannerCompensatorRecipe recipe = workStage.scannerCompensator.Recipe;
            if (!bOk)
            {
                RoiInspect.Parameter.StartLocation = recipe.InspectRoiStartLocation;
                RoiInspect.Parameter.EndLocation = recipe.InspectRoiEndLocation;
                Box_VerifyScannerCameraOffset_ImageViewer.Display();
                return;
            }
            RoiInspect = roiVisionTool;
            recipe.InspectRoiStartLocation = RoiInspect.Parameter.StartLocation;
            recipe.InspectRoiEndLocation = RoiInspect.Parameter.EndLocation;

            Box_VerifyScannerCameraOffset_ImageViewer.Display();

            _scannerCalConfig.Scanner_Calibration_InspectionRoiStartLocation_X = RoiInspect.Parameter.StartLocation.X;
            _scannerCalConfig.Scanner_Calibration_InspectionRoiStartLocation_Y = RoiInspect.Parameter.StartLocation.Y;
            _scannerCalConfig.Scanner_Calibration_InspectionRoiEndLocation_X = RoiInspect.Parameter.EndLocation.X;
            _scannerCalConfig.Scanner_Calibration_InspectionRoiEndLocation_Y = RoiInspect.Parameter.EndLocation.Y;

            _scannerCalConfig.SaveToIni();
        }
        public void SetTrainImage(VisionImage image)
        {
            this.pictureBox_VerifyScannerCameraOffset_TrainImage.Image = image.GetImage();
            this.pictureBox_VerifyScannerCameraOffset_TrainImage.BringToFront();
        }

        public void UpdataPositionData(double x, double y, double t, double circlex = 0.0, double circley = 0.0)
        {
            baseTextBox_VerifyScannerCameraOffset_PositionX.Text = x.ToString();
            baseTextBox_VerifyScannerCameraOffset_PositionY.Text = y.ToString();
            baseTextBox_VerifyScannerCameraOffset_PositionT.Text = t.ToString();

            //if (checkBox_VerifyScannerCameraOffset_CirclePos.Checked)
            //{
            //    label_VerifyScannerCameraOffset_CirclePosX.Text = circlex.ToString();
            //    label_VerifyScannerCameraOffset_CirclePosY.Text = circley.ToString();
            //}
            //else
            //{
            //    label_VerifyScannerCameraOffset_CirclePosX.Text = "---";
            //    label_VerifyScannerCameraOffset_CirclePosY.Text = "---";
            //}

            baseTextBox_VerifyScannerCameraOffset_PositionX.Refresh();
            baseTextBox_VerifyScannerCameraOffset_PositionY.Refresh();
            baseTextBox_VerifyScannerCameraOffset_PositionT.Refresh();
            //label_Setup_VerifyScannerCameraOffset_CirclePosX.Refresh();
            //label_Setup_VerifyScannerCameraOffset_CirclePosY.Refresh();

        }


        private void SetScroll(int nChannel)
        {
            hScrollBar_VerifyScannerCameraOffset_Illuminator.Minimum = (int)workStage.Config.ListIlluminationChannel[nChannel].Min;
            hScrollBar_VerifyScannerCameraOffset_Illuminator.Maximum = (int)workStage.Config.ListIlluminationChannel[nChannel].Max;

            baseLabel_VerifyScannerCameraOffset_Min.Text = hScrollBar_VerifyScannerCameraOffset_Illuminator.Minimum.ToString();
            baseLabel_VerifyScannerCameraOffset_Max.Text = hScrollBar_VerifyScannerCameraOffset_Illuminator.Maximum.ToString();
        }


        private void button_VerifyScannerCameraOffset_Vision_Save_Click(object sender, EventArgs e)
        {
            try
            {
                ApplyUIToConfig();  // UI -> 객체 저장
                ApplyAdvancedVisionUIToConfig();
                if (_scannerCalConfig.SaveToIni())
                {
                    MessageBox.Show("설정이 성공적으로 저장되었습니다.", "저장 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("설정 저장에 실패했습니다.", "저장 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("저장 중 오류 발생:\n" + ex.Message, "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }








        private void checkBox_VerifyScannerCameraOffset_Position_CheckedChanged(object sender, EventArgs e)
        {
            // 위치 기준 변경 여부 저장
            _scannerCalConfig.Scanner_Calibration_Position_Enable = false;  //checkBox_VerifyScannerCameraOffset_Position.Checked;
            if (checkBox_VerifyScannerCameraOffset_Position.Checked)
            {
                _scannerCalConfig.Scanner_Calibration_Position_Enable = true;

                checkBox_VerifyScannerCameraOffset_Position.Text = "Cal Pan";
                checkBox_VerifyScannerCameraOffset_Position.ForeColor = Color.BlueViolet;
            }
            else
            {
                _scannerCalConfig.Scanner_Calibration_Position_Enable = false;

                checkBox_VerifyScannerCameraOffset_Position.Text = "Stage Center";
                checkBox_VerifyScannerCameraOffset_Position.ForeColor = Color.BlueViolet;
            }

        }


        private void button_VerifyScannerCameraOffset_Start_Click(object sender, EventArgs e)
        {
            // 시작 동작 정의
            MessageBox.Show("검증 시작 기능이 구현되지 않았습니다.", "안내", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_VerifyScannerCameraOffset_Stop_Click(object sender, EventArgs e)
        {
            // 중지 동작 정의
            MessageBox.Show("검증 정지 기능이 구현되지 않았습니다.", "안내", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_VerifyScannerCameraOffset_Apply_Click(object sender, EventArgs e)
        {
            ApplyUIToConfig();
            ApplyAdvancedVisionUIToConfig();
            MessageBox.Show("현재 UI 설정이 내부 설정에 적용되었습니다.", "적용 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_VerifyScannerCameraOffset_CameraLive_Click(object sender, EventArgs e)
        {
            //  카메라 연결 확인
            if (!workStage.scannerCompensator.Camera.Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 카메라를 연결해야 해야 합니다.");
                return;
            }
            Box_VerifyScannerCameraOffset_ImageViewer.Simulated = false;
            workStage.scannerCompensator.Simulated = false;

            if (workStage.scannerCompensator.Camera != null)
            {
                workStage.scannerCompensator.Camera.StartLive();
            }
        }

        private void button_VerifyScannerCameraOffset_CameraStop_Click(object sender, EventArgs e)
        {
            if (!workStage.scannerCompensator.Camera.Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 카메라를 연결해야 해야 합니다.");
                return;
            }

            if (workStage.scannerCompensator.Camera != null)
            {
                workStage.scannerCompensator.Camera.StopLive();
            }
        }

        private void radioButton_VerifyScannerCameraOffset_Cross_CheckedChanged(object sender, EventArgs e)
        {
            _scannerCalConfig.Scanner_Calibration_MarkType_Cross = radioButton_VerifyScannerCameraOffset_Cross.Checked;
            _scannerCalConfig.Scanner_Calibration_MarkType_Circlle = !radioButton_VerifyScannerCameraOffset_Cross.Checked;
        }

        private void radioButton_VerifyScannerCameraOffset_Circle_CheckedChanged(object sender, EventArgs e)
        {
            _scannerCalConfig.Scanner_Calibration_MarkType_Circlle = radioButton_VerifyScannerCameraOffset_Circle.Checked;
            _scannerCalConfig.Scanner_Calibration_MarkType_Cross = !radioButton_VerifyScannerCameraOffset_Circle.Checked;
        }

        private void radioButton_VerifyScannerCameraOffset_Pattern_CheckedChanged(object sender, EventArgs e)
        {
            _scannerCalConfig.Scanner_Calibration_UsePatternMatching = radioButton_VerifyScannerCameraOffset_Pattern.Checked;
            _scannerCalConfig.Scanner_Calibration_UseBlobVisionTool = !radioButton_VerifyScannerCameraOffset_Pattern.Checked;
        }

        private void radioButton_VerifyScannerCameraOffset_Blob_CheckedChanged(object sender, EventArgs e)
        {
            _scannerCalConfig.Scanner_Calibration_UseBlobVisionTool = radioButton_VerifyScannerCameraOffset_Blob.Checked;
            _scannerCalConfig.Scanner_Calibration_UsePatternMatching = !radioButton_VerifyScannerCameraOffset_Blob.Checked;
        }

        private void button_VerifyScannerCameraOffset_Train_Click(object sender, EventArgs e)
        {
            ScannerCompensatorRecipe recipe = workStage.scannerCompensator.Recipe;
            RoiTrain.Parameter.StartLocation = recipe.TrainRoiStartLocation;
            RoiTrain.Parameter.EndLocation = recipe.TrainRoiEndLocation;

            RoiInspect.Parameter.Overlay.Visible = false;
            RoiTrain.Parameter.Overlay.Visible = true;

            PatternMatchingResult result = workStage.scannerCompensator.GetResult();
            Box_VerifyScannerCameraOffset_ImageViewer.NormalOverlays.Add(RoiTrain.Parameter.Overlay);
            foreach (var overlay in result.ResultOverlays)
                Box_VerifyScannerCameraOffset_ImageViewer.NormalOverlays.Remove(overlay);

            Box_VerifyScannerCameraOffset_ImageViewer.Display();
            m_RoiListControl.RoiTrainClickNew();
        }

        private void button_VerifyScannerCameraOffset_Inspect_Click(object sender, EventArgs e)
        {
            ScannerCompensatorRecipe recipe = workStage.scannerCompensator.Recipe;
            RoiInspect.Parameter.StartLocation = recipe.InspectRoiStartLocation;
            RoiInspect.Parameter.EndLocation = recipe.InspectRoiEndLocation;

            RoiTrain.Parameter.Overlay.Visible = false;
            RoiInspect.Parameter.Overlay.Visible = true;

            PatternMatchingResult result = workStage.scannerCompensator.GetResult();
            Box_VerifyScannerCameraOffset_ImageViewer.NormalOverlays.Add(RoiInspect.Parameter.Overlay);
            foreach (var overlay in result.ResultOverlays)
                Box_VerifyScannerCameraOffset_ImageViewer.NormalOverlays.Remove(overlay);

            Box_VerifyScannerCameraOffset_ImageViewer.Display();
            m_RoiListControl.RoiAlignClickNew();
        }

        private void radioButton_VerifyScannerCameraOffset_Light_IR_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_VerifyScannerCameraOffset_Light_IR.Checked)
            {
                workStage.Config.ListIlluminationChannel[1].Value = _scannerCalConfig.Scanner_Calibration_Illumination_IR_Value;
            }
        }

        private void radioButton_VerifyScannerCameraOffset_Light_Red_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_VerifyScannerCameraOffset_Light_Red.Checked)
            {
                workStage.Config.ListIlluminationChannel[0].Value = _scannerCalConfig.Scanner_Calibration_Illumination_Red_Value;
            }
        }

        private void button_VerifyScannerCameraOffset_Train_Set_Click(object sender, EventArgs e)
        {
            if (workStage.scannerCompensator != null)
            {
                RoiVisionTool roiVisionTool = m_RoiListControl.RoiTrainVisionTool;

                RoiTrain.Parameter.CenterLocation = roiVisionTool.Parameter.CenterLocation;
                RoiTrain.Parameter.Size = roiVisionTool.Parameter.Size;

                Box_VerifyScannerCameraOffset_ImageViewer.NormalOverlays.Add(RoiTrain.Parameter.Overlay);
                RoiTrain.Parameter.Overlay.Visible = true;
                Box_VerifyScannerCameraOffset_ImageViewer.Display();
            }

            string m_strFile = "";
            string m_strRecipe = "";
            RecipeInfo m_recipeInfo = new RecipeInfo();
            m_recipeInfo = Equipment.GetCurrentRecipe();

            workStage.scannerCompensator.Camera.LatestImage = Box_VerifyScannerCameraOffset_ImageViewer.InputImage;

            if (Box_VerifyScannerCameraOffset_ImageViewer.Simulated)
            {
                workStage.scannerCompensator.Simulated = true;
                workStage.scannerCompensator.TestImage = Box_VerifyScannerCameraOffset_ImageViewer.InputImage;
            }
            else
            {
                workStage.scannerCompensator.Simulated = false;
            }

            workStage.scannerCompensator.Train();
            SetTrainImage(workStage.scannerCompensator.TrainImage);

            if (m_recipeInfo != null)
            {
                Equipment.UpdateRecipeData();
            }
            Equipment.SetCurrentRecipe(m_recipeInfo);
            Equipment.SaveRecipe();
            FormManager.FireUpdateRecipeEvent();

            Equipment.ApplyRecipeData();

            if (workStage.scannerCompensator.Name == "ScannerCompen. (Fine)")
            {
                m_strFile = string.Format("{0}\\ScannerCal.bmp", ConfigManager.GetPatternImagePath());
                workStage.scannerCompensator.TrainImage.Save(m_strFile, QMC.Common.Vision.VisionImage.FileFilter.jpg);
            }
            //m_RoiListControl.RoiTrainClickNew();
        }

        private void baseToggleButton_VerifyScannerCameraOffset_DuplicateCheck_Click(object sender, EventArgs e)
        {
            bool status = baseToggleButton_VerifyScannerCameraOffset_DuplicateCheck.GetButtonStatus();
            if (status)
            {
                baseToggleButton_VerifyScannerCameraOffset_DuplicateCheck.UpdateToggleStatus(false);
            }
            else
            {
                baseToggleButton_VerifyScannerCameraOffset_DuplicateCheck.UpdateToggleStatus(true);
            }
            PatternMatchingParameter.DuplicateChecked = status;
            _scannerCalConfig.Scanner_Calibration_PatternMatchingParameters.DuplicateChecked = status;
        }

        private void baseToggleButton_VerifyScannerCameraOffset_UseMaskImage_Click(object sender, EventArgs e)
        {
            bool status = baseToggleButton_VerifyScannerCameraOffset_UseMaskImage.GetButtonStatus();
            if (status)
            {
                baseToggleButton_VerifyScannerCameraOffset_UseMaskImage.UpdateToggleStatus(false);
            }
            else
            {
                baseToggleButton_VerifyScannerCameraOffset_UseMaskImage.UpdateToggleStatus(true);
            }
            PatternMatchingParameter.UseMaskImage = status;
            _scannerCalConfig.Scanner_Calibration_PatternMatchingParameters.UseMaskImage = status;
        }

        private void button_VerifyScannerCameraOffset_Search_Click(object sender, EventArgs e)
        {
            UpdataPositionData(0.0, 0.0, 0.0);

            if (Box_VerifyScannerCameraOffset_ImageViewer.Simulated)
            {
                workStage.scannerCompensator.Simulated = true;
                workStage.scannerCompensator.Camera.LatestImage = Box_VerifyScannerCameraOffset_ImageViewer.InputImage;
                workStage.scannerCompensator.TestImage = Box_VerifyScannerCameraOffset_ImageViewer.InputImage;
            }
            else
            {
                workStage.scannerCompensator.Simulated = false;
            }

            if (radioButton_VerifyScannerCameraOffset_Pattern.Checked)
            {
                _scannerCalConfig.Scanner_Calibration_TrainRoiStartLocation_X = RoiTrain.Parameter.StartLocation.X;
                _scannerCalConfig.Scanner_Calibration_TrainRoiStartLocation_Y = RoiTrain.Parameter.StartLocation.Y;
                _scannerCalConfig.Scanner_Calibration_TrainRoiEndLocation_X = RoiTrain.Parameter.EndLocation.X;
                _scannerCalConfig.Scanner_Calibration_TrainRoiEndLocation_Y = RoiTrain.Parameter.EndLocation.Y;
                _scannerCalConfig.Scanner_Calibration_InspectionRoiStartLocation_X = RoiInspect.Parameter.StartLocation.X;
                _scannerCalConfig.Scanner_Calibration_InspectionRoiStartLocation_Y = RoiInspect.Parameter.StartLocation.Y;
                _scannerCalConfig.Scanner_Calibration_InspectionRoiEndLocation_X = RoiInspect.Parameter.EndLocation.X;
                _scannerCalConfig.Scanner_Calibration_InspectionRoiEndLocation_Y = RoiInspect.Parameter.EndLocation.Y;
                _scannerCalConfig.Scanner_Calibration_Illumination_Red_Value = workStage.Config.ListIlluminationChannel[0].Value;
                _scannerCalConfig.Scanner_Calibration_Illumination_IR_Value = workStage.Config.ListIlluminationChannel[1].Value;

                PatternMatchingParameter.MaxTolerance = Equipment.ToDouble(basetextBox_VerifyScannerCameraOffset_AngleTolerance.Text);
                PatternMatchingParameter.MaxInstance = Equipment.ToInt(basetextBox_VerifyScannerCameraOffset_MaxInstance.Text);
                PatternMatchingParameter.MinTolerance = PatternMatchingParameter.MaxTolerance * -1;
                PatternMatchingParameter.MinScore = Equipment.ToDouble(basetextBox_VerifyScannerCameraOffset_MinScore.Text);
                PatternMatchingParameter.DuplicateChecked = baseToggleButton_VerifyScannerCameraOffset_DuplicateCheck.GetButtonStatus();
                PatternMatchingParameter.UseMaskImage = baseToggleButton_VerifyScannerCameraOffset_UseMaskImage.GetButtonStatus();
                PatternMatchingParameter.TrainImage = pictureBox_VerifyScannerCameraOffset_TrainImage.Image;

                workStage.scannerCompensator.Recipe.PatternMatchingParameter = PatternMatchingParameter;
                _scannerCalConfig.Scanner_Calibration_PatternMatchingParameters = PatternMatchingParameter;

                PatternMatchingResult result = workStage.scannerCompensator.Search();

                if (result != null)
                {
                    Box_VerifyScannerCameraOffset_ImageViewer.ResultOverlays.Clear();
                    foreach (var overlay in result.ResultOverlays)
                    {
                        Box_VerifyScannerCameraOffset_ImageViewer.ResultOverlays.Add(overlay);
                        overlay.Visible = true;
                    }
                }

                if (result != null && result.Values.Count > 0)
                {
                    if (IsPixel)
                    {
                        UpdataPositionData(result.Values[0].X, result.Values[0].Y, result.Values[0].R);
                    }
                }

                Box_VerifyScannerCameraOffset_ImageViewer.Display();
                _scannerCalConfig.SaveToIni();
            }
            else if (radioButton_VerifyScannerCameraOffset_Blob.Checked)
            {
                MessageBox.Show("Blob 검사 기능은 현재 구현되어 있지 않습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            workStage.scannerCompensator.Camera.StartLive();
        }


        private void ApplyAdvancedVisionUIToConfig()
        {
            try
            {
                // ROI
                _scannerCalConfig.Scanner_Calibration_TrainRoiStartLocation_X = RoiTrain.Parameter.StartLocation.X;
                _scannerCalConfig.Scanner_Calibration_TrainRoiStartLocation_Y = RoiTrain.Parameter.StartLocation.Y;
                _scannerCalConfig.Scanner_Calibration_TrainRoiEndLocation_X = RoiTrain.Parameter.EndLocation.X;
                _scannerCalConfig.Scanner_Calibration_TrainRoiEndLocation_Y = RoiTrain.Parameter.EndLocation.Y;

                _scannerCalConfig.Scanner_Calibration_InspectionRoiStartLocation_X = RoiInspect.Parameter.StartLocation.X;
                _scannerCalConfig.Scanner_Calibration_InspectionRoiStartLocation_Y = RoiInspect.Parameter.StartLocation.Y;
                _scannerCalConfig.Scanner_Calibration_InspectionRoiEndLocation_X = RoiInspect.Parameter.EndLocation.X;
                _scannerCalConfig.Scanner_Calibration_InspectionRoiEndLocation_Y = RoiInspect.Parameter.EndLocation.Y;

                // PatternMatching 설정
                _scannerCalConfig.Scanner_Calibration_PatternMatchingParameters = new PatternMatchingParameters
                {
                    MaxInstance = Equipment.ToInt(basetextBox_VerifyScannerCameraOffset_MaxInstance.Text),
                    MaxTolerance = Equipment.ToDouble(basetextBox_VerifyScannerCameraOffset_AngleTolerance.Text),
                    MinScore = Equipment.ToDouble(basetextBox_VerifyScannerCameraOffset_MinScore.Text),
                    DuplicateChecked = baseToggleButton_VerifyScannerCameraOffset_DuplicateCheck.GetButtonStatus(),
                    UseMaskImage = baseToggleButton_VerifyScannerCameraOffset_UseMaskImage.GetButtonStatus()
                };

                // Blob Tool 설정
                _scannerCalConfig.Scanner_Calibration_BlobVisionToolParameter = new BlobVisionToolParameter
                {
                    HardThreshold = BlobParameter.HardThreshold,
                    MinPixels = BlobParameter.MinPixels,
                    Polarity = BlobParameter.Polarity,
                    RepeatCount = BlobParameter.RepeatCount,
                    HasChanged = BlobParameter.HasChanged
                };

                workStage.scannerCompensator.Recipe.PatternMatchingParameter = _scannerCalConfig.Scanner_Calibration_PatternMatchingParameters;
                workStage.scannerCompensator.Recipe.BlobParameter = _scannerCalConfig.Scanner_Calibration_BlobVisionToolParameter;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Vision 고급 설정 적용 중 오류: " + ex.Message);
            }
        }

        private void radioButton_VerifyScannerCameraOffset_Pixel_CheckedChanged(object sender, EventArgs e)
        {
            IsPixel = true;
        }

        private void radioButton_VerifyScannerCameraOffset_mm_CheckedChanged(object sender, EventArgs e)
        {
            IsPixel = false;
        }

        private void hScrollBarIlluminator_ValueChanged(object sender, System.EventArgs e)
        {
            int value = hScrollBar_VerifyScannerCameraOffset_Illuminator.Value;
            textBox_VerifyScannerCameraOffset_IlluminationValue.Text = value.ToString();

            // IR 선택 시 채널 2
            if (radioButton_VerifyScannerCameraOffset_Light_IR.Checked)
            {
                workStage.Config.ListIlluminationChannel[1].Value = value;
                CommonModule.Instance.Illuminator.SetVolume(value, 2);  // IR 채널
                workStage.scannerCompensator.Illuminator.SetVolume(value, 2);
            }
            else // Red 선택 시 채널 1
            {
                workStage.Config.ListIlluminationChannel[0].Value = value;
                CommonModule.Instance.Illuminator.SetVolume(value, 1);  // Red 채널
                workStage.scannerCompensator.Illuminator.SetVolume(value, 1);
            }

            textBox_VerifyScannerCameraOffset_IlluminationValue.Refresh();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (!this.Created)
                return;

            if (this.Visible && !m_bFormVisible)
            {
                m_bFormVisible = true;
                //OnShow();
                //timer_Status.Start();
                this.Box_VerifyScannerCameraOffset_ImageViewer.ResumeDisplay();
                this.Box_VerifyScannerCameraOffset_ImageViewer.StartUpdateTask();
            }
            else if (!this.Visible && m_bFormVisible)
            {
                m_bFormVisible = false;
                //OnHide();
                //timer_Status.Stop();
                this.Box_VerifyScannerCameraOffset_ImageViewer.SuspendDisplay();
                this.Box_VerifyScannerCameraOffset_ImageViewer.StopUpdateTask();
            }
        }

        private void FormNew_VerifyScannerCameraOffset_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true; // 폼이 종료되지 않도록 방지
                //OnHide();
                this.Hide();     // 대신 숨긴다
            }
        }

        private void button_VerifyScannerCameraOffset_Camera_ExposureTime_High_Click(object sender, EventArgs e)
        {
            double dExposureTime = Equipment.ToDouble(textBox_VerifyScannerCameraOffset_Illuminator_Camera_ExposureTime_High.Text);

            if (workStage.jigAligner_HighRes.Camera.Opened)
            {
                workStage.jigAligner_HighRes.Camera.SetExposureTime(dExposureTime);

            }
        }
    }
}
