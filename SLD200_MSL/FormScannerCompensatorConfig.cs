using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SLD200_MSL;
using QMC.Common;
using QMC.Common.Parts;

namespace SLD200_MSL
{
    public partial class FormScannerCompensatorConfig : FormSubContentBase
    {
        #region Field
        public ScannerCompensator m_Owner;
        public NormalDataConfigControl m_PropertyGridConfig;
        #endregion

        #region Constructor
        public FormScannerCompensatorConfig(Part part)
            : base(FormType.withButton.ToString(), part.Name)
        {
            this.m_Owner = part as ScannerCompensator;

            this.panelContent.Visible = false;
            this.panelContent.Size = new Size();

            this.flowLayoutPanelButton.FlowDirection = FlowDirection.RightToLeft;
            this.flowLayoutPanelButton.Location = new Point(0, this.baseLabelTitle.Location.Y + this.baseLabelTitle.Height);

            this.m_PropertyGridConfig = new NormalDataConfigControl();
            this.m_PropertyGridConfig.Location = new Point(flowLayoutPanelButton.Location.X + Configuration.ControlGap, flowLayoutPanelButton.Location.Y + flowLayoutPanelButton.Size.Height + Configuration.ControlGap);
            this.m_PropertyGridConfig.SetData(m_Owner.Config);
            this.m_PropertyGridConfig.SetGroupBoxName(" Config ");
            this.Controls.Add(this.m_PropertyGridConfig);

            InitializeComponent();

            CreateButton();
        }
        #endregion

        #region Method
        public void CreateButton()
        {
            {
                BaseButton btn = new BaseButton();
                btn.Text = "Save";
                btn.Name = "buttonSave";
                btn.Click += SaveButton_Click;
                flowLayoutPanelButton.Controls.Add(btn);
            }

            {
                BaseButton btn = new BaseButton();
                btn.Text = "Load";
                btn.Name = "buttonLoad";
                btn.Click += LoadButton_Click;
                flowLayoutPanelButton.Controls.Add(btn);
            }
        }
        #endregion

        #region Event Handler
        private void LoadButton_Click(object sender, EventArgs e)
        {
            Module module = m_Owner.Owner;
            if (module != null)
            {
                Equipment.LoadConfig();
                DataManager.Instance.ApplyConfigData(module);
                //m_visionCalScaleControl.UpdateUI(m_Owner.Config);
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Module module = m_Owner.Owner;
            if (module != null)
            {
                //m_visionCalScaleControl.UpdateConfigData();
                DataManager.Instance.UpdateConfigData(module);
                Equipment.SaveConfig();
            }
        }
        #endregion
    }
}
