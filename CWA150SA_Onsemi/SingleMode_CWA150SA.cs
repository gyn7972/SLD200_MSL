using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using QMC.Core;
using QMC.Common;
using QMC.Common.Parts;
using QMC.Common.Modules;
using QMC.Common.Motion.Ajin.Motions;
using ACS.SPiiPlusNET;
using static QMC.Common.Modules.WaferProbeAlign;
//using SpiralLab.Sirius;
//using QMC.Common.Motion.ACS.Motions;
using OpenCvSharp.Dnn;
using QMC.Common.Vision.Cameras;
using System.Threading;
using netDxf.Entities;
using static QMC.Common.Equipment;
using QMC.Common.VisionPart;
using MessageBox = System.Windows.Forms.MessageBox;
using OpenCvSharp.Flann;
using System.IO;
using OpenCvSharp.Internal;
using System.Net.NetworkInformation;
using NativeMethods = QMC.Core.NativeMethods;
using SpiralLab;
using SpiralLab.Sirius;
using QMC.Vision;
using Point = System.Drawing.Point;
using FontStyle = System.Drawing.FontStyle;
using Rectangle = System.Drawing.Rectangle;
using OpenCvSharp;
using QMC.Common.Motion.ACS.Motions;
using QMC.Common.Laser;
using QMC.Process.WaferProbeAlign.Parts;

namespace CWA150SA_Onsemi300
{
    public partial class SingleMode_CWA150SA : UserControl
    {
        public event JogButtonClickEventHandler JogButtonClick;
        public event JogButtonDownEventHandler JogButtonDown;
        public event JogButtonUpEventHandler JogButtonUp;
        public List<MotionAxis> AxisList { get; set; }
        public MotionAxis thetaValue { set; get; }

        static WaferProbeAlign waferProbeAlign;

        //public static Api ACS_Motion = new Api();

        private Thread m_singleModeThread;
        private bool m_bSingleModeThreadExit;

        public ProgressForm m_FormProgress;                             //  장비 초기화 시 진행창 표시

        public bool m_bMessageBox_Showed = false;

        public System.Windows.Forms.Timer timer_ACS_Status;
        public System.Windows.Forms.Timer timer_SingleMode_DIO_Status;

        MotionFunction MC_Func = new MotionFunction();

        //protected XyztStage m_Stage;
        //protected XyzztStage m_Stage;
        protected UvwzxyzStage m_Stage;
        //private _2DMappingDataControl m_2DMappingDataControl;
        //private _2DMappingFileControl m_2DMappingFileControl;

        //public CepheusWrapDll CepheusLaser = new CepheusWrapDll();

        //public QMC.Common.Stage StageCompenData = new QMC.Common.Stage();

        int count;
        //short axis = -1;
        short index = -1;
        int rowIndex;
        int columnIndex;

        int m_nBlink;
        bool m_bBlink;

        #region Tick Count Check

        public enum TickType : int
        {
            TICK_USER = 0,          //  0 : User
            TICK_USER2 = 1,         //  0 : User2
            TICK_USER3 = 2,         //  0 : User3
        }

        public int[,] TickCount_Cycle = new int[10, 2];          //  0 : User
                                                                 //  1 : 
                                                                 //  2 : 

        public void TickCount_Start(int m_nIndex)
        {
            TickCount_Cycle[m_nIndex, 0] = Environment.TickCount;
        }
        public int TickCount_Elapsed(int m_nIndex)
        {
            int TickCount_Elapsed = 0;
            TickCount_Cycle[m_nIndex, 1] = Environment.TickCount;
            TickCount_Elapsed = TickCount_Cycle[m_nIndex, 1] - TickCount_Cycle[m_nIndex, 0];

            return TickCount_Elapsed;
        }
        #endregion

