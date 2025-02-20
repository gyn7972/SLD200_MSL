using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using ACS.SPiiPlusNET;
using GCodeNet;
using QMC.Common;
using QMC.Common.Modules;
using QMC.Common.Motion.ACS.Motions;
using QMC.Common.Parts;
using QMC.Common.VisionPart;
using MessageBox = System.Windows.Forms.MessageBox;

namespace SLD200_MSL
{
    public partial class FormNew_Config : Form
    {
        static WorkStage workStage;
        static Loader loader;
        static Unloader unloader;
        static Vision vision;
        static Bds bds;

        private FormNew_KeyPad m_keyPad;
        public System.Windows.Forms.Timer timer_Status;

        private int[] m_nModuleAddrCount;                               //  모듈 별 IO 카운트용 변수
        private int m_nLaserAddrCount = 0;
        private int m_nBDSAddrCount = 0;

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
            loader.Move_Properties_Load();
            workStage.Teaching_Position_Load();
            workStage.Move_Properties_Load();
            vision.Teaching_Position_Load();
            vision.Move_Properties_Load();
            bds.Teaching_Position_Load();
            bds.Move_Properties_Load();

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

            //  Status 타이머
            timer_Status = new System.Windows.Forms.Timer();
            timer_Status.Interval = 1;                                               //  50 이었는데 10으로 변경. (50은 너무 느린 감이 없지 않아 있음. 근데 10에서 잘 될란가...?)
            timer_Status.Tick += new System.EventHandler(Timer_Status_Func);
            timer_Status.Enabled = true;

            //checkedListBox_Config_LDUL_DIO_Input.SetItemChecked(0, true);                     //  IO 상태 표시
        }

        private void Timer_Status_Func(object sender, EventArgs e)
        {
            timer_Status.Enabled = false;

            DIO_Status();
            Motor_Position();

            //m_btimer_MainWork_Stop = false;
            //timer_MainWork.Enabled = false;

            //if (!m_btimer_MainWork_Stop)
            //{
            //    timer_MainWork.Enabled = true;
            //}

            timer_Status.Enabled = true;
        }

