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
    public partial class FormMaintMain : FormContentBase
    {
        #region Define
        public enum MaintButtons
        {
            Equipment
            //Module
        }
        #endregion

        #region Field
        public FormModuleCollection FormModuleCollection { get; set; }
        //public FormMaintModule FormMaintModule { get; set; }
        public FormMaintEquipment FormMaintEquipment { get; set; }
        public FormMaintHasVisionModule FormMaintVisionModule { get; set; }

        private Dictionary<Module, Form> m_DicForms;

        protected FormModuleMaint m_FormModuleMaint;
        #endregion

        #region Constructor
        public FormMaintMain()
           : base()
        {
            InitializeComponent();

            this.BackColor = Configuration.PanelBackColor;
            this.flowLayoutPanelButton.BackColor = Configuration.PanelBackColor;

            CreatFlowPanelButton();
            FormModuleCollection = new FormModuleCollection();
            FormMaintEquipment = new FormMaintEquipment();
            FormMaintEquipment.OwnerForm = this;
            FormModuleCollection.selectedModule += ShowSelectedModule;
            FormModuleCollection.selectedVisionModule += ShowSelectedModule;

            m_FormModuleMaint = new FormModuleMaint(null);

            m_DicForms = new Dictionary<Module, Form>();
            foreach (Module module in Equipment.Modules)
            {
                Form Subform = FormManager.GetMaintForm(module);
                if(Subform != null)
                    m_DicForms.Add(module, Subform);
            }
           
            Equipment.LoadModules += Equipment_LoadModules;
        }

        private void Equipment_LoadModules()
        {
       //     FormModuleCollection.selectedModule -= ShowSelectedModule;
       //     FormModuleCollection.selectedVisionModule -= ShowSelectedModule;
            FormModuleCollection = new FormModuleCollection();
            FormModuleCollection.selectedModule += ShowSelectedModule;
            FormModuleCollection.selectedVisionModule += ShowSelectedModule;
        }
        #endregion

        #region EventHandler

        private void Button_Click(object sender, EventArgs e)
        {
            BaseButton button = sender as BaseButton;
            switch (button.Tag)
            {
                case MaintButtons.Equipment:
                    ShowForm(FormMaintEquipment);
                    break;

                //case MaintButtons.Module:
                //    this.ShowModuleForm(FormModuleCollection);
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
        private void CreatFlowPanelButton()
        {
            this.flowLayoutPanelButton.Controls.Clear();
            BaseButton[] control = new BaseButton[Enum.GetValues(typeof(MaintButtons)).Length];
            foreach (MaintButtons item in Enum.GetValues(typeof(MaintButtons)))
            {
                control[(int)item] = new BaseButton();
                control[(int)item].Parent = this;
                control[(int)item].Size = new Size(Configuration.ButtonSize.Width + 10, Configuration.ButtonSize.Height);
                control[(int)item].Name = item.ToString();
                control[(int)item].Text = item.ToString();
                control[(int)item].Tag = item;
                control[(int)item].BackColor = Color.White;
                control[(int)item].ForeColor = Color.Black;
                control[(int)item].Click += Button_Click;
                control[(int)item].Font = new System.Drawing.Font("Tahoma", 9.0F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));

                this.flowLayoutPanelButton.Controls.Add(control[(int)item]);
            }
        }

        public void ShowModuleForm(Form form)
        {
            form.TopLevel = false;
            this.panelContent.Controls.Add(form);
            form.BringToFront();
            form.Show();
        }

        private void ShowForm(Form form)
        {
            if(this.panelContent.Controls.Count > 0)
                this.panelContent.Controls[0].Visible = false;
            this.panelContent.Controls.Clear();
            form.TopLevel = false;
            this.panelContent.Controls.Add(form);
            form.BringToFront();
            form.Show();
        }

        public void FormHide()
        {
            FormModuleCollection.Hide();
        }

        public void ShowSelectedModule(Module module)
        {
            m_FormModuleMaint.InitModule(module);
            ShowSelectedModuleForm(m_FormModuleMaint);
        }

        private void ShowSelectedModuleForm(Form form)
        {
            
            form.TopLevel = false;
            if (this.panelContent.Controls.Count > 0)
                this.panelContent.Controls[0].Visible = false;
            this.panelContent.Controls.Clear();
            this.panelContent.Controls.Add(form);
            form.BringToFront();
            form.Show();
        }
        #endregion
    }
}
