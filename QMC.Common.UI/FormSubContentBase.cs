using QMC.Common.Hmi;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace QMC.Common.UI
{
    public enum FormType
    {
        withButton,
        Content,
        Maint,
        Config
    }
    public partial class FormSubContentBase : Form
    {
        #region Define
        public enum ButtonType
        {
            Save,
            Load,
        }
        #endregion

        #region Field
        protected VisionImageViewer m_visionImageViewer;
        //protected Timer m_TimerUpdateDisplay;
        protected bool m_bStopUIUpdate;
        protected Thread m_UpdateThread;
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

                CreateButton();
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
                this.panelContent.Size = new System.Drawing.Size(Configuration.ContentSize.Width + 10, Configuration.FormContentSize.Height - Configuration.PanelSize.Height * 2 + 65);
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
            //this.m_TimerUpdateDisplay = new Timer();
            //this.m_TimerUpdateDisplay.Interval = 100;
            //this.m_TimerUpdateDisplay.Tick += VisionImageViewTimer_Tick;
            EnableUIUpdate(true);
            this.FormClosing += FormSubContentBase_FormClosing;
        }

        private void FormSubContentBase_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (var obj in this.panelContent.Controls)
            {
                JogControl jog = obj as JogControl;
                if (jog != null)
                {
                    jog.UpdateUI(false);
                }

                Form form = obj as Form;
                if (form != null)
                {
                    form.Close();
                }
            }

            m_bStopUIUpdate = true;

            if (m_visionImageViewer != null)
                m_visionImageViewer.StopUpdateTask();
        }

        public FormSubContentBase()
        {
            InitializeComponent();
        }
        #endregion

        #region Property
        public FormBaseConfiguration Configuration { get; set; }
        public bool Simulation { get; set; }
        #endregion

        #region Event Handler
        protected void FormSubContentBase_VisibleChanged(object sender, System.EventArgs e)
        {
            if (m_visionImageViewer != null)
            {
                if (this.Visible)
                {
                    m_visionImageViewer.StartUpdateTask();
                }
                else
                {
                    m_visionImageViewer.StopUpdateTask();
                }
            }
            //if (this.Visible)
            //{
            //    m_TimerUpdateDisplay.Start();
            //}
            //else
            //{
            //    m_TimerUpdateDisplay.Stop();
            //}

            EnableUIUpdate(this.Visible);

            OnUpdateDisplay();
        }

        public virtual void OnUpdateRecipe()
        {

        }

        protected virtual void OnUpdateDisplay()
        {

        }

        private void BtnLoadClick(object sender, EventArgs e)
        {
            OnLoadConfig();
        }

        private void BtnSaveClick(object sender, EventArgs e)
        {
            OnSaveConfig();
        }

        public virtual void EnableUIUpdate(bool bEnable)
        {
            if (bEnable)
            {
                StartUIUpdateThread();
            }
            else
            {
                StopUIUpdateThread();
            }
        }
        private void StartUIUpdateThread()
        {
            if (m_UpdateThread != null)
            {
                StopUIUpdateThread();
            }

            m_bStopUIUpdate = false;
            m_UpdateThread = new Thread(new ThreadStart(UpdateProc));
            m_UpdateThread.Name = "FormContentDetail_UIUpdateThread";

            m_UpdateThread.Start();
        }

        private void StopUIUpdateThread()
        {
            m_bStopUIUpdate = true;
            if (m_UpdateThread != null && !m_UpdateThread.Join(1000))
            {
                m_UpdateThread.Abort();
                m_UpdateThread = null;
            }

        }
        private void UpdateProc()
        {
            while (true)
            {
                if (m_bStopUIUpdate)
                    break;

                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new MethodInvoker(OnUpdateDisplay));
                }
                else
                {
                    OnUpdateDisplay();
                }

                Thread.Sleep(100);      //  2024.11.12.  SCH : #1호기는 200으로 변경. 나머지 호기는 모두 100으로 되어 있음.
            }
        }
        protected virtual void OnLoadConfig()
        {

        }

        protected virtual void OnSaveConfig()
        {

        }
        #endregion

        #region Method
        protected void CreateButton()
        {
            foreach (ButtonType buttonInterlock in Enum.GetValues(typeof(ButtonType)))
            {
                BaseButton btn = new BaseButton();
                switch (buttonInterlock)
                {
                    case ButtonType.Save:
                        btn.Text = buttonInterlock.ToString();
                        btn.Name = string.Format("button", buttonInterlock.ToString());
                        btn.Click += BtnSaveClick;
                        break;
                    case ButtonType.Load:
                        btn.Text = buttonInterlock.ToString();
                        btn.Name = string.Format("button", buttonInterlock.ToString());
                        btn.Click += BtnLoadClick;
                        break;
                }
                flowLayoutPanelButton.Controls.Add(btn);
            }
        }
        #endregion
    }
}
