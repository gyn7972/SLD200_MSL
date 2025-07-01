using QMC.Common;
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

            _scanner = scanner;
            InitSettingTable();

            comboBoxTargetType.SelectedIndex = 0; // 기본값으로 "Top" 선택
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

            _setting.Initialize();
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
                    //MessageBox.Show("레이저 출력 성공", "완료", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                    MessageBox.Show("레이저 출력 실패", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                // Shutter 닫기
                if (workStage.workStageParameter.DI_BDS_PowerMeter_BW_Check() &&
                    !workStage.workStageParameter.DI_BDS_PowerMeter_FW_Check())
                {
                    //실패 메시지
                    strTemp = string.Format("Shutter 닫기 실패.");
                    Log.Write("SLD-200", Equipment.User_Name, strTemp);

                    var mb1 = new MessageBoxOk();
                    mb1.ShowDialog("Error !", strTemp);
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
    }
}
