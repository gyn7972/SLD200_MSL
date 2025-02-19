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
    public enum ButtonSeletSetUpIOType
    {
        IOBoard,
        IOModule,
        DigitalIO
    }


    public partial class FormSelectSetUpIO : FormSubContentBase

    {
        #region Field
        private FormIOBoard m_FormIOBoard;
        private FormIOModule m_FormIOModule;
        private FormDigitalIO m_FormDigitalIO;
        #endregion

        #region Constructor

        public FormSelectSetUpIO()
            : base(FormType.withButton.ToString(),"SetUp IO")
        {
            InitializeComponent();
            ShowPanelButton();

            this.BackColor = Configuration.PanelBackColor;

            this.FormDigitalIO = new FormDigitalIO();
            this.FormIOModule = new FormIOModule();
            this.FormIOBoard = new FormIOBoard();
        }
        #endregion

        #region Property

        public FormIOBoard FormIOBoard
        {
            get { return m_FormIOBoard; }
            set { this.m_FormIOBoard = value; }
        }
        public FormIOModule FormIOModule
        {
            get { return m_FormIOModule; }
            set { this.m_FormIOModule = value; }
        }
        public FormDigitalIO FormDigitalIO
        {
            get { return m_FormDigitalIO; }
            set { this.m_FormDigitalIO = value; }
        }
        #endregion

        #region EventHandeler
        private void Button_Click(object sender, EventArgs e)
        {
            BaseButton button = sender as BaseButton;

            switch (button.Tag)
            {
                case ButtonSeletSetUpIOType.IOBoard:
                    ShowForm(FormIOBoard);
                    break;
                case ButtonSeletSetUpIOType.IOModule:
                    ShowForm(FormIOModule);
                    break;
                case ButtonSeletSetUpIOType.DigitalIO:
                    ShowForm(FormDigitalIO);
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
            BaseButton[] control = new BaseButton[Enum.GetValues(typeof(ButtonSeletSetUpIOType)).Length];
            foreach (ButtonSeletSetUpIOType item in Enum.GetValues(typeof(ButtonSeletSetUpIOType)))
            {
                control[(int)item] = new BaseButton();
                control[(int)item].Parent = this;
                control[(int)item].Size = new Size(Configuration.ButtonSize.Width, Configuration.ButtonSize.Height);
                control[(int)item].Name = item.ToString();
                control[(int)item].Text = item.ToString();
                control[(int)item].Tag = item;
                control[(int)item].BackColor = Color.White;
                control[(int)item].ForeColor = Color.Black;
                control[(int)item].Font = new System.Drawing.Font("Tahoma", 9.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
                control[(int)(item)].Click += this.Button_Click;

                this.flowLayoutPanelButton.Controls.Add(control[(int)item]);
            }
        }
        public void ShowForm(Form form)
        {
            this.panelContent.Controls.Clear();
            form.TopLevel = false;

            //panelContent.BringToFront();

            this.panelContent.Controls.Add(form);

            form.BringToFront();
            form.Show();
        }
        #endregion
    }
}
