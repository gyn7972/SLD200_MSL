using QMC.Common;
using QMC.Common.Modules;
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
    public partial class FormCommonModuleConfig : FormSubContentBase
    {
        public enum ButtonType  
        {
            Save,
            Load,
        }

        private CommonModule m_Module;
        private PropertyGridControl m_PropertyGridControl;
        //private LineInfoControl m_LineInfoControl;
        public FormCommonModuleConfig(Module module)
            : base(FormType.withButton.ToString(), module.Name)
        {
            InitializeComponent();
            m_Module = module as CommonModule;
            this.flowLayoutPanelButton.FlowDirection = FlowDirection.RightToLeft;
            this.flowLayoutPanelButton.Location = new Point(0, this.baseLabelTitle.Location.Y + this.baseLabelTitle.Height);
            this.flowLayoutPanelButton.BackColor = Configuration.PanelBackColor;

            this.panelContent.Location = new Point(Configuration.ContentLocation.X, Configuration.PanelbuttonSize.Height);
            this.panelContent.Hide();
            m_PropertyGridControl = new PropertyGridControl();
            m_PropertyGridControl.Location = new Point(Configuration.ContentLocation.X, this.flowLayoutPanelButton.Location.Y + this.flowLayoutPanelButton.Height + Configuration.ControlGap + baseLabelModuleConfiguration.Height);
            m_PropertyGridControl.SetGroupBoxName("Parameter");
            this.Controls.Add(m_PropertyGridControl);

            /*m_LineInfoControl = new LineInfoControl(m_Module);
            m_LineInfoControl.Location = new Point(m_PropertyGridControl.Location.X + m_PropertyGridControl.Size.Width + Configuration.ControlGap, m_PropertyGridControl.Location.Y);
            this.Controls.Add(m_LineInfoControl);
            CreateButton();*/

            if (m_Module != null)
                m_PropertyGridControl.SetData(m_Module.Config);
            //m_LineInfoControl.SetMesInfo(m_Module.Config.MesInfo);
        }

        public void CreateButton()
        {
            foreach (FormCommonModuleConfig.ButtonType buttonInterlock in Enum.GetValues(typeof(FormCommonModuleConfig.ButtonType)))
            {
                BaseButton btn = new BaseButton();
                switch (buttonInterlock)
                {
                    case FormCommonModuleConfig.ButtonType.Save:
                        btn.Text = buttonInterlock.ToString();
                        btn.Name = string.Format("button", buttonInterlock.ToString());
                        btn.Click += buttonSave_Click;
                        break;
                    case FormCommonModuleConfig.ButtonType.Load:
                        btn.Text = buttonInterlock.ToString();
                        btn.Name = string.Format("button", buttonInterlock.ToString());
                        btn.Click += buttonLoad_Click;
                        break;
                }
                flowLayoutPanelButton.Controls.Add(btn);
            }
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            Equipment.LoadConfig(); // 참고 : param load
            DataManager.Instance.ApplyConfigData(m_Module);
            m_PropertyGridControl.SetData(m_Module.Config);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            DataManager.Instance.UpdateConfigData(m_Module); // 참고 : param save
            Equipment.SaveConfig(Equipment.GetCurrentRecipe().Name);
            m_Module.Initialize();
            //Equipment.Initialize();//?
        }

    }
}
