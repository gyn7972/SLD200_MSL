using QMC.Common;
using QMC.Common.Hmi;
using QMC.Common.Modules;
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

namespace CWA150SA_Onsemi300
{
    public partial class FormTwoPointAlignerMaint : FormSubContentBase
    {
        protected TwoPointAligner m_Owner;
        private SearchResultControl m_SearchResultControl;
        private IlluminatorControl m_IlluminatorControl;
        private RoiListControl m_RoiListControl;
        private TrainImageControl m_TrainImageControl;
        private ResultAlignControl m_ResultAlignControl;
        private JogControl m_JogControl;
        
        private RoiVisionTool m_Train;
        private RoiVisionTool m_Inspect;

        public FormTwoPointAlignerMaint() : this(null)
        {

        }
        public FormTwoPointAlignerMaint(Part part)
            : base(FormType.Maint.ToString(), part.Name)
        {
            m_Owner = part as TwoPointAligner;
            InitializeComponent();
            this.panelContent.Visible = false;
            this.panelContent.Size = new Size();

            m_Train = m_Owner.GetTrainRoi();
            m_Inspect = m_Owner.GetInspectRoi();

            this.m_visionImageViewer_Upper = new VisionImageViewer();
            this.Controls.Add(this.m_visionImageViewer_Upper);
            this.m_visionImageViewer_Upper.SizeMode = PictureBoxSizeMode.CenterImage;
            this.m_visionImageViewer_Upper.SuspendDisplay();
            this.m_visionImageViewer_Upper.Location = new Point(Configuration.ContentLocation.X, Configuration.ContentLocation.Y);
            this.m_visionImageViewer_Upper.Size = new Size(Configuration.MaintImageViewSize.Width, Configuration.MaintImageViewSize.Height);
            this.m_visionImageViewer_Upper.Camera = m_Owner.Camera;

            m_Owner.TrainImage.Load(@"D:\Document\Projects\MDT-400P\[MDT-400P]211207_Red Chip 취득이미지\SMTLINE\Train.bmp", QMC.Common.Vision.VisionImage.FileFilter.bmp);
            m_Owner.TestImage.Load(@"D:\Document\Projects\MDT-400P\[MDT-400P]211207_Red Chip 취득이미지\SMTLINE\Img.bmp", QMC.Common.Vision.VisionImage.FileFilter.bmp);
            //m_Owner.TestImage.Load(@"D:\Document\Projects\MDT-400P\[MDT-400P]211207_Red Chip 취득이미지\1.SourceFinder_RealTimeScan Image\11_43_38_729464_1233294090.bmp", QMC.Common.Vision.VisionImage.FileFilter.bmp);

            m_Owner.Simulated = false;
            m_visionImageViewer_Upper.Simulated = true;
            m_visionImageViewer_Upper.InputImage = m_Owner.TestImage;

            if (m_Owner.Owner is WaferProbeAlign)
            {
                m_JogControl = new JogControl(m_Owner.waferProbeAlignParameter);
            }
            else
            {
                m_JogControl = new JogControl(null);
            }            

            this.Controls.Add(m_JogControl);
            this.m_JogControl.Location = new Point(m_visionImageViewer_Upper.Location.X, m_visionImageViewer_Upper.Location.Y + m_visionImageViewer_Upper.Height + Configuration.ControlGap);


            this.m_RoiListControl = new RoiListControl(m_Train, m_Inspect, m_Owner.Camera.Resolution);
            this.Controls.Add(this.m_RoiListControl);
            this.m_RoiListControl.Location = new Point(this.m_visionImageViewer_Upper.Location.X + m_visionImageViewer_Upper.Width + Configuration.ControlGap, this.m_visionImageViewer_Upper.Location.Y);
            this.m_RoiListControl.roiTrainButtonClick += RoiTrainButtonClick;
            this.m_RoiListControl.roiAlignButtonClick += RoiInspectButtonClick;
            this.m_RoiListControl.roiTrainSaveButtonClick += RoiTrainSaveButtonClick;
            this.m_RoiListControl.roiAlignSaveButtonClick += RoiInspectSaveButtonClick;
            this.m_RoiListControl.roiTrainClick += RoiTrainClick;
            this.m_RoiListControl.roiAlignClick += RoiInspectClick;

            m_TrainImageControl = new TrainImageControl();
            this.Controls.Add(this.m_TrainImageControl);
            this.m_TrainImageControl.Location = new Point(this.m_RoiListControl.Location.X + this.m_RoiListControl.Width + Configuration.ControlGap, this.m_RoiListControl.Location.Y);
            this.m_TrainImageControl.SetTrainImage(m_Owner.Recipe.PatternMatchingParameter.TrainImage);
            this.m_TrainImageControl.TrainButtonClick += TrainButtonClick;

            m_SearchResultControl = new SearchResultControl(m_Owner);
            this.Controls.Add(this.m_SearchResultControl);
            this.m_SearchResultControl.Location = new Point(m_TrainImageControl.Location.X , m_TrainImageControl.Location.Y + m_TrainImageControl.Height + Configuration.ControlGap);
            this.m_SearchResultControl.SearchClick += SearchResultControl_SearchClick; ;
            this.m_SearchResultControl.SetPatternMatchingData(m_Owner.Recipe.PatternMatchingParameter);

            m_ResultAlignControl = new ResultAlignControl();
            this.Controls.Add(this.m_ResultAlignControl);
            m_ResultAlignControl.AlignButtonClick += AlignButtonClick;
            m_ResultAlignControl.Location = new Point(m_SearchResultControl.Location.X, m_SearchResultControl.Location.Y + m_SearchResultControl.Height + Configuration.ControlGap);

            this.m_IlluminatorControl = new IlluminatorControl(m_Owner.Recipe.IlluminationDataSet.ToList());
            this.Controls.Add(this.m_IlluminatorControl);
            this.m_IlluminatorControl.Location = new Point(m_TrainImageControl.Location.X + m_TrainImageControl.Width + Configuration.ControlGap, m_TrainImageControl.Location.Y);
            this.m_IlluminatorControl.Illuminator = m_Owner.Illuminator;
            this.m_IlluminatorControl.IlluminatorControlButton_Click += OnIlluminatorControlButtonClick;
        }

