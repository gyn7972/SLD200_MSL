using QMC.Common;
using QMC.Common.Modules;
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

namespace CWA150SA_Onsemi300
{
    public partial class PatternMatchingRecipeControl : UserControl
    {
        protected object m_Recipe;
        public PatternMatchingParameters Parameters { get; set; }
        public double Tolerance { get; set; }
        public bool DuplicateChecked { get; set; }
        public int MaxInstnce { get; set; }
        public double MinScore { get; set; }
        public VisionImage TrainImage { get; set; }
        public VisionImage LearnImage { get; set; }
        public bool UseMaskImage { get; set; }

        private RectangleD m_StartRectAngle;
        private RectangleD m_EndRectAngle;
        private RectangleD m_RectAngle;
        public PatternMatchingRecipeControl(Part part)
        {
            InitializeComponent();
            this.pictureBoxTrainImage.BackColor = Color.White;
            this.UseMaskImage = false;
            m_StartRectAngle = new RectangleD();
            m_EndRectAngle = new RectangleD();
            m_RectAngle = new RectangleD();

            SetPart(part);
            UpdateDetailParameters();
            UpdateParameters();
            
        }

        protected virtual void SetPart(Part part)
        {
            Module module = part.Owner;

            if(module is WaferProbeAlign)
            {
                if (part is VisionCalibrator)
                {
                    Parameters = ((WaferProbeAlign)module).Recipe.VisionCalibratorRecipe_Upper.PatternMatchingParameters;
                    m_Recipe = ((WaferProbeAlign)module).Recipe.VisionCalibratorRecipe_Upper;
                }
            }
/*            else if(module is ReelFeederAndMounter)
            {
                if(part is TwoPointAligner)
                {
                    Parameters = ((ReelFeederAndMounter)module).Recipe.TwoPointAlignerRecipe.PatternMatchingParameter;
                    m_Recipe = ((ReelFeederAndMounter)module).Recipe.TwoPointAlignerRecipe;
                }

                if(part is Revision)
                {
                    Parameters = ((ReelFeederAndMounter)module).Recipe.RevisionRecipe.PatternMatchingParameter;
                    m_Recipe = ((ReelFeederAndMounter)module).Recipe.RevisionRecipe;
                }
            }*/
        }

        public void SetGroupBoxName(string strName)
        {
            baseGroupBoxAlignRecipe.Text = strName;
        }

        public void UpdateDetailParameters()
        {
            this.basePropertyGridParameter.SelectedObject = m_Recipe;
        }
        
        public void UpdateParameters()
        {
            if (Parameters == null)
            {
                Parameters = new PatternMatchingParameters();
            }
            this.UseMaskImage = Parameters.UseMaskImage;
            this.checkBoxDupCheck.Checked = Parameters.DuplicateChecked;
            this.TextBoxTolerance.Text = Parameters.MaxTolerance.ToString();
            this.TextMaxInstance.Text = Parameters.MaxInstance.ToString();
            this.TextMinScore.Text = Parameters.MinScore.ToString();
            this.pictureBoxTrainImage.Image = Parameters.TrainImage.GetImage();
            this.ToggleButtonUseMaskImage.UpdateToggleStatus(Parameters.UseMaskImage);

        }
        public void UpdateParameters(PatternMatchingParameters parameter)
        {
            this.UseMaskImage = parameter.UseMaskImage;
            this.checkBoxDupCheck.Checked = parameter.DuplicateChecked;
            this.TextBoxTolerance.Text = parameter.MaxTolerance.ToString();
            this.TextMaxInstance.Text = parameter.MaxInstance.ToString();
            this.TextMinScore.Text = parameter.MinScore.ToString();
            this.pictureBoxTrainImage.Image = parameter.TrainImage.GetImage();
            this.ToggleButtonUseMaskImage.UpdateToggleStatus(parameter.UseMaskImage);

        }

        private void ChangeParametersTolerance(object sender, EventArgs e)
        {
            if (Parameters != null)
            {
                double dValue = 0.0;
                double.TryParse(TextBoxTolerance.Text, out dValue);
                Parameters.MaxTolerance = dValue;
                Parameters.MinTolerance = dValue * -1;
            }
        }
        private void ChangeParametersMaxInstnce(object sender, EventArgs e)
        {
            if (Parameters != null)
            {
                Parameters.MaxInstance = int.Parse(TextMaxInstance.Text);

            }
        }
        private void ChangeParametersMinScore(object sender, EventArgs e)
        {
            if (Parameters != null)
            {
                Parameters.MinScore = double.Parse(TextMinScore.Text);

            }
        }

        private void baseToggleButton_Click(object sender, EventArgs e)
        {
            bool bOn = ToggleButtonUseMaskImage.GetButtonStatus();
            if (bOn == false)
            {
                ToggleButtonUseMaskImage.UpdateToggleStatus(true);

                this.pictureBoxTrainImage.MouseDown += pictureBoxTrainImage_MouseDown;
                this.pictureBoxTrainImage.MouseUp += pictureBoxTrainImage_MouseUp;
                this.pictureBoxTrainImage.MouseMove += pictureBoxTrainImage_MouseMove;
                this.pictureBoxTrainImage.Paint += pictureBoxTrainImage_Paint;
            }

            else if (bOn == true)
            {
                ToggleButtonUseMaskImage.UpdateToggleStatus(false);
                this.pictureBoxTrainImage.MouseDown += null;
                this.pictureBoxTrainImage.MouseUp += null;
                this.pictureBoxTrainImage.MouseMove += null;
                this.pictureBoxTrainImage.Paint += null;
            }
            Parameters.UseMaskImage = ToggleButtonUseMaskImage.GetButtonStatus();
        }

        private void pictureBoxTrainImage_MouseDown(object sender, MouseEventArgs e)
        {
            m_StartRectAngle = new RectangleD(e.X, e.Y, 0, 0);
        }
        private void pictureBoxTrainImage_MouseUp(object sender, MouseEventArgs e)
        {
            m_EndRectAngle = new RectangleD(e.X, e.Y, 0, 0);
        }
        private void pictureBoxTrainImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                m_RectAngle = new RectangleD(m_StartRectAngle.X, m_StartRectAngle.Y,
                    Math.Max(e.X - m_StartRectAngle.Y, m_EndRectAngle.Y - m_StartRectAngle.Y),
                    Math.Max(e.Y - m_StartRectAngle.X, m_EndRectAngle.X - m_StartRectAngle.X));
                this.Refresh();
            }
            Parameters.MaskRegion = m_RectAngle;
        }
        private void pictureBoxTrainImage_Paint(object sender, PaintEventArgs e)
        {
            using (Pen pen = new Pen(Color.Red, 2))
            {
                Brush brush = new SolidBrush(Color.Red);
                e.Graphics.DrawRectangle(pen, m_RectAngle);
                e.Graphics.FillRectangle(brush, m_RectAngle);
            }
        }
        private void basePropertyGridParameter_PropertyValueChanged(object sender, EventArgs e)
        {
        }

        private void basePropertyGridParameter_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            
        }
    }
}
