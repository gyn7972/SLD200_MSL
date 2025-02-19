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
using QMC.Common.Parts;

namespace SLD200_MSL
{
    public partial class FormNeedleBlockMaint : FormSubContentBase
    {
        ModulePositionControl m_CapPosControl;
        ModulePositionControl m_NeedlePosControl;
        ModulePositionControl m_NeedleBlockPosControl;
        JogControl m_JogControl;
        GripperContorl m_GripperControl;
        //ColletPositionControl m_ColletPosControl;
        protected NeedleBlock m_Owner;
        public FormNeedleBlockMaint(Part part)
             : base(FormType.Maint.ToString(), part.Name)
        {
            
            m_Owner = part as NeedleBlock;
            if(m_Owner == null)
            {
                return;
            }
            InitializeComponent();
            m_visionImageViewer_Upper = new VisionImageViewer();
            m_JogControl = new JogControl(m_Owner);
            m_CapPosControl = new ModulePositionControl();
            m_NeedlePosControl = new ModulePositionControl();
            m_NeedleBlockPosControl = new ModulePositionControl();
            //m_GripperControl = new GripperContorl(m_Owner);
            //m_ColletPosControl = new ColletPositionControl(m_Owner);

            this.panelContent.Visible = false;
            this.panelContent.Size = new Size();

            this.Controls.Add(this.m_visionImageViewer_Upper);
            m_visionImageViewer_Upper.SizeMode = PictureBoxSizeMode.CenterImage;
            m_visionImageViewer_Upper.SuspendDisplay();
            m_visionImageViewer_Upper.Location = new Point(Configuration.ContentLocation.X, Configuration.ContentLocation.Y);
            m_visionImageViewer_Upper.Size = new Size(Configuration.MaintImageViewSize.Width, Configuration.MaintImageViewSize.Height);

            m_JogControl.Location = new Point(this.m_visionImageViewer_Upper.Location.X, this.m_visionImageViewer_Upper.Location.Y + this.m_visionImageViewer_Upper.Height + Configuration.ControlGap);
            this.Controls.Add(m_JogControl);
            /*
            m_CapPosControl.Location = new Point(m_visionImageViewer.Location.X + m_visionImageViewer.Width + 10, m_visionImageViewer.Location.Y);
            m_CapPosControl.SetPositionList(m_Owner.Config.CapPositions);
            m_CapPosControl.SetGroupboxNaem("CapPosControl");
            this.Controls.Add(m_CapPosControl);
            
            
            m_NeedlePosControl.Location = new Point(m_CapPosControl.Location.X, m_CapPosControl.Location.Y + m_CapPosControl.Height + Configuration.ControlGap);
            m_NeedlePosControl.SetPositionList(m_Owner.Config.NeedlePosition);
            m_NeedlePosControl.SetGroupboxNaem("NeedlePosControl");
            this.Controls.Add(m_NeedlePosControl);

            
            m_NeedleBlockPosControl.Location = new Point(m_CapPosControl.Location.X, m_NeedlePosControl.Location.Y + m_NeedlePosControl.Height + Configuration.ControlGap);
            m_NeedleBlockPosControl.SetPositionList(m_Owner.Config.NeedleBlockPosition);
            m_NeedleBlockPosControl.SetGroupboxNaem("NeedleBlockPosControl");
            this.Controls.Add(m_NeedleBlockPosControl);
                        
            m_ColletPosControl.Location = new Point(m_CapPosControl.Location.X + m_CapPosControl.Width + Configuration.ControlGap, m_visionImageViewer.Location.Y);
            m_ColletPosControl.SetMainGroupBoxName("Move to Collet");
            m_ColletPosControl.SetSubGroupBoxName("Needle Position");
            m_ColletPosControl.SetGripperGroupBoxName("Collet");
            
            m_GripperControl.Location = new Point(m_ColletPosControl.Location.X, m_ColletPosControl.Location.Y + m_ColletPosControl.Height + Configuration.ControlGap);
            m_GripperControl.SetGroupBoxName("Cap Vaccum");
            this.Controls.Add(m_GripperControl);
            
            this.Controls.Add(m_ColletPosControl);
            m_CapPosControl.ButtonClick += OnClickCapPosControlButton;
            m_NeedlePosControl.ButtonClick += OnClickNeedlePosControlButton;
            m_NeedleBlockPosControl.ButtonClick += OnClickNeedleBlockPosControlButton;
            m_ColletPosControl.GripperButtonClick += OnClickColletControlButton;
            m_ColletPosControl.ViewButtonClick += OnClickColletViewControlButton;
            m_GripperControl.ButtonClick += OnClickVaccumControlButton;
            */
            m_visionImageViewer_Upper.Camera = m_Owner.Camera;

            this.Load += FormNeedleBlockMaint_Load;
        }