        public SingleMode_CWA150SA()
        {
            InitializeComponent();
            AxisList = new List<MotionAxis>();

            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;

            foreach (Module module in m_collectionModules)
            {
                if (module.Name == "WaferProbeAlign")
                {
                    waferProbeAlign = module as WaferProbeAlign;
                }
            }

            m_nBlink = 0;
            m_bBlink = false;

            //  Single Mode 창의 DIO 상태를 갱신하는 타이머
            timer_SingleMode_DIO_Status = new System.Windows.Forms.Timer();
            timer_SingleMode_DIO_Status.Interval = 50;
            timer_SingleMode_DIO_Status.Tick += new System.EventHandler(timer_SingleMode_DIO_Status_Tick);
            timer_SingleMode_DIO_Status.Enabled = true;

            //  Thread Start
            //ThreadStart();

            //  Water Line 은 상시 On -> SLO 장비엔 이것이 없는 듯...?
            //waferProbeAlign.waferProbeAlignParameter.DO_LaserWater_In(true);
            //waferProbeAlign.waferProbeAlignParameter.DO_LaserWater_Out(false);
            //waferProbeAlign.waferProbeAlignParameter.DO_ScannerWater_In(true);
            //waferProbeAlign.waferProbeAlignParameter.DO_ScannerWater_Out(false);

            m_FormProgress = new ProgressForm("Laser Initialize", "레이저 초기화 진행중...");

            //this.m_2DMappingDataControl = new _2DMappingDataControl(m_Stage);
            //this.m_2DMappingDataControl.Location = new Point(11, 6);
            ////this.tabPage_2DMapping.Controls.Add(this.m_2DMappingDataControl);

            //this.m_2DMappingFileControl = new _2DMappingFileControl(m_Stage);
            //this.m_2DMappingFileControl.Location = new Point(m_2DMappingDataControl.Location.X + m_2DMappingDataControl.Size.Width + 20, 6);
            ////this.tabPage_2DMapping.Controls.Add(this.m_2DMappingFileControl);
        }

        #region Thread

        public void ThreadStart()
        {
            //  Drilling Cycle Thread
            m_bSingleModeThreadExit = false;
            m_singleModeThread = new Thread(new ThreadStart(OnSingleModeStatusCycle));
            m_singleModeThread.Start();
        }

        public void ThreadStop()
        {
            m_bSingleModeThreadExit = true;

            if (m_singleModeThread != null)
                m_singleModeThread.Join();
        }

        protected void OnSingleModeStatusCycle()
        {
            while (true)
            {
                if (m_bSingleModeThreadExit)
                {
                    break;
                }
                if (OnSingleModeStatusRun() != 0) break;
                Thread.Sleep(1);
            }
        }

        protected int OnSingleModeStatusRun()
        {
            int ret = 0;

            //SingleModeUI_DIO_Status();      //  DIO Status

            if (!m_FormProgress.HasChildren)            //  Progress 창을 실수로 닫았다면, 다시 메모리 할당하자.
            {
                m_FormProgress = new ProgressForm("Laser Initialize", "레이저 초기화 진행중...");
            }

            m_FormProgress.StartPosition = FormStartPosition.CenterScreen;
            m_FormProgress.TopMost = true;
            m_FormProgress.Show();

            ThreadStop();

            return ret;
        }
        #endregion


        private void timer_SingleMode_DIO_Status_Tick(object sender, EventArgs e)
        {
            //  테스트 : SingleMode 화면 DIO 상태 갱신

            SingleModeUI_DIO_Status();
            UpdateStatus();
        }

        private void SingleModeUI_DIO_Status()
        {
            if (waferProbeAlign.m_nHomeStep == (int)WaferProbeAlign.Home_Step.Complete)
            {
                m_FormProgress.Hide();
            }

            ///////////////////////////////////////////////////////////////////////////////////////
            //  Laser Status
            //

            //if (!waferProbeAlign.waferProbeAlignParameter.IsDO_LaserWater_In() || !waferProbeAlign.waferProbeAlignParameter.IsDO_ScannerWater_In() ||
            //    waferProbeAlign.waferProbeAlignParameter.IsDO_LaserWater_Out() || waferProbeAlign.waferProbeAlignParameter.IsDO_ScannerWater_Out())
            //{
            //    if (waferProbeAlign.m_bLaser_Emission)
            //    {
            //        waferProbeAlign.SpectraPhysicsLaserComm_Laser_On(false);

            //        if (!m_bMessageBox_Showed)
            //        {
            //            m_bMessageBox_Showed = true;
            //            MessageBox.Show("Warning!!!", "Laser Module 또는 Scanner Head 의 냉각수 순환 상태가 아닙니다.\n\n[Laser Emission Stop]");
            //        }
            //    }
            //}
            //else
            //{
            //    m_bMessageBox_Showed = false;
            //}

            //baseLabel_ExitPosPower.Text = String.Format("{0:F3}", waferProbeAlign.m_dPowerMeter_ExitPos_Value);
            //baseLabel_TargetPosPower.Text = String.Format("{0:F3}", waferProbeAlign.m_dPowerMeter_TargetPos_Value);

            ///////////////////////////////////////////////////////////////////////////////////////
            //  Input
            //
            ////  [0 - 12] Chiller Fault
            //if (Get_DI_Status("CHILLER FAULT"))
            //    pictureBoxChillerStatus.Image = global::CWA150SA_Onsemi.Properties.Resources.StopOn;
            //else
            //    pictureBoxChillerStatus.Image = global::CWA150SA_Onsemi.Properties.Resources.StopOff;


            ///////////////////////////////////////////////////////////////////////////////////////
            //  Output
            //
            ////  [1 - 08] Door Unlock
            //if (Get_DO_Status("DOOR UNLOCK"))
            //    pictureBoxDoorUnlock.Image = global::CWA150SA_Onsemi.Properties.Resources.DioRectangleOn;
            //else
            //    pictureBoxDoorUnlock.Image = global::CWA150SA_Onsemi.Properties.Resources.DioRectangleOff;
        }

