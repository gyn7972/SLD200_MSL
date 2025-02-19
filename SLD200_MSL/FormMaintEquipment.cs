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
        //private EngineerButton m_EngineerButton { get; set; }

        public Form OwnerForm { get; set; }



        #endregion

        public FormMaintEquipment()
            : base(FormType.Content.ToString(), "SLD-200_MSL")
        {
            InitializeComponent();
            m_collectionModules = Equipment.Modules;
            formConfiguration = new FormBaseConfiguration();
            //m_EngineerButton = new EngineerButton();
            CreateModuleList();
            CreateMaintButton();

            this.BackColor = Configuration.PanelBackColor;  

            #region ControlConstructor
            this.panelContent.Controls.Add(this.flowLayoutPanelModuleList);
            //this.panelContent.Controls.Add(this.groupBoxMaintEquipMent);
            //this.panelContent.Controls.Add(m_EngineerButton);
            this.panelContent.Size = new Size(formConfiguration.ContentSize.Width + 10, formConfiguration.ContentSize.Height);
            this.panelContent.Size = new Size(0,Configuration.PanelSize.Height);

            this.flowLayoutPanelButton.Location = new Point(0,20);
            this.flowLayoutPanelButton.Size = new Size(formConfiguration.ContentSize.Width + 10, formConfiguration.PanelbuttonSize.Height);
            this.flowLayoutPanelButton.BackColor = Configuration.PanelBackColor;

            this.panelContent.Location = new Point(0, this.flowLayoutPanelButton.Location.Y + this.flowLayoutPanelButton.Height);
            this.panelContent.Size = new Size(formConfiguration.ContentSize.Width, formConfiguration.ContentSize.Height - this.flowLayoutPanelButton.Height);
            this.panelContent.BackColor = Configuration.PanelBackColor;

            this.flowLayoutPanelModuleList.Size = new Size(formConfiguration.ListBoxSize.Width * 2, formConfiguration.ListBoxSize.Height);
            this.flowLayoutPanelModuleList.Location = new Point(50, Configuration.ContentLocation.Y + Configuration.PanelSize.Height);
            this.flowLayoutPanelModuleList.BackColor = Configuration.PanelBackColor;

            this.groupBoxMaintEquipMent.Size = new Size(formConfiguration.ButtonSize.Width +10, formConfiguration.ButtonSize.Height*4 +10);
            this.groupBoxMaintEquipMent.Location = new Point(this.panelContent.Width - this.groupBoxMaintEquipMent.Width-10, Configuration.ContentLocation.Y + Configuration.PanelSize.Height);
            this.groupBoxMaintEquipMent.ForeColor = Color.White;

            //this.m_EngineerButton.Location = new Point(this.groupBoxMaintEquipMent.Location.X, this.groupBoxMaintEquipMent.Location.Y + this.groupBoxMaintEquipMent.Height + Configuration.ControlGap);
            #endregion
        }
        #endregion

        #region Method
        private void CreateModuleList()
        {
            //  원래 것
            //ModuleButton[] control = new ModuleButton[m_collectionModules.Count];
            //int locationX = Configuration.ButtonSize.Width;
            //int locationY = Configuration.ButtonSize.Height;
            //for (int i = 0; i < m_collectionModules.Count; i++)
            //{
            //    //if(m_collectionModules[i] is CommonModule)                //  CommonModule 없는데?
            //    //{
            //    //    continue;
            //    //}
            //    control[i] = new ModuleButton(m_collectionModules[i]);
            //    control[i].Parent = this;
            //    control[i].Name = m_collectionModules[i].Name.ToString();
            //    control[i].Text = m_collectionModules[i].Name.ToString();
            //    //control[i].Font = new Font(control[i].Font.FontFamily, 12);
            //    control[i].Font = new System.Drawing.Font("Tahoma", 9.0F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            //    control[i].Tag = m_collectionModules[i].Name;
            //    control[i].Location = new Point(locationX, locationY);
            //    //control[i].Click += Module_Click;
            //    control[i].ModuleButtonClick += Module_Click;
            //    this.flowLayoutPanelModuleList.Controls.Add(control[i]);
            //    locationX += Configuration.ButtonSize.Width + 20;
            //}

            //  변경된 것
            Size m_nModuleButton_Size = new Size(150, 170);
            int m_nColMax = 3;                                      //  2025. 01. 28.  SCH : 3개씩 나열
            int m_nRowCount = 0;

            ModuleButton[] control = new ModuleButton[m_collectionModules.Count];
            int locationX = m_nModuleButton_Size.Width / 4;
            int locationY = m_nModuleButton_Size.Height / 4;

            //  모듈 패널 크기 변경 (가로 3개씩)
            m_nRowCount = m_collectionModules.Count / m_nColMax;
            if (m_collectionModules.Count % m_nColMax != 0)
            {
                m_nRowCount++;
            }
            this.ClientSize = new Size((m_nModuleButton_Size.Width * m_nColMax) + ((m_nModuleButton_Size.Width / 4) * (m_nColMax + 1)),
                                        (m_nModuleButton_Size.Height * m_nRowCount) + ((m_nModuleButton_Size.Height / 4) * (m_nRowCount + 1)));

            for (int i = 0; i < m_collectionModules.Count; i++)
            {
                control[i] = new ModuleButton(m_collectionModules[i]);
                control[i].Parent = this;
                control[i].Name = m_collectionModules[i].Name.ToString();
                control[i].Text = m_collectionModules[i].Name.ToString();
                //control[i].Font = new Font(control[i].Font.FontFamily, 12);
                control[i].Font = new System.Drawing.Font("Tahoma", 11.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
                control[i].ModuleButtonClick += Module_Click;
                control[i].Tag = m_collectionModules[i].Name;
                control[i].Size = m_nModuleButton_Size;
                control[i].Location = new Point(locationX, locationY);
                this.flowLayoutPanelModuleList.Controls.Add(control[i]);
                
                //  2022. 03. 24.  SCH : X 위치에 무조건 150씩 더하던 코드... Module 이 많을때는 화면 넘어가기 때문에 변경하였음.
                //locationX += Configuration.ButtonSize.Width + 20;
                locationX += m_nModuleButton_Size.Width + (m_nModuleButton_Size.Width / 4);
                //if ( locationX > (this.Size.Width - Configuration.ButtonSize.Width) )
                if (locationX > (ClientSize.Width - m_nModuleButton_Size.Width))
                {
                    locationX = m_nModuleButton_Size.Width / 4;
                    locationY += m_nModuleButton_Size.Height + (m_nModuleButton_Size.Height / 4);
                }
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
