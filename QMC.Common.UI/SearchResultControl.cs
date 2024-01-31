using QMC.Common;
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

namespace QMC.Common.UI
{
    public delegate void SearchButtonClickHandler(Control control);
    
    public partial class SearchResultControl : UserControl
    {
        #region Define
        public event SearchButtonClickHandler SearchClick;

        public enum TabPages
        {
            Parameter,
            Result,
        }
        #endregion

        #region Property
        public PatternMatchingParameters PatternMatchingParameter { set; get; }

        public double ConvertScale { get; set; }

        public bool IsPixel { get; set; }

        private Part m_Owner;
        #endregion

        #region Constructor
        public SearchResultControl(Part part)
        {

            m_Owner = part;
            InitializeComponent();

            this.tabControl1.Location = new Point(this.Location.X + 5, this.Location.Y + 15);
            this.tabControl1.Size =  new Size(this.Size.Width - 10, this.Size.Height - 20);
            
            this.radioButtonPixel.Checked = true;
            this.IsPixel = true;

            this.groupBoxPosition.Location = new Point(5,5);
            this.groupBoxPosition.Size = new Size(150, 90);
            
            this.baseGroupBoxPatternMatching.Location = new Point(5, 5);
            
            this.baseLabelX.Location = new Point(5, 20);
            this.baseLabelY.Location = new Point(baseLabelX.Location.X, baseLabelX.Location.Y + baseLabelX.Size.Height + 10);
            this.baseLabelT.Location = new Point(baseLabelY.Location.X + 2, baseLabelY.Location.Y + baseLabelY.Size.Height + 10);
            
            this.baseTextBoxX.Location = new Point(baseLabelX.Location.X + baseLabelX.Size.Width + 5, baseLabelX.Location.Y);
            this.baseTextBoxY.Location = new Point(baseLabelY.Location.X + baseLabelY.Size.Width + 5, baseLabelY.Location.Y);
            this.baseTextBoxT.Location = new Point(baseLabelT.Location.X + baseLabelT.Size.Width + 5, baseLabelT.Location.Y);
            
            this.radioButtonPixel.Location = new Point(groupBoxPosition.Location.X + groupBoxPosition.Width + 10, groupBoxPosition.Location.Y);
            this.radioButtonMilimiter.Location = new Point(radioButtonPixel.Location.X, radioButtonPixel.Location.Y + radioButtonPixel.Size.Height + 10);
            
            this.baseButtonSearch.Location = new Point(radioButtonMilimiter.Location.X, groupBoxPosition.Size.Height - baseButtonSearch.Size.Height);

            PatternMatchingParameter = new PatternMatchingParameters();

            if(part is ColletXYPositionCalibrator)
            {
                this.baseLabelT.Visible = false;
                this.baseTextBoxT.Visible = false;
            }

            TabPageCreate();
            init();
        }
        #endregion

        #region Event Handler
        private void baseButtonSearch_Click(object sender, EventArgs e)
        {
            PatternMatchingParameter.MaxTolerance = Convert.ToDouble(baseTextBoxAngleTolerance.Text);
            PatternMatchingParameter.MaxInstance = Convert.ToInt32(baseTextBoxMaxInstance.Text);
            PatternMatchingParameter.MinTolerance = (Convert.ToInt32(baseTextBoxAngleTolerance.Text)) *-1;
            PatternMatchingParameter.MinScore = Convert.ToDouble(baseTextBoxMinScore.Text);
            PatternMatchingParameter.DuplicateChecked = baseToggleButtonDuplicateCheck.GetButtonStatus();
            PatternMatchingParameter.UseMaskImage = baseToggleButtonUseMaskImage.GetButtonStatus();
            if (SearchClick != null)
            {
                SearchClick(this);
            }

            UpdateOwnerRecipe(PatternMatchingParameter);
        }

