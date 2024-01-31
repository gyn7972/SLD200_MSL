using QMC.Common;
using QMC.Common.Hmi;
using QMC.Common.Vision.Tools;
using QMC.Common.VisionPart;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using QMC.Common.Motion.ACS.Motions;
using QMC.Common.Modules;

namespace CWA150SA_Onsemi300
{
    public partial class FormVisionCalibratorMaint : FormSubContentBase
    {
        #region Field
        private JogControl m_JogControl;
        private RoiListControl m_RoiListControl;
        private TrainImageControl m_TrainImageControl;
        private SearchResultControl m_SearchResultControl;
        private ModulePositionControl m_StagePositionControl;
        private AutoScaleControl m_AutoScaleControl;
        private VisionCalibrator m_Owner;
        private IlluminatorControl m_IlluminatorControl;
        private AutoFocusControl m_AutoFocusControl;
        #endregion

        #region Property
        public RoiVisionTool RoiTrain { get; set; }
        public RoiVisionTool RoiInspect { get; set; }
        public bool ConvertChecked { get; set; }
        #endregion

        #region Constructor
        public FormVisionCalibratorMaint(Part part)
            : base(FormType.Maint.ToString(), part.Name)
        {
            m_Owner = part as VisionCalibrator;
            this.RoiTrain = m_Owner.GetTrainRoi();
            this.RoiInspect = m_Owner.GetInspectRoi();
            this.m_visionImageViewer_Upper = new VisionImageViewer();
            //this.m_JogControl = new JogControl(m_Owner.XyztStage);
            this.m_JogControl = new JogControl(m_Owner.UvwzxyzStage);
            this.m_TrainImageControl = new TrainImageControl();
            this.m_RoiListControl = new RoiListControl(RoiTrain, RoiInspect, m_Owner.Camera.Resolution);
            this.m_StagePositionControl = new ModulePositionControl();
            this.m_SearchResultControl = new SearchResultControl(m_Owner);
            this.m_AutoScaleControl = new AutoScaleControl();

            InitializeComponent();
            this.m_IlluminatorControl = new IlluminatorControl(m_Owner.Recipe.IlluminationDataSet.ToList());

            this.panelContent.Visible = false;
            this.panelContent.Size = new Size();
            this.Controls.Add(this.m_visionImageViewer_Upper);
            this.Controls.Add(this.m_JogControl);
            this.Controls.Add(this.m_TrainImageControl);
            this.Controls.Add(this.m_StagePositionControl);
            this.Controls.Add(this.m_SearchResultControl);
            this.Controls.Add(this.m_AutoScaleControl);
            this.Controls.Add(this.m_RoiListControl);
            this.Controls.Add(this.m_IlluminatorControl);

            this.m_visionImageViewer_Upper.SizeMode = PictureBoxSizeMode.StretchImage;
            this.m_visionImageViewer_Upper.SuspendDisplay();
            this.m_visionImageViewer_Upper.Location = new Point(Configuration.ContentLocation.X, Configuration.ContentLocation.Y);
            this.m_visionImageViewer_Upper.Size = Configuration.MaintMinImageViewSize;
            this.m_visionImageViewer_Upper.Camera = m_Owner.Camera;

            this.m_JogControl.Location = new Point(this.m_visionImageViewer_Upper.Location.X + m_visionImageViewer_Upper.Width + Configuration.ControlGap * 2, this.m_visionImageViewer_Upper.Location.Y);

            this.m_RoiListControl.Location = new Point(this.m_visionImageViewer_Upper.Location.X, this.m_visionImageViewer_Upper.Location.Y + m_visionImageViewer_Upper.Height + Configuration.ControlGap);
            this.m_RoiListControl.roiTrainButtonClick += RoiTrainButtonClick;
            this.m_RoiListControl.roiAlignButtonClick += RoiInspectButtonClick;
            this.m_RoiListControl.roiTrainSaveButtonClick += RoiTrainSaveButtonClick;
            this.m_RoiListControl.roiAlignSaveButtonClick += RoiInspectSaveButtonClick;
            this.m_RoiListControl.roiTrainClick += RoiTrainClick;
            this.m_RoiListControl.roiAlignClick += RoiInspectClick;

            this.m_TrainImageControl.Location = new Point(this.m_RoiListControl.Location.X + this.m_RoiListControl.Width + Configuration.ControlGap, this.m_RoiListControl.Location.Y);
            this.m_TrainImageControl.SetTrainImage(m_Owner.Recipe.PatternMatchingParameters.TrainImage);
            this.m_TrainImageControl.TrainButtonClick += TrainButtonClick;

            this.m_SearchResultControl.Location = new Point(this.m_TrainImageControl.Location.X + m_TrainImageControl.Width + Configuration.ControlGap + 3, this.m_TrainImageControl.Location.Y + Configuration.ControlGap * 10);
            this.m_SearchResultControl.SearchClick += SearchResultClick;
            this.m_SearchResultControl.SetPatternMatchingData(m_Owner.Recipe.PatternMatchingParameters);

            this.m_AutoScaleControl.Location = new Point(this.m_SearchResultControl.Location.X, this.m_SearchResultControl.Location.Y + this.m_SearchResultControl.Height + Configuration.ControlGap);
            this.m_AutoScaleControl.AutoScaleButtonClick += AutoScaleButtonClick;

            this.m_StagePositionControl.Location = new Point(this.m_JogControl.Location.X + this.m_JogControl.Size.Width + Configuration.ControlGap, m_JogControl.Location.Y);
            this.m_StagePositionControl.SetPositionList(m_Owner.Config.VisionCalPositions);
            this.m_StagePositionControl.SetGroupboxName(" Vision Calibrator Position ");
            this.m_StagePositionControl.ButtonClick += PositionControlButtonClick;

            //this.m_IlluminatorControl.Location = new Point(this.m_SearchResultControl.Location.X + this.m_SearchResultControl.Size.Width+ Configuration.ControlGap, m_SearchResultControl.Location.Y);
            this.m_IlluminatorControl.Location = new Point(this.m_JogControl.Location.X + this.m_JogControl.Size.Width + Configuration.ControlGap, m_JogControl.Location.Y + m_StagePositionControl.Size.Height + Configuration.ControlGap);
            this.m_IlluminatorControl.Illuminator = m_Owner.Illuminator;
            this.m_IlluminatorControl.IlluminatorControlButton_Click += OnIlluminatorControlButtonClick;
            this.m_visionImageViewer_Upper.Camera = m_Owner.Camera;

            this.m_AutoFocusControl = new AutoFocusControl(((WaferProbeAlign)m_Owner.Owner).autoFocuser_Upper, ((WaferProbeAlign)m_Owner.Owner));
            this.m_AutoFocusControl.Location = new Point(this.m_SearchResultControl.Location.X + this.m_SearchResultControl.Width + Configuration.ControlGap, this.m_SearchResultControl.Location.Y);
            this.Controls.Add(this.m_AutoFocusControl);

            m_Owner.UpdateResult += M_Owner_UpdateResult;
            this.VisibleChanged += FormVisionCalibratorMaint_VisibleChanged;
        }
        #endregion

