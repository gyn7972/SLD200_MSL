using SLD200_MSL;
using QMC.Common.Parts;
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
    public partial class FormScannerCompensatorRecipe : FormSubContentBase
    {
        #region Field
        protected ScannerCompensator m_Owner;
        private NormalDataConfigControl m_ScannerCompensatorRecipe;
        #endregion

        public FormScannerCompensatorRecipe(ScannerCompensator owner)
            : base(FormType.Content.ToString(), owner.Name)
        {
            InitializeComponent();

            m_Owner = owner;

            this.panelContent.Visible = false;
            this.panelContent.Size = new System.Drawing.Size();

            this.m_ScannerCompensatorRecipe = new NormalDataConfigControl();
            this.m_ScannerCompensatorRecipe.Location = new Point(flowLayoutPanelButton.Location.X + Configuration.ControlGap, flowLayoutPanelButton.Location.Y + flowLayoutPanelButton.Size.Height + Configuration.ControlGap);
            this.m_ScannerCompensatorRecipe.SetData(m_Owner.Recipe);
            this.m_ScannerCompensatorRecipe.SetGroupBoxName("Recipe");
            this.Controls.Add(this.m_ScannerCompensatorRecipe);
        }
    }
}