        private void UpdateStatus()
        {
            int ret = 0;
            int repRate = 20;           //  마곡 LGD 는 Rep-Rate 가 20 KHz 로 고정...
            int power = 0;
            int state = 0;

            //  Blink
            m_nBlink++;
            if ((m_nBlink > 0) && (m_nBlink <= 10))
            {
                m_bBlink = true;
            }
            else if ((m_nBlink > 10) && (m_nBlink <= 20))
            {
                m_bBlink = false;
            }
            else
            {
                m_nBlink = 0;
            }


        }


        #region buttonDownEvent

        private void buttonAxisXUp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonDown(JogControlButtonList.buttonCW, AxisList);
            }
        }

        private void buttonAxisXDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonDown(JogControlButtonList.buttonCCW, AxisList);
            }
        }

        #endregion

        #region buttonUpEvent

        private void buttonAxisXUp_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonUp(JogControlButtonList.buttonCW, AxisList);
            }
        }

        private void buttonAxisXDown_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                JogButtonUp(JogControlButtonList.buttonCCW, AxisList);
            }
        }

        #endregion


        private bool Get_DI_Status(string strInputName)
        {
            bool bRet = false;
            DioPoint dioPoint;

            foreach (DioPoint point in Equipment.GetAllDioPointList())
            {
                dioPoint = point;
                if (dioPoint == null)
                    return false;

                if ((dioPoint.IoType == IoType.Input) && (dioPoint.Name == strInputName))
                {
                    if (dioPoint.GetValue() == DioValue.On)
                        bRet = true;

                    break;
                }
            }

            return bRet;
        }

        private bool Get_DO_Status(string strInputName)
        {
            bool bRet = false;
            DioPoint dioPoint;

            foreach (DioPoint point in Equipment.GetAllDioPointList())
            {
                dioPoint = point;
                if (dioPoint == null)
                    return false;

                if ((dioPoint.IoType == IoType.Output) && (dioPoint.Name == strInputName))
                {
                    if (dioPoint.GetValue() == DioValue.On)
                        bRet = true;

                    break;
                }
            }

            return bRet;
        }

        private bool Set_DO_Status(string strInputName, bool bSet)
        {
            bool bRet = false;
            DioPoint dioPoint;

            foreach (DioPoint point in Equipment.GetAllDioPointList())
            {
                dioPoint = point;

                if (dioPoint == null)
                    return false;

                if ((dioPoint.IoType != IoType.Input) && (dioPoint.Name == strInputName))
                {
                    if (bSet)
                        dioPoint.Write(DioValue.On);
                    else
                        dioPoint.Write(DioValue.Off);

                    break;
                }
            }

            return bRet;
        }

        private void lblWorkStage_Vacuum0_Status_Click(object sender, EventArgs e)
        {
            //  Work Table - 0  Vacuum Sig.

            //if (waferProbeAlign.waferProbeAlignParameter.IsDO_Stage_Vacuum( 0 ))
            //    waferProbeAlign.waferProbeAlignParameter.DO_Stage_Vacuum(0, false);
            //else
            //    waferProbeAlign.waferProbeAlignParameter.DO_Stage_Vacuum(0, true);
        }

        private void lblPowerMeter_Status_Click(object sender, EventArgs e)
        {
            //  Laser Power Meter Shutter

        }

        private void btnHomeAll_Click(object sender, EventArgs e)
        {
            //waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam = waferProbeAlign.waferProbeAlignParameter.GetPositionInformation("Load");


            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "장비를 초기화 하시겠습니까?"))
                return;

            //if (!ACSSPiiPlusMotionBoard.Api.IsConnected)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "ACS 모션 제어기가 연결되지 않았습니다.");
            //    return;
            //}

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            waferProbeAlign.m_bHomeOK = false;

            waferProbeAlign.m_nHomeStep = (int)Home_Step.Start;

            //  Motion 홈 실행 타이머
            waferProbeAlign.timer_Motion_Home.Enabled = true;

            if (!m_FormProgress.HasChildren)            //  Progress 창을 실수로 닫았다면, 다시 메모리 할당하자.
            {
                m_FormProgress = new ProgressForm("Initialize", "장비 초기화 진행중...");
            }

            m_FormProgress.StartPosition = FormStartPosition.CenterScreen;
            m_FormProgress.TopMost = true;
            m_FormProgress.Show();
        }        
    }
}

