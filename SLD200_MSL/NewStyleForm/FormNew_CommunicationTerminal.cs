using QMC.Common.Modules;
using QMC.Common.Vision.Optics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Forms;
using QMC.Common.Parts;
using QMC.Common;
using System.Runtime.InteropServices;
using static QMC.Common.Equipment;
using SpiralLab.Sirius;
using static QMC.Common.Modules.WorkStage;
using static QMC.Common.Parts.DustCollectorController;
using static System.Windows.Forms.AxHost;
using static SpiralLab.Sirius.JPTTypeE;
using System.Threading;

namespace SLD200_MSL
{
    public partial class FormNew_CommunicationTerminal : Form
    {
        private static FormNew_CommunicationTerminal m_formCommTerminal = null;

        static WorkStage workStage;
        static Bds bds;

        public System.Windows.Forms.Timer timer_Status;

        public int m_nSelectedUnitIndex = 0;
        public int m_nCommPort = 0;
        public string m_strSocketIP = "";
        public int m_nSocketPort = 0;

        private bool m_bPM_BDS_ReadOnce = false;
        private bool m_bPM_BDS_ReadContinuous = false;
        private bool m_bPM_Stage_ReadOnce = false;
        private bool m_bPM_Stage_ReadContinuous = false;

        private bool m_bBET_ReadOnce = false;

        public FormNew_CommunicationTerminal()
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

                if (module.Name == "BDS")
                {
                    bds = module as Bds;
                }
            }

            hScrollBarIlluminator.ValueChanged += new System.EventHandler(hScrollBarIlluminator_ValueChanged);

