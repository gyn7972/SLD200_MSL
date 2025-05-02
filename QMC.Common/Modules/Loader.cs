
using ACS.SPiiPlusNET;
using QMC.Common.Laser;
using QMC.Common.Motion.ACS.Motions;
using QMC.Common.Motion.Ajin.Motions;
using QMC.Common.Parts;
using QMC.Common.Vision.HIKVISION;
using QMC.Common.VisionPart;
using QMC.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using SerialCommHoneywellBarcodeReader;
using static QMC.Common.Parts.WorkStageParameter;
using Point = System.Drawing.Point;
using SerialCommLaserPowerMeter1;
using SerialCommLaserPowerMeter2;
using System.IO.Ports;
using MessageBox = System.Windows.Forms.MessageBox;
using static QMC.Common.Modules.Unloader;
using System.ServiceModel.Syndication;
using static QMC.Common.Modules.WorkStage;
using System.Timers;
using System.Threading.Tasks;
using System.Security.Policy;
using System.Linq;


namespace QMC.Common.Modules
{
    [Serializable]
    public class Loader : Module
    {
        #region Define


        //#if true                                                                //  SLD-200C
#if SLD_200C                                                                 //  SLD-200U
        public enum nAxis                                                       //  SLD-200C 에서 사용하는 축 번호    
        {
            //  축 번호 변경 전 (Z0:4,    Z1:5,   TR_X:6,     TR_Z:7,     ALN_X:8,    ALN_Y:9)
            //  축 번호 변경 후 (Z0:6,    Z1:7,   TR_X:8,     TR_Z:9,     ALN_X:1,    ALN_Y:2)

            Z0 = 6,
            Z1 = 7,
            TR_X = 8,
            TR_Z = 9,
            ALN_X = 1,
            ALN_Y = 2,
        }
#else
        public enum nAxis                                                       //  SLD-200U 에서 사용하는 축 번호   
        {
            //  축 번호 변경 전 (Z0:4,    Z1:5,   TR_X:6,     TR_Z:7,     ALN_X:8,    ALN_Y:9)
            //  축 번호 변경 후 (Z0:6,    Z1:7,   TR_X:8,     TR_Z:9,     ALN_X:1,    ALN_Y:2)

            Z0 = 5,
            Z1 = 6,
            TR_X = 7,
            TR_Z = 8,
            ALN_X = 0,
            ALN_Y = 1,
        }
#endif


        #endregion



        #region Alarm
        public enum AlarmKey
        {
            FirstAlarm = 1000,
            eMAligner_Wide_Fail  ,
            MAligner_MoveXY_Widely_DoneCheck_Timeout,
            MAligner_VacuumOn_Fail,
            MAligner_MoveXY_Narrowly_Fail,
            MAligner_MoveXY_LittleWidely_Fail,
            MAligner_MoveXY_ModulePickupWaitingPos_Fail,
            LD_Stacker1_ModuleWork_PosSet = 2000,
            LD_Stacker1_ModulePickupWaitingPos_Step_No_More_Material,
            LD_Stacker1_ModulePickupWaitingPos_Step_Too_Many_Material,
            LD_Stacker1_Module_Move_To_Loading_Position_Fail,
            StackerZ_Move_OverDistance_Fail,
            LD_Stacker1_Work_Pos_Move_To_Full_Sensor_Position,
            LD_Stacker0_FullSensor_Off_MoveFail,
            LD_TransferZ_Move_ReadyPos_Timeout,
            LD_TransferX_Move_ReadyPos_Timeout,
            LD_Stacker0_ModulePickup_ConditionCheck_Stacker0ModuleNotExist,
            LD_Stacker0_ModulePickup_ConditionCheck_Stacker0FullSensorNotExist,
            LD_Stacker0_ModulePickup_ConditionCheck_PickerVacuumSensorExist,
            LD_Transfer_StackerIsWorking,
            LD_TransferX_Move_StackerPos_Timeout,
            LD_TransferZ_Move_PickUpPos_Timeout,
            LD_Transfer_PickerVacuumOn_Timeout,
            LD_Stacker1_ModulePickup_ConditionCheck_Stacker1ModuleNotExist,
            LD_Stacker1_ModulePickup_ConditionCheck_Stacker1FullSensorNotExist,
            LD_Transfer_MAlignerIsWorking,
            LD_Transfer_MAligner_Align_Timeout,
            LD_TransferX_Move_MAlignerPos_Timeout,
            LD_Transfer_PickerVacuumOff_Timeout,
            LD_TransferZ_Move_PressPos_Timeout,
            LD_Transfer_MAlignerVacuumOn_Timeout,
            LD_Transfer_MAlignerVacuumOff_Timeout,
            LD_Transfer_MAlignerXY_Move_Timeout,
            LD_Transfer_MAlignerPickUp_Timeout,
            LD_Transfer_WorkStageVacuumOn_Timeout,
            LD_Transfer_WorkStage_Is_Working,
            LD_Transfer_LaserDrilling_Is_Working,
            LD_Transfer_WorkStageMove_Timeout,
            LD_TransferX_Move_LoadingPos_Timeout,
            LD_TransferZ_Move_PutDownPos_Timeout,
            LD_MAlignerXY_Move_Widely_Timeout,
            LD_Transfer_PickerVacuumOff_MAlignerVacuumOn_Timeout,
            LD_Maligner_Not_Set_Module_Size,
            LD_TransferZ_Move_VibrationPos_Timeout,
            LD_Ionizer_Alarm,
            LastAlarm = 2999,
            
        }
        protected override void InitAlarm()
        {
            Alarm alarm = new Alarm();
            alarm.Code = (int)AlarmKey.eMAligner_Wide_Fail;
            alarm.Title = "메카닉 얼라이너";
            alarm.Cause = "메카닉 얼라이너를 대기 위치로 보내는데 실패 하였습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);

            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.MAligner_MoveXY_Widely_DoneCheck_Timeout;
            alarm.Title = "메카닉 얼라이너";
            alarm.Cause = "메카닉 얼라이너를 대기 위치로 보내는데 Timeout 되었습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);

            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.MAligner_VacuumOn_Fail;
            alarm.Title = "메카닉 얼라이너";
            alarm.Cause = "메카닉 얼라이너 진공이 ON 되지 않았습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);

            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.MAligner_MoveXY_Narrowly_Fail;
            alarm.Title = "메카닉 얼라이너";
            alarm.Cause = "메카닉 얼라이너를 얼라인 위치로 보내는데 실패 하였습니다. \n메카닉 얼라이너 끼임이나 모터를 확인 하여 주십시요.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);

            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.MAligner_MoveXY_LittleWidely_Fail;
            alarm.Title = "메카닉 얼라이너";
            alarm.Cause = "메카닉 얼라이너를 얼라인 대기 위치로 보내는데 실패 하였습니다. \n메카닉 얼라이너 끼임이나 모터를 확인 하여 주십시요.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);

            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.MAligner_MoveXY_ModulePickupWaitingPos_Fail;
            alarm.Title = "Loader Left 스태커";
            alarm.Cause = "Loader Left 을 모듈 픽업 대기 위치로 보내는데 실패 하였습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);

            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.LD_Stacker1_ModuleWork_PosSet;
            alarm.Title = "Loader Left 스태커";
            alarm.Cause = "Loader Left 을 모듈 작업 위치로 보내는데 실패 하였습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);

            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.LD_Stacker1_ModulePickupWaitingPos_Step_No_More_Material;
            alarm.Title = "Loader Left 스태커";
            alarm.Cause = "Loader Left 자재가 없습니다.";
            alarm.Source = Name;
            alarm.Grade = "Info";
            m_dicAlarms.Add(alarm.Code, alarm);

            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.LD_Stacker1_ModulePickupWaitingPos_Step_Too_Many_Material;
            alarm.Title = "Loader Left 스태커";
            alarm.Cause = "Loader Left 자재가 너무 많습니다. 자재가 없이 이 알람이 발생 했다면 센서를 점검 해주시기 바랍니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);

            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.LD_Stacker1_Module_Move_To_Loading_Position_Fail;
            alarm.Title = "Loader Left 스태커";
            alarm.Cause = "자재 로딩 위치까지 이동 하지 못하였습니다. 자재가 있는데 이 알람이 발생 했다면 센서를 점검 하여 주십시요.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);


            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.StackerZ_Move_OverDistance_Fail;
            alarm.Title = "Loader Left 스태커";
            alarm.Cause = "자재 로딩 위치까지 이동 하지 못하였습니다. 자재가 있는데 이 알람이 발생 했다면 센서를 점검 하여 주십시요.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);



            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.LD_Stacker1_Work_Pos_Move_To_Full_Sensor_Position;
            alarm.Title = "Loader Left 스태커";
            alarm.Cause = "만재 선서가 감지 되지 않았습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);


            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.LD_Stacker0_FullSensor_Off_MoveFail;
            alarm.Title = "Loader Right 스태커";
            alarm.Cause = "만재 선서가 감지 되지 않았습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);


            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.LD_TransferZ_Move_ReadyPos_Timeout;
            alarm.Title = "Loader Trasfer";
            alarm.Cause = "Loader Trasfer Z 축이 대기 위치로 이동 하지 못하였습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);

            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.LD_TransferX_Move_ReadyPos_Timeout;
            alarm.Title = "Loader Trasfer";
            alarm.Cause = "Loader Trasfer Z 축이 대기 위치로 이동 하지 못하였습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);


            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.LD_Stacker0_ModulePickup_ConditionCheck_Stacker0ModuleNotExist;
            alarm.Title = "Loader Trasfer";
            alarm.Cause = "Loader Trasfer Right Load Port에 자재가 감지 되지 않았습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);

            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.LD_Stacker0_ModulePickup_ConditionCheck_Stacker0FullSensorNotExist;
            alarm.Title = "Loader Trasfer";
            alarm.Cause = "Loader Trasfer Right Load Port에 자재가 감지 되지 않았습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);

            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.LD_Stacker0_ModulePickup_ConditionCheck_PickerVacuumSensorExist;
            alarm.Title = "Loader Trasfer";
            alarm.Cause = "Loader Trasfer Picker에 자재가 있습니다. ";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);


            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.LD_Transfer_StackerIsWorking;
            alarm.Title = "Loader Trasfer";
            alarm.Cause = "Loader Trasfer Stacker가 동작 중입니다. \n스태커가 동작 중일 때는 트랜스퍼를 동작 시킬 수 없습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);


            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.LD_TransferX_Move_StackerPos_Timeout;
            alarm.Title = "Loader Trasfer";
            alarm.Cause = "Loader Trasfer X 축이 Load Port위치로 이동 하지 못하였습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);


            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.LD_TransferZ_Move_PickUpPos_Timeout;
            alarm.Title = "Loader Trasfer";
            alarm.Cause = "Loader Trasfer Z 축이 PickUp 위치로 이동 하지 못하였습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);

            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.LD_Transfer_PickerVacuumOn_Timeout;
            alarm.Title = "Loader Trasfer";
            alarm.Cause = "Loader Trasfer Picker 진공이 형성 되지 않았습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);


            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.LD_Stacker1_ModulePickup_ConditionCheck_Stacker1ModuleNotExist;
            alarm.Title = "Loader Trasfer";
            alarm.Cause = "Loader Trasfer Left Load Port에 자재가 감지 되지 않았습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);


            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.LD_Stacker1_ModulePickup_ConditionCheck_Stacker1FullSensorNotExist;
            alarm.Title = "Loader Trasfer";
            alarm.Cause = "Loader Trasfer Left Load Port에 만재 센서에 자재가 감지 되지 않았습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);

            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.LD_Transfer_MAlignerIsWorking;
            alarm.Title = "Loader Trasfer";
            alarm.Cause = "Loader Trasfer MAligner가 동작 중입니다. \nMAligner가 동작 중일 때는 트랜스퍼를 동작 시킬 수 없습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);


            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.LD_Transfer_MAligner_Align_Timeout;
            alarm.Title = "Loader Trasfer";
            alarm.Cause = "Loader Trasfer MAligner Align 동작이 Timeout 되었습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);


            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.LD_TransferX_Move_MAlignerPos_Timeout;
            alarm.Title = "Loader Trasfer";
            alarm.Cause = "Loader Trasfer X 축이 MAligner 위치로 이동 하지 못하였습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);


            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.LD_Transfer_PickerVacuumOff_Timeout;
            alarm.Title = "Loader Trasfer";
            alarm.Cause = "Loader Trasfer Picker 진공이 해제 되지 않았습니다. \n공압솔레노이드 벨브나 IO 모듈 릴레이 점검이 필요 합니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);


            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.LD_TransferZ_Move_PressPos_Timeout;
            alarm.Title = "Loader Trasfer";
            alarm.Cause = "Loader Trasfer Z 축이 Press 위치로 이동 하지 못하였습니다./n 티칭 위치를 확인 하여 주십시요. \n그리퍼 핑거와 얼라이너 간섭이 있을수 있습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);

            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.LD_Transfer_MAlignerVacuumOn_Timeout;
            alarm.Title = "Loader Trasfer";
            alarm.Cause = "Loader Trasfer MAligner 진공이 형성 되지 않았습니다. 자재 밴딩 또는 배큠 압력을 확인 바랍니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);


            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.LD_Transfer_MAlignerVacuumOff_Timeout;
            alarm.Title = "Loader Trasfer";
            alarm.Cause = "Loader Trasfer MAligner 진공이 해제 되지 않았습니다. \n공압솔레노이드 벨브나 IO 모듈 릴레이 점검이 필요 합니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);


            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.LD_Transfer_MAlignerXY_Move_Timeout;
            alarm.Title = "Loader Trasfer";
            alarm.Cause = "Loader Trasfer MAligner XY 축이 얼라인 위치로 이동 하지 못하였습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);


            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.LD_Transfer_MAlignerPickUp_Timeout;
            alarm.Title = "Loader Trasfer";
            alarm.Cause = "Loader Trasfer MAligner PickUp 동작이 Timeout 되었습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);


            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.LD_Transfer_WorkStageVacuumOn_Timeout;
            alarm.Title = "Loader Trasfer";
            alarm.Cause = "Loader Trasfer WorkStage 진공이 형성 되지 않았습니다. 자재 밴딩 또는 배큠 압력을 확인 바랍니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);


            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.LD_Transfer_WorkStage_Is_Working;
            alarm.Title = "Loader Trasfer";
            alarm.Cause = "Loader Trasfer WorkStage가 동작 중입니다. \nWorkStage가 동작 중일 때는 트랜스퍼를 동작 시킬 수 없습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);


            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.LD_Transfer_LaserDrilling_Is_Working;
            alarm.Title = "Loader Trasfer";
            alarm.Cause = "Workstage가 가공 동작 중입니다. \n레이저 가공 동작 중일 때는 트랜스퍼를 동작 시킬 수 없습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);


            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.LD_Transfer_WorkStageMove_Timeout;
            alarm.Title = "Loader Trasfer";
            alarm.Cause = "Loader Trasfer 가 WorkStage 위치로 이동 하지 못하였습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);


            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.LD_TransferX_Move_LoadingPos_Timeout;
            alarm.Title = "Loader Trasfer";
            alarm.Cause = "Loader Trasfer X 축이 Loading 위치로 이동 하지 못하였습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);


            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.LD_TransferZ_Move_PutDownPos_Timeout;
            alarm.Title = "Loader Trasfer";
            alarm.Cause = "Loader Trasfer Z 축이 Workstage의 자재 로딩 위치로 이동 하지 못하였습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);


            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.LD_MAlignerXY_Move_Widely_Timeout;
            alarm.Title = "Loader Trasfer";
            alarm.Cause = "Loader Trasfer MAligner XY 축이 얼라인 위치로 이동 하지 못하였습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);

            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.LD_Transfer_PickerVacuumOff_MAlignerVacuumOn_Timeout;
            alarm.Title = "Loader Trasfer";
            alarm.Cause = "Loader Trasfer 피커의 진공이 해제 되지 않았거나 얼라이너의 진공이 형성 되지 않았습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);


            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.LD_Maligner_Not_Set_Module_Size;
            alarm.Title = "Loader Trasfer";
            alarm.Cause = "Loader Trasfer 자재 사이즈가 올바르지 않습니다. 레시피를 확인 하여 주십시요.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);


            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.LD_TransferZ_Move_VibrationPos_Timeout;
            alarm.Title = "Loader Trasfer";
            alarm.Cause = "Loader Trasfer Z축이 바이브레이션 이동을 하지 못하였습니다.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);


            alarm = new Alarm();
            alarm.Code = (int)AlarmKey.LD_Ionizer_Alarm;
            alarm.Title = "Loader 스태커";
            alarm.Cause = "Loader 스태커의 이오나이저가 알람상태입니다. 이오나이저 동작 상태를 확인하여 주십시오.";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);
        } 
        #endregion

        #region Variables

        // Loader Cycle 동작 Flag

        //  1. Port 동작 (Module PickUp 대기위치로 이동시키는 Cycle)s
        //  2. Port 에서 M-Aligner 로 Module 이동 Cycle
        //  3. M-Align Cycle
        //  4. M-Aligner 에서 Work Stage 로 Module 이동 Cycle

        public bool m_bLD_LPort_Complete { set; get; }                         //  Left Port 동작 완료 여부
        public bool m_bLD_RPort_Complete { set; get; }                         //  Right Port 동작 완료 여부
        public bool m_bLD_TR_ModulePickUp_LPort_Complete { set; get; }         //  L-Port Transfer Module Pick Up 동작 완료 여부
        public bool m_bLD_TR_ModulePickUp_RPort_Complete { set; get; }         //  R-Port Transfer Module Pick Up 동작 완료 여부
        public bool m_bLD_TR_ModulePutDown_MAligner_Complete { set; get; }     //  M-Aligner Transfer Module Put Down 동작 완료 여부
        public bool m_bLD_MAligner_Exist { set; get; }                         //  M-Aligner Module Exist
        public bool m_bLD_MAlign_Complete { set; get; }                        //  M-Aligner 동작 완료 여부
        public bool m_bLD_TR_ModulePickUp_MAligner_Complete { set; get; }      //  M-Aligner Transfer Module Pick Up 동작 완료 여부
        public bool m_bLD_WorkStage_LoadingComplete { set; get; }              //  Work Stage 로 Module Loading 완료 여부

        #endregion

        #region Field
        SettingParameterCollection PosParam_Loader;          //  2022. 04. 25.  SCH : 모터 위치 파라미터를 갖다쓰기 위해 선언해봄.
        //static Conveyor conveyor = new Conveyor("");            //  요거 다시해야 함. Conveyor.cs 에 정의된 변수에 접근할 수 있게... 어케 함? -_-
                                                                //  static 으로 선언하면 되긴 헌디.... 맞는건가 -_-
        public InterpolatorMotionFunction MC_Func = new InterpolatorMotionFunction();
        //public ACSSPiiPlusAxis ACS_Func = new ACSSPiiPlusAxis();


        //  다른 모듈에 접근하기 위함
        static WorkStage workStage;
        static Unloader unloader;
        protected Task m_taskTimer_LoaderWork_Tick = null;

        #endregion

        #region Property
        public LoaderConfig Config { set; get; }
        public LoaderParameterConfig ParamConfig { set; get; }
        public LoaderRecipe Recipe { set; get; }                

        public ZzxzxyStage Stage { set; get; }                         //  MSL SLD-200C, SLD-200U 

        //  레시피 변경 시 위치값을 갱신하기 위해
        public bool m_bParameterSetting_PosData_Reload { set; get; }            //  위치 데이터 다시 로드

        public bool m_bLog_1time;
        public bool m_bLog_1time2;

        //public bool ACS_Motion_isSimulationMode { set; get; }

        public System.Timers.Timer timer_LoaderWork;
        public bool m_btimer_LoaderWork_Stop;

        public bool m_bBlink;

        public bool m_bAlignVisionThread_Use;                                                       //  2022. 04. 08.  SCH : Align Vision 을 Thread 로 할지 말지?

        //  단일 동작 중 안전센서를 터치할 경우 모터 Stop
        public bool m_bInManualMoving_SafetySensor_Detected = false;

        //  Cycle 동작 중 안전센서를 터치할 경우 모터 Stop
        public bool m_bInCycleMoving_SafetySensor_Detected = false;

        //  Cycle 동작 중 엘리베이터 Z축 오버 토크가 발생할 경우 모터 Stop
        public bool m_bInCycleMoving_ElevZOverTorque_Detected = false;


        public LoaderParameter loaderParameter { set; get; }
        #endregion


        #region NewForm 을 위한 Teaching Position List 변수

        /// <summary>
        /// Config 에서 Teching Position List 가 추가되거나 삭제 되면 여기도 해줘야 함. (이 항목이 Position 배열의 Index 가 되기 때문에)
        /// </summary>
        /// 
        //  LD, UL 통합 Teaching Position List
        public enum LDUL_TeachingPosList : int
        {
            LD_RPort_ReadyPos = 0,
            LD_RPort_TopPos,
            LD_LPort_ReadyPos,
            LD_LPort_TopPos,
            LD_TR_SafetyPos,
            LD_TR_RPortPos,
            LD_TR_LPortPos,
            LD_TR_MAlignPos,
            LD_TR_WorkTablePos,
            MAligner_OpenPos,
            MAligner_ClosePos,
            MAligner_Gap100mmPos,
            UL_TR_SafetyPos,
            UL_TR_WorkTablePos,            
            UL_TR_FPortPos,
            UL_TR_RPortPos,
            UL_TR_LPortPos,
            UL_RPort_ReadyPos,
            UL_RPort_TopPos,
            UL_LPort_ReadyPos,
            UL_LPort_TopPos,
        }

        public struct stLDULAxesPos
        {
            public double LD_Transfer_X;                    //  Loader Transfer X
            public double LD_Transfer_Z;                    //  Loader Transfer Z
            public double LD_Stacker_Z0;                    //  Loader Right Port
            public double LD_Stacker_Z1;                    //  Loader Left Port
            public double MAligner_X;                       //  M-Aligner X
            public double MAligner_Y;                       //  M-Aligner Y
            public double UL_Transfer_X;                    //  Loader Transfer X
            public double UL_Transfer_Z;                    //  Loader Transfer Z
            public double UL_Stacker_Z0;                    //  Unloader Right Port
            public double UL_Stacker_Z1;                    //  Unloader Left Port
        }
        public stLDULAxesPos[] stLDULTeachingPos = new stLDULAxesPos[System.Enum.GetValues(typeof(LDUL_TeachingPosList)).Length];

        public struct stLDULMoveProperties
        {
            public int Fine_Accel;                          //  Fine Acceleration
            public int Fine_SettleDelay;                    //  Fine Settle Delay
            public int Coarse_Accel;                        //  Coarse Acceleration
            public int Coarse_SettleDelay;                  //  Coarse Settle Delay
        }
        public stLDULMoveProperties[] stLDULPosMoveProperties = new stLDULMoveProperties[System.Enum.GetValues(typeof(LDUL_TeachingPosList)).Length];

        #endregion



        public override void SetModuleScale(double dScaleX, double dScaleY, double dXaxisT, double dYaxisT, bool bInvertedX, bool bInvertedY)
        {
            //  요거 주석처리하면 안되는데... 이유가 뭘까

            throw new NotImplementedException();
        }

        #region Tick Count Check

        public int TickCount_MainCycle_Start { set; get; }
        public int TickCount_MainCycle_Current { set; get; }
        public int TickCount_MainCycle_Interval { set; get; }

        //System.Diagnostics.Stopwatch sw_DispenserMainCyc = new System.Diagnostics.Stopwatch();
        //System.Diagnostics.Stopwatch sw_DispenserSubCyc = new System.Diagnostics.Stopwatch();

        public enum TickType : int
        {
            TICK_HOME = 0,              //  0 : Initialize
            TICK_MAIN = 1,              //  1 : Main Cycle
            TICK_SUB = 2,               //  2 : Sub Cycle
            TICK_PAUSE = 3,             //  3 : Pause
            TICK_CHECK = 4,             //  4 : 체크용

            TICK_LDSZ0 = 5,             //  5 : Loader Stacker Z0
            TICK_LDSZ1 = 6,             //  6 : Loader Stacker Z1
            TICK_LDTR = 7,              //  7 : Loader Transfer

            TICK_ALIGN = 8,             //  8 : M-Align

            //TICK_LASER_INTERFACE = 3,   //  3 : Laser Interface Set
            //TICK_LASER_FOCUS = 4,       //  4 : Laser Focus Check Cycle
            //TICK_LASER_COMM = 5,        //  5 : Laser Comm. Cycle
            //TICK_POWERMETER_COMM = 6,   //  6 : Power Meter Comm. Cycle
            //TICK_ALIGN = 7,             //  7 : Wafer Align Cycle
            //TICK_RVA = 8,               //  8 : Beam Size Change Cycle
        }

        public int[,] TickCount_Cycle = new int[System.Enum.GetValues(typeof(TickType)).Length, 2];


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

        #region Initialize Cycle

        //  홈 실행 동작을 모듈에서 할지, Work Stage 에서 한꺼번에 할지 결정하고, 그에 따라 변수 및 함수를 정의한다.

        //public int m_nHomeStep { set; get; }                    //  Home Step
        //public int m_nHomeAxisCount { set; get; }
        //public bool m_bHomeOK { get; set; }
        //public enum Home_Step
        //{
        //    None = 0,
        //    Start,                                              //  시작

        //    //  알람이 발생한 축이 있을 경우, Servo Off --> Reset --> Servo On 해야 한다.
        //    AxisAlarmCheck,                                     //  서보 축 알람 체크
        //    AlarmAxisServoOff,                                  //  알람 축 서보 Off
        //    AlarmAxisServoOffCheck,                             //  알람 축 서보 Off 확인
        //    AlarmAxisAlarmResetOn,                              //  알람 축 리셋 신호 On
        //    AlarmAxisAlarmResetOff,                             //  알람 축 리셋 신호 Off (30ms delay 후 Off)
        //    AlarmAxisServoOn,                                   //  알람 축 서보 On
        //    AlarmAxisServoOnCheck,                              //  알람 축 서보 On 확인

        //    VisionY_HomeStart,                                  //  Vision Y 축 홈 실행
        //    VisionY_HomeCompleteCheck,                          //  Vision Y 축 홈 완료 체크

        //    VisionXZ_UVW_EZ_HomeStart,                          //  Vision XZ, UVW EZ 축 홈 실행
        //    VisionXZ_UVW_EZ_HomeCompleteCheck,                  //  Vision XZ, UVW EZ 축 홈 완료 체크

        //    ElevZ_Move_ReadyPos,                                //  Elev. Z 축, 대기 위치로 이동 
        //    ElevZ_Move_ReadyPos_DoneCheck,                      //  Elev. Z 축, 대기 위치로 이동 완료 확인

        //    UVW_Move_ReadyPos,                                  //  UVW 축, 대기 위치로 이동
        //    UVW_Move_ReadyPos_DoneCheck,                        //  UVW 축, 대기 위치로 이동 완료 체크

        //    VisionXYZ_Move_ReadyPos,                            //  Vision XYZ 축, 대기 위치로 이동 
        //    VisionXYZ_Move_ReadyPos_DoneCheck,                  //  Vision XYZ 축, 대기 위치로 이동 완료 확인

        //    Complete                                            //  완료
        //}

        #endregion


        #region Single Action (Stacker, Module Pickup Waiting Pos)

        public int m_nStacker0_ModulePickupWaitingPos_Step { set; get; }             //  Stacker, Module Pickup Waiting Position Step
        public int m_nStacker1_ModulePickupWaitingPos_Step { set; get; }             //  Stacker, Module Pickup Waiting Position Step


        public bool m_bStacker0_Run_byUser { set; get; }                            //  Stacker0 Module Pickup Waiting Position 동작 여부 (User 가 직접 동작 시키는 경우)
        public bool m_bStacker1_Run_byUser { set; get; }                            //  Stacker1 Module Pickup Waiting Position 동작 여부 (User 가 직접 동작 시키는 경우)


        //  자동 운전을 위한 변수
        public bool m_bStacker0_Exist { set; get; }                                 //  Stacker0 Module Exist
        public bool m_bStacker0_Complete { set; get; }                              //  Stacker0 동작 완료 여부 (Transfer 가 Stacker0 의 Module 을 PickUp 해도 되는지 확인하는 Flag)
        public bool m_bStacker1_Exist { set; get; }                                 //  Stacker1 Module Exist
        public bool m_bStacker1_Complete { set; get; }                              //  Stacker1 동작 완료 여부 (Transfer 가 Stacker1 의 Module 을 PickUp 해도 되는지 확인하는 Flag)


        public enum StackerModulePickupWaitingPos_Step
        {
            None = 0,

            Start,                                                          //  시작


            Process_Condition_Check,                                        //  동작 조건 체크 (Module Exist Sensor On Check, Transfer Cycle : None)


            //  Full Sensor 감지 상태일 경우 (Off 될 때 까지 내림 -> Off 되면 Stop -> 느리게 Up -> On 되면 Stop -> 느리게 Down -> Off 되면 Stop -> 더 느리게 Up -> On 되면 Stop -> 완료)
            StackerZ_MoveType1_FastDown,                                    //  Stacker Z 축, 빠르게 내림 (최 하단까지)
            StackerZ_MoveType1_FastDown_DoneCheck,                          //  Stacker Z 축, 빠르게 내림, 이동 완료 확인

            StackerZ_MoveType1_SlowUp,                                      //  Stacker Z 축, 느리게 올림 (최 상단까지)
            StackerZ_MoveType1_SlowUp_DoneCheck,                            //  Stacker Z 축, 느리게 올림, 이동 완료 확인

            StackerZ_MoveType1_Slow2Down,                                   //  Stacker Z 축, 더 느리게 내림 (최 하단까지)
            StackerZ_MoveType1_Slow2Down_DoneCheck,                         //  Stacker Z 축, 더 느리게 내림, 이동 완료 확인

            StackerZ_MoveType1_Slow3Up,                                     //  Stacker Z 축, 더더 느리게 올림 (최 상단까지)
            StackerZ_MoveType1_Slow3Up_DoneCheck,                           //  Stacker Z 축, 더더 느리게 올림, 이동 완료 확인


            //  Full Sensor 감지되지 않는 상태일 경우 (Up -> On 되면 Stop -> 느리게 Down -> Off 되면 Stop -> 더 느리게 Up -> On 되면 Stop -> 완료)
            StackerZ_MoveType2_FastUp,                                      //  Stacker Z 축, 빠르게 올림 (최 상단까지)
            StackerZ_MoveType2_FastUp_DoneCheck,                            //  Stacker Z 축, 빠르게 올림, 이동 완료 확인

            StackerZ_MoveType2_Slow2Down,                                   //  Stacker Z 축, 더 느리게 내림 (최 하단까지)
            StackerZ_MoveType2_Slow2Down_DoneCheck,                         //  Stacker Z 축, 더 느리게 내림, 이동 완료 확인

            StackerZ_MoveType2_Slow3Up,                                     //  Stacker Z 축, 더더 느리게 올림 (최 상단까지)
            StackerZ_MoveType2_Slow3Up_DoneCheck,                           //  Stacker Z 축, 더더 느리게 올림, 이동 완료 확인


            //  Full Sensor 감지 후 추가 이동
            StackerZ_Move_OverDistance,                                     //  Stacker Z 축, 최종 감지 위치에서 추가로 이동
            StackerZ_Move_OverDistance_DoneCheck,                           //  Stacker Z 축, 최종 감지 위치에서 추가로 이동 완료 확인


            Complete                                                        //  완료
        }
        #endregion


        #region Single Action (M_Aligner)

        public bool m_bMAlign_Complete { set; get; }                        //  M-Aligner Align 완료 여부
        public bool m_bMAlign_Retry { set; get; }                           //  M-Aligner Align 재시도 여부
        public double m_dMAlign_ModuleSize_Width { set; get; }              //  M-Aligner Module Size Width
        public double m_dMAlign_ModuleSize_Height { set; get; }             //  M-Aligner Module Size Height
        public double m_dMAlign_CalculatedModuleSize_ALN_X { set; get; }    //  계산된 M-Aligner Module Size Width 에 해당하는 ALN_X 위치
        public double m_dMAlign_CalculatedModuleSize_ALN_Y { set; get; }    //  계산된 M-Aligner Module Size Height 에 해당하는 ALN_Y 위치
        public bool m_bMAlignZone_ModuleExist { set; get; }                 //  M-Aligner Zone Exist
        public int m_nMAlign_Step { set; get; }                             //  Mechanical Align Step
        public int m_nMAlign_Step_Recovery { set; get; }                             //  Mechanical Align Step


        public enum MAlign_Step
        {
            None = 0,
            Start,                                                          //  시작
            Process_Condition_Check,                                        //  동작 조건 체크 (Transfer Cycle : None, Transfer 가 Module 을 갖다 놓았는지 확인하는 Flag : True, Module Size > 0)
            MAligner_MoveXY_Widely,                                         //  MAligner XY 축, 넓힘.
            MAligner_MoveXY_Widely_DoneCheck,                               //  MAligner XY 축, 넓힘 완료 확인.


            MAligner_ModuleVacuum_On,                                       //  Module Vacuum On
            MAligner_ModuleVacuum_OnCheck,                                  //  Module Vacuum On 확인

            MAligner_MoveXY_Narrowly,                                       //  MAligner XY 축, 좁힘.
            MAligner_MoveXY_Narrowly_DoneCheck,                             //  MAligner XY 축, 좁힘 완료 확인.

            MAligner_MoveXY_LittleWidely,                                   //  MAligner XY 축, 약간 넓힘.
            MAligner_MoveXY_LittleWidely_DoneCheck,                         //  MAligner XY 축, 약간 넓힘 완료 확인.


            Complete                                                        //  완료
        }

        #endregion


        #region Single Action (Loader Transfer)


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        ///     Stop -> Start 시 동작 Sequence 를 재설정 하기 위한 변수
        /// 
        /// </summary>
        /// 
        public int m_nLoader_Transfer_Restart_MoveType { set; get; }                        //  어떤 작업을 하다가 멈춘 것인지?
                                                                                            //      1. Stacker0 에서 Module Pick Up
                                                                                            //      2. Stacker1 에서 Module Pick Up
                                                                                            //      3. M-Aligner 에서 Module Pick Up
                                                                                            //      4. M-Aligner 에 Module Put Down
                                                                                            //      5. Work Stage 에 Module Put Down
        public int m_nLD_RESTORE_Transfer_Step { set; get; } = 0;
        public int m_nLD_RESTORE_Transfer_MoveType { set; get; } = 0;
        public bool m_bLD_RESTORE_Transfer_toWorkStage_Module_PutDown_Complete_Flag { set; get; } = false;      //  Work Stage 에 Module Put Down 완료 여부
        public bool m_bLD_RESTORE_Transfer_fromStacker0_Module_PickUp_Complete_Flag { set; get; } = false;      //  Stacker0 에서 Module Pick Up 완료 여부
        public bool m_bLD_RESTORE_Transfer_fromStacker1_Module_PickUp_Complete_Flag { set; get; } = false;      //  Stacker1 에서 Module Pick Up 완료 여부
        public bool m_bLD_RESTORE_Transfer_fromMAligner_Module_PickUp_Complete_Flag { set; get; } = false;      //  M-Aligner 에서 Module Pick Up 완료 여부
        public bool m_bLD_RESTORE_Transfer_toMAligner_Module_PutDown_Complete_Flag { set; get; } = false;       //  M-Aligner 에 Module Put Down 완료 여부
        public int m_nLD_RESTORE_MainWork_Cycle_Step { set; get; } = 0;
        public int m_nLD_RESTORE_DryRun_Cycle_Step { set; get; } = 0;
        public int m_nLD_RESTORE_LaserDrilling_Cycle_Step { set; get; } = 0;
        public bool m_bLD_RESTORE_MainWork_Cycle_Complete { set; get; } = false;

        public bool m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePickUpfromStacker0_Complete { set; get; }       //  Stacker0 에서 Module Pick Up 완료 여부
        public bool m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePickUpfromStacker1_Complete { set; get; }       //  Stacker1 에서 Module Pick Up 완료 여부
        public bool m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePickUpfromMAligner_Complete { set; get; }       //  M-Aligner 에서 Module Pick Up 완료 여부
        public bool m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePutDowntoMAligner_Complete { set; get; }        //  M-Aligner 에 Module Put Down 완료 여부
        public bool m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePutDowntoWorkStage_Complete { set; get; }       //  Work Stage 에 Module Put Down 완료 여부

        /// <summary>
        /// 
        ///     Stop -> Start 시 동작 Sequence 를 재설정 하기 위한 변수
        /// 
        /// </summary>
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        //  자동 운전을 위한 변수
        public bool m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker0_Complete { set; get; }              //  Stacker0 에서 Module Pick Up 완료 여부
        public bool m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker1_Complete { set; get; }              //  Stacker1 에서 Module Pick Up 완료 여부
        public bool m_bAUTORUN_Loader_Transfer_ModulePickUpfromMAligner_Complete { set; get; }              //  M-Aligner 에서 Module Pick Up 완료 여부
        public bool m_bAUTORUN_Loader_Transfer_ModulePutDowntoMAligner_Complete { set; get; }               //  M-Aligner 에 Module Put Down 완료 여부
        public bool m_bAUTORUN_Loader_Transfer_ModulePutDowntoWorkStage_Complete { set; get; }              //  Work Stage 에 Module Put Down 완료 여부


        public int m_nLoader_Transfer_Step { set; get; }                    //  Transfer Step
        public int m_nLoader_Transfer_Step_Recovery { set; get; }           //  Transfer Step (복구용)
        public int m_nLoaderTransferMoveType { set; get; }                  //  Transfer Move Type
        public bool m_bLD_Transfer_fromStacker0_Module_PickUp_Complete_Flag { set; get; } = false;          //  Stacker0 에서 Module Pick Up 완료 여부
        public bool m_bLD_Transfer_fromStacker1_Module_PickUp_Complete_Flag { set; get; } = false;          //  Stacker1 에서 Module Pick Up 완료 여부
        public bool m_bLD_Transfer_fromMAligner_Module_PickUp_Complete_Flag { set; get; } = false;          //  M-Aligner 에서 Module Pick Up 완료 여부
        public bool m_bLD_Transfer_toWorkStage_Module_PutDown_Complete_Flag { set; get; } = false;          //  Work Stage 에 Module Put Down 완료 여부
        public bool m_bLD_Transfer_toMAligner_Module_PutDown_Complete_Flag { set; get; } = false;           //  M-Aligner 에 Module Put Down 완료 여부

        public int m_nLoader_Transfer_Vibration_Count { set; get; } = 0;                  //  Module 털기 횟수


        public int m_nStacker0_StepUp_Count { set; get; } = 0;          //  max 10mm
        public int m_nStacker1_StepUp_Count { set; get; } = 0;          //  max 10mm

        public enum LoaderTransferMoveType : int
        {
            Cycle_None = -1,

            Cycle_Transfer_ReadyPos = 0,                                    //  Transfer Ready Position

            Cycle_Stacker0_PickUp,                                          //  Module Pick Up Cycle (Stacker 0)
            Cycle_Stacker1_PickUp,                                          //  Module Pick Up Cycle (Stacker 1)
            Cycle_MAligner_PickUp,                                          //  Module Pick Up Cycle (M-Aligner)

            Cycle_MAligner_PutDown,                                         //  Module Put Down Cycle (M-Aligner)
            Cycle_WorkStage_PutDown,                                        //  Module Put Down Cycle (Work Stage)
        }

        public int m_nLoaderTransfer_ProcessStep { set; get; }              //  Transfer Process Step
        public enum LoaderTransferProcessStep : int
        {
            LoaderStep_None = -1,

            LoaderStep_ModulePickup_fromStacker,                            //  Module Pick Up Cycle (from Stacker 0 or 1)
            LoaderStep_ModulePutDown_MAligner,                              //  Module Put Down Cycle (to M-Aligner)
            LoaderStep_ModulePickUp_MAligner,                               //  Module Pick Up Cycle (from M-Aligner)
            LoaderStep_ModulePutDown_Stage,                                 //  Module Put Down Cycle (to Work Stage)
        }

        public enum Loader_Transfer_Step
        {
            None = 0,

            Start,                                                          //  시작


            Process_Type_Check,                                             //  동작 타입 체크 (Stacker 에서 Module PickUp, M-Aligner 에서 Module PickUp, Work Stage 로 Module PutDown, M-Aligner 로 Module PutDown)


            /// <summary>
            /// Transfer 대기 위치로 이동 - 시작
            /// </summary>
            Transfer_Move_Condition_Check,                                  //  Stacker0 에서 Module Pick Up 조건 체크 (Stacker0 Module Exist Sensor On, Stacker0 Module Full Sensor On, Transfer Picker Vacuum Off Check, Stacker0 Cycle : None)

            TransferZ_Move_ReadyPos,                                        //  Transfer Z 축, 대기 위치로 이동
            TransferZ_Move_ReadyPos_DoneCheck,                              //  Transfer Z 축, 대기 위치로 이동 완료 확인

            TransferX_Move_ReadyPos,                                        //  Transfer X 축, 대기 위치로 이동
            TransferX_Move_ReadyPos_DoneCheck,                              //  Transfer X 축, 대기 위치로 이동 완료 확인
            /// <summary>
            /// Transfer 대기 위치로 이동 - 완료
            /// </summary>


            /// <summary>
            /// Stacker0 에서 Module Pick Up - 시작
            /// </summary>
            Stacker0_ModulePickup_Condition_Check,                          //  Stacker0 에서 Module Pick Up 조건 체크 (Stacker0 Module Exist Sensor On, Stacker0 Module Full Sensor On, Transfer Picker Vacuum Off Check, Stacker0 Cycle : None)

            Stacker0PickUp_TransferZ_Move_ReadyPos,                         //  Transfer Z 축, 대기 위치로 이동
            Stacker0PickUp_TransferZ_Move_ReadyPos_DoneCheck,               //  Transfer Z 축, 대기 위치로 이동 완료 확인

            Stacker0PickUp_TransferX_Move_StackerPos,                       //  Transfer X 축, Stacker 위치로 이동
            Stacker0PickUp_TransferX_Move_StackerPos_DoneCheck,             //  Transfer X 축, Stacker 위치로 이동 완료 확인

            Stacker0PickUp_TransferZ_Move_PickUpPos_1stStep,                //  Transfer Z 축, Module Pick Up 위치로 이동 (1단계, 최종 위치에서 위로 10 mm)
            Stacker0PickUp_TransferZ_Move_PickUpPos_1stStep_DoneCheck,      //  Transfer Z 축, Module Pick Up 위치로 이동 완료 확인

            Stacker0PickUp_TransferZ_Move_PickUpPos_2ndStep,                //  Transfer Z 축, Module Pick Up 위치로 이동 (2단계, 최종 위치)
            Stacker0PickUp_TransferZ_Move_PickUpPos_2ndStep_DoneCheck,      //  Transfer Z 축, Module Pick Up 위치로 이동 완료 확인
            
            Stacker0PickUp_Transfer_PickerVacuum_On,                        //  Transfer, Module Picker Vacuum On
            Stacker0PickUp_Transfer_PickerVacuum_OnCheck,                   //  Transfer, Module Picker Vacuum On 확인

            Stacker0PickUp_Transfer_Picker_StepUp,                          //  Transfer, Stacker0 Step Up
            Stacker0PickUp_Transfer_Picker_StepUp_DoneCheck,                //  Transfer, Stacker0 Step Up 완료 확인

            Stacker0PickUp_TransferZ_Move_ReadyPos2_1stStep,                //  Transfer Z 축, 대기 위치로 이동 (1단계, 현재 위치에서 위로 10 mm)
            Stacker0PickUp_TransferZ_Move_ReadyPos2_1stStep_DoneCheck,      //  Transfer Z 축, 대기 위치로 이동 완료 확인

            //  모듈 털기
            Stacker0PickUp_TransferZ_Vibration_Start,                       //  Transfer Z 축, 모듈 털기 시작
            Stacker0PickUp_TransferZ_VibrationMove_DownPos,                 //  Transfer Z 축, 아래로 2mm 이동
            Stacker0PickUp_TransferZ_VibrationMove_DownPos_DoneCheck,       //  Transfer Z 축, 아래로 2mm 이동 완료 확인
            Stacker0PickUp_TransferZ_VibrationMove_UpPos,                   //  Transfer Z 축, 위로 2mm 이동
            Stacker0PickUp_TransferZ_VibrationMove_UpPos_DoneCheck,         //  Transfer Z 축, 위로 2mm 이동 완료 확인

            Stacker0PickUp_TransferZ_Move_ReadyPos2_2ndStep,                //  Transfer Z 축, 대기 위치로 이동 (2단계, 최종 위치)
            Stacker0PickUp_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck,      //  Transfer Z 축, 대기 위치로 이동 완료 확인
            /// <summary>
            /// Stacker0 에서 Module Pick Up - 완료
            /// </summary>


            /// <summary>
            /// Stacker1 에서 Module Pick Up - 시작
            /// </summary>
            Stacker1_ModulePickup_Condition_Check,                          //  Stacker1 에서 Module Pick Up 조건 체크 (Stacker1 Module Exist Sensor On, Stacker1 Module Full Sensor On, Transfer Picker Vacuum Off Check, Stacker1 Cycle : None)

            Stacker1PickUp_TransferZ_Move_ReadyPos,                         //  Transfer Z 축, 대기 위치로 이동
            Stacker1PickUp_TransferZ_Move_ReadyPos_DoneCheck,               //  Transfer Z 축, 대기 위치로 이동 완료 확인

            Stacker1PickUp_TransferX_Move_StackerPos,                       //  Transfer X 축, Stacker 위치로 이동
            Stacker1PickUp_TransferX_Move_StackerPos_DoneCheck,             //  Transfer X 축, Stacker 위치로 이동 완료 확인

            Stacker1PickUp_TransferZ_Move_PickUpPos_1stStep,                //  Transfer Z 축, Module Pick Up 위치로 이동 (1단계, 최종 위치에서 위로 10 mm)
            Stacker1PickUp_TransferZ_Move_PickUpPos_1stStep_DoneCheck,      //  Transfer Z 축, Module Pick Up 위치로 이동 완료 확인

            Stacker1PickUp_TransferZ_Move_PickUpPos_2ndStep,                //  Transfer Z 축, Module Pick Up 위치로 이동 (2단계, 최종 위치)
            Stacker1PickUp_TransferZ_Move_PickUpPos_2ndStep_DoneCheck,      //  Transfer Z 축, Module Pick Up 위치로 이동 완료 확인

            Stacker1PickUp_Transfer_PickerVacuum_On,                        //  Transfer, Module Picker Vacuum On
            Stacker1PickUp_Transfer_PickerVacuum_OnCheck,                   //  Transfer, Module Picker Vacuum On 확인

            Stacker1PickUp_Transfer_Picker_StepUp,                          //  Transfer, Stacker1 Step Up
            Stacker1PickUp_Transfer_Picker_StepUp_DoneCheck,                //  Transfer, Stacker1 Step Up 완료 확인

            Stacker1PickUp_TransferZ_Move_ReadyPos2_1stStep,                //  Transfer Z 축, 대기 위치로 이동 (1단계, 현재 위치에서 위로 10 mm)
            Stacker1PickUp_TransferZ_Move_ReadyPos2_1stStep_DoneCheck,      //  Transfer Z 축, 대기 위치로 이동 완료 확인

            //  모듈 털기
            Stacker1PickUp_TransferZ_Vibration_Start,                       //  Transfer Z 축, 모듈 털기 시작
            Stacker1PickUp_TransferZ_VibrationMove_DownPos,                 //  Transfer Z 축, 아래로 2mm 이동
            Stacker1PickUp_TransferZ_VibrationMove_DownPos_DoneCheck,       //  Transfer Z 축, 아래로 2mm 이동 완료 확인
            Stacker1PickUp_TransferZ_VibrationMove_UpPos,                   //  Transfer Z 축, 위로 2mm 이동
            Stacker1PickUp_TransferZ_VibrationMove_UpPos_DoneCheck,         //  Transfer Z 축, 위로 2mm 이동 완료 확인

            Stacker1PickUp_TransferZ_Move_ReadyPos2_2ndStep,                //  Transfer Z 축, 대기 위치로 이동 (2단계, 최종 위치)
            Stacker1PickUp_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck,      //  Transfer Z 축, 대기 위치로 이동 완료 확인
            /// <summary>
            /// Stacker1 에서 Module Pick Up - 완료
            /// </summary>


            /// <summary>
            /// M-Aligner 에서 Module Pick Up - 시작
            /// </summary>
            MAligner_ModulePickup_Condition_Check,                          //  M-Aligner 에서 Module Pick Up 조건 체크 (M-Align 완료, M-Aligner Vacuum On Check, Transfer Cycle : None, M-Align Cycle : None)

            MAligner_MAlign_NotComplete,                                    //  M-Aligner 에서 Align 이 완료되지 않은 상태일 경우 (M-Align Cyc. Call)
            MAligner_MAlign_Start,                                          //  M-Aligner 에서 Align 시작
            MAligner_MAlign_CompleteCheck,                                  //  M-Aligner 에서 Align 완료 확인 (M-Aligner Vacuum 이 Off 이면 Pick Up 하지 않음 --> 자재가 없다고 판단)

            MAlignerPickUp_TransferZ_Move_ReadyPos,                         //  Transfer Z 축, 대기 위치로 이동
            MAlignerPickUp_TransferZ_Move_ReadyPos_DoneCheck,               //  Transfer Z 축, 대기 위치로 이동 완료 확인

            MAlignerPickUp_TransferX_Move_MAlignerPos,                      //  Transfer X 축, M-Aligner 위치로 이동
            MAlignerPickUp_TransferX_Move_MAlignerPos_DoneCheck,            //  Transfer X 축, M-Aligner 위치로 이동 완료 확인


                //  Pick Up 실패했을때의 Retry 코드
                MAlignerPickUp_Retry_Start,                                           //  Retry Start

                MAlignerPickUp_Retry_Transfer_PickerVacuum_Off,                       //  Transfer, Module Picker Vacuum Off
                MAlignerPickUp_Retry_Transfer_PickerVacuum_OffCheck,                  //  Transfer, Module Picker Vacuum Off 확인

                MAlignerPickUp_Retry_TransferZ_Move_PickUpPos_1stStep,                //  Transfer Z 축, Module Pick Up 위치로 이동 (1단계, 최종 위치에서 위로 10 mm)
                MAlignerPickUp_Retry_TransferZ_Move_PickUpPos_1stStep_DoneCheck,      //  Transfer Z 축, Module Pick Up 위치로 이동 완료 확인

                MAlignerPickUp_Retry_TransferZ_Move_PickUpPos_2ndStep,                //  Transfer Z 축, Module Pick Up 위치로 이동 (2단계, 최종 위치)
                MAlignerPickUp_Retry_TransferZ_Move_PickUpPos_2ndStep_DoneCheck,      //  Transfer Z 축, Module Pick Up 위치로 이동 완료 확인

                MAlignerPickUp_Retry_MAligner_Vacuum_On,                              //  M-Aligner, Vacuum On
                MAlignerPickUp_Retry_MAligner_Vacuum_OnCheck,                         //  M-Aligner, Vacuum On 확인

                MAlignerPickUp_Retry_TransferZ_Move_ReadyPos2_1stStep,                //  Transfer Z 축, 대기 위치로 이동 (1단계, 현재 위치에서 Aligner Pusher 를 벗어나는 높이까지)
                MAlignerPickUp_Retry_TransferZ_Move_ReadyPos2_1stStep_DoneCheck,      //  Transfer Z 축, 대기 위치로 이동 완료 확인 (여기서 Transfer Picker 의 Vacuum On 과 Aligner 의 Vacuum Off 를 동시에 확인)

                MAlignerPickUp_Retry_TransferZ_Move_ReadyPos2_2ndStep,                //  Transfer Z 축, 대기 위치로 이동 (2단계, 최종 위치)
                MAlignerPickUp_Retry_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck,      //  Transfer Z 축, 대기 위치로 이동 완료 확인

                MAligner_Retry_MAlign_Start,                                          //  M-Aligner 에서 Align 시작
                MAligner_Retry_MAlign_CompleteCheck,                                  //  M-Aligner 에서 Align 완료 확인 (M-Aligner Vacuum 이 Off 이면 Pick Up 하지 않음 --> 자재가 없다고 판단)

                MAlignerPickUp_Retry_Complete,                                        //  Retry Start


            MAlignerPickUp_TransferZ_Move_PickUpPos_1stStep,                //  Transfer Z 축, Module Pick Up 위치로 이동 (1단계, 최종 위치에서 위로 10 mm)
            MAlignerPickUp_TransferZ_Move_PickUpPos_1stStep_DoneCheck,      //  Transfer Z 축, Module Pick Up 위치로 이동 완료 확인

            MAlignerPickUp_TransferZ_Move_PickUpPos_2ndStep,                //  Transfer Z 축, Module Pick Up 위치로 이동 (2단계, 최종 위치)
            MAlignerPickUp_TransferZ_Move_PickUpPos_2ndStep_DoneCheck,      //  Transfer Z 축, Module Pick Up 위치로 이동 완료 확인

            MAlignerPickUp_Transfer_PickerVacuum_On,                        //  Transfer, Module Picker Vacuum On
            MAlignerPickUp_Transfer_PickerVacuum_OnCheck,                   //  Transfer, Module Picker Vacuum On 확인
            MAlignerPickUp_MAligner_Vacuum_Off,                             //  M-Aligner, Vacuum Off
            MAlignerPickUp_MAligner_Vacuum_OffCheck,                        //  M-Aligner, Vacuum Off Check

                MAlignerPickUp_MAlignerXY_MoveType1_Widely,                 //  M-Aligner XY 축, Module 을 들어올리기 위해 열어주는 위치로 이동 (1mm 정도) - Type #1 or #2 둘 중에 하나만 사용
                MAlignerPickUp_MAlignerXY_MoveType1_Widely_DoneCheck,       //  M-Aligner XY 축, Module 을 들어올리기 위해 열어주는 위치로 이동 완료 확인

            MAlignerPickUp_TransferZ_Move_ReadyPos2_1stStep,                //  Transfer Z 축, 대기 위치로 이동 (1단계, 현재 위치에서 Aligner Pusher 를 벗어나는 높이까지)
            MAlignerPickUp_TransferZ_Move_ReadyPos2_1stStep_DoneCheck,      //  Transfer Z 축, 대기 위치로 이동 완료 확인 (여기서 Transfer Picker 의 Vacuum On 과 Aligner 의 Vacuum Off 를 동시에 확인)

                MAlignerPickUp_MAlignerXY_MoveType2_Widely,                 //  M-Aligner XY 축, Module 을 들어올린 후 열어주는 위치로 이동 (1mm 정도) - Type #2 or #1 둘 중에 하나만 사용
                MAlignerPickUp_MAlignerXY_MoveType2_Widely_DoneCheck,       //  M-Aligner XY 축, Module 을 들어올린 후 열어주는 위치로 이동 완료 확인

            MAlignerPickUp_TransferZ_Move_ReadyPos2_2ndStep,                //  Transfer Z 축, 대기 위치로 이동 (2단계, 최종 위치)
            MAlignerPickUp_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck,      //  Transfer Z 축, 대기 위치로 이동 완료 확인
            /// <summary>
            /// M-Aligner 에서 Module Pick Up - 완료
            /// </summary>


            /// <summary>
            /// Module 을 Work Stage 에 Put Down - 시작
            /// </summary>
            WorkStage_ModulePutdown_Condition_Check,                        //  Work Stage 에 Module Put Down 조건 체크 (Transfer Picker Vacuum On Check, Stage Vacuum Off Check, Drilling Cycle : None)

            WorkStagePutDown_TransferZ_Move_ReadyPos,                       //  Transfer Z 축, 대기 위치로 이동
            WorkStagePutDown_TransferZ_Move_ReadyPos_DoneCheck,             //  Transfer Z 축, 대기 위치로 이동 완료 확인

            WorkStagePutDown_WorkStageCycle_LoadingPos_Start,               //  Work Stage, Loading 위치로 이동 Cycle 시작
            WorkStagePutDown_WorkStageCycle_LoadingPos_CompleteCheck,       //  Work Stage, Loading 위치로 이동 Cycle 완료 체크

            WorkStagePutDown_TransferX_Move_LoadingPos,                     //  Transfer X 축, Work Stage Loading 위치로 이동
            WorkStagePutDown_TransferX_Move_LoadingPos_DoneCheck,           //  Transfer X 축, Work Stage Loading 위치로 이동 완료 확인 (Work Stage Loading 위치로 이동 Cycle 완료 확인 후, Transfer X 이동 완료 확인)

            WorkStagePutDown_TransferZ_Move_PutDownPos_1stStep,             //  Transfer Z 축, Module Put Down 위치로 이동 (1단계, 최종 위치에서 위로 10 mm)
            WorkStagePutDown_TransferZ_Move_PutDownPos_1stStep_DoneCheck,   //  Transfer Z 축, Module Put Down 위치로 이동 완료 확인

            WorkStagePutDown_TransferZ_Move_PutDownPos_2ndStep,             //  Transfer Z 축, Module Put Down 위치로 이동 (2단계, 최종 위치)
            WorkStagePutDown_TransferZ_Move_PutDownPos_2ndStep_DoneCheck,   //  Transfer Z 축, Module Put Down 위치로 이동 완료 확인

            WorkStagePutDown_WorkStage_Vacuum_On,                           //  Work Stage, Vacuum On
            WorkStagePutDown_Transfer_PickerVacuum_Off,                     //  Transfer, Module Picker Vacuum Off (and Blow On)
            WorkStagePutDown_Transfer_PickerVacuum_OffCheck,                //  Transfer, Module Picker Vacuum Off 확인 (and Work Stage Vacuum On 확인)

            WorkStagePutDown_TransferZ_Move_ReadyPos2_1stStep,              //  Transfer Z 축, 대기 위치로 이동 (1단계, 현재 위치에서 위로 10 mm)
            WorkStagePutDown_TransferZ_Move_ReadyPos2_1stStep_DoneCheck,    //  Transfer Z 축, 대기 위치로 이동 완료 확인 (then Picker Blow Off)

            WorkStagePutDown_TransferZ_Move_ReadyPos2_2ndStep,              //  Transfer Z 축, 대기 위치로 이동 (2단계, 최종 위치)
            WorkStagePutDown_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck,    //  Transfer Z 축, 대기 위치로 이동 완료 확인

            WorkStagePutDown_WorkStage_VacuumCheck,                         //  Work Stage 의 공압이 확인되지 않으면 다시 로딩할 수 있도록 한다.
            /// <summary>
            /// Module 을 Work Stage 에 Put Down - 완료
            /// </summary>


            /// <summary>
            /// Module 을 M-Aligner 에 Put Down - 시작
            /// </summary>
            MAligner_ModulePutdown_Condition_Check,                         //  M-Aligner 에 Module Put Down 조건 체크 (Transfer Picker Vacuum On Check, M-Aligner Vacuum Off Check, M-Align Cycle : None)

            MAlignerPutDown_TransferZ_Move_ReadyPos,                        //  Transfer Z 축, 대기 위치로 이동
            MAlignerPutDown_TransferZ_Move_ReadyPos_DoneCheck,              //  Transfer Z 축, 대기 위치로 이동 완료 확인

            MAlignerPutDown_MAlignerXY_Move_Widely,                         //  M-Aligner XY 축, Module 을 내려놓을 수 있을만큼 넓히기
            MAlignerPutDown_MAlignerXY_Move_Widely_DoneCheck,               //  M-Aligner XY 축, Module 을 내려놓을 수 있을만큼 넓히기 완료 확인
            MAlignerPutDown_TransferX_Move_MAlignPos,                       //  Transfer X 축, M-Aligner Put Down 위치로 이동
            MAlignerPutDown_TransferX_Move_MAlignPos_DoneCheck,             //  Transfer X 축, M-Aligner Put Down 위치로 이동 완료 확인 (M-Aligner XY 축이 넓어진 후 이동 완료 확인)

            MAlignerPutDown_TransferZ_Move_PutDownPos_1stStep,              //  Transfer Z 축, Module Put Down 위치로 이동 (1단계, 최종 위치에서 위로 10 mm)
            MAlignerPutDown_TransferZ_Move_PutDownPos_1stStep_DoneCheck,    //  Transfer Z 축, Module Put Down 위치로 이동 완료 확인

            MAlignerPutDown_TransferZ_Move_PutDownPos_2ndStep,              //  Transfer Z 축, Module Put Down 위치로 이동 (2단계, 최종 위치)
            MAlignerPutDown_TransferZ_Move_PutDownPos_2ndStep_DoneCheck,    //  Transfer Z 축, Module Put Down 위치로 이동 완료 확인

            MAlignerPutDown_MAligner_Vacuum_On,                             //  M-Aligner, Vacuum On
            MAlignerPutDown_Transfer_PickerVacuum_Off,                      //  Transfer, Module Picker Vacuum Off (and Blow On)
            MAlignerPutDown_Transfer_PickerVacuum_OffCheck,                 //  Transfer, Module Picker Vacuum Off 확인 (and M-Aligner Vacuum On 확인)

            MAlignerPutDown_TransferZ_Move_ReadyPos2_1stStep,               //  Transfer Z 축, 대기 위치로 이동 (1단계, 현재 위치에서 위로 10 mm)
            MAlignerPutDown_TransferZ_Move_ReadyPos2_1stStep_DoneCheck,     //  Transfer Z 축, 대기 위치로 이동 완료 확인 (then Picker Blow Off)

            MAlignerPutDown_TransferZ_Move_ReadyPos2_2ndStep,               //  Transfer Z 축, 대기 위치로 이동 (2단계, 최종 위치)
            MAlignerPutDown_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck,     //  Transfer Z 축, 대기 위치로 이동 완료 확인

            MAlignerPutDown_PickerVacuum_MAlignerVacuum_Check,              //  Transfer Picker Vacuum On Check, M-Aligner Vacuum Off Check

            MAlignerPutDown_MAlign_Start,                                   //  M-Aligner 에서 Align 시작
            MAlignerPutDown_MAlign_CompleteCheck,                           //  M-Aligner 에서 Align 완료 확인 (M-Aligner Vacuum 이 Off 이면 Pick Up 하지 않음 --> 자재가 없다고 판단)
            /// <summary>
            /// Module 을 M-Aligner 에 Put Down - 완료
            /// </summary>


            Complete                                                        //  완료
        }
        #endregion


        #region Constructor
        public Loader(string strName) : base(strName)
        {
            bool ret = true;

            ParamConfig = new LoaderParameterConfig();
            Config = new LoaderConfig();
            //SetDispenserWork((int)DispenserWorkStatus.WORK_NONE);

            //ModuleCollection m_collectionModules;
            //m_collectionModules = Equipment.Modules;

            //foreach (Module module in m_collectionModules)
            //{
            //    if (module.Name == "WorkStage")
            //    {
            //        workStage = module as WorkStage;
            //    }

            //    if (module.Name == "Unloader")
            //    {
            //        unloader = module as Unloader;
            //    }

            //    //if (module.Name == "BDS")
            //    //{
            //    //    bds = module as Bds;
            //    //}

            //    //if (module.Name == "Vision")
            //    //{
            //    //    vision = module as Vision;
            //    //}
            //}

            //WorkStageIndex = -1;
            //m_bLaserGetStatus_Run = false;

            //Cepheus_laser = new MyCepheusLaser();

            //m_nHomeStep = (int)Home_Step.None;
            m_nLoaderTransferMoveType = (int)LoaderTransferMoveType.Cycle_None; //  Transfer Move Type
            m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;
            m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;
            m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;
            m_nMAlign_Step = (int)MAlign_Step.None;

            m_bStacker0_Complete = false;
            m_bStacker1_Complete = false;

            m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker0_Complete = false;               //  Stacker0 에서 Module Pick Up 완료 여부
            m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker1_Complete = false;               //  Stacker1 에서 Module Pick Up 완료 여부
            m_bAUTORUN_Loader_Transfer_ModulePickUpfromMAligner_Complete = false;               //  M-Aligner 에서 Module Pick Up 완료 여부
            m_bAUTORUN_Loader_Transfer_ModulePutDowntoMAligner_Complete = false;                //  M-Aligner 에 Module Put Down 완료 여부
            m_bAUTORUN_Loader_Transfer_ModulePutDowntoWorkStage_Complete = false;               //  Work Stage 에 Module Put Down 완료 여부

            m_bStacker0_Run_byUser = false;                     //  Stacker0 Module Pick Up Cycle
            m_bStacker1_Run_byUser = false;                     //  Stacker1 Module Pick Up Cycle

            m_bInManualMoving_SafetySensor_Detected = false;
            m_bInCycleMoving_SafetySensor_Detected = false;

            //  타이머를 쓰레드로 변경 --> 다시 타이머 사용하기로...

            //  쓰레드로 변경 --> 변경 취소. 그냥 타이머 쓴다. Thread 쓰니까 뭐가 막 잘 안됨 ㅡㅡ
           
            //  Loader Work 타이머
            timer_LoaderWork = new System.Timers.Timer(10);
            //timer_LoaderWork.Elapsed += Timer_LoaderWork_Tick;
            timer_LoaderWork.AutoReset = true; // 반복 실행
            timer_LoaderWork.Enabled = false; // 초기

            m_btimer_LoaderWork_Stop = false;

            m_bMAlign_Complete = false;
            m_bMAlign_Retry = false;

            m_bLog_1time = false;
            m_bLog_1time2 = false;

            TickCount_MainCycle_Start = 0;
            TickCount_MainCycle_Current = 0;
            TickCount_MainCycle_Interval = 0;


            //  Teaching Data 저장 폴더 생성
            if (Directory.Exists(ConfigManager.GetTeachingDataPath()) == false)
            {
                Directory.CreateDirectory(ConfigManager.GetTeachingDataPath());
            }

            //  Teaching Position List 변수 초기화
            for (int i = 0; i < System.Enum.GetValues(typeof(LDUL_TeachingPosList)).Length; i++)
            {
                stLDULTeachingPos[i].LD_Transfer_X = 0;
                stLDULTeachingPos[i].LD_Transfer_Z = 0;
                stLDULTeachingPos[i].LD_Stacker_Z0 = 0;
                stLDULTeachingPos[i].LD_Stacker_Z1 = 0;
                stLDULTeachingPos[i].MAligner_X = 0;
                stLDULTeachingPos[i].MAligner_Y = 0;
                stLDULTeachingPos[i].UL_Transfer_X = 0;
                stLDULTeachingPos[i].UL_Transfer_Z = 0;
                stLDULTeachingPos[i].UL_Stacker_Z0 = 0;
                stLDULTeachingPos[i].UL_Stacker_Z1 = 0;

                stLDULPosMoveProperties[i].Fine_Accel = 0;
                stLDULPosMoveProperties[i].Fine_SettleDelay = 0;
                stLDULPosMoveProperties[i].Coarse_Accel = 0;
                stLDULPosMoveProperties[i].Coarse_SettleDelay = 0;
            }

            Teaching_Position_Load();


            m_dMAlign_ModuleSize_Width = 0.0;                //  M-Aligner Module Size Width
            m_dMAlign_ModuleSize_Height = 0.0;               //  M-Aligner Module Size Height
            m_dMAlign_CalculatedModuleSize_ALN_X = 0.0;
            m_dMAlign_CalculatedModuleSize_ALN_Y = 0.0;


            m_bLD_LPort_Complete = false;                               //  L-Port 동작 완료 여부
            m_bLD_RPort_Complete = false;                               //  R-Port 동작 완료 여부
            m_bLD_TR_ModulePickUp_LPort_Complete = false;               //  Transfer L-Port Module Pick Up 동작 완료 여부
            m_bLD_TR_ModulePickUp_RPort_Complete = false;               //  Transfer R-Port Module Pick Up 동작 완료 여부
            m_bLD_TR_ModulePutDown_MAligner_Complete = false;           //  Transfer Module Put Down 동작 완료 여부
            m_bLD_MAligner_Exist = false;                               //  M-Aligner 로 Module Pick & Place
            m_bLD_MAlign_Complete = false;                              //  M-Aligner 동작 완료 여부
            m_bLD_TR_ModulePickUp_MAligner_Complete = false;            //  M-Aligner Module Pick Up 동작 완료 여부
            m_bLD_WorkStage_LoadingComplete = false;                    //  Work Stage 로 Module Loading 완료 여부

            m_nLoaderTransfer_ProcessStep = (int)LoaderTransferProcessStep.LoaderStep_None;
        }
        #endregion


        public void Module_Allocation()
        {
            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;

            foreach (Module module in m_collectionModules)
            {
                if (module.Name == "WorkStage")
                {
                    workStage = module as WorkStage;
                }

                if (module.Name == "Unloader")
                {
                    unloader = module as Unloader;
                }

                //if (module.Name == "BDS")
                //{
                //    bds = module as Bds;
                //}

                //if (module.Name == "Vision")
                //{
                //    vision = module as Vision;
                //}
            }
        }

        #region Action

        public Action<Loader_Transfer_Step> ActionLoaderTransferStep;

        #endregion

        #region IExecuter
        public override int Initialize()
        {
            return base.Initialize();
        }

        public override int OnPrepareToWork()
        {
            return base.OnPrepareToWork();
        }
        public override int OnAfterWork()
        {
            return base.OnAfterWork();
        }
        public override void Stop()
        {
            base.Stop();
        }
        public override int OnWork()
        {
            //dispenserParameter.stDispenserPosParam = dispenserParameter.GetPositionInformation("Ready");


            int ret = 0;
            ret = base.Work();

            return ret;
        }
        #endregion

        #region Module Members

        protected override int OnRun()
        {
            return base.OnRun();
        }
        protected bool IsAlarm()
        {
            bool bResult = false;
            try
            {
                var v = AlarmManager.Instance.Alarms;
                var alarmList = v.Where(t => t.Code >= (int)AlarmKey.FirstAlarm && t.Code <= (int)AlarmKey.LastAlarm);
                bResult =  alarmList.Any();
            }catch(Exception ex)
            {
                Log.Write(ex);
            }
            return bResult;


        }
        public override int Create()
        {
            int ret = base.Create();

            Stage = new ZzxzxyStage("Stage");
            Stage.Create();
            Stage.Owner = this;
            Parts.Add(Stage);

            loaderParameter = new LoaderParameter("Loader Parameter");
            loaderParameter.Create();
            loaderParameter.Owner = this;
            loaderParameter.Axes = Stage.Axes;
            Parts.Add(loaderParameter);

            //PosParam_Dispenser = GetConfigData();     //  요건 나중에

            Recipe = new LoaderRecipe(this);
            m_taskTimer_LoaderWork_Tick = Task.Factory.StartNew(() =>
            {
                Thread.CurrentThread.Name = "m_taskTimer_LoaderWork_Tick";
                while (true)
                {
                    Thread.Sleep(1);
                    if (IsAlarm())
                    {
                        continue;
                    }
                    if (m_IsModuleClose)
                    {
                        break;
                    }
                    Timer_LoaderWork_Tick(null, null);

                    
                }
            }); ;
             


            return ret;
        }

        public override void SetConfigData(object configData)
        {
            Config = configData as LoaderConfig;

            if (Config == null)
                Config = new LoaderConfig();

            Config.Init();

            if (Config.ParamConfig != null)
            {
                ParamConfig = Config.ParamConfig;

                loaderParameter.Config = ParamConfig;
            }

            Stage.Config = this.Config.StageConfig;

            //Stage.UpdateDirection();                              //  Z 축 방향 바꾸기? (주석 처리)
            //jigAligner.Config = Config.JigAlignerConfig;
        }

        public override object GetConfigData()
        {
            return Config;
        }

        public override void UpdateConfigData()
        {
            //jigAligner.Config = Config.JigAlignerConfig;

            base.UpdateConfigData();
        }

        public override void SetRecipeData(object recipeData)
        {
            LoaderRecipe recipe = recipeData as LoaderRecipe;
            if (recipe == null)
            {
                recipe = new LoaderRecipe(this);
            }


            Recipe = recipe;
            Recipe.Init(this);

            base.SetRecipeData(recipeData);
        }

        public override object GetRecipeData()
        {
            return Recipe;
        }

        public override void UpdateRecipeData()
        {
            base.UpdateRecipeData();
        }

        public override void Close()
        {
            m_IsModuleClose = true;
            if (m_taskTimer_LoaderWork_Tick != null)
            {
                m_taskTimer_LoaderWork_Tick.Wait();
                m_taskTimer_LoaderWork_Tick.Dispose();
                m_taskTimer_LoaderWork_Tick = null;
            }
            base.Close();

            if (Stage != null)
            {
                Stage.Close();
            }

            //if (ACS_Motion != null)
            //{
            //    ACS_Motion.CloseComm();
            //}
        }
        #endregion



        #region Stop 후 Start 시 동작 Sequence 를 재설정 하기 위한 함수

        public void Loader_CurrentStatus_Save_StopedByTimeout()
        {
            //  Loader 상태
            m_nLD_RESTORE_Transfer_Step = m_nLoader_Transfer_Step;
            m_nLD_RESTORE_Transfer_MoveType = m_nLoaderTransferMoveType;
            m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePutDowntoWorkStage_Complete = m_bAUTORUN_Loader_Transfer_ModulePutDowntoWorkStage_Complete;               //  Work Stage 에 Module Put Down 완료 여부
            m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePickUpfromStacker0_Complete = m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker0_Complete;               //  Stacker0 에서 Module Pick Up 완료 여부
            m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePickUpfromStacker1_Complete = m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker1_Complete;               //  Stacker1 에서 Module Pick Up 완료 여부
            m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePickUpfromMAligner_Complete = m_bAUTORUN_Loader_Transfer_ModulePickUpfromMAligner_Complete;               //  M-Aligner 에서 Module Pick Up 완료 여부
            m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePutDowntoMAligner_Complete = m_bAUTORUN_Loader_Transfer_ModulePutDowntoMAligner_Complete;                 //  M-Aligner 에 Module Put Down 완료 여부
            m_nLD_RESTORE_MainWork_Cycle_Step = workStage.m_nMainWork_Step;                                                                                              //  Main Work Cycle Step
            m_nLD_RESTORE_DryRun_Cycle_Step = workStage.m_nDryRun_Step;                                                                                                  //  Dry Run Cycle Step
            m_nLD_RESTORE_LaserDrilling_Cycle_Step = workStage.m_nLaserDrilling_MainStep;                                                                                //  Laser Drilling Cycle Step
            m_bLD_RESTORE_MainWork_Cycle_Complete = workStage.m_bMainWorkCycle_Complete;                                                                                 //  Main Work Cycle 완료 여부
            m_bLD_RESTORE_Transfer_fromStacker0_Module_PickUp_Complete_Flag = m_bLD_Transfer_fromStacker0_Module_PickUp_Complete_Flag;                            //  Stacker0 에서 Module Pick Up 완료 여부
            m_bLD_RESTORE_Transfer_fromStacker1_Module_PickUp_Complete_Flag = m_bLD_Transfer_fromStacker1_Module_PickUp_Complete_Flag;                            //  Stacker1 에서 Module Pick Up 완료 여부
            m_bLD_RESTORE_Transfer_fromMAligner_Module_PickUp_Complete_Flag = m_bLD_Transfer_fromMAligner_Module_PickUp_Complete_Flag;                            //  M-Aligner 에서 Module Pick Up 완료 여부
            m_bLD_RESTORE_Transfer_toMAligner_Module_PutDown_Complete_Flag = m_bLD_Transfer_toMAligner_Module_PutDown_Complete_Flag;                              //  M-Aligner 에 Module Put Down 완료 여부
            m_bLD_RESTORE_Transfer_toWorkStage_Module_PutDown_Complete_Flag = m_bLD_Transfer_toWorkStage_Module_PutDown_Complete_Flag;                            //  Work Stage 에 Module Put Down 완료 여부
        }

        public void Loader_Transfer_Restart_MoveType_Check()
        {
            //  Stop 했을 때의 조건들을 조합하여 시작 조건 결정

            //  1. Loader Transfer Cycle 의 Step
            //  2. Transfer 의 Move Type
            //  3. Module Pick Up 완료 여부
            //  4. Loader 가 Stacker 에서 Module 을 Pick Up 했는지 여부
            //  5. Loader 가 M-Aligner 에 Module 을 Put Down 했는지 여부
            //  6. Loader 가 M-Aligner 에서 Module 을 Pick Up 했는지 여부
            //  7. Loader 가 Stage 에 Module 을 Put Down 했는지 여부
            //  8. Main Work Cycle 의 동작 여부
            //  9. Main Work Cycle 의 완료 여부
            //  10. Main Work Cycle 이 완료되었다면, 양불 결과


            //int m_nLD_RESTORE_Transfer_Step { set; get; } = 0;
            //int m_nLD_RESTORE_Transfer_MoveType { set; get; } = 0;
            //bool m_bLD_RESTORE_Transfer_toWorkStage_Module_PutDown_Complete_Flag { set; get; } = false;      //  Work Stage 에 Module Put Down 완료 여부
            //bool m_bLD_RESTORE_Transfer_fromStacker0_Module_PickUp_Complete_Flag { set; get; } = false;      //  Stacker0 에서 Module Pick Up 완료 여부
            //bool m_bLD_RESTORE_Transfer_fromStacker1_Module_PickUp_Complete_Flag { set; get; } = false;      //  Stacker1 에서 Module Pick Up 완료 여부
            //bool m_bLD_RESTORE_Transfer_fromMAligner_Module_PickUp_Complete_Flag { set; get; } = false;      //  M-Aligner 에서 Module Pick Up 완료 여부
            //bool m_bLD_RESTORE_Transfer_toMAligner_Module_PutDown_Complete_Flag { set; get; } = false;       //  M-Aligner 에 Module Put Down 완료 여부
            //int m_nLD_RESTORE_MainWork_Cycle_Step { set; get; } = 0;
            //int m_nLD_RESTORE_DryRun_Cycle_Step { set; get; } = 0;
            //int m_nLD_RESTORE_LaserDrilling_Cycle_Step { set; get; } = 0;
            //bool m_bLD_RESTORE_MainWork_Cycle_Complete { set; get; } = false;
            //bool m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePickUpfromStacker0_Complete { set; get; }       //  Stacker0 에서 Module Pick Up 완료 여부
            //bool m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePickUpfromStacker1_Complete { set; get; }       //  Stacker1 에서 Module Pick Up 완료 여부
            //bool m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePickUpfromMAligner_Complete { set; get; }       //  M-Aligner 에서 Module Pick Up 완료 여부
            //bool m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePutDowntoMAligner_Complete { set; get; }        //  M-Aligner 에 Module Put Down 완료 여부
            //bool m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePutDowntoWorkStage_Complete { set; get; }       //  Work Stage 에 Module Put Down 완료 여부


            //  Auto Run 시 동작 조건을 결정하는 변수들. 위 Restore 조건으로 이 변수들의 값을 결정해서 Restart 하도록 한다.
            //bool m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker0_Complete { set; get; }              //  Stacker0 에서 Module Pick Up 완료 여부
            //bool m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker1_Complete { set; get; }              //  Stacker1 에서 Module Pick Up 완료 여부
            //bool m_bAUTORUN_Loader_Transfer_ModulePickUpfromMAligner_Complete { set; get; }              //  M-Aligner 에서 Module Pick Up 완료 여부
            //bool m_bAUTORUN_Loader_Transfer_ModulePutDowntoMAligner_Complete { set; get; }               //  M-Aligner 에 Module Put Down 완료 여부
            //bool m_bAUTORUN_Loader_Transfer_ModulePutDowntoWorkStage_Complete { set; get; }              //  Work Stage 에 Module Put Down 완료 여부



            ////  1. Laser Drilling 이 진행중이면? Loader 모든 동작 Stop
            //if (m_nLD_RESTORE_LaserDrilling_Cycle_Step != (int)WorkStage.LaserDrilling_Step.None)
            //{
            //    m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker0_Complete = false;                           //  Stacker0 에서 Module Pick Up 완료 여부
            //    m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker1_Complete = false;                           //  Stacker1 에서 Module Pick Up 완료 여부
            //    m_bAUTORUN_Loader_Transfer_ModulePickUpfromMAligner_Complete = false;                           //  M-Aligner 에서 Module Pick Up 완료 여부
            //    m_bAUTORUN_Loader_Transfer_ModulePutDowntoMAligner_Complete = false;                            //  M-Aligner 에 Module Put Down 완료 여부
            //    m_bAUTORUN_Loader_Transfer_ModulePutDowntoWorkStage_Complete = false;                           //  Work Stage 에 Module Put Down 완료 여부

            //    m_bStacker0_Complete = true;                //  Stacker 는 false 조건에 동작하기 때문에, Transfer 의 동작이 없을 때 동작시키도록 조건을 걸어준다.
            //    m_bStacker1_Complete = true;                //  Stacker 는 false 조건에 동작하기 때문에, Transfer 의 동작이 없을 때 동작시키도록 조건을 걸어준다.
            //}
            ////  2. Main Work Cycle 이 진행중이면? Loader 모든 동작 Stop
            //else if (m_nLD_RESTORE_MainWork_Cycle_Step != (int)WorkStage.MainWork_Step.None)
            //{
            //    m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker0_Complete = false;                           //  Stacker0 에서 Module Pick Up 완료 여부
            //    m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker1_Complete = false;                           //  Stacker1 에서 Module Pick Up 완료 여부
            //    m_bAUTORUN_Loader_Transfer_ModulePickUpfromMAligner_Complete = false;                           //  M-Aligner 에서 Module Pick Up 완료 여부
            //    m_bAUTORUN_Loader_Transfer_ModulePutDowntoMAligner_Complete = false;                            //  M-Aligner 에 Module Put Down 완료 여부
            //    m_bAUTORUN_Loader_Transfer_ModulePutDowntoWorkStage_Complete = false;                           //  Work Stage 에 Module Put Down 완료 여부

            //    m_bStacker0_Complete = true;                //  Stacker 는 false 조건에 동작하기 때문에, Transfer 의 동작이 없을 때 동작시키도록 조건을 걸어준다.
            //    m_bStacker1_Complete = true;                //  Stacker 는 false 조건에 동작하기 때문에, Transfer 의 동작이 없을 때 동작시키도록 조건을 걸어준다.
            //}
            //  3. Loader Transfer Cycle 이 None 상태이면, 동작이 없던 상태이므로 기존 조건들 그대로 적용하여 Start 한다. 
            /*else*/ if (m_nLD_RESTORE_Transfer_Step == (int)Loader_Transfer_Step.None)
            {
                m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker0_Complete = m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePickUpfromStacker0_Complete;             //  Stacker0 에서 Module Pick Up 완료 여부
                m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker1_Complete = m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePickUpfromStacker1_Complete;             //  Stacker1 에서 Module Pick Up 완료 여부
                m_bAUTORUN_Loader_Transfer_ModulePickUpfromMAligner_Complete = m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePickUpfromMAligner_Complete;             //  M-Aligner 에서 Module Pick Up 완료 여부
                m_bAUTORUN_Loader_Transfer_ModulePutDowntoMAligner_Complete = m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePutDowntoMAligner_Complete;               //  M-Aligner 에 Module Put Down 완료 여부
                m_bAUTORUN_Loader_Transfer_ModulePutDowntoWorkStage_Complete = m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePutDowntoWorkStage_Complete;             //  Work Stage 에 Module Put Down 완료 여부

                m_bStacker0_Complete = false;               //  Stacker 는 false 조건에 동작
                m_bStacker1_Complete = false;               //  Stacker 는 false 조건에 동작
            }
            //  4. Loader Transfer Cycle 이 None 이 아니면, 뭔가 동작을 하던 상황
            else
            {
                //  4-1. Loader Transfer Cycle 이 Stacker0 에서 Module Pick Up 인 경우
                if (m_nLD_RESTORE_Transfer_MoveType == (int)LoaderTransferMoveType.Cycle_Stacker0_PickUp)
                {
                    //  4-1-1. Stacker0 에서 Module Pick Up 완료했을 경우 --> M-Aligner 에 Module 을 내려놓는 Cycle 진행해야 한다.
                    if (m_bLD_RESTORE_Transfer_fromStacker0_Module_PickUp_Complete_Flag)
                    {
                        m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker0_Complete = true;
                        m_bAUTORUN_Loader_Transfer_ModulePutDowntoMAligner_Complete = false;

                        m_nLoaderTransfer_ProcessStep = (int)LoaderTransferProcessStep.LoaderStep_ModulePutDown_MAligner;

                        m_bMAlignZone_ModuleExist = false;

                        m_bStacker0_Complete = false;
                    }
                    //  4-1-2. Stacker0 에서 Module Pick Up 완료하지 못했을 경우 --> Stacker0 에서 Module Pick Up Cycle 재 진행
                    else
                    {
                        m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker0_Complete = false;
                        m_nLoaderTransfer_ProcessStep = (int)LoaderTransferProcessStep.LoaderStep_ModulePickup_fromStacker;

                        m_bStacker0_Complete = true;

                        m_bMAlignZone_ModuleExist = false;
                    }
                }
                //  4-2. Loader Transfer Cycle 이 Stacker1 에서 Module Pick Up 인 경우
                else if (m_nLD_RESTORE_Transfer_MoveType == (int)LoaderTransferMoveType.Cycle_Stacker1_PickUp)
                {
                    //  4-2-1. Stacker1 에서 Module Pick Up 완료했을 경우 --> M-Aligner 에 Module 을 내려놓는 Cycle 진행해야 한다.
                    if (m_bLD_RESTORE_Transfer_fromStacker1_Module_PickUp_Complete_Flag)
                    {
                        m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker1_Complete = true;
                        m_bAUTORUN_Loader_Transfer_ModulePutDowntoMAligner_Complete = false;

                        m_nLoaderTransfer_ProcessStep = (int)LoaderTransferProcessStep.LoaderStep_ModulePutDown_MAligner;

                        m_bMAlignZone_ModuleExist = false;

                        m_bStacker1_Complete = false;
                    }
                    //  4-2-2. Stacker1 에서 Module Pick Up 완료하지 못했을 경우 --> Stacker1 에서 Module Pick Up Cycle 재 진행
                    else
                    {
                        m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker1_Complete = false;
                        m_nLoaderTransfer_ProcessStep = (int)LoaderTransferProcessStep.LoaderStep_ModulePickup_fromStacker;
                        
                        m_bStacker1_Complete = true;

                        m_bMAlignZone_ModuleExist = false;
                    }
                }
                //  4-3. Loader Transfer Cycle 이 M-Aligner 에 Module Put Down 인 경우
                else if (m_nLD_RESTORE_Transfer_MoveType == (int)LoaderTransferMoveType.Cycle_MAligner_PutDown)
                {
                    //  4-3-1. M-Aligner 에 Module Put Down 완료했을 경우 --> M-Aligner 을 진행한다.
                    if (m_bLD_RESTORE_Transfer_toMAligner_Module_PutDown_Complete_Flag)
                    {
                        m_bAUTORUN_Loader_Transfer_ModulePutDowntoMAligner_Complete = true;
                        m_bMAlignZone_ModuleExist = true;                       //  M-Aligner 에 모듈을 내려놨으므로

                        //  M-Align 시작
                        m_nMAlign_Step = (int)MAlign_Step.Start;
                    }
                    //  4-3-2. M-Aligner 에 Module Put Down 완료하지 못했을 경우 --> M-Aligner 에 Module Put Down Cycle 재 진행
                    else
                    {
                        m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker0_Complete = m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePickUpfromStacker0_Complete;         //  둘 중에 하나는 true 겠지...
                        m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker1_Complete = m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePickUpfromStacker1_Complete;         //  둘 중에 하나는 true 겠지...

                        m_bAUTORUN_Loader_Transfer_ModulePutDowntoMAligner_Complete = false;
                        
                        m_bMAlignZone_ModuleExist = false;

                        m_nLoaderTransfer_ProcessStep = (int)LoaderTransferProcessStep.LoaderStep_ModulePutDown_MAligner;
                    }
                }
                //  4-4. Loader Transfer Cycle 이 M-Aligner 에서 Module Pick Up 인 경우
                else if (m_nLD_RESTORE_Transfer_MoveType == (int)LoaderTransferMoveType.Cycle_MAligner_PickUp)
                {
                    //  4-4-1. M-Aligner 에서 Module Pick Up 완료했을 경우 --> Work Stage 에 Module 을 내려놓는 Cycle 진행해야 한다.
                    if (m_bLD_RESTORE_Transfer_fromMAligner_Module_PickUp_Complete_Flag)
                    {
                        m_bAUTORUN_Loader_Transfer_ModulePickUpfromMAligner_Complete = true;
                        m_bAUTORUN_Loader_Transfer_ModulePutDowntoMAligner_Complete = false;
                        m_bMAlignZone_ModuleExist = false;
                        m_bMAlign_Complete = false;

                        m_nLoaderTransfer_ProcessStep = (int)LoaderTransferProcessStep.LoaderStep_ModulePutDown_Stage;
                    }
                    //  4-4-2. M-Aligner 에서 Module Pick Up 완료하지 못했을 경우 --> M-Aligner 에서 Module Pick Up Cycle 재 진행
                    else
                    {
                        m_bAUTORUN_Loader_Transfer_ModulePutDowntoMAligner_Complete = true;
                        m_bAUTORUN_Loader_Transfer_ModulePickUpfromMAligner_Complete = false;

                        m_nLoaderTransfer_ProcessStep = (int)LoaderTransferProcessStep.LoaderStep_ModulePickUp_MAligner;

                        workStage.m_bMainWorkCycle_Complete = false;

                        m_bMAlign_Complete = false;                             //  들어올리지 못했으니 얼라인을 다시 진행한다.
                        m_bMAlign_Retry = true;

                        m_bMAlignZone_ModuleExist = true;
                    }
                }
                //  4-5. Loader Transfer Cycle 이 Work Stage 에 Module Put Down 인 경우
                else if (m_nLD_RESTORE_Transfer_MoveType == (int)LoaderTransferMoveType.Cycle_WorkStage_PutDown)
                {
                    //  4-5-1. Work Stage 에 Module Put Down 완료했을 경우 --> Main Work Cycle 진행해야 한다.
                    if (m_bLD_RESTORE_Transfer_toWorkStage_Module_PutDown_Complete_Flag)
                    {
                        //  Work Stage 에 모듈을 내려놨으니 도면을 다시 로드해야 한다. (DryRun 이면 안함)
                        if (!workStage.m_bMainWorkCycle_DryRun)
                        {
                            workStage.Import_DrawingFile(Equipment.RecipeOpen_DrawingFilePath);
                        }

                        m_bAUTORUN_Loader_Transfer_ModulePutDowntoWorkStage_Complete = true;
                        m_bAUTORUN_Loader_Transfer_ModulePickUpfromMAligner_Complete = false;
                        m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker0_Complete = false;
                        m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker1_Complete = false;

                        m_nLoaderTransfer_ProcessStep = (int)LoaderTransferProcessStep.LoaderStep_ModulePickup_fromStacker;

                        workStage.m_bDryRun_Complete = false;
                        workStage.m_bLaserDrilling_Complete = false;
                    }
                    //  4-5-2. Work Stage 에 Module Put Down 완료하지 못했을 경우 --> Work Stage 에 Module Put Down Cycle 재 진행
                    //  4-5-2. Work Stage 에 Module Put Down 완료하지 못했을 경우 --> Loading 부터 다시 진행하도록 한다..
                    else
                    {
                        //m_bAUTORUN_Loader_Transfer_ModulePickUpfromMAligner_Complete = true;
                        //m_bAUTORUN_Loader_Transfer_ModulePutDowntoWorkStage_Complete = false;
                        //m_nLoaderTransfer_ProcessStep = (int)LoaderTransferProcessStep.LoaderStep_ModulePutDown_Stage;

                        m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker0_Complete = false;
                        m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker1_Complete = false;
                        m_bAUTORUN_Loader_Transfer_ModulePickUpfromMAligner_Complete = false;

                        m_nLoaderTransfer_ProcessStep = (int)LoaderTransferProcessStep.LoaderStep_ModulePickup_fromStacker;

                        m_bStacker0_Complete = false;
                        m_bStacker1_Complete = false;

                        m_bMAlignZone_ModuleExist = false;

                        workStage.workStageParameter.DO_Stage_Vacuum(false);
                        workStage.ElectroPneumaticRegulatorComm_Pressure_Set(-1.3);
                    }
                }
            }
        }

        #endregion



        #region Stacker Move Function (Module PickUp & PutDown 높이로 이동 -> 이건 Loader Unloader 에서 하도록 해야 할듯???)

        int  Run_Stacker0Module_PickupWaitingPos_Func()
        {
            int ret = 0;
            bool m_bRet = false;
            string m_strTemp;

            double m_dSpeed_Stacker_Fast = 0.0;
            double m_dSpeed_Stacker_Slow = 0.0;
            double m_dSpeed_Stacker_MoreSlow = 0.0;
            double m_dSpeedMag_forAccDec = 0.0;

            //  운전 중 Door 를 열면 장비 Stop
            if (m_nStacker0_ModulePickupWaitingPos_Step >= (int)StackerModulePickupWaitingPos_Step.Start)
            {
                //if (Config.ParamConfig.AreaSensor_Usage && (waferProbeAlignParameter.DI_AreaSensor_Detect() || waferProbeAlignParameter.DI_AlignJig_Detect()))
                //{
                //    Log.Write("CWA150SA", Equipment.User_Name, "Machine Initialize", "안전 센서 감지로 인한 장비 Stop");

                //    m_bInCycleMoving_SafetySensor_Detected = true;

                //    //  알람 정지 (LED Bar - Red Blink)
                //    Equipment.MachineStop_byAlarm = true;

                //    timer_Motion_Home.Enabled = false;
                //    m_btimer_Motion_Home_Stop = true;

                //    m_nHomeStep = (int)Home_Step.None;

                //    //MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.U, 500);
                //    //MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.V, 500);
                //    //MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.W, 500);
                //    //MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.EZ, 500);
                //    //MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.X, 500);
                //    //MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.Y, 500);
                //    //MC_Func.MC_MotorStop((int)WaferProbeAlignParameter.AxisAjinEnum.VZ, 500);

                //    for (int i = 0; i < (int)AxisAjinEnum.Max; i++)
                //    {
                //        MC_Func.MC_MotorStop(i, 2000);
                //        //MC_Func.MC_EStop(i);
                //    }

                //    if (!Equipment.User_QMC_Engineer)                   //  QMC 관리자가 아닐 경우에만 Home Flag 를 false 로
                //    {
                //        m_bHomeOK = false;                              //  안전센서 감지 시 무조건 장비 초기화 해야 함
                //    }

                //    if (!Equipment.User_QMC_Engineer && Config.ParamConfig.AreaSensor_ServoOff_Usage)           //  안전센서 감지 시 Servo Off 할 경우
                //    {
                //        for (int i = 0; i < (int)AxisAjinEnum.Max; i++)
                //        {
                //            MC_Func.MC_SetServoOnOff(i, false);
                //        }
                //    }
                //}
            }

            if (!loaderParameter.DI_Loader_Stacker_MaterialCheck((int)LoaderParameter.StackerTable.Stacker_0))
            {
                Equipment.Loader_RPort_Pause = true;
            }

            //  자동운전 시, Stacker0 동작 조건 : TR Cycle (None), Stacker0 Cycle (None), TR 이 Module 을 집어갔을 때
            if (Equipment.AutoRunStatus &&
                !Equipment.Loader_RPort_Pause &&
                m_nLoader_Transfer_Step == (int)Loader_Transfer_Step.None &&
                m_nStacker0_ModulePickupWaitingPos_Step == (int)StackerModulePickupWaitingPos_Step.None &&
                m_bStacker0_Run_byUser &&
                !m_bStacker0_Complete)                                                //  Stacker0 동작 완료되지 않은 상태 (TR 이 Module 을 집어간 후 false 로 변경됨)
            {
                Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "시작 Flag");

                m_bStacker0_Run_byUser = false;

                m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.Start;
            }


            //  자동 운전 Flag 를 On 시키는 조건 : m_bStacker0_Run_byUser 요거가 true 일 때만 Stacker0 동작
            if (Equipment.AutoRunStatus && !Equipment.Loader_RPort_Pause)
            {
                if (workStage.m_bMainWorkCycle_DryRun)          //  Dry Run
                {
                    if (!m_bStacker0_Run_byUser &&
                        !m_bStacker0_Complete &&

                        (m_nLoaderTransfer_ProcessStep == (int)LoaderTransferProcessStep.LoaderStep_ModulePickup_fromStacker))
                        //m_bLoader_Transfer_ModulePutDowntoWorkStage_Complete)               //  Work Stage 에 모듈을 내려놓은 후에 Stacker 진행
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Dry Run 시작 Flag On");

                        m_bStacker0_Run_byUser = true;
                    }
                }
                else                                            //  자동 운전
                {
                    if (!m_bStacker0_Run_byUser &&
                        !m_bStacker0_Complete &&

                        (m_nLoaderTransfer_ProcessStep == (int)LoaderTransferProcessStep.LoaderStep_ModulePickup_fromStacker) &&

                        loaderParameter.DI_Loader_Stacker_MaterialCheck((int)LoaderParameter.StackerTable.Stacker_0))               //  Stacker0 에 Module 이 감지되어 있을 때만 진행
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Auto Run 시작 Flag On");

                        m_bStacker0_Run_byUser = true;
                    }
                }
            }


            switch (m_nStacker0_ModulePickupWaitingPos_Step)
            {
                case (int)StackerModulePickupWaitingPos_Step.Start:
                    Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "시작");

                    Equipment.MachineStop_byAlarm = false;

                    //  Laoder Stacker Z 축 모터 전체 Stop
                    MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.Z0, 2000);
                    m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.Process_Condition_Check;
                    break;


                case (int)StackerModulePickupWaitingPos_Step.Process_Condition_Check:
                    Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "동작 조건 확인");

                    StackerModulePickupWaitingPos_StepProcess_Condition_Check();
                    break;


                /// <summary>
                /// Full Sensor 감지 상태일 경우 - 시작
                /// </summary>
                case (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_FastDown:                            //  Stacker Z 축, 빠르게 내림 (최 하단까지)

                    Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Stacker0 Z 축, Full 센서가 Off 되는 위치까지 이동 시작 (고속)");

                    loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Stacker0_Bottom");

                    //  Target Position 변경 : 맨 아래로 내려가는 위치
                    loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_RPort_ReadyPos].LD_Stacker_Z0;

                    //  속도 (기본 속도)
                    m_dSpeed_Stacker_Fast = Equipment.stAxisParam[(int)nAxis.Z0].Common_Speed_Fine;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = 2.0;

                    MC_Func.MC_MovePosition((int)nAxis.Z0,  
                                        loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0],
                                        m_dSpeed_Stacker_Fast,
                                        m_dSpeed_Stacker_Fast * m_dSpeedMag_forAccDec,
                                        m_dSpeed_Stacker_Fast * m_dSpeedMag_forAccDec);

                    //MC_Func.MC_MovePosition((int)UnloaderParameter.AxisAjinEnum.Z0,
                    //                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
                    //                    unloaderParameter.stUnloaderPosParam.dVel[(int)UnloaderParameter.MotionKey.Z0],
                    //                    unloaderParameter.stUnloaderPosParam.dAcc[(int)UnloaderParameter.MotionKey.Z0],
                    //                    unloaderParameter.stUnloaderPosParam.dDec[(int)UnloaderParameter.MotionKey.Z0]);

                    TickCount_Start((int)TickType.TICK_LDSZ0);

                    m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_FastDown_DoneCheck;
                    break;


                case (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_FastDown_DoneCheck:                       //  Stacker Z 축, 빠르게 내림, 이동 완료 확인

                    if (loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_0))          //  Full 감지 센서가 Off 되면 Stop     //  감지 시 Off
                    {
                        MC_Func.MC_MotorStop((int)nAxis.Z0, 2000);

                        m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_SlowUp;
                    }
                    else if (MC_Func.MC_GetDone((int)nAxis.Z0) && MC_Func.MC_PosTolerance((int)nAxis.Z0, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Stacker0 Z 축, Bottom 위치까지 이동 완료");

                        if (!loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_0))       //  감지 시 Off
                        {
                            Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Stacker0 Z 축, Bottom 위치까지 이동했으나 Full 센서 On 상태");

                            m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;

                            MessageBox.Show("LD Stacker0 Z 축, 자재가 너무 많거나 Full 수위 감지 센서 점검이 필요합니다.", "Information!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_SlowUp;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDSZ0) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Stacker0 Z 축, Full 센서가 Off 되는 위치까지 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;

                        MessageBox.Show("LD Stacker0 Z 축, Full 센서가 Off 되는 위치까지 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_SlowUp:                               //  Stacker Z 축, 느리게 올림 (최 상단까지)

                    if (MC_Func.MC_GetDone((int)nAxis.Z0))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Stacker0 Z 축, Full 센서가 On 되는 위치까지 이동 시작 (중속)");

                        loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Stacker0_Top");

                        //  Target Position 변경 : 맨 위로 올라가는 위치
                        loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_RPort_TopPos].LD_Stacker_Z0;

                        //  속도 (기본 속도 / 2)
                        m_dSpeed_Stacker_Slow = Equipment.stAxisParam[(int)nAxis.Z0].Common_Speed_Fine / 2.0;

                        //  가감속 배율
                        m_dSpeedMag_forAccDec = 2.0;

                        MC_Func.MC_MovePosition((int)nAxis.Z0,
                                            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0],
                                            m_dSpeed_Stacker_Slow,
                                            m_dSpeed_Stacker_Slow * m_dSpeedMag_forAccDec,
                                            m_dSpeed_Stacker_Slow * m_dSpeedMag_forAccDec);

                        //MC_Func.MC_MovePosition((int)UnloaderParameter.AxisAjinEnum.Z0,
                        //                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
                        //                    unloaderParameter.stUnloaderPosParam.dVel[(int)UnloaderParameter.MotionKey.Z0],
                        //                    unloaderParameter.stUnloaderPosParam.dAcc[(int)UnloaderParameter.MotionKey.Z0],
                        //                    unloaderParameter.stUnloaderPosParam.dDec[(int)UnloaderParameter.MotionKey.Z0]);

                        TickCount_Start((int)TickType.TICK_LDSZ0);

                        m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_SlowUp_DoneCheck;
                    }
                    break;


                case (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_SlowUp_DoneCheck:                          //  Stacker Z 축, 느리게 올림, 이동 완료 확인

                    if (!loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_0))       //  감지 시 Off
                    {
                        MC_Func.MC_MotorStop((int)nAxis.Z0, 2000);

                        m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_Slow2Down;
                    }
                    else if (MC_Func.MC_GetDone((int)nAxis.Z0) && MC_Func.MC_PosTolerance((int)nAxis.Z0, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Stacker0 Z 축, Top 위치까지 이동 완료");

                        if (loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_0))          //  감지 시 Off
                        {
                            Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Stacker0 Z 축, Top 위치까지 이동했으나 Full 센서 Off 상태");

                            m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;

                            Equipment.Loader_RPort_Pause = true;            //  자재는 감지되지만 Full 센서가 인식되지 않음. 

                            MessageBox.Show("LD Stacker0 Z 축, 자재가 없습니다.", "Information!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_Slow2Down;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDSZ0) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Stacker0 Z 축, Full 센서가 On 되는 위치까지 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;

                        MessageBox.Show("LD Stacker0 Z 축, Full 센서가 On 되는 위치까지 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_Slow2Down:                            //  Stacker Z 축, 더 느리게 내림 (최 하단까지)

                    if (MC_Func.MC_GetDone((int)nAxis.Z0))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Stacker0 Z 축, Full 센서가 Off 되는 위치까지 이동 시작 (저속)");

                        loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Stacker0_Bottom");

                        //  Target Position 변경 : 맨 아래로 내려가는 위치
                        loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_RPort_ReadyPos].LD_Stacker_Z0;

                        //  속도 (기본 속도 / 3)
                        //m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z0].Common_MoveSpeed / 3.0;
                        m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z0].Common_Speed_Fine / 3.0;

                        //  가감속 배율
                        m_dSpeedMag_forAccDec = 2.0;

                        MC_Func.MC_MovePosition((int)nAxis.Z0,
                                            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0],
                                            m_dSpeed_Stacker_MoreSlow,
                                            m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec,
                                            m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec);

                        //MC_Func.MC_MovePosition((int)UnloaderParameter.AxisAjinEnum.Z0,
                        //                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
                        //                    unloaderParameter.stUnloaderPosParam.dVel[(int)UnloaderParameter.MotionKey.Z0],
                        //                    unloaderParameter.stUnloaderPosParam.dAcc[(int)UnloaderParameter.MotionKey.Z0],
                        //                    unloaderParameter.stUnloaderPosParam.dDec[(int)UnloaderParameter.MotionKey.Z0]);

                        TickCount_Start((int)TickType.TICK_LDSZ0);

                        m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_Slow2Down_DoneCheck;
                    }
                    break;


                case (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_Slow2Down_DoneCheck:                       //  Stacker Z 축, 더 느리게 내림, 이동 완료 확인

                    if (loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_0))          //  감지 시 Off
                    {
                        MC_Func.MC_MotorStop((int)nAxis.Z0, 2000);

                        m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_Slow3Up;
                    }
                    else if (MC_Func.MC_GetDone((int)nAxis.Z0) && MC_Func.MC_PosTolerance((int)nAxis.Z0, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Stacker0 Z 축, Bottom 위치까지 이동 완료");

                        if (!loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_0))           //  감지 시 Off
                        {
                            Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Stacker0 Z 축, Bottom 위치까지 이동했으나 Full 센서 On 상태");

                            m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;

                            MessageBox.Show("LD Stacker0 Z 축, 자재가 너무 많거나 Full 수위 감지 센서 점검이 필요합니다.", "Information!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_Slow3Up;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDSZ0) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Stacker0 Z 축, Full 센서가 Off 되는 위치까지 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;

                        MessageBox.Show("LD Stacker0 Z 축, Full 센서가 Off 되는 위치까지 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_Slow3Up:                               //  Stacker Z 축, 더더 느리게 올림 (최 상단까지)

                    if (MC_Func.MC_GetDone((int)nAxis.Z0))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Stacker0 Z 축, Full 센서가 On 되는 위치까지 이동 시작 (저속 / 2)");

                        loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Stacker0_Top");

                        //  Target Position 변경 : 맨 위로 올라가는 위치
                        loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_RPort_TopPos].LD_Stacker_Z0;

                        //  속도 (기본 속도 / 4)
                        //m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z0].Common_MoveSpeed / 4.0;
                        m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z0].Common_Speed_Fine / 4.0;

                        //  가감속 배율
                        m_dSpeedMag_forAccDec = 2.0;

                        MC_Func.MC_MovePosition((int)nAxis.Z0,
                                            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0],
                                            m_dSpeed_Stacker_MoreSlow,
                                            m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec,
                                            m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec);

                        //MC_Func.MC_MovePosition((int)UnloaderParameter.AxisAjinEnum.Z0,
                        //                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
                        //                    unloaderParameter.stUnloaderPosParam.dVel[(int)UnloaderParameter.MotionKey.Z0],
                        //                    unloaderParameter.stUnloaderPosParam.dAcc[(int)UnloaderParameter.MotionKey.Z0],
                        //                    unloaderParameter.stUnloaderPosParam.dDec[(int)UnloaderParameter.MotionKey.Z0]);

                        TickCount_Start((int)TickType.TICK_LDSZ0);

                        m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_Slow3Up_DoneCheck;
                    }
                    break;


                case (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_Slow3Up_DoneCheck:                          //  Stacker Z 축, 더더 느리게 올림, 이동 완료 확인

                    if (!loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_0))           //  감지 시 Off
                    {
                        MC_Func.MC_MotorStop((int)nAxis.Z0, 2000);

                        //m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.Complete;
                        m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_Move_OverDistance;
                    }
                    else if (MC_Func.MC_GetDone((int)nAxis.Z0) && MC_Func.MC_PosTolerance((int)nAxis.Z0, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Stacker0 Z 축, Top 위치까지 이동 완료");

                        if (loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_0))          //  감지 시 Off
                        {
                            Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Stacker0 Z 축, Top 위치까지 이동했으나 Full 센서 Off 상태");

                            m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;

                            Equipment.Loader_RPort_Pause = true;            //  자재는 감지되지만 Full 센서가 인식되지 않음. 

                            return AlarmPost(AlarmKey.LD_Stacker0_FullSensor_Off_MoveFail);
                        }
                        else
                        {
                            //m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.Complete;
                            m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_Move_OverDistance;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDSZ0) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Stacker0 Z 축, Full 센서가 On 되는 위치까지 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;
                        return AlarmPost(AlarmKey.LD_Stacker0_FullSensor_Off_MoveFail);
                        m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;

                        MessageBox.Show("LD Stacker0 Z 축, Full 센서가 On 되는 위치까지 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                /// <summary>
                /// Full Sensor 감지 상태일 경우 - 완료
                /// </summary>



                /// <summary>
                /// Full Sensor 감지되지 않는 상태일 경우 - 시작
                /// </summary>
                case (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType2_FastUp:                               //  Stacker Z 축, 빠르게 올림 (최 상단까지)

                    Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Stacker0 Z 축, Full 센서가 On 되는 위치까지 이동 시작 (고속)");

                    loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Stacker0_Top");

                    //  Target Position 변경 : 맨 위로 올라가는 위치
                    loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_RPort_TopPos].LD_Stacker_Z0;

                    //  속도 (기본 속도)
                    m_dSpeed_Stacker_Fast = Equipment.stAxisParam[(int)nAxis.Z0].Common_Speed_Fine;

                    //  가감속 배율
                    m_dSpeedMag_forAccDec = 2.0;

                    MC_Func.MC_MovePosition((int)nAxis.Z0,
                                        loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0],
                                        m_dSpeed_Stacker_Fast,
                                        m_dSpeed_Stacker_Fast * m_dSpeedMag_forAccDec,
                                        m_dSpeed_Stacker_Fast * m_dSpeedMag_forAccDec);

                    //MC_Func.MC_MovePosition((int)UnloaderParameter.AxisAjinEnum.Z0,
                    //                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
                    //                    unloaderParameter.stUnloaderPosParam.dVel[(int)UnloaderParameter.MotionKey.Z0],
                    //                    unloaderParameter.stUnloaderPosParam.dAcc[(int)UnloaderParameter.MotionKey.Z0],
                    //                    unloaderParameter.stUnloaderPosParam.dDec[(int)UnloaderParameter.MotionKey.Z0]);

                    TickCount_Start((int)TickType.TICK_LDSZ0);

                    m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType2_FastUp_DoneCheck;
                    break;


                case (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType2_FastUp_DoneCheck:                          //  Stacker Z 축, 빠르게 올림, 이동 완료 확인

                    if (!loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_0))           //  감지 시 Off
                    {
                        MC_Func.MC_MotorStop((int)nAxis.Z0, 2000);

                        m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType2_Slow2Down;
                    }
                    else if (MC_Func.MC_GetDone((int)nAxis.Z0) && MC_Func.MC_PosTolerance((int)nAxis.Z0, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Stacker0 Z 축, Top 위치까지 이동 완료");

                        if (loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_0))          //  감지 시 Off
                        {
                            Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Stacker0 Z 축, Top 위치까지 이동했으나 Full 센서 Off 상태");

                            m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;

                            Equipment.Loader_RPort_Pause = true;            //  자재는 감지되지만 Full 센서가 인식되지 않음. 

                            MessageBox.Show("LD Stacker0 Z 축, 자재가 없습니다.", "Information!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType2_Slow2Down;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDSZ0) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Stacker0 Z 축, Full 센서가 On 되는 위치까지 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;
                        return AlarmPost(AlarmKey.LD_Stacker0_FullSensor_Off_MoveFail);
                        m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;

                        MessageBox.Show("LD Stacker0 Z 축, Full 센서가 On 되는 위치까지 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType2_Slow2Down:                            //  Stacker Z 축, 더 느리게 내림 (최 하단까지)

                    if (MC_Func.MC_GetDone((int)nAxis.Z0))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Stacker0 Z 축, Full 센서가 Off 되는 위치까지 이동 시작 (저속)");

                        loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Stacker0_Bottom");

                        //  Target Position 변경 : 맨 아래로 내려가는 위치
                        loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_RPort_ReadyPos].LD_Stacker_Z0;

                        //  속도 (기본 속도 / 3)
                        //m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z0].Common_MoveSpeed / 3.0;
                        m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z0].Common_Speed_Fine / 3.0;

                        //  가감속 배율
                        m_dSpeedMag_forAccDec = 2.0;

                        MC_Func.MC_MovePosition((int)nAxis.Z0,
                                            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0],
                                            m_dSpeed_Stacker_MoreSlow,
                                            m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec,
                                            m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec);

                        //MC_Func.MC_MovePosition((int)UnloaderParameter.AxisAjinEnum.Z0,
                        //                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
                        //                    unloaderParameter.stUnloaderPosParam.dVel[(int)UnloaderParameter.MotionKey.Z0],
                        //                    unloaderParameter.stUnloaderPosParam.dAcc[(int)UnloaderParameter.MotionKey.Z0],
                        //                    unloaderParameter.stUnloaderPosParam.dDec[(int)UnloaderParameter.MotionKey.Z0]);

                        TickCount_Start((int)TickType.TICK_LDSZ0);

                        m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType2_Slow2Down_DoneCheck;
                    }
                    break;


                case (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType2_Slow2Down_DoneCheck:                       //  Stacker Z 축, 더 느리게 내림, 이동 완료 확인

                    if (loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_0))          //  감지 시 Off
                    {
                        MC_Func.MC_MotorStop((int)nAxis.Z0, 2000);

                        m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType2_Slow3Up;
                    }
                    else if (MC_Func.MC_GetDone((int)nAxis.Z0) && MC_Func.MC_PosTolerance((int)nAxis.Z0, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Stacker0 Z 축, Bottom 위치까지 이동 완료");

                        if (!loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_0))       //  감지 시 Off
                        {
                            Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Stacker0 Z 축, Bottom 위치까지 이동했으나 Full 센서 On 상태");

                            m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;

                            MessageBox.Show("LD Stacker0 Z 축, 자재가 너무 많거나 Full 수위 감지 센서 점검이 필요합니다.", "Information!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType2_Slow3Up;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDSZ0) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Stacker0 Z 축, Full 센서가 Off 되는 위치까지 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;
                        return AlarmPost(AlarmKey.LD_Stacker0_FullSensor_Off_MoveFail);
                        m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;

                        MessageBox.Show("LD Stacker0 Z 축, Full 센서가 Off 되는 위치까지 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType2_Slow3Up:                               //  Stacker Z 축, 더더 느리게 올림 (최 상단까지)

                    if (MC_Func.MC_GetDone((int)nAxis.Z0))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Stacker0 Z 축, Full 센서가 On 되는 위치까지 이동 시작 (저속 / 2)");

                        loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Stacker0_Top");

                        //  Target Position 변경 : 맨 위로 올라가는 위치
                        loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_RPort_TopPos].LD_Stacker_Z0;

                        //  속도 (기본 속도 / 4)
                        //m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z0].Common_MoveSpeed / 4.0;
                        m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z0].Common_Speed_Fine / 4.0;

                        //  가감속 배율
                        m_dSpeedMag_forAccDec = 2.0;

                        MC_Func.MC_MovePosition((int)nAxis.Z0,
                                            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0],
                                            m_dSpeed_Stacker_MoreSlow,
                                            m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec,
                                            m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec);

                        //MC_Func.MC_MovePosition((int)UnloaderParameter.AxisAjinEnum.Z0,
                        //                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
                        //                    unloaderParameter.stUnloaderPosParam.dVel[(int)UnloaderParameter.MotionKey.Z0],
                        //                    unloaderParameter.stUnloaderPosParam.dAcc[(int)UnloaderParameter.MotionKey.Z0],
                        //                    unloaderParameter.stUnloaderPosParam.dDec[(int)UnloaderParameter.MotionKey.Z0]);

                        TickCount_Start((int)TickType.TICK_LDSZ0);

                        m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType2_Slow3Up_DoneCheck;
                    }
                    break;


                case (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType2_Slow3Up_DoneCheck:                          //  Stacker Z 축, 더더 느리게 올림, 이동 완료 확인

                    if (!loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_0))           //  감지 시 Off
                    {
                        MC_Func.MC_MotorStop((int)nAxis.Z0, 2000);

                        //m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.Complete;
                        m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_Move_OverDistance;
                    }
                    else if (MC_Func.MC_GetDone((int)nAxis.Z0) && MC_Func.MC_PosTolerance((int)nAxis.Z0, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Stacker0 Z 축, Top 위치까지 이동 완료");

                        if (loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_0))          //  감지 시 Off
                        {
                            Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Stacker0 Z 축, Top 위치까지 이동했으나 Full 센서 Off 상태");

                            m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;

                            Equipment.Loader_RPort_Pause = true;            //  자재는 감지되지만 Full 센서가 인식되지 않음. 

                            MessageBox.Show("LD Stacker0 Z 축, 자재가 없습니다.", "Information!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            //m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.Complete;
                            m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_Move_OverDistance;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDSZ0) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Stacker0 Z 축, Full 센서가 On 되는 위치까지 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;
                        return AlarmPost(AlarmKey.LD_Stacker0_FullSensor_Off_MoveFail);
                        m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;

                        MessageBox.Show("LD Stacker0 Z 축, Full 센서가 On 되는 위치까지 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                /// <summary>
                /// Full Sensor 감지되지 않는 상태일 경우 - 완료
                /// </summary>
                /// 



                /// <summary>
                /// Full Sensor 감지 후 추가 이동 - 시작
                /// </summary>
                case (int)StackerModulePickupWaitingPos_Step.StackerZ_Move_OverDistance:                               //  Stacker Z 축, 최종 감지 위치에서 추가로 이동

                    if (MC_Func.MC_GetDone((int)nAxis.Z0))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Stacker0 Z 축, Full 센서가 On 되는 위치에서 추가 이동 시작");

                        loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Stacker0_Top");

                        //  Target Position 변경 : 현재 위치에서 추가 이동
                        if (Equipment.Machine_Stacker_TopCheck_OverDistance > 1.0)
                        {
                            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0] = MC_Func.MC_GetEncPos((int)nAxis.Z0) + 1.0;
                        }
                        else
                        {
                            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0] = MC_Func.MC_GetEncPos((int)nAxis.Z0) + Equipment.Machine_Stacker_TopCheck_OverDistance;
                        }

                        //  속도 (기본 속도 / 4)
                        //m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z0].Common_MoveSpeed / 4.0;
                        m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z0].Common_Speed_Fine / 4.0;

                        //  가감속 배율
                        m_dSpeedMag_forAccDec = 2.0;

                        MC_Func.MC_MovePosition((int)nAxis.Z0,
                                            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0],
                                            m_dSpeed_Stacker_MoreSlow,
                                            m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec,
                                            m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec);

                        //MC_Func.MC_MovePosition((int)UnloaderParameter.AxisAjinEnum.Z0,
                        //                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
                        //                    unloaderParameter.stUnloaderPosParam.dVel[(int)UnloaderParameter.MotionKey.Z0],
                        //                    unloaderParameter.stUnloaderPosParam.dAcc[(int)UnloaderParameter.MotionKey.Z0],
                        //                    unloaderParameter.stUnloaderPosParam.dDec[(int)UnloaderParameter.MotionKey.Z0]);

                        TickCount_Start((int)TickType.TICK_LDSZ0);

                        m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_Move_OverDistance_DoneCheck;
                    }
                    break;


                case (int)StackerModulePickupWaitingPos_Step.StackerZ_Move_OverDistance_DoneCheck:                          //  Stacker Z 축, 최종 감지 위치에서 추가로 이동 완료 확인

                    //if (MC_Func.MC_GetDone((int)nAxis.Z0) && MC_Func.MC_PosTolerance((int)nAxis.Z0, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0]))
                    if (MC_Func.MC_GetDone((int)nAxis.Z0))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Stacker0 Z 축, Top 위치 Over 까지 이동 완료");

                        m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.Complete;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDSZ0) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Stacker0 Z 축, Top 위치 Over 까지 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;
                        return AlarmPost(AlarmKey.LD_Stacker0_FullSensor_Off_MoveFail);
                        m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;

                        MessageBox.Show("LD Stacker0 Z 축, Top 위치 Over 까지 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                /// <summary>
                /// Full Sensor 감지 후 추가 이동 - 완료
                /// </summary>



                case (int)StackerModulePickupWaitingPos_Step.Complete:

                    Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "완료");

                    m_bStacker0_Complete = true;

                    //  m_bHomeOK = true;

                    //timer_Motion_Home.Enabled = false;
                    //m_btimer_Motion_Home_Stop = true;

                    m_strTemp = "===  LD Stacker0 작업위치 이동 완료  ===";

                    m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;

                    //MessageBox.Show(m_strTemp, "Information!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
            return 0;
        }

        private int StackerModulePickupWaitingPos_StepProcess_Condition_Check()
        {
            int ret = 0;
            if (m_nLoader_Transfer_Step > (int)Loader_Transfer_Step.None)                                                       //  Transfer Cycle 이 동작중
            {
                Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Transfer 가 동작중이므로 Stacker 동작 중지.");

                //  일단 Out. (Transfer 동작이 완료되면 진행하도록 대기할 것인지는 테스트 하면서 결정하기로 함)
                m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;
            }
            else if (!workStage.m_bMainWorkCycle_DryRun && loaderParameter.DI_Loader_Stacker_MaterialCheck((int)LoaderParameter.StackerTable.Stacker_0))               //  우측 Port 에 Module 이 감지되어 있을 때만 진행
            {
                Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Stacker_0 Module Exist 센서 감지됨");

                if (!loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_0))           //  감지 시 Off
                {
                    //  Full Sensor 감지 상태일 경우 (Off 될 때 까지 내림 -> Off 되면 Stop -> 느리게 Up -> On 되면 Stop -> 느리게 Down -> Off 되면 Stop -> 더 느리게 Up -> On 되면 Stop -> 완료)
                    m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_FastDown;
                }
                else
                {
                    //  Full Sensor 감지되지 않는 상태일 경우 (Up -> On 되면 Stop -> 느리게 Down -> Off 되면 Stop -> 더 느리게 Up -> On 되면 Stop -> 완료)
                    m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType2_FastUp;
                }
            }
            else if (workStage.m_bMainWorkCycle_DryRun)
            {
                Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Dry Run 모드이므로 Cycle 진행");

                if (!loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_0))           //  감지 시 Off
                {
                    //  Full Sensor 감지 상태일 경우 (Off 될 때 까지 내림 -> Off 되면 Stop -> 느리게 Up -> On 되면 Stop -> 느리게 Down -> Off 되면 Stop -> 더 느리게 Up -> On 되면 Stop -> 완료)
                    m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_FastDown;
                }
                else
                {
                    //  Full Sensor 감지되지 않는 상태일 경우 (Up -> On 되면 Stop -> 느리게 Down -> Off 되면 Stop -> 더 느리게 Up -> On 되면 Stop -> 완료)
                    m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType2_FastUp;
                }
            }
            else if (Equipment.SeqTestMode)
            {
                Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Seq. Test 모드이므로 Cycle 진행");

                if (!loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_0))           //  감지 시 Off
                {
                    //  Full Sensor 감지 상태일 경우 (Off 될 때 까지 내림 -> Off 되면 Stop -> 느리게 Up -> On 되면 Stop -> 느리게 Down -> Off 되면 Stop -> 더 느리게 Up -> On 되면 Stop -> 완료)
                    m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_FastDown;
                }
                else
                {
                    //  Full Sensor 감지되지 않는 상태일 경우 (Up -> On 되면 Stop -> 느리게 Down -> Off 되면 Stop -> 더 느리게 Up -> On 되면 Stop -> 완료)
                    m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType2_FastUp;
                }
            }
            else
            {
                Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Stacker Module Exist 센서 감지 안됨");

                //  알람 정지 (LED Bar - Red Blink)
                Equipment.MachineStop_byAlarm = true;

                
                //timer_Motion_Home.Enabled = false;
                //m_btimer_Motion_Home_Stop = true;

                m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;

                MessageBox.Show("LD Stacker0 에 Module 이 감지되지 않음.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return ret;
        }

        int Run_Stacker1Module_PickupWaitingPos_Func()
        {
            int ret = 0;
            bool m_bRet = false;
            string m_strTemp;

            double m_dSpeed_Stacker_Fast = 0.0;
            double m_dSpeed_Stacker_Slow = 0.0;
            double m_dSpeed_Stacker_MoreSlow = 0.0;
            double m_dSpeedMag_forAccDec = 0.0;


            //  운전 중 Door 를 열면 장비 Stop
            if (m_nStacker1_ModulePickupWaitingPos_Step >= (int)StackerModulePickupWaitingPos_Step.Start)
            {
                
            }

            if (!loaderParameter.DI_Loader_Stacker_MaterialCheck((int)LoaderParameter.StackerTable.Stacker_1))
            {
                Equipment.Loader_LPort_Pause = true;
            }


            //  자동운전 시, Stacker1 동작 조건 : TR Cycle (None), Stacker1 Cycle (None), TR 이 Module 을 집어갔을 때
            if (Equipment.AutoRunStatus &&

                !Equipment.Loader_LPort_Pause &&

                m_nLoader_Transfer_Step == (int)Loader_Transfer_Step.None &&
                m_nStacker1_ModulePickupWaitingPos_Step == (int)StackerModulePickupWaitingPos_Step.None &&

                m_bStacker1_Run_byUser &&

                !m_bStacker1_Complete)                                                //  Stacker1 동작 완료되지 않은 상태 (TR 이 Module 을 집어간 후 false 로 변경됨)
            {
                Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "시작 Flag");

                m_bStacker1_Run_byUser = false;

                m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.Start;
            }


            //  자동 운전 Flag 를 On 시키는 조건 : m_bStacker1_Run_byUser 요거가 true 일 때만 Stacker1 동작
            if (Equipment.AutoRunStatus && !Equipment.Loader_LPort_Pause)
            {
                if (workStage.m_bMainWorkCycle_DryRun)          //  Dry Run
                {
                    if (!m_bStacker1_Run_byUser &&
                        !m_bStacker1_Complete &&

                        (m_nLoaderTransfer_ProcessStep == (int)LoaderTransferProcessStep.LoaderStep_ModulePickup_fromStacker))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "Dry Run 시작 Flag On");

                        m_bStacker1_Run_byUser = true;
                    }
                }
                else                                            //  자동 운전
                {
                    if (!m_bStacker1_Run_byUser &&
                        !m_bStacker1_Complete &&

                        (m_nLoaderTransfer_ProcessStep == (int)LoaderTransferProcessStep.LoaderStep_ModulePickup_fromStacker) &&

                        loaderParameter.DI_Loader_Stacker_MaterialCheck((int)LoaderParameter.StackerTable.Stacker_1))               //  Stacker1 에 Module 이 감지되어 있을 때만 진행
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "Auto Run 시작 Flag On");

                        m_bStacker1_Run_byUser = true;
                    }
                }
            }


            switch (m_nStacker1_ModulePickupWaitingPos_Step)
            {
                case (int)StackerModulePickupWaitingPos_Step.Start:
                    Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "시작");

                    Equipment.MachineStop_byAlarm = false;

                    MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.Z1, 2000);

                    m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.Process_Condition_Check;
                    break;


                case (int)StackerModulePickupWaitingPos_Step.Process_Condition_Check:
                    Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "동작 조건 확인");

                    if (m_nLoader_Transfer_Step > (int)Loader_Transfer_Step.None)                                                       //  Transfer Cycle 이 동작중
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "Transfer 가 동작중이므로 Stacker 동작 중지.");

                        //  일단 Out. (Transfer 동작이 완료되면 진행하도록 대기할 것인지는 테스트 하면서 결정하기로 함)
                        m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;
                    }
                    else if (!workStage.m_bMainWorkCycle_DryRun && loaderParameter.DI_Loader_Stacker_MaterialCheck((int)LoaderParameter.StackerTable.Stacker_1))               //  좌측 Port 에 Module 이 감지되어 있을 때만 진행
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "Stacker_1 Module Exist 센서 감지됨");

                        if (!loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_1))           //  감지 시 Off
                        {
                            //  Full Sensor 감지 상태일 경우 (Off 될 때 까지 내림 -> Off 되면 Stop -> 느리게 Up -> On 되면 Stop -> 느리게 Down -> Off 되면 Stop -> 더 느리게 Up -> On 되면 Stop -> 완료)
                            m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_FastDown;
                        }
                        else
                        {
                            //  Full Sensor 감지되지 않는 상태일 경우 (Up -> On 되면 Stop -> 느리게 Down -> Off 되면 Stop -> 더 느리게 Up -> On 되면 Stop -> 완료)
                            m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType2_FastUp;
                        }
                    }
                    else if (workStage.m_bMainWorkCycle_DryRun)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "Dry Run 모드이므로 Cycle 진행");

                        if (!loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_1))           //  감지 시 Off
                        {
                            //  Full Sensor 감지 상태일 경우 (Off 될 때 까지 내림 -> Off 되면 Stop -> 느리게 Up -> On 되면 Stop -> 느리게 Down -> Off 되면 Stop -> 더 느리게 Up -> On 되면 Stop -> 완료)
                            m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_FastDown;
                        }
                        else
                        {
                            //  Full Sensor 감지되지 않는 상태일 경우 (Up -> On 되면 Stop -> 느리게 Down -> Off 되면 Stop -> 더 느리게 Up -> On 되면 Stop -> 완료)
                            m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType2_FastUp;
                        }
                    }
                    else if (Equipment.SeqTestMode)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "Seq. Test 모드이므로 Cycle 진행");

                        if (!loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_1))           //  감지 시 Off
                        {
                            //  Full Sensor 감지 상태일 경우 (Off 될 때 까지 내림 -> Off 되면 Stop -> 느리게 Up -> On 되면 Stop -> 느리게 Down -> Off 되면 Stop -> 더 느리게 Up -> On 되면 Stop -> 완료)
                            m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_FastDown;
                        }
                        else
                        {
                            //  Full Sensor 감지되지 않는 상태일 경우 (Up -> On 되면 Stop -> 느리게 Down -> Off 되면 Stop -> 더 느리게 Up -> On 되면 Stop -> 완료)
                            m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType2_FastUp;
                        }
                    }
                    else
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "Stacker1 Module Exist 센서 감지 안됨");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        return AlarmPost(AlarmKey.LD_Stacker1_ModulePickupWaitingPos_Step_No_More_Material);
                        m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;

                        //MessageBox.Show("LD Stacker1 에 Module 이 감지되지 않음.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                /// <summary>
                /// Full Sensor 감지 상태일 경우 - 시작
                /// </summary>
                case (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_FastDown:                            //  Stacker Z 축, 빠르게 내림 (최 하단까지)

                    StackerModulePickupWaitingPos_Step_StackerZ_MoveType1_FastDown(out m_dSpeed_Stacker_Fast, out m_dSpeedMag_forAccDec);

                    m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_FastDown_DoneCheck;
                    break;


                case (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_FastDown_DoneCheck:                       //  Stacker Z 축, 빠르게 내림, 이동 완료 확인

                    if (loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_1))          //  Full 감지 센서가 Off 되면 Stop         //  감지 시 Off
                    {
                        MC_Func.MC_MotorStop((int)nAxis.Z1, 2000);

                        m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_SlowUp;
                    }
                    else if (MC_Func.MC_GetDone((int)nAxis.Z1) && MC_Func.MC_PosTolerance((int)nAxis.Z1, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z1]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "Stacker1 Z 축, Bottom 위치까지 이동 완료");

                        if (!loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_1))           //  감지 시 Off
                        {
                            Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "Stacker1 Z 축, Bottom 위치까지 이동했으나 Full 센서 On 상태");

                            m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;
                            return AlarmPost(AlarmKey.LD_Stacker1_ModulePickupWaitingPos_Step_Too_Many_Material);
                            //MessageBox.Show("LD Stacker1 Z 축, 자재가 너무 많거나 Full 수위 감지 센서 점검이 필요합니다.", "Information!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_SlowUp;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDSZ1) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "Stacker1 Z 축, Full 센서가 Off 되는 위치까지 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        return AlarmPost(AlarmKey.LD_Stacker1_ModuleWork_PosSet);
                        m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;

                        MessageBox.Show("LD Stacker1 Z 축, Full 센서가 Off 되는 위치까지 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_SlowUp:                               //  Stacker Z 축, 느리게 올림 (최 상단까지)
                    if (MC_Func.MC_GetDone((int)nAxis.Z1))
                    {
                        StackerModulePickupWaitingPos_Step_StackerZ_MoveType1_SlowUp(out m_dSpeed_Stacker_Slow, out m_dSpeedMag_forAccDec);

                        m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_SlowUp_DoneCheck;
                    }
                    break;


                case (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_SlowUp_DoneCheck:                          //  Stacker Z 축, 느리게 올림, 이동 완료 확인

                    if (!loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_1))           //  감지 시 Off
                    {
                        MC_Func.MC_MotorStop((int)nAxis.Z1, 2000);

                        m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_Slow2Down;
                    }
                    else if (MC_Func.MC_GetDone((int)nAxis.Z1) && MC_Func.MC_PosTolerance((int)nAxis.Z1, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z1]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "Stacker1 Z 축, Top 위치까지 이동 완료");

                        if (loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_1))          //  감지 시 Off
                        {
                            Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "Stacker1 Z 축, Top 위치까지 이동했으나 Full 센서 Off 상태");

                            m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;

                            Equipment.Loader_LPort_Pause = true;            //  자재는 감지되지만 Full 센서가 인식되지 않음. 
                            return AlarmPost(AlarmKey.LD_Stacker1_ModulePickupWaitingPos_Step_No_More_Material);
                            MessageBox.Show("LD Stacker1 Z 축, 자재가 없습니다.", "Information!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_Slow2Down;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDSZ1) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "Stacker1 Z 축, Full 센서가 On 되는 위치까지 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;
                        return AlarmPost(AlarmKey.LD_Stacker1_Module_Move_To_Loading_Position_Fail);
                        m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;

                        MessageBox.Show("LD Stacker1 Z 축, Full 센서가 On 되는 위치까지 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_Slow2Down:                            //  Stacker Z 축, 더 느리게 내림 (최 하단까지)
                    if (MC_Func.MC_GetDone((int)nAxis.Z1))
                    {
                        StackerModulePickupWaitingPos_Step_StackerZ_MoveType1_Slow2Down(out m_dSpeed_Stacker_MoreSlow, out m_dSpeedMag_forAccDec);

                        m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_Slow2Down_DoneCheck;
                    }
                    break;


                case (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_Slow2Down_DoneCheck:                       //  Stacker Z 축, 더 느리게 내림, 이동 완료 확인

                    if (loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_1))          //  감지 시 Off
                    {
                        MC_Func.MC_MotorStop((int)nAxis.Z1, 2000);

                        m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_Slow3Up;
                    }
                    else if (MC_Func.MC_GetDone((int)nAxis.Z1) && MC_Func.MC_PosTolerance((int)nAxis.Z1, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z1]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "Stacker1 Z 축, Bottom 위치까지 이동 완료");

                        if (!loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_1))           //  감지 시 Off
                        {
                            Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "Stacker1 Z 축, Bottom 위치까지 이동했으나 Full 센서 On 상태");

                            m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;
                        }
                        else
                        {
                            m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_Slow3Up;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDSZ1) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "Stacker1 Z 축, Full 센서가 Off 되는 위치까지 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        return AlarmPost(AlarmKey.StackerZ_Move_OverDistance_Fail);
                    
                        
                        m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;

                        MessageBox.Show("LD Stacker1 Z 축, Full 센서가 Off 되는 위치까지 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_Slow3Up:                               //  Stacker Z 축, 더더 느리게 올림 (최 상단까지)
                    if (MC_Func.MC_GetDone((int)nAxis.Z1))
                    {
                        StackerModulePickupWaitingPos_Step_StackerZ_MoveType1_Slow3Up(out m_dSpeed_Stacker_MoreSlow, out m_dSpeedMag_forAccDec);

                        m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_Slow3Up_DoneCheck;
                    }
                    break;


                case (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType1_Slow3Up_DoneCheck:                          //  Stacker Z 축, 더더 느리게 올림, 이동 완료 확인

                    if (!loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_1))           //  감지 시 Off
                    {
                        MC_Func.MC_MotorStop((int)nAxis.Z1, 2000);

                        //m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.Complete;
                        m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_Move_OverDistance;

                    }
                    else if (MC_Func.MC_GetDone((int)nAxis.Z1) && MC_Func.MC_PosTolerance((int)nAxis.Z1, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z1]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "Stacker1 Z 축, Top 위치까지 이동 완료");

                        if (loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_1))          //  감지 시 Off
                        {
                            Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "Stacker1 Z 축, Top 위치까지 이동했으나 Full 센서 Off 상태");

                            m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;

                            Equipment.Loader_LPort_Pause = true;            //  자재는 감지되지만 Full 센서가 인식되지 않음. 

                            MessageBox.Show("LD Stacker1 Z 축, 자재가 없습니다.", "Information!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            //m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.Complete;
                            m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_Move_OverDistance;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDSZ1) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "Stacker1 Z 축, Full 센서가 On 되는 위치까지 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;
                        return AlarmPost(AlarmKey.LD_Stacker1_Work_Pos_Move_To_Full_Sensor_Position);
                        m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;

                        MessageBox.Show("LD Stacker1 Z 축, Full 센서가 On 되는 위치까지 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                /// <summary>
                /// Full Sensor 감지 상태일 경우 - 완료
                /// </summary>



                /// <summary>
                /// Full Sensor 감지되지 않는 상태일 경우 - 시작
                /// </summary>
                case (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType2_FastUp:                               //  Stacker Z 축, 빠르게 올림 (최 상단까지)

                    StackerModulePickupWaitingPos_Step_StackerZ_MoveType2_FastUp(out m_dSpeed_Stacker_Fast, out m_dSpeedMag_forAccDec);

                    m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType2_FastUp_DoneCheck;
                    break;


                case (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType2_FastUp_DoneCheck:                          //  Stacker Z 축, 빠르게 올림, 이동 완료 확인

                    if (!loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_1))           //  감지 시 Off
                    {
                        MC_Func.MC_MotorStop((int)nAxis.Z1, 2000);

                        m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType2_Slow2Down;
                    }
                    else if (MC_Func.MC_GetDone((int)nAxis.Z1) && MC_Func.MC_PosTolerance((int)nAxis.Z1, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z1]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "Stacker1 Z 축, Top 위치까지 이동 완료");

                        if (loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_1))          //  감지 시 Off
                        {
                            Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "Stacker1 Z 축, Top 위치까지 이동했으나 Full 센서 Off 상태");

                            m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;

                            Equipment.Loader_LPort_Pause = true;            //  자재는 감지되지만 Full 센서가 인식되지 않음. 

                            MessageBox.Show("LD Stacker1 Z 축, 자재가 없습니다.", "Information!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType2_Slow2Down;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDSZ1) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "Stacker1 Z 축, Full 센서가 On 되는 위치까지 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        return AlarmPost(AlarmKey.LD_Stacker1_Work_Pos_Move_To_Full_Sensor_Position);
                        m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;

                        MessageBox.Show("LD Stacker1 Z 축, Full 센서가 On 되는 위치까지 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType2_Slow2Down:                            //  Stacker Z 축, 더 느리게 내림 (최 하단까지)

                    if (MC_Func.MC_GetDone((int)nAxis.Z1))
                    {
                        StackerModulePickupWaitingPos_Step_StackerZ_MoveType2_Slow2Down(out m_dSpeed_Stacker_MoreSlow, out m_dSpeedMag_forAccDec);

                        m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType2_Slow2Down_DoneCheck;
                    }
                    break;


                case (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType2_Slow2Down_DoneCheck:                       //  Stacker Z 축, 더 느리게 내림, 이동 완료 확인

                    if (loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_1))          //  감지 시 Off
                    {
                        MC_Func.MC_MotorStop((int)nAxis.Z1, 2000);

                        m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType2_Slow3Up;
                    }
                    else if (MC_Func.MC_GetDone((int)nAxis.Z1) && MC_Func.MC_PosTolerance((int)nAxis.Z1, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z1]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "Stacker1 Z 축, Bottom 위치까지 이동 완료");

                        if (!loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_1))           //  감지 시 Off
                        {
                            Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "Stacker1 Z 축, Bottom 위치까지 이동했으나 Full 센서 On 상태");

                            m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;
                        }
                        else
                        {
                            m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType2_Slow3Up;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDSZ1) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "Stacker1 Z 축, Full 센서가 Off 되는 위치까지 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;
                        return AlarmPost(AlarmKey.LD_Stacker1_Work_Pos_Move_To_Full_Sensor_Position);
                        m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;

                        MessageBox.Show("LD Stacker1 Z 축, Full 센서가 Off 되는 위치까지 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType2_Slow3Up:                               //  Stacker Z 축, 더더 느리게 올림 (최 상단까지)

                    if (MC_Func.MC_GetDone((int)nAxis.Z1))
                    {
                        StackerModulePickupWaitingPos_Step_StackerZ_MoveType2_Slow3Up(out m_dSpeed_Stacker_MoreSlow, out m_dSpeedMag_forAccDec);

                        m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType2_Slow3Up_DoneCheck;
                    }
                    break;


                case (int)StackerModulePickupWaitingPos_Step.StackerZ_MoveType2_Slow3Up_DoneCheck:                          //  Stacker Z 축, 더더 느리게 올림, 이동 완료 확인

                    if (!loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_1))           //  감지 시 Off
                    {
                        MC_Func.MC_MotorStop((int)nAxis.Z1, 2000);

                        //m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.Complete;
                        m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_Move_OverDistance;
                    }
                    else if (MC_Func.MC_GetDone((int)nAxis.Z1) && MC_Func.MC_PosTolerance((int)nAxis.Z1, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z1]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "Stacker1 Z 축, Top 위치까지 이동 완료");

                        if (loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_1))          //  감지 시 Off
                        {
                            Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "Stacker1 Z 축, Top 위치까지 이동했으나 Full 센서 Off 상태");

                            m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;

                            Equipment.Loader_LPort_Pause = true;            //  자재는 감지되지만 Full 센서가 인식되지 않음. 

                            MessageBox.Show("LD Stacker1 Z 축, 자재가 없습니다.", "Information!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            //m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.Complete;
                            m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_Move_OverDistance;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDSZ1) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "Stacker1 Z 축, Full 센서가 On 되는 위치까지 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;
                        return AlarmPost(AlarmKey.LD_Stacker1_Work_Pos_Move_To_Full_Sensor_Position);
                        m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;

                        MessageBox.Show("LD Stacker1 Z 축, Full 센서가 On 되는 위치까지 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                /// <summary>
                /// Full Sensor 감지되지 않는 상태일 경우 - 완료
                /// </summary>



                /// <summary>
                /// Full Sensor 감지 후 추가 이동 - 시작
                /// </summary>
                case (int)StackerModulePickupWaitingPos_Step.StackerZ_Move_OverDistance:                               //  Stacker Z 축, 최종 감지 위치에서 추가로 이동

                    if (MC_Func.MC_GetDone((int)nAxis.Z1))
                    {
                        StackerModulePickupWaitingPos_Step_StackerZ_Move_OverDistance(out m_dSpeed_Stacker_MoreSlow, out m_dSpeedMag_forAccDec);

                        m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.StackerZ_Move_OverDistance_DoneCheck;
                    }
                    break;


                case (int)StackerModulePickupWaitingPos_Step.StackerZ_Move_OverDistance_DoneCheck:                          //  Stacker Z 축, 최종 감지 위치에서 추가로 이동 완료 확인

                    //if (MC_Func.MC_GetDone((int)nAxis.Z1) && MC_Func.MC_PosTolerance((int)nAxis.Z1, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z1]))
                    if (MC_Func.MC_GetDone((int)nAxis.Z1))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "Stacker1 Z 축, Top 위치 Over 까지 이동 완료");

                        m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.Complete;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDSZ1) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "Stacker1 Z 축, Top 위치 Over 까지 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;
                        return AlarmPost(AlarmKey.LD_Stacker1_Work_Pos_Move_To_Full_Sensor_Position);
                        m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;

                        MessageBox.Show("LD Stacker1 Z 축, Top 위치 Over 까지 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                /// <summary>
                /// Full Sensor 감지 후 추가 이동 - 완료
                /// </summary>



                case (int)StackerModulePickupWaitingPos_Step.Complete:

                    Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "완료");

                    m_bStacker1_Complete = true;

                    //  m_bHomeOK = true;

                    //timer_Motion_Home.Enabled = false;
                    //m_btimer_Motion_Home_Stop = true;

                    m_strTemp = "===  LD Stacker1 작업위치 이동 완료  ===";

                    m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;

                    //MessageBox.Show(m_strTemp, "Information!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
            return 0;
        }

        private void StackerModulePickupWaitingPos_Step_StackerZ_Move_OverDistance(out double m_dSpeed_Stacker_MoreSlow, out double m_dSpeedMag_forAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Stacker1 Z 축, Full 센서가 On 되는 위치에서 추가 이동 시작");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Stacker1_Top");

            //  Target Position 변경 : 현재 위치에서 추가 이동
            if (Equipment.Machine_Stacker_TopCheck_OverDistance > 1.0)
            {
                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z1] = MC_Func.MC_GetEncPos((int)nAxis.Z1) + 1.0;
            }
            else
            {
                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z1] = MC_Func.MC_GetEncPos((int)nAxis.Z1) + Equipment.Machine_Stacker_TopCheck_OverDistance;
            }

            //  속도 (기본 속도 / 4)
            //m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z0].Common_MoveSpeed / 4.0;
            m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z1].Common_Speed_Fine / 4.0;

            //  가감속 배율
            m_dSpeedMag_forAccDec = 2.0;

            MC_Func.MC_MovePosition((int)nAxis.Z1,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z1],
                                m_dSpeed_Stacker_MoreSlow,
                                m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec,
                                m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec);

            //MC_Func.MC_MovePosition((int)UnloaderParameter.AxisAjinEnum.Z0,
            //                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dVel[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dAcc[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dDec[(int)UnloaderParameter.MotionKey.Z0]);

            TickCount_Start((int)TickType.TICK_LDSZ1);
        }

        private void StackerModulePickupWaitingPos_Step_StackerZ_MoveType2_Slow3Up(out double m_dSpeed_Stacker_MoreSlow, out double m_dSpeedMag_forAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "Stacker1 Z 축, Full 센서가 On 되는 위치까지 이동 시작 (저속 / 2)");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Stacker1_Top");

            //  Target Position 변경 : 맨 위로 올라가는 위치
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z1] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_LPort_TopPos].LD_Stacker_Z1;

            //  속도 (기본 속도 / 4)
            //m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z0].Common_MoveSpeed / 4.0;
            m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z1].Common_Speed_Fine / 4.0;

            //  가감속 배율
            m_dSpeedMag_forAccDec = 2.0;

            MC_Func.MC_MovePosition((int)nAxis.Z1,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z1],
                                m_dSpeed_Stacker_MoreSlow,
                                m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec,
                                m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec);

            //MC_Func.MC_MovePosition((int)UnloaderParameter.AxisAjinEnum.Z0,
            //                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dVel[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dAcc[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dDec[(int)UnloaderParameter.MotionKey.Z0]);

            TickCount_Start((int)TickType.TICK_LDSZ1);
        }

        private void StackerModulePickupWaitingPos_Step_StackerZ_MoveType2_Slow2Down(out double m_dSpeed_Stacker_MoreSlow, out double m_dSpeedMag_forAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "Stacker1 Z 축, Full 센서가 Off 되는 위치까지 이동 시작 (저속)");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Stacker1_Bottom");

            //  Target Position 변경 : 맨 아래로 내려가는 위치
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z1] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_LPort_ReadyPos].LD_Stacker_Z1;

            //  속도 (기본 속도 / 3)
            //m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z0].Common_MoveSpeed / 3.0;
            m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z1].Common_Speed_Fine / 3.0;

            //  가감속 배율
            m_dSpeedMag_forAccDec = 2.0;

            MC_Func.MC_MovePosition((int)nAxis.Z1,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z1],
                                m_dSpeed_Stacker_MoreSlow,
                                m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec,
                                m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec);

            //MC_Func.MC_MovePosition((int)UnloaderParameter.AxisAjinEnum.Z0,
            //                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dVel[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dAcc[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dDec[(int)UnloaderParameter.MotionKey.Z0]);

            TickCount_Start((int)TickType.TICK_LDSZ1);
        }

        private void StackerModulePickupWaitingPos_Step_StackerZ_MoveType2_FastUp(out double m_dSpeed_Stacker_Fast, out double m_dSpeedMag_forAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "Stacker1 Z 축, Full 센서가 On 되는 위치까지 이동 시작 (고속)");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Stacker1_Top");

            //  Target Position 변경 : 맨 위로 올라가는 위치
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z1] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_LPort_TopPos].LD_Stacker_Z1;

            //  속도 (기본 속도)
            m_dSpeed_Stacker_Fast = Equipment.stAxisParam[(int)nAxis.Z1].Common_Speed_Fine;

            //  가감속 배율
            m_dSpeedMag_forAccDec = 2.0;

            MC_Func.MC_MovePosition((int)nAxis.Z1,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z1],
                                m_dSpeed_Stacker_Fast,
                                m_dSpeed_Stacker_Fast * m_dSpeedMag_forAccDec,
                                m_dSpeed_Stacker_Fast * m_dSpeedMag_forAccDec);

            //MC_Func.MC_MovePosition((int)UnloaderParameter.AxisAjinEnum.Z0,
            //                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dVel[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dAcc[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dDec[(int)UnloaderParameter.MotionKey.Z0]);

            TickCount_Start((int)TickType.TICK_LDSZ1);
        }

        private void StackerModulePickupWaitingPos_Step_StackerZ_MoveType1_Slow3Up(out double m_dSpeed_Stacker_MoreSlow, out double m_dSpeedMag_forAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "Stacker1 Z 축, Full 센서가 On 되는 위치까지 이동 시작 (저속 / 2)");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Stacker1_Top");

            //  Target Position 변경 : 맨 위로 올라가는 위치
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z1] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_LPort_TopPos].LD_Stacker_Z1;

            //  속도 (기본 속도 / 4)
            //m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z0].Common_MoveSpeed / 4.0;
            m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z1].Common_Speed_Fine / 4.0;

            //  가감속 배율
            m_dSpeedMag_forAccDec = 2.0;

            MC_Func.MC_MovePosition((int)nAxis.Z1,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z1],
                                m_dSpeed_Stacker_MoreSlow,
                                m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec,
                                m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec);

            //MC_Func.MC_MovePosition((int)UnloaderParameter.AxisAjinEnum.Z0,
            //                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dVel[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dAcc[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dDec[(int)UnloaderParameter.MotionKey.Z0]);

            TickCount_Start((int)TickType.TICK_LDSZ1);
        }

        private void StackerModulePickupWaitingPos_Step_StackerZ_MoveType1_Slow2Down(out double m_dSpeed_Stacker_MoreSlow, out double m_dSpeedMag_forAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "Stacker1 Z 축, Full 센서가 Off 되는 위치까지 이동 시작 (저속)");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Stacker1_Bottom");

            //  Target Position 변경 : 맨 아래로 내려가는 위치
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z1] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_LPort_ReadyPos].LD_Stacker_Z1;

            //  속도 (기본 속도 / 3)
            //m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z0].Common_MoveSpeed / 3.0;
            m_dSpeed_Stacker_MoreSlow = Equipment.stAxisParam[(int)nAxis.Z1].Common_Speed_Fine / 3.0;

            //  가감속 배율
            m_dSpeedMag_forAccDec = 2.0;

            MC_Func.MC_MovePosition((int)nAxis.Z1,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z1],
                                m_dSpeed_Stacker_MoreSlow,
                                m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec,
                                m_dSpeed_Stacker_MoreSlow * m_dSpeedMag_forAccDec);

            //MC_Func.MC_MovePosition((int)UnloaderParameter.AxisAjinEnum.Z0,
            //                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dVel[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dAcc[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dDec[(int)UnloaderParameter.MotionKey.Z0]);

            TickCount_Start((int)TickType.TICK_LDSZ1);
        }

        private void StackerModulePickupWaitingPos_Step_StackerZ_MoveType1_SlowUp(out double m_dSpeed_Stacker_Slow, out double m_dSpeedMag_forAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "Stacker1 Z 축, Full 센서가 On 되는 위치까지 이동 시작 (중속)");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Stacker1_Top");

            //  Target Position 변경 : 맨 위로 올라가는 위치
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z1] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_LPort_TopPos].LD_Stacker_Z1;

            //  속도 (기본 속도 / 2)
            m_dSpeed_Stacker_Slow = Equipment.stAxisParam[(int)nAxis.Z1].Common_Speed_Fine / 2.0;

            //  가감속 배율
            m_dSpeedMag_forAccDec = 2.0;

            MC_Func.MC_MovePosition((int)nAxis.Z1,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z1],
                                m_dSpeed_Stacker_Slow,
                                m_dSpeed_Stacker_Slow * m_dSpeedMag_forAccDec,
                                m_dSpeed_Stacker_Slow * m_dSpeedMag_forAccDec);

            //MC_Func.MC_MovePosition((int)UnloaderParameter.AxisAjinEnum.Z0,
            //                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dVel[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dAcc[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dDec[(int)UnloaderParameter.MotionKey.Z0]);

            TickCount_Start((int)TickType.TICK_LDSZ1);
        }

        private void StackerModulePickupWaitingPos_Step_StackerZ_MoveType1_FastDown(out double m_dSpeed_Stacker_Fast, out double m_dSpeedMag_forAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "Stacker1 Z 축, Full 센서가 Off 되는 위치까지 이동 시작 (고속)");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Stacker1_Bottom");

            //  Target Position 변경 : 맨 아래로 내려가는 위치
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z1] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_LPort_ReadyPos].LD_Stacker_Z1;

            //  속도 (기본 속도)
            m_dSpeed_Stacker_Fast = Equipment.stAxisParam[(int)nAxis.Z1].Common_Speed_Fine;

            //  가감속 배율
            m_dSpeedMag_forAccDec = 2.0;

            MC_Func.MC_MovePosition((int)nAxis.Z1,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z1],
                                m_dSpeed_Stacker_Fast,
                                m_dSpeed_Stacker_Fast * m_dSpeedMag_forAccDec,
                                m_dSpeed_Stacker_Fast * m_dSpeedMag_forAccDec);

            //MC_Func.MC_MovePosition((int)UnloaderParameter.AxisAjinEnum.Z0,
            //                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dVel[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dAcc[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dDec[(int)UnloaderParameter.MotionKey.Z0]);

            TickCount_Start((int)TickType.TICK_LDSZ1);
        }

        private void StackerZ0_StepUp(out double m_dSpeed_Stacker_Slow, out double m_dSpeedMag_forAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Stacker0 Work Pos. Set", "Stacker0 Z 축, Step Up (1mm)");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Stacker0_Top");

            //  Target Position 변경 : 현재 위치에서 1mm 위
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0] = MC_Func.MC_GetEncPos((int)nAxis.Z0) + 1.0;

            //  이동 할 위치가 Top 위치를 벗어나는지 체크
            if (loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0] > stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_LPort_TopPos].LD_Stacker_Z0)
            {
                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_LPort_TopPos].LD_Stacker_Z0;
            }

            //  속도 (기본 속도 / 2)
            m_dSpeed_Stacker_Slow = Equipment.stAxisParam[(int)nAxis.Z0].Common_Speed_Fine / 2.0;

            //  가감속 배율
            m_dSpeedMag_forAccDec = 2.0;

            MC_Func.MC_MovePosition((int)nAxis.Z0,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0],
                                m_dSpeed_Stacker_Slow,
                                m_dSpeed_Stacker_Slow * m_dSpeedMag_forAccDec,
                                m_dSpeed_Stacker_Slow * m_dSpeedMag_forAccDec);

            //MC_Func.MC_MovePosition((int)UnloaderParameter.AxisAjinEnum.Z0,
            //                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dVel[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stsUnloaderPosParam.dAcc[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dDec[(int)UnloaderParameter.MotionKey.Z0]);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void StackerZ1_StepUp(out double m_dSpeed_Stacker_Slow, out double m_dSpeedMag_forAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Stacker1 Work Pos. Set", "Stacker1 Z 축, Step Up (1mm)");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Stacker1_Top");

            //  Target Position 변경 : 현재 위치에서 1mm 위
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z1] = MC_Func.MC_GetEncPos((int)nAxis.Z1) + 1.0;

            //  이동 할 위치가 Top 위치를 벗어나는지 체크
            if (loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z1] > stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_LPort_TopPos].LD_Stacker_Z1)
            {
                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z1] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_LPort_TopPos].LD_Stacker_Z1;
            }

            //  속도 (기본 속도 / 2)
            m_dSpeed_Stacker_Slow = Equipment.stAxisParam[(int)nAxis.Z1].Common_Speed_Fine / 2.0;

            //  가감속 배율
            m_dSpeedMag_forAccDec = 2.0;

            MC_Func.MC_MovePosition((int)nAxis.Z1,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z1],
                                m_dSpeed_Stacker_Slow,
                                m_dSpeed_Stacker_Slow * m_dSpeedMag_forAccDec,
                                m_dSpeed_Stacker_Slow * m_dSpeedMag_forAccDec);

            //MC_Func.MC_MovePosition((int)UnloaderParameter.AxisAjinEnum.Z0,
            //                    unloaderParameter.stUnloaderPosParam.dTarget[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dVel[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dAcc[(int)UnloaderParameter.MotionKey.Z0],
            //                    unloaderParameter.stUnloaderPosParam.dDec[(int)UnloaderParameter.MotionKey.Z0]);

            TickCount_Start((int)TickType.TICK_LDTR);
        }
        #endregion


        #region Transfer Cycle Function (Module PickUp & PutDown 위치로 이동)

        int Run_Transfer_Cycle_Func()
        {
            int ret = 0;
            bool m_bRet = false;
            string m_strTemp = "";

            double m_dSpeed = 0.0;
            double m_dAccDec = 0.0;


            //  운전 중 Door 를 열면 장비 Stop
            if (m_nLoader_Transfer_Step >= (int)Loader_Transfer_Step.Start)
            {   
            }


            //  자동운전 시, Transfer 동작 조건
            if (Equipment.AutoRunStatus &&
                !Equipment.Loader_Transfer_Pause &&                                     //  Loader Transfer Cycle Pause 시 동작 안되도록
                !Equipment.SocketStopped &&                                             //  Socket Stop 시 동작 안되도록
                !Equipment.CycleStopped_LoaderTransfer &&                               //  Loader 가 Cycle Stop 으로 멈추면 동작 안되도록
                !Equipment.MachineStop_byTimeout_Loader &&                              //  Loader 가 Time out 으로 멈추면 동작 안되도록
                m_nLoader_Transfer_Step == (int)Loader_Transfer_Step.None &&
                m_nMAlign_Step == (int)MAlign_Step.None) 
            {
                //  Stacker0 에서 Module 을 Pick Up 하기 위한 조건
                if (!m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker0_Complete &&

                    (m_nStacker0_ModulePickupWaitingPos_Step == (int)StackerModulePickupWaitingPos_Step.None) &&

                    m_bStacker0_Complete &&

                    (m_nLoaderTransfer_ProcessStep == (int)LoaderTransferProcessStep.LoaderStep_ModulePickup_fromStacker) &&

                    !m_bMAlignZone_ModuleExist)
                {
                    m_nLoaderTransferMoveType = (int)LoaderTransferMoveType.Cycle_Stacker0_PickUp;            //  Stacker0 에서 Module Pick Up Cycle
                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Start;
                }
                //  Stacker1 에서 Module 을 Pick Up 하기 위한 조건
                else if (!m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker1_Complete &&

                    (m_nStacker1_ModulePickupWaitingPos_Step == (int)StackerModulePickupWaitingPos_Step.None) &&

                    m_bStacker1_Complete &&

                    (m_nLoaderTransfer_ProcessStep == (int)LoaderTransferProcessStep.LoaderStep_ModulePickup_fromStacker) &&

                    !m_bMAlignZone_ModuleExist)
                {
                    m_nLoaderTransferMoveType = (int)LoaderTransferMoveType.Cycle_Stacker1_PickUp;            //  Stacker1 에서 Module Pick Up Cycle
                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Start;
                }
                //  M-Aligner 에 Module 을 Put Down 하기 위한 조건
                else if ((m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker0_Complete || m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker1_Complete) &&
                    !m_bAUTORUN_Loader_Transfer_ModulePutDowntoMAligner_Complete && 

                    (m_nMAlign_Step == (int)MAlign_Step.None) &&

                    (m_nLoaderTransfer_ProcessStep == (int)LoaderTransferProcessStep.LoaderStep_ModulePutDown_MAligner) &&

                    !m_bMAlignZone_ModuleExist)
                {
                    m_nLoaderTransferMoveType = (int)LoaderTransferMoveType.Cycle_MAligner_PutDown;           //  M-Aligner 에 Module Put Down Cycle
                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Start;
                }
                //  M-Aligner 에서 Module 을 Pick Up 하기 위한 조건
                else if (m_bAUTORUN_Loader_Transfer_ModulePutDowntoMAligner_Complete &&
                    !m_bAUTORUN_Loader_Transfer_ModulePickUpfromMAligner_Complete &&

                    (m_nMAlign_Step == (int)MAlign_Step.None) &&
                    (m_bMAlign_Complete || m_bMAlign_Retry) &&

                    !workStage.m_bMainWorkCycle_Complete &&                                                   //  Work Stage 의 완료 상태가 False 일 때 얼라인 완료된 모듈을 픽업 한다. 
                    //!m_bLoader_Transfer_ModulePutDowntoWorkStage_Complete &&                                //  Unloader 가 Work Stage 의 모듈을 가져가면, M-Aligner 에서 얼라인 완료된 모듈을 픽업 한다. 

                    //  Laser Drilling Cycle 이 완료되면 그때 Pick Up 하도록 한다.
                    //((workStage.m_bMainWorkCycle_DryRun && workStage.m_bDryRun_Complete) ||                 //  Dry Run 이면?? Dry Run 완료 확인
                    //(!workStage.m_bMainWorkCycle_DryRun && workStage.m_bLaserDrilling_Complete)) &&         //  Drilling Run 이면?? Drilling 완료 확인

                    ((workStage.m_bMainWorkCycle_DryRun && (workStage.m_nDryRun_Step == (int)WorkStage.DryRun_Step.None)) ||                        //  Dry Run 이면?? Dry Run Step None 확인
                    (!workStage.m_bMainWorkCycle_DryRun && (workStage.m_nLaserDrilling_MainStep == (int)WorkStage.LaserDrilling_Step.None))) &&     //  Drilling Run 이면?? Drilling Step None 확인

                    (m_nLoaderTransfer_ProcessStep == (int)LoaderTransferProcessStep.LoaderStep_ModulePickUp_MAligner) &&

                    m_bMAlignZone_ModuleExist)
                {
                    m_nLoaderTransferMoveType = (int)LoaderTransferMoveType.Cycle_MAligner_PickUp;           //  M-Aligner 에서 Module Pick Up Cycle
                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Start;
                }
                //  Work Stage 에 Module 을 Put Down 하기 위한 조건
                else if (m_bAUTORUN_Loader_Transfer_ModulePickUpfromMAligner_Complete &&
                    !m_bAUTORUN_Loader_Transfer_ModulePutDowntoWorkStage_Complete &&

                    (unloader.m_nUnloader_Transfer_Step == (int)Unloader_Transfer_Step.None) &&

                    (m_nLoaderTransfer_ProcessStep == (int)LoaderTransferProcessStep.LoaderStep_ModulePutDown_Stage) &&

                    (workStage.m_nLaserDrilling_MainStep == (int)WorkStage.LaserDrilling_Step.None) &&      //  Work Stage 에서 아무것도 하지 않을 때
                    (workStage.m_nWorkStage_Move_Step == (int)WorkStage.WorkStage_Move_Step.None))
                {
                    m_nLoaderTransferMoveType = (int)LoaderTransferMoveType.Cycle_WorkStage_PutDown;        //  Work Stage 에 Module Put Down Cycle
                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Start;
                }
            }



            switch (m_nLoader_Transfer_Step)
            {
                case (int)Loader_Transfer_Step.Start:
                    Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "시작");

                    Equipment.MachineStop_byAlarm = false;

                    //  Laoder Transfer XZ 축 모터 전체 Stop
                    MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.TR_X, 2000);
                    MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.TR_Z, 2000);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Process_Type_Check;
                    break;


                case (int)Loader_Transfer_Step.Process_Type_Check:
                    Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "동작 타입 확인");

                    //Cycle_Stacker0_PickUp = 0,                                      //  Module Pick Up Cycle (Stacker 0)
                    //Cycle_Stacker1_PickUp,                                          //  Module Pick Up Cycle (Stacker 1)
                    //Cycle_MAligner_PickUp,                                          //  Module Pick Up Cycle (M-Aligner)
                    //Cycle_MAligner_PutDown,                                         //  Module Put Down Cycle (M-Aligner)
                    //Cycle_WorkStage_PutDown,                                        //  Module Put Down Cycle (Work Stage)

                    switch(m_nLoaderTransferMoveType)
                    {
                        case (int)LoaderTransferMoveType.Cycle_Transfer_ReadyPos:
                            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Ready Position Move");
                            m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Transfer_Move_Condition_Check;
                            break;

                        case (int)LoaderTransferMoveType.Cycle_Stacker0_PickUp:
                            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Stacker0 에서 Module Pick Up");
                            m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker0_ModulePickup_Condition_Check;
                            break;

                        case (int)LoaderTransferMoveType.Cycle_Stacker1_PickUp:
                            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Stacker1 에서 Module Pick Up");
                            m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker1_ModulePickup_Condition_Check;
                            break;

                        case (int)LoaderTransferMoveType.Cycle_MAligner_PickUp:
                            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "M-Aligner 에서 Module Pick Up");
                            m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAligner_ModulePickup_Condition_Check;
                            break;

                        case (int)LoaderTransferMoveType.Cycle_MAligner_PutDown:
                            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "M-Aligner 에 Module Put Down");
                            m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAligner_ModulePutdown_Condition_Check;
                            break;

                        case (int)LoaderTransferMoveType.Cycle_WorkStage_PutDown:
                            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Work Stage 에 Module Put Down");
                            m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.WorkStage_ModulePutdown_Condition_Check;
                            break;

                        default:            //  Error
                            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "동작 타입 목록에 없음");
                            m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Complete;
                            break;
                    }
                    break;



                /// <summary>
                /// Transfer 대기 위치로 이동 - 시작
                /// </summary>
                case (int)Loader_Transfer_Step.Transfer_Move_Condition_Check:                            //  Stacker0 에서 Module Pick Up 조건 체크 (Stacker0 Module Exist Sensor On, Stacker0 Module Full Sensor On, Transfer Picker Vacuum Off Check, Stacker0 Cycle : None)

                    Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "TR 축, Ready Position Move 조건 체크");

                    
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "TR 축, Ready Position Move 조건 OK");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.TransferZ_Move_ReadyPos;
                    }
                    break;


                case (int)Loader_Transfer_Step.TransferZ_Move_ReadyPos:                            //  Transfer Z 축, 대기 위치로 이동

                    Loader_Transfer_Step_TransferZ_Move_ReadyPos(out m_dSpeed, out m_dAccDec);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.TransferZ_Move_ReadyPos_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.TransferZ_Move_ReadyPos_DoneCheck:                       //  Transfer Z 축, 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 완료");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.TransferX_Move_ReadyPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        return AlarmPost(AlarmKey.LD_TransferZ_Move_ReadyPos_Timeout);

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, 대기 위치로 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.TransferX_Move_ReadyPos:                            //  Transfer X 축, 대기 위치로 이동

                    Loader_Transfer_Step_TransferX_Move_ReadyPos(out m_dSpeed, out m_dAccDec);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.TransferX_Move_ReadyPos_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.TransferX_Move_ReadyPos_DoneCheck:                       //  Transfer X 축, Stacker 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_X) && MC_Func.MC_PosTolerance((int)nAxis.TR_X, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_X]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer X 축, 대기 위치로 이동 완료");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Complete;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer X 축, 대기 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        return AlarmPost(AlarmKey.LD_TransferX_Move_ReadyPos_Timeout);
                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer X 축, 대기 위치로 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                /// <summary>
                /// Transfer 대기 위치로 이동 - 완료
                /// </summary>



                /// <summary>
                /// Stacker0 에서 Module Pick Up 일 경우 - 시작
                /// </summary>
                case (int)Loader_Transfer_Step.Stacker0_ModulePickup_Condition_Check:                            //  Stacker0 에서 Module Pick Up 조건 체크 (Stacker0 Module Exist Sensor On, Stacker0 Module Full Sensor On, Transfer Picker Vacuum Off Check, Stacker0 Cycle : None)

                    Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "TR 축, Stacker0 에서 Module Pickup 조건 체크");

                    if (!workStage.m_bMainWorkCycle_DryRun && !loaderParameter.DI_Loader_Stacker_MaterialCheck((int)LoaderParameter.StackerTable.Stacker_0))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Stacker0 에 Module 이 감지되지 않음.");

                        //  Out.
                        //////////////////////////////////////////////////////////////////////////////////////////
                        //  재시작 위치 저장용
                        //
                        Equipment.MachineStop_byTimeout_Loader = true;
                        Loader_CurrentStatus_Save_StopedByTimeout();
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker1_ModulePickup_Condition_Check;
                        break;
                        return AlarmPost(AlarmKey.LD_Stacker0_ModulePickup_ConditionCheck_Stacker0ModuleNotExist);
                        //
                        //  재시작 위치 저장용
                        //////////////////////////////////////////////////////////////////////////////////////////

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;
                    }
                    else if (loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_0))         //  감지 시 Off
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Stacker0 의 Full 감지 센서에 Module 이 감지되지 않음.");

                        //  Out.

                        //////////////////////////////////////////////////////////////////////////////////////////
                        //  재시작 위치 저장용
                        //
                        Equipment.MachineStop_byTimeout_Loader = true;
                        Loader_CurrentStatus_Save_StopedByTimeout();
                        return AlarmPost(AlarmKey.LD_Stacker0_ModulePickup_ConditionCheck_Stacker0FullSensorNotExist);
                        //
                        //  재시작 위치 저장용
                        //////////////////////////////////////////////////////////////////////////////////////////

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;
                    }
                    else if (loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Inner) ||
                            loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Outer))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Picker 에 자재가 감지됨.");

                        //  Out.

                        //////////////////////////////////////////////////////////////////////////////////////////
                        //  재시작 위치 저장용
                        //
                        Equipment.MachineStop_byTimeout_Loader = true;
                        Loader_CurrentStatus_Save_StopedByTimeout();
                        //
                        //  재시작 위치 저장용
                        //////////////////////////////////////////////////////////////////////////////////////////
                        return AlarmPost(AlarmKey.LD_Stacker0_ModulePickup_ConditionCheck_PickerVacuumSensorExist);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;
                    }
                    else if (m_nStacker0_ModulePickupWaitingPos_Step > (int)StackerModulePickupWaitingPos_Step.None)                                                       //  Transfer Cycle 이 동작중
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Stacker0 이 동작중이므로 Module Pickup 동작 중지.");

                        //  Out. (Stacker 동작이 완료되면 진행하도록 대기할 것인지는 테스트 하면서 결정하기로 함)

                        //////////////////////////////////////////////////////////////////////////////////////////
                        //  재시작 위치 저장용
                        //
                        Equipment.MachineStop_byTimeout_Loader = true;
                        Loader_CurrentStatus_Save_StopedByTimeout();
                        //
                        //  재시작 위치 저장용
                        //////////////////////////////////////////////////////////////////////////////////////////
                        return AlarmPost(AlarmKey.LD_Transfer_StackerIsWorking);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;
                    }                    
                    else
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Stacker0 Module Pickup 조건 OK");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_Move_ReadyPos;
                    }
                    break;


                case (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_Move_ReadyPos:                            //  Transfer Z 축, 대기 위치로 이동

                    Loader_Transfer_Step_Stacker0PickUp_TransferZ_Move_ReadyPos(out m_dSpeed, out m_dAccDec);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_Move_ReadyPos_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_Move_ReadyPos_DoneCheck:                       //  Transfer Z 축, 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 완료");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker0PickUp_TransferX_Move_StackerPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;
                        return AlarmPost(AlarmKey.LD_TransferZ_Move_ReadyPos_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, 대기 위치로 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.Stacker0PickUp_TransferX_Move_StackerPos:                            //  Transfer X 축, Stacker 위치로 이동

                    Loader_Transfer_Step_Stacker0PickUp_TransferX_Move_StackerPos(out m_dSpeed, out m_dAccDec);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker0PickUp_TransferX_Move_StackerPos_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.Stacker0PickUp_TransferX_Move_StackerPos_DoneCheck:                       //  Transfer X 축, Stacker 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_X) && MC_Func.MC_PosTolerance((int)nAxis.TR_X, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_X]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer X 축, Stacker0 위치로 이동 완료");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_Move_PickUpPos_1stStep;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer X 축, Stacker0 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        return AlarmPost(AlarmKey.LD_TransferX_Move_StackerPos_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer X 축, Stacker0 위치로 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_Move_PickUpPos_1stStep:                            //  Transfer Z 축, Module Pick Up 대기 위치로 이동 (1단계, 최종 위치에서 위로 10 mm)

                    Loader_Transfer_Step_Stacker0PickUp_TransferZ_Move_PickUpPos_1stStep(out m_dSpeed, out m_dAccDec);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_Move_PickUpPos_1stStep_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_Move_PickUpPos_1stStep_DoneCheck:                       //  Transfer Z 축, Module Pick Up 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, Module Pickup 대기 위치로 이동 완료");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_Move_PickUpPos_2ndStep;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, Module Pickup 대기 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;
                        return AlarmPost(AlarmKey.LD_TransferZ_Move_ReadyPos_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, Module Pickup 대기 위치로 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_Move_PickUpPos_2ndStep:                            //  Transfer Z 축, Module Pick Up 위치로 이동 (2단계, 최종 위치)
                    Loader_Transfer_Step_Stacker0PickUp_TransferZ_Move_PickUpPos_2ndStep(out m_dSpeed, out m_dAccDec);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_Move_PickUpPos_2ndStep_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_Move_PickUpPos_2ndStep_DoneCheck:                       //  Transfer Z 축, Module Pick Up 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, Module Pickup 위치로 이동 완료");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker0PickUp_Transfer_PickerVacuum_On;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, Module Pickup 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        return AlarmPost(AlarmKey.LD_TransferZ_Move_PickUpPos_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, Module Pickup 위치로 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.Stacker0PickUp_Transfer_PickerVacuum_On:                            //  Transfer, Module Picker Vacuum On

                    Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer 축, Module Picker Vacuum On");

                    loaderParameter.DO_Loader_Picker_Vacuum((int)LoaderParameter.PickerVacuumPos.Inner, true);
                    loaderParameter.DO_Loader_Picker_Vacuum((int)LoaderParameter.PickerVacuumPos.Outer, true);
                    loaderParameter.DO_Loader_Picker_Blow(false);

                    m_nStacker0_StepUp_Count = 0;

                    TickCount_Start((int)TickType.TICK_LDTR);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker0PickUp_Transfer_PickerVacuum_OnCheck;
                    break;


                case (int)Loader_Transfer_Step.Stacker0PickUp_Transfer_PickerVacuum_OnCheck:                       //  Transfer, Module Picker Vacuum On 확인

                    if ((Equipment.Machine_VacuumSensor_Enable && (loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Inner) ||
                                                                loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Outer)) &&

                        (!Equipment.Machine_VacuumStableTime_Enable ||
                        (Equipment.Machine_VacuumStableTime_Enable && (TickCount_Elapsed((int)TickType.TICK_LDTR) > Equipment.Machine_VacuumStableTime)))) ||

                        (!Equipment.Machine_VacuumSensor_Enable && (TickCount_Elapsed((int)TickType.TICK_LDTR) > Equipment.Machine_SignalHoldTime)))
                    {
                        //  진공이 생성 되었으면?

                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer 축, Module Picker Vacuum On 완료");


                        //////////////////////////////////////////////////////////////////////////////////////////
                        //  복원 지점 체크용 (Stacker0 에서 Module Pick Up 완료) - Picker 공압이 형성되었으므로, Z 축을 올리면 자재가 들어올려짐
                        //
                        m_bLD_Transfer_fromStacker0_Module_PickUp_Complete_Flag = true;
                        //
                        //  복원 지점 체크용 (Stacker0 에서 Module Pick Up 완료)
                        //////////////////////////////////////////////////////////////////////////////////////////


                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_Move_ReadyPos2_1stStep;
                    }
                    else if (Equipment.Machine_LoaderStacker_LiftUp_Enable)
                    {
                        //  진공이 생성되지 않았으면? 1밀리 올리기

                        if (m_nStacker0_StepUp_Count++ < Equipment.Machine_LoaderStacker_LiftUpStep)
                        {
                            m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker0PickUp_Transfer_Picker_StepUp;
                        }
                        else
                        {
                            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer 축, Module Picker Vacuum On 실패. (Timeout)");

                            //  알람 정지 (LED Bar - Red Blink)
                            Equipment.MachineStop_byAlarm = true;

                            //timer_Motion_Home.Enabled = false;
                            //m_btimer_Motion_Home_Stop = true;

                            //////////////////////////////////////////////////////////////////////////////////////////
                            //  재시작 위치 저장용
                            //
                            Equipment.MachineStop_byTimeout_Loader = true;
                            Loader_CurrentStatus_Save_StopedByTimeout();
                            //
                            //  재시작 위치 저장용
                            //////////////////////////////////////////////////////////////////////////////////////////

                            return AlarmPost(AlarmKey.LD_Transfer_PickerVacuumOn_Timeout);
                            m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                            MessageBox.Show("Transfer 축, Module Picker Vacuum On 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (!Equipment.Machine_LoaderStacker_LiftUp_Enable && TickCount_Elapsed((int)TickType.TICK_LDTR) > 10000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer 축, Module Picker Vacuum On 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        //////////////////////////////////////////////////////////////////////////////////////////
                        //  재시작 위치 저장용
                        //
                        Equipment.MachineStop_byTimeout_Loader = true;
                        Loader_CurrentStatus_Save_StopedByTimeout();
                        //
                        //  재시작 위치 저장용
                        //////////////////////////////////////////////////////////////////////////////////////////

                        return AlarmPost(AlarmKey.LD_Transfer_PickerVacuumOn_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer 축, Module Picker Vacuum On 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }


                    //if ((Equipment.Machine_VacuumSensor_Enable && (loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Inner) ||
                    //                                                loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Outer)) &&

                    //    (!Equipment.Machine_VacuumStableTime_Enable || 
                    //    (Equipment.Machine_VacuumStableTime_Enable && (TickCount_Elapsed((int)TickType.TICK_LDTR) > Equipment.Machine_VacuumStableTime)))) ||

                    //    (!Equipment.Machine_VacuumSensor_Enable && (TickCount_Elapsed((int)TickType.TICK_LDTR) > Equipment.Machine_SignalHoldTime)))
                    //{
                    //    Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer 축, Module Picker Vacuum On 완료");


                    //    //////////////////////////////////////////////////////////////////////////////////////////
                    //    //  복원 지점 체크용 (Stacker0 에서 Module Pick Up 완료) - Picker 공압이 형성되었으므로, Z 축을 올리면 자재가 들어올려짐
                    //    //
                    //    m_bLD_Transfer_fromStacker0_Module_PickUp_Complete_Flag = true;
                    //    //
                    //    //  복원 지점 체크용 (Stacker0 에서 Module Pick Up 완료)
                    //    //////////////////////////////////////////////////////////////////////////////////////////



                    //    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_Move_ReadyPos2_1stStep;
                    //}
                    //else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 10000)
                    //{
                    //    Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer 축, Module Picker Vacuum On 실패. (Timeout)");

                    //    //  알람 정지 (LED Bar - Red Blink)
                    //    Equipment.MachineStop_byAlarm = true;

                    //    //timer_Motion_Home.Enabled = false;
                    //    //m_btimer_Motion_Home_Stop = true;

                    //    //////////////////////////////////////////////////////////////////////////////////////////
                    //    //  재시작 위치 저장용
                    //    //
                    //    Equipment.MachineStop_byTimeout_Loader = true;
                    //    Loader_CurrentStatus_Save_StopedByTimeout();
                    //    //
                    //    //  재시작 위치 저장용
                    //    //////////////////////////////////////////////////////////////////////////////////////////

                    //    return AlarmPost(AlarmKey.LD_Transfer_PickerVacuumOn_Timeout);
                    //    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                    //    MessageBox.Show("Transfer 축, Module Picker Vacuum On 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //}
                    break;


                case (int)Loader_Transfer_Step.Stacker0PickUp_Transfer_Picker_StepUp:                               //  Transfer, Module Picker Step Up

                    StackerZ0_StepUp(out m_dSpeed, out m_dAccDec);

                    TickCount_Start((int)TickType.TICK_LDTR);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker0PickUp_Transfer_Picker_StepUp_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.Stacker0PickUp_Transfer_Picker_StepUp_DoneCheck:                       //  Transfer, Module Picker Step Up 완료 확인

                    if ((TickCount_Elapsed((int)TickType.TICK_LDTR) > Equipment.Machine_LoaderStacker_LiftUp_StableTime) &&
                        MC_Func.MC_GetDone((int)nAxis.Z0) && MC_Func.MC_PosTolerance((int)nAxis.Z0, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Stacker Z0 축, Step Up 이동 완료");
                        
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker0PickUp_Transfer_PickerVacuum_OnCheck;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Stacker Z0 축, Step Up 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        //////////////////////////////////////////////////////////////////////////////////////////
                        ///LD Transfer Cycle", "Transfer Z 축, 대기 위치로 1단계 이동 실패. (Timeout)                        
                        //////////////////////////////////////////////////////////////////////////////////////////
                        ///

                        return AlarmPost(AlarmKey.LD_TransferZ_Move_ReadyPos_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, 대기 위치로 1단계 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_Move_ReadyPos2_1stStep:                            //  Transfer Z 축, 대기 위치로 이동 (1단계, 현재 위치에서 위로 10 mm)

                    Loader_Transfer_Step_Stacker0PickUp_TransferZ_Move_ReadyPos2_1stStep(out m_dSpeed, out m_dAccDec);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_Move_ReadyPos2_1stStep_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_Move_ReadyPos2_1stStep_DoneCheck:                       //  Transfer Z 축, 대기 위치로 이동 완료 확인 (1단계, 현재 위치에서 위로 10 mm)

                    //if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z]))
                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z]) &&
                        MC_Func.MC_GetDone((int)nAxis.Z0) && MC_Func.MC_PosTolerance((int)nAxis.Z0, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 1단계 이동 완료. (Z0 Stacker 하강 이동 완료.)");

                        if (Equipment.Machine_LoaderTransfer_Vibration_Enable)
                        {
                            //  모듈 털기 횟수 초기화
                            m_nLoader_Transfer_Vibration_Count = 0;

                            m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_Vibration_Start;
                        }
                        else
                        {
                            m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_Move_ReadyPos2_2ndStep;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 1단계 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;



                        //////////////////////////////////////////////////////////////////////////////////////////
                        /// 


                        return AlarmPost(AlarmKey.LD_TransferZ_Move_ReadyPos_Timeout);

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, 대기 위치로 1단계 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //  모듈 털기 - 시작
                case (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_Vibration_Start:                            //  Transfer Z 축, 모듈 털기 시작

                    if (m_nLoader_Transfer_Vibration_Count++ < Equipment.Machine_LoaderTransfer_NumberOfVibrations)
                    {
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_VibrationMove_DownPos;
                    }
                    else
                    {
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_Move_ReadyPos2_2ndStep;
                    }
                    break;


                case (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_VibrationMove_DownPos:                            //  Transfer Z 축, 아래로 2mm 이동

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z))
                    {
                        Loader_Transfer_Step_StackerPickUp_TransferZ_VibrationMove_DownPos(out m_dSpeed, out m_dAccDec);

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_VibrationMove_DownPos_DoneCheck;
                    }
                    break;


                case (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_VibrationMove_DownPos_DoneCheck:                       //  Transfer Z 축, 아래로 2mm 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 바이브레이션 이동 완료. (1단계, 현재 위치에서 2mm 아래)");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_VibrationMove_UpPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 바이브레이션 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        return AlarmPost(AlarmKey.LD_TransferZ_Move_VibrationPos_Timeout);

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, 대기 위치로 1단계 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_VibrationMove_UpPos:                            //  Transfer Z 축, 위로 2mm 이동

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z))
                    {
                        Loader_Transfer_Step_StackerPickUp_TransferZ_VibrationMove_UpPos(out m_dSpeed, out m_dAccDec);

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_VibrationMove_UpPos_DoneCheck;
                    }
                    break;


                case (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_VibrationMove_UpPos_DoneCheck:                       //  Transfer Z 축, 위로 2mm 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 바이브레이션 이동 완료. (1단계, 현재 위치에서 2mm 위)");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_Vibration_Start;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 바이브레이션 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        return AlarmPost(AlarmKey.LD_TransferZ_Move_VibrationPos_Timeout);

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, 대기 위치로 1단계 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                //  모듈 털기 - 종료
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


                case (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_Move_ReadyPos2_2ndStep:                            //  Transfer Z 축, 대기 위치로 이동 (2단계, 최종 위치)

                    Loader_Transfer_Step_Stacker0PickUp_TransferZ_Move_ReadyPos2_2ndStep(out m_dSpeed, out m_dAccDec);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck:                       //  Transfer Z 축, 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 2단계 이동 완료");

                        if (Equipment.Machine_VacuumSensor_Enable)
                        {
                            //  여기서 Module 을 정상적으로 들어 올렸는지 한번 더 체크
                            if (loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Inner) ||
                                loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Outer))
                            {
                                //  우측 Port 에서 Module PickUp 완료.
                                Equipment.AUTORUN_Loader_PickUpPort = (int)Equipment.LoaderPortList.R_Port;

                                m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Complete;
                            }
                            else
                            {
                                Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, Module Picker 공압이 형성되지 않음");


                                //////////////////////////////////////////////////////////////////////////////////////////
                                //  복원 지점 체크용 (Stacker0 에서 Module Pick Up 실패) - Picker 공압이 형성되지 않았음
                                //
                                m_bLD_Transfer_fromStacker0_Module_PickUp_Complete_Flag = false;
                                //
                                //  복원 지점 체크용 (Stacker0 에서 Module Pick Up 실패)
                                //////////////////////////////////////////////////////////////////////////////////////////


                                //  알람 정지 (LED Bar - Red Blink)
                                Equipment.MachineStop_byAlarm = true;

                                //////////////////////////////////////////////////////////////////////////////////////////
                                //  재시작 위치 저장용
                                //
                                Equipment.MachineStop_byTimeout_Loader = true;
                                Loader_CurrentStatus_Save_StopedByTimeout();
                                //
                                //  재시작 위치 저장용
                                //////////////////////////////////////////////////////////////////////////////////////////

                                return AlarmPost(AlarmKey.LD_Transfer_PickerVacuumOn_Timeout);
                                m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                                MessageBox.Show("Transfer Z 축, Module Pickup 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            //  우측 Port 에서 Module PickUp 완료.
                            Equipment.AUTORUN_Loader_PickUpPort = (int)Equipment.LoaderPortList.R_Port;

                            m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Complete;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 2단계 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        return AlarmPost(AlarmKey.LD_TransferZ_Move_ReadyPos_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, 대기 위치로 2단계 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                /// <summary>
                /// Stacker0 에서 Module Pick Up 일 경우 - 완료
                /// </summary>



                /// <summary>
                /// Stacker1 에서 Module Pick Up 일 경우 - 시작
                /// </summary>
                case (int)Loader_Transfer_Step.Stacker1_ModulePickup_Condition_Check:                            //  Stacker1 에서 Module Pick Up 조건 체크 (Stacker1 Module Exist Sensor On, Stacker1 Module Full Sensor On, Transfer Picker Vacuum Off Check, Stacker1 Cycle : None)

                    Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "TR 축, Stacker1 에서 Module Pickup 조건 체크");

                    if (!workStage.m_bMainWorkCycle_DryRun && !loaderParameter.DI_Loader_Stacker_MaterialCheck((int)LoaderParameter.StackerTable.Stacker_1))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Stacker1 에 Module 이 감지되지 않음.");

                        //  Out.

                        //////////////////////////////////////////////////////////////////////////////////////////
                        //  재시작 위치 저장용
                        //
                        Equipment.MachineStop_byTimeout_Loader = true;
                        Loader_CurrentStatus_Save_StopedByTimeout();
                        //
                        //  재시작 위치 저장용
                        //////////////////////////////////////////////////////////////////////////////////////////

                        return AlarmPost(AlarmKey.LD_Stacker1_ModulePickup_ConditionCheck_Stacker1ModuleNotExist);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;
                    }
                    else if (loaderParameter.DI_Loader_Stacker_FullCheck((int)LoaderParameter.StackerTable.Stacker_1))         //  감지 시 Off
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Stacker1 의 Full 감지 센서에 Module 이 감지되지 않음.");

                        //  Out.

                        //////////////////////////////////////////////////////////////////////////////////////////
                        //  재시작 위치 저장용
                        //
                        Equipment.MachineStop_byTimeout_Loader = true;
                        Loader_CurrentStatus_Save_StopedByTimeout();
                        //
                        //  재시작 위치 저장용
                        //////////////////////////////////////////////////////////////////////////////////////////

                        return AlarmPost(AlarmKey.LD_Stacker1_ModulePickup_ConditionCheck_Stacker1FullSensorNotExist);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;
                    }
                    else if (loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Inner) ||
                            loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Outer))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Picker 에 자재가 감지됨.");

                        //  Out.

                        //////////////////////////////////////////////////////////////////////////////////////////
                        //  재시작 위치 저장용
                        //
                        Equipment.MachineStop_byTimeout_Loader = true;
                        Loader_CurrentStatus_Save_StopedByTimeout();
                        //
                        //  재시작 위치 저장용
                        //////////////////////////////////////////////////////////////////////////////////////////

                        return AlarmPost(AlarmKey.LD_Transfer_PickerVacuumOn_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;
                    }
                    else if (m_nStacker1_ModulePickupWaitingPos_Step > (int)StackerModulePickupWaitingPos_Step.None)                                                       //  Transfer Cycle 이 동작중
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Stacker1 이 동작중이므로 Module Pickup 동작 중지.");

                        //  Out. (Stacker 동작이 완료되면 진행하도록 대기할 것인지는 테스트 하면서 결정하기로 함)

                        //////////////////////////////////////////////////////////////////////////////////////////
                        //  재시작 위치 저장용
                        //
                        Equipment.MachineStop_byTimeout_Loader = true;
                        Loader_CurrentStatus_Save_StopedByTimeout();
                        //
                        //  재시작 위치 저장용
                        //////////////////////////////////////////////////////////////////////////////////////////

                        return AlarmPost(AlarmKey.LD_Transfer_StackerIsWorking);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;
                    }
                    else
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Stacker1 Module Pickup 조건 OK");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_Move_ReadyPos;
                    }
                    break;


                case (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_Move_ReadyPos:                            //  Transfer Z 축, 대기 위치로 이동

                    Loader_Transfer_Step_Stacker1PickUp_TransferZ_Move_ReadyPos(out m_dSpeed, out m_dAccDec);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_Move_ReadyPos_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_Move_ReadyPos_DoneCheck:                       //  Transfer Z 축, 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 완료");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker1PickUp_TransferX_Move_StackerPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        //////////////////////////////////////////////////////////////////////////////////////////
                        ///Transfer Z 축, 대기 위치로 이동 실패. (Timeout)
                        //////////////////////////////////////////////////////////////////////////////////////////
                        
                        return AlarmPost(AlarmKey.LD_TransferZ_Move_ReadyPos_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, 대기 위치로 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.Stacker1PickUp_TransferX_Move_StackerPos:                            //  Transfer X 축, Stacker 위치로 이동

                    Loader_Transfer_Step_Stacker1PickUp_TransferX_Move_StackerPos(out m_dSpeed, out m_dAccDec);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker1PickUp_TransferX_Move_StackerPos_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.Stacker1PickUp_TransferX_Move_StackerPos_DoneCheck:                       //  Transfer X 축, Stacker 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_X) && MC_Func.MC_PosTolerance((int)nAxis.TR_X, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_X]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer X 축, Stacker1 위치로 이동 완료");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_Move_PickUpPos_1stStep;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer X 축, Stacker1 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        return AlarmPost(AlarmKey.LD_TransferX_Move_StackerPos_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer X 축, Stacker1 위치로 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_Move_PickUpPos_1stStep:                            //  Transfer Z 축, Module Pick Up 대기 위치로 이동 (1단계, 최종 위치에서 위로 10 mm)

                    Loader_Transfer_Step_Stacker1PickUp_TransferZ_Move_PickUpPos_1stStep(out m_dSpeed, out m_dAccDec);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_Move_PickUpPos_1stStep_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_Move_PickUpPos_1stStep_DoneCheck:                       //  Transfer Z 축, Module Pick Up 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, Module Pickup 대기 위치로 이동 완료");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_Move_PickUpPos_2ndStep;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, Module Pickup 대기 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        return AlarmPost(AlarmKey.LD_TransferZ_Move_ReadyPos_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, Module Pickup 대기 위치로 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_Move_PickUpPos_2ndStep:                            //  Transfer Z 축, Module Pick Up 위치로 이동 (2단계, 최종 위치)

                    Loader_Transfer_Step_Stacker1PickUp_TransferZ_Move_PickUpPos_2ndStep(out m_dSpeed, out m_dAccDec);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_Move_PickUpPos_2ndStep_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_Move_PickUpPos_2ndStep_DoneCheck:                       //  Transfer Z 축, Module Pick Up 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, Module Pickup 위치로 이동 완료");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker1PickUp_Transfer_PickerVacuum_On;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, Module Pickup 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        //////////////////////////////////////////////////////////////////////////////////////////
                        ///Transfer Z 축, Module Pickup 위치로 이동 실패. (Timeout)
                        /// 
                        //////////////////////////////////////////////////////////////////////////////////////////

                        return AlarmPost(AlarmKey.LD_TransferZ_Move_PickUpPos_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, Module Pickup 위치로 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.Stacker1PickUp_Transfer_PickerVacuum_On:                                     //  Transfer, Module Picker Vacuum On
                    Loader_Transfer_Step_Stacker1PickUp_Transfer_PickerVacuum_On();

                    m_nStacker1_StepUp_Count = 0;

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker1PickUp_Transfer_PickerVacuum_OnCheck;
                    break;


                case (int)Loader_Transfer_Step.Stacker1PickUp_Transfer_PickerVacuum_OnCheck:                                //  Transfer, Module Picker Vacuum On 확인

                    if ((Equipment.Machine_VacuumSensor_Enable && (loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Inner) ||
                                                                loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Outer)) &&

                        (!Equipment.Machine_VacuumStableTime_Enable ||
                        (Equipment.Machine_VacuumStableTime_Enable && (TickCount_Elapsed((int)TickType.TICK_LDTR) > Equipment.Machine_VacuumStableTime)))) ||

                        (!Equipment.Machine_VacuumSensor_Enable && (TickCount_Elapsed((int)TickType.TICK_LDTR) > Equipment.Machine_SignalHoldTime)))
                    {
                        //  진공이 생성 되었으면?

                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer 축, Module Picker Vacuum On 완료");


                        //////////////////////////////////////////////////////////////////////////////////////////
                        //  복원 지점 체크용 (Stacker1 에서 Module Pick Up 완료) - Picker 공압이 형성되었으므로, Z 축을 올리면 자재가 들어올려짐
                        //
                        m_bLD_Transfer_fromStacker1_Module_PickUp_Complete_Flag = true;
                        //
                        //  복원 지점 체크용 (Stacker0 에서 Module Pick Up 완료)
                        //////////////////////////////////////////////////////////////////////////////////////////


                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_Move_ReadyPos2_1stStep;
                    }
                    else if (Equipment.Machine_LoaderStacker_LiftUp_Enable)
                    {
                        //  진공이 생성되지 않았으면? 1밀리 올리기

                        if (m_nStacker1_StepUp_Count++ < Equipment.Machine_LoaderStacker_LiftUpStep)
                        {
                            m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker1PickUp_Transfer_Picker_StepUp;
                        }
                        else
                        {
                            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer 축, Module Picker Vacuum On 실패. (Timeout)");

                            //  알람 정지 (LED Bar - Red Blink)
                            Equipment.MachineStop_byAlarm = true;

                            //timer_Motion_Home.Enabled = false;
                            //m_btimer_Motion_Home_Stop = true;

                            //////////////////////////////////////////////////////////////////////////////////////////
                            //  재시작 위치 저장용
                            //
                            Equipment.MachineStop_byTimeout_Loader = true;
                            Loader_CurrentStatus_Save_StopedByTimeout();
                            //
                            //  재시작 위치 저장용
                            //////////////////////////////////////////////////////////////////////////////////////////

                            return AlarmPost(AlarmKey.LD_Transfer_PickerVacuumOn_Timeout);
                            m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                            MessageBox.Show("Transfer 축, Module Picker Vacuum On 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (!Equipment.Machine_LoaderStacker_LiftUp_Enable && TickCount_Elapsed((int)TickType.TICK_LDTR) > 10000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer 축, Module Picker Vacuum On 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        //////////////////////////////////////////////////////////////////////////////////////////
                        //  재시작 위치 저장용
                        //
                        Equipment.MachineStop_byTimeout_Loader = true;
                        Loader_CurrentStatus_Save_StopedByTimeout();
                        //
                        //  재시작 위치 저장용
                        //////////////////////////////////////////////////////////////////////////////////////////

                        return AlarmPost(AlarmKey.LD_Transfer_PickerVacuumOn_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer 축, Module Picker Vacuum On 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }


                    //if ((Equipment.Machine_VacuumSensor_Enable && (loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Inner) ||
                    //                                            loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Outer)) &&

                    //    (!Equipment.Machine_VacuumStableTime_Enable ||
                    //    (Equipment.Machine_VacuumStableTime_Enable && (TickCount_Elapsed((int)TickType.TICK_LDTR) > Equipment.Machine_VacuumStableTime)))) ||

                    //    (!Equipment.Machine_VacuumSensor_Enable && (TickCount_Elapsed((int)TickType.TICK_LDTR) > Equipment.Machine_SignalHoldTime)))
                    //{
                    //    Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer 축, Module Picker Vacuum On 완료");


                    //    //////////////////////////////////////////////////////////////////////////////////////////
                    //    //  복원 지점 체크용 (Stacker1 에서 Module Pick Up 완료) - Picker 공압이 형성되었으므로, Z 축을 올리면 자재가 들어올려짐
                    //    //
                    //    m_bLD_Transfer_fromStacker1_Module_PickUp_Complete_Flag = true;
                    //    //
                    //    //  복원 지점 체크용 (Stacker0 에서 Module Pick Up 완료)
                    //    //////////////////////////////////////////////////////////////////////////////////////////


                    //    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_Move_ReadyPos2_1stStep;
                    //}
                    //else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 10000)
                    //{
                    //    Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer 축, Module Picker Vacuum On 실패. (Timeout)");

                    //    //  알람 정지 (LED Bar - Red Blink)
                    //    Equipment.MachineStop_byAlarm = true;

                    //    //timer_Motion_Home.Enabled = false;
                    //    //m_btimer_Motion_Home_Stop = true;

                    //    //////////////////////////////////////////////////////////////////////////////////////////
                    //    //  재시작 위치 저장용
                    //    //
                    //    Equipment.MachineStop_byTimeout_Loader = true;
                    //    Loader_CurrentStatus_Save_StopedByTimeout();
                    //    //
                    //    //  재시작 위치 저장용
                    //    //////////////////////////////////////////////////////////////////////////////////////////

                    //    return AlarmPost(AlarmKey.LD_Transfer_PickerVacuumOn_Timeout);
                    //    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                    //    MessageBox.Show("Transfer 축, Module Picker Vacuum On 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //}
                    break;


                case (int)Loader_Transfer_Step.Stacker1PickUp_Transfer_Picker_StepUp:                               //  Transfer, Module Picker Step Up

                    StackerZ1_StepUp(out m_dSpeed, out m_dAccDec);

                    TickCount_Start((int)TickType.TICK_LDTR);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker1PickUp_Transfer_Picker_StepUp_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.Stacker1PickUp_Transfer_Picker_StepUp_DoneCheck:                       //  Transfer, Module Picker Step Up 완료 확인

                    if ((TickCount_Elapsed((int)TickType.TICK_LDTR) > Equipment.Machine_LoaderStacker_LiftUp_StableTime) &&
                        MC_Func.MC_GetDone((int)nAxis.Z1) && MC_Func.MC_PosTolerance((int)nAxis.Z1, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z1]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Stacker Z1 축, Step Up 이동 완료");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker1PickUp_Transfer_PickerVacuum_OnCheck;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Stacker Z1 축, Step Up 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        //////////////////////////////////////////////////////////////////////////////////////////
                        ///LD Transfer Cycle", "Transfer Z 축, 대기 위치로 1단계 이동 실패. (Timeout)                        
                        //////////////////////////////////////////////////////////////////////////////////////////
                        ///

                        return AlarmPost(AlarmKey.LD_TransferZ_Move_ReadyPos_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, 대기 위치로 1단계 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_Move_ReadyPos2_1stStep:                            //  Transfer Z 축, 대기 위치로 이동 (1단계, 현재 위치에서 위로 10 mm)

                    Loader_Transfer_Step_Stacker1PickUp_TransferZ_Move_ReadyPos2_1stStep(out m_dSpeed, out m_dAccDec);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_Move_ReadyPos2_1stStep_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_Move_ReadyPos2_1stStep_DoneCheck:                       //  Transfer Z 축, 대기 위치로 이동 완료 확인 (1단계, 현재 위치에서 위로 10 mm)

                    //if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z]))
                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z]) &&
                        MC_Func.MC_GetDone((int)nAxis.Z1) && MC_Func.MC_PosTolerance((int)nAxis.Z1, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z1]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 1단계 이동 완료. (Z1 Stacker 하강 이동 완료.)");

                        if (Equipment.Machine_LoaderTransfer_Vibration_Enable)
                        {
                            //  모듈 털기 횟수 초기화
                            m_nLoader_Transfer_Vibration_Count = 0;

                            m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_Vibration_Start;
                        }
                        else
                        {
                            m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_Move_ReadyPos2_2ndStep;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 1단계 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        //////////////////////////////////////////////////////////////////////////////////////////
                        ///LD Transfer Cycle", "Transfer Z 축, 대기 위치로 1단계 이동 실패. (Timeout)                        
                        //////////////////////////////////////////////////////////////////////////////////////////
                        ///

                        return AlarmPost(AlarmKey.LD_TransferZ_Move_ReadyPos_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, 대기 위치로 1단계 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //  모듈 털기 - 시작
                case (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_Vibration_Start:                            //  Transfer Z 축, 모듈 털기 시작

                    if (m_nLoader_Transfer_Vibration_Count++ < Equipment.Machine_LoaderTransfer_NumberOfVibrations)
                    {
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_VibrationMove_DownPos;
                    }
                    else
                    {
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_Move_ReadyPos2_2ndStep;
                    }
                    break;


                case (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_VibrationMove_DownPos:                            //  Transfer Z 축, 아래로 2mm 이동

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z))
                    {
                        Loader_Transfer_Step_StackerPickUp_TransferZ_VibrationMove_DownPos(out m_dSpeed, out m_dAccDec);

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_VibrationMove_DownPos_DoneCheck;
                    }
                    break;


                case (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_VibrationMove_DownPos_DoneCheck:                       //  Transfer Z 축, 아래로 2mm 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 바이브레이션 이동 완료. (1단계, 현재 위치에서 2mm 아래)");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_VibrationMove_UpPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 바이브레이션 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        return AlarmPost(AlarmKey.LD_TransferZ_Move_VibrationPos_Timeout);

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, 대기 위치로 1단계 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_VibrationMove_UpPos:                            //  Transfer Z 축, 위로 2mm 이동

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z))
                    {
                        Loader_Transfer_Step_StackerPickUp_TransferZ_VibrationMove_UpPos(out m_dSpeed, out m_dAccDec);

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_VibrationMove_UpPos_DoneCheck;
                    }
                    break;


                case (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_VibrationMove_UpPos_DoneCheck:                       //  Transfer Z 축, 위로 2mm 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 바이브레이션 이동 완료. (1단계, 현재 위치에서 2mm 위)");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_Vibration_Start;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 바이브레이션 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        return AlarmPost(AlarmKey.LD_TransferZ_Move_VibrationPos_Timeout);

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, 대기 위치로 1단계 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                //  모듈 털기 - 종료
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


                case (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_Move_ReadyPos2_2ndStep:                            //  Transfer Z 축, 대기 위치로 이동 (2단계, 최종 위치)

                    Loader_Transfer_Step_Stacker1PickUp_TransferZ_Move_ReadyPos2_2ndStep(out m_dSpeed, out m_dAccDec);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck:                       //  Transfer Z 축, 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 2단계 이동 완료");


                        if (Equipment.Machine_VacuumSensor_Enable)
                        {
                            //  여기서 Module 을 정상적으로 들어 올렸는지 한번 더 체크
                            if (loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Inner) ||
                                loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Outer))
                            {
                                //  좌측 Port 에서 Module PickUp 완료.
                                Equipment.AUTORUN_Loader_PickUpPort = (int)Equipment.LoaderPortList.L_Port;

                                m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Complete;
                            }
                            else
                            {
                                Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, Module Picker 공압이 형성되지 않음");


                                //////////////////////////////////////////////////////////////////////////////////////////
                                //  복원 지점 체크용 (Stacker1 에서 Module Pick Up 실패) - Picker 공압이 형성되지 않았음
                                //
                                m_bLD_Transfer_fromStacker1_Module_PickUp_Complete_Flag = false;
                                //
                                //  복원 지점 체크용 (Stacker0 에서 Module Pick Up 완료)
                                //////////////////////////////////////////////////////////////////////////////////////////


                                //  알람 정지 (LED Bar - Red Blink)
                                Equipment.MachineStop_byAlarm = true;

                                //////////////////////////////////////////////////////////////////////////////////////////
                                //  재시작 위치 저장용
                                //
                                Equipment.MachineStop_byTimeout_Loader = true;
                                Loader_CurrentStatus_Save_StopedByTimeout();
                                //
                                //  재시작 위치 저장용
                                //////////////////////////////////////////////////////////////////////////////////////////

                                return AlarmPost(AlarmKey.LD_Transfer_PickerVacuumOn_Timeout);
                                m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                                MessageBox.Show("Transfer Z 축, Module Pickup 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            //  좌측 Port 에서 Module PickUp 완료.
                            Equipment.AUTORUN_Loader_PickUpPort = (int)Equipment.LoaderPortList.L_Port;

                            m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Complete;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 2단계 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        return AlarmPost(AlarmKey.LD_TransferZ_Move_ReadyPos_Timeout);


                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, 대기 위치로 2단계 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                /// <summary>
                /// Stacker1 에서 Module Pick Up 일 경우 - 완료
                /// </summary>



                /// <summary>
                /// M-Aligner 에서 Module Pick Up 일 경우 - 시작
                /// </summary>
                case (int)Loader_Transfer_Step.MAligner_ModulePickup_Condition_Check:                            //  M-Aligner 에서 Module Pick Up 조건 체크 (M-Align 완료, M-Aligner Vacuum On Check, M-Align Cycle : None)

                    Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "TR 축, M-Aligner 에서 Module Pickup 조건 체크");

                    if (m_nMAlign_Step > (int)MAlign_Step.None)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "M-Aligner 동작중.");

                        //  Out.

                        //////////////////////////////////////////////////////////////////////////////////////////
                        //  재시작 위치 저장용
                        //
                        Equipment.MachineStop_byTimeout_Loader = true;
                        Loader_CurrentStatus_Save_StopedByTimeout();
                        //
                        //  재시작 위치 저장용
                        //////////////////////////////////////////////////////////////////////////////////////////

                        return AlarmPost(AlarmKey.LD_Transfer_MAlignerIsWorking);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;
                    }
                    else
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "M-Aligner 에서 Module Pickup 조건 OK");

                        //m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAligner_MAlign_Start;
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPickUp_TransferZ_Move_ReadyPos;
                    }
                    break;


                case (int)Loader_Transfer_Step.MAligner_MAlign_Start:                            //  M-Aligner 에서 Align 시작 (Align 이 완료되지 않은 상태일 경우 M-Align Cyc. Call)

                    //if (!m_bMAlign_Complete)
                    if (!m_bMAlign_Complete || m_bMAlign_Retry)
                    {
                        Loader_Transfer_Step_MAligner_MAlign_Start();

                        //  M-Align 시작
                        m_nMAlign_Step = (int)MAlign_Step.Start;

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAligner_MAlign_CompleteCheck;
                    }
                    else
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "M-Aligner, Align 이 완료된 상태이므로 Pickup 진행");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPickUp_TransferZ_Move_ReadyPos;
                    }
                    break;


                case (int)Loader_Transfer_Step.MAligner_MAlign_CompleteCheck:                       //  M-Aligner 에서 Align 완료 확인 (M-Aligner Vacuum 이 Off 이면 Pick Up 하지 않음 --> 자재가 없다고 판단)

                    if (MC_Func.MC_GetDone((int)nAxis.ALN_X) && MC_Func.MC_GetDone((int)nAxis.ALN_Y) && (m_nMAlign_Step == (int)MAlign_Step.None) && m_bMAlign_Complete)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "M-Aligner, Align 완료");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPickUp_TransferZ_Move_ReadyPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "M-Aligner, Align 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        //////////////////////////////////////////////////////////////////////////////////////////
                        ///M-Aligner, Align 실패. (Timeout)
                        ///
                        //////////////////////////////////////////////////////////////////////////////////////////  
                        ///

                        //  재시작 위치 저장용
                        
                        return AlarmPost(AlarmKey.LD_Transfer_MAligner_Align_Timeout);

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("M-Aligner, Align 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.MAlignerPickUp_TransferZ_Move_ReadyPos:                            //  Transfer Z 축, 대기 위치로 이동

                    Loader_Transfer_Step_MAlignerPickUp_TransferZ_Move_ReadyPos(out m_dSpeed, out m_dAccDec);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPickUp_TransferZ_Move_ReadyPos_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.MAlignerPickUp_TransferZ_Move_ReadyPos_DoneCheck:                       //  Transfer Z 축, 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 완료");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPickUp_TransferX_Move_MAlignerPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        return AlarmPost(AlarmKey.LD_TransferZ_Move_ReadyPos_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, 대기 위치로 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.MAlignerPickUp_TransferX_Move_MAlignerPos:                            //  Transfer X 축, M-Aligner 위치로 이동

                    Loader_Transfer_Step_MAlignerPickUp_TransferX_Move_MAlignerPos(out m_dSpeed, out m_dAccDec);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPickUp_TransferX_Move_MAlignerPos_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.MAlignerPickUp_TransferX_Move_MAlignerPos_DoneCheck:                       //  Transfer X 축, M-Aligner 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_X) && MC_Func.MC_PosTolerance((int)nAxis.TR_X, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_X]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer X 축, M-Aligner 위치로 이동 완료");

                        if (m_bMAlign_Retry)
                        {
                            m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPickUp_Retry_Start;
                        }
                        else
                        {
                            m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPickUp_TransferZ_Move_PickUpPos_1stStep;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer X 축, M-Aligner 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        return AlarmPost(AlarmKey.LD_TransferX_Move_MAlignerPos_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer X 축, M-Aligner 위치로 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ///
                //  Pick Up Retry - Start
                //
                case (int)Loader_Transfer_Step.MAlignerPickUp_Retry_Start:                            //  Retry Start

                    Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Pick Up Retry Start");

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPickUp_Retry_Transfer_PickerVacuum_Off;
                    break;


                case (int)Loader_Transfer_Step.MAlignerPickUp_Retry_Transfer_PickerVacuum_Off:                                     //  Transfer, Module Picker Vacuum Off

                    Loader_Transfer_Step_MAlignerPickUp_Retry_Transfer_PickerVacuum_Off();

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPickUp_Retry_Transfer_PickerVacuum_OffCheck;
                    break;


                case (int)Loader_Transfer_Step.MAlignerPickUp_Retry_Transfer_PickerVacuum_OffCheck:                                //  Transfer, Module Picker Vacuum Off 확인

                    if (!loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Inner) &&
                        !loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Outer))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer 축, Retry, Module Picker Vacuum Off 완료");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPickUp_Retry_TransferZ_Move_PickUpPos_1stStep;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 10000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer 축, Retry, Module Picker Vacuum Off실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;


                        //////////////////////////////////////////////////////////////////////////////////////////
                        //  재시작 위치 저장용
                        //
                        Equipment.MachineStop_byTimeout_Loader = true;
                        Loader_CurrentStatus_Save_StopedByTimeout();
                        //
                        //  재시작 위치 저장용
                        //////////////////////////////////////////////////////////////////////////////////////////

                        return AlarmPost(AlarmKey.LD_Transfer_PickerVacuumOff_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer 축, Module Picker Vacuum Off 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.MAlignerPickUp_Retry_TransferZ_Move_PickUpPos_1stStep:                            //  Transfer Z 축, Module Pick Up 위치로 이동 (1단계, 최종 위치에서 위로 10 mm)

                    Loader_Transfer_Step_MAlignerPickUp_Retry_TransferZ_Move_PickUpPos_1stStep(out m_dSpeed, out m_dAccDec);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPickUp_Retry_TransferZ_Move_PickUpPos_1stStep_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.MAlignerPickUp_Retry_TransferZ_Move_PickUpPos_1stStep_DoneCheck:                       //  Transfer Z 축, Module Pick Up 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, Retry, Module Press 대기 위치로 이동 완료");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPickUp_Retry_TransferZ_Move_PickUpPos_2ndStep;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, Retry, Module Press 대기 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        return AlarmPost(AlarmKey.LD_TransferZ_Move_PressPos_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, Retry, Module Press 대기 위치로 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.MAlignerPickUp_Retry_TransferZ_Move_PickUpPos_2ndStep:                            //  Transfer Z 축, Module Pick Up 위치로 이동 (2단계, 최종 위치)

                    Loader_Transfer_Step_MAlignerPickUp_Retry_TransferZ_Move_PickUpPos_2ndStep(out m_dSpeed, out m_dAccDec);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPickUp_Retry_TransferZ_Move_PickUpPos_2ndStep_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.MAlignerPickUp_Retry_TransferZ_Move_PickUpPos_2ndStep_DoneCheck:                       //  Transfer Z 축, Module Pick Up 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, Retry Module Press 위치로 이동 완료");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPickUp_Retry_MAligner_Vacuum_On;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, Retry, Module Press 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        //////////////////////////////////////////////////////////////////////////////////////////
                        ///

                        return AlarmPost(AlarmKey.LD_TransferZ_Move_PressPos_Timeout);

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, Retry, Module Press 위치로 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.MAlignerPickUp_Retry_MAligner_Vacuum_On:                                     //  M-Aligner, Vacuum On

                    Loader_Transfer_Step_MAlignerPickUp_Retry_MAligner_Vacuum_On();

                    TickCount_Start((int)TickType.TICK_LDTR);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPickUp_Retry_MAligner_Vacuum_OnCheck;
                    break;


                case (int)Loader_Transfer_Step.MAlignerPickUp_Retry_MAligner_Vacuum_OnCheck:                            //  Transfer Picker Vacuum On Check, M-Aligner Vacuum Off Check

                    if ((Equipment.Machine_VacuumSensor_Enable &&

                            ((Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Center && loaderParameter.DI_Loader_Aligner_VacuumCheck((int)LoaderParameter.MAlignerVacuumPos.Center)) ||
                            !Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Center) &&

                            ((Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Inner && loaderParameter.DI_Loader_Aligner_VacuumCheck((int)LoaderParameter.MAlignerVacuumPos.Inner)) ||
                            !Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Inner) &&

                            ((Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Outer && loaderParameter.DI_Loader_Aligner_VacuumCheck((int)LoaderParameter.MAlignerVacuumPos.Outer)) ||
                            !Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Outer)) ||

                        (!Equipment.Machine_VacuumSensor_Enable && (TickCount_Elapsed((int)TickType.TICK_LDTR) > Equipment.Machine_SignalHoldTime)))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer 축, Retry, M-Aligner Vacuum On 완료");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPickUp_Retry_TransferZ_Move_ReadyPos2_1stStep;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer 축, Retry,, M-Aligner Vacuum On 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;


                        //////////////////////////////////////////////////////////////////////////////////////////
                        //  복원 지점 체크용 (M-Aligner 에 Module Put Down 실패) - M-Aligner 에 공압이 형성되지 않았거나, Picker 공압이 파기되지 않았음.
                        //
                        m_bLD_Transfer_toMAligner_Module_PutDown_Complete_Flag = false;
                        //
                        //  복원 지점 체크용 (M-Aligner 에 Module Put Down 실패)
                        //////////////////////////////////////////////////////////////////////////////////////////


                        //////////////////////////////////////////////////////////////////////////////////////////
                        //  재시작 위치 저장용
                        //
                        Equipment.MachineStop_byTimeout_Loader = true;
                        Loader_CurrentStatus_Save_StopedByTimeout();
                        //
                        //  재시작 위치 저장용
                        //////////////////////////////////////////////////////////////////////////////////////////
                        ///

                        return AlarmPost(AlarmKey.LD_Transfer_MAlignerVacuumOn_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer 축, Retry, M-Aligner Vacuum On 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.MAlignerPickUp_Retry_TransferZ_Move_ReadyPos2_1stStep:                            //  Transfer Z 축, 대기 위치로 이동 (1단계, 현재 위치에서 Aligner Pusher 를 벗어나는 높이까지)

                    Loader_Transfer_Step_MAlignerPickUp_Retry_TransferZ_Move_ReadyPos2_1stStep(out m_dSpeed, out m_dAccDec);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPickUp_Retry_TransferZ_Move_ReadyPos2_1stStep_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.MAlignerPickUp_Retry_TransferZ_Move_ReadyPos2_1stStep_DoneCheck:                       //  Transfer Z 축, 대기 위치로 이동 완료 확인 (여기서 Transfer Picker 의 Vacuum On 과 Aligner 의 Vacuum Off 를 동시에 확인)

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, Retry, 대기 위치로 1단계 이동 완료");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPickUp_Retry_TransferZ_Move_ReadyPos2_2ndStep;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, Retry, 대기 위치로 1단계 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        return AlarmPost(AlarmKey.LD_TransferZ_Move_ReadyPos_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, Retry, 대기 위치로 1단계 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.MAlignerPickUp_Retry_TransferZ_Move_ReadyPos2_2ndStep:                            //  Transfer Z 축, 대기 위치로 이동 (2단계, 최종 위치)

                    Loader_Transfer_Step_MAlignerPickUp_Retry_TransferZ_Move_ReadyPos2_2ndStep(out m_dSpeed, out m_dAccDec);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPickUp_Retry_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.MAlignerPickUp_Retry_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck:                       //  Transfer Z 축, 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, Retry, 대기 위치로 2단계 이동 완료");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAligner_Retry_MAlign_Start;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, Retry, 대기 위치로 2단계 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        return AlarmPost(AlarmKey.LD_TransferZ_Move_ReadyPos_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, Retry, 대기 위치로 2단계 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.MAligner_Retry_MAlign_Start:                            //  M-Aligner 에서 Align 시작 (Align 이 완료되지 않은 상태일 경우 M-Align Cyc. Call)

                    Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "M-Aligner, Retry, Align 시작");

                    TickCount_Start((int)TickType.TICK_LDTR);

                    //  M-Align 시작
                    m_nMAlign_Step = (int)MAlign_Step.Start;

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAligner_Retry_MAlign_CompleteCheck;

                    break;


                case (int)Loader_Transfer_Step.MAligner_Retry_MAlign_CompleteCheck:                       //  M-Aligner 에서 Align 완료 확인 (M-Aligner Vacuum 이 Off 이면 Pick Up 하지 않음 --> 자재가 없다고 판단)

                    if (MC_Func.MC_GetDone((int)nAxis.ALN_X) && MC_Func.MC_GetDone((int)nAxis.ALN_Y) && (m_nMAlign_Step == (int)MAlign_Step.None) && m_bMAlign_Complete)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "M-Aligner, Retry, Align 완료");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPickUp_Retry_Complete;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "M-Aligner, Retry, Align 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        //////////////////////////////////////////////////////////////////////////////////////////
                        /// 

                        //  복원 지점 체크용 (M-Aligner 에 Module Put Down 실패) - M-Aligner 에 공압이 형성되지 않았거나, Picker 공압이 파기되지 않았음.
                        //

                        return AlarmPost(AlarmKey.LD_Transfer_MAligner_Align_Timeout);

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("M-Aligner, Retry, Align 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.MAlignerPickUp_Retry_Complete:                            //  Retry Complete

                    Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Pick Up Retry Complete");

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPickUp_TransferZ_Move_PickUpPos_1stStep;
                    break;


                ///
                //  Pick Up Retry - Complete
                //
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


                case (int)Loader_Transfer_Step.MAlignerPickUp_TransferZ_Move_PickUpPos_1stStep:                            //  Transfer Z 축, Module Pick Up 위치로 이동 (1단계, 최종 위치에서 위로 10 mm)

                    Loader_Transfer_Step_MAlignerPickUp_TransferZ_Move_PickUpPos_1stStep(out m_dSpeed, out m_dAccDec);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPickUp_TransferZ_Move_PickUpPos_1stStep_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.MAlignerPickUp_TransferZ_Move_PickUpPos_1stStep_DoneCheck:                       //  Transfer Z 축, Module Pick Up 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, Module Pickup 대기 위치로 이동 완료");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPickUp_TransferZ_Move_PickUpPos_2ndStep;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, Module Pickup 대기 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        return AlarmPost(AlarmKey.LD_TransferZ_Move_PickUpPos_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, Module Pickup 대기 위치로 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.MAlignerPickUp_TransferZ_Move_PickUpPos_2ndStep:                            //  Transfer Z 축, Module Pick Up 위치로 이동 (2단계, 최종 위치)

                    Loader_Transfer_Step_MAlignerPickUp_TransferZ_Move_PickUpPos_2ndStep(out m_dSpeed, out m_dAccDec);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPickUp_TransferZ_Move_PickUpPos_2ndStep_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.MAlignerPickUp_TransferZ_Move_PickUpPos_2ndStep_DoneCheck:                       //  Transfer Z 축, Module Pick Up 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, Module Pickup 위치로 이동 완료");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPickUp_Transfer_PickerVacuum_On;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, Module Pickup 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        return AlarmPost(AlarmKey.LD_TransferZ_Move_PickUpPos_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, Module Pickup 위치로 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.MAlignerPickUp_Transfer_PickerVacuum_On:                                     //  Transfer, Module Picker Vacuum On

                    Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer 축, Module Picker Vacuum On");

                    loaderParameter.DO_Loader_Picker_Vacuum((int)LoaderParameter.PickerVacuumPos.Inner, true);
                    loaderParameter.DO_Loader_Picker_Vacuum((int)LoaderParameter.PickerVacuumPos.Outer, true);
                    loaderParameter.DO_Loader_Picker_Blow(false);

                    TickCount_Start((int)TickType.TICK_LDTR);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPickUp_Transfer_PickerVacuum_OnCheck;
                    break;


                case (int)Loader_Transfer_Step.MAlignerPickUp_Transfer_PickerVacuum_OnCheck:                                //  Transfer, Module Picker Vacuum On 확인

                    if ((Equipment.Machine_VacuumSensor_Enable && (loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Inner) ||
                                                                loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Outer)) &&

                        (!Equipment.Machine_VacuumStableTime_Enable ||
                        (Equipment.Machine_VacuumStableTime_Enable && (TickCount_Elapsed((int)TickType.TICK_LDTR) > Equipment.Machine_VacuumStableTime)))) ||

                        (!Equipment.Machine_VacuumSensor_Enable && (TickCount_Elapsed((int)TickType.TICK_LDTR) > Equipment.Machine_SignalHoldTime)))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer 축, Module Picker Vacuum On 완료");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPickUp_MAligner_Vacuum_Off;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 10000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer 축, Module Picker Vacuum On 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;


                        //////////////////////////////////////////////////////////////////////////////////////////
                        //  재시작 위치 저장용
                        //
                        Equipment.MachineStop_byTimeout_Loader = true;
                        Loader_CurrentStatus_Save_StopedByTimeout();
                        //
                        //  재시작 위치 저장용
                        //////////////////////////////////////////////////////////////////////////////////////////

                        return AlarmPost(AlarmKey.LD_Transfer_PickerVacuumOn_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer 축, Module Picker Vacuum On 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.MAlignerPickUp_MAligner_Vacuum_Off:                                     //  M-Aligner, Vacuum Off

                    Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "M-Aligner, Module Vacuum Off");

                    loaderParameter.DO_Loader_Aligner_Vacuum((int)LoaderParameter.MAlignerVacuumPos.Center, false);
                    loaderParameter.DO_Loader_Aligner_Vacuum((int)LoaderParameter.MAlignerVacuumPos.Inner, false);
                    loaderParameter.DO_Loader_Aligner_Vacuum((int)LoaderParameter.MAlignerVacuumPos.Outer, false);
                    loaderParameter.DO_Loader_Aligner_Blow((int)LoaderParameter.MAlignerVacuumPos.Center, true);
                    loaderParameter.DO_Loader_Aligner_Blow((int)LoaderParameter.MAlignerVacuumPos.Inner, true);
                    loaderParameter.DO_Loader_Aligner_Blow((int)LoaderParameter.MAlignerVacuumPos.Outer, true);


                    //////////////////////////////////////////////////////////////////////////////////////////
                    //  복원 지점 체크용 (M-Aligner 에서 Module Pick Up 완료) - Picker 공압이 형성되었으므로, Z 축을 올리면 자재가 들어올려짐
                    //
                    m_bLD_Transfer_fromMAligner_Module_PickUp_Complete_Flag = true;
                    //
                    //  복원 지점 체크용 (M-Aligner 에서 Module Pick Up 완료)
                    //////////////////////////////////////////////////////////////////////////////////////////


                    TickCount_Start((int)TickType.TICK_LDTR);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPickUp_MAligner_Vacuum_OffCheck;
                    break;


                case (int)Loader_Transfer_Step.MAlignerPickUp_MAligner_Vacuum_OffCheck:                                //   M-Aligner, Vacuum Off 확인

                    if (!loaderParameter.DI_Loader_Aligner_VacuumCheck((int)LoaderParameter.MAlignerVacuumPos.Center) &&
                        !loaderParameter.DI_Loader_Aligner_VacuumCheck((int)LoaderParameter.MAlignerVacuumPos.Inner) &&
                        !loaderParameter.DI_Loader_Aligner_VacuumCheck((int)LoaderParameter.MAlignerVacuumPos.Outer) &&

                        (!Equipment.Machine_VacuumBlowTime_Enable ||
                        (Equipment.Machine_VacuumBlowTime_Enable && (TickCount_Elapsed((int)TickType.TICK_LDTR) > Equipment.Machine_VacuumBlowTime))) )
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "M-Aligner, Module Vacuum Off 완료");

                        //loaderParameter.DO_Loader_Aligner_Blow((int)LoaderParameter.MAlignerVacuumPos.Center, false);
                        //loaderParameter.DO_Loader_Aligner_Blow((int)LoaderParameter.MAlignerVacuumPos.Inner, false);
                        //loaderParameter.DO_Loader_Aligner_Blow((int)LoaderParameter.MAlignerVacuumPos.Outer, false);

                        //  M-Aligner 를 넓히고 Picker 를 올릴 것인지,
                        //  Picker 를 올린 후에 M-Aligner 를 넓힐 것인지 옵션에 따라...

                        if (Equipment.Machine_MAligner_ReleaseType)         //  넓히고 올리기
                        {
                            m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPickUp_MAlignerXY_MoveType1_Widely;                 //  Type #1 일 경우 (1mm 정도 Aligner 를 열어줌)
                        }
                        else                                                //  올리고 넓히기
                        {
                            m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPickUp_TransferZ_Move_ReadyPos2_1stStep;            //  Type #2 일 경우 (Picker 를 올린 후 Aligner 를 열어줌)
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 10000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "M-Aligner, Module Vacuum Off 실패. (Timeout)");

                        loaderParameter.DO_Loader_Aligner_Blow((int)LoaderParameter.MAlignerVacuumPos.Center, false);
                        loaderParameter.DO_Loader_Aligner_Blow((int)LoaderParameter.MAlignerVacuumPos.Inner, false);
                        loaderParameter.DO_Loader_Aligner_Blow((int)LoaderParameter.MAlignerVacuumPos.Outer, false);

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        return AlarmPost(AlarmKey.LD_Transfer_MAlignerVacuumOff_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer 축, M-Aligner, Module Vacuum Off 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.MAlignerPickUp_MAlignerXY_MoveType1_Widely:                            //  M-Aligner XY 축, Module 을 들어올리기 위해 열어주는 위치로 이동 (1mm 정도) - Type #1 or #2 둘 중에 하나만 사용

                    Loader_Transfer_Step_MAlignerPickUp_MAlignerXY_MoveType1_Widely(out m_dSpeed, out m_dAccDec);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPickUp_MAlignerXY_MoveType1_Widely_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.MAlignerPickUp_MAlignerXY_MoveType1_Widely_DoneCheck:                       //  Transfer Z 축, 대기 위치로 이동 완료 확인 (1단계, 현재 위치에서 위로 10 mm)

                    if (MC_Func.MC_GetDone((int)nAxis.ALN_X) && MC_Func.MC_PosTolerance((int)nAxis.ALN_X, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_X]) &&
                        MC_Func.MC_GetDone((int)nAxis.ALN_Y) && MC_Func.MC_PosTolerance((int)nAxis.ALN_Y, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_Y]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "M-Aligner XY 축, 10mm 넓히기 이동 완료");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPickUp_TransferZ_Move_ReadyPos2_1stStep;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "M-Aligner XY 축, 10mm 넓히기 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        return AlarmPost(AlarmKey.LD_Transfer_MAlignerXY_Move_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("M-Aligner XY 축, 10mm 넓히기 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.MAlignerPickUp_TransferZ_Move_ReadyPos2_1stStep:                            //  Transfer Z 축, 대기 위치로 이동 (1단계, 현재 위치에서 Aligner Pusher 를 벗어나는 높이까지)

                    Loader_Transfer_Step_MAlignerPickUp_TransferZ_Move_ReadyPos2_1stStep(out m_dSpeed, out m_dAccDec);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPickUp_TransferZ_Move_ReadyPos2_1stStep_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.MAlignerPickUp_TransferZ_Move_ReadyPos2_1stStep_DoneCheck:                       //  Transfer Z 축, 대기 위치로 이동 완료 확인 (여기서 Transfer Picker 의 Vacuum On 과 Aligner 의 Vacuum Off 를 동시에 확인)

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 1단계 이동 완료");

                        //  M-Aligner 를 넓히고 Picker 를 올릴 것인지,
                        //  Picker 를 올린 후에 M-Aligner 를 넓힐 것인지 옵션에 따라...

                        loaderParameter.DO_Loader_Aligner_Blow((int)LoaderParameter.MAlignerVacuumPos.Center, false);
                        loaderParameter.DO_Loader_Aligner_Blow((int)LoaderParameter.MAlignerVacuumPos.Inner, false);
                        loaderParameter.DO_Loader_Aligner_Blow((int)LoaderParameter.MAlignerVacuumPos.Outer, false);

                        if (Equipment.Machine_MAligner_ReleaseType)         //  넓히고 올리기
                        {
                            m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPickUp_TransferZ_Move_ReadyPos2_2ndStep;            //  Type #1 일 경우 (앞에서 열어줬으니 Z축 올리기 2단계 진행)
                        }
                        else                                                //  올리고 넓히기
                        {
                            m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPickUp_MAlignerXY_MoveType2_Widely;            //  Type #2 일 경우 (Picker 를 올렸으므로 지금 Aligner 를 열어줌)
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 1단계 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        return AlarmPost(AlarmKey.LD_TransferZ_Move_ReadyPos_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, 대기 위치로 1단계 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.MAlignerPickUp_MAlignerXY_MoveType2_Widely:                            //  M-Aligner XY 축, Module 을 들어올린 후 열어주는 위치로 이동 (1mm 정도) - Type #2 or #1 둘 중에 하나만 사용

                    Loader_Transfer_Step_MAlignerPickUp_MAlignerXY_MoveType2_Widely(out m_dSpeed, out m_dAccDec);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPickUp_MAlignerXY_MoveType2_Widely_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.MAlignerPickUp_MAlignerXY_MoveType2_Widely_DoneCheck:                       //  M-Aligner XY 축, Module 을 들어올린 후 열어주는 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.ALN_X) && MC_Func.MC_PosTolerance((int)nAxis.ALN_X, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_X]) &&
                        MC_Func.MC_GetDone((int)nAxis.ALN_Y) && MC_Func.MC_PosTolerance((int)nAxis.ALN_Y, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_Y]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "M-Aligner XY 축, 5mm 넓히기 이동 완료");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPickUp_TransferZ_Move_ReadyPos2_2ndStep;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "M-Aligner XY 축, 1mm 넓히기 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        return AlarmPost(AlarmKey.LD_Transfer_MAlignerXY_Move_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("M-Aligner XY 축, 1mm 넓히기 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.MAlignerPickUp_TransferZ_Move_ReadyPos2_2ndStep:                            //  Transfer Z 축, 대기 위치로 이동 (2단계, 최종 위치)

                    Loader_Transfer_Step_MAlignerPickUp_TransferZ_Move_ReadyPos2_2ndStep(out m_dSpeed, out m_dAccDec);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPickUp_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.MAlignerPickUp_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck:                       //  Transfer Z 축, 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 2단계 이동 완료");

                        m_bMAlign_Complete = false;                                             //  M-Aligner 에서 자재를 가져갔으므로 false 로 변경
                        m_bMAlign_Retry = false;

                        if (Equipment.Machine_VacuumSensor_Enable)
                        {
                            //  여기서 모듈을 정상적으로 Pick Up 했는지 확인
                            if (loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Inner) ||
                                loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Outer))
                            {
                                Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "M-Aligner 에서 Module Pick Up 완료.");

                                m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Complete;
                            }
                            else
                            {
                                Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "M-Aligner 에서 Module Pick Up 실패.");


                                //////////////////////////////////////////////////////////////////////////////////////////
                                //  복원 지점 체크용 (M-Aligner 에서 Module Pick Up 실패) - Picker 공압이 형성되지 않음.
                                //
                                m_bLD_Transfer_fromMAligner_Module_PickUp_Complete_Flag = false;
                                //
                                //  복원 지점 체크용 (M-Aligner 에서 Module Pick Up 실패)
                                //////////////////////////////////////////////////////////////////////////////////////////


                                //  알람 정지 (LED Bar - Red Blink)
                                Equipment.MachineStop_byAlarm = true;


                                //////////////////////////////////////////////////////////////////////////////////////////
                                //  재시작 위치 저장용
                                //
                                Equipment.MachineStop_byTimeout_Loader = true;
                                Loader_CurrentStatus_Save_StopedByTimeout();
                                //
                                //  재시작 위치 저장용
                                //////////////////////////////////////////////////////////////////////////////////////////

                                return AlarmPost(AlarmKey.LD_Transfer_MAlignerPickUp_Timeout);
                                m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                                MessageBox.Show("M-Aligner 에서 Module Pick Up 실패.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Complete;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 2단계 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        return AlarmPost(AlarmKey.LD_TransferZ_Move_ReadyPos_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, 대기 위치로 2단계 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                /// <summary>
                /// M-Aligner 에서 Module Pick Up 일 경우 - 완료
                /// </summary>



                /// <summary>
                /// Module 을 WorkStage 에 Put-Down 일 경우 - 시작
                /// </summary>
                case (int)Loader_Transfer_Step.WorkStage_ModulePutdown_Condition_Check:                            //  Work Stage 에 Module Put Down 조건 체크 (Transfer Picker Vacuum On Check, Stage Vacuum Off Check, Drilling Cycle : None)

                    Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "TR 축, Work Stage 에 Module Put Down 조건 체크");

                    if (!workStage.m_bMainWorkCycle_DryRun && Equipment.Machine_VacuumSensor_Enable && 
                        !loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Inner) && !loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Outer))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Picker 에 Module 이 감지되지 않음.");

                        //  Out.

                        //////////////////////////////////////////////////////////////////////////////////////////
                        //  재시작 위치 저장용
                        //
                        Equipment.MachineStop_byTimeout_Loader = true;
                        Loader_CurrentStatus_Save_StopedByTimeout();
                        //
                        //  재시작 위치 저장용
                        //////////////////////////////////////////////////////////////////////////////////////////

                        return AlarmPost(AlarmKey.LD_Transfer_PickerVacuumOn_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;
                    }
                    else if (workStage.workStageParameter.DI_Stage_Vacuum_Check())
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Work Stage 에 Module 이 감지됨. (Vacuum Sensor : On)");

                        //  Out.

                        //////////////////////////////////////////////////////////////////////////////////////////
                        //  재시작 위치 저장용
                        //
                        Equipment.MachineStop_byTimeout_Loader = true;
                        Loader_CurrentStatus_Save_StopedByTimeout();
                        //
                        //  재시작 위치 저장용
                        //////////////////////////////////////////////////////////////////////////////////////////

                        return AlarmPost(AlarmKey.LD_Transfer_WorkStageVacuumOn_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;
                    }
                    else if (workStage.m_nWorkStage_Move_Step > (int)WorkStage.WorkStage_Move_Step.None)                                                       //  Transfer Cycle 이 동작중
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Work Stage 가 이동 중.");

                        //  Out.

                        //////////////////////////////////////////////////////////////////////////////////////////
                        //  재시작 위치 저장용
                        //
                        Equipment.MachineStop_byTimeout_Loader = true;
                        Loader_CurrentStatus_Save_StopedByTimeout();
                        //
                        //  재시작 위치 저장용
                        //////////////////////////////////////////////////////////////////////////////////////////
                        ///

                        return AlarmPost(AlarmKey.LD_Transfer_WorkStage_Is_Working);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;
                    }
                    else if (workStage.m_nLaserDrilling_MainStep > (int)WorkStage.LaserDrilling_Step.None)                                                     //  Laser Drilling Cycle 이 동작중
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Laser Drilling Cycle 동작 중.");

                        //  Out.

                        //////////////////////////////////////////////////////////////////////////////////////////
                        //  재시작 위치 저장용
                        //
                        Equipment.MachineStop_byTimeout_Loader = true;
                        Loader_CurrentStatus_Save_StopedByTimeout();
                        //
                        //  재시작 위치 저장용
                        //////////////////////////////////////////////////////////////////////////////////////////
                        ///

                        return AlarmPost(AlarmKey.LD_Transfer_LaserDrilling_Is_Working);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;
                    }
                    else
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "TR 축, Work Stage 에 Module Put Down 조건 OK");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.WorkStagePutDown_TransferZ_Move_ReadyPos;
                    }
                    break;


                case (int)Loader_Transfer_Step.WorkStagePutDown_TransferZ_Move_ReadyPos:                            //  Transfer Z 축, 대기 위치로 이동

                    Loader_Transfer_Step_WorkStagePutDown_TransferZ_Move_ReadyPos(out m_dSpeed, out m_dAccDec);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.WorkStagePutDown_TransferZ_Move_ReadyPos_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.WorkStagePutDown_TransferZ_Move_ReadyPos_DoneCheck:                       //  Transfer Z 축, 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 완료");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.WorkStagePutDown_WorkStageCycle_LoadingPos_Start;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        return AlarmPost(AlarmKey.LD_TransferZ_Move_ReadyPos_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, 대기 위치로 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.WorkStagePutDown_WorkStageCycle_LoadingPos_Start:                            //  Work Stage, Loading 위치로 이동 Cycle 시작

                    //if (!workStage.m_bWorkStageMove_Complete)
                    if ((workStage.m_nWorkStagePosition == (int)WorkStage.WorkStagePosition.WorkStage_LoadingZone) &&
                            (workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X) > (workStage.stWorkStageTeachingPos[(int)WorkStage.WorkStage_TeachingPosList.STAGE_LoadingPos].Stage_X - 0.1)) &&
                            (workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X) < (workStage.stWorkStageTeachingPos[(int)WorkStage.WorkStage_TeachingPosList.STAGE_LoadingPos].Stage_X + 0.1)) &&
                            (workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y) > (workStage.stWorkStageTeachingPos[(int)WorkStage.WorkStage_TeachingPosList.STAGE_LoadingPos].Stage_Y - 0.1)) &&
                            (workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y) < (workStage.stWorkStageTeachingPos[(int)WorkStage.WorkStage_TeachingPosList.STAGE_LoadingPos].Stage_Y + 0.1)))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Work Stage, Module Loading 위치로 이동이 완료된 상태이므로 PutDown 진행");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.WorkStagePutDown_TransferX_Move_LoadingPos;
                    }
                    else if ((workStage.m_nWorkStage_Move_Step == (int)WorkStage.WorkStage_Move_Step.None) &&
                            (workStage.m_nLaserDrilling_MainStep == (int)WorkStage.LaserDrilling_Step.None) &&
                            (workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X) > (workStage.stWorkStageTeachingPos[(int)WorkStage.WorkStage_TeachingPosList.STAGE_LoadingPos].Stage_X - 0.1)) &&
                            (workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.X) < (workStage.stWorkStageTeachingPos[(int)WorkStage.WorkStage_TeachingPosList.STAGE_LoadingPos].Stage_X + 0.1)) &&
                            (workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y) > (workStage.stWorkStageTeachingPos[(int)WorkStage.WorkStage_TeachingPosList.STAGE_LoadingPos].Stage_Y - 0.1)) &&
                            (workStage.MC_Func.MC_GetEncPos((int)WorkStage.nAxis.Y) < (workStage.stWorkStageTeachingPos[(int)WorkStage.WorkStage_TeachingPosList.STAGE_LoadingPos].Stage_Y + 0.1)))
                    {
                        
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Work Stage, Module Loading 위치에 있으므로 PutDown 진행, (위치 및 로딩 조건 OK)");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.WorkStagePutDown_TransferX_Move_LoadingPos;
                    }
                    else
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Work Stage, Module Loading 위치로 이동 시작");

                        TickCount_Start((int)TickType.TICK_LDTR);

                        //  Work Stage 이동 시작
                        workStage.m_nWorkStageMoveType = (int)WorkStage.WorkStageMoveType.MoveTo_LoadingPos;
                        workStage.m_nWorkStage_Move_Step = (int)WorkStage.WorkStage_Move_Step.Start;

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.WorkStagePutDown_WorkStageCycle_LoadingPos_CompleteCheck;
                    }
                    break;


                case (int)Loader_Transfer_Step.WorkStagePutDown_WorkStageCycle_LoadingPos_CompleteCheck:                       //  M-Aligner 에서 Align 완료 확인 (M-Aligner Vacuum 이 Off 이면 Pick Up 하지 않음 --> 자재가 없다고 판단)

                    if (MC_Func.MC_GetDone((int)WorkStage.nAxis.X) && MC_Func.MC_GetDone((int)WorkStage.nAxis.Y) && 
                        (workStage.m_nWorkStagePosition == (int)WorkStage.WorkStagePosition.WorkStage_LoadingZone) && (workStage.m_nWorkStage_Move_Step == (int)WorkStage.WorkStage_Move_Step.None))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Work Stage, Module Loading 위치로 이동 완료");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.WorkStagePutDown_TransferX_Move_LoadingPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 6000000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Work Stage, Module Loading 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        return AlarmPost(AlarmKey.LD_Transfer_WorkStageMove_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Work Stage, Module Loading 위치로 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.WorkStagePutDown_TransferX_Move_LoadingPos:                            //  Transfer X 축, Work Stage Loading 위치로 이동

                    Loader_Transfer_Step_WorkStagePutDown_TransferX_Move_LoadingPos(out m_dSpeed, out m_dAccDec);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.WorkStagePutDown_TransferX_Move_LoadingPos_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.WorkStagePutDown_TransferX_Move_LoadingPos_DoneCheck:                       //  Transfer X 축, Work Stage Loading 위치로 이동 완료 확인 (Work Stage Loading 위치로 이동 Cycle 완료 확인 후, Transfer X 이동 완료 확인)

                    if (MC_Func.MC_GetDone((int)nAxis.TR_X) && MC_Func.MC_PosTolerance((int)nAxis.TR_X, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_X]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer X 축, Work Stage Loading 위치로 이동 완료");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.WorkStagePutDown_TransferZ_Move_PutDownPos_1stStep;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer X 축, Work Stage Loading 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        return AlarmPost(AlarmKey.LD_TransferX_Move_LoadingPos_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer X 축, Work Stage Loading 위치로 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.WorkStagePutDown_TransferZ_Move_PutDownPos_1stStep:                            //  Transfer Z 축, Module Put Down 위치로 이동 (1단계, 최종 위치에서 위로 10 mm)

                    Loader_Transfer_Step_WorkStagePutDown_TransferZ_Move_PutDownPos_1stStep(out m_dSpeed, out m_dAccDec);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.WorkStagePutDown_TransferZ_Move_PutDownPos_1stStep_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.WorkStagePutDown_TransferZ_Move_PutDownPos_1stStep_DoneCheck:                       //  Transfer Z 축, Module Put Down 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, Module PutDown 대기 위치로 이동 완료");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.WorkStagePutDown_TransferZ_Move_PutDownPos_2ndStep;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, Module PutDown 대기 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        return AlarmPost(AlarmKey.LD_TransferZ_Move_PutDownPos_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, Module PutDown 대기 위치로 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.WorkStagePutDown_TransferZ_Move_PutDownPos_2ndStep:                            //  Transfer Z 축, Module Put Down 위치로 이동 (2단계, 최종 위치)

                    Loader_Transfer_Step_WorkStagePutDown_TransferZ_Move_PutDownPos_2ndStep(out m_dSpeed, out m_dAccDec);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.WorkStagePutDown_TransferZ_Move_PutDownPos_2ndStep_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.WorkStagePutDown_TransferZ_Move_PutDownPos_2ndStep_DoneCheck:                       //  Transfer Z 축, Module Put Down 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, Module Put Down 위치로 이동 완료");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.WorkStagePutDown_WorkStage_Vacuum_On;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, Module Put Down 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        return AlarmPost(AlarmKey.LD_TransferZ_Move_PutDownPos_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, Module Put Down 위치로 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.WorkStagePutDown_WorkStage_Vacuum_On:                                     //  Work Stage, Vacuum On

                    Loader_Transfer_Step_WorkStagePutDown_WorkStage_Vacuum_On();

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.WorkStagePutDown_Transfer_PickerVacuum_Off;
                    break;


                case (int)Loader_Transfer_Step.WorkStagePutDown_Transfer_PickerVacuum_Off:                                     //  Transfer, Module Picker Vacuum Off (and Blow On)

                    Loader_Transfer_Step_WorkStagePutDown_Transfer_PickerVacuum_Off();

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.WorkStagePutDown_Transfer_PickerVacuum_OffCheck;
                    break;


                case (int)Loader_Transfer_Step.WorkStagePutDown_Transfer_PickerVacuum_OffCheck:                                //  Transfer, Module Picker Vacuum Off 확인 (and Work Stage Vacuum On 확인)

                    if ((!loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Inner) &&
                        !loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Outer)) &&
                        
                        ((Equipment.Machine_VacuumSensor_Enable && //workStage.workStageParameter.DI_Stage_Vacuum_Check() &&            //  Stage Vacuum 은 형성되지 않을 수도 있다. Picker 를 올린 후 다시 체크하도록 변경

                        (!Equipment.Machine_VacuumStableTime_Enable ||
                        (Equipment.Machine_VacuumStableTime_Enable && (TickCount_Elapsed((int)TickType.TICK_LDTR) > Equipment.Machine_VacuumStableTime)))) ||

                        (!Equipment.Machine_VacuumSensor_Enable && (TickCount_Elapsed((int)TickType.TICK_LDTR) > Equipment.Machine_SignalHoldTime))) &&

                        (!Equipment.Machine_VacuumBlowTime_Enable ||
                        (Equipment.Machine_VacuumBlowTime_Enable && (TickCount_Elapsed((int)TickType.TICK_LDTR) > Equipment.Machine_VacuumBlowTime))))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer 축, Module Picker Vacuum Off 완료");


                        //////////////////////////////////////////////////////////////////////////////////////////
                        //  복원 지점 체크용 (Work Stage 에 Module Put Down 완료) - Work Stage 공압이 형성되었고, Picker 공압이 파기되었으므로, 자재가 Stage 에 안착되었다고 본다.
                        //
                        m_bLD_Transfer_toWorkStage_Module_PutDown_Complete_Flag = true;
                        //
                        //  복원 지점 체크용 (Work Stage 에 Module Put Down 완료)
                        //////////////////////////////////////////////////////////////////////////////////////////
                        //To do: stage 압력 스펙 제어 부분 성부장님 파라미터로 빼!!!  --> ㅇㅋ염
                        if(workStage.workStageParameter.DI_Stage_Vacuum_Check() && (workStage.m_dEPRO_Value < Equipment.Machine_WorkStage_ModuleAbsorption_JudgeLevel))       //  Stage Vacuum 센서와 Regulator 값을 함께 본다.
                        {
                            m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.WorkStagePutDown_TransferZ_Move_ReadyPos2_1stStep;

                        }
                        else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 20000)
                        {
                            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Work Stage Vacuum On 실패. (Timeout)");

                            //  알람 정지 (LED Bar - Red Blink)
                            Equipment.MachineStop_byAlarm = true;

                            //timer_Motion_Home.Enabled = false;
                            //m_btimer_Motion_Home_Stop = true;

                            loaderParameter.DO_Loader_Picker_Blow(false);


                            //////////////////////////////////////////////////////////////////////////////////////////
                            //  재시작 위치 저장용
                            //
                            Equipment.MachineStop_byTimeout_Loader = true;
                            Loader_CurrentStatus_Save_StopedByTimeout();
                            //
                            //  재시작 위치 저장용
                            //////////////////////////////////////////////////////////////////////////////////////////                        

                            return AlarmPost(AlarmKey.LD_Transfer_WorkStageVacuumOn_Timeout);
                            m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;
                        }
                        else
                        {
                            //이 부분에서 자주 발생. 
                            workStage.workStageParameter.DO_Stage_Vacuum(true);
                            Thread.Sleep(100);  //너무 신호 빠른거 아닌지?
                            workStage.workStageParameter.DO_Stage_Blow(false);                   //  Blow Off

                            //  Stage Vacuum On 시, 진공레귤레이터도 함께 동작시켜야 한다.
                            workStage.ElectroPneumaticRegulatorComm_Pressure_Set(-60.0);            //  임시로 -30 고정
                            Thread.Sleep(100); //너무 신호 빠른거 아닌지?
                        }

                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 10000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer 축, Module Picker Vacuum Off 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        loaderParameter.DO_Loader_Picker_Blow(false);


                        //////////////////////////////////////////////////////////////////////////////////////////
                        //  재시작 위치 저장용
                        //
                        Equipment.MachineStop_byTimeout_Loader = true;
                        Loader_CurrentStatus_Save_StopedByTimeout();
                        //
                        //  재시작 위치 저장용
                        //////////////////////////////////////////////////////////////////////////////////////////                        

                        return AlarmPost(AlarmKey.LD_Transfer_PickerVacuumOff_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer 축, Module Picker Vacuum Off 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.WorkStagePutDown_TransferZ_Move_ReadyPos2_1stStep:                            //  Transfer Z 축, 대기 위치로 이동 (1단계, 현재 위치에서 위로 10 mm)

                    Loader_Transfer_Step_WorkStagePutDown_TransferZ_Move_ReadyPos2_1stStep(out m_dSpeed, out m_dAccDec);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.WorkStagePutDown_TransferZ_Move_ReadyPos2_1stStep_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.WorkStagePutDown_TransferZ_Move_ReadyPos2_1stStep_DoneCheck:                       //  Transfer Z 축, 대기 위치로 이동 완료 확인 (then Picker Blow Off)

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 1단계 이동 완료");

                        loaderParameter.DO_Loader_Picker_Blow(false);

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.WorkStagePutDown_TransferZ_Move_ReadyPos2_2ndStep;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 1단계 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        return AlarmPost(AlarmKey.LD_TransferZ_Move_ReadyPos_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, 대기 위치로 1단계 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.WorkStagePutDown_TransferZ_Move_ReadyPos2_2ndStep:                            //  Transfer Z 축, 대기 위치로 이동 (2단계, 최종 위치)

                    Loader_Transfer_Step_WorkStagePutDown_TransferZ_Move_ReadyPos2_2ndStep(out m_dSpeed, out m_dAccDec);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.WorkStagePutDown_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.WorkStagePutDown_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck:                       //  Transfer Z 축, 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 2단계 이동 완료");


                        if (Equipment.Machine_VacuumSensor_Enable)
                        {
                            //  여기서 Module 을 정상적으로 내려놓았는지 다시 체크                            
                            if (workStage.workStageParameter.DI_Stage_Vacuum_Check())
                            {
                                //  Loader 에서 Pick Up 한 Port 번호를 Work Stage 에 넘겨준다.
                                Equipment.AUTORUN_WorkStage_PickUpPort = Equipment.AUTORUN_Loader_PickUpPort;

                                m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Complete;
                            }
                            else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 10000)
                            {
                                Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Module 을 Work Stage 에 정상적으로 내려놓지 못함. Work Stage 에 공압이 확인되지 않음.");


                                //////////////////////////////////////////////////////////////////////////////////////////
                                //  복원 지점 체크용 (Work Stage 에 Module Put Down 실패) - Picker 공압이 파기되지 않음
                                //
                                m_bLD_Transfer_toWorkStage_Module_PutDown_Complete_Flag = false;
                                //
                                //  복원 지점 체크용 (Work Stage 에 Module Put Down 실패)
                                //////////////////////////////////////////////////////////////////////////////////////////

                                //  알람 정지 (LED Bar - Red Blink)
                                Equipment.MachineStop_byAlarm = true;

                                //////////////////////////////////////////////////////////////////////////////////////////
                                //  재시작 위치 저장용
                                //
                                Equipment.MachineStop_byTimeout_Loader = true;
                                Loader_CurrentStatus_Save_StopedByTimeout();
                                //
                                //  재시작 위치 저장용
                                //////////////////////////////////////////////////////////////////////////////////////////

                                return AlarmPost(AlarmKey.LD_Transfer_WorkStageVacuumOn_Timeout);
                                m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                                MessageBox.Show("Module 을 Work Stage 에 내려놓지 못함.\r\n[Work Stage Vacuum 확인되지 않음]\r\n\r\n[Port 의 Module Pick Up 부터 다시 진행됩니다.]\r\n\r\n[Picker 에 Module 이 있다면 제거하세요.]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            //else if (!loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Inner) &&
                            //        !loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Outer))
                            //{
                            //    Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Module 을 Work Stage 에 정상적으로 내려놓음");

                            //    //  Out.
                            //    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Complete;
                            //}
                            //else
                            //{
                            //    Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Module 을 Work Stage 에 정상적으로 내려놓지 못함");


                            //    //////////////////////////////////////////////////////////////////////////////////////////
                            //    //  복원 지점 체크용 (Work Stage 에 Module Put Down 실패) - Picker 공압이 파기되지 않음
                            //    //
                            //    m_bLD_Transfer_toWorkStage_Module_PutDown_Complete_Flag = false;
                            //    //
                            //    //  복원 지점 체크용 (Work Stage 에 Module Put Down 실패)
                            //    //////////////////////////////////////////////////////////////////////////////////////////

                            //    //  알람 정지 (LED Bar - Red Blink)
                            //    Equipment.MachineStop_byAlarm = true;

                            //    //////////////////////////////////////////////////////////////////////////////////////////
                            //    //  재시작 위치 저장용
                            //    //
                            //    Equipment.MachineStop_byTimeout_Loader = true;
                            //    Loader_CurrentStatus_Save_StopedByTimeout();
                            //    //
                            //    //  재시작 위치 저장용
                            //    //////////////////////////////////////////////////////////////////////////////////////////

                            //    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                            //    MessageBox.Show("Module 을 Work Stage 에 내려놓지 못함.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //}
                        }
                        else
                        {
                            //  Loader 에서 Pick Up 한 Port 번호를 Work Stage 에 넘겨준다.
                            Equipment.AUTORUN_WorkStage_PickUpPort = Equipment.AUTORUN_Loader_PickUpPort;

                            m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Complete;
                        }
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 2단계 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        return AlarmPost(AlarmKey.LD_TransferZ_Move_ReadyPos_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, 대기 위치로 2단계 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                /// <summary>
                /// Module 을 WorkStage 에 Put-Down 일 경우 - 완료
                /// </summary>



                /// <summary>
                /// Module 을 M-Aligner 에 Put-Down 일 경우 - 시작
                /// </summary>
                case (int)Loader_Transfer_Step.MAligner_ModulePutdown_Condition_Check:                            //  M-Aligner 에 Module Put Down 조건 체크 (Transfer Picker Vacuum On Check, M-Aligner Vacuum Off Check, M-Align Cycle : None)

                    Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "TR 축, M-Aligner 에 Module Put Down 조건 체크");

                    if (!workStage.m_bMainWorkCycle_DryRun && Equipment.Machine_VacuumSensor_Enable && !loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Inner) && !loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Outer))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Picker 에 Module 이 감지되지 않음.");

                        //  Out.
                        //m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;
                    }
                    else if (loaderParameter.DI_Loader_Aligner_VacuumCheck((int)LoaderParameter.MAlignerVacuumPos.Center) ||
                            loaderParameter.DI_Loader_Aligner_VacuumCheck((int)LoaderParameter.MAlignerVacuumPos.Inner) ||
                            loaderParameter.DI_Loader_Aligner_VacuumCheck((int)LoaderParameter.MAlignerVacuumPos.Outer))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "M-Aligner 에 Module 이 감지됨. (Vacuum Sensor : On)");

                        //  Out.
                        ///m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;
                    }
                    else if (m_nMAlign_Step > (int)MAlign_Step.None)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "M-Aligner 동작중.");

                        //  Out.
                        //m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;
                    }
                    else
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "TR 축, M-Aligner 에 Module Put Down 조건 OK");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPutDown_TransferZ_Move_ReadyPos;
                    }
                    break;


                case (int)Loader_Transfer_Step.MAlignerPutDown_TransferZ_Move_ReadyPos:                            //  Transfer Z 축, 대기 위치로 이동

                    Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 시작");

                    loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Transfer_To_M_Aligner");

                    //  Target Position 변경 : 대기 위치
                    loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_TR_SafetyPos].LD_Transfer_Z;

                    //  속도
                    m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Coarse;

                    //  가감속
                    m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Coarse;

                    MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                        loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z],
                                        m_dSpeed,
                                        m_dAccDec,
                                        m_dAccDec);

                    TickCount_Start((int)TickType.TICK_LDTR);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPutDown_TransferZ_Move_ReadyPos_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.MAlignerPutDown_TransferZ_Move_ReadyPos_DoneCheck:                       //  Transfer Z 축, 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 완료");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPutDown_MAlignerXY_Move_Widely;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;
                        
                        return AlarmPost(AlarmKey.LD_TransferZ_Move_ReadyPos_Timeout);

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, 대기 위치로 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.MAlignerPutDown_MAlignerXY_Move_Widely:                            //  M-Aligner XY 축, Module 을 내려놓을 수 있을만큼 넓히기

                    Loader_Transfer_Step_MAlignerPutDown_MAlignerXY_Move_Widely(out m_dSpeed, out m_dAccDec);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPutDown_MAlignerXY_Move_Widely_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.MAlignerPutDown_MAlignerXY_Move_Widely_DoneCheck:                       //  M-Aligner XY 축, Module 을 내려놓을 수 있을만큼 넓히기 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.ALN_X) && MC_Func.MC_PosTolerance((int)nAxis.ALN_X, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_X]) &&
                        MC_Func.MC_GetDone((int)nAxis.ALN_Y) && MC_Func.MC_PosTolerance((int)nAxis.ALN_Y, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_Y]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "M-Aligner XY 축, 현재 위치에서 20mm 넓히기 이동 완료");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPutDown_TransferX_Move_MAlignPos;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "M-Aligner XY 축, 현재 위치에서 20mm 넓히기 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        return AlarmPost(AlarmKey.LD_MAlignerXY_Move_Widely_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("M-Aligner XY 축, 현재 위치에서 20mm 넓히기 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.MAlignerPutDown_TransferX_Move_MAlignPos:                            //  Transfer X 축, M-Aligner Put Down 위치로 이동

                    Loader_Transfer_Step_MAlignerPutDown_TransferX_Move_MAlignPos(out m_dSpeed, out m_dAccDec);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPutDown_TransferX_Move_MAlignPos_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.MAlignerPutDown_TransferX_Move_MAlignPos_DoneCheck:                       //  Transfer X 축, M-Aligner Put Down 위치로 이동 완료 확인 (M-Aligner XY 축이 넓어진 후 이동 완료 확인)

                    if (MC_Func.MC_GetDone((int)nAxis.TR_X) && MC_Func.MC_PosTolerance((int)nAxis.TR_X, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_X]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer X 축, M-Aligner Put Down 위치로 이동 완료");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPutDown_TransferZ_Move_PutDownPos_1stStep;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer X 축, M-Aligner Put Down 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer X 축, M-Aligner Put Down 위치로 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.MAlignerPutDown_TransferZ_Move_PutDownPos_1stStep:                            //  Transfer Z 축, Module Put Down 위치로 이동 (1단계, 최종 위치에서 위로 10 mm)

                    Loader_Transfer_Step_MAlignerPutDown_TransferZ_Move_PutDownPos_1stStep(out m_dSpeed, out m_dAccDec);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPutDown_TransferZ_Move_PutDownPos_1stStep_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.MAlignerPutDown_TransferZ_Move_PutDownPos_1stStep_DoneCheck:                       //  Transfer Z 축, Module Put Down 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, M-Aligner Module PutDown 대기 위치로 이동 완료");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPutDown_TransferZ_Move_PutDownPos_2ndStep;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, M-Aligner Module PutDown 대기 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        return AlarmPost(AlarmKey.LD_TransferZ_Move_PutDownPos_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, M-Aligner Module PutDown 대기 위치로 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.MAlignerPutDown_TransferZ_Move_PutDownPos_2ndStep:                            //  Transfer Z 축, Module Put Down 위치로 이동 (2단계, 최종 위치)

                    Loader_Transfer_Step_MAlignerPutDown_TransferZ_Move_PutDownPos_2ndStep(out m_dSpeed, out m_dAccDec);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPutDown_TransferZ_Move_PutDownPos_2ndStep_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.MAlignerPutDown_TransferZ_Move_PutDownPos_2ndStep_DoneCheck:                       //  Transfer Z 축, Module Put Down 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, M-Aligner Module Put Down 위치로 이동 완료");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPutDown_MAligner_Vacuum_On;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, M-Aligner Module Put Down 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        return AlarmPost(AlarmKey.LD_TransferZ_Move_PutDownPos_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, M-Aligner Module Put Down 위치로 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.MAlignerPutDown_MAligner_Vacuum_On:                                     //  M-Aligner, Vacuum On

                    Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "M_Aligner, Module Vacuum On");

                    if (Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Center)
                    {
                        loaderParameter.DO_Loader_Aligner_Vacuum((int)LoaderParameter.MAlignerVacuumPos.Center, true);
                    }

                    if (Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Inner)
                    {
                        loaderParameter.DO_Loader_Aligner_Vacuum((int)LoaderParameter.MAlignerVacuumPos.Inner, true);
                    }

                    if (Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Outer)
                    {
                        loaderParameter.DO_Loader_Aligner_Vacuum((int)LoaderParameter.MAlignerVacuumPos.Outer, true);
                    }

                    //TickCount_Start((int)TickType.TICK_LDTR);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPutDown_Transfer_PickerVacuum_Off;
                    break;


                case (int)Loader_Transfer_Step.MAlignerPutDown_Transfer_PickerVacuum_Off:                                     //  Transfer, Module Picker Vacuum Off (and Blow On)

                    Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer 축, Module Picker Vacuum Off");

                    loaderParameter.DO_Loader_Picker_Vacuum((int)LoaderParameter.PickerVacuumPos.Inner, false);
                    loaderParameter.DO_Loader_Picker_Vacuum((int)LoaderParameter.PickerVacuumPos.Outer, false);
                    loaderParameter.DO_Loader_Picker_Blow(true);

                    TickCount_Start((int)TickType.TICK_LDTR);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPutDown_Transfer_PickerVacuum_OffCheck;
                    break;


                case (int)Loader_Transfer_Step.MAlignerPutDown_Transfer_PickerVacuum_OffCheck:                                //  Transfer, Module Picker Vacuum Off 확인 (and M-Aligner Vacuum On 확인)

                    if ((!loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Inner) &&
                        !loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Outer)) &&

                        (!Equipment.Machine_VacuumStableTime_Enable ||
                        (Equipment.Machine_VacuumStableTime_Enable && (TickCount_Elapsed((int)TickType.TICK_LDTR) > Equipment.Machine_VacuumStableTime))) &&

                        (!Equipment.Machine_VacuumBlowTime_Enable ||
                        (Equipment.Machine_VacuumBlowTime_Enable && (TickCount_Elapsed((int)TickType.TICK_LDTR) > Equipment.Machine_VacuumBlowTime))) )

                        //((Equipment.Machine_VacuumSensor_Enable &&

                        //    ((Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Center && loaderParameter.DI_Loader_Aligner_VacuumCheck((int)LoaderParameter.MAlignerVacuumPos.Center)) ||
                        //    !Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Center) &&

                        //    ((Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Inner && loaderParameter.DI_Loader_Aligner_VacuumCheck((int)LoaderParameter.MAlignerVacuumPos.Inner)) ||
                        //    !Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Inner) &&

                        //    ((Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Outer && loaderParameter.DI_Loader_Aligner_VacuumCheck((int)LoaderParameter.MAlignerVacuumPos.Outer)) ||
                        //    !Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Outer) &&

                        //    (!Equipment.Machine_VacuumStableTime_Enable ||
                        //    (Equipment.Machine_VacuumStableTime_Enable && (TickCount_Elapsed((int)TickType.TICK_LDTR) > Equipment.Machine_VacuumStableTime)))) ||

                        //(!Equipment.Machine_VacuumSensor_Enable && (TickCount_Elapsed((int)TickType.TICK_LDTR) > Equipment.Machine_SignalHoldTime))))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer 축, Module Picker Vacuum Off 완료");

                        loaderParameter.DO_Loader_Picker_Blow(false);


                        //////////////////////////////////////////////////////////////////////////////////////////
                        //  복원 지점 체크용 (M-Aligner 에 Module Put Down 완료) - Picker 공압이 파기되었으므로, 자재가 M-Aligner 에 안착되었다고 본다.
                        //
                        m_bLD_Transfer_toMAligner_Module_PutDown_Complete_Flag = true;
                        //
                        //  복원 지점 체크용 (M-Aligner 에 Module Put Down 완료)
                        //////////////////////////////////////////////////////////////////////////////////////////


                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPutDown_TransferZ_Move_ReadyPos2_1stStep;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 10000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer 축, Module Picker Vacuum Off 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;


                        //////////////////////////////////////////////////////////////////////////////////////////
                        //  재시작 위치 저장용
                        //
                        Equipment.MachineStop_byTimeout_Loader = true;
                        Loader_CurrentStatus_Save_StopedByTimeout();
                        //
                        //  재시작 위치 저장용
                        //////////////////////////////////////////////////////////////////////////////////////////
                        ///

                        return AlarmPost(AlarmKey.LD_Transfer_PickerVacuumOff_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer 축, Module Picker Vacuum Off 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.MAlignerPutDown_TransferZ_Move_ReadyPos2_1stStep:                            //  Transfer Z 축, 대기 위치로 이동 (1단계, 현재 위치에서 위로 10 mm)

                    Loader_Transfer_Step_MAlignerPutDown_TransferZ_Move_ReadyPos2_1stStep(out m_dSpeed, out m_dAccDec);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPutDown_TransferZ_Move_ReadyPos2_1stStep_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.MAlignerPutDown_TransferZ_Move_ReadyPos2_1stStep_DoneCheck:                       //  Transfer Z 축, 대기 위치로 이동 완료 확인 (then Picker Blow Off)

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 1단계 이동 완료");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPutDown_TransferZ_Move_ReadyPos2_2ndStep;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 1단계 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        return AlarmPost(AlarmKey.LD_TransferZ_Move_ReadyPos_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, 대기 위치로 1단계 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.MAlignerPutDown_TransferZ_Move_ReadyPos2_2ndStep:                            //  Transfer Z 축, 대기 위치로 이동 (2단계, 최종 위치)

                    Loader_Transfer_Step_MAlignerPutDown_TransferZ_Move_ReadyPos2_2ndStep(out m_dSpeed, out m_dAccDec);

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPutDown_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck;
                    break;


                case (int)Loader_Transfer_Step.MAlignerPutDown_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck:                       //  Transfer Z 축, 대기 위치로 이동 완료 확인

                    if (MC_Func.MC_GetDone((int)nAxis.TR_Z) && MC_Func.MC_PosTolerance((int)nAxis.TR_Z, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 2단계 이동 완료");

                        TickCount_Start((int)TickType.TICK_LDTR);

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPutDown_PickerVacuum_MAlignerVacuum_Check;

                        //m_bMAlign_Complete = false;

                        ////if (!Equipment.AutoRunStatus)
                        //{
                        //    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPutDown_MAlign_Start;
                        //}
                        ////else
                        ////{
                        ////    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Complete;
                        ////}
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 2단계 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        return AlarmPost(AlarmKey.LD_TransferZ_Move_ReadyPos_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer Z 축, 대기 위치로 2단계 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.MAlignerPutDown_PickerVacuum_MAlignerVacuum_Check:                            //  Transfer Picker Vacuum On Check, M-Aligner Vacuum Off Check

                    if ((!loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Inner) &&
                        !loaderParameter.DI_Loader_Picker_VacuumCheck((int)LoaderParameter.PickerVacuumPos.Outer)) &&

                        ((Equipment.Machine_VacuumSensor_Enable &&

                            ((Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Center && loaderParameter.DI_Loader_Aligner_VacuumCheck((int)LoaderParameter.MAlignerVacuumPos.Center)) ||
                            !Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Center) &&

                            //((Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Inner && loaderParameter.DI_Loader_Aligner_VacuumCheck((int)LoaderParameter.MAlignerVacuumPos.Inner)) ||
                            //!Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Inner) &&
                            //  아래 임시 코드
                            ((Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Inner && (loaderParameter.DI_Loader_Aligner_VacuumCheck((int)LoaderParameter.MAlignerVacuumPos.Inner) ||
                                                                                        loaderParameter.DI_Loader_Aligner_VacuumCheck((int)LoaderParameter.MAlignerVacuumPos.Center) ||
                                                                                        loaderParameter.DI_Loader_Aligner_VacuumCheck((int)LoaderParameter.MAlignerVacuumPos.Outer))) ||
                            !Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Inner) &&

                            ((Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Outer && loaderParameter.DI_Loader_Aligner_VacuumCheck((int)LoaderParameter.MAlignerVacuumPos.Outer)) ||
                            !Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Outer)) ||
                            
                        !Equipment.Machine_VacuumSensor_Enable))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer 축, Module Picker Vacuum Off, M-Aligner Vacuum On 완료");


                        //////////////////////////////////////////////////////////////////////////////////////////
                        //  복원 지점 체크용 (M-Aligner 에 Module Put Down 완료) - M-Aligner 에 공압이 형성되었고, Picker 공압이 파기되었으므로, 자재가 M-Aligner 에 안착되었다고 본다.
                        //
                        m_bLD_Transfer_toMAligner_Module_PutDown_Complete_Flag = true;
                        //
                        //  복원 지점 체크용 (M-Aligner 에 Module Put Down 완료)
                        //////////////////////////////////////////////////////////////////////////////////////////


                        m_bMAlign_Complete = false;
                        m_bMAlign_Retry = false;


                        //if (!Equipment.AutoRunStatus)
                        {
                            m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPutDown_MAlign_Start;
                        }
                        //else
                        //{
                        //    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Complete;
                        //}
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer 축, Module Picker Vacuum Off 또는 M-Aligner Vacuum On 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;


                        //////////////////////////////////////////////////////////////////////////////////////////
                        //  복원 지점 체크용 (M-Aligner 에 Module Put Down 실패) - M-Aligner 에 공압이 형성되지 않았거나, Picker 공압이 파기되지 않았음.
                        //
                        m_bLD_Transfer_toMAligner_Module_PutDown_Complete_Flag = false;
                        //
                        //  복원 지점 체크용 (M-Aligner 에 Module Put Down 실패)
                        //////////////////////////////////////////////////////////////////////////////////////////


                        //////////////////////////////////////////////////////////////////////////////////////////
                        //  재시작 위치 저장용
                        //
                        Equipment.MachineStop_byTimeout_Loader = true;
                        Loader_CurrentStatus_Save_StopedByTimeout();
                        //
                        //  재시작 위치 저장용
                        //////////////////////////////////////////////////////////////////////////////////////////
                        ///

                        return AlarmPost(AlarmKey.LD_Transfer_PickerVacuumOff_MAlignerVacuumOn_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("Transfer 축, Module Picker Vacuum Off 또는 M-Aligner Vacuum On 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)Loader_Transfer_Step.MAlignerPutDown_MAlign_Start:                            //  M-Aligner 에서 Align 시작

                    Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "M-Aligner, Align 시작");

                    TickCount_Start((int)TickType.TICK_LDTR);

                    m_bMAlignZone_ModuleExist = true;                       //  M-Aligner 에 모듈을 내려놨으므로

                    //  M-Align 시작
                    m_nMAlign_Step = (int)MAlign_Step.Start;

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.MAlignerPutDown_MAlign_CompleteCheck;
                    break;


                case (int)Loader_Transfer_Step.MAlignerPutDown_MAlign_CompleteCheck:                       //  M-Aligner 에서 Align 완료 확인 (M-Aligner Vacuum 이 Off 이면 Pick Up 하지 않음 --> 자재가 없다고 판단)

                    if (MC_Func.MC_GetDone((int)nAxis.ALN_X) && MC_Func.MC_GetDone((int)nAxis.ALN_Y) && (m_nMAlign_Step == (int)MAlign_Step.None))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "M-Aligner, Align 완료");

                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.Complete;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_LDTR) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "M-Aligner, Align 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        //timer_Motion_Home.Enabled = false;
                        //m_btimer_Motion_Home_Stop = true;

                        //////////////////////////////////////////////////////////////////////////////////////////
                        /// 

                        return AlarmPost(AlarmKey.LD_Transfer_MAligner_Align_Timeout);
                        m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;

                        MessageBox.Show("M-Aligner, Align 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                /// <summary>
                /// Module 을 M-Aligner 에 Put-Down 일 경우 - 완료
                /// </summary>



                case (int)Loader_Transfer_Step.Complete:

                    Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "완료");

                    //  m_bHomeOK = true;

                    //timer_Motion_Home.Enabled = false;
                    //m_btimer_Motion_Home_Stop = true;

                    switch (m_nLoaderTransferMoveType)
                    {
                        case (int)LoaderTransferMoveType.Cycle_Transfer_ReadyPos:
                            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Ready Position 이동 완료");
                            m_strTemp = "LD Transfer, Ready Position 이동 완료";
                            break;

                        case (int)LoaderTransferMoveType.Cycle_Stacker0_PickUp:
                            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Stacker0 에서 Module Pick Up 완료");
                            m_strTemp = "LD Transfer, Stacker0 에서 Module Pick Up 완료";

                            m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker0_Complete = true;
                            m_bStacker0_Complete = false;

                            m_nLoaderTransfer_ProcessStep = (int)LoaderTransferProcessStep.LoaderStep_ModulePutDown_MAligner;
                            break;

                        case (int)LoaderTransferMoveType.Cycle_Stacker1_PickUp:
                            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Stacker1 에서 Module Pick Up 완료");
                            m_strTemp = "LD Transfer, Stacker1 에서 Module Pick Up 완료";

                            m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker1_Complete = true;
                            m_bStacker1_Complete = false;

                            m_nLoaderTransfer_ProcessStep = (int)LoaderTransferProcessStep.LoaderStep_ModulePutDown_MAligner;
                            break;

                        case (int)LoaderTransferMoveType.Cycle_MAligner_PutDown:
                            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "M-Aligner 에 Module Put Down 완료");
                            m_strTemp = "LD Transfer, M-Aligner 에 Module Put Down 완료";

                            m_bAUTORUN_Loader_Transfer_ModulePutDowntoMAligner_Complete = true;
                            //m_bLoader_Transfer_ModulePickUpfromStacker_Complete = false;
                            m_bMAlignZone_ModuleExist = true;
                            //m_bMAlign_Complete = false;
                            m_bMAlign_Retry = false;

                            m_nLoaderTransfer_ProcessStep = (int)LoaderTransferProcessStep.LoaderStep_ModulePickUp_MAligner;
                            break;

                        case (int)LoaderTransferMoveType.Cycle_MAligner_PickUp:
                            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "M-Aligner 에서 Module Pick Up 완료");
                            m_strTemp = "LD Transfer, M-Aligner 에서 Module Pick Up 완료";

                            m_bAUTORUN_Loader_Transfer_ModulePickUpfromMAligner_Complete = true;
                            m_bAUTORUN_Loader_Transfer_ModulePutDowntoMAligner_Complete = false;
                            m_bMAlignZone_ModuleExist = false;
                            m_bMAlign_Complete = false;
                            m_bMAlign_Retry = false;

                            m_nLoaderTransfer_ProcessStep = (int)LoaderTransferProcessStep.LoaderStep_ModulePutDown_Stage;
                            break;

                        case (int)LoaderTransferMoveType.Cycle_WorkStage_PutDown:
                            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Work Stage 에 Module Put Down 완료");
                            m_strTemp = "LD Transfer, Work Stage 에 Module Put Down 완료";

                            //  Work Stage 에 모듈을 내려놨으니 도면을 다시 로드해야 한다. (DryRun 이면 안함)       --> Work Stage 에 Module 을 Loading 하기 시작할 때 Parsing 하도록
                            //if (!workStage.m_bMainWorkCycle_DryRun)
                            //{
                            //    workStage.Import_DrawingFile(Equipment.RecipeOpen_DrawingFilePath);
                            //}

                            //  WorkStage 의 MainWork 에서 Parsing 진행
                            Equipment.ProcessingData_Parsing_byLoader = true;

                            m_bAUTORUN_Loader_Transfer_ModulePutDowntoWorkStage_Complete = true;
                            m_bAUTORUN_Loader_Transfer_ModulePickUpfromMAligner_Complete = false;
                            m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker0_Complete = false;
                            m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker1_Complete = false;

                            m_nLoaderTransfer_ProcessStep = (int)LoaderTransferProcessStep.LoaderStep_ModulePickup_fromStacker;

                            workStage.m_bDryRun_Complete = false;
                            workStage.m_bLaserDrilling_Complete = false;


                            //  Cycle Stop 이면?   --> Loader 에게 Cycle Stop 은, Stage 에 Module 을 갖다 놓으면 Cycle 완료
                            if (Equipment.CycleStop)
                            {
                                //  Loader Transfer 돌아가지 않게
                                Equipment.CycleStopped_LoaderTransfer = true;
                            }
                            break;
                    }

                    m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;


                    //  Seq. Test 일 경우
                    if (!Equipment.AutoRunStatus && Equipment.SeqTestMode)
                    {
                        Equipment.SeqTestMode = false;

                        MessageBox.Show(m_strTemp, "Information!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;
            }
            return 0;     
        }

        private void Loader_Transfer_Step_MAlignerPutDown_TransferZ_Move_ReadyPos2_2ndStep(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 시작. (2단계, 최종 위치)");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Transfer_To_L_Port");

            //  Target Position 변경 : 대기 위치 2단계 (최종 위치)
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_TR_SafetyPos].LD_Transfer_Z;

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Coarse;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Coarse;

            MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_MAlignerPutDown_TransferZ_Move_ReadyPos2_1stStep(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 시작. (1단계, 현재 위치에서 10mm 위)");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Transfer_To_L_Port");

            //  Target Position 변경 : 현재 위치 에서 10 mm 위, 1단계
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = MC_Func.MC_GetEncPos((int)nAxis.TR_Z) + 10.0;

            //  Dry Run 모드이면 10mm 더 위로
            if (workStage.m_bMainWorkCycle_DryRun)
            {
                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] += 10.0;

                if (loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] > 0)
                {
                    loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = 0;
                }
            }

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Fine;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Fine;

            MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_MAlignerPutDown_TransferZ_Move_PutDownPos_2ndStep(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, M-Aligner Module Put Down 위치로 이동 시작.");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Transfer_To_M_Aligner");

            //  Target Position 변경 : Module Pickup 위치
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_TR_MAlignPos].LD_Transfer_Z;

            //  Dry Run 모드이면 10mm 더 위로
            if (workStage.m_bMainWorkCycle_DryRun)
            {
                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] += 10.0;

                if (loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] > 0)
                {
                    loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = 0;
                }
            }

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Fine;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Fine;

            MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_MAlignerPutDown_TransferZ_Move_PutDownPos_1stStep(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, M-Aligner Module PutDown 대기 위치로 이동 시작. (10mm 위)");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Transfer_To_M_Aligner");

            //  Target Position 변경 : Module PutDown 대기 위치
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_TR_MAlignPos].LD_Transfer_Z + 10.0;

            //  Dry Run 모드이면 10mm 더 위로
            if (workStage.m_bMainWorkCycle_DryRun)
            {
                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] += 10.0;

                if (loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] > 0)
                {
                    loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = 0;
                }
            }

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Coarse;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Coarse;

            MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_MAlignerPutDown_TransferX_Move_MAlignPos(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer X 축, M-Aligner Put Down 위치로 이동 시작");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Transfer_To_M_Aligner");

            //  Target Position 변경 : M-Aligner 위치
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_X] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_TR_MAlignPos].LD_Transfer_X;

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_X].Common_Speed_Coarse;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_X].Common_Acceleration_Coarse;

            MC_Func.MC_MovePosition((int)nAxis.TR_X,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_X],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_MAlignerPutDown_MAlignerXY_Move_Widely(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "M-Aligner XY 축, 현재 위치에서 20mm 넓히기 시작.");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("M_Aligner_Open");

            //  Target Position 변경 : 현재 위치에서 10mm 넓히기
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_X] = MC_Func.MC_GetEncPos((int)nAxis.ALN_X) + 20.0;
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_Y] = MC_Func.MC_GetEncPos((int)nAxis.ALN_Y) + 20.0;

            //  최대로 넓힌 위치값과 비교하여, 초과할 경우 최대 넓힌 위치값으로 변경
            if (loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_X] > stLDULTeachingPos[(int)LDUL_TeachingPosList.MAligner_OpenPos].MAligner_X)
            {
                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_X] = stLDULTeachingPos[(int)LDUL_TeachingPosList.MAligner_OpenPos].MAligner_X;
            }

            if (loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_Y] > stLDULTeachingPos[(int)LDUL_TeachingPosList.MAligner_OpenPos].MAligner_Y)
            {
                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_Y] = stLDULTeachingPos[(int)LDUL_TeachingPosList.MAligner_OpenPos].MAligner_Y;
            }

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.ALN_X].Common_Speed_Coarse;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.ALN_X].Common_Acceleration_Coarse;

            MC_Func.MC_MovePosition((int)nAxis.ALN_X,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_X],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            MC_Func.MC_MovePosition((int)nAxis.ALN_Y,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_Y],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_WorkStagePutDown_TransferZ_Move_ReadyPos2_2ndStep(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 시작. (2단계, 최종 위치)");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Transfer_To_L_Port");

            //  Target Position 변경 : 대기 위치 2단계 (최종 위치)
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_TR_SafetyPos].LD_Transfer_Z;

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Coarse;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Coarse;

            MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_WorkStagePutDown_TransferZ_Move_ReadyPos2_1stStep(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 시작. (1단계, 현재 위치에서 10mm 위)");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Transfer_To_L_Port");

            //  Target Position 변경 : 현재 위치 에서 10 mm 위, 1단계
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = MC_Func.MC_GetEncPos((int)nAxis.TR_Z) + 10.0;

            //  Dry Run 모드이면 10mm 더 위로
            if (workStage.m_bMainWorkCycle_DryRun)
            {
                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] += 10.0;

                if (loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] > 0)
                {
                    loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = 0;
                }
            }

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Fine;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Fine;

            MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_WorkStagePutDown_Transfer_PickerVacuum_Off()
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer 축, Module Picker Vacuum Off");

            loaderParameter.DO_Loader_Picker_Vacuum((int)LoaderParameter.PickerVacuumPos.Inner, false);
            loaderParameter.DO_Loader_Picker_Vacuum((int)LoaderParameter.PickerVacuumPos.Outer, false);
            loaderParameter.DO_Loader_Picker_Blow(true);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private static void Loader_Transfer_Step_WorkStagePutDown_WorkStage_Vacuum_On()
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "WorkStage, Module Vacuum On");

            workStage.workStageParameter.DO_Stage_Vacuum(true);
            workStage.workStageParameter.DO_Stage_Blow(false);                   //  Blow Off
            workStage.DustCollector_SetFrequence(Equipment.stLayerRecipeSet[0].DustCollectorFreq_Lower);
            Thread.Sleep(1000);
            workStage.DustCollector_On((int)nDustCollector.DustCollector_Lower);

            //  Stage Vacuum On 시, 진공레귤레이터도 함께 동작시켜야 한다.
            workStage.ElectroPneumaticRegulatorComm_Pressure_Set(-60.0);            //  임시로 -30 고정
            Thread.Sleep(100);
            //TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_WorkStagePutDown_TransferZ_Move_PutDownPos_2ndStep(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, Module Put Down 위치로 이동 시작.");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Transfer_To_WorkStage");

            //  Target Position 변경 : Module Pickup 위치
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_TR_WorkTablePos].LD_Transfer_Z;

            //  Dry Run 모드이면 10mm 더 위로
            if (workStage.m_bMainWorkCycle_DryRun)
            {
                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] += 10.0;

                if (loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] > 0)
                {
                    loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = 0;
                }
            }

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Fine;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Fine;

            MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_WorkStagePutDown_TransferZ_Move_PutDownPos_1stStep(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, Module PutDown 대기 위치로 이동 시작. (10mm 위)");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Transfer_To_WorkStage");

            //  Target Position 변경 : Module PutDown 대기 위치
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_TR_WorkTablePos].LD_Transfer_Z + 10.0;

            //  Dry Run 모드이면 10mm 더 위로
            if (workStage.m_bMainWorkCycle_DryRun)
            {
                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] += 10.0;

                if (loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] > 0)
                {
                    loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = 0;
                }
            }

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Coarse;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Coarse;

            MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_WorkStagePutDown_TransferX_Move_LoadingPos(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer X 축, Work Stage Loading 위치로 이동 시작");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Transfer_To_WorkStage");

            //  Target Position 변경 : M-Aligner 위치
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_X] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_TR_WorkTablePos].LD_Transfer_X;

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_X].Common_Speed_Coarse;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_X].Common_Acceleration_Coarse;

            MC_Func.MC_MovePosition((int)nAxis.TR_X,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_X],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_WorkStagePutDown_TransferZ_Move_ReadyPos(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 시작");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Transfer_To_M_Aligner");

            //  Target Position 변경 : 대기 위치
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_TR_SafetyPos].LD_Transfer_Z;

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Coarse;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Coarse;

            MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_MAlignerPickUp_TransferZ_Move_ReadyPos2_2ndStep(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 시작. (2단계, 최종 위치)");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Transfer_To_L_Port");

            //  Target Position 변경 : 대기 위치 2단계 (최종 위치)
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_TR_SafetyPos].LD_Transfer_Z;

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Coarse;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Coarse;

            MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_MAlignerPickUp_MAlignerXY_MoveType2_Widely(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "M-Aligner XY 축, 10mm 넓히기 시작.");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("M_Aligner_Open");

            //  Target Position 변경 : 현재 위치에서 2mm 넓히기 (Equipment.Machine_MAligner_WidenDistance 이만큼 했었는데, 2mm 면 충분하다고 판단)
            //  5mm 도 부족하다 -_- 10mm 가자
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_X] = MC_Func.MC_GetEncPos((int)nAxis.ALN_X) + 10.0;
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_Y] = MC_Func.MC_GetEncPos((int)nAxis.ALN_Y) + 10.0;

            //  Target Position 체크
            if (loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_X] > stLDULTeachingPos[(int)LDUL_TeachingPosList.MAligner_OpenPos].MAligner_X)
            {
                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_X] = stLDULTeachingPos[(int)LDUL_TeachingPosList.MAligner_OpenPos].MAligner_X;
            }

            if (loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_Y] > stLDULTeachingPos[(int)LDUL_TeachingPosList.MAligner_OpenPos].MAligner_Y)
            {
                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_Y] = stLDULTeachingPos[(int)LDUL_TeachingPosList.MAligner_OpenPos].MAligner_Y;
            }

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.ALN_X].Common_Speed_Fine;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.ALN_X].Common_Acceleration_Fine;

            MC_Func.MC_MovePosition((int)nAxis.ALN_X,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_X],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            MC_Func.MC_MovePosition((int)nAxis.ALN_Y,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_Y],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_MAlignerPickUp_TransferZ_Move_ReadyPos2_1stStep(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 시작. (1단계, 현재 위치에서 10mm 위)");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Transfer_To_L_Port");

            //  Target Position 변경 : 현재 위치 에서 10 mm 위, 1단계
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = MC_Func.MC_GetEncPos((int)nAxis.TR_Z) + 10.0;

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Fine / 4.0;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Fine;

            MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_MAlignerPickUp_MAlignerXY_MoveType1_Widely(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "M-Aligner XY 축, 10mm 넓히기 시작.");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("M_Aligner_Open");

            //  Target Position 변경 : 현재 위치에서 2mm 넓히기 (Equipment.Machine_MAligner_WidenDistance 이만큼 했었는데, 2mm 면 충분하다고 판단)
            //  5mm 도 부족하다 -_- 10mm 가자
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_X] = MC_Func.MC_GetEncPos((int)nAxis.ALN_X) + 10.0;
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_Y] = MC_Func.MC_GetEncPos((int)nAxis.ALN_Y) + 10.0;

            //  Target Position 체크
            if (loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_X] > stLDULTeachingPos[(int)LDUL_TeachingPosList.MAligner_OpenPos].MAligner_X)
            {
                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_X] = stLDULTeachingPos[(int)LDUL_TeachingPosList.MAligner_OpenPos].MAligner_X;
            }

            if (loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_Y] > stLDULTeachingPos[(int)LDUL_TeachingPosList.MAligner_OpenPos].MAligner_Y)
            {
                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_Y] = stLDULTeachingPos[(int)LDUL_TeachingPosList.MAligner_OpenPos].MAligner_Y;
            }

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.ALN_X].Common_Speed_Fine;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.ALN_X].Common_Acceleration_Fine;

            MC_Func.MC_MovePosition((int)nAxis.ALN_X,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_X],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            MC_Func.MC_MovePosition((int)nAxis.ALN_Y,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_Y],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_MAlignerPickUp_TransferZ_Move_PickUpPos_2ndStep(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, Module Pickup 위치로 이동 시작.");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Transfer_To_M_Aligner");

            //  Target Position 변경 : Module Pickup 위치
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_TR_MAlignPos].LD_Transfer_Z;

            //  Dry Run 모드이면 10mm 더 위로
            if (workStage.m_bMainWorkCycle_DryRun)
            {
                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] += 10.0;

                if (loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] > 0)
                {
                    loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = 0;
                }
            }

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Fine;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Fine;

            MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_MAlignerPickUp_TransferZ_Move_PickUpPos_1stStep(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, Module Pickup 대기 위치로 이동 시작. (10mm 위)");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Transfer_To_M_Aligner");

            //  Target Position 변경 : Module Pickup 대기 위치
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_TR_MAlignPos].LD_Transfer_Z + 10.0;

            //  Dry Run 모드이면 10mm 더 위로
            if (workStage.m_bMainWorkCycle_DryRun)
            {
                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] += 10.0;

                if (loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] > 0)
                {
                    loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = 0;
                }
            }

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Coarse;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Coarse;

            MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_MAlignerPickUp_Retry_TransferZ_Move_ReadyPos2_2ndStep(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, Retry, 대기 위치로 이동 시작. (2단계, 최종 위치)");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Transfer_To_L_Port");

            //  Target Position 변경 : 대기 위치 2단계 (최종 위치)
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_TR_SafetyPos].LD_Transfer_Z;

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Coarse;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Coarse;

            MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_MAlignerPickUp_Retry_TransferZ_Move_ReadyPos2_1stStep(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, Retry, 대기 위치로 이동 시작. (1단계, 현재 위치에서 10mm 위)");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Transfer_To_L_Port");

            //  Target Position 변경 : 현재 위치 에서 10 mm 위, 1단계
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = MC_Func.MC_GetEncPos((int)nAxis.TR_Z) + 10.0;

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Fine;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Fine;

            MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_MAlignerPickUp_Retry_MAligner_Vacuum_On()
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "M_Aligner, Retry, Module Vacuum On");

            if (Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Center)
            {
                loaderParameter.DO_Loader_Aligner_Vacuum((int)LoaderParameter.MAlignerVacuumPos.Center, true);
            }

            if (Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Inner)
            {
                loaderParameter.DO_Loader_Aligner_Vacuum((int)LoaderParameter.MAlignerVacuumPos.Inner, true);
            }

            if (Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Outer)
            {
                loaderParameter.DO_Loader_Aligner_Vacuum((int)LoaderParameter.MAlignerVacuumPos.Outer, true);
            }
        }

        private void Loader_Transfer_Step_MAlignerPickUp_Retry_TransferZ_Move_PickUpPos_2ndStep(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, Retry, Module Press 위치로 이동 시작.");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Transfer_To_M_Aligner");

            //  Target Position 변경 : Module Pickup 위치
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_TR_MAlignPos].LD_Transfer_Z;

            //  Dry Run 모드이면 10mm 더 위로
            if (workStage.m_bMainWorkCycle_DryRun)
            {
                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] += 10.0;

                if (loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] > 0)
                {
                    loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = 0;
                }
            }

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Fine;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Fine;

            MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_MAlignerPickUp_Retry_TransferZ_Move_PickUpPos_1stStep(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, Retry, Module Press 대기 위치로 이동 시작. (10mm 위)");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Transfer_To_M_Aligner");

            //  Target Position 변경 : Module Pickup 대기 위치
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_TR_MAlignPos].LD_Transfer_Z + 10.0;

            //  Dry Run 모드이면 10mm 더 위로
            if (workStage.m_bMainWorkCycle_DryRun)
            {
                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] += 10.0;

                if (loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] > 0)
                {
                    loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = 0;
                }
            }

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Coarse;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Coarse;

            MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_MAlignerPickUp_Retry_Transfer_PickerVacuum_Off()
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer 축, Retry, Module Picker Vacuum Off");

            loaderParameter.DO_Loader_Picker_Vacuum((int)LoaderParameter.PickerVacuumPos.Inner, false);
            loaderParameter.DO_Loader_Picker_Vacuum((int)LoaderParameter.PickerVacuumPos.Outer, false);
            loaderParameter.DO_Loader_Picker_Blow(false);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_MAlignerPickUp_TransferX_Move_MAlignerPos(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer X 축, M-Aligner 위치로 이동 시작");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Transfer_To_M_Aligner");

            //  Target Position 변경 : M-Aligner 위치
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_X] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_TR_MAlignPos].LD_Transfer_X;

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_X].Common_Speed_Coarse;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_X].Common_Acceleration_Coarse;

            MC_Func.MC_MovePosition((int)nAxis.TR_X,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_X],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_MAlignerPickUp_TransferZ_Move_ReadyPos(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 시작");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Transfer_To_M_Aligner");

            //  Target Position 변경 : 대기 위치
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_TR_SafetyPos].LD_Transfer_Z;

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Coarse;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Coarse;

            MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_MAligner_MAlign_Start()
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "M-Aligner, Align 시작");

            loaderParameter.DO_Loader_Aligner_Blow((int)LoaderParameter.MAlignerVacuumPos.Center, false);
            loaderParameter.DO_Loader_Aligner_Blow((int)LoaderParameter.MAlignerVacuumPos.Inner, false);
            loaderParameter.DO_Loader_Aligner_Blow((int)LoaderParameter.MAlignerVacuumPos.Outer, false);

            //  얼라인을 진행해야 하니 다시 Vacuum 을 잡는다.
            if (Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Center)
            {
                loaderParameter.DO_Loader_Aligner_Vacuum((int)LoaderParameter.MAlignerVacuumPos.Center, true);
            }

            if (Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Inner)
            {
                loaderParameter.DO_Loader_Aligner_Vacuum((int)LoaderParameter.MAlignerVacuumPos.Inner, true);
            }

            if (Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Outer)
            {
                loaderParameter.DO_Loader_Aligner_Vacuum((int)LoaderParameter.MAlignerVacuumPos.Outer, true);
            }

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_Stacker1PickUp_TransferZ_Move_ReadyPos2_2ndStep(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 시작. (2단계, 최종 위치)");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Transfer_To_L_Port");

            //  Target Position 변경 : 대기 위치 2단계 (최종 위치)
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_TR_SafetyPos].LD_Transfer_Z;

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Coarse;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Coarse;

            MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_Stacker1PickUp_TransferZ_Move_ReadyPos2_1stStep(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 시작. (1단계, 현재 위치에서 10mm 위)");

            double m_dDownDistance = 5.0;

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Transfer_To_L_Port");

            //  Target Position 변경 : 대기 위치 1단계
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_TR_LPortPos].LD_Transfer_Z + 10.0;
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z1] = MC_Func.MC_GetEncPos((int)nAxis.Z1);

            //  LoaderZ 축을 올리면서 Stacker 축을 내릴 경우
            if (Equipment.Machine_LoaderStacker_Down_afterLoaderPickUp_Enable)
            {
                m_dDownDistance = Equipment.Machine_LoaderStacker_DownDistance_afterLoaderPickUp < 0.0 ? 5.0 : Equipment.Machine_LoaderStacker_DownDistance_afterLoaderPickUp;

                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z1] -= m_dDownDistance;
            }

            //  Dry Run 모드이면 10mm 더 위로
            if (workStage.m_bMainWorkCycle_DryRun)
            {
                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] += 10.0;

                if (loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] > 0)
                {
                    loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = 0;
                }
            }

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Fine;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Fine;

            MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            MC_Func.MC_MovePosition((int)nAxis.Z1,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z1],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_Stacker1PickUp_Transfer_PickerVacuum_On()
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer 축, Module Picker Vacuum On");

            loaderParameter.DO_Loader_Picker_Vacuum((int)LoaderParameter.PickerVacuumPos.Inner, true);
            loaderParameter.DO_Loader_Picker_Vacuum((int)LoaderParameter.PickerVacuumPos.Outer, true);
            loaderParameter.DO_Loader_Picker_Blow(false);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_Stacker1PickUp_TransferZ_Move_PickUpPos_2ndStep(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, Module Pickup 위치로 이동 시작.");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Transfer_To_L_Port");

            //  Target Position 변경 : Module Pickup 위치
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_TR_LPortPos].LD_Transfer_Z;

            //  Dry Run 모드이면 10mm 더 위로
            if (workStage.m_bMainWorkCycle_DryRun)
            {
                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] += 10.0;

                if (loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] > 0)
                {
                    loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = 0;
                }
            }

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Fine;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Fine;

            MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_Stacker1PickUp_TransferZ_Move_PickUpPos_1stStep(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, Module Pickup 대기 위치로 이동 시작. (10mm 위)");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Transfer_To_L_Port");

            //  Target Position 변경 : Module Pickup 대기 위치
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_TR_LPortPos].LD_Transfer_Z + 10.0;

            //  Dry Run 모드이면 10mm 더 위로
            if (workStage.m_bMainWorkCycle_DryRun)
            {
                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] += 10.0;

                if (loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] > 0)
                {
                    loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = 0;
                }
            }

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Coarse;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Coarse;

            MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_Stacker1PickUp_TransferX_Move_StackerPos(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer X 축, Stacker1 위치로 이동 시작");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Transfer_To_R_Port");

            //  Target Position 변경 : Stacker0 위치
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_X] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_TR_LPortPos].LD_Transfer_X;

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_X].Common_Speed_Coarse;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_X].Common_Acceleration_Coarse;

            MC_Func.MC_MovePosition((int)nAxis.TR_X,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_X],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_Stacker1PickUp_TransferZ_Move_ReadyPos(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 시작");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Transfer_To_L_Port");

            //  Target Position 변경 : 대기 위치
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_TR_SafetyPos].LD_Transfer_Z;

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Coarse;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Coarse;

            MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_Stacker0PickUp_TransferZ_Move_ReadyPos2_2ndStep(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 시작. (2단계, 최종 위치)");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Transfer_To_R_Port");

            //  Target Position 변경 : 대기 위치 2단계 (최종 위치)
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_TR_SafetyPos].LD_Transfer_Z;

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Coarse;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Coarse;

            MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_Stacker0PickUp_TransferZ_Move_ReadyPos2_1stStep(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 시작. (1단계, 현재 위치에서 10mm 위)");

            double m_dDownDistance = 0.0;

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Transfer_To_R_Port");

            //  Target Position 변경 : 대기 위치 1단계
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_TR_RPortPos].LD_Transfer_Z + 10.0;
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0] = MC_Func.MC_GetEncPos((int)nAxis.Z0);

            //  LoaderZ 축을 올리면서 Stacker 축을 내릴 경우
            if (Equipment.Machine_LoaderStacker_Down_afterLoaderPickUp_Enable)
            {
                m_dDownDistance = Equipment.Machine_LoaderStacker_DownDistance_afterLoaderPickUp < 0.0 ? 5.0 : Equipment.Machine_LoaderStacker_DownDistance_afterLoaderPickUp;

                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0] -= m_dDownDistance;
            }

            //  Dry Run 모드이면 10mm 더 위로
            if (workStage.m_bMainWorkCycle_DryRun)
            {
                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] += 10.0;

                if (loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] > 0)
                {
                    loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = 0;
                }
            }

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Fine;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Fine;

            MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            MC_Func.MC_MovePosition((int)nAxis.Z0,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.Z0],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }


        private void Loader_Transfer_Step_StackerPickUp_TransferZ_VibrationMove_DownPos(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 바이브레이션 이동 시작. (1단계, 현재 위치에서 2mm 아래)");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Transfer_To_R_Port");

            //  Target Position 변경 : 대기 위치 1단계
            if ((Equipment.Machine_LoaderTransfer_Vibration_MoveDistance < 0.0) || (Equipment.Machine_LoaderTransfer_Vibration_MoveDistance > 10.0))
            {
                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = MC_Func.MC_GetEncPos((int)nAxis.TR_Z) - 2.0;
            }
            else
            {
                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = MC_Func.MC_GetEncPos((int)nAxis.TR_Z) - Equipment.Machine_LoaderTransfer_Vibration_MoveDistance;
            }            

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Coarse * 2.0;

            //  가감속
            if (Equipment.Machine_LoaderTransfer_Vibration_AccDecSpeed_Ratio <= 0.0)
            {
                m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Coarse * 2.0;
            }
            else
            {
                m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Coarse * Equipment.Machine_LoaderTransfer_Vibration_AccDecSpeed_Ratio;
            }            

            MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_StackerPickUp_TransferZ_VibrationMove_UpPos(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 바이브레이션 이동 시작. (1단계, 현재 위치에서 2mm 위)");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Transfer_To_R_Port");

            //  Target Position 변경 : 대기 위치 1단계
            if ((Equipment.Machine_LoaderTransfer_Vibration_MoveDistance < 0.0) || (Equipment.Machine_LoaderTransfer_Vibration_MoveDistance > 10.0))
            {
                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = MC_Func.MC_GetEncPos((int)nAxis.TR_Z) + 2.0;
            }
            else
            {
                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = MC_Func.MC_GetEncPos((int)nAxis.TR_Z) + Equipment.Machine_LoaderTransfer_Vibration_MoveDistance;
            }

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Coarse * 2.0;

            //  가감속
            if (Equipment.Machine_LoaderTransfer_Vibration_AccDecSpeed_Ratio <= 0.0)
            {
                m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Coarse * 2.0;
            }
            else
            {
                m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Coarse * Equipment.Machine_LoaderTransfer_Vibration_AccDecSpeed_Ratio;
            }

            MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }


        private void Loader_Transfer_Step_Stacker0PickUp_TransferZ_Move_PickUpPos_2ndStep(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, Module Pickup 위치로 이동 시작.");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Transfer_To_R_Port");

            //  Target Position 변경 : Module Pickup 위치
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_TR_RPortPos].LD_Transfer_Z;

            //  Dry Run 모드이면 10mm 더 위로
            if (workStage.m_bMainWorkCycle_DryRun)
            {
                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] += 10.0;

                if (loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] > 0)
                {
                    loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = 0;
                }
            }

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Fine;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Fine;

            MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_Stacker0PickUp_TransferZ_Move_PickUpPos_1stStep(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, Module Pickup 대기 위치로 이동 시작. (10mm 위)");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Transfer_To_R_Port");

            //  Target Position 변경 : Module Pickup 대기 위치
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_TR_RPortPos].LD_Transfer_Z + 10.0;

            //  Dry Run 모드이면 10mm 더 위로
            if (workStage.m_bMainWorkCycle_DryRun)
            {
                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] += 10.0;

                if (loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] > 0)
                {
                    loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = 0;
                }
            }

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Coarse;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Coarse;

            MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_Stacker0PickUp_TransferX_Move_StackerPos(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer X 축, Stacker0 위치로 이동 시작");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Transfer_To_R_Port");

            //  Target Position 변경 : Stacker0 위치
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_X] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_TR_RPortPos].LD_Transfer_X;

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_X].Common_Speed_Coarse;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_X].Common_Acceleration_Coarse;

            MC_Func.MC_MovePosition((int)nAxis.TR_X,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_X],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_Stacker0PickUp_TransferZ_Move_ReadyPos(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 시작");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Transfer_To_R_Port");

            //  Target Position 변경 : 대기 위치
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_TR_SafetyPos].LD_Transfer_Z;

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Coarse;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Coarse;

            MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_TransferX_Move_ReadyPos(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer X 축, 대기 위치로 이동 시작");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Transfer_To_R_Port");

            //  Target Position 변경 : Stacker0 위치
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_X] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_TR_SafetyPos].LD_Transfer_X;

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_X].Common_Speed_Coarse;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_X].Common_Acceleration_Coarse;

            MC_Func.MC_MovePosition((int)nAxis.TR_X,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_X],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        private void Loader_Transfer_Step_TransferZ_Move_ReadyPos(out double m_dSpeed, out double m_dAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "LD Transfer Cycle", "Transfer Z 축, 대기 위치로 이동 시작");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Transfer_To_R_Port");

            //  Target Position 변경 : 대기 위치
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z] = stLDULTeachingPos[(int)LDUL_TeachingPosList.LD_TR_SafetyPos].LD_Transfer_Z;

            //  속도
            m_dSpeed = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Speed_Coarse;

            //  가감속
            m_dAccDec = Equipment.stAxisParam[(int)nAxis.TR_Z].Common_Acceleration_Coarse;

            MC_Func.MC_MovePosition((int)nAxis.TR_Z,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.TR_Z],
                                m_dSpeed,
                                m_dAccDec,
                                m_dAccDec);

            TickCount_Start((int)TickType.TICK_LDTR);
        }

        #endregion


        #region M-Align Cycle Function

        int  Run_MAlign_Cycle_Func()
        {
            int ret = 0;
            int nNextStep = 0;
            bool m_bRet = false;
            string m_strTemp;

            double m_dSpeed_Align_Fast = 0.0;
            double m_dSpeed_Align_Slow = 0.0;
            double m_dSpeed_Align_MoreSlow = 0.0;
            double m_dSpeedMag_forAccDec = 0.0;


            //  운전 중 Door 를 열면 장비 Stop
            if (m_nMAlign_Step >= (int)MAlign_Step.Start)
            {   
            }

            switch (m_nMAlign_Step)
            {
                case (int)MAlign_Step.Start:
                    MAlign_Step_Start();

                    m_nMAlign_Step = (int)MAlign_Step.Process_Condition_Check;
                    break;


                case (int)MAlign_Step.Process_Condition_Check:                                                                          //  동작 조건 체크 (Transfer Cycle : None, Transfer 가 Module 을 갖다 놓았는지 확인하는 Flag : True, Module Size > 0)
                    Log.Write("SLD-200", Equipment.User_Name, "M-Align Cycle", "동작 조건 확인");

                    if (!((m_nLoader_Transfer_Step >= (int)Loader_Transfer_Step.MAlignerPutDown_MAlign_Start) &&
                        (m_nLoader_Transfer_Step <= (int)Loader_Transfer_Step.MAlignerPutDown_MAlign_CompleteCheck)) &&

                        !((m_nLoader_Transfer_Step >= (int)Loader_Transfer_Step.MAligner_Retry_MAlign_Start) &&
                        (m_nLoader_Transfer_Step <= (int)Loader_Transfer_Step.MAligner_Retry_MAlign_CompleteCheck)) &&

                        !((m_nLoader_Transfer_Step >= (int)Loader_Transfer_Step.MAligner_MAlign_Start) &&
                        (m_nLoader_Transfer_Step <= (int)Loader_Transfer_Step.MAligner_MAlign_CompleteCheck)) &&

                        (m_nLoader_Transfer_Step > (int)Loader_Transfer_Step.None))                                                       //  Transfer Cycle 이 동작중
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "M-Align Cycle", "Transfer 가 동작중이므로 M-Align 동작 중지.");

                        //  일단 Out. (Transfer 동작이 완료되면 진행하도록 대기할 것인지는 테스트 하면서 결정하기로 함)
                       // m_nMAlign_Step = (int)MAlign_Step.None;
                    }
                    //else if (((m_nLoader_Transfer_Step < (int)Loader_Transfer_Step.MAlignerPutDown_MAlign_Start) ||
                    //        (m_nLoader_Transfer_Step > (int)Loader_Transfer_Step.MAlignerPutDown_MAlign_CompleteCheck)) &&

                    //        ((m_dMAlign_ModuleSize_Width <= 0.0) || (m_dMAlign_ModuleSize_Height <= 0.0)))
                    else if ((m_dMAlign_ModuleSize_Width <= 0.0) || (m_dMAlign_ModuleSize_Height <= 0.0))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "M-Align Cycle", "Module Size 가 입력되지 않았으므로 동작 중지.");

                        //return AlarmPost(AlarmKey.LD_Maligner_Not_Set_Module_Size);
                        m_nMAlign_Step = (int)MAlign_Step.None;
                    }
                    else
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "M-Align Cycle", "M-Align 동작 조건 OK");

                        m_nMAlign_Step = (int)MAlign_Step.MAligner_MoveXY_Widely;
                    }
                    break;


                case (int)MAlign_Step.MAligner_MoveXY_Widely:                                       //  MAligner XY 축, 넓힘.

                    MAlign_Step_MAligner_MoveXY_Widely(out m_dSpeed_Align_Fast, out m_dSpeedMag_forAccDec);

                    m_nMAlign_Step = (int)MAlign_Step.MAligner_MoveXY_Widely_DoneCheck;
                    break;


                case (int)MAlign_Step.MAligner_MoveXY_Widely_DoneCheck:                             //  MAligner XY 축, 넓힘 완료 확인.
                    {
                        ret = MAlign_Step_MAligner_MoveXY_Widely_DoneCheck(ref nNextStep);
                        if (ret != 0)
                        {
                            return ret;
                        }
                        if(nNextStep != 0)
                        {
                            m_nMAlign_Step = nNextStep;
                        }
                    }

                    break;


                case (int)MAlign_Step.MAligner_ModuleVacuum_On:                                     //  Module Vacuum On

                    MAlign_Step_MAligner_ModuleVacuum_On();

                    m_nMAlign_Step = (int)MAlign_Step.MAligner_ModuleVacuum_OnCheck;
                    break;


                case (int)MAlign_Step.MAligner_ModuleVacuum_OnCheck:                                //  Module Vacuum On 확인
                    bool bVaccum = (Equipment.Machine_VacuumSensor_Enable && (loaderParameter.DI_Loader_Aligner_VacuumCheck((int)LoaderParameter.MAlignerVacuumPos.Center) ||
                                                                            loaderParameter.DI_Loader_Aligner_VacuumCheck((int)LoaderParameter.MAlignerVacuumPos.Inner) ||
                                                                            loaderParameter.DI_Loader_Aligner_VacuumCheck((int)LoaderParameter.MAlignerVacuumPos.Outer)));
                    bool bMAlingerCenter = ((Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Center && loaderParameter.DI_Loader_Aligner_VacuumCheck((int)LoaderParameter.MAlignerVacuumPos.Center)) ||
                            !Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Center);

                    //bool bMAlingerInner = ((Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Inner && loaderParameter.DI_Loader_Aligner_VacuumCheck((int)LoaderParameter.MAlignerVacuumPos.Inner)) ||
                    //       !Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Inner);
                    //  임시 코드
                    bool bMAlingerInner = ((Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Inner && (loaderParameter.DI_Loader_Aligner_VacuumCheck((int)LoaderParameter.MAlignerVacuumPos.Inner) ||
                                                                                                        loaderParameter.DI_Loader_Aligner_VacuumCheck((int)LoaderParameter.MAlignerVacuumPos.Center))) ||
                           !Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Inner);

                    bool bMAlingerOuter = ((Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Outer && loaderParameter.DI_Loader_Aligner_VacuumCheck((int)LoaderParameter.MAlignerVacuumPos.Outer)) ||
                           !Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Outer);
                    bool bMAlignerStableTime = (!Equipment.Machine_VacuumStableTime_Enable ||
                    (Equipment.Machine_VacuumStableTime_Enable && (TickCount_Elapsed((int)TickType.TICK_ALIGN) > Equipment.Machine_VacuumStableTime)));

                    bool bMAlignerSensorHold = (!Equipment.Machine_VacuumSensor_Enable && (TickCount_Elapsed((int)TickType.TICK_ALIGN) > Equipment.Machine_SignalHoldTime));

                    if (bVaccum && bMAlingerCenter && bMAlingerInner && bMAlingerOuter  && (bMAlignerStableTime || bMAlignerSensorHold))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "M-Align Cycle", "Module Vacuum On 완료");

                        m_nMAlign_Step = (int)MAlign_Step.MAligner_MoveXY_Narrowly;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ALIGN) > 6000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "M-Align Cycle", "Module Vacuum On 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        return AlarmPost(AlarmKey.MAligner_VacuumOn_Fail);

                        m_nMAlign_Step = (int)MAlign_Step.None;

                        MessageBox.Show("Module Vacuum On 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)MAlign_Step.MAligner_MoveXY_Narrowly:                                       //  MAligner XY 축, 좁힘.

                    MAlign_Step_MAligner_MoveXY_Narrowly(out m_dSpeed_Align_Slow, out m_dSpeedMag_forAccDec);

                    m_nMAlign_Step = (int)MAlign_Step.MAligner_MoveXY_Narrowly_DoneCheck;
                    break;


                case (int)MAlign_Step.MAligner_MoveXY_Narrowly_DoneCheck:                             //  MAligner XY 축, 좁힘 완료 확인.

                    if (MC_Func.MC_GetDone((int)nAxis.ALN_X) && MC_Func.MC_PosTolerance((int)nAxis.ALN_X, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_X]) &&
                        MC_Func.MC_GetDone((int)nAxis.ALN_Y) && MC_Func.MC_PosTolerance((int)nAxis.ALN_Y, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_Y]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "M-Align Cycle", "M-Aligner Module Size 보다 1.0 mm 더 좁게 Close 완료");

                        m_nMAlign_Step = (int)MAlign_Step.MAligner_MoveXY_LittleWidely;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ALIGN) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "M-Align Cycle", "M-Aligner Module Size 보다 1.0 mm 더 좁게 Close 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        return AlarmPost(AlarmKey.MAligner_MoveXY_Narrowly_Fail);

                        m_nMAlign_Step = (int)MAlign_Step.None;

                        MessageBox.Show("M-Aligner Module Size 보다 1.0 mm 더 좁게 Close 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)MAlign_Step.MAligner_MoveXY_LittleWidely:                                       //  MAligner XY 축, 약간 넓힘.

                    MAlign_Step_MAligner_MoveXY_LittleWidely(out m_dSpeed_Align_MoreSlow, out m_dSpeedMag_forAccDec);

                    m_nMAlign_Step = (int)MAlign_Step.MAligner_MoveXY_LittleWidely_DoneCheck;
                    break;


                case (int)MAlign_Step.MAligner_MoveXY_LittleWidely_DoneCheck:                             //  MAligner XY 축, 약간 넓힘 완료 확인.

                    if (MC_Func.MC_GetDone((int)nAxis.ALN_X) && MC_Func.MC_PosTolerance((int)nAxis.ALN_X, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_X]) &&
                        MC_Func.MC_GetDone((int)nAxis.ALN_Y) && MC_Func.MC_PosTolerance((int)nAxis.ALN_Y, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_Y]))
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "M-Align Cycle", "계산된 M-Aligner Module Size 위치로 이동 완료");

                        m_nMAlign_Step = (int)MAlign_Step.Complete;
                    }
                    else if (TickCount_Elapsed((int)TickType.TICK_ALIGN) > 60000)
                    {
                        Log.Write("SLD-200", Equipment.User_Name, "M-Align Cycle", "계산된 M-Aligner Module Size 위치로 이동 실패. (Timeout)");

                        //  알람 정지 (LED Bar - Red Blink)
                        Equipment.MachineStop_byAlarm = true;

                        return AlarmPost(AlarmKey.MAligner_MoveXY_LittleWidely_Fail);

                        m_nMAlign_Step = (int)MAlign_Step.None;

                        MessageBox.Show("계산된 M-Aligner Module Size 위치로 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;


                case (int)MAlign_Step.Complete:

                    Log.Write("SLD-200", Equipment.User_Name, "M-Align Cycle", "완료");

                    m_bMAlign_Complete = true;
                    m_bMAlign_Retry = false;

                    //  m_bHomeOK = true;

                    //timer_Motion_Home.Enabled = false;
                    //m_btimer_Motion_Home_Stop = true;

                    m_strTemp = "===  M-Align Cycle 완료  ===";

                    m_nMAlign_Step = (int)MAlign_Step.None;


                    //  M-Align 중 알람이 발생했을 때, 알람 해제 후 다음 Step (M-Aligner 에서 Work Stage 로 모듈 이동) 을 진행하기 위해서 Flag 를 재설정함.
                    if (m_bMAlignZone_ModuleExist)                       //  M-Aligner 에 모듈을 내려놨었다면?
                    {
                        m_bLD_Transfer_toMAligner_Module_PutDown_Complete_Flag = true;                  //  이 Flag 를 true 로 해 줘야 Work Stage 로 모듈을 이동할 수 있음.
                    }


                    //  Seq. Test 일 경우
                    if (!Equipment.AutoRunStatus && Equipment.SeqTestMode)
                    {
                        Equipment.SeqTestMode = false;

                        MessageBox.Show(m_strTemp, "Information!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;
            }
            return 0;

        }

        private void MAlign_Step_MAligner_MoveXY_LittleWidely(out double m_dSpeed_Align_MoreSlow, out double m_dSpeedMag_forAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "M-Align Cycle", "계산된 M-Aligner Module Size 위치로 이동 시작");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Stacker0_Top");

            //  Target Position 변경 : 입력한 자재 크기로 변경
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_X] = m_dMAlign_CalculatedModuleSize_ALN_X;
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_Y] = m_dMAlign_CalculatedModuleSize_ALN_Y;

            //  속도
            m_dSpeed_Align_MoreSlow = Equipment.stAxisParam[(int)nAxis.ALN_X].Common_Speed_Fine / 2.0;

            //  가감속 배율
            m_dSpeedMag_forAccDec = 2.0;

            MC_Func.MC_MovePosition((int)nAxis.ALN_X,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_X],
                                m_dSpeed_Align_MoreSlow,
                                m_dSpeed_Align_MoreSlow * m_dSpeedMag_forAccDec,
                                m_dSpeed_Align_MoreSlow * m_dSpeedMag_forAccDec);

            MC_Func.MC_MovePosition((int)nAxis.ALN_Y,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_Y],
                                m_dSpeed_Align_MoreSlow,
                                m_dSpeed_Align_MoreSlow * m_dSpeedMag_forAccDec,
                                m_dSpeed_Align_MoreSlow * m_dSpeedMag_forAccDec);

            TickCount_Start((int)TickType.TICK_ALIGN);
        }

        private void MAlign_Step_MAligner_MoveXY_Narrowly(out double m_dSpeed_Align_Slow, out double m_dSpeedMag_forAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "M-Align Cycle", "M-Aligner 계산된 Module Size 보다 1.0 mm 더 좁게 Close");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Stacker0_Top");

            //  Target Position 변경 : 입력한 자재 크기에 해당하는 위치로 변경
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_X] = m_dMAlign_CalculatedModuleSize_ALN_X;
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_Y] = m_dMAlign_CalculatedModuleSize_ALN_Y;

            //  Target Position 변경 : 입력한 자재 크기보다 1.0 mm 더 작게 변경
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_X] -= Equipment.Machine_MAligner_NarrowingDistance;
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_Y] -= Equipment.Machine_MAligner_NarrowingDistance;

            //  속도
            m_dSpeed_Align_Slow = Equipment.stAxisParam[(int)nAxis.ALN_X].Common_Speed_Fine / 2.0;

            //  가감속 배율
            m_dSpeedMag_forAccDec = 2.0;

            MC_Func.MC_MovePosition((int)nAxis.ALN_X,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_X],
                                m_dSpeed_Align_Slow,
                                m_dSpeed_Align_Slow * m_dSpeedMag_forAccDec,
                                m_dSpeed_Align_Slow * m_dSpeedMag_forAccDec);

            MC_Func.MC_MovePosition((int)nAxis.ALN_Y,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_Y],
                                m_dSpeed_Align_Slow,
                                m_dSpeed_Align_Slow * m_dSpeedMag_forAccDec,
                                m_dSpeed_Align_Slow * m_dSpeedMag_forAccDec);

            TickCount_Start((int)TickType.TICK_ALIGN);
        }

        private void MAlign_Step_MAligner_ModuleVacuum_On()
        {
            Log.Write("SLD-200", Equipment.User_Name, "M-Align Cycle", "Module Vacuum On");

            if (Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Center)
            {
                loaderParameter.DO_Loader_Aligner_Vacuum((int)LoaderParameter.MAlignerVacuumPos.Center, true);
            }

            if (Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Inner)
            {
                loaderParameter.DO_Loader_Aligner_Vacuum((int)LoaderParameter.MAlignerVacuumPos.Inner, true);
            }

            if (Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Outer)
            {
                loaderParameter.DO_Loader_Aligner_Vacuum((int)LoaderParameter.MAlignerVacuumPos.Outer, true);
            }
            TickCount_Start((int)TickType.TICK_ALIGN);
        }

        private int MAlign_Step_MAligner_MoveXY_Widely_DoneCheck(ref int nNextStep)
        {
            int ret = 0;
            if (MC_Func.MC_GetDone((int)nAxis.ALN_X) && MC_Func.MC_PosTolerance((int)nAxis.ALN_X, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_X]) &&
            MC_Func.MC_GetDone((int)nAxis.ALN_Y) && MC_Func.MC_PosTolerance((int)nAxis.ALN_Y, loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_Y]))
            {
                Log.Write("SLD-200", Equipment.User_Name, "M-Align Cycle", "M-Aligner Module Size 보다 20.0 mm 더 넓게 Open 완료");

                nNextStep = (int)MAlign_Step.MAligner_ModuleVacuum_On;
                ret = 0;
            }
            else if (TickCount_Elapsed((int)TickType.TICK_ALIGN) > 60000)
            {
                Log.Write("SLD-200", Equipment.User_Name, "M-Align Cycle", "M-Aligner Module Size 보다 10.0 mm 더 넓게 Open 이동 실패. (Timeout)");

                //  알람 정지 (LED Bar - Red Blink)
                Equipment.MachineStop_byAlarm = true;
                return AlarmPost(AlarmKey.MAligner_MoveXY_Widely_DoneCheck_Timeout);
                nNextStep = (int)MAlign_Step.None;
                ret = -1;
                MessageBox.Show("M-Aligner Module Size 보다 10.0 mm 더 넓게 Open 이동 실패", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return ret;
        }

        //TEST 위해서 publc으로 
        //protected int AlarmPost(AlarmKey AlarmCode)
        public int AlarmPost(AlarmKey AlarmCode)
        {
            Alarm alarm = GetAlarm((int)AlarmCode);
            if (alarm.Grade.Equals("Error"))
            {
                this.m_LoaderWork_Start = false;
            }
            //MessageBox.Show(alarm.Cause);
            AlarmManager.Instance.ShowAlarm(alarm);
            return alarm.Code;
        }

        private void MAlign_Step_MAligner_MoveXY_Widely(out double m_dSpeed_Align_Fast, out double m_dSpeedMag_forAccDec)
        {
            Log.Write("SLD-200", Equipment.User_Name, "M-Align Cycle", "M-Aligner Module Size 보다 10.0 mm 더 넓게 Open");

            loaderParameter.stLoaderPosParam = loaderParameter.GetPositionInformation("Stacker0_Top");

            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_X] = stLDULTeachingPos[(int)LDUL_TeachingPosList.MAligner_Gap100mmPos].MAligner_X;
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_Y] = stLDULTeachingPos[(int)LDUL_TeachingPosList.MAligner_Gap100mmPos].MAligner_Y;

            //  Target Position 변경 : 입력한 자재 크기로 변경
            //  모듈 가로 사이즈 : m_dMAlign_ModuleSize_Width
            //  모듈 세로 사이즈 : m_dMAlign_ModuleSize_Height
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_X] += (m_dMAlign_ModuleSize_Width - 100.0);
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_Y] += (m_dMAlign_ModuleSize_Height - 100.0);

            //  계산된 Module Size 에 해당하는 ALN X, Y 축 위치 저장
            m_dMAlign_CalculatedModuleSize_ALN_X = loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_X];
            m_dMAlign_CalculatedModuleSize_ALN_Y = loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_Y];

            //  Target Position 변경 : 입력한 자재 크기보다 20.0 mm 더 크게 변경
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_X] += 20.0;
            loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_Y] += 20.0;

            //  Target Position 변경 : 입력한 자재 크기보다 20.0 mm 더 크게 변경한 위치가 최대 Open 위치를 넘어갈 경우, 최대 Open 위치로 변경
            if (loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_X] > stLDULTeachingPos[(int)LDUL_TeachingPosList.MAligner_OpenPos].MAligner_X)
            {
                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_X] = stLDULTeachingPos[(int)LDUL_TeachingPosList.MAligner_OpenPos].MAligner_X;
            }
            if (loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_Y] > stLDULTeachingPos[(int)LDUL_TeachingPosList.MAligner_OpenPos].MAligner_Y)
            {
                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_Y] = stLDULTeachingPos[(int)LDUL_TeachingPosList.MAligner_OpenPos].MAligner_Y;
            }

            //  속도
            m_dSpeed_Align_Fast = Equipment.stAxisParam[(int)nAxis.ALN_X].Common_Speed_Coarse;

            //  가감속 배율
            m_dSpeedMag_forAccDec = 2.0;

            MC_Func.MC_MovePosition((int)nAxis.ALN_X,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_X],
                                m_dSpeed_Align_Fast,
                                m_dSpeed_Align_Fast * m_dSpeedMag_forAccDec,
                                m_dSpeed_Align_Fast * m_dSpeedMag_forAccDec);

            MC_Func.MC_MovePosition((int)nAxis.ALN_Y,
                                loaderParameter.stLoaderPosParam.dTarget[(int)LoaderParameter.MotionKey.ALN_Y],
                                m_dSpeed_Align_Fast,
                                m_dSpeed_Align_Fast * m_dSpeedMag_forAccDec,
                                m_dSpeed_Align_Fast * m_dSpeedMag_forAccDec);

            TickCount_Start((int)TickType.TICK_ALIGN);
        }

        private void MAlign_Step_Start()
        {
            Log.Write("SLD-200", Equipment.User_Name, "M-Align Cycle", "시작");

            Equipment.MachineStop_byAlarm = false;

            //  M-Aligner 축 모터 전체 Stop
            MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.ALN_X, 2000);
            MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.ALN_Y, 2000);

            m_dMAlign_CalculatedModuleSize_ALN_X = 0.0;
            m_dMAlign_CalculatedModuleSize_ALN_Y = 0.0;

            if (!Equipment.SeqTestMode)
            {
                m_dMAlign_ModuleSize_Width = Equipment.stLayerRecipeSet[0].ModuleInformation_Module_Width;
                m_dMAlign_ModuleSize_Height = Equipment.stLayerRecipeSet[0].ModuleInformation_Module_Height;
            }
        }

        #endregion



        #region Teaching Position List Save / Load

        public bool Teaching_Position_Load()
        {
            string strTemp = "";

            bool m_bRet = true;
            string strFIle = "";
            StringBuilder temp = new StringBuilder(255);

            strFIle = ConfigManager.GetTeachingDataPath() + "\\LDUL_TeachingPosition.ini";

            if (File.Exists(strFIle) == false)
            {
                MessageBox.Show("LDUL Teaching Position 파일이 없습니다.\r\n\r\n[Default 값으로 설정됩니다.]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return false;
            }

            //  Position 데이터 로드
            for (int i = 0; i < System.Enum.GetValues(typeof(LDUL_TeachingPosList)).Length; i++)
            {
                strTemp = string.Format("PosIndex_{0}", i);

                //  LD Transfer X
                NativeMethods.GetPrivateProfileString(strTemp, "LDTransferX", "0", temp, 255, strFIle);
                stLDULTeachingPos[i].LD_Transfer_X = Equipment.ToDouble(temp.ToString());
                //  LD Transfer Z
                NativeMethods.GetPrivateProfileString(strTemp, "LDTransferZ", "0", temp, 255, strFIle);
                stLDULTeachingPos[i].LD_Transfer_Z = Equipment.ToDouble(temp.ToString());
                //  LD Stacker Z0
                NativeMethods.GetPrivateProfileString(strTemp, "LDStackerZ0", "0", temp, 255, strFIle);
                stLDULTeachingPos[i].LD_Stacker_Z0 = Equipment.ToDouble(temp.ToString());
                //  LD Stacker Z1
                NativeMethods.GetPrivateProfileString(strTemp, "LDStackerZ1", "0", temp, 255, strFIle);
                stLDULTeachingPos[i].LD_Stacker_Z1 = Equipment.ToDouble(temp.ToString());
                //  M-Aligner X
                NativeMethods.GetPrivateProfileString(strTemp, "MAlignerX", "0", temp, 255, strFIle);
                stLDULTeachingPos[i].MAligner_X = Equipment.ToDouble(temp.ToString());
                //  M-Aligner Y
                NativeMethods.GetPrivateProfileString(strTemp, "MAlignerY", "0", temp, 255, strFIle);
                stLDULTeachingPos[i].MAligner_Y = Equipment.ToDouble(temp.ToString());
                //  UL Transfer X
                NativeMethods.GetPrivateProfileString(strTemp, "ULTransferX", "0", temp, 255, strFIle);
                stLDULTeachingPos[i].UL_Transfer_X = Equipment.ToDouble(temp.ToString());
                //  UL Transfer Z
                NativeMethods.GetPrivateProfileString(strTemp, "ULTransferZ", "0", temp, 255, strFIle);
                stLDULTeachingPos[i].UL_Transfer_Z = Equipment.ToDouble(temp.ToString());
                //  UL Stacker Z0
                NativeMethods.GetPrivateProfileString(strTemp, "ULStackerZ0", "0", temp, 255, strFIle);
                stLDULTeachingPos[i].UL_Stacker_Z0 = Equipment.ToDouble(temp.ToString());
                //  UL Stacker Z1
                NativeMethods.GetPrivateProfileString(strTemp, "ULStackerZ1", "0", temp, 255, strFIle);
                stLDULTeachingPos[i].UL_Stacker_Z1 = Equipment.ToDouble(temp.ToString());
            }

            return m_bRet;
        }

        public void Teaching_Position_Save()
        {
            string strTemp = "";

            string strFIle = "";
            strFIle = ConfigManager.GetTeachingDataPath() + "\\LDUL_TeachingPosition.ini";

            if (File.Exists(strFIle) == false)
            {
                File.Create(strFIle);

                MessageBox.Show("LDUL Teaching Position 파일을 생성하였습니다. 다시 시도하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //  Position Parameter 저장
            for (int i = 0; i < System.Enum.GetValues(typeof(LDUL_TeachingPosList)).Length; i++)
            {
                strTemp = string.Format("PosIndex_{0}", i);

                //  LD Transfer X
                NativeMethods.WritePrivateProfileString(strTemp, "LDTransferX", stLDULTeachingPos[i].LD_Transfer_X.ToString(), strFIle);
                //  LD Transfer Z
                NativeMethods.WritePrivateProfileString(strTemp, "LDTransferZ", stLDULTeachingPos[i].LD_Transfer_Z.ToString(), strFIle);
                //  LD Stacker Z0
                NativeMethods.WritePrivateProfileString(strTemp, "LDStackerZ0", stLDULTeachingPos[i].LD_Stacker_Z0.ToString(), strFIle);
                //  LD Stacker Z1
                NativeMethods.WritePrivateProfileString(strTemp, "LDStackerZ1", stLDULTeachingPos[i].LD_Stacker_Z1.ToString(), strFIle);
                //  M-Aligner X
                NativeMethods.WritePrivateProfileString(strTemp, "MAlignerX", stLDULTeachingPos[i].MAligner_X.ToString(), strFIle);
                //  M-Aligner Y
                NativeMethods.WritePrivateProfileString(strTemp, "MAlignerY", stLDULTeachingPos[i].MAligner_Y.ToString(), strFIle);
                //  UL Transfer X
                NativeMethods.WritePrivateProfileString(strTemp, "ULTransferX", stLDULTeachingPos[i].UL_Transfer_X.ToString(), strFIle);
                //  UL Transfer Z
                NativeMethods.WritePrivateProfileString(strTemp, "ULTransferZ", stLDULTeachingPos[i].UL_Transfer_Z.ToString(), strFIle);
                //  UL Stacker Z0
                NativeMethods.WritePrivateProfileString(strTemp, "ULStackerZ0", stLDULTeachingPos[i].UL_Stacker_Z0.ToString(), strFIle);
                //  UL Stacker Z1
                NativeMethods.WritePrivateProfileString(strTemp, "ULStackerZ1", stLDULTeachingPos[i].UL_Stacker_Z1.ToString(), strFIle);
            }

            //MessageBox.Show("Teaching Position 을 저장하였습니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion


        #region Teaching Position Move Properties Save / Load

        public bool Move_Properties_Load()
        {
            string strTemp = "";

            bool m_bRet = true;
            string strFIle = "";
            StringBuilder temp = new StringBuilder(255);

            strFIle = ConfigManager.GetTeachingDataPath() + "\\LDUL_MoveProperties.ini";

            if (File.Exists(strFIle) == false)
            {
                MessageBox.Show("LDUL Move Properties 파일이 없습니다.\r\n\r\n[Default 값으로 설정됩니다.]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return false;
            }

            //  Position 데이터 로드
            for (int i = 0; i < System.Enum.GetValues(typeof(LDUL_TeachingPosList)).Length; i++)
            {
                strTemp = string.Format("PosIndex_{0}", i);

                //  Fine Accel
                NativeMethods.GetPrivateProfileString(strTemp, "Fine_Accel", "20", temp, 255, strFIle);
                stLDULPosMoveProperties[i].Fine_Accel = Equipment.ToInt(temp.ToString());
                //  Fine Settle Delay
                NativeMethods.GetPrivateProfileString(strTemp, "Fine_SettleDelay", "200", temp, 255, strFIle);
                stLDULPosMoveProperties[i].Fine_SettleDelay = Equipment.ToInt(temp.ToString());
                //  Coarse Accel
                NativeMethods.GetPrivateProfileString(strTemp, "Coarse_Accel", "200", temp, 255, strFIle);
                stLDULPosMoveProperties[i].Coarse_Accel = Equipment.ToInt(temp.ToString());
                //  Coarse Settle Delay
                NativeMethods.GetPrivateProfileString(strTemp, "Coarse_SettleDelay", "20", temp, 255, strFIle);
                stLDULPosMoveProperties[i].Coarse_SettleDelay = Equipment.ToInt(temp.ToString());
            }

            return m_bRet;
        }

        public void Move_Properties_Save()
        {
            string strTemp = "";

            string strFIle = "";
            strFIle = ConfigManager.GetTeachingDataPath() + "\\LDUL_MoveProperties.ini";

            if (File.Exists(strFIle) == false)
            {
                File.Create(strFIle);

                MessageBox.Show("LDUL Move Properties 파일을 생성하였습니다. 다시 시도하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //  Position Parameter 저장
            for (int i = 0; i < System.Enum.GetValues(typeof(LDUL_TeachingPosList)).Length; i++)
            {
                strTemp = string.Format("PosIndex_{0}", i);

                //  Fine Accel
                NativeMethods.WritePrivateProfileString(strTemp, "Fine_Accel", stLDULPosMoveProperties[i].Fine_Accel.ToString(), strFIle);
                //  Fine Settle Delay
                NativeMethods.WritePrivateProfileString(strTemp, "Fine_SettleDelay", stLDULPosMoveProperties[i].Fine_SettleDelay.ToString(), strFIle);
                //  Coarse Accel
                NativeMethods.WritePrivateProfileString(strTemp, "Coarse_Accel", stLDULPosMoveProperties[i].Coarse_Accel.ToString(), strFIle);
                //  Coarse Settle Delay
                NativeMethods.WritePrivateProfileString(strTemp, "Coarse_SettleDelay", stLDULPosMoveProperties[i].Coarse_SettleDelay.ToString(), strFIle);
            }

            //MessageBox.Show("Move Properties 를 저장하였습니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion


        #region Event Handler

        public bool m_LoaderWork_Start = false;
        public bool _isLoaderWorkRunning = false; // 중복 실행 방지 플래그
        private bool m_IsModuleClose = false;
        private int m_nStacker1_ModulePickupWaitingPos_Step_Recovery;
        private int m_nStacker0_ModulePickupWaitingPos_Step_Recovery;

        private void Timer_LoaderWork_Tick(object sender, ElapsedEventArgs e)
        {
            // 중복 실행 방지
            if (_isLoaderWorkRunning)
            {
                //Console.WriteLine("Scanner Calibration is already running. Skipping this call.");
                return;
            }

            try
            {
                _isLoaderWorkRunning = true;

                int ret = 0;
                //  자동운전 시, R-Port 동작 조건 : TR Cycle (None), R-Port Cycle (None), R-Port Module Pickup Complete
                ret = Run_Stacker0Module_PickupWaitingPos_Func();
                if (ret != 0)
                {
                    SetRecoveryStaker0(m_nStacker0_ModulePickupWaitingPos_Step);
                    return;
                }
                //  자동운전 시, L-Port 동작 조건 : TR Cycle (None), L-Port Cycle (None), L-Port Module Pickup Complete
                ret = Run_Stacker1Module_PickupWaitingPos_Func();
                if (ret != 0)
                {
                    SetRecoveryStaker1(m_nStacker1_ModulePickupWaitingPos_Step);
                    return;
                }

                ret = Run_MAlign_Cycle_Func();
                if (ret != 0)
                {
                    SetRecoveryMAlign_Cycle(m_nMAlign_Step);
                    return;
                }

                //ret = Run_Transfer_Cycle_Func();
                //if (ret != 0)
                //{
                //    SetRecoveryTransfer_Cycle(m_nLoader_Transfer_Step);
                //    return;
                //}

                //  홈 실행이 완료된 후 부터 Loader Ionizer 는 상시 체크
                if (workStage != null)
                {
                    if (workStage.m_bHomeOK)
                    {
                        if (loaderParameter.IsDO_Loader_Ionizer_On() &&

                        (!loaderParameter.DI_Loader_Ionizer_AlarmCheck((int)LoaderParameter.StackerTable.Stacker_0) ||
                        !loaderParameter.DI_Loader_Ionizer_AlarmCheck((int)LoaderParameter.StackerTable.Stacker_1)))
                        {
                            AlarmPost(AlarmKey.LD_Ionizer_Alarm);
                        }
                    }
                }

                // Scanner Calibration이 활성화되지 않은 경우 종료
                if (!m_LoaderWork_Start)
                {
                    //Console.WriteLine("LoaderWork is not started.");
                    return;  
                }

                // 현재 단계가 None이면 타이머 중지
                //if (m_nLoader_Transfer_Step == (int)Loader_Transfer_Step.None)
                //{
                //    Console.WriteLine("LoaderWork completed.");
                //    return;
                //}

                // 단계별 실행
                //Console.WriteLine($"LoaderWork running at {DateTime.Now}, Step: {m_nLoader_Transfer_Step}");
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //  메인 화면 갱신용 변수
                Equipment.m_bMainProcessStatus_LD_LPort_Complete = m_bStacker1_Complete;
                Equipment.m_bMainProcessStatus_LD_RPort_Complete = m_bStacker0_Complete;

                //  M-Aligner 에 Module 을 내려놓는 단계를 진행해야 하므로, Port 에서 Pick Up 이 완료된 것으로 본다.
                Equipment.m_bMainProcessStatus_LD_Module_PortPickUp_Complete = m_nLoaderTransfer_ProcessStep == (int)LoaderTransferProcessStep.LoaderStep_ModulePutDown_MAligner ? true : false;

                //  M-Aligner 에서 Module 을 집어올리는 단계를 진행해야 하므로, M-Aligner 에 Put Down 이 완료된 것으로 본다.
                Equipment.m_bMainProcessStatus_LD_Module_MAlignerPutDown_Complete = m_bMAlignZone_ModuleExist || (m_nLoaderTransfer_ProcessStep == (int)LoaderTransferProcessStep.LoaderStep_ModulePickUp_MAligner) ? true : false;

                //  M-Align 완료
                Equipment.m_bMainProcessStatus_LD_M_Aligner_Align_Complete = m_bMAlign_Complete;

                //  Work Stage 에 Module 을 내려놓는 단계를 진행해야 하므로, M-Aligner 에서 Pick Up 이 완료된 것으로 본다.
                Equipment.m_bMainProcessStatus_LD_Module_MAlignerPickUp_Complete = m_nLoaderTransfer_ProcessStep == (int)LoaderTransferProcessStep.LoaderStep_ModulePutDown_Stage ? true : false;

                //  Work Stage 에 Module 을 내려놓는  단계 완료
                Equipment.m_bMainProcessStatus_LD_Module_WorkStagePutDown_Complete = m_bAUTORUN_Loader_Transfer_ModulePutDowntoWorkStage_Complete && (m_nLoaderTransfer_ProcessStep == (int)LoaderTransferProcessStep.LoaderStep_ModulePickup_fromStacker) ? true : false;
                //  메인 화면 갱신용 변수
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                
                if (AlarmManager.Instance.IsAlarm)
                {
                    return;
                }
                //  자동운전 시, M-Align 동작 조건 : TR Cycle (None), M-Aligner Module Exist, M-Aligner Cycle (None)
                ret = Run_MAlign_Cycle_Func();
                if (ret != 0)
                {
                    SetRecoveryMAlign_Cycle(m_nMAlign_Step);
                    return;
                }
                ////  자동운전 시, M-Aligner 에서 Module Pick-Up 조건 : M-Aligner Cycle (None), TR Cycle (None), M-Aligner Module Exist, M-Aligner Complete
                //m_nLoaderTransferMoveType = (int)LoaderTransferMoveType.Cycle_MAligner_PickUp;
                ret = Run_Transfer_Cycle_Func();
                if (ret != 0)
                {
                    SetRecoveryTransfer_Cycle(m_nLoader_Transfer_Step);
                    return;
                }
                //Console.WriteLine($"LoaderWork Running at {DateTime.Now}, Step: {m_nLoader_Transfer_Step}");
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            finally
            {
                _isLoaderWorkRunning = false; // 플래그 해제
            }
        }
        public void SetRecovery()
        {
            SetRecoveryStaker0(m_nStacker0_ModulePickupWaitingPos_Step_Recovery);
            m_nStacker0_ModulePickupWaitingPos_Step = m_nStacker0_ModulePickupWaitingPos_Step_Recovery;
            SetRecoveryStaker1(m_nStacker1_ModulePickupWaitingPos_Step_Recovery);
            m_nStacker1_ModulePickupWaitingPos_Step = m_nStacker1_ModulePickupWaitingPos_Step_Recovery;
            SetRecoveryTransfer_Cycle(m_nLoader_Transfer_Step_Recovery);
            m_nLoader_Transfer_Step = m_nLoader_Transfer_Step_Recovery;
            SetRecoveryMAlign_Cycle(m_nMAlign_Step_Recovery);
            m_nMAlign_Step = m_nMAlign_Step_Recovery;
        }

        public void ResetRecovery()
        {
            m_nStacker0_ModulePickupWaitingPos_Step_Recovery = 0;
            m_nStacker1_ModulePickupWaitingPos_Step_Recovery = 0;
            m_nLoader_Transfer_Step_Recovery = 0;
            m_nMAlign_Step_Recovery = 0;

        }
        public void SetRecoveryStaker0(int Setp)
        {
            if (Setp != (int)StackerModulePickupWaitingPos_Step.None)
            {
                m_nStacker0_ModulePickupWaitingPos_Step_Recovery = (int)StackerModulePickupWaitingPos_Step.Start;
            }
            else
            {
                m_nStacker0_ModulePickupWaitingPos_Step_Recovery = (int)StackerModulePickupWaitingPos_Step.None;
            }
        }

        public void SetRecoveryStaker1(int Setp)
        {
            if (Setp != (int)StackerModulePickupWaitingPos_Step.None)
            {
                m_nStacker1_ModulePickupWaitingPos_Step_Recovery = (int)StackerModulePickupWaitingPos_Step.Start;
            }
            else
            {
                m_nStacker1_ModulePickupWaitingPos_Step_Recovery = (int)StackerModulePickupWaitingPos_Step.None;
            }
        }

        public void SetRecoveryTransfer_Cycle(int Step)
        {
            
            if (Step == (int)Loader_Transfer_Step.None)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.None;
            }
            else if(Step <= (int)Loader_Transfer_Step.Process_Type_Check)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.None;
            }else if(Step <= (int)Loader_Transfer_Step.Transfer_Move_Condition_Check)
            {
                m_nLoader_Transfer_Step_Recovery = Step;
            }
            else if (Step <= (int)Loader_Transfer_Step.TransferZ_Move_ReadyPos_DoneCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.TransferZ_Move_ReadyPos;
            }
            else if (Step <= (int)Loader_Transfer_Step.TransferX_Move_ReadyPos_DoneCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.TransferX_Move_ReadyPos;
            }
            else if (Step <= (int)Loader_Transfer_Step.Stacker0_ModulePickup_Condition_Check)
            {   
                m_nLoader_Transfer_Step_Recovery = Step;
            }
            else if (Step <= (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_Move_ReadyPos_DoneCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_Move_ReadyPos;
            }

            else if (Step <= (int)Loader_Transfer_Step.Stacker0PickUp_TransferX_Move_StackerPos_DoneCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.Stacker0PickUp_TransferX_Move_StackerPos;
            }

            else if (Step <= (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_Move_PickUpPos_1stStep_DoneCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_Move_PickUpPos_1stStep;
            }

            else if (Step <= (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_Move_PickUpPos_2ndStep_DoneCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_Move_PickUpPos_2ndStep;
            }
            else if (Step <= (int)Loader_Transfer_Step.Stacker0PickUp_Transfer_PickerVacuum_OnCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.Stacker0PickUp_Transfer_PickerVacuum_On;
            }
            else if (Step <= (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_Move_ReadyPos2_1stStep_DoneCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_Move_ReadyPos2_1stStep;
            }
            else if (Step <= (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.Stacker0PickUp_TransferZ_Move_ReadyPos2_2ndStep;
            }
            else if (Step <= (int)Loader_Transfer_Step.Stacker1_ModulePickup_Condition_Check)
            {
                m_bStacker1_Complete = false;
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.None;  // Step;
            }
            else if (Step <= (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_Move_ReadyPos_DoneCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_Move_ReadyPos;
            }
            else if (Step <= (int)Loader_Transfer_Step.Stacker1PickUp_TransferX_Move_StackerPos_DoneCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.Stacker1PickUp_TransferX_Move_StackerPos;
            }
            else if (Step <= (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_Move_PickUpPos_1stStep_DoneCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_Move_PickUpPos_1stStep;
            }
            else if (Step <= (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_Move_PickUpPos_2ndStep_DoneCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_Move_PickUpPos_2ndStep;
            }
            else if (Step <= (int)Loader_Transfer_Step.Stacker1PickUp_Transfer_PickerVacuum_OnCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.Stacker1PickUp_Transfer_PickerVacuum_On;
            }
            else if (Step <= (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_Move_ReadyPos2_1stStep_DoneCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_Move_ReadyPos2_1stStep;
            }
            else if (Step <= (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.Stacker1PickUp_TransferZ_Move_ReadyPos2_2ndStep;
            }
            else if (Step <= (int)Loader_Transfer_Step.MAligner_ModulePickup_Condition_Check)
            {
                m_nLoader_Transfer_Step_Recovery = Step;
            }
            else if (Step <= (int)Loader_Transfer_Step.MAligner_MAlign_NotComplete)
            {
                m_nLoader_Transfer_Step_Recovery = Step;
            }
            else if (Step <= (int)Loader_Transfer_Step.MAligner_MAlign_CompleteCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.MAligner_MAlign_Start;
            }

            else if (Step <= (int)Loader_Transfer_Step.MAlignerPickUp_TransferZ_Move_ReadyPos_DoneCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.MAlignerPickUp_TransferZ_Move_ReadyPos;
            }

            else if (Step <= (int)Loader_Transfer_Step.MAlignerPickUp_TransferX_Move_MAlignerPos_DoneCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.MAlignerPickUp_TransferX_Move_MAlignerPos;
            }
            else if (Step <= (int)Loader_Transfer_Step.MAlignerPickUp_Retry_Transfer_PickerVacuum_OffCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.MAlignerPickUp_Retry_Start;
            }
            else if (Step <= (int)Loader_Transfer_Step.MAlignerPickUp_Retry_TransferZ_Move_PickUpPos_1stStep_DoneCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.MAlignerPickUp_Retry_TransferZ_Move_PickUpPos_1stStep;
            }
            else if (Step <= (int)Loader_Transfer_Step.MAlignerPickUp_Retry_TransferZ_Move_PickUpPos_2ndStep_DoneCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.MAlignerPickUp_Retry_TransferZ_Move_PickUpPos_2ndStep;
            }
            else if (Step <= (int)Loader_Transfer_Step.MAlignerPickUp_Retry_MAligner_Vacuum_OnCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.MAlignerPickUp_Retry_MAligner_Vacuum_On;
            }
            else if (Step <= (int)Loader_Transfer_Step.MAlignerPickUp_Retry_TransferZ_Move_ReadyPos2_1stStep_DoneCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.MAlignerPickUp_Retry_TransferZ_Move_ReadyPos2_1stStep;
            }
            else if (Step <= (int)Loader_Transfer_Step.MAlignerPickUp_Retry_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.MAlignerPickUp_Retry_TransferZ_Move_ReadyPos2_2ndStep;
            }
            else if (Step <= (int)Loader_Transfer_Step.MAligner_Retry_MAlign_CompleteCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.MAligner_Retry_MAlign_Start;
            }
            else if (Step <= (int)Loader_Transfer_Step.MAlignerPickUp_Retry_Complete)
            {
                m_nLoader_Transfer_Step_Recovery = Step;
            }
            else if (Step <= (int)Loader_Transfer_Step.MAlignerPickUp_TransferZ_Move_PickUpPos_1stStep_DoneCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.MAlignerPickUp_TransferZ_Move_PickUpPos_1stStep;
            }
            else if (Step <= (int)Loader_Transfer_Step.MAlignerPickUp_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.MAlignerPickUp_TransferZ_Move_PickUpPos_1stStep;
            }
            else if (Step <= (int)Loader_Transfer_Step.WorkStage_ModulePutdown_Condition_Check)
            {
                m_nLoader_Transfer_Step_Recovery = Step;
            }
            else if (Step <= (int)Loader_Transfer_Step.MAlignerPickUp_TransferZ_Move_ReadyPos_DoneCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.MAlignerPickUp_TransferZ_Move_ReadyPos;
            }

            else if (Step <= (int)Loader_Transfer_Step.MAlignerPickUp_TransferX_Move_MAlignerPos_DoneCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.MAlignerPickUp_TransferX_Move_MAlignerPos;
            }

            else if (Step <= (int)Loader_Transfer_Step.WorkStagePutDown_WorkStageCycle_LoadingPos_CompleteCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.WorkStagePutDown_WorkStageCycle_LoadingPos_Start;
            }
            else if (Step <= (int)Loader_Transfer_Step.WorkStagePutDown_TransferX_Move_LoadingPos_DoneCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.WorkStagePutDown_TransferX_Move_LoadingPos;
            }
            else if (Step <= (int)Loader_Transfer_Step.WorkStagePutDown_TransferZ_Move_PutDownPos_1stStep_DoneCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.WorkStagePutDown_TransferZ_Move_PutDownPos_1stStep;
            }
            else if (Step <= (int)Loader_Transfer_Step.WorkStagePutDown_TransferZ_Move_PutDownPos_2ndStep_DoneCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.WorkStagePutDown_TransferZ_Move_PutDownPos_2ndStep;
            }

            else if (Step <= (int)Loader_Transfer_Step.WorkStagePutDown_Transfer_PickerVacuum_OffCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.WorkStagePutDown_WorkStage_Vacuum_On;
            }

            else if (Step <= (int)Loader_Transfer_Step.WorkStagePutDown_TransferZ_Move_ReadyPos2_1stStep_DoneCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.WorkStagePutDown_TransferZ_Move_ReadyPos2_1stStep;
            }
            else if (Step <= (int)Loader_Transfer_Step.WorkStagePutDown_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.WorkStagePutDown_TransferZ_Move_ReadyPos2_2ndStep;
            }
            //else if (Step <= (int)Loader_Transfer_Step.WorkStagePutDown_WorkStage_VacuumCheck)
            //{
            //    m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.WorkStagePutDown_WorkStage_Vacuum_On;
            //}
            else if (Step <= (int)Loader_Transfer_Step.MAligner_ModulePutdown_Condition_Check)
            {
                m_nLoader_Transfer_Step_Recovery = Step;
            }
            else if (Step <= (int)Loader_Transfer_Step.MAlignerPutDown_TransferZ_Move_ReadyPos_DoneCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.MAlignerPutDown_TransferZ_Move_ReadyPos;
            }
            else if (Step <= (int)Loader_Transfer_Step.MAlignerPutDown_MAlignerXY_Move_Widely_DoneCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.MAlignerPutDown_MAlignerXY_Move_Widely;
            }

            else if (Step <= (int)Loader_Transfer_Step.MAlignerPutDown_TransferX_Move_MAlignPos_DoneCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.MAlignerPutDown_TransferX_Move_MAlignPos;
            }
            else if (Step <= (int)Loader_Transfer_Step.MAlignerPutDown_TransferZ_Move_PutDownPos_1stStep_DoneCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.MAlignerPutDown_TransferZ_Move_PutDownPos_1stStep;
            }
            else if (Step <= (int)Loader_Transfer_Step.MAlignerPutDown_TransferZ_Move_PutDownPos_2ndStep_DoneCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.MAlignerPutDown_TransferZ_Move_PutDownPos_2ndStep;
            }
            else if (Step <= (int)Loader_Transfer_Step.MAlignerPutDown_Transfer_PickerVacuum_OffCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.MAlignerPutDown_MAligner_Vacuum_On;
            }
            else if (Step <= (int)Loader_Transfer_Step.MAlignerPutDown_TransferZ_Move_ReadyPos2_1stStep_DoneCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.MAlignerPutDown_TransferZ_Move_ReadyPos2_1stStep;
            }
            else if (Step <= (int)Loader_Transfer_Step.MAlignerPutDown_TransferZ_Move_ReadyPos2_2ndStep_DoneCheck)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.MAlignerPutDown_TransferZ_Move_ReadyPos2_2ndStep;
            }
            else if (Step <= (int)Loader_Transfer_Step.MAlignerPutDown_PickerVacuum_MAlignerVacuum_Check)
            {
                m_nLoader_Transfer_Step_Recovery = (int)Loader_Transfer_Step.MAlignerPutDown_MAligner_Vacuum_On;
            }
            else
            {
                m_nLoader_Transfer_Step_Recovery = Step;
            }
        }
        private void SetRecoveryMAlign_Cycle(int Step)
        {
            if(Step ==(int) MAlign_Step.None)
            {
                m_nMAlign_Step_Recovery = (int)MAlign_Step.None;
            }
            else
            {
                m_nMAlign_Step_Recovery = (int)MAlign_Step.Start;

            }
        }

        private void Timer_LoaderWork_Func(object sender, EventArgs e)
        {
            //  동시에 진행되지 않는 함수들만 동일한 타이머로 한다.

            //m_btimer_LoaderWork_Stop = false;
            timer_LoaderWork.Enabled = false;


            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //  메인 화면 갱신용 변수
            Equipment.m_bMainProcessStatus_LD_LPort_Complete = m_bStacker1_Complete;
            Equipment.m_bMainProcessStatus_LD_RPort_Complete = m_bStacker0_Complete;

            //  M-Aligner 에 Module 을 내려놓는 단계를 진행해야 하므로, Port 에서 Pick Up 이 완료된 것으로 본다.
            Equipment.m_bMainProcessStatus_LD_Module_PortPickUp_Complete = m_nLoaderTransfer_ProcessStep == (int)LoaderTransferProcessStep.LoaderStep_ModulePutDown_MAligner ? true : false;

            //  M-Aligner 에서 Module 을 집어올리는 단계를 진행해야 하므로, M-Aligner 에 Put Down 이 완료된 것으로 본다.
            Equipment.m_bMainProcessStatus_LD_Module_MAlignerPutDown_Complete = m_bMAlignZone_ModuleExist || (m_nLoaderTransfer_ProcessStep == (int)LoaderTransferProcessStep.LoaderStep_ModulePickUp_MAligner) ? true : false;

            //  M-Align 완료
            Equipment.m_bMainProcessStatus_LD_M_Aligner_Align_Complete = m_bMAlign_Complete;

            //  Work Stage 에 Module 을 내려놓는 단계를 진행해야 하므로, M-Aligner 에서 Pick Up 이 완료된 것으로 본다.
            Equipment.m_bMainProcessStatus_LD_Module_MAlignerPickUp_Complete = m_nLoaderTransfer_ProcessStep == (int)LoaderTransferProcessStep.LoaderStep_ModulePutDown_Stage ? true : false;

            //  Work Stage 에 Module 을 내려놓는  단계 완료
            Equipment.m_bMainProcessStatus_LD_Module_WorkStagePutDown_Complete = m_bAUTORUN_Loader_Transfer_ModulePutDowntoWorkStage_Complete && (m_nLoaderTransfer_ProcessStep == (int)LoaderTransferProcessStep.LoaderStep_ModulePickup_fromStacker) ? true : false;
            //  메인 화면 갱신용 변수
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //m_bLD_LPort_Complete = false;                               //  Left Port 동작 완료 여부
            //m_bLD_RPort_Complete = false;                               //  Right Port 동작 완료 여부
            //m_bLD_TR_ModulePickUp_LPort_Complete = false;               //  L-Port Transfer Module Pick Up 동작 완료 여부
            //m_bLD_TR_ModulePickUp_RPort_Complete = false;               //  R-Port Transfer Module Pick Up 동작 완료 여부
            //m_bLD_TR_ModulePutDown_MAligner_Complete = false;           //  M-Aligner Transfer Module Put Down 동작 완료 여부
            //m_bLD_MAligner_Exist = false;                               //  M-Aligner Module Exist
            //m_bLD_MAlign_Complete = false;                              //  M-Aligner 동작 완료 여부
            //m_bLD_TR_ModulePickUp_MAligner_Complete { set; get; }       //  M-Aligner Transfer Module Pick Up 동작 완료 여부
            //m_bLD_WorkStage_LoadingComplete = false;                    //  Work Stage 로 Module Loading 완료 여부

            //  자동운전 시, R-Port 동작 조건 : TR Cycle (None), R-Port Cycle (None), R-Port Module Pickup Complete
            Run_Stacker0Module_PickupWaitingPos_Func();

            //  자동운전 시, L-Port 동작 조건 : TR Cycle (None), L-Port Cycle (None), L-Port Module Pickup Complete
            Run_Stacker1Module_PickupWaitingPos_Func();

            ////  자동운전 시, R-Port 에서 Module Pick-Up 조건 : R-Port Cycle (None), TR Cycle (None), R-Port Complete
            //m_nLoaderTransferMoveType = (int)LoaderTransferMoveType.Cycle_Stacker0_PickUp;
            //Run_Transfer_Cycle_Func();

            ////  자동운전 시, L-Port 에서 Module Pick-Up 조건 : L-Port Cycle (None), TR Cycle (None), L-Port Complete
            //m_nLoaderTransferMoveType = (int)LoaderTransferMoveType.Cycle_Stacker1_PickUp;
            //Run_Transfer_Cycle_Func();

            ////  자동운전 시, M-Aligner 로 Module Put-Down 조건 : L(R)-Port Module Pickup Complete, TR Cycle (None), M-Aligner Module Not Exist, M-Aligner Cycle (None)
            //m_nLoaderTransferMoveType = (int)LoaderTransferMoveType.Cycle_MAligner_PutDown;
            //Run_Transfer_Cycle_Func();

            //  자동운전 시, M-Align 동작 조건 : TR Cycle (None), M-Aligner Module Exist, M-Aligner Cycle (None)
            Run_MAlign_Cycle_Func();

            ////  자동운전 시, M-Aligner 에서 Module Pick-Up 조건 : M-Aligner Cycle (None), TR Cycle (None), M-Aligner Module Exist, M-Aligner Complete
            //m_nLoaderTransferMoveType = (int)LoaderTransferMoveType.Cycle_MAligner_PickUp;
            Run_Transfer_Cycle_Func();

            ////  자동운전 시, Work Stage 로 Module Put-Down 조건 : TR Cycle (None), M-Aligner Module Pickup Complete, Work Stage Cycle (None), Work Stage Loading Ready
            //m_nLoaderTransferMoveType = (int)LoaderTransferMoveType.Cycle_WorkStage_PutDown;
            //Run_Transfer_Cycle_Func();

            //if (!m_btimer_LoaderWork_Stop)
            {
                timer_LoaderWork.Enabled = true;
            }
        }

        private void Timer_ProductAlign_Func(object sender, EventArgs e)
        {
            if (!m_bAlignVisionThread_Use)
            {
                
            }
        }

        public void forThread_MainWorkCycle()
        {
            //  Main-Work Cycle

        }

        public void forThread_SubWorkCycle()
        {
            //  Sub-Work Cycle

        }

        public void forThread_AlignVisionCycle()
        {
            //Run_ProductAlign_Func();            
        }


        #endregion

        #region Method

        public List<string> GetPositionList()
        {
            List<string> ret = new List<string>();

            foreach (ZzxzxyPositionData position in Config.Positions)
            {
                if (position.Type == TargetType.Base)
                    ret.Add(position.Name);
            }
            return ret;
        }

        

        #endregion


        //        public XytCoordinate GetCurrentPosition()
        //        {
        //            XytCoordinate current = new XytCoordinate();
        //            if (Stage != null)
        //            {
        //                current.X = Stage.Axes["X"].Motor.ActualPosition;
        //                current.Y = Stage.Axes["Y"].Motor.ActualPosition;
        //                current.T = Stage.Axes["T"].Motor.ActualPosition;
        //            }
        //
        //            return current;
        //        }

    }
}