        private void AlignButtonClick()
        {
            Task<int> task = m_Owner.BeginWork();
            ProgressForm progressForm = new ProgressForm("", "", task);
            progressForm.StopProcess += AlignStopProcess;
            if (DialogResult.OK == progressForm.ShowDialog())
            {
                m_ResultAlignControl.AlignResult = m_Owner.Result.T;
            }
            m_Owner.Camera.StartLive();
        }

        private void AlignStopProcess(object target)
        {
            m_Owner.Stop();
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

        private void SearchResultControl_SearchClick(Control control)
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

                //PointD converted = new PointD((result.Values[0].X - this.m_Owner.Camera.Resolution.Width / 2) * this.m_Owner.Scale.X * (this.m_Owner.Scale.InvertedX ? 1 : -1),
                //                                  (result.Values[0].Y - this.m_Owner.Camera.Resolution.Height / 2) * this.m_Owner.Scale.Y * (this.m_Owner.Scale.InvertedY ? 1 : -1));

                m_SearchResultControl.UpdataPositionData(result.Values[0].X, result.Values[0].Y, result.Values[0].R);
            }

            m_Owner.Camera.StartLive();
        }

        private void RoiTrainButtonClick(RoiVisionTool roiVisionTool)
        {
            if (m_Owner != null)
            {
                m_Train.Parameter.CenterLocation = roiVisionTool.Parameter.CenterLocation;
                m_Train.Parameter.Size = roiVisionTool.Parameter.Size;

                m_visionImageViewer_Upper.NormalOverlays.Add(m_Train.Parameter.Overlay);
                m_Train.Parameter.Overlay.Visible = true;
                m_visionImageViewer_Upper.Display();
            }
        }

        public void RoiInspectButtonClick(RoiVisionTool roiVisionTool)
        {
            if (m_Owner != null)
            {
                m_Inspect.Parameter.CenterLocation = roiVisionTool.Parameter.CenterLocation;
                m_Inspect.Parameter.Size = roiVisionTool.Parameter.Size;

                m_visionImageViewer_Upper.NormalOverlays.Add(m_Inspect.Parameter.Overlay);
                m_Inspect.Parameter.Overlay.Visible = true;
                m_visionImageViewer_Upper.Display();
            }
        }

