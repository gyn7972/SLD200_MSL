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
using QMC.Core;
using QMC.Common;
using QMC.Common.Modules;
using QMC.Common.Vision;
using QMC.Common.Parts;
using System.Threading;
using MessageBox = System.Windows.Forms.MessageBox;


namespace CWA150SA_Onsemi300
{
    public partial class FormLeakCheck : Form
    {
        static WaferProbeAlign waferProbeAlign;
        public System.Windows.Forms.Timer timer_Status;

        int m_nLeakCheck_Blink;
        bool m_bLeakCheck_Blink;

        public bool m_bLeakCheck_Run;
        DateTime m_LeakCheck_Now = new DateTime();
        DateTime m_LeakCheck_Complete = new DateTime();
        public double m_dLeakCheck_Total;

        public FormLeakCheck()
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

            m_nLeakCheck_Blink = 0;
            m_bLeakCheck_Blink = false;

            m_bLeakCheck_Run = false;

            //  Leak Check 시간
            m_dLeakCheck_Total = waferProbeAlign.Config.ParamConfig.Packing_OP_LeakCheck_Time <= 0 ? 30 : waferProbeAlign.Config.ParamConfig.Packing_OP_LeakCheck_Time;

            //  타이머
            timer_Status = new System.Windows.Forms.Timer();
            timer_Status.Interval = 30;
            timer_Status.Tick += new System.EventHandler(Timer_StatusFunc);
            timer_Status.Enabled = true;

            this.Load += FormLeakCheck_Load;
        }

