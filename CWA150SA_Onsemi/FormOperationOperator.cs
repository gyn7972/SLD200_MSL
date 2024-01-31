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
    public partial class FormOperationOperator : FormSubContentBase
    {

        private Dictionary<Module, Form> m_DicForms;

        FormOperationGeneral FormOperationGeneral;
        
        public ModuleCollection Modules { get; set; }
        public FormOperationOperator()
            :base(FormType.withButton.ToString(), "Operator")           //  2022. 03. 21.  SCH : "Operator" 창 Title 
        {
            InitializeComponent();
            Equipment.GetModuleList();
            Modules = Equipment.Modules;
            
            CreatFlowPanelButton();
            ShowGeneralButton();
            this.flowLayoutPanelButton.Location = new System.Drawing.Point(Configuration.ButtonSize.Width  + 6, 0);
            this.flowLayoutPanelButton.Size = new System.Drawing.Size(Configuration.PanelSize.Width - Configuration.ButtonSize.Width , Configuration.PanelSize.Height);

            this.panelContent.Location = new System.Drawing.Point(0, Configuration.PanelSize.Height);
            this.panelContent.Size = new System.Drawing.Size(Configuration.PanelbuttonSize.Width + 10, Configuration.FormContentSize.Height);

            this.panelGeneral.Location = new System.Drawing.Point(0, 3);
            this.panelGeneral.Size = new Size(Configuration.ButtonSize.Width + 3, this.flowLayoutPanelButton.Size.Height);

            this.panelContent.Location = new System.Drawing.Point(0, 50);       //  2022. 03. 24.  SCH : "Operator" 글자가 너무 붙어서... 좀 내리느라고 넣음 -_-
            this.panelContent.Controls.Add(baseLabelTitle);

            m_DicForms = new Dictionary<Module, Form>();
            FormOperationGeneral = new FormOperationGeneral();
            foreach (Module module in Modules)
            {
                Form Subform = FormManager.GetOperationForm(module);
                m_DicForms.Add(module, Subform);
            }

            ShowForm(FormOperationGeneral);
        }

        public void CreatFlowPanelButton()
        {
            this.flowLayoutPanelButton.Controls.Clear();
            BaseButton[] control = new BaseButton[Modules.Count];
            for (int i = 0; i < Modules.Count; i++)
            {
                control[i] = new BaseButton();
                control[i].Parent = this;
                control[i].Size = new Size(Configuration.ButtonSize.Width, Configuration.ButtonSize.Height);
                control[i].Name = Modules[i].Name;
                control[i].Text = Modules[i].Name;
                control[i].Tag = Modules[i];
                control[i].Click += Button_Click;

                this.flowLayoutPanelButton.Controls.Add(control[i]);
            }         
        }

        public void Button_Click(object sender, EventArgs e)
        {
            BaseButton button = sender as BaseButton;
            for (int i = 0; i < Modules.Count; i++)
            {
                if (button.Name == Modules[i].Name)
                {
                    SelectedModule(Modules[i]);
                    break;
                }
            }
        }

        public void ShowGeneralButton()
        {
            this.panelContent.Controls.Clear();
            BaseButton button = new BaseButton();
            button.Parent = this;
            button.Size = Configuration.ButtonSize;
            button.Location = new Point(3, 0);
            button.Name = "General";
            button.Text = "General";
            button.Click += ButtonGeneral_Click;
            this.panelGeneral.Controls.Add(button);
        }
        public void ButtonGeneral_Click(object sender, EventArgs e)
        {
            this.panelContent.Controls.Clear();
            ShowForm(FormOperationGeneral);
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
            this.panelContent.Controls.Add(form);
            form.BringToFront();
            form.Show();
        }
    }
}
