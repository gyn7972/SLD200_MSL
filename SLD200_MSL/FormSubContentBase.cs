using QMC.Common.Hmi;
using System.Drawing;
using System.Windows.Forms;

namespace SLD200_MSL
{
    #region Define
    public enum FormType
    {
        withButton,
        Content,
        Maint,
        Config
    }
    #endregion

    public partial class FormSubContentBase : Form
    {
        #region Field
        protected VisionImageViewer m_visionImageViewer_Upper;      //  상부 비전 카메라
        protected VisionImageViewer m_visionImageViewer_Lower;      //  하부 비전 카메라
        #endregion

        #region Property
        public FormBaseConfiguration Configuration { get; set; }
        public bool Simulation { get; set; }
        #endregion

        #region Constructor
        public FormSubContentBase(string formType, string form)
        {
            Configuration = new FormBaseConfiguration();
            InitializeComponent();

            if (formType == FormType.withButton.ToString())
            {

                this.Size = new System.Drawing.Size((Point)Configuration.FormWithbuttonSize);
                this.ClientSize = new System.Drawing.Size((Point)Configuration.FormWithbuttonSize);
                this.Dock = DockStyle.Fill;
                this.Location = new System.Drawing.Point((Size)Configuration.ContentLocation); //0,0

                this.panelContent.Location = new System.Drawing.Point(0, Configuration.PanelSize.Height);
                this.panelContent.Size = new System.Drawing.Size(Configuration.PanelbuttonSize.Width, Configuration.FormWithbuttonSize.Height - Configuration.PanelSize.Height);
                this.panelContent.BackColor = Color.FromArgb(38, 38, 38);
                this.flowLayoutPanelButton.Location = new System.Drawing.Point(0, 0);
                this.flowLayoutPanelButton.Size = new System.Drawing.Size(Configuration.PanelbuttonSize.Width, Configuration.PanelSize.Height);

                this.Controls.Add(this.baseLabelTitle);
                this.baseLabelTitle.Size = Configuration.ButtonSize;
                this.baseLabelTitle.Location = new Point((Configuration.ContentSize.Width / 2) - (Configuration.ButtonSize.Width / 2), 0);
                this.baseLabelTitle.Text = form;

            //   this.flowLayoutPanelButton.BorderStyle = BorderStyle.Fixed3D;
            //    this.panelContent.BorderStyle = BorderStyle.Fixed3D;

                this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
                this.BackColor = Color.FromArgb(38, 38, 38);
                this.TopLevel = false;
                this.TopMost = true;

                this.FormBorderStyle = FormBorderStyle.None;
                this.StartPosition = FormStartPosition.Manual;


            }
            else if (formType == FormType.Content.ToString())
            {

                this.Size = new System.Drawing.Size(Configuration.FormContentSize.Width, Configuration.FormContentSize.Height);
                this.ClientSize = new System.Drawing.Size(Configuration.FormContentSize.Width, Configuration.FormContentSize.Height);
                this.Dock = DockStyle.Fill;
                this.Location = new System.Drawing.Point((Size)Configuration.ContentLocation); //0,0
                this.Controls.Add(this.baseLabelTitle);

                this.baseLabelTitle.Size = Configuration.ButtonSize;
                this.baseLabelTitle.Location = new Point((Configuration.ContentSize.Width / 2) - (Configuration.ButtonSize.Width / 2), 0);
                this.baseLabelTitle.Text = form;

                this.panelContent.Location = new System.Drawing.Point(0, Configuration.PanelSize.Height * 2);
                this.panelContent.Size = new System.Drawing.Size(Configuration.ContentSize.Width, Configuration.FormContentSize.Height - Configuration.PanelSize.Height);

                


                this.flowLayoutPanelButton.Location = new System.Drawing.Point(0, Configuration.PanelSize.Height);
                this.flowLayoutPanelButton.Size = new System.Drawing.Size(Configuration.PanelbuttonSize.Width, Configuration.PanelSize.Height);

             //   this.flowLayoutPanelButton.BorderStyle = BorderStyle.Fixed3D;
            //    this.panelContent.BorderStyle = BorderStyle.Fixed3D;
            
                this.BackColor = Color.FromArgb(38, 38, 38);
                this.panelContent.BackColor = Color.FromArgb(38, 38, 38);
                this.flowLayoutPanelButton.BackColor = Color.FromArgb(38, 38, 38);

                this.TopLevel = false;
                this.TopMost = true;
               this.FormBorderStyle = FormBorderStyle.None;
                this.StartPosition = FormStartPosition.Manual;
            }
            else if (formType == FormType.Maint.ToString())
            {

                this.Size = new System.Drawing.Size(Configuration.FormContentSize.Width, Configuration.FormContentSize.Height + Configuration.PanelSize.Height);
                
                this.ClientSize = new System.Drawing.Size(Configuration.FormContentSize.Width, Configuration.FormContentSize.Height);
                this.Dock = DockStyle.Fill;
                this.Location = new System.Drawing.Point((Size)Configuration.ContentLocation); //0,0
                this.Controls.Add(this.baseLabelTitle);
                //this.Controls.Add(this.panelContent);

                this.baseLabelTitle.Size = new Size(Configuration.ButtonSize.Width, Configuration.ButtonSize.Height + 2); ;
                this.baseLabelTitle.Location = new Point((Configuration.ContentSize.Width / 2) - (Configuration.ButtonSize.Width / 2), 0);
                this.baseLabelTitle.Text = form;

                this.panelContent.Location = new System.Drawing.Point(0, this.baseLabelTitle.Location.Y + this.baseLabelTitle.Height);
                this.panelContent.Size = new System.Drawing.Size(Configuration.ContentSize.Width+10, Configuration.FormContentSize.Height - Configuration.PanelSize.Height * 2 + 65);
                this.flowLayoutPanelButton.Hide();

                this.panelContent.BackColor = Color.FromArgb(38, 38, 38);
                this.BackColor = Color.FromArgb(38, 38, 38);

                this.TopLevel = false;
                this.TopMost = true;
                this.FormBorderStyle = FormBorderStyle.None;
                this.StartPosition = FormStartPosition.Manual;
    
                
            }
            else if (formType == FormType.Config.ToString())
            {

                this.Size = new System.Drawing.Size(Configuration.FormContentSize.Width, Configuration.FormContentSize.Height + Configuration.PanelSize.Height);

                this.ClientSize = new System.Drawing.Size(Configuration.FormContentSize.Width, Configuration.FormContentSize.Height);
                this.Dock = DockStyle.Fill;
                this.Location = new System.Drawing.Point((Size)Configuration.ContentLocation); //0,0
                this.Controls.Add(this.baseLabelTitle);
                //this.Controls.Add(this.panelContent);

                this.baseLabelTitle.Size = new Size(Configuration.ButtonSize.Width, Configuration.ButtonSize.Height + 2); ;
                this.baseLabelTitle.Location = new Point((Configuration.ContentSize.Width / 2) - (Configuration.ButtonSize.Width / 2), 0);
                this.baseLabelTitle.Text = form;

                this.panelContent.Location = new System.Drawing.Point(0, this.baseLabelTitle.Location.Y + this.baseLabelTitle.Height);
                this.panelContent.Size = new System.Drawing.Size(Configuration.ContentSize.Width + 10, Configuration.FormContentSize.Height - Configuration.PanelSize.Height * 2 + 65);
                //this.flowLayoutPanelButton.Hide();

                this.panelContent.BackColor = Color.FromArgb(38, 38, 38);
                this.BackColor = Color.FromArgb(38, 38, 38);

                this.TopLevel = false;
                this.TopMost = true;
                this.FormBorderStyle = FormBorderStyle.None;
                this.StartPosition = FormStartPosition.Manual;
            }

            this.VisibleChanged += FormSubContentBase_VisibleChanged;

        }

        public FormSubContentBase()
        {
            InitializeComponent();
        }
        #endregion

        #region Event Handler
        private void FormSubContentBase_VisibleChanged(object sender, System.EventArgs e)
        {
            if (m_visionImageViewer_Upper != null)
            {
                if (this.Visible)
                {
                    m_visionImageViewer_Upper.StartUpdateTask();
                }
                else
                {
                    m_visionImageViewer_Upper.StopUpdateTask();
                }
            }

            if (m_visionImageViewer_Lower != null)
            {
                if (this.Visible)
                {
                    m_visionImageViewer_Lower.StartUpdateTask();
                }
                else
                {
                    m_visionImageViewer_Lower.StopUpdateTask();
                }
            }

            OnVisibleChanged();
        }

        protected virtual void OnVisibleChanged()
        {

        }

        public virtual void OnUpdateRecipe()
        {

        }
        #endregion
    }
}
