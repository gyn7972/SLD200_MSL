using QMC.Common;
using QMC.Common.Modules;
using QMC.Common.Parts;
using QMC.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QMC.Common.Modules.Loader;

namespace SLD200.NewStyleForm.NewSubForm
{
    public partial class FormNewSub_Main_SemiAuto : Form
    {
        static WorkStage workStage;
        static Loader loader;
        static Unloader unloader;
        static Vision vision;
        static Bds bds;

        private System.Windows.Forms.Timer timerSemiAuto;
        private bool _isRunning_SemiAuto = false;

        public FormNewSub_Main_SemiAuto()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.DoubleBuffered = true;

            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;
            foreach (Module module in m_collectionModules)
            {
                if (module.Name == "WorkStage") workStage = module as WorkStage;
                if (module.Name == "Loader") loader = module as Loader;
                if (module.Name == "Unloader") unloader = module as Unloader;
                if (module.Name == "Vision") vision = module as Vision;
                if (module.Name == "BDS") bds = module as Bds;
            }

            timerSemiAuto = new System.Windows.Forms.Timer();
            timerSemiAuto.Interval = 100;
            timerSemiAuto.Tick += TimerSemiAuto_Tick;
            timerSemiAuto.Start();

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
            if (timerSemiAuto != null)
            {
                timerSemiAuto.Stop();
                timerSemiAuto.Tick -= TimerSemiAuto_Tick;
                timerSemiAuto.Dispose();
                timerSemiAuto = null;
            }

            // 필요 시 다른 모듈 정리도 여기에
        }

        private void TimerSemiAuto_Tick(object sender, EventArgs e)
        {
            if (_isRunning_SemiAuto)
                return;

            try
            {
                _isRunning_SemiAuto = true;
                Timer_SemiAutoRun();
            }
            catch (Exception ex)
            {
                // 로그 남기기
                Log.Write(ex);
                _isRunning_SemiAuto = false;
            }
            finally
            {
                _isRunning_SemiAuto = false;
            }
        }

        private void Timer_SemiAutoRun()
        {
            // Semi Auto 모드에서 실행할 작업들을 여기에 구현.
            UpdateButtonStatusByState(button_SemiAuto_Loading, loader.m_LoaderWork_Start, loader.IsLoaderComplete());

            //UpdateButtonStatusByState(button_SemiAuto_HeightSensor, workStage.m_LaserDrillingWork_Start, workStage.IsStageComplete(WorkStage.SemiAutoStep.MeasureHeight));
            //UpdateButtonStatusByState(button_SemiAuto_PreAlign, workStage.m_LaserDrillingWork_Start, workStage.IsStageComplete(WorkStage.SemiAutoStep.PreAlign));
            //UpdateButtonStatusByState(button_SemiAuto_FiducialAlign, workStage.m_LaserDrillingWork_Start, workStage.IsStageComplete(WorkStage.SemiAutoStep.FiducialAlign));
            //UpdateButtonStatusByState(button_SemiAuto_LaserDrilling, workStage.m_LaserDrillingWork_Start, workStage.IsStageComplete(WorkStage.SemiAutoStep.Drilling));
            UpdateButtonStatusByState(
                button_SemiAuto_HeightSensor,
                workStage._semiAutoRequest == WorkStage.SemiAutoStep.MeasureHeight,
                workStage.IsStageComplete(WorkStage.SemiAutoStep.MeasureHeight));

            UpdateButtonStatusByState(
                button_SemiAuto_PreAlign,
                workStage._semiAutoRequest == WorkStage.SemiAutoStep.PreAlign,
                workStage.IsStageComplete(WorkStage.SemiAutoStep.PreAlign));

            UpdateButtonStatusByState(
                button_SemiAuto_FiducialAlign,
                workStage._semiAutoRequest == WorkStage.SemiAutoStep.FiducialAlign,
                workStage.IsStageComplete(WorkStage.SemiAutoStep.FiducialAlign));

            UpdateButtonStatusByState(
                button_SemiAuto_LaserDrilling,
                workStage._semiAutoRequest == WorkStage.SemiAutoStep.Drilling,
                workStage.IsStageComplete(WorkStage.SemiAutoStep.Drilling));


            UpdateButtonStatusByState(button_SemiAuto_Unloading, unloader.m_UnloaderWork_Start, unloader.IsUnloaderComplete());
        }

