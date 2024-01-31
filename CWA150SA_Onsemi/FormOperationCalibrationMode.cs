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
    public partial class FormOperationCalibrationMode : FormSubContentBase
    {
        CalibrationMode_CWA150SA calibrationMode_CWA150SA;

        public FormOperationCalibrationMode()
            :base(FormType.withButton.ToString(),"Calibration Mode")
        {
            InitializeComponent();

            //this.panelContent.Visible = false;
            this.panelContent.Size = new Size();

            this.panelContent.Location = new System.Drawing.Point(0, Configuration.PanelSize.Height);
            this.panelContent.Size = new System.Drawing.Size(Configuration.PanelbuttonSize.Width + 10, Configuration.FormContentSize.Height);

            calibrationMode_CWA150SA = new CalibrationMode_CWA150SA();

            this.flowLayoutPanelButton.Location = new System.Drawing.Point(Configuration.ButtonSize.Width + 6, 0);
            this.flowLayoutPanelButton.Size = new System.Drawing.Size(Configuration.PanelSize.Width - Configuration.ButtonSize.Width, Configuration.PanelSize.Height);

            this.panelContent.Controls.Add(baseLabelTitle);

            //  여기에 Monitoring 화면에 보여줄 것을 등록해야지...
            calibrationMode_CWA150SA.Location = new Point(5, 30);
            this.panelContent.Controls.Add(calibrationMode_CWA150SA);
        }
    }
}
