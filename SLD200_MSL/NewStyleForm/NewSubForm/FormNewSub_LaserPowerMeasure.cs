using QMC.Common;
using QMC.Common.Global;
using QMC.Common.Modules;
using QMC.Common.Parts;
using QMC.Common.Q_Sequence;
using QMC.Common.VisionPart;
using QMC.Core;
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
using static QMC.Common.Q_Sequence.Sequence_VerifyScannerCameraOffset;

namespace SLD200.NewStyleForm.NewSubForm
{
    public partial class FormNewSub_LaserPowerMeasure : Form
    {
        private SpiralLabScanner _scanner;
        private SpiralLabScanner.ScannerLaserSetting _setting = new SpiralLabScanner.ScannerLaserSetting();
        static WorkStage workStage;
        static Vision vision;

        private System.Windows.Forms.Timer timerModuleStatus;
        private bool _isRunning_ModuleStatus = false;

        private List<float> _measuredPowerList = new List<float>();
        public FormNewSub_LaserPowerMeasure(SpiralLabScanner scanner)
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.DoubleBuffered = true;

            foreach (Module module in Equipment.Modules)
            {
                if (module.Name == "WorkStage") workStage = module as WorkStage;
                if (module.Name == "Vision") vision = module as Vision;
            }

            timerModuleStatus = new System.Windows.Forms.Timer();
            timerModuleStatus.Interval = 100;
            timerModuleStatus.Tick += TimerModuleStatus_Tick;
            timerModuleStatus.Start();

            _scanner = scanner;
            InitSettingTable();

            comboBoxTargetType.SelectedIndex = 0; // 기본값으로 "Top" 선택

            workStage.m_Sequence_LaserPowerMeasure.OnPowerMeasured += UpdatePowerMeasureLog;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // 사용자가 닫기(X 버튼) 누른 경우 → 숨기기만 하고 종료 안 함
                e.Cancel = true;
                this.Hide();
                return;
            }

