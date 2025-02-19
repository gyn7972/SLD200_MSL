using QMC.Common;
using QMC.Common.Modules;
using QMC.Common.VisionPart;
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
    public partial class AutoFocusControl : UserControl
    {
        #region Field
        private AutoFocuser m_AutoFocuser;
        private AutoFocusResult m_Result;
        private WorkStage m_Owner;
        #endregion

        #region Constructor
        public AutoFocusControl(AutoFocuser autoFocuser, Part part)
        {
            m_AutoFocuser = autoFocuser;
            m_Owner = part as WorkStage;
            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
        }
        public AutoFocusControl()
        {

        }
        #endregion

        #region Event Handler
        private void baseButtonAutoFocus_Click(object sender, EventArgs e)
        {
            Task<int> task = Task.Factory.StartNew(() =>
            {
                int ret = 0;
                ret = m_AutoFocuser.Work();

                this.Invoke(new Action(() =>
                {
                    //화면에 출력.
                    Init();
                }));

                return ret;
            });

            ProgressForm progressForm = new ProgressForm("Auto Focus", "Auto Focusing...", task);
            progressForm.StopProcess += ProgressForm_StopProcess;
            progressForm.ShowDialog();


            m_Result = m_AutoFocuser.Result;
            //m_Owner.Config.AutoCalzPos[m_Owner.GetColletIndex(Turret.PoistionKey.RevisionPart)] = m_Result.BestFocusPosition;
        }

        private void ProgressForm_StopProcess(object target)
        {
            m_AutoFocuser.Stop();
        }
        #endregion

        #region Method
        private void Init()
        {
            if (m_AutoFocuser != null && m_AutoFocuser.Result != null)
            {
                this.baseTextBoxPosZ.Text = m_AutoFocuser.Result.BestFocusPosition.ToString();
                this.baseTextBoxValue.Text = m_AutoFocuser.Result.Score.ToString();
            }
        }
        #endregion
    }
}
