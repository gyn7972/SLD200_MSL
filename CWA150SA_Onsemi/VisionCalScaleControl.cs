using QMC.Common.VisionPart;
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
    public delegate void VisionCalScaleControlCheckBoxEventHandler(string strName);

    public partial class VisionCalScaleControl : UserControl
    {
        public event VisionCalScaleControlCheckBoxEventHandler VisionCalScaleControlCheckBoxChecked;
        private VisionCalibratorConfig Config { get; set; }

        public VisionCalScaleControl(VisionCalibratorConfig config)
        {
            
            InitializeComponent();
            UpdateGridViewColumns();

            //this.baseLabelXValue.BackColor = Color.FromArgb(78, 78, 78);
            //this.baseLabelYValue.BackColor = Color.FromArgb(78, 78, 78);
            //this.baseLabelXAxisValue.BackColor = Color.FromArgb(78, 78, 78);
            //this.baseLabelYAxisValue.BackColor = Color.FromArgb(78, 78, 78);

            this.baseTextXValue.BackColor = Color.FromArgb(78, 78, 78);
            this.baseTextYValue.BackColor = Color.FromArgb(78, 78, 78);
            this.baseTextXAxisValue.BackColor = Color.FromArgb(78, 78, 78);
            this.baseTextYAxisValue.BackColor = Color.FromArgb(78, 78, 78);

            UpdateUI(config);
        }
        private void Init()
        {
            //baseLabelXValue.Text = Config.X.ToString();
            //baseLabelYValue.Text = Config.Y.ToString();
            //baseLabelXAxisValue.Text = Config.XAxisT.ToString();
            //baseLabelYAxisValue.Text = Config.YAxisT.ToString();

            baseTextXValue.Text = Config.X.ToString();
            baseTextYValue.Text = Config.Y.ToString();
            baseTextXAxisValue.Text = Config.XAxisT.ToString();
            baseTextYAxisValue.Text = Config.YAxisT.ToString();

            checkBoxXInverted.Checked = Config.XInveted;
            checkBoxYInverted.Checked = Config.YInveted;
            checkBoxEnable.Checked = Config.Enable;
            Movedistance.Text = Config.MoveDistance.ToString();
        }
        public void UpdateGridViewColumns()
        {
            this.CalibratorPositionGrid.Columns.Clear();

            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.Name = "Position";
                column.DataPropertyName = "Name";
                column.HeaderText = "Name";
                CalibratorPositionGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.Name = "Target";
                column.DataPropertyName = "Type";
                column.HeaderText = "Type";
                CalibratorPositionGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.Name = "X [mm]";
                column.DataPropertyName = "X";
                column.HeaderText = "X";
                CalibratorPositionGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.Name = "Y [mm]";
                column.DataPropertyName = "Y";
                column.HeaderText = "Y";
                CalibratorPositionGrid.Columns.Add(column);
            }
            {
                //DataGridViewColumn column = new DataGridViewTextBoxColumn();
                //column.Name = "T [deg]";
                //column.DataPropertyName = "T";
                //column.HeaderText = "T";
                //CalibratorPositionGrid.Columns.Add(column);
            }
        }

        private void UpdateGridData()
        {
            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = Config.VisionCalPositions;
            this.CalibratorPositionGrid.DataSource = bindingSource;
        }

        public void UpdateUI(VisionCalibratorConfig config)
        {
            Config = config;
            UpdateGridData();
            Init();
        }

        public void UpdateConfigData()
        {
            Config.X = double.Parse(baseTextXValue.Text);
            Config.Y = double.Parse(baseTextYValue.Text);
            Config.XAxisT = double.Parse(baseTextXAxisValue.Text);
            Config.YAxisT = double.Parse(baseTextYAxisValue.Text);

            Config.XInveted = checkBoxXInverted.Checked;
            Config.YInveted = checkBoxYInverted.Checked;
            Config.Enable = checkBoxEnable.Checked;
            Config.MoveDistance = double.Parse(Movedistance.Text);
        }

        private void checkBoxXInverted_CheckedChanged(object sender, EventArgs e)
        {
            Config.XInveted = checkBoxXInverted.Checked;
        }

        private void checkBoxYInverted_CheckedChanged(object sender, EventArgs e)
        {
            Config.YInveted = checkBoxYInverted.Checked;
        }

        private void checkBoxEnable_CheckedChanged(object sender, EventArgs e)
        {
            Config.Enable = checkBoxEnable.Checked;
        }

        private void baseTextBox1_TextChanged(object sender, EventArgs e)
        {
            double dMovedistance = 0;
            double.TryParse(Movedistance.Text, out dMovedistance);
            Config.MoveDistance = dMovedistance;
        }
    }


}
