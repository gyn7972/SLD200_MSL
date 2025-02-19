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
    public delegate void RunButtonClickEvent();

    public partial class ScannerCompensatorGeneralControl : UserControl
    {
        #region Field
        private ScannerCompensator m_Owner;
        public event RunButtonClickEvent RunButtonClick;
        #endregion

        #region Constructor
        public ScannerCompensatorGeneralControl(Part part)
        {
            m_Owner = part as ScannerCompensator;

            InitializeComponent();

            if (m_Owner != null)
                this.Init();
        }
        #endregion

        #region Method
        public void Init()
        {
            this.basePropertyGridParameter.SelectedObject = m_Owner.Config;
        }
        #endregion

        private void baseButtonRun_Click(object sender, EventArgs e)
        {
            m_Owner.SetRunStatus(Part.RunStatus.Run);
            m_Owner.Stage.SetRunStatus(Part.RunStatus.Run);

            Task<int> task = Task.Factory.StartNew(() =>
            {
                int ret = 0;

                m_Owner.Work();

                return ret;
            });

            ProgressForm progressForm = new ProgressForm("Scanner Compensator", "Compensating...", task);
            progressForm.StopProcess += ProgressForm_StopProcess;
            progressForm.ShowDialog();

            this.m_Owner.Camera.StartLive();

            //if (this.RunButtonClick != null)
            //{
            //    this.RunButtonClick();
            //}
        }

        private void ProgressForm_StopProcess(object target)
        {
            if(m_Owner != null)
            {
                m_Owner.Stop();
            }
        }
    }
}
