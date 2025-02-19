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
    public partial class FormUnloaderParameterMaint : FormSubContentBase
    {
        #region Field
        public Module m_Module;
        //protected ModuleStateControl m_ModuleStateControl;
        public ModulePositionControl m_UnloaderPosControl;
        public JogControl m_JogControl;
        //public AutoFocusControl m_AutoFocusControl;

        protected Unloader m_Owner;
        #endregion

        #region Constructor
        public FormUnloaderParameterMaint(Module module)
             : base(FormType.Maint.ToString(), module.Name)
        {
            m_Module = module;

            int nGap = 20;

            m_Owner = module as Unloader;

            if (m_Owner == null)
            {
                return;
            }

            InitializeComponent();

            this.panelContent.Visible = false;
            this.panelContent.Size = new Size();

            //m_JogControl = new JogControl(m_Owner);
            //m_JogControl = new JogControl(m_Owner.workStageParameter);
            m_JogControl = new JogControl(m_Owner.Stage);
            //m_JogControl.Location = new Point(m_WorkStagePosControl.Location.X + m_WorkStagePosControl.Size.Width + nGap, Configuration.ContentLocation.Y + nGap);
            //m_JogControl.Location = new Point(m_visionImageViewer.Location.X + m_visionImageViewer.Size.Width + nGap, Configuration.ContentLocation.Y/* + nGap*/);
            m_JogControl.Location = new Point(10, 10);
            this.Controls.Add(m_JogControl);

            m_UnloaderPosControl = new ModulePositionControl();
            //m_WorkStagePosControl.Location = new Point(m_visionImageViewer.Location.X + m_visionImageViewer.Size.Width + nGap, Configuration.ContentLocation.Y + nGap);
            m_UnloaderPosControl.Location = new Point(m_JogControl.Location.X, m_JogControl.Location.Y + m_JogControl.Size.Height + Configuration.ControlGap * 2);
            //m_DispenserPosControl.SetPositionList(m_Owner.Config.Positions);
            m_UnloaderPosControl.SetPositionList(m_Owner.unloaderParameter.Config.UnloaderPositions);
            //m_WorkStagePosControl.SetPositionList(m_Owner.Config.Positions);
            m_UnloaderPosControl.SetGroupboxName(" Unloader Position Control ");
            this.Controls.Add(m_UnloaderPosControl);

            //  여기는 Auto Focus 빼자
            //m_AutoFocusControl = new AutoFocusControl(m_Owner.autoFocuser, m_Owner);
            //m_AutoFocusControl.Location = new Point(m_WorkStagePosControl.Location.X + m_WorkStagePosControl.Size.Width + Configuration.ControlGap, m_WorkStagePosControl.Location.Y);
            //this.Controls.Add(m_AutoFocusControl);

            //m_GripperControl = new GripperContorl(module);
            //m_GripperControl.Location = new Point(m_ModulePosControl.Location.X + m_ModulePosControl.Size.Width + 5, m_JogControl.Size.Height + 20);
            //m_GripperControl.SetGroupBoxName(" Gripper ");

            //m_visionImageViewer_Upper.Camera = m_Owner.Camera_HighRes;
            //m_visionImageViewer_Lower.Camera = m_Owner.Camera_LowRes;
            m_UnloaderPosControl.ButtonClick += OnClickUnloaderPosControlButton;
        }
        #endregion

        #region Event Handler
        private void OnClickUnloaderPosControlButton(ModulePositionControl.ButtonType type)
        {
            if (type == ModulePositionControl.ButtonType.Save)
            {
                //선택된 Position 확인.
                string strPosition = m_UnloaderPosControl.SelectedPosition;
                UnloaderParameter.stUnloaderParam parameter;
                parameter = m_Owner.unloaderParameter.GetPositionInformation(strPosition);

                //  현재 Stacker Z0 의 위치를 가져옴.
                double z0Position = m_Owner.MC_Func.MC_GetEncPos((int)UnloaderParameter.AxisAjinEnum.Z0);
                //  현재 Stacker Z1 의 위치를 가져옴.
                double z1Position = m_Owner.MC_Func.MC_GetEncPos((int)UnloaderParameter.AxisAjinEnum.Z1);
                //  현재 Transfer X 의 위치를 가져옴.
                double trxPosition = m_Owner.MC_Func.MC_GetEncPos((int)UnloaderParameter.AxisAjinEnum.TR_X);
                //  현재 Transfer Z 의 위치를 가져옴.
                double trzPosition = m_Owner.MC_Func.MC_GetEncPos((int)UnloaderParameter.AxisAjinEnum.TR_Z);

                z0Position = Math.Round(z0Position, 4);
                z1Position = Math.Round(z1Position, 4);
                trxPosition = Math.Round(trxPosition, 4);
                trzPosition = Math.Round(trzPosition, 4);

                ZzxzCoordinate savePosition = new ZzxzCoordinate(z0Position, z1Position, trxPosition, trzPosition);

                m_Owner.Config.SetPositionData(strPosition, TargetType.Base, savePosition);

                Unloader unloader = m_Owner.Owner as Unloader;
                if (unloader != null)
                {
                    unloader.SaveConfigData();
                }

                //  갱신
                m_UnloaderPosControl.SetPositionList(m_Owner.unloaderParameter.Config.UnloaderPositions);
            }
            else if (type == ModulePositionControl.ButtonType.Move)
            {
                string strPosition = m_UnloaderPosControl.SelectedPosition;
                UnloaderParameter.stUnloaderParam parameter;
                ZzxzCoordinate basePosition = new ZzxzCoordinate();
                ZzxzCoordinate offsetPosition = new ZzxzCoordinate();
                ZzxzCoordinate targetPosition = new ZzxzCoordinate();

                parameter = m_Owner.unloaderParameter.GetPositionInformation(strPosition);

                //                m_Owner.MC_Func.MC_MovePosition
                basePosition = m_Owner.Config.GetPositionData(strPosition, TargetType.Base);
                offsetPosition = m_Owner.Config.GetPositionData(strPosition, TargetType.Offset);
                targetPosition = basePosition + offsetPosition;

                if (m_Owner.MC_Func.MC_GetEncPos((int)UnloaderParameter.AxisAjinEnum.TR_Z) != 0)
                    m_Owner.MC_Func.MC_MovePosition((int)UnloaderParameter.AxisAjinEnum.TR_Z, 0, parameter.dVel[(int)Unloader.nAxis.TR_Z], parameter.dAcc[(int)Unloader.nAxis.TR_Z], parameter.dDec[(int)Unloader.nAxis.TR_Z]);

                //ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                      //  '0' - Absolute position
                //                            (Axis)UnloaderParameter.AxisAcsEnum.StageY,
                //                             targetPosition.Y);                           //  Target position

                //ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                      //  '0' - Absolute position
                //                            (Axis)UnloaderParameter.AxisAcsEnum.StageX,
                //                             targetPosition.X);                           //  Target position

                m_Owner.MC_Func.MC_MovePosition((int)UnloaderParameter.AxisAjinEnum.TR_Z, targetPosition.TR_Z, parameter.dVel[(int)Unloader.nAxis.TR_Z], parameter.dAcc[(int)Unloader.nAxis.TR_Z], parameter.dDec[(int)Unloader.nAxis.TR_Z]);

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
