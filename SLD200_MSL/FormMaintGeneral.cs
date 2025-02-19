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

namespace SLD200_MSL
{
    public partial class FormMaintGeneral : FormSubContentBase
    {
        public Module m_Module;
        public Part m_Part;
        List<MotionAxis> m_Axis;
        public FormMaintGeneral(Module module)
            : base(FormType.Content.ToString(), module.Name)
        {
            m_Module = module;
            InitializeComponent();
            //    this.size
            m_Axis = new List<MotionAxis>();
            this.panelContent.Location = new Point(Configuration.ContentLocation.X, Configuration.PanelbuttonSize.Height);
            for (int i = 0; i < m_Module.Parts.Count; i++)
            {
                List<MotionAxis> motionAxes = null;// m_Module.Parts[i].GetMotionList();
                foreach (MotionAxis motionAxis in motionAxes)
                {
                    m_Axis.Add(motionAxis);
                }
            }
            if (m_Axis.Count > 0 )
            {
                JogControl JogControl = new JogControl(m_Module);
                FunctionControl FunctionControl = new FunctionControl(m_Module);
                ModuleStateControl ModuleStateControl = new ModuleStateControl(m_Module);
                ModuleStateControl.Location = new Point(3, Configuration.PanelSize.Height);
                JogControl.Location = new Point(ModuleStateControl.Location.X + ModuleStateControl.Size.Width, Configuration.PanelSize.Height);
                FunctionControl.Location = new Point(this.panelContent.Width - FunctionControl.Size.Width - Configuration.ContentLocation.X * 2, ModuleStateControl.Location.Y);
                this.panelContent.Controls.Add(FunctionControl);
                this.panelContent.Controls.Add(ModuleStateControl);
                this.panelContent.Controls.Add(JogControl);
                this.panelContent.Controls.Add(baseLabelTitle);
            }
            else
            {
                FunctionControl FunctionControl = new FunctionControl(m_Module);
                ModuleStateControl ModuleStateControl = new ModuleStateControl(m_Module);
                ModuleStateControl.Location = new Point(3, Configuration.PanelSize.Height);
                //JogControl.Location = new Point(Configuration.ContentLocation.X + ModuleStateControl.Size.Width, Configuration.PanelSize.Height);
                FunctionControl.Location = new Point(this.panelContent.Width - FunctionControl.Size.Width - Configuration.ContentLocation.X * 2, ModuleStateControl.Location.Y);
                this.panelContent.Controls.Add(FunctionControl);
                this.panelContent.Controls.Add(ModuleStateControl);
                //this.panelContent.Controls.Add(JogControl);
                this.panelContent.Controls.Add(baseLabelTitle);
            }
        }
    }
}
