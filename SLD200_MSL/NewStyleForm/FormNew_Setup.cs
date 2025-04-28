using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using QMC.Common;
using QMC.Common.Modules;
using QMC.Common.Motion.Ajin;
using QMC.Vision;
using QMC.Core;
using static QMC.Common.Equipment;
using QMC.Common.Parts;
using System.Numerics;
using SpiralLab.Sirius;
using Point = System.Drawing.Point;
using System.Windows.Controls;
using ListViewItem = System.Windows.Forms.ListViewItem;
using QMC.Common.Hmi;
using QMC.Common.Vision.Tools;
using QMC.Common.Vision;
using QMC.Common.VisionPart;
using static OpenCvSharp.LineIterator;
using static QMC.Common.Vision.Tools.PatternMatchingResult;
using Cognex.VisionPro.Exceptions;
//using OpenCvSharp;

namespace SLD200_MSL
{
    public partial class FormNew_Setup : Form
    {
        static WorkStage workStage;
        static Loader loader;
        static Unloader unloader;
        static Bds Bds;

        FormNew_VisionPopup m_formVisionPopup = new FormNew_VisionPopup();
        FormNew_CommunicationTerminal m_formCommTerminal = new FormNew_CommunicationTerminal();

        //  IO
        private DioPointCollection m_DioPoints;
        private IOListControl Inputlist;
        private IOListControl Outputlist;
        private DioPointCollection InputPoint;
        private DioPointCollection OutputPoint;
        public Size IOGridSize { protected set; get; }

        private FormNew_KeyPad m_keyPad;
        public System.Windows.Forms.Timer timer_Status;

        protected XyzyStage m_Stage;
        private _2DMappingDataControl m_2DMappingDataControl;
        private _2DMappingFileControl m_2DMappingFileControl;

