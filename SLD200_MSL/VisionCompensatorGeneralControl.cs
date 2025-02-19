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

namespace SLD200_MSL
{
    #region VisionCompensatorGeneralControl
    public partial class VisionCompensatorGeneralControl : UserControl
    {
        #region Property
        //public VisionCompensatorConfig Config { get; set; }
        public VisionCompensator Owner { get; set; }
        #endregion

        #region Constructor
        public VisionCompensatorGeneralControl()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();

            if (Owner != null)
                Init();
        }
        #endregion

        #region Method
        public void Init()
        {
            this.baseTextBoxAcc.Text = Owner.Config.Parameter.Acc.ToString();
            this.baseTextBoxDcc.Text = Owner.Config.Parameter.Dcc.ToString();
            this.baseTextBoxVelocity.Text = Owner.Config.Parameter.Velocity.ToString();
            this.baseTextBoxMeasurePitchX.Text = Owner.Config.Parameter.PitchDistanceX.ToString();
            this.baseTextBoxMeasurePitchY.Text = Owner.Config.Parameter.PitchDistanceY.ToString();
            this.baseTextBoxVerificationPitchX.Text = Owner.Config.Parameter.VerficationPitchDistanceX.ToString();
            this.baseTextBoxVerificationPitchY.Text = Owner.Config.Parameter.VerficationPitchDistanceY.ToString();

            this.baseToggleButtonInvertedX.UpdateToggleStatus(Owner.Config.Parameter.InvertedX);
            this.baseToggleButtonInvertedY.UpdateToggleStatus(Owner.Config.Parameter.InvertedY);

            this.comboBoxOperate.Items.Clear();
            foreach (VisionCompensator.OperatorKeys key in Enum.GetValues(typeof(VisionCompensator.OperatorKeys)))
            {
                this.comboBoxOperate.Items.Add(key);
            }
            this.comboBoxOperate.SelectedItem = Owner.Config.Parameter.Operator;

            this.basePropertyGridParameter.SelectedObject = null;
            this.basePropertyGridParameter.SelectedObject = Owner.Config.Parameter;
        }
        #endregion

        #region Event Handler
        private void baseTextBoxMeasurePitchX_TextChanged(object sender, EventArgs e)
        {
            double dValue = 0.0;
            double.TryParse(this.baseTextBoxMeasurePitchX.Text, out dValue);
            Owner.Config.Parameter.PitchDistanceX = dValue;
        }

        private void baseTextBoxMeasurePitchY_TextChanged(object sender, EventArgs e)
        {
            double dValue = 0.0;
            double.TryParse(this.baseTextBoxMeasurePitchY.Text, out dValue);
            Owner.Config.Parameter.PitchDistanceY = dValue;
        }

        private void baseTextBoxVerificationPitchX_TextChanged(object sender, EventArgs e)
        {
            double dValue = 0.0;
            double.TryParse(this.baseTextBoxVerificationPitchX.Text, out dValue);
            Owner.Config.Parameter.VerficationPitchDistanceX = dValue;
        }

        private void baseTextBoxVerificationPitchY_TextChanged(object sender, EventArgs e)
        {
            double dValue = 0.0;
            double.TryParse(this.baseTextBoxVerificationPitchY.Text, out dValue);
            Owner.Config.Parameter.VerficationPitchDistanceY = dValue;
        }

        private void baseTextBoxVelocity_TextChanged(object sender, EventArgs e)
        {
            double dValue = 0.0;
            double.TryParse(this.baseTextBoxVelocity.Text, out dValue);
            Owner.Config.Parameter.Velocity = dValue;
        }

        private void baseTextBoxAcc_TextChanged(object sender, EventArgs e)
        {
            double dValue = 0.0;
            double.TryParse(this.baseTextBoxAcc.Text, out dValue);
            Owner.Config.Parameter.Acc = dValue;
        }

        private void baseTextBoxDcc_TextChanged(object sender, EventArgs e)
        {
            double dValue = 0.0;
            double.TryParse(this.baseTextBoxDcc.Text, out dValue);
            Owner.Config.Parameter.Dcc = dValue;
        }

        private void baseToggleButtonInvertedX_Click(object sender, EventArgs e)
        {
            bool bOn = baseToggleButtonInvertedX.GetButtonStatus();
            if (bOn == true)
            {
                baseToggleButtonInvertedX.UpdateToggleStatus(false);
            }
            else
            {
                baseToggleButtonInvertedX.UpdateToggleStatus(true);
            }
            Owner.Config.Parameter.InvertedX = this.baseToggleButtonInvertedX.GetButtonStatus();

        }

        private void baseToggleButtonInvertedY_Click(object sender, EventArgs e)
        {
            bool bOn = baseToggleButtonInvertedY.GetButtonStatus();
            if (bOn == true)
            {
                baseToggleButtonInvertedY.UpdateToggleStatus(false);
            }
            else
            {
                baseToggleButtonInvertedY.UpdateToggleStatus(true);
            }
            Owner.Config.Parameter.InvertedY = this.baseToggleButtonInvertedY.GetButtonStatus();
        }

        private void comboBoxOperate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxOperate.SelectedItem.ToString() == VisionCompensator.OperatorKeys.All.ToString())
            {
                Owner.Config.Parameter.Operator = VisionCompensator.OperatorKeys.All;
            }
            else if (comboBoxOperate.SelectedItem.ToString() == VisionCompensator.OperatorKeys.Measurement.ToString())
            {
                Owner.Config.Parameter.Operator = VisionCompensator.OperatorKeys.Measurement;
            }
            else if (comboBoxOperate.SelectedItem.ToString() == VisionCompensator.OperatorKeys.Verification.ToString())
            {
                Owner.Config.Parameter.Operator = VisionCompensator.OperatorKeys.Verification;
            }
        }

        private void baseButtonRun_Click(object sender, EventArgs e)
        {
            Task<int> task = Task.Factory.StartNew(() =>
            {
                int ret = 0;

                Owner.Work();

                return ret;
            });

            ProgressForm progressForm = new ProgressForm("Vision Compensator", "Compensating...", task);
            progressForm.StopProcess += ProgressForm_StopProcess;
            progressForm.ShowDialog();

            this.Owner.Camera.StartLive();
        }

        private void ProgressForm_StopProcess(object target)
        {
            Owner.Stop();
        }
        #endregion

        private void baseGroupBoxParameter_Enter(object sender, EventArgs e)
        {

        }
    }
    #endregion
}
