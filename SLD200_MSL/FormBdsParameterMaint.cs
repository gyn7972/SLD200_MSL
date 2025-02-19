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
using QMC.Common.Hmi;
using QMC.Common.Modules;
using QMC.Common.Motion.ACS.Motions;
using QMC.Common.Parts;
using ACS.SPiiPlusNET;
//using GCodeNet.Commands;

namespace SLD200_MSL
{
    public partial class FormBdsParameterMaint : FormSubContentBase
    {
        #region Field
        public Module m_Module;
        //protected ModuleStateControl m_ModuleStateControl;
        public ModulePositionControl m_BdsPosControl;
        public JogControl m_JogControl;
        //public AutoFocusControl m_AutoFocusControl;

        protected Bds m_Owner;
        #endregion

        #region Constructor
        public FormBdsParameterMaint(Module module)
             : base(FormType.Maint.ToString(), module.Name)
        {
            m_Module = module;

            int nGap = 20;

            m_Owner = module as Bds;

            if (m_Owner == null)
            {
                return;
            }

            InitializeComponent();

            this.panelContent.Visible = false;
            this.panelContent.Size = new Size();

            //m_ModuleStateControl.Location = new Point(Configuration.ContentLocation.X, Configuration.ContentLocation.Y);
            //m_ModuleStateControl.Size = new System.Drawing.Size(Configuration.MotorStatusControlFlowPanelSize.Width, Configuration.ContentSize.Height);
            //this.Controls.Add(this.m_ModuleStateControl);

            //m_JogControl = new JogControl(m_Owner);
            //m_JogControl = new JogControl(m_Owner.workStageParameter);
            m_JogControl = new JogControl(m_Owner.Stage);
            //m_JogControl.Location = new Point(m_WorkStagePosControl.Location.X + m_WorkStagePosControl.Size.Width + nGap, Configuration.ContentLocation.Y + nGap);
            //m_JogControl.Location = new Point(m_visionImageViewer.Location.X + m_visionImageViewer.Size.Width + nGap, Configuration.ContentLocation.Y/* + nGap*/);
            m_JogControl.Location = new Point(10, 10);
            this.Controls.Add(m_JogControl);

            m_BdsPosControl = new ModulePositionControl();
            //m_WorkStagePosControl.Location = new Point(m_visionImageViewer.Location.X + m_visionImageViewer.Size.Width + nGap, Configuration.ContentLocation.Y + nGap);
            m_BdsPosControl.Location = new Point(m_JogControl.Location.X, m_JogControl.Location.Y + m_JogControl.Size.Height + Configuration.ControlGap * 2);
            //m_DispenserPosControl.SetPositionList(m_Owner.Config.Positions);
            m_BdsPosControl.SetPositionList(m_Owner.bdsParameter.Config.BdsPositions);
            //m_WorkStagePosControl.SetPositionList(m_Owner.Config.Positions);
            m_BdsPosControl.SetGroupboxName(" Bds Position Control ");
            this.Controls.Add(m_BdsPosControl);

            //  여기는 Auto Focus 빼자
            //m_AutoFocusControl = new AutoFocusControl(m_Owner.autoFocuser, m_Owner);
            //m_AutoFocusControl.Location = new Point(m_WorkStagePosControl.Location.X + m_WorkStagePosControl.Size.Width + Configuration.ControlGap, m_WorkStagePosControl.Location.Y);
            //this.Controls.Add(m_AutoFocusControl);

            //m_GripperControl = new GripperContorl(module);
            //m_GripperControl.Location = new Point(m_ModulePosControl.Location.X + m_ModulePosControl.Size.Width + 5, m_JogControl.Size.Height + 20);
            //m_GripperControl.SetGroupBoxName(" Gripper ");

            //m_visionImageViewer_Upper.Camera = m_Owner.Camera_HighRes;
            //m_visionImageViewer_Lower.Camera = m_Owner.Camera_LowRes;
            m_BdsPosControl.ButtonClick += OnClickBdsPosControlButton;
        }
        #endregion