        // Scanner Calibration
        // 현재 스캐너 보정 파일
        private string m_srcFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "correction", "cor_1to1.ct5");
        // 신규로 생성할 스캐너 보정 파일
        private string m_targetFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "correction", $"newfile.ct5");
        // 3x3 (9개) 위치에 대한 보정 테이블 입력용
        private float m_fieldSize = 105;
        private float m_rowInterval = 2; //20;
        private float m_colInterval = 2; //20;
        private int m_row = 5; //3
        private int m_col = 5; //3
        private float m_kfactor = 0; // = (float)Math.Pow(2, 20) / m_fieldSize;
        private Correction2DRtc m_correction2DRtc = null;
        private Correction2DRtcForm m_correction2DRtcForm = null;

        //private FormScannerCompensatorMaint m_formScannerCompensatorMaint = null;
        //protected VisionImageViewer m_visionImageViewer_Upper;

        private RoiListControl m_RoiListControl;

        #region Property
        public RoiVisionTool RoiTrain { get; set; }
        public RoiVisionTool RoiInspect { get; set; }
        public PatternMatchingParameters PatternMatchingParameter { set; get; }
        public BlobVisionToolParameter BlobParameter { get; set; }

        public bool IsPixel { get; set; }
        #endregion

        public FormNew_Setup()
        {
            InitializeComponent();

            m_keyPad = new FormNew_KeyPad();

            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;

            foreach (Module module in m_collectionModules)
            {
                if (module.Name == "WorkStage")
                {
                    workStage = module as WorkStage;
                }

                if (module.Name == "Loader")
                {
                    loader = module as Loader;
                }

                if (module.Name == "Unloader")
                {
                    unloader = module as Unloader;
                }

                if (module.Name == "BDS")
                {
                    Bds = module as Bds;
                }
            }

            IOGridSize = new Size(730, 590);

            InputPoint = new DioPointCollection();
            OutputPoint = new DioPointCollection();
            Inputlist = new IOListControl(InputPoint, "Input List");
            Outputlist = new IOListControl(OutputPoint, "Output List");
            m_DioPoints = new DioPointCollection();
            SetDioPoint();              //  2022. 04. 12.  SCH : 전체 한번만 추가하기 위해서
            this.Inputlist.SetControlName("Input List");
            this.Outputlist.SetControlName("Output List");

            this.Inputlist.Size = new Size(IOGridSize.Width, IOGridSize.Height);
            this.Inputlist.Location = new Point(10, 10);
            this.Inputlist.Font = new Font("Tahoma", 10, FontStyle.Bold);
            this.tabPage_Setup_IO.Controls.Add(this.Inputlist);

            this.Outputlist.Size = new Size(IOGridSize.Width, IOGridSize.Height);
            this.Outputlist.Location = new Point(this.Inputlist.Location.X + this.Inputlist.Width, this.Inputlist.Location.Y);
            this.Outputlist.Font = new Font("Tahoma", 10, FontStyle.Bold);
            this.tabPage_Setup_IO.Controls.Add(this.Outputlist);

            this.VisibleChanged += FormNew_Setup_VisibleChanged;

            //  장비 타입에 따라 Mask 축 추가 / 제거
            if (Equipment.Machine_LaserType_CO2)
            {
                listBox_Setup_Motion_SelectAxis.Items.Clear();

                listBox_Setup_Motion_SelectAxis.Items.Add("Mask Y");
                listBox_Setup_Motion_SelectAxis.Items.Add("Mechanic Aligner X");
                listBox_Setup_Motion_SelectAxis.Items.Add("Mechanic Aligner Y");
                listBox_Setup_Motion_SelectAxis.Items.Add("Work Stage X");
                listBox_Setup_Motion_SelectAxis.Items.Add("Work Stage Y");
                listBox_Setup_Motion_SelectAxis.Items.Add("Work Head Z");
                listBox_Setup_Motion_SelectAxis.Items.Add("Loader R-Port Z");
                listBox_Setup_Motion_SelectAxis.Items.Add("Loader L-Port Z");
                listBox_Setup_Motion_SelectAxis.Items.Add("Loader Transfer X");
                listBox_Setup_Motion_SelectAxis.Items.Add("Loader Transfer Z");
                listBox_Setup_Motion_SelectAxis.Items.Add("Unloader R-Port Z");
                listBox_Setup_Motion_SelectAxis.Items.Add("Unloader L-Port Z");
                listBox_Setup_Motion_SelectAxis.Items.Add("Unloader Transfer X");
                listBox_Setup_Motion_SelectAxis.Items.Add("Unloader Transfer Z");
            }
            else        //  UV
            {
                listBox_Setup_Motion_SelectAxis.Items.Clear();

                listBox_Setup_Motion_SelectAxis.Items.Add("Mechanic Aligner X");
                listBox_Setup_Motion_SelectAxis.Items.Add("Mechanic Aligner Y");
                listBox_Setup_Motion_SelectAxis.Items.Add("Work Stage X");
                listBox_Setup_Motion_SelectAxis.Items.Add("Work Stage Y");
                listBox_Setup_Motion_SelectAxis.Items.Add("Work Head Z");
                listBox_Setup_Motion_SelectAxis.Items.Add("Loader R-Port Z");
                listBox_Setup_Motion_SelectAxis.Items.Add("Loader L-Port Z");
                listBox_Setup_Motion_SelectAxis.Items.Add("Loader Transfer X");
                listBox_Setup_Motion_SelectAxis.Items.Add("Loader Transfer Z");
                listBox_Setup_Motion_SelectAxis.Items.Add("Unloader R-Port Z");
                listBox_Setup_Motion_SelectAxis.Items.Add("Unloader L-Port Z");
                listBox_Setup_Motion_SelectAxis.Items.Add("Unloader Transfer X");
                listBox_Setup_Motion_SelectAxis.Items.Add("Unloader Transfer Z");
            }

            Axis_Parameter_Apply();
            Comm_Parameter_Apply();
            Machine_Option_Apply();
            MappingData_List_Apply();
            Flatness_Measurement_Position_Apply();

            workStage.MapData_Load();

            //  Status 타이머
            timer_Status = new System.Windows.Forms.Timer();
            timer_Status.Interval = 20;                                               //  50 이었는데 10으로 변경. (50은 너무 느린 감이 없지 않아 있음. 근데 10에서 잘 될란가...?)
            timer_Status.Tick += new System.EventHandler(Timer_Status_Func);
            timer_Status.Enabled = true;

            //  for 2D Mapping
            m_Stage = workStage.Stage;
            this.m_2DMappingDataControl = new _2DMappingDataControl(m_Stage);
            this.m_2DMappingDataControl.Location = new Point(10, 6);
            this.tabPage_Setup_2DMapping.Controls.Add(this.m_2DMappingDataControl);

            this.m_2DMappingFileControl = new _2DMappingFileControl(m_Stage);
            this.m_2DMappingFileControl.Location = new Point(m_2DMappingDataControl.Location.X + m_2DMappingDataControl.Size.Width + 20, m_2DMappingDataControl.Location.Y);
            this.tabPage_Setup_2DMapping.Controls.Add(this.m_2DMappingFileControl);


            //  Scanner Calibration
            textBox_Setup_ScannerCal_LaserFrequency.Text = Equipment.Scanner_Calibration_LaserFrequency.ToString();
            textBox_Setup_ScannerCal_PulseWidth.Text = Equipment.Scanner_Calibration_LaserPulseWidth.ToString();
            textBox_Setup_ScannerCal_LaserEnergy.Text = Equipment.Scanner_Calibration_LaserEnergy.ToString();
            textBox_Setup_ScannerCal_CrossMarkLength.Text = Equipment.Scanner_Calibration_CrossMarkLength.ToString();
            textBox_Setup_ScannerCal_MarkingSpeed.Text = Equipment.Scanner_Calibration_LaserMarkSpeed.ToString();
            textBox_Setup_ScannerCal_JumpSpeed.Text = Equipment.Scanner_Calibration_LaserJumpSpeed.ToString();
            textBox_Setup_ScannerCal_LaserOnDelay.Text = Equipment.Scanner_Calibration_LaserOnDelay.ToString();
            textBox_Setup_ScannerCal_LaserOffDelay.Text = Equipment.Scanner_Calibration_LaserOffDelay.ToString();
            textBox_Setup_ScannerCal_MarkDelay.Text = Equipment.Scanner_Calibration_MarkDelay.ToString();
            textBox_Setup_ScannerCal_JumpDelay.Text = Equipment.Scanner_Calibration_JumpDelay.ToString();
            textBox_Setup_ScannerCal_PolygonDelay.Text = Equipment.Scanner_Calibration_PolygonDelay.ToString();
            textBox_Setup_ScannerCal_CalAreaWidth.Text = Equipment.Scanner_Calibration_CalAreaWidth.ToString();
            textBox_Setup_ScannerCal_CalAreaHeight.Text = Equipment.Scanner_Calibration_CalAreaHeight.ToString();
            textBox_Setup_ScannerCal_CalPitch.Text = Equipment.Scanner_Calibration_CalPitch.ToString();
            //cal Last Position Display 하자.
            label_Setup_ScannerCal_LastPosX.Text = Equipment.Scanner_Calibration_PosX_Last.ToString();
            label_Setup_ScannerCal_LastPosY.Text = Equipment.Scanner_Calibration_PosY_Last.ToString();
            label_Setup_ScannerCal_OffsetX.Text = Equipment.Scanner_Vision_Offset_Setting_X.ToString();
            label_Setup_ScannerCal_OffsetY.Text = Equipment.Scanner_Vision_Offset_Setting_Y.ToString();
            label_Setup_S_V_OffsetX.Text = Equipment.stOffsetDistance.FromScannerToFineCam.X.ToString();
            label_Setup_S_V_OffsetY.Text = Equipment.stOffsetDistance.FromScannerToFineCam.Y.ToString();

            //m_fieldSize = Equipment.Scanner_Calibration_FieldSize;
            m_srcFile = Equipment.Scanner_Calibration_srcFilePath;
            m_targetFile = Equipment.Scanner_Calibration_targetFilePath;
            m_rowInterval = (float)Equipment.Scanner_Calibration_rowInterval;
            m_colInterval = (float)Equipment.Scanner_Calibration_colInterval;
            m_row = Equipment.Scanner_Calibration_rowCount; 
            m_col = Equipment.Scanner_Calibration_colCount;

            //m_kfactor = (float)Math.Pow(2, 20) / m_fieldSize;
            if (Equipment.Machine_LaserType_CO2)                                                                                //  CO2 레이저
            {
                // theoretically size of scanner field of view (이론적인 FOV 크기) : 60mm -> 50mm
                float fov = 72.5f;          //  MSL-CO2 장비에서 맞춘 데이터
                // k factor (bits/mm) = 2^20 / fov
                float kfactor = (float)Math.Pow(2, 20) / fov; //14461.43448275862

                if (kfactor == 0)
                    kfactor = (float)14461.43448275862; //18830.1889;

                m_fieldSize = fov;
                m_kfactor = kfactor;
            }
            else //  UV 레이저
            {
                // theoretically size of scanner field of view (이론적인 FOV 크기) : 60mm
                float fov = 105.0f;         //  MSL-UV 장비에서 맞춘 데이터
                // k factor (bits/mm) = 2^20 / fov
                float kfactor = (float)Math.Pow(2, 20) / fov; //9986.438095238095

                if (kfactor == 0)
                    kfactor = (float)9986.438095238095;

                m_fieldSize = fov;
                m_kfactor = kfactor;
            }

            m_correction2DRtc = new Correction2DRtc(m_kfactor, m_row, m_col, m_rowInterval, m_colInterval, m_srcFile, m_targetFile);
            m_correction2DRtcForm = new Correction2DRtcForm(m_correction2DRtc);
            m_correction2DRtcForm.TopLevel = false; // 폼을 최상위 폼이 아니도록 설정
            m_correction2DRtcForm.FormBorderStyle = FormBorderStyle.None; // 폼의 테두리를 제거
            m_correction2DRtcForm.Dock = DockStyle.Fill; // 폼을 패널에 맞게 채움
            this.textBox_ScannerCal_LaserFrequency.Controls.Add(m_correction2DRtcForm);
            m_correction2DRtcForm.Show();

            workStage.scannerCompensator.ActionSaveDone += OnSaveDone;
            workStage.scannerCompensator.ActionSaveDoneAllData += OnSaveDataDone;

            //m_formScannerCompensatorMaint = new FormScannerCompensatorMaint(workStage.scannerCompensator
            //Scanner Calibration Vision
            //this.pictureBox_Setup_ScannerCal_TrainImage.BackColor = Color.White;

            //this.Box_Setup_ScannerCal_ImageViewer = new VisionImageViewer();
            this.Box_Setup_ScannerCal_ImageViewer.SizeMode = PictureBoxSizeMode.CenterImage;
            this.Box_Setup_ScannerCal_ImageViewer.SuspendDisplay();
            //this.Box_Setup_ScannerCal_ImageViewer.Camera = workStage.Camera_HighRes;
            this.Box_Setup_ScannerCal_ImageViewer.Camera = workStage.scannerCompensator.Camera;
            //this.Controls.Add(this.Box_Setup_ScannerCal_ImageViewer);

            this.RoiTrain = workStage.scannerCompensator.GetTrainRoi();
            this.RoiInspect = workStage.scannerCompensator.GetInspectRoi();

            //this.m_RoiListControl = new RoiListControl(RoiTrain, RoiInspect, workStage.Camera_HighRes.Resolution);
            this.m_RoiListControl = new RoiListControl(RoiTrain, RoiInspect, workStage.scannerCompensator.Camera.Resolution);
            //this.m_RoiListControl.Location = new Point(this.m_visionImageViewer_Upper.Location.X,
            //    this.m_visionImageViewer_Upper.Location.Y + m_visionImageViewer_Upper.Height + Configuration.ControlGap);
            //this.m_RoiListControl.Location = new Point(Box_Setup_ScannerCal_ImageViewer.Location.X, 
            //    this.Box_Setup_ScannerCal_ImageViewer.Location.Y + Box_Setup_ScannerCal_ImageViewer.Height);
            this.m_RoiListControl.roiTrainButtonClick += RoiTrainButtonClick;
            this.m_RoiListControl.roiAlignButtonClick += RoiInspectButtonClick;
            this.m_RoiListControl.roiTrainSaveButtonClick += RoiTrainSaveButtonClick;
            this.m_RoiListControl.roiAlignSaveButtonClick += RoiInspectSaveButtonClick;
            //this.Controls.Add(this.m_RoiListControl);

            //this.m_RoiListControl.roiTrainClick += button_Setup_ScannerCal_Train_Click;
            //this.m_RoiListControl.roiAlignClick += RoiInspectClick;

            this.pictureBox_Setup_ScannerCal_TrainImage.BackColor = Color.SpringGreen;

            ScannerCompensatorRecipe recipe = workStage.scannerCompensator.Recipe;

            Point roi = new Point(0, 0);
            roi.X = (int)Equipment.Scanner_Calibration_TrainRoiStartLocation_X;
            roi.Y = (int)Equipment.Scanner_Calibration_TrainRoiStartLocation_Y;
            recipe.TrainRoiStartLocation = roi;
            RoiTrain.Parameter.StartLocation = roi;
            roi.X = (int)Equipment.Scanner_Calibration_TrainRoiEndLocation_X;
            roi.Y = (int)Equipment.Scanner_Calibration_TrainRoiEndLocation_Y;
            recipe.TrainRoiEndLocation = roi;
            RoiTrain.Parameter.EndLocation = roi;
            roi.X = (int)Equipment.Scanner_Calibration_InspectionRoiStartLocation_X;
            roi.Y = (int)Equipment.Scanner_Calibration_InspectionRoiStartLocation_Y;
            recipe.InspectRoiStartLocation = roi;
            RoiInspect.Parameter.StartLocation = roi;
            roi.X = (int)Equipment.Scanner_Calibration_InspectionRoiEndLocation_X;
            roi.Y = (int)Equipment.Scanner_Calibration_InspectionRoiEndLocation_Y;
            recipe.InspectRoiEndLocation = roi;
            RoiInspect.Parameter.EndLocation = roi;

            if (workStage.scannerCompensator.Recipe != null)
            {
                VisionImage visionImage = workStage.scannerCompensator.Recipe.PatternMatchingParameter.TrainImage;
                this.pictureBox_Setup_ScannerCal_TrainImage.Image = visionImage.GetImage();
            }
            else
            {
                this.pictureBox_Setup_ScannerCal_TrainImage.Image = null;
            }

            PatternMatchingParameter = new PatternMatchingParameters();

            if (workStage.scannerCompensator.Recipe != null)
            {
                this.SetPatternMatchingData(workStage.scannerCompensator.Recipe.PatternMatchingParameter);
                BlobParameter = workStage.scannerCompensator.Recipe.BlobParameter;
            }

            InitPatternMatchingParameter();
            InitBlobParameter();

            this.hScrollBar_Setup_ScannerCal_Illuminator.ValueChanged += new System.EventHandler(this.hScrollBarIlluminator_ValueChanged);
            SetScroll(0);

            this.radioButton_Setup_ScannerCal_Pattern.Checked = Equipment.Scanner_Calibration_UsePatternMatching;
            this.radioButton_Setup_ScannerCal_Blob.Checked = Equipment.Scanner_Calibration_UseBlobVisionTool;
            this.radioButton_Setup_ScannerCal_Cross.Checked = Equipment.Scanner_Calibration_MarkType_Cross;
            this.radioButton_Setup_ScannerCal_Circle.Checked = Equipment.Scanner_Calibration_MarkType_Circlle;
            
            //향 후 상황봐서 적용.
            //if (Equipment.Machine_LaserType_CO2)
            //{
            //    this.radioButton_Setup_ScannerCal_Blob.Checked = true;
            //    this.radioButton_Setup_ScannerCal_Circle.Checked = true;
            //    this.radioButton_Setup_ScannerCal_Pattern.Checked = false;
            //    this.radioButton_Setup_ScannerCal_Cross.Checked = false;
            //}
            //else
            //{
                //this.radioButton_Setup_ScannerCal_Pattern.Checked = true;
                //this.radioButton_Setup_ScannerCal_Cross.Checked = true;
                //this.radioButton_Setup_ScannerCal_Blob.Checked = false;
                //this.radioButton_Setup_ScannerCal_Circle.Checked = false;
            //}

            this.radioButton_Setup_ScannerCal_Light_IR.Checked = true;
            this.radioButton_Setup_ScannerCal_Light_Red.Checked = false;
            workStage.Config.ListIlluminationChannel[0].Value = Equipment.Scanner_Calibration_Illumination_channel_01_Value; //RED
            workStage.Config.ListIlluminationChannel[1].Value = Equipment.Scanner_Calibration_Illumination_channel_02_Value; //IR

            IsPixel = true;
            this.Refresh();

        }

        // ActionSaveDone 이벤트 핸들러
        private void OnSaveDone(string message)
        {
            //m_correction2DRtc.ConvertFromDatFile(message);
            //m_correction2DRtcForm.RefreshData();
            //m_correction2DRtc.Convert();

            
        }
        private void OnSaveDataDone(QMCFindLenzCenter list)
        {
            this.ProcessCorrectionData(list);
        }

        private void FormNew_Setup_VisibleChanged(object sender, EventArgs e)
        {
            Inputlist.ShowDataGridView();
            Outputlist.ShowDataGridView();
        }

        private void SetDioPoint()
        {
            m_DioPoints.Clear();
            //foreach (Module module in m_Modules)
            {
                //if (module != null)
                {
                    foreach (DioPoint point in Equipment.GetAllDioPointList())
                    {
                        DioPoint dioPoint = point;
                        m_DioPoints.Add(dioPoint);
                    }
                }
            }
            SetDioPointInfo(m_DioPoints);
            //this.baseLabel.Text = "All";
        }

        private void SetDioPointInfo(DioPointCollection IOList)
        {
            InputPoint.Clear();
            OutputPoint.Clear();
            if (IOList != null)
            {
                foreach (DioPoint one in IOList)
                {
                    if (one.IoType == IoType.Input)
                    {
                        InputPoint.Add(one);
                    }
                    else
                    {
                        OutputPoint.Add(one);
                    }
                }
                Inputlist.UpdateGridInfo(InputPoint);
                Outputlist.UpdateGridInfo(OutputPoint);
            }
            else
            {
                Inputlist.UpdateGridInfo(InputPoint);
                Outputlist.UpdateGridInfo(OutputPoint);
            }
        }

        private void Timer_Status_Func(object sender, EventArgs e)
        {
            timer_Status.Enabled = false;

            //DIO_Status();
            Motor_Position();

            //m_btimer_MainWork_Stop = false;
            //timer_MainWork.Enabled = false;

            //if (!m_btimer_MainWork_Stop)
            //{
            //    timer_MainWork.Enabled = true;
            //}

            //  MapData Status
            if (Equipment.MapDataStatus_Activate)
            {
                button_Setup_2DMapData_Apply.Text = "Map Data Activated";
                button_Setup_2DMapData_Apply.BackColor = Color.Lime;

                workStage.Stage.Config.Use2DMap = true;
            }
            else
            {
                button_Setup_2DMapData_Apply.Text = "Map Data Deactivated";
                button_Setup_2DMapData_Apply.BackColor = Color.LightGray;
                try
                {

                    workStage.Stage.Config.Use2DMap = false;
                }catch(Exception ex)
                {
                    //Log.Write(ex);
                }
            }


            //  Scanner Calibration
            label_Setup_ScannerCal_LastPosX.Text = Equipment.Scanner_Calibration_PosX_Last.ToString();
            label_Setup_ScannerCal_LastPosY.Text = Equipment.Scanner_Calibration_PosY_Last.ToString();

            timer_Status.Enabled = true;
        }

        private void Motor_Position()
        {
            //  Loader Stacker Position
            label_Setup_EncPosition_LD_Z0.Text = string.Format("{0:F3}", loader.MC_Func.MC_GetEncPos((int)Loader.nAxis.Z0));
            label_Setup_EncPosition_LD_Z1.Text = string.Format("{0:F3}", loader.MC_Func.MC_GetEncPos((int)Loader.nAxis.Z1));

            //  Loader Transfer Position
            label_Setup_EncPosition_LD_TRX.Text = string.Format("{0:F3}", loader.MC_Func.MC_GetEncPos((int)Loader.nAxis.TR_X));
            label_Setup_EncPosition_LD_TRZ.Text = string.Format("{0:F3}", loader.MC_Func.MC_GetEncPos((int)Loader.nAxis.TR_Z));

            //  Loader Mechanic Aligner Position
            label_Setup_EncPosition_LD_ALNX.Text = string.Format("{0:F3}", loader.MC_Func.MC_GetEncPos((int)Loader.nAxis.ALN_X));
            label_Setup_EncPosition_LD_ALNY.Text = string.Format("{0:F3}", loader.MC_Func.MC_GetEncPos((int)Loader.nAxis.ALN_Y));


            //  Work Stage Position
            label_Setup_EncPosition_STAGE_X.Text = string.Format("{0:F3}", workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X));
            label_Setup_EncPosition_STAGE_Y.Text = string.Format("{0:F3}", workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y));

            //  Scanner & Camera Position
            label_Setup_EncPosition_SCANNER_Z.Text = string.Format("{0:F3}", workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Z));

            if (Equipment.Machine_LaserType_CO2)
            {
                //  Mask Position
                label_Setup_EncPosition_MASK_Y.Text = string.Format("{0:F3}", workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.MASK_Y));
            }
            else
            {
                label_Setup_EncPosition_MASK_Y.Text = "0.0";
            }

            //  Unloader Stacker Position
            label_Setup_EncPosition_UL_Z0.Text = string.Format("{0:F3}", unloader.MC_Func.MC_GetEncPos((int)Unloader.nAxis.Z0));
            label_Setup_EncPosition_UL_Z1.Text = string.Format("{0:F3}", unloader.MC_Func.MC_GetEncPos((int)Unloader.nAxis.Z1));

            //  Unloader Transfer Position
            label_Setup_EncPosition_UL_TRX.Text = string.Format("{0:F3}", unloader.MC_Func.MC_GetEncPos((int)Unloader.nAxis.TR_X));
            label_Setup_EncPosition_UL_TRZ.Text = string.Format("{0:F3}", unloader.MC_Func.MC_GetEncPos((int)Unloader.nAxis.TR_Z));
        }

        public bool Axis_Parameter_Exist()
        {
            bool m_bRet = false;
            string strFIle = "";

            strFIle = ConfigManager.GetConfigPath() + "\\Axis Setting (Do not delete or modify).ini";

            if (File.Exists(strFIle))
            {
                m_bRet = true;
            }

            return m_bRet;
        }

        public bool Axis_Parameter_Apply()
        {
            string strTemp = "";

            bool m_bRet = true;
            string strFIle = "";
            StringBuilder temp = new StringBuilder(255);

            strFIle = ConfigManager.GetConfigPath() + "\\Axis Setting (Do not delete or modify).ini";

            if (File.Exists(strFIle) == false)
            {
                MessageBox.Show("Axis Setting 파일이 없습니다.\r\n\r\n[Default 값으로 설정됩니다.]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return false;
            }

            //  축 파라미터 파일을 로드 하면 맨 첫번째 축 데이터를 표시하도록 한다.
            listBox_Setup_Motion_SelectAxis.SelectedIndex = 0;

            comboBox_Setup_Motion_Limit_Install.SelectedIndex = Equipment.stAxisParam[0].LimitSensor_Installed;
            comboBox_Setup_Motion_Limit_Active.SelectedIndex = Equipment.stAxisParam[0].LimitSensor_ActiveLevel;

            comboBox_Setup_Motion_Home_Sensing.SelectedIndex = Equipment.stAxisParam[0].Home_Sensing;
            comboBox_Setup_Motion_Home_Install.SelectedIndex = Equipment.stAxisParam[0].Home_Installed;
            comboBox_Setup_Motion_Home_Active.SelectedIndex = Equipment.stAxisParam[0].Home_ActiveLevel;
            comboBox_Setup_Motion_Home_Direction.SelectedIndex = Equipment.stAxisParam[0].Home_Direction;
            textBox_Setup_Motion_Home_Speed_1st.Text = Equipment.stAxisParam[0].Home_Speed_1st.ToString();
            textBox_Setup_Motion_Home_Speed_2nd.Text = Equipment.stAxisParam[0].Home_Speed_2nd.ToString();
            textBox_Setup_Motion_Home_Speed_3rd.Text = Equipment.stAxisParam[0].Home_Speed_3rd.ToString();
            textBox_Setup_Motion_Home_Speed_Last.Text = Equipment.stAxisParam[0].Home_Speed_Last.ToString();
            textBox_Setup_Motion_Home_Offset.Text = Equipment.stAxisParam[0].Home_Offset.ToString();

            textBox_Setup_Motion_Common_Unit.Text = Equipment.stAxisParam[0].Common_UnitPerPulse_Unit.ToString();
            textBox_Setup_Motion_Common_Pulse.Text = Equipment.stAxisParam[0].Common_UnitPerPulse_Pulse.ToString();
            textBox_Setup_Motion_Common_MinAcc.Text = Equipment.stAxisParam[0].Common_Acceleration_Min.ToString();
            textBox_Setup_Motion_Common_MaxAcc.Text = Equipment.stAxisParam[0].Common_Acceleration_Max.ToString();
            textBox_Setup_Motion_Common_FineAcc.Text = Equipment.stAxisParam[0].Common_Acceleration_Fine.ToString();
            textBox_Setup_Motion_Common_CoarseAcc.Text = Equipment.stAxisParam[0].Common_Acceleration_Coarse.ToString();
            textBox_Setup_Motion_Common_MaxSpeed.Text = Equipment.stAxisParam[0].Common_Speed_Max.ToString();
            textBox_Setup_Motion_Common_MinSpeed.Text = Equipment.stAxisParam[0].Common_Speed_Min.ToString();
            textBox_Setup_Motion_Common_FineSpeed.Text = Equipment.stAxisParam[0].Common_Speed_Fine.ToString();
            textBox_Setup_Motion_Common_CoarseSpeed.Text = Equipment.stAxisParam[0].Common_Speed_Coarse.ToString();
            textBox_Setup_Motion_Common_MinPos.Text = Equipment.stAxisParam[0].Common_Position_Min.ToString();
            textBox_Setup_Motion_Common_MaxPos.Text = Equipment.stAxisParam[0].Common_Position_Max.ToString();
            textBox_Setup_Motion_Common_SettleDelay.Text = Equipment.stAxisParam[0].Common_Settle_Delay.ToString();

            textBox_Setup_Motion_Jog_FineSpeed.Text = Equipment.stAxisParam[0].Jog_Speed_Fine.ToString();
            textBox_Setup_Motion_Jog_CoarseSpeed.Text = Equipment.stAxisParam[0].Jog_Speed_Coarse.ToString();
            textBox_Setup_Motion_Jog_MinStepSize.Text = Equipment.stAxisParam[0].Jog_StepSize_Min.ToString();
            textBox_Setup_Motion_Jog_MaxStepSize.Text = Equipment.stAxisParam[0].Jog_StepSize_Max.ToString();
            textBox_Setup_Motion_Jog_FineStepSize.Text = Equipment.stAxisParam[0].Jog_StepSize_Fine.ToString();
            textBox_Setup_Motion_Jog_CoarseStepSize.Text = Equipment.stAxisParam[0].Jog_StepSize_Coarse.ToString();

            return m_bRet;
        }

        public void Axis_Properties_Apply()
        {
            int lAxisNo = 0;
            int lMovePulse = 1;
            uint duModeProfile = 0, duUseAlarm;
            uint duMethodPulse = 0, duMethodEncoder = 0, duModeAbsRel = 0;
            double dVelocityMin = 1.0, dVelocityMax = 100.0, dMoveUnit = 1.0;

            uint duUseInp, duLevelAlmRst, duRetCode;
            uint duLevelLimitN = 0, duLevelLimitP = 0, duLevelZPhase = 0;
            uint duModeStop = 0, duLevelStop = 0, duLevelServoOn = 0;
            uint duTypeEncoder = 0;

            int lHomeDir = 0;
            uint duHomeSignal = 0, duLevelHome = 0, duZphas = 0;
            double dHomeClrTime = 1000.0, dHomeOffset = 0.0;

            uint duUse = 0, duStopMode = 0, duSelection = 0;
            double dPositivePos = 0.0, dNegativePos = 0.0;

            double dInitPos, dInitVel, dInitAccel, dInitDecel;

            for (int i = 0; i < Equipment.Max_Axis; i++)
            {
                //  Limit Install
                if (Equipment.stAxisParam[i].LimitSensor_Installed == 0)            //  Not Installed
                {
                    AXM.SetNegativeLimitNotUse(i);
                    AXM.SetPositiveLimitNotUse(i);
                }
                else if (Equipment.stAxisParam[i].LimitSensor_Installed == 1)       //  Installed
                {
                    AXM.SetNegativeLimitAction(i, MotorEventAction.Stop);
                    AXM.SetPositiveLimitAction(i, MotorEventAction.Stop);
                }

                //  Limit Active Level
                if (Equipment.stAxisParam[i].LimitSensor_ActiveLevel == 0)          //  Active Low
                {
                    AXM.SetNegativeLimitLevel(i, ActiveLevel.Low);
                    AXM.SetPositiveLimitLevel(i, ActiveLevel.Low);
                }
                else if (Equipment.stAxisParam[i].LimitSensor_ActiveLevel == 1)     //  Active High
                {
                    AXM.SetNegativeLimitLevel(i, ActiveLevel.High);
                    AXM.SetPositiveLimitLevel(i, ActiveLevel.High);
                }

                //  Home Direction
                if (Equipment.stAxisParam[i].Home_Direction == 0)                   //  Negative (CCW)
                {
                    lHomeDir = (int)Directions.Ccw;
                }
                else if (Equipment.stAxisParam[i].Home_Direction == 1)              //  Positive (CW)
                {
                    lHomeDir = (int)Directions.Cw;
                }

                //  Home Active Level
                if (Equipment.stAxisParam[i].Home_ActiveLevel == 0)                 //  Active Low
                {
                    AXM.SetHomeSensorLevel(i, ActiveLevel.Low);
                }
                else if (Equipment.stAxisParam[i].Home_ActiveLevel == 1)            //  Active High
                {
                    AXM.SetHomeSensorLevel(i, ActiveLevel.High);
                }

                //  Home Offset
                dHomeOffset = Equipment.stAxisParam[i].Home_Offset;

                //  Home Sensing
                if (Equipment.stAxisParam[i].Home_Sensing == 0)                     //  Using Home Sensor
                {
                    AXM.SetHomeMethod(i, (Directions)lHomeDir, HomeSignals.HomeSensor, (ZPhaseMethods)Equipment.stAxisParam[i].Home_ZPhase_Use, Equipment.stAxisParam[i].Home_Clear_Time, dHomeOffset);
                }
                else if (Equipment.stAxisParam[i].Home_Sensing == 1)                //  Using -Limit Sensor
                {
                    AXM.SetHomeMethod(i, (Directions)lHomeDir, HomeSignals.NegativeLimit, (ZPhaseMethods)Equipment.stAxisParam[i].Home_ZPhase_Use, Equipment.stAxisParam[i].Home_Clear_Time, dHomeOffset);
                }
                else if (Equipment.stAxisParam[i].Home_Sensing == 2)                //  Using +Limit Sensor
                {
                    AXM.SetHomeMethod(i, (Directions)lHomeDir, HomeSignals.PositiveLimit, (ZPhaseMethods)Equipment.stAxisParam[i].Home_ZPhase_Use, Equipment.stAxisParam[i].Home_Clear_Time, dHomeOffset);
                }

                //  Home Speed
                AXM.SetHomeVelocity(i, Equipment.stAxisParam[i].Home_Speed_1st, Equipment.stAxisParam[i].Home_Speed_2nd, Equipment.stAxisParam[i].Home_Speed_3rd, Equipment.stAxisParam[i].Home_Speed_Last, 
                                        Equipment.stAxisParam[i].Home_Acceleration_1st, Equipment.stAxisParam[i].Home_Acceleration_2nd);

                //  Unit Per Pulse
                AXM.AxmMotSetMoveUnitPerPulse(i, Equipment.stAxisParam[i].Common_UnitPerPulse_Unit, Equipment.stAxisParam[i].Common_UnitPerPulse_Pulse);


                //  요것들은 나중에 하자...

                //textBox_Setup_Motion_Common_MinAcc.Text = Equipment.stAxisParam[i].Common_Acceleration_Min.ToString();
                //textBox_Setup_Motion_Common_MaxAcc.Text = Equipment.stAxisParam[i].Common_Acceleration_Max.ToString();
                //textBox_Setup_Motion_Common_FineAcc.Text = Equipment.stAxisParam[i].Common_Acceleration_Fine.ToString();
                //textBox_Setup_Motion_Common_CoarseAcc.Text = Equipment.stAxisParam[i].Common_Acceleration_Coarse.ToString();
                //textBox_Setup_Motion_Common_MaxSpeed.Text = Equipment.stAxisParam[i].Common_Speed_Max.ToString();
                //textBox_Setup_Motion_Common_MinSpeed.Text = Equipment.stAxisParam[i].Common_Speed_Min.ToString();
                //textBox_Setup_Motion_Common_FineSpeed.Text = Equipment.stAxisParam[i].Common_Speed_Fine.ToString();
                //textBox_Setup_Motion_Common_CoarseSpeed.Text = Equipment.stAxisParam[i].Common_Speed_Coarse.ToString();
                //textBox_Setup_Motion_Common_MinPos.Text = Equipment.stAxisParam[i].Common_Position_Min.ToString();
                //textBox_Setup_Motion_Common_MaxPos.Text = Equipment.stAxisParam[i].Common_Position_Max.ToString();
                //textBox_Setup_Motion_Common_SettleDelay.Text = Equipment.stAxisParam[i].Common_Settle_Delay.ToString();

                //textBox_Setup_Motion_Jog_FineSpeed.Text = Equipment.stAxisParam[i].Jog_Speed_Fine.ToString();
                //textBox_Setup_Motion_Jog_CoarseSpeed.Text = Equipment.stAxisParam[i].Jog_Speed_Coarse.ToString();
                //textBox_Setup_Motion_Jog_MinStepSize.Text = Equipment.stAxisParam[i].Jog_StepSize_Min.ToString();
                //textBox_Setup_Motion_Jog_MaxStepSize.Text = Equipment.stAxisParam[i].Jog_StepSize_Max.ToString();
                //textBox_Setup_Motion_Jog_FineStepSize.Text = Equipment.stAxisParam[i].Jog_StepSize_Fine.ToString();
                //textBox_Setup_Motion_Jog_CoarseStepSize.Text = Equipment.stAxisParam[i].Jog_StepSize_Coarse.ToString();
            }

            //// Pulse/Encoder Method && Move Parameter Setting
            //duMethodPulse = ConvertComboToAxm(ref cboPulse);
            //duMethodEncoder = ConvertComboToAxm(ref cboEncoder);
            //duUseAlarm = ConvertComboToAxm(ref cboAlarm);
            //duModeAbsRel = ConvertComboToAxm(ref cboAbsRel);
            //duModeProfile = ConvertComboToAxm(ref cboProfile);
            //dVelocityMin = Equipment.ToDouble(edtMinVel.Text);
            //dVelocityMax = Equipment.ToDouble(edtMaxVel.Text);
            //lMovePulse = Equipment.ToInt(edtMovePulse.Text);
            //dMoveUnit = Equipment.ToDouble(edtMoveUnit.Text);

            //// Input/Output Signal Setting
            //duUseInp = ConvertComboToAxm(ref cboInp);
            //duLevelAlmRst = ConvertComboToAxm(ref cboAlarmReset);
            //duLevelLimitN = ConvertComboToAxm(ref cboELimitN);
            //duLevelLimitP = ConvertComboToAxm(ref cboELimitP);
            //duLevelZPhase = ConvertComboToAxm(ref cboZPhaseLev);
            //duModeStop = ConvertComboToAxm(ref cboStopMode);
            //duLevelStop = ConvertComboToAxm(ref cboStopLevel);
            //duLevelServoOn = ConvertComboToAxm(ref cboServoOn);
            //duTypeEncoder = ConvertComboToAxm(ref cboEncoderType);

            //// Home Search Setting
            //duLevelHome = ConvertComboToAxm(ref cboHomeLevel);
            //duHomeSignal = ConvertComboToAxm(ref cboHomeSignal);
            //lHomeDir = (int)ConvertComboToAxm(ref cboHomeDir);
            //duZphas = ConvertComboToAxm(ref cboZPhaseUse);
            //dHomeClrTime = Equipment.ToDouble(edtHomeClrTime.Text);
            //dHomeOffset = Equipment.ToDouble(edtHomeOffset.Text);

            //// Software Limit Setting
            //dNegativePos = Equipment.ToDouble(edtSwPosN.Text);
            //dPositivePos = Equipment.ToDouble(edtSwPosP.Text);

            //// User Move Parameter Setting
            //dInitPos = Equipment.ToDouble(edtPosition.Text);
            //dInitVel = Equipment.ToDouble(edtVelocity.Text);
            //dInitAccel = Equipment.ToDouble(edtAccel.Text);
            //dInitDecel = Equipment.ToDouble(edtDecel.Text);

            //for (lAxisNo = 0; lAxisNo < m_lAxisCounts; lAxisNo++)
            //{
            //    // Pulse/Encoder Method && Move Parameter Setting
            //    if (cboPulse.Enabled == true)
            //    {
            //        //++ 지정 축의 펄스 출력 방식을 설정합니다.
            //        //uMethod : (0)OneHighLowHigh   - 1펄스 방식, PULSE(Active High), 정방향(DIR=Low)  / 역방향(DIR=High)
            //        //          (1)OneHighHighLow   - 1펄스 방식, PULSE(Active High), 정방향(DIR=High) / 역방향(DIR=Low)
            //        //          (2)OneLowLowHigh    - 1펄스 방식, PULSE(Active Low),  정방향(DIR=Low)  / 역방향(DIR=High)
            //        //          (3)OneLowHighLow    - 1펄스 방식, PULSE(Active Low),  정방향(DIR=High) / 역방향(DIR=Low)
            //        //          (4)TwoCcwCwHigh     - 2펄스 방식, PULSE(CCW:역방향),  DIR(CW:정방향),  Active High     
            //        //          (5)TwoCcwCwLow      - 2펄스 방식, PULSE(CCW:역방향),  DIR(CW:정방향),  Active Low     
            //        //          (6)TwoCwCcwHigh     - 2펄스 방식, PULSE(CW:정방향),   DIR(CCW:역방향), Active High
            //        //          (7)TwoCwCcwLow      - 2펄스 방식, PULSE(CW:정방향),   DIR(CCW:역방향), Active Low
            //        //          (8)TwoPhase         - 2상(90' 위상차),  PULSE lead DIR(CW: 정방향), PULSE lag DIR(CCW:역방향)
            //        //          (9)TwoPhaseReverse  - 2상(90' 위상차),  PULSE lead DIR(CCW: 정방향), PULSE lag DIR(CW:역방향)
            //        CAXM.AxmMotSetPulseOutMethod(lAxisNo, duMethodPulse);
            //    }

            //    if (cboEncoder.Enabled == true)
            //    {
            //        //++ 지정 축의 Encoder 입력 방식을 설정합니다.
            //        // uMethod : (0)ObverseUpDownMode - 정방향 Up/Down
            //        //           (1)ObverseSqr1Mode   - 정방향 1체배
            //        //           (2)ObverseSqr2Mode   - 정방향 2체배
            //        //           (3)ObverseSqr4Mode   - 정방향 4체배
            //        //           (4)ReverseUpDownMode - 역방향 Up/Down
            //        //           (5)ReverseSqr1Mode   - 역방향 1체배
            //        //           (6)ReverseSqr2Mode   - 역방향 2체배
            //        //           (7)ReverseSqr4Mode   - 역방향 4체배
            //        CAXM.AxmMotSetEncInputMethod(lAxisNo, duMethodEncoder);
            //    }

            //    if (cboAlarm.Enabled == true)
            //    {
            //        //++ 지정 축의 비상정지 신호 사용유무/Active Level을 설정합니다. 
            //        CAXM.AxmSignalSetServoAlarm(lAxisNo, duUseAlarm);
            //    }
            //    if (cboAbsRel.Enabled == true)
            //    {
            //        //++ 지정 축의 구동 좌표계를 설정합니다. 
            //        // duAbsRelMode : (0)POS_ABS_MODE - 현재 위치와 상관없이 지정한 위치로 절대좌표 이동합니다.
            //        //                (1)POS_REL_MODE - 현재 위치에서 지정한 양만큼 상대좌표 이동합니다.
            //        CAXM.AxmMotSetAbsRelMode(lAxisNo, duMethodPulse);
            //    }
            //    if (cboProfile.Enabled == true)
            //    {
            //        // duProfileMode : (0)SYM_TRAPEZOID_MODE  - Symmetric Trapezoid
            //        //                 (1)ASYM_TRAPEZOID_MODE - Asymmetric Trapezoid
            //        //                 (2)QUASI_S_CURVE_MODE  - Symmetric Quasi-S Curve
            //        //                 (3)SYM_S_CURVE_MODE    - Symmetric S Curve
            //        //                 (4)ASYM_S_CURVE_MODE   - Asymmetric S Curve
            //        CAXM.AxmMotSetProfileMode(lAxisNo, duModeProfile);
            //    }

            //    //++ 지정 축의 초기 구동속도를 설정합니다.
            //    CAXM.AxmMotSetMinVel(lAxisNo, dVelocityMin);

            //    //++ 지정 축의 최대 구동속도를 설정합니다.
            //    CAXM.AxmMotSetMaxVel(lAxisNo, dVelocityMax);

            //    //++ 지정 축의 거리/속도/가속도의 제어단위를 설정합니다.
            //    CAXM.AxmMotSetMoveUnitPerPulse(lAxisNo, dMoveUnit, lMovePulse);

            //    // Input/Output Signal Setting
            //    //++ 지정 축의 Inposition(위치결정완료) 신호 Active Level/사용유무를 설정합니다.
            //    // - Inposition 신호를 사용안함으로 설정하면 모션제어 칩에서 펄스출력이 완료될 때 즉시구동 종료됩니다.
            //    // ※ [CAUTION] Inposition 신호를 사용함으로 설정하면 모션제어 칩에서 펄스출력이 완료된 후 서보팩으로 부터 
            //    //              Inposition(위치결정완료) 신호가 Active될 때 까지 모션 구동중으로 됩니다.
            //    // ※ [CAUTION] Inposition 신호를 사용할 때 Active Level이 맞지않으면 최초 한번 구동 후 모션구동이 종료되지않아 
            //    //              다음 구동을 할 수 없게 됩니다. 
            //    CAXM.AxmSignalSetInpos(lAxisNo, duUseInp);

            //    //++ 지정 축의 Alarm Reset 신호 Active Level을 설정합니다.
            //    CAXM.AxmSignalSetServoAlarmResetLevel(lAxisNo, duLevelAlmRst);



            //    //++ 지정 축의 Z상 Active Level을 설정합니다.
            //    CAXM.AxmSignalSetZphaseLevel(lAxisNo, duLevelZPhase);

            //    //++ 지정 축의 Emergency 신호 Active Level/사용유무를 설정합니다.
            //    CAXM.AxmSignalSetStop(lAxisNo, duModeStop, duLevelStop);

            //    //++ 지정 축의 Servo On/Off 신호 Active Level을 설정합니다.
            //    CAXM.AxmSignalSetServoOnLevel(lAxisNo, duLevelServoOn);

            //    //++ 지정 축의 Encoder Input Type를 설정합니다.
            //    // duEncoderType : (0)TYPE_INCREMENTAL
            //    //                 (1)TYPE_ABSOLUTE
            //    CAXDev.AxmSignalSetEncoderType(lAxisNo, duTypeEncoder);

            //    // Home Search Setting
            //    //++ 지정 축의 원점신호 Active Level을 설정합니다.
            //    CAXM.AxmHomeSetSignalLevel(lAxisNo, duLevelHome);

            //    //++ 지정 축의 원점검색 관련 정보들을 설정합니다.
            //    CAXM.AxmHomeSetMethod(lAxisNo, lHomeDir, duHomeSignal, duZphas, dHomeClrTime, dHomeOffset);

            //    // Software Limit Setting
            //    //++ 지정한 축에 Software Limit기능을 확인합니다.
            //    duRetCode = CAXM.AxmSignalGetSoftLimit(lAxisNo, ref duUse, ref duStopMode, ref duSelection, ref dPositivePos, ref dNegativePos);
            //    if (duRetCode == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            //    {
            //        //++ 지정 축의 소프트웨어 리미트를 설정합니다.
            //        // uUse       : (0)DISABLE        - 소프트웨어 리미트 기능을 사용하지 않습니다.
            //        //              (1)ENABLE         - 소프트웨어 리미트 기능을 사용합니다.
            //        // uStopMode  : (0)EMERGENCY_STOP - 소프트웨어 리미트 영역을 벗어날 경우 급정지합니다.
            //        //              (1)SLOWDOWN_STOP  - 소프트웨어 리미트 영역을 벗어날 경우 감속정지합니다.
            //        // uSelection : (0)COMMAND        - 기준위치를 지령위치로 합니다.
            //        //              (1)ACTUAL         - 기준위치를 엔코더 위치로 합니다.
            //        CAXM.AxmSignalSetSoftLimit(lAxisNo, duUse, duStopMode, duSelection, dPositivePos, dNegativePos);
            //    }

            //    //++ 지정 축의 사용자 관련 파라메타들을 설정합니다.
            //    CAXM.AxmMotSetParaLoad(lAxisNo, dInitPos, dInitVel, dInitAccel, dInitDecel);
            //}
        }

        public void Axis_Parameter_Save()
        {
            string strTemp = "";

            string strFIle = "";
            strFIle = ConfigManager.GetConfigPath() + "\\Axis Setting (Do not delete or modify).ini";

            if (File.Exists(strFIle) == false)
            {
                File.Create(strFIle);
                //return;

                MessageBox.Show("Axis Setting 파일을 생성하였습니다. 다시 시도하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //  선택된 축에 대한 데이터 갖다 넣기
            int m_nIndex = listBox_Setup_Motion_SelectAxis.SelectedIndex;

            if (m_nIndex >= 0)
            {
                Equipment.stAxisParam[m_nIndex].LimitSensor_Installed = comboBox_Setup_Motion_Limit_Install.SelectedIndex;
                Equipment.stAxisParam[m_nIndex].LimitSensor_ActiveLevel = comboBox_Setup_Motion_Limit_Active.SelectedIndex;

                Equipment.stAxisParam[m_nIndex].Home_Sensing = comboBox_Setup_Motion_Home_Sensing.SelectedIndex;
                Equipment.stAxisParam[m_nIndex].Home_Installed = comboBox_Setup_Motion_Home_Install.SelectedIndex;
                Equipment.stAxisParam[m_nIndex].Home_ActiveLevel = comboBox_Setup_Motion_Home_Active.SelectedIndex;
                Equipment.stAxisParam[m_nIndex].Home_Direction = comboBox_Setup_Motion_Home_Direction.SelectedIndex;
                Equipment.stAxisParam[m_nIndex].Home_Speed_1st = Equipment.ToDouble(textBox_Setup_Motion_Home_Speed_1st.Text);
                Equipment.stAxisParam[m_nIndex].Home_Speed_2nd = Equipment.ToDouble(textBox_Setup_Motion_Home_Speed_2nd.Text);
                Equipment.stAxisParam[m_nIndex].Home_Speed_3rd = Equipment.ToDouble(textBox_Setup_Motion_Home_Speed_3rd.Text);
                Equipment.stAxisParam[m_nIndex].Home_Speed_Last = Equipment.ToDouble(textBox_Setup_Motion_Home_Speed_Last.Text);
                Equipment.stAxisParam[m_nIndex].Home_Clear_Time = Equipment.ToDouble(textBox_Setup_Motion_Home_ClearTime.Text);
                Equipment.stAxisParam[m_nIndex].Home_ZPhase_Use = comboBox_Setup_Motion_Home_ZPhaseUse.SelectedIndex;
                Equipment.stAxisParam[m_nIndex].Home_Offset = Equipment.ToDouble(textBox_Setup_Motion_Home_Offset.Text);
                Equipment.stAxisParam[m_nIndex].Home_Acceleration_1st = Equipment.ToDouble(textBox_Setup_Motion_Home_Acceleration_1st.Text);
                Equipment.stAxisParam[m_nIndex].Home_Acceleration_2nd = Equipment.ToDouble(textBox_Setup_Motion_Home_Acceleration_2nd.Text);

                Equipment.stAxisParam[m_nIndex].Common_UnitPerPulse_Unit = Equipment.ToDouble(textBox_Setup_Motion_Common_Unit.Text);
                Equipment.stAxisParam[m_nIndex].Common_UnitPerPulse_Pulse = Equipment.ToInt(textBox_Setup_Motion_Common_Pulse.Text);
                Equipment.stAxisParam[m_nIndex].Common_Acceleration_Min = Equipment.ToDouble(textBox_Setup_Motion_Common_MinAcc.Text);
                Equipment.stAxisParam[m_nIndex].Common_Acceleration_Max = Equipment.ToDouble(textBox_Setup_Motion_Common_MaxAcc.Text);
                Equipment.stAxisParam[m_nIndex].Common_Acceleration_Fine = Equipment.ToDouble(textBox_Setup_Motion_Common_FineAcc.Text);
                Equipment.stAxisParam[m_nIndex].Common_Acceleration_Coarse = Equipment.ToDouble(textBox_Setup_Motion_Common_CoarseAcc.Text);
                Equipment.stAxisParam[m_nIndex].Common_Speed_Max = Equipment.ToDouble(textBox_Setup_Motion_Common_MaxSpeed.Text);
                Equipment.stAxisParam[m_nIndex].Common_Speed_Min = Equipment.ToDouble(textBox_Setup_Motion_Common_MinSpeed.Text);
                Equipment.stAxisParam[m_nIndex].Common_Speed_Fine = Equipment.ToDouble(textBox_Setup_Motion_Common_FineSpeed.Text);
                Equipment.stAxisParam[m_nIndex].Common_Speed_Coarse = Equipment.ToDouble(textBox_Setup_Motion_Common_CoarseSpeed.Text);
                Equipment.stAxisParam[m_nIndex].Common_Position_Min = Equipment.ToDouble(textBox_Setup_Motion_Common_MinPos.Text);
                Equipment.stAxisParam[m_nIndex].Common_Position_Max = Equipment.ToDouble(textBox_Setup_Motion_Common_MaxPos.Text);
                Equipment.stAxisParam[m_nIndex].Common_Settle_Delay = Equipment.ToDouble(textBox_Setup_Motion_Common_SettleDelay.Text);

                Equipment.stAxisParam[m_nIndex].Jog_Speed_Fine = Equipment.ToDouble(textBox_Setup_Motion_Jog_FineSpeed.Text);
                Equipment.stAxisParam[m_nIndex].Jog_Speed_Coarse = Equipment.ToDouble(textBox_Setup_Motion_Jog_CoarseSpeed.Text);
                Equipment.stAxisParam[m_nIndex].Jog_StepSize_Min = Equipment.ToDouble(textBox_Setup_Motion_Jog_MinStepSize.Text);
                Equipment.stAxisParam[m_nIndex].Jog_StepSize_Max = Equipment.ToDouble(textBox_Setup_Motion_Jog_MaxStepSize.Text);
                Equipment.stAxisParam[m_nIndex].Jog_StepSize_Fine = Equipment.ToDouble(textBox_Setup_Motion_Jog_FineStepSize.Text);
                Equipment.stAxisParam[m_nIndex].Jog_StepSize_Coarse = Equipment.ToDouble(textBox_Setup_Motion_Jog_CoarseStepSize.Text);
            }


            //  Axis Parameter 저장
            for (int i = 0; i < Equipment.Max_Axis; i++ )
            {
                strTemp = string.Format("Axis_{0}_Limit", i);
                //  Limit Sensor 설치 여부 (Not Installed, Installed)                
                NativeMethods.WritePrivateProfileString(strTemp, "Install", Equipment.stAxisParam[i].LimitSensor_Installed.ToString(), strFIle);
                //  Limit Sensor 동작 레벨 (Low, High)
                NativeMethods.WritePrivateProfileString(strTemp, "ActiveLevel", Equipment.stAxisParam[i].LimitSensor_ActiveLevel.ToString(), strFIle);

                strTemp = string.Format("Axis_{0}_Home", i);
                //  Home Sensor 형태 (Home, -Limit, +Limit)
                NativeMethods.WritePrivateProfileString(strTemp, "SensingType", Equipment.stAxisParam[i].Home_Sensing.ToString(), strFIle);
                //  Home Sensor 설치 여부 (Not Installed, Installed)
                NativeMethods.WritePrivateProfileString(strTemp, "Install", Equipment.stAxisParam[i].Home_Installed.ToString(), strFIle);
                //  Home Sensor 동작 레벨 (Low, High)
                NativeMethods.WritePrivateProfileString(strTemp, "ActiveLevel", Equipment.stAxisParam[i].Home_ActiveLevel.ToString(), strFIle);
                //  Home Sensor 동작 방향 (Negative, Positive)
                NativeMethods.WritePrivateProfileString(strTemp, "Direction", Equipment.stAxisParam[i].Home_Direction.ToString(), strFIle);
                //  Home 1st Speed
                NativeMethods.WritePrivateProfileString(strTemp, "1stSpeed", Equipment.stAxisParam[i].Home_Speed_1st.ToString(), strFIle);
                //  Home 2nd Speed
                NativeMethods.WritePrivateProfileString(strTemp, "2ndSpeed", Equipment.stAxisParam[i].Home_Speed_2nd.ToString(), strFIle);
                //  Home 3rd Speed
                NativeMethods.WritePrivateProfileString(strTemp, "3rdSpeed", Equipment.stAxisParam[i].Home_Speed_3rd.ToString(), strFIle);
                //  Home Last Speed
                NativeMethods.WritePrivateProfileString(strTemp, "LastSpeed", Equipment.stAxisParam[i].Home_Speed_Last.ToString(), strFIle);
                //  Home Clear Time
                NativeMethods.WritePrivateProfileString(strTemp, "ClearTime", Equipment.stAxisParam[i].Home_Clear_Time.ToString(), strFIle);
                //  Home ZPhase Use (Disable, Dir CCW, Dir CW)
                NativeMethods.WritePrivateProfileString(strTemp, "ZPhase", Equipment.stAxisParam[i].Home_ZPhase_Use.ToString(), strFIle);
                //  Home Offset
                NativeMethods.WritePrivateProfileString(strTemp, "Offset", Equipment.stAxisParam[i].Home_Offset.ToString(), strFIle);
                //  Home 1st Acceleration
                NativeMethods.WritePrivateProfileString(strTemp, "1stAccel", Equipment.stAxisParam[i].Home_Acceleration_1st.ToString(), strFIle);
                //  Home 2nd Acceleration
                NativeMethods.WritePrivateProfileString(strTemp, "2ndAccel", Equipment.stAxisParam[i].Home_Acceleration_2nd.ToString(), strFIle);

                strTemp = string.Format("Axis_{0}_Common", i);
                //  Unit Per Pulse (Unit)
                NativeMethods.WritePrivateProfileString(strTemp, "Unit", Equipment.stAxisParam[i].Common_UnitPerPulse_Unit.ToString(), strFIle);
                //  Unit Per Pulse (Pulse)
                NativeMethods.WritePrivateProfileString(strTemp, "Pulse", Equipment.stAxisParam[i].Common_UnitPerPulse_Pulse.ToString(), strFIle);
                //  Acceleration Min
                NativeMethods.WritePrivateProfileString(strTemp, "MinAcc", Equipment.stAxisParam[i].Common_Acceleration_Min.ToString(), strFIle);
                //  Acceleration Max
                NativeMethods.WritePrivateProfileString(strTemp, "MaxAcc", Equipment.stAxisParam[i].Common_Acceleration_Max.ToString(), strFIle);
                //  Acceleration Fine
                NativeMethods.WritePrivateProfileString(strTemp, "FineAcc", Equipment.stAxisParam[i].Common_Acceleration_Fine.ToString(), strFIle);
                //  Acceleration Coarse
                NativeMethods.WritePrivateProfileString(strTemp, "CoarseAcc", Equipment.stAxisParam[i].Common_Acceleration_Coarse.ToString(), strFIle);
                //  Speed Min
                NativeMethods.WritePrivateProfileString(strTemp, "MinSpeed", Equipment.stAxisParam[i].Common_Speed_Min.ToString(), strFIle);
                //  Speed Max
                NativeMethods.WritePrivateProfileString(strTemp, "MaxSpeed", Equipment.stAxisParam[i].Common_Speed_Max.ToString(), strFIle);
                //  Move Speed FIne
                NativeMethods.WritePrivateProfileString(strTemp, "FineSpeed", Equipment.stAxisParam[i].Common_Speed_Fine.ToString(), strFIle);
                //  Move Speed Coarse
                NativeMethods.WritePrivateProfileString(strTemp, "CoarseSpeed", Equipment.stAxisParam[i].Common_Speed_Coarse.ToString(), strFIle);
                //  Position Min
                NativeMethods.WritePrivateProfileString(strTemp, "MinPos", Equipment.stAxisParam[i].Common_Position_Min.ToString(), strFIle);
                //  Position Max
                NativeMethods.WritePrivateProfileString(strTemp, "MaxPos", Equipment.stAxisParam[i].Common_Position_Max.ToString(), strFIle);
                //  Settle Delay Time
                NativeMethods.WritePrivateProfileString(strTemp, "SettleDelay", Equipment.stAxisParam[i].Common_Settle_Delay.ToString(), strFIle);

                strTemp = string.Format("Axis_{0}_Jog", i);
                //  Jog Speed, Fine
                NativeMethods.WritePrivateProfileString(strTemp, "FineSpeed", Equipment.stAxisParam[i].Jog_Speed_Fine.ToString(), strFIle);
                //  Jog Speed, Coarse
                NativeMethods.WritePrivateProfileString(strTemp, "CoarseSpeed", Equipment.stAxisParam[i].Jog_Speed_Coarse.ToString(), strFIle);
                //  Jog StepSize, Min
                NativeMethods.WritePrivateProfileString(strTemp, "MinStepSize", Equipment.stAxisParam[i].Jog_StepSize_Min.ToString(), strFIle);
                //  Jog StepSize, Max
                NativeMethods.WritePrivateProfileString(strTemp, "MaxStepSize", Equipment.stAxisParam[i].Jog_StepSize_Max.ToString(), strFIle);
                //  Jog StepSize, Fine
                NativeMethods.WritePrivateProfileString(strTemp, "FineStepSize", Equipment.stAxisParam[i].Jog_StepSize_Fine.ToString(), strFIle);
                //  Jog StepSize, Coarse
                NativeMethods.WritePrivateProfileString(strTemp, "CoarseStepSize", Equipment.stAxisParam[i].Jog_StepSize_Coarse.ToString(), strFIle);
            }

            MessageBox.Show("Axis Setting 파일을 저장하였습니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_Setup_Motion_Save_Click(object sender, EventArgs e)
        {
            Axis_Parameter_Save();
        }

        private void listBox_Setup_Motion_SelectAxis_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  축 선택에 따른 데이터 표시

            int m_nIndex = listBox_Setup_Motion_SelectAxis.SelectedIndex;

            if (m_nIndex < 0)
            {
                return;
            }

            comboBox_Setup_Motion_Limit_Install.SelectedIndex = Equipment.stAxisParam[m_nIndex].LimitSensor_Installed;
            comboBox_Setup_Motion_Limit_Active.SelectedIndex = Equipment.stAxisParam[m_nIndex].LimitSensor_ActiveLevel;

            comboBox_Setup_Motion_Home_Sensing.SelectedIndex = Equipment.stAxisParam[m_nIndex].Home_Sensing;
            comboBox_Setup_Motion_Home_Install.SelectedIndex = Equipment.stAxisParam[m_nIndex].Home_Installed;
            comboBox_Setup_Motion_Home_Active.SelectedIndex = Equipment.stAxisParam[m_nIndex].Home_ActiveLevel;
            comboBox_Setup_Motion_Home_Direction.SelectedIndex = Equipment.stAxisParam[m_nIndex].Home_Direction;
            textBox_Setup_Motion_Home_Speed_1st.Text = Equipment.stAxisParam[m_nIndex].Home_Speed_1st.ToString();
            textBox_Setup_Motion_Home_Speed_2nd.Text = Equipment.stAxisParam[m_nIndex].Home_Speed_2nd.ToString();
            textBox_Setup_Motion_Home_Speed_3rd.Text = Equipment.stAxisParam[m_nIndex].Home_Speed_3rd.ToString();
            textBox_Setup_Motion_Home_Speed_Last.Text = Equipment.stAxisParam[m_nIndex].Home_Speed_Last.ToString();
            textBox_Setup_Motion_Home_ClearTime.Text = Equipment.stAxisParam[m_nIndex].Home_Clear_Time.ToString();
            comboBox_Setup_Motion_Home_ZPhaseUse.SelectedIndex = Equipment.stAxisParam[m_nIndex].Home_ZPhase_Use;
            textBox_Setup_Motion_Home_Offset.Text = Equipment.stAxisParam[m_nIndex].Home_Offset.ToString();
            textBox_Setup_Motion_Home_Acceleration_1st.Text = Equipment.stAxisParam[m_nIndex].Home_Acceleration_1st.ToString();
            textBox_Setup_Motion_Home_Acceleration_2nd.Text = Equipment.stAxisParam[m_nIndex].Home_Acceleration_2nd.ToString();

            textBox_Setup_Motion_Common_Unit.Text = Equipment.stAxisParam[m_nIndex].Common_UnitPerPulse_Unit.ToString();
            textBox_Setup_Motion_Common_Pulse.Text = Equipment.stAxisParam[m_nIndex].Common_UnitPerPulse_Pulse.ToString();
            textBox_Setup_Motion_Common_MinAcc.Text = Equipment.stAxisParam[m_nIndex].Common_Acceleration_Min.ToString();
            textBox_Setup_Motion_Common_MaxAcc.Text = Equipment.stAxisParam[m_nIndex].Common_Acceleration_Max.ToString();
            textBox_Setup_Motion_Common_FineAcc.Text = Equipment.stAxisParam[m_nIndex].Common_Acceleration_Fine.ToString();
            textBox_Setup_Motion_Common_CoarseAcc.Text = Equipment.stAxisParam[m_nIndex].Common_Acceleration_Coarse.ToString();
            textBox_Setup_Motion_Common_MaxSpeed.Text = Equipment.stAxisParam[m_nIndex].Common_Speed_Max.ToString();
            textBox_Setup_Motion_Common_MinSpeed.Text = Equipment.stAxisParam[m_nIndex].Common_Speed_Min.ToString();
            textBox_Setup_Motion_Common_FineSpeed.Text = Equipment.stAxisParam[m_nIndex].Common_Speed_Fine.ToString();
            textBox_Setup_Motion_Common_CoarseSpeed.Text = Equipment.stAxisParam[m_nIndex].Common_Speed_Coarse.ToString();
            textBox_Setup_Motion_Common_MinPos.Text = Equipment.stAxisParam[m_nIndex].Common_Position_Min.ToString();
            textBox_Setup_Motion_Common_MaxPos.Text = Equipment.stAxisParam[m_nIndex].Common_Position_Max.ToString();
            textBox_Setup_Motion_Common_SettleDelay.Text = Equipment.stAxisParam[m_nIndex].Common_Settle_Delay.ToString();

            textBox_Setup_Motion_Jog_FineSpeed.Text = Equipment.stAxisParam[m_nIndex].Jog_Speed_Fine.ToString();
            textBox_Setup_Motion_Jog_CoarseSpeed.Text = Equipment.stAxisParam[m_nIndex].Jog_Speed_Coarse.ToString();
            textBox_Setup_Motion_Jog_MinStepSize.Text = Equipment.stAxisParam[m_nIndex].Jog_StepSize_Min.ToString();
            textBox_Setup_Motion_Jog_MaxStepSize.Text = Equipment.stAxisParam[m_nIndex].Jog_StepSize_Max.ToString();
            textBox_Setup_Motion_Jog_FineStepSize.Text = Equipment.stAxisParam[m_nIndex].Jog_StepSize_Fine.ToString();
            textBox_Setup_Motion_Jog_CoarseStepSize.Text = Equipment.stAxisParam[m_nIndex].Jog_StepSize_Coarse.ToString();
        }

        private void comboBox_Setup_Communication_TCPIP_OpenType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int m_nIndex = comboBox_Setup_Communication_TCPIP_OpenType.SelectedIndex;

            if (m_nIndex == 0)              //  Server
            {
                label_Setup_Communication_TCPIP_IP.Text = "Host IP :";
            }
            else if (m_nIndex == 1)         //  Client
            {
                label_Setup_Communication_TCPIP_IP.Text = "Remote IP :";
            }
        }

        private void button_Setup_Comm_Save_Click(object sender, EventArgs e)
        {
            Comm_Parameter_Save();
        }

        public bool Comm_Parameter_Apply()
        {
            string strTemp = "";

            bool m_bRet = true;
            string strFIle = "";
            StringBuilder temp = new StringBuilder(255);

            strFIle = ConfigManager.GetConfigPath() + "\\Comm Setting (Do not delete or modify).ini";

            if (File.Exists(strFIle) == false)
            {
                MessageBox.Show("Comm. Setting 파일이 없습니다.\r\n\r\n[Default 값으로 설정됩니다.]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return false;
            }

            //  통신 모듈 파라미터 파일을 로드 하면 맨 첫번째 축 데이터를 표시하도록 한다.
            listBox_Setup_Communication_SelectUnit.SelectedIndex = 0;

            tabControl_Setup_Communication_Type.SelectedIndex = Equipment.stCommunicationSet[0].Comm_Type;

            if (Equipment.stCommunicationSet[0].Comm_Type == 0)                      //  TCP/IP
            {
                radioButton_Setup_Communication_Comm_TCPIP.Checked = true;

                comboBox_Setup_Communication_TCPIP_OpenType.SelectedIndex = Equipment.stCommunicationSet[0].TCPIP_PortType;
                textBox_Setup_Communication_TCPIP_IP.Text = Equipment.stCommunicationSet[0].TCPIP_IPAddress;
                textBox_Setup_Communication_TCPIP_Port.Text = Equipment.stCommunicationSet[0].TCPIP_PortNum.ToString();
            }
            else                                                                            //  RS232
            {
                radioButton_Setup_Communication_Comm_RS232.Checked = true;

                textBox_Setup_Communication_RS232_Timeout.Text = Equipment.stCommunicationSet[0].Serial_CommTimeout.ToString();
                textBox_Setup_Communication_RS232_SpacingDelay.Text = Equipment.stCommunicationSet[0].Serial_CommSpacingDelay.ToString();

                comboBox_Setup_Communication_RS232_ComPort.SelectedIndex = Equipment.stCommunicationSet[0].Serial_CommPort;
                comboBox_Setup_Communication_RS232_BaudRate.SelectedIndex = Equipment.stCommunicationSet[0].Serial_CommBaudRate;
                comboBox_Setup_Communication_RS232_DataBit.SelectedIndex = Equipment.stCommunicationSet[0].Serial_CommDataBits;
                comboBox_Setup_Communication_RS232_StopBit.SelectedIndex = Equipment.stCommunicationSet[0].Serial_CommStopBits;
                comboBox_Setup_Communication_RS232_Parity.SelectedIndex = Equipment.stCommunicationSet[0].Serial_CommParity;
                comboBox_Setup_Communication_RS232_FlowControl.SelectedIndex = Equipment.stCommunicationSet[0].Serial_CommFlowControl;
            }

            if (Equipment.stCommunicationSet[0].Connect)
            {
                radioButton_Setup_Communication_Comm_Connect.Checked = true;
            }
            else
            {
                radioButton_Setup_Communication_Comm_Disconnect.Checked = true;
            }

            return m_bRet;
        }

        public bool Machine_Option_Apply()
        {
            string strTemp = "";

            bool m_bRet = true;
            string strFIle = "";
            StringBuilder temp = new StringBuilder(255);

            strFIle = ConfigManager.GetConfigPath() + "\\Machine Option (Do not delete or modify).ini";

            if (File.Exists(strFIle) == false)
            {
                MessageBox.Show("Machine Option 파일이 없습니다.\r\n\r\n[Default 값으로 설정됩니다.]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return false;
            }


            //  Machine Name
            textBox_ModelName.Text = Equipment.Machine_Name;


            //  Laser Type                        
            if (Equipment.Machine_LaserType_CO2)                                            //  CO₂Laser
            {
                radioButton_Setup_Option_LaserType_CO2.Checked = true;
            }
            else                                                                            //  UV Laser
            {
                radioButton_Setup_Option_LaserType_UV.Checked = true;
            }


            //  Offset Distance
            textBox_Setup_Option_Offset_ScannerFineCam_X.Text = Equipment.stOffsetDistance.FromScannerToFineCam.X.ToString();
            textBox_Setup_Option_Offset_ScannerFineCam_Y.Text = Equipment.stOffsetDistance.FromScannerToFineCam.Y.ToString();
            textBox_Setup_Option_Offset_FineCamCoarseCam_X.Text = Equipment.stOffsetDistance.FromFineCamToCoarseCam.X.ToString();
            textBox_Setup_Option_Offset_FineCamCoarseCam_Y.Text = Equipment.stOffsetDistance.FromFineCamToCoarseCam.Y.ToString();
            textBox_Setup_Option_Offset_FineCamLaserHeightSensor_X.Text = Equipment.stOffsetDistance.FromFineCamToLaserHeightSensor.X.ToString();
            textBox_Setup_Option_Offset_FineCamLaserHeightSensor_Y.Text = Equipment.stOffsetDistance.FromFineCamToLaserHeightSensor.Y.ToString();


            //  Scanner Head Offset
            textBox_ScannerOffset_X.Text = Equipment.Scanner_HeadOffset_X.ToString();
            textBox_ScannerOffset_Y.Text = Equipment.Scanner_HeadOffset_Y.ToString();
            textBox_ScannerOffset_Angle.Text = Equipment.Scanner_HeadOffset_Angle.ToString();


            //  Machine Coordinate Offset
            textBox_Setup_Option_MachineOffset_StageOriginPosToScannerCenter_X.Text = Equipment.CoordinateMatchingOffset_X.ToString();
            textBox_Setup_Option_MachineOffset_StageOriginPosToScannerCenter_Y.Text = Equipment.CoordinateMatchingOffset_Y.ToString();


            //  Offset Distance
            textBox_Setup_Option_OffsetDistance_StageCenterToScannerCenter_X.Text = Equipment.StageOffset_forDrilling_X.ToString();
            textBox_Setup_Option_OffsetDistance_StageCenterToScannerCenter_Y.Text = Equipment.StageOffset_forDrilling_Y.ToString();


            //  Keyence Laser Height Sensor 기준값 설정
            textBox_Setup_Option_ReferenceValue_atVisionFocusPosition.Text = Equipment.LaserHeightSensor_ReferenceValue_atVisionFocusPosition.ToString();
            textBox_Setup_Option_ReferenceValue_atScannerFocusPosition.Text = Equipment.LaserHeightSensor_ReferenceValue_atScannerFocusPosition.ToString();


            //  집진기 동작 후 안정화 시간
            textBox_Setup_Option_DustCollector_WaitingTime.Text = Equipment.DustCollector_TurnOn_AfterStableTime.ToString();


            //  저장 폴더
            richTextBox_Recipe_TabRecipe_RecipeFileFolder.Text = Equipment.RecipeFilePath.ToString();
            richTextBox_Recipe_TabRecipe_DrawingFileFolder.Text = Equipment.DrawingFilePath.ToString();


            //  도면 렌더링 분해능
            textBox_Setup_Option_Sirius_Drawing_Resolution.Text = Equipment.SiriusDrawing_Rendering_Resolution.ToString();


            //  Options
            checkBox_Setup_Option_DoorEnable.Checked = Equipment.Machine_Door_Enable;
            checkBox_Setup_Option_VacuumSensorEnable.Checked = Equipment.Machine_VacuumSensor_Enable;
            textBox_Setup_Option_SignalHoldTime.Text = Equipment.Machine_SignalHoldTime.ToString();
            checkBox_Setup_Option_MAligner_ReleaseType.Checked = Equipment.Machine_MAligner_ReleaseType;
            textBox_Setup_Option_MAligner_WidenDistance.Text = Equipment.Machine_MAligner_WidenDistance.ToString();
            textBox_Setup_Option_MAligner_NarrowingDistance.Text = Equipment.Machine_MAligner_NarrowingDistance.ToString();
            checkBox_Setup_Option_VacuumStableTime_Enable.Checked = Equipment.Machine_VacuumStableTime_Enable;
            textBox_Setup_Option_VacuumStableTime.Text = Equipment.Machine_VacuumStableTime.ToString();
            checkBox_Setup_Option_LaserHeightCheckStableTime_Enable.Checked = Equipment.Machine_LaserHeightCheckStableTime_Enable;
            textBox_Setup_Option_LaserHeightCheckStableTime.Text = Equipment.Machine_LaserHeightCheckStableTime.ToString();
            checkBox_Setup_Option_FiducialMarkJudgementRange_Enable.Checked = Equipment.Machine_FiducialMarkJudgementRange_Enable;
            textBox_Setup_Option_FiducialMarkJudgementRange.Text = Equipment.Machine_FiducialMarkJudgementRange.ToString();
            checkBox_Setup_Option_VacuumBlowTime_Enable.Checked = Equipment.Machine_VacuumBlowTime_Enable;
            textBox_Setup_Option_VacuumBlowTime.Text = Equipment.Machine_VacuumBlowTime.ToString();

            if (Equipment.Machine_FiducialImageSave_Always)
            {
                radioButton_Setup_Option_FiducialImageSave_Always.Checked = true;
            }
            else
            {
                radioButton_Setup_Option_FiducialImageSave_FailedToFind.Checked = true;
            }

            return m_bRet;
        }

        public bool Flatness_Measurement_Position_Apply()
        {
            string strTemp = "";

            bool m_bRet = true;
            string strFIle = "";
            StringBuilder temp = new StringBuilder(255);

            strFIle = ConfigManager.GetTeachingDataPath() + "\\FlatMeasure_TeachingPosition.ini";

            if (File.Exists(strFIle) == false)
            {
                MessageBox.Show("FlatMeasure Teaching Position 파일이 없습니다.\r\n\r\n[Default 값으로 설정됩니다.]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return false;
            }

            //  첫번째 Index 로 표시
            comboBox_Setup_FlatnessMeasurementPos_List.SelectedIndex = 0;

            //  Flatness 측정 위치 데이터 적용 (0번 Index)
            textBox_Setup_Flatness_TeachingPos1_StageX.Text = Equipment.stFlatMeasurePos[0].StagePos[0].X.ToString();
            textBox_Setup_Flatness_TeachingPos1_StageY.Text = Equipment.stFlatMeasurePos[0].StagePos[0].Y.ToString();
            textBox_Setup_Flatness_TeachingPos2_StageX.Text = Equipment.stFlatMeasurePos[0].StagePos[1].X.ToString();
            textBox_Setup_Flatness_TeachingPos2_StageY.Text = Equipment.stFlatMeasurePos[0].StagePos[1].Y.ToString();
            textBox_Setup_Flatness_TeachingPos3_StageX.Text = Equipment.stFlatMeasurePos[0].StagePos[2].X.ToString();
            textBox_Setup_Flatness_TeachingPos3_StageY.Text = Equipment.stFlatMeasurePos[0].StagePos[2].Y.ToString();
            textBox_Setup_Flatness_TeachingPos4_StageX.Text = Equipment.stFlatMeasurePos[0].StagePos[3].X.ToString();
            textBox_Setup_Flatness_TeachingPos4_StageY.Text = Equipment.stFlatMeasurePos[0].StagePos[3].Y.ToString();
            textBox_Setup_Flatness_TeachingPos5_StageX.Text = Equipment.stFlatMeasurePos[0].StagePos[4].X.ToString();
            textBox_Setup_Flatness_TeachingPos5_StageY.Text = Equipment.stFlatMeasurePos[0].StagePos[4].Y.ToString();
            textBox_Setup_Flatness_TeachingPos6_StageX.Text = Equipment.stFlatMeasurePos[0].StagePos[5].X.ToString();
            textBox_Setup_Flatness_TeachingPos6_StageY.Text = Equipment.stFlatMeasurePos[0].StagePos[5].Y.ToString();
            textBox_Setup_Flatness_TeachingPos7_StageX.Text = Equipment.stFlatMeasurePos[0].StagePos[6].X.ToString();
            textBox_Setup_Flatness_TeachingPos7_StageY.Text = Equipment.stFlatMeasurePos[0].StagePos[6].Y.ToString();
            textBox_Setup_Flatness_TeachingPos8_StageX.Text = Equipment.stFlatMeasurePos[0].StagePos[7].X.ToString();
            textBox_Setup_Flatness_TeachingPos8_StageY.Text = Equipment.stFlatMeasurePos[0].StagePos[7].Y.ToString();
            textBox_Setup_Flatness_TeachingPos9_StageX.Text = Equipment.stFlatMeasurePos[0].StagePos[8].X.ToString();
            textBox_Setup_Flatness_TeachingPos9_StageY.Text = Equipment.stFlatMeasurePos[0].StagePos[8].Y.ToString();

            return m_bRet;
        }

        public void MappingData_List_Apply()
        {
            string strTemp = "";

            bool m_bRet = true;
            string strFIle = "";
            StringBuilder temp = new StringBuilder(255);

            strFIle = ConfigManager.GetConfigPath() + "\\Mapping File List (Do not delete or modify).ini";

            if (File.Exists(strFIle) == false)
            {
                MessageBox.Show("Mapping Data 리스트 파일이 없습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //  Stage Center 가 Scanner 위치에 있을 경우
                Equipment.MappingData_FilePath_Stage_Scanner = "";

                //  Stage Center 가 Fine Camera 위치에 있을 경우
                Equipment.MappingData_FilePath_Stage_FineCam = "";

                //  Stage Calibration 위치가 Scanner 위치에 있을 경우
                Equipment.MappingData_FilePath_StageCal_Scanner = "";

                //  Stage Calibration 위치가 Fine Camera 위치에 있을 경우
                Equipment.MappingData_FilePath_StageCal_FineCam = "";

                return ;
            }


            //  Stage Center 가 Scanner 위치에 있을 경우
            richTextBox_Setup_Tab2DMap_StageCenter_ScannerPosition.Text = Equipment.MappingData_FilePath_Stage_Scanner;

            //  Stage Center 가 Fine Camera 위치에 있을 경우
            richTextBox_Setup_Tab2DMap_StageCenter_FineCameraPosition.Text = Equipment.MappingData_FilePath_Stage_FineCam;

            //  Stage Calibration 위치가 Scanner 위치에 있을 경우
            richTextBox_Setup_Tab2DMap_StageCalPos_ScannerPosition.Text = Equipment.MappingData_FilePath_StageCal_Scanner;

            //  Stage Calibration 위치가 Fine Camera 위치에 있을 경우
            richTextBox_Setup_Tab2DMap_StageCalPos_FineCameraPosition.Text = Equipment.MappingData_FilePath_StageCal_FineCam;
        }


        public void Comm_Parameter_Save()
        {
            string strTemp = "";

            string strFIle = "";
            strFIle = ConfigManager.GetConfigPath() + "\\Comm Setting (Do not delete or modify).ini";

            if (File.Exists(strFIle) == false)
            {
                File.Create(strFIle);
                //return;

                MessageBox.Show("Comm. Setting 파일을 생성하였습니다. 다시 시도하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //  선택된 Comm. Unit 에 대한 데이터 갖다 넣기
            int m_nIndex = listBox_Setup_Communication_SelectUnit.SelectedIndex;

            if (m_nIndex >= 0)
            {
                Equipment.stCommunicationSet[m_nIndex].Comm_Type = radioButton_Setup_Communication_Comm_TCPIP.Checked ? 0 : 1;

                Equipment.stCommunicationSet[m_nIndex].Connect = radioButton_Setup_Communication_Comm_Connect.Checked ? true : false;

                Equipment.stCommunicationSet[m_nIndex].TCPIP_PortType = comboBox_Setup_Communication_TCPIP_OpenType.SelectedIndex;
                Equipment.stCommunicationSet[m_nIndex].TCPIP_IPAddress = textBox_Setup_Communication_TCPIP_IP.Text;
                Equipment.stCommunicationSet[m_nIndex].TCPIP_PortNum = Equipment.ToInt(textBox_Setup_Communication_TCPIP_Port.Text);

                Equipment.stCommunicationSet[m_nIndex].Serial_CommTimeout = Equipment.ToInt(textBox_Setup_Communication_RS232_Timeout.Text); 
                Equipment.stCommunicationSet[m_nIndex].Serial_CommSpacingDelay = Equipment.ToInt(textBox_Setup_Communication_RS232_SpacingDelay.Text);
                Equipment.stCommunicationSet[m_nIndex].Serial_CommPort = comboBox_Setup_Communication_RS232_ComPort.SelectedIndex;
                Equipment.stCommunicationSet[m_nIndex].Serial_CommBaudRate = comboBox_Setup_Communication_RS232_BaudRate.SelectedIndex;
                Equipment.stCommunicationSet[m_nIndex].Serial_CommDataBits = comboBox_Setup_Communication_RS232_DataBit.SelectedIndex;
                Equipment.stCommunicationSet[m_nIndex].Serial_CommStopBits = comboBox_Setup_Communication_RS232_StopBit.SelectedIndex;
                Equipment.stCommunicationSet[m_nIndex].Serial_CommParity = comboBox_Setup_Communication_RS232_Parity.SelectedIndex;
                Equipment.stCommunicationSet[m_nIndex].Serial_CommFlowControl = comboBox_Setup_Communication_RS232_FlowControl.SelectedIndex;
            }


            //  Comm. Parameter 저장
            for (int i = 0; i < System.Enum.GetValues(typeof(CommList)).Length; i++)
            {
                strTemp = string.Format("CommUnit_{0}", i);


                //  TCP/IP, RS232                                                                                   //  0 : TCP/IP,     1 : RS232
                NativeMethods.WritePrivateProfileString(strTemp, "CommType", Equipment.stCommunicationSet[i].Comm_Type.ToString(), strFIle);


                //  Not Connect, Connect                                                                            //  0 : Not Connect,    1 : Connect
                NativeMethods.WritePrivateProfileString(strTemp, "ConnectType", Equipment.stCommunicationSet[i].Connect.ToString(), strFIle);


                //  TCP/IP 의 포트 형식 (Server, Client)                                                            //  0 : Server,     1 : Client
                NativeMethods.WritePrivateProfileString(strTemp, "TCPIP_PortType", Equipment.stCommunicationSet[i].TCPIP_PortType.ToString(), strFIle);
                //  TCP/IP 의 IP 주소
                NativeMethods.WritePrivateProfileString(strTemp, "TCPIP_IPAddress", Equipment.stCommunicationSet[i].TCPIP_IPAddress.ToString(), strFIle);
                //  TCP/IP 의 Port 번호
                NativeMethods.WritePrivateProfileString(strTemp, "TCPIP_PortNum", Equipment.stCommunicationSet[i].TCPIP_PortNum.ToString(), strFIle);


                //  Timeout (ms)
                NativeMethods.WritePrivateProfileString(strTemp, "RS232_Timeout", Equipment.stCommunicationSet[i].Serial_CommTimeout.ToString(), strFIle);
                //  Spacing Delay (ms)
                NativeMethods.WritePrivateProfileString(strTemp, "RS232_SpacingDelay", Equipment.stCommunicationSet[i].Serial_CommSpacingDelay.ToString(), strFIle);
                //  COM Port                                                                                        //  0 : COM1,       1 : COM2,       2 : COM3,       3 : COM4 ....
                NativeMethods.WritePrivateProfileString(strTemp, "RS232_Port", Equipment.stCommunicationSet[i].Serial_CommPort.ToString(), strFIle);
                //  Baud Rate                                                                                       //  0 : 9600,       1 : 19200,      2 : 38400,      3 : 57600,      4 : 115200
                NativeMethods.WritePrivateProfileString(strTemp, "RS232_BaudRate", Equipment.stCommunicationSet[i].Serial_CommBaudRate.ToString(), strFIle);
                //  Data Bits                                                                                       //  0 : 5,          1 : 6,          2 : 7,          3 : 8
                NativeMethods.WritePrivateProfileString(strTemp, "RS232_DataBit", Equipment.stCommunicationSet[i].Serial_CommDataBits.ToString(), strFIle);
                //  Stop Bits                                                                                       //  0 : 1,          1 : 1.5,        2 : 2
                NativeMethods.WritePrivateProfileString(strTemp, "RS232_StopBit", Equipment.stCommunicationSet[i].Serial_CommStopBits.ToString(), strFIle);
                //  Parity                                                                                          //  0 : None,       1 : Odd,        2 : Even
                NativeMethods.WritePrivateProfileString(strTemp, "RS232_Parity", Equipment.stCommunicationSet[i].Serial_CommParity.ToString(), strFIle);
                //  Flow Control                                                                                    //  0 : None,       1 : Xon/Xoff,   2 : RTS/CTS
                NativeMethods.WritePrivateProfileString(strTemp, "RS232_FlowControl", Equipment.stCommunicationSet[i].Serial_CommFlowControl.ToString(), strFIle);
            }

            MessageBox.Show("Comm. Setting 파일을 저장하였습니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void radioButton_Setup_Communication_Comm_TCPIP_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Setup_Communication_Comm_TCPIP.Checked)
            {
                tabControl_Setup_Communication_Type.SelectedIndex = 0;              //  TCP/IP
            }
            else
            {
                tabControl_Setup_Communication_Type.SelectedIndex = 1;              //  RS232
            }
        }

        private void listBox_Setup_Communication_SelectUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  통신 모듈 선택에 따른 데이터 표시

            int m_nIndex = listBox_Setup_Communication_SelectUnit.SelectedIndex;

            if (m_nIndex < 0)
            {
                return;
            }


            tabControl_Setup_Communication_Type.SelectedIndex = Equipment.stCommunicationSet[m_nIndex].Comm_Type;

            if (Equipment.stCommunicationSet[m_nIndex].Comm_Type == 0)                      //  TCP/IP
            {
                radioButton_Setup_Communication_Comm_TCPIP.Checked = true;

                comboBox_Setup_Communication_TCPIP_OpenType.SelectedIndex = Equipment.stCommunicationSet[m_nIndex].TCPIP_PortType;
                textBox_Setup_Communication_TCPIP_IP.Text = Equipment.stCommunicationSet[m_nIndex].TCPIP_IPAddress;
                textBox_Setup_Communication_TCPIP_Port.Text = Equipment.stCommunicationSet[m_nIndex].TCPIP_PortNum.ToString();
            }
            else                                                                            //  RS232
            {
                radioButton_Setup_Communication_Comm_RS232.Checked = true;

                textBox_Setup_Communication_RS232_Timeout.Text = Equipment.stCommunicationSet[m_nIndex].Serial_CommTimeout.ToString();
                textBox_Setup_Communication_RS232_SpacingDelay.Text = Equipment.stCommunicationSet[m_nIndex].Serial_CommSpacingDelay.ToString();

                comboBox_Setup_Communication_RS232_ComPort.SelectedIndex = Equipment.stCommunicationSet[m_nIndex].Serial_CommPort;
                comboBox_Setup_Communication_RS232_BaudRate.SelectedIndex = Equipment.stCommunicationSet[m_nIndex].Serial_CommBaudRate;
                comboBox_Setup_Communication_RS232_DataBit.SelectedIndex = Equipment.stCommunicationSet[m_nIndex].Serial_CommDataBits;
                comboBox_Setup_Communication_RS232_StopBit.SelectedIndex = Equipment.stCommunicationSet[m_nIndex].Serial_CommStopBits;
                comboBox_Setup_Communication_RS232_Parity.SelectedIndex = Equipment.stCommunicationSet[m_nIndex].Serial_CommParity;
                comboBox_Setup_Communication_RS232_FlowControl.SelectedIndex = Equipment.stCommunicationSet[m_nIndex].Serial_CommFlowControl;
            }

            if (Equipment.stCommunicationSet[m_nIndex].Connect)
            {
                radioButton_Setup_Communication_Comm_Connect.Checked = true;
            }
            else
            {
                radioButton_Setup_Communication_Comm_Disconnect.Checked = true;
            }
        }

        private void button_Test_SocketConnect_Click(object sender, EventArgs e)
        {
            workStage.m_SocketLaser.Close();
        }

        private void button_Setup_Comm_ShowCommTerminal_Click(object sender, EventArgs e)
        {
            int m_nIndex = listBox_Setup_Communication_SelectUnit.SelectedIndex;

            if (m_nIndex < 0)
            {
                return;
            }

            //  선택된 Comm. Terminal 창을 띄운다.

            if (m_formCommTerminal == null)
            {
                m_formCommTerminal.CreateCommTerminal();
            }

            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm.Name == m_formCommTerminal.Name)
                {
                    m_formCommTerminal.m_nSelectedUnitIndex = m_nIndex;

                    if (Equipment.stCommunicationSet[m_nIndex].Comm_Type == 0)                      //  TCP/IP
                    {
                        m_formCommTerminal.m_strSocketIP = Equipment.stCommunicationSet[m_nIndex].TCPIP_IPAddress;
                        m_formCommTerminal.m_nSocketPort = Equipment.stCommunicationSet[m_nIndex].TCPIP_PortNum;
                    }
                    else                                                                            //  RS232
                    {
                        m_formCommTerminal.m_nCommPort = Equipment.stCommunicationSet[m_nIndex].Serial_CommPort;
                    }

                    m_formCommTerminal.FormNew_CommunicationTerminal_TabRefresh();
                    openForm.BringToFront();
                    openForm.Show();
                    return;
                }
            }

            m_formCommTerminal.m_nSelectedUnitIndex = m_nIndex;

            if (Equipment.stCommunicationSet[m_nIndex].Comm_Type == 0)                      //  TCP/IP
            {
                m_formCommTerminal.m_strSocketIP = Equipment.stCommunicationSet[m_nIndex].TCPIP_IPAddress;
                m_formCommTerminal.m_nSocketPort = Equipment.stCommunicationSet[m_nIndex].TCPIP_PortNum;
            }
            else                                                                            //  RS232
            {
                m_formCommTerminal.m_nCommPort = Equipment.stCommunicationSet[m_nIndex].Serial_CommPort;
            }

            m_formCommTerminal.Show();
        }

        private void button_Setup_Option_Save_Click(object sender, EventArgs e)
        {
            //  현재 Laser Type 을 저장한다.
            bool m_bCurrentLaserType = false;
            string strTemp = "";

            bool m_bRet = true;
            string strFIle = "";
            StringBuilder temp = new StringBuilder(255);


            //  체크 포인트
            if (((Equipment.ToDouble(textBox_Setup_Option_MachineOffset_StageOriginPosToScannerCenter_X.Text) != 0.0) || (Equipment.ToDouble(textBox_Setup_Option_MachineOffset_StageOriginPosToScannerCenter_Y.Text) != 0.0)) &&
                ((Equipment.ToDouble(textBox_Setup_Option_OffsetDistance_StageCenterToScannerCenter_X.Text) != 0.0) || (Equipment.ToDouble(textBox_Setup_Option_OffsetDistance_StageCenterToScannerCenter_Y.Text) != 0.0)))
            {
                MessageBox.Show("\"Offset Distance for Coordinate Matching\" 과\r\n\"Offset Distance to the Center of the Scanner\" 두 그룹 전체에 값이 들어가면 안됩니다.\n\r\n[두 그룹 중 한쪽에만 값이 들어가거나, 모두 0 이어야 합니다.]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            strFIle = ConfigManager.GetConfigPath() + "\\Machine Option (Do not delete or modify).ini";

            if (File.Exists(strFIle) == false)
            {
                MessageBox.Show("Machine Option 파일이 없습니다.\r\n\r\n[Default 값(CO₂)으로 설정됩니다.]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return false;
            }


            //  Machine Option  로드

            //  Laser Type                                                                            //  True : CO₂,    False : UV
            NativeMethods.GetPrivateProfileString("Machine_Option", "Laser_Type", "True", temp, 255, strFIle);
            m_bCurrentLaserType = temp.ToString() == "False" ? false : true;

            //  현재 선택한 Laser Type 을 저장하고,
            Machine_Option_Save();

            //  현재 선택한 Laser Type 과 비교한 후, 다르면 프로그램을 재시작 하도록 한다.
            if (m_bCurrentLaserType != radioButton_Setup_Option_LaserType_CO2.Checked)
            {
                MessageBox.Show("Machine Option 을 변경하였습니다.\r\n\r\n프로그램을 재시작해야 합니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void Machine_Option_Save()
        {
            string strTemp = "";

            string strFIle = "";
            strFIle = ConfigManager.GetConfigPath() + "\\Machine Option (Do not delete or modify).ini";

            if (File.Exists(strFIle) == false)
            {
                File.Create(strFIle);
                //return;

                MessageBox.Show("Machine Option 파일을 생성하였습니다. 다시 시도하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //  Machine Name
            Equipment.Machine_Name = textBox_ModelName.Text;
            NativeMethods.WritePrivateProfileString("Machine_Option", "Machine_Name", textBox_ModelName.Text, strFIle);

            //  Laser Type
            Equipment.Machine_LaserType_CO2 = radioButton_Setup_Option_LaserType_CO2.Checked;
            NativeMethods.WritePrivateProfileString("Machine_Option", "Laser_Type", radioButton_Setup_Option_LaserType_CO2.Checked.ToString(), strFIle);

            //  Options
            Equipment.Machine_Door_Enable = checkBox_Setup_Option_DoorEnable.Checked;
            NativeMethods.WritePrivateProfileString("Machine_Option", "Door_Enable", checkBox_Setup_Option_DoorEnable.Checked.ToString(), strFIle);
            Equipment.Machine_VacuumSensor_Enable = checkBox_Setup_Option_VacuumSensorEnable.Checked;
            NativeMethods.WritePrivateProfileString("Machine_Option", "VacuumSensor_Enable", checkBox_Setup_Option_VacuumSensorEnable.Checked.ToString(), strFIle);
            Equipment.Machine_SignalHoldTime = Equipment.ToInt(textBox_Setup_Option_SignalHoldTime.Text);
            NativeMethods.WritePrivateProfileString("Machine_Option", "VacuumSignalHoldTime", textBox_Setup_Option_SignalHoldTime.Text, strFIle);
            Equipment.Machine_MAligner_ReleaseType = checkBox_Setup_Option_MAligner_ReleaseType.Checked;
            NativeMethods.WritePrivateProfileString("Machine_Option", "MAligner_ReleaseType", checkBox_Setup_Option_MAligner_ReleaseType.Checked.ToString(), strFIle);
            Equipment.Machine_MAligner_NarrowingDistance = Equipment.ToDouble(textBox_Setup_Option_MAligner_NarrowingDistance.Text);
            NativeMethods.WritePrivateProfileString("Machine_Option", "MAligner_NarrowingDistance", textBox_Setup_Option_MAligner_NarrowingDistance.Text, strFIle);
            Equipment.Machine_MAligner_WidenDistance = Equipment.ToDouble(textBox_Setup_Option_MAligner_WidenDistance.Text);
            NativeMethods.WritePrivateProfileString("Machine_Option", "MAligner_WidenDistance", textBox_Setup_Option_MAligner_WidenDistance.Text, strFIle);
            Equipment.Machine_VacuumStableTime_Enable = checkBox_Setup_Option_VacuumStableTime_Enable.Checked;
            NativeMethods.WritePrivateProfileString("Machine_Option", "VacuumStableTime_Enable", checkBox_Setup_Option_VacuumStableTime_Enable.Checked.ToString(), strFIle);
            Equipment.Machine_VacuumStableTime = Equipment.ToInt(textBox_Setup_Option_VacuumStableTime.Text);
            NativeMethods.WritePrivateProfileString("Machine_Option", "VacuumStableTime", textBox_Setup_Option_VacuumStableTime.Text, strFIle);
            Equipment.Machine_LaserHeightCheckStableTime_Enable = checkBox_Setup_Option_LaserHeightCheckStableTime_Enable.Checked;
            NativeMethods.WritePrivateProfileString("Machine_Option", "LaserHeightCheckStableTime_Enable", checkBox_Setup_Option_LaserHeightCheckStableTime_Enable.Checked.ToString(), strFIle);
            Equipment.Machine_LaserHeightCheckStableTime = Equipment.ToInt(textBox_Setup_Option_LaserHeightCheckStableTime.Text);
            NativeMethods.WritePrivateProfileString("Machine_Option", "LaserHeightCheckStableTime", textBox_Setup_Option_LaserHeightCheckStableTime.Text, strFIle);
            Equipment.Machine_FiducialMarkJudgementRange_Enable = checkBox_Setup_Option_FiducialMarkJudgementRange_Enable.Checked;
            NativeMethods.WritePrivateProfileString("Machine_Option", "FiducialMarkJudgementRange_Enable", checkBox_Setup_Option_FiducialMarkJudgementRange_Enable.Checked.ToString(), strFIle);
            Equipment.Machine_FiducialMarkJudgementRange = Equipment.ToDouble(textBox_Setup_Option_FiducialMarkJudgementRange.Text);
            NativeMethods.WritePrivateProfileString("Machine_Option", "FiducialMarkJudgementRange", textBox_Setup_Option_FiducialMarkJudgementRange.Text, strFIle);
            Equipment.Machine_FiducialImageSave_Always = radioButton_Setup_Option_FiducialImageSave_Always.Checked ? true : false;
            NativeMethods.WritePrivateProfileString("Machine_Option", "FiducialImageSave_Always", radioButton_Setup_Option_FiducialImageSave_Always.Checked.ToString(), strFIle);
            Equipment.Machine_VacuumBlowTime_Enable = checkBox_Setup_Option_VacuumBlowTime_Enable.Checked;
            NativeMethods.WritePrivateProfileString("Machine_Option", "VacuumBlowTime_Enable", checkBox_Setup_Option_VacuumBlowTime_Enable.Checked.ToString(), strFIle);
            Equipment.Machine_VacuumBlowTime = Equipment.ToInt(textBox_Setup_Option_VacuumBlowTime.Text);
            NativeMethods.WritePrivateProfileString("Machine_Option", "VacuumBlowTime", textBox_Setup_Option_VacuumBlowTime.Text, strFIle);

            //  Offset Distance
            Equipment.stOffsetDistance.FromScannerToFineCam.X = Equipment.ToDouble(textBox_Setup_Option_Offset_ScannerFineCam_X.Text);
            Equipment.stOffsetDistance.FromScannerToFineCam.Y = Equipment.ToDouble(textBox_Setup_Option_Offset_ScannerFineCam_Y.Text);
            Equipment.stOffsetDistance.FromFineCamToCoarseCam.X = Equipment.ToDouble(textBox_Setup_Option_Offset_FineCamCoarseCam_X.Text);
            Equipment.stOffsetDistance.FromFineCamToCoarseCam.Y = Equipment.ToDouble(textBox_Setup_Option_Offset_FineCamCoarseCam_Y.Text);
            Equipment.stOffsetDistance.FromFineCamToLaserHeightSensor.X = Equipment.ToDouble(textBox_Setup_Option_Offset_FineCamLaserHeightSensor_X.Text);
            Equipment.stOffsetDistance.FromFineCamToLaserHeightSensor.Y = Equipment.ToDouble(textBox_Setup_Option_Offset_FineCamLaserHeightSensor_Y.Text);
            NativeMethods.WritePrivateProfileString("Offset_Distance", "From_Scanner_To_FineCam_X", textBox_Setup_Option_Offset_ScannerFineCam_X.Text, strFIle);
            NativeMethods.WritePrivateProfileString("Offset_Distance", "From_Scanner_To_FineCam_Y", textBox_Setup_Option_Offset_ScannerFineCam_Y.Text, strFIle);
            NativeMethods.WritePrivateProfileString("Offset_Distance", "From_FineCam_To_CoarseCam_X", textBox_Setup_Option_Offset_FineCamCoarseCam_X.Text, strFIle);
            NativeMethods.WritePrivateProfileString("Offset_Distance", "From_FineCam_To_CoarseCam_Y", textBox_Setup_Option_Offset_FineCamCoarseCam_Y.Text, strFIle);
            NativeMethods.WritePrivateProfileString("Offset_Distance", "From_FineCam_To_LaserHeightSensor_X", textBox_Setup_Option_Offset_FineCamLaserHeightSensor_X.Text, strFIle);
            NativeMethods.WritePrivateProfileString("Offset_Distance", "From_FineCam_To_LaserHeightSensor_Y", textBox_Setup_Option_Offset_FineCamLaserHeightSensor_Y.Text, strFIle);

            //  Scanner Head Offset
            Equipment.Scanner_HeadOffset_X = Equipment.ToDouble(textBox_ScannerOffset_X.Text);
            Equipment.Scanner_HeadOffset_Y = Equipment.ToDouble(textBox_ScannerOffset_Y.Text);
            Equipment.Scanner_HeadOffset_Angle = Equipment.ToDouble(textBox_ScannerOffset_Angle.Text);
            NativeMethods.WritePrivateProfileString("ScannerHeadOffset", "Offset_X", textBox_ScannerOffset_X.Text, strFIle);
            NativeMethods.WritePrivateProfileString("ScannerHeadOffset", "Offset_Y", textBox_ScannerOffset_Y.Text, strFIle);
            NativeMethods.WritePrivateProfileString("ScannerHeadOffset", "Offset_Angle", textBox_ScannerOffset_Angle.Text, strFIle);

            //  Coordinate System Matching Offset
            Equipment.CoordinateMatchingOffset_X = Equipment.ToDouble(textBox_Setup_Option_MachineOffset_StageOriginPosToScannerCenter_X.Text);
            Equipment.CoordinateMatchingOffset_Y = Equipment.ToDouble(textBox_Setup_Option_MachineOffset_StageOriginPosToScannerCenter_Y.Text);
            NativeMethods.WritePrivateProfileString("MachineCoordinateOffset", "Offset_X", textBox_Setup_Option_MachineOffset_StageOriginPosToScannerCenter_X.Text, strFIle);
            NativeMethods.WritePrivateProfileString("MachineCoordinateOffset", "Offset_Y", textBox_Setup_Option_MachineOffset_StageOriginPosToScannerCenter_Y.Text, strFIle);

            //  Offset Distance from Stage to Scanner
            Equipment.StageOffset_forDrilling_X = Equipment.ToDouble(textBox_Setup_Option_OffsetDistance_StageCenterToScannerCenter_X.Text);
            Equipment.StageOffset_forDrilling_Y = Equipment.ToDouble(textBox_Setup_Option_OffsetDistance_StageCenterToScannerCenter_Y.Text);
            NativeMethods.WritePrivateProfileString("Offset_Distance_forDrilling", "From_Stage_To_Scanner_X", textBox_Setup_Option_OffsetDistance_StageCenterToScannerCenter_X.Text, strFIle);
            NativeMethods.WritePrivateProfileString("Offset_Distance_forDrilling", "From_Stage_To_Scanner_Y", textBox_Setup_Option_OffsetDistance_StageCenterToScannerCenter_Y.Text, strFIle);

            //  Keyence Laser Height Sensor 기준값 설정
            Equipment.LaserHeightSensor_ReferenceValue_atVisionFocusPosition = Equipment.ToDouble(textBox_Setup_Option_ReferenceValue_atVisionFocusPosition.Text);
            Equipment.LaserHeightSensor_ReferenceValue_atScannerFocusPosition = Equipment.ToDouble(textBox_Setup_Option_ReferenceValue_atScannerFocusPosition.Text);
            NativeMethods.WritePrivateProfileString("LaserHeightSensor_ReferenceValue", "at_Vision_Focus_Position", textBox_Setup_Option_ReferenceValue_atVisionFocusPosition.Text, strFIle);
            NativeMethods.WritePrivateProfileString("LaserHeightSensor_ReferenceValue", "at_Scanner_Focus_Position", textBox_Setup_Option_ReferenceValue_atScannerFocusPosition.Text, strFIle);

            //  집진기 동작 후 대기 시간
            Equipment.DustCollector_TurnOn_AfterStableTime = Equipment.ToDouble(textBox_Setup_Option_DustCollector_WaitingTime.Text);
            NativeMethods.WritePrivateProfileString("Dust_Collector", "After_TurnOn_StableTime", textBox_Setup_Option_DustCollector_WaitingTime.Text, strFIle);

            //  레시피, 도면 폴더
            Equipment.RecipeFilePath = richTextBox_Recipe_TabRecipe_RecipeFileFolder.Text;
            NativeMethods.WritePrivateProfileString("File_Path", "RecipeFile", richTextBox_Recipe_TabRecipe_RecipeFileFolder.Text, strFIle);
            Equipment.DrawingFilePath = richTextBox_Recipe_TabRecipe_DrawingFileFolder.Text;
            NativeMethods.WritePrivateProfileString("File_Path", "DrawingFile", richTextBox_Recipe_TabRecipe_DrawingFileFolder.Text, strFIle);

            //  도면 렌더링 분해능
            Equipment.SiriusDrawing_Rendering_Resolution = Equipment.ToInt(textBox_Setup_Option_Sirius_Drawing_Resolution.Text);
            NativeMethods.WritePrivateProfileString("Sirius_Drawing", "Rendering_Resolution", textBox_Setup_Option_Sirius_Drawing_Resolution.Text, strFIle);
            SpiralLab.Sirius.Config.AngleFactor = Equipment.SiriusDrawing_Rendering_Resolution;

            ////  Scanner Calibration parameter
            //Equipment.Scanner_Calibration_LaserFrequency = Equipment.ToDouble(textBox_Setup_ScannerCal_LaserFrequency.Text);
            //Equipment.Scanner_Calibration_LaserPulseWidth = Equipment.ToDouble(textBox_Setup_ScannerCal_PulseWidth.Text);
            //Equipment.Scanner_Calibration_LaserEnergy = Equipment.ToDouble(textBox_Setup_ScannerCal_LaserEnergy.Text);
            //Equipment.Scanner_Calibration_CrossMarkLength = Equipment.ToDouble(textBox_Setup_ScannerCal_CrossMarkLength.Text);
            //Equipment.Scanner_Calibration_LaserMarkSpeed = Equipment.ToDouble(textBox_Setup_ScannerCal_MarkingSpeed.Text);
            //Equipment.Scanner_Calibration_LaserJumpSpeed = Equipment.ToDouble(textBox_Setup_ScannerCal_JumpSpeed.Text);
            //Equipment.Scanner_Calibration_LaserOnDelay = Equipment.ToDouble(textBox_Setup_ScannerCal_LaserOnDelay.Text);
            //Equipment.Scanner_Calibration_LaserOffDelay = Equipment.ToDouble(textBox_Setup_ScannerCal_LaserOffDelay.Text);
            //Equipment.Scanner_Calibration_MarkDelay = Equipment.ToDouble(textBox_Setup_ScannerCal_MarkDelay.Text);
            //Equipment.Scanner_Calibration_JumpDelay = Equipment.ToDouble(textBox_Setup_ScannerCal_JumpDelay.Text);
            //Equipment.Scanner_Calibration_PolygonDelay = Equipment.ToDouble(textBox_Setup_ScannerCal_PolygonDelay.Text);
            //Equipment.Scanner_Calibration_CalAreaWidth = Equipment.ToDouble(textBox_Setup_ScannerCal_CalAreaWidth.Text);
            //Equipment.Scanner_Calibration_CalAreaHeight = Equipment.ToDouble(textBox_Setup_ScannerCal_CalAreaHeight.Text);
            //Equipment.Scanner_Calibration_CalPitch = Equipment.ToDouble(textBox_Setup_ScannerCal_CalPitch.Text);

            //NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "Laser_Frequency", textBox_Setup_ScannerCal_LaserFrequency.Text, strFIle);
            //NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "Laser_Pulse_Width", textBox_Setup_ScannerCal_PulseWidth.Text, strFIle);
            //NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "Laser_Energy", textBox_Setup_ScannerCal_LaserEnergy.Text, strFIle);
            //NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "CrossMark_Length", textBox_Setup_ScannerCal_CrossMarkLength.Text, strFIle);
            //NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "Marking_Speed", textBox_Setup_ScannerCal_MarkingSpeed.Text, strFIle);
            //NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "Jump_Speed", textBox_Setup_ScannerCal_JumpSpeed.Text, strFIle);
            //NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "Laser_On_Delay", textBox_Setup_ScannerCal_LaserOnDelay.Text, strFIle);
            //NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "Laser_Off_Delay", textBox_Setup_ScannerCal_LaserOffDelay.Text, strFIle);
            //NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "Mark_Delay", textBox_Setup_ScannerCal_MarkDelay.Text, strFIle);
            //NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "Jump_Delay", textBox_Setup_ScannerCal_JumpDelay.Text, strFIle);
            //NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "Polygon_Delay", textBox_Setup_ScannerCal_PolygonDelay.Text, strFIle);
            //NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "Cal_Area_Width", textBox_Setup_ScannerCal_CalAreaWidth.Text, strFIle);
            //NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "Cal_Area_Height", textBox_Setup_ScannerCal_CalAreaHeight.Text, strFIle);
            //NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "Cal_Pitch", textBox_Setup_ScannerCal_CalPitch.Text, strFIle);

            //Equipment.Scanner_Calibration_srcFilePath = m_correction2DRtc.SourceCorrectionFile; // m_srcFile;
            //Equipment.Scanner_Calibration_targetFilePath = m_correction2DRtc.TargetCorrectionFile;  // m_targetFile;
            //Equipment.Scanner_Calibration_FieldSize = m_fieldSize;
            //Equipment.Scanner_Calibration_rowInterval = m_correction2DRtc.RowInterval;  // m_rowInterval;
            //Equipment.Scanner_Calibration_colInterval = m_correction2DRtc.ColInterval; //m_colInterval;
            //Equipment.Scanner_Calibration_rowCount = m_correction2DRtc.Rows;   //m_row;
            //Equipment.Scanner_Calibration_colCount = m_correction2DRtc.Cols;   //m_col;

            //m_srcFile = Equipment.Scanner_Calibration_srcFilePath;
            //m_targetFile = Equipment.Scanner_Calibration_targetFilePath;
            //m_rowInterval = (float)Equipment.Scanner_Calibration_rowInterval;
            //m_colInterval = (float)Equipment.Scanner_Calibration_colInterval;
            //m_row = Equipment.Scanner_Calibration_rowCount;
            //m_col = Equipment.Scanner_Calibration_colCount;

            //NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "srcFilePath", m_srcFile, strFIle);
            //NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "targetFilePath", m_targetFile, strFIle);
            //NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "FieldSize", m_fieldSize.ToString(), strFIle);
            //NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "rowInterval", m_rowInterval.ToString(), strFIle);
            //NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "colInterval", m_colInterval.ToString(), strFIle);
            //NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "rowCount", m_row.ToString(), strFIle);
            //NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "colCount", m_col.ToString(), strFIle);

            MessageBox.Show("Machine Option 파일을 저장하였습니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void Machine_ScannerCalibration_Save()
        {
            string strTemp = "";
            string strFIle = "";
            strFIle = ConfigManager.GetConfigPath() + "\\Machine ScannerCalibration (Do not delete or modify).ini";
            if (File.Exists(strFIle) == false)
            {
                MessageBox.Show("Machine ScannerCalibration 파일이 없습니다.\r\n\r\n[Default 값(CO₂)으로 설정됩니다.]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return false;
            }

            if (File.Exists(strFIle) == false)
            {
                File.Create(strFIle);
                //return;

                MessageBox.Show("Machine ScannerCalibration 파일을 생성하였습니다. 다시 시도하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //  Scanner Calibration parameter
            Equipment.Scanner_Calibration_LaserFrequency = Equipment.ToDouble(textBox_Setup_ScannerCal_LaserFrequency.Text);
            Equipment.Scanner_Calibration_LaserPulseWidth = Equipment.ToDouble(textBox_Setup_ScannerCal_PulseWidth.Text);
            Equipment.Scanner_Calibration_LaserEnergy = Equipment.ToDouble(textBox_Setup_ScannerCal_LaserEnergy.Text);
            Equipment.Scanner_Calibration_CrossMarkLength = Equipment.ToDouble(textBox_Setup_ScannerCal_CrossMarkLength.Text);
            Equipment.Scanner_Calibration_LaserMarkSpeed = Equipment.ToDouble(textBox_Setup_ScannerCal_MarkingSpeed.Text);
            Equipment.Scanner_Calibration_LaserJumpSpeed = Equipment.ToDouble(textBox_Setup_ScannerCal_JumpSpeed.Text);
            Equipment.Scanner_Calibration_LaserOnDelay = Equipment.ToDouble(textBox_Setup_ScannerCal_LaserOnDelay.Text);
            Equipment.Scanner_Calibration_LaserOffDelay = Equipment.ToDouble(textBox_Setup_ScannerCal_LaserOffDelay.Text);
            Equipment.Scanner_Calibration_MarkDelay = Equipment.ToDouble(textBox_Setup_ScannerCal_MarkDelay.Text);
            Equipment.Scanner_Calibration_JumpDelay = Equipment.ToDouble(textBox_Setup_ScannerCal_JumpDelay.Text);
            Equipment.Scanner_Calibration_PolygonDelay = Equipment.ToDouble(textBox_Setup_ScannerCal_PolygonDelay.Text);
            Equipment.Scanner_Calibration_CalAreaWidth = Equipment.ToDouble(textBox_Setup_ScannerCal_CalAreaWidth.Text);
            Equipment.Scanner_Calibration_CalAreaHeight = Equipment.ToDouble(textBox_Setup_ScannerCal_CalAreaHeight.Text);
            Equipment.Scanner_Calibration_CalPitch = Equipment.ToDouble(textBox_Setup_ScannerCal_CalPitch.Text);

            NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "Laser_Frequency", textBox_Setup_ScannerCal_LaserFrequency.Text, strFIle);
            NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "Laser_Pulse_Width", textBox_Setup_ScannerCal_PulseWidth.Text, strFIle);
            NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "Laser_Energy", textBox_Setup_ScannerCal_LaserEnergy.Text, strFIle);
            NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "CrossMark_Length", textBox_Setup_ScannerCal_CrossMarkLength.Text, strFIle);
            NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "Marking_Speed", textBox_Setup_ScannerCal_MarkingSpeed.Text, strFIle);
            NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "Jump_Speed", textBox_Setup_ScannerCal_JumpSpeed.Text, strFIle);
            NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "Laser_On_Delay", textBox_Setup_ScannerCal_LaserOnDelay.Text, strFIle);
            NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "Laser_Off_Delay", textBox_Setup_ScannerCal_LaserOffDelay.Text, strFIle);
            NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "Mark_Delay", textBox_Setup_ScannerCal_MarkDelay.Text, strFIle);
            NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "Jump_Delay", textBox_Setup_ScannerCal_JumpDelay.Text, strFIle);
            NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "Polygon_Delay", textBox_Setup_ScannerCal_PolygonDelay.Text, strFIle);
            NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "Cal_Area_Width", textBox_Setup_ScannerCal_CalAreaWidth.Text, strFIle);
            NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "Cal_Area_Height", textBox_Setup_ScannerCal_CalAreaHeight.Text, strFIle);
            NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "Cal_Pitch", textBox_Setup_ScannerCal_CalPitch.Text, strFIle);

            Equipment.Scanner_Calibration_srcFilePath = m_correction2DRtc.SourceCorrectionFile; // m_srcFile;
            Equipment.Scanner_Calibration_targetFilePath = m_correction2DRtc.TargetCorrectionFile;  // m_targetFile;
            Equipment.Scanner_Calibration_FieldSize = m_fieldSize;
            Equipment.Scanner_Calibration_rowInterval = m_correction2DRtc.RowInterval;  // m_rowInterval;
            Equipment.Scanner_Calibration_colInterval = m_correction2DRtc.ColInterval;  //m_colInterval;
            Equipment.Scanner_Calibration_rowCount = m_correction2DRtc.Rows;   //m_row;
            Equipment.Scanner_Calibration_colCount = m_correction2DRtc.Cols;   //m_col;

            m_srcFile = Equipment.Scanner_Calibration_srcFilePath;
            m_targetFile = Equipment.Scanner_Calibration_targetFilePath;
            m_rowInterval = (float)Equipment.Scanner_Calibration_rowInterval;
            m_colInterval = (float)Equipment.Scanner_Calibration_colInterval;
            m_row = Equipment.Scanner_Calibration_rowCount;
            m_col = Equipment.Scanner_Calibration_colCount;

            NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "srcFilePath", m_srcFile, strFIle);
            NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "targetFilePath", m_targetFile, strFIle);
            NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "FieldSize", m_fieldSize.ToString(), strFIle);
            NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "rowInterval", m_rowInterval.ToString(), strFIle);
            NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "colInterval", m_colInterval.ToString(), strFIle);
            NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "rowCount", m_row.ToString(), strFIle);
            NativeMethods.WritePrivateProfileString("Scanner_Calibration_Parameter", "colCount", m_col.ToString(), strFIle);

            MessageBox.Show("Machine ScannerCalibration 파일을 저장하였습니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnScannerOffset_Set_Click(object sender, EventArgs e)
        {
            Vector3 ScannerOffset = new Vector3(0, 0, 0);

            ScannerOffset.X = (float)Equipment.ToDouble(textBox_ScannerOffset_X.Text);
            ScannerOffset.Y = (float)Equipment.ToDouble(textBox_ScannerOffset_Y.Text);
            ScannerOffset.Z = (float)Equipment.ToDouble(textBox_ScannerOffset_Angle.Text);

            workStage.rtc.PrimaryHeadBaseOffset = ScannerOffset;
        }

        private void button_Setup_Tab2DMap_StageCenter_ScannerPosition_FileOpen_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog fd = new OpenFileDialog())
            {
                fd.CustomPlaces.Add(SLD200.Properties.Settings.Default.JobFolder);
                //fd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Recent);    //  최근 목록으로 기본폴더 설정
                fd.Filter = "2D Mapping files (*.csv)|*.csv"; //필터 설정
                fd.FilterIndex = 1; //1번 선택시 txt , 2번 선택시 *.*

                Log.Write("SLD-200", "Button Click", "스테이지 맵핑 파일 불러오기");

                if (fd.ShowDialog() == DialogResult.OK)
                {
                    filePath = fd.FileName;

                    richTextBox_Setup_Tab2DMap_StageCenter_ScannerPosition.Text = filePath;
                }
            }
        }

        private void button_Setup_Tab2DMap_StageCenter_FineCameraPosition_FileOpen_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog fd = new OpenFileDialog())
            {
                fd.CustomPlaces.Add(SLD200.Properties.Settings.Default.JobFolder);
                //fd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Recent);    //  최근 목록으로 기본폴더 설정
                fd.Filter = "2D Mapping files (*.csv)|*.csv"; //필터 설정
                fd.FilterIndex = 1; //1번 선택시 txt , 2번 선택시 *.*

                Log.Write("SLD-200", "Button Click", "스테이지 맵핑 파일 불러오기");

                if (fd.ShowDialog() == DialogResult.OK)
                {
                    filePath = fd.FileName;

                    richTextBox_Setup_Tab2DMap_StageCenter_FineCameraPosition.Text = filePath;
                }
            }
        }

        private void button_Setup_Tab2DMap_StageCalPos_ScannerPosition_FileOpen_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog fd = new OpenFileDialog())
            {
                fd.CustomPlaces.Add(SLD200.Properties.Settings.Default.JobFolder);
                //fd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Recent);    //  최근 목록으로 기본폴더 설정
                fd.Filter = "2D Mapping files (*.csv)|*.csv"; //필터 설정
                fd.FilterIndex = 1; //1번 선택시 txt , 2번 선택시 *.*

                Log.Write("SLD-200", "Button Click", "스테이지 맵핑 파일 불러오기");

                if (fd.ShowDialog() == DialogResult.OK)
                {
                    filePath = fd.FileName;

                    richTextBox_Setup_Tab2DMap_StageCalPos_ScannerPosition.Text = filePath;
                }
            }
        }

        private void button_Setup_Tab2DMap_StageCalPos_FineCameraPosition_FileOpen_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog fd = new OpenFileDialog())
            {
                fd.CustomPlaces.Add(SLD200.Properties.Settings.Default.JobFolder);
                //fd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Recent);    //  최근 목록으로 기본폴더 설정
                fd.Filter = "2D Mapping files (*.csv)|*.csv"; //필터 설정
                fd.FilterIndex = 1; //1번 선택시 txt , 2번 선택시 *.*

                Log.Write("SLD-200", "Button Click", "스테이지 맵핑 파일 불러오기");

                if (fd.ShowDialog() == DialogResult.OK)
                {
                    filePath = fd.FileName;

                    richTextBox_Setup_Tab2DMap_StageCalPos_FineCameraPosition.Text = filePath;
                }
            }
        }

        public void MapData_List_Save()
        {
            string strTemp = "";

            string strFIle = "";
            strFIle = ConfigManager.GetConfigPath() + "\\Mapping File List (Do not delete or modify).ini";

            if (File.Exists(strFIle) == false)
            {
                File.Create(strFIle);
                //return;

                MessageBox.Show("2D Mapping 데이터 리스트 파일을 생성하였습니다. 다시 시도하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            Equipment.MappingData_FilePath_Stage_Scanner = richTextBox_Setup_Tab2DMap_StageCenter_ScannerPosition.Text;
            Equipment.MappingData_FilePath_Stage_FineCam = richTextBox_Setup_Tab2DMap_StageCenter_FineCameraPosition.Text;
            Equipment.MappingData_FilePath_StageCal_Scanner = richTextBox_Setup_Tab2DMap_StageCalPos_ScannerPosition.Text;
            Equipment.MappingData_FilePath_StageCal_FineCam = richTextBox_Setup_Tab2DMap_StageCalPos_FineCameraPosition.Text;
            
            
            //  Stage Center 가 Scanner 위치에 있을 경우
            NativeMethods.WritePrivateProfileString("MappingFilePath", "StageCenter_ScannerCenter", Equipment.MappingData_FilePath_Stage_Scanner, strFIle);

            //  Stage Center 가 Fine Camera 위치에 있을 경우
            NativeMethods.WritePrivateProfileString("MappingFilePath", "StageCenter_FineCameraCenter", Equipment.MappingData_FilePath_Stage_FineCam, strFIle);

            //  Stage Calibration 위치가 Scanner 위치에 있을 경우
            NativeMethods.WritePrivateProfileString("MappingFilePath", "StageCalPos_ScannerCenter", Equipment.MappingData_FilePath_StageCal_Scanner, strFIle);

            //  Stage Calibration 위치가 Fine Camera 위치에 있을 경우
            NativeMethods.WritePrivateProfileString("MappingFilePath", "StageCalPos_FineCameraCenter", Equipment.MappingData_FilePath_StageCal_FineCam, strFIle);


            MessageBox.Show("2D Mapping 데이터 리스트 파일을 저장하였습니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void MapDataStatus_Save()
        {
            string strTemp = "";

            string strFIle = "";
            strFIle = ConfigManager.GetConfigPath() + "\\Mapping File List (Do not delete or modify).ini";

            if (File.Exists(strFIle) == false)
            {
                File.Create(strFIle);
                //return;

                MessageBox.Show("2D Mapping 데이터 리스트 파일을 생성하였습니다. 다시 시도하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            //  활성화 여부
            NativeMethods.WritePrivateProfileString("MapData", "Activate", Equipment.MapDataStatus_Activate.ToString(), strFIle);
        }

        private void button_Setup_2DMapping_Save_Click(object sender, EventArgs e)
        {
            MapData_List_Save();
        }

        private void button_Setup_ScannerFineCamOffsetChange_ImageDisplay_Show_Click(object sender, EventArgs e)
        {           
            //  Scanner 와 Fine Camera 간의 Offset 값을 변경한다.

            Equipment.m_bVisionFormOpenMode_ScannerFineCamOffsetChange = true;

            if (m_formVisionPopup == null)
            {
                m_formVisionPopup.CreateSiriusEditor();
            }

            foreach (Form openForm in System.Windows.Forms.Application.OpenForms)
            {
                if (openForm.Name == m_formVisionPopup.Name)
                {
                    openForm.BringToFront();
                    openForm.Show();
                    return;
                }
            }

            m_formVisionPopup.Show();
        }

        private void button_Setup_2DMapData_Apply_Click(object sender, EventArgs e)
        {
            //  맵 데이터 적용

            workStage.MapData_Load();

            Equipment.MapDataStatus_Activate = !Equipment.MapDataStatus_Activate;

            if (Equipment.MapDataStatus_Activate)
            {
                button_Setup_2DMapData_Apply.Text = "Map Data Activated";
                button_Setup_2DMapData_Apply.BackColor = Color.Lime;

                workStage.Stage.Config.Use2DMap = true;

                MessageBox.Show("2D Mapping 데이터 활성화", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                button_Setup_2DMapData_Apply.Text = "Map Data Deactivated";
                button_Setup_2DMapData_Apply.BackColor = Color.LightGray;

                workStage.Stage.Config.Use2DMap = false;

                MessageBox.Show("2D Mapping 데이터 비활성화", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            MapDataStatus_Save();
        }

        private void FormNew_Setup_Shown(object sender, EventArgs e)
        {
            if (Equipment.Machine_Door_Enable)
            {
                checkBox_Setup_Option_DoorEnable.Checked = true;
            }
            else
            {
                checkBox_Setup_Option_DoorEnable.Checked = false;
            }

            textBox_Setup_Option_SignalHoldTime.Text = Equipment.Machine_SignalHoldTime.ToString();

            if (Equipment.Machine_VacuumSensor_Enable)
            {
                checkBox_Setup_Option_VacuumSensorEnable.Checked = true;
                textBox_Setup_Option_SignalHoldTime.Enabled = false;
            }
            else
            {
                checkBox_Setup_Option_VacuumSensorEnable.Checked = false;
                textBox_Setup_Option_SignalHoldTime.Enabled = true;
            }

            if (Equipment.Machine_MAligner_ReleaseType)
            {
                checkBox_Setup_Option_MAligner_ReleaseType.Checked = true;
            }
            else
            {
                checkBox_Setup_Option_MAligner_ReleaseType.Checked = false;
            }

            textBox_Setup_Option_MAligner_NarrowingDistance.Text = Equipment.Machine_MAligner_NarrowingDistance.ToString();
            textBox_Setup_Option_MAligner_WidenDistance.Text = Equipment.Machine_MAligner_WidenDistance.ToString();

            if (Equipment.Machine_VacuumStableTime_Enable)
            {
                checkBox_Setup_Option_VacuumStableTime_Enable.Checked = true;
                textBox_Setup_Option_VacuumStableTime.Enabled = true;
            }
            else
            {
                checkBox_Setup_Option_VacuumStableTime_Enable.Checked = false;
                textBox_Setup_Option_VacuumStableTime.Enabled = false;
            }

            if (Equipment.Machine_LaserHeightCheckStableTime_Enable)
            {
                checkBox_Setup_Option_LaserHeightCheckStableTime_Enable.Checked = true;
                textBox_Setup_Option_LaserHeightCheckStableTime.Enabled = true;
            }
            else
            {
                checkBox_Setup_Option_LaserHeightCheckStableTime_Enable.Checked = false;
                textBox_Setup_Option_LaserHeightCheckStableTime.Enabled = false;
            }

            if (Equipment.Machine_FiducialMarkJudgementRange_Enable)
            {
                checkBox_Setup_Option_FiducialMarkJudgementRange_Enable.Checked = true;
                textBox_Setup_Option_FiducialMarkJudgementRange.Enabled = true;
            }
            else
            {
                checkBox_Setup_Option_FiducialMarkJudgementRange_Enable.Checked = false;
                textBox_Setup_Option_FiducialMarkJudgementRange.Enabled = false;
            }

            if (Equipment.Machine_FiducialImageSave_Always)
            {
                radioButton_Setup_Option_FiducialImageSave_Always.Checked = true;
            }
            else
            {
                radioButton_Setup_Option_FiducialImageSave_FailedToFind.Checked = true;
            }

            if (Equipment.Machine_VacuumBlowTime_Enable)
            {
                checkBox_Setup_Option_VacuumBlowTime_Enable.Checked = true;
                textBox_Setup_Option_VacuumBlowTime.Enabled = true;
            }
            else
            {
                checkBox_Setup_Option_VacuumBlowTime_Enable.Checked = false;
                textBox_Setup_Option_VacuumBlowTime.Enabled = false;
            }
        }

        private void checkBox_Setup_Option_VacuumSensorEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Setup_Option_VacuumSensorEnable.Checked)
            {
                Equipment.Machine_VacuumSensor_Enable = true;
                textBox_Setup_Option_SignalHoldTime.Enabled = false;
            }
            else
            {
                Equipment.Machine_VacuumSensor_Enable = false;
                textBox_Setup_Option_SignalHoldTime.Enabled = true;
            }
        }

        private void checkBox_Setup_Option_VacuumStableTime_Enable_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Setup_Option_VacuumStableTime_Enable.Checked)
            {
                Equipment.Machine_VacuumStableTime_Enable = true;
                textBox_Setup_Option_VacuumStableTime.Enabled = true;
            }
            else
            {
                Equipment.Machine_VacuumStableTime_Enable = false;
                textBox_Setup_Option_VacuumStableTime.Enabled = false;
            }
        }

        private void checkBox_Setup_Option_LaserHeightCheckStableTime_Enable_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Setup_Option_LaserHeightCheckStableTime_Enable.Checked)
            {
                Equipment.Machine_LaserHeightCheckStableTime_Enable = true;
                textBox_Setup_Option_LaserHeightCheckStableTime.Enabled = true;
            }
            else
            {
                Equipment.Machine_LaserHeightCheckStableTime_Enable = false;
                textBox_Setup_Option_LaserHeightCheckStableTime.Enabled = false;
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            //Alarm alarm = QMC.Common.Parts.GetAlarm((int)AlarmKey.eClamp_Failed);

            // 알람을 이렇게 띄워야 하는건가..
            Alarm alarm = new Alarm();
            alarm.Title = "Focus Value Failed.";
            alarm.Code = -3;
            alarm.Source = Name;
            alarm.Grade = "Error";
            alarm.Cause = "Focus Value Failed.";
            AlarmManager.Instance.ShowAlarm(alarm);


            //String message = "D:\\SLD-200\\Log\\ScannerCalData\\ScanerCalData2025-04-17 23_46_09.txt";
            //m_correction2DRtc.ConvertFromDatFile(message);
            //m_correction2DRtcForm.RefreshData();
            //m_correction2DRtc.Convert();

            //  Scanner Calibration Test
            //m_kfactor = m_correction2DRtc.KFactor;
            //m_row = m_correction2DRtc.Rows;
            //m_col = m_correction2DRtc.Cols;
            //m_rowInterval = m_correction2DRtc.RowInterval;
            //m_colInterval = m_correction2DRtc.ColInterval;
            //m_srcFile = m_correction2DRtc.SourceCorrectionFile;
            //m_targetFile = m_correction2DRtc.TargetCorrectionFile;
            //Vector2 position = new Vector2(0, 0);   //현재 장비 위치값 넣고..
            //Vector2 offset = new Vector2(0, 0);     //마크 찾은 위치값 넣어보자.
            //for (int x = 0; x < m_row; x++)
            //{
            //    for (int y = 0; y < m_col; y++)
            //    {
            //        position.X = 200.0f + (x * m_rowInterval);
            //        position.Y = 400.0f + (y * m_colInterval);
            //        offset.X = 0.12f + x;
            //        offset.Y = 0.23f + y;
            //        m_correction2DRtc.AddAbsolute(x, y, position, offset);
            //        //m_correction2DRtc.AddRelative(x, y, position, offset);
            //        //m_correction2DRtcForm.RefreshData();
            //    }
            //}
            //m_correction2DRtcForm.RefreshData();
            //m_correction2DRtc.Convert();
        }

        private void button_Setup_ScannerCal_Save_Click(object sender, EventArgs e)
        {
            //  Scanner Calibration Parameter 저장
            string strTemp = "";
            StringBuilder temp = new StringBuilder(255);

            //  체크 포인트
            if (((Equipment.ToDouble(textBox_Setup_Option_MachineOffset_StageOriginPosToScannerCenter_X.Text) != 0.0) || (Equipment.ToDouble(textBox_Setup_Option_MachineOffset_StageOriginPosToScannerCenter_Y.Text) != 0.0)) &&
                ((Equipment.ToDouble(textBox_Setup_Option_OffsetDistance_StageCenterToScannerCenter_X.Text) != 0.0) || (Equipment.ToDouble(textBox_Setup_Option_OffsetDistance_StageCenterToScannerCenter_Y.Text) != 0.0)))
            {
                MessageBox.Show("\"Offset Distance for Coordinate Matching\" 과\r\n\"Offset Distance to the Center of the Scanner\" 두 그룹 전체에 값이 들어가면 안됩니다.\n\r\n[두 그룹 중 한쪽에만 값이 들어가거나, 모두 0 이어야 합니다.]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //  Scanner Calibration 관련 파라미터 저장
            Machine_ScannerCalibration_Save();
        }

        private void btnCalStart_Click(object sender, EventArgs e)
        {
            //Test Code
            //workStage.m_ScannerCalibration_Start = true;
            ////workStage.timer_ScannerCalibration.Enabled = true;
            //workStage.timer_ScannerCalibration.Start();
            //workStage._isCalibrationRunning = false;
            //workStage.m_nScanner_Calibration_Step = (int)WorkStage.ScannerCalibration_Step.Start;
            //return;

            var mb = new QMC.Common.UI.MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Scanner Calibration - Laser부터 시작합니다.\n\n시작하시겠습니까?"))
            {
                return;
            }

            //TEST
            //if (!workStage.m_bHomeOK)
            //{
            //    var mb2 = new MessageBoxOk();
            //    mb2.ShowDialog("Information !", "장비 초기화를 해야 합니다.");
            //    return;
            //}

            if (Equipment.EqpSiriusViewer == null)
            {
                var mb2 = new MessageBoxOk();
                mb2.ShowDialog("Information !", "Scanner Board 를 초기화 해야 합니다.");
                return;
            }

            if (workStage.rtc == null)
            {
                var mb2 = new MessageBoxOk();
                mb2.ShowDialog("Information !", "Scanner Board 를 초기화 해야 합니다.");
                return;
            }

            //Equipment.Scanner_Calibration_Change;
            // 캘판 변경 유/무에 대해서 물어보는 메세지 박스해주고 True/False 리턴받기
            //var mb = new QMC.Common.UI.MessageBoxYesNo();
            var mb1 = new QMC.Common.UI.MessageBoxYesNo();
            if (DialogResult.Yes != mb1.ShowDialog("Question ?", "Scanner Calibration을 시작합니다.\n\n캘리브레이션 판이 변경되었습니까?"))
            {
                Equipment.Scanner_Calibration_Change = false;
            }
            else
            {
                Equipment.Scanner_Calibration_Change = true;
            }

            if (workStage.m_nScanner_Calibration_Step == (int)WorkStage.ScannerCalibration_Step.None)
            {
                workStage.timer_ScannerCalibration.Enabled = true;
                workStage.timer_ScannerCalibration.Start();

                workStage._isCalibrationRunning = false;
                workStage.m_ScannerCalibration_Start = true;
                
                Equipment.Scanner_Vision_Offset_Setting_Use = false;
                workStage.scannerCompensator.SetRunStatus(Part.RunStatus.Run);
                workStage.m_nScanner_Calibration_Step = (int)WorkStage.ScannerCalibration_Step.Start;
            }
        }

        private void btnCalStop_Click(object sender, EventArgs e)
        {
            Equipment.AutoRunStatus = false;

            workStage.m_ScannerCalibration_Start = false;
            workStage.timer_ScannerCalibration.Enabled = false; //이거 해야하나..
            workStage.timer_ScannerCalibration.Stop();
            workStage.scannerCompensator.SetRunStatus(Part.RunStatus.Stop);
            workStage.m_nScanner_Calibration_Step = (int)WorkStage.ScannerCalibration_Step.None;

        }

        private void btnCalStart_Vision_Click(object sender, EventArgs e)
        {
            string m_strTemp = "";

            var mb = new QMC.Common.UI.MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Scanner Calibration - Vision부터 시작합니다.\n\n시작하시겠습니까?"))
            {
                return;
            }

            var mb1 = new QMC.Common.UI.MessageBoxYesNo();
            if (DialogResult.Yes != mb1.ShowDialog("Question ?", "Mark가 Center에 위치해 있는지 확인 바랍니다.\n\n시작하시겠습니까?"))
            {
                return;
            }

            //TEST
            //if (!workStage.m_bHomeOK)
            //{
            //    var mb2 = new MessageBoxOk();
            //    mb2.ShowDialog("Information !", "장비 초기화를 해야 합니다.");
            //    return;
            //}

            if (Equipment.EqpSiriusViewer == null)
            {
                var mb2 = new MessageBoxOk();
                mb2.ShowDialog("Information !", "Scanner Board 를 초기화 해야 합니다.");
                return;
            }

            if (workStage.rtc == null)
            {
                var mb2 = new MessageBoxOk();
                mb2.ShowDialog("Information !", "Scanner Board 를 초기화 해야 합니다.");
                return;
            }

            if (workStage.m_nScanner_Calibration_Step == (int)WorkStage.ScannerCalibration_Step.None)
            {
                workStage.timer_ScannerCalibration.Enabled = true;
                workStage.timer_ScannerCalibration.Start();

                workStage._isCalibrationRunning = false;
                workStage.m_ScannerCalibration_Start = true;

                Equipment.Scanner_Vision_Offset_Setting_Use = false;
                workStage.scannerCompensator.SetRunStatus(Part.RunStatus.Run);
                //workStage.m_nScanner_Calibration_Step = (int)WorkStage.ScannerCalibration_Step.StageXY_Move_CrossMarkCenterPos; //고민 필요. 
                workStage.m_nScanner_Calibration_Step = (int)WorkStage.ScannerCalibration_Step.ScannerCompensation_StartPosition_Set;
            }
        }

        private QMCFindLenzCenter m_correctionDataList;
        // CorrectionData를 처리하는 메서드
        public void ProcessCorrectionData(QMCFindLenzCenter correctionDataList)
        {
            // CorrectionData를 처리하는 메서드 0:대기
            Equipment.Scanner_Calibration_Convert = 0;

            int indexX = 0, indexY = 0;
            foreach (var data in correctionDataList.SldData)
            {
                // CorrectionData에서 필요한 값을 추출하여 AddAbsolute 호출
                indexX = data.m_nIndexX;
                indexY = data.m_nindexY;

                // excel 계산식과 동일하게 수정.
                //double dOffsetX = data.m_dX - data.m_dMeasureX;
                //double dMeasureX = data.m_dX + dOffsetX;
                //double dOffsetY = data.m_dY - data.m_dMeasureY;
                //double dMeasureY = data.m_dY + dOffsetY;

                Vector2 position = new Vector2((float)data.m_dX, (float)data.m_dY);   //현재 장비 위치값 넣고..
                Vector2 offset = new Vector2((float)data.m_dMeasureX, (float)data.m_dMeasureY);   //현재 장비 위치값 넣고..

                m_correction2DRtc.AddAbsolute(indexX, indexY, position, offset);
                
            }

            string strPath = "D:\\SLD-200_Parameter\\"; //"D:\\SLD-200\\Log\\"; 
            //날짜, 시간, 파일이름을 넣어준다.
            DateTime now = DateTime.Now;
            string timeString = now.ToString("yyyy-MM-dd HH_mm_ss");
            string filePath = strPath + "Cor_200_U_" + timeString + ".ct5";
            if (Equipment.Machine_LaserType_CO2)
            {
                timeString = now.ToString("yyyy-MM-dd HH_mm_ss");
                filePath = strPath + "Cor_200C_" + timeString + ".ct5";
            }
            else
            {
                timeString = now.ToString("yyyy-MM-dd HH_mm_ss");
                filePath = strPath + "Cor_200U_" + timeString + ".ct5";
            }
            m_targetFile = filePath;
            m_correction2DRtc.TargetCorrectionFile = m_targetFile;
            if (m_correction2DRtcForm.InvokeRequired)
            {
                try
                {
                    this.Invoke(new MethodInvoker(delegate ()
                    {
                        m_correction2DRtcForm.RefreshData();
                        m_correction2DRtc.Convert();
                        Equipment.Scanner_Calibration_Convert = 1; // 성공

                    }));

                }
                catch(Exception ex)
                {
                    Equipment.Scanner_Calibration_Convert = -1; // 실패
                }
            }
            else
            {
                m_correction2DRtcForm.RefreshData();
                m_correction2DRtc.Convert();

                Equipment.Scanner_Calibration_Convert = -1; // 실패
            }

            m_correctionDataList = correctionDataList;

            // Todo: 구영남 - 처리 완료 메세지 확인!
            //Msg :: 처리 완료 메세지... 장비 돌리면서 확인 필요.
            while (true)
            {
                string rtcMsg = m_correction2DRtc.ResultMessage;

                //m_correction2DRtc.OnResult();
                if (rtcMsg != null)
                {
                    Equipment.Scanner_Calibration_Convert = 1; // 성공

                    // Todo: 구영남 - 처리 완료 메세지 확인!
                    //if (rtcMsg.Contains("Success")) // 성공 메시지 확인 (예: "Success"라는 문자열 포함 여부)
                    //{
                    //    Equipment.Scanner_Calibration_Convert = 1; // 성공
                    //}
                    //else
                    //{
                    //    Equipment.Scanner_Calibration_Convert = -1; // 실패
                    //}
                    break; // 루프 종료
                }

                // CPU 점유율을 낮추기 위해 잠시 대기
                Thread.Sleep(100); // 100ms 대기
            }


            //Convert완료 확인 후 해야 한다.
            //1: 기본값. HeadA || 0: HeadB. :: RTC6 내부에서 +1을 한다... 
            //if (LoadCorrectionData(0, m_targetFile))
            //{
            //    MessageBox.Show("파일을 적용했습니다.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    //m_correction2DRtcForm.RefreshData();
            //}
            //else
            //{
            //    MessageBox.Show("파일을 적용하지 못했습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}

            //Equipment.Scanner_Calibration_Convert = 1; // 성공
        }

        // Todo: 구영남 Cal파일 넣기 함수 만들것.
        public bool LoadCorrectionData(int tableIndex, string filePath)
        {
            // CorrectionData를 처리하는 메서드
            // 초기화를 해야 하냐 말아야 하냐.. 
            // 초기화를 안하고 cal파일이 들어가면 땡큐인데.. 될꺼같다.. select도 있으니깐..
            CorrectionTableIndex index = (CorrectionTableIndex)tableIndex;
            bool bRtn = workStage.rtc.CtlLoadCorrectionFile(index, filePath);   //  Sirius1
            if(bRtn == false)
            {
                MessageBox.Show("파일을 적용하지 못했습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            //cal 파일 선택하여 사용한다.
            workStage.rtc.CtlSelectCorrection(index, 0);

            m_srcFile = filePath;
            m_correction2DRtc.SourceCorrectionFile = m_srcFile;
            m_correction2DRtc.TargetCorrectionFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "correction", $"newfile.ct5"); //Equipment.Scanner_Calibration_targetFilePath;
            m_correction2DRtcForm.RefreshData();

            Equipment.Scanner_Calibration_srcFilePath = m_srcFile;

            Machine_ScannerCalibration_Save();

            return true;
        }


        private void button20_Click(object sender, EventArgs e)
        {
            m_correction2DRtc.Clear();
            
            //if(m_correctionDataList != null)
            //{
            //    ProcessCorrectionData(m_correctionDataList);
            //}

        }

        private void checkBox_Setup_Option_FiducialMarkJudgementRange_Enable_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Setup_Option_FiducialMarkJudgementRange_Enable.Checked)
            {
                Equipment.Machine_FiducialMarkJudgementRange_Enable = true;
                textBox_Setup_Option_FiducialMarkJudgementRange.Enabled = true;
            }
            else
            {
                Equipment.Machine_FiducialMarkJudgementRange_Enable = false;
                textBox_Setup_Option_FiducialMarkJudgementRange.Enabled = false;
            }
        }

        private void button_Setup_Flatness_MeasurementPosition1_Clear_Click(object sender, EventArgs e)
        {
            //  1번 위치 Clear

            int nIndex = comboBox_Setup_FlatnessMeasurementPos_List.SelectedIndex;

            if (nIndex < 0)
            {
                MessageBox.Show("측정 위치를 선택하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            textBox_Setup_Flatness_TeachingPos1_StageX.Text = "0.0";
            textBox_Setup_Flatness_TeachingPos1_StageY.Text = "0.0";
        }

        private void comboBox_Setup_FlatnessMeasurementPos_List_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  선택한 측정 위치의 데이터로 변경

            int nIndex = comboBox_Setup_FlatnessMeasurementPos_List.SelectedIndex;

            if (nIndex < 0)
            {
                MessageBox.Show("측정 위치를 선택하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //  Flatness 측정 위치 데이터 불러오기

            textBox_Setup_Flatness_TeachingPos1_StageX.Text = Equipment.stFlatMeasurePos[nIndex].StagePos[0].X.ToString();
            textBox_Setup_Flatness_TeachingPos1_StageY.Text = Equipment.stFlatMeasurePos[nIndex].StagePos[0].Y.ToString();
            textBox_Setup_Flatness_TeachingPos2_StageX.Text = Equipment.stFlatMeasurePos[nIndex].StagePos[1].X.ToString();
            textBox_Setup_Flatness_TeachingPos2_StageY.Text = Equipment.stFlatMeasurePos[nIndex].StagePos[1].Y.ToString();
            textBox_Setup_Flatness_TeachingPos3_StageX.Text = Equipment.stFlatMeasurePos[nIndex].StagePos[2].X.ToString();
            textBox_Setup_Flatness_TeachingPos3_StageY.Text = Equipment.stFlatMeasurePos[nIndex].StagePos[2].Y.ToString();
            textBox_Setup_Flatness_TeachingPos4_StageX.Text = Equipment.stFlatMeasurePos[nIndex].StagePos[3].X.ToString();
            textBox_Setup_Flatness_TeachingPos4_StageY.Text = Equipment.stFlatMeasurePos[nIndex].StagePos[3].Y.ToString();
            textBox_Setup_Flatness_TeachingPos5_StageX.Text = Equipment.stFlatMeasurePos[nIndex].StagePos[4].X.ToString();
            textBox_Setup_Flatness_TeachingPos5_StageY.Text = Equipment.stFlatMeasurePos[nIndex].StagePos[4].Y.ToString();
            textBox_Setup_Flatness_TeachingPos6_StageX.Text = Equipment.stFlatMeasurePos[nIndex].StagePos[5].X.ToString();
            textBox_Setup_Flatness_TeachingPos6_StageY.Text = Equipment.stFlatMeasurePos[nIndex].StagePos[5].Y.ToString();
            textBox_Setup_Flatness_TeachingPos7_StageX.Text = Equipment.stFlatMeasurePos[nIndex].StagePos[6].X.ToString();
            textBox_Setup_Flatness_TeachingPos7_StageY.Text = Equipment.stFlatMeasurePos[nIndex].StagePos[6].Y.ToString();
            textBox_Setup_Flatness_TeachingPos8_StageX.Text = Equipment.stFlatMeasurePos[nIndex].StagePos[7].X.ToString();
            textBox_Setup_Flatness_TeachingPos8_StageY.Text = Equipment.stFlatMeasurePos[nIndex].StagePos[7].Y.ToString();
            textBox_Setup_Flatness_TeachingPos9_StageX.Text = Equipment.stFlatMeasurePos[nIndex].StagePos[8].X.ToString();
            textBox_Setup_Flatness_TeachingPos9_StageY.Text = Equipment.stFlatMeasurePos[nIndex].StagePos[8].Y.ToString();
        }

        private void button_Setup_Flatness_MeasurementPosition_Save_Click(object sender, EventArgs e)
        {
            //  Flatness 측정 위치 저장

            int nIndex = comboBox_Setup_FlatnessMeasurementPos_List.SelectedIndex;

            if (nIndex < 0)
            {
                MessageBox.Show("측정 위치를 선택하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //  Flatness 측정 위치 데이터 저장
            Equipment.stFlatMeasurePos[nIndex].StagePos[0].X = Convert.ToDouble(textBox_Setup_Flatness_TeachingPos1_StageX.Text);
            Equipment.stFlatMeasurePos[nIndex].StagePos[0].Y = Convert.ToDouble(textBox_Setup_Flatness_TeachingPos1_StageY.Text);
            Equipment.stFlatMeasurePos[nIndex].StagePos[1].X = Convert.ToDouble(textBox_Setup_Flatness_TeachingPos2_StageX.Text);
            Equipment.stFlatMeasurePos[nIndex].StagePos[1].Y = Convert.ToDouble(textBox_Setup_Flatness_TeachingPos2_StageY.Text);
            Equipment.stFlatMeasurePos[nIndex].StagePos[2].X = Convert.ToDouble(textBox_Setup_Flatness_TeachingPos3_StageX.Text);
            Equipment.stFlatMeasurePos[nIndex].StagePos[2].Y = Convert.ToDouble(textBox_Setup_Flatness_TeachingPos3_StageY.Text);
            Equipment.stFlatMeasurePos[nIndex].StagePos[3].X = Convert.ToDouble(textBox_Setup_Flatness_TeachingPos4_StageX.Text);
            Equipment.stFlatMeasurePos[nIndex].StagePos[3].Y = Convert.ToDouble(textBox_Setup_Flatness_TeachingPos4_StageY.Text);
            Equipment.stFlatMeasurePos[nIndex].StagePos[4].X = Convert.ToDouble(textBox_Setup_Flatness_TeachingPos5_StageX.Text);
            Equipment.stFlatMeasurePos[nIndex].StagePos[4].Y = Convert.ToDouble(textBox_Setup_Flatness_TeachingPos5_StageY.Text);
            Equipment.stFlatMeasurePos[nIndex].StagePos[5].X = Convert.ToDouble(textBox_Setup_Flatness_TeachingPos6_StageX.Text);
            Equipment.stFlatMeasurePos[nIndex].StagePos[5].Y = Convert.ToDouble(textBox_Setup_Flatness_TeachingPos6_StageY.Text);
            Equipment.stFlatMeasurePos[nIndex].StagePos[6].X = Convert.ToDouble(textBox_Setup_Flatness_TeachingPos7_StageX.Text);
            Equipment.stFlatMeasurePos[nIndex].StagePos[6].Y = Convert.ToDouble(textBox_Setup_Flatness_TeachingPos7_StageY.Text);
            Equipment.stFlatMeasurePos[nIndex].StagePos[7].X = Convert.ToDouble(textBox_Setup_Flatness_TeachingPos8_StageX.Text);
            Equipment.stFlatMeasurePos[nIndex].StagePos[7].Y = Convert.ToDouble(textBox_Setup_Flatness_TeachingPos8_StageY.Text);
            Equipment.stFlatMeasurePos[nIndex].StagePos[8].X = Convert.ToDouble(textBox_Setup_Flatness_TeachingPos9_StageX.Text);
            Equipment.stFlatMeasurePos[nIndex].StagePos[8].Y = Convert.ToDouble(textBox_Setup_Flatness_TeachingPos9_StageY.Text);


            FlatMeasurePos_Data_Save();
        }


        #region Flatness Measurement Position Save / Load

        //  Load 는 Equipment 에서

        public void FlatMeasurePos_Data_Save()
        {
            string strTemp = "";
            string strTemp2 = "";

            string strFIle = "";
            strFIle = ConfigManager.GetTeachingDataPath() + "\\FlatMeasure_TeachingPosition.ini";

            if (File.Exists(strFIle) == false)
            {
                File.Create(strFIle);

                strTemp = string.Format("{0} 파일을 생성하였습니다. 다시 시도하십시오.", System.IO.Path.GetFileName(strFIle));
                MessageBox.Show(strTemp, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //  Flat Measurement Position 저장
            for (int i = 0; i < (int)System.Enum.GetValues(typeof(FlatMeasureList)).Length; i++)
            {
                strTemp = string.Format("MeasureListType_{0}", i);

                for ( int j = 0; j < 9; j++ )
                {
                    //  Position X
                    strTemp2 = string.Format("Position_{0}_X", j + 1);
                    NativeMethods.WritePrivateProfileString(strTemp, strTemp2, Equipment.stFlatMeasurePos[i].StagePos[j].X.ToString(), strFIle);

                    //  Position Y
                    strTemp2 = string.Format("Position_{0}_Y", j + 1);
                    NativeMethods.WritePrivateProfileString(strTemp, strTemp2, Equipment.stFlatMeasurePos[i].StagePos[j].Y.ToString(), strFIle);
                }
            }

            MessageBox.Show("Flatness Measure Teaching 파일을 저장하였습니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

        private void button_Setup_Flatness_MeasurementPosition_Start_Click(object sender, EventArgs e)
        {
            //  Flatness 측정 시작

            Log.Write("SLD-200", Equipment.User_Name, "Button Click", "Flatness Measurement 시작 버튼");

            string m_strTemp = "";

            if (!workStage.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            if (workStage.m_nFlatnessMeasure_Step == (int)WorkStage.FlatnessMeasure_Step.None)
            {
                var mb = new MessageBoxYesNo();

                switch(workStage.m_nFlatnessMeasure_Type)
                {
                    case (int)FlatMeasureList.Stage:
                        m_strTemp = "[Stage] Flatness 측정을 시작하시겠습니까?";
                        break;

                    case (int)FlatMeasureList.CalPos:
                        m_strTemp = "[Cal. Plate] Flatness 측정을 시작하시겠습니까?";
                        break;

                    case (int)FlatMeasureList.User1:
                        m_strTemp = "[User1] Flatness 측정을 시작하시겠습니까?";
                        break;

                    case (int)FlatMeasureList.User2:
                        m_strTemp = "[User2] Flatness 측정을 시작하시겠습니까?";
                        break;

                    case (int)FlatMeasureList.User3:
                        m_strTemp = "[User3] Flatness 측정을 시작하시겠습니까?";
                        break;
                }

                if (DialogResult.Yes != mb.ShowDialog("Question ?", m_strTemp))
                    return;

                workStage.m_nFlatnessMeasure_Step = (int)WorkStage.FlatnessMeasure_Step.Start;
                workStage.timer_Comm.Enabled = true;
                workStage.timer_Comm.Start();
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "측정중입니다.");
                return;
            }
        }

        private void button_Setup_Flatness_MeasurementPosition_Stop_Click(object sender, EventArgs e)
        {
            //  Flatness 측정 중지

            Log.Write("SLD-200", Equipment.User_Name, "Button Click", "Flatness Measurement 중지 버튼");

            string m_strTemp = "";

            if (!workStage.m_bHomeOK)
            {
                var mb = new MessageBoxOk();
                mb.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            workStage.m_nFlatnessMeasure_Step = (int)WorkStage.FlatnessMeasure_Step.None;

            workStage.MC_Func.MC_MotorStop((int)WorkStage.nAxis.X, 2000);
            workStage.MC_Func.MC_MotorStop((int)WorkStage.nAxis.Y, 2000);

            var mb1 = new MessageBoxOk();
            mb1.ShowDialog("Information !", "측정이 중지되었습니다.");
            return;
        }

        private void button_Setup_Flatness_MeasurementPosition1_Get_Click(object sender, EventArgs e)
        {
            //  1번 위치 가져오기

            int nIndex = comboBox_Setup_FlatnessMeasurementPos_List.SelectedIndex;

            if (nIndex < 0)
            {
                MessageBox.Show("측정 위치를 선택하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            textBox_Setup_Flatness_TeachingPos1_StageX.Text = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X).ToString();
            textBox_Setup_Flatness_TeachingPos1_StageY.Text = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y).ToString();
        }

        private void button_Setup_Flatness_MeasurementPosition2_Get_Click(object sender, EventArgs e)
        {
            //  2번 위치 가져오기

            int nIndex = comboBox_Setup_FlatnessMeasurementPos_List.SelectedIndex;

            if (nIndex < 0)
            {
                MessageBox.Show("측정 위치를 선택하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            textBox_Setup_Flatness_TeachingPos2_StageX.Text = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X).ToString();
            textBox_Setup_Flatness_TeachingPos2_StageY.Text = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y).ToString();
        }

        private void button_Setup_Flatness_MeasurementPosition3_Get_Click(object sender, EventArgs e)
        {
            //  3번 위치 가져오기

            int nIndex = comboBox_Setup_FlatnessMeasurementPos_List.SelectedIndex;

            if (nIndex < 0)
            {
                MessageBox.Show("측정 위치를 선택하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            textBox_Setup_Flatness_TeachingPos3_StageX.Text = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X).ToString();
            textBox_Setup_Flatness_TeachingPos3_StageY.Text = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y).ToString();
        }

        private void button_Setup_Flatness_MeasurementPosition4_Get_Click(object sender, EventArgs e)
        {
            //  4번 위치 가져오기

            int nIndex = comboBox_Setup_FlatnessMeasurementPos_List.SelectedIndex;

            if (nIndex < 0)
            {
                MessageBox.Show("측정 위치를 선택하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            textBox_Setup_Flatness_TeachingPos4_StageX.Text = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X).ToString();
            textBox_Setup_Flatness_TeachingPos4_StageY.Text = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y).ToString();
        }

        private void button_Setup_Flatness_MeasurementPosition5_Get_Click(object sender, EventArgs e)
        {
            //  5번 위치 가져오기

            int nIndex = comboBox_Setup_FlatnessMeasurementPos_List.SelectedIndex;

            if (nIndex < 0)
            {
                MessageBox.Show("측정 위치를 선택하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            textBox_Setup_Flatness_TeachingPos5_StageX.Text = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X).ToString();
            textBox_Setup_Flatness_TeachingPos5_StageY.Text = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y).ToString();
        }

        private void button_Setup_Flatness_MeasurementPosition6_Get_Click(object sender, EventArgs e)
        {
            //  6번 위치 가져오기

            int nIndex = comboBox_Setup_FlatnessMeasurementPos_List.SelectedIndex;

            if (nIndex < 0)
            {
                MessageBox.Show("측정 위치를 선택하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            textBox_Setup_Flatness_TeachingPos6_StageX.Text = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X).ToString();
            textBox_Setup_Flatness_TeachingPos6_StageY.Text = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y).ToString();
        }

        private void button_Setup_Flatness_MeasurementPosition7_Get_Click(object sender, EventArgs e)
        {
            //  7번 위치 가져오기

            int nIndex = comboBox_Setup_FlatnessMeasurementPos_List.SelectedIndex;

            if (nIndex < 0)
            {
                MessageBox.Show("측정 위치를 선택하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            textBox_Setup_Flatness_TeachingPos7_StageX.Text = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X).ToString();
            textBox_Setup_Flatness_TeachingPos7_StageY.Text = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y).ToString();
        }

        private void button_Setup_Flatness_MeasurementPosition8_Get_Click(object sender, EventArgs e)
        {
            //  8번 위치 가져오기

            int nIndex = comboBox_Setup_FlatnessMeasurementPos_List.SelectedIndex;

            if (nIndex < 0)
            {
                MessageBox.Show("측정 위치를 선택하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            textBox_Setup_Flatness_TeachingPos8_StageX.Text = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X).ToString();
            textBox_Setup_Flatness_TeachingPos8_StageY.Text = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y).ToString();
        }

        private void button_Setup_Flatness_MeasurementPosition9_Get_Click(object sender, EventArgs e)
        {
            //  9번 위치 가져오기

            int nIndex = comboBox_Setup_FlatnessMeasurementPos_List.SelectedIndex;

            if (nIndex < 0)
            {
                MessageBox.Show("측정 위치를 선택하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            textBox_Setup_Flatness_TeachingPos9_StageX.Text = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X).ToString();
            textBox_Setup_Flatness_TeachingPos9_StageY.Text = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y).ToString();
        }

        private void button_Setup_Flatness_MeasurementPosition2_Clear_Click(object sender, EventArgs e)
        {
            //  2번 위치 Clear

            int nIndex = comboBox_Setup_FlatnessMeasurementPos_List.SelectedIndex;

            if (nIndex < 0)
            {
                MessageBox.Show("측정 위치를 선택하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            textBox_Setup_Flatness_TeachingPos2_StageX.Text = "0.0";
            textBox_Setup_Flatness_TeachingPos2_StageY.Text = "0.0";
        }

        private void button_Setup_Flatness_MeasurementPosition3_Clear_Click(object sender, EventArgs e)
        {
            //  3번 위치 Clear

            int nIndex = comboBox_Setup_FlatnessMeasurementPos_List.SelectedIndex;

            if (nIndex < 0)
            {
                MessageBox.Show("측정 위치를 선택하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            textBox_Setup_Flatness_TeachingPos3_StageX.Text = "0.0";
            textBox_Setup_Flatness_TeachingPos3_StageY.Text = "0.0";
        }

        private void button_Setup_Flatness_MeasurementPosition4_Clear_Click(object sender, EventArgs e)
        {
            //  4번 위치 Clear

            int nIndex = comboBox_Setup_FlatnessMeasurementPos_List.SelectedIndex;

            if (nIndex < 0)
            {
                MessageBox.Show("측정 위치를 선택하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            textBox_Setup_Flatness_TeachingPos4_StageX.Text = "0.0";
            textBox_Setup_Flatness_TeachingPos4_StageY.Text = "0.0";
        }

        private void button_Setup_Flatness_MeasurementPosition5_Clear_Click(object sender, EventArgs e)
        {
            //  5번 위치 Clear

            int nIndex = comboBox_Setup_FlatnessMeasurementPos_List.SelectedIndex;

            if (nIndex < 0)
            {
                MessageBox.Show("측정 위치를 선택하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            textBox_Setup_Flatness_TeachingPos5_StageX.Text = "0.0";
            textBox_Setup_Flatness_TeachingPos5_StageY.Text = "0.0";
        }

        private void button_Setup_Flatness_MeasurementPosition6_Clear_Click(object sender, EventArgs e)
        {
            //  6번 위치 Clear

            int nIndex = comboBox_Setup_FlatnessMeasurementPos_List.SelectedIndex;

            if (nIndex < 0)
            {
                MessageBox.Show("측정 위치를 선택하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            textBox_Setup_Flatness_TeachingPos6_StageX.Text = "0.0";
            textBox_Setup_Flatness_TeachingPos6_StageY.Text = "0.0";
        }

        private void button_Setup_Flatness_MeasurementPosition7_Clear_Click(object sender, EventArgs e)
        {
            //  7번 위치 Clear

            int nIndex = comboBox_Setup_FlatnessMeasurementPos_List.SelectedIndex;

            if (nIndex < 0)
            {
                MessageBox.Show("측정 위치를 선택하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            textBox_Setup_Flatness_TeachingPos7_StageX.Text = "0.0";
            textBox_Setup_Flatness_TeachingPos7_StageY.Text = "0.0";
        }

        private void button_Setup_Flatness_MeasurementPosition8_Clear_Click(object sender, EventArgs e)
        {
            //  8번 위치 Clear

            int nIndex = comboBox_Setup_FlatnessMeasurementPos_List.SelectedIndex;

            if (nIndex < 0)
            {
                MessageBox.Show("측정 위치를 선택하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            textBox_Setup_Flatness_TeachingPos8_StageX.Text = "0.0";
            textBox_Setup_Flatness_TeachingPos8_StageY.Text = "0.0";
        }

        private void button_Setup_Flatness_MeasurementPosition9_Clear_Click(object sender, EventArgs e)
        {
            //  9번 위치 Clear

            int nIndex = comboBox_Setup_FlatnessMeasurementPos_List.SelectedIndex;

            if (nIndex < 0)
            {
                MessageBox.Show("측정 위치를 선택하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            textBox_Setup_Flatness_TeachingPos9_StageX.Text = "0.0";
            textBox_Setup_Flatness_TeachingPos9_StageY.Text = "0.0";
        }

        private void btnOffsetStart_Vision_Click(object sender, EventArgs e)
        {
            string m_strTemp = "";

            //TEST
            //if (!workStage.m_bHomeOK)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
            //    return;
            //}

            if (Equipment.EqpSiriusViewer == null)
            {
                MessageBox.Show("먼저 Scanner Board 를 초기화 해야 합니다.", "Information!!");
                return;
            }

            if (workStage.rtc == null)
            {
                MessageBox.Show("먼저 Scanner Board 를 초기화 해야 합니다.", "Information!!");
                return;
            }

            //Equipment.Scanner_Calibration_Change;
            // 캘판 변경 유/무에 대해서 물어보는 메세지 박스해주고 True/False 리턴받기
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Scanner Calibration을 시작합니다.\n\n캘리브레이션 판이 변경되었습니까?"))
            {
                Equipment.Scanner_Calibration_Change = false;
            }
            else
            {
                Equipment.Scanner_Calibration_Change = true;
            }

            if (workStage.m_nScanner_Calibration_Step == (int)WorkStage.ScannerCalibration_Step.None)
            {
                Equipment.Scanner_Vision_Offset_Setting_Use = true;

                workStage.m_nScanner_Calibration_Step = (int)WorkStage.ScannerCalibration_Step.Start;
                workStage.timer_ScannerCalibration.Enabled = true;

                WorkStartTick = Environment.TickCount;
            }
        }

        private void btnOffsetApply_Click(object sender, EventArgs e)
        {
            Equipment.stOffsetDistance.FromScannerToFineCam.X += Equipment.Scanner_Vision_Offset_Setting_X;
            Equipment.stOffsetDistance.FromScannerToFineCam.Y += Equipment.Scanner_Vision_Offset_Setting_Y;

            Log.Write("SLD-200", "Button Click", "Offset Distance 가 적용되었습니다.\n\r\n" +
                "X : " + Equipment.stOffsetDistance.FromScannerToFineCam.X.ToString() + "\n\r\n" +
                "Y : " + Equipment.stOffsetDistance.FromScannerToFineCam.Y.ToString());
            MessageBox.Show("Offset Distance 가 적용되었습니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }


        

        private void button_Setup_ScannerCal_Train_Click(object sender, EventArgs e)
        {
            ScannerCompensatorRecipe recipe = workStage.scannerCompensator.Recipe; //m_Owner.Recipe;
            RoiTrain.Parameter.StartLocation = recipe.TrainRoiStartLocation;
            RoiTrain.Parameter.EndLocation = recipe.TrainRoiEndLocation;

            RoiInspect.Parameter.Overlay.Visible = false;
            RoiTrain.Parameter.Overlay.Visible = true;

            PatternMatchingResult result = workStage.scannerCompensator.GetResult();
            Box_Setup_ScannerCal_ImageViewer.NormalOverlays.Add(RoiTrain.Parameter.Overlay);
            foreach (var overlay in result.ResultOverlays)
            {
                Box_Setup_ScannerCal_ImageViewer.NormalOverlays.Remove(overlay);
            }
            Box_Setup_ScannerCal_ImageViewer.Display();

            this.m_RoiListControl.RoiTrainClickNew();
        }

        private void RoiTrainButtonClick(RoiVisionTool roiVisionTool)
        {
            if (workStage.scannerCompensator != null)
            {
                RoiTrain.Parameter.CenterLocation = roiVisionTool.Parameter.CenterLocation;
                RoiTrain.Parameter.Size = roiVisionTool.Parameter.Size;

                Box_Setup_ScannerCal_ImageViewer.NormalOverlays.Add(RoiTrain.Parameter.Overlay);
                RoiTrain.Parameter.Overlay.Visible = true;
                Box_Setup_ScannerCal_ImageViewer.Display();

            }
        }

        public void RoiInspectButtonClick(RoiVisionTool roiVisionTool)
        {
            if (workStage.scannerCompensator != null)
            {
                RoiInspect.Parameter.CenterLocation = roiVisionTool.Parameter.CenterLocation;
                RoiInspect.Parameter.Size = roiVisionTool.Parameter.Size;

                Box_Setup_ScannerCal_ImageViewer.NormalOverlays.Add(RoiInspect.Parameter.Overlay);
                RoiInspect.Parameter.Overlay.Visible = true;
                Box_Setup_ScannerCal_ImageViewer.Display();
            }
        }

        private void button_Setup_ScannerCal_Inspect_Click(object sender, EventArgs e)
        {
            ScannerCompensatorRecipe recipe = workStage.scannerCompensator.Recipe;
            RoiInspect.Parameter.StartLocation = recipe.InspectRoiStartLocation;
            RoiInspect.Parameter.EndLocation = recipe.InspectRoiEndLocation;

            RoiTrain.Parameter.Overlay.Visible = false;
            RoiInspect.Parameter.Overlay.Visible = true;

            PatternMatchingResult result = workStage.scannerCompensator.GetResult();
            Box_Setup_ScannerCal_ImageViewer.NormalOverlays.Add(RoiInspect.Parameter.Overlay);
            foreach (var overlay in result.ResultOverlays)
            {
                Box_Setup_ScannerCal_ImageViewer.NormalOverlays.Remove(overlay);
            }
            Box_Setup_ScannerCal_ImageViewer.Display();

            this.m_RoiListControl.RoiAlignClickNew();
        }

        private void RoiTrainSaveButtonClick(RoiVisionTool roiVisionTool, bool bOk)
        {
            ScannerCompensatorRecipe recipe = workStage.scannerCompensator.Recipe;

            if (!bOk)
            {
                RoiTrain.Parameter.StartLocation = recipe.TrainRoiStartLocation;
                RoiTrain.Parameter.EndLocation = recipe.TrainRoiEndLocation;
                RoiTrain.Parameter.Overlay.Visible = false;
                Box_Setup_ScannerCal_ImageViewer.Display();
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
            SetTrainImage(workStage.scannerCompensator.Recipe.PatternMatchingParameter.TrainImage);

            Box_Setup_ScannerCal_ImageViewer.Display();

            Equipment.Scanner_Calibration_TrainRoiStartLocation_X = RoiTrain.Parameter.StartLocation.X;
            Equipment.Scanner_Calibration_TrainRoiStartLocation_Y = RoiTrain.Parameter.StartLocation.Y;
            Equipment.Scanner_Calibration_TrainRoiEndLocation_X = RoiTrain.Parameter.EndLocation.X;
            Equipment.Scanner_Calibration_TrainRoiEndLocation_Y = RoiTrain.Parameter.EndLocation.Y;

            Equipment.SaveRecipe();
        }

        private void RoiInspectSaveButtonClick(RoiVisionTool roiVisionTool, bool bOk)
        {
            ScannerCompensatorRecipe recipe = workStage.scannerCompensator.Recipe;
            if (!bOk)
            {
                RoiInspect.Parameter.StartLocation = recipe.InspectRoiStartLocation;
                RoiInspect.Parameter.EndLocation = recipe.InspectRoiEndLocation;
                Box_Setup_ScannerCal_ImageViewer.Display();
                return;
            }
            RoiInspect = roiVisionTool;
            recipe.InspectRoiStartLocation = RoiInspect.Parameter.StartLocation;
            recipe.InspectRoiEndLocation = RoiInspect.Parameter.EndLocation;

            //Box_Setup_ScannerCal_ImageViewer.Display();
            //if (m_JogControl != null)
            //{
            //    m_JogControl.Show();
            //    m_JogControl.BringToFront();
            //}
            //m_TrainImageControl.Show();
            Box_Setup_ScannerCal_ImageViewer.Display();

            Equipment.Scanner_Calibration_InspectionRoiStartLocation_X = RoiInspect.Parameter.StartLocation.X;
            Equipment.Scanner_Calibration_InspectionRoiStartLocation_Y = RoiInspect.Parameter.StartLocation.Y;
            Equipment.Scanner_Calibration_InspectionRoiEndLocation_X = RoiInspect.Parameter.EndLocation.X;
            Equipment.Scanner_Calibration_InspectionRoiEndLocation_Y = RoiInspect.Parameter.EndLocation.Y;

            Equipment.SaveRecipe();
        }

        private void button_Setup_ScannerCal_Train_Set_Click(object sender, EventArgs e)
        {
            if (workStage.scannerCompensator != null)
            {
                RoiVisionTool roiVisionTool = m_RoiListControl.RoiTrainVisionTool;

                RoiTrain.Parameter.CenterLocation = roiVisionTool.Parameter.CenterLocation;
                RoiTrain.Parameter.Size = roiVisionTool.Parameter.Size;

                Box_Setup_ScannerCal_ImageViewer.NormalOverlays.Add(RoiTrain.Parameter.Overlay);
                RoiTrain.Parameter.Overlay.Visible = true;
                Box_Setup_ScannerCal_ImageViewer.Display();
            }

            string m_strFile = "";
            string m_strRecipe = "";
            RecipeInfo m_recipeInfo = new RecipeInfo();
            m_recipeInfo = Equipment.GetCurrentRecipe();

            //workStage.Camera_HighRes.LatestImage = Box_Setup_ScannerCal_ImageViewer.InputImage;
            workStage.scannerCompensator.Camera.LatestImage = Box_Setup_ScannerCal_ImageViewer.InputImage;

            if (Box_Setup_ScannerCal_ImageViewer.Simulated)
            {
                workStage.scannerCompensator.Simulated = true;
                workStage.scannerCompensator.TestImage = Box_Setup_ScannerCal_ImageViewer.InputImage;
            }
            else
            {
                workStage.scannerCompensator.Simulated = false;
            }

            workStage.scannerCompensator.Train();
            SetTrainImage(workStage.scannerCompensator.TrainImage);

            //  train 할 때 레시피를 저장해줘야 정상적으로 패턴 이미지가 변경된다.
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

                //workStage.PatternMatchingImage_Reticle_Loaded_HighRes = true;
                //workStage.m_bLowerCam_AlignPattern_Reset = true;
                //workStage.scannerCompensator.IlluminationData.SetIlluminationChannel(0);
            }
        }


        private void InitPatternMatchingParameter()
        {
            if (PatternMatchingParameter != null)
            {
                basetextBox_Setup_ScannerCal_AngleTolerance.Text = Equipment.Scanner_Calibration_PatternMatchingParameters.MaxTolerance.ToString();
                PatternMatchingParameter.MaxTolerance = Equipment.Scanner_Calibration_PatternMatchingParameters.MaxTolerance;

                basetextBox_Setup_ScannerCal_MaxInstance.Text = Equipment.Scanner_Calibration_PatternMatchingParameters.MaxInstance.ToString();
                PatternMatchingParameter.MaxInstance = Equipment.Scanner_Calibration_PatternMatchingParameters.MaxInstance;

                basetextBox_Setup_ScannerCal_MinScore.Text = Equipment.Scanner_Calibration_PatternMatchingParameters.MinScore.ToString();
                PatternMatchingParameter.MinScore = Equipment.Scanner_Calibration_PatternMatchingParameters.MinScore;

                bool bOn = Equipment.Scanner_Calibration_PatternMatchingParameters.DuplicateChecked;
                baseToggleButton_Setup_ScannerCal_DuplicateCheck.UpdateToggleStatus(bOn);
                PatternMatchingParameter.DuplicateChecked = bOn;

                bOn = Equipment.Scanner_Calibration_PatternMatchingParameters.UseMaskImage;
                baseToggleButton_Setup_ScannerCal_UseMaskImage.UpdateToggleStatus(bOn);
                PatternMatchingParameter.UseMaskImage = bOn;
            }
        }

        private void InitBlobParameter()
        {
            if (BlobParameter != null)
            {
                string str;
                str = Equipment.Scanner_Calibration_BlobVisionToolParameter.HardThreshold.ToString();
                BlobParameter.HardThreshold = Equipment.Scanner_Calibration_BlobVisionToolParameter.HardThreshold;

                str = Equipment.Scanner_Calibration_BlobVisionToolParameter.MinPixels.ToString();
                BlobParameter.MinPixels = Equipment.Scanner_Calibration_BlobVisionToolParameter.MinPixels;

                str = Equipment.Scanner_Calibration_BlobVisionToolParameter.RepeatCount.ToString();
                BlobParameter.RepeatCount = Equipment.Scanner_Calibration_BlobVisionToolParameter.RepeatCount;

                str = Equipment.Scanner_Calibration_BlobVisionToolParameter.Polarity.ToString();
                BlobParameter.Polarity = Equipment.Scanner_Calibration_BlobVisionToolParameter.Polarity;

                str = Equipment.Scanner_Calibration_BlobVisionToolParameter.HasChanged.ToString();
                BlobParameter.HasChanged = Equipment.Scanner_Calibration_BlobVisionToolParameter.HasChanged;
            }
        }

        public void SetPatternMatchingData(PatternMatchingParameters parameter)
        {
            PatternMatchingParameter = parameter;
            InitPatternMatchingParameter();
        }
        private void UpdateOwnerRecipe(PatternMatchingParameters parameters)
        {
            if (workStage.scannerCompensator is ScannerCompensator)
            {
                ScannerCompensator visionCompensator = workStage.scannerCompensator as ScannerCompensator;
                visionCompensator.Recipe.PatternMatchingParameter = parameters;
                workStage.scannerCompensator = visionCompensator;
            }
            //Equipment.SaveRecipe();
        }

        private void button_Setup_ScannerCal_Search_Click(object sender, EventArgs e)
        {
            UpdataPositionData(0.0, 0.0, 0.0);

            if (Box_Setup_ScannerCal_ImageViewer.Simulated)
            {
                workStage.scannerCompensator.Simulated = true;
                workStage.scannerCompensator.Camera.LatestImage = Box_Setup_ScannerCal_ImageViewer.InputImage;
                workStage.scannerCompensator.TestImage = Box_Setup_ScannerCal_ImageViewer.InputImage;
            }
            else
            {
                workStage.scannerCompensator.Simulated = false;
            }

            if (Equipment.Scanner_Calibration_UseBlobVisionTool)
            {
                if (IsPixel == true)
                {
                    PatternMatchingParameter.MaxTolerance = 0;
                    PatternMatchingParameter.MaxInstance = Equipment.ToInt(basetextBox_Setup_ScannerCal_MaxInstance.Text);
                    PatternMatchingParameter.MinTolerance = 0;
                    PatternMatchingParameter.MinScore = Equipment.ToDouble(basetextBox_Setup_ScannerCal_MinScore.Text);
                    PatternMatchingParameter.DuplicateChecked = baseToggleButton_Setup_ScannerCal_DuplicateCheck.GetButtonStatus();
                    PatternMatchingParameter.UseMaskImage = baseToggleButton_Setup_ScannerCal_UseMaskImage.GetButtonStatus();

                    PatternMatchingResult result = workStage.scannerCompensator.GetResult();
                    if (result != null)
                    {
                        Box_Setup_ScannerCal_ImageViewer.ResultOverlays.Clear();
                    }

                    result = workStage.scannerCompensator.Search();
                    if (result != null)
                    {
                        foreach (var overlay in result.ResultOverlays)
                        {
                            Box_Setup_ScannerCal_ImageViewer.ResultOverlays.Add(overlay);
                            //overlay.Visible = true;
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
                    if (Box_Setup_ScannerCal_ImageViewer.Simulated)
                    {
                        qip.FindCirclesWidthCircleBoundary(Fiducial_circlesResult, Box_Setup_ScannerCal_ImageViewer.InputImage.RawData
                            , Box_Setup_ScannerCal_ImageViewer.InputImage.Header.Width
                            , Box_Setup_ScannerCal_ImageViewer.InputImage.Header.Height
                            , (int)pixelR, 0.5, ref bFind, (int)result.Values[0].X, (int)result.Values[0].Y);
                        //1000, 1 -> 엄청느린값 // 원의 반지름의 값이랑 오차범위
                        //센터점 전달해서 찾기로, 센터 못찾으면 그냥 센터로. 
                    }
                    else
                    {
                        qip.FindCirclesWidthCircleBoundary(Fiducial_circlesResult, workStage.scannerCompensator.Camera.LatestImage.RawData
                            , workStage.scannerCompensator.Camera.LatestImage.Header.Width
                            , workStage.scannerCompensator.Camera.LatestImage.Header.Height
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

                        Box_Setup_ScannerCal_ImageViewer.ResultOverlays.Add(overlay);
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
                PatternMatchingParameter.MaxTolerance = Equipment.ToInt(basetextBox_Setup_ScannerCal_AngleTolerance.Text);
                PatternMatchingParameter.MaxInstance = Equipment.ToInt(basetextBox_Setup_ScannerCal_MaxInstance.Text);
                PatternMatchingParameter.MinTolerance = Equipment.ToInt(basetextBox_Setup_ScannerCal_AngleTolerance.Text) * -1;
                PatternMatchingParameter.MinScore = Equipment.ToDouble(basetextBox_Setup_ScannerCal_MinScore.Text);
                PatternMatchingParameter.DuplicateChecked = baseToggleButton_Setup_ScannerCal_DuplicateCheck.GetButtonStatus();
                PatternMatchingParameter.UseMaskImage = baseToggleButton_Setup_ScannerCal_UseMaskImage.GetButtonStatus();

                workStage.scannerCompensator.Recipe.PatternMatchingParameter = PatternMatchingParameter;

                PatternMatchingResult result = workStage.scannerCompensator.GetResult();
                if (result != null)
                {
                    Box_Setup_ScannerCal_ImageViewer.ResultOverlays.Clear();
                }

                result = workStage.scannerCompensator.Search();
                if (result != null)
                {
                    foreach (var overlay in result.ResultOverlays)
                    {
                        Box_Setup_ScannerCal_ImageViewer.ResultOverlays.Add(overlay);
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
                Box_Setup_ScannerCal_ImageViewer.Display();
                UpdateOwnerRecipe(PatternMatchingParameter);
            }

            workStage.scannerCompensator.Camera.StartLive();
        }

        private void baseToggleButton_Setup_ScannerCal_DuplicateCheck_Click(object sender, EventArgs e)
        {
            bool bOn = baseToggleButton_Setup_ScannerCal_DuplicateCheck.GetButtonStatus();
            if (bOn)
            {
                baseToggleButton_Setup_ScannerCal_DuplicateCheck.UpdateToggleStatus(false);
            }
            else
            {
                baseToggleButton_Setup_ScannerCal_DuplicateCheck.UpdateToggleStatus(true);
            }
        }

        private void baseToggleButton_Setup_ScannerCal_UseMaskImage_Click(object sender, EventArgs e)
        {
            bool bOn = baseToggleButton_Setup_ScannerCal_UseMaskImage.GetButtonStatus();
            if (bOn)
            {
                baseToggleButton_Setup_ScannerCal_UseMaskImage.UpdateToggleStatus(false);
            }
            else
            {
                baseToggleButton_Setup_ScannerCal_UseMaskImage.UpdateToggleStatus(true);
            }
        }

        public void UpdataPositionData(double x, double y, double t, double circlex = 0.0, double circley = 0.0)
        {
            baseTextBox_Setup_ScannerCal_PositionX.Text = x.ToString();
            baseTextBox_Setup_ScannerCal_PositionY.Text = y.ToString();
            baseTextBox_Setup_ScannerCal_PositionT.Text = t.ToString();

            if (checkBox_Setup_ScannerCal_CirclePos.Checked)
            {
                label_Setup_ScannerCal_CirclePosX.Text = circlex.ToString();
                label_Setup_ScannerCal_CirclePosY.Text = circley.ToString();
            }
            else
            {
                label_Setup_ScannerCal_CirclePosX.Text = "---";
                label_Setup_ScannerCal_CirclePosY.Text = "---";
            }
            
            baseTextBox_Setup_ScannerCal_PositionX.Refresh();
            baseTextBox_Setup_ScannerCal_PositionY.Refresh();
            baseTextBox_Setup_ScannerCal_PositionT.Refresh();
            label_Setup_ScannerCal_CirclePosX.Refresh();
            label_Setup_ScannerCal_CirclePosY.Refresh();

        }
        public void UpdataPositionData(double x, double y)
        {
            baseTextBox_Setup_ScannerCal_PositionX.Text = x.ToString();
            baseTextBox_Setup_ScannerCal_PositionY.Text = y.ToString();
        }

        public void UpdataPositionData(double x, double y, double t, double s)
        {
            baseTextBox_Setup_ScannerCal_PositionX.Text = x.ToString();
            baseTextBox_Setup_ScannerCal_PositionY.Text = y.ToString();
            baseTextBox_Setup_ScannerCal_PositionT.Text = t.ToString();
            //m_TextBoxScore.Text = s.ToString();
        }

        private void hScrollBarIlluminator_ValueChanged(object sender, System.EventArgs e)
        {
            //  고해상도 카메라에는 Red Ring 과 IR 조명이 달려 있음
            if (radioButton_Setup_ScannerCal_Light_IR.Checked)
            {
                workStage.Config.ListIlluminationChannel[1].Value = hScrollBar_Setup_ScannerCal_Illuminator.Value;
                this.textBox_Setup_ScannerCal_IlluminationValue.Text = hScrollBar_Setup_ScannerCal_Illuminator.Value.ToString();
                CommonModule.Instance.Illuminator.SetVolume(this.hScrollBar_Setup_ScannerCal_Illuminator.Value, 2);

                workStage.scannerCompensator.Illuminator.SetVolume(workStage.Config.ListIlluminationChannel[1].Value, 2);
                
            }
            else        //  Red Ring
            {
                workStage.Config.ListIlluminationChannel[0].Value = hScrollBar_Setup_ScannerCal_Illuminator.Value;
                this.textBox_Setup_ScannerCal_IlluminationValue.Text = hScrollBar_Setup_ScannerCal_Illuminator.Value.ToString();
                CommonModule.Instance.Illuminator.SetVolume(this.hScrollBar_Setup_ScannerCal_Illuminator.Value, 1);

                workStage.scannerCompensator.Illuminator.SetVolume(workStage.Config.ListIlluminationChannel[0].Value, 1);
                
            }

            this.textBox_Setup_ScannerCal_IlluminationValue.Refresh();
        }

        private void SetScroll(int nChannel)
        {
            hScrollBar_Setup_ScannerCal_Illuminator.Minimum = (int)workStage.Config.ListIlluminationChannel[nChannel].Min;
            hScrollBar_Setup_ScannerCal_Illuminator.Maximum = (int)workStage.Config.ListIlluminationChannel[nChannel].Max;

            baseLabel_Setup_ScannerCal_Min.Text = hScrollBar_Setup_ScannerCal_Illuminator.Minimum.ToString();
            baseLabel_Setup_ScannerCal_Max.Text = hScrollBar_Setup_ScannerCal_Illuminator.Maximum.ToString();
        }

        private void radioButton_Setup_ScannerCal_Light_Red_CheckedChanged(object sender, EventArgs e)
        {
            SetScroll(0);

            //  조명값 변경
            CommonModule.Instance.Illuminator.SetVolume(workStage.Config.ListIlluminationChannel[0].Value, 1);
            CommonModule.Instance.Illuminator.TurnOnOff(true, 1);
            CommonModule.Instance.Illuminator.SetVolume(workStage.Config.ListIlluminationChannel[1].Value, 2);
            CommonModule.Instance.Illuminator.TurnOnOff(true, 2);

            hScrollBar_Setup_ScannerCal_Illuminator.Value = workStage.Config.ListIlluminationChannel[0].Value;                //  저해상도 카메라 IR 조명 (3번, Index 는 2번)
            this.textBox_Setup_ScannerCal_IlluminationValue.Text = hScrollBar_Setup_ScannerCal_Illuminator.Value.ToString();

            //  Low Mag Camera 조명 끄기
            CommonModule.Instance.Illuminator.TurnOnOff(false, 3);
        }

        private void radioButton_Setup_ScannerCal_Light_IR_CheckedChanged(object sender, EventArgs e)
        {
            SetScroll(1);

            //  조명값 변경
            CommonModule.Instance.Illuminator.SetVolume(workStage.Config.ListIlluminationChannel[1].Value, 2);
            CommonModule.Instance.Illuminator.TurnOnOff(true, 2);
            CommonModule.Instance.Illuminator.SetVolume(workStage.Config.ListIlluminationChannel[0].Value, 1);
            CommonModule.Instance.Illuminator.TurnOnOff(true, 1);

            hScrollBar_Setup_ScannerCal_Illuminator.Value = workStage.Config.ListIlluminationChannel[1].Value;                //  고해상도 카메라 IR 조명 (2번, Index 는 1번)
            this.textBox_Setup_ScannerCal_IlluminationValue.Text = hScrollBar_Setup_ScannerCal_Illuminator.Value.ToString();

            //  Low Mag Camera 조명 끄기
            CommonModule.Instance.Illuminator.TurnOnOff(false, 3);
        }
        public void SetTrainImage(VisionImage image)
        {
            this.pictureBox_Setup_ScannerCal_TrainImage.Image = image.GetImage();
            this.pictureBox_Setup_ScannerCal_TrainImage.BringToFront();
        }

        private void button_Setup_ScannerCal_Vision_Save_Click(object sender, EventArgs e)
        {
            Equipment.Scanner_Calibration_Illumination_channel_01_Value = workStage.Config.ListIlluminationChannel[0].Value; //RED
            Equipment.Scanner_Calibration_Illumination_channel_02_Value = workStage.Config.ListIlluminationChannel[1].Value; //IR

            //workStage.scannerCompensator.IlluminationData

            //Equipment.Scanner_Calibration_AngleTolerance = Equipment.ToDouble(basetextBox_Setup_ScannerCal_AngleTolerance.Text);
            //Equipment.Scanner_Calibration_MaxInstance = Equipment.ToDouble(basetextBox_Setup_ScannerCal_MaxInstance.Text);
            //Equipment.Scanner_Calibration_MinScore = Equipment.ToDouble(basetextBox_Setup_ScannerCal_MinScore.Text);
            //Equipment.Scanner_Calibration_DuplicateCheck = baseToggleButton_Setup_ScannerCal_DuplicateCheck.GetButtonStatus();
            //Equipment.Scanner_Calibration_UseMaskImage = baseToggleButton_Setup_ScannerCal_UseMaskImage.GetButtonStatus();

            Equipment.Scanner_Calibration_TrainRoiStartLocation_X = RoiTrain.Parameter.StartLocation.X;
            Equipment.Scanner_Calibration_TrainRoiStartLocation_Y = RoiTrain.Parameter.StartLocation.Y;
            Equipment.Scanner_Calibration_TrainRoiEndLocation_X = RoiTrain.Parameter.EndLocation.X;
            Equipment.Scanner_Calibration_TrainRoiEndLocation_Y = RoiTrain.Parameter.EndLocation.Y;
            Equipment.Scanner_Calibration_InspectionRoiStartLocation_X = RoiInspect.Parameter.StartLocation.X;
            Equipment.Scanner_Calibration_InspectionRoiStartLocation_Y = RoiInspect.Parameter.StartLocation.Y;
            Equipment.Scanner_Calibration_InspectionRoiEndLocation_X = RoiInspect.Parameter.EndLocation.X;
            Equipment.Scanner_Calibration_InspectionRoiEndLocation_Y = RoiInspect.Parameter.EndLocation.Y;

            Equipment.Scanner_Calibration_PatternMatchingParameters.MaxInstance = Equipment.ToInt(basetextBox_Setup_ScannerCal_MaxInstance.Text);
            Equipment.Scanner_Calibration_PatternMatchingParameters.MaxTolerance = Equipment.ToDouble(basetextBox_Setup_ScannerCal_AngleTolerance.Text);
            Equipment.Scanner_Calibration_PatternMatchingParameters.MinScore = Equipment.ToDouble(basetextBox_Setup_ScannerCal_MinScore.Text);
            Equipment.Scanner_Calibration_PatternMatchingParameters.DuplicateChecked = baseToggleButton_Setup_ScannerCal_DuplicateCheck.GetButtonStatus();
            Equipment.Scanner_Calibration_PatternMatchingParameters.UseMaskImage = baseToggleButton_Setup_ScannerCal_UseMaskImage.GetButtonStatus();

            //설정 GUI 필요하네..(BlobVisionToolParameter)
            Equipment.Scanner_Calibration_BlobVisionToolParameter.HardThreshold = BlobParameter.HardThreshold;
            Equipment.Scanner_Calibration_BlobVisionToolParameter.MinPixels = BlobParameter.MinPixels;
            Equipment.Scanner_Calibration_BlobVisionToolParameter.Polarity = BlobParameter.Polarity;
            Equipment.Scanner_Calibration_BlobVisionToolParameter.RepeatCount = BlobParameter.RepeatCount;
            Equipment.Scanner_Calibration_BlobVisionToolParameter.HasChanged = BlobParameter.HasChanged;


            workStage.scannerCompensator.Recipe.PatternMatchingParameter = PatternMatchingParameter;
            workStage.scannerCompensator.Recipe.BlobParameter = BlobParameter;

            workStage.Scanner_Calibration_Vision_Save();
        }

        private void radioButton_Setup_ScannerCal_Cross_CheckedChanged(object sender, EventArgs e)
        {
            Equipment.Scanner_Calibration_MarkType_Cross = true;
            Equipment.Scanner_Calibration_MarkType_Circlle = false;
        }

        private void radioButton_Setup_ScannerCal_Circle_CheckedChanged(object sender, EventArgs e)
        {
            Equipment.Scanner_Calibration_MarkType_Cross = false;
            Equipment.Scanner_Calibration_MarkType_Circlle = true;
        }

        private void radioButton_Setup_ScannerCal_Pattern_CheckedChanged(object sender, EventArgs e)
        {
            Equipment.Scanner_Calibration_UsePatternMatching = true;
            Equipment.Scanner_Calibration_UseBlobVisionTool = false;
        }

        private void radioButton_Setup_ScannerCal_Blob_CheckedChanged(object sender, EventArgs e)
        {
            Equipment.Scanner_Calibration_UsePatternMatching = false;
            Equipment.Scanner_Calibration_UseBlobVisionTool = true;
        }

        private void radioButton_Setup_ScannerCal_Pixel_CheckedChanged(object sender, EventArgs e)
        {
            IsPixel = true;
        }

        private void radioButton_Setup_ScannerCal_mm_CheckedChanged(object sender, EventArgs e)
        {
            IsPixel = false;
        }

        private void button_Setup_ScannerCal_Rtc6_Cal_File_Load_Click(object sender, EventArgs e)
        {
            var mb = new QMC.Common.UI.MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "신규 ct5 파일\n\n적용하시겠습니까?"))
            {
                return;
            }

            bool bRtn = LoadCorrectionData(0, m_targetFile);
            if (bRtn)
            {
                Machine_ScannerCalibration_Save();
            }
        }

        private void button_Setup_ScannerCal_CameraLive_Click(object sender, EventArgs e)
        {
            //  카메라 연결 확인
            //if (!workStage.Camera_HighRes.Opened ||
            //    !workStage.Camera_LowRes.Opened)
            if (!workStage.scannerCompensator.Camera.Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 카메라를 연결해야 해야 합니다.");
                return;
            }

            Box_Setup_ScannerCal_ImageViewer.Simulated = false;
            workStage.scannerCompensator.Simulated = false;

            this.Box_Setup_ScannerCal_ImageViewer.StartUpdateTask();
            this.Box_Setup_ScannerCal_ImageViewer.Visible = true;

            //if (workStage.Camera_HighRes != null)
            if (workStage.scannerCompensator.Camera != null)
            {
                //workStage.Camera_HighRes.StartLive();
                workStage.scannerCompensator.Camera.StartLive();
            }

        }

        private void button_Setup_ScannerCal_CameraStop_Click(object sender, EventArgs e)
        {
            //if (!workStage.Camera_HighRes.Opened ||
            //    !workStage.Camera_LowRes.Opened)
            if (!workStage.scannerCompensator.Camera.Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 카메라를 연결해야 해야 합니다.");
                return;
            }

            this.Box_Setup_ScannerCal_ImageViewer.StopUpdateTask();
            //this.Box_Setup_ScannerCal_ImageViewer.Visible = false;

            //if (workStage.Camera_HighRes != null)
            if (workStage.scannerCompensator.Camera != null)
            {
                //workStage.Camera_HighRes.StopLive();
                workStage.scannerCompensator.Camera.StopLive();
            }
        }

        private void checkBox_Setup_Option_VacuumBlowTime_Enable_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Setup_Option_VacuumBlowTime_Enable.Checked)
            {
                Equipment.Machine_VacuumBlowTime_Enable = true;
                textBox_Setup_Option_VacuumBlowTime.Enabled = true;
            }
            else
            {
                Equipment.Machine_VacuumBlowTime_Enable = false;
                textBox_Setup_Option_VacuumBlowTime.Enabled = false;
            }
        }
    }
}
