using Newtonsoft.Json.Linq;
using QMC.Common;
using QMC.Common.Global;
using QMC.Common.Modules;
using QMC.Common.Parts;
using QMC.Common.Q_Sequence;
using QMC.Common.UI;
using QMC.Common.VisionPart;
using QMC.Core;
using SLD200.Properties;
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
using static QMC.Common.Q_Sequence.Sequence_VerifyScannerCameraOffset;
using MessageBox = System.Windows.MessageBox;

namespace SLD200.NewStyleForm.NewSubForm
{
    public partial class FormNewSub_LaserPowerMeasure : Form
    {
        private SpiralLabScanner _scanner;
        private SpiralLabScanner.ScannerLaserSetting _setting = new SpiralLabScanner.ScannerLaserSetting();
        static WorkStage workStage;
        static Vision vision;
        static Bds bds;

        private System.Windows.Forms.Timer timerLaserPowerMeasureStatus;
        private bool _isRunning_LaserPowerMeasureStatus = false;

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
                if (module.Name == "BDS") bds = module as Bds;
            }

            timerLaserPowerMeasureStatus = new System.Windows.Forms.Timer();
            timerLaserPowerMeasureStatus.Interval = 100;
            timerLaserPowerMeasureStatus.Tick += TimerModuleStatus_Tick;
            timerLaserPowerMeasureStatus.Start();

            _scanner = scanner;
            InitSettingTable();

            if(Equipment.Machine_LaserType_CO2)
            {
                comboBoxTargetType.SelectedIndex = 1; // 기본값으로 "Top" 선택
                comboBox_MaskIndex.SelectedIndex = 4;
                comboBox_BETPositionIndex.SelectedIndex = 0;
            }
            else
            {
                comboBox_MaskIndex.Visible = false;
                comboBox_BETPositionIndex.Visible = false;
            }

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

        public void InitSpiralLab(SpiralLabScanner spiralLabScanner)
        {
            _scanner = spiralLabScanner;
            LoadLaserPowerMeasureSetting();
        }
        public void DisposeSemiAutoResources()
        {
            if (timerLaserPowerMeasureStatus != null)
            {
                timerLaserPowerMeasureStatus.Stop();
                timerLaserPowerMeasureStatus.Tick -= TimerModuleStatus_Tick;
                timerLaserPowerMeasureStatus.Dispose();
                timerLaserPowerMeasureStatus = null;
            }
            // 필요 시 다른 모듈 정리도 여기에
        }

        private void TimerModuleStatus_Tick(object sender, EventArgs e)
        {
            if (_isRunning_LaserPowerMeasureStatus)
                return;

            try
            {
                _isRunning_LaserPowerMeasureStatus = true;
                Timer_ModuleStatusRun();
            }
            catch (Exception ex)
            {
                // 로그 남기기
                Log.Write(ex);
                _isRunning_LaserPowerMeasureStatus = false;
            }
            finally
            {
                _isRunning_LaserPowerMeasureStatus = false;
            }
        }

        private DateTime _measureStartTime;           // 측정 시작 시간 저장용
        private int _lastLoggedSecond = -1;           // 마지막으로 기록된 시간(초)
        private void Timer_ModuleStatusRun()
        {
            // 실행할 작업들을 여기에 구현.
            if(m_bStartLaserPowerMeasure)
            {
                float duration = (float)numericUpDownDuration.Value;  // 단위: 초
                double elapsedSeconds = (DateTime.Now - _measureStartTime).TotalSeconds;

                if (elapsedSeconds >= duration)
                {
                    // 종료
                    m_bStartLaserPowerMeasure = false;
                    return;
                }

                // 1초마다 측정값 추가
                int currentSecond = (int)elapsedSeconds;
                if (currentSecond > _lastLoggedSecond)
                {
                    _lastLoggedSecond = currentSecond;

                    // 예시 측정값 (실제 측정값을 받아와야 함)
                    double measuredPower = (_setting.PowerMeterType == 0) ? workStage.m_dPowerMeterBDS_Value : workStage.m_dPowerMeterStage_Value;
                    AddPowerMeasure((float)measuredPower);
                }
            }
        }

