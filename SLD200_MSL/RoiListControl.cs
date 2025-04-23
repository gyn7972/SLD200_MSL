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

namespace SLD200_MSL
{
    public delegate void RoiTrainbuttonClickHandler(RoiVisionTool roiVisionTool);
    public delegate void RoiAlignbuttonClickHandler(RoiVisionTool roiVisionTool);
    public delegate void RoiTrainSaveClickHandler(RoiVisionTool roiVisionTool, bool bOk);
    public delegate void RoiAlignSaveClickHandler(RoiVisionTool roiVisionTool, bool bOk);

    public delegate void RoiTrainClickHandler();
    public delegate void RoiAlignClickHandler();

    public partial class RoiListControl : UserControl
    {
        public enum RoiButton
        {
            Train,
            Inspect
        }
        public RoiTrainbuttonClickHandler roiTrainButtonClick { get; set; }
        public RoiAlignbuttonClickHandler roiAlignButtonClick { get; set; }
        public RoiTrainSaveClickHandler roiTrainSaveButtonClick { get; set; }
        public RoiAlignSaveClickHandler roiAlignSaveButtonClick { get; set; }

        public RoiTrainClickHandler roiTrainClick { get; set; }
        public RoiAlignClickHandler roiAlignClick { get; set; }
        FormBaseConfiguration Configuration { get; set; }

        public RoiVisionTool RoiTrainVisionTool { get; set; }

        public RoiVisionTool RoiAlignVisionTool { get; set; }
        public Size FullSize { get; set; }

        public double CenterX { get; set; }
        public double CenterY { get; set; }
        public double RoiWidth { get; set; }
        public double RoiHeight { get; set; }




        public RoiListControl(RoiVisionTool roiTrainVisionTool, RoiVisionTool roiAlignVisionTool, Size fullsize)
        {
            InitializeComponent();
            Configuration = new FormBaseConfiguration();
            if (roiTrainVisionTool != null)
            {
                RoiTrainVisionTool = roiTrainVisionTool;
            }
            if (roiAlignVisionTool != null)
            {
                RoiAlignVisionTool = roiAlignVisionTool;
            }
            FullSize = fullsize;

            SetRioButton();
        }
        public RoiListControl() : this(null, null, new Size(640, 480))
        {

        }

