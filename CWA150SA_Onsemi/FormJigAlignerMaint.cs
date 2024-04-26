using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CWA150SA_Onsemi300;
using QMC.Common;
using QMC.Common.Hmi;
using QMC.Common.Modules;
using QMC.Common.Motion.ACS.Motions;
using QMC.Common.Parts;
using QMC.Common.Vision.Tools;
using QMC.Common.VisionPart;

namespace CWA150SA_Onsemi
{
    public partial class FormJigAlignerMaint : FormSubContentBase
    {
        #region Field
        private JogControl m_JogControl;
        private RoiListControl m_RoiListControl;
        private TrainImageControl m_TrainImageControl;
        private SearchResultControl m_SearchResultControl;
        private ModulePositionControl m_StagePositionControl;
        private JigAligner m_Owner;
        private AutoFocusControl m_AutoFocusControl;
        private IlluminatorControl m_IlluminatorControl;
        private JigAlignerControl m_JigAlignerResultControl;

        static WaferProbeAlign waferProbeAlign;

        private BaseButton m_TestFindFiducial;

        private BaseButton m_CameraInit;
        private BaseButton m_LiveStart;
        private BaseButton m_LiveStop;

        //private BaseButton m_ConnectButton;
        //private AutoScaleControl m_AutoScaleControl;
        #endregion

        #region Property
        public RoiVisionTool RoiTrain { get; set; }
        public RoiVisionTool RoiInspect { get; set; }
        public bool ConvertChecked { get; set; }
        #endregion

