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
    public partial class FormModuleMaint : FormSubContentBase
    {
        public Module m_Module;

        public ModuleCollection m_collectionModules;

        public static event EventHandlerForm m_FormHide;
        public Part Part { get; set; }

        private Dictionary<Part, Form> m_dicSubForms;
        private FormMaintDigitalIO m_formDigitalIO;
        public FormModuleMaint(Module module)
            : base(FormType.Content.ToString(), "")
        {
            this.Controls.Add(this.panelGeneral);
            InitializeComponent();
   
            this.flowLayoutPanelButton.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelButton.Size = new System.Drawing.Size(Configuration.PanelSize.Width - Configuration.ButtonSize.Width, Configuration.PanelSize.Height);

            this.panelContent.Location = new System.Drawing.Point(0, Configuration.PanelSize.Height);
            this.panelContent.Size = new System.Drawing.Size(Configuration.PanelbuttonSize.Width+20, Configuration.FormContentSize.Height -Configuration.PanelSize.Height*2 + 65);

            m_formDigitalIO = new FormMaintDigitalIO(module);
            InitModule(module);
            //   ShowGeneralButton();

            this.panelContent.MouseDown += PanelContent_MouseDown;
        }

        public void InitModule(Module module)
        {
            if (module == null)
            {
                return;
            }
            m_Module = module;
            this.baseLabelTitle.Text = m_Module.Name;
            m_formDigitalIO.InitModule(module);
            InitSubForm();
            ShowPartButton();
            this.panelContent.Controls.Clear();
        }
        private void PanelContent_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_FormHide != null)
            {
                m_FormHide();
            }
        }

        protected void InitSubForm()
        {
            m_dicSubForms = new Dictionary<Part, Form>();
            Form subForm = FormManager.GetMaintForm(m_Module);
            if (subForm != null)
                m_dicSubForms.Add(m_Module, subForm);

            //  2022. 03. 30.  SCH : Maint 에서 IO 창은 어디서 보게 해야 하나...
            Part IOPart = new Part("IO");
            m_dicSubForms.Add(IOPart, m_formDigitalIO);

            foreach (Part part in m_Module.Parts)
            {
                subForm = FormManager.GetMaintForm(part);
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
            //BaseButton[] control = new BaseButton[m_Module.Parts.Count];
            BaseButton[] control = new BaseButton[m_dicSubForms.Values.Count];
            int i = 0;
            foreach (Part part in m_dicSubForms.Keys)
            {
                control[i] = new BaseButton();
                control[i].Parent = this;
                //control[i].Size = new Size(Configuration.ButtonSize.Width - 32, Configuration.ButtonSize.Height + 5 );               //  Part 버튼 크기 조정할 때 여기 값 변경
                control[i].Size = new Size(Configuration.ButtonSize.Width, Configuration.ButtonSize.Height + 5);               //  Part 버튼 크기 조정할 때 여기 값 변경
                control[i].Name = part.Name.ToString();
                control[i].Text = part.Name.ToString();
                control[i].Click += Button_Click;
                control[i].Tag = part;

                //  2023. 06. 01.  SCH : 광기술원 전용. (필요한 버튼만 색깔을 바꿔준다. 작업자가 알아보기 쉽게...)
                if ((control[i].Name == "WaferProbeAlign") ||
                    (control[i].Name == "JigAligner Lower") ||
                    (control[i].Name == "JigAligner Upper") )
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
                if(this.panelContent.Controls.Count > 0)
                    this.panelContent.Controls[0].Visible = false;
                this.panelContent.Controls.Clear();
                subForm.TopLevel = false;
                this.panelContent.Controls.Add(subForm);
                subForm.BringToFront();
                subForm.Show();
            }
        }
    }
}
