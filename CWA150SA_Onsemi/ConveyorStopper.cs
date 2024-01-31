using QMC.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CWA150SA_Onsemi300
{
    public partial class ConveyorStopper : UserControl
    {
        private Font m_font = new Font("Arial", 12f, FontStyle.Bold);

        private Font m_ServiceFont = new Font("Arial", 11f, FontStyle.Bold);
        public Module Module { get; set; }
        public Part Part { get; set; }
        FormBaseConfiguration Configuration { get; set; }
        //public ConveyorStopper(Module module)
        public ConveyorStopper()
        {
            //Module = module;
            InitializeComponent();

            //  2022. 03. 22.  SCH : Configuration 의 Size 를 사용하지 않도록 함.
            //Configuration = new FormBaseConfiguration();
            //this.Size = Configuration.ModuleStateControlSize;

            //상태 바뀌면 바로바로 바뀔수있도록.

            //if (module.ServiceState == Module.serviceState.UserSelected)
            //{
            //    textBoxModuleService.Text = "UserSelected";
            //    textBoxModuleService.BackColor = Color.Green;
            //    textBoxModuleService.Font = m_ServiceFont;
            //    textBoxModuleService.Multiline = false;
            //}
            //else
            //{
            //    textBoxModuleService.Text = "InService";
            //    textBoxModuleService.BackColor = Color.Yellow;
            //    textBoxModuleService.Font = m_ServiceFont;
            //    textBoxModuleService.Multiline = false;
            //}
            //if (Module.CurrentState != null)
            //{
            //    textBoxModuleBehevior.Text = Module.CurrentState.Name;
            //    if (Module.CurrentState.Name == "Idle")
            //    {
            //        textBoxModuleBehevior.Text = Module.CurrentState.Name;
            //        textBoxModuleBehevior.BackColor = Color.Green;

            //    }
            //    else if (Module.CurrentState.Name == "Alarm")
            //    {
            //        textBoxModuleBehevior.Text = Module.CurrentState.Name;
            //        textBoxModuleBehevior.BackColor = Color.Red;
            //    }
            //    else if (Module.CurrentState.Name == "Ready")
            //    {
            //        textBoxModuleBehevior.Text = Module.CurrentState.Name;
            //        textBoxModuleBehevior.BackColor = Color.Yellow;
            //    }
            //    else if (Module.CurrentState.Name == "Running")
            //    {
            //        textBoxModuleBehevior.Text = Module.CurrentState.Name;
            //        textBoxModuleBehevior.BackColor = Color.Blue;
            //    }
            //}
        }
        private void baseButtonStop_Click(object sender, EventArgs e)
        {
            //Module.StopExecute();
        }
        private void baseButtonInitialize_Click(object sender, EventArgs e)
        {
            //AsyncMethod method = new AsyncMethod();

            //method.SetRunProcedure(new IntResultDelegate(Module.Initialize), null);
            //method.SetStopProcedure(new IntResultDelegate(Module.StopExecute), null);

            //AsyncResult result = method.Run();
            //ProgressForm ProgressForm = new ProgressForm(Module.Name, "Initializing...", result);
            //ProgressForm.StartPosition = FormStartPosition.CenterScreen;
            //ProgressForm.ShowDialog();
        }

        private void StopperUp_Click(object sender, EventArgs e)
        {

        }

        private void StopperDown_Click(object sender, EventArgs e)
        {

        }

        private void CarrierLock_Click(object sender, EventArgs e)
        {

        }

        private void CarrierUnlock_Click(object sender, EventArgs e)
        {

        }
    }
}