        private void UpdateOwnerRecipe(PatternMatchingParameters parameters)
        {
            if(m_Owner is VisionCompensator)
            {
                VisionCompensator visionCompensator = m_Owner as VisionCompensator;
                visionCompensator.Recipe.PatternMatchingParameters= parameters;
            }
            else if(m_Owner is Aligner)
            {
                Aligner aligner = m_Owner as Aligner;
                aligner.Recipe.PatternMatchingParameter= parameters;
            }
            else if(m_Owner is VisionCalibrator)
            {
                VisionCalibrator visionCalibrator = m_Owner as VisionCalibrator;
                visionCalibrator.Recipe.PatternMatchingParameters = parameters;
            }
            else if(m_Owner is StageThetaAgingTester)
            {
                StageThetaAgingTester stageThetaAgingTester = m_Owner as StageThetaAgingTester;
                stageThetaAgingTester.Recipe.PatternMatchingParameter = parameters;
            }
            //Equipment.SaveRecipe();
        }
        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton.Checked == radioButtonMilimiter.Checked)
            {
                //milimiter 체크 했을때
                IsPixel = false;
            }
            else
            {
                //Pixel 체크 했을때
                IsPixel = true;
            }
        }

        private void baseToggleButtonDuplicateCheck_Click(object sender, EventArgs e)
        {
            bool bOn = baseToggleButtonDuplicateCheck.GetButtonStatus();
            if (bOn)
            {
                baseToggleButtonDuplicateCheck.UpdateToggleStatus(false);
            }
            else
            {
                baseToggleButtonDuplicateCheck.UpdateToggleStatus(true);
            }
        }

        private void baseToggleButtonUseMaskImage_Click(object sender, EventArgs e)
        {
            bool bOn = baseToggleButtonUseMaskImage.GetButtonStatus();
            if (bOn)
            {
                baseToggleButtonUseMaskImage.UpdateToggleStatus(false);
            }
            else
            {
                baseToggleButtonUseMaskImage.UpdateToggleStatus(true);
            }
        }
        #endregion

        #region Method
        public void init()
        {
            if(PatternMatchingParameter != null)
            {
                baseTextBoxAngleTolerance.Text = PatternMatchingParameter.MaxTolerance.ToString();
                baseTextBoxMaxInstance.Text = PatternMatchingParameter.MaxInstance.ToString();
                baseTextBoxMinScore.Text = PatternMatchingParameter.MinScore.ToString();
                bool bOn = PatternMatchingParameter.DuplicateChecked;
                baseToggleButtonDuplicateCheck.UpdateToggleStatus(bOn);
                bOn = PatternMatchingParameter.UseMaskImage;
                baseToggleButtonUseMaskImage.UpdateToggleStatus(bOn);
            }
        }

        public void SetPatternMatchingData(PatternMatchingParameters parameter)
        {
            PatternMatchingParameter = parameter;
            init();
        }

        public void UpdataPositionData(double x, double y, double t)
        {
            baseTextBoxX.Text = x.ToString();
            baseTextBoxY.Text = y.ToString();
            baseTextBoxT.Text = t.ToString();

        }
        public void UpdataPositionData(double x, double y)
        {
            baseTextBoxX.Text = x.ToString();
            baseTextBoxY.Text = y.ToString();
        }

        public void TabPageCreate()
        {
            this.tabControl1.Controls.Clear();
            TabPage tabPage = new TabPage();


            foreach (TabPages item in Enum.GetValues(typeof(TabPages)))
            {
                string pageName = item.ToString();

                tabPage = new TabPage(pageName);
                tabPage.BackColor = Color.FromArgb(200, 200, 200);
                tabPage.ForeColor = Color.FromArgb(78, 78, 78);
                tabControl1.TabPages.Add(tabPage);
            }
            for (int i = 0; i < tabControl1.TabCount; i++)
            {
                switch (tabControl1.TabPages[i].Text)
                {
                    case "Parameter":
                        tabControl1.TabPages[i].Controls.Add(this.baseGroupBoxPatternMatching);
                        break;
                    case "Result":

                        tabControl1.TabPages[i].Controls.Add(this.radioButtonPixel);
                        tabControl1.TabPages[i].Controls.Add(this.groupBoxPosition);
                        tabControl1.TabPages[i].Controls.Add(this.radioButtonMilimiter);
                        tabControl1.TabPages[i].Controls.Add(this.baseButtonSearch);
                        break;
                    default:
                        break;
                }
            }

        }

        public void UpdataPositionData(double x, double y, double t, double s)
        {
            baseTextBoxX.Text = x.ToString();
            baseTextBoxY.Text = y.ToString();
            baseTextBoxT.Text = t.ToString();
            //m_TextBoxScore.Text = s.ToString();
        }
        #endregion

    }
}
