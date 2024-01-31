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

namespace CWA150SA_Onsemi300
{
    #region Define
    public enum GroupBoxButtonList
    {
        Stop, Intialize, ClearJob
    }
    #endregion

    public partial class FormMaintEquipment : FormSubContentBase
    {
        #region Field
        private ModuleCollection m_collectionModules;
        #region Property
        public FormBaseConfiguration formConfiguration { get; set; }
        private EngineerButton m_EngineerButton { get; set; }

        public Form OwnerForm { get; set; }



        #endregion

        public FormMaintEquipment()
            : base(FormType.Content.ToString(), "CWA-150SA_Onsemi")
        {
            InitializeComponent();
            m_collectionModules = Equipment.Modules;
            formConfiguration = new FormBaseConfiguration();
            m_EngineerButton = new EngineerButton();
            CreateModuleList();
            CreateMaintButton();
            
            #region ControlConstructor
            this.panelContent.Controls.Add(this.flowLayoutPanelModuleList);
            this.panelContent.Controls.Add(this.groupBoxMaintEquipMent);
            this.panelContent.Controls.Add(m_EngineerButton);
            this.panelContent.Size = new Size(formConfiguration.ContentSize.Width + 10, formConfiguration.ContentSize.Height);
            this.panelContent.Size = new Size(0,Configuration.PanelSize.Height);

            this.flowLayoutPanelButton.Location = new Point(0,20);
            this.flowLayoutPanelButton.Size = new Size(formConfiguration.ContentSize.Width + 10, formConfiguration.PanelbuttonSize.Height);

            this.panelContent.Location = new Point(0, this.flowLayoutPanelButton.Location.Y + this.flowLayoutPanelButton.Height);
            this.panelContent.Size = new Size(formConfiguration.ContentSize.Width, formConfiguration.ContentSize.Height - this.flowLayoutPanelButton.Height);

            this.flowLayoutPanelModuleList.Size = new Size(formConfiguration.ListBoxSize.Width * 2, formConfiguration.ListBoxSize.Height);
            this.flowLayoutPanelModuleList.Location = new Point(50, Configuration.ContentLocation.Y + Configuration.PanelSize.Height);

            this.groupBoxMaintEquipMent.Size = new Size(formConfiguration.ButtonSize.Width +10, formConfiguration.ButtonSize.Height*4 +10);
            this.groupBoxMaintEquipMent.Location = new Point(this.panelContent.Width - this.groupBoxMaintEquipMent.Width-10, Configuration.ContentLocation.Y + Configuration.PanelSize.Height);
            this.groupBoxMaintEquipMent.ForeColor = Color.White;

            this.m_EngineerButton.Location = new Point(this.groupBoxMaintEquipMent.Location.X, this.groupBoxMaintEquipMent.Location.Y + this.groupBoxMaintEquipMent.Height + Configuration.ControlGap);
            #endregion
        }
        #endregion

        #region Method
        private void CreateModuleList()
        {
            ModuleButton[] control = new ModuleButton[m_collectionModules.Count];
            int locationX = Configuration.ButtonSize.Width;
            int locationY = Configuration.ButtonSize.Height;
            for (int i = 0; i < m_collectionModules.Count; i++)
            {
                //if(m_collectionModules[i] is CommonModule)                //  CommonModule 없는데?
                //{
                //    continue;
                //}
                control[i] = new ModuleButton(m_collectionModules[i]);
                control[i].Parent = this;
                control[i].Name = m_collectionModules[i].Name.ToString();
                control[i].Text = m_collectionModules[i].Name.ToString();
                control[i].Font = new Font(control[i].Font.FontFamily, 12);
                control[i].Tag = m_collectionModules[i].Name;
                control[i].Location = new Point(locationX, locationY);
                //control[i].Click += Module_Click;
                control[i].ModuleButtonClick += Module_Click;
                this.flowLayoutPanelModuleList.Controls.Add(control[i]);
                locationX += Configuration.ButtonSize.Width + 20;
            }
        }

        private void Module_Click(string ModuleName)
        {
            //ModuleButton modulebutton = sender as ModuleButton;
            foreach (Module module in m_collectionModules)
            {
                if(module.Name == ModuleName)
                {
                    if (module != null)
                    {
                        ShowSelectedModuleForm(module);
                    }
                }
            }
        }

        private void ShowSelectedModuleForm(Module form)
        {
            FormMaintMain maintMain = this.OwnerForm as FormMaintMain;
            //Form formModuleMaint =FormManager.GetMaintForm(form);
            maintMain.ShowSelectedModule(form);
            //this.Parent.panelContent.Controls.Clear();
            //formModuleMaint.TopLevel = false;
            //this.parent.panelContent.Controls.Add(formModuleMaint);
            //formModuleMaint.BringToFront();
            //formModuleMaint.Show();
        }
        #endregion

        #region CreateMaintButton
        private void CreateMaintButton()
        {
            foreach (GroupBoxButtonList buttonInterlock in Enum.GetValues(typeof(GroupBoxButtonList)))
            {
                BaseButton btn = new BaseButton();
                switch (buttonInterlock)
                {
                    case GroupBoxButtonList.Stop:
                        btn.Text = buttonInterlock.ToString();
                        btn.Name = string.Format("button", buttonInterlock.ToString());
                        btn.Size = new System.Drawing.Size(formConfiguration.ButtonSize.Width, formConfiguration.ButtonSize.Height);
                        btn.Location = new Point(5, formConfiguration.ButtonSize.Height - 10);
                        btn.Click += ButtonStop_Click;
                        break;
                    case GroupBoxButtonList.Intialize:
                        btn.Text = buttonInterlock.ToString();
                        btn.Name = string.Format("button", buttonInterlock.ToString());
                        btn.Size = new System.Drawing.Size(formConfiguration.ButtonSize.Width, formConfiguration.ButtonSize.Height);
                        btn.Location = new Point(5, formConfiguration.ButtonSize.Height * 2 - 5);
                        btn.Click += ButtonInitialize_Click;
                        break;                    
                    case GroupBoxButtonList.ClearJob:
                        btn.Text = buttonInterlock.ToString();
                        btn.Name = string.Format("button", buttonInterlock.ToString());
                        btn.Size = new System.Drawing.Size(formConfiguration.ButtonSize.Width, formConfiguration.ButtonSize.Height);
                        btn.Location = new Point(5, formConfiguration.ButtonSize.Height * 3);
                        btn.Click += ButtonClearJob_Click;
                        break;
                }
                groupBoxMaintEquipMent.Controls.Add(btn);
            }
        }
        #endregion



        //#endregion

        #region EventHandler
        private void ButtonStop_Click(object sender, EventArgs e)
        {

        }
        private void ButtonInitialize_Click(object sender, EventArgs e)
        {

        }
        private void ButtonClearJob_Click(object sender, EventArgs e)
        {

        }
        #endregion
    }
}
