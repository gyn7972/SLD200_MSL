using QMC.Common;
using QMC.Common.Modules;
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
    #region Difine
    [Serializable]
    public enum ButtonSelectIOType
    {
        Common,
    }

    #endregion
    public partial class FormSelectIO : FormContentBase
    {
        private ModuleCollection m_Modules;
        private Module m_Module;
        private DioPointCollection m_DioPoints;
        private IOListControl Inputlist;
        private IOListControl Outputlist;
        private DioPointCollection InputPoint;
        private DioPointCollection OutputPoint;
        public FormSelectIO()
            : base()
        {
            InitializeComponent();
            this.Size = new Size(Configuration.MainSize.Width, Configuration.ContentSize.Height - Configuration.PanelSize.Height);
            this.BackColor = Color.FromArgb(38, 38, 38);
            
            m_Modules = Equipment.Modules;
            InputPoint = new DioPointCollection();
            OutputPoint = new DioPointCollection();
            Inputlist = new IOListControl(InputPoint, "Input List");
            Outputlist = new IOListControl(OutputPoint, "Output List");
            m_DioPoints = new DioPointCollection();
            //SetAllDioPoint();
            SetDioPoint();              //  2022. 04. 12.  SCH : 전체 한번만 추가하기 위해서
            this.Inputlist.SetControlName("Input List");
            this.Outputlist.SetControlName("Output List");
            this.panelContent.Visible = false;
            this.panelContent.Size = new System.Drawing.Size();
            this.Controls.Add(this.baseLabel);
            this.baseLabel.Location = new Point((Configuration.ContentSize.Width / 2) - (Configuration.ButtonSize.Width / 2), this.flowLayoutPanelButton.Location.Y + this.flowLayoutPanelButton.Height + Configuration.ControlGap);
            this.flowLayoutPanelButton.Location = new Point(0, 0);

            this.Inputlist.Size = new Size(Configuration.IOGridSize.Width, Configuration.IOGridSize.Height);
            this.Inputlist.Location = new Point(Configuration.ControlGap, this.baseLabel.Location.Y + this.baseLabel.Height + Configuration.ControlGap);
            this.Controls.Add(this.Inputlist);

            this.Outputlist.Size = new Size(Configuration.IOGridSize.Width, Configuration.IOGridSize.Height);
            this.Outputlist.Location = new Point(this.Inputlist.Location.X + this.Inputlist.Width + Configuration.ControlGap, this.Inputlist.Location.Y);
            this.Controls.Add(this.Outputlist);

            this.VisibleChanged += FormModuleIO_VisibleChanged;
            CreateModuleList();
        }
        private void FormModuleIO_VisibleChanged(object sender, EventArgs e)
        {
            Inputlist.ShowDataGridView();
            Outputlist.ShowDataGridView();
        }
        private void CreateModuleList()
        {
            this.flowLayoutPanelButton.Controls.Clear();
            BaseButton[] control = new BaseButton[m_Modules.Count + 1];
            int locationX = Configuration.ButtonSize.Width;
            int locationY = Configuration.ButtonSize.Height;
            int i = 1;
            control[0] = new BaseButton();
            control[0].Parent = this;
            control[0].Size = new Size(Configuration.ButtonSize.Width, Configuration.ButtonSize.Height);
            control[0].Name = "All";
            control[0].Text = "All";
            control[0].Click += Button_Click;
            control[0].Tag = null;
            this.flowLayoutPanelButton.Controls.Add(control[0]);
            locationX += Configuration.ButtonSize.Width;
            foreach (Module module in Equipment.Modules)
            {
                if (module.Name == "Common")
                {

                }
                else
                {
                    control[i] = new BaseButton();
                    control[i].Parent = this;
                    control[i].Size = new Size(Configuration.ButtonSize.Width, Configuration.ButtonSize.Height);
                    control[i].Name = module.Name;
                    control[i].Text = module.Name;
                    control[i].Click += Button_Click;
                    control[i].Tag = module;

                    this.flowLayoutPanelButton.Controls.Add(control[i]);
                    locationX += Configuration.ButtonSize.Width;
                    i++;
                }
            }
        }
        private void Button_Click(object sender, EventArgs e)
        {
            BaseButton button = sender as BaseButton;
            switch (button.Name)
            {
                case "All":
                    //SetAllDioPoint();
                    SetDioPoint();              //  2022. 04. 12.  SCH : 전체 한번만 추가하기 위해서
                    break;
                case "DieUnloader":
                    SetModuleDioPoint(button.Name);
                    break;
                case "DieTransfer":
                    SetModuleDioPoint(button.Name);
                    break;
                case "DieLoader":
                    SetModuleDioPoint(button.Name);
                    break;
            }
        }

        private void SetDioPoint()
        {
            m_DioPoints.Clear();
            //foreach (Module module in m_Modules)
            {
                //if (module != null)
                {
                    foreach (DioPoint point in Equipment.GetAllDioPointList())
                    {
                        DioPoint dioPoint = point;
                        m_DioPoints.Add(dioPoint);
                    }
                }
            }
            SetDioPointInfo(m_DioPoints);
            this.baseLabel.Text = "All";
        }

        private void SetAllDioPoint()
        {
            m_DioPoints.Clear();
            foreach (Module module in m_Modules)
            {
                if (module != null)
                {
                    foreach (DioPoint point in Equipment.GetAllDioPointList())
                    {
                        DioPoint dioPoint = point;
                        m_DioPoints.Add(dioPoint);
                    }
                }
            }
            SetDioPointInfo(m_DioPoints);
            this.baseLabel.Text = "All";
        }

        private void SetModuleDioPoint(string strModule)
        {
            m_DioPoints.Clear();
            foreach (Module module in m_Modules)
            {
                if (module.Name == strModule)
                {
                    m_Module = module;
                    this.baseLabel.Text = module.Name;
                    break;
                }
            }
            m_DioPoints = m_Module.GetAllDioPoints();
            SetDioPointInfo(m_DioPoints);
        }

        private void SetDioPointInfo(DioPointCollection IOList)
        {
            InputPoint.Clear();
            OutputPoint.Clear();
            if (IOList != null)
            {
                foreach (DioPoint one in IOList)
                {
                    if (one.IoType == IoType.Input)
                    {
                        InputPoint.Add(one);
                    }
                    else
                    {
                        OutputPoint.Add(one);
                    }
                }
                Inputlist.UpdateGridInfo(InputPoint);
                Outputlist.UpdateGridInfo(OutputPoint);
            }
            else
            {
                Inputlist.UpdateGridInfo(InputPoint);
                Outputlist.UpdateGridInfo(OutputPoint);
            }
        }
    }
}