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
using QMC.Common.Vision.Tools;
using QMC.Common.VisionPart;

namespace SLD200_MSL
{
    public delegate void SearchButtonClickHandler(Control control);

    public partial class SearchResultControl : UserControl
    {
        public PatternMatchingParameters m_PatternMatchingParameter { set; get; }

        public enum TabPages
        {
            Parameter,
            Result,
        }
        public event SearchButtonClickHandler SearchClick;

        public bool IsPixel
        {
            get { return this.radioButtonPixel.Checked; }
            set
            {
                if (value == true)
                {
                    this.radioButtonPixel.Checked = value;
                }
                else
                {
                    this.radioButtonPixel.Checked = false;
                }
            }
        }

        public double ConvertScale { get; set; }
        private Part m_Owner;
        public SearchResultControl(Part part)
        {
            m_Owner = part;
            InitializeComponent();
            this.tabControl1.Location = new Point(this.Location.X + 5, this.Location.Y + 15);
            this.tabControl1.Size = new Size(this.Size.Width - 10, this.Size.Height - 20);

            this.radioButtonPixel.Checked = true;

            this.groupBoxPosition.Location = new Point(5, 5);


            this.groupBoxPosition.Size = new Size(150, 90);

            this.baseGroupBoxPatternMatching.Location = new Point(5, 5);
            this.baseLabelX.Location = new Point(5, 20);
            this.baseLabelY.Location = new Point(baseLabelX.Location.X, baseLabelX.Location.Y + baseLabelX.Size.Height + 6);
            this.baseLabelT.Location = new Point(baseLabelY.Location.X, baseLabelY.Location.Y + baseLabelY.Size.Height + 6);
            this.baseTextBoxX.Location = new Point(baseLabelX.Location.X + baseLabelX.Size.Width + 5, baseLabelX.Location.Y);
            this.baseTextBoxY.Location = new Point(baseLabelY.Location.X + baseLabelY.Size.Width + 5, baseLabelY.Location.Y);
            this.baseTextBoxT.Location = new Point(baseLabelT.Location.X + baseLabelT.Size.Width + 5, baseLabelT.Location.Y);
            this.radioButtonPixel.Location = new Point(groupBoxPosition.Location.X + groupBoxPosition.Width + 10, groupBoxPosition.Location.Y + 5);
            this.radioButtonMillimiter.Location = new Point(radioButtonPixel.Location.X, radioButtonPixel.Location.Y + radioButtonPixel.Size.Height + 2);
            this.baseButtonSearch.Location = new Point(radioButtonMillimiter.Location.X, groupBoxPosition.Size.Height - baseButtonSearch.Size.Height);

            m_PatternMatchingParameter = new PatternMatchingParameters();

            TabPageCreate();
            init();
        }

        private void init()
        {
            if (m_PatternMatchingParameter != null)
            {
                baseTextBoxAngleTolerance.Text = m_PatternMatchingParameter.MaxTolerance.ToString();
                baseTextBoxMaxInstance.Text = m_PatternMatchingParameter.MaxInstance.ToString();
                baseTextBoxMinScore.Text = m_PatternMatchingParameter.MinScore.ToString();
                bool bOn = m_PatternMatchingParameter.DuplicateChecked;
                baseToggleButtonDuplicateCheck.UpdateToggleStatus(bOn);
                bOn = m_PatternMatchingParameter.UseMaskImage;
                baseToggleButtonUseMaskImage.UpdateToggleStatus(bOn);
            }
        }

        public void SetPatternMatchingData(PatternMatchingParameters parameter)
        {
            m_PatternMatchingParameter = parameter;
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

        private void baseButtonSearch_Click(object sender, EventArgs e)
        {
            m_PatternMatchingParameter.MaxTolerance = Convert.ToDouble(baseTextBoxAngleTolerance.Text);
            m_PatternMatchingParameter.MinTolerance = m_PatternMatchingParameter.MaxTolerance * -1;
            m_PatternMatchingParameter.MaxInstance = Convert.ToInt32(baseTextBoxMaxInstance.Text);
            m_PatternMatchingParameter.MinScore = Convert.ToDouble(baseTextBoxMinScore.Text);
            m_PatternMatchingParameter.DuplicateChecked = baseToggleButtonDuplicateCheck.GetButtonStatus();
            m_PatternMatchingParameter.UseMaskImage = baseToggleButtonUseMaskImage.GetButtonStatus();
            if (SearchClick != null)
            {
                SearchClick(this);
            }

        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton.Checked == radioButtonMillimiter.Checked)
            {
                //milimiter 체크 했을때

            }
            else
            {
                //Pixel 체크 했을때

            }
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
                        tabControl1.TabPages[i].Controls.Add(this.radioButtonMillimiter);
                        tabControl1.TabPages[i].Controls.Add(this.baseButtonSearch);
                        break;
                    default:
                        break;
                }
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
    }
}