        #region Event Handler
        private void OnClickBdsPosControlButton(ModulePositionControl.ButtonType type)
        {
            if (type == ModulePositionControl.ButtonType.Save)
            {
                //선택된 Position 확인.
                string strPosition = m_BdsPosControl.SelectedPosition;
                BdsParameter.stBdsParam parameter;
                parameter = m_Owner.bdsParameter.GetPositionInformation(strPosition);

                ////현재 X의 위치를 가져옴.                
                //double xPosition = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)WorkStageParameter.AxisAcsEnum.StageX);            //  현재 X 위치
                ////현재 Y의 위치를 가져옴.
                //double yPosition = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)WorkStageParameter.AxisAcsEnum.StageY);            //  현재 Y 위치

                //  현재 MASK Y 의 위치를 가져옴.
                double maskYPosition = m_Owner.MC_Func.MC_GetEncPos((int)BdsParameter.AxisAjinEnum.MASK_Y);

                maskYPosition = Math.Round(maskYPosition, 4);

                YCoordinate savePosition = new YCoordinate(maskYPosition);

                m_Owner.Config.SetPositionData(strPosition, TargetType.Base, savePosition);

                Bds bds = m_Owner.Owner as Bds;
                if (bds != null)
                {
                    bds.SaveConfigData();
                }

                //  갱신
                m_BdsPosControl.SetPositionList(m_Owner.bdsParameter.Config.BdsPositions);
            }
            else if (type == ModulePositionControl.ButtonType.Move)
            {
                string strPosition = m_BdsPosControl.SelectedPosition;
                BdsParameter.stBdsParam parameter;
                YCoordinate basePosition = new YCoordinate();
                YCoordinate offsetPosition = new YCoordinate();
                YCoordinate targetPosition = new YCoordinate();

                parameter = m_Owner.bdsParameter.GetPositionInformation(strPosition);

                //                m_Owner.MC_Func.MC_MovePosition
                basePosition = m_Owner.Config.GetPositionData(strPosition, TargetType.Base);
                offsetPosition = m_Owner.Config.GetPositionData(strPosition, TargetType.Offset);
                targetPosition = basePosition + offsetPosition;

                //  이 코드를 살려야 하나...
                //if (m_Owner.MC_Func.MC_GetEncPos((int)LoaderParameter.AxisAjinEnum.TR_Z) != 0)
                //    m_Owner.MC_Func.MC_MovePosition((int)LoaderParameter.AxisAjinEnum.TR_Z, 0, parameter.dVel[(int)Loader.nAxis.TR_Z], parameter.dAcc[(int)Loader.nAxis.TR_Z], parameter.dDec[(int)Loader.nAxis.TR_Z]);

                            //ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                      //  '0' - Absolute position
                            //                            (Axis)LoaderParameter.AxisAcsEnum.StageY,
                            //                             targetPosition.Y);                           //  Target position

                            //ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                      //  '0' - Absolute position
                            //                            (Axis)LoaderParameter.AxisAcsEnum.StageX,
                            //                             targetPosition.X);                           //  Target position

                //  이 코드를 살려야 하나...
                //m_Owner.MC_Func.MC_MovePosition((int)LoaderParameter.AxisAjinEnum.TR_Z, targetPosition.TR_Z, parameter.dVel[(int)Loader.nAxis.TR_Z], parameter.dAcc[(int)Loader.nAxis.TR_Z], parameter.dDec[(int)Loader.nAxis.TR_Z]);

                            //Task<int> result = m_Owner.MC_Func.MC_MovePosition(m_Owner.dispenserParameter.stDispenserPosParam.nAxis[(int)DispenserAndScale.nDPAxis.X], targetPosition.X
                            //                                                , m_Owner.dispenserParameter.stDispenserPosParam.dVel[(int)DispenserAndScale.nDPAxis.X]
                            //                                                , m_Owner.dispenserParameter.stDispenserPosParam.dAcc[(int)DispenserAndScale.nDPAxis.X]
                            //                                                , m_Owner.dispenserParameter.stDispenserPosParam.dDec[(int)DispenserAndScale.nDPAxis.X]);

                            //double dPosition = m_Owner.Config.GetCapPosition(strPosition, TargetType.Base);
                            //Task<int> result = m_Owner.BeginMoveCapPosition(dPosition); // 참고: 무빙, 프로그래스 바 연동
                            //                ProgressForm progressForm = new ProgressForm("Stage", "Stage Moving...", result);
                            //                progressForm.ShowDialog();
            }
            else
            {

            }
        }
        #endregion
    }
}
