namespace CWA150SA_Onsemi300
{
    partial class FormLeakCheck
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

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLeakCheck));
            this.btn_Packing_Complete = new System.Windows.Forms.Button();
            this.pictureBox_LeakTestValve_Open_On = new System.Windows.Forms.PictureBox();
            this.pictureBox_LeakTestValve_Close_On = new System.Windows.Forms.PictureBox();
            this.pictureBox_LeakTestValve_Close_Off = new System.Windows.Forms.PictureBox();
            this.pictureBox_LeakTestValve_Open_Off = new System.Windows.Forms.PictureBox();
            this.pictureBox_Air_Gauge = new System.Windows.Forms.PictureBox();
            this.button_PackingVacSig_Off = new System.Windows.Forms.Button();
            this.btn_Align_Start = new System.Windows.Forms.Button();
            this.btn_Align_Cancel = new System.Windows.Forms.Button();
            this.tb_LeakCheck_Time_Remained = new System.Windows.Forms.TextBox();
            this.btn_Packing_Cancel2 = new System.Windows.Forms.Button();
            this.baseLabel_Close = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel_Open = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel_Arrow3 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel_Arrow2 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel_Arrow1 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel_Comment_Close3 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel_Comment_Close2 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel_LeakCheck_Valve = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel_Comment_Open = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel_Comment_Close1 = new CWA150SA_Onsemi300.BaseLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_LeakTestValve_Open_On)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_LeakTestValve_Close_On)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_LeakTestValve_Close_Off)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_LeakTestValve_Open_Off)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Air_Gauge)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_Packing_Complete
            // 
            this.btn_Packing_Complete.BackColor = System.Drawing.Color.LightGray;
            this.btn_Packing_Complete.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btn_Packing_Complete.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn_Packing_Complete.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btn_Packing_Complete.Font = new System.Drawing.Font("나눔바른고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Packing_Complete.ForeColor = System.Drawing.Color.DarkRed;
            this.btn_Packing_Complete.Location = new System.Drawing.Point(468, 526);
            this.btn_Packing_Complete.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_Packing_Complete.Name = "btn_Packing_Complete";
            this.btn_Packing_Complete.Size = new System.Drawing.Size(243, 98);
            this.btn_Packing_Complete.TabIndex = 126;
            this.btn_Packing_Complete.Text = "[ 패킹 작업 마무리 진행 ]\r\n\r\n창 닫기";
            this.btn_Packing_Complete.UseVisualStyleBackColor = false;
            this.btn_Packing_Complete.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // pictureBox_LeakTestValve_Open_On
            // 
            this.pictureBox_LeakTestValve_Open_On.BackColor = System.Drawing.Color.White;
            this.pictureBox_LeakTestValve_Open_On.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox_LeakTestValve_Open_On.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_LeakTestValve_Open_On.Image")));
            this.pictureBox_LeakTestValve_Open_On.Location = new System.Drawing.Point(26, 80);
            this.pictureBox_LeakTestValve_Open_On.Name = "pictureBox_LeakTestValve_Open_On";
            this.pictureBox_LeakTestValve_Open_On.Size = new System.Drawing.Size(390, 390);
            this.pictureBox_LeakTestValve_Open_On.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_LeakTestValve_Open_On.TabIndex = 127;
            this.pictureBox_LeakTestValve_Open_On.TabStop = false;
            // 
            // pictureBox_LeakTestValve_Close_On
            // 
            this.pictureBox_LeakTestValve_Close_On.BackColor = System.Drawing.Color.White;
            this.pictureBox_LeakTestValve_Close_On.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox_LeakTestValve_Close_On.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_LeakTestValve_Close_On.Image")));
            this.pictureBox_LeakTestValve_Close_On.Location = new System.Drawing.Point(26, 80);
            this.pictureBox_LeakTestValve_Close_On.Name = "pictureBox_LeakTestValve_Close_On";
            this.pictureBox_LeakTestValve_Close_On.Size = new System.Drawing.Size(390, 390);
            this.pictureBox_LeakTestValve_Close_On.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_LeakTestValve_Close_On.TabIndex = 128;
            this.pictureBox_LeakTestValve_Close_On.TabStop = false;
            // 
            // pictureBox_LeakTestValve_Close_Off
            // 
            this.pictureBox_LeakTestValve_Close_Off.BackColor = System.Drawing.Color.White;
            this.pictureBox_LeakTestValve_Close_Off.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox_LeakTestValve_Close_Off.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_LeakTestValve_Close_Off.Image")));
            this.pictureBox_LeakTestValve_Close_Off.Location = new System.Drawing.Point(26, 80);
            this.pictureBox_LeakTestValve_Close_Off.Name = "pictureBox_LeakTestValve_Close_Off";
            this.pictureBox_LeakTestValve_Close_Off.Size = new System.Drawing.Size(390, 390);
            this.pictureBox_LeakTestValve_Close_Off.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_LeakTestValve_Close_Off.TabIndex = 137;
            this.pictureBox_LeakTestValve_Close_Off.TabStop = false;
            // 
            // pictureBox_LeakTestValve_Open_Off
            // 
            this.pictureBox_LeakTestValve_Open_Off.BackColor = System.Drawing.Color.White;
            this.pictureBox_LeakTestValve_Open_Off.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox_LeakTestValve_Open_Off.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_LeakTestValve_Open_Off.Image")));
            this.pictureBox_LeakTestValve_Open_Off.Location = new System.Drawing.Point(26, 80);
            this.pictureBox_LeakTestValve_Open_Off.Name = "pictureBox_LeakTestValve_Open_Off";
            this.pictureBox_LeakTestValve_Open_Off.Size = new System.Drawing.Size(390, 390);
            this.pictureBox_LeakTestValve_Open_Off.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_LeakTestValve_Open_Off.TabIndex = 136;
            this.pictureBox_LeakTestValve_Open_Off.TabStop = false;
            // 
            // pictureBox_Air_Gauge
            // 
            this.pictureBox_Air_Gauge.BackColor = System.Drawing.Color.White;
            this.pictureBox_Air_Gauge.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox_Air_Gauge.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_Air_Gauge.Image")));
            this.pictureBox_Air_Gauge.Location = new System.Drawing.Point(784, 393);
            this.pictureBox_Air_Gauge.Name = "pictureBox_Air_Gauge";
            this.pictureBox_Air_Gauge.Size = new System.Drawing.Size(97, 97);
            this.pictureBox_Air_Gauge.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_Air_Gauge.TabIndex = 190;
            this.pictureBox_Air_Gauge.TabStop = false;
            // 
            // button_PackingVacSig_Off
            // 
            this.button_PackingVacSig_Off.BackColor = System.Drawing.Color.LightGray;
            this.button_PackingVacSig_Off.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.button_PackingVacSig_Off.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_PackingVacSig_Off.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.button_PackingVacSig_Off.Font = new System.Drawing.Font("나눔바른고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_PackingVacSig_Off.ForeColor = System.Drawing.Color.DarkRed;
            this.button_PackingVacSig_Off.Location = new System.Drawing.Point(737, 272);
            this.button_PackingVacSig_Off.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button_PackingVacSig_Off.Name = "button_PackingVacSig_Off";
            this.button_PackingVacSig_Off.Size = new System.Drawing.Size(144, 65);
            this.button_PackingVacSig_Off.TabIndex = 191;
            this.button_PackingVacSig_Off.Text = "패킹공압 Off";
            this.button_PackingVacSig_Off.UseVisualStyleBackColor = false;
            this.button_PackingVacSig_Off.Click += new System.EventHandler(this.button_PackingVacSig_Off_Click);
            // 
            // btn_Align_Start
            // 
            this.btn_Align_Start.BackColor = System.Drawing.Color.LightGray;
            this.btn_Align_Start.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btn_Align_Start.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn_Align_Start.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btn_Align_Start.Font = new System.Drawing.Font("나눔바른고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Align_Start.ForeColor = System.Drawing.Color.DarkRed;
            this.btn_Align_Start.Location = new System.Drawing.Point(254, 526);
            this.btn_Align_Start.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_Align_Start.Name = "btn_Align_Start";
            this.btn_Align_Start.Size = new System.Drawing.Size(185, 98);
            this.btn_Align_Start.TabIndex = 195;
            this.btn_Align_Start.Text = "얼라인 작업 진행";
            this.btn_Align_Start.UseVisualStyleBackColor = false;
            this.btn_Align_Start.Click += new System.EventHandler(this.btn_Align_Start_Click);
            // 
            // btn_Align_Cancel
            // 
            this.btn_Align_Cancel.BackColor = System.Drawing.Color.LightGray;
            this.btn_Align_Cancel.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btn_Align_Cancel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn_Align_Cancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btn_Align_Cancel.Font = new System.Drawing.Font("나눔바른고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Align_Cancel.ForeColor = System.Drawing.Color.DarkRed;
            this.btn_Align_Cancel.Location = new System.Drawing.Point(488, 526);
            this.btn_Align_Cancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_Align_Cancel.Name = "btn_Align_Cancel";
            this.btn_Align_Cancel.Size = new System.Drawing.Size(185, 98);
            this.btn_Align_Cancel.TabIndex = 196;
            this.btn_Align_Cancel.Text = "얼라인 작업 취소";
            this.btn_Align_Cancel.UseVisualStyleBackColor = false;
            this.btn_Align_Cancel.Click += new System.EventHandler(this.btn_Align_Cancel_Click);
            // 
            // tb_LeakCheck_Time_Remained
            // 
            this.tb_LeakCheck_Time_Remained.BackColor = System.Drawing.Color.Yellow;
            this.tb_LeakCheck_Time_Remained.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_LeakCheck_Time_Remained.Location = new System.Drawing.Point(784, 494);
            this.tb_LeakCheck_Time_Remained.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_LeakCheck_Time_Remained.Name = "tb_LeakCheck_Time_Remained";
            this.tb_LeakCheck_Time_Remained.Size = new System.Drawing.Size(97, 29);
            this.tb_LeakCheck_Time_Remained.TabIndex = 199;
            this.tb_LeakCheck_Time_Remained.Text = "---";
            this.tb_LeakCheck_Time_Remained.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btn_Packing_Cancel2
            // 
            this.btn_Packing_Cancel2.BackColor = System.Drawing.Color.LightGray;
            this.btn_Packing_Cancel2.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btn_Packing_Cancel2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn_Packing_Cancel2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btn_Packing_Cancel2.Font = new System.Drawing.Font("나눔바른고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Packing_Cancel2.ForeColor = System.Drawing.Color.DarkRed;
            this.btn_Packing_Cancel2.Location = new System.Drawing.Point(729, 567);
            this.btn_Packing_Cancel2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_Packing_Cancel2.Name = "btn_Packing_Cancel2";
            this.btn_Packing_Cancel2.Size = new System.Drawing.Size(152, 57);
            this.btn_Packing_Cancel2.TabIndex = 200;
            this.btn_Packing_Cancel2.Text = "패킹 작업 취소";
            this.btn_Packing_Cancel2.UseVisualStyleBackColor = false;
            this.btn_Packing_Cancel2.Click += new System.EventHandler(this.btn_Packing_Cancel2_Click);
            // 
            // baseLabel_Close
            // 
            this.baseLabel_Close.BackColor = System.Drawing.Color.Black;
            this.baseLabel_Close.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabel_Close.Font = new System.Drawing.Font("나눔바른고딕", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_Close.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel_Close.Location = new System.Drawing.Point(577, 147);
            this.baseLabel_Close.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Close.Name = "baseLabel_Close";
            this.baseLabel_Close.Size = new System.Drawing.Size(194, 62);
            this.baseLabel_Close.TabIndex = 198;
            this.baseLabel_Close.Text = "[  CLOSE  ]";
            this.baseLabel_Close.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel_Open
            // 
            this.baseLabel_Open.BackColor = System.Drawing.Color.Black;
            this.baseLabel_Open.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabel_Open.Font = new System.Drawing.Font("나눔바른고딕", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_Open.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel_Open.Location = new System.Drawing.Point(577, 147);
            this.baseLabel_Open.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Open.Name = "baseLabel_Open";
            this.baseLabel_Open.Size = new System.Drawing.Size(194, 62);
            this.baseLabel_Open.TabIndex = 197;
            this.baseLabel_Open.Text = "[   OPEN   ]";
            this.baseLabel_Open.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel_Arrow3
            // 
            this.baseLabel_Arrow3.AutoSize = true;
            this.baseLabel_Arrow3.Font = new System.Drawing.Font("나눔바른고딕", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_Arrow3.ForeColor = System.Drawing.Color.White;
            this.baseLabel_Arrow3.Location = new System.Drawing.Point(649, 471);
            this.baseLabel_Arrow3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Arrow3.Name = "baseLabel_Arrow3";
            this.baseLabel_Arrow3.Size = new System.Drawing.Size(51, 42);
            this.baseLabel_Arrow3.TabIndex = 194;
            this.baseLabel_Arrow3.Text = "⇓";
            // 
            // baseLabel_Arrow2
            // 
            this.baseLabel_Arrow2.AutoSize = true;
            this.baseLabel_Arrow2.Font = new System.Drawing.Font("나눔바른고딕", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_Arrow2.ForeColor = System.Drawing.Color.White;
            this.baseLabel_Arrow2.Location = new System.Drawing.Point(649, 344);
            this.baseLabel_Arrow2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Arrow2.Name = "baseLabel_Arrow2";
            this.baseLabel_Arrow2.Size = new System.Drawing.Size(51, 42);
            this.baseLabel_Arrow2.TabIndex = 193;
            this.baseLabel_Arrow2.Text = "⇓";
            // 
            // baseLabel_Arrow1
            // 
            this.baseLabel_Arrow1.AutoSize = true;
            this.baseLabel_Arrow1.Font = new System.Drawing.Font("나눔바른고딕", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_Arrow1.ForeColor = System.Drawing.Color.White;
            this.baseLabel_Arrow1.Location = new System.Drawing.Point(649, 220);
            this.baseLabel_Arrow1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Arrow1.Name = "baseLabel_Arrow1";
            this.baseLabel_Arrow1.Size = new System.Drawing.Size(51, 42);
            this.baseLabel_Arrow1.TabIndex = 192;
            this.baseLabel_Arrow1.Text = "⇓";
            // 
            // baseLabel_Comment_Close3
            // 
            this.baseLabel_Comment_Close3.BackColor = System.Drawing.Color.Cornsilk;
            this.baseLabel_Comment_Close3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_Comment_Close3.Font = new System.Drawing.Font("나눔바른고딕", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_Comment_Close3.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Comment_Close3.Location = new System.Drawing.Point(468, 393);
            this.baseLabel_Comment_Close3.Name = "baseLabel_Comment_Close3";
            this.baseLabel_Comment_Close3.Size = new System.Drawing.Size(310, 65);
            this.baseLabel_Comment_Close3.TabIndex = 189;
            this.baseLabel_Comment_Close3.Text = "3.  공압 게이지 변화 확인";
            this.baseLabel_Comment_Close3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel_Comment_Close2
            // 
            this.baseLabel_Comment_Close2.BackColor = System.Drawing.Color.Cornsilk;
            this.baseLabel_Comment_Close2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_Comment_Close2.Font = new System.Drawing.Font("나눔바른고딕", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_Comment_Close2.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Comment_Close2.Location = new System.Drawing.Point(468, 272);
            this.baseLabel_Comment_Close2.Name = "baseLabel_Comment_Close2";
            this.baseLabel_Comment_Close2.Size = new System.Drawing.Size(264, 65);
            this.baseLabel_Comment_Close2.TabIndex = 188;
            this.baseLabel_Comment_Close2.Text = "2.  패킹공압신호 Off";
            this.baseLabel_Comment_Close2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel_LeakCheck_Valve
            // 
            this.baseLabel_LeakCheck_Valve.BackColor = System.Drawing.Color.Yellow;
            this.baseLabel_LeakCheck_Valve.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabel_LeakCheck_Valve.Font = new System.Drawing.Font("나눔바른고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_LeakCheck_Valve.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_LeakCheck_Valve.Location = new System.Drawing.Point(76, 22);
            this.baseLabel_LeakCheck_Valve.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_LeakCheck_Valve.Name = "baseLabel_LeakCheck_Valve";
            this.baseLabel_LeakCheck_Valve.Size = new System.Drawing.Size(253, 41);
            this.baseLabel_LeakCheck_Valve.TabIndex = 134;
            this.baseLabel_LeakCheck_Valve.Text = "Leak Check  밸브 방향";
            this.baseLabel_LeakCheck_Valve.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel_Comment_Open
            // 
            this.baseLabel_Comment_Open.BackColor = System.Drawing.Color.Cornsilk;
            this.baseLabel_Comment_Open.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_Comment_Open.Font = new System.Drawing.Font("나눔바른고딕", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_Comment_Open.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Comment_Open.Location = new System.Drawing.Point(468, 22);
            this.baseLabel_Comment_Open.Name = "baseLabel_Comment_Open";
            this.baseLabel_Comment_Open.Size = new System.Drawing.Size(413, 116);
            this.baseLabel_Comment_Open.TabIndex = 186;
            this.baseLabel_Comment_Open.Text = "PAK 과 웨이퍼의 얼라인 작업을 위해\r\nLeak Check  밸브의 방향을\r\n다음과 같이 세팅한다.";
            this.baseLabel_Comment_Open.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel_Comment_Close1
            // 
            this.baseLabel_Comment_Close1.BackColor = System.Drawing.Color.Cornsilk;
            this.baseLabel_Comment_Close1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.baseLabel_Comment_Close1.Font = new System.Drawing.Font("나눔바른고딕", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_Comment_Close1.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Comment_Close1.Location = new System.Drawing.Point(468, 22);
            this.baseLabel_Comment_Close1.Name = "baseLabel_Comment_Close1";
            this.baseLabel_Comment_Close1.Size = new System.Drawing.Size(413, 116);
            this.baseLabel_Comment_Close1.TabIndex = 187;
            this.baseLabel_Comment_Close1.Text = "1.  PAK  과  씬-척의 패킹 완료 후  \r\n     Leak  확인을 위해 밸브의 방향을\r\n다음과 같이 세팅한다.              " +
    "";
            this.baseLabel_Comment_Close1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormLeakCheck
            // 
            this.BackColor = System.Drawing.Color.DarkSlateGray;
            this.ClientSize = new System.Drawing.Size(904, 645);
            this.ControlBox = false;
            this.Controls.Add(this.btn_Packing_Cancel2);
            this.Controls.Add(this.tb_LeakCheck_Time_Remained);
            this.Controls.Add(this.btn_Packing_Complete);
            this.Controls.Add(this.baseLabel_Close);
            this.Controls.Add(this.baseLabel_Open);
            this.Controls.Add(this.btn_Align_Cancel);
            this.Controls.Add(this.btn_Align_Start);
            this.Controls.Add(this.baseLabel_Arrow3);
            this.Controls.Add(this.baseLabel_Arrow2);
            this.Controls.Add(this.baseLabel_Arrow1);
            this.Controls.Add(this.button_PackingVacSig_Off);
            this.Controls.Add(this.pictureBox_Air_Gauge);
            this.Controls.Add(this.baseLabel_Comment_Close3);
            this.Controls.Add(this.baseLabel_Comment_Close2);
            this.Controls.Add(this.pictureBox_LeakTestValve_Close_Off);
            this.Controls.Add(this.pictureBox_LeakTestValve_Open_Off);
            this.Controls.Add(this.baseLabel_LeakCheck_Valve);
            this.Controls.Add(this.pictureBox_LeakTestValve_Close_On);
            this.Controls.Add(this.pictureBox_LeakTestValve_Open_On);
            this.Controls.Add(this.baseLabel_Comment_Open);
            this.Controls.Add(this.baseLabel_Comment_Close1);
            this.ForeColor = System.Drawing.Color.Coral;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormLeakCheck";
            this.ShowIcon = false;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_LeakTestValve_Open_On)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_LeakTestValve_Close_On)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_LeakTestValve_Close_Off)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_LeakTestValve_Open_Off)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Air_Gauge)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btn_Packing_Complete;
        private System.Windows.Forms.PictureBox pictureBox_LeakTestValve_Open_On;
        private System.Windows.Forms.PictureBox pictureBox_LeakTestValve_Close_On;
        private BaseLabel baseLabel_LeakCheck_Valve;
        private System.Windows.Forms.PictureBox pictureBox_LeakTestValve_Close_Off;
        private System.Windows.Forms.PictureBox pictureBox_LeakTestValve_Open_Off;
        private BaseLabel baseLabel_Comment_Open;
        private BaseLabel baseLabel_Comment_Close1;
        private BaseLabel baseLabel_Comment_Close2;
        private BaseLabel baseLabel_Comment_Close3;
        private System.Windows.Forms.PictureBox pictureBox_Air_Gauge;
        private System.Windows.Forms.Button button_PackingVacSig_Off;
        private BaseLabel baseLabel_Arrow1;
        private BaseLabel baseLabel_Arrow2;
        private BaseLabel baseLabel_Arrow3;
        private System.Windows.Forms.Button btn_Align_Start;
        private System.Windows.Forms.Button btn_Align_Cancel;
        private BaseLabel baseLabel_Open;
        private BaseLabel baseLabel_Close;
        private System.Windows.Forms.TextBox tb_LeakCheck_Time_Remained;
        private System.Windows.Forms.Button btn_Packing_Cancel2;
    }
}

