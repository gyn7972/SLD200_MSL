using QMC.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;


namespace QMC.Common.Hmi
{

    public delegate void ButtonClickEvent();
    public class UIMakerEditor : UITypeEditor
    {
        public ButtonClickEvent ButtonClickEvent;
        private Part m_SelectedPart;
        private Form m_form = new Form();
        private TreeView m_TreeView = new TreeView();
        private Module m_SeletedModule;
        private Part m_SelectedParentPart;
        private List<int> m_nindex = new List<int>();
        private ModuleCollection m_collectionModules;

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }


        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService wfes = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            if (wfes != null)
            {
                /////////////////////////////////////////////////////
                //폼 클래스로 만들기 바람.
                /////////////////////////////////////////////////////
                m_form.Size = new System.Drawing.Size(500, 800);
                
                m_TreeView.Size = new System.Drawing.Size(485,700);
                m_TreeView.Location = new System.Drawing.Point(0,0);
                m_TreeView.Font = new System.Drawing.Font(m_TreeView.Font.FontFamily, 20);
                m_TreeView.AfterSelect += treeViewFunction_AfterSelect;

                Button buttonOK = new Button();
                buttonOK.Size = new System.Drawing.Size(120,50);
                buttonOK.Location = new System.Drawing.Point(m_TreeView.Location.X + 100, m_TreeView.Location.Y + m_TreeView.Height + 5);
                buttonOK.Text = "Add";
                buttonOK.Click += buttonAdd_Click;

                Button buttonCancel = new Button();
                buttonCancel.Size = new System.Drawing.Size(120,50);
                buttonCancel.Location = new System.Drawing.Point(buttonOK.Location.X + buttonOK.Width + 50, buttonOK.Location.Y);
                buttonCancel.Text = "Cancel";
                buttonCancel.Click += buttonCancel_Click;

                m_form.Controls.Add(m_TreeView);
                m_form.Controls.Add(buttonOK);
                m_form.Controls.Add(buttonCancel);

                ModuleCollection moduels = Equipment.Modules;
                if (moduels != null)
                {
                    InitTreeView(m_TreeView, "CWA-150SA_Onsemi", moduels);
                }
                /////////////////////////////////////////////////////////
                if(wfes.ShowDialog(m_form) == DialogResult.OK)
                {
                    value = m_SelectedPart;
                }
            }

            return value;
        }

        #region InitTreeView
        public void InitTreeView(TreeView treeView, string strEquipmentName, ModuleCollection modules)
        {
            treeView.Nodes.Clear();
            string strEq = string.Format("Equipment({0})", strEquipmentName);
            TreeNode nodeEquipment = treeView.Nodes.Add(strEq);
            m_collectionModules = Equipment.Modules;
            foreach (Module module in modules)
            {
                TreeNode nodeModule = nodeEquipment.Nodes.Add(module.Name);
                foreach (Part part in module.Parts)
                {
                    AddPartNode(part, nodeModule);
                }
            }
        }

        private void AddPartNode(Part part, TreeNode node)
        {
            //TreeNode partNode = node.Nodes.Add(part.Name);
            //foreach (Part sub in part.Parts)
            //{
            //    AddPartNode(sub, partNode);
            //}
        }
        #endregion

        #region treeViewFunction_AfterSelect
        private void treeViewFunction_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode selectedNode = m_TreeView.SelectedNode;
            if (selectedNode != null)
            {
                m_nindex.Clear();
                RecursivefindParents(selectedNode);


                for (int i = 0; i < m_nindex.Count; i++)
                {
                    int nIter = m_nindex[i];
                    if (i == 0)
                    {
                        m_SeletedModule = null;
                        m_SelectedPart = null;
                        m_SelectedParentPart = null;
                    }
                    else if (i == 1)
                    {
                        m_SeletedModule = m_collectionModules[nIter];

                    }
                    else if (i == 2)
                    {
                        m_SelectedPart = m_SeletedModule.Parts[nIter];
                    }
                    else
                    {
                        m_SelectedParentPart = m_SelectedPart;
                        //m_SelectedPart = m_SelectedParentPart.Parts[nIter];
                    }
                }               
            }
        }
        #endregion

        #region RecursiveFindParents
        public void RecursivefindParents(TreeNode treeNode)
        {
            if (treeNode.Parent != null)
            {
                RecursivefindParents(treeNode.Parent);
            }
            m_nindex.Add(treeNode.Index);
        }
        #endregion

        #region buttonAdd_Click
        public void buttonAdd_Click(object sender, EventArgs e)
        {
            m_form.DialogResult = DialogResult.OK;
        }
        #endregion


        #region buttonCancel_Click
        public void buttonCancel_Click(object sender, EventArgs e)
        {
            m_form.DialogResult = DialogResult.Cancel;
        }
        #endregion
    }
}
