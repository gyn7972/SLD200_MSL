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
using QMC.Common.Modules;
using QMC.Common.Parts;
using QMC.Common.Vision;
using QMC.Common.Vision.Tools;
using QMC.Common.VisionPart;
using QMC.Core;
using QMC.Common.Hmi;
using QMC.Common.UI;
using SLD200_MSL;

namespace SLD200.NewStyleForm.NewSubForm
{
    public partial class FormNewSub_Recipe_Vision : UserControl
    {
        static WorkStage workStage;
        static JigAligner Owner;

        private int nMarkType = 0; //0:Cross, 1:Circle 등
        private int nSerchType = 0; //0:PatternMatching, 1:Blob, 2:CircleA 등

        #region Property
        public RoiVisionTool RoiTrain { get; set; }
        public RoiVisionTool RoiInspect { get; set; }
        public PatternMatchingParameters PatternMatchingParameter { set; get; }
        public BlobVisionToolParameter BlobParameter { get; set; }
        #endregion

        private SLD200_MSL.RoiListControl m_RoiListControl;
        private BufferedGraphicsContext m_Context;
        private BufferedGraphics m_Graphics;

        public FormNewSub_Recipe_Vision()
        {
            InitializeComponent();

            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;
            foreach (Module module in m_collectionModules)
            {
                if (module.Name == "WorkStage")
                {
                    workStage = module as WorkStage;
                    Owner = workStage.jigAligner_LowRes;
                }
            }
            this.RoiTrain = Owner.GetTrainRoi();
            this.RoiInspect = Owner.GetInspectRoi();

            this.m_RoiListControl = new SLD200_MSL.RoiListControl(RoiTrain, RoiInspect, Owner.Camera.Resolution);
            this.m_RoiListControl.roiTrainButtonClick += RoiTrainButtonClick;
            this.m_RoiListControl.roiAlignButtonClick += RoiInspectButtonClick;
            this.m_RoiListControl.roiTrainSaveButtonClick += RoiTrainSaveButtonClick;
            this.m_RoiListControl.roiAlignSaveButtonClick += RoiInspectSaveButtonClick;

            this.ImageViewer_RecipeVision_highs = new VisionImageViewer();
            this.ImageViewer_RecipeVision_highs.SizeMode = PictureBoxSizeMode.CenterImage;
            this.ImageViewer_RecipeVision_highs.SuspendDisplay();
            this.ImageViewer_RecipeVision_highs.Camera = Owner.Camera;
            this.Controls.Add(this.ImageViewer_RecipeVision_highs);

            this.ImageViewer_RecipeVision_Rows = new VisionImageViewer();
            this.ImageViewer_RecipeVision_Rows.SizeMode = PictureBoxSizeMode.CenterImage;
            this.ImageViewer_RecipeVision_Rows.SuspendDisplay();
            this.ImageViewer_RecipeVision_Rows.Camera = Owner.Camera;
            this.Controls.Add(this.ImageViewer_RecipeVision_Rows);

        }

        private void button_RecipeVision_CameraLive_Click(object sender, EventArgs e)
        {
            if (!Owner.Camera.Opened)
            {
                var mb1 = new QMC.Common.UI.MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 카메라를 연결해야 해야 합니다.");
                return;
            }

            ImageViewer_RecipeVision_Rows.Simulated = false;
            Owner.Simulated = false;

            this.ImageViewer_RecipeVision_Rows.StartUpdateTask();
            this.ImageViewer_RecipeVision_Rows.Visible = true;

            if (Owner.Camera != null)
            {
                Owner.Camera.StartLive();
            }
        }

        private void button_RecipeVision_CameraStop_Click(object sender, EventArgs e)
        {
            if (!Owner.Camera.Opened)
            {
                var mb1 = new QMC.Common.UI.MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 카메라를 연결해야 해야 합니다.");
                return;
            }

            this.ImageViewer_RecipeVision_Rows.StopUpdateTask();

            if (Owner.Camera != null)
            {
                Owner.Camera.StopLive();
            }
        }

        private void radioButton_RecipeVision_Cross_CheckedChanged(object sender, EventArgs e)
        {
            nMarkType = 0;  //Cross
        }

