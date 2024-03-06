using System.Drawing;

namespace CWA150SA_Onsemi300
{
    partial class Monitoring_CWA150SA
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
            this.groupBoxStatus = new System.Windows.Forms.GroupBox();
            this.ledStatus_Light_Connected = new System.Windows.Forms.Button();
            this.lblStatus_Light_Connected = new CWA150SA_Onsemi300.BaseLabel();
            this.groupBoxMainPanel = new System.Windows.Forms.GroupBox();
            this.pictureBoxMainVacuumCheck = new System.Windows.Forms.PictureBox();
            this.lblMainVacuumCheck = new CWA150SA_Onsemi300.BaseLabel();
            this.pictureBoxMainAirCheck = new System.Windows.Forms.PictureBox();
            this.lblMainAirCheck = new CWA150SA_Onsemi300.BaseLabel();
            this.timer_DIO_Status = new System.Windows.Forms.Timer(this.components);
            this.pictureBoxTrainImage_Upper = new System.Windows.Forms.PictureBox();
            this.pictureBoxTrainImage_Lower = new System.Windows.Forms.PictureBox();
            this.btnHomeAll = new System.Windows.Forms.Button();
            this.btnUpperCamera_Init = new System.Windows.Forms.Button();
            this.btnUpperCamera_StartLive = new System.Windows.Forms.Button();
            this.groupBoxCameraOpticalAxisCheck = new System.Windows.Forms.GroupBox();
            this.btnReticlePos_LowerCam_GO = new System.Windows.Forms.Button();
            this.btnReticlePos_UpperCam_GO = new System.Windows.Forms.Button();
            this.groupBoxAlignCheckPosParameter = new System.Windows.Forms.GroupBox();
            this.baseButton_EmptyChip_XYPos_GO = new CWA150SA_Onsemi300.BaseButton();
            this.baseLabel8 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseButton_CameraY_GoPos_Safety = new CWA150SA_Onsemi300.BaseButton();
            this.baseLabel19 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel17 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel15 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel14 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel12 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel11 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel16 = new CWA150SA_Onsemi300.BaseLabel();
            this.lblLowerCamera_Bot_ErrorData_Y = new CWA150SA_Onsemi300.BaseLabel();
            this.lblLowerCamera_Bot_ErrorData_X = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel20 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel10 = new CWA150SA_Onsemi300.BaseLabel();
            this.lblLowerCamera_Mid_ErrorData_Y = new CWA150SA_Onsemi300.BaseLabel();
            this.lblLowerCamera_Mid_ErrorData_X = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel13 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel_LowerCam = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel_Delta_Y = new CWA150SA_Onsemi300.BaseLabel();
            this.lblLowerCamera_Top_ErrorData_Y = new CWA150SA_Onsemi300.BaseLabel();
            this.lblLowerCamera_Top_ErrorData_X = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel_Delta_X = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel9 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel_OK = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelPosition_Bot = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelPosition_Mid = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelPosition_Top = new CWA150SA_Onsemi300.BaseLabel();
            this.baseButton_Y_Pos_GO3 = new CWA150SA_Onsemi300.BaseButton();
            this.baseButton_Y_Pos_GO2 = new CWA150SA_Onsemi300.BaseButton();
            this.baseButton_Y_Pos_GO1 = new CWA150SA_Onsemi300.BaseButton();
            this.baseLabel18 = new CWA150SA_Onsemi300.BaseLabel();
            this.tabControl_User = new System.Windows.Forms.TabControl();
            this.tabPage_ManualPacking = new System.Windows.Forms.TabPage();
            this.baseLabel_PitchMove = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel_JogMove = new CWA150SA_Onsemi300.BaseLabel();
            this.baseButton_Elev_JogMove_Down = new System.Windows.Forms.Button();
            this.baseButton_Elev_JogMove_Up = new System.Windows.Forms.Button();
            this.baseButton_Elev_PitchMove_Down = new System.Windows.Forms.Button();
            this.baseButton_Elev_PitchMove_Up = new System.Windows.Forms.Button();
            this.baseButton_Elev_GoPos_Wafer_ProbeCard_Packing = new System.Windows.Forms.Button();
            this.btnManualPacking = new System.Windows.Forms.Button();
            this.baseLabelManualPacking3 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelManualPacking3_Title = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelManualPacking2 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelManualPacking2_Title = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelManualPacking1_Title = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelManualPacking1 = new CWA150SA_Onsemi300.BaseLabel();
            this.tabPage_PackingOffsetChange = new System.Windows.Forms.TabPage();
            this.checkBox_Step = new System.Windows.Forms.CheckBox();
            this.checkBox_Jog = new System.Windows.Forms.CheckBox();
            this.radioButton_Movement_05mm = new System.Windows.Forms.RadioButton();
            this.radioButton_Movement_001mm = new System.Windows.Forms.RadioButton();
            this.radioButton_Movement_01mm = new System.Windows.Forms.RadioButton();
            this.buttonAxisXUpYDown = new System.Windows.Forms.Button();
            this.buttonAxisYDown = new System.Windows.Forms.Button();
            this.buttonAxisXUp = new System.Windows.Forms.Button();
            this.buttonAxisXUpYUp = new System.Windows.Forms.Button();
            this.buttonAxisXDownYDown = new System.Windows.Forms.Button();
            this.buttonAxisYUp = new System.Windows.Forms.Button();
            this.buttonAxisXDown = new System.Windows.Forms.Button();
            this.buttonAxisXDownYUp = new System.Windows.Forms.Button();
            this.baseButton_VisionXY_Pos_Get2 = new System.Windows.Forms.Button();
            this.baseButton_VisionXY_Pos_Get1 = new System.Windows.Forms.Button();
            this.baseLabelJogButtonComb_XY = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelJogButtonComb_Title = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelPackingOffsetChange2 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelPackingOffsetChange2_Title = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelPackingOffsetChange1_Title = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelPackingOffsetChange1 = new CWA150SA_Onsemi300.BaseLabel();
            this.tabPage_ReticlePositionChange = new System.Windows.Forms.TabPage();
            this.baseButton_ReticleGlass_Pos_Get = new System.Windows.Forms.Button();
            this.checkBox_Reticle_Step = new System.Windows.Forms.CheckBox();
            this.checkBox_Reticle_Jog = new System.Windows.Forms.CheckBox();
            this.radioButton_Reticle_Movement_05mm = new System.Windows.Forms.RadioButton();
            this.radioButton_Reticle_Movement_001mm = new System.Windows.Forms.RadioButton();
            this.radioButton_Reticle_Movement_01mm = new System.Windows.Forms.RadioButton();
            this.buttonAxisXUpYDown_Reticle = new System.Windows.Forms.Button();
            this.buttonAxisYDown_Reticle = new System.Windows.Forms.Button();
            this.buttonAxisXUp_Reticle = new System.Windows.Forms.Button();
            this.buttonAxisXUpYUp_Reticle = new System.Windows.Forms.Button();
            this.buttonAxisXDownYDown_Reticle = new System.Windows.Forms.Button();
            this.buttonAxisYUp_Reticle = new System.Windows.Forms.Button();
            this.buttonAxisXDown_Reticle = new System.Windows.Forms.Button();
            this.buttonAxisXDownYUp_Reticle = new System.Windows.Forms.Button();
            this.baseLabel22 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel23 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelReticlePositionChange2 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelReticlePositionChange2_Title = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelReticlePositionChange1_Title = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelReticlePositionChange1 = new CWA150SA_Onsemi300.BaseLabel();
            this.m_visionImageViewer_Lower = new QMC.Common.Hmi.VisionImageViewer();
            this.m_visionImageViewer_Upper = new QMC.Common.Hmi.VisionImageViewer();
            this.lblLowerCamera_AlignData = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelAngle_Lower = new CWA150SA_Onsemi300.BaseLabel();
            this.lblUpperCamera_AlignData = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelAngle_Upper = new CWA150SA_Onsemi300.BaseLabel();
            this.baseGroupBox_CycleButton = new CWA150SA_Onsemi300.BaseGroupBox();
            this.baseLabel21 = new CWA150SA_Onsemi300.BaseLabel();
            this.btnPAK_Leak_Check = new System.Windows.Forms.Button();
            this.pictureBoxProbePackingCheck = new System.Windows.Forms.PictureBox();
            this.lblProbePackingCheck = new CWA150SA_Onsemi300.BaseLabel();
            this.pictureBoxWaferVacuumCheck = new System.Windows.Forms.PictureBox();
            this.lblWaferVacuumCheck = new CWA150SA_Onsemi300.BaseLabel();
            this.pictureBoxThinChuckVacuumCheck = new System.Windows.Forms.PictureBox();
            this.lblThinChuckVacuumCheck = new CWA150SA_Onsemi300.BaseLabel();
            this.pictureBoxThinChuckDetect = new System.Windows.Forms.PictureBox();
            this.baseLabelThinChuckDetect = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel7 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel6 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel5 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel_Packing = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel_Align = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel2 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseButtonWaferVacuum = new CWA150SA_Onsemi300.BaseButton();
            this.baseButtonThinChuckVacuum = new CWA150SA_Onsemi300.BaseButton();
            this.baseLabel1 = new CWA150SA_Onsemi300.BaseLabel();
            this.btnPacking = new System.Windows.Forms.Button();
            this.btnProbeCardLocking = new System.Windows.Forms.Button();
            this.btnProbeCardLoadingReady = new System.Windows.Forms.Button();
            this.btnLoadingPos = new System.Windows.Forms.Button();
            this.btnMainWork_Start = new System.Windows.Forms.Button();
            this.tabControl_ProbeCard_ClampType = new System.Windows.Forms.TabControl();
            this.tabPage_TypeA = new System.Windows.Forms.TabPage();
            this.pictureBoxProbeBWDetect = new System.Windows.Forms.PictureBox();
            this.baseLabelProbeBWDetect = new CWA150SA_Onsemi300.BaseLabel();
            this.pictureBoxTopCoverDown = new System.Windows.Forms.PictureBox();
            this.baseLabelTopCoverDown = new CWA150SA_Onsemi300.BaseLabel();
            this.pictureBoxTopCoverUp = new System.Windows.Forms.PictureBox();
            this.baseLabelTopCoverUp = new CWA150SA_Onsemi300.BaseLabel();
            this.baseButtonTopCoverDown = new CWA150SA_Onsemi300.BaseButton();
            this.baseButtonTopCoverUp = new CWA150SA_Onsemi300.BaseButton();
            this.tabPage_TypeB = new System.Windows.Forms.TabPage();
            this.baseButtonProbeClamp_BW = new CWA150SA_Onsemi300.BaseButton();
            this.baseButtonProbeClamp_FW = new CWA150SA_Onsemi300.BaseButton();
            this.baseButtonProbeUnpackingCyl_Down = new CWA150SA_Onsemi300.BaseButton();
            this.pictureBoxProbeRightClamp_FW = new System.Windows.Forms.PictureBox();
            this.pictureBoxProbeLeftClamp_FW = new System.Windows.Forms.PictureBox();
            this.pictureBoxProbeRightClamp_BW = new System.Windows.Forms.PictureBox();
            this.pictureBoxProbeLeftClamp_BW = new System.Windows.Forms.PictureBox();
            this.pictureBoxProbeUnpackingCyl_Down = new System.Windows.Forms.PictureBox();
            this.pictureBoxProbeUnpackingCyl_Up = new System.Windows.Forms.PictureBox();
            this.baseButtonProbeUnpackingCyl_Up = new CWA150SA_Onsemi300.BaseButton();
            this.baseButtonProbeClamp_Down = new CWA150SA_Onsemi300.BaseButton();
            this.baseLabel_BW = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel_FW = new CWA150SA_Onsemi300.BaseLabel();
            this.baseGroupBox_ManualButton = new CWA150SA_Onsemi300.BaseGroupBox();
            this.baseButtonProbeUnpacking = new CWA150SA_Onsemi300.BaseButton();
            this.baseButtonProbePacking = new CWA150SA_Onsemi300.BaseButton();
            this.baseButtonThinChuckStageCleaning = new CWA150SA_Onsemi300.BaseButton();
            this.baseLabelTeachingImage_Lower1 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelTeachingImage_Upper1 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel_WaferChuck_Camera1 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel_ProbeCard_Camera1 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseGroupBox1 = new CWA150SA_Onsemi300.BaseGroupBox();
            this.baseButtonChangeRecipe = new CWA150SA_Onsemi300.BaseButton();
            this.baseTextBoxCurrentRecipe = new CWA150SA_Onsemi300.BaseTextBox();
            this.baseLabelCurrentRecipe = new CWA150SA_Onsemi300.BaseLabel();
            this.lblMachine_Status = new CWA150SA_Onsemi300.BaseLabel();
            this.lblStatus_ScannerPowerMeter_Connected = new CWA150SA_Onsemi300.BaseLabel();
            this.groupBoxStatus.SuspendLayout();
            this.groupBoxMainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMainVacuumCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMainAirCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTrainImage_Upper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTrainImage_Lower)).BeginInit();
            this.groupBoxCameraOpticalAxisCheck.SuspendLayout();
            this.groupBoxAlignCheckPosParameter.SuspendLayout();
            this.tabControl_User.SuspendLayout();
            this.tabPage_ManualPacking.SuspendLayout();
            this.tabPage_PackingOffsetChange.SuspendLayout();
            this.tabPage_ReticlePositionChange.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_visionImageViewer_Lower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_visionImageViewer_Upper)).BeginInit();
            this.baseGroupBox_CycleButton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProbePackingCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWaferVacuumCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxThinChuckVacuumCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxThinChuckDetect)).BeginInit();
            this.tabControl_ProbeCard_ClampType.SuspendLayout();
            this.tabPage_TypeA.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProbeBWDetect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTopCoverDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTopCoverUp)).BeginInit();
            this.tabPage_TypeB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProbeRightClamp_FW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProbeLeftClamp_FW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProbeRightClamp_BW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProbeLeftClamp_BW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProbeUnpackingCyl_Down)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProbeUnpackingCyl_Up)).BeginInit();
            this.baseGroupBox_ManualButton.SuspendLayout();
            this.baseGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // groupBoxStatus
            // 
            this.groupBoxStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.groupBoxStatus.Controls.Add(this.ledStatus_Light_Connected);
            this.groupBoxStatus.Controls.Add(this.lblStatus_Light_Connected);
            this.groupBoxStatus.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxStatus.ForeColor = System.Drawing.Color.White;
            this.groupBoxStatus.Location = new System.Drawing.Point(4, 638);
            this.groupBoxStatus.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBoxStatus.Name = "groupBoxStatus";
            this.groupBoxStatus.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBoxStatus.Size = new System.Drawing.Size(137, 50);
            this.groupBoxStatus.TabIndex = 35;
            this.groupBoxStatus.TabStop = false;
            this.groupBoxStatus.Text = " [ 통신 포트 ] ";
            // 
            // ledStatus_Light_Connected
            // 
            this.ledStatus_Light_Connected.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ledStatus_Light_Connected.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ledStatus_Light_Connected.Image = global::CWA150SA_Onsemi.Properties.Resources.DioRectangleOff;
            this.ledStatus_Light_Connected.Location = new System.Drawing.Point(9, 24);
            this.ledStatus_Light_Connected.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.ledStatus_Light_Connected.Name = "ledStatus_Light_Connected";
            this.ledStatus_Light_Connected.Size = new System.Drawing.Size(16, 16);
            this.ledStatus_Light_Connected.TabIndex = 33;
            this.ledStatus_Light_Connected.UseVisualStyleBackColor = false;
            // 
            // lblStatus_Light_Connected
            // 
            this.lblStatus_Light_Connected.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblStatus_Light_Connected.ForeColor = System.Drawing.Color.White;
            this.lblStatus_Light_Connected.Location = new System.Drawing.Point(29, 24);
            this.lblStatus_Light_Connected.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatus_Light_Connected.Name = "lblStatus_Light_Connected";
            this.lblStatus_Light_Connected.Size = new System.Drawing.Size(100, 18);
            this.lblStatus_Light_Connected.TabIndex = 24;
            this.lblStatus_Light_Connected.Text = "조명 컨트롤러";
            this.lblStatus_Light_Connected.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBoxMainPanel
            // 
            this.groupBoxMainPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.groupBoxMainPanel.Controls.Add(this.pictureBoxMainVacuumCheck);
            this.groupBoxMainPanel.Controls.Add(this.lblMainVacuumCheck);
            this.groupBoxMainPanel.Controls.Add(this.pictureBoxMainAirCheck);
            this.groupBoxMainPanel.Controls.Add(this.lblMainAirCheck);
            this.groupBoxMainPanel.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxMainPanel.ForeColor = System.Drawing.Color.White;
            this.groupBoxMainPanel.Location = new System.Drawing.Point(4, 704);
            this.groupBoxMainPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBoxMainPanel.Name = "groupBoxMainPanel";
            this.groupBoxMainPanel.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBoxMainPanel.Size = new System.Drawing.Size(208, 55);
            this.groupBoxMainPanel.TabIndex = 39;
            this.groupBoxMainPanel.TabStop = false;
            this.groupBoxMainPanel.Text = " [ 공압 ] ";
            // 
            // pictureBoxMainVacuumCheck
            // 
            this.pictureBoxMainVacuumCheck.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
            this.pictureBoxMainVacuumCheck.Location = new System.Drawing.Point(108, 23);
            this.pictureBoxMainVacuumCheck.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBoxMainVacuumCheck.Name = "pictureBoxMainVacuumCheck";
            this.pictureBoxMainVacuumCheck.Size = new System.Drawing.Size(22, 22);
            this.pictureBoxMainVacuumCheck.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxMainVacuumCheck.TabIndex = 31;
            this.pictureBoxMainVacuumCheck.TabStop = false;
            // 
            // lblMainVacuumCheck
            // 
            this.lblMainVacuumCheck.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblMainVacuumCheck.ForeColor = System.Drawing.Color.White;
            this.lblMainVacuumCheck.Location = new System.Drawing.Point(134, 23);
            this.lblMainVacuumCheck.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMainVacuumCheck.Name = "lblMainVacuumCheck";
            this.lblMainVacuumCheck.Size = new System.Drawing.Size(61, 22);
            this.lblMainVacuumCheck.TabIndex = 30;
            this.lblMainVacuumCheck.Text = "메인 진공";
            this.lblMainVacuumCheck.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBoxMainAirCheck
            // 
            this.pictureBoxMainAirCheck.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
            this.pictureBoxMainAirCheck.Location = new System.Drawing.Point(9, 23);
            this.pictureBoxMainAirCheck.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBoxMainAirCheck.Name = "pictureBoxMainAirCheck";
            this.pictureBoxMainAirCheck.Size = new System.Drawing.Size(22, 22);
            this.pictureBoxMainAirCheck.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxMainAirCheck.TabIndex = 27;
            this.pictureBoxMainAirCheck.TabStop = false;
            // 
            // lblMainAirCheck
            // 
            this.lblMainAirCheck.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblMainAirCheck.ForeColor = System.Drawing.Color.White;
            this.lblMainAirCheck.Location = new System.Drawing.Point(35, 23);
            this.lblMainAirCheck.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMainAirCheck.Name = "lblMainAirCheck";
            this.lblMainAirCheck.Size = new System.Drawing.Size(68, 22);
            this.lblMainAirCheck.TabIndex = 13;
            this.lblMainAirCheck.Text = "메인 공압";
            this.lblMainAirCheck.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // timer_DIO_Status
            // 
            this.timer_DIO_Status.Interval = 30;
            this.timer_DIO_Status.Tick += new System.EventHandler(this.timer_DIO_Status_Tick);
            // 
            // pictureBoxTrainImage_Upper
            // 
            this.pictureBoxTrainImage_Upper.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxTrainImage_Upper.Location = new System.Drawing.Point(4, 452);
            this.pictureBoxTrainImage_Upper.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBoxTrainImage_Upper.Name = "pictureBoxTrainImage_Upper";
            this.pictureBoxTrainImage_Upper.Size = new System.Drawing.Size(150, 150);
            this.pictureBoxTrainImage_Upper.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxTrainImage_Upper.TabIndex = 110;
            this.pictureBoxTrainImage_Upper.TabStop = false;
            // 
            // pictureBoxTrainImage_Lower
            // 
            this.pictureBoxTrainImage_Lower.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxTrainImage_Lower.Location = new System.Drawing.Point(515, 452);
            this.pictureBoxTrainImage_Lower.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBoxTrainImage_Lower.Name = "pictureBoxTrainImage_Lower";
            this.pictureBoxTrainImage_Lower.Size = new System.Drawing.Size(150, 150);
            this.pictureBoxTrainImage_Lower.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxTrainImage_Lower.TabIndex = 111;
            this.pictureBoxTrainImage_Lower.TabStop = false;
            // 
            // btnHomeAll
            // 
            this.btnHomeAll.BackColor = System.Drawing.Color.LightGray;
            this.btnHomeAll.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnHomeAll.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnHomeAll.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnHomeAll.Font = new System.Drawing.Font("나눔바른고딕", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHomeAll.ForeColor = System.Drawing.Color.DarkRed;
            this.btnHomeAll.Location = new System.Drawing.Point(1184, 0);
            this.btnHomeAll.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnHomeAll.Name = "btnHomeAll";
            this.btnHomeAll.Size = new System.Drawing.Size(152, 133);
            this.btnHomeAll.TabIndex = 127;
            this.btnHomeAll.Text = "장   비\r\n초기화";
            this.btnHomeAll.UseVisualStyleBackColor = false;
            this.btnHomeAll.Click += new System.EventHandler(this.btnHomeAll_Click);
            // 
            // btnUpperCamera_Init
            // 
            this.btnUpperCamera_Init.BackColor = System.Drawing.Color.LightGray;
            this.btnUpperCamera_Init.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnUpperCamera_Init.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnUpperCamera_Init.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnUpperCamera_Init.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpperCamera_Init.ForeColor = System.Drawing.Color.DarkRed;
            this.btnUpperCamera_Init.Location = new System.Drawing.Point(1033, 0);
            this.btnUpperCamera_Init.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnUpperCamera_Init.Name = "btnUpperCamera_Init";
            this.btnUpperCamera_Init.Size = new System.Drawing.Size(85, 61);
            this.btnUpperCamera_Init.TabIndex = 128;
            this.btnUpperCamera_Init.Text = "카메라\r\n초기화";
            this.btnUpperCamera_Init.UseVisualStyleBackColor = false;
            this.btnUpperCamera_Init.Click += new System.EventHandler(this.btnUpperCamera_Init_Click);
            // 
            // btnUpperCamera_StartLive
            // 
            this.btnUpperCamera_StartLive.BackColor = System.Drawing.Color.LightGreen;
            this.btnUpperCamera_StartLive.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnUpperCamera_StartLive.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnUpperCamera_StartLive.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnUpperCamera_StartLive.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpperCamera_StartLive.ForeColor = System.Drawing.Color.DarkRed;
            this.btnUpperCamera_StartLive.Location = new System.Drawing.Point(1033, 72);
            this.btnUpperCamera_StartLive.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnUpperCamera_StartLive.Name = "btnUpperCamera_StartLive";
            this.btnUpperCamera_StartLive.Size = new System.Drawing.Size(85, 61);
            this.btnUpperCamera_StartLive.TabIndex = 129;
            this.btnUpperCamera_StartLive.Text = "라이브\r\n이미지";
            this.btnUpperCamera_StartLive.UseVisualStyleBackColor = false;
            this.btnUpperCamera_StartLive.Click += new System.EventHandler(this.btnUpperCamera_StartLive_Click);
            // 
            // groupBoxCameraOpticalAxisCheck
            // 
            this.groupBoxCameraOpticalAxisCheck.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.groupBoxCameraOpticalAxisCheck.Controls.Add(this.btnReticlePos_LowerCam_GO);
            this.groupBoxCameraOpticalAxisCheck.Controls.Add(this.btnReticlePos_UpperCam_GO);
            this.groupBoxCameraOpticalAxisCheck.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxCameraOpticalAxisCheck.ForeColor = System.Drawing.Color.White;
            this.groupBoxCameraOpticalAxisCheck.Location = new System.Drawing.Point(574, 638);
            this.groupBoxCameraOpticalAxisCheck.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBoxCameraOpticalAxisCheck.Name = "groupBoxCameraOpticalAxisCheck";
            this.groupBoxCameraOpticalAxisCheck.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBoxCameraOpticalAxisCheck.Size = new System.Drawing.Size(318, 121);
            this.groupBoxCameraOpticalAxisCheck.TabIndex = 183;
            this.groupBoxCameraOpticalAxisCheck.TabStop = false;
            this.groupBoxCameraOpticalAxisCheck.Text = " [ 카메라 광축 확인 ] ";
            // 
            // btnReticlePos_LowerCam_GO
            // 
            this.btnReticlePos_LowerCam_GO.BackColor = System.Drawing.Color.LightGray;
            this.btnReticlePos_LowerCam_GO.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnReticlePos_LowerCam_GO.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnReticlePos_LowerCam_GO.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnReticlePos_LowerCam_GO.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnReticlePos_LowerCam_GO.ForeColor = System.Drawing.Color.DarkRed;
            this.btnReticlePos_LowerCam_GO.Location = new System.Drawing.Point(161, 31);
            this.btnReticlePos_LowerCam_GO.Name = "btnReticlePos_LowerCam_GO";
            this.btnReticlePos_LowerCam_GO.Size = new System.Drawing.Size(146, 79);
            this.btnReticlePos_LowerCam_GO.TabIndex = 184;
            this.btnReticlePos_LowerCam_GO.Text = "[ 하부 카메라 ]\r\n레티클 글래스 센터\r\n확인 위치 이동";
            this.btnReticlePos_LowerCam_GO.UseVisualStyleBackColor = false;
            this.btnReticlePos_LowerCam_GO.Click += new System.EventHandler(this.btnReticlePos_LowerCam_GO_Click);
            // 
            // btnReticlePos_UpperCam_GO
            // 
            this.btnReticlePos_UpperCam_GO.BackColor = System.Drawing.Color.LightGray;
            this.btnReticlePos_UpperCam_GO.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnReticlePos_UpperCam_GO.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnReticlePos_UpperCam_GO.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnReticlePos_UpperCam_GO.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnReticlePos_UpperCam_GO.ForeColor = System.Drawing.Color.DarkRed;
            this.btnReticlePos_UpperCam_GO.Location = new System.Drawing.Point(11, 31);
            this.btnReticlePos_UpperCam_GO.Name = "btnReticlePos_UpperCam_GO";
            this.btnReticlePos_UpperCam_GO.Size = new System.Drawing.Size(146, 79);
            this.btnReticlePos_UpperCam_GO.TabIndex = 183;
            this.btnReticlePos_UpperCam_GO.Text = "[ 상부 카메라 ]\r\n레티클 글래스 센터\r\n확인 위치 이동";
            this.btnReticlePos_UpperCam_GO.UseVisualStyleBackColor = false;
            this.btnReticlePos_UpperCam_GO.Click += new System.EventHandler(this.btnReticlePos_UpperCam_GO_Click);
            // 
            // groupBoxAlignCheckPosParameter
            // 
            this.groupBoxAlignCheckPosParameter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseButton_EmptyChip_XYPos_GO);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabel8);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseButton_CameraY_GoPos_Safety);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabel19);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabel17);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabel15);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabel14);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabel12);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabel11);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabel16);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.lblLowerCamera_Bot_ErrorData_Y);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.lblLowerCamera_Bot_ErrorData_X);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabel20);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabel10);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.lblLowerCamera_Mid_ErrorData_Y);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.lblLowerCamera_Mid_ErrorData_X);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabel13);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabel_LowerCam);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabel_Delta_Y);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.lblLowerCamera_Top_ErrorData_Y);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.lblLowerCamera_Top_ErrorData_X);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabel_Delta_X);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabel9);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabel_OK);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabelPosition_Bot);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabelPosition_Mid);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabelPosition_Top);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseButton_Y_Pos_GO3);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseButton_Y_Pos_GO2);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseButton_Y_Pos_GO1);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabel18);
            this.groupBoxAlignCheckPosParameter.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxAlignCheckPosParameter.ForeColor = System.Drawing.Color.White;
            this.groupBoxAlignCheckPosParameter.Location = new System.Drawing.Point(1542, 160);
            this.groupBoxAlignCheckPosParameter.Name = "groupBoxAlignCheckPosParameter";
            this.groupBoxAlignCheckPosParameter.Size = new System.Drawing.Size(362, 280);
            this.groupBoxAlignCheckPosParameter.TabIndex = 188;
            this.groupBoxAlignCheckPosParameter.TabStop = false;
            this.groupBoxAlignCheckPosParameter.Text = " [ 얼라인 위치 확인 ] ";
            // 
            // baseButton_EmptyChip_XYPos_GO
            // 
            this.baseButton_EmptyChip_XYPos_GO.BackColor = System.Drawing.Color.LightBlue;
            this.baseButton_EmptyChip_XYPos_GO.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButton_EmptyChip_XYPos_GO.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButton_EmptyChip_XYPos_GO.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButton_EmptyChip_XYPos_GO.Location = new System.Drawing.Point(135, 133);
            this.baseButton_EmptyChip_XYPos_GO.Name = "baseButton_EmptyChip_XYPos_GO";
            this.baseButton_EmptyChip_XYPos_GO.Size = new System.Drawing.Size(58, 79);
            this.baseButton_EmptyChip_XYPos_GO.TabIndex = 211;
            this.baseButton_EmptyChip_XYPos_GO.Text = "Empty\r\nChip\r\n위치\r\n□";
            this.baseButton_EmptyChip_XYPos_GO.UseVisualStyleBackColor = false;
            this.baseButton_EmptyChip_XYPos_GO.Click += new System.EventHandler(this.baseButton_EmptyChip_XYPos_GO_Click);
            // 
            // baseLabel8
            // 
            this.baseLabel8.AutoSize = true;
            this.baseLabel8.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel8.ForeColor = System.Drawing.Color.White;
            this.baseLabel8.Location = new System.Drawing.Point(6, 28);
            this.baseLabel8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel8.Name = "baseLabel8";
            this.baseLabel8.Size = new System.Drawing.Size(37, 38);
            this.baseLabel8.TabIndex = 210;
            this.baseLabel8.Text = "판정\r\n결과";
            // 
            // baseButton_CameraY_GoPos_Safety
            // 
            this.baseButton_CameraY_GoPos_Safety.BackColor = System.Drawing.Color.LightGray;
            this.baseButton_CameraY_GoPos_Safety.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButton_CameraY_GoPos_Safety.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButton_CameraY_GoPos_Safety.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButton_CameraY_GoPos_Safety.Location = new System.Drawing.Point(88, 27);
            this.baseButton_CameraY_GoPos_Safety.Name = "baseButton_CameraY_GoPos_Safety";
            this.baseButton_CameraY_GoPos_Safety.Size = new System.Drawing.Size(113, 40);
            this.baseButton_CameraY_GoPos_Safety.TabIndex = 209;
            this.baseButton_CameraY_GoPos_Safety.Text = "카메라 안전 위치\r\n이동";
            this.baseButton_CameraY_GoPos_Safety.UseVisualStyleBackColor = false;
            this.baseButton_CameraY_GoPos_Safety.Click += new System.EventHandler(this.baseButton_CameraY_GoPos_Safety_Click);
            // 
            // baseLabel19
            // 
            this.baseLabel19.BackColor = System.Drawing.Color.Black;
            this.baseLabel19.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabel19.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel19.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel19.Location = new System.Drawing.Point(318, 240);
            this.baseLabel19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel19.Name = "baseLabel19";
            this.baseLabel19.Size = new System.Drawing.Size(34, 28);
            this.baseLabel19.TabIndex = 208;
            this.baseLabel19.Text = "mm";
            this.baseLabel19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel17
            // 
            this.baseLabel17.BackColor = System.Drawing.Color.Black;
            this.baseLabel17.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabel17.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel17.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel17.Location = new System.Drawing.Point(318, 209);
            this.baseLabel17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel17.Name = "baseLabel17";
            this.baseLabel17.Size = new System.Drawing.Size(34, 28);
            this.baseLabel17.TabIndex = 208;
            this.baseLabel17.Text = "mm";
            this.baseLabel17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel15
            // 
            this.baseLabel15.BackColor = System.Drawing.Color.Black;
            this.baseLabel15.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabel15.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel15.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel15.Location = new System.Drawing.Point(318, 174);
            this.baseLabel15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel15.Name = "baseLabel15";
            this.baseLabel15.Size = new System.Drawing.Size(34, 28);
            this.baseLabel15.TabIndex = 208;
            this.baseLabel15.Text = "mm";
            this.baseLabel15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel14
            // 
            this.baseLabel14.BackColor = System.Drawing.Color.Black;
            this.baseLabel14.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabel14.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel14.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel14.Location = new System.Drawing.Point(318, 143);
            this.baseLabel14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel14.Name = "baseLabel14";
            this.baseLabel14.Size = new System.Drawing.Size(34, 28);
            this.baseLabel14.TabIndex = 208;
            this.baseLabel14.Text = "mm";
            this.baseLabel14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel12
            // 
            this.baseLabel12.BackColor = System.Drawing.Color.Black;
            this.baseLabel12.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabel12.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel12.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel12.Location = new System.Drawing.Point(318, 108);
            this.baseLabel12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel12.Name = "baseLabel12";
            this.baseLabel12.Size = new System.Drawing.Size(34, 28);
            this.baseLabel12.TabIndex = 208;
            this.baseLabel12.Text = "mm";
            this.baseLabel12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel11
            // 
            this.baseLabel11.BackColor = System.Drawing.Color.Black;
            this.baseLabel11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabel11.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel11.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel11.Location = new System.Drawing.Point(318, 77);
            this.baseLabel11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel11.Name = "baseLabel11";
            this.baseLabel11.Size = new System.Drawing.Size(34, 28);
            this.baseLabel11.TabIndex = 208;
            this.baseLabel11.Text = "mm";
            this.baseLabel11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel16
            // 
            this.baseLabel16.BackColor = System.Drawing.Color.Black;
            this.baseLabel16.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel16.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel16.Location = new System.Drawing.Point(206, 240);
            this.baseLabel16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel16.Name = "baseLabel16";
            this.baseLabel16.Size = new System.Drawing.Size(21, 28);
            this.baseLabel16.TabIndex = 207;
            this.baseLabel16.Text = "Y";
            this.baseLabel16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLowerCamera_Bot_ErrorData_Y
            // 
            this.lblLowerCamera_Bot_ErrorData_Y.BackColor = System.Drawing.Color.Cornsilk;
            this.lblLowerCamera_Bot_ErrorData_Y.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblLowerCamera_Bot_ErrorData_Y.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblLowerCamera_Bot_ErrorData_Y.ForeColor = System.Drawing.Color.Black;
            this.lblLowerCamera_Bot_ErrorData_Y.Location = new System.Drawing.Point(229, 240);
            this.lblLowerCamera_Bot_ErrorData_Y.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLowerCamera_Bot_ErrorData_Y.Name = "lblLowerCamera_Bot_ErrorData_Y";
            this.lblLowerCamera_Bot_ErrorData_Y.Size = new System.Drawing.Size(88, 28);
            this.lblLowerCamera_Bot_ErrorData_Y.TabIndex = 206;
            this.lblLowerCamera_Bot_ErrorData_Y.Text = "0.1234";
            this.lblLowerCamera_Bot_ErrorData_Y.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLowerCamera_Bot_ErrorData_X
            // 
            this.lblLowerCamera_Bot_ErrorData_X.BackColor = System.Drawing.Color.Cornsilk;
            this.lblLowerCamera_Bot_ErrorData_X.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblLowerCamera_Bot_ErrorData_X.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblLowerCamera_Bot_ErrorData_X.ForeColor = System.Drawing.Color.Black;
            this.lblLowerCamera_Bot_ErrorData_X.Location = new System.Drawing.Point(229, 209);
            this.lblLowerCamera_Bot_ErrorData_X.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLowerCamera_Bot_ErrorData_X.Name = "lblLowerCamera_Bot_ErrorData_X";
            this.lblLowerCamera_Bot_ErrorData_X.Size = new System.Drawing.Size(88, 28);
            this.lblLowerCamera_Bot_ErrorData_X.TabIndex = 205;
            this.lblLowerCamera_Bot_ErrorData_X.Text = "0.1234";
            this.lblLowerCamera_Bot_ErrorData_X.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel20
            // 
            this.baseLabel20.BackColor = System.Drawing.Color.Black;
            this.baseLabel20.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel20.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel20.Location = new System.Drawing.Point(206, 209);
            this.baseLabel20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel20.Name = "baseLabel20";
            this.baseLabel20.Size = new System.Drawing.Size(21, 28);
            this.baseLabel20.TabIndex = 204;
            this.baseLabel20.Text = "X";
            this.baseLabel20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel10
            // 
            this.baseLabel10.BackColor = System.Drawing.Color.Black;
            this.baseLabel10.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel10.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel10.Location = new System.Drawing.Point(206, 174);
            this.baseLabel10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel10.Name = "baseLabel10";
            this.baseLabel10.Size = new System.Drawing.Size(21, 28);
            this.baseLabel10.TabIndex = 201;
            this.baseLabel10.Text = "Y";
            this.baseLabel10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLowerCamera_Mid_ErrorData_Y
            // 
            this.lblLowerCamera_Mid_ErrorData_Y.BackColor = System.Drawing.Color.Cornsilk;
            this.lblLowerCamera_Mid_ErrorData_Y.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblLowerCamera_Mid_ErrorData_Y.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblLowerCamera_Mid_ErrorData_Y.ForeColor = System.Drawing.Color.Black;
            this.lblLowerCamera_Mid_ErrorData_Y.Location = new System.Drawing.Point(229, 174);
            this.lblLowerCamera_Mid_ErrorData_Y.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLowerCamera_Mid_ErrorData_Y.Name = "lblLowerCamera_Mid_ErrorData_Y";
            this.lblLowerCamera_Mid_ErrorData_Y.Size = new System.Drawing.Size(88, 28);
            this.lblLowerCamera_Mid_ErrorData_Y.TabIndex = 200;
            this.lblLowerCamera_Mid_ErrorData_Y.Text = "0.1234";
            this.lblLowerCamera_Mid_ErrorData_Y.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLowerCamera_Mid_ErrorData_X
            // 
            this.lblLowerCamera_Mid_ErrorData_X.BackColor = System.Drawing.Color.Cornsilk;
            this.lblLowerCamera_Mid_ErrorData_X.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblLowerCamera_Mid_ErrorData_X.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblLowerCamera_Mid_ErrorData_X.ForeColor = System.Drawing.Color.Black;
            this.lblLowerCamera_Mid_ErrorData_X.Location = new System.Drawing.Point(229, 143);
            this.lblLowerCamera_Mid_ErrorData_X.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLowerCamera_Mid_ErrorData_X.Name = "lblLowerCamera_Mid_ErrorData_X";
            this.lblLowerCamera_Mid_ErrorData_X.Size = new System.Drawing.Size(88, 28);
            this.lblLowerCamera_Mid_ErrorData_X.TabIndex = 199;
            this.lblLowerCamera_Mid_ErrorData_X.Text = "0.1234";
            this.lblLowerCamera_Mid_ErrorData_X.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel13
            // 
            this.baseLabel13.BackColor = System.Drawing.Color.Black;
            this.baseLabel13.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel13.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel13.Location = new System.Drawing.Point(206, 143);
            this.baseLabel13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel13.Name = "baseLabel13";
            this.baseLabel13.Size = new System.Drawing.Size(21, 28);
            this.baseLabel13.TabIndex = 198;
            this.baseLabel13.Text = "X";
            this.baseLabel13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel_LowerCam
            // 
            this.baseLabel_LowerCam.BackColor = System.Drawing.Color.Black;
            this.baseLabel_LowerCam.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabel_LowerCam.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_LowerCam.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel_LowerCam.Location = new System.Drawing.Point(229, 27);
            this.baseLabel_LowerCam.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LowerCam.Name = "baseLabel_LowerCam";
            this.baseLabel_LowerCam.Size = new System.Drawing.Size(123, 43);
            this.baseLabel_LowerCam.TabIndex = 195;
            this.baseLabel_LowerCam.Text = "프로브 핀 위치 기준\r\n웨이퍼 위치 편차";
            this.baseLabel_LowerCam.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel_Delta_Y
            // 
            this.baseLabel_Delta_Y.BackColor = System.Drawing.Color.Black;
            this.baseLabel_Delta_Y.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_Delta_Y.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel_Delta_Y.Location = new System.Drawing.Point(206, 108);
            this.baseLabel_Delta_Y.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Delta_Y.Name = "baseLabel_Delta_Y";
            this.baseLabel_Delta_Y.Size = new System.Drawing.Size(21, 28);
            this.baseLabel_Delta_Y.TabIndex = 193;
            this.baseLabel_Delta_Y.Text = "Y";
            this.baseLabel_Delta_Y.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLowerCamera_Top_ErrorData_Y
            // 
            this.lblLowerCamera_Top_ErrorData_Y.BackColor = System.Drawing.Color.Cornsilk;
            this.lblLowerCamera_Top_ErrorData_Y.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblLowerCamera_Top_ErrorData_Y.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblLowerCamera_Top_ErrorData_Y.ForeColor = System.Drawing.Color.Black;
            this.lblLowerCamera_Top_ErrorData_Y.Location = new System.Drawing.Point(229, 108);
            this.lblLowerCamera_Top_ErrorData_Y.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLowerCamera_Top_ErrorData_Y.Name = "lblLowerCamera_Top_ErrorData_Y";
            this.lblLowerCamera_Top_ErrorData_Y.Size = new System.Drawing.Size(88, 28);
            this.lblLowerCamera_Top_ErrorData_Y.TabIndex = 192;
            this.lblLowerCamera_Top_ErrorData_Y.Text = "0.1234";
            this.lblLowerCamera_Top_ErrorData_Y.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLowerCamera_Top_ErrorData_X
            // 
            this.lblLowerCamera_Top_ErrorData_X.BackColor = System.Drawing.Color.Cornsilk;
            this.lblLowerCamera_Top_ErrorData_X.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblLowerCamera_Top_ErrorData_X.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblLowerCamera_Top_ErrorData_X.ForeColor = System.Drawing.Color.Black;
            this.lblLowerCamera_Top_ErrorData_X.Location = new System.Drawing.Point(229, 77);
            this.lblLowerCamera_Top_ErrorData_X.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLowerCamera_Top_ErrorData_X.Name = "lblLowerCamera_Top_ErrorData_X";
            this.lblLowerCamera_Top_ErrorData_X.Size = new System.Drawing.Size(88, 28);
            this.lblLowerCamera_Top_ErrorData_X.TabIndex = 191;
            this.lblLowerCamera_Top_ErrorData_X.Text = "0.1234";
            this.lblLowerCamera_Top_ErrorData_X.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel_Delta_X
            // 
            this.baseLabel_Delta_X.BackColor = System.Drawing.Color.Black;
            this.baseLabel_Delta_X.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_Delta_X.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel_Delta_X.Location = new System.Drawing.Point(206, 77);
            this.baseLabel_Delta_X.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Delta_X.Name = "baseLabel_Delta_X";
            this.baseLabel_Delta_X.Size = new System.Drawing.Size(21, 28);
            this.baseLabel_Delta_X.TabIndex = 190;
            this.baseLabel_Delta_X.Text = "X";
            this.baseLabel_Delta_X.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel9
            // 
            this.baseLabel9.BackColor = System.Drawing.Color.Red;
            this.baseLabel9.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel9.ForeColor = System.Drawing.Color.White;
            this.baseLabel9.Location = new System.Drawing.Point(43, 47);
            this.baseLabel9.Name = "baseLabel9";
            this.baseLabel9.Size = new System.Drawing.Size(33, 19);
            this.baseLabel9.TabIndex = 146;
            this.baseLabel9.Text = "NG";
            this.baseLabel9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel_OK
            // 
            this.baseLabel_OK.BackColor = System.Drawing.Color.Lime;
            this.baseLabel_OK.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_OK.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_OK.Location = new System.Drawing.Point(43, 28);
            this.baseLabel_OK.Name = "baseLabel_OK";
            this.baseLabel_OK.Size = new System.Drawing.Size(33, 19);
            this.baseLabel_OK.TabIndex = 145;
            this.baseLabel_OK.Text = "OK";
            this.baseLabel_OK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabelPosition_Bot
            // 
            this.baseLabelPosition_Bot.BackColor = System.Drawing.Color.Black;
            this.baseLabelPosition_Bot.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabelPosition_Bot.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelPosition_Bot.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabelPosition_Bot.Location = new System.Drawing.Point(10, 209);
            this.baseLabelPosition_Bot.Name = "baseLabelPosition_Bot";
            this.baseLabelPosition_Bot.Size = new System.Drawing.Size(40, 59);
            this.baseLabelPosition_Bot.TabIndex = 143;
            this.baseLabelPosition_Bot.Text = "BOT";
            this.baseLabelPosition_Bot.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.baseLabelPosition_Bot.Click += new System.EventHandler(this.baseLabelPosition_Bot_Click);
            // 
            // baseLabelPosition_Mid
            // 
            this.baseLabelPosition_Mid.BackColor = System.Drawing.Color.Black;
            this.baseLabelPosition_Mid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabelPosition_Mid.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelPosition_Mid.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabelPosition_Mid.Location = new System.Drawing.Point(10, 143);
            this.baseLabelPosition_Mid.Name = "baseLabelPosition_Mid";
            this.baseLabelPosition_Mid.Size = new System.Drawing.Size(40, 59);
            this.baseLabelPosition_Mid.TabIndex = 143;
            this.baseLabelPosition_Mid.Text = "MID";
            this.baseLabelPosition_Mid.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.baseLabelPosition_Mid.Click += new System.EventHandler(this.baseLabelPosition_Mid_Click);
            // 
            // baseLabelPosition_Top
            // 
            this.baseLabelPosition_Top.BackColor = System.Drawing.Color.DodgerBlue;
            this.baseLabelPosition_Top.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabelPosition_Top.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelPosition_Top.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabelPosition_Top.Location = new System.Drawing.Point(10, 77);
            this.baseLabelPosition_Top.Name = "baseLabelPosition_Top";
            this.baseLabelPosition_Top.Size = new System.Drawing.Size(40, 59);
            this.baseLabelPosition_Top.TabIndex = 143;
            this.baseLabelPosition_Top.Text = "TOP";
            this.baseLabelPosition_Top.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.baseLabelPosition_Top.Click += new System.EventHandler(this.baseLabelPosition_Top_Click);
            // 
            // baseButton_Y_Pos_GO3
            // 
            this.baseButton_Y_Pos_GO3.BackColor = System.Drawing.Color.LightGray;
            this.baseButton_Y_Pos_GO3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButton_Y_Pos_GO3.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButton_Y_Pos_GO3.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButton_Y_Pos_GO3.Location = new System.Drawing.Point(77, 220);
            this.baseButton_Y_Pos_GO3.Name = "baseButton_Y_Pos_GO3";
            this.baseButton_Y_Pos_GO3.Size = new System.Drawing.Size(116, 49);
            this.baseButton_Y_Pos_GO3.TabIndex = 122;
            this.baseButton_Y_Pos_GO3.Text = "BOT 위치  ▼";
            this.baseButton_Y_Pos_GO3.UseVisualStyleBackColor = false;
            this.baseButton_Y_Pos_GO3.Click += new System.EventHandler(this.baseButton_Y_Pos_GO3_Click);
            // 
            // baseButton_Y_Pos_GO2
            // 
            this.baseButton_Y_Pos_GO2.BackColor = System.Drawing.Color.LightGray;
            this.baseButton_Y_Pos_GO2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButton_Y_Pos_GO2.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButton_Y_Pos_GO2.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButton_Y_Pos_GO2.Location = new System.Drawing.Point(77, 133);
            this.baseButton_Y_Pos_GO2.Name = "baseButton_Y_Pos_GO2";
            this.baseButton_Y_Pos_GO2.Size = new System.Drawing.Size(56, 79);
            this.baseButton_Y_Pos_GO2.TabIndex = 117;
            this.baseButton_Y_Pos_GO2.Text = "MID\r\n위치\r\n▣";
            this.baseButton_Y_Pos_GO2.UseVisualStyleBackColor = false;
            this.baseButton_Y_Pos_GO2.Click += new System.EventHandler(this.baseButton_Y_Pos_GO2_Click);
            // 
            // baseButton_Y_Pos_GO1
            // 
            this.baseButton_Y_Pos_GO1.BackColor = System.Drawing.Color.LightGray;
            this.baseButton_Y_Pos_GO1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButton_Y_Pos_GO1.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButton_Y_Pos_GO1.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButton_Y_Pos_GO1.Location = new System.Drawing.Point(77, 76);
            this.baseButton_Y_Pos_GO1.Name = "baseButton_Y_Pos_GO1";
            this.baseButton_Y_Pos_GO1.Size = new System.Drawing.Size(116, 49);
            this.baseButton_Y_Pos_GO1.TabIndex = 112;
            this.baseButton_Y_Pos_GO1.Text = "TOP 위치  ▲";
            this.baseButton_Y_Pos_GO1.UseVisualStyleBackColor = false;
            this.baseButton_Y_Pos_GO1.Click += new System.EventHandler(this.baseButton_Y_Pos_GO1_Click);
            // 
            // baseLabel18
            // 
            this.baseLabel18.BackColor = System.Drawing.Color.White;
            this.baseLabel18.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel18.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel18.ForeColor = System.Drawing.Color.Black;
            this.baseLabel18.Location = new System.Drawing.Point(52, 76);
            this.baseLabel18.Name = "baseLabel18";
            this.baseLabel18.Size = new System.Drawing.Size(23, 193);
            this.baseLabel18.TabIndex = 109;
            this.baseLabel18.Text = "▲\r\n▲\r\n▲\r\n\r\n\r\n▼\r\n▼\r\n▼";
            this.baseLabel18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabControl_User
            // 
            this.tabControl_User.Controls.Add(this.tabPage_ManualPacking);
            this.tabControl_User.Controls.Add(this.tabPage_PackingOffsetChange);
            this.tabControl_User.Controls.Add(this.tabPage_ReticlePositionChange);
            this.tabControl_User.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tabControl_User.ItemSize = new System.Drawing.Size(102, 34);
            this.tabControl_User.Location = new System.Drawing.Point(1033, 165);
            this.tabControl_User.Name = "tabControl_User";
            this.tabControl_User.SelectedIndex = 0;
            this.tabControl_User.Size = new System.Drawing.Size(484, 275);
            this.tabControl_User.TabIndex = 189;
            this.tabControl_User.SelectedIndexChanged += new System.EventHandler(this.tabControl_User_SelectedIndexChanged);
            // 
            // tabPage_ManualPacking
            // 
            this.tabPage_ManualPacking.Controls.Add(this.baseLabel_PitchMove);
            this.tabPage_ManualPacking.Controls.Add(this.baseLabel_JogMove);
            this.tabPage_ManualPacking.Controls.Add(this.baseButton_Elev_JogMove_Down);
            this.tabPage_ManualPacking.Controls.Add(this.baseButton_Elev_JogMove_Up);
            this.tabPage_ManualPacking.Controls.Add(this.baseButton_Elev_PitchMove_Down);
            this.tabPage_ManualPacking.Controls.Add(this.baseButton_Elev_PitchMove_Up);
            this.tabPage_ManualPacking.Controls.Add(this.baseButton_Elev_GoPos_Wafer_ProbeCard_Packing);
            this.tabPage_ManualPacking.Controls.Add(this.btnManualPacking);
            this.tabPage_ManualPacking.Controls.Add(this.baseLabelManualPacking3);
            this.tabPage_ManualPacking.Controls.Add(this.baseLabelManualPacking3_Title);
            this.tabPage_ManualPacking.Controls.Add(this.baseLabelManualPacking2);
            this.tabPage_ManualPacking.Controls.Add(this.baseLabelManualPacking2_Title);
            this.tabPage_ManualPacking.Controls.Add(this.baseLabelManualPacking1_Title);
            this.tabPage_ManualPacking.Controls.Add(this.baseLabelManualPacking1);
            this.tabPage_ManualPacking.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tabPage_ManualPacking.Location = new System.Drawing.Point(4, 38);
            this.tabPage_ManualPacking.Name = "tabPage_ManualPacking";
            this.tabPage_ManualPacking.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_ManualPacking.Size = new System.Drawing.Size(476, 233);
            this.tabPage_ManualPacking.TabIndex = 0;
            this.tabPage_ManualPacking.Text = "    수동 패킹  ";
            this.tabPage_ManualPacking.UseVisualStyleBackColor = true;
            // 
            // baseLabel_PitchMove
            // 
            this.baseLabel_PitchMove.BackColor = System.Drawing.Color.Black;
            this.baseLabel_PitchMove.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabel_PitchMove.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_PitchMove.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel_PitchMove.Location = new System.Drawing.Point(245, 159);
            this.baseLabel_PitchMove.Name = "baseLabel_PitchMove";
            this.baseLabel_PitchMove.Size = new System.Drawing.Size(65, 22);
            this.baseLabel_PitchMove.TabIndex = 204;
            this.baseLabel_PitchMove.Text = "피치 이동";
            this.baseLabel_PitchMove.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel_JogMove
            // 
            this.baseLabel_JogMove.BackColor = System.Drawing.Color.Black;
            this.baseLabel_JogMove.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabel_JogMove.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_JogMove.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel_JogMove.Location = new System.Drawing.Point(167, 159);
            this.baseLabel_JogMove.Name = "baseLabel_JogMove";
            this.baseLabel_JogMove.Size = new System.Drawing.Size(65, 22);
            this.baseLabel_JogMove.TabIndex = 203;
            this.baseLabel_JogMove.Text = "조그 이동";
            this.baseLabel_JogMove.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseButton_Elev_JogMove_Down
            // 
            this.baseButton_Elev_JogMove_Down.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.baseButton_Elev_JogMove_Down.Image = global::CWA150SA_Onsemi.Properties.Resources.Down;
            this.baseButton_Elev_JogMove_Down.Location = new System.Drawing.Point(164, 182);
            this.baseButton_Elev_JogMove_Down.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseButton_Elev_JogMove_Down.Name = "baseButton_Elev_JogMove_Down";
            this.baseButton_Elev_JogMove_Down.Size = new System.Drawing.Size(70, 47);
            this.baseButton_Elev_JogMove_Down.TabIndex = 201;
            this.baseButton_Elev_JogMove_Down.UseVisualStyleBackColor = false;
            this.baseButton_Elev_JogMove_Down.MouseDown += new System.Windows.Forms.MouseEventHandler(this.baseButton_Elev_JogMove_Down_MouseDown);
            this.baseButton_Elev_JogMove_Down.MouseUp += new System.Windows.Forms.MouseEventHandler(this.baseButton_Elev_JogMove_Up_MouseUp);
            // 
            // baseButton_Elev_JogMove_Up
            // 
            this.baseButton_Elev_JogMove_Up.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.baseButton_Elev_JogMove_Up.Image = global::CWA150SA_Onsemi.Properties.Resources.Up;
            this.baseButton_Elev_JogMove_Up.Location = new System.Drawing.Point(164, 110);
            this.baseButton_Elev_JogMove_Up.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseButton_Elev_JogMove_Up.Name = "baseButton_Elev_JogMove_Up";
            this.baseButton_Elev_JogMove_Up.Size = new System.Drawing.Size(70, 47);
            this.baseButton_Elev_JogMove_Up.TabIndex = 202;
            this.baseButton_Elev_JogMove_Up.UseVisualStyleBackColor = false;
            this.baseButton_Elev_JogMove_Up.MouseDown += new System.Windows.Forms.MouseEventHandler(this.baseButton_Elev_JogMove_Up_MouseDown);
            this.baseButton_Elev_JogMove_Up.MouseUp += new System.Windows.Forms.MouseEventHandler(this.baseButton_Elev_JogMove_Up_MouseUp);
            // 
            // baseButton_Elev_PitchMove_Down
            // 
            this.baseButton_Elev_PitchMove_Down.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.baseButton_Elev_PitchMove_Down.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.baseButton_Elev_PitchMove_Down.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.baseButton_Elev_PitchMove_Down.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.baseButton_Elev_PitchMove_Down.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.baseButton_Elev_PitchMove_Down.ForeColor = System.Drawing.Color.White;
            this.baseButton_Elev_PitchMove_Down.Location = new System.Drawing.Point(242, 182);
            this.baseButton_Elev_PitchMove_Down.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseButton_Elev_PitchMove_Down.Name = "baseButton_Elev_PitchMove_Down";
            this.baseButton_Elev_PitchMove_Down.Size = new System.Drawing.Size(70, 47);
            this.baseButton_Elev_PitchMove_Down.TabIndex = 200;
            this.baseButton_Elev_PitchMove_Down.Text = "0.1mm\r\n▼";
            this.baseButton_Elev_PitchMove_Down.UseVisualStyleBackColor = false;
            this.baseButton_Elev_PitchMove_Down.Click += new System.EventHandler(this.baseButton_Elev_PitchMove_Down_Click);
            // 
            // baseButton_Elev_PitchMove_Up
            // 
            this.baseButton_Elev_PitchMove_Up.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.baseButton_Elev_PitchMove_Up.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.baseButton_Elev_PitchMove_Up.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.baseButton_Elev_PitchMove_Up.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.baseButton_Elev_PitchMove_Up.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.baseButton_Elev_PitchMove_Up.ForeColor = System.Drawing.Color.White;
            this.baseButton_Elev_PitchMove_Up.Location = new System.Drawing.Point(242, 110);
            this.baseButton_Elev_PitchMove_Up.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseButton_Elev_PitchMove_Up.Name = "baseButton_Elev_PitchMove_Up";
            this.baseButton_Elev_PitchMove_Up.Size = new System.Drawing.Size(70, 47);
            this.baseButton_Elev_PitchMove_Up.TabIndex = 199;
            this.baseButton_Elev_PitchMove_Up.Text = "▲\r\n0.1mm";
            this.baseButton_Elev_PitchMove_Up.UseVisualStyleBackColor = false;
            this.baseButton_Elev_PitchMove_Up.Click += new System.EventHandler(this.baseButton_Elev_PitchMove_Up_Click);
            // 
            // baseButton_Elev_GoPos_Wafer_ProbeCard_Packing
            // 
            this.baseButton_Elev_GoPos_Wafer_ProbeCard_Packing.BackColor = System.Drawing.Color.LightGray;
            this.baseButton_Elev_GoPos_Wafer_ProbeCard_Packing.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.baseButton_Elev_GoPos_Wafer_ProbeCard_Packing.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.baseButton_Elev_GoPos_Wafer_ProbeCard_Packing.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.baseButton_Elev_GoPos_Wafer_ProbeCard_Packing.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold);
            this.baseButton_Elev_GoPos_Wafer_ProbeCard_Packing.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButton_Elev_GoPos_Wafer_ProbeCard_Packing.Location = new System.Drawing.Point(3, 145);
            this.baseButton_Elev_GoPos_Wafer_ProbeCard_Packing.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseButton_Elev_GoPos_Wafer_ProbeCard_Packing.Name = "baseButton_Elev_GoPos_Wafer_ProbeCard_Packing";
            this.baseButton_Elev_GoPos_Wafer_ProbeCard_Packing.Size = new System.Drawing.Size(148, 84);
            this.baseButton_Elev_GoPos_Wafer_ProbeCard_Packing.TabIndex = 196;
            this.baseButton_Elev_GoPos_Wafer_ProbeCard_Packing.Text = "패킹 높이 하단까지\r\n씬-척 엘리베이터\r\n이동";
            this.baseButton_Elev_GoPos_Wafer_ProbeCard_Packing.UseVisualStyleBackColor = false;
            this.baseButton_Elev_GoPos_Wafer_ProbeCard_Packing.Click += new System.EventHandler(this.baseButton_Elev_GoPos_Wafer_ProbeCard_Packing_Click);
            // 
            // btnManualPacking
            // 
            this.btnManualPacking.BackColor = System.Drawing.Color.LightGray;
            this.btnManualPacking.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnManualPacking.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnManualPacking.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnManualPacking.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnManualPacking.ForeColor = System.Drawing.Color.DarkRed;
            this.btnManualPacking.Location = new System.Drawing.Point(326, 110);
            this.btnManualPacking.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnManualPacking.Name = "btnManualPacking";
            this.btnManualPacking.Size = new System.Drawing.Size(149, 119);
            this.btnManualPacking.TabIndex = 195;
            this.btnManualPacking.Text = "[ 프로브  카드 ]\r\n[ 웨이퍼 ]\r\n\r\n패킹 시작\r\n";
            this.btnManualPacking.UseVisualStyleBackColor = false;
            this.btnManualPacking.Click += new System.EventHandler(this.btnManualPacking_Click);
            // 
            // baseLabelManualPacking3
            // 
            this.baseLabelManualPacking3.BackColor = System.Drawing.Color.Cornsilk;
            this.baseLabelManualPacking3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabelManualPacking3.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelManualPacking3.ForeColor = System.Drawing.Color.Black;
            this.baseLabelManualPacking3.Location = new System.Drawing.Point(326, 38);
            this.baseLabelManualPacking3.Name = "baseLabelManualPacking3";
            this.baseLabelManualPacking3.Size = new System.Drawing.Size(148, 69);
            this.baseLabelManualPacking3.TabIndex = 192;
            this.baseLabelManualPacking3.Text = "패킹 작업 진행";
            this.baseLabelManualPacking3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabelManualPacking3_Title
            // 
            this.baseLabelManualPacking3_Title.BackColor = System.Drawing.Color.Black;
            this.baseLabelManualPacking3_Title.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabelManualPacking3_Title.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelManualPacking3_Title.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabelManualPacking3_Title.Location = new System.Drawing.Point(326, 8);
            this.baseLabelManualPacking3_Title.Name = "baseLabelManualPacking3_Title";
            this.baseLabelManualPacking3_Title.Size = new System.Drawing.Size(149, 30);
            this.baseLabelManualPacking3_Title.TabIndex = 191;
            this.baseLabelManualPacking3_Title.Text = "[            3   단계            ]";
            this.baseLabelManualPacking3_Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabelManualPacking2
            // 
            this.baseLabelManualPacking2.BackColor = System.Drawing.Color.Cornsilk;
            this.baseLabelManualPacking2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabelManualPacking2.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelManualPacking2.ForeColor = System.Drawing.Color.Black;
            this.baseLabelManualPacking2.Location = new System.Drawing.Point(164, 38);
            this.baseLabelManualPacking2.Name = "baseLabelManualPacking2";
            this.baseLabelManualPacking2.Size = new System.Drawing.Size(148, 69);
            this.baseLabelManualPacking2.TabIndex = 188;
            this.baseLabelManualPacking2.Text = "씬 - 척 의 고무패드가 \r\n프로브 카드에 접촉하는\r\n높이까지 천천히 올림";
            this.baseLabelManualPacking2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabelManualPacking2_Title
            // 
            this.baseLabelManualPacking2_Title.BackColor = System.Drawing.Color.Black;
            this.baseLabelManualPacking2_Title.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabelManualPacking2_Title.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelManualPacking2_Title.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabelManualPacking2_Title.Location = new System.Drawing.Point(164, 8);
            this.baseLabelManualPacking2_Title.Name = "baseLabelManualPacking2_Title";
            this.baseLabelManualPacking2_Title.Size = new System.Drawing.Size(149, 30);
            this.baseLabelManualPacking2_Title.TabIndex = 187;
            this.baseLabelManualPacking2_Title.Text = "[            2   단계            ]";
            this.baseLabelManualPacking2_Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabelManualPacking1_Title
            // 
            this.baseLabelManualPacking1_Title.BackColor = System.Drawing.Color.Black;
            this.baseLabelManualPacking1_Title.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabelManualPacking1_Title.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelManualPacking1_Title.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabelManualPacking1_Title.Location = new System.Drawing.Point(2, 8);
            this.baseLabelManualPacking1_Title.Name = "baseLabelManualPacking1_Title";
            this.baseLabelManualPacking1_Title.Size = new System.Drawing.Size(149, 30);
            this.baseLabelManualPacking1_Title.TabIndex = 186;
            this.baseLabelManualPacking1_Title.Text = "[            1   단계            ]";
            this.baseLabelManualPacking1_Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabelManualPacking1
            // 
            this.baseLabelManualPacking1.BackColor = System.Drawing.Color.Cornsilk;
            this.baseLabelManualPacking1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabelManualPacking1.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelManualPacking1.ForeColor = System.Drawing.Color.Black;
            this.baseLabelManualPacking1.Location = new System.Drawing.Point(2, 38);
            this.baseLabelManualPacking1.Name = "baseLabelManualPacking1";
            this.baseLabelManualPacking1.Size = new System.Drawing.Size(148, 104);
            this.baseLabelManualPacking1.TabIndex = 185;
            this.baseLabelManualPacking1.Text = "웨이퍼 && 프로브 카드\r\n패킹 전 높이까지 이동\r\n\r\n[ 설정된 패킹 높이에서\r\n   30mm 아래 위치 ]";
            this.baseLabelManualPacking1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabPage_PackingOffsetChange
            // 
            this.tabPage_PackingOffsetChange.Controls.Add(this.checkBox_Step);
            this.tabPage_PackingOffsetChange.Controls.Add(this.checkBox_Jog);
            this.tabPage_PackingOffsetChange.Controls.Add(this.radioButton_Movement_05mm);
            this.tabPage_PackingOffsetChange.Controls.Add(this.radioButton_Movement_001mm);
            this.tabPage_PackingOffsetChange.Controls.Add(this.radioButton_Movement_01mm);
            this.tabPage_PackingOffsetChange.Controls.Add(this.buttonAxisXUpYDown);
            this.tabPage_PackingOffsetChange.Controls.Add(this.buttonAxisYDown);
            this.tabPage_PackingOffsetChange.Controls.Add(this.buttonAxisXUp);
            this.tabPage_PackingOffsetChange.Controls.Add(this.buttonAxisXUpYUp);
            this.tabPage_PackingOffsetChange.Controls.Add(this.buttonAxisXDownYDown);
            this.tabPage_PackingOffsetChange.Controls.Add(this.buttonAxisYUp);
            this.tabPage_PackingOffsetChange.Controls.Add(this.buttonAxisXDown);
            this.tabPage_PackingOffsetChange.Controls.Add(this.buttonAxisXDownYUp);
            this.tabPage_PackingOffsetChange.Controls.Add(this.baseButton_VisionXY_Pos_Get2);
            this.tabPage_PackingOffsetChange.Controls.Add(this.baseButton_VisionXY_Pos_Get1);
            this.tabPage_PackingOffsetChange.Controls.Add(this.baseLabelJogButtonComb_XY);
            this.tabPage_PackingOffsetChange.Controls.Add(this.baseLabelJogButtonComb_Title);
            this.tabPage_PackingOffsetChange.Controls.Add(this.baseLabelPackingOffsetChange2);
            this.tabPage_PackingOffsetChange.Controls.Add(this.baseLabelPackingOffsetChange2_Title);
            this.tabPage_PackingOffsetChange.Controls.Add(this.baseLabelPackingOffsetChange1_Title);
            this.tabPage_PackingOffsetChange.Controls.Add(this.baseLabelPackingOffsetChange1);
            this.tabPage_PackingOffsetChange.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tabPage_PackingOffsetChange.Location = new System.Drawing.Point(4, 38);
            this.tabPage_PackingOffsetChange.Name = "tabPage_PackingOffsetChange";
            this.tabPage_PackingOffsetChange.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_PackingOffsetChange.Size = new System.Drawing.Size(476, 233);
            this.tabPage_PackingOffsetChange.TabIndex = 1;
            this.tabPage_PackingOffsetChange.Text = "    패킹 오프셋 변경    ";
            this.tabPage_PackingOffsetChange.UseVisualStyleBackColor = true;
            // 
            // checkBox_Step
            // 
            this.checkBox_Step.AutoSize = true;
            this.checkBox_Step.Checked = true;
            this.checkBox_Step.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Step.Location = new System.Drawing.Point(310, 30);
            this.checkBox_Step.Name = "checkBox_Step";
            this.checkBox_Step.Size = new System.Drawing.Size(77, 19);
            this.checkBox_Step.TabIndex = 224;
            this.checkBox_Step.Text = "스텝 이동";
            this.checkBox_Step.UseVisualStyleBackColor = true;
            this.checkBox_Step.Click += new System.EventHandler(this.checkBox_Step_Click);
            // 
            // checkBox_Jog
            // 
            this.checkBox_Jog.AutoSize = true;
            this.checkBox_Jog.Location = new System.Drawing.Point(310, 9);
            this.checkBox_Jog.Name = "checkBox_Jog";
            this.checkBox_Jog.Size = new System.Drawing.Size(77, 19);
            this.checkBox_Jog.TabIndex = 223;
            this.checkBox_Jog.Text = "연속 이동";
            this.checkBox_Jog.UseVisualStyleBackColor = true;
            this.checkBox_Jog.Click += new System.EventHandler(this.checkBox_Jog_Click);
            // 
            // radioButton_Movement_05mm
            // 
            this.radioButton_Movement_05mm.AutoSize = true;
            this.radioButton_Movement_05mm.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.radioButton_Movement_05mm.Location = new System.Drawing.Point(397, 9);
            this.radioButton_Movement_05mm.Name = "radioButton_Movement_05mm";
            this.radioButton_Movement_05mm.Size = new System.Drawing.Size(68, 19);
            this.radioButton_Movement_05mm.TabIndex = 222;
            this.radioButton_Movement_05mm.Text = "0.05 ㎜";
            this.radioButton_Movement_05mm.UseVisualStyleBackColor = true;
            this.radioButton_Movement_05mm.Click += new System.EventHandler(this.radioButton_Movement_05mm_Click);
            // 
            // radioButton_Movement_001mm
            // 
            this.radioButton_Movement_001mm.AutoSize = true;
            this.radioButton_Movement_001mm.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.radioButton_Movement_001mm.Location = new System.Drawing.Point(397, 51);
            this.radioButton_Movement_001mm.Name = "radioButton_Movement_001mm";
            this.radioButton_Movement_001mm.Size = new System.Drawing.Size(76, 19);
            this.radioButton_Movement_001mm.TabIndex = 221;
            this.radioButton_Movement_001mm.Text = "0.001 ㎜";
            this.radioButton_Movement_001mm.UseVisualStyleBackColor = true;
            this.radioButton_Movement_001mm.Click += new System.EventHandler(this.radioButton_Movement_001mm_Click);
            // 
            // radioButton_Movement_01mm
            // 
            this.radioButton_Movement_01mm.AutoSize = true;
            this.radioButton_Movement_01mm.Checked = true;
            this.radioButton_Movement_01mm.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.radioButton_Movement_01mm.Location = new System.Drawing.Point(397, 30);
            this.radioButton_Movement_01mm.Name = "radioButton_Movement_01mm";
            this.radioButton_Movement_01mm.Size = new System.Drawing.Size(68, 19);
            this.radioButton_Movement_01mm.TabIndex = 220;
            this.radioButton_Movement_01mm.TabStop = true;
            this.radioButton_Movement_01mm.Text = "0.01 ㎜";
            this.radioButton_Movement_01mm.UseVisualStyleBackColor = true;
            this.radioButton_Movement_01mm.Click += new System.EventHandler(this.radioButton_Movement_01mm_Click);
            // 
            // buttonAxisXUpYDown
            // 
            this.buttonAxisXUpYDown.BackColor = System.Drawing.Color.Lavender;
            this.buttonAxisXUpYDown.Image = global::CWA150SA_Onsemi.Properties.Resources.RightDown;
            this.buttonAxisXUpYDown.Location = new System.Drawing.Point(425, 179);
            this.buttonAxisXUpYDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisXUpYDown.Name = "buttonAxisXUpYDown";
            this.buttonAxisXUpYDown.Size = new System.Drawing.Size(49, 49);
            this.buttonAxisXUpYDown.TabIndex = 210;
            this.buttonAxisXUpYDown.UseVisualStyleBackColor = false;
            this.buttonAxisXUpYDown.Click += new System.EventHandler(this.buttonAxisXUpYDown_Click);
            this.buttonAxisXUpYDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXUpYDown_MouseDown);
            this.buttonAxisXUpYDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDownYUp_MouseUp);
            // 
            // buttonAxisYDown
            // 
            this.buttonAxisYDown.BackColor = System.Drawing.Color.Lavender;
            this.buttonAxisYDown.Image = global::CWA150SA_Onsemi.Properties.Resources.Down;
            this.buttonAxisYDown.Location = new System.Drawing.Point(374, 179);
            this.buttonAxisYDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisYDown.Name = "buttonAxisYDown";
            this.buttonAxisYDown.Size = new System.Drawing.Size(49, 49);
            this.buttonAxisYDown.TabIndex = 211;
            this.buttonAxisYDown.UseVisualStyleBackColor = false;
            this.buttonAxisYDown.Click += new System.EventHandler(this.buttonAxisYDown_Click);
            this.buttonAxisYDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisYDown_MouseDown);
            this.buttonAxisYDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDownYUp_MouseUp);
            // 
            // buttonAxisXUp
            // 
            this.buttonAxisXUp.BackColor = System.Drawing.Color.Lavender;
            this.buttonAxisXUp.Image = global::CWA150SA_Onsemi.Properties.Resources.Right;
            this.buttonAxisXUp.Location = new System.Drawing.Point(425, 129);
            this.buttonAxisXUp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisXUp.Name = "buttonAxisXUp";
            this.buttonAxisXUp.Size = new System.Drawing.Size(49, 49);
            this.buttonAxisXUp.TabIndex = 212;
            this.buttonAxisXUp.UseVisualStyleBackColor = false;
            this.buttonAxisXUp.Click += new System.EventHandler(this.buttonAxisXUp_Click);
            this.buttonAxisXUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXUp_MouseDown_1);
            this.buttonAxisXUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDownYUp_MouseUp);
            // 
            // buttonAxisXUpYUp
            // 
            this.buttonAxisXUpYUp.BackColor = System.Drawing.Color.Lavender;
            this.buttonAxisXUpYUp.Image = global::CWA150SA_Onsemi.Properties.Resources.RightUp;
            this.buttonAxisXUpYUp.Location = new System.Drawing.Point(425, 79);
            this.buttonAxisXUpYUp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisXUpYUp.Name = "buttonAxisXUpYUp";
            this.buttonAxisXUpYUp.Size = new System.Drawing.Size(49, 49);
            this.buttonAxisXUpYUp.TabIndex = 213;
            this.buttonAxisXUpYUp.UseVisualStyleBackColor = false;
            this.buttonAxisXUpYUp.Click += new System.EventHandler(this.buttonAxisXUpYUp_Click);
            this.buttonAxisXUpYUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXUpYUp_MouseDown);
            this.buttonAxisXUpYUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDownYUp_MouseUp);
            // 
            // buttonAxisXDownYDown
            // 
            this.buttonAxisXDownYDown.BackColor = System.Drawing.Color.Lavender;
            this.buttonAxisXDownYDown.Image = global::CWA150SA_Onsemi.Properties.Resources.LeftDown;
            this.buttonAxisXDownYDown.Location = new System.Drawing.Point(323, 179);
            this.buttonAxisXDownYDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisXDownYDown.Name = "buttonAxisXDownYDown";
            this.buttonAxisXDownYDown.Size = new System.Drawing.Size(49, 49);
            this.buttonAxisXDownYDown.TabIndex = 214;
            this.buttonAxisXDownYDown.UseVisualStyleBackColor = false;
            this.buttonAxisXDownYDown.Click += new System.EventHandler(this.buttonAxisXDownYDown_Click);
            this.buttonAxisXDownYDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDownYDown_MouseDown);
            this.buttonAxisXDownYDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDownYUp_MouseUp);
            // 
            // buttonAxisYUp
            // 
            this.buttonAxisYUp.BackColor = System.Drawing.Color.Lavender;
            this.buttonAxisYUp.Image = global::CWA150SA_Onsemi.Properties.Resources.Up;
            this.buttonAxisYUp.Location = new System.Drawing.Point(374, 79);
            this.buttonAxisYUp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisYUp.Name = "buttonAxisYUp";
            this.buttonAxisYUp.Size = new System.Drawing.Size(49, 49);
            this.buttonAxisYUp.TabIndex = 215;
            this.buttonAxisYUp.UseVisualStyleBackColor = false;
            this.buttonAxisYUp.Click += new System.EventHandler(this.buttonAxisYUp_Click);
            this.buttonAxisYUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisYUp_MouseDown);
            this.buttonAxisYUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDownYUp_MouseUp);
            // 
            // buttonAxisXDown
            // 
            this.buttonAxisXDown.BackColor = System.Drawing.Color.Lavender;
            this.buttonAxisXDown.Image = global::CWA150SA_Onsemi.Properties.Resources.Left;
            this.buttonAxisXDown.Location = new System.Drawing.Point(323, 129);
            this.buttonAxisXDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisXDown.Name = "buttonAxisXDown";
            this.buttonAxisXDown.Size = new System.Drawing.Size(49, 49);
            this.buttonAxisXDown.TabIndex = 216;
            this.buttonAxisXDown.UseVisualStyleBackColor = false;
            this.buttonAxisXDown.Click += new System.EventHandler(this.buttonAxisXDown_Click);
            this.buttonAxisXDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDown_MouseDown_1);
            this.buttonAxisXDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDownYUp_MouseUp);
            // 
            // buttonAxisXDownYUp
            // 
            this.buttonAxisXDownYUp.BackColor = System.Drawing.Color.Lavender;
            this.buttonAxisXDownYUp.Image = global::CWA150SA_Onsemi.Properties.Resources.LeftUp;
            this.buttonAxisXDownYUp.Location = new System.Drawing.Point(323, 79);
            this.buttonAxisXDownYUp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisXDownYUp.Name = "buttonAxisXDownYUp";
            this.buttonAxisXDownYUp.Size = new System.Drawing.Size(49, 49);
            this.buttonAxisXDownYUp.TabIndex = 217;
            this.buttonAxisXDownYUp.UseVisualStyleBackColor = false;
            this.buttonAxisXDownYUp.Click += new System.EventHandler(this.buttonAxisXDownYUp_Click);
            this.buttonAxisXDownYUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDownYUp_MouseDown);
            this.buttonAxisXDownYUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDownYUp_MouseUp);
            // 
            // baseButton_VisionXY_Pos_Get2
            // 
            this.baseButton_VisionXY_Pos_Get2.BackColor = System.Drawing.Color.LightGray;
            this.baseButton_VisionXY_Pos_Get2.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.baseButton_VisionXY_Pos_Get2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.baseButton_VisionXY_Pos_Get2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.baseButton_VisionXY_Pos_Get2.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButton_VisionXY_Pos_Get2.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButton_VisionXY_Pos_Get2.Location = new System.Drawing.Point(155, 166);
            this.baseButton_VisionXY_Pos_Get2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseButton_VisionXY_Pos_Get2.Name = "baseButton_VisionXY_Pos_Get2";
            this.baseButton_VisionXY_Pos_Get2.Size = new System.Drawing.Size(139, 63);
            this.baseButton_VisionXY_Pos_Get2.TabIndex = 209;
            this.baseButton_VisionXY_Pos_Get2.Text = "[ SET-2 ]";
            this.baseButton_VisionXY_Pos_Get2.UseVisualStyleBackColor = false;
            this.baseButton_VisionXY_Pos_Get2.Click += new System.EventHandler(this.baseButton_VisionXY_Pos_Get2_Click);
            // 
            // baseButton_VisionXY_Pos_Get1
            // 
            this.baseButton_VisionXY_Pos_Get1.BackColor = System.Drawing.Color.LightGray;
            this.baseButton_VisionXY_Pos_Get1.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.baseButton_VisionXY_Pos_Get1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.baseButton_VisionXY_Pos_Get1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.baseButton_VisionXY_Pos_Get1.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButton_VisionXY_Pos_Get1.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButton_VisionXY_Pos_Get1.Location = new System.Drawing.Point(2, 166);
            this.baseButton_VisionXY_Pos_Get1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseButton_VisionXY_Pos_Get1.Name = "baseButton_VisionXY_Pos_Get1";
            this.baseButton_VisionXY_Pos_Get1.Size = new System.Drawing.Size(139, 63);
            this.baseButton_VisionXY_Pos_Get1.TabIndex = 208;
            this.baseButton_VisionXY_Pos_Get1.Text = "[ SET-1 ]";
            this.baseButton_VisionXY_Pos_Get1.UseVisualStyleBackColor = false;
            this.baseButton_VisionXY_Pos_Get1.Click += new System.EventHandler(this.baseButton_VisionXY_Pos_Get1_Click);
            // 
            // baseLabelJogButtonComb_XY
            // 
            this.baseLabelJogButtonComb_XY.BackColor = System.Drawing.Color.LightSkyBlue;
            this.baseLabelJogButtonComb_XY.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabelJogButtonComb_XY.ForeColor = System.Drawing.Color.Black;
            this.baseLabelJogButtonComb_XY.Location = new System.Drawing.Point(378, 154);
            this.baseLabelJogButtonComb_XY.Name = "baseLabelJogButtonComb_XY";
            this.baseLabelJogButtonComb_XY.Size = new System.Drawing.Size(41, 21);
            this.baseLabelJogButtonComb_XY.TabIndex = 219;
            this.baseLabelJogButtonComb_XY.Text = "[XY]";
            this.baseLabelJogButtonComb_XY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabelJogButtonComb_Title
            // 
            this.baseLabelJogButtonComb_Title.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.baseLabelJogButtonComb_Title.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabelJogButtonComb_Title.ForeColor = System.Drawing.Color.Black;
            this.baseLabelJogButtonComb_Title.Location = new System.Drawing.Point(378, 132);
            this.baseLabelJogButtonComb_Title.Name = "baseLabelJogButtonComb_Title";
            this.baseLabelJogButtonComb_Title.Size = new System.Drawing.Size(41, 21);
            this.baseLabelJogButtonComb_Title.TabIndex = 218;
            this.baseLabelJogButtonComb_Title.Text = "Vision";
            this.baseLabelJogButtonComb_Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabelPackingOffsetChange2
            // 
            this.baseLabelPackingOffsetChange2.BackColor = System.Drawing.Color.Cornsilk;
            this.baseLabelPackingOffsetChange2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabelPackingOffsetChange2.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelPackingOffsetChange2.ForeColor = System.Drawing.Color.Black;
            this.baseLabelPackingOffsetChange2.Location = new System.Drawing.Point(155, 38);
            this.baseLabelPackingOffsetChange2.Name = "baseLabelPackingOffsetChange2";
            this.baseLabelPackingOffsetChange2.Size = new System.Drawing.Size(139, 125);
            this.baseLabelPackingOffsetChange2.TabIndex = 204;
            this.baseLabelPackingOffsetChange2.Text = "프로브 핀이 컨택 해야\r\n하는 위치가 웨이퍼\r\n카메라 중앙에 위치\r\n하도록 이동 후 \r\n\r\n[ SET-2 ] 버튼 클릭";
            this.baseLabelPackingOffsetChange2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabelPackingOffsetChange2_Title
            // 
            this.baseLabelPackingOffsetChange2_Title.BackColor = System.Drawing.Color.Black;
            this.baseLabelPackingOffsetChange2_Title.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabelPackingOffsetChange2_Title.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelPackingOffsetChange2_Title.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabelPackingOffsetChange2_Title.Location = new System.Drawing.Point(155, 8);
            this.baseLabelPackingOffsetChange2_Title.Name = "baseLabelPackingOffsetChange2_Title";
            this.baseLabelPackingOffsetChange2_Title.Size = new System.Drawing.Size(140, 30);
            this.baseLabelPackingOffsetChange2_Title.TabIndex = 203;
            this.baseLabelPackingOffsetChange2_Title.Text = "[           2   단계           ]";
            this.baseLabelPackingOffsetChange2_Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabelPackingOffsetChange1_Title
            // 
            this.baseLabelPackingOffsetChange1_Title.BackColor = System.Drawing.Color.Black;
            this.baseLabelPackingOffsetChange1_Title.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabelPackingOffsetChange1_Title.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelPackingOffsetChange1_Title.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabelPackingOffsetChange1_Title.Location = new System.Drawing.Point(2, 8);
            this.baseLabelPackingOffsetChange1_Title.Name = "baseLabelPackingOffsetChange1_Title";
            this.baseLabelPackingOffsetChange1_Title.Size = new System.Drawing.Size(140, 30);
            this.baseLabelPackingOffsetChange1_Title.TabIndex = 202;
            this.baseLabelPackingOffsetChange1_Title.Text = "[           1   단계           ]";
            this.baseLabelPackingOffsetChange1_Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabelPackingOffsetChange1
            // 
            this.baseLabelPackingOffsetChange1.BackColor = System.Drawing.Color.Cornsilk;
            this.baseLabelPackingOffsetChange1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabelPackingOffsetChange1.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelPackingOffsetChange1.ForeColor = System.Drawing.Color.Black;
            this.baseLabelPackingOffsetChange1.Location = new System.Drawing.Point(2, 38);
            this.baseLabelPackingOffsetChange1.Name = "baseLabelPackingOffsetChange1";
            this.baseLabelPackingOffsetChange1.Size = new System.Drawing.Size(139, 125);
            this.baseLabelPackingOffsetChange1.TabIndex = 201;
            this.baseLabelPackingOffsetChange1.Text = "프로브 핀 컨택 위치가\r\n웨이퍼 카메라의\r\n중앙에 위치하도록\r\n이동 후\r\n\r\n[ SET-1 ]  버튼 클릭";
            this.baseLabelPackingOffsetChange1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabPage_ReticlePositionChange
            // 
            this.tabPage_ReticlePositionChange.Controls.Add(this.baseButton_ReticleGlass_Pos_Get);
            this.tabPage_ReticlePositionChange.Controls.Add(this.checkBox_Reticle_Step);
            this.tabPage_ReticlePositionChange.Controls.Add(this.checkBox_Reticle_Jog);
            this.tabPage_ReticlePositionChange.Controls.Add(this.radioButton_Reticle_Movement_05mm);
            this.tabPage_ReticlePositionChange.Controls.Add(this.radioButton_Reticle_Movement_001mm);
            this.tabPage_ReticlePositionChange.Controls.Add(this.radioButton_Reticle_Movement_01mm);
            this.tabPage_ReticlePositionChange.Controls.Add(this.buttonAxisXUpYDown_Reticle);
            this.tabPage_ReticlePositionChange.Controls.Add(this.buttonAxisYDown_Reticle);
            this.tabPage_ReticlePositionChange.Controls.Add(this.buttonAxisXUp_Reticle);
            this.tabPage_ReticlePositionChange.Controls.Add(this.buttonAxisXUpYUp_Reticle);
            this.tabPage_ReticlePositionChange.Controls.Add(this.buttonAxisXDownYDown_Reticle);
            this.tabPage_ReticlePositionChange.Controls.Add(this.buttonAxisYUp_Reticle);
            this.tabPage_ReticlePositionChange.Controls.Add(this.buttonAxisXDown_Reticle);
            this.tabPage_ReticlePositionChange.Controls.Add(this.buttonAxisXDownYUp_Reticle);
            this.tabPage_ReticlePositionChange.Controls.Add(this.baseLabel22);
            this.tabPage_ReticlePositionChange.Controls.Add(this.baseLabel23);
            this.tabPage_ReticlePositionChange.Controls.Add(this.baseLabelReticlePositionChange2);
            this.tabPage_ReticlePositionChange.Controls.Add(this.baseLabelReticlePositionChange2_Title);
            this.tabPage_ReticlePositionChange.Controls.Add(this.baseLabelReticlePositionChange1_Title);
            this.tabPage_ReticlePositionChange.Controls.Add(this.baseLabelReticlePositionChange1);
            this.tabPage_ReticlePositionChange.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tabPage_ReticlePositionChange.Location = new System.Drawing.Point(4, 38);
            this.tabPage_ReticlePositionChange.Name = "tabPage_ReticlePositionChange";
            this.tabPage_ReticlePositionChange.Size = new System.Drawing.Size(476, 233);
            this.tabPage_ReticlePositionChange.TabIndex = 2;
            this.tabPage_ReticlePositionChange.Text = "    레티클 글래스 위치 변경    ";
            this.tabPage_ReticlePositionChange.UseVisualStyleBackColor = true;
            // 
            // baseButton_ReticleGlass_Pos_Get
            // 
            this.baseButton_ReticleGlass_Pos_Get.BackColor = System.Drawing.Color.LightGray;
            this.baseButton_ReticleGlass_Pos_Get.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.baseButton_ReticleGlass_Pos_Get.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.baseButton_ReticleGlass_Pos_Get.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.baseButton_ReticleGlass_Pos_Get.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButton_ReticleGlass_Pos_Get.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButton_ReticleGlass_Pos_Get.Location = new System.Drawing.Point(155, 146);
            this.baseButton_ReticleGlass_Pos_Get.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseButton_ReticleGlass_Pos_Get.Name = "baseButton_ReticleGlass_Pos_Get";
            this.baseButton_ReticleGlass_Pos_Get.Size = new System.Drawing.Size(139, 63);
            this.baseButton_ReticleGlass_Pos_Get.TabIndex = 244;
            this.baseButton_ReticleGlass_Pos_Get.Text = "[  SET  ]";
            this.baseButton_ReticleGlass_Pos_Get.UseVisualStyleBackColor = false;
            this.baseButton_ReticleGlass_Pos_Get.Click += new System.EventHandler(this.baseButton_ReticleGlass_Pos_Get_Click);
            // 
            // checkBox_Reticle_Step
            // 
            this.checkBox_Reticle_Step.AutoSize = true;
            this.checkBox_Reticle_Step.Checked = true;
            this.checkBox_Reticle_Step.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Reticle_Step.Location = new System.Drawing.Point(310, 30);
            this.checkBox_Reticle_Step.Name = "checkBox_Reticle_Step";
            this.checkBox_Reticle_Step.Size = new System.Drawing.Size(77, 19);
            this.checkBox_Reticle_Step.TabIndex = 243;
            this.checkBox_Reticle_Step.Text = "스텝 이동";
            this.checkBox_Reticle_Step.UseVisualStyleBackColor = true;
            this.checkBox_Reticle_Step.Click += new System.EventHandler(this.checkBox_Step_Click);
            // 
            // checkBox_Reticle_Jog
            // 
            this.checkBox_Reticle_Jog.AutoSize = true;
            this.checkBox_Reticle_Jog.Location = new System.Drawing.Point(310, 9);
            this.checkBox_Reticle_Jog.Name = "checkBox_Reticle_Jog";
            this.checkBox_Reticle_Jog.Size = new System.Drawing.Size(77, 19);
            this.checkBox_Reticle_Jog.TabIndex = 242;
            this.checkBox_Reticle_Jog.Text = "연속 이동";
            this.checkBox_Reticle_Jog.UseVisualStyleBackColor = true;
            this.checkBox_Reticle_Jog.Click += new System.EventHandler(this.checkBox_Jog_Click);
            // 
            // radioButton_Reticle_Movement_05mm
            // 
            this.radioButton_Reticle_Movement_05mm.AutoSize = true;
            this.radioButton_Reticle_Movement_05mm.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.radioButton_Reticle_Movement_05mm.Location = new System.Drawing.Point(397, 9);
            this.radioButton_Reticle_Movement_05mm.Name = "radioButton_Reticle_Movement_05mm";
            this.radioButton_Reticle_Movement_05mm.Size = new System.Drawing.Size(68, 19);
            this.radioButton_Reticle_Movement_05mm.TabIndex = 241;
            this.radioButton_Reticle_Movement_05mm.Text = "0.05 ㎜";
            this.radioButton_Reticle_Movement_05mm.UseVisualStyleBackColor = true;
            this.radioButton_Reticle_Movement_05mm.Click += new System.EventHandler(this.radioButton_Movement_05mm_Click);
            // 
            // radioButton_Reticle_Movement_001mm
            // 
            this.radioButton_Reticle_Movement_001mm.AutoSize = true;
            this.radioButton_Reticle_Movement_001mm.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.radioButton_Reticle_Movement_001mm.Location = new System.Drawing.Point(397, 51);
            this.radioButton_Reticle_Movement_001mm.Name = "radioButton_Reticle_Movement_001mm";
            this.radioButton_Reticle_Movement_001mm.Size = new System.Drawing.Size(76, 19);
            this.radioButton_Reticle_Movement_001mm.TabIndex = 240;
            this.radioButton_Reticle_Movement_001mm.Text = "0.001 ㎜";
            this.radioButton_Reticle_Movement_001mm.UseVisualStyleBackColor = true;
            this.radioButton_Reticle_Movement_001mm.Click += new System.EventHandler(this.radioButton_Movement_001mm_Click);
            // 
            // radioButton_Reticle_Movement_01mm
            // 
            this.radioButton_Reticle_Movement_01mm.AutoSize = true;
            this.radioButton_Reticle_Movement_01mm.Checked = true;
            this.radioButton_Reticle_Movement_01mm.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.radioButton_Reticle_Movement_01mm.Location = new System.Drawing.Point(397, 30);
            this.radioButton_Reticle_Movement_01mm.Name = "radioButton_Reticle_Movement_01mm";
            this.radioButton_Reticle_Movement_01mm.Size = new System.Drawing.Size(68, 19);
            this.radioButton_Reticle_Movement_01mm.TabIndex = 239;
            this.radioButton_Reticle_Movement_01mm.TabStop = true;
            this.radioButton_Reticle_Movement_01mm.Text = "0.01 ㎜";
            this.radioButton_Reticle_Movement_01mm.UseVisualStyleBackColor = true;
            this.radioButton_Reticle_Movement_01mm.Click += new System.EventHandler(this.radioButton_Movement_01mm_Click);
            // 
            // buttonAxisXUpYDown_Reticle
            // 
            this.buttonAxisXUpYDown_Reticle.BackColor = System.Drawing.Color.Lavender;
            this.buttonAxisXUpYDown_Reticle.Image = global::CWA150SA_Onsemi.Properties.Resources.RightDown;
            this.buttonAxisXUpYDown_Reticle.Location = new System.Drawing.Point(425, 179);
            this.buttonAxisXUpYDown_Reticle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisXUpYDown_Reticle.Name = "buttonAxisXUpYDown_Reticle";
            this.buttonAxisXUpYDown_Reticle.Size = new System.Drawing.Size(49, 49);
            this.buttonAxisXUpYDown_Reticle.TabIndex = 229;
            this.buttonAxisXUpYDown_Reticle.UseVisualStyleBackColor = false;
            this.buttonAxisXUpYDown_Reticle.Click += new System.EventHandler(this.buttonAxisXUpYDown_Click);
            this.buttonAxisXUpYDown_Reticle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXUpYDown_MouseDown);
            this.buttonAxisXUpYDown_Reticle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDownYUp_MouseUp);
            // 
            // buttonAxisYDown_Reticle
            // 
            this.buttonAxisYDown_Reticle.BackColor = System.Drawing.Color.Lavender;
            this.buttonAxisYDown_Reticle.Image = global::CWA150SA_Onsemi.Properties.Resources.Down;
            this.buttonAxisYDown_Reticle.Location = new System.Drawing.Point(374, 179);
            this.buttonAxisYDown_Reticle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisYDown_Reticle.Name = "buttonAxisYDown_Reticle";
            this.buttonAxisYDown_Reticle.Size = new System.Drawing.Size(49, 49);
            this.buttonAxisYDown_Reticle.TabIndex = 230;
            this.buttonAxisYDown_Reticle.UseVisualStyleBackColor = false;
            this.buttonAxisYDown_Reticle.Click += new System.EventHandler(this.buttonAxisYDown_Click);
            this.buttonAxisYDown_Reticle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisYDown_MouseDown);
            this.buttonAxisYDown_Reticle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDownYUp_MouseUp);
            // 
            // buttonAxisXUp_Reticle
            // 
            this.buttonAxisXUp_Reticle.BackColor = System.Drawing.Color.Lavender;
            this.buttonAxisXUp_Reticle.Image = global::CWA150SA_Onsemi.Properties.Resources.Right;
            this.buttonAxisXUp_Reticle.Location = new System.Drawing.Point(425, 129);
            this.buttonAxisXUp_Reticle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisXUp_Reticle.Name = "buttonAxisXUp_Reticle";
            this.buttonAxisXUp_Reticle.Size = new System.Drawing.Size(49, 49);
            this.buttonAxisXUp_Reticle.TabIndex = 231;
            this.buttonAxisXUp_Reticle.UseVisualStyleBackColor = false;
            this.buttonAxisXUp_Reticle.Click += new System.EventHandler(this.buttonAxisXUp_Click);
            this.buttonAxisXUp_Reticle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXUp_MouseDown_1);
            this.buttonAxisXUp_Reticle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDownYUp_MouseUp);
            // 
            // buttonAxisXUpYUp_Reticle
            // 
            this.buttonAxisXUpYUp_Reticle.BackColor = System.Drawing.Color.Lavender;
            this.buttonAxisXUpYUp_Reticle.Image = global::CWA150SA_Onsemi.Properties.Resources.RightUp;
            this.buttonAxisXUpYUp_Reticle.Location = new System.Drawing.Point(425, 79);
            this.buttonAxisXUpYUp_Reticle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisXUpYUp_Reticle.Name = "buttonAxisXUpYUp_Reticle";
            this.buttonAxisXUpYUp_Reticle.Size = new System.Drawing.Size(49, 49);
            this.buttonAxisXUpYUp_Reticle.TabIndex = 232;
            this.buttonAxisXUpYUp_Reticle.UseVisualStyleBackColor = false;
            this.buttonAxisXUpYUp_Reticle.Click += new System.EventHandler(this.buttonAxisXUpYUp_Click);
            this.buttonAxisXUpYUp_Reticle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXUpYUp_MouseDown);
            this.buttonAxisXUpYUp_Reticle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDownYUp_MouseUp);
            // 
            // buttonAxisXDownYDown_Reticle
            // 
            this.buttonAxisXDownYDown_Reticle.BackColor = System.Drawing.Color.Lavender;
            this.buttonAxisXDownYDown_Reticle.Image = global::CWA150SA_Onsemi.Properties.Resources.LeftDown;
            this.buttonAxisXDownYDown_Reticle.Location = new System.Drawing.Point(323, 179);
            this.buttonAxisXDownYDown_Reticle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisXDownYDown_Reticle.Name = "buttonAxisXDownYDown_Reticle";
            this.buttonAxisXDownYDown_Reticle.Size = new System.Drawing.Size(49, 49);
            this.buttonAxisXDownYDown_Reticle.TabIndex = 233;
            this.buttonAxisXDownYDown_Reticle.UseVisualStyleBackColor = false;
            this.buttonAxisXDownYDown_Reticle.Click += new System.EventHandler(this.buttonAxisXDownYDown_Click);
            this.buttonAxisXDownYDown_Reticle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDownYDown_MouseDown);
            this.buttonAxisXDownYDown_Reticle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDownYUp_MouseUp);
            // 
            // buttonAxisYUp_Reticle
            // 
            this.buttonAxisYUp_Reticle.BackColor = System.Drawing.Color.Lavender;
            this.buttonAxisYUp_Reticle.Image = global::CWA150SA_Onsemi.Properties.Resources.Up;
            this.buttonAxisYUp_Reticle.Location = new System.Drawing.Point(374, 79);
            this.buttonAxisYUp_Reticle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisYUp_Reticle.Name = "buttonAxisYUp_Reticle";
            this.buttonAxisYUp_Reticle.Size = new System.Drawing.Size(49, 49);
            this.buttonAxisYUp_Reticle.TabIndex = 234;
            this.buttonAxisYUp_Reticle.UseVisualStyleBackColor = false;
            this.buttonAxisYUp_Reticle.Click += new System.EventHandler(this.buttonAxisYUp_Click);
            this.buttonAxisYUp_Reticle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisYUp_MouseDown);
            this.buttonAxisYUp_Reticle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDownYUp_MouseUp);
            // 
            // buttonAxisXDown_Reticle
            // 
            this.buttonAxisXDown_Reticle.BackColor = System.Drawing.Color.Lavender;
            this.buttonAxisXDown_Reticle.Image = global::CWA150SA_Onsemi.Properties.Resources.Left;
            this.buttonAxisXDown_Reticle.Location = new System.Drawing.Point(323, 129);
            this.buttonAxisXDown_Reticle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisXDown_Reticle.Name = "buttonAxisXDown_Reticle";
            this.buttonAxisXDown_Reticle.Size = new System.Drawing.Size(49, 49);
            this.buttonAxisXDown_Reticle.TabIndex = 235;
            this.buttonAxisXDown_Reticle.UseVisualStyleBackColor = false;
            this.buttonAxisXDown_Reticle.Click += new System.EventHandler(this.buttonAxisXDown_Click);
            this.buttonAxisXDown_Reticle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDown_MouseDown_1);
            this.buttonAxisXDown_Reticle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDownYUp_MouseUp);
            // 
            // buttonAxisXDownYUp_Reticle
            // 
            this.buttonAxisXDownYUp_Reticle.BackColor = System.Drawing.Color.Lavender;
            this.buttonAxisXDownYUp_Reticle.Image = global::CWA150SA_Onsemi.Properties.Resources.LeftUp;
            this.buttonAxisXDownYUp_Reticle.Location = new System.Drawing.Point(323, 79);
            this.buttonAxisXDownYUp_Reticle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAxisXDownYUp_Reticle.Name = "buttonAxisXDownYUp_Reticle";
            this.buttonAxisXDownYUp_Reticle.Size = new System.Drawing.Size(49, 49);
            this.buttonAxisXDownYUp_Reticle.TabIndex = 236;
            this.buttonAxisXDownYUp_Reticle.UseVisualStyleBackColor = false;
            this.buttonAxisXDownYUp_Reticle.Click += new System.EventHandler(this.buttonAxisXDownYUp_Click);
            this.buttonAxisXDownYUp_Reticle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDownYUp_MouseDown);
            this.buttonAxisXDownYUp_Reticle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonAxisXDownYUp_MouseUp);
            // 
            // baseLabel22
            // 
            this.baseLabel22.BackColor = System.Drawing.Color.LightSkyBlue;
            this.baseLabel22.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel22.ForeColor = System.Drawing.Color.Black;
            this.baseLabel22.Location = new System.Drawing.Point(378, 154);
            this.baseLabel22.Name = "baseLabel22";
            this.baseLabel22.Size = new System.Drawing.Size(41, 21);
            this.baseLabel22.TabIndex = 238;
            this.baseLabel22.Text = "[XY]";
            this.baseLabel22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel23
            // 
            this.baseLabel23.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.baseLabel23.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel23.ForeColor = System.Drawing.Color.Black;
            this.baseLabel23.Location = new System.Drawing.Point(378, 132);
            this.baseLabel23.Name = "baseLabel23";
            this.baseLabel23.Size = new System.Drawing.Size(41, 21);
            this.baseLabel23.TabIndex = 237;
            this.baseLabel23.Text = "Vision";
            this.baseLabel23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabelReticlePositionChange2
            // 
            this.baseLabelReticlePositionChange2.BackColor = System.Drawing.Color.Cornsilk;
            this.baseLabelReticlePositionChange2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabelReticlePositionChange2.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelReticlePositionChange2.ForeColor = System.Drawing.Color.Black;
            this.baseLabelReticlePositionChange2.Location = new System.Drawing.Point(155, 38);
            this.baseLabelReticlePositionChange2.Name = "baseLabelReticlePositionChange2";
            this.baseLabelReticlePositionChange2.Size = new System.Drawing.Size(139, 104);
            this.baseLabelReticlePositionChange2.TabIndex = 228;
            this.baseLabelReticlePositionChange2.Text = "프로브 카드 카메라가\r\n레티클 글래스 센터에\r\n위치하도록 조정 후\r\n\r\n[  SET  ]  버튼 클릭";
            this.baseLabelReticlePositionChange2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabelReticlePositionChange2_Title
            // 
            this.baseLabelReticlePositionChange2_Title.BackColor = System.Drawing.Color.Black;
            this.baseLabelReticlePositionChange2_Title.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabelReticlePositionChange2_Title.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelReticlePositionChange2_Title.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabelReticlePositionChange2_Title.Location = new System.Drawing.Point(155, 8);
            this.baseLabelReticlePositionChange2_Title.Name = "baseLabelReticlePositionChange2_Title";
            this.baseLabelReticlePositionChange2_Title.Size = new System.Drawing.Size(140, 30);
            this.baseLabelReticlePositionChange2_Title.TabIndex = 227;
            this.baseLabelReticlePositionChange2_Title.Text = "[           2   단계           ]";
            this.baseLabelReticlePositionChange2_Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabelReticlePositionChange1_Title
            // 
            this.baseLabelReticlePositionChange1_Title.BackColor = System.Drawing.Color.Black;
            this.baseLabelReticlePositionChange1_Title.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabelReticlePositionChange1_Title.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelReticlePositionChange1_Title.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabelReticlePositionChange1_Title.Location = new System.Drawing.Point(2, 8);
            this.baseLabelReticlePositionChange1_Title.Name = "baseLabelReticlePositionChange1_Title";
            this.baseLabelReticlePositionChange1_Title.Size = new System.Drawing.Size(140, 30);
            this.baseLabelReticlePositionChange1_Title.TabIndex = 226;
            this.baseLabelReticlePositionChange1_Title.Text = "[           1   단계           ]";
            this.baseLabelReticlePositionChange1_Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabelReticlePositionChange1
            // 
            this.baseLabelReticlePositionChange1.BackColor = System.Drawing.Color.Cornsilk;
            this.baseLabelReticlePositionChange1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabelReticlePositionChange1.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelReticlePositionChange1.ForeColor = System.Drawing.Color.Black;
            this.baseLabelReticlePositionChange1.Location = new System.Drawing.Point(2, 38);
            this.baseLabelReticlePositionChange1.Name = "baseLabelReticlePositionChange1";
            this.baseLabelReticlePositionChange1.Size = new System.Drawing.Size(139, 104);
            this.baseLabelReticlePositionChange1.TabIndex = 225;
            this.baseLabelReticlePositionChange1.Text = "\"[상부 카메라]\r\n레티클 글래스 센터\r\n확인 위치 이동\"\r\n\r\n버튼 클릭";
            this.baseLabelReticlePositionChange1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // m_visionImageViewer_Lower
            // 
            this.m_visionImageViewer_Lower.BackColor = System.Drawing.Color.Black;
            this.m_visionImageViewer_Lower.Camera = null;
            this.m_visionImageViewer_Lower.CameraSwitch = null;
            this.m_visionImageViewer_Lower.FrameRate = 1D;
            this.m_visionImageViewer_Lower.InputImage = null;
            this.m_visionImageViewer_Lower.IsViewCustomizedImage = false;
            this.m_visionImageViewer_Lower.Location = new System.Drawing.Point(515, 26);
            this.m_visionImageViewer_Lower.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.m_visionImageViewer_Lower.Name = "m_visionImageViewer_Lower";
            this.m_visionImageViewer_Lower.OperatingType = QMC.Common.Hmi.VisionImageViewer.OperatingTypes.Center;
            this.m_visionImageViewer_Lower.Simulated = false;
            this.m_visionImageViewer_Lower.Size = new System.Drawing.Size(503, 422);
            this.m_visionImageViewer_Lower.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.m_visionImageViewer_Lower.TabIndex = 106;
            this.m_visionImageViewer_Lower.TabStop = false;
            this.m_visionImageViewer_Lower.UpdateDelayTime = 160;
            this.m_visionImageViewer_Lower.VisibleCrossLine = true;
            // 
            // m_visionImageViewer_Upper
            // 
            this.m_visionImageViewer_Upper.BackColor = System.Drawing.Color.Black;
            this.m_visionImageViewer_Upper.Camera = null;
            this.m_visionImageViewer_Upper.CameraSwitch = null;
            this.m_visionImageViewer_Upper.FrameRate = 1D;
            this.m_visionImageViewer_Upper.InputImage = null;
            this.m_visionImageViewer_Upper.IsViewCustomizedImage = false;
            this.m_visionImageViewer_Upper.Location = new System.Drawing.Point(4, 26);
            this.m_visionImageViewer_Upper.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.m_visionImageViewer_Upper.Name = "m_visionImageViewer_Upper";
            this.m_visionImageViewer_Upper.OperatingType = QMC.Common.Hmi.VisionImageViewer.OperatingTypes.Center;
            this.m_visionImageViewer_Upper.Simulated = false;
            this.m_visionImageViewer_Upper.Size = new System.Drawing.Size(503, 422);
            this.m_visionImageViewer_Upper.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.m_visionImageViewer_Upper.TabIndex = 105;
            this.m_visionImageViewer_Upper.TabStop = false;
            this.m_visionImageViewer_Upper.UpdateDelayTime = 160;
            this.m_visionImageViewer_Upper.VisibleCrossLine = true;
            // 
            // lblLowerCamera_AlignData
            // 
            this.lblLowerCamera_AlignData.BackColor = System.Drawing.Color.Cornsilk;
            this.lblLowerCamera_AlignData.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblLowerCamera_AlignData.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblLowerCamera_AlignData.ForeColor = System.Drawing.Color.Black;
            this.lblLowerCamera_AlignData.Location = new System.Drawing.Point(667, 568);
            this.lblLowerCamera_AlignData.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLowerCamera_AlignData.Name = "lblLowerCamera_AlignData";
            this.lblLowerCamera_AlignData.Size = new System.Drawing.Size(123, 34);
            this.lblLowerCamera_AlignData.TabIndex = 187;
            this.lblLowerCamera_AlignData.Text = "- - - - -";
            this.lblLowerCamera_AlignData.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabelAngle_Lower
            // 
            this.baseLabelAngle_Lower.BackColor = System.Drawing.Color.Black;
            this.baseLabelAngle_Lower.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabelAngle_Lower.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelAngle_Lower.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabelAngle_Lower.Location = new System.Drawing.Point(667, 541);
            this.baseLabelAngle_Lower.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabelAngle_Lower.Name = "baseLabelAngle_Lower";
            this.baseLabelAngle_Lower.Size = new System.Drawing.Size(123, 27);
            this.baseLabelAngle_Lower.TabIndex = 186;
            this.baseLabelAngle_Lower.Text = "[ 회전 각도(˚) ]";
            this.baseLabelAngle_Lower.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblUpperCamera_AlignData
            // 
            this.lblUpperCamera_AlignData.BackColor = System.Drawing.Color.Cornsilk;
            this.lblUpperCamera_AlignData.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblUpperCamera_AlignData.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblUpperCamera_AlignData.ForeColor = System.Drawing.Color.Black;
            this.lblUpperCamera_AlignData.Location = new System.Drawing.Point(156, 568);
            this.lblUpperCamera_AlignData.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUpperCamera_AlignData.Name = "lblUpperCamera_AlignData";
            this.lblUpperCamera_AlignData.Size = new System.Drawing.Size(123, 34);
            this.lblUpperCamera_AlignData.TabIndex = 185;
            this.lblUpperCamera_AlignData.Text = "- - - - -";
            this.lblUpperCamera_AlignData.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabelAngle_Upper
            // 
            this.baseLabelAngle_Upper.BackColor = System.Drawing.Color.Black;
            this.baseLabelAngle_Upper.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabelAngle_Upper.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelAngle_Upper.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabelAngle_Upper.Location = new System.Drawing.Point(156, 541);
            this.baseLabelAngle_Upper.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabelAngle_Upper.Name = "baseLabelAngle_Upper";
            this.baseLabelAngle_Upper.Size = new System.Drawing.Size(123, 27);
            this.baseLabelAngle_Upper.TabIndex = 184;
            this.baseLabelAngle_Upper.Text = "[ 회전 각도(˚) ]";
            this.baseLabelAngle_Upper.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseGroupBox_CycleButton
            // 
            this.baseGroupBox_CycleButton.Controls.Add(this.baseLabel21);
            this.baseGroupBox_CycleButton.Controls.Add(this.btnPAK_Leak_Check);
            this.baseGroupBox_CycleButton.Controls.Add(this.pictureBoxProbePackingCheck);
            this.baseGroupBox_CycleButton.Controls.Add(this.lblProbePackingCheck);
            this.baseGroupBox_CycleButton.Controls.Add(this.pictureBoxWaferVacuumCheck);
            this.baseGroupBox_CycleButton.Controls.Add(this.lblWaferVacuumCheck);
            this.baseGroupBox_CycleButton.Controls.Add(this.pictureBoxThinChuckVacuumCheck);
            this.baseGroupBox_CycleButton.Controls.Add(this.lblThinChuckVacuumCheck);
            this.baseGroupBox_CycleButton.Controls.Add(this.pictureBoxThinChuckDetect);
            this.baseGroupBox_CycleButton.Controls.Add(this.baseLabelThinChuckDetect);
            this.baseGroupBox_CycleButton.Controls.Add(this.baseLabel7);
            this.baseGroupBox_CycleButton.Controls.Add(this.baseLabel6);
            this.baseGroupBox_CycleButton.Controls.Add(this.baseLabel5);
            this.baseGroupBox_CycleButton.Controls.Add(this.baseLabel_Packing);
            this.baseGroupBox_CycleButton.Controls.Add(this.baseLabel_Align);
            this.baseGroupBox_CycleButton.Controls.Add(this.baseLabel2);
            this.baseGroupBox_CycleButton.Controls.Add(this.baseButtonWaferVacuum);
            this.baseGroupBox_CycleButton.Controls.Add(this.baseButtonThinChuckVacuum);
            this.baseGroupBox_CycleButton.Controls.Add(this.baseLabel1);
            this.baseGroupBox_CycleButton.Controls.Add(this.btnPacking);
            this.baseGroupBox_CycleButton.Controls.Add(this.btnProbeCardLocking);
            this.baseGroupBox_CycleButton.Controls.Add(this.btnProbeCardLoadingReady);
            this.baseGroupBox_CycleButton.Controls.Add(this.btnLoadingPos);
            this.baseGroupBox_CycleButton.Controls.Add(this.btnMainWork_Start);
            this.baseGroupBox_CycleButton.Controls.Add(this.tabControl_ProbeCard_ClampType);
            this.baseGroupBox_CycleButton.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_CycleButton.ForeColor = System.Drawing.Color.White;
            this.baseGroupBox_CycleButton.Location = new System.Drawing.Point(917, 460);
            this.baseGroupBox_CycleButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseGroupBox_CycleButton.Name = "baseGroupBox_CycleButton";
            this.baseGroupBox_CycleButton.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseGroupBox_CycleButton.Size = new System.Drawing.Size(987, 299);
            this.baseGroupBox_CycleButton.TabIndex = 126;
            this.baseGroupBox_CycleButton.TabStop = false;
            this.baseGroupBox_CycleButton.Text = " [ 작업 순서 ] ";
            // 
            // baseLabel21
            // 
            this.baseLabel21.AutoSize = true;
            this.baseLabel21.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel21.ForeColor = System.Drawing.Color.White;
            this.baseLabel21.Location = new System.Drawing.Point(533, 237);
            this.baseLabel21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel21.Name = "baseLabel21";
            this.baseLabel21.Size = new System.Drawing.Size(27, 22);
            this.baseLabel21.TabIndex = 154;
            this.baseLabel21.Text = "▶";
            // 
            // btnPAK_Leak_Check
            // 
            this.btnPAK_Leak_Check.BackColor = System.Drawing.Color.LightGray;
            this.btnPAK_Leak_Check.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnPAK_Leak_Check.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnPAK_Leak_Check.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnPAK_Leak_Check.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPAK_Leak_Check.ForeColor = System.Drawing.Color.DarkRed;
            this.btnPAK_Leak_Check.Location = new System.Drawing.Point(564, 206);
            this.btnPAK_Leak_Check.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnPAK_Leak_Check.Name = "btnPAK_Leak_Check";
            this.btnPAK_Leak_Check.Size = new System.Drawing.Size(205, 83);
            this.btnPAK_Leak_Check.TabIndex = 153;
            this.btnPAK_Leak_Check.Text = "수동 기능  :    PAK 점검\r\n\r\n(패킹 공압 관로 막힘 확인)";
            this.btnPAK_Leak_Check.UseVisualStyleBackColor = false;
            this.btnPAK_Leak_Check.Click += new System.EventHandler(this.btnPAK_Leak_Check_Click);
            // 
            // pictureBoxProbePackingCheck
            // 
            this.pictureBoxProbePackingCheck.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
            this.pictureBoxProbePackingCheck.Location = new System.Drawing.Point(790, 186);
            this.pictureBoxProbePackingCheck.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBoxProbePackingCheck.Name = "pictureBoxProbePackingCheck";
            this.pictureBoxProbePackingCheck.Size = new System.Drawing.Size(22, 22);
            this.pictureBoxProbePackingCheck.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxProbePackingCheck.TabIndex = 152;
            this.pictureBoxProbePackingCheck.TabStop = false;
            // 
            // lblProbePackingCheck
            // 
            this.lblProbePackingCheck.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblProbePackingCheck.ForeColor = System.Drawing.Color.White;
            this.lblProbePackingCheck.Location = new System.Drawing.Point(815, 186);
            this.lblProbePackingCheck.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProbePackingCheck.Name = "lblProbePackingCheck";
            this.lblProbePackingCheck.Size = new System.Drawing.Size(102, 22);
            this.lblProbePackingCheck.TabIndex = 151;
            this.lblProbePackingCheck.Text = "프로브 패킹 공압";
            this.lblProbePackingCheck.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBoxWaferVacuumCheck
            // 
            this.pictureBoxWaferVacuumCheck.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
            this.pictureBoxWaferVacuumCheck.Location = new System.Drawing.Point(119, 195);
            this.pictureBoxWaferVacuumCheck.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBoxWaferVacuumCheck.Name = "pictureBoxWaferVacuumCheck";
            this.pictureBoxWaferVacuumCheck.Size = new System.Drawing.Size(22, 22);
            this.pictureBoxWaferVacuumCheck.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxWaferVacuumCheck.TabIndex = 144;
            this.pictureBoxWaferVacuumCheck.TabStop = false;
            // 
            // lblWaferVacuumCheck
            // 
            this.lblWaferVacuumCheck.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblWaferVacuumCheck.ForeColor = System.Drawing.Color.White;
            this.lblWaferVacuumCheck.Location = new System.Drawing.Point(142, 195);
            this.lblWaferVacuumCheck.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWaferVacuumCheck.Name = "lblWaferVacuumCheck";
            this.lblWaferVacuumCheck.Size = new System.Drawing.Size(72, 22);
            this.lblWaferVacuumCheck.TabIndex = 143;
            this.lblWaferVacuumCheck.Text = "웨이퍼 진공";
            this.lblWaferVacuumCheck.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBoxThinChuckVacuumCheck
            // 
            this.pictureBoxThinChuckVacuumCheck.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
            this.pictureBoxThinChuckVacuumCheck.Location = new System.Drawing.Point(13, 195);
            this.pictureBoxThinChuckVacuumCheck.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBoxThinChuckVacuumCheck.Name = "pictureBoxThinChuckVacuumCheck";
            this.pictureBoxThinChuckVacuumCheck.Size = new System.Drawing.Size(22, 22);
            this.pictureBoxThinChuckVacuumCheck.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxThinChuckVacuumCheck.TabIndex = 142;
            this.pictureBoxThinChuckVacuumCheck.TabStop = false;
            // 
            // lblThinChuckVacuumCheck
            // 
            this.lblThinChuckVacuumCheck.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblThinChuckVacuumCheck.ForeColor = System.Drawing.Color.White;
            this.lblThinChuckVacuumCheck.Location = new System.Drawing.Point(36, 195);
            this.lblThinChuckVacuumCheck.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblThinChuckVacuumCheck.Name = "lblThinChuckVacuumCheck";
            this.lblThinChuckVacuumCheck.Size = new System.Drawing.Size(68, 22);
            this.lblThinChuckVacuumCheck.TabIndex = 141;
            this.lblThinChuckVacuumCheck.Text = "씬-척 진공";
            this.lblThinChuckVacuumCheck.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBoxThinChuckDetect
            // 
            this.pictureBoxThinChuckDetect.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
            this.pictureBoxThinChuckDetect.Location = new System.Drawing.Point(13, 169);
            this.pictureBoxThinChuckDetect.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBoxThinChuckDetect.Name = "pictureBoxThinChuckDetect";
            this.pictureBoxThinChuckDetect.Size = new System.Drawing.Size(22, 22);
            this.pictureBoxThinChuckDetect.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxThinChuckDetect.TabIndex = 140;
            this.pictureBoxThinChuckDetect.TabStop = false;
            // 
            // baseLabelThinChuckDetect
            // 
            this.baseLabelThinChuckDetect.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelThinChuckDetect.ForeColor = System.Drawing.Color.White;
            this.baseLabelThinChuckDetect.Location = new System.Drawing.Point(36, 169);
            this.baseLabelThinChuckDetect.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabelThinChuckDetect.Name = "baseLabelThinChuckDetect";
            this.baseLabelThinChuckDetect.Size = new System.Drawing.Size(68, 22);
            this.baseLabelThinChuckDetect.TabIndex = 139;
            this.baseLabelThinChuckDetect.Text = "씬-척 감지";
            this.baseLabelThinChuckDetect.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // baseLabel7
            // 
            this.baseLabel7.AutoSize = true;
            this.baseLabel7.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel7.ForeColor = System.Drawing.Color.White;
            this.baseLabel7.Location = new System.Drawing.Point(758, 27);
            this.baseLabel7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel7.Name = "baseLabel7";
            this.baseLabel7.Size = new System.Drawing.Size(23, 19);
            this.baseLabel7.TabIndex = 138;
            this.baseLabel7.Text = "▷";
            // 
            // baseLabel6
            // 
            this.baseLabel6.AutoSize = true;
            this.baseLabel6.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel6.ForeColor = System.Drawing.Color.White;
            this.baseLabel6.Location = new System.Drawing.Point(534, 27);
            this.baseLabel6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel6.Name = "baseLabel6";
            this.baseLabel6.Size = new System.Drawing.Size(23, 19);
            this.baseLabel6.TabIndex = 137;
            this.baseLabel6.Text = "▷";
            // 
            // baseLabel5
            // 
            this.baseLabel5.AutoSize = true;
            this.baseLabel5.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel5.ForeColor = System.Drawing.Color.White;
            this.baseLabel5.Location = new System.Drawing.Point(215, 27);
            this.baseLabel5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel5.Name = "baseLabel5";
            this.baseLabel5.Size = new System.Drawing.Size(23, 19);
            this.baseLabel5.TabIndex = 136;
            this.baseLabel5.Text = "▷";
            // 
            // baseLabel_Packing
            // 
            this.baseLabel_Packing.BackColor = System.Drawing.Color.Black;
            this.baseLabel_Packing.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabel_Packing.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_Packing.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel_Packing.Location = new System.Drawing.Point(788, 26);
            this.baseLabel_Packing.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Packing.Name = "baseLabel_Packing";
            this.baseLabel_Packing.Size = new System.Drawing.Size(186, 20);
            this.baseLabel_Packing.TabIndex = 135;
            this.baseLabel_Packing.Text = "패킹";
            this.baseLabel_Packing.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel_Align
            // 
            this.baseLabel_Align.BackColor = System.Drawing.Color.Black;
            this.baseLabel_Align.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabel_Align.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_Align.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel_Align.Location = new System.Drawing.Point(564, 26);
            this.baseLabel_Align.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Align.Name = "baseLabel_Align";
            this.baseLabel_Align.Size = new System.Drawing.Size(186, 20);
            this.baseLabel_Align.TabIndex = 132;
            this.baseLabel_Align.Text = "얼라인";
            this.baseLabel_Align.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel2
            // 
            this.baseLabel2.BackColor = System.Drawing.Color.Black;
            this.baseLabel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabel2.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel2.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel2.Location = new System.Drawing.Point(243, 26);
            this.baseLabel2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel2.Name = "baseLabel2";
            this.baseLabel2.Size = new System.Drawing.Size(282, 20);
            this.baseLabel2.TabIndex = 131;
            this.baseLabel2.Text = "[ 프로브 카드 ]   투입";
            this.baseLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseButtonWaferVacuum
            // 
            this.baseButtonWaferVacuum.BackColor = System.Drawing.Color.LightGray;
            this.baseButtonWaferVacuum.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonWaferVacuum.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButtonWaferVacuum.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButtonWaferVacuum.Location = new System.Drawing.Point(112, 223);
            this.baseButtonWaferVacuum.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseButtonWaferVacuum.Name = "baseButtonWaferVacuum";
            this.baseButtonWaferVacuum.Size = new System.Drawing.Size(95, 66);
            this.baseButtonWaferVacuum.TabIndex = 130;
            this.baseButtonWaferVacuum.Text = "웨이퍼 공압\r\nOn";
            this.baseButtonWaferVacuum.UseVisualStyleBackColor = false;
            this.baseButtonWaferVacuum.Click += new System.EventHandler(this.baseButtonWaferVacuum_Click);
            // 
            // baseButtonThinChuckVacuum
            // 
            this.baseButtonThinChuckVacuum.BackColor = System.Drawing.Color.LightGray;
            this.baseButtonThinChuckVacuum.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonThinChuckVacuum.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButtonThinChuckVacuum.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButtonThinChuckVacuum.Location = new System.Drawing.Point(13, 223);
            this.baseButtonThinChuckVacuum.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseButtonThinChuckVacuum.Name = "baseButtonThinChuckVacuum";
            this.baseButtonThinChuckVacuum.Size = new System.Drawing.Size(90, 66);
            this.baseButtonThinChuckVacuum.TabIndex = 127;
            this.baseButtonThinChuckVacuum.Text = "씬-척 공압\r\nOn";
            this.baseButtonThinChuckVacuum.UseVisualStyleBackColor = false;
            this.baseButtonThinChuckVacuum.Click += new System.EventHandler(this.baseButtonThinChuckVacuum_Click);
            // 
            // baseLabel1
            // 
            this.baseLabel1.BackColor = System.Drawing.Color.Black;
            this.baseLabel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabel1.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel1.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel1.Location = new System.Drawing.Point(13, 26);
            this.baseLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel1.Name = "baseLabel1";
            this.baseLabel1.Size = new System.Drawing.Size(194, 20);
            this.baseLabel1.TabIndex = 108;
            this.baseLabel1.Text = "[ 씬 - 척 && 웨이퍼 ]   투입";
            this.baseLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnPacking
            // 
            this.btnPacking.BackColor = System.Drawing.Color.LightGray;
            this.btnPacking.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnPacking.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnPacking.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnPacking.Font = new System.Drawing.Font("나눔바른고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPacking.ForeColor = System.Drawing.Color.DarkRed;
            this.btnPacking.Location = new System.Drawing.Point(788, 49);
            this.btnPacking.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnPacking.Name = "btnPacking";
            this.btnPacking.Size = new System.Drawing.Size(186, 130);
            this.btnPacking.TabIndex = 66;
            this.btnPacking.Text = "[ 프로브  카드 ]\r\n[ 웨이퍼 ]\r\n\r\n패킹  시작\r\n";
            this.btnPacking.UseVisualStyleBackColor = false;
            this.btnPacking.Click += new System.EventHandler(this.btnPacking_Click);
            // 
            // btnProbeCardLocking
            // 
            this.btnProbeCardLocking.BackColor = System.Drawing.Color.LightGray;
            this.btnProbeCardLocking.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnProbeCardLocking.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnProbeCardLocking.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnProbeCardLocking.Font = new System.Drawing.Font("나눔바른고딕", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnProbeCardLocking.ForeColor = System.Drawing.Color.DarkRed;
            this.btnProbeCardLocking.Location = new System.Drawing.Point(388, 49);
            this.btnProbeCardLocking.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnProbeCardLocking.Name = "btnProbeCardLocking";
            this.btnProbeCardLocking.Size = new System.Drawing.Size(137, 65);
            this.btnProbeCardLocking.TabIndex = 65;
            this.btnProbeCardLocking.Text = "[ 프로브  카드 ]\r\n고정 작업 시작";
            this.btnProbeCardLocking.UseVisualStyleBackColor = false;
            this.btnProbeCardLocking.Click += new System.EventHandler(this.btnProbeCardLocking_Click);
            // 
            // btnProbeCardLoadingReady
            // 
            this.btnProbeCardLoadingReady.BackColor = System.Drawing.Color.LightGray;
            this.btnProbeCardLoadingReady.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnProbeCardLoadingReady.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnProbeCardLoadingReady.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnProbeCardLoadingReady.Font = new System.Drawing.Font("나눔바른고딕", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnProbeCardLoadingReady.ForeColor = System.Drawing.Color.DarkRed;
            this.btnProbeCardLoadingReady.Location = new System.Drawing.Point(243, 49);
            this.btnProbeCardLoadingReady.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnProbeCardLoadingReady.Name = "btnProbeCardLoadingReady";
            this.btnProbeCardLoadingReady.Size = new System.Drawing.Size(137, 65);
            this.btnProbeCardLoadingReady.TabIndex = 65;
            this.btnProbeCardLoadingReady.Text = "[ 프로브  카드 ]\r\n투입 위치 이동";
            this.btnProbeCardLoadingReady.UseVisualStyleBackColor = false;
            this.btnProbeCardLoadingReady.Click += new System.EventHandler(this.btnProbeCardLoadingReady_Click);
            // 
            // btnLoadingPos
            // 
            this.btnLoadingPos.BackColor = System.Drawing.Color.LightGray;
            this.btnLoadingPos.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnLoadingPos.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnLoadingPos.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnLoadingPos.Font = new System.Drawing.Font("나눔바른고딕", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLoadingPos.ForeColor = System.Drawing.Color.DarkRed;
            this.btnLoadingPos.Location = new System.Drawing.Point(13, 49);
            this.btnLoadingPos.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnLoadingPos.Name = "btnLoadingPos";
            this.btnLoadingPos.Size = new System.Drawing.Size(194, 105);
            this.btnLoadingPos.TabIndex = 65;
            this.btnLoadingPos.Text = "[ 씬 - 척 && 웨이퍼 ]\r\n\r\n투입 위치 이동";
            this.btnLoadingPos.UseVisualStyleBackColor = false;
            this.btnLoadingPos.Click += new System.EventHandler(this.btnLoadingPos_Click);
            // 
            // btnMainWork_Start
            // 
            this.btnMainWork_Start.BackColor = System.Drawing.Color.LightGray;
            this.btnMainWork_Start.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnMainWork_Start.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnMainWork_Start.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnMainWork_Start.Font = new System.Drawing.Font("나눔바른고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnMainWork_Start.ForeColor = System.Drawing.Color.DarkRed;
            this.btnMainWork_Start.Location = new System.Drawing.Point(564, 49);
            this.btnMainWork_Start.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnMainWork_Start.Name = "btnMainWork_Start";
            this.btnMainWork_Start.Size = new System.Drawing.Size(186, 130);
            this.btnMainWork_Start.TabIndex = 62;
            this.btnMainWork_Start.Text = "[ 프로브  카드 ]\r\n[ 웨이퍼 ]\r\n\r\n얼라인  시작";
            this.btnMainWork_Start.UseVisualStyleBackColor = false;
            this.btnMainWork_Start.Click += new System.EventHandler(this.btnMainWork_Start_Click_1);
            // 
            // tabControl_ProbeCard_ClampType
            // 
            this.tabControl_ProbeCard_ClampType.Controls.Add(this.tabPage_TypeA);
            this.tabControl_ProbeCard_ClampType.Controls.Add(this.tabPage_TypeB);
            this.tabControl_ProbeCard_ClampType.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tabControl_ProbeCard_ClampType.ItemSize = new System.Drawing.Size(52, 20);
            this.tabControl_ProbeCard_ClampType.Location = new System.Drawing.Point(243, 128);
            this.tabControl_ProbeCard_ClampType.Name = "tabControl_ProbeCard_ClampType";
            this.tabControl_ProbeCard_ClampType.SelectedIndex = 0;
            this.tabControl_ProbeCard_ClampType.Size = new System.Drawing.Size(282, 161);
            this.tabControl_ProbeCard_ClampType.TabIndex = 190;
            // 
            // tabPage_TypeA
            // 
            this.tabPage_TypeA.BackColor = System.Drawing.Color.Cornsilk;
            this.tabPage_TypeA.Controls.Add(this.pictureBoxProbeBWDetect);
            this.tabPage_TypeA.Controls.Add(this.baseLabelProbeBWDetect);
            this.tabPage_TypeA.Controls.Add(this.pictureBoxTopCoverDown);
            this.tabPage_TypeA.Controls.Add(this.baseLabelTopCoverDown);
            this.tabPage_TypeA.Controls.Add(this.pictureBoxTopCoverUp);
            this.tabPage_TypeA.Controls.Add(this.baseLabelTopCoverUp);
            this.tabPage_TypeA.Controls.Add(this.baseButtonTopCoverDown);
            this.tabPage_TypeA.Controls.Add(this.baseButtonTopCoverUp);
            this.tabPage_TypeA.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tabPage_TypeA.Location = new System.Drawing.Point(4, 24);
            this.tabPage_TypeA.Name = "tabPage_TypeA";
            this.tabPage_TypeA.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_TypeA.Size = new System.Drawing.Size(274, 133);
            this.tabPage_TypeA.TabIndex = 0;
            this.tabPage_TypeA.Text = "    Type - A  ";
            // 
            // pictureBoxProbeBWDetect
            // 
            this.pictureBoxProbeBWDetect.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
            this.pictureBoxProbeBWDetect.Location = new System.Drawing.Point(3, 13);
            this.pictureBoxProbeBWDetect.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBoxProbeBWDetect.Name = "pictureBoxProbeBWDetect";
            this.pictureBoxProbeBWDetect.Size = new System.Drawing.Size(22, 22);
            this.pictureBoxProbeBWDetect.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxProbeBWDetect.TabIndex = 158;
            this.pictureBoxProbeBWDetect.TabStop = false;
            // 
            // baseLabelProbeBWDetect
            // 
            this.baseLabelProbeBWDetect.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelProbeBWDetect.ForeColor = System.Drawing.Color.Black;
            this.baseLabelProbeBWDetect.Location = new System.Drawing.Point(28, 9);
            this.baseLabelProbeBWDetect.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabelProbeBWDetect.Name = "baseLabelProbeBWDetect";
            this.baseLabelProbeBWDetect.Size = new System.Drawing.Size(113, 33);
            this.baseLabelProbeBWDetect.TabIndex = 157;
            this.baseLabelProbeBWDetect.Text = "프로브 카드 트레이 감지";
            this.baseLabelProbeBWDetect.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBoxTopCoverDown
            // 
            this.pictureBoxTopCoverDown.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
            this.pictureBoxTopCoverDown.Location = new System.Drawing.Point(3, 100);
            this.pictureBoxTopCoverDown.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBoxTopCoverDown.Name = "pictureBoxTopCoverDown";
            this.pictureBoxTopCoverDown.Size = new System.Drawing.Size(22, 22);
            this.pictureBoxTopCoverDown.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxTopCoverDown.TabIndex = 156;
            this.pictureBoxTopCoverDown.TabStop = false;
            // 
            // baseLabelTopCoverDown
            // 
            this.baseLabelTopCoverDown.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelTopCoverDown.ForeColor = System.Drawing.Color.Black;
            this.baseLabelTopCoverDown.Location = new System.Drawing.Point(28, 96);
            this.baseLabelTopCoverDown.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabelTopCoverDown.Name = "baseLabelTopCoverDown";
            this.baseLabelTopCoverDown.Size = new System.Drawing.Size(127, 33);
            this.baseLabelTopCoverDown.TabIndex = 155;
            this.baseLabelTopCoverDown.Text = "프로브 카드 커버 하강";
            this.baseLabelTopCoverDown.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBoxTopCoverUp
            // 
            this.pictureBoxTopCoverUp.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
            this.pictureBoxTopCoverUp.Location = new System.Drawing.Point(3, 72);
            this.pictureBoxTopCoverUp.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBoxTopCoverUp.Name = "pictureBoxTopCoverUp";
            this.pictureBoxTopCoverUp.Size = new System.Drawing.Size(22, 22);
            this.pictureBoxTopCoverUp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxTopCoverUp.TabIndex = 154;
            this.pictureBoxTopCoverUp.TabStop = false;
            // 
            // baseLabelTopCoverUp
            // 
            this.baseLabelTopCoverUp.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelTopCoverUp.ForeColor = System.Drawing.Color.Black;
            this.baseLabelTopCoverUp.Location = new System.Drawing.Point(28, 68);
            this.baseLabelTopCoverUp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabelTopCoverUp.Name = "baseLabelTopCoverUp";
            this.baseLabelTopCoverUp.Size = new System.Drawing.Size(127, 33);
            this.baseLabelTopCoverUp.TabIndex = 153;
            this.baseLabelTopCoverUp.Text = "프로브 카드 커버 상승";
            this.baseLabelTopCoverUp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // baseButtonTopCoverDown
            // 
            this.baseButtonTopCoverDown.BackColor = System.Drawing.Color.LightGray;
            this.baseButtonTopCoverDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonTopCoverDown.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButtonTopCoverDown.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButtonTopCoverDown.Location = new System.Drawing.Point(157, 70);
            this.baseButtonTopCoverDown.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseButtonTopCoverDown.Name = "baseButtonTopCoverDown";
            this.baseButtonTopCoverDown.Size = new System.Drawing.Size(114, 60);
            this.baseButtonTopCoverDown.TabIndex = 152;
            this.baseButtonTopCoverDown.Text = "[ 프로브 카드 ]\r\n고정 커버 내림";
            this.baseButtonTopCoverDown.UseVisualStyleBackColor = false;
            this.baseButtonTopCoverDown.Click += new System.EventHandler(this.baseButtonTopCoverDown_Click);
            // 
            // baseButtonTopCoverUp
            // 
            this.baseButtonTopCoverUp.BackColor = System.Drawing.Color.LightGray;
            this.baseButtonTopCoverUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonTopCoverUp.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButtonTopCoverUp.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButtonTopCoverUp.Location = new System.Drawing.Point(157, 6);
            this.baseButtonTopCoverUp.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseButtonTopCoverUp.Name = "baseButtonTopCoverUp";
            this.baseButtonTopCoverUp.Size = new System.Drawing.Size(114, 60);
            this.baseButtonTopCoverUp.TabIndex = 151;
            this.baseButtonTopCoverUp.Text = "[ 프로브 카드 ]\r\n고정 커버 올림";
            this.baseButtonTopCoverUp.UseVisualStyleBackColor = false;
            this.baseButtonTopCoverUp.Click += new System.EventHandler(this.baseButtonTopCoverUp_Click);
            // 
            // tabPage_TypeB
            // 
            this.tabPage_TypeB.BackColor = System.Drawing.Color.Cornsilk;
            this.tabPage_TypeB.Controls.Add(this.baseButtonProbeClamp_BW);
            this.tabPage_TypeB.Controls.Add(this.baseButtonProbeClamp_FW);
            this.tabPage_TypeB.Controls.Add(this.baseButtonProbeUnpackingCyl_Down);
            this.tabPage_TypeB.Controls.Add(this.pictureBoxProbeRightClamp_FW);
            this.tabPage_TypeB.Controls.Add(this.pictureBoxProbeLeftClamp_FW);
            this.tabPage_TypeB.Controls.Add(this.pictureBoxProbeRightClamp_BW);
            this.tabPage_TypeB.Controls.Add(this.pictureBoxProbeLeftClamp_BW);
            this.tabPage_TypeB.Controls.Add(this.pictureBoxProbeUnpackingCyl_Down);
            this.tabPage_TypeB.Controls.Add(this.pictureBoxProbeUnpackingCyl_Up);
            this.tabPage_TypeB.Controls.Add(this.baseButtonProbeUnpackingCyl_Up);
            this.tabPage_TypeB.Controls.Add(this.baseButtonProbeClamp_Down);
            this.tabPage_TypeB.Controls.Add(this.baseLabel_BW);
            this.tabPage_TypeB.Controls.Add(this.baseLabel_FW);
            this.tabPage_TypeB.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tabPage_TypeB.Location = new System.Drawing.Point(4, 24);
            this.tabPage_TypeB.Name = "tabPage_TypeB";
            this.tabPage_TypeB.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_TypeB.Size = new System.Drawing.Size(274, 133);
            this.tabPage_TypeB.TabIndex = 1;
            this.tabPage_TypeB.Text = "    Type - B    ";
            // 
            // baseButtonProbeClamp_BW
            // 
            this.baseButtonProbeClamp_BW.BackColor = System.Drawing.Color.LightGray;
            this.baseButtonProbeClamp_BW.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonProbeClamp_BW.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButtonProbeClamp_BW.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButtonProbeClamp_BW.Location = new System.Drawing.Point(75, 92);
            this.baseButtonProbeClamp_BW.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseButtonProbeClamp_BW.Name = "baseButtonProbeClamp_BW";
            this.baseButtonProbeClamp_BW.Size = new System.Drawing.Size(65, 38);
            this.baseButtonProbeClamp_BW.TabIndex = 173;
            this.baseButtonProbeClamp_BW.Text = "클램프\r\n열기";
            this.baseButtonProbeClamp_BW.UseVisualStyleBackColor = false;
            this.baseButtonProbeClamp_BW.Click += new System.EventHandler(this.baseButtonProbeClamp_BW_Click);
            // 
            // baseButtonProbeClamp_FW
            // 
            this.baseButtonProbeClamp_FW.BackColor = System.Drawing.Color.LightGray;
            this.baseButtonProbeClamp_FW.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonProbeClamp_FW.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButtonProbeClamp_FW.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButtonProbeClamp_FW.Location = new System.Drawing.Point(7, 92);
            this.baseButtonProbeClamp_FW.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseButtonProbeClamp_FW.Name = "baseButtonProbeClamp_FW";
            this.baseButtonProbeClamp_FW.Size = new System.Drawing.Size(65, 38);
            this.baseButtonProbeClamp_FW.TabIndex = 172;
            this.baseButtonProbeClamp_FW.Text = "클램프\r\n닫기";
            this.baseButtonProbeClamp_FW.UseVisualStyleBackColor = false;
            this.baseButtonProbeClamp_FW.Click += new System.EventHandler(this.baseButtonProbeClamp_FW_Click);
            // 
            // baseButtonProbeUnpackingCyl_Down
            // 
            this.baseButtonProbeUnpackingCyl_Down.BackColor = System.Drawing.Color.LightGray;
            this.baseButtonProbeUnpackingCyl_Down.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonProbeUnpackingCyl_Down.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButtonProbeUnpackingCyl_Down.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButtonProbeUnpackingCyl_Down.Location = new System.Drawing.Point(179, 70);
            this.baseButtonProbeUnpackingCyl_Down.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseButtonProbeUnpackingCyl_Down.Name = "baseButtonProbeUnpackingCyl_Down";
            this.baseButtonProbeUnpackingCyl_Down.Size = new System.Drawing.Size(92, 60);
            this.baseButtonProbeUnpackingCyl_Down.TabIndex = 171;
            this.baseButtonProbeUnpackingCyl_Down.Text = "프로브 카드\r\n언패킹 실린더\r\n내림";
            this.baseButtonProbeUnpackingCyl_Down.UseVisualStyleBackColor = false;
            this.baseButtonProbeUnpackingCyl_Down.Click += new System.EventHandler(this.baseButtonProbeUnpackingCyl_Down_Click);
            // 
            // pictureBoxProbeRightClamp_FW
            // 
            this.pictureBoxProbeRightClamp_FW.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
            this.pictureBoxProbeRightClamp_FW.Location = new System.Drawing.Point(99, 68);
            this.pictureBoxProbeRightClamp_FW.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBoxProbeRightClamp_FW.Name = "pictureBoxProbeRightClamp_FW";
            this.pictureBoxProbeRightClamp_FW.Size = new System.Drawing.Size(22, 22);
            this.pictureBoxProbeRightClamp_FW.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxProbeRightClamp_FW.TabIndex = 170;
            this.pictureBoxProbeRightClamp_FW.TabStop = false;
            // 
            // pictureBoxProbeLeftClamp_FW
            // 
            this.pictureBoxProbeLeftClamp_FW.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
            this.pictureBoxProbeLeftClamp_FW.Location = new System.Drawing.Point(25, 68);
            this.pictureBoxProbeLeftClamp_FW.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBoxProbeLeftClamp_FW.Name = "pictureBoxProbeLeftClamp_FW";
            this.pictureBoxProbeLeftClamp_FW.Size = new System.Drawing.Size(22, 22);
            this.pictureBoxProbeLeftClamp_FW.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxProbeLeftClamp_FW.TabIndex = 169;
            this.pictureBoxProbeLeftClamp_FW.TabStop = false;
            // 
            // pictureBoxProbeRightClamp_BW
            // 
            this.pictureBoxProbeRightClamp_BW.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
            this.pictureBoxProbeRightClamp_BW.Location = new System.Drawing.Point(122, 68);
            this.pictureBoxProbeRightClamp_BW.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBoxProbeRightClamp_BW.Name = "pictureBoxProbeRightClamp_BW";
            this.pictureBoxProbeRightClamp_BW.Size = new System.Drawing.Size(22, 22);
            this.pictureBoxProbeRightClamp_BW.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxProbeRightClamp_BW.TabIndex = 168;
            this.pictureBoxProbeRightClamp_BW.TabStop = false;
            // 
            // pictureBoxProbeLeftClamp_BW
            // 
            this.pictureBoxProbeLeftClamp_BW.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
            this.pictureBoxProbeLeftClamp_BW.Location = new System.Drawing.Point(2, 68);
            this.pictureBoxProbeLeftClamp_BW.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBoxProbeLeftClamp_BW.Name = "pictureBoxProbeLeftClamp_BW";
            this.pictureBoxProbeLeftClamp_BW.Size = new System.Drawing.Size(22, 22);
            this.pictureBoxProbeLeftClamp_BW.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxProbeLeftClamp_BW.TabIndex = 166;
            this.pictureBoxProbeLeftClamp_BW.TabStop = false;
            // 
            // pictureBoxProbeUnpackingCyl_Down
            // 
            this.pictureBoxProbeUnpackingCyl_Down.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
            this.pictureBoxProbeUnpackingCyl_Down.Location = new System.Drawing.Point(156, 108);
            this.pictureBoxProbeUnpackingCyl_Down.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBoxProbeUnpackingCyl_Down.Name = "pictureBoxProbeUnpackingCyl_Down";
            this.pictureBoxProbeUnpackingCyl_Down.Size = new System.Drawing.Size(22, 22);
            this.pictureBoxProbeUnpackingCyl_Down.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxProbeUnpackingCyl_Down.TabIndex = 164;
            this.pictureBoxProbeUnpackingCyl_Down.TabStop = false;
            // 
            // pictureBoxProbeUnpackingCyl_Up
            // 
            this.pictureBoxProbeUnpackingCyl_Up.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
            this.pictureBoxProbeUnpackingCyl_Up.Location = new System.Drawing.Point(156, 6);
            this.pictureBoxProbeUnpackingCyl_Up.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBoxProbeUnpackingCyl_Up.Name = "pictureBoxProbeUnpackingCyl_Up";
            this.pictureBoxProbeUnpackingCyl_Up.Size = new System.Drawing.Size(22, 22);
            this.pictureBoxProbeUnpackingCyl_Up.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxProbeUnpackingCyl_Up.TabIndex = 162;
            this.pictureBoxProbeUnpackingCyl_Up.TabStop = false;
            // 
            // baseButtonProbeUnpackingCyl_Up
            // 
            this.baseButtonProbeUnpackingCyl_Up.BackColor = System.Drawing.Color.LightGray;
            this.baseButtonProbeUnpackingCyl_Up.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonProbeUnpackingCyl_Up.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButtonProbeUnpackingCyl_Up.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButtonProbeUnpackingCyl_Up.Location = new System.Drawing.Point(179, 6);
            this.baseButtonProbeUnpackingCyl_Up.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseButtonProbeUnpackingCyl_Up.Name = "baseButtonProbeUnpackingCyl_Up";
            this.baseButtonProbeUnpackingCyl_Up.Size = new System.Drawing.Size(92, 60);
            this.baseButtonProbeUnpackingCyl_Up.TabIndex = 160;
            this.baseButtonProbeUnpackingCyl_Up.Text = "프로브 카드\r\n언패킹 실린더\r\n올림";
            this.baseButtonProbeUnpackingCyl_Up.UseVisualStyleBackColor = false;
            this.baseButtonProbeUnpackingCyl_Up.Click += new System.EventHandler(this.baseButtonProbeUnpackingCyl_Up_Click);
            // 
            // baseButtonProbeClamp_Down
            // 
            this.baseButtonProbeClamp_Down.BackColor = System.Drawing.Color.LightGray;
            this.baseButtonProbeClamp_Down.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonProbeClamp_Down.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButtonProbeClamp_Down.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButtonProbeClamp_Down.Location = new System.Drawing.Point(7, 6);
            this.baseButtonProbeClamp_Down.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseButtonProbeClamp_Down.Name = "baseButtonProbeClamp_Down";
            this.baseButtonProbeClamp_Down.Size = new System.Drawing.Size(133, 41);
            this.baseButtonProbeClamp_Down.TabIndex = 159;
            this.baseButtonProbeClamp_Down.Text = "프로브 카드 클램프\r\n내림    [Off: 올림]";
            this.baseButtonProbeClamp_Down.UseVisualStyleBackColor = false;
            this.baseButtonProbeClamp_Down.Click += new System.EventHandler(this.baseButtonProbeClamp_Down_Click);
            // 
            // baseLabel_BW
            // 
            this.baseLabel_BW.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_BW.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_BW.Location = new System.Drawing.Point(1, 52);
            this.baseLabel_BW.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_BW.Name = "baseLabel_BW";
            this.baseLabel_BW.Size = new System.Drawing.Size(143, 19);
            this.baseLabel_BW.TabIndex = 167;
            this.baseLabel_BW.Text = "┏─── Open ───┓";
            this.baseLabel_BW.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel_FW
            // 
            this.baseLabel_FW.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_FW.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_FW.Location = new System.Drawing.Point(44, 71);
            this.baseLabel_FW.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_FW.Name = "baseLabel_FW";
            this.baseLabel_FW.Size = new System.Drawing.Size(58, 19);
            this.baseLabel_FW.TabIndex = 165;
            this.baseLabel_FW.Text = "- Close -";
            this.baseLabel_FW.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseGroupBox_ManualButton
            // 
            this.baseGroupBox_ManualButton.Controls.Add(this.baseButtonProbeUnpacking);
            this.baseGroupBox_ManualButton.Controls.Add(this.baseButtonProbePacking);
            this.baseGroupBox_ManualButton.Controls.Add(this.baseButtonThinChuckStageCleaning);
            this.baseGroupBox_ManualButton.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_ManualButton.ForeColor = System.Drawing.Color.White;
            this.baseGroupBox_ManualButton.Location = new System.Drawing.Point(237, 638);
            this.baseGroupBox_ManualButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseGroupBox_ManualButton.Name = "baseGroupBox_ManualButton";
            this.baseGroupBox_ManualButton.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseGroupBox_ManualButton.Size = new System.Drawing.Size(313, 121);
            this.baseGroupBox_ManualButton.TabIndex = 125;
            this.baseGroupBox_ManualButton.TabStop = false;
            this.baseGroupBox_ManualButton.Text = " [ 수동 동작 (IO 신호) ] ";
            // 
            // baseButtonProbeUnpacking
            // 
            this.baseButtonProbeUnpacking.BackColor = System.Drawing.Color.LightGray;
            this.baseButtonProbeUnpacking.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonProbeUnpacking.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButtonProbeUnpacking.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButtonProbeUnpacking.Location = new System.Drawing.Point(211, 30);
            this.baseButtonProbeUnpacking.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseButtonProbeUnpacking.Name = "baseButtonProbeUnpacking";
            this.baseButtonProbeUnpacking.Size = new System.Drawing.Size(93, 79);
            this.baseButtonProbeUnpacking.TabIndex = 128;
            this.baseButtonProbeUnpacking.Text = "프로브\r\n언패킹 공압\r\nOn";
            this.baseButtonProbeUnpacking.UseVisualStyleBackColor = false;
            this.baseButtonProbeUnpacking.Click += new System.EventHandler(this.baseButtonProbeUnpacking_Click);
            // 
            // baseButtonProbePacking
            // 
            this.baseButtonProbePacking.BackColor = System.Drawing.Color.LightGray;
            this.baseButtonProbePacking.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonProbePacking.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButtonProbePacking.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButtonProbePacking.Location = new System.Drawing.Point(111, 30);
            this.baseButtonProbePacking.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseButtonProbePacking.Name = "baseButtonProbePacking";
            this.baseButtonProbePacking.Size = new System.Drawing.Size(93, 79);
            this.baseButtonProbePacking.TabIndex = 127;
            this.baseButtonProbePacking.Text = "프로브\r\n패킹 공압\r\nOn";
            this.baseButtonProbePacking.UseVisualStyleBackColor = false;
            this.baseButtonProbePacking.Click += new System.EventHandler(this.baseButtonProbePacking_Click);
            // 
            // baseButtonThinChuckStageCleaning
            // 
            this.baseButtonThinChuckStageCleaning.BackColor = System.Drawing.Color.LightGray;
            this.baseButtonThinChuckStageCleaning.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonThinChuckStageCleaning.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButtonThinChuckStageCleaning.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButtonThinChuckStageCleaning.Location = new System.Drawing.Point(11, 30);
            this.baseButtonThinChuckStageCleaning.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseButtonThinChuckStageCleaning.Name = "baseButtonThinChuckStageCleaning";
            this.baseButtonThinChuckStageCleaning.Size = new System.Drawing.Size(93, 79);
            this.baseButtonThinChuckStageCleaning.TabIndex = 125;
            this.baseButtonThinChuckStageCleaning.Text = "씬 - 척\r\n\r\n클리닝";
            this.baseButtonThinChuckStageCleaning.UseVisualStyleBackColor = false;
            this.baseButtonThinChuckStageCleaning.Click += new System.EventHandler(this.baseButtonThinChuckStageCleaning_Click);
            // 
            // baseLabelTeachingImage_Lower1
            // 
            this.baseLabelTeachingImage_Lower1.BackColor = System.Drawing.Color.Black;
            this.baseLabelTeachingImage_Lower1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabelTeachingImage_Lower1.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabelTeachingImage_Lower1.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabelTeachingImage_Lower1.Location = new System.Drawing.Point(667, 452);
            this.baseLabelTeachingImage_Lower1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabelTeachingImage_Lower1.Name = "baseLabelTeachingImage_Lower1";
            this.baseLabelTeachingImage_Lower1.Size = new System.Drawing.Size(61, 49);
            this.baseLabelTeachingImage_Lower1.TabIndex = 117;
            this.baseLabelTeachingImage_Lower1.Text = "티칭\r\n이미지";
            this.baseLabelTeachingImage_Lower1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabelTeachingImage_Upper1
            // 
            this.baseLabelTeachingImage_Upper1.BackColor = System.Drawing.Color.Black;
            this.baseLabelTeachingImage_Upper1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabelTeachingImage_Upper1.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabelTeachingImage_Upper1.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabelTeachingImage_Upper1.Location = new System.Drawing.Point(156, 452);
            this.baseLabelTeachingImage_Upper1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabelTeachingImage_Upper1.Name = "baseLabelTeachingImage_Upper1";
            this.baseLabelTeachingImage_Upper1.Size = new System.Drawing.Size(61, 49);
            this.baseLabelTeachingImage_Upper1.TabIndex = 116;
            this.baseLabelTeachingImage_Upper1.Text = "티칭\r\n이미지";
            this.baseLabelTeachingImage_Upper1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel_WaferChuck_Camera1
            // 
            this.baseLabel_WaferChuck_Camera1.BackColor = System.Drawing.Color.Black;
            this.baseLabel_WaferChuck_Camera1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabel_WaferChuck_Camera1.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_WaferChuck_Camera1.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel_WaferChuck_Camera1.Location = new System.Drawing.Point(710, 0);
            this.baseLabel_WaferChuck_Camera1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_WaferChuck_Camera1.Name = "baseLabel_WaferChuck_Camera1";
            this.baseLabel_WaferChuck_Camera1.Size = new System.Drawing.Size(113, 25);
            this.baseLabel_WaferChuck_Camera1.TabIndex = 115;
            this.baseLabel_WaferChuck_Camera1.Text = "[ 웨이퍼 ]";
            this.baseLabel_WaferChuck_Camera1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel_ProbeCard_Camera1
            // 
            this.baseLabel_ProbeCard_Camera1.BackColor = System.Drawing.Color.Black;
            this.baseLabel_ProbeCard_Camera1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabel_ProbeCard_Camera1.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_ProbeCard_Camera1.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel_ProbeCard_Camera1.Location = new System.Drawing.Point(199, 0);
            this.baseLabel_ProbeCard_Camera1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_ProbeCard_Camera1.Name = "baseLabel_ProbeCard_Camera1";
            this.baseLabel_ProbeCard_Camera1.Size = new System.Drawing.Size(113, 25);
            this.baseLabel_ProbeCard_Camera1.TabIndex = 114;
            this.baseLabel_ProbeCard_Camera1.Text = "[ 프로브 카드 ]";
            this.baseLabel_ProbeCard_Camera1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseGroupBox1
            // 
            this.baseGroupBox1.Controls.Add(this.baseButtonChangeRecipe);
            this.baseGroupBox1.Controls.Add(this.baseTextBoxCurrentRecipe);
            this.baseGroupBox1.Controls.Add(this.baseLabelCurrentRecipe);
            this.baseGroupBox1.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox1.ForeColor = System.Drawing.Color.White;
            this.baseGroupBox1.Location = new System.Drawing.Point(1363, 0);
            this.baseGroupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseGroupBox1.Name = "baseGroupBox1";
            this.baseGroupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseGroupBox1.Size = new System.Drawing.Size(541, 75);
            this.baseGroupBox1.TabIndex = 65;
            this.baseGroupBox1.TabStop = false;
            this.baseGroupBox1.Text = " [ 레시피 ] ";
            // 
            // baseButtonChangeRecipe
            // 
            this.baseButtonChangeRecipe.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonChangeRecipe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonChangeRecipe.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseButtonChangeRecipe.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonChangeRecipe.Location = new System.Drawing.Point(461, 23);
            this.baseButtonChangeRecipe.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseButtonChangeRecipe.Name = "baseButtonChangeRecipe";
            this.baseButtonChangeRecipe.Size = new System.Drawing.Size(68, 41);
            this.baseButtonChangeRecipe.TabIndex = 1;
            this.baseButtonChangeRecipe.Text = "선택";
            this.baseButtonChangeRecipe.UseVisualStyleBackColor = false;
            this.baseButtonChangeRecipe.Click += new System.EventHandler(this.baseButtonChangeRecipe_Click);
            // 
            // baseTextBoxCurrentRecipe
            // 
            this.baseTextBoxCurrentRecipe.BackColor = System.Drawing.Color.Khaki;
            this.baseTextBoxCurrentRecipe.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseTextBoxCurrentRecipe.Font = new System.Drawing.Font("나눔바른고딕", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseTextBoxCurrentRecipe.ForeColor = System.Drawing.Color.Black;
            this.baseTextBoxCurrentRecipe.Location = new System.Drawing.Point(105, 23);
            this.baseTextBoxCurrentRecipe.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseTextBoxCurrentRecipe.Name = "baseTextBoxCurrentRecipe";
            this.baseTextBoxCurrentRecipe.ReadOnly = true;
            this.baseTextBoxCurrentRecipe.Size = new System.Drawing.Size(353, 41);
            this.baseTextBoxCurrentRecipe.TabIndex = 2;
            this.baseTextBoxCurrentRecipe.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseLabelCurrentRecipe
            // 
            this.baseLabelCurrentRecipe.AutoSize = true;
            this.baseLabelCurrentRecipe.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelCurrentRecipe.ForeColor = System.Drawing.Color.White;
            this.baseLabelCurrentRecipe.Location = new System.Drawing.Point(8, 33);
            this.baseLabelCurrentRecipe.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabelCurrentRecipe.Name = "baseLabelCurrentRecipe";
            this.baseLabelCurrentRecipe.Size = new System.Drawing.Size(92, 22);
            this.baseLabelCurrentRecipe.TabIndex = 1;
            this.baseLabelCurrentRecipe.Text = "레시피 명 :";
            // 
            // lblMachine_Status
            // 
            this.lblMachine_Status.BackColor = System.Drawing.Color.Black;
            this.lblMachine_Status.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblMachine_Status.Font = new System.Drawing.Font("나눔바른고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMachine_Status.ForeColor = System.Drawing.Color.Yellow;
            this.lblMachine_Status.Location = new System.Drawing.Point(1363, 84);
            this.lblMachine_Status.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMachine_Status.Name = "lblMachine_Status";
            this.lblMachine_Status.Size = new System.Drawing.Size(541, 46);
            this.lblMachine_Status.TabIndex = 60;
            this.lblMachine_Status.Text = "- - -";
            this.lblMachine_Status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblStatus_ScannerPowerMeter_Connected
            // 
            this.lblStatus_ScannerPowerMeter_Connected.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblStatus_ScannerPowerMeter_Connected.ForeColor = System.Drawing.Color.White;
            this.lblStatus_ScannerPowerMeter_Connected.Location = new System.Drawing.Point(39, 69);
            this.lblStatus_ScannerPowerMeter_Connected.Name = "lblStatus_ScannerPowerMeter_Connected";
            this.lblStatus_ScannerPowerMeter_Connected.Size = new System.Drawing.Size(172, 18);
            this.lblStatus_ScannerPowerMeter_Connected.TabIndex = 37;
            this.lblStatus_ScannerPowerMeter_Connected.Text = "Power Meter #2  [Stage]";
            this.lblStatus_ScannerPowerMeter_Connected.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Monitoring_CWA150SA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.Controls.Add(this.tabControl_User);
            this.Controls.Add(this.groupBoxAlignCheckPosParameter);
            this.Controls.Add(this.lblLowerCamera_AlignData);
            this.Controls.Add(this.baseLabelAngle_Lower);
            this.Controls.Add(this.lblUpperCamera_AlignData);
            this.Controls.Add(this.baseLabelAngle_Upper);
            this.Controls.Add(this.groupBoxCameraOpticalAxisCheck);
            this.Controls.Add(this.btnUpperCamera_StartLive);
            this.Controls.Add(this.btnUpperCamera_Init);
            this.Controls.Add(this.btnHomeAll);
            this.Controls.Add(this.baseGroupBox_CycleButton);
            this.Controls.Add(this.baseGroupBox_ManualButton);
            this.Controls.Add(this.baseLabelTeachingImage_Lower1);
            this.Controls.Add(this.baseLabelTeachingImage_Upper1);
            this.Controls.Add(this.baseLabel_WaferChuck_Camera1);
            this.Controls.Add(this.baseLabel_ProbeCard_Camera1);
            this.Controls.Add(this.pictureBoxTrainImage_Lower);
            this.Controls.Add(this.pictureBoxTrainImage_Upper);
            this.Controls.Add(this.m_visionImageViewer_Lower);
            this.Controls.Add(this.m_visionImageViewer_Upper);
            this.Controls.Add(this.baseGroupBox1);
            this.Controls.Add(this.lblMachine_Status);
            this.Controls.Add(this.groupBoxMainPanel);
            this.Controls.Add(this.groupBoxStatus);
            this.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.Name = "Monitoring_CWA150SA";
            this.Size = new System.Drawing.Size(1910, 770);
            this.groupBoxStatus.ResumeLayout(false);
            this.groupBoxMainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMainVacuumCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMainAirCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTrainImage_Upper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTrainImage_Lower)).EndInit();
            this.groupBoxCameraOpticalAxisCheck.ResumeLayout(false);
            this.groupBoxAlignCheckPosParameter.ResumeLayout(false);
            this.groupBoxAlignCheckPosParameter.PerformLayout();
            this.tabControl_User.ResumeLayout(false);
            this.tabPage_ManualPacking.ResumeLayout(false);
            this.tabPage_PackingOffsetChange.ResumeLayout(false);
            this.tabPage_PackingOffsetChange.PerformLayout();
            this.tabPage_ReticlePositionChange.ResumeLayout(false);
            this.tabPage_ReticlePositionChange.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_visionImageViewer_Lower)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_visionImageViewer_Upper)).EndInit();
            this.baseGroupBox_CycleButton.ResumeLayout(false);
            this.baseGroupBox_CycleButton.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProbePackingCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWaferVacuumCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxThinChuckVacuumCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxThinChuckDetect)).EndInit();
            this.tabControl_ProbeCard_ClampType.ResumeLayout(false);
            this.tabPage_TypeA.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProbeBWDetect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTopCoverDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTopCoverUp)).EndInit();
            this.tabPage_TypeB.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProbeRightClamp_FW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProbeLeftClamp_FW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProbeRightClamp_BW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProbeLeftClamp_BW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProbeUnpackingCyl_Down)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProbeUnpackingCyl_Up)).EndInit();
            this.baseGroupBox_ManualButton.ResumeLayout(false);
            this.baseGroupBox1.ResumeLayout(false);
            this.baseGroupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timer_DIO_Status;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.GroupBox groupBoxStatus;
        private System.Windows.Forms.Button ledStatus_Light_Connected;
        private BaseLabel lblStatus_Light_Connected;
        private System.Windows.Forms.GroupBox groupBoxMainPanel;
        private System.Windows.Forms.PictureBox pictureBoxMainAirCheck;
        private BaseLabel lblMainAirCheck;
        private BaseLabel lblStatus_ScannerPowerMeter_Connected;
        private BaseLabel lblMachine_Status;
        private BaseGroupBox baseGroupBox1;
        private BaseButton baseButtonChangeRecipe;
        private BaseTextBox baseTextBoxCurrentRecipe;
        private BaseLabel baseLabelCurrentRecipe;
        private System.Windows.Forms.PictureBox pictureBoxMainVacuumCheck;
        private BaseLabel lblMainVacuumCheck;
        private QMC.Common.Hmi.VisionImageViewer m_visionImageViewer_Upper;
        private QMC.Common.Hmi.VisionImageViewer m_visionImageViewer_Lower;
        private System.Windows.Forms.PictureBox pictureBoxTrainImage_Upper;
        private System.Windows.Forms.PictureBox pictureBoxTrainImage_Lower;
        private BaseLabel baseLabel_ProbeCard_Camera1;
        private BaseLabel baseLabel_WaferChuck_Camera1;
        private BaseLabel baseLabelTeachingImage_Upper1;
        private BaseLabel baseLabelTeachingImage_Lower1;
        private BaseGroupBox baseGroupBox_ManualButton;
        private BaseGroupBox baseGroupBox_CycleButton;
        private System.Windows.Forms.Button btnRunStatus_Drilling2;
        private System.Windows.Forms.Button btnRunStatus_Drilling;
        private System.Windows.Forms.Button btnMainWork_Start;
        private System.Windows.Forms.Button btnPacking;
        private System.Windows.Forms.Button btnLoadingPos;
        private System.Windows.Forms.Button btnHomeAll;
        private System.Windows.Forms.Button btnUpperCamera_Init;
        private System.Windows.Forms.Button btnUpperCamera_StartLive;
        private System.Windows.Forms.Button btnProbeCardLoadingReady;
        private System.Windows.Forms.Button btnProbeCardLocking;
        private System.Windows.Forms.GroupBox groupBoxCameraOpticalAxisCheck;
        private System.Windows.Forms.Button btnReticlePos_LowerCam_GO;
        private System.Windows.Forms.Button btnReticlePos_UpperCam_GO;
        private BaseLabel baseLabelAngle_Upper;
        private BaseLabel lblUpperCamera_AlignData;
        private BaseLabel lblLowerCamera_AlignData;
        private BaseLabel baseLabelAngle_Lower;
        private BaseLabel baseLabel1;
        private BaseButton baseButtonWaferVacuum;
        private BaseButton baseButtonThinChuckVacuum;
        private BaseLabel baseLabel2;
        private BaseLabel baseLabel_Align;
        private BaseLabel baseLabel_Packing;
        private BaseLabel baseLabel7;
        private BaseLabel baseLabel6;
        private BaseLabel baseLabel5;
        private BaseButton baseButtonProbeUnpacking;
        private BaseButton baseButtonProbePacking;
        private BaseButton baseButtonThinChuckStageCleaning;
        private System.Windows.Forms.GroupBox groupBoxAlignCheckPosParameter;
        private BaseLabel baseLabelPosition_Bot;
        private BaseLabel baseLabelPosition_Mid;
        private BaseLabel baseLabelPosition_Top;
        private BaseButton baseButton_Y_Pos_GO3;
        private BaseButton baseButton_Y_Pos_GO2;
        private BaseButton baseButton_Y_Pos_GO1;
        private BaseLabel baseLabel18;
        private BaseLabel baseLabel9;
        private BaseLabel baseLabel_OK;
        private BaseLabel lblLowerCamera_Top_ErrorData_Y;
        private BaseLabel lblLowerCamera_Top_ErrorData_X;
        private BaseLabel baseLabel_Delta_X;
        private BaseLabel baseLabel16;
        private BaseLabel lblLowerCamera_Bot_ErrorData_Y;
        private BaseLabel lblLowerCamera_Bot_ErrorData_X;
        private BaseLabel baseLabel20;
        private BaseLabel baseLabel10;
        private BaseLabel lblLowerCamera_Mid_ErrorData_Y;
        private BaseLabel lblLowerCamera_Mid_ErrorData_X;
        private BaseLabel baseLabel13;
        private BaseLabel baseLabel_LowerCam;
        private BaseLabel baseLabel_Delta_Y;
        private BaseLabel baseLabel19;
        private BaseLabel baseLabel17;
        private BaseLabel baseLabel15;
        private BaseLabel baseLabel14;
        private BaseLabel baseLabel12;
        private BaseLabel baseLabel11;
        private System.Windows.Forms.PictureBox pictureBoxWaferVacuumCheck;
        private BaseLabel lblWaferVacuumCheck;
        private System.Windows.Forms.PictureBox pictureBoxThinChuckVacuumCheck;
        private BaseLabel lblThinChuckVacuumCheck;
        private System.Windows.Forms.PictureBox pictureBoxThinChuckDetect;
        private BaseLabel baseLabelThinChuckDetect;
        private System.Windows.Forms.PictureBox pictureBoxProbePackingCheck;
        private BaseLabel lblProbePackingCheck;
        private System.Windows.Forms.TabControl tabControl_User;
        private System.Windows.Forms.TabPage tabPage_ManualPacking;
        private System.Windows.Forms.TabPage tabPage_PackingOffsetChange;
        private BaseLabel baseLabelManualPacking1;
        private BaseLabel baseLabelManualPacking1_Title;
        private BaseLabel baseLabelManualPacking2_Title;
        private BaseLabel baseLabelManualPacking2;
        private BaseLabel baseLabelManualPacking3;
        private BaseLabel baseLabelManualPacking3_Title;
        private System.Windows.Forms.Button btnManualPacking;
        private System.Windows.Forms.Button baseButton_Elev_GoPos_Wafer_ProbeCard_Packing;
        private System.Windows.Forms.Button baseButton_Elev_PitchMove_Down;
        private System.Windows.Forms.Button baseButton_Elev_PitchMove_Up;
        private System.Windows.Forms.Button baseButton_VisionXY_Pos_Get1;
        private BaseLabel baseLabelPackingOffsetChange2;
        private BaseLabel baseLabelPackingOffsetChange2_Title;
        private BaseLabel baseLabelPackingOffsetChange1_Title;
        private BaseLabel baseLabelPackingOffsetChange1;
        private System.Windows.Forms.Button baseButton_VisionXY_Pos_Get2;
        private BaseLabel baseLabelJogButtonComb_XY;
        private BaseLabel baseLabelJogButtonComb_Title;
        private System.Windows.Forms.Button buttonAxisXUpYDown;
        private System.Windows.Forms.Button buttonAxisYDown;
        private System.Windows.Forms.Button buttonAxisXUp;
        private System.Windows.Forms.Button buttonAxisXUpYUp;
        private System.Windows.Forms.Button buttonAxisXDownYDown;
        private System.Windows.Forms.Button buttonAxisYUp;
        private System.Windows.Forms.Button buttonAxisXDown;
        private System.Windows.Forms.Button buttonAxisXDownYUp;
        private System.Windows.Forms.RadioButton radioButton_Movement_001mm;
        private System.Windows.Forms.RadioButton radioButton_Movement_01mm;
        private System.Windows.Forms.Button baseButton_Elev_JogMove_Down;
        private System.Windows.Forms.Button baseButton_Elev_JogMove_Up;
        private BaseLabel baseLabel_PitchMove;
        private BaseLabel baseLabel_JogMove;
        private System.Windows.Forms.RadioButton radioButton_Movement_05mm;
        private System.Windows.Forms.CheckBox checkBox_Step;
        private System.Windows.Forms.CheckBox checkBox_Jog;
        private System.Windows.Forms.Button btnPAK_Leak_Check;
        private BaseLabel baseLabel21;
        private System.Windows.Forms.TabPage tabPage_ReticlePositionChange;
        private System.Windows.Forms.CheckBox checkBox_Reticle_Step;
        private System.Windows.Forms.CheckBox checkBox_Reticle_Jog;
        private System.Windows.Forms.RadioButton radioButton_Reticle_Movement_05mm;
        private System.Windows.Forms.RadioButton radioButton_Reticle_Movement_001mm;
        private System.Windows.Forms.RadioButton radioButton_Reticle_Movement_01mm;
        private System.Windows.Forms.Button buttonAxisXUpYDown_Reticle;
        private System.Windows.Forms.Button buttonAxisYDown_Reticle;
        private System.Windows.Forms.Button buttonAxisXUp_Reticle;
        private System.Windows.Forms.Button buttonAxisXUpYUp_Reticle;
        private System.Windows.Forms.Button buttonAxisXDownYDown_Reticle;
        private System.Windows.Forms.Button buttonAxisYUp_Reticle;
        private System.Windows.Forms.Button buttonAxisXDown_Reticle;
        private System.Windows.Forms.Button buttonAxisXDownYUp_Reticle;
        private BaseLabel baseLabel22;
        private BaseLabel baseLabel23;
        private BaseLabel baseLabelReticlePositionChange2;
        private BaseLabel baseLabelReticlePositionChange2_Title;
        private BaseLabel baseLabelReticlePositionChange1_Title;
        private BaseLabel baseLabelReticlePositionChange1;
        private System.Windows.Forms.Button baseButton_ReticleGlass_Pos_Get;
        private System.Windows.Forms.TabControl tabControl_ProbeCard_ClampType;
        private System.Windows.Forms.TabPage tabPage_TypeA;
        private System.Windows.Forms.TabPage tabPage_TypeB;
        private System.Windows.Forms.PictureBox pictureBoxProbeBWDetect;
        private BaseLabel baseLabelProbeBWDetect;
        private System.Windows.Forms.PictureBox pictureBoxTopCoverDown;
        private BaseLabel baseLabelTopCoverDown;
        private System.Windows.Forms.PictureBox pictureBoxTopCoverUp;
        private BaseLabel baseLabelTopCoverUp;
        private BaseButton baseButtonTopCoverDown;
        private BaseButton baseButtonTopCoverUp;
        private System.Windows.Forms.PictureBox pictureBoxProbeLeftClamp_BW;
        private BaseLabel baseLabel_FW;
        private System.Windows.Forms.PictureBox pictureBoxProbeUnpackingCyl_Down;
        private System.Windows.Forms.PictureBox pictureBoxProbeUnpackingCyl_Up;
        private BaseButton baseButtonProbeUnpackingCyl_Up;
        private BaseButton baseButtonProbeClamp_Down;
        private BaseLabel baseLabel_BW;
        private System.Windows.Forms.PictureBox pictureBoxProbeRightClamp_FW;
        private System.Windows.Forms.PictureBox pictureBoxProbeLeftClamp_FW;
        private System.Windows.Forms.PictureBox pictureBoxProbeRightClamp_BW;
        private BaseButton baseButtonProbeUnpackingCyl_Down;
        private BaseButton baseButtonProbeClamp_FW;
        private BaseButton baseButtonProbeClamp_BW;
        private BaseButton baseButton_CameraY_GoPos_Safety;
        private BaseLabel baseLabel8;
        private BaseButton baseButton_EmptyChip_XYPos_GO;
    }
}
