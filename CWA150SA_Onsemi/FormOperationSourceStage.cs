using QMC.Common;
using QMC.Common.Hmi;
using QMC.Common.VisionPart;
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
    public partial class FormOperationSourceStage : FormSubContentBase
    {
        #region Field
        Module m_Module;
        //TwoChipAlinger m_Alignpart;
        //ChipFinder m_chipFinder;
        #endregion

        public FormOperationSourceStage(Module module)
            :base(FormType.withButton.ToString(), OperationModuleName.SourceLoader.ToString())
        {
            m_Module = module;
            InitializeComponent();

            this.panelContent.Location = new System.Drawing.Point(0, 0);    //  20);   //  8);
            this.panelContent.Size = new System.Drawing.Size(Configuration.ContentSize.Width, Configuration.FormContentSize.Height);
            //foreach (Part part in m_Module.Parts)
            //{
            //    if (part is TwoChipAlinger)
            //    {
            //        m_Alignpart = part as TwoChipAlinger;
            //    }
            //    else if (part is ChipFinder)
            //    {
            //        m_chipFinder = part as ChipFinder;
            //    }
            //}

           

        }

        public void ShowControls()
        {
            FunctionControl FunctionControl = new FunctionControl(m_Module);
            JogControl JogControl = new JogControl(m_Module);
            VisionImageViewer visionImageViewer = new VisionImageViewer();

            visionImageViewer.Size = Configuration.VisionImageViewerSize;
            visionImageViewer.Location = new Point(3, Configuration.PanelSize.Height);
            FunctionControl.Location = (Point)Configuration.FunctionControlLocation;
            JogControl.Location = (Point)Configuration.JogControlLocation;

            this.panelContent.Controls.Add(FunctionControl);
            this.panelContent.Controls.Add(JogControl);
            this.panelContent.Controls.Add(visionImageViewer);
            this.panelContent.Controls.Add(baseLabelTitle);

            //if (m_Alignpart != null)
            //{
            //    m_Alignpart.TestImage.Load(@"D:\Document\Projects\MDT-400P\[MDT-400P]211207_Red Chip 취득이미지\1.SourceFinder_RealTimeScan Image\11_43_38_729464_1233294090.bmp", QMC.Common.Vision.VisionImage.FileFilter.bmp);
            //    visionImageViewer.Simulated = true;
            //    visionImageViewer.Camera = m_Alignpart.Camera;
            //    m_Alignpart.Parameter.ResultOverlayVisible = true;
            //    visionImageViewer.InputImage = m_Alignpart.TestImage;
            //    visionImageViewer.Display();

            //}
            //if (m_chipFinder != null)
            //{
            //    m_chipFinder.TestImage.Load(@"D:\Document\Projects\MDT-400P\[MDT-400P]211207_Red Chip 취득이미지\1.SourceFinder_RealTimeScan Image\11_43_38_729464_1233294090.bmp", QMC.Common.Vision.VisionImage.FileFilter.bmp);
            //    visionImageViewer.Simulated = true;
            //    visionImageViewer.Camera = m_chipFinder.Camera;
            //    m_chipFinder.Parameter.ResultOverlayVisible = true;
            //    visionImageViewer.InputImage = m_chipFinder.TestImage;
            //    visionImageViewer.Display();
            //}
        }

        private void FormOperationSourceStage_Load(object sender, EventArgs e)
        {
            ShowControls();
        }
    }
}
