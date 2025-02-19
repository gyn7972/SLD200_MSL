using QMC.Common;
using QMC.Common.VisionPart;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SLD200_MSL
{
    #region FormVisionCompensatorConfig
    public partial class FormVisionCompensatorConfig : FormSubContentBase
    {
        #region Field
        public VisionCompensator m_Owner;
        public VisionCompensatorOffsetConfigControl m_OffsetControl;
        #endregion

        #region Constructor
        public FormVisionCompensatorConfig(Part part)
            : base(FormType.withButton.ToString(), part.Name)
        {
            this.m_Owner = part as VisionCompensator;
            this.m_OffsetControl = new VisionCompensatorOffsetConfigControl(m_Owner.Config);
            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();



            this.panelContent.Visible = false;
            this.panelContent.Size = new Size();

            this.Controls.Add(this.m_OffsetControl);

            this.flowLayoutPanelButton.FlowDirection = FlowDirection.RightToLeft;
            this.flowLayoutPanelButton.Location = new Point(0, this.baseLabelTitle.Location.Y + this.baseLabelTitle.Height);
            this.m_OffsetControl.Location = new Point(Configuration.ContentLocation.X, this.flowLayoutPanelButton.Location.Y + this.flowLayoutPanelButton.Height + Configuration.ControlGap);

            CreateButton();
        }
        #endregion

        #region Method
        public void CreateButton()
        {
            //foreach (FormDieLoderConfig.ButtonType button in Enum.GetValues(typeof(FormDieLoderConfig.ButtonType)))
            //{
            //    BaseButton btn = new BaseButton();
            //    switch (button)
            //    {
            //        case FormDieLoderConfig.ButtonType.Save:
            //            btn.Text = button.ToString();
            //            btn.Name = string.Format("button", button.ToString());
            //            btn.Click += SaveButton_Click;
            //            break;
            //        case FormDieLoderConfig.ButtonType.Load:
            //            btn.Text = button.ToString();
            //            btn.Name = string.Format("button", button.ToString());
            //            btn.Click += LoadButton_Click;
            //            break;
            //    }
            //    flowLayoutPanelButton.Controls.Add(btn);
            //}
        }
        #endregion

        #region Event Handler
        private void LoadButton_Click(object sender, EventArgs e)
        {
            Module module = m_Owner.Owner;
            if (module != null)
            {
                module.UpdateConfigData();
                DataManager.Instance.ApplyConfigData(module);
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {

            Module module = m_Owner.Owner;
            if (module != null)
            {
                DataManager.Instance.UpdateConfigData(module);
                module.SaveConfigData();
            }
        }
        #endregion
    }
    #endregion

}