using QMC.Common;
using QMC.Common.Global;
using QMC.Common.Modules;
using QMC.Core;
using SLD200_MSL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QMC.Common.Modules.Loader;
using static QMC.Common.Modules.Unloader;
using static QMC.Common.Modules.WorkStage;

namespace SLD200.NewStyleForm.NewSubForm
{
    public partial class FormNewSub_SemiAuto : Form
    {
        static WorkStage workStage;
        static Loader loader;
        static Unloader unloader;
        static Vision vision;
        static Bds bds;

        private System.Windows.Forms.Timer timerSemiAuto;
        private bool _isRunning_SemiAuto = false;
        public FormNewSub_SemiAuto()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.DoubleBuffered = true;

            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;
            foreach (Module module in m_collectionModules)
            {
                if (module.Name == "WorkStage") workStage = module as WorkStage;
                if (module.Name == "Loader")    loader = module as Loader;
                if (module.Name == "Unloader")  unloader = module as Unloader;
                if (module.Name == "Vision")    vision = module as Vision;
                if (module.Name == "BDS")       bds = module as Bds;
            }

            timerSemiAuto = new System.Windows.Forms.Timer();
            timerSemiAuto.Interval = 200;
            timerSemiAuto.Tick += TimerSemiAuto_Tick;
            timerSemiAuto.Start();

            //Loader
            cboSemiAutoStep_Loader.Items.AddRange(Enum.GetNames(typeof(Loader.SemiAutoStep)));
            cboSemiAutoStep_Loader.SelectedIndex = 0;

            cboSemiAutoStepSub_Loader.Items.AddRange(Enum.GetNames(typeof(Loader.LoaderTransferMoveType)));
            cboSemiAutoStepSub_Loader.SelectedIndex = 0;
            cboSemiAutoStepSub_Loader.Enabled = false; // 초기에는 비활성화

            cboDetailStep_Loader.Items.AddRange(Enum.GetNames(typeof(Loader.StackerModulePickupWaitingPos_Step)));
            cboDetailStep_Loader.SelectedIndex = 0;

            //Stage
            cboSemiAutoStep_Stage.Items.AddRange(Enum.GetNames(typeof(WorkStage.SemiAutoStep)));
            cboSemiAutoStep_Stage.SelectedIndex = 0;

            cboDetailStep_Stage.Items.AddRange(Enum.GetNames(typeof(WorkStage.LaserDrilling_Step)));
            cboDetailStep_Stage.SelectedIndex = 0;

            // Unloader
            cboSemiAutoStep_Unloader.Items.AddRange(Enum.GetNames(typeof(Unloader.SemiAutoStep)));
            cboSemiAutoStep_Unloader.SelectedIndex = 0;

            cboSemiAutoStepSub_Unloader.Items.AddRange(Enum.GetNames(typeof(Unloader.UnloaderTransferMoveType)));
            cboSemiAutoStepSub_Unloader.SelectedIndex = 0;
            cboSemiAutoStepSub_Unloader.Enabled = false; // 초기에는 비활성화