        private void DIO_Status()
        {
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
                            ((dioPoint.Address >= 21) && (dioPoint.Address <= 24)))                                     //  WorkStage Output (21 ~ 24)
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
                            (dioPoint.Address == 25))                                                                   //  Laser Output (25)
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

                    if (dioPoint.ModuleNo == 2)                                                                     //  Input
                    {
                        if (dioPoint.Address == 0)
                        {
                            m_nModuleAddrCount[dioPoint.ModuleNo] = 0;
                        }

                        if (((dioPoint.Address >= 0) && (dioPoint.Address <= 10)) ||                                    //  Loader Input (0 ~ 10)
                            ((dioPoint.Address >= 16) && (dioPoint.Address <= 24)))                                     //  Unloader Input (16 ~ 24)
                        {
                            if (dioPoint.GetValue() == DioValue.On)
                            {
                                checkedListBox_Config_LDUL_DIO_Input.SetItemChecked(m_nModuleAddrCount[dioPoint.ModuleNo], true);
                            }
                            else
                            {
                                checkedListBox_Config_LDUL_DIO_Input.SetItemChecked(m_nModuleAddrCount[dioPoint.ModuleNo], false);
                            }

                            m_nModuleAddrCount[dioPoint.ModuleNo]++;
                        }
                    }

                    if (dioPoint.ModuleNo == 3)                                                                     //  Output
                    {
                        if (dioPoint.Address == 0)
                        {
                            m_nModuleAddrCount[dioPoint.ModuleNo] = 0;
                        }

                        if (((dioPoint.Address >= 0) && (dioPoint.Address <= 7)) ||                                     //  Loader Output (0 ~ 7)
                            ((dioPoint.Address >= 16) && (dioPoint.Address <= 18)))                                     //  Unloader Output (16 ~ 18)
                        {
                            if (dioPoint.GetValue() == DioValue.On)
                            {
                                checkedListBox_Config_LDUL_DIO_Output.SetItemChecked(m_nModuleAddrCount[dioPoint.ModuleNo], true);
                            }
                            else
                            {
                                checkedListBox_Config_LDUL_DIO_Output.SetItemChecked(m_nModuleAddrCount[dioPoint.ModuleNo], false);
                            }

                            m_nModuleAddrCount[dioPoint.ModuleNo]++;
                        }
                    }
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

            //  Scanner & Camera Position
            label_Config_EncPosition_SCANNER_Z.Text = string.Format("{0:F3}", workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Z));

            //  Mask Position
            label_Config_EncPosition_MASK_Y.Text = string.Format("{0:F3}", workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.MASK_Y));


            //  Unloader Stacker Position
            label_Config_EncPosition_UL_Z0.Text = string.Format("{0:F3}", unloader.MC_Func.MC_GetEncPos((int)Unloader.nAxis.Z0));
            label_Config_EncPosition_UL_Z1.Text = string.Format("{0:F3}", unloader.MC_Func.MC_GetEncPos((int)Unloader.nAxis.Z1));

            //  Unloader Transfer Position
            label_Config_EncPosition_UL_TRX.Text = string.Format("{0:F3}", unloader.MC_Func.MC_GetEncPos((int)Unloader.nAxis.TR_X));
            label_Config_EncPosition_UL_TRZ.Text = string.Format("{0:F3}", unloader.MC_Func.MC_GetEncPos((int)Unloader.nAxis.TR_Z));
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

            //  0 (0) : Loader Transfer Inner Vacuum On
            //  1 (1) : Loader Transfer Outer Vacuum On
            //  2 (2) : Loader Transfer Air Blow On
            //  3 (3) : M - Aligner Center Vacuum On
            //  4 (4) : M - Aligner Inner Vacuum On
            //  5 (5) : M - Aligner Outer Vacuum On
            //  6 (6) : M - Aligner Air Blow On
            //  7 (7) : Loader Port Ionizer On
            //  8 (16) : Unloader Tansfer Inner Vacuum On
            //  9 (17) : Unloader Tansfer Outer Vacuum On
            //  10 (18) : Unloader Tansfer Air Blow On

            DioPoint dioPoint;

            if ((m_nIndex >= 0) && (m_nIndex <= 7))                                                         //  Loader Output (0 ~ 7)
            {
                foreach (DioPoint point in Equipment.GetAllDioPointList())
                {
                    dioPoint = point;

                    if (dioPoint == null)
                        return;

                    //  0번부터 시작하므로 그대로 사용
                    m_nOutputChannel = m_nIndex;

                    if ((dioPoint.ModuleNo == 3) && (dioPoint.Address == m_nOutputChannel))
                    {
                        if (m_bCurStatus)
                            dioPoint.Write(DioValue.Off);
                        else
                            dioPoint.Write(DioValue.On);

                        break;
                    }
                }
            }
            else if ((m_nIndex >= 8) && (m_nIndex <= 10))                                                   //  Unloader Output (16 ~ 18)
            {
                foreach (DioPoint point in Equipment.GetAllDioPointList())
                {
                    dioPoint = point;

                    if (dioPoint == null)
                        return;

                    //  16번부터 시작하므로 변환하여 사용 (8일 때 16과 같음)
                    m_nOutputChannel = m_nIndex + 8;

                    if ((dioPoint.ModuleNo == 3) && (dioPoint.Address == m_nOutputChannel))
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
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.TR_X].Jog_Speed_Fine;
                        else
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.TR_X].Jog_Speed_Coarse;

                        //  방향 설정
                        if (lfVelocity > 0) lfVelocity = lfVelocity * (-1.0);     // Negative direction : Using - (minus) velocity

                        loader.MC_Func.MC_JogMove((int)Loader.nAxis.TR_X, lfVelocity, lfVelocity * 100.0, lfVelocity * 100.0);
                    }
                    else
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.TR_X].Jog_Speed_Fine;
                        else
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.TR_X].Jog_Speed_Coarse;

                        //  방향 설정
                        if (lfVelocity > 0) lfVelocity = lfVelocity * (-1.0);     // Negative direction : Using - (minus) velocity

                        unloader.MC_Func.MC_JogMove((int)Unloader.nAxis.TR_X, lfVelocity, lfVelocity * 100.0, lfVelocity * 100.0);
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
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.TR_X].Jog_Speed_Fine;
                        else
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.TR_X].Jog_Speed_Coarse;

                        //  방향 설정
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Positive direction : Using + (plus) velocity

                        loader.MC_Func.MC_JogMove((int)Loader.nAxis.TR_X, lfVelocity, lfVelocity * 100.0, lfVelocity * 100.0);
                    }
                    else
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.TR_X].Jog_Speed_Fine;
                        else
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.TR_X].Jog_Speed_Coarse;

                        //  방향 설정
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Positive direction : Using + (plus) velocity

                        unloader.MC_Func.MC_JogMove((int)Unloader.nAxis.TR_X, lfVelocity, lfVelocity * 100.0, lfVelocity * 100.0);
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
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.TR_Z].Jog_Speed_Fine;
                        else
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.TR_Z].Jog_Speed_Coarse;

                        //  방향 설정
                        if (lfVelocity > 0) lfVelocity = lfVelocity * (-1.0);     // Negative direction : Using - (minus) velocity

                        loader.MC_Func.MC_JogMove((int)Loader.nAxis.TR_Z, lfVelocity, lfVelocity * 100.0, lfVelocity * 100.0);
                    }
                    else
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.TR_Z].Jog_Speed_Fine;
                        else
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.TR_Z].Jog_Speed_Coarse;

                        //  방향 설정
                        if (lfVelocity > 0) lfVelocity = lfVelocity * (-1.0);     // Negative direction : Using - (minus) velocity

                        unloader.MC_Func.MC_JogMove((int)Unloader.nAxis.TR_Z, lfVelocity, lfVelocity * 100.0, lfVelocity * 100.0);
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
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.TR_Z].Jog_Speed_Fine;
                        else
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.TR_Z].Jog_Speed_Coarse;

                        //  방향 설정
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Positive direction : Using + (plus) velocity

                        loader.MC_Func.MC_JogMove((int)Loader.nAxis.TR_Z, lfVelocity, lfVelocity * 100.0, lfVelocity * 100.0);
                    }
                    else
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.TR_Z].Jog_Speed_Fine;
                        else
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.TR_Z].Jog_Speed_Coarse;

                        //  방향 설정
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Positive direction : Using + (plus) velocity

                        unloader.MC_Func.MC_JogMove((int)Unloader.nAxis.TR_Z, lfVelocity, lfVelocity * 100.0, lfVelocity * 100.0);
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
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.Z0].Jog_Speed_Fine;
                        else
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.Z0].Jog_Speed_Coarse;

                        //  방향 설정
                        if (lfVelocity > 0) lfVelocity = lfVelocity * (-1.0);     // Negative direction : Using - (minus) velocity

                        loader.MC_Func.MC_JogMove((int)Loader.nAxis.Z0, lfVelocity, lfVelocity * 100.0, lfVelocity * 100.0);
                    }
                    else
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.Z0].Jog_Speed_Fine;
                        else
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.Z0].Jog_Speed_Coarse;

                        //  방향 설정
                        if (lfVelocity > 0) lfVelocity = lfVelocity * (-1.0);     // Negative direction : Using - (minus) velocity

                        unloader.MC_Func.MC_JogMove((int)Unloader.nAxis.Z0, lfVelocity, lfVelocity * 100.0, lfVelocity * 100.0);
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
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.Z0].Jog_Speed_Fine;
                        else
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.Z0].Jog_Speed_Coarse;

                        //  방향 설정
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Positive direction : Using + (plus) velocity

                        loader.MC_Func.MC_JogMove((int)Loader.nAxis.Z0, lfVelocity, lfVelocity * 100.0, lfVelocity * 100.0);
                    }
                    else
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.Z0].Jog_Speed_Fine;
                        else
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.Z0].Jog_Speed_Coarse;

                        //  방향 설정
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Positive direction : Using + (plus) velocity

                        unloader.MC_Func.MC_JogMove((int)Unloader.nAxis.Z0, lfVelocity, lfVelocity * 100.0, lfVelocity * 100.0);
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
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.Z1].Jog_Speed_Fine;
                        else
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.Z1].Jog_Speed_Coarse;

                        //  방향 설정
                        if (lfVelocity > 0) lfVelocity = lfVelocity * (-1.0);     // Negative direction : Using - (minus) velocity

                        loader.MC_Func.MC_JogMove((int)Loader.nAxis.Z1, lfVelocity, lfVelocity * 100.0, lfVelocity * 100.0);
                    }
                    else
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.Z1].Jog_Speed_Fine;
                        else
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.Z1].Jog_Speed_Coarse;

                        //  방향 설정
                        if (lfVelocity > 0) lfVelocity = lfVelocity * (-1.0);     // Negative direction : Using - (minus) velocity

                        unloader.MC_Func.MC_JogMove((int)Unloader.nAxis.Z1, lfVelocity, lfVelocity * 100.0, lfVelocity * 100.0);
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
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.Z1].Jog_Speed_Fine;
                        else
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.Z1].Jog_Speed_Coarse;

                        //  방향 설정
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Positive direction : Using + (plus) velocity

                        loader.MC_Func.MC_JogMove((int)Loader.nAxis.Z1, lfVelocity, lfVelocity * 100.0, lfVelocity * 100.0);
                    }
                    else
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.Z1].Jog_Speed_Fine;
                        else
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.Z1].Jog_Speed_Coarse;

                        //  방향 설정
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Positive direction : Using + (plus) velocity

                        unloader.MC_Func.MC_JogMove((int)Unloader.nAxis.Z1, lfVelocity, lfVelocity * 100.0, lfVelocity * 100.0);
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

            if (!this.radioButton_Config_LDUL_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                        lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.ALN_X].Jog_Speed_Fine;
                    else
                        lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.ALN_X].Jog_Speed_Coarse;

                    //  방향 설정
                    if (lfVelocity > 0) lfVelocity = lfVelocity * (-1.0);     // Negative direction : Using - (minus) velocity

                    loader.MC_Func.MC_JogMove((int)Loader.nAxis.ALN_X, lfVelocity, lfVelocity * 100.0, lfVelocity * 100.0);
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

            if (!this.radioButton_Config_LDUL_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                        lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.ALN_X].Jog_Speed_Fine;
                    else
                        lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.ALN_X].Jog_Speed_Coarse;

                    //  방향 설정
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Positive direction : Using + (plus) velocity

                    loader.MC_Func.MC_JogMove((int)Loader.nAxis.ALN_X, lfVelocity, lfVelocity * 100.0, lfVelocity * 100.0);
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

            if (!this.radioButton_Config_LDUL_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                        lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.ALN_Y].Jog_Speed_Fine;
                    else
                        lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.ALN_Y].Jog_Speed_Coarse;

                    //  방향 설정
                    if (lfVelocity > 0) lfVelocity = lfVelocity * (-1.0);     // Negative direction : Using - (minus) velocity

                    loader.MC_Func.MC_JogMove((int)Loader.nAxis.ALN_Y, lfVelocity, lfVelocity * 100.0, lfVelocity * 100.0);
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

            if (!this.radioButton_Config_LDUL_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                        lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.ALN_Y].Jog_Speed_Fine;
                    else
                        lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.ALN_Y].Jog_Speed_Coarse;

                    //  방향 설정
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Positive direction : Using + (plus) velocity

                    loader.MC_Func.MC_JogMove((int)Loader.nAxis.ALN_Y, lfVelocity, lfVelocity * 100.0, lfVelocity * 100.0);
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
                loader.MC_Func.MC_JogStop((int)Loader.nAxis.Z0);
                loader.MC_Func.MC_JogStop((int)Loader.nAxis.Z1);
                loader.MC_Func.MC_JogStop((int)Loader.nAxis.TR_X);
                loader.MC_Func.MC_JogStop((int)Loader.nAxis.TR_Z);
                loader.MC_Func.MC_JogStop((int)Loader.nAxis.ALN_X);
                loader.MC_Func.MC_JogStop((int)Loader.nAxis.ALN_Y);
            }
        }

        private void button_Config_LDUL_TRX_Neg_Click(object sender, EventArgs e)
        {
            if (!this.radioButton_Config_LDUL_JogMove_Step.Checked)
                return;

            double lfVelocity = 0.0f;
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
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.TR_X].Jog_Speed_Fine;
                        else
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.TR_X].Jog_Speed_Coarse;

                        //  속도는 양수로
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                        loader.MC_Func.MC_MoveRelPosition((int)Loader.nAxis.TR_X, dDistance * (double)nDirection, lfVelocity, lfVelocity * 2.0, lfVelocity * 2.0);
                    }
                    else
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.TR_X].Jog_Speed_Fine;
                        else
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.TR_X].Jog_Speed_Coarse;

                        //  속도는 양수로
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                        unloader.MC_Func.MC_MoveRelPosition((int)Unloader.nAxis.TR_X, dDistance * (double)nDirection, lfVelocity, lfVelocity * 2.0, lfVelocity * 2.0);
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
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.TR_X].Jog_Speed_Fine;
                        else
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.TR_X].Jog_Speed_Coarse;

                        //  속도는 양수로
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                        loader.MC_Func.MC_MoveRelPosition((int)Loader.nAxis.TR_X, dDistance * (double)nDirection, lfVelocity, lfVelocity * 2.0, lfVelocity * 2.0);
                    }
                    else
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.TR_X].Jog_Speed_Fine;
                        else
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.TR_X].Jog_Speed_Coarse;

                        //  속도는 양수로
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                        unloader.MC_Func.MC_MoveRelPosition((int)Unloader.nAxis.TR_X, dDistance * (double)nDirection, lfVelocity, lfVelocity * 2.0, lfVelocity * 2.0);
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
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.TR_Z].Jog_Speed_Fine;
                        else
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.TR_Z].Jog_Speed_Coarse;

                        //  속도는 양수로
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                        loader.MC_Func.MC_MoveRelPosition((int)Loader.nAxis.TR_Z, dDistance * (double)nDirection, lfVelocity, lfVelocity * 2.0, lfVelocity * 2.0);
                    }
                    else
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.TR_Z].Jog_Speed_Fine;
                        else
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.TR_Z].Jog_Speed_Coarse;

                        //  속도는 양수로
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                        unloader.MC_Func.MC_MoveRelPosition((int)Unloader.nAxis.TR_Z, dDistance * (double)nDirection, lfVelocity, lfVelocity * 2.0, lfVelocity * 2.0);
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
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.TR_Z].Jog_Speed_Fine;
                        else
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.TR_Z].Jog_Speed_Coarse;

                        //  속도는 양수로
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                        loader.MC_Func.MC_MoveRelPosition((int)Loader.nAxis.TR_Z, dDistance * (double)nDirection, lfVelocity, lfVelocity * 2.0, lfVelocity * 2.0);
                    }
                    else
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.TR_Z].Jog_Speed_Fine;
                        else
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.TR_Z].Jog_Speed_Coarse;

                        //  속도는 양수로
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                        unloader.MC_Func.MC_MoveRelPosition((int)Unloader.nAxis.TR_Z, dDistance * (double)nDirection, lfVelocity, lfVelocity * 2.0, lfVelocity * 2.0);
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
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.Z0].Jog_Speed_Fine;
                        else
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.Z0].Jog_Speed_Coarse;

                        //  속도는 양수로
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                        loader.MC_Func.MC_MoveRelPosition((int)Loader.nAxis.Z0, dDistance * (double)nDirection, lfVelocity, lfVelocity * 2.0, lfVelocity * 2.0);
                    }
                    else
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.Z0].Jog_Speed_Fine;
                        else
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.Z0].Jog_Speed_Coarse;

                        //  속도는 양수로
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                        unloader.MC_Func.MC_MoveRelPosition((int)Unloader.nAxis.Z0, dDistance * (double)nDirection, lfVelocity, lfVelocity * 2.0, lfVelocity * 2.0);
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
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.Z0].Jog_Speed_Fine;
                        else
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.Z0].Jog_Speed_Coarse;

                        //  속도는 양수로
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                        loader.MC_Func.MC_MoveRelPosition((int)Loader.nAxis.Z0, dDistance * (double)nDirection, lfVelocity, lfVelocity * 2.0, lfVelocity * 2.0);
                    }
                    else
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.Z0].Jog_Speed_Fine;
                        else
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.Z0].Jog_Speed_Coarse;

                        //  속도는 양수로
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                        unloader.MC_Func.MC_MoveRelPosition((int)Unloader.nAxis.Z0, dDistance * (double)nDirection, lfVelocity, lfVelocity * 2.0, lfVelocity * 2.0);
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
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.Z1].Jog_Speed_Fine;
                        else
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.Z1].Jog_Speed_Coarse;

                        //  속도는 양수로
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                        loader.MC_Func.MC_MoveRelPosition((int)Loader.nAxis.Z1, dDistance * (double)nDirection, lfVelocity, lfVelocity * 2.0, lfVelocity * 2.0);
                    }
                    else
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.Z1].Jog_Speed_Fine;
                        else
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.Z1].Jog_Speed_Coarse;

                        //  속도는 양수로
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                        unloader.MC_Func.MC_MoveRelPosition((int)Unloader.nAxis.Z1, dDistance * (double)nDirection, lfVelocity, lfVelocity * 2.0, lfVelocity * 2.0);
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
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.Z1].Jog_Speed_Fine;
                        else
                            lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.Z1].Jog_Speed_Coarse;

                        //  속도는 양수로
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                        loader.MC_Func.MC_MoveRelPosition((int)Loader.nAxis.Z1, dDistance * (double)nDirection, lfVelocity, lfVelocity * 2.0, lfVelocity * 2.0);
                    }
                    else
                    {
                        //  속도 설정
                        if (radioButton_Config_LDUL_Move_MoveMode_Fine.Checked)
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.Z1].Jog_Speed_Fine;
                        else
                            lfVelocity = Equipment.stAxisParam[(int)Unloader.nAxis.Z1].Jog_Speed_Coarse;

                        //  속도는 양수로
                        if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                        unloader.MC_Func.MC_MoveRelPosition((int)Unloader.nAxis.Z1, dDistance * (double)nDirection, lfVelocity, lfVelocity * 2.0, lfVelocity * 2.0);
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

            double lfVelocity = 0.0f;
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
                        lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.ALN_X].Jog_Speed_Fine;
                    else
                        lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.ALN_X].Jog_Speed_Coarse;

                    //  속도는 양수로
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                    loader.MC_Func.MC_MoveRelPosition((int)Loader.nAxis.ALN_X, dDistance * (double)nDirection, lfVelocity, lfVelocity * 2.0, lfVelocity * 2.0);
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

            double lfVelocity = 0.0f;
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
                        lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.ALN_X].Jog_Speed_Fine;
                    else
                        lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.ALN_X].Jog_Speed_Coarse;

                    //  속도는 양수로
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                    loader.MC_Func.MC_MoveRelPosition((int)Loader.nAxis.ALN_X, dDistance * (double)nDirection, lfVelocity, lfVelocity * 2.0, lfVelocity * 2.0);
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

            double lfVelocity = 0.0f;
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
                        lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.ALN_Y].Jog_Speed_Fine;
                    else
                        lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.ALN_Y].Jog_Speed_Coarse;

                    //  속도는 양수로
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                    loader.MC_Func.MC_MoveRelPosition((int)Loader.nAxis.ALN_Y, dDistance * (double)nDirection, lfVelocity, lfVelocity * 2.0, lfVelocity * 2.0);
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

            double lfVelocity = 0.0f;
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
                        lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.ALN_Y].Jog_Speed_Fine;
                    else
                        lfVelocity = Equipment.stAxisParam[(int)Loader.nAxis.ALN_Y].Jog_Speed_Coarse;

                    //  속도는 양수로
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                    loader.MC_Func.MC_MoveRelPosition((int)Loader.nAxis.ALN_Y, dDistance * (double)nDirection, lfVelocity, lfVelocity * 2.0, lfVelocity * 2.0);
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

            if (!this.radioButton_Config_WorkStage_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_Config_WorkStage_Move_MoveMode_Fine.Checked)
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Fine;
                    else
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Coarse;

                    //  방향 설정
                    if (lfVelocity > 0) lfVelocity = lfVelocity * (-1.0);     // Negative direction : Using - (minus) velocity

                    workStage.MC_Func.MC_JogMove((int)WorkStage.nAxis.X, lfVelocity, lfVelocity * 100.0, lfVelocity * 100.0);
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

            if (!this.radioButton_Config_WorkStage_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_Config_WorkStage_Move_MoveMode_Fine.Checked)
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Fine;
                    else
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Coarse;

                    //  방향 설정
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Positive direction : Using + (plus) velocity

                    workStage.MC_Func.MC_JogMove((int)WorkStage.nAxis.X, lfVelocity, lfVelocity * 100.0, lfVelocity * 100.0);
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

            if (!this.radioButton_Config_WorkStage_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_Config_WorkStage_Move_MoveMode_Fine.Checked)
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.Y].Jog_Speed_Fine;
                    else
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.Y].Jog_Speed_Coarse;

                    //  방향 설정
                    if (lfVelocity > 0) lfVelocity = lfVelocity * (-1.0);     // Negative direction : Using - (minus) velocity

                    workStage.MC_Func.MC_JogMove((int)WorkStage.nAxis.Y, lfVelocity, lfVelocity * 100.0, lfVelocity * 100.0);
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

            if (!this.radioButton_Config_WorkStage_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_Config_WorkStage_Move_MoveMode_Fine.Checked)
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.Y].Jog_Speed_Fine;
                    else
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.Y].Jog_Speed_Coarse;

                    //  방향 설정
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Positive direction : Using + (plus) velocity

                    workStage.MC_Func.MC_JogMove((int)WorkStage.nAxis.Y, lfVelocity, lfVelocity * 100.0, lfVelocity * 100.0);
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

            if (!this.radioButton_Config_WorkStage_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_Config_WorkStage_Move_MoveMode_Fine.Checked)
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.Z].Jog_Speed_Fine;
                    else
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.Z].Jog_Speed_Coarse;

                    //  방향 설정
                    if (lfVelocity > 0) lfVelocity = lfVelocity * (-1.0);     // Negative direction : Using - (minus) velocity

                    workStage.MC_Func.MC_JogMove((int)WorkStage.nAxis.Z, lfVelocity, lfVelocity * 100.0, lfVelocity * 100.0);
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

            if (!this.radioButton_Config_WorkStage_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_Config_WorkStage_Move_MoveMode_Fine.Checked)
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.Z].Jog_Speed_Fine;
                    else
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.Z].Jog_Speed_Coarse;

                    //  방향 설정
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Positive direction : Using + (plus) velocity

                    workStage.MC_Func.MC_JogMove((int)WorkStage.nAxis.Z, lfVelocity, lfVelocity * 100.0, lfVelocity * 100.0);
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
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Fine;
                    else
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Coarse;

                    //  속도는 양수로
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                    workStage.MC_Func.MC_MoveRelPosition((int)WorkStage.nAxis.X, dDistance * (double)nDirection, lfVelocity, lfVelocity * 2.0, lfVelocity * 2.0);
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
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Fine;
                    else
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.X].Jog_Speed_Coarse;

                    //  속도는 양수로
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                    workStage.MC_Func.MC_MoveRelPosition((int)WorkStage.nAxis.X, dDistance * (double)nDirection, lfVelocity, lfVelocity * 2.0, lfVelocity * 2.0);
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
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.Y].Jog_Speed_Fine;
                    else
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.Y].Jog_Speed_Coarse;

                    //  속도는 양수로
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                    workStage.MC_Func.MC_MoveRelPosition((int)WorkStage.nAxis.Y, dDistance * (double)nDirection, lfVelocity, lfVelocity * 2.0, lfVelocity * 2.0);
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
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.Y].Jog_Speed_Fine;
                    else
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.Y].Jog_Speed_Coarse;

                    //  속도는 양수로
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                    workStage.MC_Func.MC_MoveRelPosition((int)WorkStage.nAxis.Y, dDistance * (double)nDirection, lfVelocity, lfVelocity * 2.0, lfVelocity * 2.0);
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
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.Z].Jog_Speed_Fine;
                    else
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.Z].Jog_Speed_Coarse;

                    //  속도는 양수로
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                    workStage.MC_Func.MC_MoveRelPosition((int)WorkStage.nAxis.Z, dDistance * (double)nDirection, lfVelocity, lfVelocity * 2.0, lfVelocity * 2.0);
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
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.Z].Jog_Speed_Fine;
                    else
                        lfVelocity = Equipment.stAxisParam[(int)WorkStage.nAxis.Z].Jog_Speed_Coarse;

                    //  속도는 양수로
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                    workStage.MC_Func.MC_MoveRelPosition((int)WorkStage.nAxis.Z, dDistance * (double)nDirection, lfVelocity, lfVelocity * 2.0, lfVelocity * 2.0);
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

            if (!this.radioButton_Config_Vision_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_Config_Vision_Move_MoveMode_Fine.Checked)
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.X].Jog_Speed_Fine;
                    else
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.X].Jog_Speed_Coarse;

                    //  방향 설정
                    if (lfVelocity > 0) lfVelocity = lfVelocity * (-1.0);     // Negative direction : Using - (minus) velocity

                    vision.MC_Func.MC_JogMove((int)Vision.nAxis.X, lfVelocity, lfVelocity * 100.0, lfVelocity * 100.0);
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

            if (!this.radioButton_Config_Vision_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_Config_Vision_Move_MoveMode_Fine.Checked)
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.X].Jog_Speed_Fine;
                    else
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.X].Jog_Speed_Coarse;

                    //  방향 설정
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Positive direction : Using + (plus) velocity

                    vision.MC_Func.MC_JogMove((int)Vision.nAxis.X, lfVelocity, lfVelocity * 100.0, lfVelocity * 100.0);
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

            if (!this.radioButton_Config_Vision_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_Config_Vision_Move_MoveMode_Fine.Checked)
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.Y].Jog_Speed_Fine;
                    else
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.Y].Jog_Speed_Coarse;

                    //  방향 설정
                    if (lfVelocity > 0) lfVelocity = lfVelocity * (-1.0);     // Negative direction : Using - (minus) velocity

                    vision.MC_Func.MC_JogMove((int)Vision.nAxis.Y, lfVelocity, lfVelocity * 100.0, lfVelocity * 100.0);
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

            if (!this.radioButton_Config_Vision_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_Config_Vision_Move_MoveMode_Fine.Checked)
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.Y].Jog_Speed_Fine;
                    else
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.Y].Jog_Speed_Coarse;

                    //  방향 설정
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Positive direction : Using + (plus) velocity

                    vision.MC_Func.MC_JogMove((int)Vision.nAxis.Y, lfVelocity, lfVelocity * 100.0, lfVelocity * 100.0);
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

            if (!this.radioButton_Config_Vision_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_Config_Vision_Move_MoveMode_Fine.Checked)
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.Z].Jog_Speed_Fine;
                    else
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.Z].Jog_Speed_Coarse;

                    //  방향 설정
                    if (lfVelocity > 0) lfVelocity = lfVelocity * (-1.0);     // Negative direction : Using - (minus) velocity

                    vision.MC_Func.MC_JogMove((int)Vision.nAxis.Z, lfVelocity, lfVelocity * 100.0, lfVelocity * 100.0);
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

            if (!this.radioButton_Config_Vision_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_Config_Vision_Move_MoveMode_Fine.Checked)
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.Z].Jog_Speed_Fine;
                    else
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.Z].Jog_Speed_Coarse;

                    //  방향 설정
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Positive direction : Using + (plus) velocity

                    vision.MC_Func.MC_JogMove((int)Vision.nAxis.Z, lfVelocity, lfVelocity * 100.0, lfVelocity * 100.0);
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
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.X].Jog_Speed_Fine;
                    else
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.X].Jog_Speed_Coarse;

                    //  속도는 양수로
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                    vision.MC_Func.MC_MoveRelPosition((int)Vision.nAxis.X, dDistance * (double)nDirection, lfVelocity, lfVelocity * 2.0, lfVelocity * 2.0);
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
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.X].Jog_Speed_Fine;
                    else
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.X].Jog_Speed_Coarse;

                    //  속도는 양수로
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                    vision.MC_Func.MC_MoveRelPosition((int)Vision.nAxis.X, dDistance * (double)nDirection, lfVelocity, lfVelocity * 2.0, lfVelocity * 2.0);
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
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.Y].Jog_Speed_Fine;
                    else
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.Y].Jog_Speed_Coarse;

                    //  속도는 양수로
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                    vision.MC_Func.MC_MoveRelPosition((int)Vision.nAxis.Y, dDistance * (double)nDirection, lfVelocity, lfVelocity * 2.0, lfVelocity * 2.0);
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
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.Y].Jog_Speed_Fine;
                    else
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.Y].Jog_Speed_Coarse;

                    //  속도는 양수로
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                    vision.MC_Func.MC_MoveRelPosition((int)Vision.nAxis.Y, dDistance * (double)nDirection, lfVelocity, lfVelocity * 2.0, lfVelocity * 2.0);
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
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.Z].Jog_Speed_Fine;
                    else
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.Z].Jog_Speed_Coarse;

                    //  속도는 양수로
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                    vision.MC_Func.MC_MoveRelPosition((int)Vision.nAxis.Z, dDistance * (double)nDirection, lfVelocity, lfVelocity * 2.0, lfVelocity * 2.0);
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
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.Z].Jog_Speed_Fine;
                    else
                        lfVelocity = Equipment.stAxisParam[(int)Vision.nAxis.Z].Jog_Speed_Coarse;

                    //  속도는 양수로
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);

                    vision.MC_Func.MC_MoveRelPosition((int)Vision.nAxis.Z, dDistance * (double)nDirection, lfVelocity, lfVelocity * 2.0, lfVelocity * 2.0);
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

            if (!this.radioButton_Config_BDS_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_Config_BDS_Move_MoveMode_Fine.Checked)
                        lfVelocity = Equipment.stAxisParam[(int)Bds.nAxis.MASK_Y].Jog_Speed_Fine;
                    else
                        lfVelocity = Equipment.stAxisParam[(int)Bds.nAxis.MASK_Y].Jog_Speed_Coarse;

                    //  방향 설정
                    if (lfVelocity > 0) lfVelocity = lfVelocity * (-1.0);     // Negative direction : Using - (minus) velocity

                    bds.MC_Func.MC_JogMove((int)Bds.nAxis.MASK_Y, lfVelocity, lfVelocity * 100.0, lfVelocity * 100.0);
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

            if (!this.radioButton_Config_BDS_JogMove_Continuous.Checked)
                return;

            try
            {
                if (Equipment.AjinBoard_Opened)
                {
                    //  속도 설정
                    if (radioButton_Config_BDS_Move_MoveMode_Fine.Checked)
                        lfVelocity = Equipment.stAxisParam[(int)Bds.nAxis.MASK_Y].Jog_Speed_Fine;
                    else
                        lfVelocity = Equipment.stAxisParam[(int)Bds.nAxis.MASK_Y].Jog_Speed_Coarse;

                    //  방향 설정
                    if (lfVelocity < 0) lfVelocity = lfVelocity * (-1.0);     // Positive direction : Using + (plus) velocity

                    bds.MC_Func.MC_JogMove((int)Bds.nAxis.MASK_Y, lfVelocity, lfVelocity * 100.0, lfVelocity * 100.0);
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
                if (m_nIndex <= 10)                 //  Loader
                {
                    Loader.stLDULTeachingPos[m_nIndex].LD_Transfer_X = Convert.ToDouble(textBox_Config_LDUL_TeachingPos_TransferX.Text);
                    Loader.stLDULTeachingPos[m_nIndex].LD_Transfer_Z = Convert.ToDouble(textBox_Config_LDUL_TeachingPos_TransferZ.Text);
                    Loader.stLDULTeachingPos[m_nIndex].LD_Stacker_Z0 = Convert.ToDouble(textBox_Config_LDUL_TeachingPos_RPortZ.Text);
                    Loader.stLDULTeachingPos[m_nIndex].LD_Stacker_Z1 = Convert.ToDouble(textBox_Config_LDUL_TeachingPos_LPortZ.Text);
                    Loader.stLDULTeachingPos[m_nIndex].MAligner_X = Convert.ToDouble(textBox_Config_LDUL_TeachingPos_MAlignerX.Text);
                    Loader.stLDULTeachingPos[m_nIndex].MAligner_Y = Convert.ToDouble(textBox_Config_LDUL_TeachingPos_MAlignerY.Text);
                }
                else                                //  Unloader
                {
                    Loader.stLDULTeachingPos[m_nIndex].UL_Transfer_X = Convert.ToDouble(textBox_Config_LDUL_TeachingPos_TransferX.Text);
                    Loader.stLDULTeachingPos[m_nIndex].UL_Transfer_Z = Convert.ToDouble(textBox_Config_LDUL_TeachingPos_TransferZ.Text);
                    Loader.stLDULTeachingPos[m_nIndex].UL_Stacker_Z0 = Convert.ToDouble(textBox_Config_LDUL_TeachingPos_RPortZ.Text);
                    Loader.stLDULTeachingPos[m_nIndex].UL_Stacker_Z1 = Convert.ToDouble(textBox_Config_LDUL_TeachingPos_LPortZ.Text);
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
                if (m_nIndex <= 10)                 //  Loader
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
                    textBox_Config_LDUL_TeachingPos_TransferX.Text = Loader.stLDULTeachingPos[m_nIndex].LD_Transfer_X.ToString();
                    textBox_Config_LDUL_TeachingPos_TransferZ.Text = Loader.stLDULTeachingPos[m_nIndex].LD_Transfer_Z.ToString();
                    textBox_Config_LDUL_TeachingPos_RPortZ.Text = Loader.stLDULTeachingPos[m_nIndex].LD_Stacker_Z0.ToString();
                    textBox_Config_LDUL_TeachingPos_LPortZ.Text = Loader.stLDULTeachingPos[m_nIndex].LD_Stacker_Z1.ToString();
                    textBox_Config_LDUL_TeachingPos_MAlignerX.Text = Loader.stLDULTeachingPos[m_nIndex].MAligner_X.ToString();
                    textBox_Config_LDUL_TeachingPos_MAlignerY.Text = Loader.stLDULTeachingPos[m_nIndex].MAligner_Y.ToString();
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
                    textBox_Config_LDUL_TeachingPos_TransferX.Text = Loader.stLDULTeachingPos[m_nIndex].UL_Transfer_X.ToString();
                    textBox_Config_LDUL_TeachingPos_TransferZ.Text = Loader.stLDULTeachingPos[m_nIndex].UL_Transfer_Z.ToString();
                    textBox_Config_LDUL_TeachingPos_RPortZ.Text = Loader.stLDULTeachingPos[m_nIndex].UL_Stacker_Z0.ToString();
                    textBox_Config_LDUL_TeachingPos_LPortZ.Text = Loader.stLDULTeachingPos[m_nIndex].UL_Stacker_Z1.ToString();
                    textBox_Config_LDUL_TeachingPos_MAlignerX.Text = "---";
                    textBox_Config_LDUL_TeachingPos_MAlignerY.Text = "---";
                }
            }
        }

        private void button_Config_WorkStage_TeachingPositions_Save_Click(object sender, EventArgs e)
        {
            //  선택된 축에 대한 데이터 갖다 넣기
            int m_nIndex = listBox_Config_WorkStage_TeachingPositions.SelectedIndex;

            if (m_nIndex >= 0)
            {
                WorkStage.stWorkStageTeachingPos[m_nIndex].Stage_X = Convert.ToDouble(textBox_Config_WorkStage_TeachingPos_StageX.Text);
                WorkStage.stWorkStageTeachingPos[m_nIndex].Stage_Y = Convert.ToDouble(textBox_Config_WorkStage_TeachingPos_StageY.Text);
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
                textBox_Config_WorkStage_TeachingPos_StageX.Text = WorkStage.stWorkStageTeachingPos[m_nIndex].Stage_X.ToString();
                textBox_Config_WorkStage_TeachingPos_StageY.Text = WorkStage.stWorkStageTeachingPos[m_nIndex].Stage_Y.ToString();
            }
        }

        private void button_Config_Vision_TeachingPositions_Save_Click(object sender, EventArgs e)
        {
            //  선택된 축에 대한 데이터 갖다 넣기
            int m_nIndex = listBox_Config_Vision_TeachingPositions.SelectedIndex;

            if (m_nIndex >= 0)
            {
                Vision.stVisionTeachingPos[m_nIndex].Vision_Z = Convert.ToDouble(textBox_Config_Vision_TeachingPos_VisionZ.Text);
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
                textBox_Config_Vision_TeachingPos_VisionZ.Text = Vision.stVisionTeachingPos[m_nIndex].Vision_Z.ToString();
            }
        }

        private void button_Config_BDS_TeachingPositions_Save_Click(object sender, EventArgs e)
        {
            //  선택된 축에 대한 데이터 갖다 넣기
            int m_nIndex = listBox_Config_BDS_TeachingPositions.SelectedIndex;

            if (m_nIndex >= 0)
            {
                Bds.stBDSTeachingPos[m_nIndex].Mask_Y = Convert.ToDouble(textBox_Config_BDS_TeachingPos_MaskY.Text);
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
                textBox_Config_BDS_TeachingPos_MaskY.Text = Bds.stBDSTeachingPos[m_nIndex].Mask_Y.ToString();
            }
        }

        private void button_Config_LDUL_GetCurrentPos_ToTeachingPos_Click(object sender, EventArgs e)
        {
            //  현재 위치값을 티칭 위치값으로 설정 (저장은 아님)
            //  선택 티칭 위치에 따라 세분화 할 필요가 있음. (임시로 로더, 언로더 단위로 값을 설정하도록 한다.)
            int m_nIndex = listBox_Config_LDUL_TeachingPositions.SelectedIndex;

            if (m_nIndex >= 0)
            {
                if (m_nIndex <= 10)                 //  Loader
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
    }
}