        private void RoiInspectClick()
        {
            TwoPointAlignerRecipe recipe = m_Owner.Recipe;
            m_Inspect.Parameter.StartLocation = recipe.InspectRoiStartLocation;
            m_Inspect.Parameter.EndLocation = recipe.InspectRoiEndLocation;

            m_Train.Parameter.Overlay.Visible = false;
            m_Inspect.Parameter.Overlay.Visible = true;

            PatternMatchingResult result = m_Owner.GetResult();
            m_visionImageViewer_Upper.NormalOverlays.Add(m_Inspect.Parameter.Overlay);
            foreach (var overlay in result.ResultOverlays)
            {
                m_visionImageViewer_Upper.NormalOverlays.Remove(overlay);
            }
            m_visionImageViewer_Upper.Display();
        }

        private void RoiTrainClick()
        {
            TwoPointAlignerRecipe recipe = m_Owner.Recipe;
            m_Train.Parameter.StartLocation = recipe.TrainRoiStartLocation;
            m_Train.Parameter.EndLocation = recipe.TrainRoiEndLocation;

            m_Inspect.Parameter.Overlay.Visible = false;
            m_Train.Parameter.Overlay.Visible = true;

            PatternMatchingResult result = m_Owner.GetResult();
            m_visionImageViewer_Upper.NormalOverlays.Add(m_Train.Parameter.Overlay);
            foreach (var overlay in result.ResultOverlays)
            {
                m_visionImageViewer_Upper.NormalOverlays.Remove(overlay);
            }
            m_visionImageViewer_Upper.Display();
        }

        private void RoiInspectSaveButtonClick(RoiVisionTool roiVisionTool, bool bOk)
        {
            TwoPointAlignerRecipe recipe = m_Owner.Recipe;
            if (!bOk)
            {
                m_Inspect.Parameter.StartLocation = recipe.InspectRoiStartLocation;
                m_Inspect.Parameter.EndLocation = recipe.InspectRoiEndLocation;
                m_visionImageViewer_Upper.Display();
                return;
            }
            m_Inspect = roiVisionTool;
            recipe.InspectRoiStartLocation = m_Inspect.Parameter.StartLocation;
            recipe.InspectRoiEndLocation = m_Inspect.Parameter.EndLocation;

            m_visionImageViewer_Upper.Display();
            if (m_JogControl != null)
            {
                m_JogControl.Show();
                m_JogControl.BringToFront();
            }
            if (m_TrainImageControl != null)
            {
                m_TrainImageControl.Show();
                m_TrainImageControl.BringToFront();
            }
            if (m_SearchResultControl != null)
            {
                m_SearchResultControl.Show();
                m_SearchResultControl.BringToFront();
            }
            if (m_ResultAlignControl != null)
            {
                m_ResultAlignControl.Show();
                m_ResultAlignControl.BringToFront();
            }
            m_visionImageViewer_Upper.Display();
            Equipment.SaveRecipe();
        }

        private void RoiTrainSaveButtonClick(RoiVisionTool roiVisionTool, bool bOk)
        {
            TwoPointAlignerRecipe recipe = m_Owner.Recipe;

            if (!bOk)
            {
                m_Train.Parameter.StartLocation = recipe.TrainRoiStartLocation;
                m_Train.Parameter.EndLocation = recipe.TrainRoiEndLocation;
                m_Train.Parameter.Overlay.Visible = false;
                m_visionImageViewer_Upper.Display();
                return;
            }

            m_Train = roiVisionTool;
            m_Train.Parameter.Overlay.Visible = false;

            recipe.TrainRoiStartLocation = m_Train.Parameter.StartLocation;
            recipe.TrainRoiEndLocation = m_Train.Parameter.EndLocation;

            m_visionImageViewer_Upper.Display();
            if (m_JogControl != null)
            {
                m_JogControl.Show();
                m_JogControl.BringToFront();
            }
            if(m_TrainImageControl != null)
            {
                m_TrainImageControl.Show();
                m_TrainImageControl.BringToFront();
            }
            if(m_SearchResultControl != null)
            {
                m_SearchResultControl.Show();
                m_SearchResultControl.BringToFront();
            }
            if(m_ResultAlignControl != null)
            {
                m_ResultAlignControl.Show();
                m_ResultAlignControl.BringToFront();
            }
            m_visionImageViewer_Upper.Display();
            Equipment.SaveRecipe();
        }

        private void TrainButtonClick(TrainImageControl.ButtonType type)
        {
            m_Owner.Train();
            m_TrainImageControl.SetTrainImage(m_Owner.TrainImage);
        }
    }
}