            cboDetailStep_Unloader.Items.AddRange(Enum.GetNames(typeof(Unloader.StackerModulePutdownWaitingPos_Step)));
            cboDetailStep_Unloader.SelectedIndex = 0;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // 엔터 또는 스페이스 키 눌렀을 때 무시
            if (keyData == Keys.Enter || keyData == Keys.Space)
                return true;

            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void FormNewSub_SemiAuto_Load(object sender, EventArgs e)
        {
            int a = 0;
            a = 0;
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

        public void Timer_SemiAutoRun()
        {
            if (loader == null) return;

            Loader.SemiAutoStep selectedStepLoader = (Loader.SemiAutoStep)cboSemiAutoStep_Loader.SelectedIndex;
            switch(selectedStepLoader)
            {
                case Loader.SemiAutoStep.None:
                    break;
                case Loader.SemiAutoStep.Stacker0:
                    lblCurrentStep_Loader.Text = $"Current: {loader.m_nStacker0_ModulePickupWaitingPos_Step} " +
                      $"({Enum.GetName(typeof(StackerModulePickupWaitingPos_Step), loader.m_nStacker0_ModulePickupWaitingPos_Step)})";
                    break;
                case Loader.SemiAutoStep.Stacker1:
                    lblCurrentStep_Loader.Text = $"Current: {loader.m_nStacker1_ModulePickupWaitingPos_Step} " +
                      $"({Enum.GetName(typeof(StackerModulePickupWaitingPos_Step), loader.m_nStacker1_ModulePickupWaitingPos_Step)})";
                    break;
                case Loader.SemiAutoStep.MAlign:
                    lblCurrentStep_Loader.Text = $"Current: {loader.m_nMAlign_Step} " +
                      $"({Enum.GetName(typeof(MAlign_Step), loader.m_nMAlign_Step)})";
                    break;
                case Loader.SemiAutoStep.Transfer:
                    lblCurrentStep_Loader.Text = $"Current: {loader.m_nLoader_Transfer_Step} " +
                      $"({Enum.GetName(typeof(Loader_Transfer_Step), loader.m_nLoader_Transfer_Step)})";
                    break;
                default: 
                    break;
            }

            Unloader.SemiAutoStep selectedStepUnloader = (Unloader.SemiAutoStep)cboSemiAutoStep_Unloader.SelectedIndex;
            switch (selectedStepUnloader)
            {
                case Unloader.SemiAutoStep.None:
                    break;
                case Unloader.SemiAutoStep.Stacker0:
                    lblCurrentStep_Unloader.Text = $"Current: {unloader.m_nStacker0_ModulePutdownWaitingPos_Step} " +
                      $"({Enum.GetName(typeof(StackerModulePutdownWaitingPos_Step), unloader.m_nStacker0_ModulePutdownWaitingPos_Step)})";
                    break;
                case Unloader.SemiAutoStep.Stacker1:
                    lblCurrentStep_Unloader.Text = $"Current: {unloader.m_nStacker1_ModulePutdownWaitingPos_Step} " +
                      $"({Enum.GetName(typeof(StackerModulePutdownWaitingPos_Step), unloader.m_nStacker1_ModulePutdownWaitingPos_Step)})";
                    break;
                
                case Unloader.SemiAutoStep.Transfer:
                    lblCurrentStep_Unloader.Text = $"Current: {unloader.m_nUnloader_Transfer_Step} " +
                      $"({Enum.GetName(typeof(Unloader_Transfer_Step), unloader.m_nUnloader_Transfer_Step)})";
                    break;
                default:
                    break;
            }
        }

        private void btnSemiAutoStart_Loader_Click(object sender, EventArgs e)
        {
            if (loader == null) return;

            // 장비 구동 시 인터락 조건 확인.
            if(true)
            {
                if (!workStage.m_bHomeOK)
                {
                    var mb = new MessageBoxOk();
                    mb.ShowDialog("Information !", "장비 초기화 진행 바랍니다.\r\n");
                    return;
                }

                if (!workStage.workStageParameter.IsDO_Chiller_Run())
                {
                    var mb = new MessageBoxOk();
                    mb.ShowDialog("Information !", "Chiller 가 [[ OFF ]] 상태입니다.\r\n\r\nChiller 를 [[ ON ]] 상태로 변경 후 다시 시도 바랍니다.");
                    return;
                }
                //  Chiller 상태 체크 - Run 신호를 내보내고 있는데 Run, 신호가 들어오지 않는 경우
                if (workStage.workStageParameter.IsDO_Chiller_Run() &&
                    !workStage.workStageParameter.DI_Chiller_Run())
                {
                    var mb = new MessageBoxOk();
                    mb.ShowDialog("Information !", "Chiller 가 동작하지 않습니다.\r\n\r\nChiller 상태를 확인 후 다시 시도 바랍니다.");
                    return;
                }
                //  Chiller 상태 체크 - Run 신호를 내보내고 있는데, 알람 신호가 들어오는 경우
                if (workStage.workStageParameter.IsDO_Chiller_Run() &&
                    !workStage.workStageParameter.DI_Chiller_Alarm_Check())
                {
                    var mb = new MessageBoxOk();
                    mb.ShowDialog("Information !", "Chiller 가 Alarm 상태입니다.\r\n\r\nChiller 상태를 확인 후 다시 시도 바랍니다.");
                    return;
                }
                if (!Equipment.Machine_LaserType_CO2)
                {
                    if (workStage.m_nLaser_PulseMode != 1)
                    {
                        var mb = new MessageBoxOk();
                        mb.ShowDialog("Information !", "레이저 External 모드가 아닙니다.\r\n\r\n [[External]] 모드로 변경 후 다시 시도 바랍니다.");
                        return;
                    }
                }
                if (Equipment.AutoRunStatus || Equipment.SelectRunEnable || Equipment.SelectRunEnable_New)
                {
                    var mb = new MessageBoxOk();
                    mb.ShowDialog("Information !", "장비가 [[ 운전중 ]] 입니다.");
                    return;
                }
            }    

            Loader.SemiAutoStep selectedStep = (Loader.SemiAutoStep)cboSemiAutoStep_Loader.SelectedIndex;
            loader.SetSemiAutoRequest(selectedStep);

            loader._isSemiAutoDetailMode = chkDetailAuto_Loader.Checked;
            loader._semiAutoDetailStepRequest = !chkDetailAuto_Loader.Checked;

            //세미오토 조건 확인 필.!
            loader._isSemiAutoMode = true;
            // 시컨스 시작
            loader.m_LoaderWork_Start = true;

            lblCurrentStep_Loader.Text = $"[SemiAuto] Start: {selectedStep} (DetailMode: {loader._isSemiAutoDetailMode})";

            // 버튼 색상 업데이트
            btnSemiAutoStart_Loader.BackColor = Color.LimeGreen;
            btnSemiAutoStop_Loader.BackColor = SystemColors.Control;
        }

        private void btnSemiAutoNext_Loader_Click(object sender, EventArgs e)
        {
            if (loader == null || !loader._isSemiAutoMode) return; // || !loader._isSemiAutoDetailMode

            //SemiAutoStep && LoaderTransferMoveType
            //1. SemiAutoStep::Stacker0 or SemiAutoStep::Stacker1
            //2. SemiAutoStep::Transfer -> LoaderTransferMoveType::Cycle_Stacker0_PickUp or Cycle_Stacker1_PickUp
            //3. SemiAutoStep::Transfer -> LoaderTransferMoveType::Cycle_MAligner_PutDown
            //4. SemiAutoStep::MAlign
            //5. SemiAutoStep::Transfer -> LoaderTransferMoveType::Cycle_MAligner_PickUp
            //6. SemiAutoStep::Transfer -> LoaderTransferMoveType::Cycle_WorkStage_PutDown

            // 현재 상태 가져오기
            Loader.SemiAutoStep currentMainStep = (Loader.SemiAutoStep)cboSemiAutoStep_Loader.SelectedIndex;
            Loader.LoaderTransferMoveType currentTransferStep = (Loader.LoaderTransferMoveType)cboSemiAutoStepSub_Loader.SelectedIndex;

            // 다음 스텝 결정
            if (currentMainStep == Loader.SemiAutoStep.Stacker0 || currentMainStep == Loader.SemiAutoStep.Stacker1)
            {
                // 1 → 2: Transfer - PickUp from Stacker
                cboSemiAutoStep_Loader.SelectedItem = Loader.SemiAutoStep.Transfer.ToString();
                cboSemiAutoStepSub_Loader.SelectedItem =
                    currentMainStep == Loader.SemiAutoStep.Stacker0
                    ? Loader.LoaderTransferMoveType.Cycle_Stacker0_PickUp.ToString()
                    : Loader.LoaderTransferMoveType.Cycle_Stacker1_PickUp.ToString();
            }
            else if (currentMainStep == Loader.SemiAutoStep.Transfer)
            {
                if (currentTransferStep == Loader.LoaderTransferMoveType.Cycle_Stacker0_PickUp ||
                    currentTransferStep == Loader.LoaderTransferMoveType.Cycle_Stacker1_PickUp)
                {
                    // 2 → 3: Transfer - PutDown to MAligner
                    cboSemiAutoStepSub_Loader.SelectedItem = Loader.LoaderTransferMoveType.Cycle_MAligner_PutDown.ToString();
                }
                else if (currentTransferStep == Loader.LoaderTransferMoveType.Cycle_MAligner_PutDown)
                {
                    // 3 → 4: MAlign
                    cboSemiAutoStep_Loader.SelectedItem = Loader.SemiAutoStep.MAlign.ToString();
                }
                else if (currentTransferStep == Loader.LoaderTransferMoveType.Cycle_MAligner_PickUp)
                {
                    // 5 → 6: Transfer - PutDown to WorkStage
                    cboSemiAutoStepSub_Loader.SelectedItem = Loader.LoaderTransferMoveType.Cycle_WorkStage_PutDown.ToString();
                }
            }
            else if (currentMainStep == Loader.SemiAutoStep.MAlign)
            {
                // 4 → 5: Transfer - PickUp from MAligner
                cboSemiAutoStep_Loader.SelectedItem = Loader.SemiAutoStep.Transfer.ToString();
                cboSemiAutoStepSub_Loader.SelectedItem = Loader.LoaderTransferMoveType.Cycle_MAligner_PickUp.ToString();
            }

            // 현재 선택된 값으로 요청 세팅
            Loader.SemiAutoStep selectedStep = (Loader.SemiAutoStep)Enum.Parse(typeof(Loader.SemiAutoStep), cboSemiAutoStep_Loader.SelectedItem.ToString());
            loader.SetSemiAutoRequest(selectedStep);

            if (selectedStep == Loader.SemiAutoStep.Transfer)
            {
                Loader.LoaderTransferMoveType subStep = (Loader.LoaderTransferMoveType)Enum.Parse(typeof(Loader.LoaderTransferMoveType), cboSemiAutoStepSub_Loader.SelectedItem.ToString());
                loader.m_nLoaderTransferMoveType = (int)subStep;
            }

            loader.m_LoaderWork_Start = true;
            loader._semiAutoDetailStepRequest = true;

            lblCurrentStep_Loader.Text = $"[SemiAuto] Next: {selectedStep} ({cboSemiAutoStepSub_Loader.SelectedItem})";

            // Next 버튼 강조 (선택사항)
            btnSemiAutoNext_Loader.BackColor = Color.Gold;
            Task.Delay(2000).ContinueWith(_ =>
            {
                // 색상 원복
                if (!this.IsDisposed && btnSemiAutoNext_Loader.IsHandleCreated)
                {
                    this.Invoke(new Action(() =>
                    {
                        btnSemiAutoNext_Loader.BackColor = SystemColors.Control;
                    }));
                }
            });
        }

        private void btnSemiAutoStop_Loader_Click(object sender, EventArgs e)
        {
            if (loader == null) return;

            // 시컨스 정지
            loader.m_LoaderWork_Start = false;

            loader.ClearSemiAutoRequest();
            lblCurrentStep_Loader.Text = "[SemiAuto] Stop";

            // 버튼 색상 업데이트
            btnSemiAutoStart_Loader.BackColor = SystemColors.Control;
            btnSemiAutoStop_Loader.BackColor = Color.IndianRed;
        }

        private void ChkDetailAuto_Loader_CheckedChanged(object sender, EventArgs e)
        {
            return;

            if (loader == null) return;
            loader._isSemiAutoDetailMode = chkDetailAuto_Loader.Checked;
        }

        private void btnRunStackerStep_Loader_Click(object sender, EventArgs e)
        {
            return;

            if (loader == null) return;

            if (loader._isSemiAutoMode && loader._isSemiAutoDetailMode)
            {
                loader._semiAutoDetailStepRequest = true;
            }
        }

        private void cboSemiAutoStep_Loader_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (loader == null) return;

            Loader.SemiAutoStep selectedStep = (Loader.SemiAutoStep)cboSemiAutoStep_Loader.SelectedIndex;
            cboDetailStep_Loader.Items.Clear();

            cboSemiAutoStepSub_Loader.Items.Clear();
            cboSemiAutoStepSub_Loader.Items.AddRange(Enum.GetNames(typeof(Loader.LoaderTransferMoveType)));
            cboSemiAutoStepSub_Loader.SelectedIndex = 0;
            cboSemiAutoStepSub_Loader.Enabled = false;
            
            switch (selectedStep)
            {
                case Loader.SemiAutoStep.None:
                    break;
                case Loader.SemiAutoStep.Stacker0:
                    
                    cboDetailStep_Loader.Items.AddRange(Enum.GetNames(typeof(StackerModulePickupWaitingPos_Step)));
                    cboDetailStep_Loader.SelectedIndex = 0;
                    break;
                case Loader.SemiAutoStep.Stacker1:
                    cboDetailStep_Loader.Items.AddRange(Enum.GetNames(typeof(StackerModulePickupWaitingPos_Step)));
                    cboDetailStep_Loader.SelectedIndex = 0;
                    break;
                case Loader.SemiAutoStep.MAlign:
                    cboDetailStep_Loader.Items.AddRange(Enum.GetNames(typeof(MAlign_Step)));
                    cboDetailStep_Loader.SelectedIndex = 0;
                    break;
                case Loader.SemiAutoStep.Transfer:
                    cboDetailStep_Loader.Items.AddRange(Enum.GetNames(typeof(Loader_Transfer_Step)));
                    cboDetailStep_Loader.SelectedIndex = 0;
                    cboSemiAutoStepSub_Loader.Enabled = true; // Transfer 단계에서는 서브 스텝 활성화

                    break;
                default:
                    break;
            }
        }

