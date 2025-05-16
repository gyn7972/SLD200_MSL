using netDxf.Entities;
using QMC.Common;
using QMC.Common.Modules;
using QMC.Common.Motion.ACS.Motions;
using QMC.Common.Parts;
using QMC.Common.Vision.Optics;
using QMC.Common.Vision.Tools;
using QMC.Common.VisionPart;
using QMC.Core;
using SLD200_MSL;
using SpiralLab.Sirius;
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
using static QMC.Common.Equipment;
using static QMC.Common.Modules.Vision;
using static QMC.Common.Modules.WorkStage;
using Bitmap = System.Drawing.Bitmap;
using Image = System.Drawing.Image;
using Rectangle = System.Drawing.Rectangle;

namespace SLD200_MSL
{
    public partial class FormNew_VisionPopup : Form
    {
        private static FormNew_VisionPopup m_formVisionPopup = null;

        static WorkStage workStage;
        static Vision vision;

        private XyCoordinate xyInterpolatedCoordinate = new XyCoordinate();

        private Bitmap bm_Temp;
        private byte[] bm_RawData;
        private List<Rectangle> detectedCircles;

        public System.Windows.Forms.Timer timer_Status;

        private int m_nStageSwitchingMove_Index;            //  0: Low Mag Cam,     1: High Mag Cam,,   2: Process,     3: Calibration

        public FormNew_VisionPopup()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;

            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;

            foreach (Module module in m_collectionModules)
            {
                //if (module.Name == "WorkStage")
                if (module.Name == "WorkStage")
                {
                    workStage = module as WorkStage;
                }

                if (module.Name == "Vision")
                {
                    vision = module as Vision;
                }
            }

            this.m_visionImageViewer_HighRes.SizeMode = PictureBoxSizeMode.CenterImage;
            this.m_visionImageViewer_HighRes.SuspendDisplay();
            this.m_visionImageViewer_HighRes.Camera = workStage.Camera_HighRes;

            this.m_visionImageViewer_LowRes.SizeMode = PictureBoxSizeMode.CenterImage;
            this.m_visionImageViewer_LowRes.SuspendDisplay();
            this.m_visionImageViewer_LowRes.Camera = workStage.Camera_LowRes;

            this.hScrollBarIlluminator_IR.ValueChanged += new System.EventHandler(this.hScrollBarIlluminator_IR_ValueChanged);
            this.hScrollBarIlluminator_Red.ValueChanged += new System.EventHandler(this.hScrollBarIlluminator_Red_ValueChanged);


            SetScroll((int)WorkStage.CameraType.CAMERA_HIGH);

            //  Status 타이머
            timer_Status = new System.Windows.Forms.Timer();
            timer_Status.Interval = 20;
            timer_Status.Tick += new System.EventHandler(Timer_Status_Func);

            detectedCircles = new List<Rectangle>(); // 사각형을 저장할 필드

            m_nStageSwitchingMove_Index = 0;            //  시작은 Low Mag Camera

            radioButton_VisionPopup_Move_MoveMode_Fine.Checked = false;
            radioButton_VisionPopup_Move_MoveMode_Coarse.Checked = true;
        }

        public FormNew_VisionPopup CreateSiriusEditor()
        {
            if (m_formVisionPopup == null)
            {
                m_formVisionPopup = new FormNew_VisionPopup();
            }

            return m_formVisionPopup;
        }

        public void Socket_List_Set()
        {
            if (workStage.m_stDividedRegion_GroupData != null)
            {
                comboBox_Config_VisionPopup_AlignTest_SocketList.Items.Clear();

                for (int i = 0; i < workStage.m_stDividedRegion_GroupData[0].nGroup_Num; i++)
                {
                    comboBox_Config_VisionPopup_AlignTest_SocketList.Items.Add(i);
                }
            }
            else
            {
                comboBox_Config_VisionPopup_AlignTest_SocketList.Items.Clear();
            }
        }

        private void hScrollBarIlluminator_IR_ValueChanged(object sender, System.EventArgs e)
        {
            //  선택된 카메라에 따라 조명값 변경
            if (radioButton_VisionPopup_CameraSelection_LowMag.Checked)
            {
                //  저해상도 카메라에는 IR 한개 달려 있음
                workStage.Config.ListIlluminationChannel[2].Value = hScrollBarIlluminator_IR.Value;
                this.textBox_IlluminationValue_IR.Text = hScrollBarIlluminator_IR.Value.ToString();
                CommonModule.Instance.Illuminator.SetVolume(this.hScrollBarIlluminator_IR.Value, 3);
            }
            else
            {
                workStage.Config.ListIlluminationChannel[1].Value = hScrollBarIlluminator_IR.Value;
                this.textBox_IlluminationValue_IR.Text = hScrollBarIlluminator_IR.Value.ToString();
                CommonModule.Instance.Illuminator.SetVolume(this.hScrollBarIlluminator_IR.Value, 2);
            }
        }

        private void hScrollBarIlluminator_Red_ValueChanged(object sender, System.EventArgs e)
        {
            workStage.Config.ListIlluminationChannel[0].Value = hScrollBarIlluminator_Red.Value;
            this.textBox_IlluminationValue_Red.Text = hScrollBarIlluminator_Red.Value.ToString();
            CommonModule.Instance.Illuminator.SetVolume(this.hScrollBarIlluminator_Red.Value, 1);
        }

        private void Timer_Status_Func(object sender, EventArgs e)
        {
            //  동시에 진행되지 않는 함수들만 동일한 타이머로 한다.

            timer_Status.Enabled = false;

            Motion_Status();

            if (workStage.m_bLaserHeightCheck_Complete)
            {
                workStage.m_bLaserHeightCheck_Complete = false;
                textBox_VisionPopup_LaserHeightValue.Text = string.Format("{0:0.000}", workStage.m_dLaserHeightCheck_Value);
            }
            
            timer_Status.Enabled = true;
        }

        private void Motion_Status()
        {
            //  Limit
            if (Equipment.AjinBoard_Opened)
            {
                //  Vision
                if (vision.MC_Func.MC_isLimit_Neg((int)Vision.nAxis.X))
                {
                    button_VisionPopup_X_Neg.BackColor = Color.Red;
                    button_VisionPopup_X_Neg.ForeColor = Color.White;
                }
                else
                {
                    button_VisionPopup_X_Neg.BackColor = Color.White;
                    button_VisionPopup_X_Neg.ForeColor = Color.Black;
                }

                if (vision.MC_Func.MC_isLimit_Pos((int)Vision.nAxis.X))
                {
                    button_VisionPopup_X_Pos.BackColor = Color.Red;
                    button_VisionPopup_X_Pos.ForeColor = Color.White;
                }
                else
                {
                    button_VisionPopup_X_Pos.BackColor = Color.White;
                    button_VisionPopup_X_Pos.ForeColor = Color.Black;
                }

                if (vision.MC_Func.MC_isLimit_Neg((int)Vision.nAxis.Y))
                {
                    button_VisionPopup_Y_Neg.BackColor = Color.Red;
                    button_VisionPopup_Y_Neg.ForeColor = Color.White;
                }
                else
                {
                    button_VisionPopup_Y_Neg.BackColor = Color.White;
                    button_VisionPopup_Y_Neg.ForeColor = Color.Black;
                }

                if (vision.MC_Func.MC_isLimit_Pos((int)Vision.nAxis.Y))
                {
                    button_VisionPopup_Y_Pos.BackColor = Color.Red;
                    button_VisionPopup_Y_Pos.ForeColor = Color.White;
                }
                else
                {
                    button_VisionPopup_Y_Pos.BackColor = Color.White;
                    button_VisionPopup_Y_Pos.ForeColor = Color.Black;
                }

                if (vision.MC_Func.MC_isLimit_Neg((int)Vision.nAxis.Z))
                {
                    button_VisionPopup_Z_Neg.BackColor = Color.Red;
                    button_VisionPopup_Z_Neg.ForeColor = Color.White;
                }
                else
                {
                    button_VisionPopup_Z_Neg.BackColor = Color.White;
                    button_VisionPopup_Z_Neg.ForeColor = Color.Black;
                }

                if (vision.MC_Func.MC_isLimit_Pos((int)Vision.nAxis.Z))
                {
                    button_VisionPopup_Z_Pos.BackColor = Color.Red;
                    button_VisionPopup_Z_Pos.ForeColor = Color.White;
                }
                else
                {
                    button_VisionPopup_Z_Pos.BackColor = Color.White;
                    button_VisionPopup_Z_Pos.ForeColor = Color.Black;
                }
            }
        }

