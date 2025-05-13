using QMC.Common.Modules;
using QMC.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SLD200.NewStyleForm.FormNew_JogPopup;
using static QMC.Common.Equipment;

namespace SLD200.NewStyleForm.NewSubForm
{
    public partial class FormNewSub_JogPopup_Unloader : UserControl, IJogControlProvider
    {
        private bool m_bFormVisible = false; // 실제 Show 상태 여부
        private bool m_bInitialized = false;
        private FormNew_JogPopup m_Parent;
        static Unloader unloader;

        public bool IsJogMoveContinuous => radioButton_JogPopup_Unloader_JogMove_Continuous.Checked;
        public bool IsJogMoveStep => radioButton_JogPopup_Unloader_JogMove_Step.Checked;
        public double JogStepDistance => Equipment.ToDouble(textBox_JogPopup_Unloader_JogMove_StepSize.Text);
        public Type_Motor_Speed JogSpeedType =>
            radioButton_JogPopup_Unloader_Move_MoveMode_Fine.Checked ? Type_Motor_Speed.Fine : Type_Motor_Speed.Coarse;


        public FormNewSub_JogPopup_Unloader(FormNew_JogPopup parent)
        {
            InitializeComponent();
            m_Parent = parent;
            this.Load += FormNewSub_JogPopup_Unloader_Load; // 여기서 Load 이벤트 연결


        }

        private void FormNewSub_JogPopup_Unloader_Load(object sender, EventArgs e)
        {
            if (m_bInitialized)
                return;

            //Size 축소 / 확대 안되게 하기 위한 코드.
            this.AutoScaleMode = AutoScaleMode.None;
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.UpdateStyles();

            

            //GUI생성 완료 후 Data 및 Cintroller 업데이트!
            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;
            foreach (Module module in m_collectionModules)
            {
                if (module.Name == "Unloader")
                {
                    unloader = module as Unloader;
                }
            }

            InitAxisLabelMap();
            InitializeJogButtons();
            m_bInitialized = true;
        }

        public void OnShow()
        {
            if (m_bFormVisible)
                return;

            m_bFormVisible = true;
        }
        public void OnHide()
        {
            if (!m_bFormVisible)
                return;

            m_bFormVisible = false;
        }

        private void InitializeJogButtons()
        {
            button_JogPopup_Unloader_TransferX_Pos.Tag = "Unloader,TR_X,1";
            button_JogPopup_Unloader_TransferX_Neg.Tag = "Unloader,TR_X,-1";
            button_JogPopup_Unloader_TransferZ_Pos.Tag = "Unloader,TR_Z,1";
            button_JogPopup_Unloader_TransferZ_Neg.Tag = "Unloader,TR_Z,-1";
            button_JogPopup_Unloader_portZ0_Pos.Tag = "Unloader,Z0,1";
            button_JogPopup_Unloader_portZ0_Neg.Tag = "Unloader,Z0,-1";
            button_JogPopup_Unloader_portZ1_Pos.Tag = "Unloader,Z1,1";
            button_JogPopup_Unloader_portZ1_Neg.Tag = "Unloader,Z1,-1";


            // Initialize the jog buttons here if needed
            ConnectJogEvents(m_Parent);
        }

        // Form의 인스턴스를 받아서 델리게이트 연결
        private void ConnectJogEvents(FormNew_JogPopup parent)
        {
            button_JogPopup_Unloader_TransferX_Pos.MouseDown += parent.button_AxisJog_MouseDown;
            button_JogPopup_Unloader_TransferX_Neg.MouseDown += parent.button_AxisJog_MouseDown;
            button_JogPopup_Unloader_TransferZ_Pos.MouseDown += parent.button_AxisJog_MouseDown;
            button_JogPopup_Unloader_TransferZ_Neg.MouseDown += parent.button_AxisJog_MouseDown;
            button_JogPopup_Unloader_portZ0_Pos.MouseDown += parent.button_AxisJog_MouseDown;
            button_JogPopup_Unloader_portZ0_Neg.MouseDown += parent.button_AxisJog_MouseDown;
            button_JogPopup_Unloader_portZ1_Pos.MouseDown += parent.button_AxisJog_MouseDown;
            button_JogPopup_Unloader_portZ1_Neg.MouseDown += parent.button_AxisJog_MouseDown;
            button_JogPopup_Unloader_TransferX_Pos.MouseUp += parent.button_AxisJog_MouseUp;
            button_JogPopup_Unloader_TransferX_Neg.MouseUp += parent.button_AxisJog_MouseUp;
            button_JogPopup_Unloader_TransferZ_Pos.MouseUp += parent.button_AxisJog_MouseUp;
            button_JogPopup_Unloader_TransferZ_Neg.MouseUp += parent.button_AxisJog_MouseUp;
            button_JogPopup_Unloader_portZ0_Pos.MouseUp += parent.button_AxisJog_MouseUp;
            button_JogPopup_Unloader_portZ0_Neg.MouseUp += parent.button_AxisJog_MouseUp;
            button_JogPopup_Unloader_portZ1_Pos.MouseUp += parent.button_AxisJog_MouseUp;
            button_JogPopup_Unloader_portZ1_Neg.MouseUp += parent.button_AxisJog_MouseUp;
        }

        private Dictionary<Unloader.nAxis, Label> unloaderAxisLabelMap;
        private void InitAxisLabelMap()
        {
            unloaderAxisLabelMap = new Dictionary<Unloader.nAxis, Label>
            {
                { Unloader.nAxis.Z0, label_JogPopup_Unloader_EncPosition_UL_Z0 },
                { Unloader.nAxis.Z1, label_JogPopup_Unloader_EncPosition_UL_Z1 },
                { Unloader.nAxis.TR_X, label_JogPopup_Unloader_EncPosition_UL_TRX },
                { Unloader.nAxis.TR_Z, label_JogPopup_Unloader_EncPosition_UL_TRZ },
            };
        }

        private string FormatPos(double pos)
        {
            return double.IsNaN(pos) ? "ERR" : string.Format("{0:F3}", pos);
        }

        public void UpdateStatus()
        {
            if (!Equipment.AjinBoard_Opened)
                return;

            foreach (var pair in unloaderAxisLabelMap)
            {
                double pos = unloader.GetEncUnloaderPos_Motor(pair.Key);
                pair.Value.Text = FormatPos(pos);
            }
        }
    }
}