        private void btnReset_Loader_Click(object sender, EventArgs e)
        {
            loader.ClearSemiAutoRequest();
            loader.m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;
            loader.m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;
            loader.m_nMAlign_Step = (int)MAlign_Step.None;
            loader.m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;
            loader.m_nLoaderTransferMoveType = (int)Loader.LoaderTransferMoveType.Cycle_None;
            loader.m_nLoaderTransfer_ProcessStep = (int)Loader.LoaderTransferProcessStep.LoaderStep_None;
        }

        private void btnSemiAutoStart_Unloader_Click(object sender, EventArgs e)
        {
            if (unloader == null) return;

            // 장비 구동 시 인터락 조건 확인.
            if (true)
            {
                if (!workStage.m_bHomeOK)
                {
                    var mb = new MessageBoxOk();
                    mb.ShowDialog("Information !", "장비 초기화 진행 바랍니다.\r\n");
                    return;
                }

                if (!workStage.workStageParameter.IsDO_Chiller_Run())
                {
                    var mb = new MessageBoxOk();
                    mb.ShowDialog("Information !", "Chiller 가 [[ OFF ]] 상태입니다.\r\n\r\nChiller 를 [[ ON ]] 상태로 변경 후 다시 시도 바랍니다.");
                    return;
                }
                //  Chiller 상태 체크 - Run 신호를 내보내고 있는데 Run, 신호가 들어오지 않는 경우
                if (workStage.workStageParameter.IsDO_Chiller_Run() &&
                    !workStage.workStageParameter.DI_Chiller_Run())
                {
                    var mb = new MessageBoxOk();
                    mb.ShowDialog("Information !", "Chiller 가 동작하지 않습니다.\r\n\r\nChiller 상태를 확인 후 다시 시도 바랍니다.");
                    return;
                }
                //  Chiller 상태 체크 - Run 신호를 내보내고 있는데, 알람 신호가 들어오는 경우
                if (workStage.workStageParameter.IsDO_Chiller_Run() &&
                    !workStage.workStageParameter.DI_Chiller_Alarm_Check())
                {
                    var mb = new MessageBoxOk();
                    mb.ShowDialog("Information !", "Chiller 가 Alarm 상태입니다.\r\n\r\nChiller 상태를 확인 후 다시 시도 바랍니다.");
                    return;
                }
                if (!Equipment.Machine_LaserType_CO2)
                {
                    if (workStage.m_nLaser_PulseMode != 1)
                    {
                        var mb = new MessageBoxOk();
                        mb.ShowDialog("Information !", "레이저 External 모드가 아닙니다.\r\n\r\n [[External]] 모드로 변경 후 다시 시도 바랍니다.");
                        return;
                    }
                }
                if (Equipment.AutoRunStatus || Equipment.SelectRunEnable || Equipment.SelectRunEnable_New)
                {
                    var mb = new MessageBoxOk();
                    mb.ShowDialog("Information !", "장비가 [[ 운전중 ]] 입니다.");
                    return;
                }
            }

            Unloader.SemiAutoStep selectedStep = (Unloader.SemiAutoStep)cboSemiAutoStep_Unloader.SelectedIndex;
            unloader.SetSemiAutoRequest(selectedStep);

            unloader._isSemiAutoDetailMode = chkDetailAuto_Unloader.Checked;
            unloader._semiAutoDetailStepRequest = !chkDetailAuto_Unloader.Checked;

            //세미오토 조건 확인 필.!
            unloader._isSemiAutoMode = true;
            // 시컨스 시작
            unloader.m_UnloaderWork_Start = true;

            lblCurrentStep_Unloader.Text = $"[SemiAuto] Start: {selectedStep} (DetailMode: {unloader._isSemiAutoDetailMode})";

            // 버튼 색상 업데이트
            btnSemiAutoStart_Unloader.BackColor = Color.LimeGreen;
            btnSemiAutoStop_Unloader.BackColor = SystemColors.Control;
        }