            //  Status 타이머
            timer_Status = new System.Windows.Forms.Timer();
            timer_Status.Interval = 100;
            timer_Status.Tick += new System.EventHandler(Timer_Status_Func);
        }

        public FormNew_CommunicationTerminal CreateCommTerminal()
        {
            if (m_formCommTerminal == null)
            {
                m_formCommTerminal = new FormNew_CommunicationTerminal();
            }

            return m_formCommTerminal;
        }

        private void Timer_Status_Func(object sender, EventArgs e)
        {
            //  동시에 진행되지 않는 함수들만 동일한 타이머로 한다.

            timer_Status.Enabled = false;

            bool m_bTemp = false;

            if ((m_nSelectedUnitIndex < 0) ||
                (m_nSelectedUnitIndex > System.Enum.GetValues(typeof(CommList)).Length))
                return;

            switch (m_nSelectedUnitIndex)
            {
                case 0:
                    if (CommonModule.Instance.Illuminator == null)
                    {
                        label_CommTerminal_LinkStatus.Text = "Not Connected";
                    }
                    else
                    {
                        label_CommTerminal_LinkStatus.Text = CommonModule.Instance.Illuminator.m_bIsOpen ? "Connected" : "Disconnected";
                    }
                    break;

                case 1:
                    if (workStage.m_powerMeter_ExitPos_Comm == null)
                    {
                        label_CommTerminal_LinkStatus.Text = "Not Connected";
                    }
                    else
                    {
                        label_CommTerminal_LinkStatus.Text = workStage.m_powerMeter_ExitPos_Comm.IsOpen ? "Connected" : "Disconnected";

                        //  BDS Power Meter 읽었으면?
                        if (m_bPM_BDS_ReadOnce)
                        {
                            m_bPM_BDS_ReadOnce = false;

                            label_CommunicationTerminal_ReceivedData.Text = string.Format("BDS Power Value : {0:0.00000}", workStage.m_dPowerMeterBDS_Value);
                        }
                        else if (m_bPM_BDS_ReadContinuous)
                        {
                            label_CommunicationTerminal_ReceivedData.Text = string.Format("BDS Power Value : {0:0.00000}", workStage.m_dPowerMeterBDS_Value);
                        }
                    }
                    break;

                case 2:
                    if (workStage.m_powerMeter_TargetPos_Comm == null)
                    {
                        label_CommTerminal_LinkStatus.Text = "Not Connected";
                    }
                    else
                    {
                        label_CommTerminal_LinkStatus.Text = workStage.m_powerMeter_TargetPos_Comm.IsOpen ? "Connected" : "Disconnected";

                        //  Stage Power Meter 읽었으면?
                        if (m_bPM_Stage_ReadOnce)
                        {
                            m_bPM_Stage_ReadOnce = false;

                            label_CommunicationTerminal_ReceivedData.Text = string.Format("Stage Power Value : {0:0.00000}", workStage.m_dPowerMeterStage_Value);
                        }
                        else if (m_bPM_Stage_ReadContinuous)
                        {
                            label_CommunicationTerminal_ReceivedData.Text = string.Format("Stage Power Value : {0:0.00000}", workStage.m_dPowerMeterStage_Value);
                        }
                    }
                    break;

                case 3:
                    if (workStage.m_beamExpander_Comm == null)
                    {
                        label_CommTerminal_LinkStatus.Text = "Not Connected";
                    }
                    else
                    {
                        label_CommTerminal_LinkStatus.Text = workStage.m_beamExpander_Comm.IsOpen ? "Connected" : "Disconnected";

                        //  Beam Expander 읽었으면? 
                        if (m_bBET_ReadOnce)
                        {
                            m_bBET_ReadOnce = false;

                            label_CommunicationTerminal_ReceivedData.Text = string.Format("Beam Expander Zoom : {0}, Mrad : {1}", workStage.m_dBET_ZoomValue, workStage.m_dBET_MradValue);
                        }
                    }
                    break;

                case 4:
                    if (workStage.m_dustCollector_UpperPos_Comm == null)
                    {
                        label_CommTerminal_LinkStatus.Text = "Not Connected";
                    }
                    else
                    {
                        label_CommTerminal_LinkStatus.Text = workStage.m_dustCollector_UpperPos_Comm.IsOpen ? "Connected" : "Disconnected";
                    }
                    break;

                case 5:
                    if (workStage.m_dustCollector_LowerPos_Comm == null)
                    {
                        label_CommTerminal_LinkStatus.Text = "Not Connected";
                    }
                    else
                    {
                        label_CommTerminal_LinkStatus.Text = workStage.m_dustCollector_LowerPos_Comm.IsOpen ? "Connected" : "Disconnected";
                    }
                    break;

                case 6:
                    if (workStage.m_electroRegulator_Comm == null)
                    {
                        label_CommTerminal_LinkStatus.Text = "Not Connected";
                    }
                    else
                    {
                        label_CommTerminal_LinkStatus.Text = workStage.m_electroRegulator_Comm.IsOpen ? "Connected" : "Disconnected";

                        //  Electro Pneumatic Regulator 읽었으면?
                        label_CommunicationTerminal_ReceivedData.Text = string.Format("Electro Pneumatic Regulator Pressure : {0}", workStage.m_dEPRO_Value);

                        //if (workStage.m_bElectroRegulator_CommData_Received)
                        //{
                        //    //  읽은 값을 kPa로 변환
                        //    double m_dkPa = workStage.ElectroPneumaticRegulatorComm_ReceivedData_ConvertTo_Pressure(workStage.m_strElectroRegulator_Comm_ReceivedData);

                        //    label_CommunicationTerminal_ReceivedData.Text = string.Format("Electro Pneumatic Regulator Pressure : {0}", m_dkPa);

                        //    workStage.m_bElectroRegulator_CommData_Received = false;
                        //    workStage.m_strElectroRegulator_Comm_ReceivedData = "";
                        //}
                    }
                    break;

                case 7:
                    if (workStage.m_SocketLaser == null)
                    {
                        label_CommTerminal_LinkStatus.Text = "Not Connected";
                    }
                    else
                    {
                        label_CommTerminal_LinkStatus.Text = workStage.m_SocketLaser.isConnected ? "Connected" : "Disconnected";
                    }
                    break;

                case 8:
                    if (workStage.m_SocketLaserHeightSensor == null)
                    {
                        label_CommTerminal_LinkStatus.Text = "Not Connected";
                    }
                    else
                    {
                        label_CommTerminal_LinkStatus.Text = workStage.m_SocketLaserHeightSensor.isConnected ? "Connected" : "Disconnected";

                        //  Laser Sensor 값을 읽었으면?
                        //if (workStage.m_SocketLaserHeightSensor.IsConnected())
                        if (workStage.m_SocketLaserHeightSensor.isConnected)
                        {
                            label_CommunicationTerminal_ReceivedData.Text = string.Format("Laser Height Sensor Value : {0}", workStage.m_dLaserHeightSensorSocket_Value);                            

                            //if (workStage.m_bLaserSensorSocket_Received)
                            //{
                            //    workStage.m_bLaserSensorSocket_Received = false;

                            //    //  데이터 구분
                            //    string[] LaserSensorData = workStage.m_strLaserSensorSocket_ReceivedData.Split(',');

                            //    if (LaserSensorData.Length > 1)
                            //    {
                            //        //  값을 읽었을 때
                            //        label_CommunicationTerminal_ReceivedData.Text = string.Format("Laser Height Sensor Value : {0}", LaserSensorData[1]);
                            //        workStage.m_strLaserSensorSocket_ReceivedData = "";
                            //    }
                            //    else if (LaserSensorData.Length == 1)
                            //    {
                            //        //  명렁어 리턴일 때
                            //        label_CommunicationTerminal_ReceivedData.Text = string.Format("Laser Height Sensor Value : {0}", LaserSensorData[0]);
                            //        workStage.m_strLaserSensorSocket_ReceivedData = "";
                            //    }
                            //}
                        }
                    }
                    break;

                default: label_Activated_Unit.Text = "No units selected"; break;
            }


            timer_Status.Enabled = true;
        }

        private void FormNew_CommunicationTerminal_Shown(object sender, EventArgs e)
        {
            timer_Status.Enabled = true;

            switch (m_nSelectedUnitIndex)
            {
                case 0:
                    label_Activated_Unit.Text = "Illuminator";
                    tabControl_CommTestFunction.SelectTab(m_nSelectedUnitIndex);
                    radioButton_IlluminatorChannel_0.Checked = true;

                    SetScroll();
                    break;

                case 1:
                    label_Activated_Unit.Text = "PowerMeter (BDS)";
                    tabControl_CommTestFunction.SelectTab(m_nSelectedUnitIndex);
                    break;

                case 2:
                    label_Activated_Unit.Text = "PowerMeter (Stage)";
                    tabControl_CommTestFunction.SelectTab(m_nSelectedUnitIndex);
                    break;

                case 3:
                    label_Activated_Unit.Text = "Motorized Beam Expander";
                    tabControl_CommTestFunction.SelectTab(m_nSelectedUnitIndex);
                    break;

                case 4:
                    label_Activated_Unit.Text = "Dust Collector (Upper)";
                    tabControl_CommTestFunction.SelectTab(m_nSelectedUnitIndex);
                    break;

                case 5:
                    label_Activated_Unit.Text = "Dust Collector (Lower)";
                    tabControl_CommTestFunction.SelectTab(m_nSelectedUnitIndex);
                    break;

                case 6:
                    label_Activated_Unit.Text = "Electro Pneumatic Regulator";
                    tabControl_CommTestFunction.SelectTab(m_nSelectedUnitIndex);

                    //if (workStage.m_electroRegulator_Comm == null)
                    //{
                    //    label_CommTerminal_LinkStatus.Text = "Not Connected";
                    //}
                    //else
                    //{
                    //    label_CommTerminal_LinkStatus.Text = workStage.m_electroRegulator_Comm.IsOpen ? "Connected" : "Disconnected";
                    //}
                    break;

                case 7:
                    //if (workStage.m_SocketLaser == null)
                    //{
                    //    label_CommTerminal_LinkStatus.Text = "Not Connected";
                    //}
                    //else
                    //{
                    //    label_CommTerminal_LinkStatus.Text = workStage.m_SocketLaser.IsConnected() ? "Connected" : "Disconnected";
                    //}
                    break;

                case 8:
                    label_Activated_Unit.Text = "Laser Height Sensor";
                    tabControl_CommTestFunction.SelectTab(m_nSelectedUnitIndex);
                    break;

                default: label_Activated_Unit.Text = "No units selected"; break;
            }
        }

        public void FormNew_CommunicationTerminal_TabRefresh()
        {
            timer_Status.Enabled = true;

            switch (m_nSelectedUnitIndex)
            {
                case 0:
                    label_Activated_Unit.Text = "Illuminator";
                    tabControl_CommTestFunction.SelectTab(m_nSelectedUnitIndex);
                    radioButton_IlluminatorChannel_0.Checked = true;

                    SetScroll();
                    break;

                case 1:
                    label_Activated_Unit.Text = "PowerMeter (BDS)";
                    tabControl_CommTestFunction.SelectTab(m_nSelectedUnitIndex);
                    break;

                case 2:
                    label_Activated_Unit.Text = "PowerMeter (Stage)";
                    tabControl_CommTestFunction.SelectTab(m_nSelectedUnitIndex);
                    break;

                case 3:
                    label_Activated_Unit.Text = "Motorized Beam Expander";
                    tabControl_CommTestFunction.SelectTab(m_nSelectedUnitIndex);
                    break;

                case 4:
                    label_Activated_Unit.Text = "Dust Collector (Upper)";
                    tabControl_CommTestFunction.SelectTab(m_nSelectedUnitIndex);
                    break;

                case 5:
                    label_Activated_Unit.Text = "Dust Collector (Lower)";
                    tabControl_CommTestFunction.SelectTab(m_nSelectedUnitIndex);
                    break;

                case 6:
                    label_Activated_Unit.Text = "Electro Pneumatic Regulator";
                    tabControl_CommTestFunction.SelectTab(m_nSelectedUnitIndex);

                    //if (workStage.m_electroRegulator_Comm == null)
                    //{
                    //    label_CommTerminal_LinkStatus.Text = "Not Connected";
                    //}
                    //else
                    //{
                    //    label_CommTerminal_LinkStatus.Text = workStage.m_electroRegulator_Comm.IsOpen ? "Connected" : "Disconnected";
                    //}
                    break;

                case 7:
                    label_Activated_Unit.Text = "Laser";
                    tabControl_CommTestFunction.SelectTab(m_nSelectedUnitIndex);

                    //if (workStage.m_SocketLaser == null)
                    //{
                    //    label_CommTerminal_LinkStatus.Text = "Not Connected";
                    //}
                    //else
                    //{
                    //    label_CommTerminal_LinkStatus.Text = workStage.m_SocketLaser.IsConnected() ? "Connected" : "Disconnected";
                    //}
                    break;

                case 8:
                    label_Activated_Unit.Text = "Laser Height Sensor";
                    tabControl_CommTestFunction.SelectTab(m_nSelectedUnitIndex);
                    break;

                default: label_Activated_Unit.Text = "No units selected"; break;
            }
        }

        private void FormNew_CommunicationTerminal_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer_Status.Enabled = false;

            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void button_CommTerminal_Connect_Click(object sender, EventArgs e)
        {
            //  Connect to the selected unit

            bool m_bTemp = false;

            if ((m_nSelectedUnitIndex < 0) ||
                (m_nSelectedUnitIndex > System.Enum.GetValues(typeof(CommList)).Length))
                return;

            m_bTemp = Equipment.stCommunicationSet[m_nSelectedUnitIndex].Connect;
            Equipment.stCommunicationSet[m_nSelectedUnitIndex].Connect = true;

            switch (m_nSelectedUnitIndex)
            {
                case 0: workStage.Illuminator_Init();                   break;
                case 1: workStage.PowerMeterComm_ExitPos_Init();        break;
                case 2: workStage.PowerMeterComm_TargetPos_Init();      break;
                case 3: workStage.BeamExpanderComm_Init();              break;
                case 4: workStage.DustCollector_UpperPos_Comm_Init();   break;
                case 5: workStage.DustCollector_LowerPos_Comm_Init();   break;
                case 6: workStage.ElectroPneumaticRegulator_Comm_Init();break;
                //case 7: workStage.Laser_Socket_Connect();               break;
                case 7: workStage.RapidLxLaser_Comm_Init();             break;
                case 8: workStage.LaserSensor_Socket_Connect();         break;
                case 9: bds.InitDustCollector(DustCollectorController.CollectorPosition.Upper); break;
                case 10: bds.InitDustCollector(DustCollectorController.CollectorPosition.Lower); break;
                default: label_Activated_Unit.Text = "No units selected"; break;
            }

            Equipment.stCommunicationSet[m_nSelectedUnitIndex].Connect = m_bTemp;
        }

        private void button_CommTerminal_Disconnect_Click(object sender, EventArgs e)
        {
            //  Disconnect from the selected unit

            bool m_bTemp = false;

            if ((m_nSelectedUnitIndex < 0) ||
                (m_nSelectedUnitIndex > System.Enum.GetValues(typeof(CommList)).Length))
                return;

            switch (m_nSelectedUnitIndex)
            {
                case 0: CommonModule.Instance.Illuminator.Close();      break;
                case 1: workStage.m_powerMeter_ExitPos_Comm.CloseComm();    break;
                case 2:
                    if (workStage.m_powerMeter_TargetPos_Comm.IsOpen)
                    {
                        workStage.m_nPowerMeterStageCommStep = (int)WorkStage.PowerMeterStageComm_Step.None;
                        workStage.m_powerMeter_TargetPos_Comm.CloseComm();
                    }
                    break;
                case 3: workStage.m_beamExpander_Comm.CloseComm();          break;
                case 4: workStage.m_dustCollector_UpperPos_Comm.CloseComm();break;
                case 5: workStage.m_dustCollector_LowerPos_Comm.CloseComm();break;
                case 6: workStage.m_electroRegulator_Comm.CloseComm();      break;
                //case 7: workStage.m_SocketLaser.Close();                break;
                case 7: workStage.m_rapidLxLaser_Comm.CloseComm();          break;
                case 8: workStage.m_SocketLaserHeightSensor.Close();    break;
                default: label_Activated_Unit.Text = "No units selected"; break;
            }
        }

        private void radioButton_IlluminatorChannel_0_CheckedChanged(object sender, EventArgs e)
        {
            //  조명값 변경
            hScrollBarIlluminator.Value = workStage.Config.ListIlluminationChannel[0].Value;
            this.baseLabel1Value.Text = hScrollBarIlluminator.Value.ToString();
        }

        private void radioButton_IlluminatorChannel_1_CheckedChanged(object sender, EventArgs e)
        {
            //  조명값 변경
            hScrollBarIlluminator.Value = workStage.Config.ListIlluminationChannel[1].Value;
            this.baseLabel1Value.Text = hScrollBarIlluminator.Value.ToString();
        }

        private void SetScroll()
        {
            hScrollBarIlluminator.Minimum = (int)workStage.Config.ListIlluminationChannel[0].Min;
            hScrollBarIlluminator.Maximum = (int)workStage.Config.ListIlluminationChannel[0].Max;

            baseLabelMin.Text = hScrollBarIlluminator.Minimum.ToString();
            baseLabelMax.Text = hScrollBarIlluminator.Maximum.ToString();
        }

        private void hScrollBarIlluminator_ValueChanged(object sender, System.EventArgs e)
        {
            //  선택된 카메라에 따라 조명값 변경
            if (radioButton_IlluminatorChannel_0.Checked)
            {
                workStage.Config.ListIlluminationChannel[0].Value = hScrollBarIlluminator.Value;
                this.baseLabel1Value.Text = hScrollBarIlluminator.Value.ToString();
                CommonModule.Instance.Illuminator.SetVolume(this.hScrollBarIlluminator.Value, 0);
            }
            else
            {
                workStage.Config.ListIlluminationChannel[1].Value = hScrollBarIlluminator.Value;
                this.baseLabel1Value.Text = hScrollBarIlluminator.Value.ToString();
                CommonModule.Instance.Illuminator.SetVolume(this.hScrollBarIlluminator.Value, 1);
            }
        }

        private void button_PowerMeter_BDS_ReadOnce_Click(object sender, EventArgs e)
        {
            //workStage.m_bLaserPowerMeter_ExitPos_CommData_Received = false;
            //workStage.m_strLaserPowerMeter_ExitPos_Comm_ReceivedData = "";
            //workStage.m_nPowerMeterBDSCommRecvData_CR_Count = 0;

            //workStage.LaserPowerMeter_GetValue((int)WorkStage.nPowerMeter.ExitPos);

            m_bPM_BDS_ReadOnce = true;
            m_bPM_BDS_ReadContinuous = false;
        }

        private void button_PowerMeter_Stage_ReadOnce_Click(object sender, EventArgs e)
        {
            //workStage.m_bLaserPowerMeter_TargetPos_CommData_Received = false;
            //workStage.m_strLaserPowerMeter_TargetPos_Comm_ReceivedData = "";
            //workStage.m_nPowerMeterStageCommRecvData_CR_Count = 0;

            //workStage.LaserPowerMeter_GetValue((int)WorkStage.nPowerMeter.TargetPos);

            m_bPM_Stage_ReadOnce = true;
            m_bPM_Stage_ReadContinuous = false;
        }

        private void button_PowerMeter_BDS_ContinuousReading_Click(object sender, EventArgs e)
        {
            //workStage.m_nPowerMeterBDSCommStep = (int)WorkStage.PowerMeterBDSComm_Step.Start;

            m_bPM_BDS_ReadOnce = false;
            m_bPM_BDS_ReadContinuous = true;
        }

        private void button_PowerMeter_Stage_ContinuousReading_Click(object sender, EventArgs e)
        {
            //workStage.m_nPowerMeterStageCommStep = (int)WorkStage.PowerMeterStageComm_Step.Start;
            //if (!workStage.timer_SubWork.Enabled)
            //{
            //    workStage.timer_SubWork.Enabled = true;
            //}

            m_bPM_Stage_ReadOnce = false;
            m_bPM_Stage_ReadContinuous = true;
        }

        public byte getHex(string srcValue)
        {
            //srcValue = "78";

            return Convert.ToByte(srcValue, 16);

            //리턴되는 값은 0x78
        }

        public string getByte(byte srcValue)
        {
            //srcValue = 0x78

            return Convert.ToString(srcValue, 32);

            //리턴값은 "78";
        }

        private void button_DustCollector_Upper_Write_Click(object sender, EventArgs e)
        {
            //byte m_bData = getHex(textBox_DustCollector_Upper_Address.Text);

            if ((workStage.m_dustCollector_UpperPos_Comm != null) &&
                (textBox_DustCollector_Upper_Address.Text != "") && (textBox_DustCollector_Upper_Address.Text.Length == 4))
            {
                workStage.DustCollectorComm_Send_Write((int)WorkStage.nDustCollector.DustCollector_Upper, textBox_DustCollector_Upper_Address.Text, 1, textBox_DustCollector_Upper_Data.Text);
            }
        }

        private void button_DustCollector_Lower_WriteCommand_Click(object sender, EventArgs e)
        {
            if ((workStage.m_dustCollector_LowerPos_Comm != null) &&
                (textBox_DustCollector_Lower_Address.Text != "") && (textBox_DustCollector_Lower_Address.Text.Length == 4))
            {
                workStage.DustCollectorComm_Send_Write((int)WorkStage.nDustCollector.DustCollector_Lower, textBox_DustCollector_Lower_Address.Text, 1, textBox_DustCollector_Lower_Data.Text);
            }
        }

        private void button_ElectroPneumaticRegulator_SetValue_Click(object sender, EventArgs e)
        {
            //  압력 세팅

            if ((Equipment.ToDouble(textBox_ElectroPneumaticRegulator_SetValue.Text) > -1.3) || (Equipment.ToDouble(textBox_ElectroPneumaticRegulator_SetValue.Text) < -80.0))
            {
                MessageBox.Show("Electro Pneumatic Regulator out of range\r\n\r\n[Available Range : -1.3kPa ~ -80.0kPa]", "Information!!");
                return;
            }

            workStage.m_bElectroRegulator_CommData_Received = false;

            workStage.ElectroPneumaticRegulatorComm_Pressure_Set(Equipment.ToDouble(textBox_ElectroPneumaticRegulator_SetValue.Text));
        }

        private void button_ElectroPneumaticRegulator_Pressure_Inc_Click(object sender, EventArgs e)
        {
            workStage.m_bElectroRegulator_CommData_Received = false;

            workStage.ElectroPneumaticRegulatorComm_Pressure_Inc();
        }

        private void button_ElectroPneumaticRegulator_Pressure_Dec_Click(object sender, EventArgs e)
        {
            workStage.m_bElectroRegulator_CommData_Received = false;

            workStage.ElectroPneumaticRegulatorComm_Pressure_Dec();
        }

        private void button_ElectroPneumaticRegulator_GetPressure_Click(object sender, EventArgs e)
        {
            workStage.m_bElectroRegulator_CommData_Received = false;

            workStage.ElectroPneumaticRegulatorComm_Pressure_Read();
        }

        private void button_ElectroPneumaticRegulator_SetPressure_Read_Click(object sender, EventArgs e)
        {
            workStage.m_bElectroRegulator_CommData_Received = false;

            workStage.ElectroPneumaticRegulatorComm_SettingPressure_Read();
        }

        private void button_LaserHeightSensor_SettingMode_Click(object sender, EventArgs e)
        {
            //  Laser Height Sensor : Setting Mode 로 변경

            workStage.LaserSensor_Socket_SettingMode();
        }

        private void button_LaserHeightSensor_MeasurementMode_Click(object sender, EventArgs e)
        {
            //  Laser Height Sensor : Measurement Mode 로 변경

            workStage.LaserSensor_Socket_MeasurementMode();
        }

        private void button_LaserHeightSensor_ReadValue_Click(object sender, EventArgs e)
        {
            //  Laser Height Sensor : 측정값 읽기

            workStage.LaserSensor_Socket_ReadValue();
        }

        private void button_BeamExpander_Zoom_MoveCommand_Click(object sender, EventArgs e)
        {
            //  Beam Expander Zoom Position Move

            double m_dZoom = Equipment.ToDouble(textBox_BeamExpander_Zoom_Position.Text);

            workStage.BeamExpander_Send_Motor_SetPosition((int)WorkStage.nMotorizedBET.ZoomMotor, m_dZoom);
        }

        private void button_BeamExpander_Mrad_MoveCommand_Click(object sender, EventArgs e)
        {
            //  Beam Expander Mrad Position Move

            double m_dMrad = Equipment.ToDouble(textBox_BeamExpander_Mrad_Position.Text);

            workStage.BeamExpander_Send_Motor_SetPosition((int)WorkStage.nMotorizedBET.BeamExpansionMotor, m_dMrad);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //  Get Current Position

            //workStage.BeamExpander_Send_GetCurrentStatus_Magnification_DivergenceAngle();

            m_bBET_ReadOnce = true;
        }

        private void button_BeamExpander_Zoom_InitCommand_Click(object sender, EventArgs e)
        {
            //  Zoom Init

            workStage.BeamExpander_Send_Motor_InitialPosition((int)WorkStage.nMotorizedBET.ZoomMotor);
        }

        private void button_BeamExpander_Mrad_InitCommand_Click(object sender, EventArgs e)
        {
            //  Mrad Init

            workStage.BeamExpander_Send_Motor_InitialPosition((int)WorkStage.nMotorizedBET.BeamExpansionMotor);
        }

        private void button_TEST1_Click(object sender, EventArgs e)
        {
            bool bOn = bds.DustCollector_Upper.DustCollector_On();

            Thread.Sleep(300);  // 상태 반영 대기 (인버터 응답 지연 고려)

            if (bds.DustCollector_Upper.GetFullStatus(out var runState, out var freq, out var current, out var alarm))
            {
                Log.Write("DustCollector", $"[TEST1] 집진기 상태: {(runState == CollectorRunState.Running ? "운전 중" : "정지")}, 주파수: {freq}Hz, 전류: {current}A, 알람: {alarm}");
            }
            else
            {
                Log.Write("DustCollector", "[TEST1] 상태 읽기 실패");
            }

        }

        private void button_Test2_Click(object sender, EventArgs e)
        {
            bds.DustCollector_Upper.DustCollector_Off();

            Thread.Sleep(300);  // 상태 반영 대기 (인버터 응답 지연 고려)

            if (bds.DustCollector_Upper.GetFullStatus(out var runState, out var freq, out var current, out var alarm))
            {
                Log.Write("DustCollector", $"[TEST2] 집진기 상태: {(runState == CollectorRunState.Running ? "운전 중" : "정지")}, 주파수: {freq}Hz, 전류: {current}A, 알람: {alarm}");
            }
            else
            {
                Log.Write("DustCollector", "[TEST2] 상태 읽기 실패");
            }
        }

        private void button_Test3_Click(object sender, EventArgs e)
        {
            bool btn = bds.DustCollector_Upper.SetFrequency(10);

            //GetOutputFrequency //GetFrequency
            if (bds.DustCollector_Upper.GetOutputFrequency(out double freq))
            {
                Log.Write("DustCollector", $"[TEST3] 현재 주파수: {freq} Hz");
            }
            else
            {
                Log.Write("DustCollector", "[TEST3] 주파수 읽기 실패");
            }
        }
    }
}
