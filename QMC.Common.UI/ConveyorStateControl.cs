using QMC.Common.Parts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QMC.Common.UI
{
    public partial class ConveyorStateControl : UserControl
    {
        #region Field
        protected Conveyor m_Conveyor;
        #endregion

        #region Constructor
        public ConveyorStateControl() : this(null)
        {
        }

        public ConveyorStateControl(Part part)
        {
            InitializeComponent();
            m_Conveyor = part as Conveyor;
            UpdateState();
        }
        #endregion

        #region Method
        public void SetTitle(string strTitle)
        {
            baseGroupBoxMain.Text = strTitle;
        }

        private Image resizeImage(Image image, Size size)
        {
            return (Image)new Bitmap(image, size);
        }

        public void UpdateState()
        {
            if (m_Conveyor != null)
            {
                if (m_Conveyor.IsInputCarrier())
                {
                    pictureBoxCarrierCheck.Image = resizeImage(Properties.Resources.DioEllipseOn, pictureBoxCarrierCheck.Size);
                }
                else
                {
                    pictureBoxCarrierCheck.Image = resizeImage(Properties.Resources.DioEllipseOff, pictureBoxCarrierCheck.Size);
                }

                if (m_Conveyor.IsRun())
                {
                    pictureBoxRun.Image = resizeImage(Properties.Resources.DioEllipseOn, pictureBoxRun.Size);
                }
                else
                {
                    pictureBoxRun.Image = resizeImage(Properties.Resources.DioEllipseOff, pictureBoxRun.Size);
                }

                if (m_Conveyor.Clamper != null && m_Conveyor.Clamper.IsClamp())
                {
                    pictureBoxClamp.Image = resizeImage(Properties.Resources.DioEllipseOn, pictureBoxClamp.Size);
                    pictureBoxUnclamp.Image = resizeImage(Properties.Resources.DioEllipseOff, pictureBoxUnclamp.Size);
                }
                else if (m_Conveyor.Clamper == null)
                {
                    pictureBoxClamp.Image = resizeImage(Properties.Resources.DioEllipseOff, pictureBoxClamp.Size);
                    pictureBoxUnclamp.Image = resizeImage(Properties.Resources.DioEllipseOff, pictureBoxUnclamp.Size);
                }
                else
                {
                    pictureBoxClamp.Image = resizeImage(Properties.Resources.DioEllipseOff, pictureBoxClamp.Size);
                    pictureBoxUnclamp.Image = resizeImage(Properties.Resources.DioEllipseOn, pictureBoxUnclamp.Size);
                }

                if (m_Conveyor.Stopper != null && m_Conveyor.Stopper.IsUp())
                {
                    pictureBoxStopperUp.Image = resizeImage(Properties.Resources.DioEllipseOn, pictureBoxStopperUp.Size);
                    pictureBoxStopperDown.Image = resizeImage(Properties.Resources.DioEllipseOff, pictureBoxStopperDown.Size);
                }
                else if (m_Conveyor.Stopper == null)
                {
                    pictureBoxStopperUp.Image = resizeImage(Properties.Resources.DioEllipseOff, pictureBoxStopperUp.Size);
                    pictureBoxStopperDown.Image = resizeImage(Properties.Resources.DioEllipseOff, pictureBoxStopperDown.Size);
                }
                else
                {
                    pictureBoxStopperUp.Image = resizeImage(Properties.Resources.DioEllipseOff, pictureBoxStopperUp.Size);
                    pictureBoxStopperDown.Image = resizeImage(Properties.Resources.DioEllipseOn, pictureBoxStopperDown.Size);
                }

            }

        }

        public void SetClamperControlHiding(bool hide)
        {
            if (hide)
            {
                this.baseGroupBoxClamper.Enabled = false;
                this.baseGroupBoxClamper.Hide();
                this.baseGroupBoxMain.Size = new Size(this.baseGroupBoxMain.Width - this.baseGroupBoxClamper.Width, this.baseGroupBoxMain.Height);
                this.Size = new Size(this.Width - this.baseGroupBoxClamper.Width, this.baseGroupBoxMain.Height + 10);
            }
            else
            {
                this.baseGroupBoxClamper.Enabled = true;
                this.baseGroupBoxClamper.Show();
            }
        }

        public void SetStopperControlHiding(bool hide)
        {
            if (hide)
            {
                this.baseGroupBoxStopper.Enabled = false;
                this.baseGroupBoxStopper.Hide();
                this.baseGroupBoxMain.Size = new Size(this.baseGroupBoxMain.Width - this.baseGroupBoxStopper.Width, this.baseGroupBoxMain.Height);
                this.Size = new Size(this.Width - this.baseGroupBoxStopper.Width, this.baseGroupBoxMain.Height + 10);
            }
            else
            {
                this.baseGroupBoxStopper.Enabled = true;
                this.baseGroupBoxStopper.Show();
            }
        }

        public void SetEnable(bool enable)
        {
            this.baseButtonConveyorRun.Enabled = enable;
            this.baseButtonClamp.Enabled = enable;
            this.baseButtonStopper.Enabled = enable;
        }
        #endregion

        #region Event Handler
        private void baseButtonConveyorRun_Click(object sender, EventArgs e)
        {
            if (m_Conveyor.IsRun())
            {
                m_Conveyor.Stop();
            }
            else
            {
                m_Conveyor.Run();
            }
        }

        private void baseButtonClamp_Click(object sender, EventArgs e)
        {
            if (m_Conveyor.Clamper.IsClamp())
            {
                m_Conveyor.Clamper.Unclamp();
            }
            else
            {
                m_Conveyor.Clamper.Clamp();
            }
        }

        private void baseButtonStopper_Click(object sender, EventArgs e)
        {
            if (m_Conveyor.Stopper.IsUp())
            {
                m_Conveyor.Stopper.Down();
            }
            else
            {
                m_Conveyor.Stopper.Up();
            }
        }
        #endregion
    }
}