        private void btnSemiAutoNext_Unloader_Click(object sender, EventArgs e)
        {
            if (unloader == null || !unloader._isSemiAutoMode) return; // || !unloader._isSemiAutoDetailMode

            //SemiAutoStep && UnloaderTransferMoveType
            //1. SemiAutoStep::Transfer -> LoaderTransferMoveType::Cycle_WorkStage_PickUp
            //2. SemiAutoStep::Transfer -> UnloaderTransferMoveType::Cycle_Stacker0_PutDown or Cycle_Stacker1_PutDown or Cycle_NG_PutDown

            // 현재 상태 가져오기
            Unloader.SemiAutoStep currentMainStep = (Unloader.SemiAutoStep)cboSemiAutoStep_Unloader.SelectedIndex;
            Unloader.UnloaderTransferMoveType currentTransferStep = (Unloader.UnloaderTransferMoveType)cboSemiAutoStepSub_Unloader.SelectedIndex;

            // 다음 스텝 결정
            if (currentMainStep == Unloader.SemiAutoStep.Transfer)
            {
                if (cboSemiAutoStepSub_Unloader.SelectedItem == null)
                {
                    // Step 1 → 2 : 초기 상태에서 WorkStage PickUp
                    cboSemiAutoStepSub_Unloader.SelectedItem = Unloader.UnloaderTransferMoveType.Cycle_WorkStage_PickUp.ToString();
                }
                else if (currentTransferStep == Unloader.UnloaderTransferMoveType.Cycle_WorkStage_PickUp)
                {
                    // Step 2 → 3 : PutDown
                    // 이 부분은 외부에서 조건 결정하도록 하거나, 임시로 Cycle_Stacker0_PutDown으로 설정
                    cboSemiAutoStepSub_Unloader.SelectedItem = Unloader.UnloaderTransferMoveType.Cycle_Stacker0_PutDown.ToString();
                }
            }

            // 현재 선택된 값으로 요청 세팅
            Unloader.SemiAutoStep selectedStep = (Unloader.SemiAutoStep)Enum.Parse(typeof(Unloader.SemiAutoStep), cboSemiAutoStep_Unloader.SelectedItem.ToString());
            unloader.SetSemiAutoRequest(selectedStep);

            if (selectedStep == Unloader.SemiAutoStep.Transfer)
            {
                Unloader.UnloaderTransferMoveType subStep = (Unloader.UnloaderTransferMoveType)Enum.Parse(
                    typeof(Unloader.UnloaderTransferMoveType), cboSemiAutoStepSub_Unloader.SelectedItem.ToString());
                unloader.m_nUnloaderTransferMoveType = (int)subStep;
            }

            unloader.m_UnloaderWork_Start = true;
            unloader._semiAutoDetailStepRequest = true;

            lblCurrentStep_Unloader.Text = $"[SemiAuto] Next: {selectedStep} ({cboSemiAutoStepSub_Unloader.SelectedItem})";
            // Next 버튼 강조 (선택사항)
            btnSemiAutoNext_Unloader.BackColor = Color.Gold;
            Task.Delay(2000).ContinueWith(_ =>
            {
                // 색상 원복
                if (!this.IsDisposed && btnSemiAutoNext_Unloader.IsHandleCreated)
                {
                    this.Invoke(new Action(() =>
                    {
                        btnSemiAutoNext_Unloader.BackColor = SystemColors.Control;
                    }));
                }
            });
            //// 다음 스텝 결정
            //if (currentMainStep == Unloader.SemiAutoStep.Stacker0 || currentMainStep == Unloader.SemiAutoStep.Stacker1)
            //{
            //    // 1 → 2: Transfer - PickUp from Stacker
            //    cboSemiAutoStep_Loader.SelectedItem = Unloader.SemiAutoStep.Transfer.ToString();
            //    cboSemiAutoStepSub_Loader.SelectedItem =
            //        currentMainStep == Unloader.SemiAutoStep.Stacker0
            //        ? Unloader.UnloaderTransferMoveType.Cycle_Stacker0_PickUp.ToString()
            //        : Unloader.UnloaderTransferMoveType.Cycle_Stacker1_PickUp.ToString();
            //}
            //else if (currentMainStep == Unloader.SemiAutoStep.Transfer)
            //{
            //    if (currentTransferStep == Unloader.UnloaderTransferMoveType.Cycle_Stacker0_PickUp ||
            //        currentTransferStep == Unloader.UnloaderTransferMoveType.Cycle_Stacker1_PickUp)
            //    {
            //        // 2 → 3: Transfer - PutDown to MAligner
            //        cboSemiAutoStepSub_Loader.SelectedItem = unloader.LoaderTransferMoveType.Cycle_MAligner_PutDown.ToString();
            //    }
            //    else if (currentTransferStep == unloader.LoaderTransferMoveType.Cycle_MAligner_PutDown)
            //    {
            //        // 3 → 4: MAlign
            //        cboSemiAutoStep_Loader.SelectedItem = unloader.SemiAutoStep.MAlign.ToString();
            //    }
            //    else if (currentTransferStep == unloader.LoaderTransferMoveType.Cycle_MAligner_PickUp)
            //    {
            //        // 5 → 6: Transfer - PutDown to WorkStage
            //        cboSemiAutoStepSub_Loader.SelectedItem = unloader.LoaderTransferMoveType.Cycle_WorkStage_PutDown.ToString();
            //    }
            //}
            //else if (currentMainStep == unloader.SemiAutoStep.MAlign)
            //{
            //    // 4 → 5: Transfer - PickUp from MAligner
            //    cboSemiAutoStep_Loader.SelectedItem = unloader.SemiAutoStep.Transfer.ToString();
            //    cboSemiAutoStepSub_Loader.SelectedItem = unloader.LoaderTransferMoveType.Cycle_MAligner_PickUp.ToString();
            //}

            //// 현재 선택된 값으로 요청 세팅
            //unloader.SemiAutoStep selectedStep = (unloader.SemiAutoStep)Enum.Parse(typeof(unloader.SemiAutoStep), cboSemiAutoStep_Loader.SelectedItem.ToString());
            //unloader.SetSemiAutoRequest(selectedStep);

            //if (selectedStep == unloader.SemiAutoStep.Transfer)
            //{
            //    unloader.LoaderTransferMoveType subStep = (unloader.LoaderTransferMoveType)Enum.Parse(typeof(unloader.LoaderTransferMoveType), cboSemiAutoStepSub_Loader.SelectedItem.ToString());
            //    unloader.m_nLoaderTransferMoveType = (int)subStep;
            //}

            //unloader.m_LoaderWork_Start = true;
            //unloader._semiAutoDetailStepRequest = true;

            //lblCurrentStep_Unloader.Text = $"[SemiAuto] Next: {selectedStep} ({cboSemiAutoStepSub_Loader.SelectedItem})";

        }

