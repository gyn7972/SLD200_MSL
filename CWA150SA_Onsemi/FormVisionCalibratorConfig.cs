using QMC.Common;
using QMC.Common.Modules;
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
    public partial class FormVisionCalibratorConfig : FormSubContentBase
    {

        private VisionCalScaleControl m_visionCalScaleControl;
        private VisionCalibrator m_Owner;
        public FormVisionCalibratorConfig(Part part)
            : base(FormType.withButton.ToString(), part.Name)
        {
            m_Owner = part as VisionCalibrator;
            m_visionCalScaleControl = new VisionCalScaleControl(m_Owner.Config);
            InitializeComponent();
            this.panelContent.Visible = false;
            this.panelContent.Size = new Size();
            this.Controls.Add(m_visionCalScaleControl);

            this.flowLayoutPanelButton.FlowDirection = FlowDirection.RightToLeft;
            this.flowLayoutPanelButton.Location = new Point(0, this.baseLabelTitle.Location.Y + this.baseLabelTitle.Height);

            this.m_visionCalScaleControl.Location = new Point(Configuration.ContentLocation.X, this.flowLayoutPanelButton.Location.Y + this.flowLayoutPanelButton.Height + Configuration.ControlGap);
            this.m_visionCalScaleControl.VisionCalScaleControlCheckBoxChecked += VisionCalibratorCheckChange;

            CreateButton();
        }

        private void VisionCalibratorCheckChange(string strName)
        {
            if (strName == "XInverted")
            {

            }
            else if (strName == "YInverted")
            {

            }
            else if (strName == "Enabled")
            {

            }
        }

        public void CreateButton()
        {
            {
                BaseButton btn = new BaseButton();
                btn.Text = "Save";
                btn.Name = "buttonSave";
                btn.Click += SaveButton_Click;
                flowLayoutPanelButton.Controls.Add(btn);
            }

            {
                BaseButton btn = new BaseButton();
                btn.Text = "Load";
                btn.Name = "buttonLoad";
                btn.Click += LoadButton_Click;
                flowLayoutPanelButton.Controls.Add(btn);
            }
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            Module module = m_Owner.Owner;
            string m_strRecipe = "";
            RecipeInfo m_recipeInfo = new RecipeInfo();
            m_recipeInfo = Equipment.GetCurrentRecipe();

            if (module != null)
            {
                if (m_recipeInfo != null)
                {
                    m_strRecipe = m_recipeInfo.Name;

                    Equipment.LoadConfig(m_strRecipe); // 참고 : param load                       //  2022. 06. 30.  SCH : Recipe 에 따라 Config 파라미터를 변경하기 위해 이걸로 함.
                    DataManager.Instance.ApplyConfigData(module);
                }
                else
                {
                    Equipment.LoadConfig(); // 참고 : param load                                //  2022. 06. 30.  SCH : 원래 이건데...
                    DataManager.Instance.ApplyConfigData(module);
                }

                //m_visionCalScaleControl.UpdateUI(m_Owner.Config);
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Module module = m_Owner.Owner;
            string m_strRecipe = "";
            RecipeInfo m_recipeInfo = new RecipeInfo();
            m_recipeInfo = Equipment.GetCurrentRecipe();

            if (module != null)
            {
                //m_visionCalScaleControl.UpdateConfigData();
                if (m_recipeInfo != null)
                {
                    m_strRecipe = m_recipeInfo.Name;
                    DataManager.Instance.UpdateConfigData(module); // 참고 : param save
                                                                   //Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                    Equipment.SaveConfig(m_strRecipe);                                            //  2022. 06. 30.  SCH : Recipe 에 따라 Config 파라미터를 변경하기 위해 이걸로 함.
                }
                else
                {
                    DataManager.Instance.UpdateConfigData(module); // 참고 : param save
                    Equipment.SaveConfig();                                                     //  2022. 06. 30.  SCH : 원래 이건데...
                }
            }
        }
    }
}
