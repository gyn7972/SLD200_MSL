using SLD200_MSL;
using QMC.Common;
using QMC.Common.Parts;
using QMC.Common.Hmi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QMC.Common.Vision.Tools;
using QMC.Common.Modules;
using static QMC.Common.Parts.ScannerCompensator;

namespace SLD200_MSL
{
    public partial class FormScannerCompensatorMaint : FormSubContentBase
    {
        #region Field
        private ScannerCompensator m_Owner;
        private JogControl m_JogControl;
        private RoiListControl m_RoiListControl;
        private TrainImageControl m_TrainImageControl;
        private SearchResultControl m_SearchResultControl;
        private AutoFocusControl m_AutoFocusControl;
        private ScannerCompensatorOffsetControl m_ScannerCompensatorOffsetControl;
        private ScannerCompensatorGeneralControl m_ScannerCompensatorGeneralControl;
        private ModulePositionControl m_GridPositionControl;
        private BlobSearchResultControl m_BlobSearchResultControl;
        #endregion

        #region Property
        public RoiVisionTool RoiTrain { get; set; }
        public RoiVisionTool RoiInspect { get; set; }
        #endregion

        #region  Constructor
        public FormScannerCompensatorMaint(Part part)
            : base(FormType.Maint.ToString(), part.Name)
        {
            this.m_Owner = part as ScannerCompensator;
            this.RoiTrain = m_Owner.GetTrainRoi();
            this.RoiInspect = m_Owner.GetInspectRoi();
            
            this.panelContent.Visible = false;
            this.panelContent.Size = new Size();
            InitializeComponent();

            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;

            foreach (Module module in m_collectionModules)
            {
                //if (module.Name == "WorkStage")
                if (module.Name == "WorkStage")
                {
                    WorkStage workStage = module as WorkStage;
                    m_Owner.Stage.Interpolator = workStage.Stage.Interpolator;
                }
            }

            this.m_visionImageViewer_Upper = new VisionImageViewer();
            this.m_visionImageViewer_Upper.SizeMode = PictureBoxSizeMode.StretchImage;
            this.m_visionImageViewer_Upper.SuspendDisplay();
            this.m_visionImageViewer_Upper.Location = new Point(Configuration.ContentLocation.X, Configuration.ContentLocation.Y);
            this.m_visionImageViewer_Upper.Size = Configuration.MaintMinImageViewSize;
            this.m_visionImageViewer_Upper.Camera = m_Owner.Camera;
            this.Controls.Add(m_visionImageViewer_Upper);

            this.m_JogControl = new JogControl(m_Owner.Stage);
            this.m_JogControl.Location = new Point(this.m_visionImageViewer_Upper.Location.X + m_visionImageViewer_Upper.Width + Configuration.ControlGap, this.m_visionImageViewer_Upper.Location.Y);
            this.Controls.Add(this.m_JogControl);

            this.m_RoiListControl = new RoiListControl(RoiTrain, RoiInspect, m_Owner.Camera.Resolution);
            this.m_RoiListControl.Location = new Point(this.m_visionImageViewer_Upper.Location.X, this.m_visionImageViewer_Upper.Location.Y + m_visionImageViewer_Upper.Height + Configuration.ControlGap);
            this.m_RoiListControl.roiTrainButtonClick += RoiTrainButtonClick;
            this.m_RoiListControl.roiAlignButtonClick += RoiInspectButtonClick;
            this.m_RoiListControl.roiTrainSaveButtonClick += RoiTrainSaveButtonClick;
            this.m_RoiListControl.roiAlignSaveButtonClick += RoiInspectSaveButtonClick;
            this.m_RoiListControl.roiTrainClick += RoiTrainClick;
            this.m_RoiListControl.roiAlignClick += RoiInspectClick;
            this.Controls.Add(this.m_RoiListControl);

            this.m_TrainImageControl = new TrainImageControl();
            this.m_TrainImageControl.Location = new Point(this.m_RoiListControl.Location.X + this.m_RoiListControl.Width, this.m_RoiListControl.Location.Y);
            if(m_Owner.Recipe != null)
            {

                this.m_TrainImageControl.SetTrainImage(m_Owner.Recipe.PatternMatchingParameter.TrainImage);
            }
            this.m_TrainImageControl.TrainButtonClick += TrainButtonClick;
            this.Controls.Add(this.m_TrainImageControl);

            this.m_SearchResultControl = new SearchResultControl(m_Owner);
            //this.m_SearchResultControl.Location = new Point(this.m_TrainImageControl.Location.X + m_TrainImageControl.Width + Configuration.ControlGap, this.m_TrainImageControl.Location.Y);
            this.m_SearchResultControl.Location = new Point(this.m_JogControl.Location.X, this.m_JogControl.Location.Y + this.m_JogControl.Height + 20);
            this.m_SearchResultControl.SearchClick += SearchResultClick;
            if (m_Owner.Recipe != null)
            {
                this.m_SearchResultControl.SetPatternMatchingData(m_Owner.Recipe.PatternMatchingParameter); 
            }
            this.Controls.Add(this.m_SearchResultControl);

            this.m_AutoFocusControl = new AutoFocusControl(((WorkStage)m_Owner.Owner).autoFocuser_HighRes, ((WorkStage)m_Owner.Owner));
            this.m_AutoFocusControl.Location = new Point(this.m_SearchResultControl.Location.X + this.m_SearchResultControl.Width + Configuration.ControlGap, this.m_SearchResultControl.Location.Y);
            this.Controls.Add(this.m_AutoFocusControl);

            this.m_ScannerCompensatorGeneralControl = new ScannerCompensatorGeneralControl(m_Owner);
            this.m_ScannerCompensatorGeneralControl.Location = new Point(this.m_JogControl.Location.X + this.m_JogControl.Width + Configuration.ControlGap, this.m_JogControl.Location.Y);
            this.m_ScannerCompensatorGeneralControl.RunButtonClick += M_ScannerCompensatorGeneralControl_RunButtonClick;
            this.Controls.Add(this.m_ScannerCompensatorGeneralControl);

            this.m_ScannerCompensatorOffsetControl = new ScannerCompensatorOffsetControl(m_Owner.Config);
            this.m_ScannerCompensatorOffsetControl.Location = new Point(this.m_ScannerCompensatorGeneralControl.Location.X, this.m_ScannerCompensatorGeneralControl.Location.Y + this.m_ScannerCompensatorGeneralControl.Height + Configuration.ControlGap);
            this.m_ScannerCompensatorOffsetControl.ButtonClick += M_ScannerCompensatorOffsetControl_ButtonClick;
            //this.Controls.Add(this.m_ScannerCompensatorOffsetControl);

            this.m_BlobSearchResultControl = new BlobSearchResultControl();
            this.m_BlobSearchResultControl.Location = new Point(this.m_ScannerCompensatorGeneralControl.Location.X, this.m_ScannerCompensatorGeneralControl.Location.Y + this.m_ScannerCompensatorGeneralControl.Height + Configuration.ControlGap);
            this.m_BlobSearchResultControl.SearchClick += M_BlobSearchResultControl_SearchClick;
            this.Controls.Add(this.m_BlobSearchResultControl);

            this.m_GridPositionControl = new ModulePositionControl();
            this.m_GridPositionControl.Location = new Point(this.m_AutoFocusControl.Location.X + this.m_AutoFocusControl.Size.Width + 20, this.m_BlobSearchResultControl.Location.Y + this.m_BlobSearchResultControl.Height + Configuration.ControlGap);
            this.m_GridPositionControl.SetGroupboxName(" Start Position ");
            this.m_GridPositionControl.SetPositionList(m_Owner.Config.GridPositions);
            this.m_GridPositionControl.ButtonClick += PositionControlButtonClick;
            this.Controls.Add(this.m_GridPositionControl);
        }