            // 그 외 종료 (Application.Exit 등) → 정식 해제
            base.OnFormClosing(e);
        }

        public void DisposeSemiAutoResources()
        {
            if (timerModuleStatus != null)
            {
                timerModuleStatus.Stop();
                timerModuleStatus.Tick -= TimerModuleStatus_Tick;
                timerModuleStatus.Dispose();
                timerModuleStatus = null;
            }
            // 필요 시 다른 모듈 정리도 여기에
        }

        private void TimerModuleStatus_Tick(object sender, EventArgs e)
        {
            if (_isRunning_ModuleStatus)
                return;

            try
            {
                _isRunning_ModuleStatus = true;
                Timer_ModuleStatusRun();
            }
            catch (Exception ex)
            {
                // 로그 남기기
                Log.Write(ex);
                _isRunning_ModuleStatus = false;
            }
            finally
            {
                _isRunning_ModuleStatus = false;
            }
        }

        private void Timer_ModuleStatusRun()
        {
            // 실행할 작업들을 여기에 구현.


        }

        private void InitSettingTable()
        {
            comboBoxTargetType.SelectedIndex = 0; // 기본값으로 "Top" 선택

            dataGridViewSettings.ColumnCount = 2;
            dataGridViewSettings.Columns[0].Name = "Parameter";
            dataGridViewSettings.Columns[1].Name = "Value";
            // 열 전체를 ReadOnly로 설정
            dataGridViewSettings.Columns[0].ReadOnly = true;
            //dataGridViewSettings.Columns[1].ReadOnly = true;


            LoadLaserPowerMeasureSetting();
            if (Equipment.Machine_LaserType_CO2)
            {
                dataGridViewSettings.Rows.Add("Frequency", _setting.Frequency);
                dataGridViewSettings.Rows.Add("PulseWidth", _setting.PulseWidth);
                dataGridViewSettings.Rows.Add("DutyCycle", _setting.DutyCycle);
            }
            else
            {
                dataGridViewSettings.Rows.Add("PowerPercent", _setting.PowerPercent);
                dataGridViewSettings.Rows.Add("Frequency", _setting.Frequency);
                dataGridViewSettings.Rows.Add("PulseWidth", _setting.PulseWidth);
            }


        }

        private void buttonApplyAndFire_Click(object sender, EventArgs e)
        {
            try
            {
                _setting.Initialize();
                if (numericUpDownDuration.Value <= 0)
                {
                    MessageBox.Show("Duration는 0보다 큰 값이어야 합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                foreach (DataGridViewRow row in dataGridViewSettings.Rows)
                {
                    if (row.Cells[0].Value == null || row.Cells[1].Value == null)
                        continue;

                    string param = row.Cells[0].Value.ToString();
                    string valueStr = row.Cells[1].Value.ToString();
                    float value = float.TryParse(valueStr, out var v) ? v : 0f;

                    if (Equipment.Machine_LaserType_CO2)
                    {
                        switch (param)
                        {
                            case "Frequency": _setting.Frequency = value; break;
                            case "PulseWidth": _setting.PulseWidth = value; break;
                            case "DutyCycle": _setting.DutyCycle = value; break;
                        }
                    }
                    else
                    {
                        switch (param)
                        {
                            case "PowerPercent": _setting.PowerPercent = value; break;
                            case "Frequency": _setting.Frequency = value; break;
                            case "PulseWidth": _setting.PulseWidth = value; break;
                        }
                    }
                }

                if (_setting.PowerPercent < 0 || _setting.PowerPercent > 100)
                {
                    MessageBox.Show("PowerPercent는 0에서 100 사이의 값이어야 합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                bool result = false;
                if (!Equipment.Machine_LaserType_CO2) // UV인 경우.
                {
                    result = SetLaserPower(_setting.PowerPercent);
                    if (!result)
                        MessageBox.Show("레이저 파워 변경", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                float duration = (float)numericUpDownDuration.Value;
                result = _scanner.LaserOn(duration, _setting);
                if (result)
                {
                    SaveLaserPowerMeasureSetting();
                    MessageBox.Show("레이저 출력 성공", "완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("레이저 출력 실패", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"에러 발생: {ex.Message}", "예외", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            _scanner.LaserOff();
            // or
            _scanner.LaserAbort();
        }

        public bool SetLaserPower(float powerPercent)
        {
            bool bSuccess = false;
            string strTemp = string.Empty;
            if ((workStage.m_rapidLxLaser_Comm != null))
            {
                if (workStage.m_rapidLxLaser_Comm.IsOpen)
                {
                    workStage.RapidLxLaserComm_Laser_OutputEnergy_Set(powerPercent);

                    strTemp = string.Format("Laser Power 변경 시작, Laser Power ({0:0.000})",
                                            powerPercent);
                    Log.Write("SLD-200", "SetLaserPower", strTemp);
                    bSuccess = true;
                }
                else
                {
                    strTemp = string.Format("Laser Power 변경 실패. (Laser Comm 열리지 않음)");
                    Log.Write("SLD-200", "SetLaserPower", strTemp);
                }
            }

            return bSuccess;
        }

        private void button_MeasureReady_Click(object sender, EventArgs e)
        {
            string selectedTarget = comboBoxTargetType.SelectedItem.ToString();
            string strTemp = string.Empty;

            if (selectedTarget == "Top")
            {
                // 탑에 대한 로직
                workStage.workStageParameter.DO_BDS_PowerMeter_BW(false);
                Thread.Sleep(100); // 잠시 대기
                workStage.workStageParameter.DO_BDS_PowerMeter_FW(true);

                // 셔터 닫힘 확인을 최대 3초간 반복 체크
                bool bSuccess = false;
                int timeoutMs = 3000;
                int elapsedMs = 0;
                int intervalMs = 100;
                while (elapsedMs < timeoutMs)
                {
                    if (workStage.workStageParameter.DI_BDS_PowerMeter_BW_Check() &&
                        !workStage.workStageParameter.DI_BDS_PowerMeter_FW_Check())
                    {
                        bSuccess = true;
                        break;
                    }

                    Thread.Sleep(intervalMs);
                    elapsedMs += intervalMs;
                }

                if (!bSuccess)
                {
                    strTemp = "Shutter 닫기 실패 (3초 내 상태 도달 못함)";
                    Log.Write("SLD-200", Equipment.User_Name, strTemp);
                    new MessageBoxOk().ShowDialog("Error !", strTemp);
                    return;
                }
            }
            else if (selectedTarget == "Stage")
            {
                // 스테이지에 대한 로직
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "위치 이동 하시겠습니까?"))
                    return;

                strTemp = string.Empty;
                //  속도 설정
                Equipment.Type_Motor_Speed motor_Speed;
                motor_Speed = Equipment.Type_Motor_Speed.Coarse;
                int nIndex = 0; //Teaching Position Index

                nIndex = (int)Vision.Vision_TeachingPosList.Laser_FocusPos;
                workStage.MovetoWorkStage_TeachingPositionsZ(nIndex, motor_Speed);
                double dPosZ = vision.stVisionTeachingPos[nIndex].Vision_Z;
                Thread.Sleep(500);
                bool bWaitZ = workStage.WaitUntilInPositionAsync(WorkStage.nAxis.Z, dPosZ).Result;
                if (!bWaitZ)
                {
                    strTemp = string.Format("Z-Axis이 이동 실패.");
                    Log.Write("SLD-200", Equipment.User_Name, strTemp);

                    var mb1 = new MessageBoxOk();
                    mb1.ShowDialog("Error !", strTemp);
                    return;
                }

                nIndex = (int)WorkStage.WorkStage_TeachingPosList.STAGE_Scanner_PMPos;
                workStage.MovetoWorkStage_TeachingPositionsXY(nIndex, motor_Speed);
                double dPosX = workStage.stWorkStageTeachingPos[nIndex].Stage_X;
                double dPosY = workStage.stWorkStageTeachingPos[nIndex].Stage_Y;
                Thread.Sleep(500);
                bool bWaitX = workStage.WaitUntilInPositionAsync(WorkStage.nAxis.X, dPosX).Result;
                bool bWaitY = workStage.WaitUntilInPositionAsync(WorkStage.nAxis.Y, dPosY).Result;
                if (!bWaitX || !bWaitY)
                {
                    strTemp = string.Format("X-Axis 또는 Y-Axis이 이동 실패.");
                    Log.Write("SLD-200", Equipment.User_Name, strTemp);
                    var mb1 = new MessageBoxOk();
                    mb1.ShowDialog("Error !", strTemp);
                    return;
                }
            }
        }

        private void comboBoxTargetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            workStage.m_Sequence_LaserPowerMeasure.m_nType = comboBoxTargetType.SelectedIndex;
        }

        public void SaveLaserPowerMeasureSetting()
        {
            string iniPath = ConfigManager.GetConfigPath() + "\\ConfigFile(Do not delete or modify).ini";

            NativeMethods.WritePrivateProfileString("Laser", "TargetTypeIndex", comboBoxTargetType.SelectedIndex.ToString(), iniPath);
            NativeMethods.WritePrivateProfileString("Laser", "Duration", numericUpDownDuration.Value.ToString(), iniPath);

            if (Equipment.Machine_LaserType_CO2)
            {
                NativeMethods.WritePrivateProfileString("Laser", "Frequency", _setting.Frequency.ToString(), iniPath);
                NativeMethods.WritePrivateProfileString("Laser", "PulseWidth", _setting.PulseWidth.ToString(), iniPath);
                NativeMethods.WritePrivateProfileString("Laser", "DutyCycle", _setting.DutyCycle.ToString(), iniPath);
            }
            else
            {
                NativeMethods.WritePrivateProfileString("Laser", "PowerPercent", _setting.PowerPercent.ToString(), iniPath);
                NativeMethods.WritePrivateProfileString("Laser", "Frequency", _setting.Frequency.ToString(), iniPath);
                NativeMethods.WritePrivateProfileString("Laser", "PulseWidth", _setting.PulseWidth.ToString(), iniPath);
            }
        }

        public void LoadLaserPowerMeasureSetting()
        {
            string iniPath = ConfigManager.GetConfigPath() + "\\ConfigFile(Do not delete or modify).ini";
            StringBuilder temp = new StringBuilder(255);

            if (!File.Exists(iniPath))
            {
                MessageBox.Show("LaserPowerMeasure 설정 파일이 없습니다.\r\n[기본값으로 시작합니다.]", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _setting.Initialize();              //초기화 후 Data Load.

            NativeMethods.GetPrivateProfileString("Laser", "TargetTypeIndex", "0", temp, 255, iniPath);
            comboBoxTargetType.SelectedIndex = Equipment.ToInt(temp.ToString());

            NativeMethods.GetPrivateProfileString("Laser", "Duration", "100000", temp, 255, iniPath);
            numericUpDownDuration.Value = (decimal)Equipment.ToDouble(temp.ToString());

            if (Equipment.Machine_LaserType_CO2)
            {
                NativeMethods.GetPrivateProfileString("Laser", "Frequency", "7000", temp, 255, iniPath);
                _setting.Frequency = (float)Equipment.ToDouble(temp.ToString());

                NativeMethods.GetPrivateProfileString("Laser", "PulseWidth", "1", temp, 255, iniPath);
                _setting.PulseWidth = (float)Equipment.ToDouble(temp.ToString());

                NativeMethods.GetPrivateProfileString("Laser", "DutyCycle", "1", temp, 255, iniPath);
                _setting.DutyCycle = (float)Equipment.ToDouble(temp.ToString());
            }
            else
            {
                NativeMethods.GetPrivateProfileString("Laser", "PowerPercent", "10", temp, 255, iniPath);
                _setting.PowerPercent = (float)Equipment.ToDouble(temp.ToString());

                NativeMethods.GetPrivateProfileString("Laser", "Frequency", "500000", temp, 255, iniPath);
                _setting.Frequency = (float)Equipment.ToDouble(temp.ToString());

                NativeMethods.GetPrivateProfileString("Laser", "PulseWidth", "1", temp, 255, iniPath);
                _setting.PulseWidth = (float)Equipment.ToDouble(temp.ToString());
            }
        }

        private void button_SeqStart_Click(object sender, EventArgs e)
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "파워 측정 하시겠습니까?"))
                return;

            string strTemp = string.Empty;

            workStage.m_Sequence_LaserPowerMeasure.m_nType = comboBoxTargetType.SelectedIndex;
            workStage.m_Sequence_LaserPowerMeasure.Start();
            workStage.m_Sequence_LaserPowerMeasure.m_MainTick_Start = true;
        }

        private void button_SeqStop_Click(object sender, EventArgs e)
        {
            workStage.m_Sequence_LaserPowerMeasure.Reset();
            workStage.m_Sequence_LaserPowerMeasure.m_MainTick_Start = false;
        }

        public void UpdatePowerMeasureLog(List<float> measuredValues)
        {
            if (listBox_PowerLog.InvokeRequired)
            {
                listBox_PowerLog.Invoke(new Action(() => UpdatePowerMeasureLog(measuredValues)));
                return;
            }

            listBox_PowerLog.Items.Clear();

            int i = 1;
            foreach (float value in measuredValues)
            {
                listBox_PowerLog.Items.Add($"[{i++}] {value:F2} W");
            }

            if (measuredValues.Count > 0)
            {
                float average = measuredValues.Average();
                listBox_PowerLog.Items.Add("-----------------------------------");
                listBox_PowerLog.Items.Add($"[Average] {average:F2} W");
            }
        }

        public void AddPowerMeasure(float power)
        {
            _measuredPowerList.Add(power);
            RefreshPowerMeasureList();
        }

        public void ResetPowerMeasureList()
        {
            _measuredPowerList.Clear();
            RefreshPowerMeasureList();
        }

        private void RefreshPowerMeasureList()
        {
            if (listBox_PowerLog.InvokeRequired)
            {
                listBox_PowerLog.Invoke(new Action(RefreshPowerMeasureList));
                return;
            }

            listBox_PowerLog.Items.Clear();
            for (int i = 0; i < _measuredPowerList.Count; i++)
            {
                listBox_PowerLog.Items.Add($"측정 {i + 1}회차: {_measuredPowerList[i]:F2} W");
            }

            if (_measuredPowerList.Count > 0)
            {
                float average = _measuredPowerList.Average();
                listBox_PowerLog.Items.Add("--------------------------------------");
                listBox_PowerLog.Items.Add($"평균값: {average:F2} W");
            }
        }

        void UpdatePowerMeasureLog(float fPower)
        {
            if (listBox_PowerLog.InvokeRequired)
            {
                listBox_PowerLog.Invoke(new Action(() => UpdatePowerMeasureLog(fPower)));
                return;
            }

            AddPowerMeasure(fPower);
        }

        private void button_Test_Click(object sender, EventArgs e)
        {
            // 예시 파워 값들
            float[] testPowers = new float[] { 121.3f, 122.7f, 120.8f, 124.2f, 123.1f };

            foreach (float power in testPowers)
            {
                UpdatePowerMeasureLog(power);
                Thread.Sleep(200); // UI 업데이트 확인을 위한 딜레이 (선택)
            }

            MessageBox.Show("UpdatePowerMeasureLog 테스트 완료");
        }
    }
}
