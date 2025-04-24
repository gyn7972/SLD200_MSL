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
using System.Windows;
using System.Windows.Forms;
using ACS.SPiiPlusNET;
using Cognex.VisionPro.Implementation.Internal;

//using GCodeNet;
using QMC.Common;
using QMC.Common.Modules;
using QMC.Common.Motion.ACS.Motions;
using QMC.Common.Parts;
using QMC.Common.VisionPart;
using QMC.Core;
using SpiralLab.Sirius;
using static QMC.Common.Modules.Loader;
using static QMC.Common.Modules.Unloader;
using static QMC.Common.Modules.Vision;
using static QMC.Common.Modules.WorkStage;
using MessageBox = System.Windows.Forms.MessageBox;

namespace SLD200_MSL
{
    public partial class FormNew_Config : Form
    {
        FormNew_VisionPopup m_formVisionPopup = new FormNew_VisionPopup();

        static WorkStage workStage;
        static Loader loader;
        static Unloader unloader;
        static Vision vision;
        static Bds bds;

        private bool m_bEmgBtn_Clicked = false;

        //  Motion 이동량 표시를 위한 Zero Pos. 변수
        private double[] m_dMotionSetZeroPos_Loader = new double[(int)LoaderParameter.MotionKey.Max];
        private double[] m_dMotionSetZeroPos_WorkStage = new double[(int)WorkStageParameter.MotionKey.Max];
        private double[] m_dMotionSetZeroPos_Vision = new double[(int)WorkStageParameter.MotionKey.Max];
        private double[] m_dMotionSetZeroPos_Unloader = new double[(int)UnloaderParameter.MotionKey.Max];
        private double[] m_dMotionSetZeroPos_Bds = new double[(int)BdsParameter.MotionKey.Max];

        private FormNew_KeyPad m_keyPad;
        public System.Windows.Forms.Timer timer_Status;

        private Thread m_ConfigStatusThread;
        private bool m_bConfigStatusCycleExit;

        private int[] m_nModuleAddrCount;                               //  모듈 별 IO 카운트용 변수
        private int m_nLaserAddrCount = 0;
        private int m_nBDSAddrCount = 0;

        XyCoordinate xyInterpolatedCoordinate = new XyCoordinate();

        public FormNew_Config()
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

                if (module.Name == "Vision")
                {
                    vision = module as Vision;
                }

                if (module.Name == "BDS")
                {
                    bds = module as Bds;
                }
            }

            loader.Teaching_Position_Load();
            //loader.Move_Properties_Load();
            workStage.Teaching_Position_Load();
            //workStage.Move_Properties_Load();
            vision.Teaching_Position_Load();
            //vision.Move_Properties_Load();
            bds.Teaching_Position_Load();
            //bds.Move_Properties_Load();

            listBox_Config_LDUL_TeachingPositions.SelectedIndex = 0;                                                                            //  LDUL Teaching Position 첫번째 항목 선택
            textBox_Config_LDUL_JogMove_StepSize.Text = Equipment.stAxisParam[(int)Loader.nAxis.TR_X].Jog_StepSize_Coarse.ToString();           //  LDUL Jog Move Step Size 초기화
            listBox_Config_WorkStage_TeachingPositions.SelectedIndex = 0;                                                                       //  WorkStage Teaching Position 첫번째 항목 선택
            textBox_Config_WorkStage_JogMove_StepSize.Text = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_StepSize_Coarse.ToString();      //  Work Stage Jog Move Step Size 초기화
            listBox_Config_Vision_TeachingPositions.SelectedIndex = 0;                                                                       //  WorkStage Teaching Position 첫번째 항목 선택
            textBox_Config_Vision_JogMove_StepSize.Text = Equipment.stAxisParam[(int)Vision.nAxis.Z].Jog_StepSize_Coarse.ToString();      //  Work Stage Jog Move Step Size 초기화
            listBox_Config_BDS_TeachingPositions.SelectedIndex = 0;                                                                       //  WorkStage Teaching Position 첫번째 항목 선택
            textBox_Config_BDS_JogMove_StepSize.Text = Equipment.stAxisParam[(int)Bds.nAxis.MASK_Y].Jog_StepSize_Coarse.ToString();      //  Work Stage Jog Move Step Size 초기화

            //  IO 모듈 개수로 동적 생성해야 하지만, 임시로 고정으로 생성
            m_nModuleAddrCount = new int[4];                //  Input : 0, 2        Output : 1, 3
            for (int i = 0; i < 4; i++)
            {
                m_nModuleAddrCount[i] = 0;
            }

            m_nLaserAddrCount = 0;
            m_nBDSAddrCount = 0;

            MotionMovement_AllReset();

            //  Status 타이머
            timer_Status = new System.Windows.Forms.Timer();
            timer_Status.Interval = 20;
            timer_Status.Tick += new System.EventHandler(Timer_Status_Func);
            timer_Status.Enabled = true;

            //ThreadStart();

            m_bEmgBtn_Clicked = false;

