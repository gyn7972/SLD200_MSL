using SLD200_MSL;
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

namespace SLD200_MSL
{
    public partial class LaserPitchMoveShotterControl : UserControl
    {
        #region Field
        public LaserPitchMoveShotter m_Owner { get; set; }
        #endregion

        #region Constructor
        public LaserPitchMoveShotterControl(Part part)
        {
            m_Owner = part as LaserPitchMoveShotter;

            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();

            if (m_Owner != null)
                Init();
        }
        #endregion

        #region Method
        public void Init()
        {
            this.basePropertyGridParameter.SelectedObject = null;
            this.basePropertyGridParameter.SelectedObject = m_Owner.Config;
        }
        #endregion

        #region Event Handler
        private void baseButtonRun_Click(object sender, EventArgs e)
        {
            Task<int> task = Task.Factory.StartNew(() =>
            {
                int ret = 0;

                this.m_Owner.Work();

                return ret;
            });

            ProgressForm progressForm = new ProgressForm("Vision Compensator", "Compensating...", task);
            progressForm.StopProcess += ProgressForm_StopProcess;
            progressForm.ShowDialog();

            this.m_Owner.Camera.StartLive();
        }

        private void ProgressForm_StopProcess(object target)
        {
            this.m_Owner.Stop();
        }
        #endregion
    }
}
