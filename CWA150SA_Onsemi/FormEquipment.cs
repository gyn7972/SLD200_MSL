using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CWA150SA_Onsemi300
{
    #region Define
    public enum ButtonSeletSetUpEquipmentType
    {
        Configuration,
    }
    #endregion
    public partial class FormEquipment : FormSubContentBase
    {
        #region Field
        private FormEquipmentConfiguraton m_FormEquipmentConfiguration;
        #endregion

        #region Constructor
        public FormEquipment()
            :base(FormType.withButton.ToString(), "Equipment")
        {
            InitializeComponent();
            ShowPanelButton();
            this.FormEquipmentConfiguration = new FormEquipmentConfiguraton();
        }
        #endregion

        #region Porperty

        public FormEquipmentConfiguraton FormEquipmentConfiguration
        {
            get { return m_FormEquipmentConfiguration; }
            set { m_FormEquipmentConfiguration = value; }
        }

        #endregion

        #region Event Handler
        private void Button_Click(object sender, EventArgs e)
        {
            BaseButton button = sender as BaseButton;

            switch (button.Tag)
            {
                case ButtonSeletSetUpEquipmentType.Configuration:
                    this.ShowForm(FormEquipmentConfiguration);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Method
        public void ShowPanelButton()
        {
            this.flowLayoutPanelButton.Controls.Clear();
            BaseButton[] control = new BaseButton[Enum.GetValues(typeof(ButtonSeletSetUpEquipmentType)).Length];
            foreach (ButtonSeletSetUpEquipmentType item in Enum.GetValues(typeof(ButtonSeletSetUpEquipmentType)))
            {
                control[(int)item] = new BaseButton();
                control[(int)item].Parent = this;
                control[(int)item].Size = new Size(Configuration.ButtonSize.Width, Configuration.ButtonSize.Height);
                control[(int)item].Name = item.ToString();
                control[(int)item].Text = item.ToString();
                control[(int)item].Tag = item;
                control[(int)(item)].Click += this.Button_Click;

                this.flowLayoutPanelButton.Controls.Add(control[(int)item]);
            }
        }
        public void ShowForm(Form form)
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
