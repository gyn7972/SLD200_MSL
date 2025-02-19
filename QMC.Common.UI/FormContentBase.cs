using System;
using System.Drawing;
using System.Windows.Forms;

namespace QMC.Common.UI
{
    public partial class FormContentBase : Form
    {
        public FormBaseConfiguration Configuration { get; set; }
        public FormContentBase()
        {
            Configuration = new FormBaseConfiguration();
            InitializeComponent();

            this.Size = new System.Drawing.Size((Point)Configuration.ContentSize);
            this.Dock = DockStyle.Fill;
            this.ClientSize = new System.Drawing.Size((Point)Configuration.ContentSize);
            this.Location = new System.Drawing.Point((Size)Configuration.ContentLocation);
            //this.BackColor = Color.FromArgb(38, 38, 38);
            BackColor = Color.FromArgb(220, 220, 220);


            this.panelContent.Location = new System.Drawing.Point(0, Configuration.PanelSize.Height);
            this.panelContent.Size = new System.Drawing.Size(Configuration.MainSize.Width, Configuration.ContentSize.Height - Configuration.PanelSize.Height);
            //this.panelContent.BackColor = Color.FromArgb(38, 38, 38);
            this.panelContent.BackColor = Color.FromArgb(220, 220, 220);

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();



            this.flowLayoutPanelButton.Location = new Point(0, 0);
            this.flowLayoutPanelButton.Size = new System.Drawing.Size(Configuration.MainSize.Width, Configuration.PanelSize.Height);
            this.flowLayoutPanelButton.BackColor = Color.FromArgb(38, 38, 38);
            this.BackColor = Color.DarkGray;
            this.TopLevel = false;
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            this.Dock = DockStyle.Fill;

        }


    }
}
