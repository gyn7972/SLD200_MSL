namespace CWA150SA_Onsemi300
{
    partial class ManualMode_CWA150SA
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
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.groupBoxDetectStatus = new System.Windows.Forms.GroupBox();
            this.pictureBoxMainVacuumCheck = new System.Windows.Forms.PictureBox();
            this.lblMainVacuumCheck = new CWA150SA_Onsemi300.BaseLabel();
            this.pictureBoxMainAirCheck = new System.Windows.Forms.PictureBox();
            this.lblMainAirCheck = new CWA150SA_Onsemi300.BaseLabel();
            this.pictureBoxProbeBWDetect = new System.Windows.Forms.PictureBox();
            this.baseLabelProbeBWDetect = new CWA150SA_Onsemi300.BaseLabel();
            this.pictureBoxThinChuckDetect = new System.Windows.Forms.PictureBox();
            this.baseLabelThinChuckDetect = new CWA150SA_Onsemi300.BaseLabel();
            this.btnUpperCamera_Init = new System.Windows.Forms.Button();
            this.groupBoxAlignCheckPosParameter = new System.Windows.Forms.GroupBox();
            this.baseButton_X_Pos_GO3 = new CWA150SA_Onsemi300.BaseButton();
            this.baseButton_X_Pos_GO2 = new CWA150SA_Onsemi300.BaseButton();
            this.baseButton_X_Pos_GO1 = new CWA150SA_Onsemi300.BaseButton();
            this.baseLabelPosition_RIGHT = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelPosition_Center = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelPosition_Left = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelPosition_Bot = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelPosition_Mid = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelPosition_Top = new CWA150SA_Onsemi300.BaseLabel();
            this.baseButton_Y_Pos_GO3 = new CWA150SA_Onsemi300.BaseButton();
            this.baseButton_Y_Pos_GO2 = new CWA150SA_Onsemi300.BaseButton();
            this.baseButton_Y_Pos_GO1 = new CWA150SA_Onsemi300.BaseButton();
            this.baseLabel18 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel19 = new CWA150SA_Onsemi300.BaseLabel();
            this.btnUpperCamera_StartLive = new System.Windows.Forms.Button();
            this.m_visionImageViewer_Lower = new QMC.Common.Hmi.VisionImageViewer();
            this.m_visionImageViewer_Upper = new QMC.Common.Hmi.VisionImageViewer();
            this.btnProbeCardAlignOnly_Start = new System.Windows.Forms.Button();
            this.btnWaferAlignOnly_Start = new System.Windows.Forms.Button();
            this.lblLowerCamera_AlignData = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelAngle_Lower = new CWA150SA_Onsemi300.BaseLabel();
            this.lblUpperCamera_AlignData = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelAngle_Upper = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel_WaferChuck_Camera1 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel_ProbeCard_Camera1 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseGroupBox_CycleButton = new CWA150SA_Onsemi300.BaseGroupBox();
            this.btnAlignErrorCheck = new System.Windows.Forms.Button();
            this.btnWaferProbeCardUnpacking = new System.Windows.Forms.Button();
            this.btnWaferProbeCardUnpackingReady = new System.Windows.Forms.Button();
            this.btn_ProbeCard_Locking = new System.Windows.Forms.Button();
            this.btnLoadingPos_ProbeCard_GO = new System.Windows.Forms.Button();
            this.btnLoadingPos_Wafer_GO = new System.Windows.Forms.Button();
            this.btnSafetyPos_CamXY_GO = new System.Windows.Forms.Button();
            this.btnReticlePos_LowerCam_GO = new System.Windows.Forms.Button();
            this.btnReticlePos_UpperCam_GO = new System.Windows.Forms.Button();
            this.btnPacking = new System.Windows.Forms.Button();
            this.btnRunStatus_Drilling2 = new System.Windows.Forms.Button();
            this.btnRunStatus_Drilling = new System.Windows.Forms.Button();
            this.btnMainWork_Start = new System.Windows.Forms.Button();
            this.baseGroupBox_ManualButton = new CWA150SA_Onsemi300.BaseGroupBox();
            this.pictureBoxProbePackingCheck = new System.Windows.Forms.PictureBox();
            this.pictureBoxTopCoverDown = new System.Windows.Forms.PictureBox();
            this.pictureBoxTopCoverUp = new System.Windows.Forms.PictureBox();
            this.pictureBoxWaferVacuumCheck = new System.Windows.Forms.PictureBox();
            this.pictureBoxThinChuckVacuumCheck = new System.Windows.Forms.PictureBox();
            this.baseButtonTopCoverDown = new CWA150SA_Onsemi300.BaseButton();
            this.baseButtonTopCoverUp = new CWA150SA_Onsemi300.BaseButton();
            this.baseButtonWaferVacuum = new CWA150SA_Onsemi300.BaseButton();
            this.baseButtonProbeUnpacking = new CWA150SA_Onsemi300.BaseButton();
            this.baseButtonProbePacking = new CWA150SA_Onsemi300.BaseButton();
            this.baseButtonThinChuckVacuum = new CWA150SA_Onsemi300.BaseButton();
            this.baseButtonThinChuckStageCleaning = new CWA150SA_Onsemi300.BaseButton();
            this.groupBoxDetectStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMainVacuumCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMainAirCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProbeBWDetect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxThinChuckDetect)).BeginInit();
            this.groupBoxAlignCheckPosParameter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_visionImageViewer_Lower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_visionImageViewer_Upper)).BeginInit();
            this.baseGroupBox_CycleButton.SuspendLayout();
            this.baseGroupBox_ManualButton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProbePackingCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTopCoverDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTopCoverUp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWaferVacuumCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxThinChuckVacuumCheck)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // groupBoxDetectStatus
            // 
            this.groupBoxDetectStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.groupBoxDetectStatus.Controls.Add(this.pictureBoxMainVacuumCheck);
            this.groupBoxDetectStatus.Controls.Add(this.lblMainVacuumCheck);
            this.groupBoxDetectStatus.Controls.Add(this.pictureBoxMainAirCheck);
            this.groupBoxDetectStatus.Controls.Add(this.lblMainAirCheck);
            this.groupBoxDetectStatus.Controls.Add(this.pictureBoxProbeBWDetect);
            this.groupBoxDetectStatus.Controls.Add(this.baseLabelProbeBWDetect);
            this.groupBoxDetectStatus.Controls.Add(this.pictureBoxThinChuckDetect);
            this.groupBoxDetectStatus.Controls.Add(this.baseLabelThinChuckDetect);
            this.groupBoxDetectStatus.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxDetectStatus.ForeColor = System.Drawing.Color.White;
            this.groupBoxDetectStatus.Location = new System.Drawing.Point(611, 460);
            this.groupBoxDetectStatus.Name = "groupBoxDetectStatus";
            this.groupBoxDetectStatus.Size = new System.Drawing.Size(439, 97);
            this.groupBoxDetectStatus.TabIndex = 129;
            this.groupBoxDetectStatus.TabStop = false;
            this.groupBoxDetectStatus.Text = " [ 센서 상태 확인 ] ";
            this.groupBoxDetectStatus.Enter += new System.EventHandler(this.groupBoxDetectStatus_Enter);
            // 
            // pictureBoxMainVacuumCheck
            // 
            this.pictureBoxMainVacuumCheck.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
            this.pictureBoxMainVacuumCheck.Location = new System.Drawing.Point(13, 62);
            this.pictureBoxMainVacuumCheck.Name = "pictureBoxMainVacuumCheck";
            this.pictureBoxMainVacuumCheck.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxMainVacuumCheck.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxMainVacuumCheck.TabIndex = 39;
            this.pictureBoxMainVacuumCheck.TabStop = false;
            // 
            // lblMainVacuumCheck
            // 
            this.lblMainVacuumCheck.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMainVacuumCheck.ForeColor = System.Drawing.Color.White;
            this.lblMainVacuumCheck.Location = new System.Drawing.Point(43, 63);
            this.lblMainVacuumCheck.Name = "lblMainVacuumCheck";
            this.lblMainVacuumCheck.Size = new System.Drawing.Size(163, 25);
            this.lblMainVacuumCheck.TabIndex = 38;
            this.lblMainVacuumCheck.Text = "메인 진공 감지";
            this.lblMainVacuumCheck.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblMainVacuumCheck.Click += new System.EventHandler(this.lblMainVacuumCheck_Click);
            // 
            // pictureBoxMainAirCheck
            // 
            this.pictureBoxMainAirCheck.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
            this.pictureBoxMainAirCheck.Location = new System.Drawing.Point(13, 31);
            this.pictureBoxMainAirCheck.Name = "pictureBoxMainAirCheck";
            this.pictureBoxMainAirCheck.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxMainAirCheck.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxMainAirCheck.TabIndex = 37;
            this.pictureBoxMainAirCheck.TabStop = false;
            // 
            // lblMainAirCheck
            // 
            this.lblMainAirCheck.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMainAirCheck.ForeColor = System.Drawing.Color.White;
            this.lblMainAirCheck.Location = new System.Drawing.Point(43, 32);
            this.lblMainAirCheck.Name = "lblMainAirCheck";
            this.lblMainAirCheck.Size = new System.Drawing.Size(137, 25);
            this.lblMainAirCheck.TabIndex = 36;
            this.lblMainAirCheck.Text = "메인 공압 감지";
            this.lblMainAirCheck.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBoxProbeBWDetect
            // 
            this.pictureBoxProbeBWDetect.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
            this.pictureBoxProbeBWDetect.Location = new System.Drawing.Point(221, 62);
            this.pictureBoxProbeBWDetect.Name = "pictureBoxProbeBWDetect";
            this.pictureBoxProbeBWDetect.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxProbeBWDetect.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxProbeBWDetect.TabIndex = 35;
            this.pictureBoxProbeBWDetect.TabStop = false;
            // 
            // baseLabelProbeBWDetect
            // 
            this.baseLabelProbeBWDetect.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabelProbeBWDetect.ForeColor = System.Drawing.Color.White;
            this.baseLabelProbeBWDetect.Location = new System.Drawing.Point(251, 63);
            this.baseLabelProbeBWDetect.Name = "baseLabelProbeBWDetect";
            this.baseLabelProbeBWDetect.Size = new System.Drawing.Size(136, 25);
            this.baseLabelProbeBWDetect.TabIndex = 34;
            this.baseLabelProbeBWDetect.Text = "프로브 트레이 감지";
            this.baseLabelProbeBWDetect.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBoxThinChuckDetect
            // 
            this.pictureBoxThinChuckDetect.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
            this.pictureBoxThinChuckDetect.Location = new System.Drawing.Point(221, 31);
            this.pictureBoxThinChuckDetect.Name = "pictureBoxThinChuckDetect";
            this.pictureBoxThinChuckDetect.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxThinChuckDetect.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxThinChuckDetect.TabIndex = 27;
            this.pictureBoxThinChuckDetect.TabStop = false;
            // 
            // baseLabelThinChuckDetect
            // 
            this.baseLabelThinChuckDetect.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabelThinChuckDetect.ForeColor = System.Drawing.Color.White;
            this.baseLabelThinChuckDetect.Location = new System.Drawing.Point(251, 32);
            this.baseLabelThinChuckDetect.Name = "baseLabelThinChuckDetect";
            this.baseLabelThinChuckDetect.Size = new System.Drawing.Size(136, 25);
            this.baseLabelThinChuckDetect.TabIndex = 13;
            this.baseLabelThinChuckDetect.Text = "씬-척 감지";
            this.baseLabelThinChuckDetect.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnUpperCamera_Init
            // 
            this.btnUpperCamera_Init.BackColor = System.Drawing.Color.LightGray;
            this.btnUpperCamera_Init.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnUpperCamera_Init.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnUpperCamera_Init.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnUpperCamera_Init.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpperCamera_Init.ForeColor = System.Drawing.Color.DarkRed;
            this.btnUpperCamera_Init.Location = new System.Drawing.Point(8, 26);
            this.btnUpperCamera_Init.Name = "btnUpperCamera_Init";
            this.btnUpperCamera_Init.Size = new System.Drawing.Size(83, 62);
            this.btnUpperCamera_Init.TabIndex = 155;
            this.btnUpperCamera_Init.Text = "카메라\r\n초기화";
            this.btnUpperCamera_Init.UseVisualStyleBackColor = false;
            this.btnUpperCamera_Init.Click += new System.EventHandler(this.btnUpperCamera_Init_Click);
            // 
            // groupBoxAlignCheckPosParameter
            // 
            this.groupBoxAlignCheckPosParameter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseButton_X_Pos_GO3);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseButton_X_Pos_GO2);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseButton_X_Pos_GO1);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabelPosition_RIGHT);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabelPosition_Center);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabelPosition_Left);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabelPosition_Bot);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabelPosition_Mid);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabelPosition_Top);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseButton_Y_Pos_GO3);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseButton_Y_Pos_GO2);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseButton_Y_Pos_GO1);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabel18);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabel19);
            this.groupBoxAlignCheckPosParameter.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxAlignCheckPosParameter.ForeColor = System.Drawing.Color.White;
            this.groupBoxAlignCheckPosParameter.Location = new System.Drawing.Point(299, 460);
            this.groupBoxAlignCheckPosParameter.Name = "groupBoxAlignCheckPosParameter";
            this.groupBoxAlignCheckPosParameter.Size = new System.Drawing.Size(291, 300);
            this.groupBoxAlignCheckPosParameter.TabIndex = 156;
            this.groupBoxAlignCheckPosParameter.TabStop = false;
            this.groupBoxAlignCheckPosParameter.Text = " [ 얼라인 확인 위치 ] ";
            // 
            // baseButton_X_Pos_GO3
            // 
            this.baseButton_X_Pos_GO3.BackColor = System.Drawing.Color.LightGray;
            this.baseButton_X_Pos_GO3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButton_X_Pos_GO3.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButton_X_Pos_GO3.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButton_X_Pos_GO3.Location = new System.Drawing.Point(194, 71);
            this.baseButton_X_Pos_GO3.Name = "baseButton_X_Pos_GO3";
            this.baseButton_X_Pos_GO3.Size = new System.Drawing.Size(85, 45);
            this.baseButton_X_Pos_GO3.TabIndex = 152;
            this.baseButton_X_Pos_GO3.Text = "이동  ▶";
            this.baseButton_X_Pos_GO3.UseVisualStyleBackColor = false;
            this.baseButton_X_Pos_GO3.Click += new System.EventHandler(this.baseButton_X_Pos_GO3_Click);
            // 
            // baseButton_X_Pos_GO2
            // 
            this.baseButton_X_Pos_GO2.BackColor = System.Drawing.Color.LightGray;
            this.baseButton_X_Pos_GO2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButton_X_Pos_GO2.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButton_X_Pos_GO2.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButton_X_Pos_GO2.Location = new System.Drawing.Point(103, 71);
            this.baseButton_X_Pos_GO2.Name = "baseButton_X_Pos_GO2";
            this.baseButton_X_Pos_GO2.Size = new System.Drawing.Size(85, 45);
            this.baseButton_X_Pos_GO2.TabIndex = 151;
            this.baseButton_X_Pos_GO2.Text = "▣  이동";
            this.baseButton_X_Pos_GO2.UseVisualStyleBackColor = false;
            this.baseButton_X_Pos_GO2.Click += new System.EventHandler(this.baseButton_X_Pos_GO2_Click);
            // 
            // baseButton_X_Pos_GO1
            // 
            this.baseButton_X_Pos_GO1.BackColor = System.Drawing.Color.LightGray;
            this.baseButton_X_Pos_GO1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButton_X_Pos_GO1.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButton_X_Pos_GO1.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButton_X_Pos_GO1.Location = new System.Drawing.Point(12, 71);
            this.baseButton_X_Pos_GO1.Name = "baseButton_X_Pos_GO1";
            this.baseButton_X_Pos_GO1.Size = new System.Drawing.Size(85, 45);
            this.baseButton_X_Pos_GO1.TabIndex = 150;
            this.baseButton_X_Pos_GO1.Text = "◀  이동";
            this.baseButton_X_Pos_GO1.UseVisualStyleBackColor = false;
            this.baseButton_X_Pos_GO1.Click += new System.EventHandler(this.baseButton_X_Pos_GO1_Click);
            // 
            // baseLabelPosition_RIGHT
            // 
            this.baseLabelPosition_RIGHT.BackColor = System.Drawing.Color.Black;
            this.baseLabelPosition_RIGHT.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabelPosition_RIGHT.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelPosition_RIGHT.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabelPosition_RIGHT.Location = new System.Drawing.Point(194, 25);
            this.baseLabelPosition_RIGHT.Name = "baseLabelPosition_RIGHT";
            this.baseLabelPosition_RIGHT.Size = new System.Drawing.Size(85, 22);
            this.baseLabelPosition_RIGHT.TabIndex = 149;
            this.baseLabelPosition_RIGHT.Text = "RIGHT";
            this.baseLabelPosition_RIGHT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabelPosition_Center
            // 
            this.baseLabelPosition_Center.BackColor = System.Drawing.Color.Black;
            this.baseLabelPosition_Center.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabelPosition_Center.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelPosition_Center.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabelPosition_Center.Location = new System.Drawing.Point(103, 25);
            this.baseLabelPosition_Center.Name = "baseLabelPosition_Center";
            this.baseLabelPosition_Center.Size = new System.Drawing.Size(85, 22);
            this.baseLabelPosition_Center.TabIndex = 148;
            this.baseLabelPosition_Center.Text = "CENTER";
            this.baseLabelPosition_Center.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabelPosition_Left
            // 
            this.baseLabelPosition_Left.BackColor = System.Drawing.Color.Black;
            this.baseLabelPosition_Left.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabelPosition_Left.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelPosition_Left.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabelPosition_Left.Location = new System.Drawing.Point(12, 25);
            this.baseLabelPosition_Left.Name = "baseLabelPosition_Left";
            this.baseLabelPosition_Left.Size = new System.Drawing.Size(85, 22);
            this.baseLabelPosition_Left.TabIndex = 147;
            this.baseLabelPosition_Left.Text = "LEFT";
            this.baseLabelPosition_Left.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabelPosition_Bot
            // 
            this.baseLabelPosition_Bot.BackColor = System.Drawing.Color.Black;
            this.baseLabelPosition_Bot.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabelPosition_Bot.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelPosition_Bot.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabelPosition_Bot.Location = new System.Drawing.Point(39, 238);
            this.baseLabelPosition_Bot.Name = "baseLabelPosition_Bot";
            this.baseLabelPosition_Bot.Size = new System.Drawing.Size(66, 49);
            this.baseLabelPosition_Bot.TabIndex = 143;
            this.baseLabelPosition_Bot.Text = "BOT";
            this.baseLabelPosition_Bot.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabelPosition_Mid
            // 
            this.baseLabelPosition_Mid.BackColor = System.Drawing.Color.Black;
            this.baseLabelPosition_Mid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabelPosition_Mid.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelPosition_Mid.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabelPosition_Mid.Location = new System.Drawing.Point(39, 184);
            this.baseLabelPosition_Mid.Name = "baseLabelPosition_Mid";
            this.baseLabelPosition_Mid.Size = new System.Drawing.Size(66, 49);
            this.baseLabelPosition_Mid.TabIndex = 143;
            this.baseLabelPosition_Mid.Text = "MID";
            this.baseLabelPosition_Mid.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabelPosition_Top
            // 
            this.baseLabelPosition_Top.BackColor = System.Drawing.Color.LightGreen;
            this.baseLabelPosition_Top.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabelPosition_Top.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelPosition_Top.ForeColor = System.Drawing.Color.Black;
            this.baseLabelPosition_Top.Location = new System.Drawing.Point(39, 130);
            this.baseLabelPosition_Top.Name = "baseLabelPosition_Top";
            this.baseLabelPosition_Top.Size = new System.Drawing.Size(66, 49);
            this.baseLabelPosition_Top.TabIndex = 143;
            this.baseLabelPosition_Top.Text = "TOP";
            this.baseLabelPosition_Top.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseButton_Y_Pos_GO3
            // 
            this.baseButton_Y_Pos_GO3.BackColor = System.Drawing.Color.LightGray;
            this.baseButton_Y_Pos_GO3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButton_Y_Pos_GO3.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButton_Y_Pos_GO3.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButton_Y_Pos_GO3.Location = new System.Drawing.Point(181, 238);
            this.baseButton_Y_Pos_GO3.Name = "baseButton_Y_Pos_GO3";
            this.baseButton_Y_Pos_GO3.Size = new System.Drawing.Size(74, 49);
            this.baseButton_Y_Pos_GO3.TabIndex = 122;
            this.baseButton_Y_Pos_GO3.Text = "이동  ▼";
            this.baseButton_Y_Pos_GO3.UseVisualStyleBackColor = false;
            this.baseButton_Y_Pos_GO3.Click += new System.EventHandler(this.baseButton_Y_Pos_GO3_Click);
            // 
            // baseButton_Y_Pos_GO2
            // 
            this.baseButton_Y_Pos_GO2.BackColor = System.Drawing.Color.LightGray;
            this.baseButton_Y_Pos_GO2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButton_Y_Pos_GO2.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButton_Y_Pos_GO2.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButton_Y_Pos_GO2.Location = new System.Drawing.Point(181, 184);
            this.baseButton_Y_Pos_GO2.Name = "baseButton_Y_Pos_GO2";
            this.baseButton_Y_Pos_GO2.Size = new System.Drawing.Size(74, 49);
            this.baseButton_Y_Pos_GO2.TabIndex = 117;
            this.baseButton_Y_Pos_GO2.Text = "이동  ▣";
            this.baseButton_Y_Pos_GO2.UseVisualStyleBackColor = false;
            this.baseButton_Y_Pos_GO2.Click += new System.EventHandler(this.baseButton_Y_Pos_GO2_Click);
            // 
            // baseButton_Y_Pos_GO1
            // 
            this.baseButton_Y_Pos_GO1.BackColor = System.Drawing.Color.LightGray;
            this.baseButton_Y_Pos_GO1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButton_Y_Pos_GO1.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButton_Y_Pos_GO1.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButton_Y_Pos_GO1.Location = new System.Drawing.Point(181, 130);
            this.baseButton_Y_Pos_GO1.Name = "baseButton_Y_Pos_GO1";
            this.baseButton_Y_Pos_GO1.Size = new System.Drawing.Size(74, 49);
            this.baseButton_Y_Pos_GO1.TabIndex = 112;
            this.baseButton_Y_Pos_GO1.Text = "이동  ▲";
            this.baseButton_Y_Pos_GO1.UseVisualStyleBackColor = false;
            this.baseButton_Y_Pos_GO1.Click += new System.EventHandler(this.baseButton_Y_Pos_GO1_Click);
            // 
            // baseLabel18
            // 
            this.baseLabel18.BackColor = System.Drawing.Color.White;
            this.baseLabel18.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel18.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel18.ForeColor = System.Drawing.Color.Black;
            this.baseLabel18.Location = new System.Drawing.Point(108, 129);
            this.baseLabel18.Name = "baseLabel18";
            this.baseLabel18.Size = new System.Drawing.Size(70, 159);
            this.baseLabel18.TabIndex = 109;
            this.baseLabel18.Text = "▲\r\n▲\r\n\r\n[ V, W ]\r\n\r\n▼\r\n▼";
            this.baseLabel18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel19
            // 
            this.baseLabel19.BackColor = System.Drawing.Color.White;
            this.baseLabel19.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel19.Font = new System.Drawing.Font("나눔바른고딕", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel19.ForeColor = System.Drawing.Color.Black;
            this.baseLabel19.Location = new System.Drawing.Point(12, 50);
            this.baseLabel19.Name = "baseLabel19";
            this.baseLabel19.Size = new System.Drawing.Size(267, 18);
            this.baseLabel19.TabIndex = 108;
            this.baseLabel19.Text = "◀◀◀    [ Axis U ]    ▶▶▶";
            this.baseLabel19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnUpperCamera_StartLive
            // 
            this.btnUpperCamera_StartLive.BackColor = System.Drawing.Color.LightGray;
            this.btnUpperCamera_StartLive.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnUpperCamera_StartLive.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnUpperCamera_StartLive.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnUpperCamera_StartLive.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpperCamera_StartLive.ForeColor = System.Drawing.Color.DarkRed;
            this.btnUpperCamera_StartLive.Location = new System.Drawing.Point(8, 94);
            this.btnUpperCamera_StartLive.Name = "btnUpperCamera_StartLive";
            this.btnUpperCamera_StartLive.Size = new System.Drawing.Size(83, 62);
            this.btnUpperCamera_StartLive.TabIndex = 157;
            this.btnUpperCamera_StartLive.Text = "라이브\r\n이미지";
            this.btnUpperCamera_StartLive.UseVisualStyleBackColor = false;
            this.btnUpperCamera_StartLive.Click += new System.EventHandler(this.btnUpperCamera_StartLive_Click);
            // 
            // m_visionImageViewer_Lower
            // 
            this.m_visionImageViewer_Lower.BackColor = System.Drawing.Color.Black;
            this.m_visionImageViewer_Lower.Camera = null;
            this.m_visionImageViewer_Lower.CameraSwitch = null;
            this.m_visionImageViewer_Lower.FrameRate = 1D;
            this.m_visionImageViewer_Lower.InputImage = null;
            this.m_visionImageViewer_Lower.IsViewCustomizedImage = false;
            this.m_visionImageViewer_Lower.Location = new System.Drawing.Point(617, 26);
            this.m_visionImageViewer_Lower.Name = "m_visionImageViewer_Lower";
            this.m_visionImageViewer_Lower.OperatingType = QMC.Common.Hmi.VisionImageViewer.OperatingTypes.Center;
            this.m_visionImageViewer_Lower.Simulated = false;
            this.m_visionImageViewer_Lower.Size = new System.Drawing.Size(503, 422);
            this.m_visionImageViewer_Lower.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.m_visionImageViewer_Lower.TabIndex = 154;
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
            this.m_visionImageViewer_Upper.Location = new System.Drawing.Point(106, 26);
            this.m_visionImageViewer_Upper.Name = "m_visionImageViewer_Upper";
            this.m_visionImageViewer_Upper.OperatingType = QMC.Common.Hmi.VisionImageViewer.OperatingTypes.Center;
            this.m_visionImageViewer_Upper.Simulated = false;
            this.m_visionImageViewer_Upper.Size = new System.Drawing.Size(503, 422);
            this.m_visionImageViewer_Upper.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.m_visionImageViewer_Upper.TabIndex = 149;
            this.m_visionImageViewer_Upper.TabStop = false;
            this.m_visionImageViewer_Upper.UpdateDelayTime = 160;
            this.m_visionImageViewer_Upper.VisibleCrossLine = true;
            // 
            // btnProbeCardAlignOnly_Start
            // 
            this.btnProbeCardAlignOnly_Start.BackColor = System.Drawing.Color.SteelBlue;
            this.btnProbeCardAlignOnly_Start.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnProbeCardAlignOnly_Start.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnProbeCardAlignOnly_Start.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnProbeCardAlignOnly_Start.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnProbeCardAlignOnly_Start.ForeColor = System.Drawing.Color.White;
            this.btnProbeCardAlignOnly_Start.Location = new System.Drawing.Point(1183, 483);
            this.btnProbeCardAlignOnly_Start.Name = "btnProbeCardAlignOnly_Start";
            this.btnProbeCardAlignOnly_Start.Size = new System.Drawing.Size(234, 33);
            this.btnProbeCardAlignOnly_Start.TabIndex = 158;
            this.btnProbeCardAlignOnly_Start.Text = "테스트 : 프로브 카드 얼라인 확인";
            this.btnProbeCardAlignOnly_Start.UseVisualStyleBackColor = false;
            this.btnProbeCardAlignOnly_Start.Click += new System.EventHandler(this.btnProbeCardAlignOnly_Start_Click);
            // 
            // btnWaferAlignOnly_Start
            // 
            this.btnWaferAlignOnly_Start.BackColor = System.Drawing.Color.SteelBlue;
            this.btnWaferAlignOnly_Start.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnWaferAlignOnly_Start.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnWaferAlignOnly_Start.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnWaferAlignOnly_Start.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnWaferAlignOnly_Start.ForeColor = System.Drawing.Color.White;
            this.btnWaferAlignOnly_Start.Location = new System.Drawing.Point(1569, 483);
            this.btnWaferAlignOnly_Start.Name = "btnWaferAlignOnly_Start";
            this.btnWaferAlignOnly_Start.Size = new System.Drawing.Size(202, 33);
            this.btnWaferAlignOnly_Start.TabIndex = 159;
            this.btnWaferAlignOnly_Start.Text = "테스트 : 웨이퍼 얼라인 확인";
            this.btnWaferAlignOnly_Start.UseVisualStyleBackColor = false;
            this.btnWaferAlignOnly_Start.Click += new System.EventHandler(this.btnWaferAlignOnly_Start_Click);
            // 
            // lblLowerCamera_AlignData
            // 
            this.lblLowerCamera_AlignData.BackColor = System.Drawing.Color.Cornsilk;
            this.lblLowerCamera_AlignData.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblLowerCamera_AlignData.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblLowerCamera_AlignData.ForeColor = System.Drawing.Color.Black;
            this.lblLowerCamera_AlignData.Location = new System.Drawing.Point(1774, 484);
            this.lblLowerCamera_AlignData.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLowerCamera_AlignData.Name = "lblLowerCamera_AlignData";
            this.lblLowerCamera_AlignData.Size = new System.Drawing.Size(104, 31);
            this.lblLowerCamera_AlignData.TabIndex = 191;
            this.lblLowerCamera_AlignData.Text = "- - -";
            this.lblLowerCamera_AlignData.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabelAngle_Lower
            // 
            this.baseLabelAngle_Lower.BackColor = System.Drawing.Color.Black;
            this.baseLabelAngle_Lower.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabelAngle_Lower.Font = new System.Drawing.Font("나눔바른고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelAngle_Lower.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabelAngle_Lower.Location = new System.Drawing.Point(1879, 484);
            this.baseLabelAngle_Lower.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabelAngle_Lower.Name = "baseLabelAngle_Lower";
            this.baseLabelAngle_Lower.Size = new System.Drawing.Size(21, 31);
            this.baseLabelAngle_Lower.TabIndex = 190;
            this.baseLabelAngle_Lower.Text = "˚";
            this.baseLabelAngle_Lower.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblUpperCamera_AlignData
            // 
            this.lblUpperCamera_AlignData.BackColor = System.Drawing.Color.Cornsilk;
            this.lblUpperCamera_AlignData.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblUpperCamera_AlignData.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblUpperCamera_AlignData.ForeColor = System.Drawing.Color.Black;
            this.lblUpperCamera_AlignData.Location = new System.Drawing.Point(1420, 484);
            this.lblUpperCamera_AlignData.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUpperCamera_AlignData.Name = "lblUpperCamera_AlignData";
            this.lblUpperCamera_AlignData.Size = new System.Drawing.Size(104, 31);
            this.lblUpperCamera_AlignData.TabIndex = 189;
            this.lblUpperCamera_AlignData.Text = "- - -";
            this.lblUpperCamera_AlignData.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabelAngle_Upper
            // 
            this.baseLabelAngle_Upper.BackColor = System.Drawing.Color.Black;
            this.baseLabelAngle_Upper.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabelAngle_Upper.Font = new System.Drawing.Font("나눔바른고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelAngle_Upper.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabelAngle_Upper.Location = new System.Drawing.Point(1525, 484);
            this.baseLabelAngle_Upper.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabelAngle_Upper.Name = "baseLabelAngle_Upper";
            this.baseLabelAngle_Upper.Size = new System.Drawing.Size(21, 31);
            this.baseLabelAngle_Upper.TabIndex = 188;
            this.baseLabelAngle_Upper.Text = "˚";
            this.baseLabelAngle_Upper.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel_WaferChuck_Camera1
            // 
            this.baseLabel_WaferChuck_Camera1.BackColor = System.Drawing.Color.Black;
            this.baseLabel_WaferChuck_Camera1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabel_WaferChuck_Camera1.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_WaferChuck_Camera1.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel_WaferChuck_Camera1.Location = new System.Drawing.Point(812, 0);
            this.baseLabel_WaferChuck_Camera1.Name = "baseLabel_WaferChuck_Camera1";
            this.baseLabel_WaferChuck_Camera1.Size = new System.Drawing.Size(113, 25);
            this.baseLabel_WaferChuck_Camera1.TabIndex = 153;
            this.baseLabel_WaferChuck_Camera1.Text = "웨이퍼\r\n";
            this.baseLabel_WaferChuck_Camera1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel_ProbeCard_Camera1
            // 
            this.baseLabel_ProbeCard_Camera1.BackColor = System.Drawing.Color.Black;
            this.baseLabel_ProbeCard_Camera1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabel_ProbeCard_Camera1.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_ProbeCard_Camera1.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel_ProbeCard_Camera1.Location = new System.Drawing.Point(301, 0);
            this.baseLabel_ProbeCard_Camera1.Name = "baseLabel_ProbeCard_Camera1";
            this.baseLabel_ProbeCard_Camera1.Size = new System.Drawing.Size(113, 25);
            this.baseLabel_ProbeCard_Camera1.TabIndex = 152;
            this.baseLabel_ProbeCard_Camera1.Text = "프로브 카드";
            this.baseLabel_ProbeCard_Camera1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseGroupBox_CycleButton
            // 
            this.baseGroupBox_CycleButton.Controls.Add(this.btnAlignErrorCheck);
            this.baseGroupBox_CycleButton.Controls.Add(this.btnWaferProbeCardUnpacking);
            this.baseGroupBox_CycleButton.Controls.Add(this.btnWaferProbeCardUnpackingReady);
            this.baseGroupBox_CycleButton.Controls.Add(this.btn_ProbeCard_Locking);
            this.baseGroupBox_CycleButton.Controls.Add(this.btnLoadingPos_ProbeCard_GO);
            this.baseGroupBox_CycleButton.Controls.Add(this.btnLoadingPos_Wafer_GO);
            this.baseGroupBox_CycleButton.Controls.Add(this.btnSafetyPos_CamXY_GO);
            this.baseGroupBox_CycleButton.Controls.Add(this.btnReticlePos_LowerCam_GO);
            this.baseGroupBox_CycleButton.Controls.Add(this.btnReticlePos_UpperCam_GO);
            this.baseGroupBox_CycleButton.Controls.Add(this.btnPacking);
            this.baseGroupBox_CycleButton.Controls.Add(this.btnRunStatus_Drilling2);
            this.baseGroupBox_CycleButton.Controls.Add(this.btnRunStatus_Drilling);
            this.baseGroupBox_CycleButton.Controls.Add(this.btnMainWork_Start);
            this.baseGroupBox_CycleButton.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_CycleButton.ForeColor = System.Drawing.Color.White;
            this.baseGroupBox_CycleButton.Location = new System.Drawing.Point(1072, 509);
            this.baseGroupBox_CycleButton.Name = "baseGroupBox_CycleButton";
            this.baseGroupBox_CycleButton.Size = new System.Drawing.Size(828, 251);
            this.baseGroupBox_CycleButton.TabIndex = 131;
            this.baseGroupBox_CycleButton.TabStop = false;
            this.baseGroupBox_CycleButton.Text = " [ 사이클 동작 ] ";
            // 
            // btnAlignErrorCheck
            // 
            this.btnAlignErrorCheck.BackColor = System.Drawing.Color.LightGray;
            this.btnAlignErrorCheck.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnAlignErrorCheck.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnAlignErrorCheck.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnAlignErrorCheck.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnAlignErrorCheck.ForeColor = System.Drawing.Color.DarkRed;
            this.btnAlignErrorCheck.Location = new System.Drawing.Point(243, 114);
            this.btnAlignErrorCheck.Name = "btnAlignErrorCheck";
            this.btnAlignErrorCheck.Size = new System.Drawing.Size(130, 125);
            this.btnAlignErrorCheck.TabIndex = 187;
            this.btnAlignErrorCheck.Text = "웨이퍼 -     \r\n프로브 카드\r\n얼라인 에러\r\n검사 시작";
            this.btnAlignErrorCheck.UseVisualStyleBackColor = false;
            this.btnAlignErrorCheck.Click += new System.EventHandler(this.btnAlignErrorCheck_Click);
            // 
            // btnWaferProbeCardUnpacking
            // 
            this.btnWaferProbeCardUnpacking.BackColor = System.Drawing.Color.LightGray;
            this.btnWaferProbeCardUnpacking.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnWaferProbeCardUnpacking.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnWaferProbeCardUnpacking.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnWaferProbeCardUnpacking.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnWaferProbeCardUnpacking.ForeColor = System.Drawing.Color.DarkRed;
            this.btnWaferProbeCardUnpacking.Location = new System.Drawing.Point(527, 137);
            this.btnWaferProbeCardUnpacking.Name = "btnWaferProbeCardUnpacking";
            this.btnWaferProbeCardUnpacking.Size = new System.Drawing.Size(148, 102);
            this.btnWaferProbeCardUnpacking.TabIndex = 186;
            this.btnWaferProbeCardUnpacking.Text = "웨이퍼 -     \r\n프로브 카드\r\n언패킹 시작";
            this.btnWaferProbeCardUnpacking.UseVisualStyleBackColor = false;
            this.btnWaferProbeCardUnpacking.Click += new System.EventHandler(this.btnWaferProbeCardUnpacking_Click);
            // 
            // btnWaferProbeCardUnpackingReady
            // 
            this.btnWaferProbeCardUnpackingReady.BackColor = System.Drawing.Color.LightGray;
            this.btnWaferProbeCardUnpackingReady.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnWaferProbeCardUnpackingReady.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnWaferProbeCardUnpackingReady.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnWaferProbeCardUnpackingReady.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnWaferProbeCardUnpackingReady.ForeColor = System.Drawing.Color.DarkRed;
            this.btnWaferProbeCardUnpackingReady.Location = new System.Drawing.Point(527, 33);
            this.btnWaferProbeCardUnpackingReady.Name = "btnWaferProbeCardUnpackingReady";
            this.btnWaferProbeCardUnpackingReady.Size = new System.Drawing.Size(148, 102);
            this.btnWaferProbeCardUnpackingReady.TabIndex = 185;
            this.btnWaferProbeCardUnpackingReady.Text = "웨이퍼 -     \r\n프로브 카드\r\n언패킹 준비 위치\r\n이동";
            this.btnWaferProbeCardUnpackingReady.UseVisualStyleBackColor = false;
            this.btnWaferProbeCardUnpackingReady.Click += new System.EventHandler(this.btnWaferProbeCardUnpackingReady_Click);
            // 
            // btn_ProbeCard_Locking
            // 
            this.btn_ProbeCard_Locking.BackColor = System.Drawing.Color.LightGray;
            this.btn_ProbeCard_Locking.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btn_ProbeCard_Locking.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn_ProbeCard_Locking.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btn_ProbeCard_Locking.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_ProbeCard_Locking.ForeColor = System.Drawing.Color.DarkRed;
            this.btn_ProbeCard_Locking.Location = new System.Drawing.Point(125, 137);
            this.btn_ProbeCard_Locking.Name = "btn_ProbeCard_Locking";
            this.btn_ProbeCard_Locking.Size = new System.Drawing.Size(111, 102);
            this.btn_ProbeCard_Locking.TabIndex = 184;
            this.btn_ProbeCard_Locking.Text = "프로브 카드\r\n고정 작업\r\n시작";
            this.btn_ProbeCard_Locking.UseVisualStyleBackColor = false;
            this.btn_ProbeCard_Locking.Click += new System.EventHandler(this.btn_ProbeCard_Locking_Click);
            // 
            // btnLoadingPos_ProbeCard_GO
            // 
            this.btnLoadingPos_ProbeCard_GO.BackColor = System.Drawing.Color.LightGray;
            this.btnLoadingPos_ProbeCard_GO.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnLoadingPos_ProbeCard_GO.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnLoadingPos_ProbeCard_GO.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnLoadingPos_ProbeCard_GO.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLoadingPos_ProbeCard_GO.ForeColor = System.Drawing.Color.DarkRed;
            this.btnLoadingPos_ProbeCard_GO.Location = new System.Drawing.Point(125, 33);
            this.btnLoadingPos_ProbeCard_GO.Name = "btnLoadingPos_ProbeCard_GO";
            this.btnLoadingPos_ProbeCard_GO.Size = new System.Drawing.Size(111, 102);
            this.btnLoadingPos_ProbeCard_GO.TabIndex = 183;
            this.btnLoadingPos_ProbeCard_GO.Text = "프로브 카드\r\n투입 위치\r\n이동";
            this.btnLoadingPos_ProbeCard_GO.UseVisualStyleBackColor = false;
            this.btnLoadingPos_ProbeCard_GO.Click += new System.EventHandler(this.btnLoadingPos_ProbeCard_GO_Click);
            // 
            // btnLoadingPos_Wafer_GO
            // 
            this.btnLoadingPos_Wafer_GO.BackColor = System.Drawing.Color.LightGray;
            this.btnLoadingPos_Wafer_GO.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnLoadingPos_Wafer_GO.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnLoadingPos_Wafer_GO.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnLoadingPos_Wafer_GO.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLoadingPos_Wafer_GO.ForeColor = System.Drawing.Color.DarkRed;
            this.btnLoadingPos_Wafer_GO.Location = new System.Drawing.Point(12, 143);
            this.btnLoadingPos_Wafer_GO.Name = "btnLoadingPos_Wafer_GO";
            this.btnLoadingPos_Wafer_GO.Size = new System.Drawing.Size(106, 96);
            this.btnLoadingPos_Wafer_GO.TabIndex = 181;
            this.btnLoadingPos_Wafer_GO.Text = "웨이퍼\r\n투입 위치\r\n이동";
            this.btnLoadingPos_Wafer_GO.UseVisualStyleBackColor = false;
            this.btnLoadingPos_Wafer_GO.Click += new System.EventHandler(this.btnLoadingPos_Wafer_GO_Click);
            // 
            // btnSafetyPos_CamXY_GO
            // 
            this.btnSafetyPos_CamXY_GO.BackColor = System.Drawing.Color.LightGray;
            this.btnSafetyPos_CamXY_GO.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnSafetyPos_CamXY_GO.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnSafetyPos_CamXY_GO.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnSafetyPos_CamXY_GO.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSafetyPos_CamXY_GO.ForeColor = System.Drawing.Color.DarkRed;
            this.btnSafetyPos_CamXY_GO.Location = new System.Drawing.Point(12, 33);
            this.btnSafetyPos_CamXY_GO.Name = "btnSafetyPos_CamXY_GO";
            this.btnSafetyPos_CamXY_GO.Size = new System.Drawing.Size(106, 96);
            this.btnSafetyPos_CamXY_GO.TabIndex = 182;
            this.btnSafetyPos_CamXY_GO.Text = "카메라\r\n안전 위치\r\n이동";
            this.btnSafetyPos_CamXY_GO.UseVisualStyleBackColor = false;
            this.btnSafetyPos_CamXY_GO.Click += new System.EventHandler(this.btnSafetyPos_CamXY_GO_Click);
            // 
            // btnReticlePos_LowerCam_GO
            // 
            this.btnReticlePos_LowerCam_GO.BackColor = System.Drawing.Color.LightGray;
            this.btnReticlePos_LowerCam_GO.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnReticlePos_LowerCam_GO.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnReticlePos_LowerCam_GO.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnReticlePos_LowerCam_GO.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnReticlePos_LowerCam_GO.ForeColor = System.Drawing.Color.DarkRed;
            this.btnReticlePos_LowerCam_GO.Location = new System.Drawing.Point(683, 137);
            this.btnReticlePos_LowerCam_GO.Name = "btnReticlePos_LowerCam_GO";
            this.btnReticlePos_LowerCam_GO.Size = new System.Drawing.Size(133, 102);
            this.btnReticlePos_LowerCam_GO.TabIndex = 180;
            this.btnReticlePos_LowerCam_GO.Text = "레티클 센터\r\n확인 위치 이동\r\n[ 하부 카메라 ]";
            this.btnReticlePos_LowerCam_GO.UseVisualStyleBackColor = false;
            this.btnReticlePos_LowerCam_GO.Click += new System.EventHandler(this.btnReticlePos_LowerCam_GO_Click);
            // 
            // btnReticlePos_UpperCam_GO
            // 
            this.btnReticlePos_UpperCam_GO.BackColor = System.Drawing.Color.LightGray;
            this.btnReticlePos_UpperCam_GO.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnReticlePos_UpperCam_GO.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnReticlePos_UpperCam_GO.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnReticlePos_UpperCam_GO.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnReticlePos_UpperCam_GO.ForeColor = System.Drawing.Color.DarkRed;
            this.btnReticlePos_UpperCam_GO.Location = new System.Drawing.Point(683, 33);
            this.btnReticlePos_UpperCam_GO.Name = "btnReticlePos_UpperCam_GO";
            this.btnReticlePos_UpperCam_GO.Size = new System.Drawing.Size(133, 102);
            this.btnReticlePos_UpperCam_GO.TabIndex = 179;
            this.btnReticlePos_UpperCam_GO.Text = "레티클 센터\r\n확인 위치 이동\r\n[ 상부 카메라 ]";
            this.btnReticlePos_UpperCam_GO.UseVisualStyleBackColor = false;
            this.btnReticlePos_UpperCam_GO.Click += new System.EventHandler(this.btnReticlePos_UpperCam_GO_Click);
            // 
            // btnPacking
            // 
            this.btnPacking.BackColor = System.Drawing.Color.LightGray;
            this.btnPacking.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnPacking.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnPacking.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnPacking.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnPacking.ForeColor = System.Drawing.Color.DarkRed;
            this.btnPacking.Location = new System.Drawing.Point(377, 114);
            this.btnPacking.Name = "btnPacking";
            this.btnPacking.Size = new System.Drawing.Size(130, 125);
            this.btnPacking.TabIndex = 66;
            this.btnPacking.Text = "웨이퍼 -     \r\n프로브 카드\r\n\r\n패킹 시작";
            this.btnPacking.UseVisualStyleBackColor = false;
            this.btnPacking.Click += new System.EventHandler(this.btnPacking_Click);
            // 
            // btnRunStatus_Drilling2
            // 
            this.btnRunStatus_Drilling2.BackColor = System.Drawing.Color.Maroon;
            this.btnRunStatus_Drilling2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRunStatus_Drilling2.Location = new System.Drawing.Point(487, 34);
            this.btnRunStatus_Drilling2.Name = "btnRunStatus_Drilling2";
            this.btnRunStatus_Drilling2.Size = new System.Drawing.Size(20, 71);
            this.btnRunStatus_Drilling2.TabIndex = 64;
            this.btnRunStatus_Drilling2.UseVisualStyleBackColor = false;
            // 
            // btnRunStatus_Drilling
            // 
            this.btnRunStatus_Drilling.BackColor = System.Drawing.Color.Maroon;
            this.btnRunStatus_Drilling.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRunStatus_Drilling.Location = new System.Drawing.Point(243, 34);
            this.btnRunStatus_Drilling.Name = "btnRunStatus_Drilling";
            this.btnRunStatus_Drilling.Size = new System.Drawing.Size(20, 71);
            this.btnRunStatus_Drilling.TabIndex = 63;
            this.btnRunStatus_Drilling.UseVisualStyleBackColor = false;
            // 
            // btnMainWork_Start
            // 
            this.btnMainWork_Start.BackColor = System.Drawing.Color.LightGray;
            this.btnMainWork_Start.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnMainWork_Start.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnMainWork_Start.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnMainWork_Start.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnMainWork_Start.ForeColor = System.Drawing.Color.DarkRed;
            this.btnMainWork_Start.Location = new System.Drawing.Point(266, 33);
            this.btnMainWork_Start.Name = "btnMainWork_Start";
            this.btnMainWork_Start.Size = new System.Drawing.Size(218, 73);
            this.btnMainWork_Start.TabIndex = 62;
            this.btnMainWork_Start.Text = "웨이퍼 - 프로브 카드\r\n얼라인 시작";
            this.btnMainWork_Start.UseVisualStyleBackColor = false;
            this.btnMainWork_Start.Click += new System.EventHandler(this.btnMainWork_Start_Click);
            // 
            // baseGroupBox_ManualButton
            // 
            this.baseGroupBox_ManualButton.Controls.Add(this.pictureBoxProbePackingCheck);
            this.baseGroupBox_ManualButton.Controls.Add(this.pictureBoxTopCoverDown);
            this.baseGroupBox_ManualButton.Controls.Add(this.pictureBoxTopCoverUp);
            this.baseGroupBox_ManualButton.Controls.Add(this.pictureBoxWaferVacuumCheck);
            this.baseGroupBox_ManualButton.Controls.Add(this.pictureBoxThinChuckVacuumCheck);
            this.baseGroupBox_ManualButton.Controls.Add(this.baseButtonTopCoverDown);
            this.baseGroupBox_ManualButton.Controls.Add(this.baseButtonTopCoverUp);
            this.baseGroupBox_ManualButton.Controls.Add(this.baseButtonWaferVacuum);
            this.baseGroupBox_ManualButton.Controls.Add(this.baseButtonProbeUnpacking);
            this.baseGroupBox_ManualButton.Controls.Add(this.baseButtonProbePacking);
            this.baseGroupBox_ManualButton.Controls.Add(this.baseButtonThinChuckVacuum);
            this.baseGroupBox_ManualButton.Controls.Add(this.baseButtonThinChuckStageCleaning);
            this.baseGroupBox_ManualButton.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_ManualButton.ForeColor = System.Drawing.Color.White;
            this.baseGroupBox_ManualButton.Location = new System.Drawing.Point(611, 576);
            this.baseGroupBox_ManualButton.Name = "baseGroupBox_ManualButton";
            this.baseGroupBox_ManualButton.Size = new System.Drawing.Size(439, 184);
            this.baseGroupBox_ManualButton.TabIndex = 130;
            this.baseGroupBox_ManualButton.TabStop = false;
            this.baseGroupBox_ManualButton.Text = " [ 수동 동작 (IO 신호) ] ";
            // 
            // pictureBoxProbePackingCheck
            // 
            this.pictureBoxProbePackingCheck.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
            this.pictureBoxProbePackingCheck.Location = new System.Drawing.Point(301, 22);
            this.pictureBoxProbePackingCheck.Name = "pictureBoxProbePackingCheck";
            this.pictureBoxProbePackingCheck.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxProbePackingCheck.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxProbePackingCheck.TabIndex = 136;
            this.pictureBoxProbePackingCheck.TabStop = false;
            // 
            // pictureBoxTopCoverDown
            // 
            this.pictureBoxTopCoverDown.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
            this.pictureBoxTopCoverDown.Location = new System.Drawing.Point(166, 105);
            this.pictureBoxTopCoverDown.Name = "pictureBoxTopCoverDown";
            this.pictureBoxTopCoverDown.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxTopCoverDown.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxTopCoverDown.TabIndex = 135;
            this.pictureBoxTopCoverDown.TabStop = false;
            // 
            // pictureBoxTopCoverUp
            // 
            this.pictureBoxTopCoverUp.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
            this.pictureBoxTopCoverUp.Location = new System.Drawing.Point(166, 27);
            this.pictureBoxTopCoverUp.Name = "pictureBoxTopCoverUp";
            this.pictureBoxTopCoverUp.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxTopCoverUp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxTopCoverUp.TabIndex = 134;
            this.pictureBoxTopCoverUp.TabStop = false;
            // 
            // pictureBoxWaferVacuumCheck
            // 
            this.pictureBoxWaferVacuumCheck.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
            this.pictureBoxWaferVacuumCheck.Location = new System.Drawing.Point(13, 105);
            this.pictureBoxWaferVacuumCheck.Name = "pictureBoxWaferVacuumCheck";
            this.pictureBoxWaferVacuumCheck.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxWaferVacuumCheck.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxWaferVacuumCheck.TabIndex = 133;
            this.pictureBoxWaferVacuumCheck.TabStop = false;
            // 
            // pictureBoxThinChuckVacuumCheck
            // 
            this.pictureBoxThinChuckVacuumCheck.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
            this.pictureBoxThinChuckVacuumCheck.Location = new System.Drawing.Point(13, 27);
            this.pictureBoxThinChuckVacuumCheck.Name = "pictureBoxThinChuckVacuumCheck";
            this.pictureBoxThinChuckVacuumCheck.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxThinChuckVacuumCheck.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxThinChuckVacuumCheck.TabIndex = 132;
            this.pictureBoxThinChuckVacuumCheck.TabStop = false;
            // 
            // baseButtonTopCoverDown
            // 
            this.baseButtonTopCoverDown.BackColor = System.Drawing.Color.LightGray;
            this.baseButtonTopCoverDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonTopCoverDown.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButtonTopCoverDown.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButtonTopCoverDown.Location = new System.Drawing.Point(195, 104);
            this.baseButtonTopCoverDown.Name = "baseButtonTopCoverDown";
            this.baseButtonTopCoverDown.Size = new System.Drawing.Size(95, 69);
            this.baseButtonTopCoverDown.TabIndex = 131;
            this.baseButtonTopCoverDown.Text = "프로브 카드\r\n커버 내림";
            this.baseButtonTopCoverDown.UseVisualStyleBackColor = false;
            this.baseButtonTopCoverDown.Click += new System.EventHandler(this.baseButtonTopCoverDown_Click);
            // 
            // baseButtonTopCoverUp
            // 
            this.baseButtonTopCoverUp.BackColor = System.Drawing.Color.LightGray;
            this.baseButtonTopCoverUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonTopCoverUp.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButtonTopCoverUp.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButtonTopCoverUp.Location = new System.Drawing.Point(195, 26);
            this.baseButtonTopCoverUp.Name = "baseButtonTopCoverUp";
            this.baseButtonTopCoverUp.Size = new System.Drawing.Size(95, 69);
            this.baseButtonTopCoverUp.TabIndex = 130;
            this.baseButtonTopCoverUp.Text = "프로브 카드\r\n커버 올림";
            this.baseButtonTopCoverUp.UseVisualStyleBackColor = false;
            this.baseButtonTopCoverUp.Click += new System.EventHandler(this.baseButtonTopCoverUp_Click);
            // 
            // baseButtonWaferVacuum
            // 
            this.baseButtonWaferVacuum.BackColor = System.Drawing.Color.LightGray;
            this.baseButtonWaferVacuum.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonWaferVacuum.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButtonWaferVacuum.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButtonWaferVacuum.Location = new System.Drawing.Point(42, 104);
            this.baseButtonWaferVacuum.Name = "baseButtonWaferVacuum";
            this.baseButtonWaferVacuum.Size = new System.Drawing.Size(113, 69);
            this.baseButtonWaferVacuum.TabIndex = 129;
            this.baseButtonWaferVacuum.Text = "웨이퍼 공압\r\nOn";
            this.baseButtonWaferVacuum.UseVisualStyleBackColor = false;
            this.baseButtonWaferVacuum.Click += new System.EventHandler(this.baseButtonWaferVacuum_Click);
            // 
            // baseButtonProbeUnpacking
            // 
            this.baseButtonProbeUnpacking.BackColor = System.Drawing.Color.LightGray;
            this.baseButtonProbeUnpacking.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonProbeUnpacking.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButtonProbeUnpacking.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButtonProbeUnpacking.Location = new System.Drawing.Point(313, 75);
            this.baseButtonProbeUnpacking.Name = "baseButtonProbeUnpacking";
            this.baseButtonProbeUnpacking.Size = new System.Drawing.Size(114, 52);
            this.baseButtonProbeUnpacking.TabIndex = 128;
            this.baseButtonProbeUnpacking.Text = "프로브 언패킹\r\n공압 On";
            this.baseButtonProbeUnpacking.UseVisualStyleBackColor = false;
            this.baseButtonProbeUnpacking.Click += new System.EventHandler(this.baseButtonProbeUnpacking_Click);
            // 
            // baseButtonProbePacking
            // 
            this.baseButtonProbePacking.BackColor = System.Drawing.Color.LightGray;
            this.baseButtonProbePacking.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonProbePacking.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButtonProbePacking.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButtonProbePacking.Location = new System.Drawing.Point(330, 21);
            this.baseButtonProbePacking.Name = "baseButtonProbePacking";
            this.baseButtonProbePacking.Size = new System.Drawing.Size(97, 52);
            this.baseButtonProbePacking.TabIndex = 127;
            this.baseButtonProbePacking.Text = "프로브 패킹\r\n공압 On";
            this.baseButtonProbePacking.UseVisualStyleBackColor = false;
            this.baseButtonProbePacking.Click += new System.EventHandler(this.baseButtonProbePacking_Click);
            // 
            // baseButtonThinChuckVacuum
            // 
            this.baseButtonThinChuckVacuum.BackColor = System.Drawing.Color.LightGray;
            this.baseButtonThinChuckVacuum.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonThinChuckVacuum.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButtonThinChuckVacuum.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButtonThinChuckVacuum.Location = new System.Drawing.Point(42, 26);
            this.baseButtonThinChuckVacuum.Name = "baseButtonThinChuckVacuum";
            this.baseButtonThinChuckVacuum.Size = new System.Drawing.Size(113, 69);
            this.baseButtonThinChuckVacuum.TabIndex = 126;
            this.baseButtonThinChuckVacuum.Text = "씬-척 공압\r\nOn";
            this.baseButtonThinChuckVacuum.UseVisualStyleBackColor = false;
            this.baseButtonThinChuckVacuum.Click += new System.EventHandler(this.baseButtonThinChuckVacuum_Click);
            // 
            // baseButtonThinChuckStageCleaning
            // 
            this.baseButtonThinChuckStageCleaning.BackColor = System.Drawing.Color.LightGray;
            this.baseButtonThinChuckStageCleaning.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonThinChuckStageCleaning.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButtonThinChuckStageCleaning.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButtonThinChuckStageCleaning.Location = new System.Drawing.Point(313, 130);
            this.baseButtonThinChuckStageCleaning.Name = "baseButtonThinChuckStageCleaning";
            this.baseButtonThinChuckStageCleaning.Size = new System.Drawing.Size(114, 43);
            this.baseButtonThinChuckStageCleaning.TabIndex = 125;
            this.baseButtonThinChuckStageCleaning.Text = "씬-척 클리닝";
            this.baseButtonThinChuckStageCleaning.UseVisualStyleBackColor = false;
            this.baseButtonThinChuckStageCleaning.Click += new System.EventHandler(this.baseButtonThinChuckStageCleaning_Click);
            // 
            // ManualMode_CWA150SA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.Controls.Add(this.lblLowerCamera_AlignData);
            this.Controls.Add(this.baseLabelAngle_Lower);
            this.Controls.Add(this.lblUpperCamera_AlignData);
            this.Controls.Add(this.baseLabelAngle_Upper);
            this.Controls.Add(this.btnWaferAlignOnly_Start);
            this.Controls.Add(this.btnProbeCardAlignOnly_Start);
            this.Controls.Add(this.btnUpperCamera_StartLive);
            this.Controls.Add(this.groupBoxAlignCheckPosParameter);
            this.Controls.Add(this.btnUpperCamera_Init);
            this.Controls.Add(this.m_visionImageViewer_Lower);
            this.Controls.Add(this.baseLabel_WaferChuck_Camera1);
            this.Controls.Add(this.baseLabel_ProbeCard_Camera1);
            this.Controls.Add(this.m_visionImageViewer_Upper);
            this.Controls.Add(this.baseGroupBox_CycleButton);
            this.Controls.Add(this.baseGroupBox_ManualButton);
            this.Controls.Add(this.groupBoxDetectStatus);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ManualMode_CWA150SA";
            this.Size = new System.Drawing.Size(1910, 770);
            this.Load += new System.EventHandler(this.ManualMode_CWA150SA_Load);
            this.groupBoxDetectStatus.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMainVacuumCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMainAirCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProbeBWDetect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxThinChuckDetect)).EndInit();
            this.groupBoxAlignCheckPosParameter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_visionImageViewer_Lower)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_visionImageViewer_Upper)).EndInit();
            this.baseGroupBox_CycleButton.ResumeLayout(false);
            this.baseGroupBox_ManualButton.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProbePackingCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTopCoverDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTopCoverUp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWaferVacuumCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxThinChuckVacuumCheck)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private BaseGroupBox baseGroupBox_CycleButton;
        private System.Windows.Forms.Button btnPacking;
        private System.Windows.Forms.Button btnRunStatus_Drilling2;
        private System.Windows.Forms.Button btnRunStatus_Drilling;
        private System.Windows.Forms.Button btnMainWork_Start;
        private BaseGroupBox baseGroupBox_ManualButton;
        private System.Windows.Forms.PictureBox pictureBoxProbePackingCheck;
        private System.Windows.Forms.PictureBox pictureBoxTopCoverDown;
        private System.Windows.Forms.PictureBox pictureBoxTopCoverUp;
        private System.Windows.Forms.PictureBox pictureBoxWaferVacuumCheck;
        private System.Windows.Forms.PictureBox pictureBoxThinChuckVacuumCheck;
        private BaseButton baseButtonTopCoverDown;
        private BaseButton baseButtonTopCoverUp;
        private BaseButton baseButtonWaferVacuum;
        private BaseButton baseButtonProbeUnpacking;
        private BaseButton baseButtonProbePacking;
        private BaseButton baseButtonThinChuckVacuum;
        private BaseButton baseButtonThinChuckStageCleaning;
        private System.Windows.Forms.GroupBox groupBoxDetectStatus;
        private System.Windows.Forms.PictureBox pictureBoxProbeBWDetect;
        private BaseLabel baseLabelProbeBWDetect;
        private System.Windows.Forms.PictureBox pictureBoxThinChuckDetect;
        private BaseLabel baseLabelThinChuckDetect;
        private System.Windows.Forms.PictureBox pictureBoxMainVacuumCheck;
        private BaseLabel lblMainVacuumCheck;
        private System.Windows.Forms.PictureBox pictureBoxMainAirCheck;
        private BaseLabel lblMainAirCheck;
        private QMC.Common.Hmi.VisionImageViewer m_visionImageViewer_Upper;
        private System.Windows.Forms.Button btnUpperCamera_Init;
        private QMC.Common.Hmi.VisionImageViewer m_visionImageViewer_Lower;
        private BaseLabel baseLabel_WaferChuck_Camera1;
        private BaseLabel baseLabel_ProbeCard_Camera1;
        private System.Windows.Forms.GroupBox groupBoxAlignCheckPosParameter;
        private BaseButton baseButton_X_Pos_GO3;
        private BaseButton baseButton_X_Pos_GO2;
        private BaseButton baseButton_X_Pos_GO1;
        private BaseLabel baseLabelPosition_RIGHT;
        private BaseLabel baseLabelPosition_Center;
        private BaseLabel baseLabelPosition_Left;
        private BaseLabel baseLabelPosition_Bot;
        private BaseLabel baseLabelPosition_Mid;
        private BaseLabel baseLabelPosition_Top;
        private BaseButton baseButton_Y_Pos_GO3;
        private BaseButton baseButton_Y_Pos_GO2;
        private BaseButton baseButton_Y_Pos_GO1;
        private BaseLabel baseLabel18;
        private BaseLabel baseLabel19;
        private System.Windows.Forms.Button btnReticlePos_LowerCam_GO;
        private System.Windows.Forms.Button btnReticlePos_UpperCam_GO;
        private System.Windows.Forms.Button btnLoadingPos_Wafer_GO;
        private System.Windows.Forms.Button btnSafetyPos_CamXY_GO;
        private System.Windows.Forms.Button btnUpperCamera_StartLive;
        private System.Windows.Forms.Button btnLoadingPos_ProbeCard_GO;
        private System.Windows.Forms.Button btn_ProbeCard_Locking;
        private System.Windows.Forms.Button btnWaferProbeCardUnpacking;
        private System.Windows.Forms.Button btnWaferProbeCardUnpackingReady;
        private System.Windows.Forms.Button btnProbeCardAlignOnly_Start;
        private System.Windows.Forms.Button btnWaferAlignOnly_Start;
        private BaseLabel lblLowerCamera_AlignData;
        private BaseLabel baseLabelAngle_Lower;
        private BaseLabel lblUpperCamera_AlignData;
        private BaseLabel baseLabelAngle_Upper;
        private System.Windows.Forms.Button btnAlignErrorCheck;
    }
}
