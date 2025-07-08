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
using static QMC.Common.Equipment;
using static SLD200.NewStyleForm.FormNew_JogPopup;
using SLD200_MSL;

namespace SLD200.NewStyleForm.NewSubForm
{
    public partial class FormNewSub_JogPopup_Stage : UserControl, IJogControlProvider
    {
        private bool m_bFormVisible = false; // 실제 Show 상태 여부
        private bool m_bInitialized = false;
        private FormNew_JogPopup m_Parent;
        static WorkStage workStage;

        public bool IsJogMoveContinuous => radioButton_jogPopup_Stage_JogMove_Continuous.Checked;
        public bool IsJogMoveStep => radioButton_jogPopup_Stage_JogMove_Step.Checked;
        public double JogStepDistance => Equipment.ToDouble(textBox_jogPopup_Stage_JogMove_StepSize.Text);
        public Type_Motor_Speed JogSpeedType =>
            radioButton_jogPopup_Stage_Move_MoveMode_Fine.Checked ? Type_Motor_Speed.Fine : Type_Motor_Speed.Coarse;

        public FormNewSub_JogPopup_Stage(FormNew_JogPopup parent)
        {
            InitializeComponent();
            m_Parent = parent;
            this.AutoScaleMode = AutoScaleMode.None;

            this.Load += FormNewSub_JogPopup_Stage_Load; // 여기서 Load 이벤트 연결

        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // 엔터 또는 스페이스 키 눌렀을 때 무시
            if (keyData == Keys.Enter || keyData == Keys.Space)
                return true;

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void FormNewSub_JogPopup_Stage_Load(object sender, EventArgs e)
        {
            if(m_bInitialized)
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
                if (module.Name == "WorkStage")
                {
                    workStage = module as WorkStage;
                }
            }

            InitializeJogButtons();
            InitAxisLabelMap();

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
            button_jogPopup_Stage_StageX_Pos.Tag = "WorkStage,X,1";
            button_jogPopup_Stage_StageX_Neg.Tag = "WorkStage,X,-1";
            button_jogPopup_Stage_StageY_Pos.Tag = "WorkStage,Y,1";
            button_jogPopup_Stage_StageY_Neg.Tag = "WorkStage,Y,-1";
            button_jogPopup_Stage_VisionZ_Pos.Tag = "WorkStage,Z,1";
            button_jogPopup_Stage_VisionZ_Neg.Tag = "WorkStage,Z,-1";

            button_jogPopup_Stage_Mask_Y_Pos.Tag = "WorkStage,MASK_Y,1";
            button_jogPopup_Stage_Mask_Y_Neg.Tag = "WorkStage,MASK_Y,-1";

            ConnectJogEvents(m_Parent);
        }

        // Form의 인스턴스를 받아서 델리게이트 연결
        private void ConnectJogEvents(FormNew_JogPopup parent)
        {
            button_jogPopup_Stage_StageX_Pos.MouseDown += parent.button_AxisJog_MouseDown;
            button_jogPopup_Stage_StageX_Neg.MouseDown += parent.button_AxisJog_MouseDown;
            button_jogPopup_Stage_StageY_Pos.MouseDown += parent.button_AxisJog_MouseDown;
            button_jogPopup_Stage_StageY_Neg.MouseDown += parent.button_AxisJog_MouseDown;
            button_jogPopup_Stage_VisionZ_Pos.MouseDown += parent.button_AxisJog_MouseDown;
            button_jogPopup_Stage_VisionZ_Neg.MouseDown += parent.button_AxisJog_MouseDown;
            button_jogPopup_Stage_Mask_Y_Pos.MouseDown += parent.button_AxisJog_MouseDown;
            button_jogPopup_Stage_Mask_Y_Neg.MouseDown += parent.button_AxisJog_MouseDown;
            button_jogPopup_Stage_StageX_Pos.MouseUp += parent.button_AxisJog_MouseUp;
            button_jogPopup_Stage_StageX_Neg.MouseUp += parent.button_AxisJog_MouseUp;
            button_jogPopup_Stage_StageY_Pos.MouseUp += parent.button_AxisJog_MouseUp;
            button_jogPopup_Stage_StageY_Neg.MouseUp += parent.button_AxisJog_MouseUp;
            button_jogPopup_Stage_VisionZ_Pos.MouseUp += parent.button_AxisJog_MouseUp;
            button_jogPopup_Stage_VisionZ_Neg.MouseUp += parent.button_AxisJog_MouseUp;
            button_jogPopup_Stage_Mask_Y_Pos.MouseUp += parent.button_AxisJog_MouseUp;
            button_jogPopup_Stage_Mask_Y_Neg.MouseUp += parent.button_AxisJog_MouseUp;

        }