        public void SetRioButton()
        {
            foreach (RoiButton item in Enum.GetValues(typeof(RoiButton)))
            {
                if (RoiTrainVisionTool == null)
                {
                    if (item == RoiListControl.RoiButton.Train)
                    {
                        continue;
                    }
                }
                if (RoiAlignVisionTool == null)
                {
                    if (item == RoiListControl.RoiButton.Inspect)
                    {
                        continue;
                    }
                }
                BaseButton RoiButton = new BaseButton();
                RoiButton.Name = item.ToString();
                RoiButton.Click += Button_Click;
                RoiButton.Text = item.ToString();
                RoiButton.Size = new Size(112, 30);
                RoiButton.BackColor = Color.White;
                RoiButton.ForeColor = Color.Black;
                RoiButton.FlatAppearance.BorderColor = Color.Aqua;
                RoiButton.FlatAppearance.BorderSize = 1;

                this.flowLayoutPanelButton.Controls.Add(RoiButton);
            }

        }
        public void RoiTrainButton_Click(RoiVisionTool roiVisionTool)
        {
            if (roiTrainButtonClick != null)
                roiTrainButtonClick(RoiTrainVisionTool);
        }
        public void RoiAlignButton_Click(RoiVisionTool roiVisionTool)
        {
            if (roiAlignButtonClick != null)
                roiAlignButtonClick(RoiAlignVisionTool);
        }
        public void RoiTrainClick()
        {
            if (roiTrainClick != null)
                roiTrainClick();
        }
        public void RoiAlignClick()
        {
            if (roiAlignClick != null)
                roiAlignClick();
        }
        private void Button_Click(object sender, EventArgs e)
        {
            BaseButton baseButton = (BaseButton)sender;
            if (baseButton != null)
            {
                if (baseButton.Name == RoiButton.Train.ToString())
                {
                    RoiTrainClick();
                    FormSetRoi FormSetRoi = new FormSetRoi(RoiTrainVisionTool, FullSize);
                    FormSetRoi.RoiButtonClick += RoiTrainButton_Click;

                    FormSetRoi.Location = this.Location;
                    FormSetRoi.StartPosition = FormStartPosition.CenterScreen;

                    if (FormSetRoi.ShowDialog() == DialogResult.OK)
                    {
                        CenterX = FormSetRoi.CenterX;
                        CenterY = FormSetRoi.CenterY;
                        Width = FormSetRoi.Width;
                        Height = FormSetRoi.Height;
                        roiTrainSaveButtonClick(FormSetRoi.RoiVisionTool, true);
                    }
                    else
                    {
                        CenterX = FormSetRoi.CenterX;
                        CenterY = FormSetRoi.CenterY;
                        Width = FormSetRoi.Width;
                        Height = FormSetRoi.Height;
                        roiTrainSaveButtonClick(FormSetRoi.RoiVisionTool, false);
                    }
                }
                else
                {
                    //얼라인버튼 눌렀을때 --> Inspect 버튼을 눌렀을 때l
                    RoiAlignClick();
                    FormSetRoi FormSetRoi = new FormSetRoi(RoiAlignVisionTool, FullSize);
                    FormSetRoi.RoiButtonClick += RoiAlignButton_Click;


                    FormSetRoi.Location = this.Location;
                    FormSetRoi.StartPosition = FormStartPosition.CenterScreen;

                    if (FormSetRoi.ShowDialog() == DialogResult.OK)
                    {
                        CenterX = FormSetRoi.CenterX;
                        CenterY = FormSetRoi.CenterY;
                        Width = FormSetRoi.Width;
                        Height = FormSetRoi.Height;
                        roiAlignSaveButtonClick(FormSetRoi.RoiVisionTool, true);
                    }
                    else
                    {
                        CenterX = FormSetRoi.CenterX;
                        CenterY = FormSetRoi.CenterY;
                        Width = FormSetRoi.Width;
                        Height = FormSetRoi.Height;
                        roiAlignSaveButtonClick(FormSetRoi.RoiVisionTool, false);
                    }
                }

                //
            }
        }

        public void RoiTrainClickNew()
        {
            FormSetRoi FormSetRoi = new FormSetRoi(RoiTrainVisionTool, FullSize);
            FormSetRoi.RoiButtonClick += RoiTrainButton_Click;

            FormSetRoi.Location = this.Location;
            FormSetRoi.StartPosition = FormStartPosition.CenterScreen;

            if (FormSetRoi.ShowDialog() == DialogResult.OK)
            {
                CenterX = FormSetRoi.CenterX;
                CenterY = FormSetRoi.CenterY;
                Width = FormSetRoi.Width;
                Height = FormSetRoi.Height;
                roiTrainSaveButtonClick(FormSetRoi.RoiVisionTool, true);
            }
            else
            {
                CenterX = FormSetRoi.CenterX;
                CenterY = FormSetRoi.CenterY;
                Width = FormSetRoi.Width;
                Height = FormSetRoi.Height;
                roiTrainSaveButtonClick(FormSetRoi.RoiVisionTool, false);
            }
        }

        public void RoiAlignClickNew()
        {
            FormSetRoi FormSetRoi = new FormSetRoi(RoiAlignVisionTool, FullSize);
            FormSetRoi.RoiButtonClick += RoiAlignButton_Click;
            FormSetRoi.Location = this.Location;
            FormSetRoi.StartPosition = FormStartPosition.CenterScreen;
            if (FormSetRoi.ShowDialog() == DialogResult.OK)
            {
                CenterX = FormSetRoi.CenterX;
                CenterY = FormSetRoi.CenterY;
                Width = FormSetRoi.Width;
                Height = FormSetRoi.Height;
                roiAlignSaveButtonClick(FormSetRoi.RoiVisionTool, true);
            }
            else
            {
                CenterX = FormSetRoi.CenterX;
                CenterY = FormSetRoi.CenterY;
                Width = FormSetRoi.Width;
                Height = FormSetRoi.Height;
                roiAlignSaveButtonClick(FormSetRoi.RoiVisionTool, false);
            }
        }
    }
}