        private void radioButton_RecipeVision_Circle_CheckedChanged(object sender, EventArgs e)
        {
            nMarkType = 1;  //Circle
        }

        private void radioButton_RecipeVision_Pattern_CheckedChanged(object sender, EventArgs e)
        {
            nSerchType = 0; //PatternMatching
        }

        private void radioButton_RecipeVision_Blob_CheckedChanged(object sender, EventArgs e)
        {
            nSerchType = 1; //Blob
        }

        private void button_RecipeVision_Train_Click(object sender, EventArgs e)
        {
            JigAlignerRecipe recipe = Owner.Recipe; 
            RoiTrain.Parameter.StartLocation = recipe.TrainRoiStartLocation;
            RoiTrain.Parameter.EndLocation = recipe.TrainRoiEndLocation;

            RoiInspect.Parameter.Overlay.Visible = false;
            RoiTrain.Parameter.Overlay.Visible = true;

            PatternMatchingResult result = Owner.GetResult();
            this.ImageViewer_RecipeVision_Rows.NormalOverlays.Add(RoiTrain.Parameter.Overlay);
            foreach (var overlay in result.ResultOverlays)
            {
                this.ImageViewer_RecipeVision_Rows.NormalOverlays.Remove(overlay);
            }
            this.ImageViewer_RecipeVision_Rows.Display();

            m_RoiListControl.RoiTrainClickNew();
        }

        private void button_RecipeVision_Inspect_Click(object sender, EventArgs e)
        {
            JigAlignerRecipe recipe = Owner.Recipe;
            RoiInspect.Parameter.StartLocation = recipe.InspectRoiStartLocation;
            RoiInspect.Parameter.EndLocation = recipe.InspectRoiEndLocation;

            RoiTrain.Parameter.Overlay.Visible = false;
            RoiInspect.Parameter.Overlay.Visible = true;

            PatternMatchingResult result = Owner.GetResult();
            this.ImageViewer_RecipeVision_Rows.NormalOverlays.Add(RoiInspect.Parameter.Overlay);
            foreach (var overlay in result.ResultOverlays)
            {
                this.ImageViewer_RecipeVision_Rows.NormalOverlays.Remove(overlay);
            }
            this.ImageViewer_RecipeVision_Rows.Display();

            this.m_RoiListControl.RoiAlignClickNew();
        }

        private void RoiTrainButtonClick(RoiVisionTool roiVisionTool)
        {
            if (Owner != null)
            {
                RoiTrain.Parameter.CenterLocation = roiVisionTool.Parameter.CenterLocation;
                RoiTrain.Parameter.Size = roiVisionTool.Parameter.Size;

                this.ImageViewer_RecipeVision_Rows.NormalOverlays.Add(RoiTrain.Parameter.Overlay);
                RoiTrain.Parameter.Overlay.Visible = true;
                this.ImageViewer_RecipeVision_Rows.Display();

            }
        }

        private void RoiInspectButtonClick(RoiVisionTool roiVisionTool)
        {
            if (Owner != null)
            {
                RoiInspect.Parameter.CenterLocation = roiVisionTool.Parameter.CenterLocation;
                RoiInspect.Parameter.Size = roiVisionTool.Parameter.Size;

                this.ImageViewer_RecipeVision_Rows.NormalOverlays.Add(RoiInspect.Parameter.Overlay);
                RoiInspect.Parameter.Overlay.Visible = true;
                this.ImageViewer_RecipeVision_Rows.Display();
            }
        }

        private void RoiTrainSaveButtonClick(RoiVisionTool roiVisionTool, bool bOk)
        {
            JigAlignerRecipe recipe = Owner.Recipe;

            if (!bOk)
            {
                RoiTrain.Parameter.StartLocation = recipe.TrainRoiStartLocation;
                RoiTrain.Parameter.EndLocation = recipe.TrainRoiEndLocation;
                RoiTrain.Parameter.Overlay.Visible = false;
                this.ImageViewer_RecipeVision_Rows.Display();
                return;
            }

            RoiTrain = roiVisionTool;
            RoiTrain.Parameter.Overlay.Visible = false;

            recipe.TrainRoiStartLocation = RoiTrain.Parameter.StartLocation;
            recipe.TrainRoiEndLocation = RoiTrain.Parameter.EndLocation;

            //Box_Setup_ScannerCal_ImageViewer.Display();
            //if (m_JogControl != null)
            //{
            //    m_JogControl.Show();
            //    m_JogControl.BringToFront();
            //    m_TrainImageControl.Show();
            //    m_TrainImageControl.BringToFront();
            //}
            SetTrainImage(Owner.Recipe.PatternMatchingParameter.TrainImage);



            this.ImageViewer_RecipeVision_Rows.Display();
        }

