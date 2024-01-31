using QMC.Common;
using QMC.Common.Hmi;
using QMC.Common.Modules;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ACS.SPiiPlusNET;
using QMC.Common.Motion.ACS.Motions;
using QMC.Common.Parts;
using QMC.Core;
using Microsoft.Win32;
using SpiralLab.Sirius;
using Point = System.Drawing.Point;
using SpiralLab;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Rectangle = System.Drawing.Rectangle;
using static QMC.Common.Modules.WaferProbeAlign;
using System.Threading;
using QMC.Common.Vision.Optics;
using System.Runtime.Remoting.Channels;
using QMC.Common.VisionPart;

namespace CWA150SA_Onsemi300
{
    public partial class FormOperationManualMode : FormSubContentBase
    {
        #region Field
        protected ModuleStateControl m_ModuleStateControl;
        private IlluminatorControl m_IlluminatorControl;
        //public ModulePositionControl m_WaferProbeAlignPosControl;
        public JogControl m_JogControl;
        public AutoFocusControl m_AutoFocusControl;

        private ProgramStates m_nProgramState0;
        private ProgramStates m_nProgramState1;
        private MotorStates m_nMotorState0;
        private MotorStates m_nMotorState1;

        public System.Windows.Forms.Timer timer_LaserFocusCheck;
        public System.Windows.Forms.Timer timer_Status;

        double m_dAttenuatorPos_Min = 0.0;
        double m_dAttenuatorPos_Max = 0.0;
        double m_dTargetPower = 0.0;

        //private bool m_bSelected_HighResCamera = true;            //  waferProbeAlign class 에 선언
        private bool m_bCameraChange_1time = false;
        private int m_nTabControl_CurrentIndex = -1;

        private bool m_bBlink = false;
        private int m_nBlink = 0;

        protected WaferProbeAlign waferProbeAlign;
        #endregion

        string strRegistryTmp = "SOFTWARE\\";       //  레지스트리 최상위 폴더 지정
        string strAppName = "CWA-150SA_Onsemi";

        #region Tick Count Check
        //System.Diagnostics.Stopwatch sw_User = new System.Diagnostics.Stopwatch();

        public enum TickType : int
        {
            TICK_USER = 0,          //  0 : User
            TICK_USER2 = 1,         //  1 : User2
            TICK_USER3 = 2,         //  2 : User3
            TICK_LASER_FOCUS = 3,   //  3 : Laser Focus Check
        }

        public int[,] TickCount_Cycle = new int[10, 3];          //  0 : User
                                                                 //  1 : User2
                                                                 //  2 : User3
                                                                 //  3 : Laser Focus Check

        public void TickCount_Start(int m_nIndex)
        {
            TickCount_Cycle[m_nIndex, 0] = Environment.TickCount;
        }
        public int TickCount_Elapsed(int m_nIndex)
        {
            int TickCount_Elapsed = 0;
            TickCount_Cycle[m_nIndex, 1] = Environment.TickCount;
            TickCount_Elapsed = TickCount_Cycle[m_nIndex, 1] - TickCount_Cycle[m_nIndex, 0];

            return TickCount_Elapsed;
        }
        #endregion

        #region Registry 읽기 쓰기 삭제
        public bool WriteRegistry(string strAppName, string strSubKey, string strKey, string strValue)
        {
            RegistryKey rkReg = Registry.CurrentUser.OpenSubKey(strRegistryTmp + strAppName, true);

            //  null 이면 폴더(레지스트리)가 없으므로 만든다.
            if (rkReg == null) rkReg = Registry.CurrentUser.CreateSubKey(strRegistryTmp + strAppName);

            //  OpenSubKey (하위 폴더(레지스트리 이름), 쓰기 선택 True 쓰기 False 및 인자가 없다면 읽기)
            RegistryKey rkSub = Registry.CurrentUser.OpenSubKey(strRegistryTmp + strAppName + "\\" + strSubKey, true);

            if (rkSub == null) rkSub = Registry.CurrentUser.CreateSubKey(strRegistryTmp + strAppName + "\\" + strSubKey);

            try
            {
                rkSub.SetValue(strKey, strValue);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                return false;
            }

            return true;
        }

        public string ReadRegistry(string strAppName, string strSubKey, string strKey)
        {
            RegistryKey reg;

            try
            {
                if (Registry.CurrentUser.OpenSubKey(strRegistryTmp + strAppName).OpenSubKey(strSubKey) == null)
                {
                    WriteRegistry(strAppName, strSubKey, strKey, "0");
                }

                reg = Registry.CurrentUser.OpenSubKey(strRegistryTmp + strAppName).OpenSubKey(strSubKey);
            }
            catch (Exception ex)
            {
                return "0";
            }

            return reg.GetValue(strKey, "0").ToString();
        }

        public bool DeleteRegistry(string strSubKey)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey(strRegistryTmp, true);

            try
            {
                //  하위 폴더(레즈스트리)가 있으면 삭제 안됨.
                //  if (rk != null) rk.DeleteSubKey( strSubKey ) ;

                //  하위 폴더(레즈스트리)가 있어도 삭제.
                if (rk != null) rk.DeleteSubKeyTree(strSubKey);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        #endregion

        public FormOperationManualMode()
            :base(FormType.withButton.ToString(),"Manual Mode")
        {
            InitializeComponent();

            ManualMode_CWA150SA manualMode_CWA150SA = new ManualMode_CWA150SA();

            this.flowLayoutPanelButton.Location = new System.Drawing.Point(Configuration.ButtonSize.Width + 6, 0);
            this.flowLayoutPanelButton.Size = new System.Drawing.Size(Configuration.PanelSize.Width - Configuration.ButtonSize.Width, Configuration.PanelSize.Height);

            this.panelContent.Location = new System.Drawing.Point(0, Configuration.PanelSize.Height);
            this.panelContent.Size = new System.Drawing.Size(Configuration.PanelbuttonSize.Width + 10, Configuration.FormContentSize.Height);

            this.panelContent.Controls.Add(baseLabelTitle);

            //  여기에 Manual Mode 화면에 보여줄 것을 등록해야지...
            manualMode_CWA150SA.Location = new Point(5, 30);
            this.panelContent.Controls.Add(manualMode_CWA150SA);
        }
    }
}