        private void InitSettingTable()
        {
            comboBoxTargetType.SelectedIndex = 1; // 기본값으로 "Top" 선택

            dataGridViewSettings.ColumnCount = 2;
            dataGridViewSettings.Columns[0].Name = "Parameter";
            dataGridViewSettings.Columns[1].Name = "Value";
            // 열 전체를 ReadOnly로 설정
            dataGridViewSettings.Columns[0].ReadOnly = true;
            //dataGridViewSettings.Columns[1].ReadOnly = true;


            LoadLaserPowerMeasureSetting();
            if (Equipment.Machine_LaserType_CO2)
            {
                dataGridViewSettings.Rows.Add("Frequency(Hz)", _setting.Frequency);
                dataGridViewSettings.Rows.Add("PulseWidth(us)", _setting.PulseWidth);
                dataGridViewSettings.Rows.Add("DutyCycle(%)", _setting.DutyCycle);
            }
            else
            {
                dataGridViewSettings.Rows.Add("PowerPercent(%)", _setting.PowerPercent);
                dataGridViewSettings.Rows.Add("Frequency(Hz)", _setting.Frequency);
                dataGridViewSettings.Rows.Add("PulseWidth(us)", _setting.PulseWidth);
            }


        }

        private void buttonApplyAndFire_Click(object sender, EventArgs e)
        {
            try
            {
                string strTemp = string.Empty;
                var mb = new QMC.Core.MessageBoxOk();
                if (!m_bReadyLaserPowerMeasure)
                {
                    strTemp = "레이저 파워 측정을 시작하기 전에 준비 상태를 확인하세요.";
                    mb.ShowDialog("Error!", strTemp);

                    return;
                }
                
                if (numericUpDownDuration.Value <= 0)
                {
                    strTemp = string.Format("Duration는 0보다 큰 값이어야 합니다. 현재 값: {0}", numericUpDownDuration.Value);
                    mb.ShowDialog("Error!", strTemp);
                    return;
                }

                if (workStage.rtc == null)
                {
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
                            case "Frequency(Hz)": _setting.Frequency = value; break;
                            case "PulseWidth(us)": _setting.PulseWidth = value; break;
                            case "DutyCycle(%)":
                                if (Equipment.Machine_LaserType_CO2)
                                {
                                    if (value < 2.5f || value >= 20.0f)
                                    {
                                        strTemp = string.Format($"DutyCycle은 2.5% 이상, 20% 미만이어야 합니다.\n입력값: {value:F2}%", "DutyCycle 제한");
                                        mb.ShowDialog("Error!", strTemp);
                                        UpdateSettingRow("DutyCycle(%)", _setting.DutyCycle);
                                        return;
                                    }
                                    else
                                    {
                                        _setting.DutyCycle = value;
                                        UpdateSettingRow("PulseWidth(us)", _setting.PulseWidth);
                                    }
                                }
                                break;
                        }
                    }
                    else
                    {
                        switch (param)
                        {
                            case "PowerPercent(%)": _setting.PowerPercent = value; break;
                            case "Frequency(Hz)": _setting.Frequency = value; break;
                            case "PulseWidth(us)": _setting.PulseWidth = value; break;
                        }
                    }
                }

                bool result = false;
                if (!Equipment.Machine_LaserType_CO2) // UV인 경우.
                {
                    if (_setting.PowerPercent < 0 || _setting.PowerPercent > 100)
                    {
                        strTemp = string.Format("PowerPercent는 0에서 100 사이의 값이어야 합니다.");
                        mb.ShowDialog("Error!", strTemp);
                        return;
                    }

                    result = SetLaserPower(_setting.PowerPercent);
                    if (!result)
                    {
                        strTemp = string.Format("레이저 파워 변경-Error");
                        mb.ShowDialog("Error!", strTemp);
                        return;
                    }
                }
                else
                {
                    if (_setting.DutyCycle < 2.5f || _setting.DutyCycle >= 20.0f)
                    {
                        strTemp = $"DutyCycle은 2.5% 이상, 20% 미만이어야 합니다.\r\n현재 설정: {_setting.DutyCycle:F2}%";
                        Log.Write("LaserPowerMeasure", "LaserPowerMeasure_Start", strTemp);
                        mb.ShowDialog("Error!", strTemp);
                        return;
                    }
                }

                float duration = (float)numericUpDownDuration.Value;
                duration *= 1000; // 밀리초 단위로 변환
                if (!_scanner.LaserOn(duration, _setting))
                {
                    mb.ShowDialog("Error!", "출력 실패");
                    return;
                }

