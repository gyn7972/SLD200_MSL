using QMC.Common.Modules;
using QMC.Common.VisionPart;
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
using static OpenCvSharp.LineIterator;
using static SLD200.NewStyleForm.FormNew_JogPopup;
using static QMC.Common.Equipment;
using QMC.Common.Parts;

namespace SLD200.NewStyleForm.NewSubForm
{
    public partial class FormNewSub_JogPopup_Loader : UserControl, IJogControlProvider
    {
        private bool m_bFormVisible = false; // 실제 Show 상태 여부
        private bool m_bInitialized = false;
        private FormNew_JogPopup m_Parent;
        static Loader loader;

        public bool IsJogMoveContinuous => radioButton_JogPopup_Loader_JogMove_Continuous.Checked;
        public bool IsJogMoveStep => radioButton_JogPopup_Loader_JogMove_Step.Checked;
        public double JogStepDistance => Equipment.ToDouble(textBox_JogPopup_Loader_JogMove_StepSize.Text);
        public Type_Motor_Speed JogSpeedType =>
            radioButton_JogPopup_Loader_Move_MoveMode_Fine.Checked ? Type_Motor_Speed.Fine : Type_Motor_Speed.Coarse;


        public FormNewSub_JogPopup_Loader(FormNew_JogPopup parent)
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;

            m_Parent = parent;
            this.Load += FormNewSub_JogPopup_Loader_Load; // 여기서 Load 이벤트 연결


        }

        private void FormNewSub_JogPopup_Loader_Load(object sender, EventArgs e)
        {
            if (m_bInitialized)
                return;

            //Size 축소 / 확대 안되게 하기 위한 코드.
            //this.AutoScaleMode = AutoScaleMode.None;
            //this.DoubleBuffered = true;
            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            //this.UpdateStyles();

            

            //GUI생성 완료 후 Data 및 Cintroller 업데이트!
            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;
            foreach (Module module in m_collectionModules)
            {
                if (module.Name == "Loader")
                {
                    loader = module as Loader;
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
            // Initialize the jog buttons here if needed
            button_JogPopup_Loader_TransferZ_Pos.Tag = "Loader,TR_Z,1";
            button_JogPopup_Loader_TransferZ_Neg.Tag = "Loader,TR_Z,-1";
            button_JogPopup_Loader_TransferX_Pos.Tag = "Loader,TR_X,1";
            button_JogPopup_Loader_TransferX_Neg.Tag = "Loader,TR_X,-1";

            button_JogPopup_Loader_PortZ0_Pos.Tag = "Loader,Z0,1";
            button_JogPopup_Loader_PortZ0_Neg.Tag = "Loader,Z0,-1";
            button_JogPopup_Loader_PortZ1_Pos.Tag = "Loader,Z1,1";
            button_JogPopup_Loader_PortZ1_Neg.Tag = "Loader,Z1,-1";

            button_JogPopup_Loader_MA_X_Pos.Tag = "Loader,ALN_X,1";
            button_JogPopup_Loader_MA_X_Neg.Tag = "Loader,ALN_X,-1";
            button_JogPopup_Loader_MA_Y_Pos.Tag = "Loader,ALN_Y,1";
            button_JogPopup_Loader_MA_Y_Neg.Tag = "Loader,ALN_Y,-1";

            ConnectJogEvents(m_Parent);
        }

        // Form의 인스턴스를 받아서 델리게이트 연결
        private void ConnectJogEvents(FormNew_JogPopup parent)
        {
            button_JogPopup_Loader_TransferZ_Pos.MouseDown += parent.button_AxisJog_MouseDown;
            button_JogPopup_Loader_TransferZ_Pos.MouseUp += parent.button_AxisJog_MouseUp;
            button_JogPopup_Loader_TransferZ_Neg.MouseDown += parent.button_AxisJog_MouseDown;
            button_JogPopup_Loader_TransferZ_Neg.MouseUp += parent.button_AxisJog_MouseUp;
            button_JogPopup_Loader_TransferX_Pos.MouseDown += parent.button_AxisJog_MouseDown;
            button_JogPopup_Loader_TransferX_Pos.MouseUp += parent.button_AxisJog_MouseUp;
            button_JogPopup_Loader_TransferX_Neg.MouseDown += parent.button_AxisJog_MouseDown;
            button_JogPopup_Loader_TransferX_Neg.MouseUp += parent.button_AxisJog_MouseUp;
            button_JogPopup_Loader_PortZ0_Pos.MouseDown += parent.button_AxisJog_MouseDown;
            button_JogPopup_Loader_PortZ0_Pos.MouseUp += parent.button_AxisJog_MouseUp;
            button_JogPopup_Loader_PortZ1_Pos.MouseDown += parent.button_AxisJog_MouseDown;
            button_JogPopup_Loader_PortZ1_Pos.MouseUp += parent.button_AxisJog_MouseUp;
            button_JogPopup_Loader_PortZ0_Neg.MouseDown += parent.button_AxisJog_MouseDown;
            button_JogPopup_Loader_PortZ0_Neg.MouseUp += parent.button_AxisJog_MouseUp;
            button_JogPopup_Loader_PortZ1_Neg.MouseDown += parent.button_AxisJog_MouseDown;
            button_JogPopup_Loader_PortZ1_Neg.MouseUp += parent.button_AxisJog_MouseUp;
            button_JogPopup_Loader_MA_X_Pos.MouseDown += parent.button_AxisJog_MouseDown;
            button_JogPopup_Loader_MA_X_Pos.MouseUp += parent.button_AxisJog_MouseUp;
            button_JogPopup_Loader_MA_X_Neg.MouseDown += parent.button_AxisJog_MouseDown;
            button_JogPopup_Loader_MA_X_Neg.MouseUp += parent.button_AxisJog_MouseUp;
            button_JogPopup_Loader_MA_Y_Pos.MouseDown += parent.button_AxisJog_MouseDown;
            button_JogPopup_Loader_MA_Y_Pos.MouseUp += parent.button_AxisJog_MouseUp;
            button_JogPopup_Loader_MA_Y_Neg.MouseDown += parent.button_AxisJog_MouseDown;
            button_JogPopup_Loader_MA_Y_Neg.MouseUp += parent.button_AxisJog_MouseUp;

        }

        private Dictionary<Loader.nAxis, Label> loaderAxisLabelMap;
        private void InitAxisLabelMap()
        {
            loaderAxisLabelMap = new Dictionary<Loader.nAxis, Label>
            {
                { Loader.nAxis.Z0, label_JogPopup_EncPosition_LD_Z0 },
                { Loader.nAxis.Z1, label_JogPopup_EncPosition_LD_Z1 },
                { Loader.nAxis.TR_X, label_JogPopup_EncPosition_LD_TRX },
                { Loader.nAxis.TR_Z, label_JogPopup_EncPosition_LD_TRZ },
                { Loader.nAxis.ALN_X, label_JogPopup_EncPosition_LD_ALNX },
                { Loader.nAxis.ALN_Y, label_JogPopup_EncPosition_LD_ALNY },
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

            foreach (var pair in loaderAxisLabelMap)
            {
                double pos = loader.GetEncLoaderPos_Motor(pair.Key);

                SetValue(pair.Value, FormatPos(pos));
            }
        }

        void SetValue(Label control, string text, bool isVisible = true)
        {
            if (control.InvokeRequired)
            {
                this.Invoke(new System.Action(() =>
                {
                    //화면에 출력.
                    SetValue(control, text, isVisible);
                }));

            }
            else
            {
                control.Text = text;
                control.Visible = isVisible;
            }
        }
    }
}