        private void FormNeedleBlockMaint_Load(object sender, EventArgs e)
        {
            m_Owner.TrainImage.Load(@"D:\Document\Projects\MDT-400P\[MDT-400P]211207_Red Chip 취득이미지\SourceFinder_TrainImage.bmp", QMC.Common.Vision.VisionImage.FileFilter.bmp);
            m_Owner.TestImage.Load(@"D:\Document\Projects\MDT-400P\[MDT-400P]211207_Red Chip 취득이미지\1.SourceFinder_RealTimeScan Image\11_43_38_729464_1233294090.bmp", QMC.Common.Vision.VisionImage.FileFilter.bmp);
            m_Owner.Simulated = true;
            this.m_visionImageViewer_Upper.Simulated = true;
            this.m_visionImageViewer_Upper.Camera = m_Owner.Camera;
            this.m_visionImageViewer_Upper.InputImage = m_Owner.TestImage;
        }

        private void OnClickCapPosControlButton(ModulePositionControl.ButtonType type)
        {
            if (type == ModulePositionControl.ButtonType.Save)
            {
                //선택된 Position 확인.
                string strPosition = m_CapPosControl.SelectedPosition;
                //현제 Cap의 위치를 가져옴.
                double dPosition =  m_Owner.GetCurrentCapPosition();
                //Position에다가 현재 위치값을 저장.
                m_Owner.Config.SetCapPosition(strPosition, TargetType.Base, dPosition);
                m_Owner.Owner.SaveConfigData();
            }
            else if (type == ModulePositionControl.ButtonType.Move)
            {
                string strPosition = m_CapPosControl.SelectedPosition;
                double dPosition = m_Owner.Config.GetCapPosition(strPosition, TargetType.Base);
                m_Owner.SetRunStatus(Part.RunStatus.Run);
                Task<int> result = m_Owner.BeginMoveCapPosition(dPosition); // 참고: 무빙, 프로그래스 바 연동
                ProgressForm progressForm = new ProgressForm("Cap", "Cap Moving...", result);
                progressForm.StopProcess += StopProcess;
                progressForm.ShowDialog();
            }
            else
            {

            }
        }

        private void StopProcess(object target)
        {
            m_Owner.Stop();
        }

