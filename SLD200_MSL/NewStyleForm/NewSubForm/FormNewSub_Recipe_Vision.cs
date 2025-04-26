using QMC.Common;
using QMC.Common.Hmi;
using QMC.Common.Modules;
using QMC.Common.Parts;
using QMC.Common.UI;
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

namespace SLD200.NewStyleForm.NewSubForm
{
    public partial class FormNewSub_Recipe_Vision : UserControl
    {
        static WorkStage workStage;

        private int nMarkType = 0; //0:Cross, 1:Circle 등
        private int nSerchType = 0; //0:PatternMatching, 1:Blob, 2:CircleA 등

        #region Property
        public RoiVisionTool RoiTrain { get; set; }
        public RoiVisionTool RoiInspect { get; set; }
        public PatternMatchingParameters PatternMatchingParameter { set; get; }
        public BlobVisionToolParameter BlobParameter { get; set; }
        #endregion

        private RoiListControl m_RoiListControl;


        public FormNewSub_Recipe_Vision()
        {
            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;
            foreach (Module module in m_collectionModules)
            {
                if (module.Name == "WorkStage")
                {
                    workStage = module as WorkStage;
                }
            }
            this.ImageViewer_RecipeVision_highs = new VisionImageViewer();
            this.ImageViewer_RecipeVision_highs.SizeMode = PictureBoxSizeMode.CenterImage;
            this.ImageViewer_RecipeVision_highs.SuspendDisplay();
            this.ImageViewer_RecipeVision_highs.Camera = workStage.jigAligner_HighRes.Camera;

            this.ImageViewer_RecipeVision_Rows = new VisionImageViewer();
            this.ImageViewer_RecipeVision_Rows.SizeMode = PictureBoxSizeMode.CenterImage;
            this.ImageViewer_RecipeVision_Rows.SuspendDisplay();
            this.ImageViewer_RecipeVision_Rows.Camera = workStage.jigAligner_LowRes.Camera;
            


            InitializeComponent();
        }

        private void button_RecipeVision_CameraLive_Click(object sender, EventArgs e)
        {
            if (!workStage.jigAligner_LowRes.Camera.Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 카메라를 연결해야 해야 합니다.");
                return;
            }

            ImageViewer_RecipeVision_Rows.Simulated = false;
            workStage.jigAligner_LowRes.Simulated = false;

            this.ImageViewer_RecipeVision_Rows.StartUpdateTask();
            this.ImageViewer_RecipeVision_Rows.Visible = true;

            if (workStage.jigAligner_LowRes.Camera != null)
            {
                workStage.jigAligner_LowRes.Camera.StartLive();
            }
        }

        private void button_RecipeVision_CameraStop_Click(object sender, EventArgs e)
        {
            if (!workStage.jigAligner_LowRes.Camera.Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 카메라를 연결해야 해야 합니다.");
                return;
            }

            this.ImageViewer_RecipeVision_Rows.StopUpdateTask();

            if (workStage.jigAligner_LowRes.Camera != null)
            {
                workStage.jigAligner_LowRes.Camera.StopLive();
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
            JigAlignerRecipe recipe = workStage.jigAligner_LowRes.Recipe; 
            RoiTrain.Parameter.StartLocation = recipe.TrainRoiStartLocation;
            RoiTrain.Parameter.EndLocation = recipe.TrainRoiEndLocation;

            RoiInspect.Parameter.Overlay.Visible = false;
            RoiTrain.Parameter.Overlay.Visible = true;

            PatternMatchingResult result = workStage.scannerCompensator.GetResult();
            this.ImageViewer_RecipeVision_Rows.NormalOverlays.Add(RoiTrain.Parameter.Overlay);
            foreach (var overlay in result.ResultOverlays)
            {
                this.ImageViewer_RecipeVision_Rows.NormalOverlays.Remove(overlay);
            }
            this.ImageViewer_RecipeVision_Rows.Display();

            //this.m_RoiListControl.RoiTrainClickNew();
        }

        private void button_RecipeVision_Inspect_Click(object sender, EventArgs e)
        {

        }

        private void button_RecipeVision_Train_Set_Click(object sender, EventArgs e)
        {

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
