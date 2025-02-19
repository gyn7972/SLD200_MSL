using SLD200_MSL;
using QMC.Common;
using QMC.Common.Hmi;
using QMC.Common.Modules;
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
using static QMC.Common.Parts.LaserPitchMoveShotter;

namespace SLD200_MSL
{
    public partial class FormLaserPitchMoveShotterMaint : FormSubContentBase
    {
        #region Field
        private LaserPitchMoveShotter m_Owner;
        private JogControl m_JogControl;
        private AutoFocusControl m_AutoFocusControl;
        private ModulePositionControl m_GridPositionControl;
        private LaserPitchMoveShotterControl m_LaserPitchMoveShotterControl;
        #endregion

        #region Constructor
        public FormLaserPitchMoveShotterMaint(Part part)
            : base(FormType.Maint.ToString(), part.Name)
        {
            this.m_Owner = part as LaserPitchMoveShotter;

            this.panelContent.Visible = false;
            this.panelContent.Size = new Size();
            InitializeComponent();

            this.m_visionImageViewer_Upper = new VisionImageViewer();
            this.m_visionImageViewer_Upper.SizeMode = PictureBoxSizeMode.StretchImage;
            this.m_visionImageViewer_Upper.SuspendDisplay();
            this.m_visionImageViewer_Upper.Location = new Point(Configuration.ContentLocation.X, Configuration.ContentLocation.Y);
            //this.m_visionImageViewer.Size = new Size(450, 300);
            this.m_visionImageViewer_Upper.Size = new Size(Configuration.MaintMinImageViewSize.Width - 65, Configuration.MaintMinImageViewSize.Height - 65);
            this.m_visionImageViewer_Upper.Camera = m_Owner.Camera;
            this.Controls.Add(m_visionImageViewer_Upper);

            this.m_LaserPitchMoveShotterControl = new LaserPitchMoveShotterControl(this.m_Owner);
            this.m_LaserPitchMoveShotterControl.Location = new Point(this.m_visionImageViewer_Upper.Location.X + this.m_visionImageViewer_Upper.Width + Configuration.ControlGap + 65, this.m_visionImageViewer_Upper.Location.Y);
            this.Controls.Add(this.m_LaserPitchMoveShotterControl);

            this.m_JogControl = new JogControl(m_Owner.Stage);
            this.m_JogControl.Location = new Point(this.m_visionImageViewer_Upper.Location.X, this.m_visionImageViewer_Upper.Location.Y + this.m_visionImageViewer_Upper.Height + Configuration.ControlGap);
            this.Controls.Add(this.m_JogControl);

            this.m_GridPositionControl = new ModulePositionControl();
            this.m_GridPositionControl.Location = new Point(this.m_JogControl.Location.X + this.m_JogControl.Width + Configuration.ControlGap, this.m_JogControl.Location.Y);
            this.m_GridPositionControl.SetGroupboxName(" Start Position ");
            this.m_GridPositionControl.SetPositionList(m_Owner.Config.GridPositions);
            this.m_GridPositionControl.ButtonClick += PositionControlButtonClick;
            this.Controls.Add(this.m_GridPositionControl);

            this.m_AutoFocusControl = new AutoFocusControl(((WorkStage)m_Owner.Owner).autoFocuser_HighRes, ((WorkStage)m_Owner.Owner));
            this.m_AutoFocusControl.Location = new Point(this.m_GridPositionControl.Location.X, this.m_GridPositionControl.Location.Y + this.m_GridPositionControl.Height + Configuration.ControlGap);
            this.Controls.Add(this.m_AutoFocusControl);
        }
        #endregion

        #region Event Handler
        private void PositionControlButtonClick(ModulePositionControl.ButtonType type)
        {
            if (type == ModulePositionControl.ButtonType.Save)
            {
                string strPosition = m_GridPositionControl.SelectedPosition;
                XyzCoordinate coordinate = new XyzCoordinate();// = m_Owner.GetCurrentPosition();
                m_Owner.Stage.GetCommandPosition(ref coordinate);
                m_Owner.Config.GridPositions[(int)GridXyMotionPositionKeys.StartPosition].Coordinate = coordinate;
                //m_Owner.Config.SetCalibratorPosition(strPosition, TargetType.Base, coordinate);
                m_Owner.Owner.SaveConfigData();
            }
            else if (type == ModulePositionControl.ButtonType.Move)
            {
                string strPosition = m_GridPositionControl.SelectedPosition;
                XyzCoordinate coordinate = m_Owner.Config.GridPositions[(int)GridXyMotionPositionKeys.StartPosition].Coordinate;

                Task<int> result = m_Owner.Stage.BeginMovePosition(coordinate); // 참고: 무빙, 프로그래스 바 연동
                ProgressForm progressForm = new ProgressForm("Stage", "Stage Moving...", result);
                progressForm.StopProcess += ProgressForm_StopProcess;
                progressForm.ShowDialog();
            }
        }

        private void ProgressForm_StopProcess(object target)
        {
            m_Owner.Stop();
        }
        #endregion
    }
}
