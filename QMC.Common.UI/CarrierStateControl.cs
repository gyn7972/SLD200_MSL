using QMC.Common;
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
    public delegate void CarrierStateControlButtonClickHandler(CarrierStateControl.ButtonType type);
    public partial class CarrierStateControl : UserControl
    {
        public enum ButtonType
        {
            DataClear,
        }
        public delegate void UnitButtonClick(int nIndex);
        public event UnitButtonClick UnitClick;

        protected Size m_CarrierBaseImagesize = new Size(313, 156);
        protected Size m_CarrierSmtImagesize = new Size(27, 38);
        protected Carrier m_Carrier;
        protected List<PictureBox> m_listCarrierWorkStatusControl;
        protected List<BaseToggleButton> m_listToggleButtonControl;
        protected List<PictureBox> m_ListCarrierUnit;
        protected List<PictureBox> m_ListResult;
        protected bool m_bDisableUseWork;
        public event CarrierStateControlButtonClickHandler ButtonClick;
        public CarrierStateControl()
        {
            InitializeComponent();
            InitControl();
            m_Carrier = null;
            this.ResizeRedraw = true;
            //m_bVisible = true;

            ButtonDisable(false);
        }
        public void InitControl()
        {
            Image image = Properties.Resources.Carrier;
            Bitmap imgbitmap = new Bitmap(image);
            image = resizeImage(imgbitmap, m_CarrierBaseImagesize);
            pictureBoxCarrierBase.Image = image;

            m_listCarrierWorkStatusControl = new List<PictureBox>();
            m_listCarrierWorkStatusControl.Clear();
            m_listCarrierWorkStatusControl.Add(pictureBoxCarrierWorkStatus_1);
            m_listCarrierWorkStatusControl.Add(pictureBoxCarrierWorkStatus_2);
            m_listCarrierWorkStatusControl.Add(pictureBoxCarrierWorkStatus_3);
            m_listCarrierWorkStatusControl.Add(pictureBoxCarrierWorkStatus_4);
            m_listCarrierWorkStatusControl.Add(pictureBoxCarrierWorkStatus_5);
            m_listCarrierWorkStatusControl.Add(pictureBoxCarrierWorkStatus_6);
            m_listCarrierWorkStatusControl.Add(pictureBoxCarrierWorkStatus_7);
            m_listCarrierWorkStatusControl.Add(pictureBoxCarrierWorkStatus_8);
            m_listCarrierWorkStatusControl.Add(pictureBoxCarrierWorkStatus_9);
            m_listCarrierWorkStatusControl.Add(pictureBoxCarrierWorkStatus_10);

            m_listToggleButtonControl = new List<BaseToggleButton>();
            m_listToggleButtonControl.Clear();
            m_listToggleButtonControl.Add(baseToggleButtonCarrerSmtUnit_1);
            m_listToggleButtonControl.Add(baseToggleButtonCarrerSmtUnit_2);
            m_listToggleButtonControl.Add(baseToggleButtonCarrerSmtUnit_3);
            m_listToggleButtonControl.Add(baseToggleButtonCarrerSmtUnit_4);
            m_listToggleButtonControl.Add(baseToggleButtonCarrerSmtUnit_5);
            m_listToggleButtonControl.Add(baseToggleButtonCarrerSmtUnit_6);
            m_listToggleButtonControl.Add(baseToggleButtonCarrerSmtUnit_7);
            m_listToggleButtonControl.Add(baseToggleButtonCarrerSmtUnit_8);
            m_listToggleButtonControl.Add(baseToggleButtonCarrerSmtUnit_9);
            m_listToggleButtonControl.Add(baseToggleButtonCarrerSmtUnit_10);

            m_ListCarrierUnit = new List<PictureBox>();
            m_ListCarrierUnit.Clear();
            m_ListCarrierUnit.Add(pictureBox1);
            m_ListCarrierUnit.Add(pictureBox2);
            m_ListCarrierUnit.Add(pictureBox3);
            m_ListCarrierUnit.Add(pictureBox4);
            m_ListCarrierUnit.Add(pictureBox5);
            m_ListCarrierUnit.Add(pictureBox6);
            m_ListCarrierUnit.Add(pictureBox7);
            m_ListCarrierUnit.Add(pictureBox8);
            m_ListCarrierUnit.Add(pictureBox9);
            m_ListCarrierUnit.Add(pictureBox10);

            m_ListResult = new List<PictureBox>();
            m_ListResult.Add(pictureBoxResult_1);
            m_ListResult.Add(pictureBoxResult_2);
            m_ListResult.Add(pictureBoxResult_3);
            m_ListResult.Add(pictureBoxResult_4);
            m_ListResult.Add(pictureBoxResult_5);
            m_ListResult.Add(pictureBoxResult_6);
            m_ListResult.Add(pictureBoxResult_7);
            m_ListResult.Add(pictureBoxResult_8);
            m_ListResult.Add(pictureBoxResult_9);
            m_ListResult.Add(pictureBoxResult_10);

            for (int i = 0; i < 10; i++)
            {
                m_ListCarrierUnit[i].Visible = false;
            }
        }
        public void SetCarrierData(Carrier carrier)
        {
            m_Carrier = carrier;
        }
        public void UpdateState()
        {
            UpdateCarrierState();
            UpdateWorkState();
            UpdateUseWork();
            UpdateResult();
        }
        public Image resizeImage(Image image, Size size)
        {
            return (Image)new Bitmap(image, size);
        }
        public void SetGroupBoxName(string strName)
        {
            baseGroupBoxMain.Text = strName;
        }
        public void SetImageTest()
        {
            Image image = Properties.Resources.smt;
            Bitmap imgbitmap = new Bitmap(image);
            image = resizeImage(imgbitmap, m_CarrierSmtImagesize);
            baseToggleButtonCarrerSmtUnit_1.Image = image;
            pictureBox1.Image = image;

            image = Properties.Resources.smt;
            imgbitmap = new Bitmap(image);
            image = resizeImage(imgbitmap, m_CarrierSmtImagesize);
            baseToggleButtonCarrerSmtUnit_1.Image = image;
        }

        public void ButtonDisable(bool bDisable)
        {
            //baseToggleButtonCarrerSmtUnit_1.Visible = false;
            //baseToggleButtonCarrerSmtUnit_2.Visible = false;
            //baseToggleButtonCarrerSmtUnit_3.Visible = false;
            //baseToggleButtonCarrerSmtUnit_4.Visible = false;
            //baseToggleButtonCarrerSmtUnit_5.Visible = false;
            //baseToggleButtonCarrerSmtUnit_6.Visible = false;
            //baseToggleButtonCarrerSmtUnit_7.Visible = false;
            //baseToggleButtonCarrerSmtUnit_8.Visible = false;
            //baseToggleButtonCarrerSmtUnit_9.Visible = false;
            //baseToggleButtonCarrerSmtUnit_10.Visible = false;
            m_bDisableUseWork = bDisable;
            for (int i = 0; i < 10; i++)
            {
                m_listToggleButtonControl[i].Visible = !bDisable;
                m_ListCarrierUnit[i].Visible = bDisable;

            }
        }

        private void baseToggleButtonCarrier_1_Click(object sender, EventArgs e)
        {
        }

        private void baseToggleButtonCarrier_2_Click(object sender, EventArgs e)
        {
        }

        private void baseToggleButtonCarrier_3_Click(object sender, EventArgs e)
        {

        }

        private void baseToggleButtonCarrier_4_Click(object sender, EventArgs e)
        {

        }

        private void baseToggleButtonCarrier_5_Click(object sender, EventArgs e)
        {

        }

        private void baseToggleButtonCarrier_6_Click(object sender, EventArgs e)
        {

        }

        private void baseToggleButtonCarrier_7_Click(object sender, EventArgs e)
        {

        }

        private void baseToggleButtonCarrier_8_Click(object sender, EventArgs e)
        {

        }

        private void baseToggleButtonCarrier_9_Click(object sender, EventArgs e)
        {

        }

        private void baseToggleButtonCarrier_10_Click(object sender, EventArgs e)
        {
        }


        public void SetUnitNumber(bool enable)
        {
            if (enable)
            {
                this.baseToggleButtonCarrerSmtUnit_1.Text = "1";
                this.baseToggleButtonCarrerSmtUnit_2.Text = "2";
                this.baseToggleButtonCarrerSmtUnit_3.Text = "3";
                this.baseToggleButtonCarrerSmtUnit_4.Text = "4";
                this.baseToggleButtonCarrerSmtUnit_5.Text = "5";
                this.baseToggleButtonCarrerSmtUnit_6.Text = "6";
                this.baseToggleButtonCarrerSmtUnit_7.Text = "7";
                this.baseToggleButtonCarrerSmtUnit_8.Text = "8";
                this.baseToggleButtonCarrerSmtUnit_9.Text = "9";
                this.baseToggleButtonCarrerSmtUnit_10.Text = "10";

            }
            else
            {
                this.baseToggleButtonCarrerSmtUnit_1.Text = string.Empty;
                this.baseToggleButtonCarrerSmtUnit_2.Text = string.Empty;
                this.baseToggleButtonCarrerSmtUnit_3.Text = string.Empty;
                this.baseToggleButtonCarrerSmtUnit_4.Text = string.Empty;
                this.baseToggleButtonCarrerSmtUnit_5.Text = string.Empty;
                this.baseToggleButtonCarrerSmtUnit_6.Text = string.Empty;
                this.baseToggleButtonCarrerSmtUnit_7.Text = string.Empty;
                this.baseToggleButtonCarrerSmtUnit_8.Text = string.Empty;
                this.baseToggleButtonCarrerSmtUnit_9.Text = string.Empty;
                this.baseToggleButtonCarrerSmtUnit_10.Text = string.Empty;
            }
        }

        public void SetWorkState(int nIndex, SmtUnit.UnitWorkStateKey state)
        {

        }

        protected void UpdateWorkState(int nIndex)
        {
            if (m_Carrier != null && m_Carrier.ArrSmtUnit[nIndex] != null)
            {
                if (nIndex >= 0 && nIndex < m_Carrier.ArrSmtUnit.Length)
                {
                    if (m_Carrier.ArrSmtUnit[nIndex].WorkStatus == SmtUnit.UnitWorkStateKey.None)
                    {
                        m_listCarrierWorkStatusControl[nIndex].BackColor = Color.LightGray;
                    }
                    else if (m_Carrier.ArrSmtUnit[nIndex].WorkStatus == SmtUnit.UnitWorkStateKey.Work)
                    {
                        m_listCarrierWorkStatusControl[nIndex].BackColor = Color.Orange;
                    }
                    else if (m_Carrier.ArrSmtUnit[nIndex].WorkStatus == SmtUnit.UnitWorkStateKey.Complete)
                    {
                        m_listCarrierWorkStatusControl[nIndex].BackColor = Color.GreenYellow;
                    }
                }
            }
        }
        public void UpdateWorkState()
        {
            if (m_Carrier != null && m_Carrier.ArrSmtUnit != null)
            {
                for (int i = 0; i < m_Carrier.ArrSmtUnit.Length; i++)
                {
                    UpdateWorkState(i);
                }
            }

        }

        public void UpdateUseWork(int nIndex)
        {
            Image image = null;
            if (m_Carrier != null && m_Carrier.ArrSmtUnit[nIndex] != null)
            {
                if (m_Carrier.ArrSmtUnit[nIndex].UseWork == false)
                {
                    image = Properties.Resources.empty_Carrier;
                }
                else
                {
                    image = Properties.Resources.smt;
                }
                m_ListCarrierUnit[nIndex].Image = image;
                m_listToggleButtonControl[nIndex].Image = image;
            }

        }
        public void UpdateUseWork()
        {
            if (m_Carrier != null)
            {
                for (int i = 0; i < m_Carrier.UnitCount; i++)
                {
                    if (m_Carrier.ArrSmtUnit != null && m_Carrier.ArrSmtUnit[i] != null)
                    {

                        UpdateUseWork(i);
                    }
                    else
                    {
                        Image image = Properties.Resources.empty_Carrier;
                        Bitmap imgbitmap = new Bitmap(image);
                        image = resizeImage(imgbitmap, m_CarrierSmtImagesize);
                        m_listToggleButtonControl[i].Image = image;
                        m_ListCarrierUnit[i].Image = image;
                    }
                }
            }

        }
        public void UpdateResult()
        {
            if (m_Carrier != null && m_Carrier.ArrSmtUnit != null)
            {
                for (int i = 0; i < m_Carrier.UnitCount; i++)
                {
                    if (m_Carrier.ArrSmtUnit[i] != null)
                    {
                        if (m_Carrier.ArrSmtUnit[i].Result == SmtUnit.ResultKey.Ok)
                        {
                            m_ListResult[i].BackColor = Color.Green;
                        }
                        else if (m_Carrier.ArrSmtUnit[i].Result == SmtUnit.ResultKey.Ng)
                        {
                            m_ListResult[i].BackColor = Color.Red;
                        }
                        else
                        {
                            m_ListResult[i].BackColor = Color.DarkGray;
                        }
                    }
                    else
                    {
                        m_ListResult[i].BackColor = Color.DarkGray;
                    }
                }
            }
        }

        private void baseToggleButtonCarrerSmtUnit_1_Click(object sender, EventArgs e)
        {
            if (m_Carrier != null && m_Carrier.ArrSmtUnit[0] != null)
            {
                if (m_Carrier.ArrSmtUnit[0].UseWork)
                {
                    m_Carrier.ArrSmtUnit[0].UseWork = false;
                }
                else
                {
                    m_Carrier.ArrSmtUnit[0].UseWork = true;
                }
            }

            if (UnitClick != null)
                UnitClick(0);
        }

        private void baseToggleButtonCarrerSmtUnit_2_Click(object sender, EventArgs e)
        {
            if (m_Carrier != null && m_Carrier.ArrSmtUnit[1] != null)
            {
                if (m_Carrier.ArrSmtUnit[1].UseWork)
                {
                    m_Carrier.ArrSmtUnit[1].UseWork = false;
                }
                else
                {
                    m_Carrier.ArrSmtUnit[1].UseWork = true;
                }
            }

            if (UnitClick != null)
                UnitClick(1);
        }

        private void baseToggleButtonCarrerSmtUnit_3_Click(object sender, EventArgs e)
        {
            if (m_Carrier != null && m_Carrier.ArrSmtUnit[2] != null)
            {
                if (m_Carrier.ArrSmtUnit[2].UseWork)
                {
                    m_Carrier.ArrSmtUnit[2].UseWork = false;
                }
                else
                {
                    m_Carrier.ArrSmtUnit[2].UseWork = true;
                }
            }

            if (UnitClick != null)
                UnitClick(2);
        }

        private void baseToggleButtonCarrerSmtUnit_4_Click(object sender, EventArgs e)
        {
            if (m_Carrier != null && m_Carrier.ArrSmtUnit[3] != null)
            {
                if (m_Carrier.ArrSmtUnit[3].UseWork)
                {
                    m_Carrier.ArrSmtUnit[3].UseWork = false;
                }
                else
                {
                    m_Carrier.ArrSmtUnit[3].UseWork = true;
                }
            }

            if (UnitClick != null)
                UnitClick(3);
        }

        private void baseToggleButtonCarrerSmtUnit_5_Click(object sender, EventArgs e)
        {
            if (m_Carrier != null && m_Carrier.ArrSmtUnit[4] != null)
            {
                if (m_Carrier.ArrSmtUnit[4].UseWork)
                {
                    m_Carrier.ArrSmtUnit[4].UseWork = false;
                }
                else
                {
                    m_Carrier.ArrSmtUnit[4].UseWork = true;
                }
            }

            if (UnitClick != null)
                UnitClick(4);
        }

        private void baseToggleButtonCarrerSmtUnit_6_Click(object sender, EventArgs e)
        {
            if (m_Carrier != null && m_Carrier.ArrSmtUnit[5] != null)
            {
                if (m_Carrier.ArrSmtUnit[5].UseWork)
                {
                    m_Carrier.ArrSmtUnit[5].UseWork = false;
                }
                else
                {
                    m_Carrier.ArrSmtUnit[5].UseWork = true;
                }
            }

            if (UnitClick != null)
                UnitClick(5);
        }

        private void baseToggleButtonCarrerSmtUnit_7_Click(object sender, EventArgs e)
        {
            if (m_Carrier != null && m_Carrier.ArrSmtUnit[6] != null)
            {
                if (m_Carrier.ArrSmtUnit[6].UseWork)
                {
                    m_Carrier.ArrSmtUnit[6].UseWork = false;
                }
                else
                {
                    m_Carrier.ArrSmtUnit[6].UseWork = true;
                }
            }

            if (UnitClick != null)
                UnitClick(6);
        }

        private void baseToggleButtonCarrerSmtUnit_8_Click(object sender, EventArgs e)
        {
            if (m_Carrier != null && m_Carrier.ArrSmtUnit[7] != null)
            {
                if (m_Carrier.ArrSmtUnit[7].UseWork)
                {
                    m_Carrier.ArrSmtUnit[7].UseWork = false;
                }
                else
                {
                    m_Carrier.ArrSmtUnit[7].UseWork = true;
                }
            }

            if (UnitClick != null)
                UnitClick(7);
        }

        private void baseToggleButtonCarrerSmtUnit_9_Click(object sender, EventArgs e)
        {
            if (m_Carrier != null && m_Carrier.ArrSmtUnit[8] != null)
            {
                if (m_Carrier.ArrSmtUnit[8].UseWork)
                {
                    m_Carrier.ArrSmtUnit[8].UseWork = false;
                }
                else
                {
                    m_Carrier.ArrSmtUnit[8].UseWork = true;
                }
            }

            if (UnitClick != null)
                UnitClick(8);
        }

        private void baseToggleButtonCarrerSmtUnit_10_Click(object sender, EventArgs e)
        {
            if (m_Carrier != null && m_Carrier.ArrSmtUnit[9] != null)
            {
                if (m_Carrier.ArrSmtUnit[9].UseWork)
                {
                    m_Carrier.ArrSmtUnit[9].UseWork = false;
                }
                else
                {
                    m_Carrier.ArrSmtUnit[9].UseWork = true;
                }
            }

            if (UnitClick != null)
                UnitClick(9);
        }


        public void SetVisibleCarrier(bool bOn)
        {
            if (bOn)
            {
                //m_bVisible = true;
                Image image = Properties.Resources.Carrier;
                Bitmap imgbitmap = new Bitmap(image);
                image = resizeImage(imgbitmap, m_CarrierBaseImagesize);
                pictureBoxCarrierBase.Image = image;
                pictureBoxCarrierBase.Visible = true;
                for (int i = 0; i < 10; i++)
                {
                    ButtonDisable(m_bDisableUseWork);
                    //m_listToggleButtonControl[i].Visible = true;
                    m_listCarrierWorkStatusControl[i].Visible = true;
                    //m_ListCarrierUnit[i].Visible = true;
                    m_ListResult[i].Visible = true;
                }
            }
            else
            {
                //m_bVisible = false;
                pictureBoxCarrierBase.Image = null;
                pictureBoxCarrierBase.BackColor = Color.DarkGray;
                for (int i = 0; i < 10; i++)
                {
                    m_listToggleButtonControl[i].Visible = false;
                    m_listCarrierWorkStatusControl[i].Visible = false;
                    m_ListCarrierUnit[i].Visible = false;
                    m_ListResult[i].Visible = false;
                }
            }
        }

        private void baseToggleButtonTest_Click(object sender, EventArgs e)
        {

        }

        public void UpdateCarrierState()
        {
            if (m_Carrier == null)
            {
                SetVisibleCarrier(false);
            }
            else
            {
                SetVisibleCarrier(true);
            }
        }

        public void SetDataClearButtonState(bool bOn)
        {
            baseButtonDataClear.Enabled = bOn;
        }

        private void baseButtonDataClear_Click(object sender, EventArgs e)
        {
            if (ButtonClick != null)
            {
                ButtonClick(ButtonType.DataClear);
            }
        }

      
    }
}
