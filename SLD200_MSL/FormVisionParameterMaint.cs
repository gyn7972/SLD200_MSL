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
    public partial class FormVisionParameterMaint : FormSubContentBase
    {
        #region Field
        public Module m_Module;
        protected ModuleStateControl m_ModuleStateControl;
        public ModulePositionControl m_VisionPosControl;
        public JogControl m_JogControl;
        //public AutoFocusControl m_AutoFocusControl;

        protected Vision m_Owner;
        #endregion

        #region Constructor
        public FormVisionParameterMaint(Module module)
             : base(FormType.Maint.ToString(), module.Name)
        {
            m_Module = module;

            int nGap = 20;

            m_Owner = module as Vision;

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

            m_VisionPosControl = new ModulePositionControl();
            //m_WorkStagePosControl.Location = new Point(m_visionImageViewer.Location.X + m_visionImageViewer.Size.Width + nGap, Configuration.ContentLocation.Y + nGap);
            m_VisionPosControl.Location = new Point(m_JogControl.Location.X, m_JogControl.Location.Y + m_JogControl.Size.Height + Configuration.ControlGap * 2);
            //m_DispenserPosControl.SetPositionList(m_Owner.Config.Positions);
            m_VisionPosControl.SetPositionList(m_Owner.visionParameter.Config.ConfigVisionPositions);
            //m_WorkStagePosControl.SetPositionList(m_Owner.Config.Positions);
            m_VisionPosControl.SetGroupboxName(" Vision Position Control ");
            this.Controls.Add(m_VisionPosControl);

            //  여기는 Auto Focus 빼자
            //m_AutoFocusControl = new AutoFocusControl(m_Owner.autoFocuser, m_Owner);
            //m_AutoFocusControl.Location = new Point(m_WorkStagePosControl.Location.X + m_WorkStagePosControl.Size.Width + Configuration.ControlGap, m_WorkStagePosControl.Location.Y);
            //this.Controls.Add(m_AutoFocusControl);

            //m_GripperControl = new GripperContorl(module);
            //m_GripperControl.Location = new Point(m_ModulePosControl.Location.X + m_ModulePosControl.Size.Width + 5, m_JogControl.Size.Height + 20);
            //m_GripperControl.SetGroupBoxName(" Gripper ");

            m_visionImageViewer_Upper.Camera = m_Owner.Camera_HighRes;
            m_visionImageViewer_Lower.Camera = m_Owner.Camera_LowRes;
            m_VisionPosControl.ButtonClick += OnClickVisionPosControlButton;
        }
        #endregion

        #region Event Handler
        private void OnClickVisionPosControlButton(ModulePositionControl.ButtonType type)
        {
            if (type == ModulePositionControl.ButtonType.Save)
            {
                //선택된 Position 확인.
                string strPosition = m_VisionPosControl.SelectedPosition;
                VisionParameter.stVisionParam parameter;
                parameter = m_Owner.visionParameter.GetPositionInformation(strPosition);

                ////현재 X의 위치를 가져옴.                
                //double xPosition = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)WorkStageParameter.AxisAcsEnum.StageX);            //  현재 X 위치
                ////현재 Y의 위치를 가져옴.
                //double yPosition = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)WorkStageParameter.AxisAcsEnum.StageY);            //  현재 Y 위치

                //  현재 X 의 위치를 가져옴.
                double xPosition = m_Owner.MC_Func.MC_GetEncPos((int)WorkStageParameter.AxisAjinEnum.X);
                //  현재 Y 의 위치를 가져옴.
                double yPosition = m_Owner.MC_Func.MC_GetEncPos((int)WorkStageParameter.AxisAjinEnum.Y);
                //  현재 Z 의 위치를 가져옴.
                double zPosition = m_Owner.MC_Func.MC_GetEncPos((int)WorkStageParameter.AxisAjinEnum.Z);

                double maskYPosition = 0.0;
                if (Equipment.Machine_LaserType_CO2)
                {
                    //  현재 MASK Y 의 위치를 가져옴.
                    maskYPosition = m_Owner.MC_Func.MC_GetEncPos((int)WorkStageParameter.AxisAjinEnum.MASK_Y);
                }
                else
                {
                    maskYPosition = 0.0;
                }

                xPosition = Math.Round(xPosition, 4);
                yPosition = Math.Round(yPosition, 4);
                zPosition = Math.Round(zPosition, 4);
                maskYPosition = Math.Round(maskYPosition, 4);

                XyzyCoordinate savePosition = new XyzyCoordinate(xPosition, yPosition, zPosition, maskYPosition);

                m_Owner.Config.SetPositionData(strPosition, TargetType.Base, savePosition);

                Vision vision = m_Owner.Owner as Vision;
                if (vision != null)
                {
                    vision.SaveConfigData();
                }

                //  갱신
                m_VisionPosControl.SetPositionList(m_Owner.visionParameter.Config.ConfigVisionPositions);
            }
            else if (type == ModulePositionControl.ButtonType.Move)
            {
                string strPosition = m_VisionPosControl.SelectedPosition;
                VisionParameter.stVisionParam parameter;
                XyzyCoordinate basePosition = new XyzyCoordinate();
                XyzyCoordinate offsetPosition = new XyzyCoordinate();
                XyzyCoordinate targetPosition = new XyzyCoordinate();

                parameter = m_Owner.visionParameter.GetPositionInformation(strPosition);

                //m_Owner.MC_Func.MC_MovePosition
                basePosition = m_Owner.Config.GetPositionData(strPosition, TargetType.Base);
                offsetPosition = m_Owner.Config.GetPositionData(strPosition, TargetType.Offset);
                targetPosition = basePosition + offsetPosition;

                if (m_Owner.MC_Func.MC_GetEncPos((int)WorkStageParameter.AxisAjinEnum.Z) != 0)
                    m_Owner.MC_Func.MC_MovePosition((int)WorkStageParameter.AxisAjinEnum.Z, 0, parameter.dVel[(int)WorkStage.nAxis.Z], parameter.dAcc[(int)WorkStage.nAxis.Z], parameter.dDec[(int)WorkStage.nAxis.Z]);

                //ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                      //  '0' - Absolute position
                //                            (Axis)WorkStageParameter.AxisAcsEnum.StageY,
                //                             targetPosition.Y);                           //  Target position

                //ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                      //  '0' - Absolute position
                //                            (Axis)WorkStageParameter.AxisAcsEnum.StageX,
                //                             targetPosition.X);                           //  Target position

                m_Owner.MC_Func.MC_MovePosition((int)WorkStageParameter.AxisAjinEnum.Z, targetPosition.Z, parameter.dVel[(int)WorkStage.nAxis.Z], parameter.dAcc[(int)WorkStage.nAxis.Z], parameter.dDec[(int)WorkStage.nAxis.Z]);

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
