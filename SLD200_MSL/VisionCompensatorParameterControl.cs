using QMC.Common;
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

namespace SLD200_MSL
{
    #region VisionCompensatorParameterControl
    public partial class VisionCompensatorParameterControl : UserControl
    {
        #region Field
        private VisionCompensatorParameter m_Parameter;
        #endregion

        #region Constructor
        public VisionCompensatorParameterControl(VisionCompensatorParameter parameter)
        {
            m_Parameter = parameter;

            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();

            SetParameterProperty();
        }
        #endregion

        #region Method
        public void SetParameterProperty()
        {
            this.basePropertyGridParameter.SelectedObject = null;
            this.basePropertyGridParameter.SelectedObject = m_Parameter;
        }
        #endregion
    }
    #endregion
}