        private void btnSemiAutoStop_Unloader_Click(object sender, EventArgs e)
        {
            if (unloader == null) return;

            // 시컨스 정지
            unloader.m_UnloaderWork_Start = false;

            unloader.ClearSemiAutoRequest();
            lblCurrentStep_Unloader.Text = "[SemiAuto] Stop";

            // 버튼 색상 업데이트
            btnSemiAutoStart_Unloader.BackColor = SystemColors.Control;
            btnSemiAutoStop_Unloader.BackColor = Color.IndianRed;
        }

        private void btnReset_Unloader_Click(object sender, EventArgs e)
        {
            unloader.ClearSemiAutoRequest();
            unloader.m_nStacker0_ModulePutdownWaitingPos_Step = (int)Unloader.StackerModulePutdownWaitingPos_Step.None;
            unloader.m_nStacker1_ModulePutdownWaitingPos_Step = (int)Unloader.StackerModulePutdownWaitingPos_Step.None;
            unloader.m_nUnloader_Transfer_Step = (int)Unloader.Unloader_Transfer_Step.None;
            unloader.m_nUnloaderTransferMoveType = (int)Unloader.UnloaderTransferMoveType.Cycle_None;
        }

        private void btnRunStackerStep_Unloader_Click(object sender, EventArgs e)
        {

        }

