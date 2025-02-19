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
    public partial class FormLoaderParameterMaint : FormSubContentBase
    {
        #region Field
        public Module m_Module;
        protected ModuleStateControl m_ModuleStateControl;
        public ModulePositionControl m_LoaderPosControl;
        public JogControl m_JogControl;
        //public AutoFocusControl m_AutoFocusControl;

        protected Loader m_Owner;
        #endregion

        #region Constructor
        public FormLoaderParameterMaint(Module module)
             : base(FormType.Maint.ToString(), module.Name)
        {
            m_Module = module;

            int nGap = 20;

            m_Owner = module as Loader;

            if (m_Owner == null)
            {
                return;
            }

            InitializeComponent();

            this.panelContent.Visible = false;
            this.panelContent.Size = new Size();

            m_ModuleStateControl = new ModuleStateControl(m_Owner);
            m_visionImageViewer_Upper = new VisionImageViewer();
            m_visionImageViewer_Lower = new VisionImageViewer();

            baseLabel_Upper = new BaseLabel();
            baseLabel_Lower = new BaseLabel();

            m_ModuleStateControl.Location = new Point(Configuration.ContentLocation.X, Configuration.ContentLocation.Y);
            m_ModuleStateControl.Size = new System.Drawing.Size(Configuration.MotorStatusControlFlowPanelSize.Width, Configuration.ContentSize.Height);
            this.Controls.Add(this.m_ModuleStateControl);

            m_visionImageViewer_Upper.SizeMode = PictureBoxSizeMode.CenterImage;
            m_visionImageViewer_Upper.SuspendDisplay();
            m_visionImageViewer_Upper.Location = new Point(m_ModuleStateControl.Location.X + m_ModuleStateControl.Width + Configuration.ControlGap, m_ModuleStateControl.Location.Y);
            m_visionImageViewer_Upper.Size = Configuration.VisionImageViewerSize;
            this.Controls.Add(m_visionImageViewer_Upper);

            m_visionImageViewer_Lower.SizeMode = PictureBoxSizeMode.CenterImage;
            m_visionImageViewer_Lower.SuspendDisplay();
            m_visionImageViewer_Lower.Location = new Point(m_visionImageViewer_Upper.Location.X, m_visionImageViewer_Upper.Location.Y + m_visionImageViewer_Upper.Height + 10);
            m_visionImageViewer_Lower.Size = Configuration.VisionImageViewerSize;
            this.Controls.Add(m_visionImageViewer_Lower);

            // 
            // baseLabel_HighRes
            // 
            this.baseLabel_Upper = new BaseLabel();
            this.baseLabel_Upper.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.baseLabel_Upper.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_Upper.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_Upper.ForeColor = System.Drawing.Color.Red;
            this.baseLabel_Upper.Location = new System.Drawing.Point(m_visionImageViewer_Upper.Location.X + m_visionImageViewer_Upper.Size.Width, m_visionImageViewer_Upper.Location.Y);
            this.baseLabel_Upper.Name = "baseLabel_Upper";
            this.baseLabel_Upper.Size = new System.Drawing.Size(58, 40);
            this.baseLabel_Upper.TabIndex = 89;
            this.baseLabel_Upper.Text = "Upper";
            this.baseLabel_Upper.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Controls.Add(baseLabel_Upper);

            // 
            // baseLabel_LowRes
            // 
            this.baseLabel_Lower = new BaseLabel();
            this.baseLabel_Lower.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.baseLabel_Lower.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_Lower.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_Lower.ForeColor = System.Drawing.Color.Red;
            this.baseLabel_Lower.Location = new System.Drawing.Point(m_visionImageViewer_Lower.Location.X + m_visionImageViewer_Lower.Size.Width, m_visionImageViewer_Lower.Location.Y);
            this.baseLabel_Lower.Name = "baseLabel_Lower";
            this.baseLabel_Lower.Size = new System.Drawing.Size(58, 40);
            this.baseLabel_Lower.TabIndex = 89;
            this.baseLabel_Lower.Text = "Lower";
            this.baseLabel_Lower.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Controls.Add(baseLabel_Lower);

            //m_JogControl = new JogControl(m_Owner);
            //m_JogControl = new JogControl(m_Owner.workStageParameter);
            m_JogControl = new JogControl(m_Owner.Stage);
            //m_JogControl.Location = new Point(m_WorkStagePosControl.Location.X + m_WorkStagePosControl.Size.Width + nGap, Configuration.ContentLocation.Y + nGap);
            //m_JogControl.Location = new Point(m_visionImageViewer.Location.X + m_visionImageViewer.Size.Width + nGap, Configuration.ContentLocation.Y/* + nGap*/);
            m_JogControl.Location = new Point(baseLabel_Upper.Location.X + baseLabel_Upper.Size.Width + nGap, Configuration.ContentLocation.Y/* + nGap*/);
            this.Controls.Add(m_JogControl);

            m_LoaderPosControl = new ModulePositionControl();
            //m_WorkStagePosControl.Location = new Point(m_visionImageViewer.Location.X + m_visionImageViewer.Size.Width + nGap, Configuration.ContentLocation.Y + nGap);
            m_LoaderPosControl.Location = new Point(m_JogControl.Location.X, m_JogControl.Location.Y + m_JogControl.Size.Height + Configuration.ControlGap * 2);
            //m_DispenserPosControl.SetPositionList(m_Owner.Config.Positions);
            m_LoaderPosControl.SetPositionList(m_Owner.loaderParameter.Config.ConfigLoaderPositions);
            //m_WorkStagePosControl.SetPositionList(m_Owner.Config.Positions);
            m_LoaderPosControl.SetGroupboxName(" Loader Position Control ");
            this.Controls.Add(m_LoaderPosControl);

            //  여기는 Auto Focus 빼자
            //m_AutoFocusControl = new AutoFocusControl(m_Owner.autoFocuser, m_Owner);
            //m_AutoFocusControl.Location = new Point(m_WorkStagePosControl.Location.X + m_WorkStagePosControl.Size.Width + Configuration.ControlGap, m_WorkStagePosControl.Location.Y);
            //this.Controls.Add(m_AutoFocusControl);

            //m_GripperControl = new GripperContorl(module);
            //m_GripperControl.Location = new Point(m_ModulePosControl.Location.X + m_ModulePosControl.Size.Width + 5, m_JogControl.Size.Height + 20);
            //m_GripperControl.SetGroupBoxName(" Gripper ");

            m_LoaderPosControl.ButtonClick += OnClickLoaderPosControlButton;
        }
        #endregion

        #region Event Handler
        private void OnClickLoaderPosControlButton(ModulePositionControl.ButtonType type)
        {
            if (type == ModulePositionControl.ButtonType.Save)
            {
                //선택된 Position 확인.
                string strPosition = m_LoaderPosControl.SelectedPosition;
                LoaderParameter.stLoaderParam parameter;
                parameter = m_Owner.loaderParameter.GetPositionInformation(strPosition);

                ////현재 X의 위치를 가져옴.                
                //double xPosition = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)WorkStageParameter.AxisAcsEnum.StageX);            //  현재 X 위치
                ////현재 Y의 위치를 가져옴.
                //double yPosition = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)WorkStageParameter.AxisAcsEnum.StageY);            //  현재 Y 위치

                //  현재 Stacker Z0 의 위치를 가져옴.
                double z0Position = m_Owner.MC_Func.MC_GetEncPos((int)LoaderParameter.AxisAjinEnum.Z0);
                //  현재 Stacker Z1 의 위치를 가져옴.
                double z1Position = m_Owner.MC_Func.MC_GetEncPos((int)LoaderParameter.AxisAjinEnum.Z1);
                //  현재 Transfer X 의 위치를 가져옴.
                double trxPosition = m_Owner.MC_Func.MC_GetEncPos((int)LoaderParameter.AxisAjinEnum.TR_X);
                //  현재 Transfer Z 의 위치를 가져옴.
                double trzPosition = m_Owner.MC_Func.MC_GetEncPos((int)LoaderParameter.AxisAjinEnum.TR_Z);
                //  현재 Align X 의 위치를 가져옴.
                double alnxPosition = m_Owner.MC_Func.MC_GetEncPos((int)LoaderParameter.AxisAjinEnum.ALN_X);
                //  현재 Align Y 의 위치를 가져옴.
                double alnyPosition = m_Owner.MC_Func.MC_GetEncPos((int)LoaderParameter.AxisAjinEnum.ALN_Y);

                z0Position = Math.Round(z0Position, 4);
                z1Position = Math.Round(z1Position, 4);
                trxPosition = Math.Round(trxPosition, 4);
                trzPosition = Math.Round(trzPosition, 4);
                alnxPosition = Math.Round(alnxPosition, 4);
                alnyPosition = Math.Round(alnyPosition, 4);

                ZzxzxyCoordinate savePosition = new ZzxzxyCoordinate(z0Position, z1Position, trxPosition, trzPosition, alnxPosition, alnyPosition);

                m_Owner.Config.SetPositionData(strPosition, TargetType.Base, savePosition);

                Loader loader = m_Owner.Owner as Loader;
                if (loader != null)
                {
                    loader.SaveConfigData();
                }

                //  갱신
                m_LoaderPosControl.SetPositionList(m_Owner.loaderParameter.Config.ConfigLoaderPositions);
            }
            else if (type == ModulePositionControl.ButtonType.Move)
            {
                string strPosition = m_LoaderPosControl.SelectedPosition;
                LoaderParameter.stLoaderParam parameter;
                ZzxzxyCoordinate basePosition = new ZzxzxyCoordinate();
                ZzxzxyCoordinate offsetPosition = new ZzxzxyCoordinate();
                ZzxzxyCoordinate targetPosition = new ZzxzxyCoordinate();

                parameter = m_Owner.loaderParameter.GetPositionInformation(strPosition);

                //                m_Owner.MC_Func.MC_MovePosition
                basePosition = m_Owner.Config.GetPositionData(strPosition, TargetType.Base);
                offsetPosition = m_Owner.Config.GetPositionData(strPosition, TargetType.Offset);
                targetPosition = basePosition + offsetPosition;

                if (m_Owner.MC_Func.MC_GetEncPos((int)LoaderParameter.AxisAjinEnum.TR_Z) != 0)
                    m_Owner.MC_Func.MC_MovePosition((int)LoaderParameter.AxisAjinEnum.TR_Z, 0, parameter.dVel[(int)Loader.nAxis.TR_Z], parameter.dAcc[(int)Loader.nAxis.TR_Z], parameter.dDec[(int)Loader.nAxis.TR_Z]);

                //ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                      //  '0' - Absolute position
                //                            (Axis)LoaderParameter.AxisAcsEnum.StageY,
                //                             targetPosition.Y);                           //  Target position

                //ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                      //  '0' - Absolute position
                //                            (Axis)LoaderParameter.AxisAcsEnum.StageX,
                //                             targetPosition.X);                           //  Target position

                m_Owner.MC_Func.MC_MovePosition((int)LoaderParameter.AxisAjinEnum.TR_Z, targetPosition.TR_Z, parameter.dVel[(int)Loader.nAxis.TR_Z], parameter.dAcc[(int)Loader.nAxis.TR_Z], parameter.dDec[(int)Loader.nAxis.TR_Z]);

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
