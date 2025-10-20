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
using static QMC.Common.Equipment;
using static QMC.Common.Modules.Loader;
using static QMC.Common.Modules.Unloader;
using static QMC.Common.Modules.WorkStage;

namespace SLD200.NewStyleForm.NewSubForm
{
    public partial class FormNewSub_Main_SemiAuto : UserControl
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
            timerSemiAuto.Interval = 500;
            timerSemiAuto.Tick += TimerSemiAuto_Tick;
            timerSemiAuto.Start();

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
            if(Equipment.AutoRunStatus || Equipment.SelectRunEnable_New || Equipment.SelectRunEnable)
            {
                button_SemiAuto_Unloading.Enabled = false;
                button_SemiAuto_HeightSensor.Enabled = false;
                button_SemiAuto_PreAlign.Enabled = false;
                button_SemiAuto_FiducialAlign.Enabled = false;
                button_SemiAuto_LaserDrilling.Enabled = false;
                button_SemiAuto_Loading.Enabled = false;
                button_SemiAuto_Load_Reset.Enabled = false;
                button_SemiAuto_Stage_Reset.Enabled = false;
                button_SemiAuto_Unload_Reset.Enabled = false;
            }
            else
            {
                button_SemiAuto_Unloading.Enabled = true;
                button_SemiAuto_HeightSensor.Enabled = true;
                button_SemiAuto_PreAlign.Enabled = true;
                button_SemiAuto_FiducialAlign.Enabled = true;
                button_SemiAuto_LaserDrilling.Enabled = true;
                button_SemiAuto_Loading.Enabled = true;
                button_SemiAuto_Load_Reset.Enabled = true;
                button_SemiAuto_Stage_Reset.Enabled = true;
                button_SemiAuto_Unload_Reset.Enabled = true;
            }

            UpdateButtonStatusByState(button_SemiAuto_Loading, loader.m_LoaderWork_Start, loader.IsLoaderComplete());

            //Clear
            //UpdateButtonStatusByState(button_SemiAuto_HeightSensor, false, false);
            //UpdateButtonStatusByState(button_SemiAuto_PreAlign, false, false);
            //UpdateButtonStatusByState(button_SemiAuto_FiducialAlign, false, false);
            //UpdateButtonStatusByState(button_SemiAuto_LaserDrilling, false, false);

            switch (workStage._semiAutoRequest)
            {
                case WorkStage.SemiAutoStep.MeasureHeight:
                    UpdateButtonStatusByState(
                        button_SemiAuto_HeightSensor,
                        (workStage._semiAutoRequest == WorkStage.SemiAutoStep.MeasureHeight),
                        workStage.IsStageComplete(WorkStage.SemiAutoStep.MeasureHeight));
                    break;
                case WorkStage.SemiAutoStep.PreAlign:
                    UpdateButtonStatusByState(
                        button_SemiAuto_PreAlign,
                        (workStage._semiAutoRequest == WorkStage.SemiAutoStep.PreAlign),
                        workStage.IsStageComplete(WorkStage.SemiAutoStep.PreAlign));
                    break;
                case WorkStage.SemiAutoStep.FiducialAlign:
                    UpdateButtonStatusByState(
                        button_SemiAuto_FiducialAlign,
                        (workStage._semiAutoRequest == WorkStage.SemiAutoStep.FiducialAlign),
                        workStage.IsStageComplete(WorkStage.SemiAutoStep.FiducialAlign));
                    break;
                case WorkStage.SemiAutoStep.GoldPowderAlign:
                    UpdateButtonStatusByState(
                        button_SemiAuto_GoldPowderAlign,
                        (workStage._semiAutoRequest == WorkStage.SemiAutoStep.GoldPowderAlign),
                        workStage.IsStageComplete(WorkStage.SemiAutoStep.GoldPowderAlign));
                    break;
                case WorkStage.SemiAutoStep.Drilling:
                    UpdateButtonStatusByState(
                        button_SemiAuto_LaserDrilling,
                        (workStage._semiAutoRequest == WorkStage.SemiAutoStep.Drilling),
                        workStage.IsStageComplete(WorkStage.SemiAutoStep.Drilling));
                    break;
            }
            
