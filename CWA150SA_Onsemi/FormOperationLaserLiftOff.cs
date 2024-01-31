using QMC.Common;
using QMC.Common.Hmi;
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
    public partial class FormOperationWaferProbeAlign : FormSubContentBase
    {
        #region Field
        Module m_Module;
        GroupBox GroupBoxDispenserJog = new GroupBox();
        #endregion

        public FormOperationWaferProbeAlign(Module module)
            : base(FormType.withButton.ToString(), OperationModuleName.WaferProbeAlign.ToString())
        {
            m_Module = module;
            InitializeComponent();

            Monitoring_CWA150SA monitoring_CWA150SA = new Monitoring_CWA150SA();

            this.panelContent.Location = new System.Drawing.Point(0, 0);    //  20);   //  8);
            this.panelContent.Size = new System.Drawing.Size(Configuration.ContentSize.Width, Configuration.FormContentSize.Height);

            //  여기에 Monitoring 화면에 보여줄 것을 등록해야지...
            monitoring_CWA150SA.Location = new Point(5, 30);
            this.panelContent.Controls.Add(monitoring_CWA150SA);
        }

        private void AddGroupBox()
        {
            GroupBoxDispenserJog.Font = new System.Drawing.Font("Arial", 9F);
            GroupBoxDispenserJog.ForeColor = System.Drawing.Color.White;
            GroupBoxDispenserJog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            GroupBoxDispenserJog.Location = new System.Drawing.Point(4, Configuration.ConveyorControlGap);
            GroupBoxDispenserJog.Name = "groupBoxDispenserJog";
            //GroupBoxDispenserJog.Size = new System.Drawing.Size(250, 123);
            GroupBoxDispenserJog.TabIndex = 0;
            GroupBoxDispenserJog.TabStop = false;
            GroupBoxDispenserJog.Text = " Dispenser XYZ ";
        }

        public void ShowControls()
        {
            int m_nHeight = 0;

            this.panelContent.Controls.Add(baseLabelTitle);

            JogButtonXYZ jogButtonXYZ = new JogButtonXYZ();

            jogButtonXYZ.Location = new Point(4, 20);

            m_nHeight = jogButtonXYZ.Size.Height;

            GroupBoxDispenserJog.Controls.Add(jogButtonXYZ);            
        }

        private void FormOperationWaferProbeAlign_Load(object sender, EventArgs e)
        {
            AddGroupBox();

            ShowControls();
        }
    }
}
