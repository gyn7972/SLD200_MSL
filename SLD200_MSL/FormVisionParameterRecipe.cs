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

namespace SLD200_MSL
{
    public partial class FormLoaderParameterRecipe : FormSubContentBase
    {
        protected Loader m_Owner;
        //private IlluminatorRecipeControl m_IllumenatorRecipeControl;
        private PatternMatchingRecipeControl m_VisionCalibratorRecipeControl;
        
        public FormLoaderParameterRecipe(Loader loader)
            : base(FormType.Content.ToString(), loader.Name)
        {
            InitializeComponent();
            m_Owner = loader;

            this.panelContent.Visible = false;
            this.panelContent.Size = new System.Drawing.Size();

            TabControl tabControl = new TabControl();
            TabPage m_VisionCalibratorTabPage = new TabPage("VisionCalibrator");

            tabControl.Location = new Point(Configuration.ContentLocation.X, Configuration.ContentLocation.Y + Configuration.ButtonSize.Height + Configuration.PanelSize.Height);
            

            //m_VisionCalibratorRecipeControl = new PatternMatchingRecipeControl(m_Owner.visionCalibrator_HighRes);
            //m_VisionCalibratorRecipeControl.SetGroupBoxName("VisionCalibrator");
            //m_VisionCalibratorRecipeControl.BackColor = Color.FromArgb(90, 90, 90);
            //m_VisionCalibratorTabPage.Controls.Add(m_VisionCalibratorRecipeControl);

            tabControl.Controls.Add(m_VisionCalibratorTabPage);
            tabControl.Size = new Size(m_VisionCalibratorRecipeControl.Width + Configuration.ControlGap, m_VisionCalibratorRecipeControl.Height + Configuration.ControlGap);
            this.Controls.Add(tabControl);
        }
    }
}
