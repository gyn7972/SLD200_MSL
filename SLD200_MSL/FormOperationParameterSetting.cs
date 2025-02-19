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
    public partial class FormOperationParameterSetting : FormSubContentBase
    {
        ParameterSetting_SLD200 parameterSet_SLD200;

        public FormOperationParameterSetting()
            :base(FormType.withButton.ToString(),"Parameter Set.")
        {
            InitializeComponent();

            this.BackColor = Configuration.PanelBackColor;

            //this.panelContent.Visible = false;
            this.panelContent.Size = new Size();

            this.panelContent.Location = new System.Drawing.Point(0, Configuration.PanelSize.Height);
            this.panelContent.Size = new System.Drawing.Size(Configuration.PanelbuttonSize.Width + 10, Configuration.FormContentSize.Height);
            this.panelContent.BackColor = Configuration.PanelBackColor;

            parameterSet_SLD200 = new ParameterSetting_SLD200();

            this.flowLayoutPanelButton.Location = new System.Drawing.Point(Configuration.ButtonSize.Width + 6, 0);
            this.flowLayoutPanelButton.Size = new System.Drawing.Size(Configuration.PanelSize.Width - Configuration.ButtonSize.Width, Configuration.PanelSize.Height);
            this.flowLayoutPanelButton.BackColor = Configuration.PanelBackColor;

            baseLabelTitle.BackColor = Configuration.PanelBackColor;
            baseLabelTitle.ForeColor = Color.Black;
            this.panelContent.Controls.Add(baseLabelTitle);

            //  여기에 Monitoring 화면에 보여줄 것을 등록해야지...
            parameterSet_SLD200.Location = new Point(5, 30);
            this.panelContent.Controls.Add(parameterSet_SLD200);
        }
    }
}
