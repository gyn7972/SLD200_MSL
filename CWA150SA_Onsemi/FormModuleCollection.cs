
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
            this.Location = new Point(Configuration.ButtonSize.Width, 0);
            this.panelModules.Dock = DockStyle.Fill;
            this.FormBorderStyle = FormBorderStyle.None;
            m_collectionModules = Equipment.Modules;
            CreateModuleButton();
        }
        #endregion

        #region Method
       
        public void CreateModuleButton()
        {
            ModuleButton[] control = new ModuleButton[m_collectionModules.Count];
            int locationX = Configuration.ButtonSize.Width;
            int locationY = Configuration.ButtonSize.Height;
            for (int i = 0; i < m_collectionModules.Count; i++)
            {
                control[i] = new ModuleButton(m_collectionModules[i]);
                control[i].Parent = this;
                control[i].Name = m_collectionModules[i].Name.ToString();
                control[i].Text = m_collectionModules[i].Name.ToString();
                control[i].Font = new Font(control[i].Font.FontFamily, 12);
                control[i].ModuleButtonClick += Button_Click;
                control[i].Tag = m_collectionModules[i].Name;
                control[i].Size = new Size(150,150);
                control[i].Location = new Point(locationX, locationY);

                this.panelModules.Controls.Add(control[i]);

                //  2022. 03. 24.  SCH : X 위치에 무조건 150씩 더하던 코드... Module 이 많을때는 화면 넘어가기 때문에 변경하였음.
                locationX += Configuration.ButtonSize.Width + 20;
                if ( locationX > (this.Size.Width - Configuration.ButtonSize.Width) )
                {
                    locationX = Configuration.ButtonSize.Width;
                    locationY += control[i].Size.Height + 20;
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