        private void button_SemiAuto_Loading_Click(object sender, EventArgs e)
        {
            Log.Write("GUI", Equipment.User_Name, "ButtonClick", "button_SemiAuto_Loading_Click");
            string strTemp = "";

            bool bTest = false;
            if (bTest)
            {
                if (Equipment.AutoRunStatus)
                {
                    var mb = new MessageBoxOk();
                    mb.ShowDialog("Information !", "장비가 [[ 운전중 ]] 입니다.");
                    return;
                }

                if (!Equipment.AutoManualStatus)
                {
                    var mb = new MessageBoxOk();
                    mb.ShowDialog("Information !", "장비가 [[ AUTO ]] 상태가 아닙니다.");
                    return;
                }

                //  Loader Port 에 자재가 없으면 메세지 창 Pop up
                if (!loader.loaderParameter.DI_Loader_Stacker_MaterialCheck((int)LoaderParameter.StackerTable.Stacker_0))
                {
                    strTemp = string.Format("Loader Right Stacker 에 자재가 없으므로 Loader Pause 상태로 시작합니다.\r\n\r\n[자재 투입 후 Pause 해제 요망]");
                    var mb = new MessageBoxOk();
                    mb.ShowDialog("Information !", strTemp);
                }

                if (!loader.loaderParameter.DI_Loader_Stacker_MaterialCheck((int)LoaderParameter.StackerTable.Stacker_1))
                {
                    strTemp = string.Format("Loader Left Stacker 에 자재가 없으므로 Loader Pause 상태로 시작합니다.\r\n\r\n[자재 투입 후 Pause 해제 요망]");
                    var mb = new MessageBoxOk();
                    mb.ShowDialog("Information !", strTemp);
                }

                //  Module Loading 시 가공 데이터 Parsing
                Equipment.ProcessingData_Parsing_byLoader = false;

                //  Loader Stacker Pause 해제는 수동으로. (자동으로 풀어주니 너무 계속 한다)            
                if (loader.loaderParameter.DI_Loader_Stacker_MaterialCheck((int)LoaderParameter.StackerTable.Stacker_0))
                {
                    loader.m_bStacker0_Complete = false;              //  임시 주석 : 왼쪽 Port 만 사용

                    if (loader.m_nLoaderTransfer_ProcessStep == (int)LoaderTransferProcessStep.LoaderStep_None)
                    {
                        loader.m_nLoaderTransfer_ProcessStep = (int)LoaderTransferProcessStep.LoaderStep_ModulePickup_fromStacker;
                    }
                }
                else if (loader.loaderParameter.DI_Loader_Stacker_MaterialCheck((int)LoaderParameter.StackerTable.Stacker_1))
                {
                    loader.m_bStacker1_Complete = false;

                    if (loader.m_nLoaderTransfer_ProcessStep == (int)LoaderTransferProcessStep.LoaderStep_None)
                    {
                        loader.m_nLoaderTransfer_ProcessStep = (int)LoaderTransferProcessStep.LoaderStep_ModulePickup_fromStacker;
                    }
                }

                //  Loader Stacker 의 Pause 상태에 따라 사용 우선순위 Port 결정
                //  Pause 상태가 아닌 Port 에 우선순위 부여. (둘 다 Pause 상태이면, User 가 Pause 상태를 해제하는 Port 에 우선권 부여)
                //  우선권이 부여된 Port 의 자재가 소진될 때 까지 바뀌지 않음. (Pick Up Fail 시에만 바뀜)
                if (!Equipment.Loader_RPort_Pause && Equipment.Loader_LPort_Pause)
                {
                    loader.m_nStacker_Priority = (int)LoaderParameter.StackerTable.Stacker_0;
                }
                else if (Equipment.Loader_RPort_Pause && !Equipment.Loader_LPort_Pause)
                {
                    loader.m_nStacker_Priority = (int)LoaderParameter.StackerTable.Stacker_1;
                }
                else        //  둘 다 Pause
                {
                    loader.m_nStacker_Priority = (int)LoaderParameter.StackerTable.None;
                }
            }
            
            Equipment.SemiAutoEnable = true;
            loader.SetSemiAutoRequest(Loader.SemiAutoStep.Start);
        }

        private void button_SemiAuto_Unloading_Click(object sender, EventArgs e)
        {
            Log.Write("GUI", Equipment.User_Name, "ButtonClick", "button_SemiAuto_Unloading_Click");
            string strTemp = "";

            bool bTest = false;
            if (bTest)
            {
                if (Equipment.AutoRunStatus)
                {
                    var mb = new MessageBoxOk();
                    mb.ShowDialog("Information !", "장비가 [[ 운전중 ]] 입니다.");
                    return;
                }
                if (!Equipment.AutoManualStatus)
                {
                    var mb = new MessageBoxOk();
                    mb.ShowDialog("Information !", "장비가 [[ AUTO ]] 상태가 아닙니다.");
                    return;
                }
            }

            Equipment.SemiAutoEnable = true;
            unloader.SetSemiAutoRequest(Unloader.SemiAutoStep.Start);
        }

