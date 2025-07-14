using QMC.Common;
using QMC.Common.Modules;
using QMC.Common.Parts;
using QMC.Core;
using SpiralLab.Sirius;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QMC.Common.Modules.Vision;
using static QMC.Common.Modules.WorkStage;

namespace SLD200.NewStyleForm.NewSubForm
{
    public partial class FormNewSub_Main_MotorMove : UserControl
    {
        static WorkStage workStage;
        static Loader loader;
        static Unloader unloader;
        static Vision vision;
        static Bds bds;

        private System.Windows.Forms.Timer timerMotorMove;
        private bool _isRunning_MotorMove = false;

        private Dictionary<Control, Color> _originalBackColors = new Dictionary<Control, Color>();

        public FormNewSub_Main_MotorMove()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.DoubleBuffered = true;

            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;
            foreach (Module module in m_collectionModules)
            {
                if (module.Name == "WorkStage") workStage = module as WorkStage;
                if (module.Name == "Loader") loader = module as Loader;
                if (module.Name == "Unloader") unloader = module as Unloader;
                if (module.Name == "Vision") vision = module as Vision;
                if (module.Name == "BDS") bds = module as Bds;
            }

            timerMotorMove = new System.Windows.Forms.Timer();
            timerMotorMove.Interval = 200;
            timerMotorMove.Tick += TimerMotorMove_Tick;
            timerMotorMove.Start();

            // 초기 색상 저장
            _originalBackColors[button_MotorMove_Stage_Vacuum] = button_MotorMove_Stage_Vacuum.BackColor;
            _originalBackColors[button_MotorMove_Loader_MAlign_Vacuum] = button_MotorMove_Loader_MAlign_Vacuum.BackColor;
            _originalBackColors[button_MotorMove_Loader_Stacker_Vacuum] = button_MotorMove_Loader_Stacker_Vacuum.BackColor;
            _originalBackColors[button_MotorMove_Stage_ScannerFineCam] = button_MotorMove_Stage_ScannerFineCam.BackColor;
            _originalBackColors[button_MotorMove_Unloader_Vacuum] = button_MotorMove_Unloader_Vacuum.BackColor;//
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // 엔터 또는 스페이스 키 눌렀을 때 무시
            if (keyData == Keys.Enter || keyData == Keys.Space)
                return true;

            return base.ProcessCmdKey(ref msg, keyData);
        }

        //protected override void OnFormClosing(FormClosingEventArgs e)
        //{
        //    if (e.CloseReason == CloseReason.UserClosing)
        //    {
        //        // 사용자가 닫기(X 버튼) 누른 경우 → 숨기기만 하고 종료 안 함
        //        e.Cancel = true;
        //        this.Hide();
        //        return;
        //    }

        //    // 그 외 종료 (Application.Exit 등) → 정식 해제
        //    base.OnFormClosing(e);
        //}

        public void DisposeSemiAutoResources()
        {
            if (timerMotorMove != null)
            {
                timerMotorMove.Stop();
                timerMotorMove.Tick -= TimerMotorMove_Tick;
                timerMotorMove.Dispose();
                timerMotorMove = null;
            }

            // 필요 시 다른 모듈 정리도 여기에
        }

        private void TimerMotorMove_Tick(object sender, EventArgs e)
        {
            if (_isRunning_MotorMove)
                return;

            try
            {
                _isRunning_MotorMove = true;
                Timer_SemiAutoRun();
            }
            catch (Exception ex)
            {
                // 로그 남기기
                Log.Write(ex);
                _isRunning_MotorMove = false;
            }
            finally
            {
                _isRunning_MotorMove = false;
            }
        }

        private void Timer_SemiAutoRun()
        {
            // 실행할 작업들을 여기에 구현.

            if(Equipment.AutoRunStatus || Equipment.SelectRunEnable_New || Equipment.SelectRunEnable)
            {
                button_MotorMove_Unloader_ToStacker.Enabled = false;
                button_MotorMove_Unloader_ToStage.Enabled = false;
                button_MotorMove_Unloader_Vacuum.Enabled = false;
                button_MotorMove_Stage_ScannerCenter.Enabled = false;
                button_MotorMove_Stage_ScannerFineCam.Enabled = false;
                button_MotorMove_Stage_FineCamScanner.Enabled = false;
                button_MotorMove_Stage_FineCamHeightSensor.Enabled = false;
                button_MotorMove_Stage_HeightSensorFineCam.Enabled = false;
                button_MotorMove_Stage_FineCamCoarseCam.Enabled = false;
                button_MotorMove_Stage_CoarseCamFineCam.Enabled = false;
                button_MotorMove_Stage_ToUnloading.Enabled = false;
                button_MotorMove_Stage_ToLoading.Enabled = false;
                button_MotorMove_Stage_Vacuum.Enabled = false;
                button_MotorMove_Loader_ToStage.Enabled = false;
                button_MotorMove_Loader_ToMAlign.Enabled = false;
                button_MotorMove_Loader_ToStacker.Enabled = false;
                button_MotorMove_Loader_MAlign_Vacuum.Enabled = false;
                button_MotorMove_Loader_Stacker_Vacuum.Enabled = false;
                button_MotorMove_Unloader_ToSafetyZ.Enabled = false;
                button_MotorMove_Loader_ToSafetyZ.Enabled = false;
            }
            else
            {
                button_MotorMove_Unloader_ToStacker.Enabled = true;
                button_MotorMove_Unloader_ToStage.Enabled = true;
                button_MotorMove_Unloader_Vacuum.Enabled = true;
                button_MotorMove_Stage_ScannerCenter.Enabled = true;
                button_MotorMove_Stage_ScannerFineCam.Enabled = true;
                button_MotorMove_Stage_FineCamScanner.Enabled = true;
                button_MotorMove_Stage_FineCamHeightSensor.Enabled = true;
                button_MotorMove_Stage_HeightSensorFineCam.Enabled = true;
                button_MotorMove_Stage_FineCamCoarseCam.Enabled = true;
                button_MotorMove_Stage_CoarseCamFineCam.Enabled = true;
                button_MotorMove_Stage_ToUnloading.Enabled = true;
                button_MotorMove_Stage_ToLoading.Enabled = true;
                button_MotorMove_Stage_Vacuum.Enabled = true;
                button_MotorMove_Loader_ToStage.Enabled = true;
                button_MotorMove_Loader_ToMAlign.Enabled = true;
                button_MotorMove_Loader_ToStacker.Enabled = true;
                button_MotorMove_Loader_MAlign_Vacuum.Enabled = true;
                button_MotorMove_Loader_Stacker_Vacuum.Enabled = true;
                button_MotorMove_Unloader_ToSafetyZ.Enabled = true;
                button_MotorMove_Loader_ToSafetyZ.Enabled = true;
            }

                //  Laser Height Sensor
                double? dHeightVal = workStage.m_dLaserHeightSensorSocket_Value;
            label_MotorMove_heightSensor.Text = string.Format("{0:0.0000}", dHeightVal.HasValue ? dHeightVal.Value : 0.0f);

            //button_MotorMove_Stage_Vacuum
            bool bStageVac = workStage.workStageParameter.DI_Stage_Vacuum_Check();
            button_MotorMove_Stage_Vacuum.Text = $"Vacuum {(bStageVac ? "ON" : "OFF")}";
            SetColor(button_MotorMove_Stage_Vacuum,
                    bStageVac ? Color.LightGreen : _originalBackColors[button_MotorMove_Stage_Vacuum],
                    Color.Black);

            // MAlign Vacuum 상태 표시
            bool bMAlignVac = loader.loaderParameter.DI_Loader_Aligner_VacuumCheck((int)LoaderParameter.MAlignerVacuumPos.Center);
            button_MotorMove_Loader_MAlign_Vacuum.Text = $"Vacuum {(bMAlignVac ? "ON" : "OFF")}";
            SetColor(button_MotorMove_Loader_MAlign_Vacuum,
                     bMAlignVac ? Color.LightGreen : _originalBackColors[button_MotorMove_Loader_MAlign_Vacuum],
                     Color.Black);

            // loader Vacuum 상태 표시
            bool bloaderVac = loader.loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Inner);
            button_MotorMove_Loader_Stacker_Vacuum.Text = $"Vacuum {(bloaderVac ? "ON" : "OFF")}";
            SetColor(button_MotorMove_Loader_Stacker_Vacuum,
                     bloaderVac ? Color.LightGreen : _originalBackColors[button_MotorMove_Loader_Stacker_Vacuum],
                     Color.Black);

