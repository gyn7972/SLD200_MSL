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

namespace QMC.Common.UI
{
    public delegate void ModuleButtonClickHandler(string strButtonName);
    public partial class ModuleButton : UserControl
    {
        private Module m_Module;
        public event ModuleButtonClickHandler ModuleButtonClick;
        public ModuleButton(Module module)
        {
            m_Module = module;
            InitializeComponent();
            this.buttonModule.FlatStyle = FlatStyle.Standard;
            this.buttonModule.Text = null;
            this.buttonModule.TextAlign = ContentAlignment.TopLeft;
            this.buttonModule.Name = this.m_Module.Name;
            this.buttonModule.Text = this.m_Module.Name;
            //if (this.m_Module.CurrentState.Name == "Running")
            //{
            //    this.labelModuleStatus.BackColor = Color.Blue;
            //}
            //else if (this.m_Module.CurrentState.Name == "Alarm")
            //{
            //    this.labelModuleStatus.BackColor = Color.Red;
            //}
            //else if (this.m_Module.CurrentState.Name == "Ready")
            //{
            //    this.labelModuleStatus.BackColor = Color.Yellow;
            //}
            //else if (this.m_Module.CurrentState.Name == "Idle")
            //{
            //    this.labelModuleStatus.BackColor = Color.Green;
            //}

            //if (buttonModule.Name == "TargetStage")
            //{
            //    this.buttonModule.BackgroundImage = Properties.Resources.TargetStage_Exist;
            //}
            //else if (buttonModule.Name == "SourceStage")
            //{
            //    this.buttonModule.BackgroundImage = Properties.Resources.SourceStage_Exist;
            //}
            //else if(buttonModule.Name == "DieTransfer")
            //{
            //    this.buttonModule.BackgroundImage = Properties.Resources.DieTransfer_NonWorking;
            //}
            //else
            //{

            //}
            this.buttonModule.BackgroundImageLayout = ImageLayout.Zoom;
        }

        private void buttonModule_Click(object sender, EventArgs e)
        {
            if (ModuleButtonClick != null)
            {
                ModuleButtonClick(m_Module.Name);
            }
        }
    }
}