        private void button_SemiAuto_HeightSensor_Click(object sender, EventArgs e)
        {
            Log.Write("GUI", Equipment.User_Name, "ButtonClick", "button_SemiAuto_HeightSensor_Click");
            string strTemp = "";
            if (!CheckStageinterlock())
            {
                strTemp = string.Format("CheckStageinterlock - Fail");
                Log.Write("GUI", Equipment.User_Name, "ButtonClick", strTemp);
                return;
            }

            if (!Equipment.stLayerRecipeSet[0].ProcessOption_SocketHeightCheck_Use)
            {
                strTemp = string.Format("ProcessOption_SocketHeightCheck_Use : false");
                Log.Write("GUI", Equipment.User_Name, "ButtonClick", strTemp);

                var mb = new MessageBoxOk();
                mb.ShowDialog("Information !", "높이측정 사용 모드가 아닙니다. Data Load 후 정지합니다.");
            }

            Equipment.LaserDrillingCycStop_Reservation = false;
            Equipment.ProcessingData_Parsing_byLoader = false;              //  Module Loading 시 가공 데이터 Parsing


            Equipment.SemiAutoEnable = true;
            workStage.SetSemiAutoRequest(WorkStage.SemiAutoStep.MeasureHeight);
        }

        private void button_SemiAuto_PreAlign_Click(object sender, EventArgs e)
        {
            Log.Write("GUI", Equipment.User_Name, "ButtonClick", "button_SemiAuto_PreAlign_Click");
            string strTemp = "";
            if (!CheckStageinterlock())
            {
                strTemp = string.Format("CheckStageinterlock - Fail");
                Log.Write("GUI", Equipment.User_Name, "ButtonClick", strTemp);
                return;
            }

            if (Equipment.stLayerRecipeSet[0].ProcessOption_SocketAlign_Use)
            {
                if (workStage.IsStageComplete(WorkStage.SemiAutoStep.MeasureHeight) == false)
                {
                    var mb = new MessageBoxOk();
                    mb.ShowDialog("Information !", "PreAlign 은 Height Sensor 측정 후에만 가능합니다.");
                    return;
                }
            }
            else
            {
                strTemp = string.Format("ProcessOption_SocketAlign_Use : false");
                Log.Write("GUI", Equipment.User_Name, "ButtonClick", strTemp);

                var mb = new MessageBoxOk();
                mb.ShowDialog("Information !", "Align 사용 모드가 아닙니다.");
                return;
            }

            Equipment.LaserDrillingCycStop_Reservation = false;
            Equipment.ProcessingData_Parsing_byLoader = false;              //  Module Loading 시 가공 데이터 Parsing

            Equipment.SemiAutoEnable = true;
            workStage.SetSemiAutoRequest(WorkStage.SemiAutoStep.PreAlign);
        }

        private void button_SemiAuto_FiducialAlign_Click(object sender, EventArgs e)
        {
            Log.Write("GUI", Equipment.User_Name, "ButtonClick", "button_SemiAuto_FiducialAlign_Click");
            string strTemp = "";
            if (!CheckStageinterlock())
            {
                strTemp = string.Format("CheckStageinterlock - Fail");
                Log.Write("GUI", Equipment.User_Name, "ButtonClick", strTemp);
                return;
            }

            if(Equipment.stLayerRecipeSet[0].ProcessOption_SocketAlign_Use)
            {
                if (workStage.IsStageComplete(WorkStage.SemiAutoStep.MeasureHeight) == false &&
                    workStage.IsStageComplete(WorkStage.SemiAutoStep.PreAlign) == false)
                {
                    var mb = new MessageBoxOk();
                    mb.ShowDialog("Information !", "Fiducial Align 은 PreAlign 후에만 가능합니다.");
                    return;
                }
            }
            else
            {
                strTemp = string.Format("ProcessOption_SocketAlign_Use : false");
                Log.Write("GUI", Equipment.User_Name, "ButtonClick", strTemp);

                var mb = new MessageBoxOk();
                mb.ShowDialog("Information !", "Align 사용 모드가 아닙니다.");
                return;
            }

            Equipment.LaserDrillingCycStop_Reservation = false;
            Equipment.ProcessingData_Parsing_byLoader = false;              //  Module Loading 시 가공 데이터 Parsing

            Equipment.SemiAutoEnable = true;
            workStage.SetSemiAutoRequest(WorkStage.SemiAutoStep.FiducialAlign);
        }

