using QMC.Common.Hmi;
using System.Drawing;

namespace CWA150SA_Onsemi300
{
    partial class CalibrationMode_CWA150SA
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CalibrationMode_CWA150SA));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.groupBoxAlignStageMoveLimit = new System.Windows.Forms.GroupBox();
            this.btn_UVW_Limit_Save = new System.Windows.Forms.Button();
            this.btn_UVW_Limit_Range_Calc = new System.Windows.Forms.Button();
            this.tb_Axis_UVW_Limit_Range = new System.Windows.Forms.TextBox();
            this.baseLabel9 = new CWA150SA_Onsemi300.BaseLabel();
            this.tb_Axis_W_Limit_Plus = new System.Windows.Forms.TextBox();
            this.baseLabel6 = new CWA150SA_Onsemi300.BaseLabel();
            this.tb_Axis_W_Limit_Minus = new System.Windows.Forms.TextBox();
            this.baseLabel8 = new CWA150SA_Onsemi300.BaseLabel();
            this.tb_Axis_V_Limit_Plus = new System.Windows.Forms.TextBox();
            this.baseLabel5 = new CWA150SA_Onsemi300.BaseLabel();
            this.tb_Axis_V_Limit_Minus = new System.Windows.Forms.TextBox();
            this.baseLabel4 = new CWA150SA_Onsemi300.BaseLabel();
            this.tb_Axis_U_Limit_Plus = new System.Windows.Forms.TextBox();
            this.baseLabel3 = new CWA150SA_Onsemi300.BaseLabel();
            this.tb_Axis_U_Limit_Minus = new System.Windows.Forms.TextBox();
            this.baseLabel2 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel_Limit_W = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel_Limit_V = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel_Limit_U = new CWA150SA_Onsemi300.BaseLabel();
            this.pictureBoxUVW_Stage = new System.Windows.Forms.PictureBox();
            this.lblUpperCamera = new System.Windows.Forms.Label();
            this.lblLowerCamera = new System.Windows.Forms.Label();
            this.btnUpperCamera_Init = new System.Windows.Forms.Button();
            this.groupBoxAlignCheckPosParameter = new System.Windows.Forms.GroupBox();
            this.baseLabelAlignCenter_PackingPos = new CWA150SA_Onsemi300.BaseLabel();
            this.baseButton_X_Pos_GO3 = new CWA150SA_Onsemi300.BaseButton();
            this.baseButton_X_Pos_GO2 = new CWA150SA_Onsemi300.BaseButton();
            this.baseButton_X_Pos_GO1 = new CWA150SA_Onsemi300.BaseButton();
            this.baseLabelPosition_RIGHT = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelPosition_Center = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelPosition_Left = new CWA150SA_Onsemi300.BaseLabel();
            this.tb_Axis_Y_BOT = new System.Windows.Forms.TextBox();
            this.baseLabelPosition_Bot = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelPosition_Mid = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelPosition_Top = new CWA150SA_Onsemi300.BaseLabel();
            this.baseButton_Y_Pos_GO3 = new CWA150SA_Onsemi300.BaseButton();
            this.tb_Axis_X_RIGHT = new System.Windows.Forms.TextBox();
            this.baseLabel1 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel7 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseButton_Y_Pos_GO2 = new CWA150SA_Onsemi300.BaseButton();
            this.tb_Axis_Y_MID = new System.Windows.Forms.TextBox();
            this.tb_Axis_X_CENTER = new System.Windows.Forms.TextBox();
            this.baseLabel10 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel13 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseButton_Y_Pos_GO1 = new CWA150SA_Onsemi300.BaseButton();
            this.tb_Axis_Y_TOP = new System.Windows.Forms.TextBox();
            this.tb_Axis_X_LEFT = new System.Windows.Forms.TextBox();
            this.baseLabel18 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel19 = new CWA150SA_Onsemi300.BaseLabel();
            this.groupBoxUpperLowerCameraAlignCheck = new System.Windows.Forms.GroupBox();
            this.baseButton_GoPos_ReticleCenter_LowerCam = new CWA150SA_Onsemi300.BaseButton();
            this.baseLabelPosition_LowerCamera_ReticleCenter = new CWA150SA_Onsemi300.BaseLabel();
            this.tb_Axis_EZ_ReticleCenter_LowerCam = new System.Windows.Forms.TextBox();
            this.baseLabelPosition_UpperCamera_ReticleCenter = new CWA150SA_Onsemi300.BaseLabel();
            this.tb_Axis_Z_ReticleCenter = new System.Windows.Forms.TextBox();
            this.baseLabel36 = new CWA150SA_Onsemi300.BaseLabel();
            this.tb_Axis_Y_ReticleCenter = new System.Windows.Forms.TextBox();
            this.baseLabel37 = new CWA150SA_Onsemi300.BaseLabel();
            this.tb_Axis_X_ReticleCenter = new System.Windows.Forms.TextBox();
            this.baseLabel38 = new CWA150SA_Onsemi300.BaseLabel();
            this.tb_Axis_EZ_ReticleCenter_UpperCam = new System.Windows.Forms.TextBox();
            this.baseLabel35 = new CWA150SA_Onsemi300.BaseLabel();
            this.tb_Axis_W_ReticleCenter = new System.Windows.Forms.TextBox();
            this.tb_Axis_V_ReticleCenter = new System.Windows.Forms.TextBox();
            this.tb_Axis_U_ReticleCenter = new System.Windows.Forms.TextBox();
            this.baseLabel21 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel33 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel34 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelPosition_StageCamera_ReticleCenter = new CWA150SA_Onsemi300.BaseLabel();
            this.baseButton_GoPos_ReticleCenter_UpperCam = new CWA150SA_Onsemi300.BaseButton();
            this.baseButton_GoPos_ReticleCenter_StageCamXY = new CWA150SA_Onsemi300.BaseButton();
            this.btnReticlePos_LowerCam_GO = new System.Windows.Forms.Button();
            this.btnReticlePos_UpperCam_GO = new System.Windows.Forms.Button();
            this.btnSafetyPos_CamXY_GO = new System.Windows.Forms.Button();
            this.btnLoadingPos_Wafer_GO = new System.Windows.Forms.Button();
            this.btnUpperCamera_StartLive = new System.Windows.Forms.Button();
            this.btnLoadingPos_ProbeCard_GO = new System.Windows.Forms.Button();
            this.btn_ProbeCard_Locking = new System.Windows.Forms.Button();
            this.tb_Axis_EZ_Wafer_ProbeCard_Packing = new System.Windows.Forms.TextBox();
            this.btnUser_Registration = new System.Windows.Forms.Button();
            this.baseLabelUVWPosition_Loading = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelPosition_Wafer_Probecard_Packing = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel15 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseButton_GoPos_Wafer_ProbeCard_Packing = new CWA150SA_Onsemi300.BaseButton();
            this.m_visionImageViewer_Lower = new QMC.Common.Hmi.VisionImageViewer();
            this.m_visionImageViewer_Upper = new QMC.Common.Hmi.VisionImageViewer();
            this.baseLabel_WaferChuck_Camera1 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel_ProbeCard_Camera1 = new CWA150SA_Onsemi300.BaseLabel();
            this.groupBoxAlignStageMoveLimit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxUVW_Stage)).BeginInit();
            this.groupBoxAlignCheckPosParameter.SuspendLayout();
            this.groupBoxUpperLowerCameraAlignCheck.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_visionImageViewer_Lower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_visionImageViewer_Upper)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // groupBoxAlignStageMoveLimit
            // 
            this.groupBoxAlignStageMoveLimit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.groupBoxAlignStageMoveLimit.Controls.Add(this.btn_UVW_Limit_Save);
            this.groupBoxAlignStageMoveLimit.Controls.Add(this.btn_UVW_Limit_Range_Calc);
            this.groupBoxAlignStageMoveLimit.Controls.Add(this.tb_Axis_UVW_Limit_Range);
            this.groupBoxAlignStageMoveLimit.Controls.Add(this.baseLabel9);
            this.groupBoxAlignStageMoveLimit.Controls.Add(this.tb_Axis_W_Limit_Plus);
            this.groupBoxAlignStageMoveLimit.Controls.Add(this.baseLabel6);
            this.groupBoxAlignStageMoveLimit.Controls.Add(this.tb_Axis_W_Limit_Minus);
            this.groupBoxAlignStageMoveLimit.Controls.Add(this.baseLabel8);
            this.groupBoxAlignStageMoveLimit.Controls.Add(this.tb_Axis_V_Limit_Plus);
            this.groupBoxAlignStageMoveLimit.Controls.Add(this.baseLabel5);
            this.groupBoxAlignStageMoveLimit.Controls.Add(this.tb_Axis_V_Limit_Minus);
            this.groupBoxAlignStageMoveLimit.Controls.Add(this.baseLabel4);
            this.groupBoxAlignStageMoveLimit.Controls.Add(this.tb_Axis_U_Limit_Plus);
            this.groupBoxAlignStageMoveLimit.Controls.Add(this.baseLabel3);
            this.groupBoxAlignStageMoveLimit.Controls.Add(this.tb_Axis_U_Limit_Minus);
            this.groupBoxAlignStageMoveLimit.Controls.Add(this.baseLabel2);
            this.groupBoxAlignStageMoveLimit.Controls.Add(this.baseLabel_Limit_W);
            this.groupBoxAlignStageMoveLimit.Controls.Add(this.baseLabel_Limit_V);
            this.groupBoxAlignStageMoveLimit.Controls.Add(this.baseLabel_Limit_U);
            this.groupBoxAlignStageMoveLimit.Controls.Add(this.pictureBoxUVW_Stage);
            this.groupBoxAlignStageMoveLimit.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBoxAlignStageMoveLimit.ForeColor = System.Drawing.Color.White;
            this.groupBoxAlignStageMoveLimit.Location = new System.Drawing.Point(3, 384);
            this.groupBoxAlignStageMoveLimit.Name = "groupBoxAlignStageMoveLimit";
            this.groupBoxAlignStageMoveLimit.Size = new System.Drawing.Size(452, 379);
            this.groupBoxAlignStageMoveLimit.TabIndex = 22;
            this.groupBoxAlignStageMoveLimit.TabStop = false;
            this.groupBoxAlignStageMoveLimit.Text = " [ 얼라인 스테이지  -  구동 제한 위치 ] ";
            // 
            // btn_UVW_Limit_Save
            // 
            this.btn_UVW_Limit_Save.BackColor = System.Drawing.Color.LightGray;
            this.btn_UVW_Limit_Save.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btn_UVW_Limit_Save.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn_UVW_Limit_Save.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btn_UVW_Limit_Save.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_UVW_Limit_Save.ForeColor = System.Drawing.Color.DarkRed;
            this.btn_UVW_Limit_Save.Location = new System.Drawing.Point(263, 311);
            this.btn_UVW_Limit_Save.Name = "btn_UVW_Limit_Save";
            this.btn_UVW_Limit_Save.Size = new System.Drawing.Size(73, 55);
            this.btn_UVW_Limit_Save.TabIndex = 176;
            this.btn_UVW_Limit_Save.Text = "구동 제한\r\n위치 저장";
            this.btn_UVW_Limit_Save.UseVisualStyleBackColor = false;
            this.btn_UVW_Limit_Save.Click += new System.EventHandler(this.btn_UVW_Limit_Save_Click);
            // 
            // btn_UVW_Limit_Range_Calc
            // 
            this.btn_UVW_Limit_Range_Calc.BackColor = System.Drawing.Color.LightGray;
            this.btn_UVW_Limit_Range_Calc.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btn_UVW_Limit_Range_Calc.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn_UVW_Limit_Range_Calc.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btn_UVW_Limit_Range_Calc.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_UVW_Limit_Range_Calc.ForeColor = System.Drawing.Color.DarkRed;
            this.btn_UVW_Limit_Range_Calc.Location = new System.Drawing.Point(171, 321);
            this.btn_UVW_Limit_Range_Calc.Name = "btn_UVW_Limit_Range_Calc";
            this.btn_UVW_Limit_Range_Calc.Size = new System.Drawing.Size(75, 45);
            this.btn_UVW_Limit_Range_Calc.TabIndex = 175;
            this.btn_UVW_Limit_Range_Calc.Text = "범위값으로\r\n위치 계산";
            this.btn_UVW_Limit_Range_Calc.UseVisualStyleBackColor = false;
            this.btn_UVW_Limit_Range_Calc.Click += new System.EventHandler(this.btn_UVW_Limit_Range_Calc_Click);
            // 
            // tb_Axis_UVW_Limit_Range
            // 
            this.tb_Axis_UVW_Limit_Range.Font = new System.Drawing.Font("나눔바른고딕", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_Axis_UVW_Limit_Range.Location = new System.Drawing.Point(172, 293);
            this.tb_Axis_UVW_Limit_Range.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_Axis_UVW_Limit_Range.Name = "tb_Axis_UVW_Limit_Range";
            this.tb_Axis_UVW_Limit_Range.Size = new System.Drawing.Size(73, 27);
            this.tb_Axis_UVW_Limit_Range.TabIndex = 164;
            this.tb_Axis_UVW_Limit_Range.TabStop = false;
            this.tb_Axis_UVW_Limit_Range.Text = "0.00";
            this.tb_Axis_UVW_Limit_Range.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseLabel9
            // 
            this.baseLabel9.BackColor = System.Drawing.Color.DarkGray;
            this.baseLabel9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel9.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel9.ForeColor = System.Drawing.Color.Black;
            this.baseLabel9.Location = new System.Drawing.Point(116, 292);
            this.baseLabel9.Name = "baseLabel9";
            this.baseLabel9.Size = new System.Drawing.Size(53, 74);
            this.baseLabel9.TabIndex = 163;
            this.baseLabel9.Text = "± 범위\r\n\r\n(㎜)";
            this.baseLabel9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tb_Axis_W_Limit_Plus
            // 
            this.tb_Axis_W_Limit_Plus.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Axis_W_Limit_Plus.Location = new System.Drawing.Point(349, 190);
            this.tb_Axis_W_Limit_Plus.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_Axis_W_Limit_Plus.Name = "tb_Axis_W_Limit_Plus";
            this.tb_Axis_W_Limit_Plus.Size = new System.Drawing.Size(89, 29);
            this.tb_Axis_W_Limit_Plus.TabIndex = 162;
            this.tb_Axis_W_Limit_Plus.TabStop = false;
            this.tb_Axis_W_Limit_Plus.Text = "0.000";
            this.tb_Axis_W_Limit_Plus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseLabel6
            // 
            this.baseLabel6.BackColor = System.Drawing.Color.DarkGray;
            this.baseLabel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel6.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel6.ForeColor = System.Drawing.Color.Black;
            this.baseLabel6.Location = new System.Drawing.Point(349, 158);
            this.baseLabel6.Name = "baseLabel6";
            this.baseLabel6.Size = new System.Drawing.Size(89, 30);
            this.baseLabel6.TabIndex = 161;
            this.baseLabel6.Text = "W (+)";
            this.baseLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tb_Axis_W_Limit_Minus
            // 
            this.tb_Axis_W_Limit_Minus.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Axis_W_Limit_Minus.Location = new System.Drawing.Point(349, 265);
            this.tb_Axis_W_Limit_Minus.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_Axis_W_Limit_Minus.Name = "tb_Axis_W_Limit_Minus";
            this.tb_Axis_W_Limit_Minus.Size = new System.Drawing.Size(89, 29);
            this.tb_Axis_W_Limit_Minus.TabIndex = 160;
            this.tb_Axis_W_Limit_Minus.TabStop = false;
            this.tb_Axis_W_Limit_Minus.Text = "0.000";
            this.tb_Axis_W_Limit_Minus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseLabel8
            // 
            this.baseLabel8.BackColor = System.Drawing.Color.DarkGray;
            this.baseLabel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel8.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel8.ForeColor = System.Drawing.Color.Black;
            this.baseLabel8.Location = new System.Drawing.Point(349, 233);
            this.baseLabel8.Name = "baseLabel8";
            this.baseLabel8.Size = new System.Drawing.Size(89, 30);
            this.baseLabel8.TabIndex = 159;
            this.baseLabel8.Text = "W (-)";
            this.baseLabel8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tb_Axis_V_Limit_Plus
            // 
            this.tb_Axis_V_Limit_Plus.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Axis_V_Limit_Plus.Location = new System.Drawing.Point(14, 190);
            this.tb_Axis_V_Limit_Plus.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_Axis_V_Limit_Plus.Name = "tb_Axis_V_Limit_Plus";
            this.tb_Axis_V_Limit_Plus.Size = new System.Drawing.Size(89, 29);
            this.tb_Axis_V_Limit_Plus.TabIndex = 158;
            this.tb_Axis_V_Limit_Plus.TabStop = false;
            this.tb_Axis_V_Limit_Plus.Text = "0.000";
            this.tb_Axis_V_Limit_Plus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseLabel5
            // 
            this.baseLabel5.BackColor = System.Drawing.Color.DarkGray;
            this.baseLabel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel5.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel5.ForeColor = System.Drawing.Color.Black;
            this.baseLabel5.Location = new System.Drawing.Point(14, 158);
            this.baseLabel5.Name = "baseLabel5";
            this.baseLabel5.Size = new System.Drawing.Size(89, 30);
            this.baseLabel5.TabIndex = 157;
            this.baseLabel5.Text = "V (+)";
            this.baseLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tb_Axis_V_Limit_Minus
            // 
            this.tb_Axis_V_Limit_Minus.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Axis_V_Limit_Minus.Location = new System.Drawing.Point(14, 265);
            this.tb_Axis_V_Limit_Minus.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_Axis_V_Limit_Minus.Name = "tb_Axis_V_Limit_Minus";
            this.tb_Axis_V_Limit_Minus.Size = new System.Drawing.Size(89, 29);
            this.tb_Axis_V_Limit_Minus.TabIndex = 156;
            this.tb_Axis_V_Limit_Minus.TabStop = false;
            this.tb_Axis_V_Limit_Minus.Text = "0.000";
            this.tb_Axis_V_Limit_Minus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseLabel4
            // 
            this.baseLabel4.BackColor = System.Drawing.Color.DarkGray;
            this.baseLabel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel4.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel4.ForeColor = System.Drawing.Color.Black;
            this.baseLabel4.Location = new System.Drawing.Point(14, 233);
            this.baseLabel4.Name = "baseLabel4";
            this.baseLabel4.Size = new System.Drawing.Size(89, 30);
            this.baseLabel4.TabIndex = 155;
            this.baseLabel4.Text = "V (-)";
            this.baseLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tb_Axis_U_Limit_Plus
            // 
            this.tb_Axis_U_Limit_Plus.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Axis_U_Limit_Plus.Location = new System.Drawing.Point(331, 29);
            this.tb_Axis_U_Limit_Plus.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_Axis_U_Limit_Plus.Name = "tb_Axis_U_Limit_Plus";
            this.tb_Axis_U_Limit_Plus.Size = new System.Drawing.Size(89, 29);
            this.tb_Axis_U_Limit_Plus.TabIndex = 154;
            this.tb_Axis_U_Limit_Plus.TabStop = false;
            this.tb_Axis_U_Limit_Plus.Text = "0.000";
            this.tb_Axis_U_Limit_Plus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseLabel3
            // 
            this.baseLabel3.BackColor = System.Drawing.Color.DarkGray;
            this.baseLabel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel3.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel3.ForeColor = System.Drawing.Color.Black;
            this.baseLabel3.Location = new System.Drawing.Point(274, 28);
            this.baseLabel3.Name = "baseLabel3";
            this.baseLabel3.Size = new System.Drawing.Size(55, 30);
            this.baseLabel3.TabIndex = 153;
            this.baseLabel3.Text = "U (+)";
            this.baseLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tb_Axis_U_Limit_Minus
            // 
            this.tb_Axis_U_Limit_Minus.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Axis_U_Limit_Minus.Location = new System.Drawing.Point(172, 29);
            this.tb_Axis_U_Limit_Minus.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_Axis_U_Limit_Minus.Name = "tb_Axis_U_Limit_Minus";
            this.tb_Axis_U_Limit_Minus.Size = new System.Drawing.Size(89, 29);
            this.tb_Axis_U_Limit_Minus.TabIndex = 152;
            this.tb_Axis_U_Limit_Minus.TabStop = false;
            this.tb_Axis_U_Limit_Minus.Text = "0.000";
            this.tb_Axis_U_Limit_Minus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseLabel2
            // 
            this.baseLabel2.BackColor = System.Drawing.Color.DarkGray;
            this.baseLabel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel2.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel2.ForeColor = System.Drawing.Color.Black;
            this.baseLabel2.Location = new System.Drawing.Point(115, 28);
            this.baseLabel2.Name = "baseLabel2";
            this.baseLabel2.Size = new System.Drawing.Size(55, 30);
            this.baseLabel2.TabIndex = 151;
            this.baseLabel2.Text = "U (-)";
            this.baseLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel_Limit_W
            // 
            this.baseLabel_Limit_W.BackColor = System.Drawing.Color.Black;
            this.baseLabel_Limit_W.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabel_Limit_W.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_Limit_W.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel_Limit_W.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.baseLabel_Limit_W.Location = new System.Drawing.Point(356, 307);
            this.baseLabel_Limit_W.Name = "baseLabel_Limit_W";
            this.baseLabel_Limit_W.Size = new System.Drawing.Size(74, 59);
            this.baseLabel_Limit_W.TabIndex = 150;
            this.baseLabel_Limit_W.Text = "    ▲     (+)\r\n [ W ]\r\n    ▽     (-)";
            this.baseLabel_Limit_W.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // baseLabel_Limit_V
            // 
            this.baseLabel_Limit_V.BackColor = System.Drawing.Color.Black;
            this.baseLabel_Limit_V.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabel_Limit_V.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_Limit_V.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel_Limit_V.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.baseLabel_Limit_V.Location = new System.Drawing.Point(23, 307);
            this.baseLabel_Limit_V.Name = "baseLabel_Limit_V";
            this.baseLabel_Limit_V.Size = new System.Drawing.Size(71, 59);
            this.baseLabel_Limit_V.TabIndex = 149;
            this.baseLabel_Limit_V.Text = "    ▲    (+)\r\n [ V ]\r\n    ▽    (-)";
            this.baseLabel_Limit_V.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // baseLabel_Limit_U
            // 
            this.baseLabel_Limit_U.BackColor = System.Drawing.Color.Black;
            this.baseLabel_Limit_U.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabel_Limit_U.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_Limit_U.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel_Limit_U.Location = new System.Drawing.Point(26, 28);
            this.baseLabel_Limit_U.Name = "baseLabel_Limit_U";
            this.baseLabel_Limit_U.Size = new System.Drawing.Size(76, 50);
            this.baseLabel_Limit_U.TabIndex = 148;
            this.baseLabel_Limit_U.Text = "◁ [ U ] ▶\r\n(-)      (+)";
            this.baseLabel_Limit_U.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBoxUVW_Stage
            // 
            this.pictureBoxUVW_Stage.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxUVW_Stage.Image")));
            this.pictureBoxUVW_Stage.Location = new System.Drawing.Point(116, 64);
            this.pictureBoxUVW_Stage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBoxUVW_Stage.Name = "pictureBoxUVW_Stage";
            this.pictureBoxUVW_Stage.Size = new System.Drawing.Size(220, 221);
            this.pictureBoxUVW_Stage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxUVW_Stage.TabIndex = 112;
            this.pictureBoxUVW_Stage.TabStop = false;
            // 
            // lblUpperCamera
            // 
            this.lblUpperCamera.BackColor = System.Drawing.Color.LightSkyBlue;
            this.lblUpperCamera.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblUpperCamera.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
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
            this.lblLowerCamera.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
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
            this.btnUpperCamera_Init.BackColor = System.Drawing.Color.LightGray;
            this.btnUpperCamera_Init.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnUpperCamera_Init.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnUpperCamera_Init.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnUpperCamera_Init.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnUpperCamera_Init.ForeColor = System.Drawing.Color.DarkRed;
            this.btnUpperCamera_Init.Location = new System.Drawing.Point(8, 26);
            this.btnUpperCamera_Init.Name = "btnUpperCamera_Init";
            this.btnUpperCamera_Init.Size = new System.Drawing.Size(83, 62);
            this.btnUpperCamera_Init.TabIndex = 145;
            this.btnUpperCamera_Init.Text = "카메라\r\n초기화";
            this.btnUpperCamera_Init.UseVisualStyleBackColor = false;
            this.btnUpperCamera_Init.Click += new System.EventHandler(this.btnUpperCamera_Init_Click);
            // 
            // groupBoxAlignCheckPosParameter
            // 
            this.groupBoxAlignCheckPosParameter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabelAlignCenter_PackingPos);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseButton_X_Pos_GO3);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseButton_X_Pos_GO2);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseButton_X_Pos_GO1);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabelPosition_RIGHT);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabelPosition_Center);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabelPosition_Left);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.tb_Axis_Y_BOT);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabelPosition_Bot);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabelPosition_Mid);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabelPosition_Top);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseButton_Y_Pos_GO3);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.tb_Axis_X_RIGHT);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabel1);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabel7);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseButton_Y_Pos_GO2);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.tb_Axis_Y_MID);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.tb_Axis_X_CENTER);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabel10);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabel13);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseButton_Y_Pos_GO1);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.tb_Axis_Y_TOP);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.tb_Axis_X_LEFT);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabel18);
            this.groupBoxAlignCheckPosParameter.Controls.Add(this.baseLabel19);
            this.groupBoxAlignCheckPosParameter.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBoxAlignCheckPosParameter.ForeColor = System.Drawing.Color.White;
            this.groupBoxAlignCheckPosParameter.Location = new System.Drawing.Point(767, 498);
            this.groupBoxAlignCheckPosParameter.Name = "groupBoxAlignCheckPosParameter";
            this.groupBoxAlignCheckPosParameter.Size = new System.Drawing.Size(564, 265);
            this.groupBoxAlignCheckPosParameter.TabIndex = 146;
            this.groupBoxAlignCheckPosParameter.TabStop = false;
            this.groupBoxAlignCheckPosParameter.Text = " [ 얼라인 확인 위치   ( Top / Middle / Bottom )   ( 기준 : 프로브 카드 Tip ) ] ";
            // 
            // baseLabelAlignCenter_PackingPos
            // 
            this.baseLabelAlignCenter_PackingPos.BackColor = System.Drawing.Color.White;
            this.baseLabelAlignCenter_PackingPos.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelAlignCenter_PackingPos.ForeColor = System.Drawing.Color.Red;
            this.baseLabelAlignCenter_PackingPos.Location = new System.Drawing.Point(12, 26);
            this.baseLabelAlignCenter_PackingPos.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabelAlignCenter_PackingPos.Name = "baseLabelAlignCenter_PackingPos";
            this.baseLabelAlignCenter_PackingPos.Size = new System.Drawing.Size(539, 30);
            this.baseLabelAlignCenter_PackingPos.TabIndex = 153;
            this.baseLabelAlignCenter_PackingPos.Text = "* XY 얼라인 기준 위치  :  TOP 위치에서 프로브 카드 Tip 과 웨이퍼 Gate 가 일치되도록.";
            this.baseLabelAlignCenter_PackingPos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseButton_X_Pos_GO3
            // 
            this.baseButton_X_Pos_GO3.BackColor = System.Drawing.Color.LightGray;
            this.baseButton_X_Pos_GO3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButton_X_Pos_GO3.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButton_X_Pos_GO3.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButton_X_Pos_GO3.Location = new System.Drawing.Point(462, 189);
            this.baseButton_X_Pos_GO3.Name = "baseButton_X_Pos_GO3";
            this.baseButton_X_Pos_GO3.Size = new System.Drawing.Size(89, 52);
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
            this.baseButton_X_Pos_GO2.Location = new System.Drawing.Point(364, 189);
            this.baseButton_X_Pos_GO2.Name = "baseButton_X_Pos_GO2";
            this.baseButton_X_Pos_GO2.Size = new System.Drawing.Size(89, 52);
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
            this.baseButton_X_Pos_GO1.Location = new System.Drawing.Point(266, 189);
            this.baseButton_X_Pos_GO1.Name = "baseButton_X_Pos_GO1";
            this.baseButton_X_Pos_GO1.Size = new System.Drawing.Size(89, 52);
            this.baseButton_X_Pos_GO1.TabIndex = 150;
            this.baseButton_X_Pos_GO1.Text = "◁  이동";
            this.baseButton_X_Pos_GO1.UseVisualStyleBackColor = false;
            this.baseButton_X_Pos_GO1.Click += new System.EventHandler(this.baseButton_X_Pos_GO1_Click);
            // 
            // baseLabelPosition_RIGHT
            // 
            this.baseLabelPosition_RIGHT.BackColor = System.Drawing.Color.Black;
            this.baseLabelPosition_RIGHT.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabelPosition_RIGHT.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabelPosition_RIGHT.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabelPosition_RIGHT.Location = new System.Drawing.Point(462, 64);
            this.baseLabelPosition_RIGHT.Name = "baseLabelPosition_RIGHT";
            this.baseLabelPosition_RIGHT.Size = new System.Drawing.Size(89, 61);
            this.baseLabelPosition_RIGHT.TabIndex = 149;
            this.baseLabelPosition_RIGHT.Text = "RIGHT\r\n[ Get ]";
            this.baseLabelPosition_RIGHT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.baseLabelPosition_RIGHT.Click += new System.EventHandler(this.baseLabelPosition_RIGHT_Click);
            // 
            // baseLabelPosition_Center
            // 
            this.baseLabelPosition_Center.BackColor = System.Drawing.Color.Black;
            this.baseLabelPosition_Center.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabelPosition_Center.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabelPosition_Center.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabelPosition_Center.Location = new System.Drawing.Point(364, 64);
            this.baseLabelPosition_Center.Name = "baseLabelPosition_Center";
            this.baseLabelPosition_Center.Size = new System.Drawing.Size(89, 61);
            this.baseLabelPosition_Center.TabIndex = 148;
            this.baseLabelPosition_Center.Text = "CENTER\r\n[ Get ]";
            this.baseLabelPosition_Center.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.baseLabelPosition_Center.Click += new System.EventHandler(this.baseLabelPosition_Center_Click);
            // 
            // baseLabelPosition_Left
            // 
            this.baseLabelPosition_Left.BackColor = System.Drawing.Color.Black;
            this.baseLabelPosition_Left.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabelPosition_Left.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabelPosition_Left.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabelPosition_Left.Location = new System.Drawing.Point(266, 64);
            this.baseLabelPosition_Left.Name = "baseLabelPosition_Left";
            this.baseLabelPosition_Left.Size = new System.Drawing.Size(89, 61);
            this.baseLabelPosition_Left.TabIndex = 147;
            this.baseLabelPosition_Left.Text = "LEFT\r\n[ Get ]";
            this.baseLabelPosition_Left.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.baseLabelPosition_Left.Click += new System.EventHandler(this.baseLabelPosition_Left_Click);
            // 
            // tb_Axis_Y_BOT
            // 
            this.tb_Axis_Y_BOT.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Axis_Y_BOT.Location = new System.Drawing.Point(80, 223);
            this.tb_Axis_Y_BOT.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_Axis_Y_BOT.Name = "tb_Axis_Y_BOT";
            this.tb_Axis_Y_BOT.Size = new System.Drawing.Size(89, 29);
            this.tb_Axis_Y_BOT.TabIndex = 145;
            this.tb_Axis_Y_BOT.TabStop = false;
            this.tb_Axis_Y_BOT.Text = "0.000";
            this.tb_Axis_Y_BOT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseLabelPosition_Bot
            // 
            this.baseLabelPosition_Bot.BackColor = System.Drawing.Color.Black;
            this.baseLabelPosition_Bot.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabelPosition_Bot.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabelPosition_Bot.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabelPosition_Bot.Location = new System.Drawing.Point(12, 193);
            this.baseLabelPosition_Bot.Name = "baseLabelPosition_Bot";
            this.baseLabelPosition_Bot.Size = new System.Drawing.Size(65, 59);
            this.baseLabelPosition_Bot.TabIndex = 143;
            this.baseLabelPosition_Bot.Text = "BOT\r\n[ Get ]";
            this.baseLabelPosition_Bot.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.baseLabelPosition_Bot.Click += new System.EventHandler(this.baseLabelPosition_Bot_Click);
            // 
            // baseLabelPosition_Mid
            // 
            this.baseLabelPosition_Mid.BackColor = System.Drawing.Color.Black;
            this.baseLabelPosition_Mid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabelPosition_Mid.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabelPosition_Mid.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabelPosition_Mid.Location = new System.Drawing.Point(12, 128);
            this.baseLabelPosition_Mid.Name = "baseLabelPosition_Mid";
            this.baseLabelPosition_Mid.Size = new System.Drawing.Size(65, 59);
            this.baseLabelPosition_Mid.TabIndex = 143;
            this.baseLabelPosition_Mid.Text = "MID\r\n[ Get ]";
            this.baseLabelPosition_Mid.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.baseLabelPosition_Mid.Click += new System.EventHandler(this.baseLabelPosition_Mid_Click);
            // 
            // baseLabelPosition_Top
            // 
            this.baseLabelPosition_Top.BackColor = System.Drawing.Color.LightGreen;
            this.baseLabelPosition_Top.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabelPosition_Top.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabelPosition_Top.ForeColor = System.Drawing.Color.Black;
            this.baseLabelPosition_Top.Location = new System.Drawing.Point(12, 63);
            this.baseLabelPosition_Top.Name = "baseLabelPosition_Top";
            this.baseLabelPosition_Top.Size = new System.Drawing.Size(65, 59);
            this.baseLabelPosition_Top.TabIndex = 143;
            this.baseLabelPosition_Top.Text = "TOP\r\n[ Get ]";
            this.baseLabelPosition_Top.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.baseLabelPosition_Top.Click += new System.EventHandler(this.baseLabelPosition_Top_Click);
            // 
            // baseButton_Y_Pos_GO3
            // 
            this.baseButton_Y_Pos_GO3.BackColor = System.Drawing.Color.LightGray;
            this.baseButton_Y_Pos_GO3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButton_Y_Pos_GO3.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButton_Y_Pos_GO3.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButton_Y_Pos_GO3.Location = new System.Drawing.Point(172, 194);
            this.baseButton_Y_Pos_GO3.Name = "baseButton_Y_Pos_GO3";
            this.baseButton_Y_Pos_GO3.Size = new System.Drawing.Size(76, 58);
            this.baseButton_Y_Pos_GO3.TabIndex = 122;
            this.baseButton_Y_Pos_GO3.Text = "이동  ▽";
            this.baseButton_Y_Pos_GO3.UseVisualStyleBackColor = false;
            this.baseButton_Y_Pos_GO3.Click += new System.EventHandler(this.baseButton_Y_Pos_GO3_Click);
            // 
            // tb_Axis_X_RIGHT
            // 
            this.tb_Axis_X_RIGHT.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Axis_X_RIGHT.Location = new System.Drawing.Point(462, 155);
            this.tb_Axis_X_RIGHT.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_Axis_X_RIGHT.Name = "tb_Axis_X_RIGHT";
            this.tb_Axis_X_RIGHT.Size = new System.Drawing.Size(89, 29);
            this.tb_Axis_X_RIGHT.TabIndex = 120;
            this.tb_Axis_X_RIGHT.TabStop = false;
            this.tb_Axis_X_RIGHT.Text = "0.000";
            this.tb_Axis_X_RIGHT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseLabel1
            // 
            this.baseLabel1.BackColor = System.Drawing.Color.DarkGray;
            this.baseLabel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel1.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel1.ForeColor = System.Drawing.Color.Black;
            this.baseLabel1.Location = new System.Drawing.Point(79, 193);
            this.baseLabel1.Name = "baseLabel1";
            this.baseLabel1.Size = new System.Drawing.Size(90, 28);
            this.baseLabel1.TabIndex = 119;
            this.baseLabel1.Text = "Vi. Y   (↕)";
            this.baseLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel7
            // 
            this.baseLabel7.BackColor = System.Drawing.Color.DarkGray;
            this.baseLabel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel7.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel7.ForeColor = System.Drawing.Color.Black;
            this.baseLabel7.Location = new System.Drawing.Point(462, 127);
            this.baseLabel7.Name = "baseLabel7";
            this.baseLabel7.Size = new System.Drawing.Size(89, 26);
            this.baseLabel7.TabIndex = 118;
            this.baseLabel7.Text = "Vi. X   (↔)";
            this.baseLabel7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseButton_Y_Pos_GO2
            // 
            this.baseButton_Y_Pos_GO2.BackColor = System.Drawing.Color.LightGray;
            this.baseButton_Y_Pos_GO2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButton_Y_Pos_GO2.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButton_Y_Pos_GO2.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButton_Y_Pos_GO2.Location = new System.Drawing.Point(172, 129);
            this.baseButton_Y_Pos_GO2.Name = "baseButton_Y_Pos_GO2";
            this.baseButton_Y_Pos_GO2.Size = new System.Drawing.Size(76, 58);
            this.baseButton_Y_Pos_GO2.TabIndex = 117;
            this.baseButton_Y_Pos_GO2.Text = "이동  ▣";
            this.baseButton_Y_Pos_GO2.UseVisualStyleBackColor = false;
            this.baseButton_Y_Pos_GO2.Click += new System.EventHandler(this.baseButton_Y_Pos_GO2_Click);
            // 
            // tb_Axis_Y_MID
            // 
            this.tb_Axis_Y_MID.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Axis_Y_MID.Location = new System.Drawing.Point(80, 158);
            this.tb_Axis_Y_MID.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_Axis_Y_MID.Name = "tb_Axis_Y_MID";
            this.tb_Axis_Y_MID.Size = new System.Drawing.Size(89, 29);
            this.tb_Axis_Y_MID.TabIndex = 116;
            this.tb_Axis_Y_MID.TabStop = false;
            this.tb_Axis_Y_MID.Text = "0.000";
            this.tb_Axis_Y_MID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_Axis_X_CENTER
            // 
            this.tb_Axis_X_CENTER.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Axis_X_CENTER.Location = new System.Drawing.Point(364, 155);
            this.tb_Axis_X_CENTER.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_Axis_X_CENTER.Name = "tb_Axis_X_CENTER";
            this.tb_Axis_X_CENTER.Size = new System.Drawing.Size(89, 29);
            this.tb_Axis_X_CENTER.TabIndex = 115;
            this.tb_Axis_X_CENTER.TabStop = false;
            this.tb_Axis_X_CENTER.Text = "0.000";
            this.tb_Axis_X_CENTER.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseLabel10
            // 
            this.baseLabel10.BackColor = System.Drawing.Color.DarkGray;
            this.baseLabel10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel10.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel10.ForeColor = System.Drawing.Color.Black;
            this.baseLabel10.Location = new System.Drawing.Point(79, 128);
            this.baseLabel10.Name = "baseLabel10";
            this.baseLabel10.Size = new System.Drawing.Size(90, 28);
            this.baseLabel10.TabIndex = 114;
            this.baseLabel10.Text = "Vi. Y   (↕)";
            this.baseLabel10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel13
            // 
            this.baseLabel13.BackColor = System.Drawing.Color.DarkGray;
            this.baseLabel13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel13.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel13.ForeColor = System.Drawing.Color.Black;
            this.baseLabel13.Location = new System.Drawing.Point(364, 127);
            this.baseLabel13.Name = "baseLabel13";
            this.baseLabel13.Size = new System.Drawing.Size(89, 26);
            this.baseLabel13.TabIndex = 113;
            this.baseLabel13.Text = "Vi. X   (↔)";
            this.baseLabel13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseButton_Y_Pos_GO1
            // 
            this.baseButton_Y_Pos_GO1.BackColor = System.Drawing.Color.LightGray;
            this.baseButton_Y_Pos_GO1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButton_Y_Pos_GO1.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButton_Y_Pos_GO1.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButton_Y_Pos_GO1.Location = new System.Drawing.Point(172, 64);
            this.baseButton_Y_Pos_GO1.Name = "baseButton_Y_Pos_GO1";
            this.baseButton_Y_Pos_GO1.Size = new System.Drawing.Size(76, 58);
            this.baseButton_Y_Pos_GO1.TabIndex = 112;
            this.baseButton_Y_Pos_GO1.Text = "이동  ▲";
            this.baseButton_Y_Pos_GO1.UseVisualStyleBackColor = false;
            this.baseButton_Y_Pos_GO1.Click += new System.EventHandler(this.baseButton_Y_Pos_GO1_Click);
            // 
            // tb_Axis_Y_TOP
            // 
            this.tb_Axis_Y_TOP.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Axis_Y_TOP.Location = new System.Drawing.Point(80, 93);
            this.tb_Axis_Y_TOP.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_Axis_Y_TOP.Name = "tb_Axis_Y_TOP";
            this.tb_Axis_Y_TOP.Size = new System.Drawing.Size(89, 29);
            this.tb_Axis_Y_TOP.TabIndex = 111;
            this.tb_Axis_Y_TOP.TabStop = false;
            this.tb_Axis_Y_TOP.Text = "0.000";
            this.tb_Axis_Y_TOP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_Axis_X_LEFT
            // 
            this.tb_Axis_X_LEFT.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Axis_X_LEFT.Location = new System.Drawing.Point(266, 155);
            this.tb_Axis_X_LEFT.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_Axis_X_LEFT.Name = "tb_Axis_X_LEFT";
            this.tb_Axis_X_LEFT.Size = new System.Drawing.Size(89, 29);
            this.tb_Axis_X_LEFT.TabIndex = 110;
            this.tb_Axis_X_LEFT.TabStop = false;
            this.tb_Axis_X_LEFT.Text = "0.000";
            this.tb_Axis_X_LEFT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseLabel18
            // 
            this.baseLabel18.BackColor = System.Drawing.Color.DarkGray;
            this.baseLabel18.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel18.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel18.ForeColor = System.Drawing.Color.Black;
            this.baseLabel18.Location = new System.Drawing.Point(79, 63);
            this.baseLabel18.Name = "baseLabel18";
            this.baseLabel18.Size = new System.Drawing.Size(90, 28);
            this.baseLabel18.TabIndex = 109;
            this.baseLabel18.Text = "Vi. Y   (↕)";
            this.baseLabel18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel19
            // 
            this.baseLabel19.BackColor = System.Drawing.Color.DarkGray;
            this.baseLabel19.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel19.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel19.ForeColor = System.Drawing.Color.Black;
            this.baseLabel19.Location = new System.Drawing.Point(266, 127);
            this.baseLabel19.Name = "baseLabel19";
            this.baseLabel19.Size = new System.Drawing.Size(89, 26);
            this.baseLabel19.TabIndex = 108;
            this.baseLabel19.Text = "Vi. X   (↔)";
            this.baseLabel19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBoxUpperLowerCameraAlignCheck
            // 
            this.groupBoxUpperLowerCameraAlignCheck.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.groupBoxUpperLowerCameraAlignCheck.Controls.Add(this.baseButton_GoPos_ReticleCenter_LowerCam);
            this.groupBoxUpperLowerCameraAlignCheck.Controls.Add(this.baseLabelPosition_LowerCamera_ReticleCenter);
            this.groupBoxUpperLowerCameraAlignCheck.Controls.Add(this.tb_Axis_EZ_ReticleCenter_LowerCam);
            this.groupBoxUpperLowerCameraAlignCheck.Controls.Add(this.baseLabelPosition_UpperCamera_ReticleCenter);
            this.groupBoxUpperLowerCameraAlignCheck.Controls.Add(this.tb_Axis_Z_ReticleCenter);
            this.groupBoxUpperLowerCameraAlignCheck.Controls.Add(this.baseLabel36);
            this.groupBoxUpperLowerCameraAlignCheck.Controls.Add(this.tb_Axis_Y_ReticleCenter);
            this.groupBoxUpperLowerCameraAlignCheck.Controls.Add(this.baseLabel37);
            this.groupBoxUpperLowerCameraAlignCheck.Controls.Add(this.tb_Axis_X_ReticleCenter);
            this.groupBoxUpperLowerCameraAlignCheck.Controls.Add(this.baseLabel38);
            this.groupBoxUpperLowerCameraAlignCheck.Controls.Add(this.tb_Axis_EZ_ReticleCenter_UpperCam);
            this.groupBoxUpperLowerCameraAlignCheck.Controls.Add(this.baseLabel35);
            this.groupBoxUpperLowerCameraAlignCheck.Controls.Add(this.tb_Axis_W_ReticleCenter);
            this.groupBoxUpperLowerCameraAlignCheck.Controls.Add(this.tb_Axis_V_ReticleCenter);
            this.groupBoxUpperLowerCameraAlignCheck.Controls.Add(this.tb_Axis_U_ReticleCenter);
            this.groupBoxUpperLowerCameraAlignCheck.Controls.Add(this.baseLabel21);
            this.groupBoxUpperLowerCameraAlignCheck.Controls.Add(this.baseLabel33);
            this.groupBoxUpperLowerCameraAlignCheck.Controls.Add(this.baseLabel34);
            this.groupBoxUpperLowerCameraAlignCheck.Controls.Add(this.baseLabelPosition_StageCamera_ReticleCenter);
            this.groupBoxUpperLowerCameraAlignCheck.Controls.Add(this.baseButton_GoPos_ReticleCenter_UpperCam);
            this.groupBoxUpperLowerCameraAlignCheck.Controls.Add(this.baseButton_GoPos_ReticleCenter_StageCamXY);
            this.groupBoxUpperLowerCameraAlignCheck.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBoxUpperLowerCameraAlignCheck.ForeColor = System.Drawing.Color.White;
            this.groupBoxUpperLowerCameraAlignCheck.Location = new System.Drawing.Point(1352, 498);
            this.groupBoxUpperLowerCameraAlignCheck.Name = "groupBoxUpperLowerCameraAlignCheck";
            this.groupBoxUpperLowerCameraAlignCheck.Size = new System.Drawing.Size(387, 265);
            this.groupBoxUpperLowerCameraAlignCheck.TabIndex = 153;
            this.groupBoxUpperLowerCameraAlignCheck.TabStop = false;
            this.groupBoxUpperLowerCameraAlignCheck.Text = " [ 카메라 광축 얼라인 확인   ( 상부 / 하부 ) ] ";
            // 
            // baseButton_GoPos_ReticleCenter_LowerCam
            // 
            this.baseButton_GoPos_ReticleCenter_LowerCam.BackColor = System.Drawing.Color.LightGray;
            this.baseButton_GoPos_ReticleCenter_LowerCam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButton_GoPos_ReticleCenter_LowerCam.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButton_GoPos_ReticleCenter_LowerCam.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButton_GoPos_ReticleCenter_LowerCam.Location = new System.Drawing.Point(285, 197);
            this.baseButton_GoPos_ReticleCenter_LowerCam.Name = "baseButton_GoPos_ReticleCenter_LowerCam";
            this.baseButton_GoPos_ReticleCenter_LowerCam.Size = new System.Drawing.Size(90, 55);
            this.baseButton_GoPos_ReticleCenter_LowerCam.TabIndex = 171;
            this.baseButton_GoPos_ReticleCenter_LowerCam.Text = "위치  이동";
            this.baseButton_GoPos_ReticleCenter_LowerCam.UseVisualStyleBackColor = false;
            this.baseButton_GoPos_ReticleCenter_LowerCam.Click += new System.EventHandler(this.baseButton_GoPos_ReticleCenter_LowerCam_Click);
            // 
            // baseLabelPosition_LowerCamera_ReticleCenter
            // 
            this.baseLabelPosition_LowerCamera_ReticleCenter.BackColor = System.Drawing.Color.Black;
            this.baseLabelPosition_LowerCamera_ReticleCenter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabelPosition_LowerCamera_ReticleCenter.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelPosition_LowerCamera_ReticleCenter.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabelPosition_LowerCamera_ReticleCenter.Location = new System.Drawing.Point(227, 166);
            this.baseLabelPosition_LowerCamera_ReticleCenter.Name = "baseLabelPosition_LowerCamera_ReticleCenter";
            this.baseLabelPosition_LowerCamera_ReticleCenter.Size = new System.Drawing.Size(55, 86);
            this.baseLabelPosition_LowerCamera_ReticleCenter.TabIndex = 170;
            this.baseLabelPosition_LowerCamera_ReticleCenter.Text = "하부\r\n카메라\r\n\r\n[ Get ]";
            this.baseLabelPosition_LowerCamera_ReticleCenter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.baseLabelPosition_LowerCamera_ReticleCenter.Click += new System.EventHandler(this.baseLabelPosition_LowerCamera_ReticleCenter_Click);
            // 
            // tb_Axis_EZ_ReticleCenter_LowerCam
            // 
            this.tb_Axis_EZ_ReticleCenter_LowerCam.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Axis_EZ_ReticleCenter_LowerCam.Location = new System.Drawing.Point(285, 166);
            this.tb_Axis_EZ_ReticleCenter_LowerCam.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_Axis_EZ_ReticleCenter_LowerCam.Name = "tb_Axis_EZ_ReticleCenter_LowerCam";
            this.tb_Axis_EZ_ReticleCenter_LowerCam.Size = new System.Drawing.Size(89, 29);
            this.tb_Axis_EZ_ReticleCenter_LowerCam.TabIndex = 169;
            this.tb_Axis_EZ_ReticleCenter_LowerCam.TabStop = false;
            this.tb_Axis_EZ_ReticleCenter_LowerCam.Text = "0.000";
            this.tb_Axis_EZ_ReticleCenter_LowerCam.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseLabelPosition_UpperCamera_ReticleCenter
            // 
            this.baseLabelPosition_UpperCamera_ReticleCenter.BackColor = System.Drawing.Color.Black;
            this.baseLabelPosition_UpperCamera_ReticleCenter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabelPosition_UpperCamera_ReticleCenter.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelPosition_UpperCamera_ReticleCenter.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabelPosition_UpperCamera_ReticleCenter.Location = new System.Drawing.Point(227, 62);
            this.baseLabelPosition_UpperCamera_ReticleCenter.Name = "baseLabelPosition_UpperCamera_ReticleCenter";
            this.baseLabelPosition_UpperCamera_ReticleCenter.Size = new System.Drawing.Size(55, 86);
            this.baseLabelPosition_UpperCamera_ReticleCenter.TabIndex = 167;
            this.baseLabelPosition_UpperCamera_ReticleCenter.Text = "상부\r\n카메라\r\n\r\n[ Get ]";
            this.baseLabelPosition_UpperCamera_ReticleCenter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.baseLabelPosition_UpperCamera_ReticleCenter.Click += new System.EventHandler(this.baseLabelPosition_UpperCamera_ReticleCenter_Click);
            // 
            // tb_Axis_Z_ReticleCenter
            // 
            this.tb_Axis_Z_ReticleCenter.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Axis_Z_ReticleCenter.Location = new System.Drawing.Point(121, 181);
            this.tb_Axis_Z_ReticleCenter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_Axis_Z_ReticleCenter.Name = "tb_Axis_Z_ReticleCenter";
            this.tb_Axis_Z_ReticleCenter.Size = new System.Drawing.Size(89, 29);
            this.tb_Axis_Z_ReticleCenter.TabIndex = 166;
            this.tb_Axis_Z_ReticleCenter.TabStop = false;
            this.tb_Axis_Z_ReticleCenter.Text = "0.000";
            this.tb_Axis_Z_ReticleCenter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseLabel36
            // 
            this.baseLabel36.BackColor = System.Drawing.Color.DarkGray;
            this.baseLabel36.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel36.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel36.ForeColor = System.Drawing.Color.Black;
            this.baseLabel36.Location = new System.Drawing.Point(76, 181);
            this.baseLabel36.Name = "baseLabel36";
            this.baseLabel36.Size = new System.Drawing.Size(43, 29);
            this.baseLabel36.TabIndex = 165;
            this.baseLabel36.Text = "Vi. Z";
            this.baseLabel36.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tb_Axis_Y_ReticleCenter
            // 
            this.tb_Axis_Y_ReticleCenter.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Axis_Y_ReticleCenter.Location = new System.Drawing.Point(121, 151);
            this.tb_Axis_Y_ReticleCenter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_Axis_Y_ReticleCenter.Name = "tb_Axis_Y_ReticleCenter";
            this.tb_Axis_Y_ReticleCenter.Size = new System.Drawing.Size(89, 29);
            this.tb_Axis_Y_ReticleCenter.TabIndex = 164;
            this.tb_Axis_Y_ReticleCenter.TabStop = false;
            this.tb_Axis_Y_ReticleCenter.Text = "0.000";
            this.tb_Axis_Y_ReticleCenter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseLabel37
            // 
            this.baseLabel37.BackColor = System.Drawing.Color.DarkGray;
            this.baseLabel37.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel37.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel37.ForeColor = System.Drawing.Color.Black;
            this.baseLabel37.Location = new System.Drawing.Point(76, 151);
            this.baseLabel37.Name = "baseLabel37";
            this.baseLabel37.Size = new System.Drawing.Size(43, 29);
            this.baseLabel37.TabIndex = 163;
            this.baseLabel37.Text = "Vi. Y";
            this.baseLabel37.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tb_Axis_X_ReticleCenter
            // 
            this.tb_Axis_X_ReticleCenter.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Axis_X_ReticleCenter.Location = new System.Drawing.Point(121, 121);
            this.tb_Axis_X_ReticleCenter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_Axis_X_ReticleCenter.Name = "tb_Axis_X_ReticleCenter";
            this.tb_Axis_X_ReticleCenter.Size = new System.Drawing.Size(89, 29);
            this.tb_Axis_X_ReticleCenter.TabIndex = 162;
            this.tb_Axis_X_ReticleCenter.TabStop = false;
            this.tb_Axis_X_ReticleCenter.Text = "0.000";
            this.tb_Axis_X_ReticleCenter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseLabel38
            // 
            this.baseLabel38.BackColor = System.Drawing.Color.DarkGray;
            this.baseLabel38.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel38.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel38.ForeColor = System.Drawing.Color.Black;
            this.baseLabel38.Location = new System.Drawing.Point(76, 121);
            this.baseLabel38.Name = "baseLabel38";
            this.baseLabel38.Size = new System.Drawing.Size(43, 29);
            this.baseLabel38.TabIndex = 161;
            this.baseLabel38.Text = "Vi. X";
            this.baseLabel38.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tb_Axis_EZ_ReticleCenter_UpperCam
            // 
            this.tb_Axis_EZ_ReticleCenter_UpperCam.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Axis_EZ_ReticleCenter_UpperCam.Location = new System.Drawing.Point(285, 62);
            this.tb_Axis_EZ_ReticleCenter_UpperCam.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_Axis_EZ_ReticleCenter_UpperCam.Name = "tb_Axis_EZ_ReticleCenter_UpperCam";
            this.tb_Axis_EZ_ReticleCenter_UpperCam.Size = new System.Drawing.Size(89, 29);
            this.tb_Axis_EZ_ReticleCenter_UpperCam.TabIndex = 160;
            this.tb_Axis_EZ_ReticleCenter_UpperCam.TabStop = false;
            this.tb_Axis_EZ_ReticleCenter_UpperCam.Text = "0.000";
            this.tb_Axis_EZ_ReticleCenter_UpperCam.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseLabel35
            // 
            this.baseLabel35.BackColor = System.Drawing.Color.DarkGray;
            this.baseLabel35.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel35.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel35.ForeColor = System.Drawing.Color.Black;
            this.baseLabel35.Location = new System.Drawing.Point(226, 29);
            this.baseLabel35.Name = "baseLabel35";
            this.baseLabel35.Size = new System.Drawing.Size(149, 29);
            this.baseLabel35.TabIndex = 159;
            this.baseLabel35.Text = "엘리베이터 - Z 축";
            this.baseLabel35.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tb_Axis_W_ReticleCenter
            // 
            this.tb_Axis_W_ReticleCenter.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Axis_W_ReticleCenter.Location = new System.Drawing.Point(121, 89);
            this.tb_Axis_W_ReticleCenter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_Axis_W_ReticleCenter.Name = "tb_Axis_W_ReticleCenter";
            this.tb_Axis_W_ReticleCenter.Size = new System.Drawing.Size(89, 29);
            this.tb_Axis_W_ReticleCenter.TabIndex = 158;
            this.tb_Axis_W_ReticleCenter.TabStop = false;
            this.tb_Axis_W_ReticleCenter.Text = "0.000";
            this.tb_Axis_W_ReticleCenter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_Axis_V_ReticleCenter
            // 
            this.tb_Axis_V_ReticleCenter.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Axis_V_ReticleCenter.Location = new System.Drawing.Point(121, 59);
            this.tb_Axis_V_ReticleCenter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_Axis_V_ReticleCenter.Name = "tb_Axis_V_ReticleCenter";
            this.tb_Axis_V_ReticleCenter.Size = new System.Drawing.Size(89, 29);
            this.tb_Axis_V_ReticleCenter.TabIndex = 157;
            this.tb_Axis_V_ReticleCenter.TabStop = false;
            this.tb_Axis_V_ReticleCenter.Text = "0.000";
            this.tb_Axis_V_ReticleCenter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_Axis_U_ReticleCenter
            // 
            this.tb_Axis_U_ReticleCenter.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Axis_U_ReticleCenter.Location = new System.Drawing.Point(121, 29);
            this.tb_Axis_U_ReticleCenter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_Axis_U_ReticleCenter.Name = "tb_Axis_U_ReticleCenter";
            this.tb_Axis_U_ReticleCenter.Size = new System.Drawing.Size(89, 29);
            this.tb_Axis_U_ReticleCenter.TabIndex = 156;
            this.tb_Axis_U_ReticleCenter.TabStop = false;
            this.tb_Axis_U_ReticleCenter.Text = "0.000";
            this.tb_Axis_U_ReticleCenter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseLabel21
            // 
            this.baseLabel21.BackColor = System.Drawing.Color.DarkGray;
            this.baseLabel21.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel21.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel21.ForeColor = System.Drawing.Color.Black;
            this.baseLabel21.Location = new System.Drawing.Point(76, 89);
            this.baseLabel21.Name = "baseLabel21";
            this.baseLabel21.Size = new System.Drawing.Size(43, 29);
            this.baseLabel21.TabIndex = 155;
            this.baseLabel21.Text = "W";
            this.baseLabel21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel33
            // 
            this.baseLabel33.BackColor = System.Drawing.Color.DarkGray;
            this.baseLabel33.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel33.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel33.ForeColor = System.Drawing.Color.Black;
            this.baseLabel33.Location = new System.Drawing.Point(76, 59);
            this.baseLabel33.Name = "baseLabel33";
            this.baseLabel33.Size = new System.Drawing.Size(43, 29);
            this.baseLabel33.TabIndex = 154;
            this.baseLabel33.Text = "V";
            this.baseLabel33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel34
            // 
            this.baseLabel34.BackColor = System.Drawing.Color.DarkGray;
            this.baseLabel34.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel34.Font = new System.Drawing.Font("나눔바른고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel34.ForeColor = System.Drawing.Color.Black;
            this.baseLabel34.Location = new System.Drawing.Point(76, 29);
            this.baseLabel34.Name = "baseLabel34";
            this.baseLabel34.Size = new System.Drawing.Size(43, 29);
            this.baseLabel34.TabIndex = 153;
            this.baseLabel34.Text = "U";
            this.baseLabel34.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabelPosition_StageCamera_ReticleCenter
            // 
            this.baseLabelPosition_StageCamera_ReticleCenter.BackColor = System.Drawing.Color.Black;
            this.baseLabelPosition_StageCamera_ReticleCenter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabelPosition_StageCamera_ReticleCenter.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelPosition_StageCamera_ReticleCenter.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabelPosition_StageCamera_ReticleCenter.Location = new System.Drawing.Point(12, 29);
            this.baseLabelPosition_StageCamera_ReticleCenter.Name = "baseLabelPosition_StageCamera_ReticleCenter";
            this.baseLabelPosition_StageCamera_ReticleCenter.Size = new System.Drawing.Size(62, 223);
            this.baseLabelPosition_StageCamera_ReticleCenter.TabIndex = 143;
            this.baseLabelPosition_StageCamera_ReticleCenter.Text = "PAK\r\n카메라\r\n\r\n\r\n레티클\r\n글래스\r\n위치\r\n\r\n[ Get ]";
            this.baseLabelPosition_StageCamera_ReticleCenter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.baseLabelPosition_StageCamera_ReticleCenter.Click += new System.EventHandler(this.baseLabelPosition_StageCamera_ReticleCenter_Click);
            // 
            // baseButton_GoPos_ReticleCenter_UpperCam
            // 
            this.baseButton_GoPos_ReticleCenter_UpperCam.BackColor = System.Drawing.Color.LightGray;
            this.baseButton_GoPos_ReticleCenter_UpperCam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButton_GoPos_ReticleCenter_UpperCam.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButton_GoPos_ReticleCenter_UpperCam.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButton_GoPos_ReticleCenter_UpperCam.Location = new System.Drawing.Point(285, 93);
            this.baseButton_GoPos_ReticleCenter_UpperCam.Name = "baseButton_GoPos_ReticleCenter_UpperCam";
            this.baseButton_GoPos_ReticleCenter_UpperCam.Size = new System.Drawing.Size(90, 55);
            this.baseButton_GoPos_ReticleCenter_UpperCam.TabIndex = 122;
            this.baseButton_GoPos_ReticleCenter_UpperCam.Text = "위치  이동";
            this.baseButton_GoPos_ReticleCenter_UpperCam.UseVisualStyleBackColor = false;
            this.baseButton_GoPos_ReticleCenter_UpperCam.Click += new System.EventHandler(this.baseButton_GoPos_ReticleCenter_UpperCam_Click);
            // 
            // baseButton_GoPos_ReticleCenter_StageCamXY
            // 
            this.baseButton_GoPos_ReticleCenter_StageCamXY.BackColor = System.Drawing.Color.LightGray;
            this.baseButton_GoPos_ReticleCenter_StageCamXY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButton_GoPos_ReticleCenter_StageCamXY.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButton_GoPos_ReticleCenter_StageCamXY.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButton_GoPos_ReticleCenter_StageCamXY.Location = new System.Drawing.Point(76, 215);
            this.baseButton_GoPos_ReticleCenter_StageCamXY.Name = "baseButton_GoPos_ReticleCenter_StageCamXY";
            this.baseButton_GoPos_ReticleCenter_StageCamXY.Size = new System.Drawing.Size(135, 37);
            this.baseButton_GoPos_ReticleCenter_StageCamXY.TabIndex = 117;
            this.baseButton_GoPos_ReticleCenter_StageCamXY.Text = "레티클 위치  이동";
            this.baseButton_GoPos_ReticleCenter_StageCamXY.UseVisualStyleBackColor = false;
            this.baseButton_GoPos_ReticleCenter_StageCamXY.Click += new System.EventHandler(this.baseButton_GoPos_ReticleCenter_StageCamXY_Click);
            // 
            // btnReticlePos_LowerCam_GO
            // 
            this.btnReticlePos_LowerCam_GO.BackColor = System.Drawing.Color.LightGray;
            this.btnReticlePos_LowerCam_GO.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnReticlePos_LowerCam_GO.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnReticlePos_LowerCam_GO.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnReticlePos_LowerCam_GO.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnReticlePos_LowerCam_GO.ForeColor = System.Drawing.Color.DarkRed;
            this.btnReticlePos_LowerCam_GO.Location = new System.Drawing.Point(1727, 411);
            this.btnReticlePos_LowerCam_GO.Name = "btnReticlePos_LowerCam_GO";
            this.btnReticlePos_LowerCam_GO.Size = new System.Drawing.Size(174, 69);
            this.btnReticlePos_LowerCam_GO.TabIndex = 175;
            this.btnReticlePos_LowerCam_GO.Text = "레티클 센터 확인\r\n위치 이동 [하부 캠]";
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
            this.btnReticlePos_UpperCam_GO.Location = new System.Drawing.Point(1727, 338);
            this.btnReticlePos_UpperCam_GO.Name = "btnReticlePos_UpperCam_GO";
            this.btnReticlePos_UpperCam_GO.Size = new System.Drawing.Size(174, 69);
            this.btnReticlePos_UpperCam_GO.TabIndex = 174;
            this.btnReticlePos_UpperCam_GO.Text = "레티클 센터 확인\r\n위치 이동 [상부 캠]";
            this.btnReticlePos_UpperCam_GO.UseVisualStyleBackColor = false;
            this.btnReticlePos_UpperCam_GO.Click += new System.EventHandler(this.btnReticlePos_UpperCam_GO_Click);
            // 
            // btnSafetyPos_CamXY_GO
            // 
            this.btnSafetyPos_CamXY_GO.BackColor = System.Drawing.Color.LightGray;
            this.btnSafetyPos_CamXY_GO.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnSafetyPos_CamXY_GO.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnSafetyPos_CamXY_GO.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnSafetyPos_CamXY_GO.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSafetyPos_CamXY_GO.ForeColor = System.Drawing.Color.DarkRed;
            this.btnSafetyPos_CamXY_GO.Location = new System.Drawing.Point(1727, 3);
            this.btnSafetyPos_CamXY_GO.Name = "btnSafetyPos_CamXY_GO";
            this.btnSafetyPos_CamXY_GO.Size = new System.Drawing.Size(174, 69);
            this.btnSafetyPos_CamXY_GO.TabIndex = 176;
            this.btnSafetyPos_CamXY_GO.Text = "[ 비전 카메라 ]\r\n안전 위치 이동";
            this.btnSafetyPos_CamXY_GO.UseVisualStyleBackColor = false;
            this.btnSafetyPos_CamXY_GO.Click += new System.EventHandler(this.btnSafetyPos_CamXY_GO_Click);
            // 
            // btnLoadingPos_Wafer_GO
            // 
            this.btnLoadingPos_Wafer_GO.BackColor = System.Drawing.Color.LightGray;
            this.btnLoadingPos_Wafer_GO.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnLoadingPos_Wafer_GO.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnLoadingPos_Wafer_GO.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnLoadingPos_Wafer_GO.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLoadingPos_Wafer_GO.ForeColor = System.Drawing.Color.DarkRed;
            this.btnLoadingPos_Wafer_GO.Location = new System.Drawing.Point(1727, 90);
            this.btnLoadingPos_Wafer_GO.Name = "btnLoadingPos_Wafer_GO";
            this.btnLoadingPos_Wafer_GO.Size = new System.Drawing.Size(174, 69);
            this.btnLoadingPos_Wafer_GO.TabIndex = 176;
            this.btnLoadingPos_Wafer_GO.Text = "[ 씬 - 척 && 웨이퍼 ]\r\n투입 위치 이동";
            this.btnLoadingPos_Wafer_GO.UseVisualStyleBackColor = false;
            this.btnLoadingPos_Wafer_GO.Click += new System.EventHandler(this.btnLoadingPos_Wafer_GO_Click);
            // 
            // btnUpperCamera_StartLive
            // 
            this.btnUpperCamera_StartLive.BackColor = System.Drawing.Color.LightGray;
            this.btnUpperCamera_StartLive.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnUpperCamera_StartLive.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnUpperCamera_StartLive.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnUpperCamera_StartLive.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnUpperCamera_StartLive.ForeColor = System.Drawing.Color.DarkRed;
            this.btnUpperCamera_StartLive.Location = new System.Drawing.Point(8, 94);
            this.btnUpperCamera_StartLive.Name = "btnUpperCamera_StartLive";
            this.btnUpperCamera_StartLive.Size = new System.Drawing.Size(83, 62);
            this.btnUpperCamera_StartLive.TabIndex = 177;
            this.btnUpperCamera_StartLive.Text = "라이브\r\n이미지";
            this.btnUpperCamera_StartLive.UseVisualStyleBackColor = false;
            this.btnUpperCamera_StartLive.Click += new System.EventHandler(this.btnUpperCamera_StartLive_Click);
            // 
            // btnLoadingPos_ProbeCard_GO
            // 
            this.btnLoadingPos_ProbeCard_GO.BackColor = System.Drawing.Color.LightGray;
            this.btnLoadingPos_ProbeCard_GO.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnLoadingPos_ProbeCard_GO.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnLoadingPos_ProbeCard_GO.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnLoadingPos_ProbeCard_GO.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLoadingPos_ProbeCard_GO.ForeColor = System.Drawing.Color.DarkRed;
            this.btnLoadingPos_ProbeCard_GO.Location = new System.Drawing.Point(1727, 177);
            this.btnLoadingPos_ProbeCard_GO.Name = "btnLoadingPos_ProbeCard_GO";
            this.btnLoadingPos_ProbeCard_GO.Size = new System.Drawing.Size(174, 69);
            this.btnLoadingPos_ProbeCard_GO.TabIndex = 178;
            this.btnLoadingPos_ProbeCard_GO.Text = "[ 프로브 카드 ]\r\n고정 해제";
            this.btnLoadingPos_ProbeCard_GO.UseVisualStyleBackColor = false;
            this.btnLoadingPos_ProbeCard_GO.Click += new System.EventHandler(this.btnLoadingPos_ProbeCard_GO_Click);
            // 
            // btn_ProbeCard_Locking
            // 
            this.btn_ProbeCard_Locking.BackColor = System.Drawing.Color.LightGray;
            this.btn_ProbeCard_Locking.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btn_ProbeCard_Locking.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn_ProbeCard_Locking.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btn_ProbeCard_Locking.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_ProbeCard_Locking.ForeColor = System.Drawing.Color.DarkRed;
            this.btn_ProbeCard_Locking.Location = new System.Drawing.Point(1727, 250);
            this.btn_ProbeCard_Locking.Name = "btn_ProbeCard_Locking";
            this.btn_ProbeCard_Locking.Size = new System.Drawing.Size(174, 69);
            this.btn_ProbeCard_Locking.TabIndex = 179;
            this.btn_ProbeCard_Locking.Text = "[ 프로브 카드 ]\r\n고정";
            this.btn_ProbeCard_Locking.UseVisualStyleBackColor = false;
            this.btn_ProbeCard_Locking.Click += new System.EventHandler(this.btn_ProbeCard_Locking_Click);
            // 
            // tb_Axis_EZ_Wafer_ProbeCard_Packing
            // 
            this.tb_Axis_EZ_Wafer_ProbeCard_Packing.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_Axis_EZ_Wafer_ProbeCard_Packing.Location = new System.Drawing.Point(1812, 613);
            this.tb_Axis_EZ_Wafer_ProbeCard_Packing.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_Axis_EZ_Wafer_ProbeCard_Packing.Name = "tb_Axis_EZ_Wafer_ProbeCard_Packing";
            this.tb_Axis_EZ_Wafer_ProbeCard_Packing.Size = new System.Drawing.Size(89, 29);
            this.tb_Axis_EZ_Wafer_ProbeCard_Packing.TabIndex = 182;
            this.tb_Axis_EZ_Wafer_ProbeCard_Packing.TabStop = false;
            this.tb_Axis_EZ_Wafer_ProbeCard_Packing.Text = "0.000";
            this.tb_Axis_EZ_Wafer_ProbeCard_Packing.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnUser_Registration
            // 
            this.btnUser_Registration.BackColor = System.Drawing.Color.LightBlue;
            this.btnUser_Registration.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnUser_Registration.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnUser_Registration.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnUser_Registration.Font = new System.Drawing.Font("나눔바른고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnUser_Registration.ForeColor = System.Drawing.Color.DarkRed;
            this.btnUser_Registration.Location = new System.Drawing.Point(779, 384);
            this.btnUser_Registration.Name = "btnUser_Registration";
            this.btnUser_Registration.Size = new System.Drawing.Size(135, 77);
            this.btnUser_Registration.TabIndex = 185;
            this.btnUser_Registration.Text = "사용자 관리";
            this.btnUser_Registration.UseVisualStyleBackColor = false;
            this.btnUser_Registration.Click += new System.EventHandler(this.btnUser_Registration_Click);
            // 
            // baseLabelUVWPosition_Loading
            // 
            this.baseLabelUVWPosition_Loading.BackColor = System.Drawing.Color.LightGray;
            this.baseLabelUVWPosition_Loading.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabelUVWPosition_Loading.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelUVWPosition_Loading.ForeColor = System.Drawing.Color.DarkRed;
            this.baseLabelUVWPosition_Loading.Location = new System.Drawing.Point(476, 713);
            this.baseLabelUVWPosition_Loading.Name = "baseLabelUVWPosition_Loading";
            this.baseLabelUVWPosition_Loading.Size = new System.Drawing.Size(270, 49);
            this.baseLabelUVWPosition_Loading.TabIndex = 184;
            this.baseLabelUVWPosition_Loading.Text = "얼라인 스테이지(UVW) 의 현재 위치값 저장\r\n[  Ready  /  Load  /  Unload  ]";
            this.baseLabelUVWPosition_Loading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.baseLabelUVWPosition_Loading.Click += new System.EventHandler(this.baseLabelUVWPosition_Loading_Click);
            // 
            // baseLabelPosition_Wafer_Probecard_Packing
            // 
            this.baseLabelPosition_Wafer_Probecard_Packing.BackColor = System.Drawing.Color.Black;
            this.baseLabelPosition_Wafer_Probecard_Packing.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabelPosition_Wafer_Probecard_Packing.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelPosition_Wafer_Probecard_Packing.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabelPosition_Wafer_Probecard_Packing.Location = new System.Drawing.Point(1761, 560);
            this.baseLabelPosition_Wafer_Probecard_Packing.Name = "baseLabelPosition_Wafer_Probecard_Packing";
            this.baseLabelPosition_Wafer_Probecard_Packing.Size = new System.Drawing.Size(140, 49);
            this.baseLabelPosition_Wafer_Probecard_Packing.TabIndex = 183;
            this.baseLabelPosition_Wafer_Probecard_Packing.Text = "프로브 카드 && 씬-척\r\n패킹 위치   [ Get ]";
            this.baseLabelPosition_Wafer_Probecard_Packing.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.baseLabelPosition_Wafer_Probecard_Packing.Click += new System.EventHandler(this.baseLabelPosition_Wafer_Probecard_Packing_Click);
            // 
            // baseLabel15
            // 
            this.baseLabel15.BackColor = System.Drawing.Color.DarkGray;
            this.baseLabel15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel15.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel15.ForeColor = System.Drawing.Color.Black;
            this.baseLabel15.Location = new System.Drawing.Point(1760, 527);
            this.baseLabel15.Name = "baseLabel15";
            this.baseLabel15.Size = new System.Drawing.Size(141, 29);
            this.baseLabel15.TabIndex = 181;
            this.baseLabel15.Text = "엘리베이터 - Z 축";
            this.baseLabel15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseButton_GoPos_Wafer_ProbeCard_Packing
            // 
            this.baseButton_GoPos_Wafer_ProbeCard_Packing.BackColor = System.Drawing.Color.LightGray;
            this.baseButton_GoPos_Wafer_ProbeCard_Packing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButton_GoPos_Wafer_ProbeCard_Packing.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButton_GoPos_Wafer_ProbeCard_Packing.ForeColor = System.Drawing.Color.DarkRed;
            this.baseButton_GoPos_Wafer_ProbeCard_Packing.Location = new System.Drawing.Point(1812, 645);
            this.baseButton_GoPos_Wafer_ProbeCard_Packing.Name = "baseButton_GoPos_Wafer_ProbeCard_Packing";
            this.baseButton_GoPos_Wafer_ProbeCard_Packing.Size = new System.Drawing.Size(90, 55);
            this.baseButton_GoPos_Wafer_ProbeCard_Packing.TabIndex = 180;
            this.baseButton_GoPos_Wafer_ProbeCard_Packing.Text = "위치  이동";
            this.baseButton_GoPos_Wafer_ProbeCard_Packing.UseVisualStyleBackColor = false;
            this.baseButton_GoPos_Wafer_ProbeCard_Packing.Click += new System.EventHandler(this.baseButton_GoPos_Wafer_ProbeCard_Packing_Click);
            // 
            // m_visionImageViewer_Lower
            // 
            this.m_visionImageViewer_Lower.BackColor = System.Drawing.Color.Black;
            this.m_visionImageViewer_Lower.Camera = null;
            this.m_visionImageViewer_Lower.CameraSwitch = null;
            this.m_visionImageViewer_Lower.FrameRate = 1D;
            this.m_visionImageViewer_Lower.InputImage = null;
            this.m_visionImageViewer_Lower.IsViewCustomizedImage = false;
            this.m_visionImageViewer_Lower.Location = new System.Drawing.Point(514, 26);
            this.m_visionImageViewer_Lower.Name = "m_visionImageViewer_Lower";
            this.m_visionImageViewer_Lower.OperatingType = QMC.Common.Hmi.VisionImageViewer.OperatingTypes.Center;
            this.m_visionImageViewer_Lower.Simulated = false;
            this.m_visionImageViewer_Lower.Size = new System.Drawing.Size(400, 336);
            this.m_visionImageViewer_Lower.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.m_visionImageViewer_Lower.TabIndex = 144;
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
            this.m_visionImageViewer_Upper.Size = new System.Drawing.Size(400, 336);
            this.m_visionImageViewer_Upper.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.m_visionImageViewer_Upper.TabIndex = 143;
            this.m_visionImageViewer_Upper.TabStop = false;
            this.m_visionImageViewer_Upper.UpdateDelayTime = 160;
            this.m_visionImageViewer_Upper.VisibleCrossLine = true;
            // 
            // baseLabel_WaferChuck_Camera1
            // 
            this.baseLabel_WaferChuck_Camera1.BackColor = System.Drawing.Color.Black;
            this.baseLabel_WaferChuck_Camera1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabel_WaferChuck_Camera1.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_WaferChuck_Camera1.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel_WaferChuck_Camera1.Location = new System.Drawing.Point(658, 0);
            this.baseLabel_WaferChuck_Camera1.Name = "baseLabel_WaferChuck_Camera1";
            this.baseLabel_WaferChuck_Camera1.Size = new System.Drawing.Size(113, 25);
            this.baseLabel_WaferChuck_Camera1.TabIndex = 142;
            this.baseLabel_WaferChuck_Camera1.Text = "[ 웨이퍼 ]";
            this.baseLabel_WaferChuck_Camera1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel_ProbeCard_Camera1
            // 
            this.baseLabel_ProbeCard_Camera1.BackColor = System.Drawing.Color.Black;
            this.baseLabel_ProbeCard_Camera1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabel_ProbeCard_Camera1.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_ProbeCard_Camera1.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel_ProbeCard_Camera1.Location = new System.Drawing.Point(250, 0);
            this.baseLabel_ProbeCard_Camera1.Name = "baseLabel_ProbeCard_Camera1";
            this.baseLabel_ProbeCard_Camera1.Size = new System.Drawing.Size(113, 25);
            this.baseLabel_ProbeCard_Camera1.TabIndex = 141;
            this.baseLabel_ProbeCard_Camera1.Text = "[ 프로브 카드 ]";
            this.baseLabel_ProbeCard_Camera1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CalibrationMode_CWA150SA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.Controls.Add(this.btnUser_Registration);
            this.Controls.Add(this.baseLabelUVWPosition_Loading);
            this.Controls.Add(this.baseLabelPosition_Wafer_Probecard_Packing);
            this.Controls.Add(this.tb_Axis_EZ_Wafer_ProbeCard_Packing);
            this.Controls.Add(this.baseLabel15);
            this.Controls.Add(this.baseButton_GoPos_Wafer_ProbeCard_Packing);
            this.Controls.Add(this.btn_ProbeCard_Locking);
            this.Controls.Add(this.btnLoadingPos_ProbeCard_GO);
            this.Controls.Add(this.btnUpperCamera_StartLive);
            this.Controls.Add(this.btnLoadingPos_Wafer_GO);
            this.Controls.Add(this.btnSafetyPos_CamXY_GO);
            this.Controls.Add(this.btnReticlePos_LowerCam_GO);
            this.Controls.Add(this.btnReticlePos_UpperCam_GO);
            this.Controls.Add(this.groupBoxUpperLowerCameraAlignCheck);
            this.Controls.Add(this.groupBoxAlignCheckPosParameter);
            this.Controls.Add(this.btnUpperCamera_Init);
            this.Controls.Add(this.m_visionImageViewer_Lower);
            this.Controls.Add(this.m_visionImageViewer_Upper);
            this.Controls.Add(this.baseLabel_WaferChuck_Camera1);
            this.Controls.Add(this.baseLabel_ProbeCard_Camera1);
            this.Controls.Add(this.groupBoxAlignStageMoveLimit);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "CalibrationMode_CWA150SA";
            this.Size = new System.Drawing.Size(1910, 770);
            this.groupBoxAlignStageMoveLimit.ResumeLayout(false);
            this.groupBoxAlignStageMoveLimit.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxUVW_Stage)).EndInit();
            this.groupBoxAlignCheckPosParameter.ResumeLayout(false);
            this.groupBoxAlignCheckPosParameter.PerformLayout();
            this.groupBoxUpperLowerCameraAlignCheck.ResumeLayout(false);
            this.groupBoxUpperLowerCameraAlignCheck.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_visionImageViewer_Lower)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_visionImageViewer_Upper)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.GroupBox groupBoxAlignStageMoveLimit;
        public NeedleCalibrationJogControl needleCalibrationJogControl;

        private System.Windows.Forms.Label lblUpperCamera;
        private System.Windows.Forms.Label lblLowerCamera;
        private BaseLabel baseLabel_WaferChuck_Camera1;
        private BaseLabel baseLabel_ProbeCard_Camera1;
        private VisionImageViewer m_visionImageViewer_Upper;
        private VisionImageViewer m_visionImageViewer_Lower;
        private System.Windows.Forms.Button btnUpperCamera_Init;
        private System.Windows.Forms.GroupBox groupBoxAlignCheckPosParameter;
        private BaseLabel baseLabelPosition_Mid;
        private BaseLabel baseLabelPosition_Top;
        private BaseButton baseButton_Y_Pos_GO3;
        private System.Windows.Forms.TextBox tb_Axis_X_RIGHT;
        private BaseLabel baseLabel1;
        private BaseLabel baseLabel7;
        private BaseButton baseButton_Y_Pos_GO2;
        private System.Windows.Forms.TextBox tb_Axis_Y_MID;
        private System.Windows.Forms.TextBox tb_Axis_X_CENTER;
        private BaseLabel baseLabel10;
        private BaseLabel baseLabel13;
        private BaseButton baseButton_Y_Pos_GO1;
        private System.Windows.Forms.TextBox tb_Axis_Y_TOP;
        private System.Windows.Forms.TextBox tb_Axis_X_LEFT;
        private BaseLabel baseLabel18;
        private BaseLabel baseLabel19;
        private BaseLabel baseLabelPosition_Bot;
        private System.Windows.Forms.TextBox tb_Axis_Y_BOT;
        private BaseLabel baseLabelPosition_Left;
        private BaseLabel baseLabelPosition_RIGHT;
        private BaseLabel baseLabelPosition_Center;
        private BaseButton baseButton_X_Pos_GO3;
        private BaseButton baseButton_X_Pos_GO2;
        private BaseButton baseButton_X_Pos_GO1;
        private System.Windows.Forms.GroupBox groupBoxUpperLowerCameraAlignCheck;
        private BaseLabel baseLabelPosition_StageCamera_ReticleCenter;
        private BaseButton baseButton_GoPos_ReticleCenter_UpperCam;
        private BaseButton baseButton_GoPos_ReticleCenter_StageCamXY;
        private System.Windows.Forms.TextBox tb_Axis_EZ_ReticleCenter_UpperCam;
        private BaseLabel baseLabel35;
        private System.Windows.Forms.TextBox tb_Axis_W_ReticleCenter;
        private System.Windows.Forms.TextBox tb_Axis_V_ReticleCenter;
        private System.Windows.Forms.TextBox tb_Axis_U_ReticleCenter;
        private BaseLabel baseLabel21;
        private BaseLabel baseLabel33;
        private BaseLabel baseLabel34;
        private System.Windows.Forms.TextBox tb_Axis_Z_ReticleCenter;
        private BaseLabel baseLabel36;
        private System.Windows.Forms.TextBox tb_Axis_Y_ReticleCenter;
        private BaseLabel baseLabel37;
        private System.Windows.Forms.TextBox tb_Axis_X_ReticleCenter;
        private BaseLabel baseLabel38;
        private BaseLabel baseLabelPosition_LowerCamera_ReticleCenter;
        private System.Windows.Forms.TextBox tb_Axis_EZ_ReticleCenter_LowerCam;
        private BaseLabel baseLabelPosition_UpperCamera_ReticleCenter;
        private BaseButton baseButton_GoPos_ReticleCenter_LowerCam;
        private System.Windows.Forms.Button btnReticlePos_LowerCam_GO;
        private System.Windows.Forms.Button btnReticlePos_UpperCam_GO;
        private System.Windows.Forms.Button btnSafetyPos_CamXY_GO;
        private System.Windows.Forms.Button btnLoadingPos_Wafer_GO;
        private System.Windows.Forms.Button btnUpperCamera_StartLive;
        private System.Windows.Forms.Button btnLoadingPos_ProbeCard_GO;
        private System.Windows.Forms.Button btn_ProbeCard_Locking;
        private BaseLabel baseLabelPosition_Wafer_Probecard_Packing;
        private System.Windows.Forms.TextBox tb_Axis_EZ_Wafer_ProbeCard_Packing;
        private BaseLabel baseLabel15;
        private BaseButton baseButton_GoPos_Wafer_ProbeCard_Packing;
        private BaseLabel baseLabelAlignCenter_PackingPos;
        private BaseLabel baseLabelUVWPosition_Loading;
        private System.Windows.Forms.Button btnUser_Registration;
        private BaseLabel baseLabel_Limit_U;
        private System.Windows.Forms.PictureBox pictureBoxUVW_Stage;
        private BaseLabel baseLabel_Limit_W;
        private BaseLabel baseLabel_Limit_V;
        private System.Windows.Forms.TextBox tb_Axis_U_Limit_Plus;
        private BaseLabel baseLabel3;
        private System.Windows.Forms.TextBox tb_Axis_U_Limit_Minus;
        private BaseLabel baseLabel2;
        private System.Windows.Forms.TextBox tb_Axis_V_Limit_Minus;
        private BaseLabel baseLabel4;
        private System.Windows.Forms.TextBox tb_Axis_V_Limit_Plus;
        private BaseLabel baseLabel5;
        private System.Windows.Forms.TextBox tb_Axis_W_Limit_Plus;
        private BaseLabel baseLabel6;
        private System.Windows.Forms.TextBox tb_Axis_W_Limit_Minus;
        private BaseLabel baseLabel8;
        private BaseLabel baseLabel9;
        private System.Windows.Forms.Button btn_UVW_Limit_Range_Calc;
        private System.Windows.Forms.TextBox tb_Axis_UVW_Limit_Range;
        private System.Windows.Forms.Button btn_UVW_Limit_Save;
    }
}