        public FormJigAlignerMaint(Part part)
            : base(FormType.Maint.ToString(), part.Name)
        {
            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;

            foreach (Module module in m_collectionModules)
            {
                if (module.Name == "WaferProbeAlign")
                {
                    waferProbeAlign = module as WaferProbeAlign;
                }
            }

            m_Owner = part as JigAligner;

            this.RoiTrain = m_Owner.GetTrainRoi();
            this.RoiInspect = m_Owner.GetInspectRoi();

            this.m_visionImageViewer_Upper = new VisionImageViewer();
            //this.m_JogControl = new JogControl(m_Owner.waferProbeAlignParameter);
            this.m_JogControl = new JogControl(m_Owner.Stage);
            this.m_TrainImageControl = new TrainImageControl();
            this.m_RoiListControl = new RoiListControl(RoiTrain, RoiInspect, m_Owner.Camera.Resolution);
            this.m_StagePositionControl = new ModulePositionControl();
            this.m_SearchResultControl = new SearchResultControl(m_Owner);

            this.m_TestFindFiducial = new BaseButton();

            this.m_CameraInit = new BaseButton();
            this.m_LiveStart = new BaseButton();
            this.m_LiveStop = new BaseButton();

            InitializeComponent();

            //this.m_IlluminatorControl = new IlluminatorControl(m_Owner.Recipe.IlluminationDataSet.ToList());

            this.panelContent.Visible = false;
            this.panelContent.Size = new Size();
            this.Controls.Add(this.m_visionImageViewer_Upper);
            this.Controls.Add(this.m_JogControl);
            this.Controls.Add(this.m_TrainImageControl);
            this.Controls.Add(this.m_StagePositionControl);
            this.Controls.Add(this.m_SearchResultControl);
            this.Controls.Add(this.m_RoiListControl);
            //this.Controls.Add(this.m_TestFindFiducial);
            //this.Controls.Add(this.m_LiveStart);

            //this.Controls.Add(this.m_AutoScaleControl);
            //this.Controls.Add(this.m_IlluminatorControl);

            this.m_visionImageViewer_Upper.SizeMode = PictureBoxSizeMode.CenterImage;
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
            this.m_TrainImageControl.SetTrainImage(m_Owner.Recipe.PatternMatchingParameter.TrainImage);
            this.m_TrainImageControl.TrainButtonClick += TrainButtonClick;

            this.m_SearchResultControl.Location = new Point(this.m_TrainImageControl.Location.X + m_TrainImageControl.Width + Configuration.ControlGap + 3, this.m_TrainImageControl.Location.Y + Configuration.ControlGap * 10);
            this.m_SearchResultControl.SearchClick += SearchResultClick;
            this.m_SearchResultControl.SetPatternMatchingData(m_Owner.Recipe.PatternMatchingParameter);

            this.m_StagePositionControl.Location = new Point(this.m_JogControl.Location.X + this.m_JogControl.Size.Width + Configuration.ControlGap, m_JogControl.Location.Y);
            this.m_StagePositionControl.SetPositionList(m_Owner.Config.AlignPositions);
            this.m_StagePositionControl.SetGroupboxName(" Jig-Aligner Position ");
            this.m_StagePositionControl.ButtonClick += PositionControlButtonClick;

            //  조명
            this.m_IlluminatorControl = new IlluminatorControl(waferProbeAlign.visionCalibrator_Upper.Recipe.IlluminationDataSet.ToList());
            this.m_IlluminatorControl.Location = new Point(this.m_StagePositionControl.Location.X, m_StagePositionControl.Location.Y + m_StagePositionControl.Size.Height + Configuration.ControlGap);
            this.m_IlluminatorControl.Illuminator = waferProbeAlign.visionCalibrator_Upper.Illuminator;
            this.m_IlluminatorControl.IlluminatorControlButton_Click += M_IlluminatorControl_IlluminatorControlButton_Click;
            this.Controls.Add(m_IlluminatorControl);

            this.m_JigAlignerResultControl = new JigAlignerControl(m_Owner);
            this.m_JigAlignerResultControl.Location = new Point(this.m_SearchResultControl.Location.X, this.m_SearchResultControl.Location.Y + this.m_SearchResultControl.Height + Configuration.ControlGap);
            this.m_JigAlignerResultControl.RunButtonClick += M_JigAlignerResultControl_RunButtonClick;
            this.m_JigAlignerResultControl.SetGroupBoxName(" Jig-Aligner Result ");
            this.Controls.Add(this.m_JigAlignerResultControl);

            this.m_AutoFocusControl = new AutoFocusControl(((WaferProbeAlign)m_Owner.Owner).autoFocuser_Upper, ((WaferProbeAlign)m_Owner.Owner));
            this.m_AutoFocusControl.Location = new Point(this.m_SearchResultControl.Location.X + this.m_SearchResultControl.Width + Configuration.ControlGap, this.m_SearchResultControl.Location.Y);
            this.Controls.Add(this.m_AutoFocusControl);

            this.m_TestFindFiducial = new BaseButton();
            this.m_TestFindFiducial.Location = new Point(this.m_AutoFocusControl.Location.X + this.m_AutoFocusControl.Width + Configuration.ControlGap, this.m_AutoFocusControl.Location.Y);
            this.m_TestFindFiducial.Size = new Size(100, 50);
            this.m_TestFindFiducial.Text = "FInd Fiducial";
            this.m_TestFindFiducial.Click += M_TestFindFiducial_Click;
            this.Controls.Add(this.m_TestFindFiducial);

            this.m_CameraInit = new BaseButton();
            this.m_CameraInit.Location = new Point(this.m_RoiListControl.Location.X, this.m_RoiListControl.Location.Y + m_RoiListControl.Height + Configuration.ControlGap * 2);
            this.m_CameraInit.Size = new Size(120, 50);
            this.m_CameraInit.Font = new System.Drawing.Font("Arial", 12.0F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.m_CameraInit.Text = "Cam Init.";
            this.m_CameraInit.Click += M_CameraInit_Click;
            this.Controls.Add(this.m_CameraInit);

            this.m_LiveStart = new BaseButton();
            this.m_LiveStart.Location = new Point(this.m_CameraInit.Location.X, this.m_CameraInit.Location.Y + m_CameraInit.Height + Configuration.ControlGap);
            this.m_LiveStart.Size = new Size(120, 50);
            this.m_LiveStart.Font = new System.Drawing.Font("Arial", 12.0F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.m_LiveStart.Text = "Live Start";
            this.m_LiveStart.Click += M_LiveStart_Click;
            this.Controls.Add(this.m_LiveStart);

            m_Owner.UpdateResult += M_Owner_UpdateResult;
            this.VisibleChanged += FormVisionCalibratorMaint_VisibleChanged;
        }

        private void M_IlluminatorControl_IlluminatorControlButton_Click(IlluminatorControl.ButtonType type)
        {
            if (type == IlluminatorControl.ButtonType.Save)
            {
                //Module module = waferProbeAlign m_Owner.Owner as Module;               

                //string m_strRecipe = "";
                //RecipeInfo m_recipeInfo = new RecipeInfo();
                //m_recipeInfo = Equipment.GetCurrentRecipe();

                //if (m_recipeInfo != null)
                //{
                //    m_strRecipe = m_recipeInfo.Name;

                //    DataManager.Instance.UpdateConfigData(waferProbeAlign); // 참고 : param save
                //    //Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                //    Equipment.SaveConfig(m_strRecipe);                                            //  2022. 06. 30.  SCH : Recipe 에 따라 Config 파라미터를 변경하기 위해 이걸로 함.
                //}
                //else
                //{
                //    DataManager.Instance.UpdateConfigData(waferProbeAlign); // 참고 : param save
                //    Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                //}

                //DataManager.Instance.ApplyConfigData(waferProbeAlign);

                ////  Config 창 데이터 갱신을 위해서
                //Equipment.m_bRedraw_FormWaferProbeAlignParameterConfig = true;

                //Equipment.UpdateRecipeData();
                //Equipment.SaveRecipe();
            }
            else if (type == IlluminatorControl.ButtonType.AllOff)
            {

            }
            else { }
        }

        private void M_CameraInit_Click(object sender, EventArgs e)
        {
            m_Owner.Camera.Initialize();
        }

        private void M_LiveStart_Click(object sender, EventArgs e)
        {
            m_Owner.Camera.StartLive();
        }

        private void M_TestFindFiducial_Click(object sender, EventArgs e)
        {
            if (m_Owner != null)
            {
                PatternMatchingResult findResult = null;
                XyCoordinate coordinate = new XyCoordinate();

                //m_Owner.Recipe.pathGenerator.PathParameter.CenterCoordinate = new XyCoordinate();
                m_Owner.FindFiducialMark(out findResult, out coordinate);

                MessageBox.Show(String.Format($"Find Result : {findResult}, Coordinate : {coordinate} "));
            }

        }

        #region Evnet Handler
        private void M_JigAlignerResultControl_RunButtonClick(out double result)
        {
            result = 0.0;

            if (m_Owner == null) return;

            //Task<int> task = m_Owner.BeginWork();
            //ProgressForm progressForm = new ProgressForm("", "", task);
            //progressForm.StopProcess += ProgressForm_StopProcess;
            //progressForm.ShowDialog();
            //if (DialogResult.OK == progressForm.ShowDialog())
            //{
            //    result = m_Owner.GetJigAlignResult();
            //   // m_ResultAlignControl.AlignResult = m_Owner.Result.T;
            //}
            //m_Owner.Camera.StartLive();

            m_Owner.SetRunStatus(Part.RunStatus.Run);
            m_Owner.Stage.SetRunStatus(Part.RunStatus.Run);

            Task<int> task = Task.Factory.StartNew(() =>
            {
                int ret = 0;

                m_Owner.Work();

                return ret;
            });

            ProgressForm progressForm = new ProgressForm("JigAligner", "Align...", task);
            progressForm.StopProcess += ProgressForm_StopProcess;
            progressForm.ShowDialog();

            m_Owner.Camera.StartLive();


            ////Work;
            //m_Owner.Work();
            //result = m_Owner.GetJigAlignResult();
        }

        private void FormVisionCalibratorMaint_VisibleChanged(object sender, EventArgs e)
        {
            this.m_SearchResultControl.SetPatternMatchingData(m_Owner.Recipe.PatternMatchingParameter);

            //  Train Image 바꿔주기
            this.m_TrainImageControl.SetTrainImage(m_Owner.Recipe.PatternMatchingParameter.TrainImage);
        }

        private void M_Owner_UpdateResult(PatternMatchingResult result)
        {
            m_visionImageViewer_Upper.ResultOverlays.Clear();

            if (result == null) return;

            foreach (var overlay in result.ResultOverlays)
            {
                m_visionImageViewer_Upper.ResultOverlays.Add(overlay);
            }
        }

        private void PositionControlButtonClick(ModulePositionControl.ButtonType type)
        {
            if (type == ModulePositionControl.ButtonType.Save)
            {
                string strPosition = m_StagePositionControl.SelectedPosition;
                XyzCoordinate coordinate = new XyzCoordinate();// = m_Owner.GetCurrentPosition();
                m_Owner.Stage.GetCommandPosition(ref coordinate);
                m_Owner.Config.SetAlignPosition(strPosition, TargetType.Base, coordinate);
                m_Owner.Owner.SaveConfigData();
            }
            else if (type == ModulePositionControl.ButtonType.Move)
            {
                string strPosition = m_StagePositionControl.SelectedPosition;
                XyzCoordinate coordinate = m_Owner.Config.GetCalibratorPosition(strPosition, TargetType.Base);

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

        private void RoiInspectClick()
        {
            JigAlignerRecipe recipe = m_Owner.Recipe;
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
            JigAlignerRecipe recipe = m_Owner.Recipe;
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
            JigAlignerRecipe recipe = m_Owner.Recipe;
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
            JigAlignerRecipe recipe = m_Owner.Recipe;

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


            if (m_Owner.Name == "JigAligner Upper" )
            {
                m_strFile = string.Format("{0}\\{1}_PAK.jpg", ConfigManager.GetPatternImagePath(), m_recipeInfo.Name);
                m_Owner.TrainImage.Save(m_strFile, QMC.Common.Vision.VisionImage.FileFilter.jpg);

                waferProbeAlign.PatternMatchingImage_Loaded_Upper = true;

                waferProbeAlign.m_bUpperCam_AlignPattern_Reset = true;
            }
            else if (m_Owner.Name == "JigAligner Lower")
            {
                m_strFile = string.Format("{0}\\{1}_Wafer.jpg", ConfigManager.GetPatternImagePath(), m_recipeInfo.Name);
                m_Owner.TrainImage.Save(m_strFile, QMC.Common.Vision.VisionImage.FileFilter.jpg);

                waferProbeAlign.PatternMatchingImage_Loaded_Lower = true;

                waferProbeAlign.m_bLowerCam_AlignPattern_Reset = true;
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
                PointD converted = new PointD((result.Values[0].X - this.m_Owner.Camera.Resolution.Width / 2) * ((WaferProbeAlign)this.m_Owner.Owner).Scale.X * (((WaferProbeAlign)this.m_Owner.Owner).Scale.InvertedX ? 1 : -1),
                                              (result.Values[0].Y - this.m_Owner.Camera.Resolution.Height / 2) * ((WaferProbeAlign)this.m_Owner.Owner).Scale.Y * (((WaferProbeAlign)this.m_Owner.Owner).Scale.InvertedY ? 1 : -1));
                m_SearchResultControl.UpdataPositionData(result.Values[0].X, result.Values[0].Y, result.Values[0].R);
            }

            m_Owner.Camera.StartLive();
        }

        //private void OnIlluminatorControlButtonClick(IlluminatorControl.ButtonType type)
        //{
        //    if (type == IlluminatorControl.ButtonType.Save)
        //    {
        //        Module module = m_Owner.Owner as Module;
        //        Equipment.UpdateRecipeData();
        //        Equipment.SaveRecipe();
        //    }
        //    else if (type == IlluminatorControl.ButtonType.AllOff)
        //    {

        //    }
        //    else { }
        //}
        #endregion
    }
}