        private void OnClickNeedlePosControlButton(ModulePositionControl.ButtonType type)
        {
            if (type == ModulePositionControl.ButtonType.Save)
            {
                //선택된 Position 확인.
                string strPosition = m_NeedlePosControl.SelectedPosition;
                //현제 Cap의 위치를 가져옴.
                double dPosition = m_Owner.GetCurrentNeedlePosition();
                //Position에다가 현제 위치값을 저장.
                m_Owner.Config.SetNeedlePosition(strPosition, TargetType.Base, dPosition);
                m_Owner.Owner.SaveConfigData();
            }
            else if (type == ModulePositionControl.ButtonType.Move)
            {
                string strPosition = m_NeedlePosControl.SelectedPosition;
                double dPosition = m_Owner.Config.GetNeedlePosition(strPosition, TargetType.Base);
                m_Owner.SetRunStatus(Part.RunStatus.Run);
                Task<int> result = m_Owner.BeginMoveNeedlePosition(dPosition); // 참고: 무빙, 프로그래스 바 연동
                ProgressForm progressForm = new ProgressForm("Needle", "Needle Moving...", result);
                progressForm.StopProcess += StopProcess;
                progressForm.ShowDialog();
            }
            else
            {

            }
        }
        private void OnClickNeedleBlockPosControlButton(ModulePositionControl.ButtonType type)
        {
            if (type == ModulePositionControl.ButtonType.Save)
            {
                //선택된 Position 확인.
                string strPosition = m_NeedleBlockPosControl.SelectedPosition;
                //현제 Cap의 위치를 가져옴.
                XyCoordinate coordinate = m_Owner.GetCurrentNeedleBlockPosition();
                //Position에다가 현제 위치값을 저장.
                m_Owner.Config.SetNeedleBlockPosition(strPosition, TargetType.Base, coordinate);
                m_Owner.Owner.SaveConfigData();
            }
            else if (type == ModulePositionControl.ButtonType.Move)
            {
                XyCoordinate coordinate = new XyCoordinate();
                string strPosition = m_NeedleBlockPosControl.SelectedPosition;
                coordinate = m_Owner.Config.GetNeedleBlockPosition(strPosition, TargetType.Base);
                m_Owner.SetRunStatus(Part.RunStatus.Run);
                Task<int> result = m_Owner.BeginMovePosition(coordinate); // 참고: 무빙, 프로그래스 바 연동
                ProgressForm progressForm = new ProgressForm("NeedleBlock", "NeedleBlock Moving...", result);
                progressForm.StopProcess += StopProcess;
                progressForm.ShowDialog();
            }
            else
            {

            }        
        }
        /*
        private void OnClickColletControlButton(ColletPositionControl.ColletButtonType type)
        {
            if(type == ColletPositionControl.ColletButtonType.Pos_1)
            {

            }
            else if(type == ColletPositionControl.ColletButtonType.Pos_2)
            {

            }
            else if (type == ColletPositionControl.ColletButtonType.Pos_3)
            {

            }
            else if(type == ColletPositionControl.ColletButtonType.Pos_4)
            {

            }
            else if (type == ColletPositionControl.ColletButtonType.Pos_5)
            {

            }
            else if (type == ColletPositionControl.ColletButtonType.Pos_6)
            {

            }
            else 
            {

            }
        }
        */
        /*
        private void OnClickColletViewControlButton(ColletPositionControl.ViewButtonType type)
        {
            if(type == ColletPositionControl.ViewButtonType.Show)
            {

            }
            else if(type==ColletPositionControl.ViewButtonType.Hide)
            {

            }
            else
            {

            }
        }
        */
        private bool OnClickVaccumControlButton(GripperContorl.ButtonType type)
        {
            bool bState = false;
            string strMsg = "";
            Task<int> result = Task.Factory.StartNew(() =>
            {
                int ret = 0;
                if (type == GripperContorl.ButtonType.Hold)
                {
                    strMsg = "Hold...";
                    ret = m_Owner.Hold();
                    if (!m_Owner.IsHold())
                    {
                        bState = false;
                    }
                    else
                    {
                        bState = true;
                    }
                }
                else if (type == GripperContorl.ButtonType.Release)
                {
                    strMsg = "Release...";
                    ret = m_Owner.Release();
                    if (!m_Owner.IsRelease())
                    {
                        bState = false;
                    }
                    else
                    {
                        bState = true;
                    }
                }
                return ret;
            });
            ProgressForm progressForm = new ProgressForm("Gripper", strMsg, result);
            progressForm.StopProcess += GripperStopProcess;
            progressForm.ShowDialog();

            return bState;
        }
        private void GripperStopProcess(object target)
        {
            m_Owner.SetRunStatus(Part.RunStatus.Stop);
            m_Owner.Release();
            m_Owner.Stop();
        }
    }
}
