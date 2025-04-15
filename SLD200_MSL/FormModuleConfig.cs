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

namespace SLD200_MSL
{
    public delegate void EventHandlerForm();
    public partial class FormModuleConfig : FormSubContentBase
    {
        public Module m_Module;

        public ModuleCollection m_collectionModules;

        public static event EventHandlerForm m_FormHide;
        //public Part Part { get; set; }

        private Dictionary<Part, Form> m_dicSubForms;
        private FormMaintDigitalIO m_formDigitalIO;

        public FormModuleConfig(Module module)
            : base(FormType.Content.ToString(), "Module Configuration")
        {
            if (module == null)
            {
                return;
            }
            m_Module = module;
            //Part = new Part();
            InitializeComponent();

            this.BackColor = Configuration.PanelBackColor;

            this.Controls.Add(this.panelGeneral);
            this.flowLayoutPanelButton.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelButton.Size = new System.Drawing.Size(Configuration.PanelSize.Width - Configuration.ButtonSize.Width, Configuration.PanelSize.Height);
            this.flowLayoutPanelButton.BackColor = Configuration.PanelBackColor;

            this.panelContent.Location = new System.Drawing.Point(0, Configuration.PanelSize.Height);
            //this.panelContent.Size = new System.Drawing.Size(Configuration.PanelbuttonSize.Width, Configuration.FormContentSize.Height - Configuration.PanelSize.Height * 2);
            this.panelContent.Size = new System.Drawing.Size(Configuration.PanelbuttonSize.Width + 20, Configuration.FormContentSize.Height - Configuration.PanelSize.Height * 2 + 65);
            this.panelContent.BackColor = Configuration.PanelBackColor;

            m_formDigitalIO = new FormMaintDigitalIO(module);

            //this.panelGeneral.Location = new System.Drawing.Point(0, 3);
            // this.panelGeneral.Size = new Size(Configuration.ButtonSize.Width + 20, this.flowLayoutPanelButton.Size.Height);

            if (module != null)
            {
                m_formDigitalIO.InitModule(module);
            }

            InitSubForm();

            ShowPartButton();
            //   ShowGeneralButton();
            
            this.panelContent.MouseDown += PanelContent_MouseDown;



        }
        private void PanelContent_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_FormHide != null)
            {
                m_FormHide();
            }
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            //Equipment.LoadModuleCollection();
            //Equipment.LoadConfig();
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            //m_collectionModules = Equipment.Modules;
            //Equipment.SaveModuleCollection(m_collectionModules);
            //foreach (Module module in m_collectionModules)
            //{
            //    foreach (Part part in module.Parts)
            //    {
            //        foreach (Function function in part.GetFunctionList())
            //        {
            //            foreach (Action action in function.Actions)
            //            {
            //                ActionParameterCollection actionParameters = new ActionParameterCollection();
            //                for (int i = 0; i < action.Parameters.Count; i++)
            //                {
            //                    if (!action.Parameters[i].IsRecipe)
            //                    {
            //                        actionParameters.Add(action.Parameters[i]);
            //                    }
            //                }
            //                if (DataManager.Instance.Config.IsParameters(module.Name, part.Name, function.Name, action.Name) == false)
            //                    DataManager.Instance.Config.SetParameters(module.Name, part.Name, function.Name, action.Name, actionParameters);
            //            }
            //        }
            //    }
            //}
            //Equipment.SaveConfig();

            MessageBox.Show("Save Ok");
        }
        protected void InitSubForm()
        {
            m_dicSubForms = new Dictionary<Part, Form>();
            Form subForm = FormManager.GetConfigurationForm(m_Module);
            if (subForm != null)
                m_dicSubForms.Add(m_Module, subForm);

            Part IOPart = new Part("IO");
            m_dicSubForms.Add(IOPart, m_formDigitalIO);

            foreach (Part part in m_Module.Parts)
            {
                subForm = FormManager.GetConfigurationForm(part);
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
                //control[i].Size = new Size(Configuration.ButtonSize.Width - 32, Configuration.ButtonSize.Height + 5);
                control[i].Size = new Size(Configuration.ButtonSize.Width + 10, Configuration.ButtonSize.Height + 5);                    //  Part 버튼 크기 조정
                control[i].Name = part.Name.ToString();
                control[i].Text = part.Name.ToString();
                control[i].Click += Button_Click;
                control[i].Tag = part;
                control[i].BackColor = Color.White;
                control[i].ForeColor = Color.Black;
                control[i].Font = new System.Drawing.Font("Tahoma", 9.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));

                //  Configuration -> Module -> WorkStage 선택 시 표시될 버튼
                if (m_Module.Name == "WorkStage")
                {
                    if ((control[i].Name == "WorkStage") ||
                        (control[i].Name == "IO") ||
                        (control[i].Name == "Coarse Vision") ||
                        (control[i].Name == "Fine Vision") ||
                        (control[i].Name == "JigAligner (Fine)") ||
                        (control[i].Name == "JigAligner (Coarse)") ||
                        (control[i].Name == "ScannerCompen. (Fine)"))
                    {
                        this.flowLayoutPanelButton.Controls.Add(control[i]);
                    }
                }
                else if (m_Module.Name == "Loader")
                {
                    if ((control[i].Name == "Loader") ||
                        (control[i].Name == "IO"))
                    {
                        this.flowLayoutPanelButton.Controls.Add(control[i]);
                    }
                }
                else if (m_Module.Name == "Unloader")
                {
                    if ((control[i].Name == "Unloader") ||
                        (control[i].Name == "IO"))
                    {
                        this.flowLayoutPanelButton.Controls.Add(control[i]);
                    }
                }
                else if (m_Module.Name == "Vision")
                {
                    if ((control[i].Name == "Vision") ) // ||
                        //(control[i].Name == "Coarse Vision") ||
                        //(control[i].Name == "Fine Vision") )
                    {
                        this.flowLayoutPanelButton.Controls.Add(control[i]);
                    }
                }
                else if (m_Module.Name == "BDS")
                {
                    if ((control[i].Name == "BDS") ||
                        (control[i].Name == "IO"))
                    {
                        this.flowLayoutPanelButton.Controls.Add(control[i]);
                    }
                }

                //this.flowLayoutPanelButton.Controls.Add(control[i]);              //  추가된 Part 에 대한 버튼 몽땅 표시
                i++;
            }
        }
        
        public void Button_Click(object sender, EventArgs e)
        {
            BaseButton button = sender as BaseButton;
            if(button != null)
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
        public void SelectedPart(Part part)
        {
            //if (part == null)
            //{
            //    return;
            //}
            //this.labelTitle.Text = part.Name;
            //Part = null;
            //Part = part;
            //List<Function> functions = part.GetFunctionList();
            //listBoxFunction.DataSource = null;
            //listBoxFunction.DataSource = functions;
            //listBoxFunction.DisplayMember = "Name";
        }
    }
   
}
