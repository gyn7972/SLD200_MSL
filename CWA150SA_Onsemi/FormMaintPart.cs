using QMC.Common;
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
    public delegate void EventHandlerMaintPartForm();
    public partial class FormMaintPart : FormSubContentBase
    {
        public event EventHandlerMaintPartForm FormMaintPartHide;
        Part part;
        public FormMaintPart(Part parts)
            :base(FormType.Content.ToString(), parts.Name)
        {
            part = parts;
            InitializeComponent();
           // this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.flowLayoutPanelButton.Enabled = false;
            this.panelContent.Location = new System.Drawing.Point(0,Configuration.PanelSize.Height);
            ShowControls();
        }


        public void ShowControls()
        {
            FunctionControl FunctionControl = new FunctionControl(part);
            JogControl jogControl = new JogControl(part);

            jogControl.Location = new Point(Configuration.ContentLocation.X, 0);
            FunctionControl.Location = new Point(this.panelContent.Width - FunctionControl.Size.Width - Configuration.ContentLocation.X * 2, 0);

            this.panelContent.Controls.Add(FunctionControl);
            this.panelContent.Controls.Add(jogControl);
            //this.panelContent.Controls.Add(JogButtonCombination);
        }

        private void panelContent_MouseDown(object sender, MouseEventArgs e)
        {
            //this.FormMaintPartHide();
        }
    }
}
