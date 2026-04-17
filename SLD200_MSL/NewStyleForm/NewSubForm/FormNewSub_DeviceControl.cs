using QMC.Common;
using QMC.Common.UI;
using QMC.Common.Modules;
using QMC.Common.Parts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QMC.Common.Equipment;
using QMC.Common.VisionPart;

namespace SLD200.NewStyleForm.NewSubForm
{
    public partial class FormNewSub_DeviceControl : UserControl //Form
    {
        // 장비 초기화 상태 확인
        private Dictionary<string, Func<bool>> deviceStatusGetters;
        private Dictionary<string, PictureBox> devicePictureBoxes;

        static WorkStage workStage;
        static Bds bds;

        private System.Windows.Forms.Timer timerDeivceStatus;
        private bool _isRunning_DeviceStatus = false;

        private List<(string Name, Action OnAction, Action OffAction, PictureBox Pic)> deviceList;

        public FormNewSub_DeviceControl()
        {
            InitializeComponent();

            this.AutoScaleMode = AutoScaleMode.None;
            this.DoubleBuffered = true;

            foreach (Module module in Equipment.Modules)
            {
                if (module.Name == "WorkStage") workStage = module as WorkStage;
                if (module.Name == "BDS") bds = module as Bds;
            }

            timerDeivceStatus = new System.Windows.Forms.Timer();
            timerDeivceStatus.Interval = 500;
            timerDeivceStatus.Tick += TimerDeivceStatus_Tick;
            timerDeivceStatus.Start();

            InitializeDeviceStatusBindings();

            InitializeDeviceList();

            if(Equipment.Machine_LaserType_CO2)
            {
                lblPowermeterBds.Visible = false;
                picPowermeterBds.Visible = false;
                btnPowermeterBdsOn.Visible = false;
                btnPowermeterBdsOff.Visible = false;
            }
            else
            {
                lblBeamExpander.Visible = false;
                picBeamExpander.Visible = false;
                btnBeamExpanderOn.Visible = false;
                btnBeamExpanderOff.Visible = false;
            }
            
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // 엔터 또는 스페이스 키 눌렀을 때 무시
            if (keyData == Keys.Enter || keyData == Keys.Space)
                return true;

            return base.ProcessCmdKey(ref msg, keyData);
        }

        //protected override void OnFormClosing(FormClosingEventArgs e)
        //{
        //    if (e.CloseReason == CloseReason.UserClosing)
        //    {
        //        // 사용자가 닫기(X 버튼) 누른 경우 → 숨기기만 하고 종료 안 함
        //        e.Cancel = true;
        //        this.Hide();
        //        return;
        //    }

        //    // 그 외 종료 (Application.Exit 등) → 정식 해제
        //    base.OnFormClosing(e);
        //}

        public void DisposeSemiAutoResources()
        {
            if (timerDeivceStatus != null)
            {
                timerDeivceStatus.Stop();
                timerDeivceStatus.Tick -= TimerDeivceStatus_Tick;
                timerDeivceStatus.Dispose();
                timerDeivceStatus = null;
            }
            // 필요 시 다른 모듈 정리도 여기에
        }

        private void TimerDeivceStatus_Tick(object sender, EventArgs e)
        {
            if (_isRunning_DeviceStatus)
                return;

            try
            {
                _isRunning_DeviceStatus = true;
                Timer_DeviceStatusRun();
            }
            catch (Exception ex)
            {
                // 로그 남기기
                Log.Write(ex);
                _isRunning_DeviceStatus = false;
            }
            finally
            {
                _isRunning_DeviceStatus = false;
            }
        }

        private void Timer_DeviceStatusRun()
        {
            UpdateDeviceStatusImages();
        }

