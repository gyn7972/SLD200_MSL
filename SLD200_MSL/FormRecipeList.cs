using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QMC.Common;
using QMC.Common.Modules;
using MessageBoxYesNo = QMC.Core.MessageBoxYesNo;

namespace SLD200_MSL
{
    public delegate void ChangeRecipeEventHandler();

    public partial class FormRecipeList : Form
    {
        #region Define
        private enum CreateRecipeBottomButton
        {
            Create, Modify, Delete, Copy, Paste
        }
        private enum RecipeDataGridCloumnList
        {
            No, Name, EditTime, EditedBy, Description
        }
        #endregion

        #region Field

        private int m_nIndex = 0;
        public RecipeInfoCollection m_recipes;
        public RecipeInfo m_recipe;
        private RecipeInfo m_copyRecipe;
        public event ChangeRecipeEventHandler ChangedCurrentRecipe;
        #endregion

        #region Property
        public FormAddRecipeInfo FormAddRecipeName { get; set; }
        public FormBaseConfiguration FormBaseConfiguration { get; set; }
        #endregion
        public FormRecipeList() : this(null)
        {

        }

        public FormRecipeList(RecipeInfoCollection recipeInfos)
        {
            InitializeComponent();
            FormBaseConfiguration = new FormBaseConfiguration();
            FormAddRecipeName = new FormAddRecipeInfo();
            m_recipes = recipeInfos;
            CreatBottomFlowPanelButton();
            CreateDataGridColumns();
            UpdateDataGrid();
            
            FormAddRecipeName.eAddRecipeInfo += button_Click;
            #region ControlConstructor  

            this.Text = "Recipe List";
            this.Size = new Size(FormBaseConfiguration.ContentSize.Width / 2, FormBaseConfiguration.ContentSize.Height / 2 + FormBaseConfiguration.ButtonSize.Height * 3);
            this.BackColor = Color.FromArgb(38, 38, 38);

            this.flowLayoutPanelTop.Size = new Size(FormBaseConfiguration.ContentSize.Width / 2 - FormBaseConfiguration.ButtonSize.Width * 2, FormBaseConfiguration.PanelbuttonSize.Height);
            this.flowLayoutPanelTop.Location = new Point(0, 0);
            this.flowLayoutPanelTop.BackColor = Color.FromArgb(38, 38, 38);

            this.baseDataGridViewRecipeList.Size = new Size(FormBaseConfiguration.ContentSize.Width / 2 - 16, this.Height - this.flowLayoutPanelTop.Height * 2 - this.flowLayoutPanelControlButton.Height * 2 - 19);
            this.baseDataGridViewRecipeList.Location = new Point(this.flowLayoutPanelTop.Location.X, this.flowLayoutPanelTop.Height + 5);

            this.flowLayoutPanelControlButton.Size = new Size(FormBaseConfiguration.ContentSize.Width / 2 - 16, FormBaseConfiguration.PanelbuttonSize.Height + 4);
            this.flowLayoutPanelControlButton.Location = new Point(this.flowLayoutPanelTop.Location.X, this.baseDataGridViewRecipeList.Height + this.flowLayoutPanelTop.Height + 15);
            this.flowLayoutPanelControlButton.BackColor = Color.FromArgb(38, 38, 38);

            this.baseButtonCancel.Size = FormBaseConfiguration.RecipeButtonSize; //new Size(FormBaseConfiguration.ButtonSize.Width, FormBaseConfiguration.ButtonSize.Height);
            this.baseButtonCancel.Location = new Point(this.baseDataGridViewRecipeList.Width - this.baseButtonCancel.Width - 10, this.flowLayoutPanelTop.Location.Y + 3);

            this.baseButtonOk.Size = FormBaseConfiguration.RecipeButtonSize;
            this.baseButtonOk.Location = new Point(this.baseButtonCancel.Location.X - FormBaseConfiguration.ButtonSize.Width - 3, this.baseButtonCancel.Location.Y);

            #endregion

        }



        #region Method

