using QMC.Common;
using QMC.Common.Hmi;
using QMC.Common.Modules;
using QMC.Common.VisionPart;
using SpiralLab.Sirius;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QMC.Common.VisionPart.VisionCompensator;
using Point = System.Drawing.Point;

namespace CWA150SA_Onsemi300
{
    #region FormVisionCompensatorMaint
    public partial class FormVisionCompensatorMaint : FormSubContentBase
    {
        #region Field
        private VisionCompensator m_Owner;
        private VisionCompensatorGeneralControl m_GeneralControl;
        private ModulePositionControl m_GridPositionControl;
        private VisionCompensatorOffsetControl m_OffsetControl;
        private AutoFocusControl m_AutoFocusControl;
        private JogControl m_JogControl;
        #endregion

        public FormVisionCompensatorMaint(Part part)
            : base(FormType.Maint.ToString(), part.Name)
        {
            this.m_Owner = part as VisionCompensator;
            this.m_GeneralControl = new VisionCompensatorGeneralControl();
            this.m_GridPositionControl = new ModulePositionControl();
            this.m_visionImageViewer_Upper = new VisionImageViewer();
            this.m_OffsetControl = new VisionCompensatorOffsetControl(m_Owner.Config);
            this.m_JogControl = new JogControl(m_Owner.Stage);
            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();


            this.panelContent.Visible = false;
            this.panelContent.Size = new Size();

            this.Controls.Add(this.m_visionImageViewer_Upper);
            this.Controls.Add(this.m_GeneralControl);
            this.Controls.Add(this.m_GridPositionControl);
            this.Controls.Add(this.m_OffsetControl);

            this.m_visionImageViewer_Upper.SizeMode = PictureBoxSizeMode.StretchImage;
            this.m_visionImageViewer_Upper.SuspendDisplay();
            this.m_visionImageViewer_Upper.Location = new Point(Configuration.ContentLocation.X, Configuration.ContentLocation.Y);
            //this.m_visionImageViewer.Size = new Size(Configuration.MaintMinImageViewSize.Width, Configuration.MaintMinImageViewSize.Height - 100);
            this.m_visionImageViewer_Upper.Size = new Size(Configuration.MaintMinImageViewSize.Width - 65, Configuration.MaintMinImageViewSize.Height - 65);
            this.m_visionImageViewer_Upper.Camera = m_Owner.Camera;

            this.m_GeneralControl.Location = new Point(this.m_visionImageViewer_Upper.Location.X + this.m_JogControl.Width + Configuration.ControlGap, this.m_visionImageViewer_Upper.Location.Y);
            this.m_OffsetControl.Location = new Point(this.m_GeneralControl.Location.X + this.m_GeneralControl.Width + Configuration.ControlGap, this.m_GeneralControl.Location.Y);
            this.m_GridPositionControl.Location = new Point(this.m_GeneralControl.Location.X, this.m_GeneralControl.Location.Y + this.m_GeneralControl.Height + Configuration.ControlGap);

            this.m_GridPositionControl.SetGroupboxName(" Start Position ");
            this.m_GridPositionControl.SetPositionList(m_Owner.Config.Parameter.GridPositions);
            this.m_GridPositionControl.ButtonClick += PositionControlButtonClick;

            this.m_OffsetControl.ButtonClick += OffsetControl_ButtonClick;
            this.m_GeneralControl.Owner = this.m_Owner;
            this.m_GeneralControl.Init();

            this.m_AutoFocusControl = new AutoFocusControl(((WaferProbeAlign)m_Owner.Owner).autoFocuser_Upper, ((WaferProbeAlign)m_Owner.Owner));
            this.m_AutoFocusControl.Location = new Point(this.m_GridPositionControl.Location.X + this.m_GridPositionControl.Width + Configuration.ControlGap, this.m_GridPositionControl.Location.Y);
            this.Controls.Add(this.m_AutoFocusControl);

            this.m_JogControl.Location = new Point(this.m_visionImageViewer_Upper.Location.X, this.m_visionImageViewer_Upper.Location.Y + m_visionImageViewer_Upper.Height + Configuration.ControlGap);
            this.Controls.Add(this.m_JogControl);
        }

        #region Event Handler
        private void OffsetControl_ButtonClick(string strName)
        {
            if (strName == "Load")
            {
                Module module = m_Owner.Owner;
                if (module != null)
                {
                    module.UpdateConfigData(); // 참고 : param load
                    DataManager.Instance.ApplyConfigData(module);

                    if (m_Owner.Config != null && m_Owner.Config.XyGridSearchResults.Count != 0)
                    {
                        this.m_OffsetControl.UpdateGridData();
                    }
                }
            }
            else if (strName == "Save")
            {
                Module module = m_Owner.Owner;
                if (module != null)
                {
                    DataManager.Instance.UpdateConfigData(module); // 참고 : param save
                    module.SaveConfigData();
                }
            }
        }

        protected override void OnVisibleChanged()
        {
            base.OnVisibleChanged();

            if (this.Visible)
            {
            }
            else
            {

            }
        }

        private void PositionControlButtonClick(ModulePositionControl.ButtonType type)
        {
            if (type == ModulePositionControl.ButtonType.Save)
            {
                string strPosition = m_GridPositionControl.SelectedPosition;
                //XyztCoordinate coordinate = new XyztCoordinate();// = m_Owner.GetCurrentPosition();
                //XyzztCoordinate coordinate = new XyzztCoordinate();// = m_Owner.GetCurrentPosition();
                UvwzxyzCoordinate coordinate = new UvwzxyzCoordinate();// = m_Owner.GetCurrentPosition();

                m_Owner.Stage.GetCommandPosition(ref coordinate);
                //m_Owner.Config.Parameter.GridPositions
                m_Owner.Config.Parameter.CrossPositions[(int)GridXyMotionPositionKeys.StartPosition].Coordinate = coordinate;
                m_Owner.Config.Parameter.GridPositions[(int)GridXyMotionPositionKeys.StartPosition].Coordinate = coordinate;
                //m_Owner.Config.SetCalibratorPosition(strPosition, TargetType.Base, coordinate);
                m_Owner.Owner.SaveConfigData();
            }
            else if (type == ModulePositionControl.ButtonType.Move)
            {
                string strPosition = m_GridPositionControl.SelectedPosition;
                //XyztCoordinate coordinate = m_Owner.Config.Parameter.GridPositions[(int)GridXyMotionPositionKeys.StartPosition].Coordinate;
                //XyzztCoordinate coordinate = m_Owner.Config.Parameter.GridPositions[(int)GridXyMotionPositionKeys.StartPosition].Coordinate;
                UvwzxyzCoordinate coordinate = m_Owner.Config.Parameter.GridPositions[(int)GridXyMotionPositionKeys.StartPosition].Coordinate;

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
    #endregion
}