        private void SetTrainImage(VisionImage image)
        {
            this.pictureBox_RecipeVision_TrainImage.Image = image.GetImage();
            this.pictureBox_RecipeVision_TrainImage.BringToFront();
        }

        private void RoiInspectSaveButtonClick(RoiVisionTool roiVisionTool, bool bOk)
        {
            JigAlignerRecipe recipe = Owner.Recipe;
            if (!bOk)
            {
                RoiInspect.Parameter.StartLocation = recipe.InspectRoiStartLocation;
                RoiInspect.Parameter.EndLocation = recipe.InspectRoiEndLocation;
                this.ImageViewer_RecipeVision_Rows.Display();
                return;
            }
            RoiInspect = roiVisionTool;
            recipe.InspectRoiStartLocation = RoiInspect.Parameter.StartLocation;
            recipe.InspectRoiEndLocation = RoiInspect.Parameter.EndLocation;

            this.ImageViewer_RecipeVision_Rows.Display();


        }

        

        private void button_RecipeVision_Train_Set_Click(object sender, EventArgs e)
        {
            if (Owner != null)
            {
                RoiVisionTool roiVisionTool = m_RoiListControl.RoiTrainVisionTool;

                RoiTrain.Parameter.CenterLocation = roiVisionTool.Parameter.CenterLocation;
                RoiTrain.Parameter.Size = roiVisionTool.Parameter.Size;

                this.ImageViewer_RecipeVision_Rows.NormalOverlays.Add(RoiTrain.Parameter.Overlay);
                RoiTrain.Parameter.Overlay.Visible = true;
                this.ImageViewer_RecipeVision_Rows.Display();
            }

            string m_strFile = "";
            string m_strRecipe = "";
            RecipeInfo m_recipeInfo = new RecipeInfo();
            m_recipeInfo = Equipment.GetCurrentRecipe();

            Owner.Camera.LatestImage = this.ImageViewer_RecipeVision_Rows.InputImage;

            if (this.ImageViewer_RecipeVision_Rows.Simulated)
            {
                Owner.Simulated = true;
                Owner.TestImage = this.ImageViewer_RecipeVision_Rows.InputImage;
            }
            else
            {
                Owner.Simulated = false;
            }

            Owner.Train();
            SetTrainImage(Owner.TrainImage);

            //  train 할 때 레시피를 저장해줘야 정상적으로 패턴 이미지가 변경된다.
            if (m_recipeInfo != null)
            {
                Equipment.UpdateRecipeData();
            }
            Equipment.SetCurrentRecipe(m_recipeInfo);
            Equipment.SaveRecipe();
            FormManager.FireUpdateRecipeEvent();

            Equipment.ApplyRecipeData();

            if (Owner.Name == "JigAligner (Coarse)")
            {
                m_strFile = string.Format("{0}\\PreAlign.jpg", ConfigManager.GetPatternImagePath());
                Owner.TrainImage.Save(m_strFile, QMC.Common.Vision.VisionImage.FileFilter.jpg);

            }
        }

        private void radioButton_RecipeVision_Light_IR_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton_RecipeVision_Light_Red_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void baseToggleButton_RecipeVision_DuplicateCheck_Click(object sender, EventArgs e)
        {

        }

        private void baseToggleButton_RecipeVision_UseMaskImage_Click(object sender, EventArgs e)
        {

        }

        private void radioButton_RecipeVision_Pixel_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton_RecipeVision_mm_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button_RecipeVision_Search_Click(object sender, EventArgs e)
        {

        }

        private void checkBox_RecipeVision_CirclePos_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button_RecipeVision_Vision_Save_Click(object sender, EventArgs e)
        {

        }
    }
}