        private void cboSemiAutoStep_Unloader_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (unloader == null) return;

            Unloader.SemiAutoStep selectedStep = (Unloader.SemiAutoStep)cboSemiAutoStep_Unloader.SelectedIndex;
            cboDetailStep_Unloader.Items.Clear();

            cboSemiAutoStepSub_Unloader.Items.Clear();
            cboSemiAutoStepSub_Unloader.Items.AddRange(Enum.GetNames(typeof(Unloader.UnloaderTransferMoveType)));
            cboSemiAutoStepSub_Unloader.SelectedIndex = 0;
            cboSemiAutoStepSub_Unloader.Enabled = false;

            switch (selectedStep)
            {
                case Unloader.SemiAutoStep.None:
                    break;
                case Unloader.SemiAutoStep.Stacker0:

                    cboDetailStep_Unloader.Items.AddRange(Enum.GetNames(typeof(Unloader.StackerModulePutdownWaitingPos_Step)));
                    cboDetailStep_Unloader.SelectedIndex = 0;
                    break;
                case Unloader.SemiAutoStep.Stacker1:
                    cboDetailStep_Unloader.Items.AddRange(Enum.GetNames(typeof(Unloader.StackerModulePutdownWaitingPos_Step)));
                    cboDetailStep_Unloader.SelectedIndex = 0;
                    break;
                case Unloader.SemiAutoStep.Transfer:
                    cboDetailStep_Unloader.Items.AddRange(Enum.GetNames(typeof(Unloader.Unloader_Transfer_Step)));
                    cboDetailStep_Unloader.SelectedIndex = 0;
                    cboSemiAutoStepSub_Unloader.Enabled = true; // Transfer 단계에서는 서브 스텝 활성화

                    break;
                default:
                    break;
            }
        }

