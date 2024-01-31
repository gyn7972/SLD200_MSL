using QMC.Common;
using QMC.Common.Modules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QMC.Common.Parts;

namespace CWA150SA_Onsemi300
{
    public partial class FormWaferProbeAlignParameterRecipe : FormSubContentBase
    {
        protected WaferProbeAlign m_Owner;
        //private IlluminatorRecipeControl m_IllumenatorRecipeControl;
        private PatternMatchingRecipeControl m_VisionCalibratorRecipeControl;

        public FormWaferProbeAlignParameterRecipe(WaferProbeAlign waferProbeAlign)
            : base(FormType.Content.ToString(), waferProbeAlign.Name)
        {
            InitializeComponent();
            m_Owner = waferProbeAlign;

            this.panelContent.Visible = false;
            this.panelContent.Size = new System.Drawing.Size();

            TabControl tabControl = new TabControl();
            TabPage m_VisionCalibratorTabPage = new TabPage("VisionCalibrator");

            tabControl.Location = new Point(Configuration.ContentLocation.X, Configuration.ContentLocation.Y + Configuration.ButtonSize.Height + Configuration.PanelSize.Height);
            

            m_VisionCalibratorRecipeControl = new PatternMatchingRecipeControl(m_Owner.visionCalibrator_Upper);
            m_VisionCalibratorRecipeControl.SetGroupBoxName("VisionCalibrator");
            m_VisionCalibratorRecipeControl.BackColor = Color.FromArgb(90, 90, 90);
            m_VisionCalibratorTabPage.Controls.Add(m_VisionCalibratorRecipeControl);

            tabControl.Controls.Add(m_VisionCalibratorTabPage);
            tabControl.Size = new Size(m_VisionCalibratorRecipeControl.Width + Configuration.ControlGap, m_VisionCalibratorRecipeControl.Height + Configuration.ControlGap);
            this.Controls.Add(tabControl);
        }
    }
}
