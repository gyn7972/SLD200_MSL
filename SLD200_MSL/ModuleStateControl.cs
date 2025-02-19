using QMC.Common;
using QMC.Common.Modules;
using QMC.Common.Parts;
using QMC.Common.Vision.Cameras;
using QMC.Common.Vision.Optics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SLD200_MSL
{
    public partial class ModuleStateControl : UserControl
    {
        private Font m_font = new Font("Tahoma", 11f, FontStyle.Bold);

        private Font m_ServiceFont = new Font("Tahoma", 11f, FontStyle.Bold);
        public Module Module { get; set; }
        public Part Part { get; set; }
        static WorkStage workStage;
        FormBaseConfiguration Configuration { get; set; }
        public ModuleStateControl(Module module)
        {
            Module = module;
            InitializeComponent();
            Configuration = new FormBaseConfiguration();
            this.Size = Configuration.ModuleStateControlSize;
            //상태 바뀌면 바로바로 바뀔수있도록.

            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;

            foreach (Module module1 in m_collectionModules)
            {
                //if (module1.Name == "WorkStage")
                if (module1.Name == "WorkStage")
                {
                    workStage = module1 as WorkStage;
                }
            }

            textBoxModuleService.ReadOnly = true;
            textBoxModuleBehevior.ReadOnly = true;

            textBoxModuleBehevior.TextAlign = HorizontalAlignment.Center;
            textBoxModuleService.TextAlign = HorizontalAlignment.Center;

            textBoxModuleBehevior.Font = m_font;
            textBoxModuleBehevior.Multiline = false;

            textBoxModuleService.Font = m_font;
            textBoxModuleService.Multiline = false;

            this.flowLayoutPanelButton = new FlowLayoutPanel();
            //this.flowLayoutPanelButton.Size = new Size(Configuration.MotorStatusControlFlowPanelSize.Width, Configuration.MotorStatusControlFlowPanelSize.Height);
            this.flowLayoutPanelButton.Size = new Size(baseGroupBoxPartInit.Size.Width /*- Configuration.ControlGap_2*/ - 15, baseGroupBoxPartInit.Size.Height - Configuration.ControlGap_3);
            this.flowLayoutPanelButton.FlowDirection = FlowDirection.TopDown;
            //this.flowLayoutPanelButton.BackColor = Color.Red;
            this.flowLayoutPanelButton.Location = new Point(groupBoxStateControl.Location.X + 6/*Configuration.ControlGap + 1*/, groupBoxStateControl.Location.Y + Configuration.ControlGap_2);
            this.baseGroupBoxPartInit.Controls.Add(flowLayoutPanelButton);
            CreateButton();
        }
        private void baseButtonStop_Click(object sender, EventArgs e)
        {            
            Module.Stop();
        }
        private void baseButtonInitialize_Click(object sender, EventArgs e)
        {
            Task<int> task = Task.Factory.StartNew<int>(() =>
            {
                Module.SetRunStatus(Part.RunStatus.Run);
                Module.Initialize();
                return 0;
            });

            ProgressForm ProgressForm = new ProgressForm(Module.Name, "Initializing...", task, Module);
            ProgressForm.StopProcess += ProgressForm_StopProcess;
            ProgressForm.StartPosition = FormStartPosition.CenterScreen;
            ProgressForm.ShowDialog();
        }
        public void CreateButton()
        {
            foreach (Part part in Module.Parts)
            {
                if(part is MotionPart || part is Camera)
                {                    
                    BaseButton btn = new BaseButton();
                    btn.Text = part.Name;
                    btn.Name = string.Format("button", part.Name);
                    btn.Click += InitailizePart_Click;
                    btn.Height += 5;                                            //  2023. 04. 26.  SCH : Part Initialize 부분에 버튼 크기(높이) 조정. (2줄 짜리 Caption 이 안보여서...)
                    this.flowLayoutPanelButton.Controls.Add(btn);
                }
            }
        }

        private void InitailizePart_Click(object sender, EventArgs e)
        {
            BaseButton btn = sender as BaseButton;
            
            if(btn != null)
            {
                Part Target = null;
                foreach(Part part in Module.Parts)
                {
                    if(part.Name == btn.Text)
                    {
                        Target = part;
                        break;
                    }
                }


                if(Target != null)
                {
                    Task<int> task = Task.Factory.StartNew<int>(() =>
                    {
                        Target.SetRunStatus(Part.RunStatus.Run);
                        Target.Initialize();
                        return 0;
                    });

                    ProgressForm ProgressForm = new ProgressForm(Target.Name, "Initializing...", task, Target);
                    ProgressForm.StopProcess += ProgressForm_StopProcess;
                    ProgressForm.StartPosition = FormStartPosition.CenterScreen;
                    ProgressForm.ShowDialog();

                    if(Target.Name == "Stage")
                    {
                        if(Module !=null && Module is WorkStage)
                        {
                            ((WorkStage)Module).m_bHomeOK = true;
                        }
                    }
                }
            }
        }

        private void ProgressForm_StopProcess(object obj)
        {
            Part part = obj as Part;
            if(part != null)
            {
                part.Stop();
            }
        }
    }
}
