using ACS.SPiiPlusNET;
using QMC.Common;
using QMC.Common.Modules;
using QMC.Common.Motion.ACS.Motions;
using QMC.Common.Motion.Ajin.Motions;
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
    public partial class NeedleCalibrationJogControl : UserControl
    {
        private MotionAxis m_AxisX;
        private MotionAxis m_AxisY;
        private MotionFunction MC_Func;
        static WorkStage workStage;

        public NeedleCalibrationJogControl()
        {
            InitializeComponent();
            m_AxisX = new AjinAxlAxis();
            m_AxisY = new AjinAxlAxis();
            MC_Func = new InterpolatorMotionFunction();
            InitRadioButton();

            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;

            foreach (Module module in m_collectionModules)
            {
                //if (module.Name == "WorkStage")
                if (module.Name == "WorkStage")
                {
                    workStage = module as WorkStage;
                }
            }
        }
        private void InitRadioButton()
        {
            baseToggleButton01.UpdateToggleStatus(true);
            baseToggleButton001.UpdateToggleStatus(false);
            baseToggleButton0001.UpdateToggleStatus(false);
        }

        public void SetAxisX(MotionAxis axisX)
        {
            m_AxisX = axisX;
        }
        public void SetAxisY(MotionAxis axisY)
        {
            m_AxisY = axisY;
        }

        private double GetSelectedDistance()
        {
            double dRet = 0.0;

            if(baseToggleButton01.GetButtonStatus())
            {
                dRet = 0.1;
            }
            else if(baseToggleButton001.GetButtonStatus())
            {
                dRet = 0.01;
            }
            else if(baseToggleButton0001.GetButtonStatus())
            {
                dRet = 0.001;
            }

            return dRet;
        }

        private void buttonAxisXRight_Click(object sender, EventArgs e)
        {
            double lfVelocity = 0.0f;
            double dDistance = GetSelectedDistance();
            int nDirection = 1;
            //m_AxisX.MoveDistance(dDistance);
            if (m_AxisX == null)
            {
                return;
            }
            if (m_AxisX.Direction == MotionDirection.Backward)
                nDirection = -1;
            double dVelocity = 0;
            dVelocity = m_AxisX.Configuration.Velocity;
            //axis.MoveDistance(Step[axis] * nDirection, dVelocity, dVelocity * 5, dVelocity * 5); //참고 Step Move

            //  2022. 09. 16.  SCH : 기존 코드 (Ajin 모션)
            //  MC_Func.MC_MoveRelPosition(m_AxisX.No, dDistance * nDirection, m_AxisX.Configuration.Velocity, m_AxisX.Configuration.Acceleration, m_AxisX.Configuration.Deceleration);

            //  ACS 일 경우 ACS 모션 함수 사용해야 함.
            //lfVelocity = 50.0;     // Convert.ToDouble(tb_TestVelocity.Text.Trim());
            lfVelocity = m_AxisX.Configuration.Velocity;
            if (lfVelocity > 0) lfVelocity = lfVelocity * (-1);     // Negative direction : Using - (minus) velocity

            if (m_AxisX.Board.Configuration.BoardType == MotionBoardType.Ajin)
            {
                MC_Func.MC_JogMove(m_AxisX.No, lfVelocity, m_AxisX.Configuration.Acceleration, m_AxisX.Configuration.Deceleration);
            }
            else if (m_AxisX.Board.Configuration.BoardType == MotionBoardType.ACS)
            {
                //ACSSPiiPlusMotionBoard.Api.Jog(MotionFlags.ACSC_AMF_VELOCITY,         //  Velocity flag
                //            (Axis)WorkStageParameter.AxisAcsEnum.StageX,     //  Axis number
                //            lfVelocity);                                        //  Velocity
            }
        }

        private void buttonAxisXLeft_Click(object sender, EventArgs e)
        {
            double lfVelocity = 0.0f;
            double dDistance = GetSelectedDistance();
            int nDirection = 1;
            //m_AxisX.MoveDistance(dDistance);
            if (m_AxisX == null)
            {
                return;
            }
            if (m_AxisX.Direction == MotionDirection.Backward)
                nDirection = -1;
            double dVelocity = 0;
            dVelocity = m_AxisX.Configuration.Velocity;
            //axis.MoveDistance(Step[axis] * nDirection, dVelocity, dVelocity * 5, dVelocity * 5); //참고 Step Move

            //  2022. 09. 16.  SCH : 기존 코드 (Ajin 모션)
            //  MC_Func.MC_MoveRelPosition(m_AxisX.No, (-dDistance) * nDirection, m_AxisX.Configuration.Velocity, m_AxisX.Configuration.Acceleration, m_AxisX.Configuration.Deceleration);

            //  ACS 일 경우 ACS 모션 함수 사용해야 함.
            //lfVelocity = 50.0;     // Convert.ToDouble(tb_TestVelocity.Text.Trim());
            lfVelocity = m_AxisX.Configuration.Velocity;
            if (lfVelocity < 0) lfVelocity = lfVelocity * (-1);     // Negative direction : Using - (minus) velocity

            if (m_AxisX.Board.Configuration.BoardType == MotionBoardType.Ajin)
            {
                MC_Func.MC_JogMove(m_AxisX.No, lfVelocity, m_AxisX.Configuration.Acceleration, m_AxisX.Configuration.Deceleration);
            }
            else if (m_AxisX.Board.Configuration.BoardType == MotionBoardType.ACS)
            {
                //ACSSPiiPlusMotionBoard.Api.Jog(MotionFlags.ACSC_AMF_VELOCITY,         //  Velocity flag
                //                (Axis)WorkStageParameter.AxisAcsEnum.StageX,     //  Axis number
                //                lfVelocity);                                        //  Velocity
            }
        }

        private void buttonAxisYBwd_Click(object sender, EventArgs e)
        {
            double lfVelocity = 0.0f;
            double dDistance = GetSelectedDistance();
            int nDirection = 1;
            if (m_AxisY == null)
            {
                return;
            }
            //m_AxisX.MoveDistance(dDistance);
            if (m_AxisY.Direction == MotionDirection.Backward)
                nDirection = -1;
            double dVelocity = 0;
            dVelocity = m_AxisY.Configuration.Velocity;
            //axis.MoveDistance(Step[axis] * nDirection, dVelocity, dVelocity * 5, dVelocity * 5); //참고 Step Move

            //  2022. 09. 16.  SCH : 기존 코드 (Ajin 모션)
            //  MC_Func.MC_MoveRelPosition(m_AxisY.No, dDistance * nDirection, m_AxisY.Configuration.Velocity, m_AxisY.Configuration.Acceleration, m_AxisY.Configuration.Deceleration);

            //  ACS 일 경우 ACS 모션 함수 사용해야 함.
            //lfVelocity = 50.0;     // Convert.ToDouble(tb_TestVelocity.Text.Trim());
            lfVelocity = m_AxisY.Configuration.Velocity;
            if (lfVelocity < 0) lfVelocity = lfVelocity * (-1);     // Negative direction : Using - (minus) velocity

            if (m_AxisY.Board.Configuration.BoardType == MotionBoardType.Ajin)
            {
                MC_Func.MC_JogMove(m_AxisY.No, lfVelocity, m_AxisY.Configuration.Acceleration, m_AxisY.Configuration.Deceleration);
            }
            else if (m_AxisY.Board.Configuration.BoardType == MotionBoardType.ACS)
            {
                //ACSSPiiPlusMotionBoard.Api.Jog(MotionFlags.ACSC_AMF_VELOCITY,         //  Velocity flag
                //                (Axis)WorkStageParameter.AxisAcsEnum.StageY,     //  Axis number
                //                lfVelocity);                                        //  Velocity
            }
        }

        private void buttonAxisYFwd_Click(object sender, EventArgs e)
        {
            double lfVelocity = 0.0f;
            double dDistance = GetSelectedDistance();
            int nDirection = 1;
            if (m_AxisY == null)
            {
                return;
            }
            //m_AxisX.MoveDistance(dDistance);
            if (m_AxisY.Direction == MotionDirection.Backward)
                nDirection = -1;
            double dVelocity = 0;
            dVelocity = m_AxisY.Configuration.Velocity;
            //axis.MoveDistance(Step[axis] * nDirection, dVelocity, dVelocity * 5, dVelocity * 5); //참고 Step Move

            //  2022. 09. 16.  SCH : 기존 코드 (Ajin 모션)
            //  MC_Func.MC_MoveRelPosition(m_AxisY.No, (-dDistance) * nDirection, m_AxisY.Configuration.Velocity, m_AxisY.Configuration.Acceleration, m_AxisY.Configuration.Deceleration);

            //  ACS 일 경우 ACS 모션 함수 사용해야 함.
            lfVelocity = 50.0;     // Convert.ToDouble(tb_TestVelocity.Text.Trim());
            lfVelocity = m_AxisY.Configuration.Velocity;
            if (lfVelocity > 0) lfVelocity = lfVelocity * (-1);     // Negative direction : Using - (minus) velocity

            if (m_AxisY.Board.Configuration.BoardType == MotionBoardType.Ajin)
            {
                MC_Func.MC_JogMove(m_AxisY.No, lfVelocity, m_AxisY.Configuration.Acceleration, m_AxisY.Configuration.Deceleration);
            }
            else if (m_AxisY.Board.Configuration.BoardType == MotionBoardType.ACS)
            {
                //ACSSPiiPlusMotionBoard.Api.Jog(MotionFlags.ACSC_AMF_VELOCITY,         //  Velocity flag
                //                (Axis)WorkStageParameter.AxisAcsEnum.StageY,     //  Axis number
                //                lfVelocity);                                        //  Velocity
            }
        }

        public void UpdateState()
        {
            
        }

        private void baseToggleButton01_Click(object sender, EventArgs e)
        {
            bool bOn = baseToggleButton01.GetButtonStatus();
            if(!bOn)
            {
                baseToggleButton01.UpdateToggleStatus(true);
                baseToggleButton001.UpdateToggleStatus(false);
                baseToggleButton0001.UpdateToggleStatus(false);
            }
        }

        private void baseToggleButton001_Click(object sender, EventArgs e)
        {
            bool bOn = baseToggleButton001.GetButtonStatus();
            if (!bOn)
            {
                baseToggleButton01.UpdateToggleStatus(false);
                baseToggleButton001.UpdateToggleStatus(true);
                baseToggleButton0001.UpdateToggleStatus(false);
            }
        }

        private void baseToggleButton0001_Click(object sender, EventArgs e)
        {
            bool bOn = baseToggleButton0001.GetButtonStatus();
            if (!bOn)
            {
                baseToggleButton01.UpdateToggleStatus(false);
                baseToggleButton001.UpdateToggleStatus(false);
                baseToggleButton0001.UpdateToggleStatus(true);
            }
        }
    }
}