        private void cboSemiAutoStepSub_Unloader_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboDetailStep_Unloader_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void chkDetailAuto_Unloader_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void cboSemiAutoStepSub_Loader_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboDetailStep_Loader_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnSemiAutoStart_Stage_Click(object sender, EventArgs e)
        {
            if (workStage == null) return;

            // 장비 구동 시 인터락 조건 확인.
            if (true)
            {
                if (!workStage.m_bHomeOK)
                {
                    var mb = new MessageBoxOk();
                    mb.ShowDialog("Information !", "장비 초기화 진행 바랍니다.\r\n");
                    return;
                }

                if (!workStage.workStageParameter.IsDO_Chiller_Run())
                {
                    var mb = new MessageBoxOk();
                    mb.ShowDialog("Information !", "Chiller 가 [[ OFF ]] 상태입니다.\r\n\r\nChiller 를 [[ ON ]] 상태로 변경 후 다시 시도 바랍니다.");
                    return;
                }
                //  Chiller 상태 체크 - Run 신호를 내보내고 있는데 Run, 신호가 들어오지 않는 경우
                if (workStage.workStageParameter.IsDO_Chiller_Run() &&
                    !workStage.workStageParameter.DI_Chiller_Run())
                {
                    var mb = new MessageBoxOk();
                    mb.ShowDialog("Information !", "Chiller 가 동작하지 않습니다.\r\n\r\nChiller 상태를 확인 후 다시 시도 바랍니다.");
                    return;
                }
                //  Chiller 상태 체크 - Run 신호를 내보내고 있는데, 알람 신호가 들어오는 경우
                if (workStage.workStageParameter.IsDO_Chiller_Run() &&
                    !workStage.workStageParameter.DI_Chiller_Alarm_Check())
                {
                    var mb = new MessageBoxOk();
                    mb.ShowDialog("Information !", "Chiller 가 Alarm 상태입니다.\r\n\r\nChiller 상태를 확인 후 다시 시도 바랍니다.");
                    return;
                }
                if (!Equipment.Machine_LaserType_CO2)
                {
                    if (workStage.m_nLaser_PulseMode != 1)
                    {
                        var mb = new MessageBoxOk();
                        mb.ShowDialog("Information !", "레이저 External 모드가 아닙니다.\r\n\r\n [[External]] 모드로 변경 후 다시 시도 바랍니다.");
                        return;
                    }
                }
                if (Equipment.AutoRunStatus || Equipment.SelectRunEnable || Equipment.SelectRunEnable_New)
                {
                    var mb = new MessageBoxOk();
                    mb.ShowDialog("Information !", "장비가 [[ 운전중 ]] 입니다.");
                    return;
                }
            }

            WorkStage.SemiAutoStep selectedStep = (WorkStage.SemiAutoStep)cboSemiAutoStep_Stage.SelectedIndex;
            workStage.SetSemiAutoRequest(selectedStep);

            workStage._isSemiAutoDetailMode = chkDetailAuto_Stage.Checked;
            workStage._semiAutoDetailStepRequest = !chkDetailAuto_Stage.Checked;

            //세미오토 조건 확인 필.!
            workStage._isSemiAutoMode = true;
            // 시컨스 시작
            workStage.m_LaserDrillingWork_Start = true;

            lblCurrentStep_Stage.Text = $"[SemiAuto] Start: {selectedStep} (DetailMode: {workStage._isSemiAutoDetailMode})";

            // 버튼 색상 업데이트
            btnSemiAutoStart_Stage.BackColor = Color.LimeGreen;
            btnSemiAutoStop_Stage.BackColor = SystemColors.Control;
        }

