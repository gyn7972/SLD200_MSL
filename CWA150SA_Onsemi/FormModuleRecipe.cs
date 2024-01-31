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
namespace CWA150SA_Onsemi300
{
    public partial class FormModuleRecipe : FormSubContentBase
    {
        public Module m_Module;
        private Dictionary<Part, Form> m_dicSubForms;
        public FormModuleCollection m_FormModuleCollection { get; set; }
        public FormModuleRecipe(Module module)
            : base(FormType.Content.ToString(), "Module Recipe")
        {
            m_Module = module;
            this.panelContent.Location = new System.Drawing.Point(0, Configuration.PanelSize.Height);
            this.panelContent.Size = new System.Drawing.Size(Configuration.PanelbuttonSize.Width, Configuration.FormContentSize.Height - Configuration.PanelSize.Height * 2);

            this.flowLayoutPanelButton.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelButton.Size = new System.Drawing.Size(Configuration.PanelSize.Width - Configuration.ButtonSize.Width, Configuration.PanelSize.Height + 5);
            

            InitializeComponent();
            if (module == null)
            {
                return;
            }
            InitSubForm();
            ShowPartButton();
        }
        
        protected void InitSubForm()
        {
            m_dicSubForms = new Dictionary<Part, Form>();
            Form subForm = FormManager.GetRecipeForm(m_Module);
            if (subForm != null)
                m_dicSubForms.Add(m_Module, subForm);

            foreach (Part part in m_Module.Parts)
            {
                subForm = FormManager.GetRecipeForm(part);
                if (subForm != null)
                    m_dicSubForms.Add(part, subForm);
            }
        }
        public void ShowPartButton()
        {
            if (m_Module.Parts.Count == 0)
            {
                return;
            }
            this.flowLayoutPanelButton.Controls.Clear();
            BaseButton[] control = new BaseButton[m_Module.Parts.Count];
            int i = 0;
            foreach (Part part in m_dicSubForms.Keys)
            {
                control[i] = new BaseButton();
                control[i].Parent = this;
                control[i].Size = new Size(Configuration.ButtonSize.Width, Configuration.ButtonSize.Height + 5);
                control[i].Name = part.Name.ToString();
                control[i].Text = part.Name.ToString();
                control[i].Click += Button_Click;
                control[i].Tag = part;

                //  2023. 06. 01.  SCH : 광기술원 전용. (필요한 버튼만 색깔을 바꿔준다. 작업자가 알아보기 쉽게...)
                if ((control[i].Name == "JigAligner Lower") ||
                    (control[i].Name == "JigAligner Upper"))
                {
                    control[i].ForeColor = Color.Yellow;
                    control[i].Font = new System.Drawing.Font("Arial", 9.0F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
                }
                else
                {
                    control[i].Font = new System.Drawing.Font("Arial", 9.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
                }

                this.flowLayoutPanelButton.Controls.Add(control[i]);
                i++;
            }
        }
        public void Button_Click(object sender, EventArgs e)
        {
            BaseButton button = sender as BaseButton;
            if (button != null)
                ShowSubForm((Part)button.Tag);
        }
        public void ShowSubForm(Part part)
        {
            if (part != null && m_dicSubForms.ContainsKey(part))
            {
                Form subForm = m_dicSubForms[part];
                this.panelContent.Controls.Clear();
                subForm.TopLevel = false;
                this.panelContent.Controls.Add(subForm);
                subForm.BringToFront();
                subForm.Show();
            }
        }
    }
}
