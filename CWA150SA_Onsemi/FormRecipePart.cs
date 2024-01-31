using QMC.Common;
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
    #region Define
    public enum DatagridViewColumns
    {
        Owner,
        Name,
        Type,
        Value
    }
    #endregion

    public delegate void ParameterDelegate(RecipeParameters recipe);
    
    public partial class FormRecipePart : FormSubContentBase
    {
        #region Field
        public Module m_SelectedModule;

        public Part m_SelectedPart;
        #endregion

        #region Property
        public ParameterDelegate parameterDelegate { get; set; }
        public RecipeParameters recipeParameters { get; set; }
        public RecipeInfo CurrentRecipe { get; set; }
        public RecipeInfoCollection RecipeInfoCollection { get; set; }
        BindingSource DataGridViewBindingSource { get; set; }
        BindingSource ListBoxFunctionBindingSource { get; set; }
        BindingSource ListBoxActionBindingSource { get; set; }
        #endregion

        #region Constructor

        public FormRecipePart(Module module, RecipeInfo recipe)
            : base(FormType.withButton.ToString(), module.Name)
        {
            CurrentRecipe = recipe;
            m_SelectedModule = module;
            InitializeComponent();
            recipeParameters = new RecipeParameters();
            
            this.baseListBoxFunction.Size = new Size((Configuration.ContentSize.Width - 20) / 3 - Configuration.ButtonSize.Width * 2, this.panelContent.Size.Height - Configuration.PanelSize.Height * 3);
            this.baseListBoxFunction.Location = new Point(Configuration.ContentLocation.X + Configuration.ButtonSize.Width, Configuration.ContentLocation.Y + Configuration.PanelSize.Height * 2);

            this.baseListBoxAction.Size = new Size(this.baseListBoxFunction.Size.Width, this.baseListBoxFunction.Size.Height);
            this.baseListBoxAction.Location = new Point(this.baseListBoxFunction.Location.X + this.baseListBoxFunction.Size.Width + Configuration.ButtonSize.Width, Configuration.ContentLocation.Y + Configuration.PanelSize.Height * 2);

            this.baseDataGridViewActionParameter.Size = new Size((Configuration.ContentSize.Width - 20) / 2 - Configuration.ButtonSize.Width * 2, this.baseListBoxFunction.Size.Height);
            this.baseDataGridViewActionParameter.Location = new Point(this.baseListBoxAction.Location.X + this.baseListBoxAction.Size.Width + Configuration.ButtonSize.Width, Configuration.ContentLocation.Y + Configuration.PanelSize.Height * 2);

            ShowPartButton();
            InitDataGridviewParameterColumns();
        }
        #endregion


        #region Method

        public void ShowPartButton()
        {
         
            BaseButton[] control = new BaseButton[m_SelectedModule.Parts.Count];

            for (int i = 0; i < m_SelectedModule.Parts.Count; i++)
            {
                //if (m_SelectedModule.Parts[i].GetRecipeList().Count > 0)
                //{
                //    control[i] = new BaseButton();
                //    control[i].Parent = this;
                //    control[i].Size = new Size(Configuration.ButtonSize.Width, Configuration.ButtonSize.Height);
                //    control[i].Name = m_SelectedModule.Parts[i].Name.ToString();
                //    control[i].Text = m_SelectedModule.Parts[i].Name.ToString();
                //    control[i].Click += Button_Click;
                //    control[i].Tag = m_SelectedModule.Parts[i].Name;

                //    this.flowLayoutPanelButton.Controls.Add(control[i]);
                //}
            }
        }
        private void InitDataGridviewParameterColumns()
        {
            baseDataGridViewActionParameter.Columns.Clear();
            baseDataGridViewActionParameter.AutoGenerateColumns = false;
            foreach (DatagridViewColumns item in Enum.GetValues(typeof(DatagridViewColumns)))
            {
                DataGridViewColumn column = new DataGridViewColumn();
                if(item == DatagridViewColumns.Owner)
                {

                }
                else
                {
                    column.DataPropertyName = item.ToString();
                }
                
                column.HeaderText = item.ToString();
                column.Name = item.ToString();
                column.CellTemplate = new DataGridViewTextBoxCell();
                baseDataGridViewActionParameter.Columns.Add(column);
                baseDataGridViewActionParameter.Invalidate();
            }
        }
        public void UpdateListBox(Part part)
        {
            ListBoxFunctionBindingSource = new BindingSource();
            //ListBoxFunctionBindingSource.DataSource = part.GetFunctionList();
            baseListBoxFunction.DataSource = ListBoxFunctionBindingSource;
            baseListBoxFunction.DisplayMember = "Name";

        }
        public void SelectPart(Part part)
        {
            if (part != null)
            {
                m_SelectedPart = part;
                UpdateListBox(m_SelectedPart);
            }
        }

        #endregion

        #region Event Handler

        public void Button_Click(object sender, EventArgs e)
        {
            //BaseButton button = sender as BaseButton;
            //for (int i = 0; i < CurrentRecipe.RecipeParameterCollection.Count; i++)
            //{
            //    if (CurrentRecipe.RecipeParameterCollection[i].Part == null)
            //    {
            //        baseListBoxFunction.DataSource = null;
            //        return;
            //    }
            //    if (button.Name == CurrentRecipe.RecipeParameterCollection[i].Part.Name)
            //    {
            //        SelectPart(CurrentRecipe.RecipeParameterCollection[i].Part);
            //        break;
            //    }
            //}
        }

        private void baseListBoxFunction_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Function function = baseListBoxFunction.SelectedItem as Function;
            //if (function != null)
            //{
            //    ListBoxActionBindingSource = new BindingSource();
            //    ListBoxActionBindingSource.DataSource = function.GetRecipeActions();
            //    baseListBoxAction.DataSource = ListBoxActionBindingSource;
            //    baseListBoxAction.DisplayMember = "Name";
            //}
            //if(baseListBoxAction.Items.Count == 0)
            //{
            //    baseDataGridViewActionParameter.DataSource = null;
            //}
        }

        private void baseListBoxAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            Action action = baseListBoxAction.SelectedItem as Action;
            if (action != null)
            {
                SettingParameterCollection parameterBox = new SettingParameterCollection();
                SettingParameterCollection parameter = new SettingParameterCollection();

                DataGridViewBindingSource = new BindingSource();
                DataGridViewBindingSource.DataSource = parameter;
                baseDataGridViewActionParameter.DataSource = DataGridViewBindingSource;
            }            
        }
        private void baseDataGridViewActionParameter_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            SettingParameter action = baseDataGridViewActionParameter.Rows[e.RowIndex].DataBoundItem as SettingParameter;
            if (action != null )
            {
                baseDataGridViewActionParameter.Rows[e.RowIndex].Cells["Owner"].Value = action.Owner.Name;
            }
        }

        #endregion

    }
}