        private void FormNew_VisionPopup_FormClosing(object sender, FormClosingEventArgs e)
        {
            //timer_Status.Enabled = false;

            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void FormNew_VisionPopup_VisibleChanged(object sender, EventArgs e)
        {
            if (radioButton_VisionPopup_CameraSelection_LowMag.Checked)
            {
                m_visionImageViewer_LowRes.Visible = true;
                m_visionImageViewer_HighRes.Visible = false;

                if (m_visionImageViewer_LowRes != null)
                {
                    if (this.Visible)
                    {
                        m_visionImageViewer_LowRes.StartUpdateTask();
                    }
                    else
                    {
                        m_visionImageViewer_LowRes.StopUpdateTask();
                    }
                }
            }
            else
            {
                m_visionImageViewer_HighRes.Visible = true;
                m_visionImageViewer_LowRes.Visible = false;                

                if (m_visionImageViewer_HighRes != null)
                {
                    if (this.Visible == true)
                    {
                        m_visionImageViewer_HighRes.StartUpdateTask();
                        //AddOverlay(visionImageViewer);
                    }
                    else
                    {
                        m_visionImageViewer_HighRes.StopUpdateTask();
                    }
                }
            }

            //  Scanner - Fine Camera Offset 변경 모드일 때만 보이는 버튼
            if (Equipment.m_bVisionFormOpenMode_ScannerFineCamOffsetChange)
            {
                button_Scanner_FineCam_OffsetCheck.Visible = true;
                button_Scanner_FineCam_OffsetChange.Visible = true;
            }
            else
            {
                button_Scanner_FineCam_OffsetCheck.Visible = false;
                button_Scanner_FineCam_OffsetChange.Visible = false;
            }
        }

        private void FormNew_VisionPopup_Shown(object sender, EventArgs e)
        {
            //  선택된 카메라에 따라 이미지 뷰어 변경
            if (radioButton_VisionPopup_CameraSelection_LowMag.Checked)
            {
                m_visionImageViewer_LowRes.Visible = true;
                m_visionImageViewer_HighRes.Visible = false;

                SetScroll((int)WorkStage.CameraType.CAMERA_LOW);
            }
            else
            {
                m_visionImageViewer_HighRes.Visible = true;
                m_visionImageViewer_LowRes.Visible = false;


                SetScroll((int)WorkStage.CameraType.CAMERA_HIGH);
            }            

            timer_Status.Enabled = true;
        }

        private void SetScroll(int nCamera)
        {
            //return;

            //  Channel  0:Fine Red Ring, 1:FineIR, 2:CoarseIR

            if (nCamera == (int)WorkStage.CameraType.CAMERA_HIGH)
            {
                //  Red 조명부분 Show
                textBox_IlluminationValue_Red.Visible = true;
                hScrollBarIlluminator_Red.Visible = true;
                baseLabel_Red.Visible = true;
                baseLabelMin_Red.Visible = true;
                baseLabelMax_Red.Visible = true;

                hScrollBarIlluminator_Red.Minimum = (int)workStage.Config.ListIlluminationChannel[0].Min;
                hScrollBarIlluminator_Red.Maximum = (int)workStage.Config.ListIlluminationChannel[0].Max;
                baseLabelMin_Red.Text = hScrollBarIlluminator_Red.Minimum.ToString();
                baseLabelMax_Red.Text = hScrollBarIlluminator_Red.Maximum.ToString();
                //  조명값 변경
                hScrollBarIlluminator_Red.Value = workStage.Config.ListIlluminationChannel[0].Value;            //  고해상도 카메라 조명은 채널 1번, 2번
                this.textBox_IlluminationValue_Red.Text = hScrollBarIlluminator_Red.Value.ToString();

                hScrollBarIlluminator_IR.Minimum = (int)workStage.Config.ListIlluminationChannel[1].Min;
                hScrollBarIlluminator_IR.Maximum = (int)workStage.Config.ListIlluminationChannel[1].Max;
                baseLabelMin_IR.Text = hScrollBarIlluminator_IR.Minimum.ToString();
                baseLabelMax_IR.Text = hScrollBarIlluminator_IR.Maximum.ToString();
                //  조명값 변경
                hScrollBarIlluminator_IR.Value = workStage.Config.ListIlluminationChannel[1].Value;            //  고해상도 카메라 조명은 채널 1번, 2번
                this.textBox_IlluminationValue_IR.Text = hScrollBarIlluminator_IR.Value.ToString();
            }
            else
            {
                //  Red 조명부분 Hide
                textBox_IlluminationValue_Red.Visible = false;
                hScrollBarIlluminator_Red.Visible = false;
                baseLabel_Red.Visible = false;
                baseLabelMin_Red.Visible = false;
                baseLabelMax_Red.Visible = false;

                hScrollBarIlluminator_IR.Minimum = (int)workStage.Config.ListIlluminationChannel[2].Min;
                hScrollBarIlluminator_IR.Maximum = (int)workStage.Config.ListIlluminationChannel[2].Max;
                baseLabelMin_IR.Text = hScrollBarIlluminator_IR.Minimum.ToString();
                baseLabelMax_IR.Text = hScrollBarIlluminator_IR.Maximum.ToString();
                //  조명값 변경
                hScrollBarIlluminator_IR.Value = workStage.Config.ListIlluminationChannel[2].Value;            //  고해상도 카메라 조명은 채널 1번, 2번
                this.textBox_IlluminationValue_IR.Text = hScrollBarIlluminator_IR.Value.ToString();
            }


            //if (m_CurrentDataSet != null && m_nCurrentColum >= 0 && m_nCurrentColum < m_CurrentDataSet.Values.Count)
            //{
            //    MinValue = (int)m_CurrentDataSet.Values[m_nCurrentColum].Min;
            //    MaxValue = (int)m_CurrentDataSet.Values[m_nCurrentColum].Max;

            //    hScrollBarIlluminator.Minimum = (int)m_CurrentDataSet.Values[m_nCurrentColum].Min;
            //    hScrollBarIlluminator.Maximum = (int)m_CurrentDataSet.Values[m_nCurrentColum].Max;
            //    if (hScrollBarIlluminator.Minimum <= m_CurrentDataSet.Values[m_nCurrentColum].Value && m_CurrentDataSet.Values[m_nCurrentColum].Value <= hScrollBarIlluminator.Maximum)
            //    {
            //        hScrollBarIlluminator.Value = m_CurrentDataSet.Values[m_nCurrentColum].Value;
            //    }
            //    else
            //    {
            //        m_CurrentDataSet.Values[m_nCurrentColum].Value = (int)m_CurrentDataSet.Values[m_nCurrentColum].Max;
            //        MessageBox.Show("Out Of Range");
            //        SetScroll();
            //        UpdateIlluminatorGridColumns();
            //    }
            //    baseLabelMin.Text = MinValue.ToString();
            //    baseLabelMax.Text = MaxValue.ToString();
            //    this.baseLabel1Value.Text = this.hScrollBarIlluminator.Value.ToString();
            //}
        }

        private void radioButton_VisionPopup_CameraSelection_LowMag_CheckedChanged(object sender, EventArgs e)
        {
            //  저해상도 카메라 선택

            if (radioButton_VisionPopup_CameraSelection_LowMag.Checked)
            {
                m_visionImageViewer_LowRes.Visible = true;
                m_visionImageViewer_HighRes.Visible = false;

                if (m_visionImageViewer_LowRes != null)
                {
                    if (this.Visible)
                    {
                        if (radioButton_VisionPopup_DisplayMode_Live.Checked)
                        {
                            m_visionImageViewer_LowRes.StartUpdateTask();
                        }
                        else
                        {
                            m_visionImageViewer_LowRes.StopUpdateTask();
                        }
                    }
                    else
                    {
                        m_visionImageViewer_LowRes.StopUpdateTask();
                    }
                }
            }

            SetScroll((int)WorkStage.CameraType.CAMERA_LOW);

            hScrollBarIlluminator_IR.Value = workStage.Config.ListIlluminationChannel[2].Value;                //  저해상도 카메라 IR 조명 (3번, Index 는 2번)
            this.textBox_IlluminationValue_IR.Text = hScrollBarIlluminator_IR.Value.ToString();

            workStage.SetLightingByChannel(Equipment.LightingChannel.CoarseCamIR, hScrollBarIlluminator_IR.Value);
            Thread.Sleep(100);
            workStage.SetLightingByChannel(Equipment.LightingChannel.FineCamRed, 0, false);
            workStage.SetLightingByChannel(Equipment.LightingChannel.FineCamIR, 0, false);
        }

        private void radioButton_VisionPopup_CameraSelection_HighMag_CheckedChanged(object sender, EventArgs e)
        {
            //  고해상도 카메라 선택

            if (radioButton_VisionPopup_CameraSelection_HighMag.Checked)
            {
                m_visionImageViewer_HighRes.Visible = true;
                m_visionImageViewer_LowRes.Visible = false;

                if (m_visionImageViewer_HighRes != null)
                {
                    if (this.Visible == true)
                    {
                        if (radioButton_VisionPopup_DisplayMode_Live.Checked)
                        {
                            m_visionImageViewer_HighRes.StartUpdateTask();
                        }
                        else
                        {
                            m_visionImageViewer_HighRes.StopUpdateTask();
                        }
                    }
                    else
                    {
                        m_visionImageViewer_HighRes.StopUpdateTask();
                    }
                }
            }

            SetScroll((int)WorkStage.CameraType.CAMERA_HIGH);

            hScrollBarIlluminator_IR.Value = workStage.Config.ListIlluminationChannel[1].Value;             //  고해상도 카메라 IR 조명 (2번, Index 는 1번)
            this.textBox_IlluminationValue_IR.Text = hScrollBarIlluminator_IR.Value.ToString();
            hScrollBarIlluminator_Red.Value = workStage.Config.ListIlluminationChannel[0].Value;            //  고해상도 카메라 Red Ring 조명 (1번, Index 는 0번)
            this.textBox_IlluminationValue_Red.Text = hScrollBarIlluminator_Red.Value.ToString();

            workStage.SetLightingByChannel(Equipment.LightingChannel.CoarseCamIR, 0, false);
            Thread.Sleep(100);
            workStage.SetLightingByChannel(Equipment.LightingChannel.FineCamRed, hScrollBarIlluminator_Red.Value);
            workStage.SetLightingByChannel(Equipment.LightingChannel.FineCamIR, hScrollBarIlluminator_IR.Value);
        }

        private void radioButton_VisionPopup_DisplayMode_Live_CheckedChanged(object sender, EventArgs e)
        {
            //  Live

            if (radioButton_VisionPopup_DisplayMode_Live.Checked)
            {
                if (radioButton_VisionPopup_CameraSelection_LowMag.Checked)
                {
                    if (m_visionImageViewer_LowRes != null)
                    {
                        m_visionImageViewer_LowRes.StartUpdateTask();
                    }
                }
                else
                {
                    if (m_visionImageViewer_HighRes != null)
                    {
                        m_visionImageViewer_HighRes.StartUpdateTask();
                    }
                }
            }
        }

        private void radioButton_VisionPopup_DisplayMode_Capture_CheckedChanged(object sender, EventArgs e)
        {
            //  Live Stop

            if (radioButton_VisionPopup_DisplayMode_Capture.Checked)
            {
                if (radioButton_VisionPopup_CameraSelection_LowMag.Checked)
                {
                    if (m_visionImageViewer_LowRes != null)
                    {
                        m_visionImageViewer_LowRes.StopUpdateTask();
                    }
                }
                else
                {
                    if (m_visionImageViewer_HighRes != null)
                    {
                        m_visionImageViewer_HighRes.StopUpdateTask();
                    }
                }
            }
        }

        private void btnCamera_Init_Click(object sender, EventArgs e)
        {
            Log.Write("SLD-200", Equipment.User_Name, "Button Click", "Vision Popup UI, 카메라 초기화");

            //if (Equipment.User_Mode == null)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
            //    return;
            //}

            //  카메라 초기화
            Task<int> task = Task.Factory.StartNew<int>(() =>
            {
                workStage.Camera_HighRes.SetRunStatus(Part.RunStatus.Run);
                workStage.Camera_LowRes.SetRunStatus(Part.RunStatus.Run);
                workStage.Camera_HighRes.Initialize();
                workStage.Camera_LowRes.Initialize();
                return 0;
            });

            ProgressForm ProgressForm = new ProgressForm(workStage.Name, "Camera Initializing...", task, workStage.Camera_HighRes);
            ProgressForm.StopProcess += ProgressForm_StopProcess;
            ProgressForm.StartPosition = FormStartPosition.CenterScreen;
            ProgressForm.ShowDialog();

            workStage.Camera_HighRes.Initialize();
            workStage.Camera_LowRes.Initialize();
        }

        private void ProgressForm_StopProcess(object target)
        {
            Part part = target as Part;
            if (part != null)
            {
                //part.Stop();
            }
        }

        private void btnCamera_StartLive_Click(object sender, EventArgs e)
        {
            //if (Equipment.User_Mode == null)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
            //    return;
            //}

            //  카메라 연결 확인
            if (!workStage.Camera_HighRes.Opened ||
                !workStage.Camera_LowRes.Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 카메라를 연결해야 해야 합니다.");
                return;
            }

            if (workStage.Camera_HighRes != null)
            {
                workStage.Camera_HighRes.StartLive();
            }

            if (workStage.Camera_LowRes != null)
            {
                workStage.Camera_LowRes.StartLive();
            }
        }

        private void button_VisionPopup_X_Neg_MouseDown(object sender, MouseEventArgs e)
        {
            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDirection = -1.0;

            if (!this.radioButton_VisionPopup_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_VisionPopup_Move_MoveMode_Fine.Checked)
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.X].Jog_Speed_Fine;
                        lfAccDec = Equipment.stAxisParam[(int)Vision.nAxis.X].Common_Acceleration_Fine;
                    }
                    else
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.X].Jog_Speed_Coarse;
                        lfAccDec = Equipment.stAxisParam[(int)Vision.nAxis.X].Common_Acceleration_Coarse;
                    }

                    //  방향 설정
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Negative direction : Using - (minus) velocity

                    vision.MC_Func.MC_JogMove((int)Vision.nAxis.X, lfVelocity * dDirection, lfAccDec, lfAccDec);
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_VisionPopup_X_Pos_MouseDown(object sender, MouseEventArgs e)
        {
            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDirection = 1.0;

            if (!this.radioButton_VisionPopup_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_VisionPopup_Move_MoveMode_Fine.Checked)
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.X].Jog_Speed_Fine;
                        lfAccDec = Equipment.stAxisParam[(int)Vision.nAxis.X].Common_Acceleration_Fine;
                    }
                    else
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.X].Jog_Speed_Coarse;
                        lfAccDec = Equipment.stAxisParam[(int)Vision.nAxis.X].Common_Acceleration_Coarse;
                    }

                    //  방향 설정
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Positive direction : Using + (plus) velocity

                    vision.MC_Func.MC_JogMove((int)Vision.nAxis.X, lfVelocity * dDirection, lfAccDec, lfAccDec);
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_VisionPopup_Y_Neg_MouseDown(object sender, MouseEventArgs e)
        {
            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDirection = -1.0;

            if (!this.radioButton_VisionPopup_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_VisionPopup_Move_MoveMode_Fine.Checked)
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.Y].Jog_Speed_Fine;
                        lfAccDec = Equipment.stAxisParam[(int)Vision.nAxis.Y].Common_Acceleration_Fine;
                    }
                    else
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.Y].Jog_Speed_Coarse;
                        lfAccDec = Equipment.stAxisParam[(int)Vision.nAxis.Y].Common_Acceleration_Coarse;
                    }

                    //  방향 설정
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Negative direction : Using - (minus) velocity

                    vision.MC_Func.MC_JogMove((int)Vision.nAxis.Y, lfVelocity * dDirection, lfAccDec, lfAccDec);
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_VisionPopup_Y_Pos_MouseDown(object sender, MouseEventArgs e)
        {
            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDirection = 1.0;

            if (!this.radioButton_VisionPopup_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_VisionPopup_Move_MoveMode_Fine.Checked)
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.Y].Jog_Speed_Fine;
                        lfAccDec = Equipment.stAxisParam[(int)Vision.nAxis.Y].Common_Acceleration_Fine;
                    }
                    else
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.Y].Jog_Speed_Coarse;
                        lfAccDec = Equipment.stAxisParam[(int)Vision.nAxis.Y].Common_Acceleration_Coarse;
                    }

                    //  방향 설정
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Positive direction : Using + (plus) velocity

                    vision.MC_Func.MC_JogMove((int)Vision.nAxis.Y, lfVelocity * dDirection, lfAccDec, lfAccDec);
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_VisionPopup_Z_Neg_MouseDown(object sender, MouseEventArgs e)
        {
            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDirection = -1.0;

            if (!this.radioButton_VisionPopup_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_VisionPopup_Move_MoveMode_Fine.Checked)
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.Z].Jog_Speed_Fine;
                        lfAccDec = Equipment.stAxisParam[(int)Vision.nAxis.Z].Common_Acceleration_Fine;
                    }
                    else
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.Z].Jog_Speed_Coarse;
                        lfAccDec = Equipment.stAxisParam[(int)Vision.nAxis.Z].Common_Acceleration_Coarse;
                    }

                    //  방향 설정
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Negative direction : Using - (minus) velocity

                    vision.MC_Func.MC_JogMove((int)Vision.nAxis.Z, lfVelocity * dDirection, lfAccDec, lfAccDec);
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_VisionPopup_Z_Pos_MouseDown(object sender, MouseEventArgs e)
        {
            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDirection = 1.0;

            if (!this.radioButton_VisionPopup_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_VisionPopup_Move_MoveMode_Fine.Checked)
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.Z].Jog_Speed_Fine;
                        lfAccDec = Equipment.stAxisParam[(int)Vision.nAxis.Z].Common_Acceleration_Fine;
                    }
                    else
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.Z].Jog_Speed_Coarse;
                        lfAccDec = Equipment.stAxisParam[(int)Vision.nAxis.Z].Common_Acceleration_Coarse;
                    }

                    //  방향 설정
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Positive direction : Using + (plus) velocity

                    vision.MC_Func.MC_JogMove((int)Vision.nAxis.Z, lfVelocity * dDirection, lfAccDec, lfAccDec);
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_VisionPopup_Axis_MouseUp(object sender, MouseEventArgs e)
        {
            if (!this.radioButton_VisionPopup_JogMove_Continuous.Checked)
                return;

            if (Equipment.AjinBoard_Opened)
            {
                vision.MC_Func.MC_JogStop((int)Vision.nAxis.X);
                vision.MC_Func.MC_JogStop((int)Vision.nAxis.Y);
                vision.MC_Func.MC_JogStop((int)Vision.nAxis.Z);
            }
        }

        private void button_VisionPopup_X_Neg_Click(object sender, EventArgs e)
        {
            if (!this.radioButton_VisionPopup_JogMove_Step.Checked)
                return;

            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDistance = Equipment.ToDouble(textBox_VisionPopup_JogMove_StepSize.Text);
            int nDirection = 1;
            double dVelocity = 0;

            //  이동 거리는 양수로 변경
            if (dDistance < 0) dDistance = dDistance * (-1.0);

            //  방향 설정
            nDirection = -1;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_VisionPopup_Move_MoveMode_Fine.Checked)
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.X].Jog_Speed_Fine;
                        lfAccDec = Equipment.stAxisParam[(int)Vision.nAxis.X].Common_Acceleration_Fine;
                    }
                    else
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.X].Jog_Speed_Coarse;
                        lfAccDec = Equipment.stAxisParam[(int)Vision.nAxis.X].Common_Acceleration_Coarse;
                    }

                    //  속도는 양수로
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                    vision.MC_Func.MC_MoveRelPosition((int)Vision.nAxis.X, dDistance * (double)nDirection, lfVelocity, lfAccDec, lfAccDec);
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_VisionPopup_X_Pos_Click(object sender, EventArgs e)
        {
            if (!this.radioButton_VisionPopup_JogMove_Step.Checked)
                return;

            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDistance = Equipment.ToDouble(textBox_VisionPopup_JogMove_StepSize.Text);
            int nDirection = 1;
            double dVelocity = 0;

            //  이동 거리는 양수로 변경
            if (dDistance < 0) dDistance = dDistance * (-1.0);

            //  방향 설정
            nDirection = 1;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_VisionPopup_Move_MoveMode_Fine.Checked)
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.X].Jog_Speed_Fine;
                        lfAccDec = Equipment.stAxisParam[(int)Vision.nAxis.X].Common_Acceleration_Fine;
                    }
                    else
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.X].Jog_Speed_Coarse;
                        lfAccDec = Equipment.stAxisParam[(int)Vision.nAxis.X].Common_Acceleration_Coarse;
                    }

                    //  속도는 양수로
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                    vision.MC_Func.MC_MoveRelPosition((int)Vision.nAxis.X, dDistance * (double)nDirection, lfVelocity, lfAccDec, lfAccDec);
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_VisionPopup_Y_Neg_Click(object sender, EventArgs e)
        {
            if (!this.radioButton_VisionPopup_JogMove_Step.Checked)
                return;

            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDistance = Equipment.ToDouble(textBox_VisionPopup_JogMove_StepSize.Text);
            int nDirection = 1;
            double dVelocity = 0;

            //  이동 거리는 양수로 변경
            if (dDistance < 0) dDistance = dDistance * (-1.0);

            //  방향 설정
            nDirection = -1;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_VisionPopup_Move_MoveMode_Fine.Checked)
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.Y].Jog_Speed_Fine;
                        lfAccDec = Equipment.stAxisParam[(int)Vision.nAxis.Y].Common_Acceleration_Fine;
                    }
                    else
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.Y].Jog_Speed_Coarse;
                        lfAccDec = Equipment.stAxisParam[(int)Vision.nAxis.Y].Common_Acceleration_Coarse;
                    }

                    //  속도는 양수로
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                    vision.MC_Func.MC_MoveRelPosition((int)Vision.nAxis.Y, dDistance * (double)nDirection, lfVelocity, lfAccDec, lfAccDec);
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_VisionPopup_Y_Pos_Click(object sender, EventArgs e)
        {
            if (!this.radioButton_VisionPopup_JogMove_Step.Checked)
                return;

            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDistance = Equipment.ToDouble(textBox_VisionPopup_JogMove_StepSize.Text);
            int nDirection = 1;
            double dVelocity = 0;

            //  이동 거리는 양수로 변경
            if (dDistance < 0) dDistance = dDistance * (-1.0);

            //  방향 설정
            nDirection = 1;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_VisionPopup_Move_MoveMode_Fine.Checked)
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.Y].Jog_Speed_Fine;
                        lfAccDec = Equipment.stAxisParam[(int)Vision.nAxis.Y].Common_Acceleration_Fine;
                    }
                    else
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.Y].Jog_Speed_Coarse;
                        lfAccDec = Equipment.stAxisParam[(int)Vision.nAxis.Y].Common_Acceleration_Coarse;
                    }

                    //  속도는 양수로
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                    vision.MC_Func.MC_MoveRelPosition((int)Vision.nAxis.Y, dDistance * (double)nDirection, lfVelocity, lfAccDec, lfAccDec);
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_VisionPopup_Z_Neg_Click(object sender, EventArgs e)
        {
            if (!this.radioButton_VisionPopup_JogMove_Step.Checked)
                return;

            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDistance = Equipment.ToDouble(textBox_VisionPopup_JogMove_StepSize.Text);
            int nDirection = 1;
            double dVelocity = 0;

            //  이동 거리는 양수로 변경
            if (dDistance < 0) dDistance = dDistance * (-1.0);

            //  방향 설정
            nDirection = -1;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_VisionPopup_Move_MoveMode_Fine.Checked)
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.Z].Jog_Speed_Fine;
                        lfAccDec = Equipment.stAxisParam[(int)Vision.nAxis.Z].Common_Acceleration_Fine;
                    }
                    else
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.Z].Jog_Speed_Coarse;
                        lfAccDec = Equipment.stAxisParam[(int)Vision.nAxis.Z].Common_Acceleration_Coarse;
                    }

                    //  속도는 양수로
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                    vision.MC_Func.MC_MoveRelPosition((int)Vision.nAxis.Z, dDistance * (double)nDirection, lfVelocity, lfAccDec, lfAccDec);
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_VisionPopup_Z_Pos_Click(object sender, EventArgs e)
        {
            if (!this.radioButton_VisionPopup_JogMove_Step.Checked)
                return;

            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDistance = Equipment.ToDouble(textBox_VisionPopup_JogMove_StepSize.Text);
            int nDirection = 1;
            double dVelocity = 0;

            //  이동 거리는 양수로 변경
            if (dDistance < 0) dDistance = dDistance * (-1.0);

            //  방향 설정
            nDirection = 1;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_VisionPopup_Move_MoveMode_Fine.Checked)
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.Z].Jog_Speed_Fine;
                        lfAccDec = Equipment.stAxisParam[(int)Vision.nAxis.Z].Common_Acceleration_Fine;
                    }
                    else
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.Z].Jog_Speed_Coarse;
                        lfAccDec = Equipment.stAxisParam[(int)Vision.nAxis.Z].Common_Acceleration_Coarse;
                    }

                    //  속도는 양수로
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                    vision.MC_Func.MC_MoveRelPosition((int)Vision.nAxis.Z, dDistance * (double)nDirection, lfVelocity, lfAccDec, lfAccDec);
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Scanner_FineCam_OffsetCheck_Click(object sender, EventArgs e)
        {
            //  Scanner - Fine Camera Offset Check

            //  스캐너와 카메라 간 Offset 확인 (검증)

            double m_dAxisZPos = 0.0;

            if (!workStage.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

            //  카메라 연결 확인
            if (!workStage.Camera_HighRes.Opened ||
                !workStage.Camera_LowRes.Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 카메라를 연결해야 해야 합니다.");
                return;
            }

            

            if (workStage.rtc == null)
            {
                MessageBox.Show("먼저 Scanner Board 를 초기화 해야 합니다.", "Information!!");
                return;
            }

            if (workStage.m_nVerify_ScannerCenter_CamCenter_Step == (int)WorkStage.VerifyScannerCameraCenter_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "스캐너와 카메라의 Offset 검증을 시작하시겠습니까?\r\n\r\n1. 카메라 Focus 확인\r\n2. 현재 카메라가 보고 있는 위치를 스캐너로 이동\r\n3. ┼ 마크 가공\r\n4. 가공 위치를 다시 카메라로 이동 후 오차 확인"))
                    return;

                if (!workStage.workStageParameter.IsDO_BeamDump_Coolant_Supply() || !workStage.workStageParameter.IsDO_Scanner_Coolant_Supply() ||

                    (Equipment.Machine_LaserType_CO2 && (!workStage.workStageParameter.IsDO_Mask_Coolant_Supply() || !workStage.workStageParameter.IsDO_VarioScan_Coolant_Supply())))
                {
                    var mb2 = new MessageBoxOk();
                    mb2.ShowDialog("Warning !", "냉각수를 순환 시키고 작업을 진행해야 합니다.");
                    return;
                }

                ////  StageZ 한계위치 설정되어 있는지 체크
                //if (laserDrilling.Config.ParamConfig.Interlock_StageZ_UpperPos <= 0.0)
                //{
                //    var mb1 = new MessageBoxOk();
                //    mb1.ShowDialog("Warning !", "Stage Z축 한계 높이가 설정되어 있지 않습니다.\r\n\r\n(Config -> [17] Interlock  확인)");
                //    return;
                //}

                //laserDrilling.laserDrillingParameter.stLaserDrillingPosParam = laserDrilling.laserDrillingParameter.GetPositionInformation("WorkStage_WorkHeight");
                //m_dAxisZPos = laserDrilling.MC_Func.MC_GetEncPos((int)LaserDrilling.nAxis.Z);

                ////  StageZ 한계위치를 초과하여 이동하는지 체크
                //if ((laserDrilling.Config.ParamConfig.Interlock_StageZ_UpperPos < m_dAxisZPos) ||
                //    (laserDrilling.Config.ParamConfig.Interlock_StageZ_UpperPos < laserDrilling.laserDrillingParameter.stLaserDrillingPosParam.dTarget[(int)WorkStageParameter.MotionKey.Z]))
                //{
                //    var mb1 = new MessageBoxOk();
                //    mb1.ShowDialog("Warning !", "Stage Z축 한계 높이를 초과한 상태로 이동하려고 하였거나,\r\nLaser 가공 높이가 Z 축 한계를 초과한 상태입니다.\r\n\r\n[ Cancel ]");
                //    return;
                //}

                workStage.m_bScannerCamVerify_Complete = false;
                workStage.m_pStageXY_Pos_BeforeVerify.X = 0.0;
                workStage.m_pStageXY_Pos_BeforeVerify.Y = 0.0;
                workStage.m_pStageXY_Pos_AfterVerify.X = 0.0;
                workStage.m_pStageXY_Pos_AfterVerify.Y = 0.0;

                workStage.m_nVerify_ScannerCenter_CamCenter_Step = (int)WorkStage.VerifyScannerCameraCenter_Step.Start;
                workStage.timer_VerifyScannerCamOffset.Enabled = true;
            }
            else
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "스캐너와 카메라의 Offset 검증을 중지하시겠습니까?"))
                    return;

                workStage.m_bScannerCamVerify_Complete = false;
                workStage.m_pStageXY_Pos_BeforeVerify.X = 0.0;
                workStage.m_pStageXY_Pos_BeforeVerify.Y = 0.0;
                workStage.m_pStageXY_Pos_AfterVerify.X = 0.0;
                workStage.m_pStageXY_Pos_AfterVerify.Y = 0.0;

                workStage.timer_VerifyScannerCamOffset.Enabled = false;
                workStage.m_nVerify_ScannerCenter_CamCenter_Step = (int)WorkStage.VerifyScannerCameraCenter_Step.None;
            }
        }

        private void button_Scanner_FineCam_OffsetChange_Click(object sender, EventArgs e)
        {
            //  Scanner - Fine Camera Offset Change

            PointD m_pBeforeOffset = new PointD();

            string m_strTemp;

            double m_dOffsetX = 0.0;
            double m_dOffsetY = 0.0;

            m_pBeforeOffset.X = 0.0;
            m_pBeforeOffset.Y = 0.0;

            if (workStage.m_nVerify_ScannerCenter_CamCenter_Step == (int)WorkStage.VerifyScannerCameraCenter_Step.None)
            {
                if (workStage.m_bScannerCamVerify_Complete)
                {
                    var mb = new MessageBoxYesNo();
                    if (DialogResult.Yes != mb.ShowDialog("Question ?", "스캐너와 카메라의 Offset 을 변경하시겠습니까?\r\n\r\n[ ★★★ Camera 중심에 십자마크가 정확히 위치해야 합니다. ★★★]"))
                        return;

                    //  현재 위치 저장
                    workStage.m_pStageXY_Pos_AfterVerify.X = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X);
                    workStage.m_pStageXY_Pos_AfterVerify.Y = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y);

                    //  Offset 계산
                    m_dOffsetX = workStage.m_pStageXY_Pos_AfterVerify.X - workStage.m_pStageXY_Pos_BeforeVerify.X;
                    m_dOffsetY = workStage.m_pStageXY_Pos_AfterVerify.Y - workStage.m_pStageXY_Pos_BeforeVerify.Y;

                    //  기존 Offset
                    m_pBeforeOffset.X = Equipment.stOffsetDistance.FromScannerToFineCam.X;
                    m_pBeforeOffset.Y = Equipment.stOffsetDistance.FromScannerToFineCam.Y;

                    //  기존 Offset 에 현재 편차 반영
                    Equipment.stOffsetDistance.FromScannerToFineCam.X += m_dOffsetX;
                    Equipment.stOffsetDistance.FromScannerToFineCam.Y += m_dOffsetY;

                    Scanner_FineCam_Offset_Save();

                    m_strTemp = "스캐너와 카메라 간 Offset 이 변경되었습니다.\r\n\r\n[Before X : " + m_pBeforeOffset.X + "\tY : " + m_pBeforeOffset.Y +
                        "\r\n[After X: " + Equipment.stOffsetDistance.FromScannerToFineCam.X + "\tY: " + Equipment.stOffsetDistance.FromScannerToFineCam.Y + "]";

                    var mb2 = new MessageBoxOk();
                    mb2.ShowDialog("Information !", m_strTemp);
                }
                else
                {
                    var mb2 = new MessageBoxOk();
                    mb2.ShowDialog("Warning !", "먼저 Scanner 와 Camera Offset 검증 과정을 진행해야 합니다.");
                    return;
                }
            }
            else
            {
                var mb = new MessageBoxOk();
                mb.ShowDialog("Warning !", "스캐너와 카메라의 Offset 검증이 완료되지 않았습니다.");
            }

            workStage.m_bScannerCamVerify_Complete = false;
        }

        public void Scanner_FineCam_Offset_Save()
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


            //  Offset Distance
            NativeMethods.WritePrivateProfileString("Offset_Distance", "From_Scanner_To_FineCam_X", Equipment.stOffsetDistance.FromScannerToFineCam.X.ToString(), strFIle);
            NativeMethods.WritePrivateProfileString("Offset_Distance", "From_Scanner_To_FineCam_Y", Equipment.stOffsetDistance.FromScannerToFineCam.Y.ToString(), strFIle);


            //MessageBox.Show("Scanner 와 Fine Camera 간 Offset 데이터를 저장하였습니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_VisionPopup_FindCircle_Search_Click(object sender, EventArgs e)
        {
            //  원 찾기
            bool m_bFindCircle = false;

            int m_nImage_Width = 0;
            int m_nImage_Height = 0;

            double m_dTargetSize_Radius = 0.0;
            int m_nTargetColor = 0;


            if (textBox_VisionPopup_FiducialSize_Width.Text.Length < 0)
            {
                MessageBox.Show("Fiducial Size 를 입력하세요.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //  Fiducial 마크 크기
            m_dTargetSize_Radius = Equipment.ToDouble(textBox_VisionPopup_FiducialSize_Width.Text);

            //  Fiducial 마크 색깔
            m_nTargetColor = comboBox_VisionPopup_FiducialColor.SelectedIndex;          //  0: Black, 1: White


            detectedCircles.Clear();

            QMC_ImageProcessFindAlign aligner = new QMC_ImageProcessFindAlign();
            List<RectangleF> circlesResult = new List<RectangleF>();
            int w = workStage.Camera_LowRes.Resolution.Width;
            int h = workStage.Camera_LowRes.Resolution.Height;
            double dRaius = 0.0;
            QMC_ImageProcessFindAlignResult result = new QMC_ImageProcessFindAlignResult();
            if (radioButton_VisionPopup_CameraSelection_LowMag.Checked)
            {
                w = workStage.Camera_LowRes.Resolution.Width;
                h = workStage.Camera_LowRes.Resolution.Height;
                dRaius = m_dTargetSize_Radius / workStage.Config.ParamConfig.LowerVision_Scale_X;

            }
            else
            {
                w = workStage.Camera_HighRes.Resolution.Width;
                h = workStage.Camera_HighRes.Resolution.Height;
                dRaius = m_dTargetSize_Radius / workStage.Config.ParamConfig.UpperVision_Scale_X;
            }
            result = aligner.FindCirclesWidthCircleBoundary(circlesResult, bm_RawData, w, h, (int)dRaius, 0.015, ref m_bFindCircle, 0, 0, m_nTargetColor == 0);
            if (m_bFindCircle && (circlesResult.Count > 0))
            {
                detectedCircles.Clear();
                //  좌표 표시
                listBox_FindCircle_Result.Items.Clear();
                for (int i = 0; i < result.Circles.Count; i++)
                {
                    double dCxpx = result.Circles[i].CenterX;
                    double dCypx = result.Circles[i].CenterY;
                    double dCxmm = result.Circles[i].CenterX * workStage.Config.ParamConfig.LowerVision_Scale_X;
                    double dCymm = result.Circles[i].CenterY * workStage.Config.ParamConfig.LowerVision_Scale_Y;
                    dRaius = result.Circles[i].Radius * workStage.Config.ParamConfig.LowerVision_Scale_X;
                    listBox_FindCircle_Result.Items.Add((i + 1) + ".X(px) : " + dCxpx.ToString("F3"));
                    listBox_FindCircle_Result.Items.Add((i + 1) + ".Y(px) : " + dCypx.ToString("F3"));
                    listBox_FindCircle_Result.Items.Add((i + 1) + ".X(mm) : " + dCxmm.ToString("F3"));
                    listBox_FindCircle_Result.Items.Add((i + 1) + ".Y(mm) : " + dCymm.ToString("F3"));
                    listBox_FindCircle_Result.Items.Add((i + 1) + ".지름(mm) : " + (dRaius*2).ToString("F3"));
                    listBox_FindCircle_Result.Items.Add((i + 1) + ".Score : " + result.ScoreCollection[i].ToString("F2"));
                }

                foreach (var circle in circlesResult)
                {
                    float ratioX = (float)pictureBox_ImageDisplay.Width / w;
                    float ratioY = (float)pictureBox_ImageDisplay.Height / h;
                    float ratio = Math.Min(ratioX, ratioY);

                    int newX = (int)(circle.X * ratio);
                    int newY = (int)(circle.Y * ratio);
                    int newWidth = (int)(circle.Width * ratio);
                    int newHeight = (int)(circle.Height * ratio);

                    detectedCircles.Add(new Rectangle(newX, newY, newWidth, newHeight));
                    pictureBox_ImageDisplay.Invalidate(); // PictureBox를 다시 그리도록 요청

                }
                

            }
            else
            {
                MessageBox.Show("원 찾기 실패", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                detectedCircles.Clear();

                listBox_FindCircle_Result.Items.Clear();
            }

            //  원 찾기 후 다시 Live
            if (radioButton_VisionPopup_CameraSelection_LowMag.Checked)
            {
                if (workStage.Camera_LowRes.Opened)
                {
                    workStage.Camera_LowRes.StartLive();
                }
            }
            else
            {
                if (workStage.Camera_HighRes.Opened)
                {
                    workStage.Camera_HighRes.StartLive();
                }
            }
        }

        private Image ResizeImageToFitPictureBox(Image image, PictureBox pictureBox)
        {
            int originalWidth = image.Width;
            int originalHeight = image.Height;
            int targetWidth = pictureBox.Width;
            int targetHeight = pictureBox.Height;

            float ratioX = (float)targetWidth / originalWidth;
            float ratioY = (float)targetHeight / originalHeight;
            float ratio = Math.Min(ratioX, ratioY);

            int newWidth = (int)(originalWidth * ratio);
            int newHeight = (int)(originalHeight * ratio);

            Bitmap resizedImage = new Bitmap(newWidth, newHeight);
            using (Graphics graphics = Graphics.FromImage(resizedImage))
            {
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            return resizedImage;
        }

        private void button_VisionPopup_FindCircle_LoadImage_Click(object sender, EventArgs e)
        {
            //  Load Image

            QMC_ImageProcessFindAlign aligner = new QMC_ImageProcessFindAlign();

            detectedCircles.Clear();
            listBox_FindCircle_Result.Items.Clear();

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Bitmap files (*.bmp)|*.bmp";
                openFileDialog.Title = "Select a Bitmap file";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = openFileDialog.FileName;
                    string selectedFolder = Path.GetDirectoryName(selectedFilePath);

                    bm_Temp = new Bitmap(selectedFilePath);
                    
                    bm_RawData = new byte[workStage.Camera_HighRes.Resolution.Width * workStage.Camera_HighRes.Resolution.Height];
                    bm_RawData = aligner.ConvertBitmapToByteArray(bm_Temp);

                    pictureBox_ImageDisplay.Image = ResizeImageToFitPictureBox(bm_Temp, pictureBox_ImageDisplay);

                    //if (radioButton_VisionPopup_CameraSelection_LowMag.Checked)
                    //{
                    //    pictureBox_ImageDisplay.Image = new Bitmap(selectedFilePath);
                    //}
                    //else
                    //{
                    //    m_visionImageViewer_HighRes.Image = new Bitmap(selectedFilePath);
                    //}
                }
            }
        }

        private void pictureBox_ImageDisplay_Paint(object sender, PaintEventArgs e)
        {
            int nIndex = 0;
            foreach (var detectedCircle in detectedCircles)
            {
                Color color = Color.Blue;
                switch (nIndex)
                {
                    case 1:
                        color = Color.Red;
                        break;
                    case 2:
                        color = Color.Lime;
                        break;
                    case 3:
                        color = Color.Yellow;
                        break;
                }

                using (Pen pen = new Pen(color, 1)) // 펜의 두께를 3포인트로 설정
                {
                    e.Graphics.DrawRectangle(pen, detectedCircle);
                }
                nIndex++;
            }
        }

        private void button_VisionPopup_FindCircle_GrabImage_Click(object sender, EventArgs e)
        {
            //  이미지 Grab

            detectedCircles.Clear();
            listBox_FindCircle_Result.Items.Clear();

            Image image = new Bitmap(workStage.Camera_HighRes.Resolution.Width, workStage.Camera_HighRes.Resolution.Height);

            //  Grab
            if (radioButton_VisionPopup_CameraSelection_LowMag.Checked)
            {
                workStage.Camera_LowRes.Grab();

                bm_RawData = new byte[workStage.Camera_LowRes.Resolution.Width * workStage.Camera_LowRes.Resolution.Height];
                bm_RawData = workStage.Camera_LowRes.LatestImage.RawData;

                image = workStage.Camera_LowRes.LatestImage.GetImage();

                //workStage.Camera_LowRes.LatestImage.Save("D:\\TempImage_LowRes.bmp", QMC.Common.Vision.VisionImage.FileFilter.bmp);
                //bm_Temp = new Bitmap("D:\\TempImage_LowRes.bmp");                
            }
            else
            {
                workStage.Camera_HighRes.Grab();

                bm_RawData = new byte[workStage.Camera_HighRes.Resolution.Width * workStage.Camera_HighRes.Resolution.Height];
                bm_RawData = workStage.Camera_HighRes.LatestImage.RawData;

                image = workStage.Camera_HighRes.LatestImage.GetImage();

                //workStage.Camera_HighRes.LatestImage.Save("D:\\TempImage_HighRes.bmp", QMC.Common.Vision.VisionImage.FileFilter.bmp);
                //bm_Temp = new Bitmap("D:\\TempImage_HighRes.bmp");
            }

            //pictureBox_ImageDisplay.Image = ResizeImageToFitPictureBox(bm_Temp, pictureBox_ImageDisplay);
            pictureBox_ImageDisplay.Image = ResizeImageToFitPictureBox(image, pictureBox_ImageDisplay);
        }

        private void button_CurrentZPos_toFineCamFocus_Click(object sender, EventArgs e)
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "현재 Z축 높이를 Fine Camera, Laser, Laser Height Sensor Focus 로 설정하시겠습니까?"))
                return;

            vision.stVisionTeachingPos[(int)Vision.Vision_TeachingPosList.Vision_FocusPos].Vision_Z = Equipment.ToDouble(string.Format("{0:0.000}", vision.MC_Func.MC_GetEncPos((int)Vision.nAxis.Z).ToString()));
            vision.stVisionTeachingPos[(int)Vision.Vision_TeachingPosList.Laser_FocusPos].Vision_Z = Equipment.ToDouble(string.Format("{0:0.000}", vision.MC_Func.MC_GetEncPos((int)Vision.nAxis.Z).ToString()));
            vision.stVisionTeachingPos[(int)Vision.Vision_TeachingPosList.Laser_Sensor_HeightCheckPos].Vision_Z = Equipment.ToDouble(string.Format("{0:0.000}", vision.MC_Func.MC_GetEncPos((int)Vision.nAxis.Z).ToString()));

            //  리스트 전체 저장
            vision.Teaching_Position_Save();
        }

        private void button_CurrentZPos_toLaserFocus_Click(object sender, EventArgs e)
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "현재 Z축 높이를 Laser Focus 로 설정하시겠습니까?"))
                return;

            vision.stVisionTeachingPos[(int)Vision.Vision_TeachingPosList.Laser_FocusPos].Vision_Z = Equipment.ToDouble(string.Format("{0:0.000}", vision.MC_Func.MC_GetEncPos((int)Vision.nAxis.Z).ToString()));

            //  리스트 전체 저장
            vision.Teaching_Position_Save();
        }

        private void button_CurrentLightValue_toAlignLightValue_Click(object sender, EventArgs e)
        {
            //  현재 조명값을 얼라인 조명값으로 설정

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "현재 조명값을 Align 조명값으로 사용하시겠습니까?"))
                return;

            if (Equipment.Current_Recipe.Length > 0)
            {
                //  현재 조명값을 얼라인 조명값으로 설정
                //Equipment.stLayerRecipeSet[(int)Equipment.LayerList.Fiducial].IlluminatorValue_FineCamRed = workStage.Config.ListIlluminationChannel[0].Value;      //  Fine Camera Red
                //Equipment.stLayerRecipeSet[(int)Equipment.LayerList.Fiducial].IlluminatorValue_FineCamIR = workStage.Config.ListIlluminationChannel[1].Value;       //  Fine Camera IR
                //Equipment.stLayerRecipeSet[(int)Equipment.LayerList.Fiducial].IlluminatorValue_CoarseCamIR = workStage.Config.ListIlluminationChannel[2].Value;     //  Coarse Camera IR

                //  리스트 전체 저장
                Recipe_Data_Save_LightValue(Equipment.Current_Recipe);

                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "얼라인 조명값을 저장하였습니다.");
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Recipe 가 선택되지 않았습니다.");
            }
        }

        private void Recipe_Data_Save_LightValue(string m_strRecipeFile)
        {
            string strTemp = "";

            string strFIle = "";
            //strFIle = ConfigManager.GetRecipeDataPath() + "\\LDUL_TeachingPosition.ini";
            strFIle = m_strRecipeFile;

            if (File.Exists(strFIle) == false)
            {
                File.Create(strFIle);

                strTemp = string.Format("{0} 파일을 생성하였습니다. 다시 시도하십시오.", System.IO.Path.GetFileName(strFIle));
                MessageBox.Show(strTemp, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            //  Recipe Parameter 저장
            for (int i = 0; i < (int)System.Enum.GetValues(typeof(LayerList)).Length; i++)
            {
                strTemp = string.Format("Layer_{0}", i);

                //  Fine Cam. Red
                //NativeMethods.WritePrivateProfileString(strTemp, "FineCam_Red", Equipment.stLayerRecipeSet[i].IlluminatorValue_FineCamRed.ToString(), strFIle);
                //  Fine Cam. IR
                //NativeMethods.WritePrivateProfileString(strTemp, "FineCam_IR", Equipment.stLayerRecipeSet[i].IlluminatorValue_FineCamIR.ToString(), strFIle);
                //  Coarse Cam. IR
                //NativeMethods.WritePrivateProfileString(strTemp, "CoarseCam_IR", Equipment.stLayerRecipeSet[i].IlluminatorValue_CoarseCamIR.ToString(), strFIle);
            }
        }

        private void radioButton_Light_IR_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_VisionPopup_CameraSelection_LowMag.Checked)
            {
                SetScroll(2);

                hScrollBarIlluminator_IR.Value = workStage.Config.ListIlluminationChannel[2].Value;                //  저해상도 카메라 IR 조명 (3번, Index 는 2번)
                this.textBox_IlluminationValue_IR.Text = hScrollBarIlluminator_IR.Value.ToString();

                workStage.SetLightingByChannel(Equipment.LightingChannel.CoarseCamIR, hScrollBarIlluminator_IR.Value);
                Thread.Sleep(100);
                workStage.SetLightingByChannel(Equipment.LightingChannel.FineCamRed, 0, false);
                workStage.SetLightingByChannel(Equipment.LightingChannel.FineCamIR, 0, false);
            }
            else
            {
                SetScroll(1);

                hScrollBarIlluminator_IR.Value = workStage.Config.ListIlluminationChannel[1].Value;                //  고해상도 카메라 IR 조명 (2번, Index 는 1번)
                this.textBox_IlluminationValue_IR.Text = hScrollBarIlluminator_IR.Value.ToString();

                workStage.SetLightingByChannel(Equipment.LightingChannel.CoarseCamIR, 0, false);
                Thread.Sleep(100);
                workStage.SetLightingByChannel(Equipment.LightingChannel.FineCamRed, workStage.Config.ListIlluminationChannel[0].Value);
                workStage.SetLightingByChannel(Equipment.LightingChannel.FineCamIR, hScrollBarIlluminator_IR.Value);
            }
        }

        private void radioButton_Light_Red_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_VisionPopup_CameraSelection_HighMag.Checked)
            {
                SetScroll(0);

                hScrollBarIlluminator_IR.Value = workStage.Config.ListIlluminationChannel[0].Value;                //  저해상도 카메라 IR 조명 (3번, Index 는 2번)
                this.textBox_IlluminationValue_IR.Text = hScrollBarIlluminator_IR.Value.ToString();

                workStage.SetLightingByChannel(Equipment.LightingChannel.CoarseCamIR, 0, false);
                Thread.Sleep(100);
                workStage.SetLightingByChannel(Equipment.LightingChannel.FineCamRed, hScrollBarIlluminator_IR.Value);
                workStage.SetLightingByChannel(Equipment.LightingChannel.FineCamIR, workStage.Config.ListIlluminationChannel[1].Value);
            }
        }

        private void button_Test_SocketAlign_Start_Click(object sender, EventArgs e)
        {
            //  소켓 얼라인 시작

            if (comboBox_Config_VisionPopup_AlignTest_SocketList.Items.Count <= 0)
            {
                MessageBox.Show("소켓 리스트가 없습니다.\r\n\r\n도면 데이터를 Parsing 해야 합니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBox_Config_VisionPopup_AlignTest_SocketList.SelectedIndex < 0)
            {
                MessageBox.Show("소켓 리스트를 선택해야 합니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int m_nSocketIndex = comboBox_Config_VisionPopup_AlignTest_SocketList.SelectedIndex;

            if (workStage.m_nSocketAlign_MainStep == (int)WorkStage.SocketAlign_Step.None)
            {

                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "소켓 얼라인을 시작하시겠습니까?"))
                    return;

                workStage.m_nSocketNum_forAlign = m_nSocketIndex;
                workStage.m_nSocketAlign_MainStep = (int)WorkStage.SocketAlign_Step.Start;

                workStage.timer_VisionAlign.Enabled = true;
            }
            else
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "소켓 얼라인을 중지하시겠습니까?"))
                    return;

                Equipment.MachineStop_byUser = true;

                workStage.timer_VisionAlign.Enabled = false;
                //laserDrilling.StopThread();
                workStage.m_nSocketAlign_MainStep = (int)WorkStage.SocketAlign_Step.None;
            }
        }

        private void comboBox_Config_VisionPopup_AlignTest_SocketList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int m_nIndex = comboBox_Config_VisionPopup_AlignTest_SocketList.SelectedIndex;

            if (comboBox_Config_VisionPopup_AlignTest_SocketList.SelectedIndex < 0)
            {
                MessageBox.Show("소켓 리스트를 선택해야 합니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            textBox_Config_VisionPopup_AlignTest_Socket_CenterX.Text = workStage.m_stDividedRegion_GroupData[m_nIndex].dGroupCenter.X.ToString();
            textBox_Config_VisionPopup_AlignTest_Socket_CenterY.Text = workStage.m_stDividedRegion_GroupData[m_nIndex].dGroupCenter.Y.ToString();

            //  Fiducial Mark Pos 등록
            if (workStage.m_stDividedRegion_GroupData[m_nIndex].dFiducialPos.Length > 0)
            {
                comboBox_Config_VisionPopup_AlignTest_SelectedSocket_FiducialList.Items.Clear();

                for (int i = 0; i < workStage.m_stDividedRegion_GroupData[m_nIndex].dFiducialPos.Length; i++)
                {
                    comboBox_Config_VisionPopup_AlignTest_SelectedSocket_FiducialList.Items.Add(i);
                }
            }
        }

        private void button_Test_MoveTo_FiducialPos_Click(object sender, EventArgs e)
        {
            //  Fiducial Mark Pos. 이동

            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            
            int m_nSocketIndex = Equipment.ToInt(comboBox_Config_VisionPopup_AlignTest_SocketList.Text);
            if (m_nSocketIndex < 0)
            {
                MessageBox.Show("소켓 리스트를 선택해야 합니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int m_nFiducialIndex = comboBox_Config_VisionPopup_AlignTest_SelectedSocket_FiducialList.SelectedIndex;
            if (m_nFiducialIndex < 0)
            {
                MessageBox.Show("Fiducial 리스트를 선택해야 합니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (m_nFiducialIndex < workStage.m_stDividedRegion_GroupData[m_nSocketIndex].dFiducialPos.Length)
            {
                workStage.workStageParameter.stWorkStagePosParam = workStage.workStageParameter.GetPositionInformation("Processing");

                //  좌표계 (기존)
                workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.X] = 0.0;
                workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.Y] = 0.0;

                //  좌표계 변환 (Stage 좌표계와 Scanner 좌표계를 일치시키지 않을 경우에 사용. Stage 원점 위치에서 Scanner Center 까지의 Offset 거리를 더해서 이동시킨다.)
                workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.X] += Equipment.StageOffset_forDrilling_X;
                workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.Y] += Equipment.StageOffset_forDrilling_Y;

                //  데이터 위치를 Fine 카메라 위치로 변경
                workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.X] -= Equipment.stOffsetDistance.FromScannerToFineCam.X;
                workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.Y] -= Equipment.stOffsetDistance.FromScannerToFineCam.Y;

                //  저해상도 카메라가 활성일 경우
                if (radioButton_VisionPopup_CameraSelection_LowMag.Checked)
                {
                    //  데이터 위치를 Coarse 카메라 위치로 변경
                    workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.X] -= Equipment.stOffsetDistance.FromFineCamToCoarseCam.X;
                    workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.Y] -= Equipment.stOffsetDistance.FromFineCamToCoarseCam.Y;
                }

                //  Fiducial 위치 반영
                workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.X] -= workStage.m_stDividedRegion_GroupData[m_nSocketIndex].dFiducialPos[m_nFiducialIndex].X;
                workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.Y] -= workStage.m_stDividedRegion_GroupData[m_nSocketIndex].dFiducialPos[m_nFiducialIndex].Y;

                //  속도 설정
                if (radioButton_VisionPopup_Move_MoveMode_Fine.Checked)
                {
                    lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Fine;
                    lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Acceleration_Fine;
                }
                else
                {
                    lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Coarse;
                    lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Acceleration_Coarse;
                }

                xyInterpolatedCoordinate.X = workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.X];
                xyInterpolatedCoordinate.Y = workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.Y];
                workStage.MC_Func.MovePosition(xyInterpolatedCoordinate, lfVelocity, lfAccDec, lfAccDec);
            }
            else
            {
                MessageBox.Show("존재하지 않는 Fiducial 입니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void button_WorkStageMove_ToLowMagCamera_Click(object sender, EventArgs e)
        {
            //  to Low Mag. Camera


        }

        private void button_WorkStageMove_ToHighMagCamera_Click(object sender, EventArgs e)
        {

        }

        private void button_VisionPopup_WorkStage_StageCenter_To_ScannerCenter_Click(object sender, EventArgs e)
        {
            //  Stage Center 위치를 Scanner Center 위치로 이동

            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double lfVelocity_Z = 0.0f;
            double lfAccDec_Z = 0.0f;

            //if (!workStage.m_bHomeOK)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
            //    return;
            //}

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Stage 를 가공 위치로 보내시겠습니까?"))
                return;

            if (!workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.X) || !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Y) || !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Z) ||
                !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.X) || !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Y) || !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Z))
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Stage 가 이동중입니다.");
                return;
            }

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


            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //  맵 데이터 변경 (기준위치 : Scanner)
            //  기준위치로 보낼 때, 맵데이터를 변경한 후 보낸다.
            //  그 외에는, 위치로 보낸 후 맵데이터를 변경한다.
            workStage.MapData_Apply((int)WorkStage.nMapData_Type.MapData_Stage_Scanner);
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            //  속도 설정
            if (radioButton_VisionPopup_Move_MoveMode_Fine.Checked)
            {
                lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Fine;
                lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Acceleration_Fine;

                lfVelocity_Z = Equipment.stAxisParam[(int)WorkStage.nAxis.Z].Jog_Speed_Fine;
                lfAccDec_Z = Equipment.stAxisParam[(int)WorkStage.nAxis.Z].Common_Acceleration_Fine;
            }
            else
            {
                lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Coarse;
                lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Acceleration_Coarse;

                //  Z축은 빠르게 움직일 필요 없으니 일단 Fine 속도로 이동
                //lfVelocity_Z = Equipment.stAxisParam[(int)WorkStage.nAxis.Z].Jog_Speed_Coarse;
                //lfAccDec_Z = Equipment.stAxisParam[(int)WorkStage.nAxis.Z].Common_Acceleration_Coarse;
                lfVelocity_Z = Equipment.stAxisParam[(int)WorkStage.nAxis.Z].Jog_Speed_Fine;
                lfAccDec_Z = Equipment.stAxisParam[(int)WorkStage.nAxis.Z].Common_Acceleration_Fine;
            }

            //workStage.MC_Func.MC_MovePosition((int)WorkStage.nAxis.X, workStage.stWorkStageTeachingPos[(int)WorkStage_TeachingPosList.STAGE_ProcessingPos].Stage_X,
            //                                lfVelocity, lfAccDec, lfAccDec);
            //workStage.MC_Func.MC_MovePosition((int)WorkStage.nAxis.Y, workStage.stWorkStageTeachingPos[(int)WorkStage_TeachingPosList.STAGE_ProcessingPos].Stage_Y,
            //                                lfVelocity, lfAccDec, lfAccDec);

            xyInterpolatedCoordinate.X = workStage.stWorkStageTeachingPos[(int)WorkStage_TeachingPosList.STAGE_ProcessingPos].Stage_X;
            xyInterpolatedCoordinate.Y = workStage.stWorkStageTeachingPos[(int)WorkStage_TeachingPosList.STAGE_ProcessingPos].Stage_Y;
            workStage.MC_Func.MovePosition(xyInterpolatedCoordinate, lfVelocity, lfAccDec, lfAccDec);

            workStage.MC_Func.MC_MovePosition((int)WorkStage.nAxis.Z, vision.stVisionTeachingPos[(int)Vision_TeachingPosList.Laser_FocusPos].Vision_Z,
                                            lfVelocity_Z, lfAccDec_Z, lfAccDec_Z);

        }

        private void button_VisionPopup_WorkStage_CurrentScannerPos_To_FineCamPos_Click(object sender, EventArgs e)
        {
            //  현재 Scanner Center 위치를 Fine Camera Center 위치로 이동

            double lfTargetX = 0.0f;
            double lfTargetY = 0.0f;
            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;

            //if (!workStage.m_bHomeOK)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
            //    return;
            //}

            if (Equipment.stOffsetDistance.FromScannerToFineCam.X == 0.0 || Equipment.stOffsetDistance.FromScannerToFineCam.Y == 0.0)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Scanner Center 위치와 Fine Camera Center 위치의 Offset 거리가 설정되어 있지 않습니다.");
                return;
            }

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "현재 가공 위치를 Fine Camera 위치로 보내시겠습니까?"))
                return;

            if (!workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.X) || !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Y) || !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Z) ||
                !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.X) || !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Y) || !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Z))
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Stage 가 이동중입니다.");
                return;
            }

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



            //  Target 위치 계산
            lfTargetX = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X) - Equipment.stOffsetDistance.FromScannerToFineCam.X;
            lfTargetY = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y) - Equipment.stOffsetDistance.FromScannerToFineCam.Y;


            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //  맵 데이터 변경 (기준위치 : Scanner)
            //  기준위치로 보낼 때, 맵데이터를 변경한 후 보낸다.
            //  그 외에는, 위치로 보낸 후 맵데이터를 변경한다.
            workStage.MapData_Apply((int)WorkStage.nMapData_Type.MapData_Stage_FineCam);
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            
            //  속도 설정
            if (radioButton_VisionPopup_Move_MoveMode_Fine.Checked)
            {
                lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Fine;
                lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Acceleration_Fine;
            }
            else
            {
                lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Coarse;
                lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Acceleration_Coarse;
            }

            //workStage.MC_Func.MC_MovePosition((int)WorkStage.nAxis.X, lfTargetX, lfVelocity, lfAccDec, lfAccDec);
            //workStage.MC_Func.MC_MovePosition((int)WorkStage.nAxis.Y, lfTargetY, lfVelocity, lfAccDec, lfAccDec);

            xyInterpolatedCoordinate.X = lfTargetX;
            xyInterpolatedCoordinate.Y = lfTargetY;
            workStage.MC_Func.MovePosition(xyInterpolatedCoordinate, lfVelocity, lfAccDec, lfAccDec);
        }

        private void button_VisionPopup_WorkStage_CurrentFineCamPos_To_ScannerPos_Click(object sender, EventArgs e)
        {
            //  현재 Fine Camera Center 위치를 Scanner Center 위치로 이동

            double lfTargetX = 0.0f;
            double lfTargetY = 0.0f;
            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;

            //if (!workStage.m_bHomeOK)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
            //    return;
            //}

            if (Equipment.stOffsetDistance.FromScannerToFineCam.X == 0.0 || Equipment.stOffsetDistance.FromScannerToFineCam.Y == 0.0)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Scanner Center 위치와 Fine Camera Center 위치의 Offset 거리가 설정되어 있지 않습니다.");
                return;
            }

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "현재 Fine Camera 위치를 가공 위치로 보내시겠습니까?"))
                return;

            if (!workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.X) || !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Y) || !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Z) ||
                !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.X) || !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Y) || !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Z))
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Stage 가 이동중입니다.");
                return;
            }

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


            //  Target 위치 계산
            lfTargetX = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X) + Equipment.stOffsetDistance.FromScannerToFineCam.X;
            lfTargetY = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y) + Equipment.stOffsetDistance.FromScannerToFineCam.Y;



            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //  맵 데이터 변경 (기준위치 : Scanner)
            //  기준위치로 보낼 때, 맵데이터를 변경한 후 보낸다.
            //  그 외에는, 위치로 보낸 후 맵데이터를 변경한다.
            workStage.MapData_Apply((int)WorkStage.nMapData_Type.MapData_Stage_Scanner);
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            //  속도 설정
            if (radioButton_VisionPopup_Move_MoveMode_Fine.Checked)
            {
                lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Fine;
                lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Acceleration_Fine;
            }
            else
            {
                lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Coarse;
                lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Acceleration_Coarse;
            }

            //workStage.MC_Func.MC_MovePosition((int)WorkStage.nAxis.X, lfTargetX, lfVelocity, lfAccDec, lfAccDec);
            //workStage.MC_Func.MC_MovePosition((int)WorkStage.nAxis.Y, lfTargetY, lfVelocity, lfAccDec, lfAccDec);

            xyInterpolatedCoordinate.X = lfTargetX;
            xyInterpolatedCoordinate.Y = lfTargetY;
            workStage.MC_Func.MovePosition(xyInterpolatedCoordinate, lfVelocity, lfAccDec, lfAccDec);
        }

        private void button_VisionPopup_WorkStage_CurrentFineCamPos_To_CoarseCamPos_Click(object sender, EventArgs e)
        {
            //  현재 Fine Camera Center 위치를 Coarse Camera Center 위치로 이동

            double lfTargetX = 0.0f;
            double lfTargetY = 0.0f;
            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;

            //if (!workStage.m_bHomeOK)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
            //    return;
            //}

            if (Equipment.stOffsetDistance.FromFineCamToCoarseCam.X == 0.0 || Equipment.stOffsetDistance.FromFineCamToCoarseCam.Y == 0.0)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Fine Camera Center 위치와 Coarse Camera Center 위치의 Offset 거리가 설정되어 있지 않습니다.");
                return;
            }

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "현재 Fine Camera 위치를 Coarse Camera 위치로 보내시겠습니까?"))
                return;

            if (!workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.X) || !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Y) || !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Z) ||
                !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.X) || !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Y) || !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Z))
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Stage 가 이동중입니다.");
                return;
            }

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


            //  Target 위치 계산
            lfTargetX = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X) - Equipment.stOffsetDistance.FromFineCamToCoarseCam.X;
            lfTargetY = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y) - Equipment.stOffsetDistance.FromFineCamToCoarseCam.Y;

            //  속도 설정
            if (radioButton_VisionPopup_Move_MoveMode_Fine.Checked)
            {
                lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Fine;
                lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Acceleration_Fine;
            }
            else
            {
                lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Coarse;
                lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Acceleration_Coarse;
            }

            //workStage.MC_Func.MC_MovePosition((int)WorkStage.nAxis.X, lfTargetX, lfVelocity, lfAccDec, lfAccDec);
            //workStage.MC_Func.MC_MovePosition((int)WorkStage.nAxis.Y, lfTargetY, lfVelocity, lfAccDec, lfAccDec);

            xyInterpolatedCoordinate.X = lfTargetX;
            xyInterpolatedCoordinate.Y = lfTargetY;
            workStage.MC_Func.MovePosition(xyInterpolatedCoordinate, lfVelocity, lfAccDec, lfAccDec);


            //  저해상도 카메라 위치로 보냈으니, 카메라도 저해상도로 Live 한다.
            radioButton_VisionPopup_CameraSelection_LowMag.PerformClick();
        }

        private void button_VisionPopup_WorkStage_CurrentCoarseCamPos_To_FineCamPos_Click(object sender, EventArgs e)
        {
            //  현재 Coarse Camera Center 위치를 Fine Camera Center 위치로 이동

            double lfTargetX = 0.0f;
            double lfTargetY = 0.0f;
            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;

            //if (!workStage.m_bHomeOK)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
            //    return;
            //}

            if (Equipment.stOffsetDistance.FromFineCamToCoarseCam.X == 0.0 || Equipment.stOffsetDistance.FromFineCamToCoarseCam.Y == 0.0)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Fine Camera Center 위치와 Coarse Camera Center 위치의 Offset 거리가 설정되어 있지 않습니다.");
                return;
            }

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "현재 Coarse Camera 위치를 Fine Camera 위치로 보내시겠습니까?"))
                return;

            if (!workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.X) || !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Y) || !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Z) ||
                !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.X) || !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Y) || !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Z))
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Stage 가 이동중입니다.");
                return;
            }

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


            ////  맵 데이터 변경
            //workStage.MapData_Apply((int)WorkStage.nMapData_Type.MapData_Stage_FineCam);


            //  Target 위치 계산
            lfTargetX = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X) + Equipment.stOffsetDistance.FromFineCamToCoarseCam.X;
            lfTargetY = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y) + Equipment.stOffsetDistance.FromFineCamToCoarseCam.Y;


            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //  맵 데이터 변경 (기준위치 : Scanner)
            //  기준위치로 보낼 때, 맵데이터를 변경한 후 보낸다.
            //  그 외에는, 위치로 보낸 후 맵데이터를 변경한다.
            workStage.MapData_Apply((int)WorkStage.nMapData_Type.MapData_Stage_FineCam);
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            //  속도 설정
            if (radioButton_VisionPopup_Move_MoveMode_Fine.Checked)
            {
                lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Fine;
                lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Acceleration_Fine;
            }
            else
            {
                lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Coarse;
                lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Acceleration_Coarse;
            }

            //workStage.MC_Func.MC_MovePosition((int)WorkStage.nAxis.X, lfTargetX, lfVelocity, lfAccDec, lfAccDec);
            //workStage.MC_Func.MC_MovePosition((int)WorkStage.nAxis.Y, lfTargetY, lfVelocity, lfAccDec, lfAccDec);


            //여기 이상하잖아? 걍 현재 위치에서 + 해서 FineCam으로 가는거지
            xyInterpolatedCoordinate.X = lfTargetX;
            xyInterpolatedCoordinate.Y = lfTargetY;
            workStage.MC_Func.MovePosition(xyInterpolatedCoordinate, lfVelocity, lfAccDec, lfAccDec);


            //  고해상도 카메라 위치로 보냈으니, 카메라도 고해상도로 Live 한다.
            radioButton_VisionPopup_CameraSelection_HighMag.PerformClick();
        }

        private void button_VisionPopup_WorkStage_CurrentLaserSensorPos_To_FineCamPos_Click(object sender, EventArgs e)
        {
            //  현재 Laser Sensor 위치를 Fine Camera Center 위치로 이동

            double lfTargetX = 0.0f;
            double lfTargetY = 0.0f;
            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;

            //if (!workStage.m_bHomeOK)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
            //    return;
            //}

            if (Equipment.stOffsetDistance.FromFineCamToLaserHeightSensor.X == 0.0 || Equipment.stOffsetDistance.FromFineCamToLaserHeightSensor.Y == 0.0)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Laser Height Sensor 위치와 Fine Camera Center 위치의 Offset 거리가 설정되어 있지 않습니다.");
                return;
            }

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "현재 Laser Height Sensor 위치를 Fine Camera 위치로 보내시겠습니까?"))
                return;

            if (!workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.X) || !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Y) || !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Z) ||
                !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.X) || !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Y) || !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Z))
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Stage 가 이동중입니다.");
                return;
            }

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


            ////  맵 데이터 변경
            //workStage.MapData_Apply((int)WorkStage.nMapData_Type.MapData_Stage_FineCam);


            //  Target 위치 계산
            lfTargetX = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X) - Equipment.stOffsetDistance.FromFineCamToLaserHeightSensor.X;
            lfTargetY = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y) - Equipment.stOffsetDistance.FromFineCamToLaserHeightSensor.Y;


            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //  맵 데이터 변경 (기준위치 : Scanner)
            //  기준위치로 보낼 때, 맵데이터를 변경한 후 보낸다.
            //  그 외에는, 위치로 보낸 후 맵데이터를 변경한다.
            workStage.MapData_Apply((int)WorkStage.nMapData_Type.MapData_Stage_FineCam);
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            //  속도 설정
            if (radioButton_VisionPopup_Move_MoveMode_Fine.Checked)
            {
                lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Fine;
                lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Acceleration_Fine;
            }
            else
            {
                lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Coarse;
                lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Acceleration_Coarse;
            }

            //workStage.MC_Func.MC_MovePosition((int)WorkStage.nAxis.X, lfTargetX, lfVelocity, lfAccDec, lfAccDec);
            //workStage.MC_Func.MC_MovePosition((int)WorkStage.nAxis.Y, lfTargetY, lfVelocity, lfAccDec, lfAccDec);

            xyInterpolatedCoordinate.X = lfTargetX;
            xyInterpolatedCoordinate.Y = lfTargetY;
            workStage.MC_Func.MovePosition(xyInterpolatedCoordinate, lfVelocity, lfAccDec, lfAccDec);
        }

        private void button_VisionPopup_WorkStage_CurrentFineCamPos_To_LaserSensorPos_Click(object sender, EventArgs e)
        {
            //  현재 Fine Camera Center 위치를 Laser Sensor 위치로 이동

            double lfTargetX = 0.0f;
            double lfTargetY = 0.0f;
            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;

            //if (!workStage.m_bHomeOK)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
            //    return;
            //}

            if (Equipment.stOffsetDistance.FromFineCamToLaserHeightSensor.X == 0.0 || Equipment.stOffsetDistance.FromFineCamToLaserHeightSensor.Y == 0.0)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Laser Height Sensor 위치와 Fine Camera Center 위치의 Offset 거리가 설정되어 있지 않습니다.");
                return;
            }

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "현재 Fine Camera 위치를 Laser Height Sensor 위치로 보내시겠습니까?"))
                return;

            if (!workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.X) || !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Y) || !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Z) ||
                !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.X) || !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Y) || !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Z))
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Stage 가 이동중입니다.");
                return;
            }

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


            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ////  맵 데이터 변경 (기준위치 : Scanner)
            ////  기준위치로 보낼 때, 맵데이터를 변경한 후 보낸다.
            ////  그 외에는, 위치로 보낸 후 맵데이터를 변경한다.
            //workStage.MapData_Apply((int)WorkStage.nMapData_Type.MapData_Stage_Scanner);
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            //  Target 위치 계산
            lfTargetX = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X) + Equipment.stOffsetDistance.FromFineCamToLaserHeightSensor.X;
            lfTargetY = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y) + Equipment.stOffsetDistance.FromFineCamToLaserHeightSensor.Y;

            //  속도 설정
            if (radioButton_VisionPopup_Move_MoveMode_Fine.Checked)
            {
                lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Fine;
                lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Acceleration_Fine;
            }
            else
            {
                lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Coarse;
                lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Acceleration_Coarse;
            }

            //workStage.MC_Func.MC_MovePosition((int)WorkStage.nAxis.X, lfTargetX, lfVelocity, lfAccDec, lfAccDec);
            //workStage.MC_Func.MC_MovePosition((int)WorkStage.nAxis.Y, lfTargetY, lfVelocity, lfAccDec, lfAccDec);

            xyInterpolatedCoordinate.X = lfTargetX;
            xyInterpolatedCoordinate.Y = lfTargetY;
            workStage.MC_Func.MovePosition(xyInterpolatedCoordinate, lfVelocity, lfAccDec, lfAccDec);
        }

        private void button_VisionPopup_FindMetalPowder_Search_Click(object sender, EventArgs e)
        {
            //  메탈 파우더 찾기

            bool m_bFindCircle = false;

            int m_nImage_Width = 0;
            int m_nImage_Height = 0;

            detectedCircles.Clear();

            QMC_ImageProcessFindAlign aligner = new QMC_ImageProcessFindAlign();
            List<RectangleF> circlesResult = new List<RectangleF>();

            if (radioButton_VisionPopup_CameraSelection_LowMag.Checked)
            {
                int w = workStage.Camera_LowRes.Resolution.Width;
                int h = workStage.Camera_LowRes.Resolution.Height;

                m_nImage_Width = w;
                m_nImage_Height = h;

                // Bitmap을 byte 배열로 변환
                //byte[] pixelData = aligner.ConvertBitmapToByteArray(bm_Temp);

                //aligner.FindCirclesWidthCircleBoundary(circlesResult, bm_RawData, w, h, ref m_bFindCircle);
                aligner.FindMetalPowder(circlesResult, bm_RawData, w, h, ref m_bFindCircle);
            }
            else
            {
                int w = workStage.Camera_HighRes.Resolution.Width;
                int h = workStage.Camera_HighRes.Resolution.Height;

                m_nImage_Width = w;
                m_nImage_Height = h;

                // Bitmap을 byte 배열로 변환
                //byte[] pixelData = aligner.ConvertBitmapToByteArray(bm_Temp);                

                //aligner.FindCirclesWidthCircleBoundary(circlesResult, bm_RawData, w, h, ref m_bFindCircle);
                //aligner.FindMetalPowder(circlesResult, bm_RawData, w, h, ref m_bFindCircle);
                aligner.FindGoldPowderForAutoTreshold(circlesResult,
                                                        bm_RawData,
                                                        w,
                                                        h,
                                                        75,
                                                        Equipment.stVisionRecipeSet.dSocketCircleMarkScore,
                                                        Equipment.stVisionRecipeSet.dSocketCircleMarkSpec);
            }

            if (circlesResult.Count > 0)
            { 
                detectedCircles.Clear();

                //  좌표 표시
                listBox_FindCircle_Result.Items.Clear();
                for (int i = 0; i < circlesResult.Count; i++)
                {
                    listBox_FindCircle_Result.Items.Add((i + 1) + ".  Center X : " + circlesResult[i].X);
                    listBox_FindCircle_Result.Items.Add((i + 1) + ".  Center Y : " + circlesResult[i].Y);
                    listBox_FindCircle_Result.Items.Add((i + 1) + ".  Width : " + circlesResult[i].Width);
                    listBox_FindCircle_Result.Items.Add((i + 1) + ".  Height : " + circlesResult[i].Height);
                }

                foreach (var circle in circlesResult)
                {
                    //float ratioX = (float)pictureBox_ImageDisplay.Width / bm_Temp.Width;
                    //float ratioY = (float)pictureBox_ImageDisplay.Height / bm_Temp.Height;
                    float ratioX = (float)pictureBox_ImageDisplay.Width / m_nImage_Width;
                    float ratioY = (float)pictureBox_ImageDisplay.Height / m_nImage_Height;
                    float ratio = Math.Min(ratioX, ratioY);

                    int newX = (int)(circle.X * ratio);
                    int newY = (int)(circle.Y * ratio);
                    int newWidth = (int)(circle.Width * ratio);
                    int newHeight = (int)(circle.Height * ratio);

                    detectedCircles.Add(new Rectangle(newX, newY, newWidth, newHeight));
                    pictureBox_ImageDisplay.Invalidate(); // PictureBox를 다시 그리도록 요청

                }

                // 원의 좌표를 이미지 비율에 맞게 변환

            }
            else
            {
                MessageBox.Show("원 찾기 실패", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                detectedCircles.Clear();

                listBox_FindCircle_Result.Items.Clear();
            }

            //  원 찾기 후 다시 Live
            if (radioButton_VisionPopup_CameraSelection_LowMag.Checked)
            {
                if (workStage.Camera_LowRes.Opened)
                {
                    workStage.Camera_LowRes.StartLive();
                }
            }
            else
            {
                if (workStage.Camera_HighRes.Opened)
                {
                    workStage.Camera_HighRes.StartLive();
                }
            }
        }

        private void button_Test_MoveTo_CorrectedPos_Click(object sender, EventArgs e)
        {
            //  보정된 Mark Pos. 이동

            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            bool m_bCorrected = true;

            int m_nSocketIndex = Equipment.ToInt(comboBox_Config_VisionPopup_AlignTest_SocketList.Text);
            if (m_nSocketIndex < 0)
            {
                MessageBox.Show("소켓 리스트를 선택해야 합니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int m_nFiducialIndex = comboBox_Config_VisionPopup_AlignTest_SelectedSocket_FiducialList.SelectedIndex;
            if (m_nFiducialIndex < 0)
            {
                MessageBox.Show("Fiducial 리스트를 선택해야 합니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //  얼라인이 완료 되었는지
            for ( int i = 0; i < 4; i++)
            {
                if ((workStage.m_st4PointPosition_InspectedPos[i].dFiducial_Width == 0) || (workStage.m_st4PointPosition_InspectedPos[i].dFiducial_Height == 0))
                {
                    m_bCorrected = false;
                }
            }

            if (!m_bCorrected)
            {
                MessageBox.Show("Socket Align 을 진행해야 합니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (m_nFiducialIndex < workStage.m_stDividedRegion_GroupData[m_nSocketIndex].dFiducialPos.Length)
            {
                workStage.workStageParameter.stWorkStagePosParam = workStage.workStageParameter.GetPositionInformation("Processing");

                //  좌표계 (기존)
                workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.X] = 0.0;
                workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.Y] = 0.0;

                //  좌표계 변환 (Stage 좌표계와 Scanner 좌표계를 일치시키지 않을 경우에 사용. Stage 원점 위치에서 Scanner Center 까지의 Offset 거리를 더해서 이동시킨다.)
                workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.X] += Equipment.StageOffset_forDrilling_X;
                workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.Y] += Equipment.StageOffset_forDrilling_Y;

                //  데이터 위치를 Fine 카메라 위치로 변경
                workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.X] -= Equipment.stOffsetDistance.FromScannerToFineCam.X;
                workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.Y] -= Equipment.stOffsetDistance.FromScannerToFineCam.Y;

                //  저해상도 카메라가 활성일 경우
                if (radioButton_VisionPopup_CameraSelection_LowMag.Checked)
                {
                    //  데이터 위치를 Coarse 카메라 위치로 변경
                    workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.X] -= Equipment.stOffsetDistance.FromFineCamToCoarseCam.X;
                    workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.Y] -= Equipment.stOffsetDistance.FromFineCamToCoarseCam.Y;
                }

                //  Fiducial 위치 반영
                workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.X] -= workStage.m_stDividedRegion_GroupData[m_nSocketIndex].dFiducialPos[m_nFiducialIndex].X;
                workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.Y] -= workStage.m_stDividedRegion_GroupData[m_nSocketIndex].dFiducialPos[m_nFiducialIndex].Y;

                //  보정량 반영
                workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.X] += (workStage.m_stDividedRegion_GroupData[m_nSocketIndex].dFiducialPos[m_nFiducialIndex].X - 
                                                                                                                workStage.m_st4PointPosition_InspectedPos[m_nFiducialIndex].ptFiducial_Center.X) ;
                workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.Y] += (workStage.m_stDividedRegion_GroupData[m_nSocketIndex].dFiducialPos[m_nFiducialIndex].Y -
                                                                                                                workStage.m_st4PointPosition_InspectedPos[m_nFiducialIndex].ptFiducial_Center.Y) ;

                //  속도 설정
                if (radioButton_VisionPopup_Move_MoveMode_Fine.Checked)
                {
                    lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Fine;
                    lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Acceleration_Fine;
                }
                else
                {
                    lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Coarse;
                    lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Acceleration_Coarse;
                }

                xyInterpolatedCoordinate.X = workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.X];
                xyInterpolatedCoordinate.Y = workStage.workStageParameter.stWorkStagePosParam.dTarget[(int)WorkStageParameter.MotionKey.Y];
                workStage.MC_Func.MovePosition(xyInterpolatedCoordinate, lfVelocity, lfAccDec, lfAccDec);
            }
            else
            {
                MessageBox.Show("존재하지 않는 Fiducial 입니다.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void button_VisionPopup_LaserHeightCheck_Start_Click(object sender, EventArgs e)
        {
            //  현재 위치 레이저 높이 측정

            int m_nCameraType = radioButton_VisionPopup_CameraSelection_LowMag.Checked ? (int)WorkStage.nCameraType.Cam_LowRes : (int)WorkStage.nCameraType.Cam_HighRes;

            if (workStage.m_nLaserHeightCheck_Step == (int)WorkStage.LaserHeightCheck_Step.None)
            {

                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Laser Sensor 높이 측정을 시작하시겠습니까?"))
                    return;

                textBox_VisionPopup_LaserHeightValue.Text = "Checking...";

                workStage.m_nLaserHeightCheck_Camera = m_nCameraType;
                workStage.m_bLaserHeightCheck_Complete = false;
                workStage.m_dLaserHeightCheck_Value = 0.0;
                workStage.m_nLaserHeightCheck_Step = (int)WorkStage.LaserHeightCheck_Step.Start;

                workStage.timer_VerifyScannerCamOffset.Enabled = true;
                //workStage.timer_VisionAlign.Enabled = true;
            }
            else
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Laser Sensor 높이 측정을 중지하시겠습니까?"))
                    return;

                Equipment.MachineStop_byUser = true;

                //workStage.timer_VisionAlign.Enabled = false;
                //laserDrilling.StopThread();
                workStage.m_nLaserHeightCheck_Step = (int)WorkStage.LaserHeightCheck_Step.None;

                workStage.MC_Func.MC_MotorStop((int)WorkStage.nAxis.X, 2000);
                workStage.MC_Func.MC_MotorStop((int)WorkStage.nAxis.Y, 2000);
                workStage.MC_Func.MC_MotorStop((int)WorkStage.nAxis.Z, 2000);
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            int result = workStage.scannerCompensator.RunSearchMark();

            PatternMatchingResult result1 = workStage.scannerCompensator.GetResult();

            if (result1.Values.Count > 0)
            {
                double markPixelX = result1.Values[0].X;
                double markPixelY = result1.Values[0].Y;

                MessageBox.Show("Search Center Mark - OK");
            }

            double markPositionX = workStage.scannerCompensator.ResultPosition.X;
            double markPositionY = workStage.scannerCompensator.ResultPosition.Y;
        }

        private void btnTrain_Click(object sender, EventArgs e)
        {

        }

        private void button34_Click(object sender, EventArgs e)
        {

        }

        private void button47_Click(object sender, EventArgs e)
        {

        }

        private void button_VisionPopup_Search_Click(object sender, EventArgs e)
        {
            PatternMatchingResult result;
            int nImage_Width = 0;
            int nImage_Height = 0;

            if (radioButton_VisionPopup_CameraSelection_LowMag.Checked)
            {
                int w = workStage.Camera_LowRes.Resolution.Width;
                int h = workStage.Camera_LowRes.Resolution.Height;

                nImage_Width = w;
                nImage_Height = h;

                result = workStage.jigAligner_LowRes.Search();
                //PatternMatchingResult result1 = workStage.scannerCompensator.GetResult();
            }
            else
            {
                //안한다고 봐야함..?
                int w = workStage.Camera_HighRes.Resolution.Width;
                int h = workStage.Camera_HighRes.Resolution.Height;

                nImage_Width = w;
                nImage_Height = h;

                result = workStage.jigAligner_LowRes.Search();
                //PatternMatchingResult result1 = workStage.scannerCompensator.GetResult();
            }

            if (result.Values.Count > 0)
            {
                double markPixelX = result.Values[0].X;
                double markPixelY = result.Values[0].Y;
                double markR = result.Values[0].R;

                MessageBox.Show("Search Mark - OK");
            

                detectedCircles.Clear();
                //  좌표 표시
                listBox_VisionPopup_PM_Result.Items.Clear();
                for (int i = 0; i < result.Values.Count; i++)
                {
                    listBox_VisionPopup_PM_Result.Items.Add((i + 1) + "X:" + result.Values[i].X);
                    listBox_VisionPopup_PM_Result.Items.Add((i + 1) + "Y :" + result.Values[i].Y);
                    listBox_VisionPopup_PM_Result.Items.Add((i + 1) + "R :" + result.Values[i].R);
                }

                foreach (var circle in result.ResultOverlays)
                {
                    //float ratioX = (float)pictureBox_ImageDisplay.Width / bm_Temp.Width;
                    //float ratioY = (float)pictureBox_ImageDisplay.Height / bm_Temp.Height;
                    float ratioX = (float)pictureBox_VisionPopup_PM_ImageDisplay.Width / nImage_Width;
                    float ratioY = (float)pictureBox_VisionPopup_PM_ImageDisplay.Height / nImage_Height;
                    float ratio = Math.Min(ratioX, ratioY);

                    int newWidth = (int)(circle.Size.Width * ratio);
                    int newHeight = (int)(circle.Size.Width * ratio);
                    int newX = (int)(result.Values[0].X - (newWidth / 2) * ratio);
                    int newY = (int)(result.Values[0].Y - (newHeight / 2) * ratio);
                    
                    detectedCircles.Add(new Rectangle(newX, newY, newWidth, newHeight));
                    pictureBox_VisionPopup_PM_ImageDisplay.Invalidate(); // PictureBox를 다시 그리도록 요청

                }
            }
            else
            {
                MessageBox.Show("검출 실패", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                detectedCircles.Clear();

                listBox_FindCircle_Result.Items.Clear();
            }

            //  원 찾기 후 다시 Live
            if (radioButton_VisionPopup_CameraSelection_LowMag.Checked)
            {
                if (workStage.Camera_LowRes.Opened)
                {
                    workStage.Camera_LowRes.StartLive();
                }
            }
            else
            {
                if (workStage.Camera_HighRes.Opened)
                {
                    workStage.Camera_HighRes.StartLive();
                }
            }
        }

        private void button_VisionPopup_PM_Grab_Click(object sender, EventArgs e)
        {
            //  이미지 Grab
            detectedCircles.Clear();
            listBox_VisionPopup_PM_Result.Items.Clear();

            Image image = new Bitmap(workStage.Camera_LowRes.Resolution.Width, workStage.Camera_LowRes.Resolution.Height);

            //  Grab
            if (radioButton_VisionPopup_CameraSelection_LowMag.Checked)
            {
                if (true)
                {
                    workStage.Camera_LowRes.Grab();

                    bm_RawData = new byte[workStage.Camera_LowRes.Resolution.Width * workStage.Camera_LowRes.Resolution.Height];
                    bm_RawData = workStage.Camera_LowRes.LatestImage.RawData;
                    image = workStage.Camera_LowRes.LatestImage.GetImage();
                }
                else //Test Code
                {
                    workStage.Camera_LowRes.LatestImage = m_visionImageViewer_LowRes.InputImage;

                    bm_RawData = new byte[workStage.Camera_LowRes.Resolution.Width * workStage.Camera_LowRes.Resolution.Height];
                    //if (m_visionImageViewer_LowRes.InputImage.RawData[0]. > 0)
                    {
                        bm_RawData = workStage.Camera_LowRes.LatestImage.RawData;
                        image = workStage.Camera_LowRes.LatestImage.GetImage();
                        workStage.jigAligner_LowRes.Simulated = true;
                        workStage.jigAligner_LowRes.TestImage = image;
                    }
                }
                

                //workStage.Camera_LowRes.LatestImage.Save("D:\\TempImage_LowRes.bmp", QMC.Common.Vision.VisionImage.FileFilter.bmp);
                //bm_Temp = new Bitmap("D:\\TempImage_LowRes.bmp");                
            }
            else
            {
                workStage.Camera_HighRes.Grab();

                bm_RawData = new byte[workStage.Camera_HighRes.Resolution.Width * workStage.Camera_HighRes.Resolution.Height];
                bm_RawData = workStage.Camera_HighRes.LatestImage.RawData;

                image = workStage.Camera_HighRes.LatestImage.GetImage();

                //workStage.Camera_HighRes.LatestImage.Save("D:\\TempImage_HighRes.bmp", QMC.Common.Vision.VisionImage.FileFilter.bmp);
                //bm_Temp = new Bitmap("D:\\TempImage_HighRes.bmp");
            }

            //pictureBox_ImageDisplay.Image = ResizeImageToFitPictureBox(bm_Temp, pictureBox_ImageDisplay);
            pictureBox_VisionPopup_PM_ImageDisplay.Image = ResizeImageToFitPictureBox(image, pictureBox_VisionPopup_PM_ImageDisplay);
        }

        private void pictureBox_VisionPopup_PM_ImageDisplay_paint(object sender, PaintEventArgs e)
        {
            int nIndex = 0;
            foreach (var detectedCircle in detectedCircles)
            {
                Color color = Color.Blue;
                switch (nIndex)
                {
                    case 1:
                        color = Color.Red;
                        break;
                    case 2:
                        color = Color.Lime;
                        break;
                    case 3:
                        color = Color.Yellow;
                        break;
                }

                using (Pen pen = new Pen(color, 1)) // 펜의 두께를 3포인트로 설정
                {
                    e.Graphics.DrawRectangle(pen, detectedCircle);
                }
                nIndex++;
            }
        }
    }
}
