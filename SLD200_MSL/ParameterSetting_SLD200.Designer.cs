using QMC.Common.Hmi;
using System.Drawing;

namespace SLD200_MSL
{
    partial class ParameterSetting_SLD200
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.lblUpperCamera = new System.Windows.Forms.Label();
            this.lblLowerCamera = new System.Windows.Forms.Label();
            this.btnUpperCamera_Init = new System.Windows.Forms.Button();
            this.btnUpperCamera_StartLive = new System.Windows.Forms.Button();
            this.tabControl_ParameterSet = new System.Windows.Forms.TabControl();
            this.tabPage_Data = new System.Windows.Forms.TabPage();
            //this.SiriusViewer_Parameter = new SpiralLab.Sirius.SiriusViewerForm();
            this.tabPage_Layout = new System.Windows.Forms.TabPage();
            this.baseGroupBox_ScanAreaSet = new SLD200_MSL.BaseGroupBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.baseLabel_ScanArea_Height = new SLD200_MSL.BaseLabel();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.baseLabel_ScanArea_Width = new SLD200_MSL.BaseLabel();
            this.baseGroupBox_ModuleThickness_FiducialArea = new SLD200_MSL.BaseGroupBox();
            this.button_FiducialPos_PNLThickCheck_Start = new System.Windows.Forms.Button();
            this.textBox_FiducialPos_PNLThickness = new System.Windows.Forms.TextBox();
            this.baseLabel_FiducialPos_PNLThickness = new SLD200_MSL.BaseLabel();
            this.textBox_FiducialPos_PNLThickCheck_PosY = new System.Windows.Forms.TextBox();
            this.baseLabel_FiducialPosThickCheck_PosY = new SLD200_MSL.BaseLabel();
            this.textBox_FiducialPos_PNLThickCheck_PosX = new System.Windows.Forms.TextBox();
            this.baseLabel_FiducialPosThickCheck_PosX = new SLD200_MSL.BaseLabel();
            this.baseGroupBox_ModuleThickness_DrillingArea = new SLD200_MSL.BaseGroupBox();
            this.button_DrillingPos_PNLThickCheck_Start = new System.Windows.Forms.Button();
            this.textBox_DrillingPos_PNLThickness = new System.Windows.Forms.TextBox();
            this.baseLabel_DrillingPos_PNLThickness = new SLD200_MSL.BaseLabel();
            this.textBox_DrillingPos_PNLThickCheck_PosY = new System.Windows.Forms.TextBox();
            this.baseLabel_DrillingPosThickCheck_PosY = new SLD200_MSL.BaseLabel();
            this.textBox_DrillingPos_PNLThickCheck_PosX = new System.Windows.Forms.TextBox();
            this.baseLabel_DrillingPosThickCheck_PosX = new SLD200_MSL.BaseLabel();
            this.tabPage_Parameter = new System.Windows.Forms.TabPage();
            this.baseGroupBox_DrillingToolParam = new SLD200_MSL.BaseGroupBox();
            this.baseLabel_DrillingTool_LaserOffDelay_Unit = new SLD200_MSL.BaseLabel();
            this.textBox_DrillingTool_LaserOffDelay = new System.Windows.Forms.TextBox();
            this.baseLabel_DrillingTool_LaserOffDelay = new SLD200_MSL.BaseLabel();
            this.baseLabel_DrillingTool_LaserOnDelay_Unit = new SLD200_MSL.BaseLabel();
            this.textBox_DrillingTool_LaserOnDelay = new System.Windows.Forms.TextBox();
            this.baseLabel_DrillingTool_LaserOnDelay = new SLD200_MSL.BaseLabel();
            this.baseLabel_DrillingTool_JumpDelay_Unit = new SLD200_MSL.BaseLabel();
            this.textBox_DrillingTool_JumpDelay = new System.Windows.Forms.TextBox();
            this.baseLabel_DrillingTool_JumpDelay = new SLD200_MSL.BaseLabel();
            this.baseLabel_DrillingTool_MarkDelay_Unit = new SLD200_MSL.BaseLabel();
            this.textBox_DrillingTool_MarkDelay = new System.Windows.Forms.TextBox();
            this.baseLabel_DrillingTool_MarkDelay = new SLD200_MSL.BaseLabel();
            this.baseLabel_DrillingTool_JumpSpeed_Unit = new SLD200_MSL.BaseLabel();
            this.textBox_DrillingTool_JumpSpeed = new System.Windows.Forms.TextBox();
            this.baseLabel_DrillingTool_JumpSpeed = new SLD200_MSL.BaseLabel();
            this.baseLabel_DrillingTool_MarkSpeed_Unit = new SLD200_MSL.BaseLabel();
            this.textBox_DrillingTool_MarkSpeed = new System.Windows.Forms.TextBox();
            this.baseLabel_DrillingTool_MarkSpeed = new SLD200_MSL.BaseLabel();
            this.baseLabel_DrillingTool_Freq_Unit = new SLD200_MSL.BaseLabel();
            this.textBox_DrillingTool_Frequency = new System.Windows.Forms.TextBox();
            this.baseLabel_DrillingTool_Freq = new SLD200_MSL.BaseLabel();
            this.baseLabel_DrillingTool_ZOffset_Unit = new SLD200_MSL.BaseLabel();
            this.textBox_DrillingTool_ZOffset = new System.Windows.Forms.TextBox();
            this.baseLabel_DrillingTool_ZOffset = new SLD200_MSL.BaseLabel();
            this.textBox_DrillingTool_RepeatCount = new System.Windows.Forms.TextBox();
            this.baseLabel_DrillingTool_RepeatCount = new SLD200_MSL.BaseLabel();
            this.baseLabel_DrillingTool_HoleSize_Unit = new SLD200_MSL.BaseLabel();
            this.textBox_DrillingTool_HoleSize = new System.Windows.Forms.TextBox();
            this.baseLabel_DrillingTool_HoleSize = new SLD200_MSL.BaseLabel();
            this.comboBox_DrillingTool_MaskNo = new System.Windows.Forms.ComboBox();
            this.baseLabel_DrillingTool_MaskNo = new SLD200_MSL.BaseLabel();
            this.comboBox_DrillingTool_Type = new System.Windows.Forms.ComboBox();
            this.baseLabel_DrillingTool_Type = new SLD200_MSL.BaseLabel();
            this.textBox_DrillingTool_No = new System.Windows.Forms.TextBox();
            this.baseLabel_DrillingTool_No = new SLD200_MSL.BaseLabel();
            this.baseGroupBox_DrillingToolList = new SLD200_MSL.BaseGroupBox();
            this.button_DrillingMainTool_Save = new System.Windows.Forms.Button();
            this.button_DrillingMainTool_Open = new System.Windows.Forms.Button();
            this.button_DrillingTool_Delete = new System.Windows.Forms.Button();
            this.button_DrillingTool_Add = new System.Windows.Forms.Button();
            this.treeView_DrillingTool = new System.Windows.Forms.TreeView();
            this.tabPage_Fiducial = new System.Windows.Forms.TabPage();
            this.baseGroupBox_SearchResult = new SLD200_MSL.BaseGroupBox();
            this.baseLabel1 = new SLD200_MSL.BaseLabel();
            this.textBox_SearchResult_Size = new System.Windows.Forms.TextBox();
            this.baseLabel_SearchResult_Size = new SLD200_MSL.BaseLabel();
            this.baseLabel_MinScore_Unit = new SLD200_MSL.BaseLabel();
            this.textBox_SearchResult_MinScore = new System.Windows.Forms.TextBox();
            this.baseLabel_SearchResult_MinScore = new SLD200_MSL.BaseLabel();
            this.baseGroupBox_Light = new SLD200_MSL.BaseGroupBox();
            this.trackBar_HighResVision_Light = new System.Windows.Forms.TrackBar();
            this.trackBar_LowResVision_Light = new System.Windows.Forms.TrackBar();
            this.numericUpDown_HighResVision_Light = new System.Windows.Forms.NumericUpDown();
            this.baseLabel_HighRes_Light = new SLD200_MSL.BaseLabel();
            this.numericUpDown_LowResVision_Light = new System.Windows.Forms.NumericUpDown();
            this.baseLabel_LowRes_Light = new SLD200_MSL.BaseLabel();
            this.baseGroupBox_FiducialModel = new SLD200_MSL.BaseGroupBox();
            this.comboBox_Polarity = new System.Windows.Forms.ComboBox();
            this.baseLabel_Polarity = new SLD200_MSL.BaseLabel();
            this.pictureBox_FiducialModel_Image = new System.Windows.Forms.PictureBox();
            this.baseLabel21 = new SLD200_MSL.BaseLabel();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.baseLabel22 = new SLD200_MSL.BaseLabel();
            this.baseLabel19 = new SLD200_MSL.BaseLabel();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.baseLabel20 = new SLD200_MSL.BaseLabel();
            this.baseLabel18 = new SLD200_MSL.BaseLabel();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.baseLabel_FiducialSize_A = new SLD200_MSL.BaseLabel();
            this.comboBox_FiducialType = new System.Windows.Forms.ComboBox();
            this.baseLabel_FiducialType = new SLD200_MSL.BaseLabel();
            this.baseGroupBox_FiducialList = new SLD200_MSL.BaseGroupBox();
            this.button_MoveTo_FiducialMarkPos = new System.Windows.Forms.Button();
            this.treeView_FiducialMark = new System.Windows.Forms.TreeView();
            this.tabPage_Position = new System.Windows.Forms.TabPage();
            this.tabControl_Position = new System.Windows.Forms.TabControl();
            this.tabPage_Pos_Unloader = new System.Windows.Forms.TabPage();
            this.button_ULStacker1Pos_Empty_Get = new System.Windows.Forms.Button();
            this.textBox_ULStacker1Pos_Empty = new System.Windows.Forms.TextBox();
            this.baseLabel_ULStacker1Pos_Empty1 = new SLD200_MSL.BaseLabel();
            this.baseLabel_ULStacker1Pos_Empty = new SLD200_MSL.BaseLabel();
            this.button_ULStacker1Pos_Full_Get = new System.Windows.Forms.Button();
            this.textBox_ULStacker1Pos_Full = new System.Windows.Forms.TextBox();
            this.baseLabel_ULStacker1Pos_Full1 = new SLD200_MSL.BaseLabel();
            this.baseLabel_ULStacker1Pos_Full = new SLD200_MSL.BaseLabel();
            this.button_ULStacker0Pos_Empty_Get = new System.Windows.Forms.Button();
            this.textBox_ULStacker0Pos_Empty = new System.Windows.Forms.TextBox();
            this.baseLabel_ULStacker0Pos_Empty1 = new SLD200_MSL.BaseLabel();
            this.baseLabel_ULStacker0Pos_Empty = new SLD200_MSL.BaseLabel();
            this.button_ULStacker0Pos_Full_Get = new System.Windows.Forms.Button();
            this.textBox_ULStacker0Pos_Full = new System.Windows.Forms.TextBox();
            this.baseLabel_ULStacker0Pos_Full1 = new SLD200_MSL.BaseLabel();
            this.baseLabel_ULStacker0Pos_Full = new SLD200_MSL.BaseLabel();
            this.button_ULPickerPos_Moving_Get = new System.Windows.Forms.Button();
            this.textBox_ULPickerPos_Moving_Z = new System.Windows.Forms.TextBox();
            this.baseLabel_ULPickerPos_Moving_Z = new SLD200_MSL.BaseLabel();
            this.baseLabel_ULPickerPos_Moving = new SLD200_MSL.BaseLabel();
            this.button_ULPickerPos_Module_PutDown_Stacker1_Get = new System.Windows.Forms.Button();
            this.textBox_ULPickerPos_Module_PutDown_Stacker1_Z = new System.Windows.Forms.TextBox();
            this.baseLabel_ULPickerPos_MGZ2_Z = new SLD200_MSL.BaseLabel();
            this.textBox_ULPickerPos_Module_PutDown_Stacker1_X = new System.Windows.Forms.TextBox();
            this.baseLabel_ULPickerPos_MGZ2_X = new SLD200_MSL.BaseLabel();
            this.baseLabel_ULPickerPos_MGZ2 = new SLD200_MSL.BaseLabel();
            this.button_ULPickerPos_Module_PutDown_Stacker0_Get = new System.Windows.Forms.Button();
            this.textBox_ULPickerPos_Module_PutDown_Stacker0_Z = new System.Windows.Forms.TextBox();
            this.baseLabel_ULPickerPos_MGZ1_Z = new SLD200_MSL.BaseLabel();
            this.textBox_ULPickerPos_Module_PutDown_Stacker0_X = new System.Windows.Forms.TextBox();
            this.baseLabel_ULPickerPos_MGZ1_X = new SLD200_MSL.BaseLabel();
            this.baseLabel_ULPickerPos_MGZ1 = new SLD200_MSL.BaseLabel();
            this.button_ULPickerPos_NgBox_Get = new System.Windows.Forms.Button();
            this.button_ULPickerPos_Module_PickUp_WorkTable_Get = new System.Windows.Forms.Button();
            this.textBox_ULPickerPos_NgBox_Z = new System.Windows.Forms.TextBox();
            this.baseLabel_ULPickerPos_NgBox_Z = new SLD200_MSL.BaseLabel();
            this.textBox_ULPickerPos_NgBox_X = new System.Windows.Forms.TextBox();
            this.baseLabel_ULPickerPos_NgBox_X = new SLD200_MSL.BaseLabel();
            this.baseLabel_ULPickerPos_NGBox = new SLD200_MSL.BaseLabel();
            this.textBox_ULPickerPos_Module_PickUp_WorkTable_Z = new System.Windows.Forms.TextBox();
            this.baseLabel_ULPickerPos_WorkTable_Z = new SLD200_MSL.BaseLabel();
            this.textBox_ULPickerPos_Module_PickUp_WorkTable_X = new System.Windows.Forms.TextBox();
            this.baseLabel_ULPickerPos_WorkTable_X = new SLD200_MSL.BaseLabel();
            this.baseLabel_ULPickerPos_WorkTable = new SLD200_MSL.BaseLabel();
            this.tabPage_Pos_WorkStage = new System.Windows.Forms.TabPage();
            this.baseLabel72 = new SLD200_MSL.BaseLabel();
            this.button_MainUnitPos_Mask4_Get = new System.Windows.Forms.Button();
            this.textBox_MainUnitPos_Mask4_FwBw_Y = new System.Windows.Forms.TextBox();
            this.button_MainUnitPos_Mask3_Get = new System.Windows.Forms.Button();
            this.textBox_MainUnitPos_Mask3_FwBw_Y = new System.Windows.Forms.TextBox();
            this.button_MainUnitPos_Mask2_Get = new System.Windows.Forms.Button();
            this.textBox_MainUnitPos_Mask2_FwBw_Y = new System.Windows.Forms.TextBox();
            this.button_MainUnitPos_Mask1_Get = new System.Windows.Forms.Button();
            this.textBox_MainUnitPos_Mask1_FwBw_Y = new System.Windows.Forms.TextBox();
            this.button_MainUnitPos_MaskEmpty_Get = new System.Windows.Forms.Button();
            this.textBox_MainUnitPos_MaskEmpty_FwBw_Y = new System.Windows.Forms.TextBox();
            this.textBox_MainUnitPos_HeightSensorCalSheetLT_Z = new System.Windows.Forms.TextBox();
            this.button_MainUnitPos_HeightSensorCalSheetLT_Get = new System.Windows.Forms.Button();
            this.textBox_MainUnitPos_HeightSensorCalSheetLT_Y = new System.Windows.Forms.TextBox();
            this.textBox_MainUnitPos_HeightSensorCalSheetLT_X = new System.Windows.Forms.TextBox();
            this.textBox_MainUnitPos_HeightSensorStageCenter_Z = new System.Windows.Forms.TextBox();
            this.button_MainUnitPos_HeightSensorWorkStageCenter_Get = new System.Windows.Forms.Button();
            this.textBox_MainUnitPos_HeightSensorStageCenter_Y = new System.Windows.Forms.TextBox();
            this.textBox_MainUnitPos_HeightSensorStageCenter_X = new System.Windows.Forms.TextBox();
            this.textBox_MainUnitPos_LowResCamReticleGlass_Z = new System.Windows.Forms.TextBox();
            this.button_MainUnitPos_LowResCamReticleGlass_Get = new System.Windows.Forms.Button();
            this.textBox_MainUnitPos_LowResCamReticleGlass_Y = new System.Windows.Forms.TextBox();
            this.textBox_MainUnitPos_LowResCamReticleGlass_X = new System.Windows.Forms.TextBox();
            this.textBox_MainUnitPos_LowResCamCalSheetLT_Z = new System.Windows.Forms.TextBox();
            this.button_MainUnitPos_LowResCamCalSheetLT_Get = new System.Windows.Forms.Button();
            this.textBox_MainUnitPos_LowResCamCalSheetLT_Y = new System.Windows.Forms.TextBox();
            this.textBox_MainUnitPos_LowResCamCalSheetLT_X = new System.Windows.Forms.TextBox();
            this.textBox_MainUnitPos_LowResCamWorkStageCenter_Z = new System.Windows.Forms.TextBox();
            this.button_MainUnitPos_LowResCamWorkStageCenter_Get = new System.Windows.Forms.Button();
            this.textBox_MainUnitPos_LowResCamWorkStageCenter_Y = new System.Windows.Forms.TextBox();
            this.textBox_MainUnitPos_LowResCamWorkStageCenter_X = new System.Windows.Forms.TextBox();
            this.textBox_MainUnitPos_HighResCamReticleGlass_Z = new System.Windows.Forms.TextBox();
            this.button_MainUnitPos_HighResCamReticleGlass_Get = new System.Windows.Forms.Button();
            this.textBox_MainUnitPos_HighResCamReticleGlass_Y = new System.Windows.Forms.TextBox();
            this.textBox_MainUnitPos_HighResCamReticleGlass_X = new System.Windows.Forms.TextBox();
            this.textBox_MainUnitPos_HighResCamCalSheetLT_Z = new System.Windows.Forms.TextBox();
            this.button_MainUnitPos_HighResCamCalSheetLT_Get = new System.Windows.Forms.Button();
            this.textBox_MainUnitPos_HighResCamCalSheetLT_Y = new System.Windows.Forms.TextBox();
            this.textBox_MainUnitPos_HighResCamCalSheetLT_X = new System.Windows.Forms.TextBox();
            this.textBox_MainUnitPos_HighResCamWorkStageCenter_Z = new System.Windows.Forms.TextBox();
            this.button_MainUnitPos_HighResCamWorkStageCenter_Get = new System.Windows.Forms.Button();
            this.textBox_MainUnitPos_HighResCamWorkStageCenter_Y = new System.Windows.Forms.TextBox();
            this.textBox_MainUnitPos_HighResCamWorkStageCenter_X = new System.Windows.Forms.TextBox();
            this.textBox_MainUnitPos_ScannerPowerMeter_Z = new System.Windows.Forms.TextBox();
            this.button_MainUnitPos_ScannerPowerMeter_Get = new System.Windows.Forms.Button();
            this.textBox_MainUnitPos_ScannerPowerMeter_Y = new System.Windows.Forms.TextBox();
            this.textBox_MainUnitPos_ScannerPowerMeter_X = new System.Windows.Forms.TextBox();
            this.textBox_MainUnitPos_ScannerWorkStageCenter_Z = new System.Windows.Forms.TextBox();
            this.button_MainUnitPos_ScannerWorkStageCenter_Get = new System.Windows.Forms.Button();
            this.textBox_MainUnitPos_ScannerWorkStageCenter_Y = new System.Windows.Forms.TextBox();
            this.textBox_MainUnitPos_ScannerWorkStageCenter_X = new System.Windows.Forms.TextBox();
            this.textBox_MainUnitPos_ScannerLensCleaning_Z = new System.Windows.Forms.TextBox();
            this.button_MainUnitPos_ScannerLensCleaning_Get = new System.Windows.Forms.Button();
            this.textBox_MainUnitPos_ScannerLensCleaning_Y = new System.Windows.Forms.TextBox();
            this.textBox_MainUnitPos_ScannerLensCleaning_X = new System.Windows.Forms.TextBox();
            this.textBox_MainUnitPos_ModuleUnload_Z = new System.Windows.Forms.TextBox();
            this.button_MainUnitPos_ModuleUnload_Get = new System.Windows.Forms.Button();
            this.textBox_MainUnitPos_ModuleUnload_Y = new System.Windows.Forms.TextBox();
            this.textBox_MainUnitPos_ModuleUnload_X = new System.Windows.Forms.TextBox();
            this.textBox_MainUnitPos_ModuleLoad_Z = new System.Windows.Forms.TextBox();
            this.button_MainUnitPos_ModuleLoad_Get = new System.Windows.Forms.Button();
            this.textBox_MainUnitPos_ModuleLoad_Y = new System.Windows.Forms.TextBox();
            this.textBox_MainUnitPos_ModuleLoad_X = new System.Windows.Forms.TextBox();
            this.textBox_MainUnitPos_JigChange_Z = new System.Windows.Forms.TextBox();
            this.button_MainUnitPos_JigChange_Get = new System.Windows.Forms.Button();
            this.textBox_MainUnitPos_JigChange_Y = new System.Windows.Forms.TextBox();
            this.textBox_MainUnitPos_JigChange_X = new System.Windows.Forms.TextBox();
            this.baseLabel_MainUnitPos_Mask4 = new SLD200_MSL.BaseLabel();
            this.baseLabel74 = new SLD200_MSL.BaseLabel();
            this.baseLabel_MainUnitPos_Mask3 = new SLD200_MSL.BaseLabel();
            this.baseLabel70 = new SLD200_MSL.BaseLabel();
            this.baseLabel_MainUnitPos_Mask2 = new SLD200_MSL.BaseLabel();
            this.baseLabel67 = new SLD200_MSL.BaseLabel();
            this.baseLabel_MainUnitPos_Mask1 = new SLD200_MSL.BaseLabel();
            this.baseLabel64 = new SLD200_MSL.BaseLabel();
            this.baseLabel_MainUnitPos_MaskEmpty = new SLD200_MSL.BaseLabel();
            this.baseLabel68 = new SLD200_MSL.BaseLabel();
            this.baseLabel55 = new SLD200_MSL.BaseLabel();
            this.baseLabel59 = new SLD200_MSL.BaseLabel();
            this.baseLabel63 = new SLD200_MSL.BaseLabel();
            this.baseLabel_MainUnitPos_HeightSensorCalSheetLT = new SLD200_MSL.BaseLabel();
            this.baseLabel65 = new SLD200_MSL.BaseLabel();
            this.baseLabel66 = new SLD200_MSL.BaseLabel();
            this.baseLabel_MainUnitPos_HeightSensorStageCenter = new SLD200_MSL.BaseLabel();
            this.baseLabel52 = new SLD200_MSL.BaseLabel();
            this.baseLabel53 = new SLD200_MSL.BaseLabel();
            this.baseLabel54 = new SLD200_MSL.BaseLabel();
            this.baseLabel_MainUnitPos_LowResCameraReticleGlass = new SLD200_MSL.BaseLabel();
            this.baseLabel56 = new SLD200_MSL.BaseLabel();
            this.baseLabel57 = new SLD200_MSL.BaseLabel();
            this.baseLabel58 = new SLD200_MSL.BaseLabel();
            this.baseLabel_MainUnitPos_LowResCameraCalSheetLT = new SLD200_MSL.BaseLabel();
            this.baseLabel60 = new SLD200_MSL.BaseLabel();
            this.baseLabel61 = new SLD200_MSL.BaseLabel();
            this.baseLabel62 = new SLD200_MSL.BaseLabel();
            this.baseLabel_MainUnitPos_LowResCameraStageCenter = new SLD200_MSL.BaseLabel();
            this.baseLabel51 = new SLD200_MSL.BaseLabel();
            this.baseLabel49 = new SLD200_MSL.BaseLabel();
            this.baseLabel50 = new SLD200_MSL.BaseLabel();
            this.baseLabel_MainUnitPos_HighResCameraReticleGlass = new SLD200_MSL.BaseLabel();
            this.baseLabel46 = new SLD200_MSL.BaseLabel();
            this.baseLabel47 = new SLD200_MSL.BaseLabel();
            this.baseLabel48 = new SLD200_MSL.BaseLabel();
            this.baseLabel_MainUnitPos_HighResCameraCalSheetLT = new SLD200_MSL.BaseLabel();
            this.baseLabel43 = new SLD200_MSL.BaseLabel();
            this.baseLabel44 = new SLD200_MSL.BaseLabel();
            this.baseLabel45 = new SLD200_MSL.BaseLabel();
            this.baseLabel_MainUnitPos_HighResCameraStageCenter = new SLD200_MSL.BaseLabel();
            this.baseLabel40 = new SLD200_MSL.BaseLabel();
            this.baseLabel41 = new SLD200_MSL.BaseLabel();
            this.baseLabel42 = new SLD200_MSL.BaseLabel();
            this.baseLabel_MainUnitPos_ScannerPowerMeter = new SLD200_MSL.BaseLabel();
            this.baseLabel37 = new SLD200_MSL.BaseLabel();
            this.baseLabel38 = new SLD200_MSL.BaseLabel();
            this.baseLabel39 = new SLD200_MSL.BaseLabel();
            this.baseLabel_MainUnitPos_ScannerStageCenter = new SLD200_MSL.BaseLabel();
            this.baseLabel34 = new SLD200_MSL.BaseLabel();
            this.baseLabel35 = new SLD200_MSL.BaseLabel();
            this.baseLabel36 = new SLD200_MSL.BaseLabel();
            this.baseLabel_MainUnitPos_ScannerLensCleaning = new SLD200_MSL.BaseLabel();
            this.baseLabel31 = new SLD200_MSL.BaseLabel();
            this.baseLabel32 = new SLD200_MSL.BaseLabel();
            this.baseLabel33 = new SLD200_MSL.BaseLabel();
            this.baseLabel_MainUnitPos_ModuleUnload = new SLD200_MSL.BaseLabel();
            this.baseLabel28 = new SLD200_MSL.BaseLabel();
            this.baseLabel29 = new SLD200_MSL.BaseLabel();
            this.baseLabel30 = new SLD200_MSL.BaseLabel();
            this.baseLabel_MainUnitPos_ModuleLoad = new SLD200_MSL.BaseLabel();
            this.baseLabel27 = new SLD200_MSL.BaseLabel();
            this.baseLabel23 = new SLD200_MSL.BaseLabel();
            this.baseLabel26 = new SLD200_MSL.BaseLabel();
            this.baseLabel_MainUnitPos_JigChange = new SLD200_MSL.BaseLabel();
            this.tabPage_Pos_Loader = new System.Windows.Forms.TabPage();
            this.button_LDAlignerPos_100mmClose_Get = new System.Windows.Forms.Button();
            this.textBox_LDAlignerPos_100mmClose_Y = new System.Windows.Forms.TextBox();
            this.textBox_LDAlignerPos_100mmClose_X = new System.Windows.Forms.TextBox();
            this.button_LDAlignerPos_FullOpen_Get = new System.Windows.Forms.Button();
            this.textBox_LDAlignerPos_FullOpen_Y = new System.Windows.Forms.TextBox();
            this.textBox_LDAlignerPos_FullOpen_X = new System.Windows.Forms.TextBox();
            this.button_LDPickerPos_Module_PutDown_AlignTable_Get = new System.Windows.Forms.Button();
            this.textBox_LDPickerPos_Module_PutDown_Aligner_Y = new System.Windows.Forms.TextBox();
            this.textBox_LDPickerPos_Module_PutDown_Aligner_X = new System.Windows.Forms.TextBox();
            this.button_LDPickerPos_Module_PickUp_AlignTable_Get = new System.Windows.Forms.Button();
            this.textBox_LDPickerPos_Module_PickUp_Aligner_Y = new System.Windows.Forms.TextBox();
            this.textBox_LDPickerPos_Module_PickUp_Aligner_X = new System.Windows.Forms.TextBox();
            this.button_LDStacker1Pos_Empty_Get = new System.Windows.Forms.Button();
            this.textBox_LDStacker1Pos_Empty = new System.Windows.Forms.TextBox();
            this.button_LDStacker1Pos_Full_Get = new System.Windows.Forms.Button();
            this.textBox_LDStacker1Pos_Full = new System.Windows.Forms.TextBox();
            this.button_LDStacker0Pos_Empty_Get = new System.Windows.Forms.Button();
            this.textBox_LDStacker0Pos_Empty = new System.Windows.Forms.TextBox();
            this.button_LDStacker0Pos_Full_Get = new System.Windows.Forms.Button();
            this.textBox_LDStacker0Pos_Full = new System.Windows.Forms.TextBox();
            this.button_LDPickerPos_Module_PickUp_Stacker1_Get = new System.Windows.Forms.Button();
            this.textBox_LDPickerPos_Module_PickUp_Stacker1_Z = new System.Windows.Forms.TextBox();
            this.textBox_LDPickerPos_Module_PickUp_Stacker1_X = new System.Windows.Forms.TextBox();
            this.button_LDPickerPos_Module_PickUp_Stacker0_Get = new System.Windows.Forms.Button();
            this.textBox_LDPickerPos_Module_PickUp_Stacker0_Z = new System.Windows.Forms.TextBox();
            this.textBox_LDPickerPos_Module_PickUp_Stacker0_X = new System.Windows.Forms.TextBox();
            this.button_LDPickerPos_Module_PutDown_WorkTable_Get = new System.Windows.Forms.Button();
            this.textBox_LDPickerPos_Module_PutDown_WorkTable_Z = new System.Windows.Forms.TextBox();
            this.textBox_LDPickerPos_Module_PutDown_WorkTable_X = new System.Windows.Forms.TextBox();
            this.button_LDPickerPos_Moving_Get = new System.Windows.Forms.Button();
            this.textBox_LDPickerPos_Moving_Z = new System.Windows.Forms.TextBox();
            this.baseLabel15 = new SLD200_MSL.BaseLabel();
            this.baseLabel17 = new SLD200_MSL.BaseLabel();
            this.baseLabel_LDAlignerPos_100mmClose = new SLD200_MSL.BaseLabel();
            this.baseLabel24 = new SLD200_MSL.BaseLabel();
            this.baseLabel25 = new SLD200_MSL.BaseLabel();
            this.baseLabel_LDAlignerPos_FullOpen = new SLD200_MSL.BaseLabel();
            this.baseLabel11 = new SLD200_MSL.BaseLabel();
            this.baseLabel13 = new SLD200_MSL.BaseLabel();
            this.baseLabel_LDPickerPos_Aligner1 = new SLD200_MSL.BaseLabel();
            this.baseLabel6 = new SLD200_MSL.BaseLabel();
            this.baseLabel9 = new SLD200_MSL.BaseLabel();
            this.baseLabel_LDPickerPos_Aligner = new SLD200_MSL.BaseLabel();
            this.baseLabel10 = new SLD200_MSL.BaseLabel();
            this.baseLabel_LDStacker1Pos_Empty = new SLD200_MSL.BaseLabel();
            this.baseLabel12 = new SLD200_MSL.BaseLabel();
            this.baseLabel_LDStacker1Pos_Full = new SLD200_MSL.BaseLabel();
            this.baseLabel14 = new SLD200_MSL.BaseLabel();
            this.baseLabel_LDStacker0Pos_Empty = new SLD200_MSL.BaseLabel();
            this.baseLabel16 = new SLD200_MSL.BaseLabel();
            this.baseLabel_LDStacker0Pos_Full = new SLD200_MSL.BaseLabel();
            this.baseLabel4 = new SLD200_MSL.BaseLabel();
            this.baseLabel5 = new SLD200_MSL.BaseLabel();
            this.baseLabel_LDPickerPos_MGZ2 = new SLD200_MSL.BaseLabel();
            this.baseLabel7 = new SLD200_MSL.BaseLabel();
            this.baseLabel8 = new SLD200_MSL.BaseLabel();
            this.baseLabel_LDPickerPos_MGZ1 = new SLD200_MSL.BaseLabel();
            this.baseLabel2 = new SLD200_MSL.BaseLabel();
            this.baseLabel3 = new SLD200_MSL.BaseLabel();
            this.baseLabel_LDPickerPos_WorkTable = new SLD200_MSL.BaseLabel();
            this.baseLabel_LDPickerPos_Moving_Z = new SLD200_MSL.BaseLabel();
            this.baseLabel_LDPickerPos_Moving = new SLD200_MSL.BaseLabel();
            this.tabPage_Option = new System.Windows.Forms.TabPage();
            this.textBox_LDPickerSetting_PickerVibrationTimes = new System.Windows.Forms.TextBox();
            this.baseLabel_LDPickerSetting_Vibration = new SLD200_MSL.BaseLabel();
            this.textBox_Option_ULPickUpRetry_Count = new System.Windows.Forms.TextBox();
            this.button_Option_ULPickUpRetry_Enable = new System.Windows.Forms.Button();
            this.pictureBox_Option_ULPickUp_Retry_Enable = new System.Windows.Forms.PictureBox();
            this.textBox_Option_LDPickUpRetry_Count = new System.Windows.Forms.TextBox();
            this.button_Option_LDPickUpRetry_Enable = new System.Windows.Forms.Button();
            this.pictureBox_Option_LDPickUp_Retry_Enable = new System.Windows.Forms.PictureBox();
            this.button_Option_AllModuleThickCheck_Enable = new System.Windows.Forms.Button();
            this.pictureBox_Option_AllPanelThickCheck_Enable = new System.Windows.Forms.PictureBox();
            this.button_Option_DoorInterlock_Enable = new System.Windows.Forms.Button();
            this.pictureBox_Option_DoorInterlock_Enable = new System.Windows.Forms.PictureBox();
            this.baseLabel_Option_RetryCount2 = new SLD200_MSL.BaseLabel();
            this.baseLabel_Option_RetryCount = new SLD200_MSL.BaseLabel();
            this.tabControl_Jog = new System.Windows.Forms.TabControl();
            this.tabPage_Unloader = new System.Windows.Forms.TabPage();
            this.tabPage_WorkStage = new System.Windows.Forms.TabPage();
            this.tabPage_Loader = new System.Windows.Forms.TabPage();
            this.m_visionImageViewer_HighRes = new QMC.Common.Hmi.VisionImageViewer();
            this.m_visionImageViewer_LowRes = new QMC.Common.Hmi.VisionImageViewer();
            this.baseLabel_HighRes_Camera = new SLD200_MSL.BaseLabel();
            this.baseLabel_LowRes_Camera = new SLD200_MSL.BaseLabel();
            this.tabControl_ParameterSet.SuspendLayout();
            this.tabPage_Data.SuspendLayout();
            this.tabPage_Layout.SuspendLayout();
            this.baseGroupBox_ScanAreaSet.SuspendLayout();
            this.baseGroupBox_ModuleThickness_FiducialArea.SuspendLayout();
            this.baseGroupBox_ModuleThickness_DrillingArea.SuspendLayout();
            this.tabPage_Parameter.SuspendLayout();
            this.baseGroupBox_DrillingToolParam.SuspendLayout();
            this.baseGroupBox_DrillingToolList.SuspendLayout();
            this.tabPage_Fiducial.SuspendLayout();
            this.baseGroupBox_SearchResult.SuspendLayout();
            this.baseGroupBox_Light.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_HighResVision_Light)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_LowResVision_Light)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_HighResVision_Light)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_LowResVision_Light)).BeginInit();
            this.baseGroupBox_FiducialModel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_FiducialModel_Image)).BeginInit();
            this.baseGroupBox_FiducialList.SuspendLayout();
            this.tabPage_Position.SuspendLayout();
            this.tabControl_Position.SuspendLayout();
            this.tabPage_Pos_Unloader.SuspendLayout();
            this.tabPage_Pos_WorkStage.SuspendLayout();
            this.tabPage_Pos_Loader.SuspendLayout();
            this.tabPage_Option.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Option_ULPickUp_Retry_Enable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Option_LDPickUp_Retry_Enable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Option_AllPanelThickCheck_Enable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Option_DoorInterlock_Enable)).BeginInit();
            this.tabControl_Jog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_visionImageViewer_HighRes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_visionImageViewer_LowRes)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // lblUpperCamera
            // 
            this.lblUpperCamera.BackColor = System.Drawing.Color.LightSkyBlue;
            this.lblUpperCamera.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblUpperCamera.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblUpperCamera.ForeColor = System.Drawing.Color.Black;
            this.lblUpperCamera.Location = new System.Drawing.Point(8, 122);
            this.lblUpperCamera.Name = "lblUpperCamera";
            this.lblUpperCamera.Size = new System.Drawing.Size(50, 16);
            this.lblUpperCamera.TabIndex = 230;
            this.lblUpperCamera.Text = "Upper\r\nCamera";
            this.lblUpperCamera.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLowerCamera
            // 
            this.lblLowerCamera.BackColor = System.Drawing.Color.LightSkyBlue;
            this.lblLowerCamera.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblLowerCamera.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblLowerCamera.ForeColor = System.Drawing.Color.Black;
            this.lblLowerCamera.Location = new System.Drawing.Point(8, 122);
            this.lblLowerCamera.Name = "lblLowerCamera";
            this.lblLowerCamera.Size = new System.Drawing.Size(50, 16);
            this.lblLowerCamera.TabIndex = 230;
            this.lblLowerCamera.Text = "Lower\r\nCamera";
            this.lblLowerCamera.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnUpperCamera_Init
            // 
            this.btnUpperCamera_Init.BackColor = System.Drawing.Color.White;
            this.btnUpperCamera_Init.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnUpperCamera_Init.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnUpperCamera_Init.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnUpperCamera_Init.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnUpperCamera_Init.ForeColor = System.Drawing.Color.Black;
            this.btnUpperCamera_Init.Location = new System.Drawing.Point(434, 4);
            this.btnUpperCamera_Init.Name = "btnUpperCamera_Init";
            this.btnUpperCamera_Init.Size = new System.Drawing.Size(131, 52);
            this.btnUpperCamera_Init.TabIndex = 145;
            this.btnUpperCamera_Init.Text = "카메라 초기화";
            this.btnUpperCamera_Init.UseVisualStyleBackColor = false;
            this.btnUpperCamera_Init.Click += new System.EventHandler(this.btnUpperCamera_Init_Click);
            // 
            // btnUpperCamera_StartLive
            // 
            this.btnUpperCamera_StartLive.BackColor = System.Drawing.Color.White;
            this.btnUpperCamera_StartLive.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnUpperCamera_StartLive.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnUpperCamera_StartLive.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnUpperCamera_StartLive.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnUpperCamera_StartLive.ForeColor = System.Drawing.Color.Black;
            this.btnUpperCamera_StartLive.Location = new System.Drawing.Point(567, 4);
            this.btnUpperCamera_StartLive.Name = "btnUpperCamera_StartLive";
            this.btnUpperCamera_StartLive.Size = new System.Drawing.Size(131, 52);
            this.btnUpperCamera_StartLive.TabIndex = 177;
            this.btnUpperCamera_StartLive.Text = "라이브 이미지";
            this.btnUpperCamera_StartLive.UseVisualStyleBackColor = false;
            this.btnUpperCamera_StartLive.Click += new System.EventHandler(this.btnUpperCamera_StartLive_Click);
            // 
            // tabControl_ParameterSet
            // 
            this.tabControl_ParameterSet.Controls.Add(this.tabPage_Data);
            this.tabControl_ParameterSet.Controls.Add(this.tabPage_Layout);
            this.tabControl_ParameterSet.Controls.Add(this.tabPage_Parameter);
            this.tabControl_ParameterSet.Controls.Add(this.tabPage_Fiducial);
            this.tabControl_ParameterSet.Controls.Add(this.tabPage_Position);
            this.tabControl_ParameterSet.Controls.Add(this.tabPage_Option);
            this.tabControl_ParameterSet.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.tabControl_ParameterSet.ItemSize = new System.Drawing.Size(130, 32);
            this.tabControl_ParameterSet.Location = new System.Drawing.Point(1091, 3);
            this.tabControl_ParameterSet.Multiline = true;
            this.tabControl_ParameterSet.Name = "tabControl_ParameterSet";
            this.tabControl_ParameterSet.SelectedIndex = 0;
            this.tabControl_ParameterSet.Size = new System.Drawing.Size(816, 764);
            this.tabControl_ParameterSet.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl_ParameterSet.TabIndex = 198;
            // 
            // tabPage_Data
            // 
            this.tabPage_Data.BackColor = System.Drawing.Color.Transparent;
            //this.tabPage_Data.Controls.Add(this.SiriusViewer_Parameter);
            this.tabPage_Data.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.tabPage_Data.Location = new System.Drawing.Point(4, 36);
            this.tabPage_Data.Name = "tabPage_Data";
            this.tabPage_Data.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Data.Size = new System.Drawing.Size(808, 724);
            this.tabPage_Data.TabIndex = 0;
            this.tabPage_Data.Text = "Data";
            // 
            // SiriusViewer_Parameter
            // 
            //this.SiriusViewer_Parameter.AliasName = "NoName";
            //this.SiriusViewer_Parameter.BackColor = System.Drawing.SystemColors.Control;
            //this.SiriusViewer_Parameter.Document = null;
            //this.SiriusViewer_Parameter.FileName = "NoName";
            //this.SiriusViewer_Parameter.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.SiriusViewer_Parameter.Index = ((uint)(0u));
            //this.SiriusViewer_Parameter.Location = new System.Drawing.Point(4, 4);
            //this.SiriusViewer_Parameter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            //this.SiriusViewer_Parameter.Name = "SiriusViewer_Parameter";
            //this.SiriusViewer_Parameter.Progress = 0;
            //this.SiriusViewer_Parameter.Size = new System.Drawing.Size(799, 716);
            //this.SiriusViewer_Parameter.TabIndex = 35;
            // 
            // tabPage_Layout
            // 
            this.tabPage_Layout.BackColor = System.Drawing.Color.Silver;
            this.tabPage_Layout.Controls.Add(this.baseGroupBox_ScanAreaSet);
            this.tabPage_Layout.Controls.Add(this.baseGroupBox_ModuleThickness_FiducialArea);
            this.tabPage_Layout.Controls.Add(this.baseGroupBox_ModuleThickness_DrillingArea);
            this.tabPage_Layout.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.tabPage_Layout.Location = new System.Drawing.Point(4, 36);
            this.tabPage_Layout.Name = "tabPage_Layout";
            this.tabPage_Layout.Size = new System.Drawing.Size(808, 724);
            this.tabPage_Layout.TabIndex = 1;
            this.tabPage_Layout.Text = "Layout";
            // 
            // baseGroupBox_ScanAreaSet
            // 
            this.baseGroupBox_ScanAreaSet.Controls.Add(this.textBox2);
            this.baseGroupBox_ScanAreaSet.Controls.Add(this.baseLabel_ScanArea_Height);
            this.baseGroupBox_ScanAreaSet.Controls.Add(this.textBox3);
            this.baseGroupBox_ScanAreaSet.Controls.Add(this.baseLabel_ScanArea_Width);
            this.baseGroupBox_ScanAreaSet.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_ScanAreaSet.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox_ScanAreaSet.Location = new System.Drawing.Point(11, 251);
            this.baseGroupBox_ScanAreaSet.Name = "baseGroupBox_ScanAreaSet";
            this.baseGroupBox_ScanAreaSet.Size = new System.Drawing.Size(229, 98);
            this.baseGroupBox_ScanAreaSet.TabIndex = 3;
            this.baseGroupBox_ScanAreaSet.TabStop = false;
            this.baseGroupBox_ScanAreaSet.Text = " [ Processing Area Size ] ";
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox2.Location = new System.Drawing.Point(90, 64);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(91, 25);
            this.textBox2.TabIndex = 124;
            // 
            // baseLabel_ScanArea_Height
            // 
            this.baseLabel_ScanArea_Height.AutoSize = true;
            this.baseLabel_ScanArea_Height.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_ScanArea_Height.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_ScanArea_Height.Location = new System.Drawing.Point(7, 67);
            this.baseLabel_ScanArea_Height.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_ScanArea_Height.Name = "baseLabel_ScanArea_Height";
            this.baseLabel_ScanArea_Height.Size = new System.Drawing.Size(81, 18);
            this.baseLabel_ScanArea_Height.TabIndex = 123;
            this.baseLabel_ScanArea_Height.Text = "Height (㎜)";
            // 
            // textBox3
            // 
            this.textBox3.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox3.Location = new System.Drawing.Point(90, 33);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(91, 25);
            this.textBox3.TabIndex = 122;
            // 
            // baseLabel_ScanArea_Width
            // 
            this.baseLabel_ScanArea_Width.AutoSize = true;
            this.baseLabel_ScanArea_Width.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_ScanArea_Width.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_ScanArea_Width.Location = new System.Drawing.Point(7, 36);
            this.baseLabel_ScanArea_Width.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_ScanArea_Width.Name = "baseLabel_ScanArea_Width";
            this.baseLabel_ScanArea_Width.Size = new System.Drawing.Size(77, 18);
            this.baseLabel_ScanArea_Width.TabIndex = 121;
            this.baseLabel_ScanArea_Width.Text = "Width (㎜)";
            // 
            // baseGroupBox_ModuleThickness_FiducialArea
            // 
            this.baseGroupBox_ModuleThickness_FiducialArea.Controls.Add(this.button_FiducialPos_PNLThickCheck_Start);
            this.baseGroupBox_ModuleThickness_FiducialArea.Controls.Add(this.textBox_FiducialPos_PNLThickness);
            this.baseGroupBox_ModuleThickness_FiducialArea.Controls.Add(this.baseLabel_FiducialPos_PNLThickness);
            this.baseGroupBox_ModuleThickness_FiducialArea.Controls.Add(this.textBox_FiducialPos_PNLThickCheck_PosY);
            this.baseGroupBox_ModuleThickness_FiducialArea.Controls.Add(this.baseLabel_FiducialPosThickCheck_PosY);
            this.baseGroupBox_ModuleThickness_FiducialArea.Controls.Add(this.textBox_FiducialPos_PNLThickCheck_PosX);
            this.baseGroupBox_ModuleThickness_FiducialArea.Controls.Add(this.baseLabel_FiducialPosThickCheck_PosX);
            this.baseGroupBox_ModuleThickness_FiducialArea.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_ModuleThickness_FiducialArea.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox_ModuleThickness_FiducialArea.Location = new System.Drawing.Point(11, 131);
            this.baseGroupBox_ModuleThickness_FiducialArea.Name = "baseGroupBox_ModuleThickness_FiducialArea";
            this.baseGroupBox_ModuleThickness_FiducialArea.Size = new System.Drawing.Size(382, 98);
            this.baseGroupBox_ModuleThickness_FiducialArea.TabIndex = 2;
            this.baseGroupBox_ModuleThickness_FiducialArea.TabStop = false;
            this.baseGroupBox_ModuleThickness_FiducialArea.Text = " [ Module Thickness   (Fiducial Position) ] ";
            // 
            // button_FiducialPos_PNLThickCheck_Start
            // 
            this.button_FiducialPos_PNLThickCheck_Start.BackColor = System.Drawing.Color.White;
            this.button_FiducialPos_PNLThickCheck_Start.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_FiducialPos_PNLThickCheck_Start.FlatAppearance.BorderSize = 2;
            this.button_FiducialPos_PNLThickCheck_Start.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_FiducialPos_PNLThickCheck_Start.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_FiducialPos_PNLThickCheck_Start.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.button_FiducialPos_PNLThickCheck_Start.ForeColor = System.Drawing.Color.Black;
            this.button_FiducialPos_PNLThickCheck_Start.Location = new System.Drawing.Point(215, 32);
            this.button_FiducialPos_PNLThickCheck_Start.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_FiducialPos_PNLThickCheck_Start.Name = "button_FiducialPos_PNLThickCheck_Start";
            this.button_FiducialPos_PNLThickCheck_Start.Size = new System.Drawing.Size(157, 27);
            this.button_FiducialPos_PNLThickCheck_Start.TabIndex = 196;
            this.button_FiducialPos_PNLThickCheck_Start.Text = "Thickness Check  Start";
            this.button_FiducialPos_PNLThickCheck_Start.UseVisualStyleBackColor = false;
            // 
            // textBox_FiducialPos_PNLThickness
            // 
            this.textBox_FiducialPos_PNLThickness.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.textBox_FiducialPos_PNLThickness.Location = new System.Drawing.Point(287, 65);
            this.textBox_FiducialPos_PNLThickness.Name = "textBox_FiducialPos_PNLThickness";
            this.textBox_FiducialPos_PNLThickness.Size = new System.Drawing.Size(85, 23);
            this.textBox_FiducialPos_PNLThickness.TabIndex = 126;
            // 
            // baseLabel_FiducialPos_PNLThickness
            // 
            this.baseLabel_FiducialPos_PNLThickness.AutoSize = true;
            this.baseLabel_FiducialPos_PNLThickness.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_FiducialPos_PNLThickness.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_FiducialPos_PNLThickness.Location = new System.Drawing.Point(212, 67);
            this.baseLabel_FiducialPos_PNLThickness.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_FiducialPos_PNLThickness.Name = "baseLabel_FiducialPos_PNLThickness";
            this.baseLabel_FiducialPos_PNLThickness.Size = new System.Drawing.Size(72, 18);
            this.baseLabel_FiducialPos_PNLThickness.TabIndex = 125;
            this.baseLabel_FiducialPos_PNLThickness.Text = "Thickness";
            // 
            // textBox_FiducialPos_PNLThickCheck_PosY
            // 
            this.textBox_FiducialPos_PNLThickCheck_PosY.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_FiducialPos_PNLThickCheck_PosY.Location = new System.Drawing.Point(60, 63);
            this.textBox_FiducialPos_PNLThickCheck_PosY.Name = "textBox_FiducialPos_PNLThickCheck_PosY";
            this.textBox_FiducialPos_PNLThickCheck_PosY.Size = new System.Drawing.Size(91, 25);
            this.textBox_FiducialPos_PNLThickCheck_PosY.TabIndex = 124;
            // 
            // baseLabel_FiducialPosThickCheck_PosY
            // 
            this.baseLabel_FiducialPosThickCheck_PosY.AutoSize = true;
            this.baseLabel_FiducialPosThickCheck_PosY.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_FiducialPosThickCheck_PosY.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_FiducialPosThickCheck_PosY.Location = new System.Drawing.Point(7, 67);
            this.baseLabel_FiducialPosThickCheck_PosY.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_FiducialPosThickCheck_PosY.Name = "baseLabel_FiducialPosThickCheck_PosY";
            this.baseLabel_FiducialPosThickCheck_PosY.Size = new System.Drawing.Size(51, 18);
            this.baseLabel_FiducialPosThickCheck_PosY.TabIndex = 123;
            this.baseLabel_FiducialPosThickCheck_PosY.Text = "Pos. Y";
            // 
            // textBox_FiducialPos_PNLThickCheck_PosX
            // 
            this.textBox_FiducialPos_PNLThickCheck_PosX.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_FiducialPos_PNLThickCheck_PosX.Location = new System.Drawing.Point(60, 32);
            this.textBox_FiducialPos_PNLThickCheck_PosX.Name = "textBox_FiducialPos_PNLThickCheck_PosX";
            this.textBox_FiducialPos_PNLThickCheck_PosX.Size = new System.Drawing.Size(91, 25);
            this.textBox_FiducialPos_PNLThickCheck_PosX.TabIndex = 122;
            // 
            // baseLabel_FiducialPosThickCheck_PosX
            // 
            this.baseLabel_FiducialPosThickCheck_PosX.AutoSize = true;
            this.baseLabel_FiducialPosThickCheck_PosX.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_FiducialPosThickCheck_PosX.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_FiducialPosThickCheck_PosX.Location = new System.Drawing.Point(7, 36);
            this.baseLabel_FiducialPosThickCheck_PosX.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_FiducialPosThickCheck_PosX.Name = "baseLabel_FiducialPosThickCheck_PosX";
            this.baseLabel_FiducialPosThickCheck_PosX.Size = new System.Drawing.Size(50, 18);
            this.baseLabel_FiducialPosThickCheck_PosX.TabIndex = 121;
            this.baseLabel_FiducialPosThickCheck_PosX.Text = "Pos. X";
            // 
            // baseGroupBox_ModuleThickness_DrillingArea
            // 
            this.baseGroupBox_ModuleThickness_DrillingArea.Controls.Add(this.button_DrillingPos_PNLThickCheck_Start);
            this.baseGroupBox_ModuleThickness_DrillingArea.Controls.Add(this.textBox_DrillingPos_PNLThickness);
            this.baseGroupBox_ModuleThickness_DrillingArea.Controls.Add(this.baseLabel_DrillingPos_PNLThickness);
            this.baseGroupBox_ModuleThickness_DrillingArea.Controls.Add(this.textBox_DrillingPos_PNLThickCheck_PosY);
            this.baseGroupBox_ModuleThickness_DrillingArea.Controls.Add(this.baseLabel_DrillingPosThickCheck_PosY);
            this.baseGroupBox_ModuleThickness_DrillingArea.Controls.Add(this.textBox_DrillingPos_PNLThickCheck_PosX);
            this.baseGroupBox_ModuleThickness_DrillingArea.Controls.Add(this.baseLabel_DrillingPosThickCheck_PosX);
            this.baseGroupBox_ModuleThickness_DrillingArea.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_ModuleThickness_DrillingArea.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox_ModuleThickness_DrillingArea.Location = new System.Drawing.Point(11, 11);
            this.baseGroupBox_ModuleThickness_DrillingArea.Name = "baseGroupBox_ModuleThickness_DrillingArea";
            this.baseGroupBox_ModuleThickness_DrillingArea.Size = new System.Drawing.Size(382, 98);
            this.baseGroupBox_ModuleThickness_DrillingArea.TabIndex = 1;
            this.baseGroupBox_ModuleThickness_DrillingArea.TabStop = false;
            this.baseGroupBox_ModuleThickness_DrillingArea.Text = " [ Module Thickness   (Drilling Position) ] ";
            // 
            // button_DrillingPos_PNLThickCheck_Start
            // 
            this.button_DrillingPos_PNLThickCheck_Start.BackColor = System.Drawing.Color.White;
            this.button_DrillingPos_PNLThickCheck_Start.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_DrillingPos_PNLThickCheck_Start.FlatAppearance.BorderSize = 2;
            this.button_DrillingPos_PNLThickCheck_Start.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_DrillingPos_PNLThickCheck_Start.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_DrillingPos_PNLThickCheck_Start.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.button_DrillingPos_PNLThickCheck_Start.ForeColor = System.Drawing.Color.Black;
            this.button_DrillingPos_PNLThickCheck_Start.Location = new System.Drawing.Point(215, 32);
            this.button_DrillingPos_PNLThickCheck_Start.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_DrillingPos_PNLThickCheck_Start.Name = "button_DrillingPos_PNLThickCheck_Start";
            this.button_DrillingPos_PNLThickCheck_Start.Size = new System.Drawing.Size(157, 27);
            this.button_DrillingPos_PNLThickCheck_Start.TabIndex = 196;
            this.button_DrillingPos_PNLThickCheck_Start.Text = "Thickness Check  Start";
            this.button_DrillingPos_PNLThickCheck_Start.UseVisualStyleBackColor = false;
            // 
            // textBox_DrillingPos_PNLThickness
            // 
            this.textBox_DrillingPos_PNLThickness.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.textBox_DrillingPos_PNLThickness.Location = new System.Drawing.Point(287, 65);
            this.textBox_DrillingPos_PNLThickness.Name = "textBox_DrillingPos_PNLThickness";
            this.textBox_DrillingPos_PNLThickness.Size = new System.Drawing.Size(85, 23);
            this.textBox_DrillingPos_PNLThickness.TabIndex = 126;
            // 
            // baseLabel_DrillingPos_PNLThickness
            // 
            this.baseLabel_DrillingPos_PNLThickness.AutoSize = true;
            this.baseLabel_DrillingPos_PNLThickness.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingPos_PNLThickness.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingPos_PNLThickness.Location = new System.Drawing.Point(212, 67);
            this.baseLabel_DrillingPos_PNLThickness.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingPos_PNLThickness.Name = "baseLabel_DrillingPos_PNLThickness";
            this.baseLabel_DrillingPos_PNLThickness.Size = new System.Drawing.Size(72, 18);
            this.baseLabel_DrillingPos_PNLThickness.TabIndex = 125;
            this.baseLabel_DrillingPos_PNLThickness.Text = "Thickness";
            // 
            // textBox_DrillingPos_PNLThickCheck_PosY
            // 
            this.textBox_DrillingPos_PNLThickCheck_PosY.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_DrillingPos_PNLThickCheck_PosY.Location = new System.Drawing.Point(60, 63);
            this.textBox_DrillingPos_PNLThickCheck_PosY.Name = "textBox_DrillingPos_PNLThickCheck_PosY";
            this.textBox_DrillingPos_PNLThickCheck_PosY.Size = new System.Drawing.Size(91, 25);
            this.textBox_DrillingPos_PNLThickCheck_PosY.TabIndex = 124;
            // 
            // baseLabel_DrillingPosThickCheck_PosY
            // 
            this.baseLabel_DrillingPosThickCheck_PosY.AutoSize = true;
            this.baseLabel_DrillingPosThickCheck_PosY.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingPosThickCheck_PosY.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingPosThickCheck_PosY.Location = new System.Drawing.Point(7, 67);
            this.baseLabel_DrillingPosThickCheck_PosY.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingPosThickCheck_PosY.Name = "baseLabel_DrillingPosThickCheck_PosY";
            this.baseLabel_DrillingPosThickCheck_PosY.Size = new System.Drawing.Size(51, 18);
            this.baseLabel_DrillingPosThickCheck_PosY.TabIndex = 123;
            this.baseLabel_DrillingPosThickCheck_PosY.Text = "Pos. Y";
            // 
            // textBox_DrillingPos_PNLThickCheck_PosX
            // 
            this.textBox_DrillingPos_PNLThickCheck_PosX.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_DrillingPos_PNLThickCheck_PosX.Location = new System.Drawing.Point(60, 32);
            this.textBox_DrillingPos_PNLThickCheck_PosX.Name = "textBox_DrillingPos_PNLThickCheck_PosX";
            this.textBox_DrillingPos_PNLThickCheck_PosX.Size = new System.Drawing.Size(91, 25);
            this.textBox_DrillingPos_PNLThickCheck_PosX.TabIndex = 122;
            // 
            // baseLabel_DrillingPosThickCheck_PosX
            // 
            this.baseLabel_DrillingPosThickCheck_PosX.AutoSize = true;
            this.baseLabel_DrillingPosThickCheck_PosX.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingPosThickCheck_PosX.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingPosThickCheck_PosX.Location = new System.Drawing.Point(7, 36);
            this.baseLabel_DrillingPosThickCheck_PosX.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingPosThickCheck_PosX.Name = "baseLabel_DrillingPosThickCheck_PosX";
            this.baseLabel_DrillingPosThickCheck_PosX.Size = new System.Drawing.Size(50, 18);
            this.baseLabel_DrillingPosThickCheck_PosX.TabIndex = 121;
            this.baseLabel_DrillingPosThickCheck_PosX.Text = "Pos. X";
            // 
            // tabPage_Parameter
            // 
            this.tabPage_Parameter.BackColor = System.Drawing.Color.Silver;
            this.tabPage_Parameter.Controls.Add(this.baseGroupBox_DrillingToolParam);
            this.tabPage_Parameter.Controls.Add(this.baseGroupBox_DrillingToolList);
            this.tabPage_Parameter.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.tabPage_Parameter.Location = new System.Drawing.Point(4, 36);
            this.tabPage_Parameter.Name = "tabPage_Parameter";
            this.tabPage_Parameter.Size = new System.Drawing.Size(808, 724);
            this.tabPage_Parameter.TabIndex = 2;
            this.tabPage_Parameter.Text = "Parameter";
            // 
            // baseGroupBox_DrillingToolParam
            // 
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_LaserOffDelay_Unit);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.textBox_DrillingTool_LaserOffDelay);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_LaserOffDelay);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_LaserOnDelay_Unit);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.textBox_DrillingTool_LaserOnDelay);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_LaserOnDelay);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_JumpDelay_Unit);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.textBox_DrillingTool_JumpDelay);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_JumpDelay);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_MarkDelay_Unit);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.textBox_DrillingTool_MarkDelay);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_MarkDelay);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_JumpSpeed_Unit);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.textBox_DrillingTool_JumpSpeed);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_JumpSpeed);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_MarkSpeed_Unit);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.textBox_DrillingTool_MarkSpeed);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_MarkSpeed);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_Freq_Unit);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.textBox_DrillingTool_Frequency);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_Freq);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_ZOffset_Unit);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.textBox_DrillingTool_ZOffset);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_ZOffset);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.textBox_DrillingTool_RepeatCount);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_RepeatCount);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_HoleSize_Unit);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.textBox_DrillingTool_HoleSize);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_HoleSize);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.comboBox_DrillingTool_MaskNo);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_MaskNo);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.comboBox_DrillingTool_Type);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_Type);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.textBox_DrillingTool_No);
            this.baseGroupBox_DrillingToolParam.Controls.Add(this.baseLabel_DrillingTool_No);
            this.baseGroupBox_DrillingToolParam.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_DrillingToolParam.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox_DrillingToolParam.Location = new System.Drawing.Point(295, 11);
            this.baseGroupBox_DrillingToolParam.Name = "baseGroupBox_DrillingToolParam";
            this.baseGroupBox_DrillingToolParam.Size = new System.Drawing.Size(501, 502);
            this.baseGroupBox_DrillingToolParam.TabIndex = 12;
            this.baseGroupBox_DrillingToolParam.TabStop = false;
            this.baseGroupBox_DrillingToolParam.Text = " [ Drilling Tool Parameter ] ";
            // 
            // baseLabel_DrillingTool_LaserOffDelay_Unit
            // 
            this.baseLabel_DrillingTool_LaserOffDelay_Unit.AutoSize = true;
            this.baseLabel_DrillingTool_LaserOffDelay_Unit.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_LaserOffDelay_Unit.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_LaserOffDelay_Unit.Location = new System.Drawing.Point(449, 222);
            this.baseLabel_DrillingTool_LaserOffDelay_Unit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_LaserOffDelay_Unit.Name = "baseLabel_DrillingTool_LaserOffDelay_Unit";
            this.baseLabel_DrillingTool_LaserOffDelay_Unit.Size = new System.Drawing.Size(23, 18);
            this.baseLabel_DrillingTool_LaserOffDelay_Unit.TabIndex = 153;
            this.baseLabel_DrillingTool_LaserOffDelay_Unit.Text = "㎲";
            // 
            // textBox_DrillingTool_LaserOffDelay
            // 
            this.textBox_DrillingTool_LaserOffDelay.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_DrillingTool_LaserOffDelay.Location = new System.Drawing.Point(355, 218);
            this.textBox_DrillingTool_LaserOffDelay.Name = "textBox_DrillingTool_LaserOffDelay";
            this.textBox_DrillingTool_LaserOffDelay.Size = new System.Drawing.Size(92, 26);
            this.textBox_DrillingTool_LaserOffDelay.TabIndex = 152;
            // 
            // baseLabel_DrillingTool_LaserOffDelay
            // 
            this.baseLabel_DrillingTool_LaserOffDelay.AutoSize = true;
            this.baseLabel_DrillingTool_LaserOffDelay.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_LaserOffDelay.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_LaserOffDelay.Location = new System.Drawing.Point(244, 222);
            this.baseLabel_DrillingTool_LaserOffDelay.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_LaserOffDelay.Name = "baseLabel_DrillingTool_LaserOffDelay";
            this.baseLabel_DrillingTool_LaserOffDelay.Size = new System.Drawing.Size(110, 18);
            this.baseLabel_DrillingTool_LaserOffDelay.TabIndex = 151;
            this.baseLabel_DrillingTool_LaserOffDelay.Text = "Laser Off Delay";
            // 
            // baseLabel_DrillingTool_LaserOnDelay_Unit
            // 
            this.baseLabel_DrillingTool_LaserOnDelay_Unit.AutoSize = true;
            this.baseLabel_DrillingTool_LaserOnDelay_Unit.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_LaserOnDelay_Unit.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_LaserOnDelay_Unit.Location = new System.Drawing.Point(449, 191);
            this.baseLabel_DrillingTool_LaserOnDelay_Unit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_LaserOnDelay_Unit.Name = "baseLabel_DrillingTool_LaserOnDelay_Unit";
            this.baseLabel_DrillingTool_LaserOnDelay_Unit.Size = new System.Drawing.Size(23, 18);
            this.baseLabel_DrillingTool_LaserOnDelay_Unit.TabIndex = 150;
            this.baseLabel_DrillingTool_LaserOnDelay_Unit.Text = "㎲";
            // 
            // textBox_DrillingTool_LaserOnDelay
            // 
            this.textBox_DrillingTool_LaserOnDelay.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_DrillingTool_LaserOnDelay.Location = new System.Drawing.Point(355, 187);
            this.textBox_DrillingTool_LaserOnDelay.Name = "textBox_DrillingTool_LaserOnDelay";
            this.textBox_DrillingTool_LaserOnDelay.Size = new System.Drawing.Size(92, 26);
            this.textBox_DrillingTool_LaserOnDelay.TabIndex = 149;
            // 
            // baseLabel_DrillingTool_LaserOnDelay
            // 
            this.baseLabel_DrillingTool_LaserOnDelay.AutoSize = true;
            this.baseLabel_DrillingTool_LaserOnDelay.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_LaserOnDelay.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_LaserOnDelay.Location = new System.Drawing.Point(244, 191);
            this.baseLabel_DrillingTool_LaserOnDelay.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_LaserOnDelay.Name = "baseLabel_DrillingTool_LaserOnDelay";
            this.baseLabel_DrillingTool_LaserOnDelay.Size = new System.Drawing.Size(108, 18);
            this.baseLabel_DrillingTool_LaserOnDelay.TabIndex = 148;
            this.baseLabel_DrillingTool_LaserOnDelay.Text = "Laser On Delay";
            // 
            // baseLabel_DrillingTool_JumpDelay_Unit
            // 
            this.baseLabel_DrillingTool_JumpDelay_Unit.AutoSize = true;
            this.baseLabel_DrillingTool_JumpDelay_Unit.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_JumpDelay_Unit.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_JumpDelay_Unit.Location = new System.Drawing.Point(449, 160);
            this.baseLabel_DrillingTool_JumpDelay_Unit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_JumpDelay_Unit.Name = "baseLabel_DrillingTool_JumpDelay_Unit";
            this.baseLabel_DrillingTool_JumpDelay_Unit.Size = new System.Drawing.Size(23, 18);
            this.baseLabel_DrillingTool_JumpDelay_Unit.TabIndex = 147;
            this.baseLabel_DrillingTool_JumpDelay_Unit.Text = "㎲";
            // 
            // textBox_DrillingTool_JumpDelay
            // 
            this.textBox_DrillingTool_JumpDelay.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_DrillingTool_JumpDelay.Location = new System.Drawing.Point(355, 156);
            this.textBox_DrillingTool_JumpDelay.Name = "textBox_DrillingTool_JumpDelay";
            this.textBox_DrillingTool_JumpDelay.Size = new System.Drawing.Size(92, 26);
            this.textBox_DrillingTool_JumpDelay.TabIndex = 146;
            // 
            // baseLabel_DrillingTool_JumpDelay
            // 
            this.baseLabel_DrillingTool_JumpDelay.AutoSize = true;
            this.baseLabel_DrillingTool_JumpDelay.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_JumpDelay.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_JumpDelay.Location = new System.Drawing.Point(244, 160);
            this.baseLabel_DrillingTool_JumpDelay.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_JumpDelay.Name = "baseLabel_DrillingTool_JumpDelay";
            this.baseLabel_DrillingTool_JumpDelay.Size = new System.Drawing.Size(84, 18);
            this.baseLabel_DrillingTool_JumpDelay.TabIndex = 145;
            this.baseLabel_DrillingTool_JumpDelay.Text = "Jump Delay";
            // 
            // baseLabel_DrillingTool_MarkDelay_Unit
            // 
            this.baseLabel_DrillingTool_MarkDelay_Unit.AutoSize = true;
            this.baseLabel_DrillingTool_MarkDelay_Unit.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_MarkDelay_Unit.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_MarkDelay_Unit.Location = new System.Drawing.Point(449, 129);
            this.baseLabel_DrillingTool_MarkDelay_Unit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_MarkDelay_Unit.Name = "baseLabel_DrillingTool_MarkDelay_Unit";
            this.baseLabel_DrillingTool_MarkDelay_Unit.Size = new System.Drawing.Size(23, 18);
            this.baseLabel_DrillingTool_MarkDelay_Unit.TabIndex = 144;
            this.baseLabel_DrillingTool_MarkDelay_Unit.Text = "㎲";
            // 
            // textBox_DrillingTool_MarkDelay
            // 
            this.textBox_DrillingTool_MarkDelay.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_DrillingTool_MarkDelay.Location = new System.Drawing.Point(355, 125);
            this.textBox_DrillingTool_MarkDelay.Name = "textBox_DrillingTool_MarkDelay";
            this.textBox_DrillingTool_MarkDelay.Size = new System.Drawing.Size(92, 26);
            this.textBox_DrillingTool_MarkDelay.TabIndex = 143;
            // 
            // baseLabel_DrillingTool_MarkDelay
            // 
            this.baseLabel_DrillingTool_MarkDelay.AutoSize = true;
            this.baseLabel_DrillingTool_MarkDelay.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_MarkDelay.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_MarkDelay.Location = new System.Drawing.Point(244, 129);
            this.baseLabel_DrillingTool_MarkDelay.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_MarkDelay.Name = "baseLabel_DrillingTool_MarkDelay";
            this.baseLabel_DrillingTool_MarkDelay.Size = new System.Drawing.Size(81, 18);
            this.baseLabel_DrillingTool_MarkDelay.TabIndex = 142;
            this.baseLabel_DrillingTool_MarkDelay.Text = "Mark Delay";
            // 
            // baseLabel_DrillingTool_JumpSpeed_Unit
            // 
            this.baseLabel_DrillingTool_JumpSpeed_Unit.AutoSize = true;
            this.baseLabel_DrillingTool_JumpSpeed_Unit.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_JumpSpeed_Unit.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_JumpSpeed_Unit.Location = new System.Drawing.Point(449, 98);
            this.baseLabel_DrillingTool_JumpSpeed_Unit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_JumpSpeed_Unit.Name = "baseLabel_DrillingTool_JumpSpeed_Unit";
            this.baseLabel_DrillingTool_JumpSpeed_Unit.Size = new System.Drawing.Size(47, 18);
            this.baseLabel_DrillingTool_JumpSpeed_Unit.TabIndex = 141;
            this.baseLabel_DrillingTool_JumpSpeed_Unit.Text = "mm/s";
            // 
            // textBox_DrillingTool_JumpSpeed
            // 
            this.textBox_DrillingTool_JumpSpeed.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_DrillingTool_JumpSpeed.Location = new System.Drawing.Point(355, 94);
            this.textBox_DrillingTool_JumpSpeed.Name = "textBox_DrillingTool_JumpSpeed";
            this.textBox_DrillingTool_JumpSpeed.Size = new System.Drawing.Size(92, 26);
            this.textBox_DrillingTool_JumpSpeed.TabIndex = 140;
            // 
            // baseLabel_DrillingTool_JumpSpeed
            // 
            this.baseLabel_DrillingTool_JumpSpeed.AutoSize = true;
            this.baseLabel_DrillingTool_JumpSpeed.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_JumpSpeed.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_JumpSpeed.Location = new System.Drawing.Point(244, 98);
            this.baseLabel_DrillingTool_JumpSpeed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_JumpSpeed.Name = "baseLabel_DrillingTool_JumpSpeed";
            this.baseLabel_DrillingTool_JumpSpeed.Size = new System.Drawing.Size(88, 18);
            this.baseLabel_DrillingTool_JumpSpeed.TabIndex = 139;
            this.baseLabel_DrillingTool_JumpSpeed.Text = "Jump Speed";
            // 
            // baseLabel_DrillingTool_MarkSpeed_Unit
            // 
            this.baseLabel_DrillingTool_MarkSpeed_Unit.AutoSize = true;
            this.baseLabel_DrillingTool_MarkSpeed_Unit.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_MarkSpeed_Unit.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_MarkSpeed_Unit.Location = new System.Drawing.Point(449, 67);
            this.baseLabel_DrillingTool_MarkSpeed_Unit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_MarkSpeed_Unit.Name = "baseLabel_DrillingTool_MarkSpeed_Unit";
            this.baseLabel_DrillingTool_MarkSpeed_Unit.Size = new System.Drawing.Size(47, 18);
            this.baseLabel_DrillingTool_MarkSpeed_Unit.TabIndex = 138;
            this.baseLabel_DrillingTool_MarkSpeed_Unit.Text = "mm/s";
            // 
            // textBox_DrillingTool_MarkSpeed
            // 
            this.textBox_DrillingTool_MarkSpeed.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_DrillingTool_MarkSpeed.Location = new System.Drawing.Point(355, 63);
            this.textBox_DrillingTool_MarkSpeed.Name = "textBox_DrillingTool_MarkSpeed";
            this.textBox_DrillingTool_MarkSpeed.Size = new System.Drawing.Size(92, 26);
            this.textBox_DrillingTool_MarkSpeed.TabIndex = 137;
            // 
            // baseLabel_DrillingTool_MarkSpeed
            // 
            this.baseLabel_DrillingTool_MarkSpeed.AutoSize = true;
            this.baseLabel_DrillingTool_MarkSpeed.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_MarkSpeed.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_MarkSpeed.Location = new System.Drawing.Point(244, 67);
            this.baseLabel_DrillingTool_MarkSpeed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_MarkSpeed.Name = "baseLabel_DrillingTool_MarkSpeed";
            this.baseLabel_DrillingTool_MarkSpeed.Size = new System.Drawing.Size(85, 18);
            this.baseLabel_DrillingTool_MarkSpeed.TabIndex = 136;
            this.baseLabel_DrillingTool_MarkSpeed.Text = "Mark Speed";
            // 
            // baseLabel_DrillingTool_Freq_Unit
            // 
            this.baseLabel_DrillingTool_Freq_Unit.AutoSize = true;
            this.baseLabel_DrillingTool_Freq_Unit.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_Freq_Unit.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_Freq_Unit.Location = new System.Drawing.Point(449, 36);
            this.baseLabel_DrillingTool_Freq_Unit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_Freq_Unit.Name = "baseLabel_DrillingTool_Freq_Unit";
            this.baseLabel_DrillingTool_Freq_Unit.Size = new System.Drawing.Size(23, 18);
            this.baseLabel_DrillingTool_Freq_Unit.TabIndex = 135;
            this.baseLabel_DrillingTool_Freq_Unit.Text = "㎐";
            // 
            // textBox_DrillingTool_Frequency
            // 
            this.textBox_DrillingTool_Frequency.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_DrillingTool_Frequency.Location = new System.Drawing.Point(355, 32);
            this.textBox_DrillingTool_Frequency.Name = "textBox_DrillingTool_Frequency";
            this.textBox_DrillingTool_Frequency.Size = new System.Drawing.Size(92, 26);
            this.textBox_DrillingTool_Frequency.TabIndex = 134;
            // 
            // baseLabel_DrillingTool_Freq
            // 
            this.baseLabel_DrillingTool_Freq.AutoSize = true;
            this.baseLabel_DrillingTool_Freq.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_Freq.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_Freq.Location = new System.Drawing.Point(244, 36);
            this.baseLabel_DrillingTool_Freq.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_Freq.Name = "baseLabel_DrillingTool_Freq";
            this.baseLabel_DrillingTool_Freq.Size = new System.Drawing.Size(76, 18);
            this.baseLabel_DrillingTool_Freq.TabIndex = 133;
            this.baseLabel_DrillingTool_Freq.Text = "Frequency";
            // 
            // baseLabel_DrillingTool_ZOffset_Unit
            // 
            this.baseLabel_DrillingTool_ZOffset_Unit.AutoSize = true;
            this.baseLabel_DrillingTool_ZOffset_Unit.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_ZOffset_Unit.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_ZOffset_Unit.Location = new System.Drawing.Point(193, 191);
            this.baseLabel_DrillingTool_ZOffset_Unit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_ZOffset_Unit.Name = "baseLabel_DrillingTool_ZOffset_Unit";
            this.baseLabel_DrillingTool_ZOffset_Unit.Size = new System.Drawing.Size(23, 18);
            this.baseLabel_DrillingTool_ZOffset_Unit.TabIndex = 132;
            this.baseLabel_DrillingTool_ZOffset_Unit.Text = "㎜";
            // 
            // textBox_DrillingTool_ZOffset
            // 
            this.textBox_DrillingTool_ZOffset.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_DrillingTool_ZOffset.Location = new System.Drawing.Point(107, 187);
            this.textBox_DrillingTool_ZOffset.Name = "textBox_DrillingTool_ZOffset";
            this.textBox_DrillingTool_ZOffset.Size = new System.Drawing.Size(84, 26);
            this.textBox_DrillingTool_ZOffset.TabIndex = 131;
            // 
            // baseLabel_DrillingTool_ZOffset
            // 
            this.baseLabel_DrillingTool_ZOffset.AutoSize = true;
            this.baseLabel_DrillingTool_ZOffset.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_ZOffset.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_ZOffset.Location = new System.Drawing.Point(7, 191);
            this.baseLabel_DrillingTool_ZOffset.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_ZOffset.Name = "baseLabel_DrillingTool_ZOffset";
            this.baseLabel_DrillingTool_ZOffset.Size = new System.Drawing.Size(72, 18);
            this.baseLabel_DrillingTool_ZOffset.TabIndex = 130;
            this.baseLabel_DrillingTool_ZOffset.Text = "Z - Offset";
            // 
            // textBox_DrillingTool_RepeatCount
            // 
            this.textBox_DrillingTool_RepeatCount.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_DrillingTool_RepeatCount.Location = new System.Drawing.Point(107, 156);
            this.textBox_DrillingTool_RepeatCount.Name = "textBox_DrillingTool_RepeatCount";
            this.textBox_DrillingTool_RepeatCount.Size = new System.Drawing.Size(84, 26);
            this.textBox_DrillingTool_RepeatCount.TabIndex = 129;
            // 
            // baseLabel_DrillingTool_RepeatCount
            // 
            this.baseLabel_DrillingTool_RepeatCount.AutoSize = true;
            this.baseLabel_DrillingTool_RepeatCount.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_RepeatCount.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_RepeatCount.Location = new System.Drawing.Point(7, 160);
            this.baseLabel_DrillingTool_RepeatCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_RepeatCount.Name = "baseLabel_DrillingTool_RepeatCount";
            this.baseLabel_DrillingTool_RepeatCount.Size = new System.Drawing.Size(97, 18);
            this.baseLabel_DrillingTool_RepeatCount.TabIndex = 128;
            this.baseLabel_DrillingTool_RepeatCount.Text = "Repeat Count";
            // 
            // baseLabel_DrillingTool_HoleSize_Unit
            // 
            this.baseLabel_DrillingTool_HoleSize_Unit.AutoSize = true;
            this.baseLabel_DrillingTool_HoleSize_Unit.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_HoleSize_Unit.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_HoleSize_Unit.Location = new System.Drawing.Point(193, 129);
            this.baseLabel_DrillingTool_HoleSize_Unit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_HoleSize_Unit.Name = "baseLabel_DrillingTool_HoleSize_Unit";
            this.baseLabel_DrillingTool_HoleSize_Unit.Size = new System.Drawing.Size(23, 18);
            this.baseLabel_DrillingTool_HoleSize_Unit.TabIndex = 127;
            this.baseLabel_DrillingTool_HoleSize_Unit.Text = "㎜";
            // 
            // textBox_DrillingTool_HoleSize
            // 
            this.textBox_DrillingTool_HoleSize.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_DrillingTool_HoleSize.Location = new System.Drawing.Point(107, 125);
            this.textBox_DrillingTool_HoleSize.Name = "textBox_DrillingTool_HoleSize";
            this.textBox_DrillingTool_HoleSize.Size = new System.Drawing.Size(84, 26);
            this.textBox_DrillingTool_HoleSize.TabIndex = 126;
            // 
            // baseLabel_DrillingTool_HoleSize
            // 
            this.baseLabel_DrillingTool_HoleSize.AutoSize = true;
            this.baseLabel_DrillingTool_HoleSize.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_HoleSize.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_HoleSize.Location = new System.Drawing.Point(7, 129);
            this.baseLabel_DrillingTool_HoleSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_HoleSize.Name = "baseLabel_DrillingTool_HoleSize";
            this.baseLabel_DrillingTool_HoleSize.Size = new System.Drawing.Size(66, 18);
            this.baseLabel_DrillingTool_HoleSize.TabIndex = 125;
            this.baseLabel_DrillingTool_HoleSize.Text = "Hole Size";
            // 
            // comboBox_DrillingTool_MaskNo
            // 
            this.comboBox_DrillingTool_MaskNo.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_DrillingTool_MaskNo.FormattingEnabled = true;
            this.comboBox_DrillingTool_MaskNo.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4"});
            this.comboBox_DrillingTool_MaskNo.Location = new System.Drawing.Point(107, 94);
            this.comboBox_DrillingTool_MaskNo.Name = "comboBox_DrillingTool_MaskNo";
            this.comboBox_DrillingTool_MaskNo.Size = new System.Drawing.Size(84, 26);
            this.comboBox_DrillingTool_MaskNo.TabIndex = 124;
            this.comboBox_DrillingTool_MaskNo.Text = "0";
            // 
            // baseLabel_DrillingTool_MaskNo
            // 
            this.baseLabel_DrillingTool_MaskNo.AutoSize = true;
            this.baseLabel_DrillingTool_MaskNo.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_MaskNo.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_MaskNo.Location = new System.Drawing.Point(7, 97);
            this.baseLabel_DrillingTool_MaskNo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_MaskNo.Name = "baseLabel_DrillingTool_MaskNo";
            this.baseLabel_DrillingTool_MaskNo.Size = new System.Drawing.Size(70, 18);
            this.baseLabel_DrillingTool_MaskNo.TabIndex = 123;
            this.baseLabel_DrillingTool_MaskNo.Text = "Mask No.";
            // 
            // comboBox_DrillingTool_Type
            // 
            this.comboBox_DrillingTool_Type.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_DrillingTool_Type.FormattingEnabled = true;
            this.comboBox_DrillingTool_Type.Items.AddRange(new object[] {
            "Shot",
            "Circle",
            "Line"});
            this.comboBox_DrillingTool_Type.Location = new System.Drawing.Point(107, 63);
            this.comboBox_DrillingTool_Type.Name = "comboBox_DrillingTool_Type";
            this.comboBox_DrillingTool_Type.Size = new System.Drawing.Size(84, 26);
            this.comboBox_DrillingTool_Type.TabIndex = 122;
            this.comboBox_DrillingTool_Type.Text = "Circle";
            // 
            // baseLabel_DrillingTool_Type
            // 
            this.baseLabel_DrillingTool_Type.AutoSize = true;
            this.baseLabel_DrillingTool_Type.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_Type.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_Type.Location = new System.Drawing.Point(7, 66);
            this.baseLabel_DrillingTool_Type.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_Type.Name = "baseLabel_DrillingTool_Type";
            this.baseLabel_DrillingTool_Type.Size = new System.Drawing.Size(75, 18);
            this.baseLabel_DrillingTool_Type.TabIndex = 121;
            this.baseLabel_DrillingTool_Type.Text = "Tool Type";
            // 
            // textBox_DrillingTool_No
            // 
            this.textBox_DrillingTool_No.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_DrillingTool_No.Location = new System.Drawing.Point(107, 32);
            this.textBox_DrillingTool_No.Name = "textBox_DrillingTool_No";
            this.textBox_DrillingTool_No.ReadOnly = true;
            this.textBox_DrillingTool_No.Size = new System.Drawing.Size(84, 26);
            this.textBox_DrillingTool_No.TabIndex = 120;
            // 
            // baseLabel_DrillingTool_No
            // 
            this.baseLabel_DrillingTool_No.AutoSize = true;
            this.baseLabel_DrillingTool_No.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_DrillingTool_No.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_DrillingTool_No.Location = new System.Drawing.Point(7, 36);
            this.baseLabel_DrillingTool_No.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_DrillingTool_No.Name = "baseLabel_DrillingTool_No";
            this.baseLabel_DrillingTool_No.Size = new System.Drawing.Size(64, 18);
            this.baseLabel_DrillingTool_No.TabIndex = 119;
            this.baseLabel_DrillingTool_No.Text = "Tool No.";
            // 
            // baseGroupBox_DrillingToolList
            // 
            this.baseGroupBox_DrillingToolList.Controls.Add(this.button_DrillingMainTool_Save);
            this.baseGroupBox_DrillingToolList.Controls.Add(this.button_DrillingMainTool_Open);
            this.baseGroupBox_DrillingToolList.Controls.Add(this.button_DrillingTool_Delete);
            this.baseGroupBox_DrillingToolList.Controls.Add(this.button_DrillingTool_Add);
            this.baseGroupBox_DrillingToolList.Controls.Add(this.treeView_DrillingTool);
            this.baseGroupBox_DrillingToolList.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_DrillingToolList.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox_DrillingToolList.Location = new System.Drawing.Point(11, 11);
            this.baseGroupBox_DrillingToolList.Name = "baseGroupBox_DrillingToolList";
            this.baseGroupBox_DrillingToolList.Size = new System.Drawing.Size(263, 502);
            this.baseGroupBox_DrillingToolList.TabIndex = 11;
            this.baseGroupBox_DrillingToolList.TabStop = false;
            this.baseGroupBox_DrillingToolList.Text = " [ Drilling Tool List ] ";
            // 
            // button_DrillingMainTool_Save
            // 
            this.button_DrillingMainTool_Save.BackColor = System.Drawing.Color.White;
            this.button_DrillingMainTool_Save.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_DrillingMainTool_Save.FlatAppearance.BorderSize = 2;
            this.button_DrillingMainTool_Save.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_DrillingMainTool_Save.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_DrillingMainTool_Save.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_DrillingMainTool_Save.ForeColor = System.Drawing.Color.Black;
            this.button_DrillingMainTool_Save.Location = new System.Drawing.Point(137, 33);
            this.button_DrillingMainTool_Save.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_DrillingMainTool_Save.Name = "button_DrillingMainTool_Save";
            this.button_DrillingMainTool_Save.Size = new System.Drawing.Size(116, 51);
            this.button_DrillingMainTool_Save.TabIndex = 200;
            this.button_DrillingMainTool_Save.Text = "Main Tool\r\nSave";
            this.button_DrillingMainTool_Save.UseVisualStyleBackColor = false;
            // 
            // button_DrillingMainTool_Open
            // 
            this.button_DrillingMainTool_Open.BackColor = System.Drawing.Color.White;
            this.button_DrillingMainTool_Open.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_DrillingMainTool_Open.FlatAppearance.BorderSize = 2;
            this.button_DrillingMainTool_Open.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_DrillingMainTool_Open.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_DrillingMainTool_Open.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_DrillingMainTool_Open.ForeColor = System.Drawing.Color.Black;
            this.button_DrillingMainTool_Open.Location = new System.Drawing.Point(9, 34);
            this.button_DrillingMainTool_Open.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_DrillingMainTool_Open.Name = "button_DrillingMainTool_Open";
            this.button_DrillingMainTool_Open.Size = new System.Drawing.Size(116, 51);
            this.button_DrillingMainTool_Open.TabIndex = 199;
            this.button_DrillingMainTool_Open.Text = "Main Tool Open";
            this.button_DrillingMainTool_Open.UseVisualStyleBackColor = false;
            // 
            // button_DrillingTool_Delete
            // 
            this.button_DrillingTool_Delete.BackColor = System.Drawing.Color.White;
            this.button_DrillingTool_Delete.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_DrillingTool_Delete.FlatAppearance.BorderSize = 2;
            this.button_DrillingTool_Delete.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_DrillingTool_Delete.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_DrillingTool_Delete.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.button_DrillingTool_Delete.ForeColor = System.Drawing.Color.Black;
            this.button_DrillingTool_Delete.Location = new System.Drawing.Point(137, 451);
            this.button_DrillingTool_Delete.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_DrillingTool_Delete.Name = "button_DrillingTool_Delete";
            this.button_DrillingTool_Delete.Size = new System.Drawing.Size(116, 41);
            this.button_DrillingTool_Delete.TabIndex = 198;
            this.button_DrillingTool_Delete.Text = "Sub-Tool  Del.";
            this.button_DrillingTool_Delete.UseVisualStyleBackColor = false;
            // 
            // button_DrillingTool_Add
            // 
            this.button_DrillingTool_Add.BackColor = System.Drawing.Color.White;
            this.button_DrillingTool_Add.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_DrillingTool_Add.FlatAppearance.BorderSize = 2;
            this.button_DrillingTool_Add.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_DrillingTool_Add.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_DrillingTool_Add.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.button_DrillingTool_Add.ForeColor = System.Drawing.Color.Black;
            this.button_DrillingTool_Add.Location = new System.Drawing.Point(9, 451);
            this.button_DrillingTool_Add.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_DrillingTool_Add.Name = "button_DrillingTool_Add";
            this.button_DrillingTool_Add.Size = new System.Drawing.Size(116, 41);
            this.button_DrillingTool_Add.TabIndex = 197;
            this.button_DrillingTool_Add.Text = "Sub-Tool  Add";
            this.button_DrillingTool_Add.UseVisualStyleBackColor = false;
            // 
            // treeView_DrillingTool
            // 
            this.treeView_DrillingTool.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.treeView_DrillingTool.Location = new System.Drawing.Point(9, 98);
            this.treeView_DrillingTool.Name = "treeView_DrillingTool";
            this.treeView_DrillingTool.Size = new System.Drawing.Size(244, 347);
            this.treeView_DrillingTool.TabIndex = 1;
            // 
            // tabPage_Fiducial
            // 
            this.tabPage_Fiducial.BackColor = System.Drawing.Color.Silver;
            this.tabPage_Fiducial.Controls.Add(this.baseGroupBox_SearchResult);
            this.tabPage_Fiducial.Controls.Add(this.baseGroupBox_Light);
            this.tabPage_Fiducial.Controls.Add(this.baseGroupBox_FiducialModel);
            this.tabPage_Fiducial.Controls.Add(this.baseGroupBox_FiducialList);
            this.tabPage_Fiducial.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.tabPage_Fiducial.Location = new System.Drawing.Point(4, 36);
            this.tabPage_Fiducial.Name = "tabPage_Fiducial";
            this.tabPage_Fiducial.Size = new System.Drawing.Size(808, 724);
            this.tabPage_Fiducial.TabIndex = 3;
            this.tabPage_Fiducial.Text = "Fiducial";
            // 
            // baseGroupBox_SearchResult
            // 
            this.baseGroupBox_SearchResult.Controls.Add(this.baseLabel1);
            this.baseGroupBox_SearchResult.Controls.Add(this.textBox_SearchResult_Size);
            this.baseGroupBox_SearchResult.Controls.Add(this.baseLabel_SearchResult_Size);
            this.baseGroupBox_SearchResult.Controls.Add(this.baseLabel_MinScore_Unit);
            this.baseGroupBox_SearchResult.Controls.Add(this.textBox_SearchResult_MinScore);
            this.baseGroupBox_SearchResult.Controls.Add(this.baseLabel_SearchResult_MinScore);
            this.baseGroupBox_SearchResult.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_SearchResult.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox_SearchResult.Location = new System.Drawing.Point(295, 317);
            this.baseGroupBox_SearchResult.Name = "baseGroupBox_SearchResult";
            this.baseGroupBox_SearchResult.Size = new System.Drawing.Size(437, 104);
            this.baseGroupBox_SearchResult.TabIndex = 15;
            this.baseGroupBox_SearchResult.TabStop = false;
            this.baseGroupBox_SearchResult.Text = " [ Search Result ] ";
            // 
            // baseLabel1
            // 
            this.baseLabel1.AutoSize = true;
            this.baseLabel1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel1.ForeColor = System.Drawing.Color.Black;
            this.baseLabel1.Location = new System.Drawing.Point(179, 67);
            this.baseLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel1.Name = "baseLabel1";
            this.baseLabel1.Size = new System.Drawing.Size(23, 18);
            this.baseLabel1.TabIndex = 133;
            this.baseLabel1.Text = "%";
            // 
            // textBox_SearchResult_Size
            // 
            this.textBox_SearchResult_Size.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_SearchResult_Size.Location = new System.Drawing.Point(85, 63);
            this.textBox_SearchResult_Size.Name = "textBox_SearchResult_Size";
            this.textBox_SearchResult_Size.Size = new System.Drawing.Size(92, 26);
            this.textBox_SearchResult_Size.TabIndex = 132;
            // 
            // baseLabel_SearchResult_Size
            // 
            this.baseLabel_SearchResult_Size.AutoSize = true;
            this.baseLabel_SearchResult_Size.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_SearchResult_Size.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_SearchResult_Size.Location = new System.Drawing.Point(7, 67);
            this.baseLabel_SearchResult_Size.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_SearchResult_Size.Name = "baseLabel_SearchResult_Size";
            this.baseLabel_SearchResult_Size.Size = new System.Drawing.Size(66, 18);
            this.baseLabel_SearchResult_Size.TabIndex = 131;
            this.baseLabel_SearchResult_Size.Text = "Size  (±)";
            // 
            // baseLabel_MinScore_Unit
            // 
            this.baseLabel_MinScore_Unit.AutoSize = true;
            this.baseLabel_MinScore_Unit.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_MinScore_Unit.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_MinScore_Unit.Location = new System.Drawing.Point(179, 36);
            this.baseLabel_MinScore_Unit.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_MinScore_Unit.Name = "baseLabel_MinScore_Unit";
            this.baseLabel_MinScore_Unit.Size = new System.Drawing.Size(23, 18);
            this.baseLabel_MinScore_Unit.TabIndex = 130;
            this.baseLabel_MinScore_Unit.Text = "%";
            // 
            // textBox_SearchResult_MinScore
            // 
            this.textBox_SearchResult_MinScore.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_SearchResult_MinScore.Location = new System.Drawing.Point(85, 32);
            this.textBox_SearchResult_MinScore.Name = "textBox_SearchResult_MinScore";
            this.textBox_SearchResult_MinScore.Size = new System.Drawing.Size(92, 26);
            this.textBox_SearchResult_MinScore.TabIndex = 129;
            // 
            // baseLabel_SearchResult_MinScore
            // 
            this.baseLabel_SearchResult_MinScore.AutoSize = true;
            this.baseLabel_SearchResult_MinScore.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_SearchResult_MinScore.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_SearchResult_MinScore.Location = new System.Drawing.Point(7, 36);
            this.baseLabel_SearchResult_MinScore.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_SearchResult_MinScore.Name = "baseLabel_SearchResult_MinScore";
            this.baseLabel_SearchResult_MinScore.Size = new System.Drawing.Size(76, 18);
            this.baseLabel_SearchResult_MinScore.TabIndex = 128;
            this.baseLabel_SearchResult_MinScore.Text = "Min. Score";
            // 
            // baseGroupBox_Light
            // 
            this.baseGroupBox_Light.Controls.Add(this.trackBar_HighResVision_Light);
            this.baseGroupBox_Light.Controls.Add(this.trackBar_LowResVision_Light);
            this.baseGroupBox_Light.Controls.Add(this.numericUpDown_HighResVision_Light);
            this.baseGroupBox_Light.Controls.Add(this.baseLabel_HighRes_Light);
            this.baseGroupBox_Light.Controls.Add(this.numericUpDown_LowResVision_Light);
            this.baseGroupBox_Light.Controls.Add(this.baseLabel_LowRes_Light);
            this.baseGroupBox_Light.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_Light.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox_Light.Location = new System.Drawing.Point(295, 192);
            this.baseGroupBox_Light.Name = "baseGroupBox_Light";
            this.baseGroupBox_Light.Size = new System.Drawing.Size(437, 104);
            this.baseGroupBox_Light.TabIndex = 14;
            this.baseGroupBox_Light.TabStop = false;
            this.baseGroupBox_Light.Text = " [ Light ] ";
            // 
            // trackBar_HighResVision_Light
            // 
            this.trackBar_HighResVision_Light.AutoSize = false;
            this.trackBar_HighResVision_Light.Location = new System.Drawing.Point(196, 65);
            this.trackBar_HighResVision_Light.Maximum = 4095;
            this.trackBar_HighResVision_Light.Name = "trackBar_HighResVision_Light";
            this.trackBar_HighResVision_Light.Size = new System.Drawing.Size(231, 29);
            this.trackBar_HighResVision_Light.TabIndex = 126;
            this.trackBar_HighResVision_Light.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            // 
            // trackBar_LowResVision_Light
            // 
            this.trackBar_LowResVision_Light.AutoSize = false;
            this.trackBar_LowResVision_Light.Location = new System.Drawing.Point(196, 30);
            this.trackBar_LowResVision_Light.Maximum = 4095;
            this.trackBar_LowResVision_Light.Name = "trackBar_LowResVision_Light";
            this.trackBar_LowResVision_Light.Size = new System.Drawing.Size(231, 29);
            this.trackBar_LowResVision_Light.TabIndex = 125;
            this.trackBar_LowResVision_Light.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            // 
            // numericUpDown_HighResVision_Light
            // 
            this.numericUpDown_HighResVision_Light.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDown_HighResVision_Light.Location = new System.Drawing.Point(120, 65);
            this.numericUpDown_HighResVision_Light.Name = "numericUpDown_HighResVision_Light";
            this.numericUpDown_HighResVision_Light.Size = new System.Drawing.Size(69, 26);
            this.numericUpDown_HighResVision_Light.TabIndex = 124;
            // 
            // baseLabel_HighRes_Light
            // 
            this.baseLabel_HighRes_Light.AutoSize = true;
            this.baseLabel_HighRes_Light.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_HighRes_Light.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_HighRes_Light.Location = new System.Drawing.Point(7, 69);
            this.baseLabel_HighRes_Light.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_HighRes_Light.Name = "baseLabel_HighRes_Light";
            this.baseLabel_HighRes_Light.Size = new System.Drawing.Size(110, 18);
            this.baseLabel_HighRes_Light.TabIndex = 123;
            this.baseLabel_HighRes_Light.Text = "High-Res. Cam.";
            // 
            // numericUpDown_LowResVision_Light
            // 
            this.numericUpDown_LowResVision_Light.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDown_LowResVision_Light.Location = new System.Drawing.Point(119, 30);
            this.numericUpDown_LowResVision_Light.Name = "numericUpDown_LowResVision_Light";
            this.numericUpDown_LowResVision_Light.Size = new System.Drawing.Size(70, 26);
            this.numericUpDown_LowResVision_Light.TabIndex = 122;
            // 
            // baseLabel_LowRes_Light
            // 
            this.baseLabel_LowRes_Light.AutoSize = true;
            this.baseLabel_LowRes_Light.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_LowRes_Light.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LowRes_Light.Location = new System.Drawing.Point(7, 34);
            this.baseLabel_LowRes_Light.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LowRes_Light.Name = "baseLabel_LowRes_Light";
            this.baseLabel_LowRes_Light.Size = new System.Drawing.Size(107, 18);
            this.baseLabel_LowRes_Light.TabIndex = 121;
            this.baseLabel_LowRes_Light.Text = "Low-Res. Cam.";
            // 
            // baseGroupBox_FiducialModel
            // 
            this.baseGroupBox_FiducialModel.Controls.Add(this.comboBox_Polarity);
            this.baseGroupBox_FiducialModel.Controls.Add(this.baseLabel_Polarity);
            this.baseGroupBox_FiducialModel.Controls.Add(this.pictureBox_FiducialModel_Image);
            this.baseGroupBox_FiducialModel.Controls.Add(this.baseLabel21);
            this.baseGroupBox_FiducialModel.Controls.Add(this.textBox12);
            this.baseGroupBox_FiducialModel.Controls.Add(this.baseLabel22);
            this.baseGroupBox_FiducialModel.Controls.Add(this.baseLabel19);
            this.baseGroupBox_FiducialModel.Controls.Add(this.textBox11);
            this.baseGroupBox_FiducialModel.Controls.Add(this.baseLabel20);
            this.baseGroupBox_FiducialModel.Controls.Add(this.baseLabel18);
            this.baseGroupBox_FiducialModel.Controls.Add(this.textBox10);
            this.baseGroupBox_FiducialModel.Controls.Add(this.baseLabel_FiducialSize_A);
            this.baseGroupBox_FiducialModel.Controls.Add(this.comboBox_FiducialType);
            this.baseGroupBox_FiducialModel.Controls.Add(this.baseLabel_FiducialType);
            this.baseGroupBox_FiducialModel.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_FiducialModel.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox_FiducialModel.Location = new System.Drawing.Point(295, 11);
            this.baseGroupBox_FiducialModel.Name = "baseGroupBox_FiducialModel";
            this.baseGroupBox_FiducialModel.Size = new System.Drawing.Size(437, 160);
            this.baseGroupBox_FiducialModel.TabIndex = 13;
            this.baseGroupBox_FiducialModel.TabStop = false;
            this.baseGroupBox_FiducialModel.Text = " [ Model ] ";
            // 
            // comboBox_Polarity
            // 
            this.comboBox_Polarity.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_Polarity.FormattingEnabled = true;
            this.comboBox_Polarity.Items.AddRange(new object[] {
            "Black",
            "White",
            "Ignore"});
            this.comboBox_Polarity.Location = new System.Drawing.Point(349, 59);
            this.comboBox_Polarity.Name = "comboBox_Polarity";
            this.comboBox_Polarity.Size = new System.Drawing.Size(79, 26);
            this.comboBox_Polarity.TabIndex = 162;
            this.comboBox_Polarity.Text = "Ignore";
            // 
            // baseLabel_Polarity
            // 
            this.baseLabel_Polarity.AutoSize = true;
            this.baseLabel_Polarity.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_Polarity.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Polarity.Location = new System.Drawing.Point(346, 36);
            this.baseLabel_Polarity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Polarity.Name = "baseLabel_Polarity";
            this.baseLabel_Polarity.Size = new System.Drawing.Size(54, 18);
            this.baseLabel_Polarity.TabIndex = 161;
            this.baseLabel_Polarity.Text = "Polarity";
            // 
            // pictureBox_FiducialModel_Image
            // 
            this.pictureBox_FiducialModel_Image.Location = new System.Drawing.Point(217, 32);
            this.pictureBox_FiducialModel_Image.Name = "pictureBox_FiducialModel_Image";
            this.pictureBox_FiducialModel_Image.Size = new System.Drawing.Size(118, 118);
            this.pictureBox_FiducialModel_Image.TabIndex = 160;
            this.pictureBox_FiducialModel_Image.TabStop = false;
            // 
            // baseLabel21
            // 
            this.baseLabel21.AutoSize = true;
            this.baseLabel21.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel21.ForeColor = System.Drawing.Color.Black;
            this.baseLabel21.Location = new System.Drawing.Point(149, 128);
            this.baseLabel21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel21.Name = "baseLabel21";
            this.baseLabel21.Size = new System.Drawing.Size(23, 18);
            this.baseLabel21.TabIndex = 159;
            this.baseLabel21.Text = "㎜";
            // 
            // textBox12
            // 
            this.textBox12.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox12.Location = new System.Drawing.Point(55, 125);
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new System.Drawing.Size(92, 26);
            this.textBox12.TabIndex = 158;
            // 
            // baseLabel22
            // 
            this.baseLabel22.AutoSize = true;
            this.baseLabel22.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel22.ForeColor = System.Drawing.Color.Black;
            this.baseLabel22.Location = new System.Drawing.Point(7, 129);
            this.baseLabel22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel22.Name = "baseLabel22";
            this.baseLabel22.Size = new System.Drawing.Size(47, 18);
            this.baseLabel22.TabIndex = 157;
            this.baseLabel22.Text = "Size A";
            // 
            // baseLabel19
            // 
            this.baseLabel19.AutoSize = true;
            this.baseLabel19.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel19.ForeColor = System.Drawing.Color.Black;
            this.baseLabel19.Location = new System.Drawing.Point(149, 97);
            this.baseLabel19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel19.Name = "baseLabel19";
            this.baseLabel19.Size = new System.Drawing.Size(23, 18);
            this.baseLabel19.TabIndex = 156;
            this.baseLabel19.Text = "㎜";
            // 
            // textBox11
            // 
            this.textBox11.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox11.Location = new System.Drawing.Point(55, 94);
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(92, 26);
            this.textBox11.TabIndex = 155;
            // 
            // baseLabel20
            // 
            this.baseLabel20.AutoSize = true;
            this.baseLabel20.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel20.ForeColor = System.Drawing.Color.Black;
            this.baseLabel20.Location = new System.Drawing.Point(7, 98);
            this.baseLabel20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel20.Name = "baseLabel20";
            this.baseLabel20.Size = new System.Drawing.Size(47, 18);
            this.baseLabel20.TabIndex = 154;
            this.baseLabel20.Text = "Size A";
            // 
            // baseLabel18
            // 
            this.baseLabel18.AutoSize = true;
            this.baseLabel18.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel18.ForeColor = System.Drawing.Color.Black;
            this.baseLabel18.Location = new System.Drawing.Point(149, 66);
            this.baseLabel18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel18.Name = "baseLabel18";
            this.baseLabel18.Size = new System.Drawing.Size(23, 18);
            this.baseLabel18.TabIndex = 127;
            this.baseLabel18.Text = "㎜";
            // 
            // textBox10
            // 
            this.textBox10.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox10.Location = new System.Drawing.Point(55, 63);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(92, 26);
            this.textBox10.TabIndex = 126;
            // 
            // baseLabel_FiducialSize_A
            // 
            this.baseLabel_FiducialSize_A.AutoSize = true;
            this.baseLabel_FiducialSize_A.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_FiducialSize_A.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_FiducialSize_A.Location = new System.Drawing.Point(7, 67);
            this.baseLabel_FiducialSize_A.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_FiducialSize_A.Name = "baseLabel_FiducialSize_A";
            this.baseLabel_FiducialSize_A.Size = new System.Drawing.Size(47, 18);
            this.baseLabel_FiducialSize_A.TabIndex = 125;
            this.baseLabel_FiducialSize_A.Text = "Size A";
            // 
            // comboBox_FiducialType
            // 
            this.comboBox_FiducialType.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_FiducialType.FormattingEnabled = true;
            this.comboBox_FiducialType.Items.AddRange(new object[] {
            "Circle",
            "Rect"});
            this.comboBox_FiducialType.Location = new System.Drawing.Point(55, 32);
            this.comboBox_FiducialType.Name = "comboBox_FiducialType";
            this.comboBox_FiducialType.Size = new System.Drawing.Size(92, 26);
            this.comboBox_FiducialType.TabIndex = 122;
            this.comboBox_FiducialType.Text = "Circle";
            // 
            // baseLabel_FiducialType
            // 
            this.baseLabel_FiducialType.AutoSize = true;
            this.baseLabel_FiducialType.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_FiducialType.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_FiducialType.Location = new System.Drawing.Point(7, 36);
            this.baseLabel_FiducialType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_FiducialType.Name = "baseLabel_FiducialType";
            this.baseLabel_FiducialType.Size = new System.Drawing.Size(42, 18);
            this.baseLabel_FiducialType.TabIndex = 121;
            this.baseLabel_FiducialType.Text = "Type";
            // 
            // baseGroupBox_FiducialList
            // 
            this.baseGroupBox_FiducialList.Controls.Add(this.button_MoveTo_FiducialMarkPos);
            this.baseGroupBox_FiducialList.Controls.Add(this.treeView_FiducialMark);
            this.baseGroupBox_FiducialList.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_FiducialList.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox_FiducialList.Location = new System.Drawing.Point(11, 11);
            this.baseGroupBox_FiducialList.Name = "baseGroupBox_FiducialList";
            this.baseGroupBox_FiducialList.Size = new System.Drawing.Size(263, 336);
            this.baseGroupBox_FiducialList.TabIndex = 12;
            this.baseGroupBox_FiducialList.TabStop = false;
            this.baseGroupBox_FiducialList.Text = " [ Fiducial Mark List ] ";
            // 
            // button_MoveTo_FiducialMarkPos
            // 
            this.button_MoveTo_FiducialMarkPos.BackColor = System.Drawing.Color.White;
            this.button_MoveTo_FiducialMarkPos.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_MoveTo_FiducialMarkPos.FlatAppearance.BorderSize = 2;
            this.button_MoveTo_FiducialMarkPos.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_MoveTo_FiducialMarkPos.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_MoveTo_FiducialMarkPos.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_MoveTo_FiducialMarkPos.ForeColor = System.Drawing.Color.Black;
            this.button_MoveTo_FiducialMarkPos.Location = new System.Drawing.Point(9, 286);
            this.button_MoveTo_FiducialMarkPos.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_MoveTo_FiducialMarkPos.Name = "button_MoveTo_FiducialMarkPos";
            this.button_MoveTo_FiducialMarkPos.Size = new System.Drawing.Size(244, 41);
            this.button_MoveTo_FiducialMarkPos.TabIndex = 197;
            this.button_MoveTo_FiducialMarkPos.Text = "Move To Fiducial Mark Position";
            this.button_MoveTo_FiducialMarkPos.UseVisualStyleBackColor = false;
            // 
            // treeView_FiducialMark
            // 
            this.treeView_FiducialMark.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.treeView_FiducialMark.Location = new System.Drawing.Point(9, 32);
            this.treeView_FiducialMark.Name = "treeView_FiducialMark";
            this.treeView_FiducialMark.Size = new System.Drawing.Size(244, 248);
            this.treeView_FiducialMark.TabIndex = 1;
            // 
            // tabPage_Position
            // 
            this.tabPage_Position.BackColor = System.Drawing.Color.Silver;
            this.tabPage_Position.Controls.Add(this.tabControl_Position);
            this.tabPage_Position.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.tabPage_Position.Location = new System.Drawing.Point(4, 36);
            this.tabPage_Position.Name = "tabPage_Position";
            this.tabPage_Position.Size = new System.Drawing.Size(808, 724);
            this.tabPage_Position.TabIndex = 4;
            this.tabPage_Position.Text = "Position";
            // 
            // tabControl_Position
            // 
            this.tabControl_Position.Controls.Add(this.tabPage_Pos_Unloader);
            this.tabControl_Position.Controls.Add(this.tabPage_Pos_WorkStage);
            this.tabControl_Position.Controls.Add(this.tabPage_Pos_Loader);
            this.tabControl_Position.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl_Position.ItemSize = new System.Drawing.Size(130, 26);
            this.tabControl_Position.Location = new System.Drawing.Point(13, 11);
            this.tabControl_Position.Multiline = true;
            this.tabControl_Position.Name = "tabControl_Position";
            this.tabControl_Position.SelectedIndex = 0;
            this.tabControl_Position.Size = new System.Drawing.Size(689, 703);
            this.tabControl_Position.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl_Position.TabIndex = 200;
            // 
            // tabPage_Pos_Unloader
            // 
            this.tabPage_Pos_Unloader.BackColor = System.Drawing.Color.Gainsboro;
            this.tabPage_Pos_Unloader.Controls.Add(this.button_ULStacker1Pos_Empty_Get);
            this.tabPage_Pos_Unloader.Controls.Add(this.textBox_ULStacker1Pos_Empty);
            this.tabPage_Pos_Unloader.Controls.Add(this.baseLabel_ULStacker1Pos_Empty1);
            this.tabPage_Pos_Unloader.Controls.Add(this.baseLabel_ULStacker1Pos_Empty);
            this.tabPage_Pos_Unloader.Controls.Add(this.button_ULStacker1Pos_Full_Get);
            this.tabPage_Pos_Unloader.Controls.Add(this.textBox_ULStacker1Pos_Full);
            this.tabPage_Pos_Unloader.Controls.Add(this.baseLabel_ULStacker1Pos_Full1);
            this.tabPage_Pos_Unloader.Controls.Add(this.baseLabel_ULStacker1Pos_Full);
            this.tabPage_Pos_Unloader.Controls.Add(this.button_ULStacker0Pos_Empty_Get);
            this.tabPage_Pos_Unloader.Controls.Add(this.textBox_ULStacker0Pos_Empty);
            this.tabPage_Pos_Unloader.Controls.Add(this.baseLabel_ULStacker0Pos_Empty1);
            this.tabPage_Pos_Unloader.Controls.Add(this.baseLabel_ULStacker0Pos_Empty);
            this.tabPage_Pos_Unloader.Controls.Add(this.button_ULStacker0Pos_Full_Get);
            this.tabPage_Pos_Unloader.Controls.Add(this.textBox_ULStacker0Pos_Full);
            this.tabPage_Pos_Unloader.Controls.Add(this.baseLabel_ULStacker0Pos_Full1);
            this.tabPage_Pos_Unloader.Controls.Add(this.baseLabel_ULStacker0Pos_Full);
            this.tabPage_Pos_Unloader.Controls.Add(this.button_ULPickerPos_Moving_Get);
            this.tabPage_Pos_Unloader.Controls.Add(this.textBox_ULPickerPos_Moving_Z);
            this.tabPage_Pos_Unloader.Controls.Add(this.baseLabel_ULPickerPos_Moving_Z);
            this.tabPage_Pos_Unloader.Controls.Add(this.baseLabel_ULPickerPos_Moving);
            this.tabPage_Pos_Unloader.Controls.Add(this.button_ULPickerPos_Module_PutDown_Stacker1_Get);
            this.tabPage_Pos_Unloader.Controls.Add(this.textBox_ULPickerPos_Module_PutDown_Stacker1_Z);
            this.tabPage_Pos_Unloader.Controls.Add(this.baseLabel_ULPickerPos_MGZ2_Z);
            this.tabPage_Pos_Unloader.Controls.Add(this.textBox_ULPickerPos_Module_PutDown_Stacker1_X);
            this.tabPage_Pos_Unloader.Controls.Add(this.baseLabel_ULPickerPos_MGZ2_X);
            this.tabPage_Pos_Unloader.Controls.Add(this.baseLabel_ULPickerPos_MGZ2);
            this.tabPage_Pos_Unloader.Controls.Add(this.button_ULPickerPos_Module_PutDown_Stacker0_Get);
            this.tabPage_Pos_Unloader.Controls.Add(this.textBox_ULPickerPos_Module_PutDown_Stacker0_Z);
            this.tabPage_Pos_Unloader.Controls.Add(this.baseLabel_ULPickerPos_MGZ1_Z);
            this.tabPage_Pos_Unloader.Controls.Add(this.textBox_ULPickerPos_Module_PutDown_Stacker0_X);
            this.tabPage_Pos_Unloader.Controls.Add(this.baseLabel_ULPickerPos_MGZ1_X);
            this.tabPage_Pos_Unloader.Controls.Add(this.baseLabel_ULPickerPos_MGZ1);
            this.tabPage_Pos_Unloader.Controls.Add(this.button_ULPickerPos_NgBox_Get);
            this.tabPage_Pos_Unloader.Controls.Add(this.button_ULPickerPos_Module_PickUp_WorkTable_Get);
            this.tabPage_Pos_Unloader.Controls.Add(this.textBox_ULPickerPos_NgBox_Z);
            this.tabPage_Pos_Unloader.Controls.Add(this.baseLabel_ULPickerPos_NgBox_Z);
            this.tabPage_Pos_Unloader.Controls.Add(this.textBox_ULPickerPos_NgBox_X);
            this.tabPage_Pos_Unloader.Controls.Add(this.baseLabel_ULPickerPos_NgBox_X);
            this.tabPage_Pos_Unloader.Controls.Add(this.baseLabel_ULPickerPos_NGBox);
            this.tabPage_Pos_Unloader.Controls.Add(this.textBox_ULPickerPos_Module_PickUp_WorkTable_Z);
            this.tabPage_Pos_Unloader.Controls.Add(this.baseLabel_ULPickerPos_WorkTable_Z);
            this.tabPage_Pos_Unloader.Controls.Add(this.textBox_ULPickerPos_Module_PickUp_WorkTable_X);
            this.tabPage_Pos_Unloader.Controls.Add(this.baseLabel_ULPickerPos_WorkTable_X);
            this.tabPage_Pos_Unloader.Controls.Add(this.baseLabel_ULPickerPos_WorkTable);
            this.tabPage_Pos_Unloader.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage_Pos_Unloader.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tabPage_Pos_Unloader.Location = new System.Drawing.Point(4, 30);
            this.tabPage_Pos_Unloader.Name = "tabPage_Pos_Unloader";
            this.tabPage_Pos_Unloader.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Pos_Unloader.Size = new System.Drawing.Size(681, 669);
            this.tabPage_Pos_Unloader.TabIndex = 2;
            this.tabPage_Pos_Unloader.Text = "Unloader";
            // 
            // button_ULStacker1Pos_Empty_Get
            // 
            this.button_ULStacker1Pos_Empty_Get.BackColor = System.Drawing.Color.White;
            this.button_ULStacker1Pos_Empty_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_ULStacker1Pos_Empty_Get.FlatAppearance.BorderSize = 2;
            this.button_ULStacker1Pos_Empty_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_ULStacker1Pos_Empty_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_ULStacker1Pos_Empty_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_ULStacker1Pos_Empty_Get.ForeColor = System.Drawing.Color.Black;
            this.button_ULStacker1Pos_Empty_Get.Location = new System.Drawing.Point(602, 199);
            this.button_ULStacker1Pos_Empty_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_ULStacker1Pos_Empty_Get.Name = "button_ULStacker1Pos_Empty_Get";
            this.button_ULStacker1Pos_Empty_Get.Size = new System.Drawing.Size(70, 55);
            this.button_ULStacker1Pos_Empty_Get.TabIndex = 261;
            this.button_ULStacker1Pos_Empty_Get.Text = "Get Pos.\r\n(Z Only)";
            this.button_ULStacker1Pos_Empty_Get.UseVisualStyleBackColor = false;
            // 
            // textBox_ULStacker1Pos_Empty
            // 
            this.textBox_ULStacker1Pos_Empty.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_ULStacker1Pos_Empty.Location = new System.Drawing.Point(524, 215);
            this.textBox_ULStacker1Pos_Empty.Name = "textBox_ULStacker1Pos_Empty";
            this.textBox_ULStacker1Pos_Empty.Size = new System.Drawing.Size(77, 23);
            this.textBox_ULStacker1Pos_Empty.TabIndex = 260;
            // 
            // baseLabel_ULStacker1Pos_Empty1
            // 
            this.baseLabel_ULStacker1Pos_Empty1.AutoSize = true;
            this.baseLabel_ULStacker1Pos_Empty1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_ULStacker1Pos_Empty1.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_ULStacker1Pos_Empty1.Location = new System.Drawing.Point(480, 218);
            this.baseLabel_ULStacker1Pos_Empty1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_ULStacker1Pos_Empty1.Name = "baseLabel_ULStacker1Pos_Empty1";
            this.baseLabel_ULStacker1Pos_Empty1.Size = new System.Drawing.Size(42, 16);
            this.baseLabel_ULStacker1Pos_Empty1.TabIndex = 259;
            this.baseLabel_ULStacker1Pos_Empty1.Text = "Pos. Z";
            // 
            // baseLabel_ULStacker1Pos_Empty
            // 
            this.baseLabel_ULStacker1Pos_Empty.BackColor = System.Drawing.Color.Silver;
            this.baseLabel_ULStacker1Pos_Empty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_ULStacker1Pos_Empty.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_ULStacker1Pos_Empty.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_ULStacker1Pos_Empty.Location = new System.Drawing.Point(361, 199);
            this.baseLabel_ULStacker1Pos_Empty.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_ULStacker1Pos_Empty.Name = "baseLabel_ULStacker1Pos_Empty";
            this.baseLabel_ULStacker1Pos_Empty.Size = new System.Drawing.Size(117, 55);
            this.baseLabel_ULStacker1Pos_Empty.TabIndex = 258;
            this.baseLabel_ULStacker1Pos_Empty.Text = "[ Stacker1  (L) ]\r\nEmpty";
            this.baseLabel_ULStacker1Pos_Empty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_ULStacker1Pos_Full_Get
            // 
            this.button_ULStacker1Pos_Full_Get.BackColor = System.Drawing.Color.White;
            this.button_ULStacker1Pos_Full_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_ULStacker1Pos_Full_Get.FlatAppearance.BorderSize = 2;
            this.button_ULStacker1Pos_Full_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_ULStacker1Pos_Full_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_ULStacker1Pos_Full_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_ULStacker1Pos_Full_Get.ForeColor = System.Drawing.Color.Black;
            this.button_ULStacker1Pos_Full_Get.Location = new System.Drawing.Point(602, 136);
            this.button_ULStacker1Pos_Full_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_ULStacker1Pos_Full_Get.Name = "button_ULStacker1Pos_Full_Get";
            this.button_ULStacker1Pos_Full_Get.Size = new System.Drawing.Size(70, 55);
            this.button_ULStacker1Pos_Full_Get.TabIndex = 257;
            this.button_ULStacker1Pos_Full_Get.Text = "Get Pos.\r\n(Z Only)";
            this.button_ULStacker1Pos_Full_Get.UseVisualStyleBackColor = false;
            // 
            // textBox_ULStacker1Pos_Full
            // 
            this.textBox_ULStacker1Pos_Full.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_ULStacker1Pos_Full.Location = new System.Drawing.Point(524, 152);
            this.textBox_ULStacker1Pos_Full.Name = "textBox_ULStacker1Pos_Full";
            this.textBox_ULStacker1Pos_Full.Size = new System.Drawing.Size(77, 23);
            this.textBox_ULStacker1Pos_Full.TabIndex = 256;
            // 
            // baseLabel_ULStacker1Pos_Full1
            // 
            this.baseLabel_ULStacker1Pos_Full1.AutoSize = true;
            this.baseLabel_ULStacker1Pos_Full1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_ULStacker1Pos_Full1.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_ULStacker1Pos_Full1.Location = new System.Drawing.Point(480, 155);
            this.baseLabel_ULStacker1Pos_Full1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_ULStacker1Pos_Full1.Name = "baseLabel_ULStacker1Pos_Full1";
            this.baseLabel_ULStacker1Pos_Full1.Size = new System.Drawing.Size(42, 16);
            this.baseLabel_ULStacker1Pos_Full1.TabIndex = 255;
            this.baseLabel_ULStacker1Pos_Full1.Text = "Pos. Z";
            // 
            // baseLabel_ULStacker1Pos_Full
            // 
            this.baseLabel_ULStacker1Pos_Full.BackColor = System.Drawing.Color.Silver;
            this.baseLabel_ULStacker1Pos_Full.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_ULStacker1Pos_Full.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_ULStacker1Pos_Full.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_ULStacker1Pos_Full.Location = new System.Drawing.Point(361, 136);
            this.baseLabel_ULStacker1Pos_Full.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_ULStacker1Pos_Full.Name = "baseLabel_ULStacker1Pos_Full";
            this.baseLabel_ULStacker1Pos_Full.Size = new System.Drawing.Size(117, 55);
            this.baseLabel_ULStacker1Pos_Full.TabIndex = 254;
            this.baseLabel_ULStacker1Pos_Full.Text = "[ Stacker1  (L) ]\r\nFull";
            this.baseLabel_ULStacker1Pos_Full.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_ULStacker0Pos_Empty_Get
            // 
            this.button_ULStacker0Pos_Empty_Get.BackColor = System.Drawing.Color.White;
            this.button_ULStacker0Pos_Empty_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_ULStacker0Pos_Empty_Get.FlatAppearance.BorderSize = 2;
            this.button_ULStacker0Pos_Empty_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_ULStacker0Pos_Empty_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_ULStacker0Pos_Empty_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_ULStacker0Pos_Empty_Get.ForeColor = System.Drawing.Color.Black;
            this.button_ULStacker0Pos_Empty_Get.Location = new System.Drawing.Point(602, 73);
            this.button_ULStacker0Pos_Empty_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_ULStacker0Pos_Empty_Get.Name = "button_ULStacker0Pos_Empty_Get";
            this.button_ULStacker0Pos_Empty_Get.Size = new System.Drawing.Size(70, 55);
            this.button_ULStacker0Pos_Empty_Get.TabIndex = 253;
            this.button_ULStacker0Pos_Empty_Get.Text = "Get Pos.\r\n(Z Only)";
            this.button_ULStacker0Pos_Empty_Get.UseVisualStyleBackColor = false;
            // 
            // textBox_ULStacker0Pos_Empty
            // 
            this.textBox_ULStacker0Pos_Empty.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_ULStacker0Pos_Empty.Location = new System.Drawing.Point(524, 89);
            this.textBox_ULStacker0Pos_Empty.Name = "textBox_ULStacker0Pos_Empty";
            this.textBox_ULStacker0Pos_Empty.Size = new System.Drawing.Size(77, 23);
            this.textBox_ULStacker0Pos_Empty.TabIndex = 252;
            // 
            // baseLabel_ULStacker0Pos_Empty1
            // 
            this.baseLabel_ULStacker0Pos_Empty1.AutoSize = true;
            this.baseLabel_ULStacker0Pos_Empty1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_ULStacker0Pos_Empty1.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_ULStacker0Pos_Empty1.Location = new System.Drawing.Point(480, 92);
            this.baseLabel_ULStacker0Pos_Empty1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_ULStacker0Pos_Empty1.Name = "baseLabel_ULStacker0Pos_Empty1";
            this.baseLabel_ULStacker0Pos_Empty1.Size = new System.Drawing.Size(42, 16);
            this.baseLabel_ULStacker0Pos_Empty1.TabIndex = 251;
            this.baseLabel_ULStacker0Pos_Empty1.Text = "Pos. Z";
            // 
            // baseLabel_ULStacker0Pos_Empty
            // 
            this.baseLabel_ULStacker0Pos_Empty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_ULStacker0Pos_Empty.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_ULStacker0Pos_Empty.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_ULStacker0Pos_Empty.Location = new System.Drawing.Point(361, 73);
            this.baseLabel_ULStacker0Pos_Empty.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_ULStacker0Pos_Empty.Name = "baseLabel_ULStacker0Pos_Empty";
            this.baseLabel_ULStacker0Pos_Empty.Size = new System.Drawing.Size(117, 55);
            this.baseLabel_ULStacker0Pos_Empty.TabIndex = 250;
            this.baseLabel_ULStacker0Pos_Empty.Text = "[ Stacker0  (R) ]\r\nEmpty";
            this.baseLabel_ULStacker0Pos_Empty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_ULStacker0Pos_Full_Get
            // 
            this.button_ULStacker0Pos_Full_Get.BackColor = System.Drawing.Color.White;
            this.button_ULStacker0Pos_Full_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_ULStacker0Pos_Full_Get.FlatAppearance.BorderSize = 2;
            this.button_ULStacker0Pos_Full_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_ULStacker0Pos_Full_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_ULStacker0Pos_Full_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_ULStacker0Pos_Full_Get.ForeColor = System.Drawing.Color.Black;
            this.button_ULStacker0Pos_Full_Get.Location = new System.Drawing.Point(602, 10);
            this.button_ULStacker0Pos_Full_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_ULStacker0Pos_Full_Get.Name = "button_ULStacker0Pos_Full_Get";
            this.button_ULStacker0Pos_Full_Get.Size = new System.Drawing.Size(70, 55);
            this.button_ULStacker0Pos_Full_Get.TabIndex = 249;
            this.button_ULStacker0Pos_Full_Get.Text = "Get Pos.\r\n(Z Only)";
            this.button_ULStacker0Pos_Full_Get.UseVisualStyleBackColor = false;
            // 
            // textBox_ULStacker0Pos_Full
            // 
            this.textBox_ULStacker0Pos_Full.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_ULStacker0Pos_Full.Location = new System.Drawing.Point(524, 26);
            this.textBox_ULStacker0Pos_Full.Name = "textBox_ULStacker0Pos_Full";
            this.textBox_ULStacker0Pos_Full.Size = new System.Drawing.Size(77, 23);
            this.textBox_ULStacker0Pos_Full.TabIndex = 248;
            // 
            // baseLabel_ULStacker0Pos_Full1
            // 
            this.baseLabel_ULStacker0Pos_Full1.AutoSize = true;
            this.baseLabel_ULStacker0Pos_Full1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_ULStacker0Pos_Full1.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_ULStacker0Pos_Full1.Location = new System.Drawing.Point(480, 29);
            this.baseLabel_ULStacker0Pos_Full1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_ULStacker0Pos_Full1.Name = "baseLabel_ULStacker0Pos_Full1";
            this.baseLabel_ULStacker0Pos_Full1.Size = new System.Drawing.Size(42, 16);
            this.baseLabel_ULStacker0Pos_Full1.TabIndex = 247;
            this.baseLabel_ULStacker0Pos_Full1.Text = "Pos. Z";
            // 
            // baseLabel_ULStacker0Pos_Full
            // 
            this.baseLabel_ULStacker0Pos_Full.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_ULStacker0Pos_Full.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_ULStacker0Pos_Full.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_ULStacker0Pos_Full.Location = new System.Drawing.Point(361, 10);
            this.baseLabel_ULStacker0Pos_Full.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_ULStacker0Pos_Full.Name = "baseLabel_ULStacker0Pos_Full";
            this.baseLabel_ULStacker0Pos_Full.Size = new System.Drawing.Size(117, 55);
            this.baseLabel_ULStacker0Pos_Full.TabIndex = 246;
            this.baseLabel_ULStacker0Pos_Full.Text = "[ Stacker0  (R) ]\r\nFull";
            this.baseLabel_ULStacker0Pos_Full.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_ULPickerPos_Moving_Get
            // 
            this.button_ULPickerPos_Moving_Get.BackColor = System.Drawing.Color.White;
            this.button_ULPickerPos_Moving_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_ULPickerPos_Moving_Get.FlatAppearance.BorderSize = 2;
            this.button_ULPickerPos_Moving_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_ULPickerPos_Moving_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_ULPickerPos_Moving_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_ULPickerPos_Moving_Get.ForeColor = System.Drawing.Color.Black;
            this.button_ULPickerPos_Moving_Get.Location = new System.Drawing.Point(251, 10);
            this.button_ULPickerPos_Moving_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_ULPickerPos_Moving_Get.Name = "button_ULPickerPos_Moving_Get";
            this.button_ULPickerPos_Moving_Get.Size = new System.Drawing.Size(70, 55);
            this.button_ULPickerPos_Moving_Get.TabIndex = 245;
            this.button_ULPickerPos_Moving_Get.Text = "Get Pos.\r\n(Z Only)";
            this.button_ULPickerPos_Moving_Get.UseVisualStyleBackColor = false;
            // 
            // textBox_ULPickerPos_Moving_Z
            // 
            this.textBox_ULPickerPos_Moving_Z.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_ULPickerPos_Moving_Z.Location = new System.Drawing.Point(173, 26);
            this.textBox_ULPickerPos_Moving_Z.Name = "textBox_ULPickerPos_Moving_Z";
            this.textBox_ULPickerPos_Moving_Z.Size = new System.Drawing.Size(77, 23);
            this.textBox_ULPickerPos_Moving_Z.TabIndex = 244;
            // 
            // baseLabel_ULPickerPos_Moving_Z
            // 
            this.baseLabel_ULPickerPos_Moving_Z.AutoSize = true;
            this.baseLabel_ULPickerPos_Moving_Z.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_ULPickerPos_Moving_Z.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_ULPickerPos_Moving_Z.Location = new System.Drawing.Point(129, 29);
            this.baseLabel_ULPickerPos_Moving_Z.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_ULPickerPos_Moving_Z.Name = "baseLabel_ULPickerPos_Moving_Z";
            this.baseLabel_ULPickerPos_Moving_Z.Size = new System.Drawing.Size(42, 16);
            this.baseLabel_ULPickerPos_Moving_Z.TabIndex = 243;
            this.baseLabel_ULPickerPos_Moving_Z.Text = "Pos. Z";
            // 
            // baseLabel_ULPickerPos_Moving
            // 
            this.baseLabel_ULPickerPos_Moving.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_ULPickerPos_Moving.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_ULPickerPos_Moving.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_ULPickerPos_Moving.Location = new System.Drawing.Point(10, 10);
            this.baseLabel_ULPickerPos_Moving.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_ULPickerPos_Moving.Name = "baseLabel_ULPickerPos_Moving";
            this.baseLabel_ULPickerPos_Moving.Size = new System.Drawing.Size(117, 55);
            this.baseLabel_ULPickerPos_Moving.TabIndex = 242;
            this.baseLabel_ULPickerPos_Moving.Text = "[ Transfer ]\r\nMovable";
            this.baseLabel_ULPickerPos_Moving.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_ULPickerPos_Module_PutDown_Stacker1_Get
            // 
            this.button_ULPickerPos_Module_PutDown_Stacker1_Get.BackColor = System.Drawing.Color.White;
            this.button_ULPickerPos_Module_PutDown_Stacker1_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_ULPickerPos_Module_PutDown_Stacker1_Get.FlatAppearance.BorderSize = 2;
            this.button_ULPickerPos_Module_PutDown_Stacker1_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_ULPickerPos_Module_PutDown_Stacker1_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_ULPickerPos_Module_PutDown_Stacker1_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_ULPickerPos_Module_PutDown_Stacker1_Get.ForeColor = System.Drawing.Color.Black;
            this.button_ULPickerPos_Module_PutDown_Stacker1_Get.Location = new System.Drawing.Point(251, 199);
            this.button_ULPickerPos_Module_PutDown_Stacker1_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_ULPickerPos_Module_PutDown_Stacker1_Get.Name = "button_ULPickerPos_Module_PutDown_Stacker1_Get";
            this.button_ULPickerPos_Module_PutDown_Stacker1_Get.Size = new System.Drawing.Size(70, 55);
            this.button_ULPickerPos_Module_PutDown_Stacker1_Get.TabIndex = 241;
            this.button_ULPickerPos_Module_PutDown_Stacker1_Get.Text = "Get Pos.";
            this.button_ULPickerPos_Module_PutDown_Stacker1_Get.UseVisualStyleBackColor = false;
            // 
            // textBox_ULPickerPos_Module_PutDown_Stacker1_Z
            // 
            this.textBox_ULPickerPos_Module_PutDown_Stacker1_Z.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_ULPickerPos_Module_PutDown_Stacker1_Z.Location = new System.Drawing.Point(173, 229);
            this.textBox_ULPickerPos_Module_PutDown_Stacker1_Z.Name = "textBox_ULPickerPos_Module_PutDown_Stacker1_Z";
            this.textBox_ULPickerPos_Module_PutDown_Stacker1_Z.Size = new System.Drawing.Size(77, 23);
            this.textBox_ULPickerPos_Module_PutDown_Stacker1_Z.TabIndex = 240;
            // 
            // baseLabel_ULPickerPos_MGZ2_Z
            // 
            this.baseLabel_ULPickerPos_MGZ2_Z.AutoSize = true;
            this.baseLabel_ULPickerPos_MGZ2_Z.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_ULPickerPos_MGZ2_Z.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_ULPickerPos_MGZ2_Z.Location = new System.Drawing.Point(129, 232);
            this.baseLabel_ULPickerPos_MGZ2_Z.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_ULPickerPos_MGZ2_Z.Name = "baseLabel_ULPickerPos_MGZ2_Z";
            this.baseLabel_ULPickerPos_MGZ2_Z.Size = new System.Drawing.Size(42, 16);
            this.baseLabel_ULPickerPos_MGZ2_Z.TabIndex = 239;
            this.baseLabel_ULPickerPos_MGZ2_Z.Text = "Pos. Z";
            // 
            // textBox_ULPickerPos_Module_PutDown_Stacker1_X
            // 
            this.textBox_ULPickerPos_Module_PutDown_Stacker1_X.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_ULPickerPos_Module_PutDown_Stacker1_X.Location = new System.Drawing.Point(173, 201);
            this.textBox_ULPickerPos_Module_PutDown_Stacker1_X.Name = "textBox_ULPickerPos_Module_PutDown_Stacker1_X";
            this.textBox_ULPickerPos_Module_PutDown_Stacker1_X.Size = new System.Drawing.Size(77, 23);
            this.textBox_ULPickerPos_Module_PutDown_Stacker1_X.TabIndex = 238;
            // 
            // baseLabel_ULPickerPos_MGZ2_X
            // 
            this.baseLabel_ULPickerPos_MGZ2_X.AutoSize = true;
            this.baseLabel_ULPickerPos_MGZ2_X.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_ULPickerPos_MGZ2_X.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_ULPickerPos_MGZ2_X.Location = new System.Drawing.Point(129, 204);
            this.baseLabel_ULPickerPos_MGZ2_X.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_ULPickerPos_MGZ2_X.Name = "baseLabel_ULPickerPos_MGZ2_X";
            this.baseLabel_ULPickerPos_MGZ2_X.Size = new System.Drawing.Size(43, 16);
            this.baseLabel_ULPickerPos_MGZ2_X.TabIndex = 237;
            this.baseLabel_ULPickerPos_MGZ2_X.Text = "Pos. X";
            // 
            // baseLabel_ULPickerPos_MGZ2
            // 
            this.baseLabel_ULPickerPos_MGZ2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_ULPickerPos_MGZ2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_ULPickerPos_MGZ2.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_ULPickerPos_MGZ2.Location = new System.Drawing.Point(10, 199);
            this.baseLabel_ULPickerPos_MGZ2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_ULPickerPos_MGZ2.Name = "baseLabel_ULPickerPos_MGZ2";
            this.baseLabel_ULPickerPos_MGZ2.Size = new System.Drawing.Size(117, 55);
            this.baseLabel_ULPickerPos_MGZ2.TabIndex = 236;
            this.baseLabel_ULPickerPos_MGZ2.Text = "[  Transfer  ]\r\nModule PutDown\r\nto  Stacker1";
            this.baseLabel_ULPickerPos_MGZ2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_ULPickerPos_Module_PutDown_Stacker0_Get
            // 
            this.button_ULPickerPos_Module_PutDown_Stacker0_Get.BackColor = System.Drawing.Color.White;
            this.button_ULPickerPos_Module_PutDown_Stacker0_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_ULPickerPos_Module_PutDown_Stacker0_Get.FlatAppearance.BorderSize = 2;
            this.button_ULPickerPos_Module_PutDown_Stacker0_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_ULPickerPos_Module_PutDown_Stacker0_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_ULPickerPos_Module_PutDown_Stacker0_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_ULPickerPos_Module_PutDown_Stacker0_Get.ForeColor = System.Drawing.Color.Black;
            this.button_ULPickerPos_Module_PutDown_Stacker0_Get.Location = new System.Drawing.Point(251, 136);
            this.button_ULPickerPos_Module_PutDown_Stacker0_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_ULPickerPos_Module_PutDown_Stacker0_Get.Name = "button_ULPickerPos_Module_PutDown_Stacker0_Get";
            this.button_ULPickerPos_Module_PutDown_Stacker0_Get.Size = new System.Drawing.Size(70, 55);
            this.button_ULPickerPos_Module_PutDown_Stacker0_Get.TabIndex = 235;
            this.button_ULPickerPos_Module_PutDown_Stacker0_Get.Text = "Get Pos.";
            this.button_ULPickerPos_Module_PutDown_Stacker0_Get.UseVisualStyleBackColor = false;
            // 
            // textBox_ULPickerPos_Module_PutDown_Stacker0_Z
            // 
            this.textBox_ULPickerPos_Module_PutDown_Stacker0_Z.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_ULPickerPos_Module_PutDown_Stacker0_Z.Location = new System.Drawing.Point(173, 166);
            this.textBox_ULPickerPos_Module_PutDown_Stacker0_Z.Name = "textBox_ULPickerPos_Module_PutDown_Stacker0_Z";
            this.textBox_ULPickerPos_Module_PutDown_Stacker0_Z.Size = new System.Drawing.Size(77, 23);
            this.textBox_ULPickerPos_Module_PutDown_Stacker0_Z.TabIndex = 234;
            // 
            // baseLabel_ULPickerPos_MGZ1_Z
            // 
            this.baseLabel_ULPickerPos_MGZ1_Z.AutoSize = true;
            this.baseLabel_ULPickerPos_MGZ1_Z.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_ULPickerPos_MGZ1_Z.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_ULPickerPos_MGZ1_Z.Location = new System.Drawing.Point(129, 169);
            this.baseLabel_ULPickerPos_MGZ1_Z.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_ULPickerPos_MGZ1_Z.Name = "baseLabel_ULPickerPos_MGZ1_Z";
            this.baseLabel_ULPickerPos_MGZ1_Z.Size = new System.Drawing.Size(42, 16);
            this.baseLabel_ULPickerPos_MGZ1_Z.TabIndex = 233;
            this.baseLabel_ULPickerPos_MGZ1_Z.Text = "Pos. Z";
            // 
            // textBox_ULPickerPos_Module_PutDown_Stacker0_X
            // 
            this.textBox_ULPickerPos_Module_PutDown_Stacker0_X.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_ULPickerPos_Module_PutDown_Stacker0_X.Location = new System.Drawing.Point(173, 138);
            this.textBox_ULPickerPos_Module_PutDown_Stacker0_X.Name = "textBox_ULPickerPos_Module_PutDown_Stacker0_X";
            this.textBox_ULPickerPos_Module_PutDown_Stacker0_X.Size = new System.Drawing.Size(77, 23);
            this.textBox_ULPickerPos_Module_PutDown_Stacker0_X.TabIndex = 232;
            // 
            // baseLabel_ULPickerPos_MGZ1_X
            // 
            this.baseLabel_ULPickerPos_MGZ1_X.AutoSize = true;
            this.baseLabel_ULPickerPos_MGZ1_X.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_ULPickerPos_MGZ1_X.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_ULPickerPos_MGZ1_X.Location = new System.Drawing.Point(129, 141);
            this.baseLabel_ULPickerPos_MGZ1_X.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_ULPickerPos_MGZ1_X.Name = "baseLabel_ULPickerPos_MGZ1_X";
            this.baseLabel_ULPickerPos_MGZ1_X.Size = new System.Drawing.Size(43, 16);
            this.baseLabel_ULPickerPos_MGZ1_X.TabIndex = 231;
            this.baseLabel_ULPickerPos_MGZ1_X.Text = "Pos. X";
            // 
            // baseLabel_ULPickerPos_MGZ1
            // 
            this.baseLabel_ULPickerPos_MGZ1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_ULPickerPos_MGZ1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_ULPickerPos_MGZ1.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_ULPickerPos_MGZ1.Location = new System.Drawing.Point(10, 136);
            this.baseLabel_ULPickerPos_MGZ1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_ULPickerPos_MGZ1.Name = "baseLabel_ULPickerPos_MGZ1";
            this.baseLabel_ULPickerPos_MGZ1.Size = new System.Drawing.Size(117, 55);
            this.baseLabel_ULPickerPos_MGZ1.TabIndex = 230;
            this.baseLabel_ULPickerPos_MGZ1.Text = "[  Transfer  ]\r\nModule PutDown\r\nto  Stacker0";
            this.baseLabel_ULPickerPos_MGZ1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_ULPickerPos_NgBox_Get
            // 
            this.button_ULPickerPos_NgBox_Get.BackColor = System.Drawing.Color.White;
            this.button_ULPickerPos_NgBox_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_ULPickerPos_NgBox_Get.FlatAppearance.BorderSize = 2;
            this.button_ULPickerPos_NgBox_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_ULPickerPos_NgBox_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_ULPickerPos_NgBox_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_ULPickerPos_NgBox_Get.ForeColor = System.Drawing.Color.Black;
            this.button_ULPickerPos_NgBox_Get.Location = new System.Drawing.Point(251, 282);
            this.button_ULPickerPos_NgBox_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_ULPickerPos_NgBox_Get.Name = "button_ULPickerPos_NgBox_Get";
            this.button_ULPickerPos_NgBox_Get.Size = new System.Drawing.Size(70, 55);
            this.button_ULPickerPos_NgBox_Get.TabIndex = 229;
            this.button_ULPickerPos_NgBox_Get.Text = "Get Pos.";
            this.button_ULPickerPos_NgBox_Get.UseVisualStyleBackColor = false;
            // 
            // button_ULPickerPos_Module_PickUp_WorkTable_Get
            // 
            this.button_ULPickerPos_Module_PickUp_WorkTable_Get.BackColor = System.Drawing.Color.White;
            this.button_ULPickerPos_Module_PickUp_WorkTable_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_ULPickerPos_Module_PickUp_WorkTable_Get.FlatAppearance.BorderSize = 2;
            this.button_ULPickerPos_Module_PickUp_WorkTable_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_ULPickerPos_Module_PickUp_WorkTable_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_ULPickerPos_Module_PickUp_WorkTable_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_ULPickerPos_Module_PickUp_WorkTable_Get.ForeColor = System.Drawing.Color.Black;
            this.button_ULPickerPos_Module_PickUp_WorkTable_Get.Location = new System.Drawing.Point(251, 73);
            this.button_ULPickerPos_Module_PickUp_WorkTable_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_ULPickerPos_Module_PickUp_WorkTable_Get.Name = "button_ULPickerPos_Module_PickUp_WorkTable_Get";
            this.button_ULPickerPos_Module_PickUp_WorkTable_Get.Size = new System.Drawing.Size(70, 55);
            this.button_ULPickerPos_Module_PickUp_WorkTable_Get.TabIndex = 228;
            this.button_ULPickerPos_Module_PickUp_WorkTable_Get.Text = "Get Pos.";
            this.button_ULPickerPos_Module_PickUp_WorkTable_Get.UseVisualStyleBackColor = false;
            // 
            // textBox_ULPickerPos_NgBox_Z
            // 
            this.textBox_ULPickerPos_NgBox_Z.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_ULPickerPos_NgBox_Z.Location = new System.Drawing.Point(173, 312);
            this.textBox_ULPickerPos_NgBox_Z.Name = "textBox_ULPickerPos_NgBox_Z";
            this.textBox_ULPickerPos_NgBox_Z.Size = new System.Drawing.Size(77, 23);
            this.textBox_ULPickerPos_NgBox_Z.TabIndex = 227;
            // 
            // baseLabel_ULPickerPos_NgBox_Z
            // 
            this.baseLabel_ULPickerPos_NgBox_Z.AutoSize = true;
            this.baseLabel_ULPickerPos_NgBox_Z.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_ULPickerPos_NgBox_Z.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_ULPickerPos_NgBox_Z.Location = new System.Drawing.Point(129, 315);
            this.baseLabel_ULPickerPos_NgBox_Z.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_ULPickerPos_NgBox_Z.Name = "baseLabel_ULPickerPos_NgBox_Z";
            this.baseLabel_ULPickerPos_NgBox_Z.Size = new System.Drawing.Size(42, 16);
            this.baseLabel_ULPickerPos_NgBox_Z.TabIndex = 226;
            this.baseLabel_ULPickerPos_NgBox_Z.Text = "Pos. Z";
            // 
            // textBox_ULPickerPos_NgBox_X
            // 
            this.textBox_ULPickerPos_NgBox_X.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_ULPickerPos_NgBox_X.Location = new System.Drawing.Point(173, 284);
            this.textBox_ULPickerPos_NgBox_X.Name = "textBox_ULPickerPos_NgBox_X";
            this.textBox_ULPickerPos_NgBox_X.Size = new System.Drawing.Size(77, 23);
            this.textBox_ULPickerPos_NgBox_X.TabIndex = 225;
            // 
            // baseLabel_ULPickerPos_NgBox_X
            // 
            this.baseLabel_ULPickerPos_NgBox_X.AutoSize = true;
            this.baseLabel_ULPickerPos_NgBox_X.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_ULPickerPos_NgBox_X.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_ULPickerPos_NgBox_X.Location = new System.Drawing.Point(129, 287);
            this.baseLabel_ULPickerPos_NgBox_X.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_ULPickerPos_NgBox_X.Name = "baseLabel_ULPickerPos_NgBox_X";
            this.baseLabel_ULPickerPos_NgBox_X.Size = new System.Drawing.Size(43, 16);
            this.baseLabel_ULPickerPos_NgBox_X.TabIndex = 224;
            this.baseLabel_ULPickerPos_NgBox_X.Text = "Pos. X";
            // 
            // baseLabel_ULPickerPos_NGBox
            // 
            this.baseLabel_ULPickerPos_NGBox.BackColor = System.Drawing.Color.Silver;
            this.baseLabel_ULPickerPos_NGBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_ULPickerPos_NGBox.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_ULPickerPos_NGBox.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_ULPickerPos_NGBox.Location = new System.Drawing.Point(10, 282);
            this.baseLabel_ULPickerPos_NGBox.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_ULPickerPos_NGBox.Name = "baseLabel_ULPickerPos_NGBox";
            this.baseLabel_ULPickerPos_NGBox.Size = new System.Drawing.Size(117, 55);
            this.baseLabel_ULPickerPos_NGBox.TabIndex = 223;
            this.baseLabel_ULPickerPos_NGBox.Text = "[ Transfer ]\r\nNG Drop";
            this.baseLabel_ULPickerPos_NGBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox_ULPickerPos_Module_PickUp_WorkTable_Z
            // 
            this.textBox_ULPickerPos_Module_PickUp_WorkTable_Z.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_ULPickerPos_Module_PickUp_WorkTable_Z.Location = new System.Drawing.Point(173, 103);
            this.textBox_ULPickerPos_Module_PickUp_WorkTable_Z.Name = "textBox_ULPickerPos_Module_PickUp_WorkTable_Z";
            this.textBox_ULPickerPos_Module_PickUp_WorkTable_Z.Size = new System.Drawing.Size(77, 23);
            this.textBox_ULPickerPos_Module_PickUp_WorkTable_Z.TabIndex = 222;
            // 
            // baseLabel_ULPickerPos_WorkTable_Z
            // 
            this.baseLabel_ULPickerPos_WorkTable_Z.AutoSize = true;
            this.baseLabel_ULPickerPos_WorkTable_Z.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_ULPickerPos_WorkTable_Z.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_ULPickerPos_WorkTable_Z.Location = new System.Drawing.Point(129, 106);
            this.baseLabel_ULPickerPos_WorkTable_Z.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_ULPickerPos_WorkTable_Z.Name = "baseLabel_ULPickerPos_WorkTable_Z";
            this.baseLabel_ULPickerPos_WorkTable_Z.Size = new System.Drawing.Size(42, 16);
            this.baseLabel_ULPickerPos_WorkTable_Z.TabIndex = 221;
            this.baseLabel_ULPickerPos_WorkTable_Z.Text = "Pos. Z";
            // 
            // textBox_ULPickerPos_Module_PickUp_WorkTable_X
            // 
            this.textBox_ULPickerPos_Module_PickUp_WorkTable_X.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_ULPickerPos_Module_PickUp_WorkTable_X.Location = new System.Drawing.Point(173, 75);
            this.textBox_ULPickerPos_Module_PickUp_WorkTable_X.Name = "textBox_ULPickerPos_Module_PickUp_WorkTable_X";
            this.textBox_ULPickerPos_Module_PickUp_WorkTable_X.Size = new System.Drawing.Size(77, 23);
            this.textBox_ULPickerPos_Module_PickUp_WorkTable_X.TabIndex = 220;
            // 
            // baseLabel_ULPickerPos_WorkTable_X
            // 
            this.baseLabel_ULPickerPos_WorkTable_X.AutoSize = true;
            this.baseLabel_ULPickerPos_WorkTable_X.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_ULPickerPos_WorkTable_X.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_ULPickerPos_WorkTable_X.Location = new System.Drawing.Point(129, 78);
            this.baseLabel_ULPickerPos_WorkTable_X.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_ULPickerPos_WorkTable_X.Name = "baseLabel_ULPickerPos_WorkTable_X";
            this.baseLabel_ULPickerPos_WorkTable_X.Size = new System.Drawing.Size(43, 16);
            this.baseLabel_ULPickerPos_WorkTable_X.TabIndex = 219;
            this.baseLabel_ULPickerPos_WorkTable_X.Text = "Pos. X";
            // 
            // baseLabel_ULPickerPos_WorkTable
            // 
            this.baseLabel_ULPickerPos_WorkTable.BackColor = System.Drawing.Color.Silver;
            this.baseLabel_ULPickerPos_WorkTable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_ULPickerPos_WorkTable.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_ULPickerPos_WorkTable.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_ULPickerPos_WorkTable.Location = new System.Drawing.Point(10, 73);
            this.baseLabel_ULPickerPos_WorkTable.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_ULPickerPos_WorkTable.Name = "baseLabel_ULPickerPos_WorkTable";
            this.baseLabel_ULPickerPos_WorkTable.Size = new System.Drawing.Size(117, 55);
            this.baseLabel_ULPickerPos_WorkTable.TabIndex = 218;
            this.baseLabel_ULPickerPos_WorkTable.Text = "[  Transfer  ]\r\nModule PickUp\r\nfrom Work Stage";
            this.baseLabel_ULPickerPos_WorkTable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabPage_Pos_WorkStage
            // 
            this.tabPage_Pos_WorkStage.BackColor = System.Drawing.Color.Gainsboro;
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel72);
            this.tabPage_Pos_WorkStage.Controls.Add(this.button_MainUnitPos_Mask4_Get);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_Mask4_FwBw_Y);
            this.tabPage_Pos_WorkStage.Controls.Add(this.button_MainUnitPos_Mask3_Get);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_Mask3_FwBw_Y);
            this.tabPage_Pos_WorkStage.Controls.Add(this.button_MainUnitPos_Mask2_Get);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_Mask2_FwBw_Y);
            this.tabPage_Pos_WorkStage.Controls.Add(this.button_MainUnitPos_Mask1_Get);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_Mask1_FwBw_Y);
            this.tabPage_Pos_WorkStage.Controls.Add(this.button_MainUnitPos_MaskEmpty_Get);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_MaskEmpty_FwBw_Y);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_HeightSensorCalSheetLT_Z);
            this.tabPage_Pos_WorkStage.Controls.Add(this.button_MainUnitPos_HeightSensorCalSheetLT_Get);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_HeightSensorCalSheetLT_Y);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_HeightSensorCalSheetLT_X);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_HeightSensorStageCenter_Z);
            this.tabPage_Pos_WorkStage.Controls.Add(this.button_MainUnitPos_HeightSensorWorkStageCenter_Get);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_HeightSensorStageCenter_Y);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_HeightSensorStageCenter_X);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_LowResCamReticleGlass_Z);
            this.tabPage_Pos_WorkStage.Controls.Add(this.button_MainUnitPos_LowResCamReticleGlass_Get);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_LowResCamReticleGlass_Y);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_LowResCamReticleGlass_X);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_LowResCamCalSheetLT_Z);
            this.tabPage_Pos_WorkStage.Controls.Add(this.button_MainUnitPos_LowResCamCalSheetLT_Get);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_LowResCamCalSheetLT_Y);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_LowResCamCalSheetLT_X);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_LowResCamWorkStageCenter_Z);
            this.tabPage_Pos_WorkStage.Controls.Add(this.button_MainUnitPos_LowResCamWorkStageCenter_Get);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_LowResCamWorkStageCenter_Y);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_LowResCamWorkStageCenter_X);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_HighResCamReticleGlass_Z);
            this.tabPage_Pos_WorkStage.Controls.Add(this.button_MainUnitPos_HighResCamReticleGlass_Get);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_HighResCamReticleGlass_Y);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_HighResCamReticleGlass_X);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_HighResCamCalSheetLT_Z);
            this.tabPage_Pos_WorkStage.Controls.Add(this.button_MainUnitPos_HighResCamCalSheetLT_Get);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_HighResCamCalSheetLT_Y);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_HighResCamCalSheetLT_X);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_HighResCamWorkStageCenter_Z);
            this.tabPage_Pos_WorkStage.Controls.Add(this.button_MainUnitPos_HighResCamWorkStageCenter_Get);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_HighResCamWorkStageCenter_Y);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_HighResCamWorkStageCenter_X);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_ScannerPowerMeter_Z);
            this.tabPage_Pos_WorkStage.Controls.Add(this.button_MainUnitPos_ScannerPowerMeter_Get);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_ScannerPowerMeter_Y);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_ScannerPowerMeter_X);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_ScannerWorkStageCenter_Z);
            this.tabPage_Pos_WorkStage.Controls.Add(this.button_MainUnitPos_ScannerWorkStageCenter_Get);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_ScannerWorkStageCenter_Y);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_ScannerWorkStageCenter_X);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_ScannerLensCleaning_Z);
            this.tabPage_Pos_WorkStage.Controls.Add(this.button_MainUnitPos_ScannerLensCleaning_Get);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_ScannerLensCleaning_Y);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_ScannerLensCleaning_X);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_ModuleUnload_Z);
            this.tabPage_Pos_WorkStage.Controls.Add(this.button_MainUnitPos_ModuleUnload_Get);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_ModuleUnload_Y);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_ModuleUnload_X);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_ModuleLoad_Z);
            this.tabPage_Pos_WorkStage.Controls.Add(this.button_MainUnitPos_ModuleLoad_Get);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_ModuleLoad_Y);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_ModuleLoad_X);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_JigChange_Z);
            this.tabPage_Pos_WorkStage.Controls.Add(this.button_MainUnitPos_JigChange_Get);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_JigChange_Y);
            this.tabPage_Pos_WorkStage.Controls.Add(this.textBox_MainUnitPos_JigChange_X);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel_MainUnitPos_Mask4);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel74);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel_MainUnitPos_Mask3);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel70);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel_MainUnitPos_Mask2);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel67);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel_MainUnitPos_Mask1);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel64);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel_MainUnitPos_MaskEmpty);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel68);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel55);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel59);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel63);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel_MainUnitPos_HeightSensorCalSheetLT);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel65);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel66);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel_MainUnitPos_HeightSensorStageCenter);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel52);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel53);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel54);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel_MainUnitPos_LowResCameraReticleGlass);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel56);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel57);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel58);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel_MainUnitPos_LowResCameraCalSheetLT);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel60);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel61);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel62);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel_MainUnitPos_LowResCameraStageCenter);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel51);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel49);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel50);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel_MainUnitPos_HighResCameraReticleGlass);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel46);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel47);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel48);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel_MainUnitPos_HighResCameraCalSheetLT);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel43);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel44);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel45);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel_MainUnitPos_HighResCameraStageCenter);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel40);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel41);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel42);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel_MainUnitPos_ScannerPowerMeter);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel37);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel38);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel39);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel_MainUnitPos_ScannerStageCenter);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel34);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel35);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel36);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel_MainUnitPos_ScannerLensCleaning);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel31);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel32);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel33);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel_MainUnitPos_ModuleUnload);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel28);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel29);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel30);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel_MainUnitPos_ModuleLoad);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel27);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel23);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel26);
            this.tabPage_Pos_WorkStage.Controls.Add(this.baseLabel_MainUnitPos_JigChange);
            this.tabPage_Pos_WorkStage.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage_Pos_WorkStage.Location = new System.Drawing.Point(4, 30);
            this.tabPage_Pos_WorkStage.Name = "tabPage_Pos_WorkStage";
            this.tabPage_Pos_WorkStage.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Pos_WorkStage.Size = new System.Drawing.Size(681, 669);
            this.tabPage_Pos_WorkStage.TabIndex = 0;
            this.tabPage_Pos_WorkStage.Text = "Work Stage";
            // 
            // baseLabel72
            // 
            this.baseLabel72.AutoSize = true;
            this.baseLabel72.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel72.ForeColor = System.Drawing.Color.Black;
            this.baseLabel72.Location = new System.Drawing.Point(480, 631);
            this.baseLabel72.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel72.Name = "baseLabel72";
            this.baseLabel72.Size = new System.Drawing.Size(36, 16);
            this.baseLabel72.TabIndex = 367;
            this.baseLabel72.Text = "FB. Y";
            // 
            // button_MainUnitPos_Mask4_Get
            // 
            this.button_MainUnitPos_Mask4_Get.BackColor = System.Drawing.Color.White;
            this.button_MainUnitPos_Mask4_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_MainUnitPos_Mask4_Get.FlatAppearance.BorderSize = 2;
            this.button_MainUnitPos_Mask4_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_MainUnitPos_Mask4_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_MainUnitPos_Mask4_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_MainUnitPos_Mask4_Get.ForeColor = System.Drawing.Color.Black;
            this.button_MainUnitPos_Mask4_Get.Location = new System.Drawing.Point(602, 626);
            this.button_MainUnitPos_Mask4_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_MainUnitPos_Mask4_Get.Name = "button_MainUnitPos_Mask4_Get";
            this.button_MainUnitPos_Mask4_Get.Size = new System.Drawing.Size(70, 25);
            this.button_MainUnitPos_Mask4_Get.TabIndex = 366;
            this.button_MainUnitPos_Mask4_Get.Text = "Get Pos.";
            this.button_MainUnitPos_Mask4_Get.UseVisualStyleBackColor = false;
            // 
            // textBox_MainUnitPos_Mask4_FwBw_Y
            // 
            this.textBox_MainUnitPos_Mask4_FwBw_Y.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_Mask4_FwBw_Y.Location = new System.Drawing.Point(524, 628);
            this.textBox_MainUnitPos_Mask4_FwBw_Y.Name = "textBox_MainUnitPos_Mask4_FwBw_Y";
            this.textBox_MainUnitPos_Mask4_FwBw_Y.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_Mask4_FwBw_Y.TabIndex = 365;
            // 
            // button_MainUnitPos_Mask3_Get
            // 
            this.button_MainUnitPos_Mask3_Get.BackColor = System.Drawing.Color.White;
            this.button_MainUnitPos_Mask3_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_MainUnitPos_Mask3_Get.FlatAppearance.BorderSize = 2;
            this.button_MainUnitPos_Mask3_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_MainUnitPos_Mask3_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_MainUnitPos_Mask3_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_MainUnitPos_Mask3_Get.ForeColor = System.Drawing.Color.Black;
            this.button_MainUnitPos_Mask3_Get.Location = new System.Drawing.Point(602, 595);
            this.button_MainUnitPos_Mask3_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_MainUnitPos_Mask3_Get.Name = "button_MainUnitPos_Mask3_Get";
            this.button_MainUnitPos_Mask3_Get.Size = new System.Drawing.Size(70, 25);
            this.button_MainUnitPos_Mask3_Get.TabIndex = 362;
            this.button_MainUnitPos_Mask3_Get.Text = "Get Pos.";
            this.button_MainUnitPos_Mask3_Get.UseVisualStyleBackColor = false;
            // 
            // textBox_MainUnitPos_Mask3_FwBw_Y
            // 
            this.textBox_MainUnitPos_Mask3_FwBw_Y.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_Mask3_FwBw_Y.Location = new System.Drawing.Point(524, 597);
            this.textBox_MainUnitPos_Mask3_FwBw_Y.Name = "textBox_MainUnitPos_Mask3_FwBw_Y";
            this.textBox_MainUnitPos_Mask3_FwBw_Y.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_Mask3_FwBw_Y.TabIndex = 361;
            // 
            // button_MainUnitPos_Mask2_Get
            // 
            this.button_MainUnitPos_Mask2_Get.BackColor = System.Drawing.Color.White;
            this.button_MainUnitPos_Mask2_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_MainUnitPos_Mask2_Get.FlatAppearance.BorderSize = 2;
            this.button_MainUnitPos_Mask2_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_MainUnitPos_Mask2_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_MainUnitPos_Mask2_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_MainUnitPos_Mask2_Get.ForeColor = System.Drawing.Color.Black;
            this.button_MainUnitPos_Mask2_Get.Location = new System.Drawing.Point(602, 564);
            this.button_MainUnitPos_Mask2_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_MainUnitPos_Mask2_Get.Name = "button_MainUnitPos_Mask2_Get";
            this.button_MainUnitPos_Mask2_Get.Size = new System.Drawing.Size(70, 25);
            this.button_MainUnitPos_Mask2_Get.TabIndex = 358;
            this.button_MainUnitPos_Mask2_Get.Text = "Get Pos.";
            this.button_MainUnitPos_Mask2_Get.UseVisualStyleBackColor = false;
            // 
            // textBox_MainUnitPos_Mask2_FwBw_Y
            // 
            this.textBox_MainUnitPos_Mask2_FwBw_Y.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_Mask2_FwBw_Y.Location = new System.Drawing.Point(524, 566);
            this.textBox_MainUnitPos_Mask2_FwBw_Y.Name = "textBox_MainUnitPos_Mask2_FwBw_Y";
            this.textBox_MainUnitPos_Mask2_FwBw_Y.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_Mask2_FwBw_Y.TabIndex = 357;
            // 
            // button_MainUnitPos_Mask1_Get
            // 
            this.button_MainUnitPos_Mask1_Get.BackColor = System.Drawing.Color.White;
            this.button_MainUnitPos_Mask1_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_MainUnitPos_Mask1_Get.FlatAppearance.BorderSize = 2;
            this.button_MainUnitPos_Mask1_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_MainUnitPos_Mask1_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_MainUnitPos_Mask1_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_MainUnitPos_Mask1_Get.ForeColor = System.Drawing.Color.Black;
            this.button_MainUnitPos_Mask1_Get.Location = new System.Drawing.Point(602, 533);
            this.button_MainUnitPos_Mask1_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_MainUnitPos_Mask1_Get.Name = "button_MainUnitPos_Mask1_Get";
            this.button_MainUnitPos_Mask1_Get.Size = new System.Drawing.Size(70, 25);
            this.button_MainUnitPos_Mask1_Get.TabIndex = 354;
            this.button_MainUnitPos_Mask1_Get.Text = "Get Pos.";
            this.button_MainUnitPos_Mask1_Get.UseVisualStyleBackColor = false;
            // 
            // textBox_MainUnitPos_Mask1_FwBw_Y
            // 
            this.textBox_MainUnitPos_Mask1_FwBw_Y.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_Mask1_FwBw_Y.Location = new System.Drawing.Point(524, 535);
            this.textBox_MainUnitPos_Mask1_FwBw_Y.Name = "textBox_MainUnitPos_Mask1_FwBw_Y";
            this.textBox_MainUnitPos_Mask1_FwBw_Y.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_Mask1_FwBw_Y.TabIndex = 353;
            // 
            // button_MainUnitPos_MaskEmpty_Get
            // 
            this.button_MainUnitPos_MaskEmpty_Get.BackColor = System.Drawing.Color.White;
            this.button_MainUnitPos_MaskEmpty_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_MainUnitPos_MaskEmpty_Get.FlatAppearance.BorderSize = 2;
            this.button_MainUnitPos_MaskEmpty_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_MainUnitPos_MaskEmpty_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_MainUnitPos_MaskEmpty_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_MainUnitPos_MaskEmpty_Get.ForeColor = System.Drawing.Color.Black;
            this.button_MainUnitPos_MaskEmpty_Get.Location = new System.Drawing.Point(602, 502);
            this.button_MainUnitPos_MaskEmpty_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_MainUnitPos_MaskEmpty_Get.Name = "button_MainUnitPos_MaskEmpty_Get";
            this.button_MainUnitPos_MaskEmpty_Get.Size = new System.Drawing.Size(70, 25);
            this.button_MainUnitPos_MaskEmpty_Get.TabIndex = 350;
            this.button_MainUnitPos_MaskEmpty_Get.Text = "Get Pos.";
            this.button_MainUnitPos_MaskEmpty_Get.UseVisualStyleBackColor = false;
            // 
            // textBox_MainUnitPos_MaskEmpty_FwBw_Y
            // 
            this.textBox_MainUnitPos_MaskEmpty_FwBw_Y.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_MaskEmpty_FwBw_Y.Location = new System.Drawing.Point(524, 504);
            this.textBox_MainUnitPos_MaskEmpty_FwBw_Y.Name = "textBox_MainUnitPos_MaskEmpty_FwBw_Y";
            this.textBox_MainUnitPos_MaskEmpty_FwBw_Y.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_MaskEmpty_FwBw_Y.TabIndex = 349;
            // 
            // textBox_MainUnitPos_HeightSensorCalSheetLT_Z
            // 
            this.textBox_MainUnitPos_HeightSensorCalSheetLT_Z.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_HeightSensorCalSheetLT_Z.Location = new System.Drawing.Point(173, 636);
            this.textBox_MainUnitPos_HeightSensorCalSheetLT_Z.Name = "textBox_MainUnitPos_HeightSensorCalSheetLT_Z";
            this.textBox_MainUnitPos_HeightSensorCalSheetLT_Z.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_HeightSensorCalSheetLT_Z.TabIndex = 346;
            // 
            // button_MainUnitPos_HeightSensorCalSheetLT_Get
            // 
            this.button_MainUnitPos_HeightSensorCalSheetLT_Get.BackColor = System.Drawing.Color.White;
            this.button_MainUnitPos_HeightSensorCalSheetLT_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_MainUnitPos_HeightSensorCalSheetLT_Get.FlatAppearance.BorderSize = 2;
            this.button_MainUnitPos_HeightSensorCalSheetLT_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_MainUnitPos_HeightSensorCalSheetLT_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_MainUnitPos_HeightSensorCalSheetLT_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_MainUnitPos_HeightSensorCalSheetLT_Get.ForeColor = System.Drawing.Color.Black;
            this.button_MainUnitPos_HeightSensorCalSheetLT_Get.Location = new System.Drawing.Point(251, 584);
            this.button_MainUnitPos_HeightSensorCalSheetLT_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_MainUnitPos_HeightSensorCalSheetLT_Get.Name = "button_MainUnitPos_HeightSensorCalSheetLT_Get";
            this.button_MainUnitPos_HeightSensorCalSheetLT_Get.Size = new System.Drawing.Size(70, 76);
            this.button_MainUnitPos_HeightSensorCalSheetLT_Get.TabIndex = 344;
            this.button_MainUnitPos_HeightSensorCalSheetLT_Get.Text = "Get Pos.";
            this.button_MainUnitPos_HeightSensorCalSheetLT_Get.UseVisualStyleBackColor = false;
            // 
            // textBox_MainUnitPos_HeightSensorCalSheetLT_Y
            // 
            this.textBox_MainUnitPos_HeightSensorCalSheetLT_Y.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_HeightSensorCalSheetLT_Y.Location = new System.Drawing.Point(173, 611);
            this.textBox_MainUnitPos_HeightSensorCalSheetLT_Y.Name = "textBox_MainUnitPos_HeightSensorCalSheetLT_Y";
            this.textBox_MainUnitPos_HeightSensorCalSheetLT_Y.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_HeightSensorCalSheetLT_Y.TabIndex = 343;
            // 
            // textBox_MainUnitPos_HeightSensorCalSheetLT_X
            // 
            this.textBox_MainUnitPos_HeightSensorCalSheetLT_X.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_HeightSensorCalSheetLT_X.Location = new System.Drawing.Point(173, 586);
            this.textBox_MainUnitPos_HeightSensorCalSheetLT_X.Name = "textBox_MainUnitPos_HeightSensorCalSheetLT_X";
            this.textBox_MainUnitPos_HeightSensorCalSheetLT_X.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_HeightSensorCalSheetLT_X.TabIndex = 341;
            // 
            // textBox_MainUnitPos_HeightSensorStageCenter_Z
            // 
            this.textBox_MainUnitPos_HeightSensorStageCenter_Z.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_HeightSensorStageCenter_Z.Location = new System.Drawing.Point(173, 554);
            this.textBox_MainUnitPos_HeightSensorStageCenter_Z.Name = "textBox_MainUnitPos_HeightSensorStageCenter_Z";
            this.textBox_MainUnitPos_HeightSensorStageCenter_Z.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_HeightSensorStageCenter_Z.TabIndex = 338;
            // 
            // button_MainUnitPos_HeightSensorWorkStageCenter_Get
            // 
            this.button_MainUnitPos_HeightSensorWorkStageCenter_Get.BackColor = System.Drawing.Color.White;
            this.button_MainUnitPos_HeightSensorWorkStageCenter_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_MainUnitPos_HeightSensorWorkStageCenter_Get.FlatAppearance.BorderSize = 2;
            this.button_MainUnitPos_HeightSensorWorkStageCenter_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_MainUnitPos_HeightSensorWorkStageCenter_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_MainUnitPos_HeightSensorWorkStageCenter_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_MainUnitPos_HeightSensorWorkStageCenter_Get.ForeColor = System.Drawing.Color.Black;
            this.button_MainUnitPos_HeightSensorWorkStageCenter_Get.Location = new System.Drawing.Point(251, 502);
            this.button_MainUnitPos_HeightSensorWorkStageCenter_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_MainUnitPos_HeightSensorWorkStageCenter_Get.Name = "button_MainUnitPos_HeightSensorWorkStageCenter_Get";
            this.button_MainUnitPos_HeightSensorWorkStageCenter_Get.Size = new System.Drawing.Size(70, 76);
            this.button_MainUnitPos_HeightSensorWorkStageCenter_Get.TabIndex = 336;
            this.button_MainUnitPos_HeightSensorWorkStageCenter_Get.Text = "Get Pos.";
            this.button_MainUnitPos_HeightSensorWorkStageCenter_Get.UseVisualStyleBackColor = false;
            // 
            // textBox_MainUnitPos_HeightSensorStageCenter_Y
            // 
            this.textBox_MainUnitPos_HeightSensorStageCenter_Y.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_HeightSensorStageCenter_Y.Location = new System.Drawing.Point(173, 529);
            this.textBox_MainUnitPos_HeightSensorStageCenter_Y.Name = "textBox_MainUnitPos_HeightSensorStageCenter_Y";
            this.textBox_MainUnitPos_HeightSensorStageCenter_Y.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_HeightSensorStageCenter_Y.TabIndex = 335;
            // 
            // textBox_MainUnitPos_HeightSensorStageCenter_X
            // 
            this.textBox_MainUnitPos_HeightSensorStageCenter_X.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_HeightSensorStageCenter_X.Location = new System.Drawing.Point(173, 504);
            this.textBox_MainUnitPos_HeightSensorStageCenter_X.Name = "textBox_MainUnitPos_HeightSensorStageCenter_X";
            this.textBox_MainUnitPos_HeightSensorStageCenter_X.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_HeightSensorStageCenter_X.TabIndex = 333;
            // 
            // textBox_MainUnitPos_LowResCamReticleGlass_Z
            // 
            this.textBox_MainUnitPos_LowResCamReticleGlass_Z.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_LowResCamReticleGlass_Z.Location = new System.Drawing.Point(524, 472);
            this.textBox_MainUnitPos_LowResCamReticleGlass_Z.Name = "textBox_MainUnitPos_LowResCamReticleGlass_Z";
            this.textBox_MainUnitPos_LowResCamReticleGlass_Z.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_LowResCamReticleGlass_Z.TabIndex = 330;
            // 
            // button_MainUnitPos_LowResCamReticleGlass_Get
            // 
            this.button_MainUnitPos_LowResCamReticleGlass_Get.BackColor = System.Drawing.Color.White;
            this.button_MainUnitPos_LowResCamReticleGlass_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_MainUnitPos_LowResCamReticleGlass_Get.FlatAppearance.BorderSize = 2;
            this.button_MainUnitPos_LowResCamReticleGlass_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_MainUnitPos_LowResCamReticleGlass_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_MainUnitPos_LowResCamReticleGlass_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_MainUnitPos_LowResCamReticleGlass_Get.ForeColor = System.Drawing.Color.Black;
            this.button_MainUnitPos_LowResCamReticleGlass_Get.Location = new System.Drawing.Point(602, 420);
            this.button_MainUnitPos_LowResCamReticleGlass_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_MainUnitPos_LowResCamReticleGlass_Get.Name = "button_MainUnitPos_LowResCamReticleGlass_Get";
            this.button_MainUnitPos_LowResCamReticleGlass_Get.Size = new System.Drawing.Size(70, 76);
            this.button_MainUnitPos_LowResCamReticleGlass_Get.TabIndex = 328;
            this.button_MainUnitPos_LowResCamReticleGlass_Get.Text = "Get Pos.";
            this.button_MainUnitPos_LowResCamReticleGlass_Get.UseVisualStyleBackColor = false;
            // 
            // textBox_MainUnitPos_LowResCamReticleGlass_Y
            // 
            this.textBox_MainUnitPos_LowResCamReticleGlass_Y.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_LowResCamReticleGlass_Y.Location = new System.Drawing.Point(524, 447);
            this.textBox_MainUnitPos_LowResCamReticleGlass_Y.Name = "textBox_MainUnitPos_LowResCamReticleGlass_Y";
            this.textBox_MainUnitPos_LowResCamReticleGlass_Y.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_LowResCamReticleGlass_Y.TabIndex = 327;
            // 
            // textBox_MainUnitPos_LowResCamReticleGlass_X
            // 
            this.textBox_MainUnitPos_LowResCamReticleGlass_X.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_LowResCamReticleGlass_X.Location = new System.Drawing.Point(524, 422);
            this.textBox_MainUnitPos_LowResCamReticleGlass_X.Name = "textBox_MainUnitPos_LowResCamReticleGlass_X";
            this.textBox_MainUnitPos_LowResCamReticleGlass_X.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_LowResCamReticleGlass_X.TabIndex = 325;
            // 
            // textBox_MainUnitPos_LowResCamCalSheetLT_Z
            // 
            this.textBox_MainUnitPos_LowResCamCalSheetLT_Z.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_LowResCamCalSheetLT_Z.Location = new System.Drawing.Point(524, 390);
            this.textBox_MainUnitPos_LowResCamCalSheetLT_Z.Name = "textBox_MainUnitPos_LowResCamCalSheetLT_Z";
            this.textBox_MainUnitPos_LowResCamCalSheetLT_Z.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_LowResCamCalSheetLT_Z.TabIndex = 323;
            // 
            // button_MainUnitPos_LowResCamCalSheetLT_Get
            // 
            this.button_MainUnitPos_LowResCamCalSheetLT_Get.BackColor = System.Drawing.Color.White;
            this.button_MainUnitPos_LowResCamCalSheetLT_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_MainUnitPos_LowResCamCalSheetLT_Get.FlatAppearance.BorderSize = 2;
            this.button_MainUnitPos_LowResCamCalSheetLT_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_MainUnitPos_LowResCamCalSheetLT_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_MainUnitPos_LowResCamCalSheetLT_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_MainUnitPos_LowResCamCalSheetLT_Get.ForeColor = System.Drawing.Color.Black;
            this.button_MainUnitPos_LowResCamCalSheetLT_Get.Location = new System.Drawing.Point(602, 338);
            this.button_MainUnitPos_LowResCamCalSheetLT_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_MainUnitPos_LowResCamCalSheetLT_Get.Name = "button_MainUnitPos_LowResCamCalSheetLT_Get";
            this.button_MainUnitPos_LowResCamCalSheetLT_Get.Size = new System.Drawing.Size(70, 76);
            this.button_MainUnitPos_LowResCamCalSheetLT_Get.TabIndex = 321;
            this.button_MainUnitPos_LowResCamCalSheetLT_Get.Text = "Get Pos.";
            this.button_MainUnitPos_LowResCamCalSheetLT_Get.UseVisualStyleBackColor = false;
            // 
            // textBox_MainUnitPos_LowResCamCalSheetLT_Y
            // 
            this.textBox_MainUnitPos_LowResCamCalSheetLT_Y.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_LowResCamCalSheetLT_Y.Location = new System.Drawing.Point(524, 365);
            this.textBox_MainUnitPos_LowResCamCalSheetLT_Y.Name = "textBox_MainUnitPos_LowResCamCalSheetLT_Y";
            this.textBox_MainUnitPos_LowResCamCalSheetLT_Y.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_LowResCamCalSheetLT_Y.TabIndex = 320;
            // 
            // textBox_MainUnitPos_LowResCamCalSheetLT_X
            // 
            this.textBox_MainUnitPos_LowResCamCalSheetLT_X.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_LowResCamCalSheetLT_X.Location = new System.Drawing.Point(524, 340);
            this.textBox_MainUnitPos_LowResCamCalSheetLT_X.Name = "textBox_MainUnitPos_LowResCamCalSheetLT_X";
            this.textBox_MainUnitPos_LowResCamCalSheetLT_X.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_LowResCamCalSheetLT_X.TabIndex = 318;
            // 
            // textBox_MainUnitPos_LowResCamWorkStageCenter_Z
            // 
            this.textBox_MainUnitPos_LowResCamWorkStageCenter_Z.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_LowResCamWorkStageCenter_Z.Location = new System.Drawing.Point(524, 308);
            this.textBox_MainUnitPos_LowResCamWorkStageCenter_Z.Name = "textBox_MainUnitPos_LowResCamWorkStageCenter_Z";
            this.textBox_MainUnitPos_LowResCamWorkStageCenter_Z.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_LowResCamWorkStageCenter_Z.TabIndex = 315;
            // 
            // button_MainUnitPos_LowResCamWorkStageCenter_Get
            // 
            this.button_MainUnitPos_LowResCamWorkStageCenter_Get.BackColor = System.Drawing.Color.White;
            this.button_MainUnitPos_LowResCamWorkStageCenter_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_MainUnitPos_LowResCamWorkStageCenter_Get.FlatAppearance.BorderSize = 2;
            this.button_MainUnitPos_LowResCamWorkStageCenter_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_MainUnitPos_LowResCamWorkStageCenter_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_MainUnitPos_LowResCamWorkStageCenter_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_MainUnitPos_LowResCamWorkStageCenter_Get.ForeColor = System.Drawing.Color.Black;
            this.button_MainUnitPos_LowResCamWorkStageCenter_Get.Location = new System.Drawing.Point(602, 256);
            this.button_MainUnitPos_LowResCamWorkStageCenter_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_MainUnitPos_LowResCamWorkStageCenter_Get.Name = "button_MainUnitPos_LowResCamWorkStageCenter_Get";
            this.button_MainUnitPos_LowResCamWorkStageCenter_Get.Size = new System.Drawing.Size(70, 76);
            this.button_MainUnitPos_LowResCamWorkStageCenter_Get.TabIndex = 313;
            this.button_MainUnitPos_LowResCamWorkStageCenter_Get.Text = "Get Pos.";
            this.button_MainUnitPos_LowResCamWorkStageCenter_Get.UseVisualStyleBackColor = false;
            // 
            // textBox_MainUnitPos_LowResCamWorkStageCenter_Y
            // 
            this.textBox_MainUnitPos_LowResCamWorkStageCenter_Y.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_LowResCamWorkStageCenter_Y.Location = new System.Drawing.Point(524, 283);
            this.textBox_MainUnitPos_LowResCamWorkStageCenter_Y.Name = "textBox_MainUnitPos_LowResCamWorkStageCenter_Y";
            this.textBox_MainUnitPos_LowResCamWorkStageCenter_Y.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_LowResCamWorkStageCenter_Y.TabIndex = 312;
            // 
            // textBox_MainUnitPos_LowResCamWorkStageCenter_X
            // 
            this.textBox_MainUnitPos_LowResCamWorkStageCenter_X.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_LowResCamWorkStageCenter_X.Location = new System.Drawing.Point(524, 258);
            this.textBox_MainUnitPos_LowResCamWorkStageCenter_X.Name = "textBox_MainUnitPos_LowResCamWorkStageCenter_X";
            this.textBox_MainUnitPos_LowResCamWorkStageCenter_X.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_LowResCamWorkStageCenter_X.TabIndex = 310;
            // 
            // textBox_MainUnitPos_HighResCamReticleGlass_Z
            // 
            this.textBox_MainUnitPos_HighResCamReticleGlass_Z.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_HighResCamReticleGlass_Z.Location = new System.Drawing.Point(524, 226);
            this.textBox_MainUnitPos_HighResCamReticleGlass_Z.Name = "textBox_MainUnitPos_HighResCamReticleGlass_Z";
            this.textBox_MainUnitPos_HighResCamReticleGlass_Z.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_HighResCamReticleGlass_Z.TabIndex = 306;
            // 
            // button_MainUnitPos_HighResCamReticleGlass_Get
            // 
            this.button_MainUnitPos_HighResCamReticleGlass_Get.BackColor = System.Drawing.Color.White;
            this.button_MainUnitPos_HighResCamReticleGlass_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_MainUnitPos_HighResCamReticleGlass_Get.FlatAppearance.BorderSize = 2;
            this.button_MainUnitPos_HighResCamReticleGlass_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_MainUnitPos_HighResCamReticleGlass_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_MainUnitPos_HighResCamReticleGlass_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_MainUnitPos_HighResCamReticleGlass_Get.ForeColor = System.Drawing.Color.Black;
            this.button_MainUnitPos_HighResCamReticleGlass_Get.Location = new System.Drawing.Point(602, 174);
            this.button_MainUnitPos_HighResCamReticleGlass_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_MainUnitPos_HighResCamReticleGlass_Get.Name = "button_MainUnitPos_HighResCamReticleGlass_Get";
            this.button_MainUnitPos_HighResCamReticleGlass_Get.Size = new System.Drawing.Size(70, 76);
            this.button_MainUnitPos_HighResCamReticleGlass_Get.TabIndex = 304;
            this.button_MainUnitPos_HighResCamReticleGlass_Get.Text = "Get Pos.";
            this.button_MainUnitPos_HighResCamReticleGlass_Get.UseVisualStyleBackColor = false;
            // 
            // textBox_MainUnitPos_HighResCamReticleGlass_Y
            // 
            this.textBox_MainUnitPos_HighResCamReticleGlass_Y.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_HighResCamReticleGlass_Y.Location = new System.Drawing.Point(524, 201);
            this.textBox_MainUnitPos_HighResCamReticleGlass_Y.Name = "textBox_MainUnitPos_HighResCamReticleGlass_Y";
            this.textBox_MainUnitPos_HighResCamReticleGlass_Y.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_HighResCamReticleGlass_Y.TabIndex = 303;
            // 
            // textBox_MainUnitPos_HighResCamReticleGlass_X
            // 
            this.textBox_MainUnitPos_HighResCamReticleGlass_X.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_HighResCamReticleGlass_X.Location = new System.Drawing.Point(524, 176);
            this.textBox_MainUnitPos_HighResCamReticleGlass_X.Name = "textBox_MainUnitPos_HighResCamReticleGlass_X";
            this.textBox_MainUnitPos_HighResCamReticleGlass_X.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_HighResCamReticleGlass_X.TabIndex = 301;
            // 
            // textBox_MainUnitPos_HighResCamCalSheetLT_Z
            // 
            this.textBox_MainUnitPos_HighResCamCalSheetLT_Z.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_HighResCamCalSheetLT_Z.Location = new System.Drawing.Point(524, 144);
            this.textBox_MainUnitPos_HighResCamCalSheetLT_Z.Name = "textBox_MainUnitPos_HighResCamCalSheetLT_Z";
            this.textBox_MainUnitPos_HighResCamCalSheetLT_Z.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_HighResCamCalSheetLT_Z.TabIndex = 299;
            // 
            // button_MainUnitPos_HighResCamCalSheetLT_Get
            // 
            this.button_MainUnitPos_HighResCamCalSheetLT_Get.BackColor = System.Drawing.Color.White;
            this.button_MainUnitPos_HighResCamCalSheetLT_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_MainUnitPos_HighResCamCalSheetLT_Get.FlatAppearance.BorderSize = 2;
            this.button_MainUnitPos_HighResCamCalSheetLT_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_MainUnitPos_HighResCamCalSheetLT_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_MainUnitPos_HighResCamCalSheetLT_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_MainUnitPos_HighResCamCalSheetLT_Get.ForeColor = System.Drawing.Color.Black;
            this.button_MainUnitPos_HighResCamCalSheetLT_Get.Location = new System.Drawing.Point(602, 92);
            this.button_MainUnitPos_HighResCamCalSheetLT_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_MainUnitPos_HighResCamCalSheetLT_Get.Name = "button_MainUnitPos_HighResCamCalSheetLT_Get";
            this.button_MainUnitPos_HighResCamCalSheetLT_Get.Size = new System.Drawing.Size(70, 76);
            this.button_MainUnitPos_HighResCamCalSheetLT_Get.TabIndex = 297;
            this.button_MainUnitPos_HighResCamCalSheetLT_Get.Text = "Get Pos.";
            this.button_MainUnitPos_HighResCamCalSheetLT_Get.UseVisualStyleBackColor = false;
            // 
            // textBox_MainUnitPos_HighResCamCalSheetLT_Y
            // 
            this.textBox_MainUnitPos_HighResCamCalSheetLT_Y.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_HighResCamCalSheetLT_Y.Location = new System.Drawing.Point(524, 119);
            this.textBox_MainUnitPos_HighResCamCalSheetLT_Y.Name = "textBox_MainUnitPos_HighResCamCalSheetLT_Y";
            this.textBox_MainUnitPos_HighResCamCalSheetLT_Y.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_HighResCamCalSheetLT_Y.TabIndex = 296;
            // 
            // textBox_MainUnitPos_HighResCamCalSheetLT_X
            // 
            this.textBox_MainUnitPos_HighResCamCalSheetLT_X.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_HighResCamCalSheetLT_X.Location = new System.Drawing.Point(524, 94);
            this.textBox_MainUnitPos_HighResCamCalSheetLT_X.Name = "textBox_MainUnitPos_HighResCamCalSheetLT_X";
            this.textBox_MainUnitPos_HighResCamCalSheetLT_X.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_HighResCamCalSheetLT_X.TabIndex = 294;
            // 
            // textBox_MainUnitPos_HighResCamWorkStageCenter_Z
            // 
            this.textBox_MainUnitPos_HighResCamWorkStageCenter_Z.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_HighResCamWorkStageCenter_Z.Location = new System.Drawing.Point(524, 62);
            this.textBox_MainUnitPos_HighResCamWorkStageCenter_Z.Name = "textBox_MainUnitPos_HighResCamWorkStageCenter_Z";
            this.textBox_MainUnitPos_HighResCamWorkStageCenter_Z.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_HighResCamWorkStageCenter_Z.TabIndex = 291;
            // 
            // button_MainUnitPos_HighResCamWorkStageCenter_Get
            // 
            this.button_MainUnitPos_HighResCamWorkStageCenter_Get.BackColor = System.Drawing.Color.White;
            this.button_MainUnitPos_HighResCamWorkStageCenter_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_MainUnitPos_HighResCamWorkStageCenter_Get.FlatAppearance.BorderSize = 2;
            this.button_MainUnitPos_HighResCamWorkStageCenter_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_MainUnitPos_HighResCamWorkStageCenter_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_MainUnitPos_HighResCamWorkStageCenter_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_MainUnitPos_HighResCamWorkStageCenter_Get.ForeColor = System.Drawing.Color.Black;
            this.button_MainUnitPos_HighResCamWorkStageCenter_Get.Location = new System.Drawing.Point(602, 10);
            this.button_MainUnitPos_HighResCamWorkStageCenter_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_MainUnitPos_HighResCamWorkStageCenter_Get.Name = "button_MainUnitPos_HighResCamWorkStageCenter_Get";
            this.button_MainUnitPos_HighResCamWorkStageCenter_Get.Size = new System.Drawing.Size(70, 76);
            this.button_MainUnitPos_HighResCamWorkStageCenter_Get.TabIndex = 289;
            this.button_MainUnitPos_HighResCamWorkStageCenter_Get.Text = "Get Pos.";
            this.button_MainUnitPos_HighResCamWorkStageCenter_Get.UseVisualStyleBackColor = false;
            // 
            // textBox_MainUnitPos_HighResCamWorkStageCenter_Y
            // 
            this.textBox_MainUnitPos_HighResCamWorkStageCenter_Y.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_HighResCamWorkStageCenter_Y.Location = new System.Drawing.Point(524, 37);
            this.textBox_MainUnitPos_HighResCamWorkStageCenter_Y.Name = "textBox_MainUnitPos_HighResCamWorkStageCenter_Y";
            this.textBox_MainUnitPos_HighResCamWorkStageCenter_Y.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_HighResCamWorkStageCenter_Y.TabIndex = 288;
            // 
            // textBox_MainUnitPos_HighResCamWorkStageCenter_X
            // 
            this.textBox_MainUnitPos_HighResCamWorkStageCenter_X.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_HighResCamWorkStageCenter_X.Location = new System.Drawing.Point(524, 12);
            this.textBox_MainUnitPos_HighResCamWorkStageCenter_X.Name = "textBox_MainUnitPos_HighResCamWorkStageCenter_X";
            this.textBox_MainUnitPos_HighResCamWorkStageCenter_X.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_HighResCamWorkStageCenter_X.TabIndex = 286;
            // 
            // textBox_MainUnitPos_ScannerPowerMeter_Z
            // 
            this.textBox_MainUnitPos_ScannerPowerMeter_Z.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_ScannerPowerMeter_Z.Location = new System.Drawing.Point(173, 472);
            this.textBox_MainUnitPos_ScannerPowerMeter_Z.Name = "textBox_MainUnitPos_ScannerPowerMeter_Z";
            this.textBox_MainUnitPos_ScannerPowerMeter_Z.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_ScannerPowerMeter_Z.TabIndex = 283;
            // 
            // button_MainUnitPos_ScannerPowerMeter_Get
            // 
            this.button_MainUnitPos_ScannerPowerMeter_Get.BackColor = System.Drawing.Color.White;
            this.button_MainUnitPos_ScannerPowerMeter_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_MainUnitPos_ScannerPowerMeter_Get.FlatAppearance.BorderSize = 2;
            this.button_MainUnitPos_ScannerPowerMeter_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_MainUnitPos_ScannerPowerMeter_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_MainUnitPos_ScannerPowerMeter_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_MainUnitPos_ScannerPowerMeter_Get.ForeColor = System.Drawing.Color.Black;
            this.button_MainUnitPos_ScannerPowerMeter_Get.Location = new System.Drawing.Point(251, 420);
            this.button_MainUnitPos_ScannerPowerMeter_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_MainUnitPos_ScannerPowerMeter_Get.Name = "button_MainUnitPos_ScannerPowerMeter_Get";
            this.button_MainUnitPos_ScannerPowerMeter_Get.Size = new System.Drawing.Size(70, 76);
            this.button_MainUnitPos_ScannerPowerMeter_Get.TabIndex = 281;
            this.button_MainUnitPos_ScannerPowerMeter_Get.Text = "Get Pos.";
            this.button_MainUnitPos_ScannerPowerMeter_Get.UseVisualStyleBackColor = false;
            // 
            // textBox_MainUnitPos_ScannerPowerMeter_Y
            // 
            this.textBox_MainUnitPos_ScannerPowerMeter_Y.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_ScannerPowerMeter_Y.Location = new System.Drawing.Point(173, 447);
            this.textBox_MainUnitPos_ScannerPowerMeter_Y.Name = "textBox_MainUnitPos_ScannerPowerMeter_Y";
            this.textBox_MainUnitPos_ScannerPowerMeter_Y.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_ScannerPowerMeter_Y.TabIndex = 280;
            // 
            // textBox_MainUnitPos_ScannerPowerMeter_X
            // 
            this.textBox_MainUnitPos_ScannerPowerMeter_X.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_ScannerPowerMeter_X.Location = new System.Drawing.Point(173, 422);
            this.textBox_MainUnitPos_ScannerPowerMeter_X.Name = "textBox_MainUnitPos_ScannerPowerMeter_X";
            this.textBox_MainUnitPos_ScannerPowerMeter_X.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_ScannerPowerMeter_X.TabIndex = 278;
            // 
            // textBox_MainUnitPos_ScannerWorkStageCenter_Z
            // 
            this.textBox_MainUnitPos_ScannerWorkStageCenter_Z.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_ScannerWorkStageCenter_Z.Location = new System.Drawing.Point(173, 390);
            this.textBox_MainUnitPos_ScannerWorkStageCenter_Z.Name = "textBox_MainUnitPos_ScannerWorkStageCenter_Z";
            this.textBox_MainUnitPos_ScannerWorkStageCenter_Z.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_ScannerWorkStageCenter_Z.TabIndex = 275;
            // 
            // button_MainUnitPos_ScannerWorkStageCenter_Get
            // 
            this.button_MainUnitPos_ScannerWorkStageCenter_Get.BackColor = System.Drawing.Color.White;
            this.button_MainUnitPos_ScannerWorkStageCenter_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_MainUnitPos_ScannerWorkStageCenter_Get.FlatAppearance.BorderSize = 2;
            this.button_MainUnitPos_ScannerWorkStageCenter_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_MainUnitPos_ScannerWorkStageCenter_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_MainUnitPos_ScannerWorkStageCenter_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_MainUnitPos_ScannerWorkStageCenter_Get.ForeColor = System.Drawing.Color.Black;
            this.button_MainUnitPos_ScannerWorkStageCenter_Get.Location = new System.Drawing.Point(251, 338);
            this.button_MainUnitPos_ScannerWorkStageCenter_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_MainUnitPos_ScannerWorkStageCenter_Get.Name = "button_MainUnitPos_ScannerWorkStageCenter_Get";
            this.button_MainUnitPos_ScannerWorkStageCenter_Get.Size = new System.Drawing.Size(70, 76);
            this.button_MainUnitPos_ScannerWorkStageCenter_Get.TabIndex = 273;
            this.button_MainUnitPos_ScannerWorkStageCenter_Get.Text = "Get Pos.";
            this.button_MainUnitPos_ScannerWorkStageCenter_Get.UseVisualStyleBackColor = false;
            // 
            // textBox_MainUnitPos_ScannerWorkStageCenter_Y
            // 
            this.textBox_MainUnitPos_ScannerWorkStageCenter_Y.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_ScannerWorkStageCenter_Y.Location = new System.Drawing.Point(173, 365);
            this.textBox_MainUnitPos_ScannerWorkStageCenter_Y.Name = "textBox_MainUnitPos_ScannerWorkStageCenter_Y";
            this.textBox_MainUnitPos_ScannerWorkStageCenter_Y.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_ScannerWorkStageCenter_Y.TabIndex = 272;
            // 
            // textBox_MainUnitPos_ScannerWorkStageCenter_X
            // 
            this.textBox_MainUnitPos_ScannerWorkStageCenter_X.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_ScannerWorkStageCenter_X.Location = new System.Drawing.Point(173, 340);
            this.textBox_MainUnitPos_ScannerWorkStageCenter_X.Name = "textBox_MainUnitPos_ScannerWorkStageCenter_X";
            this.textBox_MainUnitPos_ScannerWorkStageCenter_X.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_ScannerWorkStageCenter_X.TabIndex = 270;
            // 
            // textBox_MainUnitPos_ScannerLensCleaning_Z
            // 
            this.textBox_MainUnitPos_ScannerLensCleaning_Z.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_ScannerLensCleaning_Z.Location = new System.Drawing.Point(173, 308);
            this.textBox_MainUnitPos_ScannerLensCleaning_Z.Name = "textBox_MainUnitPos_ScannerLensCleaning_Z";
            this.textBox_MainUnitPos_ScannerLensCleaning_Z.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_ScannerLensCleaning_Z.TabIndex = 267;
            // 
            // button_MainUnitPos_ScannerLensCleaning_Get
            // 
            this.button_MainUnitPos_ScannerLensCleaning_Get.BackColor = System.Drawing.Color.White;
            this.button_MainUnitPos_ScannerLensCleaning_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_MainUnitPos_ScannerLensCleaning_Get.FlatAppearance.BorderSize = 2;
            this.button_MainUnitPos_ScannerLensCleaning_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_MainUnitPos_ScannerLensCleaning_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_MainUnitPos_ScannerLensCleaning_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_MainUnitPos_ScannerLensCleaning_Get.ForeColor = System.Drawing.Color.Black;
            this.button_MainUnitPos_ScannerLensCleaning_Get.Location = new System.Drawing.Point(251, 256);
            this.button_MainUnitPos_ScannerLensCleaning_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_MainUnitPos_ScannerLensCleaning_Get.Name = "button_MainUnitPos_ScannerLensCleaning_Get";
            this.button_MainUnitPos_ScannerLensCleaning_Get.Size = new System.Drawing.Size(70, 76);
            this.button_MainUnitPos_ScannerLensCleaning_Get.TabIndex = 265;
            this.button_MainUnitPos_ScannerLensCleaning_Get.Text = "Get Pos.";
            this.button_MainUnitPos_ScannerLensCleaning_Get.UseVisualStyleBackColor = false;
            // 
            // textBox_MainUnitPos_ScannerLensCleaning_Y
            // 
            this.textBox_MainUnitPos_ScannerLensCleaning_Y.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_ScannerLensCleaning_Y.Location = new System.Drawing.Point(173, 283);
            this.textBox_MainUnitPos_ScannerLensCleaning_Y.Name = "textBox_MainUnitPos_ScannerLensCleaning_Y";
            this.textBox_MainUnitPos_ScannerLensCleaning_Y.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_ScannerLensCleaning_Y.TabIndex = 264;
            // 
            // textBox_MainUnitPos_ScannerLensCleaning_X
            // 
            this.textBox_MainUnitPos_ScannerLensCleaning_X.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_ScannerLensCleaning_X.Location = new System.Drawing.Point(173, 258);
            this.textBox_MainUnitPos_ScannerLensCleaning_X.Name = "textBox_MainUnitPos_ScannerLensCleaning_X";
            this.textBox_MainUnitPos_ScannerLensCleaning_X.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_ScannerLensCleaning_X.TabIndex = 262;
            // 
            // textBox_MainUnitPos_ModuleUnload_Z
            // 
            this.textBox_MainUnitPos_ModuleUnload_Z.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_ModuleUnload_Z.Location = new System.Drawing.Point(173, 226);
            this.textBox_MainUnitPos_ModuleUnload_Z.Name = "textBox_MainUnitPos_ModuleUnload_Z";
            this.textBox_MainUnitPos_ModuleUnload_Z.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_ModuleUnload_Z.TabIndex = 259;
            // 
            // button_MainUnitPos_ModuleUnload_Get
            // 
            this.button_MainUnitPos_ModuleUnload_Get.BackColor = System.Drawing.Color.White;
            this.button_MainUnitPos_ModuleUnload_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_MainUnitPos_ModuleUnload_Get.FlatAppearance.BorderSize = 2;
            this.button_MainUnitPos_ModuleUnload_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_MainUnitPos_ModuleUnload_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_MainUnitPos_ModuleUnload_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_MainUnitPos_ModuleUnload_Get.ForeColor = System.Drawing.Color.Black;
            this.button_MainUnitPos_ModuleUnload_Get.Location = new System.Drawing.Point(251, 174);
            this.button_MainUnitPos_ModuleUnload_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_MainUnitPos_ModuleUnload_Get.Name = "button_MainUnitPos_ModuleUnload_Get";
            this.button_MainUnitPos_ModuleUnload_Get.Size = new System.Drawing.Size(70, 76);
            this.button_MainUnitPos_ModuleUnload_Get.TabIndex = 257;
            this.button_MainUnitPos_ModuleUnload_Get.Text = "Get Pos.";
            this.button_MainUnitPos_ModuleUnload_Get.UseVisualStyleBackColor = false;
            // 
            // textBox_MainUnitPos_ModuleUnload_Y
            // 
            this.textBox_MainUnitPos_ModuleUnload_Y.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_ModuleUnload_Y.Location = new System.Drawing.Point(173, 201);
            this.textBox_MainUnitPos_ModuleUnload_Y.Name = "textBox_MainUnitPos_ModuleUnload_Y";
            this.textBox_MainUnitPos_ModuleUnload_Y.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_ModuleUnload_Y.TabIndex = 256;
            // 
            // textBox_MainUnitPos_ModuleUnload_X
            // 
            this.textBox_MainUnitPos_ModuleUnload_X.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_ModuleUnload_X.Location = new System.Drawing.Point(173, 176);
            this.textBox_MainUnitPos_ModuleUnload_X.Name = "textBox_MainUnitPos_ModuleUnload_X";
            this.textBox_MainUnitPos_ModuleUnload_X.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_ModuleUnload_X.TabIndex = 254;
            // 
            // textBox_MainUnitPos_ModuleLoad_Z
            // 
            this.textBox_MainUnitPos_ModuleLoad_Z.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_ModuleLoad_Z.Location = new System.Drawing.Point(173, 144);
            this.textBox_MainUnitPos_ModuleLoad_Z.Name = "textBox_MainUnitPos_ModuleLoad_Z";
            this.textBox_MainUnitPos_ModuleLoad_Z.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_ModuleLoad_Z.TabIndex = 251;
            // 
            // button_MainUnitPos_ModuleLoad_Get
            // 
            this.button_MainUnitPos_ModuleLoad_Get.BackColor = System.Drawing.Color.White;
            this.button_MainUnitPos_ModuleLoad_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_MainUnitPos_ModuleLoad_Get.FlatAppearance.BorderSize = 2;
            this.button_MainUnitPos_ModuleLoad_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_MainUnitPos_ModuleLoad_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_MainUnitPos_ModuleLoad_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_MainUnitPos_ModuleLoad_Get.ForeColor = System.Drawing.Color.Black;
            this.button_MainUnitPos_ModuleLoad_Get.Location = new System.Drawing.Point(251, 92);
            this.button_MainUnitPos_ModuleLoad_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_MainUnitPos_ModuleLoad_Get.Name = "button_MainUnitPos_ModuleLoad_Get";
            this.button_MainUnitPos_ModuleLoad_Get.Size = new System.Drawing.Size(70, 76);
            this.button_MainUnitPos_ModuleLoad_Get.TabIndex = 249;
            this.button_MainUnitPos_ModuleLoad_Get.Text = "Get Pos.";
            this.button_MainUnitPos_ModuleLoad_Get.UseVisualStyleBackColor = false;
            // 
            // textBox_MainUnitPos_ModuleLoad_Y
            // 
            this.textBox_MainUnitPos_ModuleLoad_Y.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_ModuleLoad_Y.Location = new System.Drawing.Point(173, 119);
            this.textBox_MainUnitPos_ModuleLoad_Y.Name = "textBox_MainUnitPos_ModuleLoad_Y";
            this.textBox_MainUnitPos_ModuleLoad_Y.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_ModuleLoad_Y.TabIndex = 248;
            // 
            // textBox_MainUnitPos_ModuleLoad_X
            // 
            this.textBox_MainUnitPos_ModuleLoad_X.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_ModuleLoad_X.Location = new System.Drawing.Point(173, 94);
            this.textBox_MainUnitPos_ModuleLoad_X.Name = "textBox_MainUnitPos_ModuleLoad_X";
            this.textBox_MainUnitPos_ModuleLoad_X.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_ModuleLoad_X.TabIndex = 246;
            // 
            // textBox_MainUnitPos_JigChange_Z
            // 
            this.textBox_MainUnitPos_JigChange_Z.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_JigChange_Z.Location = new System.Drawing.Point(173, 62);
            this.textBox_MainUnitPos_JigChange_Z.Name = "textBox_MainUnitPos_JigChange_Z";
            this.textBox_MainUnitPos_JigChange_Z.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_JigChange_Z.TabIndex = 243;
            // 
            // button_MainUnitPos_JigChange_Get
            // 
            this.button_MainUnitPos_JigChange_Get.BackColor = System.Drawing.Color.White;
            this.button_MainUnitPos_JigChange_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_MainUnitPos_JigChange_Get.FlatAppearance.BorderSize = 2;
            this.button_MainUnitPos_JigChange_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_MainUnitPos_JigChange_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_MainUnitPos_JigChange_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_MainUnitPos_JigChange_Get.ForeColor = System.Drawing.Color.Black;
            this.button_MainUnitPos_JigChange_Get.Location = new System.Drawing.Point(251, 10);
            this.button_MainUnitPos_JigChange_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_MainUnitPos_JigChange_Get.Name = "button_MainUnitPos_JigChange_Get";
            this.button_MainUnitPos_JigChange_Get.Size = new System.Drawing.Size(70, 76);
            this.button_MainUnitPos_JigChange_Get.TabIndex = 241;
            this.button_MainUnitPos_JigChange_Get.Text = "Get Pos.";
            this.button_MainUnitPos_JigChange_Get.UseVisualStyleBackColor = false;
            // 
            // textBox_MainUnitPos_JigChange_Y
            // 
            this.textBox_MainUnitPos_JigChange_Y.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_JigChange_Y.Location = new System.Drawing.Point(173, 37);
            this.textBox_MainUnitPos_JigChange_Y.Name = "textBox_MainUnitPos_JigChange_Y";
            this.textBox_MainUnitPos_JigChange_Y.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_JigChange_Y.TabIndex = 240;
            // 
            // textBox_MainUnitPos_JigChange_X
            // 
            this.textBox_MainUnitPos_JigChange_X.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_MainUnitPos_JigChange_X.Location = new System.Drawing.Point(173, 12);
            this.textBox_MainUnitPos_JigChange_X.Name = "textBox_MainUnitPos_JigChange_X";
            this.textBox_MainUnitPos_JigChange_X.Size = new System.Drawing.Size(77, 23);
            this.textBox_MainUnitPos_JigChange_X.TabIndex = 238;
            // 
            // baseLabel_MainUnitPos_Mask4
            // 
            this.baseLabel_MainUnitPos_Mask4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_MainUnitPos_Mask4.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_MainUnitPos_Mask4.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_MainUnitPos_Mask4.Location = new System.Drawing.Point(361, 626);
            this.baseLabel_MainUnitPos_Mask4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_MainUnitPos_Mask4.Name = "baseLabel_MainUnitPos_Mask4";
            this.baseLabel_MainUnitPos_Mask4.Size = new System.Drawing.Size(117, 25);
            this.baseLabel_MainUnitPos_Mask4.TabIndex = 364;
            this.baseLabel_MainUnitPos_Mask4.Text = "Mask  [ No. 4 ]";
            this.baseLabel_MainUnitPos_Mask4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel74
            // 
            this.baseLabel74.AutoSize = true;
            this.baseLabel74.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel74.ForeColor = System.Drawing.Color.Black;
            this.baseLabel74.Location = new System.Drawing.Point(480, 600);
            this.baseLabel74.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel74.Name = "baseLabel74";
            this.baseLabel74.Size = new System.Drawing.Size(36, 16);
            this.baseLabel74.TabIndex = 363;
            this.baseLabel74.Text = "FB. Y";
            // 
            // baseLabel_MainUnitPos_Mask3
            // 
            this.baseLabel_MainUnitPos_Mask3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_MainUnitPos_Mask3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_MainUnitPos_Mask3.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_MainUnitPos_Mask3.Location = new System.Drawing.Point(361, 595);
            this.baseLabel_MainUnitPos_Mask3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_MainUnitPos_Mask3.Name = "baseLabel_MainUnitPos_Mask3";
            this.baseLabel_MainUnitPos_Mask3.Size = new System.Drawing.Size(117, 25);
            this.baseLabel_MainUnitPos_Mask3.TabIndex = 360;
            this.baseLabel_MainUnitPos_Mask3.Text = "Mask  [ No. 3 ]";
            this.baseLabel_MainUnitPos_Mask3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel70
            // 
            this.baseLabel70.AutoSize = true;
            this.baseLabel70.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel70.ForeColor = System.Drawing.Color.Black;
            this.baseLabel70.Location = new System.Drawing.Point(480, 569);
            this.baseLabel70.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel70.Name = "baseLabel70";
            this.baseLabel70.Size = new System.Drawing.Size(36, 16);
            this.baseLabel70.TabIndex = 359;
            this.baseLabel70.Text = "FB. Y";
            // 
            // baseLabel_MainUnitPos_Mask2
            // 
            this.baseLabel_MainUnitPos_Mask2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_MainUnitPos_Mask2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_MainUnitPos_Mask2.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_MainUnitPos_Mask2.Location = new System.Drawing.Point(361, 564);
            this.baseLabel_MainUnitPos_Mask2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_MainUnitPos_Mask2.Name = "baseLabel_MainUnitPos_Mask2";
            this.baseLabel_MainUnitPos_Mask2.Size = new System.Drawing.Size(117, 25);
            this.baseLabel_MainUnitPos_Mask2.TabIndex = 356;
            this.baseLabel_MainUnitPos_Mask2.Text = "Mask  [ No. 2 ]";
            this.baseLabel_MainUnitPos_Mask2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel67
            // 
            this.baseLabel67.AutoSize = true;
            this.baseLabel67.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel67.ForeColor = System.Drawing.Color.Black;
            this.baseLabel67.Location = new System.Drawing.Point(480, 538);
            this.baseLabel67.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel67.Name = "baseLabel67";
            this.baseLabel67.Size = new System.Drawing.Size(36, 16);
            this.baseLabel67.TabIndex = 355;
            this.baseLabel67.Text = "FB. Y";
            // 
            // baseLabel_MainUnitPos_Mask1
            // 
            this.baseLabel_MainUnitPos_Mask1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_MainUnitPos_Mask1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_MainUnitPos_Mask1.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_MainUnitPos_Mask1.Location = new System.Drawing.Point(361, 533);
            this.baseLabel_MainUnitPos_Mask1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_MainUnitPos_Mask1.Name = "baseLabel_MainUnitPos_Mask1";
            this.baseLabel_MainUnitPos_Mask1.Size = new System.Drawing.Size(117, 25);
            this.baseLabel_MainUnitPos_Mask1.TabIndex = 352;
            this.baseLabel_MainUnitPos_Mask1.Text = "Mask  [ No. 1 ]";
            this.baseLabel_MainUnitPos_Mask1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel64
            // 
            this.baseLabel64.AutoSize = true;
            this.baseLabel64.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel64.ForeColor = System.Drawing.Color.Black;
            this.baseLabel64.Location = new System.Drawing.Point(480, 507);
            this.baseLabel64.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel64.Name = "baseLabel64";
            this.baseLabel64.Size = new System.Drawing.Size(36, 16);
            this.baseLabel64.TabIndex = 351;
            this.baseLabel64.Text = "FB. Y";
            // 
            // baseLabel_MainUnitPos_MaskEmpty
            // 
            this.baseLabel_MainUnitPos_MaskEmpty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_MainUnitPos_MaskEmpty.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_MainUnitPos_MaskEmpty.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_MainUnitPos_MaskEmpty.Location = new System.Drawing.Point(361, 502);
            this.baseLabel_MainUnitPos_MaskEmpty.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_MainUnitPos_MaskEmpty.Name = "baseLabel_MainUnitPos_MaskEmpty";
            this.baseLabel_MainUnitPos_MaskEmpty.Size = new System.Drawing.Size(117, 25);
            this.baseLabel_MainUnitPos_MaskEmpty.TabIndex = 348;
            this.baseLabel_MainUnitPos_MaskEmpty.Text = "Mask  [ Empty ]";
            this.baseLabel_MainUnitPos_MaskEmpty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel68
            // 
            this.baseLabel68.AutoSize = true;
            this.baseLabel68.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel68.ForeColor = System.Drawing.Color.Black;
            this.baseLabel68.Location = new System.Drawing.Point(129, 507);
            this.baseLabel68.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel68.Name = "baseLabel68";
            this.baseLabel68.Size = new System.Drawing.Size(43, 16);
            this.baseLabel68.TabIndex = 347;
            this.baseLabel68.Text = "Pos. X";
            // 
            // baseLabel55
            // 
            this.baseLabel55.AutoSize = true;
            this.baseLabel55.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel55.ForeColor = System.Drawing.Color.Black;
            this.baseLabel55.Location = new System.Drawing.Point(129, 639);
            this.baseLabel55.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel55.Name = "baseLabel55";
            this.baseLabel55.Size = new System.Drawing.Size(42, 16);
            this.baseLabel55.TabIndex = 345;
            this.baseLabel55.Text = "Pos. Z";
            // 
            // baseLabel59
            // 
            this.baseLabel59.AutoSize = true;
            this.baseLabel59.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel59.ForeColor = System.Drawing.Color.Black;
            this.baseLabel59.Location = new System.Drawing.Point(129, 614);
            this.baseLabel59.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel59.Name = "baseLabel59";
            this.baseLabel59.Size = new System.Drawing.Size(42, 16);
            this.baseLabel59.TabIndex = 342;
            this.baseLabel59.Text = "Pos. Y";
            // 
            // baseLabel63
            // 
            this.baseLabel63.AutoSize = true;
            this.baseLabel63.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel63.ForeColor = System.Drawing.Color.Black;
            this.baseLabel63.Location = new System.Drawing.Point(129, 589);
            this.baseLabel63.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel63.Name = "baseLabel63";
            this.baseLabel63.Size = new System.Drawing.Size(43, 16);
            this.baseLabel63.TabIndex = 340;
            this.baseLabel63.Text = "Pos. X";
            // 
            // baseLabel_MainUnitPos_HeightSensorCalSheetLT
            // 
            this.baseLabel_MainUnitPos_HeightSensorCalSheetLT.BackColor = System.Drawing.Color.Silver;
            this.baseLabel_MainUnitPos_HeightSensorCalSheetLT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_MainUnitPos_HeightSensorCalSheetLT.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_MainUnitPos_HeightSensorCalSheetLT.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_MainUnitPos_HeightSensorCalSheetLT.Location = new System.Drawing.Point(10, 584);
            this.baseLabel_MainUnitPos_HeightSensorCalSheetLT.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_MainUnitPos_HeightSensorCalSheetLT.Name = "baseLabel_MainUnitPos_HeightSensorCalSheetLT";
            this.baseLabel_MainUnitPos_HeightSensorCalSheetLT.Size = new System.Drawing.Size(117, 76);
            this.baseLabel_MainUnitPos_HeightSensorCalSheetLT.TabIndex = 339;
            this.baseLabel_MainUnitPos_HeightSensorCalSheetLT.Text = "[ Height Sensor ]\r\n\r\nCal. Sheet (Acryl)\r\nLeft - Top";
            this.baseLabel_MainUnitPos_HeightSensorCalSheetLT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel65
            // 
            this.baseLabel65.AutoSize = true;
            this.baseLabel65.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel65.ForeColor = System.Drawing.Color.Black;
            this.baseLabel65.Location = new System.Drawing.Point(129, 557);
            this.baseLabel65.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel65.Name = "baseLabel65";
            this.baseLabel65.Size = new System.Drawing.Size(42, 16);
            this.baseLabel65.TabIndex = 337;
            this.baseLabel65.Text = "Pos. Z";
            // 
            // baseLabel66
            // 
            this.baseLabel66.AutoSize = true;
            this.baseLabel66.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel66.ForeColor = System.Drawing.Color.Black;
            this.baseLabel66.Location = new System.Drawing.Point(129, 532);
            this.baseLabel66.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel66.Name = "baseLabel66";
            this.baseLabel66.Size = new System.Drawing.Size(42, 16);
            this.baseLabel66.TabIndex = 334;
            this.baseLabel66.Text = "Pos. Y";
            // 
            // baseLabel_MainUnitPos_HeightSensorStageCenter
            // 
            this.baseLabel_MainUnitPos_HeightSensorStageCenter.BackColor = System.Drawing.Color.Silver;
            this.baseLabel_MainUnitPos_HeightSensorStageCenter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_MainUnitPos_HeightSensorStageCenter.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_MainUnitPos_HeightSensorStageCenter.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_MainUnitPos_HeightSensorStageCenter.Location = new System.Drawing.Point(10, 502);
            this.baseLabel_MainUnitPos_HeightSensorStageCenter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_MainUnitPos_HeightSensorStageCenter.Name = "baseLabel_MainUnitPos_HeightSensorStageCenter";
            this.baseLabel_MainUnitPos_HeightSensorStageCenter.Size = new System.Drawing.Size(117, 76);
            this.baseLabel_MainUnitPos_HeightSensorStageCenter.TabIndex = 332;
            this.baseLabel_MainUnitPos_HeightSensorStageCenter.Text = "[ Height Sensor ]\r\n\r\nWork Stage\r\nCenter";
            this.baseLabel_MainUnitPos_HeightSensorStageCenter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel52
            // 
            this.baseLabel52.AutoSize = true;
            this.baseLabel52.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel52.ForeColor = System.Drawing.Color.Black;
            this.baseLabel52.Location = new System.Drawing.Point(480, 425);
            this.baseLabel52.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel52.Name = "baseLabel52";
            this.baseLabel52.Size = new System.Drawing.Size(43, 16);
            this.baseLabel52.TabIndex = 331;
            this.baseLabel52.Text = "Pos. X";
            // 
            // baseLabel53
            // 
            this.baseLabel53.AutoSize = true;
            this.baseLabel53.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel53.ForeColor = System.Drawing.Color.Black;
            this.baseLabel53.Location = new System.Drawing.Point(480, 475);
            this.baseLabel53.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel53.Name = "baseLabel53";
            this.baseLabel53.Size = new System.Drawing.Size(42, 16);
            this.baseLabel53.TabIndex = 329;
            this.baseLabel53.Text = "Pos. Z";
            // 
            // baseLabel54
            // 
            this.baseLabel54.AutoSize = true;
            this.baseLabel54.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel54.ForeColor = System.Drawing.Color.Black;
            this.baseLabel54.Location = new System.Drawing.Point(480, 450);
            this.baseLabel54.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel54.Name = "baseLabel54";
            this.baseLabel54.Size = new System.Drawing.Size(42, 16);
            this.baseLabel54.TabIndex = 326;
            this.baseLabel54.Text = "Pos. Y";
            // 
            // baseLabel_MainUnitPos_LowResCameraReticleGlass
            // 
            this.baseLabel_MainUnitPos_LowResCameraReticleGlass.BackColor = System.Drawing.Color.Silver;
            this.baseLabel_MainUnitPos_LowResCameraReticleGlass.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_MainUnitPos_LowResCameraReticleGlass.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_MainUnitPos_LowResCameraReticleGlass.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_MainUnitPos_LowResCameraReticleGlass.Location = new System.Drawing.Point(361, 420);
            this.baseLabel_MainUnitPos_LowResCameraReticleGlass.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_MainUnitPos_LowResCameraReticleGlass.Name = "baseLabel_MainUnitPos_LowResCameraReticleGlass";
            this.baseLabel_MainUnitPos_LowResCameraReticleGlass.Size = new System.Drawing.Size(117, 76);
            this.baseLabel_MainUnitPos_LowResCameraReticleGlass.TabIndex = 324;
            this.baseLabel_MainUnitPos_LowResCameraReticleGlass.Text = "[ Low Res. Cam. ]\r\n\r\nReticle Glass";
            this.baseLabel_MainUnitPos_LowResCameraReticleGlass.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel56
            // 
            this.baseLabel56.AutoSize = true;
            this.baseLabel56.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel56.ForeColor = System.Drawing.Color.Black;
            this.baseLabel56.Location = new System.Drawing.Point(480, 393);
            this.baseLabel56.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel56.Name = "baseLabel56";
            this.baseLabel56.Size = new System.Drawing.Size(42, 16);
            this.baseLabel56.TabIndex = 322;
            this.baseLabel56.Text = "Pos. Z";
            // 
            // baseLabel57
            // 
            this.baseLabel57.AutoSize = true;
            this.baseLabel57.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel57.ForeColor = System.Drawing.Color.Black;
            this.baseLabel57.Location = new System.Drawing.Point(480, 368);
            this.baseLabel57.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel57.Name = "baseLabel57";
            this.baseLabel57.Size = new System.Drawing.Size(42, 16);
            this.baseLabel57.TabIndex = 319;
            this.baseLabel57.Text = "Pos. Y";
            // 
            // baseLabel58
            // 
            this.baseLabel58.AutoSize = true;
            this.baseLabel58.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel58.ForeColor = System.Drawing.Color.Black;
            this.baseLabel58.Location = new System.Drawing.Point(478, 343);
            this.baseLabel58.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel58.Name = "baseLabel58";
            this.baseLabel58.Size = new System.Drawing.Size(43, 16);
            this.baseLabel58.TabIndex = 317;
            this.baseLabel58.Text = "Pos. X";
            // 
            // baseLabel_MainUnitPos_LowResCameraCalSheetLT
            // 
            this.baseLabel_MainUnitPos_LowResCameraCalSheetLT.BackColor = System.Drawing.Color.Silver;
            this.baseLabel_MainUnitPos_LowResCameraCalSheetLT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_MainUnitPos_LowResCameraCalSheetLT.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_MainUnitPos_LowResCameraCalSheetLT.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_MainUnitPos_LowResCameraCalSheetLT.Location = new System.Drawing.Point(361, 338);
            this.baseLabel_MainUnitPos_LowResCameraCalSheetLT.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_MainUnitPos_LowResCameraCalSheetLT.Name = "baseLabel_MainUnitPos_LowResCameraCalSheetLT";
            this.baseLabel_MainUnitPos_LowResCameraCalSheetLT.Size = new System.Drawing.Size(117, 76);
            this.baseLabel_MainUnitPos_LowResCameraCalSheetLT.TabIndex = 316;
            this.baseLabel_MainUnitPos_LowResCameraCalSheetLT.Text = "[ Low Res. Cam. ]\r\n\r\nCal. Sheet (Acryl)\r\nLeft - Top";
            this.baseLabel_MainUnitPos_LowResCameraCalSheetLT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel60
            // 
            this.baseLabel60.AutoSize = true;
            this.baseLabel60.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel60.ForeColor = System.Drawing.Color.Black;
            this.baseLabel60.Location = new System.Drawing.Point(480, 311);
            this.baseLabel60.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel60.Name = "baseLabel60";
            this.baseLabel60.Size = new System.Drawing.Size(42, 16);
            this.baseLabel60.TabIndex = 314;
            this.baseLabel60.Text = "Pos. Z";
            // 
            // baseLabel61
            // 
            this.baseLabel61.AutoSize = true;
            this.baseLabel61.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel61.ForeColor = System.Drawing.Color.Black;
            this.baseLabel61.Location = new System.Drawing.Point(480, 286);
            this.baseLabel61.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel61.Name = "baseLabel61";
            this.baseLabel61.Size = new System.Drawing.Size(42, 16);
            this.baseLabel61.TabIndex = 311;
            this.baseLabel61.Text = "Pos. Y";
            // 
            // baseLabel62
            // 
            this.baseLabel62.AutoSize = true;
            this.baseLabel62.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel62.ForeColor = System.Drawing.Color.Black;
            this.baseLabel62.Location = new System.Drawing.Point(478, 261);
            this.baseLabel62.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel62.Name = "baseLabel62";
            this.baseLabel62.Size = new System.Drawing.Size(43, 16);
            this.baseLabel62.TabIndex = 309;
            this.baseLabel62.Text = "Pos. X";
            // 
            // baseLabel_MainUnitPos_LowResCameraStageCenter
            // 
            this.baseLabel_MainUnitPos_LowResCameraStageCenter.BackColor = System.Drawing.Color.Silver;
            this.baseLabel_MainUnitPos_LowResCameraStageCenter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_MainUnitPos_LowResCameraStageCenter.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_MainUnitPos_LowResCameraStageCenter.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_MainUnitPos_LowResCameraStageCenter.Location = new System.Drawing.Point(361, 256);
            this.baseLabel_MainUnitPos_LowResCameraStageCenter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_MainUnitPos_LowResCameraStageCenter.Name = "baseLabel_MainUnitPos_LowResCameraStageCenter";
            this.baseLabel_MainUnitPos_LowResCameraStageCenter.Size = new System.Drawing.Size(117, 76);
            this.baseLabel_MainUnitPos_LowResCameraStageCenter.TabIndex = 308;
            this.baseLabel_MainUnitPos_LowResCameraStageCenter.Text = "[ Low Res. Cam. ]\r\n\r\nWork Stage\r\nCenter";
            this.baseLabel_MainUnitPos_LowResCameraStageCenter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel51
            // 
            this.baseLabel51.AutoSize = true;
            this.baseLabel51.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel51.ForeColor = System.Drawing.Color.Black;
            this.baseLabel51.Location = new System.Drawing.Point(480, 179);
            this.baseLabel51.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel51.Name = "baseLabel51";
            this.baseLabel51.Size = new System.Drawing.Size(43, 16);
            this.baseLabel51.TabIndex = 307;
            this.baseLabel51.Text = "Pos. X";
            // 
            // baseLabel49
            // 
            this.baseLabel49.AutoSize = true;
            this.baseLabel49.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel49.ForeColor = System.Drawing.Color.Black;
            this.baseLabel49.Location = new System.Drawing.Point(480, 229);
            this.baseLabel49.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel49.Name = "baseLabel49";
            this.baseLabel49.Size = new System.Drawing.Size(42, 16);
            this.baseLabel49.TabIndex = 305;
            this.baseLabel49.Text = "Pos. Z";
            // 
            // baseLabel50
            // 
            this.baseLabel50.AutoSize = true;
            this.baseLabel50.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel50.ForeColor = System.Drawing.Color.Black;
            this.baseLabel50.Location = new System.Drawing.Point(480, 204);
            this.baseLabel50.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel50.Name = "baseLabel50";
            this.baseLabel50.Size = new System.Drawing.Size(42, 16);
            this.baseLabel50.TabIndex = 302;
            this.baseLabel50.Text = "Pos. Y";
            // 
            // baseLabel_MainUnitPos_HighResCameraReticleGlass
            // 
            this.baseLabel_MainUnitPos_HighResCameraReticleGlass.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_MainUnitPos_HighResCameraReticleGlass.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_MainUnitPos_HighResCameraReticleGlass.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_MainUnitPos_HighResCameraReticleGlass.Location = new System.Drawing.Point(361, 174);
            this.baseLabel_MainUnitPos_HighResCameraReticleGlass.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_MainUnitPos_HighResCameraReticleGlass.Name = "baseLabel_MainUnitPos_HighResCameraReticleGlass";
            this.baseLabel_MainUnitPos_HighResCameraReticleGlass.Size = new System.Drawing.Size(117, 76);
            this.baseLabel_MainUnitPos_HighResCameraReticleGlass.TabIndex = 300;
            this.baseLabel_MainUnitPos_HighResCameraReticleGlass.Text = "[ HighRes. Cam. ]\r\n\r\nReticle Glass";
            this.baseLabel_MainUnitPos_HighResCameraReticleGlass.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel46
            // 
            this.baseLabel46.AutoSize = true;
            this.baseLabel46.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel46.ForeColor = System.Drawing.Color.Black;
            this.baseLabel46.Location = new System.Drawing.Point(480, 147);
            this.baseLabel46.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel46.Name = "baseLabel46";
            this.baseLabel46.Size = new System.Drawing.Size(42, 16);
            this.baseLabel46.TabIndex = 298;
            this.baseLabel46.Text = "Pos. Z";
            // 
            // baseLabel47
            // 
            this.baseLabel47.AutoSize = true;
            this.baseLabel47.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel47.ForeColor = System.Drawing.Color.Black;
            this.baseLabel47.Location = new System.Drawing.Point(480, 122);
            this.baseLabel47.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel47.Name = "baseLabel47";
            this.baseLabel47.Size = new System.Drawing.Size(42, 16);
            this.baseLabel47.TabIndex = 295;
            this.baseLabel47.Text = "Pos. Y";
            // 
            // baseLabel48
            // 
            this.baseLabel48.AutoSize = true;
            this.baseLabel48.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel48.ForeColor = System.Drawing.Color.Black;
            this.baseLabel48.Location = new System.Drawing.Point(478, 97);
            this.baseLabel48.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel48.Name = "baseLabel48";
            this.baseLabel48.Size = new System.Drawing.Size(43, 16);
            this.baseLabel48.TabIndex = 293;
            this.baseLabel48.Text = "Pos. X";
            // 
            // baseLabel_MainUnitPos_HighResCameraCalSheetLT
            // 
            this.baseLabel_MainUnitPos_HighResCameraCalSheetLT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_MainUnitPos_HighResCameraCalSheetLT.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_MainUnitPos_HighResCameraCalSheetLT.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_MainUnitPos_HighResCameraCalSheetLT.Location = new System.Drawing.Point(361, 92);
            this.baseLabel_MainUnitPos_HighResCameraCalSheetLT.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_MainUnitPos_HighResCameraCalSheetLT.Name = "baseLabel_MainUnitPos_HighResCameraCalSheetLT";
            this.baseLabel_MainUnitPos_HighResCameraCalSheetLT.Size = new System.Drawing.Size(117, 76);
            this.baseLabel_MainUnitPos_HighResCameraCalSheetLT.TabIndex = 292;
            this.baseLabel_MainUnitPos_HighResCameraCalSheetLT.Text = "[ HighRes. Cam. ]\r\n\r\nCal. Sheet (Acryl)\r\nLeft - Top";
            this.baseLabel_MainUnitPos_HighResCameraCalSheetLT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel43
            // 
            this.baseLabel43.AutoSize = true;
            this.baseLabel43.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel43.ForeColor = System.Drawing.Color.Black;
            this.baseLabel43.Location = new System.Drawing.Point(480, 65);
            this.baseLabel43.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel43.Name = "baseLabel43";
            this.baseLabel43.Size = new System.Drawing.Size(42, 16);
            this.baseLabel43.TabIndex = 290;
            this.baseLabel43.Text = "Pos. Z";
            // 
            // baseLabel44
            // 
            this.baseLabel44.AutoSize = true;
            this.baseLabel44.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel44.ForeColor = System.Drawing.Color.Black;
            this.baseLabel44.Location = new System.Drawing.Point(480, 40);
            this.baseLabel44.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel44.Name = "baseLabel44";
            this.baseLabel44.Size = new System.Drawing.Size(42, 16);
            this.baseLabel44.TabIndex = 287;
            this.baseLabel44.Text = "Pos. Y";
            // 
            // baseLabel45
            // 
            this.baseLabel45.AutoSize = true;
            this.baseLabel45.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel45.ForeColor = System.Drawing.Color.Black;
            this.baseLabel45.Location = new System.Drawing.Point(478, 15);
            this.baseLabel45.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel45.Name = "baseLabel45";
            this.baseLabel45.Size = new System.Drawing.Size(43, 16);
            this.baseLabel45.TabIndex = 285;
            this.baseLabel45.Text = "Pos. X";
            // 
            // baseLabel_MainUnitPos_HighResCameraStageCenter
            // 
            this.baseLabel_MainUnitPos_HighResCameraStageCenter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_MainUnitPos_HighResCameraStageCenter.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_MainUnitPos_HighResCameraStageCenter.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_MainUnitPos_HighResCameraStageCenter.Location = new System.Drawing.Point(361, 10);
            this.baseLabel_MainUnitPos_HighResCameraStageCenter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_MainUnitPos_HighResCameraStageCenter.Name = "baseLabel_MainUnitPos_HighResCameraStageCenter";
            this.baseLabel_MainUnitPos_HighResCameraStageCenter.Size = new System.Drawing.Size(117, 76);
            this.baseLabel_MainUnitPos_HighResCameraStageCenter.TabIndex = 284;
            this.baseLabel_MainUnitPos_HighResCameraStageCenter.Text = "[ HighRes. Cam. ]\r\n\r\nWork Stage\r\nCenter";
            this.baseLabel_MainUnitPos_HighResCameraStageCenter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel40
            // 
            this.baseLabel40.AutoSize = true;
            this.baseLabel40.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel40.ForeColor = System.Drawing.Color.Black;
            this.baseLabel40.Location = new System.Drawing.Point(129, 475);
            this.baseLabel40.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel40.Name = "baseLabel40";
            this.baseLabel40.Size = new System.Drawing.Size(42, 16);
            this.baseLabel40.TabIndex = 282;
            this.baseLabel40.Text = "Pos. Z";
            // 
            // baseLabel41
            // 
            this.baseLabel41.AutoSize = true;
            this.baseLabel41.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel41.ForeColor = System.Drawing.Color.Black;
            this.baseLabel41.Location = new System.Drawing.Point(129, 450);
            this.baseLabel41.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel41.Name = "baseLabel41";
            this.baseLabel41.Size = new System.Drawing.Size(42, 16);
            this.baseLabel41.TabIndex = 279;
            this.baseLabel41.Text = "Pos. Y";
            // 
            // baseLabel42
            // 
            this.baseLabel42.AutoSize = true;
            this.baseLabel42.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel42.ForeColor = System.Drawing.Color.Black;
            this.baseLabel42.Location = new System.Drawing.Point(129, 425);
            this.baseLabel42.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel42.Name = "baseLabel42";
            this.baseLabel42.Size = new System.Drawing.Size(43, 16);
            this.baseLabel42.TabIndex = 277;
            this.baseLabel42.Text = "Pos. X";
            // 
            // baseLabel_MainUnitPos_ScannerPowerMeter
            // 
            this.baseLabel_MainUnitPos_ScannerPowerMeter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_MainUnitPos_ScannerPowerMeter.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_MainUnitPos_ScannerPowerMeter.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_MainUnitPos_ScannerPowerMeter.Location = new System.Drawing.Point(10, 420);
            this.baseLabel_MainUnitPos_ScannerPowerMeter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_MainUnitPos_ScannerPowerMeter.Name = "baseLabel_MainUnitPos_ScannerPowerMeter";
            this.baseLabel_MainUnitPos_ScannerPowerMeter.Size = new System.Drawing.Size(117, 76);
            this.baseLabel_MainUnitPos_ScannerPowerMeter.TabIndex = 276;
            this.baseLabel_MainUnitPos_ScannerPowerMeter.Text = "[  Scanner  ]\r\n\r\nPower Meter";
            this.baseLabel_MainUnitPos_ScannerPowerMeter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel37
            // 
            this.baseLabel37.AutoSize = true;
            this.baseLabel37.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel37.ForeColor = System.Drawing.Color.Black;
            this.baseLabel37.Location = new System.Drawing.Point(129, 393);
            this.baseLabel37.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel37.Name = "baseLabel37";
            this.baseLabel37.Size = new System.Drawing.Size(42, 16);
            this.baseLabel37.TabIndex = 274;
            this.baseLabel37.Text = "Pos. Z";
            // 
            // baseLabel38
            // 
            this.baseLabel38.AutoSize = true;
            this.baseLabel38.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel38.ForeColor = System.Drawing.Color.Black;
            this.baseLabel38.Location = new System.Drawing.Point(129, 368);
            this.baseLabel38.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel38.Name = "baseLabel38";
            this.baseLabel38.Size = new System.Drawing.Size(42, 16);
            this.baseLabel38.TabIndex = 271;
            this.baseLabel38.Text = "Pos. Y";
            // 
            // baseLabel39
            // 
            this.baseLabel39.AutoSize = true;
            this.baseLabel39.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel39.ForeColor = System.Drawing.Color.Black;
            this.baseLabel39.Location = new System.Drawing.Point(129, 343);
            this.baseLabel39.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel39.Name = "baseLabel39";
            this.baseLabel39.Size = new System.Drawing.Size(43, 16);
            this.baseLabel39.TabIndex = 269;
            this.baseLabel39.Text = "Pos. X";
            // 
            // baseLabel_MainUnitPos_ScannerStageCenter
            // 
            this.baseLabel_MainUnitPos_ScannerStageCenter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_MainUnitPos_ScannerStageCenter.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_MainUnitPos_ScannerStageCenter.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_MainUnitPos_ScannerStageCenter.Location = new System.Drawing.Point(10, 338);
            this.baseLabel_MainUnitPos_ScannerStageCenter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_MainUnitPos_ScannerStageCenter.Name = "baseLabel_MainUnitPos_ScannerStageCenter";
            this.baseLabel_MainUnitPos_ScannerStageCenter.Size = new System.Drawing.Size(117, 76);
            this.baseLabel_MainUnitPos_ScannerStageCenter.TabIndex = 268;
            this.baseLabel_MainUnitPos_ScannerStageCenter.Text = "[  Scanner  ]\r\n\r\nWork Stage\r\nCenter";
            this.baseLabel_MainUnitPos_ScannerStageCenter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel34
            // 
            this.baseLabel34.AutoSize = true;
            this.baseLabel34.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel34.ForeColor = System.Drawing.Color.Black;
            this.baseLabel34.Location = new System.Drawing.Point(129, 311);
            this.baseLabel34.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel34.Name = "baseLabel34";
            this.baseLabel34.Size = new System.Drawing.Size(42, 16);
            this.baseLabel34.TabIndex = 266;
            this.baseLabel34.Text = "Pos. Z";
            // 
            // baseLabel35
            // 
            this.baseLabel35.AutoSize = true;
            this.baseLabel35.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel35.ForeColor = System.Drawing.Color.Black;
            this.baseLabel35.Location = new System.Drawing.Point(129, 286);
            this.baseLabel35.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel35.Name = "baseLabel35";
            this.baseLabel35.Size = new System.Drawing.Size(42, 16);
            this.baseLabel35.TabIndex = 263;
            this.baseLabel35.Text = "Pos. Y";
            // 
            // baseLabel36
            // 
            this.baseLabel36.AutoSize = true;
            this.baseLabel36.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel36.ForeColor = System.Drawing.Color.Black;
            this.baseLabel36.Location = new System.Drawing.Point(129, 261);
            this.baseLabel36.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel36.Name = "baseLabel36";
            this.baseLabel36.Size = new System.Drawing.Size(43, 16);
            this.baseLabel36.TabIndex = 261;
            this.baseLabel36.Text = "Pos. X";
            // 
            // baseLabel_MainUnitPos_ScannerLensCleaning
            // 
            this.baseLabel_MainUnitPos_ScannerLensCleaning.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_MainUnitPos_ScannerLensCleaning.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_MainUnitPos_ScannerLensCleaning.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_MainUnitPos_ScannerLensCleaning.Location = new System.Drawing.Point(10, 256);
            this.baseLabel_MainUnitPos_ScannerLensCleaning.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_MainUnitPos_ScannerLensCleaning.Name = "baseLabel_MainUnitPos_ScannerLensCleaning";
            this.baseLabel_MainUnitPos_ScannerLensCleaning.Size = new System.Drawing.Size(117, 76);
            this.baseLabel_MainUnitPos_ScannerLensCleaning.TabIndex = 260;
            this.baseLabel_MainUnitPos_ScannerLensCleaning.Text = "[  Scanner  ]\r\n\r\nLens Cleaning";
            this.baseLabel_MainUnitPos_ScannerLensCleaning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel31
            // 
            this.baseLabel31.AutoSize = true;
            this.baseLabel31.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel31.ForeColor = System.Drawing.Color.Black;
            this.baseLabel31.Location = new System.Drawing.Point(129, 229);
            this.baseLabel31.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel31.Name = "baseLabel31";
            this.baseLabel31.Size = new System.Drawing.Size(42, 16);
            this.baseLabel31.TabIndex = 258;
            this.baseLabel31.Text = "Pos. Z";
            // 
            // baseLabel32
            // 
            this.baseLabel32.AutoSize = true;
            this.baseLabel32.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel32.ForeColor = System.Drawing.Color.Black;
            this.baseLabel32.Location = new System.Drawing.Point(129, 204);
            this.baseLabel32.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel32.Name = "baseLabel32";
            this.baseLabel32.Size = new System.Drawing.Size(42, 16);
            this.baseLabel32.TabIndex = 255;
            this.baseLabel32.Text = "Pos. Y";
            // 
            // baseLabel33
            // 
            this.baseLabel33.AutoSize = true;
            this.baseLabel33.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel33.ForeColor = System.Drawing.Color.Black;
            this.baseLabel33.Location = new System.Drawing.Point(129, 179);
            this.baseLabel33.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel33.Name = "baseLabel33";
            this.baseLabel33.Size = new System.Drawing.Size(43, 16);
            this.baseLabel33.TabIndex = 253;
            this.baseLabel33.Text = "Pos. X";
            // 
            // baseLabel_MainUnitPos_ModuleUnload
            // 
            this.baseLabel_MainUnitPos_ModuleUnload.BackColor = System.Drawing.Color.Silver;
            this.baseLabel_MainUnitPos_ModuleUnload.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_MainUnitPos_ModuleUnload.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_MainUnitPos_ModuleUnload.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_MainUnitPos_ModuleUnload.Location = new System.Drawing.Point(10, 174);
            this.baseLabel_MainUnitPos_ModuleUnload.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_MainUnitPos_ModuleUnload.Name = "baseLabel_MainUnitPos_ModuleUnload";
            this.baseLabel_MainUnitPos_ModuleUnload.Size = new System.Drawing.Size(117, 76);
            this.baseLabel_MainUnitPos_ModuleUnload.TabIndex = 252;
            this.baseLabel_MainUnitPos_ModuleUnload.Text = "[ Work Stage ]\r\nModule -      \r\n      Unloading\r\nto  Unloader";
            this.baseLabel_MainUnitPos_ModuleUnload.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel28
            // 
            this.baseLabel28.AutoSize = true;
            this.baseLabel28.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel28.ForeColor = System.Drawing.Color.Black;
            this.baseLabel28.Location = new System.Drawing.Point(129, 147);
            this.baseLabel28.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel28.Name = "baseLabel28";
            this.baseLabel28.Size = new System.Drawing.Size(42, 16);
            this.baseLabel28.TabIndex = 250;
            this.baseLabel28.Text = "Pos. Z";
            // 
            // baseLabel29
            // 
            this.baseLabel29.AutoSize = true;
            this.baseLabel29.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel29.ForeColor = System.Drawing.Color.Black;
            this.baseLabel29.Location = new System.Drawing.Point(129, 122);
            this.baseLabel29.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel29.Name = "baseLabel29";
            this.baseLabel29.Size = new System.Drawing.Size(42, 16);
            this.baseLabel29.TabIndex = 247;
            this.baseLabel29.Text = "Pos. Y";
            // 
            // baseLabel30
            // 
            this.baseLabel30.AutoSize = true;
            this.baseLabel30.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel30.ForeColor = System.Drawing.Color.Black;
            this.baseLabel30.Location = new System.Drawing.Point(129, 97);
            this.baseLabel30.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel30.Name = "baseLabel30";
            this.baseLabel30.Size = new System.Drawing.Size(43, 16);
            this.baseLabel30.TabIndex = 245;
            this.baseLabel30.Text = "Pos. X";
            // 
            // baseLabel_MainUnitPos_ModuleLoad
            // 
            this.baseLabel_MainUnitPos_ModuleLoad.BackColor = System.Drawing.Color.Silver;
            this.baseLabel_MainUnitPos_ModuleLoad.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_MainUnitPos_ModuleLoad.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_MainUnitPos_ModuleLoad.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_MainUnitPos_ModuleLoad.Location = new System.Drawing.Point(10, 92);
            this.baseLabel_MainUnitPos_ModuleLoad.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_MainUnitPos_ModuleLoad.Name = "baseLabel_MainUnitPos_ModuleLoad";
            this.baseLabel_MainUnitPos_ModuleLoad.Size = new System.Drawing.Size(117, 76);
            this.baseLabel_MainUnitPos_ModuleLoad.TabIndex = 244;
            this.baseLabel_MainUnitPos_ModuleLoad.Text = "[ Work Stage ]\r\n\r\nModule Loading\r\n from  Loader";
            this.baseLabel_MainUnitPos_ModuleLoad.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel27
            // 
            this.baseLabel27.AutoSize = true;
            this.baseLabel27.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel27.ForeColor = System.Drawing.Color.Black;
            this.baseLabel27.Location = new System.Drawing.Point(129, 65);
            this.baseLabel27.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel27.Name = "baseLabel27";
            this.baseLabel27.Size = new System.Drawing.Size(42, 16);
            this.baseLabel27.TabIndex = 242;
            this.baseLabel27.Text = "Pos. Z";
            // 
            // baseLabel23
            // 
            this.baseLabel23.AutoSize = true;
            this.baseLabel23.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel23.ForeColor = System.Drawing.Color.Black;
            this.baseLabel23.Location = new System.Drawing.Point(129, 40);
            this.baseLabel23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel23.Name = "baseLabel23";
            this.baseLabel23.Size = new System.Drawing.Size(42, 16);
            this.baseLabel23.TabIndex = 239;
            this.baseLabel23.Text = "Pos. Y";
            // 
            // baseLabel26
            // 
            this.baseLabel26.AutoSize = true;
            this.baseLabel26.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel26.ForeColor = System.Drawing.Color.Black;
            this.baseLabel26.Location = new System.Drawing.Point(129, 15);
            this.baseLabel26.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel26.Name = "baseLabel26";
            this.baseLabel26.Size = new System.Drawing.Size(43, 16);
            this.baseLabel26.TabIndex = 237;
            this.baseLabel26.Text = "Pos. X";
            // 
            // baseLabel_MainUnitPos_JigChange
            // 
            this.baseLabel_MainUnitPos_JigChange.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_MainUnitPos_JigChange.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_MainUnitPos_JigChange.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_MainUnitPos_JigChange.Location = new System.Drawing.Point(10, 10);
            this.baseLabel_MainUnitPos_JigChange.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_MainUnitPos_JigChange.Name = "baseLabel_MainUnitPos_JigChange";
            this.baseLabel_MainUnitPos_JigChange.Size = new System.Drawing.Size(117, 76);
            this.baseLabel_MainUnitPos_JigChange.TabIndex = 236;
            this.baseLabel_MainUnitPos_JigChange.Text = "[ Work Stage ]\r\n\r\nJig Change";
            this.baseLabel_MainUnitPos_JigChange.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabPage_Pos_Loader
            // 
            this.tabPage_Pos_Loader.BackColor = System.Drawing.Color.Gainsboro;
            this.tabPage_Pos_Loader.Controls.Add(this.button_LDAlignerPos_100mmClose_Get);
            this.tabPage_Pos_Loader.Controls.Add(this.textBox_LDAlignerPos_100mmClose_Y);
            this.tabPage_Pos_Loader.Controls.Add(this.textBox_LDAlignerPos_100mmClose_X);
            this.tabPage_Pos_Loader.Controls.Add(this.button_LDAlignerPos_FullOpen_Get);
            this.tabPage_Pos_Loader.Controls.Add(this.textBox_LDAlignerPos_FullOpen_Y);
            this.tabPage_Pos_Loader.Controls.Add(this.textBox_LDAlignerPos_FullOpen_X);
            this.tabPage_Pos_Loader.Controls.Add(this.button_LDPickerPos_Module_PutDown_AlignTable_Get);
            this.tabPage_Pos_Loader.Controls.Add(this.textBox_LDPickerPos_Module_PutDown_Aligner_Y);
            this.tabPage_Pos_Loader.Controls.Add(this.textBox_LDPickerPos_Module_PutDown_Aligner_X);
            this.tabPage_Pos_Loader.Controls.Add(this.button_LDPickerPos_Module_PickUp_AlignTable_Get);
            this.tabPage_Pos_Loader.Controls.Add(this.textBox_LDPickerPos_Module_PickUp_Aligner_Y);
            this.tabPage_Pos_Loader.Controls.Add(this.textBox_LDPickerPos_Module_PickUp_Aligner_X);
            this.tabPage_Pos_Loader.Controls.Add(this.button_LDStacker1Pos_Empty_Get);
            this.tabPage_Pos_Loader.Controls.Add(this.textBox_LDStacker1Pos_Empty);
            this.tabPage_Pos_Loader.Controls.Add(this.button_LDStacker1Pos_Full_Get);
            this.tabPage_Pos_Loader.Controls.Add(this.textBox_LDStacker1Pos_Full);
            this.tabPage_Pos_Loader.Controls.Add(this.button_LDStacker0Pos_Empty_Get);
            this.tabPage_Pos_Loader.Controls.Add(this.textBox_LDStacker0Pos_Empty);
            this.tabPage_Pos_Loader.Controls.Add(this.button_LDStacker0Pos_Full_Get);
            this.tabPage_Pos_Loader.Controls.Add(this.textBox_LDStacker0Pos_Full);
            this.tabPage_Pos_Loader.Controls.Add(this.button_LDPickerPos_Module_PickUp_Stacker1_Get);
            this.tabPage_Pos_Loader.Controls.Add(this.textBox_LDPickerPos_Module_PickUp_Stacker1_Z);
            this.tabPage_Pos_Loader.Controls.Add(this.textBox_LDPickerPos_Module_PickUp_Stacker1_X);
            this.tabPage_Pos_Loader.Controls.Add(this.button_LDPickerPos_Module_PickUp_Stacker0_Get);
            this.tabPage_Pos_Loader.Controls.Add(this.textBox_LDPickerPos_Module_PickUp_Stacker0_Z);
            this.tabPage_Pos_Loader.Controls.Add(this.textBox_LDPickerPos_Module_PickUp_Stacker0_X);
            this.tabPage_Pos_Loader.Controls.Add(this.button_LDPickerPos_Module_PutDown_WorkTable_Get);
            this.tabPage_Pos_Loader.Controls.Add(this.textBox_LDPickerPos_Module_PutDown_WorkTable_Z);
            this.tabPage_Pos_Loader.Controls.Add(this.textBox_LDPickerPos_Module_PutDown_WorkTable_X);
            this.tabPage_Pos_Loader.Controls.Add(this.button_LDPickerPos_Moving_Get);
            this.tabPage_Pos_Loader.Controls.Add(this.textBox_LDPickerPos_Moving_Z);
            this.tabPage_Pos_Loader.Controls.Add(this.baseLabel15);
            this.tabPage_Pos_Loader.Controls.Add(this.baseLabel17);
            this.tabPage_Pos_Loader.Controls.Add(this.baseLabel_LDAlignerPos_100mmClose);
            this.tabPage_Pos_Loader.Controls.Add(this.baseLabel24);
            this.tabPage_Pos_Loader.Controls.Add(this.baseLabel25);
            this.tabPage_Pos_Loader.Controls.Add(this.baseLabel_LDAlignerPos_FullOpen);
            this.tabPage_Pos_Loader.Controls.Add(this.baseLabel11);
            this.tabPage_Pos_Loader.Controls.Add(this.baseLabel13);
            this.tabPage_Pos_Loader.Controls.Add(this.baseLabel_LDPickerPos_Aligner1);
            this.tabPage_Pos_Loader.Controls.Add(this.baseLabel6);
            this.tabPage_Pos_Loader.Controls.Add(this.baseLabel9);
            this.tabPage_Pos_Loader.Controls.Add(this.baseLabel_LDPickerPos_Aligner);
            this.tabPage_Pos_Loader.Controls.Add(this.baseLabel10);
            this.tabPage_Pos_Loader.Controls.Add(this.baseLabel_LDStacker1Pos_Empty);
            this.tabPage_Pos_Loader.Controls.Add(this.baseLabel12);
            this.tabPage_Pos_Loader.Controls.Add(this.baseLabel_LDStacker1Pos_Full);
            this.tabPage_Pos_Loader.Controls.Add(this.baseLabel14);
            this.tabPage_Pos_Loader.Controls.Add(this.baseLabel_LDStacker0Pos_Empty);
            this.tabPage_Pos_Loader.Controls.Add(this.baseLabel16);
            this.tabPage_Pos_Loader.Controls.Add(this.baseLabel_LDStacker0Pos_Full);
            this.tabPage_Pos_Loader.Controls.Add(this.baseLabel4);
            this.tabPage_Pos_Loader.Controls.Add(this.baseLabel5);
            this.tabPage_Pos_Loader.Controls.Add(this.baseLabel_LDPickerPos_MGZ2);
            this.tabPage_Pos_Loader.Controls.Add(this.baseLabel7);
            this.tabPage_Pos_Loader.Controls.Add(this.baseLabel8);
            this.tabPage_Pos_Loader.Controls.Add(this.baseLabel_LDPickerPos_MGZ1);
            this.tabPage_Pos_Loader.Controls.Add(this.baseLabel2);
            this.tabPage_Pos_Loader.Controls.Add(this.baseLabel3);
            this.tabPage_Pos_Loader.Controls.Add(this.baseLabel_LDPickerPos_WorkTable);
            this.tabPage_Pos_Loader.Controls.Add(this.baseLabel_LDPickerPos_Moving_Z);
            this.tabPage_Pos_Loader.Controls.Add(this.baseLabel_LDPickerPos_Moving);
            this.tabPage_Pos_Loader.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.tabPage_Pos_Loader.Location = new System.Drawing.Point(4, 30);
            this.tabPage_Pos_Loader.Name = "tabPage_Pos_Loader";
            this.tabPage_Pos_Loader.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Pos_Loader.Size = new System.Drawing.Size(681, 669);
            this.tabPage_Pos_Loader.TabIndex = 1;
            this.tabPage_Pos_Loader.Text = "Loader";
            // 
            // button_LDAlignerPos_100mmClose_Get
            // 
            this.button_LDAlignerPos_100mmClose_Get.BackColor = System.Drawing.Color.White;
            this.button_LDAlignerPos_100mmClose_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_LDAlignerPos_100mmClose_Get.FlatAppearance.BorderSize = 2;
            this.button_LDAlignerPos_100mmClose_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_LDAlignerPos_100mmClose_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_LDAlignerPos_100mmClose_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_LDAlignerPos_100mmClose_Get.ForeColor = System.Drawing.Color.Black;
            this.button_LDAlignerPos_100mmClose_Get.Location = new System.Drawing.Point(602, 346);
            this.button_LDAlignerPos_100mmClose_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_LDAlignerPos_100mmClose_Get.Name = "button_LDAlignerPos_100mmClose_Get";
            this.button_LDAlignerPos_100mmClose_Get.Size = new System.Drawing.Size(70, 55);
            this.button_LDAlignerPos_100mmClose_Get.TabIndex = 307;
            this.button_LDAlignerPos_100mmClose_Get.Text = "Get Pos.";
            this.button_LDAlignerPos_100mmClose_Get.UseVisualStyleBackColor = false;
            // 
            // textBox_LDAlignerPos_100mmClose_Y
            // 
            this.textBox_LDAlignerPos_100mmClose_Y.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_LDAlignerPos_100mmClose_Y.Location = new System.Drawing.Point(524, 376);
            this.textBox_LDAlignerPos_100mmClose_Y.Name = "textBox_LDAlignerPos_100mmClose_Y";
            this.textBox_LDAlignerPos_100mmClose_Y.Size = new System.Drawing.Size(77, 23);
            this.textBox_LDAlignerPos_100mmClose_Y.TabIndex = 306;
            // 
            // textBox_LDAlignerPos_100mmClose_X
            // 
            this.textBox_LDAlignerPos_100mmClose_X.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_LDAlignerPos_100mmClose_X.Location = new System.Drawing.Point(524, 348);
            this.textBox_LDAlignerPos_100mmClose_X.Name = "textBox_LDAlignerPos_100mmClose_X";
            this.textBox_LDAlignerPos_100mmClose_X.Size = new System.Drawing.Size(77, 23);
            this.textBox_LDAlignerPos_100mmClose_X.TabIndex = 304;
            // 
            // button_LDAlignerPos_FullOpen_Get
            // 
            this.button_LDAlignerPos_FullOpen_Get.BackColor = System.Drawing.Color.White;
            this.button_LDAlignerPos_FullOpen_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_LDAlignerPos_FullOpen_Get.FlatAppearance.BorderSize = 2;
            this.button_LDAlignerPos_FullOpen_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_LDAlignerPos_FullOpen_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_LDAlignerPos_FullOpen_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_LDAlignerPos_FullOpen_Get.ForeColor = System.Drawing.Color.Black;
            this.button_LDAlignerPos_FullOpen_Get.Location = new System.Drawing.Point(602, 282);
            this.button_LDAlignerPos_FullOpen_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_LDAlignerPos_FullOpen_Get.Name = "button_LDAlignerPos_FullOpen_Get";
            this.button_LDAlignerPos_FullOpen_Get.Size = new System.Drawing.Size(70, 55);
            this.button_LDAlignerPos_FullOpen_Get.TabIndex = 301;
            this.button_LDAlignerPos_FullOpen_Get.Text = "Get Pos.";
            this.button_LDAlignerPos_FullOpen_Get.UseVisualStyleBackColor = false;
            // 
            // textBox_LDAlignerPos_FullOpen_Y
            // 
            this.textBox_LDAlignerPos_FullOpen_Y.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_LDAlignerPos_FullOpen_Y.Location = new System.Drawing.Point(524, 312);
            this.textBox_LDAlignerPos_FullOpen_Y.Name = "textBox_LDAlignerPos_FullOpen_Y";
            this.textBox_LDAlignerPos_FullOpen_Y.Size = new System.Drawing.Size(77, 23);
            this.textBox_LDAlignerPos_FullOpen_Y.TabIndex = 300;
            // 
            // textBox_LDAlignerPos_FullOpen_X
            // 
            this.textBox_LDAlignerPos_FullOpen_X.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_LDAlignerPos_FullOpen_X.Location = new System.Drawing.Point(524, 284);
            this.textBox_LDAlignerPos_FullOpen_X.Name = "textBox_LDAlignerPos_FullOpen_X";
            this.textBox_LDAlignerPos_FullOpen_X.Size = new System.Drawing.Size(77, 23);
            this.textBox_LDAlignerPos_FullOpen_X.TabIndex = 298;
            // 
            // button_LDPickerPos_Module_PutDown_AlignTable_Get
            // 
            this.button_LDPickerPos_Module_PutDown_AlignTable_Get.BackColor = System.Drawing.Color.White;
            this.button_LDPickerPos_Module_PutDown_AlignTable_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_LDPickerPos_Module_PutDown_AlignTable_Get.FlatAppearance.BorderSize = 2;
            this.button_LDPickerPos_Module_PutDown_AlignTable_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_LDPickerPos_Module_PutDown_AlignTable_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_LDPickerPos_Module_PutDown_AlignTable_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_LDPickerPos_Module_PutDown_AlignTable_Get.ForeColor = System.Drawing.Color.Black;
            this.button_LDPickerPos_Module_PutDown_AlignTable_Get.Location = new System.Drawing.Point(251, 346);
            this.button_LDPickerPos_Module_PutDown_AlignTable_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_LDPickerPos_Module_PutDown_AlignTable_Get.Name = "button_LDPickerPos_Module_PutDown_AlignTable_Get";
            this.button_LDPickerPos_Module_PutDown_AlignTable_Get.Size = new System.Drawing.Size(70, 55);
            this.button_LDPickerPos_Module_PutDown_AlignTable_Get.TabIndex = 295;
            this.button_LDPickerPos_Module_PutDown_AlignTable_Get.Text = "Get Pos.";
            this.button_LDPickerPos_Module_PutDown_AlignTable_Get.UseVisualStyleBackColor = false;
            // 
            // textBox_LDPickerPos_Module_PutDown_Aligner_Y
            // 
            this.textBox_LDPickerPos_Module_PutDown_Aligner_Y.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_LDPickerPos_Module_PutDown_Aligner_Y.Location = new System.Drawing.Point(173, 376);
            this.textBox_LDPickerPos_Module_PutDown_Aligner_Y.Name = "textBox_LDPickerPos_Module_PutDown_Aligner_Y";
            this.textBox_LDPickerPos_Module_PutDown_Aligner_Y.Size = new System.Drawing.Size(77, 23);
            this.textBox_LDPickerPos_Module_PutDown_Aligner_Y.TabIndex = 294;
            // 
            // textBox_LDPickerPos_Module_PutDown_Aligner_X
            // 
            this.textBox_LDPickerPos_Module_PutDown_Aligner_X.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_LDPickerPos_Module_PutDown_Aligner_X.Location = new System.Drawing.Point(173, 348);
            this.textBox_LDPickerPos_Module_PutDown_Aligner_X.Name = "textBox_LDPickerPos_Module_PutDown_Aligner_X";
            this.textBox_LDPickerPos_Module_PutDown_Aligner_X.Size = new System.Drawing.Size(77, 23);
            this.textBox_LDPickerPos_Module_PutDown_Aligner_X.TabIndex = 292;
            // 
            // button_LDPickerPos_Module_PickUp_AlignTable_Get
            // 
            this.button_LDPickerPos_Module_PickUp_AlignTable_Get.BackColor = System.Drawing.Color.White;
            this.button_LDPickerPos_Module_PickUp_AlignTable_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_LDPickerPos_Module_PickUp_AlignTable_Get.FlatAppearance.BorderSize = 2;
            this.button_LDPickerPos_Module_PickUp_AlignTable_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_LDPickerPos_Module_PickUp_AlignTable_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_LDPickerPos_Module_PickUp_AlignTable_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_LDPickerPos_Module_PickUp_AlignTable_Get.ForeColor = System.Drawing.Color.Black;
            this.button_LDPickerPos_Module_PickUp_AlignTable_Get.Location = new System.Drawing.Point(251, 282);
            this.button_LDPickerPos_Module_PickUp_AlignTable_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_LDPickerPos_Module_PickUp_AlignTable_Get.Name = "button_LDPickerPos_Module_PickUp_AlignTable_Get";
            this.button_LDPickerPos_Module_PickUp_AlignTable_Get.Size = new System.Drawing.Size(70, 55);
            this.button_LDPickerPos_Module_PickUp_AlignTable_Get.TabIndex = 289;
            this.button_LDPickerPos_Module_PickUp_AlignTable_Get.Text = "Get Pos.";
            this.button_LDPickerPos_Module_PickUp_AlignTable_Get.UseVisualStyleBackColor = false;
            // 
            // textBox_LDPickerPos_Module_PickUp_Aligner_Y
            // 
            this.textBox_LDPickerPos_Module_PickUp_Aligner_Y.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_LDPickerPos_Module_PickUp_Aligner_Y.Location = new System.Drawing.Point(173, 312);
            this.textBox_LDPickerPos_Module_PickUp_Aligner_Y.Name = "textBox_LDPickerPos_Module_PickUp_Aligner_Y";
            this.textBox_LDPickerPos_Module_PickUp_Aligner_Y.Size = new System.Drawing.Size(77, 23);
            this.textBox_LDPickerPos_Module_PickUp_Aligner_Y.TabIndex = 288;
            // 
            // textBox_LDPickerPos_Module_PickUp_Aligner_X
            // 
            this.textBox_LDPickerPos_Module_PickUp_Aligner_X.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_LDPickerPos_Module_PickUp_Aligner_X.Location = new System.Drawing.Point(173, 284);
            this.textBox_LDPickerPos_Module_PickUp_Aligner_X.Name = "textBox_LDPickerPos_Module_PickUp_Aligner_X";
            this.textBox_LDPickerPos_Module_PickUp_Aligner_X.Size = new System.Drawing.Size(77, 23);
            this.textBox_LDPickerPos_Module_PickUp_Aligner_X.TabIndex = 286;
            // 
            // button_LDStacker1Pos_Empty_Get
            // 
            this.button_LDStacker1Pos_Empty_Get.BackColor = System.Drawing.Color.White;
            this.button_LDStacker1Pos_Empty_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_LDStacker1Pos_Empty_Get.FlatAppearance.BorderSize = 2;
            this.button_LDStacker1Pos_Empty_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_LDStacker1Pos_Empty_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_LDStacker1Pos_Empty_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_LDStacker1Pos_Empty_Get.ForeColor = System.Drawing.Color.Black;
            this.button_LDStacker1Pos_Empty_Get.Location = new System.Drawing.Point(602, 199);
            this.button_LDStacker1Pos_Empty_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_LDStacker1Pos_Empty_Get.Name = "button_LDStacker1Pos_Empty_Get";
            this.button_LDStacker1Pos_Empty_Get.Size = new System.Drawing.Size(70, 55);
            this.button_LDStacker1Pos_Empty_Get.TabIndex = 283;
            this.button_LDStacker1Pos_Empty_Get.Text = "Get Pos.\r\n(Z Only)";
            this.button_LDStacker1Pos_Empty_Get.UseVisualStyleBackColor = false;
            // 
            // textBox_LDStacker1Pos_Empty
            // 
            this.textBox_LDStacker1Pos_Empty.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_LDStacker1Pos_Empty.Location = new System.Drawing.Point(524, 215);
            this.textBox_LDStacker1Pos_Empty.Name = "textBox_LDStacker1Pos_Empty";
            this.textBox_LDStacker1Pos_Empty.Size = new System.Drawing.Size(77, 23);
            this.textBox_LDStacker1Pos_Empty.TabIndex = 282;
            // 
            // button_LDStacker1Pos_Full_Get
            // 
            this.button_LDStacker1Pos_Full_Get.BackColor = System.Drawing.Color.White;
            this.button_LDStacker1Pos_Full_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_LDStacker1Pos_Full_Get.FlatAppearance.BorderSize = 2;
            this.button_LDStacker1Pos_Full_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_LDStacker1Pos_Full_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_LDStacker1Pos_Full_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_LDStacker1Pos_Full_Get.ForeColor = System.Drawing.Color.Black;
            this.button_LDStacker1Pos_Full_Get.Location = new System.Drawing.Point(602, 136);
            this.button_LDStacker1Pos_Full_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_LDStacker1Pos_Full_Get.Name = "button_LDStacker1Pos_Full_Get";
            this.button_LDStacker1Pos_Full_Get.Size = new System.Drawing.Size(70, 55);
            this.button_LDStacker1Pos_Full_Get.TabIndex = 279;
            this.button_LDStacker1Pos_Full_Get.Text = "Get Pos.\r\n(Z Only)";
            this.button_LDStacker1Pos_Full_Get.UseVisualStyleBackColor = false;
            // 
            // textBox_LDStacker1Pos_Full
            // 
            this.textBox_LDStacker1Pos_Full.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_LDStacker1Pos_Full.Location = new System.Drawing.Point(524, 152);
            this.textBox_LDStacker1Pos_Full.Name = "textBox_LDStacker1Pos_Full";
            this.textBox_LDStacker1Pos_Full.Size = new System.Drawing.Size(77, 23);
            this.textBox_LDStacker1Pos_Full.TabIndex = 278;
            // 
            // button_LDStacker0Pos_Empty_Get
            // 
            this.button_LDStacker0Pos_Empty_Get.BackColor = System.Drawing.Color.White;
            this.button_LDStacker0Pos_Empty_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_LDStacker0Pos_Empty_Get.FlatAppearance.BorderSize = 2;
            this.button_LDStacker0Pos_Empty_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_LDStacker0Pos_Empty_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_LDStacker0Pos_Empty_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_LDStacker0Pos_Empty_Get.ForeColor = System.Drawing.Color.Black;
            this.button_LDStacker0Pos_Empty_Get.Location = new System.Drawing.Point(602, 73);
            this.button_LDStacker0Pos_Empty_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_LDStacker0Pos_Empty_Get.Name = "button_LDStacker0Pos_Empty_Get";
            this.button_LDStacker0Pos_Empty_Get.Size = new System.Drawing.Size(70, 55);
            this.button_LDStacker0Pos_Empty_Get.TabIndex = 275;
            this.button_LDStacker0Pos_Empty_Get.Text = "Get Pos.\r\n(Z Only)";
            this.button_LDStacker0Pos_Empty_Get.UseVisualStyleBackColor = false;
            // 
            // textBox_LDStacker0Pos_Empty
            // 
            this.textBox_LDStacker0Pos_Empty.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_LDStacker0Pos_Empty.Location = new System.Drawing.Point(524, 89);
            this.textBox_LDStacker0Pos_Empty.Name = "textBox_LDStacker0Pos_Empty";
            this.textBox_LDStacker0Pos_Empty.Size = new System.Drawing.Size(77, 23);
            this.textBox_LDStacker0Pos_Empty.TabIndex = 274;
            // 
            // button_LDStacker0Pos_Full_Get
            // 
            this.button_LDStacker0Pos_Full_Get.BackColor = System.Drawing.Color.White;
            this.button_LDStacker0Pos_Full_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_LDStacker0Pos_Full_Get.FlatAppearance.BorderSize = 2;
            this.button_LDStacker0Pos_Full_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_LDStacker0Pos_Full_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_LDStacker0Pos_Full_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_LDStacker0Pos_Full_Get.ForeColor = System.Drawing.Color.Black;
            this.button_LDStacker0Pos_Full_Get.Location = new System.Drawing.Point(602, 10);
            this.button_LDStacker0Pos_Full_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_LDStacker0Pos_Full_Get.Name = "button_LDStacker0Pos_Full_Get";
            this.button_LDStacker0Pos_Full_Get.Size = new System.Drawing.Size(70, 55);
            this.button_LDStacker0Pos_Full_Get.TabIndex = 271;
            this.button_LDStacker0Pos_Full_Get.Text = "Get Pos.\r\n(Z Only)";
            this.button_LDStacker0Pos_Full_Get.UseVisualStyleBackColor = false;
            // 
            // textBox_LDStacker0Pos_Full
            // 
            this.textBox_LDStacker0Pos_Full.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_LDStacker0Pos_Full.Location = new System.Drawing.Point(524, 26);
            this.textBox_LDStacker0Pos_Full.Name = "textBox_LDStacker0Pos_Full";
            this.textBox_LDStacker0Pos_Full.Size = new System.Drawing.Size(77, 23);
            this.textBox_LDStacker0Pos_Full.TabIndex = 270;
            // 
            // button_LDPickerPos_Module_PickUp_Stacker1_Get
            // 
            this.button_LDPickerPos_Module_PickUp_Stacker1_Get.BackColor = System.Drawing.Color.White;
            this.button_LDPickerPos_Module_PickUp_Stacker1_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_LDPickerPos_Module_PickUp_Stacker1_Get.FlatAppearance.BorderSize = 2;
            this.button_LDPickerPos_Module_PickUp_Stacker1_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_LDPickerPos_Module_PickUp_Stacker1_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_LDPickerPos_Module_PickUp_Stacker1_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_LDPickerPos_Module_PickUp_Stacker1_Get.ForeColor = System.Drawing.Color.Black;
            this.button_LDPickerPos_Module_PickUp_Stacker1_Get.Location = new System.Drawing.Point(251, 199);
            this.button_LDPickerPos_Module_PickUp_Stacker1_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_LDPickerPos_Module_PickUp_Stacker1_Get.Name = "button_LDPickerPos_Module_PickUp_Stacker1_Get";
            this.button_LDPickerPos_Module_PickUp_Stacker1_Get.Size = new System.Drawing.Size(70, 55);
            this.button_LDPickerPos_Module_PickUp_Stacker1_Get.TabIndex = 267;
            this.button_LDPickerPos_Module_PickUp_Stacker1_Get.Text = "Get Pos.";
            this.button_LDPickerPos_Module_PickUp_Stacker1_Get.UseVisualStyleBackColor = false;
            // 
            // textBox_LDPickerPos_Module_PickUp_Stacker1_Z
            // 
            this.textBox_LDPickerPos_Module_PickUp_Stacker1_Z.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_LDPickerPos_Module_PickUp_Stacker1_Z.Location = new System.Drawing.Point(173, 229);
            this.textBox_LDPickerPos_Module_PickUp_Stacker1_Z.Name = "textBox_LDPickerPos_Module_PickUp_Stacker1_Z";
            this.textBox_LDPickerPos_Module_PickUp_Stacker1_Z.Size = new System.Drawing.Size(77, 23);
            this.textBox_LDPickerPos_Module_PickUp_Stacker1_Z.TabIndex = 266;
            // 
            // textBox_LDPickerPos_Module_PickUp_Stacker1_X
            // 
            this.textBox_LDPickerPos_Module_PickUp_Stacker1_X.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_LDPickerPos_Module_PickUp_Stacker1_X.Location = new System.Drawing.Point(173, 201);
            this.textBox_LDPickerPos_Module_PickUp_Stacker1_X.Name = "textBox_LDPickerPos_Module_PickUp_Stacker1_X";
            this.textBox_LDPickerPos_Module_PickUp_Stacker1_X.Size = new System.Drawing.Size(77, 23);
            this.textBox_LDPickerPos_Module_PickUp_Stacker1_X.TabIndex = 264;
            // 
            // button_LDPickerPos_Module_PickUp_Stacker0_Get
            // 
            this.button_LDPickerPos_Module_PickUp_Stacker0_Get.BackColor = System.Drawing.Color.White;
            this.button_LDPickerPos_Module_PickUp_Stacker0_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_LDPickerPos_Module_PickUp_Stacker0_Get.FlatAppearance.BorderSize = 2;
            this.button_LDPickerPos_Module_PickUp_Stacker0_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_LDPickerPos_Module_PickUp_Stacker0_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_LDPickerPos_Module_PickUp_Stacker0_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_LDPickerPos_Module_PickUp_Stacker0_Get.ForeColor = System.Drawing.Color.Black;
            this.button_LDPickerPos_Module_PickUp_Stacker0_Get.Location = new System.Drawing.Point(251, 136);
            this.button_LDPickerPos_Module_PickUp_Stacker0_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_LDPickerPos_Module_PickUp_Stacker0_Get.Name = "button_LDPickerPos_Module_PickUp_Stacker0_Get";
            this.button_LDPickerPos_Module_PickUp_Stacker0_Get.Size = new System.Drawing.Size(70, 55);
            this.button_LDPickerPos_Module_PickUp_Stacker0_Get.TabIndex = 261;
            this.button_LDPickerPos_Module_PickUp_Stacker0_Get.Text = "Get Pos.";
            this.button_LDPickerPos_Module_PickUp_Stacker0_Get.UseVisualStyleBackColor = false;
            // 
            // textBox_LDPickerPos_Module_PickUp_Stacker0_Z
            // 
            this.textBox_LDPickerPos_Module_PickUp_Stacker0_Z.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_LDPickerPos_Module_PickUp_Stacker0_Z.Location = new System.Drawing.Point(173, 166);
            this.textBox_LDPickerPos_Module_PickUp_Stacker0_Z.Name = "textBox_LDPickerPos_Module_PickUp_Stacker0_Z";
            this.textBox_LDPickerPos_Module_PickUp_Stacker0_Z.Size = new System.Drawing.Size(77, 23);
            this.textBox_LDPickerPos_Module_PickUp_Stacker0_Z.TabIndex = 260;
            // 
            // textBox_LDPickerPos_Module_PickUp_Stacker0_X
            // 
            this.textBox_LDPickerPos_Module_PickUp_Stacker0_X.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_LDPickerPos_Module_PickUp_Stacker0_X.Location = new System.Drawing.Point(173, 138);
            this.textBox_LDPickerPos_Module_PickUp_Stacker0_X.Name = "textBox_LDPickerPos_Module_PickUp_Stacker0_X";
            this.textBox_LDPickerPos_Module_PickUp_Stacker0_X.Size = new System.Drawing.Size(77, 23);
            this.textBox_LDPickerPos_Module_PickUp_Stacker0_X.TabIndex = 258;
            // 
            // button_LDPickerPos_Module_PutDown_WorkTable_Get
            // 
            this.button_LDPickerPos_Module_PutDown_WorkTable_Get.BackColor = System.Drawing.Color.White;
            this.button_LDPickerPos_Module_PutDown_WorkTable_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_LDPickerPos_Module_PutDown_WorkTable_Get.FlatAppearance.BorderSize = 2;
            this.button_LDPickerPos_Module_PutDown_WorkTable_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_LDPickerPos_Module_PutDown_WorkTable_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_LDPickerPos_Module_PutDown_WorkTable_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_LDPickerPos_Module_PutDown_WorkTable_Get.ForeColor = System.Drawing.Color.Black;
            this.button_LDPickerPos_Module_PutDown_WorkTable_Get.Location = new System.Drawing.Point(251, 73);
            this.button_LDPickerPos_Module_PutDown_WorkTable_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_LDPickerPos_Module_PutDown_WorkTable_Get.Name = "button_LDPickerPos_Module_PutDown_WorkTable_Get";
            this.button_LDPickerPos_Module_PutDown_WorkTable_Get.Size = new System.Drawing.Size(70, 55);
            this.button_LDPickerPos_Module_PutDown_WorkTable_Get.TabIndex = 255;
            this.button_LDPickerPos_Module_PutDown_WorkTable_Get.Text = "Get Pos.";
            this.button_LDPickerPos_Module_PutDown_WorkTable_Get.UseVisualStyleBackColor = false;
            // 
            // textBox_LDPickerPos_Module_PutDown_WorkTable_Z
            // 
            this.textBox_LDPickerPos_Module_PutDown_WorkTable_Z.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_LDPickerPos_Module_PutDown_WorkTable_Z.Location = new System.Drawing.Point(173, 103);
            this.textBox_LDPickerPos_Module_PutDown_WorkTable_Z.Name = "textBox_LDPickerPos_Module_PutDown_WorkTable_Z";
            this.textBox_LDPickerPos_Module_PutDown_WorkTable_Z.Size = new System.Drawing.Size(77, 23);
            this.textBox_LDPickerPos_Module_PutDown_WorkTable_Z.TabIndex = 254;
            // 
            // textBox_LDPickerPos_Module_PutDown_WorkTable_X
            // 
            this.textBox_LDPickerPos_Module_PutDown_WorkTable_X.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_LDPickerPos_Module_PutDown_WorkTable_X.Location = new System.Drawing.Point(173, 75);
            this.textBox_LDPickerPos_Module_PutDown_WorkTable_X.Name = "textBox_LDPickerPos_Module_PutDown_WorkTable_X";
            this.textBox_LDPickerPos_Module_PutDown_WorkTable_X.Size = new System.Drawing.Size(77, 23);
            this.textBox_LDPickerPos_Module_PutDown_WorkTable_X.TabIndex = 252;
            // 
            // button_LDPickerPos_Moving_Get
            // 
            this.button_LDPickerPos_Moving_Get.BackColor = System.Drawing.Color.White;
            this.button_LDPickerPos_Moving_Get.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_LDPickerPos_Moving_Get.FlatAppearance.BorderSize = 2;
            this.button_LDPickerPos_Moving_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_LDPickerPos_Moving_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_LDPickerPos_Moving_Get.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_LDPickerPos_Moving_Get.ForeColor = System.Drawing.Color.Black;
            this.button_LDPickerPos_Moving_Get.Location = new System.Drawing.Point(251, 10);
            this.button_LDPickerPos_Moving_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_LDPickerPos_Moving_Get.Name = "button_LDPickerPos_Moving_Get";
            this.button_LDPickerPos_Moving_Get.Size = new System.Drawing.Size(70, 55);
            this.button_LDPickerPos_Moving_Get.TabIndex = 247;
            this.button_LDPickerPos_Moving_Get.Text = "Get Pos.\r\n(Z Only)";
            this.button_LDPickerPos_Moving_Get.UseVisualStyleBackColor = false;
            // 
            // textBox_LDPickerPos_Moving_Z
            // 
            this.textBox_LDPickerPos_Moving_Z.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_LDPickerPos_Moving_Z.Location = new System.Drawing.Point(173, 26);
            this.textBox_LDPickerPos_Moving_Z.Name = "textBox_LDPickerPos_Moving_Z";
            this.textBox_LDPickerPos_Moving_Z.Size = new System.Drawing.Size(77, 23);
            this.textBox_LDPickerPos_Moving_Z.TabIndex = 246;
            // 
            // baseLabel15
            // 
            this.baseLabel15.AutoSize = true;
            this.baseLabel15.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel15.ForeColor = System.Drawing.Color.Black;
            this.baseLabel15.Location = new System.Drawing.Point(480, 379);
            this.baseLabel15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel15.Name = "baseLabel15";
            this.baseLabel15.Size = new System.Drawing.Size(42, 16);
            this.baseLabel15.TabIndex = 305;
            this.baseLabel15.Text = "Pos. Y";
            // 
            // baseLabel17
            // 
            this.baseLabel17.AutoSize = true;
            this.baseLabel17.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel17.ForeColor = System.Drawing.Color.Black;
            this.baseLabel17.Location = new System.Drawing.Point(480, 351);
            this.baseLabel17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel17.Name = "baseLabel17";
            this.baseLabel17.Size = new System.Drawing.Size(43, 16);
            this.baseLabel17.TabIndex = 303;
            this.baseLabel17.Text = "Pos. X";
            // 
            // baseLabel_LDAlignerPos_100mmClose
            // 
            this.baseLabel_LDAlignerPos_100mmClose.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_LDAlignerPos_100mmClose.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_LDAlignerPos_100mmClose.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LDAlignerPos_100mmClose.Location = new System.Drawing.Point(361, 346);
            this.baseLabel_LDAlignerPos_100mmClose.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LDAlignerPos_100mmClose.Name = "baseLabel_LDAlignerPos_100mmClose";
            this.baseLabel_LDAlignerPos_100mmClose.Size = new System.Drawing.Size(117, 55);
            this.baseLabel_LDAlignerPos_100mmClose.TabIndex = 302;
            this.baseLabel_LDAlignerPos_100mmClose.Text = "[Module Aligner]\r\nClose Size\r\n100mm";
            this.baseLabel_LDAlignerPos_100mmClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel24
            // 
            this.baseLabel24.AutoSize = true;
            this.baseLabel24.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel24.ForeColor = System.Drawing.Color.Black;
            this.baseLabel24.Location = new System.Drawing.Point(480, 315);
            this.baseLabel24.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel24.Name = "baseLabel24";
            this.baseLabel24.Size = new System.Drawing.Size(42, 16);
            this.baseLabel24.TabIndex = 299;
            this.baseLabel24.Text = "Pos. Y";
            // 
            // baseLabel25
            // 
            this.baseLabel25.AutoSize = true;
            this.baseLabel25.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel25.ForeColor = System.Drawing.Color.Black;
            this.baseLabel25.Location = new System.Drawing.Point(480, 287);
            this.baseLabel25.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel25.Name = "baseLabel25";
            this.baseLabel25.Size = new System.Drawing.Size(43, 16);
            this.baseLabel25.TabIndex = 297;
            this.baseLabel25.Text = "Pos. X";
            // 
            // baseLabel_LDAlignerPos_FullOpen
            // 
            this.baseLabel_LDAlignerPos_FullOpen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_LDAlignerPos_FullOpen.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_LDAlignerPos_FullOpen.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LDAlignerPos_FullOpen.Location = new System.Drawing.Point(361, 282);
            this.baseLabel_LDAlignerPos_FullOpen.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LDAlignerPos_FullOpen.Name = "baseLabel_LDAlignerPos_FullOpen";
            this.baseLabel_LDAlignerPos_FullOpen.Size = new System.Drawing.Size(117, 55);
            this.baseLabel_LDAlignerPos_FullOpen.TabIndex = 296;
            this.baseLabel_LDAlignerPos_FullOpen.Text = "[Module Aligner]\r\nFull Open";
            this.baseLabel_LDAlignerPos_FullOpen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel11
            // 
            this.baseLabel11.AutoSize = true;
            this.baseLabel11.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel11.ForeColor = System.Drawing.Color.Black;
            this.baseLabel11.Location = new System.Drawing.Point(129, 379);
            this.baseLabel11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel11.Name = "baseLabel11";
            this.baseLabel11.Size = new System.Drawing.Size(42, 16);
            this.baseLabel11.TabIndex = 293;
            this.baseLabel11.Text = "Pos. Y";
            // 
            // baseLabel13
            // 
            this.baseLabel13.AutoSize = true;
            this.baseLabel13.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel13.ForeColor = System.Drawing.Color.Black;
            this.baseLabel13.Location = new System.Drawing.Point(129, 351);
            this.baseLabel13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel13.Name = "baseLabel13";
            this.baseLabel13.Size = new System.Drawing.Size(43, 16);
            this.baseLabel13.TabIndex = 291;
            this.baseLabel13.Text = "Pos. X";
            // 
            // baseLabel_LDPickerPos_Aligner1
            // 
            this.baseLabel_LDPickerPos_Aligner1.BackColor = System.Drawing.Color.Silver;
            this.baseLabel_LDPickerPos_Aligner1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_LDPickerPos_Aligner1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_LDPickerPos_Aligner1.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LDPickerPos_Aligner1.Location = new System.Drawing.Point(10, 346);
            this.baseLabel_LDPickerPos_Aligner1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LDPickerPos_Aligner1.Name = "baseLabel_LDPickerPos_Aligner1";
            this.baseLabel_LDPickerPos_Aligner1.Size = new System.Drawing.Size(117, 55);
            this.baseLabel_LDPickerPos_Aligner1.TabIndex = 290;
            this.baseLabel_LDPickerPos_Aligner1.Text = "[  Transfer  ]\r\nModule PutDown\r\nto  Align Stage";
            this.baseLabel_LDPickerPos_Aligner1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel6
            // 
            this.baseLabel6.AutoSize = true;
            this.baseLabel6.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel6.ForeColor = System.Drawing.Color.Black;
            this.baseLabel6.Location = new System.Drawing.Point(129, 315);
            this.baseLabel6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel6.Name = "baseLabel6";
            this.baseLabel6.Size = new System.Drawing.Size(42, 16);
            this.baseLabel6.TabIndex = 287;
            this.baseLabel6.Text = "Pos. Y";
            // 
            // baseLabel9
            // 
            this.baseLabel9.AutoSize = true;
            this.baseLabel9.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel9.ForeColor = System.Drawing.Color.Black;
            this.baseLabel9.Location = new System.Drawing.Point(129, 287);
            this.baseLabel9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel9.Name = "baseLabel9";
            this.baseLabel9.Size = new System.Drawing.Size(43, 16);
            this.baseLabel9.TabIndex = 285;
            this.baseLabel9.Text = "Pos. X";
            // 
            // baseLabel_LDPickerPos_Aligner
            // 
            this.baseLabel_LDPickerPos_Aligner.BackColor = System.Drawing.Color.Silver;
            this.baseLabel_LDPickerPos_Aligner.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_LDPickerPos_Aligner.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_LDPickerPos_Aligner.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LDPickerPos_Aligner.Location = new System.Drawing.Point(10, 282);
            this.baseLabel_LDPickerPos_Aligner.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LDPickerPos_Aligner.Name = "baseLabel_LDPickerPos_Aligner";
            this.baseLabel_LDPickerPos_Aligner.Size = new System.Drawing.Size(117, 55);
            this.baseLabel_LDPickerPos_Aligner.TabIndex = 284;
            this.baseLabel_LDPickerPos_Aligner.Text = "[  Transfer  ]\r\nModule PickUp\r\nfrom Align Stage";
            this.baseLabel_LDPickerPos_Aligner.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel10
            // 
            this.baseLabel10.AutoSize = true;
            this.baseLabel10.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel10.ForeColor = System.Drawing.Color.Black;
            this.baseLabel10.Location = new System.Drawing.Point(480, 218);
            this.baseLabel10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel10.Name = "baseLabel10";
            this.baseLabel10.Size = new System.Drawing.Size(42, 16);
            this.baseLabel10.TabIndex = 281;
            this.baseLabel10.Text = "Pos. Z";
            // 
            // baseLabel_LDStacker1Pos_Empty
            // 
            this.baseLabel_LDStacker1Pos_Empty.BackColor = System.Drawing.Color.Silver;
            this.baseLabel_LDStacker1Pos_Empty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_LDStacker1Pos_Empty.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_LDStacker1Pos_Empty.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LDStacker1Pos_Empty.Location = new System.Drawing.Point(361, 199);
            this.baseLabel_LDStacker1Pos_Empty.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LDStacker1Pos_Empty.Name = "baseLabel_LDStacker1Pos_Empty";
            this.baseLabel_LDStacker1Pos_Empty.Size = new System.Drawing.Size(117, 55);
            this.baseLabel_LDStacker1Pos_Empty.TabIndex = 280;
            this.baseLabel_LDStacker1Pos_Empty.Text = "[ Stacker1  (L) ]\r\nEmpty";
            this.baseLabel_LDStacker1Pos_Empty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel12
            // 
            this.baseLabel12.AutoSize = true;
            this.baseLabel12.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel12.ForeColor = System.Drawing.Color.Black;
            this.baseLabel12.Location = new System.Drawing.Point(480, 155);
            this.baseLabel12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel12.Name = "baseLabel12";
            this.baseLabel12.Size = new System.Drawing.Size(42, 16);
            this.baseLabel12.TabIndex = 277;
            this.baseLabel12.Text = "Pos. Z";
            // 
            // baseLabel_LDStacker1Pos_Full
            // 
            this.baseLabel_LDStacker1Pos_Full.BackColor = System.Drawing.Color.Silver;
            this.baseLabel_LDStacker1Pos_Full.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_LDStacker1Pos_Full.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_LDStacker1Pos_Full.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LDStacker1Pos_Full.Location = new System.Drawing.Point(361, 136);
            this.baseLabel_LDStacker1Pos_Full.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LDStacker1Pos_Full.Name = "baseLabel_LDStacker1Pos_Full";
            this.baseLabel_LDStacker1Pos_Full.Size = new System.Drawing.Size(117, 55);
            this.baseLabel_LDStacker1Pos_Full.TabIndex = 276;
            this.baseLabel_LDStacker1Pos_Full.Text = "[ Stacker1  (L) ]\r\nFull";
            this.baseLabel_LDStacker1Pos_Full.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel14
            // 
            this.baseLabel14.AutoSize = true;
            this.baseLabel14.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel14.ForeColor = System.Drawing.Color.Black;
            this.baseLabel14.Location = new System.Drawing.Point(480, 92);
            this.baseLabel14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel14.Name = "baseLabel14";
            this.baseLabel14.Size = new System.Drawing.Size(42, 16);
            this.baseLabel14.TabIndex = 273;
            this.baseLabel14.Text = "Pos. Z";
            // 
            // baseLabel_LDStacker0Pos_Empty
            // 
            this.baseLabel_LDStacker0Pos_Empty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_LDStacker0Pos_Empty.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_LDStacker0Pos_Empty.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LDStacker0Pos_Empty.Location = new System.Drawing.Point(361, 73);
            this.baseLabel_LDStacker0Pos_Empty.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LDStacker0Pos_Empty.Name = "baseLabel_LDStacker0Pos_Empty";
            this.baseLabel_LDStacker0Pos_Empty.Size = new System.Drawing.Size(117, 55);
            this.baseLabel_LDStacker0Pos_Empty.TabIndex = 272;
            this.baseLabel_LDStacker0Pos_Empty.Text = "[ Stacker0  (R) ]\r\nEmpty";
            this.baseLabel_LDStacker0Pos_Empty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel16
            // 
            this.baseLabel16.AutoSize = true;
            this.baseLabel16.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel16.ForeColor = System.Drawing.Color.Black;
            this.baseLabel16.Location = new System.Drawing.Point(480, 29);
            this.baseLabel16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel16.Name = "baseLabel16";
            this.baseLabel16.Size = new System.Drawing.Size(42, 16);
            this.baseLabel16.TabIndex = 269;
            this.baseLabel16.Text = "Pos. Z";
            // 
            // baseLabel_LDStacker0Pos_Full
            // 
            this.baseLabel_LDStacker0Pos_Full.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_LDStacker0Pos_Full.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_LDStacker0Pos_Full.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LDStacker0Pos_Full.Location = new System.Drawing.Point(361, 10);
            this.baseLabel_LDStacker0Pos_Full.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LDStacker0Pos_Full.Name = "baseLabel_LDStacker0Pos_Full";
            this.baseLabel_LDStacker0Pos_Full.Size = new System.Drawing.Size(117, 55);
            this.baseLabel_LDStacker0Pos_Full.TabIndex = 268;
            this.baseLabel_LDStacker0Pos_Full.Text = "[ Stacker0  (R) ]\r\nFull";
            this.baseLabel_LDStacker0Pos_Full.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel4
            // 
            this.baseLabel4.AutoSize = true;
            this.baseLabel4.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel4.ForeColor = System.Drawing.Color.Black;
            this.baseLabel4.Location = new System.Drawing.Point(129, 232);
            this.baseLabel4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel4.Name = "baseLabel4";
            this.baseLabel4.Size = new System.Drawing.Size(42, 16);
            this.baseLabel4.TabIndex = 265;
            this.baseLabel4.Text = "Pos. Z";
            // 
            // baseLabel5
            // 
            this.baseLabel5.AutoSize = true;
            this.baseLabel5.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel5.ForeColor = System.Drawing.Color.Black;
            this.baseLabel5.Location = new System.Drawing.Point(129, 204);
            this.baseLabel5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel5.Name = "baseLabel5";
            this.baseLabel5.Size = new System.Drawing.Size(43, 16);
            this.baseLabel5.TabIndex = 263;
            this.baseLabel5.Text = "Pos. X";
            // 
            // baseLabel_LDPickerPos_MGZ2
            // 
            this.baseLabel_LDPickerPos_MGZ2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_LDPickerPos_MGZ2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_LDPickerPos_MGZ2.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LDPickerPos_MGZ2.Location = new System.Drawing.Point(10, 199);
            this.baseLabel_LDPickerPos_MGZ2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LDPickerPos_MGZ2.Name = "baseLabel_LDPickerPos_MGZ2";
            this.baseLabel_LDPickerPos_MGZ2.Size = new System.Drawing.Size(117, 55);
            this.baseLabel_LDPickerPos_MGZ2.TabIndex = 262;
            this.baseLabel_LDPickerPos_MGZ2.Text = "[  Transfer  ]\r\nModule PickUp\r\nfrom  Stacker1";
            this.baseLabel_LDPickerPos_MGZ2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel7
            // 
            this.baseLabel7.AutoSize = true;
            this.baseLabel7.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel7.ForeColor = System.Drawing.Color.Black;
            this.baseLabel7.Location = new System.Drawing.Point(129, 169);
            this.baseLabel7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel7.Name = "baseLabel7";
            this.baseLabel7.Size = new System.Drawing.Size(42, 16);
            this.baseLabel7.TabIndex = 259;
            this.baseLabel7.Text = "Pos. Z";
            // 
            // baseLabel8
            // 
            this.baseLabel8.AutoSize = true;
            this.baseLabel8.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel8.ForeColor = System.Drawing.Color.Black;
            this.baseLabel8.Location = new System.Drawing.Point(129, 141);
            this.baseLabel8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel8.Name = "baseLabel8";
            this.baseLabel8.Size = new System.Drawing.Size(43, 16);
            this.baseLabel8.TabIndex = 257;
            this.baseLabel8.Text = "Pos. X";
            // 
            // baseLabel_LDPickerPos_MGZ1
            // 
            this.baseLabel_LDPickerPos_MGZ1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_LDPickerPos_MGZ1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_LDPickerPos_MGZ1.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LDPickerPos_MGZ1.Location = new System.Drawing.Point(10, 136);
            this.baseLabel_LDPickerPos_MGZ1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LDPickerPos_MGZ1.Name = "baseLabel_LDPickerPos_MGZ1";
            this.baseLabel_LDPickerPos_MGZ1.Size = new System.Drawing.Size(117, 55);
            this.baseLabel_LDPickerPos_MGZ1.TabIndex = 256;
            this.baseLabel_LDPickerPos_MGZ1.Text = "[  Transfer  ]\r\nModule PickUp\r\nfrom  Stacker0";
            this.baseLabel_LDPickerPos_MGZ1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel2
            // 
            this.baseLabel2.AutoSize = true;
            this.baseLabel2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel2.ForeColor = System.Drawing.Color.Black;
            this.baseLabel2.Location = new System.Drawing.Point(129, 106);
            this.baseLabel2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel2.Name = "baseLabel2";
            this.baseLabel2.Size = new System.Drawing.Size(42, 16);
            this.baseLabel2.TabIndex = 253;
            this.baseLabel2.Text = "Pos. Z";
            // 
            // baseLabel3
            // 
            this.baseLabel3.AutoSize = true;
            this.baseLabel3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel3.ForeColor = System.Drawing.Color.Black;
            this.baseLabel3.Location = new System.Drawing.Point(129, 78);
            this.baseLabel3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel3.Name = "baseLabel3";
            this.baseLabel3.Size = new System.Drawing.Size(43, 16);
            this.baseLabel3.TabIndex = 251;
            this.baseLabel3.Text = "Pos. X";
            // 
            // baseLabel_LDPickerPos_WorkTable
            // 
            this.baseLabel_LDPickerPos_WorkTable.BackColor = System.Drawing.Color.Silver;
            this.baseLabel_LDPickerPos_WorkTable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_LDPickerPos_WorkTable.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_LDPickerPos_WorkTable.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LDPickerPos_WorkTable.Location = new System.Drawing.Point(10, 73);
            this.baseLabel_LDPickerPos_WorkTable.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LDPickerPos_WorkTable.Name = "baseLabel_LDPickerPos_WorkTable";
            this.baseLabel_LDPickerPos_WorkTable.Size = new System.Drawing.Size(117, 55);
            this.baseLabel_LDPickerPos_WorkTable.TabIndex = 250;
            this.baseLabel_LDPickerPos_WorkTable.Text = "[  Transfer  ]\r\nModule PutDown\r\nto  Work Stage";
            this.baseLabel_LDPickerPos_WorkTable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel_LDPickerPos_Moving_Z
            // 
            this.baseLabel_LDPickerPos_Moving_Z.AutoSize = true;
            this.baseLabel_LDPickerPos_Moving_Z.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_LDPickerPos_Moving_Z.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LDPickerPos_Moving_Z.Location = new System.Drawing.Point(129, 29);
            this.baseLabel_LDPickerPos_Moving_Z.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LDPickerPos_Moving_Z.Name = "baseLabel_LDPickerPos_Moving_Z";
            this.baseLabel_LDPickerPos_Moving_Z.Size = new System.Drawing.Size(42, 16);
            this.baseLabel_LDPickerPos_Moving_Z.TabIndex = 245;
            this.baseLabel_LDPickerPos_Moving_Z.Text = "Pos. Z";
            // 
            // baseLabel_LDPickerPos_Moving
            // 
            this.baseLabel_LDPickerPos_Moving.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_LDPickerPos_Moving.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_LDPickerPos_Moving.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LDPickerPos_Moving.Location = new System.Drawing.Point(10, 10);
            this.baseLabel_LDPickerPos_Moving.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LDPickerPos_Moving.Name = "baseLabel_LDPickerPos_Moving";
            this.baseLabel_LDPickerPos_Moving.Size = new System.Drawing.Size(117, 55);
            this.baseLabel_LDPickerPos_Moving.TabIndex = 244;
            this.baseLabel_LDPickerPos_Moving.Text = "[ Transfer ]\r\nMovable";
            this.baseLabel_LDPickerPos_Moving.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabPage_Option
            // 
            this.tabPage_Option.BackColor = System.Drawing.Color.Silver;
            this.tabPage_Option.Controls.Add(this.textBox_LDPickerSetting_PickerVibrationTimes);
            this.tabPage_Option.Controls.Add(this.baseLabel_LDPickerSetting_Vibration);
            this.tabPage_Option.Controls.Add(this.textBox_Option_ULPickUpRetry_Count);
            this.tabPage_Option.Controls.Add(this.button_Option_ULPickUpRetry_Enable);
            this.tabPage_Option.Controls.Add(this.pictureBox_Option_ULPickUp_Retry_Enable);
            this.tabPage_Option.Controls.Add(this.textBox_Option_LDPickUpRetry_Count);
            this.tabPage_Option.Controls.Add(this.button_Option_LDPickUpRetry_Enable);
            this.tabPage_Option.Controls.Add(this.pictureBox_Option_LDPickUp_Retry_Enable);
            this.tabPage_Option.Controls.Add(this.button_Option_AllModuleThickCheck_Enable);
            this.tabPage_Option.Controls.Add(this.pictureBox_Option_AllPanelThickCheck_Enable);
            this.tabPage_Option.Controls.Add(this.button_Option_DoorInterlock_Enable);
            this.tabPage_Option.Controls.Add(this.pictureBox_Option_DoorInterlock_Enable);
            this.tabPage_Option.Controls.Add(this.baseLabel_Option_RetryCount2);
            this.tabPage_Option.Controls.Add(this.baseLabel_Option_RetryCount);
            this.tabPage_Option.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.tabPage_Option.Location = new System.Drawing.Point(4, 36);
            this.tabPage_Option.Name = "tabPage_Option";
            this.tabPage_Option.Size = new System.Drawing.Size(808, 724);
            this.tabPage_Option.TabIndex = 5;
            this.tabPage_Option.Text = "Option";
            // 
            // textBox_LDPickerSetting_PickerVibrationTimes
            // 
            this.textBox_LDPickerSetting_PickerVibrationTimes.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_LDPickerSetting_PickerVibrationTimes.Location = new System.Drawing.Point(312, 152);
            this.textBox_LDPickerSetting_PickerVibrationTimes.Name = "textBox_LDPickerSetting_PickerVibrationTimes";
            this.textBox_LDPickerSetting_PickerVibrationTimes.Size = new System.Drawing.Size(51, 25);
            this.textBox_LDPickerSetting_PickerVibrationTimes.TabIndex = 251;
            this.textBox_LDPickerSetting_PickerVibrationTimes.Text = "3";
            this.textBox_LDPickerSetting_PickerVibrationTimes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseLabel_LDPickerSetting_Vibration
            // 
            this.baseLabel_LDPickerSetting_Vibration.AutoSize = true;
            this.baseLabel_LDPickerSetting_Vibration.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_LDPickerSetting_Vibration.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LDPickerSetting_Vibration.Location = new System.Drawing.Point(126, 152);
            this.baseLabel_LDPickerSetting_Vibration.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LDPickerSetting_Vibration.Name = "baseLabel_LDPickerSetting_Vibration";
            this.baseLabel_LDPickerSetting_Vibration.Size = new System.Drawing.Size(178, 34);
            this.baseLabel_LDPickerSetting_Vibration.TabIndex = 250;
            this.baseLabel_LDPickerSetting_Vibration.Text = "Number of picker vibrations\r\n(To pick up just 1 Module)";
            // 
            // textBox_Option_ULPickUpRetry_Count
            // 
            this.textBox_Option_ULPickUpRetry_Count.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_Option_ULPickUpRetry_Count.Location = new System.Drawing.Point(409, 213);
            this.textBox_Option_ULPickUpRetry_Count.Name = "textBox_Option_ULPickUpRetry_Count";
            this.textBox_Option_ULPickUpRetry_Count.Size = new System.Drawing.Size(49, 25);
            this.textBox_Option_ULPickUpRetry_Count.TabIndex = 207;
            this.textBox_Option_ULPickUpRetry_Count.Text = "3";
            this.textBox_Option_ULPickUpRetry_Count.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button_Option_ULPickUpRetry_Enable
            // 
            this.button_Option_ULPickUpRetry_Enable.BackColor = System.Drawing.Color.White;
            this.button_Option_ULPickUpRetry_Enable.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_Option_ULPickUpRetry_Enable.FlatAppearance.BorderSize = 2;
            this.button_Option_ULPickUpRetry_Enable.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_Option_ULPickUpRetry_Enable.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_Option_ULPickUpRetry_Enable.Font = new System.Drawing.Font("Tahoma", 11.25F);
            this.button_Option_ULPickUpRetry_Enable.ForeColor = System.Drawing.Color.Black;
            this.button_Option_ULPickUpRetry_Enable.Location = new System.Drawing.Point(50, 205);
            this.button_Option_ULPickUpRetry_Enable.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_Option_ULPickUpRetry_Enable.Name = "button_Option_ULPickUpRetry_Enable";
            this.button_Option_ULPickUpRetry_Enable.Size = new System.Drawing.Size(314, 39);
            this.button_Option_ULPickUpRetry_Enable.TabIndex = 205;
            this.button_Option_ULPickUpRetry_Enable.Text = "Unloader PickUp Retry      (On : Enable)";
            this.button_Option_ULPickUpRetry_Enable.UseVisualStyleBackColor = false;
            // 
            // pictureBox_Option_ULPickUp_Retry_Enable
            // 
            this.pictureBox_Option_ULPickUp_Retry_Enable.Image = global::SLD200.Properties.Resources.StartOn;
            this.pictureBox_Option_ULPickUp_Retry_Enable.Location = new System.Drawing.Point(11, 209);
            this.pictureBox_Option_ULPickUp_Retry_Enable.Name = "pictureBox_Option_ULPickUp_Retry_Enable";
            this.pictureBox_Option_ULPickUp_Retry_Enable.Size = new System.Drawing.Size(32, 32);
            this.pictureBox_Option_ULPickUp_Retry_Enable.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Option_ULPickUp_Retry_Enable.TabIndex = 204;
            this.pictureBox_Option_ULPickUp_Retry_Enable.TabStop = false;
            // 
            // textBox_Option_LDPickUpRetry_Count
            // 
            this.textBox_Option_LDPickUpRetry_Count.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textBox_Option_LDPickUpRetry_Count.Location = new System.Drawing.Point(409, 121);
            this.textBox_Option_LDPickUpRetry_Count.Name = "textBox_Option_LDPickUpRetry_Count";
            this.textBox_Option_LDPickUpRetry_Count.Size = new System.Drawing.Size(49, 25);
            this.textBox_Option_LDPickUpRetry_Count.TabIndex = 203;
            this.textBox_Option_LDPickUpRetry_Count.Text = "3";
            this.textBox_Option_LDPickUpRetry_Count.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button_Option_LDPickUpRetry_Enable
            // 
            this.button_Option_LDPickUpRetry_Enable.BackColor = System.Drawing.Color.White;
            this.button_Option_LDPickUpRetry_Enable.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_Option_LDPickUpRetry_Enable.FlatAppearance.BorderSize = 2;
            this.button_Option_LDPickUpRetry_Enable.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_Option_LDPickUpRetry_Enable.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_Option_LDPickUpRetry_Enable.Font = new System.Drawing.Font("Tahoma", 11.25F);
            this.button_Option_LDPickUpRetry_Enable.ForeColor = System.Drawing.Color.Black;
            this.button_Option_LDPickUpRetry_Enable.Location = new System.Drawing.Point(50, 113);
            this.button_Option_LDPickUpRetry_Enable.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_Option_LDPickUpRetry_Enable.Name = "button_Option_LDPickUpRetry_Enable";
            this.button_Option_LDPickUpRetry_Enable.Size = new System.Drawing.Size(314, 39);
            this.button_Option_LDPickUpRetry_Enable.TabIndex = 201;
            this.button_Option_LDPickUpRetry_Enable.Text = "  Loader PickUp Retry       (On : Enable)";
            this.button_Option_LDPickUpRetry_Enable.UseVisualStyleBackColor = false;
            // 
            // pictureBox_Option_LDPickUp_Retry_Enable
            // 
            this.pictureBox_Option_LDPickUp_Retry_Enable.Image = global::SLD200.Properties.Resources.StartOn;
            this.pictureBox_Option_LDPickUp_Retry_Enable.Location = new System.Drawing.Point(11, 117);
            this.pictureBox_Option_LDPickUp_Retry_Enable.Name = "pictureBox_Option_LDPickUp_Retry_Enable";
            this.pictureBox_Option_LDPickUp_Retry_Enable.Size = new System.Drawing.Size(32, 32);
            this.pictureBox_Option_LDPickUp_Retry_Enable.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Option_LDPickUp_Retry_Enable.TabIndex = 200;
            this.pictureBox_Option_LDPickUp_Retry_Enable.TabStop = false;
            // 
            // button_Option_AllModuleThickCheck_Enable
            // 
            this.button_Option_AllModuleThickCheck_Enable.BackColor = System.Drawing.Color.White;
            this.button_Option_AllModuleThickCheck_Enable.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_Option_AllModuleThickCheck_Enable.FlatAppearance.BorderSize = 2;
            this.button_Option_AllModuleThickCheck_Enable.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_Option_AllModuleThickCheck_Enable.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_Option_AllModuleThickCheck_Enable.Font = new System.Drawing.Font("Tahoma", 11.25F);
            this.button_Option_AllModuleThickCheck_Enable.ForeColor = System.Drawing.Color.Black;
            this.button_Option_AllModuleThickCheck_Enable.Location = new System.Drawing.Point(50, 62);
            this.button_Option_AllModuleThickCheck_Enable.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_Option_AllModuleThickCheck_Enable.Name = "button_Option_AllModuleThickCheck_Enable";
            this.button_Option_AllModuleThickCheck_Enable.Size = new System.Drawing.Size(314, 39);
            this.button_Option_AllModuleThickCheck_Enable.TabIndex = 199;
            this.button_Option_AllModuleThickCheck_Enable.Text = " All Module Thick Check    (On : Enable)";
            this.button_Option_AllModuleThickCheck_Enable.UseVisualStyleBackColor = false;
            // 
            // pictureBox_Option_AllPanelThickCheck_Enable
            // 
            this.pictureBox_Option_AllPanelThickCheck_Enable.Image = global::SLD200.Properties.Resources.StartOn;
            this.pictureBox_Option_AllPanelThickCheck_Enable.Location = new System.Drawing.Point(11, 66);
            this.pictureBox_Option_AllPanelThickCheck_Enable.Name = "pictureBox_Option_AllPanelThickCheck_Enable";
            this.pictureBox_Option_AllPanelThickCheck_Enable.Size = new System.Drawing.Size(32, 32);
            this.pictureBox_Option_AllPanelThickCheck_Enable.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Option_AllPanelThickCheck_Enable.TabIndex = 198;
            this.pictureBox_Option_AllPanelThickCheck_Enable.TabStop = false;
            // 
            // button_Option_DoorInterlock_Enable
            // 
            this.button_Option_DoorInterlock_Enable.BackColor = System.Drawing.Color.White;
            this.button_Option_DoorInterlock_Enable.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_Option_DoorInterlock_Enable.FlatAppearance.BorderSize = 2;
            this.button_Option_DoorInterlock_Enable.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_Option_DoorInterlock_Enable.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_Option_DoorInterlock_Enable.Font = new System.Drawing.Font("Tahoma", 11.25F);
            this.button_Option_DoorInterlock_Enable.ForeColor = System.Drawing.Color.Black;
            this.button_Option_DoorInterlock_Enable.Location = new System.Drawing.Point(50, 11);
            this.button_Option_DoorInterlock_Enable.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_Option_DoorInterlock_Enable.Name = "button_Option_DoorInterlock_Enable";
            this.button_Option_DoorInterlock_Enable.Size = new System.Drawing.Size(314, 39);
            this.button_Option_DoorInterlock_Enable.TabIndex = 197;
            this.button_Option_DoorInterlock_Enable.Text = "     Door   Interlock         (On : Enable)";
            this.button_Option_DoorInterlock_Enable.UseVisualStyleBackColor = false;
            // 
            // pictureBox_Option_DoorInterlock_Enable
            // 
            this.pictureBox_Option_DoorInterlock_Enable.Image = global::SLD200.Properties.Resources.StartOn;
            this.pictureBox_Option_DoorInterlock_Enable.Location = new System.Drawing.Point(11, 15);
            this.pictureBox_Option_DoorInterlock_Enable.Name = "pictureBox_Option_DoorInterlock_Enable";
            this.pictureBox_Option_DoorInterlock_Enable.Size = new System.Drawing.Size(32, 32);
            this.pictureBox_Option_DoorInterlock_Enable.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Option_DoorInterlock_Enable.TabIndex = 196;
            this.pictureBox_Option_DoorInterlock_Enable.TabStop = false;
            // 
            // baseLabel_Option_RetryCount2
            // 
            this.baseLabel_Option_RetryCount2.AutoSize = true;
            this.baseLabel_Option_RetryCount2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_Option_RetryCount2.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Option_RetryCount2.Location = new System.Drawing.Point(369, 217);
            this.baseLabel_Option_RetryCount2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Option_RetryCount2.Name = "baseLabel_Option_RetryCount2";
            this.baseLabel_Option_RetryCount2.Size = new System.Drawing.Size(38, 17);
            this.baseLabel_Option_RetryCount2.TabIndex = 206;
            this.baseLabel_Option_RetryCount2.Text = "Retry";
            // 
            // baseLabel_Option_RetryCount
            // 
            this.baseLabel_Option_RetryCount.AutoSize = true;
            this.baseLabel_Option_RetryCount.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_Option_RetryCount.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Option_RetryCount.Location = new System.Drawing.Point(369, 125);
            this.baseLabel_Option_RetryCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Option_RetryCount.Name = "baseLabel_Option_RetryCount";
            this.baseLabel_Option_RetryCount.Size = new System.Drawing.Size(38, 17);
            this.baseLabel_Option_RetryCount.TabIndex = 202;
            this.baseLabel_Option_RetryCount.Text = "Retry";
            // 
            // tabControl_Jog
            // 
            this.tabControl_Jog.Controls.Add(this.tabPage_Unloader);
            this.tabControl_Jog.Controls.Add(this.tabPage_WorkStage);
            this.tabControl_Jog.Controls.Add(this.tabPage_Loader);
            this.tabControl_Jog.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.tabControl_Jog.ItemSize = new System.Drawing.Size(130, 32);
            this.tabControl_Jog.Location = new System.Drawing.Point(434, 73);
            this.tabControl_Jog.Multiline = true;
            this.tabControl_Jog.Name = "tabControl_Jog";
            this.tabControl_Jog.SelectedIndex = 0;
            this.tabControl_Jog.Size = new System.Drawing.Size(651, 522);
            this.tabControl_Jog.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl_Jog.TabIndex = 199;
            // 
            // tabPage_Unloader
            // 
            this.tabPage_Unloader.BackColor = System.Drawing.Color.Transparent;
            this.tabPage_Unloader.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.tabPage_Unloader.Location = new System.Drawing.Point(4, 36);
            this.tabPage_Unloader.Name = "tabPage_Unloader";
            this.tabPage_Unloader.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Unloader.Size = new System.Drawing.Size(643, 482);
            this.tabPage_Unloader.TabIndex = 2;
            this.tabPage_Unloader.Text = "Unloader";
            // 
            // tabPage_WorkStage
            // 
            this.tabPage_WorkStage.BackColor = System.Drawing.Color.Transparent;
            this.tabPage_WorkStage.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.tabPage_WorkStage.Location = new System.Drawing.Point(4, 36);
            this.tabPage_WorkStage.Name = "tabPage_WorkStage";
            this.tabPage_WorkStage.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_WorkStage.Size = new System.Drawing.Size(643, 482);
            this.tabPage_WorkStage.TabIndex = 0;
            this.tabPage_WorkStage.Text = "Work Stage";
            // 
            // tabPage_Loader
            // 
            this.tabPage_Loader.BackColor = System.Drawing.Color.Transparent;
            this.tabPage_Loader.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.tabPage_Loader.Location = new System.Drawing.Point(4, 36);
            this.tabPage_Loader.Name = "tabPage_Loader";
            this.tabPage_Loader.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_Loader.Size = new System.Drawing.Size(643, 482);
            this.tabPage_Loader.TabIndex = 1;
            this.tabPage_Loader.Text = "Loader";
            // 
            // m_visionImageViewer_HighRes
            // 
            this.m_visionImageViewer_HighRes.BackColor = System.Drawing.Color.Black;
            this.m_visionImageViewer_HighRes.Camera = null;
            this.m_visionImageViewer_HighRes.CameraSwitch = null;
            this.m_visionImageViewer_HighRes.FrameRate = 1D;
            this.m_visionImageViewer_HighRes.InputImage = null;
            this.m_visionImageViewer_HighRes.IsViewCustomizedImage = false;
            this.m_visionImageViewer_HighRes.Location = new System.Drawing.Point(4, 404);
            this.m_visionImageViewer_HighRes.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.m_visionImageViewer_HighRes.Name = "m_visionImageViewer_HighRes";
            this.m_visionImageViewer_HighRes.OperatingType = QMC.Common.Hmi.VisionImageViewer.OperatingTypes.Center;
            this.m_visionImageViewer_HighRes.Simulated = false;
            this.m_visionImageViewer_HighRes.Size = new System.Drawing.Size(423, 357);
            this.m_visionImageViewer_HighRes.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.m_visionImageViewer_HighRes.TabIndex = 179;
            this.m_visionImageViewer_HighRes.TabStop = false;
            this.m_visionImageViewer_HighRes.UpdateDelayTime = 160;
            this.m_visionImageViewer_HighRes.VisibleCrossLine = true;
            // 
            // m_visionImageViewer_LowRes
            // 
            this.m_visionImageViewer_LowRes.BackColor = System.Drawing.Color.Black;
            this.m_visionImageViewer_LowRes.Camera = null;
            this.m_visionImageViewer_LowRes.CameraSwitch = null;
            this.m_visionImageViewer_LowRes.FrameRate = 1D;
            this.m_visionImageViewer_LowRes.InputImage = null;
            this.m_visionImageViewer_LowRes.IsViewCustomizedImage = false;
            this.m_visionImageViewer_LowRes.Location = new System.Drawing.Point(4, 23);
            this.m_visionImageViewer_LowRes.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.m_visionImageViewer_LowRes.Name = "m_visionImageViewer_LowRes";
            this.m_visionImageViewer_LowRes.OperatingType = QMC.Common.Hmi.VisionImageViewer.OperatingTypes.Center;
            this.m_visionImageViewer_LowRes.Simulated = false;
            this.m_visionImageViewer_LowRes.Size = new System.Drawing.Size(423, 357);
            this.m_visionImageViewer_LowRes.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.m_visionImageViewer_LowRes.TabIndex = 178;
            this.m_visionImageViewer_LowRes.TabStop = false;
            this.m_visionImageViewer_LowRes.UpdateDelayTime = 160;
            this.m_visionImageViewer_LowRes.VisibleCrossLine = true;
            // 
            // baseLabel_HighRes_Camera
            // 
            this.baseLabel_HighRes_Camera.BackColor = System.Drawing.Color.White;
            this.baseLabel_HighRes_Camera.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_HighRes_Camera.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.baseLabel_HighRes_Camera.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_HighRes_Camera.Location = new System.Drawing.Point(4, 385);
            this.baseLabel_HighRes_Camera.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_HighRes_Camera.Name = "baseLabel_HighRes_Camera";
            this.baseLabel_HighRes_Camera.Size = new System.Drawing.Size(423, 19);
            this.baseLabel_HighRes_Camera.TabIndex = 181;
            this.baseLabel_HighRes_Camera.Text = "[  Fine  Vision  ]";
            this.baseLabel_HighRes_Camera.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel_LowRes_Camera
            // 
            this.baseLabel_LowRes_Camera.BackColor = System.Drawing.Color.White;
            this.baseLabel_LowRes_Camera.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_LowRes_Camera.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.baseLabel_LowRes_Camera.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LowRes_Camera.Location = new System.Drawing.Point(4, 4);
            this.baseLabel_LowRes_Camera.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LowRes_Camera.Name = "baseLabel_LowRes_Camera";
            this.baseLabel_LowRes_Camera.Size = new System.Drawing.Size(423, 19);
            this.baseLabel_LowRes_Camera.TabIndex = 180;
            this.baseLabel_LowRes_Camera.Text = "[  Coarse  Vision  ]";
            this.baseLabel_LowRes_Camera.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ParameterSetting_SLD200
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.Controls.Add(this.tabControl_Jog);
            this.Controls.Add(this.tabControl_ParameterSet);
            this.Controls.Add(this.baseLabel_HighRes_Camera);
            this.Controls.Add(this.baseLabel_LowRes_Camera);
            this.Controls.Add(this.m_visionImageViewer_HighRes);
            this.Controls.Add(this.m_visionImageViewer_LowRes);
            this.Controls.Add(this.btnUpperCamera_StartLive);
            this.Controls.Add(this.btnUpperCamera_Init);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ParameterSetting_SLD200";
            this.Size = new System.Drawing.Size(1910, 770);
            this.tabControl_ParameterSet.ResumeLayout(false);
            this.tabPage_Data.ResumeLayout(false);
            this.tabPage_Layout.ResumeLayout(false);
            this.baseGroupBox_ScanAreaSet.ResumeLayout(false);
            this.baseGroupBox_ScanAreaSet.PerformLayout();
            this.baseGroupBox_ModuleThickness_FiducialArea.ResumeLayout(false);
            this.baseGroupBox_ModuleThickness_FiducialArea.PerformLayout();
            this.baseGroupBox_ModuleThickness_DrillingArea.ResumeLayout(false);
            this.baseGroupBox_ModuleThickness_DrillingArea.PerformLayout();
            this.tabPage_Parameter.ResumeLayout(false);
            this.baseGroupBox_DrillingToolParam.ResumeLayout(false);
            this.baseGroupBox_DrillingToolParam.PerformLayout();
            this.baseGroupBox_DrillingToolList.ResumeLayout(false);
            this.tabPage_Fiducial.ResumeLayout(false);
            this.baseGroupBox_SearchResult.ResumeLayout(false);
            this.baseGroupBox_SearchResult.PerformLayout();
            this.baseGroupBox_Light.ResumeLayout(false);
            this.baseGroupBox_Light.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_HighResVision_Light)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_LowResVision_Light)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_HighResVision_Light)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_LowResVision_Light)).EndInit();
            this.baseGroupBox_FiducialModel.ResumeLayout(false);
            this.baseGroupBox_FiducialModel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_FiducialModel_Image)).EndInit();
            this.baseGroupBox_FiducialList.ResumeLayout(false);
            this.tabPage_Position.ResumeLayout(false);
            this.tabControl_Position.ResumeLayout(false);
            this.tabPage_Pos_Unloader.ResumeLayout(false);
            this.tabPage_Pos_Unloader.PerformLayout();
            this.tabPage_Pos_WorkStage.ResumeLayout(false);
            this.tabPage_Pos_WorkStage.PerformLayout();
            this.tabPage_Pos_Loader.ResumeLayout(false);
            this.tabPage_Pos_Loader.PerformLayout();
            this.tabPage_Option.ResumeLayout(false);
            this.tabPage_Option.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Option_ULPickUp_Retry_Enable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Option_LDPickUp_Retry_Enable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Option_AllPanelThickCheck_Enable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Option_DoorInterlock_Enable)).EndInit();
            this.tabControl_Jog.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_visionImageViewer_HighRes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_visionImageViewer_LowRes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        public NeedleCalibrationJogControl needleCalibrationJogControl;

        private System.Windows.Forms.Label lblUpperCamera;
        private System.Windows.Forms.Label lblLowerCamera;
        private System.Windows.Forms.Button btnUpperCamera_Init;
        private System.Windows.Forms.Button btnUpperCamera_StartLive;
        private BaseLabel baseLabel_HighRes_Camera;
        private BaseLabel baseLabel_LowRes_Camera;
        private VisionImageViewer m_visionImageViewer_HighRes;
        private VisionImageViewer m_visionImageViewer_LowRes;
        private System.Windows.Forms.TabControl tabControl_ParameterSet;
        private System.Windows.Forms.TabPage tabPage_Data;
        private System.Windows.Forms.TabPage tabPage_Layout;
        private System.Windows.Forms.TabPage tabPage_Parameter;
        private System.Windows.Forms.TabPage tabPage_Fiducial;
        private System.Windows.Forms.TabPage tabPage_Position;
        private System.Windows.Forms.TabPage tabPage_Option;
        private BaseGroupBox baseGroupBox_ModuleThickness_DrillingArea;
        private System.Windows.Forms.TextBox textBox_DrillingPos_PNLThickCheck_PosY;
        private BaseLabel baseLabel_DrillingPosThickCheck_PosY;
        private System.Windows.Forms.TextBox textBox_DrillingPos_PNLThickCheck_PosX;
        private BaseLabel baseLabel_DrillingPosThickCheck_PosX;
        private System.Windows.Forms.TextBox textBox_DrillingPos_PNLThickness;
        private BaseLabel baseLabel_DrillingPos_PNLThickness;
        private System.Windows.Forms.Button button_DrillingPos_PNLThickCheck_Start;
        private BaseGroupBox baseGroupBox_ModuleThickness_FiducialArea;
        private System.Windows.Forms.Button button_FiducialPos_PNLThickCheck_Start;
        private System.Windows.Forms.TextBox textBox_FiducialPos_PNLThickness;
        private BaseLabel baseLabel_FiducialPos_PNLThickness;
        private System.Windows.Forms.TextBox textBox_FiducialPos_PNLThickCheck_PosY;
        private BaseLabel baseLabel_FiducialPosThickCheck_PosY;
        private System.Windows.Forms.TextBox textBox_FiducialPos_PNLThickCheck_PosX;
        private BaseLabel baseLabel_FiducialPosThickCheck_PosX;
        private BaseGroupBox baseGroupBox_DrillingToolParam;
        private BaseLabel baseLabel_DrillingTool_LaserOffDelay_Unit;
        private System.Windows.Forms.TextBox textBox_DrillingTool_LaserOffDelay;
        private BaseLabel baseLabel_DrillingTool_LaserOffDelay;
        private BaseLabel baseLabel_DrillingTool_LaserOnDelay_Unit;
        private System.Windows.Forms.TextBox textBox_DrillingTool_LaserOnDelay;
        private BaseLabel baseLabel_DrillingTool_LaserOnDelay;
        private BaseLabel baseLabel_DrillingTool_JumpDelay_Unit;
        private System.Windows.Forms.TextBox textBox_DrillingTool_JumpDelay;
        private BaseLabel baseLabel_DrillingTool_JumpDelay;
        private BaseLabel baseLabel_DrillingTool_MarkDelay_Unit;
        private System.Windows.Forms.TextBox textBox_DrillingTool_MarkDelay;
        private BaseLabel baseLabel_DrillingTool_MarkDelay;
        private BaseLabel baseLabel_DrillingTool_JumpSpeed_Unit;
        private System.Windows.Forms.TextBox textBox_DrillingTool_JumpSpeed;
        private BaseLabel baseLabel_DrillingTool_JumpSpeed;
        private BaseLabel baseLabel_DrillingTool_MarkSpeed_Unit;
        private System.Windows.Forms.TextBox textBox_DrillingTool_MarkSpeed;
        private BaseLabel baseLabel_DrillingTool_MarkSpeed;
        private BaseLabel baseLabel_DrillingTool_Freq_Unit;
        private System.Windows.Forms.TextBox textBox_DrillingTool_Frequency;
        private BaseLabel baseLabel_DrillingTool_Freq;
        private BaseLabel baseLabel_DrillingTool_ZOffset_Unit;
        private System.Windows.Forms.TextBox textBox_DrillingTool_ZOffset;
        private BaseLabel baseLabel_DrillingTool_ZOffset;
        private System.Windows.Forms.TextBox textBox_DrillingTool_RepeatCount;
        private BaseLabel baseLabel_DrillingTool_RepeatCount;
        private BaseLabel baseLabel_DrillingTool_HoleSize_Unit;
        private System.Windows.Forms.TextBox textBox_DrillingTool_HoleSize;
        private BaseLabel baseLabel_DrillingTool_HoleSize;
        private System.Windows.Forms.ComboBox comboBox_DrillingTool_MaskNo;
        private BaseLabel baseLabel_DrillingTool_MaskNo;
        private System.Windows.Forms.ComboBox comboBox_DrillingTool_Type;
        private BaseLabel baseLabel_DrillingTool_Type;
        private System.Windows.Forms.TextBox textBox_DrillingTool_No;
        private BaseLabel baseLabel_DrillingTool_No;
        private BaseGroupBox baseGroupBox_DrillingToolList;
        private System.Windows.Forms.Button button_DrillingTool_Delete;
        private System.Windows.Forms.Button button_DrillingTool_Add;
        private System.Windows.Forms.TreeView treeView_DrillingTool;
        private System.Windows.Forms.Button button_DrillingMainTool_Save;
        private System.Windows.Forms.Button button_DrillingMainTool_Open;
        private BaseGroupBox baseGroupBox_FiducialList;
        private System.Windows.Forms.Button button_MoveTo_FiducialMarkPos;
        private System.Windows.Forms.TreeView treeView_FiducialMark;
        private BaseGroupBox baseGroupBox_FiducialModel;
        private BaseLabel baseLabel18;
        private System.Windows.Forms.TextBox textBox10;
        private BaseLabel baseLabel_FiducialSize_A;
        private System.Windows.Forms.ComboBox comboBox_FiducialType;
        private BaseLabel baseLabel_FiducialType;
        private BaseLabel baseLabel21;
        private System.Windows.Forms.TextBox textBox12;
        private BaseLabel baseLabel22;
        private BaseLabel baseLabel19;
        private System.Windows.Forms.TextBox textBox11;
        private BaseLabel baseLabel20;
        private System.Windows.Forms.PictureBox pictureBox_FiducialModel_Image;
        private System.Windows.Forms.ComboBox comboBox_Polarity;
        private BaseLabel baseLabel_Polarity;
        private BaseGroupBox baseGroupBox_Light;
        private BaseLabel baseLabel_LowRes_Light;
        private System.Windows.Forms.NumericUpDown numericUpDown_HighResVision_Light;
        private BaseLabel baseLabel_HighRes_Light;
        private System.Windows.Forms.NumericUpDown numericUpDown_LowResVision_Light;
        private System.Windows.Forms.TrackBar trackBar_LowResVision_Light;
        private System.Windows.Forms.TrackBar trackBar_HighResVision_Light;
        private BaseGroupBox baseGroupBox_SearchResult;
        private BaseLabel baseLabel_MinScore_Unit;
        private System.Windows.Forms.TextBox textBox_SearchResult_MinScore;
        private BaseLabel baseLabel_SearchResult_MinScore;
        private BaseLabel baseLabel1;
        private System.Windows.Forms.TextBox textBox_SearchResult_Size;
        private BaseLabel baseLabel_SearchResult_Size;
        private System.Windows.Forms.Button button_Option_DoorInterlock_Enable;
        private System.Windows.Forms.PictureBox pictureBox_Option_DoorInterlock_Enable;
        private System.Windows.Forms.Button button_Option_AllModuleThickCheck_Enable;
        private System.Windows.Forms.PictureBox pictureBox_Option_AllPanelThickCheck_Enable;
        private System.Windows.Forms.Button button_Option_LDPickUpRetry_Enable;
        private System.Windows.Forms.PictureBox pictureBox_Option_LDPickUp_Retry_Enable;
        private System.Windows.Forms.TextBox textBox_Option_LDPickUpRetry_Count;
        private BaseLabel baseLabel_Option_RetryCount;
        private System.Windows.Forms.TextBox textBox_Option_ULPickUpRetry_Count;
        private BaseLabel baseLabel_Option_RetryCount2;
        private System.Windows.Forms.Button button_Option_ULPickUpRetry_Enable;
        private System.Windows.Forms.PictureBox pictureBox_Option_ULPickUp_Retry_Enable;
        private System.Windows.Forms.TabControl tabControl_Jog;
        private System.Windows.Forms.TabPage tabPage_WorkStage;
        private System.Windows.Forms.TabPage tabPage_Loader;
        private System.Windows.Forms.TabPage tabPage_Unloader;
        private BaseGroupBox baseGroupBox_ScanAreaSet;
        private System.Windows.Forms.TextBox textBox2;
        private BaseLabel baseLabel_ScanArea_Height;
        private System.Windows.Forms.TextBox textBox3;
        private BaseLabel baseLabel_ScanArea_Width;
        private System.Windows.Forms.TabControl tabControl_Position;
        private System.Windows.Forms.TabPage tabPage_Pos_Unloader;
        private System.Windows.Forms.TabPage tabPage_Pos_WorkStage;
        private System.Windows.Forms.TabPage tabPage_Pos_Loader;
        private System.Windows.Forms.Button button_ULPickerPos_Moving_Get;
        private System.Windows.Forms.TextBox textBox_ULPickerPos_Moving_Z;
        private BaseLabel baseLabel_ULPickerPos_Moving_Z;
        private BaseLabel baseLabel_ULPickerPos_Moving;
        private System.Windows.Forms.Button button_ULPickerPos_Module_PutDown_Stacker1_Get;
        private System.Windows.Forms.TextBox textBox_ULPickerPos_Module_PutDown_Stacker1_Z;
        private BaseLabel baseLabel_ULPickerPos_MGZ2_Z;
        private System.Windows.Forms.TextBox textBox_ULPickerPos_Module_PutDown_Stacker1_X;
        private BaseLabel baseLabel_ULPickerPos_MGZ2_X;
        private BaseLabel baseLabel_ULPickerPos_MGZ2;
        private System.Windows.Forms.Button button_ULPickerPos_Module_PutDown_Stacker0_Get;
        private System.Windows.Forms.TextBox textBox_ULPickerPos_Module_PutDown_Stacker0_Z;
        private BaseLabel baseLabel_ULPickerPos_MGZ1_Z;
        private System.Windows.Forms.TextBox textBox_ULPickerPos_Module_PutDown_Stacker0_X;
        private BaseLabel baseLabel_ULPickerPos_MGZ1_X;
        private BaseLabel baseLabel_ULPickerPos_MGZ1;
        private System.Windows.Forms.Button button_ULPickerPos_NgBox_Get;
        private System.Windows.Forms.Button button_ULPickerPos_Module_PickUp_WorkTable_Get;
        private System.Windows.Forms.TextBox textBox_ULPickerPos_NgBox_Z;
        private BaseLabel baseLabel_ULPickerPos_NgBox_Z;
        private System.Windows.Forms.TextBox textBox_ULPickerPos_NgBox_X;
        private BaseLabel baseLabel_ULPickerPos_NgBox_X;
        private BaseLabel baseLabel_ULPickerPos_NGBox;
        private System.Windows.Forms.TextBox textBox_ULPickerPos_Module_PickUp_WorkTable_Z;
        private BaseLabel baseLabel_ULPickerPos_WorkTable_Z;
        private System.Windows.Forms.TextBox textBox_ULPickerPos_Module_PickUp_WorkTable_X;
        private BaseLabel baseLabel_ULPickerPos_WorkTable_X;
        private BaseLabel baseLabel_ULPickerPos_WorkTable;
        private System.Windows.Forms.Button button_ULStacker0Pos_Full_Get;
        private System.Windows.Forms.TextBox textBox_ULStacker0Pos_Full;
        private BaseLabel baseLabel_ULStacker0Pos_Full1;
        private BaseLabel baseLabel_ULStacker0Pos_Full;
        private System.Windows.Forms.Button button_ULStacker0Pos_Empty_Get;
        private System.Windows.Forms.TextBox textBox_ULStacker0Pos_Empty;
        private BaseLabel baseLabel_ULStacker0Pos_Empty1;
        private BaseLabel baseLabel_ULStacker0Pos_Empty;
        private System.Windows.Forms.Button button_ULStacker1Pos_Empty_Get;
        private System.Windows.Forms.TextBox textBox_ULStacker1Pos_Empty;
        private BaseLabel baseLabel_ULStacker1Pos_Empty1;
        private BaseLabel baseLabel_ULStacker1Pos_Empty;
        private System.Windows.Forms.Button button_ULStacker1Pos_Full_Get;
        private System.Windows.Forms.TextBox textBox_ULStacker1Pos_Full;
        private BaseLabel baseLabel_ULStacker1Pos_Full1;
        private BaseLabel baseLabel_ULStacker1Pos_Full;
        private System.Windows.Forms.Button button_LDPickerPos_Moving_Get;
        private System.Windows.Forms.TextBox textBox_LDPickerPos_Moving_Z;
        private BaseLabel baseLabel_LDPickerPos_Moving_Z;
        private BaseLabel baseLabel_LDPickerPos_Moving;
        private System.Windows.Forms.Button button_LDPickerPos_Module_PutDown_WorkTable_Get;
        private System.Windows.Forms.TextBox textBox_LDPickerPos_Module_PutDown_WorkTable_Z;
        private BaseLabel baseLabel2;
        private System.Windows.Forms.TextBox textBox_LDPickerPos_Module_PutDown_WorkTable_X;
        private BaseLabel baseLabel3;
        private BaseLabel baseLabel_LDPickerPos_WorkTable;
        private System.Windows.Forms.Button button_LDPickerPos_Module_PickUp_Stacker1_Get;
        private System.Windows.Forms.TextBox textBox_LDPickerPos_Module_PickUp_Stacker1_Z;
        private BaseLabel baseLabel4;
        private System.Windows.Forms.TextBox textBox_LDPickerPos_Module_PickUp_Stacker1_X;
        private BaseLabel baseLabel5;
        private BaseLabel baseLabel_LDPickerPos_MGZ2;
        private System.Windows.Forms.Button button_LDPickerPos_Module_PickUp_Stacker0_Get;
        private System.Windows.Forms.TextBox textBox_LDPickerPos_Module_PickUp_Stacker0_Z;
        private BaseLabel baseLabel7;
        private System.Windows.Forms.TextBox textBox_LDPickerPos_Module_PickUp_Stacker0_X;
        private BaseLabel baseLabel8;
        private BaseLabel baseLabel_LDPickerPos_MGZ1;
        private System.Windows.Forms.Button button_LDStacker1Pos_Empty_Get;
        private System.Windows.Forms.TextBox textBox_LDStacker1Pos_Empty;
        private BaseLabel baseLabel10;
        private BaseLabel baseLabel_LDStacker1Pos_Empty;
        private System.Windows.Forms.Button button_LDStacker1Pos_Full_Get;
        private System.Windows.Forms.TextBox textBox_LDStacker1Pos_Full;
        private BaseLabel baseLabel12;
        private BaseLabel baseLabel_LDStacker1Pos_Full;
        private System.Windows.Forms.Button button_LDStacker0Pos_Empty_Get;
        private System.Windows.Forms.TextBox textBox_LDStacker0Pos_Empty;
        private BaseLabel baseLabel14;
        private BaseLabel baseLabel_LDStacker0Pos_Empty;
        private System.Windows.Forms.Button button_LDStacker0Pos_Full_Get;
        private System.Windows.Forms.TextBox textBox_LDStacker0Pos_Full;
        private BaseLabel baseLabel16;
        private BaseLabel baseLabel_LDStacker0Pos_Full;
        private System.Windows.Forms.Button button_LDPickerPos_Module_PutDown_AlignTable_Get;
        private System.Windows.Forms.TextBox textBox_LDPickerPos_Module_PutDown_Aligner_Y;
        private BaseLabel baseLabel11;
        private System.Windows.Forms.TextBox textBox_LDPickerPos_Module_PutDown_Aligner_X;
        private BaseLabel baseLabel13;
        private BaseLabel baseLabel_LDPickerPos_Aligner1;
        private System.Windows.Forms.Button button_LDPickerPos_Module_PickUp_AlignTable_Get;
        private System.Windows.Forms.TextBox textBox_LDPickerPos_Module_PickUp_Aligner_Y;
        private BaseLabel baseLabel6;
        private System.Windows.Forms.TextBox textBox_LDPickerPos_Module_PickUp_Aligner_X;
        private BaseLabel baseLabel9;
        private BaseLabel baseLabel_LDPickerPos_Aligner;
        private System.Windows.Forms.Button button_LDAlignerPos_100mmClose_Get;
        private System.Windows.Forms.TextBox textBox_LDAlignerPos_100mmClose_Y;
        private BaseLabel baseLabel15;
        private System.Windows.Forms.TextBox textBox_LDAlignerPos_100mmClose_X;
        private BaseLabel baseLabel17;
        private BaseLabel baseLabel_LDAlignerPos_100mmClose;
        private System.Windows.Forms.Button button_LDAlignerPos_FullOpen_Get;
        private System.Windows.Forms.TextBox textBox_LDAlignerPos_FullOpen_Y;
        private BaseLabel baseLabel24;
        private System.Windows.Forms.TextBox textBox_LDAlignerPos_FullOpen_X;
        private BaseLabel baseLabel25;
        private BaseLabel baseLabel_LDAlignerPos_FullOpen;
        private System.Windows.Forms.Button button_MainUnitPos_JigChange_Get;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_JigChange_Y;
        private BaseLabel baseLabel23;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_JigChange_X;
        private BaseLabel baseLabel26;
        private BaseLabel baseLabel_MainUnitPos_JigChange;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_JigChange_Z;
        private BaseLabel baseLabel27;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_ModuleLoad_Z;
        private BaseLabel baseLabel28;
        private System.Windows.Forms.Button button_MainUnitPos_ModuleLoad_Get;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_ModuleLoad_Y;
        private BaseLabel baseLabel29;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_ModuleLoad_X;
        private BaseLabel baseLabel30;
        private BaseLabel baseLabel_MainUnitPos_ModuleLoad;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_ModuleUnload_Z;
        private BaseLabel baseLabel31;
        private System.Windows.Forms.Button button_MainUnitPos_ModuleUnload_Get;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_ModuleUnload_Y;
        private BaseLabel baseLabel32;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_ModuleUnload_X;
        private BaseLabel baseLabel33;
        private BaseLabel baseLabel_MainUnitPos_ModuleUnload;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_ScannerLensCleaning_Z;
        private BaseLabel baseLabel34;
        private System.Windows.Forms.Button button_MainUnitPos_ScannerLensCleaning_Get;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_ScannerLensCleaning_Y;
        private BaseLabel baseLabel35;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_ScannerLensCleaning_X;
        private BaseLabel baseLabel36;
        private BaseLabel baseLabel_MainUnitPos_ScannerLensCleaning;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_ScannerWorkStageCenter_Z;
        private BaseLabel baseLabel37;
        private System.Windows.Forms.Button button_MainUnitPos_ScannerWorkStageCenter_Get;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_ScannerWorkStageCenter_Y;
        private BaseLabel baseLabel38;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_ScannerWorkStageCenter_X;
        private BaseLabel baseLabel39;
        private BaseLabel baseLabel_MainUnitPos_ScannerStageCenter;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_ScannerPowerMeter_Z;
        private BaseLabel baseLabel40;
        private System.Windows.Forms.Button button_MainUnitPos_ScannerPowerMeter_Get;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_ScannerPowerMeter_Y;
        private BaseLabel baseLabel41;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_ScannerPowerMeter_X;
        private BaseLabel baseLabel42;
        private BaseLabel baseLabel_MainUnitPos_ScannerPowerMeter;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_HighResCamWorkStageCenter_Z;
        private BaseLabel baseLabel43;
        private System.Windows.Forms.Button button_MainUnitPos_HighResCamWorkStageCenter_Get;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_HighResCamWorkStageCenter_Y;
        private BaseLabel baseLabel44;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_HighResCamWorkStageCenter_X;
        private BaseLabel baseLabel45;
        private BaseLabel baseLabel_MainUnitPos_HighResCameraStageCenter;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_HighResCamCalSheetLT_Z;
        private BaseLabel baseLabel46;
        private System.Windows.Forms.Button button_MainUnitPos_HighResCamCalSheetLT_Get;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_HighResCamCalSheetLT_Y;
        private BaseLabel baseLabel47;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_HighResCamCalSheetLT_X;
        private BaseLabel baseLabel48;
        private BaseLabel baseLabel_MainUnitPos_HighResCameraCalSheetLT;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_HighResCamReticleGlass_Z;
        private BaseLabel baseLabel49;
        private System.Windows.Forms.Button button_MainUnitPos_HighResCamReticleGlass_Get;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_HighResCamReticleGlass_Y;
        private BaseLabel baseLabel50;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_HighResCamReticleGlass_X;
        private BaseLabel baseLabel_MainUnitPos_HighResCameraReticleGlass;
        private BaseLabel baseLabel51;
        private BaseLabel baseLabel52;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_LowResCamReticleGlass_Z;
        private BaseLabel baseLabel53;
        private System.Windows.Forms.Button button_MainUnitPos_LowResCamReticleGlass_Get;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_LowResCamReticleGlass_Y;
        private BaseLabel baseLabel54;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_LowResCamReticleGlass_X;
        private BaseLabel baseLabel_MainUnitPos_LowResCameraReticleGlass;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_LowResCamCalSheetLT_Z;
        private BaseLabel baseLabel56;
        private System.Windows.Forms.Button button_MainUnitPos_LowResCamCalSheetLT_Get;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_LowResCamCalSheetLT_Y;
        private BaseLabel baseLabel57;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_LowResCamCalSheetLT_X;
        private BaseLabel baseLabel58;
        private BaseLabel baseLabel_MainUnitPos_LowResCameraCalSheetLT;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_LowResCamWorkStageCenter_Z;
        private BaseLabel baseLabel60;
        private System.Windows.Forms.Button button_MainUnitPos_LowResCamWorkStageCenter_Get;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_LowResCamWorkStageCenter_Y;
        private BaseLabel baseLabel61;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_LowResCamWorkStageCenter_X;
        private BaseLabel baseLabel62;
        private BaseLabel baseLabel_MainUnitPos_LowResCameraStageCenter;
        private BaseLabel baseLabel68;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_HeightSensorCalSheetLT_Z;
        private BaseLabel baseLabel55;
        private System.Windows.Forms.Button button_MainUnitPos_HeightSensorCalSheetLT_Get;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_HeightSensorCalSheetLT_Y;
        private BaseLabel baseLabel59;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_HeightSensorCalSheetLT_X;
        private BaseLabel baseLabel63;
        private BaseLabel baseLabel_MainUnitPos_HeightSensorCalSheetLT;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_HeightSensorStageCenter_Z;
        private BaseLabel baseLabel65;
        private System.Windows.Forms.Button button_MainUnitPos_HeightSensorWorkStageCenter_Get;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_HeightSensorStageCenter_Y;
        private BaseLabel baseLabel66;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_HeightSensorStageCenter_X;
        private BaseLabel baseLabel_MainUnitPos_HeightSensorStageCenter;
        private BaseLabel baseLabel64;
        private System.Windows.Forms.Button button_MainUnitPos_MaskEmpty_Get;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_MaskEmpty_FwBw_Y;
        private BaseLabel baseLabel_MainUnitPos_MaskEmpty;
        private BaseLabel baseLabel72;
        private System.Windows.Forms.Button button_MainUnitPos_Mask4_Get;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_Mask4_FwBw_Y;
        private BaseLabel baseLabel_MainUnitPos_Mask4;
        private BaseLabel baseLabel74;
        private System.Windows.Forms.Button button_MainUnitPos_Mask3_Get;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_Mask3_FwBw_Y;
        private BaseLabel baseLabel_MainUnitPos_Mask3;
        private BaseLabel baseLabel70;
        private System.Windows.Forms.Button button_MainUnitPos_Mask2_Get;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_Mask2_FwBw_Y;
        private BaseLabel baseLabel_MainUnitPos_Mask2;
        private BaseLabel baseLabel67;
        private System.Windows.Forms.Button button_MainUnitPos_Mask1_Get;
        private System.Windows.Forms.TextBox textBox_MainUnitPos_Mask1_FwBw_Y;
        private BaseLabel baseLabel_MainUnitPos_Mask1;
        //public SpiralLab.Sirius.SiriusViewerForm SiriusViewer_Parameter;
        private System.Windows.Forms.TextBox textBox_LDPickerSetting_PickerVibrationTimes;
        private BaseLabel baseLabel_LDPickerSetting_Vibration;
    }
}
