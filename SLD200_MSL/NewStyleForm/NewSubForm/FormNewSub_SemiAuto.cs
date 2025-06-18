using QMC.Common;
using QMC.Common.Modules;
using SLD200_MSL;
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
    public partial class FormNewSub_SemiAuto : Form
    {
        static WorkStage workStage;
        static Loader loader;
        static Unloader unloader;
        static Vision vision;
        static Bds bds;

        private System.Windows.Forms.Timer timerSemiAuto;
        
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
            timerSemiAuto.Interval = 100;
            timerSemiAuto.Tick += TimerSemiAuto_Tick;
            timerSemiAuto.Start();

            cboSemiAutoStep_Loader.Items.AddRange(Enum.GetNames(typeof(Loader.SemiAutoStep)));
            cboSemiAutoStep_Loader.SelectedIndex = 0;

            cboDetailStep_Loader.Items.AddRange(Enum.GetNames(typeof(StackerModulePickupWaitingPos_Step)));
            cboDetailStep_Loader.SelectedIndex = 0;
        }

        private void FormNewSub_SemiAuto_Load(object sender, EventArgs e)
        {

        }

        private void TimerSemiAuto_Tick(object sender, EventArgs e)
        {
            Timer_SemiAutoRun();
        }

        public void Timer_SemiAutoRun()
        {
            if (loader == null) return;

            Loader.SemiAutoStep selectedStep = (Loader.SemiAutoStep)cboSemiAutoStep_Loader.SelectedIndex;
            switch(selectedStep)
            {
                case Loader.SemiAutoStep.None:
                    break;
                case Loader.SemiAutoStep.Stacker0:
                    lblCurrentStep_Loader.Text = $"Current: {loader.m_nStacker0_ModulePickupWaitingPos_Step} " +
                      $"({Enum.GetName(typeof(StackerModulePickupWaitingPos_Step), loader.m_nStacker0_ModulePickupWaitingPos_Step)}), " +
                      $"Step: {loader._semiAutoRequest}, " +
                      $"DetailMode: {loader._isSemiAutoDetailMode}, " +
                      $"StepReq: {loader._semiAutoDetailStepRequest}";
                    break;
                case Loader.SemiAutoStep.Stacker1:
                    lblCurrentStep_Loader.Text = $"Current: {loader.m_nStacker1_ModulePickupWaitingPos_Step} " +
                      $"({Enum.GetName(typeof(StackerModulePickupWaitingPos_Step), loader.m_nStacker1_ModulePickupWaitingPos_Step)}), " +
                      $"Step: {loader._semiAutoRequest}, " +
                      $"DetailMode: {loader._isSemiAutoDetailMode}, " +
                      $"StepReq: {loader._semiAutoDetailStepRequest}";
                    break;
                case Loader.SemiAutoStep.MAlign:
                    lblCurrentStep_Loader.Text = $"Current: {loader.m_nMAlign_Step} " +
                      $"({Enum.GetName(typeof(MAlign_Step), loader.m_nMAlign_Step)}), " +
                      $"Step: {loader._semiAutoRequest}, " +
                      $"DetailMode: {loader._isSemiAutoDetailMode}, " +
                      $"StepReq: {loader._semiAutoDetailStepRequest}";
                    break;
                case Loader.SemiAutoStep.Transfer:
                    lblCurrentStep_Loader.Text = $"Current: {loader.m_nLoader_Transfer_Step} " +
                      $"({Enum.GetName(typeof(Loader_Transfer_Step), loader.m_nLoader_Transfer_Step)}), " +
                      $"Step: {loader._semiAutoRequest}, " +
                      $"DetailMode: {loader._isSemiAutoDetailMode}, " +
                      $"StepReq: {loader._semiAutoDetailStepRequest}";
                    break;
                default: 
                    break;
            }
        }

        private void btnSemiAutoStart_Loader_Click(object sender, EventArgs e)
        {
            if (loader == null) return;

            //세미오토 조건 확인 필.!
            loader._isSemiAutoMode = true;

            Loader.SemiAutoStep selectedStep = (Loader.SemiAutoStep)cboSemiAutoStep_Loader.SelectedIndex;
            loader.SetSemiAutoRequest(selectedStep);

            loader._isSemiAutoDetailMode = chkDetailAuto_Loader.Checked;
            loader._semiAutoDetailStepRequest = !chkDetailAuto_Loader.Checked; // 디테일모드가 아닐 때는 바로 실행

            lblCurrentStep_Loader.Text = $"[SemiAuto] Start: {selectedStep} (DetailMode: {loader._isSemiAutoDetailMode})";
        }

        private void btnSemiAutoNext_Loader_Click(object sender, EventArgs e)
        {
            if (loader == null || !loader._isSemiAutoMode || !loader._isSemiAutoDetailMode) return;

            loader._semiAutoDetailStepRequest = true;
        }

        private void btnSemiAutoStop_Loader_Click(object sender, EventArgs e)
        {
            if (loader == null) return;

            loader.ClearSemiAutoRequest();
            lblCurrentStep_Loader.Text = "[SemiAuto] Stop";
        }

        private void ChkDetailAuto_Loader_CheckedChanged(object sender, EventArgs e)
        {
            if (loader == null) return;

            loader._isSemiAutoDetailMode = chkDetailAuto_Loader.Checked;
        }

        private void btnRunStackerStep_Loader_Click(object sender, EventArgs e)
        {
            if ((Loader.SemiAutoStep)cboSemiAutoStep_Loader.SelectedIndex != Loader.SemiAutoStep.Stacker0)
            {
                MessageBox.Show("현재 Stacker0 모드에서만 사용 가능합니다.");
                return;
            }

            if (loader._isSemiAutoMode && loader._isSemiAutoDetailMode)
            {
                loader._semiAutoDetailStepRequest = true;
            }
        }

        private void cboSemiAutoStep_Loader_SelectedIndexChanged(object sender, EventArgs e)
        {
            Loader.SemiAutoStep selectedStep = (Loader.SemiAutoStep)cboSemiAutoStep_Loader.SelectedIndex;
            cboDetailStep_Loader.Items.Clear();
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
                    break;
                default:
                    break;
            }
        }
    }
}