            // unloader Vacuum 상태 표시
            bool bunloaderVac = unloader.unloaderParameter.DI_Unloader_Picker_VacuumCheck((int)UnloaderParameter.PickerVacuumPos.Inner);
            button_MotorMove_Unloader_Vacuum.Text = $"Vacuum {(bunloaderVac ? "ON" : "OFF")}";
            SetColor(button_MotorMove_Unloader_Vacuum,
                     bunloaderVac ? Color.LightGreen : _originalBackColors[button_MotorMove_Unloader_Vacuum],
                     Color.Black);
        }

        private void button_MotorMove_Loader_ToStage_Click(object sender, EventArgs e)
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "위치로 보내시겠습니까?"))
                return;

            Log.Write("GUI", Equipment.User_Name, "ButtonClick", "button_MotorMove_Loader_ToStage_Click");

            if (!loader.MC_Func.MC_GetDone((int)Loader.nAxis.TR_X) ||
                !loader.MC_Func.MC_GetDone((int)Loader.nAxis.TR_Z) ||
                !loader.MC_Func.MC_GetInposition((int)Loader.nAxis.TR_X) ||
                !loader.MC_Func.MC_GetInposition((int)Loader.nAxis.TR_Z))
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Loader Transfer가 이동중입니다.");
                return;
            }

            Equipment.Type_Motor_Speed motor_Speed;
            motor_Speed = Equipment.Type_Motor_Speed.Coarse;
            int nTeachingPosIndex = (int)Loader.LDUL_TeachingPosList.LD_TR_WorkTablePos;
            loader.MovetoLoader_TeachingPositionsTransferX(nTeachingPosIndex, motor_Speed, true);
        }

        private void button_MotorMove_Loader_ToMAlign_Click(object sender, EventArgs e)
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "위치로 보내시겠습니까?"))
                return;

            Log.Write("GUI", Equipment.User_Name, "ButtonClick", "button_MotorMove_Loader_ToMAlign_Click");

            if (!loader.MC_Func.MC_GetDone((int)Loader.nAxis.TR_X) ||
                !loader.MC_Func.MC_GetDone((int)Loader.nAxis.TR_Z) ||
                !loader.MC_Func.MC_GetInposition((int)Loader.nAxis.TR_X) ||
                !loader.MC_Func.MC_GetInposition((int)Loader.nAxis.TR_Z))
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Loader Transfer가 이동중입니다.");
                return;
            }

            Equipment.Type_Motor_Speed motor_Speed;
            motor_Speed = Equipment.Type_Motor_Speed.Coarse;
            int nTeachingPosIndex = (int)Loader.LDUL_TeachingPosList.LD_TR_MAlignPos;
            loader.MovetoLoader_TeachingPositionsTransferX(nTeachingPosIndex, motor_Speed, true);
        }

        private void button_MotorMove_Loader_ToStacker_Click(object sender, EventArgs e)
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "위치로 보내시겠습니까?"))
                return;

            Log.Write("GUI", Equipment.User_Name, "ButtonClick", "button_MotorMove_Loader_ToStacker_Click");

            if (!loader.MC_Func.MC_GetDone((int)Loader.nAxis.TR_X) ||
                !loader.MC_Func.MC_GetDone((int)Loader.nAxis.TR_Z) ||
                !loader.MC_Func.MC_GetInposition((int)Loader.nAxis.TR_X) ||
                !loader.MC_Func.MC_GetInposition((int)Loader.nAxis.TR_Z))
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Loader Transfer가 이동중입니다.");
                return;
            }

            Equipment.Type_Motor_Speed motor_Speed;
            motor_Speed = Equipment.Type_Motor_Speed.Coarse;
            int nTeachingPosIndex = (int)Loader.LDUL_TeachingPosList.LD_TR_LPortPos;
            loader.MovetoLoader_TeachingPositionsTransferX(nTeachingPosIndex, motor_Speed, true);
        }

        private void button_MotorMove_Loader_MAlign_Vacuum_Click(object sender, EventArgs e)
        {
            Log.Write("GUI", Equipment.User_Name, "ButtonClick", "button_MotorMove_Loader_MAlign_Vacuum_Click");

            if (Equipment.stLayerRecipeSet[0].ModuleInformation_Module_Width < workStage.m_pProcessConfigData.dModuleSizeSet ||
                Equipment.stLayerRecipeSet[0].ModuleInformation_Module_Height < workStage.m_pProcessConfigData.dModuleSizeSet)
            {
                if (loader.loaderParameter.DI_Loader_Aligner_VacuumCheck((int)LoaderParameter.MAlignerVacuumPos.Center) ||
                    loader.loaderParameter.DI_Loader_Aligner_VacuumCheck((int)LoaderParameter.MAlignerVacuumPos.Inner))
                {
                    loader.loaderParameter.DO_Loader_Aligner_Vacuum((int)LoaderParameter.MAlignerVacuumPos.Center, false);
                    loader.loaderParameter.DO_Loader_Aligner_Vacuum((int)LoaderParameter.MAlignerVacuumPos.Inner, false);
                }
                else
                {
                    loader.loaderParameter.DO_Loader_Aligner_Vacuum((int)LoaderParameter.MAlignerVacuumPos.Center, true);
                    loader.loaderParameter.DO_Loader_Picker_Vacuum((int)LoaderParameter.PickerVacuumPos.Inner, true);
                }
            }
            else
            {
                if (loader.loaderParameter.DI_Loader_Aligner_VacuumCheck((int)LoaderParameter.MAlignerVacuumPos.Center) ||
                    loader.loaderParameter.DI_Loader_Aligner_VacuumCheck((int)LoaderParameter.MAlignerVacuumPos.Inner) ||
                    loader.loaderParameter.DI_Loader_Aligner_VacuumCheck((int)LoaderParameter.MAlignerVacuumPos.Outer))
                {
                    loader.loaderParameter.DO_Loader_Aligner_Vacuum((int)LoaderParameter.MAlignerVacuumPos.Center, false);
                    loader.loaderParameter.DO_Loader_Aligner_Vacuum((int)LoaderParameter.MAlignerVacuumPos.Inner, false);
                    loader.loaderParameter.DO_Loader_Aligner_Vacuum((int)LoaderParameter.MAlignerVacuumPos.Outer, false);
                }
                else
                {
                    loader.loaderParameter.DO_Loader_Aligner_Vacuum((int)LoaderParameter.MAlignerVacuumPos.Center, true);
                    loader.loaderParameter.DO_Loader_Aligner_Vacuum((int)LoaderParameter.MAlignerVacuumPos.Inner, true);
                    loader.loaderParameter.DO_Loader_Aligner_Vacuum((int)LoaderParameter.MAlignerVacuumPos.Outer, true);
                }
            }
        }

        private void button_MotorMove_Loader_Stacker_Vacuum_Click(object sender, EventArgs e)
        {
            Log.Write("GUI", Equipment.User_Name, "ButtonClick", "button_MotorMove_Loader_Stacker_Vacuum_Click");

            if (Equipment.stLayerRecipeSet[0].ModuleInformation_Module_Width < workStage.m_pProcessConfigData.dModuleSizeSet ||
                Equipment.stLayerRecipeSet[0].ModuleInformation_Module_Height < workStage.m_pProcessConfigData.dModuleSizeSet)
            {
                if (loader.loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Inner))
                {
                    loader.loaderParameter.DO_Loader_Picker_Vacuum((int)LoaderParameter.PickerVacuumPos.Inner, false);
                }
                else
                {
                    loader.loaderParameter.DO_Loader_Picker_Vacuum((int)LoaderParameter.PickerVacuumPos.Inner, true);
                }
            }
            else
            {
                if (loader.loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Inner) ||
                    loader.loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Outer))
                {
                    loader.loaderParameter.DO_Loader_Picker_Vacuum((int)LoaderParameter.PickerVacuumPos.Inner, false);
                    loader.loaderParameter.DO_Loader_Picker_Vacuum((int)LoaderParameter.PickerVacuumPos.Outer, false);
                }
                else
                {
                    loader.loaderParameter.DO_Loader_Picker_Vacuum((int)LoaderParameter.PickerVacuumPos.Inner, true);
                    loader.loaderParameter.DO_Loader_Picker_Vacuum((int)LoaderParameter.PickerVacuumPos.Outer, true);
                }
            }
        }

        private void button_MotorMove_Unloader_ToStacker_Click(object sender, EventArgs e)
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "위치로 보내시겠습니까?"))
                return;

            Log.Write("GUI", Equipment.User_Name, "ButtonClick", "button_MotorMove_Unloader_ToStacker_Click");

            if (!unloader.MC_Func.MC_GetDone((int)Unloader.nAxis.TR_X) ||
                !unloader.MC_Func.MC_GetDone((int)Unloader.nAxis.TR_Z) ||
                !unloader.MC_Func.MC_GetInposition((int)Unloader.nAxis.TR_X) ||
                !unloader.MC_Func.MC_GetInposition((int)Unloader.nAxis.TR_Z))
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Unloader Transfer가 이동중입니다.");
                return;
            }

            Equipment.Type_Motor_Speed motor_Speed;
            motor_Speed = Equipment.Type_Motor_Speed.Coarse;
            int nTeachingPosIndex = (int)Loader.LDUL_TeachingPosList.UL_TR_LPortPos;
            unloader.MovetoUnloader_TeachingPositionsTransferX(nTeachingPosIndex, motor_Speed, true);
        }

        private void button_MotorMove_Unloader_ToStage_Click(object sender, EventArgs e)
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "위치로 보내시겠습니까?"))
                return;

            Log.Write("GUI", Equipment.User_Name, "ButtonClick", "button_MotorMove_Unloader_ToStage_Click");

            if (!unloader.MC_Func.MC_GetDone((int)Unloader.nAxis.TR_X) ||
                !unloader.MC_Func.MC_GetDone((int)Unloader.nAxis.TR_Z) ||
                !unloader.MC_Func.MC_GetInposition((int)Unloader.nAxis.TR_X) ||
                !unloader.MC_Func.MC_GetInposition((int)Unloader.nAxis.TR_Z))
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Unloader Transfer가 이동중입니다.");
                return;
            }

            Equipment.Type_Motor_Speed motor_Speed;
            motor_Speed = Equipment.Type_Motor_Speed.Coarse;
            int nTeachingPosIndex = (int)Loader.LDUL_TeachingPosList.UL_TR_WorkTablePos;
            unloader.MovetoUnloader_TeachingPositionsTransferX(nTeachingPosIndex, motor_Speed, true);
        }

        private void button_MotorMove_Unloader_Vacuum_Click(object sender, EventArgs e)
        {
            Log.Write("GUI", Equipment.User_Name, "ButtonClick", "button_MotorMove_Loader_Stacker_Vacuum_Click");

            if (Equipment.stLayerRecipeSet[0].ModuleInformation_Module_Width < workStage.m_pProcessConfigData.dModuleSizeSet ||
                Equipment.stLayerRecipeSet[0].ModuleInformation_Module_Height < workStage.m_pProcessConfigData.dModuleSizeSet)
            {
                if (unloader.unloaderParameter.DI_Unloader_Picker_VacuumCheck((int)UnloaderParameter.PickerVacuumPos.Inner))
                {
                    unloader.unloaderParameter.DO_Unloader_Picker_Vacuum((int)UnloaderParameter.PickerVacuumPos.Inner, false);
                }
                else
                {
                    unloader.unloaderParameter.DO_Unloader_Picker_Vacuum((int)UnloaderParameter.PickerVacuumPos.Inner, true);
                }
            }
            else
            {
                if (unloader.unloaderParameter.DI_Unloader_Picker_VacuumCheck((int)UnloaderParameter.PickerVacuumPos.Inner) ||
                    unloader.unloaderParameter.DI_Unloader_Picker_VacuumCheck((int)UnloaderParameter.PickerVacuumPos.Outer))
                {
                    unloader.unloaderParameter.DO_Unloader_Picker_Vacuum((int)UnloaderParameter.PickerVacuumPos.Inner, false);
                    unloader.unloaderParameter.DO_Unloader_Picker_Vacuum((int)UnloaderParameter.PickerVacuumPos.Outer, false);
                }
                else
                {
                    unloader.unloaderParameter.DO_Unloader_Picker_Vacuum((int)UnloaderParameter.PickerVacuumPos.Inner, true);
                    unloader.unloaderParameter.DO_Unloader_Picker_Vacuum((int)UnloaderParameter.PickerVacuumPos.Outer, true);
                }
            }
        }

        private void button_MotorMove_Stage_ScannerCenter_Click(object sender, EventArgs e)
        {
            if (Equipment.AutoRunStatus || Equipment.SelectRunEnable_New || Equipment.SemiAutoEnable || !workStage.m_bHomeOK)
            {
                return;
            }

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "위치로 보내시겠습니까?"))
                return;

            Log.Write("GUI", Equipment.User_Name, "ButtonClick", "button_MotorMove_Stage_ScannerCenter_Click");

            if (!workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.X) || 
                !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Y) || 
                !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Z) ||
                !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.X) || 
                !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Y) || 
                !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Z))
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Stage 가 이동중입니다.");
                return;
            }

            XyCoordinate xyInterpolatedCoordinate = new XyCoordinate(0.0,0.0);
            double dZPos = 0.0;
            dZPos = vision.stVisionTeachingPos[(int)Vision_TeachingPosList.Laser_FocusPos].Vision_Z;
            xyInterpolatedCoordinate.X = workStage.stWorkStageTeachingPos[(int)WorkStage_TeachingPosList.STAGE_ProcessingPos].Stage_X;
            xyInterpolatedCoordinate.Y = workStage.stWorkStageTeachingPos[(int)WorkStage_TeachingPosList.STAGE_ProcessingPos].Stage_Y;

            workStage.MapData_Apply((int)WorkStage.nMapData_Type.MapData_Stage_Scanner);

            workStage.MovetoWorkStage_ABS_PositionsZ(dZPos, Equipment.Type_Motor_Speed.Fine);
            workStage.MovetoWorkStage_ABS_PositionsXY(xyInterpolatedCoordinate, Equipment.Type_Motor_Speed.Coarse);

            workStage.Camera_HighRes.StopLive();
            workStage.Camera_LowRes.StopLive();
        }

        private void button_MotorMove_Stage_ScannerFineCam_Click(object sender, EventArgs e)
        {
            if (Equipment.AutoRunStatus || Equipment.SelectRunEnable_New || Equipment.SemiAutoEnable || !workStage.m_bHomeOK)
            {
                return;
            }

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "위치로 보내시겠습니까?"))
                return;

            Log.Write("GUI", Equipment.User_Name, "ButtonClick", "button_MotorMove_Stage_ScannerFineCam_Click");

            //아래 코드 보면 false면 구동중인거다.
            if (!workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.X) ||
                !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Y) ||
                !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Z) ||
                !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.X) ||
                !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Y) ||
                !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Z))
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Stage 가 이동중입니다.");
                return;
            }
            // 아래 확실히 Test 해보자.
            if (!workStage.IsWorkStageMoving(WorkStage.nAxis.X) ||
               !workStage.IsWorkStageMoving(WorkStage.nAxis.Y) ||
               !workStage.IsWorkStageMoving(WorkStage.nAxis.Z))
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Stage 가 이동중입니다.");
                return;
            }

            if (Equipment.stOffsetDistance.FromScannerToFineCam.X == 0.0 || 
                Equipment.stOffsetDistance.FromScannerToFineCam.Y == 0.0)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Scanner Center 위치와 Fine Camera Center 위치의 Offset 거리가 설정되어 있지 않습니다.");
                return;
            }

            // 현재 위치 구함
            double currX = workStage.GetEncWorkStagePos_Motor(WorkStage.nAxis.X);
            double currY = workStage.GetEncWorkStagePos_Motor(WorkStage.nAxis.Y);

            // Stage 기준 위치 (기존 코드)
            double expectedStageX = Equipment.StageOffset_forDrilling_X;
            double expectedStageY = Equipment.StageOffset_forDrilling_Y;

            // FineCam 기준 위치 = Stage 기준 위치 + Offset
            double expectedFineCamX = expectedStageX - Equipment.stOffsetDistance.FromScannerToFineCam.X;
            double expectedFineCamY = expectedStageY - Equipment.stOffsetDistance.FromScannerToFineCam.Y;

            // 허용 오차 (척 크기에 따라 ±150mm)
            double toleranceX = 150.0;
            double toleranceY = 300.0;

            // 현재 위치가 Stage 기준 또는 FineCam 기준 둘 중 하나라도 범위 안에 있어야 함
            bool isNearStageCenter = Math.Abs(currX - expectedStageX) <= toleranceX && Math.Abs(currY - expectedStageY) <= toleranceY;
            bool isNearFineCamCenter = Math.Abs(currX - expectedFineCamX) <= toleranceX && Math.Abs(currY - expectedFineCamY) <= toleranceY;
            // 기준 위치에서 너무 멀리 떨어진 경우: 경고 후 return
            if (!isNearStageCenter && !isNearFineCamCenter)
            {
                Log.Write("SLD-200", Equipment.User_Name, "현재 위치는 Stage 또는 FineCam 기준 위치가 아닙니다. 이동 불가.");
                new MessageBoxOk().ShowDialog("Warning !", "현재 위치는 Stage 또는 FineCam 기준 위치가 아닙니다.\n해당 위치에서 이동할 수 없습니다.");
                return;
            }
            else
            {
                // 이동할 목표 위치 (현재 위치에서 Offset 만큼 이동)
                XyCoordinate targetPosition = new XyCoordinate
                {
                    X = currX - Equipment.stOffsetDistance.FromScannerToFineCam.X,
                    Y = currY - Equipment.stOffsetDistance.FromScannerToFineCam.Y
                };

                workStage.MapData_Apply((int)WorkStage.nMapData_Type.MapData_Stage_Scanner);
                workStage.MovetoWorkStage_ABS_PositionsXY(targetPosition, Equipment.Type_Motor_Speed.Coarse);

                workStage.Camera_HighRes.StartLive();
                workStage.Camera_LowRes.StartLive();
            }
            
        }

        private void button_MotorMove_Stage_FineCamScanner_Click(object sender, EventArgs e)
        {
            if (Equipment.AutoRunStatus || Equipment.SelectRunEnable_New || Equipment.SemiAutoEnable || !workStage.m_bHomeOK)
            {
                return;
            }

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "위치로 보내시겠습니까?"))
                return;

            Log.Write("GUI", Equipment.User_Name, "ButtonClick", "button_MotorMove_Stage_FineCamScanner_Click");

            //아래 코드 보면 false면 구동중인거다.
            if (!workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.X) ||
                !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Y) ||
                !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Z) ||
                !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.X) ||
                !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Y) ||
                !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Z))
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Stage 가 이동중입니다.");
                return;
            }
            // 아래 확실히 Test 해보자.
            //if (!workStage.IsWorkStageMoving(WorkStage.nAxis.X) ||
            //   !workStage.IsWorkStageMoving(WorkStage.nAxis.Y) ||
            //   !workStage.IsWorkStageMoving(WorkStage.nAxis.Z))
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Warning !", "Stage 가 이동중입니다.");
            //    return;
            //}

            if (Equipment.stOffsetDistance.FromScannerToFineCam.X == 0.0 ||
                Equipment.stOffsetDistance.FromScannerToFineCam.Y == 0.0)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Scanner Center 위치와 Fine Camera Center 위치의 Offset 거리가 설정되어 있지 않습니다.");
                return;
            }

            if(true)
            {
                // 현재 위치
                double currX = workStage.GetEncWorkStagePos_Motor(WorkStage.nAxis.X);
                double currY = workStage.GetEncWorkStagePos_Motor(WorkStage.nAxis.Y);

                // FineCam 기준 위치 계산 (Scanner 중심에서 Offset 뺀 위치가 FineCam 위치)
                double expectedScannerPosX = Equipment.StageOffset_forDrilling_X;
                double expectedScannerPosY = Equipment.StageOffset_forDrilling_Y;

                // Stage 기준 위치 (기존 코드)
                double expectedStageX = Equipment.StageOffset_forDrilling_X;
                double expectedStageY = Equipment.StageOffset_forDrilling_Y;

                // FineCam 기준 위치 = Stage 기준 위치 + Offset
                double expectedFineCamX = expectedStageX - Equipment.stOffsetDistance.FromScannerToFineCam.X;
                double expectedFineCamY = expectedStageY - Equipment.stOffsetDistance.FromScannerToFineCam.Y;

                // 허용 오차 (척 크기에 따라 ±150mm)
                double toleranceX = 150.0;
                double toleranceY = 300.0;

                // 현재 위치가 Stage 기준 또는 FineCam 기준 둘 중 하나라도 범위 안에 있어야 함
                bool isNearStageCenter = Math.Abs(currX - expectedStageX) <= toleranceX && Math.Abs(currY - expectedStageY) <= toleranceY;
                bool isNearFineCamCenter = Math.Abs(currX - expectedFineCamX) <= toleranceX && Math.Abs(currY - expectedFineCamY) <= toleranceY;
                // 기준 위치에서 너무 멀리 떨어진 경우: 경고 후 return
                if (!isNearStageCenter && !isNearFineCamCenter)
                {
                    Log.Write("SLD-200", Equipment.User_Name, "현재 위치는 Stage 또는 FineCam 기준 위치가 아닙니다. 이동 불가.");
                    new MessageBoxOk().ShowDialog("Warning !", "현재 위치는 Stage 또는 FineCam 기준 위치가 아닙니다.\n해당 위치에서 이동할 수 없습니다.");
                    return;
                }

                // 이동할 위치는 현재 위치에서 Offset 만큼 더한 위치 → Scanner 중심
                XyCoordinate targetPosition = new XyCoordinate
                {
                    X = currX + Equipment.stOffsetDistance.FromScannerToFineCam.X,
                    Y = currY + Equipment.stOffsetDistance.FromScannerToFineCam.Y
                };

                workStage.MapData_Apply((int)WorkStage.nMapData_Type.MapData_Stage_Scanner);
                workStage.MovetoWorkStage_ABS_PositionsXY(targetPosition, Equipment.Type_Motor_Speed.Coarse);
            }
            else
            {
                XyCoordinate xyInterpolatedCoordinate = new XyCoordinate(0.0, 0.0);
                xyInterpolatedCoordinate.X =
                    workStage.GetEncWorkStagePos_Motor(WorkStage.nAxis.X) + Equipment.stOffsetDistance.FromScannerToFineCam.X;
                xyInterpolatedCoordinate.Y =
                    workStage.GetEncWorkStagePos_Motor(WorkStage.nAxis.Y) + Equipment.stOffsetDistance.FromScannerToFineCam.Y;

                workStage.MapData_Apply((int)WorkStage.nMapData_Type.MapData_Stage_Scanner);
                workStage.MovetoWorkStage_ABS_PositionsXY(xyInterpolatedCoordinate, Equipment.Type_Motor_Speed.Coarse);
            }
                
        }

        private void button_MotorMove_Stage_FineCamHeightSensor_Click(object sender, EventArgs e)
        {
            if (Equipment.AutoRunStatus || Equipment.SelectRunEnable_New || Equipment.SemiAutoEnable || !workStage.m_bHomeOK)
            {
                return;
            }

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "위치로 보내시겠습니까?"))
                return;

            Log.Write("GUI", Equipment.User_Name, "ButtonClick", "button_MotorMove_Stage_FineCamHeightSensor_Click");

            //아래 코드 보면 false면 구동중인거다.
            if (!workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.X) ||
                !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Y) ||
                !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Z) ||
                !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.X) ||
                !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Y) ||
                !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Z))
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Stage 가 이동중입니다.");
                return;
            }

            if (Equipment.stOffsetDistance.FromFineCamToLaserHeightSensor.X == 0.0 || 
                Equipment.stOffsetDistance.FromFineCamToLaserHeightSensor.Y == 0.0)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Laser Height Sensor 위치와 Fine Camera Center 위치의 Offset 거리가 설정되어 있지 않습니다.");
                return;
            }

            if(true)
            {
                // 현재 위치
                double currX = workStage.GetEncWorkStagePos_Motor(WorkStage.nAxis.X);
                double currY = workStage.GetEncWorkStagePos_Motor(WorkStage.nAxis.Y);

                // Stage 기준 위치 (기존 코드)
                double expectedStageX = Equipment.StageOffset_forDrilling_X;
                double expectedStageY = Equipment.StageOffset_forDrilling_Y;

                // FineCam 기준 위치 = Stage 기준 위치 + Offset
                double expectedFineCamX = expectedStageX - Equipment.stOffsetDistance.FromScannerToFineCam.X;
                double expectedFineCamY = expectedStageY - Equipment.stOffsetDistance.FromScannerToFineCam.Y;

                // heightsensor 기준 위치 = FineCam 기준 위치 + Offset
                double expectedHeightSensorX = expectedFineCamX + Equipment.stOffsetDistance.FromFineCamToLaserHeightSensor.X;
                double expectedHeightSensorY = expectedFineCamY + Equipment.stOffsetDistance.FromFineCamToLaserHeightSensor.Y;

                // 허용 오차 (척 크기에 따라 ±150mm)
                double toleranceX = 150.0;
                double toleranceY = 300.0;

                // 현재 위치가 heightsensor 기준 또는 FineCam 기준 둘 중 하나라도 범위 안에 있어야 함
                bool isNearStageCenter = Math.Abs(currX - expectedHeightSensorX) <= toleranceX && Math.Abs(currY - expectedHeightSensorY) <= toleranceY;
                bool isNearFineCamCenter = Math.Abs(currX - expectedFineCamX) <= toleranceX && Math.Abs(currY - expectedFineCamY) <= toleranceY;
                // 기준 위치에서 너무 멀리 떨어진 경우: 경고 후 return
                if (!isNearStageCenter && !isNearFineCamCenter)
                {
                    Log.Write("SLD-200", Equipment.User_Name, "현재 위치는 Stage 또는 FineCam 기준 위치가 아닙니다. 이동 불가.");
                    new MessageBoxOk().ShowDialog("Warning !", "현재 위치는 Stage 또는 FineCam 기준 위치가 아닙니다.\n해당 위치에서 이동할 수 없습니다.");
                    return;
                }

                // 이동할 위치는 현재 위치에서 Offset 만큼 더한 위치 → Scanner 중심
                XyCoordinate targetPosition = new XyCoordinate
                {
                    X = currX + Equipment.stOffsetDistance.FromFineCamToLaserHeightSensor.X,
                    Y = currY + Equipment.stOffsetDistance.FromFineCamToLaserHeightSensor.Y
                };

                if (Equipment.Machine_LaserType_CO2)
                {
                    workStage.MapData_Apply((int)WorkStage.nMapData_Type.MapData_Stage_Scanner);
                }
                else
                {
                    workStage.MapData_Apply((int)WorkStage.nMapData_Type.MapData_Stage_FineCam);
                }
                workStage.MovetoWorkStage_ABS_PositionsXY(targetPosition, Equipment.Type_Motor_Speed.Coarse);
            }
            else
            {
                XyCoordinate xyInterpolatedCoordinate = new XyCoordinate(0.0, 0.0);
                xyInterpolatedCoordinate.X =
                    workStage.GetEncWorkStagePos_Motor(WorkStage.nAxis.X) + Equipment.stOffsetDistance.FromFineCamToLaserHeightSensor.X;
                xyInterpolatedCoordinate.Y =
                    workStage.GetEncWorkStagePos_Motor(WorkStage.nAxis.Y) + Equipment.stOffsetDistance.FromFineCamToLaserHeightSensor.Y;

                if (Equipment.Machine_LaserType_CO2)
                {
                    workStage.MapData_Apply((int)WorkStage.nMapData_Type.MapData_Stage_Scanner);
                }
                else
                {
                    workStage.MapData_Apply((int)WorkStage.nMapData_Type.MapData_Stage_FineCam);
                }
                workStage.MovetoWorkStage_ABS_PositionsXY(xyInterpolatedCoordinate, Equipment.Type_Motor_Speed.Coarse);
            }
        }

        private void button_MotorMove_Stage_HeightSensorFineCam_Click(object sender, EventArgs e)
        {
            if (Equipment.AutoRunStatus || Equipment.SelectRunEnable_New || Equipment.SemiAutoEnable || !workStage.m_bHomeOK)
            {
                return;
            }

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "위치로 보내시겠습니까?"))
                return;

            Log.Write("GUI", Equipment.User_Name, "ButtonClick", "button_MotorMove_Stage_HeightSensorFineCam_Click");

            if (!workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.X) ||
                !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Y) ||
                !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Z) ||
                !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.X) ||
                !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Y) ||
                !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Z))
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Stage 가 이동중입니다.");
                return;
            }

            if(true)
            {
                // 현재 위치
                double currX = workStage.GetEncWorkStagePos_Motor(WorkStage.nAxis.X);
                double currY = workStage.GetEncWorkStagePos_Motor(WorkStage.nAxis.Y);

                // 기준 위치 계산 
                double expectedStageX = Equipment.StageOffset_forDrilling_X;
                double expectedStageY = Equipment.StageOffset_forDrilling_Y;

                // FineCam 기준 위치 = Stage 기준 위치 + Offset
                double expectedFineCamX = expectedStageX - Equipment.stOffsetDistance.FromScannerToFineCam.X;
                double expectedFineCamY = expectedStageY - Equipment.stOffsetDistance.FromScannerToFineCam.Y;

                // heightsensor 기준 위치 = FineCam 기준 위치 + Offset
                double expectedHeightSensorX = expectedFineCamX + Equipment.stOffsetDistance.FromFineCamToLaserHeightSensor.X;
                double expectedHeightSensorY = expectedFineCamY + Equipment.stOffsetDistance.FromFineCamToLaserHeightSensor.Y;

                // 허용 오차 (척 크기에 따라 ±150mm)
                double toleranceX = 150.0;
                double toleranceY = 300.0;

                // 현재 위치가 heightsensor 기준 또는 FineCam 기준 둘 중 하나라도 범위 안에 있어야 함
                bool isNearStageCenter = Math.Abs(currX - expectedHeightSensorX) <= toleranceX && Math.Abs(currY - expectedHeightSensorY) <= toleranceY;
                bool isNearFineCamCenter = Math.Abs(currX - expectedFineCamX) <= toleranceX && Math.Abs(currY - expectedFineCamY) <= toleranceY;
                // 기준 위치에서 너무 멀리 떨어진 경우: 경고 후 return
                if (!isNearStageCenter && !isNearFineCamCenter)
                {
                    Log.Write("SLD-200", Equipment.User_Name, "현재 위치는 Stage 또는 FineCam 기준 위치가 아닙니다. 이동 불가.");
                    new MessageBoxOk().ShowDialog("Warning !", "현재 위치는 Stage 또는 FineCam 기준 위치가 아닙니다.\n해당 위치에서 이동할 수 없습니다.");
                    return;
                }

                // 이동할 위치는 현재 위치에서 Offset 만큼 더한 위치 → Scanner 중심
                XyCoordinate targetPosition = new XyCoordinate
                {
                    X = currX - Equipment.stOffsetDistance.FromFineCamToLaserHeightSensor.X,
                    Y = currY - Equipment.stOffsetDistance.FromFineCamToLaserHeightSensor.Y
                };

                if (Equipment.Machine_LaserType_CO2)
                {
                    workStage.MapData_Apply((int)WorkStage.nMapData_Type.MapData_Stage_FineCam);
                }
                else
                {
                    workStage.MapData_Apply((int)WorkStage.nMapData_Type.MapData_Stage_FineCam);
                }
                workStage.MovetoWorkStage_ABS_PositionsXY(targetPosition, Equipment.Type_Motor_Speed.Coarse);
            }
            else
            {
                XyCoordinate xyInterpolatedCoordinate = new XyCoordinate(0.0, 0.0);
                xyInterpolatedCoordinate.X =
                    workStage.GetEncWorkStagePos_Motor(WorkStage.nAxis.X) - Equipment.stOffsetDistance.FromFineCamToLaserHeightSensor.X;
                xyInterpolatedCoordinate.Y =
                    workStage.GetEncWorkStagePos_Motor(WorkStage.nAxis.Y) - Equipment.stOffsetDistance.FromFineCamToLaserHeightSensor.Y;

                if (Equipment.Machine_LaserType_CO2)
                {
                    workStage.MapData_Apply((int)WorkStage.nMapData_Type.MapData_Stage_FineCam);
                }
                else
                {
                    workStage.MapData_Apply((int)WorkStage.nMapData_Type.MapData_Stage_FineCam);
                }
                workStage.MovetoWorkStage_ABS_PositionsXY(xyInterpolatedCoordinate, Equipment.Type_Motor_Speed.Coarse);
            }
        }

        private void button_MotorMove_Stage_FineCamCoarseCam_Click(object sender, EventArgs e)
        {
            if (Equipment.AutoRunStatus || Equipment.SelectRunEnable_New || Equipment.SemiAutoEnable || !workStage.m_bHomeOK)
            {
                return;
            }

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "위치로 보내시겠습니까?"))
                return;

            Log.Write("GUI", Equipment.User_Name, "ButtonClick", "button_MotorMove_Stage_FineCamCoarseCam_Click");

            if (!workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.X) ||
                !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Y) ||
                !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Z) ||
                !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.X) ||
                !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Y) ||
                !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Z))
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Stage 가 이동중입니다.");
                return;
            }

            if (Equipment.stOffsetDistance.FromFineCamToCoarseCam.X == 0.0 || 
                Equipment.stOffsetDistance.FromFineCamToCoarseCam.Y == 0.0)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Fine Camera Center 위치와 Coarse Camera Center 위치의 Offset 거리가 설정되어 있지 않습니다.");
                return;
            }

            if(true)
            {
                // 현재 위치
                double currX = workStage.GetEncWorkStagePos_Motor(WorkStage.nAxis.X);
                double currY = workStage.GetEncWorkStagePos_Motor(WorkStage.nAxis.Y);

                // Stage 기준 위치 (기존 코드)
                double expectedStageX = Equipment.StageOffset_forDrilling_X;
                double expectedStageY = Equipment.StageOffset_forDrilling_Y;

                // FineCam 기준 위치 = Stage 기준 위치 + Offset
                double expectedFineCamX = expectedStageX - Equipment.stOffsetDistance.FromScannerToFineCam.X;
                double expectedFineCamY = expectedStageY - Equipment.stOffsetDistance.FromScannerToFineCam.Y;

                // CoarseCam 기준 위치 = FineCam 기준 위치 + Offset
                double expectedCoarseCamX = expectedFineCamX + Equipment.stOffsetDistance.FromFineCamToCoarseCam.X;
                double expectedCoarseCamY = expectedFineCamY + Equipment.stOffsetDistance.FromFineCamToCoarseCam.Y;

                // 허용 오차 (척 크기에 따라 ±150mm)
                double toleranceX = 150.0;
                double toleranceY = 300.0;

                // 현재 위치가 heightsensor 기준 또는 FineCam 기준 둘 중 하나라도 범위 안에 있어야 함
                bool isNearStageCenter = Math.Abs(currX - expectedCoarseCamX) <= toleranceX && Math.Abs(currY - expectedCoarseCamY) <= toleranceY;
                bool isNearFineCamCenter = Math.Abs(currX - expectedFineCamX) <= toleranceX && Math.Abs(currY - expectedFineCamY) <= toleranceY;
                // 기준 위치에서 너무 멀리 떨어진 경우: 경고 후 return
                if (!isNearStageCenter && !isNearFineCamCenter)
                {
                    Log.Write("SLD-200", Equipment.User_Name, "현재 위치는 Stage 또는 FineCam 기준 위치가 아닙니다. 이동 불가.");
                    new MessageBoxOk().ShowDialog("Warning !", "현재 위치는 Stage 또는 FineCam 기준 위치가 아닙니다.\n해당 위치에서 이동할 수 없습니다.");
                    return;
                }

                // 이동할 위치는 현재 위치에서 Offset 만큼 더한 위치 → Scanner 중심
                XyCoordinate targetPosition = new XyCoordinate
                {
                    X = currX - Equipment.stOffsetDistance.FromFineCamToCoarseCam.X,
                    Y = currY - Equipment.stOffsetDistance.FromFineCamToCoarseCam.Y
                };

                workStage.MapData_Apply((int)WorkStage.nMapData_Type.MapData_Stage_FineCam);
                workStage.MovetoWorkStage_ABS_PositionsXY(targetPosition, Equipment.Type_Motor_Speed.Coarse);
            }
            else
            {
                XyCoordinate xyInterpolatedCoordinate = new XyCoordinate(0.0, 0.0);
                xyInterpolatedCoordinate.X =
                    workStage.GetEncWorkStagePos_Motor(WorkStage.nAxis.X) - Equipment.stOffsetDistance.FromFineCamToCoarseCam.X;
                xyInterpolatedCoordinate.Y =
                    workStage.GetEncWorkStagePos_Motor(WorkStage.nAxis.Y) - Equipment.stOffsetDistance.FromFineCamToCoarseCam.Y;

                workStage.MapData_Apply((int)WorkStage.nMapData_Type.MapData_Stage_FineCam);
                workStage.MovetoWorkStage_ABS_PositionsXY(xyInterpolatedCoordinate, Equipment.Type_Motor_Speed.Coarse);
            }
        }

        private void button_MotorMove_Stage_CoarseCamFineCam_Click(object sender, EventArgs e)
        {
            if (Equipment.AutoRunStatus || Equipment.SelectRunEnable_New || Equipment.SemiAutoEnable || !workStage.m_bHomeOK)
            {
                return;
            }

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "위치로 보내시겠습니까?"))
                return;

            Log.Write("GUI", Equipment.User_Name, "ButtonClick", "button_MotorMove_Stage_CoarseCamFineCam_Click");

            if (!workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.X) ||
                !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Y) ||
                !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Z) ||
                !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.X) ||
                !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Y) ||
                !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Z))
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Stage 가 이동중입니다.");
                return;
            }

            if (Equipment.stOffsetDistance.FromFineCamToCoarseCam.X == 0.0 ||
                Equipment.stOffsetDistance.FromFineCamToCoarseCam.Y == 0.0)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Fine Camera Center 위치와 Coarse Camera Center 위치의 Offset 거리가 설정되어 있지 않습니다.");
                return;
            }

            if(true)
            {
                // 현재 위치
                double currX = workStage.GetEncWorkStagePos_Motor(WorkStage.nAxis.X);
                double currY = workStage.GetEncWorkStagePos_Motor(WorkStage.nAxis.Y);

                // Stage 기준 위치 (기존 코드)
                double expectedStageX = Equipment.StageOffset_forDrilling_X;
                double expectedStageY = Equipment.StageOffset_forDrilling_Y;

                // FineCam 기준 위치 = Stage 기준 위치 + Offset
                double expectedFineCamX = expectedStageX - Equipment.stOffsetDistance.FromScannerToFineCam.X;
                double expectedFineCamY = expectedStageY - Equipment.stOffsetDistance.FromScannerToFineCam.Y;

                // CoarseCam 기준 위치 = FineCam 기준 위치 + Offset
                double expectedCoarseCamX = expectedFineCamX + Equipment.stOffsetDistance.FromFineCamToCoarseCam.X;
                double expectedCoarseCamY = expectedFineCamY + Equipment.stOffsetDistance.FromFineCamToCoarseCam.Y;

                // 허용 오차 (척 크기에 따라 ±150mm)
                double toleranceX = 150.0;
                double toleranceY = 300.0;

                // 현재 위치가 CoarseCam 기준 또는 FineCam 기준 둘 중 하나라도 범위 안에 있어야 함
                bool isNearStageCenter = Math.Abs(currX - expectedCoarseCamX) <= toleranceX && Math.Abs(currY - expectedCoarseCamY) <= toleranceY;
                bool isNearFineCamCenter = Math.Abs(currX - expectedFineCamX) <= toleranceX && Math.Abs(currY - expectedFineCamY) <= toleranceY;
                // 기준 위치에서 너무 멀리 떨어진 경우: 경고 후 return
                if (!isNearStageCenter && !isNearFineCamCenter)
                {
                    Log.Write("SLD-200", Equipment.User_Name, "현재 위치는 CoarseCam 또는 FineCam 기준 위치가 아닙니다. 이동 불가.");
                    new MessageBoxOk().ShowDialog("Warning !", "현재 위치는 CoarseCam 또는 FineCam 기준 위치가 아닙니다.\n해당 위치에서 이동할 수 없습니다.");
                    return;
                }

                // 이동할 위치는 현재 위치에서 Offset 만큼 더한 위치 → Scanner 중심
                XyCoordinate targetPosition = new XyCoordinate
                {
                    X = currX + Equipment.stOffsetDistance.FromFineCamToCoarseCam.X,
                    Y = currY + Equipment.stOffsetDistance.FromFineCamToCoarseCam.Y
                };

                workStage.MapData_Apply((int)WorkStage.nMapData_Type.MapData_Stage_FineCam);
                workStage.MovetoWorkStage_ABS_PositionsXY(targetPosition, Equipment.Type_Motor_Speed.Coarse);
            }
            else
            {
                XyCoordinate xyInterpolatedCoordinate = new XyCoordinate(0.0, 0.0);
                xyInterpolatedCoordinate.X =
                    workStage.GetEncWorkStagePos_Motor(WorkStage.nAxis.X) + Equipment.stOffsetDistance.FromFineCamToCoarseCam.X;
                xyInterpolatedCoordinate.Y =
                    workStage.GetEncWorkStagePos_Motor(WorkStage.nAxis.Y) + Equipment.stOffsetDistance.FromFineCamToCoarseCam.Y;

                workStage.MapData_Apply((int)WorkStage.nMapData_Type.MapData_Stage_FineCam);
                workStage.MovetoWorkStage_ABS_PositionsXY(xyInterpolatedCoordinate, Equipment.Type_Motor_Speed.Coarse);
            }
        }

        private void button_MotorMove_Stage_ToUnloading_Click(object sender, EventArgs e)
        {
            if (Equipment.AutoRunStatus || Equipment.SelectRunEnable_New || Equipment.SemiAutoEnable || !workStage.m_bHomeOK)
            {
                return;
            }

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "위치로 보내시겠습니까?"))
                return;

            Log.Write("GUI", Equipment.User_Name, "ButtonClick", "button_MotorMove_Stage_ToUnloading_Click");

            if (!workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.X) ||
                !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Y) ||
                !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Z) ||
                !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.X) ||
                !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Y) ||
                !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Z))
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Stage 가 이동중입니다.");
                return;
            }

            Equipment.Type_Motor_Speed motor_Speed;
            motor_Speed = Equipment.Type_Motor_Speed.Coarse;
            int nTeachingPosIndex = (int)WorkStage.WorkStage_TeachingPosList.STAGE_UnloadingPos;
            workStage.MovetoWorkStage_TeachingPositionsXY(nTeachingPosIndex, motor_Speed);

            workStage.Camera_HighRes.StopLive();
            workStage.Camera_LowRes.StopLive();
        }

        private void button_MotorMove_Stage_ToLoading_Click(object sender, EventArgs e)
        {
            if (Equipment.AutoRunStatus || Equipment.SelectRunEnable_New || Equipment.SemiAutoEnable || !workStage.m_bHomeOK)
            {
                return;
            }

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "위치로 보내시겠습니까?"))
                return;

            Log.Write("GUI", Equipment.User_Name, "ButtonClick", "button_MotorMove_Stage_ToLoading_Click");

            if (!workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.X) ||
                !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Y) ||
                !workStage.MC_Func.MC_GetDone((int)WorkStage.nAxis.Z) ||
                !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.X) ||
                !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Y) ||
                !workStage.MC_Func.MC_GetInposition((int)WorkStage.nAxis.Z))
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Stage 가 이동중입니다.");
                return;
            }

            Equipment.Type_Motor_Speed motor_Speed;
            motor_Speed = Equipment.Type_Motor_Speed.Coarse;
            int nTeachingPosIndex = (int)WorkStage.WorkStage_TeachingPosList.STAGE_LoadingPos;
            workStage.MovetoWorkStage_TeachingPositionsXY(nTeachingPosIndex, motor_Speed);

            workStage.Camera_HighRes.StopLive();
            workStage.Camera_LowRes.StopLive();
        }

        private void button_MotorMove_Stage_Vacuum_Click(object sender, EventArgs e)
        {
            Log.Write("GUI", Equipment.User_Name, "ButtonClick", "button_MotorMove_Stage_Vacuum_Click");

            if(workStage.workStageParameter.DI_Stage_Vacuum_Check())
            {
                workStage.workStageParameter.DO_Stage_Blow(true);
                workStage.workStageParameter.DO_Stage_Vacuum(false);
                Thread.Sleep(500); // 1초 대기
                workStage.workStageParameter.DO_Stage_Blow(false);
                workStage.ElectroPneumaticRegulatorComm_Pressure_Set(-1.3);
            }
            else
            {
                
                workStage.workStageParameter.DO_Stage_Blow(false);
                workStage.workStageParameter.DO_Stage_Vacuum(true);

                if (Equipment.stLayerRecipeSet[0].EPRO_ModuleAbsorptionLevel <= 0.0)
                {
                    workStage.ElectroPneumaticRegulatorComm_Pressure_Set(Equipment.stLayerRecipeSet[0].EPRO_ModuleAbsorptionLevel);
                }
                else
                {
                    workStage.ElectroPneumaticRegulatorComm_Pressure_Set(-60.0);
                }
            }
        }

        private void SetColor(Control control, Color Backcolor, Color foreColor)
        {
            if (control.InvokeRequired)
            {
                this.Invoke(new System.Action(() =>
                {
                    SetColor(control, Backcolor, foreColor);
                }));
            }
            else
            {
                control.BackColor = Backcolor;
                control.ForeColor = foreColor;
            }
        }

        private bool ShowErrorAndReturn(string msg)
        {
            ShowError(msg);
            return false;
        }

        private void ShowError(string msg)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new System.Action(() => ShowError(msg)));
                return;
            }

            Log.Write("SLD-200", Equipment.User_Name, msg);
            new QMC.Core.MessageBoxOk().ShowDialog("Error !", msg);
        }

        private void button_MotorMove_Unloader_ToSafetyZ_Click(object sender, EventArgs e)
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "위치로 보내시겠습니까?"))
                return;

            Log.Write("GUI", Equipment.User_Name, "ButtonClick", "button_MotorMove_Unloader_ToSafetyZ_Click");

            if (!unloader.MC_Func.MC_GetDone((int)Unloader.nAxis.TR_X) ||
                !unloader.MC_Func.MC_GetDone((int)Unloader.nAxis.TR_Z) ||
                !unloader.MC_Func.MC_GetInposition((int)Unloader.nAxis.TR_X) ||
                !unloader.MC_Func.MC_GetInposition((int)Unloader.nAxis.TR_Z))
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Unloader Transfer가 이동중입니다.");
                return;
            }

            Equipment.Type_Motor_Speed motor_Speed;
            motor_Speed = Equipment.Type_Motor_Speed.Fine;
            int nTeachingPosIndex = (int)Loader.LDUL_TeachingPosList.UL_TR_SafetyPos;
            unloader.MovetoUnloader_TeachingPositionsTransferZ(nTeachingPosIndex, motor_Speed, true);
        }

        private void button_MotorMove_Loader_ToSafetyZ_Click(object sender, EventArgs e)
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "위치로 보내시겠습니까?"))
                return;

            Log.Write("GUI", Equipment.User_Name, "ButtonClick", "button_MotorMove_Loader_ToSafetyZ_Click");

            if (!loader.MC_Func.MC_GetDone((int)Loader.nAxis.TR_X) ||
                !loader.MC_Func.MC_GetDone((int)Loader.nAxis.TR_Z) ||
                !loader.MC_Func.MC_GetInposition((int)Loader.nAxis.TR_X) ||
                !loader.MC_Func.MC_GetInposition((int)Loader.nAxis.TR_Z))
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Warning !", "Loader Transfer가 이동중입니다.");
                return;
            }

            Equipment.Type_Motor_Speed motor_Speed;
            motor_Speed = Equipment.Type_Motor_Speed.Coarse;
            int nTeachingPosIndex = (int)Loader.LDUL_TeachingPosList.LD_TR_SafetyPos;
            loader.MovetoLoader_TeachingPositionsTransferZ(nTeachingPosIndex, motor_Speed, true);
        }
    }
}
