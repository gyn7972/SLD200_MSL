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
    public partial class FormConfiguration : FormSubContentBase
    {
        #region Define
        public enum ConfigurationButtons
        {
            //Equipment,
            Module,
            //Carrier,
            //Axes
        }
        #endregion

        #region Field
        public FormModuleCollection FormModuleCollection { get; set; }
        //private FormConfiguratinModule m_FormConfiguratinModule { get; set; }

        //private Dictionary<Module, Form> m_dicForms;

        #endregion

        #region Constructor
        public FormConfiguration()
            : base(FormType.withButton.ToString(), "Configuration")
        {
            InitializeComponent();
            CreatFlowPanelButton();

            FormModuleCollection = new FormModuleCollection();
            FormModuleCollection.selectedModule += ShowSelectedModule;
            FormModuleCollection.selectedVisionModule += ShowSelectedModule;

            //m_dicForms = new Dictionary<Module, Form>();
            //foreach (Module module in Equipment.Modules)
            //{
            //    Form Subform = FormManager.GetConfigurationForm(module);
            //    m_dicForms.Add(module, Subform);
            //}
            Equipment.LoadModules += Equipment_LoadModules;

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
                //case ConfigurationButtons.Equipment:

                //    //  ShowForm();
                //    break;
                case ConfigurationButtons.Module:
                    this.ShowModuleForm(FormModuleCollection);
                    //        this.ShowForm(FormConfigurationModuleCollection);

                    break;
                //case ConfigurationButtons.Carrier:

                    //ShowForm();
                //    break;
                //case ConfigurationButtons.Axes:

                    //ShowForm();
                //    break;

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
            this.flowLayoutPanelButton.Controls.Clear();
            BaseButton[] control = new BaseButton[Enum.GetValues(typeof(ConfigurationButtons)).Length];
            foreach (ConfigurationButtons item in Enum.GetValues(typeof(ConfigurationButtons)))
            {
                control[(int)item] = new BaseButton();
                control[(int)item].Parent = this;
                control[(int)item].Size = new Size(Configuration.ButtonSize.Width, Configuration.ButtonSize.Height);
                control[(int)item].Name = item.ToString();
                control[(int)item].Text = item.ToString();
                control[(int)item].Tag = item;
                control[(int)(item)].Click += Button_Click;

                //  2023. 06. 01.  SCH : 광기술원 전용. (필요한 버튼만 색깔을 바꿔준다. 작업자가 알아보기 쉽게...)
                if (control[(int)item].Name == "Module")
                {
                    control[(int)item].ForeColor = Color.Yellow;
                    control[(int)item].Font = new System.Drawing.Font("Arial", 9.0F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
                }
                else
                {
                    control[(int)item].Font = new System.Drawing.Font("Arial", 9.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
                }

                this.flowLayoutPanelButton.Controls.Add(control[(int)item]);
            }
        }
        public void ShowModuleForm(Form form)
        {

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
