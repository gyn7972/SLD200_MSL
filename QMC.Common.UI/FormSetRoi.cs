using QMC.Common.Hmi;
using QMC.Common.Vision.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QMC.Common.UI
{
    public delegate void RoiButtonClickHandler(RoiVisionTool roiVisionTool);

    public partial class FormSetRoi : Form
    {

        public RoiButtonClickHandler RoiButtonClick { get; set; }

        public FormBaseConfiguration Configuration { get; set; }
        public double CenterX { get; set; }
        public double CenterY { get; set; }
        public double RoiWidth { get; set; }
        public double RoiHeight { get; set; }

        public Size FullSize { get; set; }

        public int MaxXSize { get; set; }
        public int MaxYSize { get; set; }
        public RoiVisionTool RoiVisionTool { get; set; }



        public FormSetRoi(RoiVisionTool roiVisionTool, Size fullsize)
        {
            InitializeComponent();
            FullSize = fullsize;
            Configuration = new FormBaseConfiguration();
            RoiVisionTool = roiVisionTool;

            MaxXSize = fullsize.Width;
            MaxYSize = fullsize.Height;
            this.BackColor = Color.FromArgb(150, 150, 150);

            baseTextBoxCenterX.Text = RoiVisionTool.Parameter.CenterLocation.X.ToString();
            baseTextBoxCenterY.Text = RoiVisionTool.Parameter.CenterLocation.Y.ToString();
            baseTextBoxWidth.Text = RoiVisionTool.Parameter.Size.Width.ToString();
            baseTextBoxHeight.Text = RoiVisionTool.Parameter.Size.Height.ToString();

            CenterX = RoiVisionTool.Parameter.CenterLocation.X;
            CenterY = RoiVisionTool.Parameter.CenterLocation.Y;
            RoiWidth = RoiVisionTool.Parameter.Size.Width;
            RoiHeight = RoiVisionTool.Parameter.Size.Height;

            baseTextBoxMoveToke.Text = "10";
            baseTextBoxSizeToke.Text = "10";
        }
        private void baseButtonSave_Click(object sender, EventArgs e)
        {

            CenterX = long.Parse(baseTextBoxCenterX.Text);
            CenterY = long.Parse(baseTextBoxCenterY.Text);

            RoiHeight = long.Parse(baseTextBoxHeight.Text);
            RoiWidth = long.Parse(baseTextBoxHeight.Text);

            if(RoiHeight >= FullSize.Height ||  RoiWidth >= FullSize.Width)
            {
                RoiVisionTool.Parameter.IsFull =  true;
            }
            else
            {
                RoiVisionTool.Parameter.IsFull = false;
            }
            DialogResult = DialogResult.OK;
        }

        private void baseButtonLocation_Click(object sender, EventArgs e)
        {
            BaseButton button = sender as BaseButton;
            Point point = new Point(int.Parse(baseTextBoxCenterX.Text), int.Parse(baseTextBoxCenterY.Text));
            
            //Location Move
            if (button == this.baseButtonMoveCenter)
            {
                point.X = MaxXSize / 2;
                point.Y = MaxYSize / 2;
                CenterX = point.X;
                CenterY = point.Y;

                RoiVisionTool.Parameter.CenterLocation = point;
                //가운데로 움직인다.
                RoiButtonClick(RoiVisionTool);
            }
            else if (button == this.baseButtonMoveRight)
            {
                // MoveToke 만큼 움직인다 = baseTextBoxMoveToke.Text;
                CenterX += double.Parse(baseTextBoxMoveToke.Text);
                point.X = Convert.ToInt32(CenterX);
                RoiVisionTool.Parameter.CenterLocation = point;
                RoiButtonClick(RoiVisionTool);
            }
            else if (button == this.baseButtonMoveLeft)
            {
                // MoveToke 만큼 움직인다 = baseTextBoxMoveToke.Text;
                CenterX -= double.Parse(baseTextBoxMoveToke.Text);
                point.X = Convert.ToInt32(CenterX);
                RoiVisionTool.Parameter.CenterLocation = point;

                RoiButtonClick(RoiVisionTool);
            }
            else if (button == this.baseButtonMoveDown)
            {
                // MoveToke 만큼 움직인다 = baseTextBoxMoveToke.Text;
                CenterY += double.Parse(baseTextBoxMoveToke.Text);
                point.Y = Convert.ToInt32(CenterY);
                RoiVisionTool.Parameter.CenterLocation = point;

                RoiButtonClick(RoiVisionTool);
            }
            else if (button == this.baseButtonMoveUp)
            {
                // MoveToke 만큼 움직인다 = baseTextBoxMoveToke.Text;
                CenterY -= double.Parse(baseTextBoxMoveToke.Text);
                point.Y = Convert.ToInt32(CenterY);
                RoiVisionTool.Parameter.CenterLocation = point;

                RoiButtonClick(RoiVisionTool);
            }

            baseTextBoxCenterX.Text = CenterX.ToString();
            baseTextBoxCenterY.Text = CenterY.ToString();
            baseTextBoxWidth.Text = RoiWidth.ToString();
            baseTextBoxHeight.Text = RoiHeight.ToString();


        }

        private void baseButtonSize_Click(object sender, EventArgs e)
        {
            BaseButton button = sender as BaseButton;
            //Roi Size 
            Size size = new Size(int.Parse(baseTextBoxWidth.Text), int.Parse(baseTextBoxHeight.Text));
            Point point = new Point(int.Parse(baseTextBoxCenterX.Text), int.Parse(baseTextBoxCenterY.Text));

            if (button == this.baseButtonRoiXSizeDown)
            {
                // SizeToke 만큼 사이즈를 조절한다 = baseTextBoxSizeToke.Text;
                RoiWidth -= double.Parse(baseTextBoxSizeToke.Text);
                size.Width = Convert.ToInt32(RoiWidth);
                RoiVisionTool.Parameter.Size = size;

                RoiButtonClick(RoiVisionTool);
            }
            else if (button == this.baseButtonRoiXSizeUp)
            {
                // SizeToke 만큼 사이즈를 조절한다 = baseTextBoxSizeToke.Text;
                RoiWidth += double.Parse(baseTextBoxSizeToke.Text);
                size.Width = Convert.ToInt32(RoiWidth);
                RoiVisionTool.Parameter.Size = size;

                RoiButtonClick(RoiVisionTool);
            }
            else if (button == this.baseButtonRoiYSizeDown)
            {
                // SizeToke 만큼 사이즈를 조절한다 = baseTextBoxSizeToke.Text;
                RoiHeight -= double.Parse(baseTextBoxSizeToke.Text);
                size.Height = Convert.ToInt32(RoiHeight);
                RoiVisionTool.Parameter.Size = size;


                RoiButtonClick(RoiVisionTool);
            }
            else if (button == this.baseButtonRoiYSizeUp)
            {
                // SizeToke 만큼 사이즈를 조절한다 = baseTextBoxSizeToke.Text;
                RoiHeight += double.Parse(baseTextBoxSizeToke.Text);
                size.Height = Convert.ToInt32(RoiHeight);
                RoiVisionTool.Parameter.Size = size;

                RoiButtonClick(RoiVisionTool);
            }
            else if (button == this.baseButtonFullSize)
            {
                RoiVisionTool.Parameter.IsFull = true;

                size.Width = MaxXSize;
                size.Height = MaxYSize;
                point.X = MaxXSize / 2;
                point.Y = MaxYSize / 2;

                RoiWidth = size.Width;
                RoiHeight = size.Height;
                CenterX = point.X;
                CenterY = point.Y;

                RoiVisionTool.Parameter.Size = size;
                RoiVisionTool.Parameter.CenterLocation = point;
                //사이즈를 Full로 맞춘다
                RoiButtonClick(RoiVisionTool);
            }
            baseTextBoxWidth.Text = RoiWidth.ToString();
            baseTextBoxHeight.Text = RoiHeight.ToString();
            baseTextBoxCenterX.Text = CenterX.ToString();
            baseTextBoxCenterY.Text = CenterY.ToString();


        }

        private void baseButtonClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
