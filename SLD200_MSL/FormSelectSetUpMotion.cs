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
    #region Define

    public enum ButtonSeletSetUpMotionType
    {
        Board,
        Axis,
    }
    #endregion


    public partial class FormSelectSetUpMotion : FormSubContentBase
    {
        #region Field
        private FormAxisConfiguration m_FormAxisConfiguration;
        private FormMotionBoardConfiguration m_FormMotionBoardConfiguration;
        #endregion

        #region Constructor

        public FormSelectSetUpMotion()
            : base(FormType.withButton.ToString(),"SetUp Motion")
        {
            InitializeComponent();
            ShowPanelButton();

            this.BackColor = Configuration.PanelBackColor;

            FormAxisConfiguration = new FormAxisConfiguration();
            FormMotionBoardConfiguration = new FormMotionBoardConfiguration();
        }
        #endregion


        #region Property
        public FormAxisConfiguration FormAxisConfiguration
        {
            get { return m_FormAxisConfiguration; }
            set { m_FormAxisConfiguration = value; }
        }
       
        public FormMotionBoardConfiguration FormMotionBoardConfiguration
        {
            get { return m_FormMotionBoardConfiguration; }
            set { m_FormMotionBoardConfiguration = value; }
        }
        #endregion

        #region EventHandler
       
        #endregion

        #region Method
        private void Button_Click(object sender, EventArgs e)
        {
            BaseButton button = sender as BaseButton;

            switch (button.Tag)
            {
                case ButtonSeletSetUpMotionType.Board:
                    ShowForm(FormMotionBoardConfiguration);
                    break;

                case ButtonSeletSetUpMotionType.Axis:
                    ShowForm(FormAxisConfiguration);
                    break;
            
                default:
                    break;
            }
        }
        public void ShowPanelButton()
        {
            this.flowLayoutPanelButton.Controls.Clear();
            BaseButton[] control = new BaseButton[Enum.GetValues(typeof(ButtonSeletSetUpMotionType)).Length];
            foreach (ButtonSeletSetUpMotionType item in Enum.GetValues(typeof(ButtonSeletSetUpMotionType)))
            {
                control[(int)item] = new BaseButton();
                control[(int)item].Parent = this;
                control[(int)item].Size = new Size(Configuration.ButtonSize.Width, Configuration.ButtonSize.Height);
                control[(int)item].Name = item.ToString();
                control[(int)item].Text = item.ToString();
                control[(int)item].Tag = item;
                control[(int)item].Font = new System.Drawing.Font("Tahoma", 9.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
                control[(int)item].BackColor = Color.White;
                control[(int)item].ForeColor = Color.Black;
                control[(int)(item)].Click += this.Button_Click;

                this.flowLayoutPanelButton.Controls.Add(control[(int)item]);
            }
        }

        public  void ShowForm(Form form)
        {
            this.panelContent.Controls.Clear();
            form.TopLevel = false;

            this.panelContent.Controls.Add(form);

            form.BringToFront();
            form.Activate();
            form.Show();
        }
        #endregion
    }
}