        private void FormLeakCheck_Load(object sender, EventArgs e)
        {
            //  얼라인 시작할 때와 패킹 중 Leak Check 할 때 보이는 component 를 다르게

            m_bLeakCheck_Run = false;

            if (waferProbeAlign.m_nWaferProbeAlign_MainStep == (int)WaferProbeAlign.WaferProbeAlign_Step.AlignStart_OP_Check_Confirm)
            {
                //  얼라인 시작시 OP 확인 창으로 변경

                pictureBox_LeakTestValve_Open_On.Visible = true;
                pictureBox_LeakTestValve_Open_Off.Visible = false;
                pictureBox_LeakTestValve_Close_On.Visible = false;
                pictureBox_LeakTestValve_Close_Off.Visible = false;

                baseLabel_Comment_Open.Visible = true;

                baseLabel_Comment_Close1.Visible = false;
                baseLabel_Comment_Close2.Visible = false;
                baseLabel_Comment_Close3.Visible = false;

                baseLabel_Arrow1.Visible = false;
                baseLabel_Arrow2.Visible = false;
                baseLabel_Arrow3.Visible = false;

                button_PackingVacSig_Off.Visible = false;
                pictureBox_Air_Gauge.Visible = false;

                btn_Align_Start.Visible = true;
                btn_Align_Cancel.Visible = true;
                btn_Packing_Complete.Visible = false;

                Point m_pt1 = new Point(baseLabel_Comment_Open.Location.X, baseLabel_Comment_Open.Location.Y + 100);
                Point m_pt2 = new Point(baseLabel_Open.Location.X, baseLabel_Open.Location.Y + 120);
                baseLabel_Comment_Open.Location = m_pt1;
                baseLabel_Open.Location = m_pt2;

                tb_LeakCheck_Time_Remained.Visible = false;
            }
            else if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step == (int)WaferProbeAlign.WaferProbeCard_Packing_Step.PackingComplete_OP_Check_Confirm)
            {
                //  패킹 완료시 OP 확인 창으로 변경

                pictureBox_LeakTestValve_Open_On.Visible = false;
                pictureBox_LeakTestValve_Open_Off.Visible = false;
                pictureBox_LeakTestValve_Close_On.Visible = true;
                pictureBox_LeakTestValve_Close_Off.Visible = false;

                baseLabel_Comment_Open.Visible = false;

                baseLabel_Comment_Close1.Visible = true;
                baseLabel_Comment_Close2.Visible = true;
                baseLabel_Comment_Close3.Visible = true;

                baseLabel_Arrow1.Visible = true;
                baseLabel_Arrow2.Visible = true;
                baseLabel_Arrow3.Visible = true;

                button_PackingVacSig_Off.Visible = true;
                pictureBox_Air_Gauge.Visible = true;

                btn_Align_Start.Visible = false;
                btn_Align_Cancel.Visible = false;
                btn_Packing_Complete.Visible = true;
                
                if (waferProbeAlign.Config.ParamConfig.AlignPacking_OP_LeakCheck_Usage)
                {
                    tb_LeakCheck_Time_Remained.Visible = true;
                    tb_LeakCheck_Time_Remained.Text = "- - -";
                    btn_Packing_Complete.Enabled = false;
                }
                else
                {
                    tb_LeakCheck_Time_Remained.Visible = false;
                    btn_Packing_Complete.Enabled = true;
                }
            }
            else                        //  패킹 시작할 때나 완료할 때가 아니므로 바로 창 닫아버리기~
            {
                this.Close();
            }
        }

        void Timer_StatusFunc(object sender, EventArgs e)
        {
            //  패킹 시작할 때와 Leak Check 할 때 보이는 component 를 다르게

            timer_Status.Enabled = false;

            //  Blink
            m_nLeakCheck_Blink++;
            if ((m_nLeakCheck_Blink > 0) && (m_nLeakCheck_Blink <= 15))
            {
                m_bLeakCheck_Blink = true;
            }
            else if ((m_nLeakCheck_Blink > 15) && (m_nLeakCheck_Blink <= 30))
            {
                m_bLeakCheck_Blink = false;
            }
            else
            {
                m_nLeakCheck_Blink = 0;
            }

            if (waferProbeAlign.m_nWaferProbeAlign_MainStep == (int)WaferProbeAlign.WaferProbeAlign_Step.AlignStart_OP_Check_Confirm)
            {
                //  패킹 시작시 OP 확인 창으로 변경

                if (m_bLeakCheck_Blink == true)
                {
                    pictureBox_LeakTestValve_Open_On.Visible = true;
                    pictureBox_LeakTestValve_Open_Off.Visible = false;

                    baseLabel_Open.ForeColor = Color.Black;
                    baseLabel_Open.BackColor = Color.Yellow;
                }
                else if (m_bLeakCheck_Blink == false)
                {
                    pictureBox_LeakTestValve_Open_Off.Visible = true;
                    pictureBox_LeakTestValve_Open_On.Visible = false;

                    baseLabel_Open.ForeColor = Color.Yellow;
                    baseLabel_Open.BackColor = Color.Black;
                }

                baseLabel_Comment_Open.Visible = true;
                baseLabel_Open.Visible = true;

                baseLabel_Comment_Close1.Visible = false;
                baseLabel_Comment_Close2.Visible = false;
                baseLabel_Comment_Close3.Visible = false;
                baseLabel_Close.Visible = false;

                baseLabel_Arrow1.Visible = false;
                baseLabel_Arrow2.Visible = false;
                baseLabel_Arrow3.Visible = false;

                button_PackingVacSig_Off.Visible = false;
                pictureBox_Air_Gauge.Visible = false;

                btn_Align_Start.Visible = true;
                btn_Align_Cancel.Visible = true;
                btn_Packing_Complete.Visible = false;
                btn_Packing_Cancel2.Visible = false;
            }
            else if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step == (int)WaferProbeAlign.WaferProbeCard_Packing_Step.PackingComplete_OP_Check_Confirm)
            {
                //  패킹 완료시 OP 확인 창으로 변경

                if (m_bLeakCheck_Blink == true)
                {
                    pictureBox_LeakTestValve_Close_On.Visible = true;
                    pictureBox_LeakTestValve_Close_Off.Visible = false;

                    baseLabel_Close.ForeColor = Color.Black;
                    baseLabel_Close.BackColor = Color.Yellow;
                }
                else if (m_bLeakCheck_Blink == false)
                {
                    pictureBox_LeakTestValve_Close_Off.Visible = true;
                    pictureBox_LeakTestValve_Close_On.Visible = false;

                    baseLabel_Close.ForeColor = Color.Yellow;
                    baseLabel_Close.BackColor = Color.Black;
                }

                baseLabel_Comment_Open.Visible = false;
                baseLabel_Open.Visible = false;

                baseLabel_Comment_Close1.Visible = true;
                baseLabel_Comment_Close2.Visible = true;
                baseLabel_Comment_Close3.Visible = true;
                baseLabel_Close.Visible = true;

                baseLabel_Arrow1.Visible = true;
                baseLabel_Arrow2.Visible = true;
                baseLabel_Arrow3.Visible = true;

                button_PackingVacSig_Off.Visible = true;
                pictureBox_Air_Gauge.Visible = true;

                btn_Align_Start.Visible = false;
                btn_Align_Cancel.Visible = false;
                btn_Packing_Complete.Visible = true;
                btn_Packing_Cancel2.Visible = true;
            }

            //  Leak Check 남은시간 갱신

            if (m_bLeakCheck_Run)
            {
                m_LeakCheck_Now = DateTime.Now;

                if (m_LeakCheck_Complete.Minute != 1)
                {
                    TimeSpan ts = m_LeakCheck_Now.Subtract(m_LeakCheck_Complete);
                    double diff = ts.TotalDays;

                    int m_nLeakCheck_Minute = 0;
                    int m_nLeakCheck_Second = 0;

                    if ((ts.Minutes < 0) || (ts.Seconds < 0))
                    {
                        m_nLeakCheck_Minute = ts.Minutes * -1;
                        m_nLeakCheck_Second = ts.Seconds * -1;

                        tb_LeakCheck_Time_Remained.Visible = true;
                        tb_LeakCheck_Time_Remained.Text = string.Format("{0,2}분 {1,2}초", m_nLeakCheck_Minute, m_nLeakCheck_Second);
                    }
                    else
                    {
                        tb_LeakCheck_Time_Remained.Text = "완료";
                        btn_Packing_Complete.Enabled = true;
                        m_bLeakCheck_Run = false;
                    }
                }
            }

            timer_Status.Enabled = true;
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




        private void btn_Close_Click(object sender, EventArgs e)
        {
            //  패킹 작업 마무리 (패킹 완료 시)

            if (waferProbeAlign.waferProbeAlignParameter.IsDO_Probe_Packing())
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 패킹 공압 신호를 OFF 하고,\r\n\r\n공압 게이지의 압력 변화를 확인한 후\r\n진행해야 합니다.");
                return;
            }

            Log.Write("CWA150SA", Equipment.User_Name, "Leak Check Dialog", "Packing 마무리 창 닫기 버튼 클릭");

            if (waferProbeAlign.m_nWafer_ProbeCard_Packing_Step == (int)WaferProbeAlign.WaferProbeCard_Packing_Step.PackingComplete_OP_Check_Confirm)
            {
                waferProbeAlign.m_bWafer_ProbeCard_AlignPacking_OP_Confirm = true;
            }

            waferProbeAlign.m_nLeakCheckForm_Show = 0;
            waferProbeAlign.m_bLeakCheckForm_Complete = true;
            this.Close();
        }

        private void button_PackingVacSig_Off_Click(object sender, EventArgs e)
        {
            Log.Write("CWA150SA", Equipment.User_Name, "Leak Check Dialog", "Packing 공압 신호 Off");

            waferProbeAlign.waferProbeAlignParameter.DO_Probe_Packing(false);

            if (waferProbeAlign.Config.ParamConfig.AlignPacking_OP_LeakCheck_Usage)
            {
                m_bLeakCheck_Run = true;
                m_LeakCheck_Complete = DateTime.Now.AddSeconds(m_dLeakCheck_Total);
            }
        }

        private void btn_Packing_Cancel2_Click(object sender, EventArgs e)
        {
            //  패킹 취소 (패킹 완료 시)

            var mb1 = new MessageBoxYesNo();
            if (DialogResult.Yes != mb1.ShowDialog("Question ?", "패킹 작업을 취소하시겠습니까?\r\n\r\n[다시 얼라인을 진행해야 합니다.]"))
                return;

            waferProbeAlign.waferProbeAlignParameter.DO_Probe_Packing(false);

            waferProbeAlign.m_bWafer_ProbeCard_AlignPacking_OP_Confirm = false;

            waferProbeAlign.timer_MainWork.Enabled = false;
            waferProbeAlign.m_nWafer_ProbeCard_Packing_Step = (int)WaferProbeAlign.WaferProbeCard_Packing_Step.None;


            //  패킹할 때 어떤 이유에서인지 취소했으므로, 이것저것 다 리셋. (다시 얼라인 하고 패킹 진행하도록)
            waferProbeAlign.m_bWafer_ThetaAlign_OK = false;
            waferProbeAlign.m_bWafer_XYAlign_OK = false;
            waferProbeAlign.m_bProbeCard_TiltCheck_OK = false;
            waferProbeAlign.m_bProbeCard_XYAlign_OK = false;

            waferProbeAlign.m_bProbeCard_XYAlign_ErrorCheck_OK = false;         //  Probe Card XY Align Error Check OK
            waferProbeAlign.m_bWafer_XYAlign_ErrorCheck_OK = false;             //  Wafer XY Align Error Check OK
            waferProbeAlign.m_bWaferProbeAlign_ErrorCheck_Complete = false;
            waferProbeAlign.m_bWaferProbeAlign_ErrorCheck_All_OK = false;

            waferProbeAlign.m_dWafer_ProbeCard_PackingPos_Axis_U = -1;                  //  패킹할 때의 UVW Stage 좌표 (언패킹 시 사용한다.)
            waferProbeAlign.m_dWafer_ProbeCard_PackingPos_Axis_V = -1;                  //  패킹할 때의 UVW Stage 좌표 (언패킹 시 사용한다.)
            waferProbeAlign.m_dWafer_ProbeCard_PackingPos_Axis_W = -1;                  //  패킹할 때의 UVW Stage 좌표 (언패킹 시 사용한다.)

            waferProbeAlign.m_dWafer_ProbeCard_AlignPos_Axis_U = -1;                    //  얼라인 완료되었을때 UVW Stage 좌표 (패킹 시 사용한다.)
            waferProbeAlign.m_dWafer_ProbeCard_AlignPos_Axis_V = -1;                    //  얼라인 완료되었을때 UVW Stage 좌표 (패킹 시 사용한다.)
            waferProbeAlign.m_dWafer_ProbeCard_AlignPos_Axis_W = -1;                    //  얼라인 완료되었을때 UVW Stage 좌표 (패킹 시 사용한다.)

            waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Count_Total = 3;              //  오차 확인 위치 총 개수
            waferProbeAlign.m_nWaferProbeAlign_ErrorCheck_Count = 0;                    //  오차 확인 위치 카운트 (3개,   0: TOP, 1: MID, 2: BOT)

            for (int nPos = 0; nPos < 3; nPos++)
            {
                waferProbeAlign.AlignmentErrorCheck_Position[nPos].X = 0.0;
                waferProbeAlign.AlignmentErrorCheck_Position[nPos].Y = 0.0;

                waferProbeAlign.ManualPacking_OffsetPosition_UVW[nPos] = 0.0;

                for (int nSide = 0; nSide < 2; nSide++)
                {
                    waferProbeAlign.AlignmentErrorCheck_Status[nPos, nSide] = false;

                    waferProbeAlign.AlignmentErrorCheck_MarkPosition[nPos, nSide].X = 0.0;
                    waferProbeAlign.AlignmentErrorCheck_MarkPosition[nPos, nSide].Y = 0.0;
                }
            }

            waferProbeAlign.m_nLeakCheckForm_Show = 0;
            waferProbeAlign.m_bLeakCheckForm_Complete = true;
            this.Close();
        }

        private void btn_Align_Start_Click(object sender, EventArgs e)
        {
            //  얼라인 진행 (얼라인 시작 시)

            if (waferProbeAlign.m_nWaferProbeAlign_MainStep == (int)WaferProbeAlign.WaferProbeAlign_Step.AlignStart_OP_Check_Confirm)
            {
                waferProbeAlign.m_bWafer_ProbeCard_AlignPacking_OP_Confirm = true;
            }

            waferProbeAlign.m_nLeakCheckForm_Show = 0;
            waferProbeAlign.m_bLeakCheckForm_Complete = true;
            this.Close();
        }

        private void btn_Align_Cancel_Click(object sender, EventArgs e)
        {
            //  얼라인 취소 (얼라인 시작 시)

            waferProbeAlign.m_bWafer_ProbeCard_AlignPacking_OP_Confirm = false;

            waferProbeAlign.timer_MainWork.Enabled = false;
            waferProbeAlign.m_nWaferProbeAlign_MainStep = (int)WaferProbeAlign.WaferProbeAlign_Step.None;

            waferProbeAlign.m_nLeakCheckForm_Show = 0;
            waferProbeAlign.m_bLeakCheckForm_Complete = true;
            this.Close();
        }
    }
}
