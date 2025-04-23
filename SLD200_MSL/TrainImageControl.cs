using QMC.Common.Vision;
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

namespace SLD200_MSL
{
    public delegate void TrainButtonClickEventHandler(TrainImageControl.ButtonType type);
    public partial class TrainImageControl : UserControl
    {
        public enum ButtonType
        {
            Train,
        }
        public PictureBox Picture
        {
            get
            {
                return pictureBoxTrainImage;
            }
        }
        public TrainButtonClickEventHandler TrainButtonClick;
        public TrainImageControl()
        {
            InitializeComponent();
            this.pictureBoxTrainImage.BackColor = Color.White;
        }

        private void baseButtonTrain_Click(object sender, EventArgs e)
        {
           if(TrainButtonClick != null)
            {
                TrainButtonClick(ButtonType.Train);
            }
        }

        public void SetTrainImage(VisionImage image)
        {
            this.pictureBoxTrainImage.Image = image.GetImage();
        }

        private void groupBoxTrainImage_Enter(object sender, EventArgs e)
        {

        }

        private void pictureBoxTrainImage_Click(object sender, EventArgs e)
        {

        }
    } 
}
