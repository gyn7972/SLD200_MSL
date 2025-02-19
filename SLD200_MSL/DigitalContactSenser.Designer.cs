namespace SLD200_MSL
{
    partial class DigitalContactSensor
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
            this.groupBoxDigitalContactSensor = new System.Windows.Forms.GroupBox();
            this.btnAirCylinderOff = new System.Windows.Forms.Button();
            this.btnAirCylinderOn = new System.Windows.Forms.Button();
            this.LabelWeighValueUnit = new SLD200_MSL.BaseLabel();
            this.textBoxDigitalContactSensorValue = new System.Windows.Forms.TextBox();
            this.groupBoxDigitalContactSensorOutput = new System.Windows.Forms.GroupBox();
            this.pictureBoxDigitalContactSensorIO_OUT_RESET = new System.Windows.Forms.PictureBox();
            this.pictureBoxDigitalContactSensorIO_OUT_BANKB = new System.Windows.Forms.PictureBox();
            this.pictureBoxDigitalContactSensorIO_OUT_TIMING = new System.Windows.Forms.PictureBox();
            this.pictureBoxDigitalContactSensorIO_OUT_BANKA = new System.Windows.Forms.PictureBox();
            this.pictureBoxDigitalContactSensorIO_OUT_PRESET = new System.Windows.Forms.PictureBox();
            this.labelDigitalContactSensorIO_OUT_RESET = new System.Windows.Forms.Label();
            this.labelDigitalContactSensorIO_OUT_BANKB = new System.Windows.Forms.Label();
            this.labelDigitalContactSensorIO_OUT_TIMING = new System.Windows.Forms.Label();
            this.labelDigitalContactSensorIO_OUT_BANKA = new System.Windows.Forms.Label();
            this.labelDigitalContactSensorIO_OUT_PRESET = new System.Windows.Forms.Label();
            this.groupBoxDigitalContactSensorInput = new System.Windows.Forms.GroupBox();
            this.pictureBoxDigitalContactSensorIO_IN_LL = new System.Windows.Forms.PictureBox();
            this.pictureBoxDigitalContactSensorIO_IN_HH = new System.Windows.Forms.PictureBox();
            this.pictureBoxDigitalContactSensorIO_IN_GO = new System.Windows.Forms.PictureBox();
            this.pictureBoxDigitalContactSensorIO_IN_LO = new System.Windows.Forms.PictureBox();
            this.pictureBoxDigitalContactSensorIO_IN_HIGH = new System.Windows.Forms.PictureBox();
            this.labelDigitalContactSensorIO_IN_LO = new System.Windows.Forms.Label();
            this.labelDigitalContactSensorIO_IN_HIGH = new System.Windows.Forms.Label();
            this.labelDigitalContactSensorIO_IN_LL = new System.Windows.Forms.Label();
            this.labelDigitalContactSensorIO_IN_HH = new System.Windows.Forms.Label();
            this.labelDigitalContactSensorIO_IN_GO = new System.Windows.Forms.Label();
            this.btnDigitalContactSensorReset = new System.Windows.Forms.Button();
            this.btnDigitalContactSensorPreset = new System.Windows.Forms.Button();
            this.groupBoxDigitalContactSensor.SuspendLayout();
            this.groupBoxDigitalContactSensorOutput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDigitalContactSensorIO_OUT_RESET)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDigitalContactSensorIO_OUT_BANKB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDigitalContactSensorIO_OUT_TIMING)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDigitalContactSensorIO_OUT_BANKA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDigitalContactSensorIO_OUT_PRESET)).BeginInit();
            this.groupBoxDigitalContactSensorInput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDigitalContactSensorIO_IN_LL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDigitalContactSensorIO_IN_HH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDigitalContactSensorIO_IN_GO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDigitalContactSensorIO_IN_LO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDigitalContactSensorIO_IN_HIGH)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonStop
            // 
            this.buttonStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.buttonStop.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
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
            // groupBoxDigitalContactSensor
            // 
            this.groupBoxDigitalContactSensor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.groupBoxDigitalContactSensor.Controls.Add(this.btnAirCylinderOff);
            this.groupBoxDigitalContactSensor.Controls.Add(this.btnAirCylinderOn);
            this.groupBoxDigitalContactSensor.Controls.Add(this.LabelWeighValueUnit);
            this.groupBoxDigitalContactSensor.Controls.Add(this.textBoxDigitalContactSensorValue);
            this.groupBoxDigitalContactSensor.Controls.Add(this.groupBoxDigitalContactSensorOutput);
            this.groupBoxDigitalContactSensor.Controls.Add(this.groupBoxDigitalContactSensorInput);
            this.groupBoxDigitalContactSensor.Controls.Add(this.btnDigitalContactSensorReset);
            this.groupBoxDigitalContactSensor.Controls.Add(this.btnDigitalContactSensorPreset);
            this.groupBoxDigitalContactSensor.ForeColor = System.Drawing.Color.White;
            this.groupBoxDigitalContactSensor.Location = new System.Drawing.Point(3, 3);
            this.groupBoxDigitalContactSensor.Name = "groupBoxDigitalContactSensor";
            this.groupBoxDigitalContactSensor.Size = new System.Drawing.Size(515, 297);
            this.groupBoxDigitalContactSensor.TabIndex = 10;
            this.groupBoxDigitalContactSensor.TabStop = false;
            this.groupBoxDigitalContactSensor.Text = " Digital Contact Sensor ";
            // 
            // btnAirCylinderOff
            // 
            this.btnAirCylinderOff.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnAirCylinderOff.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnAirCylinderOff.ForeColor = System.Drawing.Color.White;
            this.btnAirCylinderOff.Location = new System.Drawing.Point(174, 231);
            this.btnAirCylinderOff.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAirCylinderOff.Name = "btnAirCylinderOff";
            this.btnAirCylinderOff.Size = new System.Drawing.Size(144, 38);
            this.btnAirCylinderOff.TabIndex = 17;
            this.btnAirCylinderOff.Text = "Cylinder Air Off";
            this.btnAirCylinderOff.UseVisualStyleBackColor = false;
            // 
            // btnAirCylinderOn
            // 
            this.btnAirCylinderOn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnAirCylinderOn.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnAirCylinderOn.ForeColor = System.Drawing.Color.White;
            this.btnAirCylinderOn.Location = new System.Drawing.Point(15, 231);
            this.btnAirCylinderOn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAirCylinderOn.Name = "btnAirCylinderOn";
            this.btnAirCylinderOn.Size = new System.Drawing.Size(144, 38);
            this.btnAirCylinderOn.TabIndex = 16;
            this.btnAirCylinderOn.Text = "Cylinder Air On";
            this.btnAirCylinderOn.UseVisualStyleBackColor = false;
            // 
            // LabelWeighValueUnit
            // 
            this.LabelWeighValueUnit.AutoSize = true;
            this.LabelWeighValueUnit.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.LabelWeighValueUnit.ForeColor = System.Drawing.Color.White;
            this.LabelWeighValueUnit.Location = new System.Drawing.Point(476, 184);
            this.LabelWeighValueUnit.Name = "LabelWeighValueUnit";
            this.LabelWeighValueUnit.Size = new System.Drawing.Size(23, 16);
            this.LabelWeighValueUnit.TabIndex = 15;
            this.LabelWeighValueUnit.Text = "㎜";
            // 
            // textBoxDigitalContactSensorValue
            // 
            this.textBoxDigitalContactSensorValue.Location = new System.Drawing.Point(361, 179);
            this.textBoxDigitalContactSensorValue.Name = "textBoxDigitalContactSensorValue";
            this.textBoxDigitalContactSensorValue.Size = new System.Drawing.Size(98, 21);
            this.textBoxDigitalContactSensorValue.TabIndex = 14;
            this.textBoxDigitalContactSensorValue.Text = "0.0";
            this.textBoxDigitalContactSensorValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // groupBoxDigitalContactSensorOutput
            // 
            this.groupBoxDigitalContactSensorOutput.Controls.Add(this.pictureBoxDigitalContactSensorIO_OUT_RESET);
            this.groupBoxDigitalContactSensorOutput.Controls.Add(this.pictureBoxDigitalContactSensorIO_OUT_BANKB);
            this.groupBoxDigitalContactSensorOutput.Controls.Add(this.pictureBoxDigitalContactSensorIO_OUT_TIMING);
            this.groupBoxDigitalContactSensorOutput.Controls.Add(this.pictureBoxDigitalContactSensorIO_OUT_BANKA);
            this.groupBoxDigitalContactSensorOutput.Controls.Add(this.pictureBoxDigitalContactSensorIO_OUT_PRESET);
            this.groupBoxDigitalContactSensorOutput.Controls.Add(this.labelDigitalContactSensorIO_OUT_RESET);
            this.groupBoxDigitalContactSensorOutput.Controls.Add(this.labelDigitalContactSensorIO_OUT_BANKB);
            this.groupBoxDigitalContactSensorOutput.Controls.Add(this.labelDigitalContactSensorIO_OUT_TIMING);
            this.groupBoxDigitalContactSensorOutput.Controls.Add(this.labelDigitalContactSensorIO_OUT_BANKA);
            this.groupBoxDigitalContactSensorOutput.Controls.Add(this.labelDigitalContactSensorIO_OUT_PRESET);
            this.groupBoxDigitalContactSensorOutput.ForeColor = System.Drawing.Color.White;
            this.groupBoxDigitalContactSensorOutput.Location = new System.Drawing.Point(174, 28);
            this.groupBoxDigitalContactSensorOutput.Name = "groupBoxDigitalContactSensorOutput";
            this.groupBoxDigitalContactSensorOutput.Size = new System.Drawing.Size(150, 172);
            this.groupBoxDigitalContactSensorOutput.TabIndex = 13;
            this.groupBoxDigitalContactSensorOutput.TabStop = false;
            this.groupBoxDigitalContactSensorOutput.Text = " Output ";
            // 
            // pictureBoxDigitalContactSensorIO_OUT_RESET
            // 
            this.pictureBoxDigitalContactSensorIO_OUT_RESET.Image = global::SLD200.Properties.Resources.DioRectangleOff;
            this.pictureBoxDigitalContactSensorIO_OUT_RESET.Location = new System.Drawing.Point(11, 136);
            this.pictureBoxDigitalContactSensorIO_OUT_RESET.Name = "pictureBoxDigitalContactSensorIO_OUT_RESET";
            this.pictureBoxDigitalContactSensorIO_OUT_RESET.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxDigitalContactSensorIO_OUT_RESET.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxDigitalContactSensorIO_OUT_RESET.TabIndex = 26;
            this.pictureBoxDigitalContactSensorIO_OUT_RESET.TabStop = false;
            // 
            // pictureBoxDigitalContactSensorIO_OUT_BANKB
            // 
            this.pictureBoxDigitalContactSensorIO_OUT_BANKB.Image = global::SLD200.Properties.Resources.DioRectangleOff;
            this.pictureBoxDigitalContactSensorIO_OUT_BANKB.Location = new System.Drawing.Point(11, 107);
            this.pictureBoxDigitalContactSensorIO_OUT_BANKB.Name = "pictureBoxDigitalContactSensorIO_OUT_BANKB";
            this.pictureBoxDigitalContactSensorIO_OUT_BANKB.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxDigitalContactSensorIO_OUT_BANKB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxDigitalContactSensorIO_OUT_BANKB.TabIndex = 25;
            this.pictureBoxDigitalContactSensorIO_OUT_BANKB.TabStop = false;
            // 
            // pictureBoxDigitalContactSensorIO_OUT_TIMING
            // 
            this.pictureBoxDigitalContactSensorIO_OUT_TIMING.Image = global::SLD200.Properties.Resources.DioRectangleOff;
            this.pictureBoxDigitalContactSensorIO_OUT_TIMING.Location = new System.Drawing.Point(11, 78);
            this.pictureBoxDigitalContactSensorIO_OUT_TIMING.Name = "pictureBoxDigitalContactSensorIO_OUT_TIMING";
            this.pictureBoxDigitalContactSensorIO_OUT_TIMING.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxDigitalContactSensorIO_OUT_TIMING.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxDigitalContactSensorIO_OUT_TIMING.TabIndex = 24;
            this.pictureBoxDigitalContactSensorIO_OUT_TIMING.TabStop = false;
            // 
            // pictureBoxDigitalContactSensorIO_OUT_BANKA
            // 
            this.pictureBoxDigitalContactSensorIO_OUT_BANKA.Image = global::SLD200.Properties.Resources.DioRectangleOff;
            this.pictureBoxDigitalContactSensorIO_OUT_BANKA.Location = new System.Drawing.Point(11, 49);
            this.pictureBoxDigitalContactSensorIO_OUT_BANKA.Name = "pictureBoxDigitalContactSensorIO_OUT_BANKA";
            this.pictureBoxDigitalContactSensorIO_OUT_BANKA.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxDigitalContactSensorIO_OUT_BANKA.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxDigitalContactSensorIO_OUT_BANKA.TabIndex = 23;
            this.pictureBoxDigitalContactSensorIO_OUT_BANKA.TabStop = false;
            // 
            // pictureBoxDigitalContactSensorIO_OUT_PRESET
            // 
            this.pictureBoxDigitalContactSensorIO_OUT_PRESET.Image = global::SLD200.Properties.Resources.DioRectangleOff;
            this.pictureBoxDigitalContactSensorIO_OUT_PRESET.Location = new System.Drawing.Point(11, 20);
            this.pictureBoxDigitalContactSensorIO_OUT_PRESET.Name = "pictureBoxDigitalContactSensorIO_OUT_PRESET";
            this.pictureBoxDigitalContactSensorIO_OUT_PRESET.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxDigitalContactSensorIO_OUT_PRESET.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxDigitalContactSensorIO_OUT_PRESET.TabIndex = 22;
            this.pictureBoxDigitalContactSensorIO_OUT_PRESET.TabStop = false;
            // 
            // labelDigitalContactSensorIO_OUT_RESET
            // 
            this.labelDigitalContactSensorIO_OUT_RESET.AutoSize = true;
            this.labelDigitalContactSensorIO_OUT_RESET.Location = new System.Drawing.Point(42, 142);
            this.labelDigitalContactSensorIO_OUT_RESET.Name = "labelDigitalContactSensorIO_OUT_RESET";
            this.labelDigitalContactSensorIO_OUT_RESET.Size = new System.Drawing.Size(45, 12);
            this.labelDigitalContactSensorIO_OUT_RESET.TabIndex = 10;
            this.labelDigitalContactSensorIO_OUT_RESET.Text = "RESET";
            // 
            // labelDigitalContactSensorIO_OUT_BANKB
            // 
            this.labelDigitalContactSensorIO_OUT_BANKB.AutoSize = true;
            this.labelDigitalContactSensorIO_OUT_BANKB.Location = new System.Drawing.Point(42, 113);
            this.labelDigitalContactSensorIO_OUT_BANKB.Name = "labelDigitalContactSensorIO_OUT_BANKB";
            this.labelDigitalContactSensorIO_OUT_BANKB.Size = new System.Drawing.Size(50, 12);
            this.labelDigitalContactSensorIO_OUT_BANKB.TabIndex = 10;
            this.labelDigitalContactSensorIO_OUT_BANKB.Text = "BANK B";
            // 
            // labelDigitalContactSensorIO_OUT_TIMING
            // 
            this.labelDigitalContactSensorIO_OUT_TIMING.AutoSize = true;
            this.labelDigitalContactSensorIO_OUT_TIMING.Location = new System.Drawing.Point(42, 84);
            this.labelDigitalContactSensorIO_OUT_TIMING.Name = "labelDigitalContactSensorIO_OUT_TIMING";
            this.labelDigitalContactSensorIO_OUT_TIMING.Size = new System.Drawing.Size(48, 12);
            this.labelDigitalContactSensorIO_OUT_TIMING.TabIndex = 10;
            this.labelDigitalContactSensorIO_OUT_TIMING.Text = "TIMING";
            // 
            // labelDigitalContactSensorIO_OUT_BANKA
            // 
            this.labelDigitalContactSensorIO_OUT_BANKA.AutoSize = true;
            this.labelDigitalContactSensorIO_OUT_BANKA.Location = new System.Drawing.Point(42, 55);
            this.labelDigitalContactSensorIO_OUT_BANKA.Name = "labelDigitalContactSensorIO_OUT_BANKA";
            this.labelDigitalContactSensorIO_OUT_BANKA.Size = new System.Drawing.Size(50, 12);
            this.labelDigitalContactSensorIO_OUT_BANKA.TabIndex = 10;
            this.labelDigitalContactSensorIO_OUT_BANKA.Text = "BANK A";
            // 
            // labelDigitalContactSensorIO_OUT_PRESET
            // 
            this.labelDigitalContactSensorIO_OUT_PRESET.AutoSize = true;
            this.labelDigitalContactSensorIO_OUT_PRESET.Location = new System.Drawing.Point(42, 26);
            this.labelDigitalContactSensorIO_OUT_PRESET.Name = "labelDigitalContactSensorIO_OUT_PRESET";
            this.labelDigitalContactSensorIO_OUT_PRESET.Size = new System.Drawing.Size(53, 12);
            this.labelDigitalContactSensorIO_OUT_PRESET.TabIndex = 10;
            this.labelDigitalContactSensorIO_OUT_PRESET.Text = "PRESET";
            // 
            // groupBoxDigitalContactSensorInput
            // 
            this.groupBoxDigitalContactSensorInput.Controls.Add(this.pictureBoxDigitalContactSensorIO_IN_LL);
            this.groupBoxDigitalContactSensorInput.Controls.Add(this.pictureBoxDigitalContactSensorIO_IN_HH);
            this.groupBoxDigitalContactSensorInput.Controls.Add(this.pictureBoxDigitalContactSensorIO_IN_GO);
            this.groupBoxDigitalContactSensorInput.Controls.Add(this.pictureBoxDigitalContactSensorIO_IN_LO);
            this.groupBoxDigitalContactSensorInput.Controls.Add(this.pictureBoxDigitalContactSensorIO_IN_HIGH);
            this.groupBoxDigitalContactSensorInput.Controls.Add(this.labelDigitalContactSensorIO_IN_LO);
            this.groupBoxDigitalContactSensorInput.Controls.Add(this.labelDigitalContactSensorIO_IN_HIGH);
            this.groupBoxDigitalContactSensorInput.Controls.Add(this.labelDigitalContactSensorIO_IN_LL);
            this.groupBoxDigitalContactSensorInput.Controls.Add(this.labelDigitalContactSensorIO_IN_HH);
            this.groupBoxDigitalContactSensorInput.Controls.Add(this.labelDigitalContactSensorIO_IN_GO);
            this.groupBoxDigitalContactSensorInput.ForeColor = System.Drawing.Color.White;
            this.groupBoxDigitalContactSensorInput.Location = new System.Drawing.Point(15, 28);
            this.groupBoxDigitalContactSensorInput.Name = "groupBoxDigitalContactSensorInput";
            this.groupBoxDigitalContactSensorInput.Size = new System.Drawing.Size(144, 172);
            this.groupBoxDigitalContactSensorInput.TabIndex = 10;
            this.groupBoxDigitalContactSensorInput.TabStop = false;
            this.groupBoxDigitalContactSensorInput.Text = " Input ";
            // 
            // pictureBoxDigitalContactSensorIO_IN_LL
            // 
            this.pictureBoxDigitalContactSensorIO_IN_LL.Image = global::SLD200.Properties.Resources.DioEllipseOff;
            this.pictureBoxDigitalContactSensorIO_IN_LL.Location = new System.Drawing.Point(11, 136);
            this.pictureBoxDigitalContactSensorIO_IN_LL.Name = "pictureBoxDigitalContactSensorIO_IN_LL";
            this.pictureBoxDigitalContactSensorIO_IN_LL.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxDigitalContactSensorIO_IN_LL.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxDigitalContactSensorIO_IN_LL.TabIndex = 28;
            this.pictureBoxDigitalContactSensorIO_IN_LL.TabStop = false;
            // 
            // pictureBoxDigitalContactSensorIO_IN_HH
            // 
            this.pictureBoxDigitalContactSensorIO_IN_HH.Image = global::SLD200.Properties.Resources.DioEllipseOff;
            this.pictureBoxDigitalContactSensorIO_IN_HH.Location = new System.Drawing.Point(11, 107);
            this.pictureBoxDigitalContactSensorIO_IN_HH.Name = "pictureBoxDigitalContactSensorIO_IN_HH";
            this.pictureBoxDigitalContactSensorIO_IN_HH.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxDigitalContactSensorIO_IN_HH.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxDigitalContactSensorIO_IN_HH.TabIndex = 27;
            this.pictureBoxDigitalContactSensorIO_IN_HH.TabStop = false;
            // 
            // pictureBoxDigitalContactSensorIO_IN_GO
            // 
            this.pictureBoxDigitalContactSensorIO_IN_GO.Image = global::SLD200.Properties.Resources.DioEllipseOff;
            this.pictureBoxDigitalContactSensorIO_IN_GO.Location = new System.Drawing.Point(11, 78);
            this.pictureBoxDigitalContactSensorIO_IN_GO.Name = "pictureBoxDigitalContactSensorIO_IN_GO";
            this.pictureBoxDigitalContactSensorIO_IN_GO.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxDigitalContactSensorIO_IN_GO.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxDigitalContactSensorIO_IN_GO.TabIndex = 26;
            this.pictureBoxDigitalContactSensorIO_IN_GO.TabStop = false;
            // 
            // pictureBoxDigitalContactSensorIO_IN_LO
            // 
            this.pictureBoxDigitalContactSensorIO_IN_LO.Image = global::SLD200.Properties.Resources.DioEllipseOff;
            this.pictureBoxDigitalContactSensorIO_IN_LO.Location = new System.Drawing.Point(11, 49);
            this.pictureBoxDigitalContactSensorIO_IN_LO.Name = "pictureBoxDigitalContactSensorIO_IN_LO";
            this.pictureBoxDigitalContactSensorIO_IN_LO.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxDigitalContactSensorIO_IN_LO.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxDigitalContactSensorIO_IN_LO.TabIndex = 25;
            this.pictureBoxDigitalContactSensorIO_IN_LO.TabStop = false;
            // 
            // pictureBoxDigitalContactSensorIO_IN_HIGH
            // 
            this.pictureBoxDigitalContactSensorIO_IN_HIGH.Image = global::SLD200.Properties.Resources.DioEllipseOff;
            this.pictureBoxDigitalContactSensorIO_IN_HIGH.Location = new System.Drawing.Point(11, 20);
            this.pictureBoxDigitalContactSensorIO_IN_HIGH.Name = "pictureBoxDigitalContactSensorIO_IN_HIGH";
            this.pictureBoxDigitalContactSensorIO_IN_HIGH.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxDigitalContactSensorIO_IN_HIGH.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxDigitalContactSensorIO_IN_HIGH.TabIndex = 24;
            this.pictureBoxDigitalContactSensorIO_IN_HIGH.TabStop = false;
            // 
            // labelDigitalContactSensorIO_IN_LO
            // 
            this.labelDigitalContactSensorIO_IN_LO.AutoSize = true;
            this.labelDigitalContactSensorIO_IN_LO.Location = new System.Drawing.Point(42, 55);
            this.labelDigitalContactSensorIO_IN_LO.Name = "labelDigitalContactSensorIO_IN_LO";
            this.labelDigitalContactSensorIO_IN_LO.Size = new System.Drawing.Size(21, 12);
            this.labelDigitalContactSensorIO_IN_LO.TabIndex = 12;
            this.labelDigitalContactSensorIO_IN_LO.Text = "LO";
            // 
            // labelDigitalContactSensorIO_IN_HIGH
            // 
            this.labelDigitalContactSensorIO_IN_HIGH.AutoSize = true;
            this.labelDigitalContactSensorIO_IN_HIGH.Location = new System.Drawing.Point(42, 26);
            this.labelDigitalContactSensorIO_IN_HIGH.Name = "labelDigitalContactSensorIO_IN_HIGH";
            this.labelDigitalContactSensorIO_IN_HIGH.Size = new System.Drawing.Size(33, 12);
            this.labelDigitalContactSensorIO_IN_HIGH.TabIndex = 12;
            this.labelDigitalContactSensorIO_IN_HIGH.Text = "HIGH";
            // 
            // labelDigitalContactSensorIO_IN_LL
            // 
            this.labelDigitalContactSensorIO_IN_LL.AutoSize = true;
            this.labelDigitalContactSensorIO_IN_LL.Location = new System.Drawing.Point(42, 142);
            this.labelDigitalContactSensorIO_IN_LL.Name = "labelDigitalContactSensorIO_IN_LL";
            this.labelDigitalContactSensorIO_IN_LL.Size = new System.Drawing.Size(19, 12);
            this.labelDigitalContactSensorIO_IN_LL.TabIndex = 10;
            this.labelDigitalContactSensorIO_IN_LL.Text = "LL";
            // 
            // labelDigitalContactSensorIO_IN_HH
            // 
            this.labelDigitalContactSensorIO_IN_HH.AutoSize = true;
            this.labelDigitalContactSensorIO_IN_HH.Location = new System.Drawing.Point(42, 113);
            this.labelDigitalContactSensorIO_IN_HH.Name = "labelDigitalContactSensorIO_IN_HH";
            this.labelDigitalContactSensorIO_IN_HH.Size = new System.Drawing.Size(21, 12);
            this.labelDigitalContactSensorIO_IN_HH.TabIndex = 10;
            this.labelDigitalContactSensorIO_IN_HH.Text = "HH";
            // 
            // labelDigitalContactSensorIO_IN_GO
            // 
            this.labelDigitalContactSensorIO_IN_GO.AutoSize = true;
            this.labelDigitalContactSensorIO_IN_GO.Location = new System.Drawing.Point(42, 84);
            this.labelDigitalContactSensorIO_IN_GO.Name = "labelDigitalContactSensorIO_IN_GO";
            this.labelDigitalContactSensorIO_IN_GO.Size = new System.Drawing.Size(23, 12);
            this.labelDigitalContactSensorIO_IN_GO.TabIndex = 10;
            this.labelDigitalContactSensorIO_IN_GO.Text = "GO";
            // 
            // btnDigitalContactSensorReset
            // 
            this.btnDigitalContactSensorReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnDigitalContactSensorReset.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDigitalContactSensorReset.ForeColor = System.Drawing.Color.White;
            this.btnDigitalContactSensorReset.Location = new System.Drawing.Point(361, 90);
            this.btnDigitalContactSensorReset.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnDigitalContactSensorReset.Name = "btnDigitalContactSensorReset";
            this.btnDigitalContactSensorReset.Size = new System.Drawing.Size(138, 38);
            this.btnDigitalContactSensorReset.TabIndex = 9;
            this.btnDigitalContactSensorReset.Text = "Reset";
            this.btnDigitalContactSensorReset.UseVisualStyleBackColor = false;
            // 
            // btnDigitalContactSensorPreset
            // 
            this.btnDigitalContactSensorPreset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnDigitalContactSensorPreset.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDigitalContactSensorPreset.ForeColor = System.Drawing.Color.White;
            this.btnDigitalContactSensorPreset.Location = new System.Drawing.Point(361, 36);
            this.btnDigitalContactSensorPreset.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnDigitalContactSensorPreset.Name = "btnDigitalContactSensorPreset";
            this.btnDigitalContactSensorPreset.Size = new System.Drawing.Size(138, 38);
            this.btnDigitalContactSensorPreset.TabIndex = 8;
            this.btnDigitalContactSensorPreset.Text = "Preset";
            this.btnDigitalContactSensorPreset.UseVisualStyleBackColor = false;
            // 
            // DigitalContactSensor
            // 
            this.Controls.Add(this.groupBoxDigitalContactSensor);
            this.Name = "DigitalContactSensor";
            this.Size = new System.Drawing.Size(520, 303);
            this.groupBoxDigitalContactSensor.ResumeLayout(false);
            this.groupBoxDigitalContactSensor.PerformLayout();
            this.groupBoxDigitalContactSensorOutput.ResumeLayout(false);
            this.groupBoxDigitalContactSensorOutput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDigitalContactSensorIO_OUT_RESET)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDigitalContactSensorIO_OUT_BANKB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDigitalContactSensorIO_OUT_TIMING)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDigitalContactSensorIO_OUT_BANKA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDigitalContactSensorIO_OUT_PRESET)).EndInit();
            this.groupBoxDigitalContactSensorInput.ResumeLayout(false);
            this.groupBoxDigitalContactSensorInput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDigitalContactSensorIO_IN_LL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDigitalContactSensorIO_IN_HH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDigitalContactSensorIO_IN_GO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDigitalContactSensorIO_IN_LO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDigitalContactSensorIO_IN_HIGH)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBoxDigitalContactSensor;
        private System.Windows.Forms.Button btnDigitalContactSensorPreset;
        private System.Windows.Forms.GroupBox groupBoxDigitalContactSensorOutput;
        private System.Windows.Forms.Label labelDigitalContactSensorIO_OUT_PRESET;
        private System.Windows.Forms.Button btnDigitalContactSensorReset;
        private System.Windows.Forms.GroupBox groupBoxDigitalContactSensorInput;
        private System.Windows.Forms.Label labelDigitalContactSensorIO_IN_LO;
        private System.Windows.Forms.Label labelDigitalContactSensorIO_IN_HIGH;
        private System.Windows.Forms.Label labelDigitalContactSensorIO_IN_GO;
        private System.Windows.Forms.Label labelDigitalContactSensorIO_IN_HH;
        private System.Windows.Forms.Label labelDigitalContactSensorIO_IN_LL;
        private System.Windows.Forms.Label labelDigitalContactSensorIO_OUT_BANKB;
        private System.Windows.Forms.Label labelDigitalContactSensorIO_OUT_TIMING;
        private System.Windows.Forms.Label labelDigitalContactSensorIO_OUT_BANKA;
        private System.Windows.Forms.Label labelDigitalContactSensorIO_OUT_RESET;
        private BaseLabel LabelWeighValueUnit;
        private System.Windows.Forms.TextBox textBoxDigitalContactSensorValue;
        private System.Windows.Forms.Button btnAirCylinderOff;
        private System.Windows.Forms.Button btnAirCylinderOn;
        private System.Windows.Forms.PictureBox pictureBoxDigitalContactSensorIO_IN_LL;
        private System.Windows.Forms.PictureBox pictureBoxDigitalContactSensorIO_IN_HH;
        private System.Windows.Forms.PictureBox pictureBoxDigitalContactSensorIO_IN_GO;
        private System.Windows.Forms.PictureBox pictureBoxDigitalContactSensorIO_IN_LO;
        private System.Windows.Forms.PictureBox pictureBoxDigitalContactSensorIO_IN_HIGH;
        private System.Windows.Forms.PictureBox pictureBoxDigitalContactSensorIO_OUT_RESET;
        private System.Windows.Forms.PictureBox pictureBoxDigitalContactSensorIO_OUT_BANKB;
        private System.Windows.Forms.PictureBox pictureBoxDigitalContactSensorIO_OUT_TIMING;
        private System.Windows.Forms.PictureBox pictureBoxDigitalContactSensorIO_OUT_BANKA;
        private System.Windows.Forms.PictureBox pictureBoxDigitalContactSensorIO_OUT_PRESET;
    }
}
