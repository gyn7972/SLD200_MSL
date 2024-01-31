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

namespace QMC.Common.UI
{
    public partial class AutoFocusConfigControl : UserControl
    {
        private AutoFocuserConfig m_Config;
        public AutoFocusConfigControl(AutoFocuserConfig config)
        {
            m_Config = config;
            InitializeComponent();
            Init();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();

            this.PropertyGridAutoFocus.Size = new Size(this.PropertyGridAutoFocus.Size.Width, 250);
            this.Size = new Size(298, 280);
        }

        private void Init()
        {
            this.PropertyGridAutoFocus.SelectedObject = m_Config;
        }
    }
}
