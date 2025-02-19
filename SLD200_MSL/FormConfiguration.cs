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
    public partial class FormConfiguration : FormSubContentBase
    {
        #region Define
        public enum ConfigurationButtons
        {
            Module,

            Motion,                                 //  Setup 에서 하던 것인데, Configuration 에서 할 수 있도록 옮기자
            IO,                                     //  Setup 에서 하던 것인데, Configuration 에서 할 수 있도록 옮기자
            Equipment,                              //  Setup 에서 하던 것인데, Configuration 에서 할 수 있도록 옮기자
        }
        #endregion

        #region Field
        public FormModuleCollection FormModuleCollection { get; set; }

        private FormSelectSetUpIO m_FormSelectSetUpIO;
        private FormSelectSetUpMotion m_FormSelectSetUpMotion;
        private FormEquipment m_FormEquipment;
        #endregion

        #region Constructor
        public FormConfiguration()
            : base(FormType.withButton.ToString(), "Configuration")
        {
            InitializeComponent();
            CreatFlowPanelButton();

            this.FormSelectSetUpIO = new FormSelectSetUpIO();
            this.FormSelectSetUpMotion = new FormSelectSetUpMotion();
            this.FormEquipment = new FormEquipment();

            this.BackColor = Configuration.PanelBackColor;

            FormModuleCollection = new FormModuleCollection();
            FormModuleCollection.selectedModule += ShowSelectedModule;
            FormModuleCollection.selectedVisionModule += ShowSelectedModule;
            FormModuleCollection.Size = new Size(1900, 800);                            //  요걸 해야 Config -> Module 선택 시 화면이 전체 채워짐
            FormModuleCollection.BackColor = Configuration.PanelBackColor;

            //m_dicForms = new Dictionary<Module, Form>();
            //foreach (Module module in Equipment.Modules)
            //{
            //    Form Subform = FormManager.GetConfigurationForm(module);
            //    m_dicForms.Add(module, Subform);
            //}
            Equipment.LoadModules += Equipment_LoadModules;
        }

        public FormEquipment FormEquipment
        {
            get { return m_FormEquipment; }
            set { m_FormEquipment = value; }
        }
        public FormSelectSetUpIO FormSelectSetUpIO
        {
            get { return m_FormSelectSetUpIO; }
            set { m_FormSelectSetUpIO = value; }
        }
        public FormSelectSetUpMotion FormSelectSetUpMotion
        {
            get { return m_FormSelectSetUpMotion; }
            set { m_FormSelectSetUpMotion = value; }
        }

        private void Equipment_LoadModules()
        {
            FormModuleCollection = new FormModuleCollection();
            FormModuleCollection.selectedModule += ShowSelectedModule;
            FormModuleCollection.selectedVisionModule += ShowSelectedModule;
            //m_dicForms.Clear();
            //m_dicForms = new Dictionary<Module, Form>();
            //foreach (Module module in Equipment.Modules)
            //{
            //    Form Subform = FormManager.GetConfigurationForm(module);
            //    m_dicForms.Add(module, Subform);
            //}
        }
        #endregion

        #region Property
        //public FormConfiguratinModule FormConfiguratinModule
        //{
        //    get { return m_FormConfiguratinModule; }
        //    set { m_FormConfiguratinModule = value; }
        //}
        #endregion

        #region EventHandler

        public void Button_Click(object sender, EventArgs e)
        {

            BaseButton button = sender as BaseButton; 
            switch (button.Tag)
            {
                case ConfigurationButtons.Module:
                    this.ShowModuleForm(FormModuleCollection);
                    //this.ShowForm(FormConfigurationModuleCollection);
                    break;

                case ConfigurationButtons.Motion:
                    FormSelectSetUpMotion.ShowPanelButton();
                    ShowForm(FormSelectSetUpMotion);
                    break;

                case ConfigurationButtons.IO:
                    this.FormSelectSetUpIO.ShowPanelButton();
                    this.ShowForm(FormSelectSetUpIO);
                    break;

                case ConfigurationButtons.Equipment:
                    this.FormEquipment.ShowPanelButton();
                    this.ShowForm(FormEquipment);
                    break;

                default:
                    break;
            }
        }

        private void panelContent_MouseDown(object sender, MouseEventArgs e)
        {
            FormModuleCollection.Hide();
        }
        #endregion

        #region Method
        public void CreatFlowPanelButton()
        {
            this.flowLayoutPanelButton.Size = new Size(1900, 800);
            this.flowLayoutPanelButton.BackColor = Configuration.PanelBackColor;

            this.flowLayoutPanelButton.Controls.Clear();
            BaseButton[] control = new BaseButton[Enum.GetValues(typeof(ConfigurationButtons)).Length];
            foreach (ConfigurationButtons item in Enum.GetValues(typeof(ConfigurationButtons)))
            {
                control[(int)item] = new BaseButton();
                control[(int)item].Parent = this;
                control[(int)item].Size = new Size(Configuration.ButtonSize.Width + 10, Configuration.ButtonSize.Height);
                control[(int)item].Name = item.ToString();
                control[(int)item].Text = item.ToString();
                control[(int)item].Tag = item;
                control[(int)item].BackColor = Color.White;
                control[(int)item].ForeColor = Color.Black;
                control[(int)(item)].Click += Button_Click;
                control[(int)item].Font = new System.Drawing.Font("Tahoma", 9.0F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));

                this.flowLayoutPanelButton.Controls.Add(control[(int)item]);
            }
        }
        public void ShowModuleForm(Form form)
        {
            this.panelContent.BackColor = Configuration.PanelBackColor;
            this.panelContent.Size = new Size(1900, 800);

            form.TopLevel = false;
            this.panelContent.Controls.Add(form);
            // form.Parent = this;
            form.BringToFront();
            form.Show();
        }
        public void ShowForm(Form form)
        {
            this.panelContent.Controls.Clear();
            form.TopLevel = false;
            this.panelContent.Controls.Add(form);
            // form.Parent = this;
            form.BringToFront();
            form.Show();
        }
        public void FormHide()
        {
            FormModuleCollection.Hide();
        }
        public void ShowSelectedModule(Module module)
        {
            FormModuleConfig formModule = new FormModuleConfig(module);
            ShowSelectedModuleForm(formModule);
        }

        private void ShowSelectedModuleForm(Form form)
        {
            this.panelContent.Controls.Clear();
            form.TopLevel = false;
            this.panelContent.Controls.Add(form);
            form.BringToFront();
            form.Show();
        }


        #endregion
    }
}