            UpdateButtonStatusByState(button_SemiAuto_Unloading, unloader.m_UnloaderWork_Start, unloader.IsUnloaderComplete());
        }

        private void button_SemiAuto_Loading_Click(object sender, EventArgs e)
        {
            Log.Write("GUI", Equipment.User_Name, "ButtonClick", "button_SemiAuto_Loading_Click");
            string strTemp = "";

            var mb1 = new MessageBoxYesNo();
            if (DialogResult.Yes != mb1.ShowDialog("Question ?", "시작하시겠습니까?"))
                return;

            bool bTest = true;
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

            var mb1 = new MessageBoxYesNo();
            if (DialogResult.Yes != mb1.ShowDialog("Question ?", "시작하시겠습니까?"))
                return;

            bool bTest = true;
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

            if(!loader.m_bAUTORUN_Loader_Transfer_ModulePutDowntoWorkStage_Complete ||
                !workStage.m_bMainWorkCycle_Complete)
            {
                var mb2 = new MessageBoxYesNo();
                if (DialogResult.Yes != mb2.ShowDialog("Question ?", "강제 배출 하시겠습니까?"))
                {
                    return;
                }
                else
                {
                    loader.m_bAUTORUN_Loader_Transfer_ModulePutDowntoWorkStage_Complete = true;
                    workStage.m_bMainWorkCycle_Complete = true;
                    workStage.m_nMainWorkCycle_ResultOKNG = (int)WorkStage.MainCycle_Result.NG;
                }
            }

            Equipment.SemiAutoEnable = true;
            unloader.SetSemiAutoRequest(Unloader.SemiAutoStep.Start);
        }

        private void button_SemiAuto_HeightSensor_Click(object sender, EventArgs e)
        {
            Log.Write("GUI", Equipment.User_Name, "ButtonClick", "button_SemiAuto_HeightSensor_Click");
            string strTemp = "";

            var mb1 = new MessageBoxYesNo();
            if (DialogResult.Yes != mb1.ShowDialog("Question ?", "시작하시겠습니까?"))
                return;

            if (Equipment.AutoRunStatus || Equipment.SelectRunEnable_New)
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

            if (!CheckStageinterlock())
            {
                strTemp = string.Format("CheckStageinterlock - Fail");
                Log.Write("GUI", Equipment.User_Name, "ButtonClick", strTemp);
                return;
            }

            //if (!Equipment.stLayerRecipeSet[0].ProcessOption_SocketHeightCheck_Use)
            //{
            //    strTemp = string.Format("ProcessOption_SocketHeightCheck_Use : false");
            //    Log.Write("GUI", Equipment.User_Name, "ButtonClick", strTemp);

            //    var mb = new MessageBoxOk();
            //    mb.ShowDialog("Information !", "높이측정 사용 모드가 아닙니다. Data Load 후 정지합니다.");
            //}

            Equipment.LaserDrillingCycStop_Reservation = false;
            Equipment.ProcessingData_Parsing_byLoader = false;              //  Module Loading 시 가공 데이터 Parsing

            Equipment.SemiAutoEnable = true;
            workStage.SetSemiAutoRequest(WorkStage.SemiAutoStep.MeasureHeight);
        }

        private void button_SemiAuto_PreAlign_Click(object sender, EventArgs e)
        {
            Log.Write("GUI", Equipment.User_Name, "ButtonClick", "button_SemiAuto_PreAlign_Click");
            string strTemp = "";

            var mb1 = new MessageBoxYesNo();
            if (DialogResult.Yes != mb1.ShowDialog("Question ?", "시작하시겠습니까?"))
                return;

            if (Equipment.AutoRunStatus || Equipment.SelectRunEnable_New)
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

            if (!CheckStageinterlock())
            {
                strTemp = string.Format("CheckStageinterlock - Fail");
                Log.Write("GUI", Equipment.User_Name, "ButtonClick", strTemp);
                return;
            }

            Equipment.LaserDrillingCycStop_Reservation = false;
            Equipment.ProcessingData_Parsing_byLoader = false;              //  Module Loading 시 가공 데이터 Parsing
            workStage.m_bPreAlignCompleted = false;

            Equipment.SemiAutoEnable = true;
            workStage.SetSemiAutoRequest(WorkStage.SemiAutoStep.PreAlign);
        }

        private void button_SemiAuto_FiducialAlign_Click(object sender, EventArgs e)
        {
            Log.Write("GUI", Equipment.User_Name, "ButtonClick", "button_SemiAuto_FiducialAlign_Click");
            string strTemp = "";

            var mb1 = new MessageBoxYesNo();
            if (DialogResult.Yes != mb1.ShowDialog("Question ?", "시작하시겠습니까?"))
                return;

            if (Equipment.AutoRunStatus || Equipment.SelectRunEnable_New)
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

            if (!CheckStageinterlock())
            {
                strTemp = string.Format("CheckStageinterlock - Fail");
                Log.Write("GUI", Equipment.User_Name, "ButtonClick", strTemp);
                return;
            }

            Equipment.LaserDrillingCycStop_Reservation = false;
            Equipment.ProcessingData_Parsing_byLoader = false;              //  Module Loading 시 가공 데이터 Parsing

            Equipment.SemiAutoEnable = true;
            workStage.SetSemiAutoRequest(WorkStage.SemiAutoStep.FiducialAlign);
        }
        private void button_SemiAuto_GoldPowderAlign_Click(object sender, EventArgs e)
        {
            Log.Write("GUI", Equipment.User_Name, "ButtonClick", "button_SemiAuto_GoldPowderAlign_Click");
            string strTemp = "";

            var mb1 = new MessageBoxYesNo();
            if (DialogResult.Yes != mb1.ShowDialog("Question ?", "시작하시겠습니까?"))
                return;

            if (Equipment.AutoRunStatus || Equipment.SelectRunEnable_New)
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

            if (!CheckStageinterlock())
            {
                strTemp = string.Format("CheckStageinterlock - Fail");
                Log.Write("GUI", Equipment.User_Name, "ButtonClick", strTemp);
                return;
            }

            Equipment.LaserDrillingCycStop_Reservation = false;
            Equipment.ProcessingData_Parsing_byLoader = false;              //  Module Loading 시 가공 데이터 Parsing

            Equipment.SemiAutoEnable = true;
            workStage.SetSemiAutoRequest(WorkStage.SemiAutoStep.GoldPowderAlign);
        }

        private void button_SemiAuto_LaserDrilling_Click(object sender, EventArgs e)
        {
            return;

            Log.Write("GUI", Equipment.User_Name, "ButtonClick", "button_SemiAuto_LaserDrilling_Click");
            string strTemp = "";

            var mb1 = new MessageBoxYesNo();
            if (DialogResult.Yes != mb1.ShowDialog("Question ?", "시작하시겠습니까?"))
                return;

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

            if (!CheckStageinterlock())
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

        private void button_SemiAuto_Load_Reset_Click(object sender, EventArgs e)
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "모든 데이터를 리셋 하시겠습니까?\r\n\r\n[Loader 부터 다시 시작]"))
                return;

            if (Equipment.AutoRunStatus || Equipment.SelectRunEnable_New || Equipment.SemiAutoEnable)
                return;

            //  가공 Sequence Index 초기화 (Loading 부터 시작)
            Equipment.m_bMainProcessStatus_LD_LPort_Complete = false;                       //  Loader LPort 투입 완료
            Equipment.m_bMainProcessStatus_LD_RPort_Complete = false;                       //  Loader RPort 투입 완료
            Equipment.m_bMainProcessStatus_LD_Module_PortPickUp_Complete = false;           //  Loader Port 에서 Module Pick Up 완료
            Equipment.m_bMainProcessStatus_LD_Module_MAlignerPutDown_Complete = false;      //  Loader M-Aligner 에 Module Put Down 완료
            Equipment.m_bMainProcessStatus_LD_M_Aligner_Align_Complete = false;             //  Loader M-Align 완료
            Equipment.m_bMainProcessStatus_LD_Module_MAlignerPickUp_Complete = false;       //  Loader M-Aligner 에서 Module Pick Up 완료
            Equipment.m_bMainProcessStatus_LD_Module_WorkStagePutDown_Complete = false;     //  Loader Work Stage 에 Module Put Down 완료
            Equipment.m_bMainProcessStatus_WorkStage_Module_Process_Complete = false;       //  Work Stage Process 완료
            Equipment.m_bMainProcessStatus_UL_Module_WorkStagePickUp_Complete = false;      //  Unloader Work Stage 에서 Module Pick Up 완료
            Equipment.m_bMainProcessStatus_UL_Module_PortPutDown_Complete = false;          //  Unloader Port 에 Module Put Down 완료
            Equipment.ProcessingData_Parsing_byLoader = false;

            //  Loader 파츠 사용 변수 초기화
            loader.m_nLoaderTransferMoveType = (int)LoaderTransferMoveType.Cycle_None; //  Transfer Move Type
            loader.m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;
            loader.m_nLoaderTransfer_ProcessStep = (int)LoaderTransferProcessStep.LoaderStep_None;
            loader.m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;
            loader.m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;
            loader.m_nMAlign_Step = (int)MAlign_Step.None;
            loader.m_nStacker_Priority = (int)LoaderParameter.StackerTable.None;                            //  Stacker 우선권 초기화

            loader.m_bStacker0_Complete = false;
            loader.m_bStacker1_Complete = false;
            loader.m_bStacker0_PickUp_Failed = false;                                                       //  Stacker1 Pick Up 실패 여부 Flag 초기화
            loader.m_bStacker1_PickUp_Failed = false;                                                       //  Stacker1 Pick Up 실패 여부 Flag 초기화

            loader.m_bMAlignZone_ModuleExist = false;

            loader.m_nLD_RESTORE_Transfer_Step = 0;
            loader.m_nLD_RESTORE_Transfer_MoveType = 0;
            loader.m_bLD_RESTORE_Transfer_toWorkStage_Module_PutDown_Complete_Flag = false;                 //  Work Stage 에 Module Put Down 완료 여부
            loader.m_bLD_RESTORE_Transfer_fromStacker0_Module_PickUp_Complete_Flag = false;                 //  Stacker0 에서 Module Pick Up 완료 여부
            loader.m_bLD_RESTORE_Transfer_fromStacker1_Module_PickUp_Complete_Flag = false;                 //  Stacker1 에서 Module Pick Up 완료 여부
            loader.m_bLD_RESTORE_Transfer_fromMAligner_Module_PickUp_Complete_Flag = false;                 //  M-Aligner 에서 Module Pick Up 완료 여부
            loader.m_bLD_RESTORE_Transfer_toMAligner_Module_PutDown_Complete_Flag = false;                  //  M-Aligner 에 Module Put Down 완료 여부
            loader.m_nLD_RESTORE_MainWork_Cycle_Step = 0;
            loader.m_nLD_RESTORE_DryRun_Cycle_Step = 0;
            loader.m_nLD_RESTORE_LaserDrilling_Cycle_Step = 0;
            loader.m_bLD_RESTORE_MainWork_Cycle_Complete = false;
            loader.m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePickUpfromStacker0_Complete = false;         //  Stacker0 에서 Module Pick Up 완료 여부
            loader.m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePickUpfromStacker1_Complete = false;         //  Stacker1 에서 Module Pick Up 완료 여부
            loader.m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePickUpfromMAligner_Complete = false;         //  M-Aligner 에서 Module Pick Up 완료 여부
            loader.m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePutDowntoMAligner_Complete = false;          //  M-Aligner 에 Module Put Down 완료 여부
            loader.m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePutDowntoWorkStage_Complete = false;         //  Work Stage 에 Module Put Down 완료 여부

            loader.m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker0_Complete = false;                    //  Stacker 에서 Module Pick Up 완료 여부
            loader.m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker1_Complete = false;                    //  Stacker 에서 Module Pick Up 완료 여부
            loader.m_bAUTORUN_Loader_Transfer_ModulePickUpfromMAligner_Complete = false;                    //  M-Aligner 에서 Module Pick Up 완료 여부
            loader.m_bAUTORUN_Loader_Transfer_ModulePutDowntoMAligner_Complete = false;                     //  M-Aligner 에 Module Put Down 완료 여부
            loader.m_bAUTORUN_Loader_Transfer_ModulePutDowntoWorkStage_Complete = false;                    //  Work Stage 에 Module Put Down 완료 여부
            loader.m_bLD_Transfer_fromStacker0_Module_PickUp_Complete_Flag = false;                         //  Stacker0 에서 Module Pick Up 완료 여부
            loader.m_bLD_Transfer_fromStacker1_Module_PickUp_Complete_Flag = false;                         //  Stacker1 에서 Module Pick Up 완료 여부
            loader.m_bLD_Transfer_fromMAligner_Module_PickUp_Complete_Flag = false;                         //  M-Aligner 에서 Module Pick Up 완료 여부
            loader.m_bLD_Transfer_toWorkStage_Module_PutDown_Complete_Flag = false;                         //  Work Stage 에 Module Put Down 완료 여부
            loader.m_bLD_Transfer_toMAligner_Module_PutDown_Complete_Flag = false;                          //  M-Aligner 에 Module Put Down 완료 여부

            loader.m_bStacker0_Run_byUser = false;                                                          //  Stacker0 Module Pick Up Cycle
            loader.m_bStacker1_Run_byUser = false;                                                          //  Stacker1 Module Pick Up Cycle

            loader.m_bLD_LPort_Complete = false;                                                            //  L-Port 동작 완료 여부
            loader.m_bLD_RPort_Complete = false;                                                            //  R-Port 동작 완료 여부
            loader.m_bLD_TR_ModulePickUp_LPort_Complete = false;                                            //  Transfer L-Port Module Pick Up 동작 완료 여부
            loader.m_bLD_TR_ModulePickUp_RPort_Complete = false;                                            //  Transfer R-Port Module Pick Up 동작 완료 여부
            loader.m_bLD_TR_ModulePutDown_MAligner_Complete = false;                                        //  Transfer Module Put Down 동작 완료 여부
            loader.m_bLD_MAligner_Exist = false;                                                            //  M-Aligner 로 Module Pick & Place
            loader.m_bLD_MAlign_Complete = false;                                                           //  M-Aligner 동작 완료 여부
            loader.m_bLD_TR_ModulePickUp_MAligner_Complete = false;                                         //  M-Aligner Module Pick Up 동작 완료 여부
            loader.m_bLD_WorkStage_LoadingComplete = false;

            loader.ResetRecovery();
            Equipment.SemiAutoEnable = false;

            if (loader.loaderParameter.DI_Loader_Aligner_VacuumCheck((int)LoaderParameter.MAlignerVacuumPos.Center))
            {
                loader.loaderParameter.DO_Loader_Aligner_Vacuum((int)LoaderParameter.MAlignerVacuumPos.Center, false);

                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Reset", "Loader_Aligner - 자재 확인 바랍니다.");
            }

            if (loader.loaderParameter.DI_Loader_Aligner_VacuumCheck((int)LoaderParameter.MAlignerVacuumPos.Inner))
                loader.loaderParameter.DO_Loader_Aligner_Vacuum((int)LoaderParameter.MAlignerVacuumPos.Inner, false);

            if (loader.loaderParameter.DI_Loader_Aligner_VacuumCheck((int)LoaderParameter.MAlignerVacuumPos.Outer))
                loader.loaderParameter.DO_Loader_Aligner_Vacuum((int)LoaderParameter.MAlignerVacuumPos.Outer, false);

            if (loader.loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Inner) ||
                loader.loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Outer))
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Reset", "Loader Picker - 자재 확인 및 버큠 Off 바랍니다.");
            }
        }

        private void button_SemiAuto_Unload_Reset_Click(object sender, EventArgs e)
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "모든 데이터를 리셋 하시겠습니까?\r\n\r\n[Loader 부터 다시 시작]"))
                return;

            if (Equipment.AutoRunStatus || Equipment.SelectRunEnable_New || Equipment.SemiAutoEnable)
                return;

            //  Unloader 파츠 사용 변수 초기화
            unloader.m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.None;
            unloader.m_nUnloaderTransferMoveType = (int)UnloaderTransferMoveType.Cycle_None;
            unloader.m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.None;
            unloader.m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.None;

            unloader.m_bStacker0_Complete = false;
            unloader.m_bStacker1_Complete = false;

            unloader.m_nUL_RESTORE_Transfer_Step = 0;
            unloader.m_nUL_RESTORE_Transfer_MoveType = 0;
            unloader.m_bUL_RESTORE_Transfer_fromWorkStage_Module_PickUp_Complete_Flag = false;
            unloader.m_bUL_RESTORE_LD_Transfer_toWorkStage_Module_PutDown_Complete = false;
            unloader.m_nUL_RESTORE_MainWork_Cycle_Step = 0;
            unloader.m_nUL_RESTORE_DryRun_Cycle_Step = 0;
            unloader.m_nUL_RESTORE_LaserDrilling_Cycle_Step = 0;
            unloader.m_bUL_RESTORE_MainWork_Cycle_Complete = false;
            unloader.m_nUL_RESTORE_MainWork_Cycle_ResultOKNG = (int)WorkStage.MainCycle_Result.None;
            unloader.m_bUL_RESTORE_MainWorkCycle_ResultOK_toRPort = false;                                  //  OK 인 Module 을 R-Port 로 가져갈 것인지 L-Port 로 가져갈 것인지

            unloader.m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete = false;    //  Work Stage 에서 Module Pick Up 완료 여부
            unloader.m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePutDowntoStacker0_Complete = false;      //  Stacker0 에 Module Put Down 완료 여부
            unloader.m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePutDowntoStacker1_Complete = false;      //  Stacker1 에 Module Put Down 완료 여부
            unloader.m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePutDowntoNG_Complete = false;            //  NG-Port 에 Module Put Down 완료 여부     

            unloader.m_bAUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete = false;               //  Work Stage 에서 Module Pick Up 완료 여부
            unloader.m_bAUTORUN_Unloader_Transfer_ModulePutDowntoStacker0_Complete = false;                 //  Stacker0 에 Module Put Down 완료 여부
            unloader.m_bAUTORUN_Unloader_Transfer_ModulePutDowntoStacker1_Complete = false;                 //  Stacker1 에 Module Put Down 완료 여부
            unloader.m_bAUTORUN_Unloader_Transfer_ModulePutDowntoNG_Complete = false;                       //  NG-Port 에 Module Put Down 완료 여부
            unloader.m_bAUTORUN_Unloader_Transfer_Module_Unloading_Complete = false;                        //  Unloader Transfer Module Unloading 완료 여부
            unloader.m_bUL_Transfer_fromWorkStage_Module_PickUp_Complete_Flag = false;                      //  Work Stage 에서 Module Pick Up 완료 여부

            unloader.ResetRecovery();
            Equipment.SemiAutoEnable = false;

            if (unloader.unloaderParameter.DI_Unloader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Inner) ||
                unloader.unloaderParameter.DI_Unloader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Outer))
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Reset", "Unloader Picker - 자재 확인 및 버큠 Off 바랍니다.");
            }
        }



        private void button_SemiAuto_Stage_Reset_Click(object sender, EventArgs e)
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "모든 데이터를 리셋 하시겠습니까?\r\n\r\n[Loader 부터 다시 시작]"))
                return;

            if (Equipment.AutoRunStatus || Equipment.SelectRunEnable_New || Equipment.SemiAutoEnable)
                return;

            workStage.m_nSelectedSocket_Index = -1;
            workStage.m_nSocketAlign_StartIndex = -1;
            Equipment.SelectedSocketStartMode = (int)SelectedSocketStartModeList.All;

            //  Main 파츠 사용 변수 초기화
            workStage.m_bMainWorkCycle_Complete = false;
            //workStage.m_bMainWorkCycle_ResultOK = false;
            workStage.m_nMainWorkCycle_ResultOKNG = (int)WorkStage.MainCycle_Result.None;
            workStage.m_bMainWorkCycle_ResultOK_toRPort = true;
            workStage.m_nMainWork_Step = (int)MainWork_Step.None;                                 //  Main Work Step
            workStage.m_nMainWorkCycleType = (int)MainWorkCycleType.Cycle_None;                   //  자동 운전 시 사용하는 변수
            workStage.m_bMainWorkCycle_DryRun = false;

            //  Laser Drilling 파츠 사용 변수 초기화
            workStage.m_bLaserDrilling_Complete = false;
            workStage.m_nLaserDrilling_MainStep = (int)LaserDrilling_Step.None;

            Equipment.m_AlignMode = AlignMode.Socket;

            workStage.m_bPassedSocket_Exist = false; //  Pass Socket 존재 여부

            // workstage LaserOff2 case문에서 초기화 하는 변수 전부 같이 Reset
            //workStage.GlobalSocketStatus_Init();            //위에서 하고 있는거 같지만.
            workStage.GetDrillingData_ProcessingFlagCheck();

            //  가공이 완료되었으므로, Align 변수 false 로
            workStage.m_bAlignCompleted = false;
            Equipment.SelectRunEnable = false;
            workStage.m_bForceEjectRequest = false;

            Equipment.SemiAutoEnable = false;
            Equipment.SelectRunEnable_New = false;


            // 장비 정지 시 그냥 정지 시킨다.
            workStage.m_ScannerCameraOffsetSequence.Reset();
            workStage.scannerCompensator.SetRunStatus(Part.RunStatus.Stop);
            workStage.m_ScannerCameraOffsetSequence.m_MainTick_Start = false;
            workStage.m_bSensorRequestPending = false;   // 요청 보냄
            workStage.m_bSensorResponseReady = false;    // 응답 받음

            workStage.m_bFirstAutoCrossCheckDone = false;
            workStage.m_bFirstLaserPowerCheckDone = false;
            workStage.m_bFirstHeightCheckDone = false;

            workStage.ResetRecovery();

            if (Equipment.RecipeOpen_DrawingFilePath != null && Equipment.RecipeOpen_DrawingFilePath != "")
            {
                workStage.Import_DrawingFile(Equipment.RecipeOpen_DrawingFilePath);
            }

            if (workStage.workStageParameter.DI_Stage_Vacuum_Check())
            {
                workStage.workStageParameter.DO_Stage_Vacuum(false);

                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Reset", "workStage - 자재 확인 바랍니다.");
            }
        }

        
    }
}