        #region Evnet Handler
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

        private void M_OpenButton_Click(object sender, EventArgs e)
        {
            if (m_Owner == null)
            {
                return;
            }

            if (m_Owner.UvwzxyzStage == null)
            {
                return;
            }

            //if (ACSSPiiPlusMotionBoard.Api.IsConnected == false)
            //{
            //    string address = "10.0.0.100";
            //    int port = 701;
            //    ACSSPiiPlusMotionBoard.Api.OpenCommEthernetTCP(address, port);
            //}
            //else
            //{
            //    ACSSPiiPlusMotionBoard.Api.CloseComm();
            //}
        }

        private void FormVisionCalibratorMaint_VisibleChanged(object sender, EventArgs e)
        {
            this.m_SearchResultControl.SetPatternMatchingData(m_Owner.Recipe.PatternMatchingParameters);
        }

        private void M_Owner_UpdateResult(PatternMatchingResult result)
        {
            m_visionImageViewer_Upper.ResultOverlays.Clear();
            foreach (var overlay in result.ResultOverlays)
            {
                m_visionImageViewer_Upper.ResultOverlays.Add(overlay);
            }
        }

        private void AutoScaleButtonClick()
        {
            Task<int> task = Task.Factory.StartNew(() =>
            {
                int ret = 0;
                ret = m_Owner.Work();
                BeginInvoke(new Action(() =>
                {
                    m_AutoScaleControl.UpdateScaleValue(m_Owner.Config.Scale.X, m_Owner.Config.Scale.Y);

                }));
                m_Owner.Owner.SaveConfigData();
                m_Owner.Camera.StartLive();
                return ret;
            });

            ProgressForm progressForm = new ProgressForm("VisionCalibrator", "Auto Scale...", task);
            progressForm.ShowDialog();

        }

