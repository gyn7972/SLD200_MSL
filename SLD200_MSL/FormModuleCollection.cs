
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
    public delegate void SelectedModule(Module module);

    public delegate void SelectedVisionModule(Module module);
    public partial class FormModuleCollection : Form
    {

        #region Field
        //public FormConfiguratinModule FormConfiguratinModule { get; set; }
        public FormMaintMain FormMaint { get; set; }

        public ModuleCollection m_collectionModules;
        public FormConfiguration FormConfiguration { get; set; }

        public SelectedModule selectedModule { get; set; }

        public SelectedVisionModule selectedVisionModule { get; set; }
        public FormBaseConfiguration Configuration { get; set; }
        #endregion

        #region Constructor
        public FormModuleCollection()
        {
            Configuration = new FormBaseConfiguration();

            InitializeComponent();

            this.BackColor = Configuration.PanelBackColor;

            //this.Location = new Point(Configuration.ButtonSize.Width, 0);
            this.Location = new Point(0, 0);

            //this.panelModules.Dock = DockStyle.Fill;
            this.FormBorderStyle = FormBorderStyle.None;
            m_collectionModules = Equipment.Modules;
            this.panelModules.Size = new Size(Configuration.ListBoxSize.Width * 2, Configuration.ListBoxSize.Height);
            //this.panelModules.Location = new Point(50, Configuration.ContentLocation.Y + Configuration.PanelSize.Height);
            this.panelModules.Location = new Point(50, Configuration.ContentLocation.Y + Configuration.PanelSize.Height);
            this.panelModules.BackColor = Configuration.PanelBackColor;

            CreateModuleButton();
        }
        #endregion

        #region Method
       
        public void CreateModuleButton()
        {
            Size m_nModuleButton_Size = new Size(150, 170);
            int m_nColMax = 3;                                      //  2025. 01. 28.  SCH : 3개씩 나열
            int m_nRowCount = 0;
            
            ModuleButton[] control = new ModuleButton[m_collectionModules.Count];
            //int locationX = Configuration.ButtonSize.Width;
            //int locationY = Configuration.ButtonSize.Height;
            int locationX = m_nModuleButton_Size.Width / 4;
            int locationY = m_nModuleButton_Size.Height / 4;

            //  모듈 패널 크기 변경 (가로 3개씩)
            m_nRowCount = m_collectionModules.Count / m_nColMax;
            if (m_collectionModules.Count % m_nColMax != 0)
            {
                m_nRowCount++;
            }
            this.ClientSize = new Size((m_nModuleButton_Size.Width * m_nColMax) + ((m_nModuleButton_Size.Width / 4) * (m_nColMax + 1)) + (m_nModuleButton_Size.Width * 2),
                                        (m_nModuleButton_Size.Height * m_nRowCount) + ((m_nModuleButton_Size.Height / 4) * (m_nRowCount + 1)));

            for (int i = 0; i < m_collectionModules.Count; i++)
            {
                control[i] = new ModuleButton(m_collectionModules[i]);
                control[i].Parent = this;
                control[i].Name = m_collectionModules[i].Name.ToString();
                control[i].Text = m_collectionModules[i].Name.ToString();
                //control[i].Font = new Font(control[i].Font.FontFamily, 12);
                control[i].Font = new System.Drawing.Font("Tahoma", 11.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
                control[i].ModuleButtonClick += Button_Click;
                control[i].Tag = m_collectionModules[i].Name;
                control[i].Size = m_nModuleButton_Size;
                control[i].Location = new Point(locationX, locationY);

                this.panelModules.Controls.Add(control[i]);

                //  2022. 03. 24.  SCH : X 위치에 무조건 150씩 더하던 코드... Module 이 많을때는 화면 넘어가기 때문에 변경하였음.
                //locationX += Configuration.ButtonSize.Width + 20;
                locationX += m_nModuleButton_Size.Width + (m_nModuleButton_Size.Width / 4);
                //if ( locationX > (this.Size.Width - Configuration.ButtonSize.Width) )
                if ( locationX > (ClientSize.Width - m_nModuleButton_Size.Width))
                {
                    locationX = m_nModuleButton_Size.Width / 4;
                    locationY += m_nModuleButton_Size.Height + (m_nModuleButton_Size.Height / 4);
                }
            }
        }
        #endregion


        #region EventHandler
        public void Button_Click(string strButtonName)
        {
            for (int i = 0; i < m_collectionModules.Count; i++)
            {
                if (strButtonName == m_collectionModules[i].Name)
                {
                    selectedModule(m_collectionModules[i]);
                }
            }
            this.Hide();
        }
        #endregion
    }
}