        private Dictionary<WorkStage.nAxis, Label> workstageAxisLabelMap;
        private void InitAxisLabelMap()
        {
            workstageAxisLabelMap = new Dictionary<WorkStage.nAxis, Label>
            {
                { WorkStage.nAxis.X, label_jogPopup_Stage_EncPosition_STAGE_X },
                { WorkStage.nAxis.Y, label_jogPopup_Stage_EncPosition_STAGE_Y },
                { WorkStage.nAxis.Z, label_jogPopup_Stage_EncPosition_SCANNER_Z },
                { WorkStage.nAxis.MASK_Y, label_jogPopup_Stage_EncPosition_MASK_Y }
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

            foreach (var pair in workstageAxisLabelMap)
            {
                double pos = workStage.GetEncWorkStagePos_Motor(pair.Key);
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

        // 이거 각각 폼에 만들어야함.
        private void InitRecipeUI_KeyPad()
        {
            RegisterKeyPadDoubleClickHandlers(this); // 폼 전체에 대해 수행
        }
        private void RegisterKeyPadDoubleClickHandlers(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                // 조건: 숫자 입력용 TextBox 또는 RichTextBox만
                bool isTargetTextBox = ctrl is TextBox || ctrl is RichTextBox;

                if (isTargetTextBox && ctrl.Tag?.ToString().Contains("KeyPad") == true)
                {
                    ctrl.DoubleClick -= textBox_DoubleClick_OpenKeyPad; // 중복 연결 방지
                    ctrl.DoubleClick += textBox_DoubleClick_OpenKeyPad;

                    // 키보드 입력 제한용 Validating 연결
                    ctrl.Validating -= textBox_Validate_KeyPadRange;
                    ctrl.Validating += textBox_Validate_KeyPadRange;
                }

                // 하위 컨트롤 재귀 탐색
                if (ctrl.HasChildren)
                    RegisterKeyPadDoubleClickHandlers(ctrl);
            }
        }
        private void textBox_DoubleClick_OpenKeyPad(object sender, EventArgs e)
        {
            if (sender is Control ctrl)
            {
                string currentText = ctrl.Text ?? "0";
                var dlg = new FormNew_KeyPad();

                if (double.TryParse(currentText, out double value))
                    dlg.SetInitialValue(value);
                else
                    dlg.SetInitialValue(0);

                // Tag 파싱
                var meta = KeyPadMeta.ParseFromTag(ctrl.Tag?.ToString());

                dlg.MinValue = meta.Min;
                dlg.MaxValue = meta.Max;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    string result = dlg.EnteredValue.ToString(meta.Format);
                    ctrl.Text = result;
                }
            }
        }
        private void textBox_Validate_KeyPadRange(object sender, CancelEventArgs e)
        {
            if (sender is TextBox tb && tb.Tag != null)
            {
                var meta = KeyPadMeta.ParseFromTag(tb.Tag.ToString());

                if (double.TryParse(tb.Text, out double val))
                {
                    if (val < meta.Min)
                    {
                        tb.Text = meta.Min.ToString(meta.Format);
                        //MessageBox.Show($"최소값 {meta.Min}보다 작습니다."); // 또는 자동 보정만
                    }
                    else if (val > meta.Max)
                    {
                        tb.Text = meta.Max.ToString(meta.Format);
                        //MessageBox.Show($"최대값 {meta.Max}보다 큽니다.");
                    }
                    else
                    {
                        tb.Text = val.ToString(meta.Format);
                    }
                }
                else
                {
                    // 숫자 아님 → 초기화
                    tb.Text = meta.Min.ToString(meta.Format);
                }
            }
        }
    }
}