        private void PositionControlButtonClick(ModulePositionControl.ButtonType type)
        {
            if (type == ModulePositionControl.ButtonType.Save)
            {
                string strPosition = m_StagePositionControl.SelectedPosition;
                //XyztCoordinate coordinate = new XyztCoordinate();// = m_Owner.GetCurrentPosition();
                //XyzztCoordinate coordinate = new XyzztCoordinate();// = m_Owner.GetCurrentPosition();
                UvwzxyzCoordinate coordinate = new UvwzxyzCoordinate();// = m_Owner.GetCurrentPosition();

                m_Owner.UvwzxyzStage.GetCommandPosition(ref coordinate);
                m_Owner.Config.SetCalibratorPosition(strPosition, TargetType.Base, coordinate);
                m_Owner.Owner.SaveConfigData();
            }
            else if (type == ModulePositionControl.ButtonType.Move)
            {
                string strPosition = m_StagePositionControl.SelectedPosition;
                //XyztCoordinate coordinate = m_Owner.Config.GetCalibratorPosition(strPosition, TargetType.Base);
                //XyzztCoordinate coordinate = m_Owner.Config.GetCalibratorPosition(strPosition, TargetType.Base);
                UvwzxyzCoordinate coordinate = m_Owner.Config.GetCalibratorPosition(strPosition, TargetType.Base);

                Task<int> result = m_Owner.UvwzxyzStage.BeginMovePosition(coordinate); // 참고: 무빙, 프로그래스 바 연동
                ProgressForm progressForm = new ProgressForm("Stage", "Stage Moving...", result);
                progressForm.StopProcess += ProgressForm_StopProcess;
                progressForm.ShowDialog();
            }
        }

        private void ProgressForm_StopProcess(object target)
        {
            m_Owner.Stop();
        }

        private void RoiInspectClick()
        {
            VisionCalibratorRecipe recipe = m_Owner.Recipe;
            RoiInspect.Parameter.StartLocation = recipe.InspectRoiStartLocation;
            RoiInspect.Parameter.EndLocation = recipe.InspectRoiEndLocation;

            RoiTrain.Parameter.Overlay.Visible = false;
            RoiInspect.Parameter.Overlay.Visible = true;

            PatternMatchingResult result = m_Owner.GetResult();
            m_visionImageViewer_Upper.NormalOverlays.Add(RoiInspect.Parameter.Overlay);
            foreach (var overlay in result.ResultOverlays)
            {
                m_visionImageViewer_Upper.NormalOverlays.Remove(overlay);
            }
            m_visionImageViewer_Upper.Display();
        }

        private void RoiTrainClick()
        {
            VisionCalibratorRecipe recipe = m_Owner.Recipe;
            RoiTrain.Parameter.StartLocation = recipe.TrainRoiStartLocation;
            RoiTrain.Parameter.EndLocation = recipe.TrainRoiEndLocation;

            RoiInspect.Parameter.Overlay.Visible = false;
            RoiTrain.Parameter.Overlay.Visible = true;

            PatternMatchingResult result = m_Owner.GetResult();
            m_visionImageViewer_Upper.NormalOverlays.Add(RoiTrain.Parameter.Overlay);
            foreach (var overlay in result.ResultOverlays)
            {
                m_visionImageViewer_Upper.NormalOverlays.Remove(overlay);
            }
            m_visionImageViewer_Upper.Display();
        }

        private void RoiInspectSaveButtonClick(RoiVisionTool roiVisionTool, bool bOk)
        {
            VisionCalibratorRecipe recipe = m_Owner.Recipe;
            if (!bOk)
            {
                RoiInspect.Parameter.StartLocation = recipe.InspectRoiStartLocation;
                RoiInspect.Parameter.EndLocation = recipe.InspectRoiEndLocation;
                m_visionImageViewer_Upper.Display();
                return;
            }
            RoiInspect = roiVisionTool;
            recipe.InspectRoiStartLocation = RoiInspect.Parameter.StartLocation;
            recipe.InspectRoiEndLocation = RoiInspect.Parameter.EndLocation;

            m_visionImageViewer_Upper.Display();
            if (m_JogControl != null)
            {
                m_JogControl.Show();
                m_JogControl.BringToFront();
            }
            m_visionImageViewer_Upper.Display();
            Equipment.SaveRecipe();
        }

