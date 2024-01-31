using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QMC.Common;
using QMC.Common.VisionPart;

namespace CWA150SA_Onsemi300
{
    public partial class TwoPointAlignerRecipeControl : PatternMatchingRecipeControl
    {
        private TwoPointAligner m_twoPointAligner;
        public TwoPointAlignerRecipeControl() : this(null)
        {            
        }

        public TwoPointAlignerRecipeControl(Part part) : base(part)
        {
            InitializeComponent();
        }

        protected override void SetPart(Part part)
        {
            m_twoPointAligner = part as TwoPointAligner;
            m_Recipe = m_twoPointAligner.Recipe;
        }
    }
}
