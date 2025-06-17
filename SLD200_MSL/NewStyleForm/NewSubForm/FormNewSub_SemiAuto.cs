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
        private enum LoaderSemiAutoStep
        {
            LoaderStacker0,
            LoaderStacker1,
            LoaderMAlign,
            LoaderTransfer
        }

        static WorkStage workStage;
        static Loader loader;
        static Unloader unloader;
        static Vision vision;
        static Bds bds;

        private System.Windows.Forms.Timer timerSemiAuto;

        private bool m_isSemiAutoMode = false;
        private bool m_isSemiAutoStepTrigger = false;
        private LoaderSemiAutoStep m_currentMode = LoaderSemiAutoStep.LoaderStacker0;

        private int m_prevStacker0Step = -1;
        private bool m_isAutoDetailStep = false;

        public FormNewSub_SemiAuto()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.DoubleBuffered = true;

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

            timerSemiAuto = new System.Windows.Forms.Timer();
            timerSemiAuto.Interval = 100; // 100ms 간격
            timerSemiAuto.Tick += TimerSemiAuto_Tick;
            timerSemiAuto.Start();

            cboSemiAutoStep.Items.AddRange(Enum.GetNames(typeof(LoaderSemiAutoStep)));
            cboSemiAutoStep.SelectedIndex = 0;
            // 초기화시 스택커 스텝 리스트 바인딩
            cboStackerStep.Items.Clear();
            cboStackerStep.Items.AddRange(Enum.GetNames(typeof(StackerModulePickupWaitingPos_Step)));
            cboStackerStep.SelectedIndex = 0;
        }

        private void TimerSemiAuto_Tick(object sender, EventArgs e)
        {
            Timer_SemiAutoRun();
        }

        private void btnSemiAutoStart_Click(object sender, EventArgs e)
        {
            m_currentMode = (LoaderSemiAutoStep)cboSemiAutoStep.SelectedIndex;
            m_isSemiAutoMode = true;
            m_isSemiAutoStepTrigger = false;
            m_prevStacker0Step = -1;
            lblCurrentStep.Text = $"Current Step: {m_currentMode}";
        }

        private void btnSemiAutoNext_Click(object sender, EventArgs e)
        {
            if (!m_isSemiAutoMode)
                return;

            if (chkDetailAuto.Checked && m_currentMode == LoaderSemiAutoStep.LoaderStacker0)
            {
                m_isAutoDetailStep = true;
                m_prevStacker0Step = -1;
            }
            else
            {
                m_isSemiAutoStepTrigger = true;
            }
        }

        private void btnSemiAutoStop_Click(object sender, EventArgs e)
        {
            m_isSemiAutoMode = false;
            m_isSemiAutoStepTrigger = false;
            m_isAutoDetailStep = false;
            lblCurrentStep.Text = "Current Step: None";
        }

        public void Timer_SemiAutoRun()
        {
            if (!m_isSemiAutoMode)
                return;

            if (m_isAutoDetailStep && m_currentMode == LoaderSemiAutoStep.LoaderStacker0)
            {
                int curStep = loader.m_nStacker0_ModulePickupWaitingPos_Step;

                if (m_prevStacker0Step != -1 && m_prevStacker0Step == curStep)
                {
                    // 동일 스텝이면 아직 진행 중
                    return;
                }

                m_prevStacker0Step = curStep;
                int result = loader.Run_Stacker0Module_PickupWaitingPos_Func();

                lblCurrentStep.Text = $"Auto Detail Step: {((StackerModulePickupWaitingPos_Step)curStep)} → Result: {(result == 0 ? "OK" : "NG")}";

                if ((StackerModulePickupWaitingPos_Step)curStep == StackerModulePickupWaitingPos_Step.Complete)
                {
                    m_isAutoDetailStep = false;
                    MessageBox.Show("Stacker0 세부 스텝 완료됨.");
                }
            }
            else if (m_isSemiAutoStepTrigger)
            {
                int result = -1;
                switch (m_currentMode)
                {
                    case LoaderSemiAutoStep.LoaderStacker0:
                        result = loader.Run_Stacker0Module_PickupWaitingPos_Func();
                        break;
                    case LoaderSemiAutoStep.LoaderStacker1:
                        result = loader.Run_Stacker1Module_PickupWaitingPos_Func();
                        break;
                    case LoaderSemiAutoStep.LoaderMAlign:
                        result = loader.Run_MAlign_Cycle_Func();
                        break;
                    case LoaderSemiAutoStep.LoaderTransfer:
                        result = loader.Run_Transfer_Cycle_Func();
                        break;
                }

                lblCurrentStep.Text = $"Current Step: {m_currentMode} → Result: {(result == 0 ? "OK" : "NG")}";
                m_isSemiAutoStepTrigger = false;
            }
        }

        private void btnRunStackerStep_Click(object sender, EventArgs e)
        {
            if ((LoaderSemiAutoStep)cboSemiAutoStep.SelectedIndex != LoaderSemiAutoStep.LoaderStacker0)
            {
                MessageBox.Show("현재 Stacker0 모드에서만 사용 가능합니다.");
                return;
            }

            var selectedStep = (StackerModulePickupWaitingPos_Step)Enum.Parse(
                typeof(StackerModulePickupWaitingPos_Step), cboStackerStep.SelectedItem.ToString());

            loader.m_nStacker0_ModulePickupWaitingPos_Step = (int)selectedStep;
            int result = loader.Run_Stacker0Module_PickupWaitingPos_Func();

            lblCurrentStep.Text = $"Run Step: {selectedStep} → Result: {(result == 0 ? "OK" : "NG")}";
        }
    }
}

