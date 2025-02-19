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

namespace SLD200_MSL
{
    public partial class FunctionControl : UserControl
    {
        FormBaseConfiguration m_Configuration { get; set; }
        //List<Function> m_Functions = new List<Function>();

        Action m_Action;
        Module m_OwnerModule;
        Part m_Part;
        public BaseDataGridView dataGridViewJogControl { get; set; }

        public FunctionControl(Part part)
        {
            m_Configuration = new FormBaseConfiguration();
            if (part is Module)
            {
                m_OwnerModule = (Module)part;
                m_Part = part;
            }
            //else
            //{
            //    m_OwnerModule = part.GetOwner() as Module;
            //    m_Part = part;

            //}
            InitializeComponent();
            
            Equipment.GetCurrentRecipe();

            //m_Functions = m_Part.GetFunctionList();
            //foreach (Function function in m_Functions)
            //{
            //    List<Action> actions = function.GetRecipeActions();
            //    foreach (Action action in actions)
            //    {
            //        //          action.Parameters
            //    }

            //}
            //listBoxFunction.DataSource = null;
            //listBoxFunction.DataSource = m_Functions;
            //listBoxFunction.DisplayMember = "Name";
            


            InitdataGridViewParameterColumns();
            UpdateDataGridViewValue();

            

        }
        private void listBoxFunction_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxFunction.Items.Count == 0)
            {
                return;
            }

