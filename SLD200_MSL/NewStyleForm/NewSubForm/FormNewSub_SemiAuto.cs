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

            cboSemiAutoStep.Items.AddRange(Enum.GetNames(typeof(Loader.SemiAutoStep)));
            cboSemiAutoStep.SelectedIndex = 0;

            cboStackerStep.Items.AddRange(Enum.GetNames(typeof(StackerModulePickupWaitingPos_Step)));
            cboStackerStep.SelectedIndex = 0;
        }

        private void TimerSemiAuto_Tick(object sender, EventArgs e)
        {
            Timer_SemiAutoRun();
        }

        private void btnSemiAutoStart_Click(object sender, EventArgs e)
        {
            if (loader == null) return;

            Loader.SemiAutoStep selectedStep = (Loader.SemiAutoStep)cboSemiAutoStep.SelectedIndex;
            loader.SetSemiAutoRequest((Loader.SemiAutoStep)selectedStep);
            lblCurrentStep.Text = $"[SemiAuto] Start: {selectedStep}";
        }

        private void btnSemiAutoNext_Click(object sender, EventArgs e)
        {
            
        }

        private void btnSemiAutoStop_Click(object sender, EventArgs e)
        {
            if (loader == null) return;

            loader.ClearSemiAutoRequest();
            lblCurrentStep.Text = "[SemiAuto] Stop";
        }

        private void ChkDetailAuto_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        public void Timer_SemiAutoRun()
        {
            if (loader == null) return;

            lblCurrentStep.Text = $"Current: {loader.m_nStacker0_ModulePickupWaitingPos_Step}, SemiAutoMode: {loader._isSemiAutoMode}";
        }

        private void btnRunStackerStep_Click(object sender, EventArgs e)
        {
            if ((Loader.SemiAutoStep)cboSemiAutoStep.SelectedIndex != Loader.SemiAutoStep.Stacker0)
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
