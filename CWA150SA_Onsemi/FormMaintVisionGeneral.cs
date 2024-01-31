using QMC.Common;
using QMC.Common.Hmi;
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
    public partial class FormMaintVisionGeneral : FormSubContentBase
    {
        public Module m_Module;
        public FormMaintVisionGeneral(Module module)
                 : base(FormType.Content.ToString(), module.Name)
        {
            m_Module = module;
            InitializeComponent();
            //this.panelContent.Location = new Point(0, 0);
            //this.panelContent.Size = new System.Drawing.Size(Configuration.ContentSize.Width, Configuration.FormContentSize.Height);
            this.panelContent.Location = new System.Drawing.Point(0, 3);
            this.panelContent.Size = new System.Drawing.Size(Configuration.ContentSize.Width, Configuration.FormContentSize.Height );
            FunctionControl FunctionControl = new FunctionControl(m_Module);
            ModuleStateControl ModuleStateControl = new ModuleStateControl(m_Module);
            JogControl JogControl = new JogControl(m_Module);
            VisionImageViewer visionImageViewer = new VisionImageViewer();
            //visionControl  추가

            visionImageViewer.Size = Configuration.VisionImageViewerSize;
            ModuleStateControl.Location = (Point)Configuration.ModuleStateControlLocation;
            visionImageViewer.Location = new Point(Configuration.ContentLocation.X + ModuleStateControl.Size.Width, Configuration.PanelSize.Height);
            FunctionControl.Location = (Point)Configuration.FunctionControlLocation;
            JogControl.Location = (Point)Configuration.JogControlLocation;

            this.panelContent.Controls.Add(FunctionControl);
            this.panelContent.Controls.Add(ModuleStateControl);
            this.panelContent.Controls.Add(JogControl);
            this.panelContent.Controls.Add(visionImageViewer);
            //VisionControl Location추가
            this.panelContent.Controls.Add(baseLabelTitle);
        }
    }
}
