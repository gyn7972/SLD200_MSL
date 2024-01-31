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
    public partial class FormOperationGeneral : FormSubContentBase
    {
        #region Field
        private Dictionary<Module, Form> m_DicForms;
        private ModuleCollection m_collectionModules;

        #region Property
        FormBaseConfiguration formConfiguration { get; set; }

        #endregion
        public FormOperationGeneral()
            : base(FormType.withButton.ToString(), "General")
        {
            InitializeComponent();
            formConfiguration = new FormBaseConfiguration();
            m_collectionModules = Equipment.Modules;
            CreateModuleList();
            m_DicForms = new Dictionary<Module, Form>();
            foreach (Module module in m_collectionModules)
            {
                Form Subform = FormManager.GetOperationForm(module);
                m_DicForms.Add(module, Subform);
            }
            this.flowLayoutPanelModuleList.Size = new Size(formConfiguration.ListBoxSize.Width * 2, formConfiguration.ListBoxSize.Height);
            this.flowLayoutPanelModuleList.Location = new Point(50, Configuration.ContentLocation.Y + Configuration.PanelSize.Height);
        }
        #endregion

        #region Method
        public void CreateModuleList()
        {
            ModuleButton[] control = new ModuleButton[m_collectionModules.Count];
            int locationX = Configuration.ButtonSize.Width;
            int locationY = Configuration.ButtonSize.Height;
            for (int i = 0; i < m_collectionModules.Count; i++)
            {
                control[i] = new ModuleButton(m_collectionModules[i]);
                control[i].Parent = this;
                control[i].Name = m_collectionModules[i].Name.ToString();
                control[i].Text = m_collectionModules[i].Name.ToString();
                control[i].Font = new Font(control[i].Font.FontFamily, 12);
                control[i].Tag = m_collectionModules[i].Name;
                control[i].Location = new Point(locationX, locationY);
                control[i].ModuleButtonClick += Button_Click;
                this.flowLayoutPanelModuleList.Controls.Add(control[i]);
                locationX += Configuration.ButtonSize.Width + 20;
            }
        }

        public void SelectedModule(Module module)
        {
            this.panelContent.Controls.Clear();
            if (m_DicForms.ContainsKey(module) || module != null)
            {
                ShowForm(m_DicForms[module]);
            }
        }

        public void ShowForm(Form form)
        {
            if (form == null)
            {
                return;
            }
            this.panelContent.Controls.Clear();
            form.TopLevel = false;
            this.Parent.Controls.Add(form);
            //this.panelContent.Controls.Add(form);
            form.BringToFront();
            form.Show();
        }
        #endregion

        #region EventHandler
        public void Button_Click(string strButtonName)
        {
            for (int i = 0; i < m_collectionModules.Count; i++)
            {
                if (strButtonName == m_collectionModules[i].Name)
                {
                    SelectedModule(m_collectionModules[i]);
                    break;
                }
            }
        }
        #endregion
    }
}
