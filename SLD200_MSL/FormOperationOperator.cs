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
            
            this.BackColor = Configuration.PanelBackColor;

            //CreatFlowPanelButton();
            ShowGeneralButton();
            this.flowLayoutPanelButton.Location = new System.Drawing.Point(Configuration.ButtonSize.Width + 16, 0);
            this.flowLayoutPanelButton.Size = new System.Drawing.Size(Configuration.PanelSize.Width - Configuration.ButtonSize.Width , Configuration.PanelSize.Height);
            this.flowLayoutPanelButton.BackColor = Configuration.PanelBackColor;

            this.panelContent.Location = new System.Drawing.Point(0, Configuration.PanelSize.Height);
            this.panelContent.Size = new System.Drawing.Size(Configuration.PanelbuttonSize.Width + 10, Configuration.FormContentSize.Height);
            this.panelContent.BackColor = Configuration.PanelBackColor;

            this.panelGeneral.Location = new System.Drawing.Point(0, 3);
            this.panelGeneral.Size = new Size(Configuration.ButtonSize.Width + 13, this.flowLayoutPanelButton.Size.Height);
            this.panelGeneral.BackColor = Configuration.PanelBackColor;

            this.panelContent.Location = new System.Drawing.Point(0, 44);       //  2022. 03. 24.  SCH : "Operator" 글자가 너무 붙어서... 좀 내리느라고 넣음 -_-
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
                control[i].Font = new System.Drawing.Font("Tahoma", 9.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
                control[i].Size = new Size(Configuration.ButtonSize.Width + 10, Configuration.ButtonSize.Height);
                control[i].Name = Modules[i].Name;
                control[i].Text = Modules[i].Name;
                control[i].Tag = Modules[i];
                control[i].BackColor = Color.White;
                control[i].ForeColor = Color.Black;
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
            button.Font = new System.Drawing.Font("Tahoma", 9.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            button.Size = new Size(Configuration.ButtonSize.Width + 10, Configuration.ButtonSize.Height);
            button.Location = new Point(3, 0);
            button.Name = "General";
            button.Text = "Module";                 //   "General";
            button.BackColor = Color.White;
            button.ForeColor = Color.Black;
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