        private void btnSemiAutoNext_Stage_Click(object sender, EventArgs e)
        {
            if (workStage == null || !workStage._isSemiAutoMode) return;

            //SemiAutoStep
            //1. SemiAutoStep::Start
            //2. SemiAutoStep::MeasureHeight
            //3. SemiAutoStep::PreAlign
            //4. SemiAutoStep::FiducialAlign
            //5. SemiAutoStep::Drilling

            // 현재 상태 가져오기
            WorkStage.SemiAutoStep currentMainStep = (WorkStage.SemiAutoStep)cboSemiAutoStep_Stage.SelectedIndex;
            
            // 다음 스텝 결정
            if (currentMainStep == WorkStage.SemiAutoStep.Start)
            {
                cboSemiAutoStep_Stage.SelectedItem = WorkStage.SemiAutoStep.MeasureHeight.ToString();
            }
            else if (currentMainStep == WorkStage.SemiAutoStep.MeasureHeight)
            {
                cboSemiAutoStep_Stage.SelectedItem = WorkStage.SemiAutoStep.PreAlign.ToString();
            }
            else if (currentMainStep == WorkStage.SemiAutoStep.PreAlign)
            {
                cboSemiAutoStep_Stage.SelectedItem = WorkStage.SemiAutoStep.FiducialAlign.ToString();
            }
            else if (currentMainStep == WorkStage.SemiAutoStep.FiducialAlign)
            {
                cboSemiAutoStep_Stage.SelectedItem = WorkStage.SemiAutoStep.Drilling.ToString();
            }
            else if (currentMainStep == WorkStage.SemiAutoStep.Drilling)
            {
                // LaserDrilling 완료 후 End로 전환 (또는 다시 Start로)
                cboSemiAutoStep_Stage.SelectedItem = WorkStage.SemiAutoStep.None.ToString();
            }

            // 현재 선택된 값으로 요청 세팅
            WorkStage.SemiAutoStep selectedStep = (WorkStage.SemiAutoStep)Enum.Parse(typeof(WorkStage.SemiAutoStep), cboSemiAutoStep_Stage.SelectedItem.ToString());
            workStage.SetSemiAutoRequest(selectedStep);

            workStage.m_LaserDrillingWork_Start = true;
            workStage._semiAutoDetailStepRequest = true;

            lblCurrentStep_Stage.Text = $"[SemiAuto] Next: {selectedStep} ({cboSemiAutoStep_Stage.SelectedItem})";

            // Next 버튼 강조 (선택사항)
            btnSemiAutoNext_Stage.BackColor = Color.Gold;
            Task.Delay(2000).ContinueWith(_ =>
            {
                // 색상 원복
                if (!this.IsDisposed && btnSemiAutoNext_Stage.IsHandleCreated)
                {
                    this.Invoke(new Action(() =>
                    {
                        btnSemiAutoNext_Stage.BackColor = SystemColors.Control;
                    }));
                }
            });
        }

        private void btnSemiAutoStop_Stage_Click(object sender, EventArgs e)
        {
            if (workStage == null) return;

            // 시컨스 정지
            workStage.StopProcess();

            workStage.ClearSemiAutoRequest();
            lblCurrentStep_Stage.Text = "[SemiAuto] Stop";

            // 버튼 색상 업데이트
            btnSemiAutoStart_Stage.BackColor = SystemColors.Control;
            btnSemiAutoStop_Stage.BackColor = Color.IndianRed;
        }

        private void btnReset_Stage_Click(object sender, EventArgs e)
        {
            workStage.ClearSemiAutoRequest();
            workStage.m_nLaserDrilling_MainStep = (int)LaserDrilling_Step.None;
        }

        private void cboSemiAutoStep_Stage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (workStage == null) return;

            WorkStage.SemiAutoStep selectedStep = (WorkStage.SemiAutoStep)cboSemiAutoStep_Stage.SelectedIndex;
            cboDetailStep_Stage.Items.Clear();

            switch (selectedStep)
            {
                case WorkStage.SemiAutoStep.None:
                    break;
                case WorkStage.SemiAutoStep.Start:
                case WorkStage.SemiAutoStep.MeasureHeight:
                case WorkStage.SemiAutoStep.PreAlign:
                case WorkStage.SemiAutoStep.FiducialAlign:
                case WorkStage.SemiAutoStep.Drilling:
                    cboDetailStep_Stage.Items.AddRange(Enum.GetNames(typeof(LaserDrilling_Step)));
                    cboDetailStep_Stage.SelectedIndex = 0;
                    break;
                default:
                    break;
            }
        }

        private void cboSemiAutoStepSub_Stage_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void chkDetailAuto_Stage_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnRunStackerStep_Stage_Click(object sender, EventArgs e)
        {

        }

        private void cboDetailStep_Stage_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