                m_bStartLaserPowerMeasure = true; // 레이저 출력 시작 상태로 설정
                strTemp = string.Format("레이저 파워 출력 시작, Duration ({0:0.000})초", duration);
                // 진행률 처리
                _cts = new CancellationTokenSource();
                int lastProgress = 0;
                Task<int> delayTask = CreateDelayTask((int)duration, (elapsed) => lastProgress = elapsed, _cts.Token);

                var pf = new ProgressForm("레이저 출력 중", "지정된 시간 동안 레이저를 출력합니다.", delayTask, _scanner);
                pf.StartPosition = FormStartPosition.CenterScreen;  // 화면 중심에 표시되도록 설정
                pf.TopMost = true;
                pf.StopProcess += (obj) =>
                {
                    _cts.Cancel();  // Task 취소
                    _scanner?.LaserAbort();  // 레이저 중단
                };

                pf.ShowDialog();

                SaveLaserPowerMeasureSetting();
                if (delayTask.Result == 0)
                {
                    m_bStartLaserPowerMeasure = false;
                    strTemp = string.Format("레이저 파워 출력 완료, Duration ({0:0.000})초", duration);
                    mb.ShowDialog("Complete!", "출력 성공");
                }
                else
                {
                    m_bStartLaserPowerMeasure = false;
                    strTemp = string.Format("레이저 파워 출력 완료, Duration ({0:0.000})초", duration);
                    mb.ShowDialog("Error!", "출력이 중단되었습니다.");
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            //_scanner.rtc.CtlLaserOff();
            
            //_scanner.LaserOff();
            // or

            if(workStage.rtc == null)
            {
                return;
            }

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

        public bool m_bStartLaserPowerMeasure = false;
        public bool m_bReadyLaserPowerMeasure = false;

        private void button_MeasureReady_Click(object sender, EventArgs e)
        {
            string selectedTarget = comboBoxTargetType.SelectedItem.ToString();
            string strTemp = string.Empty;

            m_bReadyLaserPowerMeasure = false;

            if (Equipment.Machine_LaserType_CO2)
            {
                selectedTarget = "Stage";
            }

            if (selectedTarget == "Top")
            {
                if (Equipment.Machine_LaserType_CO2)
                {
                    strTemp = "CO2는 Top PowerMeter가 없습니다.";
                    Log.Write("SLD-200", Equipment.User_Name, strTemp);
                    new QMC.Core.MessageBoxOk().ShowDialog("Information !", strTemp);
                }

                // LaserShutter_Close
                workStage.workStageParameter.DO_BDS_PowerMeter_BW(false);
                Thread.Sleep(100); // 잠시 대기
                workStage.workStageParameter.DO_BDS_PowerMeter_FW(true);

                // 셔터 닫힘 확인을 최대 5초간 반복 체크
                bool bSuccess = false;
                int timeoutMs = 5000;
                int elapsedMs = 0;
                int intervalMs = 100;
                while (elapsedMs < timeoutMs)
                {
                    if (!workStage.workStageParameter.DI_BDS_PowerMeter_BW_Check() &&
                         workStage.workStageParameter.DI_BDS_PowerMeter_FW_Check())
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
                    new QMC.Core.MessageBoxOk().ShowDialog("Error !", strTemp);
                    return;
                }
            }
            else if (selectedTarget == "Stage")
            {
                // LaserShutter_Open
                workStage.workStageParameter.DO_BDS_PowerMeter_BW(true);
                Thread.Sleep(100); // 잠시 대기
                workStage.workStageParameter.DO_BDS_PowerMeter_FW(false);

                // 셔터 닫힘 확인을 최대 5초간 반복 체크
                bool bSuccess = false;
                int timeoutMs = 5000;
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
                    new QMC.Core.MessageBoxOk().ShowDialog("Error !", strTemp);
                    return;
                }

                // 스테이지에 대한 로직
                var mb = new QMC.Core.MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "위치 이동 하시겠습니까?"))
                    return;

                strTemp = string.Empty;
                //  속도 설정
                Equipment.Type_Motor_Speed motor_Speed;
                motor_Speed = Equipment.Type_Motor_Speed.Coarse;
                int nIndex = 0; //Teaching Position Index

