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
using QMC.Common.VisionPart;
using QMC.Common.Vision.EureSys;
using QMC.Common.Vision.Optics.Leesos;
using QMC.Common;
using QMC.Common.Modules;
using QMC.Common.Vision;
using QMC.Common.Parts;


namespace CWA150SA_Onsemi300
{
    public partial class FormWaferTipContactPosition : Form
    {
        static WaferProbeAlign waferProbeAlign;

        public FormWaferTipContactPosition()
        {
            InitializeComponent();

            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;

            foreach (Module module in m_collectionModules)
            {
                if (module.Name == "WaferProbeAlign")
                {
                    waferProbeAlign = module as WaferProbeAlign;
                }
            }

            this.Load += FormWaferTipContactPosition_Load;
        }

        private void FormWaferTipContactPosition_Load(object sender, EventArgs e)
        {

        }

        public void Draw_TipContact_Image()
        {
            string m_strRoot = "D:\\CWA-150SA_AlignImage";

            int m_nImage_Width = 0;
            int m_nImage_Height = 0;

            double m_dPAK_GatePin_Gap = 0.0;
            double m_dPAK_GatePin_Diameter = 0.0;
            double m_dWafer_GateContactPosition_OverlaySize = 0.0;

            Image m_TOP_PAK = null;
            Image m_MID_PAK = null;
            Image m_BOT_PAK = null;
            Image m_TOP_Wafer = null;
            Image m_MID_Wafer = null;
            Image m_BOT_Wafer = null;
            Image m_Empty = null;


            try { m_TOP_PAK = Image.FromFile(string.Format("{0}\\TOP_PAK.jpg", m_strRoot)); }
            catch (Exception e) { m_TOP_PAK = null; }

            try { m_MID_PAK = Image.FromFile(string.Format("{0}\\MID_PAK.jpg", m_strRoot)); }
            catch (Exception e) { m_TOP_PAK = null; }

            try { m_BOT_PAK = Image.FromFile(string.Format("{0}\\BOT_PAK.jpg", m_strRoot)); }
            catch (Exception e) { m_TOP_PAK = null; }

            try { m_TOP_Wafer = Image.FromFile(string.Format("{0}\\TOP_Wafer.jpg", m_strRoot)); }
            catch (Exception e) { m_TOP_Wafer = null; }

            try { m_MID_Wafer = Image.FromFile(string.Format("{0}\\MID_Wafer.jpg", m_strRoot)); }
            catch (Exception e) { m_MID_Wafer = null; }

            try { m_BOT_Wafer = Image.FromFile(string.Format("{0}\\BOT_Wafer.jpg", m_strRoot)); }
            catch (Exception e) { m_BOT_Wafer = null; }

            try { m_Empty = Image.FromFile(string.Format("{0}\\EMPTY.jpg", m_strRoot)); }
            catch (Exception e) { m_Empty = null; }

            //m_nImage_Width = waferProbeAlign.Camera_Upper.Resolution.Width;
            //m_nImage_Height = waferProbeAlign.Camera_Upper.Resolution.Height;
            m_nImage_Width = m_TOP_PAK.Width;
            m_nImage_Height = m_TOP_PAK.Height;

            Graphics grp_TOP_PAK = null;
            Graphics grp_MID_PAK = null;
            Graphics grp_BOT_PAK = null;
            Graphics grp_TOP_Wafer = null;
            Graphics grp_MID_Wafer = null;
            Graphics grp_BOT_Wafer = null;

            pictureBox_TOP_PAK.Image = m_TOP_PAK;
            pictureBox_MID_PAK.Image = m_MID_PAK;
            pictureBox_BOT_PAK.Image = m_BOT_PAK;
            pictureBox_TOP_Wafer.Image = m_TOP_Wafer;
            pictureBox_MID_Wafer.Image = m_MID_Wafer;
            pictureBox_BOT_Wafer.Image = m_BOT_Wafer;

            pictureBox_Empty_Wafer.Image = m_Empty;

            int m_nPAKPin_Gap_Width_Pixel = 0;
            int m_nPAKPin_Diameter_Pixel = 0;
            int m_nWafer_GateContactPos_Size_Pixel = 0;
            int m_nPackingOffset_X_Pixel = 0;
            int m_nPackingOffset_Y_Pixel = 0;

            m_dPAK_GatePin_Gap = waferProbeAlign.Config.ParamConfig.PAK_GatePin_Gap <= 0.0 ? 0.4 : waferProbeAlign.Config.ParamConfig.PAK_GatePin_Gap;                                                                          //  default 0 : 0.4 mm
            m_dPAK_GatePin_Diameter = waferProbeAlign.Config.ParamConfig.PAK_GatePin_Diameter <= 0.0 ? 0.11 : waferProbeAlign.Config.ParamConfig.PAK_GatePin_Diameter;                                                          //  default 0 : 0.11 mm
            m_dWafer_GateContactPosition_OverlaySize = waferProbeAlign.Config.ParamConfig.Wafer_GateContactPosition_OverlaySize <= 0.0 ? 0.07 : waferProbeAlign.Config.ParamConfig.Wafer_GateContactPosition_OverlaySize;       //  default 0 : 0.07 mm

            if (waferProbeAlign.Config.ParamConfig.UpperVision_Scale_X <= 0.0)
            {
                m_nPAKPin_Gap_Width_Pixel = (int)(m_dPAK_GatePin_Gap / 0.001726468);                //  하드 코딩 (현재 2x 렌즈에 맞춰서...)
                m_nPAKPin_Diameter_Pixel = (int)(m_dPAK_GatePin_Diameter / 0.001726468);                //  하드 코딩 (현재 2x 렌즈에 맞춰서...)
                m_nWafer_GateContactPos_Size_Pixel = (int)(m_dWafer_GateContactPosition_OverlaySize / 0.001726468);

                if (waferProbeAlign.Config.ParamConfig.Wafer_ProbreCard_PackingPos_Offset_X != 0.0)
                {
                    m_nPackingOffset_X_Pixel = (int)(waferProbeAlign.Config.ParamConfig.Wafer_ProbreCard_PackingPos_Offset_X / 0.001726468);
                }

                if (waferProbeAlign.Config.ParamConfig.Wafer_ProbreCard_PackingPos_Offset_Y != 0.0)
                {
                    m_nPackingOffset_Y_Pixel = (int)(waferProbeAlign.Config.ParamConfig.Wafer_ProbreCard_PackingPos_Offset_Y / 0.001726468);
                }
            }
            else
            {
                m_nPAKPin_Gap_Width_Pixel = (int)(m_dPAK_GatePin_Gap / waferProbeAlign.Config.ParamConfig.UpperVision_Scale_X);
                m_nPAKPin_Diameter_Pixel = (int)(m_dPAK_GatePin_Diameter / waferProbeAlign.Config.ParamConfig.UpperVision_Scale_X);
                m_nWafer_GateContactPos_Size_Pixel = (int)(m_dWafer_GateContactPosition_OverlaySize / waferProbeAlign.Config.ParamConfig.UpperVision_Scale_X);

                if (waferProbeAlign.Config.ParamConfig.Wafer_ProbreCard_PackingPos_Offset_X != 0.0)
                {
                    m_nPackingOffset_X_Pixel = (int)(waferProbeAlign.Config.ParamConfig.Wafer_ProbreCard_PackingPos_Offset_X / waferProbeAlign.Config.ParamConfig.UpperVision_Scale_X);
                }

                if (waferProbeAlign.Config.ParamConfig.Wafer_ProbreCard_PackingPos_Offset_Y != 0.0)
                {
                    m_nPackingOffset_Y_Pixel = (int)(waferProbeAlign.Config.ParamConfig.Wafer_ProbreCard_PackingPos_Offset_Y / waferProbeAlign.Config.ParamConfig.UpperVision_Scale_X);
                }
            }

            Rectangle rect_PAK_Pin = new Rectangle((m_TOP_PAK.Width / 2) - (m_nPAKPin_Gap_Width_Pixel / 2) - (m_nPAKPin_Diameter_Pixel / 2),
                                                    (m_TOP_PAK.Height / 2) - (m_nPAKPin_Diameter_Pixel / 2),
                                                    m_nPAKPin_Gap_Width_Pixel + m_nPAKPin_Diameter_Pixel, m_nPAKPin_Diameter_Pixel);                        //  PAK ROI 크기 (PAK Pin 간격 + Pin 직경 + margin)

            Rectangle rect_Wafer_Pin_Left = new Rectangle((m_TOP_PAK.Width / 2) - (m_nPAKPin_Gap_Width_Pixel / 2) - (m_nWafer_GateContactPos_Size_Pixel / 2) - m_nPackingOffset_X_Pixel,
                                                    (m_TOP_PAK.Height / 2) - (m_nWafer_GateContactPos_Size_Pixel / 2) + m_nPackingOffset_Y_Pixel,
                                                    m_nWafer_GateContactPos_Size_Pixel, m_nWafer_GateContactPos_Size_Pixel);                                            //  Wafer ROI 크기 (Wafer Pin 직경)

            Rectangle rect_Wafer_Pin_Right = new Rectangle((m_TOP_PAK.Width / 2) + (m_nPAKPin_Gap_Width_Pixel / 2) - (m_nWafer_GateContactPos_Size_Pixel / 2) - m_nPackingOffset_X_Pixel,
                                                    (m_TOP_PAK.Height / 2) - (m_nWafer_GateContactPos_Size_Pixel / 2) + m_nPackingOffset_Y_Pixel,
                                                    m_nWafer_GateContactPos_Size_Pixel, m_nWafer_GateContactPos_Size_Pixel);                                            //  Wafer ROI 크기 (Wafer Pin 직경)

            //  TOP 위치, PAK
            if (m_TOP_PAK != null)
            {
                grp_TOP_PAK = Graphics.FromImage(m_TOP_PAK);

                grp_TOP_PAK.DrawLine(new Pen(Color.Lime, 4), m_nImage_Width / 2, 1, m_nImage_Width / 2, m_nImage_Height - 1);               //  Cross Line (세로)
                grp_TOP_PAK.DrawLine(new Pen(Color.Lime, 4), 1, m_nImage_Height / 2, m_nImage_Width - 1, m_nImage_Height / 2);              //  Cross Line (가로)
                grp_TOP_PAK.DrawRectangle(new Pen(Color.Red, 12), rect_PAK_Pin);                                    //  ROI
            }

            //  MID 위치, PAK
            if (m_MID_PAK != null)
            {
                grp_MID_PAK = Graphics.FromImage(m_MID_PAK);

                grp_MID_PAK.DrawLine(new Pen(Color.Lime, 4), m_nImage_Width / 2, 1, m_nImage_Width / 2, m_nImage_Height - 1);               //  Cross Line (세로)
                grp_MID_PAK.DrawLine(new Pen(Color.Lime, 4), 1, m_nImage_Height / 2, m_nImage_Width - 1, m_nImage_Height / 2);              //  Cross Line (가로)
                grp_MID_PAK.DrawRectangle(new Pen(Color.Red, 12), rect_PAK_Pin);                                    //  ROI
            }

            //  BOT 위치, PAK
            if (m_BOT_PAK != null)
            {
                grp_BOT_PAK = Graphics.FromImage(m_BOT_PAK);

                grp_BOT_PAK.DrawLine(new Pen(Color.Lime, 4), m_nImage_Width / 2, 1, m_nImage_Width / 2, m_nImage_Height - 1);               //  Cross Line (세로)
                grp_BOT_PAK.DrawLine(new Pen(Color.Lime, 4), 1, m_nImage_Height / 2, m_nImage_Width - 1, m_nImage_Height / 2);              //  Cross Line (가로)
                grp_BOT_PAK.DrawRectangle(new Pen(Color.Red, 12), rect_PAK_Pin);                                    //  ROI
            }


            //  TOP 위치, Wafer
            if (m_TOP_Wafer != null)
            {
                grp_TOP_Wafer = Graphics.FromImage(m_TOP_Wafer);

                grp_TOP_Wafer.DrawLine(new Pen(Color.Lime, 4), m_nImage_Width / 2, 1, m_nImage_Width / 2, m_nImage_Height - 1);             //  Cross Line (세로)
                grp_TOP_Wafer.DrawLine(new Pen(Color.Lime, 4), 1, m_nImage_Height / 2, m_nImage_Width - 1, m_nImage_Height / 2);            //  Cross Line (가로)
                grp_TOP_Wafer.DrawRectangle(new Pen(Color.Red, 12), rect_Wafer_Pin_Left);                           //  ROI
                grp_TOP_Wafer.DrawRectangle(new Pen(Color.Red, 12), rect_Wafer_Pin_Right);                          //  ROI
            }

            //  MID 위치, Wafer
            if (m_MID_Wafer != null)
            {
                grp_MID_Wafer = Graphics.FromImage(m_MID_Wafer);

                grp_MID_Wafer.DrawLine(new Pen(Color.Lime, 4), m_nImage_Width / 2, 1, m_nImage_Width / 2, m_nImage_Height - 1);             //  Cross Line (세로)
                grp_MID_Wafer.DrawLine(new Pen(Color.Lime, 4), 1, m_nImage_Height / 2, m_nImage_Width - 1, m_nImage_Height / 2);            //  Cross Line (가로)
                grp_MID_Wafer.DrawRectangle(new Pen(Color.Red, 12), rect_Wafer_Pin_Left);                           //  ROI
                grp_MID_Wafer.DrawRectangle(new Pen(Color.Red, 12), rect_Wafer_Pin_Right);                          //  ROI
            }

            //  BOT 위치, Wafer
            if (m_BOT_Wafer != null)
            {
                grp_BOT_Wafer = Graphics.FromImage(m_BOT_Wafer);

                grp_BOT_Wafer.DrawLine(new Pen(Color.Lime, 4), m_nImage_Width / 2, 1, m_nImage_Width / 2, m_nImage_Height - 1);             //  Cross Line (세로)
                grp_BOT_Wafer.DrawLine(new Pen(Color.Lime, 4), 1, m_nImage_Height / 2, m_nImage_Width - 1, m_nImage_Height / 2);            //  Cross Line (가로)
                grp_BOT_Wafer.DrawRectangle(new Pen(Color.Red, 12), rect_Wafer_Pin_Left);                           //  ROI
                grp_BOT_Wafer.DrawRectangle(new Pen(Color.Red, 12), rect_Wafer_Pin_Right);                          //  ROI
            }
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();

            waferProbeAlign.m_nGateTipContactPosForm_Show = 0;
        }
    }
}
