using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QMC.Common;

namespace SLD200_MSL
{
    public delegate void EventHandlerMaintNoAxisPartForm();
    public partial class FormMaintNoAxisPart : FormSubContentBase
    {
        Part m_Part;
        public event EventHandlerMaintNoAxisPartForm FormMaintNoAxisPartHide;
        public FormMaintNoAxisPart(Part parts)
           : base(FormType.Content.ToString(), parts.Name)
        {
            m_Part = parts;
            InitializeComponent();

            this.flowLayoutPanelButton.Enabled = false;
            this.panelContent.Location = new System.Drawing.Point(0, Configuration.PanelSize.Height);
            ShowControls();
        }

        public void ShowControls()
        {
            FunctionControl FunctionControl = new FunctionControl(m_Part);
            FunctionControl.Location = new Point(Configuration.ContentLocation.X, 0);
            this.panelContent.Controls.Add(FunctionControl);
        }

        private void panelContent_MouseDown(object sender, MouseEventArgs e)
        {
            if (FormMaintNoAxisPartHide != null)
            {
                FormMaintNoAxisPartHide();
            }
        }
    }
}
