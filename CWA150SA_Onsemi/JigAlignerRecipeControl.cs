using CWA150SA_Onsemi300;
using QMC.Common;
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
    public partial class JigAlignerRecipeControl : PatternMatchingRecipeControl
    {
        #region Field
        private JigAligner m_Owner;
        #endregion

        #region Constructor
        public JigAlignerRecipeControl(Part part) : base(part)
        {
            InitializeComponent();
        }
        #endregion

        #region PatternMatchingRecipeControl Members
        protected override void SetPart(Part part)
        {
            m_Owner = part as JigAligner;
            m_Recipe = m_Owner.Recipe;
            Parameters = m_Owner.Recipe.PatternMatchingParameter;
        }
        #endregion
    }
}
