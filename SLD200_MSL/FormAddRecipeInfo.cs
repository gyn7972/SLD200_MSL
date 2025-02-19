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

namespace SLD200_MSL
{
    #region Define
    public enum ControlButtonList
    {
        OK, Cancel
    }
    #endregion

    public delegate void AddRecipeEventHandler(ControlButtonList type);
    public partial class FormAddRecipeInfo : Form
    {
        public event AddRecipeEventHandler eAddRecipeInfo;
        public FormBaseConfiguration FormBaseConfiguration { get; set; }
        public RecipeInfo m_recipe { get; set; }

        public FormAddRecipeInfo()
        {
            InitializeComponent();
            m_recipe = new RecipeInfo();
            #region ControlConstructor
            this.MaximumSize = new Size(450, 250);
            this.MinimumSize = new Size(450, 250);
            this.BackColor = Color.FromArgb(38, 38, 38);
            this.Text = "Create Recipe";

            this.baseTextBoxCreateRecipeName.ForeColor = Color.White;
            this.baseTextBoxCreateRecipeName.Font = new Font(baseTextBoxCreateRecipeName.Font.FontFamily, 15);

            this.baseTextBoxDescription.ForeColor = Color.White;
            this.baseTextBoxDescription.Font = new Font(baseTextBoxCreateRecipeName.Font.FontFamily, 15);

            this.baseButtonAdd.Text = "OK";
            this.baseButtonCancel.Text = "Cancel";
            this.baseTextBoxCreateRecipeName.Text = null;
            this.baseTextBoxDescription.Text = null;
            #endregion

            this.Load += RecipeLoad;
        }


        #region EventHandler
        private void baseButtonOK_Click(object sender, EventArgs e)
        {
            if (baseTextBoxCreateRecipeName.Text != "")
            {
                m_recipe = new RecipeInfo();
                m_recipe.Name = baseTextBoxCreateRecipeName.Text;
                m_recipe.Description = baseTextBoxDescription.Text;
                m_recipe.EditBy = " ";
                DialogResult = DialogResult.OK;
                if (eAddRecipeInfo != null)
                {
                    eAddRecipeInfo(ControlButtonList.OK);
                }
            }            
        }

        private void baseButtonCancel_Click(object sender, EventArgs e)
        {
            baseTextBoxCreateRecipeName.Text = null;
            baseTextBoxDescription.Text = null;
            DialogResult = DialogResult.Cancel;
            if (eAddRecipeInfo != null)
            {
                eAddRecipeInfo(ControlButtonList.Cancel);
            }
        }
        #endregion

        #region Method

        private void LoadModifyInfo()
        {
            baseTextBoxCreateRecipeName.Text = m_recipe.Name;
            baseTextBoxDescription.Text = m_recipe.Description;
        }

        private void RecipeLoad(object sender, EventArgs e)
        {
            if (m_recipe == null)
            {
                m_recipe = new RecipeInfo();
            }
            else
            {
                LoadModifyInfo();
            }
        }
        #endregion
    }
}
