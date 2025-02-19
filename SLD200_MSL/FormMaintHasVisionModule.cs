using QMC.Common;
using QMC.Common.Hmi;
using QMC.Common.Vision.Cameras;
using QMC.Common.Vision.Optics;
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
    public delegate void EventHandlerMaintVisionModuleForm();
    public partial class FormMaintHasVisionModule : FormSubContentBase
    {
        public Module m_Module;

        public ModuleCollection m_collectionModules;

        public static event EventHandlerMaintVisionModuleForm m_FormMaintHide;

        public FormMaintVisionGeneral m_FormMaintVisionGeneral;

        public FormMaintDigitalIO m_FormMaintDigitalIO;
        private Dictionary<Part, Form> m_DicForms;

        public FormMaintHasVisionModule(Module module)
            :base(FormType.Content.ToString(), module.Name)
        {
            m_Module = module;
            if (module == null)
            {
                return;
            }
            InitializeComponent();
            
            this.Controls.Add(this.panelGeneral);
            this.Controls.Add(this.panelIO);
            this.flowLayoutPanelButton.Location = new System.Drawing.Point(Configuration.ButtonSize.Width * 2 + 12, 0);
            this.flowLayoutPanelButton.Size = new System.Drawing.Size(Configuration.PanelSize.Width - Configuration.ButtonSize.Width * 2, Configuration.PanelSize.Height);
            // this.flowLayoutPanelButton.Enabled = false;
            this.panelContent.Location = new System.Drawing.Point(0, Configuration.PanelSize.Height);
            this.panelContent.Size = new System.Drawing.Size(Configuration.PanelbuttonSize.Width+10, Configuration.FormContentSize.Height );

            this.panelIO.Location = new System.Drawing.Point(Configuration.ButtonSize.Width + 6, 3);
            this.panelIO.Size = new Size(Configuration.ButtonSize.Width + 3, this.flowLayoutPanelButton.Size.Height);
            this.panelGeneral.Location = new System.Drawing.Point(0, 3);
            this.panelGeneral.Size = new Size(Configuration.ButtonSize.Width + 3, this.flowLayoutPanelButton.Size.Height);

            baseLabelTitle.Size = Configuration.ButtonSize;
            baseLabelTitle.Location = new Point((Configuration.ContentSize.Width / 2) - (Configuration.ButtonSize.Width / 2), 3);
            this.panelContent.Controls.Add(baseLabelTitle);
            ShowGeneralButton();
            ShowPartButton();
            ShowIOButton();
            m_FormMaintDigitalIO = new FormMaintDigitalIO(m_Module);
            m_DicForms = new Dictionary<Part, Form>();
            foreach (Part part in m_Module.Parts)
            {
                Form Subform = FormManager.GetMaintForm(part);
                if(Subform != null)
                    m_DicForms.Add(part, Subform);

            }
            m_FormMaintVisionGeneral = new FormMaintVisionGeneral(m_Module);
        }
        public void ShowGeneralButton()
        {
            this.panelContent.Controls.Clear();
            BaseButton button = new BaseButton();
            button.Parent = this;
            button.Size = Configuration.ButtonSize;
            button.Location = new Point(3, 0);
            button.Name = m_Module.Name;
            button.Text = "General";
            button.Click += ButtonGeneral_Click;
            this.panelGeneral.Controls.Add(button);
        }
        public void ShowIOButton()
        {
            BaseButton button = new BaseButton();
            button.Parent = this;
            button.Size = Configuration.ButtonSize;
            button.Location = new Point(3, 0);
            button.Name = m_Module.Name;
            button.Text = "IO";
            button.Click += ButtonIO_Click;
            this.panelIO.Controls.Add(button);
        }
        public void ShowPartButton()
        {
            this.flowLayoutPanelButton.Controls.Clear();
            BaseButton[] control = new BaseButton[m_Module.Parts.Count];
            for (int i = 0; i < m_Module.Parts.Count; i++)
            {
                if (m_Module.Parts[i] is Camera || m_Module.Parts[i] is Illuminator)
                {

                }
                else
                {
                    control[i] = new BaseButton();
                    control[i].Parent = this;
                    control[i].Size = new Size(Configuration.ButtonSize.Width + 10, Configuration.ButtonSize.Height);
                    control[i].Name = m_Module.Parts[i].Name.ToString();
                    control[i].Text = m_Module.Parts[i].Name.ToString();
                    control[i].Click += Button_Click;
                    control[i].Tag = m_Module.Parts[i].Name;
                    this.flowLayoutPanelButton.Controls.Add(control[i]);
                }
            }
        }
        public void ButtonIO_Click(object sender, EventArgs e)
        {
            this.panelContent.Controls.Clear();
            ShowForm(m_FormMaintDigitalIO);
        }
        public void SelectedPart(Part part)
        {
            this.panelContent.Controls.Clear();
            if (part != null && m_DicForms.ContainsKey(part) )
            {
                ShowForm(m_DicForms[part]);
            }
        }
        public static void FormHide()
        {
            m_FormMaintHide();
        }
        public void ShowForm(Form form)
        {
            this.panelContent.Controls.Clear();
            form.TopLevel = false;
            this.panelContent.Controls.Add(form);
            form.BringToFront();
            form.Show();
        }
        public void ButtonGeneral_Click(object sender, EventArgs e)
        {
            this.panelContent.Controls.Clear();
            ShowForm(m_FormMaintVisionGeneral);
        }
        public void Button_Click(object sender, EventArgs e)
        {
            BaseButton button = sender as BaseButton;
            for (int i = 0; i < m_Module.Parts.Count; i++)
            {
                if (button.Name == m_Module.Parts[i].Name)
                {
                    SelectedPart(m_Module.Parts[i]);
                }
            }
        }
        private void panelContent_MouseDown(object sender, MouseEventArgs e)
        {

            //m_FormMaintHide();
        }
    }
}
