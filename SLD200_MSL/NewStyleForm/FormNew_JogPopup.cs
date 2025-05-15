using QMC.Common;
using QMC.Common.Modules;
using QMC.Common.Parts;
using QMC.Common.VisionPart;
using SLD200.NewStyleForm.NewSubForm;
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

namespace SLD200.NewStyleForm
{
    public partial class FormNew_JogPopup : Form
    {
        private bool m_bFormVisible = false; // 실제 Show 상태 여부
        private bool m_bInitialized = false;

        private System.Windows.Forms.Timer timer_Status;

        Loader      loader;
        WorkStage   workStage;
        Unloader    unloader;

        private FormNewSub_JogPopup_Loader userform_Loader;
        private FormNewSub_JogPopup_Stage userform_Stage;
        private FormNewSub_JogPopup_Unloader userform_Unloader;

        public FormNew_JogPopup()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;

            this.Load += FormNewSub_JogPopup_Load; // 여기서 Load 이벤트 연결
            this.FormClosing += FormNew_JogPopup_FormClosing; // 추가
            this.tabControl_JogPopup.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
        }

        private void FormNew_JogPopup_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true; // 폼이 종료되지 않도록 방지
                OnHide();
                this.Hide();     // 대신 숨긴다
            }
        }

        private void FormNewSub_JogPopup_Load(object sender, EventArgs e)
        {
            if (m_bInitialized)
                return;

            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;
            foreach (Module module in m_collectionModules)
            {
                //if (module.Name == "WorkStage")
                if (module.Name == "WorkStage")
                {
                    workStage = module as WorkStage;
                }

                if (module.Name == "Loader")
                {
                    loader = module as Loader;
                }

                if (module.Name == "Unloader")
                {
                    unloader = module as Unloader;
                }
            }

            InitializeTabs();

            // 타이머 초기화
            timer_Status = new System.Windows.Forms.Timer();
            timer_Status.Interval = 200; // 200ms 주기
            timer_Status.Tick += Timer_Status_Tick;
            timer_Status.Start();

            m_bInitialized = true;
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (!this.Created)
                return;

            if (this.Visible && !m_bFormVisible)
            {
                m_bFormVisible = true;
                OnShow();
                timer_Status.Start();
            }
            else if (!this.Visible && m_bFormVisible)
            {
                m_bFormVisible = false;
                OnHide();
                timer_Status.Stop();
            }
        }

        private void OnShow()
        {
            tabPage_Loader.Select();
            userform_Loader.OnShow();

            //if (userform_Loader != null)
            //    userform_Loader.OnShow();
            //if (userform_Stage != null)
            //    userform_Stage.OnShow();
            //if (userform_Unloader != null)
            //    userform_Unloader.OnShow();
        }
        private void OnHide()
        {
            if (userform_Loader != null)
                userform_Loader.OnHide();
            if (userform_Stage != null)
                userform_Stage.OnHide();
            if (userform_Unloader != null)
                userform_Unloader.OnHide();
        }

        private void Timer_Status_Tick(object sender, EventArgs e)
        {
            try
            {
                // 타이머 중복 호출 방지
                timer_Status.Enabled = false;

                // 현재 선택된 탭에 따라 해당 UserControl의 상태만 업데이트
                if (tabControl_JogPopup.SelectedTab == tabPage_Loader)
                {
                    userform_Loader?.UpdateStatus();
                }
                else if (tabControl_JogPopup.SelectedTab == tabPage_Stage)
                {
                    userform_Stage?.UpdateStatus();
                }
                else if (tabControl_JogPopup.SelectedTab == tabPage_Unloader)
                {
                    userform_Unloader?.UpdateStatus();
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            finally
            {
                timer_Status.Enabled = true;
            }
        }

        private void InitializeTabs()
        {
            userform_Loader = new FormNewSub_JogPopup_Loader(this);
            userform_Stage = new FormNewSub_JogPopup_Stage(this);
            userform_Unloader = new FormNewSub_JogPopup_Unloader(this);

            userform_Loader.Dock = DockStyle.Fill;
            userform_Stage.Dock = DockStyle.Fill;
            userform_Unloader.Dock = DockStyle.Fill;
            userform_Loader.AutoSize = false;
            userform_Stage.AutoSize = false;
            userform_Unloader.AutoSize = false;

            tabPage_Loader.Controls.Add(userform_Loader);
            tabPage_Stage.Controls.Add(userform_Stage);
            tabPage_Unloader.Controls.Add(userform_Unloader);

            
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            userform_Loader.OnHide();
            userform_Stage.OnHide();
            userform_Unloader.OnHide();
            switch (tabControl_JogPopup.SelectedTab.Name)
            {
                case "tabPage_Loader":
                    userform_Loader.OnShow();
                    break;
                case "tabPage_Stage":
                    userform_Stage.OnShow();
                    break;
                case "tabPage_Unloader":
                    userform_Unloader.OnShow();
                    break;
            }
        }

        public interface IJogControlProvider
        {
            bool IsJogMoveContinuous { get; }
            bool IsJogMoveStep { get; }
            double JogStepDistance { get; }
            Type_Motor_Speed JogSpeedType { get; }
        }

        // 축 JogMove 공통 처리
        public void button_AxisJog_MouseDown(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null || btn.Tag == null)
                return;

            if (Equipment.AjinBoard_Opened)
            {
                string[] tagParts = btn.Tag?.ToString()?.Split(',');
                if (tagParts == null || tagParts.Length != 3)
                    return;

                string unit = tagParts[0];         // 예: "WorkStage", "Loader"
                string axisName = tagParts[1];     // 예: "X", "Y", "Z0", "TR_X"
                double direction = Convert.ToDouble(tagParts[2]); // -1.0 또는 1.0

                dynamic controller = GetControllerByUnit(unit);
                if (controller == null) return;

                int axis = GetAxisIndex(ref unit, axisName);
                if (axis < 0) return;

                Type_Motor_Speed speedType = GetMotorSpeedType(unit);

                if (IsJogMoveContinuous(unit))
                {
                    switch (unit)
                    {
                        case "WorkStage": workStage.MovetoWorkStage_Jog_Positions((WorkStage.nAxis)axis, (int)direction, speedType); break;
                        case "Loader": loader.MovetoLoader_Jog_Positions((Loader.nAxis)axis, (int)direction, speedType); break;
                        case "Unloader": unloader.MovetoUnloader_Jog_Positions((Unloader.nAxis)axis, (int)direction, speedType); break;
                    }
                }
                else if (IsJogMoveStep(unit))
                {
                    double distance = GetStepDistance(unit);
                    switch (unit)
                    {
                        case "WorkStage": workStage.MovetoWorkStage_Rel_Positions((WorkStage.nAxis)axis, distance, (int)direction, speedType); break;
                        case "Loader": loader.MovetoLoader_Rel_Positions((Loader.nAxis)axis, distance, (int)direction, speedType); break;
                        case "Unloader": unloader.MovetoUnloader_Rel_Positions((Unloader.nAxis)axis, distance, (int)direction, speedType); break;
                    }
                    //controller.MovetoWorkStage_Rel_Positions((WorkStage.nAxis)axis, distance, (int)direction, speedType);
                }
            }
        }

        // 축 JogStop 공통 처리
        public void button_AxisJog_MouseUp(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null || btn.Tag == null)
                return;
            if (Equipment.AjinBoard_Opened)
            {
                string[] tagParts = btn.Tag?.ToString()?.Split(',');
                if (tagParts == null || tagParts.Length < 2)
                    return;

                string unit = tagParts[0];         // 예: "loader"
                string axisName = tagParts[1];     // 예: "X"

                dynamic controller = GetControllerByUnit(unit);
                if (controller == null) return;

                int axis = GetAxisIndex(ref unit, axisName);
                if (axis < 0) return;

                switch (unit)
                {
                    case "WorkStage": workStage.MC_Func.MC_JogStop(axis); break;
                    case "Loader": loader.MC_Func.MC_JogStop(axis); break;
                    case "Unloader": unloader.MC_Func.MC_JogStop(axis); break;
                }
                //controller.MC_Func.MC_JogStop(axis);
            }
        }

        // 유닛별 컨트롤러 반환
        private object GetControllerByUnit(string unit)
        {
            if (unit == "WorkStage")
                return workStage;
            else if (unit == "Loader")
                return loader;
            else if (unit == "Unloader")
                return unloader;
            else
                return null;
        }

        // 유닛 및 축 이름으로 축 인덱스 반환
        private int GetAxisIndex(ref string unit, string axisName)
        {
            try
            {
                if (unit == "WorkStage")
                    return (int)Enum.Parse(typeof(WorkStage.nAxis), axisName);
                else if (unit == "Loader")
                    return (int)Enum.Parse(typeof(Loader.nAxis), axisName);
                else if (unit == "Unloader")
                    return (int)Enum.Parse(typeof(Unloader.nAxis), axisName);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }

            return -1;
        }

        private IJogControlProvider GetJogProviderByUnit(string unit)
        {
            if (unit == "Loader")
                return userform_Loader;
            else if (unit == "WorkStage")
                return userform_Stage;
            else if (unit == "Unloader")
                return userform_Unloader;
            return null;
        }

        private Type_Motor_Speed GetMotorSpeedType(string unit)
        {
            return GetJogProviderByUnit(unit)?.JogSpeedType ?? Type_Motor_Speed.Fine;
            
            // 필요 시 다른 유닛 추가
            return Type_Motor_Speed.Fine;
        }

        private bool IsJogMoveContinuous(string unit)
        {
            return GetJogProviderByUnit(unit)?.IsJogMoveContinuous ?? false;
        }

        private bool IsJogMoveStep(string unit)
        {
            return GetJogProviderByUnit(unit)?.IsJogMoveStep ?? false;
        }

        private double GetStepDistance(string unit)
        {
            return GetJogProviderByUnit(unit)?.JogStepDistance ?? 0.0;
        }
    }
}
