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
using QMC.Common.Modules;
namespace SLD200_MSL
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

        private void FormModuleMaint_Activated(object sender, EventArgs e)
        {
            //BaseButton button = sender as BaseButton;

            //if (button != null)
            //{
            //    if (!Equipment.User_AdminMode && ((button.Text == "ReticleAligner Lower") || (button.Text == "ReticleAligner Upper")))
            //    {
            //        foreach (Part part in m_dicSubForms.Keys)
            //        {
            //            if (part.Name == "WorkStage")
            //            {
            //                ShowSubForm(part);
            //                return;
            //            }
            //        }
            //    }
            //}
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
                control[i].Size = new Size(Configuration.ButtonSize.Width + 10, Configuration.ButtonSize.Height + 5);               //  Part 버튼 크기 조정할 때 여기 값 변경
                control[i].Name = part.Name.ToString();
                control[i].Text = part.Name.ToString();
                control[i].Click += Button_Click;
                control[i].Tag = part;
                control[i].Font = new System.Drawing.Font("Tahoma", 9.0F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));

                this.flowLayoutPanelButton.Controls.Add(control[i]);
                i++;
            }
        }

        public void Button_Click(object sender, EventArgs e)
        {
            //  2024. 03. 08.  SCH : OP 모드일 때, 마크 등록 창만 접근할 수 있도록 여기서 제한해야 함.
            //                      OP 모드일 때, JigAligner Upper / Lower 두개의 버튼만 활성화 시켜야 함.

            BaseButton button = sender as BaseButton;
            if (button != null)
            {
                if (!((Equipment.User_LoginMode == (int)Equipment.UserMode.USER_ADMIN) || (Equipment.User_LoginMode == (int)Equipment.UserMode.USER_ENGINEER)) && 
                    ((button.Text == "ReticleAligner Lower") || (button.Text == "ReticleAligner Upper")))
                {
                    MessageBox.Show("관리자 모드가 아닙니다.", "Information!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                
                ShowSubForm((Part)button.Tag);
            }
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
