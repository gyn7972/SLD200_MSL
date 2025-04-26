using netDxf.Entities;
using QMC.Common;
using QMC.Common.Modules;
using QMC.Common.Motion.ACS.Motions;
using QMC.Common.Parts;
using QMC.Common.Vision.Optics;
using QMC.Common.Vision.Tools;
using QMC.Common.VisionPart;
using QMC.Core;
using SLD200_MSL;
using SpiralLab.Sirius;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QMC.Common.Equipment;
using static QMC.Common.Modules.Vision;
using static QMC.Common.Modules.WorkStage;
using Bitmap = System.Drawing.Bitmap;
using Image = System.Drawing.Image;
using Rectangle = System.Drawing.Rectangle;

namespace SLD200_MSL
{
    public partial class FormNew_AlarmLog : Form
    {        
        public AlarmCollection Alarms { get; set; }
        Alarm Alarm { get; set; }

        private Size ConfirmButton = new Size(220, 130);
        protected Size m_imagesize = new Size(30, 25);

        protected BaseButton m_BaseButton;
        //public string m_path = System.IO.Directory.GetParent(System.Environment.CurrentDirectory).Parent.FullName;        //  요기 자꾸 뻑남

        public FormNew_AlarmLog()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;


        }
    }
}
