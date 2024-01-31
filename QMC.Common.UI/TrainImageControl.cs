using QMC.Common.Hmi;
using QMC.Common.Vision;
using QMC.Common.Vision.Cameras;
using QMC.Common.VisionPart;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QMC.Common.UI
{
    public delegate void TrainButtonClickEventHandler(TrainImageControl.ButtonType type);
    public delegate void TrainImageChangedEventHandler(VisionImage image);

    public partial class TrainImageControl : UserControl
    {
        [Serializable]
        public enum MenuItems
        {
            [Abbreviation("Image Save")]
            ImageSave,
            [Abbreviation("Image Load")]
            ImageLoad,
        }
        public enum ButtonType
        {
            Train,
        }
        public TrainPictureBox Picture
        {
            get
            {
                return pictureBoxTrainImage;
            }
        }
        public TrainButtonClickEventHandler TrainButtonClick;
        public TrainImageChangedEventHandler ImageChanged;
        private Camera m_Camera;

        public TrainImageControl(Camera camera)
        {
            m_Camera = camera;
            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();

            this.pictureBoxTrainImage.BackColor = Color.White;
            this.pictureBoxTrainImage.ImageChanged += TrainImageChanged;
            this.pictureBoxTrainImage.ResizeControl(184, 157);
        }

        private void TrainImageChanged()
        {
            if(ImageChanged != null)
            {
                ImageChanged(this.pictureBoxTrainImage.GetImage());
            }
        }

        private void baseButtonTrain_Click(object sender, EventArgs e)
        {
            if (TrainButtonClick != null)
            {
                TrainButtonClick(ButtonType.Train);
            }
        }

        public void SetTrainImage(VisionImage image)
        {
            this.pictureBoxTrainImage.SetImage(image.GetImage());
        }
        public VisionImage GetTrainImage()
        {
            VisionImage image = null;
            if (this.pictureBoxTrainImage.GetImage() != null)
            {
                image = this.pictureBoxTrainImage.GetImage();
            }
            return image;
        }

        private void baseToggleButtonAvg_Click(object sender, EventArgs e)
        {
            bool isOn = !this.baseToggleButtonAvg.GetButtonStatus();
            this.baseToggleButtonAvg.UpdateToggleStatus(isOn);
            //m_Camera.IsAvgOn = isOn;
        }

        private void pictureBoxTrainImage_MouseClick(object sender, MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (e.Button == MouseButtons.Right)
            {
                this.ContextMenuStrip.Show(e.X, e.Y);
            }
        }        
    }
}