        private void RoiTrainSaveButtonClick(RoiVisionTool roiVisionTool, bool bOk)
        {
            VisionCalibratorRecipe recipe = m_Owner.Recipe;

            if (!bOk)
            {
                RoiTrain.Parameter.StartLocation = recipe.TrainRoiStartLocation;
                RoiTrain.Parameter.EndLocation = recipe.TrainRoiEndLocation;
                RoiTrain.Parameter.Overlay.Visible = false;
                m_visionImageViewer_Upper.Display();
                return;
            }

            RoiTrain = roiVisionTool;
            RoiTrain.Parameter.Overlay.Visible = false;

            recipe.TrainRoiStartLocation = RoiTrain.Parameter.StartLocation;
            recipe.TrainRoiEndLocation = RoiTrain.Parameter.EndLocation;

            m_visionImageViewer_Upper.Display();
            if (m_JogControl != null)
            {
                m_JogControl.Show();
                m_JogControl.BringToFront();
            }
            m_visionImageViewer_Upper.Display();
            Equipment.SaveRecipe();
        }

        private void RoiTrainButtonClick(RoiVisionTool roiVisionTool)
        {
            if (m_Owner != null)
            {
                RoiTrain.Parameter.CenterLocation = roiVisionTool.Parameter.CenterLocation;
                RoiTrain.Parameter.Size = roiVisionTool.Parameter.Size;

                m_visionImageViewer_Upper.NormalOverlays.Add(RoiTrain.Parameter.Overlay);
                RoiTrain.Parameter.Overlay.Visible = true;
                m_visionImageViewer_Upper.Display();
            }
        }

        public void RoiInspectButtonClick(RoiVisionTool roiVisionTool)
        {
            if (m_Owner != null)
            {
                RoiInspect.Parameter.CenterLocation = roiVisionTool.Parameter.CenterLocation;
                RoiInspect.Parameter.Size = roiVisionTool.Parameter.Size;

                m_visionImageViewer_Upper.NormalOverlays.Add(RoiInspect.Parameter.Overlay);
                RoiInspect.Parameter.Overlay.Visible = true;
                m_visionImageViewer_Upper.Display();
            }
        }

        private void TrainButtonClick(TrainImageControl.ButtonType type)
        {
            m_Owner.Train();
            m_TrainImageControl.SetTrainImage(m_Owner.TrainImage);
        }

        private void SearchResultClick(Control control)
        {
            m_SearchResultControl = control as SearchResultControl;

            PatternMatchingResult result = m_Owner.GetResult();
            if (result != null)
            {
                m_visionImageViewer_Upper.ResultOverlays.Clear();
            }
            result = m_Owner.Search();
            if (result != null)
            {
                foreach (var overlay in result.ResultOverlays)
                {
                    m_visionImageViewer_Upper.ResultOverlays.Add(overlay);
                    overlay.Visible = true;
                }
            }
            m_visionImageViewer_Upper.Display();
            if (result != null && result.Values.Count > 0)
            { //? 스케일값 제대로 넣어줘야함
                PointD converted = new PointD((result.Values[0].X - this.m_Owner.Camera.Resolution.Width / 2) * this.m_Owner.Config.Scale.X * (this.m_Owner.Config.Scale.InvertedX ? 1 : -1),
                                                  (result.Values[0].Y - this.m_Owner.Camera.Resolution.Height / 2) * this.m_Owner.Config.Scale.Y * (this.m_Owner.Config.Scale.InvertedY ? 1 : -1));
                if (this.m_SearchResultControl.IsPixel == true)
                {
                    m_SearchResultControl.UpdataPositionData(result.Values[0].X, result.Values[0].Y, result.Values[0].R);
                }
                else
                {
                    m_SearchResultControl.UpdataPositionData(converted.X, converted.Y, result.Values[0].R);
                }
            }

            m_Owner.Camera.StartLive();
        }

        private void OnIlluminatorControlButtonClick(IlluminatorControl.ButtonType type)
        {
            if (type == IlluminatorControl.ButtonType.Save)
            {
                Module module = m_Owner.Owner as Module;
                Equipment.UpdateRecipeData();
                Equipment.SaveRecipe();
            }
            else if (type == IlluminatorControl.ButtonType.AllOff)
            {

            }
            else { }
        }
        #endregion
    }
}