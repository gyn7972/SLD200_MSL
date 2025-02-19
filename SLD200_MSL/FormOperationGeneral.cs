using QMC.Common;
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
    public partial class FormOperationGeneral : FormSubContentBase
    {
        #region Field
        private Dictionary<Module, Form> m_DicForms;
        private ModuleCollection m_collectionModules;

        #region Property
        FormBaseConfiguration formConfiguration { get; set; }

        #endregion
        public FormOperationGeneral()
            : base(FormType.withButton.ToString(), "General")
        {
            InitializeComponent();

            this.BackColor = Configuration.PanelBackColor;

            formConfiguration = new FormBaseConfiguration();
            m_collectionModules = Equipment.Modules;
            CreateModuleList();
            m_DicForms = new Dictionary<Module, Form>();
            foreach (Module module in m_collectionModules)
            {
                Form Subform = FormManager.GetOperationForm(module);
                m_DicForms.Add(module, Subform);
            }
            this.flowLayoutPanelModuleList.Size = new Size(formConfiguration.ListBoxSize.Width * 2, formConfiguration.ListBoxSize.Height);
            this.flowLayoutPanelModuleList.Location = new Point(50, Configuration.ContentLocation.Y + Configuration.PanelSize.Height);
            this.flowLayoutPanelModuleList.BackColor = Configuration.PanelBackColor;
        }
        #endregion

        #region Method
        public void CreateModuleList()
        {
            //  원래 것
            //ModuleButton[] control = new ModuleButton[m_collectionModules.Count];
            //int locationX = Configuration.ButtonSize.Width;
            //int locationY = Configuration.ButtonSize.Height;
            //for (int i = 0; i < m_collectionModules.Count; i++)
            //{
            //    control[i] = new ModuleButton(m_collectionModules[i]);
            //    control[i].Parent = this;
            //    control[i].Name = m_collectionModules[i].Name.ToString();
            //    control[i].Text = m_collectionModules[i].Name.ToString();
            //    //control[i].Font = new Font(control[i].Font.FontFamily, 12);
            //    control[i].Font = new System.Drawing.Font("Tahoma", 9.0F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            //    control[i].Tag = m_collectionModules[i].Name;
            //    control[i].Location = new Point(locationX, locationY);
            //    control[i].ModuleButtonClick += Button_Click;
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
                control[i].Tag = m_collectionModules[i].Name;
                control[i].Size = m_nModuleButton_Size;
                control[i].Location = new Point(locationX, locationY);
                control[i].ModuleButtonClick += Button_Click;
                this.flowLayoutPanelModuleList.Controls.Add(control[i]);

                //  2022. 03. 24.  SCH : X 위치에 무조건 20씩 더하던 코드... Module 이 많을때는 화면 넘어가기 때문에 변경하였음.
                //locationX += Configuration.ButtonSize.Width + 20;
                locationX += m_nModuleButton_Size.Width + (m_nModuleButton_Size.Width / 4);
                //if ( locationX > (this.Size.Width - Configuration.ButtonSize.Width) )
                if (locationX > (ClientSize.Width - m_nModuleButton_Size.Width))
                //if (locationX > (800 - m_nModuleButton_Size.Width))
                {
                    locationX = m_nModuleButton_Size.Width / 4;
                    locationY += m_nModuleButton_Size.Height + (m_nModuleButton_Size.Height / 4);
                }
            }
        }

        public void SelectedModule(Module module)
        {
            this.panelContent.Controls.Clear();
            if (m_DicForms.ContainsKey(module) || module != null)
            {
                ShowForm(m_DicForms[module]);
            }
        }

        public void ShowForm(Form form)
        {
            if (form == null)
            {
                return;
            }

            this.BackColor = Configuration.PanelBackColor;

            this.panelContent.Controls.Clear();
            form.TopLevel = false;
            this.Parent.Controls.Add(form);
            //this.panelContent.Controls.Add(form);
            form.BringToFront();
            form.Show();
        }
        #endregion

        #region EventHandler
        public void Button_Click(string strButtonName)
        {
            for (int i = 0; i < m_collectionModules.Count; i++)
            {
                if (strButtonName == m_collectionModules[i].Name)
                {
                    SelectedModule(m_collectionModules[i]);
                    break;
                }
            }
        }
        #endregion
    }
}