            dataGridViewActionParameter.DataSource = null;
            listBoxAction.DataSource = null;
            //listBoxAction.DataSource = m_Functions[listBoxFunction.SelectedIndex].Actions;
            listBoxAction.DisplayMember = "Name";


        }

        public void listBoxAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxAction.Items.Count == 0 || listBoxAction.SelectedIndex < 0)
            {
                return;
            }
            dataGridViewActionParameter.DataSource = null;

            //Function function = m_Functions[listBoxFunction.SelectedIndex];
            //m_Action = function.Actions[listBoxAction.SelectedIndex];



            UpdateDataGridViewValue();
        }
        private void InitdataGridViewParameterColumns()
        {
            dataGridViewActionParameter.Columns.Clear();
            dataGridViewActionParameter.AutoGenerateColumns = false;
            foreach (DatagridViewColumns item in Enum.GetValues(typeof(DatagridViewColumns)))
            {
                DataGridViewColumn column = new DataGridViewColumn();
                if (item == DatagridViewColumns.Owner)
                {

                }
                else
                {
                    column.DataPropertyName = item.ToString();
                }

                column.HeaderText = item.ToString();
                column.Name = item.ToString();
                column.CellTemplate = new DataGridViewTextBoxCell();
                dataGridViewActionParameter.Columns.Add(column);
                dataGridViewActionParameter.Invalidate();
            }
        }
        private void dataGridViewActionParameter_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

            Action action = listBoxAction.SelectedItem as Action;
            //if (action != null && action.Parameters != null && action.Parameters.Count > e.RowIndex)
            //{
            //    ActionParameter parameter = action.Parameters[e.RowIndex];
            //    if (parameter.Owner == null)
            //    {
            //        return;
            //    }
            //    dataGridViewActionParameter.Rows[e.RowIndex].Cells["Owner"].Value = parameter.Owner.Name;
            //}

        }

        public void UpdateDataGridViewValue()
        {
            m_Action = listBoxAction.SelectedItem as Action;
            dataGridViewActionParameter.DataSource = null;
            //dataGridViewActionParameter.DataSource = m_Action.Parameters;
            //if (m_Action != null && m_Action.Parameters.Count > 0)
            //{
            //    for (int i = 0; i < m_Action.Parameters.Count; i++)
            //    {
            //        if (m_Action.Parameters[i].ParameterType == ParameterType.Position)
            //        {
            //            if (dataGridViewActionParameter.Columns.Count < 5)
            //            {
            //                {
            //                    DataGridViewColumn column = new DataGridViewTextBoxColumn();
            //                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //                    column.Name = "SetCurrent";
            //                    dataGridViewActionParameter.Columns.Add(column);
            //                }
            //            }
            //            DataGridViewButtonCell buttonCellSetCurrent = new DataGridViewButtonCell();
            //            buttonCellSetCurrent.Value = "Set Current";
                     
            //            if (dataGridViewActionParameter.ColumnCount > 0 && dataGridViewActionParameter.RowCount > 0)
            //            {
            //                this.dataGridViewActionParameter.Rows[i].Cells[dataGridViewActionParameter.ColumnCount - 1] = buttonCellSetCurrent;
            //            }


            //        }
            //    }
            //}
            
        }
        private void baseButtonSave_Click(object sender, EventArgs e)
        {
            if (m_OwnerModule != null && m_Part != null)
            {

                string strActionName = string.Empty;
                if (m_Part is Module)
                {

                }
                else
                {
                    //foreach (Function function in m_Part.GetFunctionList())
                    //{
                        
                    //    foreach (Action action1 in function.Actions)
                    //    {
                    //        ActionParameterCollection actionParameters = new ActionParameterCollection();
                    //        ActionParameterCollection recipeParameters = new ActionParameterCollection();
                    //        if (action1.Parameters.Count > 0)
                    //        {
                    //            strActionName = action1.Name;
                    //            for (int i = 0; i < action1.Parameters.Count; i++)
                    //            {
                    //                if (!action1.Parameters[i].IsRecipe)
                    //                {
                    //                    actionParameters.Add(action1.Parameters[i]);

                    //                }
                    //                else
                    //                {
                    //                    recipeParameters.Add(action1.Parameters[i]);
                    //                }
                    //            }
                                
                    //        }
                    //        else
                    //        {
                    //            break;
                    //        }
                    //        if (recipeParameters.Count > 0)
                    //        {
                    //            RecipeInfo recipe = Equipment.GetCurrentRecipe();

                    //            recipe.RecipeParameterCollection.SetRecipeParameter(m_OwnerModule.Name, m_Part.Name, recipeParameters);

                    //        }
                    //        if (actionParameters.Count > 0)
                    //        {
                    //            DataManager.Instance.Config.SetParameters(m_OwnerModule.Name, m_Part.Name, function.Name, strActionName, actionParameters);
                    //            Equipment.ApplyRecipeData();
                    //        }
                    //    }
                        
                        
                    //}
                }
                    Equipment.SaveConfig();
                    Equipment.SaveRecipe();
            }

        }

        private void baseButtonSetCurrent_Click(object sender, EventArgs e)
        {
            m_Action = listBoxAction.SelectedItem as Action;

        }

        private void baseButtonRun_Click(object sender, EventArgs e)
        {

            //Function function = listBoxFunction.SelectedItem as Function;
            //if (function != null)
            //{
            //    AsyncMethod method = new AsyncMethod();
            //    method.Func = function;
            //    method.SetStopProcedure(new IntResultDelegate(m_Part.StopExecute), null);
            //    AsyncResult result = method.Run();
            //    ProgressForm ProgressForm = new ProgressForm(function.Name, "Function Is Running...", result);
            //    ProgressForm.StartPosition = FormStartPosition.CenterScreen;
            //    ProgressForm.ShowDialog();
            //}
        }

        private void baseButtonActionRun_Click(object sender, EventArgs e)
        {
            //Function function = listBoxFunction.SelectedItem as Function;
            //int index = listBoxAction.SelectedIndex;
            //if (function != null)
            //{
            //    AsyncMethod method = new AsyncMethod();
            //    method.Timeout = function.TimeOut;
            //    method.SetRunProcedure(new IntResultDelegate(function.Actions[index].Execute), null);
            //    method.SetStopProcedure(new IntResultDelegate(m_Part.StopExecute), null);
            //    AsyncResult result = method.Run();
            //    ProgressForm ProgressForm = new ProgressForm(function.Actions[index].Name, "Function Is Running...", result);
            //    ProgressForm.StartPosition = FormStartPosition.CenterScreen;
            //    ProgressForm.ShowDialog();
                
            //}
        }
        
        private void dataGridViewActionParameter_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            if (senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewButtonCell &&
                e.RowIndex >= 0)
            {
                if (m_Part is Module)
                {

                }
                else
                {
                    //List<MotionAxis> motionAxes = m_Part.GetMotionList();
                    //ActionParameter actionParameter = senderGrid.Rows[e.RowIndex].DataBoundItem as ActionParameter;
                    //for (int i = 0; i < motionAxes.Count; i++)
                    //{
                    //    if (motionAxes[i].ToString() == actionParameter.Owner.ToString())
                    //    {
                    //        actionParameter.Value = motionAxes[i].Motor.ActualPosition.ToString();
                    //        break;
                    //    }
                    //}

                    //dataGridViewActionParameter.DataSource = null;
                    //dataGridViewActionParameter.DataSource = m_Action.Parameters;

                    UpdateDataGridViewValue();
                }


            }
        }
    }
}