        #region CreateDataGridColumns
        public void CreateDataGridColumns()
        {
            baseDataGridViewRecipeList.Columns.Clear();
            {
                foreach (RecipeDataGridCloumnList item in Enum.GetValues(typeof(RecipeDataGridCloumnList)))
                {
                    DataGridViewColumn column = new DataGridViewColumn();
                    column.HeaderText = item.ToString();
                    column.DataPropertyName = item.ToString();
                    column.Name = item.ToString();
                    column.CellTemplate = new DataGridViewTextBoxCell();

                    baseDataGridViewRecipeList.Columns.Add(column);
                }
            }
        }
        #endregion

        #region CreatBottomFlowPanelButton
        public void CreatBottomFlowPanelButton()
        {
            Point buttonInterlockLocation = new Point();
            buttonInterlockLocation.X = 0;
            buttonInterlockLocation.Y = 0;
            foreach (CreateRecipeBottomButton buttonInterlock in Enum.GetValues(typeof(CreateRecipeBottomButton)))
            {
                BaseButton btn = new BaseButton();
                switch (buttonInterlock)
                {
                    case CreateRecipeBottomButton.Create:
                        btn.Text = buttonInterlock.ToString();
                        btn.Name = string.Format("button", buttonInterlock.ToString());
                        btn.Size = new System.Drawing.Size(FormBaseConfiguration.ButtonSize.Width, FormBaseConfiguration.ButtonSize.Height);
                        btn.Location = buttonInterlockLocation;
                        btn.Click += CreateButton_Click;
                        break;
                    case CreateRecipeBottomButton.Modify:
                        btn.Text = buttonInterlock.ToString();
                        btn.Name = string.Format("button", buttonInterlock.ToString());
                        btn.Size = new System.Drawing.Size(FormBaseConfiguration.ButtonSize.Width, FormBaseConfiguration.ButtonSize.Height);
                        btn.Location = buttonInterlockLocation;
                        btn.Click += ModifyButton_Click;
                        break;
                    case CreateRecipeBottomButton.Delete:
                        btn.Text = buttonInterlock.ToString();
                        btn.Name = string.Format("button", buttonInterlock.ToString());
                        btn.Size = new System.Drawing.Size(FormBaseConfiguration.ButtonSize.Width, FormBaseConfiguration.ButtonSize.Height);
                        btn.Location = buttonInterlockLocation;
                        btn.Click += DeleteButton_Click;
                        break;
                    case CreateRecipeBottomButton.Copy:
                        btn.Text = buttonInterlock.ToString();
                        btn.Name = string.Format("button", buttonInterlock.ToString());
                        btn.Size = new System.Drawing.Size(FormBaseConfiguration.ButtonSize.Width, FormBaseConfiguration.ButtonSize.Height);
                        btn.Location = buttonInterlockLocation;
                        btn.Click += CopyButton_Click;
                        break;
                    case CreateRecipeBottomButton.Paste:
                        btn.Text = buttonInterlock.ToString();
                        btn.Name = string.Format("button", buttonInterlock.ToString());
                        btn.Size = new System.Drawing.Size(FormBaseConfiguration.ButtonSize.Width, FormBaseConfiguration.ButtonSize.Height);
                        btn.Location = buttonInterlockLocation;
                        btn.Click += PasteButton_Click;
                        break;
                }
                flowLayoutPanelControlButton.Controls.Add(btn);
            }
        }
        #endregion

        #region ShowForm
        public void ShowForm(Form form)
        {
            form.StartPosition = FormStartPosition.CenterScreen;
            form.ShowDialog();
        }
        #endregion