        private void M_BlobSearchResultControl_SearchClick(Control control)
        {
            m_BlobSearchResultControl = control as BlobSearchResultControl;

            BlobResult result = m_Owner.GetBlobResult();
            if (result != null)
            {
                m_visionImageViewer_Upper.ResultOverlays.Clear();
            }
            result = m_Owner.Blob();
            if (result != null)
            {
                foreach (var overlay in result.ResultOverlays)
                {
                    m_visionImageViewer_Upper.ResultOverlays.Add(overlay);
                    overlay.Visible = true;
                }
            }
            m_visionImageViewer_Upper.Display();
            if (result != null && result.PixelValues.Count > 0)
            { //? 스케일값 제대로 넣어줘야함
                if (this.m_BlobSearchResultControl.IsPixel == true)
                {
                    // [i][0] : Area, [i][1] : CenterX, [i][2] : CenterY
                    this.m_BlobSearchResultControl.UpdataPositionData(result.PixelValues[0][1].Value, result.PixelValues[0][2].Value, result.PixelValues[0][0].Value);
                }
                else
                {
                    PointD converted = new PointD((result.PixelValues[0][1].Value- this.m_Owner.Camera.Resolution.Width / 2) * ((WorkStage)this.m_Owner.Owner).Scale.X * (((WorkStage)this.m_Owner.Owner).Scale.InvertedX ? 1 : -1),
                                                  (result.PixelValues[0][2].Value - this.m_Owner.Camera.Resolution.Height / 2) * ((WorkStage)this.m_Owner.Owner).Scale.Y * (((WorkStage)this.m_Owner.Owner).Scale.InvertedY ? 1 : -1));

                    this.m_BlobSearchResultControl.UpdataPositionData(converted.X, converted.Y, result.PixelValues[0][0].Value);
                }
            }

            m_Owner.Camera.StartLive();
        }

