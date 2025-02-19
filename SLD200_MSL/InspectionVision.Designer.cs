namespace SLD200_MSL
{
    partial class InspectionVision
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
            this.groupBoxInspectionVision = new System.Windows.Forms.GroupBox();
            this.labelReceivedData = new System.Windows.Forms.Label();
            this.labelVision2 = new System.Windows.Forms.Label();
            this.labelVision1 = new System.Windows.Forms.Label();
            this.textBoxReceivedData = new System.Windows.Forms.TextBox();
            this.groupBoxInspVisionOutput = new System.Windows.Forms.GroupBox();
            this.labelInspVisionIO_OUT_Vision2_InspDataRequest = new System.Windows.Forms.Label();
            this.labelInspVisionIO_OUT_Vision2_Trigger = new System.Windows.Forms.Label();
            this.labelInspVisionIO_OUT_Vision2_Reset = new System.Windows.Forms.Label();
            this.labelInspVisionIO_OUT_Vision1_InspDataRequest = new System.Windows.Forms.Label();
            this.labelInspVisionIO_OUT_Vision1_Trigger = new System.Windows.Forms.Label();
            this.labelInspVisionIO_OUT_Vision1_Reset = new System.Windows.Forms.Label();
            this.ledInspVisionIO_OUT_Vision2_InspDataRequest = new System.Windows.Forms.Button();
            this.ledInspVisionIO_OUT_Vision2_Trigger = new System.Windows.Forms.Button();
            this.ledInspVisionIO_OUT_Vision2_Reset = new System.Windows.Forms.Button();
            this.ledInspVisionIO_OUT_Vision1_InspDataRequest = new System.Windows.Forms.Button();
            this.ledInspVisionIO_OUT_Vision1_Trigger = new System.Windows.Forms.Button();
            this.ledInspVisionIO_OUT_Vision1_Reset = new System.Windows.Forms.Button();
            this.groupBoxInspVisionInput = new System.Windows.Forms.GroupBox();
            this.labelInspVisionIO_IN_Vision1_Busy = new System.Windows.Forms.Label();
            this.labelInspVisionIO_IN_VisionReady = new System.Windows.Forms.Label();
            this.ledInspVisionIO_IN_Vision1_Busy = new System.Windows.Forms.Button();
            this.ledInspVisionIO_IN_VisionReady = new System.Windows.Forms.Button();
            this.labelInspVisionIO_IN_Vision2_DataSend = new System.Windows.Forms.Label();
            this.labelInspVisionIO_IN_Vision2_NG = new System.Windows.Forms.Label();
            this.labelInspVisionIO_IN_Vision2_OK = new System.Windows.Forms.Label();
            this.labelInspVisionIO_IN_Vision2_Busy = new System.Windows.Forms.Label();
            this.labelInspVisionIO_IN_Vision1_InspDataSend = new System.Windows.Forms.Label();
            this.labelInspVisionIO_IN_Vision1_NG = new System.Windows.Forms.Label();
            this.labelInspVisionIO_IN_Vision1_OK = new System.Windows.Forms.Label();
            this.ledInspVisionIO_IN_Vision2_DataSend = new System.Windows.Forms.Button();
            this.ledInspVisionIO_IN_Vision2_NG = new System.Windows.Forms.Button();
            this.ledInspVisionIO_IN_Vision2_OK = new System.Windows.Forms.Button();
            this.ledInspVisionIO_IN_Vision2_Busy = new System.Windows.Forms.Button();
            this.ledInspVisionIO_IN_Vision1_InspDataSend = new System.Windows.Forms.Button();
            this.ledInspVisionIO_IN_Vision1_NG = new System.Windows.Forms.Button();
            this.ledInspVisionIO_IN_Vision1_OK = new System.Windows.Forms.Button();
            this.groupBoxInspectionVision.SuspendLayout();
            this.groupBoxInspVisionOutput.SuspendLayout();
            this.groupBoxInspVisionInput.SuspendLayout();
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
            // groupBoxInspectionVision
            // 
            this.groupBoxInspectionVision.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.groupBoxInspectionVision.Controls.Add(this.labelReceivedData);
            this.groupBoxInspectionVision.Controls.Add(this.labelVision2);
            this.groupBoxInspectionVision.Controls.Add(this.labelVision1);
            this.groupBoxInspectionVision.Controls.Add(this.textBoxReceivedData);
            this.groupBoxInspectionVision.Controls.Add(this.groupBoxInspVisionOutput);
            this.groupBoxInspectionVision.Controls.Add(this.groupBoxInspVisionInput);
            this.groupBoxInspectionVision.ForeColor = System.Drawing.Color.White;
            this.groupBoxInspectionVision.Location = new System.Drawing.Point(3, 3);
            this.groupBoxInspectionVision.Name = "groupBoxInspectionVision";
            this.groupBoxInspectionVision.Size = new System.Drawing.Size(487, 498);
            this.groupBoxInspectionVision.TabIndex = 10;
            this.groupBoxInspectionVision.TabStop = false;
            this.groupBoxInspectionVision.Text = " Inspection Vision ";
            // 
            // labelReceivedData
            // 
            this.labelReceivedData.AutoSize = true;
            this.labelReceivedData.Location = new System.Drawing.Point(240, 345);
            this.labelReceivedData.Name = "labelReceivedData";
            this.labelReceivedData.Size = new System.Drawing.Size(86, 12);
            this.labelReceivedData.TabIndex = 18;
            this.labelReceivedData.Text = "Received Data";
            // 
            // labelVision2
            // 
            this.labelVision2.BackColor = System.Drawing.Color.Gainsboro;
            this.labelVision2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelVision2.ForeColor = System.Drawing.Color.Black;
            this.labelVision2.Location = new System.Drawing.Point(13, 428);
            this.labelVision2.Name = "labelVision2";
            this.labelVision2.Size = new System.Drawing.Size(214, 47);
            this.labelVision2.TabIndex = 17;
            this.labelVision2.Text = " [Vision #2]  LED Mounting \r\n                  Inspection";
            this.labelVision2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelVision1
            // 
            this.labelVision1.BackColor = System.Drawing.Color.Gainsboro;
            this.labelVision1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelVision1.ForeColor = System.Drawing.Color.Black;
            this.labelVision1.Location = new System.Drawing.Point(13, 363);
            this.labelVision1.Name = "labelVision1";
            this.labelVision1.Size = new System.Drawing.Size(214, 56);
            this.labelVision1.TabIndex = 17;
            this.labelVision1.Text = " [Vision #1]  Rivetting / \r\n                  Etching / \r\n                  Glue " + "Inspection";
            this.labelVision1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxReceivedData
            // 
            this.textBoxReceivedData.Location = new System.Drawing.Point(242, 363);
            this.textBoxReceivedData.Multiline = true;
            this.textBoxReceivedData.Name = "textBoxReceivedData";
            this.textBoxReceivedData.Size = new System.Drawing.Size(232, 122);
            this.textBoxReceivedData.TabIndex = 16;
            // 
            // groupBoxInspVisionOutput
            // 
            this.groupBoxInspVisionOutput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.groupBoxInspVisionOutput.Controls.Add(this.labelInspVisionIO_OUT_Vision2_InspDataRequest);
            this.groupBoxInspVisionOutput.Controls.Add(this.labelInspVisionIO_OUT_Vision2_Trigger);
            this.groupBoxInspVisionOutput.Controls.Add(this.labelInspVisionIO_OUT_Vision2_Reset);
            this.groupBoxInspVisionOutput.Controls.Add(this.labelInspVisionIO_OUT_Vision1_InspDataRequest);
            this.groupBoxInspVisionOutput.Controls.Add(this.labelInspVisionIO_OUT_Vision1_Trigger);
            this.groupBoxInspVisionOutput.Controls.Add(this.labelInspVisionIO_OUT_Vision1_Reset);
            this.groupBoxInspVisionOutput.Controls.Add(this.ledInspVisionIO_OUT_Vision2_InspDataRequest);
            this.groupBoxInspVisionOutput.Controls.Add(this.ledInspVisionIO_OUT_Vision2_Trigger);
            this.groupBoxInspVisionOutput.Controls.Add(this.ledInspVisionIO_OUT_Vision2_Reset);
            this.groupBoxInspVisionOutput.Controls.Add(this.ledInspVisionIO_OUT_Vision1_InspDataRequest);
            this.groupBoxInspVisionOutput.Controls.Add(this.ledInspVisionIO_OUT_Vision1_Trigger);
            this.groupBoxInspVisionOutput.Controls.Add(this.ledInspVisionIO_OUT_Vision1_Reset);
            this.groupBoxInspVisionOutput.ForeColor = System.Drawing.Color.White;
            this.groupBoxInspVisionOutput.Location = new System.Drawing.Point(242, 28);
            this.groupBoxInspVisionOutput.Name = "groupBoxInspVisionOutput";
            this.groupBoxInspVisionOutput.Size = new System.Drawing.Size(232, 298);
            this.groupBoxInspVisionOutput.TabIndex = 15;
            this.groupBoxInspVisionOutput.TabStop = false;
            this.groupBoxInspVisionOutput.Text = " Output ";
            // 
            // labelInspVisionIO_OUT_Vision2_InspDataRequest
            // 
            this.labelInspVisionIO_OUT_Vision2_InspDataRequest.AutoSize = true;
            this.labelInspVisionIO_OUT_Vision2_InspDataRequest.Location = new System.Drawing.Point(42, 240);
            this.labelInspVisionIO_OUT_Vision2_InspDataRequest.Name = "labelInspVisionIO_OUT_Vision2_InspDataRequest";
            this.labelInspVisionIO_OUT_Vision2_InspDataRequest.Size = new System.Drawing.Size(177, 12);
            this.labelInspVisionIO_OUT_Vision2_InspDataRequest.TabIndex = 10;
            this.labelInspVisionIO_OUT_Vision2_InspDataRequest.Text = "Vision #2 - Insp. Data Request";
            // 
            // labelInspVisionIO_OUT_Vision2_Trigger
            // 
            this.labelInspVisionIO_OUT_Vision2_Trigger.AutoSize = true;
            this.labelInspVisionIO_OUT_Vision2_Trigger.Location = new System.Drawing.Point(42, 211);
            this.labelInspVisionIO_OUT_Vision2_Trigger.Name = "labelInspVisionIO_OUT_Vision2_Trigger";
            this.labelInspVisionIO_OUT_Vision2_Trigger.Size = new System.Drawing.Size(110, 12);
            this.labelInspVisionIO_OUT_Vision2_Trigger.TabIndex = 10;
            this.labelInspVisionIO_OUT_Vision2_Trigger.Text = "Vision #2 - Trigger";
            // 
            // labelInspVisionIO_OUT_Vision2_Reset
            // 
            this.labelInspVisionIO_OUT_Vision2_Reset.AutoSize = true;
            this.labelInspVisionIO_OUT_Vision2_Reset.Location = new System.Drawing.Point(42, 182);
            this.labelInspVisionIO_OUT_Vision2_Reset.Name = "labelInspVisionIO_OUT_Vision2_Reset";
            this.labelInspVisionIO_OUT_Vision2_Reset.Size = new System.Drawing.Size(102, 12);
            this.labelInspVisionIO_OUT_Vision2_Reset.TabIndex = 10;
            this.labelInspVisionIO_OUT_Vision2_Reset.Text = "Vision #2 - Reset";
            // 
            // labelInspVisionIO_OUT_Vision1_InspDataRequest
            // 
            this.labelInspVisionIO_OUT_Vision1_InspDataRequest.AutoSize = true;
            this.labelInspVisionIO_OUT_Vision1_InspDataRequest.Location = new System.Drawing.Point(42, 113);
            this.labelInspVisionIO_OUT_Vision1_InspDataRequest.Name = "labelInspVisionIO_OUT_Vision1_InspDataRequest";
            this.labelInspVisionIO_OUT_Vision1_InspDataRequest.Size = new System.Drawing.Size(177, 12);
            this.labelInspVisionIO_OUT_Vision1_InspDataRequest.TabIndex = 10;
            this.labelInspVisionIO_OUT_Vision1_InspDataRequest.Text = "Vision #1 - Insp. Data Request";
            // 
            // labelInspVisionIO_OUT_Vision1_Trigger
            // 
            this.labelInspVisionIO_OUT_Vision1_Trigger.AutoSize = true;
            this.labelInspVisionIO_OUT_Vision1_Trigger.Location = new System.Drawing.Point(42, 84);
            this.labelInspVisionIO_OUT_Vision1_Trigger.Name = "labelInspVisionIO_OUT_Vision1_Trigger";
            this.labelInspVisionIO_OUT_Vision1_Trigger.Size = new System.Drawing.Size(110, 12);
            this.labelInspVisionIO_OUT_Vision1_Trigger.TabIndex = 10;
            this.labelInspVisionIO_OUT_Vision1_Trigger.Text = "Vision #1 - Trigger";
            // 
            // labelInspVisionIO_OUT_Vision1_Reset
            // 
            this.labelInspVisionIO_OUT_Vision1_Reset.AutoSize = true;
            this.labelInspVisionIO_OUT_Vision1_Reset.Location = new System.Drawing.Point(42, 55);
            this.labelInspVisionIO_OUT_Vision1_Reset.Name = "labelInspVisionIO_OUT_Vision1_Reset";
            this.labelInspVisionIO_OUT_Vision1_Reset.Size = new System.Drawing.Size(102, 12);
            this.labelInspVisionIO_OUT_Vision1_Reset.TabIndex = 10;
            this.labelInspVisionIO_OUT_Vision1_Reset.Text = "Vision #1 - Reset";
            // 
            // ledInspVisionIO_OUT_Vision2_InspDataRequest
            // 
            this.ledInspVisionIO_OUT_Vision2_InspDataRequest.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ledInspVisionIO_OUT_Vision2_InspDataRequest.Image = global::SLD200.Properties.Resources.DioEllipseOn;
            this.ledInspVisionIO_OUT_Vision2_InspDataRequest.Location = new System.Drawing.Point(11, 234);
            this.ledInspVisionIO_OUT_Vision2_InspDataRequest.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ledInspVisionIO_OUT_Vision2_InspDataRequest.Name = "ledInspVisionIO_OUT_Vision2_InspDataRequest";
            this.ledInspVisionIO_OUT_Vision2_InspDataRequest.Size = new System.Drawing.Size(25, 25);
            this.ledInspVisionIO_OUT_Vision2_InspDataRequest.TabIndex = 9;
            this.ledInspVisionIO_OUT_Vision2_InspDataRequest.UseVisualStyleBackColor = false;
            // 
            // ledInspVisionIO_OUT_Vision2_Trigger
            // 
            this.ledInspVisionIO_OUT_Vision2_Trigger.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ledInspVisionIO_OUT_Vision2_Trigger.Image = global::SLD200.Properties.Resources.DioEllipseOn;
            this.ledInspVisionIO_OUT_Vision2_Trigger.Location = new System.Drawing.Point(11, 205);
            this.ledInspVisionIO_OUT_Vision2_Trigger.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ledInspVisionIO_OUT_Vision2_Trigger.Name = "ledInspVisionIO_OUT_Vision2_Trigger";
            this.ledInspVisionIO_OUT_Vision2_Trigger.Size = new System.Drawing.Size(25, 25);
            this.ledInspVisionIO_OUT_Vision2_Trigger.TabIndex = 9;
            this.ledInspVisionIO_OUT_Vision2_Trigger.UseVisualStyleBackColor = false;
            // 
            // ledInspVisionIO_OUT_Vision2_Reset
            // 
            this.ledInspVisionIO_OUT_Vision2_Reset.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ledInspVisionIO_OUT_Vision2_Reset.Image = global::SLD200.Properties.Resources.DioEllipseOn;
            this.ledInspVisionIO_OUT_Vision2_Reset.Location = new System.Drawing.Point(11, 176);
            this.ledInspVisionIO_OUT_Vision2_Reset.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ledInspVisionIO_OUT_Vision2_Reset.Name = "ledInspVisionIO_OUT_Vision2_Reset";
            this.ledInspVisionIO_OUT_Vision2_Reset.Size = new System.Drawing.Size(25, 25);
            this.ledInspVisionIO_OUT_Vision2_Reset.TabIndex = 9;
            this.ledInspVisionIO_OUT_Vision2_Reset.UseVisualStyleBackColor = false;
            // 
            // ledInspVisionIO_OUT_Vision1_InspDataRequest
            // 
            this.ledInspVisionIO_OUT_Vision1_InspDataRequest.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ledInspVisionIO_OUT_Vision1_InspDataRequest.Image = global::SLD200.Properties.Resources.DioEllipseOn;
            this.ledInspVisionIO_OUT_Vision1_InspDataRequest.Location = new System.Drawing.Point(11, 107);
            this.ledInspVisionIO_OUT_Vision1_InspDataRequest.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ledInspVisionIO_OUT_Vision1_InspDataRequest.Name = "ledInspVisionIO_OUT_Vision1_InspDataRequest";
            this.ledInspVisionIO_OUT_Vision1_InspDataRequest.Size = new System.Drawing.Size(25, 25);
            this.ledInspVisionIO_OUT_Vision1_InspDataRequest.TabIndex = 9;
            this.ledInspVisionIO_OUT_Vision1_InspDataRequest.UseVisualStyleBackColor = false;
            // 
            // ledInspVisionIO_OUT_Vision1_Trigger
            // 
            this.ledInspVisionIO_OUT_Vision1_Trigger.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ledInspVisionIO_OUT_Vision1_Trigger.Image = global::SLD200.Properties.Resources.DioEllipseOn;
            this.ledInspVisionIO_OUT_Vision1_Trigger.Location = new System.Drawing.Point(11, 78);
            this.ledInspVisionIO_OUT_Vision1_Trigger.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ledInspVisionIO_OUT_Vision1_Trigger.Name = "ledInspVisionIO_OUT_Vision1_Trigger";
            this.ledInspVisionIO_OUT_Vision1_Trigger.Size = new System.Drawing.Size(25, 25);
            this.ledInspVisionIO_OUT_Vision1_Trigger.TabIndex = 9;
            this.ledInspVisionIO_OUT_Vision1_Trigger.UseVisualStyleBackColor = false;
            // 
            // ledInspVisionIO_OUT_Vision1_Reset
            // 
            this.ledInspVisionIO_OUT_Vision1_Reset.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ledInspVisionIO_OUT_Vision1_Reset.Image = global::SLD200.Properties.Resources.DioEllipseOn;
            this.ledInspVisionIO_OUT_Vision1_Reset.Location = new System.Drawing.Point(11, 49);
            this.ledInspVisionIO_OUT_Vision1_Reset.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ledInspVisionIO_OUT_Vision1_Reset.Name = "ledInspVisionIO_OUT_Vision1_Reset";
            this.ledInspVisionIO_OUT_Vision1_Reset.Size = new System.Drawing.Size(25, 25);
            this.ledInspVisionIO_OUT_Vision1_Reset.TabIndex = 9;
            this.ledInspVisionIO_OUT_Vision1_Reset.UseVisualStyleBackColor = false;
            // 
            // groupBoxInspVisionInput
            // 
            this.groupBoxInspVisionInput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.groupBoxInspVisionInput.Controls.Add(this.labelInspVisionIO_IN_Vision1_Busy);
            this.groupBoxInspVisionInput.Controls.Add(this.labelInspVisionIO_IN_VisionReady);
            this.groupBoxInspVisionInput.Controls.Add(this.ledInspVisionIO_IN_Vision1_Busy);
            this.groupBoxInspVisionInput.Controls.Add(this.ledInspVisionIO_IN_VisionReady);
            this.groupBoxInspVisionInput.Controls.Add(this.labelInspVisionIO_IN_Vision2_DataSend);
            this.groupBoxInspVisionInput.Controls.Add(this.labelInspVisionIO_IN_Vision2_NG);
            this.groupBoxInspVisionInput.Controls.Add(this.labelInspVisionIO_IN_Vision2_OK);
            this.groupBoxInspVisionInput.Controls.Add(this.labelInspVisionIO_IN_Vision2_Busy);
            this.groupBoxInspVisionInput.Controls.Add(this.labelInspVisionIO_IN_Vision1_InspDataSend);
            this.groupBoxInspVisionInput.Controls.Add(this.labelInspVisionIO_IN_Vision1_NG);
            this.groupBoxInspVisionInput.Controls.Add(this.labelInspVisionIO_IN_Vision1_OK);
            this.groupBoxInspVisionInput.Controls.Add(this.ledInspVisionIO_IN_Vision2_DataSend);
            this.groupBoxInspVisionInput.Controls.Add(this.ledInspVisionIO_IN_Vision2_NG);
            this.groupBoxInspVisionInput.Controls.Add(this.ledInspVisionIO_IN_Vision2_OK);
            this.groupBoxInspVisionInput.Controls.Add(this.ledInspVisionIO_IN_Vision2_Busy);
            this.groupBoxInspVisionInput.Controls.Add(this.ledInspVisionIO_IN_Vision1_InspDataSend);
            this.groupBoxInspVisionInput.Controls.Add(this.ledInspVisionIO_IN_Vision1_NG);
            this.groupBoxInspVisionInput.Controls.Add(this.ledInspVisionIO_IN_Vision1_OK);
            this.groupBoxInspVisionInput.ForeColor = System.Drawing.Color.White;
            this.groupBoxInspVisionInput.Location = new System.Drawing.Point(15, 28);
            this.groupBoxInspVisionInput.Name = "groupBoxInspVisionInput";
            this.groupBoxInspVisionInput.Size = new System.Drawing.Size(212, 298);
            this.groupBoxInspVisionInput.TabIndex = 14;
            this.groupBoxInspVisionInput.TabStop = false;
            this.groupBoxInspVisionInput.Text = " Input ";
            // 
            // labelInspVisionIO_IN_Vision1_Busy
            // 
            this.labelInspVisionIO_IN_Vision1_Busy.AutoSize = true;
            this.labelInspVisionIO_IN_Vision1_Busy.Location = new System.Drawing.Point(42, 55);
            this.labelInspVisionIO_IN_Vision1_Busy.Name = "labelInspVisionIO_IN_Vision1_Busy";
            this.labelInspVisionIO_IN_Vision1_Busy.Size = new System.Drawing.Size(99, 12);
            this.labelInspVisionIO_IN_Vision1_Busy.TabIndex = 12;
            this.labelInspVisionIO_IN_Vision1_Busy.Text = "Vision #1 - Busy";
            // 
            // labelInspVisionIO_IN_VisionReady
            // 
            this.labelInspVisionIO_IN_VisionReady.AutoSize = true;
            this.labelInspVisionIO_IN_VisionReady.Location = new System.Drawing.Point(42, 26);
            this.labelInspVisionIO_IN_VisionReady.Name = "labelInspVisionIO_IN_VisionReady";
            this.labelInspVisionIO_IN_VisionReady.Size = new System.Drawing.Size(80, 12);
            this.labelInspVisionIO_IN_VisionReady.TabIndex = 12;
            this.labelInspVisionIO_IN_VisionReady.Text = "Vision Ready";
            // 
            // ledInspVisionIO_IN_Vision1_Busy
            // 
            this.ledInspVisionIO_IN_Vision1_Busy.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ledInspVisionIO_IN_Vision1_Busy.Image = global::SLD200.Properties.Resources.DioEllipseOn;
            this.ledInspVisionIO_IN_Vision1_Busy.Location = new System.Drawing.Point(11, 49);
            this.ledInspVisionIO_IN_Vision1_Busy.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ledInspVisionIO_IN_Vision1_Busy.Name = "ledInspVisionIO_IN_Vision1_Busy";
            this.ledInspVisionIO_IN_Vision1_Busy.Size = new System.Drawing.Size(25, 25);
            this.ledInspVisionIO_IN_Vision1_Busy.TabIndex = 11;
            this.ledInspVisionIO_IN_Vision1_Busy.UseVisualStyleBackColor = false;
            // 
            // ledInspVisionIO_IN_VisionReady
            // 
            this.ledInspVisionIO_IN_VisionReady.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ledInspVisionIO_IN_VisionReady.Image = global::SLD200.Properties.Resources.DioEllipseOn;
            this.ledInspVisionIO_IN_VisionReady.Location = new System.Drawing.Point(11, 20);
            this.ledInspVisionIO_IN_VisionReady.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ledInspVisionIO_IN_VisionReady.Name = "ledInspVisionIO_IN_VisionReady";
            this.ledInspVisionIO_IN_VisionReady.Size = new System.Drawing.Size(25, 25);
            this.ledInspVisionIO_IN_VisionReady.TabIndex = 11;
            this.ledInspVisionIO_IN_VisionReady.UseVisualStyleBackColor = false;
            // 
            // labelInspVisionIO_IN_Vision2_DataSend
            // 
            this.labelInspVisionIO_IN_Vision2_DataSend.AutoSize = true;
            this.labelInspVisionIO_IN_Vision2_DataSend.Location = new System.Drawing.Point(42, 269);
            this.labelInspVisionIO_IN_Vision2_DataSend.Name = "labelInspVisionIO_IN_Vision2_DataSend";
            this.labelInspVisionIO_IN_Vision2_DataSend.Size = new System.Drawing.Size(160, 12);
            this.labelInspVisionIO_IN_Vision2_DataSend.TabIndex = 10;
            this.labelInspVisionIO_IN_Vision2_DataSend.Text = "Vision #2 - Insp. Data Send";
            // 
            // labelInspVisionIO_IN_Vision2_NG
            // 
            this.labelInspVisionIO_IN_Vision2_NG.AutoSize = true;
            this.labelInspVisionIO_IN_Vision2_NG.Location = new System.Drawing.Point(42, 240);
            this.labelInspVisionIO_IN_Vision2_NG.Name = "labelInspVisionIO_IN_Vision2_NG";
            this.labelInspVisionIO_IN_Vision2_NG.Size = new System.Drawing.Size(88, 12);
            this.labelInspVisionIO_IN_Vision2_NG.TabIndex = 10;
            this.labelInspVisionIO_IN_Vision2_NG.Text = "Vision #2 - NG";
            // 
            // labelInspVisionIO_IN_Vision2_OK
            // 
            this.labelInspVisionIO_IN_Vision2_OK.AutoSize = true;
            this.labelInspVisionIO_IN_Vision2_OK.Location = new System.Drawing.Point(42, 211);
            this.labelInspVisionIO_IN_Vision2_OK.Name = "labelInspVisionIO_IN_Vision2_OK";
            this.labelInspVisionIO_IN_Vision2_OK.Size = new System.Drawing.Size(87, 12);
            this.labelInspVisionIO_IN_Vision2_OK.TabIndex = 10;
            this.labelInspVisionIO_IN_Vision2_OK.Text = "Vision #2 - OK";
            // 
            // labelInspVisionIO_IN_Vision2_Busy
            // 
            this.labelInspVisionIO_IN_Vision2_Busy.AutoSize = true;
            this.labelInspVisionIO_IN_Vision2_Busy.Location = new System.Drawing.Point(42, 182);
            this.labelInspVisionIO_IN_Vision2_Busy.Name = "labelInspVisionIO_IN_Vision2_Busy";
            this.labelInspVisionIO_IN_Vision2_Busy.Size = new System.Drawing.Size(99, 12);
            this.labelInspVisionIO_IN_Vision2_Busy.TabIndex = 10;
            this.labelInspVisionIO_IN_Vision2_Busy.Text = "Vision #2 - Busy";
            // 
            // labelInspVisionIO_IN_Vision1_InspDataSend
            // 
            this.labelInspVisionIO_IN_Vision1_InspDataSend.AutoSize = true;
            this.labelInspVisionIO_IN_Vision1_InspDataSend.Location = new System.Drawing.Point(42, 142);
            this.labelInspVisionIO_IN_Vision1_InspDataSend.Name = "labelInspVisionIO_IN_Vision1_InspDataSend";
            this.labelInspVisionIO_IN_Vision1_InspDataSend.Size = new System.Drawing.Size(160, 12);
            this.labelInspVisionIO_IN_Vision1_InspDataSend.TabIndex = 10;
            this.labelInspVisionIO_IN_Vision1_InspDataSend.Text = "Vision #1 - Insp. Data Send";
            // 
            // labelInspVisionIO_IN_Vision1_NG
            // 
            this.labelInspVisionIO_IN_Vision1_NG.AutoSize = true;
            this.labelInspVisionIO_IN_Vision1_NG.Location = new System.Drawing.Point(42, 113);
            this.labelInspVisionIO_IN_Vision1_NG.Name = "labelInspVisionIO_IN_Vision1_NG";
            this.labelInspVisionIO_IN_Vision1_NG.Size = new System.Drawing.Size(88, 12);
            this.labelInspVisionIO_IN_Vision1_NG.TabIndex = 10;
            this.labelInspVisionIO_IN_Vision1_NG.Text = "Vision #1 - NG";
            // 
            // labelInspVisionIO_IN_Vision1_OK
            // 
            this.labelInspVisionIO_IN_Vision1_OK.AutoSize = true;
            this.labelInspVisionIO_IN_Vision1_OK.Location = new System.Drawing.Point(42, 84);
            this.labelInspVisionIO_IN_Vision1_OK.Name = "labelInspVisionIO_IN_Vision1_OK";
            this.labelInspVisionIO_IN_Vision1_OK.Size = new System.Drawing.Size(87, 12);
            this.labelInspVisionIO_IN_Vision1_OK.TabIndex = 10;
            this.labelInspVisionIO_IN_Vision1_OK.Text = "Vision #1 - OK";
            // 
            // ledInspVisionIO_IN_Vision2_DataSend
            // 
            this.ledInspVisionIO_IN_Vision2_DataSend.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ledInspVisionIO_IN_Vision2_DataSend.Image = global::SLD200.Properties.Resources.DioEllipseOn;
            this.ledInspVisionIO_IN_Vision2_DataSend.Location = new System.Drawing.Point(11, 263);
            this.ledInspVisionIO_IN_Vision2_DataSend.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ledInspVisionIO_IN_Vision2_DataSend.Name = "ledInspVisionIO_IN_Vision2_DataSend";
            this.ledInspVisionIO_IN_Vision2_DataSend.Size = new System.Drawing.Size(25, 25);
            this.ledInspVisionIO_IN_Vision2_DataSend.TabIndex = 9;
            this.ledInspVisionIO_IN_Vision2_DataSend.UseVisualStyleBackColor = false;
            // 
            // ledInspVisionIO_IN_Vision2_NG
            // 
            this.ledInspVisionIO_IN_Vision2_NG.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ledInspVisionIO_IN_Vision2_NG.Image = global::SLD200.Properties.Resources.DioEllipseOn;
            this.ledInspVisionIO_IN_Vision2_NG.Location = new System.Drawing.Point(11, 234);
            this.ledInspVisionIO_IN_Vision2_NG.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ledInspVisionIO_IN_Vision2_NG.Name = "ledInspVisionIO_IN_Vision2_NG";
            this.ledInspVisionIO_IN_Vision2_NG.Size = new System.Drawing.Size(25, 25);
            this.ledInspVisionIO_IN_Vision2_NG.TabIndex = 9;
            this.ledInspVisionIO_IN_Vision2_NG.UseVisualStyleBackColor = false;
            // 
            // ledInspVisionIO_IN_Vision2_OK
            // 
            this.ledInspVisionIO_IN_Vision2_OK.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ledInspVisionIO_IN_Vision2_OK.Image = global::SLD200.Properties.Resources.DioEllipseOn;
            this.ledInspVisionIO_IN_Vision2_OK.Location = new System.Drawing.Point(11, 205);
            this.ledInspVisionIO_IN_Vision2_OK.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ledInspVisionIO_IN_Vision2_OK.Name = "ledInspVisionIO_IN_Vision2_OK";
            this.ledInspVisionIO_IN_Vision2_OK.Size = new System.Drawing.Size(25, 25);
            this.ledInspVisionIO_IN_Vision2_OK.TabIndex = 9;
            this.ledInspVisionIO_IN_Vision2_OK.UseVisualStyleBackColor = false;
            // 
            // ledInspVisionIO_IN_Vision2_Busy
            // 
            this.ledInspVisionIO_IN_Vision2_Busy.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ledInspVisionIO_IN_Vision2_Busy.Image = global::SLD200.Properties.Resources.DioEllipseOn;
            this.ledInspVisionIO_IN_Vision2_Busy.Location = new System.Drawing.Point(11, 176);
            this.ledInspVisionIO_IN_Vision2_Busy.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ledInspVisionIO_IN_Vision2_Busy.Name = "ledInspVisionIO_IN_Vision2_Busy";
            this.ledInspVisionIO_IN_Vision2_Busy.Size = new System.Drawing.Size(25, 25);
            this.ledInspVisionIO_IN_Vision2_Busy.TabIndex = 9;
            this.ledInspVisionIO_IN_Vision2_Busy.UseVisualStyleBackColor = false;
            // 
            // ledInspVisionIO_IN_Vision1_InspDataSend
            // 
            this.ledInspVisionIO_IN_Vision1_InspDataSend.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ledInspVisionIO_IN_Vision1_InspDataSend.Image = global::SLD200.Properties.Resources.DioEllipseOn;
            this.ledInspVisionIO_IN_Vision1_InspDataSend.Location = new System.Drawing.Point(11, 136);
            this.ledInspVisionIO_IN_Vision1_InspDataSend.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ledInspVisionIO_IN_Vision1_InspDataSend.Name = "ledInspVisionIO_IN_Vision1_InspDataSend";
            this.ledInspVisionIO_IN_Vision1_InspDataSend.Size = new System.Drawing.Size(25, 25);
            this.ledInspVisionIO_IN_Vision1_InspDataSend.TabIndex = 9;
            this.ledInspVisionIO_IN_Vision1_InspDataSend.UseVisualStyleBackColor = false;
            // 
            // ledInspVisionIO_IN_Vision1_NG
            // 
            this.ledInspVisionIO_IN_Vision1_NG.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ledInspVisionIO_IN_Vision1_NG.Image = global::SLD200.Properties.Resources.DioEllipseOn;
            this.ledInspVisionIO_IN_Vision1_NG.Location = new System.Drawing.Point(11, 107);
            this.ledInspVisionIO_IN_Vision1_NG.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ledInspVisionIO_IN_Vision1_NG.Name = "ledInspVisionIO_IN_Vision1_NG";
            this.ledInspVisionIO_IN_Vision1_NG.Size = new System.Drawing.Size(25, 25);
            this.ledInspVisionIO_IN_Vision1_NG.TabIndex = 9;
            this.ledInspVisionIO_IN_Vision1_NG.UseVisualStyleBackColor = false;
            // 
            // ledInspVisionIO_IN_Vision1_OK
            // 
            this.ledInspVisionIO_IN_Vision1_OK.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ledInspVisionIO_IN_Vision1_OK.Image = global::SLD200.Properties.Resources.DioEllipseOn;
            this.ledInspVisionIO_IN_Vision1_OK.Location = new System.Drawing.Point(11, 78);
            this.ledInspVisionIO_IN_Vision1_OK.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ledInspVisionIO_IN_Vision1_OK.Name = "ledInspVisionIO_IN_Vision1_OK";
            this.ledInspVisionIO_IN_Vision1_OK.Size = new System.Drawing.Size(25, 25);
            this.ledInspVisionIO_IN_Vision1_OK.TabIndex = 9;
            this.ledInspVisionIO_IN_Vision1_OK.UseVisualStyleBackColor = false;
            // 
            // InspectionVision
            // 
            this.Controls.Add(this.groupBoxInspectionVision);
            this.Name = "InspectionVision";
            this.Size = new System.Drawing.Size(493, 504);
            this.groupBoxInspectionVision.ResumeLayout(false);
            this.groupBoxInspectionVision.PerformLayout();
            this.groupBoxInspVisionOutput.ResumeLayout(false);
            this.groupBoxInspVisionOutput.PerformLayout();
            this.groupBoxInspVisionInput.ResumeLayout(false);
            this.groupBoxInspVisionInput.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBoxInspectionVision;
        private System.Windows.Forms.GroupBox groupBoxInspVisionOutput;
        private System.Windows.Forms.Label labelInspVisionIO_OUT_Vision2_Reset;
        private System.Windows.Forms.Label labelInspVisionIO_OUT_Vision1_InspDataRequest;
        private System.Windows.Forms.Label labelInspVisionIO_OUT_Vision1_Trigger;
        private System.Windows.Forms.Label labelInspVisionIO_OUT_Vision1_Reset;
        private System.Windows.Forms.Button ledInspVisionIO_OUT_Vision2_Reset;
        private System.Windows.Forms.Button ledInspVisionIO_OUT_Vision1_InspDataRequest;
        private System.Windows.Forms.Button ledInspVisionIO_OUT_Vision1_Trigger;
        private System.Windows.Forms.Button ledInspVisionIO_OUT_Vision1_Reset;
        private System.Windows.Forms.GroupBox groupBoxInspVisionInput;
        private System.Windows.Forms.Label labelInspVisionIO_IN_Vision1_Busy;
        private System.Windows.Forms.Label labelInspVisionIO_IN_VisionReady;
        private System.Windows.Forms.Button ledInspVisionIO_IN_Vision1_Busy;
        private System.Windows.Forms.Button ledInspVisionIO_IN_VisionReady;
        private System.Windows.Forms.Label labelInspVisionIO_IN_Vision2_NG;
        private System.Windows.Forms.Label labelInspVisionIO_IN_Vision2_OK;
        private System.Windows.Forms.Label labelInspVisionIO_IN_Vision2_Busy;
        private System.Windows.Forms.Label labelInspVisionIO_IN_Vision1_InspDataSend;
        private System.Windows.Forms.Label labelInspVisionIO_IN_Vision1_NG;
        private System.Windows.Forms.Label labelInspVisionIO_IN_Vision1_OK;
        private System.Windows.Forms.Button ledInspVisionIO_IN_Vision2_NG;
        private System.Windows.Forms.Button ledInspVisionIO_IN_Vision2_OK;
        private System.Windows.Forms.Button ledInspVisionIO_IN_Vision2_Busy;
        private System.Windows.Forms.Button ledInspVisionIO_IN_Vision1_InspDataSend;
        private System.Windows.Forms.Button ledInspVisionIO_IN_Vision1_NG;
        private System.Windows.Forms.Button ledInspVisionIO_IN_Vision1_OK;
        private System.Windows.Forms.Label labelInspVisionIO_IN_Vision2_DataSend;
        private System.Windows.Forms.Button ledInspVisionIO_IN_Vision2_DataSend;
        private System.Windows.Forms.Label labelInspVisionIO_OUT_Vision2_InspDataRequest;
        private System.Windows.Forms.Label labelInspVisionIO_OUT_Vision2_Trigger;
        private System.Windows.Forms.Button ledInspVisionIO_OUT_Vision2_InspDataRequest;
        private System.Windows.Forms.Button ledInspVisionIO_OUT_Vision2_Trigger;
        private System.Windows.Forms.TextBox textBoxReceivedData;
        private System.Windows.Forms.Label labelVision2;
        private System.Windows.Forms.Label labelVision1;
        private System.Windows.Forms.Label labelReceivedData;
    }
}