        #region UpdateDataGrid
        private void UpdateDataGrid()
        {
            baseDataGridViewRecipeList.Rows.Clear();
            if (m_recipes != null)
            {
                for (int i = 0; i < m_recipes.Count; i++)
                {
                    baseDataGridViewRecipeList.Rows.Add();
                    if(m_recipes[i] != null)
                    {
                        baseDataGridViewRecipeList[(int)RecipeDataGridCloumnList.No, i].Value = i;
                        baseDataGridViewRecipeList[(int)RecipeDataGridCloumnList.Name, i].Value = m_recipes[i].Name;
                        baseDataGridViewRecipeList[(int)RecipeDataGridCloumnList.EditTime, i].Value = m_recipes[i].strEditTime();
                        baseDataGridViewRecipeList[(int)RecipeDataGridCloumnList.Description, i].Value = m_recipes[i].Description;
                        baseDataGridViewRecipeList[(int)RecipeDataGridCloumnList.EditedBy, i].Value = m_recipes[i].EditBy;
                    }
                    
                }
                if (baseDataGridViewRecipeList.RowCount > 0)
                {
                    baseDataGridViewRecipeList[0, m_nIndex].Selected = true;
                }
            }
        }
        #endregion

        #endregion


        #region EventHandler

        #region BaseDataGridViewRecipeList_DoubleClick
        private void BaseDataGridViewRecipeList_DoubleClick(object sender, System.EventArgs e)
        {
            if (baseDataGridViewRecipeList.Rows.Count == 0)
            {
                return;
            }

            int nRow = baseDataGridViewRecipeList.SelectedCells[0].RowIndex;
            m_recipe = m_recipes[nRow];

            DialogResult = DialogResult.OK;
        }
        #endregion

        #region baseButtonCancel_Click
        private void baseButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        #endregion

        #region button_Click
        private void button_Click(ControlButtonList type)
        {
            switch (type)
            {
                case ControlButtonList.OK:
                    ApplyRecipe();
                    break;
                case ControlButtonList.Cancel:
                    CancelButton_Click();
                    break;
            }
        }
        #endregion

        #region ApplyRecipe
        private void ApplyRecipe()
        {
            this.m_recipe = FormAddRecipeName.m_recipe;
            Equipment.SetCurrentRecipe(this.m_recipe);
        }
        #endregion

