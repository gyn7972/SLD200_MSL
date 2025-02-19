using System.Drawing;
using System.Windows.Forms;

namespace SLD200_MSL
{
    public class WATGroupBox : GroupBox
    {
        private Color borderColor;
        public Color BorderColor
        {
            get { return this.borderColor; }

            set
            {
                this.borderColor = value;
                this.Invalidate();
            }

        }


        public WATGroupBox()
        {
            this.borderColor = Color.Black;
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            Size tSize = TextRenderer.MeasureText(this.Text, this.Font);
            Rectangle borderRect = e.ClipRectangle;
            borderRect.Y += tSize.Height / 2;
            borderRect.Height -= tSize.Height / 2;
            ControlPaint.DrawBorder(e.Graphics, borderRect, this.borderColor, ButtonBorderStyle.Solid);

            Rectangle textRect = e.ClipRectangle;
            textRect.X += 6;
            textRect.Width = tSize.Width;
            textRect.Height = tSize.Height;
            e.Graphics.FillRectangle(new SolidBrush(this.BackColor), textRect);
            e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), textRect);

        }

    }


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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitoring_CWA150SA));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.timer_DIO_Status = new System.Windows.Forms.Timer(this.components);
            this.btnHomeAll = new System.Windows.Forms.Button();
            this.btnUpperCamera_Init = new System.Windows.Forms.Button();
            this.btnUpperCamera_StartLive = new System.Windows.Forms.Button();
            this.button_WorkStart = new System.Windows.Forms.Button();
            this.button_Pause = new System.Windows.Forms.Button();
            this.button_OneCycleStop = new System.Windows.Forms.Button();
            this.button_WorkStop = new System.Windows.Forms.Button();
            this.tabControl_Move = new System.Windows.Forms.TabControl();
            this.tabPage_SingleMove = new System.Windows.Forms.TabPage();
            this.button_SingleRun_UnloaderToLoadingPos = new System.Windows.Forms.Button();
            this.button_SingleRun_UnloaderToUnloadingPos = new System.Windows.Forms.Button();
            this.button_SingleRun_LoaderToCart = new System.Windows.Forms.Button();
            this.button_SingleRun_LoaderToTable = new System.Windows.Forms.Button();
            this.button_SingleRun_UnloaderToTable = new System.Windows.Forms.Button();
            this.button_SingleRun_UnloaderToCart = new System.Windows.Forms.Button();
            this.tabPage_CycleRun = new System.Windows.Forms.TabPage();
            this.button_CycleRun_Stacker1_ModuleUnloadingReady = new System.Windows.Forms.Button();
            this.button_CycleRun_Stacker0_ModuleUnloadingReady = new System.Windows.Forms.Button();
            this.button_CycleRun_Stacker1_ModuleLoadingReady = new System.Windows.Forms.Button();
            this.button_CycleRun_Stacker0_ModuleLoadingReady = new System.Windows.Forms.Button();
            this.button_CycleRun_AllReject = new System.Windows.Forms.Button();
            this.button_CycleRun_LoaderReject = new System.Windows.Forms.Button();
            this.button_CycleRun_UnloaderReject = new System.Windows.Forms.Button();
            this.button_CycleRun_Align = new System.Windows.Forms.Button();
            this.button_CycleRun_Loading = new System.Windows.Forms.Button();
            this.button_CycleRun_Unloading = new System.Windows.Forms.Button();
            this.m_visionImageViewer_HighRes = new QMC.Common.Hmi.VisionImageViewer();
            this.m_visionImageViewer_LowRes = new QMC.Common.Hmi.VisionImageViewer();
            this.baseLabel_LoaderCarrier = new SLD200_MSL.BaseLabel();
            this.baseLabel_UnloaderCarrier = new SLD200_MSL.BaseLabel();
            this.baseGroupBox_Status = new SLD200_MSL.WATGroupBox();
            this.baseLabel_Status_LaserSystemFault = new SLD200_MSL.BaseLabel();
            this.pictureBox_Status_LaserSystemFault = new System.Windows.Forms.PictureBox();
            this.baseLabel_Status_DustCollector1_FanFault = new SLD200_MSL.BaseLabel();
            this.pictureBox_Status_DustCollector1_Fan_Fault = new System.Windows.Forms.PictureBox();
            this.baseLabel_Status_DustCollector0_FanFault = new SLD200_MSL.BaseLabel();
            this.pictureBox_Status_DustCollector0_Fan_Fault = new System.Windows.Forms.PictureBox();
            this.baseLabel_Status_MainCDA_Check = new SLD200_MSL.BaseLabel();
            this.pictureBox_Status_MainCDACheck = new System.Windows.Forms.PictureBox();
            this.baseLabel_Status_WaterLeakCheck = new SLD200_MSL.BaseLabel();
            this.pictureBox_Status_WaterLeakCheck = new System.Windows.Forms.PictureBox();
            this.baseLabel_Status_WaterFlowCheck = new SLD200_MSL.BaseLabel();
            this.pictureBox_Status_WaterFlowCheck = new System.Windows.Forms.PictureBox();
            this.baseLabel_Status_Coolant_Return = new SLD200_MSL.BaseLabel();
            this.pictureBox_Status_LaserCoolant_Return = new System.Windows.Forms.PictureBox();
            this.baseLabel_Status_Coolant_Supply = new SLD200_MSL.BaseLabel();
            this.pictureBox_Status_LaserCoolant_Supply = new System.Windows.Forms.PictureBox();
            this.baseGroupBox_Data = new SLD200_MSL.WATGroupBox();
            this.SiriusViewer_Main = new SpiralLab.Sirius.SiriusViewerForm();
            this.baseGroupBox_Module_Information = new SLD200_MSL.WATGroupBox();
            this.baseTextBox_TotalSocketCount = new SLD200_MSL.BaseTextBox();
            this.baseTextBox_SocketCountPerModule = new SLD200_MSL.BaseTextBox();
            this.baseLabel_SocketPerModule = new SLD200_MSL.BaseLabel();
            this.baseLabel_SocketCount = new SLD200_MSL.BaseLabel();
            this.baseGroupBox_Progress = new SLD200_MSL.WATGroupBox();
            this.numericUpDown_Module_TargetCount = new System.Windows.Forms.NumericUpDown();
            this.baseLabel_ModuleCount_Target = new SLD200_MSL.BaseLabel();
            this.button_PNLCount_Clear = new System.Windows.Forms.Button();
            this.baseLabel_PNLCount_NG = new SLD200_MSL.BaseLabel();
            this.baseTextBox1 = new SLD200_MSL.BaseTextBox();
            this.baseTextBox_Module_TotalCount = new SLD200_MSL.BaseTextBox();
            this.baseLabel_ModuleCount = new SLD200_MSL.BaseLabel();
            this.baseLabel_PNLCount_Total = new SLD200_MSL.BaseLabel();
            this.baseGroupBox_WorkingTime = new SLD200_MSL.WATGroupBox();
            this.button_AverageOneCycleTime_Clear = new System.Windows.Forms.Button();
            this.baseLabel_Average_OneCycleTime = new SLD200_MSL.BaseLabel();
            this.baseLabel_AverageOneCycle_Time = new SLD200_MSL.BaseLabel();
            this.progressBar_TotalRemained_Time = new System.Windows.Forms.ProgressBar();
            this.baseLabel_Total_RemainedTime = new SLD200_MSL.BaseLabel();
            this.baseLabel_TotalRunning_Time = new SLD200_MSL.BaseLabel();
            this.progressBar_OneCycle_Time = new System.Windows.Forms.ProgressBar();
            this.baseLabel_CurrentOneCycle_TotalTime = new SLD200_MSL.BaseLabel();
            this.baseLabel_CurrentOneCycle_ElapsedTime = new SLD200_MSL.BaseLabel();
            this.baseLabel_CurrentOneCycle_Time = new SLD200_MSL.BaseLabel();
            this.baseGroupBox_Recipe = new SLD200_MSL.WATGroupBox();
            this.baseButtonChangeRecipe = new SLD200_MSL.BaseButton();
            this.baseTextBoxCurrentRecipe = new SLD200_MSL.BaseTextBox();
            this.baseLabelCurrentRecipe = new SLD200_MSL.BaseLabel();
            this.baseLabel_HighRes_Camera = new SLD200_MSL.BaseLabel();
            this.baseLabel_LowRes_Camera = new SLD200_MSL.BaseLabel();
            this.lblStatus_ScannerPowerMeter_Connected = new SLD200_MSL.BaseLabel();
            this.tabControl_Move.SuspendLayout();
            this.tabPage_SingleMove.SuspendLayout();
            this.tabPage_CycleRun.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_visionImageViewer_HighRes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_visionImageViewer_LowRes)).BeginInit();
            this.baseGroupBox_Status.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Status_LaserSystemFault)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Status_DustCollector1_Fan_Fault)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Status_DustCollector0_Fan_Fault)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Status_MainCDACheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Status_WaterLeakCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Status_WaterFlowCheck)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Status_LaserCoolant_Return)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Status_LaserCoolant_Supply)).BeginInit();
            this.baseGroupBox_Data.SuspendLayout();
            this.baseGroupBox_Module_Information.SuspendLayout();
            this.baseGroupBox_Progress.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Module_TargetCount)).BeginInit();
            this.baseGroupBox_WorkingTime.SuspendLayout();
            this.baseGroupBox_Recipe.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // timer_DIO_Status
            // 
            this.timer_DIO_Status.Interval = 10;
            this.timer_DIO_Status.Tick += new System.EventHandler(this.timer_DIO_Status_Tick);
            // 
            // btnHomeAll
            // 
            this.btnHomeAll.BackColor = System.Drawing.Color.White;
            this.btnHomeAll.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.btnHomeAll.FlatAppearance.BorderSize = 2;
            this.btnHomeAll.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnHomeAll.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnHomeAll.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHomeAll.ForeColor = System.Drawing.Color.Black;
            this.btnHomeAll.Location = new System.Drawing.Point(1697, 4);
            this.btnHomeAll.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnHomeAll.Name = "btnHomeAll";
            this.btnHomeAll.Size = new System.Drawing.Size(208, 93);
            this.btnHomeAll.TabIndex = 127;
            this.btnHomeAll.Text = "Machine Initialize\r\nStart / Stop";
            this.btnHomeAll.UseVisualStyleBackColor = false;
            this.btnHomeAll.Click += new System.EventHandler(this.btnHomeAll_Click);
            // 
            // btnUpperCamera_Init
            // 
            this.btnUpperCamera_Init.BackColor = System.Drawing.Color.White;
            this.btnUpperCamera_Init.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.btnUpperCamera_Init.FlatAppearance.BorderSize = 2;
            this.btnUpperCamera_Init.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnUpperCamera_Init.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnUpperCamera_Init.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnUpperCamera_Init.ForeColor = System.Drawing.Color.Black;
            this.btnUpperCamera_Init.Location = new System.Drawing.Point(1374, 4);
            this.btnUpperCamera_Init.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnUpperCamera_Init.Name = "btnUpperCamera_Init";
            this.btnUpperCamera_Init.Size = new System.Drawing.Size(111, 73);
            this.btnUpperCamera_Init.TabIndex = 128;
            this.btnUpperCamera_Init.Text = "Camera\r\nInitialize";
            this.btnUpperCamera_Init.UseVisualStyleBackColor = false;
            this.btnUpperCamera_Init.Click += new System.EventHandler(this.btnUpperCamera_Init_Click);
            // 
            // btnUpperCamera_StartLive
            // 
            this.btnUpperCamera_StartLive.BackColor = System.Drawing.Color.White;
            this.btnUpperCamera_StartLive.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.btnUpperCamera_StartLive.FlatAppearance.BorderSize = 2;
            this.btnUpperCamera_StartLive.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnUpperCamera_StartLive.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnUpperCamera_StartLive.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnUpperCamera_StartLive.ForeColor = System.Drawing.Color.Black;
            this.btnUpperCamera_StartLive.Location = new System.Drawing.Point(1495, 4);
            this.btnUpperCamera_StartLive.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnUpperCamera_StartLive.Name = "btnUpperCamera_StartLive";
            this.btnUpperCamera_StartLive.Size = new System.Drawing.Size(90, 73);
            this.btnUpperCamera_StartLive.TabIndex = 129;
            this.btnUpperCamera_StartLive.Text = "Live\r\nStart";
            this.btnUpperCamera_StartLive.UseVisualStyleBackColor = false;
            this.btnUpperCamera_StartLive.Click += new System.EventHandler(this.btnUpperCamera_StartLive_Click);
            // 
            // button_WorkStart
            // 
            this.button_WorkStart.BackColor = System.Drawing.Color.White;
            this.button_WorkStart.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_WorkStart.FlatAppearance.BorderSize = 2;
            this.button_WorkStart.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_WorkStart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_WorkStart.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.button_WorkStart.ForeColor = System.Drawing.Color.Black;
            this.button_WorkStart.Location = new System.Drawing.Point(1737, 242);
            this.button_WorkStart.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_WorkStart.Name = "button_WorkStart";
            this.button_WorkStart.Size = new System.Drawing.Size(168, 91);
            this.button_WorkStart.TabIndex = 193;
            this.button_WorkStart.Text = "Work Start";
            this.button_WorkStart.UseVisualStyleBackColor = false;
            // 
            // button_Pause
            // 
            this.button_Pause.BackColor = System.Drawing.Color.White;
            this.button_Pause.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_Pause.FlatAppearance.BorderSize = 2;
            this.button_Pause.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_Pause.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_Pause.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.button_Pause.ForeColor = System.Drawing.Color.Black;
            this.button_Pause.Location = new System.Drawing.Point(1737, 438);
            this.button_Pause.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_Pause.Name = "button_Pause";
            this.button_Pause.Size = new System.Drawing.Size(168, 91);
            this.button_Pause.TabIndex = 194;
            this.button_Pause.Text = "Pause";
            this.button_Pause.UseVisualStyleBackColor = false;
            // 
            // button_OneCycleStop
            // 
            this.button_OneCycleStop.BackColor = System.Drawing.Color.White;
            this.button_OneCycleStop.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_OneCycleStop.FlatAppearance.BorderSize = 2;
            this.button_OneCycleStop.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_OneCycleStop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_OneCycleStop.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.button_OneCycleStop.ForeColor = System.Drawing.Color.Black;
            this.button_OneCycleStop.Location = new System.Drawing.Point(1737, 536);
            this.button_OneCycleStop.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_OneCycleStop.Name = "button_OneCycleStop";
            this.button_OneCycleStop.Size = new System.Drawing.Size(168, 91);
            this.button_OneCycleStop.TabIndex = 194;
            this.button_OneCycleStop.Text = "One Cycle Stop";
            this.button_OneCycleStop.UseVisualStyleBackColor = false;
            // 
            // button_WorkStop
            // 
            this.button_WorkStop.BackColor = System.Drawing.Color.White;
            this.button_WorkStop.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_WorkStop.FlatAppearance.BorderSize = 2;
            this.button_WorkStop.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_WorkStop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_WorkStop.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.button_WorkStop.ForeColor = System.Drawing.Color.Black;
            this.button_WorkStop.Location = new System.Drawing.Point(1737, 340);
            this.button_WorkStop.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_WorkStop.Name = "button_WorkStop";
            this.button_WorkStop.Size = new System.Drawing.Size(168, 91);
            this.button_WorkStop.TabIndex = 194;
            this.button_WorkStop.Text = "Work Stop";
            this.button_WorkStop.UseVisualStyleBackColor = false;
            // 
            // tabControl_Move
            // 
            this.tabControl_Move.Controls.Add(this.tabPage_SingleMove);
            this.tabControl_Move.Controls.Add(this.tabPage_CycleRun);
            this.tabControl_Move.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.tabControl_Move.ItemSize = new System.Drawing.Size(130, 32);
            this.tabControl_Move.Location = new System.Drawing.Point(1076, 415);
            this.tabControl_Move.Multiline = true;
            this.tabControl_Move.Name = "tabControl_Move";
            this.tabControl_Move.SelectedIndex = 0;
            this.tabControl_Move.Size = new System.Drawing.Size(619, 346);
            this.tabControl_Move.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl_Move.TabIndex = 200;
            // 
            // tabPage_SingleMove
            // 
            this.tabPage_SingleMove.BackColor = System.Drawing.Color.Transparent;
            this.tabPage_SingleMove.Controls.Add(this.button_SingleRun_UnloaderToLoadingPos);
            this.tabPage_SingleMove.Controls.Add(this.button_SingleRun_UnloaderToUnloadingPos);
            this.tabPage_SingleMove.Controls.Add(this.button_SingleRun_LoaderToCart);
            this.tabPage_SingleMove.Controls.Add(this.button_SingleRun_LoaderToTable);
            this.tabPage_SingleMove.Controls.Add(this.button_SingleRun_UnloaderToTable);
            this.tabPage_SingleMove.Controls.Add(this.button_SingleRun_UnloaderToCart);
            this.tabPage_SingleMove.Controls.Add(this.baseLabel_LoaderCarrier);
            this.tabPage_SingleMove.Controls.Add(this.baseLabel_UnloaderCarrier);
            this.tabPage_SingleMove.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.tabPage_SingleMove.Location = new System.Drawing.Point(4, 36);
            this.tabPage_SingleMove.Name = "tabPage_SingleMove";
            this.tabPage_SingleMove.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_SingleMove.Size = new System.Drawing.Size(611, 306);
            this.tabPage_SingleMove.TabIndex = 2;
            this.tabPage_SingleMove.Text = "[ Single Move ]";
            // 
            // button_SingleRun_UnloaderToLoadingPos
            // 
            this.button_SingleRun_UnloaderToLoadingPos.BackColor = System.Drawing.Color.Snow;
            this.button_SingleRun_UnloaderToLoadingPos.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_SingleRun_UnloaderToLoadingPos.FlatAppearance.BorderSize = 2;
            this.button_SingleRun_UnloaderToLoadingPos.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_SingleRun_UnloaderToLoadingPos.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_SingleRun_UnloaderToLoadingPos.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button_SingleRun_UnloaderToLoadingPos.ForeColor = System.Drawing.Color.Black;
            this.button_SingleRun_UnloaderToLoadingPos.Location = new System.Drawing.Point(330, 103);
            this.button_SingleRun_UnloaderToLoadingPos.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_SingleRun_UnloaderToLoadingPos.Name = "button_SingleRun_UnloaderToLoadingPos";
            this.button_SingleRun_UnloaderToLoadingPos.Size = new System.Drawing.Size(186, 53);
            this.button_SingleRun_UnloaderToLoadingPos.TabIndex = 211;
            this.button_SingleRun_UnloaderToLoadingPos.Text = "To Loading Pos.";
            this.button_SingleRun_UnloaderToLoadingPos.UseVisualStyleBackColor = false;
            // 
            // button_SingleRun_UnloaderToUnloadingPos
            // 
            this.button_SingleRun_UnloaderToUnloadingPos.BackColor = System.Drawing.Color.Snow;
            this.button_SingleRun_UnloaderToUnloadingPos.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_SingleRun_UnloaderToUnloadingPos.FlatAppearance.BorderSize = 2;
            this.button_SingleRun_UnloaderToUnloadingPos.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_SingleRun_UnloaderToUnloadingPos.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_SingleRun_UnloaderToUnloadingPos.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button_SingleRun_UnloaderToUnloadingPos.ForeColor = System.Drawing.Color.Black;
            this.button_SingleRun_UnloaderToUnloadingPos.Location = new System.Drawing.Point(41, 103);
            this.button_SingleRun_UnloaderToUnloadingPos.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_SingleRun_UnloaderToUnloadingPos.Name = "button_SingleRun_UnloaderToUnloadingPos";
            this.button_SingleRun_UnloaderToUnloadingPos.Size = new System.Drawing.Size(186, 53);
            this.button_SingleRun_UnloaderToUnloadingPos.TabIndex = 210;
            this.button_SingleRun_UnloaderToUnloadingPos.Text = "To Unloading Pos.";
            this.button_SingleRun_UnloaderToUnloadingPos.UseVisualStyleBackColor = false;
            // 
            // button_SingleRun_LoaderToCart
            // 
            this.button_SingleRun_LoaderToCart.BackColor = System.Drawing.Color.Snow;
            this.button_SingleRun_LoaderToCart.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_SingleRun_LoaderToCart.FlatAppearance.BorderSize = 2;
            this.button_SingleRun_LoaderToCart.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_SingleRun_LoaderToCart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_SingleRun_LoaderToCart.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button_SingleRun_LoaderToCart.ForeColor = System.Drawing.Color.Black;
            this.button_SingleRun_LoaderToCart.Location = new System.Drawing.Point(426, 42);
            this.button_SingleRun_LoaderToCart.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_SingleRun_LoaderToCart.Name = "button_SingleRun_LoaderToCart";
            this.button_SingleRun_LoaderToCart.Size = new System.Drawing.Size(124, 53);
            this.button_SingleRun_LoaderToCart.TabIndex = 209;
            this.button_SingleRun_LoaderToCart.Text = "To Cart";
            this.button_SingleRun_LoaderToCart.UseVisualStyleBackColor = false;
            // 
            // button_SingleRun_LoaderToTable
            // 
            this.button_SingleRun_LoaderToTable.BackColor = System.Drawing.Color.Snow;
            this.button_SingleRun_LoaderToTable.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_SingleRun_LoaderToTable.FlatAppearance.BorderSize = 2;
            this.button_SingleRun_LoaderToTable.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_SingleRun_LoaderToTable.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_SingleRun_LoaderToTable.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button_SingleRun_LoaderToTable.ForeColor = System.Drawing.Color.Black;
            this.button_SingleRun_LoaderToTable.Location = new System.Drawing.Point(295, 42);
            this.button_SingleRun_LoaderToTable.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_SingleRun_LoaderToTable.Name = "button_SingleRun_LoaderToTable";
            this.button_SingleRun_LoaderToTable.Size = new System.Drawing.Size(124, 53);
            this.button_SingleRun_LoaderToTable.TabIndex = 208;
            this.button_SingleRun_LoaderToTable.Text = "To Table";
            this.button_SingleRun_LoaderToTable.UseVisualStyleBackColor = false;
            // 
            // button_SingleRun_UnloaderToTable
            // 
            this.button_SingleRun_UnloaderToTable.BackColor = System.Drawing.Color.Snow;
            this.button_SingleRun_UnloaderToTable.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_SingleRun_UnloaderToTable.FlatAppearance.BorderSize = 2;
            this.button_SingleRun_UnloaderToTable.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_SingleRun_UnloaderToTable.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_SingleRun_UnloaderToTable.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button_SingleRun_UnloaderToTable.ForeColor = System.Drawing.Color.Black;
            this.button_SingleRun_UnloaderToTable.Location = new System.Drawing.Point(138, 42);
            this.button_SingleRun_UnloaderToTable.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_SingleRun_UnloaderToTable.Name = "button_SingleRun_UnloaderToTable";
            this.button_SingleRun_UnloaderToTable.Size = new System.Drawing.Size(124, 53);
            this.button_SingleRun_UnloaderToTable.TabIndex = 207;
            this.button_SingleRun_UnloaderToTable.Text = "To Table";
            this.button_SingleRun_UnloaderToTable.UseVisualStyleBackColor = false;
            // 
            // button_SingleRun_UnloaderToCart
            // 
            this.button_SingleRun_UnloaderToCart.BackColor = System.Drawing.Color.Snow;
            this.button_SingleRun_UnloaderToCart.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_SingleRun_UnloaderToCart.FlatAppearance.BorderSize = 2;
            this.button_SingleRun_UnloaderToCart.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_SingleRun_UnloaderToCart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_SingleRun_UnloaderToCart.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button_SingleRun_UnloaderToCart.ForeColor = System.Drawing.Color.Black;
            this.button_SingleRun_UnloaderToCart.Location = new System.Drawing.Point(7, 42);
            this.button_SingleRun_UnloaderToCart.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_SingleRun_UnloaderToCart.Name = "button_SingleRun_UnloaderToCart";
            this.button_SingleRun_UnloaderToCart.Size = new System.Drawing.Size(124, 53);
            this.button_SingleRun_UnloaderToCart.TabIndex = 206;
            this.button_SingleRun_UnloaderToCart.Text = "To Cart";
            this.button_SingleRun_UnloaderToCart.UseVisualStyleBackColor = false;
            // 
            // tabPage_CycleRun
            // 
            this.tabPage_CycleRun.BackColor = System.Drawing.Color.Transparent;
            this.tabPage_CycleRun.Controls.Add(this.button_CycleRun_Stacker1_ModuleUnloadingReady);
            this.tabPage_CycleRun.Controls.Add(this.button_CycleRun_Stacker0_ModuleUnloadingReady);
            this.tabPage_CycleRun.Controls.Add(this.button_CycleRun_Stacker1_ModuleLoadingReady);
            this.tabPage_CycleRun.Controls.Add(this.button_CycleRun_Stacker0_ModuleLoadingReady);
            this.tabPage_CycleRun.Controls.Add(this.button_CycleRun_AllReject);
            this.tabPage_CycleRun.Controls.Add(this.button_CycleRun_LoaderReject);
            this.tabPage_CycleRun.Controls.Add(this.button_CycleRun_UnloaderReject);
            this.tabPage_CycleRun.Controls.Add(this.button_CycleRun_Align);
            this.tabPage_CycleRun.Controls.Add(this.button_CycleRun_Loading);
            this.tabPage_CycleRun.Controls.Add(this.button_CycleRun_Unloading);
            this.tabPage_CycleRun.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.tabPage_CycleRun.Location = new System.Drawing.Point(4, 36);
            this.tabPage_CycleRun.Name = "tabPage_CycleRun";
            this.tabPage_CycleRun.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_CycleRun.Size = new System.Drawing.Size(611, 306);
            this.tabPage_CycleRun.TabIndex = 0;
            this.tabPage_CycleRun.Text = "[ Cycle Run ]";
            // 
            // button_CycleRun_Stacker1_ModuleUnloadingReady
            // 
            this.button_CycleRun_Stacker1_ModuleUnloadingReady.BackColor = System.Drawing.Color.Snow;
            this.button_CycleRun_Stacker1_ModuleUnloadingReady.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_CycleRun_Stacker1_ModuleUnloadingReady.FlatAppearance.BorderSize = 2;
            this.button_CycleRun_Stacker1_ModuleUnloadingReady.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_CycleRun_Stacker1_ModuleUnloadingReady.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_CycleRun_Stacker1_ModuleUnloadingReady.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button_CycleRun_Stacker1_ModuleUnloadingReady.ForeColor = System.Drawing.Color.Black;
            this.button_CycleRun_Stacker1_ModuleUnloadingReady.Location = new System.Drawing.Point(13, 191);
            this.button_CycleRun_Stacker1_ModuleUnloadingReady.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_CycleRun_Stacker1_ModuleUnloadingReady.Name = "button_CycleRun_Stacker1_ModuleUnloadingReady";
            this.button_CycleRun_Stacker1_ModuleUnloadingReady.Size = new System.Drawing.Size(120, 103);
            this.button_CycleRun_Stacker1_ModuleUnloadingReady.TabIndex = 209;
            this.button_CycleRun_Stacker1_ModuleUnloadingReady.Text = "[ Stacker 1]\r\nModule\r\nUnloading\r\nReady";
            this.button_CycleRun_Stacker1_ModuleUnloadingReady.UseVisualStyleBackColor = false;
            // 
            // button_CycleRun_Stacker0_ModuleUnloadingReady
            // 
            this.button_CycleRun_Stacker0_ModuleUnloadingReady.BackColor = System.Drawing.Color.Snow;
            this.button_CycleRun_Stacker0_ModuleUnloadingReady.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_CycleRun_Stacker0_ModuleUnloadingReady.FlatAppearance.BorderSize = 2;
            this.button_CycleRun_Stacker0_ModuleUnloadingReady.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_CycleRun_Stacker0_ModuleUnloadingReady.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_CycleRun_Stacker0_ModuleUnloadingReady.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button_CycleRun_Stacker0_ModuleUnloadingReady.ForeColor = System.Drawing.Color.Black;
            this.button_CycleRun_Stacker0_ModuleUnloadingReady.Location = new System.Drawing.Point(136, 191);
            this.button_CycleRun_Stacker0_ModuleUnloadingReady.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_CycleRun_Stacker0_ModuleUnloadingReady.Name = "button_CycleRun_Stacker0_ModuleUnloadingReady";
            this.button_CycleRun_Stacker0_ModuleUnloadingReady.Size = new System.Drawing.Size(120, 103);
            this.button_CycleRun_Stacker0_ModuleUnloadingReady.TabIndex = 208;
            this.button_CycleRun_Stacker0_ModuleUnloadingReady.Text = "[ Stacker 0]\r\nModule\r\nUnloading\r\nReady";
            this.button_CycleRun_Stacker0_ModuleUnloadingReady.UseVisualStyleBackColor = false;
            // 
            // button_CycleRun_Stacker1_ModuleLoadingReady
            // 
            this.button_CycleRun_Stacker1_ModuleLoadingReady.BackColor = System.Drawing.Color.Snow;
            this.button_CycleRun_Stacker1_ModuleLoadingReady.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_CycleRun_Stacker1_ModuleLoadingReady.FlatAppearance.BorderSize = 2;
            this.button_CycleRun_Stacker1_ModuleLoadingReady.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_CycleRun_Stacker1_ModuleLoadingReady.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_CycleRun_Stacker1_ModuleLoadingReady.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button_CycleRun_Stacker1_ModuleLoadingReady.ForeColor = System.Drawing.Color.Black;
            this.button_CycleRun_Stacker1_ModuleLoadingReady.Location = new System.Drawing.Point(354, 191);
            this.button_CycleRun_Stacker1_ModuleLoadingReady.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_CycleRun_Stacker1_ModuleLoadingReady.Name = "button_CycleRun_Stacker1_ModuleLoadingReady";
            this.button_CycleRun_Stacker1_ModuleLoadingReady.Size = new System.Drawing.Size(120, 103);
            this.button_CycleRun_Stacker1_ModuleLoadingReady.TabIndex = 207;
            this.button_CycleRun_Stacker1_ModuleLoadingReady.Text = "[ Stacker 1]\r\nModule\r\nLoading\r\nReady";
            this.button_CycleRun_Stacker1_ModuleLoadingReady.UseVisualStyleBackColor = false;
            // 
            // button_CycleRun_Stacker0_ModuleLoadingReady
            // 
            this.button_CycleRun_Stacker0_ModuleLoadingReady.BackColor = System.Drawing.Color.Snow;
            this.button_CycleRun_Stacker0_ModuleLoadingReady.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_CycleRun_Stacker0_ModuleLoadingReady.FlatAppearance.BorderSize = 2;
            this.button_CycleRun_Stacker0_ModuleLoadingReady.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_CycleRun_Stacker0_ModuleLoadingReady.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_CycleRun_Stacker0_ModuleLoadingReady.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button_CycleRun_Stacker0_ModuleLoadingReady.ForeColor = System.Drawing.Color.Black;
            this.button_CycleRun_Stacker0_ModuleLoadingReady.Location = new System.Drawing.Point(477, 191);
            this.button_CycleRun_Stacker0_ModuleLoadingReady.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_CycleRun_Stacker0_ModuleLoadingReady.Name = "button_CycleRun_Stacker0_ModuleLoadingReady";
            this.button_CycleRun_Stacker0_ModuleLoadingReady.Size = new System.Drawing.Size(120, 103);
            this.button_CycleRun_Stacker0_ModuleLoadingReady.TabIndex = 206;
            this.button_CycleRun_Stacker0_ModuleLoadingReady.Text = "[ Stacker 0]\r\nModule\r\nLoading\r\nReady";
            this.button_CycleRun_Stacker0_ModuleLoadingReady.UseVisualStyleBackColor = false;
            // 
            // button_CycleRun_AllReject
            // 
            this.button_CycleRun_AllReject.BackColor = System.Drawing.Color.Snow;
            this.button_CycleRun_AllReject.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_CycleRun_AllReject.FlatAppearance.BorderSize = 2;
            this.button_CycleRun_AllReject.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_CycleRun_AllReject.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_CycleRun_AllReject.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button_CycleRun_AllReject.ForeColor = System.Drawing.Color.Black;
            this.button_CycleRun_AllReject.Location = new System.Drawing.Point(222, 13);
            this.button_CycleRun_AllReject.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_CycleRun_AllReject.Name = "button_CycleRun_AllReject";
            this.button_CycleRun_AllReject.Size = new System.Drawing.Size(166, 53);
            this.button_CycleRun_AllReject.TabIndex = 205;
            this.button_CycleRun_AllReject.Text = "All Reject";
            this.button_CycleRun_AllReject.UseVisualStyleBackColor = false;
            // 
            // button_CycleRun_LoaderReject
            // 
            this.button_CycleRun_LoaderReject.BackColor = System.Drawing.Color.Snow;
            this.button_CycleRun_LoaderReject.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_CycleRun_LoaderReject.FlatAppearance.BorderSize = 2;
            this.button_CycleRun_LoaderReject.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_CycleRun_LoaderReject.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_CycleRun_LoaderReject.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button_CycleRun_LoaderReject.ForeColor = System.Drawing.Color.Black;
            this.button_CycleRun_LoaderReject.Location = new System.Drawing.Point(431, 13);
            this.button_CycleRun_LoaderReject.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_CycleRun_LoaderReject.Name = "button_CycleRun_LoaderReject";
            this.button_CycleRun_LoaderReject.Size = new System.Drawing.Size(166, 53);
            this.button_CycleRun_LoaderReject.TabIndex = 204;
            this.button_CycleRun_LoaderReject.Text = "Loader Reject";
            this.button_CycleRun_LoaderReject.UseVisualStyleBackColor = false;
            // 
            // button_CycleRun_UnloaderReject
            // 
            this.button_CycleRun_UnloaderReject.BackColor = System.Drawing.Color.Snow;
            this.button_CycleRun_UnloaderReject.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_CycleRun_UnloaderReject.FlatAppearance.BorderSize = 2;
            this.button_CycleRun_UnloaderReject.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_CycleRun_UnloaderReject.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_CycleRun_UnloaderReject.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button_CycleRun_UnloaderReject.ForeColor = System.Drawing.Color.Black;
            this.button_CycleRun_UnloaderReject.Location = new System.Drawing.Point(13, 13);
            this.button_CycleRun_UnloaderReject.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_CycleRun_UnloaderReject.Name = "button_CycleRun_UnloaderReject";
            this.button_CycleRun_UnloaderReject.Size = new System.Drawing.Size(166, 53);
            this.button_CycleRun_UnloaderReject.TabIndex = 203;
            this.button_CycleRun_UnloaderReject.Text = "Unloader Reject";
            this.button_CycleRun_UnloaderReject.UseVisualStyleBackColor = false;
            // 
            // button_CycleRun_Align
            // 
            this.button_CycleRun_Align.BackColor = System.Drawing.Color.Snow;
            this.button_CycleRun_Align.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_CycleRun_Align.FlatAppearance.BorderSize = 2;
            this.button_CycleRun_Align.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_CycleRun_Align.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_CycleRun_Align.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button_CycleRun_Align.ForeColor = System.Drawing.Color.Black;
            this.button_CycleRun_Align.Location = new System.Drawing.Point(222, 83);
            this.button_CycleRun_Align.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_CycleRun_Align.Name = "button_CycleRun_Align";
            this.button_CycleRun_Align.Size = new System.Drawing.Size(166, 53);
            this.button_CycleRun_Align.TabIndex = 202;
            this.button_CycleRun_Align.Text = "Module Align";
            this.button_CycleRun_Align.UseVisualStyleBackColor = false;
            // 
            // button_CycleRun_Loading
            // 
            this.button_CycleRun_Loading.BackColor = System.Drawing.Color.Snow;
            this.button_CycleRun_Loading.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_CycleRun_Loading.FlatAppearance.BorderSize = 2;
            this.button_CycleRun_Loading.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_CycleRun_Loading.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_CycleRun_Loading.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button_CycleRun_Loading.ForeColor = System.Drawing.Color.Black;
            this.button_CycleRun_Loading.Location = new System.Drawing.Point(431, 69);
            this.button_CycleRun_Loading.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_CycleRun_Loading.Name = "button_CycleRun_Loading";
            this.button_CycleRun_Loading.Size = new System.Drawing.Size(166, 53);
            this.button_CycleRun_Loading.TabIndex = 201;
            this.button_CycleRun_Loading.Text = "Module Loading";
            this.button_CycleRun_Loading.UseVisualStyleBackColor = false;
            // 
            // button_CycleRun_Unloading
            // 
            this.button_CycleRun_Unloading.BackColor = System.Drawing.Color.Snow;
            this.button_CycleRun_Unloading.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_CycleRun_Unloading.FlatAppearance.BorderSize = 2;
            this.button_CycleRun_Unloading.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_CycleRun_Unloading.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_CycleRun_Unloading.Font = new System.Drawing.Font("Tahoma", 12F);
            this.button_CycleRun_Unloading.ForeColor = System.Drawing.Color.Black;
            this.button_CycleRun_Unloading.Location = new System.Drawing.Point(13, 69);
            this.button_CycleRun_Unloading.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_CycleRun_Unloading.Name = "button_CycleRun_Unloading";
            this.button_CycleRun_Unloading.Size = new System.Drawing.Size(166, 53);
            this.button_CycleRun_Unloading.TabIndex = 200;
            this.button_CycleRun_Unloading.Text = "Module Unloading";
            this.button_CycleRun_Unloading.UseVisualStyleBackColor = false;
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
            this.m_visionImageViewer_HighRes.TabIndex = 106;
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
            this.m_visionImageViewer_LowRes.TabIndex = 105;
            this.m_visionImageViewer_LowRes.TabStop = false;
            this.m_visionImageViewer_LowRes.UpdateDelayTime = 160;
            this.m_visionImageViewer_LowRes.VisibleCrossLine = true;
            // 
            // baseLabel_LoaderCarrier
            // 
            this.baseLabel_LoaderCarrier.AutoSize = true;
            this.baseLabel_LoaderCarrier.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_LoaderCarrier.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LoaderCarrier.Location = new System.Drawing.Point(352, 16);
            this.baseLabel_LoaderCarrier.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LoaderCarrier.Name = "baseLabel_LoaderCarrier";
            this.baseLabel_LoaderCarrier.Size = new System.Drawing.Size(143, 19);
            this.baseLabel_LoaderCarrier.TabIndex = 205;
            this.baseLabel_LoaderCarrier.Text = "[ Loader Transfer ]";
            // 
            // baseLabel_UnloaderCarrier
            // 
            this.baseLabel_UnloaderCarrier.AutoSize = true;
            this.baseLabel_UnloaderCarrier.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_UnloaderCarrier.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_UnloaderCarrier.Location = new System.Drawing.Point(54, 16);
            this.baseLabel_UnloaderCarrier.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_UnloaderCarrier.Name = "baseLabel_UnloaderCarrier";
            this.baseLabel_UnloaderCarrier.Size = new System.Drawing.Size(159, 19);
            this.baseLabel_UnloaderCarrier.TabIndex = 204;
            this.baseLabel_UnloaderCarrier.Text = "[ Unloader Transfer ]";
            // 
            // baseGroupBox_Status
            // 
            this.baseGroupBox_Status.BorderColor = System.Drawing.Color.DarkGray;
            this.baseGroupBox_Status.Controls.Add(this.baseLabel_Status_LaserSystemFault);
            this.baseGroupBox_Status.Controls.Add(this.pictureBox_Status_LaserSystemFault);
            this.baseGroupBox_Status.Controls.Add(this.baseLabel_Status_DustCollector1_FanFault);
            this.baseGroupBox_Status.Controls.Add(this.pictureBox_Status_DustCollector1_Fan_Fault);
            this.baseGroupBox_Status.Controls.Add(this.baseLabel_Status_DustCollector0_FanFault);
            this.baseGroupBox_Status.Controls.Add(this.pictureBox_Status_DustCollector0_Fan_Fault);
            this.baseGroupBox_Status.Controls.Add(this.baseLabel_Status_MainCDA_Check);
            this.baseGroupBox_Status.Controls.Add(this.pictureBox_Status_MainCDACheck);
            this.baseGroupBox_Status.Controls.Add(this.baseLabel_Status_WaterLeakCheck);
            this.baseGroupBox_Status.Controls.Add(this.pictureBox_Status_WaterLeakCheck);
            this.baseGroupBox_Status.Controls.Add(this.baseLabel_Status_WaterFlowCheck);
            this.baseGroupBox_Status.Controls.Add(this.pictureBox_Status_WaterFlowCheck);
            this.baseGroupBox_Status.Controls.Add(this.baseLabel_Status_Coolant_Return);
            this.baseGroupBox_Status.Controls.Add(this.pictureBox_Status_LaserCoolant_Return);
            this.baseGroupBox_Status.Controls.Add(this.baseLabel_Status_Coolant_Supply);
            this.baseGroupBox_Status.Controls.Add(this.pictureBox_Status_LaserCoolant_Supply);
            this.baseGroupBox_Status.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_Status.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox_Status.Location = new System.Drawing.Point(1076, 233);
            this.baseGroupBox_Status.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseGroupBox_Status.Name = "baseGroupBox_Status";
            this.baseGroupBox_Status.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseGroupBox_Status.Size = new System.Drawing.Size(619, 128);
            this.baseGroupBox_Status.TabIndex = 196;
            this.baseGroupBox_Status.TabStop = false;
            this.baseGroupBox_Status.Text = " [ Machine Status ] ";
            // 
            // baseLabel_Status_LaserSystemFault
            // 
            this.baseLabel_Status_LaserSystemFault.AutoSize = true;
            this.baseLabel_Status_LaserSystemFault.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.baseLabel_Status_LaserSystemFault.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Status_LaserSystemFault.Location = new System.Drawing.Point(397, 32);
            this.baseLabel_Status_LaserSystemFault.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Status_LaserSystemFault.Name = "baseLabel_Status_LaserSystemFault";
            this.baseLabel_Status_LaserSystemFault.Size = new System.Drawing.Size(116, 16);
            this.baseLabel_Status_LaserSystemFault.TabIndex = 213;
            this.baseLabel_Status_LaserSystemFault.Text = "Laser System Fault";
            this.baseLabel_Status_LaserSystemFault.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox_Status_LaserSystemFault
            // 
            this.pictureBox_Status_LaserSystemFault.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_Status_LaserSystemFault.Image")));
            this.pictureBox_Status_LaserSystemFault.Location = new System.Drawing.Point(377, 32);
            this.pictureBox_Status_LaserSystemFault.Name = "pictureBox_Status_LaserSystemFault";
            this.pictureBox_Status_LaserSystemFault.Size = new System.Drawing.Size(17, 17);
            this.pictureBox_Status_LaserSystemFault.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Status_LaserSystemFault.TabIndex = 212;
            this.pictureBox_Status_LaserSystemFault.TabStop = false;
            // 
            // baseLabel_Status_DustCollector1_FanFault
            // 
            this.baseLabel_Status_DustCollector1_FanFault.AutoSize = true;
            this.baseLabel_Status_DustCollector1_FanFault.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.baseLabel_Status_DustCollector1_FanFault.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Status_DustCollector1_FanFault.Location = new System.Drawing.Point(397, 101);
            this.baseLabel_Status_DustCollector1_FanFault.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Status_DustCollector1_FanFault.Name = "baseLabel_Status_DustCollector1_FanFault";
            this.baseLabel_Status_DustCollector1_FanFault.Size = new System.Drawing.Size(182, 16);
            this.baseLabel_Status_DustCollector1_FanFault.TabIndex = 211;
            this.baseLabel_Status_DustCollector1_FanFault.Text = "Dust Collector Fan Fault  [2nd]";
            this.baseLabel_Status_DustCollector1_FanFault.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox_Status_DustCollector1_Fan_Fault
            // 
            this.pictureBox_Status_DustCollector1_Fan_Fault.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_Status_DustCollector1_Fan_Fault.Image")));
            this.pictureBox_Status_DustCollector1_Fan_Fault.Location = new System.Drawing.Point(377, 101);
            this.pictureBox_Status_DustCollector1_Fan_Fault.Name = "pictureBox_Status_DustCollector1_Fan_Fault";
            this.pictureBox_Status_DustCollector1_Fan_Fault.Size = new System.Drawing.Size(17, 17);
            this.pictureBox_Status_DustCollector1_Fan_Fault.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Status_DustCollector1_Fan_Fault.TabIndex = 210;
            this.pictureBox_Status_DustCollector1_Fan_Fault.TabStop = false;
            // 
            // baseLabel_Status_DustCollector0_FanFault
            // 
            this.baseLabel_Status_DustCollector0_FanFault.AutoSize = true;
            this.baseLabel_Status_DustCollector0_FanFault.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.baseLabel_Status_DustCollector0_FanFault.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Status_DustCollector0_FanFault.Location = new System.Drawing.Point(397, 78);
            this.baseLabel_Status_DustCollector0_FanFault.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Status_DustCollector0_FanFault.Name = "baseLabel_Status_DustCollector0_FanFault";
            this.baseLabel_Status_DustCollector0_FanFault.Size = new System.Drawing.Size(178, 16);
            this.baseLabel_Status_DustCollector0_FanFault.TabIndex = 209;
            this.baseLabel_Status_DustCollector0_FanFault.Text = "Dust Collector Fan Fault  [1st]";
            this.baseLabel_Status_DustCollector0_FanFault.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox_Status_DustCollector0_Fan_Fault
            // 
            this.pictureBox_Status_DustCollector0_Fan_Fault.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_Status_DustCollector0_Fan_Fault.Image")));
            this.pictureBox_Status_DustCollector0_Fan_Fault.Location = new System.Drawing.Point(377, 78);
            this.pictureBox_Status_DustCollector0_Fan_Fault.Name = "pictureBox_Status_DustCollector0_Fan_Fault";
            this.pictureBox_Status_DustCollector0_Fan_Fault.Size = new System.Drawing.Size(17, 17);
            this.pictureBox_Status_DustCollector0_Fan_Fault.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Status_DustCollector0_Fan_Fault.TabIndex = 208;
            this.pictureBox_Status_DustCollector0_Fan_Fault.TabStop = false;
            // 
            // baseLabel_Status_MainCDA_Check
            // 
            this.baseLabel_Status_MainCDA_Check.AutoSize = true;
            this.baseLabel_Status_MainCDA_Check.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.baseLabel_Status_MainCDA_Check.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Status_MainCDA_Check.Location = new System.Drawing.Point(28, 32);
            this.baseLabel_Status_MainCDA_Check.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Status_MainCDA_Check.Name = "baseLabel_Status_MainCDA_Check";
            this.baseLabel_Status_MainCDA_Check.Size = new System.Drawing.Size(100, 16);
            this.baseLabel_Status_MainCDA_Check.TabIndex = 207;
            this.baseLabel_Status_MainCDA_Check.Text = "Main CDA Check";
            this.baseLabel_Status_MainCDA_Check.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox_Status_MainCDACheck
            // 
            this.pictureBox_Status_MainCDACheck.Image = global::SLD200.Properties.Resources.DioEllipseOff;
            this.pictureBox_Status_MainCDACheck.Location = new System.Drawing.Point(8, 32);
            this.pictureBox_Status_MainCDACheck.Name = "pictureBox_Status_MainCDACheck";
            this.pictureBox_Status_MainCDACheck.Size = new System.Drawing.Size(17, 17);
            this.pictureBox_Status_MainCDACheck.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Status_MainCDACheck.TabIndex = 206;
            this.pictureBox_Status_MainCDACheck.TabStop = false;
            // 
            // baseLabel_Status_WaterLeakCheck
            // 
            this.baseLabel_Status_WaterLeakCheck.AutoSize = true;
            this.baseLabel_Status_WaterLeakCheck.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.baseLabel_Status_WaterLeakCheck.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Status_WaterLeakCheck.Location = new System.Drawing.Point(397, 55);
            this.baseLabel_Status_WaterLeakCheck.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Status_WaterLeakCheck.Name = "baseLabel_Status_WaterLeakCheck";
            this.baseLabel_Status_WaterLeakCheck.Size = new System.Drawing.Size(205, 16);
            this.baseLabel_Status_WaterLeakCheck.TabIndex = 205;
            this.baseLabel_Status_WaterLeakCheck.Text = "Water Leak  [In, Box, Laser, Mask]";
            this.baseLabel_Status_WaterLeakCheck.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox_Status_WaterLeakCheck
            // 
            this.pictureBox_Status_WaterLeakCheck.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_Status_WaterLeakCheck.Image")));
            this.pictureBox_Status_WaterLeakCheck.Location = new System.Drawing.Point(377, 55);
            this.pictureBox_Status_WaterLeakCheck.Name = "pictureBox_Status_WaterLeakCheck";
            this.pictureBox_Status_WaterLeakCheck.Size = new System.Drawing.Size(17, 17);
            this.pictureBox_Status_WaterLeakCheck.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Status_WaterLeakCheck.TabIndex = 204;
            this.pictureBox_Status_WaterLeakCheck.TabStop = false;
            // 
            // baseLabel_Status_WaterFlowCheck
            // 
            this.baseLabel_Status_WaterFlowCheck.AutoSize = true;
            this.baseLabel_Status_WaterFlowCheck.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.baseLabel_Status_WaterFlowCheck.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Status_WaterFlowCheck.Location = new System.Drawing.Point(28, 101);
            this.baseLabel_Status_WaterFlowCheck.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Status_WaterFlowCheck.Name = "baseLabel_Status_WaterFlowCheck";
            this.baseLabel_Status_WaterFlowCheck.Size = new System.Drawing.Size(240, 16);
            this.baseLabel_Status_WaterFlowCheck.TabIndex = 203;
            this.baseLabel_Status_WaterFlowCheck.Text = "Water Flow Check  [Scanner, Varioscan]";
            this.baseLabel_Status_WaterFlowCheck.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox_Status_WaterFlowCheck
            // 
            this.pictureBox_Status_WaterFlowCheck.Image = global::SLD200.Properties.Resources.DioEllipseOff;
            this.pictureBox_Status_WaterFlowCheck.Location = new System.Drawing.Point(8, 101);
            this.pictureBox_Status_WaterFlowCheck.Name = "pictureBox_Status_WaterFlowCheck";
            this.pictureBox_Status_WaterFlowCheck.Size = new System.Drawing.Size(17, 17);
            this.pictureBox_Status_WaterFlowCheck.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Status_WaterFlowCheck.TabIndex = 202;
            this.pictureBox_Status_WaterFlowCheck.TabStop = false;
            // 
            // baseLabel_Status_Coolant_Return
            // 
            this.baseLabel_Status_Coolant_Return.AutoSize = true;
            this.baseLabel_Status_Coolant_Return.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.baseLabel_Status_Coolant_Return.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Status_Coolant_Return.Location = new System.Drawing.Point(28, 78);
            this.baseLabel_Status_Coolant_Return.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Status_Coolant_Return.Name = "baseLabel_Status_Coolant_Return";
            this.baseLabel_Status_Coolant_Return.Size = new System.Drawing.Size(297, 16);
            this.baseLabel_Status_Coolant_Return.TabIndex = 201;
            this.baseLabel_Status_Coolant_Return.Text = "Coolant Return  [Laser, Mask, Scanner, Varioscan]";
            this.baseLabel_Status_Coolant_Return.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox_Status_LaserCoolant_Return
            // 
            this.pictureBox_Status_LaserCoolant_Return.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_Status_LaserCoolant_Return.Image")));
            this.pictureBox_Status_LaserCoolant_Return.Location = new System.Drawing.Point(8, 78);
            this.pictureBox_Status_LaserCoolant_Return.Name = "pictureBox_Status_LaserCoolant_Return";
            this.pictureBox_Status_LaserCoolant_Return.Size = new System.Drawing.Size(17, 17);
            this.pictureBox_Status_LaserCoolant_Return.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Status_LaserCoolant_Return.TabIndex = 200;
            this.pictureBox_Status_LaserCoolant_Return.TabStop = false;
            // 
            // baseLabel_Status_Coolant_Supply
            // 
            this.baseLabel_Status_Coolant_Supply.AutoSize = true;
            this.baseLabel_Status_Coolant_Supply.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.baseLabel_Status_Coolant_Supply.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Status_Coolant_Supply.Location = new System.Drawing.Point(28, 55);
            this.baseLabel_Status_Coolant_Supply.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Status_Coolant_Supply.Name = "baseLabel_Status_Coolant_Supply";
            this.baseLabel_Status_Coolant_Supply.Size = new System.Drawing.Size(297, 16);
            this.baseLabel_Status_Coolant_Supply.TabIndex = 199;
            this.baseLabel_Status_Coolant_Supply.Text = "Coolant Supply  [Laser, Mask, Scanner, Varioscan]";
            this.baseLabel_Status_Coolant_Supply.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox_Status_LaserCoolant_Supply
            // 
            this.pictureBox_Status_LaserCoolant_Supply.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_Status_LaserCoolant_Supply.Image")));
            this.pictureBox_Status_LaserCoolant_Supply.Location = new System.Drawing.Point(8, 55);
            this.pictureBox_Status_LaserCoolant_Supply.Name = "pictureBox_Status_LaserCoolant_Supply";
            this.pictureBox_Status_LaserCoolant_Supply.Size = new System.Drawing.Size(17, 17);
            this.pictureBox_Status_LaserCoolant_Supply.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Status_LaserCoolant_Supply.TabIndex = 198;
            this.pictureBox_Status_LaserCoolant_Supply.TabStop = false;
            // 
            // baseGroupBox_Data
            // 
            this.baseGroupBox_Data.BorderColor = System.Drawing.Color.DarkGray;
            this.baseGroupBox_Data.Controls.Add(this.SiriusViewer_Main);
            this.baseGroupBox_Data.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_Data.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox_Data.Location = new System.Drawing.Point(449, 233);
            this.baseGroupBox_Data.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseGroupBox_Data.Name = "baseGroupBox_Data";
            this.baseGroupBox_Data.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseGroupBox_Data.Size = new System.Drawing.Size(600, 528);
            this.baseGroupBox_Data.TabIndex = 195;
            this.baseGroupBox_Data.TabStop = false;
            this.baseGroupBox_Data.Text = " [ Data ] ";
            // 
            // SiriusViewer_Main
            // 
            this.SiriusViewer_Main.AliasName = "NoName";
            this.SiriusViewer_Main.BackColor = System.Drawing.SystemColors.Control;
            this.SiriusViewer_Main.Document = null;
            this.SiriusViewer_Main.FileName = "NoName";
            this.SiriusViewer_Main.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SiriusViewer_Main.Index = ((uint)(0u));
            this.SiriusViewer_Main.Location = new System.Drawing.Point(7, 32);
            this.SiriusViewer_Main.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SiriusViewer_Main.Name = "SiriusViewer_Main";
            this.SiriusViewer_Main.Progress = 0;
            this.SiriusViewer_Main.Size = new System.Drawing.Size(586, 489);
            this.SiriusViewer_Main.TabIndex = 36;
            // 
            // baseGroupBox_Module_Information
            // 
            this.baseGroupBox_Module_Information.BorderColor = System.Drawing.Color.DarkGray;
            this.baseGroupBox_Module_Information.Controls.Add(this.baseTextBox_TotalSocketCount);
            this.baseGroupBox_Module_Information.Controls.Add(this.baseTextBox_SocketCountPerModule);
            this.baseGroupBox_Module_Information.Controls.Add(this.baseLabel_SocketPerModule);
            this.baseGroupBox_Module_Information.Controls.Add(this.baseLabel_SocketCount);
            this.baseGroupBox_Module_Information.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_Module_Information.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox_Module_Information.Location = new System.Drawing.Point(872, 118);
            this.baseGroupBox_Module_Information.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseGroupBox_Module_Information.Name = "baseGroupBox_Module_Information";
            this.baseGroupBox_Module_Information.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseGroupBox_Module_Information.Size = new System.Drawing.Size(340, 99);
            this.baseGroupBox_Module_Information.TabIndex = 192;
            this.baseGroupBox_Module_Information.TabStop = false;
            this.baseGroupBox_Module_Information.Text = " [ Module Information ] ";
            // 
            // baseTextBox_TotalSocketCount
            // 
            this.baseTextBox_TotalSocketCount.BackColor = System.Drawing.Color.White;
            this.baseTextBox_TotalSocketCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseTextBox_TotalSocketCount.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseTextBox_TotalSocketCount.ForeColor = System.Drawing.Color.Black;
            this.baseTextBox_TotalSocketCount.Location = new System.Drawing.Point(202, 61);
            this.baseTextBox_TotalSocketCount.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseTextBox_TotalSocketCount.Name = "baseTextBox_TotalSocketCount";
            this.baseTextBox_TotalSocketCount.ReadOnly = true;
            this.baseTextBox_TotalSocketCount.Size = new System.Drawing.Size(127, 26);
            this.baseTextBox_TotalSocketCount.TabIndex = 121;
            this.baseTextBox_TotalSocketCount.Text = "0 (0)";
            this.baseTextBox_TotalSocketCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseTextBox_SocketCountPerModule
            // 
            this.baseTextBox_SocketCountPerModule.BackColor = System.Drawing.Color.White;
            this.baseTextBox_SocketCountPerModule.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseTextBox_SocketCountPerModule.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseTextBox_SocketCountPerModule.ForeColor = System.Drawing.Color.Black;
            this.baseTextBox_SocketCountPerModule.Location = new System.Drawing.Point(202, 31);
            this.baseTextBox_SocketCountPerModule.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseTextBox_SocketCountPerModule.Name = "baseTextBox_SocketCountPerModule";
            this.baseTextBox_SocketCountPerModule.ReadOnly = true;
            this.baseTextBox_SocketCountPerModule.Size = new System.Drawing.Size(127, 26);
            this.baseTextBox_SocketCountPerModule.TabIndex = 120;
            this.baseTextBox_SocketCountPerModule.Text = "0";
            this.baseTextBox_SocketCountPerModule.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseLabel_SocketPerModule
            // 
            this.baseLabel_SocketPerModule.AutoSize = true;
            this.baseLabel_SocketPerModule.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_SocketPerModule.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_SocketPerModule.Location = new System.Drawing.Point(10, 35);
            this.baseLabel_SocketPerModule.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_SocketPerModule.Name = "baseLabel_SocketPerModule";
            this.baseLabel_SocketPerModule.Size = new System.Drawing.Size(169, 18);
            this.baseLabel_SocketPerModule.TabIndex = 115;
            this.baseLabel_SocketPerModule.Text = "Socket count per Module";
            // 
            // baseLabel_SocketCount
            // 
            this.baseLabel_SocketCount.AutoSize = true;
            this.baseLabel_SocketCount.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_SocketCount.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_SocketCount.Location = new System.Drawing.Point(10, 66);
            this.baseLabel_SocketCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_SocketCount.Name = "baseLabel_SocketCount";
            this.baseLabel_SocketCount.Size = new System.Drawing.Size(132, 18);
            this.baseLabel_SocketCount.TabIndex = 113;
            this.baseLabel_SocketCount.Text = "Total Socket Count";
            // 
            // baseGroupBox_Progress
            // 
            this.baseGroupBox_Progress.BorderColor = System.Drawing.Color.DarkGray;
            this.baseGroupBox_Progress.Controls.Add(this.numericUpDown_Module_TargetCount);
            this.baseGroupBox_Progress.Controls.Add(this.baseLabel_ModuleCount_Target);
            this.baseGroupBox_Progress.Controls.Add(this.button_PNLCount_Clear);
            this.baseGroupBox_Progress.Controls.Add(this.baseLabel_PNLCount_NG);
            this.baseGroupBox_Progress.Controls.Add(this.baseTextBox1);
            this.baseGroupBox_Progress.Controls.Add(this.baseTextBox_Module_TotalCount);
            this.baseGroupBox_Progress.Controls.Add(this.baseLabel_ModuleCount);
            this.baseGroupBox_Progress.Controls.Add(this.baseLabel_PNLCount_Total);
            this.baseGroupBox_Progress.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_Progress.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox_Progress.Location = new System.Drawing.Point(872, 4);
            this.baseGroupBox_Progress.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseGroupBox_Progress.Name = "baseGroupBox_Progress";
            this.baseGroupBox_Progress.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseGroupBox_Progress.Size = new System.Drawing.Size(426, 87);
            this.baseGroupBox_Progress.TabIndex = 191;
            this.baseGroupBox_Progress.TabStop = false;
            this.baseGroupBox_Progress.Text = " [ Progress ] ";
            // 
            // numericUpDown_Module_TargetCount
            // 
            this.numericUpDown_Module_TargetCount.BackColor = System.Drawing.Color.White;
            this.numericUpDown_Module_TargetCount.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDown_Module_TargetCount.Location = new System.Drawing.Point(75, 50);
            this.numericUpDown_Module_TargetCount.Name = "numericUpDown_Module_TargetCount";
            this.numericUpDown_Module_TargetCount.Size = new System.Drawing.Size(98, 26);
            this.numericUpDown_Module_TargetCount.TabIndex = 133;
            this.numericUpDown_Module_TargetCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseLabel_ModuleCount_Target
            // 
            this.baseLabel_ModuleCount_Target.AutoSize = true;
            this.baseLabel_ModuleCount_Target.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.baseLabel_ModuleCount_Target.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_ModuleCount_Target.Location = new System.Drawing.Point(101, 30);
            this.baseLabel_ModuleCount_Target.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_ModuleCount_Target.Name = "baseLabel_ModuleCount_Target";
            this.baseLabel_ModuleCount_Target.Size = new System.Drawing.Size(45, 16);
            this.baseLabel_ModuleCount_Target.TabIndex = 131;
            this.baseLabel_ModuleCount_Target.Text = "Target";
            // 
            // button_PNLCount_Clear
            // 
            this.button_PNLCount_Clear.BackColor = System.Drawing.Color.White;
            this.button_PNLCount_Clear.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_PNLCount_Clear.FlatAppearance.BorderSize = 2;
            this.button_PNLCount_Clear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_PNLCount_Clear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_PNLCount_Clear.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.button_PNLCount_Clear.ForeColor = System.Drawing.Color.Black;
            this.button_PNLCount_Clear.Location = new System.Drawing.Point(359, 49);
            this.button_PNLCount_Clear.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_PNLCount_Clear.Name = "button_PNLCount_Clear";
            this.button_PNLCount_Clear.Size = new System.Drawing.Size(58, 29);
            this.button_PNLCount_Clear.TabIndex = 130;
            this.button_PNLCount_Clear.Text = "Clear";
            this.button_PNLCount_Clear.UseVisualStyleBackColor = false;
            // 
            // baseLabel_PNLCount_NG
            // 
            this.baseLabel_PNLCount_NG.AutoSize = true;
            this.baseLabel_PNLCount_NG.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.baseLabel_PNLCount_NG.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_PNLCount_NG.Location = new System.Drawing.Point(302, 30);
            this.baseLabel_PNLCount_NG.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_PNLCount_NG.Name = "baseLabel_PNLCount_NG";
            this.baseLabel_PNLCount_NG.Size = new System.Drawing.Size(23, 16);
            this.baseLabel_PNLCount_NG.TabIndex = 121;
            this.baseLabel_PNLCount_NG.Text = "NG";
            // 
            // baseTextBox1
            // 
            this.baseTextBox1.BackColor = System.Drawing.Color.White;
            this.baseTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseTextBox1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseTextBox1.ForeColor = System.Drawing.Color.Black;
            this.baseTextBox1.Location = new System.Drawing.Point(277, 50);
            this.baseTextBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseTextBox1.Name = "baseTextBox1";
            this.baseTextBox1.ReadOnly = true;
            this.baseTextBox1.Size = new System.Drawing.Size(77, 26);
            this.baseTextBox1.TabIndex = 120;
            this.baseTextBox1.Text = "0";
            this.baseTextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseTextBox_Module_TotalCount
            // 
            this.baseTextBox_Module_TotalCount.BackColor = System.Drawing.Color.White;
            this.baseTextBox_Module_TotalCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseTextBox_Module_TotalCount.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseTextBox_Module_TotalCount.ForeColor = System.Drawing.Color.Black;
            this.baseTextBox_Module_TotalCount.Location = new System.Drawing.Point(196, 50);
            this.baseTextBox_Module_TotalCount.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseTextBox_Module_TotalCount.Name = "baseTextBox_Module_TotalCount";
            this.baseTextBox_Module_TotalCount.ReadOnly = true;
            this.baseTextBox_Module_TotalCount.Size = new System.Drawing.Size(77, 26);
            this.baseTextBox_Module_TotalCount.TabIndex = 119;
            this.baseTextBox_Module_TotalCount.Text = "0";
            this.baseTextBox_Module_TotalCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseLabel_ModuleCount
            // 
            this.baseLabel_ModuleCount.AutoSize = true;
            this.baseLabel_ModuleCount.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_ModuleCount.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_ModuleCount.Location = new System.Drawing.Point(10, 56);
            this.baseLabel_ModuleCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_ModuleCount.Name = "baseLabel_ModuleCount";
            this.baseLabel_ModuleCount.Size = new System.Drawing.Size(54, 18);
            this.baseLabel_ModuleCount.TabIndex = 118;
            this.baseLabel_ModuleCount.Text = "Module";
            this.baseLabel_ModuleCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel_PNLCount_Total
            // 
            this.baseLabel_PNLCount_Total.AutoSize = true;
            this.baseLabel_PNLCount_Total.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.baseLabel_PNLCount_Total.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_PNLCount_Total.Location = new System.Drawing.Point(216, 30);
            this.baseLabel_PNLCount_Total.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_PNLCount_Total.Name = "baseLabel_PNLCount_Total";
            this.baseLabel_PNLCount_Total.Size = new System.Drawing.Size(36, 16);
            this.baseLabel_PNLCount_Total.TabIndex = 117;
            this.baseLabel_PNLCount_Total.Text = "Total";
            // 
            // baseGroupBox_WorkingTime
            // 
            this.baseGroupBox_WorkingTime.BorderColor = System.Drawing.Color.DarkGray;
            this.baseGroupBox_WorkingTime.Controls.Add(this.button_AverageOneCycleTime_Clear);
            this.baseGroupBox_WorkingTime.Controls.Add(this.baseLabel_Average_OneCycleTime);
            this.baseGroupBox_WorkingTime.Controls.Add(this.baseLabel_AverageOneCycle_Time);
            this.baseGroupBox_WorkingTime.Controls.Add(this.progressBar_TotalRemained_Time);
            this.baseGroupBox_WorkingTime.Controls.Add(this.baseLabel_Total_RemainedTime);
            this.baseGroupBox_WorkingTime.Controls.Add(this.baseLabel_TotalRunning_Time);
            this.baseGroupBox_WorkingTime.Controls.Add(this.progressBar_OneCycle_Time);
            this.baseGroupBox_WorkingTime.Controls.Add(this.baseLabel_CurrentOneCycle_TotalTime);
            this.baseGroupBox_WorkingTime.Controls.Add(this.baseLabel_CurrentOneCycle_ElapsedTime);
            this.baseGroupBox_WorkingTime.Controls.Add(this.baseLabel_CurrentOneCycle_Time);
            this.baseGroupBox_WorkingTime.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_WorkingTime.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox_WorkingTime.Location = new System.Drawing.Point(449, 4);
            this.baseGroupBox_WorkingTime.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseGroupBox_WorkingTime.Name = "baseGroupBox_WorkingTime";
            this.baseGroupBox_WorkingTime.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseGroupBox_WorkingTime.Size = new System.Drawing.Size(395, 213);
            this.baseGroupBox_WorkingTime.TabIndex = 190;
            this.baseGroupBox_WorkingTime.TabStop = false;
            this.baseGroupBox_WorkingTime.Text = " [ Working Time ] ";
            // 
            // button_AverageOneCycleTime_Clear
            // 
            this.button_AverageOneCycleTime_Clear.BackColor = System.Drawing.Color.White;
            this.button_AverageOneCycleTime_Clear.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_AverageOneCycleTime_Clear.FlatAppearance.BorderSize = 2;
            this.button_AverageOneCycleTime_Clear.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_AverageOneCycleTime_Clear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_AverageOneCycleTime_Clear.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_AverageOneCycleTime_Clear.ForeColor = System.Drawing.Color.Black;
            this.button_AverageOneCycleTime_Clear.Location = new System.Drawing.Point(318, 172);
            this.button_AverageOneCycleTime_Clear.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_AverageOneCycleTime_Clear.Name = "button_AverageOneCycleTime_Clear";
            this.button_AverageOneCycleTime_Clear.Size = new System.Drawing.Size(67, 33);
            this.button_AverageOneCycleTime_Clear.TabIndex = 129;
            this.button_AverageOneCycleTime_Clear.Text = "Clear";
            this.button_AverageOneCycleTime_Clear.UseVisualStyleBackColor = false;
            // 
            // baseLabel_Average_OneCycleTime
            // 
            this.baseLabel_Average_OneCycleTime.AutoSize = true;
            this.baseLabel_Average_OneCycleTime.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_Average_OneCycleTime.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Average_OneCycleTime.Location = new System.Drawing.Point(216, 178);
            this.baseLabel_Average_OneCycleTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Average_OneCycleTime.Name = "baseLabel_Average_OneCycleTime";
            this.baseLabel_Average_OneCycleTime.Size = new System.Drawing.Size(66, 18);
            this.baseLabel_Average_OneCycleTime.TabIndex = 117;
            this.baseLabel_Average_OneCycleTime.Text = "00:00:00";
            // 
            // baseLabel_AverageOneCycle_Time
            // 
            this.baseLabel_AverageOneCycle_Time.AutoSize = true;
            this.baseLabel_AverageOneCycle_Time.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_AverageOneCycle_Time.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_AverageOneCycle_Time.Location = new System.Drawing.Point(8, 178);
            this.baseLabel_AverageOneCycle_Time.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_AverageOneCycle_Time.Name = "baseLabel_AverageOneCycle_Time";
            this.baseLabel_AverageOneCycle_Time.Size = new System.Drawing.Size(171, 18);
            this.baseLabel_AverageOneCycle_Time.TabIndex = 116;
            this.baseLabel_AverageOneCycle_Time.Text = "Average One Cycle Time";
            // 
            // progressBar_TotalRemained_Time
            // 
            this.progressBar_TotalRemained_Time.Location = new System.Drawing.Point(9, 129);
            this.progressBar_TotalRemained_Time.Name = "progressBar_TotalRemained_Time";
            this.progressBar_TotalRemained_Time.Size = new System.Drawing.Size(376, 36);
            this.progressBar_TotalRemained_Time.TabIndex = 115;
            this.progressBar_TotalRemained_Time.Value = 50;
            // 
            // baseLabel_Total_RemainedTime
            // 
            this.baseLabel_Total_RemainedTime.AutoSize = true;
            this.baseLabel_Total_RemainedTime.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_Total_RemainedTime.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Total_RemainedTime.Location = new System.Drawing.Point(216, 105);
            this.baseLabel_Total_RemainedTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Total_RemainedTime.Name = "baseLabel_Total_RemainedTime";
            this.baseLabel_Total_RemainedTime.Size = new System.Drawing.Size(66, 18);
            this.baseLabel_Total_RemainedTime.TabIndex = 114;
            this.baseLabel_Total_RemainedTime.Text = "00:00:00";
            // 
            // baseLabel_TotalRunning_Time
            // 
            this.baseLabel_TotalRunning_Time.AutoSize = true;
            this.baseLabel_TotalRunning_Time.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_TotalRunning_Time.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_TotalRunning_Time.Location = new System.Drawing.Point(8, 105);
            this.baseLabel_TotalRunning_Time.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_TotalRunning_Time.Name = "baseLabel_TotalRunning_Time";
            this.baseLabel_TotalRunning_Time.Size = new System.Drawing.Size(135, 18);
            this.baseLabel_TotalRunning_Time.TabIndex = 113;
            this.baseLabel_TotalRunning_Time.Text = "Total Running Time";
            // 
            // progressBar_OneCycle_Time
            // 
            this.progressBar_OneCycle_Time.Location = new System.Drawing.Point(9, 56);
            this.progressBar_OneCycle_Time.Name = "progressBar_OneCycle_Time";
            this.progressBar_OneCycle_Time.Size = new System.Drawing.Size(376, 36);
            this.progressBar_OneCycle_Time.TabIndex = 112;
            this.progressBar_OneCycle_Time.Value = 50;
            // 
            // baseLabel_CurrentOneCycle_TotalTime
            // 
            this.baseLabel_CurrentOneCycle_TotalTime.AutoSize = true;
            this.baseLabel_CurrentOneCycle_TotalTime.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_CurrentOneCycle_TotalTime.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_CurrentOneCycle_TotalTime.Location = new System.Drawing.Point(314, 32);
            this.baseLabel_CurrentOneCycle_TotalTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_CurrentOneCycle_TotalTime.Name = "baseLabel_CurrentOneCycle_TotalTime";
            this.baseLabel_CurrentOneCycle_TotalTime.Size = new System.Drawing.Size(66, 18);
            this.baseLabel_CurrentOneCycle_TotalTime.TabIndex = 111;
            this.baseLabel_CurrentOneCycle_TotalTime.Text = "00:00:00";
            // 
            // baseLabel_CurrentOneCycle_ElapsedTime
            // 
            this.baseLabel_CurrentOneCycle_ElapsedTime.AutoSize = true;
            this.baseLabel_CurrentOneCycle_ElapsedTime.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_CurrentOneCycle_ElapsedTime.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_CurrentOneCycle_ElapsedTime.Location = new System.Drawing.Point(216, 32);
            this.baseLabel_CurrentOneCycle_ElapsedTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_CurrentOneCycle_ElapsedTime.Name = "baseLabel_CurrentOneCycle_ElapsedTime";
            this.baseLabel_CurrentOneCycle_ElapsedTime.Size = new System.Drawing.Size(66, 18);
            this.baseLabel_CurrentOneCycle_ElapsedTime.TabIndex = 110;
            this.baseLabel_CurrentOneCycle_ElapsedTime.Text = "00:00:00";
            // 
            // baseLabel_CurrentOneCycle_Time
            // 
            this.baseLabel_CurrentOneCycle_Time.AutoSize = true;
            this.baseLabel_CurrentOneCycle_Time.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabel_CurrentOneCycle_Time.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_CurrentOneCycle_Time.Location = new System.Drawing.Point(8, 32);
            this.baseLabel_CurrentOneCycle_Time.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_CurrentOneCycle_Time.Name = "baseLabel_CurrentOneCycle_Time";
            this.baseLabel_CurrentOneCycle_Time.Size = new System.Drawing.Size(112, 18);
            this.baseLabel_CurrentOneCycle_Time.TabIndex = 109;
            this.baseLabel_CurrentOneCycle_Time.Text = "One Cycle Time";
            // 
            // baseGroupBox_Recipe
            // 
            this.baseGroupBox_Recipe.BorderColor = System.Drawing.Color.DarkGray;
            this.baseGroupBox_Recipe.Controls.Add(this.baseButtonChangeRecipe);
            this.baseGroupBox_Recipe.Controls.Add(this.baseTextBoxCurrentRecipe);
            this.baseGroupBox_Recipe.Controls.Add(this.baseLabelCurrentRecipe);
            this.baseGroupBox_Recipe.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseGroupBox_Recipe.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBox_Recipe.Location = new System.Drawing.Point(1301, 133);
            this.baseGroupBox_Recipe.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseGroupBox_Recipe.Name = "baseGroupBox_Recipe";
            this.baseGroupBox_Recipe.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseGroupBox_Recipe.Size = new System.Drawing.Size(604, 84);
            this.baseGroupBox_Recipe.TabIndex = 65;
            this.baseGroupBox_Recipe.TabStop = false;
            this.baseGroupBox_Recipe.Text = " [ Recipe ] ";
            // 
            // baseButtonChangeRecipe
            // 
            this.baseButtonChangeRecipe.BackColor = System.Drawing.Color.White;
            this.baseButtonChangeRecipe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonChangeRecipe.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseButtonChangeRecipe.ForeColor = System.Drawing.Color.Black;
            this.baseButtonChangeRecipe.Location = new System.Drawing.Point(519, 32);
            this.baseButtonChangeRecipe.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseButtonChangeRecipe.Name = "baseButtonChangeRecipe";
            this.baseButtonChangeRecipe.Size = new System.Drawing.Size(77, 40);
            this.baseButtonChangeRecipe.TabIndex = 1;
            this.baseButtonChangeRecipe.Text = "Select";
            this.baseButtonChangeRecipe.UseVisualStyleBackColor = false;
            this.baseButtonChangeRecipe.Click += new System.EventHandler(this.baseButtonChangeRecipe_Click);
            // 
            // baseTextBoxCurrentRecipe
            // 
            this.baseTextBoxCurrentRecipe.BackColor = System.Drawing.Color.Gainsboro;
            this.baseTextBoxCurrentRecipe.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseTextBoxCurrentRecipe.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseTextBoxCurrentRecipe.ForeColor = System.Drawing.Color.Black;
            this.baseTextBoxCurrentRecipe.Location = new System.Drawing.Point(73, 32);
            this.baseTextBoxCurrentRecipe.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.baseTextBoxCurrentRecipe.Name = "baseTextBoxCurrentRecipe";
            this.baseTextBoxCurrentRecipe.ReadOnly = true;
            this.baseTextBoxCurrentRecipe.Size = new System.Drawing.Size(440, 40);
            this.baseTextBoxCurrentRecipe.TabIndex = 2;
            this.baseTextBoxCurrentRecipe.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // baseLabelCurrentRecipe
            // 
            this.baseLabelCurrentRecipe.AutoSize = true;
            this.baseLabelCurrentRecipe.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baseLabelCurrentRecipe.ForeColor = System.Drawing.Color.Black;
            this.baseLabelCurrentRecipe.Location = new System.Drawing.Point(10, 41);
            this.baseLabelCurrentRecipe.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabelCurrentRecipe.Name = "baseLabelCurrentRecipe";
            this.baseLabelCurrentRecipe.Size = new System.Drawing.Size(57, 18);
            this.baseLabelCurrentRecipe.TabIndex = 1;
            this.baseLabelCurrentRecipe.Text = "Name :";
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
            this.baseLabel_HighRes_Camera.TabIndex = 115;
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
            this.baseLabel_LowRes_Camera.TabIndex = 114;
            this.baseLabel_LowRes_Camera.Text = "[  Coarse  Vision  ]";
            this.baseLabel_LowRes_Camera.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblStatus_ScannerPowerMeter_Connected
            // 
            this.lblStatus_ScannerPowerMeter_Connected.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
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
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.Controls.Add(this.tabControl_Move);
            this.Controls.Add(this.baseGroupBox_Status);
            this.Controls.Add(this.baseGroupBox_Data);
            this.Controls.Add(this.button_WorkStop);
            this.Controls.Add(this.button_OneCycleStop);
            this.Controls.Add(this.button_Pause);
            this.Controls.Add(this.button_WorkStart);
            this.Controls.Add(this.baseGroupBox_Module_Information);
            this.Controls.Add(this.baseGroupBox_Progress);
            this.Controls.Add(this.baseGroupBox_WorkingTime);
            this.Controls.Add(this.baseGroupBox_Recipe);
            this.Controls.Add(this.btnUpperCamera_StartLive);
            this.Controls.Add(this.btnUpperCamera_Init);
            this.Controls.Add(this.btnHomeAll);
            this.Controls.Add(this.baseLabel_HighRes_Camera);
            this.Controls.Add(this.baseLabel_LowRes_Camera);
            this.Controls.Add(this.m_visionImageViewer_HighRes);
            this.Controls.Add(this.m_visionImageViewer_LowRes);
            this.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.Name = "Monitoring_CWA150SA";
            this.Size = new System.Drawing.Size(1910, 770);
            this.tabControl_Move.ResumeLayout(false);
            this.tabPage_SingleMove.ResumeLayout(false);
            this.tabPage_SingleMove.PerformLayout();
            this.tabPage_CycleRun.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_visionImageViewer_HighRes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_visionImageViewer_LowRes)).EndInit();
            this.baseGroupBox_Status.ResumeLayout(false);
            this.baseGroupBox_Status.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Status_LaserSystemFault)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Status_DustCollector1_Fan_Fault)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Status_DustCollector0_Fan_Fault)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Status_MainCDACheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Status_WaterLeakCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Status_WaterFlowCheck)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Status_LaserCoolant_Return)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Status_LaserCoolant_Supply)).EndInit();
            this.baseGroupBox_Data.ResumeLayout(false);
            this.baseGroupBox_Module_Information.ResumeLayout(false);
            this.baseGroupBox_Module_Information.PerformLayout();
            this.baseGroupBox_Progress.ResumeLayout(false);
            this.baseGroupBox_Progress.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Module_TargetCount)).EndInit();
            this.baseGroupBox_WorkingTime.ResumeLayout(false);
            this.baseGroupBox_WorkingTime.PerformLayout();
            this.baseGroupBox_Recipe.ResumeLayout(false);
            this.baseGroupBox_Recipe.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timer_DIO_Status;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private BaseLabel lblStatus_ScannerPowerMeter_Connected;
        private BaseButton baseButtonChangeRecipe;
        private BaseTextBox baseTextBoxCurrentRecipe;
        private BaseLabel baseLabelCurrentRecipe;
        private QMC.Common.Hmi.VisionImageViewer m_visionImageViewer_LowRes;
        private QMC.Common.Hmi.VisionImageViewer m_visionImageViewer_HighRes;
        private BaseLabel baseLabel_LowRes_Camera;
        private BaseLabel baseLabel_HighRes_Camera;
        private System.Windows.Forms.Button btnRunStatus_Drilling2;
        private System.Windows.Forms.Button btnRunStatus_Drilling;
        private System.Windows.Forms.Button btnHomeAll;
        private System.Windows.Forms.Button btnUpperCamera_Init;
        private System.Windows.Forms.Button btnUpperCamera_StartLive;
        private WATGroupBox baseGroupBox_WorkingTime;        
        private BaseLabel baseLabel_CurrentOneCycle_Time;
        private BaseLabel baseLabel_CurrentOneCycle_ElapsedTime;
        private System.Windows.Forms.ProgressBar progressBar_OneCycle_Time;
        private BaseLabel baseLabel_CurrentOneCycle_TotalTime;
        private System.Windows.Forms.ProgressBar progressBar_TotalRemained_Time;
        private BaseLabel baseLabel_Total_RemainedTime;
        private BaseLabel baseLabel_TotalRunning_Time;
        private BaseLabel baseLabel_Average_OneCycleTime;
        private BaseLabel baseLabel_AverageOneCycle_Time;
        private System.Windows.Forms.Button button_AverageOneCycleTime_Clear;
        private BaseLabel baseLabel_ModuleCount;
        private BaseLabel baseLabel_PNLCount_Total;
        private BaseTextBox baseTextBox1;
        private BaseTextBox baseTextBox_Module_TotalCount;
        private BaseLabel baseLabel_PNLCount_NG;
        private System.Windows.Forms.Button button_PNLCount_Clear;
        private BaseLabel baseLabel_SocketCount;
        private System.Windows.Forms.Button button_WorkStart;
        private System.Windows.Forms.Button button_Pause;
        private System.Windows.Forms.Button button_OneCycleStop;
        private System.Windows.Forms.Button button_WorkStop;
        private WATGroupBox baseGroupBox_Progress;
        private WATGroupBox baseGroupBox_Module_Information;
        private WATGroupBox baseGroupBox_Recipe;
        private WATGroupBox baseGroupBox_Data;
        private WATGroupBox baseGroupBox_Status;
        private BaseLabel baseLabel_ModuleCount_Target;
        private NumericUpDown numericUpDown_Module_TargetCount;
        private TabControl tabControl_Move;
        private TabPage tabPage_SingleMove;
        private Button button_SingleRun_UnloaderToLoadingPos;
        private Button button_SingleRun_UnloaderToUnloadingPos;
        private Button button_SingleRun_LoaderToCart;
        private Button button_SingleRun_LoaderToTable;
        private Button button_SingleRun_UnloaderToTable;
        private Button button_SingleRun_UnloaderToCart;
        private BaseLabel baseLabel_LoaderCarrier;
        private BaseLabel baseLabel_UnloaderCarrier;
        private TabPage tabPage_CycleRun;
        private Button button_CycleRun_AllReject;
        private Button button_CycleRun_LoaderReject;
        private Button button_CycleRun_UnloaderReject;
        private Button button_CycleRun_Align;
        private Button button_CycleRun_Loading;
        private Button button_CycleRun_Unloading;
        private BaseLabel baseLabel_Status_Coolant_Supply;
        private PictureBox pictureBox_Status_LaserCoolant_Supply;
        private BaseLabel baseLabel_Status_Coolant_Return;
        private PictureBox pictureBox_Status_LaserCoolant_Return;
        private BaseLabel baseLabel_Status_MainCDA_Check;
        private PictureBox pictureBox_Status_MainCDACheck;
        private BaseLabel baseLabel_Status_WaterLeakCheck;
        private PictureBox pictureBox_Status_WaterLeakCheck;
        private BaseLabel baseLabel_Status_WaterFlowCheck;
        private PictureBox pictureBox_Status_WaterFlowCheck;
        private BaseLabel baseLabel_Status_DustCollector1_FanFault;
        private PictureBox pictureBox_Status_DustCollector1_Fan_Fault;
        private BaseLabel baseLabel_Status_DustCollector0_FanFault;
        private PictureBox pictureBox_Status_DustCollector0_Fan_Fault;
        private BaseLabel baseLabel_Status_LaserSystemFault;
        private PictureBox pictureBox_Status_LaserSystemFault;
        private BaseTextBox baseTextBox_SocketCountPerModule;
        private BaseLabel baseLabel_SocketPerModule;
        private BaseTextBox baseTextBox_TotalSocketCount;
        public SpiralLab.Sirius.SiriusViewerForm SiriusViewer_Main;
        private Button button_CycleRun_Stacker0_ModuleLoadingReady;
        private Button button_CycleRun_Stacker1_ModuleUnloadingReady;
        private Button button_CycleRun_Stacker0_ModuleUnloadingReady;
        private Button button_CycleRun_Stacker1_ModuleLoadingReady;
    }
}
