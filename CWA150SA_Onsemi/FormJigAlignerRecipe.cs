using CWA150SA_Onsemi300;
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

namespace CWA150SA_Onsemi
{
    public partial class FormJigAlignerRecipe : FormSubContentBase
    {
        #region Field
        protected JigAligner m_Owner;
        private JigAlignerRecipeControl m_JigAlingerRecipeControl;
        #endregion

        public FormJigAlignerRecipe(JigAligner jigAligner)
            : base(FormType.Content.ToString(), jigAligner.Name)
        {
            InitializeComponent();

            m_Owner = jigAligner;

            this.panelContent.Visible = false;
            this.panelContent.Size = new System.Drawing.Size();

            this.m_JigAlingerRecipeControl = new JigAlignerRecipeControl(m_Owner);
            this.m_JigAlingerRecipeControl.Location = new Point(Configuration.ContentLocation.X, Configuration.ContentLocation.Y + Configuration.ButtonSize.Height + Configuration.PanelSize.Height);

            this.Controls.Add(this.m_JigAlingerRecipeControl);
        }
    }
}
