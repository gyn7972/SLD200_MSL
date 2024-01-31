namespace CWA150SA_Onsemi300
{
    partial class Dispenser
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
            this.buttonStop = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBoxDispenser = new System.Windows.Forms.GroupBox();
            this.groupBoxDispenserOutput = new System.Windows.Forms.GroupBox();
            this.pictureBoxDispenserIO_OUT_CHS = new System.Windows.Forms.PictureBox();
            this.pictureBoxDispenserIO_OUT_TDM_STEP = new System.Windows.Forms.PictureBox();
            this.pictureBoxDispenserIO_OUT_TMS_PRESET = new System.Windows.Forms.PictureBox();
            this.pictureBoxDispenserIO_OUT_DIS = new System.Windows.Forms.PictureBox();
            this.labelDispenserIO_OUT_CHS = new System.Windows.Forms.Label();
            this.labelDispenserIO_OUT_TDM_STEP = new System.Windows.Forms.Label();
            this.labelDispenserIO_OUT_TMS_PRESET = new System.Windows.Forms.Label();
            this.labelDispenserIO_OUT_DIS = new System.Windows.Forms.Label();
            this.groupBoxDispenserInput = new System.Windows.Forms.GroupBox();
            this.pictureBoxDispenserIO_IN_DVO = new System.Windows.Forms.PictureBox();
            this.pictureBoxDispenserIO_IN_PSE = new System.Windows.Forms.PictureBox();
            this.pictureBoxDispenserIO_IN_RSM = new System.Windows.Forms.PictureBox();
            this.pictureBoxDispenserIO_IN_DSO = new System.Windows.Forms.PictureBox();
            this.pictureBoxDispenserIO_IN_READY_DVO = new System.Windows.Forms.PictureBox();
            this.pictureBoxDispenserIO_IN_PON = new System.Windows.Forms.PictureBox();
            this.pictureBoxDispenserIO_IN_EXE = new System.Windows.Forms.PictureBox();
            this.labelDispenserIO_IN_EXE = new System.Windows.Forms.Label();
            this.pictureBoxDispenserIO_IN_DSO_END = new System.Windows.Forms.PictureBox();
            this.labelDispenserIO_IN_DSO_END = new System.Windows.Forms.Label();
            this.labelDispenserIO_IN_DVO = new System.Windows.Forms.Label();
            this.labelDispenserIO_IN_PSE = new System.Windows.Forms.Label();
            this.labelDispenserIO_IN_RSM = new System.Windows.Forms.Label();
            this.labelDispenserIO_IN_DSO = new System.Windows.Forms.Label();
            this.labelDispenserIO_IN_READY_DVO = new System.Windows.Forms.Label();
            this.labelDispenserIO_IN_PON = new System.Windows.Forms.Label();
            this.btnDispenserOn = new System.Windows.Forms.Button();
            this.groupBoxDispenser.SuspendLayout();
            this.groupBoxDispenserOutput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDispenserIO_OUT_CHS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDispenserIO_OUT_TDM_STEP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDispenserIO_OUT_TMS_PRESET)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDispenserIO_OUT_DIS)).BeginInit();
            this.groupBoxDispenserInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDispenserIO_IN_DVO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDispenserIO_IN_PSE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDispenserIO_IN_RSM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDispenserIO_IN_DSO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDispenserIO_IN_READY_DVO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDispenserIO_IN_PON)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDispenserIO_IN_EXE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDispenserIO_IN_DSO_END)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonStop
            // 
            this.buttonStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.buttonStop.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonStop.ForeColor = System.Drawing.Color.White;
            this.buttonStop.Location = new System.Drawing.Point(73, 34);
            this.buttonStop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(77, 38);
            this.buttonStop.TabIndex = 8;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = false;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(50, 95);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(127, 21);
            this.textBox1.TabIndex = 9;
            // 
            // groupBoxDispenser
            // 
            this.groupBoxDispenser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.groupBoxDispenser.Controls.Add(this.groupBoxDispenserOutput);
            this.groupBoxDispenser.Controls.Add(this.groupBoxDispenserInput);
            this.groupBoxDispenser.Controls.Add(this.btnDispenserOn);
            this.groupBoxDispenser.ForeColor = System.Drawing.Color.White;
            this.groupBoxDispenser.Location = new System.Drawing.Point(3, 3);
            this.groupBoxDispenser.Name = "groupBoxDispenser";
            this.groupBoxDispenser.Size = new System.Drawing.Size(338, 297);
            this.groupBoxDispenser.TabIndex = 10;
            this.groupBoxDispenser.TabStop = false;
            this.groupBoxDispenser.Text = " Dispenser ";
            // 
            // groupBoxDispenserOutput
            // 
            this.groupBoxDispenserOutput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.groupBoxDispenserOutput.Controls.Add(this.pictureBoxDispenserIO_OUT_CHS);
            this.groupBoxDispenserOutput.Controls.Add(this.pictureBoxDispenserIO_OUT_TDM_STEP);
            this.groupBoxDispenserOutput.Controls.Add(this.pictureBoxDispenserIO_OUT_TMS_PRESET);
            this.groupBoxDispenserOutput.Controls.Add(this.pictureBoxDispenserIO_OUT_DIS);
            this.groupBoxDispenserOutput.Controls.Add(this.labelDispenserIO_OUT_CHS);
            this.groupBoxDispenserOutput.Controls.Add(this.labelDispenserIO_OUT_TDM_STEP);
            this.groupBoxDispenserOutput.Controls.Add(this.labelDispenserIO_OUT_TMS_PRESET);
            this.groupBoxDispenserOutput.Controls.Add(this.labelDispenserIO_OUT_DIS);
            this.groupBoxDispenserOutput.ForeColor = System.Drawing.Color.White;
            this.groupBoxDispenserOutput.Location = new System.Drawing.Point(174, 28);
            this.groupBoxDispenserOutput.Name = "groupBoxDispenserOutput";
            this.groupBoxDispenserOutput.Size = new System.Drawing.Size(150, 142);
            this.groupBoxDispenserOutput.TabIndex = 13;
            this.groupBoxDispenserOutput.TabStop = false;
            this.groupBoxDispenserOutput.Text = " Output ";
            // 
            // pictureBoxDispenserIO_OUT_CHS
            // 
            this.pictureBoxDispenserIO_OUT_CHS.Image = global::CWA150SA_Onsemi.Properties.Resources.DioRectangleOff;
            this.pictureBoxDispenserIO_OUT_CHS.Location = new System.Drawing.Point(11, 107);
            this.pictureBoxDispenserIO_OUT_CHS.Name = "pictureBoxDispenserIO_OUT_CHS";
            this.pictureBoxDispenserIO_OUT_CHS.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxDispenserIO_OUT_CHS.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxDispenserIO_OUT_CHS.TabIndex = 21;
            this.pictureBoxDispenserIO_OUT_CHS.TabStop = false;
            // 
            // pictureBoxDispenserIO_OUT_TDM_STEP
            // 
            this.pictureBoxDispenserIO_OUT_TDM_STEP.Image = global::CWA150SA_Onsemi.Properties.Resources.DioRectangleOff;
            this.pictureBoxDispenserIO_OUT_TDM_STEP.Location = new System.Drawing.Point(11, 78);
            this.pictureBoxDispenserIO_OUT_TDM_STEP.Name = "pictureBoxDispenserIO_OUT_TDM_STEP";
            this.pictureBoxDispenserIO_OUT_TDM_STEP.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxDispenserIO_OUT_TDM_STEP.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxDispenserIO_OUT_TDM_STEP.TabIndex = 20;
            this.pictureBoxDispenserIO_OUT_TDM_STEP.TabStop = false;
            // 
            // pictureBoxDispenserIO_OUT_TMS_PRESET
            // 
            this.pictureBoxDispenserIO_OUT_TMS_PRESET.Image = global::CWA150SA_Onsemi.Properties.Resources.DioRectangleOff;
            this.pictureBoxDispenserIO_OUT_TMS_PRESET.Location = new System.Drawing.Point(11, 49);
            this.pictureBoxDispenserIO_OUT_TMS_PRESET.Name = "pictureBoxDispenserIO_OUT_TMS_PRESET";
            this.pictureBoxDispenserIO_OUT_TMS_PRESET.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxDispenserIO_OUT_TMS_PRESET.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxDispenserIO_OUT_TMS_PRESET.TabIndex = 19;
            this.pictureBoxDispenserIO_OUT_TMS_PRESET.TabStop = false;
            // 
            // pictureBoxDispenserIO_OUT_DIS
            // 
            this.pictureBoxDispenserIO_OUT_DIS.Image = global::CWA150SA_Onsemi.Properties.Resources.DioRectangleOff;
            this.pictureBoxDispenserIO_OUT_DIS.Location = new System.Drawing.Point(11, 20);
            this.pictureBoxDispenserIO_OUT_DIS.Name = "pictureBoxDispenserIO_OUT_DIS";
            this.pictureBoxDispenserIO_OUT_DIS.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxDispenserIO_OUT_DIS.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxDispenserIO_OUT_DIS.TabIndex = 18;
            this.pictureBoxDispenserIO_OUT_DIS.TabStop = false;
            // 
            // labelDispenserIO_OUT_CHS
            // 
            this.labelDispenserIO_OUT_CHS.AutoSize = true;
            this.labelDispenserIO_OUT_CHS.Location = new System.Drawing.Point(42, 113);
            this.labelDispenserIO_OUT_CHS.Name = "labelDispenserIO_OUT_CHS";
            this.labelDispenserIO_OUT_CHS.Size = new System.Drawing.Size(30, 12);
            this.labelDispenserIO_OUT_CHS.TabIndex = 10;
            this.labelDispenserIO_OUT_CHS.Text = "CHS";
            // 
            // labelDispenserIO_OUT_TDM_STEP
            // 
            this.labelDispenserIO_OUT_TDM_STEP.AutoSize = true;
            this.labelDispenserIO_OUT_TDM_STEP.Location = new System.Drawing.Point(42, 84);
            this.labelDispenserIO_OUT_TDM_STEP.Name = "labelDispenserIO_OUT_TDM_STEP";
            this.labelDispenserIO_OUT_TDM_STEP.Size = new System.Drawing.Size(78, 12);
            this.labelDispenserIO_OUT_TDM_STEP.TabIndex = 10;
            this.labelDispenserIO_OUT_TDM_STEP.Text = "TDM / STEP";
            // 
            // labelDispenserIO_OUT_TMS_PRESET
            // 
            this.labelDispenserIO_OUT_TMS_PRESET.AutoSize = true;
            this.labelDispenserIO_OUT_TMS_PRESET.Location = new System.Drawing.Point(42, 55);
            this.labelDispenserIO_OUT_TMS_PRESET.Name = "labelDispenserIO_OUT_TMS_PRESET";
            this.labelDispenserIO_OUT_TMS_PRESET.Size = new System.Drawing.Size(94, 12);
            this.labelDispenserIO_OUT_TMS_PRESET.TabIndex = 10;
            this.labelDispenserIO_OUT_TMS_PRESET.Text = "TMS / PRESET";
            // 
            // labelDispenserIO_OUT_DIS
            // 
            this.labelDispenserIO_OUT_DIS.AutoSize = true;
            this.labelDispenserIO_OUT_DIS.Location = new System.Drawing.Point(42, 26);
            this.labelDispenserIO_OUT_DIS.Name = "labelDispenserIO_OUT_DIS";
            this.labelDispenserIO_OUT_DIS.Size = new System.Drawing.Size(24, 12);
            this.labelDispenserIO_OUT_DIS.TabIndex = 10;
            this.labelDispenserIO_OUT_DIS.Text = "DIS";
            // 
            // groupBoxDispenserInput
            // 
            this.groupBoxDispenserInput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.groupBoxDispenserInput.Controls.Add(this.pictureBoxDispenserIO_IN_DVO);
            this.groupBoxDispenserInput.Controls.Add(this.pictureBoxDispenserIO_IN_PSE);
            this.groupBoxDispenserInput.Controls.Add(this.pictureBoxDispenserIO_IN_RSM);
            this.groupBoxDispenserInput.Controls.Add(this.pictureBoxDispenserIO_IN_DSO);
            this.groupBoxDispenserInput.Controls.Add(this.pictureBoxDispenserIO_IN_READY_DVO);
            this.groupBoxDispenserInput.Controls.Add(this.pictureBoxDispenserIO_IN_PON);
            this.groupBoxDispenserInput.Controls.Add(this.pictureBoxDispenserIO_IN_EXE);
            this.groupBoxDispenserInput.Controls.Add(this.labelDispenserIO_IN_EXE);
            this.groupBoxDispenserInput.Controls.Add(this.pictureBoxDispenserIO_IN_DSO_END);
            this.groupBoxDispenserInput.Controls.Add(this.labelDispenserIO_IN_DSO_END);
            this.groupBoxDispenserInput.Controls.Add(this.labelDispenserIO_IN_DVO);
            this.groupBoxDispenserInput.Controls.Add(this.labelDispenserIO_IN_PSE);
            this.groupBoxDispenserInput.Controls.Add(this.labelDispenserIO_IN_RSM);
            this.groupBoxDispenserInput.Controls.Add(this.labelDispenserIO_IN_DSO);
            this.groupBoxDispenserInput.Controls.Add(this.labelDispenserIO_IN_READY_DVO);
            this.groupBoxDispenserInput.Controls.Add(this.labelDispenserIO_IN_PON);
            this.groupBoxDispenserInput.ForeColor = System.Drawing.Color.White;
            this.groupBoxDispenserInput.Location = new System.Drawing.Point(15, 28);
            this.groupBoxDispenserInput.Name = "groupBoxDispenserInput";
            this.groupBoxDispenserInput.Size = new System.Drawing.Size(144, 258);
            this.groupBoxDispenserInput.TabIndex = 10;
            this.groupBoxDispenserInput.TabStop = false;
            this.groupBoxDispenserInput.Text = " Input ";
            // 
            // pictureBoxDispenserIO_IN_DVO
            // 
            this.pictureBoxDispenserIO_IN_DVO.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
            this.pictureBoxDispenserIO_IN_DVO.Location = new System.Drawing.Point(11, 223);
            this.pictureBoxDispenserIO_IN_DVO.Name = "pictureBoxDispenserIO_IN_DVO";
            this.pictureBoxDispenserIO_IN_DVO.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxDispenserIO_IN_DVO.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxDispenserIO_IN_DVO.TabIndex = 26;
            this.pictureBoxDispenserIO_IN_DVO.TabStop = false;
            // 
            // pictureBoxDispenserIO_IN_PSE
            // 
            this.pictureBoxDispenserIO_IN_PSE.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
            this.pictureBoxDispenserIO_IN_PSE.Location = new System.Drawing.Point(11, 194);
            this.pictureBoxDispenserIO_IN_PSE.Name = "pictureBoxDispenserIO_IN_PSE";
            this.pictureBoxDispenserIO_IN_PSE.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxDispenserIO_IN_PSE.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxDispenserIO_IN_PSE.TabIndex = 25;
            this.pictureBoxDispenserIO_IN_PSE.TabStop = false;
            // 
            // pictureBoxDispenserIO_IN_RSM
            // 
            this.pictureBoxDispenserIO_IN_RSM.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
            this.pictureBoxDispenserIO_IN_RSM.Location = new System.Drawing.Point(11, 165);
            this.pictureBoxDispenserIO_IN_RSM.Name = "pictureBoxDispenserIO_IN_RSM";
            this.pictureBoxDispenserIO_IN_RSM.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxDispenserIO_IN_RSM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxDispenserIO_IN_RSM.TabIndex = 24;
            this.pictureBoxDispenserIO_IN_RSM.TabStop = false;
            // 
            // pictureBoxDispenserIO_IN_DSO
            // 
            this.pictureBoxDispenserIO_IN_DSO.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
            this.pictureBoxDispenserIO_IN_DSO.Location = new System.Drawing.Point(11, 136);
            this.pictureBoxDispenserIO_IN_DSO.Name = "pictureBoxDispenserIO_IN_DSO";
            this.pictureBoxDispenserIO_IN_DSO.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxDispenserIO_IN_DSO.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxDispenserIO_IN_DSO.TabIndex = 23;
            this.pictureBoxDispenserIO_IN_DSO.TabStop = false;
            // 
            // pictureBoxDispenserIO_IN_READY_DVO
            // 
            this.pictureBoxDispenserIO_IN_READY_DVO.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
            this.pictureBoxDispenserIO_IN_READY_DVO.Location = new System.Drawing.Point(11, 107);
            this.pictureBoxDispenserIO_IN_READY_DVO.Name = "pictureBoxDispenserIO_IN_READY_DVO";
            this.pictureBoxDispenserIO_IN_READY_DVO.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxDispenserIO_IN_READY_DVO.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxDispenserIO_IN_READY_DVO.TabIndex = 22;
            this.pictureBoxDispenserIO_IN_READY_DVO.TabStop = false;
            // 
            // pictureBoxDispenserIO_IN_PON
            // 
            this.pictureBoxDispenserIO_IN_PON.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
            this.pictureBoxDispenserIO_IN_PON.Location = new System.Drawing.Point(11, 78);
            this.pictureBoxDispenserIO_IN_PON.Name = "pictureBoxDispenserIO_IN_PON";
            this.pictureBoxDispenserIO_IN_PON.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxDispenserIO_IN_PON.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxDispenserIO_IN_PON.TabIndex = 21;
            this.pictureBoxDispenserIO_IN_PON.TabStop = false;
            // 
            // pictureBoxDispenserIO_IN_EXE
            // 
            this.pictureBoxDispenserIO_IN_EXE.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
            this.pictureBoxDispenserIO_IN_EXE.Location = new System.Drawing.Point(11, 49);
            this.pictureBoxDispenserIO_IN_EXE.Name = "pictureBoxDispenserIO_IN_EXE";
            this.pictureBoxDispenserIO_IN_EXE.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxDispenserIO_IN_EXE.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxDispenserIO_IN_EXE.TabIndex = 20;
            this.pictureBoxDispenserIO_IN_EXE.TabStop = false;
            // 
            // labelDispenserIO_IN_EXE
            // 
            this.labelDispenserIO_IN_EXE.AutoSize = true;
            this.labelDispenserIO_IN_EXE.Location = new System.Drawing.Point(42, 55);
            this.labelDispenserIO_IN_EXE.Name = "labelDispenserIO_IN_EXE";
            this.labelDispenserIO_IN_EXE.Size = new System.Drawing.Size(29, 12);
            this.labelDispenserIO_IN_EXE.TabIndex = 12;
            this.labelDispenserIO_IN_EXE.Text = "EXE";
            // 
            // pictureBoxDispenserIO_IN_DSO_END
            // 
            this.pictureBoxDispenserIO_IN_DSO_END.Image = global::CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
            this.pictureBoxDispenserIO_IN_DSO_END.Location = new System.Drawing.Point(11, 20);
            this.pictureBoxDispenserIO_IN_DSO_END.Name = "pictureBoxDispenserIO_IN_DSO_END";
            this.pictureBoxDispenserIO_IN_DSO_END.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxDispenserIO_IN_DSO_END.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxDispenserIO_IN_DSO_END.TabIndex = 19;
            this.pictureBoxDispenserIO_IN_DSO_END.TabStop = false;
            // 
            // labelDispenserIO_IN_DSO_END
            // 
            this.labelDispenserIO_IN_DSO_END.AutoSize = true;
            this.labelDispenserIO_IN_DSO_END.Location = new System.Drawing.Point(42, 26);
            this.labelDispenserIO_IN_DSO_END.Name = "labelDispenserIO_IN_DSO_END";
            this.labelDispenserIO_IN_DSO_END.Size = new System.Drawing.Size(78, 12);
            this.labelDispenserIO_IN_DSO_END.TabIndex = 12;
            this.labelDispenserIO_IN_DSO_END.Text = "DSO~ / END";
            // 
            // labelDispenserIO_IN_DVO
            // 
            this.labelDispenserIO_IN_DVO.AutoSize = true;
            this.labelDispenserIO_IN_DVO.Location = new System.Drawing.Point(42, 229);
            this.labelDispenserIO_IN_DVO.Name = "labelDispenserIO_IN_DVO";
            this.labelDispenserIO_IN_DVO.Size = new System.Drawing.Size(30, 12);
            this.labelDispenserIO_IN_DVO.TabIndex = 10;
            this.labelDispenserIO_IN_DVO.Text = "DVO";
            // 
            // labelDispenserIO_IN_PSE
            // 
            this.labelDispenserIO_IN_PSE.AutoSize = true;
            this.labelDispenserIO_IN_PSE.Location = new System.Drawing.Point(42, 200);
            this.labelDispenserIO_IN_PSE.Name = "labelDispenserIO_IN_PSE";
            this.labelDispenserIO_IN_PSE.Size = new System.Drawing.Size(29, 12);
            this.labelDispenserIO_IN_PSE.TabIndex = 10;
            this.labelDispenserIO_IN_PSE.Text = "PSE";
            // 
            // labelDispenserIO_IN_RSM
            // 
            this.labelDispenserIO_IN_RSM.AutoSize = true;
            this.labelDispenserIO_IN_RSM.Location = new System.Drawing.Point(42, 171);
            this.labelDispenserIO_IN_RSM.Name = "labelDispenserIO_IN_RSM";
            this.labelDispenserIO_IN_RSM.Size = new System.Drawing.Size(32, 12);
            this.labelDispenserIO_IN_RSM.TabIndex = 10;
            this.labelDispenserIO_IN_RSM.Text = "RSM";
            // 
            // labelDispenserIO_IN_DSO
            // 
            this.labelDispenserIO_IN_DSO.AutoSize = true;
            this.labelDispenserIO_IN_DSO.Location = new System.Drawing.Point(42, 142);
            this.labelDispenserIO_IN_DSO.Name = "labelDispenserIO_IN_DSO";
            this.labelDispenserIO_IN_DSO.Size = new System.Drawing.Size(30, 12);
            this.labelDispenserIO_IN_DSO.TabIndex = 10;
            this.labelDispenserIO_IN_DSO.Text = "DSO";
            // 
            // labelDispenserIO_IN_READY_DVO
            // 
            this.labelDispenserIO_IN_READY_DVO.AutoSize = true;
            this.labelDispenserIO_IN_READY_DVO.Location = new System.Drawing.Point(42, 113);
            this.labelDispenserIO_IN_READY_DVO.Name = "labelDispenserIO_IN_READY_DVO";
            this.labelDispenserIO_IN_READY_DVO.Size = new System.Drawing.Size(93, 12);
            this.labelDispenserIO_IN_READY_DVO.TabIndex = 10;
            this.labelDispenserIO_IN_READY_DVO.Text = "READY / DVO~";
            // 
            // labelDispenserIO_IN_PON
            // 
            this.labelDispenserIO_IN_PON.AutoSize = true;
            this.labelDispenserIO_IN_PON.Location = new System.Drawing.Point(42, 84);
            this.labelDispenserIO_IN_PON.Name = "labelDispenserIO_IN_PON";
            this.labelDispenserIO_IN_PON.Size = new System.Drawing.Size(31, 12);
            this.labelDispenserIO_IN_PON.TabIndex = 10;
            this.labelDispenserIO_IN_PON.Text = "PON";
            // 
            // btnDispenserOn
            // 
            this.btnDispenserOn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnDispenserOn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDispenserOn.ForeColor = System.Drawing.Color.White;
            this.btnDispenserOn.Location = new System.Drawing.Point(195, 232);
            this.btnDispenserOn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnDispenserOn.Name = "btnDispenserOn";
            this.btnDispenserOn.Size = new System.Drawing.Size(129, 54);
            this.btnDispenserOn.TabIndex = 8;
            this.btnDispenserOn.Text = "Shot";
            this.btnDispenserOn.UseVisualStyleBackColor = false;
            this.btnDispenserOn.Click += new System.EventHandler(this.btnDispenserOn_Click);
            // 
            // Dispenser
            // 
            this.Controls.Add(this.groupBoxDispenser);
            this.Name = "Dispenser";
            this.Size = new System.Drawing.Size(343, 303);
            this.groupBoxDispenser.ResumeLayout(false);
            this.groupBoxDispenserOutput.ResumeLayout(false);
            this.groupBoxDispenserOutput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDispenserIO_OUT_CHS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDispenserIO_OUT_TDM_STEP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDispenserIO_OUT_TMS_PRESET)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDispenserIO_OUT_DIS)).EndInit();
            this.groupBoxDispenserInput.ResumeLayout(false);
            this.groupBoxDispenserInput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDispenserIO_IN_DVO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDispenserIO_IN_PSE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDispenserIO_IN_RSM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDispenserIO_IN_DSO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDispenserIO_IN_READY_DVO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDispenserIO_IN_PON)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDispenserIO_IN_EXE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDispenserIO_IN_DSO_END)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBoxDispenser;
        private System.Windows.Forms.Button btnDispenserOn;
        private System.Windows.Forms.GroupBox groupBoxDispenserOutput;
        private System.Windows.Forms.Label labelDispenserIO_OUT_DIS;
        private System.Windows.Forms.GroupBox groupBoxDispenserInput;
        private System.Windows.Forms.Label labelDispenserIO_IN_EXE;
        private System.Windows.Forms.Label labelDispenserIO_IN_DSO_END;
        private System.Windows.Forms.Label labelDispenserIO_IN_PON;
        private System.Windows.Forms.Label labelDispenserIO_IN_READY_DVO;
        private System.Windows.Forms.Label labelDispenserIO_IN_DVO;
        private System.Windows.Forms.Label labelDispenserIO_IN_PSE;
        private System.Windows.Forms.Label labelDispenserIO_IN_RSM;
        private System.Windows.Forms.Label labelDispenserIO_IN_DSO;
        private System.Windows.Forms.Label labelDispenserIO_OUT_CHS;
        private System.Windows.Forms.Label labelDispenserIO_OUT_TDM_STEP;
        private System.Windows.Forms.Label labelDispenserIO_OUT_TMS_PRESET;
        private System.Windows.Forms.PictureBox pictureBoxDispenserIO_OUT_DIS;
        private System.Windows.Forms.PictureBox pictureBoxDispenserIO_IN_DVO;
        private System.Windows.Forms.PictureBox pictureBoxDispenserIO_IN_PSE;
        private System.Windows.Forms.PictureBox pictureBoxDispenserIO_IN_RSM;
        private System.Windows.Forms.PictureBox pictureBoxDispenserIO_IN_DSO;
        private System.Windows.Forms.PictureBox pictureBoxDispenserIO_IN_READY_DVO;
        private System.Windows.Forms.PictureBox pictureBoxDispenserIO_IN_PON;
        private System.Windows.Forms.PictureBox pictureBoxDispenserIO_IN_EXE;
        private System.Windows.Forms.PictureBox pictureBoxDispenserIO_IN_DSO_END;
        private System.Windows.Forms.PictureBox pictureBoxDispenserIO_OUT_CHS;
        private System.Windows.Forms.PictureBox pictureBoxDispenserIO_OUT_TDM_STEP;
        private System.Windows.Forms.PictureBox pictureBoxDispenserIO_OUT_TMS_PRESET;
    }
}