                nIndex = (int)Vision.Vision_TeachingPosList.Vision_SafetyPos;
                workStage.MovetoWorkStage_TeachingPositionsZ(nIndex, motor_Speed);
                double dPosZ = vision.stVisionTeachingPos[nIndex].Vision_Z;
                Thread.Sleep(500);
                bool bWaitZ = workStage.WaitUntilInPositionAsync(WorkStage.nAxis.Z, dPosZ).Result;
                if (!bWaitZ)
                {
                    strTemp = string.Format("Z-Axis이 이동 실패.");
                    Log.Write("SLD-200", Equipment.User_Name, strTemp);

                    var mb1 = new QMC.Core.MessageBoxOk();
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
                    var mb1 = new QMC.Core.MessageBoxOk();
                    mb1.ShowDialog("Error !", strTemp);
                    return;
                }

                if(Equipment.Machine_LaserType_CO2)
                {
                    //nIndex = (int)Bds.BDS_TeachingPosList.BDS_Mask4Pos;
                    nIndex = comboBox_MaskIndex.SelectedIndex;
                    double dPosMaskY = bds.stBDSTeachingPos[nIndex].Mask_Y;
                    workStage.MovetoWorkStage_ABS_PositionsMaskY(dPosMaskY, motor_Speed);
                    Thread.Sleep(500);
                    bool bWaitMaskY = workStage.WaitUntilInPositionAsync(WorkStage.nAxis.MASK_Y, dPosMaskY).Result;
                    if (!bWaitMaskY)
                    {
                        strTemp = string.Format("MaskY-Axis이 이동 실패.");
                        Log.Write("SLD-200", Equipment.User_Name, strTemp);
                        var mb1 = new QMC.Core.MessageBoxOk();
                        mb1.ShowDialog("Error !", strTemp);
                        return;
                    }

                    nIndex = comboBox_BETPositionIndex.SelectedIndex;
                    workStage.LaserDrillingStepBETChange(nIndex);
                }
            }

            m_bReadyLaserPowerMeasure = true;

            strTemp = string.Format("준비 완료.");
            Log.Write("SLD-200", Equipment.User_Name, strTemp);
            var mb2 = new QMC.Core.MessageBoxOk();
            mb2.ShowDialog("Complete !", strTemp);
        }

        private void comboBoxTargetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            _setting.PowerMeterType = (comboBoxTargetType.SelectedIndex);
            //SaveLaserPowerMeasureSetting();
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
                NativeMethods.WritePrivateProfileString("Laser", "MaskIndex", comboBox_MaskIndex.SelectedIndex.ToString(), iniPath);
                NativeMethods.WritePrivateProfileString("Laser", "BetIndex", comboBox_BETPositionIndex.SelectedIndex.ToString(), iniPath);
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

            string strTemp = string.Empty;
            var mb = new QMC.Core.MessageBoxOk();
            if (!File.Exists(iniPath))
            {
                strTemp = "LaserPowerMeasure 설정 파일이 없습니다.\r\n[기본값으로 시작합니다.]";
                mb.ShowDialog("Info", strTemp);
                return;
            }

            _setting.Initialize();              //초기화 후 Data Load.

            NativeMethods.GetPrivateProfileString("Laser", "TargetTypeIndex", "0", temp, 255, iniPath);
            comboBoxTargetType.SelectedIndex = Equipment.ToInt(temp.ToString());
            _setting.PowerMeterType = comboBoxTargetType.SelectedIndex;

            NativeMethods.GetPrivateProfileString("Laser", "Duration", "100000", temp, 255, iniPath);
            numericUpDownDuration.Value = (decimal)Equipment.ToDouble(temp.ToString());
            _setting.Duration = (int)numericUpDownDuration.Value;

            if (Equipment.Machine_LaserType_CO2)
            {
                NativeMethods.GetPrivateProfileString("Laser", "Frequency", "7000", temp, 255, iniPath);
                _setting.Frequency = (float)Equipment.ToDouble(temp.ToString());

                NativeMethods.GetPrivateProfileString("Laser", "PulseWidth", "1", temp, 255, iniPath);
                _setting.PulseWidth = (float)Equipment.ToDouble(temp.ToString());

                NativeMethods.GetPrivateProfileString("Laser", "DutyCycle", "1", temp, 255, iniPath);
                _setting.DutyCycle = (float)Equipment.ToDouble(temp.ToString());

                NativeMethods.GetPrivateProfileString("Laser", "MaskIndex", "1", temp, 255, iniPath);
                comboBox_MaskIndex.SelectedIndex = Equipment.ToInt(temp.ToString());

                NativeMethods.GetPrivateProfileString("Laser", "BetIndex", "1", temp, 255, iniPath);
                comboBox_BETPositionIndex.SelectedIndex = Equipment.ToInt(temp.ToString());

                _setting.MaskIndex = comboBox_MaskIndex.SelectedIndex;
                _setting.BETIndex = comboBox_BETPositionIndex.SelectedIndex;
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
            var mb = new QMC.Core.MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "파워 측정 하시겠습니까?"))
                return;

            string strTemp = string.Empty;

            _setting.PowerMeterType = comboBoxTargetType.SelectedIndex;
            _setting.MaskIndex = comboBox_MaskIndex.SelectedIndex;
            _setting.BETIndex = comboBox_BETPositionIndex.SelectedIndex;

            SaveLaserPowerMeasureSetting();

            if (_setting.PowerMeterType == 0) // Top
            {
                if (Equipment.Machine_LaserType_CO2)
                {
                    strTemp = "Top PowerMeter는 UV장비에서만 지원합니다.";
                    Log.Write("SLD-200", Equipment.User_Name, strTemp);
                    new QMC.Core.MessageBoxOk().ShowDialog("Error !", strTemp);
                    return;
                }
            }

            ResetPowerMeasureList();
            workStage.m_Sequence_LaserPowerMeasure.Start();

            // Task로 완료 대기
            CancellationTokenSource cts = new CancellationTokenSource();
            Task<int> waitTask = CreateSequenceWaitTask(() => workStage.m_Sequence_LaserPowerMeasure.IsCompleted, cts.Token);

            var pf = new ProgressForm("파워 측정 중", "시퀀스 완료까지 기다리는 중입니다...", waitTask, _scanner);
            pf.TopMost = true;
            pf.StartPosition = FormStartPosition.CenterScreen;  // 화면 중심에 표시되도록 설정
            pf.StopProcess += (obj) =>
            {
                cts.Cancel();  // 취소 요청
                _scanner?.LaserAbort();  // 레이저 중지
                workStage.m_Sequence_LaserPowerMeasure.Reset(); // 시퀀스 강제 종료
            };

            pf.ShowDialog();

            if (waitTask.Result == 0)
            {
                new QMC.Core.MessageBoxOk().ShowDialog("Complete!", "측정 완료");
            }
            else
            {
                new QMC.Core.MessageBoxOk().ShowDialog("Canceled!", "측정 중단됨");
            }

        }

        private void button_SeqStop_Click(object sender, EventArgs e)
        {
            workStage.m_Sequence_LaserPowerMeasure.Reset(); //Stop
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
            return;
            var mb = new QMC.Core.MessageBoxOk();
            float duration = (float)numericUpDownDuration.Value;
            //if (!_scanner.LaserOn(duration, _setting))
            //{
            //    mb.ShowDialog("Error!", "출력 실패");
            //    return;
            //}

            // 진행률 처리
            _cts = new CancellationTokenSource();
            int lastProgress = 0;
            Task<int> delayTask = CreateDelayTask((int)duration, (elapsed) => lastProgress = elapsed, _cts.Token);

            var pf = new ProgressForm("레이저 출력 중", "지정된 시간 동안 레이저를 출력합니다.", delayTask, _scanner);
            pf.StartPosition = FormStartPosition.CenterScreen;  // 화면 중심에 표시되도록 설정
            pf.StopProcess += (obj) =>
            {
                _cts.Cancel();  // Task 취소
                _scanner?.LaserAbort();  // 레이저 중단
            };

            pf.ShowDialog();

            //SaveLaserPowerMeasureSetting();
            if (delayTask.Result == 0)
                mb.ShowDialog("Complete!", "출력 성공");
            else
                mb.ShowDialog("Error!", "출력이 중단되었습니다.");

            return;

            // 예시 파워 값들
            float[] testPowers = new float[] { 121.3f, 122.7f, 120.8f, 124.2f, 123.1f };

            foreach (float power in testPowers)
            {
                UpdatePowerMeasureLog(power);
                Thread.Sleep(200); // UI 업데이트 확인을 위한 딜레이 (선택)
            }

            MessageBox.Show("UpdatePowerMeasureLog 테스트 완료");
        }

        private void dataGridViewSettings_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != 1) return;

            var param = dataGridViewSettings.Rows[e.RowIndex].Cells[0].Value?.ToString();
            var valueStr = dataGridViewSettings.Rows[e.RowIndex].Cells[1].Value?.ToString();
            if (!float.TryParse(valueStr, out float value))
                return;

            var mb = new QMC.Core.MessageBoxOk();
            string strTemp = string.Empty;
            switch (param)
            {
                case "Frequency(Hz)":
                    _setting.Frequency = value;
                    UpdateSettingRow("DutyCycle(%)", _setting.DutyCycle);
                    UpdateSettingRow("PulseWidth(us)", _setting.PulseWidth);
                    break;

                case "PulseWidth(us)":
                    float BeforePulseWidth = 0;
                    BeforePulseWidth = _setting.PulseWidth;
                    _setting.PulseWidth = value;

                    // DutyCycle 자동 계산 후 검증
                    if (Equipment.Machine_LaserType_CO2)
                    {
                        float duty = _setting.DutyCycle;
                        if (duty < 2.5f || duty >= 20.0f)
                        {
                            strTemp = string.Format($"DutyCycle은 2.5% 이상, 20% 미만이어야 합니다.\n입력값: {value:F2}%", "DutyCycle 제한");
                            mb.ShowDialog("Error!", strTemp);
                            //UpdateSettingRow("DutyCycle(%)", _setting.DutyCycle);
                            // 원래 값으로 되돌림
                            UpdateSettingRow("PulseWidth(us)", BeforePulseWidth); // 다시 표시
                            //return;
                        }
                        else
                        {
                            UpdateSettingRow("DutyCycle(%)", _setting.DutyCycle);
                        }
                    }
                    //_setting.PulseWidth = value;
                    //UpdateSettingRow("DutyCycle(%)", _setting.DutyCycle);
                    break;

                case "DutyCycle(%)":
                    if (Equipment.Machine_LaserType_CO2)
                    {
                        if (value < 2.5f || value >= 20.0f)
                        {
                            strTemp = string.Format($"DutyCycle은 2.5% 이상, 20% 미만이어야 합니다.\n입력값: {value:F2}%", "DutyCycle 제한");
                            mb.ShowDialog("Error!", strTemp);
                            UpdateSettingRow("DutyCycle(%)", _setting.DutyCycle);
                        }
                        else
                        {
                            _setting.DutyCycle = value;
                            UpdateSettingRow("PulseWidth(us)", _setting.PulseWidth);
                        }
                    }
                    break;

                case "PowerPercent(%)":
                    _setting.PowerPercent = value;
                    break;
            }
        }

        private void UpdateSettingRow(string paramName, float value)
        {
            foreach (DataGridViewRow row in dataGridViewSettings.Rows)
            {
                if (row.Cells[0].Value?.ToString() == paramName)
                {
                    row.Cells[1].Value = value.ToString("F2");
                    break;
                }
            }
        }


        private void dataGridViewSettings_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dataGridViewSettings.IsCurrentCellDirty)
                dataGridViewSettings.CommitEdit(DataGridViewDataErrorContexts.Commit);

            dataGridViewSettings.Refresh();
            SaveLaserPowerMeasureSetting();
        }

        private void comboBox_MaskIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            _setting.MaskIndex = (comboBox_MaskIndex.SelectedIndex);
            SaveLaserPowerMeasureSetting();
        }

        private void comboBox_BETPositionIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            _setting.BETIndex = (comboBox_BETPositionIndex.SelectedIndex);
            SaveLaserPowerMeasureSetting();
        }


        private CancellationTokenSource _cts;
        private Task<int> CreateDelayTask(int durationMs, Action<int> onProgress, CancellationToken token)
        {
            return Task.Run(async () =>
            {
                int elapsed = 0;
                int interval = 100;

                while (elapsed < durationMs)
                {
                    if (token.IsCancellationRequested)
                        return -1;  // 취소됨

                    await Task.Delay(interval);
                    elapsed += interval;
                    onProgress?.Invoke(elapsed);
                }

                return 0;  // 완료
            }, token);
        }

        private Task<int> CreateSequenceWaitTask(Func<bool> isCompleted, CancellationToken token)
        {
            return Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    await Task.Delay(100);

                    if (isCompleted())
                        return 0;  // 완료
                }

                return -1;  // 중단됨
            }, token);
        }

        private void FormNewSub_LaserPowerMeasure_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                timerLaserPowerMeasureStatus?.Start();
                LoadLaserPowerMeasureSetting();
                RefreshPowerMeasureList();
            }
            else
            {
                timerLaserPowerMeasureStatus?.Stop();
            }
        }
    }
}