            //checkedListBox_Config_LDUL_DIO_Input.SetItemChecked(0, true);                     //  IO 상태 표시
        }

        #region Thread
        public void ThreadStart()
        {
            //  Status Cycle
            m_bConfigStatusCycleExit = false;
            m_ConfigStatusThread = new Thread(new ThreadStart(OnConfigStatusCycle));
            m_ConfigStatusThread.Start();
        }

        protected void OnConfigStatusCycle()
        {
            while (true)
            {
                if (m_bConfigStatusCycleExit)
                {
                    break;
                }
                if (OnConfigStatusRun() != 0)
                {
                    break;
                }

                Thread.Sleep(10);
            }
        }

        protected int OnConfigStatusRun()
        {
            int ret = 0;

            return ret;
        }

        public void ThreadStop()
        {
            m_bConfigStatusCycleExit = true;

            if (m_ConfigStatusThread != null)
            {
                m_ConfigStatusThread.Join();
            }
        }
        #endregion

        public void MotionMovement_AllReset()
        {
            for (int i = 0; i < (int)LoaderParameter.MotionKey.Max; i++)
            {
                m_dMotionSetZeroPos_Loader[i] = 0.0;
            }

            for (int i = 0; i < (int)WorkStageParameter.MotionKey.Max; i++)
            {
                m_dMotionSetZeroPos_WorkStage[i] = 0.0;
                m_dMotionSetZeroPos_Vision[i] = 0.0;
            }

            for (int i = 0; i < (int)UnloaderParameter.MotionKey.Max; i++)
            {
                m_dMotionSetZeroPos_Unloader[i] = 0.0;
            }

            for (int i = 0; i < (int)BdsParameter.MotionKey.Max; i++)
            {
                m_dMotionSetZeroPos_Bds[i] = 0.0;
            }
        }

        private void Timer_Status_Func(object sender, EventArgs e)
        {
            timer_Status.Enabled = false;

            DIO_Status();
            Motor_Position();
            AIO_Status();

            ///////////////////////////////////////////////////////////////////////////////////////
            //  비상 정지 시
            //
            if (CommonModule.Instance.OperationButtons.IsEMG())
            {
                //  Main Work 타이머
                workStage.timer_MainWork.Enabled = false;

                //  Sub Work 타이머
                workStage.timer_SubWork.Enabled = false;

                //  Product Align 타이머
                workStage.timer_VisionAlign.Enabled = false;

                //  Motion 홈 실행 타이머
                workStage.timer_Motion_Home.Enabled = false;

                //  Reticle Glass check 타이머
                workStage.timer_ReticleGlass_Check.Enabled = false;


                workStage.m_nHomeStep = (int)WorkStage.Home_Step.None;
                workStage.m_nLaserDrilling_MainStep = (int)WorkStage.LaserDrilling_Step.None;
                workStage.m_nFindAlignMark_Step = (int)WorkStage.FindAlignMark_Step.None;
                workStage.m_nReticleCheck_HighResCam_Step = (int)WorkStage.ReticleCheck_HighResCam_Step.None;
                workStage.m_nReticleCheck_LowResCam_Step = (int)WorkStage.ReticleCheck_LowResCam_Step.None;
                workStage.m_nSafetyPos_Move_Step = (int)WorkStage.SafetyPos_Move_Step.None;


                Equipment.MachineStop_byUser = true;


                loader.MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.Z0, 2000);
                loader.MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.Z1, 2000);
                loader.MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.TR_X, 2000);
                loader.MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.TR_Z, 2000);
                loader.MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.ALN_X, 2000);
                loader.MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.ALN_Y, 2000);
                unloader.MC_Func.MC_MotorStop((int)UnloaderParameter.AxisAjinEnum.Z0, 2000);
                unloader.MC_Func.MC_MotorStop((int)UnloaderParameter.AxisAjinEnum.Z1, 2000);
                unloader.MC_Func.MC_MotorStop((int)UnloaderParameter.AxisAjinEnum.TR_X, 2000);
                unloader.MC_Func.MC_MotorStop((int)UnloaderParameter.AxisAjinEnum.TR_Z, 2000);
                workStage.MC_Func.MC_MotorStop((int)WorkStageParameter.AxisAjinEnum.X, 2000);
                workStage.MC_Func.MC_MotorStop((int)WorkStageParameter.AxisAjinEnum.Y, 2000);
                workStage.MC_Func.MC_MotorStop((int)WorkStageParameter.AxisAjinEnum.Z, 2000);

                if (Equipment.Machine_LaserType_CO2)
                {
                    workStage.MC_Func.MC_MotorStop((int)WorkStageParameter.AxisAjinEnum.MASK_Y, 2000);
                }


                workStage.m_bHomeOK = false;


                if (m_bEmgBtn_Clicked == false)
                {
                    m_bEmgBtn_Clicked = true;

                    var mb = new MessageBoxOk();
                    mb.ShowDialog("Information !", "비상정지 버튼이 눌렸습니다!!\r\n\r\n비상정지 해제 후 장비를 초기화 하십시오.");
                }
            }
            else
            {
                m_bEmgBtn_Clicked = false;
            }

            /////////////////////////////////////////////////////////////////////////////////////
            ///

            //  PowerMeter
            label_Config_Laser_PowerMeterValue_BDS.Text = string.Format("{0:0.00000}", workStage.m_dPowerMeterBDS_Value);
            label_Config_Laser_PowerMeterValue_Stage.Text = string.Format("{0:0.00000}", workStage.m_dPowerMeterStage_Value);
            label_Config_WorkStage_PowerMeterValue_Stage.Text = string.Format("{0:0.00000}", workStage.m_dPowerMeterStage_Value);

            //  Laser Height Sensor
            label_Config_WorkStage_LaserHeightSensorValue.Text = string.Format("{0:0.00000}", workStage.m_dLaserHeightSensorSocket_Value);

            /////////////////////////////////////////////////////////////////////////////////////
            ///

            //  Laser Status

            if ((workStage.m_rapidLxLaser_Comm != null) && (workStage.m_rapidLxLaser_Comm.IsOpen))
            {
                //  Host Name
                baseLabel_Config_TabLaser_ConnectedLaserModel.Text = workStage.m_strLaser_HostName;
                if (workStage.m_strLaser_HostName.Length > 0)
                {
                    pictureBox_Config_TabLaser_Connection_to_Laser.Image = global::SLD200.Properties.Resources.DioEllipseOn;
                }
                else
                {
                    pictureBox_Config_TabLaser_Connection_to_Laser.Image = global::SLD200.Properties.Resources.DioEllipseOff;
                }

                //  System Faults
                baseLabel_Config_TabLaser_SystemFaults.Text = workStage.m_strLaser_SystemFaults;
                if (workStage.m_strLaser_SystemFaults == "SYSTEM OK ")
                {
                    pictureBox_Config_TabLaser_SystemFaults.Image = global::SLD200.Properties.Resources.DioEllipseOn;
                }
                else
                {
                    pictureBox_Config_TabLaser_SystemFaults.Image = global::SLD200.Properties.Resources.StopOn;
                }

                //  System Status
                switch (workStage.m_nLaser_SystemStatus)
                {
                    case -1:
                        baseLabel_Config_TabLaser_SystemStatus.Text = "Boot";
                        pictureBox_Config_TabLaser_SystemStatus.Image = global::SLD200.Properties.Resources.StopOff;
                        break;

                    case 0:
                        if (workStage.m_bRapidLxLaser_LaserStart)
                        {
                            baseLabel_Config_TabLaser_SystemStatus.Text = "Starting";
                        }
                        else
                        {
                            baseLabel_Config_TabLaser_SystemStatus.Text = "Ready";
                        }
                        pictureBox_Config_TabLaser_SystemStatus.Image = global::SLD200.Properties.Resources.DioEllipseOff;
                        break;

                    case 1:
                        workStage.m_bRapidLxLaser_LaserStart = false;
                        baseLabel_Config_TabLaser_SystemStatus.Text = "On";
                        pictureBox_Config_TabLaser_SystemStatus.Image = global::SLD200.Properties.Resources.DioEllipseOn;
                        break;

                    case 2:
                        workStage.m_bRapidLxLaser_LaserStart = false;
                        baseLabel_Config_TabLaser_SystemStatus.Text = "Fault";
                        pictureBox_Config_TabLaser_SystemStatus.Image = global::SLD200.Properties.Resources.StopOn;
                        break;

                    default:
                        baseLabel_Config_TabLaser_SystemStatus.Text = "Unknown";
                        pictureBox_Config_TabLaser_SystemStatus.Image = global::SLD200.Properties.Resources.DioRectangleOff;
                        break;
                }

                //  Pulse Mode
                switch (workStage.m_nLaser_PulseMode)
                {
                    case 0:
                        baseLabel_Config_TabLaser_PulseMode.Text = "Internal";
                        break;

                    case 1:
                        baseLabel_Config_TabLaser_PulseMode.Text = "External";
                        break;

                    case 2:
                        baseLabel_Config_TabLaser_PulseMode.Text = "Internal && gated";
                        break;

                    case 3:
                        baseLabel_Config_TabLaser_PulseMode.Text = "External && gated";
                        break;

                    default:
                        baseLabel_Config_TabLaser_PulseMode.Text = "Unknown";
                        break;
                }

                //  Amplifier RR
                baseLabel_Config_TabLaser_AmplifierRR.Text = string.Format("{0}", workStage.m_dLaser_AmplifierRR);

                //  Output RR
                baseLabel_Config_TabLaser_OutputRR.Text = string.Format("{0}", workStage.m_dLaser_OutputRR);

                //  Percent of Energy
                baseLabel_Config_TabLaser_PercentOfEnergy.Text = string.Format("{0}", workStage.m_dLaser_OutputEnergy);

                //  Laser Head Operating Hours
                baseLabel_Config_TabLaser_LaserHeadOperatingHours.Text = string.Format("{0}", workStage.m_dLaser_OperatingHours);

                //  Water Temperature
                baseLabel_Config_TabLaser_WaterTemperature.Text = string.Format("{0}", workStage.m_dLaser_WaterTemperature);

                //  SHG Temperature
                baseLabel_Config_TabLaser_SHGTemperature.Text = string.Format("{0}", workStage.m_dLaser_SHGTemperature);

                //  THG Temperature
                baseLabel_Config_TabLaser_THGTemperature.Text = string.Format("{0}", workStage.m_dLaser_THGTemperature);



                //  Laser Comm 최초 연결 시 세팅된 값을 읽기 위함. (User 세팅 파라미터를 현재 세팅값으로 표시하기 위해서), (0 : Get, 1 : Get Complete, 2 : Set Complete)
                if (workStage.m_nLaserComm_SetValue_Get_Process == 1)
                {
                    workStage.m_nLaserComm_SetValue_Get_Process = 2;

                    comboBox_Config_TabLaser_PulseMode.SelectedIndex = workStage.m_nLaserComm_ReadSetValue_PulseMode_Index;
                    textBox_Config_TabLaser_Amplifier.Text = workStage.m_dLaserComm_ReadSetValue_Amplifier.ToString();
                    textBox_Config_TabLaser_PercentOfEnergy.Text = workStage.m_dLaserComm_ReadSetValue_EnergyPercent.ToString();
                }
            }
            else
            {
                pictureBox_Config_TabLaser_Connection_to_Laser.Image = global::SLD200.Properties.Resources.DioEllipseOff;
                pictureBox_Config_TabLaser_SystemFaults.Image = global::SLD200.Properties.Resources.DioEllipseOff;
                pictureBox_Config_TabLaser_SystemStatus.Image = global::SLD200.Properties.Resources.DioEllipseOff;

                baseLabel_Config_TabLaser_ConnectedLaserModel.Text = "";
                baseLabel_Config_TabLaser_SystemFaults.Text = "";
                baseLabel_Config_TabLaser_SystemStatus.Text = "";
                baseLabel_Config_TabLaser_PulseMode.Text = "";
                baseLabel_Config_TabLaser_AmplifierRR.Text = "";
                baseLabel_Config_TabLaser_OutputRR.Text = "";
                baseLabel_Config_TabLaser_PercentOfEnergy.Text = "";
            }


            /////////////////////////////////////////////////////////////////////////////
            ///

            //  ElectroPneumaticRetulator
            label_Config_TabWorkStage_ElectroPneumaticRegulator_CurrentPressure.Text = workStage.m_dEPRO_Value.ToString("0.0000");
            label_Config_TabWorkStage_ElectroPneumaticRegulator_SetValue.Text = workStage.m_dEPRO_SetValue.ToString("0.0000");


            /////////////////////////////////////////////////////////////////////////////
            ///

            //  Laser Connect Button Caption
            if (workStage.m_rapidLxLaser_Comm != null)
            {
                if (workStage.m_rapidLxLaser_Comm.IsOpen)
                {
                    button_Config_TabLaser_LaserConnect.Text = "Disconnect";
                }
                else
                {
                    button_Config_TabLaser_LaserConnect.Text = "Connect";
                }
            }


            //m_btimer_MainWork_Stop = false;
            //timer_MainWork.Enabled = false;

            //if (!m_btimer_MainWork_Stop)
            //{
            //    timer_MainWork.Enabled = true;
            //}

            timer_Status.Enabled = true;
        }

        private void AIO_Status()
        {
            double dValue = PressureSensor.PressureValue;
            double dScale = workStage.StagePressureSensor.PressureScale;
            dScale = 25;
            //dValue = 5;
            double dPressure = (dValue -0.976) * dScale * -1;
            this.labelStagePressure.Text = dPressure.ToString("0.00");
        }

        private void DIO_Status()
        {
            int m_nLDUL_Input_Count = 0;
            int m_nLDUL_Output_Count = 0;

            DioPoint dioPoint;

            foreach (DioPoint point in Equipment.GetAllDioPointList())
            {
                dioPoint = point;
                if (dioPoint != null)
                {
                    if (dioPoint.ModuleNo == 0)                                                                     //  Input
                    {
                        if (dioPoint.Address == 0)
                        {
                            m_nModuleAddrCount[dioPoint.ModuleNo] = 0;
                            m_nLaserAddrCount = 0;
                            m_nBDSAddrCount = 0;
                        }

                        if (((dioPoint.Address >= 0) && (dioPoint.Address <= 10)) ||                                    //  WorkStage Input (0 ~ 10)
                            ((dioPoint.Address >= 13) && (dioPoint.Address <= 19)) ||                                   //  WorkStage Input (13 ~ 19)
                            ((dioPoint.Address >= 24) && (dioPoint.Address <= 29)))                                     //  WorkStage Input (24 ~ 29)
                        {
                            if (dioPoint.GetValue() == DioValue.On)
                            {
                                checkedListBox_Config_WorkStage_DIO_Input.SetItemChecked(m_nModuleAddrCount[dioPoint.ModuleNo], true);
                            }
                            else
                            {
                                checkedListBox_Config_WorkStage_DIO_Input.SetItemChecked(m_nModuleAddrCount[dioPoint.ModuleNo], false);
                            }

                            m_nModuleAddrCount[dioPoint.ModuleNo]++;
                        }

                        if ((dioPoint.Address == 11) || (dioPoint.Address == 12) ||                                     //  Laser Input (11, 12)
                            (dioPoint.Address == 20) || (dioPoint.Address == 30))                                       //  Laser Input (20, 30)
                        {
                            if (dioPoint.GetValue() == DioValue.On)
                            {
                                checkedListBox_Config_Laser_DIO_Input.SetItemChecked(m_nLaserAddrCount, true);
                            }
                            else
                            {
                                checkedListBox_Config_Laser_DIO_Input.SetItemChecked(m_nLaserAddrCount, false);
                            }

                            m_nLaserAddrCount++;
                        }

                        if ((dioPoint.Address >= 21) && (dioPoint.Address <= 23))                                       //  BDS Input (21 ~ 23)
                        {
                            if (dioPoint.GetValue() == DioValue.On)
                            {
                                checkedListBox_Config_BDS_DIO_Input.SetItemChecked(m_nBDSAddrCount, true);
                            }
                            else
                            {
                                checkedListBox_Config_BDS_DIO_Input.SetItemChecked(m_nBDSAddrCount, false);
                            }

                            m_nBDSAddrCount++;
                        }
                    }                                   

                    if (dioPoint.ModuleNo == 1)                                                                     //  Output
                    {
                        if (dioPoint.Address == 0)
                        {
                            m_nModuleAddrCount[dioPoint.ModuleNo] = 0;
                            m_nLaserAddrCount = 0;
                            m_nBDSAddrCount = 0;
                        }

                        if (((dioPoint.Address >= 0) && (dioPoint.Address <= 6)) ||                                     //  WorkStage Output (0 ~ 6)
                            ((dioPoint.Address >= 21) && (dioPoint.Address <= 24)) ||                                   //  WorkStage Output (21 ~ 24)
                            ((dioPoint.Address >= 26) && (dioPoint.Address <= 27)))                                     //  WorkStage Output (26 ~ 27)
                        {
                            if (dioPoint.GetValue() == DioValue.On)
                            {
                                checkedListBox_Config_WorkStage_DIO_Output.SetItemChecked(m_nModuleAddrCount[dioPoint.ModuleNo], true);
                            }
                            else
                            {
                                checkedListBox_Config_WorkStage_DIO_Output.SetItemChecked(m_nModuleAddrCount[dioPoint.ModuleNo], false);
                            }

                            m_nModuleAddrCount[dioPoint.ModuleNo]++;
                        }

                        if ((dioPoint.Address == 7) || (dioPoint.Address == 8) ||                                       //  Laser Output (7, 8)
                            ((dioPoint.Address >= 11) && (dioPoint.Address <= 14)) ||                                   //  Laser Output (11 ~ 14)
                            ((dioPoint.Address >= 18) && (dioPoint.Address <= 20)) ||                                   //  Laser Output (18 ~ 20)
                            (dioPoint.Address == 25) ||                                                                 //  Laser Output (25)
                            (dioPoint.Address == 28))                                                                   //  Laser Output (28)
                        {
                            if (dioPoint.GetValue() == DioValue.On)
                            {
                                checkedListBox_Config_Laser_DIO_Output.SetItemChecked(m_nLaserAddrCount, true);
                            }
                            else
                            {
                                checkedListBox_Config_Laser_DIO_Output.SetItemChecked(m_nLaserAddrCount, false);
                            }

                            m_nLaserAddrCount++;
                        }

                        if ((dioPoint.Address == 9) || (dioPoint.Address == 10) ||                                      //  BDS Output (9, 10)
                            ((dioPoint.Address >= 15) && (dioPoint.Address <= 17)))                                     //  BDS Output (15 ~ 17)
                        {
                            if (dioPoint.GetValue() == DioValue.On)
                            {
                                checkedListBox_Config_BDS_DIO_Output.SetItemChecked(m_nBDSAddrCount, true);
                            }
                            else
                            {
                                checkedListBox_Config_BDS_DIO_Output.SetItemChecked(m_nBDSAddrCount, false);
                            }

                            m_nBDSAddrCount++;
                        }
                    }

                    if (dioPoint.ModuleNo == 2)                                                                     //  Loader
                    {
                        if (dioPoint.Address == 0)
                        {
                            m_nModuleAddrCount[dioPoint.ModuleNo] = 0;
                        }

                        if (dioPoint.IoType == IoType.Input)                                                            //  Input
                        {
                            if ((dioPoint.Address >= 0) && (dioPoint.Address <= 12))                                        //  Loader Input (0 ~ 10) --> 2개 추가하여 12까지
                            {
                                if (dioPoint.GetValue() == DioValue.On)
                                {
                                    //checkedListBox_Config_LDUL_DIO_Input.SetItemChecked(m_nModuleAddrCount[dioPoint.ModuleNo], true);
                                    checkedListBox_Config_LDUL_DIO_Input.SetItemChecked(m_nLDUL_Input_Count, true);
                                }
                                else
                                {
                                    //checkedListBox_Config_LDUL_DIO_Input.SetItemChecked(m_nModuleAddrCount[dioPoint.ModuleNo], false);
                                    checkedListBox_Config_LDUL_DIO_Input.SetItemChecked(m_nLDUL_Input_Count, false);
                                }

                                //m_nModuleAddrCount[dioPoint.ModuleNo]++;
                                m_nLDUL_Input_Count++;
                            }
                        }
                        else                                                                                            //  Output
                        {
                            if ((dioPoint.Address >= 0) && (dioPoint.Address <= 9))                                         //  Loader Output (0 ~ 7) --> 2개 추가하여 9까지
                            {
                                if (dioPoint.GetValue() == DioValue.On)
                                {
                                    //checkedListBox_Config_LDUL_DIO_Output.SetItemChecked(m_nModuleAddrCount[dioPoint.ModuleNo], true);
                                    checkedListBox_Config_LDUL_DIO_Output.SetItemChecked(m_nLDUL_Output_Count, true);
                                }
                                else
                                {
                                    //checkedListBox_Config_LDUL_DIO_Output.SetItemChecked(m_nModuleAddrCount[dioPoint.ModuleNo], false);
                                    checkedListBox_Config_LDUL_DIO_Output.SetItemChecked(m_nLDUL_Output_Count, false);
                                }

                                //m_nModuleAddrCount[dioPoint.ModuleNo]++;
                                m_nLDUL_Output_Count++;
                            }
                        }                        
                    }

                    if (dioPoint.ModuleNo == 3)                                                                     //  Unloader
                    {
                        if (dioPoint.Address == 0)
                        {
                            m_nModuleAddrCount[dioPoint.ModuleNo] = 0;
                        }

                        if (dioPoint.IoType == IoType.Input)                                                            //  Input
                        {
                            if ((dioPoint.Address >= 0) && (dioPoint.Address <= 8))                                         //  Unloader Input (0 ~ 8)
                            {
                                if (dioPoint.GetValue() == DioValue.On)
                                {
                                    //checkedListBox_Config_LDUL_DIO_Input.SetItemChecked(m_nModuleAddrCount[dioPoint.ModuleNo], true);
                                    checkedListBox_Config_LDUL_DIO_Input.SetItemChecked(m_nLDUL_Input_Count, true);
                                }
                                else
                                {
                                    //checkedListBox_Config_LDUL_DIO_Input.SetItemChecked(m_nModuleAddrCount[dioPoint.ModuleNo], false);
                                    checkedListBox_Config_LDUL_DIO_Input.SetItemChecked(m_nLDUL_Input_Count, false);
                                }

                                //m_nModuleAddrCount[dioPoint.ModuleNo]++;
                                m_nLDUL_Input_Count++;
                            }
                        }
                        else                                                                                            //  Output
                        {
                            if ((dioPoint.Address >= 0) && (dioPoint.Address <= 2))                                         //  Unloader Output (0 ~ 2)
                            {
                                if (dioPoint.GetValue() == DioValue.On)
                                {
                                    //checkedListBox_Config_LDUL_DIO_Output.SetItemChecked(m_nModuleAddrCount[dioPoint.ModuleNo], true);
                                    checkedListBox_Config_LDUL_DIO_Output.SetItemChecked(m_nLDUL_Output_Count, true);
                                }
                                else
                                {
                                    //checkedListBox_Config_LDUL_DIO_Output.SetItemChecked(m_nModuleAddrCount[dioPoint.ModuleNo], false);
                                    checkedListBox_Config_LDUL_DIO_Output.SetItemChecked(m_nLDUL_Output_Count, false);
                                }

                                //m_nModuleAddrCount[dioPoint.ModuleNo]++;
                                m_nLDUL_Output_Count++;
                            }
                        }
                    }

                    //if (dioPoint.ModuleNo == 2)                                                                     //  Loader
                    //{
                    //    if (dioPoint.Address == 0)
                    //    {
                    //        m_nModuleAddrCount[dioPoint.ModuleNo] = 0;
                    //    }

                    //    if (((dioPoint.Address >= 0) && (dioPoint.Address <= 10)) ||                                    //  Loader Input (0 ~ 10)
                    //        ((dioPoint.Address >= 16) && (dioPoint.Address <= 24)))                                     //  Unloader Input (16 ~ 24)
                    //    {
                    //        if (dioPoint.GetValue() == DioValue.On)
                    //        {
                    //            checkedListBox_Config_LDUL_DIO_Input.SetItemChecked(m_nModuleAddrCount[dioPoint.ModuleNo], true);
                    //        }
                    //        else
                    //        {
                    //            checkedListBox_Config_LDUL_DIO_Input.SetItemChecked(m_nModuleAddrCount[dioPoint.ModuleNo], false);
                    //        }

                    //        m_nModuleAddrCount[dioPoint.ModuleNo]++;
                    //    }
                    //}

                    //if (dioPoint.ModuleNo == 3)                                                                     //  Unloader
                    //{
                    //    if (dioPoint.Address == 0)
                    //    {
                    //        m_nModuleAddrCount[dioPoint.ModuleNo] = 0;
                    //    }

                    //    if (((dioPoint.Address >= 0) && (dioPoint.Address <= 7)) ||                                     //  Loader Output (0 ~ 7)
                    //        ((dioPoint.Address >= 16) && (dioPoint.Address <= 18)))                                     //  Unloader Output (16 ~ 18)
                    //    {
                    //        if (dioPoint.GetValue() == DioValue.On)
                    //        {
                    //            checkedListBox_Config_LDUL_DIO_Output.SetItemChecked(m_nModuleAddrCount[dioPoint.ModuleNo], true);
                    //        }
                    //        else
                    //        {
                    //            checkedListBox_Config_LDUL_DIO_Output.SetItemChecked(m_nModuleAddrCount[dioPoint.ModuleNo], false);
                    //        }

                    //        m_nModuleAddrCount[dioPoint.ModuleNo]++;
                    //    }
                    //}
                }
            }
        }


        private void Motor_Position()
        {
            //  Loader Stacker Position
            label_Config_EncPosition_LD_Z0.Text = string.Format("{0:F3}", loader.MC_Func.MC_GetEncPos((int)Loader.nAxis.Z0));
            label_Config_EncPosition_LD_Z1.Text = string.Format("{0:F3}", loader.MC_Func.MC_GetEncPos((int)Loader.nAxis.Z1));

            //  Loader Transfer Position
            label_Config_EncPosition_LD_TRX.Text = string.Format("{0:F3}", loader.MC_Func.MC_GetEncPos((int)Loader.nAxis.TR_X));
            label_Config_EncPosition_LD_TRZ.Text = string.Format("{0:F3}", loader.MC_Func.MC_GetEncPos((int)Loader.nAxis.TR_Z));

            //  Loader Mechanic Aligner Position
            label_Config_EncPosition_LD_ALNX.Text = string.Format("{0:F3}", loader.MC_Func.MC_GetEncPos((int)Loader.nAxis.ALN_X));
            label_Config_EncPosition_LD_ALNY.Text = string.Format("{0:F3}", loader.MC_Func.MC_GetEncPos((int)Loader.nAxis.ALN_Y));


            //  Work Stage Position
            label_Config_EncPosition_STAGE_X.Text = string.Format("{0:F3}", workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X));
            label_Config_EncPosition_STAGE_Y.Text = string.Format("{0:F3}", workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y));
            string type = workStage.MC_Func.GetType().ToString();

            //  Scanner & Camera Position
            label_Config_EncPosition_SCANNER_Z.Text = string.Format("{0:F3}", workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Z));

            if (Equipment.Machine_LaserType_CO2)
            {
                //  Mask Position
                label_Config_EncPosition_MASK_Y.Text = string.Format("{0:F3}", workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.MASK_Y));
            }
            else
            {
                label_Config_EncPosition_MASK_Y.Text = "0.000";
            }

            //  Unloader Stacker Position
            label_Config_EncPosition_UL_Z0.Text = string.Format("{0:F3}", unloader.MC_Func.MC_GetEncPos((int)Unloader.nAxis.Z0));
            label_Config_EncPosition_UL_Z1.Text = string.Format("{0:F3}", unloader.MC_Func.MC_GetEncPos((int)Unloader.nAxis.Z1));

            //  Unloader Transfer Position
            label_Config_EncPosition_UL_TRX.Text = string.Format("{0:F3}", unloader.MC_Func.MC_GetEncPos((int)Unloader.nAxis.TR_X));
            label_Config_EncPosition_UL_TRZ.Text = string.Format("{0:F3}", unloader.MC_Func.MC_GetEncPos((int)Unloader.nAxis.TR_Z));


            //  Motion Movement 표시
            if (Equipment.AjinBoard_Opened)
            {
                //  Loader & Unloader
                if (radioButton_Config_ActiveUnit_Loader.Checked)               //  Loader 활성화
                {
                    textBox_Config_LDUL_Movement_TransferX.Text = string.Format("{0:F3}", loader.MC_Func.MC_GetEncPos((int)Loader.nAxis.TR_X) - m_dMotionSetZeroPos_Loader[(int)LoaderParameter.MotionKey.TR_X]);
                    textBox_Config_LDUL_Movement_TransferZ.Text = string.Format("{0:F3}", loader.MC_Func.MC_GetEncPos((int)Loader.nAxis.TR_Z) - m_dMotionSetZeroPos_Loader[(int)LoaderParameter.MotionKey.TR_Z]);
                    textBox_Config_LDUL_Movement_StackerZ0.Text = string.Format("{0:F3}", loader.MC_Func.MC_GetEncPos((int)Loader.nAxis.Z0) - m_dMotionSetZeroPos_Loader[(int)LoaderParameter.MotionKey.Z0]);
                    textBox_Config_LDUL_Movement_StackerZ1.Text = string.Format("{0:F3}", loader.MC_Func.MC_GetEncPos((int)Loader.nAxis.Z1) - m_dMotionSetZeroPos_Loader[(int)LoaderParameter.MotionKey.Z1]);
                    textBox_Config_LDUL_Movement_AlignX.Text = string.Format("{0:F3}", loader.MC_Func.MC_GetEncPos((int)Loader.nAxis.ALN_X) - m_dMotionSetZeroPos_Loader[(int)LoaderParameter.MotionKey.ALN_X]);
                    textBox_Config_LDUL_Movement_AlignY.Text = string.Format("{0:F3}", loader.MC_Func.MC_GetEncPos((int)Loader.nAxis.ALN_Y) - m_dMotionSetZeroPos_Loader[(int)LoaderParameter.MotionKey.ALN_Y]);
                }
                else if (radioButton_Config_ActiveUnit_Unloader.Checked)        //  Unloader 활성화
                {
                    textBox_Config_LDUL_Movement_TransferX.Text = string.Format("{0:F3}", unloader.MC_Func.MC_GetEncPos((int)Unloader.nAxis.TR_X) - m_dMotionSetZeroPos_Unloader[(int)UnloaderParameter.MotionKey.TR_X]);
                    textBox_Config_LDUL_Movement_TransferZ.Text = string.Format("{0:F3}", unloader.MC_Func.MC_GetEncPos((int)Unloader.nAxis.TR_Z) - m_dMotionSetZeroPos_Unloader[(int)UnloaderParameter.MotionKey.TR_Z]);
                    textBox_Config_LDUL_Movement_StackerZ0.Text = string.Format("{0:F3}", unloader.MC_Func.MC_GetEncPos((int)Unloader.nAxis.Z0) - m_dMotionSetZeroPos_Unloader[(int)UnloaderParameter.MotionKey.Z0]);
                    textBox_Config_LDUL_Movement_StackerZ1.Text = string.Format("{0:F3}", unloader.MC_Func.MC_GetEncPos((int)Unloader.nAxis.Z1) - m_dMotionSetZeroPos_Unloader[(int)UnloaderParameter.MotionKey.Z1]);
                }

                //  Work Stage
                textBox_Config_WorkStage_Movement_StageX.Text = string.Format("{0:F3}", workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X) - m_dMotionSetZeroPos_WorkStage[(int)WorkStageParameter.MotionKey.X]);
                textBox_Config_WorkStage_Movement_StageY.Text = string.Format("{0:F3}", workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y) - m_dMotionSetZeroPos_WorkStage[(int)WorkStageParameter.MotionKey.Y]);
                textBox_Config_WorkStage_Movement_ScannerZ.Text = string.Format("{0:F3}", workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Z) - m_dMotionSetZeroPos_WorkStage[(int)WorkStageParameter.MotionKey.Z]);

                //  Vision
                textBox_Config_Vision_Movement_StageX.Text = textBox_Config_WorkStage_Movement_StageX.Text;
                textBox_Config_Vision_Movement_StageY.Text = textBox_Config_WorkStage_Movement_StageY.Text;
                textBox_Config_Vision_Movement_VisionZ.Text = textBox_Config_WorkStage_Movement_ScannerZ.Text;

                if (Equipment.Machine_LaserType_CO2)
                {
                    //  BDS
                    textBox_Config_BDS_Movement_MaskY.Text = string.Format("{0:F3}", workStage.MC_Func.MC_GetEncPos((int)Bds.nAxis.MASK_Y) - m_dMotionSetZeroPos_Bds[(int)BdsParameter.MotionKey.MASK_Y]);
                }
                else
                {
                    //  BDS
                    textBox_Config_BDS_Movement_MaskY.Text = string.Format("{0:F3}", 0.0);
                }
            }


            //for (int i = 0; i < (int)LoaderParameter.MotionKey.Max; i++)
            //{
            //    m_dMotionSetZeroPos_Loader[i] = 0.0;
            //}

            //for (int i = 0; i < (int)WorkStageParameter.MotionKey.Max; i++)
            //{
            //    m_dMotionSetZeroPos_WorkStage[i] = 0.0;
            //    m_dMotionSetZeroPos_Vision[i] = 0.0;
            //}

            //for (int i = 0; i < (int)UnloaderParameter.MotionKey.Max; i++)
            //{
            //    m_dMotionSetZeroPos_Unloader[i] = 0.0;
            //}

            //for (int i = 0; i < (int)BdsParameter.MotionKey.Max; i++)
            //{
            //    m_dMotionSetZeroPos_Bds[i] = 0.0;
            //}


            //  Limit
            if (Equipment.AjinBoard_Opened)
            {
                //  Loader & Unloader
                if (radioButton_Config_ActiveUnit_Loader.Checked)               //  Loader 활성화
                {
                    if (loader.MC_Func.MC_isLimit_Neg((int)Loader.nAxis.Z0))
                    {
                        button_Config_LDUL_Z0_Neg.BackColor = Color.Red;   
                        button_Config_LDUL_Z0_Neg.ForeColor = Color.White;
                    }
                    else
                    {
                        button_Config_LDUL_Z0_Neg.BackColor = Color.White; 
                        button_Config_LDUL_Z0_Neg.ForeColor = Color.Black;
                    }

                    if (loader.MC_Func.MC_isLimit_Pos((int)Loader.nAxis.Z0))
                    {
                        button_Config_LDUL_Z0_Pos.BackColor = Color.Red;   
                        button_Config_LDUL_Z0_Pos.ForeColor = Color.White;
                    }
                    else
                    {
                        button_Config_LDUL_Z0_Pos.BackColor = Color.White;
                        button_Config_LDUL_Z0_Pos.ForeColor = Color.Black;
                    }

                    if (loader.MC_Func.MC_isLimit_Neg((int)Loader.nAxis.Z1))
                    {
                        button_Config_LDUL_Z1_Neg.BackColor = Color.Red;
                        button_Config_LDUL_Z1_Neg.ForeColor = Color.White;
                    }
                    else
                    {
                        button_Config_LDUL_Z1_Neg.BackColor = Color.White;
                        button_Config_LDUL_Z1_Neg.ForeColor = Color.Black;
                    }

                    if (loader.MC_Func.MC_isLimit_Pos((int)Loader.nAxis.Z1))
                    {
                        button_Config_LDUL_Z1_Pos.BackColor = Color.Red;
                        button_Config_LDUL_Z1_Pos.ForeColor = Color.White;
                    }
                    else
                    {
                        button_Config_LDUL_Z1_Pos.BackColor = Color.White;
                        button_Config_LDUL_Z1_Pos.ForeColor = Color.Black;
                    }

                    if (loader.MC_Func.MC_isLimit_Neg((int)Loader.nAxis.TR_X))
                    {
                        button_Config_LDUL_TRX_Neg.BackColor = Color.Red;
                        button_Config_LDUL_TRX_Neg.ForeColor = Color.White;
                    }
                    else
                    {
                        button_Config_LDUL_TRX_Neg.BackColor = Color.White;
                        button_Config_LDUL_TRX_Neg.ForeColor = Color.Black;
                    }

                    if (loader.MC_Func.MC_isLimit_Pos((int)Loader.nAxis.TR_X))
                    {
                        button_Config_LDUL_TRX_Pos.BackColor = Color.Red;
                        button_Config_LDUL_TRX_Pos.ForeColor = Color.White;
                    }
                    else
                    {
                        button_Config_LDUL_TRX_Pos.BackColor = Color.White;
                        button_Config_LDUL_TRX_Pos.ForeColor = Color.Black;
                    }

                    if (loader.MC_Func.MC_isLimit_Neg((int)Loader.nAxis.TR_Z))
                    {
                        button_Config_LDUL_TRZ_Neg.BackColor = Color.Red;
                        button_Config_LDUL_TRZ_Neg.ForeColor = Color.White;
                    }
                    else
                    {
                        button_Config_LDUL_TRZ_Neg.BackColor = Color.White;
                        button_Config_LDUL_TRZ_Neg.ForeColor = Color.Black;
                    }

                    if (loader.MC_Func.MC_isLimit_Pos((int)Loader.nAxis.TR_Z))
                    {
                        button_Config_LDUL_TRZ_Pos.BackColor = Color.Red;
                        button_Config_LDUL_TRZ_Pos.ForeColor = Color.White;
                    }
                    else
                    {
                        button_Config_LDUL_TRZ_Pos.BackColor = Color.White;
                        button_Config_LDUL_TRZ_Pos.ForeColor = Color.Black;
                    }

                    if (loader.MC_Func.MC_isLimit_Neg((int)Loader.nAxis.ALN_X))
                    {
                        button_Config_LD_ALNX_Neg.BackColor = Color.Red;
                        button_Config_LD_ALNX_Neg.ForeColor = Color.White;
                    }
                    else
                    {
                        button_Config_LD_ALNX_Neg.BackColor = Color.White;
                        button_Config_LD_ALNX_Neg.ForeColor = Color.Black;
                    }

                    if (loader.MC_Func.MC_isLimit_Pos((int)Loader.nAxis.ALN_X))
                    {
                        button_Config_LD_ALNX_Pos.BackColor = Color.Red;
                        button_Config_LD_ALNX_Pos.ForeColor = Color.White;
                    }
                    else
                    {
                        button_Config_LD_ALNX_Pos.BackColor = Color.White;
                        button_Config_LD_ALNX_Pos.ForeColor = Color.Black;
                    }

                    if (loader.MC_Func.MC_isLimit_Neg((int)Loader.nAxis.ALN_Y))
                    {
                        button_Config_LD_ALNY_Neg.BackColor = Color.Red;
                        button_Config_LD_ALNY_Neg.ForeColor = Color.White;
                    }
                    else
                    {
                        button_Config_LD_ALNY_Neg.BackColor = Color.White;
                        button_Config_LD_ALNY_Neg.ForeColor = Color.Black;
                    }

                    if (loader.MC_Func.MC_isLimit_Pos((int)Loader.nAxis.ALN_Y))
                    {
                        button_Config_LD_ALNY_Pos.BackColor = Color.Red;
                        button_Config_LD_ALNY_Pos.ForeColor = Color.White;
                    }
                    else
                    {
                        button_Config_LD_ALNY_Pos.BackColor = Color.White;
                        button_Config_LD_ALNY_Pos.ForeColor = Color.Black;
                    }
                }
                else if (radioButton_Config_ActiveUnit_Unloader.Checked)        //  Unloader 활성화                    
                {
                    if (unloader.MC_Func.MC_isLimit_Neg((int)Unloader.nAxis.Z0))
                    {
                        button_Config_LDUL_Z0_Neg.BackColor = Color.Red;
                        button_Config_LDUL_Z0_Neg.ForeColor = Color.White;
                    }
                    else
                    {
                        button_Config_LDUL_Z0_Neg.BackColor = Color.White;
                        button_Config_LDUL_Z0_Neg.ForeColor = Color.Black;
                    }

                    if (unloader.MC_Func.MC_isLimit_Pos((int)Unloader.nAxis.Z0))
                    {
                        button_Config_LDUL_Z0_Pos.BackColor = Color.Red;
                        button_Config_LDUL_Z0_Pos.ForeColor = Color.White;
                    }
                    else
                    {
                        button_Config_LDUL_Z0_Pos.BackColor = Color.White;
                        button_Config_LDUL_Z0_Pos.ForeColor = Color.Black;
                    }

                    if (unloader.MC_Func.MC_isLimit_Neg((int)Unloader.nAxis.Z1))
                    {
                        button_Config_LDUL_Z1_Neg.BackColor = Color.Red;
                        button_Config_LDUL_Z1_Neg.ForeColor = Color.White;
                    }
                    else
                    {
                        button_Config_LDUL_Z1_Neg.BackColor = Color.White;
                        button_Config_LDUL_Z1_Neg.ForeColor = Color.Black;
                    }

                    if (unloader.MC_Func.MC_isLimit_Pos((int)Unloader.nAxis.Z1))
                    {
                        button_Config_LDUL_Z1_Pos.BackColor = Color.Red;
                        button_Config_LDUL_Z1_Pos.ForeColor = Color.White;
                    }
                    else
                    {
                        button_Config_LDUL_Z1_Pos.BackColor = Color.White;
                        button_Config_LDUL_Z1_Pos.ForeColor = Color.Black;
                    }

                    if (unloader.MC_Func.MC_isLimit_Neg((int)Unloader.nAxis.TR_X))
                    {
                        button_Config_LDUL_TRX_Neg.BackColor = Color.Red;
                        button_Config_LDUL_TRX_Neg.ForeColor = Color.White;
                    }
                    else
                    {
                        button_Config_LDUL_TRX_Neg.BackColor = Color.White;
                        button_Config_LDUL_TRX_Neg.ForeColor = Color.Black;
                    }

                    if (unloader.MC_Func.MC_isLimit_Pos((int)Unloader.nAxis.TR_X))
                    {
                        button_Config_LDUL_TRX_Pos.BackColor = Color.Red;
                        button_Config_LDUL_TRX_Pos.ForeColor = Color.White;
                    }
                    else
                    {
                        button_Config_LDUL_TRX_Pos.BackColor = Color.White;
                        button_Config_LDUL_TRX_Pos.ForeColor = Color.Black;
                    }

                    if (unloader.MC_Func.MC_isLimit_Neg((int)Unloader.nAxis.TR_Z))
                    {
                        button_Config_LDUL_TRZ_Neg.BackColor = Color.Red;
                        button_Config_LDUL_TRZ_Neg.ForeColor = Color.White;
                    }
                    else
                    {
                        button_Config_LDUL_TRZ_Neg.BackColor = Color.White;
                        button_Config_LDUL_TRZ_Neg.ForeColor = Color.Black;
                    }

                    if (unloader.MC_Func.MC_isLimit_Pos((int)Unloader.nAxis.TR_Z))
                    {
                        button_Config_LDUL_TRZ_Pos.BackColor = Color.Red;
                        button_Config_LDUL_TRZ_Pos.ForeColor = Color.White;
                    }
                    else
                    {
                        button_Config_LDUL_TRZ_Pos.BackColor = Color.White;
                        button_Config_LDUL_TRZ_Pos.ForeColor = Color.Black;
                    }
                }


                //  Work Stage
                if (workStage.MC_Func.MC_isLimit_Neg((int)WorkStage.nAxis.X))
                {
                    button_Config_WorkStage_X_Neg.BackColor = Color.Red;
                    button_Config_WorkStage_X_Neg.ForeColor = Color.White;
                }
                else
                {
                    button_Config_WorkStage_X_Neg.BackColor = Color.White;
                    button_Config_WorkStage_X_Neg.ForeColor = Color.Black;
                }

                if (workStage.MC_Func.MC_isLimit_Pos((int)WorkStage.nAxis.X))
                {
                    button_Config_WorkStage_X_Pos.BackColor = Color.Red;
                    button_Config_WorkStage_X_Pos.ForeColor = Color.White;
                }
                else
                {
                    button_Config_WorkStage_X_Pos.BackColor = Color.White;
                    button_Config_WorkStage_X_Pos.ForeColor = Color.Black;
                }

                if (workStage.MC_Func.MC_isLimit_Neg((int)WorkStage.nAxis.Y))
                {
                    button_Config_WorkStage_Y_Neg.BackColor = Color.Red;
                    button_Config_WorkStage_Y_Neg.ForeColor = Color.White;
                }
                else
                {
                    button_Config_WorkStage_Y_Neg.BackColor = Color.White;
                    button_Config_WorkStage_Y_Neg.ForeColor = Color.Black;
                }

                if (workStage.MC_Func.MC_isLimit_Pos((int)WorkStage.nAxis.Y))
                {
                    button_Config_WorkStage_Y_Pos.BackColor = Color.Red;
                    button_Config_WorkStage_Y_Pos.ForeColor = Color.White;
                }
                else
                {
                    button_Config_WorkStage_Y_Pos.BackColor = Color.White;
                    button_Config_WorkStage_Y_Pos.ForeColor = Color.Black;
                }

                if (workStage.MC_Func.MC_isLimit_Neg((int)WorkStage.nAxis.Z))
                {
                    button_Config_WorkStage_Z_Neg.BackColor = Color.Red;
                    button_Config_WorkStage_Z_Neg.ForeColor = Color.White;
                }
                else
                {
                    button_Config_WorkStage_Z_Neg.BackColor = Color.White;
                    button_Config_WorkStage_Z_Neg.ForeColor = Color.Black;
                }

                if (workStage.MC_Func.MC_isLimit_Pos((int)WorkStage.nAxis.Z))
                {
                    button_Config_WorkStage_Z_Pos.BackColor = Color.Red;
                    button_Config_WorkStage_Z_Pos.ForeColor = Color.White;
                }
                else
                {
                    button_Config_WorkStage_Z_Pos.BackColor = Color.White;
                    button_Config_WorkStage_Z_Pos.ForeColor = Color.Black;
                }


                //  Vision
                if (vision.MC_Func.MC_isLimit_Neg((int)Vision.nAxis.X))
                {
                    button_Config_Vision_X_Neg.BackColor = Color.Red;
                    button_Config_Vision_X_Neg.ForeColor = Color.White;
                }
                else
                {
                    button_Config_Vision_X_Neg.BackColor = Color.White;
                    button_Config_Vision_X_Neg.ForeColor = Color.Black;
                }

                if (vision.MC_Func.MC_isLimit_Pos((int)Vision.nAxis.X))
                {
                    button_Config_Vision_X_Pos.BackColor = Color.Red;
                    button_Config_Vision_X_Pos.ForeColor = Color.White;
                }
                else
                {
                    button_Config_Vision_X_Pos.BackColor = Color.White;
                    button_Config_Vision_X_Pos.ForeColor = Color.Black;
                }

                if (vision.MC_Func.MC_isLimit_Neg((int)Vision.nAxis.Y))
                {
                    button_Config_Vision_Y_Neg.BackColor = Color.Red;
                    button_Config_Vision_Y_Neg.ForeColor = Color.White;
                }
                else
                {
                    button_Config_Vision_Y_Neg.BackColor = Color.White;
                    button_Config_Vision_Y_Neg.ForeColor = Color.Black;
                }

                if (vision.MC_Func.MC_isLimit_Pos((int)Vision.nAxis.Y))
                {
                    button_Config_Vision_Y_Pos.BackColor = Color.Red;
                    button_Config_Vision_Y_Pos.ForeColor = Color.White;
                }
                else
                {
                    button_Config_Vision_Y_Pos.BackColor = Color.White;
                    button_Config_Vision_Y_Pos.ForeColor = Color.Black;
                }

                if (vision.MC_Func.MC_isLimit_Neg((int)Vision.nAxis.Z))
                {
                    button_Config_Vision_Z_Neg.BackColor = Color.Red;
                    button_Config_Vision_Z_Neg.ForeColor = Color.White;
                }
                else
                {
                    button_Config_Vision_Z_Neg.BackColor = Color.White;
                    button_Config_Vision_Z_Neg.ForeColor = Color.Black;
                }

                if (vision.MC_Func.MC_isLimit_Pos((int)Vision.nAxis.Z))
                {
                    button_Config_Vision_Z_Pos.BackColor = Color.Red;
                    button_Config_Vision_Z_Pos.ForeColor = Color.White;
                }
                else
                {
                    button_Config_Vision_Z_Pos.BackColor = Color.White;
                    button_Config_Vision_Z_Pos.ForeColor = Color.Black;
                }


                if (Equipment.Machine_LaserType_CO2)
                {
                    //  BDS
                    if (bds.MC_Func.MC_isLimit_Neg((int)Bds.nAxis.MASK_Y))
                    {
                        button_Config_BDS_Y_Neg.BackColor = Color.Red;
                        button_Config_BDS_Y_Neg.ForeColor = Color.White;
                    }
                    else
                    {
                        button_Config_BDS_Y_Neg.BackColor = Color.White;
                        button_Config_BDS_Y_Neg.ForeColor = Color.Black;
                    }

                    if (bds.MC_Func.MC_isLimit_Pos((int)Bds.nAxis.MASK_Y))
                    {
                        button_Config_BDS_Y_Pos.BackColor = Color.Red;
                        button_Config_BDS_Y_Pos.ForeColor = Color.White;
                    }
                    else
                    {
                        button_Config_BDS_Y_Pos.BackColor = Color.White;
                        button_Config_BDS_Y_Pos.ForeColor = Color.Black;
                    }
                }
                else
                {
                    //  BDS
                    button_Config_BDS_Y_Neg.BackColor = Color.Gray;
                    button_Config_BDS_Y_Neg.ForeColor = Color.Black;
                    button_Config_BDS_Y_Pos.BackColor = Color.Gray;
                    button_Config_BDS_Y_Pos.ForeColor = Color.Black;
                }
            }
        }


        private void button_KeypadCall_Config_LDUL_TeachingPos_TransferX_Click(object sender, EventArgs e)
        {
            m_keyPad.StartPosition = FormStartPosition.CenterScreen;

            //  현재 값 전달
            m_keyPad.label_NumPad.Text = textBox_Config_LDUL_TeachingPos_TransferX.Text;

            if (m_keyPad.ShowDialog() == DialogResult.OK)
            {

            }
            else if (m_keyPad.DialogResult == DialogResult.Cancel)
            {
                
            }
        }

        private void checkedListBox_Config_LDUL_DIO_Output_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //  LDUL Output IO 상태 변경

            int m_nIndex = checkedListBox_Config_LDUL_DIO_Output.SelectedIndex;
            bool m_bCurStatus = checkedListBox_Config_LDUL_DIO_Output.GetItemChecked(m_nIndex);
            int m_nOutputChannel = 0;

            if (m_nIndex < 0)
                return;

            //  LDUL Output IO List (괄호는 실제 IO 번호)

            //  여기부터 Module 2
            //  0 (0) : Loader Transfer Inner Vacuum On
            //  1 (1) : Loader Transfer Outer Vacuum On
            //  2 (2) : Loader Transfer Air Blow On
            //  3 (3) : M - Aligner Center Vacuum On
            //  4 (4) : M - Aligner Inner Vacuum On
            //  5 (5) : M - Aligner Outer Vacuum On
            //  6 (6) : M - Aligner Center Air Blow On
            //  7 (7) : Loader Port Ionizer On
            //  8 (8) : M - Aligner Inner Air Blow On
            //  9 (9) : M - Aligner Outer Air Blow On

            //  여기부터 Module 3
            //  10 (16) : Unloader Tansfer Inner Vacuum On
            //  11 (17) : Unloader Tansfer Outer Vacuum On
            //  12 (18) : Unloader Tansfer Air Blow On

            DioPoint dioPoint;

            if ((m_nIndex >= 0) && (m_nIndex <= 9))                                                         //  Loader Output (0 ~ 7) --> 2개 추가하여 9까지
            {
                foreach (DioPoint point in Equipment.GetAllDioPointList())
                {
                    dioPoint = point;

                    if (dioPoint == null)
                        return;

                    //  0번부터 시작하므로 그대로 사용
                    m_nOutputChannel = m_nIndex;

                    if ((dioPoint.IoType == IoType.Output) && (dioPoint.ModuleNo == 2) && (dioPoint.Address == m_nOutputChannel))
                    {
                        if (m_bCurStatus)
                            dioPoint.Write(DioValue.Off);
                        else
                            dioPoint.Write(DioValue.On);

                        break;
                    }
                }
            }
            else if ((m_nIndex >= 10) && (m_nIndex <= 12))                                                   //  Unloader Output (16 ~ 18) --> (0 ~ 2)
            {
                foreach (DioPoint point in Equipment.GetAllDioPointList())
                {
                    dioPoint = point;

                    if (dioPoint == null)
                        return;

                    //  0번부터 시작하므로 변환하여 사용 (10일 때0과 같음)
                    m_nOutputChannel = m_nIndex - 10;

                    if ((dioPoint.IoType == IoType.Output) && (dioPoint.ModuleNo == 3) && (dioPoint.Address == m_nOutputChannel))
                    {
                        if (m_bCurStatus)
                            dioPoint.Write(DioValue.Off);
                        else
                            dioPoint.Write(DioValue.On);

                        break;
                    }
                }
            }
        }

        private void checkedListBox_Config_WorkStage_DIO_Output_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //  Work Stage Output IO 상태 변경

            int m_nIndex = checkedListBox_Config_WorkStage_DIO_Output.SelectedIndex;
            bool m_bCurStatus = checkedListBox_Config_WorkStage_DIO_Output.GetItemChecked(m_nIndex);
            int m_nOutputChannel = 0;

            if (m_nIndex < 0)
                return;

            //  WorkStage Output IO List (괄호는 실제 IO 번호)

            //  0 (0) : Start Switch Lamp
            //  1 (1) : Stop Switch Lamp
            //  2 (2) : Reset Switch Lamp
            //  3 (3) : TowerLamp Red
            //  4 (4) : TowerLamp Yellow
            //  5 (5) : TowerLamp Green
            //  6 (6) : TowerLamp Buzzer
            //  7 (21) : Work Stage Vacuum On
            //  8 (22) : Laser Cal - Sheet Vacuum On
            //  9 (23) : Work Stage Air Blow On
            //  10 (24) : Laser Cal - Sheet Air Blow On
            //  11 (26) : Dust Collector 0 Air Pulse Run
            //  12 (27) : Dust Collector 1 Air Pulse Run

            DioPoint dioPoint;

            if ((m_nIndex >= 0) && (m_nIndex <= 6))                                                         //  Work Stage Output (0 ~ 6)
            {
                foreach (DioPoint point in Equipment.GetAllDioPointList())
                {
                    dioPoint = point;

                    if (dioPoint == null)
                        return;

                    //  0번부터 시작하므로 그대로 사용
                    m_nOutputChannel = m_nIndex;

                    if ((dioPoint.ModuleNo == 1) && (dioPoint.Address == m_nOutputChannel))
                    {
                        if (m_bCurStatus)
                            dioPoint.Write(DioValue.Off);
                        else
                            dioPoint.Write(DioValue.On);

                        break;
                    }
                }
            }
            else if ((m_nIndex >= 7) && (m_nIndex <= 10))                                                   //  Work Stage Output (21 ~ 24)
            {
                foreach (DioPoint point in Equipment.GetAllDioPointList())
                {
                    dioPoint = point;

                    if (dioPoint == null)
                        return;

                    //  21번부터 시작하므로 변환하여 사용 (7일 때 21과 같음)
                    m_nOutputChannel = m_nIndex + 14;

                    if ((dioPoint.ModuleNo == 1) && (dioPoint.Address == m_nOutputChannel))
                    {
                        if (m_bCurStatus)
                            dioPoint.Write(DioValue.Off);
                        else
                            dioPoint.Write(DioValue.On);

                        break;
                    }
                }
            }
            else if ((m_nIndex >= 11) && (m_nIndex <= 12))                                                   //  Work Stage Output (26 ~ 27)
            {
                foreach (DioPoint point in Equipment.GetAllDioPointList())
                {
                    dioPoint = point;

                    if (dioPoint == null)
                        return;

                    //  26번부터 시작하므로 변환하여 사용 (11일 때 26과 같음)
                    m_nOutputChannel = m_nIndex + 15;

                    if ((dioPoint.ModuleNo == 1) && (dioPoint.Address == m_nOutputChannel))
                    {
                        if (m_bCurStatus)
                            dioPoint.Write(DioValue.Off);
                        else
                            dioPoint.Write(DioValue.On);

                        break;
                    }
                }
            }
        }

        private void checkedListBox_Config_Laser_DIO_Output_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //  Laser & Scanner Output IO 상태 변경

            int m_nIndex = checkedListBox_Config_Laser_DIO_Output.SelectedIndex;
            bool m_bCurStatus = checkedListBox_Config_Laser_DIO_Output.GetItemChecked(m_nIndex);
            int m_nOutputChannel = 0;

            if (m_nIndex < 0)
                return;

            //  Laser & Scanner Output IO List (괄호는 실제 IO 번호)

            //  0 (7) : Laser Coolant Supply
            //  1 (8) : Laser Coolant Return
            //  2 (11) : Scanner Coolant Supply
            //  3 (12) : Scanner Coolant Return
            //  4 (13) : Varioscan Coolant Supply
            //  5 (14) : Varioscan Coolant Return
            //  6 (18) : Laser Purge
            //  7 (19) : Scanner Purge
            //  8 (20) : Varioscan Purge
            //  9 (25) : Laser Shutter Command
            //  10 (28) : Laser Enable

            DioPoint dioPoint;

            if ((m_nIndex >= 0) && (m_nIndex <= 1))                                                         //  Laser & Scanner Output (7 ~ 8)
            {
                foreach (DioPoint point in Equipment.GetAllDioPointList())
                {
                    dioPoint = point;

                    if (dioPoint == null)
                        return;

                    //  7번부터 시작하므로 변환하여 사용 (0일 때 7과 같음)
                    m_nOutputChannel = m_nIndex + 7;

                    if ((dioPoint.ModuleNo == 1) && (dioPoint.Address == m_nOutputChannel))
                    {
                        if (m_bCurStatus)
                            dioPoint.Write(DioValue.Off);
                        else
                            dioPoint.Write(DioValue.On);

                        break;
                    }
                }
            }
            else if ((m_nIndex >= 2) && (m_nIndex <= 5))                                                    //  Laser & Scanner Output (11 ~ 14)
            {
                foreach (DioPoint point in Equipment.GetAllDioPointList())
                {
                    dioPoint = point;

                    if (dioPoint == null)
                        return;

                    //  11번부터 시작하므로 변환하여 사용 (2일 때 11과 같음)
                    m_nOutputChannel = m_nIndex + 9;

                    if ((dioPoint.ModuleNo == 1) && (dioPoint.Address == m_nOutputChannel))
                    {
                        if (m_bCurStatus)
                            dioPoint.Write(DioValue.Off);
                        else
                            dioPoint.Write(DioValue.On);

                        break;
                    }
                }
            }
            else if ((m_nIndex >= 6) && (m_nIndex <= 8))                                                    //  Laser & Scanner Output (18 ~ 20)
            {
                foreach (DioPoint point in Equipment.GetAllDioPointList())
                {
                    dioPoint = point;

                    if (dioPoint == null)
                        return;

                    //  18번부터 시작하므로 변환하여 사용 (6일 때 18과 같음)
                    m_nOutputChannel = m_nIndex + 12;

                    if ((dioPoint.ModuleNo == 1) && (dioPoint.Address == m_nOutputChannel))
                    {
                        if (m_bCurStatus)
                            dioPoint.Write(DioValue.Off);
                        else
                            dioPoint.Write(DioValue.On);

                        break;
                    }
                }
            }
            else if (m_nIndex == 9)                                                                         //  Laser & Scanner Output (25)
            {
                foreach (DioPoint point in Equipment.GetAllDioPointList())
                {
                    dioPoint = point;

                    if (dioPoint == null)
                        return;

                    //  25번부터 시작하므로 변환하여 사용 (9일 때 25와 같음)
                    m_nOutputChannel = m_nIndex + 16;

                    if ((dioPoint.ModuleNo == 1) && (dioPoint.Address == m_nOutputChannel))
                    {
                        if (m_bCurStatus)
                            dioPoint.Write(DioValue.Off);
                        else
                            dioPoint.Write(DioValue.On);

                        break;
                    }
                }
            }
            else if (m_nIndex == 10)                                                                         //  Laser & Scanner Output (28)
            {
                foreach (DioPoint point in Equipment.GetAllDioPointList())
                {
                    dioPoint = point;

                    if (dioPoint == null)
                        return;

                    //  28번부터 시작하므로 변환하여 사용 (10일 때 28와 같음)
                    m_nOutputChannel = m_nIndex + 18;

                    if ((dioPoint.ModuleNo == 1) && (dioPoint.Address == m_nOutputChannel))
                    {
                        if (m_bCurStatus)
                            dioPoint.Write(DioValue.Off);
                        else
                            dioPoint.Write(DioValue.On);

                        break;
                    }
                }
            }
        }

        private void checkedListBox_Config_BDS_DIO_Output_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //  BDS Output IO 상태 변경

            int m_nIndex = checkedListBox_Config_BDS_DIO_Output.SelectedIndex;
            bool m_bCurStatus = checkedListBox_Config_BDS_DIO_Output.GetItemChecked(m_nIndex);
            int m_nOutputChannel = 0;

            if (m_nIndex < 0)
                return;

            //  BDS Output IO List (괄호는 실제 IO 번호)

            //  0 (9) : Mask Coolant Supply
            //  1 (10) : Mask Coolant Return
            //  2 (15) : BDS PowerMeter FW
            //  3 (16) : BDS PowerMeter BW
            //  4 (17) : BDS Purge


            DioPoint dioPoint;

            if ((m_nIndex >= 0) && (m_nIndex <= 1))                                                         //  BDS Output (9 ~ 10)
            {
                foreach (DioPoint point in Equipment.GetAllDioPointList())
                {
                    dioPoint = point;

                    if (dioPoint == null)
                        return;

                    //  9번부터 시작하므로 변환하여 사용 (0일 때 9와 같음)
                    m_nOutputChannel = m_nIndex + 9;

                    if ((dioPoint.ModuleNo == 1) && (dioPoint.Address == m_nOutputChannel))
                    {
                        if (m_bCurStatus)
                            dioPoint.Write(DioValue.Off);
                        else
                            dioPoint.Write(DioValue.On);

                        break;
                    }
                }
            }
            else if ((m_nIndex >= 2) && (m_nIndex <= 4))                                                    //  BDS Output (15 ~ 17)
            {
                foreach (DioPoint point in Equipment.GetAllDioPointList())
                {
                    dioPoint = point;

                    if (dioPoint == null)
                        return;

                    //  15번부터 시작하므로 변환하여 사용 (2일 때 15와 같음)
                    m_nOutputChannel = m_nIndex + 13;

                    if ((dioPoint.ModuleNo == 1) && (dioPoint.Address == m_nOutputChannel))
                    {
                        if (m_bCurStatus)
                            dioPoint.Write(DioValue.Off);
                        else
                            dioPoint.Write(DioValue.On);

                        break;
                    }
                }
            }
        }

        private void button_Config_LDUL_TRX_Neg_MouseDown(object sender, MouseEventArgs e)
        {
            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDirection = -1.0;

            if (!this.radioButton_Config_LDUL_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  Active Unit 선택에 따라 동작
                    if (radioButton_Config_ActiveUnit_Loader.Checked)
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.TR_X].Jog_Speed_Fine;
                            lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.TR_X].Common_Acceleration_Fine;
                        }
                        else
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.TR_X].Jog_Speed_Coarse;
                            lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.TR_X].Common_Acceleration_Coarse;
                        }

                        //  방향 설정
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Negative direction : Using - (minus) velocity

                        loader.MC_Func.MC_JogMove((int)Loader.nAxis.TR_X, lfVelocity * dDirection, lfAccDec, lfAccDec);
                    }
                    else
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.TR_X].Jog_Speed_Fine;
                            lfAccDec = Equipment.stAxisParam[(int)Unloader.nAxis.TR_X].Common_Acceleration_Fine;
                        }
                        else
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.TR_X].Jog_Speed_Coarse;
                            lfAccDec = Equipment.stAxisParam[(int)Unloader.nAxis.TR_X].Common_Acceleration_Coarse;
                        }

                        //  방향 설정
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Negative direction : Using - (minus) velocity

                        unloader.MC_Func.MC_JogMove((int)Unloader.nAxis.TR_X, lfVelocity * dDirection, lfAccDec, lfAccDec);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_LDUL_TRX_Pos_MouseDown(object sender, MouseEventArgs e)
        {
            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDirection = 1.0;

            if (!this.radioButton_Config_LDUL_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  Active Unit 선택에 따라 동작
                    if (radioButton_Config_ActiveUnit_Loader.Checked)
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.TR_X].Jog_Speed_Fine;
                            lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.TR_X].Common_Acceleration_Fine;
                        }
                        else
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.TR_X].Jog_Speed_Coarse;
                            lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.TR_X].Common_Acceleration_Coarse;
                        }

                        //  방향 설정
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Positive direction : Using + (plus) velocity

                        loader.MC_Func.MC_JogMove((int)Loader.nAxis.TR_X, lfVelocity * dDirection, lfAccDec, lfAccDec);
                    }
                    else
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.TR_X].Jog_Speed_Fine;
                            lfAccDec = Equipment.stAxisParam[(int)Unloader.nAxis.TR_X].Common_Acceleration_Fine;
                        }
                        else
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.TR_X].Jog_Speed_Coarse;
                            lfAccDec = Equipment.stAxisParam[(int)Unloader.nAxis.TR_X].Common_Acceleration_Coarse;
                        }

                        //  방향 설정
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Positive direction : Using + (plus) velocity

                        unloader.MC_Func.MC_JogMove((int)Unloader.nAxis.TR_X, lfVelocity * dDirection, lfAccDec, lfAccDec);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_LDUL_TRZ_Neg_MouseDown(object sender, MouseEventArgs e)
        {
            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDirection = -1.0;

            if (!this.radioButton_Config_LDUL_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  Active Unit 선택에 따라 동작
                    if (radioButton_Config_ActiveUnit_Loader.Checked)
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.TR_Z].Jog_Speed_Fine;
                            lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.TR_Z].Common_Acceleration_Fine;
                        }
                        else
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.TR_Z].Jog_Speed_Coarse;
                            lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.TR_Z].Common_Acceleration_Coarse;
                        }

                        //  방향 설정
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Negative direction : Using - (minus) velocity

                        loader.MC_Func.MC_JogMove((int)Loader.nAxis.TR_Z, lfVelocity * dDirection, lfAccDec, lfAccDec);
                    }
                    else
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.TR_Z].Jog_Speed_Fine;
                            lfAccDec = Equipment.stAxisParam[(int)Unloader.nAxis.TR_Z].Common_Acceleration_Fine;
                        }
                        else
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.TR_Z].Jog_Speed_Coarse;
                            lfAccDec = Equipment.stAxisParam[(int)Unloader.nAxis.TR_Z].Common_Acceleration_Coarse;
                        }

                        //  방향 설정
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Negative direction : Using - (minus) velocity

                        unloader.MC_Func.MC_JogMove((int)Unloader.nAxis.TR_Z, lfVelocity * dDirection, lfAccDec, lfAccDec);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_LDUL_TRZ_Pos_MouseDown(object sender, MouseEventArgs e)
        {
            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDirection = 1.0;

            if (!this.radioButton_Config_LDUL_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  Active Unit 선택에 따라 동작
                    if (radioButton_Config_ActiveUnit_Loader.Checked)
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.TR_Z].Jog_Speed_Fine;
                            lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.TR_Z].Common_Acceleration_Fine;
                        }
                        else
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.TR_Z].Jog_Speed_Coarse;
                            lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.TR_Z].Common_Acceleration_Coarse;
                        }

                        //  방향 설정
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Positive direction : Using + (plus) velocity

                        loader.MC_Func.MC_JogMove((int)Loader.nAxis.TR_Z, lfVelocity * dDirection, lfAccDec, lfAccDec);
                    }
                    else
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.TR_Z].Jog_Speed_Fine;
                            lfAccDec = Equipment.stAxisParam[(int)Unloader.nAxis.TR_Z].Common_Acceleration_Fine;
                        }
                        else
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.TR_Z].Jog_Speed_Coarse;
                            lfAccDec = Equipment.stAxisParam[(int)Unloader.nAxis.TR_Z].Common_Acceleration_Coarse;
                        }

                        //  방향 설정
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Positive direction : Using + (plus) velocity

                        unloader.MC_Func.MC_JogMove((int)Unloader.nAxis.TR_Z, lfVelocity * dDirection, lfAccDec, lfAccDec);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_LDUL_Z0_Neg_MouseDown(object sender, MouseEventArgs e)
        {
            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDirection = -1.0;

            if (!this.radioButton_Config_LDUL_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  Active Unit 선택에 따라 동작
                    if (radioButton_Config_ActiveUnit_Loader.Checked)
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.Z0].Jog_Speed_Fine;
                            lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.Z0].Common_Acceleration_Fine;
                        }
                        else
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.Z0].Jog_Speed_Coarse;
                            lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.Z0].Common_Acceleration_Coarse;
                        }

                        //  방향 설정
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Negative direction : Using - (minus) velocity

                        loader.MC_Func.MC_JogMove((int)Loader.nAxis.Z0, lfVelocity * dDirection, lfAccDec, lfAccDec);
                    }
                    else
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.Z0].Jog_Speed_Fine;
                            lfAccDec = Equipment.stAxisParam[(int)Unloader.nAxis.Z0].Common_Acceleration_Fine;
                        }
                        else
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.Z0].Jog_Speed_Coarse;
                            lfAccDec = Equipment.stAxisParam[(int)Unloader.nAxis.Z0].Common_Acceleration_Coarse;
                        }

                        //  방향 설정
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Negative direction : Using - (minus) velocity

                        unloader.MC_Func.MC_JogMove((int)Unloader.nAxis.Z0, lfVelocity * dDirection, lfAccDec, lfAccDec);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_LDUL_Z0_Pos_MouseDown(object sender, MouseEventArgs e)
        {
            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDirection = 1.0;

            if (!this.radioButton_Config_LDUL_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  Active Unit 선택에 따라 동작
                    if (radioButton_Config_ActiveUnit_Loader.Checked)
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.Z0].Jog_Speed_Fine;
                            lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.Z0].Common_Acceleration_Fine;
                        }
                        else
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.Z0].Jog_Speed_Coarse;
                            lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.Z0].Common_Acceleration_Coarse;
                        }

                        //  방향 설정
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Positive direction : Using + (plus) velocity

                        loader.MC_Func.MC_JogMove((int)Loader.nAxis.Z0, lfVelocity * dDirection, lfAccDec, lfAccDec);
                    }
                    else
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.Z0].Jog_Speed_Fine;
                            lfAccDec = Equipment.stAxisParam[(int)Unloader.nAxis.Z0].Common_Acceleration_Fine;
                        }
                        else
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.Z0].Jog_Speed_Coarse;
                            lfAccDec = Equipment.stAxisParam[(int)Unloader.nAxis.Z0].Common_Acceleration_Coarse;
                        }

                        //  방향 설정
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Positive direction : Using + (plus) velocity

                        unloader.MC_Func.MC_JogMove((int)Unloader.nAxis.Z0, lfVelocity * dDirection, lfAccDec, lfAccDec);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_LDUL_Z1_Neg_MouseDown(object sender, MouseEventArgs e)
        {
            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDirection = -1.0;

            if (!this.radioButton_Config_LDUL_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  Active Unit 선택에 따라 동작
                    if (radioButton_Config_ActiveUnit_Loader.Checked)
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.Z1].Jog_Speed_Fine;
                            lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.Z1].Common_Acceleration_Fine;
                        }
                        else
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.Z1].Jog_Speed_Coarse;
                            lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.Z1].Common_Acceleration_Coarse;
                        }

                        //  방향 설정
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Negative direction : Using - (minus) velocity

                        loader.MC_Func.MC_JogMove((int)Loader.nAxis.Z1, lfVelocity * dDirection, lfAccDec, lfAccDec);
                    }
                    else
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.Z1].Jog_Speed_Fine;
                            lfAccDec = Equipment.stAxisParam[(int)Unloader.nAxis.Z1].Common_Acceleration_Fine;
                        }
                        else
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.Z1].Jog_Speed_Coarse;
                            lfAccDec = Equipment.stAxisParam[(int)Unloader.nAxis.Z1].Common_Acceleration_Coarse;
                        }

                        //  방향 설정
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Negative direction : Using - (minus) velocity

                        unloader.MC_Func.MC_JogMove((int)Unloader.nAxis.Z1, lfVelocity * dDirection, lfAccDec, lfAccDec);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_LDUL_Z1_Pos_MouseDown(object sender, MouseEventArgs e)
        {
            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDirection = 1.0;

            if (!this.radioButton_Config_LDUL_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  Active Unit 선택에 따라 동작
                    if (radioButton_Config_ActiveUnit_Loader.Checked)
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.Z1].Jog_Speed_Fine;
                            lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.Z1].Common_Acceleration_Fine;
                        }
                        else
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.Z1].Jog_Speed_Coarse;
                            lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.Z1].Common_Acceleration_Coarse;
                        }

                        //  방향 설정
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Positive direction : Using + (plus) velocity

                        loader.MC_Func.MC_JogMove((int)Loader.nAxis.Z1, lfVelocity * dDirection, lfAccDec, lfAccDec);
                    }
                    else
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.Z1].Jog_Speed_Fine;
                            lfAccDec = Equipment.stAxisParam[(int)Unloader.nAxis.Z1].Common_Acceleration_Fine;
                        }
                        else
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.Z1].Jog_Speed_Coarse;
                            lfAccDec = Equipment.stAxisParam[(int)Unloader.nAxis.Z1].Common_Acceleration_Coarse;
                        }

                        //  방향 설정
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Positive direction : Using + (plus) velocity

                        unloader.MC_Func.MC_JogMove((int)Unloader.nAxis.Z1, lfVelocity * dDirection, lfAccDec, lfAccDec);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_LD_ALNX_Neg_MouseDown(object sender, MouseEventArgs e)
        {
            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDirection = -1.0;

            if (!this.radioButton_Config_LDUL_JogMove_Continuous.Checked)
                return;

            if (!radioButton_Config_ActiveUnit_Loader.Checked)
            {
                MessageBox.Show("Please select the Loader Unit.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.ALN_X].Jog_Speed_Fine;
                        lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.ALN_X].Common_Acceleration_Fine;
                    }
                    else
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.ALN_X].Jog_Speed_Coarse;
                        lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.ALN_X].Common_Acceleration_Coarse;
                    }

                    //  방향 설정
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Negative direction : Using - (minus) velocity

                    loader.MC_Func.MC_JogMove((int)Loader.nAxis.ALN_X, lfVelocity * dDirection, lfAccDec, lfAccDec);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_LD_ALNX_Pos_MouseDown(object sender, MouseEventArgs e)
        {
            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDirection = 1.0;

            if (!this.radioButton_Config_LDUL_JogMove_Continuous.Checked)
                return;

            if (!radioButton_Config_ActiveUnit_Loader.Checked)
            {
                MessageBox.Show("Please select the Loader Unit.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.ALN_X].Jog_Speed_Fine;
                        lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.ALN_X].Common_Acceleration_Fine;
                    }
                    else
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.ALN_X].Jog_Speed_Coarse;
                        lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.ALN_X].Common_Acceleration_Coarse;
                    }

                    //  방향 설정
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Positive direction : Using + (plus) velocity

                    loader.MC_Func.MC_JogMove((int)Loader.nAxis.ALN_X, lfVelocity * dDirection, lfAccDec, lfAccDec);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_LD_ALNY_Neg_MouseDown(object sender, MouseEventArgs e)
        {
            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDirection = -1.0;

            if (!this.radioButton_Config_LDUL_JogMove_Continuous.Checked)
                return;

            if (!radioButton_Config_ActiveUnit_Loader.Checked)
            {
                MessageBox.Show("Please select the Loader Unit.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.ALN_Y].Jog_Speed_Fine;
                        lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.ALN_Y].Common_Acceleration_Fine;
                    }
                    else
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.ALN_Y].Jog_Speed_Coarse;
                        lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.ALN_Y].Common_Acceleration_Coarse;
                    }

                    //  방향 설정
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Negative direction : Using - (minus) velocity

                    loader.MC_Func.MC_JogMove((int)Loader.nAxis.ALN_Y, lfVelocity * dDirection, lfAccDec, lfAccDec);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_LD_ALNY_Pos_MouseDown(object sender, MouseEventArgs e)
        {
            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDirection = 1.0;

            if (!this.radioButton_Config_LDUL_JogMove_Continuous.Checked)
                return;

            if (!radioButton_Config_ActiveUnit_Loader.Checked)
            {
                MessageBox.Show("Please select the Loader Unit.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.ALN_Y].Jog_Speed_Fine;
                        lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.ALN_Y].Common_Acceleration_Fine;
                    }
                    else
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.ALN_Y].Jog_Speed_Coarse;
                        lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.ALN_Y].Common_Acceleration_Coarse;
                    }

                    //  방향 설정
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Positive direction : Using + (plus) velocity

                    loader.MC_Func.MC_JogMove((int)Loader.nAxis.ALN_Y, lfVelocity * dDirection, lfAccDec, lfAccDec);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_LDUL_Axis_MouseUp(object sender, MouseEventArgs e)
        {
            if (!this.radioButton_Config_LDUL_JogMove_Continuous.Checked)
                return;

            if (Equipment.AjinBoard_Opened)
            {
                if (radioButton_Config_ActiveUnit_Loader.Checked)
                {
                    loader.MC_Func.MC_JogStop((int)Loader.nAxis.Z0);
                    loader.MC_Func.MC_JogStop((int)Loader.nAxis.Z1);
                    loader.MC_Func.MC_JogStop((int)Loader.nAxis.TR_X);
                    loader.MC_Func.MC_JogStop((int)Loader.nAxis.TR_Z);
                    loader.MC_Func.MC_JogStop((int)Loader.nAxis.ALN_X);
                    loader.MC_Func.MC_JogStop((int)Loader.nAxis.ALN_Y);
                }
                else
                {
                    unloader.MC_Func.MC_JogStop((int)Unloader.nAxis.Z0);
                    unloader.MC_Func.MC_JogStop((int)Unloader.nAxis.Z1);
                    unloader.MC_Func.MC_JogStop((int)Unloader.nAxis.TR_X);
                    unloader.MC_Func.MC_JogStop((int)Unloader.nAxis.TR_Z);
                }
            }
        }

        private void button_Config_LDUL_TRX_Neg_Click(object sender, EventArgs e)
        {
            if (!this.radioButton_Config_LDUL_JogMove_Step.Checked)
                return;

            double lfVelocity = 0.0f;
            double lfAccDec = 0.0;
            double dDistance = Convert.ToDouble(textBox_Config_LDUL_JogMove_StepSize.Text);
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
                    //  Active Unit 선택에 따라 동작
                    if (radioButton_Config_ActiveUnit_Loader.Checked)
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.TR_X].Jog_Speed_Fine;
                            lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.TR_X].Common_Acceleration_Fine;
                        }
                        else
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.TR_X].Jog_Speed_Coarse;
                            lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.TR_X].Common_Acceleration_Coarse;
                        }

                        //  속도는 양수로
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                        loader.MC_Func.MC_MoveRelPosition((int)Loader.nAxis.TR_X, dDistance * (double)nDirection, lfVelocity, lfAccDec, lfAccDec);
                    }
                    else
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.TR_X].Jog_Speed_Fine;
                            lfAccDec = Equipment.stAxisParam[(int)Unloader.nAxis.TR_X].Common_Acceleration_Fine;
                        }
                        else
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.TR_X].Jog_Speed_Coarse;
                            lfAccDec = Equipment.stAxisParam[(int)Unloader.nAxis.TR_X].Common_Acceleration_Coarse;
                        }

                        //  속도는 양수로
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                        unloader.MC_Func.MC_MoveRelPosition((int)Unloader.nAxis.TR_X, dDistance * (double)nDirection, lfVelocity, lfAccDec, lfAccDec);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_LDUL_TRX_Pos_Click(object sender, EventArgs e)
        {
            if (!this.radioButton_Config_LDUL_JogMove_Step.Checked)
                return;

            double lfVelocity = 0.0f;
            double lfAccDec = 0.0;
            double dDistance = Convert.ToDouble(textBox_Config_LDUL_JogMove_StepSize.Text);
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
                    //  Active Unit 선택에 따라 동작
                    if (radioButton_Config_ActiveUnit_Loader.Checked)
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.TR_X].Jog_Speed_Fine;
                            lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.TR_X].Common_Acceleration_Fine;
                        }
                        else
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.TR_X].Jog_Speed_Coarse;
                            lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.TR_X].Common_Acceleration_Coarse;
                        }

                        //  속도는 양수로
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                        loader.MC_Func.MC_MoveRelPosition((int)Loader.nAxis.TR_X, dDistance * (double)nDirection, lfVelocity, lfAccDec, lfAccDec);
                    }
                    else
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.TR_X].Jog_Speed_Fine;
                            lfAccDec = Equipment.stAxisParam[(int)Unloader.nAxis.TR_X].Common_Acceleration_Fine;
                        }
                        else
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.TR_X].Jog_Speed_Coarse;
                            lfAccDec = Equipment.stAxisParam[(int)Unloader.nAxis.TR_X].Common_Acceleration_Coarse;
                        }

                        //  속도는 양수로
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                        unloader.MC_Func.MC_MoveRelPosition((int)Unloader.nAxis.TR_X, dDistance * (double)nDirection, lfVelocity, lfAccDec, lfAccDec);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_LDUL_TRZ_Neg_Click(object sender, EventArgs e)
        {
            if (!this.radioButton_Config_LDUL_JogMove_Step.Checked)
                return;

            double lfVelocity = 0.0f;
            double lfAccDec = 0.0;
            double dDistance = Convert.ToDouble(textBox_Config_LDUL_JogMove_StepSize.Text);
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
                    //  Active Unit 선택에 따라 동작
                    if (radioButton_Config_ActiveUnit_Loader.Checked)
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.TR_Z].Jog_Speed_Fine;
                            lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.TR_Z].Common_Acceleration_Fine;
                        }
                        else
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.TR_Z].Jog_Speed_Coarse;
                            lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.TR_Z].Common_Acceleration_Coarse;
                        }

                        //  속도는 양수로
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                        loader.MC_Func.MC_MoveRelPosition((int)Loader.nAxis.TR_Z, dDistance * (double)nDirection, lfVelocity, lfAccDec, lfAccDec);
                    }
                    else
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.TR_Z].Jog_Speed_Fine;
                            lfAccDec = Equipment.stAxisParam[(int)Unloader.nAxis.TR_Z].Common_Acceleration_Fine;
                        }
                        else
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.TR_Z].Jog_Speed_Coarse;
                            lfAccDec = Equipment.stAxisParam[(int)Unloader.nAxis.TR_Z].Common_Acceleration_Coarse;
                        }

                        //  속도는 양수로
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                        unloader.MC_Func.MC_MoveRelPosition((int)Unloader.nAxis.TR_Z, dDistance * (double)nDirection, lfVelocity, lfAccDec, lfAccDec);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_LDUL_TRZ_Pos_Click(object sender, EventArgs e)
        {
            if (!this.radioButton_Config_LDUL_JogMove_Step.Checked)
                return;

            double lfVelocity = 0.0f;
            double lfAccDec = 0.0;
            double dDistance = Convert.ToDouble(textBox_Config_LDUL_JogMove_StepSize.Text);
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
                    //  Active Unit 선택에 따라 동작
                    if (radioButton_Config_ActiveUnit_Loader.Checked)
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.TR_Z].Jog_Speed_Fine;
                            lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.TR_Z].Common_Acceleration_Fine;
                        }
                        else
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.TR_Z].Jog_Speed_Coarse;
                            lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.TR_Z].Common_Acceleration_Coarse;
                        }

                        //  속도는 양수로
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                        loader.MC_Func.MC_MoveRelPosition((int)Loader.nAxis.TR_Z, dDistance * (double)nDirection, lfVelocity, lfAccDec, lfAccDec);
                    }
                    else
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.TR_Z].Jog_Speed_Fine;
                            lfAccDec = Equipment.stAxisParam[(int)Unloader.nAxis.TR_Z].Common_Acceleration_Fine;
                        }
                        else
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.TR_Z].Jog_Speed_Coarse;
                            lfAccDec = Equipment.stAxisParam[(int)Unloader.nAxis.TR_Z].Common_Acceleration_Coarse;
                        }

                        //  속도는 양수로
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                        unloader.MC_Func.MC_MoveRelPosition((int)Unloader.nAxis.TR_Z, dDistance * (double)nDirection, lfVelocity, lfAccDec, lfAccDec);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_LDUL_Z0_Neg_Click(object sender, EventArgs e)
        {
            if (!this.radioButton_Config_LDUL_JogMove_Step.Checked)
                return;

            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDistance = Convert.ToDouble(textBox_Config_LDUL_JogMove_StepSize.Text);
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
                    //  Active Unit 선택에 따라 동작
                    if (radioButton_Config_ActiveUnit_Loader.Checked)
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.Z0].Jog_Speed_Fine;
                            lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.Z0].Common_Acceleration_Fine;
                        }
                        else
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.Z0].Jog_Speed_Coarse;
                            lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.Z0].Common_Acceleration_Coarse;
                        }

                        //  속도는 양수로
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                        loader.MC_Func.MC_MoveRelPosition((int)Loader.nAxis.Z0, dDistance * (double)nDirection, lfVelocity, lfAccDec, lfAccDec);
                    }
                    else
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.Z0].Jog_Speed_Fine;
                            lfAccDec = Equipment.stAxisParam[(int)Unloader.nAxis.Z0].Common_Acceleration_Fine;
                        }
                        else
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.Z0].Jog_Speed_Coarse;
                            lfAccDec = Equipment.stAxisParam[(int)Unloader.nAxis.Z0].Common_Acceleration_Coarse;
                        }

                        //  속도는 양수로
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                        unloader.MC_Func.MC_MoveRelPosition((int)Unloader.nAxis.Z0, dDistance * (double)nDirection, lfVelocity, lfAccDec, lfAccDec);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_LDUL_Z0_Pos_Click(object sender, EventArgs e)
        {
            if (!this.radioButton_Config_LDUL_JogMove_Step.Checked)
                return;

            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDistance = Convert.ToDouble(textBox_Config_LDUL_JogMove_StepSize.Text);
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
                    //  Active Unit 선택에 따라 동작
                    if (radioButton_Config_ActiveUnit_Loader.Checked)
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.Z0].Jog_Speed_Fine;
                            lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.Z0].Common_Acceleration_Fine;
                        }
                        else
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.Z0].Jog_Speed_Coarse;
                            lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.Z0].Common_Acceleration_Coarse;
                        }

                        //  속도는 양수로
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                        loader.MC_Func.MC_MoveRelPosition((int)Loader.nAxis.Z0, dDistance * (double)nDirection, lfVelocity, lfAccDec, lfAccDec);
                    }
                    else
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.Z0].Jog_Speed_Fine;
                            lfAccDec = Equipment.stAxisParam[(int)Unloader.nAxis.Z0].Common_Acceleration_Fine;
                        }
                        else
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.Z0].Jog_Speed_Coarse;
                            lfAccDec = Equipment.stAxisParam[(int)Unloader.nAxis.Z0].Common_Acceleration_Coarse;
                        }

                        //  속도는 양수로
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                        unloader.MC_Func.MC_MoveRelPosition((int)Unloader.nAxis.Z0, dDistance * (double)nDirection, lfVelocity, lfAccDec, lfAccDec);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_LDUL_Z1_Neg_Click(object sender, EventArgs e)
        {
            if (!this.radioButton_Config_LDUL_JogMove_Step.Checked)
                return;

            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDistance = Convert.ToDouble(textBox_Config_LDUL_JogMove_StepSize.Text);
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
                    //  Active Unit 선택에 따라 동작
                    if (radioButton_Config_ActiveUnit_Loader.Checked)
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.Z1].Jog_Speed_Fine;
                            lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.Z1].Common_Acceleration_Fine;
                        }
                        else
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.Z1].Jog_Speed_Coarse;
                            lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.Z1].Common_Acceleration_Coarse;
                        }

                        //  속도는 양수로
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                        loader.MC_Func.MC_MoveRelPosition((int)Loader.nAxis.Z1, dDistance * (double)nDirection, lfVelocity, lfAccDec, lfAccDec);
                    }
                    else
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.Z1].Jog_Speed_Fine;
                            lfAccDec = Equipment.stAxisParam[(int)Unloader.nAxis.Z1].Common_Acceleration_Fine;
                        }
                        else
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.Z1].Jog_Speed_Coarse;
                            lfAccDec = Equipment.stAxisParam[(int)Unloader.nAxis.Z1].Common_Acceleration_Coarse;
                        }

                        //  속도는 양수로
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                        unloader.MC_Func.MC_MoveRelPosition((int)Unloader.nAxis.Z1, dDistance * (double)nDirection, lfVelocity, lfAccDec, lfAccDec);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_LDUL_Z1_Pos_Click(object sender, EventArgs e)
        {
            if (!this.radioButton_Config_LDUL_JogMove_Step.Checked)
                return;

            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDistance = Convert.ToDouble(textBox_Config_LDUL_JogMove_StepSize.Text);
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
                    //  Active Unit 선택에 따라 동작
                    if (radioButton_Config_ActiveUnit_Loader.Checked)
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.Z1].Jog_Speed_Fine;
                            lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.Z1].Common_Acceleration_Fine;
                        }
                        else
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.Z1].Jog_Speed_Coarse;
                            lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.Z1].Common_Acceleration_Coarse;
                        }

                        //  속도는 양수로
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                        loader.MC_Func.MC_MoveRelPosition((int)Loader.nAxis.Z1, dDistance * (double)nDirection, lfVelocity, lfAccDec, lfAccDec);
                    }
                    else
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.Z1].Jog_Speed_Fine;
                            lfAccDec = Equipment.stAxisParam[(int)Unloader.nAxis.Z1].Common_Acceleration_Fine;
                        }
                        else
                        {
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.Z1].Jog_Speed_Coarse;
                            lfAccDec = Equipment.stAxisParam[(int)Unloader.nAxis.Z1].Common_Acceleration_Coarse;
                        }

                        //  속도는 양수로
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                        unloader.MC_Func.MC_MoveRelPosition((int)Unloader.nAxis.Z1, dDistance * (double)nDirection, lfVelocity, lfAccDec, lfAccDec);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_LD_ALNX_Neg_Click(object sender, EventArgs e)
        {
            if (!this.radioButton_Config_LDUL_JogMove_Step.Checked)
                return;

            if (!radioButton_Config_ActiveUnit_Loader.Checked)
            {
                MessageBox.Show("Please select the Loader Unit.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDistance = Convert.ToDouble(textBox_Config_LDUL_JogMove_StepSize.Text);
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
                    if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.ALN_X].Jog_Speed_Fine;
                        lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.ALN_X].Common_Acceleration_Fine;
                    }
                    else
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.ALN_X].Jog_Speed_Coarse;
                        lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.ALN_X].Common_Acceleration_Coarse;
                    }

                    //  속도는 양수로
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                    loader.MC_Func.MC_MoveRelPosition((int)Loader.nAxis.ALN_X, dDistance * (double)nDirection, lfVelocity, lfAccDec, lfAccDec);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_LD_ALNX_Pos_Click(object sender, EventArgs e)
        {
            if (!this.radioButton_Config_LDUL_JogMove_Step.Checked)
                return;

            if (!radioButton_Config_ActiveUnit_Loader.Checked)
            {
                MessageBox.Show("Please select the Loader Unit.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDistance = Convert.ToDouble(textBox_Config_LDUL_JogMove_StepSize.Text);
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
                    if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.ALN_X].Jog_Speed_Fine;
                        lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.ALN_X].Common_Acceleration_Fine;
                    }
                    else
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.ALN_X].Jog_Speed_Coarse;
                        lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.ALN_X].Common_Acceleration_Coarse;
                    }

                    //  속도는 양수로
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                    loader.MC_Func.MC_MoveRelPosition((int)Loader.nAxis.ALN_X, dDistance * (double)nDirection, lfVelocity, lfAccDec, lfAccDec);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_LD_ALNY_Neg_Click(object sender, EventArgs e)
        {
            if (!this.radioButton_Config_LDUL_JogMove_Step.Checked)
                return;

            if (!radioButton_Config_ActiveUnit_Loader.Checked)
            {
                MessageBox.Show("Please select the Loader Unit.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDistance = Convert.ToDouble(textBox_Config_LDUL_JogMove_StepSize.Text);
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
                    if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.ALN_Y].Jog_Speed_Fine;
                        lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.ALN_Y].Common_Acceleration_Fine;
                    }
                    else
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.ALN_Y].Jog_Speed_Coarse;
                        lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.ALN_Y].Common_Acceleration_Coarse;
                    }

                    //  속도는 양수로
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                    loader.MC_Func.MC_MoveRelPosition((int)Loader.nAxis.ALN_Y, dDistance * (double)nDirection, lfVelocity, lfAccDec, lfAccDec);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_LD_ALNY_Pos_Click(object sender, EventArgs e)
        {
            if (!this.radioButton_Config_LDUL_JogMove_Step.Checked)
                return;

            if (!radioButton_Config_ActiveUnit_Loader.Checked)
            {
                MessageBox.Show("Please select the Loader Unit.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDistance = Convert.ToDouble(textBox_Config_LDUL_JogMove_StepSize.Text);
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
                    if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.ALN_Y].Jog_Speed_Fine;
                        lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.ALN_Y].Common_Acceleration_Fine;
                    }
                    else
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.ALN_Y].Jog_Speed_Coarse;
                        lfAccDec = Equipment.stAxisParam[(int)Loader.nAxis.ALN_Y].Common_Acceleration_Coarse;
                    }

                    //  속도는 양수로
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                    loader.MC_Func.MC_MoveRelPosition((int)Loader.nAxis.ALN_Y, dDistance * (double)nDirection, lfVelocity, lfAccDec, lfAccDec);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_WorkStage_X_Neg_MouseDown(object sender, MouseEventArgs e)
        {
            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDirection = -1.0;

            if (!this.radioButton_Config_WorkStage_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_Config_WorkStage_Move_MoveMode_Fine.Checked)
                    {
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Fine;
                        lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Acceleration_Fine;
                    }
                    else
                    {
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Coarse;
                        lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Acceleration_Coarse;
                    }

                    //  방향 설정
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Negative direction : Using - (minus) velocity

                    workStage.MC_Func.MC_JogMove((int)WorkStage.nAxis.X, lfVelocity * dDirection, lfAccDec, lfAccDec);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_WorkStage_X_Pos_MouseDown(object sender, MouseEventArgs e)
        {
            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDirection = 1.0;

            if (!this.radioButton_Config_WorkStage_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_Config_WorkStage_Move_MoveMode_Fine.Checked)
                    {
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Fine;
                        lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Acceleration_Fine;
                    }
                    else
                    {
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Coarse;
                        lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Acceleration_Coarse;
                    }

                    //  방향 설정
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Positive direction : Using + (plus) velocity

                    workStage.MC_Func.MC_JogMove((int)WorkStage.nAxis.X, lfVelocity * dDirection, lfAccDec, lfAccDec);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_WorkStage_Y_Neg_MouseDown(object sender, MouseEventArgs e)
        {
            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDirection = -1.0;

            if (!this.radioButton_Config_WorkStage_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_Config_WorkStage_Move_MoveMode_Fine.Checked)
                    {
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.Y].Jog_Speed_Fine;
                        lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.Y].Common_Acceleration_Fine;
                    }
                    else
                    {
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.Y].Jog_Speed_Coarse;
                        lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.Y].Common_Acceleration_Coarse;
                    }

                    //  방향 설정
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Negative direction : Using - (minus) velocity

                    workStage.MC_Func.MC_JogMove((int)WorkStage.nAxis.Y, lfVelocity * dDirection, lfAccDec, lfAccDec);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_WorkStage_Y_Pos_MouseDown(object sender, MouseEventArgs e)
        {
            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDirection = 1.0;

            if (!this.radioButton_Config_WorkStage_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_Config_WorkStage_Move_MoveMode_Fine.Checked)
                    {
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.Y].Jog_Speed_Fine;
                        lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.Y].Common_Acceleration_Fine;
                    }
                    else
                    {
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.Y].Jog_Speed_Coarse;
                        lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.Y].Common_Acceleration_Coarse;
                    }

                    //  방향 설정
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Positive direction : Using + (plus) velocity

                    workStage.MC_Func.MC_JogMove((int)WorkStage.nAxis.Y, lfVelocity * dDirection, lfAccDec, lfAccDec);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_WorkStage_Z_Neg_MouseDown(object sender, MouseEventArgs e)
        {
            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDirection = -1.0;

            if (!this.radioButton_Config_WorkStage_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_Config_WorkStage_Move_MoveMode_Fine.Checked)
                    {
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.Z].Jog_Speed_Fine;
                        lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.Z].Common_Acceleration_Fine;
                    }
                    else
                    {
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.Z].Jog_Speed_Coarse;
                        lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.Z].Common_Acceleration_Coarse;
                    }

                    //  방향 설정
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Negative direction : Using - (minus) velocity

                    workStage.MC_Func.MC_JogMove((int)WorkStage.nAxis.Z, lfVelocity * dDirection, lfAccDec, lfAccDec);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_WorkStage_Z_Pos_MouseDown(object sender, MouseEventArgs e)
        {
            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDirection = 1.0;

            if (!this.radioButton_Config_WorkStage_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_Config_WorkStage_Move_MoveMode_Fine.Checked)
                    {
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.Z].Jog_Speed_Fine;
                        lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.Z].Common_Acceleration_Fine;
                    }
                    else
                    {
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.Z].Jog_Speed_Coarse;
                        lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.Z].Common_Acceleration_Coarse;
                    }

                    //  방향 설정
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Positive direction : Using + (plus) velocity

                    workStage.MC_Func.MC_JogMove((int)WorkStage.nAxis.Z, lfVelocity * dDirection, lfAccDec, lfAccDec);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_WorkStage_Axis_MouseUp(object sender, MouseEventArgs e)
        {
            if (!this.radioButton_Config_WorkStage_JogMove_Continuous.Checked)
                return;

            if (Equipment.AjinBoard_Opened)
            {
                workStage.MC_Func.MC_JogStop((int)WorkStage.nAxis.X);
                workStage.MC_Func.MC_JogStop((int)WorkStage.nAxis.Y);
                workStage.MC_Func.MC_JogStop((int)WorkStage.nAxis.Z);
            }
        }

        private void button_Config_WorkStage_X_Neg_Click(object sender, EventArgs e)
        {
            if (!this.radioButton_Config_WorkStage_JogMove_Step.Checked)
                return;

            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDistance = Convert.ToDouble(textBox_Config_WorkStage_JogMove_StepSize.Text);
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
                    if (radioButton_Config_WorkStage_Move_MoveMode_Fine.Checked)
                    {
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Fine;
                        lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Acceleration_Fine;
                    }
                    else
                    {
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Coarse;
                        lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Acceleration_Coarse;
                    }

                    //  속도는 양수로
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                    workStage.MC_Func.MC_MoveRelPosition((int)WorkStage.nAxis.X, dDistance * (double)nDirection, lfVelocity, lfAccDec, lfAccDec);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_WorkStage_X_Pos_Click(object sender, EventArgs e)
        {
            if (!this.radioButton_Config_WorkStage_JogMove_Step.Checked)
                return;

            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDistance = Convert.ToDouble(textBox_Config_WorkStage_JogMove_StepSize.Text);
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
                    if (radioButton_Config_WorkStage_Move_MoveMode_Fine.Checked)
                    {
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Fine;
                        lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Acceleration_Fine;
                    }
                    else
                    {
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Coarse;
                        lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Acceleration_Coarse;
                    }

                    //  속도는 양수로
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                    workStage.MC_Func.MC_MoveRelPosition((int)WorkStage.nAxis.X, dDistance * (double)nDirection, lfVelocity, lfAccDec, lfAccDec);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_WorkStage_Y_Neg_Click(object sender, EventArgs e)
        {
            if (!this.radioButton_Config_WorkStage_JogMove_Step.Checked)
                return;

            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDistance = Convert.ToDouble(textBox_Config_WorkStage_JogMove_StepSize.Text);
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
                    if (radioButton_Config_WorkStage_Move_MoveMode_Fine.Checked)
                    {
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.Y].Jog_Speed_Fine;
                        lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.Y].Common_Acceleration_Fine;
                    }
                    else
                    {
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.Y].Jog_Speed_Coarse;
                        lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.Y].Common_Acceleration_Coarse;
                    }

                    //  속도는 양수로
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                    workStage.MC_Func.MC_MoveRelPosition((int)WorkStage.nAxis.Y, dDistance * (double)nDirection, lfVelocity, lfAccDec, lfAccDec);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_WorkStage_Y_Pos_Click(object sender, EventArgs e)
        {
            if (!this.radioButton_Config_WorkStage_JogMove_Step.Checked)
                return;

            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDistance = Convert.ToDouble(textBox_Config_WorkStage_JogMove_StepSize.Text);
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
                    if (radioButton_Config_WorkStage_Move_MoveMode_Fine.Checked)
                    {
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.Y].Jog_Speed_Fine;
                        lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.Y].Common_Acceleration_Fine;
                    }
                    else
                    {
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.Y].Jog_Speed_Coarse;
                        lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.Y].Common_Acceleration_Coarse;
                    }

                    //  속도는 양수로
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                    workStage.MC_Func.MC_MoveRelPosition((int)WorkStage.nAxis.Y, dDistance * (double)nDirection, lfVelocity, lfAccDec, lfAccDec);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_WorkStage_Z_Neg_Click(object sender, EventArgs e)
        {
            if (!this.radioButton_Config_WorkStage_JogMove_Step.Checked)
                return;

            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDistance = Convert.ToDouble(textBox_Config_WorkStage_JogMove_StepSize.Text);
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
                    if (radioButton_Config_WorkStage_Move_MoveMode_Fine.Checked)
                    {
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.Z].Jog_Speed_Fine;
                        lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.Z].Common_Acceleration_Fine;
                    }
                    else
                    {
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.Z].Jog_Speed_Coarse;
                        lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.Z].Common_Acceleration_Coarse;
                    }

                    //  속도는 양수로
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                    workStage.MC_Func.MC_MoveRelPosition((int)WorkStage.nAxis.Z, dDistance * (double)nDirection, lfVelocity, lfAccDec, lfAccDec);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_WorkStage_Z_Pos_Click(object sender, EventArgs e)
        {
            if (!this.radioButton_Config_WorkStage_JogMove_Step.Checked)
                return;

            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDistance = Convert.ToDouble(textBox_Config_WorkStage_JogMove_StepSize.Text);
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
                    if (radioButton_Config_WorkStage_Move_MoveMode_Fine.Checked)
                    {
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.Z].Jog_Speed_Fine;
                        lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.Z].Common_Acceleration_Fine;
                    }
                    else
                    {
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.Z].Jog_Speed_Coarse;
                        lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.Z].Common_Acceleration_Coarse;
                    }

                    //  속도는 양수로
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                    workStage.MC_Func.MC_MoveRelPosition((int)WorkStage.nAxis.Z, dDistance * (double)nDirection, lfVelocity, lfAccDec, lfAccDec);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_Vision_X_Neg_MouseDown(object sender, MouseEventArgs e)
        {
            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDirection = -1.0;

            if (!this.radioButton_Config_Vision_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_Config_Vision_Move_MoveMode_Fine.Checked)
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
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_Vision_X_Pos_MouseDown(object sender, MouseEventArgs e)
        {
            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDirection = 1.0;

            if (!this.radioButton_Config_Vision_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_Config_Vision_Move_MoveMode_Fine.Checked)
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
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_Vision_Y_Neg_MouseDown(object sender, MouseEventArgs e)
        {
            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDirection = -1.0;

            if (!this.radioButton_Config_Vision_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_Config_Vision_Move_MoveMode_Fine.Checked)
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
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_Vision_Y_Pos_MouseDown(object sender, MouseEventArgs e)
        {
            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDirection = 1.0;

            if (!this.radioButton_Config_Vision_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_Config_Vision_Move_MoveMode_Fine.Checked)
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
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_Vision_Z_Neg_MouseDown(object sender, MouseEventArgs e)
        {
            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDirection = -1.0;

            if (!this.radioButton_Config_Vision_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_Config_Vision_Move_MoveMode_Fine.Checked)
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
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_Vision_Z_Pos_MouseDown(object sender, MouseEventArgs e)
        {
            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDirection = 1.0;

            if (!this.radioButton_Config_Vision_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_Config_Vision_Move_MoveMode_Fine.Checked)
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
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_Vision_Axis_MouseUp(object sender, MouseEventArgs e)
        {
            if (!this.radioButton_Config_Vision_JogMove_Continuous.Checked)
                return;

            if (Equipment.AjinBoard_Opened)
            {
                vision.MC_Func.MC_JogStop((int)Vision.nAxis.X);
                vision.MC_Func.MC_JogStop((int)Vision.nAxis.Y);
                vision.MC_Func.MC_JogStop((int)Vision.nAxis.Z);
            }
        }

        private void button_Config_Vision_X_Neg_Click(object sender, EventArgs e)
        {
            if (!this.radioButton_Config_Vision_JogMove_Step.Checked)
                return;

            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDistance = Convert.ToDouble(textBox_Config_Vision_JogMove_StepSize.Text);
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
                    if (radioButton_Config_Vision_Move_MoveMode_Fine.Checked)
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
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_Vision_X_Pos_Click(object sender, EventArgs e)
        {
            if (!this.radioButton_Config_Vision_JogMove_Step.Checked)
                return;

            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDistance = Convert.ToDouble(textBox_Config_Vision_JogMove_StepSize.Text);
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
                    if (radioButton_Config_Vision_Move_MoveMode_Fine.Checked)
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
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_Vision_Y_Neg_Click(object sender, EventArgs e)
        {
            if (!this.radioButton_Config_Vision_JogMove_Step.Checked)
                return;

            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDistance = Convert.ToDouble(textBox_Config_Vision_JogMove_StepSize.Text);
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
                    if (radioButton_Config_Vision_Move_MoveMode_Fine.Checked)
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
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_Vision_Y_Pos_Click(object sender, EventArgs e)
        {
            if (!this.radioButton_Config_Vision_JogMove_Step.Checked)
                return;

            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDistance = Convert.ToDouble(textBox_Config_Vision_JogMove_StepSize.Text);
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
                    if (radioButton_Config_Vision_Move_MoveMode_Fine.Checked)
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
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_Vision_Z_Neg_Click(object sender, EventArgs e)
        {
            if (!this.radioButton_Config_Vision_JogMove_Step.Checked)
                return;

            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDistance = Convert.ToDouble(textBox_Config_Vision_JogMove_StepSize.Text);
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
                    if (radioButton_Config_Vision_Move_MoveMode_Fine.Checked)
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
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_Vision_Z_Pos_Click(object sender, EventArgs e)
        {
            if (!this.radioButton_Config_Vision_JogMove_Step.Checked)
                return;

            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDistance = Convert.ToDouble(textBox_Config_Vision_JogMove_StepSize.Text);
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
                    if (radioButton_Config_Vision_Move_MoveMode_Fine.Checked)
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
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_BDS_Y_Neg_MouseDown(object sender, MouseEventArgs e)
        {
            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDirection = -1.0;

            if (!Equipment.Machine_LaserType_CO2)
                return;

            if (!this.radioButton_Config_BDS_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_Config_BDS_Move_MoveMode_Fine.Checked)
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Bds.nAxis.MASK_Y].Jog_Speed_Fine;
                        lfAccDec = Equipment.stAxisParam[(int)Bds.nAxis.MASK_Y].Common_Acceleration_Fine;
                    }
                    else
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Bds.nAxis.MASK_Y].Jog_Speed_Coarse;
                        lfAccDec = Equipment.stAxisParam[(int)Bds.nAxis.MASK_Y].Common_Acceleration_Coarse;
                    }

                    //  방향 설정
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Negative direction : Using - (minus) velocity

                    bds.MC_Func.MC_JogMove((int)Bds.nAxis.MASK_Y, lfVelocity * dDirection, lfAccDec, lfAccDec);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_BDS_Y_Pos_MouseDown(object sender, MouseEventArgs e)
        {
            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDirection = 1.0;

            if (!Equipment.Machine_LaserType_CO2)
                return;

            if (!this.radioButton_Config_BDS_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_Config_BDS_Move_MoveMode_Fine.Checked)
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Bds.nAxis.MASK_Y].Jog_Speed_Fine;
                        lfAccDec = Equipment.stAxisParam[(int)Bds.nAxis.MASK_Y].Common_Acceleration_Fine;
                    }
                    else
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Bds.nAxis.MASK_Y].Jog_Speed_Coarse;
                        lfAccDec = Equipment.stAxisParam[(int)Bds.nAxis.MASK_Y].Common_Acceleration_Coarse;
                    }

                    //  방향 설정
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Positive direction : Using + (plus) velocity

                    bds.MC_Func.MC_JogMove((int)Bds.nAxis.MASK_Y, lfVelocity * dDirection, lfAccDec, lfAccDec);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_LDUL_TeachingPositions_Save_Click(object sender, EventArgs e)
        {
            //  선택된 축에 대한 데이터 갖다 넣기
            int m_nIndex = listBox_Config_LDUL_TeachingPositions.SelectedIndex;

            if (m_nIndex >= 0)
            {
                if (m_nIndex <= 11)                 //  Loader
                {
                    loader.stLDULTeachingPos[m_nIndex].LD_Transfer_X = Convert.ToDouble(textBox_Config_LDUL_TeachingPos_TransferX.Text);
                    loader.stLDULTeachingPos[m_nIndex].LD_Transfer_Z = Convert.ToDouble(textBox_Config_LDUL_TeachingPos_TransferZ.Text);
                    loader.stLDULTeachingPos[m_nIndex].LD_Stacker_Z0 = Convert.ToDouble(textBox_Config_LDUL_TeachingPos_RPortZ.Text);
                    loader.stLDULTeachingPos[m_nIndex].LD_Stacker_Z1 = Convert.ToDouble(textBox_Config_LDUL_TeachingPos_LPortZ.Text);
                    loader.stLDULTeachingPos[m_nIndex].MAligner_X = Convert.ToDouble(textBox_Config_LDUL_TeachingPos_MAlignerX.Text);
                    loader.stLDULTeachingPos[m_nIndex].MAligner_Y = Convert.ToDouble(textBox_Config_LDUL_TeachingPos_MAlignerY.Text);
                }
                else                                //  Unloader
                {
                    loader.stLDULTeachingPos[m_nIndex].UL_Transfer_X = Convert.ToDouble(textBox_Config_LDUL_TeachingPos_TransferX.Text);
                    loader.stLDULTeachingPos[m_nIndex].UL_Transfer_Z = Convert.ToDouble(textBox_Config_LDUL_TeachingPos_TransferZ.Text);
                    loader.stLDULTeachingPos[m_nIndex].UL_Stacker_Z0 = Convert.ToDouble(textBox_Config_LDUL_TeachingPos_RPortZ.Text);
                    loader.stLDULTeachingPos[m_nIndex].UL_Stacker_Z1 = Convert.ToDouble(textBox_Config_LDUL_TeachingPos_LPortZ.Text);
                }
            }

            //  리스트 전체 저장
            loader.Teaching_Position_Save();
            loader.Move_Properties_Save();

            MessageBox.Show("저장하였습니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void listBox_Config_LDUL_TeachingPositions_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  Teaching 항목 선택에 따른 Position 활성/비활성
            int m_nIndex = listBox_Config_LDUL_TeachingPositions.SelectedIndex;

            if (m_nIndex >= 0)
            {
                if (m_nIndex <= 11)                 //  Loader
                {
                    //  Jog 모드 변경
                    radioButton_Config_ActiveUnit_Loader.Checked = true;

                    //  활성/비활성
                    textBox_Config_LDUL_TeachingPos_TransferX.Enabled = true;
                    textBox_Config_LDUL_TeachingPos_TransferZ.Enabled = true;
                    textBox_Config_LDUL_TeachingPos_RPortZ.Enabled = true;
                    textBox_Config_LDUL_TeachingPos_LPortZ.Enabled = true;
                    textBox_Config_LDUL_TeachingPos_MAlignerX.Enabled = true;
                    textBox_Config_LDUL_TeachingPos_MAlignerY.Enabled = true;
                    button_KeypadCall_Config_LDUL_TeachingPos_TransferX.Enabled = true;
                    button_KeypadCall_Config_LDUL_TeachingPos_TransferZ.Enabled = true;
                    button_KeypadCall_Config_LDUL_TeachingPos_RPortZ.Enabled = true;
                    button_KeypadCall_Config_LDUL_TeachingPos_LPortZ.Enabled = true;
                    button_KeypadCall_Config_LDUL_TeachingPos_MAlignerX.Enabled = true;
                    button_KeypadCall_Config_LDUL_TeachingPos_MAlignerY.Enabled = true;


                    //  데이터 표시
                    textBox_Config_LDUL_TeachingPos_TransferX.Text = loader.stLDULTeachingPos[m_nIndex].LD_Transfer_X.ToString();
                    textBox_Config_LDUL_TeachingPos_TransferZ.Text = loader.stLDULTeachingPos[m_nIndex].LD_Transfer_Z.ToString();
                    textBox_Config_LDUL_TeachingPos_RPortZ.Text = loader.stLDULTeachingPos[m_nIndex].LD_Stacker_Z0.ToString();
                    textBox_Config_LDUL_TeachingPos_LPortZ.Text = loader.stLDULTeachingPos[m_nIndex].LD_Stacker_Z1.ToString();
                    textBox_Config_LDUL_TeachingPos_MAlignerX.Text = loader.stLDULTeachingPos[m_nIndex].MAligner_X.ToString();
                    textBox_Config_LDUL_TeachingPos_MAlignerY.Text = loader.stLDULTeachingPos[m_nIndex].MAligner_Y.ToString();
                }
                else                                //  Unloader
                {
                    //  Jog 모드 변경
                    radioButton_Config_ActiveUnit_Unloader.Checked = true;

                    //  활성/비활성
                    textBox_Config_LDUL_TeachingPos_TransferX.Enabled = true;
                    textBox_Config_LDUL_TeachingPos_TransferZ.Enabled = true;
                    textBox_Config_LDUL_TeachingPos_RPortZ.Enabled = true;
                    textBox_Config_LDUL_TeachingPos_LPortZ.Enabled = true;
                    textBox_Config_LDUL_TeachingPos_MAlignerX.Enabled = false;
                    textBox_Config_LDUL_TeachingPos_MAlignerY.Enabled = false;
                    button_KeypadCall_Config_LDUL_TeachingPos_TransferX.Enabled = true;
                    button_KeypadCall_Config_LDUL_TeachingPos_TransferZ.Enabled = true;
                    button_KeypadCall_Config_LDUL_TeachingPos_RPortZ.Enabled = true;
                    button_KeypadCall_Config_LDUL_TeachingPos_LPortZ.Enabled = true;
                    button_KeypadCall_Config_LDUL_TeachingPos_MAlignerX.Enabled = false;
                    button_KeypadCall_Config_LDUL_TeachingPos_MAlignerY.Enabled = false;

                    //  데이터 표시
                    textBox_Config_LDUL_TeachingPos_TransferX.Text = loader.stLDULTeachingPos[m_nIndex].UL_Transfer_X.ToString();
                    textBox_Config_LDUL_TeachingPos_TransferZ.Text = loader.stLDULTeachingPos[m_nIndex].UL_Transfer_Z.ToString();
                    textBox_Config_LDUL_TeachingPos_RPortZ.Text = loader.stLDULTeachingPos[m_nIndex].UL_Stacker_Z0.ToString();
                    textBox_Config_LDUL_TeachingPos_LPortZ.Text = loader.stLDULTeachingPos[m_nIndex].UL_Stacker_Z1.ToString();
                    textBox_Config_LDUL_TeachingPos_MAlignerX.Text = "---";
                    textBox_Config_LDUL_TeachingPos_MAlignerY.Text = "---";
                }
            }
        }

        private void button_Config_WorkStage_TeachingPositions_Save_Click(object sender, EventArgs e)
        {
            //workStage.m_dPowerMeterBDS_Value = 123.0;
            //return;


            //  선택된 축에 대한 데이터 갖다 넣기
            int m_nIndex = listBox_Config_WorkStage_TeachingPositions.SelectedIndex;

            if (m_nIndex >= 0)
            {
                workStage.stWorkStageTeachingPos[m_nIndex].Stage_X = Convert.ToDouble(textBox_Config_WorkStage_TeachingPos_StageX.Text);
                workStage.stWorkStageTeachingPos[m_nIndex].Stage_Y = Convert.ToDouble(textBox_Config_WorkStage_TeachingPos_StageY.Text);
            }

            //  리스트 전체 저장
            workStage.Teaching_Position_Save();
            workStage.Move_Properties_Save();
        }

        private void listBox_Config_WorkStage_TeachingPositions_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  Teaching 항목 선택에 따른 Position
            int m_nIndex = listBox_Config_WorkStage_TeachingPositions.SelectedIndex;

            if (m_nIndex >= 0)
            {
                //  데이터 표시
                textBox_Config_WorkStage_TeachingPos_StageX.Text = workStage.stWorkStageTeachingPos[m_nIndex].Stage_X.ToString();
                textBox_Config_WorkStage_TeachingPos_StageY.Text = workStage.stWorkStageTeachingPos[m_nIndex].Stage_Y.ToString();
            }
        }

        private void button_Config_Vision_TeachingPositions_Save_Click(object sender, EventArgs e)
        {
            //  선택된 축에 대한 데이터 갖다 넣기
            int m_nIndex = listBox_Config_Vision_TeachingPositions.SelectedIndex;

            if (m_nIndex >= 0)
            {
                vision.stVisionTeachingPos[m_nIndex].Vision_Z = Convert.ToDouble(textBox_Config_Vision_TeachingPos_VisionZ.Text);
            }

            //  리스트 전체 저장
            vision.Teaching_Position_Save();
            vision.Move_Properties_Save();
        }

        private void listBox_Config_Vision_TeachingPositions_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  Teaching 항목 선택에 따른 Position
            int m_nIndex = listBox_Config_Vision_TeachingPositions.SelectedIndex;

            if (m_nIndex >= 0)
            {
                //  데이터 표시
                textBox_Config_Vision_TeachingPos_VisionZ.Text = vision.stVisionTeachingPos[m_nIndex].Vision_Z.ToString();
            }
        }

        private void button_Config_BDS_TeachingPositions_Save_Click(object sender, EventArgs e)
        {
            //  선택된 축에 대한 데이터 갖다 넣기
            int m_nIndex = listBox_Config_BDS_TeachingPositions.SelectedIndex;

            if (m_nIndex >= 0)
            {
                bds.stBDSTeachingPos[m_nIndex].Mask_Y = Convert.ToDouble(textBox_Config_BDS_TeachingPos_MaskY.Text);
            }

            //  리스트 전체 저장
            bds.Teaching_Position_Save();
            bds.Move_Properties_Save();
        }

        private void listBox_Config_BDS_TeachingPositions_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  Teaching 항목 선택에 따른 Position
            int m_nIndex = listBox_Config_BDS_TeachingPositions.SelectedIndex;

            if (m_nIndex >= 0)
            {
                //  데이터 표시
                textBox_Config_BDS_TeachingPos_MaskY.Text = bds.stBDSTeachingPos[m_nIndex].Mask_Y.ToString();
            }
        }

        private void button_Config_LDUL_GetCurrentPos_ToTeachingPos_Click(object sender, EventArgs e)
        {
            //  현재 위치값을 티칭 위치값으로 설정 (저장은 아님)
            //  선택 티칭 위치에 따라 세분화 할 필요가 있음. (임시로 로더, 언로더 단위로 값을 설정하도록 한다.)
            int m_nIndex = listBox_Config_LDUL_TeachingPositions.SelectedIndex;

            if (m_nIndex >= 0)
            {
                if (m_nIndex <= 11)                 //  Loader
                {
                    textBox_Config_LDUL_TeachingPos_TransferX.Text = string.Format("{0:0.000}", loader.MC_Func.MC_GetEncPos((int)Loader.nAxis.TR_X).ToString());
                    textBox_Config_LDUL_TeachingPos_TransferZ.Text = string.Format("{0:0.000}", loader.MC_Func.MC_GetEncPos((int)Loader.nAxis.TR_Z).ToString());
                    textBox_Config_LDUL_TeachingPos_RPortZ.Text = string.Format("{0:0.000}", loader.MC_Func.MC_GetEncPos((int)Loader.nAxis.Z0).ToString());
                    textBox_Config_LDUL_TeachingPos_LPortZ.Text = string.Format("{0:0.000}", loader.MC_Func.MC_GetEncPos((int)Loader.nAxis.Z1).ToString());
                    textBox_Config_LDUL_TeachingPos_MAlignerX.Text = string.Format("{0:0.000}", loader.MC_Func.MC_GetEncPos((int)Loader.nAxis.ALN_X).ToString());
                    textBox_Config_LDUL_TeachingPos_MAlignerY.Text = string.Format("{0:0.000}", loader.MC_Func.MC_GetEncPos((int)Loader.nAxis.ALN_Y).ToString());
                }
                else                                //  Unloader
                {
                    textBox_Config_LDUL_TeachingPos_TransferX.Text = string.Format("{0:0.000}", unloader.MC_Func.MC_GetEncPos((int)Unloader.nAxis.TR_X).ToString());
                    textBox_Config_LDUL_TeachingPos_TransferZ.Text = string.Format("{0:0.000}", unloader.MC_Func.MC_GetEncPos((int)Unloader.nAxis.TR_Z).ToString());
                    textBox_Config_LDUL_TeachingPos_RPortZ.Text = string.Format("{0:0.000}", unloader.MC_Func.MC_GetEncPos((int)Unloader.nAxis.Z0).ToString());
                    textBox_Config_LDUL_TeachingPos_LPortZ.Text = string.Format("{0:0.000}", unloader.MC_Func.MC_GetEncPos((int)Unloader.nAxis.Z1).ToString());
                }
            }
            else
            {
                MessageBox.Show("먼저 티칭 위치를 선택해야 합니다.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_Config_WorkStage_GetCurrentPos_ToTeachingPos_Click(object sender, EventArgs e)
        {
            //  현재 위치값을 티칭 위치값으로 설정 (저장은 아님)
            int m_nIndex = listBox_Config_WorkStage_TeachingPositions.SelectedIndex;

            if (m_nIndex >= 0)
            {
                textBox_Config_WorkStage_TeachingPos_StageX.Text = string.Format("{0:0.000}", workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X).ToString());
                textBox_Config_WorkStage_TeachingPos_StageY.Text = string.Format("{0:0.000}", workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y).ToString());
            }
            else
            {
                MessageBox.Show("먼저 티칭 위치를 선택해야 합니다.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_Config_Vision_GetCurrentPos_ToTeachingPos_Click(object sender, EventArgs e)
        {
            //  현재 위치값을 티칭 위치값으로 설정 (저장은 아님)
            int m_nIndex = listBox_Config_Vision_TeachingPositions.SelectedIndex;

            if (m_nIndex >= 0)
            {
                textBox_Config_Vision_TeachingPos_VisionZ.Text = string.Format("{0:0.000}", vision.MC_Func.MC_GetEncPos((int)Vision.nAxis.Z).ToString());
            }
            else
            {
                MessageBox.Show("먼저 티칭 위치를 선택해야 합니다.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_Config_BDS_GetCurrentPos_ToTeachingPos_Click(object sender, EventArgs e)
        {
            if (!Equipment.Machine_LaserType_CO2)
            {
                MessageBox.Show ("UV 모델에는 해당 기능이 없습니다.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            //  현재 위치값을 티칭 위치값으로 설정 (저장은 아님)
            int m_nIndex = listBox_Config_BDS_TeachingPositions.SelectedIndex;

            if (m_nIndex >= 0)
            {
                textBox_Config_BDS_TeachingPos_MaskY.Text = string.Format("{0:0.000}", bds.MC_Func.MC_GetEncPos((int)Bds.nAxis.MASK_Y).ToString());
            }
            else
            {
                MessageBox.Show("먼저 티칭 위치를 선택해야 합니다.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_Config_Vision_ImageDisplay_Show_Click(object sender, EventArgs e)
        {
            //  Vision 창을 연다.

            Equipment.m_bVisionFormOpenMode_ScannerFineCamOffsetChange = false;

            if (m_formVisionPopup == null)
            {
                m_formVisionPopup.CreateSiriusEditor();
            }

            foreach (Form openForm in System.Windows.Forms.Application.OpenForms)
            {
                if (openForm.Name == m_formVisionPopup.Name)
                {
                    //  GetDrillingData() 를 수행했으면, Socket 개수를 얼라인 테스트 쪽에 세팅한다.
                    m_formVisionPopup.Socket_List_Set();

                    openForm.BringToFront();
                    openForm.Show();
                    return;
                }
            }

            //  GetDrillingData() 를 수행했으면, Socket 개수를 얼라인 테스트 쪽에 세팅한다.
            m_formVisionPopup.Socket_List_Set();
            m_formVisionPopup.Show();
        }

        private void button_Config_LDUL_Movement_TransferX_SetZero_Click(object sender, EventArgs e)
        {
            if (radioButton_Config_ActiveUnit_Loader.Checked)               //  Loader 활성화
            {
                m_dMotionSetZeroPos_Loader[(int)LoaderParameter.MotionKey.TR_X] = loader.MC_Func.MC_GetEncPos((int)Loader.nAxis.TR_X);
            }
            else if (radioButton_Config_ActiveUnit_Unloader.Checked)        //  Unloader 활성화
            {
                m_dMotionSetZeroPos_Unloader[(int)UnloaderParameter.MotionKey.TR_X] = unloader.MC_Func.MC_GetEncPos((int)Unloader.nAxis.TR_X);
            }
        }

        private void button_Config_LDUL_Movement_TransferZ_SetZero_Click(object sender, EventArgs e)
        {
            if (radioButton_Config_ActiveUnit_Loader.Checked)               //  Loader 활성화
            {
                m_dMotionSetZeroPos_Loader[(int)LoaderParameter.MotionKey.TR_Z] = loader.MC_Func.MC_GetEncPos((int)Loader.nAxis.TR_Z);
            }
            else if (radioButton_Config_ActiveUnit_Unloader.Checked)        //  Unloader 활성화
            {
                m_dMotionSetZeroPos_Unloader[(int)UnloaderParameter.MotionKey.TR_Z] = unloader.MC_Func.MC_GetEncPos((int)Unloader.nAxis.TR_Z);
            }
        }

        private void button_Config_LDUL_Movement_StackerZ0_SetZero_Click(object sender, EventArgs e)
        {
            if (radioButton_Config_ActiveUnit_Loader.Checked)               //  Loader 활성화
            {
                m_dMotionSetZeroPos_Loader[(int)LoaderParameter.MotionKey.Z0] = loader.MC_Func.MC_GetEncPos((int)Loader.nAxis.Z0);
            }
            else if (radioButton_Config_ActiveUnit_Unloader.Checked)        //  Unloader 활성화
            {
                m_dMotionSetZeroPos_Unloader[(int)UnloaderParameter.MotionKey.Z0] = unloader.MC_Func.MC_GetEncPos((int)Unloader.nAxis.Z0);
            }
        }

        private void button_Config_LDUL_Movement_StackerZ1_SetZero_Click(object sender, EventArgs e)
        {
            if (radioButton_Config_ActiveUnit_Loader.Checked)               //  Loader 활성화
            {
                m_dMotionSetZeroPos_Loader[(int)LoaderParameter.MotionKey.Z1] = loader.MC_Func.MC_GetEncPos((int)Loader.nAxis.Z1);
            }
            else if (radioButton_Config_ActiveUnit_Unloader.Checked)        //  Unloader 활성화
            {
                m_dMotionSetZeroPos_Unloader[(int)UnloaderParameter.MotionKey.Z1] = unloader.MC_Func.MC_GetEncPos((int)Unloader.nAxis.Z0);
            }
        }

        private void button_Config_LDUL_Movement_AlignX_SetZero_Click(object sender, EventArgs e)
        {
            if (radioButton_Config_ActiveUnit_Loader.Checked)               //  Loader 활성화
            {
                m_dMotionSetZeroPos_Loader[(int)LoaderParameter.MotionKey.ALN_X] = loader.MC_Func.MC_GetEncPos((int)Loader.nAxis.ALN_X);
            }
            else
            {
                MessageBox.Show("Unloader는 Align X 축을 지원하지 않습니다.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_Config_LDUL_Movement_AlignY_SetZero_Click(object sender, EventArgs e)
        {
            if (radioButton_Config_ActiveUnit_Loader.Checked)               //  Loader 활성화
            {
                m_dMotionSetZeroPos_Loader[(int)LoaderParameter.MotionKey.ALN_Y] = loader.MC_Func.MC_GetEncPos((int)Loader.nAxis.ALN_Y);
            }
            else if (radioButton_Config_ActiveUnit_Unloader.Checked)        //  Unloader 활성화
            {
                MessageBox.Show("Unloader는 Align Y 축을 지원하지 않습니다.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_Config_WorkStage_Movement_StageX_SetZero_Click(object sender, EventArgs e)
        {
            m_dMotionSetZeroPos_WorkStage[(int)WorkStageParameter.MotionKey.X] = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X);
        }

        private void button_Config_WorkStage_Movement_StageY_SetZero_Click(object sender, EventArgs e)
        {
            m_dMotionSetZeroPos_WorkStage[(int)WorkStageParameter.MotionKey.Y] = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y);
        }

        private void button_Config_WorkStage_Movement_ScannerZ_SetZero_Click(object sender, EventArgs e)
        {
            m_dMotionSetZeroPos_WorkStage[(int)WorkStageParameter.MotionKey.Z] = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Z);
        }

        private void button_Config_Vision_Movement_StageX_SetZero_Click(object sender, EventArgs e)
        {
            //m_dMotionSetZeroPos_Vision[(int)VisionParameter.MotionKey.X] = 0.0;
            m_dMotionSetZeroPos_WorkStage[(int)WorkStageParameter.MotionKey.X] = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X);
        }

        private void button_Config_Vision_Movement_StageY_SetZero_Click(object sender, EventArgs e)
        {
            //m_dMotionSetZeroPos_Vision[(int)VisionParameter.MotionKey.Y] = 0.0;
            m_dMotionSetZeroPos_WorkStage[(int)WorkStageParameter.MotionKey.Y] = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y);
        }

        private void button_Config_Vision_Movement_VisionZ_SetZero_Click(object sender, EventArgs e)
        {
            //m_dMotionSetZeroPos_Vision[(int)VisionParameter.MotionKey.Z] = 0.0;
            m_dMotionSetZeroPos_WorkStage[(int)WorkStageParameter.MotionKey.Z] = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Z);
        }

        private void button_Config_BDS_Movement_MaskY_SetZero_Click(object sender, EventArgs e)
        {
            m_dMotionSetZeroPos_Bds[(int)BdsParameter.MotionKey.MASK_Y] = 0.0;
        }

        private void button_Config_BDS_Axis_MouseUp(object sender, MouseEventArgs e)
        {
            if (!Equipment.Machine_LaserType_CO2)
                return;

            if (!this.radioButton_Config_BDS_JogMove_Continuous.Checked)
                return;

            if (Equipment.AjinBoard_Opened)
            {
                bds.MC_Func.MC_JogStop((int)Bds.nAxis.MASK_Y);
            }
        }

        private void button_Config_BDS_Y_Neg_Click(object sender, EventArgs e)
        {
            if (!Equipment.Machine_LaserType_CO2)
                return;

            if (!this.radioButton_Config_BDS_JogMove_Step.Checked)
                return;

            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDistance = Convert.ToDouble(textBox_Config_BDS_JogMove_StepSize.Text);
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
                    if (radioButton_Config_BDS_Move_MoveMode_Fine.Checked)
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Bds.nAxis.MASK_Y].Jog_Speed_Fine;
                        lfAccDec = Equipment.stAxisParam[(int)Bds.nAxis.MASK_Y].Common_Acceleration_Fine;
                    }
                    else
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Bds.nAxis.MASK_Y].Jog_Speed_Coarse;
                        lfAccDec = Equipment.stAxisParam[(int)Bds.nAxis.MASK_Y].Common_Acceleration_Coarse;
                    }

                    //  속도는 양수로
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                    bds.MC_Func.MC_MoveRelPosition((int)Bds.nAxis.MASK_Y, dDistance * (double)nDirection, lfVelocity, lfAccDec, lfAccDec);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Config_BDS_Y_Pos_Click(object sender, EventArgs e)
        {
            if (!Equipment.Machine_LaserType_CO2)
                return;

            if (!this.radioButton_Config_BDS_JogMove_Step.Checked)
                return;

            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;
            double dDistance = Convert.ToDouble(textBox_Config_BDS_JogMove_StepSize.Text);
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
                    if (radioButton_Config_BDS_Move_MoveMode_Fine.Checked)
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Bds.nAxis.MASK_Y].Jog_Speed_Fine;
                        lfAccDec = Equipment.stAxisParam[(int)Bds.nAxis.MASK_Y].Common_Acceleration_Fine;
                    }
                    else
                    {
                        lfVelocity = Equipment.stAxisParam[(int)Bds.nAxis.MASK_Y].Jog_Speed_Coarse;
                        lfAccDec = Equipment.stAxisParam[(int)Bds.nAxis.MASK_Y].Common_Acceleration_Coarse;
                    }

                    //  속도는 양수로
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                    bds.MC_Func.MC_MoveRelPosition((int)Bds.nAxis.MASK_Y, dDistance * (double)nDirection, lfVelocity, lfAccDec, lfAccDec);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void button_Test_LDStacker0_PickupWaitPos_Cyc_Click(object sender, EventArgs e)
        {
            //  Loader R-Port, 작업 대기위치 이동

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (loader.m_nStacker0_ModulePickupWaitingPos_Step == (int)Loader.StackerModulePickupWaitingPos_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Loader R-Port, 작업 대기위치로 이동하시겠습니까?"))
                    return;

                loader.MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.Z0, 2000);

                //  Seq. Test
                Equipment.SeqTestMode = true;

                loader.m_nStacker0_ModulePickupWaitingPos_Step = (int)Loader.StackerModulePickupWaitingPos_Step.Start;

                //  Loader 실행 타이머
                loader.m_btimer_LoaderWork_Stop = false;
                loader.timer_LoaderWork.Enabled = true;
            }
            else
            {
                try
                {
                    var mb = new MessageBoxYesNo();
                    if (DialogResult.Yes != mb.ShowDialog("Question ?", "Loader R-Port, 작업 대기위치 이동 작업을 중지 하시겠습니까?"))
                        return;

                    //  Loader 실행 타이머
                    //loader.timer_LoaderWork.Enabled = false;
                    //loader.m_btimer_LoaderWork_Stop = true;

                    //  Seq. Test
                    Equipment.SeqTestMode = false;

                    loader.m_nStacker0_ModulePickupWaitingPos_Step = (int)Loader.StackerModulePickupWaitingPos_Step.None;

                    if (Equipment.AjinBoard_Opened)
                    {
                        loader.MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.Z0, 2000);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        private void button_Test_LDStacker1_PickupWaitPos_Cyc_Click(object sender, EventArgs e)
        {
            //  Loader L-Port, 작업 대기위치 이동

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (loader.m_nStacker1_ModulePickupWaitingPos_Step == (int)Loader.StackerModulePickupWaitingPos_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Loader L-Port, 작업 대기위치로 이동하시겠습니까?"))
                    return;

                loader.MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.Z1, 2000);

                //  Seq. Test
                Equipment.SeqTestMode = true;

                loader.m_nStacker1_ModulePickupWaitingPos_Step = (int)Loader.StackerModulePickupWaitingPos_Step.Start;

                //  Loader 실행 타이머
                loader.m_btimer_LoaderWork_Stop = false;
                loader.timer_LoaderWork.Enabled = true;
            }
            else
            {
                try
                {
                    var mb = new MessageBoxYesNo();
                    if (DialogResult.Yes != mb.ShowDialog("Question ?", "Loader L-Port, 작업 대기위치 이동 작업을 중지 하시겠습니까?"))
                        return;

                    //  Loader 실행 타이머
                    //loader.timer_LoaderWork.Enabled = false;
                    //loader.m_btimer_LoaderWork_Stop = true;

                    //  Seq. Test
                    Equipment.SeqTestMode = false;

                    loader.m_nStacker1_ModulePickupWaitingPos_Step = (int)Loader.StackerModulePickupWaitingPos_Step.None;

                    if (Equipment.AjinBoard_Opened)
                    {
                        loader.MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.Z1, 2000);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        private void button_Test_ULStacker0_PickupWaitPos_Cyc_Click(object sender, EventArgs e)
        {
            //  Unloader R-Port, 작업 대기위치 이동

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (unloader.m_nStacker0_ModulePutdownWaitingPos_Step == (int)Unloader.StackerModulePutdownWaitingPos_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Unloader R-Port, 작업 대기위치로 이동하시겠습니까?"))
                    return;

                unloader.MC_Func.MC_MotorStop((int)UnloaderParameter.AxisAjinEnum.Z0, 2000);

                //  Seq. Test
                Equipment.SeqTestMode = true;

                unloader.m_nStacker0_ModulePutdownWaitingPos_Step = (int)Unloader.StackerModulePutdownWaitingPos_Step.Start;

                //  Unloader 실행 타이머
                unloader.m_btimer_UnloaderWork_Stop = false;
                unloader.timer_UnloaderWork.Enabled = true;
            }
            else
            {
                try
                {
                    var mb = new MessageBoxYesNo();
                    if (DialogResult.Yes != mb.ShowDialog("Question ?", "Unloader R-Port, 작업 대기위치 이동 작업을 중지 하시겠습니까?"))
                        return;

                    //  Unloader 실행 타이머
                    //unloader.timer_UnloaderWork.Enabled = false;
                    //unloader.m_btimer_UnloaderWork_Stop = true;

                    //  Seq. Test
                    Equipment.SeqTestMode = false;

                    unloader.m_nStacker0_ModulePutdownWaitingPos_Step = (int)Unloader.StackerModulePutdownWaitingPos_Step.None;

                    if (Equipment.AjinBoard_Opened)
                    {
                        unloader.MC_Func.MC_MotorStop((int)UnloaderParameter.AxisAjinEnum.Z0, 2000);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        private void button_Test_ULStacker1_PickupWaitPos_Cyc_Click(object sender, EventArgs e)
        {
            //  Unloader L-Port, 작업 대기위치 이동

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (unloader.m_nStacker1_ModulePutdownWaitingPos_Step == (int)Unloader.StackerModulePutdownWaitingPos_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Unloader L-Port, 작업 대기위치로 이동하시겠습니까?"))
                    return;

                unloader.MC_Func.MC_MotorStop((int)UnloaderParameter.AxisAjinEnum.Z1, 2000);

                //  Seq. Test
                Equipment.SeqTestMode = true;

                unloader.m_nStacker1_ModulePutdownWaitingPos_Step = (int)Unloader.StackerModulePutdownWaitingPos_Step.Start;

                //  Unloader 실행 타이머
                unloader.m_btimer_UnloaderWork_Stop = false;
                unloader.timer_UnloaderWork.Enabled = true;
            }
            else
            {
                try
                {
                    var mb = new MessageBoxYesNo();
                    if (DialogResult.Yes != mb.ShowDialog("Question ?", "Unloader L-Port, 작업 대기위치 이동 작업을 중지 하시겠습니까?"))
                        return;

                    //  Unloader 실행 타이머
                    //unloader.timer_UnloaderWork.Enabled = false;
                    //unloader.m_btimer_UnloaderWork_Stop = true;

                    //  Seq. Test
                    Equipment.SeqTestMode = false;

                    unloader.m_nStacker1_ModulePutdownWaitingPos_Step = (int)Unloader.StackerModulePutdownWaitingPos_Step.None;

                    if (Equipment.AjinBoard_Opened)
                    {
                        unloader.MC_Func.MC_MotorStop((int)UnloaderParameter.AxisAjinEnum.Z1, 2000);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        private void button_Config_WorkStage_StageCenter_To_ScannerCenter_Click(object sender, EventArgs e)
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
            if (radioButton_Config_WorkStage_Move_MoveMode_Fine.Checked)
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

        private void button_Config_WorkStage_CurrentScannerPos_To_FineCamPos_Click(object sender, EventArgs e)
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

            ////  맵 데이터 변경
            //workStage.MapData_Apply((int)WorkStage.nMapData_Type.MapData_Stage_FineCam);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //  맵 데이터 변경 (기준위치 : Scanner)
            //  기준위치로 보낼 때, 맵데이터를 변경한 후 보낸다.
            //  그 외에는, 위치로 보낸 후 맵데이터를 변경한다.
            workStage.MapData_Apply((int)WorkStage.nMapData_Type.MapData_Stage_FineCam);
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            //  속도 설정
            if (radioButton_Config_WorkStage_Move_MoveMode_Fine.Checked)
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


            //  X, Y 축 모션이 정지했을 때 맵 데이터를 바꿔준다.
            //do
            //{
            //    //  모션 정지할때 까지 대기

            //} while (workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.X) == false || workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Y) == false);


        }

        private void button_Config_WorkStage_CurrentFineCamPos_To_ScannerPos_Click(object sender, EventArgs e)
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


            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //  맵 데이터 변경 (기준위치 : Scanner)
            //  기준위치로 보낼 때, 맵데이터를 변경한 후 보낸다.
            //  그 외에는, 위치로 보낸 후 맵데이터를 변경한다.
            workStage.MapData_Apply((int)WorkStage.nMapData_Type.MapData_Stage_Scanner);
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            //  Target 위치 계산
            lfTargetX = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X) + Equipment.stOffsetDistance.FromScannerToFineCam.X;
            lfTargetY = workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y) + Equipment.stOffsetDistance.FromScannerToFineCam.Y;

            //  속도 설정
            if (radioButton_Config_WorkStage_Move_MoveMode_Fine.Checked)
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

        public void Temp_Position_Save()
        {
            string strTemp = "";

            string strFIle = "";
            strFIle = ConfigManager.GetTeachingDataPath() + "\\WorkStage_TeachingPosition.ini";

            if (File.Exists(strFIle) == false)
            {
                File.Create(strFIle);

                MessageBox.Show("WorkStage Teaching Position 파일을 생성하였습니다. 다시 시도하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //  Temp Position 저장

            //  Temp1 Stage X
            NativeMethods.WritePrivateProfileString("TempPos1", "StageX", textBox_Config_WorkStage_TempPos1_StageX.Text.ToString(), strFIle);
            //  Temp1 Stage Y
            NativeMethods.WritePrivateProfileString("TempPos1", "StageY", textBox_Config_WorkStage_TempPos1_StageY.Text.ToString(), strFIle);

            //  Temp2 Stage X
            NativeMethods.WritePrivateProfileString("TempPos2", "StageX", textBox_Config_WorkStage_TempPos2_StageX.Text.ToString(), strFIle);
            //  Temp2 Stage Y
            NativeMethods.WritePrivateProfileString("TempPos2", "StageY", textBox_Config_WorkStage_TempPos2_StageY.Text.ToString(), strFIle);

            //  Temp3 Stage X
            NativeMethods.WritePrivateProfileString("TempPos3", "StageX", textBox_Config_WorkStage_TempPos3_StageX.Text.ToString(), strFIle);
            //  Temp3 Stage Y
            NativeMethods.WritePrivateProfileString("TempPos3", "StageY", textBox_Config_WorkStage_TempPos3_StageY.Text.ToString(), strFIle);
        }

        public bool Temp_Position_Load()
        {
            string strTemp = "";

            bool m_bRet = true;
            string strFIle = "";
            StringBuilder temp = new StringBuilder(255);

            strFIle = ConfigManager.GetTeachingDataPath() + "\\WorkStage_TeachingPosition.ini";

            if (File.Exists(strFIle) == false)
            {
                MessageBox.Show("WorkStage Teaching Position 파일이 없습니다.\r\n\r\n[Default 값으로 설정됩니다.]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return false;
            }

            //  Temp Position 데이터 로드

            //  Temp1 Stage X
            NativeMethods.GetPrivateProfileString("TempPos1", "StageX", "0", temp, 255, strFIle);
            textBox_Config_WorkStage_TempPos1_StageX.Text = temp.ToString();
            //  Temp1 Stage Y
            NativeMethods.GetPrivateProfileString("TempPos1", "StageY", "0", temp, 255, strFIle);
            textBox_Config_WorkStage_TempPos1_StageY.Text = temp.ToString();

            //  Temp2 Stage X
            NativeMethods.GetPrivateProfileString("TempPos2", "StageX", "0", temp, 255, strFIle);
            textBox_Config_WorkStage_TempPos2_StageX.Text = temp.ToString();
            //  Temp2 Stage Y
            NativeMethods.GetPrivateProfileString("TempPos2", "StageY", "0", temp, 255, strFIle);
            textBox_Config_WorkStage_TempPos2_StageY.Text = temp.ToString();

            //  Temp3 Stage X
            NativeMethods.GetPrivateProfileString("TempPos3", "StageX", "0", temp, 255, strFIle);
            textBox_Config_WorkStage_TempPos3_StageX.Text = temp.ToString();
            //  Temp3 Stage Y
            NativeMethods.GetPrivateProfileString("TempPos3", "StageY", "0", temp, 255, strFIle);
            textBox_Config_WorkStage_TempPos3_StageY.Text = temp.ToString();

            return m_bRet;
        }

        private void button_Config_WorkStage_GetCurrentPos_ToTempPos1_Click(object sender, EventArgs e)
        {
            //  Temp1 위치값 가져오기

            textBox_Config_WorkStage_TempPos1_StageX.Text = string.Format("{0:0.000}", workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X).ToString());
            textBox_Config_WorkStage_TempPos1_StageY.Text = string.Format("{0:0.000}", workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y).ToString());

            Temp_Position_Save();
        }

        private void button_Config_WorkStage_GetCurrentPos_ToTempPos2_Click(object sender, EventArgs e)
        {
            //  Temp2 위치값 가져오기

            textBox_Config_WorkStage_TempPos2_StageX.Text = string.Format("{0:0.000}", workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X).ToString());
            textBox_Config_WorkStage_TempPos2_StageY.Text = string.Format("{0:0.000}", workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y).ToString());

            Temp_Position_Save();
        }

        private void button_Config_WorkStage_GetCurrentPos_ToTempPos3_Click(object sender, EventArgs e)
        {
            //  Temp3 위치값 가져오기

            textBox_Config_WorkStage_TempPos3_StageX.Text = string.Format("{0:0.000}", workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X).ToString());
            textBox_Config_WorkStage_TempPos3_StageY.Text = string.Format("{0:0.000}", workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y).ToString());

            Temp_Position_Save();
        }

        private void FormNew_Config_Shown(object sender, EventArgs e)
        {
            Temp_Position_Load();

            //  UV Laser 일 때만 보이는 Laser
            if (Equipment.Machine_LaserType_CO2)
            {
                groupBox_Config_Laser_UVLaser.Visible = false;
            }
            else
            {
                groupBox_Config_Laser_UVLaser.Visible = true;
            }
        }

        private void button_Config_WorkStage_ToTempPos1_Move_Click(object sender, EventArgs e)
        {
            //  Temp1 위치로 이동

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

            if (Convert.ToDouble(textBox_Config_WorkStage_TempPos1_StageX.Text) == 0.0 && Convert.ToDouble(textBox_Config_WorkStage_TempPos1_StageY.Text) == 0.0)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Temp1 위치가 설정되어 있지 않습니다.");
                return;
            }

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Temp1 위치로 이동하시겠습니까?"))
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


            //  Target 위치
            lfTargetX = Convert.ToDouble(textBox_Config_WorkStage_TempPos1_StageX.Text);
            lfTargetY = Convert.ToDouble(textBox_Config_WorkStage_TempPos1_StageY.Text);

            //  속도 설정
            if (radioButton_Config_WorkStage_Move_MoveMode_Fine.Checked)
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

        private void button_Config_WorkStage_ToTempPos2_Move_Click(object sender, EventArgs e)
        {
            //  Temp2 위치로 이동

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

            if (Convert.ToDouble(textBox_Config_WorkStage_TempPos2_StageX.Text) == 0.0 && Convert.ToDouble(textBox_Config_WorkStage_TempPos2_StageY.Text) == 0.0)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Temp2 위치가 설정되어 있지 않습니다.");
                return;
            }

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Temp2 위치로 이동하시겠습니까?"))
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


            //  Target 위치
            lfTargetX = Convert.ToDouble(textBox_Config_WorkStage_TempPos2_StageX.Text);
            lfTargetY = Convert.ToDouble(textBox_Config_WorkStage_TempPos2_StageY.Text);

            //  속도 설정
            if (radioButton_Config_WorkStage_Move_MoveMode_Fine.Checked)
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

        private void button_Config_WorkStage_ToTempPos3_Move_Click(object sender, EventArgs e)
        {
            //  Temp3 위치로 이동

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

            if (Convert.ToDouble(textBox_Config_WorkStage_TempPos3_StageX.Text) == 0.0 && Convert.ToDouble(textBox_Config_WorkStage_TempPos3_StageY.Text) == 0.0)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Temp3 위치가 설정되어 있지 않습니다.");
                return;
            }

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Temp3 위치로 이동하시겠습니까?"))
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


            //  Target 위치
            lfTargetX = Convert.ToDouble(textBox_Config_WorkStage_TempPos3_StageX.Text);
            lfTargetY = Convert.ToDouble(textBox_Config_WorkStage_TempPos3_StageY.Text);

            //  속도 설정
            if (radioButton_Config_WorkStage_Move_MoveMode_Fine.Checked)
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

        private void button_Config_WorkStage_CurrentFineCamPos_To_CoarseCamPos_Click(object sender, EventArgs e)
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
            if (radioButton_Config_WorkStage_Move_MoveMode_Fine.Checked)
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

        private void button_Config_WorkStage_CurrentCoarseCamPos_To_FineCamPos_Click(object sender, EventArgs e)
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
            ///

            //  속도 설정
            if (radioButton_Config_WorkStage_Move_MoveMode_Fine.Checked)
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

        private void Button_Config_WorkStage_TeachingPositions_Move_Click(object sender, EventArgs e)
        {
            //  WorkStage Teaching Position 이동

            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;

            int m_nIndex = listBox_Config_WorkStage_TeachingPositions.SelectedIndex;

            if (m_nIndex < 0)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Teaching Position 이 선택되지 않았습니다.");
                return;
            }

            //if (!workStage.m_bHomeOK)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
            //    return;
            //}

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Stage 를 선택 위치로 보내시겠습니까?\r\n\r\n##  XY 방향 이동 시 Z축 충돌 주의!!!  ##"))
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


            //  속도 설정
            if (radioButton_Config_WorkStage_TeachingPositions_MoveMode_Fine.Checked)
            {
                lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Fine;
                lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Acceleration_Fine;
            }
            else
            {
                lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Coarse;
                lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Acceleration_Coarse;
            }

            //workStage.MC_Func.MC_MovePosition((int)WorkStage.nAxis.X, workStage.stWorkStageTeachingPos[(int)WorkStage_TeachingPosList.STAGE_ProcessingPos].Stage_X,
            //                                lfVelocity, lfAccDec, lfAccDec);
            //workStage.MC_Func.MC_MovePosition((int)WorkStage.nAxis.Y, workStage.stWorkStageTeachingPos[(int)WorkStage_TeachingPosList.STAGE_ProcessingPos].Stage_Y,
            //                                lfVelocity, lfAccDec, lfAccDec);

            xyInterpolatedCoordinate.X = workStage.stWorkStageTeachingPos[m_nIndex].Stage_X;
            xyInterpolatedCoordinate.Y = workStage.stWorkStageTeachingPos[m_nIndex].Stage_Y;
            workStage.MC_Func.MovePosition(xyInterpolatedCoordinate, lfVelocity, lfAccDec, lfAccDec);
        }

        private void button_Config_TabLaser_SystemFaultsClear_Click(object sender, EventArgs e)
        {
            //  Laser Faults Clear

            if (workStage.m_rapidLxLaser_Comm.IsOpen)
            {
                workStage.RapidLxLaserComm_Laser_SystemFaults_Clear();
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Laser Comm 이 연결되어 있지 않습니다.");
            }
        }

        private void button_Config_TabLaser_LaserStart_Click(object sender, EventArgs e)
        {
            //  Laser Start

            if (workStage.m_rapidLxLaser_Comm.IsOpen)
            {
                workStage.RapidLxLaserComm_Laser_StartStop(true);
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Laser Comm 이 연결되어 있지 않습니다.");
            }
        }

        private void button_Config_TabLaser_LaserStop_Click(object sender, EventArgs e)
        {
            //  Laser Stop

            if (workStage.m_rapidLxLaser_Comm.IsOpen)
            {
                workStage.RapidLxLaserComm_Laser_StartStop(false);
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Laser Comm 이 연결되어 있지 않습니다.");
            }
        }

        private void button_Config_TabLaser_PulseModeSet_Click(object sender, EventArgs e)
        {
            //  Pulse Mode Setting

            int m_nPulseMode = comboBox_Config_TabLaser_PulseMode.SelectedIndex;
            if (m_nPulseMode < 0)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Pulse Mode 가 선택되지 않았습니다.");
                return;
            }

            if (workStage.m_rapidLxLaser_Comm.IsOpen)
            {
                workStage.RapidLxLaserComm_Laser_PulseMode_Set(m_nPulseMode);
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Laser Comm 이 연결되어 있지 않습니다.");
            }
        }

        private void button_Config_TabLaser_AmplifierSet_Click(object sender, EventArgs e)
        {
            //  Amplifier RepRate Setting

            double m_dAmplifierRR = Convert.ToDouble(textBox_Config_TabLaser_Amplifier.Text);

            if (workStage.m_rapidLxLaser_Comm.IsOpen)
            {
                workStage.RapidLxLaserComm_Laser_AmplifierRR_Set(m_dAmplifierRR);
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Laser Comm 이 연결되어 있지 않습니다.");
            }
        }

        private void button_Config_TabLaser_PercentOfEnergySet_Click(object sender, EventArgs e)
        {
            //  Energy Percentage Setting

            double m_dEnergyPercent = Convert.ToDouble(textBox_Config_TabLaser_PercentOfEnergy.Text);

            if (workStage.m_rapidLxLaser_Comm.IsOpen)
            {
                workStage.RapidLxLaserComm_Laser_OutputEnergy_Set(m_dEnergyPercent);
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Laser Comm 이 연결되어 있지 않습니다.");
            }
        }

        private void button_Config_TabWorkStage_ElectroPneumaticRegulator_SetValue_Click(object sender, EventArgs e)
        {
            //  압력 세팅

            double m_dkPa = 0.0;

            //  음압이므로 양수가 들어와도 음수로 변경
            m_dkPa = Math.Abs(Convert.ToDouble(textBox_Config_TabWorkStage_ElectroPneumaticRegulator_SetValue.Text));

            //if ((Convert.ToDouble(textBox_Config_TabWorkStage_ElectroPneumaticRegulator_SetValue.Text) > -1.3) || (Convert.ToDouble(textBox_Config_TabWorkStage_ElectroPneumaticRegulator_SetValue.Text) < -80.0))

            if (m_dkPa == 0.0)
            {
                m_dkPa = 1.3;       //  최저 압력으로 세팅
            }
            else if ((m_dkPa < 1.3) || (m_dkPa > 80.0))
            {
                MessageBox.Show("Electro Pneumatic Regulator out of range\r\n\r\n[Available Range : -1.3kPa ~ -80.0kPa]", "Information!!");
                return;
            }

            m_dkPa *= -1.0;         //  음압으로 변경

            workStage.m_bElectroRegulator_CommData_Received = false;

            workStage.ElectroPneumaticRegulatorComm_Pressure_Set(m_dkPa);
        }

        private void button_Config_TabWorkStage_ElectroPneumaticRegulator_Pressure_Inc_Click(object sender, EventArgs e)
        {
            //  압력 증가

            workStage.m_bElectroRegulator_CommData_SetValueCheck = true;
            workStage.m_bElectroRegulator_CommData_Received = false;            

            workStage.ElectroPneumaticRegulatorComm_Pressure_Inc();
        }

        private void button_Config_TabWorkStage_ElectroPneumaticRegulator_Pressure_Dec_Click(object sender, EventArgs e)
        {
            //  압력 감소

            workStage.m_bElectroRegulator_CommData_SetValueCheck = true;
            workStage.m_bElectroRegulator_CommData_Received = false;

            workStage.ElectroPneumaticRegulatorComm_Pressure_Dec();
        }

        private void button_Config_TabWorkStage_ElectroPneumaticRegulator_GetPressure_Click(object sender, EventArgs e)
        {
            //  압력 확인

            workStage.m_bElectroRegulator_CommData_Received = false;

            workStage.ElectroPneumaticRegulatorComm_Pressure_Read();
        }

        private void button_Config_TabWorkStage_ElectroPneumaticRegulator_SetPressure_Read_Click(object sender, EventArgs e)
        {
            //  세팅 압력 확인

            workStage.m_bElectroRegulator_CommData_SetValueCheck = true;
            workStage.m_bElectroRegulator_CommData_Received = false;

            workStage.ElectroPneumaticRegulatorComm_SettingPressure_Read();
        }

        private void button_Config_TabLaser_LaserConnect_Click(object sender, EventArgs e)
        {
            //  Laser Connect

            if (button_Config_TabLaser_LaserConnect.Text == "Connect")              //  Connect (연결 안된 상태)
            {
                workStage.m_bRapidLxLaser_UserConnect = true;

                workStage.RapidLxLaser_Comm_Init();
            }
            else                                                                    //  Disconnect (연결된 상태)
            {
                workStage.m_rapidLxLaser_Comm.CloseComm();
            }
        }

        private void button_Config_TabWorkStage_DustCollector0_Run_Click(object sender, EventArgs e)
        {
            //  집진기0 켜기

            workStage.DustCollector_On((int)nDustCollector.DustCollector_Upper);
        }

        private void button_Config_TabWorkStage_DustCollector0_Stop_Click(object sender, EventArgs e)
        {
            //  집진기0 끄기

            workStage.DustCollector_Off((int)nDustCollector.DustCollector_Upper);
        }

        private void button_Config_TabWorkStage_DustCollector0_Freq_Set_Click(object sender, EventArgs e)
        {
            //  집진기0 주파수 세팅

            double m_dFreq = Convert.ToDouble(textBox_Config_TabWorkStage_DustCollector0_Freq_SetValue.Text);

            //  입력한 주파수와 가장 가까운 데이터를 찾는다. (일일히 테스트 했음. ㅡㅡ)
            double m_dRet_Freq = GetClosestValue_DustCollector(m_dFreq);

            //  주파수 단위가 0.01Hz 이므로, 100배 해야 함.
            m_dRet_Freq *= 100.0;

            //  숫자를 4자리 숫자로 고정
            string m_strFreq = m_dRet_Freq.ToString("0000");

            string m_strRet = workStage.ConvertDecimalToHex(m_strFreq);

            if (m_strRet != "NG")
            {
                workStage.m_bDustCollector_UpperPos_CommData_Received = false;
                workStage.m_strDustCollector_UpperPos_Comm_ReceivedData = "";

                workStage.DustCollectorComm_Send_Write((int)WorkStage.nDustCollector.DustCollector_Upper, "0005", 1, m_strRet);
            }
        }

        private void button_Config_TabWorkStage_DustCollector1_Run_Click(object sender, EventArgs e)
        {
            //  집진기1 켜기

            workStage.DustCollector_On((int)nDustCollector.DustCollector_Lower);
        }

        private void button_Config_TabWorkStage_DustCollector1_Stop_Click(object sender, EventArgs e)
        {
            //  집진기1 끄기

            workStage.DustCollector_Off((int)nDustCollector.DustCollector_Lower);
        }

        private void button_Config_TabWorkStage_DustCollector1_Freq_Set_Click(object sender, EventArgs e)
        {
            //  집진기1 주파수 세팅

            double m_dFreq = Convert.ToDouble(textBox_Config_TabWorkStage_DustCollector1_Freq_SetValue.Text);

            //  입력한 주파수와 가장 가까운 데이터를 찾는다. (일일히 테스트 했음. ㅡㅡ)
            double m_dRet_Freq = GetClosestValue_DustCollector(m_dFreq);

            //  주파수 단위가 0.01Hz 이므로, 100배 해야 함.
            m_dRet_Freq *= 100.0;

            //  숫자를 4자리 숫자로 고정
            string m_strFreq = m_dRet_Freq.ToString("0000");

            string m_strRet = workStage.ConvertDecimalToHex(m_strFreq);

            if (m_strRet != "NG")
            {
                workStage.m_bDustCollector_LowerPos_CommData_Received = false;
                workStage.m_strDustCollector_LowerPos_Comm_ReceivedData = "";

                workStage.DustCollectorComm_Send_Write((int)WorkStage.nDustCollector.DustCollector_Lower, "0005", 1, m_strRet);
            }
        }

        private void button_Test_ULTransfer_PickupWorkStagePos_Cyc_Click(object sender, EventArgs e)
        {
            //  ULTransfer, Module Pickup from WorkStage Position Cycle

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (unloader.m_nUnloader_Transfer_Step == (int)Unloader.Unloader_Transfer_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Unloader Transfer, Work Stage 에서 Module Pick-Up Cycle 을 진행하시겠습니까?"))
                    return;

                //  Seq. Test
                Equipment.SeqTestMode = true;

                unloader.m_nUnloaderTransferMoveType = (int)UnloaderTransferMoveType.Cycle_WorkStage_PickUp;
                unloader.m_nUnloader_Transfer_Step = (int)Unloader.Unloader_Transfer_Step.Start;

                //  Unloader 실행 타이머
                unloader.m_btimer_UnloaderWork_Stop = false;
                unloader.timer_UnloaderWork.Enabled = true;
            }
            else
            {
                try
                {
                    var mb = new MessageBoxYesNo();
                    if (DialogResult.Yes != mb.ShowDialog("Question ?", "Unloader Transfer, Work Stage 에서 Module Pick-Up Cycle 을 중지 하시겠습니까?"))
                        return;

                    //  Unloader 실행 타이머
                    //unloader.timer_UnloaderWork.Enabled = false;
                    //unloader.m_btimer_UnloaderWork_Stop = true;

                    //  Seq. Test
                    Equipment.SeqTestMode = false;

                    unloader.m_nUnloaderTransferMoveType = (int)UnloaderTransferMoveType.Cycle_None;
                    unloader.m_nUnloader_Transfer_Step = (int)Unloader.Unloader_Transfer_Step.None;

                    if (Equipment.AjinBoard_Opened)
                    {
                        unloader.MC_Func.MC_MotorStop((int)UnloaderParameter.AxisAjinEnum.TR_Z, 2000);
                        unloader.MC_Func.MC_MotorStop((int)UnloaderParameter.AxisAjinEnum.TR_X, 2000);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        private void button_Test_ULTransfer_PutdownNGPortPos_Cyc_Click(object sender, EventArgs e)
        {
            //  ULTransfer, Module Putdown to NG Port Position Cycle

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (unloader.m_nUnloader_Transfer_Step == (int)Unloader.Unloader_Transfer_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Unloader Transfer, NG-Port 에 Module Drop Cycle 을 진행하시겠습니까?"))
                    return;

                //  Seq. Test
                Equipment.SeqTestMode = true;

                unloader.m_nUnloaderTransferMoveType = (int)UnloaderTransferMoveType.Cycle_NG_PutDown;
                unloader.m_nUnloader_Transfer_Step = (int)Unloader.Unloader_Transfer_Step.Start;

                //  Unloader 실행 타이머
                unloader.m_btimer_UnloaderWork_Stop = false;
                unloader.timer_UnloaderWork.Enabled = true;
            }
            else
            {
                try
                {
                    var mb = new MessageBoxYesNo();
                    if (DialogResult.Yes != mb.ShowDialog("Question ?", "Unloader Transfer, NG-Port 에 Module Drop Cycle 을 중지 하시겠습니까?"))
                        return;

                    //  Unloader 실행 타이머
                    //unloader.timer_UnloaderWork.Enabled = false;
                    //unloader.m_btimer_UnloaderWork_Stop = true;

                    //  Seq. Test
                    Equipment.SeqTestMode = false;

                    unloader.m_nUnloaderTransferMoveType = (int)UnloaderTransferMoveType.Cycle_None;
                    unloader.m_nUnloader_Transfer_Step = (int)Unloader.Unloader_Transfer_Step.None;

                    if (Equipment.AjinBoard_Opened)
                    {
                        unloader.MC_Func.MC_MotorStop((int)UnloaderParameter.AxisAjinEnum.TR_Z, 2000);
                        unloader.MC_Func.MC_MotorStop((int)UnloaderParameter.AxisAjinEnum.TR_X, 2000);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        private void button_Test_ULTransfer_PutdownLPortPos_Cyc_Click(object sender, EventArgs e)
        {
            //  ULTransfer, Module Putdown to L Port Position Cycle

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (unloader.m_nUnloader_Transfer_Step == (int)Unloader.Unloader_Transfer_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Unloader Transfer, L-Port 에 Module Putdown Cycle 을 진행하시겠습니까?"))
                    return;

                //  Seq. Test
                Equipment.SeqTestMode = true;

                unloader.m_nUnloaderTransferMoveType = (int)UnloaderTransferMoveType.Cycle_Stacker1_PutDown;
                unloader.m_nUnloader_Transfer_Step = (int)Unloader.Unloader_Transfer_Step.Start;

                //  Unloader 실행 타이머
                unloader.m_btimer_UnloaderWork_Stop = false;
                unloader.timer_UnloaderWork.Enabled = true;
            }
            else
            {
                try
                {
                    var mb = new MessageBoxYesNo();
                    if (DialogResult.Yes != mb.ShowDialog("Question ?", "Unloader Transfer, L-Port 에 Module Putdown Cycle 을 중지 하시겠습니까?"))
                        return;

                    //  Unloader 실행 타이머
                    //unloader.timer_UnloaderWork.Enabled = false;
                    //unloader.m_btimer_UnloaderWork_Stop = true;

                    //  Seq. Test
                    Equipment.SeqTestMode = false;

                    unloader.m_nUnloaderTransferMoveType = (int)UnloaderTransferMoveType.Cycle_None;
                    unloader.m_nUnloader_Transfer_Step = (int)Unloader.Unloader_Transfer_Step.None;

                    if (Equipment.AjinBoard_Opened)
                    {
                        unloader.MC_Func.MC_MotorStop((int)UnloaderParameter.AxisAjinEnum.TR_Z, 2000);
                        unloader.MC_Func.MC_MotorStop((int)UnloaderParameter.AxisAjinEnum.TR_X, 2000);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        private void button_Test_ULTransfer_PutdownRPortPos_Cyc_Click(object sender, EventArgs e)
        {
            //  ULTransfer, Module Putdown to R Port Position Cycle

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (unloader.m_nUnloader_Transfer_Step == (int)Unloader.Unloader_Transfer_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Unloader Transfer, R-Port 에 Module Putdown Cycle 을 진행하시겠습니까?"))
                    return;

                //  Seq. Test
                Equipment.SeqTestMode = true;

                unloader.m_nUnloaderTransferMoveType = (int)UnloaderTransferMoveType.Cycle_Stacker0_PutDown;
                unloader.m_nUnloader_Transfer_Step = (int)Unloader.Unloader_Transfer_Step.Start;

                //  Unloader 실행 타이머
                unloader.m_btimer_UnloaderWork_Stop = false;
                unloader.timer_UnloaderWork.Enabled = true;
            }
            else
            {
                try
                {
                    var mb = new MessageBoxYesNo();
                    if (DialogResult.Yes != mb.ShowDialog("Question ?", "Unloader Transfer, R-Port 에 Module Putdown Cycle 을 중지 하시겠습니까?"))
                        return;

                    //  Unloader 실행 타이머
                    //unloader.timer_UnloaderWork.Enabled = false;
                    //unloader.m_btimer_UnloaderWork_Stop = true;

                    //  Seq. Test
                    Equipment.SeqTestMode = false;

                    unloader.m_nUnloaderTransferMoveType = (int)UnloaderTransferMoveType.Cycle_None;
                    unloader.m_nUnloader_Transfer_Step = (int)Unloader.Unloader_Transfer_Step.None;

                    if (Equipment.AjinBoard_Opened)
                    {
                        unloader.MC_Func.MC_MotorStop((int)UnloaderParameter.AxisAjinEnum.TR_Z, 2000);
                        unloader.MC_Func.MC_MotorStop((int)UnloaderParameter.AxisAjinEnum.TR_X, 2000);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        private void button_Test_LDTransfer_PutdownWorkStagePos_Cyc_Click(object sender, EventArgs e)
        {
            //  LDTransfer, Module Putdown to WorkStage Position Cycle

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            //var mb = new MessageBoxYesNo();
            //if (DialogResult.Yes != mb.ShowDialog("Question ?", "Work Stage 가 Loading 위치에 있습니까?"))
            //    return;

            //workStage.m_bWorkStageMove_Complete = true;

            if (loader.m_nLoader_Transfer_Step == (int)Loader.Loader_Transfer_Step.None)
            {
                var mb1 = new MessageBoxYesNo();
                if (DialogResult.Yes != mb1.ShowDialog("Question ?", "Loader Transfer, Work Stage 에 Module Put-Down Cycle 을 진행하시겠습니까?"))
                    return;

                //  Seq. Test
                Equipment.SeqTestMode = true;

                loader.m_nLoaderTransferMoveType = (int)LoaderTransferMoveType.Cycle_WorkStage_PutDown;
                loader.m_nLoader_Transfer_Step = (int)Loader.Loader_Transfer_Step.Start;
                
                //  Loader 실행 타이머
                loader.m_btimer_LoaderWork_Stop = false;
                loader.timer_LoaderWork.Enabled = true;
            }
            else
            {
                try
                {
                    var mb1 = new MessageBoxYesNo();
                    if (DialogResult.Yes != mb1.ShowDialog("Question ?", "Loader Transfer, Work Stage 에 Module Put-Down Cycle 을 중지 하시겠습니까?"))
                        return;

                    //  Loader 실행 타이머
                    //loader.timer_LoaderWork.Enabled = false;
                    //loader.m_btimer_LoaderWork_Stop = true;

                    //  Seq. Test
                    Equipment.SeqTestMode = false;

                    loader.m_nLoaderTransferMoveType = (int)LoaderTransferMoveType.Cycle_None;
                    loader.m_nLoader_Transfer_Step = (int)Loader.Loader_Transfer_Step.None;

                    if (Equipment.AjinBoard_Opened)
                    {
                        loader.MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.TR_Z, 2000);
                        loader.MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.TR_X, 2000);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        private void button_Test_LDTransfer_PutdownMAlignerPos_Cyc_Click(object sender, EventArgs e)
        {
            //  LDTransfer, Module Putdown to M-Aligner Position Cycle

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (loader.m_nLoader_Transfer_Step == (int)Loader.Loader_Transfer_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Loader Transfer, M-Aligner 에 Module Put-Down Cycle 을 진행하시겠습니까?"))
                    return;

                //  Seq. Test
                Equipment.SeqTestMode = true;


                double m_dModuleWidth = Equipment.stLayerRecipeSet[0].ModuleInformation_Module_Width;
                double m_dModuleHeight = Equipment.stLayerRecipeSet[0].ModuleInformation_Module_Height;

                if ((m_dModuleWidth <= 0.0) || (m_dModuleHeight <= 0.0))
                {
                    m_dModuleWidth = Convert.ToDouble(textBox_Config_SeqTest_ModuleSize_Width.Text);
                    m_dModuleHeight = Convert.ToDouble(textBox_Config_SeqTest_ModuleSize_Height.Text);
                }

                //m_dModuleWidth = Convert.ToDouble(textBox_Config_SeqTest_ModuleSize_Width.Text);
                //m_dModuleHeight = Convert.ToDouble(textBox_Config_SeqTest_ModuleSize_Height.Text);

                if ((m_dModuleWidth <= 0.0) || (m_dModuleHeight <= 0.0))
                {
                    var mb1 = new MessageBoxOk();
                    mb1.ShowDialog("Information !", "Mechanical Align 을 위한 Module Size 를 입력해야 합니다.");
                    return;
                }

                loader.m_dMAlign_ModuleSize_Width = m_dModuleWidth;
                loader.m_dMAlign_ModuleSize_Height = m_dModuleHeight;

                loader.m_nLoaderTransferMoveType = (int)LoaderTransferMoveType.Cycle_MAligner_PutDown;
                loader.m_nLoader_Transfer_Step = (int)Loader.Loader_Transfer_Step.Start;

                //  Loader 실행 타이머
                loader.m_btimer_LoaderWork_Stop = false;
                loader.timer_LoaderWork.Enabled = true;
            }
            else
            {
                try
                {
                    var mb = new MessageBoxYesNo();
                    if (DialogResult.Yes != mb.ShowDialog("Question ?", "Loader Transfer, M-Aligner 에 Module Put-Down Cycle 을 중지 하시겠습니까?"))
                        return;

                    //  Loader 실행 타이머
                    //loader.timer_LoaderWork.Enabled = false;
                    //loader.m_btimer_LoaderWork_Stop = true;

                    //  Seq. Test
                    Equipment.SeqTestMode = false;

                    loader.m_nLoaderTransferMoveType = (int)LoaderTransferMoveType.Cycle_None;
                    loader.m_nLoader_Transfer_Step = (int)Loader.Loader_Transfer_Step.None;

                    if (Equipment.AjinBoard_Opened)
                    {
                        loader.MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.TR_Z, 2000);
                        loader.MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.TR_X, 2000);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        private void button_Test_LDTransfer_PickupMAlignerPos_Cyc_Click(object sender, EventArgs e)
        {
            //  LDTransfer, Module Pickup from M-Aligner Position Cycle

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (loader.m_nLoader_Transfer_Step == (int)Loader.Loader_Transfer_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Loader Transfer, M-Aligner 에서 Module Pick-Up Cycle 을 진행하시겠습니까?"))
                    return;

                //  Seq. Test
                Equipment.SeqTestMode = true;

                loader.m_nLoaderTransferMoveType = (int)LoaderTransferMoveType.Cycle_MAligner_PickUp;
                loader.m_nLoader_Transfer_Step = (int)Loader.Loader_Transfer_Step.Start;

                //  Loader 실행 타이머
                loader.m_btimer_LoaderWork_Stop = false;
                loader.timer_LoaderWork.Enabled = true;
            }
            else
            {
                try
                {
                    var mb = new MessageBoxYesNo();
                    if (DialogResult.Yes != mb.ShowDialog("Question ?", "Loader Transfer, M-Aligner 에서 Module Pick-Up Cycle 을 중지 하시겠습니까?"))
                        return;

                    //  Loader 실행 타이머
                    //loader.timer_LoaderWork.Enabled = false;
                    //loader.m_btimer_LoaderWork_Stop = true;

                    //  Seq. Test
                    Equipment.SeqTestMode = false;

                    loader.m_nLoaderTransferMoveType = (int)LoaderTransferMoveType.Cycle_None;
                    loader.m_nLoader_Transfer_Step = (int)Loader.Loader_Transfer_Step.None;

                    if (Equipment.AjinBoard_Opened)
                    {
                        loader.MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.TR_Z, 2000);
                        loader.MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.TR_X, 2000);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        private void button_Test_LDTransfer_PickupLPortPos_Cyc_Click(object sender, EventArgs e)
        {
            //  LDTransfer, Module Pickup from L Port Position Cycle

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (loader.m_nLoader_Transfer_Step == (int)Loader.Loader_Transfer_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Loader Transfer, L-Port 에서 Module Pick-Up Cycle 을 진행하시겠습니까?"))
                    return;

                //  Seq. Test
                Equipment.SeqTestMode = true;

                loader.m_nLoaderTransferMoveType = (int)LoaderTransferMoveType.Cycle_Stacker1_PickUp;
                loader.m_nLoader_Transfer_Step = (int)Loader.Loader_Transfer_Step.Start;

                //  Loader 실행 타이머
                loader.m_btimer_LoaderWork_Stop = false;
                loader.timer_LoaderWork.Enabled = true; 
            }
            else
            {
                try
                {
                    var mb = new MessageBoxYesNo();
                    if (DialogResult.Yes != mb.ShowDialog("Question ?", "Loader Transfer, L-Port 에서 Module Pick-Up Cycle 을 중지 하시겠습니까?"))
                        return;

                    //  Loader 실행 타이머
                    //loader.timer_LoaderWork.Enabled = false;
                    //loader.m_btimer_LoaderWork_Stop = true;

                    //  Seq. Test
                    Equipment.SeqTestMode = false;

                    loader.m_nLoaderTransferMoveType = (int)LoaderTransferMoveType.Cycle_None;
                    loader.m_nLoader_Transfer_Step = (int)Loader.Loader_Transfer_Step.None;

                    if (Equipment.AjinBoard_Opened)
                    {
                        loader.MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.TR_Z, 2000);
                        loader.MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.TR_X, 2000);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        private void button_Test_LDTransfer_PickupRPortPos_Cyc_Click(object sender, EventArgs e)
        {
            //  LDTransfer, Module Pickup from R Port Position Cycle

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (loader.m_nLoader_Transfer_Step == (int)Loader.Loader_Transfer_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Loader Transfer, R-Port 에서 Module Pick-Up Cycle 을 진행하시겠습니까?"))
                    return;

                //  Seq. Test
                Equipment.SeqTestMode = true;

                loader.m_nLoaderTransferMoveType = (int)LoaderTransferMoveType.Cycle_Stacker0_PickUp;
                loader.m_nLoader_Transfer_Step = (int)Loader.Loader_Transfer_Step.Start;

                //  Loader 실행 타이머
                loader.m_btimer_LoaderWork_Stop = false;
                loader.timer_LoaderWork.Enabled = true;
            }
            else
            {
                try
                {
                    var mb = new MessageBoxYesNo();
                    if (DialogResult.Yes != mb.ShowDialog("Question ?", "Loader Transfer, R-Port 에서 Module Pick-Up Cycle 을 중지 하시겠습니까?"))
                        return;

                    //  Loader 실행 타이머
                    //loader.timer_LoaderWork.Enabled = false;
                    //loader.m_btimer_LoaderWork_Stop = true;

                    //  Seq. Test
                    Equipment.SeqTestMode = false;

                    loader.m_nLoaderTransferMoveType = (int)LoaderTransferMoveType.Cycle_None;
                    loader.m_nLoader_Transfer_Step = (int)Loader.Loader_Transfer_Step.None;

                    if (Equipment.AjinBoard_Opened)
                    {
                        loader.MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.TR_Z, 2000);
                        loader.MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.TR_X, 2000);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        private void Button_Config_LDUL_TeachingPositions_Move_Click(object sender, EventArgs e)
        {
            //  Loader Unloader Teaching Position 이동

            var mb = new MessageBoxOk();
            mb.ShowDialog("Information !", "미구현 기능.");
            return;



            //double lfVelocity = 0.0f;
            //double lfAccDec = 0.0f;

            //int m_nIndex = listBox_Config_LDUL_TeachingPositions.SelectedIndex;

            //if (m_nIndex < 0)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Warning !", "Teaching Position 이 선택되지 않았습니다.");
            //    return;
            //}

            ////if (!workStage.m_bHomeOK)
            ////{
            ////    var mb1 = new MessageBoxOk();
            ////    mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
            ////    return;
            ////}

            //var mb = new MessageBoxYesNo();
            //if (DialogResult.Yes != mb.ShowDialog("Question ?", "Stage 를 선택 위치로 보내시겠습니까?\r\n\r\n##  XY 방향 이동 시 Z축 충돌 주의!!!  ##"))
            //    return;

            //if (!workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.X) || !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Y) || !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Z) ||
            //    !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.X) || !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Y) || !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Z))
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Warning !", "Stage 가 이동중입니다.");
            //    return;
            //}


            //switch (m_nIndex)
            //{

            //}


            ////  속도 설정
            //if (radioButton_Config_WorkStage_TeachingPositions_MoveMode_Fine.Checked)
            //{
            //    lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Fine;
            //    lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Acceleration_Fine;
            //}
            //else
            //{
            //    lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Coarse;
            //    lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Common_Acceleration_Coarse;
            //}

            ////workStage.MC_Func.MC_MovePosition((int)WorkStage.nAxis.X, workStage.stWorkStageTeachingPos[(int)WorkStage_TeachingPosList.STAGE_ProcessingPos].Stage_X,
            ////                                lfVelocity, lfAccDec, lfAccDec);
            ////workStage.MC_Func.MC_MovePosition((int)WorkStage.nAxis.Y, workStage.stWorkStageTeachingPos[(int)WorkStage_TeachingPosList.STAGE_ProcessingPos].Stage_Y,
            ////                               lfVelocity, lfAccDec, lfAccDec);

            //xyInterpolatedCoordinate.X = workStage.stWorkStageTeachingPos[m_nIndex].Stage_X;
            //xyInterpolatedCoordinate.Y = workStage.stWorkStageTeachingPos[m_nIndex].Stage_Y;
            //workStage.MC_Func.MovePosition(xyInterpolatedCoordinate, lfVelocity, lfAccDec, lfAccDec);
        }

        private void Button_Config_Vision_TeachingPositions_Move_Click(object sender, EventArgs e)
        {
            //  Vision(Scanner) Z Teaching Position 이동

            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;

            int m_nIndex = listBox_Config_Vision_TeachingPositions.SelectedIndex;

            if (m_nIndex < 0)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Teaching Position 이 선택되지 않았습니다.");
                return;
            }

            //if (!workStage.m_bHomeOK)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
            //    return;
            //}

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Vision(Scanner) Z축을 선택 위치로 보내시겠습니까?\r\n\r\n##  Z축 충돌 주의!!!  ##"))
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


            //  속도 설정
            if (radioButton_Config_WorkStage_TeachingPositions_MoveMode_Fine.Checked)
            {
                lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.Z].Jog_Speed_Fine;
                lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.Z].Common_Acceleration_Fine;
            }
            else
            {
                lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.Z].Jog_Speed_Coarse;
                lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.Z].Common_Acceleration_Coarse;
            }

            workStage.MC_Func.MC_MovePosition((int)WorkStage.nAxis.Z, vision.stVisionTeachingPos[m_nIndex].Vision_Z,
                                            lfVelocity, lfAccDec, lfAccDec);
        }

        private void Button_Config_BDS_TeachingPositions_Move_Click(object sender, EventArgs e)
        {
            //  BDS Mask Y Teaching Position 이동

            double lfVelocity = 0.0f;
            double lfAccDec = 0.0f;

            if (!Equipment.Machine_LaserType_CO2)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "UV Laser System 에는 Mask 가 없습니다.");
                return;
            }

            int m_nIndex = listBox_Config_BDS_TeachingPositions.SelectedIndex;

            if (m_nIndex < 0)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Teaching Position 이 선택되지 않았습니다.");
                return;
            }

            //if (!workStage.m_bHomeOK)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
            //    return;
            //}

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Mask Y축을 선택 위치로 보내시겠습니까?\r\n\r\n"))
                return;


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


            //  레이저 끄기
            if (workStage.rtc != null)
            {
                workStage.rtc.CtlLaserOff();
            }

            //  셔터 (BDS Power Meter) 닫기
            int m_nShutterCloseCount = 0;
            bool m_bShutterClosed = false;
            do
            {
                m_nShutterCloseCount++;

                //  BDS Power Meter 셔터 닫기
                bds.bdsParameter.DO_BDS_PowerMeter_FW(true);
                bds.bdsParameter.DO_BDS_PowerMeter_BW(false);

                //  BDS Power Meter 셔터 닫기 확인
                if (bds.bdsParameter.DI_BDS_PowerMeter_FW() && !bds.bdsParameter.DI_BDS_PowerMeter_BW())
                {
                    m_bShutterClosed = true;
                }
            } while ((m_nShutterCloseCount < 5000) && !m_bShutterClosed);

            if (!m_bShutterClosed)
            {
                var mb1 = new MessageBoxYesNo();
                if (DialogResult.Yes != mb1.ShowDialog("Question ?", "Shutter 가 닫히지 않았습니다.\r\n\r\nMask Y축을 선택 위치 이동을 계속하시겠습니까?\r\n\r\n"))
                    return;
            }

            //  속도 설정
            if (radioButton_Config_BDS_Move_MoveMode_Fine.Checked)
            {
                lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.Z].Jog_Speed_Fine;
                lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.Z].Common_Acceleration_Fine;
            }
            else
            {
                lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.Z].Jog_Speed_Coarse;
                lfAccDec = Equipment.stAxisParam[(int)WorkStage.nAxis.Z].Common_Acceleration_Coarse;
            }
                        
            bds.MC_Func.MC_MovePosition((int)Bds.nAxis.MASK_Y, bds.stBDSTeachingPos[m_nIndex].Mask_Y,
                                            lfVelocity, lfAccDec, lfAccDec);
        }

        private void button_Config_WorkStage_CurrentLaserSensorPos_To_FineCamPos_Click(object sender, EventArgs e)
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
            ///

            //  속도 설정
            if (radioButton_Config_WorkStage_Move_MoveMode_Fine.Checked)
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

        private void button_Config_WorkStage_CurrentFineCamPos_To_LaserSensorPos_Click(object sender, EventArgs e)
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
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "현재 Fine Camera 위치를 Laser Sensor 위치로 보내시겠습니까?"))
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
            if (radioButton_Config_WorkStage_Move_MoveMode_Fine.Checked)
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

        private void button_Test_MAligner_ModuleAlign_Cyc_Click(object sender, EventArgs e)
        {
            //  Module Mechanical Alignment Cycle

            double m_dModuleWidth = 0.0;
            double m_dModuleHeight = 0.0;

            m_dModuleWidth = Convert.ToDouble(textBox_Config_SeqTest_ModuleSize_Width.Text);
            m_dModuleHeight = Convert.ToDouble(textBox_Config_SeqTest_ModuleSize_Height.Text);

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if ((m_dModuleWidth <= 0.0) || (m_dModuleHeight <= 0.0))
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Module Size 가 설정되어 있지 않습니다.");
                return;
            }

            if (loader.m_nMAlign_Step == (int)Loader.MAlign_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "M-Aligner, Module Align Cycle 을 진행하시겠습니까?"))
                    return;

                //  Module M-Align Cycle 에서 사용하는 변수에 Module Size 를 설정
                loader.m_dMAlign_ModuleSize_Width = m_dModuleWidth;
                loader.m_dMAlign_ModuleSize_Height = m_dModuleHeight;

                //  Seq. Test
                Equipment.SeqTestMode = true;

                loader.m_nMAlign_Step = (int)Loader.MAlign_Step.Start;

                //  Loader 실행 타이머
                loader.m_btimer_LoaderWork_Stop = false;
                loader.timer_LoaderWork.Enabled = true;
            }
            else
            {
                try
                {
                    var mb = new MessageBoxYesNo();
                    if (DialogResult.Yes != mb.ShowDialog("Question ?", "M-Aligner, Module Align Cycle 을 중지 하시겠습니까?"))
                        return;

                    //  Loader 실행 타이머
                    //loader.timer_LoaderWork.Enabled = false;
                    //loader.m_btimer_LoaderWork_Stop = true;

                    //  Seq. Test
                    Equipment.SeqTestMode = false;

                    loader.m_nMAlign_Step = (int)Loader.MAlign_Step.None;

                    if (Equipment.AjinBoard_Opened)
                    {
                        loader.MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.ALN_X, 2000);
                        loader.MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.ALN_Y, 2000);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        private void button_Test_WorkStage_ModuleUnloadingPos_Cyc_Click(object sender, EventArgs e)
        {
            //  Work Stage, Module Unloading Position Cycle

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (workStage.m_nWorkStage_Move_Step == (int)WorkStage.WorkStage_Move_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Work Stage, Module Unloading Position Move Cycle 을 진행하시겠습니까?"))
                    return;

                workStage.m_nWorkStageMoveType = (int)WorkStage.WorkStageMoveType.MoveTo_UnloadingPos;
                workStage.m_nWorkStage_Move_Step = (int)WorkStage.WorkStage_Move_Step.Start;

                //  Work Stage 실행 타이머
                workStage.m_btimer_SubWork_Stop = false;
                workStage.timer_SubWork.Enabled = true;
            }
            else
            {
                try
                {
                    var mb = new MessageBoxYesNo();
                    if (DialogResult.Yes != mb.ShowDialog("Question ?", "Work Stage, Module Unloading Position Move Cycle 을 중지 하시겠습니까?"))
                        return;

                    workStage.m_nWorkStageMoveType = (int)WorkStage.WorkStageMoveType.MoveTo_None;
                    workStage.m_nWorkStage_Move_Step = (int)WorkStage.WorkStage_Move_Step.None;

                    if (Equipment.AjinBoard_Opened)
                    {
                        workStage.MC_Func.MC_MotorStop((int)WorkStage.nAxis.X, 2000);
                        workStage.MC_Func.MC_MotorStop((int)WorkStage.nAxis.Y, 2000);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        private void button_Test_WorkStage_ModuleLoadingPos_Cyc_Click(object sender, EventArgs e)
        {
            //  Work Stage, Module Loading Position Cycle

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (workStage.m_nWorkStage_Move_Step == (int)WorkStage.WorkStage_Move_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Work Stage, Module Loading Position Move Cycle 을 진행하시겠습니까?"))
                    return;

                workStage.m_nWorkStageMoveType = (int)WorkStage.WorkStageMoveType.MoveTo_LoadingPos;
                workStage.m_nWorkStage_Move_Step = (int)WorkStage.WorkStage_Move_Step.Start;

                //  Work Stage 실행 타이머
                workStage.m_btimer_SubWork_Stop = false;
                workStage.timer_SubWork.Enabled = true;
            }
            else
            {
                try
                {
                    var mb = new MessageBoxYesNo();
                    if (DialogResult.Yes != mb.ShowDialog("Question ?", "Work Stage, Module Loading Position Move Cycle 을 중지 하시겠습니까?"))
                        return;

                    workStage.m_nWorkStageMoveType = (int)WorkStage.WorkStageMoveType.MoveTo_None;
                    workStage.m_nWorkStage_Move_Step = (int)WorkStage.WorkStage_Move_Step.None;

                    if (Equipment.AjinBoard_Opened)
                    {
                        workStage.MC_Func.MC_MotorStop((int)WorkStage.nAxis.X, 2000);
                        workStage.MC_Func.MC_MotorStop((int)WorkStage.nAxis.Y, 2000);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        private void button_Test_WorkStage_ScannerCenterPos_Cyc_Click(object sender, EventArgs e)
        {
            //  Work Stage, Scanner Center Position Cycle

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (workStage.m_nWorkStage_Move_Step == (int)WorkStage.WorkStage_Move_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Work Stage, Scanner Center Position Move Cycle 을 진행하시겠습니까?"))
                    return;

                workStage.m_nWorkStageMoveType = (int)WorkStage.WorkStageMoveType.MoveTo_ScannerCenterPos;
                workStage.m_nWorkStage_Move_Step = (int)WorkStage.WorkStage_Move_Step.Start;

                //  Work Stage 실행 타이머
                workStage.m_btimer_SubWork_Stop = false;
                workStage.timer_SubWork.Enabled = true;
            }
            else
            {
                try
                {
                    var mb = new MessageBoxYesNo();
                    if (DialogResult.Yes != mb.ShowDialog("Question ?", "Work Stage, Scanner Center Position Move Cycle 을 중지 하시겠습니까?"))
                        return;

                    workStage.m_nWorkStageMoveType = (int)WorkStage.WorkStageMoveType.MoveTo_None;
                    workStage.m_nWorkStage_Move_Step = (int)WorkStage.WorkStage_Move_Step.None;

                    if (Equipment.AjinBoard_Opened)
                    {
                        workStage.MC_Func.MC_MotorStop((int)WorkStage.nAxis.X, 2000);
                        workStage.MC_Func.MC_MotorStop((int)WorkStage.nAxis.Y, 2000);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        private void button_Test_WorkStage_FineCameraCenterPos_Cyc_Click(object sender, EventArgs e)
        {
            //  Work Stage, Fine Camera Center Position Cycle

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (workStage.m_nWorkStage_Move_Step == (int)WorkStage.WorkStage_Move_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Work Stage, Fine Camera Center Position Move Cycle 을 진행하시겠습니까?"))
                    return;

                workStage.m_nWorkStageMoveType = (int)WorkStage.WorkStageMoveType.MoveTo_CameraCenterPos;
                workStage.m_nWorkStage_Move_Step = (int)WorkStage.WorkStage_Move_Step.Start;

                //  Work Stage 실행 타이머
                workStage.m_btimer_SubWork_Stop = false;
                workStage.timer_SubWork.Enabled = true;
            }
            else
            {
                try
                {
                    var mb = new MessageBoxYesNo();
                    if (DialogResult.Yes != mb.ShowDialog("Question ?", "Work Stage, Fine Camera Center Position Move Cycle 을 중지 하시겠습니까?"))
                        return;

                    workStage.m_nWorkStageMoveType = (int)WorkStage.WorkStageMoveType.MoveTo_None;
                    workStage.m_nWorkStage_Move_Step = (int)WorkStage.WorkStage_Move_Step.None;

                    if (Equipment.AjinBoard_Opened)
                    {
                        workStage.MC_Func.MC_MotorStop((int)WorkStage.nAxis.X, 2000);
                        workStage.MC_Func.MC_MotorStop((int)WorkStage.nAxis.Y, 2000);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        private void button_Test_Scanner_PowerMeterPos_Cyc_Click(object sender, EventArgs e)
        {
            //  Scanner, Power Meter Position Cycle

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (workStage.m_nWorkStage_Move_Step == (int)WorkStage.WorkStage_Move_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Scanner, Laser Power Check Position Move Cycle 을 진행하시겠습니까?"))
                    return;

                workStage.m_nWorkStageMoveType = (int)WorkStage.WorkStageMoveType.MoveTo_PowerCheckPos;
                workStage.m_nWorkStage_Move_Step = (int)WorkStage.WorkStage_Move_Step.Start;

                //  Work Stage 실행 타이머
                workStage.m_btimer_SubWork_Stop = false;
                workStage.timer_SubWork.Enabled = true;
            }
            else
            {
                try
                {
                    var mb = new MessageBoxYesNo();
                    if (DialogResult.Yes != mb.ShowDialog("Question ?", "Scanner, Laser Power Check Position Move Cycle 을 중지 하시겠습니까?"))
                        return;

                    workStage.m_nWorkStageMoveType = (int)WorkStage.WorkStageMoveType.MoveTo_None;
                    workStage.m_nWorkStage_Move_Step = (int)WorkStage.WorkStage_Move_Step.None;

                    if (Equipment.AjinBoard_Opened)
                    {
                        workStage.MC_Func.MC_MotorStop((int)WorkStage.nAxis.X, 2000);
                        workStage.MC_Func.MC_MotorStop((int)WorkStage.nAxis.Y, 2000);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        private void button_TestbyUser_RPort_Start_Click(object sender, EventArgs e)
        {
            if (Equipment.AutoRunStatus)
            {
                loader.m_bStacker0_Run_byUser = true;

                loader.m_nLoaderTransfer_ProcessStep = (int)LoaderTransferProcessStep.LoaderStep_ModulePickup_fromStacker;
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Auto Run 상태가 아닙니다.");
                return;
            }
        }

        private void button_TestbyUser_LPort_Start_Click(object sender, EventArgs e)
        {
            if (Equipment.AutoRunStatus)
            {
                loader.m_bStacker1_Run_byUser = true;

                loader.m_nLoaderTransfer_ProcessStep = (int)LoaderTransferProcessStep.LoaderStep_ModulePickup_fromStacker;
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Auto Run 상태가 아닙니다.");
                return;
            }
        }

        private void button_TestbyUser_LaserDrilling_OK_Click(object sender, EventArgs e)
        {
            if (Equipment.AutoRunStatus)
            {
                workStage.m_bMainWorkCycle_Complete = true;
                //workStage.m_bMainWorkCycle_ResultOK = true;
                workStage.m_nMainWorkCycle_ResultOKNG = (int)WorkStage.MainCycle_Result.OK;
                workStage.m_bMainWorkCycle_ResultOK_toRPort = true;
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Auto Run 상태가 아닙니다.");
                return;
            }
        }

        private void button_TestbyUser_LaserDrilling_NG_Click(object sender, EventArgs e)
        {
            if (Equipment.AutoRunStatus)
            {
                workStage.m_bMainWorkCycle_Complete = true;
                //workStage.m_bMainWorkCycle_ResultOK = false;
                workStage.m_nMainWorkCycle_ResultOKNG = (int)WorkStage.MainCycle_Result.NG;
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Auto Run 상태가 아닙니다.");
                return;
            }
        }

        private void button_TestbyUser_LaserDrilling_toLPort_OK_Click(object sender, EventArgs e)
        {
            if (Equipment.AutoRunStatus)
            {
                workStage.m_bMainWorkCycle_Complete = true;
                //workStage.m_bMainWorkCycle_ResultOK = true;
                workStage.m_nMainWorkCycle_ResultOKNG = (int)WorkStage.MainCycle_Result.OK;
                workStage.m_bMainWorkCycle_ResultOK_toRPort = false;
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Auto Run 상태가 아닙니다.");
                return;
            }
        }

        private void button_Test_LDStacker0_ModulesLoadingPos_Cyc_Click(object sender, EventArgs e)
        {
            //  Loading Modules Position
            
            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (loader.m_nStacker0_ModulePickupWaitingPos_Step == (int)Loader.StackerModulePickupWaitingPos_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Loader R-Port, Module Loading 위치로 이동하시겠습니까?"))
                    return;

                //  속도
                double m_dSpeed = Equipment.stAxisParam[(int)Loader.nAxis.Z0].Common_Speed_Coarse;

                //  가감속 배율
                double m_dSpeedMag = 2.0;

                workStage.MC_Func.MC_MovePosition((int)Loader.nAxis.Z0,
                                    loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_RPort_ReadyPos].LD_Stacker_Z0,
                                    m_dSpeed,
                                    m_dSpeed * m_dSpeedMag,
                                    m_dSpeed * m_dSpeedMag);
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Loader R-Port 가 동작중입니다.");
                return;
            }
        }

        private void button_Test_LDStacker1_ModulesLoadingPos_Cyc_Click(object sender, EventArgs e)
        {
            //  Loading Modules Position

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (loader.m_nStacker1_ModulePickupWaitingPos_Step == (int)Loader.StackerModulePickupWaitingPos_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Loader L-Port, Module Loading 위치로 이동하시겠습니까?"))
                    return;

                //  속도
                double m_dSpeed = Equipment.stAxisParam[(int)Loader.nAxis.Z1].Common_Speed_Coarse;

                //  가감속 배율
                double m_dSpeedMag = 2.0;

                workStage.MC_Func.MC_MovePosition((int)Loader.nAxis.Z1,
                                    loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_RPort_ReadyPos].LD_Stacker_Z1,
                                    m_dSpeed,
                                    m_dSpeed * m_dSpeedMag,
                                    m_dSpeed * m_dSpeedMag);
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Loader L-Port 가 동작중입니다.");
                return;
            }
        }

        private void button_Test_WorkStage_ModuleLoadingFlag_OK_Click(object sender, EventArgs e)
        {
            loader.m_bAUTORUN_Loader_Transfer_ModulePutDowntoWorkStage_Complete = false;
        }

        private void button_Test_ULTransfer_AxisZ_SafetyPos_Click(object sender, EventArgs e)
        {
            //  Unloader Axis-Z, 대기위치 이동

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (unloader.m_nUnloader_Transfer_Step == (int)Unloader.Unloader_Transfer_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Unloader Transfer Z축, 대기 위치로 이동하시겠습니까?"))
                    return;

                //  속도
                double m_dSpeed = Equipment.stAxisParam[(int)Unloader.nAxis.TR_Z].Common_Speed_Coarse;

                //  가감속 배율
                double m_dSpeedMag = 2.0;

                workStage.MC_Func.MC_MovePosition((int)Unloader.nAxis.TR_Z,
                                    loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.UL_TR_SafetyPos].UL_Transfer_Z,
                                    m_dSpeed,
                                    m_dSpeed * m_dSpeedMag,
                                    m_dSpeed * m_dSpeedMag);
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Unloader Transfer 가 동작중입니다.");
                return;
            }
        }

        private void button_Test_LDTransfer_AxisZ_SafetyPos_Click(object sender, EventArgs e)
        {
            //  Loader Axis-Z, 대기위치 이동

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (loader.m_nLoader_Transfer_Step == (int)Loader.Loader_Transfer_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Loader Transfer Z축, 대기 위치로 이동하시겠습니까?"))
                    return;

                //  속도
                double m_dSpeed = Equipment.stAxisParam[(int)Loader.nAxis.TR_Z].Common_Speed_Coarse;

                //  가감속 배율
                double m_dSpeedMag = 2.0;

                workStage.MC_Func.MC_MovePosition((int)Loader.nAxis.TR_Z,
                                    loader.stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_TR_SafetyPos].LD_Transfer_Z,
                                    m_dSpeed,
                                    m_dSpeed * m_dSpeedMag,
                                    m_dSpeed * m_dSpeedMag);
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Loader Transfer 가 동작중입니다.");
                return;
            }
        }

        private void button_Test_Scanner_AxisZ_SafetyPos_Click(object sender, EventArgs e)
        {
            //  Scanner (Vision) Axis-Z, 대기위치 이동

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (!workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.X) || !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Y) || !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Z) ||
                !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.X) || !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Y) || !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Z))
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Stage 가 이동중입니다.");
                return;
            }

            if ((workStage.m_nLaserDrilling_MainStep == (int)WorkStage.LaserDrilling_Step.None) &&
                (workStage.m_nMainWork_Step == (int)WorkStage.MainWork_Step.None))
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Scanner (Vision) Z축, 대기 위치로 이동하시겠습니까?"))
                    return;

                //  속도
                double m_dSpeed = Equipment.stAxisParam[(int)WorkStage.nAxis.Z].Common_Speed_Coarse;

                //  가감속 배율
                double m_dSpeedMag = 2.0;

                workStage.MC_Func.MC_MovePosition((int)WorkStage.nAxis.Z,
                                    vision.stVisionTeachingPos[(int)Vision_TeachingPosList.Vision_SafetyPos].Vision_Z,
                                    m_dSpeed,
                                    m_dSpeed * m_dSpeedMag,
                                    m_dSpeed * m_dSpeedMag);
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Work Stage 가 동작중입니다.");
                return;
            }
        }
    }
}
