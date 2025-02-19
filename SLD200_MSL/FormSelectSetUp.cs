using System;
using System.Drawing;
using System.Windows.Forms;

namespace SLD200_MSL
{

    #region Define
    public enum ButtonSeletSetUpType
    {
        Motion,
        IO,
        Equipment
    }


    #endregion
    public partial class FormSelectSetUp : FormContentBase
    {
        #region Field
        private FormSelectSetUpIO m_FormSelectSetUpIO;
        private FormSelectSetUpMotion m_FormSelectSetUpMotion;
        private FormEquipment m_FormEquipment;
        #endregion
        public FormSelectSetUp()
            : base()
        {
            InitializeComponent();

            CreatFlowPanelButton();
            this.FormSelectSetUpIO = new FormSelectSetUpIO();
            this.FormSelectSetUpMotion = new FormSelectSetUpMotion();
            this.FormEquipment = new FormEquipment();
        }
        #region Property
        public FormEquipment FormEquipment
        {
            get { return m_FormEquipment; }
            set { m_FormEquipment = value; }
        }
        public FormSelectSetUpIO FormSelectSetUpIO
        {
            get { return m_FormSelectSetUpIO; }
            set { m_FormSelectSetUpIO = value; }
        }
        public FormSelectSetUpMotion FormSelectSetUpMotion
        {
            get { return m_FormSelectSetUpMotion; }
            set { m_FormSelectSetUpMotion = value; }
        }


        #endregion

        #region EventHandler

        public void Button_Click(object sender, EventArgs e)
        {

            BaseButton button = sender as BaseButton;
            switch (button.Tag)
            {
                case ButtonSeletSetUpType.Motion:
                    FormSelectSetUpMotion.ShowPanelButton();
                    ShowForm(FormSelectSetUpMotion);
                    break;
                case ButtonSeletSetUpType.IO:
                    this.FormSelectSetUpIO.ShowPanelButton();
                    this.ShowForm(FormSelectSetUpIO);
                    break;
                case ButtonSeletSetUpType.Equipment:
                    this.FormEquipment.ShowPanelButton();
                    this.ShowForm(FormEquipment);
                    break;

                default:
                    break;
            }
        }
        #endregion

        #region Method
        public void CreatFlowPanelButton()
        {
            this.flowLayoutPanelButton.Controls.Clear();
            this.flowLayoutPanelButton.Location = new Point(0, 0);
            BaseButton[] control = new BaseButton[Enum.GetValues(typeof(ButtonSeletSetUpType)).Length];
            foreach (ButtonSeletSetUpType item in Enum.GetValues(typeof(ButtonSeletSetUpType)))
            {
                control[(int)item] = new BaseButton();
                control[(int)item].Parent = this;
                control[(int)item].Size = new Size(Configuration.ButtonSize.Width + 10, Configuration.ButtonSize.Height);
                control[(int)item].Name = item.ToString();
                control[(int)item].Text = item.ToString();
                control[(int)item].Tag = item;
                control[(int)(item)].Click += Button_Click;

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