        #region ModifyButton_Click
        public void ModifyButton_Click(object sender, EventArgs e)
        {
            string m_strRecipeName_Before = "";
            string m_strRecipeName_After = "";
            string m_strImage_srcPAK = "";
            string m_strImage_srcWafer = "";
            string m_strImage_destPAK = "";
            string m_strImage_destWafer = "";

            FormAddRecipeInfo FormAddRecipeName = new FormAddRecipeInfo();
            if (baseDataGridViewRecipeList.RowCount > 0)
            {
                m_nIndex = baseDataGridViewRecipeList.SelectedCells[0].RowIndex;
                FormAddRecipeName.m_recipe = m_recipes[m_nIndex];
                FormAddRecipeName.StartPosition = FormStartPosition.CenterScreen;

                m_strRecipeName_Before = FormAddRecipeName.m_recipe.Name;

                if (FormAddRecipeName.ShowDialog() == DialogResult.OK)
                {
                    m_recipes[m_nIndex].Name = FormAddRecipeName.m_recipe.Name;
                    m_recipes[m_nIndex].Description = FormAddRecipeName.m_recipe.Description;

                    m_strRecipeName_After = FormAddRecipeName.m_recipe.Name;

                    //  패턴매칭 이미지가 존재하면, 이미지 파일명도 변경한다.
                    m_strImage_srcPAK = string.Format("{0}\\{1}_PAK.jpg", ConfigManager.GetPatternImagePath(), m_strRecipeName_Before);
                    m_strImage_destPAK = string.Format("{0}\\{1}_PAK.jpg", ConfigManager.GetPatternImagePath(), m_strRecipeName_After);
                    if (File.Exists(m_strImage_srcPAK))
                    {
                        try
                        {
                            File.Move(m_strImage_srcPAK, m_strImage_destPAK);

                            if (File.Exists(m_strImage_srcPAK) || !File.Exists(m_strImage_destPAK))
                            {
                                MessageBox.Show("[PAK] 패턴 이미지 파일명 변경 실패.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch (IOException ex)
                        {
                            Console.WriteLine("The PAK renaming failed: {0}", ex.ToString());
                        }
                    }

                    m_strImage_srcWafer = string.Format("{0}\\{1}_Wafer.jpg", ConfigManager.GetPatternImagePath(), m_strRecipeName_Before);
                    m_strImage_destWafer = string.Format("{0}\\{1}_Wafer.jpg", ConfigManager.GetPatternImagePath(), m_strRecipeName_After);
                    if (File.Exists(m_strImage_srcWafer))
                    {
                        try
                        {
                            File.Move(m_strImage_srcWafer, m_strImage_destWafer);

                            if (File.Exists(m_strImage_srcWafer) || !File.Exists(m_strImage_destWafer))
                            {
                                MessageBox.Show("[Wafer] 패턴 이미지 파일명 변경 실패.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch (IOException ex)
                        {
                            Console.WriteLine("The Wafer renaming failed: {0}", ex.ToString());
                        }
                    }
                }
                UpdateDataGrid();
            }
        }
        #endregion

        #region CreateButton_Click
        public void CreateButton_Click(object sender, EventArgs e)
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "현재 설정으로 레시피를 생성하시겠습니까?"))
                return;

            FormAddRecipeInfo FormAddRecipeName = new FormAddRecipeInfo();
            FormAddRecipeName.StartPosition = FormStartPosition.CenterScreen;
            if (FormAddRecipeName.ShowDialog() == DialogResult.OK)
            {
                RecipeInfo newRecipe = FormAddRecipeName.m_recipe;
                Equipment.CreateRecipes(newRecipe);
                DataManager.Instance.Recipe.Add(newRecipe);
                m_recipes = DataManager.Instance.Recipe;
                UpdateDataGrid();
            }
        }
        #endregion

        #region DeleteButton_Click
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            bool m_bPAK_PatternImage_Delete_Success = false;
            bool m_bWafer_PatternImage_Delete_Success = false;

            if (baseDataGridViewRecipeList.RowCount > 0)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "선택하신 레시피를 삭제하시겠습니까?"))
                    return;

                m_nIndex = baseDataGridViewRecipeList.SelectedCells[0].RowIndex;

                //  삭제하려는 Recipe Name
                string strRecipeName = m_recipes[m_nIndex].Name;

                m_recipes.RemoveAt(m_nIndex);
                baseDataGridViewRecipeList.Rows.RemoveAt(m_nIndex);


                //  패턴매칭 이미지가 존재하면, 이미지 파일도 삭제한다.
                string m_strImage_PAK = string.Format("{0}\\{1}_PAK.jpg", ConfigManager.GetPatternImagePath(), strRecipeName);
                string m_strImage_Wafer = string.Format("{0}\\{1}_Wafer.jpg", ConfigManager.GetPatternImagePath(), strRecipeName);

                if (File.Exists(m_strImage_PAK))
                {
                    try
                    {
                        File.Delete(m_strImage_PAK);
                        m_bPAK_PatternImage_Delete_Success = true;

                        if (File.Exists(m_strImage_PAK))
                        {
                            m_bPAK_PatternImage_Delete_Success = false;

                            MessageBox.Show("[PAK] 패턴 이미지 파일 삭제 실패.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine("Failed to delete PAK pattern image file");
                    }
                }
                else
                {
                    m_bPAK_PatternImage_Delete_Success = true;
                }

                if (File.Exists(m_strImage_Wafer))
                {
                    try
                    {
                        File.Delete(m_strImage_Wafer);
                        m_bWafer_PatternImage_Delete_Success = true;

                        if (File.Exists(m_strImage_Wafer))
                        {
                            m_bWafer_PatternImage_Delete_Success = false;

                            MessageBox.Show("[Wafer] 패턴 이미지 파일 삭제 실패.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine("Failed to delete Wafer pattern image file");
                    }
                }
                else
                {
                    m_bWafer_PatternImage_Delete_Success = true;
                }

                //  패턴 이미지 파일 삭제 성공 여부에 따라 결과 메시지 출력
                if (m_bPAK_PatternImage_Delete_Success && m_bWafer_PatternImage_Delete_Success)
                {
                    MessageBox.Show("패턴 이미지 파일 삭제 완료.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                //else
                //{
                //    MessageBox.Show("패턴 이미지 파일 삭제 실패.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}
            }
        }
        #endregion

        #region CopyButton_Click
        public void CopyButton_Click(object sender, EventArgs e)
        {
            if (baseDataGridViewRecipeList.RowCount > 0)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "선택하신 레시피의 설정값을 클립보드로 복사하시겠습니까?\r\n\r\n[\"Paste\" 하여 레시피 생성]"))
                    return;

                m_copyRecipe = new RecipeInfo();
                m_nIndex = baseDataGridViewRecipeList.SelectedCells[0].RowIndex;

                m_copyRecipe = m_recipes[m_nIndex].DeepCopy();
            }
        }
        #endregion

        #region PasteButton_Click
        public void PasteButton_Click(object sender, EventArgs e)
        {
            if (baseDataGridViewRecipeList.RowCount > 0 && m_copyRecipe != null)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "클립보드에 복사된 설정값으로 레시피를 생성(복사) 하시겠습니까?\r\n\r\n[\"Copy\" 한 레시피 설정값]"))
                    return;

                RecipeInfo newRecipe = m_copyRecipe.DeepCopy();
                char[] chars = "_".ToCharArray();
                string str = m_recipes[m_nIndex].Name.Split(chars).Last();
                string str1 = m_recipes[m_nIndex].Name.Split(chars).First();
                if (str != str1)
                {
                    if (chars.Length >= 1)
                    {
                        newRecipe.Name = m_recipes[m_nIndex].Name + "_" + "1";
                        FindRecipeName(newRecipe);
                    }
                    else
                    {
                        newRecipe.Name = str1.ToString() + "_" + (int.Parse(str) + 1).ToString();
                        for (int i = 0; i < m_recipes.Count; i++)
                        {
                            if (m_recipes[i].Name == newRecipe.Name)
                            {
                                newRecipe.Name = str1.ToString() + "_" + (int.Parse(str) + 1).ToString();
                            }
                        }
                    }
                }
                else
                {
                    newRecipe.Name = m_recipes[m_nIndex].Name + "_" + "1";

                    for (int i = 0; i < m_recipes.Count; i++)
                    {
                        if (m_recipes[i].Name == newRecipe.Name)
                        {
                            newRecipe.Name = m_recipes[i].Name.Split(chars).First().ToString() + "_" + (int.Parse(m_recipes[i].Name.Split(chars).Last()) + 1).ToString();
                        }
                    }
                }
                m_recipes.Add(newRecipe);
                UpdateDataGrid();
            }
        }

        public string FindRecipeName(RecipeInfo newRecipe)
        {
            foreach (RecipeInfo recipeList in m_recipes)
            {
                char[] chars = "_".ToCharArray();
                if (recipeList.Name == newRecipe.Name)
                {
                    int uNameLength = recipeList.Name.Length;
                    int uLastLength = recipeList.Name.Split(chars).Last().ToString().Length;
                    newRecipe.Name = recipeList.Name.Remove(uNameLength - uLastLength) + (int.Parse(recipeList.Name.Split(chars).Last()) + 1).ToString();
                    FindRecipeName(newRecipe);
                }
            }
            return m_copyRecipe.Name;
        }
        #endregion

        #region CancelButton_Click
        private void CancelButton_Click()
        {
            DialogResult = DialogResult.Cancel;
        }
        #endregion

        #region baseButtonOk_Click
        private void baseButtonOk_Click(object sender, EventArgs e)
        {
            if (baseDataGridViewRecipeList.Rows.Count == 0)
            {
                return;
            }
            
            int nRow = baseDataGridViewRecipeList.SelectedCells[0].RowIndex;
            m_recipe = m_recipes[nRow];
            
            DialogResult = DialogResult.OK;
        }
        #endregion

        #endregion


    }

}
