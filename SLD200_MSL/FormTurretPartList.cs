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
    public partial class FormTurretPartList : Form
    {
        public ModuleCollection m_collectionModules;
        public FormTurretPartList()
        {
            FormBaseConfiguration Configuration = new FormBaseConfiguration();
            InitializeComponent();
            InitTreeView();

            this.baseButtonOk.Size = new Size(Configuration.ButtonSize.Width, Configuration.ButtonSize.Height);
            this.baseButtonOk.Text = "OK";
            this.baseButtonCancel.Size = new Size(Configuration.ButtonSize.Width, Configuration.ButtonSize.Height);
            this.baseButtonCancel.Text = "Cancel";
        }

        #region InitTreeView
        private void InitTreeView()
        {
            baseTreeViewTurretPartList.Nodes.Clear();
            string strEq = string.Format("Equipment({0})", Equipment.Name);
            TreeNode nodeEquipment = baseTreeViewTurretPartList.Nodes.Add(strEq);
            m_collectionModules = Equipment.Modules;
            foreach (Module module in m_collectionModules)
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
            TreeNode partNode = node.Nodes.Add(part.Name);
            //foreach (Part sub in part.Parts)
            //{
            //    AddPartNode(sub, partNode);
            //}
        }
        #endregion
    }
}