        private void SetDeviceStatus(PictureBox pic, bool isOn)
        {
            if (pic != null)
                pic.Image = isOn ? Properties.Resources.DioEllipseOn : Properties.Resources.DioEllipseOff;
        }
        private void TryDeviceControl(string name, Action controlAction, PictureBox pic, bool isOn)
        {
            try
            {
                controlAction.Invoke();
                SetDeviceStatus(pic, isOn);
                Log.Write("SLD-200", Equipment.User_Name, "DeviceControl", $"{name} {(isOn ? "ON" : "OFF")}");
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                MessageBox.Show($"{name} {(isOn ? "ON" : "OFF")} 실패: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLaserOn_Click(object sender, EventArgs e)
        {
            if (Equipment._InitDeviceStatus.Laser)
                return;

            if (workStage == null)
                return;

            if (Equipment.Machine_LaserType_CO2)
            {
                TryDeviceControl("레이저", 
                    () => workStage.workStageParameter.DO_Laser_Enable(true), picLaser, true);
                Equipment._InitDeviceStatus.Laser = true;
                TryDeviceControl("레이저", () =>
                {
                    workStage.LaserCo2_Init();
                }, picLaser, true);
            }
            else
            {
                TryDeviceControl("레이저", () =>
                {
                    workStage.m_bRapidLxLaser_UserConnect = true;
                    workStage.RapidLxLaser_Comm_Init();
                }, picLaser, true);
            } 
        }

        private void btnLaserOff_Click(object sender, EventArgs e)
        {
            if (!Equipment._InitDeviceStatus.Laser)
                return;

            if (Equipment.Machine_LaserType_CO2)
            {
                TryDeviceControl("레이저", () => workStage.workStageParameter.DO_Laser_Enable(false), picLaser, false);
                Equipment._InitDeviceStatus.Laser = false;

                TryDeviceControl("레이저", () =>
                {
                    workStage.LaserCo2_Close();
                }, picLaser, false);
            }
            else
                TryDeviceControl("레이저", () =>
                {
                    workStage.m_bRapidLxLaser_UserConnect = false;
                    workStage.RapidLxLaser_Comm_Close();
                }, picLaser, false);
        }

        private void btnScannerOn_Click(object sender, EventArgs e) 
            => TryDeviceControl("스캐너", () => workStage.Sirius_Init(), picScanner, true);
        private void btnScannerOff_Click(object sender, EventArgs e) 
            => TryDeviceControl("스캐너", () => workStage.Sirius_Close(), picScanner, false);

        private void btnChillerOn_Click(object sender, EventArgs e) 
            => TryDeviceControl("칠러", () => workStage.ChillerComm_Init(), picChiller, true);
        private void btnChillerOff_Click(object sender, EventArgs e) 
            => TryDeviceControl("칠러", () => workStage.ChillerComm_Close(), picChiller, false);
        private void btnMotionOn_Click(object sender, EventArgs e)
        {
            // 상시 init
            //=> TryDeviceControl("모션", () => motionController.Init(), picMotion, true);
        }
        private void btnMotionOff_Click(object sender, EventArgs e)
        {
            // 상시
            // => TryDeviceControl("모션", () => motionController.Close(), picMotion, false);
        }
        private void btnIOOn_Click(object sender, EventArgs e)
        {
            // 상시
            //=> TryDeviceControl("IO", () => ioController.Init(), picIO, true);
        }
        private void btnIOOff_Click(object sender, EventArgs e)
        {
            // 상시
            //=> TryDeviceControl("IO", () => ioController.Close(), picIO, false);
        }
        private void btnHeightSensorOn_Click(object sender, EventArgs e) 
            => TryDeviceControl("높이 센서", () => workStage.LaserSensor_Socket_Connect(), picHeightSensor, true);
        private void btnHeightSensorOff_Click(object sender, EventArgs e) 
            => TryDeviceControl("높이 센서", () => workStage.LaserSensor_Socket_Disconnect(), picHeightSensor, false);

        private void btnElectroRegulatorOn_Click(object sender, EventArgs e) 
            => TryDeviceControl("정전압 조절기", () => workStage.ElectroPneumaticRegulator_Comm_Init(), picElectroRegulator, true);
        private void btnElectroRegulatorOff_Click(object sender, EventArgs e) 
            => TryDeviceControl("정전압 조절기", () => workStage.ElectroPneumaticRegulator_Comm_Close(), picElectroRegulator, false);

        private void btnIlluminatorOn_Click(object sender, EventArgs e) 
            => TryDeviceControl("조명", () => workStage.Illuminator_Init(), picIlluminator, true);
        private void btnIlluminatorOff_Click(object sender, EventArgs e) 
            => TryDeviceControl("조명", () => workStage.Illuminator_Close(), picIlluminator, false);
         
        private void btnDustcollectorUpperOn_Click(object sender, EventArgs e) 
            => TryDeviceControl("집진기(상)", () => bds.InitDustCollector(DustCollectorController.CollectorPosition.Upper), picDustcollectorUpper, true);
        private void btnDustcollectorUpperOff_Click(object sender, EventArgs e) 
            => TryDeviceControl("집진기(상)", () => bds.DisconnectDustCollector(DustCollectorController.CollectorPosition.Upper), picDustcollectorUpper, false);

        private void btnDustcollectorLowerOn_Click(object sender, EventArgs e) 
            => TryDeviceControl("집진기(하)", () => bds.InitDustCollector(DustCollectorController.CollectorPosition.Lower), picDustcollectorLower, true);
        private void btnDustcollectorLowerOff_Click(object sender, EventArgs e) 
            => TryDeviceControl("집진기(하)", () => bds.DisconnectDustCollector(DustCollectorController.CollectorPosition.Lower), picDustcollectorLower, false);

        private void btnPowermeterBdsOn_Click(object sender, EventArgs e) 
            => TryDeviceControl("파워미터(BDS)", () => workStage.PowerMeterComm_ExitPos_Init(), picPowermeterBds, true);
        private void btnPowermeterBdsOff_Click(object sender, EventArgs e) 
            => TryDeviceControl("파워미터(BDS)", () => workStage.PowerMeterComm_ExitPos_Close(), picPowermeterBds, false);

        private void btnPowermeterStageOn_Click(object sender, EventArgs e) 
            => TryDeviceControl("파워미터(Stage)", () => workStage.PowerMeterComm_TargetPos_Init(), picPowermeterStage, true);
        private void btnPowermeterStageOff_Click(object sender, EventArgs e) 
            => TryDeviceControl("파워미터(Stage)", () => workStage.PowerMeterComm_TargetPos_Close(), picPowermeterStage, false);

        private void btnBeamExpanderOn_Click(object sender, EventArgs e) 
            => TryDeviceControl("빔 익스팬더", () => workStage.BeamExpanderComm_Init(), picBeamExpander, true);
        private void btnBeamExpanderOff_Click(object sender, EventArgs e) 
            => TryDeviceControl("빔 익스팬더", () => workStage.BeamExpanderComm_Close(), picBeamExpander, false);

        private void btnCameraFineOn_Click(object sender, EventArgs e)
        => TryDeviceControl("정밀 카메라", () => workStage.InitCameraFine(), picCameraFine, true);
        private void btnCameraFineOff_Click(object sender, EventArgs e) 
            => TryDeviceControl("정밀 카메라", () => workStage.CloseCameraFine(), picCameraFine, false);

        private void btnCameraPreOn_Click(object sender, EventArgs e) 
            => TryDeviceControl("프리뷰 카메라", () => workStage.InitCameraPre(), picCameraPre, true);
        private void btnCameraPreOff_Click(object sender, EventArgs e) 
            => TryDeviceControl("프리뷰 카메라", () => workStage.CloseCameraPre(), picCameraPre, false);


        private void InitializeDeviceList()
        {
            deviceList = new List<(string, Action, Action, PictureBox)>
            {
                ("레이저",
                    () => {
                        if (Equipment.Machine_LaserType_CO2)
                            workStage.workStageParameter.DO_Laser_Enable(true);
                        else {
                            workStage.m_bRapidLxLaser_UserConnect = true;
                            workStage.RapidLxLaser_Comm_Init();
                        }
                    },
                    () => {
                        if (Equipment.Machine_LaserType_CO2)
                            workStage.workStageParameter.DO_Laser_Enable(false);
                        else {
                            workStage.m_bRapidLxLaser_UserConnect = false;
                            workStage.RapidLxLaser_Comm_Close();
                        }
                    },
                    picLaser
                ),

                ("스캐너", () => workStage.Sirius_Init(), () => workStage.Sirius_Close(), picScanner),
                ("칠러", workStage.ChillerComm_Init, workStage.ChillerComm_Close, picChiller),
                ("높이센서", () => workStage.LaserSensor_Socket_Connect(), workStage.LaserSensor_Socket_Disconnect, picHeightSensor),

                ("집진기(상)", () => bds.InitDustCollector(DustCollectorController.CollectorPosition.Upper),
                              () => bds.DisconnectDustCollector(DustCollectorController.CollectorPosition.Upper),
                              picDustcollectorUpper),

                ("집진기(하)", () => bds.InitDustCollector(DustCollectorController.CollectorPosition.Lower),
                              () => bds.DisconnectDustCollector(DustCollectorController.CollectorPosition.Lower),
                              picDustcollectorLower),

                ("Powermeter BDS", workStage.PowerMeterComm_ExitPos_Init, workStage.PowerMeterComm_ExitPos_Close, picPowermeterBds),
                ("Powermeter Stage", workStage.PowerMeterComm_TargetPos_Init, workStage.PowerMeterComm_TargetPos_Close, picPowermeterStage),

                ("BeamExpander", workStage.BeamExpanderComm_Init, workStage.BeamExpanderComm_Close, picBeamExpander),

                ("카메라 Fine", workStage.InitCameraFine, workStage.CloseCameraFine, picCameraFine),
                ("카메라 Pre", workStage.InitCameraPre, workStage.CloseCameraPre, picCameraPre),

                ("전동조절기", workStage.ElectroPneumaticRegulator_Comm_Init, workStage.ElectroPneumaticRegulator_Comm_Close, picElectroRegulator),
                ("조명", workStage.Illuminator_Init, workStage.Illuminator_Close, picIlluminator)
            };
        }

        private void btnAllOn_Click(object sender, EventArgs e)
        {
            if (!Equipment._InitDeviceStatus.MotionIo)
            {
                MessageBox.Show("Motion 시스템이 먼저 켜져 있어야 합니다.", "인터락", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (var (Name, OnAction, _, Pic) in deviceList)
            {
                TryDeviceControl(Name, OnAction, Pic, true);
            }

            //SetDeviceStatus(picLaser, true);
            //SetDeviceStatus(picScanner, true);
            //SetDeviceStatus(picChiller, true);
            //SetDeviceStatus(picMotion, true);
            //SetDeviceStatus(picIO, true);
            //SetDeviceStatus(picHeightSensor, true);
            //SetDeviceStatus(picDustcollectorUpper, true);
            //SetDeviceStatus(picDustcollectorLower, true);
            //SetDeviceStatus(picPowermeterBds, true);
            //SetDeviceStatus(picPowermeterStage, true);
            //SetDeviceStatus(picBeamExpander, true);
            //SetDeviceStatus(picCameraFine, true);
            //SetDeviceStatus(picCameraPre, true);
            //SetDeviceStatus(picElectroRegulator, true);
            //SetDeviceStatus(picIlluminator, true);
        }

        private void btnAllOff_Click(object sender, EventArgs e)
        {
            foreach (var (Name, _, OffAction, Pic) in deviceList)
            {
                TryDeviceControl(Name, OffAction, Pic, false);
            }

            //SetDeviceStatus(picLaser, false);
            //SetDeviceStatus(picScanner, false);
            //SetDeviceStatus(picChiller, false);
            ////SetDeviceStatus(picMotion, false);
            ////SetDeviceStatus(picIO, false);
            //SetDeviceStatus(picHeightSensor, false);
            //SetDeviceStatus(picDustcollectorUpper, false);
            //SetDeviceStatus(picDustcollectorLower, false);
            //SetDeviceStatus(picPowermeterBds, false);
            //SetDeviceStatus(picPowermeterStage, false);
            //SetDeviceStatus(picBeamExpander, false);
            //SetDeviceStatus(picCameraFine, false);
            //SetDeviceStatus(picCameraPre, false);
            //SetDeviceStatus(picElectroRegulator, false);
            //SetDeviceStatus(picIlluminator, false);
        }


        public void Comm_Init()
        {
            workStage.Illuminator_Init();

            //  Power Meter (Exit Position) - for UV Only
            if (workStage.m_powerMeter_ExitPos_Comm == null)
            {
                workStage.PowerMeterComm_ExitPos_Init();
            }
            else
            {
                if (!workStage.m_powerMeter_ExitPos_Comm.IsOpen)
                    workStage.PowerMeterComm_ExitPos_Init();
            }

            //  Power Meter (Target Position)
            if (workStage.m_powerMeter_TargetPos_Comm == null)
            {
                workStage.PowerMeterComm_TargetPos_Init();
            }
            else
            {
                if (!workStage.m_powerMeter_TargetPos_Comm.IsOpen)
                    workStage.PowerMeterComm_TargetPos_Init();
            }

            //  Motorized Beam Expander - for CO₂Only
            if (workStage.m_beamExpander_Comm == null)
            {
                workStage.BeamExpanderComm_Init();
            }
            else
            {
                if (!workStage.m_beamExpander_Comm.IsOpen)
                    workStage.BeamExpanderComm_Init();
            }

            //  Dust Collector (Upper Position)
            if (!bds.DustCollector_Upper.IsConnected)
            {
                bds.InitDustCollector(DustCollectorController.CollectorPosition.Upper);
            }
            else
            {
                if (!bds.DustCollector_Upper.IsConnected)
                {
                    bds.InitDustCollector(DustCollectorController.CollectorPosition.Upper);
                }
            }

            //  Dust Collector (Lower Position)
            if (!bds.DustCollector_Lower.IsConnected)
            {
                bds.InitDustCollector(DustCollectorController.CollectorPosition.Lower);
            }
            else
            {
                if (!bds.DustCollector_Lower.IsConnected)
                {
                    bds.InitDustCollector(DustCollectorController.CollectorPosition.Lower);
                }
            }

            //if (workStage.m_dustCollector_UpperPos_Comm == null)
            //{
            //    workStage.DustCollector_UpperPos_Comm_Init();
            //}
            //else
            //{
            //    if (!workStage.m_dustCollector_UpperPos_Comm.IsOpen)
            //        workStage.DustCollector_UpperPos_Comm_Init();
            //}
            ////  Dust Collector (Lower Position)
            //if (workStage.m_dustCollector_LowerPos_Comm == null)
            //{
            //    workStage.DustCollector_LowerPos_Comm_Init();

            //    bds.InitDustCollector(DustCollectorController.CollectorPosition.Lower);
            //}
            //else
            //{
            //    if (!workStage.m_dustCollector_LowerPos_Comm.IsOpen)
            //        workStage.DustCollector_LowerPos_Comm_Init();

            //    bds.InitDustCollector(DustCollectorController.CollectorPosition.Lower);
            //}


            //  Electro Pneumatic Regulator
            if (workStage.m_electroRegulator_Comm == null)
            {
                workStage.ElectroPneumaticRegulator_Comm_Init();
            }
            else
            {
                if (!workStage.m_electroRegulator_Comm.IsOpen)
                    workStage.ElectroPneumaticRegulator_Comm_Init();
            }

            // Laser m_rapidLxLaser_Comm
            if (Equipment.Machine_LaserType_CO2)
            {
                if (!workStage.workStageParameter.IsDO_Laser_Enable())
                {
                    // CO2 - Test 확인하고 하자.
                    // workStage.workStageParameter.DO_Laser_Enable(true);
                }
            }
            else
            {
                if (workStage.m_rapidLxLaser_Comm == null)
                {
                    workStage.m_bRapidLxLaser_UserConnect = true;
                    workStage.RapidLxLaser_Comm_Init();
                }
                else
                {
                    if (!workStage.m_rapidLxLaser_Comm.IsOpen)
                    {
                        workStage.m_bRapidLxLaser_UserConnect = true;
                        workStage.RapidLxLaser_Comm_Init();
                    }
                }
            }

            //  Laser Height Sensor
            if (workStage.m_SocketLaserHeightSensor == null)
            {
                workStage.LaserSensor_Socket_Connect();
            }
        }

        //초기화 상태 함수 확인 
        private void InitializeDeviceStatusBindings()
        {
            deviceStatusGetters = new Dictionary<string, Func<bool>>
            {
                { "Motion", () => Equipment._InitDeviceStatus.MotionIo },
                { "IO", () => Equipment._InitDeviceStatus.MotionIo },
                { "Laser", () => Equipment._InitDeviceStatus.Laser },
                { "Scanner", () => Equipment._InitDeviceStatus.Scanner },
                { "PowerMeter_Bds", () => Equipment._InitDeviceStatus.PowerMeter_Bds },
                { "PowerMeter_Stage", () => Equipment._InitDeviceStatus.PowerMeter_Stage },
                { "BeamExpander", () => Equipment._InitDeviceStatus.BeamExpander },
                { "DustCollector_Upper", () => Equipment._InitDeviceStatus.DustCollector_Upper },
                { "DustCollector_Lower", () => Equipment._InitDeviceStatus.DustCollector_Lower },
                { "Chiller", () => Equipment._InitDeviceStatus.Chiller },
                { "ElectroRegulator", () => Equipment._InitDeviceStatus.ElectroRegulator },
                { "HeightSensor", () => Equipment._InitDeviceStatus.HeightSensor },
                { "CameraFine", () => Equipment._InitDeviceStatus.CameraFine },
                { "CameraPre", () => Equipment._InitDeviceStatus.CameraPre },
                { "Illuminator", () => Equipment._InitDeviceStatus.Illuminator },
            };

            devicePictureBoxes = new Dictionary<string, PictureBox>
            {
                { "Motion", picMotion },
                { "IO", picIO },
                { "Laser", picLaser },
                { "Scanner", picScanner },
                { "PowerMeter_Bds", picPowermeterBds },
                { "PowerMeter_Stage", picPowermeterStage },
                { "BeamExpander", picBeamExpander },
                { "DustCollector_Upper", picDustcollectorUpper },
                { "DustCollector_Lower", picDustcollectorLower },
                { "Chiller", picChiller },
                { "ElectroRegulator", picElectroRegulator },
                { "HeightSensor", picHeightSensor },
                { "CameraFine", picCameraFine },
                { "CameraPre", picCameraPre },
                { "Illuminator", picIlluminator },
            };
        }
        private void UpdateDeviceStatusImages()
        {
            foreach (var kv in devicePictureBoxes)
            {
                string key = kv.Key;
                PictureBox pic = kv.Value;

                bool isInit = deviceStatusGetters.ContainsKey(key) && deviceStatusGetters[key]?.Invoke() == true;
                SetValue(pic, isInit);

            }
        }

        private void SetValue(PictureBox pic, bool isInit)
        {
            if (pic.InvokeRequired)
            {
                pic.Invoke(new System.Action(() =>
                {
                    //화면에 출력.

                    SetValue(pic, isInit);
                }));

            }
            else
            {

                pic.Image = isInit
                    ? global::SLD200.Properties.Resources.DioEllipseOn
                    : global::SLD200.Properties.Resources.DioEllipseOff;
            }
        }
    }
    
}
