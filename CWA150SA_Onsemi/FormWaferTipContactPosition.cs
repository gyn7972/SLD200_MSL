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
using System.Threading;


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

        public static Image ResizeImage(Image image)
        {
            if (image != null)
            {
                Bitmap croppedBitmap = new Bitmap(image);
                croppedBitmap = croppedBitmap.Clone(
                        new Rectangle(100, 100, image.Width - 200, image.Height - 200),
                        System.Drawing.Imaging.PixelFormat.DontCare);
                return croppedBitmap;
            }
            else
            {
                return image;
            }
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
            Image m_Empty_PAK = null;
            Image m_Empty_Wafer = null;


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

            try { m_Empty_PAK = Image.FromFile(string.Format("{0}\\EMPTY_PAK.jpg", m_strRoot)); }
            catch (Exception e) { m_Empty_PAK = null; }

            try { m_Empty_Wafer = Image.FromFile(string.Format("{0}\\EMPTY_Wafer.jpg", m_strRoot)); }
            catch (Exception e) { m_Empty_Wafer = null; }

            if (m_TOP_PAK != null)
            {
                m_nImage_Width = m_TOP_PAK.Width;
                m_nImage_Height = m_TOP_PAK.Height;
            }
            else if (m_MID_PAK != null)
            {
                m_nImage_Width = m_MID_PAK.Width;
                m_nImage_Height = m_MID_PAK.Height;
            }
            else if (m_BOT_PAK != null)
            {
                m_nImage_Width = m_BOT_PAK.Width;
                m_nImage_Height = m_BOT_PAK.Height;
            }
            else
            {
                m_nImage_Width = waferProbeAlign.Camera_Upper.Resolution.Width;
                m_nImage_Height = waferProbeAlign.Camera_Upper.Resolution.Height;
            }
            

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

            pictureBox_Empty_PAK.Image = m_Empty_PAK;
            pictureBox_Empty_Wafer.Image = m_Empty_Wafer;

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

            Rectangle rect_PAK_Pin = new Rectangle((m_nImage_Width / 2) - (m_nPAKPin_Gap_Width_Pixel / 2) - (m_nPAKPin_Diameter_Pixel / 2),
                                                    (m_nImage_Height / 2) - (m_nPAKPin_Diameter_Pixel / 2),
                                                    m_nPAKPin_Gap_Width_Pixel + m_nPAKPin_Diameter_Pixel, m_nPAKPin_Diameter_Pixel);                        //  PAK ROI 크기 (PAK Pin 간격 + Pin 직경 + margin)

            Rectangle rect_Wafer_Pin_Left = new Rectangle((m_nImage_Width / 2) - (m_nPAKPin_Gap_Width_Pixel / 2) - (m_nWafer_GateContactPos_Size_Pixel / 2) - m_nPackingOffset_X_Pixel,
                                                    (m_nImage_Height / 2) - (m_nWafer_GateContactPos_Size_Pixel / 2) + m_nPackingOffset_Y_Pixel,
                                                    m_nWafer_GateContactPos_Size_Pixel, m_nWafer_GateContactPos_Size_Pixel);                                            //  Wafer ROI 크기 (Wafer Pin 직경)

            Rectangle rect_Wafer_Pin_Right = new Rectangle((m_nImage_Width / 2) + (m_nPAKPin_Gap_Width_Pixel / 2) - (m_nWafer_GateContactPos_Size_Pixel / 2) - m_nPackingOffset_X_Pixel,
                                                    (m_nImage_Height / 2) - (m_nWafer_GateContactPos_Size_Pixel / 2) + m_nPackingOffset_Y_Pixel,
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

        public bool TipContactImage_TotalSave(string m_strOperator, string m_strStartTime)
        {
            bool m_bRet = true;
            string m_strDirectory = null;
            string m_strRoot = null;
            string m_strDate = null;
            string m_strRecipe = null;
            string m_strImageFile = null;

            //  데이터 확인
            if (m_strOperator == null)                                             //  Align 을 진행하지 않았으면?
            {
                m_strOperator = "UnknownOperator";
            }

            if (m_strStartTime == null)                                             //  Align 을 진행하지 않았으면? --> 이미지 저장할 필요 없음
            {
                return false;
                //m_strStartTime = "NoAlign";
            }


            //  이미지 저장 경로 (꼭대기)
            m_strRoot = "D:\\CWA-150SA_AlignImage";
            DirectoryInfo di = new DirectoryInfo(m_strRoot);
            if (!di.Exists)                                                         //  없으면 생성
            {
                di.Create();
            }

            //  이미지 저장 경로 (꼭대기 / 날짜)
            m_strDate = DateTime.Now.ToString("yyyy-MM-dd");
            m_strDirectory = string.Format("{0}\\{1}", m_strRoot, m_strDate);
            DirectoryInfo di2 = new DirectoryInfo(m_strDirectory);
            if (!di2.Exists)                                                         //  없으면 생성
            {
                di2.Create();
            }

            //  이미지 저장 경로 (꼭대기 / 날짜 / 레시피)
            RecipeInfo m_recipeInfo = new RecipeInfo();
            m_recipeInfo = Equipment.GetCurrentRecipe();

            if (m_recipeInfo != null)
            {
                m_strRecipe = m_recipeInfo.Name;
            }
            else
            {
                m_strRecipe = "NO_RECIPE";
            }

            m_strDirectory = string.Format("{0}\\{1}\\{2}", m_strRoot, m_strDate, m_strRecipe);
            DirectoryInfo di3 = new DirectoryInfo(m_strDirectory);
            if (!di3.Exists)                                                         //  없으면 생성
            {
                di3.Create();
            }

            //  이미지 저장 경로 (꼭대기 / 날짜 / 레시피 / 작업자)
            m_strDirectory = string.Format("{0}\\{1}\\{2}\\{3}", m_strRoot, m_strDate, m_strRecipe, m_strOperator);
            DirectoryInfo di4 = new DirectoryInfo(m_strDirectory);
            if (!di4.Exists)                                                         //  없으면 생성
            {
                di4.Create();
            }

            //  이미지 저장 경로 (꼭대기 / 날짜 / 레시피 / 작업자 / 작업자_얼라인시작시간_얼라인위치_카메라방향)
            //  작업자 : 작업자 이름
            //  얼라인시작시간 : 얼라인 시작 버튼을 눌렀을 때의 시간 (년-월-일)

            m_strImageFile = string.Format("{0}_{1}_AllTipPos", m_strOperator, m_strStartTime);

            ImgCapture imgCapture = new ImgCapture();
            imgCapture.SetPath(m_strDirectory);
            imgCapture.ScreenCapture(m_strImageFile, ActiveForm.Width, ActiveForm.Height, ActiveForm.Location);

            return m_bRet;
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            //  Tip Contact 화면을 이미지로 저장
            //string pngFilePath = "D:\\aaa.png";

            //int m_nStartX = 0;
            //int m_nStartY = 0;

            //m_nStartX = (Screen.PrimaryScreen.Bounds.Width - this.Width) / 2;
            //m_nStartY = (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2;

            //ImgCapture imgCapture = new ImgCapture(m_nStartX, m_nStartY - 50, this.Width, this.Height);
            //imgCapture.SetPath(pngFilePath);
            //imgCapture.DoCaptureImage();

            //ImgCapture imgCapture = new ImgCapture();
            //imgCapture.SetPath(pngFilePath);
            //imgCapture.ScreenCapture(ActiveForm.Width, ActiveForm.Height, ActiveForm.Location);


            //  닫기 버튼 안보이게 (저장 이미지에 닫기 버튼이 보이면 좀 거시기 허니까)
            btn_Close.Visible = false;

            TipContactImage_TotalSave(Equipment.User_Name, Equipment.AlignStart_Time);
            Equipment.AlignStart_Time = null;

            //  닫기 버튼 보이게
            btn_Close.Visible = true;


            this.Close();

            waferProbeAlign.m_nGateTipContactPosForm_Show = 0;
        }
    }
}