        private void button_SemiAuto_LaserDrilling_Click(object sender, EventArgs e)
        {
            Log.Write("GUI", Equipment.User_Name, "ButtonClick", "button_SemiAuto_LaserDrilling_Click");
            string strTemp = "";
            if(!CheckStageinterlock())
            {
                strTemp = string.Format("CheckStageinterlock - Fail");
                Log.Write("GUI", Equipment.User_Name, "ButtonClick", strTemp);
                return;
            }

            if (Equipment.stLayerRecipeSet[0].ProcessOption_SocketAlign_Use)
            {
                if (workStage.IsStageComplete(WorkStage.SemiAutoStep.MeasureHeight) == false &&
                workStage.IsStageComplete(WorkStage.SemiAutoStep.PreAlign) == false &&
                workStage.IsStageComplete(WorkStage.SemiAutoStep.FiducialAlign) == false)
                {
                    var mb = new MessageBoxOk();
                    mb.ShowDialog("Information !", "LaserDrilling은 FiducialAlign 후에만 가능합니다.");
                    return;
                }
            }
            else
            {
                strTemp = string.Format("ProcessOption_SocketAlign_Use : false");
                Log.Write("GUI", Equipment.User_Name, "ButtonClick", strTemp);

                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Align 사용 모드가 아닙니다. 진행하시겠습니까?"))
                    return;
            }

            Equipment.LaserDrillingCycStop_Reservation = false;
            Equipment.ProcessingData_Parsing_byLoader = false;              //  Module Loading 시 가공 데이터 Parsing

            Equipment.SemiAutoEnable = true;
            workStage.SetSemiAutoRequest(WorkStage.SemiAutoStep.Drilling);
        }

        private bool CheckStageinterlock()
        {
            bool bRtn = true;

            bool bTest = false;
            if(bTest)
            {
                if (Equipment.AutoRunStatus)
                {
                    var mb = new MessageBoxOk();
                    mb.ShowDialog("Information !", "장비가 [[ 운전중 ]] 입니다.");
                    bRtn = false;
                }
                if (!Equipment.AutoManualStatus)
                {
                    var mb = new MessageBoxOk();
                    mb.ShowDialog("Information !", "장비가 [[ AUTO ]] 상태가 아닙니다.");
                    bRtn = false;
                }

                //  Chiller 상태 체크 - Run 신호를 내보내는지
                if (!workStage.workStageParameter.IsDO_Chiller_Run())
                {
                    var mb = new MessageBoxOk();
                    mb.ShowDialog("Information !", "Chiller 가 [[ OFF ]] 상태입니다.\r\n\r\nChiller 를 [[ ON ]] 상태로 변경 후 다시 시도 바랍니다.");
                    bRtn = false;
                }
                //  Chiller 상태 체크 - Run 신호를 내보내고 있는데 Run, 신호가 들어오지 않는 경우
                if (workStage.workStageParameter.IsDO_Chiller_Run() && !workStage.workStageParameter.DI_Chiller_Run())
                {
                    var mb = new MessageBoxOk();
                    mb.ShowDialog("Information !", "Chiller 가 동작하지 않습니다.\r\n\r\nChiller 상태를 확인 후 다시 시도 바랍니다.");
                    bRtn = false;
                }
                //  Chiller 상태 체크 - Run 신호를 내보내고 있는데, 알람 신호가 들어오는 경우
                if (workStage.workStageParameter.IsDO_Chiller_Run() && !workStage.workStageParameter.DI_Chiller_Alarm_Check())
                {
                    var mb = new MessageBoxOk();
                    mb.ShowDialog("Information !", "Chiller 가 Alarm 상태입니다.\r\n\r\nChiller 상태를 확인 후 다시 시도 바랍니다.");
                    bRtn = false;
                }

                if (!Equipment.Machine_LaserType_CO2)
                {
                    if (workStage.m_nLaser_PulseMode != 1)
                    {
                        var mb = new MessageBoxOk();
                        mb.ShowDialog("Information !", "레이저 External 모드가 아닙니다.\r\n\r\n [[External]] 모드로 변경 후 다시 시도 바랍니다.");
                        bRtn = false;
                    }
                }
            }
            
            return bRtn;
        }

        private void UpdateButtonStatusByState(Button btn, bool isStarted, bool isComplete)
        {
            if (!isStarted)
            {
                SetColor(btn, SystemColors.Control, Color.Black);         // 대기: 기본 색
            }
            else if (isStarted && !isComplete)
            {
                SetColor(btn, Color.Gold, Color.Black);                   // 진행 중: 노랑
            }
            else if (isStarted && isComplete)
            {
                SetColor(btn, Color.LightGreen, Color.Black);            // 완료: 초록
            }
        }

        private void SetColor(Control control, Color Backcolor, Color foreColor)
        {
            if (control.InvokeRequired)
            {
                this.Invoke(new System.Action(() =>
                {
                    SetColor(control, Backcolor, foreColor);
                }));
            }
            else
            {
                control.BackColor = Backcolor;
                control.ForeColor = foreColor;
            }
        }
    }
}