        private void M_ScannerCompensatorOffsetControl_ButtonClick(string strName)
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
                        this.m_ScannerCompensatorOffsetControl.UpdateGridData();
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
        #endregion

        #region Event Handler
        private void ProgressForm_StopProcess(object target)
        {
            m_Owner.Stop();
        }

        private void RoiInspectClick()
        {
            ScannerCompensatorRecipe recipe = m_Owner.Recipe;
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
            ScannerCompensatorRecipe recipe = m_Owner.Recipe;
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
            ScannerCompensatorRecipe recipe = m_Owner.Recipe;
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
            m_TrainImageControl.Show();
            m_visionImageViewer_Upper.Display();
            Equipment.SaveRecipe();
        }

        private void RoiTrainSaveButtonClick(RoiVisionTool roiVisionTool, bool bOk)
        {
            ScannerCompensatorRecipe recipe = m_Owner.Recipe;

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
                m_TrainImageControl.Show();
                m_TrainImageControl.BringToFront();
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
            string m_strFile = "";
            string m_strRecipe = "";
            RecipeInfo m_recipeInfo = new RecipeInfo();
            m_recipeInfo = Equipment.GetCurrentRecipe();

            m_Owner.Train();
            m_TrainImageControl.SetTrainImage(m_Owner.TrainImage);


            //  train 할 때 레시피를 저장해줘야 정상적으로 패턴 이미지가 변경된다.
            if (m_recipeInfo != null)
            {
                Equipment.UpdateRecipeData();
            }
            Equipment.SetCurrentRecipe(m_recipeInfo);
            Equipment.SaveRecipe();
            FormManager.FireUpdateRecipeEvent();

            Equipment.ApplyRecipeData();


            //if (m_Owner.Name == "Scanner Compensator")
            if(m_Owner.Name == "ScannerCompen. (Fine)")
            {
                m_strFile = string.Format("{0}\\ScannerCal.jpg", ConfigManager.GetPatternImagePath());
                m_Owner.TrainImage.Save(m_strFile, QMC.Common.Vision.VisionImage.FileFilter.jpg);

                //workStage.PatternMatchingImage_Reticle_Loaded_HighRes = true;
                //workStage.m_bLowerCam_AlignPattern_Reset = true;
            }
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
                if (m_SearchResultControl.IsPixel == true)
                {
                    m_SearchResultControl.UpdataPositionData(result.Values[0].X, result.Values[0].Y, result.Values[0].R);
                }
                else
                {
                    PointD converted = new PointD((result.Values[0].X - this.m_Owner.Camera.Resolution.Width / 2) * ((WorkStage)this.m_Owner.Owner).Scale.X * (((WorkStage)this.m_Owner.Owner).Scale.InvertedX ? 1 : -1),
                                                  (result.Values[0].Y - this.m_Owner.Camera.Resolution.Height / 2) * ((WorkStage)this.m_Owner.Owner).Scale.Y * (((WorkStage)this.m_Owner.Owner).Scale.InvertedY ? 1 : -1));

                    m_SearchResultControl.UpdataPositionData(converted.X, converted.Y, result.Values[0].R);
                }
            }

            m_Owner.Camera.StartLive();
        }

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

        private void M_ScannerCompensatorGeneralControl_RunButtonClick()
        {
            int ret = 0;

            if (m_Owner == null)
            {
                return;
            }

            Task<int> task = Task.Factory.StartNew(() =>
            {

                m_Owner.OnWork();

                return ret;
            });

            ProgressForm progressForm = new ProgressForm("Scanner Compensator", "Compensating...", task);
            progressForm.StopProcess += ProgressForm_StopProcess;
            progressForm.ShowDialog();

            this.m_Owner.Camera.StartLive();
        }
        #endregion
    }
}
