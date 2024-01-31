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
    public delegate void RevisionParameterControlButtonClickHandler(RevisionParameterControl.ButtonType type);
    public partial class RevisionParameterControl : UserControl
    {
        #region Define
        public enum ButtonType
        {
            SaveFailedimag,
            SaveRevisionIma,
            SaveRecipe,
            LoadRecipe,
            Run,
        }
        #endregion

        #region Field
        public RevisionParameterControlButtonClickHandler ButtonClick;
        private Revision m_Owner { get; set; }
        private WaferProbeAlign m_Module { get; set; }
        RevisionRecipe m_Recipe { get; set; }
        #endregion

        #region Constructor
        public RevisionParameterControl(Part part)
        {
            InitializeComponent();

            if (part != null && part is Revision)
            {
                m_Owner = part as Revision;
                if (m_Owner != null)
                {
                    m_Module = m_Owner.Owner as WaferProbeAlign;
                }
            }
            UpdateUI();
        }
        #endregion

        #region Method
        public void UpdateUI()
        {
            if (m_Owner != null)
            {
                baseTextBoxColletSize.Text = m_Recipe.ColletSize.ToString();
                baseTextBoxChipThres.Text = m_Recipe.ChipThres.ToString();
                baseTextBoxEdgeAvgCount.Text = m_Recipe.EdgeAvgCount.ToString();
                baseTextBoxFilterOffset.Text = m_Recipe.HighPassFilterOffset.ToString();
                baseTextBoxToleranceX.Text = m_Recipe.ColletTolerenceX.ToString();

                baseTextBoxTargetWidth.Text = m_Recipe.TartgetSizeWidth.ToString();
                baseTextBoxTargetHight.Text = m_Recipe.TartgetSizeHight.ToString();

                baseToggleButtonShowEdgeImg.UpdateToggleStatus(m_Recipe.ShowEnableInspectionImage);
                baseToggleButtonUsePatternMatching.UpdateToggleStatus(m_Recipe.UsePatternMatching);

                baseToggleButtonFailedImageSave.UpdateToggleStatus(m_Recipe.SaveFaildImage);
                baseToggleButtonSaveRevisionImage.UpdateToggleStatus(m_Recipe.SaveRevisionImage);
            }

        }

        public void UpdateRecipeData()
        {
            if (m_Owner != null)
            {
                m_Recipe.ColletSize = Convert.ToInt32(baseTextBoxColletSize.Text);
                m_Recipe.ChipThres = Convert.ToInt32(baseTextBoxChipThres.Text);
                m_Recipe.EdgeAvgCount = Convert.ToInt32(baseTextBoxEdgeAvgCount.Text);
                m_Recipe.HighPassFilterOffset = Convert.ToDouble(baseTextBoxFilterOffset.Text);
                m_Recipe.ColletTolerenceX = Convert.ToDouble(baseTextBoxToleranceX.Text);

                m_Recipe.TartgetSizeWidth = Convert.ToInt32(baseTextBoxTargetWidth.Text);
                m_Recipe.TartgetSizeHight = Convert.ToInt32(baseTextBoxTargetHight.Text);

                m_Recipe.ShowEnableInspectionImage = baseToggleButtonShowEdgeImg.GetButtonStatus();
                m_Recipe.UsePatternMatching = baseToggleButtonUsePatternMatching.GetButtonStatus();

                m_Recipe.SaveFaildImage = baseToggleButtonFailedImageSave.GetButtonStatus();
                m_Recipe.SaveRevisionImage = baseToggleButtonSaveRevisionImage.GetButtonStatus();
            }
        }
        #endregion

        #region Event Handler
        private void baseToggleButtonFailedImageSave_Click(object sender, EventArgs e)
        {
            if (ButtonClick != null)
            {
                if (baseToggleButtonFailedImageSave.GetButtonStatus() == false)
                {
                    baseToggleButtonFailedImageSave.UpdateToggleStatus(true);
                    m_Recipe.SaveFaildImage = true;

                }
                else if (baseToggleButtonFailedImageSave.GetButtonStatus() == true)
                {
                    baseToggleButtonFailedImageSave.UpdateToggleStatus(false);
                    m_Recipe.SaveFaildImage = false;
                }
            }
            m_Recipe.SaveFaildImage = baseToggleButtonFailedImageSave.GetButtonStatus();
        }

        private void baseToggleButtonSaveRevisionImage_Click(object sender, EventArgs e)
        {
            if (ButtonClick != null)
            {
                if (baseToggleButtonSaveRevisionImage.GetButtonStatus() == false)
                {
                    baseToggleButtonSaveRevisionImage.UpdateToggleStatus(true);
                    m_Recipe.SaveRevisionImage = true;

                }
                else if (baseToggleButtonSaveRevisionImage.GetButtonStatus() == true)
                {
                    baseToggleButtonSaveRevisionImage.UpdateToggleStatus(false);
                    m_Recipe.SaveRevisionImage = false;

                }
            }
            m_Recipe.SaveRevisionImage = baseToggleButtonSaveRevisionImage.GetButtonStatus();
        }
        private void baseButtonSavaRecipe_Click(object sender, EventArgs e)
        {
            if (ButtonClick != null)
            {
                UpdateRecipeData();
                ButtonClick(ButtonType.SaveRecipe);
            }
        }
        private void baseButtonLoad_Click(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void baseToggleButtonShowEdgeImg_Click(object sender, EventArgs e)
        {
            if (ButtonClick != null)
            {
                if (baseToggleButtonShowEdgeImg.GetButtonStatus() == false)
                {
                    baseToggleButtonShowEdgeImg.UpdateToggleStatus(true);
                    m_Recipe.ShowEnableInspectionImage = true;

                }
                else if (baseToggleButtonShowEdgeImg.GetButtonStatus() == true)
                {
                    baseToggleButtonShowEdgeImg.UpdateToggleStatus(false);
                    m_Recipe.ShowEnableInspectionImage = false;
                }
            }
            m_Recipe.ShowEnableInspectionImage = baseToggleButtonShowEdgeImg.GetButtonStatus();
        }

        private void baseToggleButtonUsePatternMatching_Click(object sender, EventArgs e)
        {
            if (ButtonClick != null)
            {
                if (baseToggleButtonUsePatternMatching.GetButtonStatus() == false)
                {
                    baseToggleButtonUsePatternMatching.UpdateToggleStatus(true);
                    m_Recipe.UsePatternMatching = true;

                }
                else if (baseToggleButtonUsePatternMatching.GetButtonStatus() == true)
                {
                    baseToggleButtonUsePatternMatching.UpdateToggleStatus(false);
                    m_Recipe.UsePatternMatching = false;
                }
            }
            m_Recipe.UsePatternMatching = baseToggleButtonUsePatternMatching.GetButtonStatus();
        }
        #endregion

        private void baseButtonRun_Click(object sender, EventArgs e)
        {
            if(ButtonClick != null)
                ButtonClick(ButtonType.Run);
        }
    }
